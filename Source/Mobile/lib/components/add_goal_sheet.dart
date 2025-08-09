import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../constants/app_theme.dart';
import 'package:mistel/models/goals.dart';

class MilestoneInput {
  String title;
  DateTime? targetDate;
  
  MilestoneInput({
    this.title = '',
    this.targetDate,
  });
}

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
  List<MilestoneInput> milestones = [
    MilestoneInput(),
    MilestoneInput(),
    MilestoneInput(),
    MilestoneInput(),
  ];

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
            color: AppTheme.cardBackground,
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
                  color: AppTheme.textMedium.withOpacity(0.3),
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
                        color: AppTheme.textLight,
                      ),
                    ),
                    Spacer(),
                    GestureDetector(
                      onTap: () => Navigator.pop(context),
                      child: Icon(
                        Icons.close,
                        color: AppTheme.textMedium,
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
            color: AppTheme.textLight,
          ),
        ),
        SizedBox(height: 8.h),
        TextFormField(
          controller: controller,
          validator: validator,
          maxLines: maxLines,
          style: TextStyle(
            color: AppTheme.textLight,
            fontSize: 16.sp,
          ),
          decoration: InputDecoration(
            hintText: hint,
            hintStyle: TextStyle(
              color: AppTheme.textMedium.withOpacity(0.6),
            ),
            filled: true,
            fillColor: AppTheme.backgroundMedium,
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
            color: AppTheme.textLight,
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
                      : AppTheme.backgroundMedium,
                  borderRadius: BorderRadius.circular(12.r),
                  border: Border.all(
                    color: isSelected 
                        ? category.color
                        : AppTheme.textMedium.withOpacity(0.3),
                  ),
                ),
                child: Row(
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    Icon(
                      category.icon,
                      color: isSelected ? category.color : AppTheme.textMedium,
                      size: 16.sp,
                    ),
                    SizedBox(width: 8.w),
                    Text(
                      category.displayName,
                      style: TextStyle(
                        fontSize: 14.sp,
                        fontWeight: FontWeight.w500,
                        color: isSelected ? category.color : AppTheme.textMedium,
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
            color: AppTheme.textLight,
          ),
        ),
        SizedBox(height: 8.h),
        Container(
          padding: EdgeInsets.symmetric(horizontal: 12.w, vertical: 4.h),
          decoration: BoxDecoration(
            color: AppTheme.backgroundMedium,
            borderRadius: BorderRadius.circular(12.r),
          ),
          child: DropdownButton<GoalPriority>(
            value: selectedPriority,
            isExpanded: true,
            underline: SizedBox(),
            dropdownColor: AppTheme.backgroundMedium,
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
                        color: AppTheme.textLight,
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
            color: AppTheme.textLight,
          ),
        ),
        SizedBox(height: 8.h),
        GestureDetector(
          onTap: _selectDate,
          child: Container(
            width: double.infinity,
            padding: EdgeInsets.all(12.w),
            decoration: BoxDecoration(
              color: AppTheme.backgroundMedium,
              borderRadius: BorderRadius.circular(12.r),
            ),
            child: Row(
              children: [
                Icon(
                  Icons.calendar_today,
                  color: AppTheme.textMedium,
                  size: 16.sp,
                ),
                SizedBox(width: 8.w),
                Text(
                  _formatDate(selectedDate),
                  style: TextStyle(
                    color: AppTheme.textLight,
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
            color: AppTheme.textLight,
          ),
        ),
        SizedBox(height: 8.h),
        Text(
          'Break your goal into smaller, achievable steps',
          style: TextStyle(
            fontSize: 14.sp,
            color: AppTheme.textMedium,
          ),
        ),
        SizedBox(height: 12.h),
        ...milestones.asMap().entries.map((entry) {
          final index = entry.key;
          final milestoneInput = entry.value;
          return _buildMilestoneCard(index, milestoneInput);
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
                  colors: AppTheme.goalTrackerGradient,
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
                    color: AppTheme.white,
                  ),
                ),
              ),
            ),
          ),
        ),
      ],
    );
  }

  Widget _buildMilestoneCard(int index, MilestoneInput milestoneInput) {
    return Container(
      margin: EdgeInsets.only(bottom: 16.h),
      padding: EdgeInsets.all(16.w),
      decoration: BoxDecoration(
        color: AppTheme.backgroundMedium,
        borderRadius: BorderRadius.circular(12.r),
        border: Border.all(
          color: selectedCategory.color.withOpacity(0.2),
        ),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          // Header row with milestone number and optional indicator
          Row(
            children: [
              Container(
                width: 28.w,
                height: 28.h,
                decoration: BoxDecoration(
                  color: selectedCategory.color.withOpacity(0.1),
                  shape: BoxShape.circle,
                  border: Border.all(
                    color: selectedCategory.color.withOpacity(0.3),
                    width: 1.5,
                  ),
                ),
                child: Center(
                  child: Text(
                    '${index + 1}',
                    style: TextStyle(
                      fontSize: 14.sp,
                      fontWeight: FontWeight.bold,
                      color: selectedCategory.color,
                    ),
                  ),
                ),
              ),
              SizedBox(width: 12.w),
              Text(
                'Milestone ${index + 1}',
                style: TextStyle(
                  fontSize: 16.sp,
                  fontWeight: FontWeight.w600,
                  color: AppTheme.textLight,
                ),
              ),
              Spacer(),
              if (milestoneInput.title.isNotEmpty || milestoneInput.targetDate != null)
                Icon(
                  Icons.check_circle,
                  color: selectedCategory.color,
                  size: 20.sp,
                ),
            ],
          ),
          SizedBox(height: 12.h),
          
          // Title input
          TextField(
            onChanged: (value) {
              setState(() {
                milestoneInput.title = value;
              });
            },
            style: TextStyle(
              color: AppTheme.textLight,
              fontSize: 14.sp,
            ),
            decoration: InputDecoration(
              hintText: 'What milestone do you want to achieve?',
              hintStyle: TextStyle(
                color: AppTheme.textMedium.withOpacity(0.6),
              ),
              filled: true,
              fillColor: AppTheme.cardBackground,
              border: OutlineInputBorder(
                borderRadius: BorderRadius.circular(8.r),
                borderSide: BorderSide.none,
              ),
              contentPadding: EdgeInsets.symmetric(
                horizontal: 12.w,
                vertical: 12.h,
              ),
              prefixIcon: Icon(
                Icons.flag_outlined,
                color: AppTheme.textMedium,
                size: 20.sp,
              ),
            ),
          ),
          SizedBox(height: 12.h),
          
          // Target date selection
          GestureDetector(
            onTap: () => _selectMilestoneDate(index),
            child: Container(
              width: double.infinity,
              padding: EdgeInsets.symmetric(horizontal: 12.w, vertical: 12.h),
              decoration: BoxDecoration(
                color: AppTheme.cardBackground,
                borderRadius: BorderRadius.circular(8.r),
                border: Border.all(
                  color: milestoneInput.targetDate != null 
                      ? selectedCategory.color.withOpacity(0.3)
                      : AppTheme.textMedium.withOpacity(0.2),
                ),
              ),
              child: Row(
                children: [
                  Icon(
                    Icons.schedule,
                    color: milestoneInput.targetDate != null 
                        ? selectedCategory.color 
                        : AppTheme.textMedium,
                    size: 20.sp,
                  ),
                  SizedBox(width: 8.w),
                  Text(
                    milestoneInput.targetDate != null
                        ? _formatDate(milestoneInput.targetDate!)
                        : 'Set target date (optional)',
                    style: TextStyle(
                      color: milestoneInput.targetDate != null 
                          ? AppTheme.textLight 
                          : AppTheme.textMedium.withOpacity(0.6),
                      fontSize: 14.sp,
                    ),
                  ),
                  Spacer(),
                  if (milestoneInput.targetDate != null)
                    GestureDetector(
                      onTap: () {
                        setState(() {
                          milestoneInput.targetDate = null;
                        });
                      },
                      child: Icon(
                        Icons.clear,
                        color: AppTheme.textMedium,
                        size: 18.sp,
                      ),
                    ),
                ],
              ),
            ),
          ),
        ],
      ),
    );
  }

  void _selectMilestoneDate(int index) async {
    final DateTime? picked = await showDatePicker(
      context: context,
      initialDate: milestones[index].targetDate ?? selectedDate,
      firstDate: DateTime.now(),
      lastDate: selectedDate, // Can't be later than goal target date
      builder: (context, child) {
        return Theme(
          data: Theme.of(context).copyWith(
            colorScheme: ColorScheme.dark(
              primary: selectedCategory.color,
              onPrimary: AppTheme.white,
              surface: AppTheme.cardBackground,
              onSurface: AppTheme.textLight,
            ),
          ),
          child: child!,
        );
      },
    );
    
    if (picked != null) {
      setState(() {
        milestones[index].targetDate = picked;
      });
    }
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
              primary: AppTheme.goalTrackerGradient[0],
              onPrimary: AppTheme.white,
              surface: AppTheme.cardBackground,
              onSurface: AppTheme.textLight,
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

  void _createGoal() async {
    print('=== DEBUG: _createGoal() called ===');
    print('Form validation: ${_formKey.currentState?.validate()}');
    
    if (_formKey.currentState!.validate()) {
      print('Form is valid, creating goal...');
      
      final validMilestones = milestones.where((m) => m.title.trim().isNotEmpty).toList();
      print('Valid milestones count: ${validMilestones.length}');
      print('Valid milestones: ${validMilestones.map((m) => '${m.title} (${m.targetDate != null ? _formatDate(m.targetDate!) : 'no date'})')}');
      
      final goal = Goal(
        id: DateTime.now().millisecondsSinceEpoch.toString(),
        title: _titleController.text.trim(),
        description: _descriptionController.text.trim(),
        category: selectedCategory,
        priority: selectedPriority,
        targetDate: selectedDate,
        createdAt: DateTime.now(),
        milestones: validMilestones
            .map((m) => Milestone(
                  id: DateTime.now().millisecondsSinceEpoch.toString() + m.title.hashCode.toString(),
                  title: m.title.trim(),
                  isCompleted: false,
                  targetDate: m.targetDate,
                ))
            .toList(),
        currentProgress: 0.0,
        tags: [],
      );

      print('Goal created with ID: ${goal.id}');
      print('Goal local milestones: ${goal.milestones.length}');

      // Call the parent callback to save the goal
      print('Calling widget.onGoalAdded...');
      widget.onGoalAdded(goal);
      print('widget.onGoalAdded completed');
      
      // NOTE: Milestone creation will now happen AFTER the goal is saved to API
      // This is handled in the goal_tracker_screen.dart in the onGoalAdded callback
      // The milestones will be created using the real API goal ID, not the local ID
      print('Milestone creation will happen after goal is saved to API with real goal ID');
      
      Navigator.pop(context);
      
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Goal created successfully!'),
          backgroundColor: AppTheme.goalTrackerGradient[0],
        ),
      );
    } else {
      print('Form validation failed');
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