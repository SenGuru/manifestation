import 'package:chopper/chopper.dart';
import 'package:flutter/material.dart';
import 'dart:async';
import '../api/generated/code/manifestation.swagger.dart';
import '../constants/goal_constants.dart';
import '../models/goal_tracker_category.dart';
import 'unified_auth_service.dart';

class GoalTrackerService {
  static final GoalTrackerService _instance = GoalTrackerService._internal();
  factory GoalTrackerService() => _instance;
  GoalTrackerService._internal();

  late Manifestation _apiClient;
  final UnifiedAuthService _authService = UnifiedAuthService();

  void initialize({String? baseUrl}) {
    _apiClient = Manifestation.create(
      baseUrl: Uri.parse(baseUrl ?? 'http://manifestation-qa-1.senthilguru.net'),
      interceptors: [_AuthInterceptor()],
    );
  }

  // ========================= LOCAL CATEGORIES =========================

  // Simple local category mapping - no API calls needed
  static final Map<int, GoalTrackerCategory> _localCategories = {
    1: GoalTrackerCategory(
      id: '1',
      name: 'Career',
      description: 'Career and work-related goals',
      color: Color(0xFF2196F3), // Blue
      iconValue: 3600007, // target
      isDefault: true,
    ),
    2: GoalTrackerCategory(
      id: '2',
      name: 'Health',
      description: 'Health and fitness goals',
      color: Color(0xFF4CAF50), // Green
      iconValue: 3600004, // leaf
      isDefault: true,
    ),
    3: GoalTrackerCategory(
      id: '3',
      name: 'Love',
      description: 'Relationship and love goals',
      color: Color(0xFFE91E63), // Pink
      iconValue: 3600002, // heart
      isDefault: true,
    ),
    4: GoalTrackerCategory(
      id: '4',
      name: 'Money',
      description: 'Financial and money goals',
      color: Color(0xFFFF9800), // Orange
      iconValue: 3600006, // diamond
      isDefault: true,
    ),
    5: GoalTrackerCategory(
      id: '5',
      name: 'Personal',
      description: 'Personal development goals',
      color: Color(0xFF9C27B0), // Purple
      iconValue: 3600005, // star
      isDefault: true,
    ),
    6: GoalTrackerCategory(
      id: '6',
      name: 'Spiritual',
      description: 'Spiritual and mindfulness goals',
      color: Color(0xFF673AB7), // Deep Purple
      iconValue: 3600001, // flower
      isDefault: true,
    ),
  };

  // Map category names to actual database IDs from your system
  static final Map<String, int> _categoryNameToId = {
    'career': 100,    // Use actual DB ID
    'health': 101,    // Use actual DB ID  
    'love': 102,      // Use actual DB ID
    'money': 103,     // Use actual DB ID
    'personal': 104,  // Use actual DB ID
    'spiritual': 105, // Use actual DB ID
  };

  // Map database IDs back to local categories for display
  static final Map<int, int> _dbIdToLocalId = {
    100: 1, // career
    101: 2, // health
    102: 3, // love
    103: 4, // money
    104: 5, // personal
    105: 6, // spiritual
    // Handle user custom categories that might exist
    1045: 3, // Love -> love
    1046: 5, // Success -> personal
    1047: 2, // Health -> health
    1048: 4, // Abundance -> money
    1049: 5, // Confidence -> personal
    1051: 2, // Health -> health
    1052: 3, // Love -> love
    1089: 5, // test category -> personal
  };

  /// Get default categories - simple local categories, no API calls
  Future<GoalTrackerResult<List<GoalTrackerCategory>>> getDefaultCategories() async {
    return GoalTrackerResult.success(
      data: _localCategories.values.toList(),
      message: 'Local categories loaded successfully',
    );
  }

  /// Get category by ID
  GoalTrackerCategory? getCategoryById(int categoryId) {
    return _localCategories[categoryId];
  }

  /// Get category ID by name
  int? getCategoryIdByName(String categoryName) {
    return _categoryNameToId[categoryName.toLowerCase()];
  }

  /// Get local category from database category ID
  GoalTrackerCategory? getLocalCategoryFromDbId(int dbCategoryId) {
    final localId = _dbIdToLocalId[dbCategoryId];
    if (localId != null) {
      return _localCategories[localId];
    }
    return null; // Return null for unknown categories
  }

