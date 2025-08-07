import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../constants/app_theme.dart';
import '../services/unified_auth_service.dart';
import '../services/user_service.dart';
import '../widgets/stylish_loading.dart';

class AccountSettingsScreen extends StatefulWidget {
  @override
  _AccountSettingsScreenState createState() => _AccountSettingsScreenState();
}

class _AccountSettingsScreenState extends State<AccountSettingsScreen> {
  final _formKey = GlobalKey<FormState>();
  final _currentPasswordController = TextEditingController();
  final _newPasswordController = TextEditingController();
  final _confirmPasswordController = TextEditingController();
  final _emailController = TextEditingController();
  final _fullNameController = TextEditingController();

  bool _isLoading = true;
  bool _isChangingPassword = false;
  bool _isUpdatingProfile = false;
  bool _obscureCurrentPassword = true;
  bool _obscureNewPassword = true;
  bool _obscureConfirmPassword = true;
  bool _notificationsEnabled = true;
  bool _emailNotifications = true;
  bool _pushNotifications = true;
  bool _dataBackup = true;

  String _currentEmail = '';
  String _currentFullName = '';

  @override
  void initState() {
    super.initState();
    _loadUserData();
  }

  @override
  void dispose() {
    _currentPasswordController.dispose();
    _newPasswordController.dispose();
    _confirmPasswordController.dispose();
    _emailController.dispose();
    _fullNameController.dispose();
    super.dispose();
  }

