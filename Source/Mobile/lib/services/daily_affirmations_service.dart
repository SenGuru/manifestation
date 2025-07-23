import 'package:chopper/chopper.dart';
import 'package:flutter/material.dart';
import 'dart:async';
import '../api/generated/code/manifestation.swagger.dart';
import 'unified_auth_service.dart';

class DailyAffirmationsService {
  static final DailyAffirmationsService _instance = DailyAffirmationsService._internal();
  factory DailyAffirmationsService() => _instance;
  DailyAffirmationsService._internal();

  late Manifestation _apiClient;
  final UnifiedAuthService _authService = UnifiedAuthService();

  // The 5 supported default categories
  static const List<String> _supportedDefaultCategories = [
    'self-love',
    'abundance', 
    'success',
    'health',
    'confidence'
  ];

  // Hardcoded default affirmations (3 per category)
  static const Map<String, List<String>> _defaultAffirmations = {
    'self-love': [
      'I am worthy of love and respect',
      'I accept myself completely', 
      'I choose to love myself unconditionally'
    ],
    'abundance': [
      'I am open to receiving abundance',
      'Prosperity flows to me easily',
      'I attract opportunities for growth'
    ],
    'success': [
      'I am capable of achieving my dreams',
      'Success flows to me naturally',
      'I am grateful for my achievements'
    ],
    'health': [
      'My body is strong and healthy',
      'I nourish my body with love and care',
      'I am grateful for my vibrant health'
    ],
    'confidence': [
      'I believe in my abilities',
      'I am confident in who I am',
      'I trust myself to make good decisions'
    ]
  };

  void initialize({String? baseUrl}) {
    _apiClient = Manifestation.create(
      baseUrl: Uri.parse(baseUrl ?? 'http://manifestation-qa-1.senthilguru.net'),
      interceptors: [_AuthInterceptor()],
    );
  }

  // ========================= DEFAULT CATEGORIES =========================

