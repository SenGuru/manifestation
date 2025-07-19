import 'package:flutter/material.dart';

/// A milestone model for API integration
/// This replaces API-dependent milestone models with local hardcoded data
class ApiMilestone {
  final String id;
  final String goalTrackerId;
  final String title;
  final String description;
  final bool isCompleted;
  final double progressPercentage;
  final DateTime? targetDate;
  final DateTime? completedDate;
  final DateTime createdAt;
  final DateTime modifiedAt;
  final String createdBy;
  final String modifiedBy;
  final bool isDeleted;
  final int? userId;

  const ApiMilestone({
    required this.id,
    required this.goalTrackerId,
    required this.title,
    this.description = '',
    this.isCompleted = false,
    this.progressPercentage = 0.0,
    this.targetDate,
    this.completedDate,
    required this.createdAt,
    required this.modifiedAt,
    this.createdBy = 'user',
    this.modifiedBy = 'user',
    this.isDeleted = false,
    this.userId,
  });

  bool get isOverdue {
    if (targetDate == null || isCompleted) return false;
    return DateTime.now().isAfter(targetDate!);
  }

  IconData get statusIcon {
    if (isCompleted) return Icons.check_circle;
    if (isOverdue) return Icons.warning;
    return Icons.radio_button_unchecked;
  }

  Color get statusColor {
    if (isCompleted) return Colors.green;
    if (isOverdue) return Colors.red;
    return Colors.grey;
  }

  String get statusText {
    if (isCompleted) return 'Completed';
    if (isOverdue) return 'Overdue';
    return 'In Progress';
  }

  ApiMilestone copyWith({
    String? id,
    String? goalTrackerId,
    String? title,
    String? description,
    bool? isCompleted,
    double? progressPercentage,
    DateTime? targetDate,
    DateTime? completedDate,
    DateTime? createdAt,
    DateTime? modifiedAt,
    String? createdBy,
    String? modifiedBy,
    bool? isDeleted,
    int? userId,
  }) {
    return ApiMilestone(
      id: id ?? this.id,
      goalTrackerId: goalTrackerId ?? this.goalTrackerId,
      title: title ?? this.title,
      description: description ?? this.description,
      isCompleted: isCompleted ?? this.isCompleted,
      progressPercentage: progressPercentage ?? this.progressPercentage,
      targetDate: targetDate ?? this.targetDate,
      completedDate: completedDate ?? this.completedDate,
      createdAt: createdAt ?? this.createdAt,
      modifiedAt: modifiedAt ?? this.modifiedAt,
      createdBy: createdBy ?? this.createdBy,
      modifiedBy: modifiedBy ?? this.modifiedBy,
      isDeleted: isDeleted ?? this.isDeleted,
      userId: userId ?? this.userId,
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'goalTrackerId': goalTrackerId,
      'title': title,
      'description': description,
      'isCompleted': isCompleted,
      'progressPercentage': progressPercentage,
      'targetDate': targetDate?.toIso8601String(),
      'completedDate': completedDate?.toIso8601String(),
      'createdAt': createdAt.toIso8601String(),
      'modifiedAt': modifiedAt.toIso8601String(),
      'createdBy': createdBy,
      'modifiedBy': modifiedBy,
      'isDeleted': isDeleted,
      'userId': userId,
    };
  }

  factory ApiMilestone.fromJson(Map<String, dynamic> json) {
    return ApiMilestone(
      id: json['id'] ?? '',
      goalTrackerId: json['goalTrackerId'] ?? '',
      title: json['title'] ?? '',
      description: json['description'] ?? '',
      isCompleted: json['isCompleted'] ?? false,
      progressPercentage: (json['progressPercentage'] ?? 0.0).toDouble(),
      targetDate: json['targetDate'] != null ? DateTime.parse(json['targetDate']) : null,
      completedDate: json['completedDate'] != null ? DateTime.parse(json['completedDate']) : null,
      createdAt: DateTime.parse(json['createdAt'] ?? DateTime.now().toIso8601String()),
      modifiedAt: DateTime.parse(json['modifiedAt'] ?? DateTime.now().toIso8601String()),
      createdBy: json['createdBy'] ?? 'user',
      modifiedBy: json['modifiedBy'] ?? 'user',
      isDeleted: json['isDeleted'] ?? false,
      userId: json['userId'],
    );
  }
}