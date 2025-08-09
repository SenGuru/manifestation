import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:mistel/services/user_service.dart';
import '../constants/app_theme.dart';
import '../models/goals.dart';
import '../components/progress_update_dialog.dart';
import '../components/edit_goal_sheet.dart';
import '../components/milestone_card.dart';
import '../components/milestone_dialog.dart';

class GoalDetailScreen extends StatefulWidget {
  final Goal goal;
  final Function(Goal) onGoalUpdated;
  final Function(Goal) onGoalDeleted;

  const GoalDetailScreen({
    Key? key,
    required this.goal,
    required this.onGoalUpdated,
    required this.onGoalDeleted,
  }) : super(key: key);

  @override
  _GoalDetailScreenState createState() => _GoalDetailScreenState();
}

class _GoalDetailScreenState extends State<GoalDetailScreen>
    with TickerProviderStateMixin {
  late Goal currentGoal;
  late AnimationController _progressController;
  late Animation<double> _progressAnimation;
  late AnimationController _fadeController;
  late Animation<double> _fadeAnimation;
  
  bool isLoadingMilestones = false;
  int? userId;

  @override
  void initState() {
    super.initState();
    currentGoal = widget.goal;
    
    _progressController = AnimationController(
      duration: Duration(milliseconds: 1000),
      vsync: this,
    );
    _progressAnimation = Tween<double>(
      begin: 0.0,
      end: currentGoal.progressPercentage / 100,
    ).animate(CurvedAnimation(
      parent: _progressController,
      curve: Curves.easeInOut,
    ));
    
    _fadeController = AnimationController(
      duration: Duration(milliseconds: 500),
      vsync: this,
    );
    _fadeAnimation = Tween<double>(
      begin: 0.0,
      end: 1.0,
    ).animate(CurvedAnimation(
      parent: _fadeController,
      curve: Curves.easeInOut,
    ));
    
    // Start animations and load data
    Future.delayed(Duration(milliseconds: 100), () {
      if (mounted) {
        _fadeController.forward();
        _progressController.forward();
      }
    });
    
    _loadUserIdAndMilestones();
  }

  @override
  void dispose() {
    _progressController.dispose();
    _fadeController.dispose();
    super.dispose();
  }

  Future<void> _loadUserIdAndMilestones() async {
    try {
      final userIdResult = await UserService.getUserId();
      setState(() {
        userId = userIdResult;
      });
    } catch (e) {
      print('Error loading user data: $e');
    }
  }


  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppTheme.backgroundDark,
      body: SafeArea(
        child: FadeTransition(
          opacity: _fadeAnimation,
          child: CustomScrollView(
            slivers: [
              // App bar
              SliverAppBar(
                expandedHeight: 140.h,
                floating: false,
                pinned: true,
                backgroundColor: AppTheme.backgroundDark,
                elevation: 0,
                leading: GestureDetector(
                  onTap: () => Navigator.pop(context),
                  child: Container(
                    margin: EdgeInsets.all(8.w),
                    decoration: BoxDecoration(
                      color: AppTheme.cardBackground,
                      borderRadius: BorderRadius.circular(12.r),
                    ),
                    child: Icon(
                      Icons.arrow_back,
                      color: AppTheme.textLight,
                      size: 20.sp,
                    ),
                  ),
                ),
                actions: [
                  GestureDetector(
                    onTap: _showEditGoal,
                    child: Container(
                      margin: EdgeInsets.all(8.w),
                      padding: EdgeInsets.all(8.w),
                      decoration: BoxDecoration(
                        color: AppTheme.cardBackground,
                        borderRadius: BorderRadius.circular(12.r),
                      ),
                      child: Icon(
                        Icons.edit,
                        color: AppTheme.textLight,
                        size: 20.sp,
                      ),
                    ),
                  ),
                  GestureDetector(
                    onTap: _showDeleteConfirmation,
                    child: Container(
                      margin: EdgeInsets.all(8.w),
                      padding: EdgeInsets.all(8.w),
                      decoration: BoxDecoration(
                        color: Colors.red.withOpacity(0.1),
                        borderRadius: BorderRadius.circular(12.r),
                      ),
                      child: Icon(
                        Icons.delete_outline,
                        color: Colors.red,
                        size: 20.sp,
                      ),
                    ),
                  ),
                ],
                flexibleSpace: FlexibleSpaceBar(
                  background: Container(
                    decoration: BoxDecoration(
                      gradient: LinearGradient(
                        colors: [
                          currentGoal.category.color.withOpacity(0.2),
                          currentGoal.category.color.withOpacity(0.1),
                          AppTheme.backgroundDark,
                        ],
                        begin: Alignment.topCenter,
                        end: Alignment.bottomCenter,
                        stops: [0.0, 0.5, 1.0],
                      ),
                    ),
                    child: Padding(
                      padding: EdgeInsets.fromLTRB(20.w, 70.h, 20.w, 20.h),
                      child: Row(
                        children: [
                          Container(
                            width: 60.w,
                            height: 60.h,
                            decoration: BoxDecoration(
                              color: currentGoal.category.color.withOpacity(0.1),
                              borderRadius: BorderRadius.circular(16.r),
                            ),
                            child: Icon(
                              currentGoal.category.icon,
                              color: currentGoal.category.color,
                              size: 30.sp,
                            ),
                          ),
                          SizedBox(width: 16.w),
                          Expanded(
                            child: Column(
                              crossAxisAlignment: CrossAxisAlignment.start,
                              mainAxisAlignment: MainAxisAlignment.center,
                              mainAxisSize: MainAxisSize.min,
                              children: [
                                Flexible(
                                  child: Text(
                                    currentGoal.title,
                                    style: TextStyle(
                                      fontSize: 23.sp,
                                      fontWeight: FontWeight.bold,
                                      color: AppTheme.textLight,
                                    ),
                                    maxLines: 3,
                                    overflow: TextOverflow.ellipsis,
                                  ),
                                ),
                                SizedBox(height: 6.h),
                                Row(
                                  mainAxisSize: MainAxisSize.min,
                                  children: [
                                    Icon(
                                      currentGoal.status.icon,
                                      color: currentGoal.status.color,
                                      size: 13.sp,
                                    ),
                                    SizedBox(width: 4.w),
                                    Flexible(
                                      child: Text(
                                        currentGoal.status.displayName,
                                        style: TextStyle(
                                          fontSize: 13.sp,
                                          color: currentGoal.status.color,
                                          fontWeight: FontWeight.w500,
                                        ),
                                        overflow: TextOverflow.ellipsis,
                                      ),
                                    ),
                                  ],
                                ),
                              ],
                            ),
                          ),
                        ],
                      ),
                    ),
                  ),
                ),
              ),
              
              // Content
              SliverPadding(
                padding: EdgeInsets.all(20.w),
                sliver: SliverList(
                  delegate: SliverChildListDelegate([
                    // Progress section
                    _buildProgressSection(),
                    
                    SizedBox(height: 24.h),
                    
                    // Stats section
                    _buildStatsSection(),
                    
                    SizedBox(height: 24.h),
                    
                    // Description section
                    if (currentGoal.description.isNotEmpty) _buildDescriptionSection(),
                    
                    SizedBox(height: 24.h),
                    
                    // Milestones section
                    if (currentGoal.milestones.isNotEmpty) _buildMilestonesSection(),
                    
                    SizedBox(height: 24.h),
                    
                    // Timeline section
                    _buildTimelineSection(),
                    
                    SizedBox(height: 100.h), // Space for FAB
                  ]),
                ),
              ),
            ],
          ),
        ),
      ),
      floatingActionButton: FloatingActionButton.extended(
        onPressed: _showProgressUpdate,
        backgroundColor: currentGoal.category.color,
        icon: Icon(
          Icons.trending_up,
          color: AppTheme.white,
          size: 20.sp,
        ),
        label: Text(
          'Update Progress',
          style: TextStyle(
            color: AppTheme.white,
            fontSize: 14.sp,
            fontWeight: FontWeight.w600,
          ),
        ),
      ),
    );
  }

  Widget _buildProgressSection() {
    return Container(
      padding: EdgeInsets.all(20.w),
      decoration: BoxDecoration(
        color: AppTheme.cardBackground,
        borderRadius: BorderRadius.circular(16.r),
        border: Border.all(
          color: currentGoal.category.color.withOpacity(0.3),
        ),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              Text(
                'Progress',
                style: TextStyle(
                  fontSize: 18.sp,
                  fontWeight: FontWeight.bold,
                  color: AppTheme.textLight,
                ),
              ),
              Text(
                '${currentGoal.progressPercentage.round()}%',
                style: TextStyle(
                  fontSize: 32.sp,
                  fontWeight: FontWeight.bold,
                  color: currentGoal.category.color,
                ),
              ),
            ],
          ),
          
          SizedBox(height: 16.h),
          
          // Animated progress bar
          AnimatedBuilder(
            animation: _progressAnimation,
            builder: (context, child) {
              return Container(
                height: 12.h,
                decoration: BoxDecoration(
                  color: AppTheme.textMedium.withOpacity(0.2),
                  borderRadius: BorderRadius.circular(6.r),
                ),
                child: FractionallySizedBox(
                  alignment: Alignment.centerLeft,
                  widthFactor: _progressAnimation.value,
                  child: Container(
                    decoration: BoxDecoration(
                      gradient: LinearGradient(
                        colors: [
                          currentGoal.category.color,
                          currentGoal.category.color.withOpacity(0.8),
                        ],
                      ),
                      borderRadius: BorderRadius.circular(6.r),
                    ),
                  ),
                ),
              );
            },
          ),
          
          if (currentGoal.isCompleted)
            Container(
              margin: EdgeInsets.only(top: 16.h),
              padding: EdgeInsets.all(12.w),
              decoration: BoxDecoration(
                color: AppTheme.accentMint.withOpacity(0.1),
                borderRadius: BorderRadius.circular(8.r),
                border: Border.all(
                  color: AppTheme.accentMint.withOpacity(0.3),
                ),
              ),
              child: Row(
                children: [
                  Icon(
                    Icons.celebration,
                    color: AppTheme.accentMint,
                    size: 20.sp,
                  ),
                  SizedBox(width: 8.w),
                  Expanded(
                    child: Text(
                      'Congratulations! Goal completed!',
                      style: TextStyle(
                        fontSize: 14.sp,
                        color: AppTheme.accentMint,
                        fontWeight: FontWeight.w600,
                      ),
                    ),
                  ),
                ],
              ),
            ),
        ],
      ),
    );
  }

  Widget _buildStatsSection() {
    return Container(
      padding: EdgeInsets.all(20.w),
      decoration: BoxDecoration(
        color: AppTheme.cardBackground,
        borderRadius: BorderRadius.circular(16.r),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(
            'Goal Details',
            style: TextStyle(
              fontSize: 18.sp,
              fontWeight: FontWeight.bold,
              color: AppTheme.textLight,
            ),
          ),
          SizedBox(height: 16.h),
          
          Row(
            children: [
              Expanded(
                child: _buildStatCard(
                  title: 'Priority',
                  value: currentGoal.priority.displayName,
                  icon: currentGoal.priority.icon,
                  color: currentGoal.priority.color,
                ),
              ),
              SizedBox(width: 12.w),
              Expanded(
                child: _buildStatCard(
                  title: 'Category',
                  value: currentGoal.category.displayName,
                  icon: currentGoal.category.icon,
                  color: currentGoal.category.color,
                ),
              ),
            ],
          ),
          
          SizedBox(height: 12.h),
          
          Row(
            children: [
              Expanded(
                child: _buildStatCard(
                  title: 'Milestones',
                  value: '${currentGoal.completedMilestones}/${currentGoal.milestones.length}',
                  icon: Icons.flag,
                  color: AppTheme.accentGold,
                ),
              ),
              SizedBox(width: 12.w),
              Expanded(
                child: _buildStatCard(
                  title: currentGoal.isOverdue ? 'Overdue' : 'Days Left',
                  value: currentGoal.isOverdue 
                      ? '${currentGoal.daysUntilTarget.abs()}d'
                      : '${currentGoal.daysUntilTarget}d',
                  icon: Icons.calendar_today,
                  color: currentGoal.isOverdue ? Colors.red : AppTheme.visionBoardGradient[0],
                ),
              ),
            ],
          ),
        ],
      ),
    );
  }

  Widget _buildStatCard({
    required String title,
    required String value,
    required IconData icon,
    required Color color,
  }) {
    return Container(
      padding: EdgeInsets.all(12.w),
      decoration: BoxDecoration(
        color: color.withOpacity(0.1),
        borderRadius: BorderRadius.circular(12.r),
        border: Border.all(
          color: color.withOpacity(0.3),
        ),
      ),
      child: Column(
        children: [
          Icon(
            icon,
            color: color,
            size: 20.sp,
          ),
          SizedBox(height: 8.h),
          Text(
            value,
            style: TextStyle(
              fontSize: 16.sp,
              fontWeight: FontWeight.bold,
              color: color,
            ),
          ),
          Text(
            title,
            style: TextStyle(
              fontSize: 12.sp,
              color: AppTheme.textMedium,
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildDescriptionSection() {
    return Container(
      padding: EdgeInsets.all(20.w),
      decoration: BoxDecoration(
        color: AppTheme.cardBackground,
        borderRadius: BorderRadius.circular(16.r),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(
            'Description',
            style: TextStyle(
              fontSize: 18.sp,
              fontWeight: FontWeight.bold,
              color: AppTheme.textLight,
            ),
          ),
          SizedBox(height: 12.h),
          Text(
            currentGoal.description,
            style: TextStyle(
              fontSize: 16.sp,
              color: AppTheme.textMedium,
              height: 1.5,
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildMilestonesSection() {
    final hasLocalMilestones = currentGoal.milestones.isNotEmpty;
    
    return Container(
      padding: EdgeInsets.all(20.w),
      decoration: BoxDecoration(
        color: AppTheme.cardBackground,
        borderRadius: BorderRadius.circular(16.r),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              Text(
                'Milestones',
                style: TextStyle(
                  fontSize: 18.sp,
                  fontWeight: FontWeight.bold,
                  color: AppTheme.textLight,
                ),
              ),
              Row(
                children: [
                  Text(
                    '${currentGoal.completedMilestones}/${currentGoal.milestones.length}',
                    style: TextStyle(
                      fontSize: 14.sp,
                      fontWeight: FontWeight.w600,
                      color: currentGoal.category.color,
                    ),
                  ),
                  SizedBox(width: 8.w),
                  GestureDetector(
                    onTap: _showAddMilestone,
                    child: Container(
                      padding: EdgeInsets.all(6.w),
                      decoration: BoxDecoration(
                        color: currentGoal.category.color.withOpacity(0.1),
                        borderRadius: BorderRadius.circular(6.r),
                      ),
                      child: Icon(
                        Icons.add,
                        color: currentGoal.category.color,
                        size: 16.sp,
                      ),
                    ),
                  ),
                ],
              ),
            ],
          ),
          
          SizedBox(height: 16.h),
          
          // Local milestones
          if (hasLocalMilestones)
            ...currentGoal.milestones.asMap().entries.map((entry) {
              final index = entry.key;
              final milestone = entry.value;
              
              return Container(
                margin: EdgeInsets.only(bottom: 12.h),
                child: Row(
                  children: [
                    GestureDetector(
                      onTap: () => _toggleMilestone(index),
                      child: Container(
                        width: 24.w,
                        height: 24.h,
                        decoration: BoxDecoration(
                          color: milestone.isCompleted 
                              ? currentGoal.category.color
                              : Colors.transparent,
                          border: Border.all(
                            color: milestone.isCompleted 
                                ? currentGoal.category.color
                                : AppTheme.textMedium,
                            width: 2,
                          ),
                          borderRadius: BorderRadius.circular(6.r),
                        ),
                        child: milestone.isCompleted
                            ? Icon(
                                Icons.check,
                                color: AppTheme.white,
                                size: 16.sp,
                              )
                            : null,
                      ),
                    ),
                    SizedBox(width: 12.w),
                    Expanded(
                      child: Text(
                        milestone.title,
                        style: TextStyle(
                          fontSize: 16.sp,
                          color: milestone.isCompleted 
                              ? AppTheme.textMedium
                              : AppTheme.textLight,
                          decoration: milestone.isCompleted 
                              ? TextDecoration.lineThrough
                              : TextDecoration.none,
                        ),
                      ),
                    ),
                  ],
                ),
              );
            }).toList()
            
          // Empty state
          else
            Container(
              padding: EdgeInsets.all(20.w),
              child: Column(
                children: [
                  Icon(
                    Icons.flag_outlined,
                    color: AppTheme.textMedium,
                    size: 32.sp,
                  ),
                  SizedBox(height: 8.h),
                  Text(
                    'No milestones yet',
                    style: TextStyle(
                      fontSize: 14.sp,
                      color: AppTheme.textMedium,
                    ),
                  ),
                  SizedBox(height: 8.h),
                  GestureDetector(
                    onTap: _showAddMilestone,
                    child: Text(
                      'Add your first milestone',
                      style: TextStyle(
                        fontSize: 14.sp,
                        color: currentGoal.category.color,
                        fontWeight: FontWeight.w600,
                      ),
                    ),
                  ),
                ],
              ),
            ),
        ],
      ),
    );
  }

  Widget _buildTimelineSection() {
    return Container(
      padding: EdgeInsets.all(20.w),
      decoration: BoxDecoration(
        color: AppTheme.cardBackground,
        borderRadius: BorderRadius.circular(16.r),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(
            'Timeline',
            style: TextStyle(
              fontSize: 18.sp,
              fontWeight: FontWeight.bold,
              color: AppTheme.textLight,
            ),
          ),
          SizedBox(height: 16.h),
          
          _buildTimelineItem(
            title: 'Goal Created',
            date: currentGoal.createdAt,
            icon: Icons.flag,
            color: AppTheme.accentGold,
            isCompleted: true,
          ),
          
          if (currentGoal.currentProgress > 0)
            _buildTimelineItem(
              title: 'Progress Started',
              date: currentGoal.createdAt.add(Duration(days: 1)), // Placeholder
              icon: Icons.play_arrow,
              color: AppTheme.visionBoardGradient[0],
              isCompleted: true,
            ),
          
          _buildTimelineItem(
            title: 'Target Completion',
            date: currentGoal.targetDate,
            icon: Icons.celebration,
            color: currentGoal.category.color,
            isCompleted: currentGoal.isCompleted,
            isTarget: true,
          ),
        ],
      ),
    );
  }

  Widget _buildTimelineItem({
    required String title,
    required DateTime date,
    required IconData icon,
    required Color color,
    required bool isCompleted,
    bool isTarget = false,
  }) {
    return Container(
      margin: EdgeInsets.only(bottom: 12.h),
      child: Row(
        children: [
          Container(
            width: 32.w,
            height: 32.h,
            decoration: BoxDecoration(
              color: isCompleted ? color : color.withOpacity(0.1),
              borderRadius: BorderRadius.circular(8.r),
            ),
            child: Icon(
              icon,
              color: isCompleted ? AppTheme.white : color,
              size: 16.sp,
            ),
          ),
          SizedBox(width: 12.w),
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  title,
                  style: TextStyle(
                    fontSize: 16.sp,
                    fontWeight: FontWeight.w600,
                    color: isCompleted ? AppTheme.textLight : AppTheme.textMedium,
                  ),
                ),
                Text(
                  '${date.day}/${date.month}/${date.year}',
                  style: TextStyle(
                    fontSize: 14.sp,
                    color: AppTheme.textMedium,
                  ),
                ),
              ],
            ),
          ),
          if (isTarget && currentGoal.isOverdue)
            Container(
              padding: EdgeInsets.symmetric(horizontal: 8.w, vertical: 4.h),
              decoration: BoxDecoration(
                color: Colors.red.withOpacity(0.1),
                borderRadius: BorderRadius.circular(6.r),
              ),
              child: Text(
                'Overdue',
                style: TextStyle(
                  fontSize: 12.sp,
                  color: Colors.red,
                  fontWeight: FontWeight.w600,
                ),
              ),
            ),
        ],
      ),
    );
  }

  void _showProgressUpdate() {
    showDialog(
      context: context,
      builder: (context) => ProgressUpdateDialog(
        goal: currentGoal,
        onProgressUpdate: (newProgress) {
          final updatedGoal = Goal(
            id: currentGoal.id,
            title: currentGoal.title,
            description: currentGoal.description,
            category: currentGoal.category,
            priority: currentGoal.priority,
            targetDate: currentGoal.targetDate,
            createdAt: currentGoal.createdAt,
            milestones: currentGoal.milestones,
            currentProgress: newProgress,
            tags: currentGoal.tags,
          );
          
          setState(() {
            currentGoal = updatedGoal;
          });
          
          widget.onGoalUpdated(updatedGoal);
          
          // Update progress animation
          _progressAnimation = Tween<double>(
            begin: _progressAnimation.value,
            end: newProgress / 100,
          ).animate(CurvedAnimation(
            parent: _progressController,
            curve: Curves.easeInOut,
          ));
          
          _progressController.reset();
          _progressController.forward();
          
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(
              content: Text(
                newProgress >= 100.0 
                    ? 'Goal completed! 🎉' 
                    : 'Progress updated to ${newProgress.round()}%'
              ),
              backgroundColor: newProgress >= 100.0 
                  ? AppTheme.accentMint 
                  : AppTheme.goalTrackerGradient[0],
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

  void _showEditGoal() {
    showModalBottomSheet(
      context: context,
      backgroundColor: Colors.transparent,
      isScrollControlled: true,
      builder: (context) => EditGoalSheet(
        goal: currentGoal,
        onGoalUpdated: (updatedGoal) {
          setState(() {
            currentGoal = updatedGoal;
          });
          
          widget.onGoalUpdated(updatedGoal);
          
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(
              content: Text('Goal updated successfully!'),
              backgroundColor: AppTheme.goalTrackerGradient[0],
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

  void _showDeleteConfirmation() {
    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        backgroundColor: AppTheme.cardBackground,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(16.r),
        ),
        title: Text(
          'Delete Goal',
          style: TextStyle(
            color: AppTheme.textLight,
            fontSize: 18.sp,
            fontWeight: FontWeight.bold,
          ),
        ),
        content: Text(
          'Are you sure you want to delete "${currentGoal.title}"? This action cannot be undone.',
          style: TextStyle(
            color: AppTheme.textMedium,
            fontSize: 14.sp,
          ),
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context),
            child: Text(
              'Cancel',
              style: TextStyle(color: AppTheme.textMedium),
            ),
          ),
          TextButton(
            onPressed: () {
              Navigator.pop(context); // Close dialog
              Navigator.pop(context); // Close detail screen
              widget.onGoalDeleted(currentGoal);
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

  void _toggleMilestone(int index) {
    final updatedMilestones = List<Milestone>.from(currentGoal.milestones);
    updatedMilestones[index] = updatedMilestones[index].copyWith(
      isCompleted: !updatedMilestones[index].isCompleted,
    );
    
    final updatedGoal = Goal(
      id: currentGoal.id,
      title: currentGoal.title,
      description: currentGoal.description,
      category: currentGoal.category,
      priority: currentGoal.priority,
      targetDate: currentGoal.targetDate,
      createdAt: currentGoal.createdAt,
      milestones: updatedMilestones,
      currentProgress: currentGoal.currentProgress,
      tags: currentGoal.tags,
    );
    
    setState(() {
      currentGoal = updatedGoal;
    });
    
    widget.onGoalUpdated(updatedGoal);
    
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text(
          updatedMilestones[index].isCompleted 
              ? 'Milestone completed!' 
              : 'Milestone unchecked'
        ),
        backgroundColor: AppTheme.goalTrackerGradient[0],
        behavior: SnackBarBehavior.floating,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(12.r),
        ),
      ),
    );
  }

  void _showAddMilestone() {
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text('Add milestone feature temporarily disabled'),
        backgroundColor: Colors.orange,
        behavior: SnackBarBehavior.floating,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(12.r),
        ),
      ),
    );
  }

  void _showSuccessMessage(String message) {
    if (mounted) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text(message),
          backgroundColor: AppTheme.accentMint,
          behavior: SnackBarBehavior.floating,
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(12.r),
          ),
        ),
      );
    }
  }

  void _showErrorMessage(String message) {
    if (mounted) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text(message),
          backgroundColor: Colors.red,
          behavior: SnackBarBehavior.floating,
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(12.r),
          ),
        ),
      );
    }
  }

  void _showRetryDialog(String actionName, VoidCallback retryAction) {
    if (mounted) {
      showDialog(
        context: context,
        builder: (context) => AlertDialog(
          backgroundColor: AppTheme.cardBackground,
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(16.r),
          ),
          title: Text(
            'Connection Error',
            style: TextStyle(
              color: AppTheme.textLight,
              fontSize: 18.sp,
              fontWeight: FontWeight.bold,
            ),
          ),
          content: Text(
            'Unable to $actionName. This might be due to a temporary network issue.',
            style: TextStyle(
              color: AppTheme.textMedium,
              fontSize: 14.sp,
            ),
          ),
          actions: [
            TextButton(
              onPressed: () => Navigator.pop(context),
              child: Text(
                'Cancel',
                style: TextStyle(color: AppTheme.textMedium),
              ),
            ),
            TextButton(
              onPressed: () {
                Navigator.pop(context);
                retryAction();
              },
              child: Text(
                'Retry',
                style: TextStyle(color: AppTheme.goalTrackerGradient[0]),
              ),
            ),
          ],
        ),
      );
    }
  }
}