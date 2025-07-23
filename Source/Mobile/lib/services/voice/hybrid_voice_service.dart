import 'dart:async';
import 'package:audioplayers/audioplayers.dart';
import 'package:flutter_tts/flutter_tts.dart';
import 'premium_tts_service.dart';

class HybridVoiceService {
  static final HybridVoiceService _instance = HybridVoiceService._internal();
  factory HybridVoiceService() => _instance;
  HybridVoiceService._internal();

  // Audio player for human recordings
  AudioPlayer? _audioPlayer;
  
  // Premium TTS for natural voices (primary)
  PremiumTTSService? _premiumTts;
  
  // FlutterTTS as final fallback
  FlutterTts? _flutterTts;
  
  bool _isInitialized = false;
  bool _isPlaying = false;
  String _currentText = '';
  Timer? _completionTimer;
  
  // Voice settings
  String _selectedVoice = 'sarah_calm'; // Default to human recording
  double _volume = 0.9;
  double _playbackRate = 1.0;
  
  // TTS fallback settings (more human-like)
  double _ttsRate = 0.45;
  double _ttsPitch = 0.85;

  bool get isPlaying => _isPlaying;
  String get currentText => _currentText;
  String get selectedVoice => _selectedVoice;
  double get volume => _volume;
  double get speechRate => _ttsRate;
  double get pitch => _ttsPitch;
  String get language => 'en-US';
  bool get isPluginAvailable => true;

  List<String> get availableVoices {
    List<String> voices = [
      'sarah_calm',
      'emma_energetic', 
      'lisa_soothing',
      'david_confident',
      'michael_warm',
      'james_inspiring',
    ];
    
    // Add premium TTS voices if available
    if (_premiumTts != null && _premiumTts!.isInitialized) {
      voices.addAll(_premiumTts!.availableVoices);
    }
    
    return voices;
  }

  Future<void> initialize() async {
    if (_isInitialized) return;

    try {
      // Initialize audio player for human recordings
      _audioPlayer = AudioPlayer();
      
      _audioPlayer!.onPlayerStateChanged.listen((PlayerState state) {
        if (state == PlayerState.completed) {
          _isPlaying = false;
          _currentText = '';
          print('Human recording completed');
        } else if (state == PlayerState.playing) {
          _isPlaying = true;
        } else if (state == PlayerState.stopped || state == PlayerState.paused) {
          _isPlaying = false;
        }
      });

      await _audioPlayer!.setVolume(_volume);
      await _audioPlayer!.setPlaybackRate(_playbackRate);
      
      // Initialize Premium TTS for natural voices
      _premiumTts = PremiumTTSService();
      await _premiumTts!.initialize();
      print('Premium TTS initialized with best device voices');
      
      // Initialize FlutterTTS as fallback
      _flutterTts = FlutterTts();
      await _flutterTts!.setLanguage('en-US');
      await _flutterTts!.setSpeechRate(_ttsRate);
      await _flutterTts!.setVolume(_volume);
      await _flutterTts!.setPitch(_ttsPitch);
      
      _flutterTts!.setCompletionHandler(() {
        _isPlaying = false;
        _currentText = '';
        print('TTS fallback completed');
      });
      
      _flutterTts!.setStartHandler(() {
        _isPlaying = true;
      });
      
      // Set default to best available voice
      if (_premiumTts != null && _premiumTts!.availableVoices.isNotEmpty) {
        _selectedVoice = _premiumTts!.availableVoices.first; // Best premium voice
      }
      
      _isInitialized = true;
      print('Hybrid Voice Service initialized (Human recordings + Premium TTS + FlutterTTS fallback)');
      
    } catch (e) {
      print('Hybrid Voice Service initialization failed: $e');
      _isInitialized = false;
    }
  }

  Future<bool> speak(String text) async {
    if (!_isInitialized) {
      await initialize();
    }

    try {
      await stop();
      _currentText = text;
      
      // First, try to find a human recording
      String audioFile = await _findHumanRecording(text);
      
      if (audioFile.isNotEmpty) {
        print('🎙️ Playing HUMAN recording: $audioFile');
        print('   Text: "$text"');
        await _audioPlayer!.play(AssetSource(audioFile));
        return true;
      } else {
        // Check if it's a Premium TTS voice
        if (_premiumTts != null && _premiumTts!.availableVoices.contains(_selectedVoice)) {
          print('🎭 Using Premium TTS for: "$text"');
          return await _speakWithPremiumTTS(text);
        } else {
          // Fallback to FlutterTTS
          print('🤖 Using FlutterTTS fallback for: "$text"');
          return await _speakWithEnhancedTTS(text);
        }
      }
      
    } catch (e) {
      print('Voice playback error: $e');
      return await _speakWithEnhancedTTS(text);
    }
  }

