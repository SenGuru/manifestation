import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../constants/colors.dart';
import '../constants/app_theme.dart';

class ErrorPopup extends StatefulWidget {
  final String title;
  final String message;
  final String? actionText;
  final VoidCallback? onAction;
  final VoidCallback? onDismiss;

  const ErrorPopup({
    Key? key,
    required this.title,
    required this.message,
    this.actionText,
    this.onAction,
    this.onDismiss,
  }) : super(key: key);

  @override
  _ErrorPopupState createState() => _ErrorPopupState();

  static Future<void> show(
    BuildContext context, {
    required String title,
    required String message,
    String? actionText,
    VoidCallback? onAction,
    VoidCallback? onDismiss,
  }) {
    return showDialog(
      context: context,
      barrierDismissible: true,
      barrierColor: Colors.black.withOpacity(0.6),
      builder: (context) => ErrorPopup(
        title: title,
        message: message,
        actionText: actionText,
        onAction: onAction,
        onDismiss: onDismiss,
      ),
    );
  }
}

class _ErrorPopupState extends State<ErrorPopup>
    with TickerProviderStateMixin {
  late AnimationController _scaleController;
  late AnimationController _fadeController;
  late AnimationController _shakeController;
  late Animation<double> _scaleAnimation;
  late Animation<double> _fadeAnimation;
  late Animation<double> _shakeAnimation;

  @override
  void initState() {
    super.initState();

    _scaleController = AnimationController(
      duration: Duration(milliseconds: 300),
      vsync: this,
    );

    _fadeController = AnimationController(
      duration: Duration(milliseconds: 250),
      vsync: this,
    );

    _shakeController = AnimationController(
      duration: Duration(milliseconds: 600),
      vsync: this,
    );

    _scaleAnimation = Tween<double>(
      begin: 0.8,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _scaleController,
      curve: Curves.elasticOut,
    ));

    _fadeAnimation = Tween<double>(
      begin: 0.0,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _fadeController,
      curve: Curves.easeOut,
    ));

    _shakeAnimation = Tween<double>(
      begin: 0.0,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _shakeController,
      curve: Curves.elasticOut,
    ));

    // Start animations
    _fadeController.forward();
    _scaleController.forward();
    
    // Delayed shake animation for attention
    Future.delayed(Duration(milliseconds: 100), () {
      if (mounted) {
        _shakeController.forward();
      }
    });
  }

  @override
  void dispose() {
    _scaleController.dispose();
    _fadeController.dispose();
    _shakeController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return FadeTransition(
      opacity: _fadeAnimation,
      child: ScaleTransition(
        scale: _scaleAnimation,
        child: AnimatedBuilder(
          animation: _shakeAnimation,
          builder: (context, child) {
            final shakeValue = _shakeAnimation.value;
            final shakeOffset = (shakeValue * 2 - 1) * 3;
            
            return Transform.translate(
              offset: Offset(shakeOffset, 0),
              child: Dialog(
                backgroundColor: Colors.transparent,
                elevation: 0,
                child: Container(
                  margin: EdgeInsets.symmetric(horizontal: 20.w),
                  decoration: BoxDecoration(
                    color: AppTheme.cardBackground,
                    borderRadius: BorderRadius.circular(24.r),
                    boxShadow: [
                      BoxShadow(
                        color: Colors.red.withOpacity(0.2),
                        blurRadius: 30.r,
                        offset: Offset(0, 15.h),
                        spreadRadius: 5.r,
                      ),
                      BoxShadow(
                        color: AppTheme.black.withOpacity(0.3),
                        blurRadius: 20.r,
                        offset: Offset(0, 8.h),
                      ),
                    ],
                  ),
                  child: Column(
                    mainAxisSize: MainAxisSize.min,
                    children: [
                      // Error Icon with gradient background
                      Container(
                        width: double.infinity,
                        padding: EdgeInsets.only(top: 32.h, bottom: 16.h),
                        child: Container(
                          width: 80.w,
                          height: 80.h,
                          decoration: BoxDecoration(
                            gradient: LinearGradient(
                              colors: [
                                Colors.red.shade400,
                                Colors.red.shade600,
                              ],
                              begin: Alignment.topLeft,
                              end: Alignment.bottomRight,
                            ),
                            shape: BoxShape.circle,
                            boxShadow: [
                              BoxShadow(
                                color: Colors.red.withOpacity(0.4),
                                blurRadius: 20.r,
                                offset: Offset(0, 8.h),
                              ),
                            ],
                          ),
                          child: Icon(
                            Icons.error_outline_rounded,
                            color: AppTheme.white,
                            size: 40.sp,
                          ),
                        ),
                      ),

                      // Title
                      Padding(
                        padding: EdgeInsets.symmetric(horizontal: 24.w),
                        child: Text(
                          widget.title,
                          style: TextStyle(
                            fontSize: 22.sp,
                            fontWeight: FontWeight.bold,
                            color: AppTheme.textLight,
                          ),
                          textAlign: TextAlign.center,
                        ),
                      ),

                      SizedBox(height: 12.h),

                      // Message
                      Container(
                        constraints: BoxConstraints(maxHeight: 300.h),
                        child: SingleChildScrollView(
                          padding: EdgeInsets.symmetric(horizontal: 24.w),
                          child: Text(
                            widget.message,
                            style: TextStyle(
                              fontSize: 16.sp,
                              color: AppTheme.textMedium,
                              height: 1.4,
                            ),
                            textAlign: TextAlign.center,
                          ),
                        ),
                      ),

                      SizedBox(height: 32.h),

                      // Action Buttons
                      Padding(
                        padding: EdgeInsets.symmetric(horizontal: 24.w),
                        child: Row(
                          children: [
                            // Dismiss Button
                            Expanded(
                              child: _buildButton(
                                text: 'Dismiss',
                                isPrimary: false,
                                onPressed: () {
                                  Navigator.of(context).pop();
                                  widget.onDismiss?.call();
                                },
                              ),
                            ),

                            if (widget.actionText != null && widget.onAction != null) ...[
                              SizedBox(width: 12.w),
                              // Action Button
                              Expanded(
                                child: _buildButton(
                                  text: widget.actionText!,
                                  isPrimary: true,
                                  onPressed: () {
                                    Navigator.of(context).pop();
                                    widget.onAction?.call();
                                  },
                                ),
                              ),
                            ],
                          ],
                        ),
                      ),

                      SizedBox(height: 24.h),
                    ],
                  ),
                ),
              ),
            );
          },
        ),
      ),
    );
  }

  Widget _buildButton({
    required String text,
    required bool isPrimary,
    required VoidCallback onPressed,
  }) {
    return Container(
      height: 48.h,
      child: ElevatedButton(
        onPressed: onPressed,
        style: ElevatedButton.styleFrom(
          backgroundColor: Colors.transparent,
          shadowColor: Colors.transparent,
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(14.r),
          ),
          padding: EdgeInsets.zero,
          elevation: 0,
        ),
        child: Ink(
          decoration: BoxDecoration(
            gradient: isPrimary
                ? LinearGradient(
                    colors: [Colors.red.shade400, Colors.red.shade600],
                  )
                : null,
            color: isPrimary ? null : AppTheme.textMedium.withOpacity(0.1),
            borderRadius: BorderRadius.circular(14.r),
            border: isPrimary
                ? null
                : Border.all(
                    color: AppTheme.textMedium.withOpacity(0.3),
                    width: 1,
                  ),
          ),
          child: Container(
            alignment: Alignment.center,
            child: Text(
              text,
              style: TextStyle(
                fontSize: 16.sp,
                fontWeight: FontWeight.w600,
                color: isPrimary ? AppTheme.white : AppTheme.textMedium,
              ),
            ),
          ),
        ),
      ),
    );
  }
}

