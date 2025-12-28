import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../constants/app_theme.dart';

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
    try {
    _controller.dispose();
    } catch (e) {
      debugPrint('❌ Error disposing controller: $e');
    }

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
    return Container(
      padding: EdgeInsets.all(20.w),
      decoration: BoxDecoration(
        color: AppTheme.cardBackground,
        borderRadius: BorderRadius.circular(16.r),
        border: Border.all(
          color: AppTheme.textMedium.withOpacity(0.2),
        ),
      ),
      child: Column(
        children: [
          // Simple header with word count
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              Text(
                'Your Reflection',
                style: TextStyle(
                  fontSize: 16.sp,
                  fontWeight: FontWeight.w600,
                  color: AppTheme.textLight,
                ),
              ),
              Container(
                padding: EdgeInsets.symmetric(horizontal: 8.w, vertical: 4.h),
                decoration: BoxDecoration(
                  color: AppTheme.accentGold.withOpacity(0.1),
                  borderRadius: BorderRadius.circular(12.r),
                ),
                child: Text(
                  '$wordCount words',
                  style: TextStyle(
                    fontSize: 12.sp,
                    fontWeight: FontWeight.w500,
                    color: AppTheme.accentGold,
                  ),
                ),
              ),
            ],
          ),
          
          SizedBox(height: 16.h),
          
          // Text field - takes up most of the space
          Expanded(
            child: TextField(
              controller: _controller,
              onChanged: (value) {
                widget.onTextChanged(value);
                _updateWordCount(value);
              },
              style: TextStyle(
                color: AppTheme.textLight,
                fontSize: 16.sp,
                height: 1.5,
              ),
              decoration: InputDecoration(
                hintText: 'Write about your day, your thoughts, or anything on your mind...',
                hintStyle: TextStyle(
                  color: AppTheme.textMedium.withOpacity(0.6),
                  fontSize: 16.sp,
                  height: 1.5,
                ),
                border: InputBorder.none,
                contentPadding: EdgeInsets.only(
                  top: 8.h,
                  left: 8.w,
                  right: 8.w,
                  bottom: 8.h,
                ),
                isDense: true,
              ),
              maxLines: null,
              expands: true,
              textAlignVertical: TextAlignVertical.top,
              textCapitalization: TextCapitalization.sentences,
            ),
          ),
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
            color: AppTheme.affirmationGradient[0].withOpacity(0.1),
            borderRadius: BorderRadius.circular(12.r),
            border: Border.all(
              color: AppTheme.affirmationGradient[0].withOpacity(0.3),
            ),
          ),
          child: Row(
            children: [
              Icon(
                Icons.lightbulb_outline,
                color: AppTheme.affirmationGradient[0],
                size: 20.sp,
              ),
              SizedBox(width: 12.w),
              Expanded(
                child: Text(
                  'Focus on specific moments, people, or experiences that brought you joy today',
                  style: TextStyle(
                    fontSize: 14.sp,
                    color: AppTheme.textMedium,
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
        color: AppTheme.cardBackground,
        borderRadius: BorderRadius.circular(16.r),
        border: Border.all(
          color: gratitudeItems[index].isNotEmpty
              ? AppTheme.affirmationGradient[0].withOpacity(0.3)
              : AppTheme.textMedium.withOpacity(0.2),
        ),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          // Header
          Container(
            padding: EdgeInsets.all(16.w),
            decoration: BoxDecoration(
              color: AppTheme.affirmationGradient[0].withOpacity(0.05),
              borderRadius: BorderRadius.vertical(top: Radius.circular(16.r)),
            ),
            child: Row(
              children: [
                Container(
                  width: 32.w,
                  height: 32.h,
                  decoration: BoxDecoration(
                    color: AppTheme.affirmationGradient[0],
                    shape: BoxShape.circle,
                  ),
                  child: Center(
                    child: Text(
                      '${index + 1}',
                      style: TextStyle(
                        fontSize: 16.sp,
                        fontWeight: FontWeight.bold,
                        color: AppTheme.white,
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
                    color: AppTheme.textLight,
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
                color: AppTheme.textLight,
                fontSize: 16.sp,
                height: 1.4,
              ),
              decoration: InputDecoration(
                hintText: _getGratitudeHint(index),
                hintStyle: TextStyle(
                  color: AppTheme.textMedium.withOpacity(0.6),
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