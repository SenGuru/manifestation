import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:mistel/services/unified_auth_service.dart';
import 'package:mistel/services/unified_auth_service.dart';
import '../constants/colors.dart';
import '../constants/app_theme.dart';
import '../widgets/universal_back_button.dart';
import '../services/user_service.dart';
import '../services/usage_analytics_service.dart';
import '../services/theme_service.dart';
import '../models/analytics_models.dart';
import 'login_screen.dart';
import 'session_stats_screen.dart';
import 'account_settings_screen.dart';
import 'about_screen.dart';
import 'settings/security_settings_screen.dart';
import '../widgets/stylish_loading.dart';
import '../utils/navigation_utils.dart';

class ProfileScreen extends StatefulWidget {
  @override
  _ProfileScreenState createState() => _ProfileScreenState();
}

class _ProfileScreenState extends State<ProfileScreen> {
  String userName = 'Loading...';
  String userEmail = 'Loading...';
  UsageStats? _usageStats;
  bool _isLoading = true;
  int _daysActive = 0;
  int _meditationMinutes = 0;
  int _journalEntries = 0;
  int _goalsAchieved = 0;
  final ThemeService _themeService = ThemeService();

  @override
  void initState() {
    super.initState();
    _loadUserData();
    _themeService.addListener(_onThemeChanged);
  }

  @override
  void dispose() {
    _themeService.removeListener(_onThemeChanged);
    super.dispose();
  }

  void _onThemeChanged() {
    if (mounted) {
      setState(() {
        // Rebuild with new theme colors
      });
    }
  }

  Future<void> _loadUserData() async {
    try {
      // Load user info
      final fullName = await UserService.getFullName();
      final userData = await UserService.getUserData();
      final userNameStr = await UserService.getUserName();
      
      // Load usage analytics
      final analyticsService = UsageAnalyticsService();
      await analyticsService.initialize();
      final usageStats = await analyticsService.getUsageStats();
      
      // Calculate stats
      final meditationStats = usageStats.activities[ActivityType.visualMeditation];
      final affirmationStats = usageStats.activities[ActivityType.dailyAffirmations];
      
      setState(() {
        userName = fullName.isNotEmpty ? fullName : 'User';
        userEmail = userData?['model']?['email'] ?? userNameStr ?? 'user@example.com';
        _usageStats = usageStats;
        _daysActive = _calculateDaysActive(usageStats);
        _meditationMinutes = (meditationStats?.totalTimeInSeconds ?? 0) ~/ 60;
        _journalEntries = _getEstimatedJournalEntries(usageStats);
        _goalsAchieved = _getEstimatedGoalsAchieved(usageStats);
        _isLoading = false;
      });
    } catch (e) {
      print('Error loading user data: $e');
      setState(() {
        userName = 'User';
        userEmail = 'user@example.com';
        _isLoading = false;
      });
    }
  }

  int _calculateDaysActive(UsageStats stats) {
    // Calculate days active based on session history
    final sessionDates = <DateTime>{};
    
    for (final activity in stats.activities.values) {
      for (final dailyActivity in activity.weeklyActivity) {
        if (dailyActivity.sessionCount > 0) {
          sessionDates.add(DateTime(
            dailyActivity.date.year,
            dailyActivity.date.month,
            dailyActivity.date.day,
          ));
        }
      }
    }
    
    // Add some base days for demonstration
    return sessionDates.length + 40; // Base 40 days + active session days
  }

  int _getEstimatedJournalEntries(UsageStats stats) {
    // Estimate journal entries based on overall activity
    final totalSessions = stats.totalSessions;
    return (totalSessions * 0.3).round(); // Assume 30% of sessions involve journaling
  }

  int _getEstimatedGoalsAchieved(UsageStats stats) {
    // Estimate goals achieved based on streaks and activity
    final currentStreak = stats.currentStreak;
    final longestStreak = stats.longestStreak;
    return ((currentStreak + longestStreak) / 5).round(); // Rough estimation
  }

