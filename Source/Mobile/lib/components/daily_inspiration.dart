import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:mistel/constants/colors.dart';
import '../constants/app_theme.dart';
import 'package:mistel/widgets/stylish_loading.dart';
import 'dart:math' as math;

class MindfulnessMoment extends StatefulWidget {
  const MindfulnessMoment({
    Key? key,
  }) : super(key: key);

  @override
  _MindfulnessMomentState createState() => _MindfulnessMomentState();
}

class _MindfulnessMomentState extends State<MindfulnessMoment> 
    with TickerProviderStateMixin {
  late AnimationController _animationController;
  late Animation<double> _fadeAnimation;
  
  Map<String, dynamic> currentMoment = {};

  @override
  void initState() {
    super.initState();
    _setupAnimations();
    _loadMindfulnessMoment();
  }

  void _setupAnimations() {
    _animationController = AnimationController(
      duration: Duration(milliseconds: 500),
      vsync: this,
    );
    
    _fadeAnimation = Tween<double>(
      begin: 0.0,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _animationController,
      curve: Curves.easeInOut,
    ));
  }

  void _loadMindfulnessMoment() {
    setState(() {
      currentMoment = _getMindfulnessMoment();
    });
    _animationController.forward();
  }

  Map<String, dynamic> _getMindfulnessMoment() {
    final moments = [
      {
        'title': 'Deep Breathing',
        'instruction': 'Take 4 deep breaths in and out slowly.',
        'duration': '1 minute',
        'icon': Icons.air,
      },
      {
        'title': 'Gratitude Pause',
        'instruction': 'Think of 3 things you\'re grateful for.',
        'duration': '30 seconds',
        'icon': Icons.favorite,
      },
      {
        'title': 'Grounding',
        'instruction': 'Notice 5 things you see around you.',
        'duration': '1 minute',
        'icon': Icons.visibility,
      },
      {
        'title': 'Body Scan',
        'instruction': 'Scan from your toes to your head.',
        'duration': '1 minute',
        'icon': Icons.self_improvement,
      },
      {
        'title': 'Intention Setting',
        'instruction': 'Set a positive intention for today.',
        'duration': '30 seconds',
        'icon': Icons.wb_sunny,
      },
      {
        'title': 'Mindful Awareness',
        'instruction': 'Notice your thoughts without judgment.',
        'duration': '1 minute',
        'icon': Icons.cloud,
      },
      {
        'title': 'Present Moment',
        'instruction': 'Focus on what you feel right now.',
        'duration': '30 seconds',
        'icon': Icons.center_focus_strong,
      },
    ];
    
    final random = math.Random();
    return moments[random.nextInt(moments.length)];
  }

  void _refreshMoment() {
    _animationController.reset();
    _loadMindfulnessMoment();
  }

  @override
  Widget build(BuildContext context) {
    if (currentMoment.isEmpty) {
      return Container(
        width: double.infinity,
        height: 200.h,
        decoration: BoxDecoration(
          color: AppTheme.cardBackground,
          borderRadius: BorderRadius.circular(20.r),
          border: Border.all(
            color: AppTheme.borderSecondary,
            width: 1,
          ),
        ),
        child: Center(
          child: StylishLoading(
            showMessage: false,
            size: 60.w,
          ),
        ),
      );
    }
    
    return FadeTransition(
      opacity: _fadeAnimation,
      child: Container(
        width: double.infinity,
        height: 200.h,
        padding: EdgeInsets.all(20.w),
        decoration: BoxDecoration(
          gradient: LinearGradient(
            colors: [
              AppTheme.socialGradient[0].withOpacity(0.8),
              AppTheme.socialGradient[1].withOpacity(0.6),
            ],
            begin: Alignment.topLeft,
            end: Alignment.bottomRight,
          ),
          borderRadius: BorderRadius.circular(20.r),
          boxShadow: [
            BoxShadow(
              color: AppTheme.socialGradient[0].withOpacity(0.3),
              blurRadius: 8.r,
              offset: Offset(0, 4.h),
            ),
          ],
        ),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            // Header with refresh button
            Row(
              children: [
                Icon(
                  currentMoment['icon'] ?? Icons.self_improvement,
                  color: AppTheme.white,
                  size: 18.sp,
                ),
                SizedBox(width: 8.w),
                Expanded(
                  child: Text(
                    'Mindfulness Moment',
                    style: TextStyle(
                      fontSize: 14.sp,
                      fontWeight: FontWeight.w600,
                      color: AppTheme.white,
                    ),
                  ),
                ),
              ],
            ),
            
            SizedBox(height: 16.h),
            
            // Mindfulness content
            Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    currentMoment['title'] ?? '',
                    style: TextStyle(
                      fontSize: 18.sp,
                      fontWeight: FontWeight.bold,
                      color: AppTheme.white,
                    ),
                  ),
                  
                  SizedBox(height: 8.h),
                  
                  Expanded(
                    child: Text(
                      currentMoment['instruction'] ?? '',
                      style: TextStyle(
                        fontSize: 14.sp,
                        color: AppTheme.white.withOpacity(0.9),
                        height: 1.4,
                      ),
                      maxLines: 4,
                      overflow: TextOverflow.ellipsis,
                    ),
                  ),
                  
                  // Duration info
                  Row(
                    children: [
                      Icon(
                        Icons.schedule,
                        color: AppTheme.white.withOpacity(0.7),
                        size: 14.sp,
                      ),
                      SizedBox(width: 6.w),
                      Text(
                        currentMoment['duration'] ?? '',
                        style: TextStyle(
                          fontSize: 12.sp,
                          color: AppTheme.white.withOpacity(0.7),
                          fontStyle: FontStyle.italic,
                        ),
                      ),
                    ],
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }

  @override
  void dispose() {
    _animationController.dispose();
    super.dispose();
  }
}