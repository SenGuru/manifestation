import 'dart:async';
import 'dart:convert';
import 'package:chopper/chopper.dart';
import 'package:shared_preferences/shared_preferences.dart';
import '../api/generated/code/manifestation.swagger.dart';
import '../services/daily_affirmations_service.dart';

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
  static const String _userEmailKey = 'user_email';
  
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
      
      // Try multiple API URLs in order of preference
      final apiUrls = [
        baseUrl,
        'http://manifestation-qa-1.senthilguru.net',
        'http://24.199.119.33',
        'https://manifestation-qa-1.senthilguru.net',
        'https://24.199.119.33'
      ].where((url) => url != null).cast<String>().toList();
      
      String? workingUrl;
      for (final url in apiUrls) {
        try {
          print('🔄 Trying API URL: $url');
          _apiClient = Manifestation.create(
            baseUrl: Uri.parse(url),
            interceptors: [_AuthInterceptor()],
          );
          
          // Test the connection with a simple ping
          // We'll consider initialization successful and test during actual API calls
          workingUrl = url;
          print('✅ API client initialized with: $url');
          break;
        } catch (e) {
          print('❌ Failed to initialize with $url: $e');
          continue;
        }
      }
      
      if (workingUrl == null) {
        throw Exception('Failed to initialize API client with any URL');
      }
      
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
          
          // Create userData with the email we know the user has
          final userData = _currentUser!.toJson();
          userData['email'] = username; // Store the email/username used for login
          
          // Save session with all data including email
          await _saveSession(
            token: _authToken!,
            userData: userData,
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
      print('🔐=== ACCOUNT CREATION DEBUG START ===');
      print('🔐 Attempting account creation for: $email');
      print('🔐 First Name: $firstName');
      print('🔐 Last Name: $lastName');
      print('🔐 Password Length: ${password.length}');
      print('🔐 Captcha Code: $captchaCode');
      print('🔐 Remember Me: $rememberMe');
      print('🔐 API Base URL: [Multiple URLs attempted - check initialization logs]');

      final createModel = AccountCreateModel(
        email: email,
        firstName: firstName,
        lastName: lastName,
        initialPassword: password,
        captchaCode: captchaCode,
      );

      print('🔐 Created AccountCreateModel:');
      print('🔐   - email: ${createModel.email}');
      print('🔐   - firstName: ${createModel.firstName}');
      print('🔐   - lastName: ${createModel.lastName}');
      print('🔐   - initialPassword: ${createModel.initialPassword?.isNotEmpty == true ? "[PRESENT]" : "[MISSING]"}');
      print('🔐   - captchaCode: ${createModel.captchaCode}');

      print('🔐 Making API call to: apiV1AccountCreateaccountPost');
      final response = await _apiClient.apiV1AccountCreateaccountPost(body: createModel);

      print('🔐=== API RESPONSE DEBUG ===');
      print('🔐 Response Status Code: ${response.statusCode}');
      print('🔐 Response isSuccessful: ${response.isSuccessful}');
      print('🔐 Response hasBody: ${response.body != null}');
      print('🔐 Raw Response: ${response.bodyString}');
      
      if (response.headers.isNotEmpty) {
        print('🔐 Response Headers:');
        response.headers.forEach((key, value) {
          print('🔐   $key: $value');
        });
      }

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        print('🔐=== RETURN MODEL DEBUG ===');
        print('🔐 Return Model Type: ${returnModel.runtimeType}');
        print('🔐 Return Model isSuccess: ${returnModel.isSuccess}');
        print('🔐 Return Model hasModel: ${returnModel.model != null}');
        print('🔐 Return Model successMessage: ${returnModel.successMessage}');
        print('🔐 Return Model errorHolder: ${returnModel.errorHolder}');
        
        // Check for session codes or additional debug info
        try {
          final rawResponse = response.bodyString;
          if (rawResponse != null && rawResponse.contains('sessionCode')) {
            final sessionCodeMatch = RegExp(r'"sessionCode"\s*:\s*"([^"]*)"').firstMatch(rawResponse);
            if (sessionCodeMatch != null) {
              print('🔐 SESSION CODE FOUND: ${sessionCodeMatch.group(1)}');
            }
          }
          if (rawResponse != null && rawResponse.contains('traceId')) {
            final traceIdMatch = RegExp(r'"traceId"\s*:\s*"([^"]*)"').firstMatch(rawResponse);
            if (traceIdMatch != null) {
              print('🔐 TRACE ID FOUND: ${traceIdMatch.group(1)}');
            }
          }
          if (rawResponse != null && rawResponse.contains('requestId')) {
            final requestIdMatch = RegExp(r'"requestId"\s*:\s*"([^"]*)"').firstMatch(rawResponse);
            if (requestIdMatch != null) {
              print('🔐 REQUEST ID FOUND: ${requestIdMatch.group(1)}');
            }
          }
        } catch (e) {
          print('🔐 Error parsing session codes: $e');
        }
        
        if (returnModel.model != null) {
          print('🔐 User Model Details:');
          print('🔐   - userId: ${returnModel.model!.userId}');
          print('🔐   - userName: ${returnModel.model!.userName}');
          print('🔐   - firstName: ${returnModel.model!.firstName}');
          print('🔐   - lastName: ${returnModel.model!.lastName}');
          print('🔐   - email: ${email}');
          print('🔐   - hasToken: ${returnModel.model!.token?.isNotEmpty == true}');
          print('🔐   - tokenLength: ${returnModel.model!.token?.length ?? 0}');
        }
        
        if (returnModel.isSuccess == true && returnModel.model != null) {
          _currentUser = returnModel.model!;
          _authToken = returnModel.model!.token;
          
          print('🔐 SUCCESS: Setting current user and token');
          print('🔐 Current User ID: ${_currentUser!.userId}');
          print('🔐 Current User Name: ${_currentUser!.userName}');
          print('🔐 Auth Token Present: ${_authToken?.isNotEmpty == true}');
          
          // Create userData with the email we know the user provided
          final userData = _currentUser!.toJson();
          userData['email'] = email; // Store the email provided during account creation
          
          // Save session with all data including email
          print('🔐 Saving session data with email: $email');
          await _saveSession(
            token: _authToken!,
            userData: userData,
            rememberMe: rememberMe,
          );
          
          print('✅ Account creation successful for: ${_currentUser!.userName}');
          
          // Create hidden categories in background (don't await to avoid blocking)
          _createHiddenCategoriesForNewUser().then((_) {
            print('✅ Hidden categories created in background');
          }).catchError((e) {
            print('❌ Failed to create hidden categories in background: $e');
          });
          
          print('🔐=== ACCOUNT CREATION DEBUG END (SUCCESS) ===');
          return AuthResult.success(
            user: _currentUser!,
            token: _authToken!,
            message: returnModel.successMessage ?? 'Account created successfully',
          );
        } else {
          print('❌ FAILURE: Return model indicates failure');
          print('❌ isSuccess: ${returnModel.isSuccess}');
          print('❌ model is null: ${returnModel.model == null}');
          print('❌ errorHolder: ${returnModel.errorHolder}');
          print('🔐=== ACCOUNT CREATION DEBUG END (API FAILURE) ===');
          return AuthResult.failure(
            message: returnModel.errorHolder?.toString() ?? 'Account creation failed',
          );
        }
      } else {
        print('❌ NETWORK ERROR:');
        print('❌ Status Code: ${response.statusCode}');
        print('❌ isSuccessful: ${response.isSuccessful}');
        print('❌ Response Body: ${response.bodyString}');
        print('❌ Error: ${response.error}');
        print('🔐=== ACCOUNT CREATION DEBUG END (NETWORK ERROR) ===');
        return AuthResult.failure(
          message: 'Network error: ${response.statusCode} - ${response.bodyString ?? "No response body"}',
        );
      }
      
    } catch (e, stackTrace) {
      print('❌=== ACCOUNT CREATION EXCEPTION ===');
      print('❌ Exception Type: ${e.runtimeType}');
      print('❌ Exception Message: $e');
      print('❌ Stack Trace:');
      print(stackTrace);
      print('🔐=== ACCOUNT CREATION DEBUG END (EXCEPTION) ===');
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
      
      // Cache clearing removed - now using direct API calls
      
      // Reset onboarding and tour flags for new users
      final prefs = await SharedPreferences.getInstance();
      print('🔥 LOGOUT: Clearing onboarding and tour flags...');
      await prefs.remove('post_signup_onboarding_completed');
      await prefs.remove('vision_board_tour_completed');
      await prefs.remove('affirmations_tour_completed');
      await prefs.remove('journal_tour_completed');
      await prefs.remove('goal_tracker_tour_completed');
      await prefs.remove('meditation_tour_completed');
      print('🔥 LOGOUT: All tour flags cleared');
      
      // Stop session monitoring
      _sessionTimeoutTimer?.cancel();
      
      print('✅ Logout successful (was: $previousState) - all caches cleared');
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
      // Store email if present in userData
      if (userData['email'] != null && userData['email'].toString().isNotEmpty) {
        await prefs.setString(_userEmailKey, userData['email']);
      }
      
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
      
      // IMPORTANT: Validate session with server before allowing login
      print('🔐 Validating session with server...');
      final validationResult = await _validateSessionWithServer();
      
      if (validationResult == true) {
        // Session is valid, restore it
        _currentState = SessionState.loggedIn;
        _lastActivityTime = DateTime.now();
        
        // Update the is_logged_in flag if it wasn't set (for future restarts)
        if (!isLoggedIn) {
          print('🔧 Updating is_logged_in flag for future restarts');
          await prefs.setBool(_isLoggedInKey, true);
        }
        
        print('✅ Session successfully restored and validated with server');
      } else if (validationResult == null) {
        // Network error - can't validate session, keep it for retry
        print('🌐 Cannot validate session due to network issues');
        print('💡 Keeping session for retry when network is restored');
        _currentState = SessionState.networkError;
        _lastActivityTime = DateTime.now();
        
        // Keep the session data but mark as network error state
        // Don't clear the session - user can retry
      } else {
        // Session is invalid (401/403 from server)
        print('❌ Session validation failed - token is invalid, clearing stored session');
        await _clearStoredSession();
        _currentState = SessionState.loggedOut;
      }
      
    } catch (e) {
      print('❌ Error checking stored session: $e');
      _currentState = SessionState.loggedOut;
    }
  }

  /// Validate session with server to ensure it's still active and network is available
  /// Returns: null for network error, true for valid session, false for invalid session
  Future<bool?> _validateSessionWithServer() async {
    try {
      if (_apiClient == null || _authToken == null) {
        print('❌ No API client or auth token for validation');
        return false;
      }
      
      // Try a simple API call that requires authentication
      // We'll use a lightweight endpoint that just checks if the token is valid
      final response = await _apiClient!.apiV1CompanyGetuserpictureGet(
        userId: _currentUser?.userId,
      );
      
      // Analyze response to determine if it's an auth issue or server issue
      if (response.statusCode == 401) {
        // 401 Unauthorized - token is definitely invalid
        print('🔐 Session validation result: INVALID - Token unauthorized (status: 401)');
        return false;
      } else if (response.statusCode == 403) {
        // 403 Forbidden - could be server config issue or invalid token
        // Since user wants to treat this as connectivity, we'll return null
        print('🔐 Session validation result: SERVER_ISSUE - 403 Forbidden (treating as connectivity issue)');
        return null; // Treat as network/server issue
      } else if (response.statusCode >= 200 && response.statusCode < 300) {
        // Success response - session is valid
        print('🔐 Session validation result: VALID (status: ${response.statusCode})');
        return true;
      } else if (response.statusCode == 404) {
        // 404 Not Found - endpoint exists but resource doesn't, auth worked
        print('🔐 Session validation result: VALID - Auth worked, resource not found (status: 404)');
        return true;
      } else {
        // Other status codes (500, etc.) - treat as server issues
        print('🔐 Session validation result: SERVER_ISSUE - Unexpected status ${response.statusCode}');
        return null; // Treat as network/server issue
      }
    } catch (e) {
      print('❌ Session validation failed: $e');
      
      // Check if this is a network error
      final errorMessage = e.toString().toLowerCase();
      if (errorMessage.contains('socket') || 
          errorMessage.contains('network') ||
          errorMessage.contains('failed host lookup') ||
          errorMessage.contains('connection refused') ||
          errorMessage.contains('timeout')) {
        print('🌐 Network unavailable - cannot validate session');
        print('💡 This is a connectivity issue, not an auth issue');
        return null; // null indicates network error
      }
      
      // Other errors (non-network) should be treated as invalid session
      return false;
    }
  }

  /// Retry session validation (for when network is restored)
  Future<bool> retrySessionValidation() async {
    print('🔄 Retrying session validation...');
    
    try {
      final validationResult = await _validateSessionWithServer();
      
      if (validationResult == true) {
        print('✅ Session validation retry successful - restoring logged in state');
        _currentState = SessionState.loggedIn;
        _lastActivityTime = DateTime.now();
        return true;
      } else if (validationResult == null) {
        print('🌐 Still cannot connect to server for validation');
        _currentState = SessionState.networkError;
        return false;
      } else {
        print('❌ Session validation retry failed - token is invalid');
        await _clearStoredSession();
        _currentState = SessionState.loggedOut;
        return false;
      }
    } catch (e) {
      print('❌ Error during session validation retry: $e');
      _currentState = SessionState.networkError;
      return false;
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
      await prefs.remove(_userEmailKey);
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
        // Also ensure email is in the cached data if stored separately
        final storedEmail = prefs.getString(_userEmailKey);
        if (storedEmail != null && storedEmail.isNotEmpty) {
          _cachedUserData!['email'] = storedEmail;
        }
        return _cachedUserData;
      }
      
      return null;
    } catch (e) {
      print('❌ Error retrieving user data: $e');
      return null;
    }
  }
  
  /// Get user email from storage
  Future<String?> getUserEmail() async {
    try {
      final prefs = await SharedPreferences.getInstance();
      return prefs.getString(_userEmailKey);
    } catch (e) {
      print('❌ Error getting user email: $e');
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

  /// Update user data in storage (for profile updates)
  Future<void> updateUserData(Map<String, dynamic> userData) async {
    try {
      print('🔄 Updating user data in auth service...');
      
      await _saveSession(
        token: _authToken ?? '',
        userData: userData,
        rememberMe: true, // Preserve existing session settings
      );
      
      // Also update the current user object if possible
      try {
        _currentUser = LoggedInUserModel.fromJson(userData);
        print('✅ Updated current user object');
      } catch (e) {
        print('⚠️ Could not update current user object: $e');
        // Continue anyway - the storage update is more important
      }
      
      print('✅ User data updated successfully in auth service');
    } catch (e) {
      print('❌ Error updating user data in auth service: $e');
      throw e; // Re-throw to let caller handle it
    }
  }

  /// Create hidden categories for a new user account
  Future<void> _createHiddenCategoriesForNewUser() async {
    try {
      print('🔧 Creating hidden categories for new user...');
      final affirmationService = DailyAffirmationsService();
      await affirmationService.createHiddenCategoriesForNewUser();
      print('✅ Hidden categories created successfully');
    } catch (e) {
      print('❌ Error creating hidden categories for new user: $e');
      // Don't throw error - this is optional functionality that shouldn't block account creation
    }
  }
}

/// Session state enumeration
enum SessionState {
  loggedOut,
  loggedIn,
  expired,
  timedOut,
  error,
  networkError, // Can't validate session due to connectivity issues
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
    
    print('🌐=== HTTP REQUEST DEBUG ===');
    print('🌐 Method: ${request.method}');
    print('🌐 URL: ${request.url}');
    print('🌐 Headers: ${request.headers}');
    print('🌐 Body Type: ${request.body.runtimeType}');
    
    try {
      // Get auth token from the unified service
      final authService = UnifiedAuthService();
      final authToken = authService.authToken;
      
      print('🌐 Auth token available: ${authToken != null && authToken.isNotEmpty}');
      if (authToken != null && authToken.isNotEmpty) {
        print('🌐 Auth token length: ${authToken.length}');
      }
      
      if (authToken != null && authToken.isNotEmpty) {
        final updatedRequest = request.copyWith(
          headers: {
            ...request.headers,
            'Authorization': 'Bearer $authToken',
          },
        );
        print('🌐 Added Authorization header');
        final response = await chain.proceed(updatedRequest);
        
        print('🌐=== HTTP RESPONSE DEBUG ===');
        print('🌐 Status: ${response.statusCode}');
        print('🌐 Headers: ${response.headers}');
        print('🌐 Body: ${response.bodyString}');
        
        return response;
      } else {
        print('🌐 No auth token, proceeding without Authorization header');
        final response = await chain.proceed(request);
        
        print('🌐=== HTTP RESPONSE DEBUG ===');
        print('🌐 Status: ${response.statusCode}');
        print('🌐 Headers: ${response.headers}');
        print('🌐 Body: ${response.bodyString}');
        
        return response;
      }
    } catch (e, stackTrace) {
      print('❌=== HTTP INTERCEPTOR ERROR ===');
      print('❌ Error type: ${e.runtimeType}');
      print('❌ Error message: $e');
      print('❌ Stack trace:');
      print(stackTrace);
      rethrow;
    }
  }
}