import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../constants/colors.dart';
import '../constants/app_theme.dart';
import '../services/usage_analytics_service.dart';
import '../models/analytics_models.dart';
import '../widgets/stylish_loading.dart';

class SessionStatsScreen extends StatefulWidget {
  @override
  _SessionStatsScreenState createState() => _SessionStatsScreenState();
}

class _SessionStatsScreenState extends State<SessionStatsScreen> {
  UsageStats? _stats;
  bool _isLoading = true;
  String _errorMessage = '';
  
  final UsageAnalyticsService _analyticsService = UsageAnalyticsService();

  @override
  void initState() {
    super.initState();
    _initializeAndLoadStats();
  }
  
  Future<void> _initializeAndLoadStats() async {
    await _analyticsService.initialize();
    await _loadStats();
  }

  Future<void> _loadStats() async {
    try {
      print('📊 SESSION STATS SCREEN: Loading stats...');
      if (mounted) {
        setState(() {
          _isLoading = true;
          _errorMessage = '';
        });
      }

      final stats = await _analyticsService.getUsageStats();
      
      print('📊 SESSION STATS SCREEN: Stats loaded successfully');
      print('   Total sessions: ${stats.totalSessions}');
      print('   Total time: ${stats.totalTimeInSeconds}s');
      print('   Most active: ${stats.mostActiveActivity.name}');
      
      for (final activity in stats.activities.keys) {
        final activityStats = stats.activities[activity]!;
        print('   ${activity.name}: ${activityStats.totalSessions} sessions, ${activityStats.totalTimeInSeconds}s');
      }
      
      if (mounted) {
        setState(() {
          _stats = stats;
          _isLoading = false;
        });
      }
    } catch (e) {
      print('❌ SESSION STATS SCREEN: Error loading stats: $e');
      if (mounted) {
        setState(() {
          _errorMessage = 'Failed to load statistics: $e';
          _isLoading = false;
        });
      }
    }
  }




