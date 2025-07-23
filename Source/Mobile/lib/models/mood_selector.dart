import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:fun/constants/colors.dart';
import 'package:fun/models/journey_entry.dart';

class MoodSelector extends StatefulWidget {
  final Mood selectedMood;
  final Function(Mood) onMoodSelected;

  const MoodSelector({
    Key? key,
    required this.selectedMood,
    required this.onMoodSelected,
  }) : super(key: key);

  @override
  _MoodSelectorState createState() => _MoodSelectorState();
}

class _MoodSelectorState extends State<MoodSelector>
    with TickerProviderStateMixin {
  late AnimationController _scaleController;
  late Animation<double> _scaleAnimation;

  @override
  void initState() {
    super.initState();
    _scaleController = AnimationController(
      duration: Duration(milliseconds: 200),
      vsync: this,
    );
    _scaleAnimation = Tween<double>(
      begin: 1.0,
      end: 1.1,
    ).animate(CurvedAnimation(
      parent: _scaleController,
      curve: Curves.easeInOut,
    ));
  }

  @override
  void dispose() {
    _scaleController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        // Selected mood display
        _buildSelectedMoodDisplay(),
        
        SizedBox(height: 30.h),
        
        // Mood options - Make this scrollable
        Expanded(
          child: SingleChildScrollView(
            physics: BouncingScrollPhysics(),
            child: Column(
              children: Mood.values.map((mood) {
                return Container(
                  margin: EdgeInsets.only(bottom: 12.h),
                  child: _buildMoodOption(mood),
                );
              }).toList(),
            ),
          ),
        ),
      ],
    );
  }

  Widget _buildSelectedMoodDisplay() {
    return Container(
      padding: EdgeInsets.all(24.w),
      decoration: BoxDecoration(
        gradient: LinearGradient(
          colors: [
            widget.selectedMood.color.withOpacity(0.1),
            widget.selectedMood.color.withOpacity(0.05),
          ],
        ),
        borderRadius: BorderRadius.circular(20.r),
        border: Border.all(
          color: widget.selectedMood.color.withOpacity(0.3),
        ),
      ),
      child: Column(
        children: [
          Text(
            widget.selectedMood.emoji,
            style: TextStyle(fontSize: 48.sp),
          ),
          SizedBox(height: 12.h),
          Text(
            widget.selectedMood.displayName,
            style: TextStyle(
              fontSize: 20.sp,
              fontWeight: FontWeight.bold,
              color: AppColors.textLight,
            ),
          ),
          SizedBox(height: 4.h),
          Text(
            widget.selectedMood.description,
            style: TextStyle(
              fontSize: 14.sp,
              color: AppColors.textMedium,
            ),
            textAlign: TextAlign.center,
          ),
        ],
      ),
    );
  }

  Widget _buildMoodOption(Mood mood) {
    final isSelected = widget.selectedMood == mood;
    
    return GestureDetector(
      onTap: () {
        widget.onMoodSelected(mood);
        _scaleController.forward().then((_) {
          _scaleController.reverse();
        });
      },
      child: AnimatedBuilder(
        animation: _scaleAnimation,
        builder: (context, child) {
          return Transform.scale(
            scale: isSelected ? _scaleAnimation.value : 1.0,
            child: Container(
              width: double.infinity,
              padding: EdgeInsets.symmetric(horizontal: 20.w, vertical: 16.h),
              margin: EdgeInsets.symmetric(vertical: 4.h),
              decoration: BoxDecoration(
                color: isSelected 
                    ? mood.color.withOpacity(0.1)
                    : AppColors.cardBackground,
                borderRadius: BorderRadius.circular(16.r),
                border: Border.all(
                  color: isSelected 
                      ? mood.color
                      : AppColors.textMedium.withOpacity(0.2),
                  width: isSelected ? 2 : 1,
                ),
              ),
              child: Row(
                children: [
                  Text(
                    mood.emoji,
                    style: TextStyle(fontSize: 32.sp),
                  ),
                  SizedBox(width: 16.w),
                  Expanded(
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text(
                          mood.displayName,
                          style: TextStyle(
                            fontSize: 18.sp,
                            fontWeight: FontWeight.w600,
                            color: isSelected ? mood.color : AppColors.textLight,
                          ),
                        ),
                        SizedBox(height: 2.h),
                        Text(
                          mood.description,
                          style: TextStyle(
                            fontSize: 14.sp,
                            color: AppColors.textMedium,
                          ),
                        ),
                      ],
                    ),
                  ),
                  if (isSelected)
                    Icon(
                      Icons.check_circle,
                      color: mood.color,
                      size: 24.sp,
                    ),
                ],
              ),
            ),
          );
        },
      ),
    );
  }
}