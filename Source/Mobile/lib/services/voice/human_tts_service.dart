import 'dart:async';
import 'dart:math';

import 'package:flutter_tts/flutter_tts.dart';

class HumanTTSService {
  static final HumanTTSService _instance = HumanTTSService._internal();
  factory HumanTTSService() => _instance;
  HumanTTSService._internal();

  FlutterTts? _flutterTts;
  bool _isInitialized = false;
  bool _isPlaying = false;
  String _currentText = '';
  Timer? _simulationTimer;
  List<String> _availableVoices = [];
  String _selectedVoice = '';

  // Human-like speech parameters
  double _baseRate = 0.4; // Slower base rate
  double _baseVolume = 0.9; // Higher volume
  double _basePitch = 0.85; // Lower, more natural pitch
  String _language = 'en-US';
  bool _useFlutterTts = true;

  // Advanced human-like settings
  bool _enableVariation = true; // Add natural variation
  bool _enableBreathing = true; // Add breathing pauses
  bool _enableEmphasis = true; // Add emphasis to key words

  bool get isPlaying => _isPlaying;
  String get currentText => _currentText;
  List<String> get availableVoices => _availableVoices;
  String get selectedVoice => _selectedVoice;
  double get speechRate => _baseRate;
  double get volume => _baseVolume;
  double get pitch => _basePitch;
  String get language => _language;
  bool get isPluginAvailable => _useFlutterTts;

  Future<void> initialize() async {
    if (_isInitialized) {
      return;
    }

    try {
      // Try FlutterTTS first
      try {
        _flutterTts = FlutterTts();
        await _flutterTts!.setLanguage(_language);
        _useFlutterTts = true;
        print('FlutterTTS initialized for human-like speech');

        // Set up handlers
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
      }

      // Load and sort voices by human-likeness
      await _loadAndSortVoices();

      // Set the most human-like voice
      await _setMostHumanVoice();

      // Configure for maximum human-likeness
      await _configureHumanLikeSettings();

      _isInitialized = true;
      print('Human TTS initialized successfully');
    } catch (e) {
      print('Human TTS initialization failed: $e');
      _isInitialized = false;
    }
  }

  Future<void> _loadAndSortVoices() async {
    try {
      if (_useFlutterTts && _flutterTts != null) {
        final voices = await _flutterTts!.getVoices;
        if (voices != null) {
          _availableVoices = voices
              .map((voice) => voice["name"] as String)
              .toList();

          // Sort by human-likeness score
          _availableVoices.sort(
            (a, b) =>
                _getHumanLikenessScore(b).compareTo(_getHumanLikenessScore(a)),
          );
        }
      } else {
        _availableVoices = [
          'System Default',
          'Enhanced Female',
          'Enhanced Male',
        ];
      }

      print('Top human-like voices found:');
      _availableVoices.take(3).forEach((voice) {
        print('  $voice (Score: ${_getHumanLikenessScore(voice)})');
      });
    } catch (e) {
      print('Error loading voices: $e');
      _availableVoices = ['System Default'];
    }
  }

  Future<void> _setMostHumanVoice() async {
    if (_availableVoices.isEmpty) return;

    // Ultra-priority list for most human-sounding voices
    List<String> ultraHumanVoices = [
      // iOS Ultra-premium voices (most human)
      'Samantha (Premium)',
      'Samantha',
      'Alex (Premium)',
      'Alex',
      'Ava (Premium)',
      'Ava',
      'Zoe (Premium)',
      'Zoe',
      'Nicky',
      'Tom',
      'Serena',

      // Google Neural voices (extremely natural)
      'en-US-Neural2-H', // Female, very natural
      'en-US-Neural2-F', // Female, conversational
      'en-US-Neural2-C', // Male, natural
      'en-US-Neural2-D', // Male, warm
      'en-US-Neural2-A', // Female, clear
      'en-US-Neural2-J', // Male, friendly
      // Google WaveNet (very natural)
      'en-US-Wavenet-H', // Female
      'en-US-Wavenet-F', // Female
      'en-US-Wavenet-C', // Male
      'en-US-Wavenet-D', // Male
      // Microsoft Neural voices
      'Microsoft Aria', // Ultra-natural female
      'Microsoft Jenny', // Natural female
      'Microsoft Guy', // Natural male
      'Microsoft Davis', // Calm male
      'Microsoft Jane', // Conversational female
    ];

    String? bestVoice;
    for (String preferred in ultraHumanVoices) {
      bestVoice = _availableVoices.firstWhere(
        (voice) => _isVoiceMatch(voice, preferred),
        orElse: () => '',
      );
      if (bestVoice.isNotEmpty) {
        print('Found ultra-human voice: $bestVoice');
        break;
      }
    }

    // If no ultra-human voice, get the highest scored available
    if (bestVoice == null || bestVoice.isEmpty) {
      bestVoice = _availableVoices.first; // Already sorted by score
      print('Using highest-scored voice: $bestVoice');
    }

    await setVoice(bestVoice);
  }

