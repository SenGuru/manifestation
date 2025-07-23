import 'package:flutter/material.dart';

// Core meditation data model
class Meditation {
  final MeditationType type;
  final String title;
  final String description;
  final int duration; // in minutes
  final String instructor;
  final String audioPath;
  final List<String>? tags;

  const Meditation({
    required this.type,
    required this.title,
    required this.description,
    required this.duration,
    required this.instructor,
    required this.audioPath,
    this.tags,
  });

  Map<String, dynamic> toJson() {
    return {
      'type': type.index,
      'title': title,
      'description': description,
      'duration': duration,
      'instructor': instructor,
      'audioPath': audioPath,
      'tags': tags,
    };
  }

  factory Meditation.fromJson(Map<String, dynamic> json) {
    return Meditation(
      type: MeditationType.values[json['type']],
      title: json['title'],
      description: json['description'],
      duration: json['duration'],
      instructor: json['instructor'],
      audioPath: json['audioPath'],
      tags: json['tags']?.cast<String>(),
    );
  }
}

// Meditation session tracking
class MeditationSession {
  final String id;
  final MeditationType type;
  final VisualType visualType;
  final int plannedDuration; // in minutes
  final int actualDuration; // in minutes
  final DateTime startTime;
  final DateTime? endTime;
  final bool completed;
  final bool usedBreathingGuide;
  final Map<String, dynamic>? visualSettings; // Store visual-specific settings

  const MeditationSession({
    required this.id,
    required this.type,
    required this.visualType,
    required this.plannedDuration,
    required this.actualDuration,
    required this.startTime,
    this.endTime,
    required this.completed,
    required this.usedBreathingGuide,
    this.visualSettings,
  });

  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'type': type.index,
      'visualType': visualType.index,
      'plannedDuration': plannedDuration,
      'actualDuration': actualDuration,
      'startTime': startTime.toIso8601String(),
      'endTime': endTime?.toIso8601String(),
      'completed': completed,
      'usedBreathingGuide': usedBreathingGuide,
      'visualSettings': visualSettings,
    };
  }

  factory MeditationSession.fromJson(Map<String, dynamic> json) {
    return MeditationSession(
      id: json['id'],
      type: MeditationType.values[json['type']],
      visualType: VisualType.values[json['visualType']],
      plannedDuration: json['plannedDuration'],
      actualDuration: json['actualDuration'],
      startTime: DateTime.parse(json['startTime']),
      endTime: json['endTime'] != null ? DateTime.parse(json['endTime']) : null,
      completed: json['completed'],
      usedBreathingGuide: json['usedBreathingGuide'],
      visualSettings: json['visualSettings']?.cast<String, dynamic>(),
    );
  }
}

// Meditation types enum
enum MeditationType {
  manifestation,
  breathing,
  chakra,
  gratitude,
  visualization,
  mindfulness,
}

extension MeditationTypeExtension on MeditationType {
  String get name {
    switch (this) {
      case MeditationType.manifestation:
        return 'Manifestation';
      case MeditationType.breathing:
        return 'Breathing';
      case MeditationType.chakra:
        return 'Chakra';
      case MeditationType.gratitude:
        return 'Gratitude';
      case MeditationType.visualization:
        return 'Visualization';
      case MeditationType.mindfulness:
        return 'Mindfulness';
    }
  }

  String get description {
    switch (this) {
      case MeditationType.manifestation:
        return 'Focus on bringing your desires into reality';
      case MeditationType.breathing:
        return 'Center yourself through conscious breath';
      case MeditationType.chakra:
        return 'Balance and align your energy centers';
      case MeditationType.gratitude:
        return 'Cultivate appreciation and abundance';
      case MeditationType.visualization:
        return 'Create vivid mental images of your goals';
      case MeditationType.mindfulness:
        return 'Present moment awareness and acceptance';
    }
  }

  IconData get icon {
    switch (this) {
      case MeditationType.manifestation:
        return Icons.auto_awesome;
      case MeditationType.breathing:
        return Icons.air;
      case MeditationType.chakra:
        return Icons.spa;
      case MeditationType.gratitude:
        return Icons.favorite;
      case MeditationType.visualization:
        return Icons.visibility;
      case MeditationType.mindfulness:
        return Icons.self_improvement;
    }
  }
}

