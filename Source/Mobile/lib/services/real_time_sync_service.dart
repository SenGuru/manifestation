import 'dart:async';
import 'server_change_detector.dart';
import 'app_event_bus.dart';
import 'unified_auth_service.dart';
import 'affirmation_cache_service.dart';

/// Real-time sync service for instant multi-device synchronization
/// Uses aggressive polling to achieve near real-time sync (10-second detection)
class RealTimeSyncService {
  static final RealTimeSyncService _instance = RealTimeSyncService._internal();
  factory RealTimeSyncService() => _instance;
  RealTimeSyncService._internal();

  final ServerChangeDetector _detector = ServerChangeDetector();
  final UnifiedAuthService _authService = UnifiedAuthService();
  
  Timer? _aggressiveTimer;
  Timer? _backgroundTimer;
  bool _isAppInForeground = true;
  bool _isSyncing = false;
  bool _isEnabled = true;
  
  // Base URL for all API endpoints
  static const String baseUrl = 'http://manifestation-qa-1.senthilguru.net';
  
  // Real-time polling intervals
  static const Duration _foregroundInterval = Duration(seconds: 10);  // Very aggressive
  static const Duration _backgroundInterval = Duration(seconds: 30);  // Moderate
  
  // Configuration for all app data types (same as universal sync)
  final Map<String, RealTimeSyncConfig> _syncConfigs = {
    'affirmations': RealTimeSyncConfig(
      endpoint: '/api/v1/manifest/getdailyaffirmationcategorys',
      invalidateCache: () async {
        await AffirmationCacheService().invalidateCache();
      },
      enabled: true,
      priority: SyncPriority.critical,
    ),
    'goals': RealTimeSyncConfig(
      endpoint: '/api/v1/manifest/getgoaltrackers',
      invalidateCache: () async {
        print('🎯 Goals cache invalidation (placeholder)');
      },
      enabled: true,
      priority: SyncPriority.high,
    ),
    'vision_boards': RealTimeSyncConfig(
      endpoint: '/api/v1/manifest/getvisionboardpictures',
      invalidateCache: () async {
        print('🎨 Vision board cache invalidation (placeholder)');
      },
      enabled: true,
      priority: SyncPriority.high,
    ),
    'categories': RealTimeSyncConfig(
      endpoint: '/api/v1/manifest/getcategorys',
      invalidateCache: () async {
        await AffirmationCacheService().invalidateCache();
        print('📂 Categories cache invalidation');
      },
      enabled: true,
      priority: SyncPriority.medium,
    ),
    'journal': RealTimeSyncConfig(
      endpoint: '/api/v1/manifest/getjournalentries',
      invalidateCache: () async {
        print('📝 Journal cache invalidation (placeholder)');
      },
      enabled: false, // Disable until journal caching is implemented
      priority: SyncPriority.low,
    ),
  };
  
  /// Start real-time sync with aggressive polling
  void startRealTimeSync() {
    if (!_isEnabled) return;
    
    _stopAllTimers();
    
    if (_isAppInForeground) {
      _startForegroundSync();
    } else {
      _startBackgroundSync();
    }
    
    print('🚀 Real-time sync started (${_isAppInForeground ? 'foreground' : 'background'} mode)');
  }
  
  /// Stop all real-time sync timers
  void stopRealTimeSync() {
    _stopAllTimers();
    print('⏹️ Real-time sync stopped');
  }
  
  /// Enable or disable real-time sync
  void setEnabled(bool enabled) {
    _isEnabled = enabled;
    if (enabled) {
      startRealTimeSync();
    } else {
      stopRealTimeSync();
    }
    print('${enabled ? '✅' : '❌'} Real-time sync ${enabled ? 'enabled' : 'disabled'}');
  }
  
