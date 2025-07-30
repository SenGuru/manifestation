import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../constants/app_theme.dart';
import 'package:fun/models/affirmations.dart';
import 'package:fun/widgets/stylish_loading.dart';
import 'package:uuid/uuid.dart';

class AddAffirmationDialog extends StatefulWidget {
  final String categoryId;
  final Affirmation? affirmationToEdit;

  const AddAffirmationDialog({
    Key? key,
    required this.categoryId,
    this.affirmationToEdit,
  }) : super(key: key);

  @override
  _AddAffirmationDialogState createState() => _AddAffirmationDialogState();
}

class _AddAffirmationDialogState extends State<AddAffirmationDialog> {
  late TextEditingController _textController;
  final _formKey = GlobalKey<FormState>();
  bool _isLoading = false;

  @override
  void initState() {
    super.initState();
    _textController = TextEditingController(
      text: widget.affirmationToEdit?.text ?? '',
    );
  }

  @override
  void dispose() {
    _textController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final isEditing = widget.affirmationToEdit != null;
    
    return Dialog(
      backgroundColor: AppTheme.cardBackground,
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
                    color: AppTheme.accentGold.withOpacity(0.2),
                    borderRadius: BorderRadius.circular(10.r),
                  ),
                  child: Icon(
                    isEditing ? Icons.edit : Icons.add,
                    color: AppTheme.accentGold,
                    size: 20.sp,
                  ),
                ),
                SizedBox(width: 12.w),
                Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(
                        isEditing ? 'Edit Affirmation' : 'New Affirmation',
                        style: TextStyle(
                          fontSize: 18.sp,
                          fontWeight: FontWeight.bold,
                          color: AppTheme.textLight,
                        ),
                      ),
                      Text(
                        'Create your personal manifestation',
                        style: TextStyle(
                          fontSize: 12.sp,
                          color: AppTheme.textMedium,
                        ),
                      ),
                    ],
                  ),
                ),
                GestureDetector(
                  onTap: () => Navigator.pop(context),
                  child: Icon(
                    Icons.close,
                    color: AppTheme.textMedium,
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
                    'Affirmation Text',
                    style: TextStyle(
                      fontSize: 14.sp,
                      fontWeight: FontWeight.w600,
                      color: AppTheme.textLight,
                    ),
                  ),
                  SizedBox(height: 8.h),
                  Container(
                    decoration: BoxDecoration(
                      color: AppTheme.backgroundMedium,
                      borderRadius: BorderRadius.circular(12.r),
                      border: Border.all(
                        color: AppTheme.textMedium.withOpacity(0.2),
                      ),
                    ),
                    child: TextFormField(
                      controller: _textController,
                      maxLines: 4,
                      style: TextStyle(
                        fontSize: 16.sp,
                        color: AppTheme.textLight,
                        height: 1.4,
                      ),
                      decoration: InputDecoration(
                        hintText: 'Enter your positive affirmation...\ne.g., "I am confident and capable of achieving my dreams"',
                        hintStyle: TextStyle(
                          color: AppTheme.textMedium.withOpacity(0.7),
                          fontSize: 14.sp,
                        ),
                        border: InputBorder.none,
                        contentPadding: EdgeInsets.all(16.w),
                      ),
                      validator: (value) {
                        if (value == null || value.trim().isEmpty) {
                          return 'Please enter an affirmation';
                        }
                        if (value.trim().length < 10) {
                          return 'Affirmation should be at least 10 characters';
                        }
                        return null;
                      },
                    ),
                  ),
                  
                  SizedBox(height: 16.h),
                  
                  // Tips
                  Container(
                    padding: EdgeInsets.all(12.w),
                    decoration: BoxDecoration(
                      color: AppTheme.accentGold.withOpacity(0.1),
                      borderRadius: BorderRadius.circular(8.r),
                      border: Border.all(
                        color: AppTheme.accentGold.withOpacity(0.3),
                      ),
                    ),
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Row(
                          children: [
                            Icon(
                              Icons.lightbulb_outline,
                              color: AppTheme.accentGold,
                              size: 16.sp,
                            ),
                            SizedBox(width: 6.w),
                            Text(
                              'Tips for effective affirmations:',
                              style: TextStyle(
                                fontSize: 12.sp,
                                fontWeight: FontWeight.w600,
                                color: AppTheme.accentGold,
                              ),
                            ),
                          ],
                        ),
                        SizedBox(height: 6.h),
                        Text(
                          '• Use present tense ("I am" not "I will be")\n'
                          '• Keep it positive and specific\n'
                          '• Make it personal and meaningful to you',
                          style: TextStyle(
                            fontSize: 11.sp,
                            color: AppTheme.textMedium,
                            height: 1.3,
                          ),
                        ),
                      ],
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
                        color: AppTheme.backgroundMedium,
                        borderRadius: BorderRadius.circular(12.r),
                        border: Border.all(
                          color: AppTheme.textMedium.withOpacity(0.3),
                        ),
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
                SizedBox(width: 12.w),
                Expanded(
                  child: GestureDetector(
                    onTap: _isLoading ? null : _saveAffirmation,
                    child: Container(
                      height: 48.h,
                      decoration: BoxDecoration(
                        gradient: LinearGradient(
                          colors: [
                            AppTheme.accentGold,
                            AppTheme.accentGold.withOpacity(0.8),
                          ],
                        ),
                        borderRadius: BorderRadius.circular(12.r),
                      ),
                      child: Center(
                        child: _isLoading
                            ? CompactStylishLoading(
                                color: AppTheme.white,
                                size: 20.w,
                              )
                            : Text(
                                isEditing ? 'Update' : 'Add Affirmation',
                                style: TextStyle(
                                  fontSize: 16.sp,
                                  fontWeight: FontWeight.w600,
                                  color: AppTheme.white,
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

  void _saveAffirmation() async {
    if (!_formKey.currentState!.validate()) return;
    
    setState(() {
      _isLoading = true;
    });

    final affirmationText = _textController.text.trim();
    
    final affirmation = Affirmation(
      id: widget.affirmationToEdit?.id ?? const Uuid().v4(),
      text: affirmationText,
      isCustom: true,
      createdAt: widget.affirmationToEdit?.createdAt ?? DateTime.now(),
      isFavorite: widget.affirmationToEdit?.isFavorite ?? false,
    );

    Navigator.pop(context, affirmation);
  }
}