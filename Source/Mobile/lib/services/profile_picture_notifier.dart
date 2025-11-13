import 'package:flutter/foundation.dart';

/// A global notifier for profile picture updates
/// This allows all UserProfilePicture widgets to refresh when the profile picture changes
class ProfilePictureNotifier extends ChangeNotifier {
  static final ProfilePictureNotifier _instance = ProfilePictureNotifier._internal();
  factory ProfilePictureNotifier() => _instance;
  ProfilePictureNotifier._internal();

  String? _currentProfileImageUrl;
  int _updateCounter = 0;

  String? get currentProfileImageUrl => _currentProfileImageUrl;
  int get updateCounter => _updateCounter;

  /// Call this when the profile picture has been updated
  void notifyProfilePictureUpdated([String? newImageUrl]) {
    _currentProfileImageUrl = newImageUrl;
    _updateCounter++;
    notifyListeners();
  }

  /// Clear the cached image URL
  void clearCache() {
    _currentProfileImageUrl = null;
    _updateCounter++;
    notifyListeners();
  }
}