import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'dart:math' as math;
import 'package:fun/models/meditation_models.dart';

class AnimatedGeometric extends StatefulWidget {
  final GeometricPattern pattern;
  final double intensity;
  final double size;

  const AnimatedGeometric({
    Key? key,
    this.pattern = GeometricPattern.mandala,
    required this.intensity,
    required this.size,
  }) : super(key: key);

  @override
  _AnimatedGeometricState createState() => _AnimatedGeometricState();
}

class _AnimatedGeometricState extends State<AnimatedGeometric>
    with TickerProviderStateMixin {
  late AnimationController _rotationController;
  late AnimationController _pulseController;
  
  late Animation<double> _rotationAnimation;
  late Animation<double> _pulseAnimation;

  @override
  void initState() {
    super.initState();
    
    // Rotation animation - simple like candle
    _rotationController = AnimationController(
      duration: Duration(milliseconds: 8000),
      vsync: this,
    );
    
    // Pulse animation
    _pulseController = AnimationController(
      duration: Duration(milliseconds: 3000),
      vsync: this,
    );
    
    _rotationAnimation = Tween<double>(
      begin: -0.3,
      end: 0.3,
    ).animate(CurvedAnimation(
      parent: _rotationController,
      curve: Curves.easeInOut,
    ));
    
    _pulseAnimation = Tween<double>(
      begin: 0.8,
      end: 1.2,
    ).animate(CurvedAnimation(
      parent: _pulseController,
      curve: Curves.easeInOut,
    ));
    
    _startAnimations();
  }

  void _startAnimations() {
    if (widget.intensity > 0.3) {
      _rotationController.repeat(reverse: true);
      _pulseController.repeat(reverse: true);
    }
  }

  @override
  void didUpdateWidget(AnimatedGeometric oldWidget) {
    super.didUpdateWidget(oldWidget);
    if (widget.intensity != oldWidget.intensity) {
      if (widget.intensity > 0.3) {
        _startAnimations();
      } else {
        _rotationController.stop();
        _pulseController.stop();
      }
    }
  }

  @override
  void dispose() {
    _rotationController.dispose();
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
          _rotationController,
          _pulseController,
        ]),
        builder: (context, child) {
          return CustomPaint(
            size: Size(widget.size.w, widget.size.h),
            painter: GeometricPainter(
              pattern: widget.pattern,
              intensity: widget.intensity,
              pulseAnimation: _pulseAnimation.value,
              rotationAnimation: _rotationAnimation.value,
            ),
          );
        },
      ),
    );
  }
}

class GeometricPainter extends CustomPainter {
  final GeometricPattern pattern;
  final double intensity;
  final double pulseAnimation;
  final double rotationAnimation;

  GeometricPainter({
    required this.pattern,
    required this.intensity,
    required this.pulseAnimation,
    required this.rotationAnimation,
  });

  @override
  void paint(Canvas canvas, Size size) {
    final center = Offset(size.width / 2, size.height / 2);
    final radius = size.width / 3;
    
    switch (pattern) {
      case GeometricPattern.mandala:
        _drawMandala(canvas, center, radius);
        break;
      case GeometricPattern.flowerOfLife:
        _drawFlowerOfLife(canvas, center, radius);
        break;
      case GeometricPattern.spirals:
        _drawSpirals(canvas, center, radius);
        break;
      case GeometricPattern.fractals:
        _drawFractals(canvas, center, radius);
        break;
    }
  }

  void _drawMandala(Canvas canvas, Offset center, double radius) {
    final colors = pattern.colors;
    final petalCount = 8;
    
    for (int i = 0; i < petalCount; i++) {
      final angle = (i * 2 * math.pi / petalCount) + rotationAnimation;
      final petalRadius = radius * pulseAnimation * 0.8;
      
      final petalPaint = Paint()
        ..color = colors[i % colors.length].withOpacity(intensity * 0.7)
        ..style = PaintingStyle.stroke
        ..strokeWidth = 3.w;
      
      // Simple petal as circle
      final petalCenter = Offset(
        center.dx + petalRadius * 0.5 * math.cos(angle),
        center.dy + petalRadius * 0.5 * math.sin(angle),
      );
      
      canvas.drawCircle(petalCenter, petalRadius * 0.3, petalPaint);
    }
    
    // Center circle
    final centerPaint = Paint()
      ..color = colors[0].withOpacity(intensity * 0.8);
    canvas.drawCircle(center, 15.r * pulseAnimation, centerPaint);
  }

