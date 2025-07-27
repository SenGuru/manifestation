import 'dart:io';
import 'package:path_provider/path_provider.dart';
import 'affirmation_cache_service.dart';

/// Global cache manager to clear all app caches
class CacheManager {
  static final CacheManager _instance = CacheManager._internal();
  factory CacheManager() => _instance;
  CacheManager._internal();

  /// Clear all application caches
  /// Call this on logout to ensure fresh data on next login
  Future<void> clearAllCaches() async {
    try {
      print('🧹 Clearing all application caches...');
      
      // Clear affirmation cache
      await AffirmationCacheService().invalidateCache();
      
      // Clear all vision board XML files
      await _clearAllVisionBoardXMLFiles();
      
      // TODO: Add other cache clearing here when implemented
      // await GoalTrackerCacheService().invalidateCache();
      // await JournalCacheService().invalidateCache();
      
      print('✅ All caches cleared successfully');
    } catch (e) {
      print('❌ Error clearing caches: $e');
    }
  }

  /// Clear all vision board XML files on logout
  Future<void> _clearAllVisionBoardXMLFiles() async {
    try {
      print('🗑️ Clearing all vision board XML files...');
      
      final directory = await getApplicationDocumentsDirectory();
      final directoryPath = directory.path;
      final dir = Directory(directoryPath);
      
      if (!await dir.exists()) {
        print('📄 No XML files directory found');
        return;
      }
      
      int deletedCount = 0;
      
      // Delete all vision board XML files
      await for (final entity in dir.list()) {
        if (entity is File && entity.path.contains('visionboard_') && entity.path.endsWith('.xml')) {
          try {
            await entity.delete();
            deletedCount++;
            print('🗑️ Deleted: ${entity.path.split('/').last}');
          } catch (e) {
            print('❌ Error deleting ${entity.path}: $e');
          }
        }
      }
      
      print('✅ Deleted $deletedCount vision board XML files');
      
    } catch (e) {
      print('❌ Error clearing vision board XML files: $e');
    }
  }

  /// Example pattern for other services to implement cache-first loading:
  /// 
  /// Future<Result<T>> loadData({bool forceRefresh = false}) async {
  ///   await _cacheService.initialize();
  ///   
  ///   // Check if cache is empty (fresh login)
  ///   if (!_cacheService.hasCachedData) {
  ///     print('📭 No cached data - loading from API...');
  ///     return await _loadFromAPI();
  ///   }
  ///   
  ///   // Use valid cache
  ///   if (!forceRefresh && _cacheService.isCacheValid) {
  ///     print('💾 Using cached data');
  ///     return Result.success(_cacheService.cachedData);
  ///   }
  ///   
  ///   // Load fresh data and cache it
  ///   print('🌐 Loading fresh data from API...');
  ///   return await _loadFromAPI();
  /// }

  /// Check if we have any cached data across the app
  Future<bool> hasAnyCachedData() async {
    try {
      // Check if affirmation cache has data
      final affirmationCache = AffirmationCacheService();
      await affirmationCache.initialize();
      
      if (affirmationCache.hasCachedData) {
        return true;
      }
      
      // TODO: Add checks for other caches when implemented
      
      return false;
    } catch (e) {
      print('Error checking cached data: $e');
      return false;
    }
  }

  /// Get cache status for debugging
  Future<Map<String, dynamic>> getCacheStatus() async {
    try {
      final affirmationCache = AffirmationCacheService();
      await affirmationCache.initialize();
      
      return {
        'affirmations': affirmationCache.getCacheStats(),
        // TODO: Add other cache stats when implemented
      };
    } catch (e) {
      return {'error': e.toString()};
    }
  }
}