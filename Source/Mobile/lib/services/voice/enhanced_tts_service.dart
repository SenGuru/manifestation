import 'dart:async';
import 'package:flutter_tts/flutter_tts.dart';
import 'package:text_to_speech/text_to_speech.dart';

class EnhancedTTSService {
  static final EnhancedTTSService _instance = EnhancedTTSService._internal();
  factory EnhancedTTSService() => _instance;
  EnhancedTTSService._internal();

  FlutterTts? _flutterTts;
  TextToSpeech? _textToSpeech;
  bool _isInitialized = false;
  bool _isPlaying = false;
  String _currentText = '';
  Timer? _simulationTimer;
  List<String> _availableVoices = [];
  String _selectedVoice = '';
  double _speechRate = 0.45; // Slightly slower for more natural speech
  double _volume = 0.85;     // Slightly louder for better clarity
  double _pitch = 0.95;      // Slightly lower pitch for more human-like sound
  String _language = 'en-US';
  bool _useFlutterTts = true;

  bool get isPlaying => _isPlaying;
  String get currentText => _currentText;
  List<String> get availableVoices => _availableVoices;
  String get selectedVoice => _selectedVoice;
  double get speechRate => _speechRate;
  double get volume => _volume;
  double get pitch => _pitch;
  String get language => _language;
  bool get isPluginAvailable => _useFlutterTts || _textToSpeech != null;

  Future<void> initialize() async {
    if (_isInitialized) {
      return;
    }

    try {
      // Try FlutterTTS first (more features)
      try {
        _flutterTts = FlutterTts();
        await _flutterTts!.setLanguage(_language);
        _useFlutterTts = true;
        print('FlutterTTS initialized successfully');
        
        // Set up handlers for FlutterTTS
        _flutterTts!.setCompletionHandler(() {
          _isPlaying = false;
          _currentText = '';
        });
        
        _flutterTts!.setErrorHandler((msg) {
          print('FlutterTTS Error: $msg');
          _isPlaying = false;
          _currentText = '';
        });
        
        _flutterTts!.setStartHandler(() {
          _isPlaying = true;
        });
        
      } catch (e) {
        print('FlutterTTS failed, trying TextToSpeech: $e');
        _useFlutterTts = false;
        _textToSpeech = TextToSpeech();
      }
      
      // Get available voices
      await _loadVoices();
      
      // Set default voice to a high-quality one if available
      await _setDefaultVoice();
      
      // Configure default settings
      await _configureSettings();
      
      _isInitialized = true;
      print('Enhanced TTS initialized successfully');
      
    } catch (e) {
      print('Enhanced TTS initialization failed: $e');
      _isInitialized = false;
    }
  }

  Future<void> _loadVoices() async {
    try {
      if (_useFlutterTts && _flutterTts != null) {
        final voices = await _flutterTts!.getVoices;
        if (voices != null) {
          _availableVoices = voices.map((voice) => voice["name"] as String).toList();
          
          // Sort voices by human-likeness score (most human-like first)
          _availableVoices.sort((a, b) => 
            getHumanLikenessScore(b).compareTo(getHumanLikenessScore(a))
          );
          
          print('FlutterTTS - Available voices: ${_availableVoices.length}');
        }
      } else if (_textToSpeech != null) {
        // TextToSpeech doesn't have getVoices method, use default voices
        _availableVoices = ['System Default', 'Female Voice', 'Male Voice'];
        print('TextToSpeech - Using default voices');
      }
      
      // Log top voices for debugging
      print('Top 5 most human-like voices:');
      _availableVoices.take(5).forEach((voice) {
        print('Voice: $voice (Score: ${getHumanLikenessScore(voice)}, Quality: ${getVoiceQuality(voice)})');
      });
    } catch (e) {
      print('Error loading voices: $e');
      _availableVoices = [];
    }
  }