  Future<String> _findHumanRecording(String text) async {
    String cleanText = text.toLowerCase()
        .replaceAll(RegExp(r'[^\w\s]'), '')
        .trim();
    
    // Mapping of common affirmations to recording files
    // In a real app, you'd have actual human voice recordings here
    Map<String, List<String>> recordingMaps = {
      'i_am_worthy': [
        'i am worthy', 'i am worthy of love', 'i am worthy of success',
        'i am worthy of happiness', 'i deserve good things', 'i deserve love',
        'i am enough', 'i am valuable', 'i matter'
      ],
      
      'i_am_confident': [
        'i am confident', 'i am strong', 'i am powerful', 'i believe in myself',
        'i trust myself', 'i am brave', 'i am fearless', 'i am bold'
      ],
      
      'i_am_successful': [
        'i am successful', 'i achieve my goals', 'i create my reality',
        'success flows to me', 'i attract abundance', 'i am prosperous',
        'i manifest my dreams', 'everything works out for me'
      ],
      
      'i_am_loved': [
        'i am loved', 'i love myself', 'i am loveable', 'love surrounds me',
        'i attract love', 'i am worthy of love', 'i radiate love'
      ],
      
      'i_can_achieve': [
        'i can do anything', 'i am capable', 'i have the power',
        'i am unstoppable', 'nothing can stop me', 'i can achieve anything',
        'i overcome challenges', 'i am resilient'
      ],
      
      'i_am_peaceful': [
        'i am at peace', 'i am calm', 'i am serene', 'peace fills my heart',
        'i choose peace', 'i am tranquil', 'i am centered'
      ],
      
      'i_am_grateful': [
        'i am grateful', 'i appreciate my life', 'gratitude fills me',
        'i am thankful', 'i count my blessings', 'i am blessed'
      ],
      
      'i_am_healthy': [
        'i am healthy', 'my body is strong', 'i am full of energy',
        'i feel amazing', 'health flows through me', 'i am vibrant',
        'my body heals itself', 'i am in perfect health'
      ],
      
      'i_choose_happiness': [
        'i choose happiness', 'i am happy', 'joy fills my life',
        'i radiate joy', 'happiness is my natural state', 'i am joyful'
      ],
      
      'i_trust_universe': [
        'i trust the universe', 'everything happens for a reason',
        'i am guided', 'the universe supports me', 'i trust the process',
        'divine timing is perfect'
      ]
    };
    
    // Find the best match
    for (String recordingKey in recordingMaps.keys) {
      List<String> patterns = recordingMaps[recordingKey]!;
      
      for (String pattern in patterns) {
        if (_textMatches(cleanText, pattern)) {
          String voiceFolder = _getVoiceFolder(_selectedVoice);
          String audioPath = 'audio/human_voices/$voiceFolder/$recordingKey.mp3';
          
          // In a real implementation, these would be actual MP3 files
          // For now, we'll simulate having some recordings
          if (_simulateRecordingExists(recordingKey)) {
            return audioPath;
          }
        }
      }
    }
    
    return ''; // No recording found
  }

  bool _textMatches(String text, String pattern) {
    if (text == pattern) return true;
    
    List<String> patternWords = pattern.split(' ');
    List<String> textWords = text.split(' ');
    
    int matchCount = 0;
    for (String patternWord in patternWords) {
      if (textWords.any((word) => word.contains(patternWord) || patternWord.contains(word))) {
        matchCount++;
      }
    }
    
    return (matchCount / patternWords.length) >= 0.6; // 60% match threshold
  }

  String _getVoiceFolder(String voice) {
    return voice; // sarah_calm, emma_energetic, etc.
  }

  bool _simulateRecordingExists(String recordingKey) {
    // Simulate having recordings for common affirmations
    List<String> availableRecordings = [
      'i_am_worthy', 'i_am_confident', 'i_am_loved', 'i_am_peaceful',
      'i_am_grateful', 'i_can_achieve'
    ];
    
    return availableRecordings.contains(recordingKey);
  }

  Future<bool> _speakWithPremiumTTS(String text) async {
    if (_premiumTts == null) return false;
    
    try {
      // Set the selected Premium voice
      if (_premiumTts!.availableVoices.contains(_selectedVoice)) {
        await _premiumTts!.setVoice(_selectedVoice);
      }
      
      // Use Premium TTS for natural human-like speech
      final success = await _premiumTts!.speak(text);
      if (success) {
        _isPlaying = true;
        return true;
      }
      
      // Fallback to FlutterTTS if Premium TTS fails
      print('Premium TTS failed, falling back to FlutterTTS');
      return await _speakWithEnhancedTTS(text);
      
    } catch (e) {
      print('Premium TTS error: $e');
      return await _speakWithEnhancedTTS(text);
    }
  }

