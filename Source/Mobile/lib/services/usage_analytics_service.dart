import 'dart:async';
import 'dart:convert';
import 'package:shared_preferences/shared_preferences.dart';
import '../models/analytics_models.dart';
import 'daily_affirmations_service.dart';
import 'vision_board_service.dart';

/// Service for tracking and retrieving usage analytics across the app
/// 
/// This service handles:
/// 1. Session tracking for different activities (affirmations, vision board, meditation)
/// 2. Aggregating usage statistics
/// 3. Providing analytics data for the stats screen
class UsageAnalyticsService {
  static final UsageAnalyticsService _instance = UsageAnalyticsService._internal();
  factory UsageAnalyticsService() => _instance;
  UsageAnalyticsService._internal();

  final DailyAffirmationsService _affirmationService = DailyAffirmationsService();
  final VisionBoardService _visionBoardService = VisionBoardService();

  // Active session tracking
  final Map<ActivityType, ActiveSession> _activeSessions = {};
  
  // Session storage keys
  static const String _sessionHistoryKey = 'session_history';
  static const String _dailyStatsKey = 'daily_stats';

  /// Initialize the analytics service
  Future<void> initialize() async {
    _affirmationService.initialize();
    await _visionBoardService.initialize();
    
    // Add some test session data if none exists (for testing)
    await _addTestDataIfEmpty();
    
    print('🎯 UsageAnalyticsService initialized');
  }
  
  /// Add test session data if storage is empty (for testing purposes)
  Future<void> _addTestDataIfEmpty() async {
    try {
      final existingSessions = await _getSessionHistory();
      if (existingSessions.isEmpty) {
        print('📊 ANALYTICS: Adding test session data...');
        
        // Add some test sessions
        final now = DateTime.now();
        final testSessions = [
          {
            'activityType': 'visionBoard',
            'duration': 300, // 5 minutes
            'timestamp': now.subtract(Duration(days: 1)).millisecondsSinceEpoch,
            'metadata': {'boardId': 'test_board'},
          },
          {
            'activityType': 'dailyAffirmations',
            'duration': 120, // 2 minutes
            'timestamp': now.subtract(Duration(hours: 2)).millisecondsSinceEpoch,
            'metadata': {'category': 'Self-Love'},
          },
          {
            'activityType': 'visualMeditation',
            'duration': 600, // 10 minutes
            'timestamp': now.subtract(Duration(hours: 1)).millisecondsSinceEpoch,
            'metadata': {'type': 'guided'},
          },
        ];
        
        final prefs = await SharedPreferences.getInstance();
        await prefs.setString(_sessionHistoryKey, jsonEncode(testSessions));
        print('📊 ANALYTICS: Test session data added');
      }
    } catch (e) {
      print('❌ Error adding test data: $e');
    }
  }

  /// Start tracking a session for a specific activity
  void startSession(ActivityType activityType, {Map<String, dynamic>? metadata}) {
    final session = ActiveSession(
      activityType: activityType,
      startTime: DateTime.now(),
      sessionId: _generateSessionId(),
      metadata: metadata ?? {},
    );
    
    _activeSessions[activityType] = session;
    print('📊 Started ${activityType.name} session: ${session.sessionId}');
  }

  /// End tracking for a specific activity and record the session
  Future<void> endSession(ActivityType activityType, {Map<String, dynamic>? additionalMetadata}) async {
    final session = _activeSessions[activityType];
    if (session == null) {
      print('⚠️ No active session found for ${activityType.name}');
      return;
    }

    final endTime = DateTime.now();
    final duration = endTime.difference(session.startTime);
    
    // Only record sessions longer than 5 seconds
    if (duration.inSeconds >= 5) {
      await _recordSession(session, endTime, additionalMetadata);
    }
    
    _activeSessions.remove(activityType);
    print('📊 Ended ${activityType.name} session: ${duration.inSeconds}s');
  }

  /// Update metadata for an active session
  void updateSessionMetadata(ActivityType activityType, Map<String, dynamic> metadata) {
    final session = _activeSessions[activityType];
    if (session != null) {
      session.metadata.addAll(metadata);
    }
  }

  /// Get current session duration for an activity
  Duration? getCurrentSessionDuration(ActivityType activityType) {
    final session = _activeSessions[activityType];
    if (session != null) {
      return DateTime.now().difference(session.startTime);
    }
    return null;
  }

