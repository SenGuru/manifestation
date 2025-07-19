import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:manifestation_app/models/auth_models.dart';
import 'package:manifestation_app/screens/home_screen.dart';
import 'package:manifestation_app/screens/login_screen.dart';
import 'package:manifestation_app/constants/colors.dart';

class PhoneAuthScreen extends StatefulWidget {
  @override
  _PhoneAuthScreenState createState() => _PhoneAuthScreenState();
}

class _PhoneAuthScreenState extends State<PhoneAuthScreen>
    with TickerProviderStateMixin {
  final _phoneController = TextEditingController();
  final _otpController = TextEditingController();
  final _formKey = GlobalKey<FormState>();
  
  bool _isLoading = false;
  bool _otpSent = false;
  bool _resendingOtp = false;
  int _resendCountdown = 0;
  String _verificationId = '';
  String _selectedCountryCode = '+1';
  
  late AnimationController _fadeController;
  late AnimationController _slideController;
  late Animation<double> _fadeAnimation;
  late Animation<Offset> _slideAnimation;

  // Country codes data - using the CountryCode model
  final List<CountryCode> _countryCodes = CountryCode.getPopularCountries();

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
    _phoneController.dispose();
    _otpController.dispose();
    _fadeController.dispose();
    _slideController.dispose();
    super.dispose();
  }

  void _startResendCountdown() {
    setState(() {
      _resendCountdown = 60;
    });
    
    Future.doWhile(() async {
      await Future.delayed(Duration(seconds: 1));
      if (mounted) {
        setState(() {
          _resendCountdown--;
        });
        return _resendCountdown > 0;
      }
      return false;
    });
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
                    
                    SizedBox(height: 30.h),
                    
                    // Header
                    _buildHeader(),
                    
                    SizedBox(height: 50.h),
                    
                    // Phone/OTP form
                    _buildAuthForm(),
                    
                    SizedBox(height: 30.h),
                    
                    // Alternative auth options
                    _buildAlternativeOptions(),
                    
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
        // Logo
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
            Icons.phone,
            color: AppColors.white,
            size: 40.sp,
          ),
        ),
        
        SizedBox(height: 32.h),
        
        Text(
          _otpSent ? 'Verify Your Number' : 'Enter Your Phone',
          style: TextStyle(
            fontSize: 32.sp,
            fontWeight: FontWeight.bold,
            color: AppColors.textLight,
          ),
        ),
        
        SizedBox(height: 8.h),
        
        Text(
          _otpSent 
              ? 'We sent a verification code to\n${_selectedCountryCode} ${_phoneController.text}'
              : 'We\'ll send you a verification code',
          style: TextStyle(
            fontSize: 16.sp,
            color: AppColors.textMedium,
            height: 1.3,
          ),
        ),
      ],
    );
  }

  Widget _buildAuthForm() {
    return Form(
      key: _formKey,
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          if (!_otpSent) ...[
            // Phone number input
            Text(
              'Phone Number',
              style: TextStyle(
                fontSize: 16.sp,
                fontWeight: FontWeight.w600,
                color: AppColors.textLight,
              ),
            ),
            SizedBox(height: 8.h),
            _buildPhoneInputField(),
            SizedBox(height: 32.h),
            _buildSendCodeButton(),
          ] else ...[
            // OTP input
            Text(
              'Verification Code',
              style: TextStyle(
                fontSize: 16.sp,
                fontWeight: FontWeight.w600,
                color: AppColors.textLight,
              ),
            ),
            SizedBox(height: 8.h),
            _buildOtpInputField(),
            SizedBox(height: 24.h),
            _buildResendSection(),
            SizedBox(height: 32.h),
            _buildVerifyButton(),
          ],
        ],
      ),
    );
  }

  Widget _buildPhoneInputField() {
    return Row(
      children: [
        // Country code selector
        GestureDetector(
          onTap: _showCountryCodePicker,
          child: Container(
            height: 56.h,
            padding: EdgeInsets.symmetric(horizontal: 12.w),
            decoration: BoxDecoration(
              color: AppColors.cardBackground,
              borderRadius: BorderRadius.circular(16.r),
              border: Border.all(
                color: AppColors.textMedium.withOpacity(0.1),
              ),
            ),
            child: Row(
              mainAxisSize: MainAxisSize.min,
              children: [
                Text(
                  _countryCodes.firstWhere(
                    (country) => country.code == _selectedCountryCode,
                    orElse: () => _countryCodes[0],
                  ).flag,
                  style: TextStyle(fontSize: 20.sp),
                ),
                SizedBox(width: 4.w),
                Text(
                  _selectedCountryCode,
                  style: TextStyle(
                    color: AppColors.textLight,
                    fontSize: 16.sp,
                    fontWeight: FontWeight.w500,
                  ),
                ),
                SizedBox(width: 4.w),
                Icon(
                  Icons.arrow_drop_down,
                  color: AppColors.textMedium,
                  size: 20.sp,
                ),
              ],
            ),
          ),
        ),
        
        SizedBox(width: 12.w),
        
        // Phone number field
        Expanded(
          child: TextFormField(
            controller: _phoneController,
            keyboardType: TextInputType.phone,
            inputFormatters: [
              FilteringTextInputFormatter.digitsOnly,
              LengthLimitingTextInputFormatter(15),
            ],
            style: TextStyle(
              color: AppColors.textLight,
              fontSize: 16.sp,
            ),
            validator: AuthValidation.validatePhoneNumber,
            decoration: InputDecoration(
              hintText: 'Phone number',
              hintStyle: TextStyle(
                color: AppColors.textMedium.withOpacity(0.6),
              ),
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
        ),
      ],
    );
  }

  Widget _buildOtpInputField() {
    return TextFormField(
      controller: _otpController,
      keyboardType: TextInputType.number,
      textAlign: TextAlign.center,
      inputFormatters: [
        FilteringTextInputFormatter.digitsOnly,
        LengthLimitingTextInputFormatter(6),
      ],
      style: TextStyle(
        color: AppColors.textLight,
        fontSize: 24.sp,
        fontWeight: FontWeight.w600,
        letterSpacing: 8.w,
      ),
      validator: (value) {
        if (value == null || value.isEmpty) {
          return 'Verification code is required';
        }
        if (value.length != 6) {
          return 'Enter the 6-digit code';
        }
        return null;
      },
      decoration: InputDecoration(
        hintText: '• • • • • •',
        hintStyle: TextStyle(
          color: AppColors.textMedium.withOpacity(0.6),
          fontSize: 24.sp,
          letterSpacing: 8.w,
        ),
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
          vertical: 20.h,
        ),
      ),
    );
  }

  Widget _buildSendCodeButton() {
    return SizedBox(
      width: double.infinity,
      height: 56.h,
      child: ElevatedButton(
        onPressed: _isLoading ? null : _sendVerificationCode,
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
            gradient: LinearGradient(
              colors: AppColors.affirmationGradient,
            ),
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
                    'Send Code',
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

  Widget _buildResendSection() {
    return Center(
      child: Column(
        children: [
          Text(
            'Didn\'t receive the code?',
            style: TextStyle(
              fontSize: 14.sp,
              color: AppColors.textMedium,
            ),
          ),
          SizedBox(height: 8.h),
          if (_resendCountdown > 0)
            Text(
              'Resend in ${_resendCountdown}s',
              style: TextStyle(
                fontSize: 14.sp,
                color: AppColors.textMedium,
                fontWeight: FontWeight.w500,
              ),
            )
          else
            GestureDetector(
              onTap: _resendingOtp ? null : _resendVerificationCode,
              child: Text(
                _resendingOtp ? 'Sending...' : 'Resend Code',
                style: TextStyle(
                  fontSize: 14.sp,
                  color: AppColors.affirmationGradient[0],
                  fontWeight: FontWeight.w600,
                ),
              ),
            ),
        ],
      ),
    );
  }

  Widget _buildVerifyButton() {
    return SizedBox(
      width: double.infinity,
      height: 56.h,
      child: ElevatedButton(
        onPressed: _isLoading ? null : _verifyCode,
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
            gradient: LinearGradient(
              colors: AppColors.affirmationGradient,
            ),
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
                    'Verify & Continue',
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

  Widget _buildAlternativeOptions() {
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
        
        // Email login button
        GestureDetector(
          onTap: _navigateToEmailLogin,
          child: Container(
            width: double.infinity,
            height: 56.h,
            decoration: BoxDecoration(
              color: AppColors.cardBackground,
              borderRadius: BorderRadius.circular(16.r),
              border: Border.all(
                color: AppColors.textMedium.withOpacity(0.2),
              ),
            ),
            child: Row(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                Icon(
                  Icons.email_outlined,
                  color: AppColors.textLight,
                  size: 24.sp,
                ),
                SizedBox(width: 12.w),
                Text(
                  'Continue with Email',
                  style: TextStyle(
                    fontSize: 16.sp,
                    fontWeight: FontWeight.w600,
                    color: AppColors.textLight,
                  ),
                ),
              ],
            ),
          ),
        ),
      ],
    );
  }

  void _showCountryCodePicker() {
    showModalBottomSheet(
      context: context,
      backgroundColor: AppColors.backgroundDark,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.vertical(
          top: Radius.circular(24.r),
        ),
      ),
      builder: (context) => Container(
        height: 400.h,
        child: Column(
          children: [
            SizedBox(height: 12.h),
            Container(
              width: 40.w,
              height: 4.h,
              decoration: BoxDecoration(
                color: AppColors.textMedium.withOpacity(0.3),
                borderRadius: BorderRadius.circular(2.r),
              ),
            ),
            SizedBox(height: 20.h),
            Text(
              'Select Country',
              style: TextStyle(
                fontSize: 18.sp,
                fontWeight: FontWeight.bold,
                color: AppColors.textLight,
              ),
            ),
            SizedBox(height: 16.h),
            Expanded(
              child: ListView.builder(
                itemCount: _countryCodes.length,
                itemBuilder: (context, index) {
                  final country = _countryCodes[index];
                  return ListTile(
                    leading: Text(
                      country.flag,
                      style: TextStyle(fontSize: 24.sp),
                    ),
                    title: Text(
                      '${country.name} (${country.code})',
                      style: TextStyle(
                        color: AppColors.textLight,
                        fontSize: 16.sp,
                      ),
                    ),
                    onTap: () {
                      setState(() {
                        _selectedCountryCode = country.code;
                      });
                      Navigator.pop(context);
                    },
                  );
                },
              ),
            ),
          ],
        ),
      ),
    );
  }

  void _sendVerificationCode() async {
    if (!_formKey.currentState!.validate()) return;

    setState(() {
      _isLoading = true;
    });

    try {
      // Create phone auth request using your models
      final phoneAuthRequest = PhoneAuthRequest(
        phoneNumber: _phoneController.text.trim(),
        countryCode: _selectedCountryCode,
      );
      
      print('Sending OTP to: ${phoneAuthRequest.fullPhoneNumber}');
      
      await Future.delayed(Duration(seconds: 2)); // Simulate API call
      
      setState(() {
        _otpSent = true;
        _verificationId = 'mock_verification_id';
      });
      
      _startResendCountdown();
      _showSuccessSnackBar('Verification code sent!');
      
    } catch (e) {
      _showErrorSnackBar('Failed to send verification code. Please try again.');
    } finally {
      setState(() {
        _isLoading = false;
      });
    }
  }

  void _resendVerificationCode() async {
    setState(() {
      _resendingOtp = true;
    });

    try {
      // TODO: Implement resend logic
      await Future.delayed(Duration(seconds: 1));
      
      _startResendCountdown();
      _showSuccessSnackBar('Verification code resent!');
      
    } catch (e) {
      _showErrorSnackBar('Failed to resend code. Please try again.');
    } finally {
      setState(() {
        _resendingOtp = false;
      });
    }
  }

  void _verifyCode() async {
    if (!_formKey.currentState!.validate()) return;

    setState(() {
      _isLoading = true;
    });

    try {
      // Create OTP verification request using your models
      final otpRequest = OtpVerificationRequest(
        phoneNumber: _phoneController.text.trim(),
        countryCode: _selectedCountryCode,
        otp: _otpController.text,
        verificationId: _verificationId,
      );
      
      print('Verifying OTP: ${otpRequest.otp} with verification ID: ${otpRequest.verificationId}');
      
      await Future.delayed(Duration(seconds: 2)); // Simulate API call
      
      // Navigate to home screen on success
      Navigator.pushReplacement(
        context,
        MaterialPageRoute(builder: (context) => HomeScreen()),
      );
      
    } catch (e) {
      _showErrorSnackBar('Invalid verification code. Please try again.');
    } finally {
      setState(() {
        _isLoading = false;
      });
    }
  }

  void _navigateToEmailLogin() {
    Navigator.pushReplacement(
      context,
      PageRouteBuilder(
        pageBuilder: (context, animation, secondaryAnimation) => LoginScreen(),
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
}