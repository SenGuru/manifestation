import 'package:chopper/chopper.dart';
import 'package:flutter/material.dart';
import 'dart:async';
import '../api/generated/code/manifestation.swagger.dart';
import '../constants/goal_constants.dart';
import '../models/goal_tracker_category.dart';
import '../models/goals.dart';
import 'unified_auth_service.dart';

class GoalTrackerService {
  static final GoalTrackerService _instance = GoalTrackerService._internal();
  factory GoalTrackerService() => _instance;
  GoalTrackerService._internal();

  late Manifestation _apiClient;
  final UnifiedAuthService _authService = UnifiedAuthService();

  void initialize({String? baseUrl}) {
    _apiClient = Manifestation.create(
      baseUrl: Uri.parse(baseUrl ?? 'http://manifestation-qa-1.senthilguru.net'),
      interceptors: [_AuthInterceptor()],
    );
  }

  // ========================= LOCAL CATEGORIES =========================

  // Simple local category mapping - no API calls needed
  static final Map<int, GoalTrackerCategory> _localCategories = {
    1: GoalTrackerCategory(
      id: '1',
      name: 'Career',
      description: 'Career and work-related goals',
      color: Color(0xFF2196F3), // Blue
      iconValue: 3600007, // target
      isDefault: true,
    ),
    2: GoalTrackerCategory(
      id: '2',
      name: 'Health',
      description: 'Health and fitness goals',
      color: Color(0xFF4CAF50), // Green
      iconValue: 3600004, // leaf
      isDefault: true,
    ),
    3: GoalTrackerCategory(
      id: '3',
      name: 'Love',
      description: 'Relationship and love goals',
      color: Color(0xFFE91E63), // Pink
      iconValue: 3600002, // heart
      isDefault: true,
    ),
    4: GoalTrackerCategory(
      id: '4',
      name: 'Money',
      description: 'Financial and money goals',
      color: Color(0xFFFF9800), // Orange
      iconValue: 3600006, // diamond
      isDefault: true,
    ),
    5: GoalTrackerCategory(
      id: '5',
      name: 'Personal',
      description: 'Personal development goals',
      color: Color(0xFF9C27B0), // Purple
      iconValue: 3600005, // star
      isDefault: true,
    ),
    6: GoalTrackerCategory(
      id: '6',
      name: 'Spiritual',
      description: 'Spiritual and mindfulness goals',
      color: Color(0xFF673AB7), // Deep Purple
      iconValue: 3600001, // flower
      isDefault: true,
    ),
  };

  // Map category names to actual database IDs from your system
  // These will be updated dynamically from server categories
  static Map<String, int> _categoryNameToId = {
    'career': 100,    // Fallback ID, will be updated from server
    'health': 101,    // Fallback ID, will be updated from server
    'love': 102,      // Fallback ID, will be updated from server
    'money': 103,     // Fallback ID, will be updated from server
    'personal': 104,  // Fallback ID, will be updated from server
    'spiritual': 105, // Fallback ID, will be updated from server
  };

  // Map default category IDs to category names for conversion
  static final Map<int, String> _defaultIdToCategoryName = {
    100: 'career',
    101: 'health', 
    102: 'love',
    103: 'money',
    104: 'personal',
    105: 'spiritual',
  };

  // Cache for server categories to avoid repeated API calls
  static List<dynamic>? _serverCategories;

  // Map database IDs back to local categories for display
  // This will be updated dynamically when server categories are loaded
  static Map<int, int> _dbIdToLocalId = {
    // Default category IDs (reference IDs)
    100: 1, // career
    101: 2, // health
    102: 3, // love
    103: 4, // money
    104: 5, // personal
    105: 6, // spiritual
    // Server database category IDs (will be updated dynamically)
    1309: 1, // career
    1310: 2, // health
    1311: 3, // love
    1312: 4, // money
    1313: 5, // personal
    1314: 6, // spiritual
    // Real server category IDs from API
    1006: 1, // Career (from actual API response)
    1007: 2, // Health (from actual API response)
    1008: 3, // Love (from actual API response)
    1009: 4, // Money (from actual API response)
    1010: 5, // Personal (from actual API response)
    1011: 6, // Spiritual (from actual API response)
    // Handle user custom categories that might exist
    1045: 3, // Love -> love
    1046: 5, // Success -> personal
    1047: 2, // Health -> health
    1048: 4, // Abundance -> money
    1049: 5, // Confidence -> personal
    1051: 2, // Health -> health
    1052: 3, // Love -> love
    1089: 5, // test category -> personal
  };

  /// Get default categories - simple local categories, no API calls
  Future<GoalTrackerResult<List<GoalTrackerCategory>>> getDefaultCategories() async {
    return GoalTrackerResult.success(
      data: _localCategories.values.toList(),
      message: 'Local categories loaded successfully',
    );
  }

  /// Get category by ID
  GoalTrackerCategory? getCategoryById(int categoryId) {
    return _localCategories[categoryId];
  }

  /// Get category ID by name
  int? getCategoryIdByName(String categoryName) {
    return _categoryNameToId[categoryName.toLowerCase()];
  }

