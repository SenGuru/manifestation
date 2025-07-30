import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:fun/constants/colors.dart';

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
      children: [
        // Instructions
        Container(
          padding: EdgeInsets.all(16.w),
          decoration: BoxDecoration(
            color: AppColors.gradientPink.withOpacity(0.1),
            borderRadius: BorderRadius.circular(12.r),
            border: Border.all(
              color: AppColors.gradientPink.withOpacity(0.3),
            ),
          ),
          child: Row(
            children: [
              Icon(
                Icons.lightbulb_outline,
                color: AppColors.gradientPink,
                size: 20.sp,
              ),
              SizedBox(width: 12.w),
              Expanded(
                child: Text(
                  'Add what you\'re grateful for today (at least one required)',
                  style: TextStyle(
                    fontSize: 14.sp,
                    color: AppColors.textMedium,
                    height: 1.3,
                  ),
                ),
              ),
            ],
          ),
        ),
        
        SizedBox(height: 20.h),
        
        // Gratitude items with better keyboard handling
        Expanded(
          child: SingleChildScrollView(
            controller: _scrollController,
            physics: BouncingScrollPhysics(),
            child: Column(
              children: [
                // Visible gratitude items
                ..._buildVisibleGratitudeItems(),
                
                // Add button (if less than 5 items visible)
                if (_getVisibleCount() < 5) _buildAddButton(),
                
                // Extra padding for keyboard space
                SizedBox(height: MediaQuery.of(context).viewInsets.bottom + 100.h),
              ],
            ),
          ),
        ),
      ],
    );
  }

  List<Widget> _buildVisibleGratitudeItems() {
    List<Widget> items = [];
    for (int i = 0; i < _visibleItems.length; i++) {
      if (_visibleItems[i]) {
        items.add(Container(
          margin: EdgeInsets.only(bottom: 16.h),
          child: _buildCompactGratitudeItem(i),
        ));
      }
    }
    return items;
  }

  Widget _buildCompactGratitudeItem(int index) {
    final isFirstItem = index == 0;
    final canRemove = !isFirstItem && _getVisibleCount() > 1;
    
    return Container(
      decoration: BoxDecoration(
        color: AppColors.cardBackground,
        borderRadius: BorderRadius.circular(12.r),
        border: Border.all(
          color: widget.gratitudeItems[index].isNotEmpty
              ? AppColors.gradientPink.withOpacity(0.3)
              : AppColors.textMedium.withOpacity(0.15),
        ),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          // Compact header with optional remove button
          if (canRemove || !isFirstItem)
            Container(
              padding: EdgeInsets.symmetric(horizontal: 16.w, vertical: 12.h),
              child: Row(
                children: [
                  Expanded(
                    child: Text(
                      isFirstItem ? 'I\'m grateful for...' : 'Also grateful for...',
                      style: TextStyle(
                        fontSize: 14.sp,
                        fontWeight: FontWeight.w500,
                        color: AppColors.textMedium,
                      ),
                    ),
                  ),
                  if (canRemove)
                    GestureDetector(
                      onTap: () => _removeGratitudeItem(index),
                      child: Container(
                        padding: EdgeInsets.all(4.w),
                        decoration: BoxDecoration(
                          color: AppColors.textMedium.withOpacity(0.1),
                          borderRadius: BorderRadius.circular(6.r),
                        ),
                        child: Icon(
                          Icons.close,
                          size: 16.sp,
                          color: AppColors.textMedium,
                        ),
                      ),
                    ),
                ],
              ),
            ),
          
          // Text field
          Padding(
            padding: EdgeInsets.fromLTRB(16.w, isFirstItem ? 16.h : 8.h, 16.w, 16.h),
            child: TextField(
              onChanged: (value) => widget.onGratitudeChanged(index, value),
              onTap: () => _scrollToField(index),
              controller: TextEditingController(text: widget.gratitudeItems[index])
                ..selection = TextSelection.fromPosition(
                  TextPosition(offset: widget.gratitudeItems[index].length),
                ),
              style: TextStyle(
                color: AppColors.textLight,
                fontSize: 16.sp,
                height: 1.5,
              ),
              decoration: InputDecoration(
                hintText: _getGratitudeHint(index),
                hintStyle: TextStyle(
                  color: AppColors.textMedium.withOpacity(0.6),
                  fontSize: 16.sp,
                  height: 1.5,
                ),
                border: InputBorder.none,
                contentPadding: EdgeInsets.only(
                  top: 4.h,
                  left: 8.w,
                  right: 8.w,
                  bottom: 4.h,
                ),
                isDense: true,
              ),
              maxLines: 3,
              minLines: 2,
              textCapitalization: TextCapitalization.sentences,
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildAddButton() {
    return Container(
      margin: EdgeInsets.only(top: 8.h, bottom: 16.h),
      child: GestureDetector(
        onTap: _addGratitudeItem,
        child: Container(
          padding: EdgeInsets.symmetric(vertical: 16.h, horizontal: 20.w),
          decoration: BoxDecoration(
            color: AppColors.gradientPink.withOpacity(0.1),
            borderRadius: BorderRadius.circular(12.r),
            border: Border.all(
              color: AppColors.gradientPink.withOpacity(0.3),
              style: BorderStyle.solid,
            ),
          ),
          child: Row(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              Icon(
                Icons.add_circle_outline,
                color: AppColors.gradientPink,
                size: 20.sp,
              ),
              SizedBox(width: 8.w),
              Text(
                'Add Another Gratitude',
                style: TextStyle(
                  fontSize: 16.sp,
                  fontWeight: FontWeight.w500,
                  color: AppColors.gradientPink,
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
    
    // Auto scroll to the new item after a brief delay
    Future.delayed(Duration(milliseconds: 100), () {
      if (_scrollController.hasClients) {
        _scrollController.animateTo(
          _scrollController.position.maxScrollExtent,
          duration: Duration(milliseconds: 300),
          curve: Curves.easeOut,
        );
      }
    });
  }

  void _removeGratitudeItem(int index) {
    setState(() {
      _visibleItems[index] = false;
      // Clear the text when removing
      widget.onGratitudeChanged(index, '');
    });
  }

  void _scrollToField(int index) {
    // Scroll to keep the focused field visible when keyboard appears
    Future.delayed(Duration(milliseconds: 300), () {
      if (_scrollController.hasClients) {
        final context = this.context;
        final renderBox = context.findRenderObject() as RenderBox?;
        if (renderBox != null) {
          final position = renderBox.localToGlobal(Offset.zero);
          final keyboardHeight = MediaQuery.of(context).viewInsets.bottom;
          
          if (keyboardHeight > 0) {
            _scrollController.animateTo(
              _scrollController.offset + (keyboardHeight * 0.3),
              duration: Duration(milliseconds: 300),
              curve: Curves.easeOut,
            );
          }
        }
      }
    });
  }

  int _getVisibleCount() {
    return _visibleItems.where((visible) => visible).length;
  }

  String _getGratitudeHint(int index) {
    switch (index) {
      case 0:
        return 'A person who made your day better...';
      case 1:
        return 'A moment that brought you peace...';
      case 2:
        return 'Something that made you smile...';
      case 3:
        return 'An experience you cherished...';
      case 4:
        return 'Something that inspired you...';
      default:
        return 'Something you\'re grateful for...';
    }
  }
}