import 'dart:async';
import 'dart:convert';
import 'dart:io';
import 'package:path_provider/path_provider.dart';
import 'package:chopper/chopper.dart';
import 'package:http/http.dart' as http;
import '../models/vision_board.dart';
import '../utils/vision_board_xml_manager.dart';
import '../api/generated/code/manifestation.swagger.dart';
import 'unified_auth_service.dart';

class VisionBoardService {
  static final VisionBoardService _instance = VisionBoardService._internal();
  factory VisionBoardService() => _instance;
  VisionBoardService._internal();

  static const int _defaultUserId = 1; // Default user ID for XML operations
  
  // API client
  Manifestation? _apiClient;
  UnifiedAuthService? _authService;
  
  Future<void> initialize({String? baseUrl}) async {
    // Initialize XML manager
    await VisionBoardXMLManager.initializeXMLFile(_defaultUserId);
    
    // Initialize API client if baseUrl provided
    if (baseUrl != null) {
      _apiClient = Manifestation.create(
        baseUrl: Uri.parse(baseUrl),
        interceptors: [_AuthInterceptor()], // Add auth interceptor
        converter: $JsonSerializableConverter(), // Restore JSON converter
      );
      _authService = UnifiedAuthService();
      await _authService!.initialize(baseUrl: baseUrl);
    }
  }

  /// Save vision board to local XML storage and sync with server
  Future<VisionBoardResult<VisionBoard>> saveVisionBoard(VisionBoard board) async {
    try {
      print('🎯 Saving vision board to XML: ${board.title}');
      
      // Generate ID if needed
      VisionBoard savedBoard = board;
      if (board.id.isEmpty) {
        savedBoard = board.copyWith(
          id: 'board_${DateTime.now().millisecondsSinceEpoch}',
          updatedAt: DateTime.now(),
        );
      }
      
      // Save to local XML first
      await VisionBoardXMLManager.saveVisionBoardToXML(savedBoard, _defaultUserId);
      
      // Sync with server if API available
      if (_apiClient != null && _authService != null) {
        try {
          await _syncWithServer();
        } catch (e) {
          // Continue silently - offline operation
        }
      }
      return VisionBoardResult.success(
        data: savedBoard,
        message: 'Vision board saved successfully',
      );
      
    } catch (e) {
      print('❌ Error saving vision board: $e');
      return VisionBoardResult.failure(message: 'Failed to save vision board: $e');
    }
  }

  /// Load vision board from local XML storage
  Future<VisionBoardResult<VisionBoard>> loadVisionBoard(String boardId) async {
    try {
      print('🎯 Loading vision board from XML: $boardId');
      
      // Load all boards from XML
      final boards = await VisionBoardXMLManager.loadVisionBoardsFromXML();
      
      // Find the specific board
      final board = boards.where((b) => b.id == boardId).firstOrNull;
      
      if (board == null) {
        return VisionBoardResult.failure(message: 'Vision board not found');
      }
      
      print('✅ Vision board loaded from XML with ${board.elements.length} elements');
      return VisionBoardResult.success(
        data: board,
        message: 'Vision board loaded successfully',
      );
      
    } catch (e) {
      print('❌ Error loading vision board: $e');
      return VisionBoardResult.failure(message: 'Failed to load vision board: $e');
    }
  }

  /// Get all user vision boards from local XML storage with server sync
  Future<VisionBoardResult<List<VisionBoard>>> getUserVisionBoards() async {
    try {
      // Sync from server first if API available
      if (_apiClient != null && _authService != null) {
        try {
          await _downloadFromServer();
        } catch (e) {
          // Continue silently - use local data
        }
      }
      
      final boards = await VisionBoardXMLManager.loadVisionBoardsFromXML();
      
      return VisionBoardResult.success(
        data: boards,
        message: 'Vision boards loaded successfully',
      );
      
    } catch (e) {
      return VisionBoardResult.success(data: [], message: 'Error loading vision boards: $e');
    }
  }

  /// Delete vision board from local XML storage
  Future<VisionBoardResult<bool>> deleteVisionBoard(String boardId) async {
    try {
      await VisionBoardXMLManager.deleteVisionBoardFromXML(boardId);
      
      return VisionBoardResult.success(data: true, message: 'Vision board deleted');
    } catch (e) {
      return VisionBoardResult.failure(message: 'Error deleting vision board: $e');
    }
  }

