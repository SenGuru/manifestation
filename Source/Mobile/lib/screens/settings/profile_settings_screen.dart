import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../../constants/app_theme.dart';
import '../../services/user_service.dart';
import '../../services/unified_auth_service.dart';
import '../../widgets/stylish_loading.dart';
import '../../widgets/universal_back_button.dart';

class ProfileSettingsScreen extends StatefulWidget {
  @override
  _ProfileSettingsScreenState createState() => _ProfileSettingsScreenState();
}

class _ProfileSettingsScreenState extends State<ProfileSettingsScreen> 
    with TickerProviderStateMixin {
  final _formKey = GlobalKey<FormState>();
  final _emailController = TextEditingController();
  final _fullNameController = TextEditingController();
  final _passwordController = TextEditingController();

  bool _isLoading = true;
  bool _isUpdatingProfile = false;
  bool _isPasswordVisible = false;
  bool _showPasswordField = false;

  String _currentEmail = '';
  String _currentFullName = '';
  
  // Field error states
  String? _nameError;
  String? _emailError;
  String? _passwordError;
  
  late AnimationController _buttonPulseController;
  late Animation<double> _buttonPulseAnimation;
  late AnimationController _passwordFieldController;
  late Animation<double> _passwordFieldAnimation;
  late AnimationController _buttonSlideController;
  late Animation<Offset> _buttonSlideAnimation;

  @override
  void initState() {
    super.initState();
    _loadUserData();
    
    // Initialize animation controllers
    _buttonPulseController = AnimationController(
      duration: Duration(milliseconds: 2000),
      vsync: this,
    );
    
    _buttonPulseAnimation = Tween<double>(
      begin: 1.0,
      end: 1.02,
    ).animate(CurvedAnimation(
      parent: _buttonPulseController,
      curve: Curves.easeInOut,
    ));
    
    _passwordFieldController = AnimationController(
      duration: Duration(milliseconds: 500),
      vsync: this,
    );
    
    _passwordFieldAnimation = Tween<double>(
      begin: 0.0,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _passwordFieldController,
      curve: Curves.easeOut,
    ));
    
    _buttonSlideController = AnimationController(
      duration: Duration(milliseconds: 400),
      vsync: this,
    );
    
    _buttonSlideAnimation = Tween<Offset>(
      begin: Offset(0, -0.2),
      end: Offset.zero,
    ).animate(CurvedAnimation(
      parent: _buttonSlideController,
      curve: Curves.easeOut,
    ));
    
    _buttonPulseController.repeat(reverse: true);
    
    // Add listeners for validation and password field animation
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
        // Password is always required for profile updates
        if (_passwordController.text.isEmpty) {
          _passwordError = 'Current password is required';
        } else {
          _passwordError = null;
        }
      });
    });
  }

  @override
  void dispose() {
    _emailController.dispose();
    _fullNameController.dispose();
    _passwordController.dispose();
    _buttonPulseController.dispose();
    _passwordFieldController.dispose();
    _buttonSlideController.dispose();
    super.dispose();
  }

  void _checkAndShowPasswordField() {
    // Check if user has actually changed from original values (not just has text)
    final bool nameChanged = _fullNameController.text.trim() != _currentFullName;
    final bool emailChanged = _emailController.text.trim() != _currentEmail;
    final bool hasChanges = nameChanged || emailChanged;
    
    if (hasChanges && !_showPasswordField) {
      setState(() {
        _showPasswordField = true;
      });
      _passwordFieldController.forward();
      _buttonSlideController.forward();
    } else if (!hasChanges && _showPasswordField) {
      setState(() {
        _showPasswordField = false;
      });
      _passwordFieldController.reverse();
      _buttonSlideController.reverse();
      _passwordController.clear();
    }
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
              _buildBackButtonWithHeader(),
              Expanded(
                child: StylishLoading(
                  message: 'Loading profile settings...',
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
        resizeToAvoidBottomInset: true,
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
                _buildProfileForm(),
                SizedBox(height: 40.h),
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
                'Profile Settings',
                style: TextStyle(
                  fontSize: 28.sp,
                  fontWeight: FontWeight.bold,
                  color: AppTheme.textLight,
                ),
              ),
              SizedBox(height: 4.h),
              Text(
                'Update your personal information',
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

  Widget _buildProfileForm() {
    return Form(
      key: _formKey,
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          // Name field
          _buildTextField(
            controller: _fullNameController,
            label: 'Full Name',
            hint: 'Enter your full name',
            icon: Icons.person_outline,
          ),
          
          SizedBox(height: 8.h), // Reduced spacing
          
          // Email field
          _buildTextField(
            controller: _emailController,
            label: 'Email Address',
            hint: 'Enter your email address',
            icon: Icons.email_outlined,
            keyboardType: TextInputType.emailAddress,
          ),
          
          SizedBox(height: 8.h), // Reduced spacing
          
          // Animated password field (shows when user starts typing)
          AnimatedBuilder(
            animation: _passwordFieldAnimation,
            builder: (context, child) {
              return ClipRect(
                child: AnimatedContainer(
                  duration: Duration(milliseconds: 500),
                  height: _showPasswordField ? 84.h : 0, // Compact height to match reduced spacing
                  curve: Curves.easeOut,
                  child: _showPasswordField
                      ? FadeTransition(
                          opacity: _passwordFieldAnimation,
                          child: SlideTransition(
                            position: Tween<Offset>(
                              begin: Offset(0, -0.3),
                              end: Offset.zero,
                            ).animate(CurvedAnimation(
                              parent: _passwordFieldController,
                              curve: Curves.easeOut,
                            )),
                            child: Column(
                              children: [
                                SizedBox(height: 8.h), // Reduced spacing to match other fields
                                _buildTextField(
                                  controller: _passwordController,
                                  label: 'Current Password',
                                  hint: 'Enter your current password',
                                  icon: Icons.lock_outline,
                                  isPassword: true,
                                ),
                              ],
                            ),
                          ),
                        )
                      : SizedBox.shrink(),
                ),
              );
            },
          ),
          
          SizedBox(height: 24.h),
          
          // Update button
          _buildPrimaryButton(
            text: 'Update Profile',
            onPressed: _updateProfile,
            isLoading: _isUpdatingProfile,
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
        SizedBox(height: 6.h),
        
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
                height: 14.h, // Reduced from 16.h to make fields more compact
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

  Widget _buildPrimaryButton({
    required String text,
    required VoidCallback? onPressed,
    bool isLoading = false,
  }) {
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
                                !isLoading;
    
    return SlideTransition(
      position: _buttonSlideAnimation,
      child: FadeTransition(
        opacity: _buttonSlideController,
        child: AnimatedBuilder(
          animation: _buttonPulseAnimation,
          builder: (context, child) {
            return Transform.scale(
              scale: (!isButtonEnabled || isLoading) ? 1.0 : _buttonPulseAnimation.value,
              child: SizedBox(
                width: double.infinity,
                height: 56.h,
                child: ElevatedButton(
                  onPressed: isButtonEnabled ? onPressed : null,
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
                          ? LinearGradient(colors: AppTheme.affirmationGradient)
                          : null,
                      color: isButtonEnabled
                          ? null
                          : AppTheme.textMedium.withOpacity(0.3),
                      borderRadius: BorderRadius.circular(16.r),
                      boxShadow: isButtonEnabled ? [
                        BoxShadow(
                          color: AppTheme.affirmationGradient[0].withOpacity(0.4),
                          blurRadius: 20.r,
                          offset: Offset(0, 8.h),
                          spreadRadius: 2.r,
                        ),
                        BoxShadow(
                          color: AppTheme.affirmationGradient[1].withOpacity(0.2),
                          blurRadius: 40.r,
                          offset: Offset(0, 16.h),
                        ),
                      ] : null,
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
              ),
            );
          },
        ),
      ),
    );
  }

  Future<void> _updateProfile() async {
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
      
      // For now, we'll update locally since there's no direct profile update API
      // In a real app, this would call a profile update endpoint
      
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
      });
      
      // Reset animations
      _passwordFieldController.reverse();
      _buttonSlideController.reverse();
      
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