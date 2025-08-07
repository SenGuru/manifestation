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
  bool _pushNotifications = true;
  bool _emailNotifications = true;
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
        _currentFullName = 'User';
        _currentEmail = 'user@example.com';
        _fullNameController.text = 'User';
        _emailController.text = 'user@example.com';
        _isLoading = false;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    if (_isLoading) {
      return Scaffold(
        backgroundColor: AppTheme.backgroundDark,
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
      backgroundColor: AppTheme.backgroundDark,
      body: SafeArea(
        child: Column(
          children: [
            _buildHeader(),
            Expanded(
              child: SingleChildScrollView(
                physics: BouncingScrollPhysics(),
                padding: EdgeInsets.symmetric(horizontal: 24.w),
                child: Form(
                  key: _formKey,
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      SizedBox(height: 20.h),
                      _buildProfileSection(),
                      SizedBox(height: 32.h),
                      _buildSecuritySection(),
                      SizedBox(height: 32.h),
                      _buildPreferencesSection(),
                      SizedBox(height: 32.h),
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
                  color: AppTheme.textMedium.withOpacity(0.1),
                  width: 2,
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
          Text(
            'Account Settings',
            style: TextStyle(
              fontSize: 24.sp,
              fontWeight: FontWeight.bold,
              color: AppTheme.textLight,
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildProfileSection() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Profile Information',
          style: TextStyle(
            fontSize: 20.sp,
            fontWeight: FontWeight.bold,
            color: AppTheme.textLight,
          ),
        ),
        SizedBox(height: 16.h),
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
        _buildPrimaryButton(
          text: 'Update Profile',
          onPressed: _updateProfile,
          isLoading: _isUpdatingProfile,
        ),
      ],
    );
  }

  Widget _buildSecuritySection() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Security',
          style: TextStyle(
            fontSize: 20.sp,
            fontWeight: FontWeight.bold,
            color: AppTheme.textLight,
          ),
        ),
        SizedBox(height: 16.h),
        _buildTextField(
          controller: _currentPasswordController,
          label: 'Current Password',
          icon: Icons.lock_outline,
          isPassword: true,
          obscureText: _obscureCurrentPassword,
          suffixIcon: GestureDetector(
            onTap: () {
              setState(() {
                _obscureCurrentPassword = !_obscureCurrentPassword;
              });
            },
            child: Icon(
              _obscureCurrentPassword ? Icons.visibility_outlined : Icons.visibility_off_outlined,
              color: AppTheme.textMedium,
              size: 20.sp,
            ),
          ),
          validator: (value) {
            if (_newPasswordController.text.isNotEmpty && (value == null || value.isEmpty)) {
              return 'Current password is required';
            }
            return null;
          },
        ),
        SizedBox(height: 16.h),
        _buildTextField(
          controller: _newPasswordController,
          label: 'New Password',
          icon: Icons.lock_reset,
          isPassword: true,
          obscureText: _obscureNewPassword,
          suffixIcon: GestureDetector(
            onTap: () {
              setState(() {
                _obscureNewPassword = !_obscureNewPassword;
              });
            },
            child: Icon(
              _obscureNewPassword ? Icons.visibility_outlined : Icons.visibility_off_outlined,
              color: AppTheme.textMedium,
              size: 20.sp,
            ),
          ),
          validator: (value) {
            if (value != null && value.isNotEmpty && value.length < 6) {
              return 'Password must be at least 6 characters';
            }
            return null;
          },
        ),
        SizedBox(height: 16.h),
        _buildTextField(
          controller: _confirmPasswordController,
          label: 'Confirm New Password',
          icon: Icons.lock_outline,
          isPassword: true,
          obscureText: _obscureConfirmPassword,
          suffixIcon: GestureDetector(
            onTap: () {
              setState(() {
                _obscureConfirmPassword = !_obscureConfirmPassword;
              });
            },
            child: Icon(
              _obscureConfirmPassword ? Icons.visibility_outlined : Icons.visibility_off_outlined,
              color: AppTheme.textMedium,
              size: 20.sp,
            ),
          ),
          validator: (value) {
            if (_newPasswordController.text.isNotEmpty) {
              if (value == null || value.isEmpty) {
                return 'Please confirm your new password';
              }
              if (value != _newPasswordController.text) {
                return 'Passwords do not match';
              }
            }
            return null;
          },
        ),
        SizedBox(height: 20.h),
        _buildPrimaryButton(
          text: 'Change Password',
          onPressed: _changePassword,
          isLoading: _isChangingPassword,
        ),
      ],
    );
  }

  Widget _buildPreferencesSection() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Preferences',
          style: TextStyle(
            fontSize: 20.sp,
            fontWeight: FontWeight.bold,
            color: AppTheme.textLight,
          ),
        ),
        SizedBox(height: 16.h),
        _buildSimpleSwitch(
          title: 'Push Notifications',
          subtitle: 'Receive meditation reminders',
          value: _pushNotifications,
          onChanged: (value) {
            setState(() {
              _pushNotifications = value;
            });
          },
        ),
        SizedBox(height: 16.h),
        _buildSimpleSwitch(
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
        _buildSimpleSwitch(
          title: 'Data Backup',
          subtitle: 'Sync your progress to cloud',
          value: _dataBackup,
          onChanged: (value) {
            setState(() {
              _dataBackup = value;
            });
          },
        ),
      ],
    );
  }

  Widget _buildDangerZoneSection() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Account Management',
          style: TextStyle(
            fontSize: 20.sp,
            fontWeight: FontWeight.bold,
            color: AppTheme.textLight,
          ),
        ),
        SizedBox(height: 16.h),
        _buildDangerButton(
          text: 'Sign Out',
          icon: Icons.logout,
          onPressed: _signOut,
        ),
        SizedBox(height: 16.h),
        _buildDangerButton(
          text: 'Delete Account',
          icon: Icons.delete_forever,
          onPressed: _deleteAccount,
          isDangerous: true,
        ),
      ],
    );
  }

  Widget _buildTextField({
    required TextEditingController controller,
    required String label,
    required IconData icon,
    String? Function(String?)? validator,
    TextInputType? keyboardType,
    bool isPassword = false,
    bool obscureText = false,
    Widget? suffixIcon,
  }) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          label,
          style: TextStyle(
            fontSize: 16.sp,
            fontWeight: FontWeight.w600,
            color: AppTheme.textLight,
          ),
        ),
        SizedBox(height: 8.h),
        TextFormField(
          controller: controller,
          validator: validator,
          keyboardType: keyboardType,
          obscureText: obscureText,
          style: TextStyle(
            color: AppTheme.textLight,
            fontSize: 16.sp,
          ),
          decoration: InputDecoration(
            prefixIcon: Icon(
              icon,
              color: AppTheme.textMedium,
              size: 20.sp,
            ),
            suffixIcon: suffixIcon,
            filled: true,
            fillColor: AppTheme.cardBackground,
            border: OutlineInputBorder(
              borderRadius: BorderRadius.circular(16.r),
              borderSide: BorderSide.none,
            ),
            enabledBorder: OutlineInputBorder(
              borderRadius: BorderRadius.circular(16.r),
              borderSide: BorderSide(
                color: AppTheme.textMedium.withOpacity(0.1),
                width: 2,
              ),
            ),
            focusedBorder: OutlineInputBorder(
              borderRadius: BorderRadius.circular(16.r),
              borderSide: BorderSide(
                color: AppTheme.accentLavender,
                width: 2,
              ),
            ),
            errorBorder: OutlineInputBorder(
              borderRadius: BorderRadius.circular(16.r),
              borderSide: BorderSide(
                color: AppTheme.error,
                width: 2,
              ),
            ),
            focusedErrorBorder: OutlineInputBorder(
              borderRadius: BorderRadius.circular(16.r),
              borderSide: BorderSide(
                color: AppTheme.error,
                width: 2,
              ),
            ),
            contentPadding: EdgeInsets.symmetric(
              horizontal: 16.w,
              vertical: 16.h,
            ),
          ),
        ),
      ],
    );
  }

  Widget _buildSimpleSwitch({
    required String title,
    required String subtitle,
    required bool value,
    required ValueChanged<bool> onChanged,
  }) {
    return Row(
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
        Switch.adaptive(
          value: value,
          onChanged: onChanged,
          activeColor: AppTheme.accentLavender,
          activeTrackColor: AppTheme.accentLavender.withOpacity(0.3),
          inactiveThumbColor: AppTheme.textMedium,
          inactiveTrackColor: AppTheme.textMedium.withOpacity(0.2),
        ),
      ],
    );
  }

  Widget _buildPrimaryButton({
    required String text,
    required VoidCallback? onPressed,
    bool isLoading = false,
  }) {
    return SizedBox(
      width: double.infinity,
      height: 54.h,
      child: ElevatedButton(
        onPressed: isLoading ? null : onPressed,
        style: ElevatedButton.styleFrom(
          backgroundColor: Colors.transparent,
          shadowColor: Colors.transparent,
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(16.r),
          ),
          padding: EdgeInsets.zero,
          elevation: 0,
        ),
        child: Ink(
          decoration: BoxDecoration(
            gradient: LinearGradient(
              colors: [AppTheme.accentLavender, AppTheme.accentCoral],
            ),
            borderRadius: BorderRadius.circular(16.r),
            boxShadow: [
              BoxShadow(
                color: AppTheme.accentLavender.withOpacity(0.3),
                blurRadius: 12.r,
                offset: Offset(0, 6.h),
              ),
            ],
          ),
          child: Container(
            alignment: Alignment.center,
            child: isLoading
                ? SizedBox(
                    width: 20.w,
                    height: 20.w,
                    child: CircularProgressIndicator(
                      strokeWidth: 2,
                      valueColor: AlwaysStoppedAnimation<Color>(Colors.white),
                    ),
                  )
                : Text(
                    text,
                    style: TextStyle(
                      color: Colors.white,
                      fontSize: 16.sp,
                      fontWeight: FontWeight.bold,
                    ),
                  ),
          ),
        ),
      ),
    );
  }

  Widget _buildDangerButton({
    required String text,
    required IconData icon,
    required VoidCallback onPressed,
    bool isDangerous = false,
  }) {
    return Container(
      width: double.infinity,
      height: 50.h,
      decoration: BoxDecoration(
        border: Border.all(
          color: isDangerous ? AppTheme.error : AppTheme.textMedium.withOpacity(0.3),
          width: 1,
        ),
        borderRadius: BorderRadius.circular(12.r),
      ),
      child: Material(
        color: Colors.transparent,
        child: InkWell(
          onTap: onPressed,
          borderRadius: BorderRadius.circular(12.r),
          child: Center(
            child: Row(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                Icon(
                  icon,
                  color: isDangerous ? AppTheme.error : AppTheme.textMedium,
                  size: 18.sp,
                ),
                SizedBox(width: 8.w),
                Text(
                  text,
                  style: TextStyle(
                    color: isDangerous ? AppTheme.error : AppTheme.textMedium,
                    fontSize: 16.sp,
                    fontWeight: FontWeight.w600,
                  ),
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }

  // Action methods
  Future<void> _updateProfile() async {
    if (!_formKey.currentState!.validate()) return;

    setState(() {
      _isUpdatingProfile = true;
    });

    try {
      // Add your profile update logic here
      await Future.delayed(Duration(seconds: 2)); // Simulate API call
      
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Profile updated successfully'),
          backgroundColor: AppTheme.success,
        ),
      );
    } catch (e) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Failed to update profile'),
          backgroundColor: AppTheme.error,
        ),
      );
    } finally {
      setState(() {
        _isUpdatingProfile = false;
      });
    }
  }

  Future<void> _changePassword() async {
    if (!_formKey.currentState!.validate()) return;

    if (_newPasswordController.text.isEmpty) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Please enter a new password'),
          backgroundColor: AppTheme.error,
        ),
      );
      return;
    }

    setState(() {
      _isChangingPassword = true;
    });

    try {
      // Add your password change logic here
      await Future.delayed(Duration(seconds: 2)); // Simulate API call
      
      // Clear password fields
      _currentPasswordController.clear();
      _newPasswordController.clear();
      _confirmPasswordController.clear();
      
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Password changed successfully'),
          backgroundColor: AppTheme.success,
        ),
      );
    } catch (e) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Failed to change password'),
          backgroundColor: AppTheme.error,
        ),
      );
    } finally {
      setState(() {
        _isChangingPassword = false;
      });
    }
  }

  Future<void> _signOut() async {
    final confirmed = await showDialog<bool>(
      context: context,
      builder: (context) => _buildConfirmDialog(
        title: 'Sign Out',
        message: 'Are you sure you want to sign out?',
        confirmText: 'Sign Out',
      ),
    );

    if (confirmed == true) {
      try {
        await UnifiedAuthService().logout();
        Navigator.of(context).pushNamedAndRemoveUntil('/login', (route) => false);
      } catch (e) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Failed to sign out'),
            backgroundColor: AppTheme.error,
          ),
        );
      }
    }
  }

  Future<void> _deleteAccount() async {
    final confirmed = await showDialog<bool>(
      context: context,
      builder: (context) => _buildConfirmDialog(
        title: 'Delete Account',
        message: 'This action cannot be undone. All your data will be permanently deleted.',
        confirmText: 'Delete',
        isDangerous: true,
      ),
    );

    if (confirmed == true) {
      try {
        // Add your account deletion logic here
        await Future.delayed(Duration(seconds: 2)); // Simulate API call
        
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Account deletion initiated'),
            backgroundColor: AppTheme.success,
          ),
        );
      } catch (e) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Failed to delete account'),
            backgroundColor: AppTheme.error,
          ),
        );
      }
    }
  }

  Widget _buildConfirmDialog({
    required String title,
    required String message,
    required String confirmText,
    bool isDangerous = false,
  }) {
    return AlertDialog(
      backgroundColor: AppTheme.cardBackground,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(20.r),
      ),
      title: Text(
        title,
        style: TextStyle(
          color: AppTheme.textLight,
          fontSize: 20.sp,
          fontWeight: FontWeight.bold,
        ),
      ),
      content: Text(
        message,
        style: TextStyle(
          color: AppTheme.textMedium,
          fontSize: 16.sp,
        ),
      ),
      actions: [
        TextButton(
          onPressed: () => Navigator.of(context).pop(false),
          child: Text(
            'Cancel',
            style: TextStyle(
              color: AppTheme.textMedium,
              fontSize: 16.sp,
            ),
          ),
        ),
        TextButton(
          onPressed: () => Navigator.of(context).pop(true),
          child: Text(
            confirmText,
            style: TextStyle(
              color: isDangerous ? AppTheme.error : AppTheme.accentLavender,
              fontSize: 16.sp,
              fontWeight: FontWeight.bold,
            ),
          ),
        ),
      ],
    );
  }
}