import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'dart:math' as math;
import 'package:fun/models/meditation_models.dart';

class AnimatedNature extends StatefulWidget {
  final NaturePattern pattern;
  final double intensity;
  final double size;

  const AnimatedNature({
    Key? key,
    this.pattern = NaturePattern.ripples,
    required this.intensity,
    required this.size,
  }) : super(key: key);

  @override
  _AnimatedNatureState createState() => _AnimatedNatureState();
}

class _AnimatedNatureState extends State<AnimatedNature>
    with TickerProviderStateMixin {
  late AnimationController _waveController;
  late AnimationController _flowController;
  
  late Animation<double> _waveAnimation;
  late Animation<double> _flowAnimation;

  @override
  void initState() {
    super.initState();
    
    // Wave animation - simple like candle
    _waveController = AnimationController(
      duration: Duration(milliseconds: 3000),
      vsync: this,
    );
    
    // Flow animation
    _flowController = AnimationController(
      duration: Duration(milliseconds: 2000),
      vsync: this,
    );
    
    _waveAnimation = Tween<double>(
      begin: 0.8,
      end: 1.2,
    ).animate(CurvedAnimation(
      parent: _waveController,
      curve: Curves.easeInOut,
    ));
    
    _flowAnimation = Tween<double>(
      begin: 0.0,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _flowController,
      curve: Curves.easeInOut,
    ));
    
    _startAnimations();
  }

  void _startAnimations() {
    if (widget.intensity > 0.3) {
      _waveController.repeat(reverse: true);
      _flowController.repeat(reverse: true);
    }
  }

  @override
  void didUpdateWidget(AnimatedNature oldWidget) {
    super.didUpdateWidget(oldWidget);
    if (widget.intensity != oldWidget.intensity) {
      if (widget.intensity > 0.3) {
        _startAnimations();
      } else {
        _waveController.stop();
        _flowController.stop();
      }
    }
  }

  @override
  void dispose() {
    _waveController.dispose();
    _flowController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return SizedBox(
      width: widget.size.w,
      height: widget.size.h,
      child: AnimatedBuilder(
        animation: Listenable.merge([
          _waveController,
          _flowController,
        ]),
        builder: (context, child) {
          return CustomPaint(
            size: Size(widget.size.w, widget.size.h),
            painter: NaturePainter(
              pattern: widget.pattern,
              intensity: widget.intensity,
              waveAnimation: _waveAnimation.value,
              flowAnimation: _flowAnimation.value,
            ),
          );
        },
      ),
    );
  }
}

class NaturePainter extends CustomPainter {
  final NaturePattern pattern;
  final double intensity;
  final double waveAnimation;
  final double flowAnimation;

  NaturePainter({
    required this.pattern,
    required this.intensity,
    required this.waveAnimation,
    required this.flowAnimation,
  });

  @override
  void paint(Canvas canvas, Size size) {
    final center = Offset(size.width / 2, size.height / 2);
    final radius = size.width / 2 - 10.w;
    
    switch (pattern) {
      case NaturePattern.ripples:
        _drawRipples(canvas, center, radius);
        break;
      case NaturePattern.waves:
        _drawWaves(canvas, center, radius);
        break;
      case NaturePattern.flow:
        _drawFlow(canvas, center, radius);
        break;
      case NaturePattern.rain:
        _drawRain(canvas, center, radius);
        break;
    }
  }

  void _drawRipples(Canvas canvas, Offset center, double radius) {
    final colors = pattern.colors;
    final rippleCount = 5;
    
    for (int i = 0; i < rippleCount; i++) {
      final rippleRadius = (radius * 0.3) + (i * 20.w * waveAnimation);
      final opacity = intensity * (1.0 - i * 0.15) * 0.6;
      
      final ripplePaint = Paint()
        ..color = colors[0].withOpacity(opacity)
        ..style = PaintingStyle.stroke
        ..strokeWidth = 2.w;
      
      canvas.drawCircle(center, rippleRadius, ripplePaint);
    }
  }

  void _drawWaves(Canvas canvas, Offset center, double radius) {
    final colors = pattern.colors;
    final waveCount = 3;
    
    for (int i = 0; i < waveCount; i++) {
      final waveRadius = radius * 0.4 + (i * 30.w);
      final opacity = intensity * (1.0 - i * 0.2) * 0.7;
      
      final wavePaint = Paint()
        ..color = colors[i % colors.length].withOpacity(opacity)
        ..style = PaintingStyle.stroke
        ..strokeWidth = 3.w;
      
      // Simple wave circles that scale with animation
      canvas.drawCircle(center, waveRadius * waveAnimation, wavePaint);
    }
  }

  void _drawFlow(Canvas canvas, Offset center, double radius) {
    final colors = pattern.colors;
    final flowCount = 6;
    
    for (int i = 0; i < flowCount; i++) {
      final angle = (i * math.pi / 3) + (flowAnimation * math.pi * 2);
      final flowRadius = radius * 0.7;
      final flowX = center.dx + flowRadius * math.cos(angle) * waveAnimation;
      final flowY = center.dy + flowRadius * math.sin(angle) * waveAnimation;
      
      final flowPaint = Paint()
        ..color = colors[i % colors.length].withOpacity(intensity * 0.5);
      
      canvas.drawCircle(Offset(flowX, flowY), 8.r, flowPaint);
    }
  }

  void _drawRain(Canvas canvas, Offset center, double radius) {
    final colors = pattern.colors;
    final dropCount = 8;
    
    for (int i = 0; i < dropCount; i++) {
      final dropX = center.dx + (i - dropCount / 2) * 20.w;
      final dropY = center.dy + (math.sin(flowAnimation * math.pi * 2 + i) * 30.h);
      
      final dropPaint = Paint()
        ..color = colors[i % colors.length].withOpacity(intensity * 0.6);
      
      canvas.drawCircle(Offset(dropX, dropY), 6.r * waveAnimation, dropPaint);
    }
  }

  @override
  bool shouldRepaint(covariant CustomPainter oldDelegate) => true;
}