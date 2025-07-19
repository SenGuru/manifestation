import '../models/goals.dart';
import '../models/api_milestone.dart';

class MilestoneConverter {
  /// Convert ApiMilestone to local Milestone for UI compatibility
  static Milestone apiToLocal(ApiMilestone apiMilestone) {
    return Milestone(
      id: apiMilestone.id,
      title: apiMilestone.title,
      description: apiMilestone.description.isNotEmpty ? apiMilestone.description : null,
      isCompleted: apiMilestone.isCompleted,
      completedAt: apiMilestone.completedDate,
      targetDate: apiMilestone.targetDate,
    );
  }

  /// Convert local Milestone to ApiMilestone for API operations
  static ApiMilestone localToApi(Milestone milestone, String goalTrackerId, {int? userId}) {
    return ApiMilestone(
      id: milestone.id,
      goalTrackerId: goalTrackerId,
      title: milestone.title,
      description: milestone.description ?? '',
      isCompleted: milestone.isCompleted,
      progressPercentage: milestone.isCompleted ? 100.0 : 0.0,
      targetDate: milestone.targetDate,
      completedDate: milestone.completedAt,
      createdAt: DateTime.now(),
      modifiedAt: DateTime.now(),
      createdBy: 'User',
      modifiedBy: 'User',
      isDeleted: false,
      userId: userId,
    );
  }

  /// Convert list of ApiMilestones to local Milestones
  static List<Milestone> apiListToLocalList(List<ApiMilestone> apiMilestones) {
    return apiMilestones.map((apiMilestone) => apiToLocal(apiMilestone)).toList();
  }

  /// Convert list of local Milestones to ApiMilestones
  static List<ApiMilestone> localListToApiList(List<Milestone> milestones, String goalTrackerId, {int? userId}) {
    return milestones.map((milestone) => localToApi(milestone, goalTrackerId, userId: userId)).toList();
  }

  /// Create a new ApiMilestone from scratch
  static ApiMilestone createNew({
    required String goalTrackerId,
    required String title,
    String description = '',
    DateTime? targetDate,
    int? userId,
  }) {
    return ApiMilestone(
      id: '', // Will be assigned by API
      goalTrackerId: goalTrackerId,
      title: title,
      description: description,
      isCompleted: false,
      progressPercentage: 0.0,
      targetDate: targetDate,
      completedDate: null,
      createdAt: DateTime.now(),
      modifiedAt: DateTime.now(),
      createdBy: 'User',
      modifiedBy: 'User',
      isDeleted: false,
      userId: userId,
    );
  }

  /// Update an existing ApiMilestone with new data
  static ApiMilestone updateExisting(ApiMilestone existing, {
    String? title,
    String? description,
    bool? isCompleted,
    double? progressPercentage,
    DateTime? targetDate,
    DateTime? completedDate,
  }) {
    return existing.copyWith(
      title: title,
      description: description,
      isCompleted: isCompleted,
      progressPercentage: progressPercentage,
      targetDate: targetDate,
      completedDate: completedDate,
      modifiedAt: DateTime.now(),
    );
  }

  /// Mark milestone as completed
  static ApiMilestone markAsCompleted(ApiMilestone milestone) {
    return milestone.copyWith(
      isCompleted: true,
      progressPercentage: 100.0,
      completedDate: DateTime.now(),
      modifiedAt: DateTime.now(),
    );
  }

  /// Mark milestone as not completed
  static ApiMilestone markAsNotCompleted(ApiMilestone milestone) {
    return milestone.copyWith(
      isCompleted: false,
      progressPercentage: 0.0,
      completedDate: null,
      modifiedAt: DateTime.now(),
    );
  }

  /// Update milestone progress
  static ApiMilestone updateProgress(ApiMilestone milestone, double progressPercentage) {
    final isCompleted = progressPercentage >= 100.0;
    return milestone.copyWith(
      progressPercentage: progressPercentage.clamp(0.0, 100.0),
      isCompleted: isCompleted,
      completedDate: isCompleted ? DateTime.now() : null,
      modifiedAt: DateTime.now(),
    );
  }

  /// Calculate goal progress based on milestones
  static double calculateGoalProgress(List<ApiMilestone> milestones) {
    if (milestones.isEmpty) return 0.0;
    
    final totalProgress = milestones.fold<double>(
      0.0, 
      (sum, milestone) => sum + (milestone.isCompleted ? 100.0 : milestone.progressPercentage)
    );
    
    return (totalProgress / milestones.length).clamp(0.0, 100.0);
  }

  /// Get milestone summary statistics
  static Map<String, dynamic> getMilestoneSummary(List<ApiMilestone> milestones) {
    final total = milestones.length;
    final completed = milestones.where((m) => m.isCompleted).length;
    final inProgress = milestones.where((m) => !m.isCompleted && m.progressPercentage > 0).length;
    final notStarted = milestones.where((m) => !m.isCompleted && m.progressPercentage == 0).length;
    final overdue = milestones.where((m) => m.isOverdue).length;

    return {
      'total': total,
      'completed': completed,
      'inProgress': inProgress,
      'notStarted': notStarted,
      'overdue': overdue,
      'completionRate': total > 0 ? (completed / total * 100).round() : 0,
    };
  }
}