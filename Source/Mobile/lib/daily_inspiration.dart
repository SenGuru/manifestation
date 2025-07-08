import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'daily_quote_service.dart';
import '../colors.dart';

class DailyInspiration extends StatefulWidget {
  final bool refreshOnEveryOpen;
  
  const DailyInspiration({
    Key? key,
    this.refreshOnEveryOpen = true,
  }) : super(key: key);

  @override
  _DailyInspirationState createState() => _DailyInspirationState();
}

class _DailyInspirationState extends State<DailyInspiration> 
    with SingleTickerProviderStateMixin {
  late AnimationController _animationController;
  late Animation<double> _fadeAnimation;
  late Animation<Offset> _slideAnimation;
  


  Map<String, String> currentQuote = {};

  @override
  void initState() {
    super.initState();
    _setupAnimations();
    // This runs every time the widget is created = every time user opens the app
    _loadQuote();
  }

  void _setupAnimations() {
    _animationController = AnimationController(
      duration: Duration(milliseconds: 1000),
      vsync: this,
    );
    
    _fadeAnimation = Tween<double>(
      begin: 0.0,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _animationController,
      curve: Curves.easeInOut,
    ));
    
    _slideAnimation = Tween<Offset>(
      begin: Offset(0, 0.3),
      end: Offset.zero,
    ).animate(CurvedAnimation(
      parent: _animationController,
      curve: Curves.easeOutCubic,
    ));
  }

  void _loadQuote() {
    setState(() {
      currentQuote = _getQuote();
    });
    _animationController.forward();
  }

  Map<String, String> _getQuote() {
    if (widget.refreshOnEveryOpen) {
      // Use DailyQuoteService for true randomness on every app open
      return DailyQuoteService.getRandomQuote();
    } else {
      // Use DailyQuoteService for consistent daily quote
      return DailyQuoteService.getDailyQuote();
    }
  }

  void _refreshQuote() {
    _animationController.reset();
    _loadQuote();
  }

  @override
  Widget build(BuildContext context) {
    // Calculate height based on 3 lines of text + padding + header + author
    final double fixedHeight = 24.h + 8.h + (13.sp * 1.3 * 3) + 8.h + 20.h + 32.h; // ~130h
    
    if (currentQuote.isEmpty) {
      return Container(
        width: double.infinity,
        height: fixedHeight,
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
        ),
        child: Center(
          child: CircularProgressIndicator(
            valueColor: AlwaysStoppedAnimation<Color>(AppColors.accentGold),
            strokeWidth: 2,
          ),
        ),
      );
    }
    
    return SlideTransition(
      position: _slideAnimation,
      child: FadeTransition(
        opacity: _fadeAnimation,
        child: Container(
          width: double.infinity,
          height: fixedHeight, // Fixed height based on 3 lines
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
              // Header with refresh button - Fixed height
              SizedBox(
                height: 24.h, // Fixed header height
                child: Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: [
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
                    GestureDetector(
                      onTap: _refreshQuote,
                      child: Container(
                        padding: EdgeInsets.all(4.w),
                        decoration: BoxDecoration(
                          color: AppColors.white.withOpacity(0.2),
                          borderRadius: BorderRadius.circular(8.r),
                        ),
                        child: Icon(
                          Icons.refresh,
                          color: AppColors.white,
                          size: 16.sp,
                        ),
                      ),
                    ),
                  ],
                ),
              ),
              
              SizedBox(height: 8.h), // Fixed spacing
              
              // Quote text area - Fixed height for exactly 3 lines
              Container(
                height: 13.sp * 1.3 * 3, // Exact height for 3 lines (font-size * line-height * 3)
                alignment: Alignment.centerLeft,
                child: Text(
                  '"${currentQuote['quote']}"',
                  style: TextStyle(
                    fontSize: 13.sp,
                    fontWeight: FontWeight.w400,
                    color: AppColors.white,
                    height: 1.3,
                    fontStyle: FontStyle.italic,
                  ),
                  maxLines: 3, // Maximum 3 lines
                  overflow: TextOverflow.ellipsis, // Truncate if too long
                ),
              ),
              
              SizedBox(height: 8.h), // Fixed spacing
              
              // Author - Fixed area at bottom
              SizedBox(
                height: 20.h, // Fixed height for author section
                child: Align(
                  alignment: Alignment.centerRight,
                  child: Container(
                    padding: EdgeInsets.symmetric(horizontal: 8.w, vertical: 2.h),
                    decoration: BoxDecoration(
                      color: AppColors.white.withOpacity(0.15),
                      borderRadius: BorderRadius.circular(6.r),
                    ),
                    child: Text(
                      '— ${currentQuote['author']}',
                      style: TextStyle(
                        fontSize: 11.sp,
                        fontWeight: FontWeight.w500,
                        color: AppColors.white.withOpacity(0.9),
                      ),
                      maxLines: 1, // Single line for author
                      overflow: TextOverflow.ellipsis,
                    ),
                  ),
                ),
              ),
            ],
          ),
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