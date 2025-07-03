import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:manifestation_app/colors.dart';
import 'dart:math' as math;

import 'package:manifestation_app/meditation_constants.dart';

class AnimatedMinimal extends StatefulWidget {
  final double intensity;
  final double size;

  const AnimatedMinimal({
    Key? key,
    required this.intensity,
    required this.size,
  }) : super(key: key);

  @override
  _AnimatedMinimalState createState() => _AnimatedMinimalState();
}

class _AnimatedMinimalState extends State<AnimatedMinimal>
    with TickerProviderStateMixin {
  late AnimationController _breathingController;
  late AnimationController _pulseController;
  late AnimationController _rotationController;
  
  late Animation<double> _breathingAnimation;
  late Animation<double> _pulseAnimation;
  late Animation<double> _rotationAnimation;

  @override
  void initState() {
    super.initState();
    
    // Main breathing animation (inhale/exhale)
    _breathingController = AnimationController(
      duration: AppConstants.minimalBreathing,
      vsync: this,
    );
    
    // Pulse animation for depth
    _pulseController = AnimationController(
      duration: Duration(milliseconds: 2000),
      vsync: this,
    );
    
    // Subtle rotation animation
    _rotationController = AnimationController(
      duration: Duration(milliseconds: 12000),
      vsync: this,
    );
    
    _breathingAnimation = Tween<double>(
      begin: 0.8,
      end: 1.2,
    ).animate(CurvedAnimation(
      parent: _breathingController,
      curve: Curves.easeInOut,
    ));
    
    _pulseAnimation = Tween<double>(
      begin: 0.9,
      end: 1.1,
    ).animate(CurvedAnimation(
      parent: _pulseController,
      curve: Curves.easeInOut,
    ));
    
    _rotationAnimation = Tween<double>(
      begin: 0.0,
      end: 2 * math.pi,
    ).animate(CurvedAnimation(
      parent: _rotationController,
      curve: Curves.linear,
    ));
    
    _startAnimations();
  }

  void _startAnimations() {
    if (widget.intensity > 0.3) {
      _breathingController.repeat(reverse: true);
      _pulseController.repeat(reverse: true);
      _rotationController.repeat();
    }
  }

  @override
  void didUpdateWidget(AnimatedMinimal oldWidget) {
    super.didUpdateWidget(oldWidget);
    if (widget.intensity != oldWidget.intensity) {
      if (widget.intensity > 0.3) {
        _startAnimations();
      } else {
        _breathingController.stop();
        _pulseController.stop();
        _rotationController.stop();
      }
    }
  }

  @override
  void dispose() {
    _breathingController.dispose();
    _pulseController.dispose();
    _rotationController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return SizedBox(
      width: widget.size.w,
      height: widget.size.h,
      child: AnimatedBuilder(
        animation: Listenable.merge([
          _breathingController,
          _pulseController,
          _rotationController,
        ]),
        builder: (context, child) {
          return Transform.rotate(
            angle: _rotationAnimation.value * 0.1, // Very subtle rotation
            child: CustomPaint(
              size: Size(widget.size.w, widget.size.h),
              painter: MinimalPainter(
                intensity: widget.intensity,
                breathingScale: _breathingAnimation.value,
                pulseScale: _pulseAnimation.value,
                rotation: _rotationAnimation.value,
              ),
            ),
          );
        },
      ),
    );
  }
}

class MinimalPainter extends CustomPainter {
  final double intensity;
  final double breathingScale;
  final double pulseScale;
  final double rotation;

  MinimalPainter({
    required this.intensity,
    required this.breathingScale,
    required this.pulseScale,
    required this.rotation,
  });

  @override
  void paint(Canvas canvas, Size size) {
    final center = Offset(size.width / 2, size.height / 2);
    final baseRadius = size.width / 3;
    
    // Draw concentric breathing circles
    _drawBreathingCircles(canvas, center, baseRadius);
    
    // Draw geometric patterns
    _drawGeometricPattern(canvas, center, baseRadius);
    
    // Draw center focus point
    _drawCenterPoint(canvas, center, baseRadius);
    
    // Draw subtle glow
    _drawGlow(canvas, center, baseRadius);
  }

