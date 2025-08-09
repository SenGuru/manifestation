import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:package_info_plus/package_info_plus.dart';
import '../constants/app_theme.dart';
import '../widgets/universal_back_button.dart';

class AboutScreen extends StatefulWidget {
  @override
  _AboutScreenState createState() => _AboutScreenState();
}

class _AboutScreenState extends State<AboutScreen> with TickerProviderStateMixin {
  late AnimationController _fadeController;
  late AnimationController _slideController;
  late Animation<double> _fadeAnimation;
  late Animation<Offset> _slideAnimation;
  
  String _appVersion = '';
  String _buildNumber = '';
  bool _isLoading = true;

  @override
  void initState() {
    super.initState();
    _initializeAnimations();
    _loadAppInfo();
  }

  void _initializeAnimations() {
    _fadeController = AnimationController(
      duration: Duration(milliseconds: 800),
      vsync: this,
    );
    
    _slideController = AnimationController(
      duration: Duration(milliseconds: 1000),
      vsync: this,
    );
    
    _fadeAnimation = Tween<double>(
      begin: 0.0,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _fadeController,
      curve: Curves.easeOut,
    ));
    
    _slideAnimation = Tween<Offset>(
      begin: Offset(0, 0.3),
      end: Offset.zero,
    ).animate(CurvedAnimation(
      parent: _slideController,
      curve: Curves.easeOutCubic,
    ));
    
