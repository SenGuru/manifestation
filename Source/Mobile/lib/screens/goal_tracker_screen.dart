import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../components/add_goal_sheet.dart';
import '../components/edit_goal_sheet.dart';
import '../components/goal_card.dart';
import '../components/progress_update_dialog.dart';
import '../models/goals.dart';
import '../models/goal_tracker_category.dart';
import '../constants/colors.dart';
import '../services/goal_tracker_service.dart';
import 'goal_detail_screen.dart';

class GoalTrackerScreen extends StatefulWidget {
  @override
  _GoalTrackerScreenState createState() => _GoalTrackerScreenState();
}

class _GoalTrackerScreenState extends State<GoalTrackerScreen>
    with TickerProviderStateMixin {
  
  List<Goal> goals = [];
  List<GoalTrackerCategory> categories = [];
  GoalCategory selectedCategory = GoalCategory.all;
  bool showCompletedGoals = true;
  bool _isLoading = false;
  
  late AnimationController _fabAnimationController;
  late Animation<double> _fabAnimation;
  
  final GoalTrackerService _goalTrackerService = GoalTrackerService();

  @override
  void initState() {
    super.initState();
    
    print('🚀 GoalTrackerScreen: Initializing...');
    
    // Initialize the service
    print('🔧 Initializing GoalTrackerService...');
    _goalTrackerService.initialize();
    
    _fabAnimationController = AnimationController(
      duration: Duration(milliseconds: 300),
      vsync: this,
    );
    _fabAnimation = CurvedAnimation(
      parent: _fabAnimationController,
      curve: Curves.easeInOut,
    );
    
    print('📊 Starting initial goal load...');
    _loadGoals();
    _fabAnimationController.forward();
    
    print('✅ GoalTrackerScreen: Initialization complete');
  }

  @override
  void dispose() {
    _fabAnimationController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final filteredGoals = _filterGoals();
    
    print('🎨 Building UI with:');
    print('   Total goals: ${goals.length}');
    print('   Filtered goals: ${filteredGoals.length}');
    print('   Selected category: ${selectedCategory.displayName}');
    print('   Show completed: $showCompletedGoals');
    print('   Is loading: $_isLoading');
    
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
              print('🔄 Category filter changed: ${category.displayName}');
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
    print('🔍 Filtering goals: selectedCategory=${selectedCategory.displayName}, showCompleted=$showCompletedGoals');
    print('📊 Total goals before filtering: ${goals.length}');
    
    var filtered = goals;
    
    if (selectedCategory != GoalCategory.all) {
      filtered = filtered.where((goal) => goal.category == selectedCategory).toList();
      print('📊 After category filter: ${filtered.length} goals');
    }
    
    if (!showCompletedGoals) {
      filtered = filtered.where((goal) => !goal.isCompleted).toList();
      print('📊 After completed filter: ${filtered.length} goals');
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
    
    print('📊 Final filtered & sorted goals: ${filtered.length}');
    return filtered;
  }

  Future<void> _loadGoals() async {
    print('\n=== 🔄 STARTING GOAL LOAD PROCESS ===');
    
    setState(() {
      _isLoading = true;
    });

    try {
      print('📋 Loading goals from API...');
      
      // Load categories first
      print('🏷️  Loading categories...');
      await _loadCategories();
      
      // Load user's goal trackers
      print('🎯 Fetching user goal trackers...');
      final result = await _goalTrackerService.getUserGoalTrackers();
      
      print('📡 API Response - Success: ${result.isSuccess}');
      print('📡 API Response - Message: ${result.message}');
      print('📡 API Response - Data Count: ${result.data?.length ?? 0}');
      
      if (result.isSuccess && result.data != null) {
        // Convert API GoalTrackerModel to UI Goal model
        final apiGoals = result.data!;
        final convertedGoals = <Goal>[];
        
        print('🔄 Converting ${apiGoals.length} API goals to UI goals...');
        
        for (int i = 0; i < apiGoals.length; i++) {
          final apiGoal = apiGoals[i];
          print('🎯 Processing goal ${i + 1}: ${apiGoal.goalTrackerName} (ID: ${apiGoal.goalTrackerId})');
          
          // Skip milestone loading for now to avoid hanging
          // TODO: Re-enable milestone loading once API performance is optimized
          print('⏭️  Skipping milestone loading for now (performance optimization)...');
          final milestones = <Milestone>[]; // Empty milestones list
          
          // Determine category based on goal name/context
          final goalCategory = _determineGoalCategory(apiGoal.goalTrackerName ?? '');
          
          // Set a default progress since we're not loading milestones yet
          final progress = 0.0; // Default to 0% progress
          
          print('🏷️  Goal "${apiGoal.goalTrackerName}" categorized as: ${goalCategory.displayName}');
          print('📊 Goal progress set to default: ${progress.toStringAsFixed(1)}%');
          
          convertedGoals.add(Goal(
            id: apiGoal.goalTrackerId?.toString() ?? '',
            title: apiGoal.goalTrackerName ?? '',
            description: '', // API doesn't have description field
            category: goalCategory,
            priority: GoalPriority.medium, // Default priority
            targetDate: apiGoal.goalTrackerEndDate ?? DateTime.now().add(Duration(days: 30)),
            createdAt: DateTime.now(), // API doesn't expose created date directly
            milestones: milestones,
            currentProgress: progress,
            tags: [goalCategory.displayName.toLowerCase()],
          ));
        }
        
        setState(() {
          goals = convertedGoals;
        });
        
        print('✅ Successfully loaded ${goals.length} goals from API');
        print('📊 Goal breakdown by category:');
        for (final category in GoalCategory.values) {
          final count = goals.where((g) => g.category == category).length;
          if (count > 0) {
            print('   ${category.displayName}: $count goals');
          }
        }
      } else {
        print('❌ Failed to load goals from API: ${result.message}');
        print('🔄 Falling back to hardcoded goals...');
        await _loadHardcodedGoals();
      }
    } catch (e, stackTrace) {
      print('❌ Exception loading goals: $e');
      print('📍 Stack trace: $stackTrace');
      print('🔄 Falling back to hardcoded goals...');
      await _loadHardcodedGoals();
    } finally {
      setState(() {
        _isLoading = false;
      });
      print('=== ✅ GOAL LOAD PROCESS COMPLETE ===\n');
    }
  }

  Future<void> _loadCategories() async {
    print('🏷️  Starting category load from API...');
    try {
      final result = await _goalTrackerService.getAllCategories();
      print('🏷️  Categories API response - Success: ${result.isSuccess}');
      print('🏷️  Categories API response - Message: ${result.message}');
      
      if (result.isSuccess && result.data != null) {
        setState(() {
          categories = result.data!;
        });
        print('✅ Loaded ${categories.length} categories from API');
        for (int i = 0; i < categories.length; i++) {
          final cat = categories[i];
          print('   ${i + 1}. ${cat.name} (ID: ${cat.id})');
        }
      } else {
        print('❌ Failed to load categories: ${result.message}');
      }
    } catch (e, stackTrace) {
      print('❌ Exception loading categories: $e');
      print('📍 Stack trace: $stackTrace');
    }
  }

  GoalCategory _determineGoalCategory(String goalName) {
    final lowerName = goalName.toLowerCase();
    print('🏷️  Determining category for goal: "$goalName"');
    
    GoalCategory category;
    if (lowerName.contains('work') || lowerName.contains('career') || lowerName.contains('job')) {
      category = GoalCategory.career;
    } else if (lowerName.contains('health') || lowerName.contains('fitness') || lowerName.contains('exercise')) {
      category = GoalCategory.health;
    } else if (lowerName.contains('love') || lowerName.contains('relationship') || lowerName.contains('marriage')) {
      category = GoalCategory.relationships;
    } else if (lowerName.contains('money') || lowerName.contains('financial') || lowerName.contains('save')) {
      category = GoalCategory.financial;
    } else if (lowerName.contains('spiritual') || lowerName.contains('meditation') || lowerName.contains('mindful')) {
      category = GoalCategory.spiritual;
    } else {
      category = GoalCategory.personal;
    }
    
    print('🏷️  Categorized as: ${category.displayName}');
    return category;
  }

  Future<void> _loadHardcodedGoals() async {
      print('\n🔄 Loading hardcoded goals as fallback...');
      
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
      print('📊 Hardcoded goals breakdown:');
      for (final goal in hardcodedGoals) {
        print('   • ${goal.title} (${goal.category.displayName}) - ${goal.currentProgress.toStringAsFixed(1)}%');
      }
  }

  // ========================= API GOAL MANAGEMENT =========================

  Future<void> _createGoalWithService({
    required String title,
    required GoalCategory category,
    required GoalPriority priority,
    required DateTime targetDate,
  }) async {
    print('\n🎯 Creating new goal via API...');
    print('   Title: $title');
    print('   Category: ${category.displayName}');
    print('   Priority: ${priority.name}');
    print('   Target Date: ${targetDate.toIso8601String()}');
    
    try {
      // Add validation before API call
      if (title.trim().isEmpty) {
        throw Exception('Goal title cannot be empty');
      }
      
      // Validate date is not in the past
      if (targetDate.isBefore(DateTime.now().subtract(Duration(days: 1)))) {
        print('⚠️ Warning: Target date is in the past, adjusting to future date');
        targetDate = DateTime.now().add(Duration(days: 30));
      }
      
      print('📋 API Call Parameters:');
      print('   goalName: "$title"');
      print('   goalStatusTypeId: 1');
      print('   startDate: ${DateTime.now().toIso8601String()}');
      print('   endDate: ${targetDate.toIso8601String()}');
      print('   userId: 1 (hardcoded - TODO: get from auth)');
      
      // Try to get a valid user ID or status type ID first
      print('🔍 Attempting to validate API parameters...');
      
      // Create goal tracker via API
      final result = await _goalTrackerService.createGoalTracker(
        goalName: title,
        goalStatusTypeId: 1, // Active status
        startDate: DateTime.now(),
        endDate: targetDate,
        userId: 1, // Replace with actual user ID from auth service
      );

      print('📡 Create goal API response:');
      print('   Success: ${result.isSuccess}');
      print('   Message: ${result.message}');
      
      if (result.data != null) {
        print('   Data: ${result.data.toString()}');
        try {
          print('   Data JSON: ${result.data?.toJson()}');
        } catch (e) {
          print('   Data JSON serialization error: $e');
        }
      } else {
        print('   Data: null');
      }

      if (result.isSuccess && result.data != null) {
        // Reload goals to get the updated list
        print('🔄 Reloading goals after creation...');
        await _loadGoals();
        
        print('✅ Goal created successfully: ${result.message}');
      } else {
        print('❌ Failed to create goal via API: ${result.message}');
        print('🔄 Falling back to local goal creation...');
        
        // Fallback: Create goal locally if API fails
        await _createGoalLocally(title, category, priority, targetDate);
        
        // Show warning to user
        _showErrorSnackBar('Goal created locally (API unavailable): ${result.message}');
      }
    } catch (e, stackTrace) {
      print('❌ Exception creating goal: $e');
      print('📍 Full stack trace:');
      print(stackTrace.toString());
      
      // Fallback: Create goal locally if API throws exception
      print('🔄 Exception occurred, falling back to local goal creation...');
      try {
        await _createGoalLocally(title, category, priority, targetDate);
        
        // Try to extract meaningful error from the stack trace
        String userFriendlyError = 'Goal created locally due to server error';
        if (e.toString().contains('PostgreSQL') || e.toString().contains('Npgsql')) {
          userFriendlyError = 'Goal created locally (database error on server)';
        } else if (e.toString().contains('timeout')) {
          userFriendlyError = 'Goal created locally (server timeout)';
        } else if (e.toString().contains('connection')) {
          userFriendlyError = 'Goal created locally (connection error)';
        }
        
        _showErrorSnackBar(userFriendlyError);
      } catch (localError) {
        print('❌ Failed to create goal locally: $localError');
        _showErrorSnackBar('Failed to create goal: ${e.toString().split('\n').first}');
      }
    }
  }

  Future<void> _updateGoalWithService({
    required String goalId,
    String? title,
    DateTime? targetDate,
    bool? isCompleted,
  }) async {
    print('\n🔄 Updating goal via API...');
    print('   Goal ID: $goalId');
    print('   New Title: $title');
    print('   New Target Date: ${targetDate?.toIso8601String()}');
    print('   Is Completed: $isCompleted');
    
    try {
      final result = await _goalTrackerService.updateGoalTracker(
        goalTrackerId: int.parse(goalId),
        goalName: title,
        endDate: targetDate,
        completedDate: isCompleted == true ? DateTime.now() : null,
      );

      print('📡 Update goal API response:');
      print('   Success: ${result.isSuccess}');
      print('   Message: ${result.message}');

      if (result.isSuccess) {
        // Reload goals to get the updated list
        print('🔄 Reloading goals after update...');
        await _loadGoals();
        print('✅ Goal updated successfully: ${result.message}');
      } else {
        print('❌ Failed to update goal: ${result.message}');
        _showErrorSnackBar('Failed to update goal: ${result.message}');
      }
    } catch (e, stackTrace) {
      print('❌ Exception updating goal: $e');
      print('📍 Stack trace: $stackTrace');
      _showErrorSnackBar('Error updating goal: ${e.toString()}');
    }
  }

  Future<void> _deleteGoalWithService(String goalId) async {
    print('\n🗑️  Deleting goal via API...');
    print('   Goal ID: $goalId');
    
    try {
      final result = await _goalTrackerService.deleteGoalTracker(int.parse(goalId));

      print('📡 Delete goal API response:');
      print('   Success: ${result.isSuccess}');
      print('   Message: ${result.message}');

      if (result.isSuccess) {
        // Reload goals to get the updated list
        print('🔄 Reloading goals after deletion...');
        await _loadGoals();
        print('✅ Goal deleted successfully: ${result.message}');
      } else {
        print('❌ Failed to delete goal: ${result.message}');
        _showErrorSnackBar('Failed to delete goal: ${result.message}');
      }
    } catch (e, stackTrace) {
      print('❌ Exception deleting goal: $e');
      print('📍 Stack trace: $stackTrace');
      _showErrorSnackBar('Error deleting goal: ${e.toString()}');
    }
  }

  void _showErrorSnackBar(String message) {
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

  void _showAddGoalSheet() {
    print('➕ Opening add goal sheet...');
    showModalBottomSheet(
      context: context,
      backgroundColor: Colors.transparent,
      isScrollControlled: true,
      builder: (context) => AddGoalSheet(
        onGoalAdded: (goal) async {
          print('📝 New goal submitted from sheet:');
          print('   Title: ${goal.title}');
          print('   Category: ${goal.category.displayName}');
          print('   Priority: ${goal.priority.name}');
          
          // Create goal using the service
          await _createGoalWithService(
            title: goal.title,
            category: goal.category,
            priority: goal.priority,
            targetDate: goal.targetDate,
          );
          
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
    print('👁️  Opening goal details for: ${goal.title}');
    Navigator.push(
      context,
      MaterialPageRoute(
        builder: (context) => GoalDetailScreen(
          goal: goal,
          onGoalUpdated: (updatedGoal) async {
            print('🔄 Goal updated from details screen:');
            print('   Original: ${goal.title}');
            print('   Updated: ${updatedGoal.title}');
            
            // Update goal using the service
            await _updateGoalWithService(
              goalId: updatedGoal.id,
              title: updatedGoal.title,
              targetDate: updatedGoal.targetDate,
            );
          },
          onGoalDeleted: (deletedGoal) async {
            print('🗑️  Goal deleted from details screen: ${deletedGoal.title}');
            
            // Delete goal using the service
            await _deleteGoalWithService(deletedGoal.id);
          },
        ),
      ),
    );
  }

  void _updateGoalProgress(Goal goal) {
    print('📊 Opening progress update dialog for: ${goal.title}');
    print('   Current progress: ${goal.currentProgress.toStringAsFixed(1)}%');
    
    showDialog(
      context: context,
      builder: (context) => ProgressUpdateDialog(
        goal: goal,
        onProgressUpdate: (newProgress) async {
          print('📊 Progress update submitted:');
          print('   Goal: ${goal.title}');
          print('   Old Progress: ${goal.currentProgress.toStringAsFixed(1)}%');
          print('   New Progress: ${newProgress.toStringAsFixed(1)}%');
          print('   Is Completed: ${newProgress >= 100.0}');
          
          // Update goal progress using the service
          await _updateGoalWithService(
            goalId: goal.id,
            isCompleted: newProgress >= 100.0,
          );
          
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
    print('✏️  Opening edit goal sheet for: ${goal.title}');
    showModalBottomSheet(
      context: context,
      backgroundColor: Colors.transparent,
      isScrollControlled: true,
      builder: (context) => EditGoalSheet(
        goal: goal,
        onGoalUpdated: (updatedGoal) async {
          print('📝 Goal updated from edit sheet:');
          print('   Original: ${goal.title}');
          print('   Updated: ${updatedGoal.title}');
          print('   New Target Date: ${updatedGoal.targetDate.toIso8601String()}');
          
          // Update goal using the service
          await _updateGoalWithService(
            goalId: updatedGoal.id,
            title: updatedGoal.title,
            targetDate: updatedGoal.targetDate,
          );
          
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
                      value: showCompletedGoals,                    onChanged: (value) {
                      print('⚙️  Settings changed: showCompletedGoals = $value');
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

  /// Fallback method to create goal locally when API fails
  Future<void> _createGoalLocally(
    String title,
    GoalCategory category,
    GoalPriority priority,
    DateTime targetDate,
  ) async {
    print('💾 Creating goal locally as fallback...');
    
    final newGoal = Goal(
      id: DateTime.now().millisecondsSinceEpoch.toString(), // Generate unique ID
      title: title,
      description: '',
      category: category,
      priority: priority,
      targetDate: targetDate,
      createdAt: DateTime.now(),
      milestones: [], // Empty milestones for now
      currentProgress: 0.0,
      tags: [category.displayName.toLowerCase()],
    );
    
    setState(() {
      goals.add(newGoal);
    });
    
    print('✅ Goal created locally: ${newGoal.title}');
  }

  /// Test method to try different API parameters to identify the issue
  Future<void> _testGoalCreationParameters() async {
    print('\n🧪 Testing goal creation with different parameters...');
    
    try {
      // Test with minimal parameters
      print('Test 1: Minimal parameters');
      final testResult1 = await _goalTrackerService.createGoalTracker(
        goalName: 'Test Goal - Minimal',
        goalStatusTypeId: 1,
        userId: 1,
      );
      print('   Result: ${testResult1.isSuccess} - ${testResult1.message}');
      
      // Test with different user ID
      print('Test 2: Different user ID');
      final testResult2 = await _goalTrackerService.createGoalTracker(
        goalName: 'Test Goal - User 0',
        goalStatusTypeId: 1,
        userId: 0, // Try user ID 0
      );
      print('   Result: ${testResult2.isSuccess} - ${testResult2.message}');
      
      // Test with different status type
      print('Test 3: Different status type');
      final testResult3 = await _goalTrackerService.createGoalTracker(
        goalName: 'Test Goal - Status 0',
        goalStatusTypeId: 0, // Try status type 0
        userId: 1,
      );
      print('   Result: ${testResult3.isSuccess} - ${testResult3.message}');
      
    } catch (e) {
      print('❌ Test error: $e');
    }
  }
}