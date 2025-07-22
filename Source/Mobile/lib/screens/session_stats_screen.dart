import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../constants/colors.dart';

class SessionStatsScreen extends StatefulWidget {
  @override
  _SessionStatsScreenState createState() => _SessionStatsScreenState();
}

class _SessionStatsScreenState extends State<SessionStatsScreen> {
  Map<String, dynamic>? _stats;
  bool _isLoading = true;
  String _errorMessage = '';

  @override
  void initState() {
    super.initState();
    _loadStats();
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

      final stats = await _getUserStats();
      
      if (stats == null) {
        throw Exception('Stats data is null');
      }
      
      print('📊 SESSION STATS SCREEN: Stats loaded successfully');
      print('   Total sessions: ${stats['totalSessions']}');
      print('   Total time: ${stats['totalTimeInSeconds']}s');
      
      // Safe access to nested data
      final activities = stats['activities'] as Map<String, dynamic>? ?? {};
      final meditation = activities['meditation'] as Map<String, dynamic>? ?? {};
      final visionBoard = activities['visionBoard'] as Map<String, dynamic>? ?? {};
      final dailyAffirmations = activities['dailyAffirmations'] as Map<String, dynamic>? ?? {};
      
      print('   Meditation: ${meditation['totalSessions'] ?? 0} sessions, ${meditation['totalTimeInSeconds'] ?? 0}s');
      print('   Vision Board: ${visionBoard['totalSessions'] ?? 0} sessions, ${visionBoard['totalTimeInSeconds'] ?? 0}s');
      print('   Affirmations: ${dailyAffirmations['totalSessions'] ?? 0} sessions, ${dailyAffirmations['totalTimeInSeconds'] ?? 0}s');
      
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

  Future<Map<String, dynamic>> _getUserStats() async {
    // Simulate API delay
    await Future.delayed(Duration(seconds: 1));
    
    // Hardcoded statistics data
    return {
      'totalSessions': 42,
      'totalTimeInSeconds': 7200, // 2 hours
      'totalTimeFormatted': '2h 0m',
      'averageSessionTime': '5m 43s',
      'mostActiveActivity': 'Meditation',
      'currentStreak': 7,
      'longestStreak': 14,
      'thisWeekSessions': 5,
      'thisMonthSessions': 18,
      'favoriteTime': 'Morning',
      'totalDays': 30,
      'activeDays': 21,
      'consistencyRate': 70,
      'activities': {
        'meditation': {
          'totalSessions': 20,
          'totalTimeInSeconds': 3600,
          'totalTimeFormatted': '1h 0m',
          'averageSessionTime': '3m 0s',
          'percentage': 47.6,
        },
        'visionBoard': {
          'totalSessions': 12,
          'totalTimeInSeconds': 2400,
          'totalTimeFormatted': '40m 0s',
          'averageSessionTime': '3m 20s',
          'percentage': 28.6,
        },
        'affirmation': {
          'totalSessions': 10,
          'totalTimeInSeconds': 1200,
          'totalTimeFormatted': '20m 0s',
          'averageSessionTime': '2m 0s',
          'percentage': 23.8,
        },
      },
      'weeklyData': [
        {'day': 'Mon', 'sessions': 2, 'timeInMinutes': 15},
        {'day': 'Tue', 'sessions': 1, 'timeInMinutes': 8},
        {'day': 'Wed', 'sessions': 3, 'timeInMinutes': 22},
        {'day': 'Thu', 'sessions': 0, 'timeInMinutes': 0},
        {'day': 'Fri', 'sessions': 2, 'timeInMinutes': 12},
        {'day': 'Sat', 'sessions': 1, 'timeInMinutes': 5},
        {'day': 'Sun', 'sessions': 2, 'timeInMinutes': 18},
      ],
    };
  }

  Map<String, dynamic> _calculateActivityStats(List<dynamic> sessions) {
    if (sessions.isEmpty) {
      return {
        'totalSessions': 0,
        'totalTimeInSeconds': 0,
        'formattedTotalTime': '0m',
        'formattedAverageLength': '0m',
        'currentStreak': 0,
        'lastSessionDate': null,
        'weeklyActivity': _getEmptyWeeklyActivity(),
      };
    }

    int totalSessions = sessions.length;
    int totalTimeInSeconds = 0;
    DateTime? lastSessionDate;
    List<DateTime> sessionDates = [];

    for (final session in sessions) {
      // Calculate duration based on start/end times or duration field
      if (session['startTime'] != null && session['endTime'] != null) {
        final startTime = DateTime.parse(session['startTime']);
        final endTime = DateTime.parse(session['endTime']);
        totalTimeInSeconds += endTime.difference(startTime).inSeconds;
      } else if (session['durationInSeconds'] != null) {
        totalTimeInSeconds += session['durationInSeconds'] as int;
      }

      // Track session dates
      if (session['createdAt'] != null) {
        final sessionDate = DateTime.parse(session['createdAt']);
        sessionDates.add(sessionDate);
        if (lastSessionDate == null || sessionDate.isAfter(lastSessionDate)) {
          lastSessionDate = sessionDate;
        }
      }
    }

    // Calculate current streak
    int currentStreak = _calculateCurrentStreak(sessionDates);

    // Calculate average length
    int averageLength = totalSessions > 0 ? (totalTimeInSeconds / totalSessions).round() : 0;

    // Generate weekly activity
    List<Map<String, dynamic>> weeklyActivity = _generateWeeklyActivity(sessionDates);

    return {
      'totalSessions': totalSessions,
      'totalTimeInSeconds': totalTimeInSeconds,
      'formattedTotalTime': _formatDuration(totalTimeInSeconds),
      'formattedAverageLength': _formatDuration(averageLength),
      'currentStreak': currentStreak,
      'lastSessionDate': lastSessionDate,
      'weeklyActivity': weeklyActivity,
    };
  }

  int _calculateCurrentStreak(List<DateTime> sessionDates) {
    if (sessionDates.isEmpty) return 0;

    sessionDates.sort((a, b) => b.compareTo(a)); // Sort descending
    final today = DateTime.now();
    int streak = 0;

    for (int i = 0; i < sessionDates.length; i++) {
      final sessionDate = sessionDates[i];
      final daysDifference = today.difference(sessionDate).inDays;

      if (daysDifference == streak) {
        streak++;
      } else {
        break;
      }
    }

    return streak;
  }

  List<Map<String, dynamic>> _generateWeeklyActivity(List<DateTime> sessionDates) {
    final today = DateTime.now();
    final weekDays = ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'];
    List<Map<String, dynamic>> weeklyActivity = [];

    for (int i = 6; i >= 0; i--) {
      final date = today.subtract(Duration(days: i));
      final dayName = weekDays[date.weekday - 1];
      final sessionCount = sessionDates.where((sessionDate) =>
          sessionDate.year == date.year &&
          sessionDate.month == date.month &&
          sessionDate.day == date.day
      ).length;

      weeklyActivity.add({
        'dayName': dayName,
        'sessionCount': sessionCount,
        'date': date,
      });
    }

    return weeklyActivity;
  }

  List<Map<String, dynamic>> _getEmptyWeeklyActivity() {
    final weekDays = ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'];
    return weekDays.map((day) => {
      'dayName': day,
      'sessionCount': 0,
      'date': DateTime.now(),
    }).toList();
  }

  String _formatDuration(int seconds) {
    if (seconds == 0) return '0m';
    
    final hours = seconds ~/ 3600;
    final minutes = (seconds % 3600) ~/ 60;
    
    if (hours > 0) {
      return '${hours}h ${minutes}m';
    } else {
      return '${minutes}m';
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.backgroundDark,
      appBar: AppBar(
        backgroundColor: AppColors.backgroundDark,
        elevation: 0,
        leading: IconButton(
          icon: Icon(Icons.arrow_back_ios, color: AppColors.textLight),
          onPressed: () => Navigator.pop(context),
        ),
        title: Text(
          'Session Statistics',
          style: TextStyle(
            color: AppColors.textLight,
            fontSize: 20.sp,
            fontWeight: FontWeight.bold,
          ),
        ),
        actions: [
          IconButton(
            icon: Icon(Icons.refresh, color: AppColors.textLight),
            onPressed: _loadStats,
          ),
        ],
      ),
      body: _isLoading
          ? _buildLoadingState()
          : _errorMessage.isNotEmpty
              ? _buildErrorState()
              : _buildStatsContent(),
    );
  }

  Widget _buildLoadingState() {
    return Center(
      child: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          CircularProgressIndicator(
            valueColor: AlwaysStoppedAnimation<Color>(AppColors.accentLavender),
          ),
          SizedBox(height: 16.h),
          Text(
            'Loading your statistics...',
            style: TextStyle(
              color: AppColors.textMedium,
              fontSize: 16.sp,
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
              color: AppColors.error,
            ),
            SizedBox(height: 16.h),
            Text(
              'Error Loading Stats',
              style: TextStyle(
                color: AppColors.textLight,
                fontSize: 20.sp,
                fontWeight: FontWeight.bold,
              ),
            ),
            SizedBox(height: 8.h),
            Text(
              _errorMessage,
              style: TextStyle(
                color: AppColors.textMedium,
                fontSize: 14.sp,
              ),
              textAlign: TextAlign.center,
            ),
            SizedBox(height: 24.h),
            ElevatedButton(
              onPressed: _loadStats,
              style: ElevatedButton.styleFrom(
                backgroundColor: AppColors.accentLavender,
                foregroundColor: AppColors.white,
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
      color: AppColors.accentLavender,
      backgroundColor: AppColors.cardBackground,
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
            SizedBox(height: 24.h),
            _buildCurrentSessions(),
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
          colors: AppColors.analyticsGradient,
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
              color: AppColors.white,
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
                  _formatDuration(_stats!['totalTimeInSeconds']),
                  Icons.timer,
                ),
              ),
              SizedBox(width: 12.w),
              Expanded(
                child: _buildOverviewCard(
                  'Sessions',
                  _stats!['totalSessions'].toString(),
                  Icons.play_circle_outline,
                ),
              ),
            ],
          ),
          SizedBox(height: 12.h),
          _buildOverviewCard(
            'Most Active',
            _stats!['mostActiveActivity'],
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
        color: AppColors.white.withOpacity(0.1),
        borderRadius: BorderRadius.circular(12.r),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            children: [
              Icon(icon, color: AppColors.white, size: 20.sp),
              SizedBox(width: 8.w),
              Expanded(
                child: Text(
                  title,
                  style: TextStyle(
                    color: AppColors.white.withOpacity(0.8),
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
              color: AppColors.white,
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
            color: AppColors.textLight,
            fontSize: 18.sp,
            fontWeight: FontWeight.bold,
          ),
        ),
        SizedBox(height: 16.h),
        _buildActivityCard(
          'Meditation',
          _stats!['meditation'],
          AppColors.visualMeditationGradient,
          Icons.self_improvement,
        ),
        SizedBox(height: 12.h),
        _buildActivityCard(
          'Vision Board',
          _stats!['visionBoard'],
          AppColors.visionBoardGradient,
          Icons.dashboard,
        ),
        SizedBox(height: 12.h),
        _buildActivityCard(
          'Daily Affirmations',
          _stats!['dailyAffirmations'],
          AppColors.affirmationGradient,
          Icons.record_voice_over,
        ),
      ],
    );
  }

  Widget _buildActivityCard(String title, Map<String, dynamic> stats, List<Color> gradient, IconData icon) {
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
                child: Icon(icon, color: AppColors.white, size: 20.sp),
              ),
              SizedBox(width: 12.w),
              Expanded(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      title,
                      style: TextStyle(
                        color: AppColors.textLight,
                        fontSize: 16.sp,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                    Text(
                      '${stats['totalSessions']} sessions',
                      style: TextStyle(
                        color: AppColors.textMedium,
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
                    stats['formattedTotalTime'],
                    style: TextStyle(
                      color: gradient[0],
                      fontSize: 16.sp,
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                  Text(
                    'avg: ${stats['formattedAverageLength']}',
                    style: TextStyle(
                      color: AppColors.textMedium,
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
              _buildStatItem('Streak', '${stats['currentStreak']} days'),
              _buildStatItem('Last Session', _formatLastSession(stats['lastSessionDate'])),
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
            color: AppColors.textMedium,
            fontSize: 12.sp,
          ),
        ),
        SizedBox(height: 2.h),
        Text(
          value,
          style: TextStyle(
            color: AppColors.textLight,
            fontSize: 14.sp,
            fontWeight: FontWeight.w500,
          ),
        ),
      ],
    );
  }

  String _formatLastSession(DateTime? date) {
    if (date == null) return 'Never';
    
    final now = DateTime.now();
    final difference = now.difference(date);
    
    if (difference.inDays == 0) {
      return 'Today';
    } else if (difference.inDays == 1) {
      return 'Yesterday';
    } else if (difference.inDays < 7) {
      return '${difference.inDays} days ago';
    } else {
      return '${date.day}/${date.month}/${date.year}';
    }
  }

  Widget _buildWeeklyActivity() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Weekly Activity',
          style: TextStyle(
            color: AppColors.textLight,
            fontSize: 18.sp,
            fontWeight: FontWeight.bold,
          ),
        ),
        SizedBox(height: 16.h),
        Container(
          padding: EdgeInsets.all(16.w),
          decoration: BoxDecoration(
            color: AppColors.cardBackground,
            borderRadius: BorderRadius.circular(12.r),
          ),
          child: Column(
            children: [
              _buildWeeklyChart(_stats!['meditation'], 'Meditation', AppColors.visualMeditationGradient[0]),
              SizedBox(height: 16.h),
              _buildWeeklyChart(_stats!['visionBoard'], 'Vision Board', AppColors.visionBoardGradient[0]),
              SizedBox(height: 16.h),
              _buildWeeklyChart(_stats!['dailyAffirmations'], 'Affirmations', AppColors.affirmationGradient[0]),
            ],
          ),
        ),
      ],
    );
  }

