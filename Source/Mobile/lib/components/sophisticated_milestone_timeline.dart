import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:intl/intl.dart';
import '../models/goals.dart';
import '../constants/app_theme.dart';

class SophisticatedMilestoneTimeline extends StatelessWidget {
  final List<Milestone> milestones;
  final Color accentColor;

  const SophisticatedMilestoneTimeline({
    super.key,
    required this.milestones,
    required this.accentColor,
  });

  @override
  Widget build(BuildContext context) {
    // Sort milestones by created date for timeline order
    final sortedMilestones = List<Milestone>.from(milestones)
      ..sort((a, b) => a.createdAt.compareTo(b.createdAt));

    return Container(
      padding: EdgeInsets.all(20.w),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(
            'Milestone Timeline',
            style: TextStyle(
              fontSize: 18.sp,
              fontWeight: FontWeight.bold,
              color: AppTheme.textPrimary,
            ),
          ),
          SizedBox(height: 16.h),
          
          // Timeline
          ...sortedMilestones.asMap().entries.map((entry) {
            final index = entry.key;
            final milestone = entry.value;
            final isLast = index == sortedMilestones.length - 1;
            
            return _buildTimelineItem(milestone, isLast);
          }),
        ],
      ),
    );
  }

  Widget _buildTimelineItem(Milestone milestone, bool isLast) {
    final now = DateTime.now();
    final isOverdue = milestone.targetDate != null && 
                     !milestone.isCompleted && 
                     now.isAfter(milestone.targetDate!);
    final isCompleted = milestone.isCompleted;
    
    return Row(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        // Timeline indicator column
        Column(
          children: [
            // Timeline node
            Container(
              width: 20.w,
              height: 20.h,
              decoration: BoxDecoration(
                shape: BoxShape.circle,
                color: _getNodeColor(milestone),
                border: Border.all(
                  color: _getNodeBorderColor(milestone),
                  width: 2,
                ),
              ),
              child: isCompleted
                  ? Icon(
                      Icons.check,
                      size: 12.sp,
                      color: AppTheme.white,
                    )
                  : isOverdue
                      ? Icon(
                          Icons.warning,
                          size: 12.sp,
                          color: AppTheme.white,
                        )
                      : null,
            ),
            
            // Timeline line
            if (!isLast)
              Container(
                width: 2.w,
                height: 120.h,
                color: accentColor.withValues(alpha: 0.3),
              ),
          ],
        ),
        
        SizedBox(width: 16.w),
        
        // Milestone content
        Expanded(
          child: Container(
            margin: EdgeInsets.only(bottom: 24.h),
            padding: EdgeInsets.all(16.w),
            decoration: BoxDecoration(
              color: AppTheme.cardBackground,
              borderRadius: BorderRadius.circular(12.r),
              border: Border.all(
                color: _getNodeColor(milestone).withValues(alpha: 0.2),
              ),
            ),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                // Milestone title
                Row(
                  children: [
                    Expanded(
                      child: Text(
                        milestone.title,
                        style: TextStyle(
                          fontSize: 16.sp,
                          fontWeight: FontWeight.w600,
                          color: AppTheme.textPrimary,
                          decoration: isCompleted 
                              ? TextDecoration.lineThrough 
                              : null,
                        ),
                      ),
                    ),
                    _buildStatusChip(milestone),
                  ],
                ),
                
                if (milestone.description?.isNotEmpty == true) ...[
                  SizedBox(height: 8.h),
                  Text(
                    milestone.description!,
                    style: TextStyle(
                      fontSize: 14.sp,
                      color: AppTheme.textSecondary,
                    ),
                  ),
                ],
                
                SizedBox(height: 12.h),
                
                // Time information
                _buildTimeInfo(milestone),
              ],
            ),
          ),
        ),
      ],
    );
  }

  Widget _buildTimeInfo(Milestone milestone) {
    return Column(
      children: [
        // Created time
        _buildTimeRow(
          icon: Icons.add_circle_outline,
          label: 'Created',
          date: milestone.createdAt,
          color: AppTheme.textSecondary,
        ),
        
        SizedBox(height: 8.h),
        
        // Expected completion
        if (milestone.targetDate != null)
          _buildTimeRow(
            icon: Icons.schedule,
            label: 'Expected',
            date: milestone.targetDate!,
            color: _getExpectedDateColor(milestone),
          ),
        
        if (milestone.targetDate != null) SizedBox(height: 8.h),
        
        // Actual completion
        if (milestone.completedAt != null)
          _buildTimeRow(
            icon: Icons.check_circle,
            label: 'Completed',
            date: milestone.completedAt!,
            color: AppTheme.accentMint,
          ),
        
        // Performance indicator
        if (milestone.isCompleted && 
            milestone.targetDate != null && 
            milestone.completedAt != null)
          ...[
            SizedBox(height: 8.h),
            _buildPerformanceIndicator(milestone),
          ],
      ],
    );
  }

  Widget _buildTimeRow({
    required IconData icon,
    required String label,
    required DateTime date,
    required Color color,
  }) {
    final formatter = DateFormat('MMM dd, yyyy');
    final timeFormatter = DateFormat('h:mm a');
    
    return Row(
      children: [
        Icon(icon, size: 16.sp, color: color),
        SizedBox(width: 8.w),
        Text(
          '$label:',
          style: TextStyle(
            fontSize: 12.sp,
            fontWeight: FontWeight.w500,
            color: color,
          ),
        ),
        SizedBox(width: 8.w),
        Text(
          formatter.format(date),
          style: TextStyle(
            fontSize: 12.sp,
            color: AppTheme.textPrimary,
            fontWeight: FontWeight.w500,
          ),
        ),
        SizedBox(width: 4.w),
        Text(
          timeFormatter.format(date),
          style: TextStyle(
            fontSize: 11.sp,
            color: AppTheme.textSecondary,
          ),
        ),
      ],
    );
  }

  Widget _buildStatusChip(Milestone milestone) {
    final now = DateTime.now();
    final isOverdue = milestone.targetDate != null && 
                     !milestone.isCompleted && 
                     now.isAfter(milestone.targetDate!);
    
    Color chipColor;
    String chipText;
    IconData chipIcon;
    
    if (milestone.isCompleted) {
      chipColor = AppTheme.accentMint;
      chipText = 'Completed';
      chipIcon = Icons.check_circle;
    } else if (isOverdue) {
      chipColor = Colors.red;
      chipText = 'Overdue';
      chipIcon = Icons.warning;
    } else {
      chipColor = accentColor;
      chipText = 'In Progress';
      chipIcon = Icons.schedule;
    }
    
    return Container(
      padding: EdgeInsets.symmetric(horizontal: 8.w, vertical: 4.h),
      decoration: BoxDecoration(
        color: chipColor.withValues(alpha: 0.1),
        borderRadius: BorderRadius.circular(12.r),
        border: Border.all(
          color: chipColor.withValues(alpha: 0.3),
        ),
      ),
      child: Row(
        mainAxisSize: MainAxisSize.min,
        children: [
          Icon(chipIcon, size: 12.sp, color: chipColor),
          SizedBox(width: 4.w),
          Text(
            chipText,
            style: TextStyle(
              fontSize: 11.sp,
              fontWeight: FontWeight.w600,
              color: chipColor,
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildPerformanceIndicator(Milestone milestone) {
    final expectedDate = milestone.targetDate!;
    final actualDate = milestone.completedAt!;
    final daysDifference = actualDate.difference(expectedDate).inDays;
    
    Color performanceColor;
    String performanceText;
    IconData performanceIcon;
    
    if (daysDifference <= 0) {
      // Completed on time or early
      performanceColor = AppTheme.accentMint;
      performanceText = daysDifference == 0 
          ? 'Completed on time!' 
          : 'Completed ${(-daysDifference)} day${daysDifference < -1 ? 's' : ''} early!';
      performanceIcon = Icons.trending_up;
    } else {
      // Completed late
      performanceColor = Colors.orange;
      performanceText = 'Completed $daysDifference day${daysDifference > 1 ? 's' : ''} late';
      performanceIcon = Icons.trending_down;
    }
    
    return Container(
      padding: EdgeInsets.all(8.w),
      decoration: BoxDecoration(
        color: performanceColor.withValues(alpha: 0.1),
        borderRadius: BorderRadius.circular(8.r),
        border: Border.all(
          color: performanceColor.withValues(alpha: 0.3),
        ),
      ),
      child: Row(
        mainAxisSize: MainAxisSize.min,
        children: [
          Icon(performanceIcon, size: 14.sp, color: performanceColor),
          SizedBox(width: 8.w),
          Text(
            performanceText,
            style: TextStyle(
              fontSize: 12.sp,
              fontWeight: FontWeight.w600,
              color: performanceColor,
            ),
          ),
        ],
      ),
    );
  }

  Color _getNodeColor(Milestone milestone) {
    final now = DateTime.now();
    final isOverdue = milestone.targetDate != null && 
                     !milestone.isCompleted && 
                     now.isAfter(milestone.targetDate!);
    
    if (milestone.isCompleted) {
      return AppTheme.accentMint;
    } else if (isOverdue) {
      return Colors.red;
    } else {
      return accentColor;
    }
  }

  Color _getNodeBorderColor(Milestone milestone) {
    return _getNodeColor(milestone).withValues(alpha: 0.3);
  }

  Color _getExpectedDateColor(Milestone milestone) {
    final now = DateTime.now();
    if (milestone.targetDate != null && !milestone.isCompleted) {
      final daysUntilTarget = milestone.targetDate!.difference(now).inDays;
      if (daysUntilTarget < 0) {
        return Colors.red; // Overdue
      } else if (daysUntilTarget <= 7) {
        return Colors.orange; // Due soon
      }
    }
    return AppTheme.textSecondary;
  }
}