import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:fun/models/vision_board.dart';
import '../constants/app_theme.dart';
import 'package:fun/widgets/stylish_loading.dart';
import 'dart:io';

class VisionBoardCanvas extends StatefulWidget {
  final VisionBoard board;
  final VisionBoardTool selectedTool;
  final Function(VisionBoard) onBoardChanged;

  const VisionBoardCanvas({
    Key? key,
    required this.board,
    required this.selectedTool,
    required this.onBoardChanged,
  }) : super(key: key);

  @override
  _VisionBoardCanvasState createState() => _VisionBoardCanvasState();
}

class _VisionBoardCanvasState extends State<VisionBoardCanvas> {
  bool isDragging = false;
  String? draggingElementId; // Track which element is being dragged
  String? selectedElementId; // Track which element is selected
  bool isResizing = false;
  String? resizeHandle; // Which resize handle is being dragged

  // Canvas zoom and pan
  double canvasScale = 1.0;
  Offset canvasOffset = Offset.zero;
  final TransformationController transformController = TransformationController();

  @override
  Widget build(BuildContext context) {
    return Container(
      width: double.infinity,
      height: double.infinity,
      color: widget.board.backgroundColor,
      child: GestureDetector(
        behavior: HitTestBehavior.translucent,
        onTap: () {
          // Clear selection when tapping empty space
          if (selectedElementId != null) {
            setState(() {
              selectedElementId = null;
            });
          }
        },
        child: Stack(
          children: [
            // Background grid
            if (widget.selectedTool != VisionBoardTool.select)
              _buildGrid(),
            
            // Board elements
            ...widget.board.elements.asMap().entries.map((entry) {
              final index = entry.key;
              final element = entry.value;
              return _buildElement(element, index);
            }),
            
            // Selection overlay with resize handles
            if (selectedElementId != null && widget.selectedTool == VisionBoardTool.resize)
              _buildSelectionOverlay(),
            
            // Empty state
            if (widget.board.elements.isEmpty)
              _buildEmptyState(),
          ],
        ),
      ),
    );
  }

  Widget _buildGrid() {
    return Positioned.fill(
      child: CustomPaint(
        painter: GridPainter(),
      ),
    );
  }

  Widget _buildElement(VisionBoardElement element, int index) {
    final isBeingDragged = draggingElementId == element.id;
    
    return Positioned(
      left: element.position.dx,
      top: element.position.dy,
      child: GestureDetector(
        onTap: () => _onElementTap(element),
        onPanStart: (details) => _onElementPanStart(element, details),
        onPanUpdate: (details) => _onElementPanUpdate(element, details),
        onPanEnd: (details) => _onElementPanEnd(element, details),
        behavior: HitTestBehavior.opaque,
        child: AnimatedContainer(
          duration: Duration(milliseconds: isBeingDragged ? 0 : 200),
          curve: Curves.easeOut,
          child: Transform.rotate(
            angle: element.rotation,
            child: Container(
              width: element.size.width,
              height: element.size.height,
              padding: EdgeInsets.all(2.w), // Add breathing room
              decoration: BoxDecoration(
                borderRadius: BorderRadius.circular(8.r), // Rounder corners
                border: _shouldShowElementBorder(element) 
                    ? Border.all(
                        color: _getElementBorderColor(element),
                        width: _getBorderWidth(element),
                      ) 
                    : null,
                boxShadow: _getElementShadow(element, isBeingDragged),
              ),
              child: Stack(
                children: [
                  Container(
                    margin: EdgeInsets.all(2.w), // Inner margin for content
                    child: _buildElementContent(element),
                  ),
                  // Show tool cursor overlay
                  if (_shouldShowToolCursor(element))
                    _buildToolCursorOverlay(element),
                ],
              ),
            ),
          ),
        ),
      ),
    );
  }

