import 'package:flutter/material.dart';
import '../colors.dart';

class ManifestationUser {
  final String id;
  final String username;
  final String displayName;
  final String? avatarUrl;
  final String bio;
  final DateTime joinedDate;
  final UserStats stats;
  final List<String> badges;
  final bool isFollowing;
  final bool isFollowedBy;

  const ManifestationUser({
    required this.id,
    required this.username,
    required this.displayName,
    this.avatarUrl,
    required this.bio,
    required this.joinedDate,
    required this.stats,
    required this.badges,
    this.isFollowing = false,
    this.isFollowedBy = false,
  });

  ManifestationUser copyWith({
    String? id,
    String? username,
    String? displayName,
    String? avatarUrl,
    String? bio,
    DateTime? joinedDate,
    UserStats? stats,
    List<String>? badges,
    bool? isFollowing,
    bool? isFollowedBy,
  }) {
    return ManifestationUser(
      id: id ?? this.id,
      username: username ?? this.username,
      displayName: displayName ?? this.displayName,
      avatarUrl: avatarUrl ?? this.avatarUrl,
      bio: bio ?? this.bio,
      joinedDate: joinedDate ?? this.joinedDate,
      stats: stats ?? this.stats,
      badges: badges ?? this.badges,
      isFollowing: isFollowing ?? this.isFollowing,
      isFollowedBy: isFollowedBy ?? this.isFollowedBy,
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'username': username,
      'displayName': displayName,
      'avatarUrl': avatarUrl,
      'bio': bio,
      'joinedDate': joinedDate.toIso8601String(),
      'stats': stats.toJson(),
      'badges': badges,
      'isFollowing': isFollowing,
      'isFollowedBy': isFollowedBy,
    };
  }

  factory ManifestationUser.fromJson(Map<String, dynamic> json) {
    return ManifestationUser(
      id: json['id'],
      username: json['username'],
      displayName: json['displayName'],
      avatarUrl: json['avatarUrl'],
      bio: json['bio'],
      joinedDate: DateTime.parse(json['joinedDate']),
      stats: UserStats.fromJson(json['stats']),
      badges: List<String>.from(json['badges']),
      isFollowing: json['isFollowing'] ?? false,
      isFollowedBy: json['isFollowedBy'] ?? false,
    );
  }
}

class UserStats {
  final int meditationMinutes;
  final int affirmationsListened;
  final int journalEntries;
  final int goalsAchieved;
  final int visionBoardsCreated;
  final int currentStreak;
  final int longestStreak;
  final DateTime lastActive;

  const UserStats({
    required this.meditationMinutes,
    required this.affirmationsListened,
    required this.journalEntries,
    required this.goalsAchieved,
    required this.visionBoardsCreated,
    required this.currentStreak,
    required this.longestStreak,
    required this.lastActive,
  });

  Map<String, dynamic> toJson() {
    return {
      'meditationMinutes': meditationMinutes,
      'affirmationsListened': affirmationsListened,
      'journalEntries': journalEntries,
      'goalsAchieved': goalsAchieved,
      'visionBoardsCreated': visionBoardsCreated,
      'currentStreak': currentStreak,
      'longestStreak': longestStreak,
      'lastActive': lastActive.toIso8601String(),
    };
  }

  factory UserStats.fromJson(Map<String, dynamic> json) {
    return UserStats(
      meditationMinutes: json['meditationMinutes'],
      affirmationsListened: json['affirmationsListened'],
      journalEntries: json['journalEntries'],
      goalsAchieved: json['goalsAchieved'],
      visionBoardsCreated: json['visionBoardsCreated'],
      currentStreak: json['currentStreak'],
      longestStreak: json['longestStreak'],
      lastActive: DateTime.parse(json['lastActive']),
    );
  }

  int get totalActivities => 
      affirmationsListened + journalEntries + goalsAchieved + visionBoardsCreated;

  String get level {
    if (totalActivities < 10) return 'Beginner';
    if (totalActivities < 50) return 'Explorer';
    if (totalActivities < 100) return 'Practitioner';
    if (totalActivities < 250) return 'Adept';
    if (totalActivities < 500) return 'Master';
    return 'Sage';
  }

  Color get levelColor {
    switch (level) {
      case 'Beginner':
        return AppColors.textMedium;
      case 'Explorer':
        return AppColors.accentMint;
      case 'Practitioner':
        return AppColors.secondaryTeal;
      case 'Adept':
        return AppColors.accentGold;
      case 'Master':
        return AppColors.gradientPurple;
      case 'Sage':
        return AppColors.gradientPink;
      default:
        return AppColors.textMedium;
    }
  }
}

class Achievement {
  final String id;
  final String title;
  final String description;
  final IconData icon;
  final Color color;
  final AchievementCategory category;
  final DateTime unlockedAt;
  final int value; // For stats like "20 minutes", "5 goals", etc.
  final String? shareMessage;

