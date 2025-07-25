import 'dart:convert';
import 'dart:io';
import 'dart:typed_data';
import 'package:crypto/crypto.dart';
import 'package:http/http.dart' as http;
import 'package:shared_preferences/shared_preferences.dart';
import 'unified_auth_service.dart';

/// Service to detect changes on the server without modifying the API
/// Uses HTTP headers, content hashing, and response size comparison
class ServerChangeDetector {
  static final ServerChangeDetector _instance = ServerChangeDetector._internal();
  factory ServerChangeDetector() => _instance;
  ServerChangeDetector._internal();

  final UnifiedAuthService _authService = UnifiedAuthService();
  
  // Store last known state for each endpoint
  final Map<String, String> _lastEtags = {};
  final Map<String, DateTime> _lastModified = {};
  final Map<String, String> _lastContentHashes = {};
  final Map<String, int> _lastResponseSizes = {};
  final Map<String, int> _lastItemCounts = {};
  
  SharedPreferences? _prefs;
  
  /// Initialize the detector
  Future<void> initialize() async {
    _prefs ??= await SharedPreferences.getInstance();
    await _loadStoredState();
  }
  
  /// Primary method to check if server data has changed
  Future<bool> hasServerDataChanged(String endpoint, String cacheKey) async {
    try {
      await initialize();
      
      // First try lightweight HEAD request for headers
      bool headerChanged = await _checkHeadersForChange(endpoint, cacheKey);
      if (headerChanged) {
        print('📡 Header change detected for $cacheKey');
        return true;
      }
      
      // If headers don't indicate change, do content comparison
      bool contentChanged = await _checkContentForChange(endpoint, cacheKey);
      if (contentChanged) {
        print('📊 Content change detected for $cacheKey');
        return true;
      }
      
      print('✅ No changes detected for $cacheKey');
      return false;
      
    } catch (e) {
      print('⚠️ Error checking server changes for $cacheKey: $e');
      // On error, assume there might be changes to be safe
      return true;
    }
  }
  
  /// Check HTTP headers (ETag, Last-Modified) for changes
  Future<bool> _checkHeadersForChange(String endpoint, String cacheKey) async {
    try {
      final headers = await _getAuthHeaders();
      
      // Make HEAD request (faster than GET, only returns headers)
      final response = await http.head(
        Uri.parse(endpoint),
        headers: headers,
      ).timeout(Duration(seconds: 10));
      
      bool changed = false;
      
      // Check ETag header
      final currentEtag = response.headers['etag'] ?? response.headers['ETag'];
      final lastEtag = _lastEtags[cacheKey];
      
      if (currentEtag != null && currentEtag != lastEtag) {
        _lastEtags[cacheKey] = currentEtag;
        changed = true;
      }
      
      // Check Last-Modified header
      final lastModHeader = response.headers['last-modified'] ?? 
                           response.headers['Last-Modified'];
      if (lastModHeader != null) {
        try {
          final serverModified = HttpDate.parse(lastModHeader);
          final localModified = _lastModified[cacheKey];
          
          if (localModified == null || serverModified.isAfter(localModified)) {
            _lastModified[cacheKey] = serverModified;
            changed = true;
          }
        } catch (e) {
          print('Failed to parse Last-Modified header: $e');
        }
      }
      
      if (changed) {
        await _saveStoredState();
      }
      
      return changed;
      
    } catch (e) {
      print('Header check failed: $e');
      return false; // Don't assume change on header check failure
    }
  }
  
  /// Check response content for changes using hashing and size comparison
  Future<bool> _checkContentForChange(String endpoint, String cacheKey) async {
    try {
      final headers = await _getAuthHeaders();
      
      // Make GET request
      final response = await http.get(
        Uri.parse(endpoint),
        headers: headers,
      ).timeout(Duration(seconds: 30));
      
      if (response.statusCode != 200) {
        print('Non-200 response: ${response.statusCode}');
        return false;
      }
      
      bool changed = false;
      
      // Check response size
      final currentSize = response.body.length;
      final lastSize = _lastResponseSizes[cacheKey];
      
      if (lastSize != null && currentSize != lastSize) {
        _lastResponseSizes[cacheKey] = currentSize;
        changed = true;
      } else if (lastSize == null) {
        _lastResponseSizes[cacheKey] = currentSize;
      }
      
      // Check content hash for more precise change detection
      final currentHash = _computeContentHash(response.body);
      final lastHash = _lastContentHashes[cacheKey];
      
      if (lastHash != null && currentHash != lastHash) {
        _lastContentHashes[cacheKey] = currentHash;
        changed = true;
      } else if (lastHash == null) {
        _lastContentHashes[cacheKey] = currentHash;
      }
      
      // Check item count if response is a list
      final currentCount = _extractItemCount(response.body);
      if (currentCount != null) {
        final lastCount = _lastItemCounts[cacheKey];
        
        if (lastCount != null && currentCount != lastCount) {
          _lastItemCounts[cacheKey] = currentCount;
          changed = true;
        } else if (lastCount == null) {
          _lastItemCounts[cacheKey] = currentCount;
        }
      }
      
      if (changed) {
        await _saveStoredState();
      }
      
      return changed;
      
    } catch (e) {
      print('Content check failed: $e');
      return false; // Don't assume change on content check failure
    }
  }
  
