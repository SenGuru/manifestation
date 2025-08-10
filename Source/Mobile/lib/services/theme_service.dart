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
  bool _isAnimating = false;

  AppThemeMode get currentTheme => _currentTheme;
  bool get isDarkMode => _currentTheme == AppThemeMode.dark;
  bool get isLightMode => _currentTheme == AppThemeMode.light;
  bool get isAnimating => _isAnimating;

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

  /// Toggle between light and dark mode with animation
  Future<void> toggleTheme({bool animate = true}) async {
    final newTheme = _currentTheme == AppThemeMode.dark 
        ? AppThemeMode.light 
        : AppThemeMode.dark;
    
    await setTheme(newTheme, animate: animate);
  }

  /// Set specific theme mode with optional animation
  Future<void> setTheme(AppThemeMode theme, {bool animate = true}) async {
    if (_currentTheme == theme || _isAnimating) return;
    
    if (animate) {
      _isAnimating = true;
      notifyListeners();
      
      // Delay theme change to allow animation to start
      await Future.delayed(const Duration(milliseconds: 50));
    }
    
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

  /// Called when animation completes
  void completeAnimation() {
    if (_isAnimating) {
      _isAnimating = false;
      notifyListeners();
    }
  }

  /// Toggle theme instantly without animation (for internal use)
  Future<void> toggleThemeInstant() async {
    await toggleTheme(animate: false);
  }

  /// Reset to default theme (dark mode)
  Future<void> resetToDefault() async {
    await setTheme(AppThemeMode.dark);
  }
}