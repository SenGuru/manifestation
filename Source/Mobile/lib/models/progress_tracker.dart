import 'package:flutter/material.dart';

/// Unified progress tracking model that handles both milestone and goal progress
class ProgressTracker {
  final String goalId;
  final String goalTitle;
  final double manualProgress; // User-set progress override (0.0 to 100.0)
  final bool useManualProgress; // Whether to use manual or calculated progress
  final DateTime lastUpdated;
  final List<MilestoneProgress> milestones;

  const ProgressTracker({
    required this.goalId,
    required this.goalTitle,
    this.manualProgress = 0.0,
    this.useManualProgress = false,
    required this.lastUpdated,
    required this.milestones,
  });

  /// Get the current effective progress (calculated or manual)
  double get currentProgress {
    if (useManualProgress) {
      return manualProgress.clamp(0.0, 100.0);
    }
    return calculatedProgress;
  }

  /// Calculate progress from milestone completion
  double get calculatedProgress {
    if (milestones.isEmpty) return 0.0;
    
    final totalProgress = milestones
        .map((m) => m.progressPercentage)
        .reduce((a, b) => a + b);
    
    return (totalProgress / milestones.length).clamp(0.0, 100.0);
  }

  /// Get progress display info
  ProgressDisplayInfo get displayInfo {
    final current = currentProgress;
    final calculated = calculatedProgress;
    
    return ProgressDisplayInfo(
      currentProgress: current,
      calculatedProgress: calculated,
      isManualOverride: useManualProgress && (current != calculated),
      completedMilestones: milestones.where((m) => m.isCompleted).length,
      totalMilestones: milestones.length,
      isCompleted: current >= 100.0,
    );
  }

  /// Create copy with updated progress
  ProgressTracker copyWith({
    String? goalId,
    String? goalTitle,
    double? manualProgress,
    bool? useManualProgress,
    DateTime? lastUpdated,
    List<MilestoneProgress>? milestones,
  }) {
    return ProgressTracker(
      goalId: goalId ?? this.goalId,
      goalTitle: goalTitle ?? this.goalTitle,
      manualProgress: manualProgress ?? this.manualProgress,
      useManualProgress: useManualProgress ?? this.useManualProgress,
      lastUpdated: lastUpdated ?? DateTime.now(),
      milestones: milestones ?? this.milestones,
    );
  }

  /// Update a specific milestone's progress
  ProgressTracker updateMilestoneProgress(String milestoneId, double progress) {
    final updatedMilestones = milestones.map((milestone) {
      if (milestone.id == milestoneId) {
        return milestone.copyWith(
          progressPercentage: progress.clamp(0.0, 100.0),
          isCompleted: progress >= 100.0,
          lastUpdated: DateTime.now(),
        );
      }
      return milestone;
    }).toList();

    return copyWith(
      milestones: updatedMilestones,
      lastUpdated: DateTime.now(),
      // If using calculated progress, don't override to manual
      useManualProgress: useManualProgress,
    );
  }

  /// Set manual progress override
  ProgressTracker setManualProgress(double progress) {
    return copyWith(
      manualProgress: progress.clamp(0.0, 100.0),
      useManualProgress: true,
      lastUpdated: DateTime.now(),
    );
  }

  /// Switch back to calculated progress
  ProgressTracker useCalculatedProgress() {
    return copyWith(
      useManualProgress: false,
      lastUpdated: DateTime.now(),
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'goalId': goalId,
      'goalTitle': goalTitle,
      'manualProgress': manualProgress,
      'useManualProgress': useManualProgress,
      'lastUpdated': lastUpdated.toIso8601String(),
      'milestones': milestones.map((m) => m.toJson()).toList(),
    };
  }

  factory ProgressTracker.fromJson(Map<String, dynamic> json) {
    return ProgressTracker(
      goalId: json['goalId'],
      goalTitle: json['goalTitle'],
      manualProgress: (json['manualProgress'] ?? 0.0).toDouble(),
      useManualProgress: json['useManualProgress'] ?? false,
      lastUpdated: DateTime.parse(json['lastUpdated']),
      milestones: (json['milestones'] as List)
          .map((m) => MilestoneProgress.fromJson(m))
          .toList(),
    );
  }
}

/// Individual milestone progress tracking
class MilestoneProgress {
  final String id;
  final String title;
  final String? description;
  final double progressPercentage; // 0.0 to 100.0
  final bool isCompleted;
  final DateTime? targetDate;
  final DateTime lastUpdated;
  final int? apiMilestoneId; // Link to server milestone

  const MilestoneProgress({
    required this.id,
    required this.title,
    this.description,
    this.progressPercentage = 0.0,
    this.isCompleted = false,
    this.targetDate,
    required this.lastUpdated,
    this.apiMilestoneId,
  });