// Success popup variant
class SuccessPopup extends StatefulWidget {
  final String title;
  final String message;
  final String? actionText;
  final VoidCallback? onAction;
  final VoidCallback? onDismiss;

  const SuccessPopup({
    Key? key,
    required this.title,
    required this.message,
    this.actionText,
    this.onAction,
    this.onDismiss,
  }) : super(key: key);

  @override
  _SuccessPopupState createState() => _SuccessPopupState();

  static Future<void> show(
    BuildContext context, {
    required String title,
    required String message,
    String? actionText,
    VoidCallback? onAction,
    VoidCallback? onDismiss,
  }) {
    return showDialog(
      context: context,
      barrierDismissible: true,
      barrierColor: Colors.black.withOpacity(0.6),
      builder: (context) => SuccessPopup(
        title: title,
        message: message,
        actionText: actionText,
        onAction: onAction,
        onDismiss: onDismiss,
      ),
    );
  }
}

class _SuccessPopupState extends State<SuccessPopup>
    with TickerProviderStateMixin {
  late AnimationController _scaleController;
  late AnimationController _fadeController;
  late AnimationController _bounceController;
  late Animation<double> _scaleAnimation;
  late Animation<double> _fadeAnimation;
  late Animation<double> _bounceAnimation;

  @override
  void initState() {
    super.initState();

    _scaleController = AnimationController(
      duration: Duration(milliseconds: 400),
      vsync: this,
    );

    _fadeController = AnimationController(
      duration: Duration(milliseconds: 300),
      vsync: this,
    );

    _bounceController = AnimationController(
      duration: Duration(milliseconds: 800),
      vsync: this,
    );

    _scaleAnimation = Tween<double>(
      begin: 0.7,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _scaleController,
      curve: Curves.elasticOut,
    ));

    _fadeAnimation = Tween<double>(
      begin: 0.0,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _fadeController,
      curve: Curves.easeOut,
    ));

    _bounceAnimation = Tween<double>(
      begin: 0.0,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _bounceController,
      curve: Curves.elasticOut,
    ));

    // Start animations
    _fadeController.forward();
    _scaleController.forward();
    
    // Delayed bounce animation
    Future.delayed(Duration(milliseconds: 200), () {
      if (mounted) {
        _bounceController.forward();
      }
    });
  }

  @override
  void dispose() {
    _scaleController.dispose();
    _fadeController.dispose();
    _bounceController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return FadeTransition(
      opacity: _fadeAnimation,
      child: ScaleTransition(
        scale: _scaleAnimation,
        child: AnimatedBuilder(
          animation: _bounceAnimation,
          builder: (context, child) {
            return Dialog(
              backgroundColor: Colors.transparent,
              elevation: 0,
              child: Container(
                margin: EdgeInsets.symmetric(horizontal: 20.w),
                decoration: BoxDecoration(
                  color: AppTheme.cardBackground,
                  borderRadius: BorderRadius.circular(24.r),
                  boxShadow: [
                    BoxShadow(
                      color: Colors.green.withOpacity(0.2),
                      blurRadius: 30.r,
                      offset: Offset(0, 15.h),
                      spreadRadius: 5.r,
                    ),
                    BoxShadow(
                      color: AppTheme.black.withOpacity(0.3),
                      blurRadius: 20.r,
                      offset: Offset(0, 8.h),
                    ),
                  ],
                ),
                child: Column(
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    // Success Icon with gradient background
                    Container(
                      width: double.infinity,
                      padding: EdgeInsets.only(top: 32.h, bottom: 16.h),
                      child: Transform.scale(
                        scale: 0.8 + (_bounceAnimation.value * 0.2),
                        child: Container(
                          width: 80.w,
                          height: 80.h,
                          decoration: BoxDecoration(
                            gradient: LinearGradient(
                              colors: AppTheme.socialGradient,
                              begin: Alignment.topLeft,
                              end: Alignment.bottomRight,
                            ),
                            shape: BoxShape.circle,
                            boxShadow: [
                              BoxShadow(
                                color: AppTheme.socialGradient[0].withOpacity(0.4),
                                blurRadius: 20.r,
                                offset: Offset(0, 8.h),
                              ),
                            ],
                          ),
                          child: Icon(
                            Icons.check_rounded,
                            color: AppTheme.white,
                            size: 40.sp,
                          ),
                        ),
                      ),
                    ),

                    // Title
                    Padding(
                      padding: EdgeInsets.symmetric(horizontal: 24.w),
                      child: Text(
                        widget.title,
                        style: TextStyle(
                          fontSize: 22.sp,
                          fontWeight: FontWeight.bold,
                          color: AppTheme.textLight,
                        ),
                        textAlign: TextAlign.center,
                      ),
                    ),

                    SizedBox(height: 12.h),

                    // Message
                    Padding(
                      padding: EdgeInsets.symmetric(horizontal: 24.w),
                      child: Text(
                        widget.message,
                        style: TextStyle(
                          fontSize: 16.sp,
                          color: AppTheme.textMedium,
                          height: 1.4,
                        ),
                        textAlign: TextAlign.center,
                      ),
                    ),

                    SizedBox(height: 32.h),

                    // Action Buttons
                    Padding(
                      padding: EdgeInsets.symmetric(horizontal: 24.w),
                      child: Row(
                        children: [
                          // Dismiss Button
                          Expanded(
                            child: _buildButton(
                              text: 'Continue',
                              isPrimary: false,
                              onPressed: () {
                                Navigator.of(context).pop();
                                widget.onDismiss?.call();
                              },
                            ),
                          ),

                          if (widget.actionText != null && widget.onAction != null) ...[
                            SizedBox(width: 12.w),
                            // Action Button
                            Expanded(
                              child: _buildButton(
                                text: widget.actionText!,
                                isPrimary: true,
                                onPressed: () {
                                  Navigator.of(context).pop();
                                  widget.onAction?.call();
                                },
                              ),
                            ),
                          ],
                        ],
                      ),
                    ),

                    SizedBox(height: 24.h),
                  ],
                ),
              ),
            );
          },
        ),
      ),
    );
  }

  Widget _buildButton({
    required String text,
    required bool isPrimary,
    required VoidCallback onPressed,
  }) {
    return Container(
      height: 48.h,
      child: ElevatedButton(
        onPressed: onPressed,
        style: ElevatedButton.styleFrom(
          backgroundColor: Colors.transparent,
          shadowColor: Colors.transparent,
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(14.r),
          ),
          padding: EdgeInsets.zero,
          elevation: 0,
        ),
        child: Ink(
          decoration: BoxDecoration(
            gradient: isPrimary
                ? LinearGradient(colors: AppTheme.socialGradient)
                : null,
            color: isPrimary ? null : AppTheme.textMedium.withOpacity(0.1),
            borderRadius: BorderRadius.circular(14.r),
            border: isPrimary
                ? null
                : Border.all(
                    color: AppTheme.textMedium.withOpacity(0.3),
                    width: 1,
                  ),
          ),
          child: Container(
            alignment: Alignment.center,
            child: Text(
              text,
              style: TextStyle(
                fontSize: 16.sp,
                fontWeight: FontWeight.w600,
                color: isPrimary ? AppTheme.white : AppTheme.textMedium,
              ),
            ),
          ),
        ),
      ),
    );
  }
}