import 'package:just_audio/just_audio.dart';
import '../models/meditation_models.dart';

class MeditationAudioService {
  static final MeditationAudioService _instance =
      MeditationAudioService._internal();
  factory MeditationAudioService() => _instance;
  MeditationAudioService._internal();

  AudioPlayer? _audioPlayer;
  bool _isInitialized = false;
  AudioType _currentAudioType = AudioType.none;
  double _currentVolume = 0.7;

  // Initialize the audio service
  Future<void> initialize() async {
    if (!_isInitialized) {
      _audioPlayer = AudioPlayer();
      _isInitialized = true;
    }
  }

  // Play audio for meditation session
  Future<bool> playAudio(AudioType audioType, {double volume = 0.7}) async {
    try {
      if (!_isInitialized) {
        await initialize();
      }

      // Re-initialize if player was disposed
      if (_audioPlayer == null) {
        _audioPlayer = AudioPlayer();
        _isInitialized = true;
      }

      // Stop any currently playing audio first
      try {
        if (_audioPlayer!.playing) {
          await _audioPlayer!.stop();
          await Future.delayed(Duration(milliseconds: 100));
        }
      } catch (e) {
        // Ignore errors when stopping
      }

      if (audioType == AudioType.none || audioType.audioPath.isEmpty) {
        await stopAudio();
        return true;
      }

      _currentAudioType = audioType;
      _currentVolume = volume;

      try {
        // Load the audio asset
        await _audioPlayer!.setAsset(audioType.audioPath);

        // Set volume and loop mode
        await _audioPlayer!.setVolume(volume);
        await _audioPlayer!.setLoopMode(LoopMode.one);

        // Small delay to ensure everything is ready
        await Future.delayed(Duration(milliseconds: 200));

        // Start playing
        await _audioPlayer!.play();

        // Verification delay
        await Future.delayed(Duration(milliseconds: 500));
        final isPlaying = _audioPlayer!.playing;
        final processingState = _audioPlayer!.processingState;

        // Check if playback started successfully
        if (isPlaying ||
            processingState == ProcessingState.ready ||
            processingState == ProcessingState.buffering) {
          return true;
        } else {
          // Try once more after a delay
          await Future.delayed(Duration(milliseconds: 500));
          return _audioPlayer!.playing;
        }
      } catch (assetError) {
        // Try to recover by re-initializing
        try {
          _audioPlayer?.dispose();
          _audioPlayer = AudioPlayer();
          await _audioPlayer!.setAsset(audioType.audioPath);
          await _audioPlayer!.setVolume(volume);
          await _audioPlayer!.setLoopMode(LoopMode.one);
          await _audioPlayer!.play();
          return true;
        } catch (recoveryError) {
          return false;
        }
      }
    } catch (e) {
      // Fallback - continue without audio rather than blocking meditation
      return false;
    }
  }

  // Update volume during session
  Future<void> setVolume(double volume) async {
    if (_audioPlayer != null && _isInitialized) {
      _currentVolume = volume;
      await _audioPlayer!.setVolume(volume);
    }
  }

  // Pause audio
  Future<void> pauseAudio() async {
    if (_audioPlayer != null && _isInitialized) {
      await _audioPlayer!.pause();
    }
  }

  // Resume audio
  Future<void> resumeAudio() async {
    if (_audioPlayer != null && _isInitialized) {
      await _audioPlayer!.play();
    }
  }

  // Stop audio
  Future<void> stopAudio() async {
    if (_audioPlayer != null && _isInitialized) {
      await _audioPlayer!.stop();
    }
  }

  // Preview audio (short play for selection)
  Future<void> previewAudio(
    AudioType audioType, {
    Duration duration = const Duration(seconds: 3),
  }) async {
    try {
      if (!_isInitialized) {
        await initialize();
      }

      if (audioType == AudioType.none || audioType.audioPath.isEmpty) {
        return;
      }

      // Create a temporary player for preview
      final previewPlayer = AudioPlayer();

      try {
        await previewPlayer.setVolume(0.5); // Lower volume for preview
        await previewPlayer.setAsset(audioType.audioPath);
        await previewPlayer.play();

        // Stop after duration
        await Future.delayed(duration);
        await previewPlayer.stop();
      } finally {
        await previewPlayer.dispose();
      }
    } catch (e) {
      // Silently fail preview
    }
  }

  // Get current playback state
  bool get isPlaying => _audioPlayer?.playing ?? false;
  AudioType get currentAudioType => _currentAudioType;
  double get currentVolume => _currentVolume;

  // Dispose resources (only call when completely done with audio)
  Future<void> dispose() async {
    if (_audioPlayer != null) {
      try {
        if (_audioPlayer!.playing) {
          await _audioPlayer!.stop();
        }
        await _audioPlayer!.dispose();
      } catch (e) {
        // Ignore disposal errors
      }
      _audioPlayer = null;
      _isInitialized = false;
      _currentAudioType = AudioType.none;
    }
  }

  // Stop and dispose when meditation session ends completely
  Future<void> endMeditationSession() async {
    if (_audioPlayer != null && _isInitialized) {
      await stopAudio();
    }
  }
}