  /// Get milestone status info
  MilestoneStatus get status {
    if (isCompleted) return MilestoneStatus.completed;
    if (progressPercentage > 0) return MilestoneStatus.inProgress;
    if (targetDate != null && DateTime.now().isAfter(targetDate!)) {
      return MilestoneStatus.overdue;
    }
    return MilestoneStatus.notStarted;
  }

  Color get statusColor {
    switch (status) {
      case MilestoneStatus.completed:
        return Colors.green;
      case MilestoneStatus.inProgress:
        return Colors.blue;
      case MilestoneStatus.overdue:
        return Colors.red;
      case MilestoneStatus.notStarted:
        return Colors.grey;
    }
  }

  IconData get statusIcon {
    switch (status) {
      case MilestoneStatus.completed:
        return Icons.check_circle;
      case MilestoneStatus.inProgress:
        return Icons.trending_up;
      case MilestoneStatus.overdue:
        return Icons.warning;
      case MilestoneStatus.notStarted:
        return Icons.radio_button_unchecked;
    }
  }

  MilestoneProgress copyWith({
    String? id,
    String? title,
    String? description,
    double? progressPercentage,
    bool? isCompleted,
    DateTime? targetDate,
    DateTime? lastUpdated,
    int? apiMilestoneId,
  }) {
    return MilestoneProgress(
      id: id ?? this.id,
      title: title ?? this.title,
      description: description ?? this.description,
      progressPercentage: progressPercentage ?? this.progressPercentage,
      isCompleted: isCompleted ?? this.isCompleted,
      targetDate: targetDate ?? this.targetDate,
      lastUpdated: lastUpdated ?? this.lastUpdated,
      apiMilestoneId: apiMilestoneId ?? this.apiMilestoneId,
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'title': title,
      'description': description,
      'progressPercentage': progressPercentage,
      'isCompleted': isCompleted,
      'targetDate': targetDate?.toIso8601String(),
      'lastUpdated': lastUpdated.toIso8601String(),
      'apiMilestoneId': apiMilestoneId,
    };
  }

  factory MilestoneProgress.fromJson(Map<String, dynamic> json) {
    return MilestoneProgress(
      id: json['id'],
      title: json['title'],
      description: json['description'],
      progressPercentage: (json['progressPercentage'] ?? 0.0).toDouble(),
      isCompleted: json['isCompleted'] ?? false,
      targetDate: json['targetDate'] != null 
          ? DateTime.parse(json['targetDate']) 
          : null,
      lastUpdated: DateTime.parse(json['lastUpdated']),
      apiMilestoneId: json['apiMilestoneId'],
    );
  }

  /// Create from existing Milestone model
  factory MilestoneProgress.fromMilestone(dynamic milestone) {
    if (milestone.runtimeType.toString().contains('Milestone')) {
      // From local Milestone model
      return MilestoneProgress(
        id: milestone.id,
        title: milestone.title,
        description: milestone.description,
        progressPercentage: milestone.isCompleted ? 100.0 : 0.0,
        isCompleted: milestone.isCompleted,
        targetDate: milestone.targetDate,
        lastUpdated: DateTime.now(),
      );
    } else {
      // From API milestone model
      return MilestoneProgress(
        id: milestone.id?.toString() ?? DateTime.now().millisecondsSinceEpoch.toString(),
        title: milestone.title ?? 'Untitled Milestone',
        description: milestone.description,
        progressPercentage: milestone.progressPercentage?.toDouble() ?? 0.0,
        isCompleted: milestone.isCompleted ?? false,
        targetDate: milestone.targetDate,
        lastUpdated: DateTime.now(),
        apiMilestoneId: milestone.goalTrackerMileStoneId,
      );
    }
  }
}

/// Progress display information for UI
class ProgressDisplayInfo {
  final double currentProgress;
  final double calculatedProgress;
  final bool isManualOverride;
  final int completedMilestones;
  final int totalMilestones;
  final bool isCompleted;

  const ProgressDisplayInfo({
    required this.currentProgress,
    required this.calculatedProgress,
    required this.isManualOverride,
    required this.completedMilestones,
    required this.totalMilestones,
    required this.isCompleted,
  });

  String get progressText => '${currentProgress.round()}%';
  
  String get milestoneText => '$completedMilestones/$totalMilestones milestones';
  
  String get statusText {
    if (isCompleted) return 'Completed';
    if (currentProgress > 0) return 'In Progress';
    return 'Not Started';
  }
}

enum MilestoneStatus {
  notStarted,
  inProgress,
  completed,
  overdue,
}