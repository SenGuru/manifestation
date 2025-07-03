import 'dart:convert';
import 'package:flutter/material.dart';
import 'affirmations.dart';
import 'colors.dart';

class StorageService {
  static List<AffirmationCategory> _categories = [];
  static bool _isInitialized = false;

  static Future<List<AffirmationCategory>> loadCategories() async {
    try {
      if (!_isInitialized) {
        // Load default categories on first run
        _categories = _getDefaultCategories();
        _isInitialized = true;
        print('Loaded ${_categories.length} default categories');
      }
      
      return List.from(_categories);
    } catch (e) {
      print('Error loading categories: $e');
      // Return default categories on error
      _categories = _getDefaultCategories();
      return List.from(_categories);
    }
  }

  static Future<void> saveCategories(List<AffirmationCategory> categories) async {
    try {
      _categories = List.from(categories);
      print('Saved ${categories.length} categories to memory');
    } catch (e) {
      print('Error saving categories: $e');
    }
  }

  static Future<void> addCategory(AffirmationCategory category) async {
    try {
      _categories.add(category);
      print('Added category: ${category.name}');
    } catch (e) {
      print('Error adding category: $e');
    }
  }

  static Future<void> updateCategory(AffirmationCategory updatedCategory) async {
    try {
      final index = _categories.indexWhere((c) => c.id == updatedCategory.id);
      if (index != -1) {
        _categories[index] = updatedCategory;
        print('Updated category: ${updatedCategory.name}');
      }
    } catch (e) {
      print('Error updating category: $e');
    }
  }

  static Future<void> deleteCategory(String categoryId) async {
    try {
      final initialLength = _categories.length;
      _categories.removeWhere((c) => c.id == categoryId && c.isCustom);
      if (_categories.length < initialLength) {
        print('Deleted category: $categoryId');
      }
    } catch (e) {
      print('Error deleting category: $e');
    }
  }

  static Future<void> addAffirmationToCategory(String categoryId, Affirmation affirmation) async {
    try {
      final categoryIndex = _categories.indexWhere((c) => c.id == categoryId);
      
      if (categoryIndex != -1) {
        final updatedAffirmations = List<Affirmation>.from(_categories[categoryIndex].affirmations)
          ..add(affirmation);
        
        _categories[categoryIndex] = _categories[categoryIndex].copyWith(
          affirmations: updatedAffirmations,
        );
        
        print('Added affirmation to category $categoryId');
      }
    } catch (e) {
      print('Error adding affirmation: $e');
    }
  }

  static Future<void> updateAffirmation(String categoryId, Affirmation updatedAffirmation) async {
    try {
      final categoryIndex = _categories.indexWhere((c) => c.id == categoryId);
      
      if (categoryIndex != -1) {
        final affirmations = List<Affirmation>.from(_categories[categoryIndex].affirmations);
        final affirmationIndex = affirmations.indexWhere((a) => a.id == updatedAffirmation.id);
        
        if (affirmationIndex != -1) {
          affirmations[affirmationIndex] = updatedAffirmation;
          _categories[categoryIndex] = _categories[categoryIndex].copyWith(
            affirmations: affirmations,
          );
          print('Updated affirmation in category $categoryId');
        }
      }
    } catch (e) {
      print('Error updating affirmation: $e');
    }
  }

  static Future<void> deleteAffirmation(String categoryId, String affirmationId) async {
    try {
      final categoryIndex = _categories.indexWhere((c) => c.id == categoryId);
      
      if (categoryIndex != -1) {
        final affirmations = List<Affirmation>.from(_categories[categoryIndex].affirmations);
        final initialLength = affirmations.length;
        affirmations.removeWhere((a) => a.id == affirmationId);
        
        if (affirmations.length < initialLength) {
          _categories[categoryIndex] = _categories[categoryIndex].copyWith(
            affirmations: affirmations,
          );
          print('Deleted affirmation from category $categoryId');
        }
      }
    } catch (e) {
      print('Error deleting affirmation: $e');
    }
  }

  static List<AffirmationCategory> _getDefaultCategories() {
    final now = DateTime.now();
    return [
      AffirmationCategory(
        id: 'love_default',
        name: 'Love',
        color: AppColors.gradientPink,
        isCustom: false,
        createdAt: now,
        affirmations: [
          Affirmation(
            id: 'love_1',
            text: 'I am worthy of deep, unconditional love.',
            isCustom: false,
            createdAt: now,
          ),
          Affirmation(
            id: 'love_2',
            text: 'Love flows to me effortlessly and abundantly.',
            isCustom: false,
            createdAt: now,
          ),
          Affirmation(
            id: 'love_3',
            text: 'I attract healthy, loving relationships into my life.',
            isCustom: false,
            createdAt: now,
          ),
        ],
      ),
      AffirmationCategory(
        id: 'success_default',
        name: 'Success',
        color: AppColors.accentGold,
        isCustom: false,
        createdAt: now,
        affirmations: [
          Affirmation(
            id: 'success_1',
            text: 'Success flows to me naturally and easily.',
            isCustom: false,
            createdAt: now,
          ),
          Affirmation(
            id: 'success_2',
            text: 'I am capable of achieving anything I set my mind to.',
            isCustom: false,
            createdAt: now,
          ),
          Affirmation(
            id: 'success_3',
            text: 'Every challenge is an opportunity for growth.',
            isCustom: false,
            createdAt: now,
          ),
        ],
      ),
      AffirmationCategory(
        id: 'health_default',
        name: 'Health',
        color: AppColors.accentMint,
        isCustom: false,
        createdAt: now,
        affirmations: [
          Affirmation(
            id: 'health_1',
            text: 'My body is strong, healthy, and full of energy.',
            isCustom: false,
            createdAt: now,
          ),
          Affirmation(
            id: 'health_2',
            text: 'I nourish my body with love and healthy choices.',
            isCustom: false,
            createdAt: now,
          ),
          Affirmation(
            id: 'health_3',
            text: 'Healing energy flows through every cell of my being.',
            isCustom: false,
            createdAt: now,
          ),
        ],
      ),
      AffirmationCategory(
        id: 'abundance_default',
        name: 'Abundance',
        color: AppColors.gradientOrange,
        isCustom: false,
        createdAt: now,
        affirmations: [
          Affirmation(
            id: 'abundance_1',
            text: 'Abundance flows into my life from multiple sources.',
            isCustom: false,
            createdAt: now,
          ),
          Affirmation(
            id: 'abundance_2',
            text: 'I am a magnet for prosperity and wealth.',
            isCustom: false,
            createdAt: now,
          ),
          Affirmation(
            id: 'abundance_3',
            text: 'The universe provides for all my needs abundantly.',
            isCustom: false,
            createdAt: now,
          ),
        ],
      ),
      AffirmationCategory(
        id: 'confidence_default',
        name: 'Confidence',
        color: AppColors.gradientPurple,
        isCustom: false,
        createdAt: now,
        affirmations: [
          Affirmation(
            id: 'confidence_1',
            text: 'I believe in myself and my abilities completely.',
            isCustom: false,
            createdAt: now,
          ),
          Affirmation(
            id: 'confidence_2',
            text: 'I radiate confidence and positive energy.',
            isCustom: false,
            createdAt: now,
          ),
          Affirmation(
            id: 'confidence_3',
            text: 'I trust myself to make the right decisions.',
            isCustom: false,
            createdAt: now,
          ),
        ],
      ),
    ];
  }
}