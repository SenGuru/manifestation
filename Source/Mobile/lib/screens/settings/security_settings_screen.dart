import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../../constants/app_theme.dart';
import '../../widgets/stylish_loading.dart';
import '../../widgets/universal_back_button.dart';

class SecuritySettingsScreen extends StatefulWidget {
  @override
  _SecuritySettingsScreenState createState() => _SecuritySettingsScreenState();
}

class _SecuritySettingsScreenState extends State<SecuritySettingsScreen> {
  final _formKey = GlobalKey<FormState>();
  final _currentPasswordController = TextEditingController();
  final _newPasswordController = TextEditingController();
  final _confirmPasswordController = TextEditingController();

  bool _isChangingPassword = false;
  bool _isPasswordVisible = false;
  bool _isNewPasswordVisible = false;
  bool _isConfirmPasswordVisible = false;

  // Field error states
  String? _currentPasswordError;
  String? _newPasswordError;
  String? _confirmPasswordError;

  @override
  void initState() {
    super.initState();
    
    // Add listeners for validation
    _currentPasswordController.addListener(() {
      setState(() {
        if (_newPasswordController.text.isNotEmpty && _currentPasswordController.text.isEmpty) {
          _currentPasswordError = 'Current password is required';
        } else {
          _currentPasswordError = null;
        }
      });
    });
    
    _newPasswordController.addListener(() {
      setState(() {
        if (_newPasswordController.text.isNotEmpty && _newPasswordController.text.length < 6) {
          _newPasswordError = 'Password must be at least 6 characters';
        } else {
          _newPasswordError = null;
        }
        
        // Also update confirm password validation
        if (_confirmPasswordController.text.isNotEmpty) {
          if (_confirmPasswordController.text != _newPasswordController.text) {
            _confirmPasswordError = 'Passwords do not match';
          } else {
            _confirmPasswordError = null;
          }
        }
      });
    });
    
    _confirmPasswordController.addListener(() {
      setState(() {
        if (_newPasswordController.text.isNotEmpty) {
          if (_confirmPasswordController.text.isEmpty) {
            _confirmPasswordError = 'Please confirm your new password';
          } else if (_confirmPasswordController.text != _newPasswordController.text) {
            _confirmPasswordError = 'Passwords do not match';
          } else {
            _confirmPasswordError = null;
          }
        } else {
          _confirmPasswordError = null;
        }
      });
    });
  }

