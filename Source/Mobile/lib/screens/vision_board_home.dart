import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'dart:io';
import '../models/vision_board.dart';
import '../services/vision_board_service.dart';
import '../services/guided_tour_service.dart';
import 'vision_board_screen.dart';
import '../constants/app_theme.dart';
import '../widgets/stylish_loading.dart';
import '../utils/navigation_utils.dart';

class VisionBoardHome extends StatefulWidget {
  const VisionBoardHome({super.key});

  @override
  _VisionBoardHomeState createState() => _VisionBoardHomeState();
}

class _VisionBoardHomeState extends State<VisionBoardHome> {
  List<VisionBoard> visionBoards = [];
  bool isLoading = false;
  final GlobalKey _addButtonKey = GlobalKey();

  @override
  void initState() {
    super.initState();
    _loadVisionBoards();
    
    // Show guided tour after the widget is built
    WidgetsBinding.instance.addPostFrameCallback((_) {
      _showGuidedTourIfNeeded();
    });
  }

  Future<void> _showGuidedTourIfNeeded() async {
    print('🎯 🚀 Vision Board: _showGuidedTourIfNeeded called');
    print('🎯 🚀 Vision Board context: $context');
    final tourService = GuidedTourService();
    print('🎯 🚀 Vision Board: About to call showTourIfNeeded');
    await tourService.showTourIfNeeded(
      context,
      GuidedTourService.visionBoardTourKey,
      tourService.getVisionBoardTourSteps(),
    );
    print('🎯 🚀 Vision Board: showTourIfNeeded completed');
  }


  Future<void> _loadVisionBoards() async {
    setState(() {
      isLoading = true;
    });
    
    try {
      // Initialize service
      final visionBoardService = VisionBoardService();
      await visionBoardService.initialize(baseUrl: 'http://manifestation-qa-1.senthilguru.net');
      
      // Load from server
      final result = await visionBoardService.getUserVisionBoards();
      
      if (result.isSuccess) {
        visionBoards = result.data ?? [];
      } else {
        print('❌ Failed to load vision boards: ${result.message}');
        visionBoards = [];
      }
    } catch (e) {
      print('❌ Error loading vision boards: $e');
      visionBoards = [];
    }
    
    setState(() {
      isLoading = false;
    });
  }
  
  

  @override
  Widget build(BuildContext context) {
    // Show loading screen while loading vision boards
    if (isLoading) {
      return Scaffold(
        backgroundColor: AppTheme.backgroundPrimary,
        body: StylishLoading(
          message: 'Loading your vision boards...',
        ),
      );
    }

    return Scaffold(
      backgroundColor: AppTheme.backgroundPrimary,
      body: SafeArea(
        child: Column(
          children: [
            _buildHeader(),
            Expanded(
              child: visionBoards.isEmpty
                  ? _buildEmptyState()
                  : _buildVisionBoardGrid(),
            ),
          ],
        ),
      ),
      floatingActionButton: _buildCreateButton(),
    );
  }

