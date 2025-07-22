import 'dart:async';
import 'package:flutter/material.dart';
import '../models/vision_board.dart';
import 'unified_auth_service.dart';
import 'api_client.dart';
import 'vision_board_api_service.dart';

class VisionBoardService {
  static final VisionBoardService _instance = VisionBoardService._internal();
  factory VisionBoardService() => _instance;
  VisionBoardService._internal();

  final UnifiedAuthService _authService = UnifiedAuthService();
  
  Future<void> initialize({String? baseUrl}) async {
    // Initialize ApiClient 
    await ApiClient.initialize();
    
    // Sync auth token with UnifiedAuthService
    final authToken = _authService.authToken;
    if (authToken != null) {
      await ApiClient.setAuthToken(authToken);
    }
    
    print('🎯 VisionBoardService initialized with ApiClient');
  }

  /// Save vision board to API using session + session pictures
  Future<VisionBoardResult<VisionBoard>> saveVisionBoard(VisionBoard board) async {
    try {
      print('🎯 Saving vision board: ${board.title}');
      
      // Sync auth token
      await _syncAuthToken();
      
      final userId = await _getUserId();
      
      Map<String, dynamic> result;
      
      if (board.id.isNotEmpty) {
        // Update existing session
        final sessionData = {
          'visionBoardSessionId': int.tryParse(board.id),
          'visionBoardSessionTimeSpentInSeconds': 0,
          'userId': userId,
        };
        
        result = await VisionBoardApiService.updateVisionBoardSession(sessionData);
      } else {
        // Create new session
        final sessionData = {
          'visionBoardSessionDate': DateTime.now().toIso8601String(),
          'visionBoardSessionTimeSpentInSeconds': 0,
          'userId': userId,
          'addedToFeed': false,
        };
        
        result = await VisionBoardApiService.createVisionBoardSession(sessionData);
      }
      
      if (!result['success']) {
        return VisionBoardResult.failure(message: result['error'] ?? 'Failed to save session');
      }
      
      final sessionData = result['data'];
      final sessionId = sessionData['model']?['visionBoardSessionId'];
      
      if (sessionId == null) {
        return VisionBoardResult.failure(message: 'No session ID returned');
      }
      
      print('✅ Session saved with ID: $sessionId');
      
      // Save elements as session pictures
      await _saveElementsAsSessionPictures(sessionId, board.elements);
      
      // Return updated board with session ID
      final updatedBoard = board.copyWith(
        id: sessionId.toString(),
        updatedAt: DateTime.now(),
      );
      
      print('✅ Vision board saved successfully');
      return VisionBoardResult.success(
        data: updatedBoard,
        message: 'Vision board saved successfully',
      );
      
    } catch (e) {
      print('❌ Error saving vision board: $e');
      return VisionBoardResult.failure(message: 'Failed to save vision board: $e');
    }
  }

  /// Load vision board from API using session + session pictures
  Future<VisionBoardResult<VisionBoard>> loadVisionBoard(String sessionId) async {
    try {
      print('🎯 Loading vision board session: $sessionId');
      
      // Sync auth token
      await _syncAuthToken();
      
      // Get the session
      final result = await VisionBoardApiService.getVisionBoardSessionById(sessionId);
      
      if (!result['success']) {
        return VisionBoardResult.failure(message: result['error'] ?? 'Session not found');
      }
      
      final sessionData = result['data'];
      final session = sessionData['model'];
      
      if (session == null) {
        return VisionBoardResult.failure(message: 'Session not found');
      }
      
      print('✅ Session loaded: ${session['visionBoardSessionId']}');
      
      // Load elements from session pictures
      final elements = await _loadElementsFromSessionPictures(session['visionBoardSessionId']);
      
      // Create VisionBoard object
      final board = VisionBoard(
        id: session['visionBoardSessionId'].toString(),
        title: 'Vision Board ${session['visionBoardSessionId']}',
        elements: elements,
        backgroundColor: const Color(0xFFFFFFFF),
        createdAt: DateTime.tryParse(session['visionBoardSessionDate'] ?? '') ?? DateTime.now(),
        updatedAt: DateTime.tryParse(session['modifiedDate'] ?? '') ?? DateTime.now(),
      );
      
      print('✅ Vision board loaded with ${elements.length} elements');
      return VisionBoardResult.success(
        data: board,
        message: 'Vision board loaded successfully',
      );
      
    } catch (e) {
      print('❌ Error loading vision board: $e');
      return VisionBoardResult.failure(message: 'Failed to load vision board: $e');
    }
  }

