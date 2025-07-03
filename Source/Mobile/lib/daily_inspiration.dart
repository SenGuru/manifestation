import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../colors.dart';

class DailyInspiration extends StatefulWidget {
  @override
  _DailyInspirationState createState() => _DailyInspirationState();
}

class _DailyInspirationState extends State<DailyInspiration> {
  final List<Map<String, String>> inspirationalQuotes = [
    {
      'quote': 'Your thoughts become your reality. Choose them wisely.',
      'author': 'Unknown'
    },
    {
      'quote': 'What you think, you become. What you feel, you attract.',
      'author': 'Buddha'
    },
    {
      'quote': 'The universe conspires to help those who know what they want.',
      'author': 'Paulo Coelho'
    },
    {
      'quote': 'Believe in yourself and all that you are.',
      'author': 'Christian D. Larson'
    },
    {
      'quote': 'You are never too old to set another goal or dream a new dream.',
      'author': 'C.S. Lewis'
    },
    {
      'quote': 'The only impossible journey is the one you never begin.',
      'author': 'Tony Robbins'
    },
    {
      'quote': 'Your limitation—it\'s only your imagination.',
      'author': 'Unknown'
    },
  ];

  Map<String, String> get todaysQuote {
    // Use day of year to get consistent quote for each day
    final dayOfYear = DateTime.now().difference(DateTime(DateTime.now().year, 1, 1)).inDays;
    return inspirationalQuotes[dayOfYear % inspirationalQuotes.length];
  }

  @override
  Widget build(BuildContext context) {
    final quote = todaysQuote;
    
    return Container(
      width: double.infinity,
      padding: EdgeInsets.all(16.w),
      decoration: BoxDecoration(
        gradient: LinearGradient(
          colors: [
            AppColors.primaryMedium.withOpacity(0.8),
            AppColors.primaryLight.withOpacity(0.6),
          ],
          begin: Alignment.topLeft,
          end: Alignment.bottomRight,
        ),
        borderRadius: BorderRadius.circular(16.r),
        boxShadow: [
          BoxShadow(
            color: AppColors.primaryMedium.withOpacity(0.3),
            blurRadius: 8.r,
            offset: Offset(0, 4.h),
          ),
        ],
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          // Header
          Row(
            children: [
              Icon(
                Icons.auto_awesome,
                color: AppColors.accentGold,
                size: 18.sp,
              ),
              SizedBox(width: 8.w),
              Text(
                'Daily Inspiration',
                style: TextStyle(
                  fontSize: 14.sp,
                  fontWeight: FontWeight.w600,
                  color: AppColors.white,
                ),
              ),
            ],
          ),
          
          SizedBox(height: 10.h),
          
          // Quote
          Text(
            '"${quote['quote']}"',
            style: TextStyle(
              fontSize: 13.sp,
              fontWeight: FontWeight.w400,
              color: AppColors.white,
              height: 1.3,
              fontStyle: FontStyle.italic,
            ),
            maxLines: 3,
            overflow: TextOverflow.ellipsis,
          ),
          
          SizedBox(height: 8.h),
          
          // Author
          Align(
            alignment: Alignment.centerRight,
            child: Text(
              '— ${quote['author']}',
              style: TextStyle(
                fontSize: 11.sp,
                fontWeight: FontWeight.w500,
                color: AppColors.white.withOpacity(0.8),
              ),
            ),
          ),
        ],
      ),
    );
  }
}