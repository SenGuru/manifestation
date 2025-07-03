import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:manifestation_app/auth_models.dart';
import 'package:manifestation_app/home_screen.dart';
import '../colors.dart';

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
  
  PasswordStrength _passwordStrength = PasswordStrength(
    strength: 0.0,
    label: 'Enter password',
    color: Colors.grey,
  );
  
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
    
    // Password strength listener
    _passwordController.addListener(() {
      setState(() {
        _passwordStrength = PasswordStrength.calculate(_passwordController.text);
      });
    });
    
    // Start animations
    _fadeController.forward();
    _slideController.forward();
  }

  @override
  void dispose() {
    _nameController.dispose();
    _emailController.dispose();
    _passwordController.dispose();
    _confirmPasswordController.dispose();
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
                    SizedBox(height: 20.h),
                    
                    // Back button
                    _buildBackButton(),
                    
                    SizedBox(height: 20.h),
                    
                    // Header
                    _buildHeader(),
                    
                    SizedBox(height: 40.h),
                    
                    // Sign up form
                    _buildSignUpForm(),
                    
                    SizedBox(height: 24.h),
                    
                    // Social sign up options
                    _buildSocialSignUp(),
                    
                    SizedBox(height: 24.h),
                    
                    // Login link
                    _buildLoginLink(),
                    
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
          
          SizedBox(height: 20.h),
          
          // Email field
          _buildTextField(
            controller: _emailController,
            label: 'Email',
            hint: 'Enter your email',
            icon: Icons.email_outlined,
            keyboardType: TextInputType.emailAddress,
            validator: AuthValidation.validateEmail,
          ),
          
          SizedBox(height: 20.h),
          
          // Password field
          _buildTextField(
            controller: _passwordController,
            label: 'Password',
            hint: 'Create a strong password',
            icon: Icons.lock_outline,
            isPassword: true,
            validator: AuthValidation.validatePassword,
          ),
          
          // Password strength indicator
          if (_passwordController.text.isNotEmpty) ...[
            SizedBox(height: 12.h),
            _buildPasswordStrengthIndicator(),
          ],
          
          SizedBox(height: 20.h),
          
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
          
          SizedBox(height: 24.h),
          
          // Terms and conditions
          _buildTermsCheckbox(),
          
          SizedBox(height: 16.h),
          
          // Newsletter subscription
          _buildNewsletterCheckbox(),
          
          SizedBox(height: 32.h),
          
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
                width: 1,
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

  Widget _buildPasswordStrengthIndicator() {
    return Column(
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
        LinearProgressIndicator(
          value: _passwordStrength.strength,
          backgroundColor: AppColors.textMedium.withOpacity(0.2),
          valueColor: AlwaysStoppedAnimation<Color>(_passwordStrength.color),
          minHeight: 4.h,
        ),
      ],
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

  Widget _buildSignUpButton() {
    return SizedBox(
      width: double.infinity,
      height: 56.h,
      child: ElevatedButton(
        onPressed: (_isLoading || !_agreeToTerms) ? null : _handleSignUp,
        style: ElevatedButton.styleFrom(
          backgroundColor: Colors.transparent,
          shadowColor: Colors.transparent,
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(16.r),
          ),
        ).copyWith(
          backgroundColor: MaterialStateProperty.all(Colors.transparent),
        ),
        child: Ink(
          decoration: BoxDecoration(
            gradient: (_agreeToTerms && !_isLoading)
                ? LinearGradient(colors: AppColors.socialGradient)
                : null,
            color: (_agreeToTerms && !_isLoading)
                ? null
                : AppColors.textMedium.withOpacity(0.3),
            borderRadius: BorderRadius.circular(16.r),
          ),
          child: Container(
            alignment: Alignment.center,
            child: _isLoading
                ? SizedBox(
                    width: 24.w,
                    height: 24.h,
                    child: CircularProgressIndicator(
                      strokeWidth: 2,
                      valueColor: AlwaysStoppedAnimation<Color>(AppColors.white),
                    ),
                  )
                : Text(
                    'Create Account',
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

  Widget _buildSocialSignUp() {
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
                'Or sign up with',
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
                () => _handleSocialSignUp(AuthProvider.google),
              ),
            ),
            SizedBox(width: 16.w),
            Expanded(
              child: _buildSocialButton(
                'Apple',
                Icons.apple,
                Colors.black,
                Colors.white,
                () => _handleSocialSignUp(AuthProvider.apple),
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
    if (!_formKey.currentState!.validate()) return;
    if (!_agreeToTerms) {
      _showErrorSnackBar('Please agree to the Terms of Service and Privacy Policy');
      return;
    }

    setState(() {
      _isLoading = true;
    });

    try {
      // TODO: Implement actual sign up logic
      final signUpRequest = SignUpRequest(
        email: _emailController.text.trim(),
        password: _passwordController.text,
        displayName: _nameController.text.trim(),
        agreeToTerms: _agreeToTerms,
        subscribeToNewsletter: _subscribeToNewsletter,
      );
      
      await Future.delayed(Duration(seconds: 2)); // Simulate API call
      
      // Navigate to home screen on success
      Navigator.pushReplacement(
        context,
        MaterialPageRoute(builder: (context) => HomeScreen()),
      );
    } catch (e) {
      _showErrorSnackBar('Sign up failed. Please try again.');
    } finally {
      setState(() {
        _isLoading = false;
      });
    }
  }

  void _handleSocialSignUp(AuthProvider provider) async {
    setState(() {
      _isLoading = true;
    });

    try {
      // TODO: Implement social sign up logic
      await Future.delayed(Duration(seconds: 2)); // Simulate API call
      
      print('Signing up with ${provider.displayName}');
      
      // Navigate to home screen on success
      Navigator.pushReplacement(
        context,
        MaterialPageRoute(builder: (context) => HomeScreen()),
      );
    } catch (e) {
      _showErrorSnackBar('${provider.displayName} sign-up failed. Please try again.');
    } finally {
      setState(() {
        _isLoading = false;
      });
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