  Widget _buildWeeklyChart(Map<String, dynamic> stats, String title, Color color) {
    final weeklyData = stats['weeklyActivity'] as List<Map<String, dynamic>>;
    final maxSessions = weeklyData.map((d) => d['sessionCount'] as int).reduce((a, b) => a > b ? a : b);
    
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          title,
          style: TextStyle(
            color: AppColors.textLight,
            fontSize: 14.sp,
            fontWeight: FontWeight.w500,
          ),
        ),
        SizedBox(height: 8.h),
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: weeklyData.map((data) {
            final heightRatio = maxSessions > 0 ? data['sessionCount'] / maxSessions : 0;
            return Column(
              children: [
                Container(
                  width: 24.w,
                  height: 30.h,
                  alignment: Alignment.bottomCenter,
                  child: Container(
                    width: 24.w,
                    height: (heightRatio * 30.h).clamp(2.h, 30.h),
                    decoration: BoxDecoration(
                      color: color.withOpacity(0.7),
                      borderRadius: BorderRadius.circular(2.r),
                    ),
                  ),
                ),
                SizedBox(height: 4.h),
                Text(
                  data['dayName'],
                  style: TextStyle(
                    color: AppColors.textMedium,
                    fontSize: 10.sp,
                  ),
                ),
              ],
            );
          }).toList(),
        ),
      ],
    );
  }

  Widget _buildCurrentSessions() {
    // Note: Active session tracking is now handled directly by the API
    // This section shows recent sessions instead of active ones
    
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Recent Sessions',
          style: TextStyle(
            color: AppColors.textLight,
            fontSize: 18.sp,
            fontWeight: FontWeight.bold,
          ),
        ),
        SizedBox(height: 16.h),
        Container(
          padding: EdgeInsets.all(16.w),
          decoration: BoxDecoration(
            color: AppColors.cardBackground,
            borderRadius: BorderRadius.circular(12.r),
          ),
          child: Row(
            children: [
              Icon(
                Icons.info_outline,
                color: AppColors.textMedium,
                size: 20.sp,
              ),
              SizedBox(width: 12.w),
              Text(
                'Session tracking managed by API',
                style: TextStyle(
                  color: AppColors.textMedium,
                  fontSize: 14.sp,
                ),
              ),
            ],
          ),
        ),
      ],
    );
  }

  // Active session tracking methods removed - now handled by API directly
}