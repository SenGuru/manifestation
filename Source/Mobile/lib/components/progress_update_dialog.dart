import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../constants/colors.dart';
import '../models/goals.dart';

class ProgressUpdateDialog extends StatefulWidget {
  final Goal goal;
  final Function(double newProgress) onProgressUpdate;

  const ProgressUpdateDialog({
    Key? key,
    required this.goal,
    required this.onProgressUpdate,
  }) : super(key: key);

  @override
  _ProgressUpdateDialogState createState() => _ProgressUpdateDialogState();
}

class _ProgressUpdateDialogState extends State<ProgressUpdateDialog>
    with SingleTickerProviderStateMixin {
  late double _currentProgress;
  late TextEditingController _progressController;
  late AnimationController _animationController;
  late Animation<double> _progressAnimation;

  @override
  void initState() {
    super.initState();
    _currentProgress = widget.goal.currentProgress;
    _progressController = TextEditingController(
      text: _currentProgress.round().toString(),
    );
    
    _animationController = AnimationController(
      duration: Duration(milliseconds: 300),
      vsync: this,
    );
    _progressAnimation = Tween<double>(
      begin: widget.goal.currentProgress / 100,
      end: _currentProgress / 100,
    ).animate(CurvedAnimation(
      parent: _animationController,
      curve: Curves.easeInOut,
    ));
    
    _animationController.forward();
  }

  @override
  void dispose() {
    _progressController.dispose();
    _animationController.dispose();
    super.dispose();
  }

  void _updateProgress(double newProgress) {
    setState(() {
      _currentProgress = newProgress.clamp(0.0, 100.0);
      _progressController.text = _currentProgress.round().toString();
    });
    
    _progressAnimation = Tween<double>(
      begin: _progressAnimation.value,
      end: _currentProgress / 100,
    ).animate(CurvedAnimation(
      parent: _animationController,
      curve: Curves.easeInOut,
    ));
    
    _animationController.reset();
    _animationController.forward();
  }

  @override
  Widget build(BuildContext context) {
    return Dialog(
      backgroundColor: Colors.transparent,
      child: Container(
        margin: EdgeInsets.all(20.w),
        decoration: BoxDecoration(
          color: AppColors.cardBackground,
          borderRadius: BorderRadius.circular(20.r),
          border: Border.all(
            color: widget.goal.category.color.withOpacity(0.3),
          ),
        ),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            // Header
            Container(
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
              child: Column(
                children: [
                  Row(
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
                                color: AppColors.textLight,
                              ),
                            ),
                            SizedBox(height: 4.h),
                            Text(
                              widget.goal.title,
                              style: TextStyle(
                                fontSize: 14.sp,
                                color: AppColors.textMedium,
                              ),
                              maxLines: 2,
                              overflow: TextOverflow.ellipsis,
                            ),
                          ],
                        ),
                      ),
                    ],
                  ),
                ],
              ),
            ),
            
            // Progress section
            Padding(
              padding: EdgeInsets.all(24.w),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  // Current progress display
                  Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: [
                      Text(
                        'Current Progress',
                        style: TextStyle(
                          fontSize: 16.sp,
                          fontWeight: FontWeight.w600,
                          color: AppColors.textLight,
                        ),
                      ),
                      Text(
                        '${_currentProgress.round()}%',
                        style: TextStyle(
                          fontSize: 24.sp,
                          fontWeight: FontWeight.bold,
                          color: widget.goal.category.color,
                        ),
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
                          color: AppColors.textMedium.withOpacity(0.2),
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
                  
                  SizedBox(height: 24.h),
                  
                  // Progress slider
                  Text(
                    'Adjust Progress',
                    style: TextStyle(
                      fontSize: 14.sp,
                      fontWeight: FontWeight.w500,
                      color: AppColors.textMedium,
                    ),
                  ),
                  SizedBox(height: 12.h),
                  
                  SliderTheme(
                    data: SliderTheme.of(context).copyWith(
                      activeTrackColor: widget.goal.category.color,
                      inactiveTrackColor: AppColors.textMedium.withOpacity(0.2),
                      thumbColor: widget.goal.category.color,
                      overlayColor: widget.goal.category.color.withOpacity(0.2),
                      thumbShape: RoundSliderThumbShape(enabledThumbRadius: 12.r),
                      trackHeight: 6.h,
                    ),
                    child: Slider(
                      value: _currentProgress,
                      min: 0.0,
                      max: 100.0,
                      divisions: 20,
                      onChanged: _updateProgress,
                    ),
                  ),
                  
                  SizedBox(height: 16.h),
                  
                  // Quick progress buttons
                  Row(
                    children: [
                      Text(
                        'Quick Updates:',
                        style: TextStyle(
                          fontSize: 14.sp,
                          fontWeight: FontWeight.w500,
                          color: AppColors.textMedium,
                        ),
                      ),
                      SizedBox(width: 12.w),
                      Expanded(
                        child: Row(
                          children: [
                            _buildQuickButton('+10%', () {
                              _updateProgress(_currentProgress + 10);
                            }),
                            SizedBox(width: 8.w),
                            _buildQuickButton('+25%', () {
                              _updateProgress(_currentProgress + 25);
                            }),
                            SizedBox(width: 8.w),
                            _buildQuickButton('100%', () {
                              _updateProgress(100.0);
                            }),
                          ],
                        ),
                      ),
                    ],
                  ),
                  
                  SizedBox(height: 24.h),
                  
                  // Manual input
                  Row(
                    children: [
                      Text(
                        'Or enter manually:',
                        style: TextStyle(
                          fontSize: 14.sp,
                          fontWeight: FontWeight.w500,
                          color: AppColors.textMedium,
                        ),
                      ),
                      SizedBox(width: 12.w),
                      Container(
                        width: 80.w,
                        height: 40.h,
                        decoration: BoxDecoration(
                          color: AppColors.backgroundMedium,
                          borderRadius: BorderRadius.circular(8.r),
                          border: Border.all(
                            color: AppColors.textMedium.withOpacity(0.3),
                          ),
                        ),
                        child: TextField(
                          controller: _progressController,
                          textAlign: TextAlign.center,
                          keyboardType: TextInputType.number,
                          style: TextStyle(
                            color: AppColors.textLight,
                            fontSize: 14.sp,
                            fontWeight: FontWeight.w600,
                          ),
                          decoration: InputDecoration(
                            border: InputBorder.none,
                            suffix: Text(
                              '%',
                              style: TextStyle(
                                color: AppColors.textMedium,
                                fontSize: 12.sp,
                              ),
                            ),
                          ),
                          onChanged: (value) {
                            final progress = double.tryParse(value);
                            if (progress != null) {
                              _updateProgress(progress);
                            }
                          },
                        ),
                      ),
                    ],
                  ),
                  
                  // Completion status
                  if (_currentProgress >= 100.0)
                    Container(
                      margin: EdgeInsets.only(top: 16.h),
                      padding: EdgeInsets.all(12.w),
                      decoration: BoxDecoration(
                        color: AppColors.accentMint.withOpacity(0.1),
                        borderRadius: BorderRadius.circular(8.r),
                        border: Border.all(
                          color: AppColors.accentMint.withOpacity(0.3),
                        ),
                      ),
                      child: Row(
                        children: [
                          Icon(
                            Icons.celebration,
                            color: AppColors.accentMint,
                            size: 20.sp,
                          ),
                          SizedBox(width: 8.w),
                          Expanded(
                            child: Text(
                              'Congratulations! You\'ve completed this goal!',
                              style: TextStyle(
                                fontSize: 14.sp,
                                color: AppColors.accentMint,
                                fontWeight: FontWeight.w600,
                              ),
                            ),
                          ),
                        ],
                      ),
                    ),
                ],
              ),
            ),
            
            // Action buttons
            Container(
              padding: EdgeInsets.fromLTRB(24.w, 0, 24.w, 24.h),
              child: Row(
                children: [
                  Expanded(
                    child: GestureDetector(
                      onTap: () => Navigator.pop(context),
                      child: Container(
                        height: 48.h,
                        decoration: BoxDecoration(
                          color: AppColors.backgroundMedium,
                          borderRadius: BorderRadius.circular(12.r),
                          border: Border.all(
                            color: AppColors.textMedium.withOpacity(0.3),
                          ),
                        ),
                        child: Center(
                          child: Text(
                            'Cancel',
                            style: TextStyle(
                              fontSize: 16.sp,
                              fontWeight: FontWeight.w600,
                              color: AppColors.textMedium,
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
                      onTap: () {
                        widget.onProgressUpdate(_currentProgress);
                        Navigator.pop(context);
                      },
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
                            'Update Progress',
                            style: TextStyle(
                              fontSize: 16.sp,
                              fontWeight: FontWeight.w600,
                              color: AppColors.white,
                            ),
                          ),
                        ),
                      ),
                    ),
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildQuickButton(String label, VoidCallback onTap) {
    return Expanded(
      child: GestureDetector(
        onTap: onTap,
        child: Container(
          height: 32.h,
          decoration: BoxDecoration(
            color: widget.goal.category.color.withOpacity(0.1),
            borderRadius: BorderRadius.circular(6.r),
            border: Border.all(
              color: widget.goal.category.color.withOpacity(0.3),
            ),
          ),
          child: Center(
            child: Text(
              label,
              style: TextStyle(
                fontSize: 12.sp,
                fontWeight: FontWeight.w600,
                color: widget.goal.category.color,
              ),
            ),
          ),
        ),
      ),
    );
  }
}