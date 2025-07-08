import 'dart:async';

import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:manifestation_app/affirmations.dart';
import 'package:manifestation_app/category_chip.dart';
import 'package:manifestation_app/storage_service.dart';
import 'package:manifestation_app/add_affirmation_dialog.dart';
import 'package:manifestation_app/add_category_dialog.dart';
import 'package:manifestation_app/hybrid_voice_service.dart';
import 'package:manifestation_app/favorites_screen.dart';
import '../colors.dart';

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
  final HybridVoiceService _ttsService = HybridVoiceService();

  @override
  void initState() {
    super.initState();
    _loadCategories();
    _initializeTTS();
  }

Future<void> _initializeTTS() async {
  try {
    print('Initializing TTS in screen...');
    await _ttsService.initialize();
    print('TTS initialized in screen. Plugin available: ${_ttsService.isPluginAvailable}');
    
    // Show user what TTS mode is being used
    if (!_ttsService.isPluginAvailable) {
      Future.delayed(Duration(seconds: 1), () {
        if (mounted) {
          _showInfoSnackBar('Using simulated speech mode');
        }
      });
    }
  } catch (e) {
    print('Failed to initialize TTS in screen: $e');
  }
}

  Future<void> _loadCategories() async {
    setState(() {
      isLoading = true;
    });
    
    try {
      final loadedCategories = await StorageService.loadCategories();
      setState(() {
        categories = loadedCategories;
        isLoading = false;
      });
    } catch (e) {
      setState(() {
        isLoading = false;
      });
      _showErrorSnackBar('Failed to load categories');
    }
  }

  AffirmationCategory get currentCategory => categories[selectedCategoryIndex];
  Affirmation get currentAffirmation => 
      currentCategory.affirmations[currentAffirmationIndex];

  @override
  Widget build(BuildContext context) {
    if (isLoading) {
      return Scaffold(
        backgroundColor: AppColors.backgroundDark,
        body: Center(
          child: CircularProgressIndicator(
            valueColor: AlwaysStoppedAnimation<Color>(AppColors.accentGold),
          ),
        ),
      );
    }

    return Scaffold(
      backgroundColor: AppColors.backgroundDark,
      body: SafeArea(
        child: Column(
          children: [
            // Header
            _buildHeader(),
            
            // Category selection
            _buildCategorySelection(),
            
            SizedBox(height: 30.h),
            
            // Main affirmation display
            Expanded(
              child: categories.isEmpty 
                  ? _buildEmptyState()
                  : currentCategory.affirmations.isEmpty
                      ? _buildEmptyAffirmationsState()
                      : _buildAffirmationDisplay(),
            ),
            SizedBox(
              height: 20.h,
            ),
            // Player controls (only show if there are affirmations)
            if (categories.isNotEmpty && currentCategory.affirmations.isNotEmpty)
              _buildPlayerControls(),
            
            SizedBox(height: 40.h),
          ],
        ),
      ),
    );
  }

  Widget _buildHeader() {
    return Padding(
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
                  'Speak your truth into existence',
                  style: TextStyle(
                    fontSize: 14.sp,
                    color: AppColors.textMedium,
                  ),
                ),
              ],
            ),
          ),
          Row(
            children: [
              // TTS Settings button
              GestureDetector(
                onTap: _showTTSSettings,
                child: Container(
                  width: 44.w,
                  height: 44.h,
                  decoration: BoxDecoration(
                    color: AppColors.secondaryTeal.withOpacity(0.2),
                    borderRadius: BorderRadius.circular(12.r),
                  ),
                  child: Icon(
                    Icons.settings_voice,
                    color: AppColors.secondaryTeal,
                    size: 20.sp,
                  ),
                ),
              ),
              SizedBox(width: 12.w),
              // Add category button
              GestureDetector(
                onTap: _showAddCategoryDialog,
                child: Container(
                  width: 44.w,
                  height: 44.h,
                  decoration: BoxDecoration(
                    color: AppColors.accentGold.withOpacity(0.2),
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
        ],
      ),
    );
  }

  Widget _buildCategorySelection() {
    return Container(
      height: 50.h,
      child: ListView.builder(
        scrollDirection: Axis.horizontal,
        padding: EdgeInsets.symmetric(horizontal: 20.w),
        itemCount: categories.length,
        itemBuilder: (context, index) {
          return Padding(
            padding: EdgeInsets.only(right: 12.w),
            child: GestureDetector(
              onLongPress: () => _showCategoryOptions(categories[index]),
              child: CategoryChip(
                name: categories[index].name,
                color: categories[index].color,
                isSelected: selectedCategoryIndex == index,
                onTap: () => _selectCategory(index),
              ),
            ),
          );
        },
      ),
    );
  }

  Widget _buildEmptyState() {
    return Center(
      child: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          Container(
            width: 80.w,
            height: 80.h,
            decoration: BoxDecoration(
              color: AppColors.accentGold.withOpacity(0.2),
              borderRadius: BorderRadius.circular(20.r),
            ),
            child: Icon(
              Icons.category_outlined,
              color: AppColors.accentGold,
              size: 40.sp,
            ),
          ),
          SizedBox(height: 24.h),
          Text(
            'No Categories Yet',
            style: TextStyle(
              fontSize: 20.sp,
              fontWeight: FontWeight.bold,
              color: AppColors.textLight,
            ),
          ),
          SizedBox(height: 8.h),
          Text(
            'Create your first category to start\norganizing your affirmations',
            textAlign: TextAlign.center,
            style: TextStyle(
              fontSize: 14.sp,
              color: AppColors.textMedium,
              height: 1.4,
            ),
          ),
          SizedBox(height: 24.h),
          GestureDetector(
            onTap: _showAddCategoryDialog,
            child: Container(
              padding: EdgeInsets.symmetric(horizontal: 24.w, vertical: 12.h),
              decoration: BoxDecoration(
                gradient: LinearGradient(
                  colors: [
                    AppColors.accentGold,
                    AppColors.accentGold.withOpacity(0.8),
                  ],
                ),
                borderRadius: BorderRadius.circular(12.r),
              ),
              child: Text(
                'Create Category',
                style: TextStyle(
                  fontSize: 16.sp,
                  fontWeight: FontWeight.w600,
                  color: AppColors.white,
                ),
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildEmptyAffirmationsState() {
    return Center(
      child: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          Container(
            width: 80.w,
            height: 80.h,
            decoration: BoxDecoration(
              color: currentCategory.color.withOpacity(0.2),
              borderRadius: BorderRadius.circular(20.r),
            ),
            child: Icon(
              Icons.record_voice_over_outlined,
              color: currentCategory.color,
              size: 40.sp,
            ),
          ),
          SizedBox(height: 24.h),
          Text(
            'No Affirmations Yet',
            style: TextStyle(
              fontSize: 20.sp,
              fontWeight: FontWeight.bold,
              color: AppColors.textLight,
            ),
          ),
          SizedBox(height: 8.h),
          Text(
            'Add your first affirmation to\n${currentCategory.name} category',
            textAlign: TextAlign.center,
            style: TextStyle(
              fontSize: 14.sp,
              color: AppColors.textMedium,
              height: 1.4,
            ),
          ),
          SizedBox(height: 24.h),
          GestureDetector(
            onTap: _showAddAffirmationDialog,
            child: Container(
              padding: EdgeInsets.symmetric(horizontal: 24.w, vertical: 12.h),
              decoration: BoxDecoration(
                color: currentCategory.color,
                borderRadius: BorderRadius.circular(12.r),
              ),
              child: Text(
                'Add Affirmation',
                style: TextStyle(
                  fontSize: 16.sp,
                  fontWeight: FontWeight.w600,
                  color: AppColors.white,
                ),
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildAffirmationDisplay() {
    return Container(
      margin: EdgeInsets.symmetric(horizontal: 20.w),
      padding: EdgeInsets.all(24.w),
      height: 400.h, // Fixed height for consistent layout
      decoration: BoxDecoration(
        gradient: LinearGradient(
          colors: [
            currentCategory.color.withOpacity(0.1),
            currentCategory.color.withOpacity(0.05),
          ],
          begin: Alignment.topCenter,
          end: Alignment.bottomCenter,
        ),
        borderRadius: BorderRadius.circular(20.r),
        border: Border.all(
          color: currentCategory.color.withOpacity(0.3),
          width: 1,
        ),
      ),
      child: Column(
        children: [
          // Header with counter and actions
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              Text(
                '${currentAffirmationIndex + 1} of ${currentCategory.affirmations.length}',
                style: TextStyle(
                  fontSize: 14.sp,
                  color: AppColors.textMedium,
                  fontWeight: FontWeight.w500,
                ),
              ),
              Row(
                children: [
                  // View Favorites button
                  GestureDetector(
                    onTap: _goToFavorites,
                    child: Icon(
                      Icons.favorite_border,
                      color: AppColors.accentGold,
                      size: 20.sp,
                    ),
                  ),
                  SizedBox(width: 12.w),
                  // Favorite button
                  GestureDetector(
                    onTap: _toggleFavorite,
                    child: Icon(
                      currentAffirmation.isFavorite 
                          ? Icons.favorite 
                          : Icons.favorite_border,
                      color: currentAffirmation.isFavorite 
                          ? Colors.red 
                          : AppColors.textMedium,
                      size: 20.sp,
                    ),
                  ),
                  SizedBox(width: 12.w),
                  // Edit button (only for custom affirmations)
                  if (currentAffirmation.isCustom)
                    GestureDetector(
                      onTap: _showEditAffirmationDialog,
                      child: Icon(
                        Icons.edit_outlined,
                        color: AppColors.textMedium,
                        size: 20.sp,
                      ),
                    ),
                  if (currentAffirmation.isCustom) SizedBox(width: 12.w),
                  // Add new affirmation
                  GestureDetector(
                    onTap: _showAddAffirmationDialog,
                    child: Icon(
                      Icons.add_circle_outline,
                      color: currentCategory.color,
                      size: 20.sp,
                    ),
                  ),
                ],
              ),
            ],
          ),
          
          // Larger spacing to push text to true center
          SizedBox(height: 160.h),
          
          // Main affirmation text (centered)
          Text(
            currentAffirmation.text,
            textAlign: TextAlign.center,
            style: TextStyle(
              fontSize: 22.sp,
              fontWeight: FontWeight.w600,
              color: AppColors.textLight,
              height: 1.5,
            ),
          ),
          
          // Even larger spacing to push dots down further
          SizedBox(height: 140.h),
          
          // Navigation dots (at bottom)
          if (currentCategory.affirmations.length > 1)
            Row(
              mainAxisAlignment: MainAxisAlignment.center,
              children: List.generate(
                currentCategory.affirmations.length,
                (index) => GestureDetector(
                  onTap: () => _goToAffirmation(index),
                  child: Container(
                    margin: EdgeInsets.symmetric(horizontal: 6.w),
                    width: 10.w,
                    height: 10.h,
                    decoration: BoxDecoration(
                      shape: BoxShape.circle,
                      color: index == currentAffirmationIndex
                          ? currentCategory.color
                          : AppColors.textMedium.withOpacity(0.3),
                    ),
                  ),
                ),
              ),
            ),
          
          // Small buffer at bottom
          SizedBox(height: 20.h),
        ],
      ),
    );
  }

  Widget _buildPlayerControls() {
    return Padding(
      padding: EdgeInsets.symmetric(horizontal: 20.w),
      child: Row(
        mainAxisAlignment: MainAxisAlignment.spaceEvenly,
        children: [
          // Previous button
          _buildControlButton(
            icon: Icons.skip_previous,
            onTap: _previousAffirmation,
            isEnabled: currentAffirmationIndex > 0,
          ),
          
          // Play/Pause button
          Container(
            width: 70.w,
            height: 70.h,
            decoration: BoxDecoration(
              shape: BoxShape.circle,
              gradient: LinearGradient(
                colors: [
                  currentCategory.color,
                  currentCategory.color.withOpacity(0.8),
                ],
              ),
              boxShadow: [
                BoxShadow(
                  color: currentCategory.color.withOpacity(0.4),
                  blurRadius: 12.r,
                  offset: Offset(0, 4.h),
                ),
              ],
            ),
            child: IconButton(
              onPressed: _togglePlayPause,
              icon: Icon(
                isPlaying ? Icons.stop : Icons.play_arrow,
                color: AppColors.white,
                size: 32.sp,
              ),
            ),
          ),
          
          // Next button
          _buildControlButton(
            icon: Icons.skip_next,
            onTap: _nextAffirmation,
            isEnabled: currentAffirmationIndex < currentCategory.affirmations.length - 1,
          ),
        ],
      ),
    );
  }

  Widget _buildControlButton({
    required IconData icon,
    required VoidCallback onTap,
    required bool isEnabled,
  }) {
    return GestureDetector(
      onTap: isEnabled ? onTap : null,
      child: Container(
        width: 50.w,
        height: 50.h,
        decoration: BoxDecoration(
          shape: BoxShape.circle,
          color: AppColors.cardBackground,
          border: Border.all(
            color: isEnabled 
                ? AppColors.textMedium.withOpacity(0.3)
                : AppColors.textMedium.withOpacity(0.1),
          ),
        ),
        child: Icon(
          icon,
          color: isEnabled ? AppColors.textLight : AppColors.textMedium.withOpacity(0.5),
          size: 24.sp,
        ),
      ),
    );
  }

  // Category management
  void _selectCategory(int index) {
    setState(() {
      selectedCategoryIndex = index;
      currentAffirmationIndex = 0;
      isPlaying = false;
    });
    _ttsService.stop();
  }
void _showInfoSnackBar(String message) {
  ScaffoldMessenger.of(context).showSnackBar(
    SnackBar(
      content: Text(message),
      backgroundColor: AppColors.accentGold.withOpacity(0.8),
      behavior: SnackBarBehavior.floating,
      duration: Duration(seconds: 2),
    ),
  );
}

  void _showAddCategoryDialog() async {
    final result = await showDialog<AffirmationCategory>(
      context: context,
      builder: (context) => AddCategoryDialog(),
    );

    if (result != null) {
      await StorageService.addCategory(result);
      await _loadCategories();
      _showSuccessSnackBar('Category "${result.name}" created!');
    }
  }

  void _showCategoryOptions(AffirmationCategory category) {
    if (!category.isCustom) return;

    showModalBottomSheet(
      context: context,
      backgroundColor: AppColors.cardBackground,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.vertical(top: Radius.circular(20.r)),
      ),
      builder: (context) => Container(
        padding: EdgeInsets.all(20.w),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            ListTile(
              leading: Icon(Icons.edit, color: AppColors.textLight),
              title: Text('Edit Category', style: TextStyle(color: AppColors.textLight)),
              onTap: () {
                Navigator.pop(context);
                _showEditCategoryDialog(category);
              },
            ),
            ListTile(
              leading: Icon(Icons.delete, color: Colors.red),
              title: Text('Delete Category', style: TextStyle(color: Colors.red)),
              onTap: () {
                Navigator.pop(context);
                _confirmDeleteCategory(category);
              },
            ),
          ],
        ),
      ),
    );
  }

  void _showEditCategoryDialog(AffirmationCategory category) async {
    final result = await showDialog<AffirmationCategory>(
      context: context,
      builder: (context) => AddCategoryDialog(categoryToEdit: category),
    );

    if (result != null) {
      await StorageService.updateCategory(result);
      await _loadCategories();
      _showSuccessSnackBar('Category updated!');
    }
  }

  void _confirmDeleteCategory(AffirmationCategory category) {
    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        backgroundColor: AppColors.cardBackground,
        title: Text('Delete Category', style: TextStyle(color: AppColors.textLight)),
        content: Text(
          'Are you sure you want to delete "${category.name}"? This will also delete all affirmations in this category.',
          style: TextStyle(color: AppColors.textMedium),
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context),
            child: Text('Cancel', style: TextStyle(color: AppColors.textMedium)),
          ),
          TextButton(
            onPressed: () async {
              Navigator.pop(context);
              await StorageService.deleteCategory(category.id);
              await _loadCategories();
              if (categories.isNotEmpty && selectedCategoryIndex >= categories.length) {
                selectedCategoryIndex = 0;
              }
              _showSuccessSnackBar('Category deleted');
            },
            child: Text('Delete', style: TextStyle(color: Colors.red)),
          ),
        ],
      ),
    );
  }

  // Affirmation management
  void _showAddAffirmationDialog() async {
    final result = await showDialog<Affirmation>(
      context: context,
      builder: (context) => AddAffirmationDialog(
        categoryId: currentCategory.id,
      ),
    );

    if (result != null) {
      await StorageService.addAffirmationToCategory(currentCategory.id, result);
      await _loadCategories();
      // Go to the new affirmation
      setState(() {
        currentAffirmationIndex = currentCategory.affirmations.length - 1;
      });
      _showSuccessSnackBar('Affirmation added!');
    }
  }

  void _showEditAffirmationDialog() async {
    final result = await showDialog<Affirmation>(
      context: context,
      builder: (context) => AddAffirmationDialog(
        categoryId: currentCategory.id,
        affirmationToEdit: currentAffirmation,
      ),
    );

    if (result != null) {
      await StorageService.updateAffirmation(currentCategory.id, result);
      await _loadCategories();
      _showSuccessSnackBar('Affirmation updated!');
    }
  }

  void _toggleFavorite() async {
    final updatedAffirmation = currentAffirmation.copyWith(
      isFavorite: !currentAffirmation.isFavorite,
    );
    await StorageService.updateAffirmation(currentCategory.id, updatedAffirmation);
    await _loadCategories();
  }

  // Navigation
  void _goToAffirmation(int index) {
    setState(() {
      currentAffirmationIndex = index;
      isPlaying = false;
    });
    _ttsService.stop();
  }

  void _previousAffirmation() {
    if (currentAffirmationIndex > 0) {
      setState(() {
        currentAffirmationIndex--;
        isPlaying = false;
      });
      _ttsService.stop();
    }
  }

  void _nextAffirmation() {
    if (currentAffirmationIndex < currentCategory.affirmations.length - 1) {
      setState(() {
        currentAffirmationIndex++;
        isPlaying = false;
      });
      _ttsService.stop();
    }
  }

  // TTS controls - Updated methods

// Replace the _togglePlayPause method in your daily_affirmations.dart with this:

void _togglePlayPause() async {
  print('Toggle play/pause clicked, current state: $isPlaying');
  
  if (isPlaying) {
    await _ttsService.stop();
    setState(() {
      isPlaying = false;
    });
    print('Stopped TTS');
  } else {
    try {
      print('Attempting to start TTS...');
      final success = await _ttsService.speak(currentAffirmation.text);
      
      if (success) {
        setState(() {
          isPlaying = true;
        });
        print('Started TTS successfully');
        
        // Start checking status
        _startTTSStatusCheck();
      } else {
        print('Failed to start TTS');
        setState(() {
          isPlaying = false;
        });
        _showErrorSnackBar('Unable to start speech');
      }
      
    } catch (e) {
      print('Error starting TTS: $e');
      setState(() {
        isPlaying = false;
      });
      _showErrorSnackBar('Speech error occurred');
    }
  }
}


void _startTTSStatusCheck() {
  // Use a more efficient approach with periodic timer
  Timer.periodic(Duration(milliseconds: 250), (timer) {
    if (!mounted) {
      timer.cancel();
      return;
    }
    
    final currentlyPlaying = _ttsService.isPlaying;
    
    // If state changed, update UI
    if (currentlyPlaying != isPlaying) {
      setState(() {
        isPlaying = currentlyPlaying;
      });
    }
    
    // Stop checking if not playing
    if (!currentlyPlaying) {
      timer.cancel();
    }
  });
}


  // UI helpers
  void _showSuccessSnackBar(String message) {
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text(message),
        backgroundColor: AppColors.accentMint,
        behavior: SnackBarBehavior.floating,
      ),
    );
  }

  void _showErrorSnackBar(String message) {
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text(message),
        backgroundColor: Colors.red,
        behavior: SnackBarBehavior.floating,
      ),
    );
  }

  void _goToFavorites() {
    Navigator.push(
      context,
      MaterialPageRoute(
        builder: (context) => FavoritesScreen(),
      ),
    );
  }

  void _showTTSSettings() {
    showModalBottomSheet(
      context: context,
      backgroundColor: AppColors.cardBackground,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.vertical(top: Radius.circular(20.r)),
      ),
      builder: (context) => TTSSettingsSheet(ttsService: _ttsService),
    );
  }

  @override
  void dispose() {
    _ttsService.stop();
    super.dispose();
  }
}

