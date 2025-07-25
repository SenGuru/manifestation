import 'dart:async';

/// Global event bus for communicating data changes across the app
class AppEventBus {
  static final AppEventBus _instance = AppEventBus._internal();
  static AppEventBus get instance => _instance;
  
  AppEventBus._internal();
  
  final StreamController<AppEvent> _eventController = StreamController<AppEvent>.broadcast();
  
  /// Listen to events of a specific type
  Stream<T> on<T extends AppEvent>() {
    return _eventController.stream.where((event) => event is T).cast<T>();
  }
  
  /// Fire an event
  void fire(AppEvent event) {
    _eventController.add(event);
  }
  
  /// Dispose of the event bus
  void dispose() {
    _eventController.close();
  }
}

/// Base class for all app events
abstract class AppEvent {
  final DateTime timestamp;
  
  AppEvent() : timestamp = DateTime.now();
}

/// Event fired when server data changes
class DataChangedEvent extends AppEvent {
  final String dataType;
  final String? details;
  
  DataChangedEvent(this.dataType, {this.details});
  
  @override
  String toString() => 'DataChangedEvent(dataType: $dataType, details: $details)';
}

/// Event fired when sync starts
class SyncStartedEvent extends AppEvent {
  final List<String> dataTypes;
  
  SyncStartedEvent(this.dataTypes);
  
  @override
  String toString() => 'SyncStartedEvent(dataTypes: $dataTypes)';
}

/// Event fired when sync completes
class SyncCompletedEvent extends AppEvent {
  final Map<String, bool> results; // dataType -> success
  final Duration duration;
  
  SyncCompletedEvent(this.results, this.duration);
  
  @override
  String toString() => 'SyncCompletedEvent(results: $results, duration: ${duration.inMilliseconds}ms)';
}

/// Event fired when sync fails
class SyncErrorEvent extends AppEvent {
  final String dataType;
  final String error;
  
  SyncErrorEvent(this.dataType, this.error);
  
  @override
  String toString() => 'SyncErrorEvent(dataType: $dataType, error: $error)';
}

/// Event fired when cache is invalidated
class CacheInvalidatedEvent extends AppEvent {
  final String dataType;
  final String reason;
  
  CacheInvalidatedEvent(this.dataType, this.reason);
  
  @override
  String toString() => 'CacheInvalidatedEvent(dataType: $dataType, reason: $reason)';
}