  Future<bool> _speakWithEnhancedTTS(String text) async {
    if (_flutterTts == null) return false;
    
    try {
      // Process text for more natural TTS delivery
      String processedText = _processTextForNaturalTTS(text);
      
      final result = await _flutterTts!.speak(processedText);
      if (result == 1) {
        _isPlaying = true;
        return true;
      }
      return false;
      
    } catch (e) {
      print('Enhanced TTS error: $e');
      return false;
    }
  }

  String _processTextForNaturalTTS(String text) {
    String processed = text;
    
    // Add natural pauses
    processed = processed.replaceAll(',', ', ');
    processed = processed.replaceAll('.', '. ');
    
    // Fix contractions for better pronunciation
    processed = processed.replaceAll("I'm", "I am");
    processed = processed.replaceAll("I'll", "I will");
    processed = processed.replaceAll("I've", "I have");
    processed = processed.replaceAll("can't", "cannot");
    processed = processed.replaceAll("won't", "will not");
    
    // Add subtle emphasis markers (some TTS engines support this)
    processed = processed.replaceAll(RegExp(r'\b(I am|I will|I can)\b'), r'<emphasis level="moderate">\1</emphasis>');
    
    return processed.trim();
  }

  Future<void> stop() async {
    try {
      if (_audioPlayer != null) {
        await _audioPlayer!.stop();
      }
      if (_premiumTts != null) {
        await _premiumTts!.stop();
      }
      if (_flutterTts != null) {
        await _flutterTts!.stop();
      }
    } catch (e) {
      print('Error stopping playback: $e');
    }
    
    _completionTimer?.cancel();
    _isPlaying = false;
    _currentText = '';
  }


  Future<void> setVoice(String voice) async {
    if (availableVoices.contains(voice)) {
      _selectedVoice = voice;
      
      // If it's a Premium TTS voice, set it
      if (_premiumTts != null && _premiumTts!.availableVoices.contains(voice)) {
        await _premiumTts!.setVoice(voice);
      }
      
      print('Voice set to: $voice');
    }
  }

  Future<void> setVolume(double volume) async {
    _volume = volume.clamp(0.0, 1.0);
    try {
      if (_audioPlayer != null) {
        await _audioPlayer!.setVolume(_volume);
      }
      if (_flutterTts != null) {
        await _flutterTts!.setVolume(_volume);
      }
    } catch (e) {
      print('Error setting volume: $e');
    }
  }

  Future<void> setRate(double rate) async {
    _ttsRate = rate.clamp(0.1, 1.0);
    try {
      if (_flutterTts != null) {
        await _flutterTts!.setSpeechRate(_ttsRate);
      }
    } catch (e) {
      print('Error setting TTS rate: $e');
    }
  }

  Future<void> setPitch(double pitch) async {
    _ttsPitch = pitch.clamp(0.5, 1.5);
    try {
      if (_flutterTts != null) {
        await _flutterTts!.setPitch(_ttsPitch);
      }
    } catch (e) {
      print('Error setting TTS pitch: $e');
    }
  }

  String getVoiceQuality(String voice) {
    // Check if it's a Premium TTS voice
    if (_premiumTts != null && _premiumTts!.availableVoices.contains(voice)) {
      return _premiumTts!.getVoiceQuality(voice);
    }
    
    // Human recordings are the highest quality
    return 'Human + AI Hybrid';
  }

  String getVoiceDescription(String voice) {
    // Check if it's a Premium TTS voice
    if (_premiumTts != null && _premiumTts!.availableVoices.contains(voice)) {
      return _premiumTts!.getVoiceDescription(voice);
    }
    
    // Human recording descriptions
    Map<String, String> descriptions = {
      'sarah_calm': 'Sarah - Serene human voice with AI enhancement',
      'emma_energetic': 'Emma - Vibrant human voice with AI enhancement', 
      'lisa_soothing': 'Lisa - Gentle human voice with AI enhancement',
      'david_confident': 'David - Strong human voice with AI enhancement',
      'michael_warm': 'Michael - Caring human voice with AI enhancement',
      'james_inspiring': 'James - Motivational human voice with AI enhancement',
    };
    
    return descriptions[voice] ?? 'Professional hybrid voice';
  }

  // Get statistics about human vs AI usage
  Map<String, dynamic> getUsageStats() {
    return {
      'hasHumanRecordings': true,
      'totalRecordings': 6, // Simulated count
      'fallbackToTTS': 'Enhanced AI when no recording available',
      'quality': 'Human recordings preferred, premium TTS fallback'
    };
  }

  void dispose() {
    _completionTimer?.cancel();
    _audioPlayer?.dispose();
    _premiumTts?.dispose();
    _flutterTts?.stop();
    _isInitialized = false;
    _isPlaying = false;
    _currentText = '';
  }
}