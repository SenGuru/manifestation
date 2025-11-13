import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'dart:convert';
import 'dart:typed_data';
import '../constants/app_theme.dart';
import '../services/user_profile_service.dart';
import '../services/user_service.dart';
import '../services/profile_picture_notifier.dart';

class UserProfilePicture extends StatefulWidget {
  final double size;
  final int? userId;
  final String? profileGuid;
  final bool showBorder;
  final Color? borderColor;
  final double borderWidth;
  final VoidCallback? onTap;
  final String? cachedImageUrl; // Add cached image parameter
  final Function(String?)? onImageLoaded; // Callback when image is loaded

  const UserProfilePicture({
    Key? key,
    this.size = 40.0,
    this.userId,
    this.profileGuid,
    this.showBorder = true,
    this.borderColor,
    this.borderWidth = 2.0,
    this.onTap,
    this.cachedImageUrl, // Optional cached image URL
    this.onImageLoaded, // Callback when image is loaded
  }) : super(key: key);

  @override
  _UserProfilePictureState createState() => _UserProfilePictureState();
}

class _UserProfilePictureState extends State<UserProfilePicture> {
  String? _imageUrl;
  bool _isLoading = true;
  bool _hasError = false;
  final UserProfileService _profileService = UserProfileService();
  final ProfilePictureNotifier _profilePictureNotifier = ProfilePictureNotifier();
  int _lastUpdateCounter = 0;

  // Getter to access the current image URL
  String? get currentImageUrl => _imageUrl;
  
  // Method to force refresh the profile picture
  void refreshProfilePicture() {
    _loadProfilePicture();
  }

  // Helper method to determine if the image data is raw bytes
  bool _isRawImageData(String imageData) {
    // Check if it's not a URL and contains non-printable characters (typical of binary data)
    if (imageData.startsWith('https://') || 
        imageData.startsWith('https://') || 
        imageData.startsWith('data:')) {
      return false;
    }
    
    // Check for JPEG header (ÿØ) or PNG header (‰PNG) or other binary patterns
    if (imageData.isNotEmpty) {
      final firstChars = imageData.substring(0, imageData.length > 4 ? 4 : imageData.length);
      // Check for common image file signatures in raw bytes
      return firstChars.codeUnits.any((code) => code > 127 || code < 32);
    }
    
    return false;
  }

  // Helper method to convert raw image bytes to base64 data URL
  String _convertRawBytesToDataUrl(String rawBytes) {
    try {
      // Convert the raw string to bytes
      final bytes = rawBytes.codeUnits;
      
      // Convert to Uint8List
      final uint8List = Uint8List.fromList(bytes);
      
      // Convert to base64
      final base64String = base64Encode(uint8List);
      
      // Determine MIME type based on file signature
      String mimeType = 'image/jpeg'; // default
      if (bytes.length >= 8) {
        // Check PNG signature
        if (bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E && bytes[3] == 0x47) {
          mimeType = 'image/png';
        }
        // Check GIF signature
        else if (bytes[0] == 0x47 && bytes[1] == 0x49 && bytes[2] == 0x46) {
          mimeType = 'image/gif';
        }
        // Check WebP signature
        else if (bytes[8] == 0x57 && bytes[9] == 0x45 && bytes[10] == 0x42 && bytes[11] == 0x50) {
          mimeType = 'image/webp';
        }
      }
      
      return 'data:$mimeType;base64,$base64String';
    } catch (e) {
      debugPrint('❌ Error converting raw bytes to data URL: $e');
      return '';
    }
  }

