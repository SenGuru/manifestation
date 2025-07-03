import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:manifestation_app/daily_affirmations.dart';
import 'package:manifestation_app/daily_inspiration.dart';
import 'package:manifestation_app/feature_card.dart';
import 'package:manifestation_app/dual_feature_card.dart';
import 'package:manifestation_app/meditation_screen.dart';
import 'package:manifestation_app/wide_feature_card.dart';
import 'package:manifestation_app/goal_tracker_screen.dart';
import 'package:manifestation_app/manifestation_journal.dart';
import 'package:manifestation_app/streak_counter.dart';
import 'package:manifestation_app/vision_board_screen.dart';
import 'package:manifestation_app/social_feed_screen.dart'; // Add this import
import 'colors.dart';

class HomeScreen extends StatefulWidget {
  @override
  _HomeScreenState createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  int currentStreak = 7; // Example streak counter
  
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
    return Column(
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
                title: 'Candle Meditation',
                subtitle: 'Guided Peace',
                icon: Icons.local_fire_department,
                gradientColors: AppColors.candleMeditationGradient,
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
            builder: (context) => VisionBoardScreen(),
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
            builder: (context) => MeditationScreen(),
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
            builder: (context) => MeditationScreen(),
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
}