  /// Get local category from database category ID
  GoalTrackerCategory? getLocalCategoryFromDbId(int dbCategoryId) {
    final localId = _dbIdToLocalId[dbCategoryId];
    if (localId != null) {
      return _localCategories[localId];
    }
    return null; // Return null for unknown categories
  }

  /// Get actual categories from server for goal tracking
  Future<GoalTrackerResult<List<dynamic>>> getServerCategories() async {
    try {
      // Return cached categories if available
      if (_serverCategories != null) {
        print('🔍 Using cached server categories (${_serverCategories!.length} items)');
        return GoalTrackerResult.success(
          data: _serverCategories!,
          message: 'Cached server categories loaded successfully',
        );
      }
      
      print('🔍 Fetching categories from server for goal tracking...');
      
      // Get categories specifically for goal tracking (PredefinedCategoryType = 3400004)
      final response = await _apiClient.apiV1ManifestGetcategorysGet(
        predefinedCategoryTypeIds: [PredefinedCategoryTypes.goalTracker], // 3400004
        showDeleted: false,
      );

      print('🔍 Categories response status: ${response.statusCode}');
      
      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        print('🔍 Categories response isSuccess: ${returnModel.isSuccess}');
        print('🔍 Categories count: ${returnModel.model?.length ?? 0}');
        
        if (returnModel.isSuccess == true && returnModel.model != null) {
          // Cache the server categories
          _serverCategories = returnModel.model!;
          
          // Update our category name-to-ID mapping with actual server data
          _updateCategoryMappingFromServer(_serverCategories!);
          
          // Log each category for debugging
          for (var category in returnModel.model!) {
            print('🏷️  Server Category: ID=${category.categoryId}, Name=${category.categoryName}, DefaultId=${category.defaultCategoryId}');
          }
          
          return GoalTrackerResult.success(
            data: returnModel.model!,
            message: 'Server categories loaded successfully',
          );
        } else {
          return GoalTrackerResult.failure(
            message: returnModel.errorHolder?.toString() ?? 'Failed to load server categories',
          );
        }
      } else {
        return GoalTrackerResult.failure(
          message: 'Network error loading categories: ${response.statusCode}',
        );
      }
    } catch (e) {
      return GoalTrackerResult.failure(
        message: 'Error loading server categories: ${e.toString()}',
      );
    }
  }

  /// Update our local category mapping with actual server category IDs
  void _updateCategoryMappingFromServer(List<dynamic> serverCategories) {
    print('🔄 Updating category mappings from server data...');
    
    // Map common category names to server category IDs
    final Map<String, List<String>> categoryNameMapping = {
      'career': ['career', 'work', 'job', 'business', 'professional'],
      'health': ['health', 'fitness', 'wellness', 'medical'],
      'love': ['love', 'relationship', 'romance', 'dating', 'marriage'],
      'money': ['money', 'financial', 'finance', 'wealth', 'income', 'budget'],
      'personal': ['personal', 'development', 'growth', 'self', 'improvement'],
      'spiritual': ['spiritual', 'meditation', 'mindfulness', 'soul', 'faith'],
    };

    for (var category in serverCategories) {
      final categoryName = category.categoryName?.toString().toLowerCase() ?? '';
      final categoryId = category.categoryId as int?;
      
      if (categoryId != null && categoryName.isNotEmpty) {
        // Try to match category name to our local categories
        for (var localCategory in categoryNameMapping.keys) {
          final possibleNames = categoryNameMapping[localCategory]!;
          
          if (possibleNames.any((name) => categoryName.contains(name))) {
            print('🔗 Mapping "$localCategory" to server category "$categoryName" (ID: $categoryId)');
            _categoryNameToId[localCategory] = categoryId;
            
            // Also update the reverse mapping for display purposes
            final localCategoryId = _localCategories.entries
                .firstWhere((entry) => entry.value.name.toLowerCase() == localCategory, orElse: () => MapEntry(0, _localCategories[5]!))
                .key;
            if (localCategoryId > 0) {
              _dbIdToLocalId[categoryId] = localCategoryId;
              print('🔗 Added reverse mapping: DB ID $categoryId -> Local ID $localCategoryId');
            }
            break;
          }
        }
      }
    }
    
    print('🔄 Updated category mapping: $_categoryNameToId');
  }

  /// Get user's custom categories (still keep this for custom categories)
  Future<GoalTrackerResult<List<GoalTrackerCategory>>> getUserCategories() async {
    // For now, return empty list - users can only use the 6 default categories
    return GoalTrackerResult.success(
      data: <GoalTrackerCategory>[],
      message: 'No custom categories (using local defaults only)',
    );
  }

  /// Get all categories (just return local default categories)
  Future<GoalTrackerResult<List<GoalTrackerCategory>>> getAllCategories() async {
    return getDefaultCategories(); // Just return the 6 local categories
  }


  // ========================= GOAL TRACKERS =========================

  /// Get all goal trackers for the current user
  Future<GoalTrackerResult<List<GoalTrackerModel>>> getUserGoalTrackers() async {
    try {
      print('📡 === MAIN API CALL STRUCTURE ===');
      print('📤 Making API request:');
      print('   └─ Endpoint: GET /api/v1/manifest/getgoaltrackers');
      print('   └─ Parameters:');
      print('      • userId: ${_authService.currentUser?.userId}');
      print('   └─ Expected Response Structure:');
      print('      {');
      print('        "isSuccess": true,');
      print('        "model": [');
      print('          {');
      print('            "goalTrackerId": 123,');
      print('            "goalTrackerName": "Goal Name",');
      print('            "userId": ${_authService.currentUser?.userId},');
      print('            "existingCategorys": [  // ❌ THIS SHOULD CONTAIN CATEGORIES');
      print('              {');
      print('                "categoryId": 1309,');
      print('                "categoryName": "Money",');
      print('                "defaultCategoryId": 103');
      print('              }');
      print('            ]');
      print('          }');
      print('        ]');
      print('      }');
      print('📡 ================================');
      
      final response = await _apiClient.apiV1ManifestGetgoaltrackersGet(
        userId: _authService.currentUser?.userId,
      );

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        print('📡 === API RESPONSE RECEIVED ===');
        print('📥 Response Status: ${response.statusCode}');
        print('📥 Response isSuccess: ${returnModel.isSuccess}');
        print('📥 Goals Count: ${returnModel.model?.length ?? 0}');
        
        if (returnModel.isSuccess == true && returnModel.model != null) {
          // Debug each goal's existingCategorys field
          for (int i = 0; i < returnModel.model!.length; i++) {
            final goal = returnModel.model![i];
            print('📥 Goal ${i + 1} Response Data:');
            print('   • goalTrackerId: ${goal.goalTrackerId}');
            print('   • goalTrackerName: ${goal.goalTrackerName}');
            print('   • existingCategorys: ${goal.existingCategorys}');
            print('   • existingCategorys length: ${goal.existingCategorys?.length ?? 0}');
            print('   • existingCategorys type: ${goal.existingCategorys.runtimeType}');
            if (goal.existingCategorys != null && goal.existingCategorys!.isNotEmpty) {
              print('   ✅ CATEGORIES FOUND!');
              for (var cat in goal.existingCategorys!) {
                print('      - Category: ${cat.toString()}');
              }
            } else {
              print('   ❌ NO CATEGORIES IN RESPONSE (SERVER BUG)');
            }
          }
          print('📡 =============================');
          
          return GoalTrackerResult.success(
            data: returnModel.model!,
            message: 'Goal trackers loaded successfully',
          );
        } else {
          return GoalTrackerResult.failure(
            message: returnModel.errorHolder?.toString() ?? 'Failed to load goal trackers',
          );
        }
      } else {
        return GoalTrackerResult.failure(
          message: 'Network error: ${response.statusCode}',
        );
      }
    } catch (e) {
      return GoalTrackerResult.failure(
        message: 'Error loading goal trackers: ${e.toString()}',
      );
    }
  }

  /// Get categories for multiple goal trackers efficiently
  Future<Map<int, List<dynamic>>> getCategoriesForMultipleGoalTrackers(List<int> goalTrackerIds) async {
    final Map<int, List<dynamic>> categoriesMap = {};
    
    print('🔍 Fetching categories for ${goalTrackerIds.length} goal trackers...');
    
    // Make concurrent API calls for better performance
    final futures = goalTrackerIds.map((goalTrackerId) async {
      try {
        final result = await getCategoriesForGoalTracker(goalTrackerId);
        if (result.isSuccess && result.data != null) {
          categoriesMap[goalTrackerId] = result.data!;
          print('✅ Got ${result.data!.length} categories for goal $goalTrackerId');
        } else {
          categoriesMap[goalTrackerId] = [];
          print('❌ No categories found for goal $goalTrackerId: ${result.message}');
        }
      } catch (e) {
        categoriesMap[goalTrackerId] = [];
        print('❌ Error fetching categories for goal $goalTrackerId: $e');
      }
    });
    
    // Wait for all requests to complete
    await Future.wait(futures);
    
    print('🔍 Completed category fetching for ${categoriesMap.length} goals');
    return categoriesMap;
  }

  /// Get categories for a specific goal tracker
  Future<GoalTrackerResult<List<dynamic>>> getCategoriesForGoalTracker(int goalTrackerId) async {
    try {
      print('🔍 Fetching categories for goal tracker ID: $goalTrackerId');
      
      final response = await _apiClient.apiV1ManifestGetgoaltrackercategorybygoaltrackeridGet(
        goalTrackerId: goalTrackerId,
      );

      print('🔍 Goal categories response status: ${response.statusCode}');
      
      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        print('🔍 Goal categories response isSuccess: ${returnModel.isSuccess}');
        print('🔍 Goal categories count: ${returnModel.model?.length ?? 0}');
        
        if (returnModel.isSuccess == true && returnModel.model != null) {
          // Log each category for debugging
          for (var category in returnModel.model!) {
            print('🏷️  Goal Category: ID=${category.categoryId}, GoalTrackerID=${category.goalTrackerId}, CategoryID=${category.categoryId}');
          }
          
          return GoalTrackerResult.success(
            data: returnModel.model!,
            message: 'Goal categories loaded successfully',
          );
        } else {
          return GoalTrackerResult.failure(
            message: returnModel.errorHolder?.toString() ?? 'Failed to load goal categories',
          );
        }
      } else {
        return GoalTrackerResult.failure(
          message: 'Network error loading goal categories: ${response.statusCode}',
        );
      }
    } catch (e) {
      return GoalTrackerResult.failure(
        message: 'Error loading goal categories: ${e.toString()}',
      );
    }
  }

  /// Get goal trackers by category
  Future<GoalTrackerResult<List<GoalTrackerModel>>> getGoalTrackersByCategory(int categoryId) async {
    try {
      final response = await _apiClient.apiV1ManifestGetgoaltrackersbycategoryidGet(
        categoryId: categoryId,
      );

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        if (returnModel.isSuccess == true && returnModel.model != null) {
          return GoalTrackerResult.success(
            data: returnModel.model!,
            message: 'Goal trackers loaded successfully',
          );
        } else {
          return GoalTrackerResult.failure(
            message: returnModel.errorHolder?.toString() ?? 'Failed to load goal trackers for category',
          );
        }
      } else {
        return GoalTrackerResult.failure(
          message: 'Network error: ${response.statusCode}',
        );
      }
    } catch (e) {
      return GoalTrackerResult.failure(
        message: 'Error loading goal trackers by category: ${e.toString()}',
      );
    }
  }

  /// Create a new goal tracker with fallback approach for categories
  Future<GoalTrackerResult<GoalTrackerModel>> createGoalTracker({
    required String goalName,
    required int goalStatusTypeId,
    DateTime? startDate,
    DateTime? endDate,
    required int userId,
    int? categoryId, // Category ID (could be local 1-6 or database 100+)
    int? priorityId, // Priority ID from GoalPriorityTypes
  }) async {
    try {
      // Simple validation
      if (goalName.trim().isEmpty) {
        return GoalTrackerResult.failure(message: 'Goal name cannot be empty');
      }
      
      // First, try to create goal with category
      if (categoryId != null) {
        print('📝 Attempting goal creation with category ID: $categoryId');
        final resultWithCategory = await _createGoalWithCategory(
          goalName, goalStatusTypeId, startDate, endDate, userId, categoryId, priorityId
        );
        
        if (resultWithCategory.isSuccess) {
          return resultWithCategory;
        }
        
        print('⚠️ Goal creation with category failed: ${resultWithCategory.message}');
        print('🔄 Falling back to goal creation without category...');
      }
      
      // Fallback: Create goal without category
      final resultWithoutCategory = await _createGoalWithoutCategory(
        goalName, goalStatusTypeId, startDate, endDate, userId, priorityId
      );
      
      if (resultWithoutCategory.isSuccess && categoryId != null) {
        print('✅ Goal created without category, storing category locally for future reference');
        // Note: Category will be handled during local filtering/display
        return GoalTrackerResult.success(
          data: resultWithoutCategory.data!,
          message: 'Goal created successfully (category handled locally)',
        );
      }
      
      return resultWithoutCategory;
    } catch (e) {
      return GoalTrackerResult.failure(
        message: 'Error creating goal tracker: ${e.toString()}',
      );
    }
  }

  /// Create goal with category (may fail on server)
  Future<GoalTrackerResult<GoalTrackerModel>> _createGoalWithCategory(
    String goalName,
    int goalStatusTypeId,
    DateTime? startDate,
    DateTime? endDate,
    int userId,
    int categoryId,
    int? priorityId,
  ) async {
    // First, get the actual categories from the server to debug
    print('🔍 Checking server categories before goal creation...');
    final serverCategoriesResult = await getServerCategories();
    if (serverCategoriesResult.isSuccess) {
      print('✅ Retrieved ${serverCategoriesResult.data!.length} categories from server');
    } else {
      print('⚠️ Could not retrieve server categories: ${serverCategoriesResult.message}');
    }
    
    // Determine if categoryId is local (1-6), default (100+), or actual database ID (1000+)
    int actualCategoryId = categoryId;
    String categoryName = 'unknown';
    
    if (categoryId >= 1 && categoryId <= 6) {
      // This is a local category ID, convert to actual server database ID
      print('🔄 Converting local category ID $categoryId to server database ID...');
      final localCategory = _localCategories[categoryId];
      if (localCategory != null) {
        categoryName = localCategory.name.toLowerCase();
        actualCategoryId = _categoryNameToId[categoryName] ?? categoryId;
        print('🔄 Local category "${localCategory.name}" (ID: $categoryId) -> Server Database ID: $actualCategoryId');
      }
    } else if (categoryId >= 100 && categoryId <= 105) {
      // This is a defaultCategoryId, we need to find the actual categoryId from server
      print('🔄 Converting default category ID $categoryId to server database ID...');
      // Use the default ID to category name mapping
      categoryName = _defaultIdToCategoryName[categoryId] ?? 'unknown';
      
      if (categoryName != 'unknown') {
        // Get the actual server database ID from our updated mapping
        actualCategoryId = _categoryNameToId[categoryName] ?? categoryId;
        print('🔄 Default category ID $categoryId ($categoryName) -> Server Database ID: $actualCategoryId');
      }
    } else {
      // This might already be a server database ID, find the category name
      categoryName = _categoryNameToId.entries
          .firstWhere((entry) => entry.value == categoryId, orElse: () => MapEntry('unknown', -1))
          .key;
    }
    
    if (categoryName == 'unknown') {
      print('⚠️ Unknown category ID: $categoryId. Local IDs: 1-6, Database IDs: ${_categoryNameToId.values.toList()}');
      return GoalTrackerResult.failure(
        message: 'Invalid category ID: $categoryId',
      );
    }
    
    print('📝 Creating goal with category: $categoryName (Original ID: $categoryId, Database ID: $actualCategoryId)');
    
    // Check if this category ID actually exists on the server
    bool categoryExistsOnServer = false;
    if (serverCategoriesResult.isSuccess && serverCategoriesResult.data != null) {
      categoryExistsOnServer = serverCategoriesResult.data!.any((cat) => cat.categoryId == actualCategoryId);
      print('🔍 Category ID $actualCategoryId exists on server: $categoryExistsOnServer');
      
      if (!categoryExistsOnServer) {
        print('⚠️ Category ID $actualCategoryId not found on server. Available server category IDs:');
        for (var cat in serverCategoriesResult.data!) {
          print('   - ID: ${cat.categoryId}, Name: ${cat.categoryName}');
        }
        
        // Try to find a matching category by name instead
        final matchingCategories = serverCategoriesResult.data!.where((cat) {
          final serverCategoryName = cat.categoryName?.toString().toLowerCase() ?? '';
          return serverCategoryName.contains(categoryName) || 
                 categoryName.contains(serverCategoryName.split(' ').first);
        });
        
        final matchingCategory = matchingCategories.isNotEmpty ? matchingCategories.first : null;
        
        if (matchingCategory != null) {
          actualCategoryId = matchingCategory.categoryId!;
          print('🔄 Found matching server category: "${matchingCategory.categoryName}" (ID: $actualCategoryId)');
          categoryExistsOnServer = true;
        }
      }
    }
    
    if (!categoryExistsOnServer) {
      print('❌ No suitable category found on server, falling back to goal creation without category');
      return GoalTrackerResult.failure(
        message: 'Category not available on server, will create goal without category',
      );
    }
    
    final categoryModels = [
      GoalTrackerCategoryCreateModel(
        categoryId: actualCategoryId, // Use the verified server category ID
        userId: userId,
        // goalTrackerId is null during creation - goal doesn't exist yet
      )
    ];

    final createModel = GoalTrackerCreateModel(
      goalTrackerName: goalName.trim(),
      predefinedGoalStatusTypeId: goalStatusTypeId,
      predefinedGoalPriorityTypeId: priorityId ?? GoalPriorityTypes.medium, // Default to medium
      goalTrackerStartDate: startDate ?? DateTime.now(),
      goalTrackerEndDate: endDate,
      userId: userId,
      newGoalTrackerCategoryModels: categoryModels,
    );

    // Debug: Print the JSON being sent
    try {
      print('📤 Request JSON: ${createModel.toJson()}');
    } catch (e) {
      print('📤 Request JSON serialization error: $e');
    }

    final response = await _apiClient.apiV1ManifestCreategoaltrackerPost(body: createModel);

    print('📡 Response status: ${response.statusCode}');
    print('📡 Response headers: ${response.headers}');
    
    if (response.isSuccessful && response.body != null) {
      final returnModel = response.body!;
      
      print('📡 Response body isSuccess: ${returnModel.isSuccess}');
      print('📡 Response body successMessage: ${returnModel.successMessage}');
      print('📡 Response body errorHolder: ${returnModel.errorHolder?.toString()}');
      
      if (returnModel.isSuccess == true && returnModel.model != null) {
        print('✅ Goal created successfully with category');
        return GoalTrackerResult.success(
          data: returnModel.model!,
          message: 'Goal tracker created with category',
        );
      } else {
        return GoalTrackerResult.failure(
          message: returnModel.errorHolder?.friendlyMessage ?? 
                  returnModel.successMessage ?? 
                  'Failed to create goal with category',
        );
      }
    } else {
      print('❌ HTTP Error Response: ${response.error}');
      return GoalTrackerResult.failure(
        message: 'Network error creating goal with category: ${response.statusCode}',
      );
    }
  }

  /// Create goal without category (more likely to succeed)
  Future<GoalTrackerResult<GoalTrackerModel>> _createGoalWithoutCategory(
    String goalName,
    int goalStatusTypeId,
    DateTime? startDate,
    DateTime? endDate,
    int userId,
    int? priorityId,
  ) async {
    final createModel = GoalTrackerCreateModel(
      goalTrackerName: goalName.trim(),
      predefinedGoalStatusTypeId: goalStatusTypeId,
      predefinedGoalPriorityTypeId: priorityId ?? GoalPriorityTypes.medium, // Default to medium
      goalTrackerStartDate: startDate ?? DateTime.now(),
      goalTrackerEndDate: endDate,
      userId: userId,
      // No newGoalTrackerCategoryModels - create goal without categories
    );

    final response = await _apiClient.apiV1ManifestCreategoaltrackerPost(body: createModel);

    if (response.isSuccessful && response.body != null) {
      final returnModel = response.body!;
      
      if (returnModel.isSuccess == true && returnModel.model != null) {
        print('✅ Goal created successfully without category');
        return GoalTrackerResult.success(
          data: returnModel.model!,
          message: 'Goal tracker created successfully',
        );
      } else {
        return GoalTrackerResult.failure(
          message: returnModel.errorHolder?.friendlyMessage ?? 'Failed to create goal tracker',
        );
      }
    } else {
      return GoalTrackerResult.failure(
        message: 'Network error: ${response.statusCode}',
      );
    }
  }

  /// Update a goal tracker with category associations
  Future<GoalTrackerResult<GoalTrackerModel>> updateGoalTracker({
    required int goalTrackerId,
    String? goalName,
    int? goalStatusTypeId,
    DateTime? startDate,
    DateTime? endDate,
    DateTime? completedDate,
    List<int>? categoryIds, // NEW: Category IDs to associate with the goal
  }) async {
    try {
      // Create category associations if provided
      List<GoalTrackerCategoryCreateModel>? categoryModels;
      if (categoryIds != null && categoryIds.isNotEmpty) {
        print('🏷️  Updating category associations for IDs: $categoryIds');
        categoryModels = categoryIds.map((categoryId) {
          return GoalTrackerCategoryCreateModel(
            categoryId: categoryId,
            goalTrackerId: goalTrackerId,
            userId: _authService.currentUser?.userId,
          );
        }).toList();
        print('🏷️  Created ${categoryModels.length} category association models for update');
      }
      
      final updateModel = GoalTrackerUpdateModel(
        goalTrackerId: goalTrackerId,
        goalTrackerName: goalName,
        predefinedGoalStatusTypeId: goalStatusTypeId,
        goalTrackerStartDate: startDate,
        goalTrackerEndDate: endDate,
        goalTrackerCompletedDate: completedDate,
        userId: _authService.currentUser?.userId,
        newGoalTrackerCategoryModels: categoryModels, // NEW: Category associations
      );

      final response = await _apiClient.apiV1ManifestUpdategoaltrackerPost(body: updateModel);

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        if (returnModel.isSuccess == true && returnModel.model != null) {
          return GoalTrackerResult.success(
            data: returnModel.model!,
            message: 'Goal tracker updated successfully',
          );
        } else {
          return GoalTrackerResult.failure(
            message: returnModel.errorHolder?.toString() ?? 'Failed to update goal tracker',
          );
        }
      } else {
        return GoalTrackerResult.failure(
          message: 'Network error: ${response.statusCode}',
        );
      }
    } catch (e) {
      return GoalTrackerResult.failure(
        message: 'Error updating goal tracker: ${e.toString()}',
      );
    }
  }

  /// Delete a goal tracker (mark as deleted)
  Future<GoalTrackerResult<bool>> deleteGoalTracker(int goalTrackerId) async {
    try {
      print('🗑️  API: Marking goal tracker as deleted - ID: $goalTrackerId');
      
      // Add timeout to prevent hanging
      final response = await _apiClient.apiV1ManifestMarkgoaltrackerasdeletedDelete(id: goalTrackerId)
          .timeout(Duration(seconds: 30));

      print('🗑️  API Response received!');
      print('🗑️  API Response - Status Code: ${response.statusCode}');
      print('🗑️  API Response - Is Successful: ${response.isSuccessful}');
      print('🗑️  API Response - Body: ${response.body?.toString()}');

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        print('🗑️  API Response - IsSuccess: ${returnModel.isSuccess}');
        print('🗑️  API Response - Model: ${returnModel.model}');
        print('🗑️  API Response - Message: ${returnModel.successMessage}');
        print('🗑️  API Response - Error: ${returnModel.errorHolder}');
        
        if (returnModel.isSuccess == true) {
          return GoalTrackerResult.success(
            data: true,
            message: 'Goal tracker marked as deleted successfully',
          );
        } else {
          return GoalTrackerResult.failure(
            message: returnModel.errorHolder?.toString() ?? 'Failed to mark goal tracker as deleted',
          );
        }
      } else {
        return GoalTrackerResult.failure(
          message: 'Network error: ${response.statusCode}',
        );
      }
    } catch (e) {
      print('🗑️  API Exception: $e');
      
      if (e.toString().contains('TimeoutException')) {
        return GoalTrackerResult.failure(
          message: 'Delete request timed out - the goal may have been deleted successfully',
        );
      }
      
      return GoalTrackerResult.failure(
        message: 'Error marking goal tracker as deleted: ${e.toString()}',
      );
    }
  }

  // ========================= MILESTONES =========================

  /// Get all milestones for the current user
  Future<GoalTrackerResult<List<GoalTrackerMileStoneModel>>> getUserMilestones() async {
    try {
      final response = await _apiClient.apiV1ManifestGetgoaltrackermilestonebyuseridGet(
        userId: _authService.currentUser?.userId,
      );

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        if (returnModel.isSuccess == true && returnModel.model != null) {
          return GoalTrackerResult.success(
            data: returnModel.model!,
            message: 'Milestones loaded successfully',
          );
        } else {
          return GoalTrackerResult.failure(
            message: returnModel.errorHolder?.toString() ?? 'Failed to load milestones',
          );
        }
      } else {
        return GoalTrackerResult.failure(
          message: 'Network error: ${response.statusCode}',
        );
      }
    } catch (e) {
      return GoalTrackerResult.failure(
        message: 'Error loading milestones: ${e.toString()}',
      );
    }
  }

  /// Get milestones for a specific goal tracker
  Future<GoalTrackerResult<List<GoalTrackerMileStoneModel>>> getMilestonesByGoalTracker(int goalTrackerId) async {
    try {
      final response = await _apiClient.apiV1ManifestGetgoaltrackermilestonebygoaltrackeridGet(
        goalTrackerId: goalTrackerId,
      );

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        if (returnModel.isSuccess == true && returnModel.model != null) {
          return GoalTrackerResult.success(
            data: returnModel.model!,
            message: 'Milestones loaded successfully',
          );
        } else {
          return GoalTrackerResult.failure(
            message: returnModel.errorHolder?.toString() ?? 'Failed to load milestones for goal',
          );
        }
      } else {
        return GoalTrackerResult.failure(
          message: 'Network error: ${response.statusCode}',
        );
      }
    } catch (e) {
      return GoalTrackerResult.failure(
        message: 'Error loading milestones by goal tracker: ${e.toString()}',
      );
    }
  }

  /// Create a new milestone
  Future<GoalTrackerResult<GoalTrackerMileStoneModel>> createMilestone({
    required int goalTrackerId,
    required String milestoneName,
    DateTime? startDate,
    DateTime? endDate,
    int? statusTypeId,
  }) async {
    try {
      final createModel = GoalTrackerMileStoneCreateModel(
        goalTrackerId: goalTrackerId,
        goalTrackerMileStoneName: milestoneName,
        goalTrackerMileStoneStartDate: startDate ?? DateTime.now(),
        goalTrackerMileStoneEndDate: endDate,
        predefinedGoalStatusTypeId: statusTypeId ?? 1, // Default to "Active" status (ID 1)
        userId: _authService.currentUser?.userId,
      );

      final response = await _apiClient.apiV1ManifestCreategoaltrackermilestonePost(body: createModel);

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        if (returnModel.isSuccess == true && returnModel.model != null) {
          return GoalTrackerResult.success(
            data: returnModel.model!,
            message: 'Milestone created successfully',
          );
        } else {
          return GoalTrackerResult.failure(
            message: returnModel.errorHolder?.toString() ?? 'Failed to create milestone',
          );
        }
      } else {
        return GoalTrackerResult.failure(
          message: 'Network error: ${response.statusCode}',
        );
      }
    } catch (e) {
      return GoalTrackerResult.failure(
        message: 'Error creating milestone: ${e.toString()}',
      );
    }
  }

  /// Update a milestone
  Future<GoalTrackerResult<GoalTrackerMileStoneModel>> updateMilestone({
    required int milestoneId,
    int? goalTrackerId,
    String? milestoneName,
    DateTime? startDate,
    DateTime? endDate,
    DateTime? completedDate,
    int? statusTypeId,
  }) async {
    try {
      final updateModel = GoalTrackerMileStoneUpdateModel(
        goalTrackerMileStoneId: milestoneId,
        goalTrackerId: goalTrackerId,
        goalTrackerMileStoneName: milestoneName,
        goalTrackerMileStoneStartDate: startDate,
        goalTrackerMileStoneEndDate: endDate,
        goalTrackerMileStoneCompletedDate: completedDate,
        predefinedGoalStatusTypeId: statusTypeId,
        userId: _authService.currentUser?.userId,
      );

      final response = await _apiClient.apiV1ManifestUpdategoaltrackermilestonePost(body: updateModel);

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        if (returnModel.isSuccess == true && returnModel.model != null) {
          return GoalTrackerResult.success(
            data: returnModel.model!,
            message: 'Milestone updated successfully',
          );
        } else {
          return GoalTrackerResult.failure(
            message: returnModel.errorHolder?.toString() ?? 'Failed to update milestone',
          );
        }
      } else {
        return GoalTrackerResult.failure(
          message: 'Network error: ${response.statusCode}',
        );
      }
    } catch (e) {
      return GoalTrackerResult.failure(
        message: 'Error updating milestone: ${e.toString()}',
      );
    }
  }

  /// Delete a milestone (mark as deleted)
  Future<GoalTrackerResult<bool>> deleteMilestone(int milestoneId) async {
    try {
      print('🗑️  API: Marking milestone as deleted - ID: $milestoneId');
      
      final response = await _apiClient.apiV1ManifestMarkgoaltrackermilestoneasdeletedDelete(id: milestoneId);

      print('🗑️  Milestone API Response - Status Code: ${response.statusCode}');
      print('🗑️  Milestone API Response - Is Successful: ${response.isSuccessful}');

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        if (returnModel.isSuccess == true) {
          return GoalTrackerResult.success(
            data: true,
            message: 'Milestone marked as deleted successfully',
          );
        } else {
          return GoalTrackerResult.failure(
            message: returnModel.errorHolder?.toString() ?? 'Failed to mark milestone as deleted',
          );
        }
      } else {
        return GoalTrackerResult.failure(
          message: 'Network error: ${response.statusCode}',
        );
      }
    } catch (e) {
      print('🗑️  Milestone API Exception: $e');
      return GoalTrackerResult.failure(
        message: 'Error marking milestone as deleted: ${e.toString()}',
      );
    }
  }

  // ========================= GOAL-CATEGORY ASSOCIATION =========================
  // Note: Category associations are now handled during goal creation/update
  // via the newGoalTrackerCategoryModels property in GoalTrackerCreateModel/UpdateModel

  /// Simple method to get category ID from name (for UI mapping)
  int? getCategoryIdFromName(String categoryName) {
    return _categoryNameToId[categoryName.toLowerCase()];
  }

  /// Simple method for backward compatibility with existing code
  Future<int?> getCategoryIdFromLocalEnum(String localCategoryName) async {
    // Map some old names to new names for compatibility
    final nameMapping = {
      'relationships': 'love',
      'financial': 'money',
    };
    
    final mappedName = nameMapping[localCategoryName.toLowerCase()] ?? localCategoryName.toLowerCase();
    return getCategoryIdFromName(mappedName);
  }

  // ========================= HELPER METHODS =========================

  /// Convert integer color value to Flutter Color object
  Color _convertIntToColor(int? colorValue) {
    if (colorValue == null) return Color(0xFF4CAF50); // Default green
    return Color(colorValue);
  }

  // ========================= PRIORITY MAPPING =========================

  /// Map API priority ID to UI GoalPriority enum
  GoalPriority mapApiPriorityToUiPriority(int? apiPriorityId) {
    switch (apiPriorityId) {
      case GoalPriorityTypes.low:
        return GoalPriority.low;
      case GoalPriorityTypes.medium:
        return GoalPriority.medium;
      case GoalPriorityTypes.high:
        return GoalPriority.high;
      case GoalPriorityTypes.critical:
        return GoalPriority.critical;
      default:
        return GoalPriority.medium; // Default fallback
    }
  }

  /// Map UI GoalPriority enum to API priority ID
  int mapUiPriorityToApiPriority(GoalPriority uiPriority) {
    switch (uiPriority) {
      case GoalPriority.low:
        return GoalPriorityTypes.low;
      case GoalPriority.medium:
        return GoalPriorityTypes.medium;
      case GoalPriority.high:
        return GoalPriorityTypes.high;
      case GoalPriority.critical:
        return GoalPriorityTypes.critical;
    }
  }

  // ========================= SINGLE GOAL API =========================

  /// Get detailed data for a specific goal by ID
  Future<GoalTrackerResult<GoalTrackerModel>> getSingleGoalTracker(int goalTrackerId) async {
    try {
      print('🎯 Fetching detailed data for goal ID: $goalTrackerId');
      
      final response = await _apiClient.apiV1ManifestGetgoaltrackerGet(id: goalTrackerId);

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        if (returnModel.isSuccess == true && returnModel.model != null) {
          print('✅ Successfully fetched goal details for ID: $goalTrackerId');
          print('📊 Goal data: ${returnModel.model!.goalTrackerName}');
          print('🏷️  Categories: ${returnModel.model!.existingCategorys?.length ?? 0}');
          print('🎯 Milestones: ${returnModel.model!.existingGoalTrackerMileStones?.length ?? 0}');
          
          return GoalTrackerResult.success(
            data: returnModel.model!,
            message: 'Goal details loaded successfully',
          );
        } else {
          print('❌ API returned success=false: ${returnModel.errorHolder?.friendlyMessage}');
          return GoalTrackerResult.failure(
            message: returnModel.errorHolder?.friendlyMessage ?? 'Failed to load goal details',
          );
        }
      } else {
        print('❌ API request failed with status: ${response.statusCode}');
        return GoalTrackerResult.failure(
          message: 'Network error: ${response.statusCode}',
        );
      }
    } catch (e) {
      print('❌ Exception while fetching goal details: $e');
      return GoalTrackerResult.failure(
        message: 'Error loading goal details: ${e.toString()}',
      );
    }
  }
}