  const Achievement({
    required this.id,
    required this.title,
    required this.description,
    required this.icon,
    required this.color,
    required this.category,
    required this.unlockedAt,
    required this.value,
    this.shareMessage,
  });

  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'title': title,
      'description': description,
      'icon': icon.codePoint,
      'color': color.value,
      'category': category.index,
      'unlockedAt': unlockedAt.toIso8601String(),
      'value': value,
      'shareMessage': shareMessage,
    };
  }

  factory Achievement.fromJson(Map<String, dynamic> json) {
    return Achievement(
      id: json['id'],
      title: json['title'],
      description: json['description'],
      icon: IconData(json['icon'], fontFamily: 'MaterialIcons'),
      color: Color(json['color']),
      category: AchievementCategory.values[json['category']],
      unlockedAt: DateTime.parse(json['unlockedAt']),
      value: json['value'],
      shareMessage: json['shareMessage'],
    );
  }

  // Predefined achievements
  static List<Achievement> getAvailableAchievements() {
    return [
      // Meditation achievements
      Achievement(
        id: 'meditation_first',
        title: 'First Light',
        description: 'Completed your first meditation',
        icon: Icons.self_improvement,
        color: AppColors.visualMeditationGradient[0],
        category: AchievementCategory.meditation,
        unlockedAt: DateTime.now(),
        value: 1,
        shareMessage: 'Just completed my first meditation! 🧘‍♀️',
      ),
      Achievement(
        id: 'meditation_100min',
        title: 'Zen Master',
        description: 'Meditated for 100 total minutes',
        icon: Icons.spa,
        color: AppColors.visualMeditationGradient[1],
        category: AchievementCategory.meditation,
        unlockedAt: DateTime.now(),
        value: 100,
        shareMessage: 'Reached 100 minutes of meditation! Finding my inner peace ✨',
      ),
      
      // Affirmation achievements
      Achievement(
        id: 'affirmation_streak_7',
        title: 'Positive Vibes',
        description: 'Listened to affirmations for 7 days straight',
        icon: Icons.favorite,
        color: AppColors.affirmationGradient[0],
        category: AchievementCategory.affirmations,
        unlockedAt: DateTime.now(),
        value: 7,
        shareMessage: 'Kept my positive mindset for a whole week! 💖',
      ),
      
      // Journal achievements
      Achievement(
        id: 'journal_30_entries',
        title: 'Reflection Master',
        description: 'Wrote 30 journal entries',
        icon: Icons.book,
        color: AppColors.journalGradient[0],
        category: AchievementCategory.journaling,
        unlockedAt: DateTime.now(),
        value: 30,
        shareMessage: 'Documented 30 days of my manifestation journey! 📖',
      ),
      
      // Goal achievements
      Achievement(
        id: 'goal_first_complete',
        title: 'Goal Getter',
        description: 'Completed your first goal',
        icon: Icons.flag,
        color: AppColors.goalTrackerGradient[0],
        category: AchievementCategory.goals,
        unlockedAt: DateTime.now(),
        value: 1,
        shareMessage: 'Just manifested my first goal into reality! 🎯',
      ),
      
      // Vision board achievements
      Achievement(
        id: 'vision_board_created',
        title: 'Visionary',
        description: 'Created your first vision board',
        icon: Icons.dashboard,
        color: AppColors.visionBoardGradient[0],
        category: AchievementCategory.visionBoard,
        unlockedAt: DateTime.now(),
        value: 1,
        shareMessage: 'Visualized my dreams with my first vision board! 🎨',
      ),
      
      // Streak achievements
      Achievement(
        id: 'streak_30',
        title: 'Consistency Champion',
        description: 'Maintained a 30-day practice streak',
        icon: Icons.local_fire_department,
        color: AppColors.accentGold,
        category: AchievementCategory.streaks,
        unlockedAt: DateTime.now(),
        value: 30,
        shareMessage: 'On fire with a 30-day manifestation streak! 🔥',
      ),
    ];
  }
}

enum AchievementCategory {
  meditation,
  affirmations,
  journaling,
  goals,
  visionBoard,
  streaks,
  social,
}

extension AchievementCategoryExtension on AchievementCategory {
  String get displayName {
    switch (this) {
      case AchievementCategory.meditation:
        return 'Meditation';
      case AchievementCategory.affirmations:
        return 'Affirmations';
      case AchievementCategory.journaling:
        return 'Journaling';
      case AchievementCategory.goals:
        return 'Goals';
      case AchievementCategory.visionBoard:
        return 'Vision Board';
      case AchievementCategory.streaks:
        return 'Streaks';
      case AchievementCategory.social:
        return 'Social';
    }
  }

