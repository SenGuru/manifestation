// Example usage of the authentication system
// This file shows how to integrate the auth service with your app

import 'package:flutter/material.dart';
import 'package:manifestation_app/auth_service.dart';
import 'package:manifestation_app/auth_models.dart';
import 'package:manifestation_app/api_config.dart';

class AuthExample extends StatefulWidget {
  @override
  _AuthExampleState createState() => _AuthExampleState();
}

class _AuthExampleState extends State<AuthExample> {
  final AuthService _authService = AuthService();
  final TextEditingController _emailController = TextEditingController();
  final TextEditingController _passwordController = TextEditingController();
  
  bool _isLoading = false;
  String _message = '';

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: Text('Auth Example')),
      body: Padding(
        padding: EdgeInsets.all(16.0),
        child: Column(
          children: [
            TextField(
              controller: _emailController,
              decoration: InputDecoration(
                labelText: 'Email',
                border: OutlineInputBorder(),
              ),
              keyboardType: TextInputType.emailAddress,
            ),
            SizedBox(height: 16),
            TextField(
              controller: _passwordController,
              decoration: InputDecoration(
                labelText: 'Password',
                border: OutlineInputBorder(),
              ),
              obscureText: true,
            ),
            SizedBox(height: 24),
            
            if (_isLoading)
              CircularProgressIndicator()
            else
              Column(
                children: [
                  ElevatedButton(
                    onPressed: _handleLogin,
                    child: Text('Login'),
                  ),
                  SizedBox(height: 8),
                  ElevatedButton(
                    onPressed: _handleSignUp,
                    child: Text('Sign Up'),
                  ),
                  SizedBox(height: 8),
                  ElevatedButton(
                    onPressed: _handleResetPassword,
                    child: Text('Reset Password'),
                  ),
                ],
              ),
            
            SizedBox(height: 24),
            if (_message.isNotEmpty)
              Container(
                padding: EdgeInsets.all(12),
                decoration: BoxDecoration(
                  color: Colors.blue.shade50,
                  borderRadius: BorderRadius.circular(8),
                ),
                child: Text(_message),
              ),
          ],
        ),
      ),
    );
  }

  void _handleLogin() async {
    setState(() {
      _isLoading = true;
      _message = '';
    });

    try {
      final result = await _authService.login(
        email: _emailController.text.trim(),
        password: _passwordController.text,
        rememberMe: true,
      );

      setState(() {
        _isLoading = false;
        _message = result.isSuccess 
            ? 'Login successful! User: ${result.user?.displayName ?? 'Unknown'}'
            : 'Login failed: ${result.message}';
      });
    } catch (e) {
      setState(() {
        _isLoading = false;
        _message = 'Error: ${e.toString()}';
      });
    }
  }

  void _handleSignUp() async {
    setState(() {
      _isLoading = true;
      _message = '';
    });

    try {
      final result = await _authService.signUp(
        email: _emailController.text.trim(),
        password: _passwordController.text,
        firstName: 'John',
        lastName: 'Doe',
      );

      setState(() {
        _isLoading = false;
        _message = result.isSuccess 
            ? 'Sign up successful! User: ${result.user?.displayName ?? 'Unknown'}'
            : 'Sign up failed: ${result.message}';
      });
    } catch (e) {
      setState(() {
        _isLoading = false;
        _message = 'Error: ${e.toString()}';
      });
    }
  }

  void _handleResetPassword() async {
    setState(() {
      _isLoading = true;
      _message = '';
    });

    try {
      final result = await _authService.resetPassword(
        email: _emailController.text.trim(),
      );

      setState(() {
        _isLoading = false;
        _message = result.isSuccess 
            ? 'Password reset email sent!'
            : 'Reset failed: ${result.message}';
      });
    } catch (e) {
      setState(() {
        _isLoading = false;
        _message = 'Error: ${e.toString()}';
      });
    }
  }

  @override
  void dispose() {
    _emailController.dispose();
    _passwordController.dispose();
    super.dispose();
  }
}

// Example of how to check authentication status
class AuthChecker {
  static Future<bool> checkAuthStatus() async {
    final authService = AuthService();
    
    try {
      return await authService.isAuthenticated();
    } catch (e) {
      print('Auth check failed: $e');
      return false;
    }
  }
  
  static Future<bool> checkServerConnection() async {
    final authService = AuthService();
    
    try {
      return await authService.ping();
    } catch (e) {
      print('Server ping failed: $e');
      return false;
    }
  }
}

// Example of a simple auth state manager
class AuthStateManager {
  static AuthUser? _currentUser;
  static String? _token;
  
  static AuthUser? get currentUser => _currentUser;
  static String? get token => _token;
  static bool get isLoggedIn => _currentUser != null && _token != null;
  
  static void setUser(AuthUser user, String token) {
    _currentUser = user;
    _token = token;
  }
  
  static void logout() {
    _currentUser = null;
    _token = null;
  }
}

// Example of how to handle auth results
class AuthResultHandler {
  static void handleAuthResult(BuildContext context, AuthResult result) {
    if (result.isSuccess) {
      // Save user data
      if (result.user != null && result.token != null) {
        AuthStateManager.setUser(result.user!, result.token!);
      }
      
      // Show success message
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text(result.message ?? 'Success!'),
          backgroundColor: Colors.green,
        ),
      );
      
      // Navigate to home or dashboard
      // Navigator.pushReplacementNamed(context, '/home');
    } else {
      // Show error message
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text(result.message ?? 'An error occurred'),
          backgroundColor: Colors.red,
        ),
      );
    }
  }
}