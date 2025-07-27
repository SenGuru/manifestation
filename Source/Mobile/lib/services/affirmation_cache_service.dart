import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:shared_preferences/shared_preferences.dart';
import '../models/affirmation_category.dart';
import '../models/affirmation_item.dart';

/// Smart caching service for daily affirmations
/// Prevents unnecessary API calls and improves user experience
class AffirmationCacheService {
  static final AffirmationCacheService _instance = AffirmationCacheService._internal();
  factory AffirmationCacheService() => _instance;
  AffirmationCacheService._internal();

  static const String _cacheKey = 'affirmation_categories_cache';
  static const String _lastUpdatedKey = 'affirmation_cache_last_updated';
  static const String _dirtyFlagsKey = 'affirmation_dirty_flags';
  
  // Cache expiration: 24 hours
  static const Duration _cacheExpiration = Duration(hours: 24);
  
  SharedPreferences? _prefs;
  List<AffirmationCategory>? _cachedCategories;
  DateTime? _lastUpdated;
  Set<String> _dirtyCategories = {};

  /// Initialize the cache service
  Future<void> initialize() async {
    _prefs ??= await SharedPreferences.getInstance();
    await _loadFromStorage();
  }

  /// Check if cache is valid and not expired
  bool get isCacheValid {
    if (_cachedCategories == null || _lastUpdated == null) {
      return false;
    }
    
    final now = DateTime.now();
    final isExpired = now.difference(_lastUpdated!) > _cacheExpiration;
    
    return !isExpired;
  }

  /// Check if we have any cached data (even if stale)
  bool get hasCachedData {
    return _cachedCategories != null && _cachedCategories!.isNotEmpty;
  }

  /// Get cached categories if available
  List<AffirmationCategory>? get cachedCategories {
    return _cachedCategories;
  }

  /// Check if a specific category needs refresh
  bool isCategoryDirty(String categoryId) {
    return _dirtyCategories.contains(categoryId);
  }

  /// Get cache age in minutes
  int get cacheAgeMinutes {
    if (_lastUpdated == null) return 0;
    return DateTime.now().difference(_lastUpdated!).inMinutes;
  }

  /// Check if we should fetch fresh data
  /// Returns true if cache is empty OR expired
  bool get shouldFetchFresh {
    return !hasCachedData || !isCacheValid;
  }

  /// Save categories to cache
  Future<void> saveCategories(List<AffirmationCategory> categories) async {
    await initialize();
    
    _cachedCategories = List.from(categories);
    _lastUpdated = DateTime.now();
    _dirtyCategories.clear(); // Clear dirty flags after successful save
    
    // Save to persistent storage
    await _saveToStorage();
    
    print('💾 Affirmation cache updated: ${categories.length} categories cached');
  }

  /// Mark a category as dirty (needs refresh)
  Future<void> markCategoryDirty(String categoryId) async {
    await initialize();
    
    _dirtyCategories.add(categoryId);
    await _saveDirtyFlags();
    
    print('🏷️ Category marked as dirty: $categoryId');
  }

  /// Mark multiple categories as dirty
  Future<void> markCategoriesDirty(List<String> categoryIds) async {
    await initialize();
    
    _dirtyCategories.addAll(categoryIds);
    await _saveDirtyFlags();
    
    print('🏷️ Categories marked as dirty: $categoryIds');
  }

  /// Clear cache and force refresh on next load
  Future<void> invalidateCache() async {
    await initialize();
    
    _cachedCategories = null;
    _lastUpdated = null;
    _dirtyCategories.clear();
    
    await _clearStorage();
    
    print('🗑️ Affirmation cache invalidated');
  }

  /// Update a single category in cache
  Future<void> updateCategoryInCache(AffirmationCategory updatedCategory) async {
    if (_cachedCategories == null) return;
    
    final index = _cachedCategories!.indexWhere((cat) => cat.id == updatedCategory.id);
    if (index != -1) {
      _cachedCategories![index] = updatedCategory;
      _dirtyCategories.remove(updatedCategory.id);
      await _saveToStorage();
      
      print('📝 Category updated in cache: ${updatedCategory.name}');
    }
  }

