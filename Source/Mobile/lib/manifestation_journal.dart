import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:manifestation_app/gratitude_selection.dart';
import 'package:manifestation_app/journal_prompt.dart' hide GratitudeSection;
import 'package:manifestation_app/journey_entry.dart';
import 'package:manifestation_app/mood_selector.dart';
import 'package:manifestation_app/calendar_view_screen.dart';
import '../colors.dart';

class ManifestationJournalScreen extends StatefulWidget {
  @override
  _ManifestationJournalScreenState createState() => _ManifestationJournalScreenState();
}

class _ManifestationJournalScreenState extends State<ManifestationJournalScreen> {
  final PageController _pageController = PageController();
  final TextEditingController _reflectionController = TextEditingController();
  
  int currentPage = 0;
  Mood selectedMood = Mood.neutral;
  List<String> gratitudeItems = ['', '', ''];
  String reflection = '';
  String intention = '';
  bool isEditingExistingEntry = false;
  JournalEntry? existingEntry;
  
  final List<String> journalPrompts = [
    "What am I most grateful for today?",
    "What would make today feel successful?",
    "How can I align my actions with my intentions?",
    "What limiting belief can I release today?",
    "What abundance am I ready to receive?",
    "How did I show up as my best self today?",
    "What signs of manifestation did I notice?",
  ];

  @override
  void initState() {
    super.initState();
    _checkForExistingEntry();
  }

