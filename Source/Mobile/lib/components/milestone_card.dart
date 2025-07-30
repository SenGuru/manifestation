import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../models/api_milestone.dart';
import '../constants/app_theme.dart';

class MilestoneCard extends StatelessWidget {
  final ApiMilestone milestone;
  final VoidCallback? onTap;
  final VoidCallback? onEdit;
  final VoidCallback? onDelete;
  final ValueChanged<bool>? onCompletionChanged;
  final ValueChanged<double>? onProgressChanged;
  final bool showActions;
  final bool compact;

  const MilestoneCard({
    Key? key,
    required this.milestone,
    this.onTap,
    this.onEdit,
    this.onDelete,
    this.onCompletionChanged,
    this.onProgressChanged,
    this.showActions = true,
    this.compact = false,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Container(
      margin: EdgeInsets.only(bottom: compact ? 8.h : 12.h),
      decoration: BoxDecoration(
        color: AppTheme.cardBackground,
        borderRadius: BorderRadius.circular(compact ? 12.r : 16.r),
        border: Border.all(
          color: milestone.isCompleted 
              ? AppTheme.accentMint.withOpacity(0.3)
              : milestone.isOverdue 
                  ? Colors.red.withOpacity(0.3)
                  : AppTheme.textMedium.withOpacity(0.1),
        ),
        boxShadow: [
          BoxShadow(
            color: AppTheme.backgroundDark.withOpacity(0.1),
            blurRadius: compact ? 4.r : 8.r,
            offset: Offset(0, compact ? 2.h : 4.h),
          ),
        ],
      ),
      child: Material(
        color: Colors.transparent,
        child: InkWell(
          onTap: onTap,
          borderRadius: BorderRadius.circular(compact ? 12.r : 16.r),
          child: Padding(
            padding: EdgeInsets.all(compact ? 12.w : 16.w),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                // Header row
                Row(
                  children: [
                    // Completion checkbox
                    GestureDetector(
                      onTap: () => onCompletionChanged?.call(!milestone.isCompleted),
                      child: Container(
                        width: compact ? 20.w : 24.w,
                        height: compact ? 20.h : 24.h,
                        decoration: BoxDecoration(
                          color: milestone.isCompleted 
                              ? AppTheme.accentMint
                              : Colors.transparent,
                          border: Border.all(
                            color: milestone.isCompleted 
                                ? AppTheme.accentMint
                                : AppTheme.textMedium.withOpacity(0.3),
                            width: 2,
                          ),
                          borderRadius: BorderRadius.circular(4.r),
                        ),
                        child: milestone.isCompleted
                            ? Icon(
                                Icons.check,
                                color: AppTheme.white,
                                size: compact ? 14.sp : 16.sp,
                              )
                            : null,
                      ),
                    ),
                    
                    SizedBox(width: 12.w),
                    
                    // Title and status
                    Expanded(
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          Text(
                            milestone.title,
                            style: TextStyle(
                              fontSize: compact ? 14.sp : 16.sp,
                              fontWeight: FontWeight.w600,
                              color: milestone.isCompleted 
                                  ? AppTheme.textMedium
                                  : AppTheme.textLight,
                              decoration: milestone.isCompleted 
                                  ? TextDecoration.lineThrough
                                  : null,
                            ),
                            maxLines: compact ? 1 : 2,
                            overflow: TextOverflow.ellipsis,
                          ),
                          
                          if (!compact) ...[
                            SizedBox(height: 4.h),
                            Row(
                              children: [
                                Icon(
                                  milestone.statusIcon,
                                  color: milestone.statusColor,
                                  size: 14.sp,
                                ),
                                SizedBox(width: 4.w),
                                Text(
                                  milestone.statusText,
                                  style: TextStyle(
                                    fontSize: 12.sp,
                                    color: milestone.statusColor,
                                    fontWeight: FontWeight.w500,
                                  ),
                                ),
                              ],
                            ),
                          ],
                        ],
                      ),
                    ),
                    
                    // Actions menu
                    if (showActions)
                      PopupMenuButton<String>(
                        icon: Icon(
                          Icons.more_vert,
                          color: AppTheme.textMedium,
                          size: compact ? 18.sp : 20.sp,
                        ),
                        color: AppTheme.cardBackground,
                        onSelected: (value) {
                          switch (value) {
                            case 'edit':
                              onEdit?.call();
                              break;
                            case 'delete':
                              onDelete?.call();
                              break;
                          }
                        },
                        itemBuilder: (context) => [
                          PopupMenuItem(
                            value: 'edit',
                            child: Row(
                              children: [
                                Icon(Icons.edit, color: AppTheme.textLight, size: 16.sp),
                                SizedBox(width: 8.w),
                                Text(
                                  'Edit',
                                  style: TextStyle(color: AppTheme.textLight),
                                ),
                              ],
                            ),
                          ),
                          PopupMenuItem(
                            value: 'delete',
                            child: Row(
                              children: [
                                Icon(Icons.delete, color: Colors.red, size: 16.sp),
                                SizedBox(width: 8.w),
                                Text(
                                  'Delete',
                                  style: TextStyle(color: Colors.red),
                                ),
                              ],
                            ),
                          ),
                        ],
                      ),
                  ],
                ),
                
                // Description
                if (!compact && milestone.description.isNotEmpty) ...[
                  SizedBox(height: 8.h),
                  Text(
                    milestone.description,
                    style: TextStyle(
                      fontSize: 13.sp,
                      color: AppTheme.textMedium,
                      height: 1.4,
                    ),
                    maxLines: 2,
                    overflow: TextOverflow.ellipsis,
                  ),
                ],
                
                // Progress bar (if not completed and has progress)
                if (!compact && !milestone.isCompleted && milestone.progressPercentage > 0) ...[
                  SizedBox(height: 12.h),
                  Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Row(
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        children: [
                          Text(
                            'Progress',
                            style: TextStyle(
                              fontSize: 12.sp,
                              color: AppTheme.textMedium,
                              fontWeight: FontWeight.w500,
                            ),
                          ),
                          Text(
                            '${milestone.progressPercentage.round()}%',
                            style: TextStyle(
                              fontSize: 12.sp,
                              color: AppTheme.textLight,
                              fontWeight: FontWeight.w600,
                            ),
                          ),
                        ],
                      ),
                      SizedBox(height: 6.h),
                      Container(
                        height: 6.h,
                        decoration: BoxDecoration(
                          color: AppTheme.textMedium.withOpacity(0.1),
                          borderRadius: BorderRadius.circular(3.r),
                        ),
                        child: FractionallySizedBox(
                          alignment: Alignment.centerLeft,
                          widthFactor: milestone.progressPercentage / 100,
                          child: Container(
                            decoration: BoxDecoration(
                              color: AppTheme.goalTrackerGradient[0],
                              borderRadius: BorderRadius.circular(3.r),
                            ),
                          ),
                        ),
                      ),
                    ],
                  ),
                ],
                
                // Target date
                if (!compact && milestone.targetDate != null) ...[
                  SizedBox(height: 8.h),
                  Row(
                    children: [
                      Icon(
                        milestone.isOverdue ? Icons.warning : Icons.schedule,
                        color: milestone.isOverdue ? Colors.red : AppTheme.textMedium,
                        size: 14.sp,
                      ),
                      SizedBox(width: 4.w),
                      Text(
                        milestone.isOverdue ? 'Overdue' : 'Due',
                        style: TextStyle(
                          fontSize: 12.sp,
                          color: milestone.isOverdue ? Colors.red : AppTheme.textMedium,
                          fontWeight: FontWeight.w500,
                        ),
                      ),
                      SizedBox(width: 4.w),
                      Text(
                        _formatDate(milestone.targetDate!),
                        style: TextStyle(
                          fontSize: 12.sp,
                          color: milestone.isOverdue ? Colors.red : AppTheme.textLight,
                          fontWeight: FontWeight.w500,
                        ),
                      ),
                    ],
                  ),
                ],
              ],
            ),
          ),
        ),
      ),
    );
  }

  String _formatDate(DateTime date) {
    final now = DateTime.now();
    final difference = date.difference(now).inDays;
    
    if (difference == 0) {
      return 'Today';
    } else if (difference == 1) {
      return 'Tomorrow';
    } else if (difference == -1) {
      return 'Yesterday';
    } else if (difference > 0) {
      return 'in $difference days';
    } else {
      return '${-difference} days ago';
    }
  }
}