  Future<void> _configureHumanLikeSettings() async {
    try {
      if (_useFlutterTts && _flutterTts != null) {
        // Configure for maximum human-likeness
        await _flutterTts!.setSpeechRate(_baseRate);
        await _flutterTts!.setVolume(_baseVolume);
        await _flutterTts!.setPitch(_basePitch);
        await _flutterTts!.setLanguage(_language);

        // Advanced human-like configurations
        try {
          await _flutterTts!.setSharedInstance(true);
          print('Shared instance enabled for premium quality');
        } catch (e) {
          print('Shared instance not available: $e');
        }

        // Try to enable high-quality audio on iOS
        try {
          await _flutterTts!.setSilence(50); // Small silence for natural rhythm
          print('Natural rhythm configured');
        } catch (e) {
          print('Silence configuration not available: $e');
        }
      }

      print(
        'Human-like TTS configured: Rate=${_baseRate}, Volume=${_baseVolume}, Pitch=${_basePitch}',
      );
    } catch (e) {
      print('Error configuring human-like settings: $e');
    }
  }

  Future<bool> speak(String text) async {
    if (!_isInitialized) {
      await initialize();
    }

    try {
      await stop();

      // Process text for ultra-human speech
      final processedText = _processForUltraHumanSpeech(text);
      _currentText = processedText;

      if (_useFlutterTts && _flutterTts != null) {
        // Speak with dynamic human-like variations
        await _speakWithHumanVariation(processedText);
        return true;
      }

      return _simulateSpeak(processedText);
    } catch (e) {
      print('Human TTS error: $e');
      _isPlaying = false;
      return _simulateSpeak(text);
    }
  }

  Future<void> _speakWithHumanVariation(String text) async {
    if (!_enableVariation) {
      final result = await _flutterTts!.speak(text);
      if (result == 1) _isPlaying = true;
      return;
    }

    // Split into phrases for natural delivery
    List<String> phrases = _splitIntoNaturalPhrases(text);

    for (int i = 0; i < phrases.length; i++) {
      if (!_isPlaying && i > 0) break; // Stop if user stopped

      String phrase = phrases[i].trim();
      if (phrase.isEmpty) continue;

      // Add slight variation to each phrase for naturalness
      double phraseRate =
          _baseRate + (Random().nextDouble() * 0.1 - 0.05); // ±0.05 variation
      double phrasePitch =
          _basePitch + (Random().nextDouble() * 0.1 - 0.05); // ±0.05 variation

      // Adjust for phrase importance
      if (_isImportantPhrase(phrase)) {
        phraseRate *= 0.95; // Slightly slower for emphasis
        phrasePitch *= 0.98; // Slightly lower for gravity
      }

      // Configure for this phrase
      await _flutterTts!.setSpeechRate(phraseRate.clamp(0.3, 0.6));
      await _flutterTts!.setPitch(phrasePitch.clamp(0.7, 1.1));

      // Speak the phrase
      final result = await _flutterTts!.speak(phrase);
      if (result == 1) {
        _isPlaying = true;

        // Add natural pause between phrases
        if (i < phrases.length - 1) {
          await Future.delayed(
            Duration(milliseconds: 200 + Random().nextInt(300)),
          );
        }
      }
    }
  }

  String _processForUltraHumanSpeech(String text) {
    String processed = text;

    // 1. Add natural breathing patterns
    if (_enableBreathing) {
      processed = _addBreathingPauses(processed);
    }

    // 2. Add emphasis markers for key affirmation words
    if (_enableEmphasis) {
      processed = _addEmphasisMarkers(processed);
    }

    // 3. Fix pronunciation for common words
    processed = _fixPronunciation(processed);

    // 4. Add natural rhythm markers
    processed = _addRhythmMarkers(processed);

    print('Ultra-human processed text: "$processed"');
    return processed;
  }

