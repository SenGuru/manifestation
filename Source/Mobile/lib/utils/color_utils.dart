import 'package:flutter/material.dart';

/// Utility class for color conversion and manipulation
class ColorUtils {
  /// Converts an integer color value to a hex string
  /// 
  /// Example:
  /// - intToHex(0xFF4CAF50) returns "#4CAF50"
  /// - intToHex(0xFFFF0000) returns "#FF0000"
  /// - intToHex(0xFF000000) returns "#000000"
  static String intToHex(int colorValue) {
    return '#${colorValue.toRadixString(16).padLeft(8, '0').substring(2).toUpperCase()}';
  }

  /// Converts a hex string to an integer color value
  /// 
  /// Example:
  /// - hexToInt("#4CAF50") returns 0xFF4CAF50
  /// - hexToInt("#FF0000") returns 0xFFFF0000
  /// - hexToInt("4CAF50") returns 0xFF4CAF50 (# is optional)
  static int hexToInt(String hex) {
    String hexColor = hex.replaceAll('#', '');
    if (hexColor.length == 6) {
      hexColor = 'FF$hexColor'; // Add alpha if not present
    }
    return int.parse(hexColor, radix: 16);
  }

  /// Converts a Flutter Color object to a hex string
  /// 
  /// Example:
  /// - colorToHex(Colors.green) returns "#4CAF50"
  /// - colorToHex(Colors.red) returns "#F44336"
  static String colorToHex(Color color) {
    return intToHex(color.value);
  }

  /// Converts a hex string to a Flutter Color object
  /// 
  /// Example:
  /// - hexToColor("#4CAF50") returns Color(0xFF4CAF50)
  /// - hexToColor("#FF0000") returns Color(0xFFFF0000)
  static Color hexToColor(String hex) {
    return Color(hexToInt(hex));
  }

  /// Gets a readable text color (black or white) based on background color
  /// 
  /// Example:
  /// - getTextColor(Colors.white) returns Colors.black
  /// - getTextColor(Colors.black) returns Colors.white
  /// - getTextColor(Colors.blue) returns Colors.white
  static Color getTextColor(Color backgroundColor) {
    // Calculate luminance
    double luminance = (0.299 * backgroundColor.red + 
                       0.587 * backgroundColor.green + 
                       0.114 * backgroundColor.blue) / 255;
    
    // Return black text for light backgrounds, white text for dark backgrounds
    return luminance > 0.5 ? Colors.black : Colors.white;
  }

  /// Generates a list of predefined category colors
  static List<Color> getPredefinedCategoryColors() {
    return [
      const Color(0xFF4CAF50), // Green
      const Color(0xFF2196F3), // Blue
      const Color(0xFFFF9800), // Orange
      const Color(0xFF9C27B0), // Purple
      const Color(0xFFE91E63), // Pink
      const Color(0xFF607D8B), // Blue Grey
      const Color(0xFF795548), // Brown
      const Color(0xFF3F51B5), // Indigo
      const Color(0xFF009688), // Teal
      const Color(0xFFCDDC39), // Lime
      const Color(0xFFFF5722), // Deep Orange
      const Color(0xFF673AB7), // Deep Purple
    ];
  }

  /// Gets a random color from the predefined category colors
  static Color getRandomCategoryColor() {
    final colors = getPredefinedCategoryColors();
    return colors[DateTime.now().millisecondsSinceEpoch % colors.length];
  }
}