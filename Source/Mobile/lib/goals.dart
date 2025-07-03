import 'package:flutter/material.dart';
import '../colors.dart';

class Goal {
  final String id;
  final String title;
  final String description;
  final GoalCategory category;
  final GoalPriority priority;
  final DateTime targetDate;
  final DateTime createdAt;
  final DateTime? updatedAt;
  final List<Milestone> milestones;
  final double currentProgress; // 0.0 to 100.0
  final List<String> tags;
  final String? imageUrl;

  const Goal({
    required this.id,
    required this.title,
    required this.description,
    required this.category,
    required this.priority,
    required this.targetDate,
    required this.createdAt,
    this.updatedAt,
    required this.milestones,
    required this.currentProgress,
    required this.tags,
    this.imageUrl,
  });

  Goal copyWith({
    String? id,
    String? title,
    String? description,
    GoalCategory? category,
    GoalPriority? priority,
    DateTime? targetDate,
    DateTime? createdAt,
    DateTime? updatedAt,
    List<Milestone>? milestones,
    double? currentProgress,
    List<String>? tags,
    String? imageUrl,
  }) {
    return Goal(
      id: id ?? this.id,
      title: title ?? this.title,
      description: description ?? this.description,
      category: category ?? this.category,
      priority: priority ?? this.priority,
      targetDate: targetDate ?? this.targetDate,
      createdAt: createdAt ?? this.createdAt,
      updatedAt: updatedAt ?? DateTime.now(),
      milestones: milestones ?? this.milestones,
      currentProgress: currentProgress ?? this.currentProgress,
      tags: tags ?? this.tags,
      imageUrl: imageUrl ?? this.imageUrl,
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'title': title,
      'description': description,
      'category': category.index,
      'priority': priority.index,
      'targetDate': targetDate.toIso8601String(),
      'createdAt': createdAt.toIso8601String(),
      'updatedAt': updatedAt?.toIso8601String(),
      'milestones': milestones.map((m) => m.toJson()).toList(),
      'currentProgress': currentProgress,
      'tags': tags,
      'imageUrl': imageUrl,
    };
  }

  factory Goal.fromJson(Map<String, dynamic> json) {
    return Goal(
      id: json['id'],
      title: json['title'],
      description: json['description'],
      category: GoalCategory.values[json['category']],
      priority: GoalPriority.values[json['priority']],
      targetDate: DateTime.parse(json['targetDate']),
      createdAt: DateTime.parse(json['createdAt']),
      updatedAt: json['updatedAt'] != null 
          ? DateTime.parse(json['updatedAt'])
          : null,
      milestones: (json['milestones'] as List)
          .map((m) => Milestone.fromJson(m))
          .toList(),
      currentProgress: json['currentProgress'].toDouble(),
      tags: List<String>.from(json['tags']),
      imageUrl: json['imageUrl'],
    );
  }

  bool get isCompleted => currentProgress >= 100.0;
  
  bool get isOverdue => !isCompleted && DateTime.now().isAfter(targetDate);
  
  int get daysUntilTarget => targetDate.difference(DateTime.now()).inDays;
  
  double get progressPercentage => currentProgress.clamp(0.0, 100.0);
  
  int get completedMilestones => milestones.where((m) => m.isCompleted).length;
  
  GoalStatus get status {
    if (isCompleted) return GoalStatus.completed;
    if (isOverdue) return GoalStatus.overdue;
    if (currentProgress > 0) return GoalStatus.inProgress;
    return GoalStatus.notStarted;
  }

