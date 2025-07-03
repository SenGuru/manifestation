import 'package:flutter/material.dart';
import 'package:manifestation_app/meditation_models.dart';

class AppConstants {
  // Screen dimensions for iPhone 15 Pro Max
  static const double screenWidth = 430.0;  // Points
  static const double screenHeight = 932.0; // Points
  static const double statusBarHeight = 47.0;
  static const double homeIndicatorHeight = 34.0;
  
  // Animation durations
  static const Duration fastAnimation = Duration(milliseconds: 200);
  static const Duration normalAnimation = Duration(milliseconds: 300);
  static const Duration slowAnimation = Duration(milliseconds: 500);
  static const Duration breathingCycle = Duration(seconds: 8); // 4 sec in, 4 sec out
  
  // Visual animation timings
  static const Duration candleFlicker = Duration(milliseconds: 2000);
  static const Duration candleGlow = Duration(milliseconds: 3000);
  static const Duration moonGlow = Duration(milliseconds: 4000);
  static const Duration moonClouds = Duration(milliseconds: 15000);
  static const Duration natureWaves = Duration(milliseconds: 2500);
  static const Duration minimalBreathing = Duration(milliseconds: 4000);
  static const Duration geometricRotation = Duration(milliseconds: 6000);
  
  // Sizes for different visual elements
  static const double previewSize = 140.0;
  static const double activeSessionSize = 300.0;
  static const double iconSize = 24.0;
  static const double smallIconSize = 16.0;
  
  // Spacing
  static const double paddingSmall = 8.0;
  static const double paddingMedium = 16.0;
  static const double paddingLarge = 20.0;
  static const double paddingXLarge = 24.0;
  
  // Border radius
  static const double radiusSmall = 8.0;
  static const double radiusMedium = 12.0;
  static const double radiusLarge = 16.0;
  static const double radiusXLarge = 20.0;
  static const double radiusCircular = 999.0;
  
  // Available durations (in minutes)
  static const List<int> meditationDurations = [5, 10, 15, 20, 30, 60];
  
  // Default meditation sessions
  static const List<Meditation> defaultMeditations = [
    Meditation(
      type: MeditationType.manifestation,
      title: 'Manifestation Focus',
      description: 'Visualize your dreams becoming reality',
      duration: 10,
      instructor: 'Sarah Chen',
      audioPath: 'assets/audio/manifestation_10min.mp3',
      tags: ['abundance', 'goals', 'visualization'],
    ),
    Meditation(
      type: MeditationType.breathing,
      title: 'Deep Breathing',
      description: 'Center yourself through conscious breath',
      duration: 5,
      instructor: 'Michael Roberts',
      audioPath: 'assets/audio/breathing_5min.mp3',
      tags: ['calm', 'focus', 'stress-relief'],
    ),
    Meditation(
      type: MeditationType.chakra,
      title: 'Chakra Alignment',
      description: 'Balance and align your energy centers',
      duration: 15,
      instructor: 'Luna Martinez',
      audioPath: 'assets/audio/chakra_15min.mp3',
      tags: ['energy', 'balance', 'healing'],
    ),
    Meditation(
      type: MeditationType.gratitude,
      title: 'Gratitude Practice',
      description: 'Cultivate appreciation and abundance',
      duration: 8,
      instructor: 'David Thompson',
      audioPath: 'assets/audio/gratitude_8min.mp3',
      tags: ['thankfulness', 'positivity', 'abundance'],
    ),
    Meditation(
      type: MeditationType.visualization,
      title: 'Creative Visualization',
      description: 'Paint vivid pictures of your ideal future',
      duration: 12,
      instructor: 'Emma Wilson',
      audioPath: 'assets/audio/visualization_12min.mp3',
      tags: ['creativity', 'imagination', 'goals'],
    ),
    Meditation(
      type: MeditationType.mindfulness,
      title: 'Present Moment',
      description: 'Embrace the here and now with awareness',
      duration: 10,
      instructor: 'James Lee',
      audioPath: 'assets/audio/mindfulness_10min.mp3',
      tags: ['awareness', 'present', 'acceptance'],
    ),
  ];
  
  // Visual configuration defaults
  static const Map<VisualType, Map<String, dynamic>> visualDefaults = {
    VisualType.candle: {
      'defaultColor': CandleColor.warm,
      'intensityRange': [0.3, 1.0],
      'flickerIntensity': 0.5,
    },
    VisualType.moon: {
      'defaultPhase': MoonPhase.fullMoon,
      'intensityRange': [0.4, 1.0],
      'glowIntensity': 0.7,
    },
    VisualType.nature: {
      'defaultPattern': NaturePattern.ripples,
      'intensityRange': [0.3, 1.0],
      'waveSpeed': 0.6,
    },
    VisualType.minimal: {
      'intensityRange': [0.5, 1.0],
      'breathingSpeed': 0.8,
      'strokeWidth': 3.0,
    },
    VisualType.geometric: {
      'defaultPattern': GeometricPattern.mandala,
      'intensityRange': [0.4, 1.0],
      'rotationSpeed': 0.3,
    },
  };
  
  // Text styles base configurations
  static const Map<String, Map<String, dynamic>> textStyleConfigs = {
    'headline': {
      'fontSize': 24.0,
      'fontWeight': FontWeight.bold,
      'letterSpacing': -0.5,
    },
    'title': {
      'fontSize': 18.0,
      'fontWeight': FontWeight.w600,
      'letterSpacing': -0.2,
    },
    'body': {
      'fontSize': 16.0,
      'fontWeight': FontWeight.normal,
      'letterSpacing': 0.0,
    },
    'caption': {
      'fontSize': 14.0,
      'fontWeight': FontWeight.w500,
      'letterSpacing': 0.1,
    },
    'small': {
      'fontSize': 12.0,
      'fontWeight': FontWeight.normal,
      'letterSpacing': 0.2,
    },
  };
  
  // Audio settings
  static const Map<String, dynamic> audioSettings = {
    'fadeInDuration': 2000, // milliseconds
    'fadeOutDuration': 3000, // milliseconds
    'defaultVolume': 0.7,
    'backgroundMusicVolume': 0.3,
  };
  
  // Session tracking
  static const Map<String, dynamic> sessionSettings = {
    'autoSave': true,
    'minSessionDuration': 60, // seconds - minimum to count as completed
    'preparationTime': 10, // seconds before session starts
    'completionCelebrationTime': 3, // seconds
  };
  
  // Visual intensity mappings
  static double getPreviewIntensity(VisualType visualType) {
    switch (visualType) {
      case VisualType.candle:
        return 0.6;
      case VisualType.moon:
        return 0.8;
      case VisualType.nature:
        return 0.7;
      case VisualType.minimal:
        return 0.7;
      case VisualType.geometric:
        return 0.7;
    }
  }
  
  static double getActiveIntensity(bool isPlaying) {
    return isPlaying ? 1.0 : 0.3;
  }
  
  // Responsive scaling factors for iPhone 15 Pro Max
  static const Map<String, double> scalingFactors = {
    'text': 1.0,
    'icon': 1.0,
    'spacing': 1.0,
    'visual': 1.0,
  };
  
  // Haptic feedback settings
  static const Map<String, bool> hapticSettings = {
    'selectionFeedback': true,
    'impactFeedback': true,
    'notificationFeedback': true,
    'sessionStart': true,
    'sessionEnd': true,
  };
  
  // Performance settings
  static const Map<String, dynamic> performanceSettings = {
    'maxAnimationFPS': 60,
    'reduceMotionRespect': true,
    'highQualityAnimations': true,
    'preloadAnimations': true,
  };
}