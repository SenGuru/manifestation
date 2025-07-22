import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:shared_preferences/shared_preferences.dart';

class ApiClient {
  static const String baseUrl = 'http://manifestation-qa-1.senthilguru.net';
  static const Duration timeoutDuration = Duration(seconds: 30);
  
  // Basic auth credentials for API access
  static const String _basicAuthUser = 'SpecialUser';
  static const String _basicAuthPassword = 'SpecialPassword1';
  
  // Auth token storage
  static const String _authTokenKey = 'auth_token';
  static String? _authToken;
  
  /// Initialize and load stored auth token
  static Future<void> initialize() async {
    try {
      final prefs = await SharedPreferences.getInstance();
      _authToken = prefs.getString(_authTokenKey);
      if (_authToken != null) {
        print('Loaded stored auth token from SharedPreferences');
      }
    } catch (e) {
      print('Error loading auth token from SharedPreferences: $e');
    }
  }
  
  /// Set authentication token and persist it
  static Future<void> setAuthToken(String token) async {
    try {
      _authToken = token;
      final prefs = await SharedPreferences.getInstance();
      await prefs.setString(_authTokenKey, token);
      print('Auth token saved to SharedPreferences');
    } catch (e) {
      print('Error saving auth token to SharedPreferences: $e');
      _authToken = token; // Still set in memory even if persistence fails
    }
  }
  
  /// Clear authentication token from memory and storage
  static Future<void> clearAuthToken() async {
    try {
      _authToken = null;
      final prefs = await SharedPreferences.getInstance();
      await prefs.remove(_authTokenKey);
      print('Auth token cleared from SharedPreferences');
    } catch (e) {
      print('Error clearing auth token from SharedPreferences: $e');
      _authToken = null; // Still clear from memory even if persistence fails
    }
  }
  
  /// Get default headers
  static Map<String, String> get _defaultHeaders {
    final headers = {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
    };
    
    // Add basic auth for API access
    final basicAuth = 'Basic ${base64Encode(utf8.encode('$_basicAuthUser:$_basicAuthPassword'))}';
    
    // If user is authenticated, use bearer token, otherwise use basic auth
    if (_authToken != null) {
      headers['Authorization'] = 'Bearer $_authToken';
    } else {
      headers['Authorization'] = basicAuth;
    }
    
    return headers;
  }
  
  /// Generic GET request
  static Future<Map<String, dynamic>> get(String endpoint) async {
    try {
      final uri = Uri.parse('$baseUrl$endpoint');
      
      print('=== API CLIENT GET DEBUG ===');
      print('URL: $uri');
      print('Headers: $_defaultHeaders');
      
      final response = await http.get(
        uri,
        headers: _defaultHeaders,
      ).timeout(timeoutDuration);
      
      print('=== HTTP GET RESPONSE DEBUG ===');
      print('Status Code: ${response.statusCode}');
      print('Response Headers: ${response.headers}');
      print('Response Body: ${response.body}');
      print('Response Body Length: ${response.body.length}');
      
      return _handleResponse(response);
    } catch (e) {
      print('=== API CLIENT GET ERROR ===');
      print('Error: $e');
      print('Error Type: ${e.runtimeType}');
      
      return _handleError(e);
    }
  }
  
  /// Generic POST request
  static Future<Map<String, dynamic>> post(String endpoint, {Map<String, dynamic>? body}) async {
    try {
      final uri = Uri.parse('$baseUrl$endpoint');
      
      print('=== API CLIENT POST DEBUG ===');
      print('URL: $uri');
      print('Headers: $_defaultHeaders');
      print('Body: ${body != null ? jsonEncode(body) : 'null'}');
      
      final response = await http.post(
        uri,
        headers: _defaultHeaders,
        body: body != null ? jsonEncode(body) : null,
      ).timeout(timeoutDuration);
      
      print('=== HTTP RESPONSE DEBUG ===');
      print('Status Code: ${response.statusCode}');
      print('Response Headers: ${response.headers}');
      print('Response Body: ${response.body}');
      print('Response Body Length: ${response.body.length}');
      
      return _handleResponse(response);
    } catch (e) {
      print('=== API CLIENT POST ERROR ===');
      print('Error: $e');
      print('Error Type: ${e.runtimeType}');
      
      return _handleError(e);
    }
  }
  
  /// Generic PUT request
  static Future<Map<String, dynamic>> put(String endpoint, {Map<String, dynamic>? body}) async {
    try {
      final uri = Uri.parse('$baseUrl$endpoint');
      final response = await http.put(
        uri,
        headers: _defaultHeaders,
        body: body != null ? jsonEncode(body) : null,
      ).timeout(timeoutDuration);
      
      return _handleResponse(response);
    } catch (e) {
      return _handleError(e);
    }
  }
  
