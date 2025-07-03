import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:manifestation_app/meditation_constants.dart';
import 'package:manifestation_app/meditation_models.dart';
import 'dart:math' as math;

class AnimatedCandle extends StatefulWidget {
  final CandleColor color;
  final double intensity;
  final double size;

  const AnimatedCandle({
    Key? key,
    required this.color,
    required this.intensity,
    required this.size,
  }) : super(key: key);

  @override
  _AnimatedCandleState createState() => _AnimatedCandleState();
}

class _AnimatedCandleState extends State<AnimatedCandle>
    with TickerProviderStateMixin {
  late AnimationController _flameController;
  late AnimationController _flickerController;
  late AnimationController _glowController;
  
  late Animation<double> _flameHeight;
  late Animation<double> _flameWidth;
  late Animation<double> _flickerOffset;
  late Animation<double> _glowIntensity;

  @override
  void initState() {
    super.initState();
    
    // Main flame animation
    _flameController = AnimationController(
      duration: AppConstants.candleFlicker,
      vsync: this,
    );
    
    // Subtle flicker animation
    _flickerController = AnimationController(
      duration: Duration(milliseconds: 300),
      vsync: this,
    );
    
    // Glow pulse animation
    _glowController = AnimationController(
      duration: AppConstants.candleGlow,
      vsync: this,
    );
    
    // Setup animations
    _flameHeight = Tween<double>(
      begin: 0.8,
      end: 1.2,
    ).animate(CurvedAnimation(
      parent: _flameController,
      curve: Curves.easeInOut,
    ));
    
    _flameWidth = Tween<double>(
      begin: 0.9,
      end: 1.1,
    ).animate(CurvedAnimation(
      parent: _flameController,
      curve: Curves.easeInOut,
    ));
    
    _flickerOffset = Tween<double>(
      begin: -3.0,
      end: 3.0,
    ).animate(CurvedAnimation(
      parent: _flickerController,
      curve: Curves.easeInOut,
    ));
    
    _glowIntensity = Tween<double>(
      begin: 0.3,
      end: 0.8,
    ).animate(CurvedAnimation(
      parent: _glowController,
      curve: Curves.easeInOut,
    ));
    
    // Start animations
    _startAnimations();
  }

  void _startAnimations() {
    if (widget.intensity > 0.3) {
      _flameController.repeat(reverse: true);
      _flickerController.repeat(reverse: true);
      _glowController.repeat(reverse: true);
    }
  }

  @override
  void didUpdateWidget(AnimatedCandle oldWidget) {
    super.didUpdateWidget(oldWidget);
    if (widget.intensity != oldWidget.intensity) {
      if (widget.intensity > 0.3) {
        _startAnimations();
      } else {
        _flameController.stop();
        _flickerController.stop();
        _glowController.stop();
      }
    }
  }

  @override
  void dispose() {
    _flameController.dispose();
    _flickerController.dispose();
    _glowController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return SizedBox(
      width: widget.size.w,
      height: (widget.size * 2.5).h, // Taller to accommodate flame
      child: AnimatedBuilder(
        animation: Listenable.merge([
          _flameController,
          _flickerController,
          _glowController,
        ]),
        builder: (context, child) {
          return CustomPaint(
            size: Size(widget.size.w, (widget.size * 2.5).h),
            painter: CandlePainter(
              color: widget.color,
              intensity: widget.intensity,
              flameHeight: _flameHeight.value,
              flameWidth: _flameWidth.value,
              flickerOffset: _flickerOffset.value,
              glowIntensity: _glowIntensity.value,
            ),
          );
        },
      ),
    );
  }
}

class CandlePainter extends CustomPainter {
  final CandleColor color;
  final double intensity;
  final double flameHeight;
  final double flameWidth;
  final double flickerOffset;
  final double glowIntensity;

  CandlePainter({
    required this.color,
    required this.intensity,
    required this.flameHeight,
    required this.flameWidth,
    required this.flickerOffset,
    required this.glowIntensity,
  });

  @override
  void paint(Canvas canvas, Size size) {
    final centerX = size.width / 2;
    final candleHeight = size.height * 0.55; // Space for flame
    final flameStartY = size.height - candleHeight - 20.h;
    
    // Draw glow effect first
    _drawGlow(canvas, centerX, flameStartY, size);
    
    // Draw candle body
    _drawCandleBody(canvas, centerX, candleHeight, size);
    
    // Draw wick
    _drawWick(canvas, centerX, flameStartY);
    
    // Draw flame on top
    _drawFlame(canvas, centerX, flameStartY);
  }

