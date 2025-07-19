import 'dart:async';
import 'package:manifestation_app/api/generated/code/manifestation.swagger.dart';
import 'package:manifestation_app/services/api_config.dart';
import 'package:chopper/chopper.dart';
import 'package:manifestation_app/services/session_service.dart';
import 'package:flutter/material.dart';

/// Goal data model for UI
class Goal {
  final String id;
  final String title;
  final String description;
  final GoalCategory category;
  final DateTime startDate;
  final DateTime? endDate;
  final DateTime? completedDate;
  final GoalStatus status;
  final List<GoalMilestone> milestones;
  final DateTime createdAt;

  Goal({
    required this.id,
    required this.title,
    required this.description,
    required this.category,
    required this.startDate,
    this.endDate,
    this.completedDate,
    required this.status,
    this.milestones = const [],
    required this.createdAt,
  });

  Goal copyWith({
    String? id,
    String? title,
    String? description,
    GoalCategory? category,
    DateTime? startDate,
    DateTime? endDate,
    DateTime? completedDate,
    GoalStatus? status,
    List<GoalMilestone>? milestones,
    DateTime? createdAt,
  }) {
    return Goal(
      id: id ?? this.id,
      title: title ?? this.title,
      description: description ?? this.description,
      category: category ?? this.category,
      startDate: startDate ?? this.startDate,
      endDate: endDate ?? this.endDate,
      completedDate: completedDate ?? this.completedDate,
      status: status ?? this.status,
      milestones: milestones ?? this.milestones,
      createdAt: createdAt ?? this.createdAt,
    );
  }
}

/// Goal milestone data model
class GoalMilestone {
  final String id;
  final String goalId;
  final String title;
  final DateTime? startDate;
  final DateTime? endDate;
  final DateTime? completedDate;
  final GoalStatus status;
  final DateTime createdAt;

  GoalMilestone({
    required this.id,
    required this.goalId,
    required this.title,
    this.startDate,
    this.endDate,
    this.completedDate,
    required this.status,
    required this.createdAt,
  });
}

/// Goal categories enum
enum GoalCategory {
  career('Career', Color(0xFF4CAF50)),
  health('Health', Color(0xFF2196F3)),
  love('Love', Color(0xFFE91E63)),
  money('Money', Color(0xFFFF9800)),
  personal('Personal', Color(0xFF9C27B0)),
  spiritual('Spiritual', Color(0xFF795548));

  const GoalCategory(this.name, this.color);
  final String name;
  final Color color;
}

/// Goal status enum
enum GoalStatus {
  notStarted('Not Started'),
  inProgress('In Progress'),
  completed('Completed'),
  paused('Paused'),
  cancelled('Cancelled');

  const GoalStatus(this.name);
  final String name;
}

/// Service to handle goal API calls
class GoalApiService {
  final Manifestation _apiClient;
  
  GoalApiService({String? baseUrl}) : _apiClient = Manifestation.create(
    baseUrl: Uri.parse(baseUrl ?? ApiConfig.baseUrl),
    interceptors: [
      _BasicAuthInterceptor(),
      _SessionTokenInterceptor(),
    ],
  );

  // GOAL OPERATIONS

