import 'package:flutter/material.dart';
import '../services/app_lifecycle_manager.dart';
import '../services/app_event_bus.dart';
import 'dart:async';

/// Wrapper widget that adds pull-to-refresh functionality for real-time sync
/// Use this around screens that should refresh when data changes
class SyncRefreshWrapper extends StatefulWidget {
  final Widget child;
  final List<String> dataTypes;
  final bool showRefreshIndicator;
  final String? screenName;
  
  const SyncRefreshWrapper({
    Key? key,
    required this.child,
    this.dataTypes = const [],
    this.showRefreshIndicator = true,
    this.screenName,
  }) : super(key: key);
  
  @override
  _SyncRefreshWrapperState createState() => _SyncRefreshWrapperState();
}

class _SyncRefreshWrapperState extends State<SyncRefreshWrapper> {
  StreamSubscription? _eventSubscription;
  bool _isRefreshing = false;
  
  @override
  void initState() {
    super.initState();
    
    // Check for changes when screen loads
    WidgetsBinding.instance.addPostFrameCallback((_) {
      _checkForChangesOnLoad();
    });
    
    // Listen for data change events
    _eventSubscription = AppEventBus.instance.on<DataChangedEvent>().listen((event) {
      // Only refresh if this screen cares about the changed data type
      if (widget.dataTypes.isEmpty || widget.dataTypes.contains(event.dataType)) {
        if (mounted) {
          setState(() {
            // Trigger UI refresh
          });
        }
      }
    });
  }
  
  @override
  void dispose() {
    _eventSubscription?.cancel();
    super.dispose();
  }
  
  /// Check for changes when screen loads
  void _checkForChangesOnLoad() async {
    if (widget.dataTypes.isNotEmpty) {
      for (final dataType in widget.dataTypes) {
        await AppLifecycleManager.instance.checkBeforeScreenLoad(dataType);
      }
    } else {
      // Check all data types if none specified
      await AppLifecycleManager.instance.forceSyncAll();
    }
  }
  
  /// Handle pull-to-refresh
  Future<void> _handleRefresh() async {
    if (_isRefreshing) return;
    
    setState(() {
      _isRefreshing = true;
    });
    
    try {
      if (widget.dataTypes.isNotEmpty) {
        // Check specific data types
        for (final dataType in widget.dataTypes) {
          await AppLifecycleManager.instance.forceSyncDataType(dataType);
        }
      } else {
        // Check all data types
        await AppLifecycleManager.instance.forceSyncAll();
      }
      
      // Add a small delay to show the refresh indicator
      await Future.delayed(Duration(milliseconds: 500));
      
    } catch (e) {
      print('Refresh failed: $e');
    } finally {
      if (mounted) {
        setState(() {
          _isRefreshing = false;
        });
      }
    }
  }
  
  @override
  Widget build(BuildContext context) {
    if (!widget.showRefreshIndicator) {
      return widget.child;
    }
    
    return RefreshIndicator(
      onRefresh: _handleRefresh,
      displacement: 60,
      backgroundColor: Theme.of(context).cardColor,
      color: Theme.of(context).primaryColor,
      child: widget.child,
    );
  }
}

/// Extension to easily add sync refresh to any widget
extension SyncRefreshExtension on Widget {
  /// Wrap this widget with sync refresh functionality
  Widget withSyncRefresh({
    List<String> dataTypes = const [],
    bool showRefreshIndicator = true,
    String? screenName,
  }) {
    return SyncRefreshWrapper(
      dataTypes: dataTypes,
      showRefreshIndicator: showRefreshIndicator,
      screenName: screenName,
      child: this,
    );
  }
}

/// Pre-configured refresh wrappers for common screens
class AffirmationsRefreshWrapper extends StatelessWidget {
  final Widget child;
  
  const AffirmationsRefreshWrapper({Key? key, required this.child}) : super(key: key);
  
  @override
  Widget build(BuildContext context) {
    return SyncRefreshWrapper(
      dataTypes: ['affirmations', 'categories'],
      screenName: 'Affirmations',
      child: child,
    );
  }
}

class GoalsRefreshWrapper extends StatelessWidget {
  final Widget child;
  
  const GoalsRefreshWrapper({Key? key, required this.child}) : super(key: key);
  
  @override
  Widget build(BuildContext context) {
    return SyncRefreshWrapper(
      dataTypes: ['goals', 'categories'],
      screenName: 'Goals',
      child: child,
    );
  }
}

class VisionBoardRefreshWrapper extends StatelessWidget {
  final Widget child;
  
  const VisionBoardRefreshWrapper({Key? key, required this.child}) : super(key: key);
  
  @override
  Widget build(BuildContext context) {
    return SyncRefreshWrapper(
      dataTypes: ['vision_boards'],
      screenName: 'Vision Board',
      child: child,
    );
  }
}

class JournalRefreshWrapper extends StatelessWidget {
  final Widget child;
  
  const JournalRefreshWrapper({Key? key, required this.child}) : super(key: key);
  
  @override
  Widget build(BuildContext context) {
    return SyncRefreshWrapper(
      dataTypes: ['journal'],
      screenName: 'Journal',
      child: child,
    );
  }
}