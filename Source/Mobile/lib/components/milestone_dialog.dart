import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../models/api_milestone.dart';
import '../constants/colors.dart';
import '../utils/milestone_converter.dart';
import '../widgets/stylish_loading.dart';

class MilestoneDialog extends StatefulWidget {
  final String goalTrackerId;
  final ApiMilestone? milestone; // null for creating new milestone
  final Function(ApiMilestone) onSave;
  final int? userId;

  const MilestoneDialog({
    Key? key,
    required this.goalTrackerId,
    this.milestone,
    required this.onSave,
    this.userId,
  }) : super(key: key);

  @override
  _MilestoneDialogState createState() => _MilestoneDialogState();
}

class _MilestoneDialogState extends State<MilestoneDialog> {
  late TextEditingController _titleController;
  late TextEditingController _descriptionController;
  late double _progressPercentage;
  DateTime? _targetDate;
  bool _isCompleted = false;
  bool _isSaving = false;

  @override
  void initState() {
    super.initState();
    _titleController = TextEditingController(text: widget.milestone?.title ?? '');
    _descriptionController = TextEditingController(text: widget.milestone?.description ?? '');
    _progressPercentage = widget.milestone?.progressPercentage ?? 0.0;
    _targetDate = widget.milestone?.targetDate;
    _isCompleted = widget.milestone?.isCompleted ?? false;
  }