  void _drawGlow(Canvas canvas, double centerX, double flameY, Size size) {
    if (intensity < 0.3) return;
    
    // Synchronize glow with flame
    final adjustedCenterX = centerX + flickerOffset;
    final currentFlameHeight = 160.h * flameHeight * intensity;
    final glowCenterY = flameY - (currentFlameHeight * 0.4);
    
    // Main glow
    final glowPaint = Paint()
      ..color = color.primaryColor.withOpacity(0.25 * intensity * glowIntensity)
      ..maskFilter = MaskFilter.blur(BlurStyle.normal, (60 + (20 * currentFlameHeight / 160)).r);

    canvas.drawOval(
      Rect.fromCenter(
        center: Offset(adjustedCenterX, glowCenterY),
        width: (100.w + currentFlameHeight * 0.8) * intensity * flameWidth,
        height: (80.h + currentFlameHeight * 0.6) * intensity * flameHeight,
      ),
      glowPaint,
    );

    // Outer soft glow
    final softGlowPaint = Paint()
      ..color = color.primaryColor.withOpacity(0.12 * intensity * glowIntensity)
      ..maskFilter = MaskFilter.blur(BlurStyle.normal, (120 + (30 * currentFlameHeight / 160)).r);

    canvas.drawOval(
      Rect.fromCenter(
        center: Offset(adjustedCenterX, glowCenterY),
        width: (180.w + currentFlameHeight * 1.2) * intensity * flameWidth,
        height: (140.h + currentFlameHeight * 0.8) * intensity * flameHeight,
      ),
      softGlowPaint,
    );
  }

  void _drawCandleBody(Canvas canvas, double centerX, double candleHeight, Size size) {
    final candleWidth = size.width * 0.8; // Wide candle
    final candlePaint = Paint()
      ..shader = LinearGradient(
        begin: Alignment.centerLeft,
        end: Alignment.centerRight,
        colors: [
          Color(0xFFF0F0F0), // Light gray
          Color(0xFFFFE4B5), // Moccasin
          Color(0xFFF5F5DC), // Beige
          Color(0xFFFFE4B5), // Moccasin
          Color(0xFFF0F0F0), // Light gray
        ],
        stops: [0.0, 0.25, 0.5, 0.75, 1.0],
      ).createShader(Rect.fromLTWH(
        centerX - candleWidth / 2,
        size.height - candleHeight,
        candleWidth,
        candleHeight,
      ));

    // Main candle body
    canvas.drawRRect(
      RRect.fromRectAndRadius(
        Rect.fromLTWH(
          centerX - candleWidth / 2,
          size.height - candleHeight,
          candleWidth,
          candleHeight,
        ),
        Radius.circular(10.r),
      ),
      candlePaint,
    );

    // Add texture lines
    _drawCandleTexture(canvas, centerX, candleWidth, size.height - candleHeight, candleHeight);

    // Wax drips
    _drawWaxDrips(canvas, centerX, candleWidth, size.height - candleHeight);
  }

  void _drawCandleTexture(Canvas canvas, double centerX, double candleWidth, double candleTop, double candleHeight) {
    final texturePaint = Paint()
      ..color = Color(0xFFE0E0E0).withOpacity(0.3)
      ..strokeWidth = 1.w;

    // Vertical texture lines
    for (int i = 0; i < 6; i++) {
      final x = centerX - candleWidth * 0.35 + (i * candleWidth * 0.14);
      canvas.drawLine(
        Offset(x, candleTop + 20.h),
        Offset(x, candleTop + candleHeight - 20.h),
        texturePaint,
      );
    }
  }

