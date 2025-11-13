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
  
  // Calendar-specific gradients
  static List<Color> calendarGradient = [
    Color(0xFF8B5CF6),    // Purple
    Color(0xFF06B6D4),    // Teal
  ];
  
  static List<Color> statsGradient = [
    Color(0xFFFBBF24),    // Gold
    Color(0xFFFF8A50),    // Orange
  ];
  
  static List<Color> analyticsGradient = [
    Color(0xFF3B82F6),    // Blue
    Color(0xFF8B5CF6),    // Purple
  ];
  
  static List<Color> visualMeditationGradient = [
    Color(0xFFcc2b5e),
    Color(0xFF753a88),
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
  
  // Journal Entry Status Colors
  static const Color entryComplete = Color(0xFF10B981);    // Green for complete entries
  static const Color entryPartial = Color(0xFFF59E0B);     // Amber for partial entries
  static const Color entryEmpty = Color(0xFF6B7280);       // Gray for empty days
  static const Color entryToday = Color(0xFFFBBF24);       // Gold for today
  
  // Mood-based colors for calendar indicators
  static const Color moodHappy = Color(0xFF10B981);        // Bright green
  static const Color moodExcited = Color(0xFFFF8A50);      // Orange
  static const Color moodContent = Color(0xFF06B6D4);      // Teal
  static const Color moodCalm = Color(0xFF8B5CF6);         // Purple
  static const Color moodNeutral = Color(0xFF6B7280);      // Gray
  static const Color moodSad = Color(0xFF3B82F6);          // Blue
  static const Color moodAnxious = Color(0xFFEF4444);      // Red
  static const Color moodOverwhelmed = Color(0xFFF59E0B);  // Amber
  static const Color moodGrateful = Color(0xFFFF6B9D);     // Pink
  static const Color moodPeaceful = Color(0xFF34D399);     // Emerald
  
  // Streak colors for analytics
  static const Color streakFire = Color(0xFFFF4500);       // Orange-red for fire
  static const Color streakGold = Color(0xFFFFD700);       // Gold for achievements
  static const Color streakSilver = Color(0xFFC0C0C0);     // Silver for milestones
  
  // Chart colors for analytics
  static List<Color> chartColors = [
    Color(0xFF8B5CF6),    // Purple
    Color(0xFF06B6D4),    // Teal
    Color(0xFFFF6B9D),    // Pink
    Color(0xFFFBBF24),    // Gold
    Color(0xFF34D399),    // Emerald
    Color(0xFFFF8A50),    // Orange
    Color(0xFF3B82F6),    // Blue
    Color(0xFFEF4444),    // Red
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
  
  // Calendar-specific opacity variants
  static Color get calendarToday => accentGold.withOpacity(0.3);
  static Color get calendarSelected => gradientPurple.withOpacity(0.2);
  static Color get calendarHover => white.withOpacity(0.1);
  
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
  
  // Helper method to get mood color
  static Color getMoodColor(String mood) {
    switch (mood.toLowerCase()) {
      case 'happy':
      case 'joyful':
        return moodHappy;
      case 'excited':
      case 'energetic':
        return moodExcited;
      case 'content':
      case 'satisfied':
        return moodContent;
      case 'calm':
      case 'relaxed':
        return moodCalm;
      case 'neutral':
      case 'okay':
        return moodNeutral;
      case 'sad':
      case 'down':
        return moodSad;
      case 'anxious':
      case 'worried':
        return moodAnxious;
      case 'overwhelmed':
      case 'stressed':
        return moodOverwhelmed;
      case 'grateful':
      case 'thankful':
        return moodGrateful;
      case 'peaceful':
      case 'serene':
        return moodPeaceful;
      default:
        return moodNeutral;
    }
  }
  
  // Helper method to get streak color based on count
  static Color getStreakColor(int streakCount) {
    if (streakCount >= 30) {
      return streakGold;
    } else if (streakCount >= 14) {
      return streakSilver;
    } else if (streakCount >= 7) {
      return streakFire;
    } else if (streakCount >= 3) {
      return accentGold;
    } else {
      return textMedium;
    }
  }
  
  // Helper method to get chart color by index
  static Color getChartColor(int index) {
    return chartColors[index % chartColors.length];
  }
  
  // Helper method to get calendar gradient for different features
  static List<Color> getCalendarGradient(String feature) {
    switch (feature.toLowerCase()) {
      case 'calendar':
        return calendarGradient;
      case 'stats':
      case 'analytics':
        return statsGradient;
      case 'journal':
        return journalGradient;
      case 'mood':
        return [moodHappy, moodCalm];
      case 'gratitude':
        return gratitudeGradient;
      default:
        return calendarGradient;
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
  
  // Method to get entry completion color
  static Color getEntryCompletionColor(bool hasEntry, bool isToday, double completionPercentage) {
    if (isToday) {
      return entryToday;
    } else if (!hasEntry) {
      return entryEmpty;
    } else if (completionPercentage >= 0.8) {
      return entryComplete;
    } else if (completionPercentage >= 0.4) {
      return entryPartial;
    } else {
      return entryEmpty;
    }
  }
  
  // Method to get gradient for time-based features
  static List<Color> getTimeBasedGradient() {
    final hour = DateTime.now().hour;
    if (hour >= 5 && hour < 12) {
      return [accentGold, gradientOrange]; // Morning
    } else if (hour >= 12 && hour < 17) {
      return [gradientOrange, accentCoral]; // Afternoon
    } else if (hour >= 17 && hour < 21) {
      return [accentLavender, gradientPurple]; // Evening
    } else {
      return [secondaryTeal, gradientTeal]; // Night
    }
  }
}