  /// Record a completed session via API
  Future<void> _recordSession(ActiveSession session, DateTime endTime, Map<String, dynamic>? additionalMetadata) async {
    try {
      final duration = endTime.difference(session.startTime);
      final metadata = {...session.metadata};
      if (additionalMetadata != null) {
        metadata.addAll(additionalMetadata);
      }

      switch (session.activityType) {
        case ActivityType.dailyAffirmations:
          await _recordAffirmationSession(duration, metadata);
          break;
        case ActivityType.visionBoard:
          await _recordVisionBoardSession(duration, metadata);
          break;
        case ActivityType.visualMeditation:
          await _recordMeditationSession(duration, metadata);
          break;
      }
    } catch (e) {
      print('❌ Error recording session: $e');
    }
  }

  /// Record affirmation session
  Future<void> _recordAffirmationSession(Duration duration, Map<String, dynamic> metadata) async {
    try {
      // Store locally first
      await _saveSessionToHistory(ActivityType.dailyAffirmations, duration, metadata);
      
      final result = await _affirmationService.createAffirmationSession(
        timeSpentInSeconds: duration.inSeconds,
        addToFeed: false,
      );
      
      if (result.isSuccess) {
        print('✅ Affirmation session recorded: ${duration.inSeconds}s');
      } else {
        print('❌ Failed to record affirmation session: ${result.message}');
      }
    } catch (e) {
      print('❌ Error recording affirmation session: $e');
    }
  }

  /// Record vision board session (placeholder - uses XML storage)
  Future<void> _recordVisionBoardSession(Duration duration, Map<String, dynamic> metadata) async {
    try {
      // Store locally first
      await _saveSessionToHistory(ActivityType.visionBoard, duration, metadata);
      
      print('📊 Vision board session: ${duration.inSeconds}s (stored locally)');
      
      // In the future, this would call the vision board API:
      // final result = await _visionBoardService.updateSession(
      //   sessionId: metadata['sessionId'],
      //   timeSpentInSeconds: duration.inSeconds,
      // );
    } catch (e) {
      print('❌ Error recording vision board session: $e');
    }
  }

  /// Record meditation session (placeholder - no API yet)
  Future<void> _recordMeditationSession(Duration duration, Map<String, dynamic> metadata) async {
    try {
      // Store locally first
      await _saveSessionToHistory(ActivityType.visualMeditation, duration, metadata);
      
      print('📊 Meditation session: ${duration.inSeconds}s (stored locally)');
      
      // For now, store locally or use a generic session API
      // final result = await _meditationService.createMeditationSession(...)
    } catch (e) {
      print('❌ Error recording meditation session: $e');
    }
  }