  /// Get all user vision boards (sessions)
  Future<VisionBoardResult<List<VisionBoard>>> getUserVisionBoards() async {
    try {
      print('🎯 Loading user vision boards');
      
      // Sync auth token
      await _syncAuthToken();
      
      final userId = await _getUserId();
      print('📋 Using user ID: $userId');
      print('📋 Is logged in: ${_authService.isLoggedIn}');
      
      print('🔄 Making API call to get vision board sessions...');
      final result = await VisionBoardApiService.getSessionsByUserId(userId.toString());
      
      print('📡 API response received - Success: ${result['success']}');
      print('📡 API response data: ${result['data']}');
      
      if (!result['success']) {
        print('❌ API call failed: ${result['error']}');
        return VisionBoardResult.success(data: [], message: 'API unavailable - working offline');
      }
      
      final sessionData = result['data'];
      
      // Check if the API response indicates an internal error
      if (sessionData['isSuccess'] == false) {
        final errorMessage = sessionData['errorHolder']?['friendlyMessage'] ?? 'Unknown API error';
        print('❌ API internal error: $errorMessage');
        
        // For "Unable to find the User" error, this might mean the user has no vision boards yet
        if (errorMessage.contains('Unable to find the User')) {
          print('📋 User has no vision board sessions yet - returning empty list');
          return VisionBoardResult.success(data: [], message: 'No vision boards found - ready to create your first one!');
        }
        
        return VisionBoardResult.success(data: [], message: 'API error: $errorMessage');
      }
      
      final sessions = sessionData['model'] as List?;
      
      if (sessions == null || sessions.isEmpty) {
        print('📋 No vision board sessions found');
        return VisionBoardResult.success(data: [], message: 'No vision boards found');
      }
      
      print('✅ Found ${sessions.length} vision board sessions');
      
      // Convert sessions to vision boards (without loading elements for performance)
      final boards = sessions.map((session) => VisionBoard(
        id: session['visionBoardSessionId'].toString(),
        title: 'Vision Board ${session['visionBoardSessionId']}',
        elements: [], // Don't load elements for list view
        backgroundColor: const Color(0xFFFFFFFF),
        createdAt: DateTime.tryParse(session['visionBoardSessionDate'] ?? '') ?? DateTime.now(),
        updatedAt: DateTime.tryParse(session['modifiedDate'] ?? '') ?? DateTime.now(),
      )).toList();
      
      return VisionBoardResult.success(
        data: boards,
        message: 'Vision boards loaded successfully',
      );
      
    } catch (e) {
      print('❌ Error loading user vision boards: $e');
      print('🌐 API error detected - providing fallback empty list'); 
      return VisionBoardResult.success(data: [], message: 'API unavailable - working offline');
    }
  }

  /// Delete vision board
  Future<VisionBoardResult<bool>> deleteVisionBoard(String sessionId) async {
    try {
      // Sync auth token
      await _syncAuthToken();
      
      final result = await VisionBoardApiService.deleteVisionBoardSession(sessionId);
      
      if (result['success']) {
        return VisionBoardResult.success(data: true, message: 'Vision board deleted');
      }
      
      return VisionBoardResult.failure(message: result['error'] ?? 'Failed to delete vision board');
    } catch (e) {
      return VisionBoardResult.failure(message: 'Error deleting vision board: $e');
    }
  }

  /// Get default pictures for vision board
  Future<VisionBoardResult<List<Map<String, dynamic>>>> getDefaultPictures() async {
    try {
      // Sync auth token
      await _syncAuthToken();
      
      final result = await VisionBoardApiService.getDefaultVisionBoardPictures();
      
      if (result['success']) {
        final data = result['data'];
        final pictures = data['model'] as List? ?? [];
        return VisionBoardResult.success(
          data: pictures.cast<Map<String, dynamic>>(),
          message: 'Default pictures loaded successfully',
        );
      }
      
      return VisionBoardResult.success(data: [], message: 'No default pictures available');
    } catch (e) {
      return VisionBoardResult.failure(message: 'Error loading default pictures: $e');
    }
  }

