import 'package:flutter/material.dart';
import 'package:manifestation_app/session_service.dart';
import 'package:manifestation_app/auth_service.dart';
import 'package:manifestation_app/login_screen.dart';
import 'package:manifestation_app/home_screen.dart';

class AuthWrapper extends StatefulWidget {
  @override
  _AuthWrapperState createState() => _AuthWrapperState();
}

class _AuthWrapperState extends State<AuthWrapper> {
  bool _isLoading = true;
  bool _isAuthenticated = false;
  final _authService = AuthService();

  @override
  void initState() {
    super.initState();
    _checkAuthenticationStatus();
  }

  Future<void> _checkAuthenticationStatus() async {
    try {
      // Check if user has a valid session
      final hasValidSession = await SessionService.instance.isLoggedIn();
      
      if (hasValidSession) {
        // Verify with server that session is still valid
        final isServerAuthenticated = await _authService.isAuthenticated();
        
        if (isServerAuthenticated) {
          setState(() {
            _isAuthenticated = true;
            _isLoading = false;
          });
          return;
        } else {
          // Session expired, clear it
          await SessionService.instance.clearSession();
        }
      }
      
      setState(() {
        _isAuthenticated = false;
        _isLoading = false;
      });
    } catch (e) {
      print('Error checking authentication: $e');
      setState(() {
        _isAuthenticated = false;
        _isLoading = false;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    if (_isLoading) {
      return Scaffold(
        body: Center(
          child: CircularProgressIndicator(),
        ),
      );
    }
    
    if (_isAuthenticated) {
      return HomeScreen();
    } else {
      return LoginScreen();
    }
  }
}