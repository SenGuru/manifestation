import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../constants/colors.dart';
import 'onboarding_screen.dart'; // WelcomeScreen
import 'home_screen.dart';
import 'login_screen.dart';

class SplashScreen extends StatefulWidget {
  @override
  _SplashScreenState createState() => _SplashScreenState();
}

class _SplashScreenState extends State<SplashScreen>
    with TickerProviderStateMixin {
  late AnimationController _logoController;
  late AnimationController _fadeController;
  late Animation<double> _logoAnimation;
  late Animation<double> _fadeAnimation;
  
  bool _isInitializing = true;
  String _statusMessage = 'Initializing...';

  @override
  void initState() {
    super.initState();
    
    _logoController = AnimationController(
      duration: Duration(milliseconds: 2000),
      vsync: this,
    );
    
    _fadeController = AnimationController(
      duration: Duration(milliseconds: 800),
      vsync: this,
    );
    
    _logoAnimation = Tween<double>(
      begin: 0.0,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _logoController,
      curve: Curves.elasticOut,
    ));
    
    _fadeAnimation = Tween<double>(
      begin: 0.0,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _fadeController,
      curve: Curves.easeInOut,
    ));
    
    _startInitialization();
  }

  @override
  void dispose() {
    _logoController.dispose();
    _fadeController.dispose();
    super.dispose();
  }

  Future<void> _startInitialization() async {
    try {
      // Start logo animation
      _logoController.forward();
      await Future.delayed(Duration(milliseconds: 500));
      _fadeController.forward();
      
      setState(() {
        _statusMessage = 'Checking session...';
      });
      
      // Simulate session check delay
      await Future.delayed(Duration(milliseconds: 1000));
      
      // Hardcoded session check - always go to login for now
      final isLoggedIn = false; // Hardcoded to always show login
      print('=== SPLASH SCREEN SESSION CHECK ===');
      print('Is logged in: $isLoggedIn');
      
      await Future.delayed(Duration(milliseconds: 800)); // Minimum splash time
      
      // Check if user has seen onboarding
      final hasSeenOnboarding = await _hasSeenOnboarding();
      
      setState(() {
        _statusMessage = hasSeenOnboarding ? 'Ready to go!' : 'Welcome!';
      });
      
      await Future.delayed(Duration(milliseconds: 500));
      
      if (hasSeenOnboarding) {
        _navigateToLogin();
      } else {
        _navigateToOnboarding();
      }
    } catch (e) {
      print('Error during initialization: $e');
      setState(() {
        _statusMessage = 'Starting up...';
      });
      
      await Future.delayed(Duration(milliseconds: 1000));
      _navigateToOnboarding();
    } finally {
      setState(() {
        _isInitializing = false;
      });
    }
  }

  Future<bool> _hasSeenOnboarding() async {
    // TODO: Check SharedPreferences for onboarding completion
    // For now, always show onboarding for new installs
    return false;
  }

  void _navigateToOnboarding() {
    Navigator.of(context).pushReplacement(
      PageRouteBuilder(
        pageBuilder: (context, animation, secondaryAnimation) => WelcomeScreen(),
        transitionsBuilder: (context, animation, secondaryAnimation, child) {
          return FadeTransition(opacity: animation, child: child);
        },
        transitionDuration: Duration(milliseconds: 500),
      ),
    );
  }

  void _navigateToLogin() {
    Navigator.of(context).pushReplacement(
      PageRouteBuilder(
        pageBuilder: (context, animation, secondaryAnimation) => LoginScreen(),
        transitionsBuilder: (context, animation, secondaryAnimation, child) {
          return FadeTransition(opacity: animation, child: child);
        },
        transitionDuration: Duration(milliseconds: 500),
      ),
    );
  }

  void _navigateToHome() {
    Navigator.of(context).pushReplacement(
      PageRouteBuilder(
        pageBuilder: (context, animation, secondaryAnimation) => HomeScreen(),
        transitionsBuilder: (context, animation, secondaryAnimation, child) {
          return FadeTransition(opacity: animation, child: child);
        },
        transitionDuration: Duration(milliseconds: 500),
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.backgroundDark,
      body: SafeArea(
        child: Column(
          children: [
            Expanded(
              child: Center(
                child: Column(
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: [
                    // Logo animation
                    ScaleTransition(
                      scale: _logoAnimation,
                      child: Container(
                        width: 120.w,
                        height: 120.h,
                        decoration: BoxDecoration(
                          gradient: LinearGradient(
                            colors: AppColors.affirmationGradient,
                            begin: Alignment.topLeft,
                            end: Alignment.bottomRight,
                          ),
                          shape: BoxShape.circle,
                          boxShadow: [
                            BoxShadow(
                              color: AppColors.affirmationGradient[0].withOpacity(0.4),
                              blurRadius: 30.r,
                              offset: Offset(0, 10.h),
                            ),
                          ],
                        ),
                        child: Icon(
                          Icons.auto_awesome,
                          color: AppColors.white,
                          size: 60.sp,
                        ),
                      ),
                    ),
                    
                    SizedBox(height: 40.h),
                    
                    // App name
                    FadeTransition(
                      opacity: _fadeAnimation,
                      child: Text(
                        'Manifestation',
                        style: TextStyle(
                          fontSize: 32.sp,
                          fontWeight: FontWeight.bold,
                          color: AppColors.textLight,
                          letterSpacing: 1.2,
                        ),
                      ),
                    ),
                    
                    SizedBox(height: 8.h),
                    
                    // Tagline
                    FadeTransition(
                      opacity: _fadeAnimation,
                      child: Text(
                        'Transform Your Dreams Into Reality',
                        style: TextStyle(
                          fontSize: 16.sp,
                          color: AppColors.textMedium,
                          letterSpacing: 0.5,
                        ),
                      ),
                    ),
                  ],
                ),
              ),
            ),
            
            // Status and loading
            Padding(
              padding: EdgeInsets.only(bottom: 60.h),
              child: Column(
                children: [
                  if (_isInitializing) ...[
                    // Loading indicator
                    SizedBox(
                      width: 24.w,
                      height: 24.h,
                      child: CircularProgressIndicator(
                        strokeWidth: 2,
                        valueColor: AlwaysStoppedAnimation<Color>(
                          AppColors.affirmationGradient[0],
                        ),
                      ),
                    ),
                    
                    SizedBox(height: 16.h),
                    
                    // Status message
                    FadeTransition(
                      opacity: _fadeAnimation,
                      child: Text(
                        _statusMessage,
                        style: TextStyle(
                          fontSize: 14.sp,
                          color: AppColors.textMedium,
                        ),
                      ),
                    ),
                  ],
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }
}