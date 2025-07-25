import 'dart:async';
import 'server_change_detector.dart';
import 'app_event_bus.dart';
import 'unified_auth_service.dart';
import 'affirmation_cache_service.dart';

/// Universal sync service that monitors all app data for server changes
/// Uses client-side change detection without requiring server API modifications
class UniversalSyncService {
  static final UniversalSyncService _instance = UniversalSyncService._internal();
  factory UniversalSyncService() => _instance;
  UniversalSyncService._internal();

  final ServerChangeDetector _detector = ServerChangeDetector();
  final UnifiedAuthService _authService = UnifiedAuthService();
  
  Timer? _syncTimer;
  bool _isActive = true;
  bool _isSyncing = false;
  
  // Base URL for all API endpoints
  static const String baseUrl = 'http://manifestation-qa-1.senthilguru.net';
  
  // Configuration for all app data types
  final Map<String, SyncConfig> _syncConfigs = {
    'affirmations': SyncConfig(
      endpoint: '/api/v1/manifest/getdailyaffirmationcategorys',
      invalidateCache: () async {
        await AffirmationCacheService().invalidateCache();
      },
      enabled: true,
      priority: SyncPriority.high,
    ),
    'goals': SyncConfig(
      endpoint: '/api/v1/manifest/getgoaltrackers',
      invalidateCache: () async {
        // TODO: Add goal tracker cache invalidation when implemented
        print('🎯 Goals cache invalidation (placeholder)');
      },
      enabled: true,
      priority: SyncPriority.high,
    ),
    'vision_boards': SyncConfig(
      endpoint: '/api/v1/manifest/getvisionboardpictures',
      invalidateCache: () async {
        // TODO: Add vision board cache invalidation when implemented
        print('🎨 Vision board cache invalidation (placeholder)');
      },
      enabled: true,
      priority: SyncPriority.medium,
    ),
    'categories': SyncConfig(
      endpoint: '/api/v1/manifest/getcategorys',
      invalidateCache: () async {
        // Categories might be cached by multiple services
        await AffirmationCacheService().invalidateCache();
        print('📂 Categories cache invalidation');
      },
      enabled: true,
      priority: SyncPriority.medium,
    ),
    'journal': SyncConfig(
      endpoint: '/api/v1/manifest/getjournalentries',
      invalidateCache: () async {
        // TODO: Add journal cache invalidation when implemented
        print('📝 Journal cache invalidation (placeholder)');
      },
      enabled: false, // Disable until journal caching is implemented
      priority: SyncPriority.low,
    ),
  };
  
  /// Start the periodic sync process
  void startPeriodicSync() {
    if (_syncTimer != null) {
      _syncTimer!.cancel();
    }
    
    // Check every 3 minutes when app is active
    _syncTimer = Timer.periodic(Duration(minutes: 3), (_) {
      if (_isActive && !_isSyncing) {
        checkAllDataForChanges();
      }
    });
    
    print('🔄 Universal sync service started (checking every 3 minutes)');
  }
  
  /// Stop the periodic sync process
  void stopPeriodicSync() {
    _syncTimer?.cancel();
    _syncTimer = null;
    print('⏹️ Universal sync service stopped');
  }
  
  /// Set whether the app is currently active
  void setAppActive(bool active) {
    _isActive = active;
    if (active) {
      // When app becomes active, check for changes immediately
      Future.delayed(Duration(seconds: 1), () {
        if (_isActive && !_isSyncing) {
          checkAllDataForChanges();
        }
      });
    }
  }
  
  /// Check all enabled data types for server changes
  Future<void> checkAllDataForChanges() async {
    if (_isSyncing) {
      print('🔄 Sync already in progress, skipping...');
      return;
    }
    
    if (!_isUserLoggedIn()) {
      print('👤 User not logged in, skipping sync');
      return;
    }
    
    _isSyncing = true;
    final startTime = DateTime.now();
    
    print('🔍 Checking all data for server changes...');
    
    // Get enabled sync configs sorted by priority
    final enabledConfigs = _syncConfigs.entries
        .where((entry) => entry.value.enabled)
        .toList()
      ..sort((a, b) => b.value.priority.index.compareTo(a.value.priority.index));
    
    final dataTypes = enabledConfigs.map((e) => e.key).toList();
    AppEventBus.instance.fire(SyncStartedEvent(dataTypes));
    
    final results = <String, bool>{};
    
    // Check each data type
    for (final entry in enabledConfigs) {
      final dataType = entry.key;
      final config = entry.value;
      
      try {
        final hasChanged = await _checkDataType(dataType, config);
        results[dataType] = true;
        
        if (hasChanged) {
          print('📱 Server has newer $dataType data');
        }
      } catch (e) {
        print('⚠️ Failed to check $dataType: $e');
        results[dataType] = false;
        AppEventBus.instance.fire(SyncErrorEvent(dataType, e.toString()));
      }
    }
    
    final duration = DateTime.now().difference(startTime);
    AppEventBus.instance.fire(SyncCompletedEvent(results, duration));
    
    _isSyncing = false;
    
    print('✅ Sync completed in ${duration.inMilliseconds}ms - checked ${results.length} data types');
  }
  