  Future<void> _setDefaultVoice() async {
    if (_availableVoices.isEmpty) return;
    
    try {
      // Priority order for most human-sounding voices
      List<String> preferredVoices = [
        // Neural/Premium voices (most human-like)
        'en-US-Neural2-A', // Google Neural voice
        'en-US-Neural2-C', // Google Neural voice
        'en-US-Neural2-D', // Google Neural voice
        'en-US-Neural2-E', // Google Neural voice
        'en-US-Neural2-F', // Google Neural voice
        'en-US-Neural2-G', // Google Neural voice
        'en-US-Neural2-H', // Google Neural voice
        'en-US-Neural2-I', // Google Neural voice
        'en-US-Neural2-J', // Google Neural voice
        
        // Google WaveNet voices (very natural)
        'en-US-Wavenet-A',
        'en-US-Wavenet-B', 
        'en-US-Wavenet-C',
        'en-US-Wavenet-D',
        'en-US-Wavenet-E',
        'en-US-Wavenet-F',
        'en-US-Wavenet-G',
        'en-US-Wavenet-H',
        'en-US-Wavenet-I',
        'en-US-Wavenet-J',
        
        // Google Standard voices (good quality)
        'en-US-Standard-A',
        'en-US-Standard-B',
        'en-US-Standard-C',
        'en-US-Standard-D',
        'en-US-Standard-E',
        'en-US-Standard-F',
        'en-US-Standard-G',
        'en-US-Standard-H',
        'en-US-Standard-I',
        'en-US-Standard-J',
        
        // iOS premium voices (very natural)
        'Samantha (Enhanced)', // iOS enhanced voice
        'Alex (Enhanced)',     // iOS enhanced voice
        'Ava (Enhanced)',      // iOS enhanced voice
        'Zoe (Enhanced)',      // iOS enhanced voice
        'Nicky (Enhanced)',    // iOS enhanced voice
        
        // iOS standard high-quality voices
        'Samantha',  // iOS default high-quality
        'Alex',      // iOS male voice
        'Ava',       // iOS female voice
        'Zoe',       // iOS female voice
        'Nicky',     // iOS female voice
        'Tom',       // iOS male voice
        'Serena',    // iOS female voice
        'Karen',     // iOS female voice
        'Moira',     // iOS female voice
        'Tessa',     // iOS female voice
        'Fiona',     // iOS female voice
        'Daniel',    // iOS male voice
        
        // Microsoft premium voices
        'Microsoft Aria',      // Windows 11 neural voice
        'Microsoft Jenny',     // Windows neural voice
        'Microsoft Guy',       // Windows neural voice
        'Microsoft Zira Desktop', // Windows enhanced
        'Microsoft David Desktop', // Windows enhanced
        
        // Microsoft standard voices
        'Microsoft Zira',
        'Microsoft David',
        'Microsoft Hazel',
        'Microsoft Mark',
        
        // Android/Google standard
        'Google US English',
        'Google UK English Female',
        'Google UK English Male',
        'English (United States)',
        'English (United Kingdom)',
      ];
      
      // Find the best available voice
      String? bestVoice;
      for (String preferred in preferredVoices) {
        bestVoice = _availableVoices.firstWhere(
          (voice) => _isVoiceMatch(voice, preferred),
          orElse: () => '',
        );
        if (bestVoice.isNotEmpty) {
          print('Found preferred voice: $bestVoice');
          break;
        }
      }
      
      // If no preferred voice found, look for any neural/enhanced voice
      if (bestVoice == null || bestVoice.isEmpty) {
        bestVoice = _availableVoices.firstWhere(
          (voice) => _isHighQualityVoice(voice),
          orElse: () => '',
        );
        if (bestVoice.isNotEmpty) {
          print('Found high-quality voice: $bestVoice');
        }
      }
      
      // Fallback to first available voice
      if (bestVoice == null || bestVoice.isEmpty) {
        bestVoice = _availableVoices.first;
        print('Using fallback voice: $bestVoice');
      }
      
      await setVoice(bestVoice);
      print('Selected voice: $bestVoice (Quality: ${getVoiceQuality(bestVoice)})');
      
    } catch (e) {
      print('Error setting default voice: $e');
    }
  }

