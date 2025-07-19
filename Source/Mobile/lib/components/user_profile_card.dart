import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:fun/models/social_models.dart';
import 'package:fun/constants/colors.dart';


class UserProfileCard extends StatefulWidget {
  final ManifestationUser user;
  final VoidCallback onTap;
  final VoidCallback onFollow;

  const UserProfileCard({
    Key? key,
    required this.user,
    required this.onTap,
    required this.onFollow,
  }) : super(key: key);

  @override
  _UserProfileCardState createState() => _UserProfileCardState();
}

class _UserProfileCardState extends State<UserProfileCard>
    with SingleTickerProviderStateMixin {
  late AnimationController _followController;
  late Animation<double> _followAnimation;

  @override
  void initState() {
    super.initState();
    _followController = AnimationController(
      duration: Duration(milliseconds: 300),
      vsync: this,
    );
    _followAnimation = Tween<double>(
      begin: 1.0,
      end: 0.95,
    ).animate(CurvedAnimation(
      parent: _followController,
      curve: Curves.easeInOut,
    ));
  }

  @override
  void dispose() {
    _followController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: widget.onTap,
      child: Container(
        padding: EdgeInsets.all(16.w),
        decoration: BoxDecoration(
          color: AppColors.cardBackground,
          borderRadius: BorderRadius.circular(16.r),
          border: Border.all(
            color: AppColors.textMedium.withOpacity(0.1),
          ),
          boxShadow: [
            BoxShadow(
              color: AppColors.black.withOpacity(0.05),
              blurRadius: 8.r,
              offset: Offset(0, 4.h),
            ),
          ],
        ),
        child: Column(
          children: [
            // User header
            Row(
              children: [
                // Avatar with online status
                Stack(
                  children: [
                    Container(
                      width: 50.w,
                      height: 50.h,
                      decoration: BoxDecoration(
                        gradient: LinearGradient(
                          colors: [
                            widget.user.stats.levelColor,
                            widget.user.stats.levelColor.withOpacity(0.7),
                          ],
                        ),
                        shape: BoxShape.circle,
                      ),
                      child: Center(
                        child: Text(
                          widget.user.displayName[0].toUpperCase(),
                          style: TextStyle(
                            fontSize: 20.sp,
                            fontWeight: FontWeight.bold,
                            color: AppColors.white,
                          ),
                        ),
                      ),
                    ),
                    // Online indicator
                    if (_isRecentlyActive())
                      Positioned(
                        bottom: 2.h,
                        right: 2.w,
                        child: Container(
                          width: 12.w,
                          height: 12.h,
                          decoration: BoxDecoration(
                            color: AppColors.accentMint,
                            shape: BoxShape.circle,
                            border: Border.all(
                              color: AppColors.cardBackground,
                              width: 2,
                            ),
                          ),
                        ),
                      ),
                  ],
                ),
                SizedBox(width: 16.w),
                
                // User info
                Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Row(
                        children: [
                          Text(
                            widget.user.displayName,
                            style: TextStyle(
                              fontSize: 16.sp,
                              fontWeight: FontWeight.bold,
                              color: AppColors.textLight,
                            ),
                          ),
                          SizedBox(width: 4.w),
                          Container(
                            padding: EdgeInsets.symmetric(horizontal: 6.w, vertical: 2.h),
                            decoration: BoxDecoration(
                              color: widget.user.stats.levelColor.withOpacity(0.1),
                              borderRadius: BorderRadius.circular(8.r),
                            ),
                            child: Text(
                              widget.user.stats.level,
                              style: TextStyle(
                                fontSize: 10.sp,
                                fontWeight: FontWeight.w600,
                                color: widget.user.stats.levelColor,
                              ),
                            ),
                          ),
                        ],
                      ),
                      SizedBox(height: 2.h),
                      Text(
                        '@${widget.user.username}',
                        style: TextStyle(
                          fontSize: 13.sp,
                          color: AppColors.textMedium,
                        ),
                      ),
                      if (widget.user.bio.isNotEmpty) ...[
                        SizedBox(height: 4.h),
                        Text(
                          widget.user.bio,
                          style: TextStyle(
                            fontSize: 12.sp,
                            color: AppColors.textMedium,
                          ),
                          maxLines: 1,
                          overflow: TextOverflow.ellipsis,
                        ),
                      ],
                    ],
                  ),
                ),
                
                // Follow button
                AnimatedBuilder(
                  animation: _followAnimation,
                  builder: (context, child) {
                    return Transform.scale(
                      scale: _followAnimation.value,
                      child: GestureDetector(
                        onTap: () {
                          _followController.forward().then((_) {
                            _followController.reverse();
                          });
                          widget.onFollow();
                        },
                        child: Container(
                          padding: EdgeInsets.symmetric(
                            horizontal: 16.w,
                            vertical: 8.h,
                          ),
                          decoration: BoxDecoration(
                            gradient: widget.user.isFollowing
                                ? null
                                : LinearGradient(colors: AppColors.socialGradient),
                            color: widget.user.isFollowing
                                ? AppColors.backgroundMedium
                                : null,
                            borderRadius: BorderRadius.circular(20.r),
                            border: widget.user.isFollowing
                                ? Border.all(
                                    color: AppColors.textMedium.withOpacity(0.3),
                                  )
                                : null,
                          ),
                          child: Text(
                            widget.user.isFollowing ? 'Following' : 'Follow',
                            style: TextStyle(
                              fontSize: 12.sp,
                              fontWeight: FontWeight.w600,
                              color: widget.user.isFollowing
                                  ? AppColors.textMedium
                                  : AppColors.white,
                            ),
                          ),
                        ),
                      ),
                    );
                  },
                ),
              ],
            ),
            
            SizedBox(height: 16.h),
            
            // Stats row
            Row(
              children: [
                Expanded(
                  child: _buildStatItem(
                    '${widget.user.stats.currentStreak}',
                    'Day Streak',
                    Icons.local_fire_department,
                    AppColors.accentGold,
                  ),
                ),
                Expanded(
                  child: _buildStatItem(
                    '${widget.user.stats.meditationMinutes}m',
                    'Meditation',
                    Icons.self_improvement,
                    AppColors.visualMeditationGradient[0],
                  ),
                ),
                Expanded(
                  child: _buildStatItem(
                    '${widget.user.stats.goalsAchieved}',
                    'Goals',
                    Icons.flag,
                    AppColors.goalTrackerGradient[0],
                  ),
                ),
                Expanded(
                  child: _buildStatItem(
                    '${widget.user.stats.totalActivities}',
                    'Total',
                    Icons.trending_up,
                    AppColors.socialGradient[0],
                  ),
                ),
              ],
            ),
            
            // Badges row (if user has badges)
            if (widget.user.badges.isNotEmpty) ...[
              SizedBox(height: 16.h),
              _buildBadgesSection(),
            ],
          ],
        ),
      ),
    );
  }

  Widget _buildStatItem(String value, String label, IconData icon, Color color) {
    return Column(
      children: [
        Container(
          width: 32.w,
          height: 32.h,
          decoration: BoxDecoration(
            color: color.withOpacity(0.1),
            borderRadius: BorderRadius.circular(8.r),
          ),
          child: Icon(
            icon,
            color: color,
            size: 16.sp,
          ),
        ),
        SizedBox(height: 6.h),
        Text(
          value,
          style: TextStyle(
            fontSize: 14.sp,
            fontWeight: FontWeight.bold,
            color: AppColors.textLight,
          ),
        ),
        Text(
          label,
          style: TextStyle(
            fontSize: 10.sp,
            color: AppColors.textMedium,
          ),
        ),
      ],
    );
  }

  Widget _buildBadgesSection() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Recent Badges',
          style: TextStyle(
            fontSize: 12.sp,
            fontWeight: FontWeight.w600,
            color: AppColors.textMedium,
          ),
        ),
        SizedBox(height: 8.h),
        Row(
          children: widget.user.badges.take(3).map((badge) {
            return Container(
              margin: EdgeInsets.only(right: 8.w),
              child: _buildBadge(badge),
            );
          }).toList(),
        ),
      ],
    );
  }

  Widget _buildBadge(String badge) {
    // Map badge IDs to display info
    final badgeInfo = _getBadgeInfo(badge);
    
    return Container(
      padding: EdgeInsets.symmetric(horizontal: 8.w, vertical: 4.h),
      decoration: BoxDecoration(
        color: badgeInfo['color'].withOpacity(0.1),
        borderRadius: BorderRadius.circular(12.r),
        border: Border.all(
          color: badgeInfo['color'].withOpacity(0.3),
        ),
      ),
      child: Row(
        mainAxisSize: MainAxisSize.min,
        children: [
          Text(
            badgeInfo['emoji'],
            style: TextStyle(fontSize: 12.sp),
          ),
          SizedBox(width: 4.w),
          Text(
            badgeInfo['name'],
            style: TextStyle(
              fontSize: 10.sp,
              fontWeight: FontWeight.w600,
              color: badgeInfo['color'],
            ),
          ),
        ],
      ),
    );
  }

  Map<String, dynamic> _getBadgeInfo(String badge) {
    switch (badge) {
      case 'streak_7':
        return {
          'name': '7 Day',
          'emoji': '🔥',
          'color': AppColors.accentGold,
        };
      case 'streak_30':
        return {
          'name': '30 Day',
          'emoji': '🔥',
          'color': AppColors.accentGold,
        };
      case 'meditation_master':
        return {
          'name': 'Zen',
          'emoji': '🧘‍♀️',
          'color': AppColors.visualMeditationGradient[0],
        };
      case 'goal_achiever':
        return {
          'name': 'Goals',
          'emoji': '🎯',
          'color': AppColors.goalTrackerGradient[0],
        };
      case 'goal_crusher':
        return {
          'name': 'Crusher',
          'emoji': '💪',
          'color': AppColors.goalTrackerGradient[1],
        };
      case 'vision_creator':
        return {
          'name': 'Vision',
          'emoji': '🎨',
          'color': AppColors.visionBoardGradient[0],
        };
      case 'gratitude_master':
        return {
          'name': 'Grateful',
          'emoji': '🙏',
          'color': AppColors.affirmationGradient[0],
        };
      case 'affirmation_lover':
        return {
          'name': 'Positive',
          'emoji': '💖',
          'color': AppColors.affirmationGradient[1],
        };
      case 'zen_master':
        return {
          'name': 'Master',
          'emoji': '🏆',
          'color': AppColors.gradientPurple,
        };
      default:
        return {
          'name': 'Badge',
          'emoji': '⭐',
          'color': AppColors.textMedium,
        };
    }
  }

  bool _isRecentlyActive() {
    final timeDifference = DateTime.now().difference(widget.user.stats.lastActive);
    return timeDifference.inHours < 24;
  }
}