import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../constants/colors.dart';
import 'login_screen.dart';

class ProfileScreen extends StatefulWidget {
  @override
  _ProfileScreenState createState() => _ProfileScreenState();
}

class _ProfileScreenState extends State<ProfileScreen> {
  String userName = 'Alex Johnson';
  String userEmail = 'alex.johnson@example.com';

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.backgroundDark,
      body: SafeArea(
        child: SingleChildScrollView(
          physics: BouncingScrollPhysics(),
          child: Column(
            children: [
              // Header with back button
              _buildHeader(),
              
              // Profile info section
              _buildProfileInfo(),
              
              SizedBox(height: 30.h),
              
              // Menu sections
              _buildMenuSection(),
              
              SizedBox(height: 30.h),
              
              // Logout button
              _buildLogoutButton(),
              
              SizedBox(height: 40.h),
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
          GestureDetector(
            onTap: () => Navigator.pop(context),
            child: Container(
              width: 44.w,
              height: 44.h,
              decoration: BoxDecoration(
                color: AppColors.cardBackground,
                borderRadius: BorderRadius.circular(12.r),
                border: Border.all(
                  color: AppColors.textMedium.withOpacity(0.2),
                ),
              ),
              child: Icon(
                Icons.arrow_back,
                color: AppColors.textLight,
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
              color: AppColors.textLight,
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildProfileInfo() {
    return Container(
      margin: EdgeInsets.symmetric(horizontal: 20.w),
      padding: EdgeInsets.all(24.w),
      decoration: BoxDecoration(
        color: AppColors.cardBackground,
        borderRadius: BorderRadius.circular(20.r),
        border: Border.all(
          color: AppColors.textMedium.withOpacity(0.1),
        ),
      ),
      child: Column(
        children: [
          // Profile picture
          Container(
            width: 100.w,
            height: 100.h,
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
                  blurRadius: 20.r,
                  offset: Offset(0, 10.h),
                ),
              ],
            ),
            child: Icon(
              Icons.person,
              color: AppColors.white,
              size: 50.sp,
            ),
          ),
          
          SizedBox(height: 20.h),
          
          // Name
          Text(
            userName,
            style: TextStyle(
              fontSize: 28.sp,
              fontWeight: FontWeight.bold,
              color: AppColors.textLight,
            ),
          ),
          
          SizedBox(height: 8.h),
          
          // Email
          Text(
            userEmail,
            style: TextStyle(
              fontSize: 16.sp,
              color: AppColors.textMedium,
            ),
          ),
          
          SizedBox(height: 20.h),
          
          // Edit profile button
          GestureDetector(
            onTap: () {
              // TODO: Navigate to edit profile
            },
            child: Container(
              padding: EdgeInsets.symmetric(horizontal: 24.w, vertical: 12.h),
              decoration: BoxDecoration(
                border: Border.all(
                  color: AppColors.affirmationGradient[0],
                  width: 2,
                ),
                borderRadius: BorderRadius.circular(25.r),
              ),
              child: Text(
                'Edit Profile',
                style: TextStyle(
                  fontSize: 16.sp,
                  fontWeight: FontWeight.w600,
                  color: AppColors.affirmationGradient[0],
                ),
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
        color: AppColors.cardBackground,
        borderRadius: BorderRadius.circular(20.r),
        border: Border.all(
          color: AppColors.textMedium.withOpacity(0.1),
        ),
      ),
      child: Column(
        children: [
          _buildMenuItem(
            icon: Icons.person_outline,
            title: 'Account Settings',
            subtitle: 'Privacy, security, change password',
            onTap: () {
              // TODO: Navigate to account settings
            },
          ),
          
          _buildDivider(),
          
          _buildMenuItem(
            icon: Icons.notifications_outlined,
            title: 'Notifications',
            subtitle: 'Push notifications, reminders',
            onTap: () {
              // TODO: Navigate to notifications
            },
          ),
          
          _buildDivider(),
          
          _buildMenuItem(
            icon: Icons.palette_outlined,
            title: 'Appearance',
            subtitle: 'Theme, display preferences',
            onTap: () {
              // TODO: Navigate to appearance settings
            },
          ),
          
          _buildDivider(),
          
          _buildMenuItem(
            icon: Icons.analytics_outlined,
            title: 'Progress & Stats',
            subtitle: 'View your manifestation journey',
            onTap: () {
              // TODO: Navigate to stats
            },
          ),
          
          _buildDivider(),
          
          _buildMenuItem(
            icon: Icons.backup_outlined,
            title: 'Backup & Sync',
            subtitle: 'Cloud backup, data export',
            onTap: () {
              // TODO: Navigate to backup settings
            },
          ),
          
          _buildDivider(),
          
          _buildMenuItem(
            icon: Icons.help_outline,
            title: 'Help & Support',
            subtitle: 'FAQ, contact support',
            onTap: () {
              // TODO: Navigate to help
            },
          ),
          
          _buildDivider(),
          
          _buildMenuItem(
            icon: Icons.info_outline,
            title: 'About',
            subtitle: 'Version, terms, privacy policy',
            onTap: () {
              // TODO: Navigate to about
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
    return ListTile(
      onTap: onTap,
      contentPadding: EdgeInsets.symmetric(horizontal: 20.w, vertical: 8.h),
      leading: Container(
        width: 44.w,
        height: 44.h,
        decoration: BoxDecoration(
          color: AppColors.backgroundDark,
          borderRadius: BorderRadius.circular(12.r),
        ),
        child: Icon(
          icon,
          color: AppColors.textMedium,
          size: 22.sp,
        ),
      ),
      title: Text(
        title,
        style: TextStyle(
          fontSize: 16.sp,
          fontWeight: FontWeight.w600,
          color: AppColors.textLight,
        ),
      ),
      subtitle: Text(
        subtitle,
        style: TextStyle(
          fontSize: 14.sp,
          color: AppColors.textMedium,
        ),
      ),
      trailing: Icon(
        Icons.chevron_right,
        color: AppColors.textMedium,
        size: 20.sp,
      ),
    );
  }

  Widget _buildDivider() {
    return Container(
      height: 1,
      margin: EdgeInsets.symmetric(horizontal: 20.w),
      color: AppColors.textMedium.withOpacity(0.1),
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
        backgroundColor: AppColors.cardBackground,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(16.r),
        ),
        title: Text(
          'Logout',
          style: TextStyle(
            color: AppColors.textLight,
            fontSize: 20.sp,
            fontWeight: FontWeight.bold,
          ),
        ),
        content: Text(
          'Are you sure you want to logout?',
          style: TextStyle(
            color: AppColors.textMedium,
            fontSize: 16.sp,
          ),
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context),
            child: Text(
              'Cancel',
              style: TextStyle(
                color: AppColors.textMedium,
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
                  backgroundColor: AppColors.cardBackground,
                  content: Column(
                    mainAxisSize: MainAxisSize.min,
                    children: [
                      CircularProgressIndicator(
                        valueColor: AlwaysStoppedAnimation<Color>(AppColors.affirmationGradient[0]),
                      ),
                      SizedBox(height: 16.h),
                      Text(
                        'Logging out...',
                        style: TextStyle(
                          color: AppColors.textLight,
                          fontSize: 16.sp,
                        ),
                      ),
                    ],
                  ),
                ),
              );
              
              try {
                print('Profile screen initiating logout...');
                
                // Simulate logout delay
                await Future.delayed(Duration(seconds: 1));
                
                // Hardcoded success result
                final logoutResult = {'success': true, 'message': 'Logout successful'};
                
                print('Logout result: $logoutResult');
                
                // Close loading dialog
                Navigator.pop(context);
                
                if (logoutResult['success'] == true) {
                  // Show success message briefly
                  ScaffoldMessenger.of(context).showSnackBar(
                    SnackBar(
                      content: Text('Successfully logged out'),
                      backgroundColor: Colors.green,
                      duration: Duration(seconds: 1),
                    ),
                  );
                } else {
                  // Show warning but still proceed with navigation
                  ScaffoldMessenger.of(context).showSnackBar(
                    SnackBar(
                      content: Text((logoutResult['message'] as String?) ?? 'Logout completed with warnings'),
                      backgroundColor: Colors.orange,
                      duration: Duration(seconds: 2),
                    ),
                  );
                }
                
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
                color: AppColors.white,
                fontSize: 16.sp,
                fontWeight: FontWeight.w600,
              ),
            ),
          ),
        ],
      ),
    );
  }
}