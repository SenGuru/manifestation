import 'dart:async';
import 'package:manifestation_app/api/generated/code/manifestation.swagger.dart';
import 'package:manifestation_app/services/api_config.dart';
import 'package:chopper/chopper.dart';
import 'package:manifestation_app/services/session_service.dart';
import 'package:manifestation_app/screens/affirmations.dart';
import 'package:manifestation_app/constants/colors.dart';
import 'package:flutter/material.dart';

/// Service to handle affirmations API calls using the 3-table structure:
/// 1. Category table
/// 2. DailyAffirmationText table  
/// 3. DailyAffirmationSessionCategory junction table
class AffirmationApiService {
  final Manifestation _apiClient;
  
  AffirmationApiService({String? baseUrl}) : _apiClient = Manifestation.create(
    baseUrl: Uri.parse(baseUrl ?? ApiConfig.baseUrl),
    interceptors: [
      _BasicAuthInterceptor(),
      _SessionTokenInterceptor(),
    ],
  );

  // CATEGORY OPERATIONS

  /// Get categories - 5 core defaults + user custom categories (filter out other API defaults)
  Future<List<AffirmationCategory>> getCategories() async {
    try {
      print('Getting categories from API...');
      
      final response = await _apiClient.apiV1ManifestGetcategorysGet();
      const coreOrder = ['Love', 'Success', 'Health', 'Abundance', 'Confidence'];
      final foundApiCategories = <String, AffirmationCategory>{};
      final customCategories = <AffirmationCategory>[];
      
      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        if (returnModel.isSuccess == true && returnModel.model != null) {
          
          for (final categoryModel in returnModel.model!) {
            final categoryName = categoryModel.categoryName ?? '';
            
            // Get our 5 core default categories
            if (coreOrder.contains(categoryName)) {
              foundApiCategories[categoryName] = _mapCategoryModelToUI(categoryModel);
            } 
            // Get user-created custom categories (has userId, not one of the core 5)
            else if (categoryModel.userId != null) {
              print('Found custom category: $categoryName (userId: ${categoryModel.userId})');
              customCategories.add(_mapCategoryModelToUI(categoryModel));
            }
            // Ignore other API default categories
          }
          
          print('Found ${foundApiCategories.length} core defaults and ${customCategories.length} custom categories from API');
        }
      }
      
      // Build the final list: 5 core defaults first, then custom categories
      final finalCategories = <AffirmationCategory>[];
      final hardcodedDefaults = Map.fromEntries(
        _getDefaultCategories().map((cat) => MapEntry(cat.name, cat))
      );
      
      // Add the 5 core defaults in correct order
      for (final categoryName in coreOrder) {
        final category = foundApiCategories[categoryName] ?? hardcodedDefaults[categoryName]!;
        finalCategories.add(category);
      }
      
      // Sort custom categories by creation date (newest first) and add them
      customCategories.sort((a, b) => b.createdAt.compareTo(a.createdAt));
      finalCategories.addAll(customCategories);
      
      print('Returning ${finalCategories.length} total categories: 5 defaults + ${customCategories.length} custom');
      return finalCategories;
      
    } catch (e) {
      print('Error getting categories: $e');
      return _getDefaultCategories();
    }
  }
  
  /// Check if a category name is one of our 5 core default categories
  bool _isCoreDefaultCategory(String categoryName) {
    const coreDefaults = ['Love', 'Success', 'Health', 'Abundance', 'Confidence'];
    return coreDefaults.contains(categoryName);
  }
  

  /// Create a new category
  Future<AffirmationCategory?> createCategory(AffirmationCategory category) async {
    try {
      print('Creating category: ${category.name}');
      
      final createModel = CategoryCreateModel(
        categoryName: category.name,
        userId: null, // Will be set by API based on session
      );

      final response = await _apiClient.apiV1ManifestCreatecategoryPost(body: createModel);
      
      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        if (returnModel.isSuccess == true && returnModel.model != null) {
          print('Created category successfully');
          return _mapCategoryModelToUI(returnModel.model!);
        }
      }
      
      print('Failed to create category');
      return null;
    } catch (e) {
      print('Error creating category: $e');
      return null;
    }
  }

  /// Update an existing category
  Future<AffirmationCategory?> updateCategory(AffirmationCategory category) async {
    try {
      print('Updating category: ${category.name}');
      
      final updateModel = CategoryUpdateModel(
        categoryId: int.tryParse(category.id),
        categoryName: category.name,
        userId: null, // Will be set by API based on session
      );

      final response = await _apiClient.apiV1ManifestUpdatecategoryPost(body: updateModel);
      
      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        if (returnModel.isSuccess == true && returnModel.model != null) {
          print('Updated category successfully');
          return _mapCategoryModelToUI(returnModel.model!);
        }
      }
      
      print('Failed to update category');
      return null;
    } catch (e) {
      print('Error updating category: $e');
      return null;
    }
  }

  /// Delete a category (only custom categories)
  Future<bool> deleteCategory(String categoryId) async {
    try {
      print('Deleting category: $categoryId');
      
      final id = int.tryParse(categoryId);
      if (id == null) return false;

      final response = await _apiClient.apiV1ManifestMarkcategoryasdeletedDelete(id: id);
      
      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        if (returnModel.isSuccess == true) {
          print('Deleted category successfully');
          return true;
        }
      }
      
      print('Failed to delete category');
      return false;
    } catch (e) {
      print('Error deleting category: $e');
      return false;
    }
  }

  // AFFIRMATION TEXT OPERATIONS

  /// Get affirmations for a specific category
  Future<List<Affirmation>> getAffirmationsForCategory(String categoryId) async {
    try {
      print('Getting affirmations for category: $categoryId');
      
      final id = int.tryParse(categoryId);
      if (id == null) return [];

      final response = await _apiClient.apiV1ManifestGetdailyaffirmationtextbycategoryidGet(categoryId: id);
      
      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        if (returnModel.isSuccess == true && returnModel.model != null) {
          final affirmations = returnModel.model!.map((textModel) => 
            _mapAffirmationTextModelToUI(textModel)
          ).toList();
          
          print('Retrieved ${affirmations.length} affirmations for category $categoryId');
          return affirmations;
        }
      }
      
      print('Failed to get affirmations for category $categoryId');
      return [];
    } catch (e) {
      print('Error getting affirmations for category: $e');
      return [];
    }
  }

  /// Create a new affirmation
  Future<Affirmation?> createAffirmation(String categoryId, Affirmation affirmation) async {
    try {
      print('Creating affirmation in category: $categoryId');
      
      final createModel = DailyAffirmationTextCreateModel(
        dailyAffirmationTextValue: affirmation.text,
        categoryId: int.tryParse(categoryId),
        userId: null, // Will be set by API based on session
      );

      final response = await _apiClient.apiV1ManifestCreatedailyaffirmationtextPost(body: createModel);
      
      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        if (returnModel.isSuccess == true && returnModel.model != null) {
          print('Created affirmation successfully');
          return _mapAffirmationTextModelToUI(returnModel.model!);
        }
      }
      
      print('Failed to create affirmation');
      return null;
    } catch (e) {
      print('Error creating affirmation: $e');
      return null;
    }
  }

  /// Update an existing affirmation
  Future<Affirmation?> updateAffirmation(Affirmation affirmation) async {
    try {
      print('Updating affirmation: ${affirmation.id}');
      
      final updateModel = DailyAffirmationTextUpdateModel(
        dailyAffirmationTextId: int.tryParse(affirmation.id),
        dailyAffirmationTextValue: affirmation.text,
        categoryId: null, // Keep existing category
        userId: null, // Will be set by API based on session
      );

      final response = await _apiClient.apiV1ManifestUpdatedailyaffirmationtextPost(body: updateModel);
      
      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        if (returnModel.isSuccess == true && returnModel.model != null) {
          print('Updated affirmation successfully');
          return _mapAffirmationTextModelToUI(returnModel.model!);
        }
      }
      
      print('Failed to update affirmation');
      return null;
    } catch (e) {
      print('Error updating affirmation: $e');
      return null;
    }
  }

  /// Delete an affirmation
  Future<bool> deleteAffirmation(String affirmationId) async {
    try {
      print('Deleting affirmation: $affirmationId');
      
      final id = int.tryParse(affirmationId);
      if (id == null) return false;

      final response = await _apiClient.apiV1ManifestMarkdailyaffirmationtextasdeletedDelete(id: id);
      
      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        if (returnModel.isSuccess == true) {
          print('Deleted affirmation successfully');
          return true;
        }
      }
      
      print('Failed to delete affirmation');
      return false;
    } catch (e) {
      print('Error deleting affirmation: $e');
      return false;
    }
  }

  // MAPPING FUNCTIONS

  /// Map API CategoryModel to UI AffirmationCategory
  AffirmationCategory _mapCategoryModelToUI(CategoryModel categoryModel) {
    final categoryName = categoryModel.categoryName ?? '';
    
    // Assign colors based on category name
    Color categoryColor = _getCategoryColor(categoryName);

    // Determine if category is one of our core defaults
    final isDefaultCategory = _isCoreDefaultCategory(categoryName);

    return AffirmationCategory(
      id: categoryModel.categoryId?.toString() ?? '',
      name: categoryName,
      color: categoryColor,
      isCustom: !isDefaultCategory,
      createdAt: categoryModel.createdDate ?? DateTime.now(),
      affirmations: [], // Will be loaded separately
    );
  }
  
  /// Get the appropriate color for a category name
  Color _getCategoryColor(String categoryName) {
    switch (categoryName) {
      case 'Love':
        return AppColors.gradientPink;
      case 'Success':
        return AppColors.accentGold;
      case 'Health':
        return AppColors.accentMint;
      case 'Abundance':
        return AppColors.gradientOrange;
      case 'Confidence':
        return AppColors.gradientPurple;
      default:
        // For custom categories, use a default color
        return AppColors.gradientPurple;
    }
  }

  /// Map API DailyAffirmationTextModel to UI Affirmation
  Affirmation _mapAffirmationTextModelToUI(DailyAffirmationTextModel textModel) {
    return Affirmation(
      id: textModel.dailyAffirmationTextId?.toString() ?? '',
      text: textModel.dailyAffirmationTextValue ?? '',
      isFavorite: false, // TODO: Add favorite functionality when API supports it
      lastPlayed: null, // TODO: Add last played when API supports it
      isCustom: true, // Assume custom for now, TODO: determine based on API logic
      createdAt: textModel.createdDate ?? DateTime.now(),
    );
  }

  /// Get hardcoded default categories (fallback)
  List<AffirmationCategory> _getDefaultCategories() {
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

// Reuse the auth interceptors from other services
class _BasicAuthInterceptor implements Interceptor {
  static const String _basicAuthCredentials = 'U3BlY2lhbFVzZXI6U3BlY2lhbFBhc3N3b3JkMQ==';
  
  @override
  FutureOr<Response<BodyType>> intercept<BodyType>(Chain<BodyType> chain) async {
    final request = chain.request;
    final authenticatedRequest = request.copyWith(
      headers: Map.from(request.headers)
        ..addAll({
          'Authorization': 'Basic $_basicAuthCredentials',
        }),
    );
    
    return chain.proceed(authenticatedRequest);
  }
}

class _SessionTokenInterceptor implements Interceptor {
  @override
  FutureOr<Response<BodyType>> intercept<BodyType>(Chain<BodyType> chain) async {
    final request = chain.request;
    
    // Get session token
    final sessionToken = await SessionService.instance.getSessionToken();
    
    final headers = Map<String, String>.from(request.headers);
    
    // Add session token if available
    if (sessionToken != null) {
      headers['X-Session-Token'] = sessionToken;
      headers['Authorization'] = 'Bearer $sessionToken';
    }
    
    final authenticatedRequest = request.copyWith(headers: headers);
    return chain.proceed(authenticatedRequest);
  }
}