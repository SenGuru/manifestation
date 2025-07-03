import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:manifestation_app/animated_candle.dart';
import 'package:manifestation_app/animated_geometric.dart';
import 'package:manifestation_app/animated_minimal.dart';
import 'package:manifestation_app/animated_moon.dart';
import 'package:manifestation_app/animated_nature.dart';
import 'package:manifestation_app/breathing_guide.dart';
import 'package:manifestation_app/colors.dart';
import 'package:manifestation_app/meditation_constants.dart';
import 'package:manifestation_app/meditation_models.dart';
import 'package:manifestation_app/meditation_timer.dart';
import 'package:manifestation_app/visual_selector.dart';

class MeditationScreen extends StatefulWidget {
  @override
  _MeditationScreenState createState() => _MeditationScreenState();
}

class _MeditationScreenState extends State<MeditationScreen>
    with TickerProviderStateMixin {
  // Session state
  bool isSessionActive = false;
  bool isPlaying = false;
  bool showBreathingGuide = false;
  
  // Selected options
  int selectedDuration = 10;
  MeditationType selectedMeditationType = MeditationType.manifestation;
  VisualType selectedVisualType = VisualType.candle;
  
  // Visual-specific settings
  CandleColor selectedCandleColor = CandleColor.warm;
  MoonPhase selectedMoonPhase = MoonPhase.fullMoon;
  NaturePattern selectedNaturePattern = NaturePattern.ripples;
  GeometricPattern selectedGeometricPattern = GeometricPattern.mandala;
  
  // Animation controllers
  late AnimationController _sessionController;
  late AnimationController _breathingController;
  late AnimationController _setupController;
  
  // Session tracking
  int remainingSeconds = 0;
  SessionPhase currentPhase = SessionPhase.preparation;
  
  @override
  void initState() {
    super.initState();
    
    _sessionController = AnimationController(
      vsync: this,
      duration: Duration(minutes: selectedDuration),
    );
    
    _breathingController = AnimationController(
      vsync: this,
      duration: AppConstants.breathingCycle,
    );
    
    _setupController = AnimationController(
      vsync: this,
      duration: AppConstants.normalAnimation,
    );
    
    _sessionController.addListener(_onSessionProgress);
    _sessionController.addStatusListener(_onSessionStatusChange);
  }

  void _onSessionProgress() {
    if (mounted) {
      setState(() {
        remainingSeconds = ((1 - _sessionController.value) * selectedDuration * 60).round();
        _updateSessionPhase();
      });
    }
  }

  void _onSessionStatusChange(AnimationStatus status) {
    if (status == AnimationStatus.completed) {
      _endSession();
    }
  }

  void _updateSessionPhase() {
    final progress = _sessionController.value;
    SessionPhase newPhase;
    
    if (progress < 0.1) {
      newPhase = SessionPhase.beginning;
    } else if (progress < 0.3) {
      newPhase = SessionPhase.deepening;
    } else if (progress < 0.7) {
      newPhase = SessionPhase.peak;
    } else if (progress < 0.9) {
      newPhase = SessionPhase.integration;
    } else {
      newPhase = SessionPhase.completion;
    }
    
    if (newPhase != currentPhase) {
      setState(() {
        currentPhase = newPhase;
      });
    }
  }

  @override
  void dispose() {
    _sessionController.dispose();
    _breathingController.dispose();
    _setupController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.backgroundDark,
      body: SafeArea(
        child: isSessionActive ? _buildActiveSession() : _buildSetupScreen(),
      ),
    );
  }

  Widget _buildSetupScreen() {
    return SingleChildScrollView(
      child: Padding(
        padding: EdgeInsets.all(AppConstants.paddingLarge.w),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            _buildHeader(),
            SizedBox(height: 30.h),
            _buildVisualPreview(),
            SizedBox(height: 40.h),
            _buildVisualTypeSection(),
            SizedBox(height: 30.h),
            _buildVisualCustomization(),
            SizedBox(height: 30.h),
            _buildMeditationTypeSection(),
            SizedBox(height: 30.h),
            _buildDurationSection(),
            SizedBox(height: 40.h),
            _buildStartButton(),
            SizedBox(height: 20.h),
          ],
        ),
      ),
    );
  }

  Widget _buildActiveSession() {
    return Stack(
      children: [
        // Background gradient
        Container(
          decoration: BoxDecoration(
            gradient: LinearGradient(
              begin: Alignment.topCenter,
              end: Alignment.bottomCenter,
              colors: [
                AppColors.backgroundDark,
                AppColors.backgroundMedium,
                AppColors.backgroundDark,
              ],
            ),
          ),
        ),
        
        // Top controls
        SafeArea(
          child: Padding(
            padding: EdgeInsets.all(AppConstants.paddingLarge.w),
            child: Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                // Session timer
                SessionTimer(
                  remainingSeconds: remainingSeconds,
                  totalSeconds: selectedDuration * 60,
                  currentPhase: currentPhase,
                  isActive: isPlaying,
                  accentColor: _getCurrentGradient()[0],
                ),
                
                // Play/Pause button
                _buildPlayPauseButton(),
              ],
            ),
          ),
        ),
        
        // Main visual in center
        Positioned.fill(
          child: Center(
            child: GestureDetector(
              onTap: _toggleBreathingGuide,
              child: _buildActiveVisual(),
            ),
          ),
        ),
        
        // Breathing guide overlay
        if (showBreathingGuide)
          Positioned(
            top: 200.h,
            left: 0,
            right: 0,
            child: BreathingGuide(
              controller: _breathingController,
              isActive: isPlaying,
              color: _getCurrentGradient()[0],
            ),
          ),
        
        // Bottom controls
        Positioned(
          bottom: 50.h,
          left: 0,
          right: 0,
          child: _buildSessionControls(),
        ),
      ],
    );
  }

  Widget _buildHeader() {
    return Row(
      children: [
        GestureDetector(
          onTap: () => Navigator.pop(context),
          child: Container(
            width: 44.w,
            height: 44.h,
            decoration: BoxDecoration(
              color: AppColors.cardBackground,
              borderRadius: BorderRadius.circular(AppConstants.radiusMedium.r),
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
                'Meditation',
                style: TextStyle(
                  fontSize: 24.sp,
                  fontWeight: FontWeight.bold,
                  color: AppColors.textLight,
                ),
              ),
              Text(
                'Find your inner peace',
                style: TextStyle(
                  fontSize: 14.sp,
                  color: AppColors.textMedium,
                ),
              ),
            ],
          ),
        ),
      ],
    );
  }

  Widget _buildVisualPreview() {
    return Center(
      child: Container(
        height: 150.h,
        child: AnimatedSwitcher(
          duration: AppConstants.slowAnimation,
          switchInCurve: Curves.easeInOut,
          switchOutCurve: Curves.easeInOut,
          child: _buildPreviewWidget(),
        ),
      ),
    );
  }

  Widget _buildPreviewWidget() {
    final intensity = AppConstants.getPreviewIntensity(selectedVisualType);
    final size = AppConstants.previewSize;
    
    switch (selectedVisualType) {
      case VisualType.candle:
        return AnimatedCandle(
          key: ValueKey('candle-${selectedCandleColor}'),
          color: selectedCandleColor,
          intensity: intensity,
          size: size,
        );
      case VisualType.moon:
        return AnimatedMoon(
          key: ValueKey('moon-${selectedMoonPhase}'),
          phase: selectedMoonPhase,
          intensity: intensity,
          size: size,
        );
      case VisualType.nature:
        return AnimatedNature(
          key: ValueKey('nature-${selectedNaturePattern}'),
          pattern: selectedNaturePattern,
          intensity: intensity,
          size: size,
        );
      case VisualType.minimal:
        return AnimatedMinimal(
          key: ValueKey('minimal'),
          intensity: intensity,
          size: size,
        );
      case VisualType.geometric:
        return AnimatedGeometric(
          key: ValueKey('geometric-${selectedGeometricPattern}'),
          pattern: selectedGeometricPattern,
          intensity: intensity,
          size: size,
        );
    }
  }

  Widget _buildVisualTypeSection() {
    return VisualSelector(
      selectedVisual: selectedVisualType,
      onVisualChanged: (visual) {
        setState(() {
          selectedVisualType = visual;
        });
        _setupController.forward().then((_) {
          _setupController.reverse();
        });
      },
    );
  }

  Widget _buildVisualCustomization() {
    Map<String, dynamic> currentSettings = _getCurrentVisualSettings();
    
    return AnimatedSwitcher(
      duration: AppConstants.normalAnimation,
      child: VisualCustomization(
        key: ValueKey(selectedVisualType),
        visualType: selectedVisualType,
        settings: currentSettings,
        onSettingsChanged: _updateVisualSettings,
      ),
    );
  }

  Widget _buildMeditationTypeSection() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Meditation Practice',
          style: TextStyle(
            fontSize: 18.sp,
            fontWeight: FontWeight.w600,
            color: AppColors.textLight,
          ),
        ),
        SizedBox(height: 16.h),
        Container(
          decoration: BoxDecoration(
            color: AppColors.cardBackground,
            borderRadius: BorderRadius.circular(AppConstants.radiusLarge.r),
            border: Border.all(
              color: AppColors.textMedium.withOpacity(0.1),
              width: 1.w,
            ),
          ),
          child: Column(
            children: MeditationType.values.take(6).map((type) {
              final meditation = AppConstants.defaultMeditations.firstWhere(
                (m) => m.type == type,
              );
              final isSelected = selectedMeditationType == type;
              final isLast = type == MeditationType.values.take(6).last;
              
              return _buildMeditationTypeCard(meditation, isSelected, isLast);
            }).toList(),
          ),
        ),
      ],
    );
  }

  Widget _buildMeditationTypeCard(Meditation meditation, bool isSelected, bool isLast) {
    return GestureDetector(
      onTap: () => setState(() => selectedMeditationType = meditation.type),
      child: AnimatedContainer(
        duration: AppConstants.fastAnimation,
        decoration: BoxDecoration(
          color: isSelected 
              ? _getCurrentGradient()[0].withOpacity(0.1)
              : Colors.transparent,
          borderRadius: BorderRadius.vertical(
            top: meditation.type == MeditationType.values.first 
                ? Radius.circular(AppConstants.radiusLarge.r) 
                : Radius.zero,
            bottom: isLast 
                ? Radius.circular(AppConstants.radiusLarge.r) 
                : Radius.zero,
          ),
        ),
        child: Container(
          padding: EdgeInsets.all(AppConstants.paddingLarge.w),
          decoration: BoxDecoration(
            border: !isLast ? Border(
              bottom: BorderSide(
                color: AppColors.textMedium.withOpacity(0.1),
                width: 1.w,
              ),
            ) : null,
          ),
          child: Row(
            children: [
              AnimatedContainer(
                duration: AppConstants.fastAnimation,
                width: 50.w,
                height: 50.h,
                decoration: BoxDecoration(
                  gradient: isSelected
                      ? LinearGradient(colors: _getCurrentGradient())
                      : null,
                  color: isSelected ? null : AppColors.backgroundMedium,
                  borderRadius: BorderRadius.circular(AppConstants.radiusMedium.r),
                ),
                child: Icon(
                  meditation.type.icon,
                  color: isSelected ? AppColors.white : AppColors.textMedium,
                  size: 24.sp,
                ),
              ),
              SizedBox(width: 16.w),
              Expanded(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      meditation.title,
                      style: TextStyle(
                        fontSize: 16.sp,
                        fontWeight: FontWeight.w600,
                        color: AppColors.textLight,
                      ),
                    ),
                    SizedBox(height: 4.h),
                    Text(
                      meditation.description,
                      style: TextStyle(
                        fontSize: 14.sp,
                        color: AppColors.textMedium,
                      ),
                    ),
                    SizedBox(height: 4.h),
                    Row(
                      children: [
                        Icon(
                          Icons.person,
                          size: 12.sp,
                          color: AppColors.textMedium.withOpacity(0.6),
                        ),
                        SizedBox(width: 4.w),
                        Text(
                          meditation.instructor,
                          style: TextStyle(
                            fontSize: 12.sp,
                            color: AppColors.textMedium.withOpacity(0.6),
                          ),
                        ),
                      ],
                    ),
                  ],
                ),
              ),
              AnimatedScale(
                scale: isSelected ? 1.0 : 0.0,
                duration: AppConstants.fastAnimation,
                child: Container(
                  width: 24.w,
                  height: 24.h,
                  decoration: BoxDecoration(
                    gradient: LinearGradient(colors: _getCurrentGradient()),
                    shape: BoxShape.circle,
                  ),
                  child: Icon(
                    Icons.check,
                    color: AppColors.white,
                    size: 16.sp,
                  ),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }

  Widget _buildDurationSection() {
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
        Container(
          height: 60.h,
          child: ListView.builder(
            scrollDirection: Axis.horizontal,
            itemCount: AppConstants.meditationDurations.length,
            itemBuilder: (context, index) {
              final duration = AppConstants.meditationDurations[index];
              final isSelected = selectedDuration == duration;
              
              return Padding(
                padding: EdgeInsets.only(right: 12.w),
                child: GestureDetector(
                  onTap: () => setState(() => selectedDuration = duration),
                  child: AnimatedContainer(
                    duration: AppConstants.fastAnimation,
                    width: 80.w,
                    decoration: BoxDecoration(
                      gradient: isSelected
                          ? LinearGradient(colors: _getCurrentGradient())
                          : null,
                      color: isSelected ? null : AppColors.cardBackground,
                      borderRadius: BorderRadius.circular(AppConstants.radiusLarge.r),
                      border: Border.all(
                        color: isSelected
                            ? Colors.transparent
                            : AppColors.textMedium.withOpacity(0.2),
                        width: 1.w,
                      ),
                      boxShadow: isSelected
                          ? [
                              BoxShadow(
                                color: _getCurrentGradient()[0].withOpacity(0.3),
                                blurRadius: 8.r,
                                offset: Offset(0, 4.h),
                              ),
                            ]
                          : [],
                    ),
                    child: Column(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [
                        Text(
                          '${duration}',
                          style: TextStyle(
                            fontSize: 20.sp,
                            fontWeight: FontWeight.bold,
                            color: isSelected ? AppColors.white : AppColors.textLight,
                          ),
                        ),
                        Text(
                          'min',
                          style: TextStyle(
                            fontSize: 12.sp,
                            color: isSelected 
                                ? AppColors.white.withOpacity(0.8) 
                                : AppColors.textMedium,
                          ),
                        ),
                      ],
                    ),
                  ),
                ),
              );
            },
          ),
        ),
      ],
    );
  }

  Widget _buildStartButton() {
    return Container(
      width: double.infinity,
      height: 60.h,
      decoration: BoxDecoration(
        gradient: LinearGradient(colors: _getCurrentGradient()),
        borderRadius: BorderRadius.circular(AppConstants.radiusXLarge.r),
        boxShadow: [
          BoxShadow(
            color: _getCurrentGradient()[0].withOpacity(0.4),
            blurRadius: 20.r,
            offset: Offset(0, 8.h),
          ),
        ],
      ),
      child: Material(
        color: Colors.transparent,
        child: InkWell(
          onTap: _startSession,
          borderRadius: BorderRadius.circular(AppConstants.radiusXLarge.r),
          child: Container(
            alignment: Alignment.center,
            child: Row(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                Icon(
                  Icons.play_arrow,
                  color: AppColors.white,
                  size: 24.sp,
                ),
                SizedBox(width: 8.w),
                Text(
                  'Begin Meditation',
                  style: TextStyle(
                    fontSize: 18.sp,
                    fontWeight: FontWeight.w600,
                    color: AppColors.white,
                  ),
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }

  Widget _buildPlayPauseButton() {
    return GestureDetector(
      onTap: _togglePlayPause,
      onLongPress: _showEndSessionDialog,
      child: Container(
        width: 50.w,
        height: 50.h,
        decoration: BoxDecoration(
          gradient: LinearGradient(
            colors: isPlaying 
                ? _getCurrentGradient().map((c) => c.withOpacity(0.8)).toList()
                : _getCurrentGradient(),
          ),
          shape: BoxShape.circle,
          boxShadow: [
            BoxShadow(
              color: _getCurrentGradient()[0].withOpacity(0.3),
              blurRadius: 8.r,
              offset: Offset(0, 4.h),
            ),
          ],
        ),
        child: Icon(
          isPlaying ? Icons.pause : Icons.play_arrow,
          color: AppColors.white,
          size: 24.sp,
        ),
      ),
    );
  }

  Widget _buildActiveVisual() {
    final intensity = AppConstants.getActiveIntensity(isPlaying);
    final size = AppConstants.activeSessionSize;
    
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
      case VisualType.minimal:
        return AnimatedMinimal(
          intensity: intensity,
          size: size,
        );
      case VisualType.geometric:
        return AnimatedGeometric(
          pattern: selectedGeometricPattern,
          intensity: intensity,
          size: size,
        );
    }
  }

  Widget _buildSessionControls() {
    return Center(
      child: Container(
        padding: EdgeInsets.symmetric(horizontal: 16.w, vertical: 8.h),
        decoration: BoxDecoration(
          color: AppColors.black.withOpacity(0.2),
          borderRadius: BorderRadius.circular(20.r),
        ),
        child: Text(
          showBreathingGuide ? 'Tap to hide breathing guide' : 'Tap for breathing guide',
          style: TextStyle(
            fontSize: 12.sp,
            color: AppColors.textMedium.withOpacity(0.7),
          ),
        ),
      ),
    );
  }

  // Helper methods
  List<Color> _getCurrentGradient() {
    return AppColors.getVisualGradient(selectedVisualType.name);
  }

  Map<String, dynamic> _getCurrentVisualSettings() {
    switch (selectedVisualType) {
      case VisualType.candle:
        return {'candleColor': selectedCandleColor};
      case VisualType.moon:
        return {'moonPhase': selectedMoonPhase};
      case VisualType.nature:
        return {'naturePattern': selectedNaturePattern};
      case VisualType.minimal:
        return {'breathingSpeed': 0.8};
      case VisualType.geometric:
        return {'geometricPattern': selectedGeometricPattern};
    }
  }

  void _updateVisualSettings(Map<String, dynamic> settings) {
    setState(() {
      switch (selectedVisualType) {
        case VisualType.candle:
          if (settings.containsKey('candleColor')) {
            selectedCandleColor = settings['candleColor'];
          }
          break;
        case VisualType.moon:
          if (settings.containsKey('moonPhase')) {
            selectedMoonPhase = settings['moonPhase'];
          }
          break;
        case VisualType.nature:
          if (settings.containsKey('naturePattern')) {
            selectedNaturePattern = settings['naturePattern'];
          }
          break;
        case VisualType.minimal:
          // Breathing speed handled in the widget
          break;
        case VisualType.geometric:
          if (settings.containsKey('geometricPattern')) {
            selectedGeometricPattern = settings['geometricPattern'];
          }
          break;
      }
    });
  }

  // Session control methods
  void _startSession() {
    setState(() {
      isSessionActive = true;
      remainingSeconds = selectedDuration * 60;
      currentPhase = SessionPhase.preparation;
    });
    _sessionController.duration = Duration(minutes: selectedDuration);
    
    // Auto-start after a brief preparation
    Future.delayed(Duration(seconds: 3), () {
      if (mounted && isSessionActive) {
        _togglePlayPause();
      }
    });
  }

  void _togglePlayPause() {
    setState(() {
      isPlaying = !isPlaying;
    });
    
    if (isPlaying) {
      _sessionController.forward();
      if (showBreathingGuide) {
        _breathingController.repeat();
      }
    } else {
      _sessionController.stop();
      _breathingController.stop();
    }
  }

  void _toggleBreathingGuide() {
    setState(() {
      showBreathingGuide = !showBreathingGuide;
    });
    
    if (showBreathingGuide && isPlaying) {
      _breathingController.repeat();
    } else {
      _breathingController.stop();
    }
  }

  void _showEndSessionDialog() {
    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        backgroundColor: AppColors.cardBackground,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(AppConstants.radiusLarge.r),
        ),
        title: Text(
          'End Session',
          style: TextStyle(
            color: AppColors.textLight,
            fontSize: 18.sp,
            fontWeight: FontWeight.bold,
          ),
        ),
        content: Text(
          'Are you sure you want to end your meditation session?',
          style: TextStyle(
            color: AppColors.textMedium,
            fontSize: 14.sp,
          ),
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context),
            child: Text(
              'Continue',
              style: TextStyle(color: AppColors.textMedium),
            ),
          ),
          TextButton(
            onPressed: () {
              Navigator.pop(context);
              _endSession();
            },
            child: Text(
              'End Session',
              style: TextStyle(color: _getCurrentGradient()[0]),
            ),
          ),
        ],
      ),
    );
  }

  void _endSession() {
    setState(() {
      isSessionActive = false;
      isPlaying = false;
      showBreathingGuide = false;
    });
    _sessionController.reset();
    _breathingController.reset();
    
    // Show completion dialog
    _showCompletionDialog();
  }

  void _showCompletionDialog() {
    final completedMinutes = selectedDuration - (remainingSeconds / 60).ceil();
    
    showDialog(
      context: context,
      barrierDismissible: false,
      builder: (context) => AlertDialog(
        backgroundColor: AppColors.cardBackground,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(AppConstants.radiusLarge.r),
        ),
        title: Row(
          children: [
            Container(
              width: 40.w,
              height: 40.h,
              decoration: BoxDecoration(
                gradient: LinearGradient(colors: _getCurrentGradient()),
                shape: BoxShape.circle,
              ),
              child: Icon(
                Icons.check,
                color: AppColors.white,
                size: 20.sp,
              ),
            ),
            SizedBox(width: 12.w),
            Expanded(
              child: Text(
                'Session Complete',
                style: TextStyle(
                  color: AppColors.textLight,
                  fontSize: 18.sp,
                  fontWeight: FontWeight.bold,
                ),
              ),
            ),
          ],
        ),
        content: Column(
          mainAxisSize: MainAxisSize.min,
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              'Great work! You meditated for $completedMinutes minutes.',
              style: TextStyle(
                color: AppColors.textMedium,
                fontSize: 14.sp,
              ),
            ),
            SizedBox(height: 16.h),
            Container(
              padding: EdgeInsets.all(12.w),
              decoration: BoxDecoration(
                color: _getCurrentGradient()[0].withOpacity(0.1),
                borderRadius: BorderRadius.circular(AppConstants.radiusMedium.r),
              ),
              child: Row(
                children: [
                  Icon(
                    selectedVisualType.icon,
                    color: _getCurrentGradient()[0],
                    size: 16.sp,
                  ),
                  SizedBox(width: 8.w),
                  Text(
                    '${selectedVisualType.name} • ${selectedMeditationType.name}',
                    style: TextStyle(
                      color: _getCurrentGradient()[0],
                      fontSize: 12.sp,
                      fontWeight: FontWeight.w500,
                    ),
                  ),
                ],
              ),
            ),
          ],
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context),
            child: Text(
              'Done',
              style: TextStyle(color: _getCurrentGradient()[0]),
            ),
          ),
        ],
      ),
    );
  }
}