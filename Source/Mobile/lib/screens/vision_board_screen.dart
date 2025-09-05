
import 'package:flutter/material.dart';
import 'package:flutter/rendering.dart';
import 'dart:ui' as ui;
import 'dart:typed_data';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../models/vision_board.dart';
import '../constants/app_theme.dart';
import '../components/vision_board_canvas.dart';
import '../components/vision_board_toolbar.dart';
import '../services/usage_analytics_service.dart';
import '../services/vision_board_service.dart';
import '../models/analytics_models.dart';
import 'package:image_picker/image_picker.dart';
import 'package:path_provider/path_provider.dart';
import 'dart:io';
import 'dart:convert';
import 'dart:async';
import '../widgets/stylish_loading.dart';
import '../widgets/saving_vision_board_dialog.dart';
import 'vision_board_home.dart';
import '../widgets/universal_back_button.dart';

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
  List<VisionBoard> undoStack = [];
  int undoIndex = -1;
  bool hasUnsavedChanges = false;
  VisionBoard? lastSavedState;
  bool hasBeenSavedToServer = false;
  int? serverPictureId; // Store the server picture ID
  bool isLoadingContent = false; // Track if we're loading content from server
  
  late AnimationController _toolbarController;
  late Animation<double> _toolbarAnimation;
  
  late AnimationController _zoomController;
  
  final ImagePicker _imagePicker = ImagePicker();
  final TextEditingController _textController = TextEditingController();
  Color _selectedTextColor = Colors.black;
  double _selectedFontSize = 16.0;
  
  // Session tracking with UsageAnalyticsService
  final UsageAnalyticsService _analyticsService = UsageAnalyticsService();
  bool _sessionStarted = false;
  
  // GlobalKey for capturing canvas as image
  final GlobalKey _canvasKey = GlobalKey();
  final GlobalKey _thumbnailCanvasKey = GlobalKey(); // Separate key for clean thumbnail capture

  @override
  void initState() {
    super.initState();
    currentBoard = widget.existingBoard ?? VisionBoard.empty();
    
    // Ensure new boards get a stable ID immediately
    if (currentBoard.id.isEmpty) {
      currentBoard = currentBoard.copyWith(
        id: DateTime.now().millisecondsSinceEpoch.toString(),
      );
    }
    
    _saveToUndoStack();
    _startSessionTracking();
    
    // Load board elements if this is an existing board
    if (widget.existingBoard != null) {
      _loadBoardElements();
      // Only mark as saved to server if it's truly an existing board from server
      // New boards created with _createNewBoard have IDs but aren't on server yet
      hasBeenSavedToServer = true;
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
    _autoSaveTimer?.cancel(); // Cancel auto-save timer
    
    // No final sync needed
    
    // End session tracking
    _endSessionTracking();
    
    super.dispose();
  }
  
  /// No final sync needed - everything is saved directly to server

  /// End session tracking for vision board
  Future<void> _endSessionTracking() async {
    if (_sessionStarted) {
      await _analyticsService.endSession(
        ActivityType.visionBoard,
        additionalMetadata: {
          'boardId': currentBoard.id,
          'boardTitle': currentBoard.title,
          'elementCount': currentBoard.elements.length,
        },
      );
      _sessionStarted = false;
      print('📊 Vision board session ended');
    }
  }

  /// Start session tracking for vision board using UsageAnalyticsService
  Future<void> _startSessionTracking() async {
    try {
      await _analyticsService.initialize();
      _analyticsService.startSession(
        ActivityType.visionBoard,
        metadata: {
          'boardId': currentBoard.id,
          'boardTitle': currentBoard.title,
          'isNewBoard': widget.existingBoard == null,
        },
      );
      _sessionStarted = true;
      print('📊 Vision board session started');
    } catch (e) {
      print('❌ Error starting vision board session: $e');
    }
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
    // Show loading screen while content is being loaded from server
    if (isLoadingContent) {
      return Scaffold(
        backgroundColor: AppTheme.backgroundPrimary,
        body: StylishLoading(
          message: 'Loading your vision board...',
        ),
      );
    }
    
    return Scaffold(
      backgroundColor: AppTheme.backgroundDark,
      body: SafeArea(
        child: Column(
          children: [
            // Top header
            _buildHeader(),
            
            // Main canvas area
            Expanded(
              child: Stack(
                children: [
                  _buildCanvasArea(),
                  
                  // Quick actions (only background color picker)
                  _buildQuickActions(),
                ],
              ),
            ),
            
            // Floating toolbar with better positioning
            AnimatedBuilder(
              animation: _toolbarAnimation,
              builder: (context, child) {
                return Transform.translate(
                  offset: Offset(0, (1 - _toolbarAnimation.value) * 120.h),
                  child: Opacity(
                    opacity: _toolbarAnimation.value,
                    child: Container(
                      margin: EdgeInsets.only(bottom: 12.h), // Add safe area
                      child: _buildFloatingToolbar(),
                    ),
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
      margin: EdgeInsets.only(
        left: 20.w,
        right: 20.w,
        top: 12.h,
        bottom: 8.h, // Less bottom margin to prevent cutoff
      ),
      decoration: BoxDecoration(
        gradient: LinearGradient(
          colors: [
            currentBoard.backgroundColor,
            currentBoard.backgroundColor.withValues(alpha: 0.98),
          ],
          begin: Alignment.topCenter,
          end: Alignment.bottomCenter,
        ),
        borderRadius: BorderRadius.circular(28.r),
        border: Border.all(
          color: AppTheme.textMedium.withValues(alpha: 0.08),
          width: 1.w,
        ),
        boxShadow: [
          BoxShadow(
            color: AppTheme.black.withValues(alpha: 0.08),
            blurRadius: 25.r,
            offset: Offset(0, 12.h),
          ),
          BoxShadow(
            color: AppTheme.visionBoardGradient[0].withValues(alpha: 0.05),
            blurRadius: 40.r,
            offset: Offset(0, 20.h),
          ),
        ],
      ),
      child: ClipRRect(
        borderRadius: BorderRadius.circular(28.r),
        child: Container(
          child: RepaintBoundary(
            key: _canvasKey,
            child: Container(
              padding: EdgeInsets.all(12.w),
              child: VisionBoardCanvas(
                board: currentBoard,
                selectedTool: selectedTool,
                onBoardChanged: _onBoardChanged,
                onDeselectTool: _deselectTool,
              ),
            ),
          ),
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
              child: UniversalBackButton(),
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
                    color: AppTheme.cardBackground,
                    borderRadius: BorderRadius.circular(16.r),
                    border: Border.all(
                      color: AppTheme.textMedium.withValues(alpha: 0.1),
                    ),
                  ),
                  child: Row(
                    children: [
                      Icon(
                        Icons.edit,
                        color: AppTheme.textMedium,
                        size: 16.sp,
                      ),
                      SizedBox(width: 8.w),
                      Expanded(
                        child: Text(
                          currentBoard.title.isEmpty ? 'Untitled Board' : currentBoard.title,
                          style: TextStyle(
                            fontSize: 16.sp,
                            fontWeight: FontWeight.w600,
                            color: AppTheme.textLight,
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
                    colors: AppTheme.visionBoardGradient,
                  ),
                  borderRadius: BorderRadius.circular(16.r),
                  boxShadow: [
                    BoxShadow(
                      color: AppTheme.visionBoardGradient[0].withValues(alpha: 0.3),
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
                      color: AppTheme.white,
                      size: 16.sp,
                    ),
                    SizedBox(width: 8.w),
                    Text(
                      'Save',
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
          ],
        ),
    );
  }

  Widget _buildFloatingToolbar() {
    return VisionBoardToolbar(
      selectedTool: selectedTool,
      onToolSelected: (tool) {
        setState(() {
          selectedTool = tool;
        });
      },
      onAddText: _showTextEditor,
      onAddImage: _showImagePicker,
      onUndo: _undo,
      onRedo: _redo,
      canUndo: undoIndex > 0,
      canRedo: undoIndex < undoStack.length - 1,
    );
  }


  Widget _buildQuickActions() {
    return Positioned(
      top: 20.h,
      right: 20.w,
      child: Column(
        children: [
          // Backgrounds only
          _buildQuickActionButton(
            icon: Icons.color_lens,
            onTap: _showBackgrounds,
            tooltip: 'Backgrounds',
            color: AppTheme.visionBoardGradient[0],
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
          color: color.withValues(alpha: 0.1),
          borderRadius: BorderRadius.circular(16.r),
          border: Border.all(
            color: color.withValues(alpha: 0.2),
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




  void _onBoardChanged(VisionBoard newBoard) {
    setState(() {
      currentBoard = newBoard;
      hasUnsavedChanges = _hasChangesFromLastSave();
    });
    _saveToUndoStack();
    
    // Auto-save disabled for server-only mode
    _autoSaveToServer();
  }
  
  void _deselectTool() {
    setState(() {
      selectedTool = VisionBoardTool.select;
    });
  }
  
  Timer? _autoSaveTimer;
  
  void _autoSaveToServer() {
    // Cancel previous timer if it exists
    _autoSaveTimer?.cancel();
    
    // Set a new timer to save after 2 seconds of no changes
    _autoSaveTimer = Timer(Duration(seconds: 2), () async {
      // Auto-save is disabled for now - only manual save to server
      // This prevents too many API calls during editing
      print('🔄 Auto-save triggered (disabled for server-only mode)');
    });
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
        color: AppTheme.cardBackground,
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
              color: AppTheme.textMedium.withValues(alpha: 0.3),
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
                color: AppTheme.textLight,
              ),
            ),
          ),
          
          // Options - Only Gallery and Internet
          Expanded(
            child: Padding(
              padding: EdgeInsets.symmetric(horizontal: 24.w),
              child: Column(
                children: [
                  // Gallery option
                  _buildEnhancedImageSourceCard(
                    icon: Icons.photo_library_rounded,
                    title: 'Gallery',
                    subtitle: 'Upload from your device photos',
                    color: AppTheme.visionBoardGradient[0],
                    onTap: () {
                      Navigator.pop(context, currentBoard);
                      _addImageFromGallery();
                    },
                  ),
                  
                  SizedBox(height: 20.h),
                  
                  // Internet URL option
                  _buildEnhancedImageSourceCard(
                    icon: Icons.link_rounded,
                    title: 'Image URL',
                    subtitle: 'Paste an image URL from the internet',
                    color: AppTheme.journalGradient[0],
                    onTap: () {
                      Navigator.pop(context, currentBoard);
                      _addImageFromURL();
                    },
                  ),
                  
                  SizedBox(height: 32.h),
                  
                  // Info text
                  Container(
                    padding: EdgeInsets.all(16.w),
                    decoration: BoxDecoration(
                      color: AppTheme.backgroundMedium.withValues(alpha: 0.5),
                      borderRadius: BorderRadius.circular(12.r),
                      border: Border.all(
                        color: AppTheme.textMedium.withValues(alpha: 0.1),
                      ),
                    ),
                    child: Row(
                      children: [
                        Icon(
                          Icons.info_outline_rounded,
                          color: AppTheme.textMedium,
                          size: 20.sp,
                        ),
                        SizedBox(width: 12.w),
                        Expanded(
                          child: Text(
                            'Use gallery for local photos or paste image URLs from the web',
                            style: TextStyle(
                              fontSize: 13.sp,
                              color: AppTheme.textMedium,
                              height: 1.3,
                            ),
                          ),
                        ),
                      ],
                    ),
                  ),
                ],
              ),
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
          color: AppTheme.backgroundMedium,
          borderRadius: BorderRadius.circular(20.r),
          border: Border.all(
            color: AppTheme.textMedium.withValues(alpha: 0.1),
          ),
        ),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Container(
              width: 60.w,
              height: 60.h,
              decoration: BoxDecoration(
                color: color.withValues(alpha: 0.1),
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
                color: AppTheme.textLight,
              ),
            ),
            SizedBox(height: 4.h),
            Text(
              subtitle,
              style: TextStyle(
                fontSize: 12.sp,
                color: AppTheme.textMedium,
              ),
              textAlign: TextAlign.center,
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildEnhancedImageSourceCard({
    required IconData icon,
    required String title,
    required String subtitle,
    required Color color,
    required VoidCallback onTap,
  }) {
    return GestureDetector(
      onTap: onTap,
      child: Container(
        width: double.infinity,
        padding: EdgeInsets.all(24.w),
        decoration: BoxDecoration(
          gradient: LinearGradient(
            colors: [
              AppTheme.cardBackground,
              AppTheme.cardBackground.withValues(alpha: 0.8),
            ],
            begin: Alignment.topLeft,
            end: Alignment.bottomRight,
          ),
          borderRadius: BorderRadius.circular(20.r),
          border: Border.all(
            color: color.withValues(alpha: 0.2),
            width: 1.5,
          ),
          boxShadow: [
            BoxShadow(
              color: color.withValues(alpha: 0.1),
              blurRadius: 20.r,
              offset: Offset(0, 8.h),
            ),
          ],
        ),
        child: Row(
          children: [
            // Icon container
            Container(
              width: 64.w,
              height: 64.h,
              decoration: BoxDecoration(
                gradient: LinearGradient(
                  colors: [color, color.withValues(alpha: 0.8)],
                  begin: Alignment.topLeft,
                  end: Alignment.bottomRight,
                ),
                borderRadius: BorderRadius.circular(18.r),
                boxShadow: [
                  BoxShadow(
                    color: color.withValues(alpha: 0.3),
                    blurRadius: 12.r,
                    offset: Offset(0, 6.h),
                  ),
                ],
              ),
              child: Icon(
                icon,
                color: AppTheme.white,
                size: 32.sp,
              ),
            ),
            
            SizedBox(width: 20.w),
            
            // Text content
            Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    title,
                    style: TextStyle(
                      fontSize: 20.sp,
                      fontWeight: FontWeight.bold,
                      color: AppTheme.textPrimary,
                    ),
                  ),
                  SizedBox(height: 6.h),
                  Text(
                    subtitle,
                    style: TextStyle(
                      fontSize: 14.sp,
                      color: AppTheme.textSecondary,
                      height: 1.3,
                    ),
                  ),
                ],
              ),
            ),
            
            // Arrow indicator
            Container(
              width: 40.w,
              height: 40.h,
              decoration: BoxDecoration(
                color: color.withValues(alpha: 0.1),
                borderRadius: BorderRadius.circular(12.r),
              ),
              child: Icon(
                Icons.arrow_forward_rounded,
                color: color,
                size: 20.sp,
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildTextEditorSheet() {
    return StatefulBuilder(
      builder: (context, setModalState) => GestureDetector(
        onTap: () {
          // Dismiss keyboard when tapping anywhere on the sheet
          FocusScope.of(context).unfocus();
        },
        child: Container(
          height: MediaQuery.of(context).size.height * 0.75,
          margin: EdgeInsets.all(16.w),
          decoration: BoxDecoration(
            gradient: LinearGradient(
              colors: [
                AppTheme.cardBackground,
                AppTheme.cardBackground.withValues(alpha: 0.95),
              ],
              begin: Alignment.topCenter,
              end: Alignment.bottomCenter,
            ),
            borderRadius: BorderRadius.circular(28.r),
            border: Border.all(
              color: Color(0xFF6C63FF).withValues(alpha: 0.2),
              width: 1.5,
            ),
            boxShadow: [
              BoxShadow(
                color: Color(0xFF6C63FF).withValues(alpha: 0.15),
                blurRadius: 30.r,
                offset: Offset(0, 15.h),
              ),
              BoxShadow(
                color: AppTheme.black.withValues(alpha: 0.1),
                blurRadius: 20.r,
                offset: Offset(0, 8.h),
              ),
            ],
          ),
          child: Column(
          children: [
            // Modern handle with gradient
            Container(
              width: 50.w,
              height: 5.h,
              margin: EdgeInsets.only(top: 12.h),
              decoration: BoxDecoration(
                gradient: LinearGradient(
                  colors: [Color(0xFF6C63FF), Color(0xFF5A52E8)],
                ),
                borderRadius: BorderRadius.circular(3.r),
              ),
            ),
            
            // Enhanced header
            Padding(
              padding: EdgeInsets.all(24.w),
              child: Row(
                children: [
                  // Icon with gradient background
                  Container(
                    width: 48.w,
                    height: 48.h,
                    decoration: BoxDecoration(
                      gradient: LinearGradient(
                        colors: [Color(0xFF6C63FF), Color(0xFF5A52E8)],
                      ),
                      borderRadius: BorderRadius.circular(16.r),
                      boxShadow: [
                        BoxShadow(
                          color: Color(0xFF6C63FF).withValues(alpha: 0.3),
                          blurRadius: 12.r,
                          offset: Offset(0, 6.h),
                        ),
                      ],
                    ),
                    child: Stack(
                      alignment: Alignment.center,
                      children: [
                        Container(
                          width: 24.w,
                          height: 18.h,
                          decoration: BoxDecoration(
                            color: AppTheme.white.withValues(alpha: 0.2),
                            borderRadius: BorderRadius.circular(4.r),
                          ),
                        ),
                        Text(
                          'Aa',
                          style: TextStyle(
                            fontSize: 16.sp,
                            fontWeight: FontWeight.bold,
                            color: AppTheme.white,
                          ),
                        ),
                      ],
                    ),
                  ),
                  
                  SizedBox(width: 16.w),
                  
                  // Title and subtitle
                  Expanded(
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text(
                          'Add Text',
                          style: TextStyle(
                            fontSize: 24.sp,
                            fontWeight: FontWeight.bold,
                            color: AppTheme.textPrimary,
                          ),
                        ),
                        SizedBox(height: 4.h),
                        Text(
                          'Create inspiring text for your vision',
                          style: TextStyle(
                            fontSize: 14.sp,
                            color: AppTheme.textSecondary,
                          ),
                        ),
                      ],
                    ),
                  ),
                  
                  // Enhanced close button
                  GestureDetector(
                    onTap: () => Navigator.pop(context),
                    child: Container(
                      width: 40.w,
                      height: 40.h,
                      decoration: BoxDecoration(
                        color: AppTheme.textMedium.withValues(alpha: 0.1),
                        borderRadius: BorderRadius.circular(12.r),
                      ),
                      child: Icon(
                        Icons.close_rounded,
                        color: AppTheme.textMedium,
                        size: 20.sp,
                      ),
                    ),
                  ),
                ],
              ),
            ),
            
            // Enhanced content area
            Expanded(
              child: Padding(
                padding: EdgeInsets.symmetric(horizontal: 24.w),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    // Live preview section
                    
                    // Enhanced text field
                    TextField(
                      controller: _textController,
                      style: TextStyle(
                        fontSize: 16.sp,
                        color: AppTheme.textPrimary,
                        fontWeight: FontWeight.w500,
                      ),
                      maxLines: 3,
                      onChanged: (value) => setModalState(() {}),
                      decoration: InputDecoration(
                        hintText: 'Type your inspirational message...',
                        hintStyle: TextStyle(
                          color: AppTheme.textMedium.withValues(alpha: 0.6),
                          fontSize: 16.sp,
                        ),
                        filled: true,
                        fillColor: AppTheme.backgroundMedium.withValues(alpha: 0.3),
                        border: OutlineInputBorder(
                          borderRadius: BorderRadius.circular(16.r),
                          borderSide: BorderSide.none,
                        ),
                        focusedBorder: OutlineInputBorder(
                          borderRadius: BorderRadius.circular(16.r),
                          borderSide: BorderSide(
                            color: Color(0xFF6C63FF),
                            width: 2,
                          ),
                        ),
                        contentPadding: EdgeInsets.all(20.w),
                      ),
                    ),
                    
                    SizedBox(height: 24.h),
                    
                    // Font size with visual feedback
                    Row(
                      mainAxisAlignment: MainAxisAlignment.spaceBetween,
                      children: [
                        Text(
                          'Font Size',
                          style: TextStyle(
                            fontSize: 16.sp,
                            fontWeight: FontWeight.w600,
                            color: AppTheme.textPrimary,
                          ),
                        ),
                        Container(
                          padding: EdgeInsets.symmetric(horizontal: 12.w, vertical: 6.h),
                          decoration: BoxDecoration(
                            color: Color(0xFF6C63FF).withValues(alpha: 0.1),
                            borderRadius: BorderRadius.circular(8.r),
                          ),
                          child: Text(
                            '${_selectedFontSize.round()}px',
                            style: TextStyle(
                              fontSize: 14.sp,
                              fontWeight: FontWeight.w600,
                              color: Color(0xFF6C63FF),
                            ),
                          ),
                        ),
                      ],
                    ),
                    SizedBox(height: 12.h),
                    SliderTheme(
                      data: SliderTheme.of(context).copyWith(
                        activeTrackColor: Color(0xFF6C63FF),
                        inactiveTrackColor: AppTheme.textMedium.withValues(alpha: 0.2),
                        thumbColor: Color(0xFF6C63FF),
                        overlayColor: Color(0xFF6C63FF).withValues(alpha: 0.2),
                        trackHeight: 4.h,
                      ),
                      child: Slider(
                        value: _selectedFontSize,
                        min: 12.0,
                        max: 48.0,
                        divisions: 18,
                        onChanged: (value) {
                          setModalState(() {
                            _selectedFontSize = value;
                          });
                        },
                      ),
                    ),
                    
                    SizedBox(height: 24.h),
                    
                    // Enhanced color picker with navigation
                    Text(
                      'Text Color',
                      style: TextStyle(
                        fontSize: 16.sp,
                        fontWeight: FontWeight.w600,
                        color: AppTheme.textPrimary,
                      ),
                    ),
                    SizedBox(height: 12.h),
                    _buildColorPickerWithArrows(setModalState),
                  ],
                ),
              ),
            ),
            
            // Enhanced action buttons
            Padding(
              padding: EdgeInsets.all(24.w),
              child: Row(
                children: [
                  // Cancel button
                  Expanded(
                    child: GestureDetector(
                      onTap: () => Navigator.pop(context),
                      child: Container(
                        height: 56.h,
                        decoration: BoxDecoration(
                          color: AppTheme.textMedium.withValues(alpha: 0.1),
                          borderRadius: BorderRadius.circular(16.r),
                        ),
                        child: Center(
                          child: Text(
                            'Cancel',
                            style: TextStyle(
                              fontSize: 16.sp,
                              fontWeight: FontWeight.w600,
                              color: AppTheme.textMedium,
                            ),
                          ),
                        ),
                      ),
                    ),
                  ),
                  
                  SizedBox(width: 16.w),
                  
                  // Add button
                  Expanded(
                    flex: 2,
                    child: GestureDetector(
                      onTap: () {
                        if (_textController.text.trim().isNotEmpty) {
                          _addTextToBoard();
                          Navigator.pop(context, currentBoard);
                        }
                      },
                      child: Container(
                        height: 56.h,
                        decoration: BoxDecoration(
                          gradient: LinearGradient(
                            colors: [Color(0xFF6C63FF), Color(0xFF5A52E8)],
                          ),
                          borderRadius: BorderRadius.circular(16.r),
                          boxShadow: [
                            BoxShadow(
                              color: Color(0xFF6C63FF).withValues(alpha: 0.4),
                              blurRadius: 16.r,
                              offset: Offset(0, 8.h),
                            ),
                          ],
                        ),
                        child: Center(
                          child: Row(
                            mainAxisAlignment: MainAxisAlignment.center,
                            children: [
                              Icon(
                                Icons.add_rounded,
                                color: AppTheme.white,
                                size: 20.sp,
                              ),
                              SizedBox(width: 8.w),
                              Text(
                                'Add to Vision Board',
                                style: TextStyle(
                                  fontSize: 16.sp,
                                  fontWeight: FontWeight.bold,
                                  color: AppTheme.white,
                                ),
                              ),
                            ],
                          ),
                        ),
                      ),
                    ),
                  ),
                ],
              ),
            ),
          ],
        ),
        ),
      ),
    );
  }

  Widget _buildColorOption(Color color, StateSetter setModalState) {
    final isSelected = _selectedTextColor == color;
    return GestureDetector(
      onTap: () {
        setModalState(() {
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
            color: isSelected ? AppTheme.textLight : AppTheme.textMedium.withValues(alpha: 0.2),
            width: isSelected ? 3 : 1,
          ),
          boxShadow: [
            BoxShadow(
              color: Colors.black.withValues(alpha: 0.1),
              blurRadius: 4.r,
              offset: Offset(0, 2.h),
            ),
          ],
        ),
        child: isSelected
            ? Icon(
                Icons.check,
                color: color == Colors.white || color == AppTheme.accentMint 
                    ? Colors.black 
                    : Colors.white,
                size: 20.sp,
              )
            : null,
      ),
    );
  }

  Widget _buildEnhancedColorOption(Color color, StateSetter setModalState) {
    final isSelected = _selectedTextColor == color;
    return GestureDetector(
      onTap: () {
        setModalState(() {
          _selectedTextColor = color;
        });
      },
      child: AnimatedContainer(
        duration: Duration(milliseconds: 200),
        width: 48.w,
        height: 48.h,
        margin: EdgeInsets.only(right: 12.w),
        decoration: BoxDecoration(
          color: color,
          borderRadius: BorderRadius.circular(16.r),
          border: Border.all(
            color: isSelected ? Color(0xFF6C63FF) : AppTheme.textMedium.withValues(alpha: 0.2),
            width: isSelected ? 3 : 1,
          ),
          boxShadow: [
            BoxShadow(
              color: isSelected 
                  ? Color(0xFF6C63FF).withValues(alpha: 0.3)
                  : Colors.black.withValues(alpha: 0.1),
              blurRadius: isSelected ? 8.r : 4.r,
              offset: Offset(0, isSelected ? 4.h : 2.h),
            ),
          ],
        ),
        child: isSelected
            ? Icon(
                Icons.check_rounded,
                color: color == Colors.white || color.computeLuminance() > 0.7
                    ? Colors.black 
                    : Colors.white,
                size: 22.sp,
              )
            : null,
      ),
    );
  }

  Widget _buildColorPickerWithArrows(StateSetter setModalState) {
    final ScrollController _colorScrollController = ScrollController();
    final List<Color> colors = [
      Colors.black,
      Colors.white,
      Color(0xFF6C63FF),
      AppTheme.accentGold,
      AppTheme.accentCoral,
      AppTheme.accentMint,
      Colors.red,
      Colors.blue,
      Colors.green,
      Colors.orange,
      Colors.pink,
      Colors.purple,
      Colors.teal,
      Colors.indigo,
      Colors.amber,
      Colors.cyan,
    ];

    return Container(
      height: 60.h,
      child: Row(
        children: [
          // Left arrow
          GestureDetector(
            onTap: () {
              _colorScrollController.animateTo(
                _colorScrollController.offset - 120.w,
                duration: Duration(milliseconds: 300),
                curve: Curves.easeInOut,
              );
            },
            child: Container(
              width: 32.w,
              height: 48.h,
              decoration: BoxDecoration(
                color: Color(0xFF6C63FF).withValues(alpha: 0.1),
                borderRadius: BorderRadius.circular(12.r),
                border: Border.all(
                  color: Color(0xFF6C63FF).withValues(alpha: 0.2),
                ),
              ),
              child: Icon(
                Icons.chevron_left_rounded,
                color: Color(0xFF6C63FF),
                size: 20.sp,
              ),
            ),
          ),
          
          SizedBox(width: 8.w),
          
          // Color list
          Expanded(
            child: ListView.builder(
              controller: _colorScrollController,
              scrollDirection: Axis.horizontal,
              itemCount: colors.length,
              itemBuilder: (context, index) {
                return _buildEnhancedColorOption(colors[index], setModalState);
              },
            ),
          ),
          
          SizedBox(width: 8.w),
          
          // Right arrow
          GestureDetector(
            onTap: () {
              _colorScrollController.animateTo(
                _colorScrollController.offset + 120.w,
                duration: Duration(milliseconds: 300),
                curve: Curves.easeInOut,
              );
            },
            child: Container(
              width: 32.w,
              height: 48.h,
              decoration: BoxDecoration(
                color: Color(0xFF6C63FF).withValues(alpha: 0.1),
                borderRadius: BorderRadius.circular(12.r),
                border: Border.all(
                  color: Color(0xFF6C63FF).withValues(alpha: 0.2),
                ),
              ),
              child: Icon(
                Icons.chevron_right_rounded,
                color: Color(0xFF6C63FF),
                size: 20.sp,
              ),
            ),
          ),
        ],
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
        backgroundColor: AppTheme.accentMint,
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


  void _editTitle() {
    final titleController = TextEditingController(text: currentBoard.title);
    
    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        backgroundColor: AppTheme.cardBackground,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(20.r),
        ),
        title: Text(
          'Edit Title',
          style: TextStyle(
            color: AppTheme.textLight,
            fontSize: 20.sp,
            fontWeight: FontWeight.bold,
          ),
        ),
        content: TextField(
          controller: titleController,
          style: TextStyle(color: AppTheme.textLight),
          autofocus: true,
          decoration: InputDecoration(
            hintText: 'Enter board title',
            hintStyle: TextStyle(color: AppTheme.textMedium),
            enabledBorder: UnderlineInputBorder(
              borderSide: BorderSide(color: AppTheme.textMedium.withValues(alpha: 0.3)),
            ),
            focusedBorder: UnderlineInputBorder(
              borderSide: BorderSide(color: AppTheme.visionBoardGradient[0]),
            ),
          ),
          onSubmitted: (newTitle) {
            _updateBoardTitle(newTitle.trim());
            Navigator.pop(context, currentBoard);
          },
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context),
            child: Text(
              'Cancel',
              style: TextStyle(color: AppTheme.textMedium),
            ),
          ),
          TextButton(
            onPressed: () {
              _updateBoardTitle(titleController.text.trim());
              Navigator.pop(context, currentBoard);
            },
            child: Text(
              'Save',
              style: TextStyle(color: AppTheme.visionBoardGradient[0]),
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

  Future<Uint8List?> _captureThumbnail() async {
    try {
      print('📸 Capturing clean vision board thumbnail...');
      
      // Wait a frame for UI to settle
      await Future.delayed(Duration(milliseconds: 100));
      
      // Use the clean thumbnail canvas that has no selection indicators
      RenderRepaintBoundary? boundary = _thumbnailCanvasKey.currentContext?.findRenderObject() as RenderRepaintBoundary?;
      
      if (boundary == null) {
        print('❌ Failed to find clean thumbnail RenderRepaintBoundary');
        
        // Try to use main canvas as fallback
        boundary = _canvasKey.currentContext?.findRenderObject() as RenderRepaintBoundary?;
        if (boundary == null) {
          print('❌ Failed to find main canvas RenderRepaintBoundary as fallback');
          return null;
        }
        print('⚠️ Using main canvas as fallback for thumbnail');
      }
      
      // Wait for render boundary to be ready
      if (!boundary.debugNeedsPaint) {
        // Boundary is clean, safe to capture
        print('✅ Boundary is ready for capture');
      } else {
        print('⚠️ Boundary needs paint, waiting...');
        await Future.delayed(Duration(milliseconds: 200));
      }
      
      // Capture the image at a lower resolution for thumbnail
      // Using scale 0.7 for better quality thumbnail
      ui.Image image = await boundary.toImage(pixelRatio: 0.7);
      print('📸 Image captured: ${image.width}x${image.height}');
      
      // Convert to byte data
      ByteData? byteData = await image.toByteData(format: ui.ImageByteFormat.png);
      
      if (byteData == null) {
        print('❌ Failed to convert image to byte data');
        image.dispose();
        return null;
      }
      
      Uint8List pngBytes = byteData.buffer.asUint8List();
      print('✅ Clean thumbnail captured successfully (${pngBytes.length} bytes) - no selection indicators');
      
      // Dispose the image to free memory
      image.dispose();
      
      return pngBytes;
    } catch (e, stackTrace) {
      print('❌ Error capturing clean thumbnail: $e');
      print('❌ Stack trace: $stackTrace');
      return null;
    }
  }

  Future<Map<String, dynamic>?> _saveThumbnailToFile(Uint8List thumbnailBytes) async {
    try {
      // Get temporary directory
      final directory = await getTemporaryDirectory();
      
      // Generate filename
      final timestamp = DateTime.now().millisecondsSinceEpoch;
      final fileName = 'vision_board_thumbnail_$timestamp.png';
      final filePath = '${directory.path}/$fileName';
      
      // Write to file
      final file = File(filePath);
      await file.writeAsBytes(thumbnailBytes);
      
      print('✅ Thumbnail saved to: $filePath');
      
      return {
        'fileName': fileName,
        'filePath': filePath,
        'fileSize': thumbnailBytes.length,
      };
    } catch (e) {
      print('❌ Error saving thumbnail to file: $e');
      return null;
    }
  }

  Future<void> _saveBoard() async {
    try {
      print('🎯 Saving vision board to server...');
      print('🔍 DEBUG: Save board state:');
      print('  - currentBoard.id: ${currentBoard.id}');
      print('  - currentBoard.title: ${currentBoard.title}');
      print('  - currentBoard.elements.length: ${currentBoard.elements.length}');
      print('  - serverPictureId: $serverPictureId');
      print('  - hasBeenSavedToServer: $hasBeenSavedToServer');
      
      // Ensure the board has a proper ID if it's empty
      if (currentBoard.id.isEmpty) {
        currentBoard = currentBoard.copyWith(
          id: 'board_${DateTime.now().millisecondsSinceEpoch}',
        );
        print('🔍 DEBUG: Generated new board ID: ${currentBoard.id}');
      }
      
      // Determine if this is a new board or existing board
      final isNewBoard = serverPictureId == null;
      final boardDisplayName = currentBoard.title.isEmpty ? 'Untitled Vision Board' : currentBoard.title;
      
      // Show saving dialog
      showDialog(
        context: context,
        barrierDismissible: false,
        builder: (context) => SavingVisionBoardDialog(
          message: isNewBoard ? 'Creating Vision Board...' : 'Updating Vision Board...',
          boardName: boardDisplayName,
        ),
      );
      
      try {
        // Initialize service
        final visionBoardService = VisionBoardService();
        print('🔍 DEBUG: Initializing vision board service...');
        await visionBoardService.initialize(baseUrl: 'http://manifestation-qa-1.senthilguru.net');
        print('🔍 DEBUG: Service initialized successfully');
        
        print('🔍 DEBUG: isNewBoard: $isNewBoard (based on serverPictureId == null)');
        
        // Capture thumbnail before saving
        Map<String, dynamic>? thumbnailInfo;
        final thumbnailBytes = await _captureThumbnail();
        if (thumbnailBytes != null) {
          thumbnailInfo = await _saveThumbnailToFile(thumbnailBytes);
          print('🔍 DEBUG: Thumbnail info: $thumbnailInfo');
        } else {
          print('⚠️ Warning: Could not capture thumbnail');
        }
        
        if (isNewBoard) {
          // Create new vision board on server
          print('🔍 DEBUG: Calling createVisionBoard with thumbnail...');
          final result = await visionBoardService.createVisionBoard(
            currentBoard,
            thumbnailInfo: thumbnailInfo,
          );
          print('🔍 DEBUG: createVisionBoard result: isSuccess=${result.isSuccess}, message=${result.message}, data=${result.data}');
          
          // Hide saving dialog
          Navigator.of(context).pop();
          
          if (result.isSuccess) {
            print('✅ Vision board created on server with picture ID: ${result.data}');
            serverPictureId = result.data; // Store the server picture ID
            print('🔍 DEBUG: Stored serverPictureId: $serverPictureId');
            lastSavedState = currentBoard.copyWith();
            hasUnsavedChanges = false;
            hasBeenSavedToServer = true; // Mark as saved to server
            
            ScaffoldMessenger.of(context).showSnackBar(
              SnackBar(
                content: Text('Vision board created successfully!'),
                backgroundColor: AppTheme.accentMint,
                duration: Duration(seconds: 1),
              ),
            );
            
            Navigator.pushReplacement(
              context,
              MaterialPageRoute(builder: (context) => VisionBoardHome()),
            );
          } else {
            print('🔍 DEBUG: Create failed - showing error: ${result.message}');
            _showErrorSnackBar('Failed to create vision board: ${result.message}');
          }
        } else {
          // Update existing vision board on server using stored picture ID
          final result = await visionBoardService.updateVisionBoard(
            currentBoard, 
            pictureId: serverPictureId,
            thumbnailInfo: thumbnailInfo,
          );
          
          // Hide saving dialog
          Navigator.of(context).pop();
          
          if (result.isSuccess) {
            print('✅ Vision board updated on server');
            lastSavedState = currentBoard.copyWith();
            hasUnsavedChanges = false;
            
            ScaffoldMessenger.of(context).showSnackBar(
              SnackBar(
                content: Text('Vision board updated successfully!'),
                backgroundColor: AppTheme.accentMint,
                duration: Duration(seconds: 1),
              ),
            );
            
            Navigator.pushReplacement(
              context,
              MaterialPageRoute(builder: (context) => VisionBoardHome()),
            );
          } else {
            _showErrorSnackBar('Failed to update vision board: ${result.message}');
          }
        }
      } catch (e) {
        // Make sure to hide the dialog even if there's an error
        Navigator.of(context).pop();
        throw e; // Re-throw the error to be handled by the outer catch
      }
      
    } catch (e) {
      print('❌ Save board error: $e');
      // Ensure dialog is hidden in case of any error
      if (Navigator.canPop(context)) {
        Navigator.of(context).pop();
      }
      _showErrorSnackBar('Failed to save vision board: $e');
    }
  }
  
  Future<void> _loadBoardElements() async {
    if (widget.existingBoard != null && widget.existingBoard!.id.isNotEmpty) {
      print('📝 Loading full board content from server for: ${widget.existingBoard!.id}');
      
      // Set loading state
      setState(() {
        isLoadingContent = true;
      });
      
      try {
        // Initialize vision board service
        final visionBoardService = VisionBoardService();
        await visionBoardService.initialize(baseUrl: 'http://manifestation-qa-1.senthilguru.net');
        
        // Load full board content from server
        final result = await visionBoardService.getVisionBoard(widget.existingBoard!.id);
        
        if (result.isSuccess && result.data != null) {
          print('✅ Loaded full board with ${result.data!.elements.length} elements');
          
          // Get the picture ID for this existing board so we can update it later
          try {
            final pictureId = await visionBoardService.getPictureIdForBoard(widget.existingBoard!.id);
            if (pictureId != null) {
              serverPictureId = pictureId;
              print('🔍 DEBUG: Retrieved serverPictureId: $serverPictureId for existing board');
            }
          } catch (e) {
            print('⚠️ Could not get picture ID for existing board: $e');
          }
          
          // Update current board with full content from server
          setState(() {
            currentBoard = result.data!;
            isLoadingContent = false;
          });
          
          // Clear undo stack and create new initial state
          undoStack.clear();
          undoIndex = -1;
          _saveToUndoStack();
          
        } else {
          print('❌ Failed to load board content: ${result.message}');
          // Keep the lightweight board but show empty state
          setState(() {
            isLoadingContent = false;
          });
        }
        
      } catch (e) {
        print('❌ Error loading board elements: $e');
        // Keep the lightweight board but show empty state
        setState(() {
          isLoadingContent = false;
        });
      }
    }
  }
  
  // NOTE: API loading disabled - using XML storage only
  /* Future<void> _loadBoardFromAPI(String sessionId) async {
    // Implementation commented out for XML-only mode
  } */

  // NOTE: API loading disabled - using XML storage only
  /* Future<List<VisionBoard>> _loadSavedBoards() async {
    // Implementation commented out for XML-only mode
    return [];
  } */

  // NOTE: API deletion disabled - using XML storage only
  /* Future<void> _deleteSavedBoard(String boardId) async {
    // Implementation commented out for XML-only mode
  } */


  Widget _buildBackgroundSheet() {
    final backgroundColors = [
      Colors.white,
      AppTheme.backgroundMedium,
      AppTheme.backgroundDark,
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
        color: AppTheme.cardBackground,
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
              color: AppTheme.textMedium.withValues(alpha: 0.3),
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
                    color: AppTheme.textLight,
                  ),
                ),
                GestureDetector(
                  onTap: () => Navigator.pop(context),
                  child: Container(
                    width: 32.w,
                    height: 32.h,
                    decoration: BoxDecoration(
                      color: AppTheme.backgroundMedium,
                      borderRadius: BorderRadius.circular(8.r),
                    ),
                    child: Icon(
                      Icons.close,
                      color: AppTheme.textMedium,
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
                      Navigator.pop(context, currentBoard);
                    },
                    child: Container(
                      decoration: BoxDecoration(
                        color: color,
                        borderRadius: BorderRadius.circular(16.r),
                        border: Border.all(
                          color: isSelected 
                              ? AppTheme.visionBoardGradient[0] 
                              : AppTheme.textMedium.withValues(alpha: 0.2),
                          width: isSelected ? 3 : 1,
                        ),
                        boxShadow: [
                          BoxShadow(
                            color: Colors.black.withValues(alpha: 0.1),
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
        backgroundColor: AppTheme.accentMint,
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


  void _handleBackButton() {
    if (hasUnsavedChanges) {
      _showExitDialog();
    } else {
      Navigator.pop(context, currentBoard);
    }
  }
  
  void _showExitDialog() {
    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        backgroundColor: AppTheme.cardBackground,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(20.r),
        ),
        title: Text(
          'Save Changes?',
          style: TextStyle(
            color: AppTheme.textLight,
            fontSize: 20.sp,
            fontWeight: FontWeight.bold,
          ),
        ),
        content: Text(
          'You have unsaved changes. Would you like to save before leaving?',
          style: TextStyle(
            color: AppTheme.textMedium,
            fontSize: 16.sp,
          ),
        ),
        actions: [
          TextButton(
            onPressed: () {
              Navigator.pop(context, currentBoard);
              Navigator.pushReplacement(
                context,
                MaterialPageRoute(builder: (context) => VisionBoardHome()),
              );
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
              style: TextStyle(color: AppTheme.textMedium),
            ),
          ),
          TextButton(
            onPressed: () {
              Navigator.pop(context, currentBoard);
              _saveBoard().then((_) {
                Navigator.pushReplacement(
                  context,
                  MaterialPageRoute(builder: (context) => VisionBoardHome()),
                );
              });
            },
            child: Text(
              'Save',
              style: TextStyle(color: AppTheme.visionBoardGradient[0]),
            ),
          ),
        ],
      ),
    );
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

  void _addImageFromURL() async {
    showModalBottomSheet(
      context: context,
      backgroundColor: Colors.transparent,
      isScrollControlled: true,
      builder: (context) => _buildURLImageInput(),
    );
  }

  Widget _buildURLImageInput() {
    final TextEditingController _urlController = TextEditingController();
    bool _isValidUrl = false;
    String? _previewUrl;
    
    return StatefulBuilder(
      builder: (context, setModalState) => GestureDetector(
        onTap: () {
          // Dismiss keyboard when tapping anywhere on the sheet
          FocusScope.of(context).unfocus();
        },
        child: Container(
          height: MediaQuery.of(context).size.height * 0.75,
          margin: EdgeInsets.all(16.w),
          decoration: BoxDecoration(
            gradient: LinearGradient(
              colors: [
                AppTheme.cardBackground,
                AppTheme.cardBackground.withValues(alpha: 0.95),
              ],
              begin: Alignment.topCenter,
              end: Alignment.bottomCenter,
            ),
            borderRadius: BorderRadius.circular(28.r),
            border: Border.all(
              color: AppTheme.journalGradient[0].withValues(alpha: 0.2),
              width: 1.5,
            ),
            boxShadow: [
              BoxShadow(
                color: AppTheme.journalGradient[0].withValues(alpha: 0.15),
                blurRadius: 30.r,
                offset: Offset(0, 15.h),
              ),
              BoxShadow(
                color: AppTheme.black.withValues(alpha: 0.1),
                blurRadius: 20.r,
                offset: Offset(0, 8.h),
              ),
            ],
          ),
          child: Column(
          children: [
            // Handle
            Container(
              width: 50.w,
              height: 5.h,
              margin: EdgeInsets.only(top: 12.h),
              decoration: BoxDecoration(
                gradient: LinearGradient(
                  colors: AppTheme.journalGradient,
                ),
                borderRadius: BorderRadius.circular(3.r),
              ),
            ),
            
            // Header
            Padding(
              padding: EdgeInsets.all(24.w),
              child: Row(
                children: [
                  // Icon with gradient background
                  Container(
                    width: 48.w,
                    height: 48.h,
                    decoration: BoxDecoration(
                      gradient: LinearGradient(
                        colors: AppTheme.journalGradient,
                      ),
                      borderRadius: BorderRadius.circular(16.r),
                      boxShadow: [
                        BoxShadow(
                          color: AppTheme.journalGradient[0].withValues(alpha: 0.3),
                          blurRadius: 12.r,
                          offset: Offset(0, 6.h),
                        ),
                      ],
                    ),
                    child: Icon(
                      Icons.link_rounded,
                      color: AppTheme.white,
                      size: 24.sp,
                    ),
                  ),
                  
                  SizedBox(width: 16.w),
                  
                  // Title and subtitle
                  Expanded(
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text(
                          'Add Image URL',
                          style: TextStyle(
                            fontSize: 24.sp,
                            fontWeight: FontWeight.bold,
                            color: AppTheme.textPrimary,
                          ),
                        ),
                        SizedBox(height: 4.h),
                        Text(
                          'Paste an image URL from the internet',
                          style: TextStyle(
                            fontSize: 14.sp,
                            color: AppTheme.textSecondary,
                          ),
                        ),
                      ],
                    ),
                  ),
                  
                  // Close button
                  GestureDetector(
                    onTap: () => Navigator.pop(context),
                    child: Container(
                      width: 40.w,
                      height: 40.h,
                      decoration: BoxDecoration(
                        color: AppTheme.textMedium.withValues(alpha: 0.1),
                        borderRadius: BorderRadius.circular(12.r),
                      ),
                      child: Icon(
                        Icons.close_rounded,
                        color: AppTheme.textMedium,
                        size: 20.sp,
                      ),
                    ),
                  ),
                ],
              ),
            ),
            
            // URL input field
            Padding(
              padding: EdgeInsets.symmetric(horizontal: 24.w),
              child: Container(
                decoration: BoxDecoration(
                  color: AppTheme.backgroundMedium.withValues(alpha: 0.5),
                  borderRadius: BorderRadius.circular(16.r),
                  border: Border.all(
                    color: _isValidUrl 
                        ? AppTheme.accentMint.withValues(alpha: 0.5)
                        : AppTheme.journalGradient[0].withValues(alpha: 0.2),
                    width: _isValidUrl ? 2 : 1,
                  ),
                ),
                child: TextField(
                  controller: _urlController,
                  style: TextStyle(
                    fontSize: 16.sp,
                    color: AppTheme.textPrimary,
                  ),
                  decoration: InputDecoration(
                    hintText: 'https://example.com/image.jpg',
                    hintStyle: TextStyle(
                      color: AppTheme.textMedium.withValues(alpha: 0.6),
                      fontSize: 16.sp,
                    ),
                    prefixIcon: Icon(
                      Icons.link_rounded,
                      color: AppTheme.journalGradient[0],
                      size: 24.sp,
                    ),
                    suffixIcon: _isValidUrl 
                        ? Icon(
                            Icons.check_circle_rounded,
                            color: AppTheme.accentMint,
                            size: 24.sp,
                          )
                        : null,
                    border: InputBorder.none,
                    contentPadding: EdgeInsets.all(16.w),
                  ),
                  onChanged: (value) {
                    setModalState(() {
                      _isValidUrl = _isValidImageUrl(value);
                      _previewUrl = _isValidUrl ? value : null;
                    });
                  },
                ),
              ),
            ),
            
            SizedBox(height: 24.h),
            
            // Preview area
            Expanded(
              child: Padding(
                padding: EdgeInsets.symmetric(horizontal: 24.w),
                child: Container(
                  width: double.infinity,
                  decoration: BoxDecoration(
                    color: AppTheme.backgroundMedium.withValues(alpha: 0.3),
                    borderRadius: BorderRadius.circular(20.r),
                    border: Border.all(
                      color: AppTheme.textMedium.withValues(alpha: 0.1),
                    ),
                  ),
                  child: _previewUrl != null
                      ? ClipRRect(
                          borderRadius: BorderRadius.circular(20.r),
                          child: Image.network(
                            _previewUrl!,
                            fit: BoxFit.contain,
                            loadingBuilder: (context, child, loadingProgress) {
                              if (loadingProgress == null) return child;
                              return Center(
                                child: Column(
                                  mainAxisAlignment: MainAxisAlignment.center,
                                  children: [
                                    CircularProgressIndicator(
                                      color: AppTheme.journalGradient[0],
                                      value: loadingProgress.expectedTotalBytes != null
                                          ? loadingProgress.cumulativeBytesLoaded /
                                              loadingProgress.expectedTotalBytes!
                                          : null,
                                    ),
                                    SizedBox(height: 16.h),
                                    Text(
                                      'Loading preview...',
                                      style: TextStyle(
                                        fontSize: 14.sp,
                                        color: AppTheme.textMedium,
                                      ),
                                    ),
                                  ],
                                ),
                              );
                            },
                            errorBuilder: (context, error, stackTrace) {
                              return Column(
                                mainAxisAlignment: MainAxisAlignment.center,
                                children: [
                                  Icon(
                                    Icons.error_outline_rounded,
                                    color: Colors.red.withValues(alpha: 0.6),
                                    size: 48.sp,
                                  ),
                                  SizedBox(height: 16.h),
                                  Text(
                                    'Failed to load image',
                                    style: TextStyle(
                                      fontSize: 16.sp,
                                      fontWeight: FontWeight.w600,
                                      color: Colors.red.withValues(alpha: 0.8),
                                    ),
                                  ),
                                  SizedBox(height: 8.h),
                                  Text(
                                    'Please check the URL and try again',
                                    style: TextStyle(
                                      fontSize: 14.sp,
                                      color: AppTheme.textMedium.withValues(alpha: 0.6),
                                    ),
                                    textAlign: TextAlign.center,
                                  ),
                                ],
                              );
                            },
                          ),
                        )
                      : Column(
                          mainAxisAlignment: MainAxisAlignment.center,
                          children: [
                            Icon(
                              Icons.image_outlined,
                              color: AppTheme.textMedium.withValues(alpha: 0.6),
                              size: 64.sp,
                            ),
                            SizedBox(height: 16.h),
                            Text(
                              'Paste image URL above',
                              style: TextStyle(
                                fontSize: 18.sp,
                                fontWeight: FontWeight.w600,
                                color: AppTheme.textMedium.withValues(alpha: 0.8),
                              ),
                            ),
                            SizedBox(height: 8.h),
                            Text(
                              'Copy an image URL from the web and paste it here to add to your vision board',
                              style: TextStyle(
                                fontSize: 14.sp,
                                color: AppTheme.textMedium.withValues(alpha: 0.6),
                              ),
                              textAlign: TextAlign.center,
                            ),
                          ],
                        ),
                ),
              ),
            ),
            
            // Action buttons
            Padding(
              padding: EdgeInsets.all(24.w),
              child: Row(
                children: [
                  // Cancel button
                  Expanded(
                    child: GestureDetector(
                      onTap: () => Navigator.pop(context),
                      child: Container(
                        height: 56.h,
                        decoration: BoxDecoration(
                          color: AppTheme.textMedium.withValues(alpha: 0.1),
                          borderRadius: BorderRadius.circular(16.r),
                        ),
                        child: Center(
                          child: Text(
                            'Cancel',
                            style: TextStyle(
                              fontSize: 16.sp,
                              fontWeight: FontWeight.w600,
                              color: AppTheme.textMedium,
                            ),
                          ),
                        ),
                      ),
                    ),
                  ),
                  
                  SizedBox(width: 16.w),
                  
                  // Add button
                  Expanded(
                    flex: 2,
                    child: GestureDetector(
                      onTap: _isValidUrl
                          ? () {
                              _addImageFromUrlToBoard(_urlController.text.trim());
                              Navigator.pop(context, currentBoard);
                            }
                          : null,
                      child: Container(
                        height: 56.h,
                        decoration: BoxDecoration(
                          gradient: _isValidUrl
                              ? LinearGradient(colors: AppTheme.journalGradient)
                              : null,
                          color: _isValidUrl ? null : AppTheme.textMedium.withValues(alpha: 0.3),
                          borderRadius: BorderRadius.circular(16.r),
                          boxShadow: _isValidUrl
                              ? [
                                  BoxShadow(
                                    color: AppTheme.journalGradient[0].withValues(alpha: 0.4),
                                    blurRadius: 16.r,
                                    offset: Offset(0, 8.h),
                                  ),
                                ]
                              : null,
                        ),
                        child: Center(
                          child: Row(
                            mainAxisAlignment: MainAxisAlignment.center,
                            children: [
                              Icon(
                                Icons.add_rounded,
                                color: _isValidUrl ? AppTheme.white : AppTheme.textMedium.withValues(alpha: 0.6),
                                size: 20.sp,
                              ),
                              SizedBox(width: 8.w),
                              Text(
                                'Add to Vision Board',
                                style: TextStyle(
                                  fontSize: 16.sp,
                                  fontWeight: FontWeight.bold,
                                  color: _isValidUrl ? AppTheme.white : AppTheme.textMedium.withValues(alpha: 0.6),
                                ),
                              ),
                            ],
                          ),
                        ),
                      ),
                    ),
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
      ),
    );
  }

  bool _isValidImageUrl(String url) {
    if (url.isEmpty) return false;
    
    // Basic URL validation
    final Uri? uri = Uri.tryParse(url);
    if (uri == null || !uri.hasScheme || !uri.hasAuthority) return false;
    
    // Check for common image extensions
    final imageExtensions = ['.jpg', '.jpeg', '.png', '.gif', '.webp', '.bmp', '.svg'];
    final lowercaseUrl = url.toLowerCase();
    
    return imageExtensions.any((ext) => lowercaseUrl.contains(ext)) || 
           lowercaseUrl.contains('image') ||
           lowercaseUrl.contains('img') ||
           (uri.scheme == 'https' && uri.host.isNotEmpty);
  }

  void _addImageFromUrlToBoard(String imageUrl) {
    // Create image element with URL
    final imageElement = VisionBoardImageElement(
      id: DateTime.now().millisecondsSinceEpoch.toString(),
      position: Offset(100.w, 100.h),
      size: Size(200.w, 200.h),
      imagePath: '', // No local path for URL images
      imageUrl: imageUrl,
      fit: BoxFit.cover,
    );

    final updatedElements = [...currentBoard.elements, imageElement];
    final updatedBoard = currentBoard.copyWith(elements: updatedElements);
    
    _onBoardChanged(updatedBoard);
    
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Row(
          children: [
            Icon(Icons.check_circle, color: AppTheme.white, size: 20.sp),
            SizedBox(width: 12.w),
            Text(
              'Image added from URL!',
              style: TextStyle(
                color: AppTheme.white,
                fontWeight: FontWeight.w500,
              ),
            ),
          ],
        ),
        backgroundColor: AppTheme.accentMint,
        behavior: SnackBarBehavior.floating,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(12.r),
        ),
        margin: EdgeInsets.all(16.w),
        duration: Duration(seconds: 2),
      ),
    );
  }
  
  void _showDefaultImagesDialog(List<Map<String, dynamic>> defaultImages) {
    showModalBottomSheet(
      context: context,
      backgroundColor: Colors.transparent,
      isScrollControlled: true,
      builder: (context) => Container(
        height: MediaQuery.of(context).size.height * 0.7,
        margin: EdgeInsets.all(20.w),
        decoration: BoxDecoration(
          color: AppTheme.cardBackground,
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
                color: AppTheme.textMedium.withValues(alpha: 0.3),
                borderRadius: BorderRadius.circular(2.r),
              ),
            ),
            
            // Header
            Padding(
              padding: EdgeInsets.all(24.w),
              child: Text(
                'Choose Stock Image',
                style: TextStyle(
                  fontSize: 24.sp,
                  fontWeight: FontWeight.bold,
                  color: AppTheme.textLight,
                ),
              ),
            ),
            
            // Images grid
            Expanded(
              child: GridView.builder(
                padding: EdgeInsets.symmetric(horizontal: 24.w),
                gridDelegate: SliverGridDelegateWithFixedCrossAxisCount(
                  crossAxisCount: 2,
                  crossAxisSpacing: 16.w,
                  mainAxisSpacing: 16.h,
                  childAspectRatio: 1.0,
                ),
                itemCount: defaultImages.length,
                itemBuilder: (context, index) {
                  final image = defaultImages[index];
                  return GestureDetector(
                    onTap: () {
                      Navigator.pop(context, currentBoard);
                      _addDefaultImageToBoard(image);
                    },
                    child: Container(
                      decoration: BoxDecoration(
                        color: AppTheme.backgroundMedium,
                        borderRadius: BorderRadius.circular(16.r),
                        border: Border.all(
                          color: AppTheme.textMedium.withValues(alpha: 0.2),
                        ),
                      ),
                      child: ClipRRect(
                        borderRadius: BorderRadius.circular(16.r),
                        child: Stack(
                          fit: StackFit.expand,
                          children: [
                            // Try to load the image, fallback to placeholder
                            if (image['location'] != null && image['location'].toString().isNotEmpty)
                              Image.network(
                                image['location'].toString(),
                                fit: BoxFit.cover,
                                errorBuilder: (context, error, stackTrace) => 
                                    _buildSimplePlaceholder(),
                              )
                            else
                              _buildSimplePlaceholder(),
                            
                            // Overlay with image info
                            Positioned(
                              bottom: 0,
                              left: 0,
                              right: 0,
                              child: Container(
                                padding: EdgeInsets.all(8.w),
                                decoration: BoxDecoration(
                                  gradient: LinearGradient(
                                    begin: Alignment.topCenter,
                                    end: Alignment.bottomCenter,
                                    colors: [
                                      Colors.transparent,
                                      Colors.black.withValues(alpha: 0.7),
                                    ],
                                  ),
                                ),
                                child: Text(
                                  image['fileName']?.toString() ?? 'Stock Image',
                                  style: TextStyle(
                                    color: Colors.white,
                                    fontSize: 12.sp,
                                    fontWeight: FontWeight.w600,
                                  ),
                                  textAlign: TextAlign.center,
                                  maxLines: 1,
                                  overflow: TextOverflow.ellipsis,
                                ),
                              ),
                            ),
                          ],
                        ),
                      ),
                    ),
                  );
                },
              ),
            ),
            
            SizedBox(height: 20.h),
          ],
        ),
      ),
    );
  }
  
  void _addDefaultImageToBoard(Map<String, dynamic> defaultImage) {
    // Create image element using the default image
    final imageElement = VisionBoardImageElement(
      id: DateTime.now().millisecondsSinceEpoch.toString(),
      position: Offset(100.w, 100.h),
      size: Size(200.w, 200.h),
      imagePath: '', // No local path for default images
      imageUrl: defaultImage['location']?.toString(), // Use the API image URL
      fit: BoxFit.cover,
    );

    final updatedElements = [...currentBoard.elements, imageElement];
    final updatedBoard = currentBoard.copyWith(elements: updatedElements);
    
    _onBoardChanged(updatedBoard);
    
    // NOTE: Picture tracking now handled by analytics service
    if (_sessionStarted) {
      _analyticsService.updateSessionMetadata(
        ActivityType.visionBoard,
        {
          'lastDefaultImageUsed': defaultImage['defaultVisionBoardPictureName'],
          'defaultImageCount': (currentBoard.elements.length + 1),
        },
      );
    }
    
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text('Stock image added to vision board'),
        backgroundColor: AppTheme.accentMint,
        duration: Duration(seconds: 2),
      ),
    );
  }


  Future<void> _addImageToBoard(XFile image) async {
    try {
      // Get the app's documents directory for local storage
      final Directory appDir = await getApplicationDocumentsDirectory();
      final String fileName = 'vision_board_${DateTime.now().millisecondsSinceEpoch}.jpg';
      final String localPath = '${appDir.path}/$fileName';
      
      // Copy the image to local storage for immediate use
      await File(image.path).copy(localPath);
      
      // API upload disabled - using local storage only
      String? apiImageUrl;
      
      // Create image element with both local and API paths
      final imageElement = VisionBoardImageElement(
        id: DateTime.now().millisecondsSinceEpoch.toString(),
        position: Offset(100.w, 100.h),
        size: Size(200.w, 200.h),
        imagePath: localPath,
        imageUrl: apiImageUrl, // Will be null if API upload failed
        fit: BoxFit.cover,
      );

      final updatedElements = [...currentBoard.elements, imageElement];
      final updatedBoard = currentBoard.copyWith(elements: updatedElements);
      
      _onBoardChanged(updatedBoard);
      
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Image added locally'),
          backgroundColor: AppTheme.accentMint,
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
        backgroundColor: AppTheme.accentMint,
        duration: Duration(seconds: 2),
      ),
    );
  }

  Widget _buildSimplePlaceholder() {
    return Container(
      color: AppTheme.backgroundMedium,
      child: Center(
        child: Icon(
          Icons.image_outlined,
          color: AppTheme.textMedium,
          size: 32.sp,
        ),
      ),
    );
  }

  void _showErrorSnackBar(String message) {
    if (!mounted) return; // Check if widget is still mounted
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