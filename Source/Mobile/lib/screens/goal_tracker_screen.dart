import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:manifestation_app/components/add_goal_sheet.dart';
import 'package:manifestation_app/components/goal_card.dart';
import 'package:manifestation_app/constants/colors.dart';
import 'package:manifestation_app/screens/goals.dart';
import 'package:manifestation_app/services/goal_api_service.dart' as api;

class GoalTrackerScreen extends StatefulWidget {
  @override
  _GoalTrackerScreenState createState() => _GoalTrackerScreenState();
}

class _GoalTrackerScreenState extends State<GoalTrackerScreen>
    with TickerProviderStateMixin {
  List<Goal> goals = [];
  GoalCategory selectedCategory = GoalCategory.all;
  bool showCompletedGoals = true;
  bool isLoading = true;
  final api.GoalApiService _goalService = api.GoalApiService();
  
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
              child: isLoading
                  ? Center(
                      child: CircularProgressIndicator(
                        valueColor: AlwaysStoppedAnimation<Color>(AppColors.goalTrackerGradient[0]),
                      ),
                    )
                  : filteredGoals.isEmpty 
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
                  'Track your manifestation journey',
                  style: TextStyle(
                    fontSize: 14.sp,
                    color: AppColors.textMedium,
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildStatsOverview() {
    final totalGoals = goals.length;
    final completedGoals = goals.where((g) => g.isCompleted).length;
    final inProgressGoals = goals.where((g) => g.status == GoalStatus.inProgress).length;
    
    return Container(
      margin: EdgeInsets.symmetric(horizontal: 20.w),
      padding: EdgeInsets.all(20.w),
      decoration: BoxDecoration(
        gradient: LinearGradient(
          colors: AppColors.goalTrackerGradient,
          begin: Alignment.topLeft,
          end: Alignment.bottomRight,
        ),
        borderRadius: BorderRadius.circular(16.r),
      ),
      child: Row(
        mainAxisAlignment: MainAxisAlignment.spaceAround,
        children: [
          _buildStatItem('Total', totalGoals.toString(), Icons.flag),
          _buildStatItem('Active', inProgressGoals.toString(), Icons.trending_up),
          _buildStatItem('Completed', completedGoals.toString(), Icons.check_circle),
        ],
      ),
    );
  }

  Widget _buildStatItem(String label, String value, IconData icon) {
    return Column(
      children: [
        Icon(icon, color: AppColors.white, size: 24.sp),
        SizedBox(height: 8.h),
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
      child: ListView(
        scrollDirection: Axis.horizontal,
        padding: EdgeInsets.symmetric(horizontal: 20.w),
        children: GoalCategory.values.map((category) {
          final isSelected = selectedCategory == category;
          return GestureDetector(
            onTap: () {
              setState(() {
                selectedCategory = category;
              });
            },
            child: Container(
              margin: EdgeInsets.only(right: 12.w),
              padding: EdgeInsets.symmetric(horizontal: 16.w, vertical: 12.h),
              decoration: BoxDecoration(
                color: isSelected ? category.color : AppColors.cardBackground,
                borderRadius: BorderRadius.circular(25.r),
                border: Border.all(
                  color: isSelected ? category.color : AppColors.textMedium.withOpacity(0.3),
                ),
              ),
              child: Text(
                category.displayName,
                style: TextStyle(
                  fontSize: 14.sp,
                  fontWeight: FontWeight.w600,
                  color: isSelected ? AppColors.white : AppColors.textLight,
                ),
              ),
            ),
          );
        }).toList(),
      ),
    );
  }

  Widget _buildEmptyState() {
    return Center(
      child: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          Container(
            width: 80.w,
            height: 80.h,
            decoration: BoxDecoration(
              color: AppColors.goalTrackerGradient[0].withOpacity(0.2),
              borderRadius: BorderRadius.circular(20.r),
            ),
            child: Icon(
              Icons.track_changes,
              color: AppColors.goalTrackerGradient[0],
              size: 40.sp,
            ),
          ),
          SizedBox(height: 24.h),
          Text(
            'No Goals Yet',
            style: TextStyle(
              fontSize: 20.sp,
              fontWeight: FontWeight.bold,
              color: AppColors.textLight,
            ),
          ),
          SizedBox(height: 8.h),
          Text(
            selectedCategory == GoalCategory.all
                ? 'Create your first goal to start tracking progress'
                : 'No goals in ${selectedCategory.displayName} category',
            textAlign: TextAlign.center,
            style: TextStyle(
              fontSize: 14.sp,
              color: AppColors.textMedium,
              height: 1.4,
            ),
          ),
          SizedBox(height: 24.h),
          GestureDetector(
            onTap: _showAddGoalSheet,
            child: Container(
              padding: EdgeInsets.symmetric(horizontal: 24.w, vertical: 12.h),
              decoration: BoxDecoration(
                gradient: LinearGradient(colors: AppColors.goalTrackerGradient),
                borderRadius: BorderRadius.circular(12.r),
              ),
              child: Text(
                'Create Goal',
                style: TextStyle(
                  fontSize: 16.sp,
                  fontWeight: FontWeight.w600,
                  color: AppColors.white,
                ),
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildGoalsList(List<Goal> goals) {
    return ListView.builder(
      padding: EdgeInsets.all(20.w),
      itemCount: goals.length,
      itemBuilder: (context, index) {
        return Container(
          margin: EdgeInsets.only(bottom: 16.h),
          child: GoalCard(
            goal: goals[index],
            onTap: () => _showGoalDetails(goals[index]),
            onEdit: () => _editGoal(goals[index]),
            onDelete: () => _deleteGoal(goals[index]),
            onProgressUpdate: () => _updateGoalProgress(goals[index]),
          ),
        );
      },
    );
  }

  List<Goal> _filterGoals() {
    var filtered = goals.where((goal) {
      final categoryMatch = selectedCategory == GoalCategory.all || goal.category == selectedCategory;
      final completionMatch = showCompletedGoals || !goal.isCompleted;
      return categoryMatch && completionMatch;
    }).toList();
    
    // Sort by creation date (newest first)
    filtered.sort((a, b) => b.createdAt.compareTo(a.createdAt));
    
    return filtered;
  }

  Future<void> _loadGoals() async {
    setState(() {
      isLoading = true;
    });

    try {
      final apiGoals = await _goalService.getGoals();
      final convertedGoals = apiGoals.map((apiGoal) => _convertApiGoalToLocal(apiGoal)).toList();
      
      setState(() {
        goals = convertedGoals;
        isLoading = false;
      });
    } catch (e) {
      print('Error loading goals: $e');
      setState(() {
        isLoading = false;
        // Keep sample goals if API fails
        goals = [
          Goal.sample1(),
          Goal.sample2(),
          Goal.sample3(),
        ];
      });
    }
  }

  /// Convert API Goal to local Goal model
  Goal _convertApiGoalToLocal(api.Goal apiGoal) {
    return Goal(
      id: apiGoal.id,
      title: apiGoal.title,
      description: apiGoal.description,
      category: _convertApiCategoryToLocal(apiGoal.category),
      priority: GoalPriority.medium, // Default priority since API doesn't have this
      targetDate: apiGoal.endDate ?? DateTime.now().add(Duration(days: 30)),
      createdAt: apiGoal.createdAt,
      milestones: [], // No milestones for now
      currentProgress: _calculateProgressFromStatus(apiGoal.status),
      tags: [], // Default empty tags
    );
  }

  /// Convert API milestones to local milestones
  List<Milestone> _convertApiMilestonesToLocal(List<api.GoalMilestone> apiMilestones) {
    return apiMilestones.map((apiMilestone) => Milestone(
      id: apiMilestone.id,
      title: apiMilestone.title,
      description: null, // API milestone doesn't have description
      isCompleted: apiMilestone.status == api.GoalStatus.completed,
      completedAt: apiMilestone.completedDate,
      targetDate: apiMilestone.endDate,
    )).toList();
  }

  /// Convert API GoalCategory to local GoalCategory
  GoalCategory _convertApiCategoryToLocal(api.GoalCategory apiCategory) {
    switch (apiCategory) {
      case api.GoalCategory.career:
        return GoalCategory.career;
      case api.GoalCategory.health:
        return GoalCategory.health;
      case api.GoalCategory.love:
        return GoalCategory.relationships; // Map love to relationships
      case api.GoalCategory.money:
        return GoalCategory.financial; // Map money to financial
      case api.GoalCategory.personal:
        return GoalCategory.personal;
      case api.GoalCategory.spiritual:
        return GoalCategory.spiritual;
    }
  }

  /// Convert API GoalStatus to progress percentage
  double _calculateProgressFromStatus(api.GoalStatus status) {
    switch (status) {
      case api.GoalStatus.notStarted:
        return 0.0;
      case api.GoalStatus.inProgress:
        return 50.0; // Assume 50% for in progress
      case api.GoalStatus.completed:
        return 100.0;
      case api.GoalStatus.paused:
        return 25.0; // Assume 25% for paused
      case api.GoalStatus.cancelled:
        return 0.0;
    }
  }

  void _showAddGoalSheet() {
    showModalBottomSheet(
      context: context,
      backgroundColor: Colors.transparent,
      isScrollControlled: true,
      builder: (context) => AddGoalSheet(
        onGoalAdded: (goal) async {
          // Convert local goal to API goal and create it
          final apiGoal = await _createApiGoalFromLocal(goal);
          if (apiGoal != null) {
            await _loadGoals(); // Reload goals from API
            _showSuccessSnackBar('Goal created successfully!');
          } else {
            _showErrorSnackBar('Failed to create goal');
          }
        },
      ),
    );
  }

  Future<api.Goal?> _createApiGoalFromLocal(Goal localGoal) async {
    try {
      final apiGoal = api.Goal(
        id: '', // Will be set by API
        title: localGoal.title,
        description: localGoal.description,
        category: _convertLocalCategoryToApi(localGoal.category),
        startDate: localGoal.createdAt,
        endDate: localGoal.targetDate,
        status: localGoal.isCompleted ? api.GoalStatus.completed : api.GoalStatus.notStarted,
        createdAt: localGoal.createdAt,
      );

      return await _goalService.createGoal(apiGoal);
    } catch (e) {
      print('Error creating API goal: $e');
      return null;
    }
  }

  api.GoalCategory _convertLocalCategoryToApi(GoalCategory localCategory) {
    switch (localCategory) {
      case GoalCategory.career:
        return api.GoalCategory.career;
      case GoalCategory.health:
        return api.GoalCategory.health;
      case GoalCategory.relationships:
        return api.GoalCategory.love;
      case GoalCategory.financial:
        return api.GoalCategory.money;
      case GoalCategory.personal:
        return api.GoalCategory.personal;
      case GoalCategory.spiritual:
        return api.GoalCategory.spiritual;
      default:
        return api.GoalCategory.personal;
    }
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
        title: Text('Delete Goal', style: TextStyle(color: AppColors.textLight)),
        content: Text(
          'Are you sure you want to delete "${goal.title}"?',
          style: TextStyle(color: AppColors.textMedium),
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context),
            child: Text('Cancel', style: TextStyle(color: AppColors.textMedium)),
          ),
          TextButton(
            onPressed: () async {
              Navigator.pop(context);
              try {
                // Convert local goal ID to API format
                final success = await _goalService.deleteGoal(goal.id);
                if (success) {
                  await _loadGoals();
                  _showSuccessSnackBar('Goal deleted');
                } else {
                  _showErrorSnackBar('Failed to delete goal');
                }
              } catch (e) {
                _showErrorSnackBar('Error deleting goal');
              }
            },
            child: Text('Delete', style: TextStyle(color: Colors.red)),
          ),
        ],
      ),
    );
  }

  void _showSuccessSnackBar(String message) {
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text(message),
        backgroundColor: AppColors.accentMint,
        behavior: SnackBarBehavior.floating,
      ),
    );
  }

  void _showErrorSnackBar(String message) {
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text(message),
        backgroundColor: Colors.red,
        behavior: SnackBarBehavior.floating,
      ),
    );
  }
}