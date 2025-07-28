import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:fun/models/vision_board.dart';
import 'package:fun/constants/colors.dart';
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
        behavior: HitTestBehavior.deferToChild, // Let child elements handle gestures first
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
            
            // No selection overlay needed - drag-only interaction
            
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
    
    return Positioned(
      left: element.position.dx,
      top: element.position.dy,
      child: GestureDetector(
        onPanStart: (details) => _onElementPanStart(element, details),
        onPanUpdate: (details) => _onElementPanUpdate(element, details),
        onPanEnd: (details) => _onElementPanEnd(element, details),
        behavior: HitTestBehavior.opaque, // Opaque for reliable gesture detection
        child: Transform.rotate(
          angle: element.rotation,
          child: Container(
            width: element.size.width,
            height: element.size.height,
            decoration: BoxDecoration(
              borderRadius: BorderRadius.circular(4.r),
            ),
            child: _buildElementContent(element),
          ),
        ),
      ),
    );
  }

  Widget _buildElementContent(VisionBoardElement element) {
    if (element is VisionBoardImageElement) {
      return Container(
        decoration: BoxDecoration(
          color: AppColors.textMedium.withOpacity(0.1),
          borderRadius: BorderRadius.circular(8.r),
          boxShadow: [
            BoxShadow(
              color: AppColors.black.withOpacity(0.1),
              blurRadius: 4.r,
              offset: Offset(0, 2.h),
            ),
          ],
        ),
        child: ClipRRect(
          borderRadius: BorderRadius.circular(8.r),
          child: _buildImageWidget(element),
        ),
      );
    } else if (element is VisionBoardTextElement) {
      return Container(
        padding: EdgeInsets.all(12.w),
        decoration: BoxDecoration(
          color: element.style.backgroundColor ?? Colors.transparent,
          borderRadius: BorderRadius.circular(8.r),
          boxShadow: element.style.backgroundColor != null ? [
            BoxShadow(
              color: AppColors.black.withOpacity(0.1),
              blurRadius: 4.r,
              offset: Offset(0, 2.h),
            ),
          ] : null,
        ),
        child: Center(
          child: Text(
            element.text,
            style: element.style.copyWith(
              fontSize: (element.style.fontSize ?? 16.sp) * (element.size.width / 200).clamp(0.5, 2.0),
            ),
            textAlign: element.alignment,
            overflow: TextOverflow.visible,
            maxLines: null,
          ),
        ),
      );
    }
    
    return Container();
  }

  Widget _buildImageWidget(VisionBoardImageElement element) {
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
              color: AppColors.secondaryTeal,
              size: 30.w,
            ),
          );
        },
        errorBuilder: (context, error, stackTrace) {
          print('🌐 Network image failed, trying local: $error');
          // Fallback to local image
          if (element.imagePath.isNotEmpty) {
            return Image.file(
              File(element.imagePath),
              fit: element.fit,
              width: double.infinity,
              height: double.infinity,
              errorBuilder: (context, error, stackTrace) => _buildImagePlaceholder(),
            );
          }
          return _buildImagePlaceholder();
        },
      );
    } else if (element.imagePath.isNotEmpty) {
      return Image.file(
        File(element.imagePath),
        fit: element.fit,
        width: double.infinity,
        height: double.infinity,
        errorBuilder: (context, error, stackTrace) => _buildImagePlaceholder(),
      );
    } else {
      return _buildImagePlaceholder();
    }
  }

  Widget _buildImagePlaceholder() {
    return Container(
      decoration: BoxDecoration(
        color: AppColors.backgroundMedium,
        borderRadius: BorderRadius.circular(8.r),
        border: Border.all(
          color: AppColors.textMedium.withOpacity(0.2),
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
              color: AppColors.textMedium,
              size: 32.sp,
            ),
            SizedBox(height: 8.h),
            Text(
              'Tap to add image',
              style: TextStyle(
                color: AppColors.textMedium,
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
          color: AppColors.white.withOpacity(0.9),
          borderRadius: BorderRadius.circular(20.r),
          border: Border.all(
            color: AppColors.textMedium.withOpacity(0.2),
            width: 2,
            style: BorderStyle.solid,
          ),
        ),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            Icon(
              Icons.dashboard_outlined,
              color: AppColors.textMedium.withOpacity(0.5),
              size: 64.sp,
            ),
            SizedBox(height: 16.h),
            Text(
              'Your vision board is empty',
              style: TextStyle(
                fontSize: 20.sp,
                fontWeight: FontWeight.w600,
                color: AppColors.textMedium.withOpacity(0.8),
              ),
              textAlign: TextAlign.center,
            ),
            SizedBox(height: 8.h),
            Text(
              'Start creating by adding images and text\nUse the toolbar below to get started',
              style: TextStyle(
                fontSize: 14.sp,
                color: AppColors.textMedium.withOpacity(0.6),
                height: 1.4,
              ),
              textAlign: TextAlign.center,
            ),
          ],
        ),
      ),
    );
  }

  // Removed tap handling - drag-only interaction

  // Removed selection logic - drag-only interaction

  void _onElementPanStart(VisionBoardElement element, DragStartDetails details) {
    print('🎯 Pan start for element: ${element.id}');
    
    // Always allow dragging - no tool selection needed
    setState(() {
      isDragging = true;
      draggingElementId = element.id;
    });
    
    print('✅ Started dragging element: ${element.id}');
  }

  void _onElementPanUpdate(VisionBoardElement element, DragUpdateDetails details) {
    // Simple: if we're dragging this element, update its position
    if (isDragging && draggingElementId == element.id) {
      final delta = details.delta;
      final newPosition = element.position + delta;
      _updateElementPosition(element, newPosition);
    }
  }
  
  bool _canDragElement(VisionBoardElement element) {
    // Simple check: currently dragging this element
    return isDragging && draggingElementId == element.id;
  }

  void _onElementPanEnd(VisionBoardElement element, DragEndDetails details) {
    print('🛑 Pan end for element: ${element.id}');
    
    // Clear drag states
    if (mounted) {
      setState(() {
        isDragging = false;
        draggingElementId = null;
      });
      
      print('✅ Finished dragging element: ${element.id}');
    }
  }

  // Removed resize and rotation functionality - drag-only interaction

  VisionBoardElement? _getElementAtPosition(Offset position) {
    for (int i = widget.board.elements.length - 1; i >= 0; i--) {
      final element = widget.board.elements[i];
      final rect = Rect.fromLTWH(
        element.position.dx,
        element.position.dy,
        element.size.width,
        element.size.height,
      );
      
      if (rect.contains(position)) {
        return element;
      }
    }
    return null;
  }

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
      ..color = AppColors.textMedium.withOpacity(0.1)
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