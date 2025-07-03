import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:manifestation_app/vision_board.dart';
import '../colors.dart';

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
  VisionBoardElement? selectedElement;
  Offset? dragOffset;
  Offset? initialPanPosition;
  Size? initialElementSize;
  double? initialRotation;
  bool isDragging = false;
  bool isResizing = false;
  bool isRotating = false;

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
        onTapDown: _onTapDown,
        child: InteractiveViewer(
          transformationController: transformController,
          minScale: 0.1,
          maxScale: 5.0,
          constrained: false,
          child: Container(
            width: MediaQuery.of(context).size.width * 2,
            height: MediaQuery.of(context).size.height * 2,
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
                
                // Selection overlay
                if (selectedElement != null)
                  _buildSelectionOverlay(selectedElement!),
                
                // Empty state
                if (widget.board.elements.isEmpty)
                  _buildEmptyState(),
              ],
            ),
          ),
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
    final isSelected = selectedElement?.id == element.id;
    
    return Positioned(
      left: element.position.dx,
      top: element.position.dy,
      child: GestureDetector(
        onTapDown: (_) => _selectElement(element),
        onPanStart: (details) => _onElementPanStart(element, details),
        onPanUpdate: (details) => _onElementPanUpdate(element, details),
        onPanEnd: (details) => _onElementPanEnd(element, details),
        child: Transform.rotate(
          angle: element.rotation,
          child: Container(
            width: element.size.width,
            height: element.size.height,
            decoration: BoxDecoration(
              border: isSelected
                  ? Border.all(
                      color: AppColors.secondaryTeal.withOpacity(0.3),
                      width: 1,
                    )
                  : null,
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
          child: element.imagePath.isNotEmpty
              ? Image.asset(
                  element.imagePath,
                  fit: element.fit,
                  width: double.infinity,
                  height: double.infinity,
                  errorBuilder: (context, error, stackTrace) => _buildImagePlaceholder(),
                )
              : _buildImagePlaceholder(),
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

  Widget _buildSelectionOverlay(VisionBoardElement element) {
    return Positioned(
      left: element.position.dx - 8.w,
      top: element.position.dy - 8.h,
      child: Container(
        width: element.size.width + 16.w,
        height: element.size.height + 16.h,
        child: Stack(
          children: [
            // Selection border
            Positioned.fill(
              child: Container(
                decoration: BoxDecoration(
                  border: Border.all(
                    color: AppColors.secondaryTeal,
                    width: 2,
                  ),
                  borderRadius: BorderRadius.circular(8.r),
                ),
              ),
            ),
            
            // Corner handles for resizing
            if (widget.selectedTool == VisionBoardTool.resize)
              ..._buildResizeHandles(element),
            
            // Rotation handle
            if (widget.selectedTool == VisionBoardTool.rotate)
              _buildRotationHandle(element),
            
            // Delete button
            _buildDeleteButton(element),
            
            // Layer controls
            _buildLayerControls(element),
          ],
        ),
      ),
    );
  }

  List<Widget> _buildResizeHandles(VisionBoardElement element) {
    return [
      // Corner handles
      _buildResizeHandle(element, ResizeDirection.topLeft, -8.w, -8.h),
      _buildResizeHandle(element, ResizeDirection.topRight, element.size.width, -8.h),
      _buildResizeHandle(element, ResizeDirection.bottomLeft, -8.w, element.size.height),
      _buildResizeHandle(element, ResizeDirection.bottomRight, element.size.width, element.size.height),
      
      // Edge handles
      _buildResizeHandle(element, ResizeDirection.top, element.size.width / 2 - 6.w, -8.h),
      _buildResizeHandle(element, ResizeDirection.bottom, element.size.width / 2 - 6.w, element.size.height),
      _buildResizeHandle(element, ResizeDirection.left, -8.w, element.size.height / 2 - 6.h),
      _buildResizeHandle(element, ResizeDirection.right, element.size.width, element.size.height / 2 - 6.h),
    ];
  }

  Widget _buildResizeHandle(VisionBoardElement element, ResizeDirection direction, double left, double top) {
    return Positioned(
      left: left,
      top: top,
      child: GestureDetector(
        onPanStart: (details) => _onResizeStart(element, direction, details),
        onPanUpdate: (details) => _onResizeUpdate(element, direction, details),
        onPanEnd: (details) => _onResizeEnd(element, details),
        child: Container(
          width: 12.w,
          height: 12.h,
          decoration: BoxDecoration(
            color: AppColors.secondaryTeal,
            shape: BoxShape.circle,
            border: Border.all(
              color: AppColors.white,
              width: 2,
            ),
            boxShadow: [
              BoxShadow(
                color: AppColors.black.withOpacity(0.2),
                blurRadius: 4.r,
                offset: Offset(0, 2.h),
              ),
            ],
          ),
        ),
      ),
    );
  }

  Widget _buildRotationHandle(VisionBoardElement element) {
    return Positioned(
      top: -32.h,
      left: (element.size.width / 2) - 12.w,
      child: GestureDetector(
        onPanStart: (details) => _onRotationStart(element, details),
        onPanUpdate: (details) => _onRotationUpdate(element, details),
        onPanEnd: (details) => _onRotationEnd(element, details),
        child: Container(
          width: 24.w,
          height: 24.h,
          decoration: BoxDecoration(
            color: AppColors.accentGold,
            shape: BoxShape.circle,
            border: Border.all(
              color: AppColors.white,
              width: 2,
            ),
            boxShadow: [
              BoxShadow(
                color: AppColors.black.withOpacity(0.2),
                blurRadius: 4.r,
                offset: Offset(0, 2.h),
              ),
            ],
          ),
          child: Icon(
            Icons.rotate_right,
            color: AppColors.white,
            size: 14.sp,
          ),
        ),
      ),
    );
  }

  Widget _buildDeleteButton(VisionBoardElement element) {
    return Positioned(
      top: -12.h,
      right: -12.w,
      child: GestureDetector(
        onTap: () => _deleteElement(element),
        child: Container(
          width: 24.w,
          height: 24.h,
          decoration: BoxDecoration(
            color: Colors.red,
            shape: BoxShape.circle,
            border: Border.all(
              color: AppColors.white,
              width: 2,
            ),
            boxShadow: [
              BoxShadow(
                color: AppColors.black.withOpacity(0.2),
                blurRadius: 4.r,
                offset: Offset(0, 2.h),
              ),
            ],
          ),
          child: Icon(
            Icons.close,
            color: AppColors.white,
            size: 14.sp,
          ),
        ),
      ),
    );
  }

  Widget _buildLayerControls(VisionBoardElement element) {
    return Positioned(
      top: -12.h,
      left: -12.w,
      child: Column(
        children: [
          // Bring to front
          GestureDetector(
            onTap: () => _bringToFront(element),
            child: Container(
              width: 20.w,
              height: 20.h,
              decoration: BoxDecoration(
                color: AppColors.gradientPurple,
                borderRadius: BorderRadius.circular(4.r),
                border: Border.all(color: AppColors.white, width: 1),
              ),
              child: Icon(Icons.keyboard_arrow_up, color: AppColors.white, size: 12.sp),
            ),
          ),
          SizedBox(height: 4.h),
          // Send to back
          GestureDetector(
            onTap: () => _sendToBack(element),
            child: Container(
              width: 20.w,
              height: 20.h,
              decoration: BoxDecoration(
                color: AppColors.gradientPurple.withOpacity(0.7),
                borderRadius: BorderRadius.circular(4.r),
                border: Border.all(color: AppColors.white, width: 1),
              ),
              child: Icon(Icons.keyboard_arrow_down, color: AppColors.white, size: 12.sp),
            ),
          ),
        ],
      ),
    );
  }

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

  void _onTapDown(TapDownDetails details) {
    if (widget.selectedTool == VisionBoardTool.select) {
      final tappedElement = _getElementAtPosition(details.localPosition);
      setState(() {
        selectedElement = tappedElement;
      });
    }
  }

  void _selectElement(VisionBoardElement element) {
    setState(() {
      selectedElement = element;
    });
  }

  void _onElementPanStart(VisionBoardElement element, DragStartDetails details) {
    if (widget.selectedTool == VisionBoardTool.move || widget.selectedTool == VisionBoardTool.select) {
      setState(() {
        selectedElement = element;
        dragOffset = details.localPosition;
        isDragging = true;
      });
    }
  }

  void _onElementPanUpdate(VisionBoardElement element, DragUpdateDetails details) {
    if (isDragging && (widget.selectedTool == VisionBoardTool.move || widget.selectedTool == VisionBoardTool.select)) {
      final newPosition = element.position + (details.localPosition - dragOffset!);
      _updateElementPosition(element, newPosition);
    }
  }

  void _onElementPanEnd(VisionBoardElement element, DragEndDetails details) {
    setState(() {
      isDragging = false;
      dragOffset = null;
    });
  }

  // Resize functionality
  void _onResizeStart(VisionBoardElement element, ResizeDirection direction, DragStartDetails details) {
    setState(() {
      isResizing = true;
      initialPanPosition = details.localPosition;
      initialElementSize = element.size;
    });
  }

  void _onResizeUpdate(VisionBoardElement element, ResizeDirection direction, DragUpdateDetails details) {
    if (!isResizing || initialElementSize == null || initialPanPosition == null) return;

    final delta = details.localPosition - initialPanPosition!;
    Size newSize = initialElementSize!;
    Offset newPosition = element.position;

    switch (direction) {
      case ResizeDirection.topLeft:
        newSize = Size(
          (initialElementSize!.width - delta.dx).clamp(50.0, double.infinity),
          (initialElementSize!.height - delta.dy).clamp(50.0, double.infinity),
        );
        newPosition = Offset(
          element.position.dx + (initialElementSize!.width - newSize.width),
          element.position.dy + (initialElementSize!.height - newSize.height),
        );
        break;
      case ResizeDirection.topRight:
        newSize = Size(
          (initialElementSize!.width + delta.dx).clamp(50.0, double.infinity),
          (initialElementSize!.height - delta.dy).clamp(50.0, double.infinity),
        );
        newPosition = Offset(
          element.position.dx,
          element.position.dy + (initialElementSize!.height - newSize.height),
        );
        break;
      case ResizeDirection.bottomLeft:
        newSize = Size(
          (initialElementSize!.width - delta.dx).clamp(50.0, double.infinity),
          (initialElementSize!.height + delta.dy).clamp(50.0, double.infinity),
        );
        newPosition = Offset(
          element.position.dx + (initialElementSize!.width - newSize.width),
          element.position.dy,
        );
        break;
      case ResizeDirection.bottomRight:
        newSize = Size(
          (initialElementSize!.width + delta.dx).clamp(50.0, double.infinity),
          (initialElementSize!.height + delta.dy).clamp(50.0, double.infinity),
        );
        break;
      case ResizeDirection.top:
        newSize = Size(
          initialElementSize!.width,
          (initialElementSize!.height - delta.dy).clamp(50.0, double.infinity),
        );
        newPosition = Offset(
          element.position.dx,
          element.position.dy + (initialElementSize!.height - newSize.height),
        );
        break;
      case ResizeDirection.bottom:
        newSize = Size(
          initialElementSize!.width,
          (initialElementSize!.height + delta.dy).clamp(50.0, double.infinity),
        );
        break;
      case ResizeDirection.left:
        newSize = Size(
          (initialElementSize!.width - delta.dx).clamp(50.0, double.infinity),
          initialElementSize!.height,
        );
        newPosition = Offset(
          element.position.dx + (initialElementSize!.width - newSize.width),
          element.position.dy,
        );
        break;
      case ResizeDirection.right:
        newSize = Size(
          (initialElementSize!.width + delta.dx).clamp(50.0, double.infinity),
          initialElementSize!.height,
        );
        break;
    }

    _updateElement(element, position: newPosition, size: newSize);
  }

  void _onResizeEnd(VisionBoardElement element, DragEndDetails details) {
    setState(() {
      isResizing = false;
      initialPanPosition = null;
      initialElementSize = null;
    });
  }

  // Rotation functionality
  void _onRotationStart(VisionBoardElement element, DragStartDetails details) {
    setState(() {
      isRotating = true;
      initialRotation = element.rotation;
      initialPanPosition = details.localPosition;
    });
  }

  void _onRotationUpdate(VisionBoardElement element, DragUpdateDetails details) {
    if (!isRotating || initialRotation == null || initialPanPosition == null) return;

    final center = Offset(element.size.width / 2, element.size.height / 2);
    final initialAngle = (initialPanPosition! - center).direction;
    final currentAngle = (details.localPosition - center).direction;
    final deltaAngle = currentAngle - initialAngle;
    
    final newRotation = (initialRotation! + deltaAngle) % (2 * 3.14159);
    _updateElement(element, rotation: newRotation);
  }

  void _onRotationEnd(VisionBoardElement element, DragEndDetails details) {
    setState(() {
      isRotating = false;
      initialRotation = null;
      initialPanPosition = null;
    });
  }

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

  void _updateElementPosition(VisionBoardElement element, Offset newPosition) {
    _updateElement(element, position: newPosition);
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

  void _deleteElement(VisionBoardElement element) {
    final updatedElements = widget.board.elements
        .where((e) => e.id != element.id)
        .toList();

    final updatedBoard = widget.board.copyWith(elements: updatedElements);
    widget.onBoardChanged(updatedBoard);
    
    setState(() {
      selectedElement = null;
    });
  }

  void _bringToFront(VisionBoardElement element) {
    final maxZIndex = widget.board.elements
        .map((e) => e.zIndex)
        .fold(0, (max, current) => current > max ? current : max);
    
    _updateElement(element, zIndex: maxZIndex + 1);
  }

  void _sendToBack(VisionBoardElement element) {
    final minZIndex = widget.board.elements
        .map((e) => e.zIndex)
        .fold(0, (min, current) => current < min ? current : min);
    
    _updateElement(element, zIndex: minZIndex - 1);
  }
}

enum ResizeDirection {
  topLeft,
  topRight,
  bottomLeft,
  bottomRight,
  top,
  bottom,
  left,
  right,
}

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