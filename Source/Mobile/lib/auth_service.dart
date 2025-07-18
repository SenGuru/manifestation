import 'dart:async';
import 'dart:convert';
import 'package:manifestation_app/api/generated/code/manifestation.swagger.dart';
import 'package:manifestation_app/auth_models.dart';
import 'package:manifestation_app/api_config.dart';
import 'package:manifestation_app/session_service.dart';
import 'package:chopper/chopper.dart';

class _BasicAuthInterceptor implements Interceptor {
  static const String _basicAuthCredentials = 'U3BlY2lhbFVzZXI6U3BlY2lhbFBhc3N3b3JkMQ=='; // SpecialUser:SpecialPassword1
  
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

class AuthService {
  final Manifestation _apiClient;
 
  AuthService({String? baseUrl}) : _apiClient = Manifestation.create(
    baseUrl: Uri.parse(baseUrl ?? ApiConfig.baseUrl),
    interceptors: [
      _BasicAuthInterceptor(),
      _SessionTokenInterceptor(),
    ],
  );


  /// Encode basic auth credentials
  static String _encodeBasicAuth(String username, String password) {
    final credentials = '$username:$password';
    final bytes = utf8.encode(credentials);
    final base64Str = base64.encode(bytes);
    return base64Str;
  }

  /// Login with email and password
  Future<AuthResult> login({
    required String email,
    required String password,
    bool rememberMe = false,
  }) async {
    try {
      final loginModel = LoginModel(
        username: email,
        password: password,
        rememberMe: rememberMe,
      );

      final response = await _apiClient.apiV1AccountLoginPost(body: loginModel);
      
      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        if (returnModel.isSuccess == true && returnModel.model != null) {
          final user = _mapToAuthUser(returnModel.model!);
          final token = returnModel.model!.token;
          
          // Save session data
          await SessionService.instance.saveSession(
            token: token ?? '',
            userData: {
              'id': returnModel.model!.userId,
              'email': returnModel.model!.userName,
              'firstName': returnModel.model!.firstName,
              'lastName': returnModel.model!.lastName,
            },
            expiryDate: DateTime.now().add(Duration(days: 30)), // 30 day expiry
          );
          
          return AuthResult.success(
            user: user,
            token: token,
          );
        }
        
        return AuthResult.error(
          message: returnModel.errorHolder?.friendlyMessage ?? 'Login failed',
          statusCode: response.statusCode,
        );
      }
      
      return AuthResult.error(
        message: 'Login failed',
        statusCode: response.statusCode,
      );
    } catch (e) {
      return AuthResult.error(
        message: 'Network error: ${e.toString()}',
      );
    }
  }

