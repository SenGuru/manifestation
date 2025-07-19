import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:manifestation_app/components/journey_entry.dart';
import 'package:manifestation_app/constants/colors.dart';
import 'package:manifestation_app/screens/journal_entry_details_screen.dart';
import 'package:manifestation_app/screens/journal_stats_screen.dart';

class CalendarViewScreen extends StatefulWidget {
  @override
  _CalendarViewScreenState createState() => _CalendarViewScreenState();
}

class _CalendarViewScreenState extends State<CalendarViewScreen> {
  DateTime selectedDate = DateTime.now();
  DateTime currentMonth = DateTime.now();
  Map<String, JournalEntry> journalEntries = {}; // Key: YYYY-MM-DD format
  
  @override
  void initState() {
    super.initState();
    _loadJournalEntries();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.backgroundDark,
      body: SafeArea(
        child: Column(
          children: [
            _buildHeader(),
            _buildMonthSelector(),
            _buildCalendarGrid(),
            _buildSelectedDateDetails(),
          ],
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
                color: AppColors.cardBackground,
                borderRadius: BorderRadius.circular(12.r),
              ),
              child: Icon(
                Icons.arrow_back,
                color: AppColors.textLight,
                size: 20.sp,
              ),
            ),
          ),
          SizedBox(width: 16.w),
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  'Journal Calendar',
                  style: TextStyle(
                    fontSize: 24.sp,
                    fontWeight: FontWeight.bold,
                    color: AppColors.textLight,
                  ),
                ),
                Text(
                  'View your manifestation journey',
                  style: TextStyle(
                    fontSize: 14.sp,
                    color: AppColors.textMedium,
                  ),
                ),
              ],
            ),
          ),
          _buildStatsButton(),
        ],
      ),
    );
  }

  Widget _buildStatsButton() {
    return GestureDetector(
      onTap: _showStatsView,
      child: Container(
        width: 44.w,
        height: 44.h,
        decoration: BoxDecoration(
          color: AppColors.accentGold.withOpacity(0.1),
          borderRadius: BorderRadius.circular(12.r),
          border: Border.all(
            color: AppColors.accentGold.withOpacity(0.3),
          ),
        ),
        child: Icon(
          Icons.analytics_outlined,
          color: AppColors.accentGold,
          size: 20.sp,
        ),
      ),
    );
  }

  Widget _buildMonthSelector() {
    return Padding(
      padding: EdgeInsets.symmetric(horizontal: 20.w),
      child: Row(
        mainAxisAlignment: MainAxisAlignment.spaceBetween,
        children: [
          GestureDetector(
            onTap: _previousMonth,
            child: Container(
              width: 40.w,
              height: 40.h,
              decoration: BoxDecoration(
                color: AppColors.cardBackground,
                borderRadius: BorderRadius.circular(10.r),
              ),
              child: Icon(
                Icons.chevron_left,
                color: AppColors.textLight,
                size: 20.sp,
              ),
            ),
          ),
          Text(
            _formatMonthYear(currentMonth),
            style: TextStyle(
              fontSize: 20.sp,
              fontWeight: FontWeight.w600,
              color: AppColors.textLight,
            ),
          ),
          GestureDetector(
            onTap: _nextMonth,
            child: Container(
              width: 40.w,
              height: 40.h,
              decoration: BoxDecoration(
                color: AppColors.cardBackground,
                borderRadius: BorderRadius.circular(10.r),
              ),
              child: Icon(
                Icons.chevron_right,
                color: AppColors.textLight,
                size: 20.sp,
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildCalendarGrid() {
    return Expanded(
      flex: 2,
      child: Padding(
        padding: EdgeInsets.all(20.w),
        child: Column(
          children: [
            // Weekday headers
            Row(
              children: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat']
                  .map((day) => Expanded(
                        child: Center(
                          child: Text(
                            day,
                            style: TextStyle(
                              fontSize: 14.sp,
                              fontWeight: FontWeight.w500,
                              color: AppColors.textMedium,
                            ),
                          ),
                        ),
                      ))
                  .toList(),
            ),
            SizedBox(height: 16.h),
            
            // Calendar days
            Expanded(
              child: GridView.builder(
                gridDelegate: SliverGridDelegateWithFixedCrossAxisCount(
                  crossAxisCount: 7,
                  childAspectRatio: 1,
                  mainAxisSpacing: 8.h,
                  crossAxisSpacing: 8.w,
                ),
                itemCount: _getDaysInMonth(),
                itemBuilder: (context, index) {
                  final date = _getDateForIndex(index);
                  final hasEntry = _hasEntryForDate(date);
                  final isSelected = _isSameDay(date, selectedDate);
                  final isToday = _isSameDay(date, DateTime.now());
                  final isCurrentMonth = date.month == currentMonth.month;
                  
                  return GestureDetector(
                    onTap: () => _selectDate(date),
                    child: Container(
                      decoration: BoxDecoration(
                        color: isSelected 
                            ? AppColors.gradientPurple 
                            : hasEntry 
                                ? AppColors.gradientPink.withOpacity(0.1)
                                : Colors.transparent,
                        borderRadius: BorderRadius.circular(8.r),
                        border: isToday 
                            ? Border.all(color: AppColors.accentGold, width: 2)
                            : hasEntry
                                ? Border.all(color: AppColors.gradientPink.withOpacity(0.3))
                                : null,
                      ),
                      child: Stack(
                        children: [
                          Center(
                            child: Text(
                              '${date.day}',
                              style: TextStyle(
                                fontSize: 14.sp,
                                fontWeight: isSelected ? FontWeight.bold : FontWeight.normal,
                                color: !isCurrentMonth
                                    ? AppColors.textMedium.withOpacity(0.3)
                                    : isSelected
                                        ? AppColors.white
                                        : AppColors.textLight,
                              ),
                            ),
                          ),
                          if (hasEntry && !isSelected)
                            Positioned(
                              top: 4.h,
                              right: 4.w,
                              child: Container(
                                width: 6.w,
                                height: 6.h,
                                decoration: BoxDecoration(
                                  color: _getMoodColorForDate(date),
                                  shape: BoxShape.circle,
                                ),
                              ),
                            ),
                        ],
                      ),
                    ),
                  );
                },
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildSelectedDateDetails() {
    final entry = _getEntryForDate(selectedDate);
    final isToday = _isSameDay(selectedDate, DateTime.now());
    
    return Expanded(
      flex: 1,
      child: Container(
        width: double.infinity,
        padding: EdgeInsets.all(20.w),
        decoration: BoxDecoration(
          color: AppColors.cardBackground,
          borderRadius: BorderRadius.vertical(top: Radius.circular(24.r)),
        ),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            // Date header
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      _formatSelectedDate(selectedDate),
                      style: TextStyle(
                        fontSize: 20.sp,
                        fontWeight: FontWeight.bold,
                        color: AppColors.textLight,
                      ),
                    ),
                    if (isToday)
                      Text(
                        'Today',
                        style: TextStyle(
                          fontSize: 14.sp,
                          color: AppColors.accentGold,
                          fontWeight: FontWeight.w500,
                        ),
                      ),
                  ],
                ),
                if (entry != null || isToday)
                  _buildActionButton(entry, isToday),
              ],
            ),
            
            SizedBox(height: 16.h),
            
            // Entry content
            Expanded(
              child: entry != null 
                  ? _buildEntryPreview(entry)
                  : _buildEmptyState(isToday),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildActionButton(JournalEntry? entry, bool isToday) {
    if (entry != null) {
      return Row(
        mainAxisSize: MainAxisSize.min,
        children: [
          GestureDetector(
            onTap: () => _viewEntry(entry),
            child: Container(
              padding: EdgeInsets.symmetric(horizontal: 16.w, vertical: 8.h),
              decoration: BoxDecoration(
                color: AppColors.gradientPurple.withOpacity(0.1),
                borderRadius: BorderRadius.circular(20.r),
                border: Border.all(color: AppColors.gradientPurple.withOpacity(0.3)),
              ),
              child: Text(
                'View',
                style: TextStyle(
                  fontSize: 12.sp,
                  fontWeight: FontWeight.w500,
                  color: AppColors.gradientPurple,
                ),
              ),
            ),
          ),
          if (isToday) SizedBox(width: 8.w),
          if (isToday)
            GestureDetector(
              onTap: () => _editEntry(entry),
              child: Container(
                padding: EdgeInsets.symmetric(horizontal: 16.w, vertical: 8.h),
                decoration: BoxDecoration(
                  color: AppColors.accentGold.withOpacity(0.1),
                  borderRadius: BorderRadius.circular(20.r),
                  border: Border.all(color: AppColors.accentGold.withOpacity(0.3)),
                ),
                child: Text(
                  'Edit',
                  style: TextStyle(
                    fontSize: 12.sp,
                    fontWeight: FontWeight.w500,
                    color: AppColors.accentGold,
                  ),
                ),
              ),
            ),
        ],
      );
    } else if (isToday) {
      return GestureDetector(
        onTap: _createTodayEntry,
        child: Container(
          padding: EdgeInsets.symmetric(horizontal: 16.w, vertical: 8.h),
          decoration: BoxDecoration(
            gradient: LinearGradient(colors: AppColors.journalGradient),
            borderRadius: BorderRadius.circular(20.r),
          ),
          child: Text(
            'Create',
            style: TextStyle(
              fontSize: 12.sp,
              fontWeight: FontWeight.w500,
              color: AppColors.white,
            ),
          ),
        ),
      );
    }
    return SizedBox.shrink();
  }

  Widget _buildEntryPreview(JournalEntry entry) {
    return SingleChildScrollView(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          // Mood
          Row(
            children: [
              Icon(entry.mood.icon, color: entry.mood.color, size: 16.sp),
              SizedBox(width: 8.w),
              Text(
                entry.mood.displayName,
                style: TextStyle(
                  fontSize: 14.sp,
                  fontWeight: FontWeight.w500,
                  color: AppColors.textLight,
                ),
              ),
            ],
          ),
          
          if (entry.gratitudes.isNotEmpty) ...[
            SizedBox(height: 12.h),
            Text(
              'Gratitudes',
              style: TextStyle(
                fontSize: 12.sp,
                fontWeight: FontWeight.w600,
                color: AppColors.gradientPink,
              ),
            ),
            SizedBox(height: 4.h),
            ...entry.gratitudes.take(2).map((gratitude) => Padding(
              padding: EdgeInsets.only(bottom: 2.h),
              child: Text(
                '• $gratitude',
                style: TextStyle(
                  fontSize: 12.sp,
                  color: AppColors.textMedium,
                ),
                maxLines: 1,
                overflow: TextOverflow.ellipsis,
              ),
            )).toList(),
            if (entry.gratitudes.length > 2)
              Text(
                '• +${entry.gratitudes.length - 2} more',
                style: TextStyle(
                  fontSize: 12.sp,
                  color: AppColors.textMedium.withOpacity(0.7),
                  fontStyle: FontStyle.italic,
                ),
              ),
          ],
          
          if (entry.reflection.isNotEmpty) ...[
            SizedBox(height: 12.h),
            Text(
              'Reflection',
              style: TextStyle(
                fontSize: 12.sp,
                fontWeight: FontWeight.w600,
                color: AppColors.accentGold,
              ),
            ),
            SizedBox(height: 4.h),
            Text(
              entry.reflection,
              style: TextStyle(
                fontSize: 12.sp,
                color: AppColors.textMedium,
              ),
              maxLines: 3,
              overflow: TextOverflow.ellipsis,
            ),
          ],
          
          if (entry.intention.isNotEmpty) ...[
            SizedBox(height: 12.h),
            Text(
              'Intention',
              style: TextStyle(
                fontSize: 12.sp,
                fontWeight: FontWeight.w600,
                color: AppColors.secondaryTeal,
              ),
            ),
            SizedBox(height: 4.h),
            Text(
              entry.intention,
              style: TextStyle(
                fontSize: 12.sp,
                color: AppColors.textMedium,
              ),
              maxLines: 2,
              overflow: TextOverflow.ellipsis,
            ),
          ],
        ],
      ),
    );
  }

  Widget _buildEmptyState(bool isToday) {
    return Center(
      child: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          Icon(
            isToday ? Icons.edit_calendar : Icons.calendar_today_outlined,
            color: AppColors.textMedium.withOpacity(0.5),
            size: 32.sp,
          ),
          SizedBox(height: 12.h),
          Text(
            isToday 
                ? 'No journal entry for today yet'
                : 'No journal entry for this date',
            style: TextStyle(
              fontSize: 14.sp,
              color: AppColors.textMedium,
            ),
          ),
          if (isToday) ...[
            SizedBox(height: 8.h),
            Text(
              'Tap "Create" to start your daily reflection',
              style: TextStyle(
                fontSize: 12.sp,
                color: AppColors.textMedium.withOpacity(0.7),
              ),
            ),
          ],
        ],
      ),
    );
  }

  // Helper methods
  void _previousMonth() {
    setState(() {
      currentMonth = DateTime(currentMonth.year, currentMonth.month - 1);
    });
  }

  void _nextMonth() {
    setState(() {
      currentMonth = DateTime(currentMonth.year, currentMonth.month + 1);
    });
  }

  void _selectDate(DateTime date) {
    setState(() {
      selectedDate = date;
    });
  }

  int _getDaysInMonth() {
    final firstDay = DateTime(currentMonth.year, currentMonth.month, 1);
    final lastDay = DateTime(currentMonth.year, currentMonth.month + 1, 0);
    final startDate = firstDay.subtract(Duration(days: firstDay.weekday % 7));
    final endDate = lastDay.add(Duration(days: 6 - lastDay.weekday % 7));
    return endDate.difference(startDate).inDays + 1;
  }

  DateTime _getDateForIndex(int index) {
    final firstDay = DateTime(currentMonth.year, currentMonth.month, 1);
    final startDate = firstDay.subtract(Duration(days: firstDay.weekday % 7));
    return startDate.add(Duration(days: index));
  }

  bool _hasEntryForDate(DateTime date) {
    return journalEntries.containsKey(_formatDateKey(date));
  }

  JournalEntry? _getEntryForDate(DateTime date) {
    return journalEntries[_formatDateKey(date)];
  }

  Color _getMoodColorForDate(DateTime date) {
    final entry = _getEntryForDate(date);
    return entry?.mood.color ?? AppColors.gradientPink;
  }

  bool _isSameDay(DateTime date1, DateTime date2) {
    return date1.year == date2.year &&
           date1.month == date2.month &&
           date1.day == date2.day;
  }

  String _formatDateKey(DateTime date) {
    return '${date.year}-${date.month.toString().padLeft(2, '0')}-${date.day.toString().padLeft(2, '0')}';
  }

  String _formatMonthYear(DateTime date) {
    final months = [
      'January', 'February', 'March', 'April', 'May', 'June',
      'July', 'August', 'September', 'October', 'November', 'December'
    ];
    return '${months[date.month - 1]} ${date.year}';
  }

  String _formatSelectedDate(DateTime date) {
    final months = [
      'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
      'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'
    ];
    return '${months[date.month - 1]} ${date.day}, ${date.year}';
  }

  void _loadJournalEntries() {
    // TODO: Load from local storage
    // Temporary mock data for demonstration
    journalEntries = {
      '2025-07-01': JournalEntry(
        id: '1',
        date: DateTime(2025, 7, 1),
        mood: Mood.amazing,
        gratitudes: ['Morning coffee', 'Good weather', 'Time with family'],
        reflection: 'Today was a wonderful day filled with small joys...',
        intention: 'Tomorrow I will focus on being present',
      ),
      '2025-07-03': JournalEntry(
        id: '2',
        date: DateTime(2025, 7, 3),
        mood: Mood.good,
        gratitudes: ['Peaceful morning', 'Good book'],
        reflection: 'Felt very centered today...',
        intention: 'Continue this peaceful energy',
      ),
    };
  }

  void _createTodayEntry() {
    Navigator.pop(context, 'create_new');
  }

  void _editEntry(JournalEntry entry) {
    Navigator.pop(context, 'edit_${entry.id}');
  }

  void _viewEntry(JournalEntry entry) {
    // Navigate to detailed view
    Navigator.push(
      context,
      MaterialPageRoute(
        builder: (context) => JournalEntryDetailScreen(entry: entry),
      ),
    );
  }

  void _showStatsView() {
    Navigator.push(
      context,
      MaterialPageRoute(
        builder: (context) => JournalStatsScreen(entries: journalEntries),
      ),
    );
  }
}