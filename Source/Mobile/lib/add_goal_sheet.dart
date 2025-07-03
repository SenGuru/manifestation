import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:manifestation_app/goals.dart';
import '../colors.dart';

class AddGoalSheet extends StatefulWidget {
  final Function(Goal) onGoalAdded;

  const AddGoalSheet({
    Key? key,
    required this.onGoalAdded,
  }) : super(key: key);

  @override
  _AddGoalSheetState createState() => _AddGoalSheetState();
}

class _AddGoalSheetState extends State<AddGoalSheet> {
  final _formKey = GlobalKey<FormState>();
  final _titleController = TextEditingController();
  final _descriptionController = TextEditingController();
  
  GoalCategory selectedCategory = GoalCategory.career;
  GoalPriority selectedPriority = GoalPriority.medium;
  DateTime selectedDate = DateTime.now().add(Duration(days: 90));
  List<String> milestones = ['', '', '', ''];

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
                    Text(
                      'Create New Goal',
                      style: TextStyle(
                        fontSize: 20.sp,
                        fontWeight: FontWeight.bold,
                        color: AppColors.textLight,
                      ),
                    ),
                    Spacer(),
                    GestureDetector(
                      onTap: () => Navigator.pop(context),
                      child: Icon(
                        Icons.close,
                        color: AppColors.textMedium,
                        size: 24.sp,
                      ),
                    ),
                  ],
                ),
              ),
              
              // Form content
              Expanded(
                child: Form(
                  key: _formKey,
                  child: ListView(
                    controller: scrollController,
                    padding: EdgeInsets.symmetric(horizontal: 20.w),
                    children: [
                      // Title
                      _buildTextField(
                        controller: _titleController,
                        label: 'Goal Title',
                        hint: 'What do you want to achieve?',
                        validator: (value) {
                          if (value == null || value.isEmpty) {
                            return 'Please enter a goal title';
                          }
                          return null;
                        },
                      ),
                      
                      SizedBox(height: 20.h),
                      
                      // Description
                      _buildTextField(
                        controller: _descriptionController,
                        label: 'Description',
                        hint: 'Describe your goal in detail...',
                        maxLines: 3,
                      ),
                      
                      SizedBox(height: 20.h),
                      
                      // Category
                      _buildCategorySection(),
                      
                      SizedBox(height: 20.h),
                      
                      // Priority and Date
                      Row(
                        children: [
                          Expanded(child: _buildPrioritySection()),
                          SizedBox(width: 16.w),
                          Expanded(child: _buildDateSection()),
                        ],
                      ),
                      
                      SizedBox(height: 20.h),
                      
                      // Milestones
                      _buildMilestonesSection(),
                      
                      SizedBox(height: 100.h), // Space for buttons
                    ],
                  ),
                ),
              ),
            ],
          ),
        );
      },
    );
  }

  Widget _buildTextField({
    required TextEditingController controller,
    required String label,
    required String hint,
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
          style: TextStyle(
            color: AppColors.textLight,
            fontSize: 16.sp,
          ),
          decoration: InputDecoration(
            hintText: hint,
            hintStyle: TextStyle(
              color: AppColors.textMedium.withOpacity(0.6),
            ),
            filled: true,
            fillColor: AppColors.backgroundMedium,
            border: OutlineInputBorder(
              borderRadius: BorderRadius.circular(12.r),
              borderSide: BorderSide.none,
            ),
            contentPadding: EdgeInsets.all(16.w),
          ),
        ),
      ],
    );
  }

  Widget _buildCategorySection() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
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
                padding: EdgeInsets.symmetric(horizontal: 16.w, vertical: 12.h),
                decoration: BoxDecoration(
                  color: isSelected 
                      ? category.color.withOpacity(0.1)
                      : AppColors.backgroundMedium,
                  borderRadius: BorderRadius.circular(12.r),
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
                    SizedBox(width: 8.w),
                    Text(
                      category.displayName,
                      style: TextStyle(
                        fontSize: 14.sp,
                        fontWeight: FontWeight.w500,
                        color: isSelected ? category.color : AppColors.textMedium,
                      ),
                    ),
                  ],
                ),
              ),
            );
          }).toList(),
        ),
      ],
    );
  }

  Widget _buildPrioritySection() {
    return Column(
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
          padding: EdgeInsets.symmetric(horizontal: 12.w, vertical: 4.h),
          decoration: BoxDecoration(
            color: AppColors.backgroundMedium,
            borderRadius: BorderRadius.circular(12.r),
          ),
          child: DropdownButton<GoalPriority>(
            value: selectedPriority,
            isExpanded: true,
            underline: SizedBox(),
            dropdownColor: AppColors.backgroundMedium,
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
                        color: AppColors.textLight,
                        fontSize: 14.sp,
                      ),
                    ),
                  ],
                ),
              );
            }).toList(),
          ),
        ),
      ],
    );
  }

  Widget _buildDateSection() {
    return Column(
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
          onTap: _selectDate,
          child: Container(
            width: double.infinity,
            padding: EdgeInsets.all(12.w),
            decoration: BoxDecoration(
              color: AppColors.backgroundMedium,
              borderRadius: BorderRadius.circular(12.r),
            ),
            child: Row(
              children: [
                Icon(
                  Icons.calendar_today,
                  color: AppColors.textMedium,
                  size: 16.sp,
                ),
                SizedBox(width: 8.w),
                Text(
                  _formatDate(selectedDate),
                  style: TextStyle(
                    color: AppColors.textLight,
                    fontSize: 14.sp,
                  ),
                ),
              ],
            ),
          ),
        ),
      ],
    );
  }

  Widget _buildMilestonesSection() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
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
        ...milestones.asMap().entries.map((entry) {
          final index = entry.key;
          final milestone = entry.value;
          return Container(
            margin: EdgeInsets.only(bottom: 12.h),
            child: Row(
              children: [
                Container(
                  width: 24.w,
                  height: 24.h,
                  decoration: BoxDecoration(
                    color: selectedCategory.color.withOpacity(0.1),
                    shape: BoxShape.circle,
                    border: Border.all(
                      color: selectedCategory.color.withOpacity(0.3),
                    ),
                  ),
                  child: Center(
                    child: Text(
                      '${index + 1}',
                      style: TextStyle(
                        fontSize: 12.sp,
                        fontWeight: FontWeight.bold,
                        color: selectedCategory.color,
                      ),
                    ),
                  ),
                ),
                SizedBox(width: 12.w),
                Expanded(
                  child: TextField(
                    onChanged: (value) {
                      milestones[index] = value;
                    },
                    style: TextStyle(
                      color: AppColors.textLight,
                      fontSize: 14.sp,
                    ),
                    decoration: InputDecoration(
                      hintText: 'Milestone ${index + 1}...',
                      hintStyle: TextStyle(
                        color: AppColors.textMedium.withOpacity(0.6),
                      ),
                      filled: true,
                      fillColor: AppColors.backgroundMedium,
                      border: OutlineInputBorder(
                        borderRadius: BorderRadius.circular(8.r),
                        borderSide: BorderSide.none,
                      ),
                      contentPadding: EdgeInsets.symmetric(
                        horizontal: 12.w,
                        vertical: 8.h,
                      ),
                    ),
                  ),
                ),
              ],
            ),
          );
        }).toList(),
        
        // Create button
        SizedBox(height: 20.h),
        SizedBox(
          width: double.infinity,
          height: 56.h,
          child: ElevatedButton(
            onPressed: _createGoal,
            style: ElevatedButton.styleFrom(
              backgroundColor: Colors.transparent,
              shadowColor: Colors.transparent,
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(16.r),
              ),
            ).copyWith(
              backgroundColor: MaterialStateProperty.all(Colors.transparent),
            ),
            child: Ink(
              decoration: BoxDecoration(
                gradient: LinearGradient(
                  colors: AppColors.goalTrackerGradient,
                ),
                borderRadius: BorderRadius.circular(16.r),
              ),
              child: Container(
                alignment: Alignment.center,
                child: Text(
                  'Create Goal',
                  style: TextStyle(
                    fontSize: 18.sp,
                    fontWeight: FontWeight.w600,
                    color: AppColors.white,
                  ),
                ),
              ),
            ),
          ),
        ),
      ],
    );
  }

  void _selectDate() async {
    final DateTime? picked = await showDatePicker(
      context: context,
      initialDate: selectedDate,
      firstDate: DateTime.now(),
      lastDate: DateTime.now().add(Duration(days: 365 * 5)),
      builder: (context, child) {
        return Theme(
          data: Theme.of(context).copyWith(
            colorScheme: ColorScheme.dark(
              primary: AppColors.goalTrackerGradient[0],
              onPrimary: AppColors.white,
              surface: AppColors.cardBackground,
              onSurface: AppColors.textLight,
            ),
          ),
          child: child!,
        );
      },
    );
    
    if (picked != null && picked != selectedDate) {
      setState(() {
        selectedDate = picked;
      });
    }
  }

  void _createGoal() {
    if (_formKey.currentState!.validate()) {
      final goal = Goal(
        id: DateTime.now().millisecondsSinceEpoch.toString(),
        title: _titleController.text.trim(),
        description: _descriptionController.text.trim(),
        category: selectedCategory,
        priority: selectedPriority,
        targetDate: selectedDate,
        createdAt: DateTime.now(),
        milestones: milestones
            .where((m) => m.trim().isNotEmpty)
            .map((m) => Milestone(
                  id: DateTime.now().millisecondsSinceEpoch.toString() + m.hashCode.toString(),
                  title: m.trim(),
                  isCompleted: false,
                ))
            .toList(),
        currentProgress: 0.0,
        tags: [],
      );

      widget.onGoalAdded(goal);
      Navigator.pop(context);
      
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Goal created successfully!'),
          backgroundColor: AppColors.goalTrackerGradient[0],
        ),
      );
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