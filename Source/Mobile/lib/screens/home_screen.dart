import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';  
import 'daily_affirmations.dart';
import '../components/daily_inspiration.dart';
import '../components/feature_card.dart';
import '../components/dual_feature_card.dart';
import 'meditation_screen_premium.dart';
import 'visual_meditation_screen.dart';
import '../components/wide_feature_card.dart';
import 'goal_tracker_screen.dart';
import 'manifestation_journal.dart';
import '../components/streak_counter.dart';
import 'vision_board_home.dart';
import 'social_feed_screen.dart';
import '../constants/colors.dart';
import 'login_screen.dart';
import 'profile_screen.dart';
import 'session_stats_screen.dart';

class HomeScreen extends StatefulWidget {
  @override
  _HomeScreenState createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  int currentStreak = 7; // Example streak counter
  String userName = 'User'; // Default name, will be updated from user data

  @override
  void initState() {
    super.initState();
    _loadUserName();
  }

  void _loadUserName() async {
    try {
      // Hardcoded user name placeholder
      final firstName = 'Alex';
      setState(() {
        userName = firstName.isNotEmpty ? firstName : 'User';
      });
      print('Loaded user name: $userName');
    } catch (e) {
      print('Error loading user name: $e');
      setState(() {
        userName = 'User';
      });
    }
  }
  
