import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../constants/app_theme.dart';
import '../models/goals.dart';
import '../models/progress_tracker.dart';
import '../services/progress_update_service.dart';

class EnhancedProgressUpdateDialog extends StatefulWidget {
  final Goal goal;
  final Function(ProgressTracker updatedTracker) onProgressUpdate;

  const EnhancedProgressUpdateDialog({
    Key? key,
    required this.goal,
    required this.onProgressUpdate,
  }) : super(key: key);

  @override
  _EnhancedProgressUpdateDialogState createState() => _EnhancedProgressUpdateDialogState();
}

class _EnhancedProgressUpdateDialogState extends State<EnhancedProgressUpdateDialog>
    with SingleTickerProviderStateMixin {
  late ProgressUpdateService _progressService;
  late AnimationController _animationController;
  late Animation<double> _progressAnimation;
  
  ProgressTracker? _progressTracker;
  bool _isLoading = true;
  bool _useManualProgress = false;
  double _manualProgress = 0.0;
  final Map<String, double> _milestoneProgressUpdates = {};

  @override
  void initState() {
    super.initState();
    _progressService = ProgressUpdateService();
    
    _animationController = AnimationController(
      duration: Duration(milliseconds: 300),
      vsync: this,
    );
    
    _loadProgressTracker();
  }

  @override
  void dispose() {
    _animationController.dispose();
    super.dispose();
  }

  Future<void> _loadProgressTracker() async {
    try {
      final tracker = await _progressService.getProgressTracker(widget.goal.id);
      setState(() {
        _progressTracker = tracker;
        _useManualProgress = tracker.useManualProgress;
        _manualProgress = tracker.manualProgress;
        _isLoading = false;
      });
      
      _updateProgressAnimation();
    } catch (e) {
      setState(() {
        _isLoading = false;
      });
      print('❌ Error loading progress tracker: $e');
    }
  }

  void _updateProgressAnimation() {
    if (_progressTracker == null) return;
    
    _progressAnimation = Tween<double>(
      begin: _progressAnimation?.value ?? 0.0,
      end: _progressTracker!.currentProgress / 100,
    ).animate(CurvedAnimation(
      parent: _animationController,
      curve: Curves.easeInOut,
    ));
    
    _animationController.reset();
    _animationController.forward();
  }

  Future<void> _updateMilestoneProgress(String milestoneId, double progress) async {
    if (_progressTracker == null) return;
    
    setState(() {
      _milestoneProgressUpdates[milestoneId] = progress;
    });
    
    // Update locally first for immediate UI feedback
    final updatedTracker = _progressTracker!.updateMilestoneProgress(milestoneId, progress);
    setState(() {
      _progressTracker = updatedTracker;
    });
    
    _updateProgressAnimation();
    
    // Sync to service in background
    _progressService.updateMilestoneProgress(widget.goal.id, milestoneId, progress);
  }

  Future<void> _updateGoalProgress(double progress) async {
    if (_progressTracker == null) return;
    
    setState(() {
      _manualProgress = progress;
      _useManualProgress = true;
    });
    
    // Update locally first
    final updatedTracker = _progressTracker!.setManualProgress(progress);
    setState(() {
      _progressTracker = updatedTracker;
    });
    
    _updateProgressAnimation();
  }

  Future<void> _switchToCalculatedProgress() async {
    if (_progressTracker == null) return;
    
    setState(() {
      _useManualProgress = false;
    });
    
    final updatedTracker = _progressTracker!.useCalculatedProgress();
    setState(() {
      _progressTracker = updatedTracker;
    });
    
    _updateProgressAnimation();
  }

  Future<void> _saveProgress() async {
    if (_progressTracker == null) return;
    
    try {
      // Apply all pending milestone updates
      for (final entry in _milestoneProgressUpdates.entries) {
        await _progressService.updateMilestoneProgress(
          widget.goal.id, 
          entry.key, 
          entry.value,
        );
      }
      
      // Apply goal progress update if manual
      if (_useManualProgress) {
        await _progressService.updateGoalProgress(
          widget.goal.id, 
          _manualProgress,
          overrideCalculation: true,
        );
      } else {
        await _progressService.updateGoalProgress(
          widget.goal.id, 
          0.0, // Value doesn't matter
          overrideCalculation: false,
        );
      }
      
      // Get final tracker state
      final finalTracker = await _progressService.getProgressTracker(
        widget.goal.id, 
        forceRefresh: true,
      );
      
      widget.onProgressUpdate(finalTracker);
      Navigator.pop(context);
      
    } catch (e) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Failed to save progress: $e')),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    if (_isLoading) {
      return Dialog(
        backgroundColor: Colors.transparent,
        child: Container(
          padding: EdgeInsets.all(40.w),
          decoration: BoxDecoration(
            color: AppTheme.cardBackground,
            borderRadius: BorderRadius.circular(20.r),
          ),
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              CircularProgressIndicator(color: widget.goal.category.color),
              SizedBox(height: 16.h),
              Text(
                'Loading progress data...',
                style: TextStyle(
                  color: AppTheme.textLight,
                  fontSize: 16.sp,
                ),
              ),
            ],
          ),
        ),
      );
    }

    if (_progressTracker == null) {
      return Dialog(
        backgroundColor: Colors.transparent,
        child: Container(
          padding: EdgeInsets.all(24.w),
          decoration: BoxDecoration(
            color: AppTheme.cardBackground,
            borderRadius: BorderRadius.circular(20.r),
          ),
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              Icon(Icons.error, color: Colors.red, size: 48.sp),
              SizedBox(height: 16.h),
              Text(
                'Failed to load progress data',
                style: TextStyle(
                  color: AppTheme.textLight,
                  fontSize: 16.sp,
                ),
              ),
              SizedBox(height: 16.h),
              ElevatedButton(
                onPressed: () => Navigator.pop(context),
                child: Text('Close'),
              ),
            ],
          ),
        ),
      );
    }

    final displayInfo = _progressTracker!.displayInfo;

    return Dialog(
      backgroundColor: Colors.transparent,
      child: Container(
        margin: EdgeInsets.all(20.w),
        decoration: BoxDecoration(
          color: AppTheme.cardBackground,
          borderRadius: BorderRadius.circular(20.r),
          border: Border.all(
            color: widget.goal.category.color.withOpacity(0.3),
          ),
        ),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            _buildHeader(),
            _buildProgressSection(displayInfo),
            if (_progressTracker!.milestones.isNotEmpty) ...[
              _buildMilestonesSection(),
            ],
            _buildProgressModeSelector(),
            _buildActionButtons(),
          ],
        ),
      ),
    );
  }

  Widget _buildHeader() {
    return Container(
      padding: EdgeInsets.all(24.w),
      decoration: BoxDecoration(
        gradient: LinearGradient(
          colors: [
            widget.goal.category.color.withOpacity(0.1),
            widget.goal.category.color.withOpacity(0.05),
          ],
          begin: Alignment.topLeft,
          end: Alignment.bottomRight,
        ),
        borderRadius: BorderRadius.vertical(top: Radius.circular(20.r)),
      ),
      child: Row(
        children: [
          Container(
            width: 50.w,
            height: 50.h,
            decoration: BoxDecoration(
              color: widget.goal.category.color.withOpacity(0.1),
              borderRadius: BorderRadius.circular(12.r),
            ),
            child: Icon(
              widget.goal.category.icon,
              color: widget.goal.category.color,
              size: 24.sp,
            ),
          ),
          SizedBox(width: 16.w),
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  'Update Progress',
                  style: TextStyle(
                    fontSize: 18.sp,
                    fontWeight: FontWeight.bold,
                    color: AppTheme.textLight,
                  ),
                ),
                SizedBox(height: 4.h),
                Text(
                  widget.goal.title,
                  style: TextStyle(
                    fontSize: 14.sp,
                    color: AppTheme.textMedium,
                  ),
                  maxLines: 2,
                  overflow: TextOverflow.ellipsis,
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildProgressSection(ProgressDisplayInfo displayInfo) {
    return Padding(
      padding: EdgeInsets.all(24.w),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          // Current progress display
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    'Current Progress',
                    style: TextStyle(
                      fontSize: 16.sp,
                      fontWeight: FontWeight.w600,
                      color: AppTheme.textLight,
                    ),
                  ),
                  if (displayInfo.isManualOverride) ...[
                    SizedBox(height: 4.h),
                    Row(
                      children: [
                        Icon(Icons.edit, size: 12.sp, color: AppTheme.textMedium),
                        SizedBox(width: 4.w),
                        Text(
                          'Manual override',
                          style: TextStyle(
                            fontSize: 12.sp,
                            color: AppTheme.textMedium,
                            fontStyle: FontStyle.italic,
                          ),
                        ),
                      ],
                    ),
                  ],
                ],
              ),
              Column(
                crossAxisAlignment: CrossAxisAlignment.end,
                children: [
                  Text(
                    displayInfo.progressText,
                    style: TextStyle(
                      fontSize: 24.sp,
                      fontWeight: FontWeight.bold,
                      color: widget.goal.category.color,
                    ),
                  ),
                  if (_progressTracker!.milestones.isNotEmpty) ...[
                    Text(
                      displayInfo.milestoneText,
                      style: TextStyle(
                        fontSize: 12.sp,
                        color: AppTheme.textMedium,
                      ),
                    ),
                  ],
                ],
              ),
            ],
          ),
          
          SizedBox(height: 16.h),
          
          // Animated progress bar
          AnimatedBuilder(
            animation: _progressAnimation,
            builder: (context, child) {
              return Container(
                height: 12.h,
                decoration: BoxDecoration(
                  color: AppTheme.textMedium.withOpacity(0.2),
                  borderRadius: BorderRadius.circular(6.r),
                ),
                child: FractionallySizedBox(
                  alignment: Alignment.centerLeft,
                  widthFactor: _progressAnimation.value,
                  child: Container(
                    decoration: BoxDecoration(
                      gradient: LinearGradient(
                        colors: [
                          widget.goal.category.color,
                          widget.goal.category.color.withOpacity(0.8),
                        ],
                      ),
                      borderRadius: BorderRadius.circular(6.r),
                    ),
                  ),
                ),
              );
            },
          ),
          
          // Show calculated vs manual progress comparison
          if (displayInfo.isManualOverride) ...[
            SizedBox(height: 12.h),
            Row(
              children: [
                Icon(Icons.calculate, size: 14.sp, color: AppTheme.textMedium),
                SizedBox(width: 4.w),
                Text(
                  'Calculated from milestones: ${displayInfo.calculatedProgress.round()}%',
                  style: TextStyle(
                    fontSize: 12.sp,
                    color: AppTheme.textMedium,
                  ),
                ),
              ],
            ),
          ],
        ],
      ),
    );
  }

  Widget _buildMilestonesSection() {
    return Container(
      margin: EdgeInsets.symmetric(horizontal: 24.w),
      padding: EdgeInsets.all(16.w),
      decoration: BoxDecoration(
        color: AppTheme.backgroundMedium.withOpacity(0.5),
        borderRadius: BorderRadius.circular(12.r),
        border: Border.all(
          color: widget.goal.category.color.withOpacity(0.2),
        ),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            children: [
              Icon(
                Icons.flag,
                color: widget.goal.category.color,
                size: 18.sp,
              ),
              SizedBox(width: 8.w),
              Text(
                'Milestones',
                style: TextStyle(
                  fontSize: 16.sp,
                  fontWeight: FontWeight.w600,
                  color: AppTheme.textLight,
                ),
              ),
            ],
          ),
          SizedBox(height: 16.h),
          
          ..._progressTracker!.milestones.map((milestone) {
            final currentProgress = _milestoneProgressUpdates[milestone.id] ?? 
                                  milestone.progressPercentage;
            
            return Container(
              margin: EdgeInsets.only(bottom: 16.h),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Row(
                    children: [
                      Icon(
                        milestone.statusIcon,
                        color: milestone.statusColor,
                        size: 16.sp,
                      ),
                      SizedBox(width: 8.w),
                      Expanded(
                        child: Text(
                          milestone.title,
                          style: TextStyle(
                            fontSize: 14.sp,
                            fontWeight: FontWeight.w500,
                            color: AppTheme.textLight,
                          ),
                        ),
                      ),
                      Text(
                        '${currentProgress.round()}%',
                        style: TextStyle(
                          fontSize: 14.sp,
                          fontWeight: FontWeight.bold,
                          color: milestone.statusColor,
                        ),
                      ),
                    ],
                  ),
                  SizedBox(height: 8.h),
                  
                  // Milestone progress slider
                  SliderTheme(
                    data: SliderTheme.of(context).copyWith(
                      activeTrackColor: milestone.statusColor,
                      inactiveTrackColor: AppTheme.textMedium.withOpacity(0.2),
                      thumbColor: milestone.statusColor,
                      overlayColor: milestone.statusColor.withOpacity(0.2),
                      thumbShape: RoundSliderThumbShape(enabledThumbRadius: 8.r),
                      trackHeight: 4.h,
                    ),
                    child: Slider(
                      value: currentProgress,
                      min: 0.0,
                      max: 100.0,
                      divisions: 20,
                      onChanged: (value) {
                        _updateMilestoneProgress(milestone.id, value);
                      },
                    ),
                  ),
                  
                  // Quick milestone buttons
                  Row(
                    children: [
                      _buildMilestoneQuickButton('25%', () {
                        _updateMilestoneProgress(milestone.id, 25.0);
                      }),
                      SizedBox(width: 8.w),
                      _buildMilestoneQuickButton('50%', () {
                        _updateMilestoneProgress(milestone.id, 50.0);
                      }),
                      SizedBox(width: 8.w),
                      _buildMilestoneQuickButton('75%', () {
                        _updateMilestoneProgress(milestone.id, 75.0);
                      }),
                      SizedBox(width: 8.w),
                      _buildMilestoneQuickButton('Done', () {
                        _updateMilestoneProgress(milestone.id, 100.0);
                      }),
                    ],
                  ),
                ],
              ),
            );
          }).toList(),
        ],
      ),
    );
  }

  Widget _buildMilestoneQuickButton(String label, VoidCallback onTap) {
    return Expanded(
      child: GestureDetector(
        onTap: onTap,
        child: Container(
          height: 24.h,
          decoration: BoxDecoration(
            color: widget.goal.category.color.withOpacity(0.1),
            borderRadius: BorderRadius.circular(4.r),
            border: Border.all(
              color: widget.goal.category.color.withOpacity(0.3),
            ),
          ),
          child: Center(
            child: Text(
              label,
              style: TextStyle(
                fontSize: 10.sp,
                fontWeight: FontWeight.w600,
                color: widget.goal.category.color,
              ),
            ),
          ),
        ),
      ),
    );
  }

  Widget _buildProgressModeSelector() {
    return Container(
      margin: EdgeInsets.all(24.w),
      padding: EdgeInsets.all(16.w),
      decoration: BoxDecoration(
        color: AppTheme.backgroundMedium.withOpacity(0.3),
        borderRadius: BorderRadius.circular(12.r),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(
            'Progress Calculation',
            style: TextStyle(
              fontSize: 14.sp,
              fontWeight: FontWeight.w600,
              color: AppTheme.textLight,
            ),
          ),
          SizedBox(height: 12.h),
          
          // Calculated progress option
          GestureDetector(
            onTap: _switchToCalculatedProgress,
            child: Row(
              children: [
                Radio<bool>(
                  value: false,
                  groupValue: _useManualProgress,
                  onChanged: (value) => _switchToCalculatedProgress(),
                  activeColor: widget.goal.category.color,
                ),
                Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(
                        'Auto-calculate from milestones',
                        style: TextStyle(
                          fontSize: 13.sp,
                          fontWeight: FontWeight.w500,
                          color: AppTheme.textLight,
                        ),
                      ),
                      Text(
                        'Progress = average of milestone completion',
                        style: TextStyle(
                          fontSize: 11.sp,
                          color: AppTheme.textMedium,
                        ),
                      ),
                    ],
                  ),
                ),
              ],
            ),
          ),
          
          // Manual progress option
          GestureDetector(
            onTap: () => setState(() => _useManualProgress = true),
            child: Row(
              children: [
                Radio<bool>(
                  value: true,
                  groupValue: _useManualProgress,
                  onChanged: (value) => setState(() => _useManualProgress = true),
                  activeColor: widget.goal.category.color,
                ),
                Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(
                        'Set manual progress',
                        style: TextStyle(
                          fontSize: 13.sp,
                          fontWeight: FontWeight.w500,
                          color: AppTheme.textLight,
                        ),
                      ),
                      Text(
                        'Override automatic calculation',
                        style: TextStyle(
                          fontSize: 11.sp,
                          color: AppTheme.textMedium,
                        ),
                      ),
                    ],
                  ),
                ),
              ],
            ),
          ),
          
          // Manual progress slider (shown when manual mode is selected)
          if (_useManualProgress) ...[
            SizedBox(height: 16.h),
            SliderTheme(
              data: SliderTheme.of(context).copyWith(
                activeTrackColor: widget.goal.category.color,
                inactiveTrackColor: AppTheme.textMedium.withOpacity(0.2),
                thumbColor: widget.goal.category.color,
                overlayColor: widget.goal.category.color.withOpacity(0.2),
                thumbShape: RoundSliderThumbShape(enabledThumbRadius: 10.r),
                trackHeight: 6.h,
              ),
              child: Slider(
                value: _manualProgress,
                min: 0.0,
                max: 100.0,
                divisions: 20,
                onChanged: _updateGoalProgress,
              ),
            ),
          ],
        ],
      ),
    );
  }

  Widget _buildActionButtons() {
    return Container(
      padding: EdgeInsets.fromLTRB(24.w, 0, 24.w, 24.h),
      child: Row(
        children: [
          Expanded(
            child: GestureDetector(
              onTap: () => Navigator.pop(context),
              child: Container(
                height: 48.h,
                decoration: BoxDecoration(
                  color: AppTheme.backgroundMedium,
                  borderRadius: BorderRadius.circular(12.r),
                  border: Border.all(
                    color: AppTheme.textMedium.withOpacity(0.3),
                  ),
                ),
                child: Center(
                  child: Text(
                    'Cancel',
                    style: TextStyle(
                      fontSize: 16.sp,
                      fontWeight: FontWeight.w600,
                      color: AppTheme.textMedium,
                    ),
                  ),
                ),
              ),
            ),
          ),
          SizedBox(width: 16.w),
          Expanded(
            flex: 2,
            child: GestureDetector(
              onTap: _saveProgress,
              child: Container(
                height: 48.h,
                decoration: BoxDecoration(
                  gradient: LinearGradient(
                    colors: [
                      widget.goal.category.color,
                      widget.goal.category.color.withOpacity(0.8),
                    ],
                  ),
                  borderRadius: BorderRadius.circular(12.r),
                ),
                child: Center(
                  child: Text(
                    'Save Progress',
                    style: TextStyle(
                      fontSize: 16.sp,
                      fontWeight: FontWeight.w600,
                      color: AppTheme.white,
                    ),
                  ),
                ),
              ),
            ),
          ),
        ],
      ),
    );
  }
}