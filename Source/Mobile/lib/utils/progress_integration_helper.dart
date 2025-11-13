import 'package:flutter/material.dart';
import 'package:mistel/components/enhanced_progress_update_dialog.dart';

import '../models/goals.dart';
import '../models/progress_tracker.dart';
import '../models/api_milestone.dart';
import '../services/progress_update_service.dart';

/// Helper class to integrate the new progress system with existing Goal models
class ProgressIntegrationHelper {
  static final ProgressUpdateService _progressService = ProgressUpdateService();

  /// Convert existing Goal model to ProgressTracker
  static ProgressTracker goalToProgressTracker(Goal goal) {
    // Convert Goal milestones to MilestoneProgress
    final milestoneProgressList = goal.milestones.map((milestone) {
      return MilestoneProgress.fromMilestone(milestone);
    }).toList();

    return ProgressTracker(
      goalId: goal.id,
      goalTitle: goal.title,
      manualProgress: goal.currentProgress,
      useManualProgress: true, // Existing goals use manual progress by default
      lastUpdated: goal.updatedAt ?? DateTime.now(),
      milestones: milestoneProgressList,
    );
  }

  /// Update existing Goal model with ProgressTracker data
  static Goal updateGoalFromProgressTracker(Goal goal, ProgressTracker tracker) {
    // Convert MilestoneProgress back to Milestone
    final updatedMilestones = tracker.milestones.map((milestoneProgress) {
      // Find matching milestone in original goal
      final originalMilestone = goal.milestones.firstWhere(
        (m) => m.id == milestoneProgress.id,
        orElse: () => Milestone(
          id: milestoneProgress.id,
          title: milestoneProgress.title,
          description: milestoneProgress.description,
          isCompleted: milestoneProgress.isCompleted,
          completedAt: milestoneProgress.isCompleted ? DateTime.now() : null,
          targetDate: milestoneProgress.targetDate,
        ),
      );

      return originalMilestone.copyWith(
        isCompleted: milestoneProgress.isCompleted,
        completedAt: milestoneProgress.isCompleted ? DateTime.now() : null,
      );
    }).toList();

    return goal.copyWith(
      currentProgress: tracker.currentProgress,
      updatedAt: tracker.lastUpdated,
      milestones: updatedMilestones,
    );
  }

  /// Show enhanced progress dialog for existing Goal
  static Future<void> showEnhancedProgressDialog(
    BuildContext context,
    Goal goal,
    Function(Goal updatedGoal) onGoalUpdated,
  ) async {
    // This would import the enhanced dialog and show it
    // Implementation depends on your navigation setup
    
    final result = await showDialog<ProgressTracker>(
      context: context,
      builder: (context) => EnhancedProgressUpdateDialog(
        goal: goal,
        onProgressUpdate: (updatedTracker) {
          Navigator.pop(context, updatedTracker);
        },
      ),
    );

    if (result != null) {
      final updatedGoal = updateGoalFromProgressTracker(goal, result);
      onGoalUpdated(updatedGoal);
    }
  }

  /// Quick method to update goal progress with milestone-based calculation
  static Future<Goal> updateGoalProgressAutomatically(Goal goal) async {
    final tracker = goalToProgressTracker(goal);
    
    // Use calculated progress instead of manual
    final updatedTracker = tracker.useCalculatedProgress();
    
    // Sync with service
    await _progressService.updateGoalProgress(
      goal.id,
      0.0, // Value doesn't matter for calculated mode
      overrideCalculation: false,
    );
    
    return updateGoalFromProgressTracker(goal, updatedTracker);
  }

  /// Convert API milestone data to MilestoneProgress
  static MilestoneProgress apiMilestoneToMilestoneProgress(ApiMilestone apiMilestone) {
    return MilestoneProgress(
      id: apiMilestone.id,
      title: apiMilestone.title,
      description: apiMilestone.description,
      progressPercentage: apiMilestone.progressPercentage,
      isCompleted: apiMilestone.isCompleted,
      targetDate: apiMilestone.targetDate,
      lastUpdated: apiMilestone.modifiedAt,
      apiMilestoneId: int.tryParse(apiMilestone.id),
    );
  }

  /// Create ProgressTracker from API goal data
  static Future<ProgressTracker> createProgressTrackerFromApiGoal(
    dynamic apiGoal,
    List<ApiMilestone>? apiMilestones,
  ) async {
    final milestones = apiMilestones?.map(apiMilestoneToMilestoneProgress).toList() ?? [];
    
    return ProgressTracker(
      goalId: apiGoal.goalTrackerId?.toString() ?? 'unknown',
      goalTitle: apiGoal.goalTrackerName ?? 'Untitled Goal',
      manualProgress: 0.0,
      useManualProgress: false, // Start with calculated progress for API goals
      lastUpdated: DateTime.now(),
      milestones: milestones,
    );
  }

  /// Example usage patterns
  static Future<void> exampleUsage() async {
    // Example 1: Convert existing goal to new progress system
    final existingGoal = Goal.sample1();
    final tracker = goalToProgressTracker(existingGoal);
    
    print('Goal: ${tracker.goalTitle}');
    print('Current Progress: ${tracker.currentProgress}%');
    print('Calculated Progress: ${tracker.calculatedProgress}%');
    print('Milestones: ${tracker.milestones.length}');
    
    // Example 2: Update milestone progress
    final updatedTracker = tracker.updateMilestoneProgress('m1', 75.0);
    print('After milestone update: ${updatedTracker.calculatedProgress}%');
    
    // Example 3: Switch to calculated progress
    final calculatedTracker = updatedTracker.useCalculatedProgress();
    print('Using calculated progress: ${calculatedTracker.currentProgress}%');
    
    // Example 4: Convert back to Goal model
    final updatedGoal = updateGoalFromProgressTracker(existingGoal, calculatedTracker);
    print('Updated goal progress: ${updatedGoal.currentProgress}%');
  }
}

// Extension methods to make integration even easier
extension GoalProgressExtension on Goal {
  /// Convert this Goal to a ProgressTracker
  ProgressTracker toProgressTracker() {
    return ProgressIntegrationHelper.goalToProgressTracker(this);
  }
  
  /// Update this Goal from a ProgressTracker
  Goal fromProgressTracker(ProgressTracker tracker) {
    return ProgressIntegrationHelper.updateGoalFromProgressTracker(this, tracker);
  }
  
  /// Get calculated progress based on milestones
  double get calculatedProgress {
    if (milestones.isEmpty) return currentProgress;
    
    final completedCount = milestones.where((m) => m.isCompleted).length;
    return (completedCount / milestones.length * 100).clamp(0.0, 100.0);
  }
  
  /// Check if manual progress differs from calculated progress
  bool get hasManualProgressOverride {
    return (currentProgress - calculatedProgress).abs() > 5.0; // 5% tolerance
  }
}