import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../widgets/universal_back_button.dart';
import '../models/vision_board.dart';
import '../services/vision_board_service.dart';
import '../services/unified_auth_service.dart';
import 'vision_board_screen.dart';
import '../constants/app_theme.dart';
import '../widgets/stylish_loading.dart';
import '../utils/navigation_utils.dart';
import 'dart:typed_data';

class VisionBoardGalleryScreen extends StatefulWidget {
  final List<VisionBoard> initialVisionBoards;

  const VisionBoardGalleryScreen({
    Key? key,
    this.initialVisionBoards = const [],
  }) : super(key: key);

  @override
  _VisionBoardGalleryScreenState createState() => _VisionBoardGalleryScreenState();
}

class _VisionBoardGalleryScreenState extends State<VisionBoardGalleryScreen> {
  List<VisionBoard> visionBoards = [];
  bool isLoading = false;
  bool isLoadingMore = false;
  bool hasMoreData = true;
  int currentPage = 0;
  final int itemsPerPage = 6; // Load 6 vision boards at a time
  
  final ScrollController _scrollController = ScrollController();
  final TextEditingController _searchController = TextEditingController();
  String _searchQuery = '';
  List<VisionBoard> _filteredBoards = [];
  List<VisionBoard> _allAvailableBoards = []; // Cache all boards from API

  @override
  void initState() {
    super.initState();
    // Start with initial boards if provided
    if (widget.initialVisionBoards.isNotEmpty) {
      _allAvailableBoards = List.from(widget.initialVisionBoards);
      _loadInitialBoards();
    } else {
      _loadAllVisionBoards();
    }
    _scrollController.addListener(_onScroll);
    _searchController.addListener(_onSearchChanged);
  }

  @override
  void dispose() {
    _scrollController.dispose();
    _searchController.dispose();
    super.dispose();
  }

  void _onScroll() {
    if (_scrollController.position.pixels >= _scrollController.position.maxScrollExtent - 200) {
      if (!isLoadingMore && hasMoreData) {
        _loadMoreVisionBoards();
      }
    }
  }

  void _onSearchChanged() {
    setState(() {
      _searchQuery = _searchController.text.toLowerCase();
      _filterBoards();
    });
  }

  void _filterBoards() {
    if (_searchQuery.isEmpty) {
      _filteredBoards = visionBoards;
    } else {
      _filteredBoards = visionBoards.where((board) {
        final title = board.title.toLowerCase();
        return title.contains(_searchQuery);
      }).toList();
    }
  }

