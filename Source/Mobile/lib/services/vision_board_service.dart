import 'dart:async';
import 'dart:convert';
import 'dart:io';
import 'dart:math' as math;
import 'package:flutter/foundation.dart';
import 'package:chopper/chopper.dart';
import 'package:http/http.dart' as http;
import '../models/vision_board.dart';
import '../api/generated/code/manifestation.swagger.dart';
import 'unified_auth_service.dart';
import 'package:xml/xml.dart';
import 'package:flutter/material.dart';
import 'package:path_provider/path_provider.dart';

/// Background image processing for isolates
class ImageProcessingParams {
  final String imagePath;
  
  ImageProcessingParams(this.imagePath);
}

/// Background function to process image encoding (must be top-level for compute())
String processImageInBackground(ImageProcessingParams params) {
  try {
    final imageFile = File(params.imagePath);
    if (imageFile.existsSync()) {
      final imageBytes = imageFile.readAsBytesSync();
      return base64Encode(imageBytes);
    }
    return '';
  } catch (e) {
    debugPrint('❌ Background image processing error: $e');
    return '';
  }
}

/// LRU Cache implementation to prevent unbounded memory growth
class LRUCache<K, V> {
  final int maxSize;
  final Map<K, V> _cache = <K, V>{};
  final List<K> _accessOrder = <K>[];
  
  LRUCache(this.maxSize);
  
  V? get(K key) {
    if (_cache.containsKey(key)) {
      // Move to end (most recently used)
      _accessOrder.remove(key);
      _accessOrder.add(key);
      return _cache[key];
    }
    return null;
  }
  
  void put(K key, V value) {
    if (_cache.containsKey(key)) {
      // Update existing key
      _cache[key] = value;
      _accessOrder.remove(key);
      _accessOrder.add(key);
    } else {
      // Add new key
      if (_cache.length >= maxSize) {
        // Remove least recently used
        final oldest = _accessOrder.removeAt(0);
        _cache.remove(oldest);
      }
      _cache[key] = value;
      _accessOrder.add(key);
    }
  }
  
  void remove(K key) {
    _cache.remove(key);
    _accessOrder.remove(key);
  }
  
  void clear() {
    _cache.clear();
    _accessOrder.clear();
  }
  
  int get length => _cache.length;
  bool get isEmpty => _cache.isEmpty;
  bool get isNotEmpty => _cache.isNotEmpty;
  
  // Add missing getters for compatibility
  Iterable<V> get values => _cache.values;
  Iterable<K> get keys => _cache.keys;
}

/// Clean Vision Board Service - Server Only
/// 
/// This service:
/// 1. Generates XML from VisionBoard models
/// 2. Uploads/updates XML files on server using Swagger client
/// 3. Retrieves and parses XML from server
/// 4. No local storage - everything is server-based
class VisionBoardService {
  static final VisionBoardService _instance = VisionBoardService._internal();
  factory VisionBoardService() => _instance;
  VisionBoardService._internal();
  
  Manifestation? _apiClient;
  UnifiedAuthService? _authService;
  bool _isInitialized = false;

  // Cache duration - expire cached vision boards after 30 minutes
  static const Duration _cacheDuration = Duration(minutes: 30);

  // LRU cache for vision board XML content (max 50 items, ~5MB estimated)
  final LRUCache<String, String> _memoryCache = LRUCache<String, String>(50);
  final LRUCache<String, DateTime> _cacheTimestamps = LRUCache<String, DateTime>(50);

  // LRU cache for picture metadata (max 100 items)
  final LRUCache<int, List<dynamic>> _picturesCache = LRUCache<int, List<dynamic>>(100);
  final LRUCache<int, DateTime> _picturesCacheTimestamps = LRUCache<int, DateTime>(100);
  
  /// Clear all cached vision board content
  void clearCache() {
    _memoryCache.clear();
    _cacheTimestamps.clear();
    _picturesCache.clear();
    _picturesCacheTimestamps.clear();
    print('🗑️ Cleared all vision board memory cache');
  }
  
  /// Clear specific vision board from cache
  void clearCacheForBoard(String visionBoardId, int userId) {
    final cacheKey = '${userId}_$visionBoardId';
    _memoryCache.remove(cacheKey);
    _cacheTimestamps.remove(cacheKey);
    print('🗑️ Cleared cache for vision board: $visionBoardId');
  }
  
  /// Invalidate pictures cache for specific user (call when vision boards are modified)
  void invalidatePicturesCache(int userId) {
    _picturesCache.remove(userId);
    _picturesCacheTimestamps.remove(userId);
    print('🗑️ Invalidated pictures cache for user $userId');
  }
  
  /// Get cache statistics
  Map<String, dynamic> getCacheStats() {
    // Calculate expired entries
    final now = DateTime.now();
    int expiredCount = 0;
    for (final key in _cacheTimestamps.keys) {
      final timestamp = _cacheTimestamps.get(key);
      if (timestamp != null && now.difference(timestamp) >= _cacheDuration) {
        expiredCount++;
      }
    }

    return {
      'cachedBoards': _memoryCache.length,
      'expiredBoards': expiredCount,
      'totalCacheSize': _memoryCache.values.fold(0, (sum, xml) => sum + xml.length),
      'oldestCache': _cacheTimestamps.values.isEmpty ? null : _cacheTimestamps.values.reduce((a, b) => a.isBefore(b) ? a : b),
      'newestCache': _cacheTimestamps.values.isEmpty ? null : _cacheTimestamps.values.reduce((a, b) => a.isAfter(b) ? a : b),
      'cacheDuration': '${_cacheDuration.inMinutes} minutes',
    };
  }

  /// Clear expired cache entries
  void clearExpiredCache() {
    final now = DateTime.now();
    final keysToRemove = <String>[];

    // Find all expired entries
    for (final key in _cacheTimestamps.keys) {
      final timestamp = _cacheTimestamps.get(key);
      if (timestamp != null && now.difference(timestamp) >= _cacheDuration) {
        keysToRemove.add(key);
      }
    }

    // Remove expired entries
    for (final key in keysToRemove) {
      _memoryCache.remove(key);
      _cacheTimestamps.remove(key);
    }

    if (keysToRemove.isNotEmpty) {
      print('🗑️ Cleared ${keysToRemove.length} expired cache entries');
    }
  }
  
  /// Public method to get picture ID for a vision board (for external use)
  Future<int?> getPictureIdForBoard(String visionBoardId) async {
    final userId = await _authService!.getUserId();
    if (userId == null) return null;
    return await _getPictureIdByVisionBoardId(visionBoardId, userId);
  }
  
  Future<void> initialize({String? baseUrl}) async {
    // Only initialize once to prevent multiple initialization
    if (_isInitialized) {
      print('📋 VisionBoardService already initialized, skipping...');
      return;
    }
    
    if (baseUrl != null) {
      print('📋 Initializing VisionBoardService...');
      final chopperClient = ChopperClient(
        baseUrl: Uri.parse(baseUrl),
        interceptors: [_AuthInterceptor()],
        converter: $JsonSerializableConverter(),
        services: [Manifestation.create()],
      );
      
      _apiClient = chopperClient.getService<Manifestation>();
      _authService = UnifiedAuthService();
      await _authService!.initialize(baseUrl: baseUrl);
      print('✅ VisionBoardService initialized successfully');
    }
    
    _isInitialized = true;
  }
  