  IconData get icon {
    switch (this) {
      case AchievementCategory.meditation:
        return Icons.self_improvement;
      case AchievementCategory.affirmations:
        return Icons.favorite;
      case AchievementCategory.journaling:
        return Icons.book;
      case AchievementCategory.goals:
        return Icons.flag;
      case AchievementCategory.visionBoard:
        return Icons.dashboard;
      case AchievementCategory.streaks:
        return Icons.local_fire_department;
      case AchievementCategory.social:
        return Icons.people;
    }
  }
}

class SocialPost {
  final String id;
  final ManifestationUser user;
  final SocialPostType type;
  final String content;
  final Achievement? achievement;
  final DateTime createdAt;
  final List<String> likes;
  final List<SocialComment> comments;
  final Map<String, dynamic>? metadata; // For additional data

  const SocialPost({
    required this.id,
    required this.user,
    required this.type,
    required this.content,
    this.achievement,
    required this.createdAt,
    required this.likes,
    required this.comments,
    this.metadata,
  });

  bool get isLikedByCurrentUser => likes.contains('current_user_id'); // TODO: Replace with actual current user ID

  SocialPost copyWith({
    String? id,
    ManifestationUser? user,
    SocialPostType? type,
    String? content,
    Achievement? achievement,
    DateTime? createdAt,
    List<String>? likes,
    List<SocialComment>? comments,
    Map<String, dynamic>? metadata,
  }) {
    return SocialPost(
      id: id ?? this.id,
      user: user ?? this.user,
      type: type ?? this.type,
      content: content ?? this.content,
      achievement: achievement ?? this.achievement,
      createdAt: createdAt ?? this.createdAt,
      likes: likes ?? this.likes,
      comments: comments ?? this.comments,
      metadata: metadata ?? this.metadata,
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'user': user.toJson(),
      'type': type.index,
      'content': content,
      'achievement': achievement?.toJson(),
      'createdAt': createdAt.toIso8601String(),
      'likes': likes,
      'comments': comments.map((c) => c.toJson()).toList(),
      'metadata': metadata,
    };
  }

  factory SocialPost.fromJson(Map<String, dynamic> json) {
    return SocialPost(
      id: json['id'],
      user: ManifestationUser.fromJson(json['user']),
      type: SocialPostType.values[json['type']],
      content: json['content'],
      achievement: json['achievement'] != null 
          ? Achievement.fromJson(json['achievement'])
          : null,
      createdAt: DateTime.parse(json['createdAt']),
      likes: List<String>.from(json['likes']),
      comments: (json['comments'] as List)
          .map((c) => SocialComment.fromJson(c))
          .toList(),
      metadata: json['metadata'],
    );
  }
}

enum SocialPostType {
  achievement,
  reflection,
  milestone,
  inspiration,
  goalComplete,
}

extension SocialPostTypeExtension on SocialPostType {
  String get displayName {
    switch (this) {
      case SocialPostType.achievement:
        return 'Achievement Unlocked';
      case SocialPostType.reflection:
        return 'Daily Reflection';
      case SocialPostType.milestone:
        return 'Milestone Reached';
      case SocialPostType.inspiration:
        return 'Inspiration Shared';
      case SocialPostType.goalComplete:
        return 'Goal Completed';
    }
  }

  IconData get icon {
    switch (this) {
      case SocialPostType.achievement:
        return Icons.emoji_events;
      case SocialPostType.reflection:
        return Icons.lightbulb;
      case SocialPostType.milestone:
        return Icons.flag;
      case SocialPostType.inspiration:
        return Icons.auto_awesome;
      case SocialPostType.goalComplete:
        return Icons.check_circle;
    }
  }

  Color get color {
    switch (this) {
      case SocialPostType.achievement:
        return AppColors.accentGold;
      case SocialPostType.reflection:
        return AppColors.journalGradient[0];
      case SocialPostType.milestone:
        return AppColors.goalTrackerGradient[0];
      case SocialPostType.inspiration:
        return AppColors.affirmationGradient[0];
      case SocialPostType.goalComplete:
        return AppColors.accentMint;
    }
  }
}

class SocialComment {
  final String id;
  final String userId;
  final String username;
  final String content;
  final DateTime createdAt;

  const SocialComment({
    required this.id,
    required this.userId,
    required this.username,
    required this.content,
    required this.createdAt,
  });

  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'userId': userId,
      'username': username,
      'content': content,
      'createdAt': createdAt.toIso8601String(),
    };
  }

  factory SocialComment.fromJson(Map<String, dynamic> json) {
    return SocialComment(
      id: json['id'],
      userId: json['userId'],
      username: json['username'],
      content: json['content'],
      createdAt: DateTime.parse(json['createdAt']),
    );
  }
}