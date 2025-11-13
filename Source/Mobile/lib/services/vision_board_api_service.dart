import 'dart:convert';
import 'dart:io';
import 'package:flutter/material.dart';
import 'package:path_provider/path_provider.dart';
import 'api_client.dart';

class VisionBoardApiService {
  /// Initialize service
  static Future<void> initialize() async {
    print('VisionBoardApiService initialized');
  }

  /// Get all vision board pictures (GET /api/v1/manifest/getvisionboardpictures)
  static Future<Map<String, dynamic>> getAllVisionBoardPictures() async {
    try {
      return await ApiClient.get('/api/v1/manifest/getvisionboardpictures');
    } catch (e) {
      return {
        'success': false,
        'message': 'Failed to retrieve vision board pictures: $e',
        'error': e.toString(),
      };
    }
  }

  /// Get vision board picture by ID (GET /api/v1/manifest/getvisionboardpicture)
  static Future<Map<String, dynamic>> getVisionBoardPictureById(String pictureId) async {
    try {
      return await ApiClient.get('/api/v1/manifest/getvisionboardpicture?id=$pictureId');
    } catch (e) {
      return {
        'success': false,
        'message': 'Failed to retrieve vision board picture: $e',
        'error': e.toString(),
      };
    }
  }

  /// Create vision board picture (POST /api/v1/manifest/createvisionboardpicture)
  static Future<Map<String, dynamic>> createVisionBoardPicture(Map<String, dynamic> pictureData) async {
    try {
      return await ApiClient.post('/api/v1/manifest/createvisionboardpicture', body: pictureData);
    } catch (e) {
      return {
        'success': false,
        'message': 'Failed to create vision board picture: $e',
        'error': e.toString(),
      };
    }
  }

  /// Update vision board picture (POST /api/v1/manifest/updatevisionboardpicture)
  static Future<Map<String, dynamic>> updateVisionBoardPicture(Map<String, dynamic> pictureData) async {
    try {
      return await ApiClient.post('/api/v1/manifest/updatevisionboardpicture', body: pictureData);
    } catch (e) {
      return {
        'success': false,
        'message': 'Failed to update vision board picture: $e',
        'error': e.toString(),
      };
    }
  }

  /// Delete vision board picture (POST /api/v1/manifest/markvisionboardpictureasdeleted)
  static Future<Map<String, dynamic>> deleteVisionBoardPicture(String pictureId) async {
    try {
      final body = {'id': pictureId};
      return await ApiClient.post('/api/v1/manifest/markvisionboardpictureasdeleted', body: body);
    } catch (e) {
      return {
        'success': false,
        'message': 'Failed to delete vision board picture: $e',
        'error': e.toString(),
      };
    }
  }

  /// Get all vision board sessions (GET /api/v1/manifest/getvisionboardsessions)
  static Future<Map<String, dynamic>> getAllVisionBoardSessions() async {
    try {
      return await ApiClient.get('/api/v1/manifest/getvisionboardsessions');
    } catch (e) {
      return {
        'success': false,
        'message': 'Failed to retrieve vision board sessions: $e',
        'error': e.toString(),
      };
    }
  }

  /// Get vision board session by ID (GET /api/v1/manifest/getvisionboardsession)
  static Future<Map<String, dynamic>> getVisionBoardSessionById(String sessionId) async {
    try {
      return await ApiClient.get('/api/v1/manifest/getvisionboardsession?id=$sessionId');
    } catch (e) {
      return {
        'success': false,
        'message': 'Failed to retrieve vision board session: $e',
        'error': e.toString(),
      };
    }
  }

  /// Create vision board session (POST /api/v1/manifest/createvisionboardsession)
  static Future<Map<String, dynamic>> createVisionBoardSession(Map<String, dynamic> sessionData) async {
    try {
      return await ApiClient.post('/api/v1/manifest/createvisionboardsession', body: sessionData);
    } catch (e) {
      return {
        'success': false,
        'message': 'Failed to create vision board session: $e',
        'error': e.toString(),
      };
    }
  }

