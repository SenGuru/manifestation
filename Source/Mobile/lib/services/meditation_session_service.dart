import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:manifestation_app/services/session_service.dart';
import 'package:shared_preferences/shared_preferences.dart';

class MeditationSessionService {
  static const String _baseUrl = 'https://api.manifestation.com/api/v1/manifest';
  static const String _localSessionsKey = 'local_meditation_sessions';

  static MeditationSessionService? _instance;
  static MeditationSessionService get instance {
    _instance ??= MeditationSessionService._();
    return _instance!;
  }

  MeditationSessionService._();

  /// Create a new meditation session
  Future<bool> createMeditationSession({
    required DateTime sessionDate,
    required int timeSpentInSeconds,
    int? meditationFileId,
    bool addToFeed = false,
  }) async {
    try {
      print('Creating meditation session: ${timeSpentInSeconds}s on ${sessionDate.toIso8601String()}');
      
      final userId = await SessionService.instance.getUserId();
      print('User ID: $userId');
      
      if (userId == null) {
        print('No user ID, storing session locally');
        return await _storeSessionLocally(sessionDate, timeSpentInSeconds);
      }

      final token = await SessionService.instance.getSessionToken();
      print('Token available: ${token != null ? 'yes' : 'no'}');
      
      if (token == null) {
        print('No token, storing session locally');
        return await _storeSessionLocally(sessionDate, timeSpentInSeconds);
      }

      print('Making API call to create session...');
      
      final response = await http.post(
        Uri.parse('$_baseUrl/createmeditationsession'),
        headers: {
          'Content-Type': 'application/json',
          'Authorization': 'Bearer $token',
        },
        body: jsonEncode({
          'meditationSessionDate': sessionDate.toIso8601String(),
          'meditationSessionTimeSpentInSeconds': timeSpentInSeconds,
          'meditationFileId': meditationFileId ?? 1,
          'userId': userId,
          'addedToFeed': addToFeed,
          'addedToFeedDate': addToFeed ? DateTime.now().toIso8601String() : null,
        }),
      );

      print('API response: ${response.statusCode} - ${response.body}');

      if (response.statusCode == 200) {
        print('Meditation session created successfully via API');
        return true;
      } else {
        print('API call failed, storing session locally');
        return await _storeSessionLocally(sessionDate, timeSpentInSeconds);
      }
    } catch (e) {
      return await _storeSessionLocally(sessionDate, timeSpentInSeconds);
    }
  }

