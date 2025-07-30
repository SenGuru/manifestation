import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../constants/colors.dart';

class StylishLoading extends StatefulWidget {
  final String? message;
  final double? size;
  final bool showMessage;

  const StylishLoading({
    Key? key,
    this.message,
    this.size,
    this.showMessage = true,
  }) : super(key: key);

  @override
  _StylishLoadingState createState() => _StylishLoadingState();
}

class _StylishLoadingState extends State<StylishLoading>
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
    
    // Hue rotation animation for text
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
      end: 360.0, // Full hue rotation (0-360 degrees)
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
    final loadingSize = widget.size ?? 80.w;
    
    return Center(
      child: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          // App logo with gentle pulsing and glow
          AnimatedBuilder(
            animation: Listenable.merge([_pulseAnimation, _glowAnimation]),
            builder: (context, child) {
              return SizedBox(
                width: loadingSize,
                height: loadingSize,
                child: Stack(
                  alignment: Alignment.center,
                  children: [
                    // Outer glow effect
                    Container(
                      width: loadingSize * 1.8,
                      height: loadingSize * 1.8,
                      decoration: BoxDecoration(
                        shape: BoxShape.circle,
                        gradient: RadialGradient(
                          colors: [
                            AppColors.affirmationGradient[0].withValues(alpha: _glowAnimation.value * 0.15),
                            AppColors.affirmationGradient[0].withValues(alpha: 0),
                          ],
                        ),
                      ),
                    ),
                    // Middle glow circle
                    Container(
                      width: loadingSize * 1.3,
                      height: loadingSize * 1.3,
                      decoration: BoxDecoration(
                        shape: BoxShape.circle,
                        gradient: RadialGradient(
                          colors: [
                            AppColors.affirmationGradient[1].withValues(alpha: _glowAnimation.value * 0.2),
                            AppColors.affirmationGradient[1].withValues(alpha: 0),
                          ],
                        ),
                      ),
                    ),
                    // Main logo background circle
                    Transform.scale(
                      scale: _pulseAnimation.value,
                      child: Container(
                        width: loadingSize * 0.75,
                        height: loadingSize * 0.75,
                        decoration: BoxDecoration(
                          shape: BoxShape.circle,
                          gradient: LinearGradient(
                            colors: AppColors.affirmationGradient,
                            begin: Alignment.topLeft,
                            end: Alignment.bottomRight,
                          ),
                          boxShadow: [
                            BoxShadow(
                              color: AppColors.affirmationGradient[0].withValues(alpha: 0.4),
                              blurRadius: 20.r,
                              spreadRadius: 1.r,
                            ),
                          ],
                        ),
                        child: Icon(
                          Icons.auto_awesome,
                          color: AppColors.white,
                          size: loadingSize * 0.35,
                        ),
                      ),
                    ),
                  ],
                ),
              );
            },
          ),
          
          if (widget.showMessage) ...[
            SizedBox(height: 32.h),
            
            // Loading message with flowing gradient hue rotation
            AnimatedBuilder(
              animation: Listenable.merge([_glowAnimation, _shimmerAnimation]),
              builder: (context, child) {
                final double progress = _shimmerAnimation.value / 360.0; // Normalize to 0-1
                
                return ShaderMask(
                  shaderCallback: (bounds) {
                    // Create a continuously flowing gradient that never resets
                    final double offset = -progress * 3.0; // Negative for left-to-right flow
                    
                    // Generate colors that smoothly cycle through: pink → orange → white → pink
                    List<Color> generateGradientColors() {
                      final List<Color> colors = [];
                      final List<Color> baseColors = [
                        AppColors.affirmationGradient[0], // Pink
                        AppColors.affirmationGradient[1], // Orange
                        AppColors.white,                   // White
                        AppColors.affirmationGradient[0], // Pink (for seamless loop)
                      ];
                      
                      // Create smooth gradient stops
                      for (int i = 0; i < 8; i++) {
                        // Calculate continuous position without hard resets
                        final double position = (offset + i * 0.25); // Smooth increment
                        final double normalizedPosition = position - position.floor(); // Get fractional part (0-1)
                        final double colorIndex = normalizedPosition * (baseColors.length - 1); // Scale to color array
                        
                        // Get the two colors to interpolate between
                        final int lowerIndex = colorIndex.floor();
                        final int upperIndex = (lowerIndex + 1) % (baseColors.length - 1); // Wrap around excluding duplicate
                        final double t = colorIndex - lowerIndex; // Interpolation factor
                        
                        // Smooth interpolation between colors
                        final Color interpolatedColor = Color.lerp(
                          baseColors[lowerIndex],
                          baseColors[upperIndex],
                          t,
                        )!;
                        
                        colors.add(interpolatedColor);
                      }
                      
                      return colors;
                    }
                    
                    return LinearGradient(
                      colors: generateGradientColors(),
                      stops: [0.0, 0.14, 0.28, 0.42, 0.56, 0.70, 0.84, 1.0],
                      begin: Alignment.centerLeft,
                      end: Alignment.centerRight,
                    ).createShader(bounds);
                  },
                  child: Text(
                    widget.message ?? 'Loading...',
                    style: TextStyle(
                      fontSize: 16.sp,
                      color: AppColors.white, // This will be masked by the gradient
                      fontWeight: FontWeight.w600,
                      letterSpacing: 1.5,
                      height: 1.2,
                      shadows: [
                        Shadow(
                          color: AppColors.affirmationGradient[0].withValues(alpha: _glowAnimation.value * 0.3),
                          blurRadius: 8.r,
                          offset: Offset(0, 1.h),
                        ),
                        Shadow(
                          color: AppColors.affirmationGradient[1].withValues(alpha: _glowAnimation.value * 0.2),
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
          ],
        ],
      ),
    );
  }
}

class StylishLoadingOverlay extends StatelessWidget {
  final String? message;
  final bool isVisible;

  const StylishLoadingOverlay({
    Key? key,
    this.message,
    this.isVisible = true,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    if (!isVisible) return SizedBox.shrink();

    return Container(
      color: AppColors.backgroundDark.withValues(alpha: 0.8),
      child: StylishLoading(
        message: message,
      ),
    );
  }
}

class StylishLoadingDialog extends StatelessWidget {
  final String? message;

  const StylishLoadingDialog({
    Key? key,
    this.message,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Dialog(
      backgroundColor: Colors.transparent,
      elevation: 0,
      child: Container(
        padding: EdgeInsets.all(24.w),
        decoration: BoxDecoration(
          color: AppColors.cardBackground,
          borderRadius: BorderRadius.circular(20.r),
          boxShadow: [
            BoxShadow(
              color: Colors.black.withValues(alpha: 0.3),
              blurRadius: 20.r,
              offset: Offset(0, 10.h),
            ),
          ],
        ),
        child: StylishLoading(
          message: message,
          size: 80.w,
        ),
      ),
    );
  }

  static Future<void> show(
    BuildContext context, {
    String? message,
  }) {
    return showDialog(
      context: context,
      barrierDismissible: false,
      builder: (context) => StylishLoadingDialog(message: message),
    );
  }

  static void hide(BuildContext context) {
    Navigator.of(context).pop();
  }
}

// Small loading indicator for buttons
class CompactStylishLoading extends StatelessWidget {
  final Color? color;
  final double? size;

  const CompactStylishLoading({
    Key? key,
    this.color,
    this.size,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return SizedBox(
      width: size ?? 24.w,
      height: size ?? 24.h,
      child: CircularProgressIndicator(
        strokeWidth: 2.5,
        valueColor: AlwaysStoppedAnimation<Color>(color ?? AppColors.accentCoral),
        backgroundColor: (color ?? AppColors.accentCoral).withValues(alpha: 0.2),
      ),
    );
  }
}