import 'dart:async';

import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:flutter_tts/flutter_tts.dart';
import '../models/affirmations.dart';
import '../constants/colors.dart';
import '../constants/app_theme.dart';
import '../constants/category_icons.dart';
import '../utils/color_utils.dart';
import '../services/daily_affirmations_service.dart';
import '../services/usage_analytics_service.dart';
import '../services/guided_tour_service.dart';
import '../services/subscription_service.dart';
import '../components/premium_upgrade_dialog.dart';
import '../models/analytics_models.dart';
import '../widgets/stylish_loading.dart';
import '../widgets/universal_back_button.dart';

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
        
        final isCustomCategory = !categoryData.isDefault;
        print('📂 Category: ${categoryData.name} - isDefault: ${categoryData.isDefault}, isCustom: $isCustomCategory');
        
        uiCategories.add(AffirmationCategory(
          id: categoryData.id,
          name: categoryData.name,
          color: categoryColor,
          iconValue: categoryData.iconId,
          affirmations: affirmations,
          isCustom: isCustomCategory, // Mark as custom if not default
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
      AppTheme.affirmationGradient[0],
      AppTheme.goalTrackerGradient[0],
      AppTheme.accentGold,
      AppTheme.accentCoral,
      AppTheme.visionBoardGradient[0],
      AppTheme.visualMeditationGradient[0],
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
    // Show loading screen while loading affirmations
    if (isLoading) {
      return Scaffold(
        backgroundColor: AppTheme.backgroundPrimary,
        body: StylishLoading(
          message: 'Loading affirmations...',
        ),
      );
    }

    return Scaffold(
      backgroundColor: AppTheme.backgroundPrimary,
      body: SafeArea(
        child: Column(
          children: [
            _buildHeader(),
            _buildCategoryChips(),
            _buildAffirmationContent(),
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
          UniversalBackButton(),
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
                    color: AppTheme.textPrimary,
                  ),
                ),
                Text(
                  'Transform your mindset with positive thoughts',
                  style: TextStyle(
                    fontSize: 14.sp,
                    color: AppTheme.textSecondary,
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
                color: AppTheme.cardBackground,
                borderRadius: BorderRadius.circular(12.r),
              ),
              child: Icon(
                Icons.add_circle_outline,
                color: AppTheme.accentGold,
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
                    ? category.color.withValues(alpha: 0.1)
                    : AppTheme.cardBackground,
                borderRadius: BorderRadius.circular(25.r),
                border: Border.all(
                  color: isSelected ? category.color : AppTheme.textSecondary.withValues(alpha: 0.3),
                ),
              ),
              child: Center(
                child: Row(
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    Icon(
                      category.iconData,
                      color: isSelected ? category.color : AppTheme.textSecondary,
                      size: 16.sp,
                    ),
                    SizedBox(width: 6.w),
                    Text(
                      category.name,
                      style: TextStyle(
                        fontSize: 14.sp,
                        fontWeight: isSelected ? FontWeight.w600 : FontWeight.w500,
                        color: isSelected ? category.color : AppTheme.textSecondary,
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


  Widget _buildAffirmationContent() {
    if (categories.isEmpty) {
      return Expanded(
        child: Center(
          child: Text(
            'No affirmations available',
            style: TextStyle(
              fontSize: 16.sp,
              color: AppTheme.textSecondary,
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
              currentCategory.color.withValues(alpha: 0.1),
              currentCategory.color.withAlpha((0.05 * 255).round()),
            ],
            begin: Alignment.topCenter,
            end: Alignment.bottomCenter,
          ),
          borderRadius: BorderRadius.circular(20.r),
          border: Border.all(
            color: currentCategory.color.withValues(alpha: 0.3),
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
                    color: AppTheme.textPrimary,
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
                color: AppTheme.cardBackground,
                shape: BoxShape.circle,
                border: Border.all(
                  color: AppTheme.textSecondary.withValues(alpha: 0.3),
                ),
              ),
              child: Icon(
                Icons.edit,
                color: AppTheme.textPrimary,
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
                color: AppTheme.cardBackground,
                shape: BoxShape.circle,
                border: Border.all(
                  color: AppTheme.error.withValues(alpha: 0.3),
                ),
              ),
              child: Icon(
                Icons.delete_outline,
                color: AppTheme.error,
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
                    : AppTheme.textSecondary.withValues(alpha: 0.3),
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
              color: currentCategory.color.withValues(alpha: 0.1),
              borderRadius: BorderRadius.circular(24.r),
              border: Border.all(
                color: currentCategory.color.withValues(alpha: 0.3),
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
          color: AppTheme.cardBackground,
          shape: BoxShape.circle,
          border: Border.all(
            color: enabled 
                ? AppTheme.textSecondary.withValues(alpha: 0.3)
                : AppTheme.textSecondary.withValues(alpha: 0.1),
          ),
        ),
        child: Icon(
          icon,
          color: enabled ? AppTheme.textPrimary : AppTheme.textSecondary.withValues(alpha: 0.5),
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
          color: AppTheme.white,
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
          backgroundColor: AppTheme.cardBackground,
          title: Text(
            'Delete Affirmation',
            style: TextStyle(color: AppTheme.textPrimary),
          ),
          content: Text(
            'Are you sure you want to delete this affirmation?',
            style: TextStyle(color: AppTheme.textSecondary),
          ),
          actions: [
            TextButton(
              onPressed: () => Navigator.of(context).pop(false),
              child: Text('Cancel', style: TextStyle(color: AppTheme.textSecondary)),
            ),
            TextButton(
              onPressed: () => Navigator.of(context).pop(true),
              child: Text('Delete', style: TextStyle(color: AppTheme.error)),
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
          backgroundColor: AppTheme.cardBackground,
          title: Text(
            'Edit Affirmation',
            style: TextStyle(color: AppTheme.textPrimary),
          ),
          content: TextField(
            controller: textController,
            style: TextStyle(color: AppTheme.textPrimary),
            maxLines: 3,
            decoration: InputDecoration(
              hintText: 'Enter your affirmation...',
              hintStyle: TextStyle(color: AppTheme.textSecondary),
              border: OutlineInputBorder(
                borderRadius: BorderRadius.circular(12.r),
                borderSide: BorderSide(color: AppTheme.textSecondary.withValues(alpha: 0.3)),
              ),
              enabledBorder: OutlineInputBorder(
                borderRadius: BorderRadius.circular(12.r),
                borderSide: BorderSide(color: AppTheme.textSecondary.withValues(alpha: 0.3)),
              ),
              focusedBorder: OutlineInputBorder(
                borderRadius: BorderRadius.circular(12.r),
                borderSide: BorderSide(color: AppTheme.affirmationGradient[0]),
              ),
            ),
          ),
          actions: [
            TextButton(
              onPressed: () => Navigator.of(context).pop(),
              child: Text('Cancel', style: TextStyle(color: AppTheme.textSecondary)),
            ),
            TextButton(
              onPressed: () async {
                final newText = textController.text.trim();
                if (newText.isNotEmpty) {
                  Navigator.of(context).pop();
                  await _updateAffirmation(affirmation, newText);
                }
              },
              child: Text('Save', style: TextStyle(color: AppTheme.affirmationGradient[0])),
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
        backgroundColor: AppTheme.success,
        behavior: SnackBarBehavior.floating,
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12.r)),
      ),
    );
  }

  void _showErrorSnackBar(String message) {
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text(message),
        backgroundColor: AppTheme.error,
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
      backgroundColor: AppTheme.cardBackground,
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
                  color: AppTheme.textSecondary.withValues(alpha: 0.3),
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
                  color: AppTheme.textPrimary,
                ),
              ),
              SizedBox(height: 20.h),
              
              // Edit option
              ListTile(
                leading: Icon(Icons.edit, color: AppTheme.textPrimary),
                title: Text(
                  'Edit Category',
                  style: TextStyle(color: AppTheme.textPrimary),
                ),
                onTap: () {
                  Navigator.pop(context);
                  _showEditCategoryDialog(category);
                },
              ),
              
              // Delete option
              ListTile(
                leading: Icon(Icons.delete, color: AppTheme.error),
                title: Text(
                  'Delete Category',
                  style: TextStyle(color: AppTheme.error),
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
          backgroundColor: AppTheme.cardBackground,
          title: Text(
            'Edit Category',
            style: TextStyle(color: AppTheme.textPrimary),
          ),
          content: TextField(
            controller: nameController,
            style: TextStyle(color: AppTheme.textPrimary),
            decoration: InputDecoration(
              hintText: 'Category name...',
              hintStyle: TextStyle(color: AppTheme.textSecondary),
              border: OutlineInputBorder(
                borderRadius: BorderRadius.circular(12.r),
                borderSide: BorderSide(color: AppTheme.textSecondary.withValues(alpha: 0.3)),
              ),
              enabledBorder: OutlineInputBorder(
                borderRadius: BorderRadius.circular(12.r),
                borderSide: BorderSide(color: AppTheme.textSecondary.withValues(alpha: 0.3)),
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
              child: Text('Cancel', style: TextStyle(color: AppTheme.textSecondary)),
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
          backgroundColor: AppTheme.cardBackground,
          title: Text(
            'Delete Category',
            style: TextStyle(color: AppTheme.textPrimary),
          ),
          content: Text(
            'Are you sure you want to delete "${category.name}"? This will also delete all affirmations in this category.',
            style: TextStyle(color: AppTheme.textSecondary),
          ),
          actions: [
            TextButton(
              onPressed: () => Navigator.of(context).pop(),
              child: Text('Cancel', style: TextStyle(color: AppTheme.textSecondary)),
            ),
            TextButton(
              onPressed: () async {
                Navigator.of(context).pop();
                await _deleteCategory(category, categoryIndex);
              },
              child: Text('Delete', style: TextStyle(color: AppTheme.error)),
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
          colorAsInteger: category.color.toARGB32(),
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
              color: category.color.withValues(alpha: 0.1),
              shape: BoxShape.circle,
              border: Border.all(
                color: category.color.withValues(alpha: 0.3),
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
              color: AppTheme.textPrimary,
            ),
            textAlign: TextAlign.center,
          ),
          
          SizedBox(height: 12.h),
          
          // Subtitle
          Text(
            'Create your first positive affirmation to begin manifesting your dreams in ${category.name.toLowerCase()}.',
            style: TextStyle(
              fontSize: 16.sp,
              color: AppTheme.textSecondary,
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
                    color: category.color.withValues(alpha: 0.3),
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
                    color: AppTheme.white,
                    size: 20.sp,
                  ),
                  SizedBox(width: 8.w),
                  Text(
                    'Add Your First Affirmation',
                    style: TextStyle(
                      fontSize: 16.sp,
                      fontWeight: FontWeight.w600,
                      color: AppTheme.white,
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
                      color: AppTheme.textSecondary,
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
    // Check subscription limits for affirmations in this category
    final subscriptionService = SubscriptionService();
    if (!subscriptionService.canCreateAffirmation(category.affirmations.length)) {
      print('🚫 Affirmation creation blocked - limit reached: ${category.affirmations.length}/${SubscriptionService.maxFreeAffirmationsPerCategory}');
      showDialog(
        context: context,
        builder: (context) => PremiumUpgradeDialog(
          feature: 'affirmation',
          customMessage: 'Unlock unlimited affirmations and premium categories',
        ),
      );
      return;
    }
    
    final TextEditingController textController = TextEditingController();
    
    showDialog(
      context: context,
      builder: (BuildContext context) {
        return AlertDialog(
          backgroundColor: AppTheme.cardBackground,
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
                    color: AppTheme.textPrimary,
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
                  color: AppTheme.textSecondary,
                  fontSize: 14.sp,
                ),
              ),
              SizedBox(height: 16.h),
              TextField(
                controller: textController,
                style: TextStyle(color: AppTheme.textPrimary),
                maxLines: 3,
                decoration: InputDecoration(
                  hintText: 'I am worthy of ${category.name.toLowerCase()}...',
                  hintStyle: TextStyle(color: AppTheme.textSecondary.withValues(alpha: 0.7)),
                  border: OutlineInputBorder(
                    borderRadius: BorderRadius.circular(12.r),
                    borderSide: BorderSide(color: AppTheme.textSecondary.withValues(alpha: 0.3)),
                  ),
                  enabledBorder: OutlineInputBorder(
                    borderRadius: BorderRadius.circular(12.r),
                    borderSide: BorderSide(color: AppTheme.textSecondary.withValues(alpha: 0.3)),
                  ),
                  focusedBorder: OutlineInputBorder(
                    borderRadius: BorderRadius.circular(12.r),
                    borderSide: BorderSide(color: category.color, width: 2),
                  ),
                  filled: true,
                  fillColor: AppTheme.backgroundMedium,
                ),
              ),
            ],
          ),
          actions: [
            TextButton(
              onPressed: () => Navigator.of(context).pop(),
              child: Text('Cancel', style: TextStyle(color: AppTheme.textSecondary)),
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
                style: TextStyle(color: AppTheme.white),
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
    // Check subscription limits for custom categories only (default categories don't count)
    final subscriptionService = SubscriptionService();
    final customCategoriesCount = categories.where((cat) => cat.isCustom).length;
    final totalCategoriesCount = categories.length;
    
    print('🔍 Category Creation Check:');
    print('  - Total categories: $totalCategoriesCount');
    print('  - Custom categories: $customCategoriesCount');
    print('  - Is Premium: ${subscriptionService.isPremium}');
    print('  - Max free custom categories: ${SubscriptionService.maxFreeCustomAffirmationCategories}');
    print('  - Can create: ${subscriptionService.canCreateAffirmationCategory(customCategoriesCount)}');
    
    // Debug each category
    categories.forEach((category) {
      print('  📂 ${category.name}: isCustom=${category.isCustom}');
    });
    
    if (!subscriptionService.canCreateAffirmationCategory(customCategoriesCount)) {
      print('🚫 Category creation blocked - custom category limit reached: $customCategoriesCount/${SubscriptionService.maxFreeCustomAffirmationCategories}');
      showDialog(
        context: context,
        builder: (context) => PremiumUpgradeDialog(
          feature: 'affirmation_category',
          customMessage: 'Unlock unlimited custom categories and premium features',
        ),
      );
      return;
    }
    
    showModalBottomSheet(
      context: context,
      backgroundColor: Colors.transparent,
      isScrollControlled: true,
      builder: (BuildContext context) => _AddCategoryBottomSheet(
        onCategoryAdded: () {
          _loadCategories(); // Reload categories when a new one is added
        },
      ),
    );
  }
}

class _AddCategoryBottomSheet extends StatefulWidget {
  final VoidCallback onCategoryAdded;

  const _AddCategoryBottomSheet({required this.onCategoryAdded});

  @override
  _AddCategoryBottomSheetState createState() => _AddCategoryBottomSheetState();
}

class _AddCategoryBottomSheetState extends State<_AddCategoryBottomSheet> {
  final TextEditingController _nameController = TextEditingController();
  Color _selectedColor = AppTheme.accentGold;
  PredefinedIconTypes _selectedIcon = PredefinedIconTypes.flower;
  bool _isCreating = false;

  @override
  void dispose() {
    _nameController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Container(
      height: MediaQuery.of(context).size.height * 0.85,
      decoration: BoxDecoration(
        color: AppTheme.cardBackground,
        borderRadius: BorderRadius.vertical(top: Radius.circular(24.r)),
        boxShadow: [
          BoxShadow(
            color: AppTheme.black.withValues(alpha: 0.1),
            blurRadius: 20.r,
            offset: Offset(0, -8.h),
          ),
        ],
      ),
      child: Column(
        children: [
          // Drag handle
          _buildDragHandle(),
          
          // Header with preview
          _buildHeader(),
          
          // Content
          Expanded(
            child: SingleChildScrollView(
              padding: EdgeInsets.all(24.w),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  // Category name input
                  _buildNameInput(),
                  SizedBox(height: 32.h),
                  
                  // Color selection with navigation
                  _buildColorSection(),
                  SizedBox(height: 32.h),
                  
                  // Icon selection with navigation
                  _buildIconSection(),
                  SizedBox(height: 32.h),
                ],
              ),
            ),
          ),
          
          // Action buttons
          _buildActionButtons(),
        ],
      ),
    );
  }

  Widget _buildDragHandle() {
    return Container(
      width: 40.w,
      height: 4.h,
      margin: EdgeInsets.only(top: 12.h, bottom: 8.h),
      decoration: BoxDecoration(
        color: AppTheme.textSecondary.withValues(alpha: 0.3),
        borderRadius: BorderRadius.circular(2.r),
      ),
    );
  }

  Widget _buildHeader() {
    return Container(
      padding: EdgeInsets.all(24.w),
      decoration: BoxDecoration(
        gradient: LinearGradient(
          colors: [
            _selectedColor.withValues(alpha: 0.1),
            _selectedColor.withValues(alpha: 0.05),
          ],
          begin: Alignment.topLeft,
          end: Alignment.bottomRight,
        ),
        borderRadius: BorderRadius.zero,
      ),
      child: Row(
        children: [
          // Preview icon
          Container(
            width: 56.w,
            height: 56.h,
            decoration: BoxDecoration(
              color: _selectedColor,
              borderRadius: BorderRadius.circular(16.r),
              boxShadow: [
                BoxShadow(
                  color: _selectedColor.withValues(alpha: 0.3),
                  blurRadius: 12.r,
                  offset: Offset(0, 4.h),
                ),
              ],
            ),
            child: Icon(
              Icons.add,
              color: ColorUtils.getTextColor(_selectedColor),
              size: 28.sp,
            ),
          ),
          SizedBox(width: 16.w),
          
          // Title and subtitle
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  'Create Category',
                  style: TextStyle(
                    color: AppTheme.textPrimary,
                    fontSize: 20.sp,
                    fontWeight: FontWeight.bold,
                  ),
                ),
                SizedBox(height: 4.h),
                Text(
                  _nameController.text.isEmpty 
                      ? 'Enter name, pick color & icon'
                      : _nameController.text,
                  style: TextStyle(
                    color: AppTheme.textSecondary,
                    fontSize: 14.sp,
                  ),
                  maxLines: 1,
                  overflow: TextOverflow.ellipsis,
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildNameInput() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Category Name',
          style: TextStyle(
            color: AppTheme.textPrimary,
            fontSize: 16.sp,
            fontWeight: FontWeight.w600,
          ),
        ),
        SizedBox(height: 12.h),
        Container(
          decoration: BoxDecoration(
            borderRadius: BorderRadius.circular(16.r),
            border: Border.all(
              color: AppTheme.textSecondary.withValues(alpha: 0.2),
            ),
          ),
          child: TextField(
            controller: _nameController,
            style: TextStyle(
              color: AppTheme.textPrimary,
              fontSize: 16.sp,
            ),
            onChanged: (value) => setState(() {}), // Update preview
            decoration: InputDecoration(
              hintText: 'e.g., Health & Wellness',
              hintStyle: TextStyle(
                color: AppTheme.textSecondary,
                fontSize: 16.sp,
              ),
              border: InputBorder.none,
              contentPadding: EdgeInsets.all(16.w),
            ),
          ),
        ),
      ],
    );
  }

  Widget _buildColorSection() {
    final colors = ColorUtils.getPredefinedCategoryColors();
    final colorsPerPage = 15;
    final totalPages = (colors.length / colorsPerPage).ceil();
    
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Choose Color',
          style: TextStyle(
            color: AppTheme.textPrimary,
            fontSize: 16.sp,
            fontWeight: FontWeight.w600,
          ),
        ),
        SizedBox(height: 16.h),
        
        // Color selection grid
        SizedBox(
          height: 120.h,
          child: GridView.builder(
            scrollDirection: Axis.horizontal,
            gridDelegate: SliverGridDelegateWithFixedCrossAxisCount(
              crossAxisCount: 3,
              mainAxisSpacing: 12.w,
              crossAxisSpacing: 12.h,
              childAspectRatio: 1.0,
            ),
            itemCount: colors.length,
            itemBuilder: (context, index) => _buildColorOption(colors[index]),
          ),
        ),
      ],
    );
  }

  Widget _buildIconSection() {
    final icons = PredefinedIconTypes.getAllIcons();
    final iconsPerPage = 20;
    final totalPages = (icons.length / iconsPerPage).ceil();
    
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Choose Icon',
          style: TextStyle(
            color: AppTheme.textPrimary,
            fontSize: 16.sp,
            fontWeight: FontWeight.w600,
          ),
        ),
        SizedBox(height: 16.h),
        
        // Icon selection grid
        SizedBox(
          height: 200.h,
          child: GridView.builder(
            gridDelegate: SliverGridDelegateWithFixedCrossAxisCount(
              crossAxisCount: 6,
              mainAxisSpacing: 12.h,
              crossAxisSpacing: 12.w,
              childAspectRatio: 1.0,
            ),
            itemCount: icons.length,
            itemBuilder: (context, index) => _buildIconOption(icons[index]),
          ),
        ),
      ],
    );
  }

  Widget _buildColorOption(Color color) {
    final isSelected = _selectedColor == color;
    
    return GestureDetector(
      onTap: () => setState(() => _selectedColor = color),
      child: Container(
        width: 44.w,
        height: 44.h,
        decoration: BoxDecoration(
          color: color,
          shape: BoxShape.circle,
          border: Border.all(
            color: isSelected 
                ? AppTheme.white 
                : AppTheme.textSecondary.withValues(alpha: 0.3),
            width: isSelected ? 3 : 1,
          ),
          boxShadow: isSelected ? [
            BoxShadow(
              color: color.withValues(alpha: 0.3),
              blurRadius: 8.r,
              offset: Offset(0, 2.h),
            ),
          ] : null,
        ),
        child: isSelected 
            ? Icon(
                Icons.check,
                color: ColorUtils.getTextColor(color),
                size: 20.sp,
              )
            : null,
      ),
    );
  }

  Widget _buildIconOption(PredefinedIconTypes iconType) {
    final isSelected = _selectedIcon == iconType;
    
    return GestureDetector(
      onTap: () => setState(() => _selectedIcon = iconType),
      child: Container(
        decoration: BoxDecoration(
          color: isSelected 
              ? _selectedColor.withValues(alpha: 0.15)
              : AppTheme.backgroundPrimary,
          borderRadius: BorderRadius.circular(12.r),
          border: Border.all(
            color: isSelected 
                ? _selectedColor 
                : AppTheme.textSecondary.withValues(alpha: 0.2),
            width: isSelected ? 2 : 1,
          ),
        ),
        child: Icon(
          iconType.iconData,
          color: isSelected ? _selectedColor : AppTheme.textSecondary,
          size: 24.sp,
        ),
      ),
    );
  }

  Widget _buildActionButtons() {
    return Container(
      padding: EdgeInsets.fromLTRB(24.w, 16.h, 24.w, 32.h),
      decoration: BoxDecoration(
        color: AppTheme.backgroundPrimary,
        border: Border(
          top: BorderSide(
            color: AppTheme.textSecondary.withValues(alpha: 0.1),
            width: 1,
          ),
        ),
      ),
      child: Row(
        children: [
          Expanded(
            child: TextButton(
              onPressed: _isCreating ? null : () => Navigator.of(context).pop(),
              style: TextButton.styleFrom(
                padding: EdgeInsets.symmetric(vertical: 16.h),
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(16.r),
                ),
              ),
              child: Text(
                'Cancel',
                style: TextStyle(
                  color: AppTheme.textSecondary,
                  fontSize: 16.sp,
                  fontWeight: FontWeight.w600,
                ),
              ),
            ),
          ),
          SizedBox(width: 16.w),
          Expanded(
            flex: 2,
            child: ElevatedButton(
              onPressed: _isCreating ? null : _createCategory,
              style: ElevatedButton.styleFrom(
                backgroundColor: _selectedColor,
                padding: EdgeInsets.symmetric(vertical: 16.h),
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(16.r),
                ),
                elevation: 0,
                shadowColor: Colors.transparent,
              ),
              child: _isCreating
                  ? CompactStylishLoading(
                      color: ColorUtils.getTextColor(_selectedColor),
                      size: 20.w,
                    )
                  : Text(
                      'Create Category',
                      style: TextStyle(
                        color: ColorUtils.getTextColor(_selectedColor),
                        fontSize: 16.sp,
                        fontWeight: FontWeight.w600,
                      ),
                    ),
            ),
          ),
        ],
      ),
    );
  }

  Future<void> _createCategory() async {
    final name = _nameController.text.trim();
    if (name.isEmpty) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Please enter a category name'),
          backgroundColor: AppTheme.error,
        ),
      );
      return;
    }

    // Double-check subscription limits as a safety measure
    final subscriptionService = SubscriptionService();
    // We need to get categories from parent widget - but since this is already checked
    // in _showAddCategoryDialog, this is just a safety measure
    print('🔒 Double-checking category creation limits before API call');

    setState(() {
      _isCreating = true;
    });

    try {
      // Create the category using the API
      final affirmationService = DailyAffirmationsService();
      final result = await affirmationService.createCategory(
        categoryName: name,
        colorAsInteger: _selectedColor.toARGB32(),
        predefinedIconTypeId: _selectedIcon.value,
      );
      
      if (result.isSuccess) {
        Navigator.of(context).pop();
        widget.onCategoryAdded();
        
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Category "$name" created successfully!'),
            backgroundColor: AppTheme.success,
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
            backgroundColor: AppTheme.error,
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
          backgroundColor: AppTheme.error,
          behavior: SnackBarBehavior.floating,
          shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12.r)),
        ),
      );
    }
  }
}