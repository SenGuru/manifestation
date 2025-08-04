import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:fun/services/user_service.dart';
import '../constants/app_theme.dart';
import '../models/goals.dart';
import '../components/progress_update_dialog.dart';
import '../components/edit_goal_sheet.dart';
import '../components/milestone_dialog.dart';
import '../widgets/universal_back_button.dart';
import '../models/api_milestone.dart';
import '../services/goal_tracker_service.dart';
import '../constants/goal_constants.dart';

class GoalDetailScreenRedesigned extends StatefulWidget {
  final Goal goal;
  final Function(Goal) onGoalUpdated;
  final Function(Goal) onGoalDeleted;

  const GoalDetailScreenRedesigned({
    Key? key,
    required this.goal,
    required this.onGoalUpdated,
    required this.onGoalDeleted,
  }) : super(key: key);

  @override
  _GoalDetailScreenRedesignedState createState() => _GoalDetailScreenRedesignedState();
}

class _GoalDetailScreenRedesignedState extends State<GoalDetailScreenRedesigned>
    with TickerProviderStateMixin {
  late Goal currentGoal;
  late AnimationController _progressController;
  late Animation<double> _progressAnimation;
  
  int? userId;
  final GoalTrackerService _goalTrackerService = GoalTrackerService();

  @override
  void initState() {
    super.initState();
    currentGoal = widget.goal;
    
    // Initialize the service
    _goalTrackerService.initialize();
    
    _progressController = AnimationController(
      duration: Duration(milliseconds: 800),
      vsync: this,
    );
    _progressAnimation = Tween<double>(
      begin: 0.0,
      end: currentGoal.progressPercentage / 100,
    ).animate(CurvedAnimation(
      parent: _progressController,
      curve: Curves.easeOut,
    ));
    
    Future.delayed(Duration(milliseconds: 200), () {
      if (mounted) {
        _progressController.forward();
      }
    });
    
    _loadUserIdAndMilestones();
  }

  @override
  void dispose() {
    _progressController.dispose();
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
        child: Column(
          children: [
            // Header like other screens
            _buildHeader(),
            
            // Content
            Expanded(
              child: SingleChildScrollView(
                padding: EdgeInsets.all(20.w),
                child: Column(
                  children: [
                    // Progress section
                    _buildProgressCard(),
                    SizedBox(height: 20.h),
                    
                    // Quick stats row
                    _buildQuickStatsRow(),
                    SizedBox(height: 20.h),
                    
                    // Description
                    if (currentGoal.description.isNotEmpty) 
                      _buildDescriptionCard(),
                    if (currentGoal.description.isNotEmpty) 
                      SizedBox(height: 20.h),
                    
                    // Milestones
                    _buildMilestonesCard(),
                    SizedBox(height: 20.h),
                    
                    // Timeline
                    _buildTimelineCard(),
                    
                    SizedBox(height: 20.h), // Small space at bottom
                  ],
                ),
              ),
            ),
          ],
        ),
      ),
      floatingActionButton: Container(
        decoration: BoxDecoration(
          color: currentGoal.category.color,
          borderRadius: BorderRadius.circular(16.r),
          boxShadow: [
            BoxShadow(
              color: currentGoal.category.color.withOpacity(0.3),
              blurRadius: 12.r,
              offset: Offset(0, 4.h),
            ),
          ],
        ),
        child: Material(
          color: Colors.transparent,
          child: InkWell(
            onTap: _showProgressUpdate,
            borderRadius: BorderRadius.circular(16.r),
            child: Container(
              padding: EdgeInsets.symmetric(horizontal: 20.w, vertical: 16.h),
              child: Row(
                mainAxisSize: MainAxisSize.min,
                children: [
                  Icon(
                    Icons.trending_up,
                    color: Colors.white,
                    size: 20.sp,
                  ),
                  SizedBox(width: 8.w),
                  Text(
                    'Update Progress',
                    style: TextStyle(
                      color: Colors.white,
                      fontSize: 14.sp,
                      fontWeight: FontWeight.w600,
                    ),
                  ),
                ],
              ),
            ),
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
          UniversalBackButton(),
          SizedBox(width: 16.w),
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
              children: [
                Text(
                  currentGoal.title,
                  style: TextStyle(
                    fontSize: 24.sp,
                    fontWeight: FontWeight.bold,
                    color: AppTheme.textLight,
                  ),
                  maxLines: 2,
                  overflow: TextOverflow.ellipsis,
                ),
                Text(
                  currentGoal.status.displayName,
                  style: TextStyle(
                    fontSize: 14.sp,
                    color: currentGoal.status.color,
                    fontWeight: FontWeight.w500,
                  ),
                ),
              ],
            ),
          ),
          // Actions
          GestureDetector(
            onTap: _showEditGoal,
            child: Container(
              width: 44.w,
              height: 44.h,
              margin: EdgeInsets.only(right: 8.w),
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
              width: 44.w,
              height: 44.h,
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
      ),
    );
  }

  Widget _buildProgressCard() {
    return Container(
      height: 120.h,
      padding: EdgeInsets.all(24.w),
      decoration: BoxDecoration(
        color: AppTheme.cardBackground,
        borderRadius: BorderRadius.circular(16.r),
      ),
      child: Center(
        child: Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            Text(
              'Progress',
              style: TextStyle(
                fontSize: 16.sp,
                fontWeight: FontWeight.w600,
                color: AppTheme.textLight,
              ),
            ),
            Text(
              '${currentGoal.progressPercentage.round()}%',
              style: TextStyle(
                fontSize: 28.sp,
                fontWeight: FontWeight.w700,
                color: currentGoal.category.color,
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildQuickStatsRow() {
    return Row(
      children: [
        Expanded(
          child: _buildStatItem(
            'Priority',
            currentGoal.priority.displayName,
            currentGoal.priority.icon,
            currentGoal.priority.color,
          ),
        ),
        SizedBox(width: 12.w),
        Expanded(
          child: _buildStatItem(
            'Category',
            currentGoal.category.displayName,
            currentGoal.category.icon,
            currentGoal.category.color,
          ),
        ),
        SizedBox(width: 12.w),
        Expanded(
          child: _buildStatItem(
            currentGoal.isOverdue ? 'Overdue' : 'Days Left',
            currentGoal.isOverdue 
                ? '${currentGoal.daysUntilTarget.abs()}d'
                : '${currentGoal.daysUntilTarget}d',
            Icons.schedule,
            currentGoal.isOverdue ? Colors.red : AppTheme.textMedium,
          ),
        ),
      ],
    );
  }

  Widget _buildStatItem(String label, String value, IconData icon, Color color) {
    return Container(
      padding: EdgeInsets.all(16.w),
      decoration: BoxDecoration(
        color: AppTheme.cardBackground,
        borderRadius: BorderRadius.circular(12.r),
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
              fontSize: 14.sp,
              fontWeight: FontWeight.w700,
              color: color,
            ),
          ),
          Text(
            label,
            style: TextStyle(
              fontSize: 11.sp,
              color: AppTheme.textMedium,
            ),
            textAlign: TextAlign.center,
          ),
        ],
      ),
    );
  }

  Widget _buildDescriptionCard() {
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
              fontSize: 16.sp,
              fontWeight: FontWeight.w600,
              color: AppTheme.textLight,
            ),
          ),
          SizedBox(height: 12.h),
          Text(
            currentGoal.description,
            style: TextStyle(
              fontSize: 14.sp,
              color: AppTheme.textMedium,
              height: 1.5,
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildMilestonesCard() {
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
                  fontSize: 16.sp,
                  fontWeight: FontWeight.w600,
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
          
          // Milestones list or empty state
          if (currentGoal.milestones.isNotEmpty)
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
                      width: 20.w,
                      height: 20.h,
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
                        borderRadius: BorderRadius.circular(4.r),
                      ),
                      child: milestone.isCompleted
                          ? Icon(
                              Icons.check,
                              color: Colors.white,
                              size: 12.sp,
                            )
                          : null,
                    ),
                  ),
                  SizedBox(width: 12.w),
                  Expanded(
                    child: Text(
                      milestone.title,
                      style: TextStyle(
                        fontSize: 14.sp,
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

  Widget _buildTimelineCard() {
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
              fontSize: 16.sp,
              fontWeight: FontWeight.w600,
              color: AppTheme.textLight,
            ),
          ),
          SizedBox(height: 16.h),
          
          _buildTimelineItem(
            'Goal Created',
            currentGoal.createdAt,
            Icons.flag_outlined,
            AppTheme.textMedium,
            true,
          ),
          
          if (currentGoal.currentProgress > 0)
            _buildTimelineItem(
              'Progress Started',
              currentGoal.createdAt.add(Duration(days: 1)),
              Icons.play_arrow,
              currentGoal.category.color,
              true,
            ),
          
          _buildTimelineItem(
            'Target Date',
            currentGoal.targetDate,
            Icons.celebration_outlined,
            currentGoal.isCompleted ? Colors.green : AppTheme.textMedium,
            currentGoal.isCompleted,
          ),
        ],
      ),
    );
  }

  Widget _buildTimelineItem(
    String title, 
    DateTime date, 
    IconData icon, 
    Color color, 
    bool isCompleted
  ) {
    return Container(
      margin: EdgeInsets.only(bottom: 16.h),
      child: Row(
        children: [
          Container(
            width: 28.w,
            height: 28.h,
            decoration: BoxDecoration(
              color: isCompleted ? color : color.withOpacity(0.2),
              borderRadius: BorderRadius.circular(6.r),
            ),
            child: Icon(
              icon,
              color: isCompleted ? Colors.white : color,
              size: 14.sp,
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
                    fontSize: 14.sp,
                    fontWeight: FontWeight.w500,
                    color: AppTheme.textLight,
                  ),
                ),
                Text(
                  '${date.day}/${date.month}/${date.year}',
                  style: TextStyle(
                    fontSize: 12.sp,
                    color: AppTheme.textMedium,
                  ),
                ),
              ],
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
          
          _progressAnimation = Tween<double>(
            begin: _progressAnimation.value,
            end: newProgress / 100,
          ).animate(CurvedAnimation(
            parent: _progressController,
            curve: Curves.easeOut,
          ));
          
          _progressController.reset();
          _progressController.forward();
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
              Navigator.pop(context);
              Navigator.pop(context);
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

  void _toggleMilestone(int index) async {
    final milestone = currentGoal.milestones[index];
    final isNowCompleted = !milestone.isCompleted;
    
    print('🎯 Toggling milestone completion:');
    print('   Milestone: ${milestone.title}');
    print('   ID: ${milestone.id}');
    print('   Was completed: ${milestone.isCompleted}');
    print('   Now completed: $isNowCompleted');
    
    try {
      // Parse milestone ID
      final milestoneId = int.tryParse(milestone.id);
      if (milestoneId == null) {
        throw Exception('Invalid milestone ID: ${milestone.id}');
      }
      
      // Parse goal ID for the API call
      final goalId = int.tryParse(currentGoal.id);
      if (goalId == null) {
        throw Exception('Invalid goal ID: ${currentGoal.id}');
      }
      
      // Update milestone completion via API
      print('📡 Updating milestone completion via API...');
      print('   goalTrackerId: $goalId');
      print('   milestoneId: $milestoneId');
      print('   completedDate: ${isNowCompleted ? DateTime.now() : null}');
      
      // Try different milestone status types like in creation
      final statusTypesToTry = [
        3700001, // Milestone Active/Open
        3700002, // Milestone In Progress  
        3700003, // Milestone Pending
        GoalStatusTypes.open,       // Basic Open
        GoalStatusTypes.completed,  // Basic Complete
        1,       // Basic Active
        2,       // Basic Complete
      ];
      
      dynamic result;
      bool success = false;
      
      for (int i = 0; i < statusTypesToTry.length; i++) {
        final statusId = statusTypesToTry[i];
        print('🧪 Update attempt ${i + 1}: Trying statusTypeId: $statusId');
        
        result = await _goalTrackerService.updateMilestone(
          milestoneId: milestoneId,
          goalTrackerId: goalId,
          milestoneName: milestone.title, // Required: milestone name
          completedDate: isNowCompleted ? DateTime.now() : null,
          statusTypeId: statusId, // Try different status types
        );
        
        print('📡 Response for statusTypeId $statusId:');
        print('   isSuccess: ${result.isSuccess}');
        print('   message: ${result.message}');
        
        if (result.isSuccess) {
          print('✅ SUCCESS with statusTypeId: $statusId');
          success = true;
          break;
        } else {
          print('❌ Failed with statusTypeId $statusId, trying next...');
          if (i < statusTypesToTry.length - 1) {
            await Future.delayed(Duration(milliseconds: 300)); // Small delay between attempts
          }
        }
      }
      
      print('📡 Final API Response:');
      print('   isSuccess: ${result?.isSuccess}');
      print('   message: ${result?.message}');
      
      if (success && result != null && result.isSuccess) {
        print('✅ Milestone completion updated successfully in database');
        
        // Update local UI state
        final updatedMilestones = List<Milestone>.from(currentGoal.milestones);
        updatedMilestones[index] = updatedMilestones[index].copyWith(
          isCompleted: isNowCompleted,
          completedAt: isNowCompleted ? DateTime.now() : null,
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
        
        // Show success feedback and prompt for progress update if milestone was completed
        if (isNowCompleted) {
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(
              content: Text('Milestone completed!'),
              backgroundColor: Colors.green,
              duration: Duration(seconds: 2),
            ),
          );
          
          // Prompt user to update goal progress after milestone completion
          Future.delayed(Duration(milliseconds: 500), () {
            _promptProgressUpdateAfterMilestone();
          });
        } else {
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(
              content: Text('Milestone marked as incomplete'),
              backgroundColor: currentGoal.category.color,
              duration: Duration(seconds: 2),
            ),
          );
        }
      } else {
        print('❌ Failed to update milestone: ${result.message}');
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Failed to update milestone'),
            backgroundColor: Colors.red,
            duration: Duration(seconds: 2),
          ),
        );
      }
    } catch (e) {
      print('❌ Exception updating milestone: $e');
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Error updating milestone'),
          backgroundColor: Colors.red,
          duration: Duration(seconds: 2),
        ),
      );
    }
  }

  void _showAddMilestone() async {
    if (userId == null) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('User not loaded. Please try again.'),
          backgroundColor: Colors.red,
          behavior: SnackBarBehavior.fixed,
        ),
      );
      return;
    }

    showDialog(
      context: context,
      builder: (context) => MilestoneDialog(
        goalTrackerId: currentGoal.id,
        userId: userId,
        onSave: (apiMilestone) async {
          await _createMilestoneViaAPI(apiMilestone);
        },
      ),
    );
  }

  Future<void> _createMilestoneViaAPI(ApiMilestone apiMilestone) async {
    try {
      print('🎯 Creating milestone via API...');
      print('   Goal ID: ${currentGoal.id}');
      print('   Milestone Title: ${apiMilestone.title}');
      print('   Target Date: ${apiMilestone.targetDate}');
      print('   Description: ${apiMilestone.description}');
      print('   User ID: $userId');

      // Parse goal ID to int
      final goalId = int.tryParse(currentGoal.id);
      if (goalId == null) {
        throw Exception('Invalid goal ID: ${currentGoal.id}');
      }

      print('📡 Making API call to createMilestone...');
      print('   goalTrackerId: $goalId');
      print('   milestoneName: ${apiMilestone.title}');
      print('   startDate: ${DateTime.now()}');
      print('   endDate: ${apiMilestone.targetDate}');

      // Ensure end date is valid (not earlier than start date)
      final startDate = DateTime.now();
      DateTime? endDate = apiMilestone.targetDate;
      
      if (endDate != null) {
        if (endDate.isBefore(startDate)) {
          print('⚠️  Target date is in the past, setting to tomorrow...');
          // If target date is in the past, set to tomorrow
          endDate = startDate.add(Duration(days: 1));
        } else if (isSameDay(endDate, startDate)) {
          print('⚠️  Target date is today, setting to null to avoid constraints...');
          // If same day, make it null to avoid time conflicts
          endDate = null;
        }
      }
      
      print('📅 Final dates:');
      print('   startDate: $startDate');
      print('   endDate: $endDate');

      // Create milestone via service - try different milestone status types
      print('🔍 Trying milestone creation with different status types...');
      
      // Try common milestone status type IDs
      final statusTypesToTry = [
        3700001, // Milestone Active/Open
        3700002, // Milestone In Progress  
        3700003, // Milestone Pending
        1,       // Basic Active
        2,       // Basic Complete
        null,    // No status (let server default)
      ];
      
      dynamic result;
      
      for (int i = 0; i < statusTypesToTry.length; i++) {
        final statusId = statusTypesToTry[i];
        print('🧪 Attempt ${i + 1}: Trying statusTypeId: $statusId');
        
        result = await _goalTrackerService.createMilestone(
          goalTrackerId: goalId,
          milestoneName: apiMilestone.title,
          startDate: startDate,
          endDate: endDate,
          statusTypeId: statusId,
        );
        
        print('📡 Response for statusTypeId $statusId:');
        print('   isSuccess: ${result.isSuccess}');
        print('   message: ${result.message}');
        
        if (result.isSuccess) {
          print('✅ SUCCESS with statusTypeId: $statusId');
          break;
        } else {
          print('❌ Failed with statusTypeId $statusId, trying next...');
          if (i < statusTypesToTry.length - 1) {
            await Future.delayed(Duration(milliseconds: 500)); // Small delay between attempts
          }
        }
      }

      print('📡 Final API Response:');
      print('   isSuccess: ${result?.isSuccess}');
      print('   message: ${result?.message}');
      print('   data: ${result?.data}');

      if (result != null && result.isSuccess && result.data != null) {
        print('✅ Milestone created successfully: ${result.message}');
        
        // Convert API milestone to UI milestone
        final newMilestone = Milestone(
          id: result.data!.goalTrackerMileStoneId?.toString() ?? '',
          title: result.data!.goalTrackerMileStoneName ?? '',
          description: apiMilestone.description,
          isCompleted: result.data!.goalTrackerMileStoneCompletedDate != null,
          completedAt: result.data!.goalTrackerMileStoneCompletedDate,
          targetDate: result.data!.goalTrackerMileStoneEndDate,
        );

        // Add milestone to current goal
        final updatedMilestones = List<Milestone>.from(currentGoal.milestones);
        updatedMilestones.add(newMilestone);

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

        // Refresh goal data to ensure milestone count is updated everywhere
        await _refreshGoalData();

        // Show success message
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Milestone created successfully!'),
            backgroundColor: Colors.green,
            behavior: SnackBarBehavior.fixed,
          ),
        );
      } else {
        print('❌ Failed to create milestone: ${result?.message ?? "No result"}');
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Failed to create milestone'),
            backgroundColor: Colors.red,
            behavior: SnackBarBehavior.fixed,
          ),
        );
      }
    } catch (e) {
      print('❌ Exception creating milestone: $e');
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Error creating milestone'),
          backgroundColor: Colors.red,
          behavior: SnackBarBehavior.fixed,
        ),
      );
    }
  }

  bool isSameDay(DateTime date1, DateTime date2) {
    return date1.year == date2.year && 
           date1.month == date2.month && 
           date1.day == date2.day;
  }

  void _promptProgressUpdateAfterMilestone() {
    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        backgroundColor: AppTheme.cardBackground,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(16.r),
        ),
        title: Text(
          'Update Goal Progress?',
          style: TextStyle(
            color: AppTheme.textLight,
            fontSize: 18.sp,
            fontWeight: FontWeight.bold,
          ),
        ),
        content: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            Text(
              'You completed a milestone! Would you like to update your overall goal progress?',
              style: TextStyle(
                color: AppTheme.textMedium,
                fontSize: 14.sp,
              ),
            ),
            SizedBox(height: 12.h),
            Text(
              'Current progress: ${currentGoal.progressPercentage.round()}%',
              style: TextStyle(
                color: currentGoal.category.color,
                fontSize: 16.sp,
                fontWeight: FontWeight.w600,
              ),
            ),
          ],
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context),
            child: Text(
              'Not Now',
              style: TextStyle(color: AppTheme.textMedium),
            ),
          ),
          TextButton(
            onPressed: () {
              Navigator.pop(context);
              _showProgressUpdate();
            },
            child: Text(
              'Update Progress',
              style: TextStyle(color: currentGoal.category.color),
            ),
          ),
        ],
      ),
    );
  }

  Future<void> _refreshGoalData() async {
    try {
      print('🔄 Refreshing goal data after milestone creation...');
      
      // Parse goal ID
      final goalId = int.tryParse(currentGoal.id);
      if (goalId == null) {
        print('❌ Invalid goal ID for refresh: ${currentGoal.id}');
        return;
      }

      // Fetch fresh milestone data
      final milestoneResult = await _goalTrackerService.getMilestonesByGoalTracker(goalId);
      
      if (milestoneResult.isSuccess && milestoneResult.data != null) {
        print('✅ Refreshed milestone data: ${milestoneResult.data!.length} milestones');
        
        // Convert API milestones to UI milestones
        final refreshedMilestones = milestoneResult.data!.map((apiMilestone) => Milestone(
          id: apiMilestone.goalTrackerMileStoneId?.toString() ?? '',
          title: apiMilestone.goalTrackerMileStoneName ?? '',
          description: '',
          isCompleted: apiMilestone.goalTrackerMileStoneCompletedDate != null,
          completedAt: apiMilestone.goalTrackerMileStoneCompletedDate,
          targetDate: apiMilestone.goalTrackerMileStoneEndDate,
        )).toList();

        // Update current goal with fresh milestone data
        final refreshedGoal = Goal(
          id: currentGoal.id,
          title: currentGoal.title,
          description: currentGoal.description,
          category: currentGoal.category,
          priority: currentGoal.priority,
          targetDate: currentGoal.targetDate,
          createdAt: currentGoal.createdAt,
          milestones: refreshedMilestones,
          currentProgress: currentGoal.currentProgress,
          tags: currentGoal.tags,
        );

        setState(() {
          currentGoal = refreshedGoal;
        });

        widget.onGoalUpdated(refreshedGoal);
        print('✅ Goal data refreshed with ${refreshedMilestones.length} milestones');
      } else {
        print('❌ Failed to refresh milestone data: ${milestoneResult.message}');
      }
    } catch (e) {
      print('❌ Exception refreshing goal data: $e');
    }
  }
}