class TTSSettingsSheet extends StatefulWidget {
  final HybridVoiceService ttsService;

  const TTSSettingsSheet({Key? key, required this.ttsService}) : super(key: key);

  @override
  _TTSSettingsSheetState createState() => _TTSSettingsSheetState();
}

class _TTSSettingsSheetState extends State<TTSSettingsSheet> {
  late double speechRate;
  late double volume;
  late double pitch;
  String selectedVoice = '';
  List<String> availableVoices = [];

  @override
  void initState() {
    super.initState();
    speechRate = widget.ttsService.speechRate;
    volume = widget.ttsService.volume;
    pitch = widget.ttsService.pitch;
    selectedVoice = widget.ttsService.selectedVoice;
    availableVoices = widget.ttsService.availableVoices;
  }

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: EdgeInsets.all(20.w),
      child: Column(
        mainAxisSize: MainAxisSize.min,
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          // Header
          Row(
            children: [
              Text(
                'Voice Settings',
                style: TextStyle(
                  fontSize: 20.sp,
                  fontWeight: FontWeight.bold,
                  color: AppColors.textLight,
                ),
              ),
              Spacer(),
              GestureDetector(
                onTap: () => Navigator.pop(context),
                child: Icon(
                  Icons.close,
                  color: AppColors.textMedium,
                  size: 24.sp,
                ),
              ),
            ],
          ),
          
          SizedBox(height: 24.h),
          
          // Voice selection
          if (availableVoices.isNotEmpty) ...[
            Text(
              'Voice',
              style: TextStyle(
                fontSize: 16.sp,
                fontWeight: FontWeight.w600,
                color: AppColors.textLight,
              ),
            ),
            SizedBox(height: 12.h),
            Container(
              height: 100.h,
              child: ListView.builder(
                scrollDirection: Axis.horizontal,
                itemCount: availableVoices.length > 5 ? 5 : availableVoices.length,
                itemBuilder: (context, index) {
                  final voice = availableVoices[index];
                  final isSelected = voice == selectedVoice;
                  final quality = widget.ttsService.getVoiceQuality(voice);
                  
                  return GestureDetector(
                    onTap: () => _selectVoice(voice),
                    child: Container(
                      width: 150.w,
                      margin: EdgeInsets.only(right: 12.w),
                      padding: EdgeInsets.all(12.w),
                      decoration: BoxDecoration(
                        color: isSelected 
                            ? AppColors.accentGold.withOpacity(0.2)
                            : AppColors.backgroundMedium,
                        borderRadius: BorderRadius.circular(12.r),
                        border: Border.all(
                          color: isSelected 
                              ? AppColors.accentGold
                              : quality == 'Human + AI Hybrid'
                                  ? AppColors.affirmationGradient[0].withOpacity(0.8)
                                  : Colors.transparent,
                          width: quality == 'Human + AI Hybrid' ? 3 : 1,
                        ),
                        gradient: quality == 'Human + AI Hybrid' 
                            ? LinearGradient(
                                colors: [
                                  AppColors.affirmationGradient[0].withOpacity(0.3),
                                  AppColors.visionBoardGradient[1].withOpacity(0.3),
                                ],
                                begin: Alignment.topLeft,
                                end: Alignment.bottomRight,
                              )
                            : null,
                      ),
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          Row(
                            children: [
                              if (quality == 'Ultra Human')
                                Icon(
                                  Icons.psychology,
                                  size: 14.sp,
                                  color: AppColors.affirmationGradient[0],
                                ),
                              if (quality == 'Ultra Human') SizedBox(width: 4.w),
                              if (quality == 'Very Human')
                                Icon(
                                  Icons.record_voice_over,
                                  size: 14.sp,
                                  color: AppColors.visionBoardGradient[0],
                                ),
                              if (quality == 'Very Human') SizedBox(width: 4.w),
                              Expanded(
                                child: Text(
                                  voice.length > 12 ? '${voice.substring(0, 12)}...' : voice,
                                  style: TextStyle(
                                    fontSize: 11.sp,
                                    color: AppColors.textLight,
                                    fontWeight: quality == 'Ultra Human' || quality == 'Very Human'
                                        ? FontWeight.w600 
                                        : FontWeight.w500,
                                  ),
                                ),
                              ),
                            ],
                          ),
                          SizedBox(height: 4.h),
                          Text(
                            quality,
                            style: TextStyle(
                              fontSize: 9.sp,
                              color: quality == 'Ultra Human' 
                                  ? AppColors.affirmationGradient[0]
                                  : quality == 'Very Human'
                                      ? AppColors.visionBoardGradient[0]
                                      : quality == 'Premium' 
                                          ? AppColors.accentGold
                                          : AppColors.textMedium,
                              fontWeight: quality == 'Ultra Human' || quality == 'Very Human'
                                  ? FontWeight.w600 
                                  : FontWeight.normal,
                            ),
                          ),
                          if (quality == 'Ultra Human')
                            Padding(
                              padding: EdgeInsets.only(top: 2.h),
                              child: Text(
                                'Sounds Like Human',
                                style: TextStyle(
                                  fontSize: 8.sp,
                                  color: AppColors.affirmationGradient[0],
                                  fontWeight: FontWeight.w600,
                                ),
                              ),
                            ),
                          if (quality == 'Very Human')
                            Padding(
                              padding: EdgeInsets.only(top: 2.h),
                              child: Text(
                                'Natural Speech',
                                style: TextStyle(
                                  fontSize: 8.sp,
                                  color: AppColors.visionBoardGradient[0],
                                  fontWeight: FontWeight.w500,
                                ),
                              ),
                            ),
                        ],
                      ),
                    ),
                  );
                },
              ),
            ),
            SizedBox(height: 24.h),
          ],
          
          // Speech Rate
          _buildSlider(
            'Speech Rate',
            speechRate,
            0.1,
            2.0,
            (value) {
              setState(() {
                speechRate = value;
              });
              widget.ttsService.setRate(value);
            },
          ),
          
          SizedBox(height: 20.h),
          
          // Volume
          _buildSlider(
            'Volume',
            volume,
            0.0,
            1.0,
            (value) {
              setState(() {
                volume = value;
              });
              widget.ttsService.setVolume(value);
            },
          ),
          
          SizedBox(height: 20.h),
          
          // Pitch
          _buildSlider(
            'Pitch',
            pitch,
            0.5,
            2.0,
            (value) {
              setState(() {
                pitch = value;
              });
              widget.ttsService.setPitch(value);
            },
          ),
          
          SizedBox(height: 24.h),
          
          // Test button
          Container(
            width: double.infinity,
            height: 50.h,
            child: ElevatedButton(
              onPressed: _testVoice,
              style: ElevatedButton.styleFrom(
                backgroundColor: AppColors.accentGold,
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(12.r),
                ),
              ),
              child: Text(
                'Test Voice',
                style: TextStyle(
                  fontSize: 16.sp,
                  fontWeight: FontWeight.w600,
                  color: AppColors.white,
                ),
              ),
            ),
          ),
          
          SizedBox(height: 20.h),
        ],
      ),
    );
  }

  Widget _buildSlider(
    String title,
    double value,
    double min,
    double max,
    Function(double) onChanged,
  ) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            Text(
              title,
              style: TextStyle(
                fontSize: 16.sp,
                fontWeight: FontWeight.w600,
                color: AppColors.textLight,
              ),
            ),
            Text(
              value.toStringAsFixed(1),
              style: TextStyle(
                fontSize: 14.sp,
                color: AppColors.textMedium,
              ),
            ),
          ],
        ),
        SizedBox(height: 8.h),
        SliderTheme(
          data: SliderTheme.of(context).copyWith(
            activeTrackColor: AppColors.accentGold,
            inactiveTrackColor: AppColors.accentGold.withOpacity(0.3),
            thumbColor: AppColors.accentGold,
            overlayColor: AppColors.accentGold.withOpacity(0.2),
          ),
          child: Slider(
            value: value,
            min: min,
            max: max,
            onChanged: onChanged,
          ),
        ),
      ],
    );
  }

  void _selectVoice(String voice) {
    setState(() {
      selectedVoice = voice;
    });
    widget.ttsService.setVoice(voice);
  }

  void _testVoice() {
    widget.ttsService.speak('I am worthy of love, success, and happiness. My voice sounds beautiful and natural as I speak my truth.');
  }
}