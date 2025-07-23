import 'package:flutter/material.dart';
import '../constants/colors.dart';
import '../utils/color_utils.dart';
import '../constants/category_icons.dart';

class GoalTrackerCategory {
  final String id;
  final String name;
  final String description;
  final Color color; // Direct Color object like AffirmationCategory
  final int? iconValue; // Icon stored as integer value
  final bool isDefault;
  final int? userId;
  final DateTime? createdAt;
  final DateTime? updatedAt;

  const GoalTrackerCategory({
    required this.id,
    required this.name,
    required this.description,
    required this.color,
    this.iconValue,
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
      color: json['color'] is int 
          ? Color(json['color']) 
          : (json['color'] is String 
              ? ColorUtils.hexToColor(json['color']) 
              : AppColors.goalTrackerGradient[0]),
      iconValue: json['iconValue'] as int? ?? json['predefinedIconTypeId'] as int?,
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

  /// Create a category with a hex color string
  factory GoalTrackerCategory.withHexColor({
    required String id,
    required String name,
    required String description,
    required String colorHex,
    int? iconValue,
    bool isDefault = false,
    int? userId,
    DateTime? createdAt,
    DateTime? updatedAt,
  }) {
    return GoalTrackerCategory(
      id: id,
      name: name,
      description: description,
      color: ColorUtils.hexToColor(colorHex),
      iconValue: iconValue,
      isDefault: isDefault,
      userId: userId,
      createdAt: createdAt,
      updatedAt: updatedAt,
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'name': name,
      'description': description,
      'color': color.value,
      if (iconValue != null) 'iconValue': iconValue,
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
    Color? color,
    int? iconValue,
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
      iconValue: iconValue ?? this.iconValue,
      isDefault: isDefault ?? this.isDefault,
      userId: userId ?? this.userId,
      createdAt: createdAt ?? this.createdAt,
      updatedAt: updatedAt ?? this.updatedAt,
    );
  }

  /// Get the color as a hex string
  String get colorHex => ColorUtils.colorToHex(color);

  /// Get readable text color for this category's background
  Color get textColor => ColorUtils.getTextColor(color);

  /// Get the icon data for this category
  IconData get iconData => CategoryIconUtils.getIconFromValue(iconValue);

  /// Get the icon type for this category
  PredefinedIconTypes get iconType => 
      PredefinedIconTypes.fromValue(iconValue ?? 0) ?? 
      CategoryIconUtils.getDefaultIconForCategory(name);

  /// Get color as Flutter Color object (kept for backward compatibility)
  Color get displayColor => color;

  /// Get icon as IconData (kept for backward compatibility)
  IconData get displayIcon => iconData;

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
  final Color color;
  final int? iconValue;
  final int? userId;

  const CreateGoalTrackerCategoryRequest({
    required this.name,
    required this.description,
    required this.color,
    this.iconValue,
    this.userId,
  });

  Map<String, dynamic> toJson() {
    return {
      'name': name,
      'description': description,
      'color': color.value,
      if (iconValue != null) 'iconValue': iconValue,
      if (userId != null) 'userId': userId,
      'isDefault': false,
    };
  }

  /// Create request with hex color
  factory CreateGoalTrackerCategoryRequest.withHexColor({
    required String name,
    required String description,
    required String colorHex,
    int? iconValue,
    int? userId,
  }) {
    return CreateGoalTrackerCategoryRequest(
      name: name,
      description: description,
      color: ColorUtils.hexToColor(colorHex),
      iconValue: iconValue,
      userId: userId,
    );
  }
}