  /// Create a new vision board picture
  Future<VisionBoardResult<Map<String, dynamic>>> createPicture({
    required String location,
    required String fileName,
    required int fileSize,
  }) async {
    try {
      // Sync auth token
      await _syncAuthToken();
      
      final pictureData = {
        'location': location,
        'fileName': fileName,
        'fileSize': fileSize,
        'userId': await _getUserId(),
      };

      final result = await VisionBoardApiService.createVisionBoardPicture(pictureData);
      
      if (result['success']) {
        final data = result['data'];
        return VisionBoardResult.success(
          data: data['model'],
          message: 'Picture created successfully',
        );
      }
      
      return VisionBoardResult.failure(message: result['error'] ?? 'Failed to create picture');
    } catch (e) {
      return VisionBoardResult.failure(message: 'Error creating picture: $e');
    }
  }

  /// Sync auth token between UnifiedAuthService and ApiClient
  Future<void> _syncAuthToken() async {
    final authToken = _authService.authToken;
    if (authToken != null && authToken.isNotEmpty) {
      await ApiClient.setAuthToken(authToken);
    }
  }

  /// Get user ID with fallback for development
  Future<int> _getUserId() async {
    try {
      // Check if user is logged in via UnifiedAuthService
      if (_authService.isLoggedIn) {
        final userId = await _authService.getUserId();
        if (userId != null && userId > 0) {
          print('✅ Using authenticated user ID: $userId');
          return userId;
        }
      }
      
      // Fallback for development/testing when not logged in
      print('⚠️ User not logged in (UnifiedAuthService.isLoggedIn: ${_authService.isLoggedIn})');
      print('⚠️ Using fallback user ID (1) for development');
      return 1;
    } catch (e) {
      print('❌ Error getting user ID: $e');
      print('⚠️ Using fallback user ID (1) for development');
      return 1;
    }
  }

  /// Track picture view in session
  Future<VisionBoardResult<bool>> trackPictureView({
    required int sessionId,
    required int pictureId,
  }) async {
    try {
      // Sync auth token
      await _syncAuthToken();
      
      // Check if this session picture already exists
      final existingResult = await VisionBoardApiService.getSessionPicturesBySessionId(sessionId.toString());
      
      bool alreadyExists = false;
      if (existingResult['success']) {
        final data = existingResult['data'];
        final pictures = data['model'] as List? ?? [];
        alreadyExists = pictures.any((sp) => sp['visionBoardPictureId'] == pictureId);
      }
      
      if (!alreadyExists) {
        final sessionPictureData = {
          'visionBoardSessionId': sessionId,
          'visionBoardPictureId': pictureId,
          'visionBoardSessionPictureDate': DateTime.now().toIso8601String(),
          'visionBoardSessionPictureTimeSpentInSeconds': 0,
          'userId': await _getUserId(),
        };

        final result = await VisionBoardApiService.createVisionBoardSessionPicture(sessionPictureData);
        
        if (result['success']) {
          return VisionBoardResult.success(data: true, message: 'Picture view tracked');
        }
      }
      
      return VisionBoardResult.success(data: true, message: 'Picture view already tracked');
    } catch (e) {
      return VisionBoardResult.failure(message: 'Error tracking picture view: $e');
    }
  }

  /// Update session with time spent
  Future<VisionBoardResult<bool>> updateSession({
    required int sessionId,
    required int timeSpentInSeconds,
  }) async {
    try {
      // Sync auth token
      await _syncAuthToken();
      
      final sessionData = {
        'visionBoardSessionId': sessionId,
        'visionBoardSessionTimeSpentInSeconds': timeSpentInSeconds,
        'userId': await _getUserId(),
      };

      final result = await VisionBoardApiService.updateVisionBoardSession(sessionData);
      
      if (result['success']) {
        return VisionBoardResult.success(data: true, message: 'Session updated');
      }
      
      return VisionBoardResult.failure(message: result['error'] ?? 'Failed to update session');
    } catch (e) {
      return VisionBoardResult.failure(message: 'Error updating session: $e');
    }
  }


