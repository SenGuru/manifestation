import 'package:flutter/material.dart';

/// Enum for different activity types that can be tracked
enum ActivityType {
  dailyAffirmations,
  visionBoard,
  visualMeditation,
}

extension ActivityTypeExtension on ActivityType {
  String get name {
    switch (this) {
      case ActivityType.dailyAffirmations:
        return 'Daily Affirmations';
      case ActivityType.visionBoard:
        return 'Vision Board';
      case ActivityType.visualMeditation:
        return 'Visual Meditation';
    }
  }

  String get shortName {
    switch (this) {
      case ActivityType.dailyAffirmations:
        return 'Affirmations';
      case ActivityType.visionBoard:
        return 'Vision Board';
      case ActivityType.visualMeditation:
        return 'Meditation';
    }
  }

  IconData get icon {
    switch (this) {
      case ActivityType.dailyAffirmations:
        return Icons.record_voice_over;
      case ActivityType.visionBoard:
        return Icons.dashboard;
      case ActivityType.visualMeditation:
        return Icons.self_improvement;
    }
  }

  List<Color> get gradientColors {
    switch (this) {
      case ActivityType.dailyAffirmations:
        return [Color(0xFF667eea), Color(0xFF764ba2)];
      case ActivityType.visionBoard:
        return [Color(0xFF4facfe), Color(0xFF00f2fe)];
      case ActivityType.visualMeditation:
        return [Color(0xFFa8edea), Color(0xFFfed6e3)];
    }
  }

  Color get primaryColor {
    return gradientColors.first;
  }
}

/// Model for tracking an active session
class ActiveSession {
  final ActivityType activityType;
  final DateTime startTime;
  final String sessionId;
  final Map<String, dynamic> metadata;

  ActiveSession({
    required this.activityType,
    required this.startTime,
    required this.sessionId,
    required this.metadata,
  });

  Duration get currentDuration => DateTime.now().difference(startTime);
}

/// Model for daily activity data
class DailyActivity {
  final DateTime date;
  final int sessionCount;
  final int totalTimeInSeconds;

  DailyActivity({
    required this.date,
    required this.sessionCount,
    required this.totalTimeInSeconds,
  });

  String get dayName {
    const dayNames = ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'];
    return dayNames[date.weekday - 1];
  }

  int get timeInMinutes => totalTimeInSeconds ~/ 60;

  bool get hasActivity => sessionCount > 0;
}

/// Model for activity-specific statistics
class ActivityStats {
  final ActivityType activityType;
  final int totalSessions;
  final int totalTimeInSeconds;
  final int averageSessionTimeInSeconds;
  final int currentStreak;
  final DateTime? lastSessionDate;
  final List<DailyActivity> weeklyActivity;

  ActivityStats({
    required this.activityType,
    required this.totalSessions,
    required this.totalTimeInSeconds,
    required this.averageSessionTimeInSeconds,
    required this.currentStreak,
    this.lastSessionDate,
    required this.weeklyActivity,
  });

  String get formattedTotalTime => _formatDuration(totalTimeInSeconds);
  String get formattedAverageTime => _formatDuration(averageSessionTimeInSeconds);

  double get percentage {
    // This would be calculated based on total app usage when available
    return 0.0;
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

  static ActivityStats empty(ActivityType activityType) {
    return ActivityStats(
      activityType: activityType,
      totalSessions: 0,
      totalTimeInSeconds: 0,
      averageSessionTimeInSeconds: 0,
      currentStreak: 0,
      lastSessionDate: null,
      weeklyActivity: [],
    );
  }

  String formatLastSession() {
    if (lastSessionDate == null) return 'Never';
    
    final now = DateTime.now();
    final difference = now.difference(lastSessionDate!);
    
    if (difference.inDays == 0) {
      return 'Today';
    } else if (difference.inDays == 1) {
      return 'Yesterday';
    } else if (difference.inDays < 7) {
      return '${difference.inDays} days ago';
    } else {
      return '${lastSessionDate!.day}/${lastSessionDate!.month}/${lastSessionDate!.year}';
    }
  }
}

/// Model for overall usage statistics
class UsageStats {
  final int totalSessions;
  final int totalTimeInSeconds;
  final int averageSessionTimeInSeconds;
  final ActivityType mostActiveActivity;
  final int currentStreak;
  final int longestStreak;
  final int thisWeekSessions;
  final int thisMonthSessions;
  final Map<ActivityType, ActivityStats> activities;

  UsageStats({
    required this.totalSessions,
    required this.totalTimeInSeconds,
    required this.averageSessionTimeInSeconds,
    required this.mostActiveActivity,
    required this.currentStreak,
    required this.longestStreak,
    required this.thisWeekSessions,
    required this.thisMonthSessions,
    required this.activities,
  });

  String get formattedTotalTime => _formatDuration(totalTimeInSeconds);
  String get formattedAverageTime => _formatDuration(averageSessionTimeInSeconds);

  ActivityStats? getActivityStats(ActivityType activityType) {
    return activities[activityType];
  }

  List<DailyActivity> get combinedWeeklyActivity {
    // Combine weekly activity from all activities
    final Map<DateTime, DailyActivity> combinedData = {};
    
    for (final activityStats in activities.values) {
      for (final dailyActivity in activityStats.weeklyActivity) {
        final dateKey = DateTime(dailyActivity.date.year, dailyActivity.date.month, dailyActivity.date.day);
        
        if (combinedData.containsKey(dateKey)) {
          final existing = combinedData[dateKey]!;
          combinedData[dateKey] = DailyActivity(
            date: dateKey,
            sessionCount: existing.sessionCount + dailyActivity.sessionCount,
            totalTimeInSeconds: existing.totalTimeInSeconds + dailyActivity.totalTimeInSeconds,
          );
        } else {
          combinedData[dateKey] = DailyActivity(
            date: dateKey,
            sessionCount: dailyActivity.sessionCount,
            totalTimeInSeconds: dailyActivity.totalTimeInSeconds,
          );
        }
      }
    }
    
    final sortedEntries = combinedData.entries.toList()
      ..sort((a, b) => a.key.compareTo(b.key));
    
    return sortedEntries.map((entry) => entry.value).toList();
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

  double get consistencyRate {
    if (totalSessions == 0) return 0.0;
    return (thisMonthSessions / 30.0 * 100).clamp(0.0, 100.0);
  }

  String get favoriteTime {
    // This would be calculated from actual session times
    // For now, return a placeholder
    return 'Morning';
  }
}

/// Result wrapper for analytics operations
class AnalyticsResult<T> {
  final bool isSuccess;
  final T? data;
  final String message;

  const AnalyticsResult._({
    required this.isSuccess,
    this.data,
    required this.message,
  });

  factory AnalyticsResult.success({T? data, required String message}) {
    return AnalyticsResult._(
      isSuccess: true,
      data: data,
      message: message,
    );
  }

  factory AnalyticsResult.failure({required String message}) {
    return AnalyticsResult._(
      isSuccess: false,
      message: message,
    );
  }
}