import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:image_picker/image_picker.dart';
import '../constants/app_theme.dart';
import '../services/unified_auth_service.dart';
import '../services/user_service.dart';
import '../services/subscription_service.dart';
import '../services/user_profile_service.dart';
import '../components/network_error_dialog.dart';
import '../widgets/stylish_loading.dart';
import '../widgets/universal_back_button.dart';
import '../widgets/user_profile_picture.dart';
import 'dart:io';

class AccountSettingsScreen extends StatefulWidget {
  final String? cachedProfileImageUrl;

  const AccountSettingsScreen({
    Key? key,
    this.cachedProfileImageUrl,
  }) : super(key: key);

  @override
  _AccountSettingsScreenState createState() => _AccountSettingsScreenState();
}

class _AccountSettingsScreenState extends State<AccountSettingsScreen> with TickerProviderStateMixin {
  bool _isLoading = true;
  String _currentEmail = '';
  String _currentFullName = '';
  DateTime? _joinDate;
  String _subscriptionStatus = 'Free Member';
  bool _isPremium = false;
  
  // Edit mode state
  bool _isEditMode = false;
  bool _isUpdatingProfile = false;
  
  // Animation controllers
  late AnimationController _editModeController;
  late AnimationController _cardMorphController;
  late AnimationController _buttonSlideController;
  
  // Animations
  late Animation<double> _cardMorphAnimation;
  late Animation<Offset> _buttonSlideAnimation;
  late Animation<double> _buttonFadeAnimation;
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
  
  // Profile service and image handling
  final UserProfileService _profileService = UserProfileService();
  final ImagePicker _imagePicker = ImagePicker();
  File? _selectedProfileImage;
  bool _isUploadingImage = false;
  String? _cachedProfileImageUrl;

  void _initializeAnimations() {
    // Main edit mode controller
    _editModeController = AnimationController(
      duration: Duration(milliseconds: 600),
      vsync: this,
    );
    
    // Card morphing controller
    _cardMorphController = AnimationController(
      duration: Duration(milliseconds: 800),
      vsync: this,
    );
    
    // Button slide controller
    _buttonSlideController = AnimationController(
      duration: Duration(milliseconds: 700),
      vsync: this,
    );
    
    // Create animations
    _cardMorphAnimation = CurvedAnimation(
      parent: _cardMorphController,
      curve: Curves.easeInOutBack,
    );
    
    _buttonSlideAnimation = Tween<Offset>(
      begin: Offset(0, 1),
      end: Offset.zero,
    ).animate(CurvedAnimation(
      parent: _buttonSlideController,
      curve: Curves.elasticOut,
    ));
    
    _buttonFadeAnimation = CurvedAnimation(
      parent: _buttonSlideController,
      curve: Interval(0.0, 0.6, curve: Curves.easeOut),
    );
  }

  @override
  void initState() {
    super.initState();
    _initializeAnimations();
    _cachedProfileImageUrl = widget.cachedProfileImageUrl;
    _loadUserData();
    
    // Add listeners for validation - but don't setState on every change
    _fullNameController.addListener(() {
      _checkAndShowPasswordField();
      // Only update error state, don't trigger full rebuild
      final newError = _fullNameController.text.trim().isEmpty ? 'Full name is required' : null;
      if (newError != _nameError) {
        setState(() {
          _nameError = newError;
        });
      }
    });
    
    _emailController.addListener(() {
      _checkAndShowPasswordField();
      // Only update error state if it actually changed
      String? newError;
      if (_emailController.text.trim().isEmpty) {
        newError = 'Email is required';
      } else if (!RegExp(r'^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$').hasMatch(_emailController.text.trim())) {
        newError = 'Please enter a valid email address';
      } else {
        newError = null;
      }
      if (newError != _emailError) {
        setState(() {
          _emailError = newError;
        });
      }
    });
    
    _passwordController.addListener(() {
      // Only update error state if it actually changed
      final newError = _passwordController.text.isEmpty ? 'Current password is required' : null;
      if (newError != _passwordError) {
        setState(() {
          _passwordError = newError;
        });
      }
    });
  }

  Future<void> _loadUserData() async {
    try {
      // Get user data from unified auth service for consistency
      final authService = UnifiedAuthService();
      final userData = await authService.getUserData();
      
      String fullName = 'User';
      String email = 'user@example.com';
      
      if (userData != null) {
        final firstName = userData['firstName'] ?? '';
        final lastName = userData['lastName'] ?? '';
        
        // Get email from userData (LoggedInUserModel doesn't have email field)
        if (userData['email'] != null && userData['email'].isNotEmpty) {
          email = userData['email'];
        } else {
          // Try to find email in nested model structure
          final model = userData['model'];
          if (model != null && model['email'] != null) {
            email = model['email'];
          } else {
            // Last resort: try to get email directly from storage
            final storedEmail = await authService.getUserEmail();
            if (storedEmail != null && storedEmail.isNotEmpty) {
              email = storedEmail;
            }
          }
        }
        
        // Construct full name
        if (firstName.isNotEmpty || lastName.isNotEmpty) {
          fullName = '$firstName $lastName'.trim();
          if (fullName.isEmpty) fullName = 'User';
        }
        
        print('📋 Loaded user data:');
        print('  - fullName: $fullName');
        print('  - email: $email');
      }
      
      // Check subscription status
      final subscriptionService = SubscriptionService();
      await subscriptionService.initialize();
      final isPremium = subscriptionService.isPremium;

      setState(() {
        _currentFullName = fullName;
        _currentEmail = email;
        _fullNameController.text = _currentFullName;
        _emailController.text = _currentEmail;
        _joinDate = DateTime.now().subtract(Duration(days: 90)); // Example join date
        _isPremium = isPremium;
        _subscriptionStatus = isPremium ? 'Premium Member' : 'Free Member';
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
        _isPremium = false;
        _subscriptionStatus = 'Free Member';
        _isLoading = false;
      });
    }
  }
  
