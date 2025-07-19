import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:manifestation_app/models/social_models.dart';
import 'package:manifestation_app/constants/colors.dart';

class SocialPostCard extends StatefulWidget {
  final SocialPost post;
  final VoidCallback onLike;
  final VoidCallback onComment;

  const SocialPostCard({
    Key? key,
    required this.post,
    required this.onLike,
    required this.onComment,
  }) : super(key: key);

  @override
  _SocialPostCardState createState() => _SocialPostCardState();
}

class _SocialPostCardState extends State<SocialPostCard>
    with SingleTickerProviderStateMixin {
  late AnimationController _likeController;
  late Animation<double> _likeAnimation;

  @override
  void initState() {
    super.initState();
    _likeController = AnimationController(
      duration: Duration(milliseconds: 300),
      vsync: this,
    );
    _likeAnimation = Tween<double>(
      begin: 1.0,
      end: 1.3,
    ).animate(CurvedAnimation(
      parent: _likeController,
      curve: Curves.elasticOut,
    ));
  }

  @override
  void dispose() {
    _likeController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Container(
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
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          // Header
          _buildPostHeader(),
          
          // Content
          _buildPostContent(),
          
          // Achievement badge (if applicable)
          if (widget.post.achievement != null)
            _buildAchievementSection(),
          
          // Actions
          _buildPostActions(),
        ],
      ),
    );
  }

  Widget _buildPostHeader() {
    return Padding(
      padding: EdgeInsets.all(16.w),
      child: Row(
        children: [
          // User avatar
          Container(
            width: 44.w,
            height: 44.h,
            decoration: BoxDecoration(
              gradient: LinearGradient(
                colors: [
                  widget.post.type.color,
                  widget.post.type.color.withOpacity(0.7),
                ],
              ),
              shape: BoxShape.circle,
            ),
            child: Center(
              child: Text(
                widget.post.user.displayName[0].toUpperCase(),
                style: TextStyle(
                  fontSize: 18.sp,
                  fontWeight: FontWeight.bold,
                  color: AppColors.white,
                ),
              ),
            ),
          ),
          SizedBox(width: 12.w),
          
          // User info
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Row(
                  children: [
                    Text(
                      widget.post.user.displayName,
                      style: TextStyle(
                        fontSize: 16.sp,
                        fontWeight: FontWeight.bold,
                        color: AppColors.textLight,
                      ),
                    ),
                    SizedBox(width: 4.w),
                    if (widget.post.user.stats.level != 'Beginner')
                      Container(
                        padding: EdgeInsets.symmetric(horizontal: 6.w, vertical: 2.h),
                        decoration: BoxDecoration(
                          color: widget.post.user.stats.levelColor.withOpacity(0.1),
                          borderRadius: BorderRadius.circular(8.r),
                        ),
                        child: Text(
                          widget.post.user.stats.level,
                          style: TextStyle(
                            fontSize: 10.sp,
                            fontWeight: FontWeight.w600,
                            color: widget.post.user.stats.levelColor,
                          ),
                        ),
                      ),
                  ],
                ),
                Row(
                  children: [
                    Icon(
                      widget.post.type.icon,
                      color: widget.post.type.color,
                      size: 14.sp,
                    ),
                    SizedBox(width: 4.w),
                    Text(
                      widget.post.type.displayName,
                      style: TextStyle(
                        fontSize: 12.sp,
                        color: widget.post.type.color,
                        fontWeight: FontWeight.w500,
                      ),
                    ),
                    SizedBox(width: 8.w),
                    Text(
                      _formatTime(widget.post.createdAt),
                      style: TextStyle(
                        fontSize: 12.sp,
                        color: AppColors.textMedium,
                      ),
                    ),
                  ],
                ),
              ],
            ),
          ),
          
          // Post menu
          GestureDetector(
            onTap: _showPostMenu,
            child: Container(
              width: 32.w,
              height: 32.h,
              decoration: BoxDecoration(
                color: AppColors.backgroundMedium,
                borderRadius: BorderRadius.circular(8.r),
              ),
              child: Icon(
                Icons.more_horiz,
                color: AppColors.textMedium,
                size: 16.sp,
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildPostContent() {
    return Padding(
      padding: EdgeInsets.symmetric(horizontal: 16.w),
      child: Text(
        widget.post.content,
        style: TextStyle(
          fontSize: 15.sp,
          color: AppColors.textLight,
          height: 1.4,
        ),
      ),
    );
  }

  Widget _buildAchievementSection() {
    final achievement = widget.post.achievement!;
    
    return Container(
      margin: EdgeInsets.all(16.w),
      padding: EdgeInsets.all(16.w),
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
        children: [
          // Achievement icon
          Container(
            width: 50.w,
            height: 50.h,
            decoration: BoxDecoration(
              color: achievement.color.withOpacity(0.1),
              borderRadius: BorderRadius.circular(12.r),
            ),
            child: Icon(
              achievement.icon,
              color: achievement.color,
              size: 24.sp,
            ),
          ),
          SizedBox(width: 16.w),
          
          // Achievement info
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  achievement.title,
                  style: TextStyle(
                    fontSize: 16.sp,
                    fontWeight: FontWeight.bold,
                    color: AppColors.textLight,
                  ),
                ),
                SizedBox(height: 2.h),
                Text(
                  achievement.description,
                  style: TextStyle(
                    fontSize: 13.sp,
                    color: AppColors.textMedium,
                  ),
                ),
              ],
            ),
          ),
          
          // Achievement badge
          Container(
            padding: EdgeInsets.symmetric(horizontal: 8.w, vertical: 4.h),
            decoration: BoxDecoration(
              color: achievement.color,
              borderRadius: BorderRadius.circular(12.r),
            ),
            child: Text(
              '🏆',
              style: TextStyle(fontSize: 16.sp),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildPostActions() {
    return Container(
      padding: EdgeInsets.all(16.w),
      child: Row(
        children: [
          // Like button
          GestureDetector(
            onTap: () {
              widget.onLike();
              if (widget.post.isLikedByCurrentUser) {
                _likeController.forward().then((_) {
                  _likeController.reverse();
                });
              }
            },
            child: AnimatedBuilder(
              animation: _likeAnimation,
              builder: (context, child) {
                return Transform.scale(
                  scale: _likeAnimation.value,
                  child: Row(
                    mainAxisSize: MainAxisSize.min,
                    children: [
                      Icon(
                        widget.post.isLikedByCurrentUser
                            ? Icons.favorite
                            : Icons.favorite_border,
                        color: widget.post.isLikedByCurrentUser
                            ? Colors.red
                            : AppColors.textMedium,
                        size: 20.sp,
                      ),
                      SizedBox(width: 6.w),
                      Text(
                        '${widget.post.likes.length}',
                        style: TextStyle(
                          fontSize: 14.sp,
                          color: AppColors.textMedium,
                          fontWeight: FontWeight.w500,
                        ),
                      ),
                    ],
                  ),
                );
              },
            ),
          ),
          
          SizedBox(width: 24.w),
          
          // Comment button
          GestureDetector(
            onTap: widget.onComment,
            child: Row(
              mainAxisSize: MainAxisSize.min,
              children: [
                Icon(
                  Icons.chat_bubble_outline,
                  color: AppColors.textMedium,
                  size: 20.sp,
                ),
                SizedBox(width: 6.w),
                Text(
                  '${widget.post.comments.length}',
                  style: TextStyle(
                    fontSize: 14.sp,
                    color: AppColors.textMedium,
                    fontWeight: FontWeight.w500,
                  ),
                ),
              ],
            ),
          ),
          
          Spacer(),
          
          // Share button
          GestureDetector(
            onTap: _sharePost,
            child: Container(
              padding: EdgeInsets.symmetric(horizontal: 12.w, vertical: 6.h),
              decoration: BoxDecoration(
                color: AppColors.socialGradient[0].withOpacity(0.1),
                borderRadius: BorderRadius.circular(20.r),
              ),
              child: Row(
                mainAxisSize: MainAxisSize.min,
                children: [
                  Icon(
                    Icons.share,
                    color: AppColors.socialGradient[0],
                    size: 16.sp,
                  ),
                  SizedBox(width: 4.w),
                  Text(
                    'Share',
                    style: TextStyle(
                      fontSize: 12.sp,
                      color: AppColors.socialGradient[0],
                      fontWeight: FontWeight.w600,
                    ),
                  ),
                ],
              ),
            ),
          ),
        ],
      ),
    );
  }

  String _formatTime(DateTime dateTime) {
    final now = DateTime.now();
    final difference = now.difference(dateTime);
    
    if (difference.inMinutes < 60) {
      return '${difference.inMinutes}m';
    } else if (difference.inHours < 24) {
      return '${difference.inHours}h';
    } else if (difference.inDays < 7) {
      return '${difference.inDays}d';
    } else {
      return '${(difference.inDays / 7).floor()}w';
    }
  }

  void _showPostMenu() {
    showModalBottomSheet(
      context: context,
      backgroundColor: Colors.transparent,
      builder: (context) => Container(
        margin: EdgeInsets.all(20.w),
        decoration: BoxDecoration(
          color: AppColors.cardBackground,
          borderRadius: BorderRadius.circular(16.r),
        ),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            Container(
              width: 40.w,
              height: 4.h,
              margin: EdgeInsets.only(top: 12.h),
              decoration: BoxDecoration(
                color: AppColors.textMedium.withOpacity(0.3),
                borderRadius: BorderRadius.circular(2.r),
              ),
            ),
            
            _buildMenuOption(
              icon: Icons.bookmark_border,
              title: 'Save Post',
              onTap: () {
                Navigator.pop(context);
                // TODO: Save post functionality
              },
            ),
            
            _buildMenuOption(
              icon: Icons.flag_outlined,
              title: 'Report Post',
              onTap: () {
                Navigator.pop(context);
                // TODO: Report post functionality
              },
            ),
            
            if (widget.post.user.id == 'current_user')
              _buildMenuOption(
                icon: Icons.delete_outline,
                title: 'Delete Post',
                onTap: () {
                  Navigator.pop(context);
                  // TODO: Delete post functionality
                },
                isDestructive: true,
              ),
            
            SizedBox(height: 12.h),
          ],
        ),
      ),
    );
  }

  Widget _buildMenuOption({
    required IconData icon,
    required String title,
    required VoidCallback onTap,
    bool isDestructive = false,
  }) {
    return GestureDetector(
      onTap: onTap,
      child: Container(
        width: double.infinity,
        padding: EdgeInsets.symmetric(horizontal: 20.w, vertical: 16.h),
        child: Row(
          children: [
            Icon(
              icon,
              color: isDestructive ? Colors.red : AppColors.textMedium,
              size: 20.sp,
            ),
            SizedBox(width: 16.w),
            Text(
              title,
              style: TextStyle(
                fontSize: 16.sp,
                color: isDestructive ? Colors.red : AppColors.textLight,
                fontWeight: FontWeight.w500,
              ),
            ),
          ],
        ),
      ),
    );
  }

  void _sharePost() {
    // TODO: Implement share functionality
    print('Sharing post: ${widget.post.id}');
  }
}