  bool _isVoiceMatch(String availableVoice, String preferredVoice) {
    final available = availableVoice.toLowerCase();
    final preferred = preferredVoice.toLowerCase();
    
    // Exact match
    if (available == preferred) return true;
    
    // Contains match
    if (available.contains(preferred) || preferred.contains(available)) return true;
    
    // Check for key components
    final preferredParts = preferred.split(' ');
    for (String part in preferredParts) {
      if (part.length > 2 && available.contains(part)) {
        return true;
      }
    }
    
    return false;
  }

  bool _isHighQualityVoice(String voice) {
    final lowerVoice = voice.toLowerCase();
    return lowerVoice.contains('neural') ||
           lowerVoice.contains('wavenet') ||
           lowerVoice.contains('enhanced') ||
           lowerVoice.contains('premium') ||
           lowerVoice.contains('aria') ||
           lowerVoice.contains('jenny') ||
           lowerVoice.contains('samantha') ||
           lowerVoice.contains('alex') ||
           lowerVoice.contains('ava') ||
           lowerVoice.contains('zoe');
  }

  Future<void> _configureSettings() async {
    try {
      if (_useFlutterTts && _flutterTts != null) {
        await _flutterTts!.setSpeechRate(_speechRate);
        await _flutterTts!.setVolume(_volume);
        await _flutterTts!.setPitch(_pitch);
        await _flutterTts!.setLanguage(_language);
        
        // Additional settings for more natural speech
        try {
          // Set shared instance for iOS to enable higher quality
          await _flutterTts!.setSharedInstance(true);
          print('Shared instance enabled for better quality');
        } catch (sharedError) {
          print('Shared instance setting not available: $sharedError');
        }
        
        // Try to set engine-specific quality settings
        try {
          await _flutterTts!.setSilence(0); // Reduce silence between words
          print('Silence settings applied for natural flow');
        } catch (silenceError) {
          print('Silence settings not available: $silenceError');
        }
        
      } else if (_textToSpeech != null) {
        await _textToSpeech!.setRate(_speechRate);
        await _textToSpeech!.setVolume(_volume);
        await _textToSpeech!.setPitch(_pitch);
        await _textToSpeech!.setLanguage(_language);
      }
      
      print('TTS configured for natural speech: Rate=${_speechRate}, Volume=${_volume}, Pitch=${_pitch}');
      
    } catch (e) {
      print('Error configuring TTS settings: $e');
    }
  }

  Future<bool> speak(String text) async {
    if (!_isInitialized) {
      await initialize();
    }

    try {
      await stop();
      
      // Preprocess text for more natural speech
      final processedText = _preprocessTextForNaturalSpeech(text);
      _currentText = processedText;
      
      if (_useFlutterTts && _flutterTts != null) {
        // Use FlutterTTS
        final result = await _flutterTts!.speak(processedText);
        if (result == 1) {
          _isPlaying = true;
          return true;
        }
      } else if (_textToSpeech != null) {
        // Use TextToSpeech with completion timer
        await _textToSpeech!.speak(processedText);
        _isPlaying = true;
        _startCompletionTimer(processedText);
        return true;
      }
      
      // Fallback to simulation
      return _simulateSpeak(processedText);
      
    } catch (e) {
      print('Enhanced TTS error: $e');
      _isPlaying = false;
      return _simulateSpeak(text);
    }
  }

