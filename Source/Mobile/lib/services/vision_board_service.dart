import 'dart:async';
import 'package:manifestation_app/api/generated/code/manifestation.swagger.dart';
import 'package:manifestation_app/services/api_config.dart';
import 'package:chopper/chopper.dart';
import 'package:manifestation_app/services/session_service.dart';

class VisionBoardService {
  final Manifestation _apiClient;
  
  VisionBoardService({String? baseUrl}) : _apiClient = Manifestation.create(
    baseUrl: Uri.parse(baseUrl ?? ApiConfig.baseUrl),
    interceptors: [
      _BasicAuthInterceptor(),
      _SessionTokenInterceptor(),
    ],
  );

  /// Get all vision board sessions for the user
  Future<VisionBoardResult<List<VisionBoardSession>>> getUserVisionBoards({
    int? userId,
  }) async {
    try {
      final response = await _apiClient.apiV1ManifestGetvisionboardsessionsGet(
        userId: userId,
      );
      
      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        if (returnModel.isSuccess == true && returnModel.model != null) {
          final sessions = returnModel.model!.map((session) => 
            _mapToVisionBoardSession(session)
          ).toList();
          
          return VisionBoardResult.success(data: sessions);
        }
        
        return VisionBoardResult.error(
          message: returnModel.errorHolder?.friendlyMessage ?? 'Failed to load vision boards',
        );
      }
      
      return VisionBoardResult.error(message: 'Failed to load vision boards');
    } catch (e) {
      return VisionBoardResult.error(message: 'Network error: ${e.toString()}');
    }
  }

  /// Create a new vision board session
  Future<VisionBoardResult<VisionBoardSession>> createVisionBoard({
    required String title,
    int? userId,
    bool addToFeed = false,
  }) async {
    try {
      print('Creating vision board session - Title: $title, UserId: $userId');
      
      final createModel = VisionBoardSessionCreateModel(
        visionBoardSessionDate: DateTime.now(),
        visionBoardSessionTimeSpentInSeconds: 0,
        userId: userId,
        addedToFeed: addToFeed,
        addedToFeedDate: addToFeed ? DateTime.now() : null,
      );

      print('Making API call to create vision board session...');
      
      final response = await _apiClient.apiV1ManifestCreatevisionboardsessionPost(
        body: createModel,
      );
      
      print('Vision board creation API response: ${response.statusCode} - Success: ${response.isSuccessful}');
      
      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        print('Vision board creation response success: ${returnModel.isSuccess}');
        
        if (returnModel.isSuccess == true && returnModel.model != null) {
          final session = _mapToVisionBoardSession(returnModel.model!);
          print('Vision board session created successfully with ID: ${session.id}');
          
          return VisionBoardResult.success(data: session);
        }
        
        final errorMessage = returnModel.errorHolder?.friendlyMessage ?? 'Failed to create vision board';
        print('Vision board creation failed: $errorMessage');
        
        return VisionBoardResult.error(message: errorMessage);
      }
      
      print('Vision board creation API call failed');
      return VisionBoardResult.error(message: 'Failed to create vision board');
    } catch (e) {
      print('Vision board creation error: $e');
      return VisionBoardResult.error(message: 'Network error: ${e.toString()}');
    }
  }

  /// Update an existing vision board session
  Future<VisionBoardResult<VisionBoardSession>> updateVisionBoard({
    required int sessionId,
    int? timeSpentInSeconds,
    bool? addToFeed,
  }) async {
    try {
      print('Updating vision board session - ID: $sessionId, Time: ${timeSpentInSeconds}s');
      
      final updateModel = VisionBoardSessionUpdateModel(
        visionBoardSessionId: sessionId,
        visionBoardSessionTimeSpentInSeconds: timeSpentInSeconds,
        addedToFeed: addToFeed,
        addedToFeedDate: addToFeed == true ? DateTime.now() : null,
      );

      print('Making API call to update vision board session...');
      
      final response = await _apiClient.apiV1ManifestUpdatevisionboardsessionPost(
        body: updateModel,
      );
      
      print('Vision board update API response: ${response.statusCode} - Success: ${response.isSuccessful}');
      
      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        print('Vision board update response success: ${returnModel.isSuccess}');
        
        if (returnModel.isSuccess == true && returnModel.model != null) {
          final session = _mapToVisionBoardSession(returnModel.model!);
          print('Vision board session updated successfully: ${timeSpentInSeconds}s');
          
          return VisionBoardResult.success(data: session);
        }
        
        final errorMessage = returnModel.errorHolder?.friendlyMessage ?? 'Failed to update vision board';
        print('Vision board update failed: $errorMessage');
        
        return VisionBoardResult.error(message: errorMessage);
      }
      
      print('Vision board update API call failed');
      return VisionBoardResult.error(message: 'Failed to update vision board');
    } catch (e) {
      print('Vision board update error: $e');
      return VisionBoardResult.error(message: 'Network error: ${e.toString()}');
    }
  }

  /// Delete a vision board session
  Future<VisionBoardResult<bool>> deleteVisionBoard({required int sessionId}) async {
    try {
      final response = await _apiClient.apiV1ManifestMarkvisionboardsessionasdeletedDelete(
        id: sessionId,
      );
      
      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        if (returnModel.isSuccess == true) {
          return VisionBoardResult.success(data: true);
        }
        
        return VisionBoardResult.error(
          message: returnModel.errorHolder?.friendlyMessage ?? 'Failed to delete vision board',
        );
      }
      
      return VisionBoardResult.error(message: 'Failed to delete vision board');
    } catch (e) {
      return VisionBoardResult.error(message: 'Network error: ${e.toString()}');
    }
  }

  /// Get vision board statistics for the user
  Future<VisionBoardResult<VisionBoardStats>> getVisionBoardStats({int? userId}) async {
    try {
      print('Getting vision board stats for user: $userId');
      
      final result = await getUserVisionBoards(userId: userId);
      
      if (!result.isSuccess || result.data == null) {
        print('Failed to get vision boards: ${result.message}');
        return VisionBoardResult.success(data: VisionBoardStats.empty());
      }
      
      final sessions = result.data!;
      print('Found ${sessions.length} vision board sessions for stats');
      
      if (sessions.isEmpty) {
        return VisionBoardResult.success(data: VisionBoardStats.empty());
      }

      final totalSessions = sessions.length;
      final totalTimeInSeconds = sessions.fold<int>(
        0, 
        (sum, session) => sum + session.timeSpentInSeconds,
      );
      final averageSessionTime = totalTimeInSeconds / totalSessions;

      // Calculate this week/month sessions
      final now = DateTime.now();
      final weekAgo = now.subtract(Duration(days: 7));
      final monthAgo = DateTime(now.year, now.month - 1, now.day);

      final thisWeekSessions = sessions.where((s) => s.createdAt.isAfter(weekAgo)).length;
      final thisMonthSessions = sessions.where((s) => s.createdAt.isAfter(monthAgo)).length;

      // Count total images (placeholder - would need API call to get session pictures)
      final totalImages = sessions.length * 5; // Estimated

      final stats = VisionBoardStats(
        totalSessions: totalSessions,
        totalTimeInSeconds: totalTimeInSeconds,
        averageSessionTime: averageSessionTime.round(),
        thisWeekSessions: thisWeekSessions,
        thisMonthSessions: thisMonthSessions,
        totalImages: totalImages,
      );
      
      print('Vision board stats calculated: ${stats.totalSessions} sessions, ${stats.totalTimeInSeconds}s total');
      
      return VisionBoardResult.success(data: stats);
    } catch (e) {
      print('Error getting vision board stats: $e');
      return VisionBoardResult.error(message: 'Failed to calculate stats: ${e.toString()}');
    }
  }

  /// Add an element/picture to a vision board session
  Future<VisionBoardResult<VisionBoardPicture>> addVisionBoardElement({
    required int sessionId,
    required int visionBoardPictureId,
    int? timeSpentInSeconds,
    int? userId,
  }) async {
    try {
      final createModel = VisionBoardSessionPictureCreateModel(
        visionBoardSessionId: sessionId,
        visionBoardSessionPictureDate: DateTime.now(),
        visionBoardPictureId: visionBoardPictureId,
        visionBoardSessionPictureTimeSpentInSeconds: timeSpentInSeconds ?? 0,
        userId: userId,
      );

      final response = await _apiClient.apiV1ManifestCreatevisionboardsessionpicturePost(
        body: createModel,
      );
      
      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        if (returnModel.isSuccess == true && returnModel.model != null) {
          return VisionBoardResult.success(
            data: _mapToVisionBoardPicture(returnModel.model!),
          );
        }
        
        return VisionBoardResult.error(
          message: returnModel.errorHolder?.friendlyMessage ?? 'Failed to add element',
        );
      }
      
      return VisionBoardResult.error(message: 'Failed to add element');
    } catch (e) {
      return VisionBoardResult.error(message: 'Network error: ${e.toString()}');
    }
  }

  /// Map API model to local model
  VisionBoardSession _mapToVisionBoardSession(VisionBoardSessionModel apiSession) {
    return VisionBoardSession(
      id: apiSession.visionBoardSessionId?.toString() ?? '',
      title: 'Vision Board ${apiSession.visionBoardSessionId}', // API doesn't have title
      createdAt: apiSession.visionBoardSessionDate ?? DateTime.now(),
      timeSpentInSeconds: apiSession.visionBoardSessionTimeSpentInSeconds ?? 0,
      addedToFeed: apiSession.addedToFeed ?? false,
      userId: apiSession.userId,
    );
  }

  /// Map API picture model to local model
  VisionBoardPicture _mapToVisionBoardPicture(VisionBoardSessionPictureModel apiPicture) {
    return VisionBoardPicture(
      id: apiPicture.visionBoardSessionPictureId?.toString() ?? '',
      sessionId: apiPicture.visionBoardSessionId?.toString() ?? '',
      imageUrl: '', // You'll need to get this from the API model
      positionX: 0.0, // You'll need to map these from the API model
      positionY: 0.0,
      width: 100.0,
      height: 100.0,
      rotation: 0.0,
      zIndex: 0,
    );
  }
}