    // Start animations with stagger
    Future.delayed(Duration(milliseconds: 200), () {
      if (mounted) {
        _fadeController.forward();
        _slideController.forward();
      }
    });
  }

  Future<void> _loadAppInfo() async {
    try {
      final packageInfo = await PackageInfo.fromPlatform();
      setState(() {
        _appVersion = packageInfo.version;
        _buildNumber = packageInfo.buildNumber;
        _isLoading = false;
      });
    } catch (e) {
      setState(() {
        _appVersion = '1.0.0';
        _buildNumber = '1';
        _isLoading = false;
      });
    }
  }

  @override
  void dispose() {
    _fadeController.dispose();
    _slideController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppTheme.backgroundDark,
      body: SafeArea(
        child: SingleChildScrollView(
          physics: BouncingScrollPhysics(),
          padding: EdgeInsets.symmetric(horizontal: 24.w),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              SizedBox(height: 16.h),
              _buildHeader(),
              SizedBox(height: 40.h),
              FadeTransition(
                opacity: _fadeAnimation,
                child: SlideTransition(
                  position: _slideAnimation,
                  child: Column(
                    children: [
                      _buildHeroSection(),
                      SizedBox(height: 48.h),
                      _buildInfoSection(),
                      SizedBox(height: 32.h),
                      _buildTeamSection(),
                      SizedBox(height: 32.h),
                      _buildLegalSection(),
                      SizedBox(height: 40.h),
                    ],
                  ),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }

  Widget _buildHeader() {
    return Row(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        UniversalBackButton(),
        SizedBox(width: 16.w),
        Expanded(
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(
                'About',
                style: TextStyle(
                  fontSize: 28.sp,
                  fontWeight: FontWeight.bold,
                  color: AppTheme.textLight,
                ),
              ),
              SizedBox(height: 4.h),
              Text(
                'Learn more about this app',
                style: TextStyle(
                  fontSize: 14.sp,
                  color: AppTheme.textMedium,
                  height: 1.3,
                ),
              ),
            ],
          ),
        ),
      ],
    );
  }

  Widget _buildHeroSection() {
    return Container(
      width: double.infinity,
      padding: EdgeInsets.all(32.w),
      decoration: BoxDecoration(
        gradient: LinearGradient(
          colors: [
            AppTheme.socialGradient[0],
            AppTheme.socialGradient[1],
          ],
          begin: Alignment.topLeft,
          end: Alignment.bottomRight,
        ),
        borderRadius: BorderRadius.circular(24.r),
        boxShadow: [
          BoxShadow(
            color: AppTheme.socialGradient[0].withOpacity(0.3),
            blurRadius: 20.r,
            offset: Offset(0, 10.h),
          ),
        ],
      ),
      child: Column(
        children: [
          // App Icon
          Container(
            width: 80.w,
            height: 80.h,
            decoration: BoxDecoration(
              color: AppTheme.white.withOpacity(0.2),
              borderRadius: BorderRadius.circular(20.r),
              border: Border.all(
                color: AppTheme.white.withOpacity(0.3),
                width: 2,
              ),
            ),
            child: Icon(
              Icons.auto_awesome_rounded,
              size: 40.sp,
              color: AppTheme.white,
            ),
          ),
          
          SizedBox(height: 24.h),
          
          // App Name
          Text(
            'Manifestation',
            style: TextStyle(
              fontSize: 24.sp,
              fontWeight: FontWeight.bold,
              color: AppTheme.white,
              letterSpacing: 0.5,
            ),
          ),
          
          SizedBox(height: 8.h),
          
          // Version Info
          if (!_isLoading)
            Container(
              padding: EdgeInsets.symmetric(horizontal: 12.w, vertical: 6.h),
              decoration: BoxDecoration(
                color: AppTheme.white.withOpacity(0.2),
                borderRadius: BorderRadius.circular(20.r),
              ),
              child: Text(
                'Version $_appVersion ($_buildNumber)',
                style: TextStyle(
                  fontSize: 12.sp,
                  color: AppTheme.white.withOpacity(0.9),
                  fontWeight: FontWeight.w500,
                ),
              ),
            ),
          
          SizedBox(height: 16.h),
          
          // Tagline
          Text(
            'Transform your life through the power of intentional manifestation',
            style: TextStyle(
              fontSize: 16.sp,
              color: AppTheme.white.withOpacity(0.9),
              height: 1.5,
            ),
            textAlign: TextAlign.center,
          ),
        ],
      ),
    );
  }

  Widget _buildInfoSection() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'What is Manifestation?',
          style: TextStyle(
            fontSize: 20.sp,
            fontWeight: FontWeight.bold,
            color: AppTheme.textLight,
          ),
        ),
        
        SizedBox(height: 16.h),
        
        _buildFeatureCard(
          icon: Icons.psychology_outlined,
          title: 'Mindful Practice',
          description: 'Daily affirmations, gratitude journaling, and intention setting to align your thoughts with your goals.',
        ),
        
        SizedBox(height: 16.h),
        
        _buildFeatureCard(
          icon: Icons.track_changes_outlined,
          title: 'Goal Tracking',
          description: 'Set meaningful goals, track your progress, and celebrate your achievements along the way.',
        ),
        
        SizedBox(height: 16.h),
        
        _buildFeatureCard(
          icon: Icons.insights_outlined,
          title: 'Personal Growth',
          description: 'Gain insights into your patterns, build positive habits, and create lasting change.',
        ),
      ],
    );
  }

  Widget _buildFeatureCard({
    required IconData icon,
    required String title,
    required String description,
  }) {
    return Container(
      padding: EdgeInsets.all(20.w),
      decoration: BoxDecoration(
        color: AppTheme.cardBackground,
        borderRadius: BorderRadius.circular(16.r),
        border: Border.all(
          color: AppTheme.textMedium.withOpacity(0.1),
        ),
      ),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Container(
            width: 48.w,
            height: 48.h,
            decoration: BoxDecoration(
              color: AppTheme.socialGradient[0].withOpacity(0.1),
              borderRadius: BorderRadius.circular(12.r),
            ),
            child: Icon(
              icon,
              color: AppTheme.socialGradient[0],
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
                    fontWeight: FontWeight.bold,
                    color: AppTheme.textLight,
                  ),
                ),
                SizedBox(height: 8.h),
                Text(
                  description,
                  style: TextStyle(
                    fontSize: 14.sp,
                    color: AppTheme.textMedium,
                    height: 1.4,
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildTeamSection() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Built with ❤️',
          style: TextStyle(
            fontSize: 20.sp,
            fontWeight: FontWeight.bold,
            color: AppTheme.textLight,
          ),
        ),
        
        SizedBox(height: 16.h),
        
        Container(
          width: double.infinity,
          padding: EdgeInsets.all(24.w),
          decoration: BoxDecoration(
            color: AppTheme.cardBackground,
            borderRadius: BorderRadius.circular(20.r),
            border: Border.all(
              color: AppTheme.textMedium.withOpacity(0.1),
            ),
          ),
          child: Column(
            children: [
              Container(
                width: 60.w,
                height: 60.h,
                decoration: BoxDecoration(
                  gradient: LinearGradient(
                    colors: AppTheme.socialGradient,
                  ),
                  borderRadius: BorderRadius.circular(30.r),
                ),
                child: Icon(
                  Icons.groups_rounded,
                  color: AppTheme.white,
                  size: 30.sp,
                ),
              ),
              
              SizedBox(height: 16.h),
              
              Text(
                'Development Team',
                style: TextStyle(
                  fontSize: 18.sp,
                  fontWeight: FontWeight.bold,
                  color: AppTheme.textLight,
                ),
              ),
              
              SizedBox(height: 8.h),
              
              Text(
                'Created by a passionate team dedicated to helping you manifest your dreams and achieve your goals.',
                style: TextStyle(
                  fontSize: 14.sp,
                  color: AppTheme.textMedium,
                  height: 1.5,
                ),
                textAlign: TextAlign.center,
              ),
            ],
          ),
        ),
      ],
    );
  }

  Widget _buildLegalSection() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Legal',
          style: TextStyle(
            fontSize: 20.sp,
            fontWeight: FontWeight.bold,
            color: AppTheme.textLight,
          ),
        ),
        
        SizedBox(height: 16.h),
        
        _buildLegalItem(
          title: 'Privacy Policy',
          subtitle: 'How we protect your data',
          onTap: () {
            // TODO: Open privacy policy
            ScaffoldMessenger.of(context).showSnackBar(
              SnackBar(
                content: Text('Privacy Policy - Coming Soon'),
                behavior: SnackBarBehavior.floating,
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(12.r),
                ),
              ),
            );
          },
        ),
        
        SizedBox(height: 12.h),
        
        _buildLegalItem(
          title: 'Terms of Service',
          subtitle: 'App usage terms and conditions',
          onTap: () {
            // TODO: Open terms of service
            ScaffoldMessenger.of(context).showSnackBar(
              SnackBar(
                content: Text('Terms of Service - Coming Soon'),
                behavior: SnackBarBehavior.floating,
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(12.r),
                ),
              ),
            );
          },
        ),
        
        SizedBox(height: 12.h),
        
        _buildLegalItem(
          title: 'Open Source Licenses',
          subtitle: 'Third-party libraries and licenses',
          onTap: () {
            showLicensePage(
              context: context,
              applicationName: 'Manifestation',
              applicationVersion: _appVersion,
              applicationIcon: Container(
                width: 48.w,
                height: 48.h,
                decoration: BoxDecoration(
                  gradient: LinearGradient(colors: AppTheme.socialGradient),
                  borderRadius: BorderRadius.circular(12.r),
                ),
                child: Icon(
                  Icons.auto_awesome_rounded,
                  color: AppTheme.white,
                  size: 24.sp,
                ),
              ),
            );
          },
        ),
      ],
    );
  }

  Widget _buildLegalItem({
    required String title,
    required String subtitle,
    required VoidCallback onTap,
  }) {
    return Container(
      decoration: BoxDecoration(
        color: AppTheme.cardBackground,
        borderRadius: BorderRadius.circular(16.r),
        border: Border.all(
          color: AppTheme.textMedium.withOpacity(0.1),
        ),
      ),
      child: Material(
        color: Colors.transparent,
        child: InkWell(
          onTap: onTap,
          borderRadius: BorderRadius.circular(16.r),
          child: Padding(
            padding: EdgeInsets.all(16.w),
            child: Row(
              children: [
                Container(
                  width: 40.w,
                  height: 40.h,
                  decoration: BoxDecoration(
                    color: AppTheme.textMedium.withOpacity(0.1),
                    borderRadius: BorderRadius.circular(10.r),
                  ),
                  child: Icon(
                    Icons.article_outlined,
                    color: AppTheme.textMedium,
                    size: 20.sp,
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
                          color: AppTheme.textLight,
                        ),
                      ),
                      SizedBox(height: 4.h),
                      Text(
                        subtitle,
                        style: TextStyle(
                          fontSize: 14.sp,
                          color: AppTheme.textMedium,
                        ),
                      ),
                    ],
                  ),
                ),
                
                Icon(
                  Icons.chevron_right,
                  color: AppTheme.textMedium.withOpacity(0.6),
                  size: 20.sp,
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }
}