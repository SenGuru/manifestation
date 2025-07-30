import 'package:flutter/material.dart';

/// Widget that wraps content and dismisses keyboard when tapping outside text fields
class KeyboardDismissalWrapper extends StatelessWidget {
  final Widget child;

  const KeyboardDismissalWrapper({
    Key? key,
    required this.child,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: () {
        // Dismiss keyboard when tapping outside
        FocusScope.of(context).unfocus();
      },
      child: child,
    );
  }
}