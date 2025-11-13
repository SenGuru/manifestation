import 'dart:async';
import 'dart:convert';
import 'package:shared_preferences/shared_preferences.dart';
import 'user_service.dart';

/// Enhanced session management with comprehensive state tracking
class SessionManager {
  // Session state keys
  static const String _isLoggedInKey = 'is_logged_in';
  static const String _authTokenKey = 'auth_token';
  static const String _tokenExpiryKey = 'token_expiry';
  static const String _lastLoginTimeKey = 'last_login_time';
  static const String _sessionIdKey = 'session_id';
  static const String _rememberMeKey = 'remember_me';
  static const String _autoLoginEnabledKey = 'auto_login_enabled';
  static const String _sessionTimeoutKey = 'session_timeout';
  
  // Session settings
  static const Duration _defaultSessionTimeout = Duration(hours: 24);
  static const Duration _rememberMeTimeout = Duration(days: 30);
  
  // In-memory session state
  static bool _isInitialized = false;
  static SessionState _currentState = SessionState.loggedOut;
  static DateTime? _lastActivityTime;
  static Timer? _sessionTimeoutTimer;
  
  /// Initialize session manager on app startup
  static Future<void> initialize() async {
    if (_isInitialized) return;
    
    try {
      print('=== SESSION MANAGER INITIALIZATION ===');
      
      // Check existing session state
      await _checkStoredSession();
      
      // Start session monitoring
      _startSessionMonitoring();
      
      _isInitialized = true;
      print('Session manager initialized with state: $_currentState');
    } catch (e) {
      print('Error initializing session manager: $e');
      _currentState = SessionState.loggedOut;
      _isInitialized = true;
    }
  }
  