  /// Create new account
  Future<AuthResult> signUp({
    required String email,
    required String password,
    required String firstName,
    required String lastName,
    String? captchaCode,
  }) async {
    try {
      final accountModel = AccountCreateModel(
        email: email,
        initialPassword: password,
        firstName: firstName,
        lastName: lastName,
        captchaCode: captchaCode,
      );

      final response = await _apiClient.apiV1AccountCreateaccountPost(body: accountModel);
      
      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        if (returnModel.isSuccess == true && returnModel.model != null) {
          final user = _mapToAuthUser(returnModel.model!);
          final token = returnModel.model!.token;
          
          // Save session data after successful signup
          await SessionService.instance.saveSession(
            token: token ?? '',
            userData: {
              'id': returnModel.model!.userId,
              'email': returnModel.model!.userName,
              'firstName': returnModel.model!.firstName,
              'lastName': returnModel.model!.lastName,
            },
            expiryDate: DateTime.now().add(Duration(days: 30)), // 30 day expiry
          );
          
          return AuthResult.success(
            user: user,
            token: token,
          );
        }
        
        return AuthResult.error(
          message: returnModel.errorHolder?.friendlyMessage ?? 'Account creation failed',
          statusCode: response.statusCode,
        );
      }
      
      return AuthResult.error(
        message: 'Account creation failed',
        statusCode: response.statusCode,
      );
    } catch (e) {
      return AuthResult.error(
        message: 'Network error: ${e.toString()}',
      );
    }
  }

  /// Check if user is authenticated
  Future<bool> isAuthenticated() async {
    try {
      // First check if we have a valid session
      final hasValidSession = await SessionService.instance.isLoggedIn();
      if (!hasValidSession) {
        return false;
      }
      
      // Then verify with server
      final response = await _apiClient.apiV1AccountAuthorizedpingPost();
      final isServerAuthenticated = response.isSuccessful && 
             response.body?.isSuccess == true && 
             response.body?.model == true;
      
      // If server says not authenticated, clear local session
      if (!isServerAuthenticated) {
        await SessionService.instance.clearSession();
      }
      
      return isServerAuthenticated;
    } catch (e) {
      return false;
    }
  }

  /// Ping server to check connectivity
  Future<bool> ping() async {
    try {
      final response = await _apiClient.apiV1AccountPingPost();
      return response.isSuccessful && 
             response.body?.isSuccess == true && 
             response.body?.model == true;
    } catch (e) {
      return false;
    }
  }

  /// Reset password
  Future<AuthResult> resetPassword({
    required String email,
    String? clientRedirectURL,
  }) async {
    try {
      final response = await _apiClient.apiV1AccountUserrecoverforgotpasswordPost(
        body: RecoverPasswordRequestModel(
          userName: email,
          clientRedirectURL: clientRedirectURL,
        ),
      );
      
      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        if (returnModel.isSuccess == true) {
          return AuthResult.success(
            message: returnModel.successMessage ?? 'Password reset email sent',
          );
        }
        
        return AuthResult.error(
          message: returnModel.errorHolder?.friendlyMessage ?? 'Password reset failed',
          statusCode: response.statusCode,
        );
      }
      
      return AuthResult.error(
        message: 'Password reset failed',
        statusCode: response.statusCode,
      );
    } catch (e) {
      return AuthResult.error(
        message: 'Network error: ${e.toString()}',
      );
    }
  }

  /// Logout user and clear session
  Future<void> logout() async {
    // Clear local session (no server logout endpoint available)
    await SessionService.instance.clearSession();
    print('User logged out successfully');
  }
  
  /// Get current user from session
  Future<AuthUser?> getCurrentUser() async {
    final userData = await SessionService.instance.getUserData();
    if (userData != null) {
      return AuthUser(
        id: userData['id']?.toString() ?? '',
        email: userData['email'] ?? '',
        displayName: '${userData['firstName'] ?? ''} ${userData['lastName'] ?? ''}'.trim(),
        provider: AuthProvider.email,
        isEmailVerified: true,
        createdAt: DateTime.now(),
        lastLoginAt: DateTime.now(),
      );
    }
    return null;
  }

  /// Map API user model to local AuthUser model
  AuthUser _mapToAuthUser(LoggedInUserModel apiUser) {
    return AuthUser(
      id: apiUser.userId?.toString() ?? '',
      email: apiUser.userName,
      displayName: '${apiUser.firstName ?? ''} ${apiUser.lastName ?? ''}'.trim(),
      provider: AuthProvider.email,
      isEmailVerified: true, // Assuming verified if login successful
      createdAt: DateTime.now(), // API doesn't provide this
      lastLoginAt: DateTime.now(),
    );
  }
}

/// Result wrapper for authentication operations
class AuthResult {
  final bool isSuccess;
  final AuthUser? user;
  final String? token;
  final String? message;
  final int? statusCode;

  const AuthResult._({
    required this.isSuccess,
    this.user,
    this.token,
    this.message,
    this.statusCode,
  });

  factory AuthResult.success({
    AuthUser? user,
    String? token,
    String? message,
  }) {
    return AuthResult._(
      isSuccess: true,
      user: user,
      token: token,
      message: message,
    );
  }

  factory AuthResult.error({
    required String message,
    int? statusCode,
  }) {
    return AuthResult._(
      isSuccess: false,
      message: message,
      statusCode: statusCode,
    );
  }
}