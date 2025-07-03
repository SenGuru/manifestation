import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:manifestation_app/meditation_constants.dart';
import 'package:manifestation_app/meditation_models.dart';
import 'dart:math' as math;

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
  late AnimationController _morphController;
  
  late Animation<double> _rotationAnimation;
  late Animation<double> _pulseAnimation;
  late Animation<double> _morphAnimation;

  @override
  void initState() {
    super.initState();
    
    // Rotation animation
    _rotationController = AnimationController(
      duration: AppConstants.geometricRotation,
      vsync: this,
    );
    
    // Pulse animation
    _pulseController = AnimationController(
      duration: Duration(milliseconds: 3000),
      vsync: this,
    );
    
    // Morph animation for pattern changes
    _morphController = AnimationController(
      duration: Duration(milliseconds: 4000),
      vsync: this,
    );
    
    _rotationAnimation = Tween<double>(
      begin: 0.0,
      end: 2 * math.pi,
    ).animate(CurvedAnimation(
      parent: _rotationController,
      curve: Curves.linear,
    ));
    
    _pulseAnimation = Tween<double>(
      begin: 0.8,
      end: 1.2,
    ).animate(CurvedAnimation(
      parent: _pulseController,
      curve: Curves.easeInOut,
    ));
    
    _morphAnimation = Tween<double>(
      begin: 0.0,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _morphController,
      curve: Curves.easeInOut,
    ));
    
    _startAnimations();
  }

  void _startAnimations() {
    if (widget.intensity > 0.3) {
      _rotationController.repeat();
      _pulseController.repeat(reverse: true);
      _morphController.repeat(reverse: true);
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
        _morphController.stop();
      }
    }
  }

  @override
  void dispose() {
    _rotationController.dispose();
    _pulseController.dispose();
    _morphController.dispose();
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
          _morphController,
        ]),
        builder: (context, child) {
          return CustomPaint(
            size: Size(widget.size.w, widget.size.h),
            painter: GeometricPainter(
              pattern: widget.pattern,
              intensity: widget.intensity,
              rotation: _rotationAnimation.value,
              pulse: _pulseAnimation.value,
              morph: _morphAnimation.value,
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
  final double rotation;
  final double pulse;
  final double morph;

  GeometricPainter({
    required this.pattern,
    required this.intensity,
    required this.rotation,
    required this.pulse,
    required this.morph,
  });

  @override
  void paint(Canvas canvas, Size size) {
    final center = Offset(size.width / 2, size.height / 2);
    final radius = size.width / 2.5;
    
    canvas.save();
    canvas.translate(center.dx, center.dy);
    canvas.rotate(rotation * 0.5); // Slow rotation
    canvas.translate(-center.dx, -center.dy);
    
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
    
    canvas.restore();
    
    // Add glow effect
    _drawGlow(canvas, center, radius);
  }

  void _drawMandala(Canvas canvas, Offset center, double radius) {
    final colors = pattern.colors;
    
    // Draw concentric layers of petals
    for (int layer = 0; layer < 4; layer++) {
      final layerRadius = radius * (0.3 + layer * 0.2) * pulse;
      final petals = 6 + layer * 2;
      final layerOpacity = (1.0 - layer * 0.2) * intensity;
      
      for (int i = 0; i < petals; i++) {
        final angle = (2 * math.pi * i / petals) + rotation * (layer % 2 == 0 ? 1 : -1) * 0.3;
        final petalCenter = Offset(
          center.dx + layerRadius * 0.6 * math.cos(angle),
          center.dy + layerRadius * 0.6 * math.sin(angle),
        );
        
        // Draw petal shape
        final petalPath = Path();
        final petalSize = radius * 0.15 * pulse;
        
        petalPath.addOval(Rect.fromCenter(
          center: petalCenter,
          width: petalSize,
          height: petalSize * 2,
        ));
        
        final petalPaint = Paint()
          ..color = colors[layer % 2].withOpacity(layerOpacity)
          ..style = PaintingStyle.stroke
          ..strokeWidth = (2 + layer).w;
        
        canvas.save();
        canvas.translate(petalCenter.dx, petalCenter.dy);
        canvas.rotate(angle);
        canvas.translate(-petalCenter.dx, -petalCenter.dy);
        canvas.drawPath(petalPath, petalPaint);
        canvas.restore();
      }
      
      // Draw connecting circle
      final circlePaint = Paint()
        ..color = colors[layer % 2].withOpacity(layerOpacity * 0.5)
        ..style = PaintingStyle.stroke
        ..strokeWidth = 1.w;
      
      canvas.drawCircle(center, layerRadius, circlePaint);
    }
    
    // Central symbol
    _drawCentralSymbol(canvas, center, radius * 0.2 * pulse);
  }

  void _drawFlowerOfLife(Canvas canvas, Offset center, double radius) {
    final colors = pattern.colors;
    final circleRadius = radius * 0.25 * pulse;
    
    // Central circle
    final centralPaint = Paint()
      ..color = colors[0].withOpacity(0.8 * intensity)
      ..style = PaintingStyle.stroke
      ..strokeWidth = 3.w;
    
    canvas.drawCircle(center, circleRadius, centralPaint);
    
    // First ring of circles
    for (int i = 0; i < 6; i++) {
      final angle = i * math.pi / 3 + rotation * 0.2;
      final circleCenter = Offset(
        center.dx + circleRadius * math.cos(angle),
        center.dy + circleRadius * math.sin(angle),
      );
      
      final ringPaint = Paint()
        ..color = colors[1].withOpacity(0.6 * intensity)
        ..style = PaintingStyle.stroke
        ..strokeWidth = 2.w;
      
      canvas.drawCircle(circleCenter, circleRadius, ringPaint);
    }
    
    // Second ring of circles
    for (int i = 0; i < 12; i++) {
      final angle = i * math.pi / 6 + rotation * -0.1;
      final distance = circleRadius * math.sqrt(3);
      final circleCenter = Offset(
        center.dx + distance * math.cos(angle),
        center.dy + distance * math.sin(angle),
      );
      
      final outerPaint = Paint()
        ..color = colors[0].withOpacity(0.4 * intensity)
        ..style = PaintingStyle.stroke
        ..strokeWidth = 1.5.w;
      
      canvas.drawCircle(circleCenter, circleRadius, outerPaint);
    }
  }

  void _drawSpirals(Canvas canvas, Offset center, double radius) {
    final colors = pattern.colors;
    
    // Draw multiple spirals
    for (int spiral = 0; spiral < 3; spiral++) {
      final spiralPath = Path();
      final spiralRotation = rotation + spiral * 2 * math.pi / 3;
      final spiralRadius = radius * pulse;
      final turns = 2.5;
      
      for (int i = 0; i <= 200; i++) {
        final t = i / 200.0;
        final angle = t * turns * 2 * math.pi + spiralRotation;
        final currentRadius = spiralRadius * t * (1 + 0.3 * math.sin(t * 4 * math.pi + morph * 2 * math.pi));
        
        final x = center.dx + currentRadius * math.cos(angle);
        final y = center.dy + currentRadius * math.sin(angle);
        
        if (i == 0) {
          spiralPath.moveTo(x, y);
        } else {
          spiralPath.lineTo(x, y);
        }
      }
      
      final spiralPaint = Paint()
        ..color = colors[spiral % 2].withOpacity((0.8 - spiral * 0.2) * intensity)
        ..style = PaintingStyle.stroke
        ..strokeWidth = (4 - spiral).w
        ..strokeCap = StrokeCap.round;
      
      canvas.drawPath(spiralPath, spiralPaint);
    }
  }

  void _drawFractals(Canvas canvas, Offset center, double radius) {
    final colors = pattern.colors;
    
    // Draw fractal tree pattern
    _drawFractalBranch(
      canvas,
      center,
      radius * pulse,
      -math.pi / 2 + rotation * 0.1, // Start pointing up
      5, // Depth
      colors[0].withOpacity(intensity),
    );
  }

  void _drawFractalBranch(Canvas canvas, Offset start, double length, double angle, int depth, Color color) {
    if (depth <= 0 || length < 5) return;
    
    final end = Offset(
      start.dx + length * math.cos(angle),
      start.dy + length * math.sin(angle),
    );
    
    final branchPaint = Paint()
      ..color = color.withOpacity(depth / 5.0)
      ..strokeWidth = depth.toDouble().w
      ..strokeCap = StrokeCap.round;
    
    canvas.drawLine(start, end, branchPaint);
    
    // Draw child branches
    final angleOffset = math.pi / 6 + morph * math.pi / 12;
    final lengthRatio = 0.7 + morph * 0.1;
    
    _drawFractalBranch(
      canvas,
      end,
      length * lengthRatio,
      angle - angleOffset,
      depth - 1,
      color,
    );
    
    _drawFractalBranch(
      canvas,
      end,
      length * lengthRatio,
      angle + angleOffset,
      depth - 1,
      color,
    );
  }

  void _drawCentralSymbol(Canvas canvas, Offset center, double size) {
    final colors = pattern.colors;
    
    // Draw a central sacred symbol
    final symbolPaint = Paint()
      ..color = colors[0].withOpacity(0.9 * intensity)
      ..style = PaintingStyle.stroke
      ..strokeWidth = 3.w;
    
    // Draw a complex central pattern
    for (int i = 0; i < 8; i++) {
      final angle = i * math.pi / 4 + rotation;
      final startRadius = size * 0.3;
      final endRadius = size;
      
      final start = Offset(
        center.dx + startRadius * math.cos(angle),
        center.dy + startRadius * math.sin(angle),
      );
      
      final end = Offset(
        center.dx + endRadius * math.cos(angle),
        center.dy + endRadius * math.sin(angle),
      );
      
      canvas.drawLine(start, end, symbolPaint);
    }
    
    // Central circle
    final centralCirclePaint = Paint()
      ..color = colors[1].withOpacity(0.7 * intensity)
      ..style = PaintingStyle.fill;
    
    canvas.drawCircle(center, size * 0.2, centralCirclePaint);
  }

  void _drawGlow(Canvas canvas, Offset center, double radius) {
    if (intensity < 0.4) return;
    
    final colors = pattern.colors;
    
    // Outer glow
    final glowPaint = Paint()
      ..color = colors[0].withOpacity(0.1 * intensity)
      ..maskFilter = MaskFilter.blur(BlurStyle.normal, 40.r);
    
    canvas.drawCircle(center, radius * pulse * 1.5, glowPaint);
    
    // Inner glow
    final innerGlowPaint = Paint()
      ..color = colors[1].withOpacity(0.15 * intensity)
      ..maskFilter = MaskFilter.blur(BlurStyle.normal, 20.r);
    
    canvas.drawCircle(center, radius * pulse, innerGlowPaint);
  }

  @override
  bool shouldRepaint(covariant CustomPainter oldDelegate) => true;
}