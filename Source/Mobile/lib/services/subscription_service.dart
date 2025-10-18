import 'dart:async';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:chopper/chopper.dart';
import '../api/generated/code/manifestation.swagger.dart';
import 'unified_auth_service.dart';

/// Service to manage subscription status and usage limits
class SubscriptionService {
  static final SubscriptionService _instance = SubscriptionService._internal();
  factory SubscriptionService() => _instance;
  SubscriptionService._internal();

  static const String _premiumKey = 'is_premium_user';
  static const String _subscriptionExpiryKey = 'subscription_expiry';
  static const String _lastCheckKey = 'last_premium_check';
  static const String _premiumUserIdKey = 'premium_user_id';
  
  bool _isPremium = false;
  DateTime? _subscriptionExpiry;
  DateTime? _lastServerCheck;
  int? _premiumUserId;
  
  Manifestation? _apiClient;
  UnifiedAuthService? _authService;
  bool _isInitialized = false;
  
  // Free tier limits
  static const int maxFreeCustomAffirmationCategories = 3; // User can create 3 custom categories on top of 5 default ones
  static const int maxFreeAffirmationsPerCategory = 3;
  static const int maxFreeVisionBoards = 3;
  static const int maxFreeGoals = 5;
  static const int maxFreeMilestonesPerGoal = 2;
  static const int maxFreeGratitudes = 3;
  static const int maxFreeMeditationCategories = 3;
  static const int maxFreeMeditationDesignsPerCategory = 3;
  
  /// Initialize the subscription service
  Future<void> initialize({String? baseUrl}) async {
    // Only initialize once to prevent multiple initialization messages
    if (_isInitialized) {
      print('📱 SubscriptionService already initialized, skipping...');
      return;
    }
    
    // Initialize API client if baseUrl provided
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
    
    await _loadSubscriptionStatus();
    
    // Check with server if we haven't checked recently (every 24 hours)
    if (_shouldCheckWithServer()) {
      await checkPremiumStatusFromServer();
    }
    
    _isInitialized = true;
  }
  
  /// Check if we should verify premium status with server
  bool _shouldCheckWithServer() {
    if (_lastServerCheck == null) return true;
    final hoursSinceLastCheck = DateTime.now().difference(_lastServerCheck!).inHours;
    return hoursSinceLastCheck >= 24;
  }
  
  /// Load subscription status from local storage
  Future<void> _loadSubscriptionStatus() async {
    try {
      final prefs = await SharedPreferences.getInstance();
      
      // Get current user ID from auth service
      final currentUserId = _authService?.userId;
      final storedPremiumUserId = prefs.getInt(_premiumUserIdKey);
      
      // Check if the stored premium status belongs to the current user
      if (currentUserId != null && storedPremiumUserId != null && currentUserId == storedPremiumUserId) {
        _isPremium = prefs.getBool(_premiumKey) ?? false;
        _premiumUserId = storedPremiumUserId;
        
        final expiryString = prefs.getString(_subscriptionExpiryKey);
        if (expiryString != null) {
          _subscriptionExpiry = DateTime.parse(expiryString);
          
          // Check if subscription has expired
          if (_subscriptionExpiry != null && _subscriptionExpiry!.isBefore(DateTime.now())) {
            _isPremium = false;
            await _saveSubscriptionStatus();
          }
        }
        
        final lastCheckString = prefs.getString(_lastCheckKey);
        if (lastCheckString != null) {
          _lastServerCheck = DateTime.parse(lastCheckString);
        }
        
        print('📱 Subscription Service initialized for user $currentUserId:');
        print('   Is Premium: $_isPremium');
        print('   Expiry: $_subscriptionExpiry');
        print('   Last Server Check: $_lastServerCheck');
      } else {
        // Different user or no user logged in, reset premium status
        print('📱 Different user detected (current: $currentUserId, stored: $storedPremiumUserId), resetting premium status');
        _isPremium = false;
        _subscriptionExpiry = null;
        _lastServerCheck = null;
        _premiumUserId = currentUserId;
        
        // Clear the stored premium data since it belongs to a different user
        if (currentUserId != storedPremiumUserId) {
          await clearPremiumStatus();
        }
      }
    } catch (e) {
      print('❌ Error loading subscription status: $e');
      _isPremium = false;
    }
  }
  
  /// Save subscription status to local storage
  Future<void> _saveSubscriptionStatus() async {
    try {
      final prefs = await SharedPreferences.getInstance();
      await prefs.setBool(_premiumKey, _isPremium);
      
      // Save the user ID associated with this premium status
      final currentUserId = _authService?.userId;
      if (currentUserId != null) {
        await prefs.setInt(_premiumUserIdKey, currentUserId);
        _premiumUserId = currentUserId;
      }
      
      if (_subscriptionExpiry != null) {
        await prefs.setString(_subscriptionExpiryKey, _subscriptionExpiry!.toIso8601String());
      } else {
        await prefs.remove(_subscriptionExpiryKey);
      }
      
      if (_lastServerCheck != null) {
        await prefs.setString(_lastCheckKey, _lastServerCheck!.toIso8601String());
      }
    } catch (e) {
      print('❌ Error saving subscription status: $e');
    }
  }
  