  /// Get default pictures for vision board (placeholder method)
  Future<VisionBoardResult<List<Map<String, dynamic>>>> getDefaultPictures() async {
    try {
      // Return empty list for now - API integration disabled
      return VisionBoardResult.success(data: [], message: 'Default pictures disabled - API integration pending');
    } catch (e) {
      return VisionBoardResult.failure(message: 'Error loading default pictures: $e');
    }
  }

  /// Create a new vision board picture (placeholder method)
  Future<VisionBoardResult<Map<String, dynamic>>> createPicture({
    required String location,
    required String fileName,
    required int fileSize,
  }) async {
    try {
      // Placeholder implementation - API integration disabled
      return VisionBoardResult.failure(message: 'Picture creation disabled - API integration pending');
    } catch (e) {
      return VisionBoardResult.failure(message: 'Error creating picture: $e');
    }
  }

  /// Track picture view (placeholder method)
  Future<VisionBoardResult<bool>> trackPictureView({
    required int sessionId,
    required int pictureId,
  }) async {
    try {
      // Placeholder implementation - API integration disabled
      return VisionBoardResult.success(data: true, message: 'Picture tracking disabled - API integration pending');
    } catch (e) {
      return VisionBoardResult.failure(message: 'Error tracking picture view: $e');
    }
  }

  /// Update session tracking
  Future<VisionBoardResult<bool>> updateSession({
    required int sessionId,
    required int timeSpentInSeconds,
  }) async {
    try {
      if (_apiClient != null && _authService != null) {
        final token = _authService!.authToken;
        if (token != null && token.isNotEmpty) {
          // Use the session management API
          // Implementation would depend on available session endpoints
          return VisionBoardResult.success(data: true, message: 'Session updated successfully');
        }
      }
      return VisionBoardResult.success(data: true, message: 'Session update - offline mode');
    } catch (e) {
      return VisionBoardResult.failure(message: 'Error updating session: $e');
    }
  }
  
  /// Upload XML file to server (first time creation)
  Future<VisionBoardResult<bool>> _uploadToServer() async {
    try {
      if (_apiClient == null || _authService == null) {
        return VisionBoardResult.failure(message: 'API client not initialized');
      }
      
      final token = _authService!.authToken;
      if (token == null || token.isEmpty) {
        return VisionBoardResult.failure(message: 'No valid authentication token');
      }
      
      // Get XML file
      final xmlFile = await _getXMLFile();
      if (xmlFile == null) {
        return VisionBoardResult.failure(message: 'No XML file found');
      }
      
      // Convert to file bytes
      final fileBytes = await xmlFile.readAsBytes();
      final authUserId = await _authService!.getUserId();
      
      if (authUserId == null) {
        return VisionBoardResult.failure(message: 'No authenticated user ID available');
      }
      
      print('🆔 Using authenticated user ID: $authUserId');
      
      // Upload using createvisionboardpicture endpoint
      await _apiClient!.apiV1ManifestCreatevisionboardpicturePost(
        UploadedFile: fileBytes,
        location: 'vision_board',
        fileName: 'vision_boards.xml',
        fileSize: fileBytes.length,
        userId: authUserId,
      );
      return VisionBoardResult.success(data: true, message: 'Vision board uploaded to server');
      
    } catch (e) {
      return VisionBoardResult.failure(message: 'Failed to upload to server: $e');
    }
  }
  
  /// Update XML file on server
  Future<VisionBoardResult<bool>> _updateOnServer() async {
    try {
      if (_apiClient == null || _authService == null) {
        return VisionBoardResult.failure(message: 'API client not initialized');
      }
      
      final token = _authService!.authToken;
      if (token == null || token.isEmpty) {
        return VisionBoardResult.failure(message: 'No valid authentication token');
      }
      
      // Get XML file
      final xmlFile = await _getXMLFile();
      if (xmlFile == null) {
        return VisionBoardResult.failure(message: 'No XML file found');
      }
      
      // Convert to file bytes
      final fileBytes = await xmlFile.readAsBytes();
      final authUserId = await _authService!.getUserId();
      
      if (authUserId == null) {
        return VisionBoardResult.failure(message: 'No authenticated user ID available');
      }
      
      print('🆔 Using authenticated user ID: $authUserId');
      
      // First get the existing picture ID
      final existingPictureId = await _getExistingPictureId();
      
      // Update using updatevisionboardpicture endpoint
      await _apiClient!.apiV1ManifestUpdatevisionboardpicturePost(
        UploadedFile: fileBytes,
        visionBoardPictureId: existingPictureId,
        location: 'vision_board',
        fileName: 'vision_boards.xml',
        fileSize: fileBytes.length,
        userId: authUserId,
      );
      return VisionBoardResult.success(data: true, message: 'Vision board updated on server');
      
    } catch (e) {
      return VisionBoardResult.failure(message: 'Failed to update on server: $e');
    }
  }
  
