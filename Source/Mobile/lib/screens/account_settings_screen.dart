import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../constants/app_theme.dart';
import '../services/unified_auth_service.dart';
import '../services/user_service.dart';
import '../widgets/stylish_loading.dart';
import '../widgets/universal_back_button.dart';
import 'settings/preferences_settings_screen.dart';
import 'settings/account_management_screen.dart';

class AccountSettingsScreen extends StatefulWidget {
  @override
  _AccountSettingsScreenState createState() => _AccountSettingsScreenState();
}

class _AccountSettingsScreenState extends State<AccountSettingsScreen> {
  bool _isLoading = true;
  String _currentEmail = '';
  String _currentFullName = '';
  DateTime? _joinDate;
  String _accountStatus = 'Active';
  
  // Edit mode state
  bool _isEditMode = false;
  bool _isUpdatingProfile = false;
  final _formKey = GlobalKey<FormState>();
  final _emailController = TextEditingController();
  final _fullNameController = TextEditingController();
  final _passwordController = TextEditingController();
  bool _isPasswordVisible = false;
  bool _showPasswordField = false;
  
  // Field error states
  String? _nameError;
  String? _emailError;
  String? _passwordError;

  @override
  void initState() {
    super.initState();
    _loadUserData();
    
    // Add listeners for validation
    _fullNameController.addListener(() {
      _checkAndShowPasswordField();
      setState(() {
        _nameError = _fullNameController.text.trim().isEmpty ? 'Full name is required' : null;
      });
    });
    
    _emailController.addListener(() {
      _checkAndShowPasswordField();
      setState(() {
        if (_emailController.text.trim().isEmpty) {
          _emailError = 'Email is required';
        } else if (!RegExp(r'^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$').hasMatch(_emailController.text.trim())) {
          _emailError = 'Please enter a valid email address';
        } else {
          _emailError = null;
        }
      });
    });
    
    _passwordController.addListener(() {
      setState(() {
        if (_passwordController.text.isEmpty) {
          _passwordError = 'Current password is required';
        } else {
          _passwordError = null;
        }
      });
    });
  }

  Future<void> _loadUserData() async {
    try {
      final fullName = await UserService.getFullName();
      final userData = await UserService.getUserData();
      final email = userData?['model']?['email'] ?? '';

      setState(() {
        _currentFullName = fullName.isNotEmpty ? fullName : 'User';
        _currentEmail = email.isNotEmpty ? email : 'user@example.com';
        _fullNameController.text = _currentFullName;
        _emailController.text = _currentEmail;
        _joinDate = DateTime.now().subtract(Duration(days: 90)); // Example join date
        _isLoading = false;
      });
    } catch (e) {
      print('Error loading user data: $e');
      setState(() {
        _currentFullName = 'User';
        _currentEmail = 'user@example.com';
        _fullNameController.text = _currentFullName;
        _emailController.text = _currentEmail;
        _joinDate = DateTime.now().subtract(Duration(days: 90));
        _isLoading = false;
      });
    }
  }
  
  @override
  void dispose() {
    _emailController.dispose();
    _fullNameController.dispose();
    _passwordController.dispose();
    super.dispose();
  }
  