  /// Get user's custom categories (still keep this for custom categories)
  Future<GoalTrackerResult<List<GoalTrackerCategory>>> getUserCategories() async {
    // For now, return empty list - users can only use the 6 default categories
    return GoalTrackerResult.success(
      data: <GoalTrackerCategory>[],
      message: 'No custom categories (using local defaults only)',
    );
  }

  /// Get all categories (just return local default categories)
  Future<GoalTrackerResult<List<GoalTrackerCategory>>> getAllCategories() async {
    return getDefaultCategories(); // Just return the 6 local categories
  }


  // ========================= GOAL TRACKERS =========================

  /// Get all goal trackers for the current user
  Future<GoalTrackerResult<List<GoalTrackerModel>>> getUserGoalTrackers() async {
    try {
      final response = await _apiClient.apiV1ManifestGetgoaltrackersGet(
        userId: _authService.currentUser?.userId,
      );

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        if (returnModel.isSuccess == true && returnModel.model != null) {
          return GoalTrackerResult.success(
            data: returnModel.model!,
            message: 'Goal trackers loaded successfully',
          );
        } else {
          return GoalTrackerResult.failure(
            message: returnModel.errorHolder?.toString() ?? 'Failed to load goal trackers',
          );
        }
      } else {
        return GoalTrackerResult.failure(
          message: 'Network error: ${response.statusCode}',
        );
      }
    } catch (e) {
      return GoalTrackerResult.failure(
        message: 'Error loading goal trackers: ${e.toString()}',
      );
    }
  }

  /// Get goal trackers by category
  Future<GoalTrackerResult<List<GoalTrackerModel>>> getGoalTrackersByCategory(int categoryId) async {
    try {
      final response = await _apiClient.apiV1ManifestGetgoaltrackersbycategoryidGet(
        categoryId: categoryId,
      );

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        if (returnModel.isSuccess == true && returnModel.model != null) {
          return GoalTrackerResult.success(
            data: returnModel.model!,
            message: 'Goal trackers loaded successfully',
          );
        } else {
          return GoalTrackerResult.failure(
            message: returnModel.errorHolder?.toString() ?? 'Failed to load goal trackers for category',
          );
        }
      } else {
        return GoalTrackerResult.failure(
          message: 'Network error: ${response.statusCode}',
        );
      }
    } catch (e) {
      return GoalTrackerResult.failure(
        message: 'Error loading goal trackers by category: ${e.toString()}',
      );
    }
  }

  /// Create a new goal tracker - simple version with local category ID
  Future<GoalTrackerResult<GoalTrackerModel>> createGoalTracker({
    required String goalName,
    required int goalStatusTypeId,
    DateTime? startDate,
    DateTime? endDate,
    required int userId,
    int? categoryId, // Simple single category ID (1-6 for local categories)
  }) async {
    try {
      // Simple validation
      if (goalName.trim().isEmpty) {
        return GoalTrackerResult.failure(message: 'Goal name cannot be empty');
      }
      
      // Create goal model with category association if provided
      List<GoalTrackerCategoryCreateModel>? categoryModels;
      if (categoryId != null) {
        categoryModels = [
          GoalTrackerCategoryCreateModel(
            categoryId: categoryId, // Use real database ID
            userId: userId,
          )
        ];
        print('📝 Creating goal with database category ID: $categoryId');
      }

      final createModel = GoalTrackerCreateModel(
        goalTrackerName: goalName.trim(),
        predefinedGoalStatusTypeId: goalStatusTypeId,
        goalTrackerStartDate: startDate ?? DateTime.now(),
        goalTrackerEndDate: endDate,
        userId: userId,
        newGoalTrackerCategoryModels: categoryModels,
      );

      final response = await _apiClient.apiV1ManifestCreategoaltrackerPost(body: createModel);

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        if (returnModel.isSuccess == true && returnModel.model != null) {
          // If we have a categoryId, we'll handle it on the client side during loading
          print('✅ Goal created successfully, categoryId $categoryId will be handled locally');
          return GoalTrackerResult.success(
            data: returnModel.model!,
            message: 'Goal tracker created successfully',
          );
        } else {
          return GoalTrackerResult.failure(
            message: returnModel.errorHolder?.friendlyMessage ?? 'Failed to create goal tracker',
          );
        }
      } else {
        return GoalTrackerResult.failure(
          message: 'Network error: ${response.statusCode}',
        );
      }
    } catch (e) {
      return GoalTrackerResult.failure(
        message: 'Error creating goal tracker: ${e.toString()}',
      );
    }
  }

  /// Update a goal tracker with category associations
  Future<GoalTrackerResult<GoalTrackerModel>> updateGoalTracker({
    required int goalTrackerId,
    String? goalName,
    int? goalStatusTypeId,
    DateTime? startDate,
    DateTime? endDate,
    DateTime? completedDate,
    List<int>? categoryIds, // NEW: Category IDs to associate with the goal
  }) async {
    try {
      // Create category associations if provided
      List<GoalTrackerCategoryCreateModel>? categoryModels;
      if (categoryIds != null && categoryIds.isNotEmpty) {
        print('🏷️  Updating category associations for IDs: $categoryIds');
        categoryModels = categoryIds.map((categoryId) {
          return GoalTrackerCategoryCreateModel(
            categoryId: categoryId,
            goalTrackerId: goalTrackerId,
            userId: _authService.currentUser?.userId,
          );
        }).toList();
        print('🏷️  Created ${categoryModels.length} category association models for update');
      }
      
      final updateModel = GoalTrackerUpdateModel(
        goalTrackerId: goalTrackerId,
        goalTrackerName: goalName,
        predefinedGoalStatusTypeId: goalStatusTypeId,
        goalTrackerStartDate: startDate,
        goalTrackerEndDate: endDate,
        goalTrackerCompletedDate: completedDate,
        userId: _authService.currentUser?.userId,
        newGoalTrackerCategoryModels: categoryModels, // NEW: Category associations
      );

      final response = await _apiClient.apiV1ManifestUpdategoaltrackerPost(body: updateModel);

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        if (returnModel.isSuccess == true && returnModel.model != null) {
          return GoalTrackerResult.success(
            data: returnModel.model!,
            message: 'Goal tracker updated successfully',
          );
        } else {
          return GoalTrackerResult.failure(
            message: returnModel.errorHolder?.toString() ?? 'Failed to update goal tracker',
          );
        }
      } else {
        return GoalTrackerResult.failure(
          message: 'Network error: ${response.statusCode}',
        );
      }
    } catch (e) {
      return GoalTrackerResult.failure(
        message: 'Error updating goal tracker: ${e.toString()}',
      );
    }
  }

  /// Delete a goal tracker (mark as deleted)
  Future<GoalTrackerResult<bool>> deleteGoalTracker(int goalTrackerId) async {
    try {
      print('🗑️  API: Marking goal tracker as deleted - ID: $goalTrackerId');
      
      // Add timeout to prevent hanging
      final response = await _apiClient.apiV1ManifestMarkgoaltrackerasdeletedDelete(id: goalTrackerId)
          .timeout(Duration(seconds: 30));

      print('🗑️  API Response received!');
      print('🗑️  API Response - Status Code: ${response.statusCode}');
      print('🗑️  API Response - Is Successful: ${response.isSuccessful}');
      print('🗑️  API Response - Body: ${response.body?.toString()}');

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        print('🗑️  API Response - IsSuccess: ${returnModel.isSuccess}');
        print('🗑️  API Response - Model: ${returnModel.model}');
        print('🗑️  API Response - Message: ${returnModel.successMessage}');
        print('🗑️  API Response - Error: ${returnModel.errorHolder}');
        
        if (returnModel.isSuccess == true) {
          return GoalTrackerResult.success(
            data: true,
            message: 'Goal tracker marked as deleted successfully',
          );
        } else {
          return GoalTrackerResult.failure(
            message: returnModel.errorHolder?.toString() ?? 'Failed to mark goal tracker as deleted',
          );
        }
      } else {
        return GoalTrackerResult.failure(
          message: 'Network error: ${response.statusCode}',
        );
      }
    } catch (e) {
      print('🗑️  API Exception: $e');
      
      if (e.toString().contains('TimeoutException')) {
        return GoalTrackerResult.failure(
          message: 'Delete request timed out - the goal may have been deleted successfully',
        );
      }
      
      return GoalTrackerResult.failure(
        message: 'Error marking goal tracker as deleted: ${e.toString()}',
      );
    }
  }

  // ========================= MILESTONES =========================

  /// Get all milestones for the current user
  Future<GoalTrackerResult<List<GoalTrackerMileStoneModel>>> getUserMilestones() async {
    try {
      final response = await _apiClient.apiV1ManifestGetgoaltrackermilestonebyuseridGet(
        userId: _authService.currentUser?.userId,
      );

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        if (returnModel.isSuccess == true && returnModel.model != null) {
          return GoalTrackerResult.success(
            data: returnModel.model!,
            message: 'Milestones loaded successfully',
          );
        } else {
          return GoalTrackerResult.failure(
            message: returnModel.errorHolder?.toString() ?? 'Failed to load milestones',
          );
        }
      } else {
        return GoalTrackerResult.failure(
          message: 'Network error: ${response.statusCode}',
        );
      }
    } catch (e) {
      return GoalTrackerResult.failure(
        message: 'Error loading milestones: ${e.toString()}',
      );
    }
  }

  /// Get milestones for a specific goal tracker
  Future<GoalTrackerResult<List<GoalTrackerMileStoneModel>>> getMilestonesByGoalTracker(int goalTrackerId) async {
    try {
      final response = await _apiClient.apiV1ManifestGetgoaltrackermilestonebygoaltrackeridGet(
        goalTrackerId: goalTrackerId,
      );

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        if (returnModel.isSuccess == true && returnModel.model != null) {
          return GoalTrackerResult.success(
            data: returnModel.model!,
            message: 'Milestones loaded successfully',
          );
        } else {
          return GoalTrackerResult.failure(
            message: returnModel.errorHolder?.toString() ?? 'Failed to load milestones for goal',
          );
        }
      } else {
        return GoalTrackerResult.failure(
          message: 'Network error: ${response.statusCode}',
        );
      }
    } catch (e) {
      return GoalTrackerResult.failure(
        message: 'Error loading milestones by goal tracker: ${e.toString()}',
      );
    }
  }

  /// Create a new milestone
  Future<GoalTrackerResult<GoalTrackerMileStoneModel>> createMilestone({
    required int goalTrackerId,
    required String milestoneName,
    DateTime? startDate,
    DateTime? endDate,
    int? statusTypeId,
  }) async {
    try {
      final createModel = GoalTrackerMileStoneCreateModel(
        goalTrackerId: goalTrackerId,
        goalTrackerMileStoneName: milestoneName,
        goalTrackerMileStoneStartDate: startDate ?? DateTime.now(),
        goalTrackerMileStoneEndDate: endDate,
        predefinedGoalStatusTypeId: statusTypeId ?? 1, // Default to "Active" status (ID 1)
        userId: _authService.currentUser?.userId,
      );

      final response = await _apiClient.apiV1ManifestCreategoaltrackermilestonePost(body: createModel);

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        if (returnModel.isSuccess == true && returnModel.model != null) {
          return GoalTrackerResult.success(
            data: returnModel.model!,
            message: 'Milestone created successfully',
          );
        } else {
          return GoalTrackerResult.failure(
            message: returnModel.errorHolder?.toString() ?? 'Failed to create milestone',
          );
        }
      } else {
        return GoalTrackerResult.failure(
          message: 'Network error: ${response.statusCode}',
        );
      }
    } catch (e) {
      return GoalTrackerResult.failure(
        message: 'Error creating milestone: ${e.toString()}',
      );
    }
  }

  /// Update a milestone
  Future<GoalTrackerResult<GoalTrackerMileStoneModel>> updateMilestone({
    required int milestoneId,
    int? goalTrackerId,
    String? milestoneName,
    DateTime? startDate,
    DateTime? endDate,
    DateTime? completedDate,
    int? statusTypeId,
  }) async {
    try {
      final updateModel = GoalTrackerMileStoneUpdateModel(
        goalTrackerMileStoneId: milestoneId,
        goalTrackerId: goalTrackerId,
        goalTrackerMileStoneName: milestoneName,
        goalTrackerMileStoneStartDate: startDate,
        goalTrackerMileStoneEndDate: endDate,
        goalTrackerMileStoneCompletedDate: completedDate,
        predefinedGoalStatusTypeId: statusTypeId,
        userId: _authService.currentUser?.userId,
      );

      final response = await _apiClient.apiV1ManifestUpdategoaltrackermilestonePost(body: updateModel);

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        if (returnModel.isSuccess == true && returnModel.model != null) {
          return GoalTrackerResult.success(
            data: returnModel.model!,
            message: 'Milestone updated successfully',
          );
        } else {
          return GoalTrackerResult.failure(
            message: returnModel.errorHolder?.toString() ?? 'Failed to update milestone',
          );
        }
      } else {
        return GoalTrackerResult.failure(
          message: 'Network error: ${response.statusCode}',
        );
      }
    } catch (e) {
      return GoalTrackerResult.failure(
        message: 'Error updating milestone: ${e.toString()}',
      );
    }
  }

  /// Delete a milestone (mark as deleted)
  Future<GoalTrackerResult<bool>> deleteMilestone(int milestoneId) async {
    try {
      print('🗑️  API: Marking milestone as deleted - ID: $milestoneId');
      
      final response = await _apiClient.apiV1ManifestMarkgoaltrackermilestoneasdeletedDelete(id: milestoneId);

      print('🗑️  Milestone API Response - Status Code: ${response.statusCode}');
      print('🗑️  Milestone API Response - Is Successful: ${response.isSuccessful}');

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        if (returnModel.isSuccess == true) {
          return GoalTrackerResult.success(
            data: true,
            message: 'Milestone marked as deleted successfully',
          );
        } else {
          return GoalTrackerResult.failure(
            message: returnModel.errorHolder?.toString() ?? 'Failed to mark milestone as deleted',
          );
        }
      } else {
        return GoalTrackerResult.failure(
          message: 'Network error: ${response.statusCode}',
        );
      }
    } catch (e) {
      print('🗑️  Milestone API Exception: $e');
      return GoalTrackerResult.failure(
        message: 'Error marking milestone as deleted: ${e.toString()}',
      );
    }
  }

  // ========================= GOAL-CATEGORY ASSOCIATION =========================
  // Note: Category associations are now handled during goal creation/update
  // via the newGoalTrackerCategoryModels property in GoalTrackerCreateModel/UpdateModel

  /// Simple method to get category ID from name (for UI mapping)
  int? getCategoryIdFromName(String categoryName) {
    return _categoryNameToId[categoryName.toLowerCase()];
  }

  /// Simple method for backward compatibility with existing code
  Future<int?> getCategoryIdFromLocalEnum(String localCategoryName) async {
    // Map some old names to new names for compatibility
    final nameMapping = {
      'relationships': 'love',
      'financial': 'money',
    };
    
    final mappedName = nameMapping[localCategoryName.toLowerCase()] ?? localCategoryName.toLowerCase();
    return getCategoryIdFromName(mappedName);
  }

  // ========================= HELPER METHODS =========================

  /// Convert integer color value to Flutter Color object
  Color _convertIntToColor(int? colorValue) {
    if (colorValue == null) return Color(0xFF4CAF50); // Default green
    return Color(colorValue);
  }
}

