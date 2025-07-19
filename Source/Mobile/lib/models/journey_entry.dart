import 'package:flutter/material.dart';
import 'package:fun/constants/colors.dart';

class JournalEntry {
  final String id;
  final DateTime date;
  final Mood mood;
  final List<String> gratitudes;
  final String reflection;
  final String intention;
  final DateTime createdAt;
  final DateTime? updatedAt;

  const JournalEntry({
    required this.id,
    required this.date,
    required this.mood,
    required this.gratitudes,
    required this.reflection,
    required this.intention,
    DateTime? createdAt,
    this.updatedAt,
  }) : createdAt = createdAt ?? date;

  JournalEntry copyWith({
    String? id,
    DateTime? date,
    Mood? mood,
    List<String>? gratitudes,
    String? reflection,
    String? intention,
    DateTime? createdAt,
    DateTime? updatedAt,
  }) {
    return JournalEntry(
      id: id ?? this.id,
      date: date ?? this.date,
      mood: mood ?? this.mood,
      gratitudes: gratitudes ?? this.gratitudes,
      reflection: reflection ?? this.reflection,
      intention: intention ?? this.intention,
      createdAt: createdAt ?? this.createdAt,
      updatedAt: updatedAt ?? DateTime.now(),
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'date': date.toIso8601String(),
      'mood': mood.index,
      'gratitudes': gratitudes,
      'reflection': reflection,
      'intention': intention,
      'createdAt': createdAt.toIso8601String(),
      'updatedAt': updatedAt?.toIso8601String(),
    };
  }

  factory JournalEntry.fromJson(Map<String, dynamic> json) {
    return JournalEntry(
      id: json['id'],
      date: DateTime.parse(json['date']),
      mood: Mood.values[json['mood']],
      gratitudes: List<String>.from(json['gratitudes']),
      reflection: json['reflection'],
      intention: json['intention'],
      createdAt: DateTime.parse(json['createdAt']),
      updatedAt: json['updatedAt'] != null 
          ? DateTime.parse(json['updatedAt'])
          : null,
    );
  }

  bool get isComplete {
    return gratitudes.isNotEmpty && reflection.isNotEmpty;
  }

  double get completionPercentage {
    int filledSections = 0;
    int totalSections = 4;

    if (mood != Mood.neutral) filledSections++;
    if (gratitudes.where((g) => g.isNotEmpty).isNotEmpty) filledSections++;
    if (reflection.isNotEmpty) filledSections++;
    if (intention.isNotEmpty) filledSections++;

    return filledSections / totalSections;
  }
}

enum Mood {
  terrible,
  bad,
  neutral,
  good,
  amazing,
}

extension MoodExtension on Mood {
  String get displayName {
    switch (this) {
      case Mood.terrible:
        return 'Terrible';
      case Mood.bad:
        return 'Bad';
      case Mood.neutral:
        return 'Neutral';
      case Mood.good:
        return 'Good';
      case Mood.amazing:
        return 'Amazing';
    }
  }

  String get emoji {
    switch (this) {
      case Mood.terrible:
        return '😢';
      case Mood.bad:
        return '😔';
      case Mood.neutral:
        return '😐';
      case Mood.good:
        return '😊';
      case Mood.amazing:
        return '🤩';
    }
  }

  IconData get icon {
    switch (this) {
      case Mood.terrible:
        return Icons.sentiment_very_dissatisfied;
      case Mood.bad:
        return Icons.sentiment_dissatisfied;
      case Mood.neutral:
        return Icons.sentiment_neutral;
      case Mood.good:
        return Icons.sentiment_satisfied;
      case Mood.amazing:
        return Icons.sentiment_very_satisfied;
    }
  }

  Color get color {
    switch (this) {
      case Mood.terrible:
        return Color(0xFFE53E3E); // Red
      case Mood.bad:
        return Color(0xFFFF8A50); // Orange
      case Mood.neutral:
        return AppColors.textMedium; // Gray
      case Mood.good:
        return AppColors.secondaryTeal; // Teal
      case Mood.amazing:
        return AppColors.accentMint; // Green
    }
  }

  String get description {
    switch (this) {
      case Mood.terrible:
        return 'Having a really tough day';
      case Mood.bad:
        return 'Feeling down or frustrated';
      case Mood.neutral:
        return 'Feeling okay, nothing special';
      case Mood.good:
        return 'Feeling positive and content';
      case Mood.amazing:
        return 'Feeling fantastic and energized';
    }
  }
}

class JournalStats {
  final int totalEntries;
  final int currentStreak;
  final int longestStreak;
  final Map<Mood, int> moodCounts;
  final List<String> topGratitudes;
  final DateTime? lastEntryDate;

  const JournalStats({
    required this.totalEntries,
    required this.currentStreak,
    required this.longestStreak,
    required this.moodCounts,
    required this.topGratitudes,
    this.lastEntryDate,
  });

  factory JournalStats.empty() {
    return JournalStats(
      totalEntries: 0,
      currentStreak: 0,
      longestStreak: 0,
      moodCounts: {},
      topGratitudes: [],
    );
  }

  Mood get averageMood {
    if (moodCounts.isEmpty) return Mood.neutral;
    
    double total = 0;
    int count = 0;
    
    moodCounts.forEach((mood, frequency) {
      total += mood.index * frequency;
      count += frequency;
    });
    
    final average = total / count;
    return Mood.values[average.round().clamp(0, Mood.values.length - 1)];
  }

  double get completionRate {
    // This would be calculated from actual data
    return 0.75; // Placeholder
  }
}