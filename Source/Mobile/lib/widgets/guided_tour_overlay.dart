import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'dart:math' as math;
import '../constants/colors.dart';

class GuidedTourOverlay extends StatefulWidget {
  final List<TourStep> steps;
  final VoidCallback onComplete;
  final VoidCallback? onSkip;

  const GuidedTourOverlay({
    Key? key,
    required this.steps,
    required this.onComplete,
    this.onSkip,
  }) : super(key: key);

  @override
  _GuidedTourOverlayState createState() => _GuidedTourOverlayState();
}

class _GuidedTourOverlayState extends State<GuidedTourOverlay>
    with TickerProviderStateMixin {
  int _currentStep = 0;
  late AnimationController _fadeController;
  late AnimationController _pulseController;
  late Animation<double> _fadeAnimation;
  late Animation<double> _pulseAnimation;

  @override
  void initState() {
    super.initState();
    
    _fadeController = AnimationController(
      duration: Duration(milliseconds: 300),
      vsync: this,
    );
    
    _pulseController = AnimationController(
      duration: Duration(milliseconds: 1500),
      vsync: this,
    );
    
    _fadeAnimation = Tween<double>(
      begin: 0.0,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _fadeController,
      curve: Curves.easeInOut,
    ));
    
    _pulseAnimation = Tween<double>(
      begin: 1.0,
      end: 1.2,
    ).animate(CurvedAnimation(
      parent: _pulseController,
      curve: Curves.easeInOut,
    ));
    
    _fadeController.forward();
    _pulseController.repeat(reverse: true);
  }

  @override
  void dispose() {
    _fadeController.dispose();
    _pulseController.dispose();
    super.dispose();
  }

  void _nextStep() {
    if (_currentStep < widget.steps.length - 1) {
      setState(() {
        _currentStep++;
      });
    } else {
      _completeTour();
    }
  }

  void _previousStep() {
    if (_currentStep > 0) {
      setState(() {
        _currentStep--;
      });
    }
  }

  void _completeTour() {
    _fadeController.reverse().then((_) {
      widget.onComplete();
    });
  }

  void _skipTour() {
    _fadeController.reverse().then((_) {
      if (widget.onSkip != null) {
        widget.onSkip!();
      } else {
        widget.onComplete();
      }
    });
  }

  @override
  Widget build(BuildContext context) {
    final currentStep = widget.steps[_currentStep];
    
    return FadeTransition(
      opacity: _fadeAnimation,
      child: Material(
        color: Colors.transparent,
        child: Stack(
          children: [
            // Dark overlay
            Container(
              color: Colors.black.withOpacity(0.7),
              width: double.infinity,
              height: double.infinity,
            ),
            
            // Highlight area (if target widget is specified)
            if (currentStep.targetKey != null)
              _buildHighlightArea(currentStep),
            
            // Tour content
            _buildTourContent(currentStep),
          ],
        ),
      ),
    );
  }

  Widget _buildHighlightArea(TourStep step) {
    return Positioned.fill(
      child: CustomPaint(
        painter: HighlightPainter(
          targetKey: step.targetKey!,
          pulseAnimation: _pulseAnimation,
        ),
      ),
    );
  }

  Widget _buildTourContent(TourStep step) {
    return Positioned(
      bottom: 100.h,
      left: 24.w,
      right: 24.w,
      child: Container(
        padding: EdgeInsets.all(24.w),
        decoration: BoxDecoration(
          color: AppColors.cardBackground,
          borderRadius: BorderRadius.circular(20.r),
          boxShadow: [
            BoxShadow(
              color: Colors.black.withOpacity(0.3),
              blurRadius: 20.r,
              offset: Offset(0, 10.h),
            ),
          ],
        ),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            // Step indicator
            Row(
              mainAxisAlignment: MainAxisAlignment.center,
              children: List.generate(widget.steps.length, (index) {
                return Container(
                  margin: EdgeInsets.only(right: 8.w),
                  width: index == _currentStep ? 24.w : 8.w,
                  height: 4.h,
                  decoration: BoxDecoration(
                    color: index == _currentStep 
                        ? AppColors.affirmationGradient[0]
                        : AppColors.textMedium.withOpacity(0.3),
                    borderRadius: BorderRadius.circular(2.r),
                  ),
                );
              }),
            ),
            
            SizedBox(height: 20.h),
            
            // Icon
            if (step.icon != null)
              Container(
                width: 60.w,
                height: 60.h,
                decoration: BoxDecoration(
                  gradient: LinearGradient(
                    colors: step.iconGradient ?? AppColors.affirmationGradient,
                  ),
                  shape: BoxShape.circle,
                ),
                child: Icon(
                  step.icon,
                  color: AppColors.white,
                  size: 30.sp,
                ),
              ),
            
            if (step.icon != null) SizedBox(height: 16.h),
            
            // Title
            Text(
              step.title,
              style: TextStyle(
                fontSize: 20.sp,
                fontWeight: FontWeight.bold,
                color: AppColors.textLight,
              ),
              textAlign: TextAlign.center,
            ),
            
            SizedBox(height: 12.h),
            
            // Description
            Text(
              step.description,
              style: TextStyle(
                fontSize: 16.sp,
                color: AppColors.textMedium,
                height: 1.4,
              ),
              textAlign: TextAlign.center,
            ),
            
            SizedBox(height: 24.h),
            
            // Navigation buttons
            Row(
              children: [
                // Skip button
                if (widget.onSkip != null)
                  Expanded(
                    child: Container(
                      height: 56.h,
                      child: ElevatedButton(
                        onPressed: _skipTour,
                        style: ElevatedButton.styleFrom(
                          backgroundColor: AppColors.cardBackground,
                          shadowColor: Colors.transparent,
                          shape: RoundedRectangleBorder(
                            borderRadius: BorderRadius.circular(16.r),
                          ),
                          side: BorderSide(
                            color: AppColors.textMedium.withOpacity(0.2),
                            width: 1,
                          ),
                        ),
                        child: FittedBox(
                          fit: BoxFit.scaleDown,
                          child: Text(
                            'Skip Tour',
                            style: TextStyle(
                              fontSize: 16.sp,
                              fontWeight: FontWeight.w600,
                              color: AppColors.textMedium,
                            ),
                            maxLines: 1,
                            overflow: TextOverflow.ellipsis,
                          ),
                        ),
                      ),
                    ),
                  ),
                
                // Previous button
                if (_currentStep > 0) ...[
                  if (widget.onSkip != null) SizedBox(width: 12.w),
                  Expanded(
                    child: Container(
                      height: 56.h,
                      child: ElevatedButton(
                        onPressed: _previousStep,
                        style: ElevatedButton.styleFrom(
                          backgroundColor: AppColors.cardBackground,
                          shadowColor: Colors.transparent,
                          shape: RoundedRectangleBorder(
                            borderRadius: BorderRadius.circular(16.r),
                          ),
                          side: BorderSide(
                            color: AppColors.textMedium.withOpacity(0.2),
                            width: 1,
                          ),
                        ),
                        child: FittedBox(
                          fit: BoxFit.scaleDown,
                          child: Text(
                            'Previous',
                            style: TextStyle(
                              fontSize: 16.sp,
                              fontWeight: FontWeight.w600,
                              color: AppColors.textLight,
                            ),
                            maxLines: 1,
                            overflow: TextOverflow.ellipsis,
                          ),
                        ),
                      ),
                    ),
                  ),
                ],
                
                if (_currentStep > 0 || widget.onSkip != null) SizedBox(width: 12.w),
                
                // Next/Finish button
                Expanded(
                  flex: widget.onSkip != null || _currentStep > 0 ? 1 : 2,
                  child: Container(
                    height: 56.h,
                    child: ElevatedButton(
                      onPressed: _nextStep,
                      style: ElevatedButton.styleFrom(
                        backgroundColor: Colors.transparent,
                        shadowColor: Colors.transparent,
                        shape: RoundedRectangleBorder(
                          borderRadius: BorderRadius.circular(16.r),
                        ),
                        padding: EdgeInsets.zero,
                      ).copyWith(
                        backgroundColor: MaterialStateProperty.all(Colors.transparent),
                      ),
                      child: Ink(
                        decoration: BoxDecoration(
                          gradient: LinearGradient(
                            colors: AppColors.affirmationGradient,
                            begin: Alignment.topLeft,
                            end: Alignment.bottomRight,
                          ),
                          borderRadius: BorderRadius.circular(16.r),
                          boxShadow: [
                            BoxShadow(
                              color: AppColors.affirmationGradient[0].withOpacity(0.3),
                              blurRadius: 12.r,
                              offset: Offset(0, 6.h),
                            ),
                          ],
                        ),
                        child: Container(
                          alignment: Alignment.center,
                          child: FittedBox(
                            fit: BoxFit.scaleDown,
                            child: Text(
                              _currentStep == widget.steps.length - 1 ? 'Got it!' : 'Next',
                              style: TextStyle(
                                fontSize: 16.sp,
                                fontWeight: FontWeight.w600,
                                color: AppColors.white,
                              ),
                              maxLines: 1,
                              overflow: TextOverflow.ellipsis,
                            ),
                          ),
                        ),
                      ),
                    ),
                  ),
                ),
              ],
            ),
          ],
        ),
      ),
    );
  }
}

