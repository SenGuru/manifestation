import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'dart:convert';
import 'dart:async';
import 'package:shared_preferences/shared_preferences.dart';
import '../../constants/app_theme.dart';
import '../../widgets/stylish_loading.dart';
import '../../widgets/universal_back_button.dart';
import '../../api/generated/code/manifestation.swagger.dart';
import 'package:chopper/chopper.dart';
import '../../services/unified_auth_service.dart';

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
  
  // API client
  late Manifestation _apiClient;

  @override
  void initState() {
    super.initState();
    
    // Initialize API client with auth interceptor
    _initializeApiClient();
    
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

  void _initializeApiClient() {
    _apiClient = Manifestation.create(
      baseUrl: Uri.parse('https://manifestation-qa-1.senthilguru.net'),
      interceptors: [_AuthInterceptor()],
    );
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
    print('🔐 === PASSWORD CHANGE INITIATED ===');
    
    if (_currentPasswordError != null || _newPasswordError != null || _confirmPasswordError != null) {
      print('❌ Validation errors present:');
      print('   Current password error: $_currentPasswordError');
      print('   New password error: $_newPasswordError');
      print('   Confirm password error: $_confirmPasswordError');
      return;
    }

    if (_newPasswordController.text.isEmpty) {
      print('❌ New password is empty');
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Please enter a new password'),
          backgroundColor: Colors.red,
        ),
      );
      return;
    }

    if (_currentPasswordController.text.isEmpty) {
      print('❌ Current password is empty');
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Please enter your current password'),
          backgroundColor: Colors.red,
        ),
      );
      return;
    }

    print('✅ Validation passed');
    print('📝 Current password length: ${_currentPasswordController.text.length}');
    print('📝 New password length: ${_newPasswordController.text.length}');
    print('📝 Passwords match: ${_newPasswordController.text == _confirmPasswordController.text}');

    setState(() {
      _isChangingPassword = true;
    });

    try {
      // Create the password change request model
      final changePasswordModel = ChangePasswordModel(
        currentPassword: _currentPasswordController.text,
        password: _newPasswordController.text,
        retypedPassword: _confirmPasswordController.text,
      );
      
      print('📤 === SENDING PASSWORD CHANGE REQUEST ===');
      print('   Endpoint: POST /api/v1/account/changecurrentpassword');
      print('   Base URL: https://manifestation-qa-1.senthilguru.net');
      print('   Full URL: https://manifestation-qa-1.senthilguru.net/api/v1/account/changecurrentpassword');
      
      // Show request payload (with passwords partially hidden for security)
      print('   📋 Request Body (JSON):');
      print('   {');
      print('     "currentPassword": "${_currentPasswordController.text.isNotEmpty ? _currentPasswordController.text.substring(0, 1) + '*' * (_currentPasswordController.text.length - 1) : "[empty]"}",');
      print('     "password": "${_newPasswordController.text.isNotEmpty ? _newPasswordController.text.substring(0, 1) + '*' * (_newPasswordController.text.length - 1) : "[empty]"}",');
      print('     "retypedPassword": "${_confirmPasswordController.text.isNotEmpty ? _confirmPasswordController.text.substring(0, 1) + '*' * (_confirmPasswordController.text.length - 1) : "[empty]"}"');
      print('   }');
      
      // Show actual JSON that will be sent (for debugging - remove in production)
      final requestJson = changePasswordModel.toJson();
      print('   🔍 Actual Request JSON structure:');
      print('   ${requestJson.keys.toList()}');
      
      // Call the API to change password
      print('   ⏳ Sending request...');
      final response = await _apiClient.apiV1AccountChangecurrentpasswordPost(
        body: changePasswordModel,
      );
      
      print('📥 === API RESPONSE RECEIVED ===');
      print('   Status Code: ${response.statusCode}');
      print('   HTTP Is Successful: ${response.isSuccessful}');
      print('   Has Body: ${response.body != null}');
      
      // Print full response body structure
      if (response.body != null) {
        print('   📋 Response Body Structure:');
        print('   {');
        print('     "isSuccess": ${response.body?.isSuccess},');
        print('     "successMessage": ${response.body?.successMessage},');
        print('     "activityId": ${response.body?.activityId},');
        print('     "server": ${response.body?.server},');
        print('     "processId": ${response.body?.processId},');
        
        if (response.body?.model != null) {
          print('     "model": {');
          print('       "userId": ${response.body?.model?.userId},');
          print('       "userName": ${response.body?.model?.userName},');
          print('       "firstName": ${response.body?.model?.firstName},');
          print('       "lastName": ${response.body?.model?.lastName},');
          print('       "token": ${response.body?.model?.token != null ? "[TOKEN PRESENT - ${response.body!.model!.token!.length} chars]" : "null"},');
          print('     },');
        } else {
          print('     "model": null,');
        }
        
        if (response.body?.errorHolder != null) {
          print('     "errorHolder": {');
          print('       "friendlyMessage": "${response.body?.errorHolder?.friendlyMessage}",');
          print('       "internalErrorMessage": "${response.body?.errorHolder?.internalErrorMessage}",');
          if (response.body?.errorHolder?.errorItemList != null && response.body!.errorHolder!.errorItemList!.isNotEmpty) {
            print('       "errorItemList": [');
            for (var error in response.body!.errorHolder!.errorItemList!) {
              print('         { "key": "${error.key}", "value": "${error.$value}" },');
            }
            print('       ]');
          }
          print('     }');
        } else {
          print('     "errorHolder": null');
        }
        print('   }');
      } else if (response.error != null) {
        print('   ❌ Response Error: ${response.error}');
      }
      
      // Check both HTTP success AND the actual API response success flag
      if (response.isSuccessful && response.body != null && response.body?.isSuccess == true) {
        // Password changed successfully
        print('✅ === PASSWORD CHANGE SUCCESSFUL ===');
        
        // Clear password fields
        _currentPasswordController.clear();
        _newPasswordController.clear();
        _confirmPasswordController.clear();
        
        if (mounted) {
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(
              content: Text('Password changed successfully'),
              backgroundColor: Colors.green,
            ),
          );
        }
        
        // Update the stored auth token if returned in the response
        if (response.body?.model?.token != null) {
          print('🔑 New auth token received, updating stored token...');
          // Store the new token after password change
          final prefs = await SharedPreferences.getInstance();
          await prefs.setString('auth_token', response.body!.model!.token!);
          print('✅ Auth token updated successfully');
        } else {
          print('ℹ️ No new auth token in response');
        }
      } else {
        // Handle API error - either HTTP error or business logic error
        print('❌ === PASSWORD CHANGE FAILED ===');
        print('   Status Code: ${response.statusCode}');
        print('   HTTP Successful: ${response.isSuccessful}');
        print('   API isSuccess: ${response.body?.isSuccess}');
        print('   Error Body: ${response.error}');
        print('   Response Body: ${response.body}');
        
        String errorMessage = 'Failed to change password';
        
        // First check if we have an error in the response body (business logic error)
        if (response.body?.errorHolder != null) {
          // Use friendlyMessage or internalErrorMessage from ReturnError
          errorMessage = response.body?.errorHolder?.friendlyMessage ?? 
                       response.body?.errorHolder?.internalErrorMessage ?? 
                       errorMessage;
          print('   Error from errorHolder: $errorMessage');
          
          // Check for specific error messages
          if (errorMessage.toLowerCase().contains('password') && 
              (errorMessage.toLowerCase().contains('incorrect') || 
               errorMessage.toLowerCase().contains('wrong') || 
               errorMessage.toLowerCase().contains('invalid'))) {
            errorMessage = 'Current password is incorrect';
          }
        }
        // Otherwise try to extract error message from response error (HTTP error)
        else if (response.error != null) {
          try {
            final errorBody = response.error as Map<String, dynamic>;
            errorMessage = errorBody['message'] ?? errorMessage;
            print('   Extracted error message: $errorMessage');
          } catch (e) {
            print('   Could not extract error message: $e');
          }
        }
        
        if (mounted) {
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(
              content: Text(errorMessage),
              backgroundColor: Colors.red,
            ),
          );
        }
      }
    } catch (e) {
      print('❌ === EXCEPTION IN PASSWORD CHANGE ===');
      print('   Exception type: ${e.runtimeType}');
      print('   Exception details: $e');
      
      if (e is Response) {
        print('   Response status: ${e.statusCode}');
        print('   Response body: ${e.body}');
        print('   Response error: ${e.error}');
      }
      
      String errorMessage = 'Failed to change password';
      
      // Handle specific error types
      if (e.toString().contains('401') || e.toString().contains('Unauthorized')) {
        errorMessage = 'Current password is incorrect';
      } else if (e.toString().contains('400') || e.toString().contains('Bad Request')) {
        errorMessage = 'Invalid password format or requirements not met';
      } else if (e.toString().contains('SocketException') || e.toString().contains('Failed host lookup')) {
        errorMessage = 'No internet connection. Please check your network.';
      }
      
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text(errorMessage),
            backgroundColor: Colors.red,
          ),
        );
      }
    } finally {
      print('🏁 === PASSWORD CHANGE PROCESS COMPLETED ===');
      print('   Clearing loading state...');
      
      if (mounted) {
        setState(() {
          _isChangingPassword = false;
        });
      }
      
      print('✅ Ready for next operation');
    }
  }
}