  // Sample goals for demonstration
  factory Goal.sample1() {
    return Goal(
      id: 'goal_1',
      title: 'Launch My Dream Business',
      description: 'Start my own sustainable lifestyle brand focusing on eco-friendly products',
      category: GoalCategory.career,
      priority: GoalPriority.high,
      targetDate: DateTime.now().add(Duration(days: 180)),
      createdAt: DateTime.now().subtract(Duration(days: 30)),
      milestones: [
        Milestone(
          id: 'm1',
          title: 'Complete business plan',
          isCompleted: true,
          completedAt: DateTime.now().subtract(Duration(days: 20)),
        ),
        Milestone(
          id: 'm2',
          title: 'Secure initial funding',
          isCompleted: true,
          completedAt: DateTime.now().subtract(Duration(days: 10)),
        ),
        Milestone(
          id: 'm3',
          title: 'Develop first product line',
          isCompleted: false,
        ),
        Milestone(
          id: 'm4',
          title: 'Launch website and marketing',
          isCompleted: false,
        ),
      ],
      currentProgress: 35.0,
      tags: ['business', 'entrepreneurship', 'sustainability'],
    );
  }

  factory Goal.sample2() {
    return Goal(
      id: 'goal_2',
      title: 'Find My Soulmate',
      description: 'Manifest a loving, supportive partnership with someone who shares my values',
      category: GoalCategory.relationships,
      priority: GoalPriority.medium,
      targetDate: DateTime.now().add(Duration(days: 365)),
      createdAt: DateTime.now().subtract(Duration(days: 45)),
      milestones: [
        Milestone(
          id: 'm5',
          title: 'Work on self-love and confidence',
          isCompleted: true,
          completedAt: DateTime.now().subtract(Duration(days: 30)),
        ),
        Milestone(
          id: 'm6',
          title: 'Join social activities and dating apps',
          isCompleted: true,
          completedAt: DateTime.now().subtract(Duration(days: 15)),
        ),
        Milestone(
          id: 'm7',
          title: 'Go on regular dates',
          isCompleted: false,
        ),
        Milestone(
          id: 'm8',
          title: 'Build meaningful connection',
          isCompleted: false,
        ),
      ],
      currentProgress: 25.0,
      tags: ['love', 'relationships', 'self-improvement'],
    );
  }

  factory Goal.sample3() {
    return Goal(
      id: 'goal_3',
      title: 'Achieve Financial Freedom',
      description: 'Build enough passive income to live comfortably without working',
      category: GoalCategory.financial,
      priority: GoalPriority.high,
      targetDate: DateTime.now().add(Duration(days: 1095)), // 3 years
      createdAt: DateTime.now().subtract(Duration(days: 60)),
      milestones: [
        Milestone(
          id: 'm9',
          title: 'Create detailed budget plan',
          isCompleted: true,
          completedAt: DateTime.now().subtract(Duration(days: 50)),
        ),
        Milestone(
          id: 'm10',
          title: 'Build emergency fund (6 months)',
          isCompleted: true,
          completedAt: DateTime.now().subtract(Duration(days: 20)),
        ),
        Milestone(
          id: 'm11',
          title: 'Start investing in index funds',
          isCompleted: false,
        ),
        Milestone(
          id: 'm12',
          title: 'Develop multiple income streams',
          isCompleted: false,
        ),
      ],
      currentProgress: 40.0,
      tags: ['money', 'investing', 'freedom'],
    );
  }
}

class Milestone {
  final String id;
  final String title;
  final String? description;
  final bool isCompleted;
  final DateTime? completedAt;
  final DateTime? targetDate;

  const Milestone({
    required this.id,
    required this.title,
    this.description,
    required this.isCompleted,
    this.completedAt,
    this.targetDate,
  });

  Milestone copyWith({
    String? id,
    String? title,
    String? description,
    bool? isCompleted,
    DateTime? completedAt,
    DateTime? targetDate,
  }) {
    return Milestone(
      id: id ?? this.id,
      title: title ?? this.title,
      description: description ?? this.description,
      isCompleted: isCompleted ?? this.isCompleted,
      completedAt: completedAt ?? this.completedAt,
      targetDate: targetDate ?? this.targetDate,
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'title': title,
      'description': description,
      'isCompleted': isCompleted,
      'completedAt': completedAt?.toIso8601String(),
      'targetDate': targetDate?.toIso8601String(),
    };
  }