  /// Set app foreground/background state
  void setAppInForeground(bool inForeground) {
    if (_isAppInForeground != inForeground) {
      _isAppInForeground = inForeground;
      
      if (_isEnabled) {
        startRealTimeSync(); // Restart with appropriate interval
        
        if (inForeground) {
          // Immediate check when app comes to foreground
          Future.delayed(Duration(milliseconds: 500), () {
            if (_isAppInForeground && !_isSyncing) {
              checkAllDataForChanges(reason: 'App foreground');
            }
          });
        }
      }
    }
  }
  
  /// Start aggressive foreground sync (every 10 seconds)
  void _startForegroundSync() {
    _aggressiveTimer = Timer.periodic(_foregroundInterval, (_) {
      if (_isAppInForeground && !_isSyncing) {
        checkAllDataForChanges(reason: 'Foreground poll');
      }
    });
    
    print('🔥 Foreground sync: checking every ${_foregroundInterval.inSeconds} seconds');
  }
  
  /// Start moderate background sync (every 30 seconds)
  void _startBackgroundSync() {
    _backgroundTimer = Timer.periodic(_backgroundInterval, (_) {
      if (!_isAppInForeground && !_isSyncing) {
        checkAllDataForChanges(reason: 'Background poll');
      }
    });
    
    print('🔄 Background sync: checking every ${_backgroundInterval.inSeconds} seconds');
  }
  
  /// Stop all timers
  void _stopAllTimers() {
    _aggressiveTimer?.cancel();
    _backgroundTimer?.cancel();
    _aggressiveTimer = null;
    _backgroundTimer = null;
  }
  
  /// Check all enabled data types for server changes (same as universal sync but optimized)
  Future<void> checkAllDataForChanges({String reason = 'Manual'}) async {
    if (_isSyncing) {
      print('🔄 Sync already in progress, skipping...');
      return;
    }
    
    if (!_isUserLoggedIn()) {
      return; // Silently skip if not logged in
    }
    
    _isSyncing = true;
    final startTime = DateTime.now();
    
    // Get enabled configs sorted by priority (critical first)
    final enabledConfigs = _syncConfigs.entries
        .where((entry) => entry.value.enabled)
        .toList()
      ..sort((a, b) => b.value.priority.index.compareTo(a.value.priority.index));
    
    final dataTypes = enabledConfigs.map((e) => e.key).toList();
    AppEventBus.instance.fire(SyncStartedEvent(dataTypes));
    
    final results = <String, bool>{};
    int changesDetected = 0;
    
    // Check each data type (prioritize critical ones)
    for (final entry in enabledConfigs) {
      final dataType = entry.key;
      final config = entry.value;
      
      try {
        final hasChanged = await _checkDataType(dataType, config);
        results[dataType] = true;
        
        if (hasChanged) {
          changesDetected++;
          print('📡 [$reason] Server change detected: $dataType');
        }
      } catch (e) {
        results[dataType] = false;
        // Don't spam error logs for network issues in real-time mode
        if (_isAppInForeground) {
          print('⚠️ [$reason] Failed to check $dataType: $e');
        }
        AppEventBus.instance.fire(SyncErrorEvent(dataType, e.toString()));
      }
    }
    
    final duration = DateTime.now().difference(startTime);
    AppEventBus.instance.fire(SyncCompletedEvent(results, duration));
    
    _isSyncing = false;
    
    // Only log successful syncs with changes to reduce noise
    if (changesDetected > 0 || _isAppInForeground) {
      print('✅ [$reason] Sync completed in ${duration.inMilliseconds}ms - $changesDetected changes found');
    }
  }
  
  /// Check a specific data type for changes (optimized for real-time)
  Future<bool> _checkDataType(String dataType, RealTimeSyncConfig config) async {
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
        // Invalidate the cache
        await config.invalidateCache();
        
        // Notify the app about the change
        AppEventBus.instance.fire(DataChangedEvent(dataType, details: 'Real-time server update'));
        AppEventBus.instance.fire(CacheInvalidatedEvent(dataType, 'Real-time sync'));
        
        return true;
      }
      