  String _addBreathingPauses(String text) {
    String result = text;

    // Add micro-pauses for natural breathing
    result = result.replaceAll('. ', '.  '); // Longer pause after sentences
    result = result.replaceAll(', ', ',  '); // Pause after commas
    result = result.replaceAll(
      RegExp(r'\b(and|or|but|so|because)\b'),
      r'  \1  ',
    ); // Pause around conjunctions

    // Add breathing before important phrases
    result = result.replaceAll(
      RegExp(r'\b(I am|I will|I can|I have|I deserve)\b'),
      r'  \1',
    );

    return result;
  }

  String _addEmphasisMarkers(String text) {
    String result = text;

    // Emphasize power words in affirmations
    Map<String, String> emphasisWords = {
      'I am': 'I am', // Keep natural
      'I will': 'I will', // Keep natural
      'I can': 'I can', // Keep natural
      'capable': 'capable', // Natural emphasis
      'confident': 'confident',
      'strong': 'strong',
      'powerful': 'powerful',
      'successful': 'successful',
      'worthy': 'worthy',
      'deserving': 'deserving',
      'amazing': 'amazing',
      'beautiful': 'beautiful',
      'brilliant': 'brilliant',
    };

    emphasisWords.forEach((word, replacement) {
      result = result.replaceAll(
        RegExp(r'\b' + word + r'\b', caseSensitive: false),
        replacement,
      );
    });

    return result;
  }

  String _fixPronunciation(String text) {
    String result = text;

    // Fix common mispronunciations in TTS
    Map<String, String> pronunciationFixes = {
      'can\'t': 'cannot',
      'won\'t': 'will not',
      'I\'m': 'I am',
      'I\'ll': 'I will',
      'I\'ve': 'I have',
      'you\'re': 'you are',
      'it\'s': 'it is',
      'that\'s': 'that is',
    };

    pronunciationFixes.forEach((from, to) {
      result = result.replaceAll(from, to);
    });

    return result;
  }

  String _addRhythmMarkers(String text) {
    // Add subtle rhythm cues for more natural delivery
    String result = text;

    // Ensure proper spacing around punctuation
    result = result.replaceAll(RegExp(r'\s*,\s*'), ', ');
    result = result.replaceAll(RegExp(r'\s*\.\s*'), '. ');
    result = result.replaceAll(RegExp(r'\s*!\s*'), '! ');
    result = result.replaceAll(RegExp(r'\s*\?\s*'), '? ');

    // Clean up multiple spaces
    result = result.replaceAll(RegExp(r'\s+'), ' ').trim();

    return result;
  }

  List<String> _splitIntoNaturalPhrases(String text) {
    // Split text into natural speaking phrases
    List<String> phrases = [];

    // First split by sentences
    List<String> sentences = text.split(RegExp(r'[.!?]+'));

    for (String sentence in sentences) {
      if (sentence.trim().isEmpty) continue;

      // If sentence is long, split by natural pauses
      if (sentence.length > 30) {
        List<String> parts = sentence.split(
          RegExp(r'[,;:]|(\sand\s)|(\sor\s)|(\sbut\s)'),
        );
        phrases.addAll(parts.where((p) => p.trim().isNotEmpty));
      } else {
        phrases.add(sentence.trim());
      }
    }

    return phrases;
  }

  bool _isImportantPhrase(String phrase) {
    phrase = phrase.toLowerCase();
    return phrase.contains('i am') ||
        phrase.contains('i will') ||
        phrase.contains('i can') ||
        phrase.contains('i deserve') ||
        phrase.contains('i have');
  }

