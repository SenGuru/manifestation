import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../constants/app_theme.dart';

class GratitudeSection extends StatefulWidget {
  final List<String> gratitudeItems;
  final Function(int, String) onGratitudeChanged;

  const GratitudeSection({
    Key? key,
    required this.gratitudeItems,
    required this.onGratitudeChanged,
  }) : super(key: key);

  @override
  State<GratitudeSection> createState() => _GratitudeSectionState();
}

class _GratitudeSectionState extends State<GratitudeSection> {
  late List<bool> _visibleItems;
  late ScrollController _scrollController;
  
  @override
  void initState() {
    super.initState();
    _scrollController = ScrollController();
    
    // Initialize visibility - show items that have content, minimum 1
    _visibleItems = List.generate(5, (index) => false);
    
    // Show items that have content
    int visibleCount = 0;
    for (int i = 0; i < widget.gratitudeItems.length; i++) {
      if (widget.gratitudeItems[i].isNotEmpty) {
        _visibleItems[i] = true;
        visibleCount++;
      }
    }
    
    // Ensure at least one item is visible
    if (visibleCount == 0) {
      _visibleItems[0] = true;
    }
  }
  
  @override
  void dispose() {
    _scrollController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        // Simple header like mood screen
        Text(
          'Daily Gratitudes',
          style: TextStyle(
            fontSize: 24.sp,
            fontWeight: FontWeight.bold,
            color: AppTheme.textPrimary,
          ),
        ),
        SizedBox(height: 8.h),
        Text(
          'What are you grateful for today?',
          style: TextStyle(
            fontSize: 16.sp,
            color: AppTheme.textSecondary,
          ),
        ),
        
        SizedBox(height: 32.h),
        
        // Gratitude items
        Expanded(
          child: SingleChildScrollView(
            controller: _scrollController,
            child: Column(
              children: [
                // Visible gratitude items
                ..._buildGratitudeItems(),
                
                // Add button (if less than 5 items visible)
                if (_getVisibleCount() < 5) _buildAddButton(),
                
                // Extra padding for keyboard space
                SizedBox(height: 100.h),
              ],
            ),
          ),
        ),
      ],
    );
  }

  List<Widget> _buildGratitudeItems() {
    List<Widget> items = [];
    for (int i = 0; i < _visibleItems.length; i++) {
      if (_visibleItems[i]) {
        items.add(
          Container(
            margin: EdgeInsets.only(bottom: 16.h),
            child: _buildGratitudeItem(i),
          ),
        );
      }
    }
    return items;
  }

  Widget _buildGratitudeItem(int index) {
    final canRemove = index > 0 && _getVisibleCount() > 1;
    final hasContent = widget.gratitudeItems[index].isNotEmpty;
    
    return Container(
      decoration: BoxDecoration(
        color: AppTheme.cardBackground,
        borderRadius: BorderRadius.circular(18.r),
        border: Border.all(
          color: hasContent 
            ? AppTheme.journalGradient[0].withOpacity(0.3)
            : AppTheme.borderSecondary,
          width: hasContent ? 1.5 : 1,
        ),
        boxShadow: [
          BoxShadow(
            color: hasContent 
              ? AppTheme.journalGradient[0].withOpacity(0.08)
              : AppTheme.black.withOpacity(0.04),
            blurRadius: hasContent ? 8.r : 4.r,
            offset: Offset(0, 2.h),
          ),
        ],
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          // Header
          Padding(
            padding: EdgeInsets.fromLTRB(20.w, 18.h, 16.w, 12.h),
            child: Row(
              children: [
                Container(
                  width: 28.w,
                  height: 28.h,
                  decoration: BoxDecoration(
                    color: hasContent 
                      ? AppTheme.journalGradient[0] 
                      : AppTheme.textMedium.withOpacity(0.3),
                    borderRadius: BorderRadius.circular(14.r),
                  ),
                  child: Center(
                    child: Text(
                      '${index + 1}',
                      style: TextStyle(
                        fontSize: 12.sp,
                        fontWeight: FontWeight.bold,
                        color: Colors.white,
                      ),
                    ),
                  ),
                ),
                SizedBox(width: 12.w),
                Expanded(
                  child: Text(
                    index == 0 ? 'I\'m grateful for...' : 'Also grateful for...',
                    style: TextStyle(
                      fontSize: 16.sp,
                      fontWeight: FontWeight.w600,
                      color: AppTheme.textLight,
                    ),
                  ),
                ),
                if (canRemove)
                  GestureDetector(
                    onTap: () => _removeGratitudeItem(index),
                    child: Container(
                      width: 32.w,
                      height: 32.h,
                      decoration: BoxDecoration(
                        color: AppTheme.textMedium.withOpacity(0.1),
                        borderRadius: BorderRadius.circular(16.r),
                      ),
                      child: Icon(
                        Icons.close,
                        size: 16.sp,
                        color: AppTheme.textMedium,
                      ),
                    ),
                  ),
              ],
            ),
          ),
          
          // Text field with proper styling to match other pages
          Container(
            margin: EdgeInsets.fromLTRB(20.w, 0, 20.w, 24.h),
            decoration: BoxDecoration(
              color: AppTheme.cardBackground,
              borderRadius: BorderRadius.circular(16.r),
              border: Border.all(
                color: AppTheme.borderSecondary,
              ),
            ),
            child: TextField(
              onChanged: (value) => widget.onGratitudeChanged(index, value),
              controller: TextEditingController(text: widget.gratitudeItems[index])
                ..selection = TextSelection.fromPosition(
                  TextPosition(offset: widget.gratitudeItems[index].length),
                ),
              style: TextStyle(
                color: AppTheme.textPrimary,
                fontSize: 16.sp,
                height: 1.6,
                fontWeight: FontWeight.w400,
              ),
              decoration: InputDecoration(
                hintText: _getGratitudeHint(index),
                hintStyle: TextStyle(
                  color: AppTheme.textSecondary,
                  fontSize: 16.sp,
                  height: 1.6,
                ),
                border: InputBorder.none,
                contentPadding: EdgeInsets.all(20.w),
                isDense: false,
              ),
              maxLines: null,
              minLines: 4,
              maxLength: 500, // Backend limit for gratitudes
              textAlignVertical: TextAlignVertical.top,
              textCapitalization: TextCapitalization.sentences,
              buildCounter: (context, {required currentLength, required isFocused, maxLength}) {
                return Padding(
                  padding: EdgeInsets.only(top: 4.h),
                  child: Text(
                    '$currentLength / $maxLength',
                    style: TextStyle(
                      fontSize: 12.sp,
                      color: currentLength > (maxLength! * 0.9) 
                        ? Colors.orange 
                        : AppTheme.textSecondary,
                    ),
                  ),
                );
              },
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildAddButton() {
    return Container(
      margin: EdgeInsets.only(top: 12.h, bottom: 24.h),
      child: GestureDetector(
        onTap: _addGratitudeItem,
        child: Container(
          padding: EdgeInsets.symmetric(vertical: 18.h, horizontal: 24.w),
          decoration: BoxDecoration(
            color: AppTheme.cardBackground,
            borderRadius: BorderRadius.circular(16.r),
            border: Border.all(
              color: AppTheme.journalGradient[0].withOpacity(0.4),
              width: 1.5,
              style: BorderStyle.solid,
            ),
            boxShadow: [
              BoxShadow(
                color: AppTheme.journalGradient[0].withOpacity(0.1),
                blurRadius: 6.r,
                offset: Offset(0, 2.h),
              ),
            ],
          ),
          child: Row(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              Container(
                padding: EdgeInsets.all(6.w),
                decoration: BoxDecoration(
                  color: AppTheme.journalGradient[0].withOpacity(0.1),
                  borderRadius: BorderRadius.circular(8.r),
                ),
                child: Icon(
                  Icons.add,
                  color: AppTheme.journalGradient[0],
                  size: 18.sp,
                ),
              ),
              SizedBox(width: 12.w),
              Text(
                'Add another gratitude',
                style: TextStyle(
                  fontSize: 16.sp,
                  fontWeight: FontWeight.w600,
                  color: AppTheme.journalGradient[0],
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }

  void _addGratitudeItem() {
    setState(() {
      // Find the first non-visible item and make it visible
      for (int i = 0; i < _visibleItems.length; i++) {
        if (!_visibleItems[i]) {
          _visibleItems[i] = true;
          break;
        }
      }
    });
  }

  void _removeGratitudeItem(int index) {
    setState(() {
      _visibleItems[index] = false;
      widget.onGratitudeChanged(index, '');
    });
  }

  int _getVisibleCount() {
    return _visibleItems.where((visible) => visible).length;
  }

  String _getGratitudeHint(int index) {
    switch (index) {
      case 0:
        return 'Someone or something that brought joy...';
      case 1:
        return 'A moment that made you smile...';
      case 2:
        return 'Something beautiful you noticed...';
      case 3:
        return 'An experience you appreciated...';
      case 4:
        return 'A blessing in your life...';
      default:
        return 'Something you\'re grateful for...';
    }
  }
}