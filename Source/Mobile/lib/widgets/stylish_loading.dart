import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:lottie/lottie.dart';
import '../constants/colors.dart';

class StylishLoading extends StatelessWidget {
  final String? message;
  final double? size;
  final bool showMessage;

  const StylishLoading({
    Key? key,
    this.message,
    this.size,
    this.showMessage = true,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Center(
      child: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          // Lottie animation
          Container(
            width: size ?? 120.w,
            height: size ?? 120.h,
            child: Lottie.asset(
              'assets/Loading.json',
              width: size ?? 120.w,
              height: size ?? 120.h,
              fit: BoxFit.contain,
              errorBuilder: (context, error, stackTrace) {
                // Fallback to circular indicator if Lottie fails
                return Container(
                  width: size ?? 120.w,
                  height: size ?? 120.h,
                  child: Center(
                    child: CircularProgressIndicator(
                      color: AppColors.affirmationGradient[0],
                      strokeWidth: 3.0,
                    ),
                  ),
                );
              },
            ),
          ),
          
          if (showMessage) ...[
            SizedBox(height: 24.h),
            
            // Loading message
            Text(
              message ?? 'Loading...',
              style: TextStyle(
                fontSize: 16.sp,
                color: AppColors.textMedium,
                fontWeight: FontWeight.w500,
              ),
              textAlign: TextAlign.center,
            ),
          ],
        ],
      ),
    );
  }
}

class StylishLoadingOverlay extends StatelessWidget {
  final String? message;
  final bool isVisible;

  const StylishLoadingOverlay({
    Key? key,
    this.message,
    this.isVisible = true,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    if (!isVisible) return SizedBox.shrink();

    return Container(
      color: AppColors.backgroundDark.withOpacity(0.8),
      child: StylishLoading(
        message: message,
      ),
    );
  }
}

class StylishLoadingDialog extends StatelessWidget {
  final String? message;

  const StylishLoadingDialog({
    Key? key,
    this.message,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Dialog(
      backgroundColor: Colors.transparent,
      elevation: 0,
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
        child: StylishLoading(
          message: message,
          size: 80.w,
        ),
      ),
    );
  }

  static Future<void> show(
    BuildContext context, {
    String? message,
  }) {
    return showDialog(
      context: context,
      barrierDismissible: false,
      builder: (context) => StylishLoadingDialog(message: message),
    );
  }

  static void hide(BuildContext context) {
    Navigator.of(context).pop();
  }
}

// Small loading indicator for buttons
class CompactStylishLoading extends StatelessWidget {
  final Color? color;
  final double? size;

  const CompactStylishLoading({
    Key? key,
    this.color,
    this.size,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return SizedBox(
      width: size ?? 24.w,
      height: size ?? 24.h,
      child: Lottie.asset(
        'assets/Loading.json',
        width: size ?? 24.w,
        height: size ?? 24.h,
        fit: BoxFit.contain,
        errorBuilder: (context, error, stackTrace) {
          // Fallback to circular indicator if Lottie fails
          return SizedBox(
            width: size ?? 24.w,
            height: size ?? 24.h,
            child: CircularProgressIndicator(
              color: color ?? AppColors.white,
              strokeWidth: 2.0,
            ),
          );
        },
      ),
    );
  }
}