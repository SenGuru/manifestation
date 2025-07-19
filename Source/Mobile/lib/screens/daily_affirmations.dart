import 'dart:async';

import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../models/affirmations.dart';
import 'favorites_screen.dart';
import '../constants/colors.dart';

class DailyAffirmationsScreen extends StatefulWidget {
  @override
  _DailyAffirmationsScreenState createState() => _DailyAffirmationsScreenState();
}

class _DailyAffirmationsScreenState extends State<DailyAffirmationsScreen> {
  int selectedCategoryIndex = 0;
  int currentAffirmationIndex = 0;
  bool isPlaying = false;
  bool isLoading = true;
  
  List<AffirmationCategory> categories = [];
  
  // Session tracking
  String? _sessionTracker;

  @override
  void initState() {
    super.initState();
    _loadCategories();
    _initializeTTS();
    _startSessionTracking();
  }

  Future<void> _initializeTTS() async {
    try {
      print('TTS functionality disabled in offline mode');
    } catch (e) {
      print('TTS initialization skipped: $e');
    }
  }

  void _startSessionTracking() {
    _sessionTracker = '${DateTime.now().millisecondsSinceEpoch}';
    print('Session tracking started with ID: $_sessionTracker');
  }

  Future<void> _loadCategories() async {
    setState(() {
      isLoading = true;
    });
    
    try {
      print('=== LOADING MOCK AFFIRMATIONS ===');
      
      // Load mock default content
      await _loadDefaultContent();
      
      print('Total categories loaded: ${categories.length}');
      categories.forEach((category) {
        print('Category: ${category.name}, Affirmations: ${category.affirmations.length}');
      });
      
    } catch (e) {
      print('Error loading categories: $e');
      
      // Fallback to default content
      await _loadDefaultContent();
    } finally {
      setState(() {
        isLoading = false;
      });
    }
  }

  Future<void> _loadDefaultContent() async {
    try {
      print('Loading mock default content...');
      
      // Create mock categories with affirmations
      final mockCategories = [
        AffirmationCategory(
          id: '1',
          name: 'Self-Love',
          color: AppColors.affirmationGradient[0],
          affirmations: [
            Affirmation(
              id: '1',
              text: 'I am worthy of love and respect',
              isFavorite: false,
              isCustom: false,
            ),
            Affirmation(
              id: '2',
              text: 'I accept myself completely',
              isFavorite: false,
              isCustom: false,
            ),
            Affirmation(
              id: '3',
              text: 'I choose to love myself unconditionally',
              isFavorite: false,
              isCustom: false,
            ),
          ],
        ),
        AffirmationCategory(
          id: '2',
          name: 'Success',
          color: AppColors.goalTrackerGradient[0],
          affirmations: [
            Affirmation(
              id: '4',
              text: 'I am capable of achieving my dreams',
              isFavorite: false,
              isCustom: false,
            ),
            Affirmation(
              id: '5',
              text: 'Success flows to me naturally',
              isFavorite: false,
              isCustom: false,
            ),
            Affirmation(
              id: '6',
              text: 'I am grateful for my achievements',
              isFavorite: false,
              isCustom: false,
            ),
          ],
        ),
        AffirmationCategory(
          id: '3',
          name: 'Abundance',
          color: AppColors.accentGold,
          affirmations: [
            Affirmation(
              id: '7',
              text: 'I am open to receiving abundance',
              isFavorite: false,
              isCustom: false,
            ),
            Affirmation(
              id: '8',
              text: 'Prosperity flows to me easily',
              isFavorite: false,
              isCustom: false,
            ),
            Affirmation(
              id: '9',
              text: 'I attract opportunities for growth',
              isFavorite: false,
              isCustom: false,
            ),
          ],
        ),
      ];
      
      setState(() {
        categories = mockCategories;
      });
      
      print('Mock categories loaded successfully');
      
    } catch (e) {
      print('Error loading default content: $e');
    }
  }

