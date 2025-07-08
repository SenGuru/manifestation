import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:manifestation_app/colors.dart';
import 'dart:math' as math;

class BreathingGuide extends StatefulWidget {
  final AnimationController controller;
  final bool isActive;
  final Color? color;

  const BreathingGuide({
    Key? key,
    required this.controller,
    required this.isActive,
    this.color,
  }) : super(key: key);

  @override
  _BreathingGuideState createState() => _BreathingGuideState();
}

class _BreathingGuideState extends State<BreathingGuide>
    with TickerProviderStateMixin {
  late Animation<double> _breathingAnimation;
  late Animation<double> _textOpacityAnimation;
  
  String currentPhase = 'Breathe In';
  bool _isInhaling = true;

  @override
  void initState() {
    super.initState();
    
    _breathingAnimation = Tween<double>(
      begin: 0.8,
      end: 1.2,
    ).animate(CurvedAnimation(
      parent: widget.controller,
      curve: Curves.easeInOut,
    ));
    
    _textOpacityAnimation = Tween<double>(
      begin: 0.6,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: widget.controller,
      curve: Curves.easeInOut,
    ));
    
    widget.controller.addListener(_onAnimationUpdate);
  }

  void _onAnimationUpdate() {
    if (mounted) {
      final value = widget.controller.value;
      
      // Determine if we're inhaling or exhaling
      if (value <= 0.5 && !_isInhaling) {
        setState(() {
          _isInhaling = true;
          currentPhase = 'Breathe In';
        });
      } else if (value > 0.5 && _isInhaling) {
        setState(() {
          _isInhaling = false;
          currentPhase = 'Breathe Out';
        });
      }
    }
  }

  @override
  void dispose() {
    widget.controller.removeListener(_onAnimationUpdate);
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    if (!widget.isActive) return SizedBox.shrink();
    
    return Center(
      child: AnimatedBuilder(
        animation: widget.controller,
        builder: (context, child) {
          return Column( // Removed fixed height container to prevent overflow
            mainAxisSize: MainAxisSize.min, // Use minimum space needed
            children: [
              // Breathing circle section
              Container(
                width: 180.w, // Reduced size
                height: 180.h, // Reduced size
                child: Stack(
                  alignment: Alignment.center,
                  children: [
                    // Breathing circle
                    _buildBreathingCircle(),
                    
                    // Instruction text
                    _buildInstructionText(),
                  ],
                ),
              ),
              
              SizedBox(height: 15.h), // Reduced spacing
              
              // Progress indicator section - now separated
              _buildProgressIndicator(),
            ],
          );
        },
      ),
    );
  }

  Widget _buildBreathingCircle() {
    final primaryColor = widget.color ?? AppColors.accentLavender;
    
    return AnimatedBuilder(
      animation: _breathingAnimation,
      builder: (context, child) {
        return Container(
          width: (150 * _breathingAnimation.value).w,
          height: (150 * _breathingAnimation.value).h,
          decoration: BoxDecoration(
            shape: BoxShape.circle,
            border: Border.all(
              color: primaryColor.withOpacity(0.6),
              width: 3.w,
            ),
            boxShadow: [
              BoxShadow(
                color: primaryColor.withOpacity(0.3),
                blurRadius: 20.r,
                spreadRadius: 5.r,
              ),
            ],
          ),
          child: Container(
            margin: EdgeInsets.all(20.w),
            decoration: BoxDecoration(
              shape: BoxShape.circle,
              color: primaryColor.withOpacity(0.1),
              border: Border.all(
                color: primaryColor.withOpacity(0.3),
                width: 2.w,
              ),
            ),
            child: Container(
              margin: EdgeInsets.all(15.w),
              decoration: BoxDecoration(
                shape: BoxShape.circle,
                color: primaryColor.withOpacity(0.05),
                border: Border.all(
                  color: primaryColor.withOpacity(0.2),
                  width: 1.w,
                ),
              ),
            ),
          ),
        );
      },
    );
  }

  Widget _buildInstructionText() {
    final primaryColor = widget.color ?? AppColors.accentLavender;
    
    return AnimatedBuilder(
      animation: _textOpacityAnimation,
      builder: (context, child) {
        return Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Text(
              currentPhase,
              style: TextStyle(
                fontSize: 18.sp,
                fontWeight: FontWeight.w600,
                color: AppColors.textLight.withOpacity(_textOpacityAnimation.value),
              ),
            ),
            SizedBox(height: 8.h),
            Container(
              padding: EdgeInsets.symmetric(horizontal: 12.w, vertical: 4.h),
              decoration: BoxDecoration(
                color: primaryColor.withOpacity(0.1),
                borderRadius: BorderRadius.circular(12.r),
              ),
              child: Text(
                _isInhaling ? '4 seconds' : '4 seconds',
                style: TextStyle(
                  fontSize: 12.sp,
                  color: primaryColor.withOpacity(_textOpacityAnimation.value),
                ),
              ),
            ),
          ],
        );
      },
    );
  }

  Widget _buildProgressIndicator() {
    final primaryColor = widget.color ?? AppColors.accentLavender;
    
    return Container(
      height: 60.h, // Reduced height to prevent overflow
      child: Column(
        mainAxisSize: MainAxisSize.min, // Use minimum space needed
        children: [
          // Progress bar
          Container(
            width: 180.w, // Reduced width to match circle
            height: 4.h,
            decoration: BoxDecoration(
              color: primaryColor.withOpacity(0.2),
              borderRadius: BorderRadius.circular(2.r),
            ),
            child: Stack(
              children: [
                Container(
                  width: 180.w * widget.controller.value,
                  height: 4.h,
                  decoration: BoxDecoration(
                    color: primaryColor,
                    borderRadius: BorderRadius.circular(2.r),
                  ),
                ),
              ],
            ),
          ),
          
          SizedBox(height: 8.h), // Reduced spacing
          
          // Phase indicator dots
          Row(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              _buildPhaseDot(true, _isInhaling, primaryColor),
              SizedBox(width: 8.w),
              _buildPhaseDot(false, !_isInhaling, primaryColor),
            ],
          ),
          
          SizedBox(height: 8.h), // Reduced spacing
          
          // Helpful tip
          Text(
            'Focus on the circle\'s rhythm',
            style: TextStyle(
              fontSize: 10.sp,
              color: AppColors.textMedium.withOpacity(0.6),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildPhaseDot(bool isInhale, bool isActive, Color color) {
    return Container(
      width: 8.w,
      height: 8.h,
      decoration: BoxDecoration(
        shape: BoxShape.circle,
        color: isActive ? color : color.withOpacity(0.3),
        border: Border.all(
          color: color.withOpacity(0.5),
          width: 1.w,
        ),
      ),
    );
  }
}

// Advanced breathing guide with multiple patterns
class AdvancedBreathingGuide extends StatefulWidget {
  final AnimationController controller;
  final bool isActive;
  final BreathingPattern pattern;
  final Color? color;

  const AdvancedBreathingGuide({
    Key? key,
    required this.controller,
    required this.isActive,
    this.pattern = BreathingPattern.fourFour,
    this.color,
  }) : super(key: key);

  @override
  _AdvancedBreathingGuideState createState() => _AdvancedBreathingGuideState();
}

class _AdvancedBreathingGuideState extends State<AdvancedBreathingGuide> {
  @override
  Widget build(BuildContext context) {
    if (!widget.isActive) return SizedBox.shrink();
    
    return _buildPatternSpecificGuide();
  }

  Widget _buildPatternSpecificGuide() {
    switch (widget.pattern) {
      case BreathingPattern.fourFour:
        return BreathingGuide(
          controller: widget.controller,
          isActive: widget.isActive,
          color: widget.color,
        );
      case BreathingPattern.fourSevenEight:
        return _build478Pattern();
      case BreathingPattern.boxBreathing:
        return _buildBoxPattern();
      case BreathingPattern.triangleBreathing:
        return _buildTrianglePattern();
    }
  }

  Widget _build478Pattern() {
    // 4-7-8 breathing pattern implementation
    return Container(
      width: 200.w,
      height: 200.h,
      child: CustomPaint(
        painter: BreathingPatternPainter(
          progress: widget.controller.value,
          pattern: widget.pattern,
          color: widget.color ?? AppColors.accentLavender,
        ),
      ),
    );
  }

  Widget _buildBoxPattern() {
    // Box breathing pattern implementation
    return Container(
      width: 180.w,
      height: 180.h,
      child: CustomPaint(
        painter: BreathingPatternPainter(
          progress: widget.controller.value,
          pattern: widget.pattern,
          color: widget.color ?? AppColors.accentLavender,
        ),
      ),
    );
  }

  Widget _buildTrianglePattern() {
    // Triangle breathing pattern implementation
    return Container(
      width: 160.w,
      height: 160.h,
      child: CustomPaint(
        painter: BreathingPatternPainter(
          progress: widget.controller.value,
          pattern: widget.pattern,
          color: widget.color ?? AppColors.accentLavender,
        ),
      ),
    );
  }
}

enum BreathingPattern {
  fourFour,       // 4 in, 4 out
  fourSevenEight, // 4 in, 7 hold, 8 out
  boxBreathing,   // 4 in, 4 hold, 4 out, 4 hold
  triangleBreathing, // 3 in, 3 hold, 3 out
}

class BreathingPatternPainter extends CustomPainter {
  final double progress;
  final BreathingPattern pattern;
  final Color color;

  BreathingPatternPainter({
    required this.progress,
    required this.pattern,
    required this.color,
  });

  @override
  void paint(Canvas canvas, Size size) {
    final center = Offset(size.width / 2, size.height / 2);
    
    switch (pattern) {
      case BreathingPattern.fourFour:
        _drawCirclePattern(canvas, center, size);
        break;
      case BreathingPattern.fourSevenEight:
        _draw478Pattern(canvas, center, size);
        break;
      case BreathingPattern.boxBreathing:
        _drawBoxPattern(canvas, center, size);
        break;
      case BreathingPattern.triangleBreathing:
        _drawTrianglePattern(canvas, center, size);
        break;
    }
  }

  void _drawCirclePattern(Canvas canvas, Offset center, Size size) {
    final radius = size.width / 3;
    final currentRadius = radius * (0.7 + 0.6 * math.sin(progress * 2 * math.pi));
    
    final paint = Paint()
      ..color = color.withOpacity(0.6)
      ..style = PaintingStyle.stroke
      ..strokeWidth = 3.w;
    
    canvas.drawCircle(center, currentRadius, paint);
  }

  void _draw478Pattern(Canvas canvas, Offset center, Size size) {
    // Implementation for 4-7-8 pattern visualization
    final paint = Paint()
      ..color = color.withOpacity(0.6)
      ..style = PaintingStyle.stroke
      ..strokeWidth = 2.w;
    
    // Draw a triangle that changes based on the 4-7-8 rhythm
    final path = Path();
    final radius = size.width / 3;
    
    for (int i = 0; i < 3; i++) {
      final angle = i * 2 * math.pi / 3 - math.pi / 2;
      final currentRadius = radius * (0.8 + 0.4 * math.sin(progress * 2 * math.pi + i));
      final x = center.dx + currentRadius * math.cos(angle);
      final y = center.dy + currentRadius * math.sin(angle);
      
      if (i == 0) {
        path.moveTo(x, y);
      } else {
        path.lineTo(x, y);
      }
    }
    path.close();
    
    canvas.drawPath(path, paint);
  }

  void _drawBoxPattern(Canvas canvas, Offset center, Size size) {
    // Implementation for box breathing pattern
    final paint = Paint()
      ..color = color.withOpacity(0.6)
      ..style = PaintingStyle.stroke
      ..strokeWidth = 3.w;
    
    final sideLength = size.width / 2;
    final currentScale = 0.7 + 0.6 * math.sin(progress * 2 * math.pi);
    final scaledLength = sideLength * currentScale;
    
    final rect = Rect.fromCenter(
      center: center,
      width: scaledLength,
      height: scaledLength,
    );
    
    canvas.drawRect(rect, paint);
    
    // Draw progress dot moving around the box
    final dotProgress = progress * 4; // 4 sides
    final side = dotProgress.floor();
    final sideProgress = dotProgress - side;
    
    Offset dotPosition;
    switch (side % 4) {
      case 0: // Top side
        dotPosition = Offset(
          rect.left + rect.width * sideProgress,
          rect.top,
        );
        break;
      case 1: // Right side
        dotPosition = Offset(
          rect.right,
          rect.top + rect.height * sideProgress,
        );
        break;
      case 2: // Bottom side
        dotPosition = Offset(
          rect.right - rect.width * sideProgress,
          rect.bottom,
        );
        break;
      case 3: // Left side
        dotPosition = Offset(
          rect.left,
          rect.bottom - rect.height * sideProgress,
        );
        break;
      default:
        dotPosition = center;
    }
    
    final dotPaint = Paint()
      ..color = color
      ..style = PaintingStyle.fill;
    
    canvas.drawCircle(dotPosition, 4.r, dotPaint);
  }

  void _drawTrianglePattern(Canvas canvas, Offset center, Size size) {
    // Implementation for triangle breathing pattern
    final paint = Paint()
      ..color = color.withOpacity(0.6)
      ..style = PaintingStyle.stroke
      ..strokeWidth = 3.w;
    
    final radius = size.width / 3;
    final currentScale = 0.7 + 0.6 * math.sin(progress * 2 * math.pi);
    
    final path = Path();
    for (int i = 0; i < 3; i++) {
      final angle = i * 2 * math.pi / 3 - math.pi / 2;
      final x = center.dx + radius * currentScale * math.cos(angle);
      final y = center.dy + radius * currentScale * math.sin(angle);
      
      if (i == 0) {
        path.moveTo(x, y);
      } else {
        path.lineTo(x, y);
      }
    }
    path.close();
    
    canvas.drawPath(path, paint);
    
    // Draw progress indicator
    final progressAngle = progress * 2 * math.pi - math.pi / 2;
    final progressX = center.dx + (radius * currentScale + 15.w) * math.cos(progressAngle);
    final progressY = center.dy + (radius * currentScale + 15.h) * math.sin(progressAngle);
    
    final progressPaint = Paint()
      ..color = color
      ..style = PaintingStyle.fill;
    
    canvas.drawCircle(Offset(progressX, progressY), 3.r, progressPaint);
  }

  @override
  bool shouldRepaint(covariant CustomPainter oldDelegate) => true;
}