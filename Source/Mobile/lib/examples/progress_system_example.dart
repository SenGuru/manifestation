import 'package:flutter/material.dart';
import '../models/goals.dart';
import '../models/progress_tracker.dart';
import '../services/progress_update_service.dart';
import '../utils/progress_integration_helper.dart';
import '../components/enhanced_progress_update_dialog.dart';

/// Example showing how to use the new progress system with existing goals
class ProgressSystemExample extends StatefulWidget {
  @override
  _ProgressSystemExampleState createState() => _ProgressSystemExampleState();
}

class _ProgressSystemExampleState extends State<ProgressSystemExample> {
  final ProgressUpdateService _progressService = ProgressUpdateService();
  Goal? _currentGoal;
  ProgressTracker? _progressTracker;

  @override
  void initState() {
    super.initState();
    _loadExampleGoal();
  }

  void _loadExampleGoal() {
    // Use one of your existing sample goals
    _currentGoal = Goal.sample1(); // "Launch My Dream Business"
    
    // Convert to new progress system
    _progressTracker = _currentGoal!.toProgressTracker();
    
    setState(() {});
    
    print('🎯 Example Goal Loaded:');
    print('  - Title: ${_currentGoal!.title}');
    print('  - Original Progress: ${_currentGoal!.currentProgress}%');
    print('  - Calculated Progress: ${_currentGoal!.calculatedProgress}%');
    print('  - Has Override: ${_currentGoal!.hasManualProgressOverride}');
    print('  - Milestones: ${_currentGoal!.milestones.length}');
  }

  void _showEnhancedProgressDialog() {
    if (_currentGoal == null) return;

    showDialog(
      context: context,
      builder: (context) => EnhancedProgressUpdateDialog(
        goal: _currentGoal!,
        onProgressUpdate: (updatedTracker) {
          setState(() {
            _progressTracker = updatedTracker;
            _currentGoal = _currentGoal!.fromProgressTracker(updatedTracker);
          });

          print('🎯 Progress Updated:');
          print('  - New Progress: ${updatedTracker.currentProgress}%');
          print('  - Is Manual: ${updatedTracker.useManualProgress}');
          print('  - Calculated: ${updatedTracker.calculatedProgress}%');
        },
      ),
    );
  }

  void _demoProgressUpdates() async {
    if (_progressTracker == null) return;

    print('🎯 Demo: Updating milestone progress...');
    
    // Update first milestone to 50%
    final milestone1Id = _progressTracker!.milestones.first.id;
    final result1 = await _progressService.updateMilestoneProgress(
      _progressTracker!.goalId, 
      milestone1Id, 
      50.0
    );
    
    if (result1.isSuccess) {
      setState(() {
        _progressTracker = result1.tracker!;
        _currentGoal = _currentGoal!.fromProgressTracker(result1.tracker!);
      });
      print('✅ Milestone 1 updated to 50%');
      print('  - New goal progress: ${result1.tracker!.currentProgress}%');
    }

    // Wait a moment, then update second milestone
    await Future.delayed(Duration(seconds: 1));
    
    if (_progressTracker!.milestones.length > 1) {
      final milestone2Id = _progressTracker!.milestones[1].id;
      final result2 = await _progressService.updateMilestoneProgress(
        _progressTracker!.goalId, 
        milestone2Id, 
        75.0
      );
      
      if (result2.isSuccess) {
        setState(() {
          _progressTracker = result2.tracker!;
          _currentGoal = _currentGoal!.fromProgressTracker(result2.tracker!);
        });
        print('✅ Milestone 2 updated to 75%');
        print('  - New goal progress: ${result2.tracker!.currentProgress}%');
      }
    }
  }

  void _toggleProgressMode() async {
    if (_progressTracker == null) return;

    final result = await _progressService.updateGoalProgress(
      _progressTracker!.goalId,
      _progressTracker!.useManualProgress ? 0.0 : 80.0,
      overrideCalculation: !_progressTracker!.useManualProgress,
    );

    if (result.isSuccess) {
      setState(() {
        _progressTracker = result.tracker!;
        _currentGoal = _currentGoal!.fromProgressTracker(result.tracker!);
      });
      
      print('🎯 Progress mode toggled:');
      print('  - Mode: ${result.tracker!.useManualProgress ? "Manual" : "Calculated"}');
      print('  - Progress: ${result.tracker!.currentProgress}%');
    }
  }

