import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../constants/app_theme.dart';

class PremiumUpgradeDialog extends StatefulWidget {
  final String feature;
  final String customMessage;
  final VoidCallback? onUpgrade;
  // Fixed text sizing, overflow, badge cutoff, and added spacing - v4

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
  late AnimationController _transitionController;
  late Animation<double> _scaleAnimation;
  late Animation<double> _shimmerAnimation;
  
  bool _showPricing = false;
  String _selectedPlan = 'yearly'; // Default to yearly (best value)

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
    
    _transitionController = AnimationController(
      duration: Duration(milliseconds: 500),
      vsync: this,
    );
    
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
    _transitionController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Dialog(
      backgroundColor: Colors.transparent,
      child: ScaleTransition(
        scale: _scaleAnimation,
        child: Container(
          width: MediaQuery.of(context).size.width * 0.98, // Even wider - 98% of screen width
          margin: EdgeInsets.symmetric(horizontal: 4.w, vertical: 20.h), // Extremely minimal horizontal margin
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
              
              // Content with smooth transition
              Flexible(
                child: Container(
                  constraints: BoxConstraints(
                    maxHeight: 400.h, // Made shorter - reduced from 480h to 420h
                  ),
                  child: Stack(
                    children: [
                    // Original content
                    AnimatedPositioned(
                      duration: Duration(milliseconds: 600),
                      curve: Curves.easeInOutCubic,
                      left: _showPricing ? -400.w : 0,
                      right: _showPricing ? 400.w : 0,
                      top: 0,
                      child: AnimatedOpacity(
                        opacity: _showPricing ? 0.0 : 1.0,
                        duration: Duration(milliseconds: 400),
                        curve: Curves.easeInOut,
                        child: Column(
                          mainAxisSize: MainAxisSize.min,
                          children: [
                            _buildFeaturesList(),
                            _buildActionButtons(),
                          ],
                        ),
                      ),
                    ),
                    
                    // Pricing content
                    AnimatedPositioned(
                      duration: Duration(milliseconds: 600),
                      curve: Curves.easeInOutCubic,
                      left: _showPricing ? 0 : 400.w,
                      right: _showPricing ? 0 : -400.w,
                      top: 0,
                      child: AnimatedOpacity(
                        opacity: _showPricing ? 1.0 : 0.0,
                        duration: Duration(milliseconds: 400),
                        curve: Curves.easeInOut,
                        child: Container(
                          constraints: BoxConstraints(
                            maxHeight: 380.h, // Made shorter - reduced from 440h to 380h
                          ),
                          child: SingleChildScrollView(
                            child: _buildPricingContent(),
                          ),
                        ),
                      ),
                    ),
                  ],
                  ),
                ),
              ),
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
      {'icon': '🔮', 'text': 'Unlimited Vision Boards'},
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
      {'icon': '🔮', 'text': 'Unlimited Vision Boards'},
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
              onPressed: _showPricingOptions,
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

  void _showPricingOptions() {
    setState(() {
      _showPricing = true;
    });
  }

  void _goBackToFeatures() {
    setState(() {
      _showPricing = false;
    });
  }

  Widget _buildPricingContent() {
    return Padding(
      padding: EdgeInsets.all(24.w),
      child: Column(
        mainAxisSize: MainAxisSize.min,
        children: [
          Text(
            'Choose Your Plan',
            style: TextStyle(
              fontSize: 18.sp,
              fontWeight: FontWeight.bold,
              color: AppTheme.textPrimary,
            ),
          ),
          
          SizedBox(height: 24.h),
          
          // Monthly Plan
          _buildPricingOption(
            'monthly',
            'Monthly Plan',
            '\$4.99',
            'per month',
            false,
          ),
          
          SizedBox(height: 16.h),
          
          // Yearly Plan (Best Value)
          _buildPricingOption(
            'yearly',
            'Yearly Plan',
            '\$49.99',
            'per year',
            true,
            savings: 'Save 17%',
          ),
          
          SizedBox(height: 20.h),
          
          // Action buttons for pricing
          _buildPricingActionButtons(),
          
          SizedBox(height: 12.h),
          
          Text(
            'Cancel anytime. No hidden fees.',
            style: TextStyle(
              fontSize: 11.sp,
              color: AppTheme.textTertiary,
            ),
            textAlign: TextAlign.center,
          ),
        ],
      ),
    );
  }

Widget _buildPricingOption(
    String planId,
    String title,
    String price,
    String period,
    bool isPopular, {
    String? savings,
  }) {
    final bool isSelected = _selectedPlan == planId;
    
    return GestureDetector(
      onTap: () {
        setState(() {
          _selectedPlan = planId;
        });
      },
      child: AnimatedContainer(
        duration: Duration(milliseconds: 200),
        curve: Curves.easeInOut,
        width: double.infinity,
        padding: EdgeInsets.symmetric(vertical: 16.h, horizontal: 16.w),
        decoration: BoxDecoration(
          color: isSelected ? AppTheme.accentGold.withValues(alpha: 0.1) : AppTheme.cardBackground,
          borderRadius: BorderRadius.circular(16.r),
          border: Border.all(
            color: isSelected ? AppTheme.accentGold : AppTheme.accentGold.withValues(alpha: 0.3),
            width: isSelected ? 2.0 : 1.5,
          ),
          boxShadow: isSelected ? [
            BoxShadow(
              color: AppTheme.accentGold.withValues(alpha: 0.2),
              blurRadius: 8.r,
              offset: Offset(0, 2.h),
            ),
          ] : null,
        ),
        child: Row(
          children: [
            // Radio button
            Container(
              width: 24.w,
              height: 24.h,
              decoration: BoxDecoration(
                shape: BoxShape.circle,
                border: Border.all(
                  color: isSelected ? AppTheme.accentGold : AppTheme.textTertiary,
                  width: 2,
                ),
                color: isSelected ? AppTheme.accentGold : Colors.transparent,
              ),
              child: isSelected
                  ? Center(
                      child: Container(
                        width: 8.w,
                        height: 8.h,
                        decoration: BoxDecoration(
                          shape: BoxShape.circle,
                          color: AppTheme.black,
                        ),
                      ),
                    )
                  : null,
            ),
            
            SizedBox(width: 12.w),
            
            // Title and Period
            Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    title,
                    style: TextStyle(
                      fontSize: 16.sp,
                      fontWeight: FontWeight.bold,
                      color: isSelected ? AppTheme.accentGold : AppTheme.textPrimary,
                    ),
                  ),
                  SizedBox(height: 4.h),
                  Text(
                    period,
                    style: TextStyle(
                      fontSize: 13.sp,
                      color: AppTheme.textSecondary,
                    ),
                  ),
                ],
              ),
            ),

            // Price and Badge grouped together
            Column(
              crossAxisAlignment: CrossAxisAlignment.end,
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                if (isPopular)
                  Container(
                    padding: EdgeInsets.symmetric(horizontal: 8.w, vertical: 4.h),
                    decoration: BoxDecoration(
                      color: AppTheme.accentGold,
                      borderRadius: BorderRadius.circular(6.r),
                    ),
                    child: Text(
                      savings ?? 'BEST VALUE',
                      style: TextStyle(
                        fontSize: 9.sp,
                        fontWeight: FontWeight.bold,
                        color: AppTheme.black,
                      ),
                    ),
                  ),
                if (isPopular) SizedBox(height: 6.h),
                Text(
                  price,
                  style: TextStyle(
                    fontSize: 18.sp,
                    fontWeight: FontWeight.bold,
                    color: isSelected ? AppTheme.accentGold : AppTheme.textPrimary,
                  ),
                ),
              ],
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildPricingActionButtons() {
    return Column(
      children: [
        // Primary purchase button
        Container(
          width: double.infinity,
          height: 48.h,
          child: ElevatedButton(
            onPressed: () => Navigator.pop(context),
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
                  'Subscribe ${_selectedPlan == 'yearly' ? 'Yearly' : 'Monthly'}',
                  style: TextStyle(
                    fontSize: 15.sp,
                    fontWeight: FontWeight.bold,
                    color: AppTheme.black,
                  ),
                ),
              ],
            ),
          ),
        ),
        
        SizedBox(height: 8.h),
        
        // Back button
        TextButton(
          onPressed: _goBackToFeatures,
          style: TextButton.styleFrom(
            padding: EdgeInsets.symmetric(vertical: 10.h, horizontal: 20.w),
            shape: RoundedRectangleBorder(
              borderRadius: BorderRadius.circular(12.r),
            ),
          ),
          child: Text(
            'Back',
            style: TextStyle(
              fontSize: 14.sp,
              color: AppTheme.textTertiary,
              fontWeight: FontWeight.w500,
            ),
          ),
        ),
      ],
    );
  }


}