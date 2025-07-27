import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:shared_preferences/shared_preferences.dart';
import '../constants/colors.dart';
import '../models/meditation_models.dart';
import '../services/usage_analytics_service.dart';
import '../services/guided_tour_service.dart';
import '../models/analytics_models.dart';
import '../components/visual_selector.dart';
import '../components/animated_candle.dart';
import '../components/animated_moon.dart';
import '../components/animated_nature.dart';
import '../components/animated_geometric.dart';
import '../components/animated_minimal.dart';

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
  
  // Controllers
  late AnimationController _sessionController;
  late AnimationController _backgroundController;
  late Animation<double> _backgroundFade;
  
  // Session tracking with UsageAnalyticsService
  final UsageAnalyticsService _analyticsService = UsageAnalyticsService();
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
      print('📊 Visual meditation analytics initialized');
    } catch (e) {
      print('❌ Error initializing visual meditation analytics: $e');
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
    _sessionController.dispose();
    _backgroundController.dispose();
    
    // End session tracking if active
    if (_sessionStarted) {
      _endSessionTracking();
    }
    
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
      });
    } catch (e) {
      print('Error loading visual meditation settings: $e');
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
    } catch (e) {
      print('Error saving visual meditation settings: $e');
    }
  }

  void _startSession() async {
    try {
      print('📊 Starting visual meditation session');
      
      // Start analytics tracking
      await _startSessionTracking();
      
      setState(() {
        isSessionActive = true;
        isPlaying = true;
        remainingSeconds = selectedDuration * 60;
        _sessionStartTime = DateTime.now();
      });
      
      _backgroundController.forward();
      _startTimer();
      
      print('📊 Visual meditation session started');
    } catch (e) {
      print('❌ Error starting meditation session: $e');
      // Continue without tracking if there's an error
      setState(() {
        isSessionActive = true;
        isPlaying = true;
        remainingSeconds = selectedDuration * 60;
        _sessionStartTime = DateTime.now();
      });
      
      _backgroundController.forward();
      _startTimer();
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
      print('📊 Visual meditation session tracking started');
    } catch (e) {
      print('❌ Error starting visual meditation session tracking: $e');
    }
  }

  void _startTimer() {
    _sessionController.repeat();
    
    Future.delayed(Duration(seconds: 1), () {
      if (isPlaying && remainingSeconds > 0) {
        setState(() {
          remainingSeconds--;
        });
        _startTimer();
      } else if (remainingSeconds <= 0) {
        _endSession();
      }
    });
  }

  void _pauseSession() {
    setState(() {
      isPlaying = false;
    });
    _sessionController.stop();
    
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
    setState(() {
      isSessionActive = false;
      isPlaying = false;
    });
    
    _sessionController.stop();
    _backgroundController.reverse();
    
    // End session tracking
    await _endSessionTracking();
    
    print('📊 Visual meditation session completed');
    
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
        print('📊 Visual meditation session tracking ended');
      } catch (e) {
        print('❌ Error ending visual meditation session tracking: $e');
      }
    }
  }

  void _showCompletionDialog() {
    showDialog(
      context: context,
      barrierDismissible: false,
      builder: (context) => AlertDialog(
        backgroundColor: AppColors.cardBackground,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(20.r),
        ),
        title: Text(
          'Session Complete',
          style: TextStyle(
            color: AppColors.textLight,
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
                color: AppColors.textMedium,
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
                color: AppColors.visualMeditationGradient[0],
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
      backgroundColor: AppColors.backgroundDark,
      body: AnimatedBuilder(
        animation: _backgroundFade,
        builder: (context, child) {
          return Container(
            decoration: BoxDecoration(
              gradient: LinearGradient(
                begin: Alignment.topCenter,
                end: Alignment.bottomCenter,
                colors: [
                  AppColors.backgroundDark,
                  AppColors.backgroundDark.withOpacity(0.9 - (_backgroundFade.value * 0.4)),
                  Color.lerp(AppColors.backgroundDark, Colors.black, _backgroundFade.value * 0.6)!,
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
          GestureDetector(
            onTap: () => Navigator.pop(context),
            child: Container(
              width: 44.w,
              height: 44.h,
              decoration: BoxDecoration(
                color: AppColors.cardBackground,
                borderRadius: BorderRadius.circular(12.r),
                border: Border.all(
                  color: AppColors.textMedium.withOpacity(0.2),
                ),
              ),
              child: Icon(
                Icons.arrow_back,
                color: AppColors.textLight,
                size: 20.sp,
              ),
            ),
          ),
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
                    color: AppColors.textLight,
                  ),
                ),
                SizedBox(height: 4.h),
                Text(
                  'Focus your mind with guided visual elements',
                  style: TextStyle(
                    fontSize: 14.sp,
                    color: AppColors.textMedium,
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
        color: AppColors.cardBackground,
        borderRadius: BorderRadius.circular(20.r),
        border: Border.all(
          color: AppColors.textMedium.withOpacity(0.1),
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
                AppColors.backgroundDark.withOpacity(0.8),
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
            color: AppColors.textLight,
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

  Widget _buildDurationSelector() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Duration',
          style: TextStyle(
            fontSize: 18.sp,
            fontWeight: FontWeight.w600,
            color: AppColors.textLight,
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
                      colors: AppColors.visualMeditationGradient,
                    ) : null,
                    color: isSelected ? null : AppColors.cardBackground,
                    borderRadius: BorderRadius.circular(12.r),
                    border: Border.all(
                      color: isSelected 
                        ? AppColors.visualMeditationGradient[0]
                        : AppColors.textMedium.withOpacity(0.2),
                    ),
                  ),
                  child: Text(
                    '${duration}m',
                    style: TextStyle(
                      fontSize: 16.sp,
                      fontWeight: FontWeight.w600,
                      color: isSelected ? AppColors.white : AppColors.textMedium,
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
            color: AppColors.textLight,
          ),
        ),
        SizedBox(height: 16.h),
        _buildIntensitySlider(),
        SizedBox(height: 16.h),
        _buildSizeSlider(),
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
            color: AppColors.textMedium,
          ),
        ),
        SizedBox(height: 8.h),
        SliderTheme(
          data: SliderTheme.of(context).copyWith(
            activeTrackColor: AppColors.visualMeditationGradient[0],
            inactiveTrackColor: AppColors.textMedium.withOpacity(0.3),
            thumbColor: AppColors.visualMeditationGradient[0],
            overlayColor: AppColors.visualMeditationGradient[0].withOpacity(0.2),
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
            color: AppColors.textMedium,
          ),
        ),
        SizedBox(height: 8.h),
        SliderTheme(
          data: SliderTheme.of(context).copyWith(
            activeTrackColor: AppColors.visualMeditationGradient[0],
            inactiveTrackColor: AppColors.textMedium.withOpacity(0.3),
            thumbColor: AppColors.visualMeditationGradient[0],
            overlayColor: AppColors.visualMeditationGradient[0].withOpacity(0.2),
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

  Widget _buildCandleColorSelector() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Candle Color',
          style: TextStyle(
            fontSize: 14.sp,
            color: AppColors.textMedium,
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
                    color: isSelected ? AppColors.visualMeditationGradient[0] : AppColors.cardBackground,
                    borderRadius: BorderRadius.circular(8.r),
                    border: Border.all(
                      color: isSelected 
                        ? AppColors.visualMeditationGradient[0]
                        : AppColors.textMedium.withOpacity(0.2),
                    ),
                  ),
                  child: Text(
                    color.name,
                    style: TextStyle(
                      fontSize: 12.sp,
                      color: isSelected ? AppColors.white : AppColors.textMedium,
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
            color: AppColors.textMedium,
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
                    color: isSelected ? AppColors.visualMeditationGradient[0] : AppColors.cardBackground,
                    borderRadius: BorderRadius.circular(8.r),
                    border: Border.all(
                      color: isSelected 
                        ? AppColors.visualMeditationGradient[0]
                        : AppColors.textMedium.withOpacity(0.2),
                    ),
                  ),
                  child: Text(
                    phase.name,
                    style: TextStyle(
                      fontSize: 12.sp,
                      color: isSelected ? AppColors.white : AppColors.textMedium,
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
            color: AppColors.textMedium,
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
                    color: isSelected ? AppColors.visualMeditationGradient[0] : AppColors.cardBackground,
                    borderRadius: BorderRadius.circular(8.r),
                    border: Border.all(
                      color: isSelected 
                        ? AppColors.visualMeditationGradient[0]
                        : AppColors.textMedium.withOpacity(0.2),
                    ),
                  ),
                  child: Text(
                    pattern.name,
                    style: TextStyle(
                      fontSize: 12.sp,
                      color: isSelected ? AppColors.white : AppColors.textMedium,
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
            color: AppColors.textMedium,
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
                    color: isSelected ? AppColors.visualMeditationGradient[0] : AppColors.cardBackground,
                    borderRadius: BorderRadius.circular(8.r),
                    border: Border.all(
                      color: isSelected 
                        ? AppColors.visualMeditationGradient[0]
                        : AppColors.textMedium.withOpacity(0.2),
                    ),
                  ),
                  child: Text(
                    pattern.name,
                    style: TextStyle(
                      fontSize: 12.sp,
                      color: isSelected ? AppColors.white : AppColors.textMedium,
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
          colors: AppColors.visualMeditationGradient,
        ),
        borderRadius: BorderRadius.circular(16.r),
        boxShadow: [
          BoxShadow(
            color: AppColors.visualMeditationGradient[0].withOpacity(0.4),
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
                color: AppColors.white,
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
            child: Center(
              child: _buildSelectedVisual(),
            ),
          ),
          _buildSessionControls(),
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
            color: AppColors.textLight,
          ),
        ),
        SizedBox(height: 8.h),
        Text(
          '${minutes.toString().padLeft(2, '0')}:${seconds.toString().padLeft(2, '0')}',
          style: TextStyle(
            fontSize: 32.sp,
            fontWeight: FontWeight.bold,
            color: AppColors.visualMeditationGradient[0],
          ),
        ),
        SizedBox(height: 20.h),
      ],
    );
  }

  Widget _buildSessionControls() {
    return Row(
      mainAxisAlignment: MainAxisAlignment.spaceEvenly,
      children: [
        _buildControlButton(
          icon: Icons.stop,
          onTap: _endSession,
          color: Colors.red,
        ),
        _buildControlButton(
          icon: isPlaying ? Icons.pause : Icons.play_arrow,
          onTap: isPlaying ? _pauseSession : _resumeSession,
          color: AppColors.visualMeditationGradient[0],
          isPrimary: true,
        ),
      ],
    );
  }

  Widget _buildControlButton({
    required IconData icon,
    required VoidCallback onTap,
    required Color color,
    bool isPrimary = false,
  }) {
    return GestureDetector(
      onTap: onTap,
      child: Container(
        width: isPrimary ? 80.w : 60.w,
        height: isPrimary ? 80.h : 60.h,
        decoration: BoxDecoration(
          color: color,
          shape: BoxShape.circle,
          boxShadow: [
            BoxShadow(
              color: color.withOpacity(0.4),
              blurRadius: 20.r,
              offset: Offset(0, 10.h),
            ),
          ],
        ),
        child: Icon(
          icon,
          color: AppColors.white,
          size: isPrimary ? 32.sp : 24.sp,
        ),
      ),
    );
  }
}