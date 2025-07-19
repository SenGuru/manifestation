import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../components/add_goal_sheet.dart';
import '../components/edit_goal_sheet.dart';
import '../components/goal_card.dart';
import '../components/progress_update_dialog.dart';
import '../models/goals.dart';
import '../constants/colors.dart';
import 'goal_detail_screen.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'dart:convert';

class GoalTrackerScreen extends StatefulWidget {
  @override
  _GoalTrackerScreenState createState() => _GoalTrackerScreenState();
}

class _GoalTrackerScreenState extends State<GoalTrackerScreen>
    with TickerProviderStateMixin {
  
  List<Goal> goals = [];
  GoalCategory selectedCategory = GoalCategory.all;
  bool showCompletedGoals = true;
  bool _isLoading = false;
  
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
              child: _isLoading
                  ? _buildLoadingState()
                  : filteredGoals.isEmpty 
                      ? _buildEmptyState()
                      : RefreshIndicator(
                          onRefresh: _loadGoals,
                          color: AppColors.goalTrackerGradient[0],
                          child: _buildGoalsList(filteredGoals),
                        ),
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
                border: Border.all(
                      color: AppColors.textMedium.withOpacity(0.1),
                    ),
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
    final overallProgress = totalGoals > 0 ? goals.map((g) => g.currentProgress).reduce((a, b) => a + b) / totalGoals : 0.0;

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

  Widget _buildLoadingState() {
    return Center(
      child: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          CircularProgressIndicator(
            valueColor: AlwaysStoppedAnimation<Color>(AppColors.goalTrackerGradient[0]),
          ),
          SizedBox(height: 16.h),
          Text(
            'Loading your goals...',
            style: TextStyle(
              fontSize: 16.sp,
              color: AppColors.textMedium,
            ),
          ),
        ],
      ),
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

  Future<void> _loadGoals() async {
    setState(() {
      _isLoading = true;
    });

    try {
      print('=== LOADING HARDCODED GOALS ===');
      
      // Simulate loading delay
      await Future.delayed(Duration(milliseconds: 500));
      
      // Create hardcoded goals
      final hardcodedGoals = [
        Goal(
          id: '1',
          title: 'Get Promoted at Work',
          description: 'Work towards getting a promotion to senior level',
          category: GoalCategory.career,
          priority: GoalPriority.high,
          targetDate: DateTime.now().add(Duration(days: 180)),
          createdAt: DateTime.now().subtract(Duration(days: 30)),
          milestones: [
            Milestone(
              id: '1',
              title: 'Complete leadership training',
              description: 'Finish the leadership development program',
              isCompleted: true,
              completedAt: DateTime.now().subtract(Duration(days: 10)),
              targetDate: DateTime.now().add(Duration(days: 60)),
            ),
            Milestone(
              id: '2',
              title: 'Present quarterly results',
              description: 'Successfully present Q4 results to management',
              isCompleted: false,
              targetDate: DateTime.now().add(Duration(days: 45)),
            ),
          ],
          currentProgress: 60.0,
          tags: ['career', 'professional'],
        ),
        Goal(
          id: '2',
          title: 'Run a Marathon',
          description: 'Complete a full 26.2 mile marathon',
          category: GoalCategory.health,
          priority: GoalPriority.medium,
          targetDate: DateTime.now().add(Duration(days: 365)),
          createdAt: DateTime.now().subtract(Duration(days: 15)),
          milestones: [
            Milestone(
              id: '3',
              title: 'Run 10K consistently',
              description: 'Be able to run 10K without stopping',
              isCompleted: true,
              completedAt: DateTime.now().subtract(Duration(days: 5)),
              targetDate: DateTime.now().add(Duration(days: 90)),
            ),
            Milestone(
              id: '4',
              title: 'Complete half marathon',
              description: 'Finish a 13.1 mile half marathon',
              isCompleted: false,
              targetDate: DateTime.now().add(Duration(days: 180)),
            ),
          ],
          currentProgress: 35.0,
          tags: ['health', 'fitness'],
        ),
        Goal(
          id: '3',
          title: 'Save \$10,000',
          description: 'Build an emergency fund of \$10,000',
          category: GoalCategory.financial,
          priority: GoalPriority.high,
          targetDate: DateTime.now().add(Duration(days: 365)),
          createdAt: DateTime.now().subtract(Duration(days: 60)),
          milestones: [
            Milestone(
              id: '5',
              title: 'Save \$2,500',
              description: 'First quarter of emergency fund',
              isCompleted: true,
              completedAt: DateTime.now().subtract(Duration(days: 20)),
              targetDate: DateTime.now().add(Duration(days: 90)),
            ),
            Milestone(
              id: '6',
              title: 'Save \$5,000',
              description: 'Halfway to goal',
              isCompleted: false,
              targetDate: DateTime.now().add(Duration(days: 180)),
            ),
          ],
          currentProgress: 25.0,
          tags: ['money', 'savings'],
        ),
      ];
      
      setState(() {
        goals = hardcodedGoals;
      });
      
      print('✅ Successfully loaded ${goals.length} hardcoded goals');
    } catch (e) {
      print('Exception loading goals: $e');
      setState(() {
        goals = [];
      });
    } finally {
      setState(() {
        _isLoading = false;
      });
    }
  }

  // Goals are now loaded as hardcoded data

  // Store goal-category mapping locally
  Future<void> _storeGoalCategory(String goalId, GoalCategory category) async {
    try {
      final prefs = await SharedPreferences.getInstance();
      final key = 'goal_categories';
      final existing = prefs.getString(key) ?? '{}';
      final Map<String, dynamic> categories = json.decode(existing);
      
      categories[goalId] = category.name;
      await prefs.setString(key, json.encode(categories));
      
      print('💾 Stored category ${category.name} for goal $goalId');
    } catch (e) {
      print('❌ Error storing goal category: $e');
    }
  }

  void _showAddGoalSheet() {
    showModalBottomSheet(
      context: context,
      backgroundColor: Colors.transparent,
      isScrollControlled: true,
      builder: (context) => AddGoalSheet(
        onGoalAdded: (goal) async {
          // Add the goal to the UI
          setState(() {
            goals.add(goal);
          });
          
          // Store goal category locally
          await _storeGoalCategory(goal.id, goal.category);
          
          print('Goal "${goal.title}" saved locally');
          
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(
              content: Text('Goal added successfully!'),
              backgroundColor: AppColors.goalTrackerGradient[0],
              behavior: SnackBarBehavior.floating,
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(12.r),
              ),
            ),
          );
        },
      ),
    );
  }

  void _showGoalDetails(Goal goal) {
    Navigator.push(
      context,
      MaterialPageRoute(
        builder: (context) => GoalDetailScreen(
          goal: goal,
          onGoalUpdated: (updatedGoal) async {
            // Update the goal in the UI
            setState(() {
              final index = goals.indexWhere((g) => g.id == goal.id);
              if (index != -1) {
                goals[index] = updatedGoal;
              }
            });
            
            // Goal updated locally - no server sync needed
          },
          onGoalDeleted: (deletedGoal) async {
            // Goal deleted locally
            setState(() {
              goals.remove(deletedGoal);
            });
          },
        ),
      ),
    );
  }

  void _updateGoalProgress(Goal goal) {
    showDialog(
      context: context,
      builder: (context) => ProgressUpdateDialog(
        goal: goal,
        onProgressUpdate: (newProgress) async {
          // Update the goal locally
          final updatedGoal = Goal(
            id: goal.id,
            title: goal.title,
            description: goal.description,
            category: goal.category,
            priority: goal.priority,
            targetDate: goal.targetDate,
            createdAt: goal.createdAt,
            milestones: goal.milestones,
            currentProgress: newProgress,
            tags: goal.tags,
          );
          
          setState(() {
            final index = goals.indexWhere((g) => g.id == goal.id);
            if (index != -1) {
              goals[index] = updatedGoal;
            }
          });
          
          // Update on server
          // Goal updated locally - no server sync needed
          
          // Show success message
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(
              content: Text(
                newProgress >= 100.0 
                    ? 'Goal completed! 🎉' 
                    : 'Progress updated to ${newProgress.round()}%'
              ),
              backgroundColor: newProgress >= 100.0 
                  ? AppColors.accentMint 
                  : AppColors.goalTrackerGradient[0],
              behavior: SnackBarBehavior.floating,
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(12.r),
              ),
            ),
          );
        },
      ),
    );
  }

  void _editGoal(Goal goal) {
    showModalBottomSheet(
      context: context,
      backgroundColor: Colors.transparent,
      isScrollControlled: true,
      builder: (context) => EditGoalSheet(
        goal: goal,
        onGoalUpdated: (updatedGoal) async {
          // Update the goal in the UI
          setState(() {
            final index = goals.indexWhere((g) => g.id == goal.id);
            if (index != -1) {
              goals[index] = updatedGoal;
            }
          });
          
          // Update on server
          // Goal updated locally - no server sync needed
          
          // Show success message
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(
              content: Text('Goal updated successfully!'),
              backgroundColor: AppColors.goalTrackerGradient[0],
              behavior: SnackBarBehavior.floating,
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(12.r),
              ),
            ),
          );
        },
      ),
    );
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
            onPressed: () async {
              Navigator.pop(context);
              // Delete goal locally
              setState(() {
                goals.remove(goal);
              });
              ScaffoldMessenger.of(context).showSnackBar(
                SnackBar(
                  content: Text('Goal deleted successfully'),
                  backgroundColor: Colors.green,
                  behavior: SnackBarBehavior.floating,
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(12.r),
                  ),
                ),
              );
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
      isScrollControlled: true,
      builder: (context) => Container(
        height: MediaQuery.of(context).size.height * 0.6,
        margin: EdgeInsets.all(20.w),
        decoration: BoxDecoration(
          color: AppColors.cardBackground,
          borderRadius: BorderRadius.circular(24.r),
        ),
        child: Column(
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
              child: Text(
                'Goal Settings',
                style: TextStyle(
                  fontSize: 24.sp,
                  fontWeight: FontWeight.bold,
                  color: AppColors.textLight,
                ),
              ),
            ),
            Expanded(
              child: ListView(
                padding: EdgeInsets.symmetric(horizontal: 24.w),
                children: [
                  ListTile(
                    leading: Icon(
                      Icons.visibility,
                      color: AppColors.textLight,
                    ),
                    title: Text(
                      'Show Completed Goals',
                      style: TextStyle(
                        color: AppColors.textLight,
                        fontSize: 16.sp,
                      ),
                    ),
                    trailing: Switch(
                      value: showCompletedGoals,
                      onChanged: (value) {
                        setState(() {
                          showCompletedGoals = value;
                        });
                        Navigator.pop(context);
                      },
                      activeColor: AppColors.goalTrackerGradient[0],
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
}