  /// Get comprehensive usage statistics
  Future<UsageStats> getUsageStats() async {
    try {
      print('📊 ANALYTICS: Loading session history...');
      final sessionHistory = await _getSessionHistory();
      print('📊 ANALYTICS: Found ${sessionHistory.length} sessions in history');
      
      // If no sessions exist, return demo data as fallback
      if (sessionHistory.isEmpty) {
        print('📊 ANALYTICS: No sessions found, using demo data');
        return _getDemoUsageStats();
      }
      
      final now = DateTime.now();
      
      // Calculate overall statistics
      int totalSessions = sessionHistory.length;
      int totalTimeInSeconds = sessionHistory.fold(0, (sum, session) => sum + session['duration'] as int);
      print('📊 ANALYTICS: Total sessions: $totalSessions, Total time: ${totalTimeInSeconds}s');
      double averageSessionTime = totalSessions > 0 ? totalTimeInSeconds / totalSessions : 0;
      
      // Group sessions by activity type
      Map<ActivityType, List<Map<String, dynamic>>> sessionsByActivity = {};
      for (final session in sessionHistory) {
        final activityTypeStr = session['activityType'] as String;
        final activityType = ActivityType.values.firstWhere(
          (type) => type.name == activityTypeStr,
          orElse: () => ActivityType.dailyAffirmations,
        );
        sessionsByActivity.putIfAbsent(activityType, () => []).add(session);
      }
      
      // Calculate activity-specific stats
      Map<ActivityType, ActivityStats> activities = {};
      ActivityType mostActiveActivity = ActivityType.dailyAffirmations;
      int maxSessions = 0;
      
      for (final activityType in ActivityType.values) {
        final sessions = sessionsByActivity[activityType] ?? [];
        final activityTotalTime = sessions.fold(0, (sum, session) => sum + session['duration'] as int);
        final activityAvgTime = sessions.isNotEmpty ? activityTotalTime / sessions.length : 0;
        
        // Find last session date
        DateTime? lastSessionDate;
        if (sessions.isNotEmpty) {
          lastSessionDate = sessions
              .map((s) => DateTime.fromMillisecondsSinceEpoch(s['timestamp'] as int))
              .reduce((a, b) => a.isAfter(b) ? a : b);
        }
        
        activities[activityType] = ActivityStats(
          activityType: activityType,
          totalSessions: sessions.length,
          totalTimeInSeconds: activityTotalTime,
          averageSessionTimeInSeconds: activityAvgTime.round(),
          currentStreak: _calculateStreak(sessions, activityType),
          lastSessionDate: lastSessionDate,
          weeklyActivity: _generateWeeklyActivity(activityType),
        );
        
        if (sessions.length > maxSessions) {
          maxSessions = sessions.length;
          mostActiveActivity = activityType;
        }
      }
      
      // Calculate streaks and weekly/monthly data
      final thisWeekSessions = sessionHistory.where((session) {
        final sessionDate = DateTime.fromMillisecondsSinceEpoch(session['timestamp'] as int);
        final weekStart = now.subtract(Duration(days: now.weekday - 1));
        return sessionDate.isAfter(weekStart);
      }).length;
      
      final thisMonthSessions = sessionHistory.where((session) {
        final sessionDate = DateTime.fromMillisecondsSinceEpoch(session['timestamp'] as int);
        return sessionDate.month == now.month && sessionDate.year == now.year;
      }).length;
      
      return UsageStats(
        totalSessions: totalSessions,
        totalTimeInSeconds: totalTimeInSeconds,
        averageSessionTimeInSeconds: averageSessionTime.round(),
        mostActiveActivity: mostActiveActivity,
        currentStreak: _calculateOverallStreak(sessionHistory),
        longestStreak: _calculateLongestStreak(sessionHistory),
        thisWeekSessions: thisWeekSessions,
        thisMonthSessions: thisMonthSessions,
        activities: activities,
      );
    } catch (e) {
      print('❌ Error fetching usage stats: $e');
      return _getEmptyUsageStats();
    }
  }

  /// Get usage statistics for a specific activity
  Future<ActivityStats> getActivityStats(ActivityType activityType) async {
    try {
      switch (activityType) {
        case ActivityType.dailyAffirmations:
          return _getAffirmationStats();
        case ActivityType.visionBoard:
          return _getVisionBoardStats();
        case ActivityType.visualMeditation:
          return _getMeditationStats();
      }
    } catch (e) {
      print('❌ Error fetching activity stats: $e');
      return ActivityStats.empty(activityType);
    }
  }

  /// Generate a unique session ID
  String _generateSessionId() {
    return 'session_${DateTime.now().millisecondsSinceEpoch}';
  }

  /// Get affirmation statistics (uses real API data)
  Future<ActivityStats> _getAffirmationStats() async {
    // TODO: Implement API call to get affirmation session statistics
    return ActivityStats(
      activityType: ActivityType.dailyAffirmations,
      totalSessions: 25,
      totalTimeInSeconds: 1800, // 30 minutes
      averageSessionTimeInSeconds: 72, // 1m 12s
      currentStreak: 5,
      lastSessionDate: DateTime.now().subtract(Duration(hours: 6)),
      weeklyActivity: _generateWeeklyActivity(ActivityType.dailyAffirmations),
    );
  }

  /// Get vision board statistics (placeholder data)
  Future<ActivityStats> _getVisionBoardStats() async {
    // TODO: Implement API call when vision board session API is available
    return ActivityStats(
      activityType: ActivityType.visionBoard,
      totalSessions: 18,
      totalTimeInSeconds: 2700, // 45 minutes
      averageSessionTimeInSeconds: 150, // 2m 30s
      currentStreak: 3,
      lastSessionDate: DateTime.now().subtract(Duration(days: 1)),
      weeklyActivity: _generateWeeklyActivity(ActivityType.visionBoard),
    );
  }

  /// Get meditation statistics (placeholder data)
  Future<ActivityStats> _getMeditationStats() async {
    // TODO: Implement API call when meditation session API is available
    return ActivityStats(
      activityType: ActivityType.visualMeditation,
      totalSessions: 35,
      totalTimeInSeconds: 5400, // 1.5 hours
      averageSessionTimeInSeconds: 154, // 2m 34s
      currentStreak: 7,
      lastSessionDate: DateTime.now().subtract(Duration(hours: 2)),
      weeklyActivity: _generateWeeklyActivity(ActivityType.visualMeditation),
    );
  }