// Visual types enum
enum VisualType {
  candle,
  moon,
  nature,
  minimal,
  geometric,
}

extension VisualTypeExtension on VisualType {
  String get name {
    switch (this) {
      case VisualType.candle:
        return 'Candle';
      case VisualType.moon:
        return 'Moon';
      case VisualType.nature:
        return 'Nature';
      case VisualType.minimal:
        return 'Minimal';
      case VisualType.geometric:
        return 'Geometric';
    }
  }

  String get description {
    switch (this) {
      case VisualType.candle:
        return 'Warm, flickering flame';
      case VisualType.moon:
        return 'Celestial moonlight';
      case VisualType.nature:
        return 'Flowing water ripples';
      case VisualType.minimal:
        return 'Simple breathing circle';
      case VisualType.geometric:
        return 'Sacred geometry patterns';
    }
  }

  IconData get icon {
    switch (this) {
      case VisualType.candle:
        return Icons.local_fire_department;
      case VisualType.moon:
        return Icons.nightlight_round;
      case VisualType.nature:
        return Icons.water;
      case VisualType.minimal:
        return Icons.circle_outlined;
      case VisualType.geometric:
        return Icons.filter_vintage;
    }
  }
}

// Candle specific enums
enum CandleColor {
  warm,
  cool,
  purple,
  gold,
  white,
}

extension CandleColorExtension on CandleColor {
  String get name {
    switch (this) {
      case CandleColor.warm:
        return 'Warm';
      case CandleColor.cool:
        return 'Cool';
      case CandleColor.purple:
        return 'Purple';
      case CandleColor.gold:
        return 'Gold';
      case CandleColor.white:
        return 'White';
    }
  }

  List<Color> get flameGradient {
    switch (this) {
      case CandleColor.warm:
        return [
          Color(0xFFFFD700), // Gold
          Color(0xFFFF6B35), // Orange-red
          Color(0xFFFF4500), // Red-orange
        ];
      case CandleColor.cool:
        return [
          Color(0xFF87CEEB), // Sky blue
          Color(0xFF4682B4), // Steel blue
          Color(0xFF191970), // Midnight blue
        ];
      case CandleColor.purple:
        return [
          Color(0xFFDDA0DD), // Plum
          Color(0xFF9370DB), // Medium purple
          Color(0xFF4B0082), // Indigo
        ];
      case CandleColor.gold:
        return [
          Color(0xFFFFFFE0), // Light yellow
          Color(0xFFFFD700), // Gold
          Color(0xFFB8860B), // Dark goldenrod
        ];
      case CandleColor.white:
        return [
          Color(0xFFF0F8FF), // Alice blue
          Color(0xFFE0E0E0), // Light gray
          Color(0xFFC0C0C0), // Silver
        ];
    }
  }

  Color get primaryColor {
    return flameGradient[1];
  }

  String get mood {
    switch (this) {
      case CandleColor.warm:
        return 'Comfort & Energy';
      case CandleColor.cool:
        return 'Calm & Peace';
      case CandleColor.purple:
        return 'Spiritual & Mystical';
      case CandleColor.gold:
        return 'Abundance & Success';
      case CandleColor.white:
        return 'Purity & Clarity';
    }
  }
}

// Moon phases enum
enum MoonPhase {
  newMoon,
  waxingCrescent,
  firstQuarter,
  waxingGibbous,
  fullMoon,
  waningGibbous,
  lastQuarter,
  waningCrescent,
}

extension MoonPhaseExtension on MoonPhase {
  String get name {
    switch (this) {
      case MoonPhase.newMoon:
        return 'New Moon';
      case MoonPhase.waxingCrescent:
        return 'Waxing Crescent';
      case MoonPhase.firstQuarter:
        return 'First Quarter';
      case MoonPhase.waxingGibbous:
        return 'Waxing Gibbous';
      case MoonPhase.fullMoon:
        return 'Full Moon';
      case MoonPhase.waningGibbous:
        return 'Waning Gibbous';
      case MoonPhase.lastQuarter:
        return 'Last Quarter';
      case MoonPhase.waningCrescent:
        return 'Waning Crescent';
    }
  }

