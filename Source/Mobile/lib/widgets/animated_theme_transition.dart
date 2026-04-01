import 'dart:math' as math;
import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';

class AnimatedThemeTransition extends StatefulWidget {
  final Widget child;
  final bool isDarkMode;
  final Duration duration;
  final VoidCallback? onAnimationComplete;

  const AnimatedThemeTransition({
    Key? key,
    required this.child,
    required this.isDarkMode,
    this.duration = const Duration(milliseconds: 1500),
    this.onAnimationComplete,
  }) : super(key: key);

  @override
  State<AnimatedThemeTransition> createState() => _AnimatedThemeTransitionState();
}

class _AnimatedThemeTransitionState extends State<AnimatedThemeTransition>
    with TickerProviderStateMixin {
  late AnimationController _controller;
  late AnimationController _bubbleController;
  late Animation<double> _animation;
  late Animation<double> _bubbleAnimation;
  late Animation<double> _waveAnimation1;
  late Animation<double> _waveAnimation2;
  late Animation<double> _waveAnimation3;
  
  bool _isAnimating = false;
  bool _previousDarkMode = false;
  
  final List<BubbleData> _bubbles = [];
  final int _numberOfBubbles = 15;

  @override
  void initState() {
    super.initState();
    _previousDarkMode = widget.isDarkMode;
    _setupAnimations();
    _generateBubbles();
  }

  void _setupAnimations() {
    _controller = AnimationController(
      duration: widget.duration,
      vsync: this,
    );

    _bubbleController = AnimationController(
      duration: Duration(milliseconds: widget.duration.inMilliseconds + 500),
      vsync: this,
    );

    _animation = CurvedAnimation(
      parent: _controller,
      curve: Curves.easeInOutCubic,
    );

    _bubbleAnimation = CurvedAnimation(
      parent: _bubbleController,
      curve: Curves.easeOutQuart,
    );

    // Multiple wave animations for ocean effect
    _waveAnimation1 = Tween<double>(
      begin: 0.0,
      end: 2 * math.pi,
    ).animate(CurvedAnimation(
      parent: _controller,
      curve: Curves.easeInOut,
    ));

    _waveAnimation2 = Tween<double>(
      begin: 0.0,
      end: 3 * math.pi,
    ).animate(CurvedAnimation(
      parent: _controller,
      curve: Curves.easeInOutSine,
    ));

    _waveAnimation3 = Tween<double>(
      begin: 0.0,
      end: 1.5 * math.pi,
    ).animate(CurvedAnimation(
      parent: _controller,
      curve: Curves.easeInOutQuad,
    ));

    _controller.addStatusListener((status) {
      if (status == AnimationStatus.completed) {
        setState(() {
          _isAnimating = false;
        });
        widget.onAnimationComplete?.call();
      }
    });
  }

  void _generateBubbles() {
    _bubbles.clear();
    final random = math.Random();
    
    for (int i = 0; i < _numberOfBubbles; i++) {
      _bubbles.add(BubbleData(
        x: random.nextDouble(),
        y: random.nextDouble(),
        size: random.nextDouble() * 20 + 10,
        speed: random.nextDouble() * 0.5 + 0.3,
        opacity: random.nextDouble() * 0.6 + 0.2,
        phase: random.nextDouble() * 2 * math.pi,
      ));
    }
  }

  @override
  void didUpdateWidget(AnimatedThemeTransition oldWidget) {
    super.didUpdateWidget(oldWidget);
    
    if (widget.isDarkMode != _previousDarkMode && !_isAnimating) {
      _previousDarkMode = widget.isDarkMode;
      _startTransition();
    }
  }

  void _startTransition() {
    setState(() {
      _isAnimating = true;
    });
    
    _generateBubbles();
    _controller.forward(from: 0.0);
    _bubbleController.forward(from: 0.0);
  }

  @override
  void dispose() {
    try {
    _controller.dispose();
    } catch (e) {
      debugPrint('❌ Error disposing controller: $e');
    }

    _bubbleController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Stack(
      alignment: Alignment.topLeft,
      children: [
        widget.child,
        if (_isAnimating)
          AnimatedBuilder(
            animation: _animation,
            builder: (context, child) {
              return CustomPaint(
                painter: OceanThemeTransitionPainter(
                  progress: _animation.value,
                  bubbleProgress: _bubbleAnimation.value,
                  wave1: _waveAnimation1.value,
                  wave2: _waveAnimation2.value,
                  wave3: _waveAnimation3.value,
                  bubbles: _bubbles,
                  isDarkMode: widget.isDarkMode,
                ),
                size: Size.infinite,
              );
            },
          ),
      ],
    );
  }
}

