import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:manifestation_app/screens/vision_board.dart';
import 'package:manifestation_app/components/vision_board_canvas.dart';
import 'package:image_picker/image_picker.dart';
import 'package:path_provider/path_provider.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'dart:io';
import 'dart:convert';
import 'package:manifestation_app/constants/colors.dart';
import 'package:manifestation_app/services/vision_board_service.dart';

class VisionBoardScreen extends StatefulWidget {
  final VisionBoard? existingBoard;

  const VisionBoardScreen({
    super.key,
    this.existingBoard,
  });

  @override
  _VisionBoardScreenState createState() => _VisionBoardScreenState();
}

class _VisionBoardScreenState extends State<VisionBoardScreen>
    with TickerProviderStateMixin {
  late VisionBoard currentBoard;
  late VisionBoard _initialBoard; // Track initial state
  VisionBoardTool selectedTool = VisionBoardTool.select;
  bool showToolbar = true;
  bool isFullscreen = false;
  List<VisionBoard> undoStack = [];
  int undoIndex = -1;
  bool _hasUnsavedChanges = false;
  
  // Session tracking variables
  DateTime? sessionStartTime;
  int sessionDuration = 0;
  VisionBoardSession? currentSession;
  final VisionBoardService _visionBoardService = VisionBoardService();
  
  late AnimationController _toolbarController;
  late Animation<double> _toolbarAnimation;
  
  late AnimationController _zoomController;
  
  final ImagePicker _imagePicker = ImagePicker();

  @override
  void initState() {
    super.initState();
    currentBoard = widget.existingBoard ?? VisionBoard.empty();
    _initialBoard = currentBoard.copyWith(); // Save initial state
    _saveToUndoStack();
    
    _toolbarController = AnimationController(
      duration: Duration(milliseconds: 300),
      vsync: this,
    );
    _toolbarAnimation = CurvedAnimation(
      parent: _toolbarController,
      curve: Curves.easeInOut,
    );
    
    _zoomController = AnimationController(
      duration: Duration(milliseconds: 200),
      vsync: this,
    );
    
    _toolbarController.forward();
    
    // Start session tracking
    _startSession();
  }

  @override
  void dispose() {
    _toolbarController.dispose();
    _zoomController.dispose();
    
    // End session tracking
    _endSession();
    
    super.dispose();
  }

  void _saveToUndoStack() {
    if (undoIndex < undoStack.length - 1) {
      undoStack = undoStack.sublist(0, undoIndex + 1);
    }
    
    undoStack.add(currentBoard.copyWith());
    undoIndex = undoStack.length - 1;
    
    if (undoStack.length > 50) {
      undoStack.removeAt(0);
      undoIndex--;
    }
    
    // Check if changes have been made
    _checkForChanges();
  }
  
  void _checkForChanges() {
    final hasChanges = !_areBoardsEqual(_initialBoard, currentBoard);
    if (hasChanges != _hasUnsavedChanges) {
      setState(() {
        _hasUnsavedChanges = hasChanges;
      });
    }
  }
  
  bool _areBoardsEqual(VisionBoard board1, VisionBoard board2) {
    // Compare basic properties
    if (board1.title != board2.title ||
        board1.backgroundColor != board2.backgroundColor ||
        board1.elements.length != board2.elements.length) {
      return false;
    }
    
    // Compare elements
    for (int i = 0; i < board1.elements.length; i++) {
      final elem1 = board1.elements[i];
      final elem2 = board2.elements[i];
      
      if (elem1.id != elem2.id ||
          elem1.position != elem2.position ||
          elem1.size != elem2.size ||
          elem1.rotation != elem2.rotation ||
          elem1.zIndex != elem2.zIndex) {
        return false;
      }
      
      // Compare element-specific properties
      if (elem1 is VisionBoardTextElement && elem2 is VisionBoardTextElement) {
        if (elem1.text != elem2.text || elem1.style != elem2.style) {
          return false;
        }
      } else if (elem1 is VisionBoardImageElement && elem2 is VisionBoardImageElement) {
        if (elem1.imagePath != elem2.imagePath || elem1.fit != elem2.fit) {
          return false;
        }
      }
    }
    
    return true;
  }

  @override
  Widget build(BuildContext context) {
    return PopScope(
      canPop: !_hasUnsavedChanges,
      onPopInvokedWithResult: (didPop, result) {
        if (!didPop && _hasUnsavedChanges) {
          _showExitDialog();
        }
      },
      child: Scaffold(
      backgroundColor: AppColors.backgroundDark,
      body: SafeArea(
        child: Column(
          children: [
            // Top header
            if (!isFullscreen) _buildHeader(),
            
            // Main canvas area
            Expanded(
              child: Stack(
                children: [
                  _buildCanvasArea(),
                  
                  // Quick actions
                  if (!isFullscreen) _buildQuickActions(),
                  
                  // Fullscreen toggle
                  _buildFullscreenToggle(),
                ],
              ),
            ),
            
            // Floating toolbar
            AnimatedBuilder(
              animation: _toolbarAnimation,
              builder: (context, child) {
                return Transform.translate(
                  offset: Offset(0, (1 - _toolbarAnimation.value) * 100.h),
                  child: Opacity(
                    opacity: _toolbarAnimation.value,
                    child: _buildFloatingToolbar(),
                  ),
                );
              },
            ),
          ],
        ),
      ),
    ),
    );
  }

  Widget _buildCanvasArea() {
    return Container(
      margin: EdgeInsets.all(isFullscreen ? 0 : 16.w),
      decoration: BoxDecoration(
        color: AppColors.white,
        borderRadius: BorderRadius.circular(isFullscreen ? 0 : 24.r),
        boxShadow: isFullscreen ? null : [
          BoxShadow(
            color: AppColors.black.withOpacity(0.1),
            blurRadius: 20.r,
            offset: Offset(0, 10.h),
          ),
        ],
      ),
      child: ClipRRect(
        borderRadius: BorderRadius.circular(isFullscreen ? 0 : 24.r),
        child: Stack(
          children: [
            // Canvas
            VisionBoardCanvas(
              board: currentBoard,
              selectedTool: selectedTool,
              onBoardChanged: _onBoardChanged,
            ),
            
            // Canvas overlay controls
            if (currentBoard.elements.isNotEmpty)
              _buildCanvasOverlay(),
          ],
        ),
      ),
    );
  }

  Widget _buildHeader() {
    return Container(
      padding: EdgeInsets.symmetric(horizontal: 20.w, vertical: 16.h),
      decoration: BoxDecoration(
        color: AppColors.backgroundDark.withOpacity(0.95),
        border: Border(
          bottom: BorderSide(
            color: AppColors.textMedium.withOpacity(0.1),
            width: 1,
          ),
        ),
      ),
      child: Row(
        children: [
          // Back button
          GestureDetector(
            onTap: () => _handleBackPress(),
            child: Container(
              width: 44.w,
              height: 44.h,
              decoration: BoxDecoration(
                color: AppColors.cardBackground.withOpacity(0.8),
                borderRadius: BorderRadius.circular(14.r),
                border: Border.all(
                  color: AppColors.visionBoardGradient[0].withOpacity(0.2),
                ),
              ),
              child: Icon(
                Icons.arrow_back_rounded,
                color: AppColors.textLight,
                size: 20.sp,
              ),
            ),
          ),
          
          SizedBox(width: 16.w),
          
          // Board title
          Expanded(
            child: GestureDetector(
              onTap: _editTitle,
              child: Container(
                height: 44.h,
                padding: EdgeInsets.symmetric(horizontal: 16.w),
                decoration: BoxDecoration(
                  color: AppColors.cardBackground.withOpacity(0.8),
                  borderRadius: BorderRadius.circular(14.r),
                  border: Border.all(
                    color: AppColors.visionBoardGradient[0].withOpacity(0.2),
                  ),
                ),
                child: Row(
                  children: [
                    Icon(
                      Icons.edit_rounded,
                      color: AppColors.visionBoardGradient[0],
                      size: 16.sp,
                    ),
                    SizedBox(width: 8.w),
                    Expanded(
                      child: Text(
                        currentBoard.title.isEmpty ? 'Untitled Board' : currentBoard.title,
                        style: TextStyle(
                          fontSize: 16.sp,
                          fontWeight: FontWeight.w600,
                          color: AppColors.textLight,
                        ),
                        overflow: TextOverflow.ellipsis,
                      ),
                    ),
                  ],
                ),
              ),
            ),
          ),
          
          SizedBox(width: 16.w),
          
          // Timer display
          Container(
            padding: EdgeInsets.symmetric(horizontal: 12.w, vertical: 8.h),
            decoration: BoxDecoration(
              gradient: LinearGradient(
                colors: AppColors.visionBoardGradient.map((c) => c.withOpacity(0.2)).toList(),
              ),
              borderRadius: BorderRadius.circular(12.r),
              border: Border.all(
                color: AppColors.visionBoardGradient[0].withOpacity(0.3),
              ),
            ),
            child: Row(
              mainAxisSize: MainAxisSize.min,
              children: [
                Icon(
                  Icons.access_time_rounded,
                  color: AppColors.visionBoardGradient[0],
                  size: 14.sp,
                ),
                SizedBox(width: 4.w),
                Text(
                  _formatSessionTime(),
                  style: TextStyle(
                    fontSize: 12.sp,
                    fontWeight: FontWeight.w600,
                    color: AppColors.visionBoardGradient[0],
                  ),
                ),
              ],
            ),
          ),
          
          SizedBox(width: 12.w),
          
          // Delete button (only show for existing boards)
          if (widget.existingBoard != null) ...[
            GestureDetector(
              onTap: _showDeleteConfirmation,
              child: Container(
                height: 44.h,
                padding: EdgeInsets.symmetric(horizontal: 16.w),
                decoration: BoxDecoration(
                  color: Colors.red.withOpacity(0.1),
                  borderRadius: BorderRadius.circular(14.r),
                  border: Border.all(
                    color: Colors.red.withOpacity(0.3),
                  ),
                ),
                child: Row(
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    Icon(
                      Icons.delete_rounded,
                      color: Colors.red,
                      size: 16.sp,
                    ),
                    SizedBox(width: 8.w),
                    Text(
                      'Delete',
                      style: TextStyle(
                        fontSize: 14.sp,
                        fontWeight: FontWeight.w600,
                        color: Colors.red,
                      ),
                    ),
                  ],
                ),
              ),
            ),
            
            SizedBox(width: 12.w),
          ],
          
          // Save button
          GestureDetector(
            onTap: _saveBoard,
            child: Container(
              height: 44.h,
              padding: EdgeInsets.symmetric(horizontal: 16.w),
              decoration: BoxDecoration(
                gradient: LinearGradient(
                  colors: AppColors.visionBoardGradient,
                ),
                borderRadius: BorderRadius.circular(14.r),
                boxShadow: [
                  BoxShadow(
                    color: AppColors.visionBoardGradient[0].withOpacity(0.3),
                    blurRadius: 8.r,
                    offset: Offset(0, 4.h),
                  ),
                ],
              ),
              child: Row(
                mainAxisSize: MainAxisSize.min,
                children: [
                  Icon(
                    Icons.save_rounded,
                    color: AppColors.white,
                    size: 16.sp,
                  ),
                  SizedBox(width: 8.w),
                  Text(
                    'Save',
                    style: TextStyle(
                      fontSize: 14.sp,
                      fontWeight: FontWeight.w600,
                      color: AppColors.white,
                    ),
                  ),
                ],
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildFloatingToolbar() {
    return Container(
      margin: EdgeInsets.all(20.w),
      decoration: BoxDecoration(
        gradient: LinearGradient(
          colors: [
            AppColors.cardBackground.withOpacity(0.95),
            AppColors.cardBackground.withOpacity(0.85),
          ],
          begin: Alignment.topCenter,
          end: Alignment.bottomCenter,
        ),
        borderRadius: BorderRadius.circular(24.r),
        border: Border.all(
          color: AppColors.visionBoardGradient[0].withOpacity(0.2),
          width: 1.5,
        ),
        boxShadow: [
          BoxShadow(
            color: AppColors.black.withOpacity(0.15),
            blurRadius: 30.r,
            offset: Offset(0, 15.h),
          ),
          BoxShadow(
            color: AppColors.visionBoardGradient[0].withOpacity(0.1),
            blurRadius: 10.r,
            offset: Offset(0, 5.h),
          ),
        ],
      ),
      child: Column(
        mainAxisSize: MainAxisSize.min,
        children: [
          // Main action buttons row
          Container(
            padding: EdgeInsets.symmetric(horizontal: 24.w, vertical: 16.h),
            child: Row(
              mainAxisAlignment: MainAxisAlignment.spaceEvenly,
              children: [
                _buildMainActionButton(
                  icon: Icons.add_photo_alternate_rounded,
                  label: 'Image',
                  onTap: _showImagePicker,
                  gradient: AppColors.visionBoardGradient,
                ),
                _buildMainActionButton(
                  icon: Icons.text_fields_rounded,
                  label: 'Text',
                  onTap: _showTextEditor,
                  gradient: AppColors.chakraGradient,
                ),
                _buildMainActionButton(
                  icon: Icons.delete_rounded,
                  label: 'Delete',
                  onTap: selectedTool == VisionBoardTool.delete ? null : () => _selectTool(VisionBoardTool.delete),
                  gradient: selectedTool == VisionBoardTool.delete ? AppColors.gratitudeGradient : null,
                  isSelected: selectedTool == VisionBoardTool.delete,
                ),
              ],
            ),
          ),
          
          // Secondary actions row
          Container(
            padding: EdgeInsets.fromLTRB(24.w, 0, 24.w, 16.h),
            child: Row(
              mainAxisAlignment: MainAxisAlignment.spaceEvenly,
              children: [
                _buildSecondaryAction(
                  icon: Icons.undo_rounded,
                  onTap: undoIndex > 0 ? _undo : null,
                  isEnabled: undoIndex > 0,
                ),
                _buildSecondaryAction(
                  icon: Icons.redo_rounded,
                  onTap: undoIndex < undoStack.length - 1 ? _redo : null,
                  isEnabled: undoIndex < undoStack.length - 1,
                ),
                _buildSecondaryAction(
                  icon: Icons.grid_view_rounded,
                  onTap: _showTemplates,
                ),
                _buildSecondaryAction(
                  icon: Icons.palette_rounded,
                  onTap: _showBackgroundPicker,
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildMainActionButton({
    required IconData icon,
    required String label,
    required VoidCallback? onTap,
    List<Color>? gradient,
    bool isSelected = false,
  }) {
    return GestureDetector(
      onTap: onTap,
      child: AnimatedContainer(
        duration: Duration(milliseconds: 200),
        padding: EdgeInsets.symmetric(horizontal: 16.w, vertical: 12.h),
        decoration: BoxDecoration(
          gradient: gradient != null && (isSelected || gradient == AppColors.visionBoardGradient || gradient == AppColors.chakraGradient)
              ? LinearGradient(colors: gradient)
              : null,
          color: gradient == null && !isSelected ? AppColors.backgroundMedium : null,
          borderRadius: BorderRadius.circular(16.r),
          border: Border.all(
            color: isSelected 
                ? gradient?.first.withOpacity(0.4) ?? AppColors.visionBoardGradient[0].withOpacity(0.4)
                : AppColors.textMedium.withOpacity(0.1),
            width: 1.5,
          ),
          boxShadow: gradient != null && (isSelected || gradient == AppColors.visionBoardGradient || gradient == AppColors.chakraGradient) ? [
            BoxShadow(
              color: gradient.first.withOpacity(0.3),
              blurRadius: 8.r,
              offset: Offset(0, 4.h),
            ),
          ] : null,
        ),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            Icon(
              icon,
              color: gradient != null && (isSelected || gradient == AppColors.visionBoardGradient || gradient == AppColors.chakraGradient)
                  ? AppColors.white
                  : AppColors.textLight,
              size: 24.sp,
            ),
            SizedBox(height: 4.h),
            Text(
              label,
              style: TextStyle(
                fontSize: 12.sp,
                fontWeight: FontWeight.w600,
                color: gradient != null && (isSelected || gradient == AppColors.visionBoardGradient || gradient == AppColors.chakraGradient)
                    ? AppColors.white
                    : AppColors.textLight,
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildSecondaryAction({
    required IconData icon,
    required VoidCallback? onTap,
    bool isEnabled = true,
  }) {
    return GestureDetector(
      onTap: isEnabled ? onTap : null,
      child: AnimatedContainer(
        duration: Duration(milliseconds: 200),
        width: 44.w,
        height: 44.h,
        decoration: BoxDecoration(
          color: isEnabled 
              ? AppColors.backgroundMedium.withOpacity(0.8)
              : AppColors.backgroundMedium.withOpacity(0.3),
          borderRadius: BorderRadius.circular(12.r),
          border: Border.all(
            color: AppColors.textMedium.withOpacity(0.1),
          ),
        ),
        child: Icon(
          icon,
          color: isEnabled 
              ? AppColors.textLight
              : AppColors.textMedium.withOpacity(0.5),
          size: 20.sp,
        ),
      ),
    );
  }

  void _showBackgroundPicker() {
    showModalBottomSheet(
      context: context,
      backgroundColor: Colors.transparent,
      builder: (context) => Container(
        decoration: BoxDecoration(
          color: AppColors.cardBackground,
          borderRadius: BorderRadius.vertical(top: Radius.circular(24.r)),
        ),
        padding: EdgeInsets.all(24.w),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            Text(
              'Choose Background',
              style: TextStyle(
                fontSize: 18.sp,
                fontWeight: FontWeight.bold,
                color: AppColors.textLight,
              ),
            ),
            SizedBox(height: 20.h),
            Wrap(
              spacing: 16.w,
              runSpacing: 16.h,
              children: [
                AppColors.white,
                AppColors.backgroundLight,
                AppColors.backgroundMedium,
                AppColors.primaryLight,
                AppColors.accentGold,
                AppColors.accentMint,
                Colors.pink.shade100,
                Colors.blue.shade100,
                Colors.purple.shade100,
              ].map((color) => GestureDetector(
                onTap: () {
                  setState(() {
                    currentBoard = currentBoard.copyWith(backgroundColor: color);
                    _hasUnsavedChanges = true;
                  });
                  _saveToUndoStack();
                  Navigator.pop(context);
                },
                child: Container(
                  width: 50.w,
                  height: 50.h,
                  decoration: BoxDecoration(
                    color: color,
                    borderRadius: BorderRadius.circular(12.r),
                    border: Border.all(
                      color: currentBoard.backgroundColor == color 
                          ? AppColors.visionBoardGradient[0]
                          : AppColors.textMedium.withOpacity(0.2),
                      width: currentBoard.backgroundColor == color ? 3 : 1,
                    ),
                  ),
                ),
              )).toList(),
            ),
            SizedBox(height: 20.h),
          ],
        ),
      ),
    );
  }

  Widget _buildQuickActions() {
    return Positioned(
      top: 20.h,
      right: 20.w,
      child: Column(
        children: [
          // Templates
          _buildQuickActionButton(
            icon: Icons.grid_view,
            onTap: _showTemplates,
            tooltip: 'Templates',
            color: AppColors.accentGold,
          ),
          
          SizedBox(height: 12.h),
          
          // Backgrounds
          _buildQuickActionButton(
            icon: Icons.color_lens,
            onTap: _showBackgrounds,
            tooltip: 'Backgrounds',
            color: AppColors.secondaryTeal,
          ),
          
          SizedBox(height: 12.h),
          
          // Export
          _buildQuickActionButton(
            icon: Icons.share,
            onTap: _showExportOptions,
            tooltip: 'Share',
            color: AppColors.gradientPurple,
          ),
        ],
      ),
    );
  }

  Widget _buildQuickActionButton({
    required IconData icon,
    required VoidCallback onTap,
    required String tooltip,
    required Color color,
  }) {
    return GestureDetector(
      onTap: onTap,
      child: Container(
        width: 48.w,
        height: 48.h,
        decoration: BoxDecoration(
          color: color.withOpacity(0.1),
          borderRadius: BorderRadius.circular(16.r),
          border: Border.all(
            color: color.withOpacity(0.2),
          ),
        ),
        child: Icon(
          icon,
          color: color,
          size: 20.sp,
        ),
      ),
    );
  }

  Widget _buildFullscreenToggle() {
    return Positioned(
      top: 20.h,
      left: 20.w,
      child: GestureDetector(
        onTap: _toggleFullscreen,
        child: Container(
          width: 48.w,
          height: 48.h,
          decoration: BoxDecoration(
            color: AppColors.cardBackground.withOpacity(0.9),
            borderRadius: BorderRadius.circular(16.r),
            border: Border.all(
              color: AppColors.textMedium.withOpacity(0.1),
            ),
          ),
          child: Icon(
            isFullscreen ? Icons.fullscreen_exit : Icons.fullscreen,
            color: AppColors.textLight,
            size: 20.sp,
          ),
        ),
      ),
    );
  }

  Widget _buildCanvasOverlay() {
    return Positioned(
      top: 16.h,
      left: 16.w,
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          // Session timer
          Container(
            padding: EdgeInsets.symmetric(horizontal: 12.w, vertical: 8.h),
            decoration: BoxDecoration(
              color: AppColors.visionBoardGradient[0].withOpacity(0.9),
              borderRadius: BorderRadius.circular(20.r),
            ),
            child: Row(
              mainAxisSize: MainAxisSize.min,
              children: [
                Icon(
                  Icons.timer,
                  color: AppColors.white,
                  size: 14.sp,
                ),
                SizedBox(width: 6.w),
                Text(
                  _formatSessionTime(),
                  style: TextStyle(
                    fontSize: 12.sp,
                    color: AppColors.white,
                    fontWeight: FontWeight.w600,
                  ),
                ),
              ],
            ),
          ),
          SizedBox(height: 8.h),
          // Items count
          Container(
            padding: EdgeInsets.symmetric(horizontal: 12.w, vertical: 8.h),
            decoration: BoxDecoration(
              color: AppColors.black.withOpacity(0.7),
              borderRadius: BorderRadius.circular(20.r),
            ),
            child: Row(
              mainAxisSize: MainAxisSize.min,
              children: [
                Icon(
                  Icons.dashboard,
                  color: AppColors.white,
                  size: 14.sp,
                ),
                SizedBox(width: 6.w),
                Text(
                  '${currentBoard.elements.length} items',
                  style: TextStyle(
                    fontSize: 12.sp,
                    color: AppColors.white,
                    fontWeight: FontWeight.w500,
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  void _selectTool(VisionBoardTool tool) {
    setState(() {
      selectedTool = tool;
    });
    
    // Animate tool selection
    _zoomController.forward().then((_) {
      _zoomController.reverse();
    });
  }

  void _onBoardChanged(VisionBoard newBoard) {
    setState(() {
      currentBoard = newBoard;
    });
    _saveToUndoStack();
  }

  void _showImagePicker() {
    showModalBottomSheet(
      context: context,
      backgroundColor: Colors.transparent,
      isScrollControlled: true,
      builder: (context) => _buildImagePickerSheet(),
    );
  }

  Widget _buildImagePickerSheet() {
    return Container(
      height: MediaQuery.of(context).size.height * 0.6,
      margin: EdgeInsets.all(20.w),
      decoration: BoxDecoration(
        color: AppColors.cardBackground,
        borderRadius: BorderRadius.circular(24.r),
      ),
      child: Column(
        children: [
          // Handle
          Container(
            width: 40.w,
            height: 4.h,
            margin: EdgeInsets.only(top: 12.h),
            decoration: BoxDecoration(
              color: AppColors.textMedium.withOpacity(0.3),
              borderRadius: BorderRadius.circular(2.r),
            ),
          ),
          
          // Header
          Padding(
            padding: EdgeInsets.all(24.w),
            child: Text(
              'Add Image',
              style: TextStyle(
                fontSize: 24.sp,
                fontWeight: FontWeight.bold,
                color: AppColors.textLight,
              ),
            ),
          ),
          
          // Options grid
          Expanded(
            child: GridView.count(
              crossAxisCount: 2,
              padding: EdgeInsets.symmetric(horizontal: 24.w),
              crossAxisSpacing: 16.w,
              mainAxisSpacing: 16.h,
              children: [
                _buildImageSourceCard(
                  icon: Icons.camera_alt,
                  title: 'Camera',
                  subtitle: 'Take a photo',
                  color: AppColors.accentGold,
                  onTap: () {
                    Navigator.pop(context);
                    _addImageFromCamera();
                  },
                ),
                _buildImageSourceCard(
                  icon: Icons.photo_library,
                  title: 'Gallery',
                  subtitle: 'Choose from photos',
                  color: AppColors.secondaryTeal,
                  onTap: () {
                    Navigator.pop(context);
                    _addImageFromGallery();
                  },
                ),
                _buildImageSourceCard(
                  icon: Icons.search,
                  title: 'Search',
                  subtitle: 'Find online images',
                  color: AppColors.gradientPurple,
                  onTap: () {
                    Navigator.pop(context);
                    _addImageFromSearch();
                  },
                ),
                _buildImageSourceCard(
                  icon: Icons.auto_awesome,
                  title: 'AI Generate',
                  subtitle: 'Create with AI',
                  color: AppColors.accentCoral,
                  onTap: () {
                    Navigator.pop(context);
                    _addImageFromAI();
                  },
                ),
              ],
            ),
          ),
          
          SizedBox(height: 20.h),
        ],
      ),
    );
  }

  Widget _buildImageSourceCard({
    required IconData icon,
    required String title,
    required String subtitle,
    required Color color,
    required VoidCallback onTap,
  }) {
    return GestureDetector(
      onTap: onTap,
      child: Container(
        decoration: BoxDecoration(
          color: AppColors.backgroundMedium,
          borderRadius: BorderRadius.circular(20.r),
          border: Border.all(
            color: AppColors.textMedium.withOpacity(0.1),
          ),
        ),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Container(
              width: 60.w,
              height: 60.h,
              decoration: BoxDecoration(
                color: color.withOpacity(0.1),
                borderRadius: BorderRadius.circular(20.r),
              ),
              child: Icon(
                icon,
                color: color,
                size: 28.sp,
              ),
            ),
            SizedBox(height: 16.h),
            Text(
              title,
              style: TextStyle(
                fontSize: 16.sp,
                fontWeight: FontWeight.bold,
                color: AppColors.textLight,
              ),
            ),
            SizedBox(height: 4.h),
            Text(
              subtitle,
              style: TextStyle(
                fontSize: 12.sp,
                color: AppColors.textMedium,
              ),
              textAlign: TextAlign.center,
            ),
          ],
        ),
      ),
    );
  }



  void _addTextToBoard(String text, double fontSize, Color color) {
    final textElement = VisionBoardTextElement(
      id: DateTime.now().millisecondsSinceEpoch.toString(),
      position: Offset(100.w, 100.h),
      size: Size(200.w, 100.h),
      text: text,
      style: TextStyle(
        fontSize: fontSize,
        color: color,
        fontWeight: FontWeight.w600,
      ),
      alignment: TextAlign.center,
    );

    final updatedElements = [...currentBoard.elements, textElement];
    final updatedBoard = currentBoard.copyWith(elements: updatedElements);
    
    _onBoardChanged(updatedBoard);
    
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text('Text added to vision board'),
        backgroundColor: AppColors.accentMint,
        duration: Duration(seconds: 2),
      ),
    );
  }

  void _showTextEditor() {
    showModalBottomSheet(
      context: context,
      backgroundColor: Colors.transparent,
      isScrollControlled: true,
      builder: (context) => TextEditorSheet(
        onTextAdded: _addTextToBoard,
      ),
    );
  }

  void _undo() {
    if (undoIndex > 0) {
      setState(() {
        undoIndex--;
        currentBoard = undoStack[undoIndex];
      });
    }
  }

  void _redo() {
    if (undoIndex < undoStack.length - 1) {
      setState(() {
        undoIndex++;
        currentBoard = undoStack[undoIndex];
      });
    }
  }

  void _toggleFullscreen() {
    setState(() {
      isFullscreen = !isFullscreen;
    });
    
    if (isFullscreen) {
      _toolbarController.reverse();
    } else {
      _toolbarController.forward();
    }
  }

  void _editTitle() {
    final titleController = TextEditingController(text: currentBoard.title);
    
    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        backgroundColor: AppColors.cardBackground,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(20.r),
        ),
        title: Text(
          'Edit Title',
          style: TextStyle(
            color: AppColors.textLight,
            fontSize: 20.sp,
            fontWeight: FontWeight.bold,
          ),
        ),
        content: TextField(
          controller: titleController,
          style: TextStyle(color: AppColors.textLight),
          autofocus: true,
          decoration: InputDecoration(
            hintText: 'Enter board title',
            hintStyle: TextStyle(color: AppColors.textMedium),
            enabledBorder: UnderlineInputBorder(
              borderSide: BorderSide(color: AppColors.textMedium.withOpacity(0.3)),
            ),
            focusedBorder: UnderlineInputBorder(
              borderSide: BorderSide(color: AppColors.visionBoardGradient[0]),
            ),
          ),
          onSubmitted: (newTitle) {
            _updateBoardTitle(newTitle.trim());
            Navigator.pop(context);
          },
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context),
            child: Text(
              'Cancel',
              style: TextStyle(color: AppColors.textMedium),
            ),
          ),
          TextButton(
            onPressed: () {
              _updateBoardTitle(titleController.text.trim());
              Navigator.pop(context);
            },
            child: Text(
              'Save',
              style: TextStyle(color: AppColors.visionBoardGradient[0]),
            ),
          ),
        ],
      ),
    );
  }

  void _updateBoardTitle(String newTitle) {
    setState(() {
      currentBoard = currentBoard.copyWith(title: newTitle);
    });
    _saveToUndoStack();
  }

  Future<void> _saveBoard() async {
    try {
      // Only proceed if there are actual changes to save
      if (!_hasUnsavedChanges) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('No changes to save'),
            backgroundColor: AppColors.textMedium,
            duration: Duration(seconds: 2),
          ),
        );
        // Still navigate back even if no changes (no result to avoid unnecessary refresh)
        Navigator.pop(context);
        return;
      }

      final prefs = await SharedPreferences.getInstance();
      
      // Generate board ID if it doesn't exist
      if (currentBoard.id.isEmpty) {
        final newId = DateTime.now().millisecondsSinceEpoch.toString();
        currentBoard = currentBoard.copyWith(id: newId);
      }
      
      // Create vision board session when saving with changes
      await _createVisionBoardSession();
      
      // Save current board
      final boardJson = jsonEncode(currentBoard.toJson());
      await prefs.setString('vision_board_${currentBoard.id}', boardJson);
      
      // Update list of saved boards
      final savedBoards = prefs.getStringList('saved_vision_boards') ?? [];
      if (!savedBoards.contains(currentBoard.id)) {
        savedBoards.add(currentBoard.id);
        await prefs.setStringList('saved_vision_boards', savedBoards);
      }
      
      // Update the initial board state and reset unsaved changes
      _initialBoard = currentBoard.copyWith();
      setState(() {
        _hasUnsavedChanges = false;
      });
      
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Vision board saved successfully!'),
          backgroundColor: AppColors.accentMint,
          duration: Duration(seconds: 2),
        ),
      );
      
      // Navigate back after successful save with result to refresh list
      Navigator.pop(context, 'saved');
    } catch (e) {
      _showErrorSnackBar('Failed to save vision board: $e');
    }
  }

  Future<void> _createVisionBoardSession() async {
    // Only create session if we haven't already and if it's a new board
    if (currentSession == null && widget.existingBoard == null) {
      print('Creating vision board session during save...');
      try {
        final title = currentBoard.title.isEmpty ? 'New Vision Board' : currentBoard.title;
        print('Vision board title: $title');
        
        final result = await _visionBoardService.createVisionBoard(
          title: title,
          addToFeed: false,
        );
        
        if (result.isSuccess && result.data != null) {
          currentSession = result.data;
          print('Vision board session created successfully: ${currentSession?.id}');
          
          // Update session with current time spent
          if (sessionDuration > 0) {
            await _updateVisionBoardSession();
          }
        } else {
          print('Failed to create vision board session: ${result.message}');
        }
      } catch (e) {
        print('Error creating vision board session: $e');
      }
    } else if (currentSession != null) {
      // Update existing session
      await _updateVisionBoardSession();
    }
  }

  Future<void> _updateVisionBoardSession() async {
    if (currentSession != null && sessionDuration > 0) {
      print('Updating vision board session ${currentSession!.id} with ${sessionDuration}s');
      
      try {
        final result = await _visionBoardService.updateVisionBoard(
          sessionId: int.parse(currentSession!.id),
          timeSpentInSeconds: sessionDuration,
          addToFeed: false,
        );
        
        if (result.isSuccess) {
          print('Vision board session updated successfully: ${sessionDuration} seconds');
        } else {
          print('Failed to update vision board session: ${result.message}');
        }
      } catch (e) {
        print('Error updating vision board session: $e');
      }
    }
  }

  Future<List<VisionBoard>> _loadSavedBoards() async {
    try {
      final prefs = await SharedPreferences.getInstance();
      final savedBoardIds = prefs.getStringList('saved_vision_boards') ?? [];
      final List<VisionBoard> boards = [];
      
      for (final boardId in savedBoardIds) {
        final boardJson = prefs.getString('vision_board_$boardId');
        if (boardJson != null) {
          final boardData = jsonDecode(boardJson);
          boards.add(VisionBoard.fromJson(boardData));
        }
      }
      
      return boards;
    } catch (e) {
      print('Error loading saved boards: $e');
      return [];
    }
  }

  Future<void> _deleteSavedBoard(String boardId) async {
    try {
      final prefs = await SharedPreferences.getInstance();
      await prefs.remove('vision_board_$boardId');
      
      final savedBoards = prefs.getStringList('saved_vision_boards') ?? [];
      savedBoards.remove(boardId);
      await prefs.setStringList('saved_vision_boards', savedBoards);
    } catch (e) {
      print('Error deleting board: $e');
    }
  }

  void _showTemplates() {
    showModalBottomSheet(
      context: context,
      backgroundColor: Colors.transparent,
      isScrollControlled: true,
      builder: (context) => _buildTemplateSheet(),
    );
  }

  Widget _buildTemplateSheet() {
    final templates = VisionBoardTemplate.getDefaultTemplates();

    return Container(
      height: MediaQuery.of(context).size.height * 0.7,
      margin: EdgeInsets.all(20.w),
      decoration: BoxDecoration(
        color: AppColors.cardBackground,
        borderRadius: BorderRadius.circular(24.r),
      ),
      child: Column(
        children: [
          // Handle
          Container(
            width: 40.w,
            height: 4.h,
            margin: EdgeInsets.only(top: 12.h),
            decoration: BoxDecoration(
              color: AppColors.textMedium.withOpacity(0.3),
              borderRadius: BorderRadius.circular(2.r),
            ),
          ),
          
          // Header
          Padding(
            padding: EdgeInsets.all(24.w),
            child: Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                Text(
                  'Choose Template',
                  style: TextStyle(
                    fontSize: 24.sp,
                    fontWeight: FontWeight.bold,
                    color: AppColors.textLight,
                  ),
                ),
                GestureDetector(
                  onTap: () => Navigator.pop(context),
                  child: Container(
                    width: 32.w,
                    height: 32.h,
                    decoration: BoxDecoration(
                      color: AppColors.backgroundMedium,
                      borderRadius: BorderRadius.circular(8.r),
                    ),
                    child: Icon(
                      Icons.close,
                      color: AppColors.textMedium,
                      size: 18.sp,
                    ),
                  ),
                ),
              ],
            ),
          ),
          
          // Template list
          Expanded(
            child: ListView.builder(
              padding: EdgeInsets.symmetric(horizontal: 24.w),
              itemCount: templates.length,
              itemBuilder: (context, index) {
                final template = templates[index];
                return GestureDetector(
                  onTap: () {
                    _applyTemplate(template);
                    Navigator.pop(context);
                  },
                  child: Container(
                    margin: EdgeInsets.only(bottom: 16.h),
                    padding: EdgeInsets.all(20.w),
                    decoration: BoxDecoration(
                      color: AppColors.backgroundMedium,
                      borderRadius: BorderRadius.circular(16.r),
                      border: Border.all(
                        color: AppColors.textMedium.withOpacity(0.1),
                      ),
                    ),
                    child: Row(
                      children: [
                        Container(
                          width: 48.w,
                          height: 48.h,
                          decoration: BoxDecoration(
                            color: template.color.withOpacity(0.1),
                            borderRadius: BorderRadius.circular(12.r),
                          ),
                          child: Icon(
                            template.icon,
                            color: template.color,
                            size: 24.sp,
                          ),
                        ),
                        SizedBox(width: 16.w),
                        Expanded(
                          child: Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              Text(
                                template.name,
                                style: TextStyle(
                                  fontSize: 16.sp,
                                  fontWeight: FontWeight.bold,
                                  color: AppColors.textLight,
                                ),
                              ),
                              SizedBox(height: 4.h),
                              Text(
                                template.description,
                                style: TextStyle(
                                  fontSize: 14.sp,
                                  color: AppColors.textMedium,
                                ),
                              ),
                            ],
                          ),
                        ),
                        Icon(
                          Icons.arrow_forward_ios,
                          color: AppColors.textMedium.withOpacity(0.6),
                          size: 16.sp,
                        ),
                      ],
                    ),
                  ),
                );
              },
            ),
          ),
          
          SizedBox(height: 20.h),
        ],
      ),
    );
  }

  void _applyTemplate(VisionBoardTemplate template) {
    setState(() {
      currentBoard = VisionBoard.fromTemplate(template);
    });
    _saveToUndoStack();
    
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text('${template.name} template applied'),
        backgroundColor: AppColors.accentMint,
        duration: Duration(seconds: 2),
      ),
    );
  }

  Widget _buildBackgroundSheet() {
    final backgroundColors = [
      Colors.white,
      AppColors.backgroundMedium,
      AppColors.backgroundDark,
      Colors.grey[100]!,
      Color(0xFFF8F9FA),
      Color(0xFFE8F4FD),
      Color(0xFFFFF8E1),
      Color(0xFFE8F5E8),
      Color(0xFFFCE4EC),
      Color(0xFFF3E5F5),
      Color(0xFFEDE7F6),
      Color(0xFFE0F2F1),
    ];

    return Container(
      height: MediaQuery.of(context).size.height * 0.6,
      margin: EdgeInsets.all(20.w),
      decoration: BoxDecoration(
        color: AppColors.cardBackground,
        borderRadius: BorderRadius.circular(24.r),
      ),
      child: Column(
        children: [
          // Handle
          Container(
            width: 40.w,
            height: 4.h,
            margin: EdgeInsets.only(top: 12.h),
            decoration: BoxDecoration(
              color: AppColors.textMedium.withOpacity(0.3),
              borderRadius: BorderRadius.circular(2.r),
            ),
          ),
          
          // Header
          Padding(
            padding: EdgeInsets.all(24.w),
            child: Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                Text(
                  'Choose Background',
                  style: TextStyle(
                    fontSize: 24.sp,
                    fontWeight: FontWeight.bold,
                    color: AppColors.textLight,
                  ),
                ),
                GestureDetector(
                  onTap: () => Navigator.pop(context),
                  child: Container(
                    width: 32.w,
                    height: 32.h,
                    decoration: BoxDecoration(
                      color: AppColors.backgroundMedium,
                      borderRadius: BorderRadius.circular(8.r),
                    ),
                    child: Icon(
                      Icons.close,
                      color: AppColors.textMedium,
                      size: 18.sp,
                    ),
                  ),
                ),
              ],
            ),
          ),
          
          // Background options
          Expanded(
            child: Padding(
              padding: EdgeInsets.symmetric(horizontal: 24.w),
              child: GridView.builder(
                gridDelegate: SliverGridDelegateWithFixedCrossAxisCount(
                  crossAxisCount: 4,
                  crossAxisSpacing: 16.w,
                  mainAxisSpacing: 16.h,
                  childAspectRatio: 1.0,
                ),
                itemCount: backgroundColors.length,
                itemBuilder: (context, index) {
                  final color = backgroundColors[index];
                  final isSelected = currentBoard.backgroundColor == color;
                  
                  return GestureDetector(
                    onTap: () {
                      _updateBackgroundColor(color);
                      Navigator.pop(context);
                    },
                    child: Container(
                      decoration: BoxDecoration(
                        color: color,
                        borderRadius: BorderRadius.circular(16.r),
                        border: Border.all(
                          color: isSelected 
                              ? AppColors.visionBoardGradient[0] 
                              : AppColors.textMedium.withOpacity(0.2),
                          width: isSelected ? 3 : 1,
                        ),
                        boxShadow: [
                          BoxShadow(
                            color: Colors.black.withOpacity(0.1),
                            blurRadius: 4.r,
                            offset: Offset(0, 2.h),
                          ),
                        ],
                      ),
                      child: isSelected
                          ? Icon(
                              Icons.check,
                              color: color == Colors.white || color.computeLuminance() > 0.7 
                                  ? Colors.black 
                                  : Colors.white,
                              size: 24.sp,
                            )
                          : null,
                    ),
                  );
                },
              ),
            ),
          ),
          
          SizedBox(height: 20.h),
        ],
      ),
    );
  }

  void _updateBackgroundColor(Color color) {
    setState(() {
      currentBoard = currentBoard.copyWith(backgroundColor: color);
    });
    _saveToUndoStack();
    
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text('Background updated'),
        backgroundColor: AppColors.accentMint,
        duration: Duration(seconds: 1),
      ),
    );
  }

  void _showBackgrounds() {
    showModalBottomSheet(
      context: context,
      backgroundColor: Colors.transparent,
      isScrollControlled: true,
      builder: (context) => _buildBackgroundSheet(),
    );
  }

  void _showExportOptions() {
    // TODO: Implement export
    print('Show export options');
  }

  void _handleBackPress() {
    if (_hasUnsavedChanges) {
      _showExitDialog();
    } else {
      Navigator.pop(context);
    }
  }
  
  void _showExitDialog() {
    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        backgroundColor: AppColors.cardBackground,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(20.r),
        ),
        title: Text(
          'Save Changes?',
          style: TextStyle(
            color: AppColors.textLight,
            fontSize: 20.sp,
            fontWeight: FontWeight.bold,
          ),
        ),
        content: Text(
          'You have unsaved changes. Would you like to save before leaving?',
          style: TextStyle(
            color: AppColors.textMedium,
            fontSize: 16.sp,
          ),
        ),
        actions: [
          TextButton(
            onPressed: () {
              Navigator.pop(context);
              Navigator.pop(context);
            },
            child: Text(
              'Discard',
              style: TextStyle(color: Colors.red),
            ),
          ),
          TextButton(
            onPressed: () => Navigator.pop(context),
            child: Text(
              'Cancel',
              style: TextStyle(color: AppColors.textMedium),
            ),
          ),
          TextButton(
            onPressed: () async {
              Navigator.pop(context);
              await _saveBoard();
              Navigator.pop(context);
            },
            child: Text(
              'Save',
              style: TextStyle(color: AppColors.visionBoardGradient[0]),
            ),
          ),
        ],
      ),
    );
  }

  void _showDeleteConfirmation() {
    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        backgroundColor: AppColors.cardBackground,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(20.r),
        ),
        title: Text(
          'Delete Vision Board?',
          style: TextStyle(
            color: AppColors.textLight,
            fontSize: 20.sp,
            fontWeight: FontWeight.bold,
          ),
        ),
        content: Text(
          'Are you sure you want to delete "${currentBoard.title.isEmpty ? 'Untitled Board' : currentBoard.title}"? This action cannot be undone.',
          style: TextStyle(
            color: AppColors.textMedium,
            fontSize: 16.sp,
          ),
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context),
            child: Text(
              'Cancel',
              style: TextStyle(color: AppColors.textMedium),
            ),
          ),
          TextButton(
            onPressed: () async {
              Navigator.pop(context);
              await _deleteBoard();
            },
            child: Text(
              'Delete',
              style: TextStyle(color: Colors.red, fontWeight: FontWeight.w600),
            ),
          ),
        ],
      ),
    );
  }

  Future<void> _deleteBoard() async {
    try {
      // Show loading indicator
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Row(
            children: [
              SizedBox(
                width: 16.w,
                height: 16.h,
                child: CircularProgressIndicator(
                  strokeWidth: 2,
                  valueColor: AlwaysStoppedAnimation<Color>(AppColors.white),
                ),
              ),
              SizedBox(width: 12.w),
              Text('Deleting vision board...'),
            ],
          ),
          backgroundColor: AppColors.textMedium,
          duration: Duration(seconds: 30),
        ),
      );

      // For existing boards, try to find and delete from API using board ID
      // Since currentSession might not be set for existing boards that haven't been saved yet
      if (widget.existingBoard != null && currentBoard.id.isNotEmpty) {
        print('Deleting existing vision board from API using board ID: ${currentBoard.id}');
        try {
          // Try to parse the board ID as session ID for API deletion
          final sessionId = int.tryParse(currentBoard.id);
          if (sessionId != null) {
            final result = await _visionBoardService.deleteVisionBoard(sessionId: sessionId);
            if (!result.isSuccess) {
              print('Failed to delete from API: ${result.message}');
            } else {
              print('Successfully deleted from API');
            }
          } else {
            print('Could not parse board ID as session ID for API deletion');
          }
        } catch (e) {
          print('Error calling API delete: $e');
          // Continue with local deletion even if API fails
        }
      } else if (currentSession != null) {
        // For new boards that have a current session
        print('Deleting vision board session from API: ${currentSession!.id}');
        final result = await _visionBoardService.deleteVisionBoard(
          sessionId: int.parse(currentSession!.id),
        );
        
        if (!result.isSuccess) {
          print('Failed to delete from API: ${result.message}');
          // Continue with local deletion even if API fails
        } else {
          print('Successfully deleted from API');
        }
      } else {
        print('No session or board ID to delete from API, only deleting locally');
      }

      // Delete from local storage
      await _deleteSavedBoard(currentBoard.id);
      
      // Hide loading indicator
      ScaffoldMessenger.of(context).hideCurrentSnackBar();
      
      // Show success message
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Vision board deleted successfully'),
          backgroundColor: AppColors.accentMint,
          duration: Duration(seconds: 2),
        ),
      );
      
      // Navigate back to the vision board list with a result to indicate deletion
      Navigator.pop(context, 'deleted');
      
    } catch (e) {
      // Hide loading indicator
      ScaffoldMessenger.of(context).hideCurrentSnackBar();
      
      print('Error deleting vision board: $e');
      _showErrorSnackBar('Failed to delete vision board: $e');
    }
  }

  Future<void> _addImageFromCamera() async {
    try {
      final XFile? image = await _imagePicker.pickImage(
        source: ImageSource.camera,
        imageQuality: 80,
      );
      
      if (image != null) {
        await _addImageToBoard(image);
      }
    } catch (e) {
      _showErrorSnackBar('Failed to take photo: $e');
    }
  }

  Future<void> _addImageFromGallery() async {
    try {
      final XFile? image = await _imagePicker.pickImage(
        source: ImageSource.gallery,
        imageQuality: 80,
      );
      
      if (image != null) {
        await _addImageToBoard(image);
      }
    } catch (e) {
      _showErrorSnackBar('Failed to pick image: $e');
    }
  }

  void _addImageFromSearch() {
    // TODO: Implement search functionality
    _addPlaceholderImage('Search');
  }

  void _addImageFromAI() {
    // TODO: Implement AI generation
    _addPlaceholderImage('AI');
  }

  Future<void> _addImageToBoard(XFile image) async {
    try {
      // Get the app's documents directory
      final Directory appDir = await getApplicationDocumentsDirectory();
      final String fileName = 'vision_board_${DateTime.now().millisecondsSinceEpoch}.jpg';
      final String localPath = '${appDir.path}/$fileName';
      
      // Copy the image to the local storage
      await File(image.path).copy(localPath);
      
      // Create image element
      final imageElement = VisionBoardImageElement(
        id: DateTime.now().millisecondsSinceEpoch.toString(),
        position: Offset(100.w, 100.h),
        size: Size(200.w, 200.h),
        imagePath: localPath,
        fit: BoxFit.cover,
      );

      final updatedElements = [...currentBoard.elements, imageElement];
      final updatedBoard = currentBoard.copyWith(elements: updatedElements);
      
      _onBoardChanged(updatedBoard);
      
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Image added to vision board'),
          backgroundColor: AppColors.accentMint,
          duration: Duration(seconds: 2),
        ),
      );
    } catch (e) {
      _showErrorSnackBar('Failed to add image: $e');
    }
  }

  void _addPlaceholderImage(String source) {
    final imageElement = VisionBoardImageElement(
      id: DateTime.now().millisecondsSinceEpoch.toString(),
      position: Offset(100.w, 100.h),
      size: Size(200.w, 200.h),
      imagePath: '',
      fit: BoxFit.cover,
    );

    final updatedElements = [...currentBoard.elements, imageElement];
    final updatedBoard = currentBoard.copyWith(elements: updatedElements);
    
    _onBoardChanged(updatedBoard);
    
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text('$source placeholder added'),
        backgroundColor: AppColors.accentMint,
        duration: Duration(seconds: 2),
      ),
    );
  }

  void _showErrorSnackBar(String message) {
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text(message),
        backgroundColor: Colors.red,
        duration: Duration(seconds: 3),
      ),
    );
  }

  // Session tracking methods
  Future<void> _startSession() async {
    print('Starting vision board session...');
    sessionStartTime = DateTime.now();
    sessionDuration = 0;
    
    // Start session timer
    _startSessionTimer();
    
    // Don't create session immediately - only when saving with changes
    print('Session tracking started, will create vision board session when saving with changes');
  }

  void _startSessionTimer() {
    Future.delayed(Duration(seconds: 1), () {
      if (mounted && sessionStartTime != null) {
        setState(() {
          sessionDuration++;
        });
        if (sessionDuration % 30 == 0) { // Log every 30 seconds
          print('Vision board session duration: ${sessionDuration}s');
        }
        _startSessionTimer();
      }
    });
  }

  Future<void> _endSession() async {
    print('Ending vision board session - Duration: ${sessionDuration}s');
    
    if (sessionStartTime == null) {
      print('No session start time recorded');
      return;
    }
    
    // Only update session if it exists (was created during save)
    if (currentSession != null) {
      await _updateVisionBoardSession();
    } else {
      print('No session to update - session only created when saving with changes');
    }
  }

  String _formatSessionTime() {
    final minutes = sessionDuration ~/ 60;
    final seconds = sessionDuration % 60;
    return '${minutes.toString().padLeft(2, '0')}:${seconds.toString().padLeft(2, '0')}';
  }
}

