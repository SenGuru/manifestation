import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../constants/app_theme.dart';
import 'dart:math' as math;

class MeditationTimer extends StatefulWidget {
  final int remainingSeconds;
  final int totalSeconds;
  final bool isActive;
  final Color? accentColor;
  final VoidCallback? onTap;

  const MeditationTimer({
    Key? key,
    required this.remainingSeconds,
    required this.totalSeconds,
    this.isActive = false,
    this.accentColor,
    this.onTap,
  }) : super(key: key);

  @override
  _MeditationTimerState createState() => _MeditationTimerState();
}

class _MeditationTimerState extends State<MeditationTimer>
    with TickerProviderStateMixin {
  late AnimationController _pulseController;
  late Animation<double> _pulseAnimation;

  @override
  void initState() {
    super.initState();
    
    _pulseController = AnimationController(
      duration: Duration(milliseconds: 2000),
      vsync: this,
    );
    
    _pulseAnimation = Tween<double>(
      begin: 0.95,
      end: 1.05,
    ).animate(CurvedAnimation(
      parent: _pulseController,
      curve: Curves.easeInOut,
    ));
    
    if (widget.isActive) {
      _pulseController.repeat(reverse: true);
    }
  }

  @override
  void didUpdateWidget(MeditationTimer oldWidget) {
    super.didUpdateWidget(oldWidget);
    if (widget.isActive != oldWidget.isActive) {
      if (widget.isActive) {
        _pulseController.repeat(reverse: true);
      } else {
        _pulseController.stop();
      }
    }
  }

  @override
  void dispose() {
    _pulseController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: widget.onTap,
      child: AnimatedBuilder(
        animation: _pulseAnimation,
        builder: (context, child) {
          return Transform.scale(
            scale: widget.isActive ? _pulseAnimation.value : 1.0,
            child: Container(
              padding: EdgeInsets.symmetric(horizontal: 16.w, vertical: 8.h),
              decoration: BoxDecoration(
                color: AppTheme.black.withOpacity(0.3),
                borderRadius: BorderRadius.circular(20.r),
                border: Border.all(
                  color: (widget.accentColor ?? AppTheme.accentLavender).withOpacity(0.3),
                  width: 1.w,
                ),
                boxShadow: widget.isActive
                    ? [
                        BoxShadow(
                          color: (widget.accentColor ?? AppTheme.accentLavender).withOpacity(0.2),
                          blurRadius: 8.r,
                          offset: Offset(0, 2.h),
                        ),
                      ]
                    : [],
              ),
              child: Row(
                mainAxisSize: MainAxisSize.min,
                children: [
                  // Progress indicator
                  _buildProgressIndicator(),
                  
                  SizedBox(width: 8.w),
                  
                  // Timer text
                  _buildTimerText(),
                ],
              ),
            ),
          );
        },
      ),
    );
  }

  Widget _buildProgressIndicator() {
    final progress = widget.totalSeconds > 0
        ? (widget.totalSeconds - widget.remainingSeconds) / widget.totalSeconds
        : 0.0;
    
    return Container(
      width: 20.w,
      height: 20.h,
      child: Stack(
        children: [
          // Background circle
          Container(
            decoration: BoxDecoration(
              shape: BoxShape.circle,
              color: AppTheme.textMedium.withOpacity(0.2),
            ),
          ),
          
          // Progress circle
          CustomPaint(
            size: Size(20.w, 20.h),
            painter: CircularProgressPainter(
              progress: progress,
              color: widget.accentColor ?? AppTheme.accentLavender,
              strokeWidth: 2.w,
            ),
          ),
          
          // Center dot
          Center(
            child: Container(
              width: 6.w,
              height: 6.h,
              decoration: BoxDecoration(
                shape: BoxShape.circle,
                color: (widget.accentColor ?? AppTheme.accentLavender).withOpacity(0.8),
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildTimerText() {
    return Text(
      _formatTime(widget.remainingSeconds),
      style: TextStyle(
        fontSize: 18.sp,
        fontWeight: FontWeight.bold,
        color: widget.accentColor ?? AppTheme.accentLavender,
        letterSpacing: 0.5,
      ),
    );
  }

  String _formatTime(int seconds) {
    final minutes = seconds ~/ 60;
    final remainingSeconds = seconds % 60;
    return '${minutes.toString().padLeft(2, '0')}:${remainingSeconds.toString().padLeft(2, '0')}';
  }
}

class CircularProgressPainter extends CustomPainter {
  final double progress;
  final Color color;
  final double strokeWidth;

  CircularProgressPainter({
    required this.progress,
    required this.color,
    required this.strokeWidth,
  });

  @override
  void paint(Canvas canvas, Size size) {
    final center = Offset(size.width / 2, size.height / 2);
    final radius = (size.width - strokeWidth) / 2;
    
    final paint = Paint()
      ..color = color
      ..strokeWidth = strokeWidth
      ..style = PaintingStyle.stroke
      ..strokeCap = StrokeCap.round;
    
    const startAngle = -math.pi / 2; // Start from top
    final sweepAngle = 2 * math.pi * progress;
    
    canvas.drawArc(
      Rect.fromCircle(center: center, radius: radius),
      startAngle,
      sweepAngle,
      false,
      paint,
    );
  }

  @override
  bool shouldRepaint(covariant CircularProgressPainter oldDelegate) {
    // Only repaint if progress, color, or stroke width changed
    return oldDelegate.progress != progress ||
           oldDelegate.color != color ||
           oldDelegate.strokeWidth != strokeWidth;
  }
}

// Enhanced timer with session phases
class SessionTimer extends StatefulWidget {
  final int remainingSeconds;
  final int totalSeconds;
  final SessionPhase currentPhase;
  final bool isActive;
  final Color? accentColor;
  final VoidCallback? onTap;
  final VoidCallback? onPhaseComplete;

  const SessionTimer({
    Key? key,
    required this.remainingSeconds,
    required this.totalSeconds,
    required this.currentPhase,
    this.isActive = false,
    this.accentColor,
    this.onTap,
    this.onPhaseComplete,
  }) : super(key: key);

  @override
  _SessionTimerState createState() => _SessionTimerState();
}

class _SessionTimerState extends State<SessionTimer>
    with TickerProviderStateMixin {
  late AnimationController _phaseController;
  late Animation<double> _phaseAnimation;

  @override
  void initState() {
    super.initState();
    
    _phaseController = AnimationController(
      duration: Duration(milliseconds: 1000),
      vsync: this,
    );
    
    _phaseAnimation = Tween<double>(
      begin: 0.0,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _phaseController,
      curve: Curves.easeInOut,
    ));
  }

  @override
  void didUpdateWidget(SessionTimer oldWidget) {
    super.didUpdateWidget(oldWidget);
    if (widget.currentPhase != oldWidget.currentPhase) {
      _phaseController.forward().then((_) {
        _phaseController.reverse();
        if (widget.onPhaseComplete != null) {
          widget.onPhaseComplete!();
        }
      });
    }
  }

  @override
  void dispose() {
    _phaseController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        // Phase indicator
        _buildPhaseIndicator(),
        
        SizedBox(height: 8.h),
        
        // Main timer
        MeditationTimer(
          remainingSeconds: widget.remainingSeconds,
          totalSeconds: widget.totalSeconds,
          isActive: widget.isActive,
          accentColor: widget.accentColor,
          onTap: widget.onTap,
        ),
      ],
    );
  }

  Widget _buildPhaseIndicator() {
    return AnimatedBuilder(
      animation: _phaseAnimation,
      builder: (context, child) {
        return Transform.scale(
          scale: 1.0 + (_phaseAnimation.value * 0.1),
          child: Container(
            padding: EdgeInsets.symmetric(horizontal: 12.w, vertical: 4.h),
            decoration: BoxDecoration(
              color: widget.currentPhase.color.withOpacity(0.2),
              borderRadius: BorderRadius.circular(12.r),
              border: Border.all(
                color: widget.currentPhase.color.withOpacity(0.5),
                width: 1.w,
              ),
            ),
            child: Row(
              mainAxisSize: MainAxisSize.min,
              children: [
                Icon(
                  widget.currentPhase.icon,
                  size: 12.sp,
                  color: widget.currentPhase.color,
                ),
                SizedBox(width: 4.w),
                Text(
                  widget.currentPhase.name,
                  style: TextStyle(
                    fontSize: 10.sp,
                    fontWeight: FontWeight.w500,
                    color: widget.currentPhase.color,
                  ),
                ),
              ],
            ),
          ),
        );
      },
    );
  }
}

enum SessionPhase {
  preparation,
  beginning,
  deepening,
  peak,
  integration,
  completion,
}

extension SessionPhaseExtension on SessionPhase {
  String get name {
    switch (this) {
      case SessionPhase.preparation:
        return 'Preparing';
      case SessionPhase.beginning:
        return 'Beginning';
      case SessionPhase.deepening:
        return 'Deepening';
      case SessionPhase.peak:
        return 'Peak';
      case SessionPhase.integration:
        return 'Integration';
      case SessionPhase.completion:
        return 'Completion';
    }
  }

  IconData get icon {
    switch (this) {
      case SessionPhase.preparation:
        return Icons.schedule;
      case SessionPhase.beginning:
        return Icons.play_arrow;
      case SessionPhase.deepening:
        return Icons.trending_down;
      case SessionPhase.peak:
        return Icons.star;
      case SessionPhase.integration:
        return Icons.trending_up;
      case SessionPhase.completion:
        return Icons.check_circle;
    }
  }

  Color get color {
    switch (this) {
      case SessionPhase.preparation:
        return AppTheme.accentGold;
      case SessionPhase.beginning:
        return AppTheme.accentMint;
      case SessionPhase.deepening:
        return AppTheme.visionBoardGradient[0];
      case SessionPhase.peak:
        return AppTheme.accentLavender;
      case SessionPhase.integration:
        return AppTheme.affirmationGradient[0];
      case SessionPhase.completion:
        return AppTheme.accentMint;
    }
  }

  Duration getDuration(int totalMinutes) {
    switch (this) {
      case SessionPhase.preparation:
        return Duration(seconds: 10);
      case SessionPhase.beginning:
        return Duration(minutes: (totalMinutes * 0.15).round());
      case SessionPhase.deepening:
        return Duration(minutes: (totalMinutes * 0.25).round());
      case SessionPhase.peak:
        return Duration(minutes: (totalMinutes * 0.4).round());
      case SessionPhase.integration:
        return Duration(minutes: (totalMinutes * 0.15).round());
      case SessionPhase.completion:
        return Duration(seconds: 30);
    }
  }
}