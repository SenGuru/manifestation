import 'package:flutter/material.dart';
import 'dart:async';
import '../services/app_lifecycle_manager.dart';
import '../services/app_event_bus.dart';
import '../widgets/sync_status_widget.dart';

/// Demo screen to test and demonstrate the Universal Sync Service
/// This screen shows sync status, events, and allows manual testing
class SyncDemoScreen extends StatefulWidget {
  @override
  _SyncDemoScreenState createState() => _SyncDemoScreenState();
}

class _SyncDemoScreenState extends State<SyncDemoScreen> {
  final List<String> _logs = [];
  final ScrollController _scrollController = ScrollController();
  StreamSubscription? _eventSubscription;
  Map<String, dynamic> _syncStatus = {};
  
  @override
  void initState() {
    super.initState();
    _updateSyncStatus();
    
    // Listen to all sync events
    _eventSubscription = AppEventBus.instance.on<AppEvent>().listen((event) {
      setState(() {
        final timestamp = DateTime.now().toString().substring(11, 19);
        _logs.insert(0, '[$timestamp] ${event.toString()}');
        
        // Keep only last 50 logs
        if (_logs.length > 50) {
          _logs.removeLast();
        }
      });
      
      // Auto-scroll to top
      if (_scrollController.hasClients) {
        _scrollController.animateTo(
          0,
          duration: Duration(milliseconds: 300),
          curve: Curves.easeOut,
        );
      }
    });
    
    // Update sync status every 2 seconds
    Timer.periodic(Duration(seconds: 2), (_) {
      if (mounted) {
        _updateSyncStatus();
      }
    });
  }
  
  @override
  void dispose() {
    _eventSubscription?.cancel();
    _scrollController.dispose();
    super.dispose();
  }
  