  @override
  void dispose() {
    _pageController.dispose();
    _reflectionController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.backgroundDark,
      body: SafeArea(
        child: Column(
          children: [
            // Header with calendar integration
            _buildHeader(),
            
            // Show existing entry warning if editing
            if (isEditingExistingEntry) _buildEditingWarning(),
            
            // Progress indicator
            _buildProgressIndicator(),
            
            // Page content
            Expanded(
              child: PageView(
                controller: _pageController,
                onPageChanged: (index) {
                  setState(() {
                    currentPage = index;
                  });
                },
                children: [
                  _buildMoodPage(),
                  _buildGratitudePage(),
                  _buildReflectionPage(),
                  _buildIntentionPage(),
                  _buildSummaryPage(),
                ],
              ),
            ),
            
            // Navigation
            _buildNavigation(),
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
                  isEditingExistingEntry ? 'Edit Journal Entry' : 'Daily Journal',
                  style: TextStyle(
                    fontSize: 24.sp,
                    fontWeight: FontWeight.bold,
                    color: AppColors.textLight,
                  ),
                ),
                Text(
                  _formatDate(DateTime.now()),
                  style: TextStyle(
                    fontSize: 14.sp,
                    color: AppColors.textMedium,
                  ),
                ),
              ],
            ),
          ),
          // Calendar icon for viewing past entries
          GestureDetector(
            onTap: _showCalendarView,
            child: Container(
              width: 44.w,
              height: 44.h,
              decoration: BoxDecoration(
                color: AppColors.gradientPurple.withOpacity(0.1),
                borderRadius: BorderRadius.circular(12.r),
                border: Border.all(
                  color: AppColors.gradientPurple.withOpacity(0.3),
                ),
              ),
              child: Icon(
                Icons.calendar_today,
                color: AppColors.gradientPurple,
                size: 20.sp,
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildEditingWarning() {
    return Container(
      margin: EdgeInsets.symmetric(horizontal: 20.w),
      padding: EdgeInsets.all(16.w),
      decoration: BoxDecoration(
        color: AppColors.accentGold.withOpacity(0.1),
        borderRadius: BorderRadius.circular(12.r),
        border: Border.all(
          color: AppColors.accentGold.withOpacity(0.3),
        ),
      ),
      child: Row(
        children: [
          Icon(
            Icons.edit,
            color: AppColors.accentGold,
            size: 16.sp,
          ),
          SizedBox(width: 8.w),
          Expanded(
            child: Text(
              'You\'re editing today\'s journal entry. Your changes will overwrite the existing entry.',
              style: TextStyle(
                fontSize: 12.sp,
                color: AppColors.accentGold,
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildProgressIndicator() {
    return Container(
      margin: EdgeInsets.symmetric(horizontal: 20.w, vertical: 16.h),
      child: Row(
        children: List.generate(5, (index) {
          return Expanded(
            child: Container(
              height: 4.h,
              margin: EdgeInsets.only(right: index < 4 ? 8.w : 0),
              decoration: BoxDecoration(
                color: index <= currentPage
                    ? AppColors.gradientPurple
                    : AppColors.textMedium.withOpacity(0.3),
                borderRadius: BorderRadius.circular(2.r),
              ),
            ),
          );
        }),
      ),
    );
  }

  Widget _buildMoodPage() {
    return Padding(
      padding: EdgeInsets.all(20.w),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          SizedBox(height: 20.h),
          Text(
            'How are you feeling?',
            style: TextStyle(
              fontSize: 28.sp,
              fontWeight: FontWeight.bold,
              color: AppColors.textLight,
            ),
          ),
          SizedBox(height: 8.h),
          Text(
            'Choose the mood that best describes your current state',
            style: TextStyle(
              fontSize: 16.sp,
              color: AppColors.textMedium,
            ),
          ),
          SizedBox(height: 20.h),
          Expanded(
            child: MoodSelector(
              selectedMood: selectedMood,
              onMoodSelected: (mood) {
                setState(() {
                  selectedMood = mood;
                });
              },
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildGratitudePage() {
    return Padding(
      padding: EdgeInsets.all(20.w),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          SizedBox(height: 20.h),
          Text(
            'Three Gratitudes',
            style: TextStyle(
              fontSize: 28.sp,
              fontWeight: FontWeight.bold,
              color: AppColors.textLight,
            ),
          ),
          SizedBox(height: 8.h),
          Text(
            'Write three things you\'re grateful for today',
            style: TextStyle(
              fontSize: 16.sp,
              color: AppColors.textMedium,
            ),
          ),
          SizedBox(height: 20.h),
          Expanded(
            child: GratitudeSection(
              gratitudeItems: gratitudeItems,
              onGratitudeChanged: (index, value) {
                setState(() {
                  gratitudeItems[index] = value;
                });
              },
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildReflectionPage() {
    final prompt = journalPrompts[DateTime.now().day % journalPrompts.length];
    
    return Padding(
      padding: EdgeInsets.all(20.w),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          SizedBox(height: 20.h),
          Text(
            'Daily Reflection',
            style: TextStyle(
              fontSize: 28.sp,
              fontWeight: FontWeight.bold,
              color: AppColors.textLight,
            ),
          ),
          SizedBox(height: 8.h),
          Text(
            'Take a moment to reflect on your day',
            style: TextStyle(
              fontSize: 16.sp,
              color: AppColors.textMedium,
            ),
          ),
          SizedBox(height: 20.h),
          Expanded(
            child: JournalPrompt(
              prompt: prompt,
              initialText: reflection,
              onTextChanged: (value) {
                setState(() {
                  reflection = value;
                });
              },
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildIntentionPage() {
    return Padding(
      padding: EdgeInsets.all(20.w),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          SizedBox(height: 20.h),
          Text(
            'Tomorrow\'s Intention',
            style: TextStyle(
              fontSize: 28.sp,
              fontWeight: FontWeight.bold,
              color: AppColors.textLight,
            ),
          ),
          SizedBox(height: 8.h),
          Text(
            'Set a clear intention for tomorrow',
            style: TextStyle(
              fontSize: 16.sp,
              color: AppColors.textMedium,
            ),
          ),
          SizedBox(height: 20.h),
          Expanded(
            child: Container(
              padding: EdgeInsets.all(20.w),
              decoration: BoxDecoration(
                color: AppColors.cardBackground,
                borderRadius: BorderRadius.circular(16.r),
                border: Border.all(
                  color: AppColors.gradientPurple.withOpacity(0.3),
                ),
              ),
              child: TextField(
                onChanged: (value) {
                  setState(() {
                    intention = value;
                  });
                },
                controller: TextEditingController(text: intention)
                  ..selection = TextSelection.fromPosition(
                    TextPosition(offset: intention.length),
                  ),
                style: TextStyle(
                  color: AppColors.textLight,
                  fontSize: 16.sp,
                  height: 1.5,
                ),
                decoration: InputDecoration(
                  hintText: 'Tomorrow, I intend to...',
                  hintStyle: TextStyle(
                    color: AppColors.textMedium.withOpacity(0.7),
                    fontSize: 16.sp,
                  ),
                  border: InputBorder.none,
                ),
                maxLines: null,
                expands: true,
                textAlignVertical: TextAlignVertical.top,
                textCapitalization: TextCapitalization.sentences,
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildSummaryPage() {
    return Padding(
      padding: EdgeInsets.all(20.w),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          SizedBox(height: 20.h),
          Text(
            'Journal Complete!',
            style: TextStyle(
              fontSize: 28.sp,
              fontWeight: FontWeight.bold,
              color: AppColors.textLight,
            ),
          ),
          SizedBox(height: 8.h),
          Text(
            'Your thoughts have been saved',
            style: TextStyle(
              fontSize: 16.sp,
              color: AppColors.textMedium,
            ),
          ),
          SizedBox(height: 20.h),
          Expanded(
            child: SingleChildScrollView(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  _buildSummarySection(
                    title: 'Mood',
                    content: selectedMood.displayName,
                    icon: selectedMood.icon,
                    color: selectedMood.color,
                  ),
                  SizedBox(height: 20.h),
                  _buildSummarySection(
                    title: 'Gratitudes',
                    content: gratitudeItems.where((item) => item.isNotEmpty).join('\n• '),
                    icon: Icons.favorite,
                    color: AppColors.gradientPink,
                  ),
                  SizedBox(height: 20.h),
                  if (reflection.isNotEmpty)
                    _buildSummarySection(
                      title: 'Reflection',
                      content: reflection,
                      icon: Icons.lightbulb,
                      color: AppColors.accentGold,
                    ),
                  SizedBox(height: 20.h),
                  if (intention.isNotEmpty)
                    _buildSummarySection(
                      title: 'Tomorrow\'s Intention',
                      content: intention,
                      icon: Icons.flag,
                      color: AppColors.secondaryTeal,
                    ),
                  SizedBox(height: 20.h),
                ],
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildSummarySection({
    required String title,
    required String content,
    required IconData icon,
    required Color color,
  }) {
    return Container(
      padding: EdgeInsets.all(16.w),
      decoration: BoxDecoration(
        color: AppColors.cardBackground,
        borderRadius: BorderRadius.circular(12.r),
        border: Border.all(
          color: color.withOpacity(0.3),
        ),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            children: [
              Icon(icon, color: color, size: 20.sp),
              SizedBox(width: 8.w),
              Text(
                title,
                style: TextStyle(
                  fontSize: 16.sp,
                  fontWeight: FontWeight.w600,
                  color: AppColors.textLight,
                ),
              ),
            ],
          ),
          SizedBox(height: 8.h),
          Text(
            content.isNotEmpty ? content : 'Not provided',
            style: TextStyle(
              fontSize: 14.sp,
              color: content.isNotEmpty ? AppColors.textMedium : AppColors.textMedium.withOpacity(0.5),
              height: 1.4,
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildNavigation() {
    return Container(
      padding: EdgeInsets.all(20.w),
      child: Row(
        children: [
          if (currentPage > 0)
            Expanded(
              child: GestureDetector(
                onTap: _previousPage,
                child: Container(
                  height: 50.h,
                  decoration: BoxDecoration(
                    color: AppColors.cardBackground,
                    borderRadius: BorderRadius.circular(12.r),
                    border: Border.all(
                      color: AppColors.textMedium.withOpacity(0.3),
                    ),
                  ),
                  child: Center(
                    child: Text(
                      'Previous',
                      style: TextStyle(
                        fontSize: 16.sp,
                        fontWeight: FontWeight.w600,
                        color: AppColors.textMedium,
                      ),
                    ),
                  ),
                ),
              ),
            ),
          if (currentPage > 0) SizedBox(width: 16.w),
          Expanded(
            flex: currentPage == 0 ? 1 : 1,
            child: GestureDetector(
              onTap: currentPage < 4 ? _nextPage : _saveEntry,
              child: Container(
                height: 50.h,
                decoration: BoxDecoration(
                  gradient: LinearGradient(
                    colors: AppColors.journalGradient,
                  ),
                  borderRadius: BorderRadius.circular(12.r),
                ),
                child: Center(
                  child: Text(
                    currentPage < 4 ? 'Next' : (isEditingExistingEntry ? 'Update Entry' : 'Save Entry'),
                    style: TextStyle(
                      fontSize: 16.sp,
                      fontWeight: FontWeight.w600,
                      color: AppColors.white,
                    ),
                  ),
                ),
              ),
            ),
          ),
        ],
      ),
    );
  }

  void _previousPage() {
    if (currentPage > 0) {
      _pageController.previousPage(
        duration: Duration(milliseconds: 300),
        curve: Curves.easeInOut,
      );
    }
  }

  void _nextPage() {
    if (currentPage < 4) {
      _pageController.nextPage(
        duration: Duration(milliseconds: 300),
        curve: Curves.easeInOut,
      );
    }
  }

  void _saveEntry() {
    final entry = JournalEntry(
      id: isEditingExistingEntry 
          ? existingEntry?.id ?? DateTime.now().millisecondsSinceEpoch.toString()
          : DateTime.now().millisecondsSinceEpoch.toString(),
      date: DateTime.now(),
      mood: selectedMood,
      gratitudes: gratitudeItems.where((item) => item.isNotEmpty).toList(),
      reflection: reflection,
      intention: intention,
    );
    
    // TODO: Save to local storage
    print('Saving journal entry: ${entry.toJson()}');
    
    // Show success and return to home
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text(isEditingExistingEntry ? 'Journal entry updated!' : 'Journal entry saved!'),
        backgroundColor: AppColors.gradientPurple,
      ),
    );
    
    Navigator.pop(context);
  }

  void _checkForExistingEntry() {
    // TODO: Check if there's already an entry for today
    // For now, we'll simulate this check
    if (_hasEntryForToday()) {
      _loadExistingEntry();
    }
  }

  bool _hasEntryForToday() {
    // TODO: Implement actual check against local storage
    // This would check your storage mechanism
    return false; // Change this based on actual data
  }

  void _loadExistingEntry() {
    // TODO: Load existing entry data from storage
    setState(() {
      isEditingExistingEntry = true;
      // Populate fields with existing data when you implement storage
      // selectedMood = existingEntry!.mood;
      // gratitudeItems = List.from(existingEntry!.gratitudes);
      // reflection = existingEntry!.reflection;
      // intention = existingEntry!.intention;
    });
  }

  void _showCalendarView() async {
    final result = await Navigator.push(
      context,
      MaterialPageRoute(
        builder: (context) => CalendarViewScreen(),
      ),
    );
    
    if (result != null) {
      if (result == 'create_new') {
        // User wants to create a new entry for today
        _checkForExistingEntry();
      } else if (result.toString().startsWith('edit_')) {
        // User wants to edit an existing entry
        final entryId = result.toString().substring(5);
        _loadEntryForEdit(entryId);
      }
    }
  }

  void _loadEntryForEdit(String entryId) {
    // TODO: Load specific entry for editing
    setState(() {
      isEditingExistingEntry = true;
      // Load entry data by ID from your storage
    });
  }

  String _formatDate(DateTime date) {
    final months = [
      'January', 'February', 'March', 'April', 'May', 'June',
      'July', 'August', 'September', 'October', 'November', 'December'
    ];
    return '${months[date.month - 1]} ${date.day}, ${date.year}';
  }
}