  @override
  void dispose() {
    _titleController.dispose();
    _descriptionController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final isEditing = widget.milestone != null;
    
    return Dialog(
      backgroundColor: Colors.transparent,
      child: Container(
        width: double.infinity,
        constraints: BoxConstraints(maxHeight: 600.h),
        decoration: BoxDecoration(
          color: AppColors.cardBackground,
          borderRadius: BorderRadius.circular(20.r),
        ),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            // Header
            Container(
              padding: EdgeInsets.all(20.w),
              decoration: BoxDecoration(
                gradient: LinearGradient(
                  colors: AppColors.goalTrackerGradient,
                  begin: Alignment.topLeft,
                  end: Alignment.bottomRight,
                ),
                borderRadius: BorderRadius.only(
                  topLeft: Radius.circular(20.r),
                  topRight: Radius.circular(20.r),
                ),
              ),
              child: Row(
                children: [
                  Icon(
                    isEditing ? Icons.edit : Icons.add_task,
                    color: AppColors.white,
                    size: 24.sp,
                  ),
                  SizedBox(width: 12.w),
                  Expanded(
                    child: Text(
                      isEditing ? 'Edit Milestone' : 'Create Milestone',
                      style: TextStyle(
                        fontSize: 18.sp,
                        fontWeight: FontWeight.bold,
                        color: AppColors.white,
                      ),
                    ),
                  ),
                  GestureDetector(
                    onTap: () => Navigator.pop(context),
                    child: Icon(
                      Icons.close,
                      color: AppColors.white,
                      size: 24.sp,
                    ),
                  ),
                ],
              ),
            ),
            
            // Form content
            Flexible(
              child: SingleChildScrollView(
                padding: EdgeInsets.all(20.w),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    // Title field
                    _buildLabel('Milestone Title'),
                    SizedBox(height: 8.h),
                    _buildTextField(
                      controller: _titleController,
                      hintText: 'Enter milestone title...',
                      maxLines: 1,
                    ),
                    
                    SizedBox(height: 20.h),
                    
                    // Description field
                    _buildLabel('Description (Optional)'),
                    SizedBox(height: 8.h),
                    _buildTextField(
                      controller: _descriptionController,
                      hintText: 'Add details about this milestone...',
                      maxLines: 3,
                    ),
                    
                    SizedBox(height: 20.h),
                    
                    // Target date
                    _buildLabel('Target Date (Optional)'),
                    SizedBox(height: 8.h),
                    GestureDetector(
                      onTap: _selectTargetDate,
                      child: Container(
                        width: double.infinity,
                        padding: EdgeInsets.symmetric(horizontal: 16.w, vertical: 14.h),
                        decoration: BoxDecoration(
                          color: AppColors.backgroundDark,
                          borderRadius: BorderRadius.circular(12.r),
                          border: Border.all(
                            color: AppColors.textMedium.withOpacity(0.3),
                          ),
                        ),
                        child: Row(
                          children: [
                            Icon(
                              Icons.calendar_today,
                              color: AppColors.textMedium,
                              size: 18.sp,
                            ),
                            SizedBox(width: 12.w),
                            Expanded(
                              child: Text(
                                _targetDate != null
                                    ? _formatDate(_targetDate!)
                                    : 'Select target date',
                                style: TextStyle(
                                  fontSize: 14.sp,
                                  color: _targetDate != null
                                      ? AppColors.textLight
                                      : AppColors.textMedium,
                                ),
                              ),
                            ),
                            if (_targetDate != null)
                              GestureDetector(
                                onTap: () => setState(() => _targetDate = null),
                                child: Icon(
                                  Icons.clear,
                                  color: AppColors.textMedium,
                                  size: 18.sp,
                                ),
                              ),
                          ],
                        ),
                      ),
                    ),
                    
                    SizedBox(height: 20.h),
                    
                    // Progress section
                    if (isEditing) ...[
                      _buildLabel('Progress'),
                      SizedBox(height: 12.h),
                      
                      // Completion toggle
                      Row(
                        children: [
                          GestureDetector(
                            onTap: () {
                              setState(() {
                                _isCompleted = !_isCompleted;
                                if (_isCompleted) {
                                  _progressPercentage = 100.0;
                                } else if (_progressPercentage == 100.0) {
                                  _progressPercentage = 0.0;
                                }
                              });
                            },
                            child: Container(
                              width: 24.w,
                              height: 24.h,
                              decoration: BoxDecoration(
                                color: _isCompleted 
                                    ? AppColors.accentMint
                                    : Colors.transparent,
                                border: Border.all(
                                  color: _isCompleted 
                                      ? AppColors.accentMint
                                      : AppColors.textMedium.withOpacity(0.3),
                                  width: 2,
                                ),
                                borderRadius: BorderRadius.circular(4.r),
                              ),
                              child: _isCompleted
                                  ? Icon(
                                      Icons.check,
                                      color: AppColors.white,
                                      size: 16.sp,
                                    )
                                  : null,
                            ),
                          ),
                          SizedBox(width: 12.w),
                          Text(
                            'Mark as completed',
                            style: TextStyle(
                              fontSize: 14.sp,
                              color: AppColors.textLight,
                              fontWeight: FontWeight.w500,
                            ),
                          ),
                        ],
                      ),
                      
                      if (!_isCompleted) ...[
                        SizedBox(height: 16.h),
                        
                        // Progress slider
                        Row(
                          children: [
                            Text(
                              'Progress: ${_progressPercentage.round()}%',
                              style: TextStyle(
                                fontSize: 14.sp,
                                color: AppColors.textLight,
                                fontWeight: FontWeight.w500,
                              ),
                            ),
                          ],
                        ),
                        
                        SizedBox(height: 8.h),
                        
                        SliderTheme(
                          data: SliderTheme.of(context).copyWith(
                            activeTrackColor: AppColors.goalTrackerGradient[0],
                            inactiveTrackColor: AppColors.textMedium.withOpacity(0.2),
                            thumbColor: AppColors.goalTrackerGradient[0],
                            overlayColor: AppColors.goalTrackerGradient[0].withOpacity(0.2),
                            trackHeight: 6.h,
                          ),
                          child: Slider(
                            value: _progressPercentage,
                            min: 0.0,
                            max: 100.0,
                            divisions: 20,
                            onChanged: (value) {
                              setState(() {
                                _progressPercentage = value;
                                _isCompleted = value >= 100.0;
                              });
                            },
                          ),
                        ),
                      ],
                    ],
                  ],
                ),
              ),
            ),
            
            // Action buttons
            Container(
              padding: EdgeInsets.all(20.w),
              child: Row(
                children: [
                  Expanded(
                    child: TextButton(
                      onPressed: _isSaving ? null : () => Navigator.pop(context),
                      style: TextButton.styleFrom(
                        padding: EdgeInsets.symmetric(vertical: 14.h),
                        backgroundColor: AppColors.textMedium.withOpacity(0.1),
                        shape: RoundedRectangleBorder(
                          borderRadius: BorderRadius.circular(12.r),
                        ),
                      ),
                      child: Text(
                        'Cancel',
                        style: TextStyle(
                          fontSize: 14.sp,
                          fontWeight: FontWeight.w600,
                          color: AppColors.textMedium,
                        ),
                      ),
                    ),
                  ),
                  
                  SizedBox(width: 12.w),
                  
                  Expanded(
                    flex: 2,
                    child: ElevatedButton(
                      onPressed: _isSaving ? null : _saveMilestone,
                      style: ElevatedButton.styleFrom(
                        padding: EdgeInsets.symmetric(vertical: 14.h),
                        backgroundColor: AppColors.goalTrackerGradient[0],
                        shape: RoundedRectangleBorder(
                          borderRadius: BorderRadius.circular(12.r),
                        ),
                        elevation: 0,
                      ),
                      child: _isSaving
                          ? CompactStylishLoading(
                              color: AppColors.white,
                              size: 20.w,
                            )
                          : Text(
                              isEditing ? 'Update' : 'Create',
                              style: TextStyle(
                                fontSize: 14.sp,
                                fontWeight: FontWeight.w600,
                                color: AppColors.white,
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
    );
  }

  Widget _buildLabel(String text) {
    return Text(
      text,
      style: TextStyle(
        fontSize: 14.sp,
        fontWeight: FontWeight.w600,
        color: AppColors.textLight,
      ),
    );
  }

  Widget _buildTextField({
    required TextEditingController controller,
    required String hintText,
    int maxLines = 1,
  }) {
    return TextField(
      controller: controller,
      maxLines: maxLines,
      style: TextStyle(
        fontSize: 14.sp,
        color: AppColors.textLight,
      ),
      decoration: InputDecoration(
        hintText: hintText,
        hintStyle: TextStyle(
          fontSize: 14.sp,
          color: AppColors.textMedium,
        ),
        filled: true,
        fillColor: AppColors.backgroundDark,
        border: OutlineInputBorder(
          borderRadius: BorderRadius.circular(12.r),
          borderSide: BorderSide(
            color: AppColors.textMedium.withOpacity(0.3),
          ),
        ),
        enabledBorder: OutlineInputBorder(
          borderRadius: BorderRadius.circular(12.r),
          borderSide: BorderSide(
            color: AppColors.textMedium.withOpacity(0.3),
          ),
        ),
        focusedBorder: OutlineInputBorder(
          borderRadius: BorderRadius.circular(12.r),
          borderSide: BorderSide(
            color: AppColors.goalTrackerGradient[0],
            width: 2,
          ),
        ),
        contentPadding: EdgeInsets.symmetric(
          horizontal: 16.w,
          vertical: maxLines > 1 ? 16.h : 14.h,
        ),
      ),
    );
  }

  void _selectTargetDate() async {
    final date = await showDatePicker(
      context: context,
      initialDate: _targetDate ?? DateTime.now(),
      firstDate: DateTime.now(),
      lastDate: DateTime.now().add(Duration(days: 365 * 2)),
      builder: (context, child) {
        return Theme(
          data: Theme.of(context).copyWith(
            colorScheme: ColorScheme.dark(
              primary: AppColors.goalTrackerGradient[0],
              surface: AppColors.cardBackground,
            ),
          ),
          child: child!,
        );
      },
    );
    
    if (date != null) {
      setState(() => _targetDate = date);
    }
  }

  void _saveMilestone() async {
    if (_titleController.text.trim().isEmpty) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Please enter a milestone title'),
          backgroundColor: Colors.red,
        ),
      );
      return;
    }

    setState(() => _isSaving = true);

    try {
      ApiMilestone milestone;
      
      if (widget.milestone != null) {
        // Update existing milestone
        milestone = MilestoneConverter.updateExisting(
          widget.milestone!,
          title: _titleController.text.trim(),
          description: _descriptionController.text.trim(),
          isCompleted: _isCompleted,
          progressPercentage: _progressPercentage,
          targetDate: _targetDate,
          completedDate: _isCompleted ? DateTime.now() : null,
        );
      } else {
        // Create new milestone
        milestone = MilestoneConverter.createNew(
          goalTrackerId: widget.goalTrackerId,
          title: _titleController.text.trim(),
          description: _descriptionController.text.trim(),
          targetDate: _targetDate,
          userId: widget.userId,
        );
      }

      widget.onSave(milestone);
      Navigator.pop(context);
    } catch (e) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Error saving milestone: $e'),
          backgroundColor: Colors.red,
        ),
      );
    } finally {
      setState(() => _isSaving = false);
    }
  }

  String _formatDate(DateTime date) {
    final months = [
      'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
      'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'
    ];
    
    return '${months[date.month - 1]} ${date.day}, ${date.year}';
  }
}