  int _getHumanLikenessScore(String voice) {
    final lowerVoice = voice.toLowerCase();

    // Ultra-high scores for premium human voices
    if (lowerVoice.contains('samantha')) return 100;
    if (lowerVoice.contains('alex') && !lowerVoice.contains('alexa')) return 98;
    if (lowerVoice.contains('ava')) return 96;
    if (lowerVoice.contains('zoe')) return 94;
    if (lowerVoice.contains('aria')) return 92;
    if (lowerVoice.contains('jenny')) return 90;

    // High scores for neural/enhanced voices
    if (lowerVoice.contains('neural2')) return 95;
    if (lowerVoice.contains('neural')) return 88;
    if (lowerVoice.contains('wavenet')) return 85;
    if (lowerVoice.contains('enhanced')) return 82;
    if (lowerVoice.contains('premium')) return 80;

    // Good scores for quality voices
    if (lowerVoice.contains('nicky')) return 78;
    if (lowerVoice.contains('tom')) return 76;
    if (lowerVoice.contains('serena')) return 74;
    if (lowerVoice.contains('guy')) return 72;
    if (lowerVoice.contains('davis')) return 70;

    // Standard voices
    if (lowerVoice.contains('google')) return 60;
    if (lowerVoice.contains('microsoft')) return 55;
    if (lowerVoice.contains('standard')) return 50;

    return 30;
  }

  bool _isVoiceMatch(String availableVoice, String preferredVoice) {
    final available = availableVoice.toLowerCase();
    final preferred = preferredVoice.toLowerCase();

    if (available == preferred) return true;
    if (available.contains(preferred) || preferred.contains(available))
      return true;

    final preferredParts = preferred.split(' ');
    for (String part in preferredParts) {
      if (part.length > 2 && available.contains(part)) {
        return true;
      }
    }

    return false;
  }

  Future<void> setVoice(String voice) async {
    if (_availableVoices.contains(voice)) {
      try {
        if (_useFlutterTts && _flutterTts != null) {
          await _flutterTts!.setVoice({"name": voice, "locale": _language});
        }
        _selectedVoice = voice;
        print('Human voice set to: $voice');
      } catch (e) {
        print('Error setting voice: $e');
      }
    }
  }

  Future<void> stop() async {
    try {
      if (_useFlutterTts && _flutterTts != null) {
        await _flutterTts!.stop();
      }
    } catch (e) {
      print('Error stopping TTS: $e');
    }

    _simulationTimer?.cancel();
    _isPlaying = false;
    _currentText = '';
  }

  void _startCompletionTimer(String text) {
    final wordCount = text.split(' ').length;
    final estimatedDurationSeconds = (wordCount / 120) * 60; // Slower speech
    final duration = Duration(seconds: (estimatedDurationSeconds + 1).round());

    _simulationTimer?.cancel();
    _simulationTimer = Timer(duration, () {
      if (_isPlaying) {
        _isPlaying = false;
        _currentText = '';
      }
    });
  }

  bool _simulateSpeak(String text) {
    try {
      _simulationTimer?.cancel();
      _currentText = text;
      _isPlaying = true;

      final wordCount = text.split(' ').length;
      final duration = Duration(
        seconds: (wordCount * 0.6).round() + 2,
      ); // Slower simulation

      _simulationTimer = Timer(duration, () {
        if (_isPlaying) {
          _isPlaying = false;
          _currentText = '';
        }
      });

      return true;
    } catch (e) {
      _isPlaying = false;
      return false;
    }
  }

  Future<void> setRate(double rate) async {
    _baseRate = rate.clamp(0.1, 1.0);
    try {
      if (_useFlutterTts && _flutterTts != null) {
        await _flutterTts!.setSpeechRate(_baseRate);
      }
    } catch (e) {
      print('Error setting rate: $e');
    }
  }

  Future<void> setVolume(double volume) async {
    _baseVolume = volume.clamp(0.0, 1.0);
    try {
      if (_useFlutterTts && _flutterTts != null) {
        await _flutterTts!.setVolume(_baseVolume);
      }
    } catch (e) {
      print('Error setting volume: $e');
    }
  }

  Future<void> setPitch(double pitch) async {
    _basePitch = pitch.clamp(0.5, 1.5);
    try {
      if (_useFlutterTts && _flutterTts != null) {
        await _flutterTts!.setPitch(_basePitch);
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
      }
    } catch (e) {
      print('Error setting language: $e');
    }
  }

  String getVoiceQuality(String voice) {
    final score = _getHumanLikenessScore(voice);
    if (score >= 90) return 'Ultra Human';
    if (score >= 80) return 'Very Human';
    if (score >= 70) return 'Human-Like';
    if (score >= 60) return 'Natural';
    return 'Standard';
  }

  void dispose() {
    _simulationTimer?.cancel();
    _flutterTts?.stop();
    _isInitialized = false;
    _isPlaying = false;
    _currentText = '';
  }
}
