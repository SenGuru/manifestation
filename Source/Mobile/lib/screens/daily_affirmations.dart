import 'dart:async';

import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:flutter_tts/flutter_tts.dart';
import '../models/affirmations.dart';
import '../constants/colors.dart';
import '../constants/category_icons.dart';
import '../utils/color_utils.dart';
import '../services/daily_affirmations_service.dart';
import '../services/usage_analytics_service.dart';
import '../services/guided_tour_service.dart';
import '../models/analytics_models.dart';

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
  List<AffirmationCategoryData> apiCategories = [];
  
  // Services
  final DailyAffirmationsService _affirmationService = DailyAffirmationsService();
  final UsageAnalyticsService _analyticsService = UsageAnalyticsService();
  final FlutterTts _flutterTts = FlutterTts();
  
  // Session tracking
  String? _sessionTracker;
  DateTime? _sessionStartTime;
  bool _sessionStarted = false;

  @override
  void initState() {
    super.initState();
    _initializeService();
    _loadCategories();
    _initializeTTS();
    _startSessionTracking();
    _startAnalyticsSession();
    
    // Show guided tour after the widget is built
    WidgetsBinding.instance.addPostFrameCallback((_) {
      _showGuidedTourIfNeeded();
    });
  }

  Future<void> _showGuidedTourIfNeeded() async {
    final tourService = GuidedTourService();
    await tourService.showTourIfNeeded(
      context,
      GuidedTourService.affirmationsTourKey,
      tourService.getAffirmationsTourSteps(),
    );
  }
  
  void _initializeService() {
    _affirmationService.initialize(baseUrl: 'http://manifestation-qa-1.senthilguru.net');
  }

  Future<void> _initializeTTS() async {
    try {
      await _flutterTts.setLanguage('en-US');
      await _flutterTts.setSpeechRate(0.5);
      await _flutterTts.setVolume(0.8);
      await _flutterTts.setPitch(1.0);
      
      _flutterTts.setStartHandler(() {
        if (mounted) {
          setState(() {
            isPlaying = true;
          });
        }
      });

      _flutterTts.setCompletionHandler(() {
        if (mounted) {
          setState(() {
            isPlaying = false;
          });
        }
      });

      _flutterTts.setErrorHandler((message) {
        print('TTS Error: $message');
        if (mounted) {
          setState(() {
            isPlaying = false;
          });
        }
      });

      print('✅ TTS initialized successfully');
    } catch (e) {
      print('❌ TTS initialization failed: $e');
    }
  }

  void _startSessionTracking() {
    _sessionTracker = '${DateTime.now().millisecondsSinceEpoch}';
    _sessionStartTime = DateTime.now();
    print('Session tracking started with ID: $_sessionTracker');
  }

  Future<void> _loadCategories() async {
    setState(() {
      isLoading = true;
    });
    
    try {
      print('=== LOADING AFFIRMATIONS FROM API ===');
      
      // Load from API first, fallback to mock if needed
      final result = await _affirmationService.getAllAffirmationCategories();
      
      if (result.isSuccess && result.data != null) {
        print('✅ Loaded ${result.data!.length} categories from API');
        await _processApiData(result.data!);
      } else {
        print('❌ API failed: ${result.message}');
        print('📦 Falling back to mock data');
        await _loadDefaultContent();
      }
      
      print('Total categories loaded: ${categories.length}');
      categories.forEach((category) {
        print('Category: ${category.name}, Affirmations: ${category.affirmations.length}');
      });
      
    } catch (e) {
      print('❌ Error loading categories: $e');
      print('📦 Falling back to mock data');
      
      // Fallback to default content
      await _loadDefaultContent();
    } finally {
      setState(() {
        isLoading = false;
      });
    }
  }

  Future<void> _processApiData(List<AffirmationCategoryData> apiData) async {
    try {
      apiCategories = apiData;
      
      // Convert AffirmationCategoryData to AffirmationCategory objects for UI compatibility
      final uiCategories = <AffirmationCategory>[];
      
      for (int i = 0; i < apiCategories.length; i++) {
        final categoryData = apiCategories[i];
        
        // Convert AffirmationItem to Affirmation objects
        final affirmations = categoryData.affirmations.map((item) => Affirmation(
          id: item.id,
          text: item.text,
          isFavorite: item.isFavorite,
          isCustom: !item.isDefault,
        )).toList();
        
        // Use the actual color from the category data, or fallback to generated color
        final categoryColor = categoryData.color ?? _getCategoryColor(i);
        
        uiCategories.add(AffirmationCategory(
          id: categoryData.id,
          name: categoryData.name,
          color: categoryColor,
          iconValue: categoryData.iconId,
          affirmations: affirmations,
        ));
      }
      
      setState(() {
        categories = uiCategories;
      });
      
    } catch (e) {
      print('Error processing API data: $e');
      throw e;
    }
  }

  Color _getCategoryColor(int index) {
    final colors = [
      AppColors.affirmationGradient[0],
      AppColors.goalTrackerGradient[0],
      AppColors.accentGold,
      AppColors.accentCoral,
      AppColors.visionBoardGradient[0],
      AppColors.manifestationGradient[0],
    ];
    return colors[index % colors.length];
  }

  Future<void> _loadDefaultContent() async {
    try {
      print('Loading mock default content...');
      
      // Create mock categories with affirmations
      final mockCategories = [
        AffirmationCategory(
          id: '1',
          name: 'Self-Love',
          color: const Color(0xFFE91E63), // Pink
          iconValue: 3600002, // Heart
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
          color: const Color(0xFFFF9800), // Orange
          iconValue: 3600008, // Trophy
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
          color: const Color(0xFF4CAF50), // Green
          iconValue: 3600006, // Diamond
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
        AffirmationCategory(
          id: '4',
          name: 'Health',
          color: const Color(0xFF009688), // Teal
          iconValue: 3600004, // Leaf
          affirmations: [
            Affirmation(
              id: '10',
              text: 'My body is strong and healthy',
              isFavorite: false,
              isCustom: false,
            ),
            Affirmation(
              id: '11',
              text: 'I nourish my body with love and care',
              isFavorite: false,
              isCustom: false,
            ),
            Affirmation(
              id: '12',
              text: 'I am grateful for my vibrant health',
              isFavorite: false,
              isCustom: false,
            ),
          ],
        ),
        AffirmationCategory(
          id: '5',
          name: 'Confidence',
          color: const Color(0xFF3F51B5), // Indigo
          iconValue: 3600005, // Star
          affirmations: [
            Affirmation(
              id: '13',
              text: 'I believe in my abilities',
              isFavorite: false,
              isCustom: false,
            ),
            Affirmation(
              id: '14',
              text: 'I am confident in who I am',
              isFavorite: false,
              isCustom: false,
            ),
            Affirmation(
              id: '15',
              text: 'I trust myself to make good decisions',
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
    // Record session data before disposing
    _recordSessionData();
    // End analytics session
    _endAnalyticsSession();
    // Stop and dispose TTS
    _flutterTts.stop();
    super.dispose();
  }

  Future<void> _recordSessionData() async {
    if (_sessionStartTime == null) return;
    
    try {
      final timeSpentInSeconds = DateTime.now().difference(_sessionStartTime!).inSeconds;
      
      if (timeSpentInSeconds > 5) { // Only record sessions longer than 5 seconds
        print('🎯 Recording affirmation session: ${timeSpentInSeconds}s');
        
        final result = await _affirmationService.createAffirmationSession(
          timeSpentInSeconds: timeSpentInSeconds,
          addToFeed: false,
        );
        
        if (result.isSuccess) {
          print('✅ Session recorded successfully');
        } else {
          print('❌ Failed to record session: ${result.message}');
        }
      }
    } catch (e) {
      print('❌ Error recording session: $e');
    }
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
            onTap: _showAddCategoryDialog,
            child: Container(
              width: 44.w,
              height: 44.h,
              decoration: BoxDecoration(
                color: AppColors.cardBackground,
                borderRadius: BorderRadius.circular(12.r),
              ),
              child: Icon(
                Icons.add_circle_outline,
                color: AppColors.accentGold,
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
      height: 50.h,
      child: ListView.builder(
        scrollDirection: Axis.horizontal,
        padding: EdgeInsets.symmetric(horizontal: 20.w),
        itemCount: categories.length,
        itemBuilder: (context, index) {
          final category = categories[index];
          final isSelected = selectedCategoryIndex == index;
          
          return GestureDetector(
            onTap: () async {
              await _flutterTts.stop();
              setState(() {
                selectedCategoryIndex = index;
                currentAffirmationIndex = 0;
                isPlaying = false;
              });
            },
            onLongPress: () => _showCategoryOptions(category, index),
            child: Container(
              margin: EdgeInsets.only(right: 12.w),
              padding: EdgeInsets.symmetric(horizontal: 20.w, vertical: 8.h),
              decoration: BoxDecoration(
                color: isSelected 
                    ? category.color.withAlpha((0.1 * 255).round())
                    : AppColors.cardBackground,
                borderRadius: BorderRadius.circular(25.r),
                border: Border.all(
                  color: isSelected ? category.color : AppColors.textMedium.withAlpha((0.3 * 255).round()),
                ),
              ),
              child: Center(
                child: Row(
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    Icon(
                      category.iconData,
                      color: isSelected ? category.color : AppColors.textMedium,
                      size: 16.sp,
                    ),
                    SizedBox(width: 6.w),
                    Text(
                      category.name,
                      style: TextStyle(
                        fontSize: 14.sp,
                        fontWeight: isSelected ? FontWeight.w600 : FontWeight.w500,
                        color: isSelected ? category.color : AppColors.textMedium,
                      ),
                    ),
                  ],
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
        child: _buildEmptyAffirmationsState(currentCategory),
      );
    }

    final currentAffirmation = currentCategory.affirmations[currentAffirmationIndex];

    return Expanded(
      child: Container(
        margin: EdgeInsets.all(20.w),
        padding: EdgeInsets.all(24.w),
        decoration: BoxDecoration(
          gradient: LinearGradient(
            colors: [
              currentCategory.color.withAlpha((0.1 * 255).round()),
              currentCategory.color.withAlpha((0.05 * 255).round()),
            ],
            begin: Alignment.topCenter,
            end: Alignment.bottomCenter,
          ),
          borderRadius: BorderRadius.circular(20.r),
          border: Border.all(
            color: currentCategory.color.withAlpha((0.3 * 255).round()),
            width: 1,
          ),
        ),
        child: Column(
          children: [
            // Header with affirmation actions for custom affirmations
            if (!currentAffirmation.isCustom) 
              SizedBox(height: 16.h)
            else
              _buildAffirmationActions(),
            
            Expanded(
              child: Center(
                child: Text(
                  currentAffirmation.text,
                  style: TextStyle(
                    fontSize: 24.sp,
                    fontWeight: FontWeight.w600,
                    color: AppColors.textLight,
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

  Widget _buildAffirmationActions() {
    return Padding(
      padding: EdgeInsets.only(bottom: 8.h),
      child: Row(
        mainAxisAlignment: MainAxisAlignment.end,
        children: [
          // Edit button
          GestureDetector(
            onTap: _editCurrentAffirmation,
            child: Container(
              width: 36.w,
              height: 36.h,
              decoration: BoxDecoration(
                color: AppColors.cardBackground,
                shape: BoxShape.circle,
                border: Border.all(
                  color: AppColors.textMedium.withAlpha((0.3 * 255).round()),
                ),
              ),
              child: Icon(
                Icons.edit,
                color: AppColors.textLight,
                size: 18.sp,
              ),
            ),
          ),
          SizedBox(width: 12.w),
          // Delete button
          GestureDetector(
            onTap: _deleteCurrentAffirmation,
            child: Container(
              width: 36.w,
              height: 36.h,
              decoration: BoxDecoration(
                color: AppColors.cardBackground,
                shape: BoxShape.circle,
                border: Border.all(
                  color: AppColors.error.withAlpha((0.3 * 255).round()),
                ),
              ),
              child: Icon(
                Icons.delete_outline,
                color: AppColors.error,
                size: 18.sp,
              ),
            ),
          ),
        ],
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
                    ? currentCategory.color 
                    : AppColors.textMedium.withAlpha((0.3 * 255).round()),
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
        SizedBox(height: 16.h),
        // Add new affirmation button
        GestureDetector(
          onTap: () => _showAddAffirmationDialog(currentCategory),
          child: Container(
            padding: EdgeInsets.symmetric(horizontal: 20.w, vertical: 12.h),
            decoration: BoxDecoration(
              color: currentCategory.color.withAlpha((0.1 * 255).round()),
              borderRadius: BorderRadius.circular(24.r),
              border: Border.all(
                color: currentCategory.color.withAlpha((0.3 * 255).round()),
                width: 1.5,
              ),
            ),
            child: Row(
              mainAxisSize: MainAxisSize.min,
              children: [
                Icon(
                  Icons.add,
                  size: 18.sp,
                  color: currentCategory.color,
                ),
                SizedBox(width: 8.w),
                Text(
                  'Add New Affirmation',
                  style: TextStyle(
                    fontSize: 14.sp,
                    fontWeight: FontWeight.w600,
                    color: currentCategory.color,
                  ),
                ),
              ],
            ),
          ),
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
          color: AppColors.cardBackground,
          shape: BoxShape.circle,
          border: Border.all(
            color: enabled 
                ? AppColors.textMedium.withAlpha((0.3 * 255).round())
                : AppColors.textMedium.withAlpha((0.1 * 255).round()),
          ),
        ),
        child: Icon(
          icon,
          color: enabled ? AppColors.textLight : AppColors.textMedium.withAlpha((0.5 * 255).round()),
          size: 24.sp,
        ),
      ),
    );
  }

  Widget _buildPlayPauseButton() {
    final currentCategory = categories[selectedCategoryIndex];
    
    return GestureDetector(
      onTap: _togglePlayPause,
      child: Container(
        width: 64.w,
        height: 64.h,
        decoration: BoxDecoration(
          shape: BoxShape.circle,
          gradient: LinearGradient(
            colors: [
              currentCategory.color,
              currentCategory.color.withAlpha((0.8 * 255).round()),
            ],
          ),
          boxShadow: [
            BoxShadow(
              color: currentCategory.color.withAlpha((0.4 * 255).round()),
              blurRadius: 12.r,
              offset: Offset(0, 4.h),
            ),
          ],
        ),
        child: Icon(
          isPlaying ? Icons.pause : Icons.play_arrow,
          color: AppColors.white,
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
    
    try {
      if (isPlaying) {
        await _flutterTts.stop();
        setState(() {
          isPlaying = false;
        });
        print('Paused affirmation: ${currentAffirmation.text}');
      } else {
        print('🔊 Playing affirmation: ${currentAffirmation.text}');
        await _flutterTts.speak(currentAffirmation.text);
      }
    } catch (e) {
      print('❌ TTS Error: $e');
      setState(() {
        isPlaying = false;
      });
      _showErrorSnackBar('Unable to play audio. Please check your device settings.');
    }
  }

  void _nextAffirmation() async {
    final currentCategory = categories[selectedCategoryIndex];
    if (currentAffirmationIndex < currentCategory.affirmations.length - 1) {
      await _flutterTts.stop();
      setState(() {
        currentAffirmationIndex++;
        isPlaying = false;
      });
    }
  }

  void _previousAffirmation() async {
    if (currentAffirmationIndex > 0) {
      await _flutterTts.stop();
      setState(() {
        currentAffirmationIndex--;
        isPlaying = false;
      });
    }
  }

  void _editCurrentAffirmation() {
    final currentCategory = categories[selectedCategoryIndex];
    final currentAffirmation = currentCategory.affirmations[currentAffirmationIndex];
    
    if (!currentAffirmation.isCustom) {
      _showErrorSnackBar('Cannot edit default affirmations');
      return;
    }

    _showEditAffirmationDialog(currentAffirmation);
  }

  void _deleteCurrentAffirmation() async {
    final currentCategory = categories[selectedCategoryIndex];
    final currentAffirmation = currentCategory.affirmations[currentAffirmationIndex];
    
    if (!currentAffirmation.isCustom) {
      _showErrorSnackBar('Cannot delete default affirmations');
      return;
    }

    // Show confirmation dialog
    final shouldDelete = await showDialog<bool>(
      context: context,
      builder: (BuildContext context) {
        return AlertDialog(
          backgroundColor: AppColors.cardBackground,
          title: Text(
            'Delete Affirmation',
            style: TextStyle(color: AppColors.textLight),
          ),
          content: Text(
            'Are you sure you want to delete this affirmation?',
            style: TextStyle(color: AppColors.textMedium),
          ),
          actions: [
            TextButton(
              onPressed: () => Navigator.of(context).pop(false),
              child: Text('Cancel', style: TextStyle(color: AppColors.textMedium)),
            ),
            TextButton(
              onPressed: () => Navigator.of(context).pop(true),
              child: Text('Delete', style: TextStyle(color: AppColors.error)),
            ),
          ],
        );
      },
    );

    if (shouldDelete == true) {
      try {
        // Call the API to delete the affirmation
        final affirmationId = int.tryParse(currentAffirmation.id);
        if (affirmationId != null) {
          final result = await _affirmationService.deleteAffirmationText(affirmationId);
          
          if (result.isSuccess) {
            _showInfoSnackBar('Affirmation deleted successfully');
            // Reload categories to refresh the UI
            await _loadCategories();
          } else {
            _showErrorSnackBar('Failed to delete affirmation: ${result.message}');
          }
        }
      } catch (e) {
        _showErrorSnackBar('Error deleting affirmation: $e');
      }
    }
  }

  void _showEditAffirmationDialog(Affirmation affirmation) {
    final TextEditingController textController = TextEditingController(text: affirmation.text);
    
    showDialog(
      context: context,
      builder: (BuildContext context) {
        return AlertDialog(
          backgroundColor: AppColors.cardBackground,
          title: Text(
            'Edit Affirmation',
            style: TextStyle(color: AppColors.textLight),
          ),
          content: TextField(
            controller: textController,
            style: TextStyle(color: AppColors.textLight),
            maxLines: 3,
            decoration: InputDecoration(
              hintText: 'Enter your affirmation...',
              hintStyle: TextStyle(color: AppColors.textMedium),
              border: OutlineInputBorder(
                borderRadius: BorderRadius.circular(12.r),
                borderSide: BorderSide(color: AppColors.textMedium.withAlpha((0.3 * 255).round())),
              ),
              enabledBorder: OutlineInputBorder(
                borderRadius: BorderRadius.circular(12.r),
                borderSide: BorderSide(color: AppColors.textMedium.withAlpha((0.3 * 255).round())),
              ),
              focusedBorder: OutlineInputBorder(
                borderRadius: BorderRadius.circular(12.r),
                borderSide: BorderSide(color: AppColors.affirmationGradient[0]),
              ),
            ),
          ),
          actions: [
            TextButton(
              onPressed: () => Navigator.of(context).pop(),
              child: Text('Cancel', style: TextStyle(color: AppColors.textMedium)),
            ),
            TextButton(
              onPressed: () async {
                final newText = textController.text.trim();
                if (newText.isNotEmpty) {
                  Navigator.of(context).pop();
                  await _updateAffirmation(affirmation, newText);
                }
              },
              child: Text('Save', style: TextStyle(color: AppColors.affirmationGradient[0])),
            ),
          ],
        );
      },
    );
  }

  Future<void> _updateAffirmation(Affirmation affirmation, String newText) async {
    try {
      final affirmationId = int.tryParse(affirmation.id);
      if (affirmationId != null) {
        final result = await _affirmationService.updateAffirmationText(
          affirmationTextId: affirmationId,
          text: newText,
        );
        
        if (result.isSuccess) {
          _showInfoSnackBar('Affirmation updated successfully');
          // Reload categories to refresh the UI
          await _loadCategories();
        } else {
          _showErrorSnackBar('Failed to update affirmation: ${result.message}');
        }
      }
    } catch (e) {
      _showErrorSnackBar('Error updating affirmation: $e');
    }
  }

  void _showInfoSnackBar(String message) {
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text(message),
        backgroundColor: AppColors.success,
        behavior: SnackBarBehavior.floating,
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12.r)),
      ),
    );
  }

  void _showErrorSnackBar(String message) {
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text(message),
        backgroundColor: AppColors.error,
        behavior: SnackBarBehavior.floating,
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12.r)),
      ),
    );
  }

  /// Start analytics session tracking
  Future<void> _startAnalyticsSession() async {
    try {
      await _analyticsService.initialize();
      _analyticsService.startSession(
        ActivityType.dailyAffirmations,
        metadata: {
          'totalCategories': categories.length,
          'selectedCategory': selectedCategoryIndex,
        },
      );
      _sessionStarted = true;
      print('📊 Daily affirmations analytics session started');
    } catch (e) {
      print('❌ Error starting affirmations analytics session: $e');
    }
  }

  /// End analytics session tracking
  Future<void> _endAnalyticsSession() async {
    if (_sessionStarted) {
      try {
        await _analyticsService.endSession(
          ActivityType.dailyAffirmations,
          additionalMetadata: {
            'categoriesViewed': 1,
            'finalCategory': categories.isNotEmpty ? categories[selectedCategoryIndex].name : 'unknown',
            'affirmationsViewed': currentAffirmationIndex + 1,
          },
        );
        _sessionStarted = false;
        print('📊 Daily affirmations analytics session ended');
      } catch (e) {
        print('❌ Error ending affirmations analytics session: $e');
      }
    }
  }

  void _showCategoryOptions(AffirmationCategory category, int categoryIndex) {
    // Only allow editing/deleting custom categories (not default ones)
    final isCustomCategory = categoryIndex >= 5; // First 5 are default categories
    
    if (!isCustomCategory) {
      _showErrorSnackBar('Cannot modify default categories');
      return;
    }

    showModalBottomSheet(
      context: context,
      backgroundColor: AppColors.cardBackground,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.vertical(top: Radius.circular(20.r)),
      ),
      builder: (BuildContext context) {
        return Container(
          padding: EdgeInsets.all(20.w),
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              // Handle bar
              Container(
                width: 40.w,
                height: 4.h,
                decoration: BoxDecoration(
                  color: AppColors.textMedium.withAlpha((0.3 * 255).round()),
                  borderRadius: BorderRadius.circular(2.r),
                ),
              ),
              SizedBox(height: 20.h),
              
              // Category name
              Text(
                category.name,
                style: TextStyle(
                  fontSize: 18.sp,
                  fontWeight: FontWeight.bold,
                  color: AppColors.textLight,
                ),
              ),
              SizedBox(height: 20.h),
              
              // Edit option
              ListTile(
                leading: Icon(Icons.edit, color: AppColors.textLight),
                title: Text(
                  'Edit Category',
                  style: TextStyle(color: AppColors.textLight),
                ),
                onTap: () {
                  Navigator.pop(context);
                  _showEditCategoryDialog(category);
                },
              ),
              
              // Delete option
              ListTile(
                leading: Icon(Icons.delete, color: AppColors.error),
                title: Text(
                  'Delete Category',
                  style: TextStyle(color: AppColors.error),
                ),
                onTap: () {
                  Navigator.pop(context);
                  _showDeleteCategoryDialog(category, categoryIndex);
                },
              ),
              
              SizedBox(height: 20.h),
            ],
          ),
        );
      },
    );
  }

  void _showEditCategoryDialog(AffirmationCategory category) {
    final TextEditingController nameController = TextEditingController(text: category.name);
    
    showDialog(
      context: context,
      builder: (BuildContext context) {
        return AlertDialog(
          backgroundColor: AppColors.cardBackground,
          title: Text(
            'Edit Category',
            style: TextStyle(color: AppColors.textLight),
          ),
          content: TextField(
            controller: nameController,
            style: TextStyle(color: AppColors.textLight),
            decoration: InputDecoration(
              hintText: 'Category name...',
              hintStyle: TextStyle(color: AppColors.textMedium),
              border: OutlineInputBorder(
                borderRadius: BorderRadius.circular(12.r),
                borderSide: BorderSide(color: AppColors.textMedium.withAlpha((0.3 * 255).round())),
              ),
              enabledBorder: OutlineInputBorder(
                borderRadius: BorderRadius.circular(12.r),
                borderSide: BorderSide(color: AppColors.textMedium.withAlpha((0.3 * 255).round())),
              ),
              focusedBorder: OutlineInputBorder(
                borderRadius: BorderRadius.circular(12.r),
                borderSide: BorderSide(color: category.color),
              ),
            ),
          ),
          actions: [
            TextButton(
              onPressed: () => Navigator.of(context).pop(),
              child: Text('Cancel', style: TextStyle(color: AppColors.textMedium)),
            ),
            TextButton(
              onPressed: () async {
                final newName = nameController.text.trim();
                if (newName.isNotEmpty) {
                  Navigator.of(context).pop();
                  await _updateCategoryName(category, newName);
                }
              },
              child: Text('Save', style: TextStyle(color: category.color)),
            ),
          ],
        );
      },
    );
  }

  void _showDeleteCategoryDialog(AffirmationCategory category, int categoryIndex) {
    showDialog(
      context: context,
      builder: (BuildContext context) {
        return AlertDialog(
          backgroundColor: AppColors.cardBackground,
          title: Text(
            'Delete Category',
            style: TextStyle(color: AppColors.textLight),
          ),
          content: Text(
            'Are you sure you want to delete "${category.name}"? This will also delete all affirmations in this category.',
            style: TextStyle(color: AppColors.textMedium),
          ),
          actions: [
            TextButton(
              onPressed: () => Navigator.of(context).pop(),
              child: Text('Cancel', style: TextStyle(color: AppColors.textMedium)),
            ),
            TextButton(
              onPressed: () async {
                Navigator.of(context).pop();
                await _deleteCategory(category, categoryIndex);
              },
              child: Text('Delete', style: TextStyle(color: AppColors.error)),
            ),
          ],
        );
      },
    );
  }

  Future<void> _updateCategoryName(AffirmationCategory category, String newName) async {
    try {
      final categoryId = int.tryParse(category.id);
      if (categoryId != null) {
        final result = await _affirmationService.updateCategory(
          categoryId: categoryId,
          categoryName: newName,
          colorAsInteger: category.color.value,
          predefinedIconTypeId: 3600001, // Default flower icon, would need to store this properly
        );
        
        if (result.isSuccess) {
          _showInfoSnackBar('Category name updated to "$newName"');
          await _loadCategories(); // Reload categories
        } else {
          _showErrorSnackBar('Failed to update category: ${result.message}');
        }
      } else {
        _showErrorSnackBar('Invalid category ID');
      }
    } catch (e) {
      _showErrorSnackBar('Error updating category: $e');
    }
  }

  Future<void> _deleteCategory(AffirmationCategory category, int categoryIndex) async {
    try {
      final categoryId = int.tryParse(category.id);
      if (categoryId != null) {
        final result = await _affirmationService.deleteCategory(categoryId);
        
        if (result.isSuccess) {
          _showInfoSnackBar('Category "${category.name}" deleted successfully');
          
          // If the deleted category was selected, switch to first category
          if (selectedCategoryIndex == categoryIndex) {
            await _flutterTts.stop();
            setState(() {
              selectedCategoryIndex = 0;
              currentAffirmationIndex = 0;
              isPlaying = false;
            });
          } else if (selectedCategoryIndex > categoryIndex) {
            // Adjust selected index if needed
            setState(() {
              selectedCategoryIndex--;
            });
          }
          
          await _loadCategories(); // Reload categories
        } else {
          _showErrorSnackBar('Failed to delete category: ${result.message}');
        }
      } else {
        _showErrorSnackBar('Invalid category ID');
      }
    } catch (e) {
      _showErrorSnackBar('Error deleting category: $e');
    }
  }

  Widget _buildEmptyAffirmationsState(AffirmationCategory category) {
    return Container(
      margin: EdgeInsets.all(20.w),
      padding: EdgeInsets.all(32.w),
      decoration: BoxDecoration(
        gradient: LinearGradient(
          colors: [
            category.color.withAlpha((0.05 * 255).round()),
            category.color.withAlpha((0.02 * 255).round()),
          ],
          begin: Alignment.topCenter,
          end: Alignment.bottomCenter,
        ),
        borderRadius: BorderRadius.circular(20.r),
        border: Border.all(
          color: category.color.withAlpha((0.2 * 255).round()),
          width: 1.5,
        ),
      ),
      child: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          // Large icon with animation-ready container
          Container(
            width: 80.w,
            height: 80.h,
            decoration: BoxDecoration(
              color: category.color.withAlpha((0.1 * 255).round()),
              shape: BoxShape.circle,
              border: Border.all(
                color: category.color.withAlpha((0.3 * 255).round()),
                width: 2,
              ),
            ),
            child: Icon(
              category.iconData,
              color: category.color,
              size: 40.sp,
            ),
          ),
          
          SizedBox(height: 24.h),
          
          // Title
          Text(
            'Start Your ${category.name} Journey',
            style: TextStyle(
              fontSize: 22.sp,
              fontWeight: FontWeight.bold,
              color: AppColors.textLight,
            ),
            textAlign: TextAlign.center,
          ),
          
          SizedBox(height: 12.h),
          
          // Subtitle
          Text(
            'Create your first positive affirmation to begin manifesting your dreams in ${category.name.toLowerCase()}.',
            style: TextStyle(
              fontSize: 16.sp,
              color: AppColors.textMedium,
              height: 1.4,
            ),
            textAlign: TextAlign.center,
          ),
          
          SizedBox(height: 32.h),
          
          // Add affirmation button
          GestureDetector(
            onTap: () => _showAddAffirmationDialog(category),
            child: Container(
              padding: EdgeInsets.symmetric(horizontal: 32.w, vertical: 16.h),
              decoration: BoxDecoration(
                gradient: LinearGradient(
                  colors: [
                    category.color,
                    category.color.withAlpha((0.8 * 255).round()),
                  ],
                ),
                borderRadius: BorderRadius.circular(25.r),
                boxShadow: [
                  BoxShadow(
                    color: category.color.withAlpha((0.3 * 255).round()),
                    blurRadius: 12.r,
                    offset: Offset(0, 6.h),
                  ),
                ],
              ),
              child: Row(
                mainAxisSize: MainAxisSize.min,
                children: [
                  Icon(
                    Icons.add_circle_outline,
                    color: AppColors.white,
                    size: 20.sp,
                  ),
                  SizedBox(width: 8.w),
                  Text(
                    'Add Your First Affirmation',
                    style: TextStyle(
                      fontSize: 16.sp,
                      fontWeight: FontWeight.w600,
                      color: AppColors.white,
                    ),
                  ),
                ],
              ),
            ),
          ),
          
          SizedBox(height: 20.h),
          
          // Inspiration text
          Container(
            padding: EdgeInsets.symmetric(horizontal: 20.w, vertical: 12.h),
            decoration: BoxDecoration(
              color: category.color.withAlpha((0.08 * 255).round()),
              borderRadius: BorderRadius.circular(12.r),
              border: Border.all(
                color: category.color.withAlpha((0.2 * 255).round()),
                width: 1,
              ),
            ),
            child: Row(
              mainAxisSize: MainAxisSize.min,
              children: [
                Icon(
                  Icons.lightbulb_outline,
                  color: category.color,
                  size: 18.sp,
                ),
                SizedBox(width: 8.w),
                Flexible(
                  child: Text(
                    'Tip: Start with "I am..." or "I believe..." statements',
                    style: TextStyle(
                      fontSize: 14.sp,
                      color: AppColors.textMedium,
                      fontStyle: FontStyle.italic,
                    ),
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  void _showAddAffirmationDialog(AffirmationCategory category) {
    final TextEditingController textController = TextEditingController();
    
    showDialog(
      context: context,
      builder: (BuildContext context) {
        return AlertDialog(
          backgroundColor: AppColors.cardBackground,
          shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(20.r)),
          title: Row(
            children: [
              Icon(
                category.iconData,
                color: category.color,
                size: 24.sp,
              ),
              SizedBox(width: 12.w),
              Expanded(
                child: Text(
                  'Add ${category.name} Affirmation',
                  style: TextStyle(
                    color: AppColors.textLight,
                    fontSize: 18.sp,
                    fontWeight: FontWeight.bold,
                  ),
                ),
              ),
            ],
          ),
          content: Column(
            mainAxisSize: MainAxisSize.min,
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(
                'Create a positive affirmation that resonates with your ${category.name.toLowerCase()} goals.',
                style: TextStyle(
                  color: AppColors.textMedium,
                  fontSize: 14.sp,
                ),
              ),
              SizedBox(height: 16.h),
              TextField(
                controller: textController,
                style: TextStyle(color: AppColors.textLight),
                maxLines: 3,
                decoration: InputDecoration(
                  hintText: 'I am worthy of ${category.name.toLowerCase()}...',
                  hintStyle: TextStyle(color: AppColors.textMedium.withAlpha((0.7 * 255).round())),
                  border: OutlineInputBorder(
                    borderRadius: BorderRadius.circular(12.r),
                    borderSide: BorderSide(color: AppColors.textMedium.withAlpha((0.3 * 255).round())),
                  ),
                  enabledBorder: OutlineInputBorder(
                    borderRadius: BorderRadius.circular(12.r),
                    borderSide: BorderSide(color: AppColors.textMedium.withAlpha((0.3 * 255).round())),
                  ),
                  focusedBorder: OutlineInputBorder(
                    borderRadius: BorderRadius.circular(12.r),
                    borderSide: BorderSide(color: category.color, width: 2),
                  ),
                  filled: true,
                  fillColor: AppColors.backgroundMedium,
                ),
              ),
            ],
          ),
          actions: [
            TextButton(
              onPressed: () => Navigator.of(context).pop(),
              child: Text('Cancel', style: TextStyle(color: AppColors.textMedium)),
            ),
            ElevatedButton(
              onPressed: () async {
                final text = textController.text.trim();
                if (text.isNotEmpty) {
                  Navigator.of(context).pop();
                  await _addAffirmationToCategory(category, text);
                }
              },
              style: ElevatedButton.styleFrom(
                backgroundColor: category.color,
                shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12.r)),
              ),
              child: Text(
                'Add Affirmation',
                style: TextStyle(color: AppColors.white),
              ),
            ),
          ],
        );
      },
    );
  }

  Future<void> _addAffirmationToCategory(AffirmationCategory category, String text) async {
    try {
      final categoryId = int.tryParse(category.id);
      if (categoryId != null) {
        final result = await _affirmationService.createAffirmationText(
          text: text,
          categoryId: categoryId,
        );
        
        if (result.isSuccess) {
          _showInfoSnackBar('Affirmation added successfully!');
          await _loadCategories(); // Reload categories to show the new affirmation
        } else {
          _showErrorSnackBar('Failed to add affirmation: ${result.message}');
        }
      } else {
        _showErrorSnackBar('Cannot add affirmation to default category');
      }
    } catch (e) {
      _showErrorSnackBar('Error adding affirmation: $e');
    }
  }

  void _showAddCategoryDialog() {
    showDialog(
      context: context,
      builder: (BuildContext context) => _AddCategoryDialog(
        onCategoryAdded: () {
          _loadCategories(); // Reload categories when a new one is added
        },
      ),
    );
  }
}

class _AddCategoryDialog extends StatefulWidget {
  final VoidCallback onCategoryAdded;

  const _AddCategoryDialog({required this.onCategoryAdded});

  @override
  _AddCategoryDialogState createState() => _AddCategoryDialogState();
}

class _AddCategoryDialogState extends State<_AddCategoryDialog> {
  final TextEditingController _nameController = TextEditingController();
  Color _selectedColor = AppColors.accentGold;
  PredefinedIconTypes _selectedIcon = PredefinedIconTypes.flower;
  bool _isCreating = false;

  @override
  Widget build(BuildContext context) {
    return AlertDialog(
      backgroundColor: AppColors.cardBackground,
      title: Text(
        'Add Category',
        style: TextStyle(
          color: AppColors.textLight,
          fontSize: 20.sp,
          fontWeight: FontWeight.bold,
        ),
      ),
      content: SizedBox(
        width: double.maxFinite,
        child: SingleChildScrollView(
          child: Column(
            mainAxisSize: MainAxisSize.min,
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
            // Category name input
            Text(
              'Category Name',
              style: TextStyle(
                color: AppColors.textLight,
                fontSize: 16.sp,
                fontWeight: FontWeight.w600,
              ),
            ),
            SizedBox(height: 8.h),
            TextField(
              controller: _nameController,
              style: TextStyle(color: AppColors.textLight),
              decoration: InputDecoration(
                hintText: 'Enter category name...',
                hintStyle: TextStyle(color: AppColors.textMedium),
                border: OutlineInputBorder(
                  borderRadius: BorderRadius.circular(12.r),
                  borderSide: BorderSide(color: AppColors.textMedium.withAlpha((0.3 * 255).round())),
                ),
                enabledBorder: OutlineInputBorder(
                  borderRadius: BorderRadius.circular(12.r),
                  borderSide: BorderSide(color: AppColors.textMedium.withAlpha((0.3 * 255).round())),
                ),
                focusedBorder: OutlineInputBorder(
                  borderRadius: BorderRadius.circular(12.r),
                  borderSide: BorderSide(color: _selectedColor),
                ),
              ),
            ),
            SizedBox(height: 20.h),
            
            // Color selection
            Text(
              'Choose Color',
              style: TextStyle(
                color: AppColors.textLight,
                fontSize: 16.sp,
                fontWeight: FontWeight.w600,
              ),
            ),
            SizedBox(height: 12.h),
            _buildColorSelector(),
            SizedBox(height: 20.h),
            
            // Icon selection
            Text(
              'Choose Icon',
              style: TextStyle(
                color: AppColors.textLight,
                fontSize: 16.sp,
                fontWeight: FontWeight.w600,
              ),
            ),
            SizedBox(height: 12.h),
            _buildIconSelector(),
          ],
        ),
        ),
      ),
      actions: [
        TextButton(
          onPressed: _isCreating ? null : () => Navigator.of(context).pop(),
          child: Text(
            'Cancel',
            style: TextStyle(color: AppColors.textMedium),
          ),
        ),
        ElevatedButton(
          onPressed: _isCreating ? null : _createCategory,
          style: ElevatedButton.styleFrom(
            backgroundColor: _selectedColor,
            shape: RoundedRectangleBorder(
              borderRadius: BorderRadius.circular(12.r),
            ),
          ),
          child: _isCreating
              ? SizedBox(
                  width: 16.w,
                  height: 16.h,
                  child: CircularProgressIndicator(
                    strokeWidth: 2,
                    valueColor: AlwaysStoppedAnimation<Color>(AppColors.white),
                  ),
                )
              : Text(
                  'Create',
                  style: TextStyle(color: AppColors.white),
                ),
        ),
      ],
    );
  }

  Widget _buildColorSelector() {
    final predefinedColors = ColorUtils.getPredefinedCategoryColors();
    
    return SizedBox(
      height: 60.h,
      child: ListView.builder(
        scrollDirection: Axis.horizontal,
        itemCount: predefinedColors.length,
        itemBuilder: (context, index) {
          final color = predefinedColors[index];
          final isSelected = _selectedColor == color;
          
          return GestureDetector(
            onTap: () {
              setState(() {
                _selectedColor = color;
              });
            },
            child: Container(
              width: 50.w,
              height: 50.h,
              margin: EdgeInsets.only(right: 12.w),
              decoration: BoxDecoration(
                color: color,
                shape: BoxShape.circle,
                border: isSelected 
                    ? Border.all(color: AppColors.white, width: 3)
                    : Border.all(color: AppColors.textMedium.withAlpha((0.3 * 255).round()), width: 1),
              ),
              child: isSelected 
                  ? Icon(
                      Icons.check,
                      color: ColorUtils.getTextColor(color),
                      size: 24.sp,
                    )
                  : null,
            ),
          );
        },
      ),
    );
  }

  Widget _buildIconSelector() {
    final allIcons = PredefinedIconTypes.getAllIcons();
    
    return SizedBox(
      height: 120.h,
      child: GridView.builder(
        shrinkWrap: true,
        physics: NeverScrollableScrollPhysics(),
        gridDelegate: SliverGridDelegateWithFixedCrossAxisCount(
          crossAxisCount: 5,
          mainAxisSpacing: 8.h,
          crossAxisSpacing: 8.w,
        ),
        itemCount: allIcons.length,
        itemBuilder: (context, index) {
          final iconType = allIcons[index];
          final isSelected = _selectedIcon == iconType;
          
          return GestureDetector(
            onTap: () {
              setState(() {
                _selectedIcon = iconType;
              });
            },
            child: Container(
              decoration: BoxDecoration(
                color: isSelected 
                    ? _selectedColor.withAlpha((0.2 * 255).round())
                    : AppColors.backgroundMedium,
                borderRadius: BorderRadius.circular(12.r),
                border: isSelected
                    ? Border.all(color: _selectedColor, width: 2)
                    : Border.all(color: AppColors.textMedium.withAlpha((0.3 * 255).round()), width: 1),
              ),
              child: Icon(
                iconType.iconData,
                color: isSelected ? _selectedColor : AppColors.textMedium,
                size: 24.sp,
              ),
            ),
          );
        },
      ),
    );
  }

  Future<void> _createCategory() async {
    final name = _nameController.text.trim();
    if (name.isEmpty) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Please enter a category name'),
          backgroundColor: AppColors.error,
        ),
      );
      return;
    }

    setState(() {
      _isCreating = true;
    });

    try {
      // Create the category using the API
      final affirmationService = DailyAffirmationsService();
      final result = await affirmationService.createCategory(
        categoryName: name,
        colorAsInteger: _selectedColor.value,
        predefinedIconTypeId: _selectedIcon.value,
      );
      
      if (result.isSuccess) {
        Navigator.of(context).pop();
        widget.onCategoryAdded();
        
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Category "$name" created successfully!'),
            backgroundColor: AppColors.success,
            behavior: SnackBarBehavior.floating,
            shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12.r)),
          ),
        );
      } else {
        setState(() {
          _isCreating = false;
        });
        
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Failed to create category: ${result.message}'),
            backgroundColor: AppColors.error,
            behavior: SnackBarBehavior.floating,
            shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12.r)),
          ),
        );
      }
    } catch (e) {
      setState(() {
        _isCreating = false;
      });
      
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Error creating category: $e'),
          backgroundColor: AppColors.error,
          behavior: SnackBarBehavior.floating,
          shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12.r)),
        ),
      );
    }
  }

  @override
  void dispose() {
    _nameController.dispose();
    super.dispose();
  }
}