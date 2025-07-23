import 'package:chopper/chopper.dart';
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

  /// Get default categories (system categories like career, health, love, money, personal, spiritual)
  /// These are filtered to only return the main categories we support
  Future<GoalTrackerResult<List<GoalTrackerCategory>>> getDefaultCategories() async {
    try {
      final response = await _apiClient.apiV1ManifestGetdefaultcategorysGet();

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        if (returnModel.isSuccess == true && returnModel.model != null) {
          // Filter to only include the main categories we support
          final supportedCategoryNames = ['career', 'health', 'love', 'money', 'personal', 'spiritual'];
          
          final filteredCategories = returnModel.model!.where((category) {
            final categoryName = category.defaultCategoryName?.toLowerCase() ?? '';
            return supportedCategoryNames.any((supported) => categoryName.contains(supported));
          }).map((category) {
            return GoalTrackerCategory(
              id: category.defaultCategoryId?.toString() ?? '',
              name: category.defaultCategoryName ?? '',
              description: '', // DefaultCategoryModel doesn't have description
              color: null, // DefaultCategoryModel doesn't have color field
              icon: null, // DefaultCategoryModel doesn't have icon field  
              isDefault: true,
            );
          }).toList();

          return GoalTrackerResult.success(
            data: filteredCategories,
            message: 'Default categories loaded successfully',
          );
        } else {
          return GoalTrackerResult.failure(
            message: returnModel.errorHolder?.toString() ?? 'Failed to load default categories',
          );
        }
      } else {
        return GoalTrackerResult.failure(
          message: 'Network error: ${response.statusCode}',
        );
      }
    } catch (e) {
      return GoalTrackerResult.failure(
        message: 'Error loading default categories: ${e.toString()}',
      );
    }
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
              color: _convertIntToHexColor(category.colorAsInteger),
              icon: _mapCategoryIcon(category.predefinedIconTypeId),
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

  /// Convert integer color value to hex string
  String? _convertIntToHexColor(int? colorValue) {
    if (colorValue == null) return null;
    return '#${colorValue.toRadixString(16).padLeft(8, '0').substring(2)}';
  }

  /// Map icon enum value to icon name string
  String? _mapCategoryIcon(int? iconValue) {
    if (iconValue == null) return null;
    
    // Map icon enum values to icon names
    // You'll need to update this mapping based on your actual icon enum values
    switch (iconValue) {
      case 1:
        return 'career';
      case 2:
        return 'health';
      case 3:
        return 'love';
      case 4:
        return 'money';
      case 5:
        return 'personal';
      case 6:
        return 'spiritual';
      default:
        return 'flag';
    }
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
