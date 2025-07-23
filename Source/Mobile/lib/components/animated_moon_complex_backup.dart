/*
import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:fun/constants/meditation_constants.dart';
import 'dart:math' as math;

import 'package:fun/models/meditation_models.dart';

class AnimatedMoon extends StatefulWidget {
  final MoonPhase phase;
  final double intensity;
  final double size;

  const AnimatedMoon({
    Key? key,
    required this.phase,
    required this.intensity,
    required this.size,
  }) : super(key: key);

  @override
  _AnimatedMoonState createState() => _AnimatedMoonState();
}

class _AnimatedMoonState extends State<AnimatedMoon>
    with TickerProviderStateMixin {
  late AnimationController _glowController;
  late AnimationController _shimmerController;
  
  late Animation<double> _glowPulse;
  late Animation<double> _shimmerOffset;

  @override
  void initState() {
    super.initState();
    
    // Glow pulse animation - simple like candle
    _glowController = AnimationController(
      duration: Duration(milliseconds: 3000),
      vsync: this,
    );
    
    // Shimmer effect animation
    _shimmerController = AnimationController(
      duration: Duration(milliseconds: 2000),
      vsync: this,
    );
    
    // Setup animations
    _glowPulse = Tween<double>(
      begin: 0.7,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _glowController,
      curve: Curves.easeInOut,
    ));
    
    _shimmerOffset = Tween<double>(
      begin: -1.0,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _shimmerController,
      curve: Curves.easeInOut,
    ));
    
    // Start animations
    _startAnimations();
  }

  void _startAnimations() {
    if (widget.intensity > 0.3) {
      _glowController.repeat(reverse: true);
      _shimmerController.repeat(reverse: true);
    }
  }

  @override
  void didUpdateWidget(AnimatedMoon oldWidget) {
    super.didUpdateWidget(oldWidget);
    if (widget.intensity != oldWidget.intensity) {
      if (widget.intensity > 0.3) {
        _startAnimations();
      } else {
        _glowController.stop();
        _shimmerController.stop();
      }
    }
  }

  @override
  void dispose() {
    _glowController.dispose();
    _shimmerController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return SizedBox(
      width: widget.size.w,
      height: widget.size.h,
      child: AnimatedBuilder(
        animation: Listenable.merge([
          _glowController,
          _shimmerController,
        ]),
        builder: (context, child) {
          return CustomPaint(
            size: Size(widget.size.w, widget.size.h),
            painter: MoonPainter(
              phase: widget.phase,
              intensity: widget.intensity,
              glowPulse: _glowPulse.value,
              shimmerOffset: _shimmerOffset.value,
            ),
          );
        },
      ),
    );
  }
}

class MoonPainter extends CustomPainter {
  final MoonPhase phase;
  final double intensity;
  final double glowPulse;
  final double shimmerOffset;

  MoonPainter({
    required this.phase,
    required this.intensity,
    required this.glowPulse,
    required this.shimmerOffset,
  });

  @override
  void paint(Canvas canvas, Size size) {
    final center = Offset(size.width / 2, size.height / 2);
    final radius = size.width / 2 - 10.w;
    
    // Draw simple glow
    _drawGlow(canvas, center, radius);
    
    // Draw moon body
    _drawMoon(canvas, center, radius);
    
    // Draw phase shadow if needed
    _drawPhase(canvas, center, radius);
  }

  void _drawGlow(Canvas canvas, Offset center, double radius) {
    if (intensity < 0.3) return;
    
    final glowIntensity = intensity * glowPulse;
    final glowColor = phase.glowColor;
    
    // Simple glow like candle
    final glowPaint = Paint()
      ..color = glowColor.withOpacity(0.3 * glowIntensity)
      ..maskFilter = MaskFilter.blur(BlurStyle.normal, 30.r);
    
    canvas.drawCircle(center, radius + 40.w, glowPaint);
  }

  void _drawMoon(Canvas canvas, Offset center, double radius) {
    // Simple moon body with gradient
    final moonPaint = Paint()
      ..shader = RadialGradient(
        colors: phase.moonGradient,
        stops: [0.0, 0.6, 1.0],
      ).createShader(Rect.fromCircle(center: center, radius: radius));
    
    canvas.drawCircle(center, radius, moonPaint);
  }

  void _drawPhase(Canvas canvas, Offset center, double radius) {
    // Simple phase shadow
    final illumination = phase.illumination;
    if (illumination >= 1.0) return; // Full moon, no shadow
    
    final shadowPaint = Paint()
      ..color = Colors.black.withOpacity(0.7 * (1.0 - illumination));
    
    // Simple shadow approach
    if (illumination < 0.5) {
      // New moon or crescent phases
      final shadowWidth = radius * 2 * (1.0 - illumination * 2);
      final shadowRect = Rect.fromCenter(
        center: Offset(center.dx + radius - shadowWidth / 2, center.dy),
        width: shadowWidth,
        height: radius * 2,
      );
      canvas.drawOval(shadowRect, shadowPaint);
    }
  }
      final glowOpacity = (0.15 - i * 0.04) * glowIntensity;
      
      final glowPaint = Paint()
        ..color = phase.glowColor.withOpacity(glowOpacity)
        ..maskFilter = MaskFilter.blur(BlurStyle.normal, (20 + i * 10).r);
      
      canvas.drawCircle(center, glowRadius, glowPaint);
    }
  }

  void _drawClouds(Canvas canvas, Offset center, double radius, Size size) {
    if (intensity < 0.3) return;
    
    final cloudPaint = Paint()
      ..color = Colors.white.withOpacity(0.1 * intensity)
      ..maskFilter = MaskFilter.blur(BlurStyle.normal, 15.r);
    
    // Animated cloud positions
    final cloudOffset = cloudsMove * size.width * 2;
    
    // Draw multiple cloud layers
    for (int i = 0; i < 4; i++) {
      final cloudX = (center.dx + cloudOffset + i * 100.w) % (size.width + 200.w) - 100.w;
      final cloudY = center.dy + (i - 2) * 40.h + math.sin(cloudsMove * 2 * math.pi + i) * 20.h;
      
      _drawCloud(canvas, Offset(cloudX, cloudY), (60 + i * 15).w, cloudPaint);
    }
  }

  void _drawCloud(Canvas canvas, Offset center, double size, Paint paint) {
    final cloudPath = Path();
    
    // Create fluffy cloud shape
    cloudPath.addOval(Rect.fromCenter(center: center, width: size, height: size * 0.6));
    cloudPath.addOval(Rect.fromCenter(center: center + Offset(-size * 0.3, 0), width: size * 0.8, height: size * 0.5));
    cloudPath.addOval(Rect.fromCenter(center: center + Offset(size * 0.3, 0), width: size * 0.7, height: size * 0.4));
    cloudPath.addOval(Rect.fromCenter(center: center + Offset(0, -size * 0.2), width: size * 0.6, height: size * 0.4));
    
    canvas.drawPath(cloudPath, paint);
  }

  void _drawMoonPhase(Canvas canvas, Offset center, double radius) {
    final moonPaint = Paint()
      ..shader = RadialGradient(
        colors: phase.moonGradient,
        stops: [0.0, 0.7, 1.0],
      ).createShader(Rect.fromCircle(center: center, radius: radius));
    
    // Draw full moon circle
    canvas.drawCircle(center, radius, moonPaint);
    
    // Draw phase shadow if not full moon
    if (phase != MoonPhase.fullMoon) {
      _drawPhaseShadow(canvas, center, radius);
    }
  }

  void _drawPhaseShadow(Canvas canvas, Offset center, double radius) {
    final shadowPaint = Paint()
      ..color = Color(0xFF1A202C).withOpacity(0.8)
      ..style = PaintingStyle.fill;
    
    final shadowPath = Path();
    
    switch (phase) {
      case MoonPhase.newMoon:
        shadowPath.addOval(Rect.fromCircle(center: center, radius: radius));
        break;
      case MoonPhase.waxingCrescent:
        shadowPath.addOval(Rect.fromCenter(
          center: center + Offset(-radius * 0.6, 0),
          width: radius * 1.4,
          height: radius * 2,
        ));
        break;
      case MoonPhase.firstQuarter:
        shadowPath.addRect(Rect.fromLTWH(
          center.dx - radius,
          center.dy - radius,
          radius,
          radius * 2,
        ));
        break;
      case MoonPhase.waxingGibbous:
        shadowPath.addOval(Rect.fromCenter(
          center: center + Offset(-radius * 0.3, 0),
          width: radius * 0.8,
          height: radius * 2,
        ));
        break;
      case MoonPhase.waningGibbous:
        shadowPath.addOval(Rect.fromCenter(
          center: center + Offset(radius * 0.3, 0),
          width: radius * 0.8,
          height: radius * 2,
        ));
        break;
      case MoonPhase.lastQuarter:
        shadowPath.addRect(Rect.fromLTWH(
          center.dx,
          center.dy - radius,
          radius,
          radius * 2,
        ));
        break;
      case MoonPhase.waningCrescent:
        shadowPath.addOval(Rect.fromCenter(
          center: center + Offset(radius * 0.6, 0),
          width: radius * 1.4,
          height: radius * 2,
        ));
        break;
      default:
        break;
    }
    
    // Clip the shadow to the moon circle
    canvas.clipPath(Path()..addOval(Rect.fromCircle(center: center, radius: radius)));
    canvas.drawPath(shadowPath, shadowPaint);
  }

  void _drawSurfaceDetails(Canvas canvas, Offset center, double radius) {
    // Only draw surface details if moon is sufficiently illuminated
    if (phase.illumination < 0.3) return;
    
    final craterPaint = Paint()
      ..color = Colors.grey.withOpacity(0.3 * intensity)
      ..style = PaintingStyle.fill;
    
    final mariaPaint = Paint()
      ..color = Colors.grey.withOpacity(0.2 * intensity)
      ..style = PaintingStyle.fill;
    
    // Draw maria (dark patches)
    canvas.drawOval(
      Rect.fromCenter(
        center: center + Offset(-radius * 0.3, -radius * 0.2 + craterFloat.h),
        width: radius * 0.4,
        height: radius * 0.3,
      ),
      mariaPaint,
    );
    
    canvas.drawOval(
      Rect.fromCenter(
        center: center + Offset(radius * 0.2, radius * 0.3 + craterFloat.h),
        width: radius * 0.3,
        height: radius * 0.25,
      ),
      mariaPaint,
    );
    
    // Draw craters
    final craterPositions = [
      Offset(-radius * 0.4, -radius * 0.5),
      Offset(radius * 0.3, -radius * 0.3),
      Offset(-radius * 0.2, radius * 0.4),
      Offset(radius * 0.4, radius * 0.2),
      Offset(0, -radius * 0.6),
    ];
    
    for (int i = 0; i < craterPositions.length; i++) {
      final craterCenter = center + craterPositions[i] + Offset(0, craterFloat.h * (i.isOdd ? 1 : -1));
      final craterRadius = (8 + i * 3).r;
      
      canvas.drawCircle(craterCenter, craterRadius, craterPaint);
      
      // Add rim highlight
      final rimPaint = Paint()
        ..color = Colors.white.withOpacity(0.1 * intensity)
        ..style = PaintingStyle.stroke
        ..strokeWidth = 1.w;
      
      canvas.drawCircle(craterCenter, craterRadius, rimPaint);
    }
  }

  void _drawShimmer(Canvas canvas, Offset center, double radius) {
    if (intensity < 0.5 || phase.illumination < 0.5) return;
    
    final shimmerPaint = Paint()
      ..shader = LinearGradient(
        begin: Alignment.topLeft,
        end: Alignment.bottomRight,
        colors: [
          Colors.transparent,
          Colors.white.withOpacity(0.3 * intensity),
          Colors.transparent,
        ],
        stops: [0.0, 0.5, 1.0],
        transform: GradientRotation(shimmerOffset * math.pi),
      ).createShader(Rect.fromCircle(center: center, radius: radius));
    
    canvas.drawCircle(center, radius, shimmerPaint);
  }

  void _drawAtmosphericGlow(Canvas canvas, Offset center, double radius) {
    if (intensity < 0.4) return;
    
    final atmosphericPaint = Paint()
      ..color = phase.glowColor.withOpacity(0.05 * intensity * glowPulse)
      ..maskFilter = MaskFilter.blur(BlurStyle.normal, 40.r);
    
    canvas.drawCircle(center, radius + 60.w, atmosphericPaint);
  }

  @override
  bool shouldRepaint(covariant CustomPainter oldDelegate) => true;
}
*/