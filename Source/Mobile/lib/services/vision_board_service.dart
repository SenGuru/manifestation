import 'dart:async';
import 'dart:convert';
import 'dart:io';
import 'dart:typed_data';
import 'dart:math' as math;
import 'package:chopper/chopper.dart';
import 'package:http/http.dart' as http;
import '../models/vision_board.dart';
import '../api/generated/code/manifestation.swagger.dart';
import 'unified_auth_service.dart';
import 'package:xml/xml.dart';
import 'package:flutter/material.dart';
import 'package:path_provider/path_provider.dart';

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
  
  // Memory cache for vision board XML content (cleared on app restart)
  final Map<String, String> _memoryCache = {};
  final Map<String, DateTime> _cacheTimestamps = {};
  
  /// Clear all cached vision board content
  void clearCache() {
    _memoryCache.clear();
    _cacheTimestamps.clear();
    print('🗑️ Cleared all vision board memory cache');
  }
  
  /// Clear specific vision board from cache
  void clearCacheForBoard(String visionBoardId, int userId) {
    final cacheKey = '${userId}_$visionBoardId';
    _memoryCache.remove(cacheKey);
    _cacheTimestamps.remove(cacheKey);
    print('🗑️ Cleared cache for vision board: $visionBoardId');
  }
  
  /// Get cache statistics
  Map<String, dynamic> getCacheStats() {
    return {
      'cachedBoards': _memoryCache.length,
      'totalCacheSize': _memoryCache.values.fold(0, (sum, xml) => sum + xml.length),
      'oldestCache': _cacheTimestamps.values.isEmpty ? null : _cacheTimestamps.values.reduce((a, b) => a.isBefore(b) ? a : b),
      'newestCache': _cacheTimestamps.values.isEmpty ? null : _cacheTimestamps.values.reduce((a, b) => a.isAfter(b) ? a : b),
    };
  }
  
  /// Public method to get picture ID for a vision board (for external use)
  Future<int?> getPictureIdForBoard(String visionBoardId) async {
    final userId = await _authService!.getUserId();
    if (userId == null) return null;
    return await _getPictureIdByVisionBoardId(visionBoardId, userId);
  }
  
  Future<void> initialize({String? baseUrl}) async {
    if (baseUrl != null) {
      final chopperClient = ChopperClient(
        baseUrl: Uri.parse(baseUrl),
        interceptors: [_AuthInterceptor()],
        converter: $JsonSerializableConverter(),
        services: [Manifestation.create()],
      );
      
      _apiClient = chopperClient.getService<Manifestation>();
      _authService = UnifiedAuthService();
      await _authService!.initialize(baseUrl: baseUrl);
    }
  }
  
  // ========================= CREATE VISION BOARD =========================
  
  /// Create a new vision board on server
  Future<VisionBoardResult<int?>> createVisionBoard(VisionBoard visionBoard) async {
    try {
      print('📤 Creating vision board "${visionBoard.title}" on server...');
      
      if (_apiClient == null || _authService == null) {
        return VisionBoardResult.failure(message: 'Service not initialized');
      }
      
      final userId = await _authService!.getUserId();
      if (userId == null) {
        return VisionBoardResult.failure(message: 'User not authenticated');
      }
      
      // Generate XML content
      final xmlContent = _generateVisionBoardXML(visionBoard, userId);
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
        final pictureId = response.body['model']?['visionBoardPictureId'];
        print('✅ Vision board created successfully with ID: $pictureId');
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
      return VisionBoardResult.failure(message: 'Failed to create vision board: $e');
    }
  }
  
  // ========================= UPDATE VISION BOARD =========================
  
  /// Update existing vision board on server
  Future<VisionBoardResult<bool>> updateVisionBoard(VisionBoard visionBoard, {int? pictureId}) async {
    try {
      if (_apiClient == null || _authService == null) {
        return VisionBoardResult.failure(message: 'Service not initialized');
      }
      
      final userId = await _authService!.getUserId();
      if (userId == null) {
        return VisionBoardResult.failure(message: 'User not authenticated');
      }
      
      // Use provided picture ID or lookup by vision board ID
      final finalPictureId = pictureId ?? await _getPictureIdByVisionBoardId(visionBoard.id, userId);
      if (finalPictureId == null) {
        return VisionBoardResult.failure(message: 'Vision board not found on server');
      }
      
      // Generate updated XML content
      final xmlContent = _generateVisionBoardXML(visionBoard, userId);
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
      return VisionBoardResult.failure(message: 'Failed to update vision board: $e');
    }
  }
  
  // ========================= GET VISION BOARDS =========================
  
  /// Get all vision boards for current user (metadata only for listing)
  Future<VisionBoardResult<List<VisionBoard>>> getUserVisionBoards() async {
    try {
      if (_apiClient == null || _authService == null) {
        return VisionBoardResult.failure(message: 'Service not initialized');
      }
      
      final userId = await _authService!.getUserId();
      if (userId == null) {
        return VisionBoardResult.failure(message: 'User not authenticated');
      }
      
      print('📥 Getting vision board list for user: $userId');
      
      // Get list of vision boards from server (metadata only)
      final response = await _apiClient!.apiV1ManifestGetvisionboardpicturesGet(
        userId: userId,
      );
      
      if (!response.isSuccessful || response.body?.isSuccess != true) {
        return VisionBoardResult.failure(
          message: response.body?.errorHolder?.toString() ?? 'Failed to get vision boards'
        );
      }
      
      final visionBoards = <VisionBoard>[];
      final pictures = response.body?.model ?? [];
      
      print('📋 Found ${pictures.length} vision board files');
      
      // Create lightweight VisionBoard objects from metadata (no XML content)
      for (final picture in pictures) {
        if (picture.fileName?.endsWith('.xml') == true && picture.isDeleted != true) {
          try {
            final visionBoard = _createVisionBoardFromMetadata(picture);
            if (visionBoard != null) {
              visionBoards.add(visionBoard);
              print('📝 Added vision board: ${visionBoard.id} (Picture ID: ${picture.visionBoardPictureId})');
            }
          } catch (e) {
            print('⚠️ Failed to create vision board from metadata ${picture.visionBoardPictureId}: $e');
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
      return VisionBoardResult.failure(message: 'Failed to get vision boards: $e');
    }
  }
  
  /// Get specific vision board with full content by ID
  Future<VisionBoardResult<VisionBoard?>> getVisionBoard(String visionBoardId) async {
    try {
      if (_apiClient == null || _authService == null) {
        return VisionBoardResult.failure(message: 'Service not initialized');
      }
      
      final userId = await _authService!.getUserId();
      if (userId == null) {
        return VisionBoardResult.failure(message: 'User not authenticated');
      }
      
      print('📥 Getting full vision board content for ID: $visionBoardId');
      
      // Check memory cache first
      final cacheKey = '${userId}_$visionBoardId';
      if (_memoryCache.containsKey(cacheKey)) {
        final cachedXml = _memoryCache[cacheKey]!;
        final cacheTime = _cacheTimestamps[cacheKey]!;
        print('💾 Found cached XML content (${cachedXml.length} chars, cached at ${cacheTime.toIso8601String()})');
        
        // Parse cached XML content
        final visionBoard = await _parseVisionBoardXML(cachedXml);
        if (visionBoard != null) {
          print('✅ Successfully parsed cached vision board with ${visionBoard.elements.length} elements');
          return VisionBoardResult.success(
            data: visionBoard,
            message: 'Vision board retrieved from cache'
          );
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
        return VisionBoardResult.failure(message: 'Failed to download vision board content');
      }
      
      // Get the XML content from the response
      final xmlContent = response.bodyString;
      if (xmlContent.trim().isEmpty) {
        print('❌ Empty response from download endpoint');
        return VisionBoardResult.failure(message: 'Empty vision board content');
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
      _memoryCache[cacheKey] = xmlContent;
      _cacheTimestamps[cacheKey] = DateTime.now();
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
        return VisionBoardResult.failure(message: 'Failed to parse vision board XML');
      }
      
    } catch (e) {
      print('❌ Error getting vision board: $e');
      return VisionBoardResult.failure(message: 'Failed to get vision board: $e');
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
        return VisionBoardResult.failure(message: 'Service not initialized');
      }
      
      final userId = await _authService!.getUserId();
      if (userId == null) {
        return VisionBoardResult.failure(message: 'User not authenticated');
      }
      
      // First, get the picture ID for this vision board
      final pictureId = await _getPictureIdByVisionBoardId(visionBoardId, userId);
      if (pictureId == null) {
        return VisionBoardResult.failure(message: 'Vision board not found on server');
      }
      
      print('🗑️ Deleting vision board $visionBoardId (Picture ID: $pictureId)');
      
      final response = await _apiClient!.apiV1ManifestMarkvisionboardpictureasdeletedDelete(
        id: pictureId,
      );
      
      if (response.isSuccessful && response.body?.isSuccess == true) {
        print('✅ Vision board deleted successfully');
        return VisionBoardResult.success(
          data: true,
          message: 'Vision board deleted successfully'
        );
      } else {
        final errorMessage = response.body?.errorHolder?.toString() ?? 
                            'Failed to delete vision board';
        return VisionBoardResult.failure(message: errorMessage);
      }
      
    } catch (e) {
      print('❌ Error deleting vision board: $e');
      return VisionBoardResult.failure(message: 'Failed to delete vision board: $e');
    }
  }
  
  // ========================= HELPER METHODS =========================
  
  /// Get picture ID for a vision board by its ID
  Future<int?> _getPictureIdByVisionBoardId(String visionBoardId, int userId) async {
    try {
      print('🔍 Looking for picture ID for vision board: $visionBoardId');
      
      // Get all vision board pictures for user using correct endpoint
      final response = await _apiClient!.apiV1ManifestGetvisionboardpicturesGet(
        userId: userId,
      );
      
      if (response.isSuccessful && response.body?.isSuccess == true) {
        final pictures = response.body?.model ?? [];
        print('🔍 Found ${pictures.length} pictures, searching for match...');
        
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
      } else {
        print('❌ Failed to get pictures list: ${response.body?.errorHolder}');
      }
      
      return null;
    } catch (e) {
      print('❌ Error getting picture ID: $e');
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
      
      return VisionBoard(
        id: boardId,
        title: 'Vision Board', // Default title, could be enhanced
        elements: [], // Empty elements since we can't parse XML
        backgroundColor: const Color(0xFFFFFFFF), // White color
        createdAt: createdAt,
        updatedAt: modifiedAt,
      );
    } catch (e) {
      print('❌ Error creating VisionBoard from metadata: $e');
      return null;
    }
  }
  
  /// Generate XML content for vision board
  String _generateVisionBoardXML(VisionBoard visionBoard, int userId) {
    final builder = XmlBuilder();
    builder.processing('xml', 'version="1.0" encoding="UTF-8"');
    builder.element('VisionBoards', nest: () {
      builder.attribute('version', '1.0');
      builder.attribute('lastModified', DateTime.now().toUtc().toIso8601String());
      builder.attribute('userId', userId.toString());
      
      builder.element('VisionBoard', nest: () {
        builder.attribute('id', visionBoard.id);
        builder.attribute('title', visionBoard.title);
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
                
                // Convert image to base64 and embed in XML
                print('🔄 Processing image element: ${element.id}');
                print('🔄 Image path: ${element.imagePath}');
                
                try {
                  final imageFile = File(element.imagePath);
                  print('📁 Checking if image file exists: ${element.imagePath}');
                  
                  if (imageFile.existsSync()) {
                    print('✅ Image file exists, reading bytes...');
                    final imageBytes = imageFile.readAsBytesSync();
                    print('✅ Read ${imageBytes.length} bytes from image');
                    
                    final base64Image = base64Encode(imageBytes);
                    builder.attribute('imageData', base64Image);
                    print('📷 Successfully embedded image as base64: ${base64Image.length} characters');
                    print('🔍 Base64 preview: ${base64Image.substring(0, 50)}...');
                  } else {
                    print('❌ Image file not found at path: ${element.imagePath}');
                  }
                } catch (e) {
                  print('❌ Error reading image file: $e');
                  print('❌ Stack trace: $e');
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