  void _drawWaxDrips(Canvas canvas, double centerX, double candleWidth, double candleTop) {
    final dripPaint = Paint()
      ..color = Color(0xFFFFE4B5).withOpacity(0.9);

    // Left drip
    final leftDripPath = Path();
    leftDripPath.moveTo(centerX - candleWidth * 0.28, candleTop + 30.h);
    leftDripPath.quadraticBezierTo(
      centerX - candleWidth * 0.45, candleTop + 60.h,
      centerX - candleWidth * 0.35, candleTop + 100.h,
    );
    leftDripPath.quadraticBezierTo(
      centerX - candleWidth * 0.28, candleTop + 120.h,
      centerX - candleWidth * 0.35, candleTop + 140.h,
    );
    leftDripPath.lineTo(centerX - candleWidth * 0.25, candleTop + 145.h);
    leftDripPath.close();

    canvas.drawPath(leftDripPath, dripPaint);

    // Right drip
    final rightDripPath = Path();
    rightDripPath.moveTo(centerX + candleWidth * 0.25, candleTop + 50.h);
    rightDripPath.quadraticBezierTo(
      centerX + candleWidth * 0.4, candleTop + 80.h,
      centerX + candleWidth * 0.3, candleTop + 120.h,
    );
    rightDripPath.quadraticBezierTo(
      centerX + candleWidth * 0.25, candleTop + 140.h,
      centerX + candleWidth * 0.3, candleTop + 160.h,
    );
    rightDripPath.lineTo(centerX + candleWidth * 0.2, candleTop + 155.h);
    rightDripPath.close();

    canvas.drawPath(rightDripPath, dripPaint);

    // Center small drip
    final centerDripPath = Path();
    centerDripPath.moveTo(centerX + 12.w, candleTop + 25.h);
    centerDripPath.quadraticBezierTo(
      centerX + 18.w, candleTop + 40.h,
      centerX + 10.w, candleTop + 60.h,
    );
    centerDripPath.quadraticBezierTo(
      centerX + 6.w, candleTop + 70.h,
      centerX + 12.w, candleTop + 80.h,
    );
    centerDripPath.lineTo(centerX + 6.w, candleTop + 75.h);
    centerDripPath.close();

    canvas.drawPath(centerDripPath, dripPaint);
  }

  void _drawWick(Canvas canvas, double centerX, double flameY) {
    // Wick shadow
    final wickShadowPaint = Paint()
      ..color = Color(0xFF1A0F0A).withOpacity(0.8)
      ..strokeWidth = 6.w
      ..strokeCap = StrokeCap.round;

    canvas.drawLine(
      Offset(centerX + 1.w, flameY + 2.h),
      Offset(centerX + 1.w, flameY - 18.h),
      wickShadowPaint,
    );

    // Main wick body
    final wickPaint = Paint()
      ..color = Color(0xFF2F1B14)
      ..strokeWidth = 5.w
      ..strokeCap = StrokeCap.round;

    canvas.drawLine(
      Offset(centerX, flameY),
      Offset(centerX, flameY - 20.h),
      wickPaint,
    );

    // Upper wick part
    final upperWickPaint = Paint()
      ..color = Color(0xFF3A2418)
      ..strokeWidth = 3.w
      ..strokeCap = StrokeCap.round;

    canvas.drawLine(
      Offset(centerX, flameY - 12.h),
      Offset(centerX, flameY - 20.h),
      upperWickPaint,
    );

    // Burnt tip
    final burntTipPaint = Paint()
      ..color = Color(0xFF0D0502);

    canvas.drawCircle(
      Offset(centerX, flameY - 20.h),
      2.8.r,
      burntTipPaint,
    );

    // Burnt particles
    final particlePaint = Paint()
      ..color = Color(0xFF1A0F0A);

    canvas.drawCircle(Offset(centerX - 1.5.w, flameY - 19.h), 0.8.r, particlePaint);
    canvas.drawCircle(Offset(centerX + 1.2.w, flameY - 18.5.h), 0.6.r, particlePaint);
    canvas.drawCircle(Offset(centerX - 0.8.w, flameY - 21.h), 0.5.r, particlePaint);

    // Tip glow where flame meets wick
    if (intensity > 0.5) {
      final tipGlowPaint = Paint()
        ..color = color.flameGradient[0].withOpacity(0.6)
        ..maskFilter = MaskFilter.blur(BlurStyle.normal, 3.r);

      canvas.drawCircle(
        Offset(centerX, flameY - 20.h),
        (4 * intensity).r,
        tipGlowPaint,
      );
    }

    // Wick fibers for texture
    final fiberPaint = Paint()
      ..color = Color(0xFF4A3226).withOpacity(0.7)
      ..strokeWidth = 0.5.w;

    for (int i = 0; i < 3; i++) {
      final offsetX = (i - 1) * 0.8.w;
      canvas.drawLine(
        Offset(centerX + offsetX, flameY - 5.h),
        Offset(centerX + offsetX, flameY - 16.h),
        fiberPaint,
      );
    }
  }