  /// Get supported default categories from API
  Future<AffirmationResult<List<DefaultCategoryModel>>> getSupportedDefaultCategories() async {
    try {
      final response = await _apiClient.apiV1ManifestGetdefaultcategorysGet();

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        if (returnModel.isSuccess == true && returnModel.model != null) {
          // Filter to only include supported categories
          final filteredCategories = returnModel.model!.where((category) {
            final categoryName = category.defaultCategoryName?.toLowerCase() ?? '';
            return _supportedDefaultCategories.any((supported) => 
              categoryName.contains(supported) || supported.contains(categoryName)
            );
          }).toList();

          return AffirmationResult.success(
            data: filteredCategories,
            message: 'Default categories loaded successfully',
          );
        } else {
          return AffirmationResult.failure(
            message: returnModel.errorHolder?.toString() ?? 'Failed to load default categories',
          );
        }
      } else {
        return AffirmationResult.failure(
          message: 'Network error: ${response.statusCode}',
        );
      }
    } catch (e) {
      return AffirmationResult.failure(
        message: 'Error loading default categories: ${e.toString()}',
      );
    }
  }

  // ========================= USER CATEGORIES =========================

  /// Get user's custom categories (reuse from goal tracker service pattern)
  Future<AffirmationResult<List<CategoryModel>>> getUserCategories() async {
    try {
      final response = await _apiClient.apiV1ManifestGetcategorysGet(
        userId: _authService.currentUser?.userId,
      );

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        if (returnModel.isSuccess == true && returnModel.model != null) {
          return AffirmationResult.success(
            data: returnModel.model!,
            message: 'User categories loaded successfully',
          );
        } else {
          return AffirmationResult.failure(
            message: returnModel.errorHolder?.toString() ?? 'Failed to load user categories',
          );
        }
      } else {
        return AffirmationResult.failure(
          message: 'Network error: ${response.statusCode}',
        );
      }
    } catch (e) {
      return AffirmationResult.failure(
        message: 'Error loading user categories: ${e.toString()}',
      );
    }
  }

  // ========================= AFFIRMATION TEXTS =========================

  /// Get user's custom affirmation texts by category
  Future<AffirmationResult<List<DailyAffirmationTextModel>>> getUserAffirmationTextsByCategory(int categoryId) async {
    try {
      final response = await _apiClient.apiV1ManifestGetdailyaffirmationtextbycategoryidGet(
        categoryId: categoryId,
      );

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        if (returnModel.isSuccess == true && returnModel.model != null) {
          return AffirmationResult.success(
            data: returnModel.model!,
            message: 'User affirmation texts for category loaded successfully',
          );
        } else {
          return AffirmationResult.failure(
            message: returnModel.errorHolder?.toString() ?? 'Failed to load user affirmation texts for category',
          );
        }
      } else {
        return AffirmationResult.failure(
          message: 'Network error: ${response.statusCode}',
        );
      }
    } catch (e) {
      return AffirmationResult.failure(
        message: 'Error loading user affirmation texts by category: ${e.toString()}',
      );
    }
  }

  /// Create a new custom affirmation text
  Future<AffirmationResult<DailyAffirmationTextModel>> createAffirmationText({
    required String text,
    required int categoryId,
  }) async {
    try {
      final createModel = DailyAffirmationTextCreateModel(
        dailyAffirmationTextValue: text,
        categoryId: categoryId,
        userId: _authService.currentUser?.userId,
      );

      final response = await _apiClient.apiV1ManifestCreatedailyaffirmationtextPost(body: createModel);

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        if (returnModel.isSuccess == true && returnModel.model != null) {
          return AffirmationResult.success(
            data: returnModel.model!,
            message: 'Affirmation text created successfully',
          );
        } else {
          return AffirmationResult.failure(
            message: returnModel.errorHolder?.toString() ?? 'Failed to create affirmation text',
          );
        }
      } else {
        return AffirmationResult.failure(
          message: 'Network error: ${response.statusCode}',
        );
      }
    } catch (e) {
      return AffirmationResult.failure(
        message: 'Error creating affirmation text: ${e.toString()}',
      );
    }
  }

  /// Update an existing affirmation text
  Future<AffirmationResult<DailyAffirmationTextModel>> updateAffirmationText({
    required int affirmationTextId,
    required String text,
  }) async {
    try {
      final updateModel = DailyAffirmationTextUpdateModel(
        dailyAffirmationTextId: affirmationTextId,
        dailyAffirmationTextValue: text,
        userId: _authService.currentUser?.userId,
      );

      final response = await _apiClient.apiV1ManifestUpdatedailyaffirmationtextPost(body: updateModel);

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        if (returnModel.isSuccess == true && returnModel.model != null) {
          return AffirmationResult.success(
            data: returnModel.model!,
            message: 'Affirmation text updated successfully',
          );
        } else {
          return AffirmationResult.failure(
            message: returnModel.errorHolder?.toString() ?? 'Failed to update affirmation text',
          );
        }
      } else {
        return AffirmationResult.failure(
          message: 'Network error: ${response.statusCode}',
        );
      }
    } catch (e) {
      return AffirmationResult.failure(
        message: 'Error updating affirmation text: ${e.toString()}',
      );
    }
  }

  /// Delete an existing affirmation text
  Future<AffirmationResult<String>> deleteAffirmationText(int affirmationTextId) async {
    try {
      final response = await _apiClient.apiV1ManifestMarkdailyaffirmationtextasdeletedDelete(
        id: affirmationTextId,
      );

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        if (returnModel.isSuccess == true) {
          return AffirmationResult.success(
            data: 'Deleted successfully',
            message: 'Affirmation text deleted successfully',
          );
        } else {
          return AffirmationResult.failure(
            message: returnModel.errorHolder?.toString() ?? 'Failed to delete affirmation text',
          );
        }
      } else {
        return AffirmationResult.failure(
          message: 'Network error: ${response.statusCode}',
        );
      }
    } catch (e) {
      return AffirmationResult.failure(
        message: 'Error deleting affirmation text: ${e.toString()}',
      );
    }
  }

  // ========================= SESSION TRACKING =========================

  /// Create a new affirmation session
  Future<AffirmationResult<DailyAffirmationSessionModel>> createAffirmationSession({
    DateTime? sessionDate,
    int? timeSpentInSeconds,
    bool? addToFeed,
  }) async {
    try {
      final createModel = DailyAffirmationSessionCreateModel(
        dailyAffirmationSessionDate: sessionDate ?? DateTime.now(),
        dailyAffirmationSessionTimeSpentInSeconds: timeSpentInSeconds,
        userId: _authService.currentUser?.userId,
        addedToFeed: addToFeed,
        addedToFeedDate: addToFeed == true ? DateTime.now() : null,
      );

      final response = await _apiClient.apiV1ManifestCreatedailyaffirmationsessionPost(body: createModel);

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        if (returnModel.isSuccess == true && returnModel.model != null) {
          return AffirmationResult.success(
            data: returnModel.model!,
            message: 'Affirmation session created successfully',
          );
        } else {
          return AffirmationResult.failure(
            message: returnModel.errorHolder?.toString() ?? 'Failed to create affirmation session',
          );
        }
      } else {
        return AffirmationResult.failure(
          message: 'Network error: ${response.statusCode}',
        );
      }
    } catch (e) {
      return AffirmationResult.failure(
        message: 'Error creating affirmation session: ${e.toString()}',
      );
    }
  }

  // ========================= COMBINED DATA METHODS =========================

  /// Get all affirmations properly structured: default categories + user categories
  Future<AffirmationResult<List<AffirmationCategoryData>>> getAllAffirmationCategories() async {
    try {
      List<AffirmationCategoryData> allCategories = [];

      // 1. Add ONLY the 5 hardcoded default categories with hardcoded affirmations
      // Do NOT call any API for default categories - use only our hardcoded ones
      for (int i = 0; i < _supportedDefaultCategories.length; i++) {
        final categoryName = _supportedDefaultCategories[i];
        final categoryDisplayName = _getCategoryDisplayName(categoryName);
        final affirmationTexts = _defaultAffirmations[categoryName] ?? [];
        
        final affirmations = affirmationTexts.map((text) => AffirmationItem(
          id: 'default_${categoryName}_${affirmationTexts.indexOf(text)}',
          text: text,
          isDefault: true,
          categoryName: categoryDisplayName,
        )).toList();

        allCategories.add(AffirmationCategoryData(
          id: 'default_$i',
          name: categoryDisplayName,
          isDefault: true,
          affirmations: affirmations,
          color: _getDefaultCategoryColor(categoryName),
          iconId: _getDefaultCategoryIconId(categoryName),
        ));
      }

      // 2. Get user custom categories and their affirmations (exclude any that match our 5 default names)
      final userCategoriesResult = await getUserCategories();
      if (userCategoriesResult.isSuccess && userCategoriesResult.data != null) {
        
        for (final userCategory in userCategoriesResult.data!) {
          if (userCategory.categoryId != null) {
            final categoryName = (userCategory.categoryName ?? '').toLowerCase().trim();
            
            // Skip this user category if it matches any of our 5 default category names
            final isDefaultCategoryName = _supportedDefaultCategories.any((defaultName) =>
              categoryName == defaultName ||
              categoryName == _getCategoryDisplayName(defaultName).toLowerCase() ||
              categoryName.contains(defaultName) ||
              defaultName.contains(categoryName)
            );
            
            if (isDefaultCategoryName) {
              print('Skipping user category that matches default: $categoryName');
              continue; // Skip this category to avoid duplicates
            }
            
            // Get affirmations for this user category
            final userAffirmationsResult = await getUserAffirmationTextsByCategory(
              userCategory.categoryId!
            );

            List<AffirmationItem> userAffirmations = [];
            if (userAffirmationsResult.isSuccess && userAffirmationsResult.data != null) {
              userAffirmations = userAffirmationsResult.data!.map((affirmationText) => 
                AffirmationItem(
                  id: affirmationText.dailyAffirmationTextId?.toString() ?? '',
                  text: affirmationText.dailyAffirmationTextValue ?? '',
                  isDefault: false,
                  categoryName: userCategory.categoryName ?? 'Custom',
                )
              ).toList();
            }

            // Convert color integer to Color object
            Color? categoryColor;
            if (userCategory.colorAsInteger != null) {
              categoryColor = Color(userCategory.colorAsInteger!);
            }

            allCategories.add(AffirmationCategoryData(
              id: userCategory.categoryId!.toString(),
              name: userCategory.categoryName ?? 'Custom Category',
              isDefault: false,
              affirmations: userAffirmations,
              color: categoryColor,
              iconId: userCategory.predefinedIconTypeId,
            ));
          }
        }
      }

      return AffirmationResult.success(
        data: allCategories,
        message: 'All affirmation categories loaded successfully',
      );

    } catch (e) {
      return AffirmationResult.failure(
        message: 'Error loading affirmation categories: ${e.toString()}',
      );
    }
  }

  // ========================= HELPER METHODS =========================

  String _getCategoryDisplayName(String categoryKey) {
    switch (categoryKey) {
      case 'self-love':
        return 'Self-Love';
      case 'abundance':
        return 'Abundance';
      case 'success':
        return 'Success';
      case 'health':
        return 'Health';
      case 'confidence':
        return 'Confidence';
      default:
        return categoryKey.replaceFirst(categoryKey[0], categoryKey[0].toUpperCase());
    }
  }

  Color _getDefaultCategoryColor(String categoryKey) {
    switch (categoryKey) {
      case 'self-love':
        return const Color(0xFFE91E63); // Pink
      case 'abundance':
        return const Color(0xFF4CAF50); // Green
      case 'success':
        return const Color(0xFFFF9800); // Orange
      case 'health':
        return const Color(0xFF009688); // Teal
      case 'confidence':
        return const Color(0xFF3F51B5); // Indigo
      default:
        return const Color(0xFF9C27B0); // Purple
    }
  }

  int _getDefaultCategoryIconId(String categoryKey) {
    switch (categoryKey) {
      case 'self-love':
        return 3600002; // Heart
      case 'abundance':
        return 3600006; // Diamond
      case 'success':
        return 3600008; // Trophy
      case 'health':
        return 3600004; // Leaf
      case 'confidence':
        return 3600005; // Star
      default:
        return 3600001; // Flower
    }
  }

  // ========================= CATEGORY MANAGEMENT =========================

  /// Create a new custom category
  Future<AffirmationResult<CategoryModel>> createCategory({
    required String categoryName,
    required int colorAsInteger,
    required int predefinedIconTypeId,
  }) async {
    try {
      // First, get a default category to associate with this custom category
      int? defaultCategoryId;
      int? predefinedCategoryTypeId;
      try {
        final defaultCategoriesResult = await getSupportedDefaultCategories();
        if (defaultCategoriesResult.isSuccess && 
            defaultCategoriesResult.data != null && 
            defaultCategoriesResult.data!.isNotEmpty) {
          // Use the first available default category as a base
          final firstDefaultCategory = defaultCategoriesResult.data!.first;
          defaultCategoryId = firstDefaultCategory.defaultCategoryId;
          predefinedCategoryTypeId = firstDefaultCategory.predefinedCategoryTypeId;
          print('Using defaultCategoryId: $defaultCategoryId, predefinedCategoryTypeId: $predefinedCategoryTypeId');
        }
      } catch (e) {
        print('Warning: Could not get default category info: $e');
      }

      final createModel = CategoryCreateModel(
        categoryName: categoryName,
        userId: _authService.currentUser?.userId,
        colorAsInteger: colorAsInteger,
        predefinedIconTypeId: predefinedIconTypeId,
        defaultCategoryId: defaultCategoryId, // Provide a default category ID
        predefinedCategoryTypeId: predefinedCategoryTypeId, // Use the same type as default categories
      );

      final response = await _apiClient.apiV1ManifestCreatecategoryPost(body: createModel);

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        if (returnModel.isSuccess == true && returnModel.model != null) {
          return AffirmationResult.success(
            data: returnModel.model!,
            message: 'Category created successfully',
          );
        } else {
          return AffirmationResult.failure(
            message: returnModel.errorHolder?.toString() ?? 'Failed to create category',
          );
        }
      } else {
        return AffirmationResult.failure(
          message: 'Network error: ${response.statusCode}',
        );
      }
    } catch (e) {
      return AffirmationResult.failure(
        message: 'Error creating category: ${e.toString()}',
      );
    }
  }

  /// Update an existing category
  Future<AffirmationResult<CategoryModel>> updateCategory({
    required int categoryId,
    required String categoryName,
    required int colorAsInteger,
    required int predefinedIconTypeId,
  }) async {
    try {
      final updateModel = CategoryUpdateModel(
        categoryId: categoryId,
        categoryName: categoryName,
        userId: _authService.currentUser?.userId,
        colorAsInteger: colorAsInteger,
        predefinedIconTypeId: predefinedIconTypeId,
        // predefinedCategoryTypeId: null, // Leave as null
        // defaultCategoryId: null, // null for custom categories
      );

      final response = await _apiClient.apiV1ManifestUpdatecategoryPost(body: updateModel);

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        if (returnModel.isSuccess == true && returnModel.model != null) {
          return AffirmationResult.success(
            data: returnModel.model!,
            message: 'Category updated successfully',
          );
        } else {
          return AffirmationResult.failure(
            message: returnModel.errorHolder?.toString() ?? 'Failed to update category',
          );
        }
      } else {
        return AffirmationResult.failure(
          message: 'Network error: ${response.statusCode}',
        );
      }
    } catch (e) {
      return AffirmationResult.failure(
        message: 'Error updating category: ${e.toString()}',
      );
    }
  }

  /// Delete an existing category
  Future<AffirmationResult<String>> deleteCategory(int categoryId) async {
    try {
      final response = await _apiClient.apiV1ManifestMarkcategoryasdeletedDelete(
        id: categoryId,
      );

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        if (returnModel.isSuccess == true) {
          return AffirmationResult.success(
            data: 'Deleted successfully',
            message: 'Category deleted successfully',
          );
        } else {
          return AffirmationResult.failure(
            message: returnModel.errorHolder?.toString() ?? 'Failed to delete category',
          );
        }
      } else {
        return AffirmationResult.failure(
          message: 'Network error: ${response.statusCode}',
        );
      }
    } catch (e) {
      return AffirmationResult.failure(
        message: 'Error deleting category: ${e.toString()}',
      );
    }
  }
}