  @override
  void dispose() {
    // TTS cleanup removed
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.backgroundDark,
      body: SafeArea(
        child: Column(
          children: [
            _buildHeader(),
            _buildCategoryChips(),
            if (isLoading) _buildLoadingState() else _buildAffirmationContent(),
          ],
        ),
      ),
    );
  }

  Widget _buildHeader() {
    return Container(
      padding: EdgeInsets.all(20.w),
      child: Row(
        children: [
          GestureDetector(
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
          ),
          SizedBox(width: 16.w),
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  'Daily Affirmations',
                  style: TextStyle(
                    fontSize: 24.sp,
                    fontWeight: FontWeight.bold,
                    color: AppColors.textLight,
                  ),
                ),
                Text(
                  'Transform your mindset with positive thoughts',
                  style: TextStyle(
                    fontSize: 14.sp,
                    color: AppColors.textMedium,
                  ),
                ),
              ],
            ),
          ),
          GestureDetector(
            onTap: () => Navigator.push(
              context,
              MaterialPageRoute(builder: (context) => FavoritesScreen()),
            ),
            child: Container(
              width: 44.w,
              height: 44.h,
              decoration: BoxDecoration(
                color: AppColors.cardBackground,
                borderRadius: BorderRadius.circular(12.r),
              ),
              child: Icon(
                Icons.favorite,
                color: AppColors.accentCoral,
                size: 20.sp,
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildCategoryChips() {
    return Container(
      height: 60.h,
      child: ListView.builder(
        scrollDirection: Axis.horizontal,
        padding: EdgeInsets.symmetric(horizontal: 20.w),
        itemCount: categories.length,
        itemBuilder: (context, index) {
          final category = categories[index];
          final isSelected = selectedCategoryIndex == index;
          
          return GestureDetector(
            onTap: () {
              setState(() {
                selectedCategoryIndex = index;
                currentAffirmationIndex = 0;
                isPlaying = false;
              });
            },
            child: Container(
              margin: EdgeInsets.only(right: 12.w),
              padding: EdgeInsets.symmetric(horizontal: 20.w, vertical: 12.h),
              decoration: BoxDecoration(
                color: isSelected 
                    ? category.color.withOpacity(0.1)
                    : AppColors.cardBackground,
                borderRadius: BorderRadius.circular(25.r),
                border: Border.all(
                  color: isSelected ? category.color : AppColors.textMedium.withOpacity(0.3),
                ),
              ),
              child: Text(
                category.name,
                style: TextStyle(
                  fontSize: 14.sp,
                  fontWeight: isSelected ? FontWeight.w600 : FontWeight.w500,
                  color: isSelected ? category.color : AppColors.textMedium,
                ),
              ),
            ),
          );
        },
      ),
    );
  }

  Widget _buildLoadingState() {
    return Expanded(
      child: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            CircularProgressIndicator(
              valueColor: AlwaysStoppedAnimation<Color>(AppColors.affirmationGradient[0]),
            ),
            SizedBox(height: 16.h),
            Text(
              'Loading affirmations...',
              style: TextStyle(
                fontSize: 16.sp,
                color: AppColors.textMedium,
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildAffirmationContent() {
    if (categories.isEmpty) {
      return Expanded(
        child: Center(
          child: Text(
            'No affirmations available',
            style: TextStyle(
              fontSize: 16.sp,
              color: AppColors.textMedium,
            ),
          ),
        ),
      );
    }

    final currentCategory = categories[selectedCategoryIndex];
    if (currentCategory.affirmations.isEmpty) {
      return Expanded(
        child: Center(
          child: Text(
            'No affirmations in this category',
            style: TextStyle(
              fontSize: 16.sp,
              color: AppColors.textMedium,
            ),
          ),
        ),
      );
    }

    final currentAffirmation = currentCategory.affirmations[currentAffirmationIndex];

    return Expanded(
      child: Container(
        margin: EdgeInsets.all(20.w),
        padding: EdgeInsets.all(24.w),
        decoration: BoxDecoration(
          gradient: LinearGradient(
            colors: AppColors.affirmationGradient,
            begin: Alignment.topLeft,
            end: Alignment.bottomRight,
          ),
          borderRadius: BorderRadius.circular(24.r),
        ),
        child: Column(
          children: [
            Expanded(
              child: Center(
                child: Text(
                  currentAffirmation.text,
                  style: TextStyle(
                    fontSize: 24.sp,
                    fontWeight: FontWeight.w600,
                    color: AppColors.white,
                    height: 1.4,
                  ),
                  textAlign: TextAlign.center,
                ),
              ),
            ),
            SizedBox(height: 24.h),
            _buildControls(),
          ],
        ),
      ),
    );
  }

  Widget _buildControls() {
    final currentCategory = categories[selectedCategoryIndex];
    
    return Column(
      children: [
        // Navigation dots
        Row(
          mainAxisAlignment: MainAxisAlignment.center,
          children: List.generate(
            currentCategory.affirmations.length,
            (index) => Container(
              margin: EdgeInsets.symmetric(horizontal: 4.w),
              width: 8.w,
              height: 8.h,
              decoration: BoxDecoration(
                shape: BoxShape.circle,
                color: index == currentAffirmationIndex 
                    ? AppColors.white 
                    : AppColors.white.withOpacity(0.3),
              ),
            ),
          ),
        ),
        SizedBox(height: 24.h),
        // Control buttons
        Row(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            _buildControlButton(
              icon: Icons.skip_previous,
              onTap: _previousAffirmation,
              enabled: currentAffirmationIndex > 0,
            ),
            SizedBox(width: 20.w),
            _buildPlayPauseButton(),
            SizedBox(width: 20.w),
            _buildControlButton(
              icon: Icons.skip_next,
              onTap: _nextAffirmation,
              enabled: currentAffirmationIndex < currentCategory.affirmations.length - 1,
            ),
          ],
        ),
      ],
    );
  }

  Widget _buildControlButton({
    required IconData icon,
    required VoidCallback onTap,
    required bool enabled,
  }) {
    return GestureDetector(
      onTap: enabled ? onTap : null,
      child: Container(
        width: 48.w,
        height: 48.h,
        decoration: BoxDecoration(
          color: AppColors.white.withOpacity(enabled ? 0.2 : 0.1),
          shape: BoxShape.circle,
        ),
        child: Icon(
          icon,
          color: AppColors.white.withOpacity(enabled ? 1.0 : 0.3),
          size: 24.sp,
        ),
      ),
    );
  }

  Widget _buildPlayPauseButton() {
    return GestureDetector(
      onTap: _togglePlayPause,
      child: Container(
        width: 64.w,
        height: 64.h,
        decoration: BoxDecoration(
          color: AppColors.white,
          shape: BoxShape.circle,
        ),
        child: Icon(
          isPlaying ? Icons.pause : Icons.play_arrow,
          color: AppColors.affirmationGradient[0],
          size: 32.sp,
        ),
      ),
    );
  }

  void _togglePlayPause() async {
    if (_sessionTracker == null) {
      print('No session tracker available');
      return;
    }
    
    final currentCategory = categories[selectedCategoryIndex];
    final currentAffirmation = currentCategory.affirmations[currentAffirmationIndex];
    
    if (isPlaying) {
      setState(() {
        isPlaying = false;
      });
      print('Paused affirmation: ${currentAffirmation.text}');
    } else {
      setState(() {
        isPlaying = true;
      });
      
      // Mock TTS playback with timer
      print('Mock playing affirmation: ${currentAffirmation.text}');
      Timer(Duration(seconds: 5), () {
        if (mounted) {
          setState(() {
            isPlaying = false;
          });
        }
      });
    }
  }

  void _nextAffirmation() {
    final currentCategory = categories[selectedCategoryIndex];
    if (currentAffirmationIndex < currentCategory.affirmations.length - 1) {
      setState(() {
        currentAffirmationIndex++;
        isPlaying = false;
      });
    }
  }

  void _previousAffirmation() {
    if (currentAffirmationIndex > 0) {
      setState(() {
        currentAffirmationIndex--;
        isPlaying = false;
      });
    }
  }
}