import 'package:flutter/material.dart';
import 'dart:async';
import '../services/app_lifecycle_manager.dart';
import '../services/app_event_bus.dart';

/// Widget that shows sync status and allows debugging sync operations
/// Useful for development and troubleshooting
class SyncStatusWidget extends StatefulWidget {
  final bool showDetails;
  final bool compact;
  
  const SyncStatusWidget({
    Key? key,
    this.showDetails = false,
    this.compact = true,
  }) : super(key: key);
  
  @override
  _SyncStatusWidgetState createState() => _SyncStatusWidgetState();
}

class _SyncStatusWidgetState extends State<SyncStatusWidget> {
  Map<String, dynamic> _syncStatus = {};
  List<AppEvent> _recentEvents = [];
  StreamSubscription? _eventSubscription;
  Timer? _statusTimer;
  
  @override
  void initState() {
    super.initState();
    _updateStatus();
    
    // Listen to sync events
    _eventSubscription = AppEventBus.instance.on<AppEvent>().listen((event) {
      setState(() {
        _recentEvents.insert(0, event);
        // Keep only last 10 events
        if (_recentEvents.length > 10) {
          _recentEvents = _recentEvents.take(10).toList();
        }
      });
    });
    
    // Update status every 5 seconds
    _statusTimer = Timer.periodic(Duration(seconds: 5), (_) {
      _updateStatus();
    });
  }
  
  @override
  void dispose() {
    _eventSubscription?.cancel();
    _statusTimer?.cancel();
    super.dispose();
  }
  
  void _updateStatus() {
    setState(() {
      _syncStatus = AppLifecycleManager.instance.getSyncStatus();
    });
  }
  
  @override
  Widget build(BuildContext context) {
    if (widget.compact) {
      return _buildCompactWidget();
    } else {
      return _buildDetailedWidget();
    }
  }
  
  Widget _buildCompactWidget() {
    final isSyncing = _syncStatus['is_syncing'] ?? false;
    final isActive = _syncStatus['is_active'] ?? _syncStatus['is_foreground'] ?? false;
    final isLoggedIn = _syncStatus['is_logged_in'] ?? false;
    final syncMode = _syncStatus['sync_mode'] ?? 'unknown';
    final isRealTime = syncMode == 'real-time';
    
    return Container(
      padding: EdgeInsets.symmetric(horizontal: 8, vertical: 4),
      decoration: BoxDecoration(
        color: _getSyncStatusColor().withOpacity(0.1),
        borderRadius: BorderRadius.circular(12),
        border: Border.all(color: _getSyncStatusColor(), width: 1),
      ),
      child: Row(
        mainAxisSize: MainAxisSize.min,
        children: [
          Icon(
            _getSyncStatusIcon(),
            size: 14,
            color: _getSyncStatusColor(),
          ),
          SizedBox(width: 4),
          Text(
            _getSyncStatusText(),
            style: TextStyle(
              fontSize: 11,
              color: _getSyncStatusColor(),
              fontWeight: FontWeight.w500,
            ),
          ),
          if (isRealTime) ...[
            SizedBox(width: 4),
            Container(
              padding: EdgeInsets.symmetric(horizontal: 4, vertical: 1),
              decoration: BoxDecoration(
                color: Colors.red.withOpacity(0.2),
                borderRadius: BorderRadius.circular(6),
              ),
              child: Text(
                'RT',
                style: TextStyle(
                  fontSize: 8,
                  color: Colors.red[700],
                  fontWeight: FontWeight.bold,
                ),
              ),
            ),
          ],
          if (widget.showDetails) ...[
            SizedBox(width: 8),
            GestureDetector(
              onTap: () => _showDetailDialog(),
              child: Icon(
                Icons.info_outline,
                size: 14,
                color: Colors.grey[600],
              ),
            ),
          ],
        ],
      ),
    );
  }
  
