import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../models/auth_models.dart';
import '../services/unified_auth_service.dart';
import 'signup_page.dart';
import 'home_screen.dart';
import 'forgot_password_screen.dart';
import '../constants/colors.dart';
import '../widgets/stylish_loading.dart';

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
  
  late AnimationController _fadeController;
  late AnimationController _slideController;
  late Animation<double> _fadeAnimation;
  late Animation<Offset> _slideAnimation;

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
    
    // Start animations
    _fadeController.forward();
    _slideController.forward();
  }


  @override
  void dispose() {
    _emailController.dispose();
    _passwordController.dispose();
    _fadeController.dispose();
    _slideController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.backgroundDark,
      body: SafeArea(
        child: FadeTransition(
          opacity: _fadeAnimation,
          child: SlideTransition(
            position: _slideAnimation,
            child: SingleChildScrollView(
              physics: BouncingScrollPhysics(),
              child: Padding(
                padding: EdgeInsets.symmetric(horizontal: 24.w),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    SizedBox(height: 40.h),
                    
                    // Header
                    _buildHeader(),
                    
                    SizedBox(height: 40.h),
                    
                    
                    // Login form
                    _buildLoginForm(),
                    
                    SizedBox(height: 30.h),
                    
                    // Social login options
                    _buildSocialLogin(),
                    
                    SizedBox(height: 30.h),
                    
                    // Sign up link
                    _buildSignUpLink(),
                    
                    SizedBox(height: 40.h),
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
              colors: AppColors.affirmationGradient,
              begin: Alignment.topLeft,
              end: Alignment.bottomRight,
            ),
            shape: BoxShape.circle,
            boxShadow: [
              BoxShadow(
                color: AppColors.affirmationGradient[0].withOpacity(0.3),
                blurRadius: 20.r,
                offset: Offset(0, 10.h),
              ),
            ],
          ),
          child: Icon(
            Icons.auto_awesome,
            color: AppColors.white,
            size: 40.sp,
          ),
        ),
        
        SizedBox(height: 32.h),
        
        Text(
          'Welcome Back',
          style: TextStyle(
            fontSize: 32.sp,
            fontWeight: FontWeight.bold,
            color: AppColors.textLight,
          ),
        ),
        
        SizedBox(height: 8.h),
        
        Text(
          'Sign in to continue your manifestation journey',
          style: TextStyle(
            fontSize: 16.sp,
            color: AppColors.textMedium,
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
                            ? AppColors.affirmationGradient[0]
                            : Colors.transparent,
                        borderRadius: BorderRadius.circular(4.r),
                        border: Border.all(
                          color: _rememberMe 
                              ? AppColors.affirmationGradient[0]
                              : AppColors.textMedium.withOpacity(0.5),
                          width: 2,
                        ),
                      ),
                      child: _rememberMe
                          ? Icon(
                              Icons.check,
                              color: AppColors.white,
                              size: 14.sp,
                            )
                          : null,
                    ),
                    SizedBox(width: 8.w),
                    Text(
                      'Remember me',
                      style: TextStyle(
                        fontSize: 14.sp,
                        color: AppColors.textMedium,
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
                    color: AppColors.affirmationGradient[0],
                    fontWeight: FontWeight.w600,
                  ),
                ),
              ),
            ],
          ),
          
          SizedBox(height: 32.h),
          
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
            color: AppColors.textLight,
          ),
        ),
        SizedBox(height: 8.h),
        TextFormField(
          controller: controller,
          validator: validator,
          obscureText: isPassword && !_isPasswordVisible,
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
                        _isPasswordVisible = !_isPasswordVisible;
                      });
                    },
                    child: Icon(
                      _isPasswordVisible
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
                width: 1,
              ),
            ),
            focusedBorder: OutlineInputBorder(
              borderRadius: BorderRadius.circular(16.r),
              borderSide: BorderSide(
                color: AppColors.affirmationGradient[0],
                width: 2,
              ),
            ),
            errorBorder: OutlineInputBorder(
              borderRadius: BorderRadius.circular(16.r),
              borderSide: BorderSide(
                color: Colors.red,
                width: 1,
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

  Widget _buildPrimaryLoginButton() {
    return SizedBox(
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
        ).copyWith(
          backgroundColor: MaterialStateProperty.all(Colors.transparent),
        ),
        child: Ink(
          decoration: BoxDecoration(
            gradient: LinearGradient(
              colors: AppColors.affirmationGradient,
            ),
            borderRadius: BorderRadius.circular(16.r),
            boxShadow: [
              BoxShadow(
                color: AppColors.affirmationGradient[0].withOpacity(0.3),
                blurRadius: 20.r,
                offset: Offset(0, 10.h),
              ),
            ],
          ),
          child: Container(
            alignment: Alignment.center,
            child: _isLoading
                ? CompactStylishLoading(size: 20.w)
                : Text(
                    'Sign In',
                    style: TextStyle(
                      fontSize: 18.sp,
                      fontWeight: FontWeight.w600,
                      color: AppColors.white,
                    ),
                  ),
          ),
        ),
      ),
    );
  }

  Widget _buildSocialLogin() {
    return Column(
      children: [
        // Divider
        Row(
          children: [
            Expanded(
              child: Container(
                height: 1,
                color: AppColors.textMedium.withOpacity(0.3),
              ),
            ),
            Padding(
              padding: EdgeInsets.symmetric(horizontal: 16.w),
              child: Text(
                'Or continue with',
                style: TextStyle(
                  fontSize: 14.sp,
                  color: AppColors.textMedium,
                ),
              ),
            ),
            Expanded(
              child: Container(
                height: 1,
                color: AppColors.textMedium.withOpacity(0.3),
              ),
            ),
          ],
        ),
        
        SizedBox(height: 24.h),
        
        // Social buttons
        Row(
          children: [
            Expanded(
              child: _buildSocialButton(
                'Google',
                Icons.g_mobiledata,
                Colors.white,
                Colors.black87,
                () => _handleSocialLogin(AuthProvider.google),
              ),
            ),
            SizedBox(width: 16.w),
            Expanded(
              child: _buildSocialButton(
                'Apple',
                Icons.apple,
                Colors.black,
                Colors.white,
                () => _handleSocialLogin(AuthProvider.apple),
              ),
            ),
          ],
        ),
      ],
    );
  }

  Widget _buildSocialButton(
    String label,
    IconData icon,
    Color backgroundColor,
    Color textColor,
    VoidCallback onTap,
  ) {
    return GestureDetector(
      onTap: onTap,
      child: Container(
        height: 56.h,
        decoration: BoxDecoration(
          color: backgroundColor,
          borderRadius: BorderRadius.circular(16.r),
          border: Border.all(
            color: AppColors.textMedium.withOpacity(0.2),
          ),
          boxShadow: [
            BoxShadow(
              color: AppColors.black.withOpacity(0.05),
              blurRadius: 8.r,
              offset: Offset(0, 4.h),
            ),
          ],
        ),
        child: Row(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Icon(
              icon,
              color: textColor,
              size: 24.sp,
            ),
            SizedBox(width: 12.w),
            Text(
              label,
              style: TextStyle(
                fontSize: 16.sp,
                fontWeight: FontWeight.w600,
                color: textColor,
              ),
            ),
          ],
        ),
      ),
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
              color: AppColors.textMedium,
            ),
            children: [
              TextSpan(text: "Don't have an account? "),
              TextSpan(
                text: 'Sign Up',
                style: TextStyle(
                  color: AppColors.affirmationGradient[0],
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
    if (!_formKey.currentState!.validate()) return;

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

        // Navigate to home screen on success
        Navigator.pushReplacement(
          context,
          MaterialPageRoute(builder: (context) => HomeScreen()),
        );
      } else {
        _showErrorSnackBar(result.message);
      }
    } catch (e) {
      _showErrorSnackBar('Unexpected error occurred. Please try again.');
    } finally {
      setState(() {
        _isLoading = false;
      });
    }
  }

  void _handleSocialLogin(AuthProvider provider) async {
    setState(() {
      _isLoading = true;
    });

    try {
      // TODO: Implement social login logic
      await Future.delayed(Duration(seconds: 2)); // Simulate API call
      
      print('Signing in with ${provider.displayName}');
      
      // Navigate to home screen on success
      Navigator.pushReplacement(
        context,
        MaterialPageRoute(builder: (context) => HomeScreen()),
      );
    } catch (e) {
      _showErrorSnackBar('${provider.displayName} sign-in failed. Please try again.');
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
          return SlideTransition(
            position: animation.drive(
              Tween(begin: Offset(1.0, 0.0), end: Offset.zero)
                  .chain(CurveTween(curve: Curves.easeInOut)),
            ),
            child: child,
          );
        },
        transitionDuration: Duration(milliseconds: 300),
      ),
    );
  }

  void _navigateToForgotPassword() {
    Navigator.push(
      context,
      MaterialPageRoute(builder: (context) => ForgotPasswordScreen()),
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
}