import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:manifestation_app/constants/colors.dart';
import 'package:manifestation_app/models/meditation_models.dart';
import 'package:manifestation_app/components/visual_selector.dart';
import 'package:manifestation_app/components/animated_candle.dart';
import 'package:manifestation_app/components/animated_moon.dart';
import 'package:manifestation_app/components/animated_nature.dart';
import 'package:manifestation_app/components/animated_geometric.dart';
import 'package:manifestation_app/components/animated_minimal.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:manifestation_app/services/meditation_session_service.dart';

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
  DateTime? sessionStartTime;
  int actualSessionDuration = 0;
  
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
  
  final List<int> durations = [5, 10, 15, 20, 30];
  
  // SharedPreferences methods
  Future<void> _loadPreferences() async {
    final prefs = await SharedPreferences.getInstance();
    
    setState(() {
      // Load visual type
      final visualTypeIndex = prefs.getInt('visual_type') ?? 0;
      selectedVisualType = VisualType.values[visualTypeIndex];
      
      // Load visual customizations
      final candleColorIndex = prefs.getInt('candle_color') ?? 0;
      selectedCandleColor = CandleColor.values[candleColorIndex];
      
      final moonPhaseIndex = prefs.getInt('moon_phase') ?? 0;
      selectedMoonPhase = MoonPhase.values[moonPhaseIndex];
      
      final naturePatternIndex = prefs.getInt('nature_pattern') ?? 0;
      selectedNaturePattern = NaturePattern.values[naturePatternIndex];
      
      final geometricPatternIndex = prefs.getInt('geometric_pattern') ?? 0;
      selectedGeometricPattern = GeometricPattern.values[geometricPatternIndex];
      
      // Load intensity and size
      visualIntensity = prefs.getDouble('visual_intensity') ?? 0.8;
      visualSize = prefs.getDouble('visual_size') ?? 200.0;
      
      // Load duration
      selectedDuration = prefs.getInt('duration') ?? 10;
      remainingSeconds = selectedDuration * 60;
    });
  }
  
  Future<void> _savePreferences() async {
    final prefs = await SharedPreferences.getInstance();
    
    // Save visual type
    await prefs.setInt('visual_type', selectedVisualType.index);
    
    // Save visual customizations
    await prefs.setInt('candle_color', selectedCandleColor.index);
    await prefs.setInt('moon_phase', selectedMoonPhase.index);
    await prefs.setInt('nature_pattern', selectedNaturePattern.index);
    await prefs.setInt('geometric_pattern', selectedGeometricPattern.index);
    
    // Save intensity and size
    await prefs.setDouble('visual_intensity', visualIntensity);
    await prefs.setDouble('visual_size', visualSize);
    
    // Save duration
    await prefs.setInt('duration', selectedDuration);
  }
  
  // Save meditation session
  Future<void> _saveSession() async {
    if (sessionStartTime == null) {
      print('Session not saved: no start time');
      return;
    }
    
    print('Attempting to save meditation session: ${actualSessionDuration}s duration');
    
    try {
      final success = await MeditationSessionService.instance.createMeditationSession(
        sessionDate: sessionStartTime!,
        timeSpentInSeconds: actualSessionDuration,
        meditationFileId: _getMeditationFileId(),
        addToFeed: false, // User can choose to share later
      );
      
      if (success) {
        print('Meditation session saved successfully: ${actualSessionDuration}s at ${sessionStartTime!.toIso8601String()}');
      } else {
        print('Failed to save meditation session');
      }
    } catch (e) {
      print('Error saving meditation session: $e');
    }
  }
  
  // Get meditation file ID based on selected visual type
  int _getMeditationFileId() {
    // This should map to actual meditation file IDs from your backend
    // For now, using visual type index as a placeholder
    switch (selectedVisualType) {
      case VisualType.candle:
        return 1;
      case VisualType.moon:
        return 2;
      case VisualType.nature:
        return 3;
      case VisualType.geometric:
        return 4;
      case VisualType.minimal:
        return 5;
      default:
        return 1;
    }
  }

  @override
  void initState() {
    super.initState();
    _initializeControllers();
    remainingSeconds = selectedDuration * 60;
    _loadPreferences();
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
    _savePreferences();
    super.dispose();
  }

  void _startSession() {
    setState(() {
      isSessionActive = true;
      isPlaying = true;
      remainingSeconds = selectedDuration * 60;
      sessionStartTime = DateTime.now();
      actualSessionDuration = 0;
    });
    
    _backgroundController.forward();
    _startTimer();
  }

  void _startTimer() {
    _sessionController.repeat();
    
    Future.delayed(Duration(seconds: 1), () {
      if (isPlaying && remainingSeconds > 0) {
        setState(() {
          remainingSeconds--;
          actualSessionDuration++;
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
  }

  void _resumeSession() {
    setState(() {
      isPlaying = true;
    });
    _startTimer();
  }

  void _endSession() {
    setState(() {
      isSessionActive = false;
      isPlaying = false;
    });
    
    _sessionController.stop();
    _backgroundController.reverse();
    
    // Save session data
    _saveSession();
    
    // Save preferences before showing completion dialog
    _savePreferences();
    
    // Show completion dialog
    _showCompletionDialog();
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
              'Wonderful! You\'ve completed your meditation session.',
              style: TextStyle(
                color: AppColors.textMedium,
                fontSize: 16.sp,
              ),
            ),
            SizedBox(height: 12.h),
            Container(
              padding: EdgeInsets.symmetric(horizontal: 12.w, vertical: 8.h),
              decoration: BoxDecoration(
                color: AppColors.visualMeditationGradient[0].withOpacity(0.1),
                borderRadius: BorderRadius.circular(8.r),
              ),
              child: Row(
                mainAxisSize: MainAxisSize.min,
                children: [
                  Icon(
                    Icons.timer,
                    color: AppColors.visualMeditationGradient[0],
                    size: 16.sp,
                  ),
                  SizedBox(width: 6.w),
                  Text(
                    'Duration: ${(actualSessionDuration / 60).floor()}m ${actualSessionDuration % 60}s',
                    style: TextStyle(
                      color: AppColors.visualMeditationGradient[0],
                      fontSize: 14.sp,
                      fontWeight: FontWeight.w500,
                    ),
                  ),
                ],
              ),
            ),
            SizedBox(height: 8.h),
            Text(
              'Session saved to your progress!',
              style: TextStyle(
                color: AppColors.textMedium,
                fontSize: 12.sp,
                fontStyle: FontStyle.italic,
              ),
            ),
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
            _savePreferences();
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
                  _savePreferences();
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
              _savePreferences();
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
              _savePreferences();
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
                  _savePreferences();
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
                  _savePreferences();
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
                  _savePreferences();
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
                  _savePreferences();
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