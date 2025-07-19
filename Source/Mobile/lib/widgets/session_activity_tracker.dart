import 'package:flutter/material.dart';
import '../services/session_manager.dart';

/// Widget that tracks user activity for session management
class SessionActivityTracker extends StatefulWidget {
  final Widget child;
  
  const SessionActivityTracker({
    Key? key,
    required this.child,
  }) : super(key: key);

  @override
  _SessionActivityTrackerState createState() => _SessionActivityTrackerState();
}

class _SessionActivityTrackerState extends State<SessionActivityTracker>
    with WidgetsBindingObserver {
  
  @override
  void initState() {
    super.initState();
    WidgetsBinding.instance.addObserver(this);
    // Record initial activity
    SessionManager.recordActivity();
  }

  @override
  void dispose() {
    WidgetsBinding.instance.removeObserver(this);
    super.dispose();
  }

  @override
  void didChangeAppLifecycleState(AppLifecycleState state) {
    super.didChangeAppLifecycleState(state);
    
    switch (state) {
      case AppLifecycleState.resumed:
        print('App resumed - recording activity');
        SessionManager.recordActivity();
        break;
      case AppLifecycleState.paused:
        print('App paused');
        break;
      case AppLifecycleState.inactive:
        print('App inactive');
        break;
      case AppLifecycleState.detached:
        print('App detached');
        SessionManager.dispose();
        break;
      case AppLifecycleState.hidden:
        print('App hidden');
        break;
    }
  }

  void _onUserInteraction() {
    // Record activity on any user interaction
    SessionManager.recordActivity();
  }

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: _onUserInteraction,
      onPanDown: (_) => _onUserInteraction(),
      onScaleStart: (_) => _onUserInteraction(),
      behavior: HitTestBehavior.translucent,
      child: NotificationListener<ScrollNotification>(
        onNotification: (notification) {
          _onUserInteraction();
          return false;
        },
        child: widget.child,
      ),
    );
  }
}