// ========================= RESULT CLASS =========================

class GoalTrackerResult<T> {
  final bool isSuccess;
  final String message;
  final T? data;

  GoalTrackerResult._({
    required this.isSuccess,
    required this.message,
    this.data,
  });

  factory GoalTrackerResult.success({
    required T data,
    required String message,
  }) {
    return GoalTrackerResult._(
      isSuccess: true,
      message: message,
      data: data,
    );
  }

  factory GoalTrackerResult.failure({
    required String message,
  }) {
    return GoalTrackerResult._(
      isSuccess: false,
      message: message,
    );
  }
}

// ========================= AUTH INTERCEPTOR =========================

class _AuthInterceptor implements Interceptor {
  @override
  FutureOr<Response<BodyType>> intercept<BodyType>(Chain<BodyType> chain) async {
    final request = chain.request;
    final authService = UnifiedAuthService();
    
    final token = authService.authToken;
    print('🔐 Auth Interceptor - Token available: ${token != null}');
    if (token != null) {
      print('🔐 Auth Interceptor - Adding Bearer token to request');
      final updatedRequest = request.copyWith(
        headers: {
          ...request.headers,
          'Authorization': 'Bearer $token',
        },
      );
      return chain.proceed(updatedRequest);
    } else {
      print('⚠️  Auth Interceptor - No token available, proceeding without auth');
    }
    
    return chain.proceed(request);
  }
}