  @override
  void initState() {
    super.initState();
    // Listen to profile picture updates
    _profilePictureNotifier.addListener(_onProfilePictureUpdated);
    
    // Use cached image if available, otherwise load from API
    if (widget.cachedImageUrl != null && widget.cachedImageUrl!.isNotEmpty) {
      String processedImageUrl = widget.cachedImageUrl!;
      
      // Convert raw image data to data URL if needed
      if (_isRawImageData(processedImageUrl)) {
        processedImageUrl = _convertRawBytesToDataUrl(processedImageUrl);
      }
      
      setState(() {
        _imageUrl = processedImageUrl;
        _isLoading = false;
        _hasError = false;
      });
      // Notify parent of cached image after build completes
      if (widget.onImageLoaded != null) {
        WidgetsBinding.instance.addPostFrameCallback((_) {
          widget.onImageLoaded!(_imageUrl);
        });
      }
    } else {
      _loadProfilePicture();
    }
  }

  @override
  void dispose() {
    _profilePictureNotifier.removeListener(_onProfilePictureUpdated);
    super.dispose();
  }

  void _onProfilePictureUpdated() {
    // Check if this is a new update
    if (_profilePictureNotifier.updateCounter != _lastUpdateCounter) {
      _lastUpdateCounter = _profilePictureNotifier.updateCounter;
      // Force reload the profile picture from the server
      _loadProfilePicture();
    }
  }

  @override
  void didUpdateWidget(UserProfilePicture oldWidget) {
    super.didUpdateWidget(oldWidget);
    // Check if cachedImageUrl changed
    if (widget.cachedImageUrl != oldWidget.cachedImageUrl) {
      if (widget.cachedImageUrl != null && widget.cachedImageUrl!.isNotEmpty) {
        String processedImageUrl = widget.cachedImageUrl!;
        
        // Convert raw image data to data URL if needed
        if (_isRawImageData(processedImageUrl)) {
          processedImageUrl = _convertRawBytesToDataUrl(processedImageUrl);
        }
        
        setState(() {
          _imageUrl = processedImageUrl;
          _isLoading = false;
          _hasError = false;
        });
        return;
      }
    }
    // Reload if userId or profileGuid changed
    if (widget.userId != oldWidget.userId || 
        widget.profileGuid != oldWidget.profileGuid) {
      _loadProfilePicture();
    }
  }

  Future<void> _loadProfilePicture() async {
    setState(() {
      _isLoading = true;
      _hasError = false;
    });

    try {
      // Initialize the service
      await _profileService.initialize();

      // Get userId if not provided
      int? userId = widget.userId;
      if (userId == null) {
        userId = await UserService.getUserId();
      }

      // Call the API
      final result = await _profileService.getUserPicture(
        userId: userId,
        profileGuid: widget.profileGuid,
      );

      if (mounted) {
        setState(() {
          _isLoading = false;
          if (result.isSuccess) {
            String processedImageUrl = result.data ?? '';
            
            // Convert raw image data to data URL if needed
            if (processedImageUrl.isNotEmpty && _isRawImageData(processedImageUrl)) {
              debugPrint('🔄 Converting raw image data to data URL...');
              processedImageUrl = _convertRawBytesToDataUrl(processedImageUrl);
              if (processedImageUrl.isNotEmpty) {
                debugPrint('✅ Successfully converted raw image data to data URL');
              } else {
                debugPrint('❌ Failed to convert raw image data to data URL');
                processedImageUrl = '';
              }
            }
            
            _imageUrl = processedImageUrl.isNotEmpty ? processedImageUrl : null;
            _hasError = _imageUrl == null;
            
            if (_imageUrl != null) {
              debugPrint('✅ Profile picture processed successfully');
            } else {
              debugPrint('❌ Profile picture processing failed');
            }
          } else {
            _imageUrl = null;
            _hasError = true;
            debugPrint('❌ Failed to load profile picture: ${result.errorMessage}');
          }
        });
        
        // Notify parent of loaded image after build completes
        if (widget.onImageLoaded != null) {
          WidgetsBinding.instance.addPostFrameCallback((_) {
            widget.onImageLoaded!(_imageUrl);
          });
        }
      }
    } catch (e) {
      debugPrint('❌ Error loading profile picture: $e');
      if (mounted) {
        setState(() {
          _isLoading = false;
          _hasError = true;
          _imageUrl = null;
        });
        
        // Notify parent that image failed to load after build completes
        if (widget.onImageLoaded != null) {
          WidgetsBinding.instance.addPostFrameCallback((_) {
            widget.onImageLoaded!(null);
          });
        }
      }
    }
  }

