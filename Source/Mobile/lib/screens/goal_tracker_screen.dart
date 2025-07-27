import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:fun/services/unified_auth_service.dart';
import '../components/add_goal_sheet.dart';
import '../components/edit_goal_sheet.dart';
import '../components/goal_card.dart';
import '../components/progress_update_dialog.dart';
import '../models/goals.dart';
import '../models/goal_tracker_category.dart';
import '../constants/colors.dart';
import '../constants/goal_constants.dart';
import '../services/goal_tracker_service.dart';
import '../services/guided_tour_service.dart';
import '../api/generated/code/manifestation.swagger.dart';
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
    
    // Show guided tour after the widget is built
    WidgetsBinding.instance.addPostFrameCallback((_) {
      _showGuidedTourIfNeeded();
    });
    
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

  Future<void> _showGuidedTourIfNeeded() async {
    final tourService = GuidedTourService();
    await tourService.showTourIfNeeded(
      context,
      GuidedTourService.goalTrackerTourKey,
      tourService.getGoalTrackerTourSteps(),
    );
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
    print('\n🔍 === FILTERING GOALS ===');
    print('🔍 Selected category: ${selectedCategory.displayName}');
    print('🔍 Show completed: $showCompletedGoals');
    print('📊 Total goals before filtering: ${goals.length}');
    
    // Debug: Show all goals and their categories
    for (int i = 0; i < goals.length; i++) {
      final goal = goals[i];
      print('   Goal ${i + 1}: "${goal.title}" → Category: ${goal.category.displayName} (${goal.category.name})');
    }
    
    var filtered = goals;
    
    if (selectedCategory != GoalCategory.all) {
      final beforeCount = filtered.length;
      filtered = filtered.where((goal) => goal.category == selectedCategory).toList();
      print('📊 Category filter "${selectedCategory.displayName}": ${beforeCount} → ${filtered.length} goals');
      
      // Debug: Show which goals passed the category filter
      if (filtered.isNotEmpty) {
        print('📊 Goals in category "${selectedCategory.displayName}":');
        for (int i = 0; i < filtered.length; i++) {
          print('   ✓ ${filtered[i].title}');
        }
      } else {
        print('📊 No goals found in category "${selectedCategory.displayName}"');
      }
    }
    
    if (!showCompletedGoals) {
      final beforeCount = filtered.length;
      filtered = filtered.where((goal) => !goal.isCompleted).toList();
      print('📊 Completion filter: ${beforeCount} → ${filtered.length} goals');
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
    
    print('📊 FINAL: ${filtered.length} goals after filtering and sorting');
    print('🔍 === END FILTERING ===\n');
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
        
        // Load all user milestones once (much faster than per-goal API calls)
        print('📋 Loading all user milestones in one API call...');
        final allUserMilestones = await _loadAllUserMilestones();
        
        for (int i = 0; i < apiGoals.length; i++) {
          final apiGoal = apiGoals[i];
          print('🎯 Processing goal ${i + 1}: ${apiGoal.goalTrackerName} (ID: ${apiGoal.goalTrackerId})');
          
          // Get milestones for this specific goal from the loaded data
          final milestones = _getMilestonesForGoal(apiGoal, allUserMilestones);
          
          // Determine category from existingCategorys using only our supported default categories
          final goalCategory = _getGoalCategoryFromExistingCategories(apiGoal.existingCategorys, apiGoal.goalTrackerName ?? '');
          
          // Calculate progress based on completed milestones
          final progress = _calculateGoalProgress(milestones);
          
          print('🏷️  Goal "${apiGoal.goalTrackerName}" FINAL categorized as: ${goalCategory.displayName}');
          print('📊 Goal progress calculated: ${progress.toStringAsFixed(1)}%');
          
          convertedGoals.add(Goal(
            id: apiGoal.goalTrackerId?.toString() ?? '',
            title: apiGoal.goalTrackerName ?? '',
            description: '', // API doesn't have description field
            category: goalCategory,
            priority: GoalPriority.medium, // Default priority
            targetDate: apiGoal.goalTrackerEndDate ?? DateTime.now().add(Duration(days: 30)),
            createdAt: apiGoal.goalTrackerStartDate ?? DateTime.now(), // Use start date as creation date
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
        
        // Auto-associate existing goals that have no categories
        // Temporarily disabled due to server-side API issues
        // await _autoAssociateExistingGoalsWithCategories(apiGoals, convertedGoals);
        print('⚠️  Auto-association disabled due to server-side API issues with category validation');
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

  /// Get goal category from existingCategorys using database ID mapping
  GoalCategory _getGoalCategoryFromExistingCategories(List<CategoryModel>? existingCategorys, String goalName) {
    print('🏷️  Processing existingCategorys for goal: "$goalName"');
    print('🏷️  ExistingCategorys count: ${existingCategorys?.length ?? 0}');
    
    if (existingCategorys != null && existingCategorys.isNotEmpty) {
      // Get the first category and use its database ID for mapping
      final category = existingCategorys.first;
      final dbCategoryId = category.defaultCategoryId;
      
      if (dbCategoryId != null) {
        print('🏷️  Found database category ID: $dbCategoryId');
        
        // Use the service to map database ID to local category
        final localCategory = _goalTrackerService.getLocalCategoryFromDbId(dbCategoryId);
        if (localCategory != null) {
          final goalCategory = _mapLocalCategoryToGoalCategory(localCategory);
          print('🏷️  ✅ Mapped DB ID $dbCategoryId to: ${goalCategory.displayName}');
          return goalCategory;
        } else {
          print('🏷️  ⚠️  Unknown database category ID: $dbCategoryId');
        }
      }
    } else {
      print('🏷️  No existingCategorys found, using fallback logic');
    }
    
    // Fallback to goal name analysis if no categories found
    return _fallbackCategoryFromGoalName(goalName);
  }

  /// Map local category to GoalCategory enum
  GoalCategory _mapLocalCategoryToGoalCategory(GoalTrackerCategory localCategory) {
    switch (localCategory.name.toLowerCase()) {
      case 'career':
        return GoalCategory.career;
      case 'health':
        return GoalCategory.health;
      case 'love':
        return GoalCategory.relationships; // Note: relationships maps to love
      case 'money':
        return GoalCategory.financial; // Note: financial maps to money
      case 'personal':
        return GoalCategory.personal;
      case 'spiritual':
        return GoalCategory.spiritual;
      default:
        return GoalCategory.personal; // Default fallback
    }
  }

  /// Fallback method: determine category from goal name if no API category data
  GoalCategory _fallbackCategoryFromGoalName(String goalName) {
    final lowerName = goalName.toLowerCase().trim();
    print('🏷️  Fallback categorization for goal: "$goalName"');
    
    GoalCategory category;
    if (lowerName.contains('work') || lowerName.contains('career') || lowerName.contains('job') || lowerName.contains('business') || lowerName.contains('professional')) {
      category = GoalCategory.career;
    } else if (lowerName.contains('health') || lowerName.contains('fitness') || lowerName.contains('exercise') || lowerName.contains('wellness') || lowerName.contains('weight')) {
      category = GoalCategory.health;
    } else if (lowerName.contains('love') || lowerName.contains('relationship') || lowerName.contains('marriage') || lowerName.contains('dating') || lowerName.contains('romance') || lowerName.contains('partner')) {
      category = GoalCategory.relationships;
    } else if (lowerName.contains('money') || lowerName.contains('financial') || lowerName.contains('save') || lowerName.contains('income') || lowerName.contains('wealth') || lowerName.contains('invest')) {
      category = GoalCategory.financial;
    } else if (lowerName.contains('spiritual') || lowerName.contains('meditation') || lowerName.contains('mindful') || lowerName.contains('zen') || lowerName.contains('soul') || lowerName.contains('enlighten')) {
      category = GoalCategory.spiritual;
    } else {
      category = GoalCategory.personal;
    }
    
    print('🏷️  Fallback categorized as: ${category.displayName}');
    return category;
  }

  /// Load all user milestones in one API call (much more efficient)
  Future<List<GoalTrackerMileStoneModel>> _loadAllUserMilestones() async {
    try {
      print('🌐 Making single API call to get all user milestones...');
      final result = await _goalTrackerService.getUserMilestones();
      
      if (result.isSuccess && result.data != null) {
        print('✅ Loaded ${result.data!.length} total milestones for user');
        return result.data!;
      } else {
        print('❌ Failed to load user milestones: ${result.message}');
        return <GoalTrackerMileStoneModel>[];
      }
    } catch (e) {
      print('❌ Exception loading user milestones: $e');
      return <GoalTrackerMileStoneModel>[];
    }
  }

  /// Get milestones for a specific goal from the pre-loaded user milestones
  List<Milestone> _getMilestonesForGoal(GoalTrackerModel apiGoal, List<GoalTrackerMileStoneModel> allUserMilestones) {
    print('🎯 Finding milestones for goal ${apiGoal.goalTrackerName}...');
    
    // First, check if the goal has embedded milestones
    if (apiGoal.existingGoalTrackerMileStones != null && apiGoal.existingGoalTrackerMileStones!.isNotEmpty) {
      print('📋 Found ${apiGoal.existingGoalTrackerMileStones!.length} embedded milestones');
      return _convertApiMilestonesToUiMilestones(apiGoal.existingGoalTrackerMileStones!);
    }
    
    // If no embedded milestones, filter from the all-user-milestones data
    if (apiGoal.goalTrackerId != null) {
      final goalMilestones = allUserMilestones.where((milestone) {
        return milestone.goalTrackerId == apiGoal.goalTrackerId;
      }).toList();
      
      if (goalMilestones.isNotEmpty) {
        print('📋 Found ${goalMilestones.length} milestones from user milestone data');
        return _convertApiMilestonesToUiMilestones(goalMilestones);
      }
    }
    
    print('📋 No milestones found for this goal');
    return <Milestone>[];
  }

  /// Load milestones for a goal from embedded data or API call
  Future<List<Milestone>> _loadMilestonesForGoal(GoalTrackerModel apiGoal) async {
    print('🎯 Loading milestones for goal ${apiGoal.goalTrackerName}...');
    
    // First, try to use embedded milestones
    if (apiGoal.existingGoalTrackerMileStones != null && apiGoal.existingGoalTrackerMileStones!.isNotEmpty) {
      print('📋 Found ${apiGoal.existingGoalTrackerMileStones!.length} embedded milestones');
      return _convertApiMilestonesToUiMilestones(apiGoal.existingGoalTrackerMileStones!);
    }
    
    // If no embedded milestones, make API call to get them
    if (apiGoal.goalTrackerId != null) {
      print('🔍 No embedded milestones, fetching from API...');
      try {
        final result = await _goalTrackerService.getMilestonesByGoalTracker(apiGoal.goalTrackerId!);
        if (result.isSuccess && result.data != null) {
          print('📋 Fetched ${result.data!.length} milestones from API');
          return _convertApiMilestonesToUiMilestones(result.data!);
        } else {
          print('❌ Failed to fetch milestones: ${result.message}');
        }
      } catch (e) {
        print('❌ Exception fetching milestones: $e');
      }
    }
    
    print('📋 No milestones found for goal');
    return <Milestone>[];
  }

  /// Convert API GoalTrackerMileStoneModel to UI Milestone model
  List<Milestone> _convertApiMilestonesToUiMilestones(List<GoalTrackerMileStoneModel> apiMilestones) {
    return apiMilestones.map((apiMilestone) {
      final isCompleted = apiMilestone.goalTrackerMileStoneCompletedDate != null;
      
      return Milestone(
        id: apiMilestone.goalTrackerMileStoneId?.toString() ?? '',
        title: apiMilestone.goalTrackerMileStoneName ?? '',
        description: '', // API doesn't provide description
        isCompleted: isCompleted,
        completedAt: apiMilestone.goalTrackerMileStoneCompletedDate,
        targetDate: apiMilestone.goalTrackerMileStoneEndDate,
      );
    }).toList();
  }

  /// Calculate goal progress based on milestone completion
  double _calculateGoalProgress(List<Milestone> milestones) {
    if (milestones.isEmpty) {
      print('📊 No milestones found, progress = 0%');
      return 0.0;
    }
    
    final completedCount = milestones.where((m) => m.isCompleted).length;
    final totalCount = milestones.length;
    final progressPercentage = (completedCount / totalCount) * 100;
    
    print('📊 Progress: $completedCount/$totalCount milestones completed = ${progressPercentage.toStringAsFixed(1)}%');
    return progressPercentage;
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
      print('   goalStatusTypeId: ${GoalStatusTypes.open} (${GoalStatusTypes.open.goalStatusDisplayName})');
      print('   startDate: ${DateTime.now().toIso8601String()}');
      print('   endDate: ${targetDate.toIso8601String()}');
      
      // Get the actual authenticated user ID
      final currentUser = UnifiedAuthService().currentUser;
      final userId = currentUser?.userId ?? 1;
      print('🔍 Using authenticated user ID: $userId');
      print('🔍 User info: ${currentUser?.firstName} ${currentUser?.lastName} (${currentUser?.userName})');
      
      // Get category ID for the selected category (simple local mapping) 
      int? categoryId;
      if (category != GoalCategory.all) {
        print('🏷️  Getting category ID for: ${category.displayName}');
        categoryId = await _goalTrackerService.getCategoryIdFromLocalEnum(category.name);
        print('🏷️  Found category ID: $categoryId for ${category.displayName}');
      }
      
      // Create goal tracker via API (simple, no complex associations)
      final result = await _goalTrackerService.createGoalTracker(
        goalName: title,
        goalStatusTypeId: GoalStatusTypes.open,
        startDate: DateTime.now(),
        endDate: targetDate,
        userId: userId,
        categoryId: categoryId, // Simple single category ID for local handling
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
        print('✅ Goal created successfully (category $categoryId handled locally): ${result.message}');
        
        // Reload goals to get the updated list
        print('🔄 Reloading goals after creation...');
        await _loadGoals();
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
        print('✅ Goal marked as deleted successfully: ${result.message}');
        
        // Show success message
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
      } else {
        print('❌ Failed to mark goal as deleted: ${result.message}');
        
        // If it's a timeout, still try to reload goals in case it worked
        if (result.message.contains('timed out')) {
          print('🔄 Timeout occurred, reloading goals to check if deletion worked...');
          await _loadGoals();
          
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(
              content: Text('Delete request timed out - please check if goal was removed'),
              backgroundColor: Colors.orange,
              behavior: SnackBarBehavior.floating,
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(12.r),
              ),
            ),
          );
        } else {
          _showErrorSnackBar('Failed to delete goal: ${result.message}');
        }
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
              print('🗑️  Deleting goal: ${goal.title} (ID: ${goal.id})');
              
              // Delete goal using the service (mark as deleted)
              await _deleteGoalWithService(goal.id);
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


  /// Get API category name from GoalCategory enum
  String _getApiCategoryNameFromGoalCategory(GoalCategory category) {
    switch (category) {
      case GoalCategory.career:
        return 'career';
      case GoalCategory.health:
        return 'health';
      case GoalCategory.relationships:
        return 'love';
      case GoalCategory.financial:
        return 'money';
      case GoalCategory.personal:
        return 'personal';
      case GoalCategory.spiritual:
        return 'spiritual';
      case GoalCategory.all:
        return 'all';
    }
  }

  /// Auto-associate existing goals that have no categories with default categories
  Future<void> _autoAssociateExistingGoalsWithCategories(
    List<GoalTrackerModel> apiGoals,
    List<Goal> convertedGoals,
  ) async {
    print('\n🔄 Checking for goals that need category association...');
    
    int associationsAttempted = 0;
    for (int i = 0; i < apiGoals.length; i++) {
      final apiGoal = apiGoals[i];
      final convertedGoal = convertedGoals[i];
      
      // Check if this goal has no API categories
      if (apiGoal.existingCategorys == null || apiGoal.existingCategorys!.isEmpty) {
        associationsAttempted++;
        print('🏷️  Goal "${apiGoal.goalTrackerName}" has no categories');
        print('⚠️  Note: Goals should now be created with categories via newGoalTrackerCategoryModels');
        
        // TODO: Consider updating existing goals without categories
        // This would require a separate update API call with categoryIds
      }
    }
    
    if (associationsAttempted > 0) {
      print('🔄 Attempted to associate $associationsAttempted goals with categories');
      print('⚠️  Note: You may need to refresh/reload to see category changes');
    } else {
      print('✅ All goals already have category associations');
    }
  }

  /// Test method to try different API parameters to identify the issue
  Future<void> _testGoalCreationParameters() async {
    print('\n🧪 Testing goal creation with different parameters...');
    
    try {
      // Test with correct Open status
      print('Test 1: Using GoalStatusTypes.open');
      final testResult1 = await _goalTrackerService.createGoalTracker(
        goalName: 'Test Goal - Open Status',
        goalStatusTypeId: GoalStatusTypes.open,
        userId: 1,
      );
      print('   Result: ${testResult1.isSuccess} - ${testResult1.message}');
      
      // Test with Completed status
      print('Test 2: Using GoalStatusTypes.completed');
      final testResult2 = await _goalTrackerService.createGoalTracker(
        goalName: 'Test Goal - Completed Status',
        goalStatusTypeId: GoalStatusTypes.completed,
        userId: 1,
      );
      print('   Result: ${testResult2.isSuccess} - ${testResult2.message}');
      
      // Test with Abandoned status
      print('Test 3: Using GoalStatusTypes.abandoned');
      final testResult3 = await _goalTrackerService.createGoalTracker(
        goalName: 'Test Goal - Abandoned Status',
        goalStatusTypeId: GoalStatusTypes.abandoned,
        userId: 1,
      );
      print('   Result: ${testResult3.isSuccess} - ${testResult3.message}');
      
    } catch (e) {
      print('❌ Test error: $e');
    }
  }
}