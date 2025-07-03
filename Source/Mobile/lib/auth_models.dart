import 'package:flutter/material.dart';

// Extend your existing AuthProvider enum
enum AuthProvider {
  email,
  phone,  // Added phone support
  google,
  apple,
}

extension AuthProviderExtension on AuthProvider {
  String get displayName {
    switch (this) {
      case AuthProvider.email:
        return 'Email';
      case AuthProvider.phone:
        return 'Phone';
      case AuthProvider.google:
        return 'Google';
      case AuthProvider.apple:
        return 'Apple';
    }
  }

  IconData get icon {
    switch (this) {
      case AuthProvider.email:
        return Icons.email;
      case AuthProvider.phone:
        return Icons.phone;
      case AuthProvider.google:
        return Icons.g_mobiledata;
      case AuthProvider.apple:
        return Icons.apple;
    }
  }

  Color get color {
    switch (this) {
      case AuthProvider.email:
        return Colors.blue;
      case AuthProvider.phone:
        return Colors.green;
      case AuthProvider.google:
        return Colors.red;
      case AuthProvider.apple:
        return Colors.black;
    }
  }
}

// Extended AuthUser to support phone numbers
class AuthUser {
  final String id;
  final String? email;  // Made nullable for phone-only users
  final String? phoneNumber;  // Added phone support
  final String? displayName;
  final String? photoUrl;
  final AuthProvider provider;
  final bool isEmailVerified;
  final bool isPhoneVerified;  // Added phone verification
  final DateTime createdAt;
  final DateTime? lastLoginAt;

  const AuthUser({
    required this.id,
    this.email,
    this.phoneNumber,
    this.displayName,
    this.photoUrl,
    required this.provider,
    required this.isEmailVerified,
    this.isPhoneVerified = false,
    required this.createdAt,
    this.lastLoginAt,
  });

  AuthUser copyWith({
    String? id,
    String? email,
    String? phoneNumber,
    String? displayName,
    String? photoUrl,
    AuthProvider? provider,
    bool? isEmailVerified,
    bool? isPhoneVerified,
    DateTime? createdAt,
    DateTime? lastLoginAt,
  }) {
    return AuthUser(
      id: id ?? this.id,
      email: email ?? this.email,
      phoneNumber: phoneNumber ?? this.phoneNumber,
      displayName: displayName ?? this.displayName,
      photoUrl: photoUrl ?? this.photoUrl,
      provider: provider ?? this.provider,
      isEmailVerified: isEmailVerified ?? this.isEmailVerified,
      isPhoneVerified: isPhoneVerified ?? this.isPhoneVerified,
      createdAt: createdAt ?? this.createdAt,
      lastLoginAt: lastLoginAt ?? this.lastLoginAt,
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'email': email,
      'phoneNumber': phoneNumber,
      'displayName': displayName,
      'photoUrl': photoUrl,
      'provider': provider.name,
      'isEmailVerified': isEmailVerified,
      'isPhoneVerified': isPhoneVerified,
      'createdAt': createdAt.toIso8601String(),
      'lastLoginAt': lastLoginAt?.toIso8601String(),
    };
  }

  factory AuthUser.fromJson(Map<String, dynamic> json) {
    return AuthUser(
      id: json['id'],
      email: json['email'],
      phoneNumber: json['phoneNumber'],
      displayName: json['displayName'],
      photoUrl: json['photoUrl'],
      provider: AuthProvider.values.firstWhere(
        (p) => p.name == json['provider'],
        orElse: () => AuthProvider.email,
      ),
      isEmailVerified: json['isEmailVerified'] ?? false,
      isPhoneVerified: json['isPhoneVerified'] ?? false,
      createdAt: DateTime.parse(json['createdAt']),
      lastLoginAt: json['lastLoginAt'] != null 
          ? DateTime.parse(json['lastLoginAt'])
          : null,
    );
  }
}

// Extended LoginRequest to support phone auth
class LoginRequest {
  final String? email;
  final String? password;
  final String? phoneNumber;
  final String? countryCode;
  final AuthProvider provider;
  final bool rememberMe;

  const LoginRequest({
    this.email,
    this.password,
    this.phoneNumber,
    this.countryCode,
    this.provider = AuthProvider.email,
    this.rememberMe = false,
  });

  // Email login constructor
  const LoginRequest.email({
    required String email,
    required String password,
    bool rememberMe = false,
  }) : this(
    email: email,
    password: password,
    provider: AuthProvider.email,
    rememberMe: rememberMe,
  );

  // Phone login constructor
  const LoginRequest.phone({
    required String phoneNumber,
    required String countryCode,
  }) : this(
    phoneNumber: phoneNumber,
    countryCode: countryCode,
    provider: AuthProvider.phone,
  );

