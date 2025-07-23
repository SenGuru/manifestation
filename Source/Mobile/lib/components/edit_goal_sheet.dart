import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:fun/constants/colors.dart';
import 'package:fun/models/goals.dart';

class EditGoalSheet extends StatefulWidget {
  final Goal goal;
  final Function(Goal) onGoalUpdated;

  const EditGoalSheet({
    Key? key,
    required this.goal,
    required this.onGoalUpdated,
  }) : super(key: key);

  @override
  _EditGoalSheetState createState() => _EditGoalSheetState();
}

class _EditGoalSheetState extends State<EditGoalSheet> {
  final _formKey = GlobalKey<FormState>();
  late final TextEditingController _titleController;
  late final TextEditingController _descriptionController;
  
  late GoalCategory selectedCategory;
  late GoalPriority selectedPriority;
  late DateTime selectedDate;
  late List<String> milestones;

  @override
  void initState() {
    super.initState();
    
    // Initialize with existing goal data
    _titleController = TextEditingController(text: widget.goal.title);
    _descriptionController = TextEditingController(text: widget.goal.description);
    selectedCategory = widget.goal.category;
    selectedPriority = widget.goal.priority;
    selectedDate = widget.goal.targetDate;
    
    // Convert milestones to strings for editing
    milestones = List.generate(4, (index) {
      if (index < widget.goal.milestones.length) {
        return widget.goal.milestones[index].title;
      }
      return '';
    });
  }

