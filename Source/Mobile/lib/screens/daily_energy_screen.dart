import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../constants/colors.dart';
import '../constants/app_theme.dart';
import 'dart:math' as math;

class DailyEnergyScreen extends StatefulWidget {
  @override
  _DailyEnergyScreenState createState() => _DailyEnergyScreenState();
}

class _DailyEnergyScreenState extends State<DailyEnergyScreen>
    with TickerProviderStateMixin {
  late AnimationController _sunController;
  late AnimationController _moonController;
  late AnimationController _pulseController;
  late Animation<double> _sunAnimation;
  late Animation<double> _moonAnimation;
  late Animation<double> _pulseAnimation;
  
  double _energyPercentage = 0.0;
  late DateTime _currentTime;

  @override
  void initState() {
    super.initState();
    _currentTime = DateTime.now();
    
    _sunController = AnimationController(
      duration: Duration(milliseconds: 3000),
      vsync: this,
    );
    
    _moonController = AnimationController(
      duration: Duration(milliseconds: 4000),
      vsync: this,
    );
    
    _pulseController = AnimationController(
      duration: Duration(milliseconds: 2000),
      vsync: this,
    );
    
    _sunAnimation = Tween<double>(
      begin: 0.0,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _sunController,
      curve: Curves.easeInOut,
    ));
    
    _moonAnimation = Tween<double>(
      begin: 0.0,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _moonController,
      curve: Curves.easeInOut,
    ));
    
    _pulseAnimation = Tween<double>(
      begin: 1.0,
      end: 1.1,
    ).animate(CurvedAnimation(
      parent: _pulseController,
      curve: Curves.easeInOut,
    ));
    
    _calculateEnergyLevel();
    _startAnimations();
    
    // Update every minute
    Stream.periodic(Duration(minutes: 1)).listen((_) {
      if (mounted) {
        _calculateEnergyLevel();
      }
    });
  }

  void _calculateEnergyLevel() {
    final now = DateTime.now();
    final startOfDay = DateTime(now.year, now.month, now.day);
    
    final totalMinutesInDay = 24 * 60;
    final minutesSinceStart = now.difference(startOfDay).inMinutes;
    
    setState(() {
      _currentTime = now;
      _energyPercentage = (minutesSinceStart / totalMinutesInDay).clamp(0.0, 1.0);
    });
  }

  void _startAnimations() {
    final hour = _currentTime.hour;
    
    // Sun is more active during day (6 AM - 8 PM)
    if (hour >= 6 && hour <= 20) {
      _sunController.repeat(reverse: true);
      _pulseController.repeat(reverse: true);
      if (_moonController.isAnimating) {
        _moonController.stop();
      }
    } else {
      // Moon is active at night (8 PM - 6 AM)
      _moonController.repeat(reverse: true);
      if (_sunController.isAnimating) {
        _sunController.stop();
      }
      if (_pulseController.isAnimating) {
        _pulseController.stop();
      }
    }
  }

  Color _getEnergyColor() {
    if (_energyPercentage <= 0.33) {
      return Color.lerp(Color(0xFFFF3B30), Color(0xFFFF6B47), _energyPercentage * 3)!;
    } else if (_energyPercentage <= 0.66) {
      final normalizedProgress = (_energyPercentage - 0.33) * 3;
      return Color.lerp(Color(0xFFFF9500), Color(0xFFFFB347), normalizedProgress)!;
    } else {
      final normalizedProgress = (_energyPercentage - 0.66) * 3;
      return Color.lerp(Color(0xFF34C759), Color(0xFF30D158), normalizedProgress)!;
    }
  }

  String _getEnergyLabel() {
    if (_energyPercentage <= 0.33) {
      return 'Building Energy';
    } else if (_energyPercentage <= 0.66) {
      return 'Rising Energy';
    } else {
      return 'Peak Energy';
    }
  }

  String _getTimePhase() {
    final hour = _currentTime.hour;
    if (hour >= 5 && hour < 12) {
      return 'Morning';
    } else if (hour >= 12 && hour < 17) {
      return 'Afternoon';
    } else if (hour >= 17 && hour < 21) {
      return 'Evening';
    } else {
      return 'Night';
    }
  }

  // Calculate celestial body vertical position based on time
  double _calculateCelestialPosition(double containerHeight) {
    final hour = _currentTime.hour;
    final minute = _currentTime.minute;
    final totalMinutes = hour * 60 + minute;
    
    // Full day cycle: 0 (midnight) to 1440 minutes (next midnight)
    // Normalize to 0.0 - 1.0 where:
    // - 0.0 (midnight) = bottom (ground level)
    // - 0.5 (noon) = top (highest point)  
    // - 1.0 (next midnight) = bottom (ground level)
    
    final normalizedTime = totalMinutes / 1440.0; // 0.0 to 1.0
    
    // Create a sine wave where:
    // - At midnight (0.0 and 1.0): position is at bottom
    // - At noon (0.5): position is at top
    final sineValue = math.sin(normalizedTime * 2 * math.pi);
    
    // Convert sine value (-1 to 1) to position (containerHeight to 50)
    // When sine = -1 (midnight): y = containerHeight (at ground)
    // When sine = 1 (noon): y = 50 (at top)
    final y = containerHeight - ((sineValue + 1) / 2) * (containerHeight - 50);
    
    return y;
  }

  bool _isDaytime() {
    final hour = _currentTime.hour;
    return hour >= 6 && hour < 18;
  }

  Widget _buildCelestialBody() {
    return Container(
      height: 300.h,
      width: double.infinity,
      child: LayoutBuilder(
        builder: (context, constraints) {
          final yPosition = _calculateCelestialPosition(constraints.maxHeight);
          final isDaytime = _isDaytime();
          
          return Stack(
            children: [
              // Single celestial body in center, moving vertically
              AnimatedPositioned(
                duration: Duration(milliseconds: 2000),
                left: constraints.maxWidth / 2 - 40.w, // Center horizontally
                top: yPosition - 40.h, // Move vertically based on time
                child: isDaytime ? _buildRealisticSun() : _buildRealisticMoon(),
              ),
            ],
          );
        },
      ),
    );
  }

  Widget _buildRealisticSun() {
    return AnimatedBuilder(
      animation: _pulseAnimation,
      builder: (context, child) {
        return Transform.scale(
          scale: _pulseAnimation.value,
          child: Container(
            width: 80.w,
            height: 80.h,
            decoration: BoxDecoration(
              shape: BoxShape.circle,
              gradient: RadialGradient(
                colors: [
                  Color(0xFFFFFDD0), // Cream center
                  Color(0xFFFFF700), // Bright yellow
                  Color(0xFFFFD700), // Gold
                  Color(0xFFFFA500), // Orange edge
                ],
                stops: [0.0, 0.3, 0.7, 1.0],
              ),
              boxShadow: [
                BoxShadow(
                  color: Color(0xFFFFD700).withOpacity(0.8),
                  blurRadius: 40.r,
                  spreadRadius: 8.r,
                ),
                BoxShadow(
                  color: Color(0xFFFFA500).withOpacity(0.4),
                  blurRadius: 60.r,
                  spreadRadius: 15.r,
                ),
              ],
            ),
          ),
        );
      },
    );
  }

  Widget _buildRealisticMoon() {
    return AnimatedBuilder(
      animation: _moonAnimation,
      builder: (context, child) {
        return Transform.rotate(
          angle: _moonAnimation.value * 0.05, // Subtle rotation
          child: Container(
            width: 70.w,
            height: 70.h,
            decoration: BoxDecoration(
              shape: BoxShape.circle,
              gradient: RadialGradient(
                colors: [
                  Color(0xFFFFFAFA), // White center
                  Color(0xFFF5F5F5), // Light gray
                  Color(0xFFE0E0E0), // Medium gray
                  Color(0xFFD3D3D3), // Darker gray edge
                ],
                stops: [0.0, 0.4, 0.7, 1.0],
              ),
              boxShadow: [
                BoxShadow(
                  color: Color(0xFFE6E6FA).withOpacity(0.6),
                  blurRadius: 30.r,
                  spreadRadius: 5.r,
                ),
                BoxShadow(
                  color: Color(0xFFD8BFD8).withOpacity(0.3),
                  blurRadius: 50.r,
                  spreadRadius: 10.r,
                ),
              ],
            ),
            child: Container(
              // Add some subtle crater-like shadows for realism
              decoration: BoxDecoration(
                shape: BoxShape.circle,
                gradient: RadialGradient(
                  center: Alignment(-0.3, -0.3),
                  colors: [
                    Color(0xFFFFFAFA).withOpacity(0.9),
                    Color(0xFFE0E0E0).withOpacity(0.7),
                    Colors.transparent,
                  ],
                  stops: [0.0, 0.6, 1.0],
                ),
              ),
            ),
          ),
        );
      },
    );
  }

  @override
  void dispose() {
    _sunController.dispose();
    _moonController.dispose();
    _pulseController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppTheme.backgroundDark,
      body: Container(
        decoration: BoxDecoration(
          gradient: LinearGradient(
            begin: Alignment.topCenter,
            end: Alignment.bottomCenter,
            colors: [
              AppTheme.backgroundDark,
              AppTheme.backgroundDark.withOpacity(0.8),
              _getEnergyColor().withOpacity(0.1),
            ],
          ),
        ),
        child: SafeArea(
          child: Column(
            children: [
              _buildHeader(),
              Expanded(
                child: SingleChildScrollView(
                  child: Column(
                    children: [
                      SizedBox(height: 20.h),
                      _buildEnergyDisplay(),
                      SizedBox(height: 30.h),
                      _buildCelestialBody(),
                      SizedBox(height: 30.h),
                      _buildEnergyStats(),
                      SizedBox(height: 20.h),
                    ],
                  ),
                ),
              ),
            ],
          ),
        ),
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
                color: AppTheme.cardBackground,
                borderRadius: BorderRadius.circular(12.r),
                border: Border.all(
                  color: AppTheme.textMedium.withOpacity(0.2),
                ),
              ),
              child: Icon(
                Icons.arrow_back,
                color: AppTheme.textLight,
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
                  'Daily Energy',
                  style: TextStyle(
                    fontSize: 24.sp,
                    fontWeight: FontWeight.bold,
                    color: AppTheme.textLight,
                  ),
                ),
                SizedBox(height: 4.h),
                Text(
                  'Track your energy throughout the day',
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

  Widget _buildEnergyDisplay() {
    return Container(
      margin: EdgeInsets.symmetric(horizontal: 20.w),
      padding: EdgeInsets.all(30.w),
      decoration: BoxDecoration(
        color: AppTheme.cardBackground,
        borderRadius: BorderRadius.circular(25.r),
        border: Border.all(
          color: _getEnergyColor().withOpacity(0.3),
          width: 2,
        ),
        boxShadow: [
          BoxShadow(
            color: _getEnergyColor().withOpacity(0.1),
            blurRadius: 30.r,
            offset: Offset(0, 10.h),
          ),
        ],
      ),
      child: Column(
        children: [
          // Large energy circle
          Container(
            width: 150.w,
            height: 150.h,
            child: Stack(
              alignment: Alignment.center,
              children: [
                // Background circle
                Container(
                  width: 150.w,
                  height: 150.h,
                  child: CircularProgressIndicator(
                    value: 1.0,
                    strokeWidth: 8.w,
                    backgroundColor: AppTheme.textTertiary.withOpacity(0.1),
                    valueColor: AlwaysStoppedAnimation<Color>(
                      AppTheme.textTertiary.withOpacity(0.1),
                    ),
                  ),
                ),
                
                // Progress circle
                Container(
                  width: 150.w,
                  height: 150.h,
                  child: CircularProgressIndicator(
                    value: _energyPercentage,
                    strokeWidth: 8.w,
                    backgroundColor: Colors.transparent,
                    valueColor: AlwaysStoppedAnimation<Color>(_getEnergyColor()),
                    strokeCap: StrokeCap.round,
                  ),
                ),
                
                // Center content
                Column(
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: [
                    Text(
                      '${(_energyPercentage * 100).round()}%',
                      style: TextStyle(
                        fontSize: 36.sp,
                        fontWeight: FontWeight.bold,
                        color: _getEnergyColor(),
                      ),
                    ),
                    SizedBox(height: 8.h),
                    Container(
                      width: 8.w,
                      height: 8.h,
                      decoration: BoxDecoration(
                        color: _getEnergyColor(),
                        shape: BoxShape.circle,
                        boxShadow: [
                          BoxShadow(
                            color: _getEnergyColor().withOpacity(0.5),
                            blurRadius: 6.r,
                            spreadRadius: 2.r,
                          ),
                        ],
                      ),
                    ),
                  ],
                ),
              ],
            ),
          ),
          
          SizedBox(height: 24.h),
          
          Text(
            _getEnergyLabel(),
            style: TextStyle(
              fontSize: 20.sp,
              fontWeight: FontWeight.w600,
              color: AppTheme.textPrimary,
            ),
          ),
          
          SizedBox(height: 8.h),
          
          Text(
            '${_getTimePhase()} • ${_currentTime.hour.toString().padLeft(2, '0')}:${_currentTime.minute.toString().padLeft(2, '0')}',
            style: TextStyle(
              fontSize: 14.sp,
              color: AppTheme.textSecondary,
            ),
          ),
        ],
      ),
    );
  }


  Widget _buildEnergyStats() {
    final hour = _currentTime.hour;
    final remainingHours = 24 - hour;
    final progressHours = hour + (_currentTime.minute / 60);
    
    return Container(
      margin: EdgeInsets.symmetric(horizontal: 20.w),
      padding: EdgeInsets.all(20.w),
      decoration: BoxDecoration(
        color: AppTheme.cardBackground,
        borderRadius: BorderRadius.circular(20.r),
        border: Border.all(
          color: AppTheme.borderSecondary,
          width: 1,
        ),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(
            'Energy Insights',
            style: TextStyle(
              fontSize: 18.sp,
              fontWeight: FontWeight.w600,
              color: AppTheme.textLight,
            ),
          ),
          
          SizedBox(height: 16.h),
          
          Row(
            children: [
              Expanded(
                child: _buildStatCard(
                  title: 'Hours Elapsed',
                  value: progressHours.toStringAsFixed(1),
                  icon: Icons.access_time,
                  color: _getEnergyColor(),
                ),
              ),
              SizedBox(width: 12.w),
              Expanded(
                child: _buildStatCard(
                  title: 'Hours Remaining',
                  value: remainingHours.toString(),
                  icon: Icons.schedule,
                  color: AppTheme.textSecondary,
                ),
              ),
            ],
          ),
          
          SizedBox(height: 12.h),
          
          Row(
            children: [
              Expanded(
                child: _buildStatCard(
                  title: 'Energy Phase',
                  value: _getTimePhase(),
                  icon: hour >= 6 && hour <= 20 ? Icons.wb_sunny : Icons.nightlight_round,
                  color: hour >= 6 && hour <= 20 ? Color(0xFFFFA500) : Color(0xFF6A5ACD),
                ),
              ),
              SizedBox(width: 12.w),
              Expanded(
                child: _buildStatCard(
                  title: 'Day Progress',
                  value: '${(progressHours / 24 * 100).round()}%',
                  icon: Icons.trending_up,
                  color: _getEnergyColor(),
                ),
              ),
            ],
          ),
        ],
      ),
    );
  }

  Widget _buildStatCard({
    required String title,
    required String value,
    required IconData icon,
    required Color color,
  }) {
    return Container(
      padding: EdgeInsets.all(16.w),
      decoration: BoxDecoration(
        color: color.withOpacity(0.1),
        borderRadius: BorderRadius.circular(12.r),
        border: Border.all(
          color: color.withOpacity(0.2),
          width: 1,
        ),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            children: [
              Icon(
                icon,
                size: 16.sp,
                color: color,
              ),
              SizedBox(width: 6.w),
              Expanded(
                child: Text(
                  title,
                  style: TextStyle(
                    fontSize: 12.sp,
                    color: AppTheme.textSecondary,
                  ),
                ),
              ),
            ],
          ),
          SizedBox(height: 8.h),
          Text(
            value,
            style: TextStyle(
              fontSize: 18.sp,
              fontWeight: FontWeight.bold,
              color: color,
            ),
          ),
        ],
      ),
    );
  }
}