  /// Remove a category from cache
  Future<void> removeCategoryFromCache(String categoryId) async {
    if (_cachedCategories == null) return;
    
    _cachedCategories!.removeWhere((cat) => cat.id == categoryId);
    _dirtyCategories.remove(categoryId);
    await _saveToStorage();
    
    print('🗑️ Category removed from cache: $categoryId');
  }

  /// Add a new category to cache
  Future<void> addCategoryToCache(AffirmationCategory newCategory) async {
    if (_cachedCategories == null) {
      _cachedCategories = [];
    }
    
    _cachedCategories!.add(newCategory);
    await _saveToStorage();
    
    print('➕ Category added to cache: ${newCategory.name}');
  }

  /// Replace temporary category with real one
  Future<void> replaceCategoryInCache(String tempId, AffirmationCategory realCategory) async {
    if (_cachedCategories == null) return;
    
    final tempIndex = _cachedCategories!.indexWhere((cat) => cat.id == tempId);
    if (tempIndex != -1) {
      _cachedCategories![tempIndex] = realCategory;
      await _saveToStorage();
      print('🔄 Replaced temp category with real one: $tempId');
    }
  }

  /// Add affirmation to specific category (optimistic update)
  Future<void> addAffirmationToCategory(String categoryId, AffirmationItem affirmationItem) async {
    if (_cachedCategories == null) return;
    
    final categoryIndex = _cachedCategories!.indexWhere((cat) => cat.id == categoryId);
    if (categoryIndex != -1) {
      final updatedAffirmations = List<AffirmationItem>.from(_cachedCategories![categoryIndex].affirmations);
      updatedAffirmations.add(affirmationItem);
      
      _cachedCategories![categoryIndex] = _cachedCategories![categoryIndex].copyWith(
        affirmations: updatedAffirmations,
      );
      
      await _saveToStorage();
      print('➕ Affirmation added to category $categoryId');
    }
  }

  /// Remove affirmation from cache (revert optimistic update)
  Future<void> removeAffirmationFromCache(String affirmationId) async {
    if (_cachedCategories == null) return;
    
    for (int i = 0; i < _cachedCategories!.length; i++) {
      final updatedAffirmations = _cachedCategories![i].affirmations
          .where((affirmation) => affirmation.id != affirmationId)
          .toList()
          .cast<AffirmationItem>();
      
      if (updatedAffirmations.length != _cachedCategories![i].affirmations.length) {
        _cachedCategories![i] = _cachedCategories![i].copyWith(
          affirmations: updatedAffirmations,
        );
        await _saveToStorage();
        print('🗑️ Affirmation removed from cache: $affirmationId');
        break;
      }
    }
  }

  /// Replace temporary affirmation with real one
  Future<void> replaceAffirmationInCache(String tempId, AffirmationItem realAffirmation) async {
    if (_cachedCategories == null) return;
    
    for (int i = 0; i < _cachedCategories!.length; i++) {
      final affirmations = _cachedCategories![i].affirmations;
      final tempIndex = affirmations.indexWhere((a) => a.id == tempId);
      
      if (tempIndex != -1) {
        final updatedAffirmations = List<AffirmationItem>.from(affirmations);
        updatedAffirmations[tempIndex] = realAffirmation;
        
        _cachedCategories![i] = _cachedCategories![i].copyWith(
          affirmations: updatedAffirmations,
        );
        
        await _saveToStorage();
        print('🔄 Replaced temp affirmation with real one: $tempId');
        break;
      }
    }
  }