  // ========================= CREATE VISION BOARD =========================
  
  /// Create a new vision board on server
  Future<VisionBoardResult<int?>> createVisionBoard(VisionBoard visionBoard, {Map<String, dynamic>? thumbnailInfo}) async {
    try {
      print('📤 Creating vision board "${visionBoard.title}" on server...');

      // Debug title before processing
      print('🔍 DEBUG: Raw title: "${visionBoard.title}"');
      print('🔍 DEBUG: Title length: ${visionBoard.title.length}');
      print('🔍 DEBUG: Title bytes: ${utf8.encode(visionBoard.title).length}');

      if (_apiClient == null || _authService == null) {
        return VisionBoardResult.failure(message: 'Vision Board service is not ready. Please restart the app.');
      }

      final userId = await _authService!.getUserId();
      if (userId == null) {
        return VisionBoardResult.failure(message: 'Your session has expired. Please log in again.');
      }
      
      // Generate XML content
      final xmlContent = await _generateVisionBoardXML(visionBoard, userId);
      final xmlBytes = utf8.encode(xmlContent);
      
      print('Vision board details: ID=${visionBoard.id}, Title=${visionBoard.title}, Elements=${visionBoard.elements.length}');
      print('Generated XML content (${xmlBytes.length} bytes):');
      print(xmlContent.length > 500 ? '${xmlContent.substring(0, 500)}...' : xmlContent);
      
      // Check if XML contains base64 image data
      final imageDataCount = 'imageData='.allMatches(xmlContent).length;
      print('📊 XML contains $imageDataCount imageData attributes');
      if (imageDataCount > 0) {
        print('✅ XML successfully contains embedded base64 image data');
      } else {
        print('⚠️ XML does not contain any base64 image data - images may not sync');
      }
      
      // Use manual HTTP for multipart upload (Swagger client has multipart issues)
      print('API call parameters:');
      print('  - UploadedFile: ${xmlBytes.length} bytes');
      print('  - location: ${visionBoard.id}');
      print('  - fileName: visionboard_${visionBoard.id}.xml');
      print('  - fileSize: ${xmlBytes.length}');
      print('  - userId: $userId');
      
      final client = _apiClient!.client;
      final authService = UnifiedAuthService();
      final token = authService.authToken;
      
      final uri = Uri.parse('${client.baseUrl}/api/v1/manifest/createvisionboardpicture');
      final request = http.MultipartRequest('POST', uri);
      
      // Add headers
      if (token != null) {
        request.headers['Authorization'] = 'Bearer $token';
      }
      
      // Add form fields
      request.fields['Location'] = visionBoard.id;
      request.fields['FileName'] = 'visionboard_${visionBoard.id}.xml';
      request.fields['FileSize'] = xmlBytes.length.toString();
      request.fields['UserId'] = userId.toString();
      
      // Add Title field - API might be expecting this as a separate field
      final cleanTitle = visionBoard.title.trim();
      final finalTitle = cleanTitle.isEmpty ? 'Untitled Vision Board' : cleanTitle;
      final truncatedTitle = finalTitle.length > 100 ? finalTitle.substring(0, 100) : finalTitle;
      request.fields['Title'] = truncatedTitle;
      
      print('🔍 DEBUG: Form field Title: "$truncatedTitle" (${truncatedTitle.length} chars)');
      
      // Add ThumbnailFileName field - API requires this
      if (thumbnailInfo != null && thumbnailInfo['fileName'] != null) {
        request.fields['ThumbnailFileName'] = thumbnailInfo['fileName'];
        print('🔍 DEBUG: Form field ThumbnailFileName: "${thumbnailInfo['fileName']}"');
      } else {
        // Provide a default thumbnail filename if none provided
        request.fields['ThumbnailFileName'] = 'thumbnail_${visionBoard.id}.png';
        print('🔍 DEBUG: Form field ThumbnailFileName: "thumbnail_${visionBoard.id}.png" (default)');
      }
      
      // Add XML file
      request.files.add(http.MultipartFile.fromBytes(
        'UploadedFile',
        xmlBytes,
        filename: 'visionboard_${visionBoard.id}.xml',
      ));
      
      // Add thumbnail file if provided
      if (thumbnailInfo != null && thumbnailInfo['filePath'] != null) {
        try {
          final thumbnailFile = File(thumbnailInfo['filePath']);
          if (await thumbnailFile.exists()) {
            final thumbnailBytes = await thumbnailFile.readAsBytes();
            request.files.add(http.MultipartFile.fromBytes(
              'ThumbnailFile',
              thumbnailBytes,
              filename: thumbnailInfo['fileName'] ?? 'thumbnail.png',
            ));
            print('🔍 DEBUG: Added thumbnail file to request: ${thumbnailBytes.length} bytes');
          } else {
            print('⚠️ Thumbnail file not found at path: ${thumbnailInfo['filePath']}');
          }
        } catch (e) {
          print('⚠️ Error adding thumbnail file: $e');
        }
      }
      
      print('Manual multipart request:');
      print('  - Fields: ${request.fields}');
      print('  - Files: ${request.files.length}');
      
      final streamedResponse = await request.send();
      final responseString = await streamedResponse.stream.bytesToString();
      
      // Convert to response format
      final responseData = jsonDecode(responseString);
      final response = _ManualResponse(
        isSuccessful: streamedResponse.statusCode >= 200 && streamedResponse.statusCode < 300,
        statusCode: streamedResponse.statusCode,
        body: responseData,
      );
      
      if (response.isSuccessful && response.body['isSuccess'] == true) {
        final pictureId = response.body['model']?['visionBoardPictureId'];
        print('✅ Vision board created successfully with ID: $pictureId');
        
        // Upload thumbnail as a separate request if we have one
        if (pictureId != null && thumbnailInfo != null && thumbnailInfo['filePath'] != null) {
          print('📸 Uploading thumbnail separately for vision board ID: $pictureId');
          final thumbnailUploaded = await _uploadThumbnail(pictureId, thumbnailInfo);
          if (thumbnailUploaded) {
            print('✅ Thumbnail uploaded successfully');
          } else {
            print('⚠️ Thumbnail upload failed, but vision board was created');
          }
        }
        
        return VisionBoardResult.success(
          data: pictureId,
          message: 'Vision board created successfully'
        );
      } else {
        print('❌ API Response Details:');
        print('  - Status Code: ${response.statusCode}');
        print('  - Is Successful: ${response.isSuccessful}');
        print('  - Response Body: ${response.body}');
        print('  - Error Holder: ${response.body['errorHolder']}');
        print('  - Success Message: ${response.body['successMessage']}');
        
        final errorMessage = response.body['errorHolder']?.toString() ?? 
                            response.body['successMessage'] ?? 
                            'Failed to create vision board';
        return VisionBoardResult.failure(message: errorMessage);
      }
      
    } catch (e) {
      print('❌ Error creating vision board: $e');
      final errorStr = e.toString().toLowerCase();
      if (errorStr.contains('socketexception') || errorStr.contains('failed host lookup')) {
        return VisionBoardResult.failure(message: 'No internet connection. Please check your network and try again.');
      } else if (errorStr.contains('timeout')) {
        return VisionBoardResult.failure(message: 'Server is not responding. Please try again later.');
      } else if (errorStr.contains('401') || errorStr.contains('unauthorized')) {
        return VisionBoardResult.failure(message: 'Your session has expired. Please log in again.');
      } else {
        return VisionBoardResult.failure(message: 'Could not save vision board. Please try again.');
      }
    }
  }
  
