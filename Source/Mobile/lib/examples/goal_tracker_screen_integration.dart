// Example of how to integrate the new progress system into your existing goal_tracker_screen.dart

// 1. ADD THESE IMPORTS at the top of goal_tracker_screen.dart:
// import '../components/enhanced_progress_update_dialog.dart';  // NEW: Enhanced dialog
// import '../models/progress_tracker.dart';                     // NEW: Progress models
// import '../services/progress_update_service.dart';            // NEW: Progress service
// import '../utils/progress_integration_helper.dart';           // NEW: Integration helper

// 2. ADD THIS FIELD to _GoalTrackerScreenState class:
// class _GoalTrackerScreenState extends State<GoalTrackerScreen> {
//   // ... existing fields ...
//   
//   final ProgressUpdateService _progressUpdateService = ProgressUpdateService(); // NEW: Add this line
//   
//   // ... rest of existing code ...
// }

// 3. REPLACE THE _updateGoalProgress method with this improved version:

/*
void _updateGoalProgress(Goal goal) {
  print('📊 Opening enhanced progress update dialog for: ${goal.title}');
  print('   Current progress: ${goal.currentProgress.toStringAsFixed(1)}%');
  print('   Calculated progress: ${goal.calculatedProgress.toStringAsFixed(1)}%');
  print('   Has manual override: ${goal.hasManualProgressOverride}');
  
  showDialog(
    context: context,
    builder: (context) => EnhancedProgressUpdateDialog(  // CHANGED: Use new dialog
      goal: goal,
      onProgressUpdate: (ProgressTracker updatedTracker) async {  // CHANGED: Receive ProgressTracker
        print('📊 Enhanced progress update submitted:');
        print('   Goal: ${goal.title}');
        print('   Old Progress: ${goal.currentProgress.toStringAsFixed(1)}%');
        print('   New Progress: ${updatedTracker.currentProgress.toStringAsFixed(1)}%');
        print('   Progress Mode: ${updatedTracker.useManualProgress ? "Manual" : "Calculated"}');
        print('   Is Completed: ${updatedTracker.currentProgress >= 100.0}');
        
        // Convert back to Goal model for existing code compatibility
        final updatedGoal = goal.fromProgressTracker(updatedTracker);  // NEW: Use helper
        
        // Update goal progress using the existing service
        await _updateGoalWithService(
          goalId: goal.id,
          isCompleted: updatedTracker.currentProgress >= 100.0,
        );
        
        // Update the local goals list with the new progress
        setState(() {
          final index = goals.indexWhere((g) => g.id == goal.id);
          if (index != -1) {
            goals[index] = updatedGoal;  // CHANGED: Use updated goal
          }
        });
        
        // Show success message with more detail
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text(
              updatedTracker.useManualProgress 
                  ? 'Progress set to ${updatedTracker.currentProgress.round()}%'
                  : 'Progress calculated as ${updatedTracker.currentProgress.round()}% from milestones',
            ),
            backgroundColor: goal.category.color,
            duration: Duration(seconds: 3),
          ),
        );
        
        print('✅ Goal progress updated successfully');
      },
    ),
  );
}
*/

// 4. OPTIONAL: Add this helper method to show progress statistics:

/*
void _showProgressStatistics() {
  final totalGoals = goals.length;
  final completedGoals = goals.where((g) => g.isCompleted).length;
  final inProgressGoals = goals.where((g) => g.currentProgress > 0 && !g.isCompleted).length;
  final notStartedGoals = goals.where((g) => g.currentProgress == 0).length;
  final overrideGoals = goals.where((g) => g.hasManualProgressOverride).length;
  
  final avgProgress = totalGoals > 0 
      ? goals.map((g) => g.currentProgress).reduce((a, b) => a + b) / totalGoals
      : 0.0;
  
  final avgCalculatedProgress = totalGoals > 0 
      ? goals.map((g) => g.calculatedProgress).reduce((a, b) => a + b) / totalGoals
      : 0.0;
  
  showDialog(
    context: context,
    builder: (context) => AlertDialog(
      title: Text('Progress Statistics'),
      content: Column(
        mainAxisSize: MainAxisSize.min,
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text('📊 Total Goals: $totalGoals'),
          Text('✅ Completed: $completedGoals'),
          Text('🔄 In Progress: $inProgressGoals'),
          Text('⏸️ Not Started: $notStartedGoals'),
          Text('✏️ Manual Overrides: $overrideGoals'),
          Divider(),
          Text('📈 Average Progress: ${avgProgress.round()}%'),
          Text('🧮 Average Calculated: ${avgCalculatedProgress.round()}%'),
        ],
      ),
      actions: [
        TextButton(
          onPressed: () => Navigator.pop(context),
          child: Text('Close'),
        ),
      ],
    ),
  );
}
*/

// 5. OPTIONAL: Add this to dispose method:

/*
@override
void dispose() {
  _fabAnimationController.dispose();
  _progressUpdateService.dispose();  // NEW: Dispose progress service
  super.dispose();
}
*/

/* 
🎯 SUMMARY OF CHANGES:

1. IMPORTS: Add 4 new imports for enhanced progress system
2. FIELD: Add ProgressUpdateService instance
3. METHOD: Replace _updateGoalProgress with enhanced version
4. HELPER: Optional progress statistics method
5. CLEANUP: Dispose progress service

🚀 BENEFITS:

✅ Milestone-based progress tracking
✅ Visual milestone sliders in dialog
✅ Choice between calculated vs manual progress
✅ Real-time progress animations
✅ Better progress display with override indicators
✅ Backward compatible with existing Goal model
✅ No breaking changes to existing code

🔄 MIGRATION STEPS:

1. Copy the imports and field addition
2. Replace the _updateGoalProgress method
3. Test with existing goals
4. Optionally add statistics feature
5. All existing functionality continues to work!

The new system is completely backward compatible - your existing goals will work exactly the same, but now users get the enhanced milestone-based progress tracking!
*/