import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:fun/constants/colors.dart';

class WideFeatureCard extends StatefulWidget {
  final String title;
  final String subtitle;
  final IconData icon;
  final List<Color> gradientColors;
  final VoidCallback onTap;
  final bool isLocked;

  const WideFeatureCard({
    Key? key,
    required this.title,
    required this.subtitle,
    required this.icon,
    required this.gradientColors,
    required this.onTap,
    this.isLocked = false,
  }) : super(key: key);

  @override
  _WideFeatureCardState createState() => _WideFeatureCardState();
}

class _WideFeatureCardState extends State<WideFeatureCard>
    with TickerProviderStateMixin {
  late AnimationController _floatController;
  late AnimationController _opacityController;
  late Animation<double> _floatAnimation;
  late Animation<double> _opacityAnimation;

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
    
    // Start animations with slight random delay
    Future.delayed(Duration(milliseconds: (widget.title.hashCode % 1000)), () {
      if (mounted) {
        _floatController.repeat(reverse: true);
        _opacityController.repeat(reverse: true);
      }
    });
  }

  @override
  void dispose() {
    _floatController.dispose();
    _opacityController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: widget.onTap,
      child: Container(
        width: double.infinity,
        height: 180.h,
        decoration: BoxDecoration(
          borderRadius: BorderRadius.circular(20.r),
          gradient: LinearGradient(
            colors: widget.gradientColors,
            begin: Alignment.topLeft,
            end: Alignment.bottomRight,
          ),
          boxShadow: [
            BoxShadow(
              color: widget.gradientColors[0].withOpacity(0.3),
              blurRadius: 12.r,
              offset: Offset(0, 6.h),
            ),
          ],
        ),
        child: ClipRRect(
          borderRadius: BorderRadius.circular(20.r),
          child: Stack(
            children: [
              // Multiple animated background bubbles for wider card
              AnimatedBuilder(
                animation: Listenable.merge([_floatController, _opacityController]),
                builder: (context, child) {
                  return Stack(
                    children: [
                      // First bubble
                      Positioned(
                        top: -20.h + _floatAnimation.value,
                        right: -20.w,
                        child: Container(
                          width: 80.w,
                          height: 80.h,
                          decoration: BoxDecoration(
                            shape: BoxShape.circle,
                            color: AppColors.white.withOpacity(_opacityAnimation.value),
                          ),
                        ),
                      ),
                      // Second bubble (offset animation)
                      Positioned(
                        top: -30.h - _floatAnimation.value * 0.5,
                        left: -30.w,
                        child: Container(
                          width: 60.w,
                          height: 60.h,
                          decoration: BoxDecoration(
                            shape: BoxShape.circle,
                            color: AppColors.white.withOpacity(_opacityAnimation.value * 0.7),
                          ),
                        ),
                      ),
                      // Third bubble
                      Positioned(
                        bottom: -25.h + _floatAnimation.value * 0.8,
                        right: 60.w,
                        child: Container(
                          width: 50.w,
                          height: 50.h,
                          decoration: BoxDecoration(
                            shape: BoxShape.circle,
                            color: AppColors.white.withOpacity(_opacityAnimation.value * 0.5),
                          ),
                        ),
                      ),
                    ],
                  );
                },
              ),
              
              // Main content
              Padding(
                padding: EdgeInsets.all(20.w),
                child: Row(
                  children: [
                    // Left side - Icon and text content
                    Expanded(
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        children: [
                          // Icon container
                          Container(
                            width: 50.w,
                            height: 50.h,
                            decoration: BoxDecoration(
                              color: AppColors.white.withOpacity(0.2),
                              borderRadius: BorderRadius.circular(14.r),
                            ),
                            child: Icon(
                              widget.icon,
                              color: AppColors.white,
                              size: 26.sp,
                            ),
                          ),
                          
                          // Text content
                          Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              Text(
                                widget.title,
                                style: TextStyle(
                                  fontSize: 22.sp,
                                  fontWeight: FontWeight.bold,
                                  color: AppColors.white,
                                  height: 1.2,
                                ),
                              ),
                              SizedBox(height: 4.h),
                              Text(
                                widget.subtitle,
                                style: TextStyle(
                                  fontSize: 14.sp,
                                  fontWeight: FontWeight.w400,
                                  color: AppColors.white.withOpacity(0.8),
                                  height: 1.2,
                                ),
                              ),
                            ],
                          ),
                        ],
                      ),
                    ),
                    
                    // Right side - Action area
                    Column(
                      mainAxisAlignment: MainAxisAlignment.end,
                      children: [
                        Container(
                          padding: EdgeInsets.symmetric(
                            horizontal: 16.w,
                            vertical: 8.h,
                          ),
                          decoration: BoxDecoration(
                            color: AppColors.white.withOpacity(0.2),
                            borderRadius: BorderRadius.circular(20.r),
                          ),
                          child: Row(
                            mainAxisSize: MainAxisSize.min,
                            children: [
                              Text(
                                'Connect',
                                style: TextStyle(
                                  fontSize: 14.sp,
                                  fontWeight: FontWeight.w600,
                                  color: AppColors.white,
                                ),
                              ),
                              SizedBox(width: 8.w),
                              Icon(
                                Icons.arrow_forward,
                                color: AppColors.white,
                                size: 16.sp,
                              ),
                            ],
                          ),
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
                      color: AppColors.black.withOpacity(0.4),
                    ),
                    child: Center(
                      child: Column(
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: [
                          Icon(
                            Icons.lock,
                            color: AppColors.white,
                            size: 32.sp,
                          ),
                          SizedBox(height: 8.h),
                          Text(
                            'Premium',
                            style: TextStyle(
                              color: AppColors.white,
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
      ),
    );
  }
}