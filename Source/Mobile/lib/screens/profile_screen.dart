import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:mistel/services/unified_auth_service.dart';
import '../constants/colors.dart';
import '../constants/app_theme.dart';
import '../widgets/universal_back_button.dart';
import '../services/user_service.dart';
import '../services/theme_service.dart';
import 'login_screen.dart';
import 'session_stats_screen.dart';
import 'account_settings_screen.dart';
import 'about_screen.dart';
import 'settings/security_settings_screen.dart';
import '../widgets/stylish_loading.dart';
import '../widgets/user_profile_picture.dart';
import '../utils/navigation_utils.dart';

class ProfileScreen extends StatefulWidget {
  final String? cachedProfileImageUrl;

  const ProfileScreen({
    Key? key,
    this.cachedProfileImageUrl,
  }) : super(key: key);

  @override
  _ProfileScreenState createState() => _ProfileScreenState();
}

class _ProfileScreenState extends State<ProfileScreen> {
  String userName = 'Loading...';
  String userEmail = 'Loading...';
  bool _isLoading = true;
  final ThemeService _themeService = ThemeService();
  String? _cachedProfileImageUrl;

  @override
  void initState() {
    super.initState();
    _cachedProfileImageUrl = widget.cachedProfileImageUrl;
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
      // Get user data from unified auth service for consistency
      final authService = UnifiedAuthService();
      final userData = await authService.getUserData();
      
      String fullName = 'User';
      String email = 'user@example.com';
      
      if (userData != null) {
        final firstName = userData['firstName'] ?? '';
        final lastName = userData['lastName'] ?? '';
        
        // Get email from userData 
        if (userData['email'] != null && userData['email'].isNotEmpty) {
          email = userData['email'];
        } else {
          // Try to find email in nested model structure
          final model = userData['model'];
          if (model != null && model['email'] != null) {
            email = model['email'];
          } else {
            // Last resort: try to get email directly from storage
            final storedEmail = await authService.getUserEmail();
            if (storedEmail != null && storedEmail.isNotEmpty) {
              email = storedEmail;
            }
          }
        }
        
        // Construct full name
        if (firstName.isNotEmpty || lastName.isNotEmpty) {
          fullName = '$firstName $lastName'.trim();
          if (fullName.isEmpty) fullName = 'User';
        }
        
        print('📋 Profile - Loaded user data:');
        print('  - fullName: $fullName');
        print('  - email: $email');
      }
      
      if (mounted) {
        setState(() {
          userName = fullName;
          userEmail = email;
          _isLoading = false;
        });
      }
    } catch (e) {
      print('Error loading user data: $e');
      if (mounted) {
        setState(() {
          userName = 'User';
          userEmail = 'user@example.com';
          _isLoading = false;
        });
      }
    }
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
        child: SingleChildScrollView(
          child: Column(
            children: [
              // Header with back button
              _buildHeader(),
              
              // Profile info section
              _buildProfileInfo(),
              
              SizedBox(height: 20.h),
              
              // Menu sections with connected spacer
              _buildMenuSectionWithSpacer(),
              
              SizedBox(height: 20.h),
              
              // Logout button
              _buildLogoutButton(),
              
              SizedBox(height: 20.h),
            ],
          ),
        ),
      ),
    );
  }

  Widget _buildHeader() {
    return Padding(
      padding: EdgeInsets.all(20.w),
      child: Row(
        children: [
          UniversalBackButton(),
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
          // Profile picture using consistent UserProfilePicture widget
          UserProfilePicture(
            size: 65.0,
            borderColor: AppTheme.affirmationGradient[0],
            borderWidth: 0.0,
            showBorder: false,
            cachedImageUrl: _cachedProfileImageUrl,
            onImageLoaded: (String? imageUrl) {
              // Update cached image URL if it loads successfully
              if (imageUrl != null) {
                setState(() {
                  _cachedProfileImageUrl = imageUrl;
                });
              }
            },
            onTap: () async {
              HapticFeedback.lightImpact();
              final result = await pushWithoutSwipe(
                context,
                AccountSettingsScreen(cachedProfileImageUrl: _cachedProfileImageUrl),
              );
              // Update cached image URL if one was returned
              if (result != null && result is String) {
                setState(() {
                  _cachedProfileImageUrl = result;
                });
              }
              // Refresh user data when returning from account settings
              _loadUserData();
            },
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
            onTap: () async {
              HapticFeedback.lightImpact();
              final result = await pushWithoutSwipe(
                context,
                AccountSettingsScreen(cachedProfileImageUrl: _cachedProfileImageUrl),
              );
              // Update cached image URL if one was returned
              if (result != null && result is String) {
                setState(() {
                  _cachedProfileImageUrl = result;
                });
              }
              // Refresh user data when returning from account settings
              _loadUserData();
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

  Widget _buildMenuSectionWithSpacer() {
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
          
          _buildDivider(),
          
          // Connected empty spacer section
          Container(
            height: 40.h,
            width: double.infinity,
            // No additional decoration - inherits from parent container
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
        onTap: () {
          HapticFeedback.selectionClick();
          onTap();
        },
        borderRadius: BorderRadius.zero,
        child: Container(
          padding: EdgeInsets.symmetric(horizontal: 20.w, vertical: 16.h),
          child: Row(
            children: [
              Container(
                width: 48.w,
                height: 48.h,
                decoration: BoxDecoration(
                  gradient: LinearGradient(
                    colors: [
                      AppTheme.affirmationGradient[0].withOpacity(0.1),
                      AppTheme.affirmationGradient[1].withOpacity(0.05),
                    ],
                    begin: Alignment.topLeft,
                    end: Alignment.bottomRight,
                  ),
                  borderRadius: BorderRadius.circular(14.r),
                ),
                child: Icon(
                  icon,
                  color: AppTheme.affirmationGradient[0],
                  size: 24.sp,
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
        onPressed: () {
          HapticFeedback.mediumImpact();
          _handleLogout();
        },
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
            onPressed: () {
              HapticFeedback.lightImpact();
              Navigator.pop(context);
            },
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
              HapticFeedback.mediumImpact();
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


  Widget _buildThemeToggleItem() {
    return Material(
      color: Colors.transparent,
      child: InkWell(
        onTap: () async {
          HapticFeedback.selectionClick();
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