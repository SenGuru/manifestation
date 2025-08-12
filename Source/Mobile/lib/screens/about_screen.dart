import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'dart:math' as math;
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
  late AnimationController _scaleController;
  late AnimationController _rotationController;
  late AnimationController _shimmerController;
  late AnimationController _particleController;
  
  late Animation<double> _fadeAnimation;
  late Animation<Offset> _slideAnimation;
  late Animation<double> _scaleAnimation;
  late Animation<double> _rotationAnimation;
  late Animation<double> _shimmerAnimation;
  
  final List<AnimationController> _itemControllers = [];
  final List<Animation<double>> _itemAnimations = [];
  
  String _appVersion = '';
  String _buildNumber = '';
  bool _isLoading = true;
  bool _showParticles = false;

  @override
  void initState() {
    super.initState();
    _initializeAnimations();
    _loadAppInfo();
    
    // Add haptic feedback on init
    HapticFeedback.lightImpact();
  }

  void _initializeAnimations() {
    // Main animations
    _fadeController = AnimationController(
      duration: Duration(milliseconds: 1200),
      vsync: this,
    );
    
    _slideController = AnimationController(
      duration: Duration(milliseconds: 1400),
      vsync: this,
    );
    
    _scaleController = AnimationController(
      duration: Duration(milliseconds: 1600),
      vsync: this,
    );
    
    _rotationController = AnimationController(
      duration: Duration(seconds: 20),
      vsync: this,
    )..repeat();
    
    _shimmerController = AnimationController(
      duration: Duration(seconds: 2),
      vsync: this,
    )..repeat();
    
    _particleController = AnimationController(
      duration: Duration(seconds: 4),
      vsync: this,
    )..repeat();
    
    // Animation definitions
    _fadeAnimation = Tween<double>(
      begin: 0.0,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _fadeController,
      curve: Curves.easeOut,
    ));
    
    _slideAnimation = Tween<Offset>(
      begin: Offset(0, 0.5),
      end: Offset.zero,
    ).animate(CurvedAnimation(
      parent: _slideController,
      curve: Curves.easeOutQuart,
    ));
    
    _scaleAnimation = Tween<double>(
      begin: 0.8,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _scaleController,
      curve: Curves.elasticOut,
    ));
    
    _rotationAnimation = Tween<double>(
      begin: 0,
      end: 2 * math.pi,
    ).animate(CurvedAnimation(
      parent: _rotationController,
      curve: Curves.linear,
    ));
    
    _shimmerAnimation = Tween<double>(
      begin: 0,
      end: 1,
    ).animate(CurvedAnimation(
      parent: _shimmerController,
      curve: Curves.easeInOut,
    ));
    
    // Initialize item animations (for staggered list effects)
    for (int i = 0; i < 10; i++) {
      final controller = AnimationController(
        duration: Duration(milliseconds: 600 + (i * 100)),
        vsync: this,
      );
      _itemControllers.add(controller);
      
      _itemAnimations.add(
        Tween<double>(
          begin: 0,
          end: 1,
        ).animate(CurvedAnimation(
          parent: controller,
          curve: Interval(
            0.1 * i,
            0.3 + (0.1 * i),
            curve: Curves.easeOutCubic,
          ),
        )),
      );
    }
    
    // Start animations with cascade effect
    Future.delayed(Duration(milliseconds: 100), () {
      if (mounted) {
        _fadeController.forward();
        Future.delayed(Duration(milliseconds: 200), () {
          _slideController.forward();
          _scaleController.forward();
        });
        
        // Stagger item animations
        for (int i = 0; i < _itemControllers.length; i++) {
          Future.delayed(Duration(milliseconds: 400 + (i * 50)), () {
            if (mounted) {
              _itemControllers[i].forward();
            }
          });
        }
        
        // Enable particles after initial load
        Future.delayed(Duration(milliseconds: 800), () {
          if (mounted) {
            setState(() {
              _showParticles = true;
            });
          }
        });
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
    _scaleController.dispose();
    _rotationController.dispose();
    _shimmerController.dispose();
    _particleController.dispose();
    for (final controller in _itemControllers) {
      controller.dispose();
    }
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppTheme.backgroundPrimary,
      body: Stack(
        children: [
          // Animated background gradient
          _buildAnimatedBackground(),
          
          // Floating particles
          if (_showParticles) _buildParticleEffect(),
          
          // Main content
          SafeArea(
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
                      child: ScaleTransition(
                        scale: _scaleAnimation,
                        child: Column(
                          children: [
                            _buildHeroSection(),
                            SizedBox(height: 48.h),
                            _buildInfoSection(),
                            SizedBox(height: 32.h),
                            _buildStatsSection(),
                            SizedBox(height: 32.h),
                            _buildTeamSection(),
                            SizedBox(height: 32.h),
                            _buildLegalSection(),
                            SizedBox(height: 32.h),
                            _buildFooter(),
                            SizedBox(height: 40.h),
                          ],
                        ),
                      ),
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

  Widget _buildAnimatedBackground() {
    return AnimatedBuilder(
      animation: _rotationAnimation,
      builder: (context, child) {
        return Container(
          decoration: BoxDecoration(
            gradient: LinearGradient(
              colors: [
                AppTheme.backgroundPrimary,
                AppTheme.affirmationGradient[0].withOpacity(0.05),
                AppTheme.affirmationGradient[1].withOpacity(0.05),
                AppTheme.backgroundPrimary,
              ],
              begin: Alignment(-1 + 0.3 * math.sin(_rotationAnimation.value), -1 + 0.3 * math.cos(_rotationAnimation.value)),
              end: Alignment(1 + 0.3 * math.sin(_rotationAnimation.value), 1 + 0.3 * math.cos(_rotationAnimation.value)),
              stops: const [0.0, 0.3, 0.7, 1.0],
            ),
          ),
        );
      },
    );
  }
  
  Widget _buildParticleEffect() {
    return AnimatedBuilder(
      animation: _particleController,
      builder: (context, child) {
        return CustomPaint(
          painter: ParticlePainter(_particleController.value),
          size: Size.infinite,
        );
      },
    );
  }
  
  Widget _buildHeroSection() {
    return AnimatedBuilder(
      animation: _shimmerAnimation,
      builder: (context, child) {
        return Container(
          width: double.infinity,
          padding: EdgeInsets.all(32.w),
          decoration: BoxDecoration(
            gradient: LinearGradient(
              colors: AppTheme.affirmationGradient,
              begin: Alignment(
                -1 + 0.5 * math.sin(_shimmerAnimation.value * 2 * math.pi),
                -1 + 0.5 * math.cos(_shimmerAnimation.value * 2 * math.pi),
              ),
              end: Alignment(
                1 + 0.5 * math.sin(_shimmerAnimation.value * 2 * math.pi + math.pi),
                1 + 0.5 * math.cos(_shimmerAnimation.value * 2 * math.pi + math.pi),
              ),
            ),
            borderRadius: BorderRadius.circular(32.r),
            boxShadow: [
              BoxShadow(
                color: AppTheme.affirmationGradient[0].withOpacity(0.4),
                blurRadius: 30.r,
                offset: Offset(0, 15.h),
              ),
              BoxShadow(
                color: AppTheme.affirmationGradient[1].withOpacity(0.3),
                blurRadius: 20.r,
                offset: Offset(-10.w, -5.h),
              ),
            ],
          ),
          child: Column(
            children: [
              // Animated App Icon
              AnimatedBuilder(
                animation: _rotationAnimation,
                builder: (context, child) {
                  return Transform(
                    alignment: Alignment.center,
                    transform: Matrix4.identity()
                      ..setEntry(3, 2, 0.001)
                      ..rotateY(_rotationAnimation.value * 0.5),
                    child: Container(
                      width: 100.w,
                      height: 100.h,
                      decoration: BoxDecoration(
                        gradient: LinearGradient(
                          colors: [
                            AppTheme.white.withOpacity(0.3),
                            AppTheme.white.withOpacity(0.1),
                          ],
                          begin: Alignment.topLeft,
                          end: Alignment.bottomRight,
                        ),
                        borderRadius: BorderRadius.circular(24.r),
                        border: Border.all(
                          color: AppTheme.white.withOpacity(0.4),
                          width: 2,
                        ),
                        boxShadow: [
                          BoxShadow(
                            color: AppTheme.white.withOpacity(0.3),
                            blurRadius: 20.r,
                            spreadRadius: 5.r,
                          ),
                        ],
                      ),
                      child: Icon(
                        Icons.auto_awesome_rounded,
                        size: 50.sp,
                        color: AppTheme.white,
                      ),
                    ),
                  );
                },
              ),
              
              SizedBox(height: 24.h),
              
              // App Name with gradient text
              ShaderMask(
                blendMode: BlendMode.srcIn,
                shaderCallback: (bounds) => LinearGradient(
                  colors: [
                    AppTheme.white,
                    AppTheme.white.withOpacity(0.8),
                  ],
                  begin: Alignment.topCenter,
                  end: Alignment.bottomCenter,
                ).createShader(bounds),
                child: Text(
                  'Mistel',
                  style: TextStyle(
                    fontSize: 32.sp,
                    fontWeight: FontWeight.bold,
                    letterSpacing: 2,
                    shadows: [
                      Shadow(
                        color: AppTheme.white.withOpacity(0.5),
                        blurRadius: 10,
                        offset: Offset(0, 2),
                      ),
                    ],
                  ),
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
      },
    );
  }

  Widget _buildInfoSection() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        if (_itemAnimations.isNotEmpty)
          FadeTransition(
            opacity: _itemAnimations[0],
            child: Text(
              'Features That Transform',
              style: TextStyle(
                fontSize: 24.sp,
                fontWeight: FontWeight.bold,
                color: AppTheme.textPrimary,
                letterSpacing: 0.5,
              ),
            ),
          ),
        
        SizedBox(height: 20.h),
        
        if (_itemAnimations.length > 1)
          FadeTransition(
            opacity: _itemAnimations[1],
            child: _buildEnhancedFeatureCard(
              icon: Icons.psychology_outlined,
              title: 'Mindful Practice',
              description: 'Daily affirmations, gratitude journaling, and intention setting to align your thoughts with your goals.',
              gradient: AppTheme.affirmationGradient,
              index: 0,
            ),
          ),
        
        SizedBox(height: 16.h),
        
        if (_itemAnimations.length > 2)
          FadeTransition(
            opacity: _itemAnimations[2],
            child: _buildEnhancedFeatureCard(
              icon: Icons.track_changes_outlined,
              title: 'Goal Tracking',
              description: 'Set meaningful goals, track your progress, and celebrate your achievements along the way.',
              gradient: AppTheme.affirmationGradient,
              index: 1,
            ),
          ),
        
        SizedBox(height: 16.h),
        
        if (_itemAnimations.length > 3)
          FadeTransition(
            opacity: _itemAnimations[3],
            child: _buildEnhancedFeatureCard(
              icon: Icons.insights_outlined,
              title: 'Personal Growth',
              description: 'Gain insights into your patterns, build positive habits, and create lasting change.',
              gradient: AppTheme.affirmationGradient,
              index: 2,
            ),
          ),
      ],
    );
  }
  
  Widget _buildEnhancedFeatureCard({
    required IconData icon,
    required String title,
    required String description,
    required List<Color> gradient,
    required int index,
  }) {
    return GestureDetector(
      onTap: () {
        HapticFeedback.lightImpact();
      },
      child: AnimatedContainer(
        duration: Duration(milliseconds: 300),
        curve: Curves.easeOut,
        padding: EdgeInsets.all(20.w),
        decoration: BoxDecoration(
          color: AppTheme.cardBackground,
          borderRadius: BorderRadius.circular(20.r),
          border: Border.all(
            color: gradient[0].withOpacity(0.2),
            width: 1.5,
          ),
          boxShadow: [
            BoxShadow(
              color: gradient[0].withOpacity(0.1),
              blurRadius: 20.r,
              offset: Offset(0, 8.h),
            ),
          ],
        ),
        child: Row(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Hero(
              tag: 'feature_icon_$index',
              child: Container(
                width: 56.w,
                height: 56.h,
                decoration: BoxDecoration(
                  gradient: LinearGradient(
                    colors: gradient,
                    begin: Alignment.topLeft,
                    end: Alignment.bottomRight,
                  ),
                  borderRadius: BorderRadius.circular(16.r),
                  boxShadow: [
                    BoxShadow(
                      color: gradient[0].withOpacity(0.3),
                      blurRadius: 12.r,
                      offset: Offset(0, 6.h),
                    ),
                  ],
                ),
                child: Icon(
                  icon,
                  color: AppTheme.white,
                  size: 28.sp,
                ),
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
                      fontSize: 18.sp,
                      fontWeight: FontWeight.bold,
                      color: AppTheme.textPrimary,
                    ),
                  ),
                  SizedBox(height: 8.h),
                  Text(
                    description,
                    style: TextStyle(
                      fontSize: 14.sp,
                      color: AppTheme.textSecondary,
                      height: 1.5,
                    ),
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }
  
  Widget _buildStatsSection() {
    return Container(
      padding: EdgeInsets.all(24.w),
      decoration: BoxDecoration(
        gradient: LinearGradient(
          colors: [
            AppTheme.cardBackground,
            AppTheme.backgroundSecondary,
          ],
          begin: Alignment.topLeft,
          end: Alignment.bottomRight,
        ),
        borderRadius: BorderRadius.circular(24.r),
        border: Border.all(
          color: AppTheme.borderPrimary,
        ),
      ),
      child: Column(
        children: [
          Text(
            'Join Our Growing Community',
            style: TextStyle(
              fontSize: 20.sp,
              fontWeight: FontWeight.bold,
              color: AppTheme.textPrimary,
            ),
          ),
          SizedBox(height: 24.h),
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceEvenly,
            children: [
              _buildStatItem('10K+', 'Active Users', Icons.people_outline),
              _buildStatItem('50K+', 'Goals Set', Icons.flag_outlined),
              _buildStatItem('4.8', 'App Rating', Icons.star_outline),
            ],
          ),
        ],
      ),
    );
  }
  
  Widget _buildStatItem(String value, String label, IconData icon) {
    return Column(
      children: [
        Icon(
          icon,
          color: AppTheme.affirmationGradient[0],
          size: 32.sp,
        ),
        SizedBox(height: 8.h),
        Text(
          value,
          style: TextStyle(
            fontSize: 24.sp,
            fontWeight: FontWeight.bold,
            color: AppTheme.textPrimary,
          ),
        ),
        Text(
          label,
          style: TextStyle(
            fontSize: 12.sp,
            color: AppTheme.textSecondary,
          ),
        ),
      ],
    );
  }

  Widget _buildFooter() {
    return Container(
      padding: EdgeInsets.all(24.w),
      decoration: BoxDecoration(
        color: AppTheme.cardBackground.withOpacity(0.5),
        borderRadius: BorderRadius.circular(20.r),
      ),
      child: Column(
        children: [
          Text(
            'Made with ❤️ in California',
            style: TextStyle(
              fontSize: 16.sp,
              color: AppTheme.textSecondary,
            ),
          ),
          SizedBox(height: 8.h),
          Text(
            '© 2025 Mistel. All rights reserved.',
            style: TextStyle(
              fontSize: 14.sp,
              color: AppTheme.textTertiary,
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

// Custom painter for particle effects
class ParticlePainter extends CustomPainter {
  final double animationValue;
  final List<Particle> particles = [];
  
  ParticlePainter(this.animationValue) {
    // Generate particles
    for (int i = 0; i < 30; i++) {
      particles.add(Particle(
        x: math.Random().nextDouble(),
        y: math.Random().nextDouble(),
        size: math.Random().nextDouble() * 3 + 1,
        speed: math.Random().nextDouble() * 0.5 + 0.5,
        opacity: math.Random().nextDouble() * 0.3 + 0.1,
      ));
    }
  }
  
  @override
  void paint(Canvas canvas, Size size) {
    final paint = Paint()
      ..style = PaintingStyle.fill;
    
    for (final particle in particles) {
      final y = (particle.y - animationValue * particle.speed) % 1.0;
      final adjustedY = y < 0 ? y + 1 : y;
      
      paint.color = AppTheme.affirmationGradient[0].withOpacity(particle.opacity);
      
      canvas.drawCircle(
        Offset(particle.x * size.width, adjustedY * size.height),
        particle.size,
        paint,
      );
    }
  }
  
  @override
  bool shouldRepaint(ParticlePainter oldDelegate) => true;
}

class Particle {
  final double x;
  final double y;
  final double size;
  final double speed;
  final double opacity;
  
  Particle({
    required this.x,
    required this.y,
    required this.size,
    required this.speed,
    required this.opacity,
  });
}