  Widget _buildElementContent(VisionBoardElement element) {
    if (element is VisionBoardImageElement) {
      return Container(
        decoration: BoxDecoration(
          color: AppTheme.white,
          borderRadius: BorderRadius.circular(12.r),
          border: Border.all(
            color: AppTheme.textMedium.withOpacity(0.1),
            width: 1.w,
          ),
          boxShadow: [
            BoxShadow(
              color: AppTheme.black.withOpacity(0.08),
              blurRadius: 8.r,
              offset: Offset(0, 4.h),
            ),
          ],
        ),
        child: ClipRRect(
          borderRadius: BorderRadius.circular(12.r),
          child: _buildImageWidget(element),
        ),
      );
    } else if (element is VisionBoardTextElement) {
      return Text(
        element.text,
        style: element.style.copyWith(
          fontSize: (element.style.fontSize ?? 16.sp) * 
              (element.size.width / 200).clamp(0.6, 1.8),
          height: 1.3, // Better line height
          shadows: null, // No shadows - completely clean text
        ),
        textAlign: element.alignment,
        overflow: TextOverflow.visible,
        maxLines: null,
      );
    }
    
    return Container();
  }

  Widget _buildImageWidget(VisionBoardImageElement element) {
    print('🖼️ Building image widget for element ${element.id}');
    print('🖼️ ImagePath: "${element.imagePath}"');
    print('🖼️ ImageUrl: "${element.imageUrl ?? 'null'}"');
    
    // Try to load from URL first, then local path, then placeholder
    if (element.imageUrl != null && element.imageUrl!.isNotEmpty) {
      return Image.network(
        element.imageUrl!,
        fit: element.fit,
        width: double.infinity,
        height: double.infinity,
        loadingBuilder: (context, child, loadingProgress) {
          if (loadingProgress == null) return child;
          return Center(
            child: CompactStylishLoading(
              color: AppTheme.visionBoardGradient[0],
              size: 30.w,
            ),
          );
        },
        errorBuilder: (context, error, stackTrace) {
          print('🌐 Network image failed, trying local: $error');
          // Fallback to local image
          if (element.imagePath.isNotEmpty) {
            final imageFile = File(element.imagePath);
            print('📁 Checking local file: ${element.imagePath}');
            print('📁 File exists: ${imageFile.existsSync()}');
            return Image.file(
              imageFile,
              fit: element.fit,
              width: double.infinity,
              height: double.infinity,
              errorBuilder: (context, error, stackTrace) {
                print('❌ Local image failed too: $error');
                return _buildImagePlaceholder();
              },
            );
          }
          return _buildImagePlaceholder();
        },
      );
    } else if (element.imagePath.isNotEmpty) {
      final imageFile = File(element.imagePath);
      print('📁 Loading image from path: ${element.imagePath}');
      print('📁 File exists: ${imageFile.existsSync()}');
      return Image.file(
        imageFile,
        fit: element.fit,
        width: double.infinity,
        height: double.infinity,
        errorBuilder: (context, error, stackTrace) {
          print('❌ Local image load failed: $error');
          return _buildImagePlaceholder();
        },
      );
    } else {
      return _buildImagePlaceholder();
    }
  }

