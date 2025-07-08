import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:manifestation_app/vision_board.dart';
import 'package:manifestation_app/vision_board_screen.dart';
import 'colors.dart';

class VisionBoardHome extends StatefulWidget {
  const VisionBoardHome({super.key});

  @override
  _VisionBoardHomeState createState() => _VisionBoardHomeState();
}

class _VisionBoardHomeState extends State<VisionBoardHome> {
  List<VisionBoard> visionBoards = [];
  bool isLoading = false;

  @override
  void initState() {
    super.initState();
    _loadVisionBoards();
  }

  void _loadVisionBoards() {
    setState(() {
      isLoading = true;
    });
    
    // TODO: Load from storage
    // For now, create some sample boards
    visionBoards = [
      VisionBoard(
        id: '1',
        title: 'Dream Home',
        elements: [],
        createdAt: DateTime.now().subtract(Duration(days: 5)),
        updatedAt: DateTime.now().subtract(Duration(days: 2)),
        backgroundColor: AppColors.white,
      ),
      VisionBoard(
        id: '2',
        title: 'Career Goals',
        elements: [],
        createdAt: DateTime.now().subtract(Duration(days: 10)),
        updatedAt: DateTime.now().subtract(Duration(days: 1)),
        backgroundColor: AppColors.white,
      ),
      VisionBoard(
        id: '3',
        title: 'Travel Dreams',
        elements: [],
        createdAt: DateTime.now().subtract(Duration(days: 15)),
        updatedAt: DateTime.now().subtract(Duration(days: 3)),
        backgroundColor: AppColors.white,
      ),
    ];
    
    setState(() {
      isLoading = false;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.backgroundDark,
      body: SafeArea(
        child: Column(
          children: [
            _buildHeader(),
            Expanded(
              child: isLoading 
                  ? _buildLoadingState()
                  : visionBoards.isEmpty
                      ? _buildEmptyState()
                      : _buildVisionBoardGrid(),
            ),
          ],
        ),
      ),
      floatingActionButton: _buildCreateButton(),
    );
  }

  Widget _buildHeader() {
    return Container(
      padding: EdgeInsets.all(20.w),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
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
                      'Vision Boards',
                      style: TextStyle(
                        fontSize: 32.sp,
                        fontWeight: FontWeight.bold,
                        color: AppColors.textLight,
                      ),
                    ),
                    Text(
                      'Visualize your dreams',
                      style: TextStyle(
                        fontSize: 16.sp,
                        color: AppColors.textMedium,
                      ),
                    ),
                  ],
                ),
              ),
              _buildOptionsButton(),
            ],
          ),
          SizedBox(height: 20.h),
          _buildStatsRow(),
        ],
      ),
    );
  }

  Widget _buildOptionsButton() {
    return GestureDetector(
      onTap: _showOptionsMenu,
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
          Icons.more_vert,
          color: AppColors.textLight,
          size: 20.sp,
        ),
      ),
    );
  }

  Widget _buildStatsRow() {
    return Row(
      children: [
        _buildStatCard(
          icon: Icons.dashboard,
          title: 'Total Boards',
          value: '${visionBoards.length}',
          color: AppColors.visionBoardGradient[0],
        ),
        SizedBox(width: 12.w),
        _buildStatCard(
          icon: Icons.edit,
          title: 'This Week',
          value: '${visionBoards.where((b) => b.updatedAt.isAfter(DateTime.now().subtract(Duration(days: 7)))).length}',
          color: AppColors.visionBoardGradient[1],
        ),
        SizedBox(width: 12.w),
        _buildStatCard(
          icon: Icons.favorite,
          title: 'Favorites',
          value: '2', // TODO: Implement favorites
          color: AppColors.accentGold,
        ),
      ],
    );
  }

  Widget _buildStatCard({
    required IconData icon,
    required String title,
    required String value,
    required Color color,
  }) {
    return Expanded(
      child: Container(
        padding: EdgeInsets.all(16.w),
        decoration: BoxDecoration(
          color: AppColors.cardBackground,
          borderRadius: BorderRadius.circular(16.r),
          border: Border.all(
            color: AppColors.textMedium.withOpacity(0.1),
          ),
        ),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Container(
              width: 32.w,
              height: 32.h,
              decoration: BoxDecoration(
                color: color.withOpacity(0.1),
                borderRadius: BorderRadius.circular(8.r),
              ),
              child: Icon(
                icon,
                color: color,
                size: 16.sp,
              ),
            ),
            SizedBox(height: 8.h),
            Text(
              value,
              style: TextStyle(
                fontSize: 20.sp,
                fontWeight: FontWeight.bold,
                color: AppColors.textLight,
              ),
            ),
            Text(
              title,
              style: TextStyle(
                fontSize: 12.sp,
                color: AppColors.textMedium,
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildLoadingState() {
    return Center(
      child: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          CircularProgressIndicator(
            valueColor: AlwaysStoppedAnimation<Color>(AppColors.visionBoardGradient[0]),
          ),
          SizedBox(height: 16.h),
          Text(
            'Loading your vision boards...',
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
      child: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          Container(
            width: 120.w,
            height: 120.h,
            decoration: BoxDecoration(
              gradient: LinearGradient(
                colors: AppColors.visionBoardGradient,
                begin: Alignment.topLeft,
                end: Alignment.bottomRight,
              ),
              borderRadius: BorderRadius.circular(60.r),
            ),
            child: Icon(
              Icons.dashboard,
              color: AppColors.white,
              size: 60.sp,
            ),
          ),
          SizedBox(height: 24.h),
          Text(
            'No Vision Boards Yet',
            style: TextStyle(
              fontSize: 24.sp,
              fontWeight: FontWeight.bold,
              color: AppColors.textLight,
            ),
          ),
          SizedBox(height: 8.h),
          Text(
            'Create your first vision board to start\nmanifesting your dreams',
            style: TextStyle(
              fontSize: 16.sp,
              color: AppColors.textMedium,
              height: 1.5,
            ),
            textAlign: TextAlign.center,
          ),
          SizedBox(height: 32.h),
          _buildCreateBoardButton(),
        ],
      ),
    );
  }

  Widget _buildVisionBoardGrid() {
    return SingleChildScrollView(
      padding: EdgeInsets.symmetric(horizontal: 20.w),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          _buildSectionHeader('Recent', 'View all'),
          SizedBox(height: 16.h),
          GridView.builder(
            shrinkWrap: true,
            physics: NeverScrollableScrollPhysics(),
            gridDelegate: SliverGridDelegateWithFixedCrossAxisCount(
              crossAxisCount: 2,
              crossAxisSpacing: 16.w,
              mainAxisSpacing: 16.h,
              childAspectRatio: 0.75,
            ),
            itemCount: visionBoards.length,
            itemBuilder: (context, index) {
              final board = visionBoards[index];
              return _buildVisionBoardCard(board);
            },
          ),
          SizedBox(height: 32.h),
          _buildTemplatesSection(),
          SizedBox(height: 100.h), // Space for FAB
        ],
      ),
    );
  }

  Widget _buildSectionHeader(String title, String action) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.spaceBetween,
      children: [
        Text(
          title,
          style: TextStyle(
            fontSize: 20.sp,
            fontWeight: FontWeight.bold,
            color: AppColors.textLight,
          ),
        ),
        GestureDetector(
          onTap: () {
            // TODO: Navigate to all boards
          },
          child: Text(
            action,
            style: TextStyle(
              fontSize: 14.sp,
              color: AppColors.visionBoardGradient[0],
              fontWeight: FontWeight.w600,
            ),
          ),
        ),
      ],
    );
  }

  Widget _buildVisionBoardCard(VisionBoard board) {
    return GestureDetector(
      onTap: () => _openBoard(board),
      onLongPress: () => _showBoardOptions(board),
      child: Container(
        decoration: BoxDecoration(
          color: AppColors.cardBackground,
          borderRadius: BorderRadius.circular(20.r),
          border: Border.all(
            color: AppColors.textMedium.withOpacity(0.1),
          ),
          boxShadow: [
            BoxShadow(
              color: AppColors.black.withOpacity(0.05),
              blurRadius: 10.r,
              offset: Offset(0, 5.h),
            ),
          ],
        ),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Expanded(
              flex: 3,
              child: Container(
                width: double.infinity,
                decoration: BoxDecoration(
                  gradient: LinearGradient(
                    colors: [
                      AppColors.visionBoardGradient[0].withOpacity(0.1),
                      AppColors.visionBoardGradient[1].withOpacity(0.05),
                    ],
                    begin: Alignment.topLeft,
                    end: Alignment.bottomRight,
                  ),
                  borderRadius: BorderRadius.vertical(top: Radius.circular(20.r)),
                ),
                child: _buildPlaceholderThumbnail(), // Always show placeholder for now
              ),
            ),
            Expanded(
              flex: 2,
              child: Padding(
                padding: EdgeInsets.all(8.w),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    Row(
                      children: [
                        Expanded(
                          child: Text(
                            board.title,
                            style: TextStyle(
                              fontSize: 16.sp,
                              fontWeight: FontWeight.bold,
                              color: AppColors.textLight,
                            ),
                            maxLines: 1,
                            overflow: TextOverflow.ellipsis,
                          ),
                        ),
                        Container(
                          width: 24.w,
                          height: 24.h,
                          decoration: BoxDecoration(
                            color: AppColors.visionBoardGradient[0].withOpacity(0.1),
                            borderRadius: BorderRadius.circular(6.r),
                          ),
                          child: Icon(
                            Icons.more_vert,
                            color: AppColors.visionBoardGradient[0],
                            size: 14.sp,
                          ),
                        ),
                      ],
                    ),
                    SizedBox(height: 2.h),
                    Text(
                      '${board.elements.length} items • Last edited ${_formatDate(board.updatedAt)}',
                      style: TextStyle(
                        fontSize: 12.sp,
                        color: AppColors.textMedium,
                      ),
                      maxLines: 2,
                      overflow: TextOverflow.ellipsis,
                    ),
                    Spacer(),
                    Row(
                      children: [
                        Icon(
                          Icons.edit,
                          color: AppColors.textMedium.withOpacity(0.6),
                          size: 12.sp,
                        ),
                        SizedBox(width: 4.w),
                        Text(
                          _formatDate(board.updatedAt),
                          style: TextStyle(
                            fontSize: 10.sp,
                            color: AppColors.textMedium.withOpacity(0.6),
                          ),
                        ),
                        Spacer(),
                        Container(
                          padding: EdgeInsets.symmetric(horizontal: 6.w, vertical: 2.h),
                          decoration: BoxDecoration(
                            color: AppColors.visionBoardGradient[0].withOpacity(0.1),
                            borderRadius: BorderRadius.circular(4.r),
                          ),
                          child: Text(
                            '${board.elements.length} items',
                            style: TextStyle(
                              fontSize: 10.sp,
                              color: AppColors.visionBoardGradient[0],
                              fontWeight: FontWeight.w500,
                            ),
                          ),
                        ),
                      ],
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

  Widget _buildPlaceholderThumbnail() {
    return Container(
      width: double.infinity,
      height: double.infinity,
      child: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          Icon(
            Icons.dashboard,
            color: AppColors.visionBoardGradient[0].withOpacity(0.3),
            size: 40.sp,
          ),
          SizedBox(height: 8.h),
          Text(
            'Vision Board',
            style: TextStyle(
              fontSize: 12.sp,
              color: AppColors.textMedium.withOpacity(0.6),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildTemplatesSection() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        _buildSectionHeader('Templates', 'Browse all'),
        SizedBox(height: 16.h),
        Container(
          height: 120.h,
          child: ListView.builder(
            scrollDirection: Axis.horizontal,
            itemCount: 5,
            itemBuilder: (context, index) {
              return Container(
                width: 160.w,
                margin: EdgeInsets.only(right: 16.w),
                child: _buildTemplateCard(index),
              );
            },
          ),
        ),
      ],
    );
  }

  Widget _buildTemplateCard(int index) {
    final templates = [
      {'name': 'Dream Home', 'icon': Icons.home, 'color': AppColors.accentGold},
      {'name': 'Career Goals', 'icon': Icons.work, 'color': AppColors.secondaryTeal},
      {'name': 'Travel Dreams', 'icon': Icons.flight, 'color': AppColors.gradientPurple},
      {'name': 'Relationships', 'icon': Icons.favorite, 'color': AppColors.accentCoral},
      {'name': 'Health & Fitness', 'icon': Icons.fitness_center, 'color': AppColors.accentMint},
    ];

    final template = templates[index];
    
    return GestureDetector(
      onTap: () => _createFromTemplate(template['name'] as String),
      child: Container(
        decoration: BoxDecoration(
          color: AppColors.cardBackground,
          borderRadius: BorderRadius.circular(16.r),
          border: Border.all(
            color: AppColors.textMedium.withOpacity(0.1),
          ),
        ),
        child: Column(
          children: [
            Expanded(
              child: Container(
                width: double.infinity,
                decoration: BoxDecoration(
                  color: (template['color'] as Color).withOpacity(0.1),
                  borderRadius: BorderRadius.vertical(top: Radius.circular(16.r)),
                ),
                child: Icon(
                  template['icon'] as IconData,
                  color: template['color'] as Color,
                  size: 32.sp,
                ),
              ),
            ),
            Padding(
              padding: EdgeInsets.all(12.w),
              child: Text(
                template['name'] as String,
                style: TextStyle(
                  fontSize: 12.sp,
                  fontWeight: FontWeight.w600,
                  color: AppColors.textLight,
                ),
                textAlign: TextAlign.center,
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildCreateButton() {
    return Container(
      width: 56.w,
      height: 56.h,
      decoration: BoxDecoration(
        gradient: LinearGradient(
          colors: AppColors.visionBoardGradient,
          begin: Alignment.topLeft,
          end: Alignment.bottomRight,
        ),
        borderRadius: BorderRadius.circular(28.r),
        boxShadow: [
          BoxShadow(
            color: AppColors.visionBoardGradient[0].withOpacity(0.4),
            blurRadius: 20.r,
            offset: Offset(0, 10.h),
          ),
        ],
      ),
      child: Material(
        color: Colors.transparent,
        child: InkWell(
          onTap: _createNewBoard,
          borderRadius: BorderRadius.circular(28.r),
          child: Icon(
            Icons.add,
            color: AppColors.white,
            size: 28.sp,
          ),
        ),
      ),
    );
  }

  Widget _buildCreateBoardButton() {
    return Container(
      width: 200.w,
      height: 50.h,
      decoration: BoxDecoration(
        gradient: LinearGradient(
          colors: AppColors.visionBoardGradient,
          begin: Alignment.topLeft,
          end: Alignment.bottomRight,
        ),
        borderRadius: BorderRadius.circular(25.r),
        boxShadow: [
          BoxShadow(
            color: AppColors.visionBoardGradient[0].withOpacity(0.4),
            blurRadius: 20.r,
            offset: Offset(0, 10.h),
          ),
        ],
      ),
      child: Material(
        color: Colors.transparent,
        child: InkWell(
          onTap: _createNewBoard,
          borderRadius: BorderRadius.circular(25.r),
          child: Row(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              Icon(
                Icons.add,
                color: AppColors.white,
                size: 20.sp,
              ),
              SizedBox(width: 8.w),
              Text(
                'Create Vision Board',
                style: TextStyle(
                  fontSize: 14.sp,
                  fontWeight: FontWeight.w600,
                  color: AppColors.white,
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }

  String _formatDate(DateTime date) {
    final now = DateTime.now();
    final difference = now.difference(date);
    
    if (difference.inDays == 0) {
      return 'Today';
    } else if (difference.inDays == 1) {
      return 'Yesterday';
    } else if (difference.inDays < 7) {
      return '${difference.inDays} days ago';
    } else {
      return '${date.day}/${date.month}/${date.year}';
    }
  }

  void _createNewBoard() {
    Navigator.push(
      context,
      MaterialPageRoute(
        builder: (context) => VisionBoardScreen(),
      ),
    ).then((result) {
      if (result != null) {
        _loadVisionBoards(); // Refresh the list
      }
    });
  }

  void _openBoard(VisionBoard board) {
    Navigator.push(
      context,
      MaterialPageRoute(
        builder: (context) => VisionBoardScreen(existingBoard: board),
      ),
    ).then((result) {
      if (result != null) {
        _loadVisionBoards(); // Refresh the list
      }
    });
  }

  void _createFromTemplate(String templateName) {
    // TODO: Implement template creation
    print('Creating from template: $templateName');
    _createNewBoard();
  }

  void _showBoardOptions(VisionBoard board) {
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
                children: [
                  Text(
                    board.title,
                    style: TextStyle(
                      fontSize: 18.sp,
                      fontWeight: FontWeight.bold,
                      color: AppColors.textLight,
                    ),
                  ),
                  SizedBox(height: 24.h),
                  _buildOptionItem(Icons.edit, 'Edit', () {
                    Navigator.pop(context);
                    _openBoard(board);
                  }),
                  _buildOptionItem(Icons.copy, 'Duplicate', () {
                    Navigator.pop(context);
                    _duplicateBoard(board);
                  }),
                  _buildOptionItem(Icons.share, 'Share', () {
                    Navigator.pop(context);
                    _shareBoard(board);
                  }),
                  _buildOptionItem(Icons.delete, 'Delete', () {
                    Navigator.pop(context);
                    _deleteBoard(board);
                  }, isDestructive: true),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildOptionItem(IconData icon, String title, VoidCallback onTap, {bool isDestructive = false}) {
    return GestureDetector(
      onTap: onTap,
      child: Container(
        width: double.infinity,
        padding: EdgeInsets.symmetric(vertical: 16.h),
        child: Row(
          children: [
            Icon(
              icon,
              color: isDestructive ? Colors.red : AppColors.textMedium,
              size: 20.sp,
            ),
            SizedBox(width: 16.w),
            Text(
              title,
              style: TextStyle(
                fontSize: 16.sp,
                color: isDestructive ? Colors.red : AppColors.textLight,
              ),
            ),
          ],
        ),
      ),
    );
  }

  void _showOptionsMenu() {
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
                children: [
                  Text(
                    'Options',
                    style: TextStyle(
                      fontSize: 18.sp,
                      fontWeight: FontWeight.bold,
                      color: AppColors.textLight,
                    ),
                  ),
                  SizedBox(height: 24.h),
                  _buildOptionItem(Icons.sort, 'Sort & Filter', () {
                    Navigator.pop(context);
                    _showSortOptions();
                  }),
                  _buildOptionItem(Icons.cloud_sync, 'Sync Boards', () {
                    Navigator.pop(context);
                    _syncBoards();
                  }),
                  _buildOptionItem(Icons.settings, 'Settings', () {
                    Navigator.pop(context);
                    _showSettings();
                  }),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }

  void _duplicateBoard(VisionBoard board) {
    // TODO: Implement board duplication
    print('Duplicating board: ${board.title}');
  }

  void _shareBoard(VisionBoard board) {
    // TODO: Implement board sharing
    print('Sharing board: ${board.title}');
  }

  void _deleteBoard(VisionBoard board) {
    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        backgroundColor: AppColors.cardBackground,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(16.r),
        ),
        title: Text(
          'Delete Vision Board',
          style: TextStyle(
            color: AppColors.textLight,
            fontSize: 18.sp,
            fontWeight: FontWeight.bold,
          ),
        ),
        content: Text(
          'Are you sure you want to delete "${board.title}"? This action cannot be undone.',
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
                visionBoards.remove(board);
              });
              ScaffoldMessenger.of(context).showSnackBar(
                SnackBar(
                  content: Text('Vision board deleted'),
                  backgroundColor: AppColors.accentMint,
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

  void _showSortOptions() {
    // TODO: Implement sort options
    print('Show sort options');
  }

  void _syncBoards() {
    // TODO: Implement sync
    print('Sync boards');
  }

  void _showSettings() {
    // TODO: Implement settings
    print('Show settings');
  }
}