class BubbleData {
  final double x;
  final double y;
  final double size;
  final double speed;
  final double opacity;
  final double phase;

  BubbleData({
    required this.x,
    required this.y,
    required this.size,
    required this.speed,
    required this.opacity,
    required this.phase,
  });
}

class OceanThemeTransitionPainter extends CustomPainter {
  final double progress;
  final double bubbleProgress;
  final double wave1;
  final double wave2;
  final double wave3;
  final List<BubbleData> bubbles;
  final bool isDarkMode;

  OceanThemeTransitionPainter({
    required this.progress,
    required this.bubbleProgress,
    required this.wave1,
    required this.wave2,
    required this.wave3,
    required this.bubbles,
    required this.isDarkMode,
  });

  @override
  void paint(Canvas canvas, Size size) {
    if (progress == 0.0) return;

    // Theme colors
    final newColor = isDarkMode ? const Color(0xFF1A1A2E) : const Color(0xFFFFFFFF);
    
    // Calculate the diagonal line position
    double lineX, lineY;
    
    if (isDarkMode) {
      // Dark mode: line goes from top-left to bottom-right
      lineX = (size.width + size.height) * progress - size.height;
      lineY = (size.width + size.height) * progress - size.width;
    } else {
      // Light mode: line goes from bottom-right to top-left  
      lineX = size.width - (size.width + size.height) * progress + size.height;
      lineY = size.height - (size.width + size.height) * progress + size.width;
    }
    
    // Create mask for the area that should be changed
    final path = Path();
    
    if (isDarkMode) {
      // Everything above/left of the diagonal line
      path.moveTo(0, 0);
      path.lineTo(math.max(0, lineX), 0);
      path.lineTo(0, math.max(0, lineY));
      path.close();
      
      if (lineX > 0 && lineY > 0) {
        path.moveTo(math.min(size.width, lineX), 0);
        path.lineTo(size.width, 0);
        path.lineTo(size.width, math.min(size.height, lineY));
        path.close();
      }
      
      if (lineX > size.width) {
        path.moveTo(0, math.max(0, lineY));
        path.lineTo(0, size.height);
        path.lineTo(math.min(size.width, lineX - size.width), size.height);
        path.close();
      }
    } else {
      // Everything below/right of the diagonal line
      path.moveTo(size.width, size.height);
      path.lineTo(math.min(size.width, lineX), size.height);
      path.lineTo(size.width, math.min(size.height, lineY));
      path.close();
      
      if (lineX < size.width && lineY < size.height) {
        path.moveTo(math.max(0, lineX), size.height);
        path.lineTo(0, size.height);
        path.lineTo(0, math.max(0, lineY));
        path.close();
      }
      
      if (lineX < 0) {
        path.moveTo(size.width, math.min(size.height, lineY));
        path.lineTo(size.width, 0);
        path.lineTo(math.max(0, size.width + lineX), 0);
        path.close();
      }
    }
    
    // Draw the new color only in the masked area
    canvas.save();
    canvas.clipPath(path);
    canvas.drawRect(Rect.fromLTWH(0, 0, size.width, size.height), Paint()..color = newColor);
    canvas.restore();
  }

  @override
  bool shouldRepaint(OceanThemeTransitionPainter oldDelegate) {
    return oldDelegate.progress != progress ||
           oldDelegate.bubbleProgress != bubbleProgress ||
           oldDelegate.wave1 != wave1 ||
           oldDelegate.wave2 != wave2 ||
           oldDelegate.wave3 != wave3;
  }
}