  Future<void> _loadAllVisionBoards() async {
    setState(() {
      isLoading = true;
    });

    try {
      final visionBoardService = VisionBoardService();
      await visionBoardService.initialize(baseUrl: 'http://manifestation-qa-1.senthilguru.net');
      
      final result = await visionBoardService.getUserVisionBoards();
      
      if (result.isSuccess) {
        setState(() {
          _allAvailableBoards = result.data ?? [];
          _loadInitialBoards();
        });
        
        print('✅ Loaded ${_allAvailableBoards.length} total vision boards');
      } else {
        print('❌ Failed to load vision boards: ${result.message}');
        setState(() {
          hasMoreData = false;
        });
      }
    } catch (e) {
      print('❌ Error loading vision boards: $e');
      setState(() {
        hasMoreData = false;
      });
      
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Failed to load vision boards'),
            backgroundColor: Colors.red,
          ),
        );
      }
    }

    setState(() {
      isLoading = false;
    });
  }

  void _loadInitialBoards() {
    final endIndex = itemsPerPage;
    final initialBoards = _allAvailableBoards.take(endIndex).toList();
    
    setState(() {
      visionBoards = initialBoards;
      currentPage = 1;
      hasMoreData = endIndex < _allAvailableBoards.length;
      _filterBoards();
    });
    
    print('✅ Loaded initial ${initialBoards.length} vision boards');
  }

  void _loadMoreVisionBoards() {
    if (isLoadingMore || !hasMoreData) return;

    setState(() {
      isLoadingMore = true;
    });

    // Simulate loading delay for better UX
    Future.delayed(Duration(milliseconds: 500), () {
      final startIndex = currentPage * itemsPerPage;
      final endIndex = startIndex + itemsPerPage;
      final nextBoards = _allAvailableBoards.skip(startIndex).take(itemsPerPage).toList();
      
      setState(() {
        visionBoards.addAll(nextBoards);
        currentPage++;
        hasMoreData = endIndex < _allAvailableBoards.length;
        isLoadingMore = false;
        _filterBoards();
      });
      
      print('✅ Loaded ${nextBoards.length} more vision boards (page $currentPage)');
    });
  }

  Future<void> _refreshVisionBoards() async {
    setState(() {
      currentPage = 0;
      hasMoreData = true;
      visionBoards.clear();
    });
    
    await _loadAllVisionBoards();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppTheme.backgroundPrimary,
      body: SafeArea(
        child: Column(
          children: [
            _buildHeader(),
            _buildSearchBar(),
            Expanded(
              child: isLoading && visionBoards.isEmpty
                  ? StylishLoading(message: 'Loading vision boards...')
                  : _filteredBoards.isEmpty
                      ? _buildEmptyState()
                      : _buildVisionBoardsGrid(),
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
          UniversalBackButton(),
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
                  '${_allAvailableBoards.length} total boards • ${visionBoards.length} loaded',
                  style: TextStyle(
                    fontSize: 14.sp,
                    color: AppTheme.textSecondary,
                  ),
                ),
              ],
            ),
          ),
          _buildSortButton(),
        ],
      ),
    );
  }

  Widget _buildSortButton() {
    return GestureDetector(
      onTap: _showSortOptions,
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
          Icons.sort_rounded,
          color: AppTheme.textPrimary,
          size: 20.sp,
        ),
      ),
    );
  }

  Widget _buildSearchBar() {
    return Container(
      margin: EdgeInsets.symmetric(horizontal: 20.w, vertical: 8.h),
      decoration: BoxDecoration(
        color: AppTheme.cardBackground,
        borderRadius: BorderRadius.circular(16.r),
        border: Border.all(
          color: AppTheme.textSecondary.withValues(alpha: 0.1),
        ),
        boxShadow: [
          BoxShadow(
            color: AppTheme.black.withValues(alpha: 0.03),
            blurRadius: 4.r,
            offset: Offset(0, 2.h),
          ),
        ],
      ),
      child: TextField(
        controller: _searchController,
        style: TextStyle(
          fontSize: 16.sp,
          color: AppTheme.textPrimary,
          fontWeight: FontWeight.w500,
        ),
        decoration: InputDecoration(
          hintText: 'Search vision boards...',
          hintStyle: TextStyle(
            fontSize: 16.sp,
            color: AppTheme.textSecondary.withValues(alpha: 0.6),
            fontWeight: FontWeight.w400,
          ),
          prefixIcon: Container(
            padding: EdgeInsets.all(12.w),
            child: Icon(
              Icons.search_rounded,
              color: AppTheme.textSecondary.withValues(alpha: 0.7),
              size: 20.sp,
            ),
          ),
          suffixIcon: _searchQuery.isNotEmpty
              ? GestureDetector(
                  onTap: () {
                    _searchController.clear();
                  },
                  child: Container(
                    padding: EdgeInsets.all(12.w),
                    child: Icon(
                      Icons.clear_rounded,
                      color: AppTheme.textSecondary.withValues(alpha: 0.7),
                      size: 20.sp,
                    ),
                  ),
                )
              : null,
          border: InputBorder.none,
          enabledBorder: InputBorder.none,
          focusedBorder: InputBorder.none,
          contentPadding: EdgeInsets.symmetric(
            horizontal: 16.w,
            vertical: 16.h,
          ),
          filled: true,
          fillColor: Colors.transparent,
        ),
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
                colors: AppTheme.visionBoardGradient.map((c) => c.withValues(alpha: 0.1)).toList(),
                begin: Alignment.topLeft,
                end: Alignment.bottomRight,
              ),
              borderRadius: BorderRadius.circular(60.r),
            ),
            child: Icon(
              _searchQuery.isNotEmpty ? Icons.search_off_rounded : Icons.image_not_supported_outlined,
              color: AppTheme.visionBoardGradient[0],
              size: 60.sp,
            ),
          ),
          SizedBox(height: 24.h),
          Text(
            _searchQuery.isNotEmpty ? 'No Results Found' : 'No Vision Boards Available',
            style: TextStyle(
              fontSize: 24.sp,
              fontWeight: FontWeight.bold,
              color: AppTheme.textPrimary,
            ),
          ),
          SizedBox(height: 8.h),
          Text(
            _searchQuery.isNotEmpty 
                ? 'Try different search terms'
                : 'Create your first vision board to get started',
            style: TextStyle(
              fontSize: 16.sp,
              color: AppTheme.textSecondary,
              height: 1.5,
            ),
            textAlign: TextAlign.center,
          ),
        ],
      ),
    );
  }

  Widget _buildVisionBoardsGrid() {
    return RefreshIndicator(
      onRefresh: _refreshVisionBoards,
      child: CustomScrollView(
        controller: _scrollController,
        slivers: [
          SliverPadding(
            padding: EdgeInsets.symmetric(horizontal: 20.w, vertical: 16.h),
            sliver: SliverGrid(
              gridDelegate: SliverGridDelegateWithFixedCrossAxisCount(
                crossAxisCount: 2,
                crossAxisSpacing: 16.w,
                mainAxisSpacing: 16.h,
                childAspectRatio: 0.75,
              ),
              delegate: SliverChildBuilderDelegate(
                (context, index) {
                  if (index < _filteredBoards.length) {
                    return _buildVisionBoardCard(_filteredBoards[index]);
                  }
                  return null;
                },
                childCount: _filteredBoards.length,
              ),
            ),
          ),
          if (isLoadingMore) ...[
            SliverToBoxAdapter(
              child: Container(
                padding: EdgeInsets.all(20.w),
                child: Center(
                  child: Column(
                    children: [
                      CircularProgressIndicator(
                        valueColor: AlwaysStoppedAnimation<Color>(
                          AppTheme.visionBoardGradient[0],
                        ),
                      ),
                      SizedBox(height: 12.h),
                      Text(
                        'Loading more vision boards...',
                        style: TextStyle(
                          fontSize: 14.sp,
                          color: AppTheme.textSecondary,
                        ),
                      ),
                    ],
                  ),
                ),
              ),
            ),
          ],
          if (!hasMoreData && _filteredBoards.isNotEmpty) ...[
            SliverToBoxAdapter(
              child: Container(
                padding: EdgeInsets.all(20.w),
                child: Center(
                  child: Text(
                    'End of List',
                    style: TextStyle(
                      fontSize: 14.sp,
                      color: AppTheme.textSecondary,
                    ),
                  ),
                ),
              ),
            ),
          ],
        ],
      ),
    );
  }

  Widget _buildVisionBoardCard(VisionBoard board) {
    return GestureDetector(
      onTap: () {
        HapticFeedback.lightImpact();
        _openVisionBoard(board);
      },
      onLongPress: () {
        HapticFeedback.heavyImpact();
        _showVisionBoardOptions(board);
      },
      child: Container(
        decoration: BoxDecoration(
          color: AppTheme.cardBackground,
          borderRadius: BorderRadius.circular(20.r),
          border: Border.all(
            color: AppTheme.textSecondary.withValues(alpha: 0.1),
          ),
          boxShadow: [
            BoxShadow(
              color: AppTheme.black.withValues(alpha: 0.05),
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
                      AppTheme.visionBoardGradient[0].withValues(alpha: 0.1),
                      AppTheme.visionBoardGradient[1].withValues(alpha: 0.05),
                    ],
                    begin: Alignment.topLeft,
                    end: Alignment.bottomRight,
                  ),
                  borderRadius: BorderRadius.vertical(top: Radius.circular(20.r)),
                ),
                child: _buildVisionBoardPreview(board),
              ),
            ),
            Expanded(
              flex: 1,
              child: Container(
                padding: EdgeInsets.all(8.w),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: [
                    Flexible(
                      child: Text(
                        board.title.isEmpty ? 'Untitled Vision Board' : board.title,
                        style: TextStyle(
                          fontSize: 14.sp,
                          fontWeight: FontWeight.bold,
                          color: AppTheme.textPrimary,
                        ),
                        maxLines: 1,
                        overflow: TextOverflow.ellipsis,
                      ),
                    ),
                    SizedBox(height: 2.h),
                    Flexible(
                      child: Row(
                        children: [
                          Icon(
                            Icons.access_time_rounded,
                            color: AppTheme.textSecondary.withValues(alpha: 0.7),
                            size: 10.sp,
                          ),
                          SizedBox(width: 3.w),
                          Expanded(
                            child: Text(
                              _formatDate(board.updatedAt.toIso8601String()),
                              style: TextStyle(
                                fontSize: 10.sp,
                                color: AppTheme.textSecondary.withValues(alpha: 0.7),
                                fontWeight: FontWeight.w500,
                              ),
                              overflow: TextOverflow.ellipsis,
                            ),
                          ),
                        ],
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

  Widget _buildVisionBoardPreview(VisionBoard board) {
    // Show a preview similar to the home screen
    final displayTitle = board.title.isEmpty ? 'Vision Board' : board.title;
    final firstLetter = displayTitle.substring(0, 1).toUpperCase();
    
    return Container(
      width: double.infinity,
      height: double.infinity,
      decoration: BoxDecoration(
        gradient: LinearGradient(
          colors: [
            AppTheme.visionBoardGradient[0].withValues(alpha: 0.3),
            AppTheme.visionBoardGradient[1].withValues(alpha: 0.2),
          ],
          begin: Alignment.topLeft,
          end: Alignment.bottomRight,
        ),
        borderRadius: BorderRadius.vertical(top: Radius.circular(20.r)),
      ),
      child: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Container(
              width: 50.w,
              height: 50.h,
              decoration: BoxDecoration(
                color: AppTheme.white,
                shape: BoxShape.circle,
                boxShadow: [
                  BoxShadow(
                    color: AppTheme.visionBoardGradient[0].withValues(alpha: 0.3),
                    blurRadius: 15.r,
                    offset: Offset(0, 8.h),
                  ),
                ],
              ),
              child: Center(
                child: Text(
                  firstLetter,
                  style: TextStyle(
                    fontSize: 24.sp,
                    fontWeight: FontWeight.bold,
                    color: AppTheme.visionBoardGradient[0],
                  ),
                ),
              ),
            ),
            SizedBox(height: 8.h),
            if (board.elements.isNotEmpty)
              Container(
                padding: EdgeInsets.symmetric(horizontal: 8.w, vertical: 2.h),
                decoration: BoxDecoration(
                  color: AppTheme.white.withValues(alpha: 0.9),
                  borderRadius: BorderRadius.circular(8.r),
                ),
                child: Text(
                  '${board.elements.length} items',
                  style: TextStyle(
                    fontSize: 10.sp,
                    color: AppTheme.textSecondary,
                    fontWeight: FontWeight.w500,
                  ),
                ),
              ),
          ],
        ),
      ),
    );
  }

  String _formatDate(String dateString) {
    if (dateString.isEmpty) return 'Unknown';
    
    try {
      final date = DateTime.parse(dateString);
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
    } catch (e) {
      return 'Unknown';
    }
  }

  void _openVisionBoard(VisionBoard board) {
    pushWithoutSwipe<VisionBoard>(
      context,
      VisionBoardScreen(existingBoard: board),
    ).then((result) {
      if (result != null) {
        // Update the board in our lists
        setState(() {
          final index = _allAvailableBoards.indexWhere((b) => b.id == result.id);
          if (index != -1) {
            _allAvailableBoards[index] = result;
          }
          
          final visibleIndex = visionBoards.indexWhere((b) => b.id == result.id);
          if (visibleIndex != -1) {
            visionBoards[visibleIndex] = result;
          }
          
          _filterBoards();
        });
      }
    });
  }

  void _showVisionBoardOptions(VisionBoard board) {
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
                    board.title.isEmpty ? 'Untitled Vision Board' : board.title,
                    style: TextStyle(
                      fontSize: 18.sp,
                      fontWeight: FontWeight.bold,
                      color: AppTheme.textPrimary,
                    ),
                    maxLines: 2,
                    overflow: TextOverflow.ellipsis,
                  ),
                  SizedBox(height: 24.h),
                  _buildOptionItem(Icons.edit, 'Edit', () {
                    Navigator.pop(context);
                    _openVisionBoard(board);
                  }),
                  _buildOptionItem(Icons.copy, 'Duplicate', () {
                    Navigator.pop(context);
                    _duplicateVisionBoard(board);
                  }),
                  _buildOptionItem(Icons.share_rounded, 'Share', () {
                    Navigator.pop(context);
                    _shareVisionBoard(board);
                  }),
                  _buildOptionItem(Icons.delete_outline, 'Delete', () {
                    Navigator.pop(context);
                    _deleteVisionBoard(board);
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

  void _showSortOptions() {
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
                    'Sort Vision Boards',
                    style: TextStyle(
                      fontSize: 18.sp,
                      fontWeight: FontWeight.bold,
                      color: AppTheme.textPrimary,
                    ),
                  ),
                  SizedBox(height: 24.h),
                  _buildOptionItem(Icons.access_time_rounded, 'Newest First', () {
                    Navigator.pop(context);
                    _sortVisionBoards('newest');
                  }),
                  _buildOptionItem(Icons.access_time_filled_rounded, 'Oldest First', () {
                    Navigator.pop(context);
                    _sortVisionBoards('oldest');
                  }),
                  _buildOptionItem(Icons.sort_by_alpha_rounded, 'Name A-Z', () {
                    Navigator.pop(context);
                    _sortVisionBoards('name_asc');
                  }),
                  _buildOptionItem(Icons.sort_by_alpha_rounded, 'Name Z-A', () {
                    Navigator.pop(context);
                    _sortVisionBoards('name_desc');
                  }),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }

  void _sortVisionBoards(String sortType) {
    setState(() {
      switch (sortType) {
        case 'newest':
          _allAvailableBoards.sort((a, b) => b.updatedAt.compareTo(a.updatedAt));
          visionBoards.sort((a, b) => b.updatedAt.compareTo(a.updatedAt));
          break;
        case 'oldest':
          _allAvailableBoards.sort((a, b) => a.updatedAt.compareTo(b.updatedAt));
          visionBoards.sort((a, b) => a.updatedAt.compareTo(b.updatedAt));
          break;
        case 'name_asc':
          _allAvailableBoards.sort((a, b) => a.title.toLowerCase().compareTo(b.title.toLowerCase()));
          visionBoards.sort((a, b) => a.title.toLowerCase().compareTo(b.title.toLowerCase()));
          break;
        case 'name_desc':
          _allAvailableBoards.sort((a, b) => b.title.toLowerCase().compareTo(a.title.toLowerCase()));
          visionBoards.sort((a, b) => b.title.toLowerCase().compareTo(a.title.toLowerCase()));
          break;
      }
      _filterBoards();
    });

    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text('Vision boards sorted successfully'),
        backgroundColor: AppTheme.visionBoardGradient[0],
      ),
    );
  }

  void _duplicateVisionBoard(VisionBoard board) {
    // TODO: Implement duplication functionality
    print('Duplicating vision board: ${board.title}');
    
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text('Duplicate functionality coming soon'),
        backgroundColor: AppTheme.visionBoardGradient[0],
      ),
    );
  }

  void _shareVisionBoard(VisionBoard board) {
    // TODO: Implement share functionality
    print('Sharing vision board: ${board.title}');
    
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text('Share functionality coming soon'),
        backgroundColor: AppTheme.visionBoardGradient[0],
      ),
    );
  }

  void _deleteVisionBoard(VisionBoard board) {
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
          'Are you sure you want to delete "${board.title.isEmpty ? 'this vision board' : board.title}"? This action cannot be undone.',
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
            onPressed: () {
              Navigator.pop(context);
              _performDeleteVisionBoard(board);
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

  Future<void> _performDeleteVisionBoard(VisionBoard board) async {
    try {
      final visionBoardService = VisionBoardService();
      await visionBoardService.initialize(baseUrl: 'http://manifestation-qa-1.senthilguru.net');
      
      final result = await visionBoardService.deleteVisionBoard(board.id);
      
      if (result.isSuccess) {
        setState(() {
          _allAvailableBoards.removeWhere((b) => b.id == board.id);
          visionBoards.removeWhere((b) => b.id == board.id);
          _filterBoards();
        });
        
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Vision board deleted successfully'),
            backgroundColor: AppTheme.visionBoardGradient[0],
          ),
        );
      } else {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Failed to delete vision board: ${result.message}'),
            backgroundColor: Colors.red,
          ),
        );
      }
    } catch (e) {
      print('❌ Delete vision board error: $e');
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Failed to delete vision board: $e'),
          backgroundColor: Colors.red,
        ),
      );
    }
  }
}