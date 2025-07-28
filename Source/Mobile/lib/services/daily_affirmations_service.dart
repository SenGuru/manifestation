import 'package:chopper/chopper.dart';
import 'package:flutter/material.dart';
import 'dart:async';
import '../api/generated/code/manifestation.swagger.dart';
import '../models/affirmation_item.dart';
import '../constants/goal_constants.dart';
import 'unified_auth_service.dart';

class DailyAffirmationsService {
  static final DailyAffirmationsService _instance = DailyAffirmationsService._internal();
  factory DailyAffirmationsService() => _instance;
  DailyAffirmationsService._internal();
  late Manifestation _apiClient;
  final UnifiedAuthService _authService = UnifiedAuthService();

  // The 5 supported default categories (matching server names)
  static const List<String> _supportedDefaultCategories = [
    'Love',
    'Abundance', 
    'Success',
    'Health',
    'Confidence'
  ];

  // Hardcoded default affirmations (3 per category) - using server category names
  static const Map<String, List<String>> _defaultAffirmations = {
    'Love': [
      'I am worthy of love and respect',
      'I accept myself completely', 
      'I choose to love myself unconditionally'
    ],
    'Abundance': [
      'I am open to receiving abundance',
      'Prosperity flows to me easily',
      'I attract opportunities for growth'
    ],
    'Success': [
      'I am capable of achieving my dreams',
      'Success flows to me naturally',
      'I am grateful for my achievements'
    ],
    'Health': [
      'My body is strong and healthy',
      'I nourish my body with love and care',
      'I am grateful for my vibrant health'
    ],
    'Confidence': [
      'I believe in my abilities',
      'I am confident in who I am',
      'I trust myself to make good decisions'
    ]
  };

  bool _isInitialized = false;

  void _ensureInitialized() {
    if (!_isInitialized) {
      _apiClient = Manifestation.create(
        baseUrl: Uri.parse('http://manifestation-qa-1.senthilguru.net'),
        interceptors: [_AuthInterceptor()],
      );
      _isInitialized = true;
    }
  }

  void initialize({String? baseUrl}) {
    if (!_isInitialized) {
      _apiClient = Manifestation.create(
        baseUrl: Uri.parse(baseUrl ?? 'http://manifestation-qa-1.senthilguru.net'),
        interceptors: [_AuthInterceptor()],
      );
      _isInitialized = true;
    }
  }

  // ========================= DEFAULT CATEGORIES =========================