  // ========================= UPDATE VISION BOARD =========================
  
  /// Update existing vision board on server
  Future<VisionBoardResult<bool>> updateVisionBoard(VisionBoard visionBoard, {int? pictureId, Map<String, dynamic>? thumbnailInfo}) async {
    try {
      if (_apiClient == null || _authService == null) {
        return VisionBoardResult.failure(message: 'Vision Board service is not ready. Please restart the app.');
      }

      final userId = await _authService!.getUserId();
      if (userId == null) {
        return VisionBoardResult.failure(message: 'Your session has expired. Please log in again.');
      }
      
      // Use provided picture ID or lookup by vision board ID
      final finalPictureId = pictureId ?? await _getPictureIdByVisionBoardId(visionBoard.id, userId);
      if (finalPictureId == null) {
        return VisionBoardResult.failure(message: 'Vision board not found. It may have been deleted or moved.');
      }
      
      // Generate updated XML content
      final xmlContent = await _generateVisionBoardXML(visionBoard, userId);
      final xmlBytes = utf8.encode(xmlContent);
      
      print('📤 Updating vision board "${visionBoard.title}" (Picture ID: $finalPictureId) on server...');
      
      // Check if XML contains base64 image data
      final imageDataCount = 'imageData='.allMatches(xmlContent).length;
      print('📊 Update XML contains $imageDataCount imageData attributes');
      if (imageDataCount > 0) {
        print('✅ Update XML successfully contains embedded base64 image data');
      } else {
        print('⚠️ Update XML does not contain any base64 image data - images may not sync');
      }
      
      // Use manual HTTP for multipart upload (same as create method)
      print('API call parameters:');
      print('  - UploadedFile: ${xmlBytes.length} bytes');
      print('  - visionBoardPictureId: $finalPictureId');
      print('  - location: ${visionBoard.id}');
      print('  - fileName: visionboard_${visionBoard.id}.xml');
      print('  - fileSize: ${xmlBytes.length}');
      print('  - userId: $userId');
      
      final client = _apiClient!.client;
      final authService = UnifiedAuthService();
      final token = authService.authToken;
      
      final uri = Uri.parse('${client.baseUrl}/api/v1/manifest/updatevisionboardpicture');
      final request = http.MultipartRequest('POST', uri);
      
      // Add headers
      if (token != null) {
        request.headers['Authorization'] = 'Bearer $token';
      }
      
      // Add form fields
      request.fields['VisionBoardPictureId'] = finalPictureId.toString();
      request.fields['Location'] = visionBoard.id;
      request.fields['FileName'] = 'visionboard_${visionBoard.id}.xml';
      request.fields['FileSize'] = xmlBytes.length.toString();
      request.fields['UserId'] = userId.toString();
      
      // Add Title field - API might be expecting this as a separate field
      final cleanTitle = visionBoard.title.trim();
      final finalTitle = cleanTitle.isEmpty ? 'Untitled Vision Board' : cleanTitle;
      final truncatedTitle = finalTitle.length > 100 ? finalTitle.substring(0, 100) : finalTitle;
      request.fields['Title'] = truncatedTitle;
      
      print('🔍 DEBUG: Update Title: "$truncatedTitle" (${truncatedTitle.length} chars)');
      
      // Add thumbnail metadata and file if provided (required by server)
      if (thumbnailInfo != null) {
        request.fields['ThumbnailFileName'] = thumbnailInfo['fileName'] ?? 'thumbnail.png';
        request.fields['ThumbnailFileSize'] = thumbnailInfo['fileSize'].toString();
        
        // Also try to include the thumbnail file in the same request
        if (thumbnailInfo['filePath'] != null) {
          final thumbnailFile = File(thumbnailInfo['filePath']);
          if (await thumbnailFile.exists()) {
            final thumbnailBytes = await thumbnailFile.readAsBytes();
            
            // Add thumbnail file to the same request (as a second file)
            request.files.add(http.MultipartFile.fromBytes(
              'Thumbnail', // Try this field name
              thumbnailBytes,
              filename: thumbnailInfo['fileName'] ?? 'thumbnail.png',
            ));
            
            print('📸 Added thumbnail metadata + file to update: ${thumbnailInfo['fileName']} (${thumbnailBytes.length} bytes)');
          } else {
            print('⚠️ Thumbnail file not found at path: ${thumbnailInfo['filePath']}');
          }
        }
      }
      
      // Add file
      request.files.add(http.MultipartFile.fromBytes(
        'UploadedFile',
        xmlBytes,
        filename: 'visionboard_${visionBoard.id}.xml',
      ));
      
      print('Manual multipart request:');
      print('  - Fields: ${request.fields}');
      print('  - Files: ${request.files.length}');
      
      final streamedResponse = await request.send();
      final responseString = await streamedResponse.stream.bytesToString();
      
      // Convert to response format
      final responseData = jsonDecode(responseString);
      final response = _ManualResponse(
        isSuccessful: streamedResponse.statusCode >= 200 && streamedResponse.statusCode < 300,
        statusCode: streamedResponse.statusCode,
        body: responseData,
      );
      
      if (response.isSuccessful && response.body['isSuccess'] == true) {
        print('✅ Vision board updated successfully');
        
        // Clear cache for this vision board since it was updated
        clearCacheForBoard(visionBoard.id, userId);
        
        // Thumbnail should now be included in the main request above
        
        return VisionBoardResult.success(
          data: true,
          message: 'Vision board updated successfully'
        );
      } else {
        print('❌ API Response Details:');
        print('  - Status Code: ${response.statusCode}');
        print('  - Is Successful: ${response.isSuccessful}');
        print('  - Response Body: ${response.body}');
        print('  - Error Holder: ${response.body['errorHolder']}');
        print('  - Success Message: ${response.body['successMessage']}');
        
        final errorMessage = response.body['errorHolder']?.toString() ?? 
                            response.body['successMessage'] ?? 
                            'Failed to update vision board';
        return VisionBoardResult.failure(message: errorMessage);
      }
      
    } catch (e) {
      print('❌ Error updating vision board: $e');
      final errorStr = e.toString().toLowerCase();
      if (errorStr.contains('socketexception') || errorStr.contains('failed host lookup')) {
        return VisionBoardResult.failure(message: 'No internet connection. Changes saved locally, will sync when online.');
      } else if (errorStr.contains('timeout')) {
        return VisionBoardResult.failure(message: 'Server is not responding. Please try again later.');
      } else if (errorStr.contains('401') || errorStr.contains('unauthorized')) {
        return VisionBoardResult.failure(message: 'Your session has expired. Please log in again.');
      } else {
        return VisionBoardResult.failure(message: 'Could not update vision board. Please try again.');
      }
    }
  }
  
