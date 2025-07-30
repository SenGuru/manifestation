import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../models/auth_models.dart';
import '../services/unified_auth_service.dart';
import 'post_signup_onboarding.dart';
import '../constants/colors.dart';
import '../constants/app_theme.dart';
import '../widgets/stylish_loading.dart';
import '../widgets/keyboard_dismissal_wrapper.dart';
import '../widgets/universal_back_button.dart';
import '../components/error_popup.dart';

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
  
  // Field error states
  String? _nameError;
  String? _emailError;
  String? _passwordError;
  String? _confirmPasswordError;
  
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
    
    // Password strength and validation listeners
    _passwordController.addListener(() {
      setState(() {
        _passwordStrength = PasswordStrength.calculate(_passwordController.text);
        _passwordError = AuthValidation.validatePassword(_passwordController.text);
        _confirmPasswordError = AuthValidation.validateConfirmPassword(
          _passwordController.text, 
          _confirmPasswordController.text
        );
      });
    });
    
    _nameController.addListener(() {
      setState(() {
        _nameError = AuthValidation.validateDisplayName(_nameController.text);
      });
    });
    
    _emailController.addListener(() {
      setState(() {
        _emailError = AuthValidation.validateEmail(_emailController.text);
      });
    });
    
    _confirmPasswordController.addListener(() {
      setState(() {
        _confirmPasswordError = AuthValidation.validateConfirmPassword(
          _passwordController.text, 
          _confirmPasswordController.text
        );
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
    final bool isKeyboardOpen = MediaQuery.of(context).viewInsets.bottom > 0;
    
    return Scaffold(
      backgroundColor: AppTheme.backgroundDark,
      resizeToAvoidBottomInset: false,
      body: KeyboardDismissalWrapper(
        child: SafeArea(
          child: FadeTransition(
            opacity: _fadeAnimation,
            child: SlideTransition(
              position: _slideAnimation,
              child: isKeyboardOpen
                  ? SingleChildScrollView(
                      physics: ClampingScrollPhysics(),
                      padding: EdgeInsets.only(
                        left: 24.w,
                        right: 24.w,
                        bottom: MediaQuery.of(context).viewInsets.bottom + 16.h,
                      ),
                      child: _buildContent(),
                    )
                  : Padding(
                      padding: EdgeInsets.symmetric(horizontal: 24.w),
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          SizedBox(height: 16.h),
                          
                          // Back button with header inline
                          _buildBackButtonWithHeader(),
                          
                          SizedBox(height: 18.h),
                          
                          // Sign up form
                          Expanded(
                            child: _buildSignUpForm(),
                          ),
                          
                          // Login link
                          _buildLoginLink(),
                          
                          SizedBox(height: 16.h),
                        ],
                      ),
                    ),
            ),
          ),
        ),
      ),
    );
  }

  Widget _buildContent() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        SizedBox(height: 16.h),
        
        // Back button with header inline
        _buildBackButtonWithHeader(),
        
        SizedBox(height: 18.h),
        
        // Sign up form
        _buildSignUpForm(),
        
        SizedBox(height: 16.h),
        
        // Login link
        _buildLoginLink(),
        
        SizedBox(height: 16.h),
      ],
    );
  }

  Widget _buildBackButtonWithHeader() {
    return Row(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        // Back button
        UniversalBackButton(),
        
        SizedBox(width: 16.w),
        
        // Header text
        Expanded(
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(
                'Create Account',
                style: TextStyle(
                  fontSize: 28.sp, // Slightly smaller to fit better
                  fontWeight: FontWeight.bold,
                  color: AppTheme.textLight,
                ),
              ),
              
              SizedBox(height: 4.h),
              
              Text(
                'Join our community and start manifesting your dreams',
                style: TextStyle(
                  fontSize: 14.sp, // Slightly smaller to fit better
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

  Widget _buildSignUpForm() {
    return Form(
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
          
          SizedBox(height: 12.h),
          
          // Email field
          _buildTextField(
            controller: _emailController,
            label: 'Email',
            hint: 'Enter your email',
            icon: Icons.email_outlined,
            keyboardType: TextInputType.emailAddress,
            validator: AuthValidation.validateEmail,
          ),
          
          SizedBox(height: 12.h),
          
          // Password field
          _buildTextField(
            controller: _passwordController,
            label: 'Password',
            hint: 'Create a strong password',
            icon: Icons.lock_outline,
            isPassword: true,
            validator: AuthValidation.validatePassword,
          ),
          
          // Animated password strength indicator
          _buildAnimatedPasswordStrengthIndicator(),
          
          SizedBox(height: 12.h),
          
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
          
          SizedBox(height: 16.h),
          
          // Terms and conditions
          _buildTermsCheckbox(),
          
          SizedBox(height: 8.h),
          
          // Newsletter subscription
          _buildNewsletterCheckbox(),
          
          // Fixed space for validation errors
          _buildValidationErrorContainer(),
          
          SizedBox(height: 12.h),
          
          // Sign up button
          _buildSignUpButton(),
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
            color: AppTheme.textLight,
          ),
        ),
        SizedBox(height: 6.h),
        
        // TextField with custom error handling
        Container(
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              TextFormField(
                controller: controller,
                validator: (value) {
                  // Store validation result but don't show in TextFormField
                  final result = validator?.call(value);
                  return null; // Always return null to prevent default error display
                },
                obscureText: isPassword && (isConfirmPassword 
                    ? !_isConfirmPasswordVisible 
                    : !_isPasswordVisible),
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
              
              // Fixed space for individual field errors
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
    if (controller == _nameController) return _nameError;
    if (controller == _emailController) return _emailError;
    if (controller == _passwordController) return _passwordError;
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

  Widget _buildAnimatedPasswordStrengthIndicator() {
    return AnimatedContainer(
      duration: Duration(milliseconds: 400),
      curve: Curves.easeInOutCubic,
      height: _passwordController.text.isNotEmpty ? 48.h : 0.h,
      clipBehavior: Clip.hardEdge,
      decoration: BoxDecoration(),
      child: AnimatedOpacity(
        opacity: _passwordController.text.isNotEmpty ? 1.0 : 0.0,
        duration: Duration(milliseconds: 200),
        curve: Curves.easeInOut,
        child: AnimatedSlide(
          duration: Duration(milliseconds: 300),
          curve: Curves.easeOutCubic,
          offset: _passwordController.text.isNotEmpty 
              ? Offset(0, 0) 
              : Offset(0, -0.3),
          child: SingleChildScrollView(
            physics: NeverScrollableScrollPhysics(),
            child: Container(
              margin: EdgeInsets.only(top: 8.h),
              child: Column(
                mainAxisSize: MainAxisSize.min,
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Row(
                    children: [
                      Flexible(
                        child: Text(
                          'Password strength: ',
                          style: TextStyle(
                            fontSize: 12.sp,
                            color: AppTheme.textMedium,
                          ),
                          overflow: TextOverflow.ellipsis,
                        ),
                      ),
                      Flexible(
                        child: Text(
                          _passwordStrength.label,
                          style: TextStyle(
                            fontSize: 12.sp,
                            fontWeight: FontWeight.w600,
                            color: _passwordStrength.color,
                          ),
                          overflow: TextOverflow.ellipsis,
                        ),
                      ),
                    ],
                  ),
                  SizedBox(height: 6.h),
                  TweenAnimationBuilder<double>(
                    duration: Duration(milliseconds: 500),
                    curve: Curves.easeInOutCubic,
                    tween: Tween(begin: 0.0, end: _passwordStrength.strength),
                    builder: (context, value, child) {
                      return LinearProgressIndicator(
                        value: value,
                        backgroundColor: AppTheme.textMedium.withOpacity(0.2),
                        valueColor: AlwaysStoppedAnimation<Color>(_passwordStrength.color),
                        minHeight: 4.h,
                      );
                    },
                  ),
                ],
              ),
            ),
          ),
        ),
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
                  ? AppTheme.socialGradient[0]
                  : Colors.transparent,
              borderRadius: BorderRadius.circular(4.r),
              border: Border.all(
                color: _agreeToTerms 
                    ? AppTheme.socialGradient[0]
                    : AppTheme.textMedium.withOpacity(0.5),
                width: 2,
              ),
            ),
            child: _agreeToTerms
                ? Icon(
                    Icons.check,
                    color: AppTheme.white,
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
                  color: AppTheme.textMedium,
                  height: 1.3,
                ),
                children: [
                  TextSpan(text: 'I agree to the '),
                  TextSpan(
                    text: 'Terms of Service',
                    style: TextStyle(
                      color: AppTheme.socialGradient[0],
                      fontWeight: FontWeight.w600,
                    ),
                  ),
                  TextSpan(text: ' and '),
                  TextSpan(
                    text: 'Privacy Policy',
                    style: TextStyle(
                      color: AppTheme.socialGradient[0],
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
                  ? AppTheme.socialGradient[0]
                  : Colors.transparent,
              borderRadius: BorderRadius.circular(4.r),
              border: Border.all(
                color: _subscribeToNewsletter 
                    ? AppTheme.socialGradient[0]
                    : AppTheme.textMedium.withOpacity(0.5),
                width: 2,
              ),
            ),
            child: _subscribeToNewsletter
                ? Icon(
                    Icons.check,
                    color: AppTheme.white,
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
                color: AppTheme.textMedium,
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildValidationErrorContainer() {
    return Container(
      height: 16.h, // Further reduced space for validation errors
      margin: EdgeInsets.only(top: 2.h),
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
                    BoxShadow(
                      color: AppTheme.socialGradient[1].withOpacity(0.2),
                      blurRadius: 40.r,
                      offset: Offset(0, 16.h),
                    ),
                  ] : null,
                ),
                child: Container(
                  alignment: Alignment.center,
                  child: _isLoading
                      ? CompactStylishLoading(size: 20.w, color: AppTheme.white)
                      : Text(
                          'Create Account',
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
              color: AppTheme.textMedium,
            ),
            children: [
              TextSpan(text: "Already have an account? "),
              TextSpan(
                text: 'Sign In',
                style: TextStyle(
                  color: AppTheme.socialGradient[0],
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
    
    // Custom validation since we disabled TextFormField's built-in validation
    bool hasErrors = false;
    
    if (_nameError != null) hasErrors = true;
    if (_emailError != null) hasErrors = true;
    if (_passwordError != null) hasErrors = true;
    if (_confirmPasswordError != null) hasErrors = true;
    
    if (hasErrors) {
      return;
    }
    
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
        // Show success popup before navigation
        SuccessPopup.show(
          context,
          title: 'Account Created!',
          message: 'Welcome to our manifestation community! Let\'s get you started on your journey.',
          actionText: 'Begin Journey',
          onAction: () {
            Navigator.pushReplacement(
              context,
              MaterialPageRoute(builder: (context) => PostSignupOnboardingScreen()),
            );
          },
          onDismiss: () {
            Navigator.pushReplacement(
              context,
              MaterialPageRoute(builder: (context) => PostSignupOnboardingScreen()),
            );
          },
        );
      } else {
        // Show beautiful error popup instead of validation error
        ErrorPopup.show(
          context,
          title: 'Account Creation Failed',
          message: _getSignupErrorMessage(result.message),
          actionText: 'Try Again',
          onAction: () {
            // Clear sensitive fields
            _passwordController.clear();
            _confirmPasswordController.clear();
            FocusScope.of(context).requestFocus(FocusNode());
          },
        );
      }
    } catch (e) {
      ErrorPopup.show(
        context,
        title: 'Connection Error',
        message: 'Unable to create your account right now. Please check your internet connection and try again.',
        actionText: 'Retry',
        onAction: () => _handleSignUp(),
      );
    } finally {
      setState(() {
        _isLoading = false;
      });
    }
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
}