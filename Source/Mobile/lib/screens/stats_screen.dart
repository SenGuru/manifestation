import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:manifestation_app/constants/colors.dart';
import 'package:manifestation_app/services/meditation_session_service.dart';
import 'package:manifestation_app/services/vision_board_service.dart';
import 'package:manifestation_app/services/session_service.dart';

class StatsScreen extends StatefulWidget {
  @override
  _StatsScreenState createState() => _StatsScreenState();
}

class _StatsScreenState extends State<StatsScreen> with SingleTickerProviderStateMixin {
  late TabController _tabController;
  late VisionBoardService _visionBoardService;
  
  @override
  void initState() {
    super.initState();
    _tabController = TabController(length: 3, vsync: this);
    _visionBoardService = VisionBoardService();
  }
  
  @override
  void dispose() {
    _tabController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.backgroundDark,
      appBar: AppBar(
        backgroundColor: AppColors.backgroundDark,
        elevation: 0,
        leading: IconButton(
          icon: Icon(Icons.arrow_back, color: AppColors.textLight),
          onPressed: () => Navigator.pop(context),
        ),
        title: Text(
          'Activity Stats',
          style: TextStyle(
            color: AppColors.textLight,
            fontSize: 20.sp,
            fontWeight: FontWeight.bold,
          ),
        ),
        bottom: TabBar(
          controller: _tabController,
          indicatorColor: AppColors.primaryLight,
          labelColor: AppColors.textLight,
          unselectedLabelColor: AppColors.textMedium,
          tabs: [
            Tab(text: 'Meditation'),
            Tab(text: 'Vision Board'),
            Tab(text: 'Affirmations'),
          ],
        ),
      ),
      body: TabBarView(
        controller: _tabController,
        children: [
          _buildMeditationStats(),
          _buildVisionBoardStats(),
          _buildAffirmationsStats(),
        ],
      ),
    );
  }

  Widget _buildMeditationStats() {
    print('Building meditation stats widget');
    
    return FutureBuilder<MeditationStats>(
      future: MeditationSessionService.instance.getMeditationStats(),
      builder: (context, snapshot) {
        print('Stats FutureBuilder state: ${snapshot.connectionState}');
        
        if (snapshot.connectionState == ConnectionState.waiting) {
          print('Stats loading...');
          return Center(
            child: CircularProgressIndicator(
              valueColor: AlwaysStoppedAnimation<Color>(AppColors.primaryLight),
            ),
          );
        }

        if (snapshot.hasError) {
          print('Stats error: ${snapshot.error}');
          return Center(
            child: Text(
              'Error loading meditation stats',
              style: TextStyle(color: AppColors.textMedium),
            ),
          );
        }

        final stats = snapshot.data!;
        print('Stats loaded: ${stats.totalSessions} sessions, ${stats.totalTimeInSeconds}s total time');
        return SingleChildScrollView(
          padding: EdgeInsets.all(20.w),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              _buildStatsCard(
                title: 'Total Sessions',
                value: stats.totalSessions.toString(),
                icon: Icons.self_improvement,
                gradient: AppColors.visualMeditationGradient,
              ),
              SizedBox(height: 16.h),
              _buildStatsCard(
                title: 'Total Time',
                value: stats.totalTimeFormatted,
                icon: Icons.access_time,
                gradient: AppColors.breathingGradient,
              ),
              SizedBox(height: 16.h),
              _buildStatsCard(
                title: 'Average Session',
                value: stats.averageTimeFormatted,
                icon: Icons.trending_up,
                gradient: AppColors.chakraGradient,
              ),
              SizedBox(height: 16.h),
              Row(
                children: [
                  Expanded(
                    child: _buildSmallStatsCard(
                      title: 'This Week',
                      value: stats.thisWeekSessions.toString(),
                      icon: Icons.calendar_today,
                      gradient: AppColors.gratitudeGradient,
                    ),
                  ),
                  SizedBox(width: 16.w),
                  Expanded(
                    child: _buildSmallStatsCard(
                      title: 'This Month',
                      value: stats.thisMonthSessions.toString(),
                      icon: Icons.calendar_month,
                      gradient: AppColors.mindfulnessGradient,
                    ),
                  ),
                ],
              ),
              SizedBox(height: 24.h),
              _buildRecentSessions(),
            ],
          ),
        );
      },
    );
  }

  Widget _buildVisionBoardStats() {
    print('Building vision board stats widget');
    
    return FutureBuilder<int?>(
      future: SessionService.instance.getUserId(),
      builder: (context, userSnapshot) {
        if (userSnapshot.connectionState == ConnectionState.waiting) {
          return Center(
            child: CircularProgressIndicator(
              valueColor: AlwaysStoppedAnimation<Color>(AppColors.primaryLight),
            ),
          );
        }

        final userId = userSnapshot.data;
        print('Vision board stats - User ID: $userId');

        return FutureBuilder<VisionBoardResult<VisionBoardStats>>(
          future: _visionBoardService.getVisionBoardStats(userId: userId),
          builder: (context, snapshot) {
            print('Vision board stats FutureBuilder state: ${snapshot.connectionState}');
            
            if (snapshot.connectionState == ConnectionState.waiting) {
              print('Vision board stats loading...');
              return Center(
                child: CircularProgressIndicator(
                  valueColor: AlwaysStoppedAnimation<Color>(AppColors.primaryLight),
                ),
              );
            }

            if (snapshot.hasError) {
              print('Vision board stats error: ${snapshot.error}');
              return SingleChildScrollView(
                padding: EdgeInsets.all(20.w),
                child: Column(
                  children: [
                    _buildPlaceholderMessage('Error loading vision board stats'),
                  ],
                ),
              );
            }

            final result = snapshot.data;
            if (result == null || !result.isSuccess || result.data == null) {
              print('Vision board stats failed: ${result?.message ?? 'no data'}');
              return SingleChildScrollView(
                padding: EdgeInsets.all(20.w),
                child: Column(
                  children: [
                    _buildPlaceholderMessage('No vision board data available'),
                  ],
                ),
              );
            }

            final stats = result.data!;
            print('Vision board stats loaded: ${stats.totalSessions} sessions, ${stats.totalTimeInSeconds}s total time');

            return SingleChildScrollView(
              padding: EdgeInsets.all(20.w),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  _buildStatsCard(
                    title: 'Vision Boards Created',
                    value: stats.totalSessions.toString(),
                    icon: Icons.dashboard,
                    gradient: AppColors.visionBoardGradient,
                  ),
                  SizedBox(height: 16.h),
                  _buildStatsCard(
                    title: 'Time Spent',
                    value: stats.totalTimeFormatted,
                    icon: Icons.access_time,
                    gradient: AppColors.analyticsGradient,
                  ),
                  SizedBox(height: 16.h),
                  _buildStatsCard(
                    title: 'Images Added',
                    value: stats.totalImages.toString(),
                    icon: Icons.image,
                    gradient: AppColors.calendarGradient,
                  ),
                  SizedBox(height: 16.h),
                  Row(
                    children: [
                      Expanded(
                        child: _buildSmallStatsCard(
                          title: 'This Week',
                          value: stats.thisWeekSessions.toString(),
                          icon: Icons.calendar_today,
                          gradient: AppColors.gratitudeGradient,
                        ),
                      ),
                      SizedBox(width: 16.w),
                      Expanded(
                        child: _buildSmallStatsCard(
                          title: 'This Month',
                          value: stats.thisMonthSessions.toString(),
                          icon: Icons.calendar_month,
                          gradient: AppColors.mindfulnessGradient,
                        ),
                      ),
                    ],
                  ),
                  SizedBox(height: 24.h),
                  _buildRecentVisionBoardSessions(userId: userId),
                ],
              ),
            );
          },
        );
      },
    );
  }

  Widget _buildAffirmationsStats() {
    return SingleChildScrollView(
      padding: EdgeInsets.all(20.w),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          _buildStatsCard(
            title: 'Affirmations Read',
            value: '156', // Placeholder
            icon: Icons.record_voice_over,
            gradient: AppColors.affirmationGradient,
          ),
          SizedBox(height: 16.h),
          _buildStatsCard(
            title: 'Time Spent',
            value: '2h 30m', // Placeholder
            icon: Icons.access_time,
            gradient: AppColors.analyticsGradient,
          ),
          SizedBox(height: 16.h),
          _buildStatsCard(
            title: 'Favorites',
            value: '23', // Placeholder
            icon: Icons.favorite,
            gradient: AppColors.gratitudeGradient,
          ),
          SizedBox(height: 16.h),
          Row(
            children: [
              Expanded(
                child: _buildSmallStatsCard(
                  title: 'This Week',
                  value: '28', // Placeholder
                  icon: Icons.calendar_today,
                  gradient: AppColors.manifestationGradient,
                ),
              ),
              SizedBox(width: 16.w),
              Expanded(
                child: _buildSmallStatsCard(
                  title: 'This Month',
                  value: '87', // Placeholder
                  icon: Icons.calendar_month,
                  gradient: AppColors.mindfulnessGradient,
                ),
              ),
            ],
          ),
          SizedBox(height: 24.h),
          _buildPlaceholderMessage('Affirmations stats coming soon!'),
        ],
      ),
    );
  }

  Widget _buildStatsCard({
    required String title,
    required String value,
    required IconData icon,
    required List<Color> gradient,
  }) {
    return Container(
      width: double.infinity,
      padding: EdgeInsets.all(20.w),
      decoration: BoxDecoration(
        gradient: LinearGradient(
          colors: gradient.map((color) => color.withOpacity(0.2)).toList(),
          begin: Alignment.topLeft,
          end: Alignment.bottomRight,
        ),
        borderRadius: BorderRadius.circular(16.r),
        border: Border.all(
          color: gradient[0].withOpacity(0.3),
          width: 1,
        ),
      ),
      child: Row(
        children: [
          Container(
            padding: EdgeInsets.all(12.w),
            decoration: BoxDecoration(
              gradient: LinearGradient(colors: gradient),
              borderRadius: BorderRadius.circular(12.r),
            ),
            child: Icon(
              icon,
              color: AppColors.white,
              size: 24.sp,
            ),
          ),
          SizedBox(width: 16.w),
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  title,
                  style: TextStyle(
                    color: AppColors.textMedium,
                    fontSize: 14.sp,
                    fontWeight: FontWeight.w500,
                  ),
                ),
                SizedBox(height: 4.h),
                Text(
                  value,
                  style: TextStyle(
                    color: AppColors.textLight,
                    fontSize: 24.sp,
                    fontWeight: FontWeight.bold,
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildSmallStatsCard({
    required String title,
    required String value,
    required IconData icon,
    required List<Color> gradient,
  }) {
    return Container(
      padding: EdgeInsets.all(16.w),
      decoration: BoxDecoration(
        gradient: LinearGradient(
          colors: gradient.map((color) => color.withOpacity(0.2)).toList(),
          begin: Alignment.topLeft,
          end: Alignment.bottomRight,
        ),
        borderRadius: BorderRadius.circular(16.r),
        border: Border.all(
          color: gradient[0].withOpacity(0.3),
          width: 1,
        ),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Icon(
            icon,
            color: gradient[0],
            size: 20.sp,
          ),
          SizedBox(height: 8.h),
          Text(
            title,
            style: TextStyle(
              color: AppColors.textMedium,
              fontSize: 12.sp,
              fontWeight: FontWeight.w500,
            ),
          ),
          SizedBox(height: 4.h),
          Text(
            value,
            style: TextStyle(
              color: AppColors.textLight,
              fontSize: 20.sp,
              fontWeight: FontWeight.bold,
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildRecentSessions() {
    print('Building recent sessions widget');
    
    return FutureBuilder<List<MeditationSession>>(
      future: MeditationSessionService.instance.getUserMeditationSessions(),
      builder: (context, snapshot) {
        print('Recent sessions FutureBuilder state: ${snapshot.connectionState}');
        
        if (snapshot.connectionState == ConnectionState.waiting) {
          print('Recent sessions loading...');
          return Center(
            child: CircularProgressIndicator(
              valueColor: AlwaysStoppedAnimation<Color>(AppColors.primaryLight),
            ),
          );
        }

        if (snapshot.hasError || !snapshot.hasData) {
          print('Recent sessions error: ${snapshot.hasError ? snapshot.error : 'no data'}');
          return _buildPlaceholderMessage('No recent sessions found');
        }

        final sessions = snapshot.data!;
        print('Recent sessions loaded: ${sessions.length} sessions');
        
        if (sessions.isEmpty) {
          print('No sessions found, showing placeholder');
          return _buildPlaceholderMessage('No meditation sessions yet');
        }

        // Show only the last 5 sessions
        final recentSessions = sessions.take(5).toList();
        print('Showing ${recentSessions.length} recent sessions');

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
            SizedBox(height: 12.h),
            ...recentSessions.map((session) => _buildSessionItem(session)).toList(),
          ],
        );
      },
    );
  }

  Widget _buildSessionItem(MeditationSession session) {
    final duration = session.timeSpentInSeconds ?? 0;
    final minutes = duration ~/ 60;
    final seconds = duration % 60;
    final durationText = minutes > 0 ? '${minutes}m ${seconds}s' : '${seconds}s';

    return Container(
      margin: EdgeInsets.only(bottom: 12.h),
      padding: EdgeInsets.all(16.w),
      decoration: BoxDecoration(
        color: AppColors.cardBackground,
        borderRadius: BorderRadius.circular(12.r),
        border: Border.all(
          color: AppColors.textMedium.withOpacity(0.1),
        ),
      ),
      child: Row(
        children: [
          Container(
            padding: EdgeInsets.all(8.w),
            decoration: BoxDecoration(
              gradient: LinearGradient(colors: AppColors.visualMeditationGradient),
              borderRadius: BorderRadius.circular(8.r),
            ),
            child: Icon(
              Icons.self_improvement,
              color: AppColors.white,
              size: 16.sp,
            ),
          ),
          SizedBox(width: 12.w),
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  'Meditation Session',
                  style: TextStyle(
                    color: AppColors.textLight,
                    fontSize: 14.sp,
                    fontWeight: FontWeight.w500,
                  ),
                ),
                SizedBox(height: 2.h),
                Text(
                  _formatDate(session.sessionDate),
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
                durationText,
                style: TextStyle(
                  color: AppColors.textLight,
                  fontSize: 14.sp,
                  fontWeight: FontWeight.bold,
                ),
              ),
              if (session.isLocal)
                Text(
                  'Local',
                  style: TextStyle(
                    color: AppColors.textMedium,
                    fontSize: 10.sp,
                  ),
                ),
            ],
          ),
        ],
      ),
    );
  }

  Widget _buildPlaceholderMessage(String message) {
    return Container(
      width: double.infinity,
      padding: EdgeInsets.all(40.w),
      child: Column(
        children: [
          Icon(
            Icons.info_outline,
            color: AppColors.textMedium,
            size: 48.sp,
          ),
          SizedBox(height: 16.h),
          Text(
            message,
            style: TextStyle(
              color: AppColors.textMedium,
              fontSize: 16.sp,
            ),
            textAlign: TextAlign.center,
          ),
        ],
      ),
    );
  }

  Widget _buildRecentVisionBoardSessions({int? userId}) {
    print('Building recent vision board sessions widget');
    
    return FutureBuilder<VisionBoardResult<List<VisionBoardSession>>>(
      future: _visionBoardService.getUserVisionBoards(userId: userId),
      builder: (context, snapshot) {
        print('Recent vision board sessions FutureBuilder state: ${snapshot.connectionState}');
        
        if (snapshot.connectionState == ConnectionState.waiting) {
          print('Recent vision board sessions loading...');
          return Center(
            child: CircularProgressIndicator(
              valueColor: AlwaysStoppedAnimation<Color>(AppColors.primaryLight),
            ),
          );
        }

        if (snapshot.hasError) {
          print('Recent vision board sessions error: ${snapshot.error}');
          return _buildPlaceholderMessage('No recent vision board sessions found');
        }

        final result = snapshot.data;
        if (result == null || !result.isSuccess || result.data == null) {
          print('Recent vision board sessions failed: ${result?.message ?? 'no data'}');
          return _buildPlaceholderMessage('No recent vision board sessions found');
        }

        final sessions = result.data!;
        print('Recent vision board sessions loaded: ${sessions.length} sessions');
        
        if (sessions.isEmpty) {
          print('No vision board sessions found, showing placeholder');
          return _buildPlaceholderMessage('No vision board sessions yet');
        }

        // Show only the last 5 sessions
        final recentSessions = sessions.take(5).toList();
        print('Showing ${recentSessions.length} recent vision board sessions');

        return Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              'Recent Vision Board Sessions',
              style: TextStyle(
                color: AppColors.textLight,
                fontSize: 18.sp,
                fontWeight: FontWeight.bold,
              ),
            ),
            SizedBox(height: 12.h),
            ...recentSessions.map((session) => _buildVisionBoardSessionItem(session)).toList(),
          ],
        );
      },
    );
  }

  Widget _buildVisionBoardSessionItem(VisionBoardSession session) {
    final duration = session.timeSpentInSeconds;
    final minutes = duration ~/ 60;
    final seconds = duration % 60;
    final durationText = minutes > 0 ? '${minutes}m ${seconds}s' : '${seconds}s';

    return Container(
      margin: EdgeInsets.only(bottom: 12.h),
      padding: EdgeInsets.all(16.w),
      decoration: BoxDecoration(
        color: AppColors.cardBackground,
        borderRadius: BorderRadius.circular(12.r),
        border: Border.all(
          color: AppColors.textMedium.withOpacity(0.1),
        ),
      ),
      child: Row(
        children: [
          Container(
            padding: EdgeInsets.all(8.w),
            decoration: BoxDecoration(
              gradient: LinearGradient(colors: AppColors.visionBoardGradient),
              borderRadius: BorderRadius.circular(8.r),
            ),
            child: Icon(
              Icons.dashboard,
              color: AppColors.white,
              size: 16.sp,
            ),
          ),
          SizedBox(width: 12.w),
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  session.title,
                  style: TextStyle(
                    color: AppColors.textLight,
                    fontSize: 14.sp,
                    fontWeight: FontWeight.w500,
                  ),
                ),
                SizedBox(height: 2.h),
                Text(
                  _formatDate(session.createdAt),
                  style: TextStyle(
                    color: AppColors.textMedium,
                    fontSize: 12.sp,
                  ),
                ),
              ],
            ),
          ),
          Text(
            durationText,
            style: TextStyle(
              color: AppColors.textLight,
              fontSize: 14.sp,
              fontWeight: FontWeight.bold,
            ),
          ),
        ],
      ),
    );
  }

  String _formatDate(DateTime date) {
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
}