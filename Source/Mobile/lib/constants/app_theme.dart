import 'package:flutter/material.dart';
import '../services/theme_service.dart';

class AppTheme {
  static final ThemeService _themeService = ThemeService();
  
  // Get current theme mode
  static bool get isDarkMode {
    final isDark = _themeService.isDarkMode;
    print('🎨 AppTheme.isDarkMode called, returning: $isDark');
    return isDark;
  }
  static bool get isLightMode => _themeService.isLightMode;
  
  // =================== BACKGROUND COLORS ===================
  static Color get backgroundPrimary => isDarkMode 
      ? const Color(0xFF1A1A2E)    // Dark background
      : const Color(0xFFFFFFFF);   // Pure white background
      
  static Color get backgroundSecondary => isDarkMode 
      ? const Color(0xFF16213E)    // Medium dark
      : const Color(0xFFF8FAFC);   // Very light gray secondary
      
  static Color get backgroundTertiary => isDarkMode 
      ? const Color(0xFF252A4A)    // Light dark
      : const Color(0xFFF1F5F9);   // Light gray tertiary
      
  static Color get cardBackground => isDarkMode 
      ? const Color(0xFF252A4A)    // Dark card
      : const Color(0xFFFFFFFF);   // Pure white card
      
  static Color get surfaceElevated => isDarkMode 
      ? const Color(0xFF2D3748)    // Elevated dark surface
      : const Color(0xFFF8FAFC);   // Elevated light surface
  
  // =================== TEXT COLORS ===================
  static Color get textPrimary => isDarkMode 
      ? const Color(0xFFE5E7EB)    // Light text on dark
      : const Color(0xFF111827);   // Very dark text on light (better contrast)
      
  static Color get textSecondary => isDarkMode 
      ? const Color(0xFFD1D5DB)    // Medium light text
      : const Color(0xFF374151);   // Darker medium text on light
      
  static Color get textTertiary => isDarkMode 
      ? const Color(0xFF9CA3AF)    // Darker text
      : const Color(0xFF6B7280);   // Medium gray text
      
  static Color get textDisabled => isDarkMode 
      ? const Color(0xFF6B7280)    // Disabled dark text
      : const Color(0xFF9CA3AF);   // Disabled light text
  
  // =================== BORDER COLORS ===================
  static Color get borderPrimary => isDarkMode 
      ? textSecondary.withValues(alpha: 0.2)  // Subtle border on dark
      : const Color(0xFFE5E7EB);               // Light gray border on light
      
  static Color get borderSecondary => isDarkMode 
      ? textTertiary.withValues(alpha: 0.1)   // Very subtle on dark
      : const Color(0xFFF3F4F6);               // Very light border on light
      
  static Color get divider => isDarkMode 
      ? textSecondary.withValues(alpha: 0.1)  // Divider on dark
      : const Color(0xFFE5E7EB);               // Light gray divider on light
  
  // =================== INTERACTIVE COLORS ===================
  static Color get buttonBackground => isDarkMode 
      ? const Color(0xFF374151)    // Dark button
      : const Color(0xFFF9FAFB);   // Very light button
      
  static Color get buttonBackgroundPressed => isDarkMode 
      ? const Color(0xFF4B5563)    // Pressed dark button
      : const Color(0xFFF3F4F6);   // Pressed light button
      
  static Color get inputBackground => isDarkMode 
      ? cardBackground             // Same as card on dark
      : const Color(0xFFFFFFFF);   // Pure white input on light
      
  static Color get inputBorder => isDarkMode 
      ? borderPrimary              // Standard border
      : const Color(0xFFD1D5DB);   // Medium gray border on light
  
  // =================== STATUS COLORS (SAME FOR BOTH THEMES) ===================
  static const Color success = Color(0xFF10B981);          // Success green
  static const Color warning = Color(0xFFF59E0B);          // Warning amber
  static const Color error = Color(0xFFEF4444);            // Error red
  static const Color info = Color(0xFF3B82F6);             // Info blue
  
  // =================== GRADIENT COLORS (SAME FOR BOTH THEMES) ===================
  static List<Color> get affirmationGradient => [
    const Color(0xFFFF6B9D),
    const Color(0xFFFF8A50),
  ];
  
  static List<Color> get visionBoardGradient => [
    const Color(0xFF06B6D4),
    const Color(0xFF4ECDC4),
  ];
  
  static List<Color> get journalGradient => [
    const Color(0xFF8B5CF6),
    const Color(0xFFC084FC),
  ];
  
  static List<Color> get visualMeditationGradient => [
    const Color(0xFFcc2b5e),
    const Color(0xFF753a88),
  ];
  
