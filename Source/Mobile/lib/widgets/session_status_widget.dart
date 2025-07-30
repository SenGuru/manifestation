import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import '../services/session_manager.dart';
import '../constants/app_theme.dart';
import '../widgets/stylish_loading.dart';

/// Widget that displays session status information
class SessionStatusWidget extends StatefulWidget {
  final bool showDetails;
  
  const SessionStatusWidget({
    Key? key,
    this.showDetails = false,
  }) : super(key: key);

  @override
  _SessionStatusWidgetState createState() => _SessionStatusWidgetState();
}

class _SessionStatusWidgetState extends State<SessionStatusWidget> {
  SessionInfo? _sessionInfo;
  bool _isLoading = true;

  @override
  void initState() {
    super.initState();
    _loadSessionInfo();
  }

  Future<void> _loadSessionInfo() async {
    try {
      final sessionInfo = await SessionManager.getSessionInfo();
      setState(() {
        _sessionInfo = sessionInfo;
        _isLoading = false;
      });
    } catch (e) {
      print('Error loading session info: $e');
      setState(() {
        _isLoading = false;
      });
    }
  }

  Color _getStatusColor() {
    if (_sessionInfo == null) return Colors.grey;
    
    switch (_sessionInfo!.state) {
      case SessionState.loggedIn:
        return Colors.green;
      case SessionState.loggedOut:
        return Colors.grey;
      case SessionState.expired:
        return Colors.orange;
      case SessionState.timedOut:
        return Colors.red;
      case SessionState.error:
        return Colors.red;
    }
  }

  String _getStatusText() {
    if (_sessionInfo == null) return 'Unknown';
    
    switch (_sessionInfo!.state) {
      case SessionState.loggedIn:
        return 'Logged In';
      case SessionState.loggedOut:
        return 'Logged Out';
      case SessionState.expired:
        return 'Session Expired';
      case SessionState.timedOut:
        return 'Session Timed Out';
      case SessionState.error:
        return 'Session Error';
    }
  }

  @override
  Widget build(BuildContext context) {
    if (_isLoading) {
      return Container(
        padding: EdgeInsets.all(8.w),
        child: Row(
          mainAxisSize: MainAxisSize.min,
          children: [
            CompactStylishLoading(
              color: AppTheme.textMedium,
              size: 12.w,
            ),
            SizedBox(width: 8.w),
            Text(
              'Loading...',
              style: TextStyle(
                fontSize: 12.sp,
                color: AppTheme.textMedium,
              ),
            ),
          ],
        ),
      );
    }

    if (widget.showDetails) {
      return _buildDetailedStatus();
    } else {
      return _buildSimpleStatus();
    }
  }

  Widget _buildSimpleStatus() {
    return Container(
      padding: EdgeInsets.symmetric(horizontal: 8.w, vertical: 4.h),
      decoration: BoxDecoration(
        color: _getStatusColor().withOpacity(0.1),
        borderRadius: BorderRadius.circular(12.r),
        border: Border.all(
          color: _getStatusColor().withOpacity(0.3),
          width: 1,
        ),
      ),
      child: Row(
        mainAxisSize: MainAxisSize.min,
        children: [
          Container(
            width: 8.w,
            height: 8.h,
            decoration: BoxDecoration(
              color: _getStatusColor(),
              shape: BoxShape.circle,
            ),
          ),
          SizedBox(width: 6.w),
          Text(
            _getStatusText(),
            style: TextStyle(
              fontSize: 11.sp,
              color: _getStatusColor(),
              fontWeight: FontWeight.w500,
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildDetailedStatus() {
    return Container(
      padding: EdgeInsets.all(16.w),
      decoration: BoxDecoration(
        color: AppTheme.cardBackground,
        borderRadius: BorderRadius.circular(12.r),
        border: Border.all(
          color: AppTheme.textMedium.withOpacity(0.2),
        ),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          // Status header
          Row(
            children: [
              Container(
                width: 12.w,
                height: 12.h,
                decoration: BoxDecoration(
                  color: _getStatusColor(),
                  shape: BoxShape.circle,
                ),
              ),
              SizedBox(width: 8.w),
              Text(
                'Session Status',
                style: TextStyle(
                  fontSize: 16.sp,
                  fontWeight: FontWeight.w600,
                  color: AppTheme.textLight,
                ),
              ),
            ],
          ),
          
          SizedBox(height: 12.h),
          
          // Status details
          if (_sessionInfo != null) ...[
            _buildStatusItem('Status', _getStatusText()),
            if (_sessionInfo!.sessionId != null)
              _buildStatusItem('Session ID', _sessionInfo!.sessionId!.substring(0, 12) + '...'),
            if (_sessionInfo!.lastLoginTime != null)
              _buildStatusItem('Last Login', _formatDateTime(_sessionInfo!.lastLoginTime!)),
            if (_sessionInfo!.tokenExpiry != null)
              _buildStatusItem('Token Expires', _formatDateTime(_sessionInfo!.tokenExpiry!)),
            if (_sessionInfo!.lastActivity != null)
              _buildStatusItem('Last Activity', _formatDateTime(_sessionInfo!.lastActivity!)),
            _buildStatusItem('Remember Me', _sessionInfo!.rememberMe ? 'Yes' : 'No'),
            _buildStatusItem('Auto Login', _sessionInfo!.autoLoginEnabled ? 'Yes' : 'No'),
          ],
        ],
      ),
    );
  }

  Widget _buildStatusItem(String label, String value) {
    return Padding(
      padding: EdgeInsets.only(bottom: 6.h),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          SizedBox(
            width: 100.w,
            child: Text(
              label + ':',
              style: TextStyle(
                fontSize: 13.sp,
                color: AppTheme.textMedium,
                fontWeight: FontWeight.w500,
              ),
            ),
          ),
          Expanded(
            child: Text(
              value,
              style: TextStyle(
                fontSize: 13.sp,
                color: AppTheme.textLight,
              ),
            ),
          ),
        ],
      ),
    );
  }

  String _formatDateTime(DateTime dateTime) {
    final now = DateTime.now();
    final difference = now.difference(dateTime);
    
    if (difference.inMinutes < 1) {
      return 'Just now';
    } else if (difference.inMinutes < 60) {
      return '${difference.inMinutes}m ago';
    } else if (difference.inHours < 24) {
      return '${difference.inHours}h ago';
    } else {
      return '${dateTime.day}/${dateTime.month}/${dateTime.year}';
    }
  }
}