  void _checkAndShowPasswordField() {
    // Check if user has actually changed from original values
    final bool nameChanged = _fullNameController.text.trim() != _currentFullName;
    final bool emailChanged = _emailController.text.trim() != _currentEmail;
    final bool hasChanges = nameChanged || emailChanged;
    
    if (hasChanges && !_showPasswordField) {
      setState(() {
        _showPasswordField = true;
      });
    } else if (!hasChanges && _showPasswordField) {
      setState(() {
        _showPasswordField = false;
      });
      _passwordController.clear();
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
              _buildBackButtonWithHeader(),
              Expanded(
                child: StylishLoading(
                  message: 'Loading account information...',
                ),
              ),
            ],
          ),
        ),
      );
    }

    return GestureDetector(
      onTap: () {
        // Dismiss keyboard when tapping outside text fields
        FocusScope.of(context).unfocus();
      },
      child: Scaffold(
        backgroundColor: AppTheme.backgroundDark,
        resizeToAvoidBottomInset: false, // Prevent widgets from moving when keyboard appears
        body: SafeArea(
          child: SingleChildScrollView(
            physics: BouncingScrollPhysics(),
            padding: EdgeInsets.symmetric(horizontal: 24.w),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                SizedBox(height: 16.h),
                _buildBackButtonWithHeader(),
                SizedBox(height: 24.h),
                _buildMorphingAccountSection(),
                SizedBox(height: 32.h),
                _buildSettingsNavigation(),
                SizedBox(height: 40.h),
                // Extra bottom padding for keyboard clearance when in edit mode
                if (_isEditMode) SizedBox(height: 200.h),
              ],
            ),
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
                'Account Settings',
                style: TextStyle(
                  fontSize: 28.sp,
                  fontWeight: FontWeight.bold,
                  color: AppTheme.textLight,
                ),
              ),
              SizedBox(height: 4.h),
              Text(
                'Manage your account and preferences',
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

  Widget _buildMorphingAccountSection() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        // Header with animated title and button
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            AnimatedSwitcher(
              duration: Duration(milliseconds: 300),
              child: Text(
                _isEditMode ? 'Edit Profile' : 'Account Information',
                key: ValueKey(_isEditMode),
                style: TextStyle(
                  fontSize: 20.sp,
                  fontWeight: FontWeight.bold,
                  color: AppTheme.textLight,
                ),
              ),
            ),
            GestureDetector(
              onTap: () {
                if (_isEditMode) {
                  // Cancel edit mode
                  FocusScope.of(context).unfocus();
                  setState(() {
                    _isEditMode = false;
                    _showPasswordField = false;
                    _passwordController.clear();
                    _fullNameController.text = _currentFullName;
                    _emailController.text = _currentEmail;
                    _nameError = null;
                    _emailError = null;
                    _passwordError = null;
                  });
                } else {
                  // Enter edit mode
                  setState(() {
                    _isEditMode = true;
                  });
                }
              },
              child: AnimatedContainer(
                duration: Duration(milliseconds: 300),
                padding: EdgeInsets.symmetric(horizontal: 16.w, vertical: 8.h),
                decoration: BoxDecoration(
                  border: Border.all(
                    color: _isEditMode ? AppTheme.textMedium : AppTheme.socialGradient[0],
                    width: 2,
                  ),
                  borderRadius: BorderRadius.circular(20.r),
                ),
                child: AnimatedSwitcher(
                  duration: Duration(milliseconds: 300),
                  child: Text(
                    _isEditMode ? 'Cancel' : 'Edit',
                    key: ValueKey(_isEditMode),
                    style: TextStyle(
                      fontSize: 14.sp,
                      fontWeight: FontWeight.w600,
                      color: _isEditMode ? AppTheme.textMedium : AppTheme.socialGradient[0],
                    ),
                  ),
                ),
              ),
            ),
          ],
        ),
        
        SizedBox(height: 16.h),
        
        // Name card that morphs
        _buildMorphingCard(
          isEditable: true,
          icon: Icons.person_outline,
          title: 'Full Name',
          controller: _fullNameController,
          hint: 'Enter your full name',
        ),
        
        SizedBox(height: 12.h),
        
        // Email card that morphs  
        _buildMorphingCard(
          isEditable: true,
          icon: Icons.email_outlined,
          title: 'Email Address',
          controller: _emailController,
          hint: 'Enter your email address',
          keyboardType: TextInputType.emailAddress,
        ),
        
        // Animated password field that slides down when needed
        ClipRect(
          child: AnimatedAlign(
            duration: Duration(milliseconds: 500),
            curve: Curves.easeOutCubic,
            alignment: Alignment.topCenter,
            heightFactor: _showPasswordField ? 1.0 : 0.0,
            child: Column(
              mainAxisSize: MainAxisSize.min,
              children: [
                SizedBox(height: 12.h),
                _buildMorphingCard(
                  isEditable: true,
                  icon: Icons.lock_outline,
                  title: 'Current Password',
                  controller: _passwordController,
                  hint: 'Enter your current password',
                  isPassword: true,
                ),
              ],
            ),
          ),
        ),
        
        // Static cards that fade out in edit mode
        AnimatedOpacity(
          duration: Duration(milliseconds: 400),
          opacity: _isEditMode ? 0.0 : 1.0,
          child: AnimatedContainer(
            duration: Duration(milliseconds: 400),
            height: _isEditMode ? 0 : null,
            curve: Curves.easeOutCubic,
            child: Column(
              children: [
                SizedBox(height: 12.h),
                _buildInfoCard(
                  icon: Icons.calendar_today_outlined,
                  title: 'Member Since',
                  subtitle: _joinDate != null 
                      ? '${_joinDate!.day}/${_joinDate!.month}/${_joinDate!.year}'
                      : 'Unknown',
                ),
                SizedBox(height: 12.h),
                _buildInfoCard(
                  icon: Icons.verified_outlined,
                  title: 'Account Status',
                  subtitle: _accountStatus,
                  subtitleColor: _accountStatus == 'Active' ? Colors.green : Colors.orange,
                ),
              ],
            ),
          ),
        ),
        
        // Update button that slides up in edit mode
        ClipRect(
          child: AnimatedAlign(
            duration: Duration(milliseconds: 500),
            curve: Curves.easeOutCubic,
            alignment: Alignment.topCenter,
            heightFactor: _isEditMode ? 1.0 : 0.0,
            child: Column(
              mainAxisSize: MainAxisSize.min,
              children: [
                SizedBox(height: 24.h),
                _buildUpdateButton(),
              ],
            ),
          ),
        ),
      ],
    );
  }

  Widget _buildMorphingCard({
    required bool isEditable,
    required IconData icon,
    required String title,
    required TextEditingController controller,
    required String hint,
    TextInputType? keyboardType,
    bool isPassword = false,
  }) {
    final hasError = _getFieldError(controller) != null;
    
    return Container(
      width: double.infinity,
      padding: EdgeInsets.all(16.w),
      decoration: BoxDecoration(
        color: AppTheme.cardBackground,
        borderRadius: BorderRadius.circular(16.r),
        border: Border.all(
          color: _isEditMode && hasError
              ? Colors.red.withOpacity(0.5)
              : AppTheme.textMedium.withOpacity(0.1),
          width: _isEditMode && hasError ? 2 : 1,
        ),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          // Header row with icon and title
          Row(
            children: [
              Container(
                width: 40.w,
                height: 40.h,
                decoration: BoxDecoration(
                  color: _isEditMode && hasError
                      ? Colors.red.withOpacity(0.1)
                      : (icon == Icons.lock_outline && _isEditMode)
                          ? Colors.red.withOpacity(0.1)
                          : AppTheme.socialGradient[0].withOpacity(0.1),
                  borderRadius: BorderRadius.circular(12.r),
                ),
                child: Icon(
                  icon,
                  color: _isEditMode && hasError
                      ? Colors.red
                      : (icon == Icons.lock_outline && _isEditMode)
                          ? Colors.red[400]
                          : AppTheme.socialGradient[0],
                  size: 20.sp,
                ),
              ),
              SizedBox(width: 16.w),
              Expanded(
                child: Text(
                  title,
                  style: TextStyle(
                    fontSize: 14.sp,
                    color: AppTheme.textMedium,
                    fontWeight: FontWeight.w500,
                  ),
                ),
              ),
            ],
          ),
          
          // Morphing content area
          _isEditMode
              ? _buildEditField(controller, hint, keyboardType, isPassword)
              : _buildDisplayValue(controller.text),
          
          // Error message area
          if (_isEditMode)
            AnimatedContainer(
              duration: Duration(milliseconds: 200),
              height: hasError ? 20.h : 0,
              child: hasError
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
                            _getFieldError(controller) ?? '',
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
            ),
        ],
      ),
    );
  }
  
  Widget _buildEditField(TextEditingController controller, String hint, TextInputType? keyboardType, bool isPassword) {
    final hasError = _getFieldError(controller) != null;
    
    return Padding(
      padding: EdgeInsets.only(top: 8.h),
      child: TextFormField(
        controller: controller,
        obscureText: isPassword && !_isPasswordVisible,
        keyboardType: keyboardType,
        style: TextStyle(
          color: AppTheme.textLight,
          fontSize: 16.sp,
          fontWeight: FontWeight.w600,
        ),
        decoration: InputDecoration(
          hintText: hint,
          hintStyle: TextStyle(
            color: AppTheme.textMedium.withOpacity(0.6),
            fontSize: 16.sp,
            fontWeight: FontWeight.w400,
          ),
          suffixIcon: isPassword
              ? GestureDetector(
                  onTap: () {
                    setState(() {
                      _isPasswordVisible = !_isPasswordVisible;
                    });
                  },
                  child: Icon(
                    _isPasswordVisible ? Icons.visibility_off_outlined : Icons.visibility_outlined,
                    color: AppTheme.textMedium,
                    size: 20.sp,
                  ),
                )
              : null,
          filled: true,
          fillColor: AppTheme.backgroundDark.withOpacity(0.3),
          border: OutlineInputBorder(
            borderRadius: BorderRadius.circular(12.r),
            borderSide: BorderSide(
              color: hasError 
                  ? Colors.red.withOpacity(0.4)
                  : AppTheme.textMedium.withOpacity(0.2),
              width: 1.5,
            ),
          ),
          enabledBorder: OutlineInputBorder(
            borderRadius: BorderRadius.circular(12.r),
            borderSide: BorderSide(
              color: hasError 
                  ? Colors.red.withOpacity(0.4)
                  : AppTheme.textMedium.withOpacity(0.2),
              width: 1.5,
            ),
          ),
          focusedBorder: OutlineInputBorder(
            borderRadius: BorderRadius.circular(12.r),
            borderSide: BorderSide(
              color: hasError 
                  ? Colors.red
                  : AppTheme.socialGradient[0],
              width: 2.0,
            ),
          ),
          errorBorder: OutlineInputBorder(
            borderRadius: BorderRadius.circular(12.r),
            borderSide: BorderSide(
              color: Colors.red.withOpacity(0.4),
              width: 1.5,
            ),
          ),
          focusedErrorBorder: OutlineInputBorder(
            borderRadius: BorderRadius.circular(12.r),
            borderSide: BorderSide(
              color: Colors.red,
              width: 2.0,
            ),
          ),
          contentPadding: EdgeInsets.symmetric(horizontal: 16.w, vertical: 16.h),
        ),
      ),
    );
  }
  
  Widget _buildDisplayValue(String value) {
    return Padding(
      padding: EdgeInsets.only(top: 4.h),
      child: Align(
        alignment: Alignment.centerLeft,
        child: Text(
          value.isNotEmpty ? value : 'Not set',
          style: TextStyle(
            fontSize: 16.sp,
            color: value.isNotEmpty ? AppTheme.textLight : AppTheme.textMedium,
            fontWeight: FontWeight.w600,
          ),
        ),
      ),
    );
  }

  Widget _buildInfoCard({
    required IconData icon,
    required String title,
    required String subtitle,
    Color? subtitleColor,
  }) {
    return Container(
      width: double.infinity,
      padding: EdgeInsets.all(16.w),
      decoration: BoxDecoration(
        color: AppTheme.cardBackground,
        borderRadius: BorderRadius.circular(16.r),
        border: Border.all(
          color: AppTheme.textMedium.withOpacity(0.1),
          width: 1,
        ),
      ),
      child: Row(
        children: [
          Container(
            width: 40.w,
            height: 40.h,
            decoration: BoxDecoration(
              color: AppTheme.socialGradient[0].withOpacity(0.1),
              borderRadius: BorderRadius.circular(12.r),
            ),
            child: Icon(
              icon,
              color: AppTheme.socialGradient[0],
              size: 20.sp,
            ),
          ),
          SizedBox(width: 16.w),
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  title,
                  style: TextStyle(
                    fontSize: 14.sp,
                    color: AppTheme.textMedium,
                    fontWeight: FontWeight.w500,
                  ),
                ),
                SizedBox(height: 4.h),
                Text(
                  subtitle,
                  style: TextStyle(
                    fontSize: 16.sp,
                    color: subtitleColor ?? AppTheme.textLight,
                    fontWeight: FontWeight.w600,
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildSettingsNavigation() {
    return SizedBox.shrink();
  }

  Widget _buildNavigationButton({
    required IconData icon,
    required String title,
    required String subtitle,
    required VoidCallback onTap,
    bool isDestructive = false,
  }) {
    return Container(
      width: double.infinity,
      decoration: BoxDecoration(
        color: AppTheme.cardBackground,
        borderRadius: BorderRadius.circular(16.r),
        border: Border.all(
          color: isDestructive 
              ? Colors.red.withOpacity(0.2)
              : AppTheme.textMedium.withOpacity(0.1),
          width: 1,
        ),
      ),
      child: Material(
        color: Colors.transparent,
        child: InkWell(
          onTap: onTap,
          borderRadius: BorderRadius.circular(16.r),
          child: Padding(
            padding: EdgeInsets.all(16.w),
            child: Row(
              children: [
                Container(
                  width: 48.w,
                  height: 48.h,
                  decoration: BoxDecoration(
                    color: isDestructive 
                        ? Colors.red.withOpacity(0.1)
                        : AppTheme.socialGradient[0].withOpacity(0.1),
                    borderRadius: BorderRadius.circular(12.r),
                  ),
                  child: Icon(
                    icon,
                    color: isDestructive 
                        ? Colors.red
                        : AppTheme.socialGradient[0],
                    size: 24.sp,
                  ),
                ),
                SizedBox(width: 16.w),
                Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(
                        title,
                        style: TextStyle(
                          fontSize: 16.sp,
                          color: isDestructive 
                              ? Colors.red
                              : AppTheme.textLight,
                          fontWeight: FontWeight.w600,
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
                Icon(
                  Icons.chevron_right,
                  color: AppTheme.textMedium.withOpacity(0.6),
                  size: 20.sp,
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }
  
  Widget _buildEditProfileSection() {
    return Form(
      key: _formKey,
      child: Column(
        key: ValueKey('edit_profile'),
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              Text(
                'Edit Profile',
                style: TextStyle(
                  fontSize: 20.sp,
                  fontWeight: FontWeight.bold,
                  color: AppTheme.textLight,
                ),
              ),
              GestureDetector(
                onTap: () {
                  // Dismiss keyboard first
                  FocusScope.of(context).unfocus();
                  setState(() {
                    _isEditMode = false;
                    _showPasswordField = false;
                    _passwordController.clear();
                    // Reset to original values
                    _fullNameController.text = _currentFullName;
                    _emailController.text = _currentEmail;
                    _nameError = null;
                    _emailError = null;
                    _passwordError = null;
                  });
                },
                child: Container(
                  padding: EdgeInsets.symmetric(horizontal: 16.w, vertical: 8.h),
                  decoration: BoxDecoration(
                    border: Border.all(
                      color: AppTheme.textMedium,
                      width: 2,
                    ),
                    borderRadius: BorderRadius.circular(20.r),
                  ),
                  child: Text(
                    'Cancel',
                    style: TextStyle(
                      fontSize: 14.sp,
                      fontWeight: FontWeight.w600,
                      color: AppTheme.textMedium,
                    ),
                  ),
                ),
              ),
            ],
          ),
          SizedBox(height: 24.h),
          
          // Name field
          _buildTextField(
            controller: _fullNameController,
            label: 'Full Name',
            hint: 'Enter your full name',
            icon: Icons.person_outline,
          ),
          
          SizedBox(height: 16.h),
          
          // Email field
          _buildTextField(
            controller: _emailController,
            label: 'Email Address',
            hint: 'Enter your email address',
            icon: Icons.email_outlined,
            keyboardType: TextInputType.emailAddress,
          ),
          
          SizedBox(height: 16.h),
          
          // Animated password field (shows when user starts typing)
          AnimatedContainer(
            duration: Duration(milliseconds: 500),
            height: _showPasswordField ? null : 0,
            curve: Curves.easeOut,
            child: _showPasswordField
                ? Column(
                    children: [
                      _buildTextField(
                        controller: _passwordController,
                        label: 'Current Password',
                        hint: 'Enter your current password',
                        icon: Icons.lock_outline,
                        isPassword: true,
                      ),
                      SizedBox(height: 16.h),
                    ],
                  )
                : SizedBox.shrink(),
          ),
          
          SizedBox(height: 24.h),
          
          // Update button
          _buildUpdateButton(),
        ],
      ),
    );
  }
  
  Widget _buildTextField({
    required TextEditingController controller,
    required String label,
    required String hint,
    required IconData icon,
    TextInputType? keyboardType,
    bool isPassword = false,
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
        
        Container(
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              TextFormField(
                controller: controller,
                validator: (value) => null, // Custom validation handled by listeners
                obscureText: isPassword && !_isPasswordVisible,
                keyboardType: keyboardType,
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
                              _isPasswordVisible = !_isPasswordVisible;
                            });
                          },
                          child: Icon(
                            _isPasswordVisible ? Icons.visibility_off_outlined : Icons.visibility_outlined,
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
                      color: _getFieldError(controller) != null 
                          ? Colors.red
                          : AppTheme.socialGradient[0],
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
    if (controller == _fullNameController) return _nameError;
    if (controller == _emailController) return _emailError;
    if (controller == _passwordController) return _passwordError;
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
  
  Widget _buildUpdateButton() {
    // Check if user has made changes from original values
    final bool nameChanged = _fullNameController.text.trim() != _currentFullName;
    final bool emailChanged = _emailController.text.trim() != _currentEmail;
    final bool hasChanges = nameChanged || emailChanged;
    
    // Check if form has errors
    final bool hasErrors = _nameError != null || _emailError != null || _passwordError != null;
    
    // Button is enabled if user made changes, no validation errors, and password provided when needed
    final bool isButtonEnabled = hasChanges && 
                                !hasErrors && 
                                (!_showPasswordField || _passwordController.text.trim().isNotEmpty) && 
                                !_isUpdatingProfile;
    
    return SizedBox(
      width: double.infinity,
      height: 56.h,
      child: ElevatedButton(
        onPressed: isButtonEnabled ? _updateProfile : null,
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
            gradient: isButtonEnabled
                ? LinearGradient(colors: AppTheme.socialGradient)
                : null,
            color: isButtonEnabled
                ? null
                : AppTheme.textMedium.withOpacity(0.3),
            borderRadius: BorderRadius.circular(16.r),
            boxShadow: isButtonEnabled ? [
              BoxShadow(
                color: AppTheme.socialGradient[0].withOpacity(0.4),
                blurRadius: 20.r,
                offset: Offset(0, 8.h),
                spreadRadius: 2.r,
              ),
            ] : null,
          ),
          child: Container(
            alignment: Alignment.center,
            child: _isUpdatingProfile
                ? CompactStylishLoading(size: 20.w, color: AppTheme.white)
                : Text(
                    'Update Profile',
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
  
  Future<void> _updateProfile() async {
    // Dismiss keyboard first
    FocusScope.of(context).unfocus();
    
    // Check for validation errors
    if (_nameError != null || _emailError != null || _passwordError != null) {
      return;
    }

    // Password is always required for any profile updates
    if (_passwordController.text.isEmpty) {
      setState(() {
        _passwordError = 'Current password is required';
      });
      return;
    }

    setState(() {
      _isUpdatingProfile = true;
    });

    try {
      final authService = UnifiedAuthService();
      
      // Parse the full name
      final fullName = _fullNameController.text.trim();
      final nameParts = fullName.split(' ');
      final firstName = nameParts.isNotEmpty ? nameParts[0] : '';
      final lastName = nameParts.length > 1 ? nameParts.skip(1).join(' ') : '';
      
      // Update local user data
      await UserService.updateUserProfile(
        firstName: firstName,
        lastName: lastName,
        userName: _emailController.text.trim(),
      );
      
      // Verify password with a login call (this validates the current password)
      final currentUserData = await UserService.getUserData();
      final currentUserName = currentUserData?['model']?['userName'] ?? _emailController.text.trim();
      
      final loginResult = await authService.login(
        username: currentUserName,
        password: _passwordController.text.trim(),
        rememberMe: true,
      );
      
      if (!loginResult.isSuccess) {
        throw Exception('Invalid current password');
      }
      
      // Clear password field after successful update
      _passwordController.clear();
      
      // Update current values to new values
      setState(() {
        _currentFullName = fullName;
        _currentEmail = _emailController.text.trim();
        _showPasswordField = false;
        _isEditMode = false;
      });
      
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Row(
            children: [
              Icon(Icons.check_circle, color: Colors.white),
              SizedBox(width: 8),
              Text('Profile updated successfully'),
            ],
          ),
          backgroundColor: Colors.green,
          behavior: SnackBarBehavior.floating,
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(12),
          ),
        ),
      );
    } catch (e) {
      String errorMessage = 'Failed to update profile';
      
      if (e.toString().contains('Invalid current password')) {
        errorMessage = 'Current password is incorrect';
        setState(() {
          _passwordError = 'Current password is incorrect';
        });
      } else if (e.toString().contains('network') || e.toString().contains('connection')) {
        errorMessage = 'Network error. Please check your connection.';
      }
      
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Row(
            children: [
              Icon(Icons.error_outline, color: Colors.white),
              SizedBox(width: 8),
              Expanded(child: Text(errorMessage)),
            ],
          ),
          backgroundColor: Colors.red,
          behavior: SnackBarBehavior.floating,
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(12),
          ),
        ),
      );
    } finally {
      setState(() {
        _isUpdatingProfile = false;
      });
    }
  }
}