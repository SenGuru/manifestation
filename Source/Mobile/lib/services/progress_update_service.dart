import 'dart:async';
import 'package:flutter/material.dart';
import '../models/progress_tracker.dart';
import '../models/goals.dart';
import '../models/api_milestone.dart';
import 'goal_tracker_service.dart';

/// Service for managing unified progress updates across goals and milestones
class ProgressUpdateService {
  static final ProgressUpdateService _instance = ProgressUpdateService._internal();
  factory ProgressUpdateService() => _instance;
  ProgressUpdateService._internal();

  final GoalTrackerService _goalService = GoalTrackerService();
  
  // Cache for progress trackers to avoid repeated API calls
  final Map<String, ProgressTracker> _progressCache = {};
  
  // Stream controllers for real-time progress updates
  final StreamController<ProgressTracker> _progressUpdatesController = 
      StreamController<ProgressTracker>.broadcast();
  
  Stream<ProgressTracker> get progressUpdates => _progressUpdatesController.stream;

  // ========================= MAIN PROGRESS OPERATIONS =========================

  /// Get or create progress tracker for a goal
  Future<ProgressTracker> getProgressTracker(String goalId, {bool forceRefresh = false}) async {
    if (!forceRefresh && _progressCache.containsKey(goalId)) {
      return _progressCache[goalId]!;
    }

    try {
      print('📊 Creating progress tracker for goal: $goalId');
      
      // Try to get from API first
      final apiGoalId = int.tryParse(goalId);
      if (apiGoalId != null) {
        final result = await _goalService.getSingleGoalTracker(apiGoalId);
        if (result.isSuccess && result.data != null) {
          final tracker = await _createProgressTrackerFromApi(result.data!);
          _progressCache[goalId] = tracker;
          return tracker;
        }
      }
      
      // Fallback: Create from local Goal model if available
      return _createEmptyProgressTracker(goalId);
      
    } catch (e) {
      print('❌ Error getting progress tracker: $e');
      return _createEmptyProgressTracker(goalId);
    }
  }

  /// Update milestone progress and recalculate goal progress
  Future<ProgressUpdateResult> updateMilestoneProgress(
    String goalId, 
    String milestoneId, 
    double progress,
  ) async {
    try {
      print('📊 Updating milestone progress: Goal=$goalId, Milestone=$milestoneId, Progress=$progress%');
      
      // Get current progress tracker
      final tracker = await getProgressTracker(goalId);
      
      // Update milestone progress locally
      final updatedTracker = tracker.updateMilestoneProgress(milestoneId, progress);
      
      // Try to sync with API if possible
      final milestone = updatedTracker.milestones.firstWhere((m) => m.id == milestoneId);
      if (milestone.apiMilestoneId != null) {
        await _syncMilestoneProgressToApi(milestone);
      }
      
      // Update cache and notify listeners
      _progressCache[goalId] = updatedTracker;
      _progressUpdatesController.add(updatedTracker);
      
      print('✅ Milestone progress updated successfully');
      print('📊 New calculated goal progress: ${updatedTracker.calculatedProgress.round()}%');
      
      return ProgressUpdateResult.success(
        tracker: updatedTracker,
        message: 'Milestone progress updated successfully',
      );
      
    } catch (e) {
      print('❌ Error updating milestone progress: $e');
      return ProgressUpdateResult.failure(
        message: 'Failed to update milestone progress: $e',
      );
    }
  }