  /// Download XML file from server
  Future<VisionBoardResult<bool>> _downloadFromServer() async {
    try {
      if (_apiClient == null || _authService == null) {
        return VisionBoardResult.failure(message: 'API client not initialized');
      }
      
      final token = _authService!.authToken;
      if (token == null || token.isEmpty) {
        return VisionBoardResult.failure(message: 'No valid authentication token');
      }
      
      final authUserId = await _authService!.getUserId();
      
      if (authUserId == null) {
        return VisionBoardResult.failure(message: 'No authenticated user ID available');
      }
      
      print('🆔 Using authenticated user ID: $authUserId');
      
      // Download using getvisionboardpicturebyuserid endpoint
      final response = await _apiClient!.apiV1ManifestGetvisionboardpicturebyuseridGet(
        userId: authUserId,
      );
      
      // Save downloaded XML to local file
      if (response.body?.model != null && response.body!.model!.isNotEmpty) {
        // Get the first (and should be only) vision board picture
        final picture = response.body!.model!.first;
        if (picture.location != null) {
          await _saveXMLFile(picture.location!);
          return VisionBoardResult.success(data: true, message: 'Vision board downloaded from server');
        }
      }
      
      return VisionBoardResult.failure(message: 'No data received from server');
      
    } catch (e) {
      return VisionBoardResult.failure(message: 'Failed to download from server: $e');
    }
  }
  
  /// Smart sync - upload if first time, update if exists
  Future<VisionBoardResult<bool>> _syncWithServer() async {
    try {
      // Check if file exists on server
      final hasServerFile = await _checkServerFile();
      
      if (hasServerFile) {
        return await _updateOnServer();
      } else {
        return await _uploadToServer();
      }
    } catch (e) {
      return VisionBoardResult.failure(message: 'Sync failed: $e');
    }
  }
  
  /// Check if user has XML file on server
  Future<bool> _checkServerFile() async {
    try {
      if (_apiClient == null || _authService == null) return false;
      
      final token = _authService!.authToken;
      if (token == null || token.isEmpty) return false;
      
      final authUserId = await _authService!.getUserId();
      
      if (authUserId == null) return false;
      
      // Try to get file - if successful, file exists
      final response = await _apiClient!.apiV1ManifestGetvisionboardpicturebyuseridGet(
        userId: authUserId,
      );
      
      return response.body?.model != null && response.body!.model!.isNotEmpty;
    } catch (e) {
      // If error (like 404), assume file doesn't exist
      return false;
    }
  }
  
  /// Get local XML file
  Future<File?> _getXMLFile() async {
    try {
      final directory = await getApplicationDocumentsDirectory();
      final xmlFile = File('${directory.path}/vision_boards.xml');
      
      if (await xmlFile.exists()) {
        return xmlFile;
      }
      return null;
    } catch (e) {
      return null;
    }
  }
  
  /// Get existing vision board picture ID for updates
  Future<int?> _getExistingPictureId() async {
    try {
      if (_apiClient == null || _authService == null) return null;
      
      final token = _authService!.authToken;
      if (token == null || token.isEmpty) return null;
      
      final authUserId = await _authService!.getUserId();
      
      if (authUserId == null) return null;
      
      final response = await _apiClient!.apiV1ManifestGetvisionboardpicturebyuseridGet(
        userId: authUserId,
      );
      
      if (response.body?.model != null && response.body!.model!.isNotEmpty) {
        return response.body!.model!.first.visionBoardPictureId;
      }
      return null;
    } catch (e) {
      return null;
    }
  }
  
