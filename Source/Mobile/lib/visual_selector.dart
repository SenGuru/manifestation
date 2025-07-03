import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:manifestation_app/colors.dart';
import 'package:manifestation_app/meditation_constants.dart';
import 'package:manifestation_app/meditation_models.dart';

class VisualSelector extends StatefulWidget {
  final VisualType selectedVisual;
  final Function(VisualType) onVisualChanged;
  final bool showDescription;

  const VisualSelector({
    Key? key,
    required this.selectedVisual,
    required this.onVisualChanged,
    this.showDescription = true,
  }) : super(key: key);

  @override
  _VisualSelectorState createState() => _VisualSelectorState();
}

class _VisualSelectorState extends State<VisualSelector>
    with TickerProviderStateMixin {
  late AnimationController _selectionController;
  late Animation<double> _scaleAnimation;

  @override
  void initState() {
    super.initState();
    
    _selectionController = AnimationController(
      duration: AppConstants.normalAnimation,
      vsync: this,
    );
    
    _scaleAnimation = Tween<double>(
      begin: 1.0,
      end: 1.1,
    ).animate(CurvedAnimation(
      parent: _selectionController,
      curve: Curves.easeInOut,
    ));
  }

  @override
  void dispose() {
    _selectionController.dispose();
    super.dispose();
  }

  void _onVisualSelected(VisualType visual) {
    if (visual != widget.selectedVisual) {
      _selectionController.forward().then((_) {
        _selectionController.reverse();
      });
      widget.onVisualChanged(visual);
    }
  }

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Visual Experience',
          style: TextStyle(
            fontSize: 18.sp,
            fontWeight: FontWeight.w600,
            color: AppColors.textLight,
          ),
        ),
        if (widget.showDescription) ...[
          SizedBox(height: 4.h),
          Text(
            'Choose your meditation visual companion',
            style: TextStyle(
              fontSize: 14.sp,
              color: AppColors.textMedium,
            ),
          ),
        ],
        SizedBox(height: 16.h),
        Container(
          height: 140.h,
          child: ListView.builder(
            scrollDirection: Axis.horizontal,
            itemCount: VisualType.values.length,
            itemBuilder: (context, index) {
              final visualType = VisualType.values[index];
              final isSelected = widget.selectedVisual == visualType;
              
              return Padding(
                padding: EdgeInsets.only(right: 16.w),
                child: _buildVisualCard(visualType, isSelected),
              );
            },
          ),
        ),
      ],
    );
  }

  Widget _buildVisualCard(VisualType visualType, bool isSelected) {
    final gradient = _getVisualGradient(visualType);
    
    return GestureDetector(
      onTap: () => _onVisualSelected(visualType),
      child: AnimatedBuilder(
        animation: _scaleAnimation,
        builder: (context, child) {
          return Transform.scale(
            scale: isSelected ? _scaleAnimation.value : 1.0,
            child: AnimatedContainer(
              duration: AppConstants.normalAnimation,
              curve: Curves.easeInOut,
              width: 110.w,
              decoration: BoxDecoration(
                borderRadius: BorderRadius.circular(20.r),
                gradient: isSelected
                    ? LinearGradient(
                        begin: Alignment.topLeft,
                        end: Alignment.bottomRight,
                        colors: gradient,
                      )
                    : null,
                color: isSelected ? null : AppColors.cardBackground,
                border: Border.all(
                  color: isSelected
                      ? Colors.transparent
                      : AppColors.textMedium.withOpacity(0.1),
                  width: 1.5.w,
                ),
                boxShadow: isSelected
                    ? [
                        BoxShadow(
                          color: gradient[0].withOpacity(0.3),
                          blurRadius: 20.r,
                          offset: Offset(0, 8.h),
                        ),
                      ]
                    : [],
              ),
              child: Column(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  // Icon container
                  AnimatedContainer(
                    duration: AppConstants.normalAnimation,
                    width: 48.w,
                    height: 48.h,
                    decoration: BoxDecoration(
                      shape: BoxShape.circle,
                      color: isSelected
                          ? AppColors.white.withOpacity(0.2)
                          : AppColors.textMedium.withOpacity(0.1),
                    ),
                    child: Icon(
                      visualType.icon,
                      color: isSelected ? AppColors.white : AppColors.textMedium,
                      size: 24.sp,
                    ),
                  ),
                  
                  SizedBox(height: 12.h),
                  
                  // Visual type name
                  Text(
                    visualType.name,
                    style: TextStyle(
                      fontSize: 14.sp,
                      fontWeight: FontWeight.w600,
                      color: isSelected ? AppColors.white : AppColors.textMedium,
                    ),
                  ),
                  
                  SizedBox(height: 4.h),
                  
                  // Description
                  Padding(
                    padding: EdgeInsets.symmetric(horizontal: 8.w),
                    child: Text(
                      visualType.description,
                      textAlign: TextAlign.center,
                      style: TextStyle(
                        fontSize: 10.sp,
                        color: isSelected 
                            ? AppColors.white.withOpacity(0.8) 
                            : AppColors.textMedium.withOpacity(0.6),
                      ),
                      maxLines: 2,
                      overflow: TextOverflow.ellipsis,
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

  List<Color> _getVisualGradient(VisualType visualType) {
    switch (visualType) {
      case VisualType.candle:
        return AppColors.candleGradient;
      case VisualType.moon:
        return AppColors.moonGradient;
      case VisualType.nature:
        return AppColors.natureGradient;
      case VisualType.minimal:
        return AppColors.minimalGradient;
      case VisualType.geometric:
        return AppColors.geometricGradient;
    }
  }
}

// Compact visual selector for limited space
class CompactVisualSelector extends StatelessWidget {
  final VisualType selectedVisual;
  final Function(VisualType) onVisualChanged;

  const CompactVisualSelector({
    Key? key,
    required this.selectedVisual,
    required this.onVisualChanged,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Container(
      height: 60.h,
      child: ListView.builder(
        scrollDirection: Axis.horizontal,
        itemCount: VisualType.values.length,
        itemBuilder: (context, index) {
          final visualType = VisualType.values[index];
          final isSelected = selectedVisual == visualType;
          final gradient = _getVisualGradient(visualType);
          
          return Padding(
            padding: EdgeInsets.only(right: 12.w),
            child: GestureDetector(
              onTap: () => onVisualChanged(visualType),
              child: AnimatedContainer(
                duration: AppConstants.fastAnimation,
                width: 60.w,
                height: 60.h,
                decoration: BoxDecoration(
                  gradient: isSelected
                      ? LinearGradient(colors: gradient)
                      : null,
                  color: isSelected ? null : AppColors.cardBackground,
                  borderRadius: BorderRadius.circular(16.r),
                  border: Border.all(
                    color: isSelected
                        ? Colors.transparent
                        : AppColors.textMedium.withOpacity(0.2),
                    width: 1.w,
                  ),
                ),
                child: Icon(
                  visualType.icon,
                  color: isSelected ? AppColors.white : AppColors.textMedium,
                  size: 24.sp,
                ),
              ),
            ),
          );
        },
      ),
    );
  }

  List<Color> _getVisualGradient(VisualType visualType) {
    switch (visualType) {
      case VisualType.candle:
        return AppColors.candleGradient;
      case VisualType.moon:
        return AppColors.moonGradient;
      case VisualType.nature:
        return AppColors.natureGradient;
      case VisualType.minimal:
        return AppColors.minimalGradient;
      case VisualType.geometric:
        return AppColors.geometricGradient;
    }
  }
}

// Visual type customization options
class VisualCustomization extends StatelessWidget {
  final VisualType visualType;
  final Map<String, dynamic> settings;
  final Function(Map<String, dynamic>) onSettingsChanged;

  const VisualCustomization({
    Key? key,
    required this.visualType,
    required this.settings,
    required this.onSettingsChanged,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    switch (visualType) {
      case VisualType.candle:
        return _buildCandleCustomization();
      case VisualType.moon:
        return _buildMoonCustomization();
      case VisualType.nature:
        return _buildNatureCustomization();
      case VisualType.minimal:
        return _buildMinimalCustomization();
      case VisualType.geometric:
        return _buildGeometricCustomization();
    }
  }

  Widget _buildCandleCustomization() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Candle Color',
          style: TextStyle(
            fontSize: 16.sp,
            fontWeight: FontWeight.w600,
            color: AppColors.textLight,
          ),
        ),
        SizedBox(height: 12.h),
        Container(
          height: 80.h,
          child: ListView.builder(
            scrollDirection: Axis.horizontal,
            itemCount: CandleColor.values.length,
            itemBuilder: (context, index) {
              final color = CandleColor.values[index];
              final isSelected = settings['candleColor'] == color;
              
              return Padding(
                padding: EdgeInsets.only(right: 12.w),
                child: GestureDetector(
                  onTap: () {
                    final newSettings = Map<String, dynamic>.from(settings);
                    newSettings['candleColor'] = color;
                    onSettingsChanged(newSettings);
                  },
                  child: AnimatedContainer(
                    duration: AppConstants.fastAnimation,
                    width: 70.w,
                    decoration: BoxDecoration(
                      color: isSelected
                          ? color.primaryColor.withOpacity(0.2)
                          : AppColors.cardBackground,
                      borderRadius: BorderRadius.circular(16.r),
                      border: Border.all(
                        color: isSelected
                            ? color.primaryColor
                            : AppColors.textMedium.withOpacity(0.2),
                        width: 2.w,
                      ),
                    ),
                    child: Column(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [
                        Container(
                          width: 24.w,
                          height: 24.h,
                          decoration: BoxDecoration(
                            shape: BoxShape.circle,
                            color: color.primaryColor,
                            boxShadow: isSelected
                                ? [
                                    BoxShadow(
                                      color: color.primaryColor.withOpacity(0.5),
                                      blurRadius: 8.r,
                                      offset: Offset(0, 2.h),
                                    ),
                                  ]
                                : [],
                          ),
                        ),
                        SizedBox(height: 8.h),
                        Text(
                          color.name,
                          style: TextStyle(
                            fontSize: 12.sp,
                            fontWeight: FontWeight.w500,
                            color: isSelected ? color.primaryColor : AppColors.textMedium,
                          ),
                        ),
                      ],
                    ),
                  ),
                ),
              );
            },
          ),
        ),
      ],
    );
  }

  Widget _buildMoonCustomization() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Moon Phase',
          style: TextStyle(
            fontSize: 16.sp,
            fontWeight: FontWeight.w600,
            color: AppColors.textLight,
          ),
        ),
        SizedBox(height: 12.h),
        Container(
          height: 80.h,
          child: ListView.builder(
            scrollDirection: Axis.horizontal,
            itemCount: MoonPhase.values.length,
            itemBuilder: (context, index) {
              final phase = MoonPhase.values[index];
              final isSelected = settings['moonPhase'] == phase;
              
              return Padding(
                padding: EdgeInsets.only(right: 12.w),
                child: GestureDetector(
                  onTap: () {
                    final newSettings = Map<String, dynamic>.from(settings);
                    newSettings['moonPhase'] = phase;
                    onSettingsChanged(newSettings);
                  },
                  child: AnimatedContainer(
                    duration: AppConstants.fastAnimation,
                    width: 90.w,
                    decoration: BoxDecoration(
                      color: isSelected
                          ? phase.glowColor.withOpacity(0.1)
                          : AppColors.cardBackground,
                      borderRadius: BorderRadius.circular(16.r),
                      border: Border.all(
                        color: isSelected
                            ? phase.glowColor
                            : AppColors.textMedium.withOpacity(0.2),
                        width: 2.w,
                      ),
                    ),
                    child: Column(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [
                        Container(
                          width: 20.w,
                          height: 20.h,
                          decoration: BoxDecoration(
                            shape: BoxShape.circle,
                            gradient: RadialGradient(
                              colors: phase.moonGradient,
                            ),
                            boxShadow: isSelected
                                ? [
                                    BoxShadow(
                                      color: phase.glowColor.withOpacity(0.5),
                                      blurRadius: 8.r,
                                      offset: Offset(0, 2.h),
                                    ),
                                  ]
                                : [],
                          ),
                        ),
                        SizedBox(height: 8.h),
                        Text(
                          phase.name,
                          textAlign: TextAlign.center,
                          style: TextStyle(
                            fontSize: 10.sp,
                            fontWeight: FontWeight.w500,
                            color: isSelected ? phase.glowColor : AppColors.textMedium,
                          ),
                        ),
                      ],
                    ),
                  ),
                ),
              );
            },
          ),
        ),
      ],
    );
  }

  Widget _buildNatureCustomization() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Nature Pattern',
          style: TextStyle(
            fontSize: 16.sp,
            fontWeight: FontWeight.w600,
            color: AppColors.textLight,
          ),
        ),
        SizedBox(height: 12.h),
        Container(
          height: 80.h,
          child: ListView.builder(
            scrollDirection: Axis.horizontal,
            itemCount: NaturePattern.values.length,
            itemBuilder: (context, index) {
              final pattern = NaturePattern.values[index];
              final isSelected = settings['naturePattern'] == pattern;
              
              return Padding(
                padding: EdgeInsets.only(right: 12.w),
                child: GestureDetector(
                  onTap: () {
                    final newSettings = Map<String, dynamic>.from(settings);
                    newSettings['naturePattern'] = pattern;
                    onSettingsChanged(newSettings);
                  },
                  child: AnimatedContainer(
                    duration: AppConstants.fastAnimation,
                    width: 70.w,
                    decoration: BoxDecoration(
                      gradient: isSelected
                          ? LinearGradient(colors: pattern.colors)
                          : null,
                      color: isSelected ? null : AppColors.cardBackground,
                      borderRadius: BorderRadius.circular(16.r),
                      border: Border.all(
                        color: isSelected
                            ? Colors.transparent
                            : AppColors.textMedium.withOpacity(0.2),
                        width: 2.w,
                      ),
                    ),
                    child: Column(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [
                        Icon(
                          _getNaturePatternIcon(pattern),
                          color: isSelected ? AppColors.white : AppColors.textMedium,
                          size: 24.sp,
                        ),
                        SizedBox(height: 8.h),
                        Text(
                          pattern.name,
                          style: TextStyle(
                            fontSize: 12.sp,
                            fontWeight: FontWeight.w500,
                            color: isSelected ? AppColors.white : AppColors.textMedium,
                          ),
                        ),
                      ],
                    ),
                  ),
                ),
              );
            },
          ),
        ),
      ],
    );
  }

  Widget _buildMinimalCustomization() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Breathing Speed',
          style: TextStyle(
            fontSize: 16.sp,
            fontWeight: FontWeight.w600,
            color: AppColors.textLight,
          ),
        ),
        SizedBox(height: 12.h),
        Container(
          padding: EdgeInsets.symmetric(horizontal: 16.w),
          child: Row(
            children: [
              Text(
                'Slow',
                style: TextStyle(
                  fontSize: 12.sp,
                  color: AppColors.textMedium,
                ),
              ),
              Expanded(
                child: Slider(
                  value: settings['breathingSpeed'] ?? 0.8,
                  min: 0.5,
                  max: 1.5,
                  divisions: 10,
                  activeColor: AppColors.minimalGradient[0],
                  inactiveColor: AppColors.textMedium.withOpacity(0.2),
                  onChanged: (value) {
                    final newSettings = Map<String, dynamic>.from(settings);
                    newSettings['breathingSpeed'] = value;
                    onSettingsChanged(newSettings);
                  },
                ),
              ),
              Text(
                'Fast',
                style: TextStyle(
                  fontSize: 12.sp,
                  color: AppColors.textMedium,
                ),
              ),
            ],
          ),
        ),
      ],
    );
  }

  Widget _buildGeometricCustomization() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Geometric Pattern',
          style: TextStyle(
            fontSize: 16.sp,
            fontWeight: FontWeight.w600,
            color: AppColors.textLight,
          ),
        ),
        SizedBox(height: 12.h),
        Container(
          height: 80.h,
          child: ListView.builder(
            scrollDirection: Axis.horizontal,
            itemCount: GeometricPattern.values.length,
            itemBuilder: (context, index) {
              final pattern = GeometricPattern.values[index];
              final isSelected = settings['geometricPattern'] == pattern;
              
              return Padding(
                padding: EdgeInsets.only(right: 12.w),
                child: GestureDetector(
                  onTap: () {
                    final newSettings = Map<String, dynamic>.from(settings);
                    newSettings['geometricPattern'] = pattern;
                    onSettingsChanged(newSettings);
                  },
                  child: AnimatedContainer(
                    duration: AppConstants.fastAnimation,
                    width: 90.w,
                    decoration: BoxDecoration(
                      gradient: isSelected
                          ? LinearGradient(colors: pattern.colors)
                          : null,
                      color: isSelected ? null : AppColors.cardBackground,
                      borderRadius: BorderRadius.circular(16.r),
                      border: Border.all(
                        color: isSelected
                            ? Colors.transparent
                            : AppColors.textMedium.withOpacity(0.2),
                        width: 2.w,
                      ),
                    ),
                    child: Column(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [
                        Icon(
                          _getGeometricPatternIcon(pattern),
                          color: isSelected ? AppColors.white : AppColors.textMedium,
                          size: 24.sp,
                        ),
                        SizedBox(height: 8.h),
                        Text(
                          pattern.name,
                          textAlign: TextAlign.center,
                          style: TextStyle(
                            fontSize: 10.sp,
                            fontWeight: FontWeight.w500,
                            color: isSelected ? AppColors.white : AppColors.textMedium,
                          ),
                        ),
                      ],
                    ),
                  ),
                ),
              );
            },
          ),
        ),
      ],
    );
  }

  IconData _getNaturePatternIcon(NaturePattern pattern) {
    switch (pattern) {
      case NaturePattern.ripples:
        return Icons.radio_button_unchecked;
      case NaturePattern.waves:
        return Icons.waves;
      case NaturePattern.flow:
        return Icons.trending_up;
      case NaturePattern.rain:
        return Icons.grain;
    }
  }

  IconData _getGeometricPatternIcon(GeometricPattern pattern) {
    switch (pattern) {
      case GeometricPattern.mandala:
        return Icons.filter_vintage;
      case GeometricPattern.flowerOfLife:
        return Icons.local_florist;
      case GeometricPattern.spirals:
        return Icons.rotate_right;
      case GeometricPattern.fractals:
        return Icons.insights;
    }
  }
}