  /// Generic PATCH request
  static Future<Map<String, dynamic>> patch(String endpoint, {Map<String, dynamic>? body}) async {
    try {
      final uri = Uri.parse('$baseUrl$endpoint');
      final response = await http.patch(
        uri,
        headers: _defaultHeaders,
        body: body != null ? jsonEncode(body) : null,
      ).timeout(timeoutDuration);
      
      return _handleResponse(response);
    } catch (e) {
      return _handleError(e);
    }
  }
  
  /// Generic DELETE request
  static Future<Map<String, dynamic>> delete(String endpoint) async {
    try {
      final uri = Uri.parse('$baseUrl$endpoint');
      final response = await http.delete(
        uri,
        headers: _defaultHeaders,
      ).timeout(timeoutDuration);
      
      return _handleResponse(response);
    } catch (e) {
      return _handleError(e);
    }
  }
  
  /// Handle HTTP response
  static Map<String, dynamic> _handleResponse(http.Response response) {
    print('=== RESPONSE HANDLER DEBUG ===');
    print('Status Code: ${response.statusCode}');
    print('Raw Body: ${response.body}');
    
    Map<String, dynamic> responseData;
    
    try {
      responseData = jsonDecode(response.body);
      print('Parsed JSON: $responseData');
    } catch (e) {
      print('JSON Parse Error: $e');
      // If response body is not valid JSON
      responseData = {'message': response.body};
      print('Using fallback responseData: $responseData');
    }
    
    if (response.statusCode >= 200 && response.statusCode < 300) {
      print('=== SUCCESS RESPONSE PATH ===');
      final result = {
        'success': true,
        'statusCode': response.statusCode,
        'data': responseData,
        'message': responseData['message'] ?? 'Request successful',
      };
      print('Final success result: $result');
      return result;
    } else {
      print('=== ERROR RESPONSE PATH ===');
      final result = {
        'success': false,
        'statusCode': response.statusCode,
        'error': responseData['error'] ?? responseData['message'] ?? 'Request failed',
        'data': responseData,
      };
      print('Final error result: $result');
      return result;
    }
  }
  
  /// Handle errors (network, timeout, etc.)
  static Map<String, dynamic> _handleError(dynamic error) {
    String errorMessage;
    
    if (error.toString().contains('SocketException') || 
        error.toString().contains('ECONNREFUSED')) {
      errorMessage = 'No internet connection or server unavailable';
    } else if (error.toString().contains('TimeoutException')) {
      errorMessage = 'Request timeout - please try again';
    } else if (error.toString().contains('FormatException')) {
      errorMessage = 'Invalid response format from server';
    } else {
      errorMessage = 'Network error: ${error.toString()}';
    }
    
    print('API Error: $errorMessage');
    
    return {
      'success': false,
      'error': errorMessage,
      'statusCode': null,
      'data': null,
    };
  }
  
  /// Upload file (for images, etc.)
  static Future<Map<String, dynamic>> uploadFile(
    String endpoint, 
    String filePath, 
    String fieldName,
    {Map<String, String>? additionalFields}
  ) async {
    try {
      final uri = Uri.parse('$baseUrl$endpoint');
      final request = http.MultipartRequest('POST', uri);
      
      // Add headers (but remove Content-Type to let http package set it for multipart)
      final headers = Map<String, String>.from(_defaultHeaders);
      headers.remove('Content-Type'); // Remove so multipart boundary is set correctly
      request.headers.addAll(headers);
      
      // Add file if filePath is provided and not empty
      if (filePath.isNotEmpty) {
        final file = await http.MultipartFile.fromPath(fieldName, filePath);
        request.files.add(file);
      }
      
      // Add additional fields if provided
      if (additionalFields != null) {
        request.fields.addAll(additionalFields);
      }
      
      final streamedResponse = await request.send().timeout(timeoutDuration);
      final response = await http.Response.fromStream(streamedResponse);
      
      return _handleResponse(response);
    } catch (e) {
      return _handleError(e);
    }
  }

  /// Send form data (multipart/form-data without file)
  static Future<Map<String, dynamic>> postFormData(
    String endpoint,
    Map<String, String> fields,
  ) async {
    try {
      final uri = Uri.parse('$baseUrl$endpoint');
      final request = http.MultipartRequest('POST', uri);
      
      // Add headers (but remove Content-Type to let http package set it for multipart)
      final headers = Map<String, String>.from(_defaultHeaders);
      headers.remove('Content-Type'); // Remove so multipart boundary is set correctly
      request.headers.addAll(headers);
      
      // Add fields
      request.fields.addAll(fields);
      
      final streamedResponse = await request.send().timeout(timeoutDuration);
      final response = await http.Response.fromStream(streamedResponse);
      
      return _handleResponse(response);
    } catch (e) {
      return _handleError(e);
    }
  }
  
  /// Check if user is authenticated
  static bool get isAuthenticated => _authToken != null;
  
  /// Get current auth token
  static String? get authToken => _authToken;
}