import 'dart:async';
import 'dart:convert';
import 'package:chopper/chopper.dart';
import 'package:flutter/foundation.dart';
import 'package:shared_preferences/shared_preferences.dart';
import '../api/generated/code/manifestation.swagger.dart';

/// Unified authentication service that combines login, session management, and user data
class UnifiedAuthService {
  static final UnifiedAuthService _instance = UnifiedAuthService._internal();
  factory UnifiedAuthService() => _instance;
  UnifiedAuthService._internal();

  // API Client
  late Manifestation _apiClient;
  
  // Session state keys
  static const String _isLoggedInKey = 'is_logged_in';
  static const String _authTokenKey = 'auth_token';
  static const String _tokenExpiryKey = 'token_expiry';
  static const String _lastLoginTimeKey = 'last_login_time';
  static const String _sessionIdKey = 'session_id';
  static const String _rememberMeKey = 'remember_me';
  static const String _currentUserKey = 'current_user';
  static const String _userIdKey = 'user_id';
  static const String _userNameKey = 'user_name';
  static const String _firstNameKey = 'first_name';
  static const String _lastNameKey = 'last_name';
  
  // Session settings
  static const Duration _defaultSessionTimeout = Duration(hours: 24);
  static const Duration _rememberMeTimeout = Duration(days: 30);
  
  // In-memory state
  bool _isInitialized = false;
  SessionState _currentState = SessionState.loggedOut;
  DateTime? _lastActivityTime;
  Timer? _sessionTimeoutTimer;
  LoggedInUserModel? _currentUser;
  String? _authToken;
  int? _cachedUserId;
  Map<String, dynamic>? _cachedUserData;

  /// Initialize the unified auth service
  Future<void> initialize({String? baseUrl}) async {
    if (_isInitialized) return;
    
    try {
      print('=== UNIFIED AUTH SERVICE INITIALIZATION ===');
      
      // Initialize API client
      final apiBaseUrl = baseUrl ?? 'http://manifestation-qa-1.senthilguru.net';
      _apiClient = Manifestation.create(
        baseUrl: Uri.parse(apiBaseUrl),
        interceptors: [_AuthInterceptor()],
      );
      
      // Check existing session state
      await _checkStoredSession();
      
      // Start session monitoring
      _startSessionMonitoring();
      
      _isInitialized = true;
      print('Unified auth service initialized with state: $_currentState');
    } catch (e) {
      print('Error initializing unified auth service: $e');
      _currentState = SessionState.loggedOut;
      _isInitialized = true;
    }
  }

  /// Login with username/email and password
  Future<AuthResult> login({
    required String username,
    required String password,
    bool rememberMe = false,
  }) async {
    try {
      print('🔐 Attempting login for: $username');

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
          
          // Save session with all data
          await _saveSession(
            token: _authToken!,
            userData: _currentUser!.toJson(),
            rememberMe: rememberMe,
          );
          
          print('✅ Login successful for: ${_currentUser!.userName}');
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
      print('❌ Login error: $e');
      return AuthResult.failure(message: 'Network error: $e');
    }
  }

  /// Create new account
  Future<AuthResult> createAccount({
    required String email,
    required String firstName,
    required String lastName,
    required String password,
    required String captchaCode,
    bool rememberMe = false,
  }) async {
    try {
      print('🔐 Attempting account creation for: $email');

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
          
          // Save session
          await _saveSession(
            token: _authToken!,
            userData: _currentUser!.toJson(),
            rememberMe: rememberMe,
          );
          
          print('✅ Account creation successful for: ${_currentUser!.userName}');
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
      print('❌ Account creation error: $e');
      return AuthResult.failure(message: 'Network error: $e');
    }
  }

  /// Logout user and clear session
  Future<void> logout() async {
    try {
      print('🔓 Logging out user');
      
      final previousState = _currentState;
      _currentState = SessionState.loggedOut;
      _lastActivityTime = null;
      _currentUser = null;
      _authToken = null;
      _cachedUserId = null;
      _cachedUserData = null;
      
      // Clear stored session
      await _clearStoredSession();
      
      // Stop session monitoring
      _sessionTimeoutTimer?.cancel();
      
      print('✅ Logout successful (was: $previousState)');
    } catch (e) {
      print('❌ Logout error: $e');
    }
  }

