import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../constants/colors.dart';
import '../constants/app_theme.dart';

class UniversalBackButton extends StatelessWidget {
  final VoidCallback? onPressed;
  final double? size;
  final double? iconSize;
  
  const UniversalBackButton({
    Key? key,
    this.onPressed,
    this.size,
    this.iconSize,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: () {
        HapticFeedback.lightImpact();
        if (onPressed != null) {
          onPressed!();
        } else {
          Navigator.pop(context);
        }
      },
      child: Container(
        width: size ?? 44.w,
        height: size ?? 44.h,
        decoration: BoxDecoration(
          color: AppTheme.cardBackground,
          borderRadius: BorderRadius.circular(12.r),
          border: Border.all(
            color: AppTheme.borderSecondary, // Proper border overlay
            width: 1,
          ),
        ),
        child: Icon(
          Icons.arrow_back,
          color: AppTheme.textPrimary,
          size: iconSize ?? 20.sp,
        ),
      ),
    );
  }
}