  /// Get supported default categories from API
  Future<AffirmationResult<List<DefaultCategoryModel>>> getSupportedDefaultCategories() async {
    try {
      final response = await _apiClient.apiV1ManifestGetdefaultcategorysGet();

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        if (returnModel.isSuccess == true && returnModel.model != null) {
          print('🔍 Total default categories from API: ${returnModel.model!.length}');
          
          // Filter to only include daily affirmation categories using proper enum
          final filteredCategories = returnModel.model!.where((category) {
            final isAffirmationCategory = category.predefinedCategoryTypeId == PredefinedCategoryTypes.dailyAffirmation;
            print('📋 Category: ${category.defaultCategoryName} (TypeID: ${category.predefinedCategoryTypeId}, TypeName: ${category.predefinedCategoryTypeName}) -> ${isAffirmationCategory ? "INCLUDED" : "EXCLUDED"}');
            return isAffirmationCategory;
          }).toList();

          print('✅ Filtered daily affirmation categories: ${filteredCategories.length}');
          
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

  /// Get user's custom categories
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

  /// Get all affirmation categories directly from API
  Future<AffirmationResult<List<AffirmationCategoryData>>> getAllAffirmationCategories() async {
    _ensureInitialized();
    print('🌐 Loading affirmation data from API...');
    return await _loadFreshCategories();
  }

  /// Get all affirmations properly structured: default categories + user categories
  Future<AffirmationResult<List<AffirmationCategoryData>>> _loadFreshCategories() async {
    try {
      List<AffirmationCategoryData> allCategories = [];

      // 1. Get server default categories and add them with local affirmations
      final defaultCategoriesResult = await getSupportedDefaultCategories();
      if (defaultCategoriesResult.isSuccess && defaultCategoriesResult.data != null) {
        for (final serverCategory in defaultCategoriesResult.data!) {
          final categoryName = serverCategory.defaultCategoryName ?? '';
          final affirmationTexts = _defaultAffirmations[categoryName] ?? [];
          
          print('🏷️ Processing server category: $categoryName with ${affirmationTexts.length} affirmations');
          
          final affirmations = affirmationTexts.map((text) => AffirmationItem(
            id: 'default_${categoryName}_${affirmationTexts.indexOf(text)}',
            text: text,
            isDefault: true,
            categoryName: categoryName,
          )).toList();

          allCategories.add(AffirmationCategoryData(
            id: serverCategory.defaultCategoryId?.toString() ?? 'default_${categoryName}',
            name: categoryName,
            isDefault: true,
            affirmations: affirmations,
            color: _getDefaultCategoryColor(categoryName),
            iconId: _getDefaultCategoryIconId(categoryName),
          ));
        }
      }

      // 2. Get user custom categories and their affirmations (filter by daily affirmation type)
      final userCategoriesResult = await getUserCategories();
      if (userCategoriesResult.isSuccess && userCategoriesResult.data != null) {
        
        for (final userCategory in userCategoriesResult.data!) {
          if (userCategory.categoryId != null) {
            // FIRST: Check if this is a daily affirmation category using proper enum
            final isAffirmationCategory = userCategory.predefinedCategoryTypeId == PredefinedCategoryTypes.dailyAffirmation;
            print('📋 User Category: ${userCategory.categoryName} (TypeID: ${userCategory.predefinedCategoryTypeId}) -> ${isAffirmationCategory ? "DAILY_AFFIRMATION" : "OTHER_TYPE"}');
            
            if (!isAffirmationCategory) {
              print('❌ Skipping user category - not a daily affirmation category: ${userCategory.categoryName}');
              continue; // Skip this category - it's not for daily affirmations
            }
            
            final categoryName = (userCategory.categoryName ?? '').toLowerCase().trim();
            
            // SECOND: Skip this user category if it matches any of our 5 default category names
            final isDefaultCategoryName = _supportedDefaultCategories.any((defaultName) =>
              categoryName == defaultName.toLowerCase() ||
              categoryName.contains(defaultName.toLowerCase()) ||
              defaultName.toLowerCase().contains(categoryName)
            );
            
            if (isDefaultCategoryName) {
              print('⚠️ Skipping user category that matches default: $categoryName');
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
    // Server already provides proper display names, just return as-is
    return categoryKey;
  }

  Color _getDefaultCategoryColor(String categoryKey) {
    switch (categoryKey) {
      case 'Love':
        return const Color(0xFFE91E63); // Pink
      case 'Abundance':
        return const Color(0xFF4CAF50); // Green
      case 'Success':
        return const Color(0xFFFF9800); // Orange
      case 'Health':
        return const Color(0xFF009688); // Teal
      case 'Confidence':
        return const Color(0xFF3F51B5); // Indigo
      default:
        return const Color(0xFF9C27B0); // Purple
    }
  }

  int _getDefaultCategoryIconId(String categoryKey) {
    switch (categoryKey) {
      case 'Love':
        return 3600002; // Heart
      case 'Abundance':
        return 3600006; // Diamond
      case 'Success':
        return 3600008; // Trophy
      case 'Health':
        return 3600004; // Leaf
      case 'Confidence':
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
      // Get default category info for API call
      int? defaultCategoryId;
      int? predefinedCategoryTypeId;
      try {
        final defaultCategoriesResult = await getSupportedDefaultCategories();
        if (defaultCategoriesResult.isSuccess && 
            defaultCategoriesResult.data != null && 
            defaultCategoriesResult.data!.isNotEmpty) {
          final firstDefaultCategory = defaultCategoriesResult.data!.first;
          defaultCategoryId = firstDefaultCategory.defaultCategoryId;
          predefinedCategoryTypeId = firstDefaultCategory.predefinedCategoryTypeId;
        }
      } catch (e) {
        print('Warning: Could not get default category info: $e');
      }

      final createModel = CategoryCreateModel(
        categoryName: categoryName,
        userId: _authService.currentUser?.userId,
        colorAsInteger: colorAsInteger,
        predefinedIconTypeId: predefinedIconTypeId,
        defaultCategoryId: defaultCategoryId,
        predefinedCategoryTypeId: predefinedCategoryTypeId,
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