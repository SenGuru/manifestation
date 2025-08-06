import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../constants/app_theme.dart';
import '../models/meditation_models.dart';
import '../services/meditation_audio_service.dart';

class AudioSelector extends StatefulWidget {
  final AudioType selectedAudio;
  final Function(AudioType) onAudioChanged;
  final bool showDescription;

  const AudioSelector({
    Key? key,
    required this.selectedAudio,
    required this.onAudioChanged,
    this.showDescription = true,
  }) : super(key: key);

  @override
  _AudioSelectorState createState() => _AudioSelectorState();
}

class _AudioSelectorState extends State<AudioSelector>
    with TickerProviderStateMixin {
  late AnimationController _selectionController;
  late Animation<double> _scaleAnimation;

  @override
  void initState() {
    super.initState();
    
    _selectionController = AnimationController(
      duration: Duration(milliseconds: 300),
      vsync: this,
    );
    
    _scaleAnimation = Tween<double>(
      begin: 1.0,
      end: 1.05,
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

  @override
  Widget build(BuildContext context) {
    return SingleChildScrollView(
      scrollDirection: Axis.horizontal,
      child: Row(
        children: AudioType.values.map((audioType) {
          final isSelected = widget.selectedAudio == audioType;
          return GestureDetector(
            onTap: () {
              widget.onAudioChanged(audioType);
              if (isSelected) {
                _selectionController.forward().then((_) {
                  _selectionController.reverse();
                });
              }
            },
            child: AnimatedBuilder(
              animation: _scaleAnimation,
              builder: (context, child) {
                return Transform.scale(
                  scale: isSelected ? _scaleAnimation.value : 1.0,
                  child: Container(
                    width: 120.w,
                    height: 100.h,
                    margin: EdgeInsets.only(right: 12.w),
                    decoration: BoxDecoration(
                      gradient: isSelected
                          ? LinearGradient(
                              colors: audioType.colors,
                              begin: Alignment.topLeft,
                              end: Alignment.bottomRight,
                            )
                          : null,
                      color: isSelected ? null : AppTheme.cardBackground,
                      borderRadius: BorderRadius.circular(16.r),
                      border: Border.all(
                        color: isSelected
                            ? audioType.colors[0]
                            : AppTheme.textMedium.withOpacity(0.2),
                        width: isSelected ? 2.w : 1.w,
                      ),
                      boxShadow: isSelected
                          ? [
                              BoxShadow(
                                color: audioType.colors[0].withOpacity(0.3),
                                blurRadius: 12.r,
                                offset: Offset(0, 6.h),
                              ),
                            ]
                          : [
                              BoxShadow(
                                color: AppTheme.black.withOpacity(0.05),
                                blurRadius: 4.r,
                                offset: Offset(0, 2.h),
                              ),
                            ],
                    ),
                    child: Column(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [
                        Container(
                          width: 36.w,
                          height: 36.h,
                          decoration: BoxDecoration(
                            color: isSelected
                                ? AppTheme.white.withOpacity(0.2)
                                : audioType.colors[0].withOpacity(0.1),
                            borderRadius: BorderRadius.circular(12.r),
                          ),
                          child: Icon(
                            audioType.icon,
                            color: isSelected
                                ? AppTheme.white
                                : audioType.colors[0],
                            size: 20.sp,
                          ),
                        ),
                        SizedBox(height: 8.h),
                        Text(
                          audioType.name,
                          style: TextStyle(
                            fontSize: 12.sp,
                            fontWeight: FontWeight.w600,
                            color: isSelected
                                ? AppTheme.white
                                : AppTheme.textMedium,
                          ),
                          textAlign: TextAlign.center,
                        ),
                        if (widget.showDescription) ...[
                          SizedBox(height: 4.h),
                          Text(
                            audioType.description,
                            style: TextStyle(
                              fontSize: 8.sp,
                              color: isSelected
                                  ? AppTheme.white.withOpacity(0.8)
                                  : AppTheme.textMedium.withOpacity(0.7),
                            ),
                            textAlign: TextAlign.center,
                            maxLines: 2,
                            overflow: TextOverflow.ellipsis,
                          ),
                        ],
                      ],
                    ),
                  ),
                );
              },
            ),
          );
        }).toList(),
      ),
    );
  }
}

// Compact version for smaller spaces
class CompactAudioSelector extends StatefulWidget {
  final AudioType selectedAudio;
  final Function(AudioType) onAudioChanged;

  const CompactAudioSelector({
    Key? key,
    required this.selectedAudio,
    required this.onAudioChanged,
  }) : super(key: key);

  @override
  _CompactAudioSelectorState createState() => _CompactAudioSelectorState();
}

class _CompactAudioSelectorState extends State<CompactAudioSelector> {
  final MeditationAudioService _audioService = MeditationAudioService();

  @override
  Widget build(BuildContext context) {
    return SingleChildScrollView(
      scrollDirection: Axis.horizontal,
      child: Row(
        children: AudioType.values.map((audioType) {
          final isSelected = widget.selectedAudio == audioType;
          return GestureDetector(
            onTap: () {
              widget.onAudioChanged(audioType);
              // Preview disabled to avoid conflicts with meditation audio
              // if (audioType != AudioType.none) {
              //   _audioService.previewAudio(audioType, duration: Duration(seconds: 2));
              // }
            },
            child: Container(
              width: 80.w,
              height: 80.h,
              margin: EdgeInsets.only(right: 12.w),
              decoration: BoxDecoration(
                gradient: isSelected
                    ? LinearGradient(
                        colors: audioType.colors,
                        begin: Alignment.topLeft,
                        end: Alignment.bottomRight,
                      )
                    : null,
                color: isSelected ? null : AppTheme.cardBackground,
                borderRadius: BorderRadius.circular(12.r),
                border: Border.all(
                  color: isSelected
                      ? audioType.colors[0]
                      : AppTheme.textMedium.withOpacity(0.2),
                  width: isSelected ? 2.w : 1.w,
                ),
              ),
              child: Column(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  Icon(
                    audioType.icon,
                    color: isSelected
                        ? AppTheme.white
                        : audioType.colors[0],
                    size: 24.sp,
                  ),
                  SizedBox(height: 6.h),
                  Text(
                    audioType.name,
                    style: TextStyle(
                      fontSize: 10.sp,
                      fontWeight: FontWeight.w600,
                      color: isSelected
                          ? AppTheme.white
                          : AppTheme.textMedium,
                    ),
                    textAlign: TextAlign.center,
                  ),
                ],
              ),
            ),
          );
        }).toList(),
      ),
    );
  }
}