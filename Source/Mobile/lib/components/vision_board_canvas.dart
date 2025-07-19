import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:manifestation_app/screens/vision_board.dart';
import 'dart:io';
import 'package:manifestation_app/constants/colors.dart';

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
  // Simple dragging state
  bool isDragging = false;

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
        behavior: HitTestBehavior.translucent,
        child: Stack(
          children: [
            // Background grid - always show for better visual reference
            _buildGrid(),
            
            // Board elements
            ...widget.board.elements.asMap().entries.map((entry) {
              final index = entry.key;
              final element = entry.value;
              return _buildElement(element, index);
            }),
            
            // No more selection overlay - removed for better UX
            
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
    // Remove selection check - no more visual selection needed
    
    return Positioned(
      left: element.position.dx,
      top: element.position.dy,
      child: GestureDetector(
        onTap: () => _selectElement(element),
        onPanStart: (details) {
          setState(() => isDragging = true);
        },
        onPanUpdate: (details) {
          // Direct dragging - elements are always moveable
          final newPosition = element.position + details.delta;
          _updateElementPosition(element, newPosition);
        },
        onPanEnd: (details) {
          setState(() => isDragging = false);
        },
        behavior: HitTestBehavior.opaque,
        child: Container(
          width: element.size.width,
          height: element.size.height,
          child: _buildElementContent(element),
        ),
      ),
    );
  }

  Widget _buildElementContent(VisionBoardElement element) {
    if (element is VisionBoardImageElement) {
      return ClipRRect(
        borderRadius: BorderRadius.circular(8.r),
        child: Container(
          decoration: BoxDecoration(
            color: AppColors.backgroundLight,
            borderRadius: BorderRadius.circular(8.r),
          ),
          child: element.imagePath.isNotEmpty
              ? Image.file(
                  File(element.imagePath),
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
        padding: EdgeInsets.all(8.w),
        child: Center(
          child: Text(
            element.text,
            style: element.style.copyWith(
              fontSize: (element.style.fontSize ?? 16.sp) * (element.size.width / 200).clamp(0.8, 2.0),
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
        color: AppColors.backgroundLight,
        borderRadius: BorderRadius.circular(8.r),
        border: Border.all(
          color: AppColors.textMedium.withOpacity(0.15),
          width: 1,
        ),
      ),
      child: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Icon(
              Icons.image_outlined,
              color: AppColors.textMedium.withOpacity(0.6),
              size: 28.sp,
            ),
            SizedBox(height: 8.h),
            Text(
              'Add Image',
              style: TextStyle(
                color: AppColors.textMedium.withOpacity(0.7),
                fontSize: 11.sp,
                fontWeight: FontWeight.w500,
              ),
            ),
          ],
        ),
      ),
    );
  }

  // Selection overlay removed - no more rectangle selection UI


  Widget _buildEmptyState() {
    return Center(
      child: Container(
        margin: EdgeInsets.symmetric(horizontal: 40.w),
        padding: EdgeInsets.all(40.w),
        decoration: BoxDecoration(
          color: AppColors.white.withOpacity(0.95),
          borderRadius: BorderRadius.circular(24.r),
          border: Border.all(
            color: AppColors.textMedium.withOpacity(0.1),
            width: 1,
          ),
          boxShadow: [
            BoxShadow(
              color: AppColors.black.withOpacity(0.05),
              blurRadius: 20.r,
              offset: Offset(0, 8.h),
            ),
          ],
        ),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            Container(
              width: 80.w,
              height: 80.h,
              decoration: BoxDecoration(
                color: AppColors.accentGold.withOpacity(0.1),
                borderRadius: BorderRadius.circular(20.r),
              ),
              child: Icon(
                Icons.dashboard_outlined,
                color: AppColors.accentGold.withOpacity(0.7),
                size: 40.sp,
              ),
            ),
            SizedBox(height: 24.h),
            Text(
              'Your vision board is empty',
              style: TextStyle(
                fontSize: 22.sp,
                fontWeight: FontWeight.w700,
                color: AppColors.textDark,
              ),
              textAlign: TextAlign.center,
            ),
            SizedBox(height: 12.h),
            Text(
              'Start creating by adding images and text.\nUse the toolbar below to get started.',
              style: TextStyle(
                fontSize: 16.sp,
                color: AppColors.textMedium.withOpacity(0.8),
                height: 1.5,
              ),
              textAlign: TextAlign.center,
            ),
          ],
        ),
      ),
    );
  }

  void _onTapDown(TapDownDetails details) {
    // Handle background taps for deselecting elements or other canvas interactions
    // Could add new element creation logic here in the future
  }

  void _selectElement(VisionBoardElement element) {
    // Handle element selection based on current tool
    if (widget.selectedTool == VisionBoardTool.delete) {
      _deleteElement(element);
      return;
    }
    // For move tool, selection is handled directly in gesture detection
  }



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

  void _deleteElement(VisionBoardElement element) {
    final updatedElements = widget.board.elements
        .where((e) => e.id != element.id)
        .toList();

    final updatedBoard = widget.board.copyWith(elements: updatedElements);
    widget.onBoardChanged(updatedBoard);
  }
}


class GridPainter extends CustomPainter {
  @override
  void paint(Canvas canvas, Size size) {
    final paint = Paint()
      ..color = AppColors.textMedium.withOpacity(0.05)
      ..strokeWidth = 0.5;

    const gridSize = 30.0;

    // Draw vertical lines with better spacing
    for (double x = gridSize; x < size.width; x += gridSize) {
      canvas.drawLine(
        Offset(x, 0),
        Offset(x, size.height),
        paint,
      );
    }

    // Draw horizontal lines with better spacing
    for (double y = gridSize; y < size.height; y += gridSize) {
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