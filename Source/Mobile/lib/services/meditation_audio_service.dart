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

      // Re-initialize if player was disposed
      if (_audioPlayer == null) {
        print('🔄 Re-initializing audio player');
        _audioPlayer = AudioPlayer();
        _isInitialized = true;
      }

      // Stop any currently playing audio first
      try {
        if (_audioPlayer!.playing) {
          await _audioPlayer!.stop();
          await Future.delayed(Duration(milliseconds: 100)); // Small delay after stop
        }
      } catch (e) {
        print('⚠️ Error stopping previous audio: $e');
      }

      if (audioType == AudioType.none || audioType.audioPath.isEmpty) {
        await stopAudio();
        print('🎵 Audio set to Silent - no audio will play');
        return true;
      }

      _currentAudioType = audioType;
      _currentVolume = volume;

      print('🎵 Attempting to play ${audioType.name} audio from ${audioType.audioPath}');
      
      try {
        // Load the audio asset first
        print('🎵 Loading audio asset: ${audioType.audioPath}');
        final duration = await _audioPlayer!.setAsset(audioType.audioPath);
        print('🎵 Asset loaded successfully, duration: ${duration?.inSeconds ?? 0}s');
        
        // Set volume after loading
        await _audioPlayer!.setVolume(volume);
        print('🎵 Volume set to ${(volume * 100).round()}%');
        
        // For meditation, we want to loop the audio
        await _audioPlayer!.setLoopMode(LoopMode.one);
        print('🎵 Loop mode set');
        
        // Small delay to ensure everything is ready
        await Future.delayed(Duration(milliseconds: 200));
        
        // Start playing
        print('🎵 Starting playback...');
        await _audioPlayer!.play();
        print('🎵 Play command sent');

        // Shorter verification delay
        await Future.delayed(Duration(milliseconds: 500));
        final isPlaying = _audioPlayer!.playing;
        final playerState = _audioPlayer!.playerState;
        final processingState = _audioPlayer!.processingState;
        
        print('🎵 Player state: $playerState');
        print('🎵 Processing state: $processingState');
        print('🎵 Is playing: $isPlaying');
        
        // More lenient success check - sometimes processing state might be buffering
        if (isPlaying || processingState == ProcessingState.ready || processingState == ProcessingState.buffering) {
          print('✅ Successfully playing ${audioType.name} audio at volume ${(volume * 100).round()}%');
          return true;
        } else {
          // Try once more after a delay
          await Future.delayed(Duration(milliseconds: 500));
          if (_audioPlayer!.playing) {
            print('✅ Audio started after retry - ${audioType.name}');
            return true;
          }
          print('❌ Audio failed to start playing - State: $playerState, Processing: $processingState');
          return false;
        }
      } catch (assetError) {
        print('❌ Error loading or playing audio asset: $assetError');
        print('❌ Error type: ${assetError.runtimeType}');
        // Try to recover by re-initializing
        try {
          _audioPlayer?.dispose();
          _audioPlayer = AudioPlayer();
          await _audioPlayer!.setAsset(audioType.audioPath);
          await _audioPlayer!.setVolume(volume);
          await _audioPlayer!.setLoopMode(LoopMode.one);
          await _audioPlayer!.play();
          print('✅ Recovered from error and started playing');
          return true;
        } catch (recoveryError) {
          print('❌ Recovery failed: $recoveryError');
          return false;
        }
      }
    } catch (e) {
      print('❌ Error playing audio: $e');
      print('❌ AudioType: ${audioType.name}, Path: ${audioType.audioPath}');
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

  // Dispose resources (only call when completely done with audio)
  Future<void> dispose() async {
    if (_audioPlayer != null) {
      try {
        if (_audioPlayer!.playing) {
          await _audioPlayer!.stop();
        }
        await _audioPlayer!.dispose();
      } catch (e) {
        print('❌ Error disposing audio player: $e');
      }
      _audioPlayer = null;
      _isInitialized = false;
      _currentAudioType = AudioType.none;
      print('🎵 Meditation audio service disposed');
    }
  }
  
  // Stop and dispose when meditation session ends completely
  Future<void> endMeditationSession() async {
    if (_audioPlayer != null && _isInitialized) {
      await stopAudio();
      print('🎵 Meditation session ended - audio stopped but service remains active');
    }
  }
}