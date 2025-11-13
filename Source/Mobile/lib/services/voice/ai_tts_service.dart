import 'dart:async';
import 'dart:convert';
import 'dart:io';
import 'dart:typed_data';
import 'package:flutter/services.dart';
import 'package:http/http.dart' as http;
import 'package:audioplayers/audioplayers.dart';
import 'package:path_provider/path_provider.dart';
import 'package:shared_preferences/shared_preferences.dart';

enum TTSProvider {
  elevenlabs,     // Premium, most human-like
  openai,         // Good quality, natural
  azure,          // Microsoft's voices
  googleCloud,    // Google's WaveNet voices
  amazonPolly,    // Amazon's neural voices
}

class Voice {
  final String id;
  final String name;
  final String language;
  final String description;
  final TTSProvider provider;
  final bool isNeural;
  final String gender;
  final String accent;

  const Voice({
    required this.id,
    required this.name,
    required this.language,
    required this.description,
    required this.provider,
    required this.isNeural,
    required this.gender,
    required this.accent,
  });
}

class AITTSService {
  static final AITTSService _instance = AITTSService._internal();
  factory AITTSService() => _instance;
  AITTSService._internal();

  final AudioPlayer _audioPlayer = AudioPlayer();
  bool _isPlaying = false;
  bool _isInitialized = false;
  
  // Current settings
  Voice? _selectedVoice;
  double _speechRate = 1.0;
  double _volume = 1.0;
  double _stability = 0.75; // ElevenLabs specific
  double _clarity = 0.75;   // ElevenLabs specific
  
  // API Keys (should be stored securely in production)
  String? _elevenLabsApiKey;
  String? _openAiApiKey;
  String? _azureApiKey;
  String? _googleApiKey;
  String? _awsAccessKey;

  // Available voices with natural, human-like options
  static const List<Voice> _premiumVoices = [
    // ElevenLabs - Most Natural
    Voice(
      id: 'pNInz6obpgDQGcFmaJgB', // Adam
      name: 'Adam',
      language: 'en-US',
      description: 'Deep, warm American male voice - Premium',
      provider: TTSProvider.elevenlabs,
      isNeural: true,
      gender: 'Male',
      accent: 'American',
    ),
    Voice(
      id: 'EXAVITQu4vr4xnSDxMaL', // Bella
      name: 'Bella',
      language: 'en-US', 
      description: 'Soft, gentle American female voice - Premium',
      provider: TTSProvider.elevenlabs,
      isNeural: true,
      gender: 'Female',
      accent: 'American',
    ),
    Voice(
      id: 'MF3mGyEYCl7XYWbV9V6O', // Elli
      name: 'Elli',
      language: 'en-US',
      description: 'Young, energetic American female voice - Premium',
      provider: TTSProvider.elevenlabs,
      isNeural: true,
      gender: 'Female', 
      accent: 'American',
    ),
    Voice(
      id: 'TxGEqnHWrfWFTfGW9XjX', // Josh
      name: 'Josh',
      language: 'en-US',
      description: 'Confident, clear American male voice - Premium',
      provider: TTSProvider.elevenlabs,
      isNeural: true,
      gender: 'Male',
      accent: 'American',
    ),
    
    // OpenAI - High Quality
    Voice(
      id: 'alloy',
      name: 'Alloy',
      language: 'en-US',
      description: 'Balanced, neutral voice - High Quality',
      provider: TTSProvider.openai,
      isNeural: true,
      gender: 'Neutral',
      accent: 'American',
    ),
    Voice(
      id: 'echo',
      name: 'Echo',
      language: 'en-US',
      description: 'Clear, articulate male voice - High Quality',
      provider: TTSProvider.openai,
      isNeural: true,
      gender: 'Male',
      accent: 'American',
    ),
    Voice(
      id: 'nova',
      name: 'Nova',
      language: 'en-US',
      description: 'Warm, friendly female voice - High Quality',
      provider: TTSProvider.openai,
      isNeural: true,
      gender: 'Female',
      accent: 'American',
    ),
    
    // Azure Neural Voices - Good Quality
    Voice(
      id: 'en-US-JennyNeural',
      name: 'Jenny Neural',
      language: 'en-US',
      description: 'Professional, clear female voice - Neural',
      provider: TTSProvider.azure,
      isNeural: true,
      gender: 'Female',
      accent: 'American',
    ),
    Voice(
      id: 'en-US-GuyNeural',
      name: 'Guy Neural',
      language: 'en-US',
      description: 'Confident, professional male voice - Neural',
      provider: TTSProvider.azure,
      isNeural: true,
      gender: 'Male',
      accent: 'American',
    ),
  ];

