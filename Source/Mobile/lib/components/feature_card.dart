import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../constants/app_theme.dart';

class FeatureCard extends StatefulWidget {
  final String title;
  final String subtitle;
  final IconData icon;
  final List<Color> gradientColors;
  final VoidCallback onTap;
  final bool isLocked;

  const FeatureCard({
    super.key,
    required this.title,
    required this.subtitle,
    required this.icon,
    required this.gradientColors,
    required this.onTap,
    this.isLocked = false,
  });

  @override
  _FeatureCardState createState() => _FeatureCardState();
}

class _FeatureCardState extends State<FeatureCard>
    with TickerProviderStateMixin {
  late AnimationController _floatController;
  late AnimationController _opacityController;
  late AnimationController _gradientController;
  late Animation<double> _floatAnimation;
  late Animation<double> _opacityAnimation;
  late Animation<double> _gradientAnimation;

  @override
  void initState() {
    super.initState();
    
    // Float animation
    _floatController = AnimationController(
      duration: Duration(seconds: 3),
      vsync: this,
    );
    _floatAnimation = Tween<double>(
      begin: -3.0,
      end: 3.0,
    ).animate(CurvedAnimation(
      parent: _floatController,
      curve: Curves.easeInOut,
    ));
    
    // Opacity animation
    _opacityController = AnimationController(
      duration: Duration(milliseconds: 2500),
      vsync: this,
    );
    _opacityAnimation = Tween<double>(
      begin: 0.05,
      end: 0.15,
    ).animate(CurvedAnimation(
      parent: _opacityController,
      curve: Curves.easeInOut,
    ));
    
    // Gradient flow animation - slow diagonal movement
    _gradientController = AnimationController(
      duration: Duration(seconds: 8),
      vsync: this,
    );
    _gradientAnimation = Tween<double>(
      begin: 0.0,
      end: 2.0,
    ).animate(CurvedAnimation(
      parent: _gradientController,
      curve: Curves.linear,
    ));
    
    // Start animations with slight random delay
    Future.delayed(Duration(milliseconds: (widget.title.hashCode % 1000)), () {
      if (mounted) {
        _floatController.repeat(reverse: true);
        _opacityController.repeat(reverse: true);
        _gradientController.repeat();
      }
    });
  }

  @override
  void dispose() {
    _floatController.dispose();
    _opacityController.dispose();
    _gradientController.dispose();
    super.dispose();
  }

  LinearGradient _buildSeamlessGradient() {
    final double progress = _gradientAnimation.value / 2.0; // Normalize to 0-1
    final double offset = -progress * 2.0; // Continuous flow
    
    // Create a longer gradient pattern that repeats seamlessly
    final List<Color> gradientColors = [];
    final List<double> gradientStops = [];
    
    // Generate 8 color stops for smooth seamless flow
    for (int i = 0; i < 8; i++) {
      final double stop = i / 7.0; // 0.0 to 1.0
      final double shiftedStop = (stop + offset) % 1.0;
      
      // Create wave pattern: color1 → color2 → color1 → color2
      final double wavePosition = shiftedStop * 4.0; // 0-4 range for 2 full waves
      final int colorIndex = (wavePosition.floor() % 2);
      final double t = wavePosition - wavePosition.floor();
      
      // Smooth interpolation between the two gradient colors
      final Color color = Color.lerp(
        widget.gradientColors[colorIndex],
        widget.gradientColors[(colorIndex + 1) % 2],
        t,
      )!;
      
      gradientColors.add(color);
      gradientStops.add(stop);
    }
    
    return LinearGradient(
      colors: gradientColors,
      stops: gradientStops,
      begin: Alignment(-1.5, -1.5),
      end: Alignment(1.5, 1.5),
    );
  }

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: () {
        HapticFeedback.lightImpact();
        widget.onTap();
      },
      child: AnimatedBuilder(
        animation: _gradientAnimation,
        builder: (context, child) {
          return Container(
            height: 180.h,
            decoration: BoxDecoration(
              borderRadius: BorderRadius.circular(20.r),
              gradient: _buildSeamlessGradient(),
              boxShadow: [
                BoxShadow(
                  color: widget.gradientColors[0].withValues(alpha: 0.3),
                  blurRadius: 12.r,
                  offset: Offset(0, 6.h),
                ),
              ],
            ),
        child: ClipRRect(
          borderRadius: BorderRadius.circular(20.r),
          child: Stack(
            children: [
              // Animated background bubble
              AnimatedBuilder(
                animation: Listenable.merge([_floatController, _opacityController]),
                builder: (context, child) {
                  return Positioned(
                    top: -20.h + _floatAnimation.value,
                    right: -20.w,
                    child: Container(
                      width: 80.w,
                      height: 80.h,
                      decoration: BoxDecoration(
                        shape: BoxShape.circle,
                        color: AppTheme.white.withValues(alpha: _opacityAnimation.value),
                      ),
                    ),
                  );
                },
              ),
              
              // Main content
              Padding(
                padding: EdgeInsets.all(16.w),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: [
                    // Icon container
                    Container(
                      width: 44.w,
                      height: 44.h,
                      decoration: BoxDecoration(
                        color: AppTheme.white.withValues(alpha: 0.2),
                        borderRadius: BorderRadius.circular(12.r),
                      ),
                      child: Icon(
                        widget.icon,
                        color: AppTheme.white,
                        size: 22.sp,
                      ),
                    ),
                    
                    // Text content
                    Flexible(
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        mainAxisSize: MainAxisSize.min,
                        children: [
                          Text(
                            widget.title,
                            style: TextStyle(
                              fontSize: 16.sp,
                              fontWeight: FontWeight.bold,
                              color: AppTheme.white,
                              height: 1.2,
                            ),
                            maxLines: 2,
                            overflow: TextOverflow.ellipsis,
                          ),
                          SizedBox(height: 2.h),
                          Text(
                            widget.subtitle,
                            style: TextStyle(
                              fontSize: 12.sp,
                              fontWeight: FontWeight.w400,
                              color: AppTheme.white.withValues(alpha: 0.8),
                              height: 1.2,
                            ),
                            maxLines: 1,
                            overflow: TextOverflow.ellipsis,
                          ),
                        ],
                      ),
                    ),
                    
                    // Action button
                    Row(
                      mainAxisSize: MainAxisSize.min,
                      children: [
                        Text(
                          'Start',
                          style: TextStyle(
                            fontSize: 12.sp,
                            fontWeight: FontWeight.w600,
                            color: AppTheme.white,
                          ),
                        ),
                        SizedBox(width: 6.w),
                        Icon(
                          Icons.arrow_forward,
                          color: AppTheme.white,
                          size: 14.sp,
                        ),
                      ],
                    ),
                  ],
                ),
              ),
              
              // Lock overlay for premium features (if needed later)
              if (widget.isLocked)
                Positioned.fill(
                  child: Container(
                    decoration: BoxDecoration(
                      borderRadius: BorderRadius.circular(20.r),
                      color: AppTheme.black.withValues(alpha: 0.4),
                    ),
                    child: Center(
                      child: Column(
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: [
                          Icon(
                            Icons.lock,
                            color: AppTheme.white,
                            size: 32.sp,
                          ),
                          SizedBox(height: 8.h),
                          Text(
                            'Premium',
                            style: TextStyle(
                              color: AppTheme.white,
                              fontSize: 16.sp,
                              fontWeight: FontWeight.w600,
                            ),
                          ),
                        ],
                      ),
                    ),
                  ),
                ),
            ],
          ),
        ),
          );
        },
      ),
    );
  }
}