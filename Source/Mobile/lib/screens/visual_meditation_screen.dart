import 'dart:async';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:shared_preferences/shared_preferences.dart';
import '../constants/colors.dart';
import '../constants/app_theme.dart';
import '../models/meditation_models.dart';
import '../services/usage_analytics_service.dart';
import '../services/guided_tour_service.dart';
import '../services/meditation_audio_service.dart';
import '../models/analytics_models.dart';
import '../components/visual_selector.dart';
import '../components/audio_selector.dart';
import '../components/animated_candle.dart';
import '../components/animated_moon.dart';
import '../components/animated_nature.dart';
import '../components/animated_geometric.dart';
import '../components/animated_minimal.dart';
import '../widgets/universal_back_button.dart';

class VisualMeditationScreen extends StatefulWidget {
  @override
  _VisualMeditationScreenState createState() => _VisualMeditationScreenState();
}

class _VisualMeditationScreenState extends State<VisualMeditationScreen>
    with TickerProviderStateMixin {

  // Session state
  bool isSessionActive = false;
  bool isPlaying = false;
  int selectedDuration = 10; // minutes
  int remainingSeconds = 0;
  
  // Visual settings
  VisualType selectedVisualType = VisualType.candle;
  CandleColor selectedCandleColor = CandleColor.warm;
  MoonPhase selectedMoonPhase = MoonPhase.fullMoon;
  NaturePattern selectedNaturePattern = NaturePattern.ripples;
  GeometricPattern selectedGeometricPattern = GeometricPattern.mandala;
  
  // Visual customization
  double visualIntensity = 0.8;
  double visualSize = 200.0;
  
  // Audio settings
  AudioType selectedAudioType = AudioType.none;
  double audioVolume = 0.7;
  
  // Controllers
  late AnimationController _sessionController;
  late AnimationController _backgroundController;
  late Animation<double> _backgroundFade;
  
  // Timer for meditation session
  Timer? _meditationTimer;
  
  // Session tracking with UsageAnalyticsService
  final UsageAnalyticsService _analyticsService = UsageAnalyticsService();
  final MeditationAudioService _audioService = MeditationAudioService();
  bool _sessionStarted = false;
  DateTime? _sessionStartTime;
  
  final List<int> durations = [5, 10, 15, 20, 30];

  @override
  void initState() {
    super.initState();
    _initializeControllers();
    _loadSettings();
    _initializeAnalytics();
    remainingSeconds = selectedDuration * 60;
    
    // Show guided tour after the widget is built
    WidgetsBinding.instance.addPostFrameCallback((_) {
      _showGuidedTourIfNeeded();
    });
  }
  
  Future<void> _initializeAnalytics() async {
    try {
      await _analyticsService.initialize();
      await _audioService.initialize();
    } catch (e) {
      // Silently handle initialization errors
    }
  }

  void _initializeControllers() {
    _sessionController = AnimationController(
      duration: Duration(seconds: 1),
      vsync: this,
    );
    
    _backgroundController = AnimationController(
      duration: Duration(milliseconds: 800),
      vsync: this,
    );
    
    _backgroundFade = Tween<double>(
      begin: 0.0,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _backgroundController,
      curve: Curves.easeInOut,
    ));
  }

  @override
  void dispose() {
    // Cancel timer to prevent double counting
    _meditationTimer?.cancel();
    _meditationTimer = null;

    _sessionController.dispose();
    _backgroundController.dispose();

    // End session tracking if active (but don't stop audio)
    if (_sessionStarted && isSessionActive) {
      _endSessionTracking();
    }

    // Don't dispose audio service here - let it continue playing
    // Only stop audio if user explicitly ended the session

    super.dispose();
  }

  Future<void> _showGuidedTourIfNeeded() async {
    final tourService = GuidedTourService();
    await tourService.showTourIfNeeded(
      context,
      GuidedTourService.meditationTourKey,
      tourService.getMeditationTourSteps(),
    );
  }

  Future<void> _loadSettings() async {
    try {
      final prefs = await SharedPreferences.getInstance();
      
      setState(() {
        // Load visual type
        final visualTypeIndex = prefs.getInt('visual_meditation_visual_type') ?? VisualType.candle.index;
        selectedVisualType = VisualType.values[visualTypeIndex];
        
        // Load duration
        selectedDuration = prefs.getInt('visual_meditation_duration') ?? 10;
        
        // Load intensity and size
        visualIntensity = prefs.getDouble('visual_meditation_intensity') ?? 0.8;
        visualSize = prefs.getDouble('visual_meditation_size') ?? 200.0;
        
        // Load candle color
        final candleColorIndex = prefs.getInt('visual_meditation_candle_color') ?? CandleColor.warm.index;
        selectedCandleColor = CandleColor.values[candleColorIndex];
        
        // Load moon phase
        final moonPhaseIndex = prefs.getInt('visual_meditation_moon_phase') ?? MoonPhase.fullMoon.index;
        selectedMoonPhase = MoonPhase.values[moonPhaseIndex];
        
        // Load nature pattern
        final naturePatternIndex = prefs.getInt('visual_meditation_nature_pattern') ?? NaturePattern.ripples.index;
        selectedNaturePattern = NaturePattern.values[naturePatternIndex];
        
        // Load geometric pattern
        final geometricPatternIndex = prefs.getInt('visual_meditation_geometric_pattern') ?? GeometricPattern.mandala.index;
        selectedGeometricPattern = GeometricPattern.values[geometricPatternIndex];
        
        // Load audio settings
        final audioTypeIndex = prefs.getInt('visual_meditation_audio_type') ?? AudioType.none.index;
        selectedAudioType = AudioType.values[audioTypeIndex];
        audioVolume = prefs.getDouble('visual_meditation_audio_volume') ?? 0.7;
      });
    } catch (e) {
      // Silently handle load errors - will use defaults
    }
  }

  Future<void> _saveSettings() async {
    try {
      final prefs = await SharedPreferences.getInstance();

      // Save visual type
      await prefs.setInt('visual_meditation_visual_type', selectedVisualType.index);

      // Save duration
      await prefs.setInt('visual_meditation_duration', selectedDuration);

      // Save intensity and size
      await prefs.setDouble('visual_meditation_intensity', visualIntensity);
      await prefs.setDouble('visual_meditation_size', visualSize);

      // Save candle color
      await prefs.setInt('visual_meditation_candle_color', selectedCandleColor.index);

      // Save moon phase
      await prefs.setInt('visual_meditation_moon_phase', selectedMoonPhase.index);

      // Save nature pattern
      await prefs.setInt('visual_meditation_nature_pattern', selectedNaturePattern.index);

      // Save geometric pattern
      await prefs.setInt('visual_meditation_geometric_pattern', selectedGeometricPattern.index);

      // Save audio settings
      await prefs.setInt('visual_meditation_audio_type', selectedAudioType.index);
      await prefs.setDouble('visual_meditation_audio_volume', audioVolume);
    } catch (e) {
      // Silently handle save errors
    }
  }

  void _startSession() async {
    // Set session state and start immediately
    setState(() {
      isSessionActive = true;
      isPlaying = true;
      remainingSeconds = selectedDuration * 60;
      _sessionStartTime = DateTime.now();
    });

    // Start UI animations
    _backgroundController.forward();

    // Start timer immediately - no delays, no complexity
    _startTimer();

    // Start analytics tracking (async, don't wait)
    _startSessionTracking().catchError((e) {
      // Silently handle analytics errors
    });

    // Start audio (async, don't wait)
    if (selectedAudioType != AudioType.none) {
      _audioService.playAudio(selectedAudioType, volume: audioVolume).catchError((e) {
        // Silently handle audio errors
      });
    }
  }
  
  /// Start session tracking for visual meditation
  Future<void> _startSessionTracking() async {
    try {
      _analyticsService.startSession(
        ActivityType.visualMeditation,
        metadata: {
          'visualType': selectedVisualType.name,
          'duration': selectedDuration,
          'intensity': visualIntensity,
          'size': visualSize,
          'candleColor': selectedCandleColor.name,
          'moonPhase': selectedMoonPhase.name,
          'naturePattern': selectedNaturePattern.name,
          'geometricPattern': selectedGeometricPattern.name,
        },
      );
      _sessionStarted = true;
    } catch (e) {
      // Silently handle tracking errors
    }
  }

  void _startTimer() {
    // Cancel any existing timer
    _meditationTimer?.cancel();

    // Start UI animation
    _sessionController.repeat();

    // Start countdown timer
    _meditationTimer = Timer.periodic(Duration(seconds: 1), (timer) {
      if (!mounted) {
        timer.cancel();
        _meditationTimer = null;
        return;
      }

      if (remainingSeconds > 0) {
        setState(() {
          remainingSeconds--;
        });
      } else {
        timer.cancel();
        _meditationTimer = null;
        _endSession();
      }
    });
  }

  void _pauseSession() {
    // Cancel timer first to prevent double counting
    _meditationTimer?.cancel();
    _meditationTimer = null;

    setState(() {
      isPlaying = false;
    });
    _sessionController.stop();

    // Pause audio
    _audioService.pauseAudio();

    // Update session metadata
    if (_sessionStarted) {
      _analyticsService.updateSessionMetadata(
        ActivityType.visualMeditation,
        {
          'paused': true,
          'pauseTime': DateTime.now().toIso8601String(),
          'remainingSeconds': remainingSeconds,
        },
      );
    }
  }

  void _resumeSession() {
    setState(() {
      isPlaying = true;
    });

    // Resume audio
    _audioService.resumeAudio();

    // Start timer again
    _startTimer();

    // Update session metadata
    if (_sessionStarted) {
      _analyticsService.updateSessionMetadata(
        ActivityType.visualMeditation,
        {
          'paused': false,
          'resumeTime': DateTime.now().toIso8601String(),
          'remainingSeconds': remainingSeconds,
        },
      );
    }
  }

  void _endSession() async {
    if (!isSessionActive) {
      return;
    }

    // Cancel timer immediately to prevent double counting
    _meditationTimer?.cancel();
    _meditationTimer = null;

    setState(() {
      isSessionActive = false;
      isPlaying = false;
    });

    _sessionController.stop();
    _backgroundController.reverse();

    // End meditation session audio properly
    await _audioService.endMeditationSession();

    // End session tracking
    await _endSessionTracking();

    // Show completion dialog
    _showCompletionDialog();
  }

  /// End session tracking for visual meditation
  Future<void> _endSessionTracking() async {
    if (_sessionStarted) {
      try {
        final actualDuration = _sessionStartTime != null
            ? DateTime.now().difference(_sessionStartTime!).inSeconds
            : selectedDuration * 60;

        await _analyticsService.endSession(
          ActivityType.visualMeditation,
          additionalMetadata: {
            'completed': remainingSeconds <= 0,
            'actualDuration': actualDuration,
            'plannedDuration': selectedDuration * 60,
            'completionPercentage': ((selectedDuration * 60 - remainingSeconds) / (selectedDuration * 60) * 100).round(),
          },
        );
        _sessionStarted = false;
      } catch (e) {
        // Silently handle tracking errors
      }
    }
  }

  void _showCompletionDialog() {
    showDialog(
      context: context,
      barrierDismissible: false,
      builder: (context) => AlertDialog(
        backgroundColor: AppTheme.cardBackground,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(20.r),
        ),
        title: Text(
          'Session Complete',
          style: TextStyle(
            color: AppTheme.textLight,
            fontSize: 20.sp,
            fontWeight: FontWeight.bold,
          ),
        ),
        content: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            Text(
              'Wonderful! You\'ve completed your ${selectedDuration}-minute visual meditation session.',
              style: TextStyle(
                color: AppTheme.textMedium,
                fontSize: 16.sp,
              ),
            ),
            // Session tracking info removed
          ],
        ),
        actions: [
          TextButton(
            onPressed: () {
              Navigator.pop(context);
              Navigator.pop(context);
            },
            child: Text(
              'Done',
              style: TextStyle(
                color: AppTheme.visualMeditationGradient[0],
                fontSize: 16.sp,
                fontWeight: FontWeight.w600,
              ),
            ),
          ),
        ],
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppTheme.backgroundDark,
      body: AnimatedBuilder(
        animation: _backgroundFade,
        builder: (context, child) {
          return Container(
            decoration: BoxDecoration(
              gradient: LinearGradient(
                begin: Alignment.topCenter,
                end: Alignment.bottomCenter,
                colors: [
                  AppTheme.backgroundDark,
                  AppTheme.backgroundDark.withOpacity(0.9 - (_backgroundFade.value * 0.4)),
                  Color.lerp(AppTheme.backgroundDark, Colors.black, _backgroundFade.value * 0.6)!,
                ],
              ),
            ),
            child: SafeArea(
              child: isSessionActive ? _buildActiveSession() : _buildSetupScreen(),
            ),
          );
        },
      ),
    );
  }

  Widget _buildSetupScreen() {
    return SingleChildScrollView(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          _buildHeader(),
          Padding(
            padding: EdgeInsets.symmetric(horizontal: 20.w),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                SizedBox(height: 10.h),
          _buildVisualPreview(),
          SizedBox(height: 30.h),
          _buildVisualSelector(),
          SizedBox(height: 30.h),
          _buildAudioSelector(),
          SizedBox(height: 30.h),
          _buildDurationSelector(),
          SizedBox(height: 30.h),
          _buildCustomizationOptions(),
          SizedBox(height: 40.h),
          _buildStartButton(),
          SizedBox(height: 20.h),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildHeader() {
    return Padding(
      padding: EdgeInsets.all(20.w),
      child: Row(
        children: [
          UniversalBackButton(),
          SizedBox(width: 16.w),
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  'Visual Meditation',
                  style: TextStyle(
                    fontSize: 24.sp,
                    fontWeight: FontWeight.bold,
                    color: AppTheme.textLight,
                  ),
                ),
                SizedBox(height: 4.h),
                Text(
                  'Focus your mind with guided visual elements',
                  style: TextStyle(
                    fontSize: 14.sp,
                    color: AppTheme.textMedium,
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildVisualPreview() {
    return Container(
      width: double.infinity,
      height: 300.h,
      decoration: BoxDecoration(
        color: AppTheme.cardBackground,
        borderRadius: BorderRadius.circular(20.r),
        border: Border.all(
          color: AppTheme.textMedium.withOpacity(0.1),
        ),
      ),
      child: ClipRRect(
        borderRadius: BorderRadius.circular(20.r),
        child: Container(
          decoration: BoxDecoration(
            gradient: LinearGradient(
              begin: Alignment.topCenter,
              end: Alignment.bottomCenter,
              colors: [
                AppTheme.backgroundDark.withOpacity(0.8),
                Colors.black.withOpacity(0.9),
              ],
            ),
          ),
          child: Center(
            child: _buildSelectedVisual(previewMode: true),
          ),
        ),
      ),
    );
  }

  Widget _buildSelectedVisual({bool previewMode = false}) {
    final size = previewMode ? 120.0 : visualSize;
    final intensity = previewMode ? 0.8 : visualIntensity;
    
    switch (selectedVisualType) {
      case VisualType.candle:
        return AnimatedCandle(
          color: selectedCandleColor,
          intensity: intensity,
          size: size,
        );
      case VisualType.moon:
        return AnimatedMoon(
          phase: selectedMoonPhase,
          intensity: intensity,
          size: size,
        );
      case VisualType.nature:
        return AnimatedNature(
          pattern: selectedNaturePattern,
          intensity: intensity,
          size: size,
        );
      case VisualType.geometric:
        return AnimatedGeometric(
          pattern: selectedGeometricPattern,
          intensity: intensity,
          size: size,
        );
      case VisualType.minimal:
        return AnimatedMinimal(
          intensity: intensity,
          size: size,
        );
    }
  }

  Widget _buildVisualSelector() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Choose Your Visual',
          style: TextStyle(
            fontSize: 18.sp,
            fontWeight: FontWeight.w600,
            color: AppTheme.textLight,
          ),
        ),
        SizedBox(height: 16.h),
        CompactVisualSelector(
          selectedVisual: selectedVisualType,
          onVisualChanged: (type) {
            setState(() {
              selectedVisualType = type;
            });
            _saveSettings();
          },
        ),
      ],
    );
  }

  Widget _buildAudioSelector() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Choose Your Audio',
          style: TextStyle(
            fontSize: 18.sp,
            fontWeight: FontWeight.w600,
            color: AppTheme.textLight,
          ),
        ),
        SizedBox(height: 16.h),
        CompactAudioSelector(
          selectedAudio: selectedAudioType,
          onAudioChanged: (type) {
            setState(() {
              selectedAudioType = type;
            });
            _saveSettings();
          },
        ),
      ],
    );
  }

  Widget _buildDurationSelector() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Duration',
          style: TextStyle(
            fontSize: 18.sp,
            fontWeight: FontWeight.w600,
            color: AppTheme.textLight,
          ),
        ),
        SizedBox(height: 16.h),
        SingleChildScrollView(
          scrollDirection: Axis.horizontal,
          child: Row(
            children: durations.map((duration) {
              final isSelected = selectedDuration == duration;
              return GestureDetector(
                onTap: () {
                  setState(() {
                    selectedDuration = duration;
                    remainingSeconds = duration * 60;
                  });
                  _saveSettings();
                },
                child: Container(
                  margin: EdgeInsets.only(right: 12.w),
                  padding: EdgeInsets.symmetric(horizontal: 20.w, vertical: 12.h),
                  decoration: BoxDecoration(
                    gradient: isSelected ? LinearGradient(
                      colors: AppTheme.visualMeditationGradient,
                    ) : null,
                    color: isSelected ? null : AppTheme.cardBackground,
                    borderRadius: BorderRadius.circular(12.r),
                    border: Border.all(
                      color: isSelected 
                        ? AppTheme.visualMeditationGradient[0]
                        : AppTheme.textMedium.withOpacity(0.2),
                    ),
                  ),
                  child: Text(
                    '${duration}m',
                    style: TextStyle(
                      fontSize: 16.sp,
                      fontWeight: FontWeight.w600,
                      color: isSelected ? AppTheme.white : AppTheme.textMedium,
                    ),
                  ),
                ),
              );
            }).toList(),
          ),
        ),
      ],
    );
  }

  Widget _buildCustomizationOptions() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Customization',
          style: TextStyle(
            fontSize: 18.sp,
            fontWeight: FontWeight.w600,
            color: AppTheme.textLight,
          ),
        ),
        SizedBox(height: 16.h),
        _buildIntensitySlider(),
        SizedBox(height: 16.h),
        _buildSizeSlider(),
        if (selectedAudioType != AudioType.none) ...[
          SizedBox(height: 16.h),
          _buildVolumeSlider(),
        ],
        if (selectedVisualType == VisualType.candle) ...[
          SizedBox(height: 16.h),
          _buildCandleColorSelector(),
        ],
        if (selectedVisualType == VisualType.moon) ...[
          SizedBox(height: 16.h),
          _buildMoonPhaseSelector(),
        ],
        if (selectedVisualType == VisualType.nature) ...[
          SizedBox(height: 16.h),
          _buildNaturePatternSelector(),
        ],
        if (selectedVisualType == VisualType.geometric) ...[
          SizedBox(height: 16.h),
          _buildGeometricPatternSelector(),
        ],
      ],
    );
  }

  Widget _buildIntensitySlider() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Intensity',
          style: TextStyle(
            fontSize: 14.sp,
            color: AppTheme.textMedium,
          ),
        ),
        SizedBox(height: 8.h),
        SliderTheme(
          data: SliderTheme.of(context).copyWith(
            activeTrackColor: AppTheme.visualMeditationGradient[0],
            inactiveTrackColor: AppTheme.textMedium.withOpacity(0.3),
            thumbColor: AppTheme.visualMeditationGradient[0],
            overlayColor: AppTheme.visualMeditationGradient[0].withOpacity(0.2),
          ),
          child: Slider(
            value: visualIntensity,
            min: 0.1,
            max: 1.0,
            divisions: 9,
            onChanged: (value) {
              setState(() {
                visualIntensity = value;
              });
              _saveSettings();
            },
          ),
        ),
      ],
    );
  }

  Widget _buildSizeSlider() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Size',
          style: TextStyle(
            fontSize: 14.sp,
            color: AppTheme.textMedium,
          ),
        ),
        SizedBox(height: 8.h),
        SliderTheme(
          data: SliderTheme.of(context).copyWith(
            activeTrackColor: AppTheme.visualMeditationGradient[0],
            inactiveTrackColor: AppTheme.textMedium.withOpacity(0.3),
            thumbColor: AppTheme.visualMeditationGradient[0],
            overlayColor: AppTheme.visualMeditationGradient[0].withOpacity(0.2),
          ),
          child: Slider(
            value: visualSize,
            min: 100.0,
            max: 300.0,
            divisions: 8,
            onChanged: (value) {
              setState(() {
                visualSize = value;
              });
              _saveSettings();
            },
          ),
        ),
      ],
    );
  }

  Widget _buildVolumeSlider() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Audio Volume',
          style: TextStyle(
            fontSize: 14.sp,
            color: AppTheme.textMedium,
          ),
        ),
        SizedBox(height: 8.h),
        SliderTheme(
          data: SliderTheme.of(context).copyWith(
            activeTrackColor: selectedAudioType.colors[0],
            inactiveTrackColor: AppTheme.textMedium.withOpacity(0.3),
            thumbColor: selectedAudioType.colors[0],
            overlayColor: selectedAudioType.colors[0].withOpacity(0.2),
          ),
          child: Slider(
            value: audioVolume,
            min: 0.0,
            max: 1.0,
            divisions: 10,
            onChanged: (value) {
              setState(() {
                audioVolume = value;
              });
              _saveSettings();
            },
          ),
        ),
      ],
    );
  }

  Widget _buildCandleColorSelector() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Candle Color',
          style: TextStyle(
            fontSize: 14.sp,
            color: AppTheme.textMedium,
          ),
        ),
        SizedBox(height: 8.h),
        SingleChildScrollView(
          scrollDirection: Axis.horizontal,
          child: Row(
            children: CandleColor.values.map((color) {
              final isSelected = selectedCandleColor == color;
              return GestureDetector(
                onTap: () {
                  setState(() {
                    selectedCandleColor = color;
                  });
                  _saveSettings();
                },
                child: Container(
                  margin: EdgeInsets.only(right: 12.w),
                  padding: EdgeInsets.symmetric(horizontal: 16.w, vertical: 8.h),
                  decoration: BoxDecoration(
                    color: isSelected ? AppTheme.visualMeditationGradient[0] : AppTheme.cardBackground,
                    borderRadius: BorderRadius.circular(8.r),
                    border: Border.all(
                      color: isSelected 
                        ? AppTheme.visualMeditationGradient[0]
                        : AppTheme.textMedium.withOpacity(0.2),
                    ),
                  ),
                  child: Text(
                    color.name,
                    style: TextStyle(
                      fontSize: 12.sp,
                      color: isSelected ? AppTheme.white : AppTheme.textMedium,
                    ),
                  ),
                ),
              );
            }).toList(),
          ),
        ),
      ],
    );
  }

  Widget _buildMoonPhaseSelector() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Moon Phase',
          style: TextStyle(
            fontSize: 14.sp,
            color: AppTheme.textMedium,
          ),
        ),
        SizedBox(height: 8.h),
        SingleChildScrollView(
          scrollDirection: Axis.horizontal,
          child: Row(
            children: MoonPhase.values.map((phase) {
              final isSelected = selectedMoonPhase == phase;
              return GestureDetector(
                onTap: () {
                  setState(() {
                    selectedMoonPhase = phase;
                  });
                  _saveSettings();
                },
                child: Container(
                  margin: EdgeInsets.only(right: 12.w),
                  padding: EdgeInsets.symmetric(horizontal: 16.w, vertical: 8.h),
                  decoration: BoxDecoration(
                    color: isSelected ? AppTheme.visualMeditationGradient[0] : AppTheme.cardBackground,
                    borderRadius: BorderRadius.circular(8.r),
                    border: Border.all(
                      color: isSelected 
                        ? AppTheme.visualMeditationGradient[0]
                        : AppTheme.textMedium.withOpacity(0.2),
                    ),
                  ),
                  child: Text(
                    phase.name,
                    style: TextStyle(
                      fontSize: 12.sp,
                      color: isSelected ? AppTheme.white : AppTheme.textMedium,
                    ),
                  ),
                ),
              );
            }).toList(),
          ),
        ),
      ],
    );
  }

  Widget _buildNaturePatternSelector() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Nature Pattern',
          style: TextStyle(
            fontSize: 14.sp,
            color: AppTheme.textMedium,
          ),
        ),
        SizedBox(height: 8.h),
        SingleChildScrollView(
          scrollDirection: Axis.horizontal,
          child: Row(
            children: NaturePattern.values.map((pattern) {
              final isSelected = selectedNaturePattern == pattern;
              return GestureDetector(
                onTap: () {
                  setState(() {
                    selectedNaturePattern = pattern;
                  });
                  _saveSettings();
                },
                child: Container(
                  margin: EdgeInsets.only(right: 12.w),
                  padding: EdgeInsets.symmetric(horizontal: 16.w, vertical: 8.h),
                  decoration: BoxDecoration(
                    color: isSelected ? AppTheme.visualMeditationGradient[0] : AppTheme.cardBackground,
                    borderRadius: BorderRadius.circular(8.r),
                    border: Border.all(
                      color: isSelected 
                        ? AppTheme.visualMeditationGradient[0]
                        : AppTheme.textMedium.withOpacity(0.2),
                    ),
                  ),
                  child: Text(
                    pattern.name,
                    style: TextStyle(
                      fontSize: 12.sp,
                      color: isSelected ? AppTheme.white : AppTheme.textMedium,
                    ),
                  ),
                ),
              );
            }).toList(),
          ),
        ),
      ],
    );
  }

  Widget _buildGeometricPatternSelector() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Geometric Pattern',
          style: TextStyle(
            fontSize: 14.sp,
            color: AppTheme.textMedium,
          ),
        ),
        SizedBox(height: 8.h),
        SingleChildScrollView(
          scrollDirection: Axis.horizontal,
          child: Row(
            children: GeometricPattern.values.map((pattern) {
              final isSelected = selectedGeometricPattern == pattern;
              return GestureDetector(
                onTap: () {
                  setState(() {
                    selectedGeometricPattern = pattern;
                  });
                  _saveSettings();
                },
                child: Container(
                  margin: EdgeInsets.only(right: 12.w),
                  padding: EdgeInsets.symmetric(horizontal: 16.w, vertical: 8.h),
                  decoration: BoxDecoration(
                    color: isSelected ? AppTheme.visualMeditationGradient[0] : AppTheme.cardBackground,
                    borderRadius: BorderRadius.circular(8.r),
                    border: Border.all(
                      color: isSelected 
                        ? AppTheme.visualMeditationGradient[0]
                        : AppTheme.textMedium.withOpacity(0.2),
                    ),
                  ),
                  child: Text(
                    pattern.name,
                    style: TextStyle(
                      fontSize: 12.sp,
                      color: isSelected ? AppTheme.white : AppTheme.textMedium,
                    ),
                  ),
                ),
              );
            }).toList(),
          ),
        ),
      ],
    );
  }

  Widget _buildStartButton() {
    return Container(
      width: double.infinity,
      height: 56.h,
      decoration: BoxDecoration(
        gradient: LinearGradient(
          colors: AppTheme.visualMeditationGradient,
        ),
        borderRadius: BorderRadius.circular(16.r),
        boxShadow: [
          BoxShadow(
            color: AppTheme.visualMeditationGradient[0].withOpacity(0.4),
            blurRadius: 20.r,
            offset: Offset(0, 10.h),
          ),
        ],
      ),
      child: Material(
        color: Colors.transparent,
        child: InkWell(
          onTap: _startSession,
          borderRadius: BorderRadius.circular(16.r),
          child: Center(
            child: Text(
              'Begin Meditation',
              style: TextStyle(
                fontSize: 18.sp,
                fontWeight: FontWeight.w600,
                color: AppTheme.white,
              ),
            ),
          ),
        ),
      ),
    );
  }

  Widget _buildActiveSession() {
    return Container(
      padding: EdgeInsets.all(20.w),
      child: Column(
        children: [
          _buildSessionHeader(),
          Expanded(
            child: Container(
              padding: EdgeInsets.only(bottom: 30.h),
              child: Center(
                child: ConstrainedBox(
                  constraints: BoxConstraints(
                    maxHeight: MediaQuery.of(context).size.height * 0.6,
                    maxWidth: MediaQuery.of(context).size.width * 0.8,
                  ),
                  child: GestureDetector(
                    onTap: () {
                      HapticFeedback.mediumImpact();
                      // Simple pause/resume toggle
                      if (isPlaying) {
                        _pauseSession();
                      } else if (isSessionActive) {
                        _resumeSession();
                      }
                    },
                    onLongPress: () {
                      HapticFeedback.heavyImpact();
                      // Long press to show options
                      _showSessionOptions();
                    },
                    child: _buildSelectedVisual(),
                  ),
                ),
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildSessionHeader() {
    final minutes = remainingSeconds ~/ 60;
    final seconds = remainingSeconds % 60;
    
    return Column(
      children: [
        Text(
          selectedVisualType.name,
          style: TextStyle(
            fontSize: 24.sp,
            fontWeight: FontWeight.w600,
            color: AppTheme.textLight,
          ),
        ),
        SizedBox(height: 8.h),
        Text(
          '${minutes.toString().padLeft(2, '0')}:${seconds.toString().padLeft(2, '0')}',
          style: TextStyle(
            fontSize: 32.sp,
            fontWeight: FontWeight.bold,
            color: AppTheme.visualMeditationGradient[0],
          ),
        ),
        SizedBox(height: 12.h),
        if (selectedAudioType != AudioType.none)
          Row(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              Icon(
                selectedAudioType.icon,
                size: 16.sp,
                color: selectedAudioType.colors[0],
              ),
              SizedBox(width: 8.w),
              Text(
                selectedAudioType.name,
                style: TextStyle(
                  fontSize: 14.sp,
                  color: selectedAudioType.colors[0],
                  fontWeight: FontWeight.w500,
                ),
              ),
            ],
          )
        else
          Text(
            'Silent Session',
            style: TextStyle(
              fontSize: 14.sp,
              color: AppTheme.textMedium.withOpacity(0.7),
            ),
          ),
        SizedBox(height: 20.h),
      ],
    );
  }

  void _showSessionOptions() {
    showModalBottomSheet(
      context: context,
      backgroundColor: Colors.transparent,
      builder: (context) => Container(
        decoration: BoxDecoration(
          color: AppTheme.cardBackground,
          borderRadius: BorderRadius.vertical(top: Radius.circular(20.r)),
        ),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            // Handle bar
            Container(
              width: 40.w,
              height: 4.h,
              margin: EdgeInsets.only(top: 12.h, bottom: 20.h),
              decoration: BoxDecoration(
                color: AppTheme.textMedium.withOpacity(0.3),
                borderRadius: BorderRadius.circular(2.r),
              ),
            ),
            
            // Session status
            Padding(
              padding: EdgeInsets.symmetric(horizontal: 20.w),
              child: Text(
                isPlaying ? 'Meditation In Progress' : 'Meditation Paused',
                style: TextStyle(
                  fontSize: 18.sp,
                  fontWeight: FontWeight.w600,
                  color: AppTheme.textLight,
                ),
              ),
            ),
            
            SizedBox(height: 20.h),
            
            // Audio control
            _buildOptionTile(
              icon: selectedAudioType.icon,
              title: 'Change Audio',
              subtitle: selectedAudioType.name,
              onTap: () {
                Navigator.pop(context);
                _showAudioSelector();
              },
            ),
            
            // Pause/Resume
            _buildOptionTile(
              icon: isPlaying ? Icons.pause : Icons.play_arrow,
              title: isPlaying ? 'Pause Session' : 'Resume Session',
              subtitle: isPlaying ? 'Temporarily stop the session' : 'Continue your meditation',
              onTap: () {
                Navigator.pop(context);
                if (isPlaying) {
                  _pauseSession();
                } else if (isSessionActive) {
                  _resumeSession();
                }
              },
            ),
            
            // End session
            _buildOptionTile(
              icon: Icons.stop,
              title: 'End Session',
              subtitle: 'Complete your meditation now',
              onTap: () {
                Navigator.pop(context);
                _showEndConfirmation();
              },
              isDestructive: true,
            ),
            
            SizedBox(height: 20.h),
          ],
        ),
      ),
    );
  }

  Widget _buildOptionTile({
    required IconData icon,
    required String title,
    required String subtitle,
    required VoidCallback onTap,
    bool isDestructive = false,
  }) {
    return InkWell(
      onTap: onTap,
      child: Padding(
        padding: EdgeInsets.symmetric(horizontal: 20.w, vertical: 16.h),
        child: Row(
          children: [
            Container(
              width: 48.w,
              height: 48.h,
              decoration: BoxDecoration(
                color: isDestructive 
                  ? Colors.red.withOpacity(0.1)
                  : AppTheme.visualMeditationGradient[0].withOpacity(0.1),
                borderRadius: BorderRadius.circular(12.r),
              ),
              child: Icon(
                icon,
                color: isDestructive ? Colors.red : AppTheme.visualMeditationGradient[0],
                size: 24.sp,
              ),
            ),
            SizedBox(width: 16.w),
            Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    title,
                    style: TextStyle(
                      fontSize: 16.sp,
                      fontWeight: FontWeight.w600,
                      color: AppTheme.textLight,
                    ),
                  ),
                  SizedBox(height: 2.h),
                  Text(
                    subtitle,
                    style: TextStyle(
                      fontSize: 12.sp,
                      color: AppTheme.textMedium,
                    ),
                  ),
                ],
              ),
            ),
            Icon(
              Icons.arrow_forward_ios,
              size: 16.sp,
              color: AppTheme.textMedium.withOpacity(0.5),
            ),
          ],
        ),
      ),
    );
  }

  void _showAudioSelector() {
    showModalBottomSheet(
      context: context,
      backgroundColor: Colors.transparent,
      builder: (context) => Container(
        height: 300.h,
        decoration: BoxDecoration(
          color: AppTheme.cardBackground,
          borderRadius: BorderRadius.vertical(top: Radius.circular(20.r)),
        ),
        child: Column(
          children: [
            // Handle bar
            Container(
              width: 40.w,
              height: 4.h,
              margin: EdgeInsets.only(top: 12.h, bottom: 20.h),
              decoration: BoxDecoration(
                color: AppTheme.textMedium.withOpacity(0.3),
                borderRadius: BorderRadius.circular(2.r),
              ),
            ),
            
            Text(
              'Change Audio',
              style: TextStyle(
                fontSize: 18.sp,
                fontWeight: FontWeight.w600,
                color: AppTheme.textLight,
              ),
            ),
            
            SizedBox(height: 20.h),
            
            Expanded(
              child: Padding(
                padding: EdgeInsets.symmetric(horizontal: 20.w),
                child: CompactAudioSelector(
                  selectedAudio: selectedAudioType,
                  onAudioChanged: (audioType) {
                    setState(() {
                      selectedAudioType = audioType;
                    });
                    
                    // Change audio during session
                    if (audioType != AudioType.none) {
                      _audioService.playAudio(audioType, volume: audioVolume);
                    } else {
                      _audioService.stopAudio();
                    }
                    
                    _saveSettings();
                    Navigator.pop(context);
                  },
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }

  void _showEndConfirmation() {
    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        backgroundColor: AppTheme.cardBackground,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(20.r),
        ),
        title: Text(
          'End Session?',
          style: TextStyle(
            color: AppTheme.textLight,
            fontSize: 18.sp,
            fontWeight: FontWeight.bold,
          ),
        ),
        content: Text(
          'Are you sure you want to end your meditation session now?',
          style: TextStyle(
            color: AppTheme.textMedium,
            fontSize: 14.sp,
          ),
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context),
            child: Text(
              'Continue',
              style: TextStyle(
                color: AppTheme.textMedium,
                fontSize: 14.sp,
              ),
            ),
          ),
          TextButton(
            onPressed: () {
              Navigator.pop(context);
              _endSession();
            },
            child: Text(
              'End Session',
              style: TextStyle(
                color: Colors.red,
                fontSize: 14.sp,
                fontWeight: FontWeight.w600,
              ),
            ),
          ),
        ],
      ),
    );
  }
}