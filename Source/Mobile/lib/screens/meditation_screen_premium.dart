import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'dart:ui';
import '../constants/colors.dart';
import '../models/meditation_models.dart';
import '../components/meditation_timer.dart';

class PremiumMeditationScreen extends StatefulWidget {
  @override
  _PremiumMeditationScreenState createState() => _PremiumMeditationScreenState();
}

class _PremiumMeditationScreenState extends State<PremiumMeditationScreen>
    with TickerProviderStateMixin {
  
  // Core state
  bool isSessionActive = false;
  bool isPlaying = false;
  int selectedDuration = 10;
  int selectedMeditationType = 0;
  int selectedAmbience = 0;
  
  // Session tracking
  int remainingSeconds = 0;
  late AnimationController _sessionController;
  late AnimationController _breathingController;
  late AnimationController _uiController;
  
  // Premium animations
  late AnimationController _backgroundController;
  late Animation<double> _backgroundAnimation;
  late Animation<double> _fadeAnimation;
  late Animation<Offset> _slideAnimation;

  final List<int> durations = [5, 10, 15, 20, 30];
  final List<String> meditationTypes = [
    'Mindfulness',
    'Focus',
    'Sleep',
    'Anxiety Relief',
    'Manifestation'
  ];
  final List<String> ambienceTypes = [
    'Ocean Waves',
    'Forest Rain',
    'White Noise',
    'Singing Bowls',
    'Silence'
  ];

  @override
  void initState() {
    super.initState();
    _initializeAnimations();
  }

  void _initializeAnimations() {
    // Session timer
    _sessionController = AnimationController(
      vsync: this,
      duration: Duration(minutes: selectedDuration),
    );
    
    // Breathing animation
    _breathingController = AnimationController(
      vsync: this,
      duration: Duration(seconds: 4),
    );
    
    // UI transitions
    _uiController = AnimationController(
      vsync: this,
      duration: Duration(milliseconds: 800),
    );
    
    // Background ambience
    _backgroundController = AnimationController(
      vsync: this,
      duration: Duration(seconds: 8),
    );
    
    // Smooth animations
    _backgroundAnimation = Tween<double>(
      begin: 0.0,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _backgroundController,
      curve: Curves.easeInOut,
    ));
    
    _fadeAnimation = Tween<double>(
      begin: 0.0,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _uiController,
      curve: Curves.easeOutCubic,
    ));
    
    _slideAnimation = Tween<Offset>(
      begin: Offset(0, 0.1),
      end: Offset.zero,
    ).animate(CurvedAnimation(
      parent: _uiController,
      curve: Curves.easeOutCubic,
    ));
    
    // Start UI animation
    _uiController.forward();
    _backgroundController.repeat(reverse: true);
    
    // Session listener
    _sessionController.addListener(_onSessionProgress);
  }

  void _onSessionProgress() {
    if (mounted) {
      setState(() {
        remainingSeconds = ((1 - _sessionController.value) * selectedDuration * 60).round();
      });
    }
  }

  @override
  void dispose() {
    _sessionController.dispose();
    _breathingController.dispose();
    _uiController.dispose();
    _backgroundController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.backgroundDark,
      body: AnimatedBuilder(
        animation: _backgroundAnimation,
        builder: (context, child) {
          return Container(
            decoration: BoxDecoration(
              gradient: LinearGradient(
                begin: Alignment.topLeft,
                end: Alignment.bottomRight,
                colors: [
                  AppColors.backgroundDark,
                  AppColors.backgroundDark.withBlue(
                    (AppColors.backgroundDark.blue + _backgroundAnimation.value * 10).clamp(0, 255).toInt()
                  ),
                  AppColors.backgroundDark,
                ],
              ),
            ),
            child: isSessionActive ? _buildActiveSession() : _buildSetupScreen(),
          );
        },
      ),
    );
  }

  Widget _buildSetupScreen() {
    return SafeArea(
      child: FadeTransition(
        opacity: _fadeAnimation,
        child: SlideTransition(
          position: _slideAnimation,
          child: SingleChildScrollView(
            physics: BouncingScrollPhysics(),
            child: Padding(
              padding: EdgeInsets.all(24.w),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  _buildPremiumHeader(),
                  SizedBox(height: 40.h),
                  _buildCentralVisual(),
                  SizedBox(height: 50.h),
                  _buildPremiumDurationSelector(),
                  SizedBox(height: 40.h),
                  _buildPremiumMeditationTypeSelector(),
                  SizedBox(height: 40.h),
                  _buildPremiumAmbienceSelector(),
                  SizedBox(height: 60.h),
                  _buildPremiumStartButton(),
                  SizedBox(height: 20.h),
                ],
              ),
            ),
          ),
        ),
      ),
    );
  }

  Widget _buildPremiumHeader() {
    return Row(
      children: [
        GestureDetector(
          onTap: () => Navigator.pop(context),
          child: Container(
            width: 48.w,
            height: 48.h,
            decoration: BoxDecoration(
              color: AppColors.white.withOpacity(0.05),
              borderRadius: BorderRadius.circular(16.r),
              border: Border.all(
                color: AppColors.white.withOpacity(0.1),
                width: 1,
              ),
            ),
            child: Icon(
              Icons.arrow_back_ios_rounded,
              color: AppColors.textLight,
              size: 20.sp,
            ),
          ),
        ),
        SizedBox(width: 20.w),
        Expanded(
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(
                'Meditation',
                style: TextStyle(
                  fontSize: 28.sp,
                  fontWeight: FontWeight.w700,
                  color: AppColors.textLight,
                  letterSpacing: -0.5,
                ),
              ),
              SizedBox(height: 4.h),
              Text(
                'Find your moment of peace',
                style: TextStyle(
                  fontSize: 16.sp,
                  color: AppColors.textMedium,
                  fontWeight: FontWeight.w400,
                ),
              ),
            ],
          ),
        ),
      ],
    );
  }

  Widget _buildCentralVisual() {
    return Center(
      child: AnimatedBuilder(
        animation: _breathingController,
        builder: (context, child) {
          final scale = 1.0 + (_breathingController.value * 0.1);
          return Transform.scale(
            scale: scale,
            child: Container(
              width: 200.w,
              height: 200.h,
              decoration: BoxDecoration(
                shape: BoxShape.circle,
                gradient: RadialGradient(
                  colors: [
                    AppColors.affirmationGradient[0].withOpacity(0.3),
                    AppColors.affirmationGradient[1].withOpacity(0.1),
                    Colors.transparent,
                  ],
                ),
                boxShadow: [
                  BoxShadow(
                    color: AppColors.affirmationGradient[0].withOpacity(0.2),
                    blurRadius: 40.r,
                    spreadRadius: 10.r,
                  ),
                ],
              ),
              child: Container(
                margin: EdgeInsets.all(40.w),
                decoration: BoxDecoration(
                  shape: BoxShape.circle,
                  gradient: LinearGradient(
                    colors: AppColors.affirmationGradient,
                  ),
                ),
                child: Icon(
                  Icons.self_improvement,
                  color: AppColors.white,
                  size: 60.sp,
                ),
              ),
            ),
          );
        },
      ),
    );
  }

  Widget _buildPremiumDurationSelector() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Duration',
          style: TextStyle(
            fontSize: 20.sp,
            fontWeight: FontWeight.w600,
            color: AppColors.textLight,
          ),
        ),
        SizedBox(height: 20.h),
        Container(
          height: 70.h,
          child: ListView.builder(
            scrollDirection: Axis.horizontal,
            itemCount: durations.length,
            itemBuilder: (context, index) {
              final duration = durations[index];
              final isSelected = selectedDuration == duration;
              
              return Padding(
                padding: EdgeInsets.only(right: 16.w),
                child: GestureDetector(
                  onTap: () => setState(() => selectedDuration = duration),
                  child: AnimatedContainer(
                    duration: Duration(milliseconds: 300),
                    width: 90.w,
                    decoration: BoxDecoration(
                      gradient: isSelected
                          ? LinearGradient(colors: AppColors.affirmationGradient)
                          : null,
                      color: isSelected ? null : AppColors.white.withOpacity(0.05),
                      borderRadius: BorderRadius.circular(20.r),
                      border: Border.all(
                        color: isSelected
                            ? Colors.transparent
                            : AppColors.white.withOpacity(0.1),
                        width: 1,
                      ),
                      boxShadow: isSelected
                          ? [
                              BoxShadow(
                                color: AppColors.affirmationGradient[0].withOpacity(0.3),
                                blurRadius: 20.r,
                                offset: Offset(0, 8.h),
                              ),
                            ]
                          : null,
                    ),
                    child: Column(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [
                        Text(
                          '$duration',
                          style: TextStyle(
                            fontSize: 24.sp,
                            fontWeight: FontWeight.w700,
                            color: isSelected ? AppColors.white : AppColors.textLight,
                          ),
                        ),
                        Text(
                          'min',
                          style: TextStyle(
                            fontSize: 14.sp,
                            fontWeight: FontWeight.w500,
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

  Widget _buildPremiumMeditationTypeSelector() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Practice Type',
          style: TextStyle(
            fontSize: 20.sp,
            fontWeight: FontWeight.w600,
            color: AppColors.textLight,
          ),
        ),
        SizedBox(height: 20.h),
        ...meditationTypes.asMap().entries.map((entry) {
          final index = entry.key;
          final type = entry.value;
          final isSelected = selectedMeditationType == index;
          
          return Padding(
            padding: EdgeInsets.only(bottom: 12.h),
            child: GestureDetector(
              onTap: () => setState(() => selectedMeditationType = index),
              child: AnimatedContainer(
                duration: Duration(milliseconds: 300),
                padding: EdgeInsets.all(20.w),
                decoration: BoxDecoration(
                  gradient: isSelected
                      ? LinearGradient(
                          colors: AppColors.affirmationGradient.map((c) => c.withOpacity(0.1)).toList(),
                        )
                      : null,
                  color: isSelected ? null : AppColors.white.withOpacity(0.02),
                  borderRadius: BorderRadius.circular(16.r),
                  border: Border.all(
                    color: isSelected
                        ? AppColors.affirmationGradient[0].withOpacity(0.3)
                        : AppColors.white.withOpacity(0.05),
                    width: 1,
                  ),
                ),
                child: Row(
                  children: [
                    Container(
                      width: 12.w,
                      height: 12.h,
                      decoration: BoxDecoration(
                        shape: BoxShape.circle,
                        gradient: isSelected
                            ? LinearGradient(colors: AppColors.affirmationGradient)
                            : null,
                        color: isSelected ? null : AppColors.white.withOpacity(0.2),
                      ),
                    ),
                    SizedBox(width: 16.w),
                    Text(
                      type,
                      style: TextStyle(
                        fontSize: 16.sp,
                        fontWeight: FontWeight.w500,
                        color: isSelected ? AppColors.textLight : AppColors.textMedium,
                      ),
                    ),
                  ],
                ),
              ),
            ),
          );
        }).toList(),
      ],
    );
  }

  Widget _buildPremiumAmbienceSelector() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Background Sound',
          style: TextStyle(
            fontSize: 20.sp,
            fontWeight: FontWeight.w600,
            color: AppColors.textLight,
          ),
        ),
        SizedBox(height: 20.h),
        Container(
          height: 60.h,
          child: ListView.builder(
            scrollDirection: Axis.horizontal,
            itemCount: ambienceTypes.length,
            itemBuilder: (context, index) {
              final ambience = ambienceTypes[index];
              final isSelected = selectedAmbience == index;
              
              return Padding(
                padding: EdgeInsets.only(right: 12.w),
                child: GestureDetector(
                  onTap: () => setState(() => selectedAmbience = index),
                  child: AnimatedContainer(
                    duration: Duration(milliseconds: 300),
                    padding: EdgeInsets.symmetric(horizontal: 20.w, vertical: 16.h),
                    decoration: BoxDecoration(
                      gradient: isSelected
                          ? LinearGradient(colors: AppColors.affirmationGradient)
                          : null,
                      color: isSelected ? null : AppColors.white.withOpacity(0.05),
                      borderRadius: BorderRadius.circular(16.r),
                      border: Border.all(
                        color: isSelected
                            ? Colors.transparent
                            : AppColors.white.withOpacity(0.1),
                        width: 1,
                      ),
                    ),
                    child: Center(
                      child: Text(
                        ambience,
                        style: TextStyle(
                          fontSize: 14.sp,
                          fontWeight: FontWeight.w500,
                          color: isSelected ? AppColors.white : AppColors.textMedium,
                        ),
                      ),
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

  Widget _buildPremiumStartButton() {
    return Container(
      width: double.infinity,
      height: 60.h,
      decoration: BoxDecoration(
        gradient: LinearGradient(colors: AppColors.affirmationGradient),
        borderRadius: BorderRadius.circular(20.r),
        boxShadow: [
          BoxShadow(
            color: AppColors.affirmationGradient[0].withOpacity(0.4),
            blurRadius: 20.r,
            offset: Offset(0, 10.h),
          ),
        ],
      ),
      child: Material(
        color: Colors.transparent,
        child: InkWell(
          onTap: _startSession,
          borderRadius: BorderRadius.circular(20.r),
          child: Container(
            alignment: Alignment.center,
            child: Row(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                Icon(
                  Icons.play_arrow_rounded,
                  color: AppColors.white,
                  size: 28.sp,
                ),
                SizedBox(width: 12.w),
                Text(
                  'Begin Session',
                  style: TextStyle(
                    fontSize: 18.sp,
                    fontWeight: FontWeight.w600,
                    color: AppColors.white,
                    letterSpacing: 0.5,
                  ),
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }

  Widget _buildActiveSession() {
    return Stack(
      children: [
        // Background breathing animation
        Positioned.fill(
          child: AnimatedBuilder(
            animation: _breathingController,
            builder: (context, child) {
              final progress = _breathingController.value;
              final scale = 1.0 + (progress * 0.2);
              return Transform.scale(
                scale: scale,
                child: Container(
                  decoration: BoxDecoration(
                    gradient: RadialGradient(
                      center: Alignment.center,
                      colors: [
                        AppColors.affirmationGradient[0].withOpacity(0.1),
                        Colors.transparent,
                      ],
                      stops: [0.3, 1.0],
                    ),
                  ),
                ),
              );
            },
          ),
        ),
        
        // Timer and controls
        SafeArea(
          child: Padding(
            padding: EdgeInsets.all(24.w),
            child: Column(
              children: [
                // Top bar
                Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: [
                    _buildSessionTimer(),
                    _buildPlayPauseButton(),
                  ],
                ),
                
                Spacer(),
                
                // Central breathing visual
                _buildBreathingVisual(),
                
                Spacer(),
                
                // Bottom controls
                _buildBottomControls(),
              ],
            ),
          ),
        ),
      ],
    );
  }

  Widget _buildSessionTimer() {
    final minutes = remainingSeconds ~/ 60;
    final seconds = remainingSeconds % 60;
    
    return Container(
      padding: EdgeInsets.symmetric(horizontal: 20.w, vertical: 12.h),
      decoration: BoxDecoration(
        color: AppColors.black.withOpacity(0.3),
        borderRadius: BorderRadius.circular(20.r),
        border: Border.all(
          color: AppColors.white.withOpacity(0.1),
          width: 1,
        ),
      ),
      child: Text(
        '${minutes.toString().padLeft(2, '0')}:${seconds.toString().padLeft(2, '0')}',
        style: TextStyle(
          fontSize: 18.sp,
          fontWeight: FontWeight.w600,
          color: AppColors.white,
          fontFeatures: [FontFeature.tabularFigures()],
        ),
      ),
    );
  }

  Widget _buildPlayPauseButton() {
    return GestureDetector(
      onTap: _togglePlayPause,
      onLongPress: _endSession,
      child: Container(
        width: 60.w,
        height: 60.h,
        decoration: BoxDecoration(
          gradient: LinearGradient(colors: AppColors.affirmationGradient),
          shape: BoxShape.circle,
          boxShadow: [
            BoxShadow(
              color: AppColors.affirmationGradient[0].withOpacity(0.4),
              blurRadius: 15.r,
              offset: Offset(0, 5.h),
            ),
          ],
        ),
        child: Icon(
          isPlaying ? Icons.pause_rounded : Icons.play_arrow_rounded,
          color: AppColors.white,
          size: 30.sp,
        ),
      ),
    );
  }

  Widget _buildBreathingVisual() {
    return AnimatedBuilder(
      animation: _breathingController,
      builder: (context, child) {
        final scale = 1.0 + (_breathingController.value * 0.3);
        return Transform.scale(
          scale: scale,
          child: Container(
            width: 250.w,
            height: 250.h,
            decoration: BoxDecoration(
              shape: BoxShape.circle,
              gradient: RadialGradient(
                colors: [
                  AppColors.affirmationGradient[0].withOpacity(0.3),
                  AppColors.affirmationGradient[1].withOpacity(0.1),
                  Colors.transparent,
                ],
              ),
              border: Border.all(
                color: AppColors.white.withOpacity(0.1),
                width: 2,
              ),
            ),
            child: Center(
              child: Text(
                _getBreathingText(),
                style: TextStyle(
                  fontSize: 24.sp,
                  fontWeight: FontWeight.w500,
                  color: AppColors.white,
                ),
              ),
            ),
          ),
        );
      },
    );
  }

  Widget _buildBottomControls() {
    return Column(
      children: [
        Text(
          meditationTypes[selectedMeditationType],
          style: TextStyle(
            fontSize: 16.sp,
            fontWeight: FontWeight.w500,
            color: AppColors.textMedium,
          ),
        ),
        SizedBox(height: 8.h),
        Text(
          ambienceTypes[selectedAmbience],
          style: TextStyle(
            fontSize: 14.sp,
            color: AppColors.textMedium.withOpacity(0.7),
          ),
        ),
      ],
    );
  }

  String _getBreathingText() {
    final progress = _breathingController.value;
    if (progress < 0.25) return 'Breathe in...';
    if (progress < 0.5) return 'Hold...';
    if (progress < 0.75) return 'Breathe out...';
    return 'Hold...';
  }

  // Session control methods
  void _startSession() {
    setState(() {
      isSessionActive = true;
      isPlaying = true;
      remainingSeconds = selectedDuration * 60;
    });
    
    _sessionController.duration = Duration(minutes: selectedDuration);
    _sessionController.forward();
    _breathingController.repeat();
  }

  void _togglePlayPause() {
    setState(() {
      isPlaying = !isPlaying;
    });
    
    if (isPlaying) {
      _sessionController.forward();
      _breathingController.repeat();
    } else {
      _sessionController.stop();
      _breathingController.stop();
    }
  }

  void _endSession() {
    setState(() {
      isSessionActive = false;
      isPlaying = false;
    });
    
    _sessionController.reset();
    _breathingController.reset();
    
    _showCompletionDialog();
  }

  void _showCompletionDialog() {
    showDialog(
      context: context,
      barrierDismissible: false,
      builder: (context) => Dialog(
        backgroundColor: AppColors.cardBackground,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(20.r),
        ),
        child: Padding(
          padding: EdgeInsets.all(30.w),
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              Container(
                width: 60.w,
                height: 60.h,
                decoration: BoxDecoration(
                  gradient: LinearGradient(colors: AppColors.affirmationGradient),
                  shape: BoxShape.circle,
                ),
                child: Icon(
                  Icons.check_rounded,
                  color: AppColors.white,
                  size: 30.sp,
                ),
              ),
              SizedBox(height: 20.h),
              Text(
                'Session Complete',
                style: TextStyle(
                  fontSize: 22.sp,
                  fontWeight: FontWeight.w700,
                  color: AppColors.textLight,
                ),
              ),
              SizedBox(height: 12.h),
              Text(
                'Well done! You\'ve completed your meditation.',
                style: TextStyle(
                  fontSize: 16.sp,
                  color: AppColors.textMedium,
                ),
                textAlign: TextAlign.center,
              ),
              SizedBox(height: 30.h),
              Container(
                width: double.infinity,
                height: 50.h,
                decoration: BoxDecoration(
                  gradient: LinearGradient(colors: AppColors.affirmationGradient),
                  borderRadius: BorderRadius.circular(16.r),
                ),
                child: Material(
                  color: Colors.transparent,
                  child: InkWell(
                    onTap: () => Navigator.pop(context),
                    borderRadius: BorderRadius.circular(16.r),
                    child: Center(
                      child: Text(
                        'Done',
                        style: TextStyle(
                          fontSize: 16.sp,
                          fontWeight: FontWeight.w600,
                          color: AppColors.white,
                        ),
                      ),
                    ),
                  ),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}