  /// Update vision board session (POST /api/v1/manifest/updatevisionboardsession)
  static Future<Map<String, dynamic>> updateVisionBoardSession(Map<String, dynamic> sessionData) async {
    try {
      return await ApiClient.post('/api/v1/manifest/updatevisionboardsession', body: sessionData);
    } catch (e) {
      return {
        'success': false,
        'message': 'Failed to update vision board session: $e',
        'error': e.toString(),
      };
    }
  }

  /// Delete vision board session (POST /api/v1/manifest/markvisionboardsessionasdeleted)
  static Future<Map<String, dynamic>> deleteVisionBoardSession(String sessionId) async {
    try {
      final body = {'id': sessionId};
      return await ApiClient.post('/api/v1/manifest/markvisionboardsessionasdeleted', body: body);
    } catch (e) {
      return {
        'success': false,
        'message': 'Failed to delete vision board session: $e',
        'error': e.toString(),
      };
    }
  }

  /// Get all vision board session pictures (GET /api/v1/manifest/getvisionboardsessionpictures)
  static Future<Map<String, dynamic>> getAllVisionBoardSessionPictures() async {
    try {
      return await ApiClient.get('/api/v1/manifest/getvisionboardsessionpictures');
    } catch (e) {
      return {
        'success': false,
        'message': 'Failed to retrieve vision board session pictures: $e',
        'error': e.toString(),
      };
    }
  }

  /// Get vision board session picture by ID (GET /api/v1/manifest/getvisionboardsessionpicture)
  static Future<Map<String, dynamic>> getVisionBoardSessionPictureById(String sessionPictureId) async {
    try {
      return await ApiClient.get('/api/v1/manifest/getvisionboardsessionpicture?id=$sessionPictureId');
    } catch (e) {
      return {
        'success': false,
        'message': 'Failed to retrieve vision board session picture: $e',
        'error': e.toString(),
      };
    }
  }

  /// Create vision board session picture (POST /api/v1/manifest/createvisionboardsessionpicture)
  static Future<Map<String, dynamic>> createVisionBoardSessionPicture(Map<String, dynamic> sessionPictureData) async {
    try {
      return await ApiClient.post('/api/v1/manifest/createvisionboardsessionpicture', body: sessionPictureData);
    } catch (e) {
      return {
        'success': false,
        'message': 'Failed to create vision board session picture: $e',
        'error': e.toString(),
      };
    }
  }

  /// Update vision board session picture (POST /api/v1/manifest/updatevisionboardsessionpicture)
  static Future<Map<String, dynamic>> updateVisionBoardSessionPicture(Map<String, dynamic> sessionPictureData) async {
    try {
      return await ApiClient.post('/api/v1/manifest/updatevisionboardsessionpicture', body: sessionPictureData);
    } catch (e) {
      return {
        'success': false,
        'message': 'Failed to update vision board session picture: $e',
        'error': e.toString(),
      };
    }
  }

  /// Delete vision board session picture (POST /api/v1/manifest/markvisionboardsessionpictureasdeleted)
  static Future<Map<String, dynamic>> deleteVisionBoardSessionPicture(String sessionPictureId) async {
    try {
      final body = {'id': sessionPictureId};
      return await ApiClient.post('/api/v1/manifest/markvisionboardsessionpictureasdeleted', body: body);
    } catch (e) {
      return {
        'success': false,
        'message': 'Failed to delete vision board session picture: $e',
        'error': e.toString(),
      };
    }
  }

  /// Get vision board session pictures by session ID (GET /api/v1/manifest/getvisionboardsessionpicturebyvisionboardsessionid)
  static Future<Map<String, dynamic>> getSessionPicturesBySessionId(String sessionId) async {
    try {
      return await ApiClient.get('/api/v1/manifest/getvisionboardsessionpicturebyvisionboardsessionid?id=$sessionId');
    } catch (e) {
      return {
        'success': false,
        'message': 'Failed to retrieve session pictures by session ID: $e',
        'error': e.toString(),
      };
    }
  }