  /// Check a specific data type for changes
  Future<bool> _checkDataType(String dataType, SyncConfig config) async {
    try {
      final fullEndpoint = '$baseUrl${config.endpoint}';
      
      // Add user ID to endpoint if user is logged in
      final userId = _authService.currentUser?.userId;
      final endpointWithUser = userId != null 
          ? '$fullEndpoint?userId=$userId'
          : fullEndpoint;
      
      final hasChanged = await _detector.hasServerDataChanged(
        endpointWithUser,
        dataType,
      );
      
      if (hasChanged) {
        print('📡 Server change detected for $dataType, invalidating cache...');
        
        // Invalidate the cache
        await config.invalidateCache();
        
        // Notify the app about the change
        AppEventBus.instance.fire(DataChangedEvent(dataType, details: 'Server data updated'));
        AppEventBus.instance.fire(CacheInvalidatedEvent(dataType, 'Server data changed'));
        
        return true;
      }
      
      return false;
    } catch (e) {
      print('⚠️ Error checking $dataType: $e');
      rethrow;
    }
  }
  
  /// Check for changes before loading a specific screen
  Future<void> checkBeforeScreenLoad(String dataType) async {
    final config = _syncConfigs[dataType];
    if (config == null || !config.enabled) {
      return;
    }
    
    if (!_isUserLoggedIn()) {
      return;
    }
    
    try {
      print('🔍 Checking $dataType before screen load...');
      await _checkDataType(dataType, config);
    } catch (e) {
      print('⚠️ Failed to check $dataType before screen load: $e');
    }
  }
  
  /// Force a sync for a specific data type
  Future<bool> forceSyncDataType(String dataType) async {
    final config = _syncConfigs[dataType];
    if (config == null) {
      print('⚠️ Unknown data type: $dataType');
      return false;
    }
    
    if (!_isUserLoggedIn()) {
      print('👤 User not logged in, cannot force sync');
      return false;
    }
    
    try {
      print('🔄 Force syncing $dataType...');
      
      // Clear stored state for this data type to force a fresh check
      await _detector.initialize();
      // Note: We'd need to add a method to clear specific data type state
      
      final hasChanged = await _checkDataType(dataType, config);
      print(hasChanged 
          ? '✅ $dataType force sync completed - changes found'
          : '✅ $dataType force sync completed - no changes');
      
      return true;
    } catch (e) {
      print('⚠️ Failed to force sync $dataType: $e');
      return false;
    }
  }
  
  /// Enable or disable sync for a specific data type
  void setDataTypeEnabled(String dataType, bool enabled) {
    final config = _syncConfigs[dataType];
    if (config != null) {
      config.enabled = enabled;
      print('${enabled ? '✅' : '❌'} $dataType sync ${enabled ? 'enabled' : 'disabled'}');
    }
  }
  
  /// Get sync status for all data types
  Map<String, dynamic> getSyncStatus() {
    return {
      'is_syncing': _isSyncing,
      'is_active': _isActive,
      'is_logged_in': _isUserLoggedIn(),
      'enabled_types': _syncConfigs.entries
          .where((e) => e.value.enabled)
          .map((e) => e.key)
          .toList(),
      'disabled_types': _syncConfigs.entries
          .where((e) => !e.value.enabled)
          .map((e) => e.key)
          .toList(),
      'detector_debug': _detector.getDebugInfo(),
    };
  }
  
  /// Reset all sync state (useful for logout or debugging)
  Future<void> resetSyncState() async {
    print('🗑️ Resetting sync state...');
    await _detector.clearStoredState();
    _isSyncing = false;
    print('✅ Sync state reset complete');
  }
  
  /// Check if user is logged in
  bool _isUserLoggedIn() {
    final user = _authService.currentUser;
    final token = _authService.authToken;
    return user != null && token != null && token.isNotEmpty;
  }
  
  /// Dispose of the sync service
  void dispose() {
    stopPeriodicSync();
  }
}

/// Configuration for syncing a specific data type
class SyncConfig {
  final String endpoint;
  final Future<void> Function() invalidateCache;
  bool enabled;
  final SyncPriority priority;
  
  SyncConfig({
    required this.endpoint,
    required this.invalidateCache,
    this.enabled = true,
    this.priority = SyncPriority.medium,
  });
}

/// Priority levels for sync operations
enum SyncPriority {
  low,
  medium,
  high,
}