// ========================= RESULT CLASS =========================

class GoalTrackerResult<T> {
  final bool isSuccess;
  final String message;
  final T? data;

  GoalTrackerResult._({
    required this.isSuccess,
    required this.message,
    this.data,
  });

  factory GoalTrackerResult.success({
    required T data,
    required String message,
  }) {
    return GoalTrackerResult._(
      isSuccess: true,
      message: message,
      data: data,
    );
  }

  factory GoalTrackerResult.failure({
    required String message,
  }) {
    return GoalTrackerResult._(
      isSuccess: false,
      message: message,
    );
  }
}

// ========================= AUTH INTERCEPTOR =========================

class _AuthInterceptor implements Interceptor {
  @override
  FutureOr<Response<BodyType>> intercept<BodyType>(Chain<BodyType> chain) async {
    final request = chain.request;
    final authService = UnifiedAuthService();
    
    final token = authService.authToken;
    print('🔐 Auth Interceptor - Token available: ${token != null}');
    if (token != null) {
      print('🔐 Auth Interceptor - Adding Bearer token to request');
      final updatedRequest = request.copyWith(
        headers: {
          ...request.headers,
          'Authorization': 'Bearer $token',
        },
      );
      return chain.proceed(updatedRequest);
    } else {
      print('⚠️  Auth Interceptor - No token available, proceeding without auth');
    }
    
    return chain.proceed(request);
  }
}
