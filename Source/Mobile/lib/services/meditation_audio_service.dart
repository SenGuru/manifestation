import 'package:just_audio/just_audio.dart';
import '../models/meditation_models.dart';

class MeditationAudioService {
  static final MeditationAudioService _instance = MeditationAudioService._internal();
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
      print('🎵 Meditation audio service initialized');
    }
  }

  // Play audio for meditation session
  Future<bool> playAudio(AudioType audioType, {double volume = 0.7}) async {
    try {
      if (!_isInitialized) {
        await initialize();
      }

      if (audioType == AudioType.none || audioType.audioPath.isEmpty) {
        await stopAudio();
        return true;
      }

      _currentAudioType = audioType;
      _currentVolume = volume;

      // Set volume
      await _audioPlayer!.setVolume(volume);
      
      // For meditation, we want to loop the audio
      await _audioPlayer!.setLoopMode(LoopMode.one);

      // Load and play the audio
      await _audioPlayer!.setAsset(audioType.audioPath);
      await _audioPlayer!.play();

      print('🎵 Playing ${audioType.name} audio at volume ${(volume * 100).round()}%');
      return true;
    } catch (e) {
      print('❌ Error playing audio: $e');
      // Fallback - continue without audio rather than blocking meditation
      return false;
    }
  }

  // Update volume during session
  Future<void> setVolume(double volume) async {
    if (_audioPlayer != null && _isInitialized) {
      _currentVolume = volume;
      await _audioPlayer!.setVolume(volume);
      print('🎵 Audio volume set to ${(volume * 100).round()}%');
    }
  }

  // Pause audio
  Future<void> pauseAudio() async {
    if (_audioPlayer != null && _isInitialized) {
      await _audioPlayer!.pause();
      print('⏸️ Audio paused');
    }
  }

  // Resume audio
  Future<void> resumeAudio() async {
    if (_audioPlayer != null && _isInitialized) {
      await _audioPlayer!.play();
      print('▶️ Audio resumed');
    }
  }

  // Stop audio
  Future<void> stopAudio() async {
    if (_audioPlayer != null && _isInitialized) {
      await _audioPlayer!.stop();
      print('⏹️ Audio stopped');
    }
  }

  // Preview audio (short play for selection)
  Future<void> previewAudio(AudioType audioType, {Duration duration = const Duration(seconds: 3)}) async {
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
        
        print('🎵 Previewing ${audioType.name} audio');
        
        // Stop after duration
        await Future.delayed(duration);
        await previewPlayer.stop();
      } finally {
        await previewPlayer.dispose();
      }
    } catch (e) {
      print('❌ Error previewing audio: $e');
    }
  }

  // Get current playback state
  bool get isPlaying => _audioPlayer?.playing ?? false;
  AudioType get currentAudioType => _currentAudioType;
  double get currentVolume => _currentVolume;

  // Dispose resources
  Future<void> dispose() async {
    if (_audioPlayer != null) {
      await _audioPlayer!.dispose();
      _audioPlayer = null;
      _isInitialized = false;
      print('🎵 Meditation audio service disposed');
    }
  }
}