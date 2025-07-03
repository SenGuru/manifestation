import 'package:flutter/material.dart';

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
  final List<Affirmation> affirmations;
  final bool isCustom;
  final DateTime createdAt;

  AffirmationCategory({
    required this.id,
    required this.name,
    required this.color,
    required this.affirmations,
    this.isCustom = false,
    DateTime? createdAt,
  }) : createdAt = createdAt ?? DateTime.now();

  AffirmationCategory copyWith({
    String? id,
    String? name,
    Color? color,
    List<Affirmation>? affirmations,
    bool? isCustom,
    DateTime? createdAt,
  }) {
    return AffirmationCategory(
      id: id ?? this.id,
      name: name ?? this.name,
      color: color ?? this.color,
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
      'affirmations': affirmations.map((a) => a.toJson()).toList(),
      'isCustom': isCustom,
      'createdAt': createdAt.toIso8601String(),
    };
  }

  factory AffirmationCategory.fromJson(Map<String, dynamic> json) {
    return AffirmationCategory(
      id: json['id'],
      name: json['name'],
      color: Color(json['color']),
      affirmations: (json['affirmations'] as List)
          .map((a) => Affirmation.fromJson(a))
          .toList(),
      isCustom: json['isCustom'] ?? false,
      createdAt: json['createdAt'] != null
          ? DateTime.parse(json['createdAt'])
          : DateTime.now(),
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