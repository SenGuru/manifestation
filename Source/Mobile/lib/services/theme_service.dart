import 'package:flutter/material.dart';
import 'package:shared_preferences/shared_preferences.dart';

enum AppThemeMode { light, dark }

class ThemeService extends ChangeNotifier {
  static final ThemeService _instance = ThemeService._internal();
  factory ThemeService() {
    print('🎨 ThemeService factory called, returning instance: ${_instance.hashCode}');
    return _instance;
  }
  ThemeService._internal() {
    print('🎨 ThemeService._internal() called, creating instance: ${this.hashCode}');
  }

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
    print('🎨 Toggle theme called - current: ${_currentTheme.name}');
    print('🎨 _isAnimating before toggle: $_isAnimating');
    
    // Force reset animation flag if it's stuck
    if (_isAnimating && !animate) {
      print('🎨 Resetting stuck animation flag');
      _isAnimating = false;
    }
    
    final newTheme = _currentTheme == AppThemeMode.dark 
        ? AppThemeMode.light 
        : AppThemeMode.dark;
    
    await setTheme(newTheme, animate: animate);
  }

  /// Set specific theme mode with optional animation
  Future<void> setTheme(AppThemeMode theme, {bool animate = true}) async {
    print('🎨 setTheme called - current: ${_currentTheme.name}, new: ${theme.name}');
    print('🎨 _isAnimating: $_isAnimating, same theme: ${_currentTheme == theme}');
    if (_currentTheme == theme) {
      print('🎨 setTheme cancelled - same theme');
      return;
    }
    if (_isAnimating) {
      print('🎨 setTheme cancelled - currently animating');
      return;
    }
    
    if (animate) {
      _isAnimating = true;
      print('🎨 Starting animation, notifying listeners');
      notifyListeners();
      
      // Delay theme change to allow animation to start
      await Future.delayed(const Duration(milliseconds: 50));
    }
    
    print('🎨 Changing theme from ${_currentTheme.name} to ${theme.name}');
    _currentTheme = theme;
    print('🎨 Theme changed, notifying listeners');
    notifyListeners();
    
    // Reset animation flag
    if (animate) {
      await Future.delayed(const Duration(milliseconds: 100));
      _isAnimating = false;
      print('🎨 Animation completed, flag reset');
    }
    
    try {
      final prefs = await SharedPreferences.getInstance();
      await prefs.setString(_themeKey, theme.toString());
      print('🎨 Theme saved to preferences: ${theme.name} mode');
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