  /// Enable premium feature using server API
  Future<bool> enablePremiumFeature({String? password, int validityYears = 1}) async {
    try {
      print('🔄 Enabling premium feature on server...');
      
      if (_apiClient == null || _authService == null) {
        print('⚠️ API client not initialized, using local mock activation');
        await activatePremium(expiryDate: DateTime.now().add(Duration(days: validityYears * 365)));
        return true;
      }
      
      final enableModel = EnablePremiumFeatureModel(
        password: password,
        validityYears: validityYears,
      );
      
      final response = await _apiClient!.apiV1CompanyEnablepremiumfeaturePost(
        body: enableModel,
      );
      
      if (response.isSuccessful && response.body?.isSuccess == true) {
        print('✅ Premium feature enabled on server');
        
        // Update local status
        final expiryDate = DateTime.now().add(Duration(days: validityYears * 365));
        await activatePremium(expiryDate: expiryDate);
        
        _lastServerCheck = DateTime.now();
        await _saveSubscriptionStatus();
        
        return true;
      } else {
        print('❌ Failed to enable premium feature:');
        print('   Status: ${response.statusCode}');
        print('   Success: ${response.body?.isSuccess}');
        print('   Message: ${response.body?.successMessage}');
        print('   Error: ${response.body?.errorHolder}');
        return false;
      }
    } catch (e) {
      print('❌ Error enabling premium feature: $e');
      return false;
    }
  }
  
  /// Check premium status from server (if API for checking exists)
  Future<void> checkPremiumStatusFromServer() async {
    try {
      print('🔄 Checking premium status with server...');
      
      if (_apiClient == null || _authService == null) {
        print('⚠️ API client not initialized, skipping server check');
        return;
      }
      
      // Note: The ispremiumfeatureenabled endpoint doesn't seem to exist in the swagger
      // For now, we'll just update the last check time and rely on local status
      // In a real implementation, you would call the server to verify status
      
      _lastServerCheck = DateTime.now();
      await _saveSubscriptionStatus();
      
      print('✅ Server check completed (using local status)');
      
    } catch (e) {
      print('❌ Error checking premium status from server: $e');
    }
  }
  
  /// Check if user has premium subscription
  bool get isPremium => _isPremium;
  
  /// Get subscription expiry date
  DateTime? get subscriptionExpiry => _subscriptionExpiry;
  
  /// Set mock premium status for testing (only for development/testing)
  Future<void> setMockPremiumStatus(bool isPremium) async {
    try {
      _isPremium = isPremium;
      
      if (isPremium) {
        // Set expiry to 1 year from now for mock premium
        _subscriptionExpiry = DateTime.now().add(Duration(days: 365));
      } else {
        _subscriptionExpiry = null;
      }
      
      await _saveSubscriptionStatus();
      
      print('🧪 Mock Premium Status Updated:');
      print('   Is Premium: $_isPremium');
      print('   Mock Expiry: $_subscriptionExpiry');
    } catch (e) {
      print('❌ Error setting mock premium status: $e');
    }
  }
  
  /// Activate premium subscription
  Future<void> activatePremium({DateTime? expiryDate}) async {
    _isPremium = true;
    _subscriptionExpiry = expiryDate;
    await _saveSubscriptionStatus();
    print('✅ Premium subscription activated until: $expiryDate');
  }
  
  /// Deactivate premium subscription
  Future<void> deactivatePremium() async {
    _isPremium = false;
    _subscriptionExpiry = null;
    await _saveSubscriptionStatus();
    print('❌ Premium subscription deactivated');
  }
  
  // Usage limit checks for different features
  
  /// Check if user can create more affirmation categories
  /// Only counts custom categories (default categories don't count against limit)
  bool canCreateAffirmationCategory(int currentCustomCount) {
    if (isPremium) return true;
    return currentCustomCount < maxFreeCustomAffirmationCategories;
  }
  
  /// Check if user can create more affirmations in a category
  bool canCreateAffirmation(int currentCountInCategory) {
    if (isPremium) return true;
    return currentCountInCategory < maxFreeAffirmationsPerCategory;
  }
  
  /// Check if user can create more vision boards
  bool canCreateVisionBoard(int currentCount) {
    if (isPremium) return true;
    return currentCount < maxFreeVisionBoards;
  }
  
  /// Check if user can create more goals
  bool canCreateGoal(int currentCount) {
    if (isPremium) return true;
    return currentCount < maxFreeGoals;
  }
  
