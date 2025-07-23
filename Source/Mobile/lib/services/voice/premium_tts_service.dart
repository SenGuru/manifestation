import 'dart:async';
import 'package:flutter_tts/flutter_tts.dart';

class PremiumTTSService {
  static final PremiumTTSService _instance = PremiumTTSService._internal();
  factory PremiumTTSService() => _instance;
  PremiumTTSService._internal();

  FlutterTts? _flutterTts;
  bool _isPlaying = false;
  bool _isInitialized = false;
  
  // Voice settings
  String _selectedVoice = '';
  double _speechRate = 0.45; // Slower for more natural speech
  double _volume = 0.9;
  double _pitch = 0.85; // Lower pitch sounds more human
  
  // Premium voices found on device
  List<Map<String, dynamic>> _premiumVoices = [];

  // Ultra-premium voice patterns that sound most human
  static const List<Map<String, dynamic>> _ultraHumanVoicePatterns = [
    // iOS Premium Enhanced Voices (highest quality)
    {'pattern': 'samantha', 'priority': 100, 'quality': 'Ultra Human', 'name': 'Samantha'},
    {'pattern': 'alex', 'priority': 95, 'quality': 'Ultra Human', 'name': 'Alex'},
    {'pattern': 'ava', 'priority': 90, 'quality': 'Ultra Human', 'name': 'Ava'},
    {'pattern': 'zoe', 'priority': 85, 'quality': 'Ultra Human', 'name': 'Zoe'},
    
    // Google Neural voices (Android/newer systems)
    {'pattern': 'neural2', 'priority': 98, 'quality': 'Ultra Human', 'name': 'Neural AI'},
    {'pattern': 'neural', 'priority': 88, 'quality': 'Very Human', 'name': 'Neural Voice'},
    {'pattern': 'wavenet', 'priority': 82, 'quality': 'Very Human', 'name': 'WaveNet'},
    
    // Microsoft Premium voices
    {'pattern': 'aria', 'priority': 80, 'quality': 'Very Human', 'name': 'Aria'},
    {'pattern': 'jenny', 'priority': 75, 'quality': 'Very Human', 'name': 'Jenny'},
    
    // iOS High Quality voices
    {'pattern': 'nicky', 'priority': 70, 'quality': 'Premium', 'name': 'Nicky'},
    {'pattern': 'tom', 'priority': 68, 'quality': 'Premium', 'name': 'Tom'},
    {'pattern': 'serena', 'priority': 66, 'quality': 'Premium', 'name': 'Serena'},
    
    // Enhanced indicators
    {'pattern': 'enhanced', 'priority': 78, 'quality': 'Very Human', 'name': 'Enhanced'},
    {'pattern': 'premium', 'priority': 76, 'quality': 'Very Human', 'name': 'Premium'},
    {'pattern': 'high-quality', 'priority': 74, 'quality': 'Premium', 'name': 'High Quality'},
  ];

  bool get isPlaying => _isPlaying;
  bool get isInitialized => _isInitialized;
  List<String> get availableVoices => _premiumVoices.map((v) => v['id'] as String).toList();
  String get selectedVoice => _selectedVoice;
  double get speechRate => _speechRate;
  double get volume => _volume;
  double get pitch => _pitch;