  Widget _buildImagePlaceholder() {
    return Container(
      decoration: BoxDecoration(
        color: AppTheme.backgroundMedium,
        borderRadius: BorderRadius.circular(8.r),
        border: Border.all(
          color: AppTheme.textMedium.withOpacity(0.2),
          width: 2,
          style: BorderStyle.solid,
        ),
      ),
      child: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Icon(
              Icons.image_outlined,
              color: AppTheme.textMedium,
              size: 32.sp,
            ),
            SizedBox(height: 8.h),
            Text(
              'Tap to add image',
              style: TextStyle(
                color: AppTheme.textMedium,
                fontSize: 12.sp,
                fontWeight: FontWeight.w500,
              ),
            ),
          ],
        ),
      ),
    );
  }

  // Removed unused UI building methods - drag-only interaction

  Widget _buildEmptyState() {
    return Center(
      child: Container(
        padding: EdgeInsets.all(32.w),
        decoration: BoxDecoration(
          color: AppTheme.white.withOpacity(0.9),
          borderRadius: BorderRadius.circular(20.r),
          border: Border.all(
            color: AppTheme.textMedium.withOpacity(0.2),
            width: 2,
            style: BorderStyle.solid,
          ),
        ),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            Icon(
              Icons.dashboard_outlined,
              color: AppTheme.textMedium.withOpacity(0.5),
              size: 64.sp,
            ),
            SizedBox(height: 16.h),
            Text(
              'Your vision board is empty',
              style: TextStyle(
                fontSize: 20.sp,
                fontWeight: FontWeight.w600,
                color: AppTheme.textMedium.withOpacity(0.8),
              ),
              textAlign: TextAlign.center,
            ),
            SizedBox(height: 8.h),
            Text(
              'Start creating by adding images and text\nUse the toolbar below to get started',
              style: TextStyle(
                fontSize: 14.sp,
                color: AppTheme.textMedium.withOpacity(0.6),
                height: 1.4,
              ),
              textAlign: TextAlign.center,
            ),
          ],
        ),
      ),
    );
  }

  // Element interaction handlers
  void _onElementTap(VisionBoardElement element) {
    switch (widget.selectedTool) {
      case VisionBoardTool.select:
      case VisionBoardTool.move:
      case VisionBoardTool.resize:
        // Select the element for move/resize operations
        setState(() {
          selectedElementId = element.id;
        });
        break;
      case VisionBoardTool.delete:
        // Delete the element immediately
        _deleteElement(element);
        break;
      case VisionBoardTool.rotate:
        // Select for rotation (could add rotation gesture here)
        setState(() {
          selectedElementId = element.id;
        });
        break;
    }
  }

  void _deleteElement(VisionBoardElement element) {
    final updatedElements = widget.board.elements.where((e) => e.id != element.id).toList();
    final updatedBoard = widget.board.copyWith(elements: updatedElements);
    
    // Clear selection if we deleted the selected element
    if (selectedElementId == element.id) {
      selectedElementId = null;
    }
    
    widget.onBoardChanged(updatedBoard);
  }

  // Visual feedback methods
  bool _shouldShowElementBorder(VisionBoardElement element) {
    return selectedElementId == element.id || 
           draggingElementId == element.id ||
           widget.selectedTool == VisionBoardTool.delete;
  }

  Color _getElementBorderColor(VisionBoardElement element) {
    if (widget.selectedTool == VisionBoardTool.delete) {
      return Colors.red.withOpacity(0.8);
    } else if (selectedElementId == element.id) {
      return AppTheme.visionBoardGradient[0];
    } else if (draggingElementId == element.id) {
      return AppTheme.visionBoardGradient[1];
    }
    return AppTheme.textMedium.withOpacity(0.3);
  }

  double _getBorderWidth(VisionBoardElement element) {
    if (widget.selectedTool == VisionBoardTool.delete) {
      return 3.w;
    } else if (selectedElementId == element.id) {
      return 2.5.w;
    } else if (draggingElementId == element.id) {
      return 2.w;
    }
    return 1.w;
  }

  List<BoxShadow> _getElementShadow(VisionBoardElement element, bool isBeingDragged) {
    // No shadows for text elements - keep them completely clean
    if (element is VisionBoardTextElement) {
      return [];
    }
    
    // Only apply shadows to image elements
    if (isBeingDragged) {
      return [
        BoxShadow(
          color: AppTheme.visionBoardGradient[0].withOpacity(0.4),
          blurRadius: 20.r,
          offset: Offset(0, 10.h),
        ),
        BoxShadow(
          color: AppTheme.black.withOpacity(0.1),
          blurRadius: 8.r,
          offset: Offset(0, 4.h),
        ),
      ];
    } else if (selectedElementId == element.id) {
      return [
        BoxShadow(
          color: AppTheme.visionBoardGradient[0].withOpacity(0.2),
          blurRadius: 12.r,
          offset: Offset(0, 6.h),
        ),
      ];
    } else if (widget.selectedTool == VisionBoardTool.delete) {
      return [
        BoxShadow(
          color: Colors.red.withOpacity(0.2),
          blurRadius: 8.r,
          offset: Offset(0, 4.h),
        ),
      ];
    }
    return [
      BoxShadow(
        color: AppTheme.black.withOpacity(0.05),
        blurRadius: 4.r,
        offset: Offset(0, 2.h),
      ),
    ];
  }

  bool _shouldShowToolCursor(VisionBoardElement element) {
    return widget.selectedTool == VisionBoardTool.delete && 
           selectedElementId != element.id; // Don't show on selected element
  }

  Widget _buildToolCursorOverlay(VisionBoardElement element) {
    if (widget.selectedTool == VisionBoardTool.delete) {
      return Positioned.fill(
        child: Container(
          decoration: BoxDecoration(
            color: Colors.red.withOpacity(0.1),
            borderRadius: BorderRadius.circular(4.r),
          ),
          child: Center(
            child: Container(
              width: 40.w,
              height: 40.w,
              decoration: BoxDecoration(
                color: Colors.red.withOpacity(0.9),
                shape: BoxShape.circle,
              ),
              child: Icon(
                Icons.delete_rounded,
                color: Colors.white,
                size: 20.sp,
              ),
            ),
          ),
        ),
      );
    }
    return SizedBox.shrink();
  }

  // Selection overlay with resize handles
  Widget _buildSelectionOverlay() {
    if (selectedElementId == null || widget.board.elements.isEmpty) {
      return SizedBox.shrink();
    }
    
    VisionBoardElement? selectedElement;
    try {
      selectedElement = widget.board.elements.firstWhere(
        (e) => e.id == selectedElementId,
      );
    } catch (e) {
      return SizedBox.shrink();
    }

    return Positioned(
      left: selectedElement.position.dx - 12.w,
      top: selectedElement.position.dy - 12.w,
      child: AnimatedContainer(
        duration: Duration(milliseconds: 200),
        curve: Curves.easeOut,
        width: selectedElement.size.width + 24.w,
        height: selectedElement.size.height + 24.w,
        decoration: BoxDecoration(
          border: Border.all(
            color: AppTheme.visionBoardGradient[0],
            width: 2.w,
          ),
          borderRadius: BorderRadius.circular(12.r),
          boxShadow: [
            BoxShadow(
              color: AppTheme.visionBoardGradient[0].withOpacity(0.1),
              blurRadius: 8.r,
              offset: Offset(0, 4.h),
            ),
          ],
        ),
        child: Stack(
          children: [
            // Corner resize handles
            _buildResizeHandle('top-left', -10.w, -10.w),
            _buildResizeHandle('top-right', selectedElement.size.width + 4.w, -10.w),
            _buildResizeHandle('bottom-left', -10.w, selectedElement.size.height + 4.w),
            _buildResizeHandle('bottom-right', selectedElement.size.width + 4.w, selectedElement.size.height + 4.w),
            
            // Edge handles
            _buildResizeHandle('top-center', selectedElement.size.width / 2 - 3.w, -10.w),
            _buildResizeHandle('bottom-center', selectedElement.size.width / 2 - 3.w, selectedElement.size.height + 4.w),
            _buildResizeHandle('left-center', -10.w, selectedElement.size.height / 2 - 3.w),
            _buildResizeHandle('right-center', selectedElement.size.width + 4.w, selectedElement.size.height / 2 - 3.w),
          ],
        ),
      ),
    );
  }

  Widget _buildResizeHandle(String handleId, double left, double top) {
    return Positioned(
      left: left,
      top: top,
      child: GestureDetector(
        onPanStart: (details) => _onResizeStart(handleId, details),
        onPanUpdate: (details) => _onResizeUpdate(handleId, details),
        onPanEnd: (details) => _onResizeEnd(handleId, details),
        child: AnimatedContainer(
          duration: Duration(milliseconds: 150),
          width: 20.w,
          height: 20.w,
          decoration: BoxDecoration(
            gradient: LinearGradient(
              colors: [
                AppTheme.white,
                AppTheme.white.withOpacity(0.95),
              ],
              begin: Alignment.topLeft,
              end: Alignment.bottomRight,
            ),
            border: Border.all(
              color: AppTheme.visionBoardGradient[0],
              width: 2.5.w,
            ),
            borderRadius: BorderRadius.circular(10.w),
            boxShadow: [
              BoxShadow(
                color: AppTheme.visionBoardGradient[0].withOpacity(0.3),
                blurRadius: 8.r,
                offset: Offset(0, 4.h),
              ),
              BoxShadow(
                color: AppTheme.black.withOpacity(0.1),
                blurRadius: 4.r,
                offset: Offset(0, 2.h),
              ),
            ],
          ),
          child: _getResizeHandleCursor(handleId),
        ),
      ),
    );
  }

  Widget _getResizeHandleCursor(String handleId) {
    IconData icon;
    switch (handleId) {
      case 'top-left':
      case 'bottom-right':
        icon = Icons.north_west;
        break;
      case 'top-right':
      case 'bottom-left':
        icon = Icons.north_east;
        break;
      case 'top-center':
      case 'bottom-center':
        icon = Icons.north;
        break;
      case 'left-center':
      case 'right-center':
        icon = Icons.west;
        break;
      default:
        icon = Icons.open_with;
    }
    
    return Icon(
      icon,
      size: 10.sp,
      color: AppTheme.visionBoardGradient[0],
    );
  }

  // Resize handling
  void _onResizeStart(String handleId, DragStartDetails details) {
    setState(() {
      isResizing = true;
      resizeHandle = handleId;
    });
  }

  void _onResizeUpdate(String handleId, DragUpdateDetails details) {
    if (!isResizing || selectedElementId == null) return;

    final selectedElement = widget.board.elements.firstWhere((e) => e.id == selectedElementId);
    final delta = details.delta;
    
    Size newSize = selectedElement.size;
    Offset newPosition = selectedElement.position;

    // Calculate new size and position based on which handle is being dragged
    switch (handleId) {
      case 'top-left':
        newSize = Size(
          (selectedElement.size.width - delta.dx).clamp(50.w, double.infinity),
          (selectedElement.size.height - delta.dy).clamp(50.h, double.infinity),
        );
        newPosition = Offset(
          selectedElement.position.dx + (selectedElement.size.width - newSize.width),
          selectedElement.position.dy + (selectedElement.size.height - newSize.height),
        );
        break;
      case 'top-right':
        newSize = Size(
          (selectedElement.size.width + delta.dx).clamp(50.w, double.infinity),
          (selectedElement.size.height - delta.dy).clamp(50.h, double.infinity),
        );
        newPosition = Offset(
          selectedElement.position.dx,
          selectedElement.position.dy + (selectedElement.size.height - newSize.height),
        );
        break;
      case 'bottom-left':
        newSize = Size(
          (selectedElement.size.width - delta.dx).clamp(50.w, double.infinity),
          (selectedElement.size.height + delta.dy).clamp(50.h, double.infinity),
        );
        newPosition = Offset(
          selectedElement.position.dx + (selectedElement.size.width - newSize.width),
          selectedElement.position.dy,
        );
        break;
      case 'bottom-right':
        newSize = Size(
          (selectedElement.size.width + delta.dx).clamp(50.w, double.infinity),
          (selectedElement.size.height + delta.dy).clamp(50.h, double.infinity),
        );
        break;
      case 'top-center':
        newSize = Size(
          selectedElement.size.width,
          (selectedElement.size.height - delta.dy).clamp(50.h, double.infinity),
        );
        newPosition = Offset(
          selectedElement.position.dx,
          selectedElement.position.dy + (selectedElement.size.height - newSize.height),
        );
        break;
      case 'bottom-center':
        newSize = Size(
          selectedElement.size.width,
          (selectedElement.size.height + delta.dy).clamp(50.h, double.infinity),
        );
        break;
      case 'left-center':
        newSize = Size(
          (selectedElement.size.width - delta.dx).clamp(50.w, double.infinity),
          selectedElement.size.height,
        );
        newPosition = Offset(
          selectedElement.position.dx + (selectedElement.size.width - newSize.width),
          selectedElement.position.dy,
        );
        break;
      case 'right-center':
        newSize = Size(
          (selectedElement.size.width + delta.dx).clamp(50.w, double.infinity),
          selectedElement.size.height,
        );
        break;
    }

    _updateElement(selectedElement, size: newSize, position: newPosition);
  }

  void _onResizeEnd(String handleId, DragEndDetails details) {
    setState(() {
      isResizing = false;
      resizeHandle = null;
    });
  }

  void _onElementPanStart(VisionBoardElement element, DragStartDetails details) {
    // Only allow dragging with move tool or when element is selected
    if (widget.selectedTool == VisionBoardTool.move || 
        (widget.selectedTool == VisionBoardTool.select && selectedElementId == element.id)) {
      setState(() {
        isDragging = true;
        draggingElementId = element.id;
        selectedElementId = element.id; // Select the element being dragged
      });
    }
  }

  void _onElementPanUpdate(VisionBoardElement element, DragUpdateDetails details) {
    // Only update position if we're in drag mode
    if (isDragging && draggingElementId == element.id) {
      final delta = details.delta;
      final newPosition = element.position + delta;
      _updateElementPosition(element, newPosition);
    }
  }

  void _onElementPanEnd(VisionBoardElement element, DragEndDetails details) {
    if (mounted) {
      setState(() {
        isDragging = false;
        draggingElementId = null;
      });
    }
  }

  // Removed resize and rotation functionality - drag-only interaction


  // Removed selection state methods - drag-only interaction

  void _updateElementPosition(VisionBoardElement element, Offset newPosition) {
    // Get the current canvas container size from the rendering context
    final RenderBox? renderBox = context.findRenderObject() as RenderBox?;
    final canvasSize = renderBox?.size ?? MediaQuery.of(context).size;
    
    // Add basic boundary constraints within the actual canvas area
    final constrainedPosition = Offset(
      newPosition.dx.clamp(0.0, canvasSize.width - element.size.width),
      newPosition.dy.clamp(0.0, canvasSize.height - element.size.height),
    );
    _updateElement(element, position: constrainedPosition);
  }

  void _updateElement(VisionBoardElement element, {
    Offset? position,
    Size? size,
    double? rotation,
    int? zIndex,
  }) {
    final updatedElements = widget.board.elements.map((e) {
      if (e.id == element.id) {
        return e.copyWith(
          position: position,
          size: size,
          rotation: rotation,
          zIndex: zIndex,
        );
      }
      return e;
    }).toList();

    final updatedBoard = widget.board.copyWith(elements: updatedElements);
    widget.onBoardChanged(updatedBoard);
  }

  // Removed unused element manipulation methods - drag-only interaction
}

// Removed resize direction enum - not needed for drag-only interaction

class GridPainter extends CustomPainter {
  @override
  void paint(Canvas canvas, Size size) {
    final paint = Paint()
      ..color = AppTheme.textMedium.withOpacity(0.1)
      ..strokeWidth = 0.5;

    const gridSize = 20.0;

    // Draw vertical lines
    for (double x = 0; x <= size.width; x += gridSize) {
      canvas.drawLine(
        Offset(x, 0),
        Offset(x, size.height),
        paint,
      );
    }

    // Draw horizontal lines
    for (double y = 0; y <= size.height; y += gridSize) {
      canvas.drawLine(
        Offset(0, y),
        Offset(size.width, y),
        paint,
      );
    }
  }

  @override
  bool shouldRepaint(covariant CustomPainter oldDelegate) => false;
}