  void _updateSyncStatus() {
    setState(() {
      _syncStatus = AppLifecycleManager.instance.getSyncStatus();
    });
  }
  
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Universal Sync Demo'),
        actions: [
          SyncStatusWidget(showDetails: true),
          SizedBox(width: 16),
        ],
      ),
      body: SingleChildScrollView(
        padding: EdgeInsets.all(16),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            // Quick Actions
            _buildQuickActions(),
            SizedBox(height: 24),
            
            // Sync Status
            _buildSyncStatus(),
            SizedBox(height: 24),
            
            // Data Type Controls
            _buildDataTypeControls(),
            SizedBox(height: 24),
            
            // Event Log
            _buildEventLog(),
          ],
        ),
      ),
    );
  }
  
  Widget _buildQuickActions() {
    final isRealTime = _syncStatus['sync_mode'] == 'real-time';
    
    return Card(
      child: Padding(
        padding: EdgeInsets.all(16),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              'Quick Actions',
              style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
            ),
            SizedBox(height: 12),
            
            // Sync Mode Toggle
            Row(
              children: [
                Text('Sync Mode:', style: TextStyle(fontWeight: FontWeight.w500)),
                SizedBox(width: 12),
                SegmentedButton<bool>(
                  segments: [
                    ButtonSegment(
                      value: false,
                      label: Text('Standard (3min)'),
                      icon: Icon(Icons.sync),
                    ),
                    ButtonSegment(
                      value: true,
                      label: Text('Real-Time (10s)'),
                      icon: Icon(Icons.flash_on),
                    ),
                  ],
                  selected: {isRealTime},
                  onSelectionChanged: (Set<bool> newSelection) {
                    AppLifecycleManager.instance.setRealTimeSyncEnabled(newSelection.first);
                    _updateSyncStatus();
                  },
                ),
              ],
            ),
            
            SizedBox(height: 12),
            Wrap(
              spacing: 8,
              runSpacing: 8,
              children: [
                ElevatedButton.icon(
                  onPressed: () {
                    AppLifecycleManager.instance.forceSyncAll();
                  },
                  icon: Icon(isRealTime ? Icons.flash_on : Icons.refresh),
                  label: Text('Force Sync'),
                ),
                ElevatedButton.icon(
                  onPressed: () {
                    AppLifecycleManager.instance.resetSyncState();
                  },
                  icon: Icon(Icons.clear_all),
                  label: Text('Reset State'),
                ),
                ElevatedButton.icon(
                  onPressed: () {
                    setState(() {
                      _logs.clear();
                    });
                  },
                  icon: Icon(Icons.delete),
                  label: Text('Clear Logs'),
                ),
                if (isRealTime) ...[
                  ElevatedButton.icon(
                    onPressed: () {
                      AppLifecycleManager.instance.realTimeSyncService.pauseSync();
                      _updateSyncStatus();
                    },
                    icon: Icon(Icons.pause),
                    label: Text('Pause'),
                    style: ElevatedButton.styleFrom(backgroundColor: Colors.orange),
                  ),
                  ElevatedButton.icon(
                    onPressed: () {
                      AppLifecycleManager.instance.realTimeSyncService.resumeSync();
                      _updateSyncStatus();
                    },
                    icon: Icon(Icons.play_arrow),
                    label: Text('Resume'),
                    style: ElevatedButton.styleFrom(backgroundColor: Colors.green),
                  ),
                ],
              ],
            ),
          ],
        ),
      ),
    );
  }
  
  Widget _buildSyncStatus() {
    final isSyncing = _syncStatus['is_syncing'] ?? false;
    final isActive = _syncStatus['is_active'] ?? false;
    final isLoggedIn = _syncStatus['is_logged_in'] ?? false;
    final enabledTypes = _syncStatus['enabled_types'] as List? ?? [];
    final disabledTypes = _syncStatus['disabled_types'] as List? ?? [];
    
    return Card(
      child: Padding(
        padding: EdgeInsets.all(16),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              'Sync Status',
              style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
            ),
            SizedBox(height: 12),
            _buildStatusRow('Syncing', isSyncing),
            _buildStatusRow('App Active', isActive),
            _buildStatusRow('Logged In', isLoggedIn),
            SizedBox(height: 8),
            Text('Enabled: ${enabledTypes.join(', ')}', style: TextStyle(fontSize: 12)),
            Text('Disabled: ${disabledTypes.join(', ')}', style: TextStyle(fontSize: 12)),
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
  
  Widget _buildDataTypeControls() {
    final enabledTypes = _syncStatus['enabled_types'] as List? ?? [];
    final disabledTypes = _syncStatus['disabled_types'] as List? ?? [];
    final allTypes = [...enabledTypes, ...disabledTypes];
    
    return Card(
      child: Padding(
        padding: EdgeInsets.all(16),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              'Data Type Controls',
              style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
            ),
            SizedBox(height: 12),
            ...allTypes.map((dataType) => _buildDataTypeRow(
              dataType,
              enabledTypes.contains(dataType),
            )),
          ],
        ),
      ),
    );
  }
  
  Widget _buildDataTypeRow(String dataType, bool enabled) {
    return Padding(
      padding: EdgeInsets.symmetric(vertical: 4),
      child: Row(
        children: [
          Expanded(
            child: Text(dataType),
          ),
          Switch(
            value: enabled,
            onChanged: (value) {
              AppLifecycleManager.instance.setDataTypeEnabled(dataType, value);
              _updateSyncStatus();
            },
          ),
          SizedBox(width: 8),
          IconButton(
            icon: Icon(Icons.sync, size: 20),
            onPressed: () {
              AppLifecycleManager.instance.forceSyncDataType(dataType);
            },
          ),
        ],
      ),
    );
  }
  
  Widget _buildEventLog() {
    return Card(
      child: Padding(
        padding: EdgeInsets.all(16),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: [
                Text(
                  'Event Log',
                  style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
                ),
                Spacer(),
                Text(
                  '${_logs.length} events',
                  style: TextStyle(fontSize: 12, color: Colors.grey),
                ),
              ],
            ),
            SizedBox(height: 12),
            Container(
              height: 300,
              decoration: BoxDecoration(
                border: Border.all(color: Colors.grey.withOpacity(0.3)),
                borderRadius: BorderRadius.circular(8),
              ),
              child: _logs.isEmpty
                  ? Center(
                      child: Text(
                        'No events yet. Try triggering a sync!',
                        style: TextStyle(color: Colors.grey),
                      ),
                    )
                  : ListView.builder(
                      controller: _scrollController,
                      itemCount: _logs.length,
                      itemBuilder: (context, index) {
                        return Padding(
                          padding: EdgeInsets.symmetric(horizontal: 8, vertical: 2),
                          child: Text(
                            _logs[index],
                            style: TextStyle(
                              fontSize: 11,
                              fontFamily: 'monospace',
                              color: _getLogColor(_logs[index]),
                            ),
                          ),
                        );
                      },
                    ),
            ),
          ],
        ),
      ),
    );
  }
  
  Color _getLogColor(String log) {
    if (log.contains('Error') || log.contains('❌')) return Colors.red;
    if (log.contains('Success') || log.contains('✅')) return Colors.green;
    if (log.contains('Warning') || log.contains('⚠️')) return Colors.orange;
    if (log.contains('DataChanged')) return Colors.blue;
    return Colors.black87;
  }
}