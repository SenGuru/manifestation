import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:manifestation_app/constants/colors.dart';
import 'package:manifestation_app/screens/affirmations.dart';
import 'package:uuid/uuid.dart';

class AddCategoryDialog extends StatefulWidget {
  final AffirmationCategory? categoryToEdit;

  const AddCategoryDialog({
    Key? key,
    this.categoryToEdit,
  }) : super(key: key);

  @override
  _AddCategoryDialogState createState() => _AddCategoryDialogState();
}

class _AddCategoryDialogState extends State<AddCategoryDialog> {
  late TextEditingController _nameController;
  final _formKey = GlobalKey<FormState>();
  bool _isLoading = false;
  Color _selectedColor = AppColors.gradientPink;
  final List<Color> _availableColors = [
    AppColors.gradientPink,
    AppColors.gradientOrange,
    AppColors.gradientPurple,
    AppColors.gradientBlue,
    AppColors.gradientTeal,
    AppColors.gradientIndigo,
    AppColors.accentGold,
    AppColors.accentLavender,
    AppColors.accentMint,
    AppColors.secondaryGold,
    AppColors.secondaryRose,
    AppColors.secondaryTeal,
  ];

// use link2db as the orm
// load the configuration file locally from a separate folder that resides outside the source folder. 
// when returning data in the api, you need to return using a return model or a return list model
// create appropriate loggging and store configuration in the database in the configuration item table. 
// create a separate service layer and a repository layer. 
  @override
  void initState() {
    super.initState();
    _nameController = TextEditingController(
      text: widget.categoryToEdit?.name ?? '',
    );
    _selectedColor = widget.categoryToEdit?.color ?? AppColors.gradientPink;
  }

