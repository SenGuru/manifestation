import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:fun/models/vision_board.dart';
import 'package:fun/constants/colors.dart';

class VisionBoardToolbar extends StatelessWidget {
  final VisionBoardTool selectedTool;
  final Function(VisionBoardTool) onToolSelected;
  final VoidCallback onAddImage;
  final VoidCallback onAddText;
  final VoidCallback onUndo;
  final VoidCallback onRedo;
  final VoidCallback onSave;
  final bool canUndo;
  final bool canRedo;

  const VisionBoardToolbar({
    Key? key,
    required this.selectedTool,
    required this.onToolSelected,
    required this.onAddImage,
    required this.onAddText,
    required this.onUndo,
    required this.onRedo,
    required this.onSave,
    this.canUndo = false,
    this.canRedo = false,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Container(
      margin: EdgeInsets.symmetric(horizontal: 20.w),
      padding: EdgeInsets.symmetric(horizontal: 16.w, vertical: 12.h),
      decoration: BoxDecoration(
        color: AppColors.cardBackground,
        borderRadius: BorderRadius.circular(20.r),
        border: Border.all(
          color: AppColors.textMedium.withOpacity(0.2),
        ),
        boxShadow: [
          BoxShadow(
            color: AppColors.black.withOpacity(0.1),
            blurRadius: 10.r,
            offset: Offset(0, 4.h),
          ),
        ],
      ),
      child: SingleChildScrollView(
        scrollDirection: Axis.horizontal,
        child: Row(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            // Add Image
            _buildActionButton(
              icon: Icons.add_photo_alternate,
              label: 'Image',
              onTap: onAddImage,
              color: AppColors.secondaryTeal,
              isAction: true,
            ),
            
            SizedBox(width: 8.w),
            
            // Add Text
            _buildActionButton(
              icon: Icons.text_fields,
              label: 'Text',
              onTap: onAddText,
              color: AppColors.accentGold,
              isAction: true,
            ),
            
            // Tool separator
            Container(
              width: 1,
              height: 30.h,
              margin: EdgeInsets.symmetric(horizontal: 16.w),
              color: AppColors.textMedium.withOpacity(0.2),
            ),
            
            // Tool selector
            _buildToolSelector(),
            
            // Tool separator
            Container(
              width: 1,
              height: 30.h,
              margin: EdgeInsets.symmetric(horizontal: 16.w),
              color: AppColors.textMedium.withOpacity(0.2),
            ),
            
            // Undo
            _buildActionButton(
              icon: Icons.undo,
              label: 'Undo',
              onTap: canUndo ? onUndo : null,
              color: canUndo ? AppColors.textLight : AppColors.textMedium,
              isEnabled: canUndo,
            ),
            
            SizedBox(width: 8.w),
            
            // Redo
            _buildActionButton(
              icon: Icons.redo,
              label: 'Redo',
              onTap: canRedo ? onRedo : null,
              color: canRedo ? AppColors.textLight : AppColors.textMedium,
              isEnabled: canRedo,
            ),
            
            // Tool separator
            Container(
              width: 1,
              height: 30.h,
              margin: EdgeInsets.symmetric(horizontal: 16.w),
              color: AppColors.textMedium.withOpacity(0.2),
            ),
            
            // Save
            _buildActionButton(
              icon: Icons.save,
              label: 'Save',
              onTap: onSave,
              color: AppColors.accentMint,
              isAction: true,
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildActionButton({
    required IconData icon,
    required String label,
    required VoidCallback? onTap,
    required Color color,
    bool isSelected = false,
    bool isAction = false,
    bool isEnabled = true,
  }) {
    return GestureDetector(
      onTap: isEnabled ? onTap : null,
      child: AnimatedContainer(
        duration: Duration(milliseconds: 200),
        padding: EdgeInsets.symmetric(horizontal: 12.w, vertical: 8.h),
        decoration: BoxDecoration(
          color: isSelected 
              ? color.withOpacity(0.15) 
              : (isAction ? color.withOpacity(0.1) : Colors.transparent),
          borderRadius: BorderRadius.circular(10.r),
          border: isSelected
              ? Border.all(color: color.withOpacity(0.3))
              : null,
        ),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            AnimatedContainer(
              duration: Duration(milliseconds: 200),
              padding: EdgeInsets.all(isAction ? 4.w : 2.w),
              decoration: BoxDecoration(
                color: isAction ? color.withOpacity(0.1) : Colors.transparent,
                borderRadius: BorderRadius.circular(6.r),
              ),
              child: Icon(
                icon,
                color: isEnabled ? color : color.withOpacity(0.4),
                size: isAction ? 22.sp : 20.sp,
              ),
            ),
            SizedBox(height: 4.h),
            Text(
              label,
              style: TextStyle(
                fontSize: 10.sp,
                fontWeight: isSelected ? FontWeight.w600 : FontWeight.w500,
                color: isEnabled 
                    ? (isSelected ? color : AppColors.textMedium) 
                    : AppColors.textMedium.withOpacity(0.4),
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildToolSelector() {
    return GestureDetector(
      onTap: () => _showToolSelector(),
      child: Container(
        padding: EdgeInsets.symmetric(horizontal: 12.w, vertical: 8.h),
        decoration: BoxDecoration(
          gradient: LinearGradient(
            colors: [
              AppColors.gradientPurple.withOpacity(0.1),
              AppColors.gradientBlue.withOpacity(0.1),
            ],
          ),
          borderRadius: BorderRadius.circular(10.r),
          border: Border.all(
            color: AppColors.gradientPurple.withOpacity(0.3),
          ),
        ),
        child: Row(
          mainAxisSize: MainAxisSize.min,
          children: [
            Column(
              mainAxisSize: MainAxisSize.min,
              children: [
                Container(
                  padding: EdgeInsets.all(4.w),
                  decoration: BoxDecoration(
                    color: AppColors.gradientPurple.withOpacity(0.1),
                    borderRadius: BorderRadius.circular(6.r),
                  ),
                  child: Icon(
                    selectedTool.icon,
                    color: AppColors.gradientPurple,
                    size: 22.sp,
                  ),
                ),
                SizedBox(height: 4.h),
                Text(
                  selectedTool.name,
                  style: TextStyle(
                    fontSize: 10.sp,
                    fontWeight: FontWeight.w600,
                    color: AppColors.gradientPurple,
                  ),
                ),
              ],
            ),
            SizedBox(width: 6.w),
            Icon(
              Icons.keyboard_arrow_down,
              color: AppColors.gradientPurple,
              size: 16.sp,
            ),
          ],
        ),
      ),
    );
  }

  void _showToolSelector() {
    // This will be called from the parent widget
    // We need to pass the context through
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
    }
  }
}