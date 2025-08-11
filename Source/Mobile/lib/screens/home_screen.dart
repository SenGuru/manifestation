import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:mistel/screens/session_stats_screen.dart';  
import 'daily_affirmations.dart';
import '../components/daily_inspiration.dart';
import '../components/feature_card.dart';
import '../components/dual_feature_card.dart';
import 'meditation_screen_premium.dart';
import 'visual_meditation_screen.dart';
import '../components/wide_feature_card.dart';
import 'goal_tracker_screen.dart';
import 'manifestation_journal.dart';
import '../components/energy_meter.dart';
import 'vision_board_home.dart';
import 'social_feed_screen.dart';
import '../utils/navigation_utils.dart';
import '../constants/colors.dart';
import '../constants/app_theme.dart';
import 'login_screen.dart';
import 'profile_screen.dart';
import '../services/subscription_service.dart';
import '../components/premium_upgrade_dialog.dart';

class HomeScreen extends StatefulWidget {
  @override
  _HomeScreenState createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  int currentStreak = 7;
  String userName = 'Alex';

  @override
  void initState() {
    super.initState();
    _loadUserName();
    
    // Show premium upgrade dialog for non-premium users after a short delay
    WidgetsBinding.instance.addPostFrameCallback((_) {
      _checkAndShowPremiumUpgrade();
    });
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

  void _checkAndShowPremiumUpgrade() async {
    try {
      final subscriptionService = SubscriptionService();
      await subscriptionService.initialize();
      
      // Check if user is premium
      if (!subscriptionService.isPremium) {
        print('🎯 User is not premium, showing upgrade dialog');
        
        // Show the premium upgrade dialog after a short delay for better UX
        await Future.delayed(Duration(milliseconds: 1000));
        
        if (mounted) {
          showDialog(
            context: context,
            barrierDismissible: true,
            builder: (context) => PremiumUpgradeDialog(
              feature: 'app_launch',
              customMessage: 'Unlock all features and enhance your manifestation journey with Premium!',
              onUpgrade: () {
                _handlePremiumUpgrade();
              },
            ),
          );
        }
      } else {
        print('✅ User has premium access');
      }
    } catch (e) {
      print('❌ Error checking premium status: $e');
    }
  }

  void _handlePremiumUpgrade() {
    print('🚀 Premium upgrade requested - showing dialog');
    
    showDialog(
      context: context,
      builder: (context) => PremiumUpgradeDialog(
        feature: 'Premium Features',
        customMessage: 'Unlock all premium features to enhance your manifestation journey',
        onUpgrade: () {
          print('Premium upgrade completed');
        },
      ),
    );
  }
  
  Widget _buildTopSection() {
    return Row(
      children: [
        // Daily inspiration takes more space
        Expanded(
          flex: 3,
          child: MindfulnessMoment(),
        ),
        
        SizedBox(width: 12.w),
        
        // Energy meter takes less space
        Expanded(
          flex: 2,
          child: EnergyMeter(),
        ),
      ],
    );
  }
  
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppTheme.backgroundPrimary,
      body: SafeArea(
        child: Padding(
          padding: EdgeInsets.symmetric(horizontal: 24.w),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              SizedBox(height: 20.h),
              
              // Header
              _buildHeader(),
              
              SizedBox(height: 24.h),
              
              // Top section
              _buildTopSection(),
              
              SizedBox(height: 32.h),
              
              // Features grid
              Expanded(
                child: _buildFeaturesGrid(),
              ),
            ],
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
                  color: AppTheme.textSecondary,
                  fontWeight: FontWeight.w400,
                ),
              ),
              SizedBox(height: 8.h),
              Text(
                'Ready to Manifest?',
                style: TextStyle(
                  fontSize: 32.sp,
                  color: AppTheme.textPrimary,
                  fontWeight: FontWeight.bold,
                ),
              ),
            ],
          ),
        ),
        
        
        // Profile button
        GestureDetector(
          onTap: () {
            pushWithoutSwipe(
              context,
              ProfileScreen(),
            );
          },
          child: Container(
            width: 44.w,
            height: 44.h,
            decoration: BoxDecoration(
              gradient: LinearGradient(
                colors: AppTheme.affirmationGradient,
                begin: Alignment.topLeft,
                end: Alignment.bottomRight,
              ),
              shape: BoxShape.circle,
              boxShadow: [
                BoxShadow(
                  color: AppTheme.affirmationGradient[0].withValues(alpha: 0.3),
                  blurRadius: 8.r,
                  offset: Offset(0, 4.h),
                ),
              ],
            ),
            child: Icon(
              Icons.person,
              color: AppTheme.white,
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
            color: AppTheme.textPrimary,
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
                gradientColors: AppTheme.affirmationGradient,
                onTap: () => _navigateToFeature('affirmations'),
              ),
            ),
            SizedBox(width: 16.w),
            Expanded(
              child: FeatureCard(
                title: 'Vision Board',
                subtitle: 'Visualize Dreams',
                icon: Icons.dashboard,
                gradientColors: AppTheme.visionBoardGradient,
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
                leftGradientColors: AppTheme.journalGradient,
                leftOnTap: () => _navigateToFeature('journal'),
                rightTitle: 'Goal Tracker',
                rightSubtitle: 'Track Progress',
                rightIcon: Icons.track_changes,
                rightGradientColors: AppTheme.goalTrackerGradient,
                rightOnTap: () => _navigateToFeature('goal_tracker'),
              ),
            ),
            SizedBox(width: 16.w),
            Expanded(
              child: FeatureCard(
                title: 'Visual Meditation',
                subtitle: 'Guided Focus',
                icon: Icons.visibility,
                gradientColors: AppTheme.visualMeditationGradient,
                onTap: () => _navigateToFeature('candle_meditation'),
              ),
            ),
          ],
        ),
        
        SizedBox(height: 16.h),
        
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
        pushWithoutSwipe(
          context,
          DailyAffirmationsScreen(),
        );
        break;
      case 'vision_board':
        pushWithoutSwipe(
          context,
          VisionBoardHome(),
        );
        break;
      case 'journal':
        pushWithoutSwipe(
          context,
          ManifestationJournalScreen(),
        );
        break;
      case 'candle_meditation':
        pushWithoutSwipe(
          context,
          VisualMeditationScreen(),
        );
        break;
      case 'goal_tracker':
        pushWithoutSwipe(
          context,
          GoalTrackerScreen(),
        );
        break;
      case 'moon_phases':
        pushWithoutSwipe(
          context,
          PremiumMeditationScreen(),
        );
        break;
      case 'social':
        pushWithoutSwipe(
          context,
          SocialFeedScreen(),
        );
        break;
      case 'stats':
        pushWithoutSwipe(
          context,
          SessionStatsScreen(),
        );
        break;
      default:
        print('Unknown feature: $feature');
    }
  }

}