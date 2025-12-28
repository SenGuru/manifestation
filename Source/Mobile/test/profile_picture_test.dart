import 'package:flutter_test/flutter_test.dart';
import 'package:mistel/services/profile_picture_notifier.dart';

void main() {
  group('ProfilePictureNotifier', () {
    test('should be a singleton', () {
      final notifier1 = ProfilePictureNotifier();
      final notifier2 = ProfilePictureNotifier();
      expect(identical(notifier1, notifier2), isTrue);
    });

    test('should increment update counter when notified', () {
      final notifier = ProfilePictureNotifier();
      final initialCounter = notifier.updateCounter;
      
      notifier.notifyProfilePictureUpdated();
      
      expect(notifier.updateCounter, equals(initialCounter + 1));
    });

    test('should store new image URL when notified with URL', () {
      final notifier = ProfilePictureNotifier();
      const testUrl = 'https://example.com/profile.jpg';
      
      notifier.notifyProfilePictureUpdated(testUrl);
      
      expect(notifier.currentProfileImageUrl, equals(testUrl));
    });

    test('should notify listeners when profile picture updated', () {
      final notifier = ProfilePictureNotifier();
      bool listenerCalled = false;
      
      notifier.addListener(() {
        listenerCalled = true;
      });
      
      notifier.notifyProfilePictureUpdated();
      
      expect(listenerCalled, isTrue);
    });

    test('should clear cache and notify listeners', () {
      final notifier = ProfilePictureNotifier();
      const testUrl = 'https://example.com/profile.jpg';
      bool listenerCalled = false;
      
      notifier.notifyProfilePictureUpdated(testUrl);
      expect(notifier.currentProfileImageUrl, equals(testUrl));
      
      notifier.addListener(() {
        listenerCalled = true;
      });
      
      notifier.clearCache();
      
      expect(notifier.currentProfileImageUrl, isNull);
      expect(listenerCalled, isTrue);
    });
  });
}