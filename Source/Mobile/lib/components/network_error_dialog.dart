import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'dart:math' as math;
import '../constants/app_theme.dart';

class NetworkErrorDialog extends StatefulWidget {
  final String? customMessage;
  final VoidCallback? onRetry;
  final bool showRetryButton;

  const NetworkErrorDialog({
    Key? key,
    this.customMessage,
    this.onRetry,
    this.showRetryButton = true,
  }) : super(key: key);

  @override
  _NetworkErrorDialogState createState() => _NetworkErrorDialogState();
}

class _NetworkErrorDialogState extends State<NetworkErrorDialog>
    with TickerProviderStateMixin {
  late AnimationController _scaleController;
  late AnimationController _rotationController;
  late AnimationController _pulseController;
  late AnimationController _fadeController;
  
  late Animation<double> _scaleAnimation;
  late Animation<double> _rotationAnimation;
  late Animation<double> _pulseAnimation;
  late Animation<double> _fadeAnimation;
  late Animation<double> _contentFadeAnimation;

  @override
  void initState() {
    super.initState();
    
    // Scale animation for entrance
    _scaleController = AnimationController(
      duration: Duration(milliseconds: 600),
      vsync: this,
    );
    
    // Rotation animation for the icon
    _rotationController = AnimationController(
      duration: Duration(seconds: 4),
      vsync: this,
    )..repeat();
    
    // Pulse animation for the glow effect
    _pulseController = AnimationController(
      duration: Duration(seconds: 2),
      vsync: this,
    )..repeat(reverse: true);
    
    // Fade animation for content
    _fadeController = AnimationController(
      duration: Duration(milliseconds: 800),
      vsync: this,
    );
    
    _scaleAnimation = CurvedAnimation(
      parent: _scaleController,
      curve: Curves.elasticOut,
    );
    
    _rotationAnimation = CurvedAnimation(
      parent: _rotationController,
      curve: Curves.linear,
    );
    
    _pulseAnimation = Tween<double>(
      begin: 0.95,
      end: 1.05,
    ).animate(CurvedAnimation(
      parent: _pulseController,
      curve: Curves.easeInOut,
    ));
    
    _fadeAnimation = Tween<double>(
      begin: 0.0,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _fadeController,
      curve: Interval(0.0, 0.5, curve: Curves.easeOut),
    ));
    
    _contentFadeAnimation = Tween<double>(
      begin: 0.0,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _fadeController,
      curve: Interval(0.3, 1.0, curve: Curves.easeOut),
    ));
    
    // Start animations
    _scaleController.forward();
    _fadeController.forward();
  }
  
  @override
  void dispose() {
    _scaleController.dispose();
    _rotationController.dispose();
    _pulseController.dispose();
    _fadeController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return WillPopScope(
      onWillPop: () async {
        Navigator.of(context).pop();
        return false;
      },
      child: AnimatedBuilder(
        animation: Listenable.merge([
          _scaleAnimation,
          _fadeAnimation,
        ]),
        builder: (context, child) {
          return Opacity(
            opacity: _fadeAnimation.value,
            child: Transform.scale(
              scale: _scaleAnimation.value,
              child: Dialog(
                backgroundColor: Colors.transparent,
                elevation: 0,
                child: Container(
                  constraints: BoxConstraints(
                    maxWidth: 340.w,
                  ),
                  decoration: BoxDecoration(
                    borderRadius: BorderRadius.circular(28.r),
                  ),
                  child: ClipRRect(
                    borderRadius: BorderRadius.circular(28.r),
                    child: BackdropFilter(
                      filter: ColorFilter.mode(
                        Colors.black.withOpacity(0.1),
                        BlendMode.darken,
                      ),
                      child: Container(
                        decoration: BoxDecoration(
                          gradient: LinearGradient(
                            begin: Alignment.topLeft,
                            end: Alignment.bottomRight,
                            colors: [
                              AppTheme.cardBackground,
                              AppTheme.cardBackground.withOpacity(0.95),
                            ],
                          ),
                          borderRadius: BorderRadius.circular(28.r),
                          border: Border.all(
                            color: AppTheme.textMedium.withOpacity(0.1),
                            width: 1,
                          ),
                          boxShadow: [
                            BoxShadow(
                              color: Colors.black.withOpacity(0.2),
                              blurRadius: 20.r,
                              offset: Offset(0, 10.h),
                            ),
                            BoxShadow(
                              color: AppTheme.affirmationGradient[0].withOpacity(0.05),
                              blurRadius: 40.r,
                              offset: Offset(0, 0),
                            ),
                          ],
                        ),
                        child: Padding(
                          padding: EdgeInsets.all(28.w),
                          child: Column(
                            mainAxisSize: MainAxisSize.min,
                            children: [
                              // Animated Icon Section
                              _buildAnimatedIcon(),
                              
                              SizedBox(height: 24.h),
                              
                              // Title with fade animation
                              FadeTransition(
                                opacity: _contentFadeAnimation,
                                child: Text(
                                  'Connection Issue',
                                  style: TextStyle(
                                    fontSize: 24.sp,
                                    fontWeight: FontWeight.bold,
                                    color: AppTheme.textPrimary,
                                    letterSpacing: -0.5,
                                  ),
                                ),
                              ),
                              
                              SizedBox(height: 12.h),
                              
                              // Message with fade animation
                              FadeTransition(
                                opacity: _contentFadeAnimation,
                                child: Text(
                                  widget.customMessage ?? 
                                  "We're having trouble connecting to our servers. Please check your internet connection and try again.",
                                  style: TextStyle(
                                    fontSize: 14.sp,
                                    color: AppTheme.textSecondary,
                                    height: 1.5,
                                  ),
                                  textAlign: TextAlign.center,
                                ),
                              ),
                              
                              SizedBox(height: 28.h),
                              
                              // Buttons with stagger animation
                              FadeTransition(
                                opacity: _contentFadeAnimation,
                                child: _buildButtons(),
                              ),
                            ],
                          ),
                        ),
                      ),
                    ),
                  ),
                ),
              ),
            ),
          );
        },
      ),
    );
  }

  Widget _buildAnimatedIcon() {
    return Container(
      height: 100.h,
      width: 100.w,
      child: Stack(
        alignment: Alignment.center,
        children: [
          // Pulsing background circle
          AnimatedBuilder(
            animation: _pulseAnimation,
            builder: (context, child) {
              return Transform.scale(
                scale: _pulseAnimation.value,
                child: Container(
                  width: 90.w,
                  height: 90.h,
                  decoration: BoxDecoration(
                    shape: BoxShape.circle,
                    gradient: RadialGradient(
                      colors: [
                        Colors.orange.withOpacity(0.1),
                        Colors.orange.withOpacity(0.05),
                        Colors.transparent,
                      ],
                    ),
                  ),
                ),
              );
            },
          ),
          
          // Rotating outer ring
          AnimatedBuilder(
            animation: _rotationAnimation,
            builder: (context, child) {
              return Transform.rotate(
                angle: _rotationAnimation.value * 2 * math.pi,
                child: CustomPaint(
                  size: Size(80.w, 80.h),
                  painter: _DottedCirclePainter(
                    color: Colors.orange.withOpacity(0.3),
                  ),
                ),
              );
            },
          ),
          
          // Center icon with subtle bounce
          AnimatedBuilder(
            animation: _pulseAnimation,
            builder: (context, child) {
              return Transform.scale(
                scale: 1.0 + (_pulseAnimation.value - 1.0) * 0.3,
                child: Container(
                  width: 60.w,
                  height: 60.h,
                  decoration: BoxDecoration(
                    shape: BoxShape.circle,
                    color: Colors.orange.withOpacity(0.1),
                    border: Border.all(
                      color: Colors.orange.withOpacity(0.2),
                      width: 2,
                    ),
                  ),
                  child: Icon(
                    Icons.wifi_off_rounded,
                    size: 30.sp,
                    color: Colors.orange,
                  ),
                ),
              );
            },
          ),
        ],
      ),
    );
  }

  Widget _buildButtons() {
    return Row(
      children: [
        // Cancel button
        Expanded(
          child: TextButton(
            onPressed: () {
              HapticFeedback.lightImpact();
              Navigator.of(context).pop();
            },
            style: TextButton.styleFrom(
              padding: EdgeInsets.symmetric(vertical: 14.h),
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(16.r),
                side: BorderSide(
                  color: AppTheme.textMedium.withOpacity(0.2),
                  width: 1,
                ),
              ),
            ),
            child: Text(
              'Cancel',
              style: TextStyle(
                fontSize: 15.sp,
                color: AppTheme.textSecondary,
                fontWeight: FontWeight.w600,
              ),
            ),
          ),
        ),
        
        if (widget.showRetryButton) ...[
          SizedBox(width: 12.w),
          
          // Retry button
          Expanded(
            child: Container(
              decoration: BoxDecoration(
                borderRadius: BorderRadius.circular(16.r),
                gradient: LinearGradient(
                  colors: [
                    Colors.orange.shade400,
                    Colors.orange.shade600,
                  ],
                ),
                boxShadow: [
                  BoxShadow(
                    color: Colors.orange.withOpacity(0.3),
                    blurRadius: 8.r,
                    offset: Offset(0, 4.h),
                  ),
                ],
              ),
              child: TextButton(
                onPressed: () {
                  HapticFeedback.lightImpact();
                  Navigator.of(context).pop();
                  widget.onRetry?.call();
                },
                style: TextButton.styleFrom(
                  padding: EdgeInsets.symmetric(vertical: 14.h),
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(16.r),
                  ),
                  foregroundColor: Colors.white,
                ),
                child: Row(
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: [
                    Icon(
                      Icons.refresh_rounded,
                      size: 18.sp,
                      color: Colors.white,
                    ),
                    SizedBox(width: 6.w),
                    Text(
                      'Try Again',
                      style: TextStyle(
                        fontSize: 15.sp,
                        color: Colors.white,
                        fontWeight: FontWeight.w600,
                      ),
                    ),
                  ],
                ),
              ),
            ),
          ),
        ],
      ],
    );
  }
}

