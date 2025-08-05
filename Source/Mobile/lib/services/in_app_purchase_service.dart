import 'dart:async';
import 'dart:io';
import 'package:flutter/services.dart';
import 'package:in_app_purchase/in_app_purchase.dart';
import 'package:in_app_purchase_storekit/in_app_purchase_storekit.dart';
import 'package:in_app_purchase_android/in_app_purchase_android.dart';
import 'unified_auth_service.dart';
import 'subscription_service.dart';
import '../api/generated/code/manifestation.swagger.dart';

/// Service to handle in-app purchases for subscriptions
class InAppPurchaseService {
  static final InAppPurchaseService _instance = InAppPurchaseService._internal();
  factory InAppPurchaseService() => _instance;
  InAppPurchaseService._internal();

  final InAppPurchase _iap = InAppPurchase.instance;
  StreamSubscription<List<PurchaseDetails>>? _subscription;
  
  // Subscription product IDs (configure these in App Store Connect / Google Play Console)
  static const String monthlySubId = 'manifestation_monthly_premium';
  static const String yearlySubId = 'manifestation_yearly_premium';
  
  static const Set<String> _productIds = {monthlySubId, yearlySubId};
  
  List<ProductDetails> _products = [];
  bool _isAvailable = false;
  bool _purchasePending = false;
  
  // Getters
  List<ProductDetails> get products => _products;
  bool get isAvailable => _isAvailable;
  bool get purchasePending => _purchasePending;
  
  ProductDetails? get monthlyProduct => 
      _products.where((p) => p.id == monthlySubId).firstOrNull;
  
  ProductDetails? get yearlyProduct => 
      _products.where((p) => p.id == yearlySubId).firstOrNull;

  /// Initialize the in-app purchase service
  Future<bool> initialize() async {
    try {
      print('🛒 Initializing In-App Purchase Service...');
      
      // Check if in-app purchases are available
      _isAvailable = await _iap.isAvailable();
      if (!_isAvailable) {
        print('❌ In-app purchases not available on this device');
        return false;
      }
      
      // Query product details
      await _loadProducts();
      
      // Setup purchase listener
      _setupPurchaseListener();
      
      // Restore previous purchases on iOS
      if (Platform.isIOS) {
        await _restorePurchases();
      }
      
      print('✅ In-App Purchase Service initialized successfully');
      return true;
      
    } catch (e) {
      print('❌ Error initializing In-App Purchase Service: $e');
      return false;
    }
  }

  /// Load available subscription products
  Future<void> _loadProducts() async {
    try {
      print('📦 Loading subscription products...');
      
      final ProductDetailsResponse response = await _iap.queryProductDetails(_productIds);
      
      if (response.notFoundIDs.isNotEmpty) {
        print('⚠️ Some products not found: ${response.notFoundIDs}');
      }
      
      _products = response.productDetails;
      
      print('✅ Loaded ${_products.length} products:');
      for (final product in _products) {
        print('  - ${product.id}: ${product.title} - ${product.price}');
      }
      
    } catch (e) {
      print('❌ Error loading products: $e');
    }
  }

  /// Setup purchase stream listener
  void _setupPurchaseListener() {
    print('👂 Setting up purchase listener...');
    
    final purchaseUpdated = _iap.purchaseStream;
    _subscription = purchaseUpdated.listen(
      (List<PurchaseDetails> purchaseDetailsList) {
        _handlePurchaseUpdate(purchaseDetailsList);
      },
      onDone: () {
        print('🔚 Purchase stream closed');
      },
      onError: (error) {
        print('❌ Purchase stream error: $error');
      },
    );
  }

  /// Handle purchase updates
  Future<void> _handlePurchaseUpdate(List<PurchaseDetails> purchaseDetailsList) async {
    for (final PurchaseDetails purchaseDetails in purchaseDetailsList) {
      print('📱 Purchase update: ${purchaseDetails.productID} - ${purchaseDetails.status}');
      
      switch (purchaseDetails.status) {
        case PurchaseStatus.pending:
          _purchasePending = true;
          _showPendingUI();
          break;
          
        case PurchaseStatus.purchased:
        case PurchaseStatus.restored:
          _purchasePending = false;
          await _handleSuccessfulPurchase(purchaseDetails);
          break;
          
        case PurchaseStatus.error:
          _purchasePending = false;
          _handlePurchaseError(purchaseDetails);
          break;
          
        case PurchaseStatus.canceled:
          _purchasePending = false;
          print('🚫 Purchase canceled by user');
          break;
      }

      // Complete the purchase for platform stores
      if (purchaseDetails.pendingCompletePurchase) {
        await _iap.completePurchase(purchaseDetails);
      }
    }
  }