  @override
  void dispose() {
    _currentPasswordController.dispose();
    _newPasswordController.dispose();
    _confirmPasswordController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppTheme.backgroundDark,
      resizeToAvoidBottomInset: false,
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
              _buildSecurityForm(),
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
                'Security',
                style: TextStyle(
                  fontSize: 28.sp,
                  fontWeight: FontWeight.bold,
                  color: AppTheme.textLight,
                ),
              ),
              SizedBox(height: 4.h),
              Text(
                'Update your password and security settings',
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

  Widget _buildSecurityForm() {
    return Form(
      key: _formKey,
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(
            'Change Password',
            style: TextStyle(
              fontSize: 18.sp,
              fontWeight: FontWeight.bold,
              color: AppTheme.textLight,
            ),
          ),
          
          SizedBox(height: 16.h),
          
          // Current password field
          _buildTextField(
            controller: _currentPasswordController,
            label: 'Current Password',
            hint: 'Enter current password',
            icon: Icons.lock_outline,
            isPassword: true,
            isCurrentPassword: true,
          ),
          
          SizedBox(height: 12.h),
          
          // New password field
          _buildTextField(
            controller: _newPasswordController,
            label: 'New Password',
            hint: 'Enter new password',
            icon: Icons.lock_reset,
            isPassword: true,
            isNewPassword: true,
          ),
          
          SizedBox(height: 12.h),
          
          // Confirm new password field
          _buildTextField(
            controller: _confirmPasswordController,
            label: 'Confirm New Password',
            hint: 'Confirm new password',
            icon: Icons.lock_outline,
            isPassword: true,
            isConfirmPassword: true,
          ),
          
          SizedBox(height: 24.h),
          
          // Change password button
          _buildPrimaryButton(
            text: 'Change Password',
            onPressed: _changePassword,
            isLoading: _isChangingPassword,
          ),
        ],
      ),
    );
  }

  Widget _buildTextField({
    required TextEditingController controller,
    required String label,
    required String hint,
    required IconData icon,
    bool isPassword = false,
    bool isCurrentPassword = false,
    bool isNewPassword = false,
    bool isConfirmPassword = false,
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
        SizedBox(height: 6.h),
        
        Container(
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              TextFormField(
                controller: controller,
                validator: (value) => null, // Custom validation handled by listeners
                obscureText: isPassword && (
                  isConfirmPassword 
                      ? !_isConfirmPasswordVisible 
                      : isNewPassword 
                          ? !_isNewPasswordVisible 
                          : !_isPasswordVisible),
                style: TextStyle(
                  color: AppTheme.textLight,
                  fontSize: 16.sp,
                ),
                decoration: InputDecoration(
                  hintText: hint,
                  hintStyle: TextStyle(
                    color: AppTheme.textMedium.withOpacity(0.6),
                  ),
                  prefixIcon: Icon(
                    icon,
                    color: AppTheme.textMedium,
                    size: 20.sp,
                  ),
                  suffixIcon: isPassword
                      ? GestureDetector(
                          onTap: () {
                            setState(() {
                              if (isConfirmPassword) {
                                _isConfirmPasswordVisible = !_isConfirmPasswordVisible;
                              } else if (isNewPassword) {
                                _isNewPasswordVisible = !_isNewPasswordVisible;
                              } else {
                                _isPasswordVisible = !_isPasswordVisible;
                              }
                            });
                          },
                          child: Icon(
                            (isConfirmPassword ? _isConfirmPasswordVisible : 
                             isNewPassword ? _isNewPasswordVisible : _isPasswordVisible)
                                ? Icons.visibility_off_outlined
                                : Icons.visibility_outlined,
                            color: AppTheme.textMedium,
                            size: 20.sp,
                          ),
                        )
                      : null,
                  filled: true,
                  fillColor: AppTheme.cardBackground,
                  border: OutlineInputBorder(
                    borderRadius: BorderRadius.circular(16.r),
                    borderSide: BorderSide.none,
                  ),
                  enabledBorder: OutlineInputBorder(
                    borderRadius: BorderRadius.circular(16.r),
                    borderSide: BorderSide(
                      color: _getFieldError(controller) != null 
                          ? Colors.red.withOpacity(0.5)
                          : AppTheme.textMedium.withOpacity(0.1),
                      width: 2,
                    ),
                  ),
                  focusedBorder: OutlineInputBorder(
                    borderRadius: BorderRadius.circular(16.r),
                    borderSide: BorderSide(
                      color: AppTheme.socialGradient[0],
                      width: 2,
                    ),
                  ),
                  errorBorder: OutlineInputBorder(
                    borderRadius: BorderRadius.circular(16.r),
                    borderSide: BorderSide(
                      color: Colors.red,
                      width: 2,
                    ),
                  ),
                  focusedErrorBorder: OutlineInputBorder(
                    borderRadius: BorderRadius.circular(16.r),
                    borderSide: BorderSide(
                      color: Colors.red,
                      width: 2,
                    ),
                  ),
                  contentPadding: EdgeInsets.symmetric(
                    horizontal: 16.w,
                    vertical: 16.h,
                  ),
                ),
              ),
              
              Container(
                height: 16.h,
                margin: EdgeInsets.only(top: 2.h),
                child: _buildFieldError(_getFieldError(controller)),
              ),
            ],
          ),
        ),
      ],
    );
  }
  
  String? _getFieldError(TextEditingController controller) {
    if (controller == _currentPasswordController) return _currentPasswordError;
    if (controller == _newPasswordController) return _newPasswordError;
    if (controller == _confirmPasswordController) return _confirmPasswordError;
    return null;
  }
  
  Widget _buildFieldError(String? errorText) {
    return AnimatedOpacity(
      opacity: errorText != null && errorText.isNotEmpty ? 1.0 : 0.0,
      duration: Duration(milliseconds: 200),
      child: errorText != null && errorText.isNotEmpty
          ? Row(
              children: [
                Icon(
                  Icons.error_outline,
                  color: Colors.red,
                  size: 12.sp,
                ),
                SizedBox(width: 4.w),
                Expanded(
                  child: Text(
                    errorText,
                    style: TextStyle(
                      color: Colors.red,
                      fontSize: 11.sp,
                      fontWeight: FontWeight.w500,
                    ),
                    maxLines: 1,
                    overflow: TextOverflow.ellipsis,
                  ),
                ),
              ],
            )
          : SizedBox.shrink(),
    );
  }

  Widget _buildPrimaryButton({
    required String text,
    required VoidCallback? onPressed,
    bool isLoading = false,
  }) {
    return SizedBox(
      width: double.infinity,
      height: 56.h,
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
            gradient: LinearGradient(colors: AppTheme.socialGradient),
            borderRadius: BorderRadius.circular(16.r),
            boxShadow: [
              BoxShadow(
                color: AppTheme.socialGradient[0].withOpacity(0.4),
                blurRadius: 20.r,
                offset: Offset(0, 8.h),
                spreadRadius: 2.r,
              ),
              BoxShadow(
                color: AppTheme.socialGradient[1].withOpacity(0.2),
                blurRadius: 40.r,
                offset: Offset(0, 16.h),
              ),
            ],
          ),
          child: Container(
            alignment: Alignment.center,
            child: isLoading
                ? CompactStylishLoading(size: 20.w, color: AppTheme.white)
                : Text(
                    text,
                    style: TextStyle(
                      fontSize: 18.sp,
                      fontWeight: FontWeight.w600,
                      color: AppTheme.white,
                      letterSpacing: 0.5,
                    ),
                  ),
          ),
        ),
      ),
    );
  }

  Future<void> _changePassword() async {
    if (_currentPasswordError != null || _newPasswordError != null || _confirmPasswordError != null) {
      return;
    }

    if (_newPasswordController.text.isEmpty) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Please enter a new password'),
          backgroundColor: Colors.red,
        ),
      );
      return;
    }

    setState(() {
      _isChangingPassword = true;
    });

    try {
      await Future.delayed(Duration(seconds: 2)); // Simulate API call
      
      // Clear password fields
      _currentPasswordController.clear();
      _newPasswordController.clear();
      _confirmPasswordController.clear();
      
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Password changed successfully'),
          backgroundColor: Colors.green,
        ),
      );
    } catch (e) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Failed to change password'),
          backgroundColor: Colors.red,
        ),
      );
    } finally {
      setState(() {
        _isChangingPassword = false;
      });
    }
  }
}