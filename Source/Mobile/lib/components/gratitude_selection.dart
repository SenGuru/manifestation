import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:fun/constants/colors.dart';

class GratitudeSection extends StatelessWidget {
  final List<String> gratitudeItems;
  final Function(int, String) onGratitudeChanged;

  const GratitudeSection({
    Key? key,
    required this.gratitudeItems,
    required this.onGratitudeChanged,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        // Instructions
        Container(
          padding: EdgeInsets.all(16.w),
          decoration: BoxDecoration(
            color: AppColors.gradientPink.withOpacity(0.1),
            borderRadius: BorderRadius.circular(12.r),
            border: Border.all(
              color: AppColors.gradientPink.withOpacity(0.3),
            ),
          ),
          child: Row(
            children: [
              Icon(
                Icons.lightbulb_outline,
                color: AppColors.gradientPink,
                size: 20.sp,
              ),
              SizedBox(width: 12.w),
              Expanded(
                child: Text(
                  'Focus on specific moments, people, or experiences that brought you joy today',
                  style: TextStyle(
                    fontSize: 14.sp,
                    color: AppColors.textMedium,
                    height: 1.3,
                  ),
                ),
              ),
            ],
          ),
        ),
        
        SizedBox(height: 30.h),
        
        // Gratitude items
        Expanded(
          child: ListView.builder(
            itemCount: 3,
            itemBuilder: (context, index) {
              return Container(
                margin: EdgeInsets.only(bottom: 20.h),
                child: _buildGratitudeItem(index),
              );
            },
          ),
        ),
      ],
    );
  }

  Widget _buildGratitudeItem(int index) {
    return Container(
      decoration: BoxDecoration(
        color: AppColors.cardBackground,
        borderRadius: BorderRadius.circular(16.r),
        border: Border.all(
          color: gratitudeItems[index].isNotEmpty
              ? AppColors.gradientPink.withOpacity(0.3)
              : AppColors.textMedium.withOpacity(0.2),
        ),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          // Header
          Container(
            padding: EdgeInsets.all(16.w),
            decoration: BoxDecoration(
              color: AppColors.gradientPink.withOpacity(0.05),
              borderRadius: BorderRadius.vertical(top: Radius.circular(16.r)),
            ),
            child: Row(
              children: [
                Container(
                  width: 32.w,
                  height: 32.h,
                  decoration: BoxDecoration(
                    color: AppColors.gradientPink,
                    shape: BoxShape.circle,
                  ),
                  child: Center(
                    child: Text(
                      '${index + 1}',
                      style: TextStyle(
                        fontSize: 16.sp,
                        fontWeight: FontWeight.bold,
                        color: AppColors.white,
                      ),
                    ),
                  ),
                ),
                SizedBox(width: 12.w),
                Text(
                  'I\'m grateful for...',
                  style: TextStyle(
                    fontSize: 16.sp,
                    fontWeight: FontWeight.w600,
                    color: AppColors.textLight,
                  ),
                ),
              ],
            ),
          ),
          
          // Text field
          Padding(
            padding: EdgeInsets.all(16.w),
            child: TextField(
              onChanged: (value) => onGratitudeChanged(index, value),
              controller: TextEditingController(text: gratitudeItems[index])
                ..selection = TextSelection.fromPosition(
                  TextPosition(offset: gratitudeItems[index].length),
                ),
              style: TextStyle(
                color: AppColors.textLight,
                fontSize: 16.sp,
                height: 1.4,
              ),
              decoration: InputDecoration(
                hintText: _getGratitudeHint(index),
                hintStyle: TextStyle(
                  color: AppColors.textMedium.withOpacity(0.6),
                  fontSize: 16.sp,
                ),
                border: InputBorder.none,
                contentPadding: EdgeInsets.symmetric(
                  vertical: 12.h,
                  horizontal: 16.w,
                ),
              ),
              maxLines: 3,
              textCapitalization: TextCapitalization.sentences,
            ),
          ),
        ],
      ),
    );
  }

  String _getGratitudeHint(int index) {
    switch (index) {
      case 0:
        return 'A person who made your day better...';
      case 1:
        return 'A moment that brought you peace...';
      case 2:
        return 'Something that made you smile...';
      default:
        return 'Something you\'re grateful for...';
    }
  }
}