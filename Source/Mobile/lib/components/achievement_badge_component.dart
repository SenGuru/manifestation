import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../constants/app_theme.dart';
import 'package:mistel/models/social_models.dart';

class AchievementBadge extends StatefulWidget {
  final Achievement achievement;
  final VoidCallback onTap;
  final bool showDetails;

  const AchievementBadge({
    Key? key,
    required this.achievement,
    required this.onTap,
    this.showDetails = true,
  }) : super(key: key);

  @override
  _AchievementBadgeState createState() => _AchievementBadgeState();
}

class _AchievementBadgeState extends State<AchievementBadge>
    with SingleTickerProviderStateMixin {
  late AnimationController _shimmerController;
  late Animation<double> _shimmerAnimation;

  @override
  void initState() {
    super.initState();
    _shimmerController = AnimationController(
      duration: Duration(seconds: 2),
      vsync: this,
    );
    _shimmerAnimation = Tween<double>(
      begin: -1.0,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _shimmerController,
      curve: Curves.easeInOut,
    ));
    
    // Start shimmer animation
    _shimmerController.repeat();
  }

  @override
  void dispose() {
    _shimmerController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: widget.onTap,
      child: Container(
        padding: EdgeInsets.all(16.w),
        decoration: BoxDecoration(
          gradient: LinearGradient(
            colors: [
              widget.achievement.color.withOpacity(0.1),
              widget.achievement.color.withOpacity(0.05),
            ],
          ),
          borderRadius: BorderRadius.circular(16.r),
          border: Border.all(
            color: widget.achievement.color.withOpacity(0.3),
          ),
          boxShadow: [
            BoxShadow(
              color: widget.achievement.color.withOpacity(0.2),
              blurRadius: 8.r,
              offset: Offset(0, 4.h),
            ),
          ],
        ),
        child: Stack(
          children: [
            // Shimmer effect
            AnimatedBuilder(
              animation: _shimmerAnimation,
              builder: (context, child) {
                return Positioned.fill(
                  child: Container(
                    decoration: BoxDecoration(
                      borderRadius: BorderRadius.circular(16.r),
                      gradient: LinearGradient(
                        colors: [
                          Colors.transparent,
                          AppTheme.white.withOpacity(0.1),
                          Colors.transparent,
                        ],
                        stops: [0.0, 0.5, 1.0],
                        transform: GradientRotation(_shimmerAnimation.value * 3.14159),
                      ),
                    ),
                  ),
                );
              },
            ),
            
            // Main content
            Row(
              children: [
                // Achievement icon with glow
                Stack(
                  alignment: Alignment.center,
                  children: [
                    // Glow effect
                    Container(
                      width: 60.w,
                      height: 60.h,
                      decoration: BoxDecoration(
                        color: widget.achievement.color.withOpacity(0.3),
                        shape: BoxShape.circle,
                        boxShadow: [
                          BoxShadow(
                            color: widget.achievement.color.withOpacity(0.4),
                            blurRadius: 20.r,
                            spreadRadius: 2.r,
                          ),
                        ],
                      ),
                    ),
                    // Icon container
                    Container(
                      width: 50.w,
                      height: 50.h,
                      decoration: BoxDecoration(
                        gradient: LinearGradient(
                          colors: [
                            widget.achievement.color,
                            widget.achievement.color.withOpacity(0.8),
                          ],
                          begin: Alignment.topLeft,
                          end: Alignment.bottomRight,
                        ),
                        shape: BoxShape.circle,
                      ),
                      child: Icon(
                        widget.achievement.icon,
                        color: AppTheme.white,
                        size: 24.sp,
                      ),
                    ),
                    // Trophy overlay
                    Positioned(
                      top: -2.h,
                      right: -2.w,
                      child: Container(
                        width: 20.w,
                        height: 20.h,
                        decoration: BoxDecoration(
                          color: AppTheme.accentGold,
                          shape: BoxShape.circle,
                          boxShadow: [
                            BoxShadow(
                              color: AppTheme.accentGold.withOpacity(0.5),
                              blurRadius: 4.r,
                            ),
                          ],
                        ),
                        child: Center(
                          child: Text(
                            '🏆',
                            style: TextStyle(fontSize: 10.sp),
                          ),
                        ),
                      ),
                    ),
                  ],
                ),
                
                SizedBox(width: 16.w),
                
                // Achievement details
                if (widget.showDetails)
                  Expanded(
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Row(
                          children: [
                            Expanded(
                              child: Text(
                                widget.achievement.title,
                                style: TextStyle(
                                  fontSize: 16.sp,
                                  fontWeight: FontWeight.bold,
                                  color: AppTheme.textLight,
                                ),
                              ),
                            ),
                            Container(
                              padding: EdgeInsets.symmetric(
                                horizontal: 8.w,
                                vertical: 4.h,
                              ),
                              decoration: BoxDecoration(
                                color: widget.achievement.color.withOpacity(0.1),
                                borderRadius: BorderRadius.circular(12.r),
                              ),
                              child: Text(
                                widget.achievement.category.displayName,
                                style: TextStyle(
                                  fontSize: 10.sp,
                                  fontWeight: FontWeight.w600,
                                  color: widget.achievement.color,
                                ),
                              ),
                            ),
                          ],
                        ),
                        SizedBox(height: 4.h),
                        Text(
                          widget.achievement.description,
                          style: TextStyle(
                            fontSize: 13.sp,
                            color: AppTheme.textMedium,
                            height: 1.3,
                          ),
                          maxLines: 2,
                          overflow: TextOverflow.ellipsis,
                        ),
                        SizedBox(height: 8.h),
                        Row(
                          children: [
                            Icon(
                              Icons.access_time,
                              color: AppTheme.textMedium,
                              size: 12.sp,
                            ),
                            SizedBox(width: 4.w),
                            Text(
                              _formatDate(widget.achievement.unlockedAt),
                              style: TextStyle(
                                fontSize: 11.sp,
                                color: AppTheme.textMedium,
                              ),
                            ),
                            Spacer(),
                            if (widget.achievement.shareMessage != null)
                              GestureDetector(
                                onTap: () => _shareAchievement(),
                                child: Container(
                                  padding: EdgeInsets.symmetric(
                                    horizontal: 8.w,
                                    vertical: 4.h,
                                  ),
                                  decoration: BoxDecoration(
                                    color: AppTheme.socialGradient[0].withOpacity(0.1),
                                    borderRadius: BorderRadius.circular(8.r),
                                  ),
                                  child: Row(
                                    mainAxisSize: MainAxisSize.min,
                                    children: [
                                      Icon(
                                        Icons.share,
                                        color: AppTheme.socialGradient[0],
                                        size: 12.sp,
                                      ),
                                      SizedBox(width: 4.w),
                                      Text(
                                        'Share',
                                        style: TextStyle(
                                          fontSize: 10.sp,
                                          fontWeight: FontWeight.w600,
                                          color: AppTheme.socialGradient[0],
                                        ),
                                      ),
                                    ],
                                  ),
                                ),
                              ),
                          ],
                        ),
                      ],
                    ),
                  ),
              ],
            ),
          ],
        ),
      ),
    );
  }

  String _formatDate(DateTime dateTime) {
    final now = DateTime.now();
    final difference = now.difference(dateTime);
    
    if (difference.inDays == 0) {
      return 'Today';
    } else if (difference.inDays == 1) {
      return 'Yesterday';
    } else if (difference.inDays < 7) {
      return '${difference.inDays} days ago';
    } else if (difference.inDays < 30) {
      final weeks = (difference.inDays / 7).floor();
      return '${weeks} week${weeks > 1 ? 's' : ''} ago';
    } else {
      final months = (difference.inDays / 30).floor();
      return '${months} month${months > 1 ? 's' : ''} ago';
    }
  }

  void _shareAchievement() {
    // TODO: Implement share functionality
    print('Sharing achievement: ${widget.achievement.title}');
    print('Share message: ${widget.achievement.shareMessage}');
  }
}

