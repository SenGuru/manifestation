import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../constants/app_theme.dart';
import '../services/unified_auth_service.dart';
import '../components/network_error_dialog.dart';
import '../widgets/stylish_loading.dart';
import 'onboarding_screen.dart';
import 'home_screen.dart';
import 'login_screen.dart';
import 'post_signup_onboarding.dart';
import 'package:shared_preferences/shared_preferences.dart';

class SplashScreen extends StatefulWidget {
  @override
  _SplashScreenState createState() => _SplashScreenState();
}

class _SplashScreenState extends State<SplashScreen>
    with SingleTickerProviderStateMixin {
  late AnimationController _animationController;
  late Animation<double> _logoScale;
  late Animation<double> _fadeAnimation;
  late Animation<double> _glowAnimation;
  
  @override
  void initState() {
    super.initState();
    
    _animationController = AnimationController(
      duration: Duration(milliseconds: 2000),
      vsync: this,
    );
    
    _logoScale = Tween<double>(
      begin: 0.5,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _animationController,
      curve: Interval(0.0, 0.5, curve: Curves.easeOutBack),
    ));
    
    _fadeAnimation = Tween<double>(
      begin: 0.0,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _animationController,
      curve: Interval(0.3, 0.7, curve: Curves.easeIn),
    ));

    _glowAnimation = Tween<double>(
      begin: 0.5,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _animationController,
      curve: Curves.easeInOut,
    ));
    
    _animationController.forward();
    _checkAuthAndNavigate();
  }

  @override
  void dispose() {
    _animationController.dispose();
    super.dispose();
  }

  Future<void> _checkAuthAndNavigate() async {
    // Minimum splash display time for smooth experience
    await Future.wait([
      Future.delayed(Duration(milliseconds: 2200)),
      _performAuthCheck(),
    ]);
  }

  Future<void> _performAuthCheck() async {
    try {
      final authService = UnifiedAuthService();
      final prefs = await SharedPreferences.getInstance();
      
      // Check various onboarding states
      final hasSeenInitialOnboarding = prefs.getBool('initial_onboarding_completed') ?? false;
      final hasCompletedPostSignup = prefs.getBool('post_signup_onboarding_completed') ?? false;
      final isReturningUser = prefs.getBool('is_returning_user') ?? false;
      
      await Future.delayed(Duration(milliseconds: 500)); // Small buffer for smooth transition
      
      if (!mounted) return;
      
      // Determine navigation based on auth and onboarding state
      Widget targetScreen;
      
      if (authService.isLoggedIn) {
        if (!hasCompletedPostSignup && !isReturningUser) {
          targetScreen = PostSignupOnboardingScreen();
        } else {
          targetScreen = HomeScreen();
        }
      } else {
        if (!hasSeenInitialOnboarding) {
          targetScreen = WelcomeScreen();
        } else {
          targetScreen = LoginScreen();
        }
      }
      
      // Smooth fade transition
      Navigator.pushReplacement(
        context,
        PageRouteBuilder(
          pageBuilder: (context, animation, secondaryAnimation) => targetScreen,
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
          transitionDuration: Duration(milliseconds: 500),
        ),
      );
      
    } catch (e) {
      print('Error during auth check: $e');
      // Navigate to welcome screen as fallback
      if (mounted) {
        Navigator.pushReplacement(
          context,
          MaterialPageRoute(builder: (_) => WelcomeScreen()),
        );
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppTheme.backgroundDark,
      body: Center(
        child: AnimatedBuilder(
          animation: _animationController,
          builder: (context, child) {
            return Column(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                // Animated logo
                Transform.scale(
                  scale: _logoScale.value,
                  child: Container(
                    width: 120.w,
                    height: 120.h,
                    decoration: BoxDecoration(
                      gradient: LinearGradient(
                        colors: AppTheme.affirmationGradient,
                        begin: Alignment.topLeft,
                        end: Alignment.bottomRight,
                      ),
                      shape: BoxShape.circle,
                      boxShadow: [
                        BoxShadow(
                          color: AppTheme.affirmationGradient[0]
                              .withOpacity(0.3 * _glowAnimation.value),
                          blurRadius: 30.r * _glowAnimation.value,
                          spreadRadius: 10.r * _glowAnimation.value,
                        ),
                      ],
                    ),
                    child: Icon(
                      Icons.auto_awesome,
                      color: AppTheme.white,
                      size: 60.sp,
                    ),
                  ),
                ),
                
                SizedBox(height: 40.h),
                
                // App title
                FadeTransition(
                  opacity: _fadeAnimation,
                  child: Column(
                    children: [
                      Text(
                        'Manifestation',
                        style: TextStyle(
                          fontSize: 36.sp,
                          fontWeight: FontWeight.bold,
                          color: AppTheme.textLight,
                          letterSpacing: 1.2,
                        ),
                      ),
                      SizedBox(height: 8.h),
                      Text(
                        'Transform your dreams into reality',
                        style: TextStyle(
                          fontSize: 16.sp,
                          color: AppTheme.textMedium,
                          fontWeight: FontWeight.w400,
                        ),
                      ),
                    ],
                  ),
                ),
                
                SizedBox(height: 80.h),
                
                // Simple loading indicator
                FadeTransition(
                  opacity: _fadeAnimation,
                  child: SizedBox(
                    width: 24.w,
                    height: 24.h,
                    child: CircularProgressIndicator(
                      strokeWidth: 2,
                      valueColor: AlwaysStoppedAnimation<Color>(
                        AppTheme.affirmationGradient[0].withOpacity(0.8),
                      ),
                    ),
                  ),
                ),
              ],
            );
          },
        ),
      ),
    );
  }
}