/// Local models for vision board data
class VisionBoardSession {
  final String id;
  final String title;
  final DateTime createdAt;
  final int timeSpentInSeconds;
  final bool addedToFeed;
  final int? userId;

  const VisionBoardSession({
    required this.id,
    required this.title,
    required this.createdAt,
    this.timeSpentInSeconds = 0,
    this.addedToFeed = false,
    this.userId,
  });
}

class VisionBoardPicture {
  final String id;
  final String sessionId;
  final String imageUrl;
  final double positionX;
  final double positionY;
  final double width;
  final double height;
  final double rotation;
  final int zIndex;

  const VisionBoardPicture({
    required this.id,
    required this.sessionId,
    required this.imageUrl,
    required this.positionX,
    required this.positionY,
    required this.width,
    required this.height,
    this.rotation = 0.0,
    this.zIndex = 0,
  });
}

/// Vision board statistics model
class VisionBoardStats {
  final int totalSessions;
  final int totalTimeInSeconds;
  final int averageSessionTime;
  final int thisWeekSessions;
  final int thisMonthSessions;
  final int totalImages;

  const VisionBoardStats({
    required this.totalSessions,
    required this.totalTimeInSeconds,
    required this.averageSessionTime,
    required this.thisWeekSessions,
    required this.thisMonthSessions,
    required this.totalImages,
  });