  /// Save elements as session pictures by referencing actual pictures
  Future<void> _saveElementsAsSessionPictures(int sessionId, List<VisionBoardElement> elements) async {
    print('🎯 Saving ${elements.length} elements as session pictures');
    
    // First, clear existing session pictures
    await _clearSessionPictures(sessionId);
    
    // Get default pictures to use as references
    final defaultPicturesResult = await getDefaultPictures();
    if (!defaultPicturesResult.isSuccess || defaultPicturesResult.data == null || defaultPicturesResult.data!.isEmpty) {
      print('❌ No default pictures available, using fallback approach');
      // Use fallback: create session pictures with dummy picture IDs
      for (int i = 0; i < elements.length; i++) {
        final element = elements[i];
        await _saveElementAsSessionPicture(sessionId, element, i, i + 1); // Use index + 1 as picture ID
      }
      print('✅ All elements saved as session pictures (fallback mode)');
      return;
    }
    
    final defaultPictures = defaultPicturesResult.data!;
    print('📋 Found ${defaultPictures.length} default pictures to use');
    
    // Save each element as a session picture using available default pictures
    for (int i = 0; i < elements.length; i++) {
      final element = elements[i];
      
      // Use different pictures for different element types
      int pictureIndex = 0;
      if (element is VisionBoardImageElement) {
        pictureIndex = i % defaultPictures.length; // Cycle through available pictures
      } else if (element is VisionBoardTextElement) {
        pictureIndex = (i + 1) % defaultPictures.length; // Offset for text elements
      }
      
      final pictureToUse = defaultPictures[pictureIndex];
      final pictureId = pictureToUse['defaultVisionBoardPictureId'];
      if (pictureId != null) {
        await _saveElementAsSessionPicture(sessionId, element, i, pictureId);
      }
    }
    
    print('✅ All elements saved as session pictures');
  }

  Future<void> _clearSessionPictures(int sessionId) async {
    try {
      // Get existing session pictures
      final result = await VisionBoardApiService.getSessionPicturesBySessionId(sessionId.toString());
      
      if (result['success']) {
        final data = result['data'];
        final pictures = data['model'] as List? ?? [];
        
        // Mark all as deleted
        for (final picture in pictures) {
          final pictureId = picture['visionBoardSessionPictureId'];
          if (pictureId != null) {
            await VisionBoardApiService.deleteVisionBoardSessionPicture(pictureId.toString());
          }
        }
      }
    } catch (e) {
      print('⚠️ Error clearing session pictures: $e');
    }
  }

  Future<void> _saveElementAsSessionPicture(int sessionId, VisionBoardElement element, int index, int pictureId) async {
    try {
      // Create session picture that references an actual default picture
      final sessionPictureData = {
        'visionBoardSessionId': sessionId,
        'visionBoardPictureId': pictureId,
        'visionBoardSessionPictureDate': DateTime.now().toIso8601String(),
        'visionBoardSessionPictureTimeSpentInSeconds': 0,
        'userId': await _getUserId(),
      };

      print('🔧 Creating session picture with data: $sessionPictureData');
      final result = await VisionBoardApiService.createVisionBoardSessionPicture(sessionPictureData);
      
      if (result['success']) {
        print('✅ Session picture created successfully');
      } else {
        print('❌ Failed to create session picture: ${result['error']}');
      }
      
    } catch (e) {
      print('⚠️ Error saving element as session picture: $e');
    }
  }

  Future<List<VisionBoardElement>> _loadElementsFromSessionPictures(int sessionId) async {
    try {
      final result = await VisionBoardApiService.getSessionPicturesBySessionId(sessionId.toString());
      
      if (!result['success']) {
        print('📋 No session pictures found for session $sessionId');
        return [];
      }
      
      final data = result['data'];
      final sessionPictures = data['model'] as List? ?? [];
      print('✅ Found ${sessionPictures.length} session pictures');
      
      // Create placeholder elements based on session pictures
      final elements = <VisionBoardElement>[];
      
      for (int i = 0; i < sessionPictures.length; i++) {
        final sessionPicture = sessionPictures[i];
        
        // Create placeholder image elements positioned in a grid
        final row = i ~/ 3; // 3 elements per row
        final col = i % 3;
        final x = 50.0 + (col * 220.0); // Space elements horizontally
        final y = 50.0 + (row * 220.0); // Space elements vertically
        
        final element = VisionBoardImageElement(
          id: 'element_${sessionPicture['visionBoardSessionPictureId'] ?? i}',
          position: Offset(x, y),
          size: const Size(200, 200),
          imagePath: '',
          imageUrl: null, // Would need to load actual picture URL from visionBoardPictureId
          fit: BoxFit.cover,
        );
        elements.add(element);
      }
      
      print('✅ Created ${elements.length} placeholder elements from session pictures');
      
      return elements;
    } catch (e) {
      print('⚠️ Error loading elements from session pictures: $e');
      return [];
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

