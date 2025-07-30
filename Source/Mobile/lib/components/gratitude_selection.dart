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

class _GratitudeSectionState extends State<GratitudeSection> with TickerProviderStateMixin {
  late List<bool> _visibleItems;
  late ScrollController _scrollController;
  late List<AnimationController> _itemControllers;
  late List<Animation<double>> _itemAnimations;
  late List<Animation<Offset>> _slideAnimations;
  
  @override
  void initState() {
    super.initState();
    _scrollController = ScrollController();
    
    // Initialize animation controllers
    _itemControllers = List.generate(5, (index) => 
      AnimationController(
        duration: Duration(milliseconds: 300),
        vsync: this,
      )
    );
    
    // Initialize animations
    _itemAnimations = _itemControllers.map((controller) =>
      Tween<double>(begin: 0.0, end: 1.0).animate(
        CurvedAnimation(parent: controller, curve: Curves.easeOutCubic)
      )
    ).toList();
    
    _slideAnimations = _itemControllers.map((controller) =>
      Tween<Offset>(begin: Offset(0, 0.15), end: Offset.zero).animate(
        CurvedAnimation(parent: controller, curve: Curves.easeOutCubic)
      )
    ).toList();
    
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
    
    // Start initial animations
    Future.delayed(Duration(milliseconds: 200), () {
      for (int i = 0; i < _visibleItems.length; i++) {
        if (_visibleItems[i]) {
          Future.delayed(Duration(milliseconds: i * 150), () {
            if (mounted) _itemControllers[i].forward();
          });
        }
      }
    });
  }
  