      return false;
    } catch (e) {
      rethrow;
    }
  }
  
  /// Force an immediate check for a specific data type
  Future<bool> forceCheckDataType(String dataType) async {
    final config = _syncConfigs[dataType];
    if (config == null || !config.enabled) {
      return false;
    }
    
    if (!_isUserLoggedIn()) {
      return false;
    }
    
    try {
      print('🔍 Force checking $dataType...');
      final hasChanged = await _checkDataType(dataType, config);
      print(hasChanged 
          ? '✅ $dataType force check: changes found'
          : '✅ $dataType force check: no changes');
      
      return hasChanged;
    } catch (e) {
      print('⚠️ Force check failed for $dataType: $e');
      return false;
    }
  }
  
  /// Force immediate check of all data types
  Future<void> forceCheckAll() async {
    await checkAllDataForChanges(reason: 'Force check');
  }
  
  /// Enable or disable sync for a specific data type
  void setDataTypeEnabled(String dataType, bool enabled) {
    final config = _syncConfigs[dataType];
    if (config != null) {
      config.enabled = enabled;
      print('${enabled ? '✅' : '❌'} Real-time $dataType sync ${enabled ? 'enabled' : 'disabled'}');
    }
  }
  
  /// Get real-time sync status
  Map<String, dynamic> getRealTimeSyncStatus() {
    return {
      'is_enabled': _isEnabled,
      'is_syncing': _isSyncing,
      'is_foreground': _isAppInForeground,
      'is_logged_in': _isUserLoggedIn(),
      'foreground_interval_seconds': _foregroundInterval.inSeconds,
      'background_interval_seconds': _backgroundInterval.inSeconds,
      'enabled_types': _syncConfigs.entries
          .where((e) => e.value.enabled)
          .map((e) => e.key)
          .toList(),
      'disabled_types': _syncConfigs.entries
          .where((e) => !e.value.enabled)
          .map((e) => e.key)
          .toList(),
      'has_foreground_timer': _aggressiveTimer != null,
      'has_background_timer': _backgroundTimer != null,
    };
  }
  
  /// Reset all sync state
  Future<void> resetSyncState() async {
    print('🗑️ Resetting real-time sync state...');
    await _detector.clearStoredState();
    _isSyncing = false;
    print('✅ Real-time sync state reset');
  }
  
  /// Pause real-time sync temporarily (e.g., during heavy operations)
  void pauseSync() {
    _stopAllTimers();
    print('⏸️ Real-time sync paused');
  }
  
  /// Resume real-time sync after pause
  void resumeSync() {
    if (_isEnabled) {
      startRealTimeSync();
      print('▶️ Real-time sync resumed');
    }
  }
  
  /// Check if user is logged in
  bool _isUserLoggedIn() {
    final user = _authService.currentUser;
    final token = _authService.authToken;
    return user != null && token != null && token.isNotEmpty;
  }
  
  /// Get current sync interval based on app state
  Duration get currentInterval {
    if (!_isEnabled) return Duration.zero;
    return _isAppInForeground ? _foregroundInterval : _backgroundInterval;
  }
  
  /// Get time until next sync check
  Duration? get timeUntilNextSync {
    final timer = _isAppInForeground ? _aggressiveTimer : _backgroundTimer;
    if (timer == null) return null;
    
    // This is an approximation - Timer doesn't expose remaining time
    return currentInterval;
  }
  
  /// Dispose of the service
  void dispose() {
    stopRealTimeSync();
  }
}

/// Configuration for real-time syncing a specific data type
class RealTimeSyncConfig {
  final String endpoint;
  final Future<void> Function() invalidateCache;
  bool enabled;
  final SyncPriority priority;
  
  RealTimeSyncConfig({
    required this.endpoint,
    required this.invalidateCache,
    this.enabled = true,
    this.priority = SyncPriority.medium,
  });
}

/// Priority levels for real-time sync operations
enum SyncPriority {
  low,
  medium,
  high,
  critical,
}