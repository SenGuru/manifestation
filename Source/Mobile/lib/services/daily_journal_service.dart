import 'package:chopper/chopper.dart';
import 'package:mistel/services/unified_auth_service.dart';
import 'dart:async';
import '../api/generated/code/manifestation.swagger.dart';

class DailyJournalService {
  static final DailyJournalService _instance = DailyJournalService._internal();
  factory DailyJournalService() => _instance;
  DailyJournalService._internal();

  late Manifestation _apiClient;
  final UnifiedAuthService _authService = UnifiedAuthService();

  void initialize({String? baseUrl}) {
    _apiClient = Manifestation.create(
      baseUrl: Uri.parse(baseUrl ?? 'https://manifestation-qa-1.senthilguru.net'),
      interceptors: [_AuthInterceptor()],
    );
  }

  /// Manual method to run constraint debugging - call this when app starts
  Future<void> runConstraintDebugging() async {
    print('🔧 MANUAL CONSTRAINT DEBUGGING INITIATED');
    await testJournalCreationParameters();
  }

  // ========================= DAILY JOURNAL CRUD =========================

  /// Get all daily journals for the current user
  Future<DailyJournalResult<List<DailyJournalModel>>> getUserDailyJournals() async {
    try {
      final response = await _apiClient.apiV1ManifestGetdailyjournalbyuseridGet(
        userId: _authService.currentUser?.userId,
      );

      print('📖 Get user journals API response:');
      print('   Status Code: ${response.statusCode}');
      print('   Is Successful: ${response.isSuccessful}');

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        print('📖 API Response details:');
        print('   IsSuccess: ${returnModel.isSuccess}');
        print('   Journal count: ${returnModel.model?.length ?? 0}');
        
        if (returnModel.isSuccess == true && returnModel.model != null) {
          return DailyJournalResult.success(
            data: returnModel.model!,
            message: 'Daily journals loaded successfully',
          );
        } else {
          return DailyJournalResult.failure(
            message: returnModel.errorHolder?.toString() ?? 'Failed to load daily journals',
          );
        }
      } else {
        return DailyJournalResult.failure(
          message: 'Network error: ${response.statusCode}',
        );
      }
    } catch (e) {
      print('📖 Exception loading daily journals: $e');
      return DailyJournalResult.failure(
        message: 'Error loading daily journals: ${e.toString()}',
      );
    }
  }

  /// Get a specific daily journal by ID
  Future<DailyJournalResult<DailyJournalModel>> getDailyJournal(int journalId) async {
    try {
      final response = await _apiClient.apiV1ManifestGetdailyjournalGet(id: journalId);

      print('📖 Get journal by ID API response:');
      print('   Status Code: ${response.statusCode}');
      print('   Is Successful: ${response.isSuccessful}');

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        if (returnModel.isSuccess == true && returnModel.model != null) {
          return DailyJournalResult.success(
            data: returnModel.model!,
            message: 'Daily journal loaded successfully',
          );
        } else {
          return DailyJournalResult.failure(
            message: returnModel.errorHolder?.toString() ?? 'Failed to load daily journal',
          );
        }
      } else {
        return DailyJournalResult.failure(
          message: 'Network error: ${response.statusCode}',
        );
      }
    } catch (e) {
      print('📖 Exception loading daily journal: $e');
      return DailyJournalResult.failure(
        message: 'Error loading daily journal: ${e.toString()}',
      );
    }
  }

  /// Create a new daily journal entry
  Future<DailyJournalResult<DailyJournalModel>> createDailyJournal({
    required DateTime journalDate,
    required int moodTypeId,
    String? gratitude1,
    String? gratitude2,
    String? gratitude3,
    String? gratitude4,
    String? gratitude5,
    String? reflection,
    String? tomorrowsIntentions,
  }) async {
    try {
      // Validate required parameters
      final currentUser = _authService.currentUser;
      if (currentUser?.userId == null || currentUser!.userId! <= 0) {
        return DailyJournalResult.failure(
          message: 'User must be authenticated to create journal entry',
        );
      }

      // Validate moodTypeId is one of the valid backend enum values
      const validMoodIds = [3700001, 3700002, 3700003, 3700004, 3700005];
      if (!validMoodIds.contains(moodTypeId)) {
        return DailyJournalResult.failure(
          message: 'Valid mood selection is required (backend enum values: 3700001-3700005, got: $moodTypeId)',
        );
      }

      // Validate gratitude text lengths (database field limits)
      final gratitudes = [gratitude1, gratitude2, gratitude3, gratitude4, gratitude5];
      for (int i = 0; i < gratitudes.length; i++) {
        final gratitude = gratitudes[i];
        if (gratitude != null && gratitude.length > 500) {
          return DailyJournalResult.failure(
            message: 'Gratitude ${i + 1} is too long (max 500 characters)',
          );
        }
      }

      // Validate reflection and intentions lengths
      if (reflection != null && reflection.length > 2000) {
        return DailyJournalResult.failure(
          message: 'Reflection is too long (max 2000 characters)',
        );
      }

      if (tomorrowsIntentions != null && tomorrowsIntentions.length > 2000) {
        return DailyJournalResult.failure(
          message: 'Tomorrow\'s intentions is too long (max 2000 characters)',
        );
      }

      // Validate date is not in the future beyond reasonable limits
      final now = DateTime.now();
      if (journalDate.isAfter(now.add(Duration(days: 1)))) {
        return DailyJournalResult.failure(
          message: 'Journal date cannot be more than 1 day in the future',
        );
      }

      print('📖 Creating daily journal with parameters:');
      print('   Date: ${journalDate.toIso8601String()}');
      print('   User ID: ${currentUser.userId}');
      print('   Mood Type ID: $moodTypeId');
      print('   Gratitudes: ${[gratitude1, gratitude2, gratitude3, gratitude4, gratitude5].where((g) => g?.isNotEmpty == true).length} entries');
      print('   Has Reflection: ${reflection?.isNotEmpty == true}');
      print('   Has Intentions: ${tomorrowsIntentions?.isNotEmpty == true}');

      // Check if an entry already exists for this date to prevent duplicate constraint violation
      final dateOnly = DateTime(journalDate.year, journalDate.month, journalDate.day);
      final existingResult = await getDailyJournalForDate(dateOnly);
      if (existingResult.isSuccess && existingResult.data != null) {
        print('📖 ⚠️  Entry already exists for this date - this would cause a duplicate constraint violation');
        print('📖 🔄 Converting create to update operation...');
        
        // Convert to update operation instead
        final existingEntry = existingResult.data!;
        final existingId = existingEntry.dailyJournalId;
        if (existingId != null) {
          return await updateDailyJournal(
            journalId: existingId,
            journalDate: journalDate,
            moodTypeId: moodTypeId,
            gratitude1: gratitude1,
            gratitude2: gratitude2,
            gratitude3: gratitude3,
            gratitude4: gratitude4,
            gratitude5: gratitude5,
            reflection: reflection,
            tomorrowsIntentions: tomorrowsIntentions,
          );
        }
      }

      // Clean and validate fields before sending to API
      final cleanGratitude1 = gratitude1?.trim();
      final cleanGratitude2 = gratitude2?.trim();
      final cleanGratitude3 = gratitude3?.trim();
      final cleanGratitude4 = gratitude4?.trim();
      final cleanGratitude5 = gratitude5?.trim();
      final cleanReflection = reflection?.trim();
      final cleanIntentions = tomorrowsIntentions?.trim();

      // CRITICAL: API requires gratitude1 to be non-null/non-empty
      // If no gratitudes provided, use a default message
      String finalGratitude1 = cleanGratitude1 ?? '';
      if (finalGratitude1.isEmpty) {
        // If gratitude1 is null or empty, check if we have other gratitudes to move up
        if (cleanGratitude2?.isNotEmpty == true) {
          finalGratitude1 = cleanGratitude2!;
        } else if (cleanGratitude3?.isNotEmpty == true) {
          finalGratitude1 = cleanGratitude3!;
        } else if (cleanGratitude4?.isNotEmpty == true) {
          finalGratitude1 = cleanGratitude4!;
        } else if (cleanGratitude5?.isNotEmpty == true) {
          finalGratitude1 = cleanGratitude5!;
        } else {
          // If no gratitudes at all, use a default
          finalGratitude1 = 'Today\'s experiences';
        }
      }

      print('📖 Final gratitude1 after validation: "$finalGratitude1"');

      final createModel = DailyJournalCreateModel(
        dailyJournalDate: journalDate,
        userId: currentUser.userId,
        predefinedMoodTypeId: moodTypeId, // moodTypeId should already be the correct backend enum value
        gratitude1: finalGratitude1, // Always ensure this is non-empty
        gratitude2: (cleanGratitude2?.isEmpty == true) ? null : cleanGratitude2,
        gratitude3: (cleanGratitude3?.isEmpty == true) ? null : cleanGratitude3,
        gratitude4: (cleanGratitude4?.isEmpty == true) ? null : cleanGratitude4,
        gratitude5: (cleanGratitude5?.isEmpty == true) ? null : cleanGratitude5,
        yourReflection: (cleanReflection?.isEmpty == true) ? null : cleanReflection,
        tommorowsIntentions: (cleanIntentions?.isEmpty == true) ? null : cleanIntentions,
      );

      print('📖 Journal creation model JSON: ${createModel.toJson()}');

      final response = await _apiClient.apiV1ManifestCreatedailyjournalPost(body: createModel);

      print('📖 Create journal API response:');
      print('   Status Code: ${response.statusCode}');
      print('   Is Successful: ${response.isSuccessful}');
      print('   Response Headers: ${response.headers}');
      print('   Response Body Type: ${response.body.runtimeType}');
      print('   Response Body: ${response.body?.toString()}');
      print('   Response Error: ${response.error?.toString()}');

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        print('📖 API Response details:');
        print('   IsSuccess: ${returnModel.isSuccess}');
        print('   Model: ${returnModel.model?.toString()}');
        print('   Success Message: ${returnModel.successMessage}');
        print('   Error Holder: ${returnModel.errorHolder?.toString()}');
        
        // Try to extract more detailed error information
        if (returnModel.errorHolder != null) {
          final errorHolder = returnModel.errorHolder!;
          print('📖 Detailed Error Analysis:');
          print('   Error Type: ${errorHolder.runtimeType}');
          print('   Error JSON: ${errorHolder.toJson()}');
          if (errorHolder.friendlyMessage != null) {
            print('   Friendly Message: ${errorHolder.friendlyMessage}');
          }
          if (errorHolder.exception != null) {
            print('   Exception: ${errorHolder.exception}');
          }
          // Note: stackTrace property doesn't exist on ReturnError type
        }
        
        if (returnModel.isSuccess == true && returnModel.model != null) {
          return DailyJournalResult.success(
            data: returnModel.model!,
            message: 'Daily journal created successfully',
          );
        } else {
          // Return detailed error message
          String errorMessage = 'Failed to create daily journal';
          if (returnModel.errorHolder?.friendlyMessage != null) {
            errorMessage = returnModel.errorHolder!.friendlyMessage!;
          } else if (returnModel.errorHolder?.exception != null) {
            errorMessage = returnModel.errorHolder!.exception!.toString();
          }
          
          return DailyJournalResult.failure(
            message: errorMessage,
          );
        }
      } else {
        // Handle HTTP-level errors
        String errorMessage = 'Network error: ${response.statusCode}';
        if (response.error != null) {
          errorMessage += ' - ${response.error.toString()}';
        }
        
        print('📖 HTTP Error Details:');
        print('   Error Object: ${response.error?.runtimeType}');
        print('   Error String: ${response.error?.toString()}');
        
        return DailyJournalResult.failure(
          message: errorMessage,
        );
      }
    } catch (e) {
      print('📖 Journal creation exception: $e');
      return DailyJournalResult.failure(
        message: 'Error creating daily journal: ${e.toString()}',
      );
    }
  }

  /// Update an existing daily journal entry
  Future<DailyJournalResult<DailyJournalModel>> updateDailyJournal({
    required int journalId,
    DateTime? journalDate,
    int? moodTypeId,
    String? gratitude1,
    String? gratitude2,
    String? gratitude3,
    String? gratitude4,
    String? gratitude5,
    String? reflection,
    String? tomorrowsIntentions,
  }) async {
    try {
      final currentUser = _authService.currentUser;
      if (currentUser?.userId == null || currentUser!.userId! <= 0) {
        return DailyJournalResult.failure(
          message: 'User must be authenticated to update journal entry',
        );
      }

      print('📖 Updating daily journal with parameters:');
      print('   Journal ID: $journalId');
      print('   User ID: ${currentUser.userId}');
      print('   New Date: ${journalDate?.toIso8601String()}');
      print('   New Mood Type ID: $moodTypeId');

      // Clean gratitudes and ensure gratitude1 is not empty (same logic as create)
      final cleanGratitude1 = gratitude1?.trim();
      final cleanGratitude2 = gratitude2?.trim();
      final cleanGratitude3 = gratitude3?.trim();
      final cleanGratitude4 = gratitude4?.trim();
      final cleanGratitude5 = gratitude5?.trim();

      // Ensure gratitude1 is not empty for updates too
      String finalGratitude1 = cleanGratitude1 ?? '';
      if (finalGratitude1.isEmpty) {
        // If gratitude1 is null or empty, check if we have other gratitudes to move up
        if (cleanGratitude2?.isNotEmpty == true) {
          finalGratitude1 = cleanGratitude2!;
        } else if (cleanGratitude3?.isNotEmpty == true) {
          finalGratitude1 = cleanGratitude3!;
        } else if (cleanGratitude4?.isNotEmpty == true) {
          finalGratitude1 = cleanGratitude4!;
        } else if (cleanGratitude5?.isNotEmpty == true) {
          finalGratitude1 = cleanGratitude5!;
        } else {
          // If no gratitudes at all, use a default
          finalGratitude1 = 'Today\'s experiences';
        }
      }

      print('📖 Update - Final gratitude1: "$finalGratitude1"');

      final updateModel = DailyJournalUpdateModel(
        dailyJournalId: journalId,
        dailyJournalDate: journalDate,
        userId: currentUser.userId,
        predefinedMoodTypeId: moodTypeId,
        gratitude1: finalGratitude1, // Always ensure this is non-empty
        gratitude2: (cleanGratitude2?.isEmpty == true) ? null : cleanGratitude2,
        gratitude3: (cleanGratitude3?.isEmpty == true) ? null : cleanGratitude3,
        gratitude4: (cleanGratitude4?.isEmpty == true) ? null : cleanGratitude4,
        gratitude5: (cleanGratitude5?.isEmpty == true) ? null : cleanGratitude5,
        yourReflection: reflection?.trim(),
        tommorowsIntentions: tomorrowsIntentions?.trim(),
      );

      final response = await _apiClient.apiV1ManifestUpdatedailyjournalPost(body: updateModel);

      print('📖 Update journal API response:');
      print('   Status Code: ${response.statusCode}');
      print('   Is Successful: ${response.isSuccessful}');

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        if (returnModel.isSuccess == true && returnModel.model != null) {
          return DailyJournalResult.success(
            data: returnModel.model!,
            message: 'Daily journal updated successfully',
          );
        } else {
          return DailyJournalResult.failure(
            message: returnModel.errorHolder?.toString() ?? 'Failed to update daily journal',
          );
        }
      } else {
        return DailyJournalResult.failure(
          message: 'Network error: ${response.statusCode}',
        );
      }
    } catch (e) {
      print('📖 Journal update exception: $e');
      return DailyJournalResult.failure(
        message: 'Error updating daily journal: ${e.toString()}',
      );
    }
  }

  /// Delete a daily journal entry (mark as deleted)
  Future<DailyJournalResult<bool>> deleteDailyJournal(int journalId) async {
    try {
      print('🗑️  API: Marking daily journal as deleted - ID: $journalId');
      
      // Add timeout to prevent hanging
      final response = await _apiClient.apiV1ManifestMarkdailyjournalasdeletedDelete(id: journalId)
          .timeout(Duration(seconds: 30));

      print('🗑️  API Response received!');
      print('🗑️  API Response - Status Code: ${response.statusCode}');
      print('🗑️  API Response - Is Successful: ${response.isSuccessful}');
      print('🗑️  API Response - Body: ${response.body?.toString()}');

      if (response.isSuccessful && response.body != null) {
        final returnModel = response.body!;
        
        print('🗑️  API Response - IsSuccess: ${returnModel.isSuccess}');
        print('🗑️  API Response - Message: ${returnModel.successMessage}');
        print('🗑️  API Response - Error: ${returnModel.errorHolder}');
        
        if (returnModel.isSuccess == true) {
          return DailyJournalResult.success(
            data: true,
            message: 'Daily journal marked as deleted successfully',
          );
        } else {
          return DailyJournalResult.failure(
            message: returnModel.errorHolder?.toString() ?? 'Failed to mark daily journal as deleted',
          );
        }
      } else {
        return DailyJournalResult.failure(
          message: 'Network error: ${response.statusCode}',
        );
      }
    } catch (e) {
      print('🗑️  API Exception: $e');
      
      if (e.toString().contains('TimeoutException')) {
        return DailyJournalResult.failure(
          message: 'Delete request timed out - the journal may have been deleted successfully',
        );
      }
      
      return DailyJournalResult.failure(
        message: 'Error marking daily journal as deleted: ${e.toString()}',
      );
    }
  }

  /// Get daily journal for a specific date
  Future<DailyJournalResult<DailyJournalModel?>> getDailyJournalForDate(DateTime date) async {
    try {
      // Get all user journals and filter by date
      final allJournalsResult = await getUserDailyJournals();
      
      if (allJournalsResult.isSuccess && allJournalsResult.data != null) {
        final dateOnly = DateTime(date.year, date.month, date.day);
        
        final matchingJournal = allJournalsResult.data!.firstWhere(
          (journal) {
            if (journal.dailyJournalDate == null) return false;
            final journalDateOnly = DateTime(
              journal.dailyJournalDate!.year,
              journal.dailyJournalDate!.month, 
              journal.dailyJournalDate!.day
            );
            return journalDateOnly.isAtSameMomentAs(dateOnly);
          },
          orElse: () => DailyJournalModel(),
        );

        // Check if we found a valid journal (has an ID)
        if (matchingJournal.dailyJournalId != null) {
          return DailyJournalResult.success(
            data: matchingJournal,
            message: 'Journal found for date',
          );
        } else {
          return DailyJournalResult.success(
            data: null,
            message: 'No journal found for date',
          );
        }
      } else {
        return DailyJournalResult.failure(
          message: allJournalsResult.message,
        );
      }
    } catch (e) {
      print('📖 Exception getting journal for date: $e');
      return DailyJournalResult.failure(
        message: 'Error getting journal for date: ${e.toString()}',
      );
    }
  }

  // ========================= HELPER METHODS =========================

  /// Map mood enum index to API mood type ID using backend PredefinedMoodTypes enum
  /// Backend enum: Terrible=3700001, Bad=3700002, Neutral=3700003, Good=3700004, Amazing=3700005
  int mapMoodToApiMoodTypeId(int moodIndex) {
    // Mood enum: terrible=0, bad=1, neutral=2, good=3, amazing=4
    // Backend PredefinedMoodTypes enum values
    const moodTypeIds = [
      3700001, // Terrible
      3700002, // Bad  
      3700003, // Neutral
      3700004, // Good
      3700005, // Amazing
    ];
    return moodTypeIds[moodIndex.clamp(0, 4)];
  }

  /// Map API mood type ID back to mood enum index using backend PredefinedMoodTypes enum
  int mapApiMoodTypeIdToMood(int apiMoodTypeId) {
    // Backend PredefinedMoodTypes enum: 3700001-3700005
    // Map back to mood enum: terrible=0, bad=1, neutral=2, good=3, amazing=4
    switch (apiMoodTypeId) {
      case 3700001: return 0; // Terrible
      case 3700002: return 1; // Bad
      case 3700003: return 2; // Neutral  
      case 3700004: return 3; // Good
      case 3700005: return 4; // Amazing
      default: return 2; // Default to Neutral if unknown ID
    }
  }

  /// Test method to try different journal creation parameters to identify constraints
  Future<void> testJournalCreationParameters() async {
    print('\n🧪 DEBUGGING JOURNAL CREATION CONSTRAINTS');
    print('🧪 ========================================');
    
    try {
      final currentUser = _authService.currentUser;
      final userId = currentUser?.userId ?? 1;
      final today = DateTime.now();
      
      print('🧪 Current User ID: $userId');
      print('🧪 Current Date: ${today.toIso8601String()}');
      
      // Test 0: Check if entry already exists for today (duplicate constraint check)
      print('\n🧪 Test 0: Checking for existing journal entry for today');
      try {
        final existingCheck = await getUserDailyJournals();
        if (existingCheck.isSuccess && existingCheck.data != null) {
          final todayEntries = existingCheck.data!.where((journal) {
            if (journal.dailyJournalDate == null) return false;
            final journalDate = journal.dailyJournalDate!;
            final todayDate = DateTime(today.year, today.month, today.day);
            final entryDate = DateTime(journalDate.year, journalDate.month, journalDate.day);
            return entryDate.isAtSameMomentAs(todayDate);
          }).toList();
          
          print('   📋 Found ${todayEntries.length} existing entries for today');
          if (todayEntries.isNotEmpty) {
            print('   ⚠️  DUPLICATE CONSTRAINT: Entry already exists for today!');
            print('   📄 Existing entry ID: ${todayEntries.first.dailyJournalId}');
            print('   📅 Existing entry date: ${todayEntries.first.dailyJournalDate}');
            print('   👤 Existing entry user: ${todayEntries.first.userId}');
            print('   🚨 This explains the PostgreSQL constraint violation!');
          } else {
            print('   ✅ No existing entries found for today - should be safe to create');
          }
        }
      } catch (e) {
        print('   💥 Error checking existing entries: $e');
      }
      
      // Test 1: Minimal parameters WITH required gratitude1
      print('\n🧪 Test 1: Minimal parameters WITH required gratitude1');
      try {
        final testModel1 = DailyJournalCreateModel(
          dailyJournalDate: today,
          userId: userId,
          predefinedMoodTypeId: 3700003, // Neutral mood using correct backend enum
          gratitude1: 'Test gratitude - required field', // Now including required field
        );
        print('   📋 Test Model: ${testModel1.toJson()}');
        final response1 = await _apiClient.apiV1ManifestCreatedailyjournalPost(body: testModel1);
        print('   ✅ HTTP Success: ${response1.isSuccessful}');
        print('   ✅ API Success: ${response1.body?.isSuccess}');
        print('   📄 Response: ${response1.body?.toString()}');
        if (response1.body?.errorHolder != null) {
          print('   ❌ Error: ${response1.body?.errorHolder?.friendlyMessage}');
          print('   ❌ Error Details: ${response1.body?.errorHolder?.toString()}');
        }
        if (response1.body?.isSuccess == true) {
          print('   🎉 SUCCESS! Minimal parameters work.');
          return; // If this works, no need to continue testing
        }
      } catch (e) {
        print('   💥 Exception: $e');
      }
      
      // Test 2: Different mood types (with required gratitude1)
      print('\n🧪 Test 2: Testing different mood types (with required gratitude1)');
      final correctMoodIds = [3700001, 3700002, 3700003, 3700004, 3700005]; // Backend enum values
      for (int i = 0; i < correctMoodIds.length; i++) {
        final moodId = correctMoodIds[i];
        try {
          print('   Testing correct mood ID: $moodId');
          final testModel = DailyJournalCreateModel(
            dailyJournalDate: today,
            userId: userId,
            predefinedMoodTypeId: moodId,
            gratitude1: 'Test gratitude for mood $moodId', // Include required field
          );
          final response = await _apiClient.apiV1ManifestCreatedailyjournalPost(body: testModel);
          print('     Result: HTTP=${response.isSuccessful}, API=${response.body?.isSuccess}');
          if (response.body?.errorHolder != null) {
            print('     Error: ${response.body?.errorHolder?.friendlyMessage}');
          }
          if (response.body?.isSuccess == true) {
            print('   🎉 SUCCESS! Mood ID $moodId works.');
            return;
          }
        } catch (e) {
          print('     Exception: $e');
        }
      }
      
      // Test 3: Different user IDs
      print('\n🧪 Test 3: Testing different user IDs');
      final testUserIds = [1, userId, 0, null];
      for (final testUserId in testUserIds) {
        try {
          print('   Testing user ID: $testUserId');
          final testModel = DailyJournalCreateModel(
            dailyJournalDate: today,
            userId: testUserId,
            predefinedMoodTypeId: 3700003, // Neutral mood using correct backend enum
          );
          final response = await _apiClient.apiV1ManifestCreatedailyjournalPost(body: testModel);
          print('     Result: HTTP=${response.isSuccessful}, API=${response.body?.isSuccess}');
          if (response.body?.errorHolder != null) {
            print('     Error: ${response.body?.errorHolder?.friendlyMessage}');
          }
          if (response.body?.isSuccess == true) {
            print('   🎉 SUCCESS! User ID $testUserId works.');
            return;
          }
        } catch (e) {
          print('     Exception: $e');
        }
      }
      
      // Test 4: Different dates
      print('\n🧪 Test 4: Testing different dates');
      final testDates = [
        DateTime.now(),
        DateTime.now().subtract(Duration(days: 1)),
        DateTime(2024, 1, 1),
        DateTime(today.year, today.month, today.day), // Today at midnight
      ];
      for (final testDate in testDates) {
        try {
          print('   Testing date: ${testDate.toIso8601String()}');
          final testModel = DailyJournalCreateModel(
            dailyJournalDate: testDate,
            userId: userId,
            predefinedMoodTypeId: 3700003, // Neutral mood using correct backend enum
          );
          final response = await _apiClient.apiV1ManifestCreatedailyjournalPost(body: testModel);
          print('     Result: HTTP=${response.isSuccessful}, API=${response.body?.isSuccess}');
          if (response.body?.errorHolder != null) {
            print('     Error: ${response.body?.errorHolder?.friendlyMessage}');
          }
          if (response.body?.isSuccess == true) {
            print('   🎉 SUCCESS! Date ${testDate.toIso8601String()} works.');
            return;
          }
        } catch (e) {
          print('     Exception: $e');
        }
      }
      
      // Test 5: Completely empty model
      print('\n🧪 Test 5: Empty model to see required field errors');
      try {
        final testModel = DailyJournalCreateModel();
        print('   📋 Empty Model: ${testModel.toJson()}');
        final response = await _apiClient.apiV1ManifestCreatedailyjournalPost(body: testModel);
        print('   Result: HTTP=${response.isSuccessful}, API=${response.body?.isSuccess}');
        if (response.body?.errorHolder != null) {
          print('   Error: ${response.body?.errorHolder?.friendlyMessage}');
          print('   Error Details: ${response.body?.errorHolder?.toString()}');
        }
      } catch (e) {
        print('   Exception: $e');
      }
      
      // Test 6: Try with a different date to bypass duplicate constraint
      print('\n🧪 Test 6: Testing with different date to bypass duplicate constraint');
      try {
        final differentDate = DateTime(2023, 1, 1); // Use a date far in the past
        print('   Testing with date: ${differentDate.toIso8601String()}');
        final testModel = DailyJournalCreateModel(
          dailyJournalDate: differentDate,
          userId: userId,
          predefinedMoodTypeId: 3700003, // Neutral mood using correct backend enum
          gratitude1: 'Test gratitude with different date',
        );
        final response = await _apiClient.apiV1ManifestCreatedailyjournalPost(body: testModel);
        print('   Result: HTTP=${response.isSuccessful}, API=${response.body?.isSuccess}');
        if (response.body?.errorHolder != null) {
          print('   Error: ${response.body?.errorHolder?.friendlyMessage}');
        }
        if (response.body?.isSuccess == true) {
          print('   🎉 SUCCESS! Different date works - confirms duplicate constraint issue');
          print('   🧪 DIAGNOSIS: The database has a UNIQUE constraint preventing duplicate entries for same user+date');
          return;
        }
      } catch (e) {
        print('   Exception: $e');
      }
      
      // Test 7: Test with completely minimal data to isolate the issue
      print('\n🧪 Test 7: Ultra-minimal test with only required fields');
      try {
        final minimalDate = DateTime(2020, 1, 1); // Far in past
        print('   Testing ultra-minimal model...');
        final testModel = DailyJournalCreateModel(
          dailyJournalDate: minimalDate,
          userId: 1, // Use a simple user ID
          gratitude1: 'test', // Minimal gratitude
        );
        print('   📋 Minimal Model: ${testModel.toJson()}');
        final response = await _apiClient.apiV1ManifestCreatedailyjournalPost(body: testModel);
        print('   Result: HTTP=${response.isSuccessful}, API=${response.body?.isSuccess}');
        if (response.body?.errorHolder != null) {
          print('   Error: ${response.body?.errorHolder?.friendlyMessage}');
          final fullError = response.body!.errorHolder!.toJson();
          print('   Full Error: $fullError');
        }
        if (response.body?.isSuccess == true) {
          print('   🎉 SUCCESS! Ultra-minimal works');
          return;
        }
      } catch (e) {
        print('   Exception: $e');
      }

      print('\n🧪 ========================================');
      print('🧪 ALL TESTS COMPLETED - NO SUCCESS FOUND');
      print('🧪 ========================================');
      
    } catch (e) {
      print('❌ Test framework error: $e');
    }
  }
}

// ========================= RESULT CLASS =========================

class DailyJournalResult<T> {
  final bool isSuccess;
  final String message;
  final T? data;

  DailyJournalResult._({
    required this.isSuccess,
    required this.message,
    this.data,
  });

  factory DailyJournalResult.success({
    required T data,
    required String message,
  }) {
    return DailyJournalResult._(
      isSuccess: true,
      message: message,
      data: data,
    );
  }

  factory DailyJournalResult.failure({
    required String message,
  }) {
    return DailyJournalResult._(
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