  // ========================= GET VISION BOARDS =========================
  
  /// Get all vision boards for current user (metadata only for listing)
  Future<VisionBoardResult<List<VisionBoard>>> getUserVisionBoards() async {
    try {
      if (_apiClient == null || _authService == null) {
        return VisionBoardResult.failure(message: 'Vision Board service is not ready. Please restart the app.');
      }

      final userId = await _authService!.getUserId();
      if (userId == null) {
        return VisionBoardResult.failure(message: 'Your session has expired. Please log in again.');
      }
      
      print('📥 Getting vision board list for user: $userId');
      
      // Get list of vision boards from server (metadata only)
      final response = await _apiClient!.apiV1ManifestGetvisionboardpicturesGet(
        userId: userId,
      );
      
      if (!response.isSuccessful || response.body?.isSuccess != true) {
        final statusCode = response.statusCode;
        if (statusCode == 401 || statusCode == 403) {
          return VisionBoardResult.failure(message: 'Your session has expired. Please log in again.');
        } else if (statusCode == 404) {
          return VisionBoardResult.failure(message: 'No vision boards found. Create your first one!');
        } else if (statusCode >= 500) {
          return VisionBoardResult.failure(message: 'Server error. Our team has been notified. Please try again later.');
        }
        return VisionBoardResult.failure(
          message: response.body?.errorHolder?.toString() ?? 'Could not load vision boards. Please try again.'
        );
      }
      
      final visionBoards = <VisionBoard>[];
      final pictures = response.body?.model ?? [];
      
      print('📋 Found ${pictures.length} vision board files');
      
      // Create VisionBoard objects directly from metadata only - no XML downloading
      for (final picture in pictures) {
        if (picture.fileName?.endsWith('.xml') == true && picture.isDeleted != true) {
          try {
            final visionBoard = _createVisionBoardFromMetadata(picture);
            if (visionBoard != null) {
              visionBoards.add(visionBoard);
              print('📝 Added vision board: "${visionBoard.title}" (ID: ${visionBoard.id})');
            }
          } catch (e) {
            print('❌ Failed to create vision board from picture ${picture.visionBoardPictureId}: $e');
            continue;
          }
        }
      }
      
      // Sort by creation date (newest first)
      visionBoards.sort((a, b) => b.createdAt.compareTo(a.createdAt));
      
      print('✅ Retrieved ${visionBoards.length} vision boards for home screen');
      return VisionBoardResult.success(
        data: visionBoards,
        message: 'Vision boards retrieved successfully'
      );
      
    } catch (e) {
      print('❌ Error getting vision boards: $e');
      final errorStr = e.toString().toLowerCase();
      if (errorStr.contains('socketexception') || errorStr.contains('failed host lookup')) {
        return VisionBoardResult.failure(message: 'No internet connection. Please check your network.');
      } else if (errorStr.contains('timeout')) {
        return VisionBoardResult.failure(message: 'Request timed out. Please try again.');
      } else if (errorStr.contains('401') || errorStr.contains('unauthorized')) {
        return VisionBoardResult.failure(message: 'Your session has expired. Please log in again.');
      } else {
        return VisionBoardResult.failure(message: 'Could not load vision boards. Please check your connection and try again.');
      }
    }
  }
  
  /// Get specific vision board with full content by ID
  Future<VisionBoardResult<VisionBoard?>> getVisionBoard(String visionBoardId) async {
    try {
      if (_apiClient == null || _authService == null) {
        return VisionBoardResult.failure(message: 'Vision Board service is not ready. Please restart the app.');
      }

      final userId = await _authService!.getUserId();
      if (userId == null) {
        return VisionBoardResult.failure(message: 'Your session has expired. Please log in again.');
      }
      
      print('📥 Getting full vision board content for ID: $visionBoardId');
      
      // Check memory cache first
      final cacheKey = '${userId}_$visionBoardId';
      final cachedXml = _memoryCache.get(cacheKey);
      final cacheTime = _cacheTimestamps.get(cacheKey);

      if (cachedXml != null && cacheTime != null) {
        // Check if cache is still valid (not expired)
        final cacheAge = DateTime.now().difference(cacheTime);
        if (cacheAge < _cacheDuration) {
          print('💾 Found valid cached XML content (${cachedXml.length} chars, age: ${cacheAge.inMinutes}m)');

          // Parse cached XML content
          final visionBoard = await _parseVisionBoardXML(cachedXml);
          if (visionBoard != null) {
            print('✅ Successfully parsed cached vision board with ${visionBoard.elements.length} elements');
            return VisionBoardResult.success(
              data: visionBoard,
              message: 'Vision board retrieved from cache'
            );
          }
        } else {
          print('🗑️ Cache expired (age: ${cacheAge.inMinutes}m), will fetch fresh data');
          // Clear expired cache entry
          _memoryCache.remove(cacheKey);
          _cacheTimestamps.remove(cacheKey);
        }
      }
      
      // Not in cache or cache invalid, download from server
      print('🌐 Downloading fresh content from server');
      
      // First, find the picture ID for this vision board ID
      final pictureId = await _getPictureIdForVisionBoard(visionBoardId, userId);
      if (pictureId == null) {
        return VisionBoardResult.failure(message: 'Vision board not found');
      }
      
      print('🔍 Found picture ID $pictureId for vision board $visionBoardId');
      
      // Download the actual XML content using the correct endpoint
      final response = await _apiClient!.apiV1ManifestDownloadvisionboardpicturefileGet(
        visionBoardPictureId: pictureId,
      );
      
      if (!response.isSuccessful) {
        print('❌ Failed to download vision board file: ${response.statusCode}');
        final statusCode = response.statusCode;
        if (statusCode == 404) {
          return VisionBoardResult.failure(message: 'Vision board not found. It may have been deleted.');
        } else if (statusCode == 401 || statusCode == 403) {
          return VisionBoardResult.failure(message: 'Your session has expired. Please log in again.');
        } else if (statusCode >= 500) {
          return VisionBoardResult.failure(message: 'Server error. Please try again later.');
        }
        return VisionBoardResult.failure(message: 'Could not load vision board. Please try again.');
      }

      // Get the XML content from the response
      final xmlContent = response.bodyString;
      if (xmlContent.trim().isEmpty) {
        print('❌ Empty response from download endpoint');
        return VisionBoardResult.failure(message: 'Vision board is empty or corrupted. Please recreate it.');
      }
      
      print('✅ Downloaded XML content (${xmlContent.length} chars)');
      print('🔍 XML preview: ${xmlContent.substring(0, math.min(200, xmlContent.length))}...');
      
      // Check if XML contains imageData attributes (base64 images)
      if (xmlContent.contains('imageData=')) {
        print('✅ XML contains base64 image data');
      } else {
        print('⚠️ XML does not contain base64 image data - using legacy image paths');
      }
      
      // Cache the XML content in memory
      _memoryCache.put(cacheKey, xmlContent);
      _cacheTimestamps.put(cacheKey, DateTime.now());
      print('💾 Cached XML content for vision board $visionBoardId');
      
      // Parse the XML content to create VisionBoard object
      final visionBoard = await _parseVisionBoardXML(xmlContent);
      if (visionBoard != null) {
        print('✅ Successfully parsed vision board with ${visionBoard.elements.length} elements');
        return VisionBoardResult.success(
          data: visionBoard,
          message: 'Vision board retrieved successfully'
        );
      } else {
        return VisionBoardResult.failure(message: 'Vision board data is corrupted. Please try recreating it.');
      }

    } catch (e) {
      print('❌ Error getting vision board: $e');
      final errorStr = e.toString().toLowerCase();
      if (errorStr.contains('socketexception') || errorStr.contains('failed host lookup')) {
        return VisionBoardResult.failure(message: 'No internet connection. Please check your network.');
      } else if (errorStr.contains('timeout')) {
        return VisionBoardResult.failure(message: 'Request timed out. Please try again.');
      } else if (errorStr.contains('formatexception') || errorStr.contains('xmlexception')) {
        return VisionBoardResult.failure(message: 'Vision board data is corrupted. Please try recreating it.');
      } else {
        return VisionBoardResult.failure(message: 'Could not load vision board. Please try again.');
      }
    }
  }
  