// Compact milestone list item for use in lists
class MilestoneListItem extends StatelessWidget {
  final ApiMilestone milestone;
  final VoidCallback? onTap;
  final ValueChanged<bool>? onCompletionChanged;

  const MilestoneListItem({
    Key? key,
    required this.milestone,
    this.onTap,
    this.onCompletionChanged,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return ListTile(
      onTap: onTap,
      leading: GestureDetector(
        onTap: () => onCompletionChanged?.call(!milestone.isCompleted),
        child: Icon(
          milestone.statusIcon,
          color: milestone.statusColor,
          size: 20.sp,
        ),
      ),
      title: Text(
        milestone.title,
        style: TextStyle(
          fontSize: 14.sp,
          fontWeight: FontWeight.w500,
          color: milestone.isCompleted 
              ? AppTheme.textMedium
              : AppTheme.textLight,
          decoration: milestone.isCompleted 
              ? TextDecoration.lineThrough
              : null,
        ),
        maxLines: 1,
        overflow: TextOverflow.ellipsis,
      ),
      subtitle: milestone.targetDate != null
          ? Text(
              _formatDate(milestone.targetDate!),
              style: TextStyle(
                fontSize: 12.sp,
                color: milestone.isOverdue ? Colors.red : AppTheme.textMedium,
              ),
            )
          : null,
      trailing: milestone.progressPercentage > 0 && !milestone.isCompleted
          ? Container(
              padding: EdgeInsets.symmetric(horizontal: 8.w, vertical: 4.h),
              decoration: BoxDecoration(
                color: AppTheme.goalTrackerGradient[0].withOpacity(0.1),
                borderRadius: BorderRadius.circular(12.r),
              ),
              child: Text(
                '${milestone.progressPercentage.round()}%',
                style: TextStyle(
                  fontSize: 11.sp,
                  color: AppTheme.goalTrackerGradient[0],
                  fontWeight: FontWeight.w600,
                ),
              ),
            )
          : null,
    );
  }

  String _formatDate(DateTime date) {
    final now = DateTime.now();
    final difference = date.difference(now).inDays;
    
    if (difference == 0) {
      return 'Today';
    } else if (difference == 1) {
      return 'Tomorrow';
    } else if (difference == -1) {
      return 'Yesterday';
    } else if (difference > 0) {
      return 'in $difference days';
    } else {
      return '${-difference} days ago';
    }
  }
}