  void _drawFlame(Canvas canvas, double centerX, double flameY) {
    if (intensity < 0.1) return;

    final adjustedCenterX = centerX + flickerOffset;
    final flameBaseWidth = 60.w * flameWidth * intensity;
    final flameTopHeight = 160.h * flameHeight * intensity;

    // Create flame path
    final flamePath = Path();
    
    // Elliptical base
    flamePath.addOval(Rect.fromCenter(
      center: Offset(adjustedCenterX, flameY - 15.h),
      width: flameBaseWidth,
      height: 30.h,
    ));
    
    // Main flame body
    flamePath.moveTo(adjustedCenterX - flameBaseWidth / 2, flameY - 15.h);
    flamePath.quadraticBezierTo(
      adjustedCenterX - flameBaseWidth / 2.2, flameY - flameTopHeight * 0.6,
      adjustedCenterX - 8.w, flameY - flameTopHeight * 0.9,
    );
    flamePath.quadraticBezierTo(
      adjustedCenterX, flameY - flameTopHeight,
      adjustedCenterX + 8.w, flameY - flameTopHeight * 0.9,
    );
    flamePath.quadraticBezierTo(
      adjustedCenterX + flameBaseWidth / 2.2, flameY - flameTopHeight * 0.6,
      adjustedCenterX + flameBaseWidth / 2, flameY - 15.h,
    );
    flamePath.close();

    // Flame gradient
    final flameGradient = RadialGradient(
      center: Alignment.bottomCenter,
      colors: color.flameGradient,
      stops: [0.0, 0.6, 1.0],
    );

    final flamePaint = Paint()
      ..shader = flameGradient.createShader(Rect.fromLTWH(
        adjustedCenterX - flameBaseWidth,
        flameY - flameTopHeight,
        flameBaseWidth * 2,
        flameTopHeight,
      ));

    canvas.drawPath(flamePath, flamePaint);

    // Inner flame highlight
    final innerFlamePath = Path();
    innerFlamePath.moveTo(adjustedCenterX - flameBaseWidth / 2.3, flameY - 10.h);
    innerFlamePath.quadraticBezierTo(
      adjustedCenterX - flameBaseWidth / 3.2, flameY - flameTopHeight * 0.4,
      adjustedCenterX - 5.w, flameY - flameTopHeight * 0.75,
    );
    innerFlamePath.quadraticBezierTo(
      adjustedCenterX, flameY - flameTopHeight * 0.85,
      adjustedCenterX + 5.w, flameY - flameTopHeight * 0.75,
    );
    innerFlamePath.quadraticBezierTo(
      adjustedCenterX + flameBaseWidth / 3.2, flameY - flameTopHeight * 0.4,
      adjustedCenterX + flameBaseWidth / 2.3, flameY - 10.h,
    );
    innerFlamePath.close();

    final innerFlamePaint = Paint()
      ..color = Colors.white.withOpacity(0.8 * intensity);

    canvas.drawPath(innerFlamePath, innerFlamePaint);

    // Secondary inner flame for depth
    final secondaryInnerPath = Path();
    secondaryInnerPath.moveTo(adjustedCenterX - flameBaseWidth / 3.5, flameY - 8.h);
    secondaryInnerPath.quadraticBezierTo(
      adjustedCenterX - flameBaseWidth / 5.5, flameY - flameTopHeight * 0.3,
      adjustedCenterX - 3.w, flameY - flameTopHeight * 0.6,
    );
    secondaryInnerPath.quadraticBezierTo(
      adjustedCenterX, flameY - flameTopHeight * 0.7,
      adjustedCenterX + 3.w, flameY - flameTopHeight * 0.6,
    );
    secondaryInnerPath.quadraticBezierTo(
      adjustedCenterX + flameBaseWidth / 5.5, flameY - flameTopHeight * 0.3,
      adjustedCenterX + flameBaseWidth / 3.5, flameY - 8.h,
    );
    secondaryInnerPath.close();

    final secondaryInnerPaint = Paint()
      ..color = Colors.white.withOpacity(0.6 * intensity);

    canvas.drawPath(secondaryInnerPath, secondaryInnerPaint);
  }

  @override
  bool shouldRepaint(covariant CustomPainter oldDelegate) => true;
}