// Auth interceptor to add authentication headers
class _AuthInterceptor implements Interceptor {
  @override
  FutureOr<Response<BodyType>> intercept<BodyType>(Chain<BodyType> chain) async {
    final request = chain.request;
    
    print('🔒 === AUTH INTERCEPTOR ===');
    print('   Request URL: ${request.url}');
    print('   Request Method: ${request.method}');
    print('   Request Headers (before auth): ${request.headers}');
    
    try {
      // Get auth token from the unified service
      final authService = UnifiedAuthService();
      final authToken = authService.authToken;
      
      if (authToken != null && authToken.isNotEmpty) {
        print('   ✅ Using Bearer token authentication');
        print('   Token length: ${authToken.length}');
        print('   Token preview: ${authToken.substring(0, 10)}...${authToken.substring(authToken.length - 10)}');
        
        final updatedRequest = request.copyWith(
          headers: {
            ...request.headers,
            'Authorization': 'Bearer $authToken',
          },
        );
        
        print('   📤 Final Request Headers:');
        updatedRequest.headers.forEach((key, value) {
          if (key.toLowerCase() == 'authorization') {
            print('     $key: Bearer [${authToken.length} chars]');
          } else {
            print('     $key: $value');
          }
        });
        
        print('   ⏳ Sending HTTP request...');
        final response = await chain.proceed(updatedRequest);
        
        print('   📥 HTTP Response:');
        print('     Status: ${response.statusCode}');
        print('     Headers: ${response.base.headers}');
        
        return response;
      } else {
        print('   ⚠️ No auth token available, using basic auth');
        print('   Basic auth credentials: SpecialUser:SpecialPassword1');
        
        // Add basic auth if no token
        final basicAuth = 'Basic ${base64Encode(utf8.encode('SpecialUser:SpecialPassword1'))}';
        final updatedRequest = request.copyWith(
          headers: {
            ...request.headers,
            'Authorization': basicAuth,
          },
        );
        
        print('   📤 Final Request Headers:');
        updatedRequest.headers.forEach((key, value) {
          if (key.toLowerCase() == 'authorization') {
            print('     $key: Basic [encoded SpecialUser:SpecialPassword1]');
          } else {
            print('     $key: $value');
          }
        });
        
        print('   ⏳ Sending HTTP request...');
        final response = await chain.proceed(updatedRequest);
        
        print('   📥 HTTP Response:');
        print('     Status: ${response.statusCode}');
        print('     Headers: ${response.base.headers}');
        
        return response;
      }
    } catch (e) {
      print('   ❌ Error in auth interceptor: $e');
      print('   Using basic auth as fallback');
      // Return request with basic auth as fallback
      final basicAuth = 'Basic ${base64Encode(utf8.encode('SpecialUser:SpecialPassword1'))}';
      final updatedRequest = request.copyWith(
        headers: {
          ...request.headers,
          'Authorization': basicAuth,
        },
      );
      
      final response = await chain.proceed(updatedRequest);
      print('   Fallback response status: ${response.statusCode}');
      return response;
    }
  }
}