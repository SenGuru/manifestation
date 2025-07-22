import 'package:chopper/chopper.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'dart:convert';
import 'dart:async';
import '../api/generated/code/manifestation.swagger.dart';

class AuthService {
  static final AuthService _instance = AuthService._internal();
  factory AuthService() => _instance;
  AuthService._internal();

  late Manifestation _apiClient;
  LoggedInUserModel? _currentUser;
  String? _authToken;

  void initialize({String? baseUrl}) {
    _apiClient = Manifestation.create(
      baseUrl: Uri.parse(baseUrl ?? 'https://your-api-base-url.com'),
      interceptors: [_AuthInterceptor()],
    );
    _loadStoredAuthData();
  }

  Future<void> _loadStoredAuthData() async {
    try {
      final prefs = await SharedPreferences.getInstance();
      final storedToken = prefs.getString('auth_token');
      final storedUserJson = prefs.getString('current_user');
      
      if (storedToken != null && storedUserJson != null) {
        _authToken = storedToken;
        // Parse stored user data
        final userMap = jsonDecode(storedUserJson);
        _currentUser = LoggedInUserModel.fromJson(userMap);
        print('✅ Restored auth session: ${_currentUser?.userName}');
      }
    } catch (e) {
      print('❌ Error loading stored auth data: $e');
      // Clear invalid data
      await clearStoredAuthData();
    }
  }

  Future<void> _saveAuthData() async {
    try {
      final prefs = await SharedPreferences.getInstance();
      
      if (_authToken != null) {
        await prefs.setString('auth_token', _authToken!);
      }
      
      if (_currentUser != null) {
        final userJson = jsonEncode(_currentUser!.toJson());
        await prefs.setString('current_user', userJson);
      }
      
      print('✅ Auth data saved to SharedPreferences');
    } catch (e) {
      print('❌ Error saving auth data: $e');
    }
  }

  Future<void> clearStoredAuthData() async {
    try {
      final prefs = await SharedPreferences.getInstance();
      await prefs.remove('auth_token');
      await prefs.remove('current_user');
      print('✅ Cleared stored auth data');
    } catch (e) {
      print('❌ Error clearing auth data: $e');
    }
  }

  LoggedInUserModel? get currentUser => _currentUser;
  String? get authToken => _authToken;
  bool get isLoggedIn => _currentUser != null && _authToken != null;

  Future<AuthResult> login({
    required String username,
    required String password,
    bool rememberMe = false,
  }) async {
    try {
      final loginModel = LoginModel(
        username: username,
        password: password,
        rememberMe: rememberMe,
      );

      final response = await _apiClient.apiV1AccountLoginPost(body: loginModel);

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        if (returnModel.isSuccess == true && returnModel.model != null) {
          _currentUser = returnModel.model!;
          _authToken = returnModel.model!.token;
          
          // Save to SharedPreferences
          await _saveAuthData();
          
          return AuthResult.success(
            user: _currentUser!,
            token: _authToken!,
            message: returnModel.successMessage ?? 'Login successful',
          );
        } else {
          return AuthResult.failure(
            message: returnModel.errorHolder?.toString() ?? 'Login failed',
          );
        }
      } else {
        return AuthResult.failure(
          message: 'Network error: ${response.statusCode}',
        );
      }
    } catch (e) {
      return AuthResult.failure(
        message: 'Login error: ${e.toString()}',
      );
    }
  }

  Future<AuthResult> createAccount({
    required String email,
    required String firstName,
    required String lastName,
    required String password,
    String? captchaCode,
  }) async {
    try {
      final createModel = AccountCreateModel(
        email: email,
        firstName: firstName,
        lastName: lastName,
        initialPassword: password,
        captchaCode: captchaCode,
      );

      final response = await _apiClient.apiV1AccountCreateaccountPost(body: createModel);

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        if (returnModel.isSuccess == true && returnModel.model != null) {
          _currentUser = returnModel.model!;
          _authToken = returnModel.model!.token;
          
          // Save to SharedPreferences
          await _saveAuthData();
          
          return AuthResult.success(
            user: _currentUser!,
            token: _authToken!,
            message: returnModel.successMessage ?? 'Account created successfully',
          );
        } else {
          return AuthResult.failure(
            message: returnModel.errorHolder?.toString() ?? 'Account creation failed',
          );
        }
      } else {
        return AuthResult.failure(
          message: 'Network error: ${response.statusCode}',
        );
      }
    } catch (e) {
      return AuthResult.failure(
        message: 'Account creation error: ${e.toString()}',
      );
    }
  }

  Future<void> logout() async {
    _currentUser = null;
    _authToken = null;
    await clearStoredAuthData();
  }

  Future<bool> ping() async {
    try {
      final response = await _apiClient.apiV1AccountPingPost();
      return response.isSuccessful;
    } catch (e) {
      return false;
    }
  }

  Future<bool> authorizedPing() async {
    try {
      final response = await _apiClient.apiV1AccountAuthorizedpingPost();
      return response.isSuccessful;
    } catch (e) {
      return false;
    }
  }
}

class AuthResult {
  final bool isSuccess;
  final String message;
  final LoggedInUserModel? user;
  final String? token;

  AuthResult._({
    required this.isSuccess,
    required this.message,
    this.user,
    this.token,
  });

  factory AuthResult.success({
    required LoggedInUserModel user,
    required String token,
    required String message,
  }) {
    return AuthResult._(
      isSuccess: true,
      message: message,
      user: user,
      token: token,
    );
  }

  factory AuthResult.failure({
    required String message,
  }) {
    return AuthResult._(
      isSuccess: false,
      message: message,
    );
  }
}

// Auth interceptor to add token to requests  
class _AuthInterceptor implements Interceptor {
  @override
  FutureOr<Response<BodyType>> intercept<BodyType>(Chain<BodyType> chain) async {
    final request = chain.request;
    final authService = AuthService();
    
    if (authService.authToken != null) {
      final updatedRequest = request.copyWith(
        headers: {
          ...request.headers,
          'Authorization': 'Bearer ${authService.authToken}',
        },
      );
      return chain.proceed(updatedRequest);
    }
    
    return chain.proceed(request);
  }
}