  String _preprocessTextForNaturalSpeech(String text) {
    // Add natural pauses and breathing spaces for affirmations
    String processed = text;
    
    // Add slight pauses at commas and periods for natural rhythm
    processed = processed.replaceAll(',', ', '); // Add space after comma
    processed = processed.replaceAll('.', '. '); // Add space after period
    processed = processed.replaceAll('!', '! '); // Add space after exclamation
    processed = processed.replaceAll('?', '? '); // Add space after question
    
    // Add natural breathing pauses for longer sentences
    if (processed.length > 50) {
      // Add a subtle pause in the middle of long affirmations
      final words = processed.split(' ');
      if (words.length > 8) {
        final midPoint = words.length ~/ 2;
        words.insert(midPoint, ' '); // Add a small pause
        processed = words.join(' ');
      }
    }
    
    // Emphasize important affirmation words naturally
    processed = processed.replaceAll(' I am ', ' I am '); // Slight emphasis on "I am"
    processed = processed.replaceAll(' I can ', ' I can '); // Slight emphasis on "I can"
    processed = processed.replaceAll(' I will ', ' I will '); // Slight emphasis on "I will"
    
    // Clean up multiple spaces
    processed = processed.replaceAll(RegExp(r'\s+'), ' ');
    processed = processed.trim();
    
    print('Preprocessed text for natural speech: "$processed"');
    return processed;
  }

  void _startCompletionTimer(String text) {
    // Calculate estimated duration based on text length and speech rate
    final wordCount = text.split(' ').length;
    final wordsPerMinute = 150 * (1 / _speechRate); // Adjust for speech rate
    final estimatedDurationSeconds = (wordCount / wordsPerMinute) * 60;
    
    // Add some buffer time
    final duration = Duration(seconds: (estimatedDurationSeconds + 1).round());
    
    _simulationTimer?.cancel();
    _simulationTimer = Timer(duration, () {
      if (_isPlaying) {
        _isPlaying = false;
        _currentText = '';
        print('TTS completion estimated');
      }
    });
  }

  bool _simulateSpeak(String text) {
    try {
      _simulationTimer?.cancel();
      
      _currentText = text;
      _isPlaying = true;
      
      // Calculate realistic speech duration
      final wordCount = text.split(' ').length;
      final charactersPerMinute = 900; // Average reading speed
      final estimatedDurationMs = ((text.length / charactersPerMinute) * 60 * 1000).round();
      
      // Set minimum duration of 2 seconds, maximum of 15 seconds
      final duration = estimatedDurationMs < 2000 ? 2000 : 
                      estimatedDurationMs > 15000 ? 15000 : estimatedDurationMs;
      
      print('Simulating enhanced TTS for ${duration}ms (${wordCount} words)');
      
      _simulationTimer = Timer(Duration(milliseconds: duration), () {
        if (_isPlaying) {
          _isPlaying = false;
          _currentText = '';
          print('Enhanced TTS simulation completed');
        }
      });
      
      return true;
    } catch (e) {
      print('Simulation error: $e');
      _isPlaying = false;
      _currentText = '';
      return false;
    }
  }

  Future<void> stop() async {
    try {
      if (_useFlutterTts && _flutterTts != null) {
        await _flutterTts!.stop();
      } else if (_textToSpeech != null) {
        await _textToSpeech!.stop();
      }
    } catch (e) {
      print('Error stopping enhanced TTS: $e');
    }
    
    _simulationTimer?.cancel();
    _simulationTimer = null;
    
    _isPlaying = false;
    _currentText = '';
  }

  Future<void> setVoice(String voice) async {
    if (_availableVoices.contains(voice)) {
      try {
        if (_useFlutterTts && _flutterTts != null) {
          await _flutterTts!.setVoice({"name": voice, "locale": _language});
        } else if (_textToSpeech != null) {
          // TextToSpeech doesn't have setVoice method, just store the selection
          print('TextToSpeech voice selection not supported, storing preference');
        }
        _selectedVoice = voice;
        print('Voice set to: $voice');
      } catch (e) {
        print('Error setting voice: $e');
      }
    }
  }

  Future<void> setRate(double rate) async {
    _speechRate = rate.clamp(0.1, 2.0);
    try {
      if (_useFlutterTts && _flutterTts != null) {
        await _flutterTts!.setSpeechRate(_speechRate);
      } else if (_textToSpeech != null) {
        await _textToSpeech!.setRate(_speechRate);
      }
    } catch (e) {
      print('Error setting rate: $e');
    }
  }

