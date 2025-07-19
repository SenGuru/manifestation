import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:fun/constants/colors.dart';
import 'package:fun/constants/category_icons.dart';

class CategoryChip extends StatelessWidget {
  final String name;
  final Color color;
  final bool isSelected;
  final bool isDefault;
  final VoidCallback onTap;
  final int? iconValue;

  const CategoryChip({
    Key? key,
    required this.name,
    required this.color,
    required this.isSelected,
    required this.onTap,
    this.isDefault = false,
    this.iconValue,
  }) : super(key: key);

  /// Get category icon using the comprehensive icon system
  IconData _getCategoryIcon() {
    // Use the icon value if provided, otherwise get default icon based on category name
    if (iconValue != null) {
      return CategoryIconUtils.getIconFromValue(iconValue);
    }
    
    // Fallback to default icon based on category name
    return CategoryIconUtils.getDefaultIconForCategory(name).iconData;
  }

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: onTap,
      child: AnimatedContainer(
        duration: Duration(milliseconds: 200),
        curve: Curves.easeInOut,
        padding: EdgeInsets.symmetric(
          horizontal: 20.w,
          vertical: 12.h,
        ),
        decoration: BoxDecoration(
          gradient: isSelected
              ? LinearGradient(
                  colors: [
                    color,
                    color.withOpacity(0.8),
                  ],
                  begin: Alignment.topLeft,
                  end: Alignment.bottomRight,
                )
              : null,
          color: isSelected ? null : AppColors.cardBackground,
          borderRadius: BorderRadius.circular(25.r),
          border: Border.all(
            color: isSelected 
                ? color.withOpacity(0.5)
                : AppColors.textMedium.withOpacity(0.3),
            width: 1,
          ),
          boxShadow: isSelected
              ? [
                  BoxShadow(
                    color: color.withOpacity(0.3),
                    blurRadius: 8.r,
                    offset: Offset(0, 2.h),
                  ),
                ]
              : null,
        ),
        child: Row(
          mainAxisSize: MainAxisSize.min,
          children: [
            // Show icon for all categories now
            Icon(
              _getCategoryIcon(),
              color: isSelected ? AppColors.white.withOpacity(0.8) : color.withOpacity(0.7),
              size: 14.sp,
            ),
            SizedBox(width: 6.w),
            Text(
              name,
              style: TextStyle(
                fontSize: 14.sp,
                fontWeight: isSelected ? FontWeight.w600 : FontWeight.w500,
                color: isSelected ? AppColors.white : AppColors.textMedium,
              ),
            ),
          ],
        ),
      ),
    );
  }
}