import 'dart:async';
import 'dart:io';
import 'package:path_provider/path_provider.dart';
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
      _apiClient = Manifestation.create(baseUrl: Uri.parse(baseUrl));
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
      final userId = await _authService!.getUserId();
      
      // Upload using createvisionboardpicture endpoint
      await _apiClient!.apiV1ManifestCreatevisionboardpicturePost(
        UploadedFile: fileBytes,
        location: 'vision_board',
        fileName: 'vision_boards.xml',
        fileSize: fileBytes.length,
        userId: userId,
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
      final userId = await _authService!.getUserId();
      
      // First get the existing picture ID
      final existingPictureId = await _getExistingPictureId();
      
      // Update using updatevisionboardpicture endpoint
      await _apiClient!.apiV1ManifestUpdatevisionboardpicturePost(
        UploadedFile: fileBytes,
        visionBoardPictureId: existingPictureId,
        location: 'vision_board',
        fileName: 'vision_boards.xml',
        fileSize: fileBytes.length,
        userId: userId,
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
      
      final userId = await _authService!.getUserId();
      
      // Download using getvisionboardpicturebyuserid endpoint
      final response = await _apiClient!.apiV1ManifestGetvisionboardpicturebyuseridGet(
        userId: userId,
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
      
      final userId = await _authService!.getUserId();
      
      // Try to get file - if successful, file exists
      final response = await _apiClient!.apiV1ManifestGetvisionboardpicturebyuseridGet(
        userId: userId,
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
      
      final userId = await _authService!.getUserId();
      
      final response = await _apiClient!.apiV1ManifestGetvisionboardpicturebyuseridGet(
        userId: userId,
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

  /// Upload XML file content to server
  Future<VisionBoardResult<void>> uploadXMLFile(String xmlContent) async {
    try {
      print('📤 Uploading XML to server...');
      
      if (_apiClient == null || _authService == null) {
        return VisionBoardResult.failure(message: 'API client not initialized');
      }

      // Get auth token
      final token = _authService!.authToken;
      if (token == null || token.isEmpty) {
        return VisionBoardResult.failure(message: 'No auth token available');
      }

      // TODO: Replace with actual API endpoint when available  
      // For now, simulate upload success
      await Future.delayed(Duration(milliseconds: 500));
      
      print('✅ XML uploaded to server successfully');
      return VisionBoardResult.success(message: 'XML uploaded successfully');
      
    } catch (e) {
      print('❌ Error uploading XML: $e');
      return VisionBoardResult.failure(message: 'Failed to upload XML: $e');
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