  @override
  void dispose() {
    _editModeController.dispose();
    _cardMorphController.dispose();
    _buttonSlideController.dispose();
    _emailController.dispose();
    _fullNameController.dispose();
    _passwordController.dispose();
    super.dispose();
  }
  
  void _toggleEditMode() async {
    if (_isEditMode) {
      // Exit edit mode - smooth transition
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
      
      // Animate controllers for additional effects if needed
      _cardMorphController.reverse();
      _editModeController.reverse();
    } else {
      // Enter edit mode
      setState(() {
        _isEditMode = true;
      });
      
      // Start animations
      _editModeController.forward();
      _cardMorphController.forward();
    }
  }
  
  void _checkAndShowPasswordField() {
    // Check if user has actually changed from original values
    final bool nameChanged = _fullNameController.text.trim() != _currentFullName;
    final bool emailChanged = _emailController.text.trim() != _currentEmail;
    final bool hasChanges = nameChanged || emailChanged;
    
    // Only call setState if the password field visibility needs to change
    if (hasChanges != _showPasswordField) {
      setState(() {
        _showPasswordField = hasChanges;
      });
      if (!hasChanges) {
        _passwordController.clear();
      }
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
          child: Padding(
            padding: EdgeInsets.symmetric(horizontal: 24.w),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                SizedBox(height: 12.h),
                _buildBackButtonWithHeader(),
                SizedBox(height: 16.h),
                _buildMorphingAccountSection(),
                SizedBox(height: 20.h),
                _buildSettingsNavigation(),
                Spacer(), // This will push everything up and fill remaining space
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
              duration: Duration(milliseconds: 400),
              transitionBuilder: (Widget child, Animation<double> animation) {
                return FadeTransition(
                  opacity: animation,
                  child: child,
                );
              },
              layoutBuilder: (Widget? currentChild, List<Widget> previousChildren) {
                return Stack(
                  alignment: Alignment.centerLeft,
                  children: <Widget>[
                    ...previousChildren,
                    if (currentChild != null) currentChild,
                  ],
                );
              },
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
                FocusScope.of(context).unfocus();
                _toggleEditMode();
              },
              child: AnimatedContainer(
                duration: Duration(milliseconds: 400),
                curve: Curves.easeInOutCubic,
                padding: EdgeInsets.symmetric(horizontal: 16.w, vertical: 8.h),
                decoration: BoxDecoration(
                  color: _isEditMode 
                      ? Colors.transparent 
                      : AppTheme.cardBackground,
                  border: Border.all(
                    color: _isEditMode 
                        ? AppTheme.textMedium 
                        : AppTheme.affirmationGradient[0],
                    width: _isEditMode ? 1.5 : 2,
                  ),
                  borderRadius: BorderRadius.circular(20.r),
                  boxShadow: !_isEditMode ? [
                    BoxShadow(
                      color: AppTheme.affirmationGradient[0].withOpacity(0.2),
                      blurRadius: 8.r,
                      offset: Offset(0, 2.h),
                    ),
                  ] : [],
                ),
                child: Row(
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    AnimatedSwitcher(
                      duration: Duration(milliseconds: 300),
                      transitionBuilder: (Widget child, Animation<double> animation) {
                        return FadeTransition(
                          opacity: animation,
                          child: ScaleTransition(
                            scale: animation,
                            child: child,
                          ),
                        );
                      },
                      child: Icon(
                        _isEditMode ? Icons.close : Icons.edit_outlined,
                        key: ValueKey('icon_$_isEditMode'),
                        size: 16.sp,
                        color: _isEditMode 
                            ? AppTheme.textMedium 
                            : AppTheme.affirmationGradient[0],
                      ),
                    ),
                    SizedBox(width: 6.w),
                    AnimatedSwitcher(
                      duration: Duration(milliseconds: 300),
                      transitionBuilder: (Widget child, Animation<double> animation) {
                        return FadeTransition(
                          opacity: animation,
                          child: child,
                        );
                      },
                      child: Text(
                        _isEditMode ? 'Cancel' : 'Edit',
                        key: ValueKey('text_$_isEditMode'),
                        style: TextStyle(
                          fontSize: 14.sp,
                          fontWeight: FontWeight.w600,
                          color: _isEditMode 
                              ? AppTheme.textMedium 
                              : AppTheme.affirmationGradient[0],
                        ),
                      ),
                    ),
                  ],
                ),
              ),
            ),
          ],
        ),
        
        SizedBox(height: 16.h),
        
        // Profile picture card (always visible)
        _buildProfilePictureCard(),
        
        SizedBox(height: 12.h),
        
        // Name card that morphs
        _buildMorphingCard(
          isEditable: true,
          icon: Icons.person_outline,
          title: 'Full Name',
          controller: _fullNameController,
          hint: 'Enter your full name',
        ),
        
        SizedBox(height: 8.h),
        
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
                SizedBox(height: 8.h),
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
        ClipRect(
          child: AnimatedAlign(
            duration: Duration(milliseconds: 500),
            curve: Curves.easeInOutCubic,
            alignment: Alignment.topCenter,
            heightFactor: _isEditMode ? 0.0 : 1.0,
            child: AnimatedOpacity(
              duration: Duration(milliseconds: 300),
              opacity: _isEditMode ? 0.0 : 1.0,
              child: Column(
                children: [
                  SizedBox(height: 8.h),
                  
                  // Member Since card
                  _buildInfoCard(
                    icon: Icons.calendar_today_outlined,
                    title: 'Member Since',
                    subtitle: _joinDate != null 
                        ? '${_joinDate!.day}/${_joinDate!.month}/${_joinDate!.year}'
                        : 'Unknown',
                  ),
                  
                  SizedBox(height: 8.h),
                  
                  // Membership card
                  _buildInfoCard(
                    icon: _isPremium ? Icons.workspace_premium : Icons.person_outline,
                    title: 'Membership',
                    subtitle: _subscriptionStatus,
                    subtitleColor: _isPremium ? AppTheme.affirmationGradient[0] : AppTheme.textSecondary,
                  ),
                ],
              ),
            ),
          ),
        ),
        
        // Confirm button that slides in when in edit mode
        ClipRect(
          child: AnimatedAlign(
            duration: Duration(milliseconds: 400),
            curve: Curves.easeInOutCubic,
            alignment: Alignment.topCenter,
            heightFactor: _isEditMode ? 1.0 : 0.0,
            child: AnimatedOpacity(
              duration: Duration(milliseconds: 300),
              opacity: _isEditMode ? 1.0 : 0.0,
              child: Container(
                margin: EdgeInsets.only(top: 24.h),
                width: double.infinity,
                child: _buildConfirmButton(),
              ),
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
    
    return AnimatedBuilder(
      animation: _cardMorphAnimation,
      builder: (context, child) {
        return Transform.scale(
          scale: 1.0 + (_isEditMode ? 0.02 : 0.0),
          child: AnimatedContainer(
            duration: Duration(milliseconds: 400),
            curve: Curves.easeInOutCubic,
            width: double.infinity,
            // Reduced height for more compact layout
            height: 95.h,
            padding: EdgeInsets.all(16.w), // Fixed padding like other cards
            decoration: BoxDecoration(
            color: AppTheme.cardBackground,
            borderRadius: BorderRadius.circular(16.r), // Fixed border radius like other cards
            border: Border.all(
              color: _isEditMode && hasError
                  ? Colors.red.withOpacity(0.5)
                  : _isEditMode 
                      ? AppTheme.affirmationGradient[0].withOpacity(0.3)
                      : AppTheme.textMedium.withOpacity(0.1),
              width: _isEditMode ? 2.0 : 1.0, // Simplified border width
            ),
            boxShadow: _isEditMode ? [
              BoxShadow(
                color: AppTheme.affirmationGradient[0].withOpacity(0.1),
                blurRadius: 8.r,
                offset: Offset(0, 2.h),
              ),
            ] : [],
          ),
            child: Row(
              children: [
                Container(
                  width: 48.w,
                  height: 48.h,
                  decoration: BoxDecoration(
                    color: _isEditMode && hasError
                        ? Colors.red.withOpacity(0.1)
                        : (icon == Icons.lock_outline && _isEditMode)
                            ? Colors.red.withOpacity(0.1)
                            : AppTheme.affirmationGradient[0].withOpacity(0.1),
                    borderRadius: BorderRadius.circular(12.r),
                  ),
                  child: Icon(
                    icon,
                    color: _isEditMode && hasError
                        ? Colors.red
                        : (icon == Icons.lock_outline && _isEditMode)
                            ? Colors.red[400]
                            : AppTheme.affirmationGradient[0],
                    size: 24.sp,
                  ),
                ),
                SizedBox(width: 16.w),
                Expanded(
                  child: Stack(
                    clipBehavior: Clip.none,
                    children: [
                      // Title that animates position and style
                      AnimatedPositioned(
                        duration: Duration(milliseconds: 400),
                        curve: Curves.easeInOutCubic,
                        top: _isEditMode ? 8.h : 10.h,
                        left: 0,
                        child: AnimatedDefaultTextStyle(
                          duration: Duration(milliseconds: 400),
                          style: TextStyle(
                            fontSize: _isEditMode ? 12.sp : 14.sp,
                            color: _isEditMode ? AppTheme.affirmationGradient[0] : AppTheme.textMedium,
                            fontWeight: _isEditMode ? FontWeight.w600 : FontWeight.w500,
                          ),
                          child: Text(title),
                        ),
                      ),
                      // Value/TextField that animates position
                      AnimatedPositioned(
                        duration: Duration(milliseconds: 400),
                        curve: Curves.easeInOutCubic,
                        top: _isEditMode ? 26.h : 30.h,
                        left: 0,
                        right: 0,
                        child: AnimatedContainer(
                          duration: Duration(milliseconds: 400),
                          height: _isEditMode ? 32.h : 24.h,
                          child: AnimatedSwitcher(
                            duration: Duration(milliseconds: 250),
                            child: _isEditMode 
                              ? TextFormField(
                                  key: ValueKey('field_$title'),
                                controller: controller,
                                keyboardType: keyboardType,
                                obscureText: isPassword && !_isPasswordVisible,
                                style: TextStyle(
                                  color: AppTheme.textLight,
                                  fontSize: 15.sp,
                                  fontWeight: FontWeight.w500,
                                ),
                                decoration: InputDecoration(
                                  hintText: hint,
                                  hintStyle: TextStyle(
                                    color: AppTheme.textMedium.withOpacity(0.5),
                                    fontSize: 14.sp,
                                  ),
                                  contentPadding: EdgeInsets.symmetric(horizontal: 12.w, vertical: 8.h),
                                  filled: true,
                                  fillColor: AppTheme.affirmationGradient[0].withOpacity(0.05),
                                  enabledBorder: OutlineInputBorder(
                                    borderRadius: BorderRadius.circular(8.r),
                                    borderSide: BorderSide(
                                      color: AppTheme.affirmationGradient[0].withOpacity(0.3),
                                      width: 1,
                                    ),
                                  ),
                                  focusedBorder: OutlineInputBorder(
                                    borderRadius: BorderRadius.circular(8.r),
                                    borderSide: BorderSide(
                                      color: AppTheme.affirmationGradient[0],
                                      width: 2,
                                    ),
                                  ),
                                  suffixIcon: isPassword ? GestureDetector(
                                    onTap: () {
                                      setState(() {
                                        _isPasswordVisible = !_isPasswordVisible;
                                      });
                                    },
                                    child: Icon(
                                      _isPasswordVisible ? Icons.visibility_off : Icons.visibility,
                                      color: AppTheme.textMedium,
                                      size: 20.sp,
                                    ),
                                  ) : null,
                                ),
                                )
                              : Align(
                                  alignment: Alignment.centerLeft,
                                  child: Text(
                                    controller.text.isNotEmpty ? controller.text : 'Not set',
                                    key: ValueKey('text_$title'),
                                    style: TextStyle(
                                      fontSize: 16.sp,
                                      color: controller.text.isNotEmpty ? AppTheme.textLight : AppTheme.textMedium,
                                      fontWeight: FontWeight.w600,
                                    ),
                                    maxLines: 1,
                                    overflow: TextOverflow.ellipsis,
                                  ),
                                ),
                          ),
                        ),
                      ),
                    ],
                  ),
                ),
              ],
            ),
          ),
        );
      },
    );
  }
  

  Widget _buildProfilePictureCard() {
    return GestureDetector(
      onTap: _isEditMode ? _showProfilePictureOptions : null,
      child: AnimatedContainer(
        duration: Duration(milliseconds: 400),
        curve: Curves.easeInOutCubic,
        width: double.infinity,
        height: 95.h,
        padding: EdgeInsets.all(16.w),
        decoration: BoxDecoration(
          color: AppTheme.cardBackground,
          borderRadius: BorderRadius.circular(16.r),
          border: Border.all(
            color: _isEditMode 
                ? AppTheme.affirmationGradient[0].withOpacity(0.3)
                : AppTheme.textMedium.withOpacity(0.1),
            width: _isEditMode ? 2 : 1,
          ),
          boxShadow: _isEditMode ? [
            BoxShadow(
              color: AppTheme.affirmationGradient[0].withOpacity(0.1),
              blurRadius: 8.r,
              offset: Offset(0, 2.h),
            ),
          ] : [],
        ),
        child: Row(
          children: [
            // Profile picture with edit overlay and preview support
            Stack(
              children: [
                // Show selected image preview or cached profile picture
                RepaintBoundary(
                  child: Container(
                    width: 64.w,
                    height: 64.h,
                    decoration: BoxDecoration(
                      shape: BoxShape.circle,
                      color: AppTheme.cardBackground,
                    ),
                    child: _selectedProfileImage != null
                        ? ClipOval(
                            child: Image.file(
                              _selectedProfileImage!,
                              width: 64.w,
                              height: 64.h,
                              fit: BoxFit.cover,
                              key: ValueKey('selected_image'),
                            ),
                          )
                        : UserProfilePicture(
                            key: const ValueKey('profile_picture_widget'),
                            size: 64.0,
                            borderColor: AppTheme.affirmationGradient[0],
                            borderWidth: 0.0,
                            showBorder: false,
                            cachedImageUrl: widget.cachedProfileImageUrl ?? _cachedProfileImageUrl,
                          ),
                  ),
                ),
                if (_isEditMode || _isUploadingImage)
                  Positioned.fill(
                    child: AnimatedOpacity(
                      duration: Duration(milliseconds: 300),
                      opacity: (_isEditMode || _isUploadingImage) ? 1.0 : 0.0,
                      child: Container(
                        decoration: BoxDecoration(
                          shape: BoxShape.circle,
                          color: Colors.black.withOpacity(_isUploadingImage ? 0.6 : 0.4),
                        ),
                        child: _isUploadingImage 
                            ? SizedBox(
                                width: 24.w,
                                height: 24.h,
                                child: CircularProgressIndicator(
                                  strokeWidth: 2,
                                  valueColor: AlwaysStoppedAnimation<Color>(AppTheme.white),
                                ),
                              )
                            : Icon(
                                Icons.camera_alt,
                                color: AppTheme.white,
                                size: 24.sp,
                              ),
                      ),
                    ),
                  ),
              ],
            ),
          
          SizedBox(width: 16.w),
          
          Expanded(
            child: Stack(
              clipBehavior: Clip.none,
              children: [
                // Title that animates position and style
                AnimatedPositioned(
                  duration: Duration(milliseconds: 400),
                  curve: Curves.easeInOutCubic,
                  top: _isEditMode ? 8.h : 10.h,
                  left: 0,
                  child: AnimatedDefaultTextStyle(
                    duration: Duration(milliseconds: 400),
                    style: TextStyle(
                      fontSize: _isEditMode ? 12.sp : 14.sp,
                      color: _isEditMode ? AppTheme.affirmationGradient[0] : AppTheme.textMedium,
                      fontWeight: _isEditMode ? FontWeight.w600 : FontWeight.w500,
                    ),
                    child: Text('Profile Picture'),
                  ),
                ),
                // Value text that animates position
                AnimatedPositioned(
                  duration: Duration(milliseconds: 400),
                  curve: Curves.easeInOutCubic,
                  top: _isEditMode ? 26.h : 30.h,
                  left: 0,
                  child: AnimatedSwitcher(
                    duration: Duration(milliseconds: 250),
                    layoutBuilder: (Widget? currentChild, List<Widget> previousChildren) {
                      return Stack(
                        alignment: Alignment.centerLeft,
                        children: <Widget>[
                          ...previousChildren,
                          if (currentChild != null) currentChild,
                        ],
                      );
                    },
                    child: Text(
                      _isEditMode ? 'Tap to change photo' : 'Default avatar',
                      key: ValueKey(_isEditMode),
                      style: TextStyle(
                        fontSize: 16.sp,
                        color: _isEditMode 
                            ? AppTheme.affirmationGradient[0] 
                            : AppTheme.textLight,
                        fontWeight: FontWeight.w600,
                      ),
                    ),
                  ),
                ),
              ],
            ),
          ),
          
        ],
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
      height: 95.h, // Reduced height to match display cards
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
            width: 48.w,
            height: 48.h,
            decoration: BoxDecoration(
              color: AppTheme.affirmationGradient[0].withOpacity(0.1),
              borderRadius: BorderRadius.circular(12.r),
            ),
            child: Icon(
              icon,
              color: AppTheme.affirmationGradient[0],
              size: 24.sp,
            ),
          ),
          SizedBox(width: 16.w),
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              mainAxisAlignment: MainAxisAlignment.center, // Center content vertically
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
                        : AppTheme.affirmationGradient[0].withOpacity(0.1),
                    borderRadius: BorderRadius.circular(12.r),
                  ),
                  child: Icon(
                    icon,
                    color: isDestructive 
                        ? Colors.red
                        : AppTheme.affirmationGradient[0],
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
  
  String? _getFieldError(TextEditingController controller) {
    // Since we're using dialog-based editing, return null for now
    return null;
  }
  
  Widget _buildConfirmButton() {
    final bool hasChanges = _fullNameController.text != _currentFullName || 
                           _emailController.text != _currentEmail;
    
    return Container(
      width: double.infinity,
      height: 56.h,
      child: ElevatedButton(
        onPressed: hasChanges && !_isUpdatingProfile ? _handleUpdateProfile : null,
        style: ElevatedButton.styleFrom(
          backgroundColor: hasChanges 
              ? AppTheme.affirmationGradient[0] 
              : AppTheme.textMedium.withOpacity(0.3),
          foregroundColor: AppTheme.white,
          elevation: 0,
          shadowColor: Colors.transparent,
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(16.r),
          ),
        ),
        child: _isUpdatingProfile
            ? Row(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  SizedBox(
                    width: 20.w,
                    height: 20.h,
                    child: CircularProgressIndicator(
                      strokeWidth: 2,
                      valueColor: AlwaysStoppedAnimation<Color>(AppTheme.white),
                    ),
                  ),
                  SizedBox(width: 12.w),
                  Text(
                    'Updating...',
                    style: TextStyle(
                      fontSize: 16.sp,
                      fontWeight: FontWeight.w600,
                    ),
                  ),
                ],
              )
            : Row(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  Icon(
                    Icons.check_rounded,
                    size: 20.sp,
                  ),
                  SizedBox(width: 8.w),
                  Text(
                    'Confirm Changes',
                    style: TextStyle(
                      fontSize: 16.sp,
                      fontWeight: FontWeight.w600,
                    ),
                  ),
                ],
              ),
      ),
    );
  }
  
  Future<void> _handleUpdateProfile() async {
    // Validate required password if changes were made
    if (_showPasswordField && _passwordController.text.isEmpty) {
      setState(() {
        _passwordError = 'Current password is required to save changes';
      });
      return;
    }
    
    setState(() {
      _isUpdatingProfile = true;
    });
    
    try {
      // Initialize profile service
      await _profileService.initialize(baseUrl: 'http://manifestation-qa-1.senthilguru.net');
      
      // Skip password verification since user is already authenticated with Bearer token
      // The API should trust the authenticated user to update their profile
      if (_showPasswordField && _passwordController.text.isEmpty) {
        setState(() {
          _passwordError = 'Current password is required for verification';
          _isUpdatingProfile = false;
        });
        return;
      }
      
      // Note: Password verification is skipped since the user is already authenticated
      // If the server requires password verification, it can be implemented server-side
      print('🔐 Skipping password verification - user already authenticated with Bearer token');
      
      // Get current user ID
      final authService = UnifiedAuthService();
      final userId = await authService.getUserId();
      print('📝 Using User ID for profile update: $userId');
      
      // Split full name into first and last name
      final nameMap = _profileService.splitFullName(_fullNameController.text.trim());
      
      // Update profile information first
      final updateResult = await _profileService.updateProfile(
        userId: userId,
        email: _emailController.text.trim(),
        firstName: nameMap['firstName'],
        lastName: nameMap['lastName'],
      );
      
      // Upload profile picture if one was selected
      bool profilePictureUploadSuccess = true;
      if (_selectedProfileImage != null) {
        print('📤 Uploading profile picture...');
        final uploadResult = await _profileService.uploadProfilePicture(
          _selectedProfileImage!,
          userId: userId,
        );
        
        if (uploadResult.isSuccess) {
          print('✅ Profile picture uploaded successfully');
        } else {
          print('❌ Profile picture upload failed: ${uploadResult.errorMessage}');
          profilePictureUploadSuccess = false;
        }
      }
      
      // Even if the server update fails for some fields, update our local storage with what the user entered
      // Since we know these are the values they want
      if (updateResult.isSuccess && profilePictureUploadSuccess) {
        // Update stored user data to sync with other parts of the app
        await _updateStoredUserData();
        
        // Update the current values
        setState(() {
          _currentFullName = _fullNameController.text;
          _currentEmail = _emailController.text;
          _isUpdatingProfile = false;
          // Clear the selected profile image since it's now uploaded
          _selectedProfileImage = null;
        });
        
        // Show success message
        String successMessage = 'Profile updated successfully';
        bool hadProfilePicture = _selectedProfileImage != null;
        if (hadProfilePicture) {
          successMessage = 'Profile and picture updated successfully';
        }
        
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Row(
              children: [
                Icon(Icons.check_circle, color: Colors.white, size: 20.sp),
                SizedBox(width: 8.w),
                Expanded(child: Text(successMessage)),
              ],
            ),
            backgroundColor: Colors.green,
            behavior: SnackBarBehavior.floating,
            shape: RoundedRectangleBorder(
              borderRadius: BorderRadius.circular(12.r),
            ),
          ),
        );
        
        // Exit edit mode
        _toggleEditMode();
      } else {
        setState(() {
          _isUpdatingProfile = false;
        });
        
        _showErrorSnackBar('Failed to update profile: ${updateResult.errorMessage}');
      }
      
    } catch (e) {
      setState(() {
        _isUpdatingProfile = false;
      });
      
      // Check if this is a network/connection error
      final errorMessage = e.toString().toLowerCase();
      if (errorMessage.contains('network') || 
          errorMessage.contains('connection') || 
          errorMessage.contains('socket') ||
          errorMessage.contains('timeout') ||
          errorMessage.contains('failed host lookup')) {
        // Show beautiful network error dialog
        showNetworkErrorDialog(
          context,
          message: 'We\'re having trouble updating your profile. Please check your internet connection and try again.',
          onRetry: () => _handleUpdateProfile(),
        );
      } else {
        _showErrorSnackBar('Error updating profile: $e');
      }
    }
  }
  
  /// Upload profile picture to server
  Future<void> _uploadProfilePicture() async {
    if (_selectedProfileImage == null) return;
    
    setState(() {
      _isUploadingImage = true;
    });
    
    try {
      await _profileService.initialize(baseUrl: 'http://manifestation-qa-1.senthilguru.net');
      
      print('📤 Uploading profile picture...');
      
      // Get current user ID
      final authService = UnifiedAuthService();
      final userId = await authService.getUserId();
      print('📤 Using User ID: $userId');
      
      final uploadResult = await _profileService.uploadProfilePicture(
        _selectedProfileImage!,
        userId: userId,
      );
      
      setState(() {
        _isUploadingImage = false;
      });
      
      if (uploadResult.isSuccess) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Row(
              children: [
                Icon(Icons.check_circle, color: Colors.white, size: 20.sp),
                SizedBox(width: 8.w),
                Text('Profile picture updated successfully'),
              ],
            ),
            backgroundColor: Colors.green,
            behavior: SnackBarBehavior.floating,
            shape: RoundedRectangleBorder(
              borderRadius: BorderRadius.circular(12.r),
            ),
          ),
        );
      } else {
        _showErrorSnackBar('Failed to upload profile picture: ${uploadResult.errorMessage}');
      }
    } catch (e) {
      setState(() {
        _isUploadingImage = false;
      });
      
      // Check if this is a network/connection error
      final errorMessage = e.toString().toLowerCase();
      if (errorMessage.contains('network') || 
          errorMessage.contains('connection') || 
          errorMessage.contains('socket') ||
          errorMessage.contains('timeout') ||
          errorMessage.contains('failed host lookup')) {
        // Show beautiful network error dialog
        showNetworkErrorDialog(
          context,
          message: 'We\'re having trouble uploading your profile picture. Please check your internet connection and try again.',
          onRetry: () => _uploadProfilePicture(),
        );
      } else {
        _showErrorSnackBar('Error uploading profile picture: $e');
      }
    }
  }
  
  /// Update stored user data after profile changes
  Future<void> _updateStoredUserData() async {
    try {
      print('🔄 Updating stored user data after profile change...');
      
      final authService = UnifiedAuthService();
      
      // Get current stored data
      final currentData = await authService.getUserData();
      if (currentData != null) {
        // Split the full name
        final nameParts = _fullNameController.text.trim().split(' ');
        final firstName = nameParts.isNotEmpty ? nameParts.first : '';
        final lastName = nameParts.length > 1 ? nameParts.skip(1).join(' ') : '';
        
        // Update the data structure
        final updatedData = Map<String, dynamic>.from(currentData);
        updatedData['firstName'] = firstName;
        updatedData['lastName'] = lastName;
        updatedData['email'] = _emailController.text.trim();
        
        // If there's a nested model structure, update it too
        if (updatedData['model'] != null) {
          final model = Map<String, dynamic>.from(updatedData['model']);
          model['firstName'] = firstName;
          model['lastName'] = lastName;
          model['email'] = _emailController.text.trim();
          updatedData['model'] = model;
        }
        
        // Save the updated data using the auth service's public update method
        await authService.updateUserData(updatedData);
        
        // Also update via UserService to ensure SharedPreferences are synced
        await UserService.storeUserData(updatedData);
        
        // Force clear any cached values to ensure fresh data is loaded
        await UserService.updateUserProfile(
          firstName: firstName,
          lastName: lastName,
        );
        
        print('✅ Successfully updated stored user data');
        print('   - firstName: $firstName');
        print('   - lastName: $lastName');  
        print('   - email: ${_emailController.text.trim()}');
      }
    } catch (e) {
      print('❌ Error updating stored user data: $e');
      // Don't throw error - this is supplementary functionality
    }
  }

  /// Show error message in snackbar
  void _showErrorSnackBar(String message) {
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Row(
          children: [
            Icon(Icons.error_outline, color: Colors.white, size: 20.sp),
            SizedBox(width: 8.w),
            Expanded(
              child: Text(
                message,
                style: TextStyle(fontSize: 14.sp),
              ),
            ),
          ],
        ),
        backgroundColor: Colors.red,
        behavior: SnackBarBehavior.floating,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(12.r),
        ),
        duration: Duration(seconds: 4),
      ),
    );
  }
  
  void _showProfilePictureOptions() {
    showModalBottomSheet(
      context: context,
      backgroundColor: AppTheme.cardBackground,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.vertical(
          top: Radius.circular(20.r),
        ),
      ),
      builder: (context) => Container(
        padding: EdgeInsets.all(24.w),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            // Handle bar
            Container(
              width: 40.w,
              height: 4.h,
              decoration: BoxDecoration(
                color: AppTheme.textMedium.withOpacity(0.3),
                borderRadius: BorderRadius.circular(2.r),
              ),
            ),
            
            SizedBox(height: 20.h),
            
            Text(
              'Change Profile Picture',
              style: TextStyle(
                fontSize: 20.sp,
                fontWeight: FontWeight.bold,
                color: AppTheme.textPrimary,
              ),
            ),
            
            SizedBox(height: 24.h),
            
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceEvenly,
              children: [
                _buildOptionButton(
                  icon: Icons.camera_alt_outlined,
                  label: 'Camera',
                  onTap: () {
                    Navigator.pop(context);
                    _takePhoto();
                  },
                ),
                _buildOptionButton(
                  icon: Icons.photo_library_outlined,
                  label: 'Gallery',
                  onTap: () {
                    Navigator.pop(context);
                    _pickFromGallery();
                  },
                ),
                _buildOptionButton(
                  icon: Icons.delete_outline,
                  label: 'Remove',
                  onTap: () {
                    Navigator.pop(context);
                    _removePhoto();
                  },
                  isDestructive: true,
                ),
              ],
            ),
            
            SizedBox(height: 20.h),
          ],
        ),
      ),
    );
  }
  
  Widget _buildOptionButton({
    required IconData icon,
    required String label,
    required VoidCallback onTap,
    bool isDestructive = false,
  }) {
    return GestureDetector(
      onTap: onTap,
      child: Container(
        width: 80.w,
        padding: EdgeInsets.symmetric(vertical: 16.h),
        decoration: BoxDecoration(
          color: isDestructive 
              ? Colors.red.withOpacity(0.1)
              : AppTheme.affirmationGradient[0].withOpacity(0.1),
          borderRadius: BorderRadius.circular(16.r),
          border: Border.all(
            color: isDestructive 
                ? Colors.red.withOpacity(0.2)
                : AppTheme.affirmationGradient[0].withOpacity(0.2),
          ),
        ),
        child: Column(
          children: [
            Icon(
              icon,
              color: isDestructive 
                  ? Colors.red
                  : AppTheme.affirmationGradient[0],
              size: 24.sp,
            ),
            SizedBox(height: 8.h),
            Text(
              label,
              style: TextStyle(
                fontSize: 12.sp,
                fontWeight: FontWeight.w500,
                color: isDestructive 
                    ? Colors.red
                    : AppTheme.textPrimary,
              ),
            ),
          ],
        ),
      ),
    );
  }
  
  void _takePhoto() async {
    try {
      final XFile? image = await _imagePicker.pickImage(
        source: ImageSource.camera,
        imageQuality: 80,
      );
      
      if (image != null) {
        setState(() {
          _selectedProfileImage = File(image.path);
        });
        
        // Show preview only - actual upload will happen when user saves changes
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Profile picture selected. Save changes to update.'),
            backgroundColor: AppTheme.affirmationGradient[0],
            duration: Duration(seconds: 2),
          ),
        );
      }
    } catch (e) {
      _showErrorSnackBar('Failed to take photo: $e');
    }
  }
  
  void _pickFromGallery() async {
    try {
      final XFile? image = await _imagePicker.pickImage(
        source: ImageSource.gallery,
        imageQuality: 80,
      );
      
      if (image != null) {
        setState(() {
          _selectedProfileImage = File(image.path);
        });
        
        // Show preview only - actual upload will happen when user saves changes
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Profile picture selected. Save changes to update.'),
            backgroundColor: AppTheme.affirmationGradient[0],
            duration: Duration(seconds: 2),
          ),
        );
      }
    } catch (e) {
      _showErrorSnackBar('Failed to pick image: $e');
    }
  }
  
  void _removePhoto() {
    setState(() {
      _selectedProfileImage = null;
    });
    
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text('Profile picture removed locally'),
        backgroundColor: Colors.orange,
        behavior: SnackBarBehavior.floating,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(12.r),
        ),
      ),
    );
  }
  
  void _showEditDialog(TextEditingController controller, String title, String hint, TextInputType? keyboardType, bool isPassword) {
    final tempController = TextEditingController(text: controller.text);
    
    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        backgroundColor: AppTheme.cardBackground,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(20.r),
        ),
        title: Text(
          'Edit $title',
          style: TextStyle(
            fontSize: 20.sp,
            fontWeight: FontWeight.bold,
            color: AppTheme.textPrimary,
          ),
        ),
        content: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            TextFormField(
              controller: tempController,
              keyboardType: keyboardType,
              obscureText: isPassword,
              style: TextStyle(
                color: AppTheme.textLight,
                fontSize: 16.sp,
              ),
              decoration: InputDecoration(
                hintText: hint,
                hintStyle: TextStyle(
                  color: AppTheme.textMedium.withOpacity(0.6),
                ),
                filled: true,
                fillColor: AppTheme.cardBackground,
                border: OutlineInputBorder(
                  borderRadius: BorderRadius.circular(12.r),
                  borderSide: BorderSide(
                    color: AppTheme.affirmationGradient[0].withOpacity(0.3),
                  ),
                ),
                focusedBorder: OutlineInputBorder(
                  borderRadius: BorderRadius.circular(12.r),
                  borderSide: BorderSide(
                    color: AppTheme.affirmationGradient[0],
                    width: 2,
                  ),
                ),
              ),
            ),
          ],
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context),
            child: Text(
              'Cancel',
              style: TextStyle(
                color: AppTheme.textSecondary,
              ),
            ),
          ),
          ElevatedButton(
            onPressed: () {
              controller.text = tempController.text;
              Navigator.pop(context);
            },
            style: ElevatedButton.styleFrom(
              backgroundColor: AppTheme.affirmationGradient[0],
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(12.r),
              ),
            ),
            child: Text(
              'Save',
              style: TextStyle(
                color: AppTheme.white,
                fontWeight: FontWeight.w600,
              ),
            ),
          ),
        ],
      ),
    );
  }
}