  Widget _buildHeader() {
    return Container(
      padding: EdgeInsets.all(20.w),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            children: [
              GestureDetector(
                onTap: () => Navigator.pop(context),
                child: Container(
                  width: 44.w,
                  height: 44.h,
                  decoration: BoxDecoration(
                    color: AppTheme.cardBackground,
                    borderRadius: BorderRadius.circular(12.r),
                    border: Border.all(
                      color: AppTheme.textSecondary.withValues(alpha: 0.1),
                    ),
                  ),
                  child: Icon(
                    Icons.arrow_back,
                    color: AppTheme.textPrimary,
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
                      'Vision Boards',
                      style: TextStyle(
                        fontSize: 24.sp,
                        fontWeight: FontWeight.bold,
                        color: AppTheme.textPrimary,
                      ),
                    ),
                    Text(
                      'Visualize your dreams',
                      style: TextStyle(
                        fontSize: 14.sp,
                        color: AppTheme.textSecondary,
                      ),
                    ),
                  ],
                ),
              ),
              _buildOptionsButton(),
            ],
          ),
          SizedBox(height: 20.h),
          _buildStatsRow(),
        ],
      ),
    );
  }

  Widget _buildOptionsButton() {
    return GestureDetector(
      onTap: _showOptionsMenu,
      child: Container(
        width: 44.w,
        height: 44.h,
        decoration: BoxDecoration(
          color: AppTheme.cardBackground,
          borderRadius: BorderRadius.circular(12.r),
          border: Border.all(
            color: AppTheme.textSecondary.withValues(alpha: 0.1),
          ),
        ),
        child: Icon(
          Icons.more_vert,
          color: AppTheme.textPrimary,
          size: 20.sp,
        ),
      ),
    );
  }

  Widget _buildStatsRow() {
    return Row(
      children: [
        _buildStatCard(
          icon: Icons.dashboard,
          title: 'Total Boards',
          value: '${visionBoards.length}',
          color: AppTheme.visionBoardGradient[0],
        ),
      ],
    );
  }

  Widget _buildStatCard({
    required IconData icon,
    required String title,
    required String value,
    required Color color,
  }) {
    return Expanded(
      child: Container(
        padding: EdgeInsets.all(16.w),
        decoration: BoxDecoration(
          color: AppTheme.cardBackground,
          borderRadius: BorderRadius.circular(16.r),
          border: Border.all(
            color: AppTheme.textSecondary.withValues(alpha: 0.1),
          ),
        ),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Container(
              width: 32.w,
              height: 32.h,
              decoration: BoxDecoration(
                color: color.withValues(alpha: 0.1),
                borderRadius: BorderRadius.circular(8.r),
              ),
              child: Icon(
                icon,
                color: color,
                size: 16.sp,
              ),
            ),
            SizedBox(height: 8.h),
            Text(
              value,
              style: TextStyle(
                fontSize: 20.sp,
                fontWeight: FontWeight.bold,
                color: AppTheme.textPrimary,
              ),
            ),
            Text(
              title,
              style: TextStyle(
                fontSize: 12.sp,
                color: AppTheme.textSecondary,
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
            width: 120.w,
            height: 120.h,
            decoration: BoxDecoration(
              gradient: LinearGradient(
                colors: AppTheme.visionBoardGradient,
                begin: Alignment.topLeft,
                end: Alignment.bottomRight,
              ),
              borderRadius: BorderRadius.circular(60.r),
            ),
            child: Icon(
              Icons.dashboard,
              color: AppTheme.white,
              size: 60.sp,
            ),
          ),
          SizedBox(height: 24.h),
          Text(
            'No Vision Boards Yet',
            style: TextStyle(
              fontSize: 24.sp,
              fontWeight: FontWeight.bold,
              color: AppTheme.textPrimary,
            ),
          ),
          SizedBox(height: 8.h),
          Text(
            'Create your first vision board to start\nmanifesting your dreams',
            style: TextStyle(
              fontSize: 16.sp,
              color: AppTheme.textSecondary,
              height: 1.5,
            ),
            textAlign: TextAlign.center,
          ),
          SizedBox(height: 32.h),
          _buildCreateBoardButton(),
        ],
      ),
    );
  }

  Widget _buildVisionBoardGrid() {
    return SingleChildScrollView(
      padding: EdgeInsets.symmetric(horizontal: 20.w),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          _buildSectionHeader('Recent', 'View all'),
          SizedBox(height: 16.h),
          GridView.builder(
            shrinkWrap: true,
            physics: NeverScrollableScrollPhysics(),
            gridDelegate: SliverGridDelegateWithFixedCrossAxisCount(
              crossAxisCount: 2,
              crossAxisSpacing: 16.w,
              mainAxisSpacing: 16.h,
              childAspectRatio: 0.75,
            ),
            itemCount: visionBoards.length,
            itemBuilder: (context, index) {
              final board = visionBoards[index];
              return _buildVisionBoardCard(board);
            },
          ),
          SizedBox(height: 32.h),
          _buildTemplatesSection(),
          SizedBox(height: 100.h), // Space for FAB
        ],
      ),
    );
  }

  Widget _buildSectionHeader(String title, String action) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.spaceBetween,
      children: [
        Text(
          title,
          style: TextStyle(
            fontSize: 20.sp,
            fontWeight: FontWeight.bold,
            color: AppTheme.textPrimary,
          ),
        ),
        GestureDetector(
          onTap: () {
            // TODO: Navigate to all boards
          },
          child: Text(
            action,
            style: TextStyle(
              fontSize: 14.sp,
              color: AppTheme.visionBoardGradient[0],
              fontWeight: FontWeight.w600,
            ),
          ),
        ),
      ],
    );
  }

  Widget _buildVisionBoardCard(VisionBoard board) {
    return GestureDetector(
      onTap: () => _openBoard(board),
      onLongPress: () => _showBoardOptions(board),
      child: Container(
        decoration: BoxDecoration(
          color: AppTheme.cardBackground,
          borderRadius: BorderRadius.circular(20.r),
          border: Border.all(
            color: AppTheme.textSecondary.withValues(alpha: 0.1),
          ),
          boxShadow: [
            BoxShadow(
              color: AppTheme.black.withValues(alpha: 0.05),
              blurRadius: 10.r,
              offset: Offset(0, 5.h),
            ),
          ],
        ),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Expanded(
              flex: 3,
              child: Container(
                width: double.infinity,
                decoration: BoxDecoration(
                  gradient: LinearGradient(
                    colors: [
                      AppTheme.visionBoardGradient[0].withValues(alpha: 0.1),
                      AppTheme.visionBoardGradient[1].withValues(alpha: 0.05),
                    ],
                    begin: Alignment.topLeft,
                    end: Alignment.bottomRight,
                  ),
                  borderRadius: BorderRadius.vertical(top: Radius.circular(20.r)),
                ),
                child: _buildVisionBoardPreview(board),
              ),
            ),
            Expanded(
              flex: 1,
              child: Padding(
                padding: EdgeInsets.all(6.w),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    Text(
                      board.title.isEmpty ? 'Untitled Vision Board' : board.title,
                      style: TextStyle(
                        fontSize: 16.sp,
                        fontWeight: FontWeight.bold,
                        color: AppTheme.textPrimary,
                      ),
                      maxLines: 1,
                      overflow: TextOverflow.ellipsis,
                    ),
                    SizedBox(height: 6.h),
                    Row(
                      children: [
                        Icon(
                          Icons.schedule,
                          color: AppTheme.textSecondary.withValues(alpha: 0.7),
                          size: 12.sp,
                        ),
                        SizedBox(width: 4.w),
                        Text(
                          _formatDate(board.updatedAt),
                          style: TextStyle(
                            fontSize: 11.sp,
                            color: AppTheme.textSecondary.withValues(alpha: 0.7),
                            fontWeight: FontWeight.w500,
                          ),
                        ),
                      ],
                    ),
                  ],
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildPlaceholderThumbnail() {
    return Container(
      width: double.infinity,
      height: double.infinity,
      decoration: BoxDecoration(
        color: AppTheme.cardBackground,
        borderRadius: BorderRadius.vertical(top: Radius.circular(20.r)),
      ),
      child: Container(), // Completely empty - shows nothing
    );
  }

  Widget _buildVisionBoardPreview(VisionBoard board) {
    // Check if we have a thumbnail URL from the server
    if (board.thumbnailUrl != null && board.thumbnailUrl!.isNotEmpty) {
      return _buildThumbnailFromUrl(board.thumbnailUrl!);
    }
    
    // If no thumbnail, just show empty placeholder - don't try to render elements
    return _buildPlaceholderThumbnail();
  }
  
  Widget _buildPreviewElement(VisionBoardElement element, VisionBoard board) {
    // Scale factor for miniature version (make elements smaller)
    const double scaleFactor = 0.6;
    final double previewWidth = 160.w; // Approximate card width
    final double previewHeight = 120.h; // Approximate card height
    
    // Scale position and size to fit in preview
    final scaledX = (element.position.dx / 400) * previewWidth * scaleFactor;
    final scaledY = (element.position.dy / 600) * previewHeight * scaleFactor;
    final scaledWidth = (element.size.width / 400) * previewWidth * scaleFactor;
    final scaledHeight = (element.size.height / 600) * previewHeight * scaleFactor;
    
    return Positioned(
      left: scaledX.clamp(0.0, previewWidth - scaledWidth),
      top: scaledY.clamp(0.0, previewHeight - scaledHeight),
      child: Container(
        width: scaledWidth.clamp(20.w, previewWidth),
        height: scaledHeight.clamp(20.h, previewHeight),
        child: Transform.rotate(
          angle: element.rotation * 0.5, // Reduce rotation for preview
          child: _buildPreviewElementContent(element),
        ),
      ),
    );
  }
  
  Widget _buildPreviewElementContent(VisionBoardElement element) {
    if (element is VisionBoardImageElement) {
      return Container(
        decoration: BoxDecoration(
          color: AppTheme.white,
          borderRadius: BorderRadius.circular(4.r),
          border: Border.all(
            color: AppTheme.textSecondary.withValues(alpha: 0.2),
            width: 0.5,
          ),
        ),
        child: ClipRRect(
          borderRadius: BorderRadius.circular(4.r),
          child: element.imagePath.isNotEmpty
              ? Image.file(
                  File(element.imagePath),
                  fit: BoxFit.cover,
                  errorBuilder: (context, error, stackTrace) => Icon(
                    Icons.image,
                    color: AppTheme.textSecondary.withValues(alpha: 0.4),
                    size: 12.sp,
                  ),
                )
              : Icon(
                  Icons.image,
                  color: AppTheme.textSecondary.withValues(alpha: 0.4),
                  size: 12.sp,
                ),
        ),
      );
    } else if (element is VisionBoardTextElement) {
      return Container(
        padding: EdgeInsets.all(2.w),
        decoration: BoxDecoration(
          color: element.style.color?.withValues(alpha: 0.1) ?? AppTheme.white.withValues(alpha: 0.8),
          borderRadius: BorderRadius.circular(2.r),
          border: Border.all(
            color: element.style.color?.withValues(alpha: 0.3) ?? AppTheme.textSecondary.withValues(alpha: 0.2),
            width: 0.5,
          ),
        ),
        child: Text(
          element.text.length > 20 ? '${element.text.substring(0, 20)}...' : element.text,
          style: TextStyle(
            fontSize: 6.sp,
            color: element.style.color ?? AppTheme.textPrimary,
            fontWeight: FontWeight.w600,
          ),
          maxLines: 2,
          overflow: TextOverflow.ellipsis,
        ),
      );
    }
    
    return SizedBox.shrink();
  }

  Widget _buildThumbnailFromUrl(String thumbnailUrl) {
    return Container(
      width: double.infinity,
      height: double.infinity,
      child: ClipRRect(
        borderRadius: BorderRadius.vertical(top: Radius.circular(20.r)),
        child: Image.network(
          thumbnailUrl,
          fit: BoxFit.cover,
          loadingBuilder: (context, child, loadingProgress) {
            if (loadingProgress == null) return child;
            return Center(
              child: CircularProgressIndicator(
                value: loadingProgress.expectedTotalBytes != null
                    ? loadingProgress.cumulativeBytesLoaded /
                        loadingProgress.expectedTotalBytes!
                    : null,
                valueColor: AlwaysStoppedAnimation<Color>(
                  AppTheme.visionBoardGradient[0],
                ),
              ),
            );
          },
          errorBuilder: (context, error, stackTrace) {
            print('❌ Failed to load thumbnail: $error');
            // Fallback to placeholder if thumbnail fails to load
            return _buildPlaceholderThumbnail();
          },
        ),
      ),
    );
  }

  Widget _buildTemplatesSection() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        _buildSectionHeader('Templates', 'Browse all'),
        SizedBox(height: 16.h),
        Container(
          height: 120.h,
          child: ListView.builder(
            scrollDirection: Axis.horizontal,
            itemCount: 5,
            itemBuilder: (context, index) {
              return Container(
                width: 160.w,
                margin: EdgeInsets.only(right: 16.w),
                child: _buildTemplateCard(index),
              );
            },
          ),
        ),
      ],
    );
  }

  Widget _buildTemplateCard(int index) {
    final templates = [
      {'name': 'Dream Home', 'icon': Icons.home, 'color': AppTheme.accentGold},
      {'name': 'Career Goals', 'icon': Icons.work, 'color': AppTheme.visionBoardGradient[0]},
      {'name': 'Travel Dreams', 'icon': Icons.flight, 'color': AppTheme.journalGradient[0]},
      {'name': 'Relationships', 'icon': Icons.favorite, 'color': AppTheme.accentCoral},
      {'name': 'Health & Fitness', 'icon': Icons.fitness_center, 'color': AppTheme.accentMint},
    ];

    final template = templates[index];
    
    return GestureDetector(
      onTap: () => _createFromTemplate(template['name'] as String),
      child: Container(
        decoration: BoxDecoration(
          color: AppTheme.cardBackground,
          borderRadius: BorderRadius.circular(16.r),
          border: Border.all(
            color: AppTheme.textSecondary.withValues(alpha: 0.1),
          ),
        ),
        child: Column(
          children: [
            Expanded(
              child: Container(
                width: double.infinity,
                decoration: BoxDecoration(
                  color: (template['color'] as Color).withValues(alpha: 0.1),
                  borderRadius: BorderRadius.vertical(top: Radius.circular(16.r)),
                ),
                child: Icon(
                  template['icon'] as IconData,
                  color: template['color'] as Color,
                  size: 32.sp,
                ),
              ),
            ),
            Padding(
              padding: EdgeInsets.all(12.w),
              child: Text(
                template['name'] as String,
                style: TextStyle(
                  fontSize: 12.sp,
                  fontWeight: FontWeight.w600,
                  color: AppTheme.textPrimary,
                ),
                textAlign: TextAlign.center,
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildCreateButton() {
    return Container(
      width: 56.w,
      height: 56.h,
      decoration: BoxDecoration(
        gradient: LinearGradient(
          colors: AppTheme.visionBoardGradient,
          begin: Alignment.topLeft,
          end: Alignment.bottomRight,
        ),
        borderRadius: BorderRadius.circular(28.r),
        boxShadow: [
          BoxShadow(
            color: AppTheme.visionBoardGradient[0].withValues(alpha: 0.4),
            blurRadius: 20.r,
            offset: Offset(0, 10.h),
          ),
        ],
      ),
      child: Material(
        color: Colors.transparent,
        child: InkWell(
          onTap: _createNewBoard,
          borderRadius: BorderRadius.circular(28.r),
          child: Icon(
            Icons.add,
            color: AppTheme.white,
            size: 28.sp,
          ),
        ),
      ),
    );
  }

  Widget _buildCreateBoardButton() {
    return Container(
      width: 200.w,
      height: 50.h,
      decoration: BoxDecoration(
        gradient: LinearGradient(
          colors: AppTheme.visionBoardGradient,
          begin: Alignment.topLeft,
          end: Alignment.bottomRight,
        ),
        borderRadius: BorderRadius.circular(25.r),
        boxShadow: [
          BoxShadow(
            color: AppTheme.visionBoardGradient[0].withValues(alpha: 0.4),
            blurRadius: 20.r,
            offset: Offset(0, 10.h),
          ),
        ],
      ),
      child: Material(
        color: Colors.transparent,
        child: InkWell(
          onTap: _createNewBoard,
          borderRadius: BorderRadius.circular(25.r),
          child: Row(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              Icon(
                Icons.add,
                color: AppTheme.white,
                size: 20.sp,
              ),
              SizedBox(width: 8.w),
              Text(
                'Create Vision Board',
                style: TextStyle(
                  fontSize: 14.sp,
                  fontWeight: FontWeight.w600,
                  color: AppTheme.white,
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }

  String _formatDate(DateTime date) {
    final now = DateTime.now();
    final difference = now.difference(date);
    
    if (difference.inDays == 0) {
      return 'Today';
    } else if (difference.inDays == 1) {
      return 'Yesterday';
    } else if (difference.inDays < 7) {
      return '${difference.inDays} days ago';
    } else {
      return '${date.day}/${date.month}/${date.year}';
    }
  }

  void _createNewBoard() async {
    // Create a new empty board with a generated ID
    final newBoard = VisionBoard(
      id: 'board_${DateTime.now().millisecondsSinceEpoch}',
      title: '', // Start with empty title - user can edit in the editor
      elements: [],
      backgroundColor: AppTheme.white,
      createdAt: DateTime.now(),
      updatedAt: DateTime.now(),
    );
    
    pushWithoutSwipe<VisionBoard>(
      context,
      VisionBoardScreen(existingBoard: newBoard),
    ).then((result) {
      // Reload vision boards from server after editing
      _loadVisionBoards();
    });
  }

  void _openBoard(VisionBoard board) {
    pushWithoutSwipe<VisionBoard>(
      context,
      VisionBoardScreen(existingBoard: board),
    ).then((result) {
      if (result != null) {
        // Update the specific board in the list
        setState(() {
          final index = visionBoards.indexWhere((b) => b.id == result.id);
          if (index != -1) {
            visionBoards[index] = result;
          }
        });
      }
    });
  }

  void _createFromTemplate(String templateName) {
    // TODO: Implement template creation
    print('Creating from template: $templateName');
    _createNewBoard();
  }

  void _showBoardOptions(VisionBoard board) {
    showModalBottomSheet(
      context: context,
      backgroundColor: Colors.transparent,
      builder: (context) => Container(
        margin: EdgeInsets.all(20.w),
        decoration: BoxDecoration(
          color: AppTheme.cardBackground,
          borderRadius: BorderRadius.circular(20.r),
        ),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            Container(
              width: 40.w,
              height: 4.h,
              margin: EdgeInsets.only(top: 12.h),
              decoration: BoxDecoration(
                color: AppTheme.textSecondary.withValues(alpha: 0.3),
                borderRadius: BorderRadius.circular(2.r),
              ),
            ),
            Padding(
              padding: EdgeInsets.all(24.w),
              child: Column(
                children: [
                  Text(
                    board.title.isEmpty ? 'Untitled Vision Board' : board.title,
                    style: TextStyle(
                      fontSize: 18.sp,
                      fontWeight: FontWeight.bold,
                      color: AppTheme.textPrimary,
                    ),
                  ),
                  SizedBox(height: 24.h),
                  _buildOptionItem(Icons.edit, 'Edit', () {
                    Navigator.pop(context);
                    _openBoard(board);
                  }),
                  _buildOptionItem(Icons.text_fields, 'Rename', () {
                    Navigator.pop(context);
                    _renameBoard(board);
                  }),
                  _buildOptionItem(Icons.copy, 'Duplicate', () {
                    Navigator.pop(context);
                    _duplicateBoard(board);
                  }),
                  _buildOptionItem(Icons.share, 'Share', () {
                    Navigator.pop(context);
                    _shareBoard(board);
                  }),
                  _buildOptionItem(Icons.delete, 'Delete', () {
                    Navigator.pop(context);
                    _deleteBoard(board);
                  }, isDestructive: true),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildOptionItem(IconData icon, String title, VoidCallback onTap, {bool isDestructive = false}) {
    return GestureDetector(
      onTap: onTap,
      child: Container(
        width: double.infinity,
        padding: EdgeInsets.symmetric(vertical: 16.h),
        child: Row(
          children: [
            Icon(
              icon,
              color: isDestructive ? Colors.red : AppTheme.textSecondary,
              size: 20.sp,
            ),
            SizedBox(width: 16.w),
            Text(
              title,
              style: TextStyle(
                fontSize: 16.sp,
                color: isDestructive ? Colors.red : AppTheme.textPrimary,
              ),
            ),
          ],
        ),
      ),
    );
  }

  void _showOptionsMenu() {
    showModalBottomSheet(
      context: context,
      backgroundColor: Colors.transparent,
      builder: (context) => Container(
        margin: EdgeInsets.all(20.w),
        decoration: BoxDecoration(
          color: AppTheme.cardBackground,
          borderRadius: BorderRadius.circular(20.r),
        ),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            Container(
              width: 40.w,
              height: 4.h,
              margin: EdgeInsets.only(top: 12.h),
              decoration: BoxDecoration(
                color: AppTheme.textSecondary.withValues(alpha: 0.3),
                borderRadius: BorderRadius.circular(2.r),
              ),
            ),
            Padding(
              padding: EdgeInsets.all(24.w),
              child: Column(
                children: [
                  Text(
                    'Options',
                    style: TextStyle(
                      fontSize: 18.sp,
                      fontWeight: FontWeight.bold,
                      color: AppTheme.textPrimary,
                    ),
                  ),
                  SizedBox(height: 24.h),
                  _buildOptionItem(Icons.sort, 'Sort & Filter', () {
                    Navigator.pop(context);
                    _showSortOptions();
                  }),
                  // Cloud sync options disabled - API endpoints pending
                  // _buildOptionItem(Icons.cloud_sync, 'Sync Boards', () {
                  //   Navigator.pop(context);
                  //   _syncBoards();
                  // }),
                  // _buildOptionItem(Icons.upload, 'Upload to Cloud', () {
                  //   Navigator.pop(context);
                  //   _uploadToCloud();
                  // }),
                  // _buildOptionItem(Icons.download, 'Download from Cloud', () {
                  //   Navigator.pop(context);
                  //   _downloadFromCloud();
                  // }),
                  _buildOptionItem(Icons.settings, 'Settings', () {
                    Navigator.pop(context);
                    _showSettings();
                  }),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }

  void _duplicateBoard(VisionBoard board) {
    // TODO: Implement board duplication
    print('Duplicating board: ${board.title}');
  }

  void _shareBoard(VisionBoard board) {
    // TODO: Implement board sharing
    print('Sharing board: ${board.title}');
  }

  void _renameBoard(VisionBoard board) async {
    final TextEditingController nameController = TextEditingController();
    nameController.text = board.title; // Pre-populate with current name
    
    final newName = await showDialog<String>(
      context: context,
      builder: (context) => AlertDialog(
        backgroundColor: AppTheme.cardBackground,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(20.r),
        ),
        title: Text(
          'Rename Vision Board',
          style: TextStyle(
            fontSize: 20.sp,
            fontWeight: FontWeight.bold,
            color: AppTheme.textPrimary,
          ),
        ),
        content: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            Text(
              'Enter a new name for your vision board',
              style: TextStyle(
                fontSize: 14.sp,
                color: AppTheme.textSecondary,
              ),
            ),
            SizedBox(height: 16.h),
            TextField(
              controller: nameController,
              autofocus: true,
              decoration: InputDecoration(
                hintText: 'Vision board name',
                hintStyle: TextStyle(
                  color: AppTheme.textTertiary,
                  fontSize: 14.sp,
                ),
                filled: true,
                fillColor: AppTheme.surfaceElevated,
                border: OutlineInputBorder(
                  borderRadius: BorderRadius.circular(12.r),
                  borderSide: BorderSide.none,
                ),
                contentPadding: EdgeInsets.symmetric(
                  horizontal: 16.w,
                  vertical: 12.h,
                ),
              ),
              style: TextStyle(
                fontSize: 16.sp,
                color: AppTheme.textPrimary,
              ),
              onSubmitted: (value) {
                final name = value.trim();
                Navigator.pop(context, name.isEmpty ? board.title : name);
              },
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
                fontSize: 14.sp,
              ),
            ),
          ),
          TextButton(
            onPressed: () {
              final name = nameController.text.trim();
              Navigator.pop(context, name.isEmpty ? board.title : name);
            },
            child: Text(
              'Rename',
              style: TextStyle(
                color: AppTheme.affirmationGradient.first,
                fontSize: 14.sp,
                fontWeight: FontWeight.w600,
              ),
            ),
          ),
        ],
      ),
    );
    
    // If user cancelled or name is the same, don't update
    if (newName == null || newName == board.title) return;
    
    try {
      // Update the board with the new name
      final updatedBoard = board.copyWith(title: newName);
      
      // Update in local state
      setState(() {
        final index = visionBoards.indexWhere((b) => b.id == board.id);
        if (index != -1) {
          visionBoards[index] = updatedBoard;
        }
      });
      
      // TODO: Update on server via API
      // For now, we just update locally
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Vision board renamed to "$newName"'),
          backgroundColor: AppTheme.accentMint,
          duration: Duration(seconds: 2),
        ),
      );
      
    } catch (e) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Failed to rename vision board: $e'),
          backgroundColor: Colors.red,
          duration: Duration(seconds: 3),
        ),
      );
    }
  }

  void _deleteBoard(VisionBoard board) {
    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        backgroundColor: AppTheme.cardBackground,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(16.r),
        ),
        title: Text(
          'Delete Vision Board',
          style: TextStyle(
            color: AppTheme.textPrimary,
            fontSize: 18.sp,
            fontWeight: FontWeight.bold,
          ),
        ),
        content: Text(
          'Are you sure you want to delete "${board.title.isEmpty ? 'this vision board' : board.title}"? This action cannot be undone.',
          style: TextStyle(
            color: AppTheme.textSecondary,
            fontSize: 14.sp,
          ),
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context),
            child: Text(
              'Cancel',
              style: TextStyle(color: AppTheme.textSecondary),
            ),
          ),
          TextButton(
            onPressed: () async {
              Navigator.pop(context);
              await _performDelete(board);
            },
            child: Text(
              'Delete',
              style: TextStyle(color: Colors.red),
            ),
          ),
        ],
      ),
    );
  }
  
  Future<void> _performDelete(VisionBoard board) async {
    try {
      // Initialize service
      final visionBoardService = VisionBoardService();
      await visionBoardService.initialize(baseUrl: 'http://manifestation-qa-1.senthilguru.net');
      
      // Delete from server using vision board ID
      final result = await visionBoardService.deleteVisionBoard(board.id);
      
      if (result.isSuccess) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Vision board deleted successfully'),
            backgroundColor: AppTheme.accentMint,
          ),
        );
        
        // Reload from server to refresh the list
        await _loadVisionBoards();
      } else {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Failed to delete vision board: ${result.message}'),
            backgroundColor: Colors.red,
          ),
        );
      }
      
    } catch (e) {
      print('❌ Delete board error: $e');
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Failed to delete vision board: $e'),
          backgroundColor: Colors.red,
        ),
      );
    }
  }

  void _showSortOptions() {
    // TODO: Implement sort options
    print('Show sort options');
  }

  void _syncBoards() async {
    // Refresh from server - no local sync needed
    await _loadVisionBoards();
    
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text('Vision boards refreshed from server'),
        backgroundColor: AppTheme.accentMint,
      ),
    );
  }

  void _uploadToCloud() async {
    // No separate upload needed - everything is already on server
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text('All vision boards are automatically saved to server'),
        backgroundColor: AppTheme.accentMint,
      ),
    );
  }

  void _downloadFromCloud() async {
    // Refresh from server
    await _loadVisionBoards();
    
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text('Vision boards refreshed from server'),
        backgroundColor: AppTheme.accentMint,
      ),
    );
  }

  void _showSettings() {
    // TODO: Implement settings
    print('Show settings');
  }
}