import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:in_app_purchase/in_app_purchase.dart';
import '../constants/app_theme.dart';
import '../services/in_app_purchase_service.dart';
import '../widgets/stylish_loading.dart';

class PremiumPurchaseScreen extends StatefulWidget {
  @override
  _PremiumPurchaseScreenState createState() => _PremiumPurchaseScreenState();
}

class _PremiumPurchaseScreenState extends State<PremiumPurchaseScreen>
    with TickerProviderStateMixin {
  final InAppPurchaseService _iapService = InAppPurchaseService();
  
  late AnimationController _fadeController;
  late AnimationController _scaleController;
  late Animation<double> _fadeAnimation;
  late Animation<double> _scaleAnimation;
  
  bool _isLoading = true;
  bool _isPurchasing = false;
  String _selectedPlan = 'yearly'; // Default to yearly for better value
  
  @override
  void initState() {
    super.initState();
    
    _fadeController = AnimationController(
      duration: Duration(milliseconds: 800),
      vsync: this,
    );
    
    _scaleController = AnimationController(
      duration: Duration(milliseconds: 600),
      vsync: this,
    );
    
    _fadeAnimation = Tween<double>(begin: 0.0, end: 1.0).animate(
      CurvedAnimation(parent: _fadeController, curve: Curves.easeOut),
    );
    
    _scaleAnimation = Tween<double>(begin: 0.8, end: 1.0).animate(
      CurvedAnimation(parent: _scaleController, curve: Curves.elasticOut),
    );
    
    _initializePurchases();
  }
  
  @override
  void dispose() {
    _fadeController.dispose();
    _scaleController.dispose();
    super.dispose();
  }
  
  Future<void> _initializePurchases() async {
    final success = await _iapService.initialize();
    
    setState(() {
      _isLoading = false;
    });
    
    if (success) {
      _fadeController.forward();
      _scaleController.forward();
    }
  }

  @override
  Widget build(BuildContext context) {
    if (_isLoading) {
      return Scaffold(
        backgroundColor: AppTheme.backgroundPrimary,
        body: SafeArea(
          child: Column(
            children: [
              _buildHeader(),
              Expanded(
                child: StylishLoading(
                  message: 'Loading premium plans...',
                ),
              ),
            ],
          ),
        ),
      );
    }

    return Scaffold(
      backgroundColor: AppTheme.backgroundPrimary,
      body: SafeArea(
        child: FadeTransition(
          opacity: _fadeAnimation,
          child: ScaleTransition(
            scale: _scaleAnimation,
            child: Column(
              children: [
                _buildHeader(),
                Expanded(
                  child: SingleChildScrollView(
                    padding: EdgeInsets.all(24.w),
                    child: Column(
                      children: [
                        _buildHeroSection(),
                        SizedBox(height: 32.h),
                        _buildFeaturesList(),
                        SizedBox(height: 40.h),
                        _buildPricingPlans(),
                        SizedBox(height: 32.h),
                        _buildPurchaseButton(),
                        SizedBox(height: 16.h),
                        _buildRestoreButton(),
                        SizedBox(height: 24.h),
                        _buildTermsAndPrivacy(),
                      ],
                    ),
                  ),
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }

  Widget _buildHeader() {
    return Padding(
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
          Text(
            'Upgrade to Premium',
            style: TextStyle(
              fontSize: 20.sp,
              fontWeight: FontWeight.bold,
              color: AppTheme.textPrimary,
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildHeroSection() {
    return Column(
      children: [
        // Premium crown icon with gradient
        Container(
          width: 100.w,
          height: 100.h,
          decoration: BoxDecoration(
            shape: BoxShape.circle,
            gradient: LinearGradient(
              colors: [
                AppTheme.accentGold,
                Color(0xFFFFA726),
              ],
              begin: Alignment.topLeft,
              end: Alignment.bottomRight,
            ),
            boxShadow: [
              BoxShadow(
                color: AppTheme.accentGold.withValues(alpha: 0.3),
                blurRadius: 20.r,
                offset: Offset(0, 10.h),
              ),
            ],
          ),
          child: Icon(
            Icons.workspace_premium_rounded,
            color: AppTheme.white,
            size: 50.sp,
          ),
        ),
        
        SizedBox(height: 24.h),
        
        Text(
          'Unlock Your Full\nManifestation Power',
          style: TextStyle(
            fontSize: 28.sp,
            fontWeight: FontWeight.bold,
            color: AppTheme.textPrimary,
            height: 1.2,
          ),
          textAlign: TextAlign.center,
        ),
        
        SizedBox(height: 12.h),
        
        Text(
          'Transform your life with unlimited access to all premium features',
          style: TextStyle(
            fontSize: 16.sp,
            color: AppTheme.textSecondary,
            height: 1.4,
          ),
          textAlign: TextAlign.center,
        ),
      ],
    );
  }

  Widget _buildFeaturesList() {
    final features = [
      {'icon': '✨', 'title': 'Unlimited Affirmations', 'desc': 'Create as many custom affirmation categories as you want'},
      {'icon': '🎯', 'title': 'Advanced Goals', 'desc': 'Set unlimited goals with detailed tracking and milestones'},
      {'icon': '🔮', 'title': 'Premium Vision Boards', 'desc': 'Create unlimited vision boards with advanced customization'},
      {'icon': '📊', 'title': 'Detailed Analytics', 'desc': 'Deep insights into your manifestation journey and progress'},
      {'icon': '🧘', 'title': 'Exclusive Meditations', 'desc': 'Access to premium guided meditation sessions'},
      {'icon': '🎨', 'title': 'Custom Themes', 'desc': 'Personalize your app with beautiful premium themes'},
    ];

    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Premium Features',
          style: TextStyle(
            fontSize: 20.sp,
            fontWeight: FontWeight.bold,
            color: AppTheme.textPrimary,
          ),
        ),
        
        SizedBox(height: 20.h),
        
        ...features.map((feature) => _buildFeatureItem(
          feature['icon']!,
          feature['title']!,
          feature['desc']!,
        )),
      ],
    );
  }

  Widget _buildFeatureItem(String icon, String title, String description) {
    return Padding(
      padding: EdgeInsets.only(bottom: 20.h),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Container(
            width: 44.w,
            height: 44.h,
            decoration: BoxDecoration(
              color: AppTheme.accentGold.withValues(alpha: 0.1),
              borderRadius: BorderRadius.circular(12.r),
            ),
            child: Center(
              child: Text(
                icon,
                style: TextStyle(fontSize: 20.sp),
              ),
            ),
          ),
          
          SizedBox(width: 16.w),
          
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  title,
                  style: TextStyle(
                    fontSize: 16.sp,
                    fontWeight: FontWeight.w600,
                    color: AppTheme.textPrimary,
                  ),
                ),
                
                SizedBox(height: 4.h),
                
                Text(
                  description,
                  style: TextStyle(
                    fontSize: 14.sp,
                    color: AppTheme.textSecondary,
                    height: 1.3,
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildPricingPlans() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Choose Your Plan',
          style: TextStyle(
            fontSize: 20.sp,
            fontWeight: FontWeight.bold,
            color: AppTheme.textPrimary,
          ),
        ),
        
        SizedBox(height: 20.h),
        
        // Monthly Plan
        _buildPricingCard(
          'monthly',
          'Monthly',
          _iapService.monthlyProduct?.price ?? '\$9.99',
          'per month',
          false,
        ),
        
        SizedBox(height: 16.h),
        
        // Yearly Plan (Best Value)
        _buildPricingCard(
          'yearly',
          'Yearly',
          _iapService.yearlyProduct?.price ?? '\$99.99',
          'per year',
          true,
          savings: '20% OFF',
        ),
      ],
    );
  }

  Widget _buildPricingCard(
    String planId,
    String title,
    String price,
    String period,
    bool isPopular, {
    String? savings,
  }) {
    final isSelected = _selectedPlan == planId;
    
    return GestureDetector(
      onTap: () => setState(() => _selectedPlan = planId),
      child: Container(
        width: double.infinity,
        decoration: BoxDecoration(
          color: isSelected ? AppTheme.accentGold.withValues(alpha: 0.1) : AppTheme.cardBackground,
          borderRadius: BorderRadius.circular(16.r),
          border: Border.all(
            color: isSelected ? AppTheme.accentGold : AppTheme.textSecondary.withValues(alpha: 0.1),
            width: isSelected ? 2 : 1,
          ),
        ),
        child: Stack(
          children: [
            Padding(
              padding: EdgeInsets.all(20.w),
              child: Row(
                children: [
                  // Selection indicator
                  Container(
                    width: 24.w,
                    height: 24.h,
                    decoration: BoxDecoration(
                      shape: BoxShape.circle,
                      border: Border.all(
                        color: isSelected ? AppTheme.accentGold : AppTheme.textSecondary.withValues(alpha: 0.3),
                        width: 2,
                      ),
                      color: isSelected ? AppTheme.accentGold : Colors.transparent,
                    ),
                    child: isSelected ? Icon(
                      Icons.check,
                      color: AppTheme.white,
                      size: 16.sp,
                    ) : null,
                  ),
                  
                  SizedBox(width: 16.w),
                  
                  Expanded(
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text(
                          title,
                          style: TextStyle(
                            fontSize: 18.sp,
                            fontWeight: FontWeight.bold,
                            color: AppTheme.textPrimary,
                          ),
                        ),
                        
                        SizedBox(height: 4.h),
                        
                        Text(
                          period,
                          style: TextStyle(
                            fontSize: 14.sp,
                            color: AppTheme.textSecondary,
                          ),
                        ),
                      ],
                    ),
                  ),
                  
                  Text(
                    price,
                    style: TextStyle(
                      fontSize: 20.sp,
                      fontWeight: FontWeight.bold,
                      color: AppTheme.textPrimary,
                    ),
                  ),
                ],
              ),
            ),
            
            // Popular badge
            if (isPopular)
              Positioned(
                top: 12.h,
                right: 12.w,
                child: Container(
                  padding: EdgeInsets.symmetric(horizontal: 8.w, vertical: 4.h),
                  decoration: BoxDecoration(
                    color: AppTheme.accentGold,
                    borderRadius: BorderRadius.circular(8.r),
                  ),
                  child: Text(
                    savings ?? 'BEST VALUE',
                    style: TextStyle(
                      fontSize: 10.sp,
                      fontWeight: FontWeight.bold,
                      color: AppTheme.black,
                    ),
                  ),
                ),
              ),
          ],
        ),
      ),
    );
  }

  Widget _buildPurchaseButton() {
    return Container(
      width: double.infinity,
      height: 56.h,
      child: ElevatedButton(
        onPressed: _isPurchasing ? null : _handlePurchase,
        style: ElevatedButton.styleFrom(
          backgroundColor: AppTheme.accentGold,
          foregroundColor: AppTheme.black,
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(16.r),
          ),
          elevation: 0,
          shadowColor: Colors.transparent,
        ),
        child: _isPurchasing
            ? Row(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  SizedBox(
                    width: 20.w,
                    height: 20.h,
                    child: CircularProgressIndicator(
                      color: AppTheme.black,
                      strokeWidth: 2,
                    ),
                  ),
                  SizedBox(width: 12.w),
                  Text(
                    'Processing...',
                    style: TextStyle(
                      fontSize: 16.sp,
                      fontWeight: FontWeight.bold,
                      color: AppTheme.black,
                    ),
                  ),
                ],
              )
            : Row(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  Icon(
                    Icons.diamond_rounded,
                    size: 20.sp,
                    color: AppTheme.black,
                  ),
                  SizedBox(width: 8.w),
                  Text(
                    'Start Premium',
                    style: TextStyle(
                      fontSize: 16.sp,
                      fontWeight: FontWeight.bold,
                      color: AppTheme.black,
                    ),
                  ),
                ],
              ),
      ),
    );
  }

  Widget _buildRestoreButton() {
    return TextButton(
      onPressed: _handleRestore,
      child: Text(
        'Restore Previous Purchases',
        style: TextStyle(
          fontSize: 14.sp,
          color: AppTheme.textSecondary,
          fontWeight: FontWeight.w500,
        ),
      ),
    );
  }

  Widget _buildTermsAndPrivacy() {
    return Column(
      children: [
        Text(
          'By purchasing, you agree to our Terms of Service and Privacy Policy',
          style: TextStyle(
            fontSize: 12.sp,
            color: AppTheme.textTertiary,
            height: 1.3,
          ),
          textAlign: TextAlign.center,
        ),
        
        SizedBox(height: 8.h),
        
        Row(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            TextButton(
              onPressed: () {
                // TODO: Open Terms of Service
              },
              child: Text(
                'Terms',
                style: TextStyle(
                  fontSize: 12.sp,
                  color: AppTheme.accentGold,
                  decoration: TextDecoration.underline,
                ),
              ),
            ),
            
            Text(
              ' • ',
              style: TextStyle(
                fontSize: 12.sp,
                color: AppTheme.textTertiary,
              ),
            ),
            
            TextButton(
              onPressed: () {
                // TODO: Open Privacy Policy
              },
              child: Text(
                'Privacy',
                style: TextStyle(
                  fontSize: 12.sp,
                  color: AppTheme.accentGold,
                  decoration: TextDecoration.underline,
                ),
              ),
            ),
          ],
        ),
      ],
    );
  }

  Future<void> _handlePurchase() async {
    setState(() {
      _isPurchasing = true;
    });

    try {
      bool success = false;
      
      if (_selectedPlan == 'monthly') {
        success = await _iapService.purchaseMonthlySubscription();
      } else if (_selectedPlan == 'yearly') {
        success = await _iapService.purchaseYearlySubscription();
      }

      if (success) {
        // Show success message
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Row(
              children: [
                Icon(Icons.check_circle, color: AppTheme.white, size: 20.sp),
                SizedBox(width: 12.w),
                Text(
                  'Purchase initiated successfully!',
                  style: TextStyle(
                    color: AppTheme.white,
                    fontWeight: FontWeight.w500,
                  ),
                ),
              ],
            ),
            backgroundColor: Colors.green,
            behavior: SnackBarBehavior.floating,
            shape: RoundedRectangleBorder(
              borderRadius: BorderRadius.circular(12.r),
            ),
            margin: EdgeInsets.all(16.w),
          ),
        );
      }

    } catch (e) {
      // Show error message
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Purchase failed: ${e.toString()}'),
          backgroundColor: Colors.red,
          behavior: SnackBarBehavior.floating,
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(12.r),
          ),
          margin: EdgeInsets.all(16.w),
        ),
      );
    } finally {
      setState(() {
        _isPurchasing = false;
      });
    }
  }

  Future<void> _handleRestore() async {
    // TODO: Implement restore purchases
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text('Restore purchases coming soon'),
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