  /// Handle successful purchase
  Future<void> _handleSuccessfulPurchase(PurchaseDetails purchaseDetails) async {
    try {
      print('🎉 Purchase successful: ${purchaseDetails.productID}');
      
      // Send receipt to backend for verification
      final success = await _verifyPurchaseWithBackend(purchaseDetails);
      
      if (success) {
        // Update local subscription status
        final subscriptionService = SubscriptionService();
        
        // Calculate expiry date based on product
        DateTime expiryDate;
        if (purchaseDetails.productID == monthlySubId) {
          expiryDate = DateTime.now().add(Duration(days: 30));
        } else if (purchaseDetails.productID == yearlySubId) {
          expiryDate = DateTime.now().add(Duration(days: 365));
        } else {
          expiryDate = DateTime.now().add(Duration(days: 30)); // Default to monthly
        }
        
        await subscriptionService.activatePremium(expiryDate: expiryDate);
        
        print('✅ Premium subscription activated until: $expiryDate');
      } else {
        print('❌ Backend verification failed');
      }
      
    } catch (e) {
      print('❌ Error handling successful purchase: $e');
    }
  }

  /// Verify purchase with backend
  Future<bool> _verifyPurchaseWithBackend(PurchaseDetails purchaseDetails) async {
    try {
      print('🔐 Verifying purchase with backend...');
      
      final authService = UnifiedAuthService();
      final userId = await authService.getUserId();
      
      if (userId == null) {
        print('❌ User not authenticated');
        return false;
      }

      // Get platform-specific receipt data
      String receiptData;
      String platform;
      
      if (Platform.isIOS) {
        platform = 'ios';
        // For iOS, get the receipt data from StoreKit
        receiptData = purchaseDetails.verificationData.serverVerificationData;
      } else if (Platform.isAndroid) {
        platform = 'android';
        // For Android, get the purchase token
        final androidDetails = purchaseDetails.verificationData;
        receiptData = androidDetails.serverVerificationData;
      } else {
        print('❌ Unsupported platform');
        return false;
      }

      // TODO: Call your ASP.NET backend API to verify the receipt
      // This is where you'd make an HTTP request to your backend
      print('📤 Receipt data: ${receiptData.substring(0, 50)}...');
      print('📱 Platform: $platform');
      print('👤 User ID: $userId');
      print('🏷️ Product ID: ${purchaseDetails.productID}');
      
      // For now, return true (implement actual backend call later)
      await Future.delayed(Duration(seconds: 1)); // Simulate network call
      return true;
      
    } catch (e) {
      print('❌ Error verifying purchase with backend: $e');
      return false;
    }
  }

  /// Handle purchase error
  void _handlePurchaseError(PurchaseDetails purchaseDetails) {
    print('❌ Purchase error: ${purchaseDetails.error?.message}');
    
    // You could show an error dialog here
    // or emit an event for the UI to handle
  }

  /// Show pending purchase UI
  void _showPendingUI() {
    print('⏳ Purchase pending...');
    // You could show a loading indicator here
    // or emit an event for the UI to handle
  }

  /// Start purchase flow for a product
  Future<bool> purchaseProduct(ProductDetails product) async {
    try {
      print('🛒 Starting purchase for: ${product.id}');
      
      if (!_isAvailable) {
        print('❌ In-app purchases not available');
        return false;
      }
      
      _purchasePending = true;
      
      final PurchaseParam purchaseParam = PurchaseParam(
        productDetails: product,
      );
      
      // For subscriptions, use buyNonConsumable
      final success = await _iap.buyNonConsumable(purchaseParam: purchaseParam);
      
      if (!success) {
        _purchasePending = false;
        print('❌ Failed to initiate purchase');
        return false;
      }
      
      return true;
      
    } catch (e) {
      _purchasePending = false;
      print('❌ Error purchasing product: $e');
      return false;
    }
  }

  /// Purchase monthly subscription
  Future<bool> purchaseMonthlySubscription() async {
    final product = monthlyProduct;
    if (product == null) {
      print('❌ Monthly subscription product not available');
      return false;
    }
    
    return await purchaseProduct(product);
  }

  /// Purchase yearly subscription
  Future<bool> purchaseYearlySubscription() async {
    final product = yearlyProduct;
    if (product == null) {
      print('❌ Yearly subscription product not available');
      return false;
    }
    
    return await purchaseProduct(product);
  }

  /// Restore purchases (mainly for iOS)
  Future<void> _restorePurchases() async {
    try {
      print('🔄 Restoring previous purchases...');
      await _iap.restorePurchases();
    } catch (e) {
      print('❌ Error restoring purchases: $e');
    }
  }

  /// Check if user has active subscription
  Future<bool> checkActiveSubscription() async {
    try {
      // TODO: Check with backend for active subscription
      // For now, check local subscription service
      final subscriptionService = SubscriptionService();
      await subscriptionService.initialize();
      
      return subscriptionService.isPremium;
      
    } catch (e) {
      print('❌ Error checking active subscription: $e');
      return false;
    }
  }

  /// Dispose the service
  void dispose() {
    print('🗑️ Disposing In-App Purchase Service');
    _subscription?.cancel();
  }
}

/// Extension to add firstOrNull method
extension IterableExtension<T> on Iterable<T> {
  T? get firstOrNull {
    final iterator = this.iterator;
    if (iterator.moveNext()) {
      return iterator.current;
    }
    return null;
  }
}