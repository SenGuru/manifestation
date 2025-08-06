import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../models/vision_board.dart';
import '../services/vision_board_service.dart';
import 'vision_board_screen.dart';
import '../constants/colors.dart';
import '../constants/app_theme.dart';
import '../widgets/stylish_loading.dart';

class VisionBoardsListScreen extends StatefulWidget {
  const VisionBoardsListScreen({super.key});

  @override
  _VisionBoardsListScreenState createState() => _VisionBoardsListScreenState();
}

class _VisionBoardsListScreenState extends State<VisionBoardsListScreen> {
  List<VisionBoard> visionBoards = [];
  List<VisionBoard> filteredBoards = [];
  bool isLoading = false;
  bool isLoadingMore = false;
  String searchQuery = '';
  String sortBy = 'recent'; // recent, name, oldest
  
  final ScrollController _scrollController = ScrollController();
  final TextEditingController _searchController = TextEditingController();
  
  // Pagination
  static const int itemsPerPage = 10;
  int currentPage = 0;
  bool hasMoreItems = true;

  @override
  void initState() {
    super.initState();
    _loadVisionBoards();
    _scrollController.addListener(_onScroll);
  }

  @override
  void dispose() {
    _scrollController.dispose();
    _searchController.dispose();
    super.dispose();
  }

  void _onScroll() {
    if (_scrollController.position.pixels >= _scrollController.position.maxScrollExtent - 200) {
      if (!isLoadingMore && hasMoreItems) {
        _loadMoreBoards();
      }
    }
  }

  Future<void> _loadVisionBoards() async {
    setState(() {
      isLoading = true;
      currentPage = 0;
      hasMoreItems = true;
    });
    
    try {
      final visionBoardService = VisionBoardService();
      await visionBoardService.initialize(baseUrl: 'http://manifestation-qa-1.senthilguru.net');
      
      final result = await visionBoardService.getUserVisionBoards();
      
      if (result.isSuccess) {
        visionBoards = result.data ?? [];
        _applyFilters();
      } else {
        print('❌ Failed to load vision boards: ${result.message}');
        visionBoards = [];
        filteredBoards = [];
      }
    } catch (e) {
      print('❌ Error loading vision boards: $e');
      visionBoards = [];
      filteredBoards = [];
    }
    
    setState(() {
      isLoading = false;
    });
  }

  Future<void> _loadMoreBoards() async {
    if (isLoadingMore || !hasMoreItems) return;
    
    setState(() {
      isLoadingMore = true;
    });
    
    // Simulate pagination by showing more items from the filtered list
    await Future.delayed(Duration(milliseconds: 500));
    
    final nextPageStart = (currentPage + 1) * itemsPerPage;
    if (nextPageStart >= filteredBoards.length) {
      hasMoreItems = false;
    } else {
      currentPage++;
    }
    
    setState(() {
      isLoadingMore = false;
    });
  }

  void _applyFilters() {
    List<VisionBoard> filtered = List.from(visionBoards);
    
    // Apply search filter
    if (searchQuery.isNotEmpty) {
      filtered = filtered.where((board) => 
        board.title.toLowerCase().contains(searchQuery.toLowerCase())
      ).toList();
    }
    
    // Apply sorting
    switch (sortBy) {
      case 'recent':
        filtered.sort((a, b) => b.updatedAt.compareTo(a.updatedAt));
        break;
      case 'oldest':
        filtered.sort((a, b) => a.updatedAt.compareTo(b.updatedAt));
        break;
      case 'name':
        filtered.sort((a, b) => a.title.compareTo(b.title));
        break;
    }
    
    setState(() {
      filteredBoards = filtered;
      currentPage = 0;
      hasMoreItems = filtered.length > itemsPerPage;
    });
  }

  List<VisionBoard> get displayedBoards {
    final endIndex = ((currentPage + 1) * itemsPerPage).clamp(0, filteredBoards.length);
    return filteredBoards.take(endIndex).toList();
  }

