import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:manifestation_app/vision_board.dart';
import 'package:manifestation_app/vision_board_canvas.dart';
import 'colors.dart';

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
  VisionBoardTool selectedTool = VisionBoardTool.select;
  bool showToolbar = true;
  bool isFullscreen = false;
  List<VisionBoard> undoStack = [];
  int undoIndex = -1;
  
  late AnimationController _toolbarController;
  late Animation<double> _toolbarAnimation;
  
  late AnimationController _zoomController;

  @override
  void initState() {
    super.initState();
    currentBoard = widget.existingBoard ?? VisionBoard.empty();
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
  }

  @override
  void dispose() {
    _toolbarController.dispose();
    _zoomController.dispose();
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
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
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
      height: 80.h,
      padding: EdgeInsets.symmetric(horizontal: 20.w, vertical: 16.h),
      child: Row(
        children: [
            // Back button
            GestureDetector(
              onTap: () => _showExitDialog(),
              child: Container(
                width: 48.w,
                height: 48.h,
                decoration: BoxDecoration(
                  color: AppColors.cardBackground,
                  borderRadius: BorderRadius.circular(16.r),
                  border: Border.all(
                    color: AppColors.textMedium.withOpacity(0.1),
                  ),
                ),
                child: Icon(
                  Icons.arrow_back,
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
                  height: 48.h,
                  padding: EdgeInsets.symmetric(horizontal: 16.w),
                  decoration: BoxDecoration(
                    color: AppColors.cardBackground,
                    borderRadius: BorderRadius.circular(16.r),
                    border: Border.all(
                      color: AppColors.textMedium.withOpacity(0.1),
                    ),
                  ),
                  child: Row(
                    children: [
                      Icon(
                        Icons.edit,
                        color: AppColors.textMedium,
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
            
            // Save button
            GestureDetector(
              onTap: _saveBoard,
              child: Container(
                height: 48.h,
                padding: EdgeInsets.symmetric(horizontal: 20.w),
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
                child: Row(
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    Icon(
                      Icons.save,
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
      height: 80.h,
      margin: EdgeInsets.all(20.w),
      decoration: BoxDecoration(
          color: AppColors.cardBackground,
          borderRadius: BorderRadius.circular(24.r),
          border: Border.all(
            color: AppColors.textMedium.withOpacity(0.1),
          ),
          boxShadow: [
            BoxShadow(
              color: AppColors.black.withOpacity(0.1),
              blurRadius: 20.r,
              offset: Offset(0, 10.h),
            ),
          ],
        ),
        child: Row(
          children: [
            // Tool selector
            Expanded(
              child: _buildToolSelector(),
            ),
            
            // Divider
            Container(
              width: 1.w,
              height: 40.h,
              color: AppColors.textMedium.withOpacity(0.1),
            ),
            
            // Action buttons
            _buildActionButtons(),
          ],
        ),
    );
  }

  Widget _buildToolSelector() {
    return Container(
      height: 80.h,
      child: Row(
        mainAxisAlignment: MainAxisAlignment.spaceEvenly,
        children: VisionBoardTool.values.map((tool) {
          final isSelected = selectedTool == tool;
          return Expanded(
            child: GestureDetector(
              onTap: () => _selectTool(tool),
              child: AnimatedContainer(
                duration: Duration(milliseconds: 200),
                margin: EdgeInsets.symmetric(horizontal: 2.w),
                height: 56.h,
              decoration: BoxDecoration(
                gradient: isSelected
                    ? LinearGradient(colors: AppColors.visionBoardGradient)
                    : null,
                color: isSelected ? null : Colors.transparent,
                borderRadius: BorderRadius.circular(16.r),
                boxShadow: isSelected ? [
                  BoxShadow(
                    color: AppColors.visionBoardGradient[0].withOpacity(0.3),
                    blurRadius: 8.r,
                    offset: Offset(0, 4.h),
                  ),
                ] : null,
              ),
              child: Column(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  Icon(
                    tool.icon,
                    color: isSelected ? AppColors.white : AppColors.textMedium,
                    size: 20.sp,
                  ),
                  SizedBox(height: 4.h),
                  Text(
                    tool.shortName,
                    style: TextStyle(
                      fontSize: 10.sp,
                      fontWeight: FontWeight.w500,
                      color: isSelected ? AppColors.white : AppColors.textMedium,
                    ),
                  ),
                ],
              ),
            ),
          ),
          );
        }).toList(),
      ),
    );
  }

  Widget _buildActionButtons() {
    return Container(
      padding: EdgeInsets.symmetric(horizontal: 16.w),
      child: Row(
        children: [
          // Add Image
          _buildActionButton(
            icon: Icons.image,
            onTap: _showImagePicker,
            tooltip: 'Add Image',
          ),
          
          SizedBox(width: 12.w),
          
          // Add Text
          _buildActionButton(
            icon: Icons.text_fields,
            onTap: _showTextEditor,
            tooltip: 'Add Text',
          ),
          
          SizedBox(width: 12.w),
          
          // Undo
          _buildActionButton(
            icon: Icons.undo,
            onTap: undoIndex > 0 ? _undo : null,
            tooltip: 'Undo',
            isEnabled: undoIndex > 0,
          ),
          
          SizedBox(width: 12.w),
          
          // Redo
          _buildActionButton(
            icon: Icons.redo,
            onTap: undoIndex < undoStack.length - 1 ? _redo : null,
            tooltip: 'Redo',
            isEnabled: undoIndex < undoStack.length - 1,
          ),
        ],
      ),
    );
  }

  Widget _buildActionButton({
    required IconData icon,
    required VoidCallback? onTap,
    required String tooltip,
    bool isEnabled = true,
  }) {
    return GestureDetector(
      onTap: isEnabled ? onTap : null,
      child: Container(
        width: 40.w,
        height: 40.h,
        decoration: BoxDecoration(
          color: isEnabled
              ? AppColors.backgroundMedium
              : AppColors.backgroundMedium.withOpacity(0.5),
          borderRadius: BorderRadius.circular(12.r),
        ),
        child: Icon(
          icon,
          color: isEnabled ? AppColors.textLight : AppColors.textMedium.withOpacity(0.5),
          size: 18.sp,
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
      child: Container(
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

  void _showTextEditor() {
    // TODO: Implement text editor
    print('Show text editor');
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
    // TODO: Implement title editing
    print('Edit title');
  }

  void _saveBoard() {
    // TODO: Implement save functionality
    print('Save board');
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text('Vision board saved!'),
        backgroundColor: AppColors.accentMint,
      ),
    );
  }

  void _showTemplates() {
    // TODO: Implement templates
    print('Show templates');
  }

  void _showBackgrounds() {
    // TODO: Implement backgrounds
    print('Show backgrounds');
  }

  void _showExportOptions() {
    // TODO: Implement export
    print('Show export options');
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
            onPressed: () {
              Navigator.pop(context);
              _saveBoard();
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

  void _addImageFromCamera() {
    // TODO: Implement camera
    _addPlaceholderImage('Camera');
  }

  void _addImageFromGallery() {
    // TODO: Implement gallery
    _addPlaceholderImage('Gallery');
  }

  void _addImageFromSearch() {
    // TODO: Implement search
    _addPlaceholderImage('Search');
  }

  void _addImageFromAI() {
    // TODO: Implement AI generation
    _addPlaceholderImage('AI');
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
        content: Text('$source image placeholder added'),
        backgroundColor: AppColors.accentMint,
        duration: Duration(seconds: 2),
      ),
    );
  }
}

// Extension for tool short names (using the original names to avoid conflicts)
extension VisionBoardToolShortName on VisionBoardTool {
  String get shortName {
    switch (this) {
      case VisionBoardTool.select:
        return 'Select';
      case VisionBoardTool.move:
        return 'Move';
      case VisionBoardTool.resize:
        return 'Resize';
      case VisionBoardTool.rotate:
        return 'Rotate';
      case VisionBoardTool.delete:
        return 'Delete';
    }
  }
}