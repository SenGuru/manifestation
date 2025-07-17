class ApiConfig {
  // Your API base URL (without the swagger path)
  // Since swagger works without port, try standard HTTP port
  static const String baseUrl = 'http://manifestation-qa-1.senthilguru.net';
  
  // Timeout configurations
  static const Duration connectTimeout = Duration(seconds: 30);
  static const Duration receiveTimeout = Duration(seconds: 30);
  
  // Common headers
  static const Map<String, String> commonHeaders = {
    'Content-Type': 'application/json',
    'Accept': 'application/json',
  };
  
  // Error messages
  static const String networkError = 'Network error. Please check your connection.';
  static const String serverError = 'Server error. Please try again later.';
  static const String timeoutError = 'Request timed out. Please try again.';
  static const String unknownError = 'An unexpected error occurred.';
  
  // Success messages
  static const String loginSuccess = 'Login successful!';
  static const String signUpSuccess = 'Account created successfully!';
  static const String passwordResetSuccess = 'Password reset email sent successfully!';
  
  // Validation messages
  static const String invalidEmail = 'Please enter a valid email address';
  static const String weakPassword = 'Password must be at least 8 characters long';
  static const String passwordMismatch = 'Passwords do not match';
  static const String agreementRequired = 'You must agree to the terms and conditions';
}