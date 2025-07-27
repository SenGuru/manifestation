import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
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
          // Stylish circular loading indicator
          SizedBox(
            width: size ?? 80.w,
            height: size ?? 80.h,
            child: Stack(
              alignment: Alignment.center,
              children: [
                // Background circle
                Container(
                  width: size ?? 80.w,
                  height: size ?? 80.h,
                  decoration: BoxDecoration(
                    shape: BoxShape.circle,
                    color: AppColors.accentCoral.withValues(alpha: 0.1),
                  ),
                ),
                // Main loading indicator
                SizedBox(
                  width: (size ?? 80.w) * 0.8,
                  height: (size ?? 80.h) * 0.8,
                  child: CircularProgressIndicator(
                    strokeWidth: 4.0,
                    valueColor: AlwaysStoppedAnimation<Color>(AppColors.accentCoral),
                    backgroundColor: AppColors.accentCoral.withValues(alpha: 0.2),
                  ),
                ),
                // Inner accent circle
                Container(
                  width: (size ?? 80.w) * 0.3,
                  height: (size ?? 80.h) * 0.3,
                  decoration: BoxDecoration(
                    shape: BoxShape.circle,
                    gradient: LinearGradient(
                      colors: AppColors.affirmationGradient,
                      begin: Alignment.topLeft,
                      end: Alignment.bottomRight,
                    ),
                    boxShadow: [
                      BoxShadow(
                        color: AppColors.accentCoral.withValues(alpha: 0.3),
                        blurRadius: 8.r,
                        offset: Offset(0, 2.h),
                      ),
                    ],
                  ),
                ),
              ],
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
      color: AppColors.backgroundDark.withValues(alpha: 0.8),
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
              color: Colors.black.withValues(alpha: 0.3),
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
      child: CircularProgressIndicator(
        strokeWidth: 2.5,
        valueColor: AlwaysStoppedAnimation<Color>(color ?? AppColors.accentCoral),
        backgroundColor: (color ?? AppColors.accentCoral).withValues(alpha: 0.2),
      ),
    );
  }
}