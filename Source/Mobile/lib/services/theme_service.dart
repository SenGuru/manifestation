import 'package:flutter/material.dart';
import 'package:shared_preferences/shared_preferences.dart';

enum AppThemeMode { light, dark }

class ThemeService extends ChangeNotifier {
  static final ThemeService _instance = ThemeService._internal();
  factory ThemeService() => _instance;
  ThemeService._internal();

  static const String _themeKey = 'app_theme_mode';
  AppThemeMode _currentTheme = AppThemeMode.dark; // Default to dark mode
  bool _isInitialized = false;

  AppThemeMode get currentTheme => _currentTheme;
  bool get isDarkMode => _currentTheme == AppThemeMode.dark;
  bool get isLightMode => _currentTheme == AppThemeMode.light;

  /// Initialize theme service and load saved preference
  Future<void> initialize() async {
    if (_isInitialized) return;
    
    try {
      final prefs = await SharedPreferences.getInstance();
      final savedTheme = prefs.getString(_themeKey);
      
      if (savedTheme != null) {
        _currentTheme = AppThemeMode.values.firstWhere(
          (theme) => theme.toString() == savedTheme,
          orElse: () => AppThemeMode.dark,
        );
      }
      
      _isInitialized = true;
      print('🎨 ThemeService initialized: ${_currentTheme.name} mode');
    } catch (e) {
      print('Error initializing theme service: $e');
      _currentTheme = AppThemeMode.dark;
      _isInitialized = true;
    }
  }

  /// Toggle between light and dark mode
  Future<void> toggleTheme() async {
    final newTheme = _currentTheme == AppThemeMode.dark 
        ? AppThemeMode.light 
        : AppThemeMode.dark;
    
    await setTheme(newTheme);
  }

  /// Set specific theme mode
  Future<void> setTheme(AppThemeMode theme) async {
    if (_currentTheme == theme) return;
    
    _currentTheme = theme;
    notifyListeners();
    
    try {
      final prefs = await SharedPreferences.getInstance();
      await prefs.setString(_themeKey, theme.toString());
      print('🎨 Theme changed to: ${theme.name} mode');
    } catch (e) {
      print('Error saving theme preference: $e');
    }
  }

  /// Reset to default theme (dark mode)
  Future<void> resetToDefault() async {
    await setTheme(AppThemeMode.dark);
  }
}