  /// Get vision board session pictures by user ID (GET /api/v1/manifest/getvisionboardsessionpicturebyuserid)
  static Future<Map<String, dynamic>> getSessionPicturesByUserId(String userId) async {
    try {
      return await ApiClient.get('/api/v1/manifest/getvisionboardsessionpicturebyuserid?id=$userId');
    } catch (e) {
      return {
        'success': false,
        'message': 'Failed to retrieve session pictures by user ID: $e',
        'error': e.toString(),
      };
    }
  }

  /// Get vision board sessions by user ID (GET /api/v1/manifest/getvisionboardsessionbyuserid)
  static Future<Map<String, dynamic>> getSessionsByUserId(String userId) async {
    try {
      return await ApiClient.get('/api/v1/manifest/getvisionboardsessionbyuserid?id=$userId');
    } catch (e) {
      return {
        'success': false,
        'message': 'Failed to retrieve sessions by user ID: $e',
        'error': e.toString(),
      };
    }
  }

  /// Get default vision board pictures (GET /api/v1/manifest/getdefaultvisionboardpictures)
  static Future<Map<String, dynamic>> getDefaultVisionBoardPictures() async {
    try {
      return await ApiClient.get('/api/v1/manifest/getdefaultvisionboardpictures');
    } catch (e) {
      return {
        'success': false,
        'message': 'Failed to retrieve default vision board pictures: $e',
        'error': e.toString(),
      };
    }
  }

  /// Create default vision board picture (POST /api/v1/manifest/createdefaultvisionboardpicture)
  static Future<Map<String, dynamic>> createDefaultVisionBoardPicture(Map<String, dynamic> pictureData) async {
    try {
      return await ApiClient.post('/api/v1/manifest/createdefaultvisionboardpicture', body: pictureData);
    } catch (e) {
      return {
        'success': false,
        'message': 'Failed to create default vision board picture: $e',
        'error': e.toString(),
      };
    }
  }

  /// Search vision board pictures (GET /api/v1/manifest/getvisionboardpicturesearch)
  static Future<Map<String, dynamic>> searchVisionBoardPictures(String query) async {
    try {
      final encodedQuery = Uri.encodeComponent(query);
      return await ApiClient.get('/api/v1/manifest/getvisionboardpicturesearch?searchText=$encodedQuery');
    } catch (e) {
      return {
        'success': false,
        'message': 'Failed to search vision board pictures: $e',
        'error': e.toString(),
      };
    }
  }

  /// Search vision board sessions (GET /api/v1/manifest/getvisionboardsessionsearch)
  static Future<Map<String, dynamic>> searchVisionBoardSessions(String query) async {
    try {
      final encodedQuery = Uri.encodeComponent(query);
      return await ApiClient.get('/api/v1/manifest/getvisionboardsessionsearch?searchText=$encodedQuery');
    } catch (e) {
      return {
        'success': false,
        'message': 'Failed to search vision board sessions: $e',
        'error': e.toString(),
      };
    }
  }

  /// Search vision board session pictures (GET /api/v1/manifest/getvisionboardsessionpicturesearch)
  static Future<Map<String, dynamic>> searchVisionBoardSessionPictures(String query) async {
    try {
      final encodedQuery = Uri.encodeComponent(query);
      return await ApiClient.get('/api/v1/manifest/getvisionboardsessionpicturesearch?searchText=$encodedQuery');
    } catch (e) {
      return {
        'success': false,
        'message': 'Failed to search vision board session pictures: $e',
        'error': e.toString(),
      };
    }
  }