// Extension for tool short names (using the original names to avoid conflicts)
extension VisionBoardToolShortName on VisionBoardTool {
  String get shortName {
    switch (this) {
      case VisionBoardTool.select:
        return 'Select';
      case VisionBoardTool.delete:
        return 'Delete';
    }
  }
}

// Separate StatefulWidget for text editor to handle its own state
class TextEditorSheet extends StatefulWidget {
  final Function(String text, double fontSize, Color color) onTextAdded;

  const TextEditorSheet({
    Key? key,
    required this.onTextAdded,
  }) : super(key: key);

  @override
  _TextEditorSheetState createState() => _TextEditorSheetState();
}

class _TextEditorSheetState extends State<TextEditorSheet> {
  final TextEditingController _textController = TextEditingController();
  Color _selectedTextColor = Colors.black;
  double _selectedFontSize = 16.0;

  @override
  void dispose() {
    _textController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Container(
      height: MediaQuery.of(context).size.height * 0.7,
      margin: EdgeInsets.all(20.w),
      decoration: BoxDecoration(
        color: AppColors.cardBackground,
        borderRadius: BorderRadius.circular(24.r),
      ),
      child: Column(
        children: [
          // Handle
          Container(
            width: 40.w,
            height: 4.h,
            margin: EdgeInsets.only(top: 12.h),
            decoration: BoxDecoration(
              color: AppColors.textMedium.withOpacity(0.3),
              borderRadius: BorderRadius.circular(2.r),
            ),
          ),
          
          // Header
          Padding(
            padding: EdgeInsets.all(24.w),
            child: Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                Text(
                  'Add Text',
                  style: TextStyle(
                    fontSize: 24.sp,
                    fontWeight: FontWeight.bold,
                    color: AppColors.textLight,
                  ),
                ),
                GestureDetector(
                  onTap: () => Navigator.pop(context),
                  child: Container(
                    width: 32.w,
                    height: 32.h,
                    decoration: BoxDecoration(
                      color: AppColors.backgroundMedium,
                      borderRadius: BorderRadius.circular(8.r),
                    ),
                    child: Icon(
                      Icons.close,
                      color: AppColors.textMedium,
                      size: 18.sp,
                    ),
                  ),
                ),
              ],
            ),
          ),
          
          // Text input
          Expanded(
            child: Padding(
              padding: EdgeInsets.symmetric(horizontal: 24.w),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  // Text field
                  Container(
                    decoration: BoxDecoration(
                      color: AppColors.backgroundMedium,
                      borderRadius: BorderRadius.circular(16.r),
                      border: Border.all(
                        color: AppColors.textMedium.withOpacity(0.1),
                      ),
                    ),
                    child: TextField(
                      controller: _textController,
                      style: TextStyle(
                        fontSize: 16.sp, // Keep consistent readable size
                        color: AppColors.textLight,
                        fontWeight: FontWeight.w500,
                      ),
                      maxLines: 4,
                      decoration: InputDecoration(
                        hintText: 'Enter your inspirational text...',
                        hintStyle: TextStyle(
                          color: AppColors.textMedium.withOpacity(0.6),
                          fontSize: 16.sp,
                        ),
                        border: InputBorder.none,
                        contentPadding: EdgeInsets.all(20.w),
                      ),
                    ),
                  ),
                  
                  SizedBox(height: 24.h),
                  
                  // Font size slider
                  Text(
                    'Font Size',
                    style: TextStyle(
                      fontSize: 16.sp,
                      fontWeight: FontWeight.w600,
                      color: AppColors.textLight,
                    ),
                  ),
                  SizedBox(height: 12.h),
                  SliderTheme(
                    data: SliderTheme.of(context).copyWith(
                      activeTrackColor: AppColors.visionBoardGradient[0],
                      inactiveTrackColor: AppColors.textMedium.withOpacity(0.2),
                      thumbColor: AppColors.visionBoardGradient[0],
                      overlayColor: AppColors.visionBoardGradient[0].withOpacity(0.2),
                    ),
                    child: Slider(
                      value: _selectedFontSize,
                      min: 12.0,
                      max: 48.0,
                      divisions: 18,
                      label: _selectedFontSize.round().toString(),
                      onChanged: (value) {
                        setState(() {
                          _selectedFontSize = value;
                        });
                      },
                    ),
                  ),
                  
                  SizedBox(height: 24.h),
                  
                  // Color picker
                  Text(
                    'Text Color',
                    style: TextStyle(
                      fontSize: 16.sp,
                      fontWeight: FontWeight.w600,
                      color: AppColors.textLight,
                    ),
                  ),
                  SizedBox(height: 12.h),
                  Container(
                    height: 60.h,
                    child: ListView(
                      scrollDirection: Axis.horizontal,
                      children: [
                        Colors.black,
                        Colors.white,
                        AppColors.visionBoardGradient[0],
                        AppColors.secondaryTeal,
                        AppColors.accentGold,
                        AppColors.accentCoral,
                        AppColors.gradientPurple,
                        AppColors.accentMint,
                        Colors.red,
                        Colors.blue,
                        Colors.green,
                        Colors.orange,
                      ].map((color) => _buildColorOption(color)).toList(),
                    ),
                  ),
                ],
              ),
            ),
          ),
          