  /// Save session data to storage
  Future<void> _saveSession({
    required String token,
    required Map<String, dynamic> userData,
    bool rememberMe = false,
    Duration? customTimeout,
  }) async {
    try {
      final prefs = await SharedPreferences.getInstance();
      final now = DateTime.now();
      final sessionTimeout = customTimeout ?? (rememberMe ? _rememberMeTimeout : _defaultSessionTimeout);
      final tokenExpiry = now.add(sessionTimeout);
      
      // Store session data
      await prefs.setBool(_isLoggedInKey, true);
      await prefs.setString(_authTokenKey, token);
      await prefs.setString(_tokenExpiryKey, tokenExpiry.toIso8601String());
      await prefs.setString(_lastLoginTimeKey, now.toIso8601String());
      await prefs.setBool(_rememberMeKey, rememberMe);
      
      // Store user data
      await prefs.setString(_currentUserKey, jsonEncode(userData));
      
      // Store individual fields for quick access
      final model = userData['model'] ?? userData;
      await prefs.setInt(_userIdKey, model['userId'] ?? 0);
      await prefs.setString(_userNameKey, model['userName'] ?? '');
      await prefs.setString(_firstNameKey, model['firstName'] ?? '');
      await prefs.setString(_lastNameKey, model['lastName'] ?? '');
      
      // Cache the data
      _cachedUserData = userData;
      _cachedUserId = model['userId'];
      
      // Update session state
      _currentState = SessionState.loggedIn;
      _lastActivityTime = now;
      
      print('✅ Session saved successfully');
    } catch (e) {
      print('❌ Error saving session: $e');
    }
  }

  /// Check if there's a valid stored session
  Future<void> _checkStoredSession() async {
    try {
      final prefs = await SharedPreferences.getInstance();
      
      final isLoggedIn = prefs.getBool(_isLoggedInKey) ?? false;
      final storedToken = prefs.getString(_authTokenKey);
      final tokenExpiryString = prefs.getString(_tokenExpiryKey);
      final lastLoginString = prefs.getString(_lastLoginTimeKey);
      final rememberMe = prefs.getBool(_rememberMeKey) ?? false;
      final storedUserJson = prefs.getString(_currentUserKey);
      
      print('Stored session check:');
      print('- Is logged in: $isLoggedIn');
      print('- Has token: ${storedToken != null}');
      print('- Token length: ${storedToken?.length ?? 0}');
      print('- Token expiry: $tokenExpiryString');
      print('- Remember me: $rememberMe');
      print('- Has user data: ${storedUserJson != null}');
      print('- User data length: ${storedUserJson?.length ?? 0}');
      
      // Check if we have essential data (token is more important than the flag)
      if (storedToken == null || storedToken.isEmpty) {
        print('❌ No valid token found, logging out');
        _currentState = SessionState.loggedOut;
        return;
      }
      
      // If we have a token but the flag is false, it might be from the old system
      if (!isLoggedIn) {
        print('⚠️ Token exists but is_logged_in flag is false (possibly from old auth system)');
        print('⚠️ Proceeding with token-based authentication');
      }
      
      // Check token expiry
      if (tokenExpiryString != null) {
        final tokenExpiry = DateTime.parse(tokenExpiryString);
        if (DateTime.now().isAfter(tokenExpiry)) {
          print('Token expired, clearing session');
          await _clearStoredSession();
          _currentState = SessionState.expired;
          return;
        }
      }
      
      // Check last login time for remember me
      if (lastLoginString != null) {
        final lastLogin = DateTime.parse(lastLoginString);
        final sessionTimeout = rememberMe ? _rememberMeTimeout : _defaultSessionTimeout;
        
        if (DateTime.now().difference(lastLogin) > sessionTimeout) {
          print('Session timed out, clearing session');
          await _clearStoredSession();
          _currentState = SessionState.timedOut;
          return;
        }
      }
      
      // Restore session data
      _authToken = storedToken;
      if (storedUserJson != null) {
        try {
          print('🔍 Parsing stored user data...');
          final userMap = jsonDecode(storedUserJson);
          print('🔍 Decoded user map keys: ${userMap.keys.toList()}');
          _currentUser = LoggedInUserModel.fromJson(userMap);
          _cachedUserData = userMap;
          _cachedUserId = userMap['userId'];
          print('✅ Restored user session: ${_currentUser?.userName} (ID: $_cachedUserId)');
        } catch (e) {
          print('❌ Error parsing stored user data: $e');
          print('❌ Raw user data: $storedUserJson');
        }
      }
      
      // Session is valid, restore it
      _currentState = SessionState.loggedIn;
      _lastActivityTime = DateTime.now();
      
      // Update the is_logged_in flag if it wasn't set (for future restarts)
      if (!isLoggedIn) {
        print('🔧 Updating is_logged_in flag for future restarts');
        await prefs.setBool(_isLoggedInKey, true);
      }
      
      print('✅ Session successfully restored');
      
    } catch (e) {
      print('❌ Error checking stored session: $e');
      _currentState = SessionState.loggedOut;
    }
  }