  factory VisionBoardStats.empty() {
    return VisionBoardStats(
      totalSessions: 0,
      totalTimeInSeconds: 0,
      averageSessionTime: 0,
      thisWeekSessions: 0,
      thisMonthSessions: 0,
      totalImages: 0,
    );
  }

  String get totalTimeFormatted {
    final hours = totalTimeInSeconds ~/ 3600;
    final minutes = (totalTimeInSeconds % 3600) ~/ 60;
    
    if (hours > 0) {
      return '${hours}h ${minutes}m';
    } else {
      return '${minutes}m';
    }
  }

  String get averageTimeFormatted {
    final minutes = averageSessionTime ~/ 60;
    final seconds = averageSessionTime % 60;
    
    if (minutes > 0) {
      return '${minutes}m ${seconds}s';
    } else {
      return '${seconds}s';
    }
  }
}

/// Result wrapper for vision board operations
class VisionBoardResult<T> {
  final bool isSuccess;
  final T? data;
  final String? message;

  const VisionBoardResult._({
    required this.isSuccess,
    this.data,
    this.message,
  });

  factory VisionBoardResult.success({T? data, String? message}) {
    return VisionBoardResult._(
      isSuccess: true,
      data: data,
      message: message,
    );
  }

  factory VisionBoardResult.error({required String message}) {
    return VisionBoardResult._(
      isSuccess: false,
      message: message,
    );
  }
}

// Reuse the auth interceptors from AuthService
class _BasicAuthInterceptor implements Interceptor {
  static const String _basicAuthCredentials = 'U3BlY2lhbFVzZXI6U3BlY2lhbFBhc3N3b3JkMQ==';
  
  @override
  FutureOr<Response<BodyType>> intercept<BodyType>(Chain<BodyType> chain) async {
    final request = chain.request;
    final authenticatedRequest = request.copyWith(
      headers: Map.from(request.headers)
        ..addAll({
          'Authorization': 'Basic $_basicAuthCredentials',
        }),
    );
    
    return chain.proceed(authenticatedRequest);
  }
}

class _SessionTokenInterceptor implements Interceptor {
  @override
  FutureOr<Response<BodyType>> intercept<BodyType>(Chain<BodyType> chain) async {
    final request = chain.request;
    
    // Get session token
    final sessionToken = await SessionService.instance.getSessionToken();
    
    final headers = Map<String, String>.from(request.headers);
    
    // Add session token if available
    if (sessionToken != null) {
      headers['X-Session-Token'] = sessionToken;
      // Also try Authorization Bearer format as backup
      headers['Authorization'] = 'Bearer $sessionToken';
    }
    
    final authenticatedRequest = request.copyWith(headers: headers);
    return chain.proceed(authenticatedRequest);
  }
}