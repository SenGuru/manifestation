import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../constants/app_theme.dart';
import 'dart:math' as math;

import 'package:fun/constants/meditation_constants.dart';


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
  
  late Animation<double> _breathingAnimation;
  late Animation<double> _pulseAnimation;

  @override
  void initState() {
    super.initState();
    
    // Main breathing animation - simple and smooth like candle
    _breathingController = AnimationController(
      duration: Duration(milliseconds: 3000),
      vsync: this,
    );
    
    // Pulse animation for depth
    _pulseController = AnimationController(
      duration: Duration(milliseconds: 2000),
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
    
    _startAnimations();
  }

  void _startAnimations() {
    if (widget.intensity > 0.3) {
      _breathingController.repeat(reverse: true);
      _pulseController.repeat(reverse: true);
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
      }
    }
  }

  @override
  void dispose() {
    _breathingController.dispose();
    _pulseController.dispose();
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
        ]),
        builder: (context, child) {
          return CustomPaint(
            size: Size(widget.size.w, widget.size.h),
            painter: MinimalPainter(
              intensity: widget.intensity,
              breathingScale: _breathingAnimation.value,
              pulseScale: _pulseAnimation.value,
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

  MinimalPainter({
    required this.intensity,
    required this.breathingScale,
    required this.pulseScale,
  });

  @override
  void paint(Canvas canvas, Size size) {
    final center = Offset(size.width / 2, size.height / 2);
    final baseRadius = size.width / 3;
    
    // Draw simple breathing circles - like candle flame layers
    _drawBreathingCircles(canvas, center, baseRadius);
    
    // Draw center focus point
    _drawCenterPoint(canvas, center, baseRadius);
  }

  void _drawBreathingCircles(Canvas canvas, Offset center, double baseRadius) {
    final colors = AppTheme.visualMeditationGradient;
    
    // Outer circle - main breathing circle
    final outerRadius = baseRadius * breathingScale * 1.2;
    final outerPaint = Paint()
      ..color = colors[0].withOpacity(0.3 * intensity)
      ..style = PaintingStyle.stroke
      ..strokeWidth = 3.w;
    
    canvas.drawCircle(center, outerRadius, outerPaint);
    
    // Middle circle - pulse animation
    final middleRadius = baseRadius * breathingScale * 0.8;
    final middlePaint = Paint()
      ..color = colors[0].withOpacity(0.5 * intensity)
      ..style = PaintingStyle.stroke
      ..strokeWidth = 4.w;
    
    canvas.drawCircle(center, middleRadius, middlePaint);
    
    // Inner circle - counter breathing (smooth reverse motion)
    final innerRadius = baseRadius * 0.4 * (2.0 - breathingScale) * pulseScale;
    final innerPaint = Paint()
      ..color = colors[1].withOpacity(0.7 * intensity)
      ..style = PaintingStyle.stroke
      ..strokeWidth = 2.w;
    
    canvas.drawCircle(center, innerRadius, innerPaint);
  }

  void _drawCenterPoint(Canvas canvas, Offset center, double baseRadius) {
    final colors = AppTheme.visualMeditationGradient;
    
    // Simple center point that breathes
    final centerRadius = 12.r * breathingScale * intensity;
    final centerPaint = Paint()
      ..shader = RadialGradient(
        colors: [
          colors[1].withOpacity(0.8),
          colors[0].withOpacity(0.3),
          Colors.transparent,
        ],
        stops: [0.0, 0.7, 1.0],
      ).createShader(Rect.fromCircle(center: center, radius: centerRadius));
    
    canvas.drawCircle(center, centerRadius, centerPaint);
    
    // Inner core - simple white dot
    final corePaint = Paint()
      ..color = Colors.white.withOpacity(0.9 * intensity);
    
    canvas.drawCircle(center, 4.r * pulseScale, corePaint);
  }

  @override
  bool shouldRepaint(covariant CustomPainter oldDelegate) => true;
}