  Map<String, dynamic> toJson() {
    return {
      'email': email,
      'password': password,
      'phoneNumber': phoneNumber,
      'countryCode': countryCode,
      'provider': provider.name,
      'rememberMe': rememberMe,
    };
  }
}

// Extended SignUpRequest to support phone auth
class SignUpRequest {
  final String? email;
  final String? password;
  final String? phoneNumber;
  final String? countryCode;
  final String? displayName;
  final AuthProvider provider;
  final bool agreeToTerms;
  final bool subscribeToNewsletter;

  const SignUpRequest({
    this.email,
    this.password,
    this.phoneNumber,
    this.countryCode,
    this.displayName,
    this.provider = AuthProvider.email,
    required this.agreeToTerms,
    this.subscribeToNewsletter = false,
  });

  // Email signup constructor
  const SignUpRequest.email({
    required String email,
    required String password,
    String? displayName,
    required bool agreeToTerms,
    bool subscribeToNewsletter = false,
  }) : this(
    email: email,
    password: password,
    displayName: displayName,
    provider: AuthProvider.email,
    agreeToTerms: agreeToTerms,
    subscribeToNewsletter: subscribeToNewsletter,
  );

  // Phone signup constructor
  const SignUpRequest.phone({
    required String phoneNumber,
    required String countryCode,
    String? displayName,
    required bool agreeToTerms,
    bool subscribeToNewsletter = false,
  }) : this(
    phoneNumber: phoneNumber,
    countryCode: countryCode,
    displayName: displayName,
    provider: AuthProvider.phone,
    agreeToTerms: agreeToTerms,
    subscribeToNewsletter: subscribeToNewsletter,
  );

  Map<String, dynamic> toJson() {
    return {
      'email': email,
      'password': password,
      'phoneNumber': phoneNumber,
      'countryCode': countryCode,
      'displayName': displayName,
      'provider': provider.name,
      'agreeToTerms': agreeToTerms,
      'subscribeToNewsletter': subscribeToNewsletter,
    };
  }
}

// Phone-specific models
class PhoneAuthRequest {
  final String phoneNumber;
  final String countryCode;

  const PhoneAuthRequest({
    required this.phoneNumber,
    required this.countryCode,
  });

  String get fullPhoneNumber => '$countryCode$phoneNumber';

  Map<String, dynamic> toJson() {
    return {
      'phoneNumber': phoneNumber,
      'countryCode': countryCode,
      'fullPhoneNumber': fullPhoneNumber,
    };
  }
}

class OtpVerificationRequest {
  final String phoneNumber;
  final String countryCode;
  final String otp;
  final String verificationId;

  const OtpVerificationRequest({
    required this.phoneNumber,
    required this.countryCode,
    required this.otp,
    required this.verificationId,
  });

  Map<String, dynamic> toJson() {
    return {
      'phoneNumber': phoneNumber,
      'countryCode': countryCode,
      'otp': otp,
      'verificationId': verificationId,
    };
  }
}

// Country code model
class CountryCode {
  final String code;
  final String name;
  final String flag;
  final String iso;

  const CountryCode({
    required this.code,
    required this.name,
    required this.flag,
    required this.iso,
  });

  static List<CountryCode> getPopularCountries() {
    return [
      CountryCode(code: '+1', name: 'United States', flag: '🇺🇸', iso: 'US'),
      CountryCode(code: '+44', name: 'United Kingdom', flag: '🇬🇧', iso: 'GB'),
      CountryCode(code: '+91', name: 'India', flag: '🇮🇳', iso: 'IN'),
      CountryCode(code: '+86', name: 'China', flag: '🇨🇳', iso: 'CN'),
      CountryCode(code: '+81', name: 'Japan', flag: '🇯🇵', iso: 'JP'),
      CountryCode(code: '+49', name: 'Germany', flag: '🇩🇪', iso: 'DE'),
      CountryCode(code: '+33', name: 'France', flag: '🇫🇷', iso: 'FR'),
      CountryCode(code: '+7', name: 'Russia', flag: '🇷🇺', iso: 'RU'),
      CountryCode(code: '+61', name: 'Australia', flag: '🇦🇺', iso: 'AU'),
      CountryCode(code: '+55', name: 'Brazil', flag: '🇧🇷', iso: 'BR'),
      CountryCode(code: '+52', name: 'Mexico', flag: '🇲🇽', iso: 'MX'),
      CountryCode(code: '+39', name: 'Italy', flag: '🇮🇹', iso: 'IT'),
      CountryCode(code: '+34', name: 'Spain', flag: '🇪🇸', iso: 'ES'),
      CountryCode(code: '+82', name: 'South Korea', flag: '🇰🇷', iso: 'KR'),
      CountryCode(code: '+27', name: 'South Africa', flag: '🇿🇦', iso: 'ZA'),
    ];
  }
}

