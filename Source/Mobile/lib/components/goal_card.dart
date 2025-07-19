import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:fun/constants/colors.dart';
import 'package:fun/models/goals.dart';

class GoalCard extends StatefulWidget {
  final Goal goal;
  final VoidCallback onTap;
  final VoidCallback onProgressUpdate;
  final VoidCallback onEdit;
  final VoidCallback onDelete;

  const GoalCard({
    Key? key,
    required this.goal,
    required this.onTap,
    required this.onProgressUpdate,
    required this.onEdit,
    required this.onDelete,
  }) : super(key: key);

  @override
  _GoalCardState createState() => _GoalCardState();
}

class _GoalCardState extends State<GoalCard>
    with SingleTickerProviderStateMixin {
  late AnimationController _progressController;
  late Animation<double> _progressAnimation;
  bool isExpanded = false;

  @override
  void initState() {
    super.initState();
    _progressController = AnimationController(
      duration: Duration(milliseconds: 1000),
      vsync: this,
    );
    _progressAnimation = Tween<double>(
      begin: 0.0,
      end: widget.goal.progressPercentage / 100,
    ).animate(CurvedAnimation(
      parent: _progressController,
      curve: Curves.easeInOut,
    ));
    
    // Start animation after a short delay
    Future.delayed(Duration(milliseconds: 200), () {
      if (mounted) {
        _progressController.forward();
      }
    });
  }

  @override
  void dispose() {
    _progressController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: widget.onTap,
      child: Container(
        decoration: BoxDecoration(
          color: AppColors.cardBackground,
          borderRadius: BorderRadius.circular(16.r),
          border: Border.all(
            color: widget.goal.category.color.withOpacity(0.3),
          ),
          boxShadow: [
            BoxShadow(
              color: AppColors.black.withOpacity(0.1),
              blurRadius: 8.r,
              offset: Offset(0, 4.h),
            ),
          ],
        ),
        child: Column(
          children: [
            // Main content
            Padding(
              padding: EdgeInsets.all(16.w),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  // Header row
                  Row(
                    children: [
                      // Category icon
                      Container(
                        width: 40.w,
                        height: 40.h,
                        decoration: BoxDecoration(
                          color: widget.goal.category.color.withOpacity(0.1),
                          borderRadius: BorderRadius.circular(10.r),
                        ),
                        child: Icon(
                          widget.goal.category.icon,
                          color: widget.goal.category.color,
                          size: 20.sp,
                        ),
                      ),
                      SizedBox(width: 12.w),
                      
                      // Title and status
                      Expanded(
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            Text(
                              widget.goal.title,
                              style: TextStyle(
                                fontSize: 16.sp,
                                fontWeight: FontWeight.bold,
                                color: AppColors.textLight,
                              ),
                              maxLines: 2,
                              overflow: TextOverflow.ellipsis,
                            ),
                            SizedBox(height: 2.h),
                            Row(
                              children: [
                                Icon(
                                  widget.goal.status.icon,
                                  color: widget.goal.status.color,
                                  size: 12.sp,
                                ),
                                SizedBox(width: 4.w),
                                Text(
                                  widget.goal.status.displayName,
                                  style: TextStyle(
                                    fontSize: 12.sp,
                                    color: widget.goal.status.color,
                                    fontWeight: FontWeight.w500,
                                  ),
                                ),
                              ],
                            ),
                          ],
                        ),
                      ),
                      
                      // Priority badge
                      Container(
                        padding: EdgeInsets.symmetric(horizontal: 8.w, vertical: 4.h),
                        decoration: BoxDecoration(
                          color: widget.goal.priority.color.withOpacity(0.1),
                          borderRadius: BorderRadius.circular(12.r),
                        ),
                        child: Row(
                          mainAxisSize: MainAxisSize.min,
                          children: [
                            Icon(
                              widget.goal.priority.icon,
                              color: widget.goal.priority.color,
                              size: 12.sp,
                            ),
                            SizedBox(width: 4.w),
                            Text(
                              widget.goal.priority.displayName,
                              style: TextStyle(
                                fontSize: 10.sp,
                                color: widget.goal.priority.color,
                                fontWeight: FontWeight.w600,
                              ),
                            ),
                          ],
                        ),
                      ),
                    ],
                  ),
                  
                  SizedBox(height: 16.h),
                  
                  // Progress section
                  Row(
                    children: [
                      Expanded(
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            Row(
                              mainAxisAlignment: MainAxisAlignment.spaceBetween,
                              children: [
                                Text(
                                  'Progress',
                                  style: TextStyle(
                                    fontSize: 14.sp,
                                    color: AppColors.textMedium,
                                    fontWeight: FontWeight.w500,
                                  ),
                                ),
                                Text(
                                  '${widget.goal.progressPercentage.round()}%',
                                  style: TextStyle(
                                    fontSize: 14.sp,
                                    color: widget.goal.category.color,
                                    fontWeight: FontWeight.bold,
                                  ),
                                ),
                              ],
                            ),
                            SizedBox(height: 8.h),
                            
                            // Animated progress bar
                            AnimatedBuilder(
                              animation: _progressAnimation,
                              builder: (context, child) {
                                return Container(
                                  height: 6.h,
                                  decoration: BoxDecoration(
                                    color: AppColors.textMedium.withOpacity(0.2),
                                    borderRadius: BorderRadius.circular(3.r),
                                  ),
                                  child: FractionallySizedBox(
                                    alignment: Alignment.centerLeft,
                                    widthFactor: _progressAnimation.value,
                                    child: Container(
                                      decoration: BoxDecoration(
                                        gradient: LinearGradient(
                                          colors: [
                                            widget.goal.category.color,
                                            widget.goal.category.color.withOpacity(0.7),
                                          ],
                                        ),
                                        borderRadius: BorderRadius.circular(3.r),
                                      ),
                                    ),
                                  ),
                                );
                              },
                            ),
                          ],
                        ),
                      ),
                      SizedBox(width: 16.w),
                      
                      // Quick action button
                      GestureDetector(
                        onTap: widget.onProgressUpdate,
                        child: Container(
                          width: 36.w,
                          height: 36.h,
                          decoration: BoxDecoration(
                            color: widget.goal.category.color.withOpacity(0.1),
                            borderRadius: BorderRadius.circular(8.r),
                          ),
                          child: Icon(
                            Icons.add,
                            color: widget.goal.category.color,
                            size: 20.sp,
                          ),
                        ),
                      ),
                    ],
                  ),
                  
                  SizedBox(height: 16.h),
                  
                  // Stats row
                  Row(
                    children: [
                      _buildStatItem(
                        icon: Icons.check_circle_outline,
                        value: '${widget.goal.completedMilestones}/${widget.goal.milestones.length}',
                        label: 'Milestones',
                      ),
                      SizedBox(width: 24.w),
                      _buildStatItem(
                        icon: Icons.calendar_today,
                        value: widget.goal.daysUntilTarget > 0 
                            ? '${widget.goal.daysUntilTarget}d'
                            : 'Overdue',
                        label: widget.goal.daysUntilTarget > 0 ? 'Days left' : '',
                        isOverdue: widget.goal.isOverdue,
                      ),
                      Spacer(),
                      
                      // Expand/collapse button
                      GestureDetector(
                        onTap: () {
                          setState(() {
                            isExpanded = !isExpanded;
                          });
                        },
                        child: Icon(
                          isExpanded ? Icons.expand_less : Icons.expand_more,
                          color: AppColors.textMedium,
                          size: 20.sp,
                        ),
                      ),
                    ],
                  ),
                ],
              ),
            ),
            
            // Expanded section
            if (isExpanded) _buildExpandedSection(),
          ],
        ),
      ),
    );
  }

  Widget _buildStatItem({
    required IconData icon,
    required String value,
    required String label,
    bool isOverdue = false,
  }) {
    return Row(
      mainAxisSize: MainAxisSize.min,
      children: [
        Icon(
          icon,
          color: isOverdue ? Colors.red : AppColors.textMedium,
          size: 14.sp,
        ),
        SizedBox(width: 4.w),
        Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              value,
              style: TextStyle(
                fontSize: 12.sp,
                fontWeight: FontWeight.w600,
                color: isOverdue ? Colors.red : AppColors.textLight,
              ),
            ),
            if (label.isNotEmpty)
              Text(
                label,
                style: TextStyle(
                  fontSize: 10.sp,
                  color: AppColors.textMedium,
                ),
              ),
          ],
        ),
      ],
    );
  }

  Widget _buildExpandedSection() {
    return Container(
      decoration: BoxDecoration(
        color: AppColors.backgroundMedium,
        borderRadius: BorderRadius.vertical(bottom: Radius.circular(16.r)),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          // Description
          if (widget.goal.description.isNotEmpty)
            Padding(
              padding: EdgeInsets.all(16.w),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    'Description',
                    style: TextStyle(
                      fontSize: 14.sp,
                      fontWeight: FontWeight.w600,
                      color: AppColors.textLight,
                    ),
                  ),
                  SizedBox(height: 8.h),
                  Text(
                    widget.goal.description,
                    style: TextStyle(
                      fontSize: 14.sp,
                      color: AppColors.textMedium,
                      height: 1.4,
                    ),
                  ),
                ],
              ),
            ),
          
          // Milestones
          if (widget.goal.milestones.isNotEmpty)
            Padding(
              padding: EdgeInsets.all(16.w),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    'Milestones',
                    style: TextStyle(
                      fontSize: 14.sp,
                      fontWeight: FontWeight.w600,
                      color: AppColors.textLight,
                    ),
                  ),
                  SizedBox(height: 12.h),
                  ...widget.goal.milestones.take(3).map((milestone) {
                    return Container(
                      margin: EdgeInsets.only(bottom: 8.h),
                      child: Row(
                        children: [
                          Icon(
                            milestone.isCompleted 
                                ? Icons.check_circle
                                : Icons.radio_button_unchecked,
                            color: milestone.isCompleted 
                                ? AppColors.accentMint
                                : AppColors.textMedium,
                            size: 16.sp,
                          ),
                          SizedBox(width: 8.w),
                          Expanded(
                            child: Text(
                              milestone.title,
                              style: TextStyle(
                                fontSize: 13.sp,
                                color: milestone.isCompleted 
                                    ? AppColors.textMedium
                                    : AppColors.textLight,
                                decoration: milestone.isCompleted 
                                    ? TextDecoration.lineThrough
                                    : TextDecoration.none,
                              ),
                            ),
                          ),
                        ],
                      ),
                    );
                  }).toList(),
                  
                  if (widget.goal.milestones.length > 3)
                    Text(
                      '+${widget.goal.milestones.length - 3} more milestones',
                      style: TextStyle(
                        fontSize: 12.sp,
                        color: AppColors.textMedium,
                        fontStyle: FontStyle.italic,
                      ),
                    ),
                ],
              ),
            ),
          
          // Action buttons
          Container(
            padding: EdgeInsets.all(16.w),
            child: Row(
              children: [
                Expanded(
                  child: GestureDetector(
                    onTap: widget.onEdit,
                    child: Container(
                      height: 40.h,
                      decoration: BoxDecoration(
                        color: AppColors.cardBackground,
                        borderRadius: BorderRadius.circular(8.r),
                        border: Border.all(
                          color: AppColors.textMedium.withOpacity(0.3),
                        ),
                      ),
                      child: Center(
                        child: Row(
                          mainAxisAlignment: MainAxisAlignment.center,
                          children: [
                            Icon(
                              Icons.edit,
                              color: AppColors.textMedium,
                              size: 16.sp,
                            ),
                            SizedBox(width: 8.w),
                            Text(
                              'Edit',
                              style: TextStyle(
                                fontSize: 14.sp,
                                color: AppColors.textMedium,
                                fontWeight: FontWeight.w500,
                              ),
                            ),
                          ],
                        ),
                      ),
                    ),
                  ),
                ),
                SizedBox(width: 12.w),
                GestureDetector(
                  onTap: widget.onDelete,
                  child: Container(
                    width: 40.w,
                    height: 40.h,
                    decoration: BoxDecoration(
                      color: Colors.red.withOpacity(0.1),
                      borderRadius: BorderRadius.circular(8.r),
                    ),
                    child: Icon(
                      Icons.delete_outline,
                      color: Colors.red,
                      size: 16.sp,
                    ),
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }
}