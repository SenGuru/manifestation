import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../models/auth_models.dart';
import '../services/unified_auth_service.dart';
import 'signup_page.dart';
import 'home_screen.dart';
import 'forgot_password_screen.dart';
import '../constants/colors.dart';
import '../constants/app_theme.dart';
import '../widgets/stylish_loading.dart';
import '../widgets/keyboard_dismissal_wrapper.dart';
import '../components/error_popup.dart';

class LoginScreen extends StatefulWidget {
  @override
  _LoginScreenState createState() => _LoginScreenState();
}

class _LoginScreenState extends State<LoginScreen>
    with TickerProviderStateMixin {
  final _formKey = GlobalKey<FormState>();
  final _emailController = TextEditingController();
  final _passwordController = TextEditingController();
  
  bool _isPasswordVisible = false;
  bool _rememberMe = false;
  bool _isLoading = false;
  String _validationError = '';
  
  // Field error states
  String? _usernameError;
  String? _passwordError;
  
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
    
    // Start animations
    _fadeController.forward();
    _slideController.forward();
    _buttonPulseController.repeat(reverse: true);
    
    // Validation listeners
    _emailController.addListener(() {
      setState(() {
        _usernameError = _emailController.text.trim().isEmpty ? 'Username is required' : null;
      });
    });
    
    _passwordController.addListener(() {
      setState(() {
        _passwordError = _passwordController.text.trim().isEmpty ? 'Password is required' : null;
      });
    });
  }


  @override
  void dispose() {
    _emailController.dispose();
    _passwordController.dispose();
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
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          SizedBox(height: 40.h),
                          _buildHeader(),
                          SizedBox(height: 40.h),
                          _buildLoginForm(),
                          SizedBox(height: 24.h),
                          _buildSignUpLink(),
                          SizedBox(height: 16.h),
                        ],
                      ),
                    )
                  : Padding(
                      padding: EdgeInsets.symmetric(horizontal: 24.w),
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          SizedBox(height: 40.h),
                          _buildHeader(),
                          SizedBox(height: 40.h),
                          _buildLoginForm(),
                          SizedBox(height: 24.h), // Same spacing as keyboard mode
                          _buildSignUpLink(),
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

  Widget _buildHeader() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        // Logo/Icon
        Container(
          width: 80.w,
          height: 80.h,
          decoration: BoxDecoration(
            gradient: LinearGradient(
              colors: AppTheme.affirmationGradient,
              begin: Alignment.topLeft,
              end: Alignment.bottomRight,
            ),
            shape: BoxShape.circle,
            boxShadow: [
              BoxShadow(
                color: AppTheme.affirmationGradient[0].withOpacity(0.3),
                blurRadius: 20.r,
                offset: Offset(0, 10.h),
              ),
            ],
          ),
          child: Icon(
            Icons.auto_awesome,
            color: AppTheme.white,
            size: 40.sp,
          ),
        ),
        
        SizedBox(height: 32.h),
        
        Text(
          'Welcome Back',
          style: TextStyle(
            fontSize: 32.sp,
            fontWeight: FontWeight.bold,
            color: AppTheme.textLight,
          ),
        ),
        
        SizedBox(height: 8.h),
        
        Text(
          'Sign in to continue your manifestation journey',
          style: TextStyle(
            fontSize: 16.sp,
            color: AppTheme.textMedium,
            height: 1.3,
          ),
        ),
      ],
    );
  }


  Widget _buildLoginForm() {
    return Form(
      key: _formKey,
      child: Column(
        children: [
          // Username field
          _buildTextField(
            controller: _emailController,
            label: 'Username',
            hint: 'Enter your username',
            icon: Icons.person_outline,
            validator: (value) {
              if (value == null || value.isEmpty) {
                return 'Username is required';
              }
              return null;
            },
          ),
          
          SizedBox(height: 20.h),
          
          // Password field
          _buildTextField(
            controller: _passwordController,
            label: 'Password',
            hint: 'Enter your password',
            icon: Icons.lock_outline,
            isPassword: true,
            validator: (value) {
              if (value == null || value.isEmpty) {
                return 'Password is required';
              }
              return null;
            },
          ),
          
          SizedBox(height: 16.h),
          
          // Remember me & Forgot password
          Row(
            children: [
              GestureDetector(
                onTap: () {
                  setState(() {
                    _rememberMe = !_rememberMe;
                  });
                },
                child: Row(
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    Container(
                      width: 20.w,
                      height: 20.h,
                      decoration: BoxDecoration(
                        color: _rememberMe 
                            ? AppTheme.affirmationGradient[0]
                            : Colors.transparent,
                        borderRadius: BorderRadius.circular(4.r),
                        border: Border.all(
                          color: _rememberMe 
                              ? AppTheme.affirmationGradient[0]
                              : AppTheme.textMedium.withOpacity(0.5),
                          width: 2,
                        ),
                      ),
                      child: _rememberMe
                          ? Icon(
                              Icons.check,
                              color: AppTheme.white,
                              size: 14.sp,
                            )
                          : null,
                    ),
                    SizedBox(width: 8.w),
                    Text(
                      'Remember me',
                      style: TextStyle(
                        fontSize: 14.sp,
                        color: AppTheme.textMedium,
                      ),
                    ),
                  ],
                ),
              ),
              
              Spacer(),
              
              GestureDetector(
                onTap: _navigateToForgotPassword,
                child: Text(
                  'Forgot Password?',
                  style: TextStyle(
                    fontSize: 14.sp,
                    color: AppTheme.affirmationGradient[0],
                    fontWeight: FontWeight.w600,
                  ),
                ),
              ),
            ],
          ),
          
          // Fixed space for validation errors
          _buildValidationErrorContainer(),
          
          SizedBox(height: 16.h),
          
          // Login button
          _buildPrimaryLoginButton(),
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
        SizedBox(height: 8.h),
        
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
                            _isPasswordVisible
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
                      color: AppTheme.affirmationGradient[0],
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
                height: 20.h,
                margin: EdgeInsets.only(top: 4.h),
                child: _buildFieldError(_getFieldError(controller)),
              ),
            ],
          ),
        ),
      ],
    );
  }
  
  String? _getFieldError(TextEditingController controller) {
    if (controller == _emailController) return _usernameError;
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

  Widget _buildValidationErrorContainer() {
    return Container(
      height: 30.h, // Fixed space for validation errors
      margin: EdgeInsets.only(top: 16.h),
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

  Widget _buildPrimaryLoginButton() {
    return AnimatedBuilder(
      animation: _buttonPulseAnimation,
      builder: (context, child) {
        return Transform.scale(
          scale: _isLoading ? 1.0 : _buttonPulseAnimation.value,
          child: SizedBox(
            width: double.infinity,
            height: 56.h,
            child: ElevatedButton(
              onPressed: _isLoading ? null : _handleLogin,
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
                    colors: AppTheme.affirmationGradient,
                  ),
                  borderRadius: BorderRadius.circular(16.r),
                  boxShadow: [
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
                  ],
                ),
                child: Container(
                  alignment: Alignment.center,
                  child: _isLoading
                      ? CompactStylishLoading(size: 20.w, color: AppTheme.white)
                      : Text(
                          'Sign In',
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



  Widget _buildSignUpLink() {
    return Center(
      child: GestureDetector(
        onTap: _navigateToSignUp,
        child: RichText(
          text: TextSpan(
            style: TextStyle(
              fontSize: 16.sp,
              color: AppTheme.textMedium,
            ),
            children: [
              TextSpan(text: "Don't have an account? "),
              TextSpan(
                text: 'Sign Up',
                style: TextStyle(
                  color: AppTheme.affirmationGradient[0],
                  fontWeight: FontWeight.w600,
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }

  void _handleLogin() async {
    // Clear previous validation error
    setState(() {
      _validationError = '';
    });
    
    // Custom validation since we disabled TextFormField's built-in validation
    if (_usernameError != null || _passwordError != null) {
      return;
    }

    setState(() {
      _isLoading = true;
    });

    try {
      final authService = UnifiedAuthService();
      
      final result = await authService.login(
        username: _emailController.text.trim(),
        password: _passwordController.text.trim(),
        rememberMe: _rememberMe,
      );

      if (result.isSuccess) {
        // Show success popup before navigation
        SuccessPopup.show(
          context,
          title: 'Welcome Back!',
          message: 'You\'ve successfully signed in. Ready to continue your manifestation journey?',
          actionText: 'Let\'s Go',
          onAction: () {
            Navigator.push(
              context,
              MaterialPageRoute(builder: (context) => HomeScreen()),
            );
          },
          onDismiss: () {
            Navigator.push(
              context,
              MaterialPageRoute(builder: (context) => HomeScreen()),
            );
          },
        );
      } else {
        // Show beautiful error popup instead of validation error
        ErrorPopup.show(
          context,
          title: 'Login Failed',
          message: _getLoginErrorMessage(result.message),
          actionText: 'Try Again',
          onAction: () {
            // Clear fields and focus on email
            _emailController.clear();
            _passwordController.clear();
            FocusScope.of(context).requestFocus(FocusNode());
          },
        );
      }
    } catch (e) {
      ErrorPopup.show(
        context,
        title: 'Connection Error',
        message: 'Unable to connect to our servers. Please check your internet connection and try again.',
        actionText: 'Retry',
        onAction: () => _handleLogin(),
      );
    } finally {
      setState(() {
        _isLoading = false;
      });
    }
  }



  void _navigateToSignUp() {
    Navigator.push(
      context,
      PageRouteBuilder(
        pageBuilder: (context, animation, secondaryAnimation) => SignUpScreen(),
        transitionsBuilder: (context, animation, secondaryAnimation, child) {
          return FadeTransition(
            opacity: animation.drive(
              Tween(begin: 0.0, end: 1.0)
                  .chain(CurveTween(curve: Curves.easeInOut)),
            ),
            child: ScaleTransition(
              scale: animation.drive(
                Tween(begin: 0.95, end: 1.0)
                    .chain(CurveTween(curve: Curves.easeOutCubic)),
              ),
              child: child,
            ),
          );
        },
        transitionDuration: Duration(milliseconds: 400),
      ),
    );
  }

  void _navigateToForgotPassword() {
    Navigator.push(
      context,
      MaterialPageRoute(builder: (context) => ForgotPasswordScreen()),
    );
  }

  String _getLoginErrorMessage(String apiMessage) {
    final message = apiMessage.toLowerCase();
    
    if (message.contains('user not found') || message.contains('account not found') || message.contains('invalid username')) {
      return 'Account not found. Please check your username or sign up.';
    } else if (message.contains('invalid password') || message.contains('incorrect password') || message.contains('wrong password')) {
      return 'Incorrect password. Please try again.';
    } else if (message.contains('account locked') || message.contains('account disabled')) {
      return 'Account temporarily locked. Please contact support.';
    } else if (message.contains('too many attempts') || message.contains('rate limit')) {
      return 'Too many login attempts. Please wait and try again.';
    } else if (message.contains('network') || message.contains('connection') || message.contains('timeout')) {
      return 'Connection error. Please check your internet and try again.';
    } else if (message.contains('server') || message.contains('internal error')) {
      return 'Server error. Please try again in a moment.';
    } else {
      // Fallback for any other error messages
      return 'Login failed. Please check your credentials and try again.';
    }
  }

}