import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../colors.dart';

class StreakCounter extends StatelessWidget {
  final int streakDays;
  final VoidCallback? onTap;

  const StreakCounter({
    Key? key,
    required this.streakDays,
    this.onTap,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: onTap,
      child: Container(
        width: double.infinity,
        padding: EdgeInsets.all(16.w),
        decoration: BoxDecoration(
          color: AppColors.cardBackground,
          borderRadius: BorderRadius.circular(16.r),
          border: Border.all(
            color: AppColors.accentGold.withOpacity(0.3),
            width: 1,
          ),
        ),
        child: Column(
          children: [
            // Fire icon with glow effect
            Container(
              width: 40.w,
              height: 40.h,
              decoration: BoxDecoration(
                shape: BoxShape.circle,
                gradient: LinearGradient(
                  colors: [
                    AppColors.accentGold,
                    AppColors.gradientOrange,
                  ],
                  begin: Alignment.topLeft,
                  end: Alignment.bottomRight,
                ),
                boxShadow: [
                  BoxShadow(
                    color: AppColors.accentGold.withOpacity(0.4),
                    blurRadius: 6.r,
                    offset: Offset(0, 2.h),
                  ),
                ],
              ),
              child: Icon(
                Icons.local_fire_department,
                color: AppColors.white,
                size: 20.sp,
              ),
            ),
            
            SizedBox(height: 8.h),
            
            // Streak number
            Text(
              '$streakDays',
              style: TextStyle(
                fontSize: 24.sp,
                fontWeight: FontWeight.bold,
                color: AppColors.accentGold,
              ),
            ),
            
            // Days label
            Text(
              streakDays == 1 ? 'Day' : 'Days',
              style: TextStyle(
                fontSize: 12.sp,
                fontWeight: FontWeight.w500,
                color: AppColors.textLight,
              ),
            ),
            
            SizedBox(height: 8.h),
            
            // Progress dots (horizontal)
            Row(
              mainAxisAlignment: MainAxisAlignment.center,
              children: List.generate(7, (index) {
                return Container(
                  margin: EdgeInsets.symmetric(horizontal: 2.w),
                  width: 6.w,
                  height: 6.h,
                  decoration: BoxDecoration(
                    shape: BoxShape.circle,
                    color: index < (streakDays % 7) 
                        ? AppColors.accentGold 
                        : AppColors.textMedium.withOpacity(0.3),
                  ),
                );
              }),
            ),
          ],
        ),
      ),
    );
  }

  String _getStreakMessage(int days) {
    if (days == 0) {
      return 'Start your manifestation journey today!';
    } else if (days == 1) {
      return 'Great start! Keep the momentum going.';
    } else if (days < 7) {
      return 'Building the habit. You\'re doing amazing!';
    } else if (days < 21) {
      return 'Consistency is key. Keep manifesting!';
    } else if (days < 30) {
      return 'You\'re on fire! Amazing dedication.';
    } else {
      return 'Manifestation master! You\'re unstoppable.';
    }
  }
}