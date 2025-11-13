import 'dart:convert';
import 'package:shared_preferences/shared_preferences.dart';
// import 'api_client.dart'; // Removed - using new generated API

class UserService {
  static const String _userDataKey = 'user_data';
  static const String _userIdKey = 'user_id';
  static const String _userNameKey = 'user_name';
  static const String _firstNameKey = 'first_name';
  static const String _lastNameKey = 'last_name';

  // Cached user data
  static Map<String, dynamic>? _cachedUserData;
  static int? _cachedUserId;
  
  // Singleton SharedPreferences instance to prevent redundant getInstance calls
  static SharedPreferences? _prefsInstance;
  
  /// Get or create SharedPreferences instance (singleton pattern)
  static Future<SharedPreferences> get _prefs async {
    _prefsInstance ??= await SharedPreferences.getInstance();
    return _prefsInstance!;
  }

  /// Store user data and auth token from login response
  static Future<void> storeUserData(Map<String, dynamic> userData, {String? authToken}) async {
    try {
      final prefs = await _prefs;
      
      // Store complete user data as JSON
      await prefs.setString(_userDataKey, jsonEncode(userData));
      
      // Store individual fields for quick access
      final model = userData['model'] ?? userData;
      await prefs.setInt(_userIdKey, model['userId'] ?? 0);
      await prefs.setString(_userNameKey, model['userName'] ?? '');
      await prefs.setString(_firstNameKey, model['firstName'] ?? '');
      await prefs.setString(_lastNameKey, model['lastName'] ?? '');
      
      // Cache the data
      _cachedUserData = userData;
      _cachedUserId = model['userId'];
      
      // Store auth token if provided (handled by ApiManager now)
      // Note: AuthToken is now handled by ApiManager.updateAuthToken()
      
      print('User data stored successfully');
    } catch (e) {
      print('Error storing user data: $e');
    }
  }

  /// Get stored user data
  static Future<Map<String, dynamic>?> getUserData() async {
    if (_cachedUserData != null) {
      return _cachedUserData;
    }

    try {
      final prefs = await _prefs;
      final userDataString = prefs.getString(_userDataKey);
      
      if (userDataString != null) {
        _cachedUserData = jsonDecode(userDataString);
        return _cachedUserData;
      }
      
      return null;
    } catch (e) {
      print('Error retrieving user data: $e');
      return null;
    }
  }

  /// Get current user ID (with in-memory caching for performance)
  static Future<int?> getUserId() async {
    // Return cached value if available
    if (_cachedUserId != null && _cachedUserId! > 0) {
      return _cachedUserId;
    }
    
    try {
      final prefs = await _prefs;
      final userId = prefs.getInt(_userIdKey);
      final validUserId = userId != 0 ? userId : null;
      
      // Cache the result for future calls
      _cachedUserId = validUserId;
      
      return validUserId;
    } catch (e) {
      print('Error getting user ID: $e');
      return null;
    }
  }

  /// Get current user's first name
  static Future<String?> getFirstName() async {
    try {
      final prefs = await _prefs;
      return prefs.getString(_firstNameKey);
    } catch (e) {
      print('Error getting first name: $e');
      return null;
    }
  }

  /// Get current user's last name
  static Future<String?> getLastName() async {
    try {
      final prefs = await _prefs;
      return prefs.getString(_lastNameKey);
    } catch (e) {
      print('Error getting last name: $e');
      return null;
    }
  }

  /// Get current user's full name
  static Future<String> getFullName() async {
    try {
      final firstName = await getFirstName() ?? '';
      final lastName = await getLastName() ?? '';
      
      if (firstName.isNotEmpty && lastName.isNotEmpty) {
        return '$firstName $lastName';
      } else if (firstName.isNotEmpty) {
        return firstName;
      } else if (lastName.isNotEmpty) {
        return lastName;
      } else {
        return 'User';
      }
    } catch (e) {
      print('Error getting full name: $e');
      return 'User';
    }
  }

  /// Get current username
  static Future<String?> getUserName() async {
    try {
      final prefs = await _prefs;
      return prefs.getString(_userNameKey);
    } catch (e) {
      print('Error getting username: $e');
      return null;
    }
  }