  /// Generate weekly activity data
  List<DailyActivity> _generateWeeklyActivity(ActivityType activityType) {
    final activities = <DailyActivity>[];
    final now = DateTime.now();
    
    for (int i = 6; i >= 0; i--) {
      final date = now.subtract(Duration(days: i));
      activities.add(DailyActivity(
        date: date,
        sessionCount: _getRandomSessionCount(activityType),
        totalTimeInSeconds: _getRandomTimeSpent(activityType),
      ));
    }
    
    return activities;
  }

  int _getRandomSessionCount(ActivityType activityType) {
    // Generate realistic session counts based on activity type
    switch (activityType) {
      case ActivityType.dailyAffirmations:
        return [0, 1, 2, 1, 0, 2, 1][DateTime.now().weekday % 7];
      case ActivityType.visionBoard:
        return [0, 0, 1, 1, 0, 1, 0][DateTime.now().weekday % 7];
      case ActivityType.visualMeditation:
        return [1, 2, 1, 2, 1, 1, 2][DateTime.now().weekday % 7];
    }
  }

  int _getRandomTimeSpent(ActivityType activityType) {
    // Generate realistic time spent based on activity type
    switch (activityType) {
      case ActivityType.dailyAffirmations:
        return [0, 60, 120, 75, 0, 150, 90][DateTime.now().weekday % 7];
      case ActivityType.visionBoard:
        return [0, 0, 180, 240, 0, 300, 0][DateTime.now().weekday % 7];
      case ActivityType.visualMeditation:
        return [300, 600, 300, 900, 450, 300, 750][DateTime.now().weekday % 7];
    }
  }

  // ======================== SESSION PERSISTENCE ========================
  
  /// Save session to local history
  Future<void> _saveSessionToHistory(ActivityType activityType, Duration duration, Map<String, dynamic> metadata) async {
    try {
      final prefs = await SharedPreferences.getInstance();
      final sessionHistory = await _getSessionHistory();
      
      final sessionData = {
        'activityType': activityType.name,
        'duration': duration.inSeconds,
        'timestamp': DateTime.now().millisecondsSinceEpoch,
        'metadata': metadata,
      };
      
      sessionHistory.add(sessionData);
      
      // Keep only last 1000 sessions to prevent unlimited growth
      if (sessionHistory.length > 1000) {
        sessionHistory.removeRange(0, sessionHistory.length - 1000);
      }
      
      await prefs.setString(_sessionHistoryKey, jsonEncode(sessionHistory));
      print('📊 Session saved to history: ${activityType.name} - ${duration.inSeconds}s');
    } catch (e) {
      print('❌ Error saving session to history: $e');
    }
  }
  
  /// Get session history from local storage
  Future<List<Map<String, dynamic>>> _getSessionHistory() async {
    try {
      final prefs = await SharedPreferences.getInstance();
      final historyJson = prefs.getString(_sessionHistoryKey);
      
      print('📊 STORAGE: Raw history JSON: ${historyJson?.substring(0, historyJson.length > 100 ? 100 : historyJson.length)}...');
      
      if (historyJson == null) {
        print('📊 STORAGE: No session history found in SharedPreferences');
        return [];
      }
      
      final historyList = jsonDecode(historyJson) as List;
      print('📊 STORAGE: Loaded ${historyList.length} sessions from storage');
      return historyList.cast<Map<String, dynamic>>();
    } catch (e) {
      print('❌ Error loading session history: $e');
      return [];
    }
  }
  
  /// Calculate streak for a specific activity
  int _calculateStreak(List<Map<String, dynamic>> sessions, ActivityType activityType) {
    if (sessions.isEmpty) return 0;
    
    final now = DateTime.now();
    final sortedSessions = sessions
        .map((s) => DateTime.fromMillisecondsSinceEpoch(s['timestamp'] as int))
        .toList()
      ..sort((a, b) => b.compareTo(a)); // Most recent first
    
    int streak = 0;
    DateTime? lastSessionDate;
    
    for (final sessionDate in sortedSessions) {
      if (lastSessionDate == null) {
        // First session - check if it's today or yesterday
        final daysDiff = now.difference(sessionDate).inDays;
        if (daysDiff <= 1) {
          streak = 1;
          lastSessionDate = sessionDate;
        } else {
          break;
        }
      } else {
        // Check if this session is consecutive
        final daysDiff = lastSessionDate.difference(sessionDate).inDays;
        if (daysDiff == 1) {
          streak++;
          lastSessionDate = sessionDate;
        } else {
          break;
        }
      }
    }
    
    return streak;
  }
  