  @override
  Widget build(BuildContext context) {
    if (_isLoading) {
      return Scaffold(
        backgroundColor: AppTheme.backgroundDark,
        body: Center(
          child: StylishLoading(
            message: 'Loading your statistics...',
          ),
        ),
      );
    }

    return Scaffold(
      backgroundColor: AppTheme.backgroundDark,
      body: SafeArea(
        child: Column(
          children: [
            _buildHeader(),
            Expanded(
              child: _errorMessage.isNotEmpty
                  ? _buildErrorState()
                  : _buildStatsContent(),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildHeader() {
    return Padding(
      padding: EdgeInsets.all(20.w),
      child: Row(
        children: [
          GestureDetector(
            onTap: () => Navigator.pop(context),
            child: Container(
              width: 44.w,
              height: 44.h,
              decoration: BoxDecoration(
                color: AppTheme.cardBackground,
                borderRadius: BorderRadius.circular(12.r),
                border: Border.all(
                  color: AppTheme.textMedium.withOpacity(0.2),
                ),
              ),
              child: Icon(
                Icons.arrow_back,
                color: AppTheme.textLight,
                size: 20.sp,
              ),
            ),
          ),
          SizedBox(width: 16.w),
          Expanded(
            child: Text(
              'Progress & Stats',
              style: TextStyle(
                color: AppTheme.textLight,
                fontSize: 20.sp,
                fontWeight: FontWeight.bold,
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildErrorState() {
    return Center(
      child: Padding(
        padding: EdgeInsets.all(20.w),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Icon(
              Icons.error_outline,
              size: 48.sp,
              color: AppTheme.error,
            ),
            SizedBox(height: 16.h),
            Text(
              'Error Loading Stats',
              style: TextStyle(
                color: AppTheme.textLight,
                fontSize: 20.sp,
                fontWeight: FontWeight.bold,
              ),
            ),
            SizedBox(height: 8.h),
            Text(
              _errorMessage,
              style: TextStyle(
                color: AppTheme.textMedium,
                fontSize: 14.sp,
              ),
              textAlign: TextAlign.center,
            ),
            SizedBox(height: 24.h),
            ElevatedButton(
              onPressed: _loadStats,
              style: ElevatedButton.styleFrom(
                backgroundColor: AppTheme.accentLavender,
                foregroundColor: AppTheme.white,
              ),
              child: Text('Retry'),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildStatsContent() {
    if (_stats == null) return Container();

    return RefreshIndicator(
      onRefresh: _loadStats,
      color: AppTheme.accentLavender,
      backgroundColor: AppTheme.cardBackground,
      child: SingleChildScrollView(
        physics: AlwaysScrollableScrollPhysics(),
        padding: EdgeInsets.all(20.w),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            _buildOverviewSection(),
            SizedBox(height: 24.h),
            _buildActivityBreakdown(),
            SizedBox(height: 24.h),
            _buildWeeklyActivity(),
            SizedBox(height: 20.h),
          ],
        ),
      ),
    );
  }

  Widget _buildOverviewSection() {
    return Container(
      padding: EdgeInsets.all(20.w),
      decoration: BoxDecoration(
        gradient: LinearGradient(
          colors: AppTheme.analyticsGradient,
          begin: Alignment.topLeft,
          end: Alignment.bottomRight,
        ),
        borderRadius: BorderRadius.circular(16.r),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(
            'Overview',
            style: TextStyle(
              color: AppTheme.white,
              fontSize: 20.sp,
              fontWeight: FontWeight.bold,
            ),
          ),
          SizedBox(height: 16.h),
          Row(
            children: [
              Expanded(
                child: _buildOverviewCard(
                  'Total Time',
                  _stats!.formattedTotalTime,
                  Icons.timer,
                ),
              ),
              SizedBox(width: 12.w),
              Expanded(
                child: _buildOverviewCard(
                  'Sessions',
                  _stats!.totalSessions.toString(),
                  Icons.play_circle_outline,
                ),
              ),
            ],
          ),
          SizedBox(height: 12.h),
          _buildOverviewCard(
            'Most Active',
            _stats!.mostActiveActivity.shortName,
            Icons.trending_up,
            fullWidth: true,
          ),
        ],
      ),
    );
  }

  Widget _buildOverviewCard(String title, String value, IconData icon, {bool fullWidth = false}) {
    return Container(
      padding: EdgeInsets.all(16.w),
      decoration: BoxDecoration(
        color: AppTheme.white.withOpacity(0.1),
        borderRadius: BorderRadius.circular(12.r),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            children: [
              Icon(icon, color: AppTheme.white, size: 20.sp),
              SizedBox(width: 8.w),
              Expanded(
                child: Text(
                  title,
                  style: TextStyle(
                    color: AppTheme.white.withOpacity(0.8),
                    fontSize: 14.sp,
                  ),
                ),
              ),
            ],
          ),
          SizedBox(height: 8.h),
          Text(
            value,
            style: TextStyle(
              color: AppTheme.white,
              fontSize: 18.sp,
              fontWeight: FontWeight.bold,
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildActivityBreakdown() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Activity Breakdown',
          style: TextStyle(
            color: AppTheme.textLight,
            fontSize: 18.sp,
            fontWeight: FontWeight.bold,
          ),
        ),
        SizedBox(height: 16.h),
        ..._stats!.activities.entries.map((entry) {
          final activityType = entry.key;
          final activityStats = entry.value;
          return Padding(
            padding: EdgeInsets.only(bottom: 12.h),
            child: _buildActivityCard(
              activityType.shortName,
              activityStats,
              activityType.gradientColors,
              activityType.icon,
            ),
          );
        }),
      ],
    );
  }

  Widget _buildActivityCard(String title, ActivityStats stats, List<Color> gradient, IconData icon) {
    return Container(
      padding: EdgeInsets.all(16.w),
      decoration: BoxDecoration(
        gradient: LinearGradient(
          colors: gradient.map((c) => c.withOpacity(0.1)).toList(),
          begin: Alignment.topLeft,
          end: Alignment.bottomRight,
        ),
        borderRadius: BorderRadius.circular(12.r),
        border: Border.all(
          color: gradient[0].withOpacity(0.3),
          width: 1.w,
        ),
      ),
      child: Column(
        children: [
          Row(
            children: [
              Container(
                padding: EdgeInsets.all(8.w),
                decoration: BoxDecoration(
                  gradient: LinearGradient(colors: gradient),
                  borderRadius: BorderRadius.circular(8.r),
                ),
                child: Icon(icon, color: AppTheme.white, size: 20.sp),
              ),
              SizedBox(width: 12.w),
              Expanded(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      title,
                      style: TextStyle(
                        color: AppTheme.textLight,
                        fontSize: 16.sp,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                    Text(
                      '${stats.totalSessions} sessions',
                      style: TextStyle(
                        color: AppTheme.textMedium,
                        fontSize: 12.sp,
                      ),
                    ),
                  ],
                ),
              ),
              Column(
                crossAxisAlignment: CrossAxisAlignment.end,
                children: [
                  Text(
                    stats.formattedTotalTime,
                    style: TextStyle(
                      color: gradient[0],
                      fontSize: 16.sp,
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                  Text(
                    'avg: ${stats.formattedAverageTime}',
                    style: TextStyle(
                      color: AppTheme.textMedium,
                      fontSize: 12.sp,
                    ),
                  ),
                ],
              ),
            ],
          ),
          SizedBox(height: 12.h),
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              _buildStatItem('Streak', '${stats.currentStreak} days'),
              _buildStatItem('Last Session', stats.formatLastSession()),
            ],
          ),
        ],
      ),
    );
  }

  Widget _buildStatItem(String label, String value) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          label,
          style: TextStyle(
            color: AppTheme.textMedium,
            fontSize: 12.sp,
          ),
        ),
        SizedBox(height: 2.h),
        Text(
          value,
          style: TextStyle(
            color: AppTheme.textLight,
            fontSize: 14.sp,
            fontWeight: FontWeight.w500,
          ),
        ),
      ],
    );
  }


  Widget _buildWeeklyActivity() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Row(
          children: [
            Container(
              padding: EdgeInsets.all(8.w),
              decoration: BoxDecoration(
                gradient: LinearGradient(colors: AppTheme.analyticsGradient),
                borderRadius: BorderRadius.circular(8.r),
              ),
              child: Icon(
                Icons.calendar_view_week,
                color: AppTheme.white,
                size: 20.sp,
              ),
            ),
            SizedBox(width: 12.w),
            Text(
              'Weekly Activity',
              style: TextStyle(
                color: AppTheme.textLight,
                fontSize: 20.sp,
                fontWeight: FontWeight.bold,
              ),
            ),
          ],
        ),
        SizedBox(height: 20.h),
        ..._stats!.activities.entries.map((entry) {
          final activityType = entry.key;
          final activityStats = entry.value;
          return Padding(
            padding: EdgeInsets.only(bottom: 20.h),
            child: _buildEnhancedWeeklyChart(
              activityStats,
              activityType.shortName,
              activityType.gradientColors,
              activityType.icon,
            ),
          );
        }),
      ],
    );
  }

  Widget _buildEnhancedWeeklyChart(ActivityStats stats, String title, List<Color> gradient, IconData icon) {
    final weeklyData = stats.weeklyActivity;
    final maxSessions = weeklyData.isEmpty ? 1 : weeklyData.map((d) => d.sessionCount).reduce((a, b) => a > b ? a : b);
    final totalWeekSessions = weeklyData.fold(0, (sum, data) => sum + data.sessionCount);
    
    return Container(
      padding: EdgeInsets.all(20.w),
      decoration: BoxDecoration(
        gradient: LinearGradient(
          colors: gradient.map((c) => c.withOpacity(0.05)).toList(),
          begin: Alignment.topLeft,
          end: Alignment.bottomRight,
        ),
        borderRadius: BorderRadius.circular(16.r),
        border: Border.all(
          color: gradient[0].withOpacity(0.2),
          width: 1.w,
        ),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          // Header with icon and title
          Row(
            children: [
              Container(
                padding: EdgeInsets.all(10.w),
                decoration: BoxDecoration(
                  gradient: LinearGradient(colors: gradient),
                  borderRadius: BorderRadius.circular(12.r),
                  boxShadow: [
                    BoxShadow(
                      color: gradient[0].withOpacity(0.3),
                      blurRadius: 8.r,
                      offset: Offset(0, 4.h),
                    ),
                  ],
                ),
                child: Icon(icon, color: AppTheme.white, size: 22.sp),
              ),
              SizedBox(width: 16.w),
              Expanded(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      title,
                      style: TextStyle(
                        color: AppTheme.textLight,
                        fontSize: 18.sp,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                    SizedBox(height: 4.h),
                    Text(
                      '$totalWeekSessions sessions this week',
                      style: TextStyle(
                        color: AppTheme.textMedium,
                        fontSize: 14.sp,
                      ),
                    ),
                  ],
                ),
              ),
            ],
          ),
          
          SizedBox(height: 24.h),
          
          // Chart
          Container(
            height: 100.h,
            child: Row(
              mainAxisAlignment: MainAxisAlignment.spaceEvenly,
              crossAxisAlignment: CrossAxisAlignment.end,
              children: weeklyData.asMap().entries.map((entry) {
                final index = entry.key;
                final data = entry.value;
                final heightRatio = maxSessions > 0 ? data.sessionCount / maxSessions : 0;
                final barHeight = (heightRatio * 60.h).clamp(4.h, 60.h);
                final isToday = data.date.day == DateTime.now().day;
                
                return Expanded(
                  child: Padding(
                    padding: EdgeInsets.symmetric(horizontal: 2.w),
                    child: Column(
                      mainAxisAlignment: MainAxisAlignment.end,
                      children: [
                        // Session count label
                        SizedBox(
                          height: 16.h,
                          child: data.sessionCount > 0 
                            ? Text(
                                '${data.sessionCount}',
                                style: TextStyle(
                                  color: gradient[0],
                                  fontSize: 10.sp,
                                  fontWeight: FontWeight.bold,
                                ),
                              )
                            : null,
                        ),
                        
                        SizedBox(height: 2.h),
                        
                        // Bar
                        Container(
                          width: 24.w,
                          height: barHeight,
                          decoration: BoxDecoration(
                            gradient: data.sessionCount > 0
                                ? LinearGradient(
                                    colors: gradient,
                                    begin: Alignment.bottomCenter,
                                    end: Alignment.topCenter,
                                  )
                                : null,
                            color: data.sessionCount == 0 
                                ? AppTheme.textMedium.withOpacity(0.1)
                                : null,
                            borderRadius: BorderRadius.circular(4.r),
                            boxShadow: data.sessionCount > 0 ? [
                              BoxShadow(
                                color: gradient[0].withOpacity(0.2),
                                blurRadius: 3.r,
                                offset: Offset(0, 1.h),
                              ),
                            ] : null,
                          ),
                        ),
                        
                        SizedBox(height: 4.h),
                        
                        // Day label
                        Container(
                          padding: EdgeInsets.symmetric(horizontal: 4.w, vertical: 1.h),
                          decoration: isToday ? BoxDecoration(
                            color: gradient[0].withOpacity(0.2),
                            borderRadius: BorderRadius.circular(6.r),
                          ) : null,
                          child: Text(
                            data.dayName,
                            style: TextStyle(
                              color: isToday ? gradient[0] : AppTheme.textMedium,
                              fontSize: 10.sp,
                              fontWeight: isToday ? FontWeight.bold : FontWeight.normal,
                            ),
                          ),
                        ),
                      ],
                    ),
                  ),
                );
              }).toList(),
            ),
          ),
        ],
      ),
    );
  }

  // Recent sessions section removed - now handled by API directly
}