// ========================= RESULT CLASSES =========================

class AffirmationResult<T> {
  final bool isSuccess;
  final String message;
  final T? data;

  AffirmationResult._({
    required this.isSuccess,
    required this.message,
    this.data,
  });

  factory AffirmationResult.success({
    required T data,
    required String message,
  }) {
    return AffirmationResult._(
      isSuccess: true,
      message: message,
      data: data,
    );
  }

  factory AffirmationResult.failure({
    required String message,
  }) {
    return AffirmationResult._(
      isSuccess: false,
      message: message,
    );
  }
}

/// Unified affirmation item for UI display
class AffirmationItem {
  final String id;
  final String text;
  final bool isDefault;
  final String categoryName;
  final bool isFavorite;

  AffirmationItem({
    required this.id,
    required this.text,
    required this.isDefault,
    required this.categoryName,
    this.isFavorite = false,
  });
}

/// Category data structure for affirmations
class AffirmationCategoryData {
  final String id;
  final String name;
  final bool isDefault;
  final List<AffirmationItem> affirmations;
  final Color? color;
  final int? iconId;

  AffirmationCategoryData({
    required this.id,
    required this.name,
    required this.isDefault,
    required this.affirmations,
    this.color,
    this.iconId,
  });
}

// ========================= AUTH INTERCEPTOR =========================

class _AuthInterceptor implements Interceptor {
  @override
  FutureOr<Response<BodyType>> intercept<BodyType>(Chain<BodyType> chain) async {
    final request = chain.request;
    
    try {
      final authService = UnifiedAuthService();
      final authToken = authService.authToken;
      
      if (authToken != null && authToken.isNotEmpty) {
        final updatedRequest = request.copyWith(
          headers: {
            ...request.headers,
            'Authorization': 'Bearer $authToken',
          },
        );
        return chain.proceed(updatedRequest);
      }
    } catch (e) {
      print('❌ Error getting auth token: $e');
    }
    
    return chain.proceed(request);
  }
}