  Future<void> initialize() async {
    if (_isInitialized) return;

    try {
      _flutterTts = FlutterTts();
      
      // Configure for maximum quality
      await _flutterTts!.setLanguage('en-US');
      await _flutterTts!.setSpeechRate(_speechRate);
      await _flutterTts!.setVolume(_volume);
      await _flutterTts!.setPitch(_pitch);
      
      // Enable high-quality audio on iOS
      try {
        await _flutterTts!.setSharedInstance(true);
        await _flutterTts!.setSilence(100); // Natural pauses
      } catch (e) {
        print('Advanced TTS settings not available: $e');
      }
      
      // Set up handlers
      _flutterTts!.setCompletionHandler(() {
        _isPlaying = false;
        print('Premium TTS completed');
      });
      
      _flutterTts!.setStartHandler(() {
        _isPlaying = true;
      });
      
      _flutterTts!.setErrorHandler((msg) {
        print('Premium TTS Error: $msg');
        _isPlaying = false;
      });
      
      // Find and rank premium voices
      await _findPremiumVoices();
      
      // Set the best voice as default
      if (_premiumVoices.isNotEmpty) {
        await setVoice(_premiumVoices.first['id']);
      }
      
      _isInitialized = true;
      print('Premium TTS initialized with ${_premiumVoices.length} high-quality voices');
      
    } catch (e) {
      print('Premium TTS initialization failed: $e');
      _isInitialized = false;
    }
  }

  Future<void> _findPremiumVoices() async {
    try {
      final voices = await _flutterTts!.getVoices;
      if (voices == null) return;
      
      _premiumVoices.clear();
      Map<String, Map<String, dynamic>> voiceData = {};
      
      // Analyze each voice for quality
      for (var voice in voices) {
        final voiceName = voice['name']?.toString() ?? '';
        final voiceLocale = voice['locale']?.toString() ?? '';
        final lowerName = voiceName.toLowerCase();
        
        // Only English voices
        if (!voiceLocale.toLowerCase().startsWith('en')) continue;
        
        // Find best matching pattern
        Map<String, dynamic>? bestMatch;
        int highestPriority = 0;
        
        for (var pattern in _ultraHumanVoicePatterns) {
          if (lowerName.contains(pattern['pattern'])) {
            if (pattern['priority'] > highestPriority) {
              highestPriority = pattern['priority'];
              bestMatch = pattern;
            }
          }
        }
        
        if (bestMatch != null) {
          voiceData[voiceName] = {
            'id': voiceName,
            'name': _beautifyVoiceName(voiceName, bestMatch['name']),
            'quality': bestMatch['quality'],
            'priority': bestMatch['priority'],
            'locale': voiceLocale,
            'raw': voice,
          };
        }
      }
      
      // Sort by priority (highest first) and add to premium voices
      var sortedVoices = voiceData.values.toList()
        ..sort((a, b) => b['priority'].compareTo(a['priority']));
      
      _premiumVoices = sortedVoices.take(10).toList(); // Top 10 voices
      
      print('Found ${_premiumVoices.length} premium voices:');
      for (var voice in _premiumVoices.take(3)) {
        print('  ${voice['name']} - ${voice['quality']} (${voice['priority']})');
      }
      
    } catch (e) {
      print('Error finding premium voices: $e');
    }
  }

  String _beautifyVoiceName(String rawName, String patternName) {
    // Clean up voice names for better display
    String name = rawName;
    
    // Remove common prefixes/suffixes
    name = name.replaceAll(RegExp(r'\b(com\.apple\.speech\.synthesis\.voice\.|Microsoft |Google |Samsung )', caseSensitive: false), '');
    name = name.replaceAll(RegExp(r'\.(premium|enhanced|compact)$', caseSensitive: false), '');
    
    // Use pattern name if it's cleaner
    if (patternName.length < name.length && !patternName.contains('AI')) {
      return patternName;
    }
    
    return name;
  }

  Future<bool> speak(String text) async {
    if (!_isInitialized || _flutterTts == null) {
      print('Premium TTS not initialized');
      return false;
    }

    try {
      await stop();
      
      // Process text for ultra-natural speech
      final processedText = _processForUltraNaturalSpeech(text);
      
      print('🎭 Using Premium TTS ($_selectedVoice) for: "$text"');
      
      final result = await _flutterTts!.speak(processedText);
      if (result == 1) {
        _isPlaying = true;
        return true;
      }
      
      return false;
      
    } catch (e) {
      print('Premium TTS error: $e');
      _isPlaying = false;
      return false;
    }
  }

