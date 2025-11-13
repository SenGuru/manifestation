import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:shared_preferences/shared_preferences.dart';
import '../constants/app_theme.dart';
import '../utils/navigation_utils.dart';
import 'login_screen.dart';
import 'signup_page.dart';

class WelcomeScreen extends StatefulWidget {
  @override
  _WelcomeScreenState createState() => _WelcomeScreenState();
}

class _WelcomeScreenState extends State<WelcomeScreen>
    with SingleTickerProviderStateMixin {
  late AnimationController _mainController;
  late Animation<double> _fadeAnimation;
  late Animation<Offset> _slideAnimation;
  late Animation<double> _scaleAnimation;
  late Animation<double> _floatAnimation;
  
  bool _featuresVisible = false;

  @override
  void initState() {
    super.initState();
    
    _mainController = AnimationController(
      duration: Duration(milliseconds: 1200),
      vsync: this,
    );
    
    _fadeAnimation = Tween<double>(
      begin: 0.0,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _mainController,
      curve: Interval(0.0, 0.6, curve: Curves.easeOut),
    ));
    
    _slideAnimation = Tween<Offset>(
      begin: Offset(0, 0.2),
      end: Offset.zero,
    ).animate(CurvedAnimation(
      parent: _mainController,
      curve: Interval(0.2, 0.7, curve: Curves.easeOutCubic),
    ));
    
    _scaleAnimation = Tween<double>(
      begin: 0.8,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _mainController,
      curve: Interval(0.0, 0.5, curve: Curves.easeOut),
    ));
    
    // Subtle floating animation
    _floatAnimation = Tween<double>(
      begin: -5.0,
      end: 5.0,
    ).animate(CurvedAnimation(
      parent: _mainController,
      curve: Curves.easeInOut,
    ));
    
    _mainController.forward();
    _mainController.addStatusListener((status) {
      if (status == AnimationStatus.completed) {
        setState(() {
          _featuresVisible = true;
        });
      }
    });
  }

  @override
  void dispose() {
    _mainController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppTheme.backgroundDark,
      body: SafeArea(
        child: AnimatedBuilder(
          animation: _mainController,
          builder: (context, child) {
            return FadeTransition(
              opacity: _fadeAnimation,
              child: SlideTransition(
                position: _slideAnimation,
                child: Padding(
                  padding: EdgeInsets.symmetric(horizontal: 24.w),
                  child: Column(
                    children: [
                      Expanded(
                        child: SingleChildScrollView(
                          physics: BouncingScrollPhysics(),
                          child: Column(
                            children: [
                              SizedBox(height: 20.h),
                              _buildHeader(),
                              SizedBox(height: 30.h),
                              _buildFeatures(),
                            ],
                          ),
                        ),
                      ),
                      _buildActionButtons(),
                      SizedBox(height: 20.h),
                    ],
                  ),
                ),
              ),
            );
          },
        ),
      ),
    );
  }

  Widget _buildHeader() {
    return Column(
      children: [
        // Logo
        Transform.scale(
          scale: _scaleAnimation.value,
          child: Container(
            width: 100.w,
            height: 100.h,
            decoration: BoxDecoration(
              gradient: LinearGradient(
                colors: AppTheme.affirmationGradient,
                begin: Alignment.topLeft,
                end: Alignment.bottomRight,
              ),
              shape: BoxShape.circle,
              boxShadow: [
                BoxShadow(
                  color: AppTheme.affirmationGradient[0].withOpacity(0.3),
                  blurRadius: 25.r,
                  offset: Offset(0, 10.h),
                ),
              ],
            ),
            child: Icon(
              Icons.auto_awesome,
              color: AppTheme.white,
              size: 50.sp,
            ),
          ),
        ),
        
        SizedBox(height: 30.h),
        
        Text(
          'Manifestation',
          style: TextStyle(
            fontSize: 36.sp,
            fontWeight: FontWeight.bold,
            color: AppTheme.textLight,
          ),
        ),
        
        SizedBox(height: 8.h),
        
        Text(
          'Transform your dreams into reality',
          style: TextStyle(
            fontSize: 17.sp,
            color: AppTheme.textMedium,
            fontWeight: FontWeight.w400,
          ),
          textAlign: TextAlign.center,
        ),
      ],
    );
  }

  Widget _buildFeatures() {
    final features = [
      {
        'icon': Icons.favorite,
        'title': 'Daily Affirmations',
        'description': 'Powerful words to rewire your mindset',
        'gradient': AppTheme.affirmationGradient,
      },
      {
        'icon': Icons.dashboard,
        'title': 'Vision Boards',
        'description': 'Visualize your dreams and goals',
        'gradient': AppTheme.visionBoardGradient,
      },
      {
        'icon': Icons.people,
        'title': 'Community',
        'description': 'Connect with fellow manifesters',
        'gradient': AppTheme.socialGradient,
      },
    ];

    return AnimatedOpacity(
      opacity: _featuresVisible ? 1.0 : 0.0,
      duration: Duration(milliseconds: 600),
      child: Column(
        children: [
          Text(
            'Everything you need to manifest',
            style: TextStyle(
              fontSize: 20.sp,
              fontWeight: FontWeight.w600,
              color: AppTheme.textLight,
            ),
            textAlign: TextAlign.center,
          ),
          
          SizedBox(height: 24.h),
          
          ...features.asMap().entries.map((entry) {
            final index = entry.key;
            final feature = entry.value;
            
            return AnimatedContainer(
              duration: Duration(milliseconds: 400 + (index * 100)),
              curve: Curves.easeOutCubic,
              margin: EdgeInsets.only(bottom: 16.h),
              transform: Matrix4.translationValues(
                0, 
                _featuresVisible ? 0 : 20.h, 
                0
              ),
              child: _buildFeatureCard(
                feature['icon'] as IconData,
                feature['title'] as String,
                feature['description'] as String,
                feature['gradient'] as List<Color>,
              ),
            );
          }).toList(),
        ],
      ),
    );
  }

  Widget _buildFeatureCard(
    IconData icon,
    String title,
    String description,
    List<Color> gradient,
  ) {
    return Container(
      padding: EdgeInsets.all(16.w),
      decoration: BoxDecoration(
        color: AppTheme.cardBackground,
        borderRadius: BorderRadius.circular(16.r),
        border: Border.all(
          color: gradient[0].withOpacity(0.15),
        ),
      ),
      child: Row(
        children: [
          Container(
            width: 48.w,
            height: 48.h,
            decoration: BoxDecoration(
              gradient: LinearGradient(colors: gradient),
              borderRadius: BorderRadius.circular(12.r),
            ),
            child: Icon(
              icon,
              color: AppTheme.white,
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
                    color: AppTheme.textLight,
                  ),
                ),
                SizedBox(height: 4.h),
                Text(
                  description,
                  style: TextStyle(
                    fontSize: 13.sp,
                    color: AppTheme.textMedium,
                    height: 1.3,
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildActionButtons() {
    return AnimatedOpacity(
      opacity: _featuresVisible ? 1.0 : 0.0,
      duration: Duration(milliseconds: 800),
      child: Column(
        children: [
          // Get Started button
          Container(
            width: double.infinity,
            height: 54.h,
            decoration: BoxDecoration(
              gradient: LinearGradient(
                colors: AppTheme.affirmationGradient,
              ),
              borderRadius: BorderRadius.circular(16.r),
              boxShadow: [
                BoxShadow(
                  color: AppTheme.affirmationGradient[0].withOpacity(0.25),
                  blurRadius: 15.r,
                  offset: Offset(0, 8.h),
                ),
              ],
            ),
            child: Material(
              color: Colors.transparent,
              child: InkWell(
                onTap: _navigateToSignUp,
                borderRadius: BorderRadius.circular(16.r),
                child: Center(
                  child: Text(
                    'Get Started',
                    style: TextStyle(
                      fontSize: 17.sp,
                      fontWeight: FontWeight.w600,
                      color: AppTheme.white,
                    ),
                  ),
                ),
              ),
            ),
          ),
          
          SizedBox(height: 14.h),
          
          // Sign In button
          Container(
            width: double.infinity,
            height: 54.h,
            decoration: BoxDecoration(
              color: AppTheme.cardBackground,
              borderRadius: BorderRadius.circular(16.r),
              border: Border.all(
                color: AppTheme.textMedium.withOpacity(0.2),
              ),
            ),
            child: Material(
              color: Colors.transparent,
              child: InkWell(
                onTap: _navigateToLogin,
                borderRadius: BorderRadius.circular(16.r),
                child: Center(
                  child: Text(
                    'Sign In',
                    style: TextStyle(
                      fontSize: 17.sp,
                      fontWeight: FontWeight.w600,
                      color: AppTheme.textLight,
                    ),
                  ),
                ),
              ),
            ),
          ),
          
          SizedBox(height: 20.h),
          
          // Terms text
          Padding(
            padding: EdgeInsets.symmetric(horizontal: 20.w),
            child: RichText(
              textAlign: TextAlign.center,
              text: TextSpan(
                style: TextStyle(
                  fontSize: 12.sp,
                  color: AppTheme.textMedium.withOpacity(0.8),
                  height: 1.4,
                ),
                children: [
                  TextSpan(text: 'By continuing, you agree to our '),
                  TextSpan(
                    text: 'Terms of Service',
                    style: TextStyle(
                      color: AppTheme.affirmationGradient[0],
                      fontWeight: FontWeight.w500,
                    ),
                  ),
                  TextSpan(text: ' and '),
                  TextSpan(
                    text: 'Privacy Policy',
                    style: TextStyle(
                      color: AppTheme.affirmationGradient[0],
                      fontWeight: FontWeight.w500,
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

  Future<void> _navigateToSignUp() async {
    await _markInitialOnboardingCompleted();
    
    Navigator.push(
      context,
      PageRouteBuilder(
        pageBuilder: (context, animation, secondaryAnimation) => SignUpScreen(),
        transitionsBuilder: (context, animation, secondaryAnimation, child) {
          return FadeTransition(
            opacity: animation.drive(
              Tween(begin: 0.0, end: 1.0).chain(
                CurveTween(curve: Curves.easeInOut),
              ),
            ),
            child: child,
          );
        },
        transitionDuration: Duration(milliseconds: 400),
      ),
    );
  }

  Future<void> _navigateToLogin() async {
    await _markInitialOnboardingCompleted();
    
    Navigator.push(
      context,
      PageRouteBuilder(
        pageBuilder: (context, animation, secondaryAnimation) => LoginScreen(),
        transitionsBuilder: (context, animation, secondaryAnimation, child) {
          return FadeTransition(
            opacity: animation.drive(
              Tween(begin: 0.0, end: 1.0).chain(
                CurveTween(curve: Curves.easeInOut),
              ),
            ),
            child: child,
          );
        },
        transitionDuration: Duration(milliseconds: 400),
      ),
    );
  }

  Future<void> _markInitialOnboardingCompleted() async {
    try {
      final prefs = await SharedPreferences.getInstance();
      await prefs.setBool('initial_onboarding_completed', true);
    } catch (e) {
      print('Error marking initial onboarding as completed: $e');
    }
  }
}