  /// Update goal progress manually (override calculated progress)
  Future<ProgressUpdateResult> updateGoalProgress(
    String goalId, 
    double progress, {
    bool overrideCalculation = true,
  }) async {
    try {
      print('📊 Updating goal progress: Goal=$goalId, Progress=$progress%, Override=$overrideCalculation');
      
      // Get current progress tracker
      final tracker = await getProgressTracker(goalId);
      
      // Update goal progress
      final updatedTracker = overrideCalculation 
          ? tracker.setManualProgress(progress)
          : tracker.useCalculatedProgress();
      
      // Try to sync with API if possible
      final apiGoalId = int.tryParse(goalId);
      if (apiGoalId != null) {
        await _syncGoalProgressToApi(apiGoalId, progress);
      }
      
      // Update cache and notify listeners
      _progressCache[goalId] = updatedTracker;
      _progressUpdatesController.add(updatedTracker);
      
      print('✅ Goal progress updated successfully');
      
      return ProgressUpdateResult.success(
        tracker: updatedTracker,
        message: overrideCalculation 
            ? 'Goal progress set to $progress%'
            : 'Switched to calculated progress (${updatedTracker.calculatedProgress.round()}%)',
      );
      
    } catch (e) {
      print('❌ Error updating goal progress: $e');
      return ProgressUpdateResult.failure(
        message: 'Failed to update goal progress: $e',
      );
    }
  }

  /// Calculate goal progress from milestones
  Future<double> calculateGoalProgress(String goalId) async {
    final tracker = await getProgressTracker(goalId);
    return tracker.calculatedProgress;
  }

  /// Mark milestone as completed (sets progress to 100%)
  Future<ProgressUpdateResult> completeMilestone(String goalId, String milestoneId) async {
    return updateMilestoneProgress(goalId, milestoneId, 100.0);
  }

  /// Mark goal as completed (sets progress to 100%)
  Future<ProgressUpdateResult> completeGoal(String goalId) async {
    return updateGoalProgress(goalId, 100.0);
  }

  // ========================= PROGRESS TRACKER CREATION =========================

  /// Create progress tracker from API goal data
  Future<ProgressTracker> _createProgressTrackerFromApi(dynamic apiGoal) async {
    final goalId = apiGoal.goalTrackerId?.toString() ?? 'unknown';
    final goalTitle = apiGoal.goalTrackerName ?? 'Untitled Goal';
    
    // Convert API milestones to MilestoneProgress
    final milestones = <MilestoneProgress>[];
    if (apiGoal.existingGoalTrackerMileStones != null) {
      for (final apiMilestone in apiGoal.existingGoalTrackerMileStones!) {
        milestones.add(MilestoneProgress.fromMilestone(apiMilestone));
      }
    }
    
    // Try to get existing progress from goal data
    double existingProgress = 0.0;
    
    // Check if goal has completion date or status that indicates progress
    if (apiGoal.goalTrackerCompletedDate != null) {
      existingProgress = 100.0;
    } else if (milestones.isNotEmpty) {
      // Use milestone-based calculation as starting point
      final totalProgress = milestones
          .map((m) => m.progressPercentage)
          .reduce((a, b) => a + b);
      existingProgress = totalProgress / milestones.length;
    }
    
    return ProgressTracker(
      goalId: goalId,
      goalTitle: goalTitle,
      manualProgress: existingProgress,
      useManualProgress: false, // Start with calculated progress
      lastUpdated: DateTime.now(),
      milestones: milestones,
    );
  }

  /// Create empty progress tracker for local goals
  ProgressTracker _createEmptyProgressTracker(String goalId) {
    return ProgressTracker(
      goalId: goalId,
      goalTitle: 'Goal $goalId',
      manualProgress: 0.0,
      useManualProgress: false,
      lastUpdated: DateTime.now(),
      milestones: [],
    );
  }

  // ========================= API SYNCHRONIZATION =========================

  /// Sync milestone progress to API
  Future<void> _syncMilestoneProgressToApi(MilestoneProgress milestone) async {
    if (milestone.apiMilestoneId == null) return;
    
    try {
      print('🔄 Syncing milestone progress to API: ID=${milestone.apiMilestoneId}, Progress=${milestone.progressPercentage}%');
      
      // Use existing goal tracker service to update milestone
      // Note: You may need to add progress field to milestone update model
      await _goalService.updateMilestone(
        milestoneId: milestone.apiMilestoneId!,
        milestoneName: milestone.title,
        completedDate: milestone.isCompleted ? DateTime.now() : null,
        // TODO: Add progressPercentage field to API model if not exists
      );
      
      print('✅ Milestone synced to API successfully');
    } catch (e) {
      print('⚠️ Failed to sync milestone to API: $e');
      // Continue without API sync - local progress is still updated
    }
  }