  /// Helper method to find picture ID for a given vision board ID
  Future<int?> _getPictureIdForVisionBoard(String visionBoardId, int userId) async {
    // Use the same improved logic as _getPictureIdByVisionBoardId
    return await _getPictureIdByVisionBoardId(visionBoardId, userId);
  }
  
  // ========================= DELETE VISION BOARD =========================
  
  /// Delete vision board from server
  Future<VisionBoardResult<bool>> deleteVisionBoard(String visionBoardId) async {
    try {
      if (_apiClient == null || _authService == null) {
        return VisionBoardResult.failure(message: 'Vision Board service is not ready. Please restart the app.');
      }

      final userId = await _authService!.getUserId();
      if (userId == null) {
        return VisionBoardResult.failure(message: 'Your session has expired. Please log in again.');
      }
      
      // First, get the picture ID for this vision board
      final pictureId = await _getPictureIdByVisionBoardId(visionBoardId, userId);
      if (pictureId == null) {
        return VisionBoardResult.failure(message: 'Vision board not found. It may have already been deleted.');
      }
      
      print('🗑️ Deleting vision board $visionBoardId (Picture ID: $pictureId)');
      
      final response = await _apiClient!.apiV1ManifestMarkvisionboardpictureasdeletedDelete(
        id: pictureId,
      );
      
      if (response.isSuccessful && response.body?.isSuccess == true) {
        print('✅ Vision board deleted successfully');
        // Clear cache after deletion
        clearCacheForBoard(visionBoardId, userId);
        invalidatePicturesCache(userId);

        return VisionBoardResult.success(
          data: true,
          message: 'Vision board deleted successfully'
        );
      } else {
        final statusCode = response.statusCode;
        if (statusCode == 404) {
          return VisionBoardResult.failure(message: 'Vision board already deleted or not found.');
        } else if (statusCode == 401 || statusCode == 403) {
          return VisionBoardResult.failure(message: 'Your session has expired. Please log in again.');
        } else if (statusCode >= 500) {
          return VisionBoardResult.failure(message: 'Server error. Please try again later.');
        }
        final errorMessage = response.body?.errorHolder?.toString() ??
                            'Could not delete vision board. Please try again.';
        return VisionBoardResult.failure(message: errorMessage);
      }

    } catch (e) {
      print('❌ Error deleting vision board: $e');
      final errorStr = e.toString().toLowerCase();
      if (errorStr.contains('socketexception') || errorStr.contains('failed host lookup')) {
        return VisionBoardResult.failure(message: 'No internet connection. Please check your network.');
      } else if (errorStr.contains('timeout')) {
        return VisionBoardResult.failure(message: 'Request timed out. Please try again.');
      } else {
        return VisionBoardResult.failure(message: 'Could not delete vision board. Please try again.');
      }
    }
  }
  
  // ========================= HELPER METHODS =========================
  
  /// Get picture ID for a vision board by its ID
  Future<int?> _getPictureIdByVisionBoardId(String visionBoardId, int userId) async {
    try {
      print('🔍 Looking for picture ID for vision board: $visionBoardId');

      // Check cache first (using same cache duration as vision boards)
      final cachedTimestamp = _picturesCacheTimestamps.get(userId);
      final cachedPictures = _picturesCache.get(userId);

      List<dynamic>? pictures;

      // Try to use cached data if valid
      if (cachedTimestamp != null && cachedPictures != null) {
        final cacheAge = DateTime.now().difference(cachedTimestamp);
        if (cacheAge < _cacheDuration) {
          // Cache is still valid, use it
          pictures = cachedPictures;
          print('📋 Using cached pictures list (${pictures.length} pictures, age: ${cacheAge.inMinutes}m)');
        } else {
          print('🗑️ Pictures cache expired (age: ${cacheAge.inMinutes}m), will fetch fresh data');
          // Clear expired cache
          _picturesCache.remove(userId);
          _picturesCacheTimestamps.remove(userId);
        }
      }

      // If we didn't get pictures from cache, fetch from API
      if (pictures == null) {
        final response = await _apiClient!.apiV1ManifestGetvisionboardpicturesGet(
          userId: userId,
        );

        if (response.isSuccessful && response.body?.isSuccess == true) {
          pictures = response.body?.model ?? [];

          // Cache the results
          _picturesCache.put(userId, pictures);
          _picturesCacheTimestamps.put(userId, DateTime.now());

          print('🔍 Found ${pictures.length} pictures, cached for future use');
        } else {
          print('❌ Failed to get pictures list: ${response.body?.errorHolder}');
          return null;
        }
      }

      // Find the picture with matching location (vision board ID)
      for (final picture in pictures) {
        print('🔍 Checking picture: ID=${picture.visionBoardPictureId}, location=${picture.location}, fileName=${picture.fileName}');

        // Check multiple possible matches
        if ((picture.location == visionBoardId ||
             picture.fileName?.contains(visionBoardId) == true) &&
            picture.fileName?.endsWith('.xml') == true &&
            picture.isDeleted != true) {
          print('✅ Found matching picture ID: ${picture.visionBoardPictureId}');
          return picture.visionBoardPictureId;
        }
      }

      print('⚠️ No matching picture found for vision board: $visionBoardId');
      return null;
    } catch (e) {
      print('❌ Error getting picture ID: $e');
      return null;
    }
  }
  
  
  /// Create VisionBoard with actual title by fetching XML content
  Future<VisionBoard?> _createVisionBoardWithTitle(dynamic metadata) async {
    try {
      final pictureId = metadata.visionBoardPictureId;
      if (pictureId == null) return null;
      
      // Build thumbnail URL if available
      String? thumbnailUrl;
      if (metadata.visionBoardPictureId != null) {
        // Use the downloadvisionboardpicturethumbnail endpoint
        final baseUrl = _apiClient!.client.baseUrl;
        // Add timestamp to bust cache and force reload of updated thumbnails
        final timestamp = DateTime.now().millisecondsSinceEpoch;
        thumbnailUrl = '$baseUrl/api/v1/manifest/downloadvisionboardpicturethumbnail?id=${metadata.visionBoardPictureId}&t=$timestamp';
        print('📸 Built thumbnail URL with cache buster: $thumbnailUrl');
      }
      
      // Download the XML content to extract the real title and elements
      final xmlContent = await _downloadVisionBoardXML(pictureId);
      if (xmlContent != null) {
        // Parse the XML to create a complete VisionBoard
        final board = await _parseVisionBoardXML(xmlContent);
        if (board != null && thumbnailUrl != null) {
          // Add thumbnail URL to the parsed board
          return board.copyWith(thumbnailUrl: thumbnailUrl);
        }
        return board;
      } else {
        print('⚠️ Could not download XML for picture $pictureId, using metadata fallback');
        return _createVisionBoardFromMetadata(metadata);
      }
    } catch (e) {
      print('❌ Error in _createVisionBoardWithTitle: $e');
      return _createVisionBoardFromMetadata(metadata);
    }
  }