  @override
  Widget build(BuildContext context) {
    if (isLoading) {
      return Scaffold(
        backgroundColor: AppTheme.backgroundPrimary,
        body: StylishLoading(
          message: 'Loading all vision boards...',
        ),
      );
    }

    return Scaffold(
      backgroundColor: AppTheme.backgroundPrimary,
      body: SafeArea(
        child: Column(
          children: [
            _buildHeader(),
            _buildSearchAndFilter(),
            Expanded(
              child: filteredBoards.isEmpty
                  ? _buildEmptyState()
                  : _buildBoardsList(),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildHeader() {
    return Container(
      padding: EdgeInsets.all(20.w),
      child: Row(
        children: [
          GestureDetector(
            onTap: () => Navigator.pop(context),
            child: Container(
              width: 44.w,
              height: 44.h,
              decoration: BoxDecoration(
                color: AppTheme.cardBackground,
                borderRadius: BorderRadius.circular(12.r),
                border: Border.all(
                  color: AppTheme.textSecondary.withValues(alpha: 0.1),
                ),
              ),
              child: Icon(
                Icons.arrow_back,
                color: AppTheme.textPrimary,
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
                  'All Vision Boards',
                  style: TextStyle(
                    fontSize: 24.sp,
                    fontWeight: FontWeight.bold,
                    color: AppTheme.textPrimary,
                  ),
                ),
                Text(
                  '${filteredBoards.length} boards',
                  style: TextStyle(
                    fontSize: 14.sp,
                    color: AppTheme.textSecondary,
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildSearchAndFilter() {
    return Container(
      padding: EdgeInsets.symmetric(horizontal: 20.w),
      child: Column(
        children: [
          // Search bar
          Container(
            decoration: BoxDecoration(
              color: AppTheme.cardBackground,
              borderRadius: BorderRadius.circular(16.r),
              border: Border.all(
                color: AppTheme.textSecondary.withValues(alpha: 0.1),
              ),
            ),
            child: TextField(
              controller: _searchController,
              onChanged: (value) {
                setState(() {
                  searchQuery = value;
                });
                _applyFilters();
              },
              decoration: InputDecoration(
                hintText: 'Search vision boards...',
                hintStyle: TextStyle(
                  color: AppTheme.textSecondary,
                  fontSize: 16.sp,
                ),
                prefixIcon: Icon(
                  Icons.search,
                  color: AppTheme.textSecondary,
                  size: 20.sp,
                ),
                border: InputBorder.none,
                contentPadding: EdgeInsets.symmetric(
                  horizontal: 16.w,
                  vertical: 16.h,
                ),
              ),
              style: TextStyle(
                color: AppTheme.textPrimary,
                fontSize: 16.sp,
              ),
            ),
          ),
          SizedBox(height: 16.h),
          // Sort options
          Row(
            children: [
              Text(
                'Sort by:',
                style: TextStyle(
                  fontSize: 14.sp,
                  color: AppTheme.textSecondary,
                  fontWeight: FontWeight.w500,
                ),
              ),
              SizedBox(width: 12.w),
              Expanded(
                child: Row(
                  children: [
                    _buildSortChip('Recent', 'recent'),
                    SizedBox(width: 8.w),
                    _buildSortChip('Name', 'name'),
                    SizedBox(width: 8.w),
                    _buildSortChip('Oldest', 'oldest'),
                  ],
                ),
              ),
            ],
          ),
          SizedBox(height: 20.h),
        ],
      ),
    );
  }

  Widget _buildSortChip(String label, String value) {
    final isSelected = sortBy == value;
    return GestureDetector(
      onTap: () {
        setState(() {
          sortBy = value;
        });
        _applyFilters();
      },
      child: Container(
        padding: EdgeInsets.symmetric(horizontal: 12.w, vertical: 6.h),
        decoration: BoxDecoration(
          color: isSelected 
            ? AppTheme.visionBoardGradient[0].withValues(alpha: 0.1)
            : AppTheme.cardBackground,
          borderRadius: BorderRadius.circular(20.r),
          border: Border.all(
            color: isSelected 
              ? AppTheme.visionBoardGradient[0]
              : AppTheme.textSecondary.withValues(alpha: 0.2),
          ),
        ),
        child: Text(
          label,
          style: TextStyle(
            fontSize: 12.sp,
            color: isSelected 
              ? AppTheme.visionBoardGradient[0]
              : AppTheme.textSecondary,
            fontWeight: FontWeight.w500,
          ),
        ),
      ),
    );
  }

  Widget _buildBoardsList() {
    return ListView.builder(
      controller: _scrollController,
      padding: EdgeInsets.symmetric(horizontal: 20.w),
      itemCount: displayedBoards.length + (isLoadingMore ? 1 : 0),
      itemBuilder: (context, index) {
        if (index == displayedBoards.length) {
          return Container(
            padding: EdgeInsets.all(20.w),
            child: Center(
              child: CompactStylishLoading(
                color: AppTheme.visionBoardGradient[0],
                size: 20.w,
              ),
            ),
          );
        }
        
        final board = displayedBoards[index];
        return Container(
          margin: EdgeInsets.only(bottom: 16.h),
          child: _buildVisionBoardListItem(board),
        );
      },
    );
  }

  Widget _buildVisionBoardListItem(VisionBoard board) {
    return GestureDetector(
      onTap: () => _openBoard(board),
      onLongPress: () => _showBoardOptions(board),
      child: Container(
        padding: EdgeInsets.all(16.w),
        decoration: BoxDecoration(
          color: AppTheme.cardBackground,
          borderRadius: BorderRadius.circular(16.r),
          border: Border.all(
            color: AppTheme.textSecondary.withValues(alpha: 0.1),
          ),
          boxShadow: [
            BoxShadow(
              color: AppTheme.black.withValues(alpha: 0.05),
              blurRadius: 10.r,
              offset: Offset(0, 2.h),
            ),
          ],
        ),
        child: Row(
          children: [
            // Thumbnail
            Container(
              width: 60.w,
              height: 60.h,
              decoration: BoxDecoration(
                gradient: LinearGradient(
                  colors: [
                    AppTheme.visionBoardGradient[0].withValues(alpha: 0.1),
                    AppTheme.visionBoardGradient[1].withValues(alpha: 0.05),
                  ],
                  begin: Alignment.topLeft,
                  end: Alignment.bottomRight,
                ),
                borderRadius: BorderRadius.circular(12.r),
              ),
              child: Icon(
                Icons.dashboard,
                color: AppTheme.visionBoardGradient[0].withValues(alpha: 0.6),
                size: 24.sp,
              ),
            ),
            SizedBox(width: 16.w),
            // Content
            Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    board.title,
                    style: TextStyle(
                      fontSize: 16.sp,
                      fontWeight: FontWeight.bold,
                      color: AppTheme.textPrimary,
                    ),
                    maxLines: 1,
                    overflow: TextOverflow.ellipsis,
                  ),
                  SizedBox(height: 4.h),
                  Text(
                    '${board.elements.length} items • Updated ${_formatDate(board.updatedAt)}',
                    style: TextStyle(
                      fontSize: 12.sp,
                      color: AppTheme.textSecondary,
                    ),
                  ),
                ],
              ),
            ),
            // Options
            GestureDetector(
              onTap: () => _showBoardOptions(board),
              child: Container(
                width: 32.w,
                height: 32.h,
                decoration: BoxDecoration(
                  color: AppTheme.textSecondary.withValues(alpha: 0.1),
                  borderRadius: BorderRadius.circular(8.r),
                ),
                child: Icon(
                  Icons.more_vert,
                  color: AppTheme.textSecondary,
                  size: 16.sp,
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildEmptyState() {
    return Center(
      child: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          Icon(
            Icons.search_off,
            size: 80.sp,
            color: AppTheme.textSecondary.withValues(alpha: 0.5),
          ),
          SizedBox(height: 16.h),
          Text(
            searchQuery.isEmpty ? 'No Vision Boards' : 'No Results Found',
            style: TextStyle(
              fontSize: 20.sp,
              fontWeight: FontWeight.bold,
              color: AppTheme.textPrimary,
            ),
          ),
          SizedBox(height: 8.h),
          Text(
            searchQuery.isEmpty 
              ? 'Create your first vision board to get started'
              : 'Try adjusting your search or filters',
            style: TextStyle(
              fontSize: 14.sp,
              color: AppTheme.textSecondary,
            ),
            textAlign: TextAlign.center,
          ),
        ],
      ),
    );
  }

  String _formatDate(DateTime date) {
    final now = DateTime.now();
    final difference = now.difference(date);
    
    if (difference.inDays == 0) {
      return 'today';
    } else if (difference.inDays == 1) {
      return 'yesterday';
    } else if (difference.inDays < 7) {
      return '${difference.inDays}d ago';
    } else {
      return '${date.day}/${date.month}/${date.year}';
    }
  }

  void _openBoard(VisionBoard board) {
    Navigator.push(
      context,
      MaterialPageRoute(
        builder: (context) => VisionBoardScreen(existingBoard: board),
      ),
    ).then((result) {
      if (result != null) {
        // Update the specific board in the list
        setState(() {
          final index = visionBoards.indexWhere((b) => b.id == result.id);
          if (index != -1) {
            visionBoards[index] = result;
            _applyFilters();
          }
        });
      }
    });
  }

  void _showBoardOptions(VisionBoard board) {
    showModalBottomSheet(
      context: context,
      backgroundColor: Colors.transparent,
      builder: (context) => Container(
        margin: EdgeInsets.all(20.w),
        decoration: BoxDecoration(
          color: AppTheme.cardBackground,
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
                color: AppTheme.textSecondary.withValues(alpha: 0.3),
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
                      color: AppTheme.textPrimary,
                    ),
                  ),
                  SizedBox(height: 24.h),
                  _buildOptionItem(Icons.edit, 'Edit', () {
                    Navigator.pop(context);
                    _openBoard(board);
                  }),
                  _buildOptionItem(Icons.share, 'Share', () {
                    Navigator.pop(context);
                    // TODO: Implement sharing
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
              color: isDestructive ? Colors.red : AppTheme.textSecondary,
              size: 20.sp,
            ),
            SizedBox(width: 16.w),
            Text(
              title,
              style: TextStyle(
                fontSize: 16.sp,
                color: isDestructive ? Colors.red : AppTheme.textPrimary,
              ),
            ),
          ],
        ),
      ),
    );
  }

  void _deleteBoard(VisionBoard board) {
    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        backgroundColor: AppTheme.cardBackground,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(16.r),
        ),
        title: Text(
          'Delete Vision Board',
          style: TextStyle(
            color: AppTheme.textPrimary,
            fontSize: 18.sp,
            fontWeight: FontWeight.bold,
          ),
        ),
        content: Text(
          'Are you sure you want to delete "${board.title}"? This action cannot be undone.',
          style: TextStyle(
            color: AppTheme.textSecondary,
            fontSize: 14.sp,
          ),
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context),
            child: Text(
              'Cancel',
              style: TextStyle(color: AppTheme.textSecondary),
            ),
          ),
          TextButton(
            onPressed: () async {
              Navigator.pop(context);
              await _performDelete(board);
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
  
  Future<void> _performDelete(VisionBoard board) async {
    try {
      final visionBoardService = VisionBoardService();
      await visionBoardService.initialize(baseUrl: 'http://manifestation-qa-1.senthilguru.net');
      
      final result = await visionBoardService.deleteVisionBoard(board.id);
      
      if (result.isSuccess) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Vision board deleted successfully'),
            backgroundColor: AppTheme.accentMint,
          ),
        );
        
        // Remove from local list and refresh
        setState(() {
          visionBoards.removeWhere((b) => b.id == board.id);
          _applyFilters();
        });
      } else {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Failed to delete vision board: ${result.message}'),
            backgroundColor: Colors.red,
          ),
        );
      }
    } catch (e) {
      print('❌ Delete board error: $e');
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Failed to delete vision board: $e'),
          backgroundColor: Colors.red,
        ),
      );
    }
  }
}