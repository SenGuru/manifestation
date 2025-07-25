import 'package:flutter/material.dart';
import 'universal_sync_service.dart';
import 'real_time_sync_service.dart';
import 'app_event_bus.dart';

/// Manages app lifecycle events and coordinates with sync service
class AppLifecycleManager with WidgetsBindingObserver {
  static final AppLifecycleManager _instance = AppLifecycleManager._internal();
  static AppLifecycleManager get instance => _instance;
  
  AppLifecycleManager._internal();
  
  final UniversalSyncService _syncService = UniversalSyncService();
  final RealTimeSyncService _realTimeSyncService = RealTimeSyncService();
  bool _isInitialized = false;
  bool _useRealTimeSync = true; // Toggle between real-time and standard sync
  
  /// Initialize the lifecycle manager
  void initialize() {
    if (_isInitialized) return;
    
    WidgetsBinding.instance.addObserver(this);
    
    if (_useRealTimeSync) {
      _realTimeSyncService.startRealTimeSync();
      print('🚀 App lifecycle manager initialized with REAL-TIME sync (10-second polling)');
    } else {
      _syncService.startPeriodicSync();
      print('🔄 App lifecycle manager initialized with standard sync (3-minute polling)');
    }
    
    _isInitialized = true;
    
    // Check for changes on app start
    WidgetsBinding.instance.addPostFrameCallback((_) {
      if (_useRealTimeSync) {
        _realTimeSyncService.checkAllDataForChanges(reason: 'App start');
      } else {
        _syncService.checkAllDataForChanges();
      }
    });
  }
  
  /// Dispose of the lifecycle manager
  void dispose() {
    if (!_isInitialized) return;
    
    WidgetsBinding.instance.removeObserver(this);
    _syncService.stopPeriodicSync();
    _realTimeSyncService.stopRealTimeSync();
    _isInitialized = false;
    
    print('🛑 App lifecycle manager disposed');
  }
  
  @override
  void didChangeAppLifecycleState(AppLifecycleState state) {
    super.didChangeAppLifecycleState(state);
    
    switch (state) {
      case AppLifecycleState.resumed:
        // App came to foreground
        print('📱 App resumed - switching to foreground sync mode');
        if (_useRealTimeSync) {
          _realTimeSyncService.setAppInForeground(true);
        } else {
          _syncService.setAppActive(true);
        }
        break;
        
      case AppLifecycleState.paused:
        // App went to background
        print('📱 App paused - switching to background sync mode');
        if (_useRealTimeSync) {
          _realTimeSyncService.setAppInForeground(false);
        } else {
          _syncService.setAppActive(false);
        }
        break;
        
      case AppLifecycleState.detached:
        // App is being terminated
        print('📱 App detached - stopping sync');
        if (_useRealTimeSync) {
          _realTimeSyncService.setAppInForeground(false);
        } else {
          _syncService.setAppActive(false);
        }
        break;
        
      case AppLifecycleState.inactive:
        // App is inactive (e.g., during a phone call)
        print('📱 App inactive');
        break;
        
      case AppLifecycleState.hidden:
        // App is hidden (iOS/Android specific)
        print('📱 App hidden');
        break;
    }
  }
  
  /// Get the universal sync service instance
  UniversalSyncService get syncService => _syncService;
  
  /// Get the real-time sync service instance
  RealTimeSyncService get realTimeSyncService => _realTimeSyncService;
  
  /// Switch between real-time and standard sync modes
  void setRealTimeSyncEnabled(bool enabled) {
    if (_useRealTimeSync == enabled) return;
    
    _useRealTimeSync = enabled;
    
    if (_isInitialized) {
      // Stop current sync mode
      _syncService.stopPeriodicSync();
      _realTimeSyncService.stopRealTimeSync();
      
      // Start new sync mode
      if (enabled) {
        _realTimeSyncService.startRealTimeSync();
        print('🚀 Switched to REAL-TIME sync mode (10-second polling)');
      } else {
        _syncService.startPeriodicSync();
        print('🔄 Switched to STANDARD sync mode (3-minute polling)');
      }
    }
  }
  
  /// Check if real-time sync is enabled
  bool get isRealTimeSyncEnabled => _useRealTimeSync;
  
  /// Check for changes before loading a specific data type
  Future<void> checkBeforeScreenLoad(String dataType) async {
    if (_useRealTimeSync) {
      await _realTimeSyncService.forceCheckDataType(dataType);
    } else {
      await _syncService.checkBeforeScreenLoad(dataType);
    }
  }
  
  /// Force sync a specific data type
  Future<bool> forceSyncDataType(String dataType) async {
    if (_useRealTimeSync) {
      return await _realTimeSyncService.forceCheckDataType(dataType);
    } else {
      return await _syncService.forceSyncDataType(dataType);
    }
  }
  
  /// Force sync all data types immediately
  Future<void> forceSyncAll() async {
    if (_useRealTimeSync) {
      await _realTimeSyncService.forceCheckAll();
    } else {
      await _syncService.checkAllDataForChanges();
    }
  }
  
  /// Enable/disable sync for a data type
  void setDataTypeEnabled(String dataType, bool enabled) {
    if (_useRealTimeSync) {
      _realTimeSyncService.setDataTypeEnabled(dataType, enabled);
    } else {
      _syncService.setDataTypeEnabled(dataType, enabled);
    }
  }
  
  /// Get current sync status
  Map<String, dynamic> getSyncStatus() {
    if (_useRealTimeSync) {
      final status = _realTimeSyncService.getRealTimeSyncStatus();
      status['sync_mode'] = 'real-time';
      return status;
    } else {
      final status = _syncService.getSyncStatus();
      status['sync_mode'] = 'standard';
      return status;
    }
  }
  
  /// Reset all sync state (useful for logout)
  Future<void> resetSyncState() async {
    if (_useRealTimeSync) {
      await _realTimeSyncService.resetSyncState();
    } else {
      await _syncService.resetSyncState();
    }
  }
}