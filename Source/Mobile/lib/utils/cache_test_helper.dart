import '../services/cache_manager.dart';
import '../services/affirmation_cache_service.dart';

/// Helper class to test cache functionality
class CacheTestHelper {
  static final CacheTestHelper _instance = CacheTestHelper._internal();
  factory CacheTestHelper() => _instance;
  CacheTestHelper._internal();

  /// Test the complete cache flow
  Future<Map<String, dynamic>> testCacheFlow() async {
    final results = <String, dynamic>{};
    
    try {
      // 1. Check initial cache state
      final cacheManager = CacheManager();
      final affirmationCache = AffirmationCacheService();
      await affirmationCache.initialize();
      
      results['initial_has_data'] = affirmationCache.hasCachedData;
      results['initial_is_valid'] = affirmationCache.isCacheValid;
      results['initial_should_fetch'] = affirmationCache.shouldFetchFresh;
      
      // 2. Clear all caches
      await cacheManager.clearAllCaches();
      
      // 3. Check state after clearing
      await affirmationCache.initialize(); // Reinitialize after clearing
      results['after_clear_has_data'] = affirmationCache.hasCachedData;
      results['after_clear_should_fetch'] = affirmationCache.shouldFetchFresh;
      
      // 4. Get cache status
      results['cache_status'] = await cacheManager.getCacheStatus();
      
      results['test_passed'] = true;
      results['message'] = 'Cache test completed successfully';
      
    } catch (e) {
      results['test_passed'] = false;
      results['error'] = e.toString();
      results['message'] = 'Cache test failed';
    }
    
    return results;
  }

  /// Simulate logout and check cache clearing
  Future<Map<String, dynamic>> testLogoutCacheClearing() async {
    final results = <String, dynamic>{};
    
    try {
      // Before logout - check if we have any data
      final cacheManager = CacheManager();
      results['before_logout_has_data'] = await cacheManager.hasAnyCachedData();
      
      // Simulate logout (just clear caches)
      await cacheManager.clearAllCaches();
      
      // After logout - should have no data
      results['after_logout_has_data'] = await cacheManager.hasAnyCachedData();
      
      // Check specific cache
      final affirmationCache = AffirmationCacheService();
      await affirmationCache.initialize();
      results['affirmation_cache_empty'] = !affirmationCache.hasCachedData;
      results['should_fetch_fresh'] = affirmationCache.shouldFetchFresh;
      
      results['logout_test_passed'] = !results['after_logout_has_data'] && 
                                     results['affirmation_cache_empty'] && 
                                     results['should_fetch_fresh'];
      
    } catch (e) {
      results['logout_test_passed'] = false;
      results['error'] = e.toString();
    }
    
    return results;
  }

  /// Print cache test results in a readable format
  void printTestResults(Map<String, dynamic> results) {
    print('\n=== CACHE TEST RESULTS ===');
    results.forEach((key, value) {
      final emoji = key.contains('passed') && value == true ? '✅' : 
                   key.contains('passed') && value == false ? '❌' : 
                   key.contains('error') ? '⚠️' : '📊';
      print('$emoji $key: $value');
    });
    print('========================\n');
  }
}