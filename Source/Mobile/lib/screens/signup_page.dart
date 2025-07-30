import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../models/auth_models.dart';
import '../services/unified_auth_service.dart';
import 'home_screen.dart';
import 'post_signup_onboarding.dart';
import '../constants/colors.dart';
import '../widgets/stylish_loading.dart';
import '../widgets/keyboard_dismissal_wrapper.dart';

class SignUpScreen extends StatefulWidget {
  @override
  _SignUpScreenState createState() => _SignUpScreenState();
}

class _SignUpScreenState extends State<SignUpScreen>
    with TickerProviderStateMixin {
  final _formKey = GlobalKey<FormState>();
  final _nameController = TextEditingController();
  final _emailController = TextEditingController();
  final _passwordController = TextEditingController();
  final _confirmPasswordController = TextEditingController();
  
  bool _isPasswordVisible = false;
  bool _isConfirmPasswordVisible = false;
  bool _agreeToTerms = false;
  bool _subscribeToNewsletter = false;
  bool _isLoading = false;
  String _validationError = '';
  
  PasswordStrength _passwordStrength = PasswordStrength(
    strength: 0.0,
    label: 'Enter password',
    color: Colors.grey,
  );
  
  late AnimationController _fadeController;
  late AnimationController _slideController;
  late AnimationController _buttonPulseController;
  late Animation<double> _fadeAnimation;
  late Animation<Offset> _slideAnimation;
  late Animation<double> _buttonPulseAnimation;

  @override
  void initState() {
    super.initState();
    
    _fadeController = AnimationController(
      duration: Duration(milliseconds: 1500),
      vsync: this,
    );
    
    _slideController = AnimationController(
      duration: Duration(milliseconds: 1200),
      vsync: this,
    );
    
    _buttonPulseController = AnimationController(
      duration: Duration(milliseconds: 2000),
      vsync: this,
    );
    
    _fadeAnimation = Tween<double>(
      begin: 0.0,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _fadeController,
      curve: Curves.easeInOut,
    ));
    
    _slideAnimation = Tween<Offset>(
      begin: Offset(0, 0.3),
      end: Offset.zero,
    ).animate(CurvedAnimation(
      parent: _slideController,
      curve: Curves.easeOutCubic,
    ));
    
    _buttonPulseAnimation = Tween<double>(
      begin: 1.0,
      end: 1.02,
    ).animate(CurvedAnimation(
      parent: _buttonPulseController,
      curve: Curves.easeInOut,
    ));
    
    // Password strength listener
    _passwordController.addListener(() {
      setState(() {
        _passwordStrength = PasswordStrength.calculate(_passwordController.text);
      });
    });
    
    // Start animations
    _fadeController.forward();
    _slideController.forward();
    _buttonPulseController.repeat(reverse: true);
  }


  @override
  void dispose() {
    _nameController.dispose();
    _emailController.dispose();
    _passwordController.dispose();
    _confirmPasswordController.dispose();
    _fadeController.dispose();
    _slideController.dispose();
    _buttonPulseController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.backgroundDark,
      body: KeyboardDismissalWrapper(
        child: SafeArea(
          child: FadeTransition(
            opacity: _fadeAnimation,
            child: SlideTransition(
              position: _slideAnimation,
              child: Padding(
                padding: EdgeInsets.symmetric(horizontal: 24.w),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    SizedBox(height: 20.h),
                    
                    // Back button
                    _buildBackButton(),
                    
                    SizedBox(height: 16.h),
                    
                    // Header
                    _buildHeader(),
                    
                    SizedBox(height: 24.h),
                    
                    // Sign up form
                    Expanded(
                      child: _buildSignUpForm(),
                    ),
                    
                    // Login link
                    _buildLoginLink(),
                    
                    SizedBox(height: 20.h),
                  ],
                ),
              ),
            ),
          ),
        ),
      ),
    );
  }

  Widget _buildBackButton() {
    return GestureDetector(
      onTap: () => Navigator.pop(context),
      child: Container(
        width: 44.w,
        height: 44.h,
        decoration: BoxDecoration(
          color: AppColors.cardBackground,
          borderRadius: BorderRadius.circular(12.r),
        ),
        child: Icon(
          Icons.arrow_back,
          color: AppColors.textLight,
          size: 20.sp,
        ),
      ),
    );
  }

  Widget _buildHeader() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Create Account',
          style: TextStyle(
            fontSize: 32.sp,
            fontWeight: FontWeight.bold,
            color: AppColors.textLight,
          ),
        ),
        
        SizedBox(height: 8.h),
        
        Text(
          'Join our community and start manifesting your dreams',
          style: TextStyle(
            fontSize: 16.sp,
            color: AppColors.textMedium,
            height: 1.3,
          ),
        ),
      ],
    );
  }

  Widget _buildSignUpForm() {
    return SingleChildScrollView(
      child: Form(
        key: _formKey,
        child: Column(
          children: [
            // Name field
            _buildTextField(
              controller: _nameController,
              label: 'Full Name',
              hint: 'Enter your full name',
              icon: Icons.person_outline,
              validator: AuthValidation.validateDisplayName,
            ),
            
            SizedBox(height: 16.h),
            
            // Email field
            _buildTextField(
              controller: _emailController,
              label: 'Email',
              hint: 'Enter your email',
              icon: Icons.email_outlined,
              keyboardType: TextInputType.emailAddress,
              validator: AuthValidation.validateEmail,
            ),
            
            SizedBox(height: 16.h),
            
            // Password field
            _buildTextField(
              controller: _passwordController,
              label: 'Password',
              hint: 'Create a strong password',
              icon: Icons.lock_outline,
              isPassword: true,
              validator: AuthValidation.validatePassword,
            ),
            
            // Password strength indicator - always allocated space
            SizedBox(height: 8.h),
            _buildPasswordStrengthIndicator(),
            
            SizedBox(height: 16.h),
            
            // Confirm password field
            _buildTextField(
              controller: _confirmPasswordController,
              label: 'Confirm Password',
              hint: 'Confirm your password',
              icon: Icons.lock_outline,
              isPassword: true,
              isConfirmPassword: true,
              validator: (value) => AuthValidation.validateConfirmPassword(
                _passwordController.text,
                value,
              ),
            ),
            
            SizedBox(height: 20.h),
            
            // Terms and conditions
            _buildTermsCheckbox(),
            
            SizedBox(height: 12.h),
            
            // Newsletter subscription
            _buildNewsletterCheckbox(),
            
            // Fixed space for validation errors
            _buildValidationErrorContainer(),
            
            SizedBox(height: 16.h),
            
            // Sign up button
            _buildSignUpButton(),
          ],
        ),
      ),
    );
  }

  Widget _buildTextField({
    required TextEditingController controller,
    required String label,
    required String hint,
    required IconData icon,
    bool isPassword = false,
    bool isConfirmPassword = false,
    TextInputType? keyboardType,
    String? Function(String?)? validator,
  }) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          label,
          style: TextStyle(
            fontSize: 16.sp,
            fontWeight: FontWeight.w600,
            color: AppColors.textLight,
          ),
        ),
        SizedBox(height: 8.h),
        TextFormField(
          controller: controller,
          validator: validator,
          obscureText: isPassword && (isConfirmPassword 
              ? !_isConfirmPasswordVisible 
              : !_isPasswordVisible),
          keyboardType: keyboardType,
          style: TextStyle(
            color: AppColors.textLight,
            fontSize: 16.sp,
          ),
          decoration: InputDecoration(
            hintText: hint,
            hintStyle: TextStyle(
              color: AppColors.textMedium.withOpacity(0.6),
            ),
            prefixIcon: Icon(
              icon,
              color: AppColors.textMedium,
              size: 20.sp,
            ),
            suffixIcon: isPassword
                ? GestureDetector(
                    onTap: () {
                      setState(() {
                        if (isConfirmPassword) {
                          _isConfirmPasswordVisible = !_isConfirmPasswordVisible;
                        } else {
                          _isPasswordVisible = !_isPasswordVisible;
                        }
                      });
                    },
                    child: Icon(
                      (isConfirmPassword ? _isConfirmPasswordVisible : _isPasswordVisible)
                          ? Icons.visibility_off_outlined
                          : Icons.visibility_outlined,
                      color: AppColors.textMedium,
                      size: 20.sp,
                    ),
                  )
                : null,
            filled: true,
            fillColor: AppColors.cardBackground,
            border: OutlineInputBorder(
              borderRadius: BorderRadius.circular(16.r),
              borderSide: BorderSide.none,
            ),
            enabledBorder: OutlineInputBorder(
              borderRadius: BorderRadius.circular(16.r),
              borderSide: BorderSide(
                color: AppColors.textMedium.withOpacity(0.1),
                width: 2,
              ),
            ),
            focusedBorder: OutlineInputBorder(
              borderRadius: BorderRadius.circular(16.r),
              borderSide: BorderSide(
                color: AppColors.socialGradient[0],
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
            // Remove error text to prevent jumping
            errorStyle: TextStyle(height: 0, fontSize: 0),
            contentPadding: EdgeInsets.symmetric(
              horizontal: 16.w,
              vertical: 16.h,
            ),
          ),
        ),
      ],
    );
  }

  Widget _buildPasswordStrengthIndicator() {
    // Always allocate fixed space to prevent layout jumping
    return Container(
      height: 40.h, // Fixed height to prevent jumping
      child: AnimatedOpacity(
        opacity: _passwordController.text.isNotEmpty ? 1.0 : 0.0,
        duration: Duration(milliseconds: 300),
        curve: Curves.easeInOut,
        child: _passwordController.text.isNotEmpty 
          ? Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Row(
                  children: [
                    Text(
                      'Password strength: ',
                      style: TextStyle(
                        fontSize: 12.sp,
                        color: AppColors.textMedium,
                      ),
                    ),
                    Text(
                      _passwordStrength.label,
                      style: TextStyle(
                        fontSize: 12.sp,
                        fontWeight: FontWeight.w600,
                        color: _passwordStrength.color,
                      ),
                    ),
                  ],
                ),
                SizedBox(height: 6.h),
                AnimatedContainer(
                  duration: Duration(milliseconds: 300),
                  curve: Curves.easeInOut,
                  child: LinearProgressIndicator(
                    value: _passwordStrength.strength,
                    backgroundColor: AppColors.textMedium.withOpacity(0.2),
                    valueColor: AlwaysStoppedAnimation<Color>(_passwordStrength.color),
                    minHeight: 4.h,
                  ),
                ),
              ],
            )
          : SizedBox.shrink(), // Invisible placeholder
      ),
    );
  }

  Widget _buildTermsCheckbox() {
    return GestureDetector(
      onTap: () {
        setState(() {
          _agreeToTerms = !_agreeToTerms;
        });
      },
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Container(
            width: 20.w,
            height: 20.h,
            decoration: BoxDecoration(
              color: _agreeToTerms 
                  ? AppColors.socialGradient[0]
                  : Colors.transparent,
              borderRadius: BorderRadius.circular(4.r),
              border: Border.all(
                color: _agreeToTerms 
                    ? AppColors.socialGradient[0]
                    : AppColors.textMedium.withOpacity(0.5),
                width: 2,
              ),
            ),
            child: _agreeToTerms
                ? Icon(
                    Icons.check,
                    color: AppColors.white,
                    size: 14.sp,
                  )
                : null,
          ),
          SizedBox(width: 12.w),
          Expanded(
            child: RichText(
              text: TextSpan(
                style: TextStyle(
                  fontSize: 14.sp,
                  color: AppColors.textMedium,
                  height: 1.3,
                ),
                children: [
                  TextSpan(text: 'I agree to the '),
                  TextSpan(
                    text: 'Terms of Service',
                    style: TextStyle(
                      color: AppColors.socialGradient[0],
                      fontWeight: FontWeight.w600,
                    ),
                  ),
                  TextSpan(text: ' and '),
                  TextSpan(
                    text: 'Privacy Policy',
                    style: TextStyle(
                      color: AppColors.socialGradient[0],
                      fontWeight: FontWeight.w600,
                    ),
                  ),
                  TextSpan(text: ' *'),
                ],
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildNewsletterCheckbox() {
    return GestureDetector(
      onTap: () {
        setState(() {
          _subscribeToNewsletter = !_subscribeToNewsletter;
        });
      },
      child: Row(
        children: [
          Container(
            width: 20.w,
            height: 20.h,
            decoration: BoxDecoration(
              color: _subscribeToNewsletter 
                  ? AppColors.socialGradient[0]
                  : Colors.transparent,
              borderRadius: BorderRadius.circular(4.r),
              border: Border.all(
                color: _subscribeToNewsletter 
                    ? AppColors.socialGradient[0]
                    : AppColors.textMedium.withOpacity(0.5),
                width: 2,
              ),
            ),
            child: _subscribeToNewsletter
                ? Icon(
                    Icons.check,
                    color: AppColors.white,
                    size: 14.sp,
                  )
                : null,
          ),
          SizedBox(width: 12.w),
          Expanded(
            child: Text(
              'Send me manifestation tips and updates',
              style: TextStyle(
                fontSize: 14.sp,
                color: AppColors.textMedium,
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildValidationErrorContainer() {
    return Container(
      height: 30.h, // Fixed space for validation errors
      margin: EdgeInsets.only(top: 8.h),
      child: AnimatedOpacity(
        opacity: _validationError.isNotEmpty ? 1.0 : 0.0,
        duration: Duration(milliseconds: 300),
        curve: Curves.easeInOut,
        child: _validationError.isNotEmpty
            ? Container(
                padding: EdgeInsets.symmetric(horizontal: 12.w, vertical: 6.h),
                decoration: BoxDecoration(
                  color: Colors.red.withOpacity(0.1),
                  borderRadius: BorderRadius.circular(8.r),
                  border: Border.all(
                    color: Colors.red.withOpacity(0.3),
                    width: 1,
                  ),
                ),
                child: Row(
                  children: [
                    Icon(
                      Icons.error_outline,
                      color: Colors.red,
                      size: 16.sp,
                    ),
                    SizedBox(width: 8.w),
                    Expanded(
                      child: Text(
                        _validationError,
                        style: TextStyle(
                          color: Colors.red,
                          fontSize: 12.sp,
                          fontWeight: FontWeight.w500,
                        ),
                        maxLines: 1,
                        overflow: TextOverflow.ellipsis,
                      ),
                    ),
                  ],
                ),
              )
            : SizedBox.shrink(),
      ),
    );
  }

  Widget _buildSignUpButton() {
    final bool isButtonEnabled = _agreeToTerms && !_isLoading;
    
    return AnimatedBuilder(
      animation: _buttonPulseAnimation,
      builder: (context, child) {
        return Transform.scale(
          scale: (!isButtonEnabled || _isLoading) ? 1.0 : _buttonPulseAnimation.value,
          child: SizedBox(
            width: double.infinity,
            height: 56.h,
            child: ElevatedButton(
              onPressed: isButtonEnabled ? _handleSignUp : null,
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
                      ? LinearGradient(colors: AppColors.socialGradient)
                      : null,
                  color: isButtonEnabled
                      ? null
                      : AppColors.textMedium.withOpacity(0.3),
                  borderRadius: BorderRadius.circular(16.r),
                  boxShadow: isButtonEnabled ? [
                    BoxShadow(
                      color: AppColors.socialGradient[0].withOpacity(0.4),
                      blurRadius: 20.r,
                      offset: Offset(0, 8.h),
                      spreadRadius: 2.r,
                    ),
                    BoxShadow(
                      color: AppColors.socialGradient[1].withOpacity(0.2),
                      blurRadius: 40.r,
                      offset: Offset(0, 16.h),
                    ),
                  ] : null,
                ),
                child: Container(
                  alignment: Alignment.center,
                  child: _isLoading
                      ? CompactStylishLoading(size: 20.w, color: AppColors.white)
                      : Text(
                          'Create Account',
                          style: TextStyle(
                            fontSize: 18.sp,
                            fontWeight: FontWeight.w600,
                            color: AppColors.white,
                            letterSpacing: 0.5,
                          ),
                        ),
                ),
              ),
            ),
          ),
        );
      },
    );
  }


  Widget _buildLoginLink() {
    return Center(
      child: GestureDetector(
        onTap: () => Navigator.pop(context),
        child: RichText(
          text: TextSpan(
            style: TextStyle(
              fontSize: 16.sp,
              color: AppColors.textMedium,
            ),
            children: [
              TextSpan(text: "Already have an account? "),
              TextSpan(
                text: 'Sign In',
                style: TextStyle(
                  color: AppColors.socialGradient[0],
                  fontWeight: FontWeight.w600,
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }

  void _handleSignUp() async {
    // Clear previous validation error
    setState(() {
      _validationError = '';
    });
    
    if (!_formKey.currentState!.validate()) return;
    if (!_agreeToTerms) {
      setState(() {
        _validationError = 'Please agree to the Terms of Service and Privacy Policy';
      });
      return;
    }

    setState(() {
      _isLoading = true;
    });

    try {
      // Parse the full name into first and last name
      final nameParts = _nameController.text.trim().split(' ');
      final firstName = nameParts.isNotEmpty ? nameParts.first : '';
      final lastName = nameParts.length > 1 ? nameParts.sublist(1).join(' ') : '';

      final authService = UnifiedAuthService();
      
      final result = await authService.createAccount(
        email: _emailController.text.trim(),
        firstName: firstName,
        lastName: lastName,
        password: _passwordController.text.trim(),
        captchaCode: 'dummy_captcha', // TODO: Add real captcha
        rememberMe: true, // Remember new users by default
      );

      if (result.isSuccess) {
        // Show success message
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text(result.message),
            backgroundColor: Colors.green,
            behavior: SnackBarBehavior.floating,
            shape: RoundedRectangleBorder(
              borderRadius: BorderRadius.circular(12.r),
            ),
          ),
        );

        // Navigate to onboarding flow for new users
        Navigator.pushReplacement(
          context,
          MaterialPageRoute(builder: (context) => PostSignupOnboardingScreen()),
        );
      } else {
        // Handle specific signup errors
        setState(() {
          _validationError = _getSignupErrorMessage(result.message);
        });
      }
    } catch (e) {
      setState(() {
        _validationError = 'Connection error. Please check your internet and try again.';
      });
    } finally {
      setState(() {
        _isLoading = false;
      });
    }
  }

  void _showOtpDialog() {
    final otpController = TextEditingController();
    bool isVerifying = false;

    showDialog(
      context: context,
      barrierDismissible: false,
      builder: (context) => StatefulBuilder(
        builder: (context, setDialogState) => AlertDialog(
          backgroundColor: AppColors.cardBackground,
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(16.r),
          ),
          title: Text(
            'Verify Email',
            style: TextStyle(
              color: AppColors.textLight,
              fontSize: 20.sp,
              fontWeight: FontWeight.bold,
            ),
          ),
          content: Column(
            mainAxisSize: MainAxisSize.min,
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(
                'We\'ve sent a verification code to:',
                style: TextStyle(
                  color: AppColors.textMedium,
                  fontSize: 14.sp,
                ),
              ),
              SizedBox(height: 4.h),
              Text(
                _emailController.text.trim(),
                style: TextStyle(
                  color: AppColors.textLight,
                  fontSize: 16.sp,
                  fontWeight: FontWeight.w600,
                ),
              ),
              SizedBox(height: 20.h),
              TextField(
                controller: otpController,
                keyboardType: TextInputType.number,
                maxLength: 6,
                style: TextStyle(
                  color: AppColors.textLight,
                  fontSize: 18.sp,
                  fontWeight: FontWeight.w600,
                  letterSpacing: 2.0,
                ),
                decoration: InputDecoration(
                  hintText: 'Enter 6-digit code',
                  hintStyle: TextStyle(
                    color: AppColors.textMedium.withOpacity(0.6),
                  ),
                  filled: true,
                  fillColor: AppColors.backgroundDark,
                  border: OutlineInputBorder(
                    borderRadius: BorderRadius.circular(12.r),
                    borderSide: BorderSide.none,
                  ),
                  contentPadding: EdgeInsets.symmetric(
                    horizontal: 16.w,
                    vertical: 16.h,
                  ),
                  counterText: '',
                ),
              ),
            ],
          ),
          actions: [
            TextButton(
              onPressed: isVerifying ? null : () => Navigator.pop(context),
              child: Text(
                'Cancel',
                style: TextStyle(
                  color: AppColors.textMedium,
                  fontSize: 16.sp,
                ),
              ),
            ),
            ElevatedButton(
              onPressed: isVerifying ? null : () async {
                if (otpController.text.length != 6) {
                  ScaffoldMessenger.of(context).showSnackBar(
                    SnackBar(
                      content: Text('Please enter a 6-digit code'),
                      backgroundColor: Colors.red,
                    ),
                  );
                  return;
                }

                setDialogState(() {
                  isVerifying = true;
                });

                try {
                  final otpCode = int.parse(otpController.text);
                  // TODO: Implement OTP verification with generated API
                  // This is a placeholder implementation as OTP verification endpoint
                  // is not yet available in the generated API
                  
                  try {
                    Navigator.pop(context); // Close dialog
                    
                    ScaffoldMessenger.of(context).showSnackBar(
                      SnackBar(
                        content: Text('OTP verification successful! Proceeding to home.'),
                        backgroundColor: Colors.green,
                        behavior: SnackBarBehavior.floating,
                        shape: RoundedRectangleBorder(
                          borderRadius: BorderRadius.circular(12.r),
                        ),
                      ),
                    );

                    // Navigate to onboarding flow
                    Navigator.pushReplacement(
                      context,
                      MaterialPageRoute(builder: (context) => PostSignupOnboardingScreen()),
                    );
                  } catch (e) {
                    ScaffoldMessenger.of(context).showSnackBar(
                      SnackBar(
                        content: Text('OTP verification failed: ${e.toString()}'),
                        backgroundColor: Colors.red,
                      ),
                    );
                  }
                } catch (e) {
                  ScaffoldMessenger.of(context).showSnackBar(
                    SnackBar(
                      content: Text('Please enter a valid 6-digit number'),
                      backgroundColor: Colors.red,
                    ),
                  );
                } finally {
                  setDialogState(() {
                    isVerifying = false;
                  });
                }
              },
              style: ElevatedButton.styleFrom(
                backgroundColor: AppColors.affirmationGradient[0],
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(12.r),
                ),
              ),
              child: isVerifying
                  ? CompactStylishLoading(size: 18.w)
                  : Text(
                      'Verify',
                      style: TextStyle(
                        color: AppColors.white,
                        fontSize: 16.sp,
                        fontWeight: FontWeight.w600,
                      ),
                    ),
            ),
          ],
        ),
      ),
    );
  }


  String _getSignupErrorMessage(String apiMessage) {
    final message = apiMessage.toLowerCase();
    
    if (message.contains('email already exists') || message.contains('email already registered') || message.contains('duplicate email')) {
      return 'This email is already registered. Please use a different email or sign in.';
    } else if (message.contains('username already exists') || message.contains('username taken') || message.contains('duplicate username')) {
      return 'This username is already taken. Please choose a different username.';
    } else if (message.contains('invalid email') || message.contains('email format')) {
      return 'Please enter a valid email address.';
    } else if (message.contains('password too weak') || message.contains('password requirements')) {
      return 'Password is too weak. Please use a stronger password.';
    } else if (message.contains('network') || message.contains('connection') || message.contains('timeout')) {
      return 'Connection error. Please check your internet and try again.';
    } else if (message.contains('server') || message.contains('internal error')) {
      return 'Server error. Please try again in a moment.';
    } else if (message.contains('captcha') || message.contains('verification')) {
      return 'Verification failed. Please try again.';
    } else {
      // Fallback for any other error messages
      return 'Account creation failed. Please check your information and try again.';
    }
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
}