  /// Get authentication headers for API requests
  Future<Map<String, String>> _getAuthHeaders() async {
    final headers = <String, String>{
      'Content-Type': 'application/json',
    };
    
    final authToken = _authService.authToken;
    if (authToken != null && authToken.isNotEmpty) {
      headers['Authorization'] = 'Bearer $authToken';
    }
    
    return headers;
  }
  
  /// Compute SHA-256 hash of content for change detection
  String _computeContentHash(String content) {
    final bytes = utf8.encode(content);
    final digest = sha256.convert(bytes);
    return digest.toString();
  }
  
  /// Extract item count from response (works with common API response formats)
  int? _extractItemCount(String responseBody) {
    try {
      final data = json.decode(responseBody);
      
      // Handle direct list
      if (data is List) {
        return data.length;
      }
      
      // Handle common API wrapper formats
      if (data is Map) {
        // Check for 'model' field (common in your API)
        if (data.containsKey('model')) {
          final model = data['model'];
          if (model is List) {
            return model.length;
          }
        }
        
        // Check for 'data' field
        if (data.containsKey('data')) {
          final dataField = data['data'];
          if (dataField is List) {
            return dataField.length;
          }
        }
        
        // Check for 'items' field
        if (data.containsKey('items')) {
          final items = data['items'];
          if (items is List) {
            return items.length;
          }
        }
      }
      
      return null;
    } catch (e) {
      return null;
    }
  }
  
  /// Load stored state from SharedPreferences
  Future<void> _loadStoredState() async {
    try {
      // Load ETags
      final etagsJson = _prefs!.getString('sync_etags');
      if (etagsJson != null) {
        final etags = Map<String, String>.from(json.decode(etagsJson));
        _lastEtags.addAll(etags);
      }
      
      // Load content hashes
      final hashesJson = _prefs!.getString('sync_hashes');
      if (hashesJson != null) {
        final hashes = Map<String, String>.from(json.decode(hashesJson));
        _lastContentHashes.addAll(hashes);
      }
      
      // Load response sizes
      final sizesJson = _prefs!.getString('sync_sizes');
      if (sizesJson != null) {
        final sizes = Map<String, int>.from(json.decode(sizesJson));
        _lastResponseSizes.addAll(sizes);
      }
      
      // Load item counts
      final countsJson = _prefs!.getString('sync_counts');
      if (countsJson != null) {
        final counts = Map<String, int>.from(json.decode(countsJson));
        _lastItemCounts.addAll(counts);
      }
      
      // Load last modified dates
      final modifiedJson = _prefs!.getString('sync_modified');
      if (modifiedJson != null) {
        final modifiedMap = Map<String, String>.from(json.decode(modifiedJson));
        modifiedMap.forEach((key, value) {
          try {
            _lastModified[key] = DateTime.parse(value);
          } catch (e) {
            print('Failed to parse stored date for $key: $e');
          }
        });
      }
      
      print('📱 Loaded sync state: ${_lastEtags.length} ETags, ${_lastContentHashes.length} hashes');
    } catch (e) {
      print('⚠️ Failed to load stored sync state: $e');
    }
  }
  
  /// Save current state to SharedPreferences
  Future<void> _saveStoredState() async {
    try {
      // Save ETags
      await _prefs!.setString('sync_etags', json.encode(_lastEtags));
      
      // Save content hashes
      await _prefs!.setString('sync_hashes', json.encode(_lastContentHashes));
      
      // Save response sizes
      await _prefs!.setString('sync_sizes', json.encode(_lastResponseSizes));
      
      // Save item counts
      await _prefs!.setString('sync_counts', json.encode(_lastItemCounts));
      
      // Save last modified dates
      final modifiedMap = <String, String>{};
      _lastModified.forEach((key, value) {
        modifiedMap[key] = value.toIso8601String();
      });
      await _prefs!.setString('sync_modified', json.encode(modifiedMap));
      
    } catch (e) {
      print('⚠️ Failed to save sync state: $e');
    }
  }
  
  /// Reset all stored state (useful for debugging or logout)
  Future<void> clearStoredState() async {
    await initialize();
    
    _lastEtags.clear();
    _lastModified.clear();
    _lastContentHashes.clear();
    _lastResponseSizes.clear();
    _lastItemCounts.clear();
    
    await _prefs!.remove('sync_etags');
    await _prefs!.remove('sync_hashes');
    await _prefs!.remove('sync_sizes');
    await _prefs!.remove('sync_counts');
    await _prefs!.remove('sync_modified');
    
    print('🗑️ Cleared all sync state');
  }
  
  /// Get debug information about stored state
  Map<String, dynamic> getDebugInfo() {
    return {
      'etags_count': _lastEtags.length,
      'hashes_count': _lastContentHashes.length,
      'sizes_count': _lastResponseSizes.length,
      'counts_count': _lastItemCounts.length,
      'modified_count': _lastModified.length,
      'last_etags': _lastEtags,
      'last_sizes': _lastResponseSizes,
      'last_counts': _lastItemCounts,
    };
  }
}