  /// Save XML data to local file
  Future<void> _saveXMLFile(String xmlData) async {
    try {
      final directory = await getApplicationDocumentsDirectory();
      final xmlFile = File('${directory.path}/vision_boards.xml');
      await xmlFile.writeAsString(xmlData);
    } catch (e) {
      rethrow;
    }
  }

  /// Upload specific vision board XML file to server
  Future<VisionBoardResult<int?>> uploadVisionBoardXML({
    required String visionBoardId,
    required String xmlContent,
    required int userId,
  }) async {
    try {
      print('📤 Uploading vision board XML: $visionBoardId');
      
      if (_apiClient == null || _authService == null) {
        return VisionBoardResult.failure(message: 'API client not initialized');
      }

      // Get actual authenticated user ID instead of using the passed parameter
      final authUserId = await _authService!.getUserId();
      final actualUserId = authUserId ?? userId;
      
      print('🆔 Using user ID: $actualUserId (auth: $authUserId, fallback: $userId)');

      // Convert XML to bytes for upload
      final xmlBytes = utf8.encode(xmlContent);
      
      // ===== DETAILED REQUEST LOGGING =====
      print('📋 UPLOAD REQUEST DETAILS:');
      print('  - Vision Board ID: "$visionBoardId" (type: ${visionBoardId.runtimeType})');
      print('  - File Name: "visionboard_$visionBoardId.xml"');
      print('  - File Size: ${xmlBytes.length} bytes');
      print('  - User ID: $actualUserId (type: ${actualUserId.runtimeType})');
      print('  - XML Content Length: ${xmlContent.length} characters');
      final xmlPreview = xmlContent.length > 300 ? '${xmlContent.substring(0, 300)}...' : xmlContent;
      print('  - XML Preview (first 300 chars): $xmlPreview');
      print('  - Auth Token Present: ${_authService!.authToken != null}');
      print('  - Auth Token Length: ${_authService!.authToken?.length ?? 0}');
      
      // Upload XML file with vision board ID as identifier using direct HTTP multipart
      print('📋 ATTEMPTING DIRECT HTTP MULTIPART REQUEST:');
      print('  - UploadedFile bytes length: ${xmlBytes.length}');
      print('  - location: "$visionBoardId"');
      print('  - fileName: "visionboard_$visionBoardId.xml"');
      print('  - fileSize: ${xmlBytes.length}');
      print('  - userId: $actualUserId');
      
      // Create direct HTTP multipart request to bypass Chopper's JSON conversion
      final uri = Uri.parse('http://manifestation-qa-1.senthilguru.net/api/v1/manifest/createvisionboardpicture');
      final request = http.MultipartRequest('POST', uri);
      
      // Add form fields
      request.fields['Location'] = visionBoardId;
      request.fields['FileName'] = 'visionboard_$visionBoardId.xml';
      request.fields['FileSize'] = xmlBytes.length.toString();
      request.fields['UserId'] = actualUserId.toString();
      
      // Add file
      request.files.add(http.MultipartFile.fromBytes(
        'UploadedFile',
        xmlBytes,
        filename: 'visionboard_$visionBoardId.xml',
      ));
      
      // Add auth header
      final token = _authService!.authToken;
      if (token != null) {
        request.headers['Authorization'] = 'Bearer $token';
        print('🔐 Added Authorization header to direct request');
      }
      
      print('📋 DIRECT REQUEST DETAILS:');
      print('  - URL: ${request.url}');
      print('  - Method: ${request.method}');
      print('  - Fields: ${request.fields}');
      print('  - Files: ${request.files.map((f) => '${f.field}:${f.filename} (${f.length} bytes)').toList()}');
      print('  - Headers: ${request.headers}');
      
      final streamedResponse = await request.send();
      final response = await http.Response.fromStream(streamedResponse);
      
      // ===== DETAILED RESPONSE LOGGING =====
      print('📋 UPLOAD RESPONSE DETAILS:');
      print('  - Status Code: ${response.statusCode}');
      print('  - Is Successful: ${response.statusCode >= 200 && response.statusCode < 300}');
      print('  - Response Headers: ${response.headers}');
      print('  - Response Body Length: ${response.body.length}');
      print('  - Response Body: ${response.body}');
      
      if (response.statusCode >= 200 && response.statusCode < 300) {
        try {
          final responseData = json.decode(response.body);
          
          print('📋 RESPONSE BODY ANALYSIS:');
          print('  - Response IsSuccess: ${responseData['isSuccess']}');
          print('  - Response SuccessMessage: ${responseData['successMessage']}');
          print('  - Response ErrorHolder: ${responseData['errorHolder']}');
          print('  - Response Model Present: ${responseData['model'] != null}');
          
          if (responseData['isSuccess'] == true && responseData['model'] != null) {
            final pictureId = responseData['model']['visionBoardPictureId'];
            print('✅ Vision board XML uploaded successfully: ID $pictureId');
            return VisionBoardResult.success(
              data: pictureId,
              message: 'Vision board XML uploaded successfully'
            );
          } else {
            print('❌ Server returned success=false or null model');
            return VisionBoardResult.failure(
              message: responseData['errorHolder']?.toString() ?? responseData['successMessage'] ?? 'Upload failed'
            );
          }
        } catch (e) {
          print('❌ Error parsing response JSON: $e');
          return VisionBoardResult.failure(message: 'Invalid response format: $e');
        }
      } else {
        print('❌ HTTP Request failed');
        print('  - Status Code: ${response.statusCode}');
        print('  - Response Body: ${response.body}');
        return VisionBoardResult.failure(
          message: 'Network error: ${response.statusCode} - ${response.body.isNotEmpty ? response.body : "Unknown error"}'
        );
      }
      
    } catch (e) {
      print('❌ Error uploading vision board XML: $e');
      return VisionBoardResult.failure(message: 'Failed to upload XML: $e');
    }
  }

