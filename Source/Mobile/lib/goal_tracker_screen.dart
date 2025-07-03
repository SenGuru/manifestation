import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:manifestation_app/add_goal_sheet.dart';
import 'package:manifestation_app/goal_card.dart';
import 'package:manifestation_app/goals.dart';
import '../colors.dart';

class GoalTrackerScreen extends StatefulWidget {
  @override
  _GoalTrackerScreenState createState() => _GoalTrackerScreenState();
}

class _GoalTrackerScreenState extends State<GoalTrackerScreen>
    with TickerProviderStateMixin {
  List<Goal> goals = [];
  GoalCategory selectedCategory = GoalCategory.all;
  bool showCompletedGoals = true;
  
  late AnimationController _fabAnimationController;
  late Animation<double> _fabAnimation;

  @override
  void initState() {
    super.initState();
    _fabAnimationController = AnimationController(
      duration: Duration(milliseconds: 300),
      vsync: this,
    );
    _fabAnimation = CurvedAnimation(
      parent: _fabAnimationController,
      curve: Curves.easeInOut,
    );
    
    _loadGoals();
    _fabAnimationController.forward();
  }

  @override
  void dispose() {
    _fabAnimationController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final filteredGoals = _filterGoals();
    
    return Scaffold(
      backgroundColor: AppColors.backgroundDark,
      body: SafeArea(
        child: Column(
          children: [
            // Header
            _buildHeader(),
            
            // Stats overview
            if (goals.isNotEmpty) _buildStatsOverview(),
            
            // Category filter
            _buildCategoryFilter(),
            
            // Goals list
            Expanded(
              child: filteredGoals.isEmpty 
                  ? _buildEmptyState()
                  : _buildGoalsList(filteredGoals),
            ),
          ],
        ),
      ),
      floatingActionButton: ScaleTransition(
        scale: _fabAnimation,
        child: FloatingActionButton(
          onPressed: _showAddGoalSheet,
          backgroundColor: AppColors.goalTrackerGradient[0],
          elevation: 8,
          child: Icon(
            Icons.add,
            color: AppColors.white,
            size: 28.sp,
          ),
        ),
      ),
    );
  }

  Widget _buildHeader() {
    return Padding(
      padding: EdgeInsets.all(20.w),
      child: Row(
        children: [
          GestureDetector(
            onTap: () => Navigator.pop(context),
            child: Container(
              width: 44.w,
              height: 44.h,
              decoration: BoxDecoration(
                color: AppColors.cardBackground,
                borderRadius: BorderRadius.circular(12.r),
              ),
              child: Icon(
                Icons.arrow_back,
                color: AppColors.textLight,
                size: 20.sp,
              ),
            ),
          ),
          SizedBox(width: 16.w),
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  'Goal Tracker',
                  style: TextStyle(
                    fontSize: 24.sp,
                    fontWeight: FontWeight.bold,
                    color: AppColors.textLight,
                  ),
                ),
                Text(
                  'Track your manifestation progress',
                  style: TextStyle(
                    fontSize: 14.sp,
                    color: AppColors.textMedium,
                  ),
                ),
              ],
            ),
          ),
          // Settings button
          GestureDetector(
            onTap: _showSettings,
            child: Container(
              width: 44.w,
              height: 44.h,
              decoration: BoxDecoration(
                color: AppColors.cardBackground,
                borderRadius: BorderRadius.circular(12.r),
              ),
              child: Icon(
                Icons.tune,
                color: AppColors.textLight,
                size: 20.sp,
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildStatsOverview() {
    final totalGoals = goals.length;
    final completedGoals = goals.where((g) => g.isCompleted).length;
    final activeGoals = goals.where((g) => !g.isCompleted).length;
    final overallProgress = totalGoals > 0 ? goals.map((g) => g.progressPercentage).reduce((a, b) => a + b) / totalGoals : 0.0;

    return Container(
      margin: EdgeInsets.symmetric(horizontal: 20.w),
      padding: EdgeInsets.all(20.w),
      decoration: BoxDecoration(
        gradient: LinearGradient(
          colors: AppColors.goalTrackerGradient,
          begin: Alignment.topLeft,
          end: Alignment.bottomRight,
        ),
        borderRadius: BorderRadius.circular(20.r),
        boxShadow: [
          BoxShadow(
            color: AppColors.goalTrackerGradient[0].withOpacity(0.3),
            blurRadius: 12.r,
            offset: Offset(0, 6.h),
          ),
        ],
      ),
      child: Column(
        children: [
          // Overall progress circle
          Row(
            children: [
              Container(
                width: 80.w,
                height: 80.h,
                child: Stack(
                  children: [
                    Center(
                      child: SizedBox(
                        width: 80.w,
                        height: 80.h,
                        child: CircularProgressIndicator(
                          value: overallProgress / 100,
                          strokeWidth: 6,
                          backgroundColor: AppColors.white.withOpacity(0.3),
                          valueColor: AlwaysStoppedAnimation<Color>(AppColors.white),
                        ),
                      ),
                    ),
                    Center(
                      child: Text(
                        '${overallProgress.round()}%',
                        style: TextStyle(
                          fontSize: 18.sp,
                          fontWeight: FontWeight.bold,
                          color: AppColors.white,
                        ),
                      ),
                    ),
                  ],
                ),
              ),
              SizedBox(width: 20.w),
              Expanded(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      'Overall Progress',
                      style: TextStyle(
                        fontSize: 16.sp,
                        fontWeight: FontWeight.w600,
                        color: AppColors.white,
                      ),
                    ),
                    SizedBox(height: 4.h),
                    Text(
                      'Keep manifesting your dreams!',
                      style: TextStyle(
                        fontSize: 14.sp,
                        color: AppColors.white.withOpacity(0.8),
                      ),
                    ),
                  ],
                ),
              ),
            ],
          ),
          
          SizedBox(height: 20.h),
          
          // Stats row
          Row(
            children: [
              Expanded(
                child: _buildStatItem('Total', totalGoals.toString(), Icons.flag),
              ),
              Container(
                width: 1,
                height: 30.h,
                color: AppColors.white.withOpacity(0.3),
              ),
              Expanded(
                child: _buildStatItem('Active', activeGoals.toString(), Icons.trending_up),
              ),
              Container(
                width: 1,
                height: 30.h,
                color: AppColors.white.withOpacity(0.3),
              ),
              Expanded(
                child: _buildStatItem('Done', completedGoals.toString(), Icons.check_circle),
              ),
            ],
          ),
        ],
      ),
    );
  }

  Widget _buildStatItem(String label, String value, IconData icon) {
    return Column(
      children: [
        Icon(
          icon,
          color: AppColors.white,
          size: 20.sp,
        ),
        SizedBox(height: 4.h),
        Text(
          value,
          style: TextStyle(
            fontSize: 20.sp,
            fontWeight: FontWeight.bold,
            color: AppColors.white,
          ),
        ),
        Text(
          label,
          style: TextStyle(
            fontSize: 12.sp,
            color: AppColors.white.withOpacity(0.8),
          ),
        ),
      ],
    );
  }

  Widget _buildCategoryFilter() {
    return Container(
      height: 50.h,
      margin: EdgeInsets.only(top: 20.h),
      child: ListView.builder(
        scrollDirection: Axis.horizontal,
        padding: EdgeInsets.symmetric(horizontal: 20.w),
        itemCount: GoalCategory.values.length,
        itemBuilder: (context, index) {
          final category = GoalCategory.values[index];
          final isSelected = selectedCategory == category;
          
          return GestureDetector(
            onTap: () {
              setState(() {
                selectedCategory = category;
              });
            },
            child: Container(
              margin: EdgeInsets.only(right: 12.w),
              padding: EdgeInsets.symmetric(horizontal: 20.w, vertical: 12.h),
              decoration: BoxDecoration(
                color: isSelected 
                    ? category.color.withOpacity(0.1)
                    : AppColors.cardBackground,
                borderRadius: BorderRadius.circular(25.r),
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
                      fontWeight: isSelected ? FontWeight.w600 : FontWeight.w500,
                      color: isSelected ? category.color : AppColors.textMedium,
                    ),
                  ),
                ],
              ),
            ),
          );
        },
      ),
    );
  }

  Widget _buildGoalsList(List<Goal> filteredGoals) {
    return ListView.builder(
      padding: EdgeInsets.all(20.w),
      itemCount: filteredGoals.length,
      itemBuilder: (context, index) {
        final goal = filteredGoals[index];
        return Container(
          margin: EdgeInsets.only(bottom: 16.h),
          child: GoalCard(
            goal: goal,
            onTap: () => _showGoalDetails(goal),
            onProgressUpdate: () => _updateGoalProgress(goal),
            onEdit: () => _editGoal(goal),
            onDelete: () => _deleteGoal(goal),
          ),
        );
      },
    );
  }

  Widget _buildEmptyState() {
    return Center(
      child: Padding(
        padding: EdgeInsets.all(40.w),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Container(
              width: 120.w,
              height: 120.h,
              decoration: BoxDecoration(
                color: AppColors.goalTrackerGradient[0].withOpacity(0.1),
                shape: BoxShape.circle,
              ),
              child: Icon(
                Icons.track_changes,
                color: AppColors.goalTrackerGradient[0],
                size: 60.sp,
              ),
            ),
            SizedBox(height: 24.h),
            Text(
              selectedCategory == GoalCategory.all 
                  ? 'No Goals Yet'
                  : 'No ${selectedCategory.displayName} Goals',
              style: TextStyle(
                fontSize: 24.sp,
                fontWeight: FontWeight.bold,
                color: AppColors.textLight,
              ),
            ),
            SizedBox(height: 12.h),
            Text(
              selectedCategory == GoalCategory.all
                  ? 'Start manifesting by creating your first goal'
                  : 'Create a goal in this category to get started',
              style: TextStyle(
                fontSize: 16.sp,
                color: AppColors.textMedium,
              ),
              textAlign: TextAlign.center,
            ),
            SizedBox(height: 32.h),
            GestureDetector(
              onTap: _showAddGoalSheet,
              child: Container(
                padding: EdgeInsets.symmetric(horizontal: 32.w, vertical: 16.h),
                decoration: BoxDecoration(
                  gradient: LinearGradient(
                    colors: AppColors.goalTrackerGradient,
                  ),
                  borderRadius: BorderRadius.circular(25.r),
                ),
                child: Row(
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    Icon(
                      Icons.add,
                      color: AppColors.white,
                      size: 20.sp,
                    ),
                    SizedBox(width: 8.w),
                    Text(
                      'Create Goal',
                      style: TextStyle(
                        fontSize: 16.sp,
                        fontWeight: FontWeight.w600,
                        color: AppColors.white,
                      ),
                    ),
                  ],
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }

  List<Goal> _filterGoals() {
    var filtered = goals;
    
    if (selectedCategory != GoalCategory.all) {
      filtered = filtered.where((goal) => goal.category == selectedCategory).toList();
    }
    
    if (!showCompletedGoals) {
      filtered = filtered.where((goal) => !goal.isCompleted).toList();
    }
    
    // Sort by priority and creation date
    filtered.sort((a, b) {
      if (a.isCompleted != b.isCompleted) {
        return a.isCompleted ? 1 : -1; // Active goals first
      }
      if (a.priority != b.priority) {
        return b.priority.index - a.priority.index; // High priority first
      }
      return b.createdAt.compareTo(a.createdAt); // Newest first
    });
    
    return filtered;
  }

  void _loadGoals() {
    // TODO: Load goals from local storage
    // For now, add some sample goals
    setState(() {
      goals = [
        Goal.sample1(),
        Goal.sample2(),
        Goal.sample3(),
      ];
    });
  }

  void _showAddGoalSheet() {
    showModalBottomSheet(
      context: context,
      backgroundColor: Colors.transparent,
      isScrollControlled: true,
      builder: (context) => AddGoalSheet(
        onGoalAdded: (goal) {
          setState(() {
            goals.add(goal);
          });
        },
      ),
    );
  }

  void _showGoalDetails(Goal goal) {
    // TODO: Navigate to goal details screen
    print('Showing details for: ${goal.title}');
  }

  void _updateGoalProgress(Goal goal) {
    // TODO: Show progress update dialog
    print('Updating progress for: ${goal.title}');
  }

  void _editGoal(Goal goal) {
    // TODO: Show edit goal sheet
    print('Editing goal: ${goal.title}');
  }

  void _deleteGoal(Goal goal) {
    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        backgroundColor: AppColors.cardBackground,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(16.r),
        ),
        title: Text(
          'Delete Goal',
          style: TextStyle(
            color: AppColors.textLight,
            fontSize: 18.sp,
            fontWeight: FontWeight.bold,
          ),
        ),
        content: Text(
          'Are you sure you want to delete "${goal.title}"? This action cannot be undone.',
          style: TextStyle(
            color: AppColors.textMedium,
            fontSize: 14.sp,
          ),
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context),
            child: Text(
              'Cancel',
              style: TextStyle(color: AppColors.textMedium),
            ),
          ),
          TextButton(
            onPressed: () {
              Navigator.pop(context);
              setState(() {
                goals.remove(goal);
              });
            },
            child: Text(
              'Delete',
              style: TextStyle(color: Colors.red),
            ),
          ),
        ],
      ),
    );
  }

  void _showSettings() {
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
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    'Settings',
                    style: TextStyle(
                      fontSize: 20.sp,
                      fontWeight: FontWeight.bold,
                      color: AppColors.textLight,
                    ),
                  ),
                  SizedBox(height: 24.h),
                  SwitchListTile(
                    title: Text(
                      'Show Completed Goals',
                      style: TextStyle(
                        color: AppColors.textLight,
                        fontSize: 16.sp,
                      ),
                    ),
                    subtitle: Text(
                      'Display goals you\'ve already achieved',
                      style: TextStyle(
                        color: AppColors.textMedium,
                        fontSize: 14.sp,
                      ),
                    ),
                    value: showCompletedGoals,
                    onChanged: (value) {
                      setState(() {
                        showCompletedGoals = value;
                      });
                      Navigator.pop(context);
                    },
                    activeColor: AppColors.goalTrackerGradient[0],
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }
}