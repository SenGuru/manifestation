import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../constants/colors.dart';
import '../constants/app_theme.dart';
import 'dart:math' as math;

class EnergyMeter extends StatefulWidget {
  @override
  _EnergyMeterState createState() => _EnergyMeterState();
}

class _EnergyMeterState extends State<EnergyMeter>
    with SingleTickerProviderStateMixin {
  late AnimationController _pulseController;
  late Animation<double> _pulseAnimation;
  double _energyPercentage = 0.0;

  @override
  void initState() {
    super.initState();
    _pulseController = AnimationController(
      duration: Duration(milliseconds: 2000),
      vsync: this,
    );
    
    _pulseAnimation = Tween<double>(
      begin: 1.0,
      end: 1.08,
    ).animate(CurvedAnimation(
      parent: _pulseController,
      curve: Curves.easeInOut,
    ));
    
    _calculateEnergyLevel();
    _pulseController.repeat(reverse: true);
  }

  void _calculateEnergyLevel() {
    final now = DateTime.now();
    final startOfDay = DateTime(now.year, now.month, now.day);
    final endOfDay = startOfDay.add(Duration(days: 1));
    
    final totalMinutesInDay = 24 * 60;
    final minutesSinceStart = now.difference(startOfDay).inMinutes;
    
    setState(() {
      _energyPercentage = (minutesSinceStart / totalMinutesInDay).clamp(0.0, 1.0);
    });
  }

  Color _getEnergyColor() {
    if (_energyPercentage <= 0.33) {
      // Red spectrum
      return Color.lerp(Color(0xFFFF3B30), Color(0xFFFF6B47), _energyPercentage * 3)!;
    } else if (_energyPercentage <= 0.66) {
      // Orange spectrum
      final normalizedProgress = (_energyPercentage - 0.33) * 3;
      return Color.lerp(Color(0xFFFF9500), Color(0xFFFFB347), normalizedProgress)!;
    } else {
      // Green spectrum
      final normalizedProgress = (_energyPercentage - 0.66) * 3;
      return Color.lerp(Color(0xFF34C759), Color(0xFF30D158), normalizedProgress)!;
    }
  }

  String _getEnergyLabel() {
    if (_energyPercentage <= 0.33) {
      return 'Building';
    } else if (_energyPercentage <= 0.66) {
      return 'Rising';
    } else {
      return 'Peak Energy';
    }
  }

  @override
  void dispose() {
    _pulseController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: EdgeInsets.all(20.w),
      decoration: BoxDecoration(
        color: AppTheme.cardBackground,
        borderRadius: BorderRadius.circular(20.r),
        border: Border.all(
          color: AppTheme.borderSecondary,
          width: 1,
        ),
        boxShadow: [
          BoxShadow(
            color: _getEnergyColor().withOpacity(0.1),
            blurRadius: 20.r,
            offset: Offset(0, 8.h),
          ),
        ],
      ),
      child: Column(
        children: [
          // Energy meter circle
          AnimatedBuilder(
            animation: _pulseAnimation,
            builder: (context, child) {
              return Transform.scale(
                scale: _energyPercentage > 0.8 ? _pulseAnimation.value : 1.0,
                child: Container(
                  width: 80.w,
                  height: 80.h,
                  child: Stack(
                    alignment: Alignment.center,
                    children: [
                      // Background circle
                      Container(
                        width: 80.w,
                        height: 80.h,
                        child: CircularProgressIndicator(
                          value: 1.0,
                          strokeWidth: 6.w,
                          backgroundColor: AppTheme.textTertiary.withValues(alpha: 0.1),
                          valueColor: AlwaysStoppedAnimation<Color>(
                            AppTheme.textTertiary.withValues(alpha: 0.1),
                          ),
                        ),
                      ),
                      
                      // Progress circle
                      Container(
                        width: 80.w,
                        height: 80.h,
                        child: CircularProgressIndicator(
                          value: _energyPercentage,
                          strokeWidth: 6.w,
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
                              fontSize: 18.sp,
                              fontWeight: FontWeight.bold,
                              color: _getEnergyColor(),
                            ),
                          ),
                          SizedBox(height: 2.h),
                          Container(
                            width: 6.w,
                            height: 6.h,
                            decoration: BoxDecoration(
                              color: _getEnergyColor(),
                              shape: BoxShape.circle,
                              boxShadow: [
                                BoxShadow(
                                  color: _getEnergyColor().withOpacity(0.5),
                                  blurRadius: 4.r,
                                  spreadRadius: 1.r,
                                ),
                              ],
                            ),
                          ),
                        ],
                      ),
                    ],
                  ),
                ),
              );
            },
          ),
          
          SizedBox(height: 16.h),
          
          // Energy label
          Text(
            _getEnergyLabel(),
            style: TextStyle(
              fontSize: 16.sp,
              fontWeight: FontWeight.w600,
              color: AppTheme.textPrimary,
            ),
          ),
          
          SizedBox(height: 4.h),
          
          // Subtext
          Text(
            'Daily Energy',
            style: TextStyle(
              fontSize: 12.sp,
              color: AppTheme.textSecondary,
            ),
          ),
        ],
      ),
    );
  }
}