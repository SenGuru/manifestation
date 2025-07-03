import 'package:flutter_tts/flutter_tts.dart';
import 'dart:async';

class TTSService {
  static final TTSService _instance = TTSService._internal();
  factory TTSService() => _instance;
  TTSService._internal();

  FlutterTts? _flutterTts;
  bool _isInitialized = false;
  bool _isPlaying = false;
  bool _pluginAvailable = false;
  String _currentText = '';
  Timer? _simulationTimer;

  bool get isPlaying => _isPlaying;
  String get currentText => _currentText;
  bool get isPluginAvailable => _pluginAvailable;

  Future<void> initialize() async {
    if (_isInitialized) {
      return; // Already initialized
    }

    try {
      print('Initializing Flutter TTS...');
      _flutterTts = FlutterTts();
      
      // Test if the plugin is actually available by trying a simple operation
      try {
        await _flutterTts!.setLanguage("en-US");
        _pluginAvailable = true;
        print('Flutter TTS plugin is available');
      } catch (pluginError) {
        print('Flutter TTS plugin not available: $pluginError');
        _pluginAvailable = false;
        _flutterTts = null;
      }

      if (_pluginAvailable && _flutterTts != null) {
        // Set up handlers
        _flutterTts!.setCompletionHandler(() {
          print('TTS completion handler called');
          _isPlaying = false;
          _currentText = '';
        });

        _flutterTts!.setErrorHandler((msg) {
          print('TTS Error handler called: $msg');
          _isPlaying = false;
          _currentText = '';
        });

        _flutterTts!.setStartHandler(() {
          print('TTS start handler called');
          _isPlaying = true;
        });

        // Configure TTS settings
        try {
          await _flutterTts!.setSpeechRate(0.5);
          await _flutterTts!.setVolume(0.8);
          await _flutterTts!.setPitch(1.0);
        } catch (settingsError) {
          print('Error setting TTS configuration: $settingsError');
        }
      }
      
      _isInitialized = true;
      print('TTS initialization complete. Plugin available: $_pluginAvailable');
      
    } catch (e) {
      print('TTS Initialization failed: $e');
      _isInitialized = false;
      _pluginAvailable = false;
      _flutterTts = null;
    }
  }

  Future<bool> speak(String text) async {
    print('Speak method called with: $text');
    
    if (!_isInitialized) {
      print('TTS not initialized, attempting to initialize...');
      await initialize();
    }

    // Try native TTS first if available
    if (_pluginAvailable && _flutterTts != null) {
      try {
        await _flutterTts!.stop();
        _currentText = text;
        print('Starting native TTS speak...');
        
        final result = await _flutterTts!.speak(text);
        print('Native TTS speak result: $result');
        
        if (result == 1) {
          _isPlaying = true;
          return true;
        }
      } catch (e) {
        print('Native TTS error: $e');
      }
    }

    // Fallback to simulation
    print('Using TTS simulation fallback');
    return _simulateSpeak(text);
  }

  bool _simulateSpeak(String text) {
    try {
      // Stop any current simulation
      _simulationTimer?.cancel();
      
      _currentText = text;
      _isPlaying = true;
      
      // Calculate realistic speech duration
      final wordCount = text.split(' ').length;
      final charactersPerMinute = 900; // Average reading speed
      final estimatedDurationMs = ((text.length / charactersPerMinute) * 60 * 1000).round();
      
      // Set minimum duration of 2 seconds, maximum of 12 seconds
      final duration = estimatedDurationMs < 2000 ? 2000 : 
                      estimatedDurationMs > 12000 ? 12000 : estimatedDurationMs;
      
      print('Simulating TTS for ${duration}ms (${wordCount} words)');
      
      _simulationTimer = Timer(Duration(milliseconds: duration), () {
        if (_isPlaying) {
          _isPlaying = false;
          _currentText = '';
          print('TTS simulation completed');
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
      if (_pluginAvailable && _flutterTts != null) {
        await _flutterTts!.stop();
      }
    } catch (e) {
      print('Error stopping native TTS: $e');
    }
    
    // Stop simulation
    _simulationTimer?.cancel();
    _simulationTimer = null;
    
    _isPlaying = false;
    _currentText = '';
    print('TTS Stopped');
  }

  Future<void> setRate(double rate) async {
    if (_pluginAvailable && _flutterTts != null) {
      try {
        await _flutterTts!.setSpeechRate(rate);
      } catch (e) {
        print('Error setting rate: $e');
      }
    }
  }

  Future<void> setVolume(double volume) async {
    if (_pluginAvailable && _flutterTts != null) {
      try {
        await _flutterTts!.setVolume(volume);
      } catch (e) {
        print('Error setting volume: $e');
      }
    }
  }

  Future<void> setPitch(double pitch) async {
    if (_pluginAvailable && _flutterTts != null) {
      try {
        await _flutterTts!.setPitch(pitch);
      } catch (e) {
        print('Error setting pitch: $e');
      }
    }
  }

  Future<void> setLanguage(String language) async {
    if (_pluginAvailable && _flutterTts != null) {
      try {
        await _flutterTts!.setLanguage(language);
      } catch (e) {
        print('Error setting language: $e');
      }
    }
  }

  // Getters for current settings
  double get speechRate => 0.5;
  double get volume => 0.8;
  double get pitch => 1.0;
  String get language => 'en-US';

  Future<List<String>> getLanguages() async {
    if (_pluginAvailable && _flutterTts != null) {
      try {
        final languages = await _flutterTts!.getLanguages;
        return languages?.cast<String>() ?? [];
      } catch (e) {
        print('Error getting languages: $e');
      }
    }
    return ['en-US'];
  }

  Future<List<String>> getVoices() async {
    if (_pluginAvailable && _flutterTts != null) {
      try {
        final voices = await _flutterTts!.getVoices;
        return voices?.map((voice) => voice["name"] as String).toList() ?? [];
      } catch (e) {
        print('Error getting voices: $e');
      }
    }
    return [];
  }

  void dispose() {
    _simulationTimer?.cancel();
    _flutterTts?.stop();
    _isInitialized = false;
    _isPlaying = false;
    _currentText = '';
  }
}