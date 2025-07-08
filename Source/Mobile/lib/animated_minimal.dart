import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:manifestation_app/colors.dart';
import 'dart:math' as math;

import 'package:manifestation_app/meditation_constants.dart';

class AnimatedMinimal extends StatefulWidget {
  final double intensity;
  final double size;
  final double breathingSpeed; // NEW: Add breathing speed parameter

  const AnimatedMinimal({
    Key? key,
    required this.intensity,
    required this.size,
    this.breathingSpeed = 0.8, // Default speed
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
    _setupAnimations();
  }

  void _setupAnimations() {
    // Calculate dynamic duration based on breathing speed
    // Base duration from AppConstants, adjusted by speed
    final baseDuration = AppConstants.minimalBreathing.inMilliseconds;
    final adjustedDuration = (baseDuration / widget.breathingSpeed).round();
    
    // Main breathing animation (inhale/exhale) - now dynamic
    _breathingController = AnimationController(
      duration: Duration(milliseconds: adjustedDuration),
      vsync: this,
    );
    
    // Pulse animation for depth - also adjusted
    _pulseController = AnimationController(
      duration: Duration(milliseconds: (2000 / widget.breathingSpeed).round()),
      vsync: this,
    );
    
    // Subtle rotation animation - slightly adjusted
    _rotationController = AnimationController(
      duration: Duration(milliseconds: (12000 / widget.breathingSpeed).round()),
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
      _breathingController.repeat(reverse: true); // BACK TO ORIGINAL - this was actually better
      _pulseController.repeat(reverse: true); // BACK TO ORIGINAL - this was actually better
      _rotationController.repeat();
    }
  }

  void _stopAnimations() {
    _breathingController.stop();
    _pulseController.stop();
    _rotationController.stop();
  }

  void _restartAnimationsWithNewSpeed() {
    // Stop current animations
    _stopAnimations();
    
    // Dispose old controllers
    _breathingController.dispose();
    _pulseController.dispose();
    _rotationController.dispose();
    
    // Setup new animations with updated speed
    _setupAnimations();
  }

  @override
  void didUpdateWidget(AnimatedMinimal oldWidget) {
    super.didUpdateWidget(oldWidget);
    
    // Check if breathing speed changed
    if (widget.breathingSpeed != oldWidget.breathingSpeed) {
      _restartAnimationsWithNewSpeed();
      return;
    }
    
    // Check if intensity changed
    if (widget.intensity != oldWidget.intensity) {
      if (widget.intensity > 0.3) {
        _startAnimations();
      } else {
        _stopAnimations();
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
                breathingSpeed: widget.breathingSpeed, // Pass speed to painter
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
  final double breathingSpeed; // NEW: Add breathing speed

  MinimalPainter({
    required this.intensity,
    required this.breathingScale,
    required this.pulseScale,
    required this.rotation,
    required this.breathingSpeed,
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
    
    // Adjust stroke width based on breathing speed for visual feedback
    final speedMultiplier = 0.5 + (breathingSpeed * 0.5); // Range: 0.75 - 1.25
    
    // Outer circle - main breathing circle
    final outerRadius = baseRadius * breathingScale * 1.3;
    final outerPaint = Paint()
      ..color = colors[0].withOpacity(0.3 * intensity)
      ..style = PaintingStyle.stroke
      ..strokeWidth = 3.w * speedMultiplier;
    
    canvas.drawCircle(center, outerRadius, outerPaint);
    
    // Middle circle - pulse animation
    final middleRadius = baseRadius * breathingScale;
    final middlePaint = Paint()
      ..color = colors[0].withOpacity(0.6 * intensity)
      ..style = PaintingStyle.stroke
      ..strokeWidth = 4.w * speedMultiplier;
    
    canvas.drawCircle(center, middleRadius, middlePaint);
    
    // Inner circle - counter breathing
    final innerRadius = baseRadius * 0.6 * (2.0 - breathingScale) * pulseScale;
    final innerPaint = Paint()
      ..color = colors[1].withOpacity(0.8 * intensity)
      ..style = PaintingStyle.stroke
      ..strokeWidth = 2.w * speedMultiplier;
    
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
    final speedMultiplier = 0.5 + (breathingSpeed * 0.5);
    
    // Draw petals/leaves pattern
    for (int i = 0; i < 8; i++) {
      final angle = (i * math.pi / 4) + rotation * 0.2;
      final startX = center.dx + patternRadius * 0.3 * math.cos(angle);
      final startY = center.dy + patternRadius * 0.3 * math.sin(angle);
      final endX = center.dx + patternRadius * math.cos(angle);
      final endY = center.dy + patternRadius * math.sin(angle);
      
      final petalPaint = Paint()
        ..color = colors[i % 2].withOpacity(0.4 * intensity)
        ..strokeWidth = 2.w * speedMultiplier
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
        3.r * pulseScale * speedMultiplier,
        dotPaint,
      );
    }
  }

  void _drawCenterPoint(Canvas canvas, Offset center, double baseRadius) {
    final colors = AppColors.minimalGradient;
    final speedMultiplier = 0.8 + (breathingSpeed * 0.4); // Subtle size adjustment
    
    // Breathing center point
    final centerRadius = 8.r * breathingScale * intensity * speedMultiplier;
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
    
    canvas.drawCircle(center, 4.r * pulseScale * speedMultiplier, corePaint);
  }

  void _drawGlow(Canvas canvas, Offset center, double baseRadius) {
    if (intensity < 0.4) return;
    
    final colors = AppColors.minimalGradient;
    final glowRadius = baseRadius * breathingScale * 1.5;
    final speedMultiplier = 0.7 + (breathingSpeed * 0.6); // More pronounced glow adjustment
    
    // Outer glow
    final outerGlowPaint = Paint()
      ..color = colors[0].withOpacity(0.1 * intensity * speedMultiplier)
      ..maskFilter = MaskFilter.blur(BlurStyle.normal, 30.r);
    
    canvas.drawCircle(center, glowRadius, outerGlowPaint);
    
    // Inner glow
    final innerGlowPaint = Paint()
      ..color = colors[1].withOpacity(0.15 * intensity * speedMultiplier)
      ..maskFilter = MaskFilter.blur(BlurStyle.normal, 15.r);
    
    canvas.drawCircle(center, glowRadius * 0.7, innerGlowPaint);
    
    // Core glow
    final coreGlowPaint = Paint()
      ..color = Colors.white.withOpacity(0.2 * intensity * speedMultiplier)
      ..maskFilter = MaskFilter.blur(BlurStyle.normal, 8.r);
    
    canvas.drawCircle(center, baseRadius * 0.3 * breathingScale, coreGlowPaint);
  }

  @override
  bool shouldRepaint(covariant CustomPainter oldDelegate) => true;
}