  // Getters
  bool get isPlaying => _isPlaying;
  bool get isInitialized => _isInitialized;
  List<Voice> get availableVoices => _premiumVoices;
  Voice? get selectedVoice => _selectedVoice;
  double get speechRate => _speechRate;
  double get volume => _volume;
  double get stability => _stability;
  double get clarity => _clarity;

  Future<void> initialize() async {
    if (_isInitialized) return;

    try {
      // Load API keys from secure storage (implement as needed)
      await _loadApiKeys();
      
      // Set default voice to ElevenLabs Adam if available
      _selectedVoice = _premiumVoices.first;
      
      _isInitialized = true;
      print('AI TTS Service initialized successfully');
    } catch (e) {
      print('Error initializing AI TTS Service: $e');
      _isInitialized = false;
    }
  }

  Future<void> _loadApiKeys() async {
    try {
      final prefs = await SharedPreferences.getInstance();
      _elevenLabsApiKey = prefs.getString('tts_elevenlabs_api_key') ?? '';
      _openAiApiKey = prefs.getString('tts_openai_api_key') ?? '';
      _azureApiKey = prefs.getString('tts_azure_api_key') ?? '';
      
      print('Loaded API keys from preferences (ElevenLabs: ${_elevenLabsApiKey?.isNotEmpty}, OpenAI: ${_openAiApiKey?.isNotEmpty}, Azure: ${_azureApiKey?.isNotEmpty})');
    } catch (e) {
      print('Error loading API keys: $e');
      _elevenLabsApiKey = '';
      _openAiApiKey = '';
      _azureApiKey = '';
    }
  }

  Future<bool> speak(String text) async {
    if (!_isInitialized || _selectedVoice == null) {
      print('TTS not initialized or no voice selected');
      return false;
    }

    if (_isPlaying) {
      await stop();
    }

    try {
      _isPlaying = true;
      
      switch (_selectedVoice!.provider) {
        case TTSProvider.elevenlabs:
          return await _speakWithElevenLabs(text);
        case TTSProvider.openai:
          return await _speakWithOpenAI(text);
        case TTSProvider.azure:
          return await _speakWithAzure(text);
        default:
          print('Provider not implemented: ${_selectedVoice!.provider}');
          _isPlaying = false;
          return false;
      }
    } catch (e) {
      print('Error in speak: $e');
      _isPlaying = false;
      return false;
    }
  }

  Future<bool> _speakWithElevenLabs(String text) async {
    if (_elevenLabsApiKey == null || _elevenLabsApiKey!.isEmpty) {
      print('ElevenLabs API key not set');
      _isPlaying = false;
      return false;
    }

    try {
      final url = 'https://api.elevenlabs.io/v1/text-to-speech/${_selectedVoice!.id}';
      
      final response = await http.post(
        Uri.parse(url),
        headers: {
          'Accept': 'audio/mpeg',
          'Content-Type': 'application/json',
          'xi-api-key': _elevenLabsApiKey!,
        },
        body: jsonEncode({
          'text': text,
          'model_id': 'eleven_monolingual_v1',
          'voice_settings': {
            'stability': _stability,
            'similarity_boost': _clarity,
            'style': 0.0,
            'use_speaker_boost': true,
          },
        }),
      );

      if (response.statusCode == 200) {
        return await _playAudioFromBytes(response.bodyBytes);
      } else {
        print('ElevenLabs API error: ${response.statusCode} - ${response.body}');
        _isPlaying = false;
        return false;
      }
    } catch (e) {
      print('Error with ElevenLabs: $e');
      _isPlaying = false;
      return false;
    }
  }

