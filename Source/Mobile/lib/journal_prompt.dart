import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../colors.dart';

class JournalPrompt extends StatefulWidget {
  final String prompt;
  final String initialText;
  final Function(String) onTextChanged;

  const JournalPrompt({
    Key? key,
    required this.prompt,
    required this.initialText,
    required this.onTextChanged,
  }) : super(key: key);

  @override
  _JournalPromptState createState() => _JournalPromptState();
}

class _JournalPromptState extends State<JournalPrompt> {
  late TextEditingController _controller;
  int wordCount = 0;

  @override
  void initState() {
    super.initState();
    _controller = TextEditingController(text: widget.initialText);
    _updateWordCount(widget.initialText);
  }

  @override
  void dispose() {
    _controller.dispose();
    super.dispose();
  }

  void _updateWordCount(String text) {
    final words = text.trim().split(RegExp(r'\s+'));
    setState(() {
      wordCount = text.trim().isEmpty ? 0 : words.length;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        // Prompt card
        Container(
          width: double.infinity,
          padding: EdgeInsets.all(20.w),
          decoration: BoxDecoration(
            gradient: LinearGradient(
              colors: [
                AppColors.accentGold.withOpacity(0.1),
                AppColors.accentGold.withOpacity(0.05),
              ],
            ),
            borderRadius: BorderRadius.circular(16.r),
            border: Border.all(
              color: AppColors.accentGold.withOpacity(0.3),
            ),
          ),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Row(
                children: [
                  Icon(
                    Icons.psychology,
                    color: AppColors.accentGold,
                    size: 24.sp,
                  ),
                  SizedBox(width: 12.w),
                  Text(
                    'Today\'s Prompt',
                    style: TextStyle(
                      fontSize: 16.sp,
                      fontWeight: FontWeight.w600,
                      color: AppColors.accentGold,
                    ),
                  ),
                ],
              ),
              SizedBox(height: 12.h),
              Text(
                widget.prompt,
                style: TextStyle(
                  fontSize: 18.sp,
                  fontWeight: FontWeight.w500,
                  color: AppColors.textLight,
                  height: 1.3,
                ),
              ),
            ],
          ),
        ),
        
        SizedBox(height: 24.h),
        
        // Writing area
        Expanded(
          child: Container(
            padding: EdgeInsets.all(20.w),
            decoration: BoxDecoration(
              color: AppColors.cardBackground,
              borderRadius: BorderRadius.circular(16.r),
              border: Border.all(
                color: AppColors.textMedium.withOpacity(0.2),
              ),
            ),
            child: Column(
              children: [
                // Stats bar
                Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: [
                    Text(
                      'Your Reflection',
                      style: TextStyle(
                        fontSize: 16.sp,
                        fontWeight: FontWeight.w600,
                        color: AppColors.textLight,
                      ),
                    ),
                    Container(
                      padding: EdgeInsets.symmetric(horizontal: 8.w, vertical: 4.h),
                      decoration: BoxDecoration(
                        color: AppColors.accentGold.withOpacity(0.1),
                        borderRadius: BorderRadius.circular(12.r),
                      ),
                      child: Text(
                        '$wordCount words',
                        style: TextStyle(
                          fontSize: 12.sp,
                          fontWeight: FontWeight.w500,
                          color: AppColors.accentGold,
                        ),
                      ),
                    ),
                  ],
                ),
                
                SizedBox(height: 16.h),
                
                // Text field
                Expanded(
                  child: TextField(
                    controller: _controller,
                    onChanged: (value) {
                      widget.onTextChanged(value);
                      _updateWordCount(value);
                    },
                    style: TextStyle(
                      color: AppColors.textLight,
                      fontSize: 16.sp,
                      height: 1.5,
                    ),
                    decoration: InputDecoration(
                      hintText: 'Start writing your thoughts...\n\nTake your time to reflect deeply. There\'s no rush.',
                      hintStyle: TextStyle(
                        color: AppColors.textMedium.withOpacity(0.6),
                        fontSize: 16.sp,
                        height: 1.5,
                      ),
                      border: InputBorder.none,
                      contentPadding: EdgeInsets.symmetric(
                        vertical: 12.h,
                        horizontal: 16.w,
                      ),
                    ),
                    maxLines: null,
                    expands: true,
                    textAlignVertical: TextAlignVertical.top,
                    textCapitalization: TextCapitalization.sentences,
                  ),
                ),
              ],
            ),
          ),
        ),
        
        // Writing tips
        SizedBox(height: 16.h),
        _buildWritingTips(),
      ],
    );
  }

  Widget _buildWritingTips() {
    final tips = [
      'Write freely without worrying about grammar',
      'Focus on your emotions and feelings',
      'Be honest and authentic with yourself',
    ];

    return Container(
      padding: EdgeInsets.all(16.w),
      decoration: BoxDecoration(
        color: AppColors.secondaryTeal.withOpacity(0.05),
        borderRadius: BorderRadius.circular(12.r),
        border: Border.all(
          color: AppColors.secondaryTeal.withOpacity(0.2),
        ),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            children: [
              Icon(
                Icons.tips_and_updates,
                color: AppColors.secondaryTeal,
                size: 16.sp,
              ),
              SizedBox(width: 8.w),
              Text(
                'Writing Tips',
                style: TextStyle(
                  fontSize: 14.sp,
                  fontWeight: FontWeight.w600,
                  color: AppColors.secondaryTeal,
                ),
              ),
            ],
          ),
          SizedBox(height: 8.h),
          ...tips.map((tip) => Padding(
            padding: EdgeInsets.only(bottom: 4.h),
            child: Row(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  '• ',
                  style: TextStyle(
                    color: AppColors.textMedium,
                    fontSize: 12.sp,
                  ),
                ),
                Expanded(
                  child: Text(
                    tip,
                    style: TextStyle(
                      color: AppColors.textMedium,
                      fontSize: 12.sp,
                      height: 1.3,
                    ),
                  ),
                ),
              ],
            ),
          )).toList(),
        ],
      ),
    );
  }
}

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