  factory Milestone.fromJson(Map<String, dynamic> json) {
    return Milestone(
      id: json['id'],
      title: json['title'],
      description: json['description'],
      isCompleted: json['isCompleted'],
      completedAt: json['completedAt'] != null 
          ? DateTime.parse(json['completedAt'])
          : null,
      targetDate: json['targetDate'] != null 
          ? DateTime.parse(json['targetDate'])
          : null,
    );
  }
}

enum GoalCategory {
  all,
  career,
  health,
  relationships,
  financial,
  personal,
  spiritual,
}

extension GoalCategoryExtension on GoalCategory {
  String get displayName {
    switch (this) {
      case GoalCategory.all:
        return 'All';
      case GoalCategory.career:
        return 'Career';
      case GoalCategory.health:
        return 'Health';
      case GoalCategory.relationships:
        return 'Love';
      case GoalCategory.financial:
        return 'Money';
      case GoalCategory.personal:
        return 'Personal';
      case GoalCategory.spiritual:
        return 'Spiritual';
    }
  }

  IconData get icon {
    switch (this) {
      case GoalCategory.all:
        return Icons.all_inclusive;
      case GoalCategory.career:
        return Icons.business_center;
      case GoalCategory.health:
        return Icons.favorite;
      case GoalCategory.relationships:
        return Icons.people;
      case GoalCategory.financial:
        return Icons.account_balance_wallet;
      case GoalCategory.personal:
        return Icons.person;
      case GoalCategory.spiritual:
        return Icons.spa;
    }
  }

  Color get color {
    switch (this) {
      case GoalCategory.all:
        return AppColors.textMedium;
      case GoalCategory.career:
        return AppColors.gradientBlue;
      case GoalCategory.health:
        return AppColors.gradientPink;
      case GoalCategory.relationships:
        return AppColors.accentLavender;
      case GoalCategory.financial:
        return AppColors.accentGold;
      case GoalCategory.personal:
        return AppColors.secondaryTeal;
      case GoalCategory.spiritual:
        return AppColors.gradientPurple;
    }
  }
}

enum GoalPriority {
  low,
  medium,
  high,
}

extension GoalPriorityExtension on GoalPriority {
  String get displayName {
    switch (this) {
      case GoalPriority.low:
        return 'Low';
      case GoalPriority.medium:
        return 'Medium';
      case GoalPriority.high:
        return 'High';
    }
  }

  Color get color {
    switch (this) {
      case GoalPriority.low:
        return AppColors.accentMint;
      case GoalPriority.medium:
        return AppColors.gradientOrange;
      case GoalPriority.high:
        return AppColors.gradientPink;
    }
  }

  IconData get icon {
    switch (this) {
      case GoalPriority.low:
        return Icons.low_priority;
      case GoalPriority.medium:
        return Icons.remove;
      case GoalPriority.high:
        return Icons.priority_high;
    }
  }
}

enum GoalStatus {
  notStarted,
  inProgress,
  completed,
  overdue,
}

extension GoalStatusExtension on GoalStatus {
  String get displayName {
    switch (this) {
      case GoalStatus.notStarted:
        return 'Not Started';
      case GoalStatus.inProgress:
        return 'In Progress';
      case GoalStatus.completed:
        return 'Completed';
      case GoalStatus.overdue:
        return 'Overdue';
    }
  }

  Color get color {
    switch (this) {
      case GoalStatus.notStarted:
        return AppColors.textMedium;
      case GoalStatus.inProgress:
        return AppColors.gradientBlue;
      case GoalStatus.completed:
        return AppColors.accentMint;
      case GoalStatus.overdue:
        return Colors.red;
    }
  }

  IconData get icon {
    switch (this) {
      case GoalStatus.notStarted:
        return Icons.hourglass_empty;
      case GoalStatus.inProgress:
        return Icons.trending_up;
      case GoalStatus.completed:
        return Icons.check_circle;
      case GoalStatus.overdue:
        return Icons.warning;
    }
  }
}