  String _processForUltraNaturalSpeech(String text) {
    String processed = text;
    
    // 1. Fix contractions for better pronunciation
    processed = processed.replaceAll("I'm", "I am");
    processed = processed.replaceAll("I'll", "I will");
    processed = processed.replaceAll("I've", "I have");
    processed = processed.replaceAll("can't", "cannot");
    processed = processed.replaceAll("won't", "will not");
    processed = processed.replaceAll("don't", "do not");
    processed = processed.replaceAll("wouldn't", "would not");
    processed = processed.replaceAll("couldn't", "could not");
    processed = processed.replaceAll("shouldn't", "should not");
    
    // 2. Add natural breathing pauses
    processed = processed.replaceAll('. ', '.  '); // Longer pause after sentences
    processed = processed.replaceAll(', ', ',  '); // Pause after commas
    processed = processed.replaceAll(': ', ':  '); // Pause after colons
    
    // 3. Add emphasis spacing for affirmation keywords
    processed = processed.replaceAll(RegExp(r'\\b(I am|I will|I can|I have|I deserve)\\b'), r'  \\1  ');
    
    // 4. Slow down numbers and important words
    processed = processed.replaceAll(RegExp(r'\\b(\\d+)\\b'), r'  \\1  ');
    
    // 5. Clean up multiple spaces
    processed = processed.replaceAll(RegExp(r'\\s+'), ' ').trim();
    
    return processed;
  }

  Future<void> stop() async {
    if (_isPlaying && _flutterTts != null) {
      await _flutterTts!.stop();
      _isPlaying = false;
    }
  }

  Future<void> pause() async {
    if (_flutterTts != null) {
      await _flutterTts!.pause();
    }
  }

  Future<void> resume() async {
    // FlutterTTS doesn't support resume, just restart speech if needed
    if (_flutterTts != null && _isPlaying) {
      // Resume functionality not available in FlutterTTS
    }
  }

  Future<void> setVoice(String voiceId) async {
    if (!availableVoices.contains(voiceId)) return;
    
    try {
      final voiceData = _premiumVoices.firstWhere((v) => v['id'] == voiceId);
      final rawVoice = voiceData['raw'];
      
      await _flutterTts!.setVoice({
        'name': rawVoice['name'],
        'locale': rawVoice['locale'] ?? 'en-US'
      });
      
      _selectedVoice = voiceId;
      print('Set premium voice: ${voiceData['name']}');
      
    } catch (e) {
      print('Error setting premium voice: $e');
    }
  }

  void setSpeechRate(double rate) {
    _speechRate = rate.clamp(0.25, 1.0); // Limit to natural range
    _flutterTts?.setSpeechRate(_speechRate);
  }

  void setVolume(double volume) {
    _volume = volume.clamp(0.0, 1.0);
    _flutterTts?.setVolume(_volume);
  }

  void setPitch(double pitch) {
    _pitch = pitch.clamp(0.5, 1.2); // Limit to natural range
    _flutterTts?.setPitch(_pitch);
  }

  String getVoiceQuality(String voiceId) {
    try {
      final voiceData = _premiumVoices.firstWhere((v) => v['id'] == voiceId);
      return voiceData['quality'] ?? 'Standard';
    } catch (e) {
      return 'Standard';
    }
  }

  String getVoiceName(String voiceId) {
    try {
      final voiceData = _premiumVoices.firstWhere((v) => v['id'] == voiceId);
      return voiceData['name'] ?? voiceId;
    } catch (e) {
      return voiceId;
    }
  }

  String getVoiceDescription(String voiceId) {
    try {
      final voiceData = _premiumVoices.firstWhere((v) => v['id'] == voiceId);
      return '${voiceData['name']} - ${voiceData['quality']} voice';
    } catch (e) {
      return '$voiceId - Premium voice';
    }
  }

  void dispose() {
    _flutterTts?.stop();
    _isInitialized = false;
    _isPlaying = false;
  }
}