  /// Download XML content for a vision board picture
  Future<String?> _downloadVisionBoardXML(int pictureId) async {
    try {
      print('⬇️ Downloading XML content for picture ID: $pictureId');
      
      // Download the actual XML content using the correct endpoint
      final response = await _apiClient!.apiV1ManifestDownloadvisionboardpicturefileGet(
        visionBoardPictureId: pictureId,
      );
      
      if (!response.isSuccessful) {
        print('❌ Failed to download vision board file: ${response.statusCode}');
        return null;
      }
      
      // Get the XML content from the response
      final xmlContent = response.bodyString;
      if (xmlContent.trim().isEmpty) {
        print('❌ Empty response from download endpoint');
        return null;
      }
      
      print('✅ Downloaded XML content: ${xmlContent.length} characters');
      return xmlContent;
    } catch (e) {
      print('❌ Error downloading XML for picture $pictureId: $e');
      return null;
    }
  }

  /// Create VisionBoard from metadata when XML is not available
  VisionBoard? _createVisionBoardFromMetadata(dynamic metadata) {
    try {
      // Extract what we can from the metadata
      final fileName = metadata.fileName as String?;
      if (fileName == null) return null;
      
      // Extract board ID from filename (e.g., "visionboard_board_1754292620094.xml")
      final match = RegExp(r'visionboard_(.+)\.xml').firstMatch(fileName);
      final boardId = match?.group(1) ?? DateTime.now().millisecondsSinceEpoch.toString();
      
      final createdAt = DateTime.tryParse(metadata.createdDate?.toString() ?? '') ?? DateTime.now();
      final modifiedAt = DateTime.tryParse(metadata.modifiedDate?.toString() ?? '') ?? DateTime.now();
      
      print('🔧 Creating VisionBoard from metadata: ID=$boardId, Created=$createdAt');
      print('📋 Available metadata fields: ${metadata.toString()}');
      
      // Try to extract title from metadata or use a better default
      String boardTitle = 'Vision Board'; // Default fallback
      
      // Check if the server metadata contains a title field
      if (metadata.title != null && metadata.title.toString().isNotEmpty) {
        boardTitle = metadata.title.toString();
        print('✅ Found title in metadata: $boardTitle');
      } else {
        // Generate a more meaningful default title based on date
        final dateStr = createdAt.day.toString().padLeft(2, '0') + '/' + 
                       createdAt.month.toString().padLeft(2, '0');
        boardTitle = 'Vision Board $dateStr';
        print('📝 Using date-based title: $boardTitle');
      }
      
      // Build thumbnail URL if available
      String? thumbnailUrl;
      if (metadata.visionBoardPictureId != null) {
        // Use the downloadvisionboardpicturethumbnail endpoint
        final baseUrl = _apiClient!.client.baseUrl;
        // Add timestamp to bust cache and force reload of updated thumbnails
        final timestamp = DateTime.now().millisecondsSinceEpoch;
        thumbnailUrl = '$baseUrl/api/v1/manifest/downloadvisionboardpicturethumbnail?id=${metadata.visionBoardPictureId}&t=$timestamp';
        print('📸 Built thumbnail URL with cache buster: $thumbnailUrl');
      }
      
      return VisionBoard(
        id: boardId,
        title: boardTitle,
        elements: [], // Empty elements since we can't parse XML
        backgroundColor: const Color(0xFFFFFFFF), // White color
        createdAt: createdAt,
        updatedAt: modifiedAt,
        thumbnailUrl: thumbnailUrl,
      );
    } catch (e) {
      print('❌ Error creating VisionBoard from metadata: $e');
      return null;
    }
  }
  