  /// Create multiple vision board pictures (POST /api/v1/manifest/createmultiplevisionboardpicture)
  static Future<Map<String, dynamic>> createMultipleVisionBoardPictures(List<Map<String, dynamic>> picturesData) async {
    try {
      return await ApiClient.post('/api/v1/manifest/createmultiplevisionboardpicture', body: {'items': picturesData});
    } catch (e) {
      return {
        'success': false,
        'message': 'Failed to create multiple vision board pictures: $e',
        'error': e.toString(),
      };
    }
  }

  /// Create multiple vision board sessions (POST /api/v1/manifest/createmultiplevisionboardsession)
  static Future<Map<String, dynamic>> createMultipleVisionBoardSessions(List<Map<String, dynamic>> sessionsData) async {
    try {
      return await ApiClient.post('/api/v1/manifest/createmultiplevisionboardsession', body: {'items': sessionsData});
    } catch (e) {
      return {
        'success': false,
        'message': 'Failed to create multiple vision board sessions: $e',
        'error': e.toString(),
      };
    }
  }

  /// Create multiple vision board session pictures (POST /api/v1/manifest/createmultiplevisionboardsessionpicture)
  static Future<Map<String, dynamic>> createMultipleVisionBoardSessionPictures(List<Map<String, dynamic>> sessionPicturesData) async {
    try {
      return await ApiClient.post('/api/v1/manifest/createmultiplevisionboardsessionpicture', body: {'items': sessionPicturesData});
    } catch (e) {
      return {
        'success': false,
        'message': 'Failed to create multiple vision board session pictures: $e',
        'error': e.toString(),
      };
    }
  }

  /// Upload image for vision board (using file attachment endpoints)
  static Future<Map<String, dynamic>> uploadImage(File imageFile, String fileName) async {
    try {
      // Use the file attachment upload endpoint
      return await ApiClient.uploadFile(
        '/api/v1/manifest/uploadfileattachment',
        imageFile.path,
        'file',
        additionalFields: {
          'fileName': fileName,
          'fileType': 'image',
        },
      );
    } catch (e) {
      return {
        'success': false,
        'message': 'Failed to upload image: $e',
        'error': e.toString(),
      };
    }
  }

  /// Download file attachment (GET /api/v1/manifest/downloadfileattachmentfile)
  static Future<Map<String, dynamic>> downloadFileAttachment(String fileId) async {
    try {
      return await ApiClient.get('/api/v1/manifest/downloadfileattachmentfile?id=$fileId');
    } catch (e) {
      return {
        'success': false,
        'message': 'Failed to download file attachment: $e',
        'error': e.toString(),
      };
    }
  }

  /// Validate file attachment (GET /api/v1/manifest/isfileattachmentfilevalid)
  static Future<Map<String, dynamic>> validateFileAttachment(String fileId) async {
    try {
      return await ApiClient.get('/api/v1/manifest/isfileattachmentfilevalid?id=$fileId');
    } catch (e) {
      return {
        'success': false,
        'message': 'Failed to validate file attachment: $e',
        'error': e.toString(),
      };
    }
  }

  /// Get file attachment folders (GET /api/v1/manifest/getfileattachmentfolders)
  static Future<Map<String, dynamic>> getFileAttachmentFolders() async {
    try {
      return await ApiClient.get('/api/v1/manifest/getfileattachmentfolders');
    } catch (e) {
      return {
        'success': false,
        'message': 'Failed to retrieve file attachment folders: $e',
        'error': e.toString(),
      };
    }
  }

  /// Create file attachment folder (POST /api/v1/manifest/createfileattachmentfolder)
  static Future<Map<String, dynamic>> createFileAttachmentFolder(Map<String, dynamic> folderData) async {
    try {
      return await ApiClient.post('/api/v1/manifest/createfileattachmentfolder', body: folderData);
    } catch (e) {
      return {
        'success': false,
        'message': 'Failed to create file attachment folder: $e',
        'error': e.toString(),
      };
    }
  }
}