class TourStep {
  final String title;
  final String description;
  final IconData? icon;
  final List<Color>? iconGradient;
  final GlobalKey? targetKey;

  TourStep({
    required this.title,
    required this.description,
    this.icon,
    this.iconGradient,
    this.targetKey,
  });
}

class HighlightPainter extends CustomPainter {
  final GlobalKey targetKey;
  final Animation<double> pulseAnimation;

  HighlightPainter({
    required this.targetKey,
    required this.pulseAnimation,
  }) : super(repaint: pulseAnimation);

  @override
  void paint(Canvas canvas, Size size) {
    final renderBox = targetKey.currentContext?.findRenderObject() as RenderBox?;
    if (renderBox == null) return;

    final targetSize = renderBox.size;
    final targetPosition = renderBox.localToGlobal(Offset.zero);
    
    // Create a path that covers the entire screen except the target area
    final fullPath = Path()..addRect(Rect.fromLTWH(0, 0, size.width, size.height));
    
    // Create highlight circle
    final highlightRadius = (math.max(targetSize.width, targetSize.height) / 2 + 20.w) * pulseAnimation.value;
    final highlightCenter = Offset(
      targetPosition.dx + targetSize.width / 2,
      targetPosition.dy + targetSize.height / 2,
    );
    
    final highlightPath = Path()..addOval(
      Rect.fromCircle(center: highlightCenter, radius: highlightRadius),
    );
    
    // Subtract highlight from full path
    final cutoutPath = Path.combine(PathOperation.difference, fullPath, highlightPath);
    
    // Paint the overlay
    final paint = Paint()
      ..color = Colors.black.withOpacity(0.3)
      ..style = PaintingStyle.fill;
    
    canvas.drawPath(cutoutPath, paint);
    
    // Paint highlight border
    final borderPaint = Paint()
      ..color = AppColors.affirmationGradient[0].withOpacity(0.5)
      ..style = PaintingStyle.stroke
      ..strokeWidth = 3.w;
    
    canvas.drawCircle(highlightCenter, highlightRadius, borderPaint);
  }

  @override
  bool shouldRepaint(covariant CustomPainter oldDelegate) => true;
}