  Widget _buildDefaultAvatar() {
    return Container(
      width: widget.size.w,
      height: widget.size.h,
      decoration: BoxDecoration(
        gradient: LinearGradient(
          colors: AppTheme.affirmationGradient,
          begin: Alignment.topLeft,
          end: Alignment.bottomRight,
        ),
        shape: BoxShape.circle,
        border: widget.showBorder
            ? Border.all(
                color: widget.borderColor ?? AppTheme.affirmationGradient[0],
                width: widget.borderWidth,
              )
            : null,
        boxShadow: [
          BoxShadow(
            color: AppTheme.affirmationGradient[0].withOpacity(0.4),
            blurRadius: 12.r,
            offset: Offset(0, 4.h),
          ),
        ],
      ),
      child: Icon(
        Icons.person_rounded,
        size: (widget.size * 0.6).sp,
        color: AppTheme.white,
      ),
    );
  }

  Widget _buildLoadingAvatar() {
    return Container(
      width: widget.size.w,
      height: widget.size.h,
      decoration: BoxDecoration(
        gradient: LinearGradient(
          colors: AppTheme.affirmationGradient.map((c) => c.withOpacity(0.3)).toList(),
          begin: Alignment.topLeft,
          end: Alignment.bottomRight,
        ),
        shape: BoxShape.circle,
        border: widget.showBorder
            ? Border.all(
                color: widget.borderColor ?? AppTheme.affirmationGradient[0].withOpacity(0.5),
                width: widget.borderWidth,
              )
            : null,
      ),
      child: Center(
        child: SizedBox(
          width: (widget.size * 0.4).w,
          height: (widget.size * 0.4).h,
          child: CircularProgressIndicator(
            strokeWidth: 2.0,
            valueColor: AlwaysStoppedAnimation<Color>(
              AppTheme.affirmationGradient[0],
            ),
          ),
        ),
      ),
    );
  }

  Widget _buildProfileImage() {
    if (_imageUrl == null || _imageUrl!.isEmpty) {
      return _buildDefaultAvatar();
    }

    return Container(
      width: widget.size.w,
      height: widget.size.h,
      decoration: BoxDecoration(
        shape: BoxShape.circle,
        border: widget.showBorder
            ? Border.all(
                color: widget.borderColor ?? AppTheme.socialGradient[0],
                width: widget.borderWidth,
              )
            : null,
      ),
      child: ClipOval(
        child: _imageUrl!.startsWith('data:')
            ? Image.memory(
                // Handle base64 data URLs
                Uri.parse(_imageUrl!).data!.contentAsBytes(),
                width: widget.size.w,
                height: widget.size.h,
                fit: BoxFit.cover,
                errorBuilder: (context, error, stackTrace) {
                  debugPrint('❌ Error loading profile image: $error');
                  return _buildDefaultAvatar();
                },
              )
            : Image.network(
                _imageUrl!,
                width: widget.size.w,
                height: widget.size.h,
                fit: BoxFit.cover,
                loadingBuilder: (context, child, loadingProgress) {
                  if (loadingProgress == null) return child;
                  return _buildLoadingAvatar();
                },
                errorBuilder: (context, error, stackTrace) {
                  debugPrint('❌ Error loading profile image: $error');
                  return _buildDefaultAvatar();
                },
              ),
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    Widget avatar;

    if (_isLoading) {
      avatar = _buildLoadingAvatar();
    } else if (_hasError || _imageUrl == null || _imageUrl!.isEmpty) {
      avatar = _buildDefaultAvatar();
    } else {
      avatar = _buildProfileImage();
    }

    // Wrap with GestureDetector if onTap is provided
    if (widget.onTap != null) {
      return GestureDetector(
        onTap: widget.onTap,
        child: avatar,
      );
    }

    return avatar;
  }
}