  Future<bool> _speakWithOpenAI(String text) async {
    if (_openAiApiKey == null || _openAiApiKey!.isEmpty) {
      print('OpenAI API key not set');
      _isPlaying = false;
      return false;
    }

    try {
      final url = 'https://api.openai.com/v1/audio/speech';
      
      final response = await http.post(
        Uri.parse(url),
        headers: {
          'Authorization': 'Bearer $_openAiApiKey',
          'Content-Type': 'application/json',
        },
        body: jsonEncode({
          'model': 'tts-1-hd', // High quality model
          'input': text,
          'voice': _selectedVoice!.id,
          'speed': _speechRate,
        }),
      );

      if (response.statusCode == 200) {
        return await _playAudioFromBytes(response.bodyBytes);
      } else {
        print('OpenAI API error: ${response.statusCode} - ${response.body}');
        _isPlaying = false;
        return false;
      }
    } catch (e) {
      print('Error with OpenAI: $e');
      _isPlaying = false;
      return false;
    }
  }

  Future<bool> _speakWithAzure(String text) async {
    // Azure implementation would go here
    print('Azure TTS not implemented yet');
    _isPlaying = false;
    return false;
  }

  Future<bool> _playAudioFromBytes(Uint8List audioBytes) async {
    try {
      // Save audio to temporary file
      final tempDir = await getTemporaryDirectory();
      final audioFile = File('${tempDir.path}/tts_audio_${DateTime.now().millisecondsSinceEpoch}.mp3');
      await audioFile.writeAsBytes(audioBytes);

      // Play the audio
      await _audioPlayer.play(DeviceFileSource(audioFile.path));
      
      // Set up completion handler
      _audioPlayer.onPlayerComplete.listen((_) {
        _isPlaying = false;
        // Clean up temp file
        audioFile.delete().catchError((e) => print('Error deleting temp file: $e'));
      });

      return true;
    } catch (e) {
      print('Error playing audio: $e');
      _isPlaying = false;
      return false;
    }
  }

  Future<void> stop() async {
    if (_isPlaying) {
      await _audioPlayer.stop();
      _isPlaying = false;
    }
  }

  Future<void> pause() async {
    if (_isPlaying) {
      await _audioPlayer.pause();
    }
  }

  Future<void> resume() async {
    await _audioPlayer.resume();
  }

  // Voice and settings management
  void setVoice(Voice voice) {
    _selectedVoice = voice;
  }

  void setSpeechRate(double rate) {
    _speechRate = rate.clamp(0.25, 4.0);
  }

  void setVolume(double volume) {
    _volume = volume.clamp(0.0, 1.0);
    _audioPlayer.setVolume(_volume);
  }

  void setStability(double stability) {
    _stability = stability.clamp(0.0, 1.0);
  }

  void setClarity(double clarity) {
    _clarity = clarity.clamp(0.0, 1.0);
  }

  // API Key management with persistent storage
  Future<void> setElevenLabsApiKey(String key) async {
    _elevenLabsApiKey = key;
    try {
      final prefs = await SharedPreferences.getInstance();
      await prefs.setString('tts_elevenlabs_api_key', key);
    } catch (e) {
      print('Error saving ElevenLabs API key: $e');
    }
  }

  Future<void> setOpenAIApiKey(String key) async {
    _openAiApiKey = key;
    try {
      final prefs = await SharedPreferences.getInstance();
      await prefs.setString('tts_openai_api_key', key);
    } catch (e) {
      print('Error saving OpenAI API key: $e');
    }
  }

  Future<void> setAzureApiKey(String key) async {
    _azureApiKey = key;
    try {
      final prefs = await SharedPreferences.getInstance();
      await prefs.setString('tts_azure_api_key', key);
    } catch (e) {
      print('Error saving Azure API key: $e');
    }
  }
  
  // Getter methods for API key status
  bool get hasElevenLabsKey => _elevenLabsApiKey?.isNotEmpty ?? false;
  bool get hasOpenAIKey => _openAiApiKey?.isNotEmpty ?? false;
  bool get hasAzureKey => _azureApiKey?.isNotEmpty ?? false;

  String getVoiceQuality(Voice voice) {
    switch (voice.provider) {
      case TTSProvider.elevenlabs:
        return 'Human + AI Hybrid';
      case TTSProvider.openai:
        return 'Premium Neural';
      case TTSProvider.azure:
        return 'Neural Enhanced';
      default:
        return 'Standard';
    }
  }

  void dispose() {
    _audioPlayer.dispose();
  }
}