  /// Clear stored session data
  Future<void> _clearStoredSession() async {
    try {
      final prefs = await SharedPreferences.getInstance();
      await prefs.remove(_isLoggedInKey);
      await prefs.remove(_authTokenKey);
      await prefs.remove(_tokenExpiryKey);
      await prefs.remove(_lastLoginTimeKey);
      await prefs.remove(_sessionIdKey);
      await prefs.remove(_rememberMeKey);
      await prefs.remove(_currentUserKey);
      await prefs.remove(_userIdKey);
      await prefs.remove(_userNameKey);
      await prefs.remove(_firstNameKey);
      await prefs.remove(_lastNameKey);
    } catch (e) {
      print('❌ Error clearing stored session: $e');
    }
  }

  /// Start session monitoring
  void _startSessionMonitoring() {
    _sessionTimeoutTimer?.cancel();
    _sessionTimeoutTimer = Timer.periodic(
      const Duration(minutes: 1),
      (timer) => _checkSessionTimeout(),
    );
  }

  /// Check for session timeout
  void _checkSessionTimeout() async {
    if (_currentState != SessionState.loggedIn || _lastActivityTime == null) {
      return;
    }

    final now = DateTime.now();
    final timeSinceLastActivity = now.difference(_lastActivityTime!);
    
    if (timeSinceLastActivity > _defaultSessionTimeout) {
      print('Session timed out due to inactivity');
      await logout();
      _currentState = SessionState.timedOut;
    }
  }

  /// Record user activity to reset timeout
  void recordActivity() {
    if (_currentState == SessionState.loggedIn) {
      _lastActivityTime = DateTime.now();
    }
  }

  // Getters
  LoggedInUserModel? get currentUser => _currentUser;
  String? get authToken => _authToken;
  bool get isLoggedIn => _currentState == SessionState.loggedIn;
  SessionState get sessionState => _currentState;
  
  /// Get current user ID
  Future<int?> getUserId() async {
    // Return cached value if available
    if (_cachedUserId != null && _cachedUserId! > 0) {
      return _cachedUserId;
    }
    
    // If current user is available, use that
    if (_currentUser?.userId != null) {
      _cachedUserId = _currentUser!.userId;
      return _cachedUserId;
    }
    
    try {
      final prefs = await SharedPreferences.getInstance();
      final userId = prefs.getInt(_userIdKey);
      final validUserId = userId != 0 ? userId : null;
      
      // Cache the result for future calls
      _cachedUserId = validUserId;
      
      return validUserId;
    } catch (e) {
      print('❌ Error getting user ID: $e');
      return null;
    }
  }

  /// Get stored user data
  Future<Map<String, dynamic>?> getUserData() async {
    if (_cachedUserData != null) {
      return _cachedUserData;
    }

    try {
      final prefs = await SharedPreferences.getInstance();
      final userDataString = prefs.getString(_currentUserKey);
      
      if (userDataString != null) {
        _cachedUserData = jsonDecode(userDataString);
        return _cachedUserData;
      }
      
      return null;
    } catch (e) {
      print('❌ Error retrieving user data: $e');
      return null;
    }
  }

  /// Get session info
  SessionInfo getSessionInfo() {
    return SessionInfo(
      isLoggedIn: _currentState == SessionState.loggedIn,
      state: _currentState,
      lastActivityTime: _lastActivityTime,
    );
  }
}

/// Session state enumeration
enum SessionState {
  loggedOut,
  loggedIn,
  expired,
  timedOut,
  error,
}

/// Session information model
class SessionInfo {
  final bool isLoggedIn;
  final SessionState state;
  final DateTime? lastActivityTime;

  const SessionInfo({
    required this.isLoggedIn,
    required this.state,
    this.lastActivityTime,
  });
}

/// Authentication result wrapper
class AuthResult {
  final bool isSuccess;
  final LoggedInUserModel? user;
  final String? token;
  final String message;

  const AuthResult._({
    required this.isSuccess,
    this.user,
    this.token,
    required this.message,
  });

  factory AuthResult.success({
    required LoggedInUserModel user,
    required String token,
    required String message,
  }) {
    return AuthResult._(
      isSuccess: true,
      user: user,
      token: token,
      message: message,
    );
  }

  factory AuthResult.failure({required String message}) {
    return AuthResult._(
      isSuccess: false,
      message: message,
    );
  }
}

/// Auth interceptor to add token to requests
class _AuthInterceptor implements Interceptor {
  @override
  FutureOr<Response<BodyType>> intercept<BodyType>(Chain<BodyType> chain) async {
    final request = chain.request;
    
    try {
      // Get auth token from the unified service
      final authService = UnifiedAuthService();
      final authToken = authService.authToken;
      
      if (authToken != null && authToken.isNotEmpty) {
        final updatedRequest = request.copyWith(
          headers: {
            ...request.headers,
            'Authorization': 'Bearer $authToken',
          },
        );
        return chain.proceed(updatedRequest);
      }
    } catch (e) {
      print('❌ Error getting auth token: $e');
    }
    
    return chain.proceed(request);
  }
}