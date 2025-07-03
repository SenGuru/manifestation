import 'package:flutter/material.dart';

class AppColors {
  // Primary Colors - Deep, grounding tones
  static const Color primaryDeep = Color(0xFF2D1B69);      // Deep purple
  static const Color primaryMedium = Color(0xFF4A3A8C);    // Medium purple
  static const Color primaryLight = Color(0xFF6B5AC7);     // Light purple
  
  // Secondary Colors - Warm, manifestation energy
  static const Color secondaryGold = Color(0xFFD4AF37);    // Manifestation gold
  static const Color secondaryRose = Color(0xFFE8A5A5);    // Soft rose
  static const Color secondaryTeal = Color(0xFF4ECDC4);    // Calming teal
  
  // Gradient Colors for Cards
  static const Color gradientPink = Color(0xFFFF6B9D);     // Vibrant pink
  static const Color gradientOrange = Color(0xFFFF8A50);   // Warm orange
  static const Color gradientPurple = Color(0xFF8B5CF6);   // Purple
  static const Color gradientBlue = Color(0xFF3B82F6);     // Blue
  static const Color gradientTeal = Color(0xFF06B6D4);     // Teal
  static const Color gradientIndigo = Color(0xFF6366F1);   // Indigo
  
  // Neutral Colors
  static const Color backgroundDark = Color(0xFF1A1A2E);   // Dark background
  static const Color backgroundMedium = Color(0xFF16213E);  // Medium dark
  static const Color backgroundLight = Color(0xFF252A4A);   // Light dark
  static const Color textLight = Color(0xFFE5E7EB);        // Light text
  static const Color textMedium = Color(0xFFD1D5DB);       // Medium text
  static const Color textDark = Color(0xFF9CA3AF);         // Dark text
  static const Color cardBackground = Color(0xFF252A4A);    // Card background
  
  // Accent Colors
  static const Color accentGold = Color(0xFFFBBF24);       // Bright gold
  static const Color accentLavender = Color(0xFFC084FC);   // Lavender
  static const Color accentMint = Color(0xFF34D399);       // Mint green
  static const Color accentCoral = Color(0xFFFF6B9D);      // Coral
  
  // Basic Colors (commonly used)
  static const Color white = Color(0xFFFFFFFF);            // Pure white
  static const Color black = Color(0xFF000000);            // Pure black
  static const Color transparent = Color(0x00000000);      // Transparent
  
  // Special gradient combinations for feature cards
  static List<Color> affirmationGradient = [
    Color(0xFFFF6B9D),
    Color(0xFFFF8A50),
  ];
  
  static List<Color> visionBoardGradient = [
    Color(0xFF06B6D4),
    Color(0xFF4ECDC4),
  ];
  
  static List<Color> journalGradient = [
    Color(0xFF8B5CF6),
    Color(0xFFC084FC),
  ];
  
  static List<Color> candleMeditationGradient = [
    Color(0xFFD4AF37),
    Color(0xFFFF8A50),
  ];
  
  static List<Color> goalTrackerGradient = [
    Color(0xFF3B82F6),
    Color(0xFF6366F1),
  ];
  
  static List<Color> moonPhaseGradient = [
    Color(0xFF6366F1),
    Color(0xFF8B5CF6),
  ];
  
  static List<Color> socialGradient = [
    Color(0xFF34D399),  // Mint green
    Color(0xFF06B6D4),  // Teal
  ];
  
  // Visual Type Gradients (used in meditation screen)
  static List<Color> candleGradient = [
    Color(0xFFD4AF37), // Gold
    Color(0xFFFF8A50), // Orange
  ];
  
  static List<Color> moonGradient = [
    Color(0xFF6366F1), // Indigo
    Color(0xFF8B5CF6), // Purple
  ];
  
  static List<Color> natureGradient = [
    Color(0xFF06B6D4), // Cyan
    Color(0xFF34D399), // Emerald
  ];
  
  static List<Color> minimalGradient = [
    Color(0xFF8B5CF6), // Purple
    Color(0xFFC084FC), // Light purple
  ];
  
  static List<Color> geometricGradient = [
    Color(0xFFEC4899), // Pink
    Color(0xFFF97316), // Orange
  ];
  
  // Meditation Type Colors
  static List<Color> manifestationGradient = [
    Color(0xFFD4AF37), // Gold
    Color(0xFFFF8A50), // Orange
  ];
  
  static List<Color> breathingGradient = [
    Color(0xFF06B6D4), // Cyan
    Color(0xFF0891B2), // Blue
  ];
  
  static List<Color> chakraGradient = [
    Color(0xFF8B5CF6), // Purple
    Color(0xFFC084FC), // Light purple
  ];
  
  static List<Color> gratitudeGradient = [
    Color(0xFFFF6B9D), // Pink
    Color(0xFFFF8A50), // Orange
  ];
  
  static List<Color> visualizationGradient = [
    Color(0xFF3B82F6), // Blue
    Color(0xFF6366F1), // Indigo
  ];
  
  static List<Color> mindfulnessGradient = [
    Color(0xFF34D399), // Emerald
    Color(0xFF06B6D4), // Cyan
  ];
  
  // Status Colors
  static const Color success = Color(0xFF10B981);          // Success green
  static const Color warning = Color(0xFFF59E0B);          // Warning amber
  static const Color error = Color(0xFFEF4444);            // Error red
  static const Color info = Color(0xFF3B82F6);             // Info blue
  
  // Opacity Variants
  static Color get surfaceOverlay => white.withOpacity(0.05);
  static Color get divider => textMedium.withOpacity(0.1);
  static Color get shimmer => white.withOpacity(0.1);
  static Color get shadow => black.withOpacity(0.3);
  
  // Helper method to get gradient by visual type
  static List<Color> getVisualGradient(String visualType) {
    switch (visualType.toLowerCase()) {
      case 'candle':
        return candleGradient;
      case 'moon':
        return moonGradient;
      case 'nature':
        return natureGradient;
      case 'minimal':
        return minimalGradient;
      case 'geometric':
        return geometricGradient;
      default:
        return candleGradient;
    }
  }
  
  // Helper method to get gradient by meditation type
  static List<Color> getMeditationGradient(String meditationType) {
    switch (meditationType.toLowerCase()) {
      case 'manifestation':
        return manifestationGradient;
      case 'breathing':
        return breathingGradient;
      case 'chakra':
        return chakraGradient;
      case 'gratitude':
        return gratitudeGradient;
      case 'visualization':
        return visualizationGradient;
      case 'mindfulness':
        return mindfulnessGradient;
      default:
        return manifestationGradient;
    }
  }
  
  // Dynamic color based on time of day
  static Color getTimeBasedAccent() {
    final hour = DateTime.now().hour;
    if (hour >= 5 && hour < 12) {
      return accentGold; // Morning - Gold
    } else if (hour >= 12 && hour < 17) {
      return accentCoral; // Afternoon - Coral
    } else if (hour >= 17 && hour < 21) {
      return accentLavender; // Evening - Lavender
    } else {
      return secondaryTeal; // Night - Teal
    }
  }
}