  /// Check if user is logged in
  static Future<bool> isLoggedIn() async {
    try {
      final userData = await getUserData();
      return userData != null && userData.isNotEmpty;
    } catch (e) {
      print('Error checking login status: $e');
      return false;
    }
  }

  /// Clear user data (logout) with comprehensive cleanup
  static Future<Map<String, dynamic>> clearUserData() async {
    try {
      print('=== USER SERVICE CLEANUP START ===');
      
      final prefs = await _prefs;
      
      // Log what we're about to clear
      final currentUserId = prefs.getInt(_userIdKey);
      final currentUserName = prefs.getString(_userNameKey);
      print('Clearing data for user: $currentUserName (ID: $currentUserId)');
      
      // Clear auth token first (handled by session manager now)
      // Auth token clearing is now handled by SessionManager.clearSession()
      
      // Clear all user-related data
      final clearResults = await Future.wait([
        prefs.remove(_userDataKey),
        prefs.remove(_userIdKey),
        prefs.remove(_userNameKey),
        prefs.remove(_firstNameKey),
        prefs.remove(_lastNameKey),
      ]);
      
      print('Data removal results: $clearResults');
      
      // Clear memory cache
      final hadCachedData = _cachedUserData != null;
      final hadCachedUserId = _cachedUserId != null;
      _cachedUserData = null;
      _cachedUserId = null;
      print('Memory cache cleared: data=$hadCachedData, userId=$hadCachedUserId');
      
      // Verify all data is cleared
      final verification = await _verifyDataCleared();
      
      if (verification['allCleared'] == true) {
        print('=== USER DATA CLEANUP SUCCESS ===');
        return {
          'success': true,
          'message': 'User data cleared successfully',
        };
      } else {
        print('=== USER DATA CLEANUP WARNING ===');
        print('Some data may not have been fully cleared: ${verification['remaining']}');
        return {
          'success': false,
          'message': 'Some user data may not have been fully cleared',
          'remaining': verification['remaining'],
        };
      }
      
    } catch (e) {
      print('=== USER DATA CLEANUP ERROR ===');
      print('Error clearing user data: $e');
      
      // Force clear cache even if SharedPreferences failed
      _cachedUserData = null;
      // Auth token clearing is now handled by SessionManager
      
      return {
        'success': false,
        'message': 'Error occurred while clearing user data',
        'error': e.toString(),
      };
    }
  }
  
  /// Verify that all user data has been cleared
  static Future<Map<String, dynamic>> _verifyDataCleared() async {
    try {
      final prefs = await _prefs;
      
      final remaining = <String>[];
      
      // Check each piece of user data
      if (prefs.getString(_userDataKey) != null) remaining.add('userData');
      if (prefs.getInt(_userIdKey) != null) remaining.add('userId');
      if (prefs.getString(_userNameKey) != null) remaining.add('userName');
      if (prefs.getString(_firstNameKey) != null) remaining.add('firstName');
      if (prefs.getString(_lastNameKey) != null) remaining.add('lastName');
      if (_cachedUserData != null) remaining.add('cachedData');
      
      return {
        'allCleared': remaining.isEmpty,
        'remaining': remaining,
      };
    } catch (e) {
      print('Error verifying data cleared: $e');
      return {
        'allCleared': false,
        'remaining': ['verification_error'],
        'error': e.toString(),
      };
    }
  }

  /// Update user profile information
  static Future<void> updateUserProfile({
    String? firstName,
    String? lastName,
    String? userName,
  }) async {
    try {
      final userData = await getUserData();
      if (userData != null) {
        final model = userData['model'] ?? userData;
        
        // Update the model
        if (firstName != null) model['firstName'] = firstName;
        if (lastName != null) model['lastName'] = lastName;
        if (userName != null) model['userName'] = userName;
        
        // Store updated data
        await storeUserData(userData);
      }
    } catch (e) {
      print('Error updating user profile: $e');
    }
  }

  /// Get user info for API calls that need userId
  static Future<Map<String, dynamic>> getUserInfoForApi() async {
    try {
      final userData = await getUserData();
      if (userData != null) {
        final model = userData['model'] ?? userData;
        return {
          'userId': model['userId'],
          'userName': model['userName'],
          'firstName': model['firstName'],
          'lastName': model['lastName'],
        };
      }
      return {};
    } catch (e) {
      print('Error getting user info for API: $e');
      return {};
    }
  }
}