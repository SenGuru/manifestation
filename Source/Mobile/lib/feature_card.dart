import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../colors.dart';

class FeatureCard extends StatefulWidget {
  final String title;
  final String subtitle;
  final IconData icon;
  final List<Color> gradientColors;
  final VoidCallback onTap;
  final bool isLocked;

  const FeatureCard({
    Key? key,
    required this.title,
    required this.subtitle,
    required this.icon,
    required this.gradientColors,
    required this.onTap,
    this.isLocked = false,
  }) : super(key: key);

  @override
  _FeatureCardState createState() => _FeatureCardState();
}

class _FeatureCardState extends State<FeatureCard>
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
                        color: AppColors.white.withOpacity(_opacityAnimation.value),
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
                        color: AppColors.white.withOpacity(0.2),
                        borderRadius: BorderRadius.circular(12.r),
                      ),
                      child: Icon(
                        widget.icon,
                        color: AppColors.white,
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
                              color: AppColors.white,
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
                              color: AppColors.white.withOpacity(0.8),
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
                            color: AppColors.white,
                          ),
                        ),
                        SizedBox(width: 6.w),
                        Icon(
                          Icons.arrow_forward,
                          color: AppColors.white,
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