  /// Check if there's a valid stored session
  static Future<void> _checkStoredSession() async {
    try {
      final prefs = await SharedPreferences.getInstance();
      
      final isLoggedIn = prefs.getBool(_isLoggedInKey) ?? false;
      final storedToken = prefs.getString(_authTokenKey);
      final tokenExpiryString = prefs.getString(_tokenExpiryKey);
      final lastLoginString = prefs.getString(_lastLoginTimeKey);
      final rememberMe = prefs.getBool(_rememberMeKey) ?? false;
      
      print('Stored session check:');
      print('- Is logged in: $isLoggedIn');
      print('- Has token: ${storedToken != null}');
      print('- Token expiry: $tokenExpiryString');
      print('- Remember me: $rememberMe');
      
      if (!isLoggedIn || storedToken == null) {
        _currentState = SessionState.loggedOut;
        return;
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
      
      // Session is valid, restore it
      // Token is now handled by ApiService during initialization
      _currentState = SessionState.loggedIn;
      _lastActivityTime = DateTime.now();
      
      print('Valid session restored');
    } catch (e) {
      print('Error checking stored session: $e');
      _currentState = SessionState.loggedOut;
    }
  }
  
  /// Start session monitoring for automatic timeout
  static void _startSessionMonitoring() {
    _sessionTimeoutTimer?.cancel();
    _sessionTimeoutTimer = Timer.periodic(Duration(minutes: 1), (timer) {
      _checkSessionTimeout();
    });
  }
  
  /// Check if session has timed out due to inactivity
  static void _checkSessionTimeout() async {
    if (_currentState != SessionState.loggedIn || _lastActivityTime == null) {
      return;
    }
    
    final prefs = await SharedPreferences.getInstance();
    final rememberMe = prefs.getBool(_rememberMeKey) ?? false;
    final inactivityTimeout = rememberMe ? Duration(hours: 2) : Duration(minutes: 30);
    
    if (DateTime.now().difference(_lastActivityTime!) > inactivityTimeout) {
      print('Session timed out due to inactivity');
      await logout(reason: 'Session timed out due to inactivity');
    }
  }
  
  /// Record user activity to reset timeout
  static void recordActivity() {
    if (_currentState == SessionState.loggedIn) {
      _lastActivityTime = DateTime.now();
    }
  }
  
  /// Login with enhanced session tracking
  static Future<bool> login({
    required String token,
    required Map<String, dynamic> userData,
    bool rememberMe = false,
    Duration? customTimeout,
  }) async {
    try {
      print('=== SESSION MANAGER LOGIN ===');
      print('Remember me: $rememberMe');
      
      final prefs = await SharedPreferences.getInstance();
      final now = DateTime.now();
      final sessionId = _generateSessionId();
      
      // Calculate token expiry
      final timeout = customTimeout ?? (rememberMe ? _rememberMeTimeout : _defaultSessionTimeout);
      final tokenExpiry = now.add(timeout);
      
      // Store session data
      await Future.wait([
        prefs.setBool(_isLoggedInKey, true),
        prefs.setString(_authTokenKey, token),
        prefs.setString(_tokenExpiryKey, tokenExpiry.toIso8601String()),
        prefs.setString(_lastLoginTimeKey, now.toIso8601String()),
        prefs.setString(_sessionIdKey, sessionId),
        prefs.setBool(_rememberMeKey, rememberMe),
        prefs.setBool(_autoLoginEnabledKey, rememberMe),
      ]);
      
      // Store user data
      await UserService.storeUserData(userData);
      
      // Set API token (handled by ApiManager)
      // Token is set via ApiManager.updateAuthToken() during initialization
      
      // Update session state
      _currentState = SessionState.loggedIn;
      _lastActivityTime = now;
      
      print('Session established successfully');
      print('- Session ID: $sessionId');
      print('- Token expires: $tokenExpiry');
      print('- Remember me: $rememberMe');
      
      return true;
    } catch (e) {
      print('Error during login session setup: $e');
      return false;
    }
  }
  
  /// Logout with comprehensive cleanup
  static Future<bool> logout({String? reason}) async {
    try {
      print('=== SESSION MANAGER LOGOUT ===');
      print('Reason: ${reason ?? "User initiated"}');
      
      // Clear stored session
      await _clearStoredSession();
      
      // Clear API token (handled by ApiManager)
      // Token clearing is now handled by ApiManager
      
      // Clear user data
      await UserService.clearUserData();
      
      // Update session state
      final previousState = _currentState;
      _currentState = SessionState.loggedOut;
      _lastActivityTime = null;
      
      // Cancel session monitoring
      _sessionTimeoutTimer?.cancel();
      
      print('Session cleared successfully');
      print('Previous state: $previousState');
      
      return true;
    } catch (e) {
      print('Error during logout: $e');
      return false;
    }
  }
  
  /// Clear stored session data
  static Future<void> _clearStoredSession() async {
    try {
      final prefs = await SharedPreferences.getInstance();
      
      await Future.wait([
        prefs.remove(_isLoggedInKey),
        prefs.remove(_authTokenKey),
        prefs.remove(_tokenExpiryKey),
        prefs.remove(_lastLoginTimeKey),
        prefs.remove(_sessionIdKey),
        prefs.remove(_rememberMeKey),
        prefs.remove(_autoLoginEnabledKey),
        prefs.remove(_sessionTimeoutKey),
      ]);
      
      print('Stored session data cleared');
    } catch (e) {
      print('Error clearing stored session: $e');
    }
  }
  
  /// Check if user is currently logged in
  static bool get isLoggedIn => _currentState == SessionState.loggedIn;
  
  /// Get current session state
  static SessionState get sessionState => _currentState;
  
  /// Check if auto-login is enabled
  static Future<bool> get isAutoLoginEnabled async {
    try {
      final prefs = await SharedPreferences.getInstance();
      return prefs.getBool(_autoLoginEnabledKey) ?? false;
    } catch (e) {
      print('Error checking auto-login status: $e');
      return false;
    }
  }
  
  /// Check if remember me was enabled
  static Future<bool> get isRememberMeEnabled async {
    try {
      final prefs = await SharedPreferences.getInstance();
      return prefs.getBool(_rememberMeKey) ?? false;
    } catch (e) {
      print('Error checking remember me status: $e');
      return false;
    }
  }
  
  /// Get current auth token
  static Future<String?> getAuthToken() async {
    try {
      final prefs = await SharedPreferences.getInstance();
      return prefs.getString(_authTokenKey);
    } catch (e) {
      print('Error getting auth token: $e');
      return null;
    }
  }
  
  /// Get session information
  static Future<SessionInfo> getSessionInfo() async {
    try {
      final prefs = await SharedPreferences.getInstance();
      
      return SessionInfo(
        isLoggedIn: isLoggedIn,
        state: _currentState,
        sessionId: prefs.getString(_sessionIdKey),
        lastLoginTime: prefs.getString(_lastLoginTimeKey) != null 
            ? DateTime.parse(prefs.getString(_lastLoginTimeKey)!)
            : null,
        tokenExpiry: prefs.getString(_tokenExpiryKey) != null
            ? DateTime.parse(prefs.getString(_tokenExpiryKey)!)
            : null,
        rememberMe: prefs.getBool(_rememberMeKey) ?? false,
        autoLoginEnabled: prefs.getBool(_autoLoginEnabledKey) ?? false,
        lastActivity: _lastActivityTime,
      );
    } catch (e) {
      print('Error getting session info: $e');
      return SessionInfo(
        isLoggedIn: false,
        state: SessionState.loggedOut,
      );
    }
  }
  
  /// Force refresh session state from storage
  static Future<void> refreshSessionState() async {
    print('Refreshing session state...');
    await _checkStoredSession();
  }
  
  /// Extend current session
  static Future<bool> extendSession({Duration? extension}) async {
    if (_currentState != SessionState.loggedIn) {
      return false;
    }
    
    try {
      final prefs = await SharedPreferences.getInstance();
      final isRememberMe = prefs.getBool(_rememberMeKey) ?? false;
      final extensionDuration = extension ?? (isRememberMe ? _rememberMeTimeout : _defaultSessionTimeout);
      
      final newExpiry = DateTime.now().add(extensionDuration);
      await prefs.setString(_tokenExpiryKey, newExpiry.toIso8601String());
      
      print('Session extended until: $newExpiry');
      return true;
    } catch (e) {
      print('Error extending session: $e');
      return false;
    }
  }
  
  /// Generate unique session ID
  static String _generateSessionId() {
    final timestamp = DateTime.now().millisecondsSinceEpoch;
    final random = (timestamp * 1000 + DateTime.now().microsecond) % 1000000;
    return 'session_${timestamp}_$random';
  }
  
  /// Clean up on app disposal
  static void dispose() {
    _sessionTimeoutTimer?.cancel();
    _isInitialized = false;
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

/// Session information data class
class SessionInfo {
  final bool isLoggedIn;
  final SessionState state;
  final String? sessionId;
  final DateTime? lastLoginTime;
  final DateTime? tokenExpiry;
  final bool rememberMe;
  final bool autoLoginEnabled;
  final DateTime? lastActivity;
  
  const SessionInfo({
    required this.isLoggedIn,
    required this.state,
    this.sessionId,
    this.lastLoginTime,
    this.tokenExpiry,
    this.rememberMe = false,
    this.autoLoginEnabled = false,
    this.lastActivity,
  });
  
  @override
  String toString() {
    return 'SessionInfo{'
        'isLoggedIn: $isLoggedIn, '
        'state: $state, '
        'sessionId: $sessionId, '
        'lastLoginTime: $lastLoginTime, '
        'tokenExpiry: $tokenExpiry, '
        'rememberMe: $rememberMe, '
        'autoLoginEnabled: $autoLoginEnabled, '
        'lastActivity: $lastActivity'
        '}';
  }
}

