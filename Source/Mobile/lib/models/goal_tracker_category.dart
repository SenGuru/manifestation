import 'package:flutter/material.dart';
import '../constants/colors.dart';

class GoalTrackerCategory {
  final String id;
  final String name;
  final String description;
  final String? color;
  final String? icon;
  final bool isDefault;
  final int? userId;
  final DateTime? createdAt;
  final DateTime? updatedAt;

  const GoalTrackerCategory({
    required this.id,
    required this.name,
    required this.description,
    this.color,
    this.icon,
    this.isDefault = false,
    this.userId,
    this.createdAt,
    this.updatedAt,
  });

  factory GoalTrackerCategory.fromJson(Map<String, dynamic> json) {
    return GoalTrackerCategory(
      id: json['id']?.toString() ?? json['categoryId']?.toString() ?? '',
      name: json['name'] ?? json['categoryName'] ?? json['defaultCategoryName'] ?? '',
      description: json['description'] ?? '',
      color: json['color'],
      icon: json['icon'],
      isDefault: json['isDefault'] == true,
      userId: json['userId'] as int?,
      createdAt: json['createdAt'] != null 
          ? DateTime.tryParse(json['createdAt'].toString())
          : null,
      updatedAt: json['updatedAt'] != null 
          ? DateTime.tryParse(json['updatedAt'].toString())
          : null,
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'name': name,
      'description': description,
      if (color != null) 'color': color,
      if (icon != null) 'icon': icon,
      'isDefault': isDefault,
      if (userId != null) 'userId': userId,
      if (createdAt != null) 'createdAt': createdAt!.toIso8601String(),
      if (updatedAt != null) 'updatedAt': updatedAt!.toIso8601String(),
    };
  }

  GoalTrackerCategory copyWith({
    String? id,
    String? name,
    String? description,
    String? color,
    String? icon,
    bool? isDefault,
    int? userId,
    DateTime? createdAt,
    DateTime? updatedAt,
  }) {
    return GoalTrackerCategory(
      id: id ?? this.id,
      name: name ?? this.name,
      description: description ?? this.description,
      color: color ?? this.color,
      icon: icon ?? this.icon,
      isDefault: isDefault ?? this.isDefault,
      userId: userId ?? this.userId,
      createdAt: createdAt ?? this.createdAt,
      updatedAt: updatedAt ?? this.updatedAt,
    );
  }

  // UI Helper methods
  Color get displayColor {
    if (color != null && color!.isNotEmpty) {
      try {
        // Handle hex colors
        String colorString = color!;
        if (colorString.startsWith('#')) {
          colorString = colorString.substring(1);
        }
        if (colorString.length == 6) {
          colorString = 'FF$colorString';
        }
        return Color(int.parse(colorString, radix: 16));
      } catch (e) {
        // Fallback to category-based colors
      }
    }
    
    // Fallback colors based on category name
    final lowerName = name.toLowerCase();
    if (lowerName.contains('career') || lowerName.contains('work')) {
      return AppColors.secondaryTeal;
    } else if (lowerName.contains('health') || lowerName.contains('fitness')) {
      return AppColors.accentMint;
    } else if (lowerName.contains('financial') || lowerName.contains('money')) {
      return AppColors.accentGold;
    } else if (lowerName.contains('relationship') || lowerName.contains('love')) {
      return AppColors.gradientPink;
    } else if (lowerName.contains('spiritual') || lowerName.contains('mindful')) {
      return AppColors.gradientPurple;
    } else {
      return AppColors.goalTrackerGradient[0];
    }
  }

  IconData get displayIcon {
    if (icon != null && icon!.isNotEmpty) {
      // Map icon names to IconData if needed
      switch (icon!.toLowerCase()) {
        case 'work':
        case 'career':
          return Icons.work;
        case 'health':
        case 'fitness':
          return Icons.fitness_center;
        case 'money':
        case 'financial':
          return Icons.attach_money;
        case 'love':
        case 'relationship':
          return Icons.favorite;
        case 'spiritual':
        case 'mindfulness':
          return Icons.self_improvement;
        case 'travel':
          return Icons.flight;
        case 'education':
        case 'learning':
          return Icons.school;
        case 'creative':
        case 'art':
          return Icons.palette;
        default:
          return Icons.flag;
      }
    }
    
    // Fallback icons based on category name
    final lowerName = name.toLowerCase();
    if (lowerName.contains('career') || lowerName.contains('work')) {
      return Icons.work;
    } else if (lowerName.contains('health') || lowerName.contains('fitness')) {
      return Icons.fitness_center;
    } else if (lowerName.contains('financial') || lowerName.contains('money')) {
      return Icons.attach_money;
    } else if (lowerName.contains('relationship') || lowerName.contains('love')) {
      return Icons.favorite;
    } else if (lowerName.contains('spiritual') || lowerName.contains('mindful')) {
      return Icons.self_improvement;
    } else if (lowerName.contains('travel')) {
      return Icons.flight;
    } else if (lowerName.contains('education') || lowerName.contains('learning')) {
      return Icons.school;
    } else if (lowerName.contains('creative') || lowerName.contains('art')) {
      return Icons.palette;
    } else {
      return Icons.flag;
    }
  }

  String get displayName => name.isNotEmpty ? name : 'Untitled Category';

  @override
  bool operator ==(Object other) =>
      identical(this, other) ||
      other is GoalTrackerCategory &&
          runtimeType == other.runtimeType &&
          id == other.id;

  @override
  int get hashCode => id.hashCode;

  @override
  String toString() => 'GoalTrackerCategory(id: $id, name: $name)';
}

// Helper class for creating new categories
class CreateGoalTrackerCategoryRequest {
  final String name;
  final String description;
  final String? color;
  final String? icon;
  final int? userId;

  const CreateGoalTrackerCategoryRequest({
    required this.name,
    required this.description,
    this.color,
    this.icon,
    this.userId,
  });

  Map<String, dynamic> toJson() {
    return {
      'name': name,
      'description': description,
      if (color != null) 'color': color,
      if (icon != null) 'icon': icon,
      if (userId != null) 'userId': userId,
      'isDefault': false,
    };
  }
}