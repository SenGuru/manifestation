import 'dart:async';
import 'dart:convert';
import 'dart:io';
import 'dart:typed_data';
import 'package:chopper/chopper.dart';
import 'package:image_picker/image_picker.dart';
import 'package:http/http.dart' as http;
import '../api/generated/code/manifestation.swagger.dart';
import '../services/unified_auth_service.dart';
import '../services/api_client.dart';

class UserProfileService {
  static final UserProfileService _instance = UserProfileService._internal();
  factory UserProfileService() => _instance;
  UserProfileService._internal();
  
  Manifestation? _apiClient;
  bool _isInitialized = false;

  Future<void> initialize({String? baseUrl}) async {
    // Only initialize once to prevent multiple initialization
    if (_isInitialized) {
      print('👤 UserProfileService already initialized, skipping...');
      return;
    }
    
    print('👤 Initializing UserProfileService...');
    final authService = UnifiedAuthService();
    final token = authService.authToken;
    
    if (token == null) {
      throw Exception('No authentication token found');
    }

    // Set the token in ApiClient for authentication
    await ApiClient.setAuthToken(token);
    
    // Create the Swagger client with interceptors like other services
    _apiClient = Manifestation.create(
      baseUrl: Uri.parse(baseUrl ?? 'https://manifestation-qa-1.senthilguru.net'),
      interceptors: [_AuthInterceptor()],
    );
    
    _isInitialized = true;
    print('✅ UserProfileService initialized with base URL: ${baseUrl ?? 'https://manifestation-qa-1.senthilguru.net'}');
  }

  /// Upload a user profile picture
  Future<ServiceResult<bool>> uploadProfilePicture(File imageFile, {int? userId}) async {
    try {
      if (_apiClient == null) {
        await initialize();
      }

      print('📤 Uploading profile picture...');
      
      // Get userId if not provided
      if (userId == null) {
        final authService = UnifiedAuthService();
        userId = await authService.getUserId();
        print('📤 Auto-retrieved User ID: $userId');
      }
      
      if (userId == null) {
        print('❌ No user ID available for profile picture upload');
        return ServiceResult.failure('User ID not available');
      }
      
      // Use raw HTTP multipart request instead of Chopper-generated client
      // This is similar to how VisionBoardService handles multipart uploads
      
      final authService = UnifiedAuthService();
      final authToken = authService.authToken;
      
      if (authToken == null) {
        return ServiceResult.failure('No authentication token available');
      }
      
      // Create multipart request
      final uri = Uri.parse('https://manifestation-qa-1.senthilguru.net/api/v1/company/uploaduserpicture');
      final request = http.MultipartRequest('POST', uri);
      
      // Add auth header
      request.headers['Authorization'] = 'Bearer $authToken';
      
      // Add form fields
      request.fields['UserId'] = userId.toString();
      
      // Read file bytes and add file
      final imageBytes = await imageFile.readAsBytes();
      
      // Get file extension from the original file
      String filename = 'profile_picture.jpg'; // Default
      if (imageFile.path.isNotEmpty) {
        final extension = imageFile.path.split('.').last.toLowerCase();
        filename = 'profile_picture.$extension';
      }
      
      request.files.add(http.MultipartFile.fromBytes(
        'UploadedFile',
        imageBytes,
        filename: filename,
      ));
      
      print('📤 Sending multipart request with ${imageBytes.length} bytes');
      print('📤 UserId field: ${userId.toString()}');
      
      // Send request
      final streamedResponse = await request.send();
      final response = await http.Response.fromStream(streamedResponse);
      
      print('📤 Response status: ${response.statusCode}');
      print('📤 Response body: ${response.body}');
      
      if (response.statusCode >= 200 && response.statusCode < 300) {
        try {
          // Try to parse the response as JSON
          final responseData = response.body;
          if (responseData.contains('"isSuccess":true') || responseData.contains('success')) {
            print('✅ Profile picture uploaded successfully');
            return ServiceResult.success(true);
          } else {
            print('❌ Upload failed based on response content');
            return ServiceResult.failure('Upload failed: $responseData');
          }
        } catch (e) {
          // If JSON parsing fails, just check status code
          print('✅ Profile picture uploaded successfully (status ${response.statusCode})');
          return ServiceResult.success(true);
        }
      } else {
        print('❌ Failed to upload profile picture: HTTP ${response.statusCode}');
        return ServiceResult.failure('HTTP Error ${response.statusCode}: ${response.body}');
      }
    } catch (e) {
      print('❌ Error uploading profile picture: $e');
      return ServiceResult.failure('Error uploading profile picture: $e');
    }
  }

  /// Upload profile picture from XFile (from image picker)
  Future<ServiceResult<bool>> uploadProfilePictureFromXFile(XFile imageFile, {int? userId}) async {
    try {
      final file = File(imageFile.path);
      return await uploadProfilePicture(file, userId: userId);
    } catch (e) {
      print('❌ Error converting XFile to File: $e');
      return ServiceResult.failure('Error processing image file: $e');
    }
  }

  /// Upload profile picture from bytes
  Future<ServiceResult<bool>> uploadProfilePictureFromBytes(Uint8List imageBytes, {int? userId}) async {
    try {
      if (_apiClient == null) {
        await initialize();
      }

      print('📤 Uploading profile picture from bytes...');
      
      // Get userId if not provided
      if (userId == null) {
        final authService = UnifiedAuthService();
        userId = await authService.getUserId();
        print('📤 Auto-retrieved User ID: $userId');
      }
      
      if (userId == null) {
        print('❌ No user ID available for profile picture upload');
        return ServiceResult.failure('User ID not available');
      }
      
      // Use raw HTTP multipart request for bytes upload too
      final authService = UnifiedAuthService();
      final authToken = authService.authToken;
      
      if (authToken == null) {
        return ServiceResult.failure('No authentication token available');
      }
      
      // Create multipart request
      final uri = Uri.parse('https://manifestation-qa-1.senthilguru.net/api/v1/company/uploaduserpicture');
      final request = http.MultipartRequest('POST', uri);
      
      // Add auth header
      request.headers['Authorization'] = 'Bearer $authToken';
      
      // Add form fields
      request.fields['UserId'] = userId.toString();
      
      // Add file from bytes
      request.files.add(http.MultipartFile.fromBytes(
        'UploadedFile',
        imageBytes,
        filename: 'profile_picture.jpg', // Default filename
      ));
      
      print('📤 Sending multipart request with ${imageBytes.length} bytes');
      
      // Send request
      final streamedResponse = await request.send();
      final response = await http.Response.fromStream(streamedResponse);
      
      print('📤 Response status: ${response.statusCode}');
      print('📤 Response body: ${response.body}');
      
      if (response.statusCode >= 200 && response.statusCode < 300) {
        try {
          final responseData = response.body;
          if (responseData.contains('"isSuccess":true') || responseData.contains('success')) {
            print('✅ Profile picture uploaded successfully');
            return ServiceResult.success(true);
          } else {
            print('❌ Upload failed based on response content');
            return ServiceResult.failure('Upload failed: $responseData');
          }
        } catch (e) {
          print('✅ Profile picture uploaded successfully (status ${response.statusCode})');
          return ServiceResult.success(true);
        }
      } else {
        print('❌ Failed to upload profile picture: HTTP ${response.statusCode}');
        return ServiceResult.failure('HTTP Error ${response.statusCode}: ${response.body}');
      }
    } catch (e) {
      print('❌ Error uploading profile picture: $e');
      return ServiceResult.failure('Error uploading profile picture: $e');
    }
  }

  /// Update user profile information
  Future<ServiceResult<UserProfileModel?>> updateProfile({
    int? userId,
    String? email,
    String? firstName, 
    String? lastName,
  }) async {
    try {
      if (_apiClient == null) {
        await initialize();
      }

      print('📝 Updating user profile...');
      
      // Get userId if not provided
      if (userId == null) {
        final authService = UnifiedAuthService();
        userId = await authService.getUserId();
        print('📝 Auto-retrieved User ID: $userId');
      }
      
      if (userId == null) {
        print('❌ No user ID available for profile update');
        return ServiceResult.failure('User ID not available');
      }
      
      // Include userId in case server needs it for validation
      final profileModel = MinimalUserProfileModel(
        userId: userId,
        email: email,
        firstName: firstName,
        lastName: lastName,
      );

      print('📝 Profile model being sent:');
      print('  - userId: $userId');
      print('  - email: $email');
      print('  - firstName: $firstName');
      print('  - lastName: $lastName');
      print('📝 Profile model JSON: ${profileModel.toJson()}');
      
      // Basic validation before sending
      if (email != null && email.isNotEmpty) {
        final emailRegex = RegExp(r'^[^@]+@[^@]+\.[^@]+$');
        if (!emailRegex.hasMatch(email)) {
          print('❌ Invalid email format: $email');
          return ServiceResult.failure('Invalid email format');
        }
      }

      final response = await _apiClient!.apiV1CompanyUpdateminimaluserprofileforcurrentuserPost(
        body: profileModel,
      );

      print('📝 Response status: ${response.statusCode}');
      print('📝 Response successful: ${response.isSuccessful}');
      print('📝 Response body isSuccess: ${response.body?.isSuccess}');
      
      if (response.isSuccessful && response.body?.isSuccess == true) {
        print('✅ Profile updated successfully');
        return ServiceResult.success(response.body?.model);
      } else {
        final errorDetails = response.body?.errorHolder?.toString() ?? 'Unknown error';
        final httpStatus = response.statusCode;
        print('❌ Failed to update profile:');
        print('  - HTTP Status: $httpStatus');
        print('  - Error: $errorDetails');
        print('  - Full response body: ${response.body}');
        // Check if this is the specific server error we've been encountering
        if (errorDetails.contains('Unable to update user profile') && 
            errorDetails.contains('CompanyService.UpdateMinimalUserProfileAsync')) {
          return ServiceResult.failure('Profile update is currently experiencing server-side issues. Your profile picture upload works fine, but name/email updates need server-side fixes. Please contact support.');
        }
        return ServiceResult.failure('Failed to update profile: $errorDetails (HTTP $httpStatus)');
      }
    } catch (e) {
      print('❌ Error updating profile: $e');
      return ServiceResult.failure('Error updating profile: $e');
    }
  }

  /// Get user profile picture
  /// Returns the image URL or null if no picture exists
  Future<ServiceResult<String?>> getUserPicture({int? userId, String? profileGuid}) async {
    try {
      if (_apiClient == null) {
        await initialize();
      }

      print('📸 Getting user profile picture...');
      print('   userId: $userId, profileGuid: $profileGuid');

      // Make the API call
      final response = await _apiClient!.apiV1CompanyGetuserpictureGet(
        userId: userId,
        profileGuid: profileGuid,
      );

      print('📸 Profile picture API response status: ${response.statusCode}');

      if (response.isSuccessful) {
        // The response body should contain the image URL or data
        final responseData = response.body;
        print('✅ Successfully retrieved profile picture data');
        print('📸 Response data type: ${responseData.runtimeType}');
        
        // If the response contains an image URL, return it
        if (responseData is Map && responseData.containsKey('imageUrl')) {
          return ServiceResult.success(responseData['imageUrl'] as String?);
        }
        
        // If the response contains base64 data, convert it to a data URL
        if (responseData is Map && responseData.containsKey('imageData')) {
          final base64Data = responseData['imageData'] as String;
          final dataUrl = 'data:image/jpeg;base64,$base64Data';
          return ServiceResult.success(dataUrl);
        }
        
        // If response is raw image data (Uint8List or List<int>), convert to base64 data URL
        if (responseData is List<int>) {
          try {
            final bytes = Uint8List.fromList(responseData);
            final base64String = base64.encode(bytes);
            
            // Detect image format from the first few bytes
            String mimeType = 'image/jpeg'; // default
            if (bytes.length > 3) {
              // PNG signature: 89 50 4E 47
              if (bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E && bytes[3] == 0x47) {
                mimeType = 'image/png';
              }
              // JPEG signature: FF D8 FF
              else if (bytes[0] == 0xFF && bytes[1] == 0xD8 && bytes[2] == 0xFF) {
                mimeType = 'image/jpeg';
              }
            }
            
            final dataUrl = 'data:$mimeType;base64,$base64String';
            print('📸 Converted raw image data to base64 data URL (${bytes.length} bytes, $mimeType)');
            return ServiceResult.success(dataUrl);
          } catch (e) {
            print('❌ Error converting raw image data to base64: $e');
            return ServiceResult.failure('Error processing image data: $e');
          }
        }
        
        // If response is a direct string (URL or base64), return it
        if (responseData is String && responseData.isNotEmpty) {
          // Check if it looks like raw data that starts with image signature
          if (responseData.startsWith('PNG') || responseData.codeUnits.first == 0x89) {
            print('📸 Detected raw PNG data in string format');
            try {
              final bytes = responseData.codeUnits;
              final base64String = base64.encode(bytes);
              final dataUrl = 'data:image/png;base64,$base64String';
              return ServiceResult.success(dataUrl);
            } catch (e) {
              print('❌ Error converting raw PNG string to base64: $e');
              return ServiceResult.failure('Error processing PNG data: $e');
            }
          }
          return ServiceResult.success(responseData);
        }
        
        print('📸 No profile picture found');
        return ServiceResult.success(null);
      } else {
        final statusCode = response.statusCode;
        final errorBody = response.error;
        print('❌ Failed to get profile picture: HTTP $statusCode - $errorBody');
        
        if (statusCode == 404) {
          // User has no profile picture set
          return ServiceResult.success(null);
        }
        
        return ServiceResult.failure('Failed to get profile picture: HTTP $statusCode');
      }
    } catch (e) {
      print('❌ Error getting profile picture: $e');
      return ServiceResult.failure('Error getting profile picture: $e');
    }
  }

  /// Verify user password by attempting to authenticate
  /// Since there's no dedicated verify password endpoint, we'll use the login endpoint
  Future<ServiceResult<bool>> verifyPassword(String email, String password) async {
    try {
      // Create a temporary API client without authentication for login verification
      final tempClient = Manifestation.create(
        baseUrl: Uri.parse('https://manifestation-qa-1.senthilguru.net'),
      );

      print('🔐 Verifying password...');
      
      // Use the login endpoint to verify credentials
      // LoginModel uses username field, not email
      final loginModel = LoginModel(
        username: email,
        password: password,
        rememberMe: false,
      );

      final response = await tempClient.apiV1AccountLoginPost(body: loginModel);

      if (response.isSuccessful && response.body?.isSuccess == true) {
        print('✅ Password verified successfully');
        return ServiceResult.success(true);
      } else {
        print('❌ Password verification failed: ${response.body?.errorHolder ?? 'Invalid credentials'}');
        return ServiceResult.failure(response.body?.errorHolder?.toString() ?? 'Invalid credentials');
      }
    } catch (e) {
      print('❌ Error verifying password: $e');
      return ServiceResult.failure('Error verifying password: $e');
    }
  }

  /// Split full name into first and last name
  Map<String, String> splitFullName(String fullName) {
    final parts = fullName.trim().split(' ');
    if (parts.isEmpty) {
      return {'firstName': '', 'lastName': ''};
    } else if (parts.length == 1) {
      return {'firstName': parts[0], 'lastName': ''};
    } else {
      final firstName = parts.first;
      final lastName = parts.skip(1).join(' ');
      return {'firstName': firstName, 'lastName': lastName};
    }
  }

  /// Combine first and last name into full name
  String combineNames(String? firstName, String? lastName) {
    final first = firstName?.trim() ?? '';
    final last = lastName?.trim() ?? '';
    
    if (first.isEmpty && last.isEmpty) {
      return '';
    } else if (first.isEmpty) {
      return last;
    } else if (last.isEmpty) {
      return first;
    } else {
      return '$first $last';
    }
  }
}

/// Service result wrapper for consistent error handling
class ServiceResult<T> {
  final bool isSuccess;
  final T? data;
  final String? errorMessage;

  ServiceResult._({required this.isSuccess, this.data, this.errorMessage});

  factory ServiceResult.success(T data) {
    return ServiceResult._(isSuccess: true, data: data);
  }

  factory ServiceResult.failure(String errorMessage) {
    return ServiceResult._(isSuccess: false, errorMessage: errorMessage);
  }
}

/// Auth interceptor for adding Bearer tokens to requests
class _AuthInterceptor implements Interceptor {
  @override
  FutureOr<Response<BodyType>> intercept<BodyType>(Chain<BodyType> chain) async {
    final request = chain.request;
    
    try {
      final authService = UnifiedAuthService();
      final authToken = authService.authToken;
      
      if (authToken != null && authToken.isNotEmpty) {
        print('🔐 UserProfileService - Adding Bearer token to request');
        final updatedRequest = request.copyWith(
          headers: {
            ...request.headers,
            'Authorization': 'Bearer $authToken',
          },
        );
        return chain.proceed(updatedRequest);
      } else {
        print('⚠️  UserProfileService - No token available, proceeding without auth');
      }
    } catch (e) {
      print('❌ UserProfileService - Error getting auth token: $e');
    }
    
    return chain.proceed(request);
  }
}