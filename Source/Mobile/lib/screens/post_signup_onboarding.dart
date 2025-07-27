import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:shared_preferences/shared_preferences.dart';
import '../constants/colors.dart';
import 'home_screen.dart';

class PostSignupOnboardingScreen extends StatefulWidget {
  @override
  _PostSignupOnboardingScreenState createState() => _PostSignupOnboardingScreenState();
}

class _PostSignupOnboardingScreenState extends State<PostSignupOnboardingScreen>
    with TickerProviderStateMixin {
  
  late PageController _pageController;
  late AnimationController _fadeController;
  late AnimationController _slideController;
  late Animation<double> _fadeAnimation;
  late Animation<Offset> _slideAnimation;
  
  int _currentPage = 0;
  final int _totalPages = 5;

  @override
  void initState() {
    super.initState();
    
    _pageController = PageController();
    
    _fadeController = AnimationController(
      duration: Duration(milliseconds: 800),
      vsync: this,
    );
    
    _slideController = AnimationController(
      duration: Duration(milliseconds: 600),
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
    _pageController.dispose();
    _fadeController.dispose();
    _slideController.dispose();
    super.dispose();
  }

  void _nextPage() {
    if (_currentPage < _totalPages - 1) {
      _pageController.nextPage(
        duration: Duration(milliseconds: 300),
        curve: Curves.easeInOut,
      );
    } else {
      _completeOnboarding();
    }
  }

  void _previousPage() {
    if (_currentPage > 0) {
      _pageController.previousPage(
        duration: Duration(milliseconds: 300),
        curve: Curves.easeInOut,
      );
    }
  }

  void _skipOnboarding() {
    _completeOnboarding();
  }

  Future<void> _completeOnboarding() async {
    // Mark onboarding as completed
    final prefs = await SharedPreferences.getInstance();
    await prefs.setBool('post_signup_onboarding_completed', true);
    
    // Navigate to home screen
    Navigator.pushReplacement(
      context,
      PageRouteBuilder(
        pageBuilder: (context, animation, secondaryAnimation) => HomeScreen(),
        transitionsBuilder: (context, animation, secondaryAnimation, child) {
          return FadeTransition(opacity: animation, child: child);
        },
        transitionDuration: Duration(milliseconds: 500),
      ),
    );
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
            child: Column(
              children: [
                // Header with skip button
                _buildHeader(),
                
                // Page content
                Expanded(
                  child: PageView(
                    controller: _pageController,
                    onPageChanged: (page) {
                      setState(() {
                        _currentPage = page;
                      });
                    },
                    children: [
                      _buildWelcomePage(),
                      _buildAffirmationsPage(),
                      _buildVisionBoardPage(),
                      _buildJournalPage(),
                      _buildGetStartedPage(),
                    ],
                  ),
                ),
                
                // Navigation controls
                _buildNavigationControls(),
                
                SizedBox(height: 40.h),
              ],
            ),
          ),
        ),
      ),
    );
  }

  Widget _buildHeader() {
    return Padding(
      padding: EdgeInsets.symmetric(horizontal: 24.w, vertical: 20.h),
      child: Row(
        mainAxisAlignment: MainAxisAlignment.spaceBetween,
        children: [
          // Progress indicator
          Row(
            children: List.generate(_totalPages, (index) {
              return Container(
                margin: EdgeInsets.only(right: 8.w),
                width: index == _currentPage ? 24.w : 8.w,
                height: 4.h,
                decoration: BoxDecoration(
                  color: index == _currentPage 
                      ? AppColors.affirmationGradient[0]
                      : AppColors.textMedium.withOpacity(0.3),
                  borderRadius: BorderRadius.circular(2.r),
                ),
              );
            }),
          ),
          
          // Skip button area (always present to prevent layout shift)
          SizedBox(
            width: 40.w,
            child: _currentPage < _totalPages - 1
                ? GestureDetector(
                    onTap: _skipOnboarding,
                    child: Text(
                      'Skip',
                      style: TextStyle(
                        fontSize: 16.sp,
                        color: AppColors.textMedium,
                        fontWeight: FontWeight.w500,
                      ),
                    ),
                  )
                : SizedBox(), // Empty space to maintain layout
          ),
        ],
      ),
    );
  }

  Widget _buildWelcomePage() {
    return _buildOnboardingPage(
      icon: Icons.auto_awesome,
      gradient: AppColors.affirmationGradient,
      title: 'Welcome to Manifestation!',
      subtitle: 'Your journey to transform dreams into reality begins now',
      description: 'We\'re excited to guide you through the powerful tools that will help you manifest your deepest desires and create the life you\'ve always dreamed of.',
    );
  }

  Widget _buildAffirmationsPage() {
    return _buildOnboardingPage(
      icon: Icons.favorite,
      gradient: AppColors.affirmationGradient,
      title: 'Daily Affirmations',
      subtitle: 'Rewire your mindset with powerful words',
      description: 'Start each day with personalized affirmations that align your thoughts with your goals. Watch as positive thinking becomes your natural state.',
    );
  }

  Widget _buildVisionBoardPage() {
    return _buildOnboardingPage(
      icon: Icons.dashboard,
      gradient: AppColors.visionBoardGradient,
      title: 'Vision Boards',
      subtitle: 'Visualize your dreams into existence',
      description: 'Create beautiful vision boards with images and words that represent your goals. What you can see, you can achieve.',
    );
  }

  Widget _buildJournalPage() {
    return _buildOnboardingPage(
      icon: Icons.book,
      gradient: AppColors.journalGradient,
      title: 'Manifestation Journal',
      subtitle: 'Track your thoughts and progress',
      description: 'Document your manifestation journey, reflect on your growth, and celebrate your wins along the way.',
    );
  }


  Widget _buildGetStartedPage() {
    return _buildOnboardingPage(
      icon: Icons.rocket_launch,
      gradient: AppColors.affirmationGradient,
      title: 'You\'re All Set!',
      subtitle: 'Ready to manifest your dreams',
      description: 'Everything is in place for your transformation. Take a deep breath and step into your new reality with confidence.',
      isLastPage: true,
    );
  }

  Widget _buildOnboardingPage({
    required IconData icon,
    required List<Color> gradient,
    required String title,
    required String subtitle,
    required String description,
    bool isLastPage = false,
  }) {
    return Padding(
      padding: EdgeInsets.symmetric(horizontal: 32.w),
      child: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          // Animated icon
          TweenAnimationBuilder(
            duration: Duration(milliseconds: 800),
            tween: Tween<double>(begin: 0.0, end: 1.0),
            builder: (context, double value, child) {
              return Transform.scale(
                scale: value,
                child: Container(
                  width: 120.w,
                  height: 120.h,
                  decoration: BoxDecoration(
                    gradient: LinearGradient(colors: gradient),
                    shape: BoxShape.circle,
                    boxShadow: [
                      BoxShadow(
                        color: gradient[0].withOpacity(0.4),
                        blurRadius: 30.r,
                        offset: Offset(0, 15.h),
                      ),
                    ],
                  ),
                  child: Icon(
                    icon,
                    color: AppColors.white,
                    size: 60.sp,
                  ),
                ),
              );
            },
          ),
          
          SizedBox(height: 60.h),
          
          // Title
          Text(
            title,
            style: TextStyle(
              fontSize: 32.sp,
              fontWeight: FontWeight.bold,
              color: AppColors.textLight,
            ),
            textAlign: TextAlign.center,
          ),
          
          SizedBox(height: 16.h),
          
          // Subtitle
          Text(
            subtitle,
            style: TextStyle(
              fontSize: 18.sp,
              fontWeight: FontWeight.w600,
              color: gradient[0],
            ),
            textAlign: TextAlign.center,
          ),
          
          SizedBox(height: 24.h),
          
          // Description
          Text(
            description,
            style: TextStyle(
              fontSize: 16.sp,
              color: AppColors.textMedium,
              height: 1.5,
            ),
            textAlign: TextAlign.center,
          ),
          
          if (isLastPage) ...[
            SizedBox(height: 40.h),
            
            // Motivational quote
            Container(
              padding: EdgeInsets.all(20.w),
              decoration: BoxDecoration(
                color: AppColors.cardBackground,
                borderRadius: BorderRadius.circular(16.r),
                border: Border.all(
                  color: gradient[0].withOpacity(0.3),
                ),
              ),
              child: Column(
                children: [
                  Icon(
                    Icons.format_quote,
                    color: gradient[0],
                    size: 24.sp,
                  ),
                  SizedBox(height: 8.h),
                  Text(
                    '"The universe is not outside of you. Look inside yourself; everything that you want, you already are."',
                    style: TextStyle(
                      fontSize: 14.sp,
                      color: AppColors.textMedium,
                      fontStyle: FontStyle.italic,
                      height: 1.4,
                    ),
                    textAlign: TextAlign.center,
                  ),
                  SizedBox(height: 8.h),
                  Text(
                    '- Rumi',
                    style: TextStyle(
                      fontSize: 12.sp,
                      color: gradient[0],
                      fontWeight: FontWeight.w600,
                    ),
                  ),
                ],
              ),
            ),
          ],
        ],
      ),
    );
  }

  Widget _buildNavigationControls() {
    return Padding(
      padding: EdgeInsets.symmetric(horizontal: 24.w),
      child: Row(
        children: [
          // Previous button
          if (_currentPage > 0)
            Expanded(
              child: Container(
                height: 56.h,
                child: ElevatedButton(
                  onPressed: _previousPage,
                  style: ElevatedButton.styleFrom(
                    backgroundColor: AppColors.cardBackground,
                    shadowColor: Colors.transparent,
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(16.r),
                    ),
                    side: BorderSide(
                      color: AppColors.textMedium.withOpacity(0.2),
                      width: 1,
                    ),
                  ),
                  child: FittedBox(
                    fit: BoxFit.scaleDown,
                    child: Text(
                      'Previous',
                      style: TextStyle(
                        fontSize: 16.sp,
                        fontWeight: FontWeight.w600,
                        color: AppColors.textLight,
                      ),
                      maxLines: 1,
                      overflow: TextOverflow.ellipsis,
                    ),
                  ),
                ),
              ),
            ),
          
          if (_currentPage > 0) SizedBox(width: 16.w),
          
          // Next/Get Started button
          Expanded(
            flex: _currentPage > 0 ? 1 : 1,
            child: Container(
              height: 56.h,
              child: ElevatedButton(
                onPressed: _nextPage,
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
                      begin: Alignment.topLeft,
                      end: Alignment.bottomRight,
                    ),
                    borderRadius: BorderRadius.circular(16.r),
                    boxShadow: [
                      BoxShadow(
                        color: AppColors.affirmationGradient[0].withOpacity(0.3),
                        blurRadius: 12.r,
                        offset: Offset(0, 6.h),
                      ),
                    ],
                  ),
                  child: Container(
                    alignment: Alignment.center,
                    child: FittedBox(
                      fit: BoxFit.scaleDown,
                      child: Text(
                        _currentPage == _totalPages - 1 ? 'Get Started!' : 'Next',
                        style: TextStyle(
                          fontSize: 16.sp,
                          fontWeight: FontWeight.w600,
                          color: AppColors.white,
                        ),
                        maxLines: 1,
                        overflow: TextOverflow.ellipsis,
                      ),
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
}