  void _drawBreathingCircles(Canvas canvas, Offset center, double baseRadius) {
    final colors = AppColors.minimalGradient;
    
    // Outer circle - main breathing circle
    final outerRadius = baseRadius * breathingScale * 1.3;
    final outerPaint = Paint()
      ..color = colors[0].withOpacity(0.3 * intensity)
      ..style = PaintingStyle.stroke
      ..strokeWidth = 3.w;
    
    canvas.drawCircle(center, outerRadius, outerPaint);
    
    // Middle circle - pulse animation
    final middleRadius = baseRadius * breathingScale;
    final middlePaint = Paint()
      ..color = colors[0].withOpacity(0.6 * intensity)
      ..style = PaintingStyle.stroke
      ..strokeWidth = 4.w;
    
    canvas.drawCircle(center, middleRadius, middlePaint);
    
    // Inner circle - counter breathing
    final innerRadius = baseRadius * 0.6 * (2.0 - breathingScale) * pulseScale;
    final innerPaint = Paint()
      ..color = colors[1].withOpacity(0.8 * intensity)
      ..style = PaintingStyle.stroke
      ..strokeWidth = 2.w;
    
    canvas.drawCircle(center, innerRadius, innerPaint);
    
    // Very inner circle - solid
    final coreRadius = baseRadius * 0.3 * pulseScale;
    final corePaint = Paint()
      ..color = colors[1].withOpacity(0.4 * intensity)
      ..style = PaintingStyle.fill;
    
    canvas.drawCircle(center, coreRadius, corePaint);
  }

  void _drawGeometricPattern(Canvas canvas, Offset center, double baseRadius) {
    if (intensity < 0.5) return;
    
    final colors = AppColors.minimalGradient;
    final patternRadius = baseRadius * breathingScale * 0.8;
    
    // Draw petals/leaves pattern
    for (int i = 0; i < 8; i++) {
      final angle = (i * math.pi / 4) + rotation * 0.2;
      final startX = center.dx + patternRadius * 0.3 * math.cos(angle);
      final startY = center.dy + patternRadius * 0.3 * math.sin(angle);
      final endX = center.dx + patternRadius * math.cos(angle);
      final endY = center.dy + patternRadius * math.sin(angle);
      
      final petalPaint = Paint()
        ..color = colors[i % 2].withOpacity(0.4 * intensity)
        ..strokeWidth = 2.w
        ..strokeCap = StrokeCap.round;
      
      canvas.drawLine(
        Offset(startX, startY),
        Offset(endX, endY),
        petalPaint,
      );
      
      // Add small circle at the end of each petal
      final dotPaint = Paint()
        ..color = colors[i % 2].withOpacity(0.6 * intensity)
        ..style = PaintingStyle.fill;
      
      canvas.drawCircle(
        Offset(endX, endY),
        3.r * pulseScale,
        dotPaint,
      );
    }
  }

  void _drawCenterPoint(Canvas canvas, Offset center, double baseRadius) {
    final colors = AppColors.minimalGradient;
    
    // Breathing center point
    final centerRadius = 8.r * breathingScale * intensity;
    final centerPaint = Paint()
      ..shader = RadialGradient(
        colors: [
          colors[1].withOpacity(0.8),
          colors[0].withOpacity(0.4),
          Colors.transparent,
        ],
        stops: [0.0, 0.7, 1.0],
      ).createShader(Rect.fromCircle(center: center, radius: centerRadius));
    
    canvas.drawCircle(center, centerRadius, centerPaint);
    
    // Inner core
    final corePaint = Paint()
      ..color = Colors.white.withOpacity(0.8 * intensity)
      ..style = PaintingStyle.fill;
    
    canvas.drawCircle(center, 4.r * pulseScale, corePaint);
  }

  void _drawGlow(Canvas canvas, Offset center, double baseRadius) {
    if (intensity < 0.4) return;
    
    final colors = AppColors.minimalGradient;
    final glowRadius = baseRadius * breathingScale * 1.5;
    
    // Outer glow
    final outerGlowPaint = Paint()
      ..color = colors[0].withOpacity(0.1 * intensity)
      ..maskFilter = MaskFilter.blur(BlurStyle.normal, 30.r);
    
    canvas.drawCircle(center, glowRadius, outerGlowPaint);
    
    // Inner glow
    final innerGlowPaint = Paint()
      ..color = colors[1].withOpacity(0.15 * intensity)
      ..maskFilter = MaskFilter.blur(BlurStyle.normal, 15.r);
    
    canvas.drawCircle(center, glowRadius * 0.7, innerGlowPaint);
    
    // Core glow
    final coreGlowPaint = Paint()
      ..color = Colors.white.withOpacity(0.2 * intensity)
      ..maskFilter = MaskFilter.blur(BlurStyle.normal, 8.r);
    
    canvas.drawCircle(center, baseRadius * 0.3 * breathingScale, coreGlowPaint);
  }

  @override
  bool shouldRepaint(covariant CustomPainter oldDelegate) => true;
}