  /// Generate XML content for vision board
  Future<String> _generateVisionBoardXML(VisionBoard visionBoard, int userId) async {
    // First, process all images in parallel using background isolates
    final Map<String, String> imageDataMap = {};
    final List<Future<void>> imageFutures = [];
    
    for (final element in visionBoard.elements) {
      if (element is VisionBoardImageElement) {
        imageFutures.add(
          compute(processImageInBackground, ImageProcessingParams(element.imagePath))
            .then((base64Image) {
              if (base64Image.isNotEmpty) {
                imageDataMap[element.id] = base64Image;
                print('📷 Successfully processed image ${element.id}: ${base64Image.length} characters');
              }
            })
            .catchError((e) {
              print('❌ Error processing image ${element.id}: $e');
            })
        );
      }
    }
    
    // Wait for all images to be processed
    await Future.wait(imageFutures);
    
    // Now build XML synchronously with processed image data
    final builder = XmlBuilder();
    builder.processing('xml', 'version="1.0" encoding="UTF-8"');
    builder.element('VisionBoards', nest: () {
      builder.attribute('version', '1.0');
      builder.attribute('lastModified', DateTime.now().toUtc().toIso8601String());
      builder.attribute('userId', userId.toString());
      
      builder.element('VisionBoard', nest: () {
        builder.attribute('id', visionBoard.id);
        // Ensure title is clean and within limits
        final cleanTitle = visionBoard.title.trim();
        final truncatedTitle = cleanTitle.length > 100 ? cleanTitle.substring(0, 100) : cleanTitle;
        builder.attribute('title', truncatedTitle);
        
        if (cleanTitle.length > 100) {
          print('⚠️ Title truncated from ${cleanTitle.length} to 100 characters');
        }
        print('📝 Using title: "$truncatedTitle" (${truncatedTitle.length} chars)');
        builder.attribute('backgroundColor', '#${visionBoard.backgroundColor.toARGB32().toRadixString(16).padLeft(8, '0').substring(2)}');
        builder.attribute('createdAt', visionBoard.createdAt.toUtc().toIso8601String());
        builder.attribute('updatedAt', visionBoard.updatedAt.toUtc().toIso8601String());
        
        builder.element('Elements', nest: () {
          for (final element in visionBoard.elements) {
            if (element is VisionBoardImageElement) {
              builder.element('ImageElement', nest: () {
                builder.attribute('id', element.id);
                builder.attribute('x', element.position.dx.toString());
                builder.attribute('y', element.position.dy.toString());
                builder.attribute('width', element.size.width.toString());
                builder.attribute('height', element.size.height.toString());
                builder.attribute('rotation', element.rotation.toString());
                builder.attribute('zIndex', element.zIndex.toString());
                builder.attribute('fit', element.fit.name);
                
                // Use pre-processed image data if available
                final base64Image = imageDataMap[element.id];
                if (base64Image != null) {
                  builder.attribute('imageData', base64Image);
                  print('✅ Embedded processed image data for ${element.id}');
                } else {
                  print('❌ No processed image data for ${element.id}');
                }
                
                // Keep legacy paths for fallback (but prioritize imageData)
                builder.attribute('imagePath', element.imagePath);
                if (element.imageUrl != null) {
                  builder.attribute('imageUrl', element.imageUrl!);
                }
              });
            } else if (element is VisionBoardTextElement) {
              builder.element('TextElement', nest: () {
                builder.attribute('id', element.id);
                builder.attribute('x', element.position.dx.toString());
                builder.attribute('y', element.position.dy.toString());
                builder.attribute('width', element.size.width.toString());
                builder.attribute('height', element.size.height.toString());
                builder.attribute('rotation', element.rotation.toString());
                builder.attribute('zIndex', element.zIndex.toString());
                builder.attribute('text', element.text);
                builder.attribute('fontSize', (element.style.fontSize ?? 16.0).toString());
                builder.attribute('fontWeight', (element.style.fontWeight?.index ?? 3).toString());
                builder.attribute('color', '#${(element.style.color?.toARGB32() ?? 0xFF000000).toRadixString(16).padLeft(8, '0').substring(2)}');
                builder.attribute('textAlign', element.alignment.name);
              });
            }
          }
        });
      });
    });
    return builder.buildDocument().toXmlString(pretty: true);
  }
  
  /// Parse XML content to VisionBoard model
  Future<VisionBoard?> _parseVisionBoardXML(String xmlContent) async {
    try {
      final xmlDoc = XmlDocument.parse(xmlContent);
      final rootElement = xmlDoc.rootElement;
      
      // Find the VisionBoard element
      final boardElement = rootElement.children
          .whereType<XmlElement>()
          .firstWhere((e) => e.name.local == 'VisionBoard');
      
      final id = boardElement.getAttribute('id')!;
      final title = boardElement.getAttribute('title')!;
      final backgroundColorHex = boardElement.getAttribute('backgroundColor')!;
      final backgroundColor = Color(int.parse(backgroundColorHex.replaceAll('#', 'FF'), radix: 16));
      final createdAt = DateTime.parse(boardElement.getAttribute('createdAt')!);
      final updatedAt = DateTime.parse(boardElement.getAttribute('updatedAt')!);
      
      final elements = <VisionBoardElement>[];
      final elementsNode = boardElement.children
          .whereType<XmlElement>()
          .firstWhere((e) => e.name.local == 'Elements');
      
      for (final elementNode in elementsNode.children.whereType<XmlElement>()) {
        if (elementNode.name.local == 'ImageElement') {
          elements.add(await _parseImageElement(elementNode));
        } else if (elementNode.name.local == 'TextElement') {
          elements.add(_parseTextElement(elementNode));
        }
      }
      
      return VisionBoard(
        id: id,
        title: title,
        elements: elements,
        backgroundColor: backgroundColor,
        createdAt: createdAt,
        updatedAt: updatedAt,
      );
      
    } catch (e) {
      print('❌ Error parsing vision board XML: $e');
      return null;
    }
  }
  
  /// Parse XML image element
  Future<VisionBoardImageElement> _parseImageElement(XmlElement element) async {
    String imagePath = element.getAttribute('imagePath') ?? '';
    final elementId = element.getAttribute('id') ?? 'unknown';
    
    print('🔍 Parsing image element: $elementId');
    print('🔍 Original imagePath: $imagePath');
    
    // Check if we have base64 image data embedded in XML
    final base64ImageData = element.getAttribute('imageData');
    if (base64ImageData != null && base64ImageData.isNotEmpty) {
      print('✅ Found base64 image data (${base64ImageData.length} characters)');
      try {
        // Decode base64 data
        final imageBytes = base64Decode(base64ImageData);
        
        // Create a local file from the base64 data
        final Directory appDir = await getApplicationDocumentsDirectory();
        final String fileName = 'vision_board_${elementId}.jpg';
        final String localPath = '${appDir.path}/$fileName';
        final File imageFile = File(localPath);
        
        // Write the decoded bytes to local file
        await imageFile.writeAsBytes(imageBytes);
        imagePath = localPath;
        
        print('📷 Restored image from base64: $localPath (${imageBytes.length} bytes)');
      } catch (e) {
        print('❌ Error decoding base64 image: $e');
        // Fall back to original imagePath if base64 decoding fails
        print('🔄 Falling back to original imagePath: $imagePath');
      }
    } else {
      print('⚠️ No base64 image data found, using imagePath: $imagePath');
    }
    
    return VisionBoardImageElement(
      id: element.getAttribute('id')!,
      position: Offset(
        double.parse(element.getAttribute('x')!),
        double.parse(element.getAttribute('y')!),
      ),
      size: Size(
        double.parse(element.getAttribute('width')!),
        double.parse(element.getAttribute('height')!),
      ),
      rotation: double.parse(element.getAttribute('rotation')!),
      zIndex: int.parse(element.getAttribute('zIndex')!),
      imagePath: imagePath,
      imageUrl: element.getAttribute('imageUrl'),
      fit: BoxFit.values.firstWhere((fit) => fit.name == element.getAttribute('fit')),
    );
  }
  
  /// Parse XML text element
  VisionBoardTextElement _parseTextElement(XmlElement element) {
    final colorHex = element.getAttribute('color')!;
    final color = Color(int.parse(colorHex.replaceAll('#', 'FF'), radix: 16));
    final fontSize = double.parse(element.getAttribute('fontSize')!);
    final fontWeight = FontWeight.values[int.parse(element.getAttribute('fontWeight')!)];
    final textAlign = TextAlign.values.firstWhere((align) => align.name == element.getAttribute('textAlign'));
    
    return VisionBoardTextElement(
      id: element.getAttribute('id')!,
      position: Offset(
        double.parse(element.getAttribute('x')!),
        double.parse(element.getAttribute('y')!),
      ),
      size: Size(
        double.parse(element.getAttribute('width')!),
        double.parse(element.getAttribute('height')!),
      ),
      rotation: double.parse(element.getAttribute('rotation')!),
      zIndex: int.parse(element.getAttribute('zIndex')!),
      text: element.getAttribute('text')!,
      style: TextStyle(
        fontSize: fontSize,
        fontWeight: fontWeight,
        color: color,
      ),
      alignment: textAlign,
    );
  }