  @override
  void dispose() {
    _nameController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final isEditing = widget.categoryToEdit != null;
    
    return Dialog(
      backgroundColor: AppColors.cardBackground,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(20.r),
      ),
      child: Container(
        padding: EdgeInsets.all(24.w),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            // Header
            Row(
              children: [
                Container(
                  width: 40.w,
                  height: 40.h,
                  decoration: BoxDecoration(
                    color: _selectedColor.withOpacity(0.2),
                    borderRadius: BorderRadius.circular(10.r),
                  ),
                  child: Icon(
                    isEditing ? Icons.edit : Icons.category,
                    color: _selectedColor,
                    size: 20.sp,
                  ),
                ),
                SizedBox(width: 12.w),
                Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(
                        isEditing ? 'Edit Category' : 'New Category',
                        style: TextStyle(
                          fontSize: 18.sp,
                          fontWeight: FontWeight.bold,
                          color: AppColors.textLight,
                        ),
                      ),
                      Text(
                        'Organize your manifestations',
                        style: TextStyle(
                          fontSize: 12.sp,
                          color: AppColors.textMedium,
                        ),
                      ),
                    ],
                  ),
                ),
                GestureDetector(
                  onTap: () => Navigator.pop(context),
                  child: Icon(
                    Icons.close,
                    color: AppColors.textMedium,
                    size: 20.sp,
                  ),
                ),
              ],
            ),
            
            SizedBox(height: 24.h),
            
            // Form
            Form(
              key: _formKey,
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    'Category Name',
                    style: TextStyle(
                      fontSize: 14.sp,
                      fontWeight: FontWeight.w600,
                      color: AppColors.textLight,
                    ),
                  ),
                  SizedBox(height: 8.h),
                  Container(
                    decoration: BoxDecoration(
                      color: AppColors.backgroundMedium,
                      borderRadius: BorderRadius.circular(12.r),
                      border: Border.all(
                        color: AppColors.textMedium.withOpacity(0.2),
                      ),
                    ),
                    child: TextFormField(
                      controller: _nameController,
                      style: TextStyle(
                        fontSize: 16.sp,
                        color: AppColors.textLight,
                      ),
                      decoration: InputDecoration(
                        hintText: 'e.g., Career, Self-Love, Prosperity',
                        hintStyle: TextStyle(
                          color: AppColors.textMedium.withOpacity(0.7),
                          fontSize: 14.sp,
                        ),
                        border: InputBorder.none,
                        contentPadding: EdgeInsets.all(16.w),
                      ),
                      validator: (value) {
                        if (value == null || value.trim().isEmpty) {
                          return 'Please enter a category name';
                        }
                        if (value.trim().length < 2) {
                          return 'Category name should be at least 2 characters';
                        }
                        return null;
                      },
                    ),
                  ),
                  
                  SizedBox(height: 20.h),
                  
                  // Color selection
                  Text(
                    'Category Color',
                    style: TextStyle(
                      fontSize: 14.sp,
                      fontWeight: FontWeight.w600,
                      color: AppColors.textLight,
                    ),
                  ),
                  SizedBox(height: 12.h),
                  Container(
                    height: 60.h,
                    child: GridView.builder(
                      scrollDirection: Axis.horizontal,
                      gridDelegate: SliverGridDelegateWithFixedCrossAxisCount(
                        crossAxisCount: 2,
                        mainAxisSpacing: 8.w,
                        crossAxisSpacing: 8.h,
                        childAspectRatio: 1.0,
                      ),
                      itemCount: _availableColors.length,
                      itemBuilder: (context, index) {
                        final color = _availableColors[index];
                        final isSelected = color == _selectedColor;
                        
                        return GestureDetector(
                          onTap: () {
                            setState(() {
                              _selectedColor = color;
                            });
                          },
                          child: Container(
                            width: 24.w,
                            height: 24.h,
                            decoration: BoxDecoration(
                              color: color,
                              shape: BoxShape.circle,
                              border: Border.all(
                                color: isSelected 
                                    ? AppColors.white 
                                    : Colors.transparent,
                                width: 2,
                              ),
                              boxShadow: isSelected ? [
                                BoxShadow(
                                  color: color.withOpacity(0.4),
                                  blurRadius: 8.r,
                                  offset: Offset(0, 2.h),
                                ),
                              ] : null,
                            ),
                            child: isSelected 
                                ? Icon(
                                    Icons.check,
                                    color: AppColors.white,
                                    size: 12.sp,
                                  )
                                : null,
                          ),
                        );
                      },
                    ),
                  ),
                ],
              ),
            ),
            
            SizedBox(height: 24.h),
            
            // Action buttons
            Row(
              children: [
                Expanded(
                  child: GestureDetector(
                    onTap: () => Navigator.pop(context),
                    child: Container(
                      height: 48.h,
                      decoration: BoxDecoration(
                        color: AppColors.backgroundMedium,
                        borderRadius: BorderRadius.circular(12.r),
                        border: Border.all(
                          color: AppColors.textMedium.withOpacity(0.3),
                        ),
                      ),
                      child: Center(
                        child: Text(
                          'Cancel',
                          style: TextStyle(
                            fontSize: 16.sp,
                            fontWeight: FontWeight.w600,
                            color: AppColors.textMedium,
                          ),
                        ),
                      ),
                    ),
                  ),
                ),
                SizedBox(width: 12.w),
                Expanded(
                  child: GestureDetector(
                    onTap: _isLoading ? null : _saveCategory,
                    child: Container(
                      height: 48.h,
                      decoration: BoxDecoration(
                        color: _selectedColor,
                        borderRadius: BorderRadius.circular(12.r),
                      ),
                      child: Center(
                        child: _isLoading
                            ? SizedBox(
                                width: 20.w,
                                height: 20.h,
                                child: CircularProgressIndicator(
                                  strokeWidth: 2,
                                  valueColor: AlwaysStoppedAnimation<Color>(
                                    AppColors.white,
                                  ),
                                ),
                              )
                            : Text(
                                isEditing ? 'Update' : 'Create Category',
                                style: TextStyle(
                                  fontSize: 16.sp,
                                  fontWeight: FontWeight.w600,
                                  color: AppColors.white,
                                ),
                              ),
                      ),
                    ),
                  ),
                ),
              ],
            ),
          ],
        ),
      ),
    );
  }

  void _saveCategory() async {
    if (!_formKey.currentState!.validate()) return;
    
    setState(() {
      _isLoading = true;
    });

    final categoryName = _nameController.text.trim();
    
    final category = AffirmationCategory(
      id: widget.categoryToEdit?.id ?? const Uuid().v4(),
      name: categoryName,
      color: _selectedColor,
      affirmations: widget.categoryToEdit?.affirmations ?? [],
      isCustom: true,
      createdAt: widget.categoryToEdit?.createdAt ?? DateTime.now(),
    );

    Navigator.pop(context, category);
  }
}