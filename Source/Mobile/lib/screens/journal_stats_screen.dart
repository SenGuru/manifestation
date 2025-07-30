import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../models/journey_entry.dart';
import '../constants/app_theme.dart';

class JournalStatsScreen extends StatelessWidget {
  final Map<String, JournalEntry> entries;

  const JournalStatsScreen({
    Key? key,
    required this.entries,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    final stats = _calculateStats();
    
    return Scaffold(
      backgroundColor: AppTheme.backgroundDark,
      body: SafeArea(
        child: Column(
          children: [
            _buildHeader(context),
            Expanded(
              child: SingleChildScrollView(
                padding: EdgeInsets.all(20.w),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    _buildOverviewCards(stats),
                    SizedBox(height: 24.h),
                    _buildMoodChart(stats),
                    SizedBox(height: 24.h),
                    _buildStreakSection(stats),
                    SizedBox(height: 24.h),
                    _buildRecentActivity(),
                  ],
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildHeader(BuildContext context) {
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
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  'Journal Analytics',
                  style: TextStyle(
                    fontSize: 24.sp,
                    fontWeight: FontWeight.bold,
                    color: AppTheme.textLight,
                  ),
                ),
                Text(
                  'Your manifestation insights',
                  style: TextStyle(
                    fontSize: 14.sp,
                    color: AppTheme.textMedium,
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildOverviewCards(JournalStats stats) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Overview',
          style: TextStyle(
            fontSize: 18.sp,
            fontWeight: FontWeight.w600,
            color: AppTheme.textLight,
          ),
        ),
        SizedBox(height: 12.h),
        Row(
          children: [
            Expanded(
              child: _buildStatCard(
                'Total Entries',
                '${stats.totalEntries}',
                Icons.book,
                AppTheme.journalGradient[0],
              ),
            ),
            SizedBox(width: 12.w),
            Expanded(
              child: _buildStatCard(
                'Current Streak',
                '${stats.currentStreak} days',
                Icons.local_fire_department,
                AppTheme.accentGold,
              ),
            ),
          ],
        ),
        SizedBox(height: 12.h),
        Row(
          children: [
            Expanded(
              child: _buildStatCard(
                'This Month',
                '${stats.thisMonthEntries}',
                Icons.calendar_month,
                AppTheme.affirmationGradient[0],
              ),
            ),
            SizedBox(width: 12.w),
            Expanded(
              child: _buildStatCard(
                'Avg Gratitudes',
                '${stats.avgGratitudes.toStringAsFixed(1)}',
                Icons.favorite,
                AppTheme.visionBoardGradient[0],
              ),
            ),
          ],
        ),
      ],
    );
  }

  Widget _buildStatCard(String label, String value, IconData icon, Color color) {
    return Container(
      padding: EdgeInsets.all(16.w),
      decoration: BoxDecoration(
        color: AppTheme.cardBackground,
        borderRadius: BorderRadius.circular(16.r),
        border: Border.all(
          color: color.withOpacity(0.3),
        ),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            children: [
              Icon(
                icon,
                color: color,
                size: 20.sp,
              ),
              SizedBox(width: 8.w),
              Text(
                label,
                style: TextStyle(
                  fontSize: 12.sp,
                  fontWeight: FontWeight.w500,
                  color: AppTheme.textMedium,
                ),
              ),
            ],
          ),
          SizedBox(height: 8.h),
          Text(
            value,
            style: TextStyle(
              fontSize: 20.sp,
              fontWeight: FontWeight.bold,
              color: AppTheme.textLight,
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildMoodChart(JournalStats stats) {
    return Container(
      width: double.infinity,
      padding: EdgeInsets.all(20.w),
      decoration: BoxDecoration(
        color: AppTheme.cardBackground,
        borderRadius: BorderRadius.circular(16.r),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(
            'Mood Distribution',
            style: TextStyle(
              fontSize: 18.sp,
              fontWeight: FontWeight.w600,
              color: AppTheme.textLight,
            ),
          ),
          SizedBox(height: 16.h),
          ...stats.moodDistribution.entries.map((entry) {
            final mood = entry.key;
            final count = entry.value;
            final percentage = (count / stats.totalEntries * 100).round();
            
            return Container(
              margin: EdgeInsets.only(bottom: 12.h),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Row(
                    children: [
                      Icon(
                        mood.icon,
                        color: mood.color,
                        size: 16.sp,
                      ),
                      SizedBox(width: 8.w),
                      Text(
                        mood.displayName,
                        style: TextStyle(
                          fontSize: 14.sp,
                          fontWeight: FontWeight.w500,
                          color: AppTheme.textLight,
                        ),
                      ),
                      Spacer(),
                      Text(
                        '$count ($percentage%)',
                        style: TextStyle(
                          fontSize: 12.sp,
                          color: AppTheme.textMedium,
                        ),
                      ),
                    ],
                  ),
                  SizedBox(height: 6.h),
                  LinearProgressIndicator(
                    value: count / stats.totalEntries,
                    backgroundColor: AppTheme.backgroundDark,
                    valueColor: AlwaysStoppedAnimation<Color>(mood.color),
                    minHeight: 4.h,
                  ),
                ],
              ),
            );
          }).toList(),
        ],
      ),
    );
  }

  Widget _buildStreakSection(JournalStats stats) {
    return Container(
      width: double.infinity,
      padding: EdgeInsets.all(20.w),
      decoration: BoxDecoration(
        color: AppTheme.cardBackground,
        borderRadius: BorderRadius.circular(16.r),
        border: Border.all(
          color: AppTheme.accentGold.withOpacity(0.3),
        ),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            children: [
              Icon(
                Icons.local_fire_department,
                color: AppTheme.accentGold,
                size: 20.sp,
              ),
              SizedBox(width: 8.w),
              Text(
                'Streak Information',
                style: TextStyle(
                  fontSize: 18.sp,
                  fontWeight: FontWeight.w600,
                  color: AppTheme.textLight,
                ),
              ),
            ],
          ),
          SizedBox(height: 16.h),
          Row(
            children: [
              Expanded(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      'Current Streak',
                      style: TextStyle(
                        fontSize: 12.sp,
                        color: AppTheme.textMedium,
                      ),
                    ),
                    SizedBox(height: 4.h),
                    Text(
                      '${stats.currentStreak} days',
                      style: TextStyle(
                        fontSize: 24.sp,
                        fontWeight: FontWeight.bold,
                        color: AppTheme.accentGold,
                      ),
                    ),
                  ],
                ),
              ),
              Expanded(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      'Best Streak',
                      style: TextStyle(
                        fontSize: 12.sp,
                        color: AppTheme.textMedium,
                      ),
                    ),
                    SizedBox(height: 4.h),
                    Text(
                      '${stats.bestStreak} days',
                      style: TextStyle(
                        fontSize: 24.sp,
                        fontWeight: FontWeight.bold,
                        color: AppTheme.textLight,
                      ),
                    ),
                  ],
                ),
              ),
            ],
          ),
          if (stats.currentStreak > 0) ...[
            SizedBox(height: 12.h),
            Container(
              width: double.infinity,
              padding: EdgeInsets.all(12.w),
              decoration: BoxDecoration(
                color: AppTheme.accentGold.withOpacity(0.1),
                borderRadius: BorderRadius.circular(12.r),
              ),
              child: Row(
                children: [
                  Icon(
                    Icons.celebration,
                    color: AppTheme.accentGold,
                    size: 16.sp,
                  ),
                  SizedBox(width: 8.w),
                  Text(
                    'Keep it up! You\'re on a roll!',
                    style: TextStyle(
                      fontSize: 12.sp,
                      fontWeight: FontWeight.w500,
                      color: AppTheme.accentGold,
                    ),
                  ),
                ],
              ),
            ),
          ],
        ],
      ),
    );
  }

  Widget _buildRecentActivity() {
    final recentEntries = entries.values
        .toList()
        ..sort((a, b) => b.date.compareTo(a.date));
    
    return Container(
      width: double.infinity,
      padding: EdgeInsets.all(20.w),
      decoration: BoxDecoration(
        color: AppTheme.cardBackground,
        borderRadius: BorderRadius.circular(16.r),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(
            'Recent Activity',
            style: TextStyle(
              fontSize: 18.sp,
              fontWeight: FontWeight.w600,
              color: AppTheme.textLight,
            ),
          ),
          SizedBox(height: 16.h),
          ...recentEntries.take(3).map((entry) {
            return Container(
              margin: EdgeInsets.only(bottom: 12.h),
              padding: EdgeInsets.all(12.w),
              decoration: BoxDecoration(
                color: AppTheme.backgroundDark,
                borderRadius: BorderRadius.circular(12.r),
              ),
              child: Row(
                children: [
                  Icon(
                    entry.mood.icon,
                    color: entry.mood.color,
                    size: 20.sp,
                  ),
                  SizedBox(width: 12.w),
                  Expanded(
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text(
                          _formatDate(entry.date),
                          style: TextStyle(
                            fontSize: 14.sp,
                            fontWeight: FontWeight.w500,
                            color: AppTheme.textLight,
                          ),
                        ),
                        SizedBox(height: 2.h),
                        Text(
                          '${entry.gratitudes.length} gratitudes • ${entry.mood.displayName}',
                          style: TextStyle(
                            fontSize: 12.sp,
                            color: AppTheme.textMedium,
                          ),
                        ),
                      ],
                    ),
                  ),
                ],
              ),
            );
          }).toList(),
        ],
      ),
    );
  }

  JournalStats _calculateStats() {
    final now = DateTime.now();
    final thisMonth = entries.values
        .where((entry) => entry.date.year == now.year && entry.date.month == now.month)
        .length;
    
    final moodCounts = <Mood, int>{};
    var totalGratitudes = 0;
    
    for (final entry in entries.values) {
      moodCounts[entry.mood] = (moodCounts[entry.mood] ?? 0) + 1;
      totalGratitudes += entry.gratitudes.length;
    }
    
    final avgGratitudes = entries.isNotEmpty ? totalGratitudes / entries.length : 0.0;
    
    // Calculate streaks
    final sortedDates = entries.keys.toList()..sort();
    var currentStreak = 0;
    var bestStreak = 0;
    var tempStreak = 0;
    
    final today = DateTime.now();
    final yesterday = today.subtract(Duration(days: 1));
    
    // Check if there's an entry for today or yesterday to start counting
    final todayKey = _formatDateKey(today);
    final yesterdayKey = _formatDateKey(yesterday);
    
    if (entries.containsKey(todayKey) || entries.containsKey(yesterdayKey)) {
      var checkDate = entries.containsKey(todayKey) ? today : yesterday;
      
      while (entries.containsKey(_formatDateKey(checkDate))) {
        currentStreak++;
        checkDate = checkDate.subtract(Duration(days: 1));
      }
    }
    
    // Calculate best streak
    DateTime? previousDate;
    for (final dateKey in sortedDates) {
      final date = DateTime.parse(dateKey);
      
      if (previousDate != null && 
          date.difference(previousDate).inDays == 1) {
        tempStreak++;
      } else {
        tempStreak = 1;
      }
      
      if (tempStreak > bestStreak) {
        bestStreak = tempStreak;
      }
      
      previousDate = date;
    }
    
    return JournalStats(
      totalEntries: entries.length,
      thisMonthEntries: thisMonth,
      currentStreak: currentStreak,
      bestStreak: bestStreak,
      avgGratitudes: avgGratitudes,
      moodDistribution: moodCounts,
    );
  }

  String _formatDateKey(DateTime date) {
    return '${date.year}-${date.month.toString().padLeft(2, '0')}-${date.day.toString().padLeft(2, '0')}';
  }

  String _formatDate(DateTime date) {
    final months = [
      'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
      'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'
    ];
    return '${months[date.month - 1]} ${date.day}';
  }
}

class JournalStats {
  final int totalEntries;
  final int thisMonthEntries;
  final int currentStreak;
  final int bestStreak;
  final double avgGratitudes;
  final Map<Mood, int> moodDistribution;

  JournalStats({
    required this.totalEntries,
    required this.thisMonthEntries,
    required this.currentStreak,
    required this.bestStreak,
    required this.avgGratitudes,
    required this.moodDistribution,
  });
}