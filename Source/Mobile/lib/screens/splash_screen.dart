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
    with TickerProviderStateMixin {
  late AnimationController _logoController;
  late AnimationController _fadeController;
  late AnimationController _glowController;
  late AnimationController _textController;
  late Animation<double> _logoAnimation;
  late Animation<double> _fadeAnimation;
  late Animation<double> _glowAnimation;
  late Animation<double> _textAnimation;
  late Animation<Offset> _textSlideAnimation;
  
  bool _isInitializing = true;
  String _statusMessage = 'Welcome...';

  @override
  void initState() {
    super.initState();
    
    _logoController = AnimationController(
      duration: Duration(milliseconds: 1800),
      vsync: this,
    );
    
    _fadeController = AnimationController(
      duration: Duration(milliseconds: 1200),
      vsync: this,
    );

    _glowController = AnimationController(
      duration: Duration(milliseconds: 2500),
      vsync: this,
    );

    _textController = AnimationController(
      duration: Duration(milliseconds: 1000),
      vsync: this,
    );
    
    _logoAnimation = Tween<double>(
      begin: 0.0,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _logoController,
      curve: Curves.easeOutBack,
    ));
    
    _fadeAnimation = Tween<double>(
      begin: 0.0,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _fadeController,
      curve: Curves.easeInOut,
    ));

    _glowAnimation = Tween<double>(
      begin: 0.3,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _glowController,
      curve: Curves.easeInOut,
    ));

    _textAnimation = Tween<double>(
      begin: 0.0,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _textController,
      curve: Curves.easeOutCubic,
    ));

    _textSlideAnimation = Tween<Offset>(
      begin: Offset(0, 0.3),
      end: Offset.zero,
    ).animate(CurvedAnimation(
      parent: _textController,
      curve: Curves.easeOutCubic,
    ));
    
    _startInitialization();
  }

  @override
  void dispose() {
    _logoController.dispose();
    _fadeController.dispose();
    _glowController.dispose();
    _textController.dispose();
    super.dispose();
  }

  Future<void> _startInitialization() async {
    try {
      // Staggered animation sequence for premium feel
      _logoController.forward();
      _glowController.repeat(reverse: true);
      
      await Future.delayed(Duration(milliseconds: 600));
      _fadeController.forward();
      
      await Future.delayed(Duration(milliseconds: 400));
      _textController.forward();
      
      setState(() {
        _statusMessage = 'Preparing your experience...';
      });
      
      // Simulate session check delay with smoother messaging
      await Future.delayed(Duration(milliseconds: 800));
      
      setState(() {
        _statusMessage = 'Checking session...';
      });
      
      // Check actual authentication state
      final authService = UnifiedAuthService();
      
      // Start verification process while allowing animations to continue
      setState(() {
        _statusMessage = 'Verifying credentials...';
      });
      
      // Poll for session state changes instead of blocking with delay
      SessionState sessionState;
      bool isLoggedIn;
      int attempts = 0;
      const maxAttempts = 15; // 3 seconds max (15 * 200ms)
      
      do {
        await Future.delayed(Duration(milliseconds: 200)); // Short non-blocking delay
        sessionState = authService.sessionState;
        isLoggedIn = authService.isLoggedIn;
        attempts++;
        
        // Update UI with progress indicators
        if (attempts % 3 == 0) {
          setState(() {
            _statusMessage = attempts < 5 
              ? 'Verifying credentials...' 
              : 'Connecting to server...';
          });
        }
        
        // Break if we have a definitive state or max attempts reached
        if (sessionState.toString() != 'SessionState.loggedOut' || attempts >= maxAttempts) {
          break;
        }
      } while (attempts < maxAttempts);
      print('=== SPLASH SCREEN SESSION CHECK ===');
      print('Is logged in: $isLoggedIn');
      print('Session state: $sessionState');
      print('Has current user: ${authService.currentUser != null}');
      print('Has auth token: ${authService.authToken != null}');
      
      await Future.delayed(Duration(milliseconds: 600));
      
      print('Final session state: $sessionState after $attempts attempts');
      
      // Handle network error state - show network error instead of login
      if (sessionState.toString().contains('networkError')) {
        print('🌐 Network error detected - showing connection error dialog');
        setState(() {
          _statusMessage = 'Connection issue detected...';
        });
        
        await Future.delayed(Duration(milliseconds: 500));
        
        // Show network error dialog with retry functionality
        _showNetworkErrorWithRetry();
        return; // Don't continue with normal flow
      }
      
      // Check if user has seen initial onboarding and post-signup onboarding
      final hasSeenOnboarding = await _hasSeenOnboarding();
      final hasCompletedPostSignupOnboarding = await _hasCompletedPostSignupOnboarding();
      
      setState(() {
        _statusMessage = hasSeenOnboarding ? 'Almost ready...' : 'Welcome to Manifestation!';
      });
      
      await Future.delayed(Duration(milliseconds: 600));
      
      if (isLoggedIn) {
        // User is logged in, check if they need post-signup onboarding
        if (!hasCompletedPostSignupOnboarding) {
          _navigateToPostSignupOnboarding();
        } else {
          _navigateToHome();
        }
      } else if (hasSeenOnboarding) {
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

  /// Show network error dialog with retry functionality
  void _showNetworkErrorWithRetry({int retryCount = 0}) {
    showNetworkErrorDialog(
      context,
      message: retryCount == 0 
        ? 'We\'re having trouble connecting to our servers. Please check your internet connection and try again.'
        : 'Still having connection issues. Please check your network and try again.',
      onRetry: () => _forceRetryWithValidation(),
    );
  }

  /// Force retry with explicit session validation
  void _forceRetryWithValidation() async {
    print('🔄 Forcing session validation retry...');
    
    // Reset animations first
    setState(() {
      _isInitializing = true;
      _statusMessage = 'Retrying connection...';
    });
    
    // Reset all animation controllers
    _logoController.reset();
    _fadeController.reset();
    _glowController.reset();
    _textController.reset();
    
    // Start animations
    _logoController.forward();
    _glowController.repeat(reverse: true);
    
    await Future.delayed(Duration(milliseconds: 600));
    _fadeController.forward();
    
    await Future.delayed(Duration(milliseconds: 400));
    _textController.forward();
    
    // Force the auth service to retry validation immediately
    final authService = UnifiedAuthService();
    
    setState(() {
      _statusMessage = 'Reconnecting to server...';
    });
    
    // Explicitly call retry on the auth service
    final retrySuccess = await authService.retrySessionValidation();
    
    await Future.delayed(Duration(milliseconds: 600));
    
    if (retrySuccess) {
      print('✅ Retry successful - proceeding to app');
      setState(() {
        _statusMessage = 'Connection restored!';
      });
      
      await Future.delayed(Duration(milliseconds: 800));
      
      // Check onboarding and navigate appropriately
      final hasCompletedPostSignupOnboarding = await _hasCompletedPostSignupOnboarding();
      if (!hasCompletedPostSignupOnboarding) {
        _navigateToPostSignupOnboarding();
      } else {
        _navigateToHome();
      }
    } else {
      // Still failing - check if it's still a network error or auth failure
      final sessionState = authService.sessionState;
      print('❌ Retry failed - session state: $sessionState');
      
      if (sessionState.toString().contains('networkError')) {
        // Still network issues
        setState(() {
          _statusMessage = 'Still having connection issues...';
        });
        
        await Future.delayed(Duration(milliseconds: 1000));
        _showNetworkErrorWithRetry(retryCount: 1);
      } else {
        // Auth failure - go to login
        setState(() {
          _statusMessage = 'Session expired...';
        });
        
        await Future.delayed(Duration(milliseconds: 1000));
        _navigateToLogin();
      }
    }
  }

  /// Restart the splash screen with full animation sequence for retry
  void _restartWithAnimation() {
    print('🔄 Restarting splash screen with full animation sequence...');
    
    setState(() {
      _isInitializing = true;
      _statusMessage = 'Initializing...';
    });
    
    // Reset all animation controllers to starting position
    _logoController.reset();
    _fadeController.reset();
    _glowController.reset();
    _textController.reset();
    
    // Start the initialization sequence again with animations
    _startInitialization();
  }

  Future<bool> _hasSeenOnboarding() async {
    try {
      final prefs = await SharedPreferences.getInstance();
      return prefs.getBool('initial_onboarding_completed') ?? false;
    } catch (e) {
      return false;
    }
  }

  Future<bool> _hasCompletedPostSignupOnboarding() async {
    try {
      final prefs = await SharedPreferences.getInstance();
      return prefs.getBool('post_signup_onboarding_completed') ?? false;
    } catch (e) {
      return false;
    }
  }

  void _navigateToOnboarding() {
    // Check if user is already logged in
    final authService = UnifiedAuthService();
    
    if (authService.isLoggedIn) {
      // User is logged in, go to home screen
      Navigator.of(context).pushReplacement(
        PageRouteBuilder(
          pageBuilder: (context, animation, secondaryAnimation) => HomeScreen(),
          transitionsBuilder: (context, animation, secondaryAnimation, child) {
            return FadeTransition(opacity: animation, child: child);
          },
          transitionDuration: Duration(milliseconds: 500),
        ),
      );
    } else {
      // User not logged in, go to onboarding
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

  void _navigateToPostSignupOnboarding() {
    Navigator.of(context).pushReplacement(
      PageRouteBuilder(
        pageBuilder: (context, animation, secondaryAnimation) => PostSignupOnboardingScreen(),
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
      body: Container(
        width: double.infinity,
        height: double.infinity,
        decoration: BoxDecoration(
          gradient: RadialGradient(
            center: Alignment.center,
            radius: 1.2,
            colors: [
              AppTheme.backgroundPrimary.withOpacity(0.95),
              AppTheme.backgroundPrimary,
              Color(0xFF0A0B0F),
            ],
            stops: [0.0, 0.7, 1.0],
          ),
        ),
        child: SafeArea(
          child: Column(
            children: [
              Expanded(
                child: Center(
                  child: Column(
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: [
                      // Enhanced logo with multiple animation layers
                      AnimatedBuilder(
                        animation: Listenable.merge([_logoAnimation, _glowAnimation]),
                        builder: (context, child) {
                          return Stack(
                            alignment: Alignment.center,
                            children: [
                              // Outer glow ring
                              Container(
                                width: 180.w,
                                height: 180.h,
                                decoration: BoxDecoration(
                                  shape: BoxShape.circle,
                                  gradient: RadialGradient(
                                    colors: [
                                      AppTheme.affirmationGradient[0].withOpacity(_glowAnimation.value * 0.1),
                                      AppTheme.affirmationGradient[0].withOpacity(0),
                                    ],
                                  ),
                                ),
                              ),
                              
                              // Middle glow ring
                              Container(
                                width: 150.w,
                                height: 150.h,
                                decoration: BoxDecoration(
                                  shape: BoxShape.circle,
                                  gradient: RadialGradient(
                                    colors: [
                                      AppTheme.affirmationGradient[1].withOpacity(_glowAnimation.value * 0.15),
                                      AppTheme.affirmationGradient[1].withOpacity(0),
                                    ],
                                  ),
                                ),
                              ),
                              
                              // Main logo
                              ScaleTransition(
                                scale: _logoAnimation,
                                child: Container(
                                  width: 140.w,
                                  height: 140.h,
                                  decoration: BoxDecoration(
                                    gradient: LinearGradient(
                                      colors: AppTheme.affirmationGradient,
                                      begin: Alignment.topLeft,
                                      end: Alignment.bottomRight,
                                    ),
                                    shape: BoxShape.circle,
                                    boxShadow: [
                                      BoxShadow(
                                        color: AppTheme.affirmationGradient[0].withOpacity(0.6),
                                        blurRadius: 40.r,
                                        offset: Offset(0, 8.h),
                                        spreadRadius: 0,
                                      ),
                                      BoxShadow(
                                        color: AppTheme.affirmationGradient[1].withOpacity(0.4),
                                        blurRadius: 60.r,
                                        offset: Offset(0, 16.h),
                                        spreadRadius: 0,
                                      ),
                                    ],
                                  ),
                                  child: Icon(
                                    Icons.auto_awesome,
                                    color: AppTheme.white,
                                    size: 70.sp,
                                  ),
                                ),
                              ),
                            ],
                          );
                        },
                      ),
                      
                      SizedBox(height: 48.h),
                      
                      // App name with slide animation
                      SlideTransition(
                        position: _textSlideAnimation,
                        child: FadeTransition(
                          opacity: _textAnimation,
                          child: ShaderMask(
                            shaderCallback: (bounds) => LinearGradient(
                              colors: [
                                AppTheme.affirmationGradient[0],
                                AppTheme.affirmationGradient[1],
                                AppTheme.white,
                              ],
                              stops: [0.0, 0.5, 1.0],
                            ).createShader(bounds),
                            child: Text(
                              'Mistel',
                              style: TextStyle(
                                fontSize: 36.sp,
                                fontWeight: FontWeight.w800,
                                color: AppTheme.white,
                                letterSpacing: 1.5,
                                height: 1.1,
                              ),
                            ),
                          ),
                        ),
                      ),
                      
                      SizedBox(height: 12.h),
                      
                      // Enhanced tagline
                      SlideTransition(
                        position: _textSlideAnimation,
                        child: FadeTransition(
                          opacity: _textAnimation,
                          child: Text(
                            'Transform Your Dreams Into Reality',
                            style: TextStyle(
                              fontSize: 17.sp,
                              color: AppTheme.textMedium.withOpacity(0.9),
                              letterSpacing: 0.8,
                              fontWeight: FontWeight.w500,
                              height: 1.3,
                            ),
                          ),
                        ),
                      ),
                    ],
                  ),
                ),
              ),
              
              // Enhanced status and loading section
              Padding(
                padding: EdgeInsets.only(bottom: 80.h),
                child: Column(
                  children: [
                    if (_isInitializing) ...[
                      // Premium loading indicator
                      Container(
                        width: 48.w,
                        height: 48.h,
                        child: Stack(
                          alignment: Alignment.center,
                          children: [
                            // Outer ring
                            SizedBox(
                              width: 48.w,
                              height: 48.h,
                              child: CircularProgressIndicator(
                                strokeWidth: 3,
                                valueColor: AlwaysStoppedAnimation<Color>(
                                  AppTheme.affirmationGradient[0].withOpacity(0.3),
                                ),
                                backgroundColor: AppTheme.textMedium.withOpacity(0.1),
                              ),
                            ),
                            // Inner animated circle
                            Container(
                              width: 32.w,
                              height: 32.h,
                              child: CircularProgressIndicator(
                                strokeWidth: 2,
                                valueColor: AlwaysStoppedAnimation<Color>(
                                  AppTheme.affirmationGradient[1],
                                ),
                              ),
                            ),
                          ],
                        ),
                      ),
                      
                      SizedBox(height: 24.h),
                      
                      // Animated status message
                      AnimatedSwitcher(
                        duration: Duration(milliseconds: 300),
                        child: Text(
                          _statusMessage,
                          key: ValueKey(_statusMessage),
                          style: TextStyle(
                            fontSize: 15.sp,
                            color: AppTheme.textMedium,
                            fontWeight: FontWeight.w500,
                            letterSpacing: 0.3,
                          ),
                        ),
                      ),
                      
                      SizedBox(height: 8.h),
                      
                      // Subtle progress dots
                      Row(
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: List.generate(3, (index) {
                          return AnimatedBuilder(
                            animation: _glowController,
                            builder: (context, child) {
                              final delay = index * 0.3;
                              final animValue = (_glowController.value + delay) % 1.0;
                              return Container(
                                margin: EdgeInsets.symmetric(horizontal: 4.w),
                                width: 6.w,
                                height: 6.h,
                                decoration: BoxDecoration(
                                  shape: BoxShape.circle,
                                  color: AppTheme.affirmationGradient[0].withOpacity(
                                    0.3 + (animValue * 0.7),
                                  ),
                                ),
                              );
                            },
                          );
                        }),
                      ),
                    ],
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