import 'package:chopper/chopper.dart';
import 'package:flutter/material.dart';
import 'package:fun/services/auth_service.dart';
import 'dart:async';
import '../api/generated/code/manifestation.swagger.dart';
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

  // ========================= CATEGORIES =========================

  // The 6 supported default categories with hardcoded colors and icons
  static const List<String> _supportedDefaultCategories = [
    'career',
    'health', 
    'love',
    'money',
    'personal',
    'spiritual'
  ];

  // Hardcoded colors for the 6 default categories (following affirmations pattern)
  static final Map<String, Color> _defaultCategoryColors = {
    'career': Color(0xFF2196F3),      // Blue
    'health': Color(0xFF4CAF50),      // Green 
    'love': Color(0xFFE91E63),        // Pink
    'money': Color(0xFFFF9800),       // Orange
    'personal': Color(0xFF9C27B0),    // Purple
    'spiritual': Color(0xFF673AB7),   // Deep Purple
  };

  // Hardcoded icons for the 6 default categories (using PredefinedIconTypes values)
  static final Map<String, int> _defaultCategoryIcons = {
    'career': 3600007,      // target
    'health': 3600004,      // leaf
    'love': 3600002,        // heart
    'money': 3600006,       // diamond
    'personal': 3600005,    // star
    'spiritual': 3600001,   // flower
  };

  /// Get default categories (system categories like career, health, love, money, personal, spiritual)
  /// These are filtered to only return the main categories we support with hardcoded colors and icons
  Future<GoalTrackerResult<List<GoalTrackerCategory>>> getDefaultCategories() async {
    try {
      final response = await _apiClient.apiV1ManifestGetdefaultcategorysGet();

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        if (returnModel.isSuccess == true && returnModel.model != null) {
          // Filter to only include the main categories we support
          final filteredCategories = <GoalTrackerCategory>[];
          
          for (final apiCategory in returnModel.model!) {
            final categoryName = apiCategory.defaultCategoryName?.toLowerCase() ?? '';
            
            // Find matching supported category
            final supportedCategory = _supportedDefaultCategories.firstWhere(
              (supported) => categoryName.contains(supported) || supported.contains(categoryName),
              orElse: () => '',
            );
            
            if (supportedCategory.isNotEmpty) {
              final goalCategory = GoalTrackerCategory(
                id: apiCategory.defaultCategoryId?.toString() ?? supportedCategory,
                name: _capitalizeFirst(supportedCategory),
                description: 'Default ${_capitalizeFirst(supportedCategory)} category',
                color: _defaultCategoryColors[supportedCategory] ?? Color(0xFF4CAF50),
                iconValue: _defaultCategoryIcons[supportedCategory],
                isDefault: true,
              );
              filteredCategories.add(goalCategory);
            }
          }

          // If API doesn't return all 6 categories, create the missing ones manually
          for (final supportedCategory in _supportedDefaultCategories) {
            final exists = filteredCategories.any((cat) => 
              cat.name.toLowerCase() == supportedCategory);
            
            if (!exists) {
              final goalCategory = GoalTrackerCategory(
                id: supportedCategory,
                name: _capitalizeFirst(supportedCategory),
                description: 'Default ${_capitalizeFirst(supportedCategory)} category',
                color: _defaultCategoryColors[supportedCategory] ?? Color(0xFF4CAF50),
                iconValue: _defaultCategoryIcons[supportedCategory],
                isDefault: true,
              );
              filteredCategories.add(goalCategory);
            }
          }

          return GoalTrackerResult.success(
            data: filteredCategories,
            message: 'Default categories loaded successfully',
          );
        } else {
          // Fallback: create all 6 categories manually if API fails
          return _createFallbackDefaultCategories();
        }
      } else {
        // Fallback: create all 6 categories manually if API fails
        return _createFallbackDefaultCategories();
      }
    } catch (e) {
      print('Error loading default categories from API: $e');
      // Fallback: create all 6 categories manually if API fails
      return _createFallbackDefaultCategories();
    }
  }

  /// Create fallback default categories if API fails
  GoalTrackerResult<List<GoalTrackerCategory>> _createFallbackDefaultCategories() {
    final categories = _supportedDefaultCategories.map((categoryName) {
      return GoalTrackerCategory(
        id: categoryName,
        name: _capitalizeFirst(categoryName),
        description: 'Default ${_capitalizeFirst(categoryName)} category',
        color: _defaultCategoryColors[categoryName] ?? Color(0xFF4CAF50),
        iconValue: _defaultCategoryIcons[categoryName],
        isDefault: true,
      );
    }).toList();

    return GoalTrackerResult.success(
      data: categories,
      message: 'Default categories created (fallback)',
    );
  }

  /// Helper method to capitalize first letter
  String _capitalizeFirst(String text) {
    if (text.isEmpty) return text;
    return text[0].toUpperCase() + text.substring(1);
  }

  /// Get user's custom categories
  Future<GoalTrackerResult<List<GoalTrackerCategory>>> getUserCategories() async {
    try {
      final response = await _apiClient.apiV1ManifestGetcategorysGet(
        userId: _authService.currentUser?.userId,
      );

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        if (returnModel.isSuccess == true && returnModel.model != null) {
          final categories = returnModel.model!.map((category) {
            return GoalTrackerCategory(
              id: category.categoryId?.toString() ?? '',
              name: category.categoryName ?? '',
              description: '', // CategoryModel doesn't have description field
              color: _convertIntToColor(category.colorAsInteger),
              iconValue: category.predefinedIconTypeId,
              isDefault: false,
              userId: category.userId,
              createdAt: category.createdDate,
            );
          }).toList();

          return GoalTrackerResult.success(
            data: categories,
            message: 'User categories loaded successfully',
          );
        } else {
          return GoalTrackerResult.failure(
            message: returnModel.errorHolder?.toString() ?? 'Failed to load user categories',
          );
        }
      } else {
        return GoalTrackerResult.failure(
          message: 'Network error: ${response.statusCode}',
        );
      }
    } catch (e) {
      return GoalTrackerResult.failure(
        message: 'Error loading user categories: ${e.toString()}',
      );
    }
  }

  /// Get all categories (both default and user categories combined)
  Future<GoalTrackerResult<List<GoalTrackerCategory>>> getAllCategories() async {
    try {
      final defaultResult = await getDefaultCategories();
      final userResult = await getUserCategories();

      if (defaultResult.isSuccess && userResult.isSuccess) {
        final allCategories = [
          ...defaultResult.data!,
          ...userResult.data!,
        ];

        return GoalTrackerResult.success(
          data: allCategories,
          message: 'All categories loaded successfully',
        );
      } else {
        return GoalTrackerResult.failure(
          message: 'Failed to load categories: ${defaultResult.message} | ${userResult.message}',
        );
      }
    } catch (e) {
      return GoalTrackerResult.failure(
        message: 'Error loading all categories: ${e.toString()}',
      );
    }
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

  /// Create a new goal tracker
  Future<GoalTrackerResult<GoalTrackerModel>> createGoalTracker({
    required String goalName,
    required int goalStatusTypeId,
    DateTime? startDate,
    DateTime? endDate,
    required int userId,
  }) async {
    try {
      final createModel = GoalTrackerCreateModel(
        goalTrackerName: goalName,
        predefinedGoalStatusTypeId: goalStatusTypeId,
        goalTrackerStartDate: startDate ?? DateTime.now(),
        goalTrackerEndDate: endDate,
        userId: userId,
      );

      final response = await _apiClient.apiV1ManifestCreategoaltrackerPost(body: createModel);

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        if (returnModel.isSuccess == true && returnModel.model != null) {
          return GoalTrackerResult.success(
            data: returnModel.model!,
            message: 'Goal tracker created successfully',
          );
        } else {
          return GoalTrackerResult.failure(
            message: returnModel.errorHolder?.toString() ?? 'Failed to create goal tracker',
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

  /// Update a goal tracker
  Future<GoalTrackerResult<GoalTrackerModel>> updateGoalTracker({
    required int goalTrackerId,
    String? goalName,
    int? goalStatusTypeId,
    DateTime? startDate,
    DateTime? endDate,
    DateTime? completedDate,
  }) async {
    try {
      final updateModel = GoalTrackerUpdateModel(
        goalTrackerId: goalTrackerId,
        goalTrackerName: goalName,
        predefinedGoalStatusTypeId: goalStatusTypeId,
        goalTrackerStartDate: startDate,
        goalTrackerEndDate: endDate,
        goalTrackerCompletedDate: completedDate,
        userId: _authService.currentUser?.userId,
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

  /// Delete a goal tracker
  Future<GoalTrackerResult<bool>> deleteGoalTracker(int goalTrackerId) async {
    try {
      final response = await _apiClient.apiV1ManifestMarkgoaltrackerasdeletedDelete(id: goalTrackerId);

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        if (returnModel.isSuccess == true) {
          return GoalTrackerResult.success(
            data: true,
            message: 'Goal tracker deleted successfully',
          );
        } else {
          return GoalTrackerResult.failure(
            message: returnModel.errorHolder?.toString() ?? 'Failed to delete goal tracker',
          );
        }
      } else {
        return GoalTrackerResult.failure(
          message: 'Network error: ${response.statusCode}',
        );
      }
    } catch (e) {
      return GoalTrackerResult.failure(
        message: 'Error deleting goal tracker: ${e.toString()}',
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
        predefinedGoalStatusTypeId: statusTypeId ?? 1, // Default to "Active" status
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

  /// Delete a milestone
  Future<GoalTrackerResult<bool>> deleteMilestone(int milestoneId) async {
    try {
      final response = await _apiClient.apiV1ManifestMarkgoaltrackermilestoneasdeletedDelete(id: milestoneId);

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        if (returnModel.isSuccess == true) {
          return GoalTrackerResult.success(
            data: true,
            message: 'Milestone deleted successfully',
          );
        } else {
          return GoalTrackerResult.failure(
            message: returnModel.errorHolder?.toString() ?? 'Failed to delete milestone',
          );
        }
      } else {
        return GoalTrackerResult.failure(
          message: 'Network error: ${response.statusCode}',
        );
      }
    } catch (e) {
      return GoalTrackerResult.failure(
        message: 'Error deleting milestone: ${e.toString()}',
      );
    }
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
