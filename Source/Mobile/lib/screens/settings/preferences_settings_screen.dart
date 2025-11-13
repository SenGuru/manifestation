import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../../constants/app_theme.dart';
import '../../widgets/universal_back_button.dart';

class PreferencesSettingsScreen extends StatefulWidget {
  @override
  _PreferencesSettingsScreenState createState() => _PreferencesSettingsScreenState();
}

class _PreferencesSettingsScreenState extends State<PreferencesSettingsScreen> {
  bool _pushNotifications = true;
  bool _emailNotifications = true;
  bool _dataBackup = true;
  bool _meditationReminders = true;
  bool _weeklyProgress = false;
  bool _appSounds = true;
  bool _vibration = true;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppTheme.backgroundDark,
      body: SafeArea(
        child: SingleChildScrollView(
          physics: BouncingScrollPhysics(),
          padding: EdgeInsets.symmetric(horizontal: 24.w),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              SizedBox(height: 16.h),
              _buildBackButtonWithHeader(),
              SizedBox(height: 18.h),
              _buildPreferencesContent(),
              SizedBox(height: 40.h),
            ],
          ),
        ),
      ),
    );
  }

  Widget _buildBackButtonWithHeader() {
    return Row(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        UniversalBackButton(),
        SizedBox(width: 16.w),
        Expanded(
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(
                'Preferences',
                style: TextStyle(
                  fontSize: 28.sp,
                  fontWeight: FontWeight.bold,
                  color: AppTheme.textLight,
                ),
              ),
              SizedBox(height: 4.h),
              Text(
                'Customize your app experience',
                style: TextStyle(
                  fontSize: 14.sp,
                  color: AppTheme.textMedium,
                  height: 1.3,
                ),
              ),
            ],
          ),
        ),
      ],
    );
  }

  Widget _buildPreferencesContent() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        // Notifications section
        Text(
          'Notifications',
          style: TextStyle(
            fontSize: 18.sp,
            fontWeight: FontWeight.bold,
            color: AppTheme.textLight,
          ),
        ),
        
        SizedBox(height: 16.h),
        
        _buildPreferenceSwitch(
          title: 'Push Notifications',
          subtitle: 'Receive app notifications',
          value: _pushNotifications,
          onChanged: (value) {
            setState(() {
              _pushNotifications = value;
            });
          },
        ),
        
        SizedBox(height: 16.h),
        
        _buildPreferenceSwitch(
          title: 'Email Notifications',
          subtitle: 'Get updates via email',
          value: _emailNotifications,
          onChanged: (value) {
            setState(() {
              _emailNotifications = value;
            });
          },
        ),
        
        SizedBox(height: 16.h),
        
        _buildPreferenceSwitch(
          title: 'Meditation Reminders',
          subtitle: 'Daily meditation session reminders',
          value: _meditationReminders,
          onChanged: (value) {
            setState(() {
              _meditationReminders = value;
            });
          },
        ),
        
        SizedBox(height: 16.h),
        
        _buildPreferenceSwitch(
          title: 'Weekly Progress Report',
          subtitle: 'Receive weekly summaries of your progress',
          value: _weeklyProgress,
          onChanged: (value) {
            setState(() {
              _weeklyProgress = value;
            });
          },
        ),
        
        SizedBox(height: 32.h),
        
        // Data & Privacy section
        Text(
          'Data & Privacy',
          style: TextStyle(
            fontSize: 18.sp,
            fontWeight: FontWeight.bold,
            color: AppTheme.textLight,
          ),
        ),
        
        SizedBox(height: 16.h),
        
        _buildPreferenceSwitch(
          title: 'Data Backup',
          subtitle: 'Sync your progress to cloud',
          value: _dataBackup,
          onChanged: (value) {
            setState(() {
              _dataBackup = value;
            });
          },
        ),
        
        SizedBox(height: 32.h),
        
        // App Experience section
        Text(
          'App Experience',
          style: TextStyle(
            fontSize: 18.sp,
            fontWeight: FontWeight.bold,
            color: AppTheme.textLight,
          ),
        ),
        
        SizedBox(height: 16.h),
        
        _buildPreferenceSwitch(
          title: 'App Sounds',
          subtitle: 'Enable sound effects and audio feedback',
          value: _appSounds,
          onChanged: (value) {
            setState(() {
              _appSounds = value;
            });
          },
        ),
        
        SizedBox(height: 16.h),
        
        _buildPreferenceSwitch(
          title: 'Vibration',
          subtitle: 'Haptic feedback for interactions',
          value: _vibration,
          onChanged: (value) {
            setState(() {
              _vibration = value;
            });
          },
        ),
      ],
    );
  }

  Widget _buildPreferenceSwitch({
    required String title,
    required String subtitle,
    required bool value,
    required ValueChanged<bool> onChanged,
  }) {
    return GestureDetector(
      onTap: () => onChanged(!value),
      child: Row(
        children: [
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
                SizedBox(height: 4.h),
                Text(
                  subtitle,
                  style: TextStyle(
                    fontSize: 14.sp,
                    color: AppTheme.textMedium,
                  ),
                ),
              ],
            ),
          ),
          Transform.scale(
            scale: 0.8,
            child: Switch.adaptive(
              value: value,
              onChanged: onChanged,
              activeColor: AppTheme.socialGradient[0],
              activeTrackColor: AppTheme.socialGradient[0].withOpacity(0.3),
              inactiveThumbColor: AppTheme.textMedium,
              inactiveTrackColor: AppTheme.textMedium.withOpacity(0.2),
            ),
          ),
        ],
      ),
    );
  }
}