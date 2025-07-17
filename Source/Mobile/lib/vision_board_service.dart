import 'dart:async';
import 'package:manifestation_app/api/generated/code/manifestation.swagger.dart';
import 'package:manifestation_app/api_config.dart';
import 'package:manifestation_app/session_service.dart';
import 'package:chopper/chopper.dart';

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
      final createModel = VisionBoardSessionCreateModel(
        visionBoardSessionDate: DateTime.now(),
        visionBoardSessionTimeSpentInSeconds: 0,
        userId: userId,
        addedToFeed: addToFeed,
        addedToFeedDate: addToFeed ? DateTime.now() : null,
      );

      final response = await _apiClient.apiV1ManifestCreatevisionboardsessionPost(
        body: createModel,
      );
      
      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        if (returnModel.isSuccess == true && returnModel.model != null) {
          return VisionBoardResult.success(
            data: _mapToVisionBoardSession(returnModel.model!),
          );
        }
        
        return VisionBoardResult.error(
          message: returnModel.errorHolder?.friendlyMessage ?? 'Failed to create vision board',
        );
      }
      
      return VisionBoardResult.error(message: 'Failed to create vision board');
    } catch (e) {
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
      final updateModel = VisionBoardSessionUpdateModel(
        visionBoardSessionId: sessionId,
        visionBoardSessionTimeSpentInSeconds: timeSpentInSeconds,
        addedToFeed: addToFeed,
        addedToFeedDate: addToFeed == true ? DateTime.now() : null,
      );

      final response = await _apiClient.apiV1ManifestUpdatevisionboardsessionPost(
        body: updateModel,
      );
      
      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        if (returnModel.isSuccess == true && returnModel.model != null) {
          return VisionBoardResult.success(
            data: _mapToVisionBoardSession(returnModel.model!),
          );
        }
        
        return VisionBoardResult.error(
          message: returnModel.errorHolder?.friendlyMessage ?? 'Failed to update vision board',
        );
      }
      
      return VisionBoardResult.error(message: 'Failed to update vision board');
    } catch (e) {
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