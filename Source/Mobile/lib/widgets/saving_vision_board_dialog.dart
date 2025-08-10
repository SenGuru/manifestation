import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../constants/app_theme.dart';

class SavingVisionBoardDialog extends StatefulWidget {
  final String? message;
  final String? boardName;

  const SavingVisionBoardDialog({
    Key? key,
    this.message,
    this.boardName,
  }) : super(key: key);

  @override
  _SavingVisionBoardDialogState createState() => _SavingVisionBoardDialogState();
}

class _SavingVisionBoardDialogState extends State<SavingVisionBoardDialog>
    with TickerProviderStateMixin {
  late AnimationController _pulseController;
  late AnimationController _glowController;
  late AnimationController _shimmerController;
  late Animation<double> _pulseAnimation;
  late Animation<double> _glowAnimation;
  late Animation<double> _shimmerAnimation;

  @override
  void initState() {
    super.initState();
    
    // Gentle pulsing animation
    _pulseController = AnimationController(
      duration: const Duration(milliseconds: 1500),
      vsync: this,
    );
    
    // Subtle glow animation
    _glowController = AnimationController(
      duration: const Duration(milliseconds: 2000),
      vsync: this,
    );
    
    // Shimmer animation for text
    _shimmerController = AnimationController(
      duration: const Duration(milliseconds: 3000),
      vsync: this,
    );

    _pulseAnimation = Tween<double>(
      begin: 0.85,
      end: 1.15,
    ).animate(CurvedAnimation(
      parent: _pulseController,
      curve: Curves.easeInOut,
    ));

    _glowAnimation = Tween<double>(
      begin: 0.4,
      end: 0.8,
    ).animate(CurvedAnimation(
      parent: _glowController,
      curve: Curves.easeInOut,
    ));

    _shimmerAnimation = Tween<double>(
      begin: 0.0,
      end: 360.0,
    ).animate(CurvedAnimation(
      parent: _shimmerController,
      curve: Curves.linear,
    ));

    _pulseController.repeat(reverse: true);
    _glowController.repeat(reverse: true);
    _shimmerController.repeat();
  }

  @override
  void dispose() {
    _pulseController.dispose();
    _glowController.dispose();
    _shimmerController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Dialog(
      backgroundColor: Colors.transparent,
      elevation: 0,
      child: Container(
        padding: EdgeInsets.all(32.w),
        decoration: BoxDecoration(
          color: AppTheme.cardBackground,
          borderRadius: BorderRadius.circular(20.r),
          boxShadow: [
            BoxShadow(
              color: Colors.black.withValues(alpha: 0.3),
              blurRadius: 20.r,
              offset: Offset(0, 10.h),
            ),
          ],
        ),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            // Vision board saving icon with animations
            AnimatedBuilder(
              animation: Listenable.merge([_pulseAnimation, _glowAnimation]),
              builder: (context, child) {
                return SizedBox(
                  width: 100.w,
                  height: 100.h,
                  child: Stack(
                    alignment: Alignment.center,
                    children: [
                      // Outer glow effect
                      Container(
                        width: 180.w,
                        height: 180.h,
                        decoration: BoxDecoration(
                          shape: BoxShape.circle,
                          gradient: RadialGradient(
                            colors: [
                              AppTheme.visionBoardGradient[0].withValues(alpha: _glowAnimation.value * 0.15),
                              AppTheme.visionBoardGradient[0].withValues(alpha: 0),
                            ],
                          ),
                        ),
                      ),
                      // Middle glow circle
                      Container(
                        width: 130.w,
                        height: 130.h,
                        decoration: BoxDecoration(
                          shape: BoxShape.circle,
                          gradient: RadialGradient(
                            colors: [
                              AppTheme.visionBoardGradient[1].withValues(alpha: _glowAnimation.value * 0.2),
                              AppTheme.visionBoardGradient[1].withValues(alpha: 0),
                            ],
                          ),
                        ),
                      ),
                      // Main icon background circle
                      Transform.scale(
                        scale: _pulseAnimation.value,
                        child: Container(
                          width: 75.w,
                          height: 75.h,
                          decoration: BoxDecoration(
                            shape: BoxShape.circle,
                            gradient: LinearGradient(
                              colors: AppTheme.visionBoardGradient,
                              begin: Alignment.topLeft,
                              end: Alignment.bottomRight,
                            ),
                            boxShadow: [
                              BoxShadow(
                                color: AppTheme.visionBoardGradient[0].withValues(alpha: 0.4),
                                blurRadius: 20.r,
                                spreadRadius: 1.r,
                              ),
                            ],
                          ),
                          child: Icon(
                            Icons.dashboard,
                            color: AppTheme.white,
                            size: 35.sp,
                          ),
                        ),
                      ),
                    ],
                  ),
                );
              },
            ),
            
            SizedBox(height: 32.h),
            
            // Saving message with shimmer effect
            AnimatedBuilder(
              animation: Listenable.merge([_glowAnimation, _shimmerController]),
              builder: (context, child) {
                final double progress = _shimmerAnimation.value / 360.0;
                
                return ShaderMask(
                  shaderCallback: (bounds) {
                    final double offset = -progress * 2.0;
                    
                    final List<Color> baseColors = [
                      AppTheme.visionBoardGradient[0],
                      AppTheme.visionBoardGradient[1],
                      AppTheme.white,
                    ];
                    
                    final List<Color> gradientColors = [];
                    final List<double> gradientStops = [];
                    
                    for (int i = 0; i < 6; i++) {
                      final double stop = i / 5.0;
                      final double shiftedStop = (stop + offset) % 1.0;
                      final double colorPosition = shiftedStop * 3.0;
                      final int colorIndex = colorPosition.floor() % 3;
                      final int nextColorIndex = (colorIndex + 1) % 3;
                      final double t = colorPosition - colorPosition.floor();
                      
                      final Color color = Color.lerp(
                        baseColors[colorIndex],
                        baseColors[nextColorIndex],
                        t,
                      )!;
                      
                      gradientColors.add(color);
                      gradientStops.add(stop);
                    }
                    
                    return LinearGradient(
                      colors: gradientColors,
                      stops: gradientStops,
                      begin: Alignment.centerLeft,
                      end: Alignment.centerRight,
                    ).createShader(bounds);
                  },
                  child: Text(
                    widget.message ?? 'Saving Vision Board...',
                    style: TextStyle(
                      fontSize: 18.sp,
                      color: AppTheme.white,
                      fontWeight: FontWeight.w600,
                      letterSpacing: 1.2,
                      height: 1.2,
                      shadows: [
                        Shadow(
                          color: AppTheme.visionBoardGradient[0].withValues(alpha: _glowAnimation.value * 0.3),
                          blurRadius: 8.r,
                          offset: Offset(0, 1.h),
                        ),
                        Shadow(
                          color: AppTheme.visionBoardGradient[1].withValues(alpha: _glowAnimation.value * 0.2),
                          blurRadius: 12.r,
                          offset: Offset(0, 2.h),
                        ),
                      ],
                    ),
                    textAlign: TextAlign.center,
                  ),
                );
              },
            ),
            
            // Board name if provided
            if (widget.boardName != null && widget.boardName!.isNotEmpty) ...[
              SizedBox(height: 12.h),
              Text(
                '"${widget.boardName}"',
                style: TextStyle(
                  fontSize: 14.sp,
                  color: AppTheme.textSecondary,
                  fontStyle: FontStyle.italic,
                ),
                textAlign: TextAlign.center,
                maxLines: 2,
                overflow: TextOverflow.ellipsis,
              ),
            ],
          ],
        ),
      ),
    );
  }

  static Future<void> show(
    BuildContext context, {
    String? message,
    String? boardName,
  }) {
    return showDialog(
      context: context,
      barrierDismissible: false,
      builder: (context) => SavingVisionBoardDialog(
        message: message,
        boardName: boardName,
      ),
    );
  }

  static void hide(BuildContext context) {
    Navigator.of(context).pop();
  }
}