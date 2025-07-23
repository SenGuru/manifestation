import 'package:flutter/material.dart';
import '../utils/color_utils.dart';
import '../constants/category_icons.dart';

class Affirmation {
  final String id;
  final String text;
  final bool isFavorite;
  final DateTime? lastPlayed;
  final DateTime createdAt;
  final bool isCustom;

  Affirmation({
    required this.id,
    required this.text,
    this.isFavorite = false,
    this.lastPlayed,
    DateTime? createdAt,
    this.isCustom = false,
  }) : createdAt = createdAt ?? DateTime.now();

  Affirmation copyWith({
    String? id,
    String? text,
    bool? isFavorite,
    DateTime? lastPlayed,
    DateTime? createdAt,
    bool? isCustom,
  }) {
    return Affirmation(
      id: id ?? this.id,
      text: text ?? this.text,
      isFavorite: isFavorite ?? this.isFavorite,
      lastPlayed: lastPlayed ?? this.lastPlayed,
      createdAt: createdAt ?? this.createdAt,
      isCustom: isCustom ?? this.isCustom,
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'text': text,
      'isFavorite': isFavorite,
      'lastPlayed': lastPlayed?.toIso8601String(),
      'createdAt': createdAt.toIso8601String(),
      'isCustom': isCustom,
    };
  }

  factory Affirmation.fromJson(Map<String, dynamic> json) {
    return Affirmation(
      id: json['id'],
      text: json['text'],
      isFavorite: json['isFavorite'] ?? false,
      lastPlayed: json['lastPlayed'] != null 
          ? DateTime.parse(json['lastPlayed'])
          : null,
      createdAt: json['createdAt'] != null
          ? DateTime.parse(json['createdAt'])
          : DateTime.now(),
      isCustom: json['isCustom'] ?? false,
    );
  }
}

class AffirmationCategory {
  final String id;
  final String name;
  final Color color;
  final int? iconValue; // Icon stored as integer value
  final List<Affirmation> affirmations;
  final bool isCustom;
  final DateTime createdAt;

  AffirmationCategory({
    required this.id,
    required this.name,
    required this.color,
    this.iconValue,
    required this.affirmations,
    this.isCustom = false,
    DateTime? createdAt,
  }) : createdAt = createdAt ?? DateTime.now();

  AffirmationCategory copyWith({
    String? id,
    String? name,
    Color? color,
    int? iconValue,
    List<Affirmation>? affirmations,
    bool? isCustom,
    DateTime? createdAt,
  }) {
    return AffirmationCategory(
      id: id ?? this.id,
      name: name ?? this.name,
      color: color ?? this.color,
      iconValue: iconValue ?? this.iconValue,
      affirmations: affirmations ?? this.affirmations,
      isCustom: isCustom ?? this.isCustom,
      createdAt: createdAt ?? this.createdAt,
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'name': name,
      'color': color.value,
      'iconValue': iconValue,
      'affirmations': affirmations.map((a) => a.toJson()).toList(),
      'isCustom': isCustom,
      'createdAt': createdAt.toIso8601String(),
    };
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

  factory AffirmationCategory.fromJson(Map<String, dynamic> json) {
    return AffirmationCategory(
      id: json['id'],
      name: json['name'],
      color: Color(json['color']),
      iconValue: json['iconValue'] as int?,
      affirmations: (json['affirmations'] as List)
          .map((a) => Affirmation.fromJson(a))
          .toList(),
      isCustom: json['isCustom'] ?? false,
      createdAt: json['createdAt'] != null
          ? DateTime.parse(json['createdAt'])
          : DateTime.now(),
    );
  }

  /// Create a category with a hex color string
  factory AffirmationCategory.withHexColor({
    required String id,
    required String name,
    required String colorHex,
    int? iconValue,
    required List<Affirmation> affirmations,
    bool isCustom = false,
    DateTime? createdAt,
  }) {
    return AffirmationCategory(
      id: id,
      name: name,
      color: ColorUtils.hexToColor(colorHex),
      iconValue: iconValue,
      affirmations: affirmations,
      isCustom: isCustom,
      createdAt: createdAt,
    );
  }
}

enum AffirmationTime {
  morning,
  midday,
  evening,
}

extension AffirmationTimeExtension on AffirmationTime {
  String get displayName {
    switch (this) {
      case AffirmationTime.morning:
        return 'Morning';
      case AffirmationTime.midday:
        return 'Midday';
      case AffirmationTime.evening:
        return 'Evening';
    }
  }

  String get description {
    switch (this) {
      case AffirmationTime.morning:
        return 'Start your day with positive energy';
      case AffirmationTime.midday:
        return 'Recharge your motivation';
      case AffirmationTime.evening:
        return 'Reflect and set intentions';
    }
  }
}