  /// Sync goal progress to API
  Future<void> _syncGoalProgressToApi(int goalId, double progress) async {
    try {
      print('🔄 Syncing goal progress to API: ID=$goalId, Progress=$progress%');
      
      // Update goal with completion date if 100%
      DateTime? completedDate;
      if (progress >= 100.0) {
        completedDate = DateTime.now();
      }
      
      // Use existing goal tracker service to update goal
      await _goalService.updateGoalTracker(
        goalTrackerId: goalId,
        completedDate: completedDate,
        // TODO: Add progress field to API model if not exists
      );
      
      print('✅ Goal synced to API successfully');
    } catch (e) {
      print('⚠️ Failed to sync goal to API: $e');
      // Continue without API sync - local progress is still updated
    }
  }

  // ========================= HELPER METHODS =========================

  /// Get progress summary for multiple goals
  Future<Map<String, ProgressDisplayInfo>> getProgressSummary(List<String> goalIds) async {
    final Map<String, ProgressDisplayInfo> summary = {};
    
    for (final goalId in goalIds) {
      try {
        final tracker = await getProgressTracker(goalId);
        summary[goalId] = tracker.displayInfo;
      } catch (e) {
        print('⚠️ Failed to get progress for goal $goalId: $e');
      }
    }
    
    return summary;
  }

  /// Clear progress cache (useful for refresh)
  void clearCache() {
    _progressCache.clear();
    print('🔄 Progress cache cleared');
  }

  /// Dispose resources
  void dispose() {
    _progressUpdatesController.close();
    _progressCache.clear();
  }

  // ========================= BATCH OPERATIONS =========================

  /// Update multiple milestones at once
  Future<List<ProgressUpdateResult>> updateMultipleMilestones(
    String goalId,
    Map<String, double> milestoneProgressMap,
  ) async {
    final results = <ProgressUpdateResult>[];
    
    for (final entry in milestoneProgressMap.entries) {
      final result = await updateMilestoneProgress(goalId, entry.key, entry.value);
      results.add(result);
    }
    
    return results;
  }

  /// Auto-complete overdue milestones based on rules
  Future<ProgressUpdateResult> autoCompleteOverdueMilestones(String goalId) async {
    try {
      final tracker = await getProgressTracker(goalId);
      var updatedTracker = tracker;
      
      int autoCompleted = 0;
      for (final milestone in tracker.milestones) {
        if (milestone.status == MilestoneStatus.overdue && milestone.progressPercentage < 100.0) {
          updatedTracker = updatedTracker.updateMilestoneProgress(milestone.id, 100.0);
          autoCompleted++;
        }
      }
      
      if (autoCompleted > 0) {
        _progressCache[goalId] = updatedTracker;
        _progressUpdatesController.add(updatedTracker);
        
        return ProgressUpdateResult.success(
          tracker: updatedTracker,
          message: 'Auto-completed $autoCompleted overdue milestones',
        );
      } else {
        return ProgressUpdateResult.success(
          tracker: tracker,
          message: 'No overdue milestones to auto-complete',
        );
      }
      
    } catch (e) {
      return ProgressUpdateResult.failure(
        message: 'Failed to auto-complete milestones: $e',
      );
    }
  }
}

// ========================= RESULT CLASSES =========================

class ProgressUpdateResult {
  final bool isSuccess;
  final String message;
  final ProgressTracker? tracker;

  ProgressUpdateResult._({
    required this.isSuccess,
    required this.message,
    this.tracker,
  });

  factory ProgressUpdateResult.success({
    required ProgressTracker tracker,
    required String message,
  }) {
    return ProgressUpdateResult._(
      isSuccess: true,
      message: message,
      tracker: tracker,
    );
  }

  factory ProgressUpdateResult.failure({
    required String message,
  }) {
    return ProgressUpdateResult._(
      isSuccess: false,
      message: message,
    );
  }
}