  Widget _buildDetailedWidget() {
    final syncMode = _syncStatus['sync_mode'] ?? 'unknown';
    final isRealTime = syncMode == 'real-time';
    final foregroundInterval = _syncStatus['foreground_interval_seconds'];
    final backgroundInterval = _syncStatus['background_interval_seconds'];
    
    return Card(
      child: Padding(
        padding: EdgeInsets.all(16),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: [
                Icon(isRealTime ? Icons.flash_on : Icons.sync, size: 20),
                SizedBox(width: 8),
                Text(
                  isRealTime ? 'Real-Time Sync Status' : 'Sync Status',
                  style: TextStyle(
                    fontSize: 16,
                    fontWeight: FontWeight.bold,
                  ),
                ),
                Spacer(),
                IconButton(
                  icon: Icon(Icons.refresh),
                  onPressed: () {
                    AppLifecycleManager.instance.forceSyncAll();
                  },
                ),
              ],
            ),
            SizedBox(height: 12),
            _buildStatusRow('Syncing', _syncStatus['is_syncing'] ?? false),
            _buildStatusRow(isRealTime ? 'Foreground' : 'App Active', 
                          _syncStatus['is_active'] ?? _syncStatus['is_foreground'] ?? false),
            _buildStatusRow('Logged In', _syncStatus['is_logged_in'] ?? false),
            if (isRealTime) ...[
              _buildStatusRow('Real-Time Mode', true),
            ],
            SizedBox(height: 8),
            if (isRealTime && foregroundInterval != null && backgroundInterval != null) ...[
              Text('Sync Intervals:', style: TextStyle(fontWeight: FontWeight.w500)),
              Text('  Foreground: ${foregroundInterval}s', style: TextStyle(fontSize: 12)),
              Text('  Background: ${backgroundInterval}s', style: TextStyle(fontSize: 12)),
              SizedBox(height: 8),
            ],
            Text('Enabled Types:', style: TextStyle(fontWeight: FontWeight.w500)),
            ..._buildDataTypesList(_syncStatus['enabled_types'] ?? []),
            if (_recentEvents.isNotEmpty) ...[
              SizedBox(height: 12),
              Text('Recent Events:', style: TextStyle(fontWeight: FontWeight.w500)),
              ..._recentEvents.take(5).map((event) => _buildEventRow(event)),
            ],
          ],
        ),
      ),
    );
  }
  
  Widget _buildStatusRow(String label, bool value) {
    return Padding(
      padding: EdgeInsets.symmetric(vertical: 2),
      child: Row(
        children: [
          Icon(
            value ? Icons.check_circle : Icons.cancel,
            size: 16,
            color: value ? Colors.green : Colors.red,
          ),
          SizedBox(width: 8),
          Text(label),
        ],
      ),
    );
  }
  
  List<Widget> _buildDataTypesList(List<dynamic> types) {
    return types.map((type) => Padding(
      padding: EdgeInsets.only(left: 16, top: 2),
      child: Text('• $type', style: TextStyle(fontSize: 12)),
    )).toList();
  }
  
  Widget _buildEventRow(AppEvent event) {
    return Padding(
      padding: EdgeInsets.only(left: 16, top: 2),
      child: Text(
        '${event.timestamp.toString().substring(11, 19)} - ${event.toString()}',
        style: TextStyle(fontSize: 10, color: Colors.grey[600]),
        maxLines: 1,
        overflow: TextOverflow.ellipsis,
      ),
    );
  }
  
  Color _getSyncStatusColor() {
    final isSyncing = _syncStatus['is_syncing'] ?? false;
    final isActive = _syncStatus['is_active'] ?? _syncStatus['is_foreground'] ?? false;
    final isLoggedIn = _syncStatus['is_logged_in'] ?? false;
    final syncMode = _syncStatus['sync_mode'] ?? 'unknown';
    final isRealTime = syncMode == 'real-time';
    
    if (!isLoggedIn) return Colors.grey;
    if (isSyncing) return isRealTime ? Colors.red : Colors.blue;
    if (isActive) return isRealTime ? Colors.red[600]! : Colors.green;
    return Colors.orange;
  }
  
  IconData _getSyncStatusIcon() {
    final isSyncing = _syncStatus['is_syncing'] ?? false;
    final isActive = _syncStatus['is_active'] ?? _syncStatus['is_foreground'] ?? false;
    final isLoggedIn = _syncStatus['is_logged_in'] ?? false;
    final syncMode = _syncStatus['sync_mode'] ?? 'unknown';
    final isRealTime = syncMode == 'real-time';
    
    if (!isLoggedIn) return Icons.person_off;
    if (isSyncing) return isRealTime ? Icons.flash_on : Icons.sync;
    if (isActive) return isRealTime ? Icons.flash_on : Icons.cloud_done;
    return Icons.cloud_off;
  }
  
  String _getSyncStatusText() {
    final isSyncing = _syncStatus['is_syncing'] ?? false;
    final isActive = _syncStatus['is_active'] ?? _syncStatus['is_foreground'] ?? false;
    final isLoggedIn = _syncStatus['is_logged_in'] ?? false;
    final syncMode = _syncStatus['sync_mode'] ?? 'unknown';
    final isRealTime = syncMode == 'real-time';
    
    if (!isLoggedIn) return 'Not logged in';
    if (isSyncing) return isRealTime ? 'Real-time sync...' : 'Syncing...';
    if (isActive) return isRealTime ? 'Live (10s)' : 'Active';
    return 'Inactive';
  }
  
  void _showDetailDialog() {
    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        title: Text('Sync Details'),
        content: SingleChildScrollView(
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            mainAxisSize: MainAxisSize.min,
            children: [
              Text('Status: ${_getSyncStatusText()}'),
              SizedBox(height: 8),
              Text('Enabled Types:'),
              ...(_syncStatus['enabled_types'] as List? ?? []).map(
                (type) => Text('  • $type'),
              ),
              SizedBox(height: 8),
              Text('Recent Events:'),
              ..._recentEvents.take(3).map(
                (event) => Text(
                  '${event.timestamp.toString().substring(11, 19)} - ${event.runtimeType}',
                  style: TextStyle(fontSize: 12),
                ),
              ),
            ],
          ),
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context),
            child: Text('Close'),
          ),
          TextButton(
            onPressed: () {
              AppLifecycleManager.instance.syncService.checkAllDataForChanges();
              Navigator.pop(context);
            },
            child: Text('Force Sync'),
          ),
        ],
      ),
    );
  }
}