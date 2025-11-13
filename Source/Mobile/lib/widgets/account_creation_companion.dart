import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'dart:math' as math;
import '../constants/app_theme.dart';

class AccountCreationCompanion extends StatefulWidget {
  final Function() onComplete;
  final String userName;
  
  const AccountCreationCompanion({
    Key? key,
    required this.onComplete,
    required this.userName,
  }) : super(key: key);

  @override
  _AccountCreationCompanionState createState() => _AccountCreationCompanionState();
}

class _AccountCreationCompanionState extends State<AccountCreationCompanion>
    with TickerProviderStateMixin {
  
  // Core animation controllers
  late AnimationController _breathingController;
  late AnimationController _floatingController;
  late AnimationController _particleController;
  late AnimationController _textRevealController;
  late AnimationController _progressController;
  late AnimationController _shimmerController;
  
  // Animations
  late Animation<double> _breathingAnimation;
  late Animation<double> _floatingAnimation;
  late Animation<double> _textFadeAnimation;
  late Animation<double> _textSlideAnimation;
  late Animation<double> _progressAnimation;
  late Animation<double> _shimmerAnimation;
  
  // State
  int _currentMessageIndex = 0;
  bool _isComplete = false;
  
  // Messages to show during setup
  final List<Map<String, dynamic>> _messages = [
    {
      'text': 'Welcome to your manifestation journey',
      'emoji': '✨',
      'duration': 3000,
    },
    {
      'text': 'Setting up your personal space',
      'emoji': '🏡',
      'duration': 2500,
    },
    {
      'text': 'Preparing your affirmation categories',
      'emoji': '💝',
      'duration': 2500,
    },
    {
      'text': 'Creating your vision board canvas',
      'emoji': '🎨',
      'duration': 2000,
    },
    {
      'text': 'Aligning your manifestation energy',
      'emoji': '🌟',
      'duration': 2000,
    },
    {
      'text': 'Almost ready to begin',
      'emoji': '🚀',
      'duration': 1500,
    },
  ];
  
  @override
  void initState() {
    super.initState();
    _initializeAnimations();
    _startSequence();
    
    // Gentle haptic to mark the beginning
    HapticFeedback.lightImpact();
  }
  
  void _initializeAnimations() {
    // Breathing animation for the center orb
    _breathingController = AnimationController(
      duration: Duration(seconds: 4),
      vsync: this,
    )..repeat();
    
    _breathingAnimation = Tween<double>(
      begin: 0.8,
      end: 1.2,
    ).animate(CurvedAnimation(
      parent: _breathingController,
      curve: Curves.easeInOut,
    ));
    
    // Floating animation for gentle movement
    _floatingController = AnimationController(
      duration: Duration(seconds: 6),
      vsync: this,
    )..repeat();
    
    _floatingAnimation = Tween<double>(
      begin: -10,
      end: 10,
    ).animate(CurvedAnimation(
      parent: _floatingController,
      curve: Curves.easeInOut,
    ));
    
    // Particle animation for ambient effects
    _particleController = AnimationController(
      duration: Duration(seconds: 8),
      vsync: this,
    )..repeat();
    
    // Text reveal animation
    _textRevealController = AnimationController(
      duration: Duration(milliseconds: 1200),
      vsync: this,
    );
    
    _textFadeAnimation = Tween<double>(
      begin: 0.0,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _textRevealController,
      curve: Interval(0.0, 0.6, curve: Curves.easeOut),
    ));
    
    _textSlideAnimation = Tween<double>(
      begin: 20.0,
      end: 0.0,
    ).animate(CurvedAnimation(
      parent: _textRevealController,
      curve: Interval(0.0, 0.8, curve: Curves.easeOutCubic),
    ));
    
    // Progress animation
    _progressController = AnimationController(
      duration: Duration(seconds: 15), // Total duration for all messages
      vsync: this,
    );
    
    _progressAnimation = Tween<double>(
      begin: 0.0,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _progressController,
      curve: Curves.easeInOut,
    ));
    
    // Shimmer effect
    _shimmerController = AnimationController(
      duration: Duration(seconds: 2),
      vsync: this,
    )..repeat();
    
    _shimmerAnimation = Tween<double>(
      begin: -1.0,
      end: 2.0,
    ).animate(CurvedAnimation(
      parent: _shimmerController,
      curve: Curves.easeInOut,
    ));
  }
  
  void _startSequence() async {
    // Start the overall progress
    _progressController.forward();
    
    // Cycle through messages
    for (int i = 0; i < _messages.length; i++) {
      if (!mounted) return;
      
      setState(() {
        _currentMessageIndex = i;
      });
      
      // Reset and play text animation
      _textRevealController.reset();
      _textRevealController.forward();
      
      // Add subtle haptic for each message
      if (i > 0) {
        HapticFeedback.selectionClick();
      }
      
      // Wait for the message duration
      await Future.delayed(Duration(milliseconds: _messages[i]['duration']));
    }
    
    // Final completion
    if (mounted) {
      setState(() {
        _isComplete = true;
      });
      
      // Success haptic
      HapticFeedback.mediumImpact();
      
      // Wait a moment then call completion
      await Future.delayed(Duration(milliseconds: 800));
      widget.onComplete();
    }
  }
  
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppTheme.backgroundPrimary,
      body: SafeArea(
        child: Stack(
          children: [
            // Animated gradient background
            _buildAnimatedBackground(),
            
            // Floating particles
            ..._buildFloatingParticles(),
            
            // Main content
            Center(
              child: Column(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  // Animated orb
                  _buildCentralOrb(),
                  
                  SizedBox(height: 60.h),
                  
                  // Message display
                  _buildMessageDisplay(),
                  
                  SizedBox(height: 80.h),
                  
                  // Progress indicator
                  _buildProgressIndicator(),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }
  
  Widget _buildAnimatedBackground() {
    return AnimatedBuilder(
      animation: _shimmerAnimation,
      builder: (context, child) {
        return Container(
          decoration: BoxDecoration(
            gradient: RadialGradient(
              center: Alignment(
                _shimmerAnimation.value * 0.2,
                -0.3 + _shimmerAnimation.value * 0.1,
              ),
              radius: 1.5,
              colors: [
                AppTheme.backgroundSecondary,
                AppTheme.backgroundPrimary,
              ],
            ),
          ),
        );
      },
    );
  }
  
  Widget _buildCentralOrb() {
    return AnimatedBuilder(
      animation: Listenable.merge([
        _breathingAnimation,
        _floatingAnimation,
        _particleController,
      ]),
      builder: (context, child) {
        return Transform.translate(
          offset: Offset(0, _floatingAnimation.value),
          child: Transform.scale(
            scale: _breathingAnimation.value,
            child: Container(
              width: 180.w,
              height: 180.w,
              child: Stack(
                alignment: Alignment.center,
                children: [
                  // Outer glow
                  Container(
                    width: 180.w,
                    height: 180.w,
                    decoration: BoxDecoration(
                      shape: BoxShape.circle,
                      gradient: RadialGradient(
                        colors: [
                          AppTheme.affirmationGradient[0].withValues(alpha: 0.3),
                          AppTheme.affirmationGradient[1].withValues(alpha: 0.1),
                          Colors.transparent,
                        ],
                      ),
                    ),
                  ),
                  
                  // Middle ring
                  Container(
                    width: 140.w,
                    height: 140.w,
                    decoration: BoxDecoration(
                      shape: BoxShape.circle,
                      border: Border.all(
                        color: AppTheme.affirmationGradient[0].withValues(alpha: 0.3),
                        width: 2,
                      ),
                    ),
                  ),
                  
                  // Inner orb
                  Container(
                    width: 100.w,
                    height: 100.w,
                    decoration: BoxDecoration(
                      shape: BoxShape.circle,
                      gradient: LinearGradient(
                        begin: Alignment.topLeft,
                        end: Alignment.bottomRight,
                        colors: [
                          AppTheme.affirmationGradient[0],
                          AppTheme.affirmationGradient[1],
                        ],
                      ),
                      boxShadow: [
                        BoxShadow(
                          color: AppTheme.affirmationGradient[0].withValues(alpha: 0.5),
                          blurRadius: 30,
                          spreadRadius: 10,
                        ),
                      ],
                    ),
                    child: Center(
                      child: AnimatedSwitcher(
                        duration: Duration(milliseconds: 600),
                        child: Text(
                          _messages[_currentMessageIndex]['emoji'],
                          key: ValueKey(_currentMessageIndex),
                          style: TextStyle(
                            fontSize: 40.sp,
                          ),
                        ),
                      ),
                    ),
                  ),
                ],
              ),
            ),
          ),
        );
      },
    );
  }
  
  Widget _buildMessageDisplay() {
    return AnimatedBuilder(
      animation: _textRevealController,
      builder: (context, child) {
        return Opacity(
          opacity: _textFadeAnimation.value,
          child: Transform.translate(
            offset: Offset(0, _textSlideAnimation.value),
            child: Container(
              padding: EdgeInsets.symmetric(horizontal: 40.w),
              child: Column(
                children: [
                  if (_currentMessageIndex == 0) ...[
                    Text(
                      'Hello, ${widget.userName}',
                      style: TextStyle(
                        fontSize: 28.sp,
                        fontWeight: FontWeight.bold,
                        color: AppTheme.textPrimary,
                      ),
                    ),
                    SizedBox(height: 12.h),
                  ],
                  
                  Text(
                    _messages[_currentMessageIndex]['text'],
                    textAlign: TextAlign.center,
                    style: TextStyle(
                      fontSize: 18.sp,
                      fontWeight: FontWeight.w500,
                      color: AppTheme.textSecondary,
                      height: 1.4,
                    ),
                  ),
                ],
              ),
            ),
          ),
        );
      },
    );
  }
  
  Widget _buildProgressIndicator() {
    return AnimatedBuilder(
      animation: _progressAnimation,
      builder: (context, child) {
        return Container(
          width: 200.w,
          child: Column(
            children: [
              // Progress bar
              Container(
                height: 4.h,
                decoration: BoxDecoration(
                  borderRadius: BorderRadius.circular(2.r),
                  color: AppTheme.borderSecondary,
                ),
                child: ClipRRect(
                  borderRadius: BorderRadius.circular(2.r),
                  child: LinearProgressIndicator(
                    value: _progressAnimation.value,
                    backgroundColor: Colors.transparent,
                    valueColor: AlwaysStoppedAnimation<Color>(
                      AppTheme.affirmationGradient[0],
                    ),
                  ),
                ),
              ),
              
              SizedBox(height: 12.h),
              
              // Progress text
              Text(
                '${(_progressAnimation.value * 100).toInt()}% Complete',
                style: TextStyle(
                  fontSize: 12.sp,
                  color: AppTheme.textTertiary,
                  fontWeight: FontWeight.w500,
                ),
              ),
            ],
          ),
        );
      },
    );
  }
  
  List<Widget> _buildFloatingParticles() {
    return List.generate(6, (index) {
      return AnimatedBuilder(
        animation: _particleController,
        builder: (context, child) {
          final progress = (_particleController.value + index * 0.15) % 1.0;
          final opacity = (1.0 - progress) * 0.6;
          final scale = 0.3 + progress * 0.7;
          
          return Positioned(
            left: 30.w + (index % 3) * 100.w + math.sin(progress * math.pi * 2) * 20,
            top: 100.h + progress * 400.h,
            child: Opacity(
              opacity: opacity,
              child: Transform.scale(
                scale: scale,
                child: Container(
                  width: 8.w,
                  height: 8.w,
                  decoration: BoxDecoration(
                    shape: BoxShape.circle,
                    color: AppTheme.affirmationGradient[index % 2],
                  ),
                ),
              ),
            ),
          );
        },
      );
    });
  }
  
  @override
  void dispose() {
    _breathingController.dispose();
    _floatingController.dispose();
    _particleController.dispose();
    _textRevealController.dispose();
    _progressController.dispose();
    _shimmerController.dispose();
    super.dispose();
  }
}