  double get illumination {
    switch (this) {
      case MoonPhase.newMoon:
        return 0.0;
      case MoonPhase.waxingCrescent:
      case MoonPhase.waningCrescent:
        return 0.25;
      case MoonPhase.firstQuarter:
      case MoonPhase.lastQuarter:
        return 0.5;
      case MoonPhase.waxingGibbous:
      case MoonPhase.waningGibbous:
        return 0.75;
      case MoonPhase.fullMoon:
        return 1.0;
    }
  }

  List<Color> get moonGradient {
    switch (this) {
      case MoonPhase.newMoon:
        return [
          Color(0xFF2D3748),
          Color(0xFF1A202C),
          Color(0xFF0F1419),
        ];
      case MoonPhase.waxingCrescent:
      case MoonPhase.waningCrescent:
        return [
          Color(0xFFF7FAFC),
          Color(0xFFE2E8F0),
          Color(0xFFCBD5E0),
        ];
      case MoonPhase.firstQuarter:
      case MoonPhase.lastQuarter:
        return [
          Color(0xFFFFF5F5),
          Color(0xFFFED7D7),
          Color(0xFFFBB6CE),
        ];
      case MoonPhase.waxingGibbous:
      case MoonPhase.waningGibbous:
        return [
          Color(0xFFFFFAF0),
          Color(0xFFFEEBC8),
          Color(0xFFFBD38D),
        ];
      case MoonPhase.fullMoon:
        return [
          Color(0xFFFFFFFF),
          Color(0xFFF7FAFC),
          Color(0xFFE2E8F0),
        ];
    }
  }

  Color get glowColor {
    switch (this) {
      case MoonPhase.newMoon:
        return Color(0xFF4A5568);
      case MoonPhase.waxingCrescent:
      case MoonPhase.waningCrescent:
        return Color(0xFFE2E8F0);
      case MoonPhase.firstQuarter:
      case MoonPhase.lastQuarter:
        return Color(0xFFFED7D7);
      case MoonPhase.waxingGibbous:
      case MoonPhase.waningGibbous:
        return Color(0xFFFEEBC8);
      case MoonPhase.fullMoon:
        return Color(0xFFFFFFFF);
    }
  }
}

// Nature patterns enum
enum NaturePattern {
  ripples,
  waves,
  flow,
  rain,
}

extension NaturePatternExtension on NaturePattern {
  String get name {
    switch (this) {
      case NaturePattern.ripples:
        return 'Ripples';
      case NaturePattern.waves:
        return 'Waves';
      case NaturePattern.flow:
        return 'Flow';
      case NaturePattern.rain:
        return 'Rain';
    }
  }

  List<Color> get colors {
    switch (this) {
      case NaturePattern.ripples:
        return [Color(0xFF06B6D4), Color(0xFF0891B2)];
      case NaturePattern.waves:
        return [Color(0xFF3B82F6), Color(0xFF1D4ED8)];
      case NaturePattern.flow:
        return [Color(0xFF10B981), Color(0xFF059669)];
      case NaturePattern.rain:
        return [Color(0xFF6366F1), Color(0xFF4F46E5)];
    }
  }
}

// Geometric patterns enum
enum GeometricPattern {
  mandala,
  flowerOfLife,
  spirals,
  fractals,
}

extension GeometricPatternExtension on GeometricPattern {
  String get name {
    switch (this) {
      case GeometricPattern.mandala:
        return 'Mandala';
      case GeometricPattern.flowerOfLife:
        return 'Flower of Life';
      case GeometricPattern.spirals:
        return 'Spirals';
      case GeometricPattern.fractals:
        return 'Fractals';
    }
  }

  List<Color> get colors {
    switch (this) {
      case GeometricPattern.mandala:
        return [Color(0xFF8B5CF6), Color(0xFFA855F7)];
      case GeometricPattern.flowerOfLife:
        return [Color(0xFFEC4899), Color(0xFFF97316)];
      case GeometricPattern.spirals:
        return [Color(0xFF06B6D4), Color(0xFF8B5CF6)];
      case GeometricPattern.fractals:
        return [Color(0xFFF59E0B), Color(0xFFEF4444)];
    }
  }
}