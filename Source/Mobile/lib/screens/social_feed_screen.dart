import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:manifestation_app/components/achievement_badge_component.dart';
import 'package:manifestation_app/models/social_models.dart';
import 'package:manifestation_app/components/social_post_card_component.dart';
import 'package:manifestation_app/components/user_profile_card.dart';
import 'package:manifestation_app/constants/colors.dart';

class SocialFeedScreen extends StatefulWidget {
  @override
  _SocialFeedScreenState createState() => _SocialFeedScreenState();
}

class _SocialFeedScreenState extends State<SocialFeedScreen>
    with TickerProviderStateMixin {
  late TabController _tabController;
  int selectedTabIndex = 0;
  
  // Mock data - replace with actual data later
  late List<SocialPost> feedPosts;
  late List<ManifestationUser> friends;
  late ManifestationUser currentUser;
  late List<Achievement> recentAchievements;

  @override
  void initState() {
    super.initState();
    _tabController = TabController(length: 3, vsync: this);
    _loadMockData();
  }

  @override
  void dispose() {
    _tabController.dispose();
    super.dispose();
  }

  void _loadMockData() {
    // Current user
    currentUser = ManifestationUser(
      id: 'current_user',
      username: 'manifester_you',
      displayName: 'You',
      bio: 'Manifesting my best life ✨',
      joinedDate: DateTime.now().subtract(Duration(days: 45)),
      stats: UserStats(
        meditationMinutes: 180,
        affirmationsListened: 25,
        journalEntries: 15,
        goalsAchieved: 3,
        visionBoardsCreated: 2,
        currentStreak: 7,
        longestStreak: 12,
        lastActive: DateTime.now(),
      ),
      badges: ['streak_7', 'meditation_master', 'goal_achiever'],
    );

    // Friends
    friends = [
      ManifestationUser(
        id: 'user_1',
        username: 'zen_sarah',
        displayName: 'Sarah M.',
        bio: 'Meditation teacher & manifestation coach 🧘‍♀️',
        joinedDate: DateTime.now().subtract(Duration(days: 120)),
        stats: UserStats(
          meditationMinutes: 1250,
          affirmationsListened: 89,
          journalEntries: 67,
          goalsAchieved: 12,
          visionBoardsCreated: 8,
          currentStreak: 25,
          longestStreak: 45,
          lastActive: DateTime.now().subtract(Duration(hours: 2)),
        ),
        badges: ['meditation_master', 'streak_30', 'zen_master'],
        isFollowing: true,
      ),
      ManifestationUser(
        id: 'user_2',
        username: 'dreamchaser_mike',
        displayName: 'Mike R.',
        bio: 'Entrepreneur manifesting abundance 💰',
        joinedDate: DateTime.now().subtract(Duration(days: 89)),
        stats: UserStats(
          meditationMinutes: 340,
          affirmationsListened: 45,
          journalEntries: 23,
          goalsAchieved: 8,
          visionBoardsCreated: 5,
          currentStreak: 12,
          longestStreak: 18,
          lastActive: DateTime.now().subtract(Duration(minutes: 30)),
        ),
        badges: ['goal_crusher', 'vision_creator'],
        isFollowing: true,
      ),
      ManifestationUser(
        id: 'user_3',
        username: 'mindful_luna',
        displayName: 'Luna K.',
        bio: 'Spreading positive vibes & gratitude 🌙',
        joinedDate: DateTime.now().subtract(Duration(days: 156)),
        stats: UserStats(
          meditationMinutes: 890,
          affirmationsListened: 156,
          journalEntries: 89,
          goalsAchieved: 15,
          visionBoardsCreated: 12,
          currentStreak: 34,
          longestStreak: 56,
          lastActive: DateTime.now().subtract(Duration(hours: 1)),
        ),
        badges: ['gratitude_master', 'streak_30', 'affirmation_lover'],
        isFollowing: true,
      ),
    ];

    // Feed posts
    feedPosts = [
      SocialPost(
        id: 'post_1',
        user: friends[0],
        type: SocialPostType.achievement,
        content: 'Just unlocked the Zen Master achievement! 100 minutes of meditation completed 🧘‍♀️',
        achievement: Achievement.getAvailableAchievements()[1],
        createdAt: DateTime.now().subtract(Duration(hours: 2)),
        likes: ['user_2', 'user_3', 'current_user'],
        comments: [
          SocialComment(
            id: 'comment_1',
            userId: 'user_2',
            username: 'dreamchaser_mike',
            content: 'Amazing! You\'re such an inspiration 🙏',
            createdAt: DateTime.now().subtract(Duration(hours: 1)),
          ),
        ],
      ),
      SocialPost(
        id: 'post_2',
        user: currentUser,
        type: SocialPostType.milestone,
        content: 'Reached my 7-day streak! Feeling so grateful for this journey ✨',
        createdAt: DateTime.now().subtract(Duration(hours: 6)),
        likes: ['user_1', 'user_3'],
        comments: [],
      ),
      SocialPost(
        id: 'post_3',
        user: friends[1],
        type: SocialPostType.goalComplete,
        content: 'Just manifested my dream job! The universe really does provide when you align your energy 💫',
        createdAt: DateTime.now().subtract(Duration(hours: 8)),
        likes: ['user_1', 'user_3', 'current_user'],
        comments: [
          SocialComment(
            id: 'comment_2',
            userId: 'user_1',
            username: 'zen_sarah',
            content: 'Congratulations! Your energy attracted this! 🎉',
            createdAt: DateTime.now().subtract(Duration(hours: 7)),
          ),
          SocialComment(
            id: 'comment_3',
            userId: 'current_user',
            username: 'manifester_you',
            content: 'So happy for you! This is amazing! 🥳',
            createdAt: DateTime.now().subtract(Duration(hours: 6)),
          ),
        ],
      ),
      SocialPost(
        id: 'post_4',
        user: friends[2],
        type: SocialPostType.reflection,
        content: 'Today I\'m grateful for morning sunlight, my coffee, and the ability to dream big. What are you grateful for? 🌅',
        createdAt: DateTime.now().subtract(Duration(days: 1)),
        likes: ['user_1', 'user_2'],
        comments: [],
      ),
    ];

    recentAchievements = Achievement.getAvailableAchievements().take(3).toList();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.backgroundDark,
      body: SafeArea(
        child: Column(
          children: [
            // Header
            _buildHeader(),
            
            // Tab bar
            _buildTabBar(),
            
            // Content
            Expanded(
              child: TabBarView(
                controller: _tabController,
                children: [
                  _buildFeedTab(),
                  _buildFriendsTab(),
                  _buildAchievementsTab(),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildHeader() {
    return Padding(
      padding: EdgeInsets.all(20.w),
      child: Row(
        children: [
          GestureDetector(
            onTap: () => Navigator.pop(context),
            child: Container(
              width: 44.w,
              height: 44.h,
              decoration: BoxDecoration(
                color: AppColors.cardBackground,
                borderRadius: BorderRadius.circular(12.r),
              ),
              child: Icon(
                Icons.arrow_back,
                color: AppColors.textLight,
                size: 20.sp,
              ),
            ),
          ),
          SizedBox(width: 16.w),
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  'Manifestation Community',
                  style: TextStyle(
                    fontSize: 24.sp,
                    fontWeight: FontWeight.bold,
                    color: AppColors.textLight,
                  ),
                ),
                Text(
                  'Connect with fellow manifesters',
                  style: TextStyle(
                    fontSize: 14.sp,
                    color: AppColors.textMedium,
                  ),
                ),
              ],
            ),
          ),
          // Notifications icon
          Container(
            width: 44.w,
            height: 44.h,
            decoration: BoxDecoration(
              color: AppColors.cardBackground,
              borderRadius: BorderRadius.circular(12.r),
            ),
            child: Stack(
              children: [
                Center(
                  child: Icon(
                    Icons.notifications_outlined,
                    color: AppColors.textLight,
                    size: 20.sp,
                  ),
                ),
                // Notification dot
                Positioned(
                  top: 8.h,
                  right: 8.w,
                  child: Container(
                    width: 8.w,
                    height: 8.h,
                    decoration: BoxDecoration(
                      color: AppColors.socialGradient[0],
                      shape: BoxShape.circle,
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

  Widget _buildTabBar() {
    return Container(
      margin: EdgeInsets.symmetric(horizontal: 20.w),
      decoration: BoxDecoration(
        color: AppColors.cardBackground,
        borderRadius: BorderRadius.circular(16.r),
      ),
      child: TabBar(
        controller: _tabController,
        indicator: BoxDecoration(
          gradient: LinearGradient(colors: AppColors.socialGradient),
          borderRadius: BorderRadius.circular(12.r),
        ),
        indicatorSize: TabBarIndicatorSize.tab,
        indicatorPadding: EdgeInsets.all(4.w),
        labelColor: AppColors.white,
        unselectedLabelColor: AppColors.textMedium,
        labelStyle: TextStyle(
          fontSize: 14.sp,
          fontWeight: FontWeight.w600,
        ),
        unselectedLabelStyle: TextStyle(
          fontSize: 14.sp,
          fontWeight: FontWeight.w500,
        ),
        tabs: [
          Tab(text: 'Feed'),
          Tab(text: 'Friends'),
          Tab(text: 'Achievements'),
        ],
      ),
    );
  }

  Widget _buildFeedTab() {
    return Column(
      children: [
        SizedBox(height: 20.h),
        
        // Your stats card
        _buildYourStatsCard(),
        
        SizedBox(height: 20.h),
        
        // Feed posts
        Expanded(
          child: ListView.builder(
            padding: EdgeInsets.symmetric(horizontal: 20.w),
            itemCount: feedPosts.length,
            itemBuilder: (context, index) {
              return Container(
                margin: EdgeInsets.only(bottom: 16.h),
                child: SocialPostCard(
                  post: feedPosts[index],
                  onLike: () => _toggleLike(feedPosts[index]),
                  onComment: () => _showComments(feedPosts[index]),
                ),
              );
            },
          ),
        ),
      ],
    );
  }

  Widget _buildYourStatsCard() {
    return Container(
      margin: EdgeInsets.symmetric(horizontal: 20.w),
      padding: EdgeInsets.all(20.w),
      decoration: BoxDecoration(
        gradient: LinearGradient(
          colors: [
            AppColors.socialGradient[0].withOpacity(0.1),
            AppColors.socialGradient[1].withOpacity(0.05),
          ],
        ),
        borderRadius: BorderRadius.circular(20.r),
        border: Border.all(
          color: AppColors.socialGradient[0].withOpacity(0.3),
        ),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            children: [
              // Avatar
              Container(
                width: 50.w,
                height: 50.h,
                decoration: BoxDecoration(
                  gradient: LinearGradient(colors: AppColors.socialGradient),
                  shape: BoxShape.circle,
                ),
                child: Center(
                  child: Text(
                    currentUser.displayName[0].toUpperCase(),
                    style: TextStyle(
                      fontSize: 20.sp,
                      fontWeight: FontWeight.bold,
                      color: AppColors.white,
                    ),
                  ),
                ),
              ),
              SizedBox(width: 16.w),
              Expanded(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      'Your Progress',
                      style: TextStyle(
                        fontSize: 18.sp,
                        fontWeight: FontWeight.bold,
                        color: AppColors.textLight,
                      ),
                    ),
                    Text(
                      '${currentUser.stats.level} • ${currentUser.stats.currentStreak} day streak',
                      style: TextStyle(
                        fontSize: 14.sp,
                        color: AppColors.textMedium,
                      ),
                    ),
                  ],
                ),
              ),
              // Level badge
              Container(
                padding: EdgeInsets.symmetric(horizontal: 12.w, vertical: 6.h),
                decoration: BoxDecoration(
                  color: currentUser.stats.levelColor.withOpacity(0.1),
                  borderRadius: BorderRadius.circular(12.r),
                  border: Border.all(
                    color: currentUser.stats.levelColor.withOpacity(0.3),
                  ),
                ),
                child: Text(
                  currentUser.stats.level,
                  style: TextStyle(
                    fontSize: 12.sp,
                    fontWeight: FontWeight.w600,
                    color: currentUser.stats.levelColor,
                  ),
                ),
              ),
            ],
          ),
          
          SizedBox(height: 20.h),
          
          // Stats grid
          Row(
            children: [
              Expanded(
                child: _buildStatItem(
                  '${currentUser.stats.meditationMinutes}m',
                  'Meditation',
                  Icons.self_improvement,
                  AppColors.visualMeditationGradient[0],
                ),
              ),
              Expanded(
                child: _buildStatItem(
                  '${currentUser.stats.goalsAchieved}',
                  'Goals',
                  Icons.flag,
                  AppColors.goalTrackerGradient[0],
                ),
              ),
              Expanded(
                child: _buildStatItem(
                  '${currentUser.stats.journalEntries}',
                  'Entries',
                  Icons.book,
                  AppColors.journalGradient[0],
                ),
              ),
              Expanded(
                child: _buildStatItem(
                  '${currentUser.stats.currentStreak}',
                  'Streak',
                  Icons.local_fire_department,
                  AppColors.accentGold,
                ),
              ),
            ],
          ),
        ],
      ),
    );
  }

  Widget _buildStatItem(String value, String label, IconData icon, Color color) {
    return Column(
      children: [
        Container(
          width: 40.w,
          height: 40.h,
          decoration: BoxDecoration(
            color: color.withOpacity(0.1),
            borderRadius: BorderRadius.circular(10.r),
          ),
          child: Icon(
            icon,
            color: color,
            size: 20.sp,
          ),
        ),
        SizedBox(height: 8.h),
        Text(
          value,
          style: TextStyle(
            fontSize: 16.sp,
            fontWeight: FontWeight.bold,
            color: AppColors.textLight,
          ),
        ),
        Text(
          label,
          style: TextStyle(
            fontSize: 12.sp,
            color: AppColors.textMedium,
          ),
        ),
      ],
    );
  }

  Widget _buildFriendsTab() {
    return ListView.builder(
      padding: EdgeInsets.all(20.w),
      itemCount: friends.length + 1, // +1 for "Find Friends" card
      itemBuilder: (context, index) {
        if (index == 0) {
          return Container(
            margin: EdgeInsets.only(bottom: 16.h),
            child: _buildFindFriendsCard(),
          );
        }
        
        final friend = friends[index - 1];
        return Container(
          margin: EdgeInsets.only(bottom: 16.h),
          child: UserProfileCard(
            user: friend,
            onTap: () => _showUserProfile(friend),
            onFollow: () => _toggleFollow(friend),
          ),
        );
      },
    );
  }

  Widget _buildFindFriendsCard() {
    return Container(
      padding: EdgeInsets.all(20.w),
      decoration: BoxDecoration(
        gradient: LinearGradient(
          colors: [
            AppColors.socialGradient[0].withOpacity(0.1),
            AppColors.socialGradient[1].withOpacity(0.05),
          ],
        ),
        borderRadius: BorderRadius.circular(16.r),
        border: Border.all(
          color: AppColors.socialGradient[0].withOpacity(0.3),
        ),
      ),
      child: Row(
        children: [
          Container(
            width: 50.w,
            height: 50.h,
            decoration: BoxDecoration(
              gradient: LinearGradient(colors: AppColors.socialGradient),
              borderRadius: BorderRadius.circular(12.r),
            ),
            child: Icon(
              Icons.person_add,
              color: AppColors.white,
              size: 24.sp,
            ),
          ),
          SizedBox(width: 16.w),
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  'Find Friends',
                  style: TextStyle(
                    fontSize: 16.sp,
                    fontWeight: FontWeight.bold,
                    color: AppColors.textLight,
                  ),
                ),
                Text(
                  'Connect with other manifesters',
                  style: TextStyle(
                    fontSize: 14.sp,
                    color: AppColors.textMedium,
                  ),
                ),
              ],
            ),
          ),
          Icon(
            Icons.arrow_forward_ios,
            color: AppColors.textMedium,
            size: 16.sp,
          ),
        ],
      ),
    );
  }

  Widget _buildAchievementsTab() {
    return SingleChildScrollView(
      padding: EdgeInsets.all(20.w),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          // Recent achievements
          Text(
            'Recent Achievements',
            style: TextStyle(
              fontSize: 20.sp,
              fontWeight: FontWeight.bold,
              color: AppColors.textLight,
            ),
          ),
          SizedBox(height: 16.h),
          
          ...recentAchievements.map((achievement) {
            return Container(
              margin: EdgeInsets.only(bottom: 12.h),
              child: AchievementBadge(
                achievement: achievement,
                onTap: () => _showAchievementDetails(achievement),
              ),
            );
          }).toList(),
          
          SizedBox(height: 30.h),
          
          // Achievement categories
          Text(
            'Categories',
            style: TextStyle(
              fontSize: 20.sp,
              fontWeight: FontWeight.bold,
              color: AppColors.textLight,
            ),
          ),
          SizedBox(height: 16.h),
          
          _buildAchievementCategories(),
          
          SizedBox(height: 30.h),
          
          // Share achievements button
          SizedBox(
            width: double.infinity,
            height: 50.h,
            child: ElevatedButton(
              onPressed: _shareAchievements,
              style: ElevatedButton.styleFrom(
                backgroundColor: Colors.transparent,
                shadowColor: Colors.transparent,
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(12.r),
                ),
              ).copyWith(
                backgroundColor: MaterialStateProperty.all(Colors.transparent),
              ),
              child: Ink(
                decoration: BoxDecoration(
                  gradient: LinearGradient(colors: AppColors.socialGradient),
                  borderRadius: BorderRadius.circular(12.r),
                ),
                child: Container(
                  alignment: Alignment.center,
                  child: Row(
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: [
                      Icon(
                        Icons.share,
                        color: AppColors.white,
                        size: 20.sp,
                      ),
                      SizedBox(width: 8.w),
                      Text(
                        'Share My Progress',
                        style: TextStyle(
                          fontSize: 16.sp,
                          fontWeight: FontWeight.w600,
                          color: AppColors.white,
                        ),
                      ),
                    ],
                  ),
                ),
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildAchievementCategories() {
    return GridView.builder(
      shrinkWrap: true,
      physics: NeverScrollableScrollPhysics(),
      gridDelegate: SliverGridDelegateWithFixedCrossAxisCount(
        crossAxisCount: 2,
        crossAxisSpacing: 12.w,
        mainAxisSpacing: 12.h,
        childAspectRatio: 1.2,
      ),
      itemCount: AchievementCategory.values.length,
      itemBuilder: (context, index) {
        final category = AchievementCategory.values[index];
        final achievementsInCategory = recentAchievements
            .where((a) => a.category == category)
            .length;
        
        return GestureDetector(
          onTap: () => _showCategoryAchievements(category),
          child: Container(
            padding: EdgeInsets.all(16.w),
            decoration: BoxDecoration(
              color: AppColors.cardBackground,
              borderRadius: BorderRadius.circular(16.r),
              border: Border.all(
                color: AppColors.textMedium.withOpacity(0.2),
              ),
            ),
            child: Column(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                Icon(
                  category.icon,
                  color: AppColors.socialGradient[0],
                  size: 32.sp,
                ),
                SizedBox(height: 8.h),
                Text(
                  category.displayName,
                  style: TextStyle(
                    fontSize: 14.sp,
                    fontWeight: FontWeight.w600,
                    color: AppColors.textLight,
                  ),
                  textAlign: TextAlign.center,
                ),
                SizedBox(height: 4.h),
                Text(
                  '$achievementsInCategory unlocked',
                  style: TextStyle(
                    fontSize: 12.sp,
                    color: AppColors.textMedium,
                  ),
                ),
              ],
            ),
          ),
        );
      },
    );
  }

  void _toggleLike(SocialPost post) {
    setState(() {
      final updatedPost = post.copyWith(
        likes: post.isLikedByCurrentUser
            ? post.likes.where((id) => id != 'current_user_id').toList()
            : [...post.likes, 'current_user_id'],
      );
      
      final index = feedPosts.indexWhere((p) => p.id == post.id);
      if (index != -1) {
        feedPosts[index] = updatedPost;
      }
    });
  }

  void _showComments(SocialPost post) {
    showModalBottomSheet(
      context: context,
      backgroundColor: Colors.transparent,
      isScrollControlled: true,
      builder: (context) => Container(
        height: MediaQuery.of(context).size.height * 0.7,
        decoration: BoxDecoration(
          color: AppColors.cardBackground,
          borderRadius: BorderRadius.vertical(top: Radius.circular(20.r)),
        ),
        child: Column(
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
            Padding(
              padding: EdgeInsets.all(20.w),
              child: Text(
                'Comments',
                style: TextStyle(
                  fontSize: 18.sp,
                  fontWeight: FontWeight.bold,
                  color: AppColors.textLight,
                ),
              ),
            ),
            Expanded(
              child: ListView.builder(
                padding: EdgeInsets.symmetric(horizontal: 20.w),
                itemCount: post.comments.length,
                itemBuilder: (context, index) {
                  final comment = post.comments[index];
                  return Container(
                    margin: EdgeInsets.only(bottom: 16.h),
                    child: Row(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Container(
                          width: 32.w,
                          height: 32.h,
                          decoration: BoxDecoration(
                            color: AppColors.socialGradient[0],
                            shape: BoxShape.circle,
                          ),
                          child: Center(
                            child: Text(
                              comment.username[0].toUpperCase(),
                              style: TextStyle(
                                fontSize: 14.sp,
                                fontWeight: FontWeight.bold,
                                color: AppColors.white,
                              ),
                            ),
                          ),
                        ),
                        SizedBox(width: 12.w),
                        Expanded(
                          child: Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              Text(
                                comment.username,
                                style: TextStyle(
                                  fontSize: 14.sp,
                                  fontWeight: FontWeight.w600,
                                  color: AppColors.textLight,
                                ),
                              ),
                              SizedBox(height: 2.h),
                              Text(
                                comment.content,
                                style: TextStyle(
                                  fontSize: 14.sp,
                                  color: AppColors.textMedium,
                                ),
                              ),
                            ],
                          ),
                        ),
                      ],
                    ),
                  );
                },
              ),
            ),
            // Comment input
            Container(
              padding: EdgeInsets.all(20.w),
              child: Row(
                children: [
                  Expanded(
                    child: TextField(
                      style: TextStyle(color: AppColors.textLight),
                      decoration: InputDecoration(
                        hintText: 'Add a comment...',
                        hintStyle: TextStyle(color: AppColors.textMedium),
                        filled: true,
                        fillColor: AppColors.backgroundMedium,
                        border: OutlineInputBorder(
                          borderRadius: BorderRadius.circular(25.r),
                          borderSide: BorderSide.none,
                        ),
                        contentPadding: EdgeInsets.symmetric(
                          horizontal: 16.w,
                          vertical: 12.h,
                        ),
                      ),
                    ),
                  ),
                  SizedBox(width: 12.w),
                  Container(
                    width: 44.w,
                    height: 44.h,
                    decoration: BoxDecoration(
                      gradient: LinearGradient(colors: AppColors.socialGradient),
                      shape: BoxShape.circle,
                    ),
                    child: IconButton(
                      onPressed: () {
                        // TODO: Add comment functionality
                        Navigator.pop(context);
                      },
                      icon: Icon(
                        Icons.send,
                        color: AppColors.white,
                        size: 20.sp,
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

  void _showUserProfile(ManifestationUser user) {
    // TODO: Navigate to user profile screen
    print('Show profile for ${user.username}');
  }

  void _toggleFollow(ManifestationUser user) {
    setState(() {
      final index = friends.indexWhere((f) => f.id == user.id);
      if (index != -1) {
        friends[index] = user.copyWith(isFollowing: !user.isFollowing);
      }
    });
  }

  void _showAchievementDetails(Achievement achievement) {
    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        backgroundColor: AppColors.cardBackground,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(16.r),
        ),
        content: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            Container(
              width: 80.w,
              height: 80.h,
              decoration: BoxDecoration(
                color: achievement.color.withOpacity(0.1),
                shape: BoxShape.circle,
              ),
              child: Icon(
                achievement.icon,
                color: achievement.color,
                size: 40.sp,
              ),
            ),
            SizedBox(height: 16.h),
            Text(
              achievement.title,
              style: TextStyle(
                fontSize: 20.sp,
                fontWeight: FontWeight.bold,
                color: AppColors.textLight,
              ),
              textAlign: TextAlign.center,
            ),
            SizedBox(height: 8.h),
            Text(
              achievement.description,
              style: TextStyle(
                fontSize: 14.sp,
                color: AppColors.textMedium,
              ),
              textAlign: TextAlign.center,
            ),
            SizedBox(height: 20.h),
            SizedBox(
              width: double.infinity,
              child: ElevatedButton(
                onPressed: () {
                  Navigator.pop(context);
                  // TODO: Share achievement
                },
                style: ElevatedButton.styleFrom(
                  backgroundColor: Colors.transparent,
                  shadowColor: Colors.transparent,
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(12.r),
                  ),
                ).copyWith(
                  backgroundColor: MaterialStateProperty.all(Colors.transparent),
                ),
                child: Ink(
                  decoration: BoxDecoration(
                    gradient: LinearGradient(colors: AppColors.socialGradient),
                    borderRadius: BorderRadius.circular(12.r),
                  ),
                  child: Container(
                    alignment: Alignment.center,
                    padding: EdgeInsets.symmetric(vertical: 12.h),
                    child: Text(
                      'Share Achievement',
                      style: TextStyle(
                        fontSize: 14.sp,
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
    );
  }

  void _showCategoryAchievements(AchievementCategory category) {
    // TODO: Show achievements for specific category
    print('Show ${category.displayName} achievements');
  }

  void _shareAchievements() {
    // TODO: Share overall progress
    print('Share achievements');
  }
}