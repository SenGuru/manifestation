import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../constants/app_theme.dart';

class PremiumUpgradeDialog extends StatefulWidget {
  final String feature;
  final String customMessage;
  final VoidCallback? onUpgrade;

  const PremiumUpgradeDialog({
    Key? key,
    required this.feature,
    this.customMessage = '',
    this.onUpgrade,
  }) : super(key: key);

  @override
  _PremiumUpgradeDialogState createState() => _PremiumUpgradeDialogState();
}

class _PremiumUpgradeDialogState extends State<PremiumUpgradeDialog>
    with TickerProviderStateMixin {
  late AnimationController _scaleController;
  late AnimationController _shimmerController;
  late Animation<double> _scaleAnimation;
  late Animation<double> _shimmerAnimation;

  @override
  void initState() {
    super.initState();
    
    _scaleController = AnimationController(
      duration: Duration(milliseconds: 200),
      vsync: this,
    );
    
    _shimmerController = AnimationController(
      duration: Duration(seconds: 2),
      vsync: this,
    )..repeat();
    
    _scaleAnimation = CurvedAnimation(
      parent: _scaleController,
      curve: Curves.easeOutBack,
    );
    
    _shimmerAnimation = Tween<double>(
      begin: -2.0,
      end: 2.0,
    ).animate(CurvedAnimation(
      parent: _shimmerController,
      curve: Curves.easeInOut,
    ));
    
    _scaleController.forward();
  }

  @override
  void dispose() {
    _scaleController.dispose();
    _shimmerController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Dialog(
      backgroundColor: Colors.transparent,
      child: ScaleTransition(
        scale: _scaleAnimation,
        child: Container(
          margin: EdgeInsets.all(20.w),
          decoration: BoxDecoration(
            color: AppTheme.cardBackground,
            borderRadius: BorderRadius.circular(24.r),
            border: Border.all(
              color: AppTheme.accentGold.withValues(alpha: 0.3),
              width: 1.5,
            ),
            boxShadow: [
              BoxShadow(
                color: AppTheme.shadow,
                blurRadius: 30.r,
                offset: Offset(0, 15.h),
                spreadRadius: 0,
              ),
              BoxShadow(
                color: AppTheme.accentGold.withValues(alpha: 0.1),
                blurRadius: 50.r,
                offset: Offset(0, 5.h),
                spreadRadius: 5.r,
              ),
            ],
          ),
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              _buildHeader(),
              _buildFeaturesList(),
              _buildActionButtons(),
            ],
          ),
        ),
      ),
    );
  }

  Widget _buildHeader() {
    return Container(
      padding: EdgeInsets.fromLTRB(24.w, 28.h, 24.w, 20.h),
      child: Column(
        children: [
          // Animated crown icon with shimmer effect
          AnimatedBuilder(
            animation: _shimmerController,
            builder: (context, child) {
              return Container(
                width: 80.w,
                height: 80.h,
                decoration: BoxDecoration(
                  gradient: LinearGradient(
                    colors: [
                      AppTheme.accentGold,
                      Color(0xFFFFA726),
                      AppTheme.accentGold,
                    ],
                    stops: [0.0, 0.5, 1.0],
                    begin: Alignment.topLeft,
                    end: Alignment.bottomRight,
                    transform: GradientRotation(_shimmerAnimation.value),
                  ),
                  shape: BoxShape.circle,
                  boxShadow: [
                    BoxShadow(
                      color: AppTheme.accentGold.withValues(alpha: 0.4),
                      blurRadius: 20.r,
                      offset: Offset(0, 8.h),
                    ),
                  ],
                ),
                child: Icon(
                  Icons.workspace_premium_rounded,
                  color: AppTheme.white,
                  size: 40.sp,
                ),
              );
            },
          ),
          
          SizedBox(height: 20.h),
          
          // Title
          Text(
            'Unlock Premium',
            style: TextStyle(
              fontSize: 26.sp,
              fontWeight: FontWeight.bold,
              color: AppTheme.textPrimary,
              letterSpacing: -0.5,
            ),
            textAlign: TextAlign.center,
          ),
          
          SizedBox(height: 8.h),
          
          // Subtitle message
          Text(
            widget.customMessage.isNotEmpty 
                ? widget.customMessage 
                : 'Get unlimited access to all premium features',
            style: TextStyle(
              fontSize: 15.sp,
              color: AppTheme.textSecondary,
              height: 1.4,
              fontWeight: FontWeight.w400,
            ),
            textAlign: TextAlign.center,
            maxLines: 3,
            overflow: TextOverflow.ellipsis,
          ),
        ],
      ),
    );
  }

  Widget _buildFeaturesList() {
    final features = [
      {'icon': '✨', 'text': 'Unlimited affirmations & goals'},
      {'icon': '🎯', 'text': 'Advanced meditation sessions'},
      {'icon': '🔮', 'text': 'Custom vision boards'},
      {'icon': '📊', 'text': 'Detailed analytics & insights'},
    ];

    return Container(
      margin: EdgeInsets.symmetric(horizontal: 24.w),
      padding: EdgeInsets.all(20.w),
      decoration: BoxDecoration(
        color: AppTheme.surfaceElevated,
        borderRadius: BorderRadius.circular(16.r),
        border: Border.all(
          color: AppTheme.accentGold.withValues(alpha: 0.2),
          width: 1,
        ),
      ),
      child: Column(
        children: features.map((feature) => _buildFeatureItem(
          feature['icon']!,
          feature['text']!,
        )).toList(),
      ),
    );
  }

  Widget _buildFeatureItem(String icon, String text) {
    final features = [
      {'icon': '✨', 'text': 'Unlimited affirmations & goals'},
      {'icon': '🎯', 'text': 'Advanced meditation sessions'},
      {'icon': '🔮', 'text': 'Custom vision boards'},
      {'icon': '📊', 'text': 'Detailed analytics & insights'},
    ];
    
    final isLastItem = features.indexWhere((f) => f['text'] == text) == features.length - 1;
    
    return Padding(
      padding: EdgeInsets.only(bottom: isLastItem ? 0 : 16.h),
      child: Row(
        children: [
          Container(
            width: 36.w,
            height: 36.h,
            decoration: BoxDecoration(
              color: AppTheme.accentGold.withValues(alpha: 0.1),
              borderRadius: BorderRadius.circular(10.r),
            ),
            child: Center(
              child: Text(
                icon,
                style: TextStyle(fontSize: 18.sp),
              ),
            ),
          ),
          SizedBox(width: 16.w),
          Expanded(
            child: Text(
              text,
              style: TextStyle(
                fontSize: 16.sp,
                color: AppTheme.textPrimary,
                fontWeight: FontWeight.w500,
                height: 1.3,
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildActionButtons() {
    return Padding(
      padding: EdgeInsets.fromLTRB(24.w, 24.h, 24.w, 28.h),
      child: Column(
        children: [
          // Primary upgrade button
          Container(
            width: double.infinity,
            height: 56.h,
            child: ElevatedButton(
              onPressed: () {
                Navigator.pop(context);
                if (widget.onUpgrade != null) {
                  widget.onUpgrade!();
                } else {
                  _handleUpgrade(context);
                }
              },
              style: ElevatedButton.styleFrom(
                backgroundColor: AppTheme.accentGold,
                foregroundColor: AppTheme.black,
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(16.r),
                ),
                elevation: 0,
                shadowColor: Colors.transparent,
              ),
              child: Row(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  Icon(
                    Icons.diamond_rounded,
                    size: 20.sp,
                    color: AppTheme.black,
                  ),
                  SizedBox(width: 8.w),
                  Text(
                    'Upgrade Now',
                    style: TextStyle(
                      fontSize: 17.sp,
                      fontWeight: FontWeight.bold,
                      color: AppTheme.black,
                    ),
                  ),
                ],
              ),
            ),
          ),
          
          SizedBox(height: 12.h),
          
          // Secondary "Not now" button
          TextButton(
            onPressed: () => Navigator.pop(context),
            style: TextButton.styleFrom(
              padding: EdgeInsets.symmetric(vertical: 12.h, horizontal: 24.w),
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(12.r),
              ),
            ),
            child: Text(
              'Maybe Later',
              style: TextStyle(
                fontSize: 16.sp,
                color: AppTheme.textTertiary,
                fontWeight: FontWeight.w500,
              ),
            ),
          ),
        ],
      ),
    );
  }

  void _handleUpgrade(BuildContext context) {
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Row(
          children: [
            Icon(Icons.rocket_launch_rounded, color: AppTheme.white, size: 20.sp),
            SizedBox(width: 12.w),
            Text(
              'Premium upgrade coming soon!',
              style: TextStyle(
                color: AppTheme.white,
                fontWeight: FontWeight.w500,
              ),
            ),
          ],
        ),
        backgroundColor: AppTheme.accentGold,
        behavior: SnackBarBehavior.floating,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(12.r),
        ),
        margin: EdgeInsets.all(16.w),
      ),
    );
  }

}