  Future<void> _loadUserData() async {
    try {
      final fullName = await UserService.getFullName();
      final userData = await UserService.getUserData();
      final email = userData?['model']?['email'] ?? '';

      setState(() {
        _currentFullName = fullName;
        _currentEmail = email;
        _fullNameController.text = fullName;
        _emailController.text = email;
        _isLoading = false;
      });
    } catch (e) {
      print('Error loading user data: $e');
      setState(() {
        _isLoading = false;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    if (_isLoading) {
      return Scaffold(
        backgroundColor: AppTheme.backgroundPrimary,
        body: SafeArea(
          child: Column(
            children: [
              _buildHeader(),
              Expanded(
                child: StylishLoading(
                  message: 'Loading account settings...',
                ),
              ),
            ],
          ),
        ),
      );
    }

    return Scaffold(
      backgroundColor: AppTheme.backgroundPrimary,
      body: SafeArea(
        child: Column(
          children: [
            _buildHeader(),
            Expanded(
              child: SingleChildScrollView(
                physics: BouncingScrollPhysics(),
                padding: EdgeInsets.all(20.w),
                child: Form(
                  key: _formKey,
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      _buildProfileSection(),
                      SizedBox(height: 24.h),
                      _buildSecuritySection(),
                      SizedBox(height: 24.h),
                      _buildNotificationSection(),
                      SizedBox(height: 24.h),
                      _buildPrivacySection(),
                      SizedBox(height: 24.h),
                      _buildDangerZoneSection(),
                      SizedBox(height: 40.h),
                    ],
                  ),
                ),
              ),
            ),
          ],
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
                  color: AppTheme.borderPrimary,
                ),
              ),
              child: Icon(
                Icons.arrow_back,
                color: AppTheme.textPrimary,
                size: 20.sp,
              ),
            ),
          ),
          SizedBox(width: 16.w),
          Text(
            'Account Settings',
            style: TextStyle(
              fontSize: 24.sp,
              fontWeight: FontWeight.bold,
              color: AppTheme.textPrimary,
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildProfileSection() {
    return _buildSection(
      title: 'Profile Information',
      child: Column(
        children: [
          _buildTextField(
            controller: _fullNameController,
            label: 'Full Name',
            icon: Icons.person_outline,
            validator: (value) {
              if (value == null || value.trim().isEmpty) {
                return 'Full name is required';
              }
              return null;
            },
          ),
          SizedBox(height: 16.h),
          _buildTextField(
            controller: _emailController,
            label: 'Email Address',
            icon: Icons.email_outlined,
            keyboardType: TextInputType.emailAddress,
            validator: (value) {
              if (value == null || value.trim().isEmpty) {
                return 'Email is required';
              }
              if (!RegExp(r'^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$').hasMatch(value.trim())) {
                return 'Please enter a valid email address';
              }
              return null;
            },
          ),
          SizedBox(height: 20.h),
          SizedBox(
            width: double.infinity,
            child: ElevatedButton(
              onPressed: _isUpdatingProfile ? null : _updateProfile,
              style: ElevatedButton.styleFrom(
                backgroundColor: AppTheme.affirmationGradient[0],
                foregroundColor: AppTheme.white,
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(12.r),
                ),
                padding: EdgeInsets.symmetric(vertical: 16.h),
              ),
              child: _isUpdatingProfile
                  ? Row(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [
                        SizedBox(
                          width: 20.w,
                          height: 20.h,
                          child: CircularProgressIndicator(
                            color: AppTheme.white,
                            strokeWidth: 2,
                          ),
                        ),
                        SizedBox(width: 12.w),
                        Text(
                          'Updating...',
                          style: TextStyle(
                            fontSize: 16.sp,
                            fontWeight: FontWeight.w600,
                          ),
                        ),
                      ],
                    )
                  : Text(
                      'Update Profile',
                      style: TextStyle(
                        fontSize: 16.sp,
                        fontWeight: FontWeight.w600,
                      ),
                    ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildSecuritySection() {
    return _buildSection(
      title: 'Security',
      child: Column(
        children: [
          _buildTextField(
            controller: _currentPasswordController,
            label: 'Current Password',
            icon: Icons.lock_outline,
            obscureText: _obscureCurrentPassword,
            suffixIcon: IconButton(
              icon: Icon(
                _obscureCurrentPassword ? Icons.visibility_off : Icons.visibility,
                color: AppTheme.textSecondary,
              ),
              onPressed: () {
                setState(() {
                  _obscureCurrentPassword = !_obscureCurrentPassword;
                });
              },
            ),
            validator: (value) {
              if (_newPasswordController.text.isNotEmpty && (value == null || value.isEmpty)) {
                return 'Current password is required to change password';
              }
              return null;
            },
          ),
          SizedBox(height: 16.h),
          _buildTextField(
            controller: _newPasswordController,
            label: 'New Password',
            icon: Icons.lock_outline,
            obscureText: _obscureNewPassword,
            suffixIcon: IconButton(
              icon: Icon(
                _obscureNewPassword ? Icons.visibility_off : Icons.visibility,
                color: AppTheme.textSecondary,
              ),
              onPressed: () {
                setState(() {
                  _obscureNewPassword = !_obscureNewPassword;
                });
              },
            ),
            validator: (value) {
              if (_currentPasswordController.text.isNotEmpty && (value == null || value.length < 6)) {
                return 'New password must be at least 6 characters';
              }
              return null;
            },
          ),
          SizedBox(height: 16.h),
          _buildTextField(
            controller: _confirmPasswordController,
            label: 'Confirm New Password',
            icon: Icons.lock_outline,
            obscureText: _obscureConfirmPassword,
            suffixIcon: IconButton(
              icon: Icon(
                _obscureConfirmPassword ? Icons.visibility_off : Icons.visibility,
                color: AppTheme.textSecondary,
              ),
              onPressed: () {
                setState(() {
                  _obscureConfirmPassword = !_obscureConfirmPassword;
                });
              },
            ),
            validator: (value) {
              if (_newPasswordController.text.isNotEmpty && value != _newPasswordController.text) {
                return 'Passwords do not match';
              }
              return null;
            },
          ),
          SizedBox(height: 20.h),
          SizedBox(
            width: double.infinity,
            child: ElevatedButton(
              onPressed: _isChangingPassword ? null : _changePassword,
              style: ElevatedButton.styleFrom(
                backgroundColor: AppTheme.goalTrackerGradient[0],
                foregroundColor: AppTheme.white,
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(12.r),
                ),
                padding: EdgeInsets.symmetric(vertical: 16.h),
              ),
              child: _isChangingPassword
                  ? Row(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [
                        SizedBox(
                          width: 20.w,
                          height: 20.h,
                          child: CircularProgressIndicator(
                            color: AppTheme.white,
                            strokeWidth: 2,
                          ),
                        ),
                        SizedBox(width: 12.w),
                        Text(
                          'Changing Password...',
                          style: TextStyle(
                            fontSize: 16.sp,
                            fontWeight: FontWeight.w600,
                          ),
                        ),
                      ],
                    )
                  : Text(
                      'Change Password',
                      style: TextStyle(
                        fontSize: 16.sp,
                        fontWeight: FontWeight.w600,
                      ),
                    ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildNotificationSection() {
    return _buildSection(
      title: 'Notifications',
      child: Column(
        children: [
          _buildSwitchTile(
            title: 'Push Notifications',
            subtitle: 'Receive notifications on this device',
            value: _pushNotifications,
            onChanged: (value) {
              setState(() {
                _pushNotifications = value;
              });
            },
          ),
          _buildDivider(),
          _buildSwitchTile(
            title: 'Email Notifications',
            subtitle: 'Receive emails about your progress',
            value: _emailNotifications,
            onChanged: (value) {
              setState(() {
                _emailNotifications = value;
              });
            },
          ),
          _buildDivider(),
          _buildSwitchTile(
            title: 'Daily Reminders',
            subtitle: 'Get reminded to practice mindfulness',
            value: _notificationsEnabled,
            onChanged: (value) {
              setState(() {
                _notificationsEnabled = value;
              });
            },
          ),
        ],
      ),
    );
  }

  Widget _buildPrivacySection() {
    return _buildSection(
      title: 'Privacy & Data',
      child: Column(
        children: [
          _buildSwitchTile(
            title: 'Data Backup',
            subtitle: 'Automatically backup your data to cloud',
            value: _dataBackup,
            onChanged: (value) {
              setState(() {
                _dataBackup = value;
              });
            },
          ),
          _buildDivider(),
          _buildActionTile(
            title: 'Export Data',
            subtitle: 'Download your data in JSON format',
            icon: Icons.download_outlined,
            onTap: _exportData,
          ),
          _buildDivider(),
          _buildActionTile(
            title: 'Privacy Policy',
            subtitle: 'Read our privacy policy',
            icon: Icons.privacy_tip_outlined,
            onTap: () {
              // TODO: Navigate to privacy policy
              _showInfoDialog('Privacy Policy', 'This would open the privacy policy document.');
            },
          ),
        ],
      ),
    );
  }

  Widget _buildDangerZoneSection() {
    return _buildSection(
      title: 'Danger Zone',
      titleColor: Colors.red,
      child: Column(
        children: [
          _buildActionTile(
            title: 'Delete Account',
            subtitle: 'Permanently delete your account and all data',
            icon: Icons.delete_forever_outlined,
            titleColor: Colors.red,
            onTap: _showDeleteAccountDialog,
          ),
        ],
      ),
    );
  }

  Widget _buildSection({
    required String title,
    required Widget child,
    Color? titleColor,
  }) {
    return Container(
      decoration: BoxDecoration(
        color: AppTheme.cardBackground,
        borderRadius: BorderRadius.circular(16.r),
        border: Border.all(
          color: AppTheme.borderSecondary,
        ),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Padding(
            padding: EdgeInsets.all(20.w),
            child: Text(
              title,
              style: TextStyle(
                fontSize: 18.sp,
                fontWeight: FontWeight.w600,
                color: titleColor ?? AppTheme.textPrimary,
              ),
            ),
          ),
          Container(
            height: 1,
            color: AppTheme.divider,
          ),
          Padding(
            padding: EdgeInsets.all(20.w),
            child: child,
          ),
        ],
      ),
    );
  }

  Widget _buildTextField({
    required TextEditingController controller,
    required String label,
    required IconData icon,
    String? Function(String?)? validator,
    TextInputType keyboardType = TextInputType.text,
    bool obscureText = false,
    Widget? suffixIcon,
  }) {
    return TextFormField(
      controller: controller,
      validator: validator,
      keyboardType: keyboardType,
      obscureText: obscureText,
      style: TextStyle(
        color: AppTheme.textPrimary,
        fontSize: 16.sp,
      ),
      decoration: InputDecoration(
        labelText: label,
        labelStyle: TextStyle(
          color: AppTheme.textSecondary,
          fontSize: 14.sp,
        ),
        prefixIcon: Icon(
          icon,
          color: AppTheme.textSecondary,
          size: 20.sp,
        ),
        suffixIcon: suffixIcon,
        filled: true,
        fillColor: AppTheme.backgroundSecondary,
        border: OutlineInputBorder(
          borderRadius: BorderRadius.circular(12.r),
          borderSide: BorderSide(
            color: AppTheme.borderSecondary,
          ),
        ),
        enabledBorder: OutlineInputBorder(
          borderRadius: BorderRadius.circular(12.r),
          borderSide: BorderSide(
            color: AppTheme.borderSecondary,
          ),
        ),
        focusedBorder: OutlineInputBorder(
          borderRadius: BorderRadius.circular(12.r),
          borderSide: BorderSide(
            color: AppTheme.affirmationGradient[0],
            width: 2,
          ),
        ),
        errorBorder: OutlineInputBorder(
          borderRadius: BorderRadius.circular(12.r),
          borderSide: BorderSide(
            color: Colors.red,
          ),
        ),
        focusedErrorBorder: OutlineInputBorder(
          borderRadius: BorderRadius.circular(12.r),
          borderSide: BorderSide(
            color: Colors.red,
            width: 2,
          ),
        ),
      ),
    );
  }

  Widget _buildSwitchTile({
    required String title,
    required String subtitle,
    required bool value,
    required ValueChanged<bool> onChanged,
  }) {
    return ListTile(
      contentPadding: EdgeInsets.zero,
      title: Text(
        title,
        style: TextStyle(
          fontSize: 16.sp,
          fontWeight: FontWeight.w600,
          color: AppTheme.textPrimary,
        ),
      ),
      subtitle: Text(
        subtitle,
        style: TextStyle(
          fontSize: 14.sp,
          color: AppTheme.textSecondary,
        ),
      ),
      trailing: Switch(
        value: value,
        onChanged: onChanged,
        activeColor: AppTheme.affirmationGradient[0],
        activeTrackColor: AppTheme.affirmationGradient[0].withOpacity(0.3),
        inactiveThumbColor: AppTheme.textTertiary,
        inactiveTrackColor: AppTheme.textTertiary.withOpacity(0.2),
      ),
    );
  }

  Widget _buildActionTile({
    required String title,
    required String subtitle,
    required IconData icon,
    required VoidCallback onTap,
    Color? titleColor,
  }) {
    return ListTile(
      contentPadding: EdgeInsets.zero,
      leading: Container(
        width: 44.w,
        height: 44.h,
        decoration: BoxDecoration(
          color: (titleColor ?? AppTheme.textSecondary).withOpacity(0.1),
          borderRadius: BorderRadius.circular(12.r),
        ),
        child: Icon(
          icon,
          color: titleColor ?? AppTheme.textSecondary,
          size: 22.sp,
        ),
      ),
      title: Text(
        title,
        style: TextStyle(
          fontSize: 16.sp,
          fontWeight: FontWeight.w600,
          color: titleColor ?? AppTheme.textPrimary,
        ),
      ),
      subtitle: Text(
        subtitle,
        style: TextStyle(
          fontSize: 14.sp,
          color: AppTheme.textSecondary,
        ),
      ),
      trailing: Icon(
        Icons.chevron_right,
        color: AppTheme.textTertiary,
        size: 20.sp,
      ),
      onTap: onTap,
    );
  }

  Widget _buildDivider() {
    return Container(
      height: 1,
      margin: EdgeInsets.symmetric(vertical: 12.h),
      color: AppTheme.divider,
    );
  }

  Future<void> _updateProfile() async {
    if (!_formKey.currentState!.validate()) return;

    setState(() {
      _isUpdatingProfile = true;
    });

    try {
      // TODO: Implement profile update API call
      await Future.delayed(Duration(seconds: 2)); // Simulate API call

      setState(() {
        _currentFullName = _fullNameController.text.trim();
        _currentEmail = _emailController.text.trim();
      });

      _showSuccessSnackBar('Profile updated successfully');
    } catch (e) {
      print('Error updating profile: $e');
      _showErrorSnackBar('Failed to update profile');
    } finally {
      setState(() {
        _isUpdatingProfile = false;
      });
    }
  }

  Future<void> _changePassword() async {
    if (!_formKey.currentState!.validate()) return;

    if (_currentPasswordController.text.isEmpty ||
        _newPasswordController.text.isEmpty ||
        _confirmPasswordController.text.isEmpty) {
      _showErrorSnackBar('Please fill in all password fields');
      return;
    }

    setState(() {
      _isChangingPassword = true;
    });

    try {
      // TODO: Implement password change API call
      await Future.delayed(Duration(seconds: 2)); // Simulate API call

      // Clear password fields
      _currentPasswordController.clear();
      _newPasswordController.clear();
      _confirmPasswordController.clear();

      _showSuccessSnackBar('Password changed successfully');
    } catch (e) {
      print('Error changing password: $e');
      _showErrorSnackBar('Failed to change password');
    } finally {
      setState(() {
        _isChangingPassword = false;
      });
    }
  }

  Future<void> _exportData() async {
    try {
      // TODO: Implement data export
      _showInfoDialog('Data Export', 'Your data export will be available shortly. Check your email for download link.');
    } catch (e) {
      print('Error exporting data: $e');
      _showErrorSnackBar('Failed to export data');
    }
  }

  void _showDeleteAccountDialog() {
    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        backgroundColor: AppTheme.cardBackground,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(16.r),
        ),
        title: Text(
          'Delete Account',
          style: TextStyle(
            color: Colors.red,
            fontSize: 20.sp,
            fontWeight: FontWeight.bold,
          ),
        ),
        content: Column(
          mainAxisSize: MainAxisSize.min,
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              'This action cannot be undone. This will permanently delete your account and all associated data including:',
              style: TextStyle(
                color: AppTheme.textSecondary,
                fontSize: 16.sp,
              ),
            ),
            SizedBox(height: 12.h),
            ...['• All your journal entries', '• Vision boards and goals', '• Progress and statistics', '• Account information']
                .map((item) => Padding(
                      padding: EdgeInsets.symmetric(vertical: 2.h),
                      child: Text(
                        item,
                        style: TextStyle(
                          color: AppTheme.textSecondary,
                          fontSize: 14.sp,
                        ),
                      ),
                    )),
            SizedBox(height: 16.h),
            Text(
              'Are you absolutely sure?',
              style: TextStyle(
                color: Colors.red,
                fontSize: 16.sp,
                fontWeight: FontWeight.w600,
              ),
            ),
          ],
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context),
            child: Text(
              'Cancel',
              style: TextStyle(
                color: AppTheme.textSecondary,
                fontSize: 16.sp,
              ),
            ),
          ),
          ElevatedButton(
            onPressed: () {
              Navigator.pop(context);
              _deleteAccount();
            },
            style: ElevatedButton.styleFrom(
              backgroundColor: Colors.red,
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(12.r),
              ),
            ),
            child: Text(
              'Delete Account',
              style: TextStyle(
                color: AppTheme.white,
                fontSize: 16.sp,
                fontWeight: FontWeight.w600,
              ),
            ),
          ),
        ],
      ),
    );
  }

  Future<void> _deleteAccount() async {
    try {
      // TODO: Implement account deletion API call
      _showInfoDialog('Account Deleted', 'Your account has been scheduled for deletion. You will be logged out shortly.');
    } catch (e) {
      print('Error deleting account: $e');
      _showErrorSnackBar('Failed to delete account');
    }
  }

  void _showSuccessSnackBar(String message) {
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text(message),
        backgroundColor: Colors.green,
        behavior: SnackBarBehavior.floating,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(12.r),
        ),
      ),
    );
  }

  void _showErrorSnackBar(String message) {
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text(message),
        backgroundColor: Colors.red,
        behavior: SnackBarBehavior.floating,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(12.r),
        ),
      ),
    );
  }

  void _showInfoDialog(String title, String message) {
    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        backgroundColor: AppTheme.cardBackground,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(16.r),
        ),
        title: Text(
          title,
          style: TextStyle(
            color: AppTheme.textPrimary,
            fontSize: 20.sp,
            fontWeight: FontWeight.bold,
          ),
        ),
        content: Text(
          message,
          style: TextStyle(
            color: AppTheme.textSecondary,
            fontSize: 16.sp,
          ),
        ),
        actions: [
          ElevatedButton(
            onPressed: () => Navigator.pop(context),
            style: ElevatedButton.styleFrom(
              backgroundColor: AppTheme.affirmationGradient[0],
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(12.r),
              ),
            ),
            child: Text(
              'OK',
              style: TextStyle(
                color: AppTheme.white,
                fontSize: 16.sp,
                fontWeight: FontWeight.w600,
              ),
            ),
          ),
        ],
      ),
    );
  }
}