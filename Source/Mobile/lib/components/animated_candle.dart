import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:mistel/constants/meditation_constants.dart';
import 'dart:math' as math;
import 'package:mistel/models/meditation_models.dart';

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

  // Determine if this is a preview based on size
  bool get isPreview => widget.size < 200;

  @override
  Widget build(BuildContext context) {
    // Adjust proportions based on preview vs active session
    final aspectRatio = isPreview ? 1.8 : 2.5;
    final containerHeight = widget.size * aspectRatio;
    
    return SizedBox(
      width: widget.size.w,
      height: containerHeight.h,
      child: AnimatedBuilder(
        animation: Listenable.merge([
          _flameController,
          _flickerController,
          _glowController,
        ]),
        builder: (context, child) {
          return CustomPaint(
            size: Size(widget.size.w, containerHeight.h),
            painter: CandlePainter(
              color: widget.color,
              intensity: widget.intensity,
              flameHeight: _flameHeight.value,
              flameWidth: _flameWidth.value,
              flickerOffset: _flickerOffset.value,
              glowIntensity: _glowIntensity.value,
              isPreview: isPreview,
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
  final bool isPreview;

  CandlePainter({
    required this.color,
    required this.intensity,
    required this.flameHeight,
    required this.flameWidth,
    required this.flickerOffset,
    required this.glowIntensity,
    required this.isPreview,
  });

  @override
  void paint(Canvas canvas, Size size) {
    final centerX = size.width / 2;
    
    // Adjust proportions for preview vs active session
    final candleHeightRatio = isPreview ? 0.65 : 0.55;
    final flameHeightRatio = isPreview ? 0.3 : 0.4;
    final candleWidthRatio = isPreview ? 0.6 : 0.8;
    
    final candleHeight = size.height * candleHeightRatio;
    final flameStartY = size.height - candleHeight - (isPreview ? 5.h : 20.h);
    
    // Draw glow effect first
    _drawGlow(canvas, centerX, flameStartY, size);
    
    // Draw candle body
    _drawCandleBody(canvas, centerX, candleHeight, size, candleWidthRatio);
    
    // Draw wick
    _drawWick(canvas, centerX, flameStartY);
    
    // Draw flame on top
    _drawFlame(canvas, centerX, flameStartY, size, flameHeightRatio);
  }

  void _drawGlow(Canvas canvas, double centerX, double flameY, Size size) {
    if (intensity < 0.3) return;
    
    // Scale glow for preview
    final glowScale = isPreview ? 0.5 : 1.0;
    final adjustedCenterX = centerX + (flickerOffset * glowScale);
    
    final baseFlameHeight = isPreview ? 60.h : 160.h;
    final currentFlameHeight = baseFlameHeight * flameHeight * intensity;
    final glowCenterY = flameY - (currentFlameHeight * 0.4);
    
    // Main glow
    final glowSize = isPreview ? 40.r : 60.r;
    final glowPaint = Paint()
      ..color = color.primaryColor.withOpacity(0.25 * intensity * glowIntensity)
      ..maskFilter = MaskFilter.blur(BlurStyle.normal, (glowSize + (10 * currentFlameHeight / baseFlameHeight)).r);

    canvas.drawOval(
      Rect.fromCenter(
        center: Offset(adjustedCenterX, glowCenterY),
        width: ((isPreview ? 50.w : 100.w) + currentFlameHeight * 0.8) * intensity * flameWidth,
        height: ((isPreview ? 40.h : 80.h) + currentFlameHeight * 0.6) * intensity * flameHeight,
      ),
      glowPaint,
    );

    // Outer soft glow
    final outerGlowSize = isPreview ? 60.r : 120.r;
    final softGlowPaint = Paint()
      ..color = color.primaryColor.withOpacity(0.12 * intensity * glowIntensity)
      ..maskFilter = MaskFilter.blur(BlurStyle.normal, (outerGlowSize + (15 * currentFlameHeight / baseFlameHeight)).r);

    canvas.drawOval(
      Rect.fromCenter(
        center: Offset(adjustedCenterX, glowCenterY),
        width: ((isPreview ? 90.w : 180.w) + currentFlameHeight * 1.2) * intensity * flameWidth,
        height: ((isPreview ? 70.h : 140.h) + currentFlameHeight * 0.8) * intensity * flameHeight,
      ),
      softGlowPaint,
    );
  }

  void _drawCandleBody(Canvas canvas, double centerX, double candleHeight, Size size, double candleWidthRatio) {
    final candleWidth = size.width * candleWidthRatio;
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
    final borderRadius = isPreview ? 5.r : 10.r;
    canvas.drawRRect(
      RRect.fromRectAndRadius(
        Rect.fromLTWH(
          centerX - candleWidth / 2,
          size.height - candleHeight,
          candleWidth,
          candleHeight,
        ),
        Radius.circular(borderRadius),
      ),
      candlePaint,
    );

    // Add texture lines
    if (!isPreview) {
      _drawCandleTexture(canvas, centerX, candleWidth, size.height - candleHeight, candleHeight);
      _drawWaxDrips(canvas, centerX, candleWidth, size.height - candleHeight);
    }
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
    // Scale wick for preview
    final wickScale = isPreview ? 0.6 : 1.0;
    final wickWidth = 5.w * wickScale;
    final wickHeight = 20.h * wickScale;
    
    // Wick shadow
    final wickShadowPaint = Paint()
      ..color = Color(0xFF1A0F0A).withOpacity(0.8)
      ..strokeWidth = (6.w * wickScale)
      ..strokeCap = StrokeCap.round;

    canvas.drawLine(
      Offset(centerX + 1.w, flameY + 2.h),
      Offset(centerX + 1.w, flameY - (18.h * wickScale)),
      wickShadowPaint,
    );

    // Main wick body
    final wickPaint = Paint()
      ..color = Color(0xFF2F1B14)
      ..strokeWidth = wickWidth
      ..strokeCap = StrokeCap.round;

    canvas.drawLine(
      Offset(centerX, flameY),
      Offset(centerX, flameY - wickHeight),
      wickPaint,
    );

    // Upper wick part
    final upperWickPaint = Paint()
      ..color = Color(0xFF3A2418)
      ..strokeWidth = (3.w * wickScale)
      ..strokeCap = StrokeCap.round;

    canvas.drawLine(
      Offset(centerX, flameY - (12.h * wickScale)),
      Offset(centerX, flameY - wickHeight),
      upperWickPaint,
    );

    // Burnt tip
    final burntTipPaint = Paint()
      ..color = Color(0xFF0D0502);

    canvas.drawCircle(
      Offset(centerX, flameY - wickHeight),
      (2.8.r * wickScale),
      burntTipPaint,
    );

    // Only draw details if not preview
    if (!isPreview) {
      // Burnt particles
      final particlePaint = Paint()
        ..color = Color(0xFF1A0F0A);

      canvas.drawCircle(Offset(centerX - 1.5.w, flameY - 19.h), 0.8.r, particlePaint);
      canvas.drawCircle(Offset(centerX + 1.2.w, flameY - 18.5.h), 0.6.r, particlePaint);
      canvas.drawCircle(Offset(centerX - 0.8.w, flameY - 21.h), 0.5.r, particlePaint);

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

    // Tip glow where flame meets wick
    if (intensity > 0.5) {
      final tipGlowPaint = Paint()
        ..color = color.flameGradient[0].withOpacity(0.6)
        ..maskFilter = MaskFilter.blur(BlurStyle.normal, (3.r * wickScale));

      canvas.drawCircle(
        Offset(centerX, flameY - wickHeight),
        ((4 * intensity) * wickScale).r,
        tipGlowPaint,
      );
    }
  }

  void _drawFlame(Canvas canvas, double centerX, double flameY, Size size, double flameHeightRatio) {
    if (intensity < 0.1) return;

    // Scale flame proportions for preview
    final flameScale = isPreview ? 0.5 : 1.0;
    final adjustedCenterX = centerX + (flickerOffset * flameScale);
    
    final baseFlameWidth = isPreview ? 30.w : 60.w;
    final baseFlameHeight = isPreview ? 60.h : 160.h;
    
    final flameBaseWidth = baseFlameWidth * flameWidth * intensity;
    final flameTopHeight = baseFlameHeight * flameHeight * intensity;

    // Create flame path
    final flamePath = Path();
    
    // Elliptical base
    final baseHeight = isPreview ? 15.h : 30.h;
    flamePath.addOval(Rect.fromCenter(
      center: Offset(adjustedCenterX, flameY - (15.h * flameScale)),
      width: flameBaseWidth,
      height: baseHeight,
    ));
    
    // Main flame body
    flamePath.moveTo(adjustedCenterX - flameBaseWidth / 2, flameY - (15.h * flameScale));
    flamePath.quadraticBezierTo(
      adjustedCenterX - flameBaseWidth / 2.2, flameY - flameTopHeight * 0.6,
      adjustedCenterX - (8.w * flameScale), flameY - flameTopHeight * 0.9,
    );
    flamePath.quadraticBezierTo(
      adjustedCenterX, flameY - flameTopHeight,
      adjustedCenterX + (8.w * flameScale), flameY - flameTopHeight * 0.9,
    );
    flamePath.quadraticBezierTo(
      adjustedCenterX + flameBaseWidth / 2.2, flameY - flameTopHeight * 0.6,
      adjustedCenterX + flameBaseWidth / 2, flameY - (15.h * flameScale),
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
    innerFlamePath.moveTo(adjustedCenterX - flameBaseWidth / 2.3, flameY - (10.h * flameScale));
    innerFlamePath.quadraticBezierTo(
      adjustedCenterX - flameBaseWidth / 3.2, flameY - flameTopHeight * 0.4,
      adjustedCenterX - (5.w * flameScale), flameY - flameTopHeight * 0.75,
    );
    innerFlamePath.quadraticBezierTo(
      adjustedCenterX, flameY - flameTopHeight * 0.85,
      adjustedCenterX + (5.w * flameScale), flameY - flameTopHeight * 0.75,
    );
    innerFlamePath.quadraticBezierTo(
      adjustedCenterX + flameBaseWidth / 3.2, flameY - flameTopHeight * 0.4,
      adjustedCenterX + flameBaseWidth / 2.3, flameY - (10.h * flameScale),
    );
    innerFlamePath.close();

    final innerFlamePaint = Paint()
      ..color = Colors.white.withOpacity(0.8 * intensity);

    canvas.drawPath(innerFlamePath, innerFlamePaint);

    // Secondary inner flame for depth
    final secondaryInnerPath = Path();
    secondaryInnerPath.moveTo(adjustedCenterX - flameBaseWidth / 3.5, flameY - (8.h * flameScale));
    secondaryInnerPath.quadraticBezierTo(
      adjustedCenterX - flameBaseWidth / 5.5, flameY - flameTopHeight * 0.3,
      adjustedCenterX - (3.w * flameScale), flameY - flameTopHeight * 0.6,
    );
    secondaryInnerPath.quadraticBezierTo(
      adjustedCenterX, flameY - flameTopHeight * 0.7,
      adjustedCenterX + (3.w * flameScale), flameY - flameTopHeight * 0.6,
    );
    secondaryInnerPath.quadraticBezierTo(
      adjustedCenterX + flameBaseWidth / 5.5, flameY - flameTopHeight * 0.3,
      adjustedCenterX + flameBaseWidth / 3.5, flameY - (8.h * flameScale),
    );
    secondaryInnerPath.close();

    final secondaryInnerPaint = Paint()
      ..color = Colors.white.withOpacity(0.6 * intensity);

    canvas.drawPath(secondaryInnerPath, secondaryInnerPaint);
  }

  @override
  bool shouldRepaint(covariant CustomPainter oldDelegate) => true;
}