  void _drawFlowerOfLife(Canvas canvas, Offset center, double radius) {
    final colors = pattern.colors;
    final circleCount = 6;
    final circleRadius = radius * 0.4;
    
    // Center circle
    final centerPaint = Paint()
      ..color = colors[0].withOpacity(intensity * 0.6)
      ..style = PaintingStyle.stroke
      ..strokeWidth = 2.w;
    canvas.drawCircle(center, circleRadius * pulseAnimation, centerPaint);
    
    // Surrounding circles
    for (int i = 0; i < circleCount; i++) {
      final angle = (i * 2 * math.pi / circleCount) + rotationAnimation;
      final circleCenter = Offset(
        center.dx + circleRadius * math.cos(angle) * pulseAnimation,
        center.dy + circleRadius * math.sin(angle) * pulseAnimation,
      );
      
      final circlePaint = Paint()
        ..color = colors[i % colors.length].withOpacity(intensity * 0.5)
        ..style = PaintingStyle.stroke
        ..strokeWidth = 2.w;
      
      canvas.drawCircle(circleCenter, circleRadius * 0.7, circlePaint);
    }
  }

  void _drawSpirals(Canvas canvas, Offset center, double radius) {
    final colors = pattern.colors;
    final spiralCount = 3;
    
    for (int i = 0; i < spiralCount; i++) {
      final spiralPaint = Paint()
        ..color = colors[i % colors.length].withOpacity(intensity * 0.6)
        ..style = PaintingStyle.stroke
        ..strokeWidth = 2.w;
      
      final path = Path();
      final startAngle = (i * 2 * math.pi / spiralCount) + rotationAnimation;
      
      // Simple spiral
      for (double t = 0; t < 3 * math.pi; t += 0.2) {
        final spiralRadius = (t / (3 * math.pi)) * radius * pulseAnimation;
        final x = center.dx + spiralRadius * math.cos(t + startAngle);
        final y = center.dy + spiralRadius * math.sin(t + startAngle);
        
        if (t == 0) {
          path.moveTo(x, y);
        } else {
          path.lineTo(x, y);
        }
      }
      
      canvas.drawPath(path, spiralPaint);
    }
  }

  void _drawFractals(Canvas canvas, Offset center, double radius) {
    final colors = pattern.colors;
    final branchCount = 6;
    
    for (int i = 0; i < branchCount; i++) {
      final angle = (i * 2 * math.pi / branchCount) + rotationAnimation;
      final branchPaint = Paint()
        ..color = colors[i % colors.length].withOpacity(intensity * 0.7)
        ..strokeWidth = 2.w;
      
      // Simple branching lines
      final endX = center.dx + radius * pulseAnimation * math.cos(angle);
      final endY = center.dy + radius * pulseAnimation * math.sin(angle);
      
      canvas.drawLine(center, Offset(endX, endY), branchPaint);
      
      // Add small branches
      final midX = center.dx + radius * 0.7 * pulseAnimation * math.cos(angle);
      final midY = center.dy + radius * 0.7 * pulseAnimation * math.sin(angle);
      
      for (int j = 0; j < 2; j++) {
        final branchAngle = angle + (j == 0 ? 0.5 : -0.5);
        final branchEndX = midX + radius * 0.3 * math.cos(branchAngle);
        final branchEndY = midY + radius * 0.3 * math.sin(branchAngle);
        
        canvas.drawLine(Offset(midX, midY), Offset(branchEndX, branchEndY), branchPaint);
      }
    }
  }

  @override
  bool shouldRepaint(covariant CustomPainter oldDelegate) => true;
}