  /// Update affirmation text in cache (optimistic update)
  /// Returns the original text for reverting if needed
  Future<String?> updateAffirmationText(String affirmationId, String newText) async {
    if (_cachedCategories == null) return null;
    
    for (int i = 0; i < _cachedCategories!.length; i++) {
      final affirmations = _cachedCategories![i].affirmations;
      final affirmationIndex = affirmations.indexWhere((a) => a.id == affirmationId);
      
      if (affirmationIndex != -1) {
        final originalText = affirmations[affirmationIndex].text;
        final updatedAffirmations = List<AffirmationItem>.from(affirmations);
        
        // Create updated affirmation with new text
        final updatedAffirmation = AffirmationItem(
          id: affirmations[affirmationIndex].id,
          text: newText,
          isDefault: affirmations[affirmationIndex].isDefault,
          categoryName: affirmations[affirmationIndex].categoryName,
          isFavorite: affirmations[affirmationIndex].isFavorite,
        );
        
        updatedAffirmations[affirmationIndex] = updatedAffirmation;
        
        _cachedCategories![i] = _cachedCategories![i].copyWith(
          affirmations: updatedAffirmations,
        );
        
        await _saveToStorage();
        print('🔄 Updated affirmation text in cache: $affirmationId');
        return originalText;
      }
    }
    
    return null;
  }

  /// Update category in cache (optimistic update)
  /// Returns the original category for reverting if needed
  Future<AffirmationCategory?> updateCategory(
    String categoryId, 
    String newName, 
    int newColor, 
    int newIconId
  ) async {
    if (_cachedCategories == null) return null;
    
    final categoryIndex = _cachedCategories!.indexWhere((cat) => cat.id == categoryId);
    if (categoryIndex != -1) {
      final originalCategory = _cachedCategories![categoryIndex];
      
      // Create updated category
      final updatedCategory = originalCategory.copyWith(
        name: newName,
        color: Color(newColor),
        iconId: newIconId,
      );
      
      _cachedCategories![categoryIndex] = updatedCategory;
      await _saveToStorage();
      
      print('🔄 Updated category in cache: $categoryId');
      return originalCategory;
    }
    
    return null;
  }


  /// Get cache statistics for debugging
  Map<String, dynamic> getCacheStats() {
    return {
      'hasCachedData': hasCachedData,
      'isCacheValid': isCacheValid,
      'categoriesCount': _cachedCategories?.length ?? 0,
      'cacheAgeMinutes': cacheAgeMinutes,
      'dirtyCategoriesCount': _dirtyCategories.length,
      'lastUpdated': _lastUpdated?.toIso8601String(),
    };
  }

  // Private methods for storage management

  Future<void> _loadFromStorage() async {
    try {
      // Load categories
      final categoriesJson = _prefs!.getString(_cacheKey);
      if (categoriesJson != null) {
        final categoriesData = jsonDecode(categoriesJson) as List;
        _cachedCategories = categoriesData
            .map((json) => AffirmationCategory.fromJson(json))
            .toList();
      }

      // Load last updated timestamp
      final lastUpdatedMs = _prefs!.getInt(_lastUpdatedKey);
      if (lastUpdatedMs != null) {
        _lastUpdated = DateTime.fromMillisecondsSinceEpoch(lastUpdatedMs);
      }

      // Load dirty flags
      final dirtyFlags = _prefs!.getStringList(_dirtyFlagsKey);
      if (dirtyFlags != null) {
        _dirtyCategories = dirtyFlags.toSet();
      }

      print('📱 Affirmation cache loaded from storage');
    } catch (e) {
      print('⚠️ Error loading affirmation cache: $e');
      await _clearStorage();
    }
  }

  Future<void> _saveToStorage() async {
    try {
      // Save categories
      if (_cachedCategories != null) {
        final categoriesJson = jsonEncode(
          _cachedCategories!.map((cat) => cat.toJson()).toList()
        );
        await _prefs!.setString(_cacheKey, categoriesJson);
      }

      // Save last updated timestamp
      if (_lastUpdated != null) {
        await _prefs!.setInt(_lastUpdatedKey, _lastUpdated!.millisecondsSinceEpoch);
      }

      // Save dirty flags
      await _saveDirtyFlags();
    } catch (e) {
      print('⚠️ Error saving affirmation cache: $e');
    }
  }

  Future<void> _saveDirtyFlags() async {
    await _prefs!.setStringList(_dirtyFlagsKey, _dirtyCategories.toList());
  }

  Future<void> _clearStorage() async {
    await _prefs!.remove(_cacheKey);
    await _prefs!.remove(_lastUpdatedKey);
    await _prefs!.remove(_dirtyFlagsKey);
  }
}