  @override
  void dispose() {
    _titleController.dispose();
    _descriptionController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return DraggableScrollableSheet(
      initialChildSize: 0.85,
      maxChildSize: 0.95,
      minChildSize: 0.5,
      builder: (context, scrollController) {
        return Container(
          decoration: BoxDecoration(
            color: AppColors.cardBackground,
            borderRadius: BorderRadius.vertical(top: Radius.circular(20.r)),
          ),
          child: Column(
            children: [
              // Handle
              Container(
                width: 40.w,
                height: 4.h,
                margin: EdgeInsets.only(top: 12.h),
                decoration: BoxDecoration(
                  color: AppColors.textMedium.withOpacity(0.3),
                  borderRadius: BorderRadius.circular(2.r),
                ),
              ),
              
              // Header
              Padding(
                padding: EdgeInsets.all(20.w),
                child: Row(
                  children: [
                    Container(
                      width: 44.w,
                      height: 44.h,
                      decoration: BoxDecoration(
                        color: selectedCategory.color.withOpacity(0.1),
                        borderRadius: BorderRadius.circular(12.r),
                      ),
                      child: Icon(
                        selectedCategory.icon,
                        color: selectedCategory.color,
                        size: 20.sp,
                      ),
                    ),
                    SizedBox(width: 16.w),
                    Expanded(
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          Text(
                            'Edit Goal',
                            style: TextStyle(
                              fontSize: 24.sp,
                              fontWeight: FontWeight.bold,
                              color: AppColors.textLight,
                            ),
                          ),
                          Text(
                            'Update your goal details',
                            style: TextStyle(
                              fontSize: 14.sp,
                              color: AppColors.textMedium,
                            ),
                          ),
                        ],
                      ),
                    ),
                    GestureDetector(
                      onTap: () => Navigator.pop(context),
                      child: Container(
                        width: 32.w,
                        height: 32.h,
                        decoration: BoxDecoration(
                          color: AppColors.textMedium.withOpacity(0.1),
                          borderRadius: BorderRadius.circular(8.r),
                        ),
                        child: Icon(
                          Icons.close,
                          color: AppColors.textMedium,
                          size: 20.sp,
                        ),
                      ),
                    ),
                  ],
                ),
              ),
              
              // Form content
              Expanded(
                child: SingleChildScrollView(
                  controller: scrollController,
                  padding: EdgeInsets.fromLTRB(20.w, 0, 20.w, 20.h),
                  child: Form(
                    key: _formKey,
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        // Title field
                        _buildFormField(
                          label: 'Goal Title',
                          controller: _titleController,
                          validator: (value) {
                            if (value == null || value.trim().isEmpty) {
                              return 'Please enter a goal title';
                            }
                            return null;
                          },
                        ),
                        
                        SizedBox(height: 20.h),
                        
                        // Description field
                        _buildFormField(
                          label: 'Description',
                          controller: _descriptionController,
                          hint: 'Describe your goal in detail...',
                          maxLines: 3,
                        ),
                        
                        SizedBox(height: 24.h),
                        
                        // Category selection
                        Text(
                          'Category',
                          style: TextStyle(
                            fontSize: 16.sp,
                            fontWeight: FontWeight.w600,
                            color: AppColors.textLight,
                          ),
                        ),
                        SizedBox(height: 12.h),
                        Wrap(
                          spacing: 8.w,
                          runSpacing: 8.h,
                          children: GoalCategory.values.where((c) => c != GoalCategory.all).map((category) {
                            final isSelected = selectedCategory == category;
                            return GestureDetector(
                              onTap: () {
                                setState(() {
                                  selectedCategory = category;
                                });
                              },
                              child: Container(
                                padding: EdgeInsets.symmetric(horizontal: 16.w, vertical: 8.h),
                                decoration: BoxDecoration(
                                  color: isSelected 
                                      ? category.color.withOpacity(0.1)
                                      : AppColors.backgroundMedium,
                                  borderRadius: BorderRadius.circular(20.r),
                                  border: Border.all(
                                    color: isSelected 
                                        ? category.color
                                        : AppColors.textMedium.withOpacity(0.3),
                                  ),
                                ),
                                child: Row(
                                  mainAxisSize: MainAxisSize.min,
                                  children: [
                                    Icon(
                                      category.icon,
                                      color: isSelected ? category.color : AppColors.textMedium,
                                      size: 16.sp,
                                    ),
                                    SizedBox(width: 6.w),
                                    Text(
                                      category.displayName,
                                      style: TextStyle(
                                        fontSize: 14.sp,
                                        fontWeight: isSelected ? FontWeight.w600 : FontWeight.w500,
                                        color: isSelected ? category.color : AppColors.textMedium,
                                      ),
                                    ),
                                  ],
                                ),
                              ),
                            );
                          }).toList(),
                        ),
                        
                        SizedBox(height: 24.h),
                        
                        // Priority selection
                        Row(
                          children: [
                            Expanded(
                              child: Column(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: [
                                  Text(
                                    'Priority',
                                    style: TextStyle(
                                      fontSize: 16.sp,
                                      fontWeight: FontWeight.w600,
                                      color: AppColors.textLight,
                                    ),
                                  ),
                                  SizedBox(height: 8.h),
                                  Container(
                                    padding: EdgeInsets.symmetric(horizontal: 16.w),
                                    decoration: BoxDecoration(
                                      color: AppColors.backgroundMedium,
                                      borderRadius: BorderRadius.circular(12.r),
                                      border: Border.all(
                                        color: AppColors.textMedium.withOpacity(0.3),
                                      ),
                                    ),
                                    child: DropdownButton<GoalPriority>(
                                      value: selectedPriority,
                                      isExpanded: true,
                                      underline: SizedBox.shrink(),
                                      dropdownColor: AppColors.cardBackground,
                                      style: TextStyle(color: AppColors.textLight),
                                      onChanged: (GoalPriority? newValue) {
                                        if (newValue != null) {
                                          setState(() {
                                            selectedPriority = newValue;
                                          });
                                        }
                                      },
                                      items: GoalPriority.values.map((priority) {
                                        return DropdownMenuItem<GoalPriority>(
                                          value: priority,
                                          child: Row(
                                            children: [
                                              Icon(
                                                priority.icon,
                                                color: priority.color,
                                                size: 16.sp,
                                              ),
                                              SizedBox(width: 8.w),
                                              Text(
                                                priority.displayName,
                                                style: TextStyle(
                                                  color: priority.color,
                                                  fontWeight: FontWeight.w500,
                                                ),
                                              ),
                                            ],
                                          ),
                                        );
                                      }).toList(),
                                    ),
                                  ),
                                ],
                              ),
                            ),
                            SizedBox(width: 16.w),
                            Expanded(
                              child: Column(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: [
                                  Text(
                                    'Target Date',
                                    style: TextStyle(
                                      fontSize: 16.sp,
                                      fontWeight: FontWeight.w600,
                                      color: AppColors.textLight,
                                    ),
                                  ),
                                  SizedBox(height: 8.h),
                                  GestureDetector(
                                    onTap: () async {
                                      final date = await showDatePicker(
                                        context: context,
                                        initialDate: selectedDate,
                                        firstDate: DateTime.now(),
                                        lastDate: DateTime.now().add(Duration(days: 365 * 5)),
                                        builder: (context, child) {
                                          return Theme(
                                            data: Theme.of(context).copyWith(
                                              colorScheme: ColorScheme.dark(
                                                primary: selectedCategory.color,
                                                surface: AppColors.cardBackground,
                                              ),
                                            ),
                                            child: child!,
                                          );
                                        },
                                      );
                                      if (date != null) {
                                        setState(() {
                                          selectedDate = date;
                                        });
                                      }
                                    },
                                    child: Container(
                                      height: 48.h,
                                      padding: EdgeInsets.symmetric(horizontal: 16.w),
                                      decoration: BoxDecoration(
                                        color: AppColors.backgroundMedium,
                                        borderRadius: BorderRadius.circular(12.r),
                                        border: Border.all(
                                          color: AppColors.textMedium.withOpacity(0.3),
                                        ),
                                      ),
                                      child: Row(
                                        children: [
                                          Icon(
                                            Icons.calendar_today,
                                            color: selectedCategory.color,
                                            size: 18.sp,
                                          ),
                                          SizedBox(width: 12.w),
                                          Text(
                                            '${selectedDate.day}/${selectedDate.month}/${selectedDate.year}',
                                            style: TextStyle(
                                              fontSize: 14.sp,
                                              color: AppColors.textLight,
                                              fontWeight: FontWeight.w500,
                                            ),
                                          ),
                                        ],
                                      ),
                                    ),
                                  ),
                                ],
                              ),
                            ),
                          ],
                        ),
                        
                        SizedBox(height: 24.h),
                        
                        // Milestones
                        Text(
                          'Milestones (Optional)',
                          style: TextStyle(
                            fontSize: 16.sp,
                            fontWeight: FontWeight.w600,
                            color: AppColors.textLight,
                          ),
                        ),
                        SizedBox(height: 8.h),
                        Text(
                          'Break your goal into smaller, achievable steps',
                          style: TextStyle(
                            fontSize: 14.sp,
                            color: AppColors.textMedium,
                          ),
                        ),
                        SizedBox(height: 12.h),
                        
                        ...List.generate(4, (index) {
                          return Container(
                            margin: EdgeInsets.only(bottom: 12.h),
                            child: Row(
                              children: [
                                Container(
                                  width: 24.w,
                                  height: 24.h,
                                  decoration: BoxDecoration(
                                    color: selectedCategory.color.withOpacity(0.1),
                                    borderRadius: BorderRadius.circular(6.r),
                                  ),
                                  child: Center(
                                    child: Text(
                                      '${index + 1}',
                                      style: TextStyle(
                                        fontSize: 12.sp,
                                        fontWeight: FontWeight.w600,
                                        color: selectedCategory.color,
                                      ),
                                    ),
                                  ),
                                ),
                                SizedBox(width: 12.w),
                                Expanded(
                                  child: TextFormField(
                                    initialValue: milestones[index],
                                    decoration: InputDecoration(
                                      hintText: 'Milestone ${index + 1}',
                                      hintStyle: TextStyle(
                                        color: AppColors.textMedium.withOpacity(0.6),
                                      ),
                                      filled: true,
                                      fillColor: AppColors.backgroundMedium,
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
                                          color: selectedCategory.color,
                                        ),
                                      ),
                                      contentPadding: EdgeInsets.symmetric(
                                        horizontal: 16.w,
                                        vertical: 12.h,
                                      ),
                                    ),
                                    style: TextStyle(
                                      color: AppColors.textLight,
                                      fontSize: 14.sp,
                                    ),
                                    onChanged: (value) {
                                      milestones[index] = value;
                                    },
                                  ),
                                ),
                              ],
                            ),
                          );
                        }),
                        
                        SizedBox(height: 24.h),
                        
                        // Current Progress Display
                        Container(
                          padding: EdgeInsets.all(16.w),
                          decoration: BoxDecoration(
                            color: selectedCategory.color.withOpacity(0.1),
                            borderRadius: BorderRadius.circular(12.r),
                            border: Border.all(
                              color: selectedCategory.color.withOpacity(0.3),
                            ),
                          ),
                          child: Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              Row(
                                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                children: [
                                  Text(
                                    'Current Progress',
                                    style: TextStyle(
                                      fontSize: 14.sp,
                                      fontWeight: FontWeight.w600,
                                      color: AppColors.textLight,
                                    ),
                                  ),
                                  Text(
                                    '${widget.goal.currentProgress.round()}%',
                                    style: TextStyle(
                                      fontSize: 18.sp,
                                      fontWeight: FontWeight.bold,
                                      color: selectedCategory.color,
                                    ),
                                  ),
                                ],
                              ),
                              SizedBox(height: 8.h),
                              Container(
                                height: 6.h,
                                decoration: BoxDecoration(
                                  color: AppColors.textMedium.withOpacity(0.2),
                                  borderRadius: BorderRadius.circular(3.r),
                                ),
                                child: FractionallySizedBox(
                                  alignment: Alignment.centerLeft,
                                  widthFactor: widget.goal.currentProgress / 100,
                                  child: Container(
                                    decoration: BoxDecoration(
                                      color: selectedCategory.color,
                                      borderRadius: BorderRadius.circular(3.r),
                                    ),
                                  ),
                                ),
                              ),
                              SizedBox(height: 8.h),
                              Text(
                                'Note: To update progress, use the progress button on the goal card',
                                style: TextStyle(
                                  fontSize: 12.sp,
                                  color: AppColors.textMedium,
                                  fontStyle: FontStyle.italic,
                                ),
                              ),
                            ],
                          ),
                        ),
                        
                        SizedBox(height: 32.h),
                        
                        // Action buttons
                        Row(
                          children: [
                            Expanded(
                              child: GestureDetector(
                                onTap: () => Navigator.pop(context),
                                child: Container(
                                  height: 50.h,
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
                            SizedBox(width: 16.w),
                            Expanded(
                              flex: 2,
                              child: GestureDetector(
                                onTap: _updateGoal,
                                child: Container(
                                  height: 50.h,
                                  decoration: BoxDecoration(
                                    gradient: LinearGradient(
                                      colors: AppColors.goalTrackerGradient,
                                    ),
                                    borderRadius: BorderRadius.circular(12.r),
                                  ),
                                  child: Center(
                                    child: Text(
                                      'Update Goal',
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
                ),
              ),
            ],
          ),
        );
      },
    );
  }

  Widget _buildFormField({
    required String label,
    required TextEditingController controller,
    String? hint,
    int maxLines = 1,
    String? Function(String?)? validator,
  }) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          label,
          style: TextStyle(
            fontSize: 16.sp,
            fontWeight: FontWeight.w600,
            color: AppColors.textLight,
          ),
        ),
        SizedBox(height: 8.h),
        TextFormField(
          controller: controller,
          validator: validator,
          maxLines: maxLines,
          decoration: InputDecoration(
            hintText: hint,
            hintStyle: TextStyle(
              color: AppColors.textMedium.withOpacity(0.6),
            ),
            filled: true,
            fillColor: AppColors.backgroundMedium,
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
                color: selectedCategory.color,
              ),
            ),
            errorBorder: OutlineInputBorder(
              borderRadius: BorderRadius.circular(12.r),
              borderSide: BorderSide(
                color: Colors.red,
              ),
            ),
            contentPadding: EdgeInsets.symmetric(
              horizontal: 16.w,
              vertical: 12.h,
            ),
          ),
          style: TextStyle(
            color: AppColors.textLight,
            fontSize: 16.sp,
          ),
        ),
      ],
    );
  }

  void _updateGoal() {
    if (_formKey.currentState!.validate()) {
      // Create milestone objects from strings
      final List<Milestone> milestoneObjects = [];
      for (int i = 0; i < milestones.length; i++) {
        if (milestones[i].trim().isNotEmpty) {
          // Check if this milestone existed before
          Milestone milestone;
          if (i < widget.goal.milestones.length) {
            // Update existing milestone
            milestone = widget.goal.milestones[i].copyWith(
              title: milestones[i].trim(),
            );
          } else {
            // Create new milestone
            milestone = Milestone(
              id: DateTime.now().millisecondsSinceEpoch.toString() + '_$i',
              title: milestones[i].trim(),
              description: '',
              isCompleted: false,
              targetDate: selectedDate,
            );
          }
          milestoneObjects.add(milestone);
        }
      }

      final updatedGoal = Goal(
        id: widget.goal.id,
        title: _titleController.text.trim(),
        description: _descriptionController.text.trim(),
        category: selectedCategory,
        priority: selectedPriority,
        targetDate: selectedDate,
        createdAt: widget.goal.createdAt,
        milestones: milestoneObjects,
        currentProgress: widget.goal.currentProgress, // Keep current progress
        tags: widget.goal.tags,
      );

      widget.onGoalUpdated(updatedGoal);
      Navigator.pop(context);
    }
  }
}