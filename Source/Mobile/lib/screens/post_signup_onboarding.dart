import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:shared_preferences/shared_preferences.dart';
import '../constants/app_theme.dart';
import 'home_screen.dart';

class PostSignupOnboardingScreen extends StatefulWidget {
  @override
  _PostSignupOnboardingScreenState createState() => _PostSignupOnboardingScreenState();
}

class _PostSignupOnboardingScreenState extends State<PostSignupOnboardingScreen>
    with TickerProviderStateMixin {
  
  late PageController _pageController;
  late AnimationController _pulseController;
  late AnimationController _iconFadeController;
  late Animation<double> _pulseAnimation;
  late Animation<double> _iconFadeAnimation;
  
  int _currentPage = 0;
  final int _totalPages = 5;
  bool _isAnimating = false;

  @override
  void initState() {
    super.initState();
    
    _pageController = PageController();
    
    // Continuous pulse animation - NEVER stops or resets
    _pulseController = AnimationController(
      duration: Duration(milliseconds: 2000),
      vsync: this,
    );
    
    // Separate fade animation for icon changes
    _iconFadeController = AnimationController(
      duration: Duration(milliseconds: 300),
      vsync: this,
    );
    
    _pulseAnimation = Tween<double>(
      begin: 0.95,
      end: 1.05,
    ).animate(CurvedAnimation(
      parent: _pulseController,
      curve: Curves.easeInOut,
    ));
    
    _iconFadeAnimation = CurvedAnimation(
      parent: _iconFadeController,
      curve: Curves.easeInOut,
    );
    
    // Start continuous pulse and never stop it
    _pulseController.repeat(reverse: true);
    _iconFadeController.forward();
  }

  @override
  void dispose() {
    _pageController.dispose();
    _pulseController.dispose();
    _iconFadeController.dispose();
    super.dispose();
  }

  void _nextPage() async {
    if (_isAnimating) return;
    
    if (_currentPage < _totalPages - 1) {
      setState(() => _isAnimating = true);
      
      // Fade out icon
      await _iconFadeController.reverse();
      
      // Change page
      await _pageController.animateToPage(
        _currentPage + 1,
        duration: Duration(milliseconds: 200),
        curve: Curves.easeOut,
      );
      
      // Fade in new icon
      await _iconFadeController.forward();
      
      setState(() => _isAnimating = false);
    } else {
      _completeOnboarding();
    }
  }

  void _previousPage() async {
    if (_isAnimating || _currentPage == 0) return;
    
    setState(() => _isAnimating = true);
    
    // Fade out icon
    await _iconFadeController.reverse();
    
    // Change page
    await _pageController.animateToPage(
      _currentPage - 1,
      duration: Duration(milliseconds: 200),
      curve: Curves.easeOut,
    );
    
    // Fade in new icon
    await _iconFadeController.forward();
    
    setState(() => _isAnimating = false);
  }

  void _skipOnboarding() {
    _completeOnboarding();
  }

  Future<void> _completeOnboarding() async {
    final prefs = await SharedPreferences.getInstance();
    await prefs.setBool('post_signup_onboarding_completed', true);
    
    Navigator.pushReplacement(
      context,
      PageRouteBuilder(
        pageBuilder: (context, animation, secondaryAnimation) => HomeScreen(),
        transitionsBuilder: (context, animation, secondaryAnimation, child) {
          return FadeTransition(opacity: animation, child: child);
        },
        transitionDuration: Duration(milliseconds: 400),
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppTheme.backgroundDark,
      body: SafeArea(
        child: Column(
          children: [
            // Header
            _buildHeader(),
            
            // FIXED LAYOUT - circle always in same spot
            Expanded(
              child: Stack(
                children: [
                  // Fixed position pulsing circle - NEVER moves
                  Positioned(
                    top: 80.h, // Fixed position from top
                    left: 0,
                    right: 0,
                    child: Center(
                      child: AnimatedBuilder(
                        animation: _pulseAnimation,
                        builder: (context, child) {
                          return Transform.scale(
                            scale: _pulseAnimation.value,
                            child: _buildCurrentIcon(),
                          );
                        },
                      ),
                    ),
                  ),
                  
                  // Text content below circle
                  Positioned(
                    top: 240.h, // Fixed position for text
                    left: 0,
                    right: 0,
                    bottom: 0,
                    child: PageView(
                      controller: _pageController,
                      physics: NeverScrollableScrollPhysics(),
                      onPageChanged: (page) {
                        setState(() {
                          _currentPage = page;
                        });
                      },
                      children: [
                        _buildTextContent(
                          'Welcome to Your Journey!',
                          'You\'ve taken the first step towards manifesting your dreams. Let\'s explore what awaits you.',
                        ),
                        _buildTextContent(
                          'Daily Affirmations',
                          'Start each day with powerful affirmations tailored to your goals. Rewire your mindset for success.',
                        ),
                        _buildTextContent(
                          'Vision Boards',
                          'Create visual representations of your dreams. See it, believe it, achieve it.',
                        ),
                        _buildTextContent(
                          'Manifestation Journal',
                          'Track your thoughts, gratitude, and progress. Document your transformation journey.',
                        ),
                        _buildFinalContent(),
                      ],
                    ),
                  ),
                ],
              ),
            ),
            
            // Navigation controls
            _buildNavigationControls(),
            SizedBox(height: 30.h),
          ],
        ),
      ),
    );
  }

  Widget _buildCurrentIcon() {
    IconData icon;
    List<Color> gradient;
    
    switch (_currentPage) {
      case 0:
        icon = Icons.celebration;
        gradient = AppTheme.affirmationGradient;
        break;
      case 1:
        icon = Icons.favorite;
        gradient = AppTheme.affirmationGradient;
        break;
      case 2:
        icon = Icons.dashboard;
        gradient = AppTheme.visionBoardGradient;
        break;
      case 3:
        icon = Icons.book;
        gradient = AppTheme.journalGradient;
        break;
      case 4:
        icon = Icons.rocket_launch;
        gradient = AppTheme.affirmationGradient;
        break;
      default:
        icon = Icons.celebration;
        gradient = AppTheme.affirmationGradient;
    }
    
    return FadeTransition(
      opacity: _iconFadeAnimation,
      child: Container(
        width: 120.w,
        height: 120.h,
        decoration: BoxDecoration(
          gradient: LinearGradient(colors: gradient),
          shape: BoxShape.circle,
          boxShadow: [
            BoxShadow(
              color: gradient[0].withOpacity(0.3),
              blurRadius: 25.r,
              offset: Offset(0, 8.h),
            ),
          ],
        ),
        child: Icon(
          icon,
          color: AppTheme.white,
          size: 60.sp,
        ),
      ),
    );
  }

  Widget _buildTextContent(String title, String description) {
    return Padding(
      padding: EdgeInsets.symmetric(horizontal: 24.w),
      child: Column(
        children: [
          // Title with consistent height
          SizedBox(
            height: 60.h,
            child: Center(
              child: Text(
                title,
                style: TextStyle(
                  fontSize: 24.sp,
                  fontWeight: FontWeight.bold,
                  color: AppTheme.textLight,
                ),
                textAlign: TextAlign.center,
                maxLines: 2,
              ),
            ),
          ),
          
          SizedBox(height: 16.h),
          
          // Description with consistent height
          SizedBox(
            height: 80.h,
            child: Center(
              child: Text(
                description,
                style: TextStyle(
                  fontSize: 15.sp,
                  color: AppTheme.textMedium,
                  height: 1.4,
                ),
                textAlign: TextAlign.center,
                maxLines: 4,
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildFinalContent() {
    return Padding(
      padding: EdgeInsets.symmetric(horizontal: 24.w),
      child: Column(
        children: [
          // Title with same height as others
          SizedBox(
            height: 60.h,
            child: Center(
              child: Text(
                'You\'re All Set!',
                style: TextStyle(
                  fontSize: 24.sp,
                  fontWeight: FontWeight.bold,
                  color: AppTheme.textLight,
                ),
                textAlign: TextAlign.center,
              ),
            ),
          ),
          
          SizedBox(height: 16.h),
          
          // Description with same height
          SizedBox(
            height: 80.h,
            child: Center(
              child: Text(
                'Your manifestation journey begins now. Let\'s make your dreams a reality together.',
                style: TextStyle(
                  fontSize: 15.sp,
                  color: AppTheme.textMedium,
                  height: 1.4,
                ),
                textAlign: TextAlign.center,
                maxLines: 4,
              ),
            ),
          ),
          
          SizedBox(height: 30.h),
          
          // Button
          Container(
            width: double.infinity,
            height: 50.h,
            decoration: BoxDecoration(
              gradient: LinearGradient(colors: AppTheme.affirmationGradient),
              borderRadius: BorderRadius.circular(25.r),
              boxShadow: [
                BoxShadow(
                  color: AppTheme.affirmationGradient[0].withOpacity(0.25),
                  blurRadius: 15.r,
                  offset: Offset(0, 8.h),
                ),
              ],
            ),
            child: Material(
              color: Colors.transparent,
              child: InkWell(
                onTap: _completeOnboarding,
                borderRadius: BorderRadius.circular(25.r),
                child: Center(
                  child: Text(
                    'Start Manifesting',
                    style: TextStyle(
                      fontSize: 16.sp,
                      fontWeight: FontWeight.w600,
                      color: AppTheme.white,
                    ),
                  ),
                ),
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildHeader() {
    return Padding(
      padding: EdgeInsets.symmetric(horizontal: 24.w, vertical: 16.h),
      child: Row(
        mainAxisAlignment: MainAxisAlignment.spaceBetween,
        children: [
          Expanded(
            child: Row(
              children: List.generate(_totalPages, (index) {
                return AnimatedContainer(
                  duration: Duration(milliseconds: 300),
                  margin: EdgeInsets.only(right: 6.w),
                  width: index == _currentPage ? 28.w : 8.w,
                  height: 4.h,
                  decoration: BoxDecoration(
                    color: index <= _currentPage 
                        ? AppTheme.affirmationGradient[0]
                        : AppTheme.textMedium.withOpacity(0.2),
                    borderRadius: BorderRadius.circular(2.r),
                  ),
                );
              }),
            ),
          ),
          SizedBox(
            width: 50.w,
            child: _currentPage < _totalPages - 1
                ? GestureDetector(
                    onTap: _skipOnboarding,
                    child: Text(
                      'Skip',
                      style: TextStyle(
                        fontSize: 15.sp,
                        color: AppTheme.textMedium,
                        fontWeight: FontWeight.w500,
                      ),
                      textAlign: TextAlign.end,
                    ),
                  )
                : SizedBox(),
          ),
        ],
      ),
    );
  }

  Widget _buildNavigationControls() {
    return Padding(
      padding: EdgeInsets.symmetric(horizontal: 24.w),
      child: Row(
        mainAxisAlignment: MainAxisAlignment.spaceBetween,
        children: [
          Container(
            width: 50.w,
            height: 50.h,
            child: _currentPage > 0
                ? Material(
                    color: AppTheme.cardBackground,
                    borderRadius: BorderRadius.circular(12.r),
                    child: InkWell(
                      onTap: _previousPage,
                      borderRadius: BorderRadius.circular(12.r),
                      child: Icon(Icons.arrow_back, color: AppTheme.textLight, size: 20.sp),
                    ),
                  )
                : SizedBox(),
          ),
          if (_currentPage < _totalPages - 1)
            Material(
              color: Colors.transparent,
              child: InkWell(
                onTap: _nextPage,
                borderRadius: BorderRadius.circular(25.r),
                child: Container(
                  padding: EdgeInsets.symmetric(horizontal: 28.w, vertical: 12.h),
                  decoration: BoxDecoration(
                    gradient: LinearGradient(colors: AppTheme.affirmationGradient),
                    borderRadius: BorderRadius.circular(25.r),
                    boxShadow: [
                      BoxShadow(
                        color: AppTheme.affirmationGradient[0].withOpacity(0.25),
                        blurRadius: 10.r,
                        offset: Offset(0, 4.h),
                      ),
                    ],
                  ),
                  child: Row(
                    mainAxisSize: MainAxisSize.min,
                    children: [
                      Text('Continue', style: TextStyle(fontSize: 16.sp, fontWeight: FontWeight.w600, color: AppTheme.white)),
                      SizedBox(width: 6.w),
                      Icon(Icons.arrow_forward, color: AppTheme.white, size: 18.sp),
                    ],
                  ),
                ),
              ),
            ),
        ],
      ),
    );
  }
}