  @override
  Widget build(BuildContext context) {
    // Show loading screen while data is being loaded
    if (_isLoading) {
      return Scaffold(
        backgroundColor: AppTheme.backgroundPrimary,
        body: Center(
          child: StylishLoading(
            message: 'Loading your profile...',
          ),
        ),
      );
    }
    
    // Show actual profile content once loaded
    return Scaffold(
      backgroundColor: AppTheme.backgroundPrimary,
      body: SafeArea(
        child: Column(
          children: [
            // Header with back button
            _buildHeader(),
            
            // Profile info section
            _buildProfileInfo(),
            
            SizedBox(height: 20.h),
            
            // Menu sections
            _buildMenuSection(),
            
            Spacer(),
            
            // Logout button
            _buildLogoutButton(),
            
            SizedBox(height: 20.h),
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
                color: AppTheme.cardBackground,
                borderRadius: BorderRadius.circular(12.r),
                border: Border.all(
                  color: AppTheme.borderPrimary,
                ),
              ),
              child: Icon(
                Icons.arrow_back,
                color: AppTheme.textPrimary,
                size: 20.sp,
              ),
            ),
          ),
          SizedBox(width: 16.w),
          Text(
            'Profile',
            style: TextStyle(
              fontSize: 24.sp,
              fontWeight: FontWeight.bold,
              color: AppTheme.textPrimary,
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildProfileInfo() {
    return Container(
      margin: EdgeInsets.symmetric(horizontal: 20.w),
      padding: EdgeInsets.all(20.w),
      decoration: BoxDecoration(
        gradient: LinearGradient(
          colors: [
            AppTheme.affirmationGradient[0].withOpacity(0.1),
            AppTheme.affirmationGradient[1].withOpacity(0.05),
          ],
          begin: Alignment.topLeft,
          end: Alignment.bottomRight,
        ),
        borderRadius: BorderRadius.circular(24.r),
        border: Border.all(
          color: AppTheme.affirmationGradient[0].withOpacity(0.2),
        ),
      ),
      child: Row(
        children: [
          // Profile picture
          Container(
            width: 65.w,
            height: 65.h,
            decoration: BoxDecoration(
              gradient: LinearGradient(
                colors: AppTheme.affirmationGradient,
                begin: Alignment.topLeft,
                end: Alignment.bottomRight,
              ),
              shape: BoxShape.circle,
              boxShadow: [
                BoxShadow(
                  color: AppTheme.affirmationGradient[0].withOpacity(0.4),
                  blurRadius: 12.r,
                  offset: Offset(0, 4.h),
                ),
              ],
            ),
            child: Icon(
              Icons.person_rounded,
              color: AppTheme.white,
              size: 32.sp,
            ),
          ),
          
          SizedBox(width: 16.w),
          
          // Name and email
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  userName,
                  style: TextStyle(
                    fontSize: 20.sp,
                    fontWeight: FontWeight.bold,
                    color: AppTheme.textPrimary,
                  ),
                ),
                SizedBox(height: 4.h),
                Text(
                  userEmail,
                  style: TextStyle(
                    fontSize: 13.sp,
                    color: AppTheme.textSecondary,
                  ),
                ),
              ],
            ),
          ),
          
          // Edit button
          GestureDetector(
            onTap: () {
              pushWithoutSwipe(
                context,
                AccountSettingsScreen(),
              );
            },
            child: Container(
              padding: EdgeInsets.all(10.w),
              decoration: BoxDecoration(
                color: AppTheme.white.withOpacity(0.9),
                borderRadius: BorderRadius.circular(12.r),
                boxShadow: [
                  BoxShadow(
                    color: AppTheme.affirmationGradient[0].withOpacity(0.1),
                    blurRadius: 8.r,
                    offset: Offset(0, 2.h),
                  ),
                ],
              ),
              child: Icon(
                Icons.edit_rounded,
                color: AppTheme.affirmationGradient[0],
                size: 20.sp,
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildMenuSection() {
    return Container(
      margin: EdgeInsets.symmetric(horizontal: 20.w),
      decoration: BoxDecoration(
        color: AppTheme.cardBackground,
        borderRadius: BorderRadius.circular(20.r),
        border: Border.all(
          color: AppTheme.borderSecondary,
        ),
      ),
      child: Column(
        children: [
          _buildMenuItem(
            icon: Icons.analytics_outlined,
            title: 'Progress & Stats',
            subtitle: 'View your manifestation journey',
            onTap: () {
              pushWithoutSwipe(
                context,
                SessionStatsScreen(),
              );
            },
          ),
          
          _buildDivider(),
          
          _buildThemeToggleItem(),
          
          _buildDivider(),
          
          _buildMenuItem(
            icon: Icons.security_outlined,
            title: 'Security',
            subtitle: 'Password, authentication and privacy',
            onTap: () {
              pushWithoutSwipe(
                context,
                SecuritySettingsScreen(),
              );
            },
          ),
          
          _buildDivider(),
          
          _buildMenuItem(
            icon: Icons.info_outline,
            title: 'About',
            subtitle: 'Version, terms, privacy policy',
            onTap: () {
              pushWithoutSwipe(
                context,
                AboutScreen(),
              );
            },
          ),
        ],
      ),
    );
  }

  Widget _buildMenuItem({
    required IconData icon,
    required String title,
    required String subtitle,
    required VoidCallback onTap,
  }) {
    return Material(
      color: Colors.transparent,
      child: InkWell(
        onTap: onTap,
        borderRadius: BorderRadius.zero,
        child: Container(
          padding: EdgeInsets.symmetric(horizontal: 20.w, vertical: 12.h),
          child: Row(
            children: [
              Container(
                width: 42.w,
                height: 42.h,
                decoration: BoxDecoration(
                  gradient: LinearGradient(
                    colors: [
                      AppTheme.affirmationGradient[0].withOpacity(0.1),
                      AppTheme.affirmationGradient[1].withOpacity(0.05),
                    ],
                    begin: Alignment.topLeft,
                    end: Alignment.bottomRight,
                  ),
                  borderRadius: BorderRadius.circular(12.r),
                ),
                child: Icon(
                  icon,
                  color: AppTheme.affirmationGradient[0],
                  size: 22.sp,
                ),
              ),
              SizedBox(width: 16.w),
              Expanded(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      title,
                      style: TextStyle(
                        fontSize: 16.sp,
                        fontWeight: FontWeight.w600,
                        color: AppTheme.textPrimary,
                      ),
                    ),
                    SizedBox(height: 2.h),
                    Text(
                      subtitle,
                      style: TextStyle(
                        fontSize: 13.sp,
                        color: AppTheme.textSecondary,
                      ),
                    ),
                  ],
                ),
              ),
              Icon(
                Icons.arrow_forward_ios_rounded,
                color: AppTheme.textTertiary,
                size: 16.sp,
              ),
            ],
          ),
        ),
      ),
    );
  }

  Widget _buildDivider() {
    return Container(
      height: 1,
      margin: EdgeInsets.symmetric(horizontal: 20.w),
      color: AppTheme.divider,
    );
  }

  Widget _buildLogoutButton() {
    return Container(
      margin: EdgeInsets.symmetric(horizontal: 20.w),
      width: double.infinity,
      child: ElevatedButton(
        onPressed: _handleLogout,
        style: ElevatedButton.styleFrom(
          backgroundColor: Colors.red.withOpacity(0.1),
          shadowColor: Colors.transparent,
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(16.r),
            side: BorderSide(
              color: Colors.red.withOpacity(0.3),
            ),
          ),
          padding: EdgeInsets.symmetric(vertical: 16.h),
        ),
        child: Row(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Icon(
              Icons.logout,
              color: Colors.red,
              size: 20.sp,
            ),
            SizedBox(width: 12.w),
            Text(
              'Logout',
              style: TextStyle(
                fontSize: 16.sp,
                fontWeight: FontWeight.w600,
                color: Colors.red,
              ),
            ),
          ],
        ),
      ),
    );
  }

  void _handleLogout() {
    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        backgroundColor: AppTheme.cardBackground,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(16.r),
        ),
        title: Text(
          'Logout',
          style: TextStyle(
            color: AppTheme.textPrimary,
            fontSize: 20.sp,
            fontWeight: FontWeight.bold,
          ),
        ),
        content: Text(
          'Are you sure you want to logout?',
          style: TextStyle(
            color: AppTheme.textSecondary,
            fontSize: 16.sp,
          ),
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context),
            child: Text(
              'Cancel',
              style: TextStyle(
                color: AppTheme.textSecondary,
                fontSize: 16.sp,
              ),
            ),
          ),
          ElevatedButton(
            onPressed: () async {
              Navigator.pop(context); // Close dialog first
              
              // Show loading indicator
              showDialog(
                context: context,
                barrierDismissible: false,
                builder: (context) => AlertDialog(
                  backgroundColor: AppTheme.cardBackground,
                  content: Column(
                    mainAxisSize: MainAxisSize.min,
                    children: [
                      CompactStylishLoading(
                        color: AppTheme.affirmationGradient[0],
                        size: 40.w,
                      ),
                      SizedBox(height: 16.h),
                      Text(
                        'Logging out...',
                        style: TextStyle(
                          color: AppTheme.textPrimary,
                          fontSize: 16.sp,
                        ),
                      ),
                    ],
                  ),
                ),
              );
              
              try {
                print('Profile screen initiating logout...');
                
                // Use UnifiedAuthService to logout
                final authService = UnifiedAuthService();
                await authService.logout();
                
                print('Logout successful');
                
                // Close loading dialog
                Navigator.pop(context);
                
                // Show success message briefly
                ScaffoldMessenger.of(context).showSnackBar(
                  SnackBar(
                    content: Text('Successfully logged out'),
                    backgroundColor: Colors.green,
                    duration: Duration(seconds: 1),
                  ),
                );
                
                // Navigate to login screen and clear all previous routes
                Navigator.pushAndRemoveUntil(
                  context,
                  MaterialPageRoute(builder: (context) => LoginScreen()),
                  (route) => false,
                );
                
              } catch (e) {
                print('Profile screen logout error: $e');
                
                // Close loading dialog if still open
                Navigator.pop(context);
                
                // Show error but still attempt navigation
                ScaffoldMessenger.of(context).showSnackBar(
                  SnackBar(
                    content: Text('Logout error occurred, but you will be signed out'),
                    backgroundColor: Colors.red,
                    duration: Duration(seconds: 2),
                  ),
                );
                
                // Still navigate to login even if logout had errors
                Navigator.pushAndRemoveUntil(
                  context,
                  MaterialPageRoute(builder: (context) => LoginScreen()),
                  (route) => false,
                );
              }
            },
            style: ElevatedButton.styleFrom(
              backgroundColor: Colors.red,
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(12.r),
              ),
            ),
            child: Text(
              'Logout',
              style: TextStyle(
                color: AppTheme.white,
                fontSize: 16.sp,
                fontWeight: FontWeight.w600,
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildStatsSection() {
    return GestureDetector(
      onTap: () {
        pushWithoutSwipe(
          context,
          SessionStatsScreen(),
        );
      },
      child: Container(
      margin: EdgeInsets.symmetric(horizontal: 20.w),
      padding: EdgeInsets.all(20.w),
      decoration: BoxDecoration(
        gradient: LinearGradient(
          colors: [
            AppTheme.affirmationGradient[0].withOpacity(0.1),
            AppTheme.affirmationGradient[1].withOpacity(0.05),
          ],
        ),
        borderRadius: BorderRadius.circular(20.r),
        border: Border.all(
          color: AppTheme.affirmationGradient[0].withOpacity(0.2),
        ),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(
            'Your Journey',
            style: TextStyle(
              fontSize: 18.sp,
              fontWeight: FontWeight.w600,
              color: AppTheme.textPrimary,
            ),
          ),
          
          SizedBox(height: 16.h),
          
          Row(
            children: [
              Expanded(
                child: _buildStatItem(
                  _isLoading ? '...' : '$_daysActive',
                  'Days Active',
                  Icons.calendar_today_outlined,
                  AppTheme.affirmationGradient[0],
                ),
              ),
              Expanded(
                child: _buildStatItem(
                  _isLoading ? '...' : '${_meditationMinutes}m',
                  'Meditation',
                  Icons.self_improvement_outlined,
                  AppTheme.visualMeditationGradient[0],
                ),
              ),
              Expanded(
                child: _buildStatItem(
                  _isLoading ? '...' : '$_journalEntries',
                  'Journal Entries',
                  Icons.book_outlined,
                  AppTheme.journalGradient[0],
                ),
              ),
              Expanded(
                child: _buildStatItem(
                  _isLoading ? '...' : '$_goalsAchieved',
                  'Achievements',
                  Icons.flag_outlined,
                  AppTheme.goalTrackerGradient[0],
                ),
              ),
            ],
          ),
          
          SizedBox(height: 16.h),
          
          // Tap indicator
          Row(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              Icon(
                Icons.analytics_outlined,
                color: AppTheme.textTertiary,
                size: 16.sp,
              ),
              SizedBox(width: 8.w),
              Text(
                'Tap for detailed stats',
                style: TextStyle(
                  fontSize: 12.sp,
                  color: AppTheme.textTertiary,
                  fontStyle: FontStyle.italic,
                ),
              ),
              SizedBox(width: 8.w),
              Icon(
                Icons.arrow_forward_ios,
                color: AppTheme.textTertiary,
                size: 12.sp,
              ),
            ],
          ),
        ],
      ),
    )
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
            borderRadius: BorderRadius.circular(12.r),
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
            color: AppTheme.textPrimary,
          ),
        ),
        Text(
          label,
          style: TextStyle(
            fontSize: 11.sp,
            color: AppTheme.textSecondary,
          ),
          textAlign: TextAlign.center,
        ),
      ],
    );
  }

  Widget _buildThemeToggleItem() {
    return Material(
      color: Colors.transparent,
      child: InkWell(
        onTap: () async {
          await _themeService.toggleTheme(animate: false);
        },
        borderRadius: BorderRadius.zero,
        child: Container(
          padding: EdgeInsets.symmetric(horizontal: 20.w, vertical: 12.h),
          child: Row(
            children: [
              Container(
                width: 42.w,
                height: 42.h,
                decoration: BoxDecoration(
                  gradient: LinearGradient(
                    colors: [
                      AppTheme.affirmationGradient[0].withOpacity(0.1),
                      AppTheme.affirmationGradient[1].withOpacity(0.05),
                    ],
                    begin: Alignment.topLeft,
                    end: Alignment.bottomRight,
                  ),
                  borderRadius: BorderRadius.circular(12.r),
                ),
                child: Icon(
                  AppTheme.isDarkMode ? Icons.light_mode_outlined : Icons.dark_mode_outlined,
                  color: AppTheme.affirmationGradient[0],
                  size: 22.sp,
                ),
              ),
              SizedBox(width: 16.w),
              Expanded(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      'Appearance',
                      style: TextStyle(
                        fontSize: 16.sp,
                        fontWeight: FontWeight.w600,
                        color: AppTheme.textPrimary,
                      ),
                    ),
                    SizedBox(height: 2.h),
                    Text(
                      AppTheme.isDarkMode ? 'Dark Mode' : 'Light Mode',
                      style: TextStyle(
                        fontSize: 13.sp,
                        color: AppTheme.textSecondary,
                      ),
                    ),
                  ],
                ),
              ),
              Container(
                width: 50.w,
                height: 26.h,
                decoration: BoxDecoration(
                  borderRadius: BorderRadius.circular(13.r),
                  color: AppTheme.isDarkMode 
                      ? AppTheme.affirmationGradient[0].withOpacity(0.2)
                      : AppTheme.textTertiary.withOpacity(0.15),
                ),
                child: Stack(
                  children: [
                    AnimatedPositioned(
                      duration: Duration(milliseconds: 200),
                      curve: Curves.easeInOut,
                      left: AppTheme.isDarkMode ? 26.w : 2.w,
                      top: 2.h,
                      child: Container(
                        width: 22.w,
                        height: 22.h,
                        decoration: BoxDecoration(
                          shape: BoxShape.circle,
                          color: AppTheme.isDarkMode 
                              ? AppTheme.affirmationGradient[0]
                              : AppTheme.textTertiary.withOpacity(0.8),
                          boxShadow: [
                            BoxShadow(
                              color: AppTheme.shadow,
                              blurRadius: 4.r,
                              offset: Offset(0, 2.h),
                            ),
                          ],
                        ),
                        child: Icon(
                          AppTheme.isDarkMode ? Icons.nightlight_round : Icons.wb_sunny_rounded,
                          color: AppTheme.white,
                          size: 13.sp,
                        ),
                      ),
                    ),
                  ],
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}