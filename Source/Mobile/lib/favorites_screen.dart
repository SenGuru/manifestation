import 'dart:async';
import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:manifestation_app/affirmations.dart';
import 'package:manifestation_app/storage_service.dart';
import 'package:manifestation_app/human_tts_service.dart';
import 'colors.dart';

class FavoritesScreen extends StatefulWidget {
  @override
  _FavoritesScreenState createState() => _FavoritesScreenState();
}

class _FavoritesScreenState extends State<FavoritesScreen> {
  List<AffirmationCategory> allCategories = [];
  List<FavoriteAffirmation> favoriteAffirmations = [];
  bool isLoading = true;
  String? currentPlayingId;
  final HumanTTSService _ttsService = HumanTTSService();

  @override
  void initState() {
    super.initState();
    _loadFavorites();
    _initializeTTS();
  }

  Future<void> _initializeTTS() async {
    try {
      await _ttsService.initialize();
    } catch (e) {
      print('Failed to initialize TTS: $e');
    }
  }

  Future<void> _loadFavorites() async {
    setState(() {
      isLoading = true;
    });
    
    try {
      final categories = await StorageService.loadCategories();
      final favorites = <FavoriteAffirmation>[];
      
      for (var category in categories) {
        for (var affirmation in category.affirmations) {
          if (affirmation.isFavorite) {
            favorites.add(FavoriteAffirmation(
              affirmation: affirmation,
              categoryName: category.name,
              categoryColor: category.color,
              categoryId: category.id,
            ));
          }
        }
      }
      
      // Sort by most recently added to favorites
      favorites.sort((a, b) => b.affirmation.createdAt.compareTo(a.affirmation.createdAt));
      
      setState(() {
        allCategories = categories;
        favoriteAffirmations = favorites;
        isLoading = false;
      });
    } catch (e) {
      setState(() {
        isLoading = false;
      });
      _showErrorSnackBar('Failed to load favorites');
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
            Expanded(
              child: isLoading
                  ? Center(
                      child: CircularProgressIndicator(
                        valueColor: AlwaysStoppedAnimation<Color>(AppColors.accentGold),
                      ),
                    )
                  : favoriteAffirmations.isEmpty
                      ? _buildEmptyState()
                      : _buildFavoritesList(),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildHeader() {
    return Container(
      padding: EdgeInsets.all(20.w),
      decoration: BoxDecoration(
        gradient: LinearGradient(
          colors: [
            AppColors.backgroundDark,
            AppColors.backgroundDark.withOpacity(0.9),
          ],
          begin: Alignment.topCenter,
          end: Alignment.bottomCenter,
        ),
      ),
      child: Column(
        children: [
          Row(
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
                      'Favorite Affirmations',
                      style: TextStyle(
                        fontSize: 24.sp,
                        fontWeight: FontWeight.bold,
                        color: AppColors.textLight,
                      ),
                    ),
                    Text(
                      '${favoriteAffirmations.length} affirmations',
                      style: TextStyle(
                        fontSize: 14.sp,
                        color: AppColors.textMedium,
                      ),
                    ),
                  ],
                ),
              ),
            ],
          ),
          SizedBox(height: 20.h),
          // Stats row
          Row(
            children: [
              _buildStatCard(
                'Total Favorites',
                favoriteAffirmations.length.toString(),
                Icons.favorite,
                AppColors.affirmationGradient,
              ),
              SizedBox(width: 12.w),
              _buildStatCard(
                'Categories',
                _getUniqueCategoriesCount().toString(),
                Icons.category,
                AppColors.visionBoardGradient,
              ),
            ],
          ),
        ],
      ),
    );
  }

  Widget _buildStatCard(String title, String value, IconData icon, List<Color> gradient) {
    return Expanded(
      child: Container(
        padding: EdgeInsets.all(16.w),
        decoration: BoxDecoration(
          gradient: LinearGradient(colors: gradient),
          borderRadius: BorderRadius.circular(12.r),
        ),
        child: Column(
          children: [
            Icon(
              icon,
              color: AppColors.white,
              size: 20.sp,
            ),
            SizedBox(height: 8.h),
            Text(
              value,
              style: TextStyle(
                fontSize: 20.sp,
                fontWeight: FontWeight.bold,
                color: AppColors.white,
              ),
            ),
            Text(
              title,
              style: TextStyle(
                fontSize: 12.sp,
                color: AppColors.white.withOpacity(0.9),
              ),
            ),
          ],
        ),
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
              Icons.favorite_outline,
              color: AppColors.accentGold,
              size: 40.sp,
            ),
          ),
          SizedBox(height: 24.h),
          Text(
            'No Favorites Yet',
            style: TextStyle(
              fontSize: 20.sp,
              fontWeight: FontWeight.bold,
              color: AppColors.textLight,
            ),
          ),
          SizedBox(height: 8.h),
          Text(
            'Tap the heart icon on any affirmation\nto add it to your favorites',
            textAlign: TextAlign.center,
            style: TextStyle(
              fontSize: 14.sp,
              color: AppColors.textMedium,
              height: 1.4,
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildFavoritesList() {
    return ListView.builder(
      padding: EdgeInsets.symmetric(horizontal: 20.w, vertical: 10.h),
      itemCount: favoriteAffirmations.length,
      itemBuilder: (context, index) {
        final favorite = favoriteAffirmations[index];
        return _buildFavoriteItem(favorite, index);
      },
    );
  }

  Widget _buildFavoriteItem(FavoriteAffirmation favorite, int index) {
    final isPlaying = currentPlayingId == favorite.affirmation.id;
    
    return Container(
      margin: EdgeInsets.only(bottom: 16.h),
      decoration: BoxDecoration(
        color: AppColors.cardBackground,
        borderRadius: BorderRadius.circular(16.r),
        border: Border.all(
          color: favorite.categoryColor.withOpacity(0.3),
        ),
      ),
      child: Column(
        children: [
          // Header with category info
          Container(
            padding: EdgeInsets.all(16.w),
            child: Row(
              children: [
                Container(
                  width: 12.w,
                  height: 12.h,
                  decoration: BoxDecoration(
                    color: favorite.categoryColor,
                    shape: BoxShape.circle,
                  ),
                ),
                SizedBox(width: 8.w),
                Text(
                  favorite.categoryName,
                  style: TextStyle(
                    fontSize: 12.sp,
                    color: AppColors.textMedium,
                    fontWeight: FontWeight.w500,
                  ),
                ),
                Spacer(),
                Text(
                  '${index + 1}',
                  style: TextStyle(
                    fontSize: 12.sp,
                    color: AppColors.textMedium,
                  ),
                ),
              ],
            ),
          ),
          
          // Affirmation text
          Container(
            padding: EdgeInsets.symmetric(horizontal: 16.w, vertical: 8.h),
            child: Text(
              favorite.affirmation.text,
              style: TextStyle(
                fontSize: 16.sp,
                color: AppColors.textLight,
                height: 1.4,
                fontWeight: FontWeight.w500,
              ),
            ),
          ),
          
          // Control buttons
          Container(
            padding: EdgeInsets.all(16.w),
            child: Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                // Play button
                GestureDetector(
                  onTap: () => _togglePlay(favorite.affirmation),
                  child: Container(
                    width: 44.w,
                    height: 44.h,
                    decoration: BoxDecoration(
                      gradient: LinearGradient(
                        colors: [
                          favorite.categoryColor,
                          favorite.categoryColor.withOpacity(0.8),
                        ],
                      ),
                      shape: BoxShape.circle,
                    ),
                    child: Icon(
                      isPlaying ? Icons.stop : Icons.play_arrow,
                      color: AppColors.white,
                      size: 20.sp,
                    ),
                  ),
                ),
                
                // Action buttons
                Row(
                  children: [
                    // Remove from favorites
                    GestureDetector(
                      onTap: () => _removeFavorite(favorite),
                      child: Container(
                        width: 44.w,
                        height: 44.h,
                        decoration: BoxDecoration(
                          color: AppColors.backgroundMedium,
                          borderRadius: BorderRadius.circular(12.r),
                        ),
                        child: Icon(
                          Icons.favorite,
                          color: Colors.red,
                          size: 18.sp,
                        ),
                      ),
                    ),
                    SizedBox(width: 12.w),
                    // Share button
                    GestureDetector(
                      onTap: () => _shareAffirmation(favorite.affirmation),
                      child: Container(
                        width: 44.w,
                        height: 44.h,
                        decoration: BoxDecoration(
                          color: AppColors.backgroundMedium,
                          borderRadius: BorderRadius.circular(12.r),
                        ),
                        child: Icon(
                          Icons.share,
                          color: AppColors.textMedium,
                          size: 18.sp,
                        ),
                      ),
                    ),
                  ],
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Future<void> _togglePlay(Affirmation affirmation) async {
    if (currentPlayingId == affirmation.id && _ttsService.isPlaying) {
      await _ttsService.stop();
      setState(() {
        currentPlayingId = null;
      });
    } else {
      await _ttsService.stop();
      final success = await _ttsService.speak(affirmation.text);
      if (success) {
        setState(() {
          currentPlayingId = affirmation.id;
        });
        _startPlaybackTracking(affirmation.id);
      }
    }
  }

  void _startPlaybackTracking(String affirmationId) {
    Timer.periodic(Duration(milliseconds: 500), (timer) {
      if (!mounted) {
        timer.cancel();
        return;
      }
      
      if (!_ttsService.isPlaying) {
        setState(() {
          currentPlayingId = null;
        });
        timer.cancel();
      }
    });
  }

  Future<void> _removeFavorite(FavoriteAffirmation favorite) async {
    try {
      final updatedAffirmation = favorite.affirmation.copyWith(isFavorite: false);
      await StorageService.updateAffirmation(favorite.categoryId, updatedAffirmation);
      await _loadFavorites();
      _showSuccessSnackBar('Removed from favorites');
    } catch (e) {
      _showErrorSnackBar('Failed to remove favorite');
    }
  }

  void _shareAffirmation(Affirmation affirmation) {
    // TODO: Implement share functionality
    _showInfoSnackBar('Share functionality coming soon!');
  }

  int _getUniqueCategoriesCount() {
    return favoriteAffirmations.map((f) => f.categoryId).toSet().length;
  }

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

  void _showInfoSnackBar(String message) {
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text(message),
        backgroundColor: AppColors.accentGold,
        behavior: SnackBarBehavior.floating,
      ),
    );
  }

  @override
  void dispose() {
    _ttsService.stop();
    super.dispose();
  }
}

class FavoriteAffirmation {
  final Affirmation affirmation;
  final String categoryName;
  final Color categoryColor;
  final String categoryId;

  FavoriteAffirmation({
    required this.affirmation,
    required this.categoryName,
    required this.categoryColor,
    required this.categoryId,
  });
}