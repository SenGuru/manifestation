import 'package:flutter/material.dart';
import 'affirmation_item.dart';

/// Model for caching affirmation categories
class AffirmationCategory {
  final String id;
  final String name;
  final bool isDefault;
  final List<AffirmationItem> affirmations;
  final Color? color;
  final int? iconId;

  const AffirmationCategory({
    required this.id,
    required this.name,
    required this.isDefault,
    required this.affirmations,
    this.color,
    this.iconId,
  });

  /// Convert to JSON for caching
  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'name': name,
      'isDefault': isDefault,
      'affirmations': affirmations.map((a) => a.toJson()).toList(),
      'color': color?.value,
      'iconId': iconId,
    };
  }

  /// Create from JSON for caching
  factory AffirmationCategory.fromJson(Map<String, dynamic> json) {
    return AffirmationCategory(
      id: json['id'] ?? '',
      name: json['name'] ?? '',
      isDefault: json['isDefault'] ?? false,
      affirmations: (json['affirmations'] as List?)
          ?.map((a) => AffirmationItem.fromJson(a))
          .toList() ?? [],
      color: json['color'] != null ? Color(json['color']) : null,
      iconId: json['iconId'],
    );
  }

  /// Create a copy with updated fields
  AffirmationCategory copyWith({
    String? id,
    String? name,
    bool? isDefault,
    List<AffirmationItem>? affirmations,
    Color? color,
    int? iconId,
  }) {
    return AffirmationCategory(
      id: id ?? this.id,
      name: name ?? this.name,
      isDefault: isDefault ?? this.isDefault,
      affirmations: affirmations ?? this.affirmations,
      color: color ?? this.color,
      iconId: iconId ?? this.iconId,
    );
  }

  @override
  bool operator ==(Object other) {
    if (identical(this, other)) return true;
    
    return other is AffirmationCategory &&
        other.id == id &&
        other.name == name &&
        other.isDefault == isDefault;
  }

  @override
  int get hashCode {
    return id.hashCode ^ name.hashCode ^ isDefault.hashCode;
  }

  @override
  String toString() {
    return 'AffirmationCategory(id: $id, name: $name, isDefault: $isDefault, affirmations: ${affirmations.length})';
  }
}