  @override
  void dispose() {
    _scrollController.dispose();
    for (var controller in _itemControllers) {
      controller.dispose();
    }
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        // Enhanced Instructions
        Container(
          padding: EdgeInsets.all(24.w),
          decoration: BoxDecoration(
            gradient: LinearGradient(
              colors: [
                AppColors.gradientPink.withOpacity(0.15),
                AppColors.accentLavender.withOpacity(0.1),
              ],
              begin: Alignment.topLeft,
              end: Alignment.bottomRight,
            ),
            borderRadius: BorderRadius.circular(20.r),
            border: Border.all(
              color: AppColors.gradientPink.withOpacity(0.2),
              width: 1.5.w,
            ),
            boxShadow: [
              BoxShadow(
                color: AppColors.gradientPink.withOpacity(0.1),
                blurRadius: 12.r,
                offset: Offset(0, 4.h),
              ),
            ],
          ),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Row(
                children: [
                  Container(
                    padding: EdgeInsets.all(12.w),
                    decoration: BoxDecoration(
                      gradient: LinearGradient(
                        colors: [AppColors.gradientPink, AppColors.accentLavender],
                      ),
                      borderRadius: BorderRadius.circular(12.r),
                    ),
                    child: Icon(
                      Icons.favorite_rounded,
                      color: Colors.white,
                      size: 24.sp,
                    ),
                  ),
                  SizedBox(width: 16.w),
                  Expanded(
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text(
                          'Daily Gratitudes',
                          style: TextStyle(
                            fontSize: 18.sp,
                            fontWeight: FontWeight.bold,
                            color: AppColors.textLight,
                          ),
                        ),
                        SizedBox(height: 4.h),
                        Text(
                          'Reflect on what brings joy to your life',
                          style: TextStyle(
                            fontSize: 14.sp,
                            color: AppColors.textMedium,
                            height: 1.3,
                          ),
                        ),
                      ],
                    ),
                  ),
                ],
              ),
            ],
          ),
        ),
        
        SizedBox(height: 32.h),
        
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
        items.add(
          Container(
            margin: EdgeInsets.only(bottom: 24.h),
            child: SlideTransition(
              position: _slideAnimations[i],
              child: ScaleTransition(
                scale: _itemAnimations[i],
                child: _buildEnhancedGratitudeItem(i),
              ),
            ),
          ),
        );
      }
    }
    return items;
  }

  Widget _buildEnhancedGratitudeItem(int index) {
    final isFirstItem = index == 0;
    final canRemove = !isFirstItem && _getVisibleCount() > 1;
    final hasContent = widget.gratitudeItems[index].isNotEmpty;
    
    return AnimatedContainer(
      duration: Duration(milliseconds: 400),
      curve: Curves.easeInOutCubic,
      decoration: BoxDecoration(
        gradient: LinearGradient(
          colors: hasContent 
            ? [
                AppColors.cardBackground,
                AppColors.cardBackground.withOpacity(0.8),
              ]
            : [
                AppColors.cardBackground.withOpacity(0.6),
                AppColors.cardBackground.withOpacity(0.4),
              ],
          begin: Alignment.topLeft,
          end: Alignment.bottomRight,
        ),
        borderRadius: BorderRadius.circular(20.r),
        border: Border.all(
          color: hasContent
              ? AppColors.gradientPink.withOpacity(0.4)
              : AppColors.textMedium.withOpacity(0.2),
          width: hasContent ? 2.w : 1.w,
        ),
        boxShadow: [
          BoxShadow(
            color: hasContent 
              ? AppColors.gradientPink.withOpacity(0.15)
              : AppColors.black.withOpacity(0.05),
            blurRadius: hasContent ? 16.r : 8.r,
            offset: Offset(0, hasContent ? 6.h : 2.h),
          ),
        ],
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          // Enhanced header with number indicator
          Container(
            padding: EdgeInsets.fromLTRB(24.w, 20.h, 16.w, 12.h),
            child: Row(
              children: [
                // Animated number indicator
                AnimatedContainer(
                  duration: Duration(milliseconds: 400),
                  curve: Curves.easeInOutCubic,
                  width: 32.w,
                  height: 32.h,
                  decoration: BoxDecoration(
                    gradient: LinearGradient(
                      colors: hasContent 
                        ? [AppColors.gradientPink, AppColors.accentLavender]
                        : [AppColors.textMedium.withOpacity(0.3), AppColors.textMedium.withOpacity(0.2)],
                    ),
                    borderRadius: BorderRadius.circular(16.r),
                    boxShadow: hasContent ? [
                      BoxShadow(
                        color: AppColors.gradientPink.withOpacity(0.3),
                        blurRadius: 8.r,
                        offset: Offset(0, 2.h),
                      ),
                    ] : null,
                  ),
                  child: Center(
                    child: AnimatedDefaultTextStyle(
                      duration: Duration(milliseconds: 400),
                      curve: Curves.easeInOutCubic,
                      style: TextStyle(
                        fontSize: 14.sp,
                        fontWeight: FontWeight.bold,
                        color: hasContent ? Colors.white : AppColors.textMedium,
                      ),
                      child: Text('${index + 1}'),
                    ),
                  ),
                ),
                SizedBox(width: 16.w),
                Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(
                        isFirstItem ? 'I\'m grateful for...' : 'Also grateful for...',
                        style: TextStyle(
                          fontSize: 16.sp,
                          fontWeight: FontWeight.w600,
                          color: AppColors.textLight,
                        ),
                      ),
                      SizedBox(height: 4.h),
                      AnimatedContainer(
                        duration: Duration(milliseconds: 500),
                        curve: Curves.easeOutCubic,
                        height: 2.h,
                        width: hasContent ? 40.w : 0.w,
                        decoration: BoxDecoration(
                          gradient: LinearGradient(
                            colors: [AppColors.gradientPink, AppColors.accentLavender],
                          ),
                          borderRadius: BorderRadius.circular(1.r),
                        ),
                      ),
                    ],
                  ),
                ),
                if (canRemove)
                  GestureDetector(
                    onTap: () => _removeGratitudeItem(index),
                    child: Container(
                      width: 36.w,
                      height: 36.h,
                      decoration: BoxDecoration(
                        color: AppColors.error.withOpacity(0.1),
                        borderRadius: BorderRadius.circular(18.r),
                        border: Border.all(
                          color: AppColors.error.withOpacity(0.3),
                        ),
                      ),
                      child: Icon(
                        Icons.close_rounded,
                        size: 18.sp,
                        color: AppColors.error.withOpacity(0.8),
                      ),
                    ),
                  ),
              ],
            ),
          ),
          
          // Enhanced text field
          Padding(
            padding: EdgeInsets.fromLTRB(24.w, 0, 24.w, 24.h),
            child: Container(
              decoration: BoxDecoration(
                color: AppColors.backgroundDark.withOpacity(0.3),
                borderRadius: BorderRadius.circular(16.r),
                border: Border.all(
                  color: AppColors.textMedium.withOpacity(0.1),
                ),
              ),
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
                  height: 1.6,
                  fontWeight: FontWeight.w400,
                ),
                decoration: InputDecoration(
                  hintText: _getGratitudeHint(index),
                  hintStyle: TextStyle(
                    color: AppColors.textMedium.withOpacity(0.6),
                    fontSize: 16.sp,
                    height: 1.6,
                    fontStyle: FontStyle.italic,
                  ),
                  border: InputBorder.none,
                  contentPadding: EdgeInsets.all(20.w),
                  isDense: false,
                ),
                maxLines: 4,
                minLines: 3,
                textCapitalization: TextCapitalization.sentences,
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildAddButton() {
    return Container(
      margin: EdgeInsets.only(top: 16.h, bottom: 24.h),
      child: GestureDetector(
        onTap: _addGratitudeItem,
        child: Container(
          padding: EdgeInsets.symmetric(vertical: 20.h, horizontal: 24.w),
          decoration: BoxDecoration(
            gradient: LinearGradient(
              colors: [
                AppColors.gradientPink.withOpacity(0.15),
                AppColors.accentLavender.withOpacity(0.1),
              ],
              begin: Alignment.topLeft,
              end: Alignment.bottomRight,
            ),
            borderRadius: BorderRadius.circular(20.r),
            border: Border.all(
              color: AppColors.gradientPink.withOpacity(0.3),
              width: 2.w,
              style: BorderStyle.solid,
            ),
            boxShadow: [
              BoxShadow(
                color: AppColors.gradientPink.withOpacity(0.1),
                blurRadius: 12.r,
                offset: Offset(0, 4.h),
              ),
            ],
          ),
          child: Row(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              Container(
                padding: EdgeInsets.all(8.w),
                decoration: BoxDecoration(
                  gradient: LinearGradient(
                    colors: [AppColors.gradientPink, AppColors.accentLavender],
                  ),
                  borderRadius: BorderRadius.circular(12.r),
                ),
                child: Icon(
                  Icons.add_rounded,
                  color: Colors.white,
                  size: 20.sp,
                ),
              ),
              SizedBox(width: 16.w),
              Text(
                'Add Another Gratitude',
                style: TextStyle(
                  fontSize: 17.sp,
                  fontWeight: FontWeight.w600,
                  color: AppColors.textLight,
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }

  void _addGratitudeItem() {
    int newIndex = -1;
    setState(() {
      // Find the first non-visible item and make it visible
      for (int i = 0; i < _visibleItems.length; i++) {
        if (!_visibleItems[i]) {
          _visibleItems[i] = true;
          newIndex = i;
          break;
        }
      }
    });
    
    // Animate the new item
    if (newIndex != -1) {
      Future.delayed(Duration(milliseconds: 100), () {
        if (mounted) {
          _itemControllers[newIndex].forward();
        }
      });
    }
    
    // Auto scroll to the new item after a brief delay
    Future.delayed(Duration(milliseconds: 300), () {
      if (_scrollController.hasClients) {
        _scrollController.animateTo(
          _scrollController.position.maxScrollExtent,
          duration: Duration(milliseconds: 500),
          curve: Curves.easeOutBack,
        );
      }
    });
  }

  void _removeGratitudeItem(int index) {
    // Animate out first
    _itemControllers[index].reverse().then((_) {
      if (mounted) {
        setState(() {
          _visibleItems[index] = false;
          // Clear the text when removing
          widget.onGratitudeChanged(index, '');
        });
      }
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
        return 'Someone who brought joy to your day...';
      case 1:
        return 'A moment that filled your heart with warmth...';
      case 2:
        return 'Something beautiful you noticed today...';
      case 3:
        return 'An experience that made you feel alive...';
      case 4:
        return 'A blessing that touched your soul...';
      default:
        return 'Something that fills you with gratitude...';
    }
  }
}