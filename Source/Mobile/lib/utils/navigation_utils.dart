import 'package:flutter/material.dart';

/// Custom route that disables swipe-to-go-back gestures while keeping back button functional
class NoSwipeRoute<T> extends MaterialPageRoute<T> {
  NoSwipeRoute({
    required WidgetBuilder builder,
    RouteSettings? settings,
  }) : super(builder: builder, settings: settings);

  @override
  bool canTransitionTo(TransitionRoute<dynamic> nextRoute) => false;

  @override
  bool canTransitionFrom(TransitionRoute<dynamic> previousRoute) => false;

  @override
  Widget buildTransitions(
    BuildContext context,
    Animation<double> animation,
    Animation<double> secondaryAnimation,
    Widget child,
  ) {
    // Use fade transition instead of slide transition to prevent swipe gestures
    return FadeTransition(
      opacity: animation,
      child: child,
    );
  }
}

/// Custom page route builder that completely disables swipe gestures
class NoSwipePageRoute<T> extends PageRouteBuilder<T> {
  final Widget child;
  final Duration duration;

  NoSwipePageRoute({
    required this.child,
    this.duration = const Duration(milliseconds: 300),
    RouteSettings? settings,
  }) : super(
          pageBuilder: (context, animation, secondaryAnimation) => child,
          transitionDuration: duration,
          settings: settings,
        );

  @override
  Widget buildTransitions(
    BuildContext context,
    Animation<double> animation,
    Animation<double> secondaryAnimation,
    Widget child,
  ) {
    // Simple fade transition - no sliding = no swipe gestures
    return FadeTransition(
      opacity: CurvedAnimation(
        parent: animation,
        curve: Curves.easeInOut,
      ),
      child: child,
    );
  }
}

/// Extension to make it easy to use
extension NavigatorExtension on NavigatorState {
  /// Push a route without swipe-back gestures but with working back button
  Future<T?> pushNoSwipe<T extends Object?>(Widget screen) {
    return push<T>(NoSwipePageRoute<T>(child: screen));
  }
}

/// Helper function for easy usage
Future<T?> pushWithoutSwipe<T extends Object?>(
  BuildContext context,
  Widget screen,
) {
  return Navigator.of(context).push<T>(
    NoSwipePageRoute<T>(child: screen),
  );
}