// Compact version for smaller spaces
class CompactAchievementBadge extends StatelessWidget {
  final Achievement achievement;
  final VoidCallback? onTap;

  const CompactAchievementBadge({
    Key? key,
    required this.achievement,
    this.onTap,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: onTap,
      child: Container(
        padding: EdgeInsets.symmetric(horizontal: 12.w, vertical: 8.h),
        decoration: BoxDecoration(
          gradient: LinearGradient(
            colors: [
              achievement.color.withOpacity(0.1),
              achievement.color.withOpacity(0.05),
            ],
          ),
          borderRadius: BorderRadius.circular(12.r),
          border: Border.all(
            color: achievement.color.withOpacity(0.3),
          ),
        ),
        child: Row(
          mainAxisSize: MainAxisSize.min,
          children: [
            Container(
              width: 24.w,
              height: 24.h,
              decoration: BoxDecoration(
                color: achievement.color,
                shape: BoxShape.circle,
              ),
              child: Icon(
                achievement.icon,
                color: AppTheme.white,
                size: 12.sp,
              ),
            ),
            SizedBox(width: 8.w),
            Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              mainAxisSize: MainAxisSize.min,
              children: [
                Text(
                  achievement.title,
                  style: TextStyle(
                    fontSize: 12.sp,
                    fontWeight: FontWeight.w600,
                    color: AppTheme.textLight,
                  ),
                ),
                Text(
                  achievement.category.displayName,
                  style: TextStyle(
                    fontSize: 10.sp,
                    color: AppTheme.textMedium,
                  ),
                ),
              ],
            ),
          ],
        ),
      ),
    );
  }
}