  Widget _buildTopSection() {
    return Row(
      children: [
        // Daily inspiration takes more space
        Expanded(
          flex: 3,
          child: DailyInspiration(),
        ),
        
        SizedBox(width: 12.w),
        
        // Streak counter takes less space
        Expanded(
          flex: 2,
          child: StreakCounter(streakDays: currentStreak),
        ),
      ],
    );
  }
  
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.backgroundDark,
      body: SafeArea(
        child: SingleChildScrollView(
          physics: BouncingScrollPhysics(),
          child: Padding(
            padding: EdgeInsets.symmetric(horizontal: 20.w),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                SizedBox(height: 20.h),
                
                // Header section
                _buildHeader(),
                
                SizedBox(height: 20.h),
                
                // Top section with daily inspiration and streak counter
                _buildTopSection(),
                
                SizedBox(height: 25.h),
                
                // Features grid
                _buildFeaturesGrid(),
                
              ],
            ),
          ),
        ),
      ),
    );
  }
  
  Widget _buildHeader() {
    return Row(
      children: [
        Expanded(
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(
                'Good ${_getGreeting()}, $userName',
                style: TextStyle(
                  fontSize: 16.sp,
                  color: AppColors.textMedium,
                  fontWeight: FontWeight.w400,
                ),
              ),
              SizedBox(height: 8.h),
              Text(
                'Ready to Manifest?',
                style: TextStyle(
                  fontSize: 32.sp,
                  color: AppColors.textLight,
                  fontWeight: FontWeight.bold,
                ),
              ),
            ],
          ),
        ),
        
        // Stats button
        GestureDetector(
          onTap: () {
            Navigator.push(
              context,
              MaterialPageRoute(builder: (context) => SessionStatsScreen()),
            );
          },
          child: Container(
            width: 44.w,
            height: 44.h,
            decoration: BoxDecoration(
              gradient: LinearGradient(
                colors: AppColors.analyticsGradient,
                begin: Alignment.topLeft,
                end: Alignment.bottomRight,
              ),
              shape: BoxShape.circle,
              boxShadow: [
                BoxShadow(
                  color: AppColors.analyticsGradient[0].withOpacity(0.3),
                  blurRadius: 8.r,
                  offset: Offset(0, 4.h),
                ),
              ],
            ),
            child: Icon(
              Icons.analytics,
              color: AppColors.white,
              size: 20.sp,
            ),
          ),
        ),
        
        SizedBox(width: 12.w),
        
        // Profile button
        GestureDetector(
          onTap: () {
            Navigator.push(
              context,
              MaterialPageRoute(builder: (context) => ProfileScreen()),
            );
          },
          child: Container(
            width: 44.w,
            height: 44.h,
            decoration: BoxDecoration(
              gradient: LinearGradient(
                colors: AppColors.affirmationGradient,
                begin: Alignment.topLeft,
                end: Alignment.bottomRight,
              ),
              shape: BoxShape.circle,
              boxShadow: [
                BoxShadow(
                  color: AppColors.affirmationGradient[0].withOpacity(0.3),
                  blurRadius: 8.r,
                  offset: Offset(0, 4.h),
                ),
              ],
            ),
            child: Icon(
              Icons.person,
              color: AppColors.white,
              size: 24.sp,
            ),
          ),
        ),
      ],
    );
  }
  
  Widget _buildFeaturesGrid() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Your Practice',
          style: TextStyle(
            fontSize: 20.sp,
            color: AppColors.textLight,
            fontWeight: FontWeight.w600,
          ),
        ),
        SizedBox(height: 20.h),
        
        // First row - Daily Affirmations & Vision Board (separate cards)
        Row(
          children: [
            Expanded(
              child: FeatureCard(
                title: 'Daily Affirmations',
                subtitle: 'Power Words',
                icon: Icons.favorite,
                gradientColors: AppColors.affirmationGradient,
                onTap: () => _navigateToFeature('affirmations'),
              ),
            ),
            SizedBox(width: 16.w),
            Expanded(
              child: FeatureCard(
                title: 'Vision Board',
                subtitle: 'Visualize Dreams',
                icon: Icons.dashboard,
                gradientColors: AppColors.visionBoardGradient,
                onTap: () => _navigateToFeature('vision_board'),
              ),
            ),
          ],
        ),
        
        SizedBox(height: 16.h),
        
        // Second row - Journal (dual card) & Candle Meditation (single card)
        Row(
          children: [
            Expanded(
              child: DualFeatureCard(
                leftTitle: 'Journal',
                leftSubtitle: 'Reflect & Record',
                leftIcon: Icons.book,
                leftGradientColors: AppColors.journalGradient,
                leftOnTap: () => _navigateToFeature('journal'),
                rightTitle: 'Goal Tracker',
                rightSubtitle: 'Track Progress',
                rightIcon: Icons.track_changes,
                rightGradientColors: AppColors.goalTrackerGradient,
                rightOnTap: () => _navigateToFeature('goal_tracker'),
              ),
            ),
            SizedBox(width: 16.w),
            Expanded(
              child: FeatureCard(
                title: 'Visual Meditation',
                subtitle: 'Guided Focus',
                icon: Icons.visibility,
                gradientColors: AppColors.visualMeditationGradient,
                onTap: () => _navigateToFeature('candle_meditation'),
              ),
            ),
          ],
        ),
        
        SizedBox(height: 16.h),
        
        // Third row - Social Feature (wide card)
        WideFeatureCard(
          title: 'Community',
          subtitle: 'Connect with fellow manifesters & share your journey',
          icon: Icons.people,
          gradientColors: AppColors.socialGradient,
          onTap: () => _navigateToFeature('social'),
        ),
        
        SizedBox(height: 16.h),
        
        // Fourth row - Stats & Analytics
        Row(
          children: [
          ],
        ),
      ],
    );
  }
  
  String _getGreeting() {
    final hour = DateTime.now().hour;
    if (hour < 12) {
      return 'Morning';
    } else if (hour < 17) {
      return 'Afternoon';
    } else {
      return 'Evening';
    }
  }
  
  void _navigateToFeature(String feature) {
    switch (feature) {
      case 'affirmations':
        Navigator.push(
          context,
          MaterialPageRoute(
            builder: (context) => DailyAffirmationsScreen(),
          ),
        );
        break;
      case 'vision_board':
        Navigator.push(
          context,
          MaterialPageRoute(
            builder: (context) => VisionBoardHome(),
          ),
        );
        break;
      case 'journal':
        Navigator.push(
          context,
          MaterialPageRoute(
            builder: (context) => ManifestationJournalScreen(),
          ),
        );
        break;
      case 'candle_meditation':
        Navigator.push(
          context,
          MaterialPageRoute(
            builder: (context) => VisualMeditationScreen(),
          ),
        );
        break;
      case 'goal_tracker':
        Navigator.push(
          context,
          MaterialPageRoute(
            builder: (context) => GoalTrackerScreen(),
          ),
        );
        break;
      case 'moon_phases':
        Navigator.push(
          context,
          MaterialPageRoute(
            builder: (context) => PremiumMeditationScreen(),
          ),
        );
        break;
      case 'social':
        Navigator.push(
          context,
          MaterialPageRoute(
            builder: (context) => SocialFeedScreen(),
          ),
        );
        break;
      case 'stats':
        Navigator.push(
          context,
          MaterialPageRoute(
            builder: (context) => SessionStatsScreen(),
          ),
        );
        break;
      default:
        print('Unknown feature: $feature');
    }
  }

}