import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:fun/models/vision_board.dart';
import '../constants/app_theme.dart';

class VisionBoardToolbar extends StatelessWidget {
  final VisionBoardTool selectedTool;
  final Function(VisionBoardTool) onToolSelected;
  final VoidCallback onAddText;
  final VoidCallback onAddImage;
  final VoidCallback? onUndo;
  final VoidCallback? onRedo;
  final bool canUndo;
  final bool canRedo;

  const VisionBoardToolbar({
    Key? key,
    required this.selectedTool,
    required this.onToolSelected,
    required this.onAddText,
    required this.onAddImage,
    this.onUndo,
    this.onRedo,
    this.canUndo = false,
    this.canRedo = false,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Container(
      margin: EdgeInsets.symmetric(horizontal: 12.w, vertical: 12.h),
      child: Container(
        padding: EdgeInsets.symmetric(horizontal: 12.w, vertical: 14.h),
        decoration: BoxDecoration(
          color: AppTheme.cardBackground,
          borderRadius: BorderRadius.circular(24.r),
          border: Border.all(
            color: AppTheme.visionBoardGradient[0].withOpacity(0.15),
            width: 1,
          ),
          boxShadow: [
            BoxShadow(
              color: AppTheme.black.withOpacity(0.12),
              blurRadius: 24.r,
              offset: Offset(0, 12.h),
            ),
          ],
        ),
        child: Row(
          mainAxisAlignment: MainAxisAlignment.spaceEvenly,
          children: [
            // Undo arrow
            _buildArrowButton(
              icon: Icons.undo_rounded,
              onTap: canUndo ? onUndo : null,
              isEnabled: canUndo,
            ),
            
            // Add Image button
            _buildActionButton(
              icon: Icons.add_photo_alternate_rounded,
              label: 'Image',
              onTap: onAddImage,
              color: AppTheme.accentGold,
            ),
            
            // Add Text button - redesigned
            _buildTextButton(),
            
            // Divider
            Container(
              width: 1.5.w,
              height: 40.h,
              decoration: BoxDecoration(
                gradient: LinearGradient(
                  colors: [
                    AppTheme.visionBoardGradient[0].withOpacity(0.2),
                    AppTheme.visionBoardGradient[1].withOpacity(0.2),
                  ],
                  begin: Alignment.topCenter,
                  end: Alignment.bottomCenter,
                ),
                borderRadius: BorderRadius.circular(1.r),
              ),
            ),
            
            // Move tool
            _buildToolButton(
              icon: Icons.open_in_full_rounded,
              label: 'Move',
              isSelected: selectedTool == VisionBoardTool.move,
              onTap: () => onToolSelected(VisionBoardTool.move),
            ),
            
            // Resize tool
            _buildToolButton(
              icon: Icons.aspect_ratio_rounded,
              label: 'Resize',
              isSelected: selectedTool == VisionBoardTool.resize,
              onTap: () => onToolSelected(VisionBoardTool.resize),
            ),
            
            // Delete tool
            _buildToolButton(
              icon: Icons.delete_outline_rounded,
              label: 'Delete',
              isSelected: selectedTool == VisionBoardTool.delete,
              onTap: () => onToolSelected(VisionBoardTool.delete),
            ),
            
            // Redo arrow
            _buildArrowButton(
              icon: Icons.redo_rounded,
              onTap: canRedo ? onRedo : null,
              isEnabled: canRedo,
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildToolButton({
    required IconData icon,
    required String label,
    required bool isSelected,
    required VoidCallback onTap,
  }) {
    return GestureDetector(
      onTap: onTap,
      child: AnimatedContainer(
        duration: Duration(milliseconds: 200),
        padding: EdgeInsets.symmetric(horizontal: 12.w, vertical: 10.h),
        decoration: BoxDecoration(
          gradient: isSelected 
              ? LinearGradient(
                  colors: AppTheme.visionBoardGradient,
                  begin: Alignment.topLeft,
                  end: Alignment.bottomRight,
                )
              : null,
          color: isSelected ? null : Colors.transparent,
          borderRadius: BorderRadius.circular(12.r),
          border: isSelected 
              ? null
              : Border.all(
                  color: AppTheme.textMedium.withOpacity(0.1),
                  width: 1,
                ),
        ),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            Icon(
              icon,
              color: isSelected ? AppTheme.white : AppTheme.textMedium,
              size: 20.sp,
            ),
            SizedBox(height: 4.h),
            Text(
              label,
              style: TextStyle(
                fontSize: 10.sp,
                fontWeight: isSelected ? FontWeight.w600 : FontWeight.w500,
                color: isSelected ? AppTheme.white : AppTheme.textMedium,
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildActionButton({
    required IconData icon,
    required String label,
    required VoidCallback onTap,
    required Color color,
  }) {
    return GestureDetector(
      onTap: onTap,
      child: Container(
        padding: EdgeInsets.symmetric(horizontal: 10.w, vertical: 8.h),
        decoration: BoxDecoration(
          gradient: LinearGradient(
            colors: [color, color.withOpacity(0.8)],
            begin: Alignment.topLeft,
            end: Alignment.bottomRight,
          ),
          borderRadius: BorderRadius.circular(12.r),
          boxShadow: [
            BoxShadow(
              color: color.withOpacity(0.3),
              blurRadius: 8.r,
              offset: Offset(0, 4.h),
            ),
          ],
        ),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            Icon(
              icon,
              color: AppTheme.white,
              size: 18.sp,
            ),
            SizedBox(height: 3.h),
            Text(
              label,
              style: TextStyle(
                fontSize: 9.sp,
                fontWeight: FontWeight.w600,
                color: AppTheme.white,
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildTextButton() {
    return GestureDetector(
      onTap: onAddText,
      child: Container(
        padding: EdgeInsets.symmetric(horizontal: 10.w, vertical: 8.h),
        decoration: BoxDecoration(
          gradient: LinearGradient(
            colors: [
              Color(0xFF6C63FF),
              Color(0xFF5A52E8),
            ],
            begin: Alignment.topLeft,
            end: Alignment.bottomRight,
          ),
          borderRadius: BorderRadius.circular(12.r),
          boxShadow: [
            BoxShadow(
              color: Color(0xFF6C63FF).withOpacity(0.4),
              blurRadius: 12.r,
              offset: Offset(0, 6.h),
            ),
          ],
        ),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            Stack(
              alignment: Alignment.center,
              children: [
                Container(
                  width: 22.w,
                  height: 16.h,
                  decoration: BoxDecoration(
                    color: AppTheme.white.withOpacity(0.2),
                    borderRadius: BorderRadius.circular(3.r),
                  ),
                ),
                Text(
                  'Aa',
                  style: TextStyle(
                    fontSize: 12.sp,
                    fontWeight: FontWeight.bold,
                    color: AppTheme.white,
                  ),
                ),
              ],
            ),
            SizedBox(height: 3.h),
            Text(
              'Text',
              style: TextStyle(
                fontSize: 9.sp,
                fontWeight: FontWeight.w600,
                color: AppTheme.white,
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildArrowButton({
    required IconData icon,
    required VoidCallback? onTap,
    required bool isEnabled,
  }) {
    return GestureDetector(
      onTap: isEnabled ? onTap : null,
      child: Container(
        width: 36.w,
        height: 36.h,
        decoration: BoxDecoration(
          color: isEnabled 
              ? AppTheme.visionBoardGradient[0].withOpacity(0.1)
              : AppTheme.textMedium.withOpacity(0.05),
          borderRadius: BorderRadius.circular(18.r),
          border: Border.all(
            color: isEnabled 
                ? AppTheme.visionBoardGradient[0].withOpacity(0.3)
                : AppTheme.textMedium.withOpacity(0.1),
            width: 1,
          ),
        ),
        child: Icon(
          icon,
          color: isEnabled 
              ? AppTheme.visionBoardGradient[0]
              : AppTheme.textMedium.withOpacity(0.3),
          size: 18.sp,
        ),
      ),
    );
  }

}

// Extension to add the missing field to toolbar
extension VisionBoardToolbarExtension on VisionBoardToolbar {
  void showToolSelector(BuildContext context) {
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
                color: AppTheme.textMedium.withOpacity(0.3),
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
                      color: AppTheme.textLight,
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
                          onToolSelected(tool);
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
                                      AppTheme.journalGradient[0].withOpacity(0.1),
                                      AppTheme.goalTrackerGradient[0].withOpacity(0.1),
                                    ],
                                  )
                                : null,
                            color: isSelected ? null : AppTheme.backgroundMedium,
                            borderRadius: BorderRadius.circular(12.r),
                            border: Border.all(
                              color: isSelected
                                  ? AppTheme.journalGradient[0].withOpacity(0.3)
                                  : AppTheme.textMedium.withOpacity(0.2),
                              width: isSelected ? 2 : 1,
                            ),
                            boxShadow: isSelected ? [
                              BoxShadow(
                                color: AppTheme.journalGradient[0].withOpacity(0.2),
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
                                      ? AppTheme.journalGradient[0].withOpacity(0.1)
                                      : Colors.transparent,
                                  borderRadius: BorderRadius.circular(8.r),
                                ),
                                child: Icon(
                                  tool.icon,
                                  color: isSelected
                                      ? AppTheme.journalGradient[0]
                                      : AppTheme.textMedium,
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
                                      ? AppTheme.journalGradient[0]
                                      : AppTheme.textMedium,
                                ),
                                textAlign: TextAlign.center,
                              ),
                              if (isSelected) ...[
                                SizedBox(height: 4.h),
                                Container(
                                  width: 20.w,
                                  height: 2.h,
                                  decoration: BoxDecoration(
                                    color: AppTheme.journalGradient[0],
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
                      color: AppTheme.textMedium,
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
        return 'Move elements around the canvas freely.';
      case VisionBoardTool.resize:
        return 'Resize elements by dragging the corners.';
      case VisionBoardTool.rotate:
        return 'Rotate elements by dragging in a circular motion.';
      case VisionBoardTool.delete:
        return 'Tap elements to delete them from the board.';
    }
  }
}