// Custom painter for dotted circle
class _DottedCirclePainter extends CustomPainter {
  final Color color;
  
  _DottedCirclePainter({required this.color});
  
  @override
  void paint(Canvas canvas, Size size) {
    final paint = Paint()
      ..color = color
      ..style = PaintingStyle.stroke
      ..strokeWidth = 2;
    
    final center = Offset(size.width / 2, size.height / 2);
    final radius = math.min(size.width, size.height) / 2;
    
    // Draw dotted circle
    const int numberOfDots = 20;
    const double dotLength = 4;
    const double spaceLength = 8;
    
    for (int i = 0; i < numberOfDots; i++) {
      final startAngle = (i * 2 * math.pi / numberOfDots);
      final sweepAngle = (dotLength / (radius * 2 * math.pi)) * 2 * math.pi;
      
      canvas.drawArc(
        Rect.fromCircle(center: center, radius: radius),
        startAngle,
        sweepAngle,
        false,
        paint,
      );
    }
  }
  
  @override
  bool shouldRepaint(covariant CustomPainter oldDelegate) => false;
}

// Helper function to show the network error dialog
void showNetworkErrorDialog(
  BuildContext context, {
  String? message,
  VoidCallback? onRetry,
  bool showRetryButton = true,
}) {
  showDialog(
    context: context,
    barrierDismissible: false,
    barrierColor: Colors.black.withOpacity(0.5),
    builder: (context) => NetworkErrorDialog(
      customMessage: message,
      onRetry: onRetry,
      showRetryButton: showRetryButton,
    ),
  );
}