  /// Get user's meditation sessions
  Future<List<MeditationSession>> getUserMeditationSessions() async {
    try {
      print('Getting user meditation sessions...');
      
      final userId = await SessionService.instance.getUserId();
      final token = await SessionService.instance.getSessionToken();
      
      print('Retrieving sessions - User ID: $userId, Token available: ${token != null}');

      if (userId == null || token == null) {
        print('No user ID or token, getting local sessions');
        return await _getLocalSessions();
      }

      print('Making API call to get sessions...');
      
      final response = await http.get(
        Uri.parse('$_baseUrl/getmeditationsessionbyuserid?userId=$userId'),
        headers: {
          'Authorization': 'Bearer $token',
        },
      );

      print('Get sessions API response: ${response.statusCode} - ${response.body}');

      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);
        final List<dynamic> sessions = data['data'] ?? [];
        
        print('Found ${sessions.length} sessions from API');
        
        return sessions.map((session) => MeditationSession.fromJson(session)).toList();
      } else {
        print('API call failed, getting local sessions');
        return await _getLocalSessions();
      }
    } catch (e) {
      return await _getLocalSessions();
    }
  }

  /// Get meditation session statistics
  Future<MeditationStats> getMeditationStats() async {
    print('Getting meditation stats...');
    
    final sessions = await getUserMeditationSessions();
    
    print('Sessions for stats calculation: ${sessions.length}');
    for (int i = 0; i < sessions.length; i++) {
      print('Session $i: ${sessions[i].timeSpentInSeconds}s on ${sessions[i].sessionDate}');
    }
    
    if (sessions.isEmpty) {
      print('No sessions found, returning empty stats');
      return MeditationStats(
        totalSessions: 0,
        totalTimeInSeconds: 0,
        averageSessionTime: 0,
        currentStreak: 0,
        longestStreak: 0,
        thisWeekSessions: 0,
        thisMonthSessions: 0,
      );
    }

    final totalSessions = sessions.length;
    final totalTimeInSeconds = sessions.fold<int>(
      0, 
      (sum, session) => sum + (session.timeSpentInSeconds ?? 0),
    );
    final averageSessionTime = totalTimeInSeconds / totalSessions;

    // Calculate this week/month sessions
    final now = DateTime.now();
    final weekAgo = now.subtract(Duration(days: 7));
    final monthAgo = DateTime(now.year, now.month - 1, now.day);

    final thisWeekSessions = sessions.where((s) => s.sessionDate.isAfter(weekAgo)).length;
    final thisMonthSessions = sessions.where((s) => s.sessionDate.isAfter(monthAgo)).length;

    return MeditationStats(
      totalSessions: totalSessions,
      totalTimeInSeconds: totalTimeInSeconds,
      averageSessionTime: averageSessionTime.round(),
      currentStreak: 0, // Simplified for now
      longestStreak: 0, // Simplified for now
      thisWeekSessions: thisWeekSessions,
      thisMonthSessions: thisMonthSessions,
    );
  }

  /// Store session locally when offline
  Future<bool> _storeSessionLocally(DateTime sessionDate, int timeSpentInSeconds) async {
    try {
      final prefs = await SharedPreferences.getInstance();
      final existingSessions = prefs.getStringList(_localSessionsKey) ?? [];
      
      final newSession = {
        'sessionDate': sessionDate.toIso8601String(),
        'timeSpentInSeconds': timeSpentInSeconds,
        'isLocal': true,
      };
      
      existingSessions.add(jsonEncode(newSession));
      await prefs.setStringList(_localSessionsKey, existingSessions);
      
      return true;
    } catch (e) {
      return false;
    }
  }

  /// Get local sessions
  Future<List<MeditationSession>> _getLocalSessions() async {
    try {
      print('Getting local sessions...');
      
      final prefs = await SharedPreferences.getInstance();
      final localSessions = prefs.getStringList(_localSessionsKey) ?? [];
      
      print('Found ${localSessions.length} local sessions');
      
      return localSessions.map((sessionString) {
        final sessionData = jsonDecode(sessionString);
        return MeditationSession(
          sessionId: 0,
          sessionDate: DateTime.parse(sessionData['sessionDate']),
          timeSpentInSeconds: sessionData['timeSpentInSeconds'],
          userId: 0,
          isLocal: true,
        );
      }).toList();
    } catch (e) {
      print('Error getting local sessions: $e');
      return [];
    }
  }
}

/// Meditation session data model
class MeditationSession {
  final int sessionId;
  final DateTime sessionDate;
  final int? timeSpentInSeconds;
  final int userId;
  final bool isLocal;

  MeditationSession({
    required this.sessionId,
    required this.sessionDate,
    this.timeSpentInSeconds,
    required this.userId,
    this.isLocal = false,
  });

  factory MeditationSession.fromJson(Map<String, dynamic> json) {
    return MeditationSession(
      sessionId: json['meditationSessionId'] ?? 0,
      sessionDate: DateTime.parse(json['meditationSessionDate']),
      timeSpentInSeconds: json['meditationSessionTimeSpentInSeconds'],
      userId: json['userId'] ?? 0,
    );
  }
}

/// Meditation statistics model
class MeditationStats {
  final int totalSessions;
  final int totalTimeInSeconds;
  final int averageSessionTime;
  final int currentStreak;
  final int longestStreak;
  final int thisWeekSessions;
  final int thisMonthSessions;

  MeditationStats({
    required this.totalSessions,
    required this.totalTimeInSeconds,
    required this.averageSessionTime,
    required this.currentStreak,
    required this.longestStreak,
    required this.thisWeekSessions,
    required this.thisMonthSessions,
  });

  String get totalTimeFormatted {
    final hours = totalTimeInSeconds ~/ 3600;
    final minutes = (totalTimeInSeconds % 3600) ~/ 60;
    
    if (hours > 0) {
      return '${hours}h ${minutes}m';
    } else {
      return '${minutes}m';
    }
  }

  String get averageTimeFormatted {
    final minutes = averageSessionTime ~/ 60;
    final seconds = averageSessionTime % 60;
    
    if (minutes > 0) {
      return '${minutes}m ${seconds}s';
    } else {
      return '${seconds}s';
    }
  }
}