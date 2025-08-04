import 'dart:async';
import 'package:shared_preferences/shared_preferences.dart';

/// Service to manage subscription status and usage limits
class SubscriptionService {
  static final SubscriptionService _instance = SubscriptionService._internal();
  factory SubscriptionService() => _instance;
  SubscriptionService._internal();

  static const String _premiumKey = 'is_premium_user';
  static const String _subscriptionExpiryKey = 'subscription_expiry';
  
  bool _isPremium = false;
  DateTime? _subscriptionExpiry;
  
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
  Future<void> initialize() async {
    await _loadSubscriptionStatus();
  }
  
  /// Load subscription status from local storage
  Future<void> _loadSubscriptionStatus() async {
    try {
      final prefs = await SharedPreferences.getInstance();
      _isPremium = prefs.getBool(_premiumKey) ?? false;
      
      final expiryString = prefs.getString(_subscriptionExpiryKey);
      if (expiryString != null) {
        _subscriptionExpiry = DateTime.parse(expiryString);
        
        // Check if subscription has expired
        if (_subscriptionExpiry != null && _subscriptionExpiry!.isBefore(DateTime.now())) {
          _isPremium = false;
          await _saveSubscriptionStatus();
        }
      }
      
      print('📱 Subscription Service initialized:');
      print('   Is Premium: $_isPremium');
      print('   Expiry: $_subscriptionExpiry');
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
      
      if (_subscriptionExpiry != null) {
        await prefs.setString(_subscriptionExpiryKey, _subscriptionExpiry!.toIso8601String());
      } else {
        await prefs.remove(_subscriptionExpiryKey);
      }
    } catch (e) {
      print('❌ Error saving subscription status: $e');
    }
  }
  
  /// Check if user has premium subscription
  bool get isPremium => _isPremium;
  
  /// Get subscription expiry date
  DateTime? get subscriptionExpiry => _subscriptionExpiry;
  
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
}