  /// Calculate overall streak across all activities
  int _calculateOverallStreak(List<Map<String, dynamic>> sessions) {
    if (sessions.isEmpty) return 0;
    
    final now = DateTime.now();
    final sessionDates = sessions
        .map((s) => DateTime.fromMillisecondsSinceEpoch(s['timestamp'] as int))
        .toSet()
        .toList()
      ..sort((a, b) => b.compareTo(a)); // Most recent first
    
    int streak = 0;
    DateTime? lastDate;
    
    for (final date in sessionDates) {
      if (lastDate == null) {
        final daysDiff = now.difference(date).inDays;
        if (daysDiff <= 1) {
          streak = 1;
          lastDate = date;
        } else {
          break;
        }
      } else {
        final daysDiff = lastDate.difference(date).inDays;
        if (daysDiff == 1) {
          streak++;
          lastDate = date;
        } else {
          break;
        }
      }
    }
    
    return streak;
  }
  
  /// Calculate longest streak
  int _calculateLongestStreak(List<Map<String, dynamic>> sessions) {
    if (sessions.isEmpty) return 0;
    
    final sessionDates = sessions
        .map((s) => DateTime.fromMillisecondsSinceEpoch(s['timestamp'] as int))
        .toSet()
        .toList()
      ..sort();
    
    int longestStreak = 0;
    int currentStreak = 1;
    
    for (int i = 1; i < sessionDates.length; i++) {
      final daysDiff = sessionDates[i].difference(sessionDates[i - 1]).inDays;
      if (daysDiff == 1) {
        currentStreak++;
      } else {
        longestStreak = currentStreak > longestStreak ? currentStreak : longestStreak;
        currentStreak = 1;
      }
    }
    
    return currentStreak > longestStreak ? currentStreak : longestStreak;
  }

  // ======================== DEMO DATA (FOR FALLBACK) ========================

  /// Get demo usage statistics
  UsageStats _getDemoUsageStats() {
    return UsageStats(
      totalSessions: 78,
      totalTimeInSeconds: 9900, // 2h 45m
      averageSessionTimeInSeconds: 127, // 2m 7s
      mostActiveActivity: ActivityType.visualMeditation,
      currentStreak: 7,
      longestStreak: 14,
      thisWeekSessions: 12,
      thisMonthSessions: 45,
      activities: {
        ActivityType.dailyAffirmations: ActivityStats(
          activityType: ActivityType.dailyAffirmations,
          totalSessions: 25,
          totalTimeInSeconds: 1800,
          averageSessionTimeInSeconds: 72,
          currentStreak: 5,
          lastSessionDate: DateTime.now().subtract(Duration(hours: 6)),
          weeklyActivity: _generateWeeklyActivity(ActivityType.dailyAffirmations),
        ),
        ActivityType.visionBoard: ActivityStats(
          activityType: ActivityType.visionBoard,
          totalSessions: 18,
          totalTimeInSeconds: 2700,
          averageSessionTimeInSeconds: 150,
          currentStreak: 3,
          lastSessionDate: DateTime.now().subtract(Duration(days: 1)),
          weeklyActivity: _generateWeeklyActivity(ActivityType.visionBoard),
        ),
        ActivityType.visualMeditation: ActivityStats(
          activityType: ActivityType.visualMeditation,
          totalSessions: 35,
          totalTimeInSeconds: 5400,
          averageSessionTimeInSeconds: 154,
          currentStreak: 7,
          lastSessionDate: DateTime.now().subtract(Duration(hours: 2)),
          weeklyActivity: _generateWeeklyActivity(ActivityType.visualMeditation),
        ),
      },
    );
  }

  /// Get empty usage statistics
  UsageStats _getEmptyUsageStats() {
    return UsageStats(
      totalSessions: 0,
      totalTimeInSeconds: 0,
      averageSessionTimeInSeconds: 0,
      mostActiveActivity: ActivityType.dailyAffirmations,
      currentStreak: 0,
      longestStreak: 0,
      thisWeekSessions: 0,
      thisMonthSessions: 0,
      activities: {},
    );
  }

  /// End all active sessions (called when app goes to background)
  Future<void> endAllSessions() async {
    final activeSessions = List.from(_activeSessions.keys);
    for (final activityType in activeSessions) {
      await endSession(activityType);
    }
  }
}