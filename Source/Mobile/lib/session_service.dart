import 'dart:convert';
import 'package:shared_preferences/shared_preferences.dart';

class SessionService {
  static const String _sessionTokenKey = 'session_token';
  static const String _userDataKey = 'user_data';
  static const String _refreshTokenKey = 'refresh_token';
  static const String _tokenExpiryKey = 'token_expiry';

  static SessionService? _instance;
  static SessionService get instance {
    _instance ??= SessionService._();
    return _instance!;
  }

  SessionService._();

  String? _cachedToken;
  Map<String, dynamic>? _cachedUserData;

  /// Save session token and user data
  Future<void> saveSession({
    required String token,
    Map<String, dynamic>? userData,
    String? refreshToken,
    DateTime? expiryDate,
  }) async {
    final prefs = await SharedPreferences.getInstance();
    
    await prefs.setString(_sessionTokenKey, token);
    _cachedToken = token;
    
    if (userData != null) {
      await prefs.setString(_userDataKey, jsonEncode(userData));
      _cachedUserData = userData;
    }
    
    if (refreshToken != null) {
      await prefs.setString(_refreshTokenKey, refreshToken);
    }
    
    if (expiryDate != null) {
      await prefs.setString(_tokenExpiryKey, expiryDate.toIso8601String());
    }
    
    print('Session saved successfully');
  }

  /// Get current session token
  Future<String?> getSessionToken() async {
    if (_cachedToken != null) {
      return _cachedToken;
    }
    
    final prefs = await SharedPreferences.getInstance();
    _cachedToken = prefs.getString(_sessionTokenKey);
    return _cachedToken;
  }

  /// Get cached user data
  Future<Map<String, dynamic>?> getUserData() async {
    if (_cachedUserData != null) {
      return _cachedUserData;
    }
    
    final prefs = await SharedPreferences.getInstance();
    final userDataString = prefs.getString(_userDataKey);
    if (userDataString != null) {
      _cachedUserData = jsonDecode(userDataString);
      return _cachedUserData;
    }
    
    return null;
  }

  /// Get refresh token
  Future<String?> getRefreshToken() async {
    final prefs = await SharedPreferences.getInstance();
    return prefs.getString(_refreshTokenKey);
  }

  /// Check if session is valid (not expired)
  Future<bool> isSessionValid() async {
    final token = await getSessionToken();
    if (token == null) return false;
    
    final prefs = await SharedPreferences.getInstance();
    final expiryString = prefs.getString(_tokenExpiryKey);
    
    if (expiryString != null) {
      final expiryDate = DateTime.parse(expiryString);
      return DateTime.now().isBefore(expiryDate);
    }
    
    // If no expiry date, assume token is still valid
    return true;
  }

  /// Check if user is logged in
  Future<bool> isLoggedIn() async {
    return await getSessionToken() != null && await isSessionValid();
  }

  /// Clear session data (logout)
  Future<void> clearSession() async {
    final prefs = await SharedPreferences.getInstance();
    
    await prefs.remove(_sessionTokenKey);
    await prefs.remove(_userDataKey);
    await prefs.remove(_refreshTokenKey);
    await prefs.remove(_tokenExpiryKey);
    
    _cachedToken = null;
    _cachedUserData = null;
    
    print('Session cleared successfully');
  }

  /// Update user data
  Future<void> updateUserData(Map<String, dynamic> userData) async {
    final prefs = await SharedPreferences.getInstance();
    await prefs.setString(_userDataKey, jsonEncode(userData));
    _cachedUserData = userData;
  }

  /// Get user ID from cached data
  Future<int?> getUserId() async {
    final userData = await getUserData();
    return userData?['id'] as int?;
  }

  /// Get user email from cached data
  Future<String?> getUserEmail() async {
    final userData = await getUserData();
    return userData?['email'] as String?;
  }
}