  /// Get all goals for the current user
  Future<List<Goal>> getGoals() async {
    try {
      print('Getting goals from API...');
      
      final response = await _apiClient.apiV1ManifestGetgoaltrackersGet();
      
      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        if (returnModel.isSuccess == true && returnModel.model != null) {
          final goals = returnModel.model!.map((goalModel) => 
            _mapGoalTrackerModelToUI(goalModel)
          ).toList();
          
          print('Retrieved ${goals.length} goals from API');
          return goals;
        }
      }
      
      print('Failed to get goals from API');
      return [];
    } catch (e) {
      print('Error getting goals: $e');
      return [];
    }
  }

  /// Create a new goal
  Future<Goal?> createGoal(Goal goal) async {
    try {
      print('Creating goal: ${goal.title}');
      
      final createModel = GoalTrackerCreateModel(
        goalTrackerName: goal.title,
        predefinedGoalStatusTypeId: _mapStatusToId(goal.status),
        goalTrackerStartDate: goal.startDate,
        goalTrackerEndDate: goal.endDate,
        goalTrackerCompletedDate: goal.completedDate,
        userId: null, // Will be set by API based on session
        addedToFeed: false,
      );

      final response = await _apiClient.apiV1ManifestCreategoaltrackerPost(body: createModel);
      
      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        if (returnModel.isSuccess == true && returnModel.model != null) {
          print('Created goal successfully');
          final createdGoal = _mapGoalTrackerModelToUI(returnModel.model!);
          
          // Create goal-category relationship
          await _createGoalCategoryRelationship(createdGoal.id, goal.category);
          
          return createdGoal;
        }
      }
      
      print('Failed to create goal');
      return null;
    } catch (e) {
      print('Error creating goal: $e');
      return null;
    }
  }

  /// Update an existing goal
  Future<Goal?> updateGoal(Goal goal) async {
    try {
      print('Updating goal: ${goal.title}');
      
      final updateModel = GoalTrackerUpdateModel(
        goalTrackerId: int.tryParse(goal.id),
        goalTrackerName: goal.title,
        predefinedGoalStatusTypeId: _mapStatusToId(goal.status),
        goalTrackerStartDate: goal.startDate,
        goalTrackerEndDate: goal.endDate,
        goalTrackerCompletedDate: goal.completedDate,
        userId: null, // Will be set by API based on session
        addedToFeed: false,
      );

      final response = await _apiClient.apiV1ManifestUpdategoaltrackerPost(body: updateModel);
      
      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        if (returnModel.isSuccess == true && returnModel.model != null) {
          print('Updated goal successfully');
          return _mapGoalTrackerModelToUI(returnModel.model!);
        }
      }
      
      print('Failed to update goal');
      return null;
    } catch (e) {
      print('Error updating goal: $e');
      return null;
    }
  }

  /// Delete a goal
  Future<bool> deleteGoal(String goalId) async {
    try {
      print('Deleting goal: $goalId');
      
      final id = int.tryParse(goalId);
      if (id == null) return false;

      final response = await _apiClient.apiV1ManifestMarkgoaltrackerasdeletedDelete(id: id);
      
      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        if (returnModel.isSuccess == true) {
          print('Deleted goal successfully');
          return true;
        }
      }
      
      print('Failed to delete goal');
      return false;
    } catch (e) {
      print('Error deleting goal: $e');
      return false;
    }
  }

  /// Get goals by category
  Future<List<Goal>> getGoalsByCategory(GoalCategory category) async {
    final allGoals = await getGoals();
    return allGoals.where((goal) => goal.category == category).toList();
  }

  // MILESTONE OPERATIONS

  /// Create a milestone for a goal
  Future<GoalMilestone?> createMilestone(String goalId, GoalMilestone milestone) async {
    try {
      print('Creating milestone for goal: $goalId');
      
      final createModel = GoalTrackerMileStoneCreateModel(
        goalTrackerId: int.tryParse(goalId),
        goalTrackerMileStoneName: milestone.title,
        goalTrackerMileStoneStartDate: milestone.startDate,
        goalTrackerMileStoneEndDate: milestone.endDate,
        predefinedGoalStatusTypeId: _mapStatusToId(milestone.status),
        goalTrackerMileStoneCompletedDate: milestone.completedDate,
        userId: null, // Will be set by API based on session
      );

      final response = await _apiClient.apiV1ManifestCreategoaltrackermilestonePost(body: createModel);
      
      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        if (returnModel.isSuccess == true && returnModel.model != null) {
          print('Created milestone successfully');
          return _mapMilestoneModelToUI(returnModel.model!);
        }
      }
      
      print('Failed to create milestone');
      return null;
    } catch (e) {
      print('Error creating milestone: $e');
      return null;
    }
  }

  /// Get milestones for a goal
  Future<List<GoalMilestone>> getMilestonesForGoal(String goalId) async {
    try {
      print('Getting milestones for goal: $goalId');
      
      final id = int.tryParse(goalId);
      if (id == null) {
        print('Invalid goal ID format: $goalId');
        return [];
      }

      // Try the specific endpoint first with timeout
      print('Calling milestone API with timeout...');
      var response = await _apiClient.apiV1ManifestGetgoaltrackermilestonebygoaltrackeridGet(goalTrackerId: id)
          .timeout(Duration(seconds: 10), onTimeout: () {
        print('Milestone API call timed out for goal $goalId');
        throw TimeoutException('Milestone API timeout', Duration(seconds: 10));
      });
      
      print('Milestone API response received - status: ${response.statusCode}');
      print('Milestone API response successful: ${response.isSuccessful}');
      
      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        print('Milestone return model isSuccess: ${returnModel.isSuccess}');
        print('Milestone return model has data: ${returnModel.model != null}');
        
        if (returnModel.isSuccess == true && returnModel.model != null) {
          final milestones = returnModel.model!.map((milestoneModel) => 
            _mapMilestoneModelToUI(milestoneModel)
          ).toList();
          
          print('Retrieved ${milestones.length} milestones for goal $goalId');
          return milestones;
        } else {
          print('API returned unsuccessful or null model for goal $goalId');
        }
      } else {
        print('Primary milestone API failed, skipping alternative for now...');
      }
      
      print('Returning empty milestones list for goal $goalId');
      return [];
    } on TimeoutException catch (e) {
      print('Milestone API timeout for goal $goalId: $e');
      return [];
    } catch (e) {
      print('Error getting milestones for goal $goalId: $e');
      return [];
    }
  }

  // HELPER METHODS

  /// Create goal-category relationship
  Future<void> _createGoalCategoryRelationship(String goalId, GoalCategory category) async {
    try {
      // This would use the goal tracker category endpoints to link the goal to a category
      // For now, we'll store the category information with the goal itself
      print('Goal-category relationship would be created here for goal $goalId and category ${category.name}');
    } catch (e) {
      print('Error creating goal-category relationship: $e');
    }
  }

  /// Map API GoalTrackerModel to UI Goal
  Goal _mapGoalTrackerModelToUI(GoalTrackerModel goalModel) {
    return Goal(
      id: goalModel.goalTrackerId?.toString() ?? '',
      title: goalModel.goalTrackerName ?? '',
      description: '', // TODO: Add description field if available in API
      category: _mapCategoryFromName(goalModel.goalTrackerName ?? ''), // TODO: Improve category mapping
      startDate: goalModel.goalTrackerStartDate ?? DateTime.now(),
      endDate: goalModel.goalTrackerEndDate,
      completedDate: goalModel.goalTrackerCompletedDate,
      status: _mapIdToStatus(goalModel.predefinedGoalStatusTypeId),
      createdAt: goalModel.createdDate ?? DateTime.now(),
    );
  }

  /// Map API GoalTrackerMileStoneModel to UI GoalMilestone
  GoalMilestone _mapMilestoneModelToUI(GoalTrackerMileStoneModel milestoneModel) {
    return GoalMilestone(
      id: milestoneModel.goalTrackerMileStoneId?.toString() ?? '',
      goalId: milestoneModel.goalTrackerId?.toString() ?? '',
      title: milestoneModel.goalTrackerMileStoneName ?? '',
      startDate: milestoneModel.goalTrackerMileStoneStartDate,
      endDate: milestoneModel.goalTrackerMileStoneEndDate,
      completedDate: milestoneModel.goalTrackerMileStoneCompletedDate,
      status: _mapIdToStatus(milestoneModel.predefinedGoalStatusTypeId),
      createdAt: milestoneModel.createdDate ?? DateTime.now(),
    );
  }

  /// Map GoalStatus to API status ID
  int? _mapStatusToId(GoalStatus status) {
    // TODO: Get actual status IDs from API
    switch (status) {
      case GoalStatus.notStarted:
        return 1;
      case GoalStatus.inProgress:
        return 2;
      case GoalStatus.completed:
        return 3;
      case GoalStatus.paused:
        return 4;
      case GoalStatus.cancelled:
        return 5;
    }
  }

  /// Map API status ID to GoalStatus
  GoalStatus _mapIdToStatus(int? statusId) {
    // TODO: Get actual status mapping from API
    switch (statusId) {
      case 1:
        return GoalStatus.notStarted;
      case 2:
        return GoalStatus.inProgress;
      case 3:
        return GoalStatus.completed;
      case 4:
        return GoalStatus.paused;
      case 5:
        return GoalStatus.cancelled;
      default:
        return GoalStatus.notStarted;
    }
  }

  /// Map category from goal name (temporary solution)
  GoalCategory _mapCategoryFromName(String goalName) {
    // TODO: Implement proper category mapping using API
    final lowerName = goalName.toLowerCase();
    if (lowerName.contains('career') || lowerName.contains('work') || lowerName.contains('job')) {
      return GoalCategory.career;
    } else if (lowerName.contains('health') || lowerName.contains('fitness') || lowerName.contains('exercise')) {
      return GoalCategory.health;
    } else if (lowerName.contains('love') || lowerName.contains('relationship') || lowerName.contains('romance')) {
      return GoalCategory.love;
    } else if (lowerName.contains('money') || lowerName.contains('financial') || lowerName.contains('income')) {
      return GoalCategory.money;
    } else if (lowerName.contains('spiritual') || lowerName.contains('meditation') || lowerName.contains('mindfulness')) {
      return GoalCategory.spiritual;
    } else {
      return GoalCategory.personal;
    }
  }
}

// Reuse the auth interceptors from other services
class _BasicAuthInterceptor implements Interceptor {
  static const String _basicAuthCredentials = 'U3BlY2lhbFVzZXI6U3BlY2lhbFBhc3N3b3JkMQ==';
  
  @override
  FutureOr<Response<BodyType>> intercept<BodyType>(Chain<BodyType> chain) async {
    final request = chain.request;
    final authenticatedRequest = request.copyWith(
      headers: Map.from(request.headers)
        ..addAll({
          'Authorization': 'Basic $_basicAuthCredentials',
        }),
    );
    
    return chain.proceed(authenticatedRequest);
  }
}

class _SessionTokenInterceptor implements Interceptor {
  @override
  FutureOr<Response<BodyType>> intercept<BodyType>(Chain<BodyType> chain) async {
    final request = chain.request;
    
    // Get session token
    final sessionToken = await SessionService.instance.getSessionToken();
    
    final headers = Map<String, String>.from(request.headers);
    
    // Add session token if available
    if (sessionToken != null) {
      headers['X-Session-Token'] = sessionToken;
      headers['Authorization'] = 'Bearer $sessionToken';
    }
    
    final authenticatedRequest = request.copyWith(headers: headers);
    return chain.proceed(authenticatedRequest);
  }
}