// Extended validation helpers
class AuthValidation {
  static String? validateEmail(String? email) {
    if (email == null || email.isEmpty) {
      return 'Email is required';
    }
    
    final emailRegex = RegExp(r'^[^@]+@[^@]+\.[^@]+$');
    if (!emailRegex.hasMatch(email)) {
      return 'Please enter a valid email';
    }
    
    return null;
  }

  static String? validatePassword(String? password) {
    if (password == null || password.isEmpty) {
      return 'Password is required';
    }
    
    if (password.length < 8) {
      return 'Password must be at least 8 characters';
    }
    
    if (!password.contains(RegExp(r'[A-Z]'))) {
      return 'Password must contain at least one uppercase letter';
    }
    
    if (!password.contains(RegExp(r'[a-z]'))) {
      return 'Password must contain at least one lowercase letter';
    }
    
    if (!password.contains(RegExp(r'[0-9]'))) {
      return 'Password must contain at least one number';
    }
    
    return null;
  }

  static String? validateDisplayName(String? name) {
    if (name == null || name.isEmpty) {
      return 'Name is required';
    }
    
    if (name.length < 2) {
      return 'Name must be at least 2 characters';
    }
    
    if (name.length > 50) {
      return 'Name must be less than 50 characters';
    }
    
    return null;
  }

  static String? validateConfirmPassword(String? password, String? confirmPassword) {
    if (confirmPassword == null || confirmPassword.isEmpty) {
      return 'Please confirm your password';
    }
    
    if (password != confirmPassword) {
      return 'Passwords do not match';
    }
    
    return null;
  }

  // Phone validation methods
  static String? validatePhoneNumber(String? value, {int minLength = 6, int maxLength = 15}) {
    if (value == null || value.isEmpty) {
      return 'Phone number is required';
    }
    
    // Remove all non-digit characters
    final digitsOnly = value.replaceAll(RegExp(r'\D'), '');
    
    if (digitsOnly.length < minLength) {
      return 'Phone number is too short';
    }
    
    if (digitsOnly.length > maxLength) {
      return 'Phone number is too long';
    }
    
    return null;
  }

  static String? validateOtp(String? value, {int expectedLength = 6}) {
    if (value == null || value.isEmpty) {
      return 'Verification code is required';
    }
    
    if (value.length != expectedLength) {
      return 'Please enter the $expectedLength-digit code';
    }
    
    if (!RegExp(r'^\d+$').hasMatch(value)) {
      return 'Verification code should contain only numbers';
    }
    
    return null;
  }
}

// Your existing PasswordStrength class remains the same
class PasswordStrength {
  final double strength; // 0.0 to 1.0
  final String label;
  final Color color;

  const PasswordStrength({
    required this.strength,
    required this.label,
    required this.color,
  });

  static PasswordStrength calculate(String password) {
    if (password.isEmpty) {
      return PasswordStrength(
        strength: 0.0,
        label: 'Enter password',
        color: Colors.grey,
      );
    }

    int score = 0;
    
    // Length check
    if (password.length >= 8) score++;
    if (password.length >= 12) score++;
    
    // Character variety checks
    if (password.contains(RegExp(r'[a-z]'))) score++;
    if (password.contains(RegExp(r'[A-Z]'))) score++;
    if (password.contains(RegExp(r'[0-9]'))) score++;
    if (password.contains(RegExp(r'[!@#$%^&*(),.?":{}|<>]'))) score++;

    final strength = (score / 6).clamp(0.0, 1.0);
    
    if (strength < 0.3) {
      return PasswordStrength(
        strength: strength,
        label: 'Weak',
        color: Colors.red,
      );
    } else if (strength < 0.6) {
      return PasswordStrength(
        strength: strength,
        label: 'Fair',
        color: Colors.orange,
      );
    } else if (strength < 0.8) {
      return PasswordStrength(
        strength: strength,
        label: 'Good',
        color: Colors.yellow,
      );
    } else {
      return PasswordStrength(
        strength: strength,
        label: 'Strong',
        color: Colors.green,
      );
    }
  }
}

// Your existing AuthState class remains the same
class AuthState {
  final AuthUser? user;
  final bool isLoading;
  final String? error;
  final bool isAuthenticated;

  const AuthState({
    this.user,
    this.isLoading = false,
    this.error,
    this.isAuthenticated = false,
  });

  AuthState copyWith({
    AuthUser? user,
    bool? isLoading,
    String? error,
    bool? isAuthenticated,
  }) {
    return AuthState(
      user: user ?? this.user,
      isLoading: isLoading ?? this.isLoading,
      error: error,
      isAuthenticated: isAuthenticated ?? this.isAuthenticated,
    );
  }
}