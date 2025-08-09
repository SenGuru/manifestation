import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:mistel/constants/colors.dart';
import '../constants/app_theme.dart';

class DualFeatureCard extends StatefulWidget {
  final String leftTitle;
  final String leftSubtitle;
  final IconData leftIcon;
  final List<Color> leftGradientColors;
  final VoidCallback leftOnTap;
  final bool leftIsLocked;
  
  final String rightTitle;
  final String rightSubtitle;
  final IconData rightIcon;
  final List<Color> rightGradientColors;
  final VoidCallback rightOnTap;
  final bool rightIsLocked;

  const DualFeatureCard({
    Key? key,
    required this.leftTitle,
    required this.leftSubtitle,
    required this.leftIcon,
    required this.leftGradientColors,
    required this.leftOnTap,
    this.leftIsLocked = false,
    required this.rightTitle,
    required this.rightSubtitle,
    required this.rightIcon,
    required this.rightGradientColors,
    required this.rightOnTap,
    this.rightIsLocked = false,
  }) : super(key: key);

  @override
  _DualFeatureCardState createState() => _DualFeatureCardState();
}

class _DualFeatureCardState extends State<DualFeatureCard>
    with TickerProviderStateMixin {
  late AnimationController _leftFloatController;
  late AnimationController _leftOpacityController;
  late Animation<double> _leftFloatAnimation;
  late Animation<double> _leftOpacityAnimation;
  
  late AnimationController _rightFloatController;
  late AnimationController _rightOpacityController;
  late Animation<double> _rightFloatAnimation;
  late Animation<double> _rightOpacityAnimation;

  @override
  void initState() {
    super.initState();
    
    // Left side animations
    _leftFloatController = AnimationController(
      duration: Duration(seconds: 3),
      vsync: this,
    );
    _leftFloatAnimation = Tween<double>(
      begin: -3.0,
      end: 3.0,
    ).animate(CurvedAnimation(
      parent: _leftFloatController,
      curve: Curves.easeInOut,
    ));
    
    _leftOpacityController = AnimationController(
      duration: Duration(milliseconds: 2500),
      vsync: this,
    );
    _leftOpacityAnimation = Tween<double>(
      begin: 0.05,
      end: 0.15,
    ).animate(CurvedAnimation(
      parent: _leftOpacityController,
      curve: Curves.easeInOut,
    ));
    
    // Right side animations
    _rightFloatController = AnimationController(
      duration: Duration(seconds: 3),
      vsync: this,
    );
    _rightFloatAnimation = Tween<double>(
      begin: -3.0,
      end: 3.0,
    ).animate(CurvedAnimation(
      parent: _rightFloatController,
      curve: Curves.easeInOut,
    ));
    
    _rightOpacityController = AnimationController(
      duration: Duration(milliseconds: 2500),
      vsync: this,
    );
    _rightOpacityAnimation = Tween<double>(
      begin: 0.05,
      end: 0.15,
    ).animate(CurvedAnimation(
      parent: _rightOpacityController,
      curve: Curves.easeInOut,
    ));
    
    // Start animations with slight random delays
    Future.delayed(Duration(milliseconds: (widget.leftTitle.hashCode % 1000)), () {
      if (mounted) {
        _leftFloatController.repeat(reverse: true);
        _leftOpacityController.repeat(reverse: true);
      }
    });
    
    Future.delayed(Duration(milliseconds: (widget.rightTitle.hashCode % 1000)), () {
      if (mounted) {
        _rightFloatController.repeat(reverse: true);
        _rightOpacityController.repeat(reverse: true);
      }
    });
  }

  @override
  void dispose() {
    _leftFloatController.dispose();
    _leftOpacityController.dispose();
    _rightFloatController.dispose();
    _rightOpacityController.dispose();
    super.dispose();
  }

  Widget _buildFeatureHalf({
    required String title,
    required String subtitle,
    required IconData icon,
    required List<Color> gradientColors,
    required VoidCallback onTap,
    required bool isLocked,
    required Animation<double> floatAnimation,
    required Animation<double> opacityAnimation,
    required bool isLeft,
  }) {
    return GestureDetector(
        onTap: onTap,
        child: Container(
          height: 180.h,
          decoration: BoxDecoration(
            borderRadius: BorderRadius.horizontal(
              left: isLeft ? Radius.circular(20.r) : Radius.zero,
              right: !isLeft ? Radius.circular(20.r) : Radius.zero,
            ),
            gradient: LinearGradient(
              colors: gradientColors,
              begin: Alignment.topLeft,
              end: Alignment.bottomRight,
            ),
            boxShadow: [
              BoxShadow(
                color: gradientColors[0].withOpacity(0.2),
                blurRadius: 8.r,
                offset: Offset(isLeft ? -2 : 2, 4.h),
              ),
            ],
          ),
          child: ClipRRect(
            borderRadius: BorderRadius.horizontal(
              left: isLeft ? Radius.circular(20.r) : Radius.zero,
              right: !isLeft ? Radius.circular(20.r) : Radius.zero,
            ),
            child: Stack(
              children: [
                // Animated background bubble
                AnimatedBuilder(
                  animation: Listenable.merge([floatAnimation, opacityAnimation]),
                  builder: (context, child) {
                    return Positioned(
                      top: -20.h + floatAnimation.value,
                      right: isLeft ? -20.w : null,
                      left: !isLeft ? -20.w : null,
                      child: Container(
                        width: 60.w,
                        height: 60.h,
                        decoration: BoxDecoration(
                          shape: BoxShape.circle,
                          color: AppTheme.white.withOpacity(opacityAnimation.value),
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
                        width: 36.w,
                        height: 36.h,
                        decoration: BoxDecoration(
                          color: AppTheme.white.withOpacity(0.2),
                          borderRadius: BorderRadius.circular(10.r),
                        ),
                        child: Icon(
                          icon,
                          color: AppTheme.white,
                          size: 18.sp,
                        ),
                      ),
                      
                      // Text content
                      Flexible(
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          mainAxisSize: MainAxisSize.min,
                          children: [
                            Text(
                              title,
                              style: TextStyle(
                                fontSize: 14.sp,
                                fontWeight: FontWeight.bold,
                                color: AppTheme.white,
                                height: 1.2,
                              ),
                              maxLines: 2,
                              overflow: TextOverflow.ellipsis,
                            ),
                            SizedBox(height: 2.h),
                            Text(
                              subtitle,
                              style: TextStyle(
                                fontSize: 10.sp,
                                fontWeight: FontWeight.w400,
                                color: AppTheme.white.withOpacity(0.8),
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
                              fontSize: 10.sp,
                              fontWeight: FontWeight.w600,
                              color: AppTheme.white,
                            ),
                          ),
                          SizedBox(width: 4.w),
                          Icon(
                            Icons.arrow_forward,
                            color: AppTheme.white,
                            size: 12.sp,
                          ),
                        ],
                      ),
                    ],
                  ),
                ),
                
                // Lock overlay for premium features
                if (isLocked)
                  Positioned.fill(
                    child: Container(
                      decoration: BoxDecoration(
                        borderRadius: BorderRadius.horizontal(
                          left: isLeft ? Radius.circular(20.r) : Radius.zero,
                          right: !isLeft ? Radius.circular(20.r) : Radius.zero,
                        ),
                        color: AppTheme.black.withOpacity(0.4),
                      ),
                      child: Center(
                        child: Column(
                          mainAxisAlignment: MainAxisAlignment.center,
                          children: [
                            Icon(
                              Icons.lock,
                              color: AppTheme.white,
                              size: 24.sp,
                            ),
                            SizedBox(height: 4.h),
                            Text(
                              'Premium',
                              style: TextStyle(
                                color: AppTheme.white,
                                fontSize: 12.sp,
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

  @override
  Widget build(BuildContext context) {
    return Container(
      height: 180.h,
      child: Row(
        children: [
          // Left feature - takes up half the width
          Flexible(
            flex: 1,
            child: _buildFeatureHalf(
              title: widget.leftTitle,
              subtitle: widget.leftSubtitle,
              icon: widget.leftIcon,
              gradientColors: widget.leftGradientColors,
              onTap: widget.leftOnTap,
              isLocked: widget.leftIsLocked,
              floatAnimation: _leftFloatAnimation,
              opacityAnimation: _leftOpacityAnimation,
              isLeft: true,
            ),
          ),
          
          // Small gap between features
          SizedBox(width: 1.w),
          
          // Right feature - takes up half the width
          Flexible(
            flex: 1,
            child: _buildFeatureHalf(
              title: widget.rightTitle,
              subtitle: widget.rightSubtitle,
              icon: widget.rightIcon,
              gradientColors: widget.rightGradientColors,
              onTap: widget.rightOnTap,
              isLocked: widget.rightIsLocked,
              floatAnimation: _rightFloatAnimation,
              opacityAnimation: _rightOpacityAnimation,
              isLeft: false,
            ),
          ),
        ],
      ),
    );
  }
}