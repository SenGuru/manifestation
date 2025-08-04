import 'dart:async';
import 'dart:convert';
import 'package:chopper/chopper.dart';
import 'package:http/http.dart' as http;
import '../models/vision_board.dart';
import '../api/generated/code/manifestation.swagger.dart';
import 'unified_auth_service.dart';
import 'package:xml/xml.dart';
import 'package:flutter/material.dart';

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
      
      // Use Swagger-generated client for update
      final response = await _apiClient!.apiV1ManifestUpdatevisionboardpicturePost(
        UploadedFile: xmlBytes,
        visionBoardPictureId: finalPictureId,
        location: visionBoard.id,
        fileName: 'visionboard_${visionBoard.id}.xml',
        fileSize: xmlBytes.length,
        userId: userId,
      );
      
      if (response.isSuccessful && response.body?.isSuccess == true) {
        print('✅ Vision board updated successfully');
        return VisionBoardResult.success(
          data: true,
          message: 'Vision board updated successfully'
        );
      } else {
        final errorMessage = response.body?.errorHolder?.toString() ?? 
                            response.body?.successMessage ?? 
                            'Failed to update vision board';
        return VisionBoardResult.failure(message: errorMessage);
      }
      
    } catch (e) {
      print('❌ Error updating vision board: $e');
      return VisionBoardResult.failure(message: 'Failed to update vision board: $e');
    }
  }
  
  // ========================= GET VISION BOARDS =========================
  
  /// Get all vision boards for current user
  Future<VisionBoardResult<List<VisionBoard>>> getUserVisionBoards() async {
    try {
      if (_apiClient == null || _authService == null) {
        return VisionBoardResult.failure(message: 'Service not initialized');
      }
      
      final userId = await _authService!.getUserId();
      if (userId == null) {
        return VisionBoardResult.failure(message: 'User not authenticated');
      }
      
      print('📥 Getting vision boards for user: $userId');
      
      // Get list of vision boards from server using correct endpoint
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
      
      // Parse each vision board from the response data
      for (final picture in pictures) {
        if (picture.fileName?.endsWith('.xml') == true) {
          try {
            // Use the vision board data directly from the response instead of downloading separately
            final visionBoard = await _parseVisionBoardFromPicture(picture);
            if (visionBoard != null) {
              visionBoards.add(visionBoard);
            }
          } catch (e) {
            print('⚠️ Failed to parse vision board ${picture.visionBoardPictureId}: $e');
            // Continue with other vision boards
          }
        }
      }
      
      // Sort by creation date (newest first)
      visionBoards.sort((a, b) => b.createdAt.compareTo(a.createdAt));
      
      print('✅ Retrieved ${visionBoards.length} vision boards');
      return VisionBoardResult.success(
        data: visionBoards,
        message: 'Vision boards retrieved successfully'
      );
      
    } catch (e) {
      print('❌ Error getting vision boards: $e');
      return VisionBoardResult.failure(message: 'Failed to get vision boards: $e');
    }
  }
  
  /// Get specific vision board by ID
  Future<VisionBoardResult<VisionBoard?>> getVisionBoard(String visionBoardId) async {
    try {
      // Get all vision boards and find the specific one
      final result = await getUserVisionBoards();
      if (!result.isSuccess) {
        return VisionBoardResult.failure(message: result.message);
      }
      
      final visionBoard = result.data?.firstWhere(
        (board) => board.id == visionBoardId,
        orElse: () => throw StateError('Vision board not found'),
      );
      
      if (visionBoard != null) {
        return VisionBoardResult.success(
          data: visionBoard,
          message: 'Vision board retrieved successfully'
        );
      } else {
        return VisionBoardResult.failure(message: 'Vision board not found');
      }
      
    } catch (e) {
      print('❌ Error getting vision board: $e');
      return VisionBoardResult.failure(message: 'Failed to get vision board: $e');
    }
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
      // Get all vision board pictures for user using correct endpoint
      final response = await _apiClient!.apiV1ManifestGetvisionboardpicturesGet(
        userId: userId,
      );
      
      if (response.isSuccessful && response.body?.isSuccess == true) {
        final pictures = response.body?.model ?? [];
        
        // Find the picture with matching location (vision board ID)
        for (final picture in pictures) {
          if (picture.location == visionBoardId && picture.fileName?.endsWith('.xml') == true) {
            return picture.visionBoardPictureId;
          }
        }
      }
      
      return null;
    } catch (e) {
      print('❌ Error getting picture ID: $e');
      return null;
    }
  }
  
  /// Parse vision board from picture metadata using correct flow
  Future<VisionBoard?> _parseVisionBoardFromPicture(dynamic picture) async {
    try {
      final pictureId = picture.visionBoardPictureId;
      if (pictureId == null) {
        print('⚠️ Picture has no ID: $picture');
        return null;
      }
      
      // Use getvisionboardpicture (singular) to get the actual XML content
      final response = await _apiClient!.apiV1ManifestGetvisionboardpictureGet(
        id: pictureId,
      );
      
      if (!response.isSuccessful || response.body?.isSuccess != true) {
        print('⚠️ Failed to get vision board content for ID $pictureId');
        return null;
      }
      
      // Check if the response contains XML content (need to determine the correct field)
      final xmlContent = response.bodyString; // Or response.body?.someField?
      if (xmlContent != null && xmlContent.isNotEmpty) {
        print('🔍 Received XML content for picture ID $pictureId (${xmlContent.length} chars):');
        print('First 500 chars: ${xmlContent.length > 500 ? xmlContent.substring(0, 500) + "..." : xmlContent}');
        print('Last 200 chars: ${xmlContent.length > 200 ? "..." + xmlContent.substring(xmlContent.length - 200) : xmlContent}');
        
        return _parseVisionBoardXML(xmlContent);
      }
      
      return null;
    } catch (e) {
      print('❌ Error parsing vision board from picture: $e');
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
        builder.attribute('backgroundColor', '#${visionBoard.backgroundColor.value.toRadixString(16).padLeft(8, '0').substring(2)}');
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
                builder.attribute('imagePath', element.imagePath);
                if (element.imageUrl != null) {
                  builder.attribute('imageUrl', element.imageUrl!);
                }
                builder.attribute('fit', element.fit.name);
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
                builder.attribute('color', '#${(element.style.color?.value ?? 0xFF000000).toRadixString(16).padLeft(8, '0').substring(2)}');
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
  VisionBoard? _parseVisionBoardXML(String xmlContent) {
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
          elements.add(_parseImageElement(elementNode));
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
  VisionBoardImageElement _parseImageElement(XmlElement element) {
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
      imagePath: element.getAttribute('imagePath')!,
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

