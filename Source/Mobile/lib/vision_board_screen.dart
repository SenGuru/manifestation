import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:manifestation_app/vision_board.dart';
import 'package:manifestation_app/vision_board_canvas.dart';
import 'package:manifestation_app/vision_board_toolbar.dart';
import '../colors.dart';

class VisionBoardScreen extends StatefulWidget {
  final VisionBoard? existingBoard;

  const VisionBoardScreen({
    Key? key,
    this.existingBoard,
  }) : super(key: key);

  @override
  _VisionBoardScreenState createState() => _VisionBoardScreenState();
}

class _VisionBoardScreenState extends State<VisionBoardScreen> {
  late VisionBoard currentBoard;
  bool isEditing = false;
  VisionBoardTool selectedTool = VisionBoardTool.select;
  List<VisionBoard> undoStack = [];
  int undoIndex = -1;
  
  @override
  void initState() {
    super.initState();
    currentBoard = widget.existingBoard ?? VisionBoard.empty();
    _saveToUndoStack();
  }

  void _saveToUndoStack() {
    // Remove any redo states
    if (undoIndex < undoStack.length - 1) {
      undoStack = undoStack.sublist(0, undoIndex + 1);
    }
    
    // Add current state
    undoStack.add(currentBoard);
    undoIndex = undoStack.length - 1;
    
    // Limit undo stack size
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
            // Header
            _buildHeader(),
            
            // Canvas area
            Expanded(
              child: Container(
                margin: EdgeInsets.symmetric(horizontal: 20.w),
                decoration: BoxDecoration(
                  color: AppColors.white,
                  borderRadius: BorderRadius.circular(20.r),
                  boxShadow: [
                    BoxShadow(
                      color: AppColors.black.withOpacity(0.1),
                      blurRadius: 20.r,
                      offset: Offset(0, 10.h),
                    ),
                  ],
                ),
                child: ClipRRect(
                  borderRadius: BorderRadius.circular(20.r),
                  child: VisionBoardCanvas(
                    board: currentBoard,
                    selectedTool: selectedTool,
                    onBoardChanged: _onBoardChanged,
                  ),
                ),
              ),
            ),
            
            SizedBox(height: 20.h),
            
            // Toolbar
            Builder(
              builder: (context) => VisionBoardToolbar(
                selectedTool: selectedTool,
                onToolSelected: _onToolSelected,
                onAddImage: _addImage,
                onAddText: _addText,
                onUndo: _undo,
                onRedo: _redo,
                onSave: _saveBoard,
                canUndo: undoIndex > 0,
                canRedo: undoIndex < undoStack.length - 1,
              ),
            ),
            
            SizedBox(height: 20.h),
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
                GestureDetector(
                  onTap: _editTitle,
                  child: Text(
                    currentBoard.title.isEmpty ? 'Untitled Board' : currentBoard.title,
                    style: TextStyle(
                      fontSize: 24.sp,
                      fontWeight: FontWeight.bold,
                      color: AppColors.textLight,
                    ),
                  ),
                ),
                Text(
                  'Visualize your dreams • ${currentBoard.elements.length} items',
                  style: TextStyle(
                    fontSize: 14.sp,
                    color: AppColors.textMedium,
                  ),
                ),
              ],
            ),
          ),
          
          // Zoom controls
          Container(
            margin: EdgeInsets.only(right: 12.w),
            padding: EdgeInsets.symmetric(horizontal: 8.w, vertical: 4.h),
            decoration: BoxDecoration(
              color: AppColors.cardBackground,
              borderRadius: BorderRadius.circular(8.r),
              border: Border.all(
                color: AppColors.textMedium.withOpacity(0.2),
              ),
            ),
            child: Row(
              mainAxisSize: MainAxisSize.min,
              children: [
                GestureDetector(
                  onTap: _zoomOut,
                  child: Icon(
                    Icons.zoom_out,
                    color: AppColors.textMedium,
                    size: 18.sp,
                  ),
                ),
                SizedBox(width: 8.w),
                Text(
                  '100%',
                  style: TextStyle(
                    fontSize: 12.sp,
                    color: AppColors.textMedium,
                    fontWeight: FontWeight.w500,
                  ),
                ),
                SizedBox(width: 8.w),
                GestureDetector(
                  onTap: _zoomIn,
                  child: Icon(
                    Icons.zoom_in,
                    color: AppColors.textMedium,
                    size: 18.sp,
                  ),
                ),
              ],
            ),
          ),
          
          // Templates button
          GestureDetector(
            onTap: _showTemplates,
            child: Container(
              padding: EdgeInsets.symmetric(horizontal: 16.w, vertical: 8.h),
              decoration: BoxDecoration(
                gradient: LinearGradient(
                  colors: AppColors.visionBoardGradient,
                ),
                borderRadius: BorderRadius.circular(12.r),
              ),
              child: Row(
                mainAxisSize: MainAxisSize.min,
                children: [
                  Icon(
                    Icons.grid_view,
                    color: AppColors.white,
                    size: 16.sp,
                  ),
                  SizedBox(width: 4.w),
                  Text(
                    'Templates',
                    style: TextStyle(
                      fontSize: 12.sp,
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

  void _onBoardChanged(VisionBoard newBoard) {
    setState(() {
      currentBoard = newBoard;
      isEditing = true;
    });
    _saveToUndoStack();
  }

  void _onToolSelected(VisionBoardTool tool) {
    setState(() {
      selectedTool = tool;
    });
  }

  void _showToolSelector(BuildContext context) {
    showModalBottomSheet(
      context: context,
      backgroundColor: Colors.transparent,
      builder: (context) => Container(
        margin: EdgeInsets.all(20.w),
        decoration: BoxDecoration(
          color: AppColors.cardBackground,
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
                color: AppColors.textMedium.withOpacity(0.3),
                borderRadius: BorderRadius.circular(2.r),
              ),
            ),
            Padding(
              padding: EdgeInsets.all(24.w),
              child: Column(
                children: [
                  Text(
                    'Select Tool',
                    style: TextStyle(
                      fontSize: 18.sp,
                      fontWeight: FontWeight.bold,
                      color: AppColors.textLight,
                    ),
                  ),
                  SizedBox(height: 24.h),
                  Wrap(
                    spacing: 16.w,
                    runSpacing: 16.h,
                    children: VisionBoardTool.values.map((tool) {
                      final isSelected = selectedTool == tool;
                      return GestureDetector(
                        onTap: () {
                          Navigator.pop(context);
                          _onToolSelected(tool);
                        },
                        child: AnimatedContainer(
                          duration: Duration(milliseconds: 200),
                          width: 80.w,
                          padding: EdgeInsets.symmetric(
                            horizontal: 12.w,
                            vertical: 16.h,
                          ),
                          decoration: BoxDecoration(
                            gradient: isSelected
                                ? LinearGradient(
                                    colors: [
                                      AppColors.gradientPurple.withOpacity(0.1),
                                      AppColors.gradientBlue.withOpacity(0.1),
                                    ],
                                  )
                                : null,
                            color: isSelected ? null : AppColors.backgroundMedium,
                            borderRadius: BorderRadius.circular(12.r),
                            border: Border.all(
                              color: isSelected
                                  ? AppColors.gradientPurple.withOpacity(0.3)
                                  : AppColors.textMedium.withOpacity(0.2),
                              width: isSelected ? 2 : 1,
                            ),
                            boxShadow: isSelected ? [
                              BoxShadow(
                                color: AppColors.gradientPurple.withOpacity(0.2),
                                blurRadius: 8.r,
                                offset: Offset(0, 4.h),
                              ),
                            ] : null,
                          ),
                          child: Column(
                            children: [
                              AnimatedContainer(
                                duration: Duration(milliseconds: 200),
                                padding: EdgeInsets.all(8.w),
                                decoration: BoxDecoration(
                                  color: isSelected 
                                      ? AppColors.gradientPurple.withOpacity(0.1)
                                      : Colors.transparent,
                                  borderRadius: BorderRadius.circular(8.r),
                                ),
                                child: Icon(
                                  tool.icon,
                                  color: isSelected
                                      ? AppColors.gradientPurple
                                      : AppColors.textMedium,
                                  size: 24.sp,
                                ),
                              ),
                              SizedBox(height: 8.h),
                              Text(
                                tool.name,
                                style: TextStyle(
                                  fontSize: 12.sp,
                                  fontWeight: isSelected ? FontWeight.w600 : FontWeight.w500,
                                  color: isSelected
                                      ? AppColors.gradientPurple
                                      : AppColors.textMedium,
                                ),
                                textAlign: TextAlign.center,
                              ),
                              if (isSelected) ...[
                                SizedBox(height: 4.h),
                                Container(
                                  width: 20.w,
                                  height: 2.h,
                                  decoration: BoxDecoration(
                                    color: AppColors.gradientPurple,
                                    borderRadius: BorderRadius.circular(1.r),
                                  ),
                                ),
                              ],
                            ],
                          ),
                        ),
                      );
                    }).toList(),
                  ),
                  SizedBox(height: 16.h),
                  Text(
                    _getToolDescription(selectedTool),
                    style: TextStyle(
                      fontSize: 12.sp,
                      color: AppColors.textMedium,
                      height: 1.4,
                    ),
                    textAlign: TextAlign.center,
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }

  String _getToolDescription(VisionBoardTool tool) {
    switch (tool) {
      case VisionBoardTool.select:
        return 'Tap to select elements. Drag to move selected items.';
      case VisionBoardTool.move:
        return 'Drag any element to move it around the canvas.';
      case VisionBoardTool.resize:
        return 'Select an element to see resize handles at the corners.';
      case VisionBoardTool.rotate:
        return 'Select an element to see the rotation handle above it.';
      case VisionBoardTool.delete:
        return 'Select elements to see the delete button.';
    }
  }

  void _addImage() {
    showModalBottomSheet(
      context: context,
      backgroundColor: Colors.transparent,
      builder: (context) => _buildImageSourceSelector(),
    );
  }

  void _addText() {
    _showTextEditor();
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

  void _zoomIn() {
    // TODO: Implement zoom in functionality
    print('Zoom in');
  }

  void _zoomOut() {
    // TODO: Implement zoom out functionality
    print('Zoom out');
  }

  void _editTitle() {
    String newTitle = currentBoard.title;
    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        backgroundColor: AppColors.cardBackground,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(16.r),
        ),
        title: Text(
          'Edit Title',
          style: TextStyle(
            color: AppColors.textLight,
            fontSize: 18.sp,
            fontWeight: FontWeight.bold,
          ),
        ),
        content: TextField(
          onChanged: (value) => newTitle = value,
          controller: TextEditingController(text: currentBoard.title),
          style: TextStyle(color: AppColors.textLight),
          decoration: InputDecoration(
            hintText: 'Board title...',
            hintStyle: TextStyle(color: AppColors.textMedium),
            border: OutlineInputBorder(
              borderRadius: BorderRadius.circular(8.r),
              borderSide: BorderSide(color: AppColors.textMedium.withOpacity(0.3)),
            ),
            focusedBorder: OutlineInputBorder(
              borderRadius: BorderRadius.circular(8.r),
              borderSide: BorderSide(color: AppColors.secondaryTeal),
            ),
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
            onPressed: () {
              Navigator.pop(context);
              setState(() {
                currentBoard = currentBoard.copyWith(title: newTitle);
                isEditing = true;
              });
              _saveToUndoStack();
            },
            child: Text(
              'Save',
              style: TextStyle(color: AppColors.secondaryTeal),
            ),
          ),
        ],
      ),
    );
  }

  void _saveBoard() {
    _showSaveDialog();
  }

  void _showTemplates() {
    showModalBottomSheet(
      context: context,
      backgroundColor: Colors.transparent,
      isScrollControlled: true,
      builder: (context) => _buildTemplateSelector(),
    );
  }

  Widget _buildImageSourceSelector() {
    return Container(
      margin: EdgeInsets.all(20.w),
      decoration: BoxDecoration(
        color: AppColors.cardBackground,
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
              color: AppColors.textMedium.withOpacity(0.3),
              borderRadius: BorderRadius.circular(2.r),
            ),
          ),
          Padding(
            padding: EdgeInsets.all(24.w),
            child: Column(
              children: [
                Text(
                  'Add Image',
                  style: TextStyle(
                    fontSize: 20.sp,
                    fontWeight: FontWeight.bold,
                    color: AppColors.textLight,
                  ),
                ),
                SizedBox(height: 24.h),
                Row(
                  children: [
                    Expanded(
                      child: _buildImageSourceOption(
                        icon: Icons.camera_alt,
                        title: 'Camera',
                        subtitle: 'Take a photo',
                        onTap: () {
                          Navigator.pop(context);
                          _addImageFromCamera();
                        },
                      ),
                    ),
                    SizedBox(width: 16.w),
                    Expanded(
                      child: _buildImageSourceOption(
                        icon: Icons.photo_library,
                        title: 'Gallery',
                        subtitle: 'Choose from photos',
                        onTap: () {
                          Navigator.pop(context);
                          _addImageFromGallery();
                        },
                      ),
                    ),
                  ],
                ),
                SizedBox(height: 16.h),
                Row(
                  children: [
                    Expanded(
                      child: _buildImageSourceOption(
                        icon: Icons.search,
                        title: 'Search',
                        subtitle: 'Find online images',
                        onTap: () {
                          Navigator.pop(context);
                          _addImageFromSearch();
                        },
                      ),
                    ),
                    SizedBox(width: 16.w),
                    Expanded(
                      child: _buildImageSourceOption(
                        icon: Icons.auto_awesome,
                        title: 'AI Generate',
                        subtitle: 'Create with AI',
                        onTap: () {
                          Navigator.pop(context);
                          _addImageFromAI();
                        },
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

  Widget _buildImageSourceOption({
    required IconData icon,
    required String title,
    required String subtitle,
    required VoidCallback onTap,
  }) {
    return GestureDetector(
      onTap: onTap,
      child: Container(
        padding: EdgeInsets.all(16.w),
        decoration: BoxDecoration(
          color: AppColors.backgroundMedium,
          borderRadius: BorderRadius.circular(12.r),
          border: Border.all(
            color: AppColors.textMedium.withOpacity(0.2),
          ),
        ),
        child: Column(
          children: [
            Icon(
              icon,
              color: AppColors.secondaryTeal,
              size: 24.sp,
            ),
            SizedBox(height: 8.h),
            Text(
              title,
              style: TextStyle(
                fontSize: 14.sp,
                fontWeight: FontWeight.w600,
                color: AppColors.textLight,
              ),
            ),
            SizedBox(height: 2.h),
            Text(
              subtitle,
              style: TextStyle(
                fontSize: 11.sp,
                color: AppColors.textMedium,
              ),
              textAlign: TextAlign.center,
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildTemplateSelector() {
    final templates = VisionBoardTemplate.getDefaultTemplates();
    
    return DraggableScrollableSheet(
      initialChildSize: 0.7,
      maxChildSize: 0.9,
      minChildSize: 0.5,
      builder: (context, scrollController) {
        return Container(
          decoration: BoxDecoration(
            color: AppColors.cardBackground,
            borderRadius: BorderRadius.vertical(top: Radius.circular(20.r)),
          ),
          child: Column(
            children: [
              Container(
                width: 40.w,
                height: 4.h,
                margin: EdgeInsets.only(top: 12.h),
                decoration: BoxDecoration(
                  color: AppColors.textMedium.withOpacity(0.3),
                  borderRadius: BorderRadius.circular(2.r),
                ),
              ),
              Padding(
                padding: EdgeInsets.all(24.w),
                child: Text(
                  'Choose Template',
                  style: TextStyle(
                    fontSize: 20.sp,
                    fontWeight: FontWeight.bold,
                    color: AppColors.textLight,
                  ),
                ),
              ),
              Expanded(
                child: GridView.builder(
                  controller: scrollController,
                  padding: EdgeInsets.symmetric(horizontal: 24.w),
                  gridDelegate: SliverGridDelegateWithFixedCrossAxisCount(
                    crossAxisCount: 2,
                    crossAxisSpacing: 16.w,
                    mainAxisSpacing: 16.h,
                    childAspectRatio: 0.8,
                  ),
                  itemCount: templates.length + 1, // +1 for blank template
                  itemBuilder: (context, index) {
                    if (index == 0) {
                      // Blank template
                      return GestureDetector(
                        onTap: () {
                          Navigator.pop(context);
                          setState(() {
                            currentBoard = VisionBoard.empty();
                          });
                          _saveToUndoStack();
                        },
                        child: Container(
                          decoration: BoxDecoration(
                            color: AppColors.backgroundMedium,
                            borderRadius: BorderRadius.circular(12.r),
                            border: Border.all(
                              color: AppColors.textMedium.withOpacity(0.2),
                            ),
                          ),
                          child: Column(
                            children: [
                              Expanded(
                                child: Container(
                                  margin: EdgeInsets.all(12.w),
                                  decoration: BoxDecoration(
                                    color: AppColors.white,
                                    borderRadius: BorderRadius.circular(8.r),
                                    border: Border.all(
                                      color: AppColors.textMedium.withOpacity(0.3),
                                      style: BorderStyle.solid,
                                    ),
                                  ),
                                  child: Center(
                                    child: Icon(
                                      Icons.add,
                                      color: AppColors.textMedium,
                                      size: 32.sp,
                                    ),
                                  ),
                                ),
                              ),
                              Padding(
                                padding: EdgeInsets.all(12.w),
                                child: Text(
                                  'Blank Canvas',
                                  style: TextStyle(
                                    fontSize: 14.sp,
                                    fontWeight: FontWeight.w600,
                                    color: AppColors.textLight,
                                  ),
                                  textAlign: TextAlign.center,
                                ),
                              ),
                            ],
                          ),
                        ),
                      );
                    }
                    
                    final template = templates[index - 1];
                    return GestureDetector(
                      onTap: () {
                        Navigator.pop(context);
                        _applyTemplate(template);
                      },
                      child: Container(
                        decoration: BoxDecoration(
                          color: AppColors.backgroundMedium,
                          borderRadius: BorderRadius.circular(12.r),
                          border: Border.all(
                            color: AppColors.textMedium.withOpacity(0.2),
                          ),
                        ),
                        child: Column(
                          children: [
                            Expanded(
                              child: Container(
                                margin: EdgeInsets.all(12.w),
                                decoration: BoxDecoration(
                                  color: template.backgroundColor,
                                  borderRadius: BorderRadius.circular(8.r),
                                ),
                                child: Center(
                                  child: Icon(
                                    template.icon,
                                    color: template.color,
                                    size: 32.sp,
                                  ),
                                ),
                              ),
                            ),
                            Padding(
                              padding: EdgeInsets.all(12.w),
                              child: Column(
                                children: [
                                  Text(
                                    template.name,
                                    style: TextStyle(
                                      fontSize: 14.sp,
                                      fontWeight: FontWeight.w600,
                                      color: AppColors.textLight,
                                    ),
                                    textAlign: TextAlign.center,
                                  ),
                                  SizedBox(height: 2.h),
                                  Text(
                                    template.description,
                                    style: TextStyle(
                                      fontSize: 10.sp,
                                      color: AppColors.textMedium,
                                    ),
                                    textAlign: TextAlign.center,
                                    maxLines: 2,
                                    overflow: TextOverflow.ellipsis,
                                  ),
                                ],
                              ),
                            ),
                          ],
                        ),
                      ),
                    );
                  },
                ),
              ),
            ],
          ),
        );
      },
    );
  }

  void _showTextEditor({VisionBoardTextElement? existingElement}) {
    String text = existingElement?.text ?? '';
    Color textColor = existingElement?.style.color ?? AppColors.textMedium;
    double fontSize = existingElement?.style.fontSize ?? 16.sp;
    FontWeight fontWeight = existingElement?.style.fontWeight ?? FontWeight.normal;
    Color backgroundColor = existingElement?.style.backgroundColor ?? Colors.transparent;
    
    showDialog(
      context: context,
      builder: (context) => StatefulBuilder(
        builder: (context, setDialogState) => AlertDialog(
          backgroundColor: AppColors.cardBackground,
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(16.r),
          ),
          title: Text(
            existingElement != null ? 'Edit Text' : 'Add Text',
            style: TextStyle(
              color: AppColors.textLight,
              fontSize: 18.sp,
              fontWeight: FontWeight.bold,
            ),
          ),
          content: SingleChildScrollView(
            child: Column(
              mainAxisSize: MainAxisSize.min,
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                // Text input
                TextField(
                  onChanged: (value) => text = value,
                  controller: TextEditingController(text: text),
                  style: TextStyle(color: AppColors.textLight),
                  decoration: InputDecoration(
                    hintText: 'Enter your manifestation...',
                    hintStyle: TextStyle(color: AppColors.textMedium),
                    border: OutlineInputBorder(
                      borderRadius: BorderRadius.circular(8.r),
                      borderSide: BorderSide(color: AppColors.textMedium.withOpacity(0.3)),
                    ),
                    focusedBorder: OutlineInputBorder(
                      borderRadius: BorderRadius.circular(8.r),
                      borderSide: BorderSide(color: AppColors.secondaryTeal),
                    ),
                  ),
                  maxLines: 3,
                ),
                
                SizedBox(height: 20.h),
                
                // Text formatting options
                Text(
                  'Formatting',
                  style: TextStyle(
                    color: AppColors.textLight,
                    fontSize: 14.sp,
                    fontWeight: FontWeight.w600,
                  ),
                ),
                SizedBox(height: 12.h),
                
                // Font size
                Row(
                  children: [
                    Text(
                      'Size: ',
                      style: TextStyle(color: AppColors.textMedium, fontSize: 12.sp),
                    ),
                    Expanded(
                      child: Slider(
                        value: fontSize,
                        min: 12.0,
                        max: 48.0,
                        divisions: 36,
                        activeColor: AppColors.secondaryTeal,
                        onChanged: (value) {
                          setDialogState(() {
                            fontSize = value;
                          });
                        },
                      ),
                    ),
                    Text(
                      '${fontSize.round()}',
                      style: TextStyle(color: AppColors.textMedium, fontSize: 12.sp),
                    ),
                  ],
                ),
                
                // Font weight
                Row(
                  children: [
                    Text(
                      'Weight: ',
                      style: TextStyle(color: AppColors.textMedium, fontSize: 12.sp),
                    ),
                    ToggleButtons(
                      isSelected: [
                        fontWeight == FontWeight.normal,
                        fontWeight == FontWeight.bold,
                      ],
                      onPressed: (index) {
                        setDialogState(() {
                          fontWeight = index == 0 ? FontWeight.normal : FontWeight.bold;
                        });
                      },
                      children: [
                        Padding(
                          padding: EdgeInsets.symmetric(horizontal: 12.w),
                          child: Text('Normal', style: TextStyle(fontSize: 10.sp)),
                        ),
                        Padding(
                          padding: EdgeInsets.symmetric(horizontal: 12.w),
                          child: Text('Bold', style: TextStyle(fontSize: 10.sp)),
                        ),
                      ],
                    ),
                  ],
                ),
                
                SizedBox(height: 12.h),
                
                // Color selection
                Text(
                  'Text Color',
                  style: TextStyle(color: AppColors.textMedium, fontSize: 12.sp),
                ),
                SizedBox(height: 8.h),
                Wrap(
                  spacing: 8.w,
                  children: [
                    AppColors.textMedium,
                    AppColors.white,
                    AppColors.secondaryTeal,
                    AppColors.accentGold,
                    AppColors.gradientPurple,
                    Colors.red,
                    Colors.green,
                    Colors.blue,
                  ].map((color) => GestureDetector(
                    onTap: () {
                      setDialogState(() {
                        textColor = color;
                      });
                    },
                    child: Container(
                      width: 32.w,
                      height: 32.h,
                      decoration: BoxDecoration(
                        color: color,
                        shape: BoxShape.circle,
                        border: Border.all(
                          color: textColor == color ? AppColors.secondaryTeal : Colors.transparent,
                          width: 3,
                        ),
                      ),
                    ),
                  )).toList(),
                ),
                
                SizedBox(height: 12.h),
                
                // Background color
                Text(
                  'Background Color',
                  style: TextStyle(color: AppColors.textMedium, fontSize: 12.sp),
                ),
                SizedBox(height: 8.h),
                Wrap(
                  spacing: 8.w,
                  children: [
                    Colors.transparent,
                    AppColors.white,
                    AppColors.cardBackground,
                    AppColors.secondaryTeal.withOpacity(0.1),
                    AppColors.accentGold.withOpacity(0.1),
                    AppColors.gradientPurple.withOpacity(0.1),
                  ].map((color) => GestureDetector(
                    onTap: () {
                      setDialogState(() {
                        backgroundColor = color;
                      });
                    },
                    child: Container(
                      width: 32.w,
                      height: 32.h,
                      decoration: BoxDecoration(
                        color: color == Colors.transparent ? AppColors.backgroundMedium : color,
                        shape: BoxShape.circle,
                        border: Border.all(
                          color: backgroundColor == color ? AppColors.secondaryTeal : AppColors.textMedium.withOpacity(0.3),
                          width: backgroundColor == color ? 3 : 1,
                        ),
                      ),
                      child: color == Colors.transparent ? Icon(
                        Icons.block,
                        color: AppColors.textMedium,
                        size: 16.sp,
                      ) : null,
                    ),
                  )).toList(),
                ),
              ],
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
              onPressed: () {
                Navigator.pop(context);
                if (text.isNotEmpty) {
                  if (existingElement != null) {
                    _updateTextElement(existingElement, text, textColor, fontSize, fontWeight, backgroundColor);
                  } else {
                    _addTextToBoard(text, textColor, fontSize, fontWeight, backgroundColor);
                  }
                }
              },
              child: Text(
                existingElement != null ? 'Update' : 'Add',
                style: TextStyle(color: AppColors.secondaryTeal),
              ),
            ),
          ],
        ),
      ),
    );
  }

  void _showSaveDialog() {
    String title = currentBoard.title;
    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        backgroundColor: AppColors.cardBackground,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(16.r),
        ),
        title: Text(
          'Save Vision Board',
          style: TextStyle(
            color: AppColors.textLight,
            fontSize: 18.sp,
            fontWeight: FontWeight.bold,
          ),
        ),
        content: TextField(
          onChanged: (value) => title = value,
          controller: TextEditingController(text: currentBoard.title),
          style: TextStyle(color: AppColors.textLight),
          decoration: InputDecoration(
            hintText: 'Board title...',
            hintStyle: TextStyle(color: AppColors.textMedium),
            border: OutlineInputBorder(
              borderRadius: BorderRadius.circular(8.r),
              borderSide: BorderSide(color: AppColors.textMedium.withOpacity(0.3)),
            ),
            focusedBorder: OutlineInputBorder(
              borderRadius: BorderRadius.circular(8.r),
              borderSide: BorderSide(color: AppColors.secondaryTeal),
            ),
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
            onPressed: () {
              Navigator.pop(context);
              _performSave(title);
            },
            child: Text(
              'Save',
              style: TextStyle(color: AppColors.secondaryTeal),
            ),
          ),
        ],
      ),
    );
  }

  void _applyTemplate(VisionBoardTemplate template) {
    setState(() {
      currentBoard = VisionBoard.fromTemplate(template);
      isEditing = true;
    });
    _saveToUndoStack();
  }

  void _addTextToBoard(String text, Color textColor, double fontSize, FontWeight fontWeight, Color backgroundColor) {
    final textElement = VisionBoardTextElement(
      id: DateTime.now().millisecondsSinceEpoch.toString(),
      position: Offset(100.w, 100.h), // Default position
      size: Size(200.w, 100.h), // Default size
      text: text,
      style: TextStyle(
        color: textColor,
        fontSize: fontSize,
        fontWeight: fontWeight,
        backgroundColor: backgroundColor == Colors.transparent ? null : backgroundColor,
      ),
      alignment: TextAlign.center,
    );

    final updatedElements = [...currentBoard.elements, textElement];
    final updatedBoard = currentBoard.copyWith(elements: updatedElements);
    
    setState(() {
      currentBoard = updatedBoard;
      isEditing = true;
    });
    _saveToUndoStack();
  }

  void _updateTextElement(VisionBoardTextElement element, String text, Color textColor, double fontSize, FontWeight fontWeight, Color backgroundColor) {
    final updatedElement = element.copyWith(
      text: text,
      style: TextStyle(
        color: textColor,
        fontSize: fontSize,
        fontWeight: fontWeight,
        backgroundColor: backgroundColor == Colors.transparent ? null : backgroundColor,
      ),
    );

    final updatedElements = currentBoard.elements.map((e) {
      return e.id == element.id ? updatedElement : e;
    }).toList();

    final updatedBoard = currentBoard.copyWith(elements: updatedElements);
    
    setState(() {
      currentBoard = updatedBoard;
      isEditing = true;
    });
    _saveToUndoStack();
  }

  void _addImageFromCamera() {
    // TODO: Implement camera functionality
    _addPlaceholderImage('Camera Image');
  }

  void _addImageFromGallery() {
    // TODO: Implement gallery picker
    _addPlaceholderImage('Gallery Image');
  }

  void _addImageFromSearch() {
    // TODO: Implement image search
    _addPlaceholderImage('Search Image');
  }

  void _addImageFromAI() {
    // TODO: Implement AI image generation
    _addPlaceholderImage('AI Generated');
  }

  void _addPlaceholderImage(String source) {
    final imageElement = VisionBoardImageElement(
      id: DateTime.now().millisecondsSinceEpoch.toString(),
      position: Offset(50.w, 50.h),
      size: Size(150.w, 150.h),
      imagePath: '', // Empty path for placeholder
      fit: BoxFit.cover,
    );

    final updatedElements = [...currentBoard.elements, imageElement];
    final updatedBoard = currentBoard.copyWith(elements: updatedElements);
    
    setState(() {
      currentBoard = updatedBoard;
      isEditing = true;
    });
    _saveToUndoStack();
    
    // Show snackbar
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text('$source placeholder added. Tap to replace with actual image.'),
        backgroundColor: AppColors.secondaryTeal,
        duration: Duration(seconds: 2),
      ),
    );
  }

  void _performSave(String title) {
    setState(() {
      currentBoard = currentBoard.copyWith(title: title);
      isEditing = false;
    });
    
    // TODO: Save to local storage or cloud
    print('Saving board: $title');
    print('Board JSON: ${currentBoard.toJson()}');
    
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text('Vision board saved successfully!'),
        backgroundColor: AppColors.accentMint,
        duration: Duration(seconds: 2),
      ),
    );
  }
}