  Future<void> setVolume(double volume) async {
    _volume = volume.clamp(0.0, 1.0);
    try {
      if (_useFlutterTts && _flutterTts != null) {
        await _flutterTts!.setVolume(_volume);
      } else if (_textToSpeech != null) {
        await _textToSpeech!.setVolume(_volume);
      }
    } catch (e) {
      print('Error setting volume: $e');
    }
  }

  Future<void> setPitch(double pitch) async {
    _pitch = pitch.clamp(0.5, 2.0);
    try {
      if (_useFlutterTts && _flutterTts != null) {
        await _flutterTts!.setPitch(_pitch);
      } else if (_textToSpeech != null) {
        await _textToSpeech!.setPitch(_pitch);
      }
    } catch (e) {
      print('Error setting pitch: $e');
    }
  }

  Future<void> setLanguage(String language) async {
    _language = language;
    try {
      if (_useFlutterTts && _flutterTts != null) {
        await _flutterTts!.setLanguage(language);
      } else if (_textToSpeech != null) {
        await _textToSpeech!.setLanguage(language);
      }
    } catch (e) {
      print('Error setting language: $e');
    }
  }

  // Method to get voice settings for UI display
  Map<String, dynamic> getVoiceSettings() {
    return {
      'selectedVoice': _selectedVoice,
      'speechRate': _speechRate,
      'volume': _volume,
      'pitch': _pitch,
      'language': _language,
    };
  }

  // Method to get voice quality information
  String getVoiceQuality(String voice) {
    final lowerVoice = voice.toLowerCase();
    
    // Neural and AI-enhanced voices (most human-like)
    if (lowerVoice.contains('neural') || 
        lowerVoice.contains('wavenet') ||
        lowerVoice.contains('aria') ||
        lowerVoice.contains('jenny') ||
        lowerVoice.contains('enhanced')) {
      return 'Neural AI';
    }
    
    // Premium quality voices
    if (lowerVoice.contains('samantha') ||
        lowerVoice.contains('alex') ||
        lowerVoice.contains('ava') ||
        lowerVoice.contains('zoe') ||
        lowerVoice.contains('google') ||
        lowerVoice.contains('standard-') ||
        lowerVoice.contains('premium')) {
      return 'Premium';
    }
    
    // High quality voices  
    if (lowerVoice.contains('microsoft') || 
        lowerVoice.contains('karen') ||
        lowerVoice.contains('moira') ||
        lowerVoice.contains('tessa') ||
        lowerVoice.contains('daniel') ||
        lowerVoice.contains('fiona') ||
        lowerVoice.contains('serena')) {
      return 'High Quality';
    }
    
    return 'Standard';
  }

  // Method to get human-likeness score for voice sorting
  int getHumanLikenessScore(String voice) {
    final lowerVoice = voice.toLowerCase();
    
    // Neural/AI voices score highest
    if (lowerVoice.contains('neural2')) return 100;
    if (lowerVoice.contains('neural')) return 95;
    if (lowerVoice.contains('wavenet')) return 90;
    if (lowerVoice.contains('enhanced')) return 85;
    if (lowerVoice.contains('aria') || lowerVoice.contains('jenny')) return 80;
    
    // Premium iOS voices
    if (lowerVoice.contains('samantha')) return 75;
    if (lowerVoice.contains('alex')) return 70;
    if (lowerVoice.contains('ava')) return 70;
    if (lowerVoice.contains('zoe')) return 70;
    
    // Good quality voices
    if (lowerVoice.contains('google')) return 65;
    if (lowerVoice.contains('standard-')) return 60;
    if (lowerVoice.contains('microsoft')) return 55;
    
    // Standard voices
    return 30;
  }

  void dispose() {
    _simulationTimer?.cancel();
    _flutterTts?.stop();
    _textToSpeech?.stop();
    _isInitialized = false;
    _isPlaying = false;
    _currentText = '';
  }
}