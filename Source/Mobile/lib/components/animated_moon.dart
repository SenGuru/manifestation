import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'dart:math' as math;
import 'package:mistel/models/meditation_models.dart';

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
    
    // Draw subtle shimmer
    _drawShimmer(canvas, center, radius);
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
    final illumination = phase.illumination;
    
    // For full moon, no shadow needed
    if (illumination >= 1.0) return;
    
    // Create clipping path for the moon circle
    canvas.save();
    final moonPath = Path()..addOval(Rect.fromCircle(center: center, radius: radius));
    canvas.clipPath(moonPath);
    
    final shadowPaint = Paint()
      ..color = Colors.black.withOpacity(0.8);
    
    // Draw shadow based on phase
    switch (phase) {
      case MoonPhase.newMoon:
        // Complete shadow
        canvas.drawCircle(center, radius, shadowPaint);
        break;
        
      case MoonPhase.waxingCrescent:
        // Shadow from right side
        final shadowRect = Rect.fromLTWH(
          center.dx - radius * 0.2,
          center.dy - radius,
          radius * 2.2,
          radius * 2,
        );
        canvas.drawOval(shadowRect, shadowPaint);
        break;
        
      case MoonPhase.firstQuarter:
        // Shadow covers right half
        final shadowRect = Rect.fromLTWH(
          center.dx,
          center.dy - radius,
          radius,
          radius * 2,
        );
        canvas.drawRect(shadowRect, shadowPaint);
        break;
        
      case MoonPhase.waxingGibbous:
        // Small shadow on right
        final shadowRect = Rect.fromLTWH(
          center.dx + radius * 0.4,
          center.dy - radius,
          radius * 0.8,
          radius * 2,
        );
        canvas.drawOval(shadowRect, shadowPaint);
        break;
        
      case MoonPhase.waningGibbous:
        // Small shadow on left
        final shadowRect = Rect.fromLTWH(
          center.dx - radius * 1.2,
          center.dy - radius,
          radius * 0.8,
          radius * 2,
        );
        canvas.drawOval(shadowRect, shadowPaint);
        break;
        
      case MoonPhase.lastQuarter:
        // Shadow covers left half
        final shadowRect = Rect.fromLTWH(
          center.dx - radius,
          center.dy - radius,
          radius,
          radius * 2,
        );
        canvas.drawRect(shadowRect, shadowPaint);
        break;
        
      case MoonPhase.waningCrescent:
        // Shadow from left side
        final shadowRect = Rect.fromLTWH(
          center.dx - radius * 2,
          center.dy - radius,
          radius * 2.2,
          radius * 2,
        );
        canvas.drawOval(shadowRect, shadowPaint);
        break;
        
      case MoonPhase.fullMoon:
        // No shadow
        break;
    }
    
    canvas.restore();
  }

  void _drawShimmer(Canvas canvas, Offset center, double radius) {
    if (intensity < 0.5) return;
    
    final shimmerOpacity = intensity * 0.2;
    
    // Simple shimmer effect
    final shimmerPaint = Paint()
      ..shader = LinearGradient(
        begin: Alignment.topLeft,
        end: Alignment.bottomRight,
        colors: [
          Colors.transparent,
          Colors.white.withOpacity(shimmerOpacity),
          Colors.transparent,
        ],
        stops: [0.0, 0.5 + shimmerOffset * 0.2, 1.0],
      ).createShader(Rect.fromCircle(center: center, radius: radius));
    
    canvas.drawCircle(center, radius, shimmerPaint);
  }

  @override
  bool shouldRepaint(covariant CustomPainter oldDelegate) => true;
}