  static List<Color> get goalTrackerGradient => [
    const Color(0xFF3B82F6),
    const Color(0xFF6366F1),
  ];
  
  static List<Color> get analyticsGradient => [
    const Color(0xFF3B82F6),
    const Color(0xFF8B5CF6),
  ];
  
  static List<Color> get socialGradient => [
    const Color(0xFF34D399),
    const Color(0xFF06B6D4),
  ];
  
  // =================== ACCENT COLORS ===================
  static const Color accentGold = Color(0xFFFBBF24);       // Bright gold
  static const Color accentLavender = Color(0xFFC084FC);   // Lavender
  static const Color accentMint = Color(0xFF34D399);       // Mint green
  static const Color accentCoral = Color(0xFFFF6B9D);      // Coral
  
  // =================== BASIC COLORS ===================
  static const Color white = Color(0xFFFFFFFF);
  static const Color black = Color(0xFF000000);
  static const Color transparent = Color(0x00000000);
  
  // =================== SHADOW & OVERLAY ===================
  static Color get shadow => isDarkMode 
      ? black.withValues(alpha: 0.4)     // Darker shadow on dark theme
      : black.withValues(alpha: 0.2);    // Lighter shadow on light theme
      
  static Color get overlay => isDarkMode 
      ? black.withValues(alpha: 0.6)     // Dark overlay
      : black.withValues(alpha: 0.4);    // Light overlay
      
  static Color get shimmer => isDarkMode 
      ? white.withValues(alpha: 0.1)     // Light shimmer on dark
      : white.withValues(alpha: 0.8);    // Strong shimmer on light
  
  // =================== THEME-AWARE HELPERS ===================
  
  /// Get appropriate text color for a given background
  static Color getContrastingTextColor(Color backgroundColor) {
    final luminance = backgroundColor.computeLuminance();
    return luminance > 0.5 ? black : white;
  }
  
  /// Get theme-aware icon color
  static Color get iconPrimary => isDarkMode 
      ? textSecondary 
      : textSecondary;  // Use secondary text color for better contrast
      
  static Color get iconSecondary => isDarkMode 
      ? textTertiary 
      : textTertiary;   // Use tertiary text color
  
  /// Get theme-aware splash color for buttons
  static Color get splashColor => isDarkMode 
      ? white.withValues(alpha: 0.1) 
      : black.withValues(alpha: 0.1);
      
  /// Get theme-aware highlight color
  static Color get highlightColor => isDarkMode 
      ? white.withValues(alpha: 0.05) 
      : black.withValues(alpha: 0.05);
  
  // =================== BACKWARDS COMPATIBILITY ===================
  // These getters maintain compatibility with existing AppColors usage
  
  static Color get backgroundDark => backgroundPrimary;
  static Color get backgroundMedium => backgroundSecondary;
  static Color get backgroundLight => backgroundTertiary;
  static Color get textLight => textPrimary;
  static Color get textMedium => textSecondary;
  static Color get textDark => textTertiary;
  
  // =================== THEME DATA GENERATION ===================
  
  /// Generate ThemeData for the current theme
  static ThemeData get themeData => ThemeData(
    brightness: isDarkMode ? Brightness.dark : Brightness.light,
    scaffoldBackgroundColor: backgroundPrimary,
    primaryColor: affirmationGradient.first,
    
    // Card theme
    cardTheme: CardThemeData(
      color: cardBackground,
      elevation: 0,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(16),
        side: BorderSide(color: borderSecondary),
      ),
    ),
    
    // App bar theme
    appBarTheme: AppBarTheme(
      backgroundColor: backgroundPrimary,
      foregroundColor: textPrimary,
      elevation: 0,
      centerTitle: true,
    ),
    
    // Icon theme
    iconTheme: IconThemeData(
      color: iconPrimary,
    ),
    
    // Divider theme
    dividerTheme: DividerThemeData(
      color: divider,
      thickness: 1,
    ),
    
    // Bottom navigation theme
    bottomNavigationBarTheme: BottomNavigationBarThemeData(
      backgroundColor: cardBackground,
      selectedItemColor: affirmationGradient.first,
      unselectedItemColor: iconSecondary,
      type: BottomNavigationBarType.fixed,
    ),
    
    // Text selection theme
    textSelectionTheme: TextSelectionThemeData(
      cursorColor: affirmationGradient.first,
      selectionColor: affirmationGradient.first.withValues(alpha: 0.3),
      selectionHandleColor: affirmationGradient.first,
    ),
    
    // Color scheme
    colorScheme: ColorScheme.fromSeed(
      seedColor: affirmationGradient.first,
      brightness: isDarkMode ? Brightness.dark : Brightness.light,
      surface: cardBackground,
      onSurface: textPrimary,
    ),
  );
}