  /// Download thumbnail for a vision board picture
  Future<Uint8List?> downloadThumbnail(int visionBoardPictureId) async {
    try {
      print('📥 Downloading thumbnail for vision board picture ID: $visionBoardPictureId');
      
      if (_apiClient == null) {
        print('❌ API client not initialized');
        return null;
      }
      
      // Use the swagger API to download thumbnail
      final response = await _apiClient!.apiV1ManifestDownloadvisionboardpicturethumbnailGet(
        visionBoardPictureId: visionBoardPictureId,
      );
      
      if (response.isSuccessful) {
        // The response body should contain the image data
        // Check if it's base64 encoded or raw bytes
        if (response.bodyBytes != null && response.bodyBytes!.isNotEmpty) {
          print('✅ Thumbnail downloaded: ${response.bodyBytes!.length} bytes');
          return response.bodyBytes;
        } else if (response.bodyString != null && response.bodyString!.isNotEmpty) {
          // Try to decode if it's base64
          try {
            final bytes = base64Decode(response.bodyString!);
            print('✅ Thumbnail downloaded (base64): ${bytes.length} bytes');
            return bytes;
          } catch (e) {
            print('⚠️ Could not decode base64, treating as raw bytes');
            return Uint8List.fromList(response.bodyString!.codeUnits);
          }
        }
      } else {
        print('❌ Failed to download thumbnail:');
        print('  - Status Code: ${response.statusCode}');
        print('  - Response: ${response.bodyString}');
      }
      
      return null;
    } catch (e) {
      print('❌ Error downloading thumbnail: $e');
      return null;
    }
  }
  
  /// Upload thumbnail for a vision board picture
  Future<bool> _uploadThumbnail(int visionBoardPictureId, Map<String, dynamic> thumbnailInfo) async {
    try {
      print('📸 Uploading thumbnail for vision board picture ID: $visionBoardPictureId');
      
      // Read thumbnail file
      final thumbnailFile = File(thumbnailInfo['filePath']);
      if (!await thumbnailFile.exists()) {
        print('❌ Thumbnail file not found at path: ${thumbnailInfo['filePath']}');
        return false;
      }
      
      final thumbnailBytes = await thumbnailFile.readAsBytes();
      print('📸 Read thumbnail: ${thumbnailBytes.length} bytes');
      
      // Use manual HTTP for thumbnail upload
      final client = _apiClient!.client;
      final authService = UnifiedAuthService();
      final token = authService.authToken;
      
      final uri = Uri.parse('${client.baseUrl}/api/v1/manifest/uploadvisionboardpicturethumbnail');
      final request = http.MultipartRequest('POST', uri);
      
      // Add headers
      if (token != null) {
        request.headers['Authorization'] = 'Bearer $token';
      }
      
      // Add form fields
      request.fields['VisionBoardPictureId'] = visionBoardPictureId.toString();
      
      // Add thumbnail file
      request.files.add(http.MultipartFile.fromBytes(
        'UploadedFile',
        thumbnailBytes,
        filename: thumbnailInfo['fileName'] ?? 'thumbnail.png',
      ));
      
      print('📸 Thumbnail upload request:');
      print('  - VisionBoardPictureId: $visionBoardPictureId');
      print('  - File: ${thumbnailInfo['fileName']} (${thumbnailBytes.length} bytes)');
      
      final streamedResponse = await request.send();
      final responseString = await streamedResponse.stream.bytesToString();
      
      if (streamedResponse.statusCode >= 200 && streamedResponse.statusCode < 300) {
        print('✅ Thumbnail uploaded successfully');
        return true;
      } else {
        print('❌ Thumbnail upload failed:');
        print('  - Status Code: ${streamedResponse.statusCode}');
        print('  - Response: $responseString');
        return false;
      }
      
    } catch (e) {
      print('❌ Error uploading thumbnail: $e');
      return false;
    }
  }
}

/// Result wrapper for vision board operations
class VisionBoardResult<T> {
  final bool isSuccess;
  final T? data;
  final String message;

  const VisionBoardResult._({
    required this.isSuccess,
    this.data,
    required this.message,
  });

  factory VisionBoardResult.success({T? data, required String message}) {
    return VisionBoardResult._(
      isSuccess: true,
      data: data, 
      message: message,
    );
  }

  factory VisionBoardResult.failure({required String message}) {
    return VisionBoardResult._(
      isSuccess: false,
      message: message,
    );
  }
}

/// Auth interceptor for API requests
class _AuthInterceptor implements Interceptor {
  @override
  FutureOr<Response<BodyType>> intercept<BodyType>(Chain<BodyType> chain) async {
    final request = chain.request;
    final authService = UnifiedAuthService();
    
    print('🔍 DEBUG: Auth interceptor - URL: ${request.url}');
    print('🔍 DEBUG: Auth interceptor - Method: ${request.method}');
    print('🔍 DEBUG: Auth interceptor - Headers: ${request.headers}');
    print('🔍 DEBUG: Auth interceptor - Body type: ${request.body.runtimeType}');
    
    final token = authService.authToken;
    print('🔍 DEBUG: Auth interceptor - Token available: ${token != null}');
    print('🔍 DEBUG: Auth interceptor - Token length: ${token?.length ?? 0}');
    
    if (token != null) {
      final existingHeaders = Map<String, String>.from(request.headers);
      existingHeaders['Authorization'] = 'Bearer $token';
      
      // Don't override content-type for multipart requests - let Chopper handle it
      if (request.url.path.contains('createvisionboardpicture') || 
          request.url.path.contains('updatevisionboardpicture')) {
        // Remove content-type header for multipart requests to let Chopper set it properly
        existingHeaders.remove('content-type');
        print('🔍 DEBUG: Auth interceptor - Removed content-type for multipart request');
      }
      
      print('🔍 DEBUG: Auth interceptor - Added Authorization header');
      print('🔍 DEBUG: Auth interceptor - Final headers: $existingHeaders');
      
      final updatedRequest = request.copyWith(headers: existingHeaders);
      final response = await chain.proceed(updatedRequest);
      
      print('🔍 DEBUG: Auth interceptor - Response status: ${response.statusCode}');
      print('🔍 DEBUG: Auth interceptor - Response headers: ${response.headers}');
      
      return response;
    }
    
    print('🔍 DEBUG: Auth interceptor - No token, proceeding without auth');
    return chain.proceed(request);
  }
}

/// Simple response wrapper for manual HTTP calls
class _ManualResponse {
  final bool isSuccessful;
  final int statusCode;
  final Map<String, dynamic> body;

  _ManualResponse({
    required this.isSuccessful,
    required this.statusCode,
    required this.body,
  });
}

