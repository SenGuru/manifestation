import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:fun/constants/meditation_constants.dart';
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
  late AnimationController _shimmerController;
  
  late Animation<double> _waveAnimation;
  late Animation<double> _flowAnimation;
  late Animation<double> _shimmerAnimation;

  @override
  void initState() {
    super.initState();
    
    // Wave animation
    _waveController = AnimationController(
      duration: AppConstants.natureWaves,
      vsync: this,
    );
    
    // Flow animation
    _flowController = AnimationController(
      duration: Duration(milliseconds: 4000),
      vsync: this,
    );
    
    // Shimmer animation
    _shimmerController = AnimationController(
      duration: Duration(milliseconds: 3500),
      vsync: this,
    );
    
    _waveAnimation = Tween<double>(
      begin: 0.0,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _waveController,
      curve: Curves.easeInOut,
    ));
    
    _flowAnimation = Tween<double>(
      begin: 0.0,
      end: 2.0 * math.pi,
    ).animate(CurvedAnimation(
      parent: _flowController,
      curve: Curves.linear,
    ));
    
    _shimmerAnimation = Tween<double>(
      begin: 0.0,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _shimmerController,
      curve: Curves.easeInOut,
    ));
    
    _startAnimations();
  }

  void _startAnimations() {
    if (widget.intensity > 0.3) {
      _waveController.repeat();
      _flowController.repeat();
      _shimmerController.repeat(reverse: true);
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
        _shimmerController.stop();
      }
    }
  }

  @override
  void dispose() {
    _waveController.dispose();
    _flowController.dispose();
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
          _waveController,
          _flowController,
          _shimmerController,
        ]),
        builder: (context, child) {
          return CustomPaint(
            size: Size(widget.size.w, widget.size.h),
            painter: NaturePainter(
              pattern: widget.pattern,
              intensity: widget.intensity,
              wavePhase: _waveAnimation.value,
              flowPhase: _flowAnimation.value,
              shimmerPhase: _shimmerAnimation.value,
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
  final double wavePhase;
  final double flowPhase;
  final double shimmerPhase;

  NaturePainter({
    required this.pattern,
    required this.intensity,
    required this.wavePhase,
    required this.flowPhase,
    required this.shimmerPhase,
  });

  @override
  void paint(Canvas canvas, Size size) {
    final center = Offset(size.width / 2, size.height / 2);
    final maxRadius = size.width / 2;
    
    switch (pattern) {
      case NaturePattern.ripples:
        _drawRipples(canvas, center, maxRadius, size);
        break;
      case NaturePattern.waves:
        _drawWaves(canvas, center, maxRadius, size);
        break;
      case NaturePattern.flow:
        _drawFlow(canvas, center, maxRadius, size);
        break;
      case NaturePattern.rain:
        _drawRain(canvas, center, maxRadius, size);
        break;
    }
    
    // Add shimmer effect
    _drawShimmer(canvas, center, maxRadius);
  }

  void _drawRipples(Canvas canvas, Offset center, double maxRadius, Size size) {
    final colors = pattern.colors;
    
    // Draw multiple concentric ripples
    for (int i = 0; i < 8; i++) {
      final progress = (wavePhase + i * 0.15) % 1.0;
      final currentRadius = maxRadius * progress * (0.2 + i * 0.1);
      final opacity = (1.0 - progress) * (0.8 - i * 0.08) * intensity;
      
      if (opacity > 0.05) {
        final ripplePaint = Paint()
          ..color = colors[0].withOpacity(opacity)
          ..style = PaintingStyle.stroke
          ..strokeWidth = (3.0 - i * 0.2).w;
        
        // Add wave distortion
        final distortion = math.sin(progress * 4 * math.pi) * 2.w;
        canvas.drawCircle(
          center + Offset(distortion, distortion * 0.5),
          currentRadius,
          ripplePaint,
        );
      }
    }
    
    // Central glow
    if (intensity > 0.5) {
      final centralGlow = Paint()
        ..color = colors[1].withOpacity(0.4 * intensity)
        ..maskFilter = MaskFilter.blur(BlurStyle.normal, 20.r);
      
      canvas.drawCircle(center, maxRadius * 0.3, centralGlow);
    }
  }

  void _drawWaves(Canvas canvas, Offset center, double maxRadius, Size size) {
    final colors = pattern.colors;
    
    // Draw flowing waves
    for (int layer = 0; layer < 4; layer++) {
      final wavePath = Path();
      final amplitude = (20 + layer * 10).h * intensity;
      final frequency = 0.02 + layer * 0.005;
      final phase = flowPhase + layer * math.pi / 4;
      final opacity = (0.7 - layer * 0.15) * intensity;
      
      // Create wave path
      wavePath.moveTo(0, center.dy + amplitude * math.sin(phase));
      
      for (double x = 0; x <= size.width; x += 2.w) {
        final y = center.dy + amplitude * math.sin(frequency * x + phase);
        wavePath.lineTo(x, y);
      }
      
      final wavePaint = Paint()
        ..color = colors[layer % 2].withOpacity(opacity)
        ..style = PaintingStyle.stroke
        ..strokeWidth = (4.0 - layer * 0.5).w
        ..strokeCap = StrokeCap.round;
      
      canvas.drawPath(wavePath, wavePaint);
    }
  }

  void _drawFlow(Canvas canvas, Offset center, double maxRadius, Size size) {
    final colors = pattern.colors;
    
    // Draw flowing spiral
    for (int i = 0; i < 3; i++) {
      final spiralPath = Path();
      final spiralRadius = maxRadius * 0.8;
      final turns = 3.0;
      final points = 100;
      final phaseOffset = flowPhase + i * 2 * math.pi / 3;
      
      for (int j = 0; j <= points; j++) {
        final t = j / points;
        final angle = t * turns * 2 * math.pi + phaseOffset;
        final radius = spiralRadius * t * (1 + 0.2 * math.sin(t * 8 * math.pi + flowPhase));
        
        final x = center.dx + radius * math.cos(angle);
        final y = center.dy + radius * math.sin(angle);
        
        if (j == 0) {
          spiralPath.moveTo(x, y);
        } else {
          spiralPath.lineTo(x, y);
        }
      }
      
      final spiralPaint = Paint()
        ..color = colors[i % 2].withOpacity((0.8 - i * 0.2) * intensity)
        ..style = PaintingStyle.stroke
        ..strokeWidth = (6.0 - i * 1.5).w
        ..strokeCap = StrokeCap.round;
      
      canvas.drawPath(spiralPath, spiralPaint);
    }
  }

  void _drawRain(Canvas canvas, Offset center, double maxRadius, Size size) {
    final colors = pattern.colors;
    final random = math.Random(42); // Fixed seed for consistent pattern
    
    // Draw rain drops
    for (int i = 0; i < 20; i++) {
      final x = random.nextDouble() * size.width;
      final baseY = random.nextDouble() * size.height;
      final y = (baseY + wavePhase * size.height * 2) % (size.height + 100.h) - 50.h;
      final speed = 0.5 + random.nextDouble() * 0.5;
      final length = (10 + random.nextDouble() * 20).h;
      
      final dropPaint = Paint()
        ..color = colors[i % 2].withOpacity(0.6 * intensity * speed)
        ..strokeWidth = (1 + random.nextDouble() * 2).w
        ..strokeCap = StrokeCap.round;
      
      canvas.drawLine(
        Offset(x, y),
        Offset(x, y + length),
        dropPaint,
      );
      
      // Rain splash effect at bottom
      if (y > size.height - 50.h && y < size.height) {
        final splashPaint = Paint()
          ..color = colors[0].withOpacity(0.3 * intensity)
          ..style = PaintingStyle.stroke
          ..strokeWidth = 1.w;
        
        canvas.drawCircle(
          Offset(x, size.height),
          (y - (size.height - 50.h)) * 0.5,
          splashPaint,
        );
      }
    }
  }

  void _drawShimmer(Canvas canvas, Offset center, double maxRadius) {
    if (intensity < 0.4) return;
    
    final shimmerPaint = Paint()
      ..shader = LinearGradient(
        begin: Alignment.topLeft,
        end: Alignment.bottomRight,
        colors: [
          Colors.transparent,
          Colors.white.withOpacity(0.2 * intensity * shimmerPhase),
          Colors.transparent,
        ],
        stops: [0.0, 0.5, 1.0],
      ).createShader(Rect.fromCircle(center: center, radius: maxRadius));
    
    canvas.drawCircle(center, maxRadius, shimmerPaint);
  }

  @override
  bool shouldRepaint(covariant CustomPainter oldDelegate) => true;
}