  /// Check if user can create more milestones for a goal
  bool canCreateMilestone(int currentMilestonesInGoal) {
    if (isPremium) return true;
    return currentMilestonesInGoal < maxFreeMilestonesPerGoal;
  }
  
  /// Check if user can add more gratitudes
  bool canAddGratitude(int currentCount) {
    if (isPremium) return true;
    return currentCount < maxFreeGratitudes;
  }
  
  /// Check if user can access daily reflection feature
  bool canAccessDailyReflection() {
    return isPremium;
  }
  
  /// Check if user can access tomorrow's intention feature
  bool canAccessTomorrowsIntention() {
    return isPremium;
  }
  
  /// Check if user can access meditation category
  bool canAccessMeditationCategory(int categoryIndex) {
    if (isPremium) return true;
    return categoryIndex < maxFreeMeditationCategories;
  }
  
  /// Check if user can access meditation design
  bool canAccessMeditationDesign(int categoryIndex, int designIndex) {
    if (isPremium) return true;
    if (categoryIndex >= maxFreeMeditationCategories) return false;
    return designIndex < maxFreeMeditationDesignsPerCategory;
  }
  
  /// Get upgrade message for specific feature
  String getUpgradeMessage(String feature) {
    switch (feature) {
      case 'affirmation_category':
        return 'Upgrade to Premium to create more than $maxFreeCustomAffirmationCategories custom affirmation categories!';
      case 'affirmation':
        return 'Upgrade to Premium to add more than $maxFreeAffirmationsPerCategory affirmations per category!';
      case 'vision_board':
        return 'Upgrade to Premium to create more than $maxFreeVisionBoards vision boards!';
      case 'goal':
        return 'Upgrade to Premium to create more than $maxFreeGoals goals!';
      case 'milestone':
        return 'Upgrade to Premium to add more than $maxFreeMilestonesPerGoal milestones per goal!';
      case 'gratitude':
        return 'Upgrade to Premium to add more than $maxFreeGratitudes gratitudes!';
      case 'daily_reflection':
        return 'Upgrade to Premium to access daily reflection feature!';
      case 'tomorrows_intention':
        return 'Upgrade to Premium to set tomorrow\'s intentions!';
      case 'meditation':
        return 'Upgrade to Premium to access all meditation categories and designs!';
      default:
        return 'Upgrade to Premium to unlock all features!';
    }
  }
  
  /// Show upgrade dialog (to be implemented with actual payment integration)
  void showUpgradeDialog() {
    // TODO: Implement upgrade dialog with payment integration
    print('🚀 Show upgrade dialog - integrate with payment provider');
  }
  
  /// Get subscription status summary
  Map<String, dynamic> getSubscriptionSummary() {
    return {
      'isPremium': _isPremium,
      'subscriptionExpiry': _subscriptionExpiry?.toIso8601String(),
      'freeLimits': {
        'customAffirmationCategories': maxFreeCustomAffirmationCategories,
        'affirmationsPerCategory': maxFreeAffirmationsPerCategory,
        'visionBoards': maxFreeVisionBoards,
        'goals': maxFreeGoals,
        'milestonesPerGoal': maxFreeMilestonesPerGoal,
        'gratitudes': maxFreeGratitudes,
        'meditationCategories': maxFreeMeditationCategories,
        'meditationDesignsPerCategory': maxFreeMeditationDesignsPerCategory,
      }
    };
  }
  
  /// Clear premium status (used on logout)
  Future<void> clearPremiumStatus() async {
    try {
      print('🧹 Clearing premium status for logout...');
      
      // Reset in-memory state
      _isPremium = false;
      _subscriptionExpiry = null;
      _lastServerCheck = null;
      _premiumUserId = null;
      
      // Clear from SharedPreferences
      final prefs = await SharedPreferences.getInstance();
      await prefs.remove(_premiumKey);
      await prefs.remove(_subscriptionExpiryKey);
      await prefs.remove(_lastCheckKey);
      await prefs.remove(_premiumUserIdKey);
      
      print('✅ Premium status cleared');
    } catch (e) {
      print('❌ Error clearing premium status: $e');
    }
  }
  
  /// Refresh premium status for a new user login
  Future<void> refreshForNewUser() async {
    try {
      print('🔄 Refreshing subscription service for new user...');
      
      // Reload subscription status which will check if it belongs to current user
      await _loadSubscriptionStatus();
      
      // Check with server for the new user's premium status
      await checkPremiumStatusFromServer();
      
      print('✅ Subscription service refreshed for new user');
    } catch (e) {
      print('❌ Error refreshing subscription service: $e');
    }
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
        return await chain.proceed(updatedRequest);
      } else {
        return await chain.proceed(request);
      }
    } catch (e) {
      print('❌ Auth interceptor error: $e');
      rethrow;
    }
  }
}