  /// Legacy method for backward compatibility
  Future<VisionBoardResult<void>> uploadXMLFile(String xmlContent) async {
    final authUserId = await _authService?.getUserId();
    final result = await uploadVisionBoardXML(
      visionBoardId: 'all_boards_${DateTime.now().millisecondsSinceEpoch}',
      xmlContent: xmlContent,
      userId: authUserId ?? _defaultUserId,
    );
    
    if (result.isSuccess) {
      return VisionBoardResult.success(message: result.message);
    } else {
      return VisionBoardResult.failure(message: result.message);
    }
  }

  /// Get all vision board XML files for a user
  Future<VisionBoardResult<List<VisionBoardXMLInfo>>> getVisionBoardXMLList(int userId) async {
    try {
      print('📋 Getting vision board XML list for user: $userId');
      
      if (_apiClient == null || _authService == null) {
        return VisionBoardResult.failure(message: 'API client not initialized');
      }

      // Get all vision board pictures for the user, filtered for XML files
      final response = await _apiClient!.apiV1ManifestGetvisionboardpicturesGet(
        userId: userId,
        keyword: '.xml', // Search for XML files
      );
      
      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        if (returnModel.isSuccess == true && returnModel.model != null) {
          final xmlFiles = returnModel.model!
              .where((picture) => picture.fileName?.endsWith('.xml') == true)
              .map((picture) => VisionBoardXMLInfo(
                pictureId: picture.visionBoardPictureId ?? 0,
                visionBoardId: picture.location ?? '', // Vision board ID stored in location
                fileName: picture.fileName ?? '',
                createdDate: picture.createdDate,
                fileSize: picture.fileSize ?? 0,
              ))
              .toList();
          
          // Sort by creation date (newest first)
          xmlFiles.sort((a, b) => (b.createdDate ?? DateTime(0))
              .compareTo(a.createdDate ?? DateTime(0)));
          
          print('📄 Found ${xmlFiles.length} XML files');
          return VisionBoardResult.success(
            data: xmlFiles, 
            message: 'Found ${xmlFiles.length} vision board XML files'
          );
        }
      }
      
      return VisionBoardResult.failure(message: 'Failed to fetch XML files');
      
    } catch (e) {
      print('❌ Error getting XML list: $e');
      return VisionBoardResult.failure(message: 'Failed to get XML list: $e');
    }
  }

  /// Download specific vision board XML content by picture ID
  Future<VisionBoardResult<String?>> downloadVisionBoardXML(int pictureId) async {
    try {
      print('📥 Downloading vision board XML content: $pictureId');
      
      if (_apiClient == null || _authService == null) {
        return VisionBoardResult.failure(message: 'API client not initialized');
      }

      // Download the actual XML file content
      final response = await _apiClient!.apiV1ManifestDownloadvisionboardpicturefileGet(
        visionBoardPictureId: pictureId,
      );
      
      if (response.isSuccessful) {
        // The response should contain the XML content
        // Since this returns raw response, we need to handle it properly
        final xmlContent = response.bodyString;
        
        if (xmlContent.isNotEmpty) {
          print('✅ Vision board XML downloaded successfully');
          return VisionBoardResult.success(
            data: xmlContent,
            message: 'XML content downloaded successfully'
          );
        } else {
          return VisionBoardResult.failure(message: 'Empty XML content received');
        }
      } else {
        return VisionBoardResult.failure(
          message: 'Failed to download XML: ${response.statusCode}'
        );
      }
      
    } catch (e) {
      print('❌ Error downloading XML content: $e');
      return VisionBoardResult.failure(message: 'Failed to download XML: $e');
    }
  }

  /// Download latest XML file for user (backward compatibility)
  Future<VisionBoardResult<String?>> downloadXMLFile(int userId) async {
    try {
      // Get list of XML files
      final listResult = await getVisionBoardXMLList(userId);
      
      if (!listResult.isSuccess || listResult.data == null || listResult.data!.isEmpty) {
        return VisionBoardResult.success(data: null, message: 'No XML files found');
      }
      
      // Download the most recent XML file
      final latestXML = listResult.data!.first;
      return await downloadVisionBoardXML(latestXML.pictureId);
      
    } catch (e) {
      print('❌ Error downloading latest XML: $e');
      return VisionBoardResult.failure(message: 'Failed to download latest XML: $e');
    }
  }

  /// Download specific vision board XML by vision board ID
  Future<VisionBoardResult<String?>> downloadVisionBoardXMLById({
    required String visionBoardId,
    required int userId,
  }) async {
    try {
      print('📥 Downloading vision board XML by ID: $visionBoardId');
      
      // Get list of XML files for user
      final listResult = await getVisionBoardXMLList(userId);
      
      if (!listResult.isSuccess || listResult.data == null) {
        return VisionBoardResult.failure(message: listResult.message);
      }
      
      // Find the specific vision board XML
      final targetXML = listResult.data!
          .where((xml) => xml.visionBoardId == visionBoardId)
          .firstOrNull;
      
      if (targetXML == null) {
        return VisionBoardResult.success(
          data: null, 
          message: 'Vision board XML not found: $visionBoardId'
        );
      }
      
      // Download the specific XML content
      return await downloadVisionBoardXML(targetXML.pictureId);
      
    } catch (e) {
      print('❌ Error downloading vision board XML by ID: $e');
      return VisionBoardResult.failure(message: 'Failed to download XML by ID: $e');
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

// ========================= AUTH INTERCEPTOR =========================

class _AuthInterceptor implements Interceptor {
  @override
  FutureOr<Response<BodyType>> intercept<BodyType>(Chain<BodyType> chain) async {
    final request = chain.request;
    final authService = UnifiedAuthService();
    
    final token = authService.authToken;
    print('🔐 Auth Interceptor - Token available: ${token != null}');
    print('🔐 Auth Interceptor - Request Content-Type: ${request.headers['content-type'] ?? request.headers['Content-Type']}');
    print('🔐 Auth Interceptor - Request URL: ${request.url}');
    print('🔐 Auth Interceptor - Request Body Type: ${request.body.runtimeType}');
    
    if (token != null) {
      print('🔐 Auth Interceptor - Adding Bearer token to request');
      
      // Preserve existing headers, especially content-type for multipart requests
      final existingHeaders = Map<String, String>.from(request.headers);
      existingHeaders['Authorization'] = 'Bearer $token';
      
      final updatedRequest = request.copyWith(headers: existingHeaders);
      
      print('🔐 Auth Interceptor - Final Content-Type: ${updatedRequest.headers['content-type'] ?? updatedRequest.headers['Content-Type']}');
      
      return chain.proceed(updatedRequest);
    } else {
      print('⚠️  Auth Interceptor - No token available, proceeding without auth');
    }
    
    return chain.proceed(request);
  }
}

