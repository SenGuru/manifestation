import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../models/vision_board.dart';
import '../components/vision_board_canvas.dart';
import 'package:image_picker/image_picker.dart';
import 'package:path_provider/path_provider.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'dart:io';
import 'dart:convert';
import '../constants/colors.dart';

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
  bool hasUnsavedChanges = false;
  VisionBoard? lastSavedState;
  
  late AnimationController _toolbarController;
  late Animation<double> _toolbarAnimation;
  
  late AnimationController _zoomController;
  
  final ImagePicker _imagePicker = ImagePicker();
  final TextEditingController _textController = TextEditingController();
  Color _selectedTextColor = Colors.black;
  double _selectedFontSize = 16.0;
  
  // Session tracking
  String? _sessionTrackerId;

  @override
  void initState() {
    super.initState();
    currentBoard = widget.existingBoard ?? VisionBoard.empty();
    _saveToUndoStack();
    _startSessionTracking();
    
    // Load board elements if this is an existing board
    if (widget.existingBoard != null) {
      _loadBoardElements();
    }
    
    _initializeControllers();
  }

  void _initializeControllers() {
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
    _textController.dispose();
    // Session tracking cleanup - local only
    super.dispose();
  }

  /// Start session tracking for vision board (local only)
  void _startSessionTracking() async {
    // Generate local session ID for tracking
    _sessionTrackerId = DateTime.now().millisecondsSinceEpoch.toString();
    print('Vision board session started locally with ID: $_sessionTrackerId');
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
              onTap: () => _handleBackButton(),
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
              onTap: () {
                _saveBoard();
                // Update the unsaved changes flag immediately
                setState(() {
                  hasUnsavedChanges = false;
                });
              },
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
      height: 88.h,
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
              height: 48.h,
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
                margin: EdgeInsets.symmetric(horizontal: 1.w),
                height: 64.h,
                decoration: BoxDecoration(
                  gradient: isSelected
                      ? LinearGradient(colors: AppColors.visionBoardGradient)
                      : null,
                  color: isSelected ? null : Colors.transparent,
                  borderRadius: BorderRadius.circular(12.r),
                  boxShadow: isSelected ? [
                    BoxShadow(
                      color: AppColors.visionBoardGradient[0].withOpacity(0.3),
                      blurRadius: 6.r,
                      offset: Offset(0, 3.h),
                    ),
                  ] : null,
                ),
                child: Column(
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: [
                    Icon(
                      tool.icon,
                      color: isSelected ? AppColors.white : AppColors.textMedium,
                      size: 22.sp,
                    ),
                    SizedBox(height: 2.h),
                    Text(
                      tool.shortName,
                      style: TextStyle(
                        fontSize: 8.sp,
                        fontWeight: FontWeight.w500,
                        color: isSelected ? AppColors.white : AppColors.textMedium,
                      ),
                      textAlign: TextAlign.center,
                      maxLines: 1,
                      overflow: TextOverflow.ellipsis,
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
      hasUnsavedChanges = _hasChangesFromLastSave();
    });
    _saveToUndoStack();
  }
  
  bool _hasChangesFromLastSave() {
    if (lastSavedState == null) return true;
    
    // Compare current board with last saved state
    final currentJson = jsonEncode(currentBoard.toJson());
    final savedJson = jsonEncode(lastSavedState!.toJson());
    
    return currentJson != savedJson;
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

  Widget _buildTextEditorSheet() {
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
                        fontSize: _selectedFontSize.sp,
                        color: _selectedTextColor,
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
                  _addTextToBoard();
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

  void _addTextToBoard() {
    final textElement = VisionBoardTextElement(
      id: DateTime.now().millisecondsSinceEpoch.toString(),
      position: Offset(100.w, 100.h),
      size: Size(200.w, 100.h),
      text: _textController.text.trim(),
      style: TextStyle(
        fontSize: _selectedFontSize,
        color: _selectedTextColor,
        fontWeight: FontWeight.w600,
      ),
      alignment: TextAlign.center,
    );

    final updatedElements = [...currentBoard.elements, textElement];
    final updatedBoard = currentBoard.copyWith(elements: updatedElements);
    
    _onBoardChanged(updatedBoard);
    
    // Clear the text controller
    _textController.clear();
    
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
      builder: (context) => _buildTextEditorSheet(),
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
      hasUnsavedChanges = _hasChangesFromLastSave();
    });
    _saveToUndoStack();
  }

  Future<void> _saveBoard() async {
    try {
      // Generate board ID if it doesn't exist
      if (currentBoard.id.isEmpty) {
        final newId = DateTime.now().millisecondsSinceEpoch.toString();
        currentBoard = currentBoard.copyWith(id: newId);
      }
      
      bool saveSuccess = false;
      String saveMethod = '';
      
      // Saving vision board to local storage only
      print('Saving vision board locally: ${currentBoard.title}');
      
      // Save locally
      try {
        final prefs = await SharedPreferences.getInstance();
        final boardJson = jsonEncode(currentBoard.toJson());
        await prefs.setString('vision_board_${currentBoard.id}', boardJson);
        
        final savedBoards = prefs.getStringList('saved_vision_boards') ?? [];
        if (!savedBoards.contains(currentBoard.id)) {
          savedBoards.add(currentBoard.id);
          await prefs.setStringList('saved_vision_boards', savedBoards);
        }
        
        if (!saveSuccess) {
          saveSuccess = true;
          saveMethod = 'Local storage only';
        } else {
          saveMethod = 'Local storage only';
        }
        
        print('✅ Vision board saved locally: ${currentBoard.title}');
      } catch (localError) {
        print('❌ Local save error: $localError');
        if (!saveSuccess) {
          throw localError;
        }
      }
      
      if (saveSuccess) {
        // Update the last saved state
        lastSavedState = currentBoard.copyWith();
        hasUnsavedChanges = false;
        
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Vision board saved successfully! ($saveMethod)'),
            backgroundColor: AppColors.accentMint,
            duration: Duration(seconds: 2),
          ),
        );
      } else {
        throw Exception('Failed to save vision board locally');
      }
    } catch (e) {
      print('❌ Save board error: $e');
      _showErrorSnackBar('Failed to save vision board: $e');
    }
  }
  
  Future<void> _loadBoardElements() async {
    // Board elements are loaded from local storage with the board data
    print('Board elements loaded from local storage with board data');
  }
  
  // Elements are saved with board data in local storage

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

  void _handleBackButton() {
    if (hasUnsavedChanges) {
      _showExitDialog();
    } else {
      Navigator.pop(context, currentBoard); // Return the current board state
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
              Navigator.pop(context, widget.existingBoard); // Return original board if discarding
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
              _saveBoard().then((_) {
                Navigator.pop(context, currentBoard); // Return updated board after saving
              });
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
      
      // Create image element (local storage only)
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
}

// Extension for tool short names (using the original names to avoid conflicts)
extension VisionBoardToolShortName on VisionBoardTool {
  String get shortName {
    switch (this) {
      case VisionBoardTool.select:
        return 'Select';
    }
  }
}