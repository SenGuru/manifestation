import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:manifestation_app/components/daily_affirmations.dart';
import 'package:manifestation_app/components/daily_inspiration.dart';
import 'package:manifestation_app/components/feature_card.dart';
import 'package:manifestation_app/components/dual_feature_card.dart';
import 'package:manifestation_app/components/wide_feature_card.dart';
import 'package:manifestation_app/components/streak_counter.dart';
import 'package:manifestation_app/constants/colors.dart';  
import 'package:manifestation_app/screens/affirmations.dart';
import 'package:manifestation_app/screens/meditation_screen.dart';
import 'package:manifestation_app/screens/manifestation_journal.dart';
import 'package:manifestation_app/screens/goal_tracker_screen.dart';
import 'package:manifestation_app/screens/social_feed_screen.dart';
import 'package:manifestation_app/screens/login_screen.dart';
import 'package:manifestation_app/services/auth_service.dart';
import 'package:manifestation_app/screens/vision_board_home.dart';
import 'package:manifestation_app/screens/stats_screen.dart';

class HomeScreen extends StatefulWidget {
  @override
  _HomeScreenState createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  int currentStreak = 7; // Example streak counter
  final _authService = AuthService();
  
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
      mainAxisAlignment: MainAxisAlignment.spaceBetween,
      children: [
        Expanded(
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(
                'Good ${_getGreeting()}',
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
        // Stats and Logout buttons
        Row(
          children: [
            // Stats button
            GestureDetector(
              onTap: () => Navigator.push(
                context,
                MaterialPageRoute(builder: (context) => StatsScreen()),
              ),
              child: Container(
                width: 44.w,
                height: 44.h,
                decoration: BoxDecoration(
                  gradient: LinearGradient(
                    colors: AppColors.visualMeditationGradient,
                  ),
                  borderRadius: BorderRadius.circular(12.r),
                ),
                child: Icon(
                  Icons.bar_chart,
                  color: AppColors.white,
                  size: 20.sp,
                ),
              ),
            ),
            SizedBox(width: 12.w),
            // Logout button
            GestureDetector(
              onTap: _showLogoutDialog,
              child: Container(
                width: 44.w,
                height: 44.h,
                decoration: BoxDecoration(
                  color: AppColors.cardBackground,
                  borderRadius: BorderRadius.circular(12.r),
                ),
                child: Icon(
                  Icons.logout,
                  color: AppColors.textMedium,
                  size: 20.sp,
                ),
              ),
            ),
          ],
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
        
        // Fourth row - Moon Phases (you can add this later)
        // Row(
        //   children: [
        //     Expanded(
        //       child: FeatureCard(
        //         title: 'Moon Phases',
        //         subtitle: 'Lunar Energy',
        //         icon: Icons.brightness_3,
        //         gradientColors: AppColors.moonPhaseGradient,
        //         onTap: () => _navigateToFeature('moon_phases'),
        //       ),
        //     ),
        //   ],
        // ),
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
            builder: (context) => VisualMeditationScreen(),
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
      default:
        print('Unknown feature: $feature');
    }
  }

  void _showLogoutDialog() {
    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        backgroundColor: AppColors.cardBackground,
        title: Text(
          'Logout',
          style: TextStyle(color: AppColors.textLight),
        ),
        content: Text(
          'Are you sure you want to logout?',
          style: TextStyle(color: AppColors.textMedium),
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context),
            child: Text(
              'Cancel',
              style: TextStyle(color: AppColors.textMedium),
            ),
          ),
          TextButton(
            onPressed: _handleLogout,
            child: Text(
              'Logout',
              style: TextStyle(color: Colors.red),
            ),
          ),
        ],
      ),
    );
  }

  void _handleLogout() async {
    try {
      // Close the dialog first
      Navigator.pop(context);
      
      // Perform logout
      await _authService.logout();
      
      // Navigate back to login screen and clear the stack
      Navigator.pushAndRemoveUntil(
        context,
        MaterialPageRoute(builder: (context) => LoginScreen()),
        (route) => false,
      );
    } catch (e) {
      print('Logout error: $e');
      // Show error if needed
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Logout failed. Please try again.'),
          backgroundColor: Colors.red,
        ),
      );
    }
  }
}