          // Add button
          Padding(
            padding: EdgeInsets.all(24.w),
            child: GestureDetector(
              onTap: () {
                if (_textController.text.trim().isNotEmpty) {
                  widget.onTextAdded(
                    _textController.text.trim(),
                    _selectedFontSize,
                    _selectedTextColor,
                  );
                  Navigator.pop(context);
                }
              },
              child: Container(
                width: double.infinity,
                height: 56.h,
                decoration: BoxDecoration(
                  gradient: LinearGradient(
                    colors: AppColors.visionBoardGradient,
                  ),
                  borderRadius: BorderRadius.circular(16.r),
                  boxShadow: [
                    BoxShadow(
                      color: AppColors.visionBoardGradient[0].withOpacity(0.3),
                      blurRadius: 8.r,
                      offset: Offset(0, 4.h),
                    ),
                  ],
                ),
                child: Center(
                  child: Text(
                    'Add Text',
                    style: TextStyle(
                      fontSize: 18.sp,
                      fontWeight: FontWeight.bold,
                      color: AppColors.white,
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

  Widget _buildColorOption(Color color) {
    final isSelected = _selectedTextColor == color;
    return GestureDetector(
      onTap: () {
        setState(() {
          _selectedTextColor = color;
        });
      },
      child: Container(
        width: 48.w,
        height: 48.h,
        margin: EdgeInsets.only(right: 12.w),
        decoration: BoxDecoration(
          color: color,
          borderRadius: BorderRadius.circular(12.r),
          border: Border.all(
            color: isSelected ? AppColors.textLight : AppColors.textMedium.withOpacity(0.2),
            width: isSelected ? 3 : 1,
          ),
          boxShadow: [
            BoxShadow(
              color: Colors.black.withOpacity(0.1),
              blurRadius: 4.r,
              offset: Offset(0, 2.h),
            ),
          ],
        ),
        child: isSelected
            ? Icon(
                Icons.check,
                color: color == Colors.white || color == AppColors.accentMint 
                    ? Colors.black 
                    : Colors.white,
                size: 20.sp,
              )
            : null,
      ),
    );
  }
}