  @override
  Widget build(BuildContext context) {
    if (_currentGoal == null || _progressTracker == null) {
      return Scaffold(
        appBar: AppBar(title: Text('Progress System Example')),
        body: Center(child: CircularProgressIndicator()),
      );
    }

    final displayInfo = _progressTracker!.displayInfo;

    return Scaffold(
      appBar: AppBar(
        title: Text('Progress System Example'),
        backgroundColor: _currentGoal!.category.color,
      ),
      body: Padding(
        padding: EdgeInsets.all(16),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            // Goal Header
            Card(
              child: Padding(
                padding: EdgeInsets.all(16),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Row(
                      children: [
                        Icon(_currentGoal!.category.icon, 
                             color: _currentGoal!.category.color),
                        SizedBox(width: 8),
                        Expanded(
                          child: Text(
                            _currentGoal!.title,
                            style: TextStyle(
                              fontSize: 18,
                              fontWeight: FontWeight.bold,
                            ),
                          ),
                        ),
                      ],
                    ),
                    SizedBox(height: 8),
                    Text(_currentGoal!.description),
                  ],
                ),
              ),
            ),

            SizedBox(height: 16),

            // Progress Display
            Card(
              child: Padding(
                padding: EdgeInsets.all(16),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Row(
                      mainAxisAlignment: MainAxisAlignment.spaceBetween,
                      children: [
                        Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            Text(
                              'Progress',
                              style: TextStyle(
                                fontSize: 16,
                                fontWeight: FontWeight.bold,
                              ),
                            ),
                            Text(
                              displayInfo.isManualOverride 
                                  ? 'Manual Override' 
                                  : 'Auto-calculated',
                              style: TextStyle(
                                fontSize: 12,
                                color: Colors.grey[600],
                                fontStyle: FontStyle.italic,
                              ),
                            ),
                          ],
                        ),
                        Column(
                          crossAxisAlignment: CrossAxisAlignment.end,
                          children: [
                            Text(
                              displayInfo.progressText,
                              style: TextStyle(
                                fontSize: 24,
                                fontWeight: FontWeight.bold,
                                color: _currentGoal!.category.color,
                              ),
                            ),
                            Text(
                              displayInfo.milestoneText,
                              style: TextStyle(
                                fontSize: 12,
                                color: Colors.grey[600],
                              ),
                            ),
                          ],
                        ),
                      ],
                    ),
                    
                    SizedBox(height: 12),
                    
                    // Progress bar
                    LinearProgressIndicator(
                      value: displayInfo.currentProgress / 100,
                      backgroundColor: Colors.grey[300],
                      valueColor: AlwaysStoppedAnimation(
                        _currentGoal!.category.color,
                      ),
                    ),
                    
                    if (displayInfo.isManualOverride) ...[
                      SizedBox(height: 8),
                      Text(
                        'Calculated: ${displayInfo.calculatedProgress.round()}%',
                        style: TextStyle(
                          fontSize: 12,
                          color: Colors.grey[600],
                        ),
                      ),
                    ],
                  ],
                ),
              ),
            ),

            SizedBox(height: 16),

            // Milestones
            if (_progressTracker!.milestones.isNotEmpty) ...[
              Card(
                child: Padding(
                  padding: EdgeInsets.all(16),
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(
                        'Milestones',
                        style: TextStyle(
                          fontSize: 16,
                          fontWeight: FontWeight.bold,
                        ),
                      ),
                      SizedBox(height: 12),
                      
                      ..._progressTracker!.milestones.map((milestone) {
                        return Padding(
                          padding: EdgeInsets.only(bottom: 8),
                          child: Row(
                            children: [
                              Icon(
                                milestone.statusIcon,
                                color: milestone.statusColor,
                                size: 16,
                              ),
                              SizedBox(width: 8),
                              Expanded(child: Text(milestone.title)),
                              Text(
                                '${milestone.progressPercentage.round()}%',
                                style: TextStyle(
                                  fontWeight: FontWeight.bold,
                                  color: milestone.statusColor,
                                ),
                              ),
                            ],
                          ),
                        );
                      }).toList(),
                    ],
                  ),
                ),
              ),

              SizedBox(height: 16),
            ],

            // Action Buttons
            Column(
              children: [
                SizedBox(
                  width: double.infinity,
                  child: ElevatedButton(
                    onPressed: _showEnhancedProgressDialog,
                    style: ElevatedButton.styleFrom(
                      backgroundColor: _currentGoal!.category.color,
                      foregroundColor: Colors.white,
                    ),
                    child: Text('Update Progress (New Dialog)'),
                  ),
                ),
                
                SizedBox(height: 8),
                
                Row(
                  children: [
                    Expanded(
                      child: ElevatedButton(
                        onPressed: _demoProgressUpdates,
                        child: Text('Demo Updates'),
                      ),
                    ),
                    SizedBox(width: 8),
                    Expanded(
                      child: ElevatedButton(
                        onPressed: _toggleProgressMode,
                        child: Text(
                          _progressTracker!.useManualProgress 
                              ? 'Use Calculated' 
                              : 'Use Manual',
                        ),
                      ),
                    ),
                  ],
                ),
              ],
            ),

            SizedBox(height: 24),

            // Debug Info
            Expanded(
              child: Card(
                child: Padding(
                  padding: EdgeInsets.all(16),
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(
                        'Debug Info',
                        style: TextStyle(
                          fontSize: 16,
                          fontWeight: FontWeight.bold,
                        ),
                      ),
                      SizedBox(height: 8),
                      Expanded(
                        child: SingleChildScrollView(
                          child: Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              Text('Goal ID: ${_progressTracker!.goalId}'),
                              Text('Manual Progress: ${_progressTracker!.manualProgress}%'),
                              Text('Calculated Progress: ${_progressTracker!.calculatedProgress}%'),
                              Text('Current Progress: ${_progressTracker!.currentProgress}%'),
                              Text('Use Manual: ${_progressTracker!.useManualProgress}'),
                              Text('Milestones: ${_progressTracker!.milestones.length}'),
                              Text('Is Completed: ${displayInfo.isCompleted}'),
                              Text('Status: ${displayInfo.statusText}'),
                              Text('Has Override: ${displayInfo.isManualOverride}'),
                              Text('Last Updated: ${_progressTracker!.lastUpdated}'),
                            ],
                          ),
                        ),
                      ),
                    ],
                  ),
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }

  @override
  void dispose() {
    _progressService.dispose();
    super.dispose();
  }
}