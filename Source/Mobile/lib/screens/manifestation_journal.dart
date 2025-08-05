import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:fun/components/gratitude_selection.dart';
import 'package:fun/models/journey_entry.dart';
import 'package:fun/constants/colors.dart';
import 'package:fun/constants/app_theme.dart';
import 'package:fun/models/mood_selector.dart';
import 'package:fun/screens/calendar_view_screen.dart';
import 'package:fun/services/daily_journal_service.dart';
import 'package:fun/services/guided_tour_service.dart';
import 'package:fun/widgets/stylish_loading.dart';
import 'package:fun/widgets/keyboard_dismissal_wrapper.dart';

class ManifestationJournalScreen extends StatefulWidget {
  // Enhanced with manual draft saving functionality - debug exit dialog
  @override
  _ManifestationJournalScreenState createState() => _ManifestationJournalScreenState();
}

class _ManifestationJournalScreenState extends State<ManifestationJournalScreen> {
  final PageController _pageController = PageController();
  final TextEditingController _reflectionController = TextEditingController();
  final DailyJournalService _journalService = DailyJournalService();
  
  int currentPage = 0;
  Mood selectedMood = Mood.neutral;
  List<String> gratitudeItems = ['', '', '', '', '']; // Expanded to 5 gratitudes
  String reflection = '';
  String intention = '';
  bool isEditingExistingEntry = false;
  JournalEntry? existingEntry;
  bool isLoading = false;
  bool isSaving = false;
  
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
    print('📖 ManifestationJournalScreen: Initializing...');
    
    // Initialize the service
    _journalService.initialize();
    
    // For debugging purposes - uncomment this line to run constraint debugging
    // _journalService.runConstraintDebugging();
    
    // Check for existing entry for today
    _checkForExistingEntry();
    
    // Load any existing draft
    _loadDraftProgress();
    
    // Show guided tour after the widget is built
    WidgetsBinding.instance.addPostFrameCallback((_) {
      _showGuidedTourIfNeeded();
    });
  }

  Future<void> _showGuidedTourIfNeeded() async {
    final tourService = GuidedTourService();
    await tourService.showTourIfNeeded(
      context,
      GuidedTourService.journalTourKey,
      tourService.getJournalTourSteps(),
    );
  }

  @override
  void dispose() {
    _pageController.dispose();
    _reflectionController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    // Show loading screen while checking for existing entry
    if (isLoading) {
      return Scaffold(
        backgroundColor: AppTheme.backgroundDark,
        body: StylishLoading(
          message: 'Loading journal...',
        ),
      );
    }

    return WillPopScope(
      onWillPop: _onWillPop,
      child: Scaffold(
        backgroundColor: AppTheme.backgroundDark,
        resizeToAvoidBottomInset: true,
        body: KeyboardDismissalWrapper(
          child: SafeArea(
            child: Column(
              children: [
                // Simple header
                _buildSimpleHeader(),
                
                // Compact progress indicator
                _buildCompactProgress(),
                
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
        ),
      ),
    );
  }

  Widget _buildSimpleHeader() {
    return Padding(
      padding: EdgeInsets.all(20.w),
      child: Row(
        children: [
          GestureDetector(
            onTap: _handleBackButton,
            child: Container(
              width: 44.w,
              height: 44.h,
              decoration: BoxDecoration(
                color: AppTheme.cardBackground,
                borderRadius: BorderRadius.circular(12.r),
                border: Border.all(
                  color: AppTheme.textMedium.withOpacity(0.1),
                ),
              ),
              child: Icon(
                Icons.arrow_back,
                color: AppTheme.textLight,
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
                  isEditingExistingEntry ? 'Edit Entry' : 'Daily Journal',
                  style: TextStyle(
                    fontSize: 24.sp,
                    fontWeight: FontWeight.bold,
                    color: AppTheme.textLight,
                  ),
                ),
                Text(
                  'Manifest your intentions through reflection',
                  style: TextStyle(
                    fontSize: 14.sp,
                    color: AppTheme.textMedium,
                  ),
                ),
              ],
            ),
          ),
          // Save Draft button
          GestureDetector(
            onTap: _hasUnsavedChanges() ? () async => await _saveDraftProgress(true) : null,
            child: Container(
              width: 44.w,
              height: 44.h,
              decoration: BoxDecoration(
                color: _hasUnsavedChanges() 
                    ? AppTheme.journalGradient[0].withOpacity(0.1)
                    : AppTheme.cardBackground,
                borderRadius: BorderRadius.circular(12.r),
                border: Border.all(
                  color: _hasUnsavedChanges() 
                      ? AppTheme.journalGradient[0].withOpacity(0.3)
                      : AppTheme.textMedium.withOpacity(0.1),
                ),
              ),
              child: Icon(
                Icons.save_outlined,
                color: _hasUnsavedChanges() 
                    ? AppTheme.journalGradient[0]
                    : AppTheme.textMedium.withOpacity(0.5),
                size: 20.sp,
              ),
            ),
          ),
          SizedBox(width: 12.w),
          GestureDetector(
            onTap: _showCalendarView,
            child: Container(
              width: 44.w,
              height: 44.h,
              decoration: BoxDecoration(
                color: AppTheme.cardBackground,
                borderRadius: BorderRadius.circular(12.r),
                border: Border.all(
                  color: AppTheme.textMedium.withOpacity(0.1),
                ),
              ),
              child: Icon(
                Icons.calendar_today,
                color: AppTheme.journalGradient[0],
                size: 20.sp,
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildCompactProgress() {
    return Container(
      margin: EdgeInsets.symmetric(horizontal: 16.w, vertical: 8.h),
      child: Row(
        children: List.generate(5, (index) {
          return Expanded(
            child: Container(
              height: 3.h,
              margin: EdgeInsets.only(right: index < 4 ? 6.w : 0),
              decoration: BoxDecoration(
                color: index <= currentPage
                    ? AppTheme.journalGradient[0]
                    : AppTheme.textMedium.withOpacity(0.2),
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
      padding: EdgeInsets.symmetric(horizontal: 24.w, vertical: 20.h),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          // Simple header
          Text(
            'How are you feeling?',
            style: TextStyle(
              fontSize: 24.sp,
              fontWeight: FontWeight.bold,
              color: AppTheme.textPrimary,
            ),
          ),
          SizedBox(height: 8.h),
          Text(
            'Choose the mood that resonates with you today',
            style: TextStyle(
              fontSize: 16.sp,
              color: AppTheme.textSecondary,
            ),
          ),
          
          SizedBox(height: 32.h),
          
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
      padding: EdgeInsets.symmetric(horizontal: 24.w, vertical: 20.h),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
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
    return Padding(
      padding: EdgeInsets.symmetric(horizontal: 24.w, vertical: 20.h),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          // Simple header
          Text(
            'Daily Reflection',
            style: TextStyle(
              fontSize: 24.sp,
              fontWeight: FontWeight.bold,
              color: AppTheme.textPrimary,
            ),
          ),
          SizedBox(height: 8.h),
          Text(
            'Share your thoughts and insights from today',
            style: TextStyle(
              fontSize: 16.sp,
              color: AppTheme.textSecondary,
            ),
          ),
          
          SizedBox(height: 24.h),
          
          Expanded(
            child: Container(
              decoration: BoxDecoration(
                color: AppTheme.cardBackground,
                borderRadius: BorderRadius.circular(16.r),
                border: Border.all(
                  color: AppTheme.borderSecondary,
                ),
              ),
              child: TextField(
                controller: _reflectionController,
                onChanged: (value) {
                  setState(() {
                    reflection = value;
                  });
                },
                style: TextStyle(
                  color: AppTheme.textPrimary,
                  fontSize: 16.sp,
                  height: 1.6,
                  fontWeight: FontWeight.w400,
                ),
                decoration: InputDecoration(
                  hintText: 'Write about your day, your feelings, lessons learned, or anything that comes to mind...',
                  hintStyle: TextStyle(
                    color: AppTheme.textSecondary,
                    fontSize: 16.sp,
                    height: 1.6,
                    fontStyle: FontStyle.italic,
                  ),
                  border: InputBorder.none,
                  contentPadding: EdgeInsets.all(20.w),
                  isDense: false,
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

  Widget _buildIntentionPage() {
    return Padding(
      padding: EdgeInsets.symmetric(horizontal: 24.w, vertical: 20.h),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          // Simple header
          Text(
            'Tomorrow\'s Intention',
            style: TextStyle(
              fontSize: 24.sp,
              fontWeight: FontWeight.bold,
              color: AppTheme.textPrimary,
            ),
          ),
          SizedBox(height: 8.h),
          Text(
            'Set a positive intention for tomorrow',
            style: TextStyle(
              fontSize: 16.sp,
              color: AppTheme.textSecondary,
            ),
          ),
          
          SizedBox(height: 24.h),
          
          Expanded(
            child: Container(
              decoration: BoxDecoration(
                color: AppTheme.cardBackground,
                borderRadius: BorderRadius.circular(16.r),
                border: Border.all(
                  color: AppTheme.borderSecondary,
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
                  color: AppTheme.textPrimary,
                  fontSize: 16.sp,
                  height: 1.6,
                  fontWeight: FontWeight.w400,
                ),
                decoration: InputDecoration(
                  hintText: 'How do you want to show up tomorrow? What energy do you want to bring? What would make tomorrow meaningful?',
                  hintStyle: TextStyle(
                    color: AppTheme.textSecondary,
                    fontSize: 16.sp,
                    height: 1.6,
                    fontStyle: FontStyle.italic,
                  ),
                  border: InputBorder.none,
                  contentPadding: EdgeInsets.all(20.w),
                  isDense: false,
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
      padding: EdgeInsets.symmetric(horizontal: 24.w, vertical: 20.h),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          // Completion header with subtle styling
          Container(
            padding: EdgeInsets.all(32.w),
            decoration: BoxDecoration(
              gradient: LinearGradient(
                colors: [
                  AppTheme.cardBackground,
                  AppTheme.cardBackground.withOpacity(0.95),
                ],
                begin: Alignment.topLeft,
                end: Alignment.bottomRight,
              ),
              borderRadius: BorderRadius.circular(24.r),
              border: Border.all(
                color: AppTheme.journalGradient[0].withOpacity(0.2),
              ),
              boxShadow: [
                BoxShadow(
                  color: AppTheme.journalGradient[0].withOpacity(0.1),
                  blurRadius: 12.r,
                  offset: Offset(0, 4.h),
                ),
              ],
            ),
            child: Column(
              children: [
                Container(
                  padding: EdgeInsets.all(16.w),
                  decoration: BoxDecoration(
                    color: AppTheme.journalGradient[0],
                    borderRadius: BorderRadius.circular(20.r),
                    boxShadow: [
                      BoxShadow(
                        color: AppTheme.journalGradient[0].withOpacity(0.3),
                        blurRadius: 8.r,
                        offset: Offset(0, 2.h),
                      ),
                    ],
                  ),
                  child: Icon(
                    Icons.check_circle_outline,
                    color: Colors.white,
                    size: 28.sp,
                  ),
                ),
                SizedBox(height: 16.h),
                Text(
                  'Journal Complete!',
                  style: TextStyle(
                    fontSize: 24.sp,
                    fontWeight: FontWeight.bold,
                    color: AppTheme.textLight,
                  ),
                  textAlign: TextAlign.center,
                ),
                SizedBox(height: 6.h),
                Text(
                  'Your thoughts have been captured beautifully',
                  style: TextStyle(
                    fontSize: 15.sp,
                    color: AppTheme.textMedium,
                    height: 1.3,
                  ),
                  textAlign: TextAlign.center,
                ),
              ],
            ),
          ),
          
          SizedBox(height: 24.h),
          
          Expanded(
            child: SingleChildScrollView(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  _buildSummarySection(
                    title: 'Your Mood',
                    content: selectedMood.displayName,
                    icon: selectedMood.icon,
                  ),
                  SizedBox(height: 16.h),
                  _buildSummarySection(
                    title: 'Gratitudes',
                    content: gratitudeItems.where((item) => item.isNotEmpty).join('\n• '),
                    icon: Icons.favorite_outline,
                  ),
                  SizedBox(height: 16.h),
                  if (reflection.isNotEmpty)
                    _buildSummarySection(
                      title: 'Daily Reflection',
                      content: reflection,
                      icon: Icons.auto_stories_outlined,
                    ),
                  SizedBox(height: 16.h),
                  if (intention.isNotEmpty)
                    _buildSummarySection(
                      title: 'Tomorrow\'s Intention',
                      content: intention,
                      icon: Icons.wb_sunny_outlined,
                    ),
                  SizedBox(height: 24.h),
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
  }) {
    if (content.isEmpty) return SizedBox.shrink();
    
    return Container(
      padding: EdgeInsets.all(24.w),
      decoration: BoxDecoration(
        color: AppTheme.cardBackground,
        borderRadius: BorderRadius.circular(20.r),
        border: Border.all(
          color: AppTheme.journalGradient[0].withOpacity(0.2),
        ),
        boxShadow: [
          BoxShadow(
            color: AppTheme.journalGradient[0].withOpacity(0.06),
            blurRadius: 8.r,
            offset: Offset(0, 2.h),
          ),
        ],
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            children: [
              Container(
                padding: EdgeInsets.all(10.w),
                decoration: BoxDecoration(
                  color: AppTheme.journalGradient[0].withOpacity(0.1),
                  borderRadius: BorderRadius.circular(12.r),
                ),
                child: Icon(
                  icon,
                  color: AppTheme.journalGradient[0],
                  size: 18.sp,
                ),
              ),
              SizedBox(width: 14.w),
              Expanded(
                child: Text(
                  title,
                  style: TextStyle(
                    fontSize: 17.sp,
                    fontWeight: FontWeight.bold,
                    color: AppTheme.textLight,
                  ),
                ),
              ),
            ],
          ),
          SizedBox(height: 16.h),
          Container(
            width: double.infinity,
            padding: EdgeInsets.all(16.w),
            decoration: BoxDecoration(
              color: AppTheme.backgroundDark.withOpacity(0.025),
              borderRadius: BorderRadius.circular(12.r),
            ),
            child: Text(
              content,
              style: TextStyle(
                fontSize: 15.sp,
                color: AppTheme.textMedium,
                height: 1.6,
              ),
            ),
          ),
        ],
      ),
    );
  }


  Widget _buildNavigation() {
    return Container(
      padding: EdgeInsets.symmetric(horizontal: 16.w, vertical: 12.h),
      child: Row(
        children: [
          if (currentPage > 0)
            Expanded(
              child: GestureDetector(
                onTap: _previousPage,
                child: Container(
                  height: 44.h,
                  decoration: BoxDecoration(
                    color: AppTheme.cardBackground,
                    borderRadius: BorderRadius.circular(12.r),
                    border: Border.all(
                      color: AppTheme.textMedium.withOpacity(0.2),
                    ),
                  ),
                  child: Center(
                    child: Text(
                      'Previous',
                      style: TextStyle(
                        fontSize: 15.sp,
                        fontWeight: FontWeight.w600,
                        color: AppTheme.textMedium,
                      ),
                    ),
                  ),
                ),
              ),
            ),
          if (currentPage > 0) SizedBox(width: 12.w),
          Expanded(
            flex: currentPage == 0 ? 1 : 1,
            child: GestureDetector(
              onTap: isSaving ? null : (currentPage < 4 ? _nextPage : _saveEntry),
              child: Container(
                height: 44.h,
                decoration: BoxDecoration(
                  gradient: LinearGradient(
                    colors: AppTheme.journalGradient,
                  ),
                  borderRadius: BorderRadius.circular(12.r),
                ),
                child: Center(
                  child: isSaving && currentPage == 4
                      ? Row(
                          mainAxisSize: MainAxisSize.min,
                          children: [
                            CompactStylishLoading(
                              color: AppTheme.white,
                              size: 16.w,
                            ),
                            SizedBox(width: 8.w),
                            Text(
                              isEditingExistingEntry ? 'Updating...' : 'Saving...',
                              style: TextStyle(
                                fontSize: 15.sp,
                                fontWeight: FontWeight.w600,
                                color: AppTheme.white,
                              ),
                            ),
                          ],
                        )
                      : Text(
                          currentPage < 4 ? 'Next' : (isEditingExistingEntry ? 'Update Entry' : 'Save Entry'),
                          style: TextStyle(
                            fontSize: 15.sp,
                            fontWeight: FontWeight.w600,
                            color: AppTheme.white,
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

  Future<void> _saveEntry() async {
    setState(() {
      isSaving = true;
    });

    try {
      print('📖 Saving journal entry...');
      print('   Is Editing Existing: $isEditingExistingEntry');
      print('   Mood: ${selectedMood.displayName}');
      print('   Gratitudes: ${gratitudeItems.where((item) => item.isNotEmpty).length}');
      print('   Has Reflection: ${reflection.isNotEmpty}');
      print('   Has Intention: ${intention.isNotEmpty}');

      // Clean gratitudes and ensure we have at least some content
      final cleanGratitudes = gratitudeItems.map((g) => g.trim()).where((g) => g.isNotEmpty).toList();
      
      // If no gratitudes provided, show a helpful error
      if (cleanGratitudes.isEmpty) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Please add at least one thing you\'re grateful for'),
            backgroundColor: Colors.orange,
            behavior: SnackBarBehavior.floating,
          ),
        );
        return;
      }

      if (isEditingExistingEntry && existingEntry != null) {
        // Update existing journal entry
        final existingId = int.tryParse(existingEntry!.id);
        if (existingId == null) {
          throw Exception('Invalid existing journal ID');
        }

        final result = await _journalService.updateDailyJournal(
          journalId: existingId,
          journalDate: DateTime.now(),
          moodTypeId: _journalService.mapMoodToApiMoodTypeId(selectedMood.index), // Use correct backend enum mapping
          gratitude1: gratitudeItems.isNotEmpty ? gratitudeItems[0].trim() : null,
          gratitude2: gratitudeItems.length > 1 ? gratitudeItems[1].trim() : null,
          gratitude3: gratitudeItems.length > 2 ? gratitudeItems[2].trim() : null,
          gratitude4: gratitudeItems.length > 3 ? gratitudeItems[3].trim() : null,
          gratitude5: gratitudeItems.length > 4 ? gratitudeItems[4].trim() : null,
          reflection: reflection.trim().isEmpty ? null : reflection.trim(),
          tomorrowsIntentions: intention.trim().isEmpty ? null : intention.trim(),
        );

        if (result.isSuccess) {
          print('📖 Journal entry updated successfully!');
          await _clearDraftProgress(); // Clear draft since entry is saved
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(
              content: Text('Journal entry updated successfully! 📖'),
              backgroundColor: Colors.green,
              behavior: SnackBarBehavior.floating,
            ),
          );
          Navigator.pop(context);
        } else {
          throw Exception(result.message);
        }
      } else {
        // Create new journal entry
        final result = await _journalService.createDailyJournal(
          journalDate: DateTime.now(),
          moodTypeId: _journalService.mapMoodToApiMoodTypeId(selectedMood.index), // Use correct backend enum mapping
          gratitude1: gratitudeItems.isNotEmpty ? gratitudeItems[0].trim() : null,
          gratitude2: gratitudeItems.length > 1 ? gratitudeItems[1].trim() : null,
          gratitude3: gratitudeItems.length > 2 ? gratitudeItems[2].trim() : null,
          gratitude4: gratitudeItems.length > 3 ? gratitudeItems[3].trim() : null,
          gratitude5: gratitudeItems.length > 4 ? gratitudeItems[4].trim() : null,
          reflection: reflection.trim().isEmpty ? null : reflection.trim(),
          tomorrowsIntentions: intention.trim().isEmpty ? null : intention.trim(),
        );

        if (result.isSuccess) {
          print('📖 Journal entry created successfully!');
          await _clearDraftProgress(); // Clear draft since entry is saved
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(
              content: Text('Journal entry saved successfully! 📖'),
              backgroundColor: Colors.green,
              behavior: SnackBarBehavior.floating,
            ),
          );
          Navigator.pop(context);
        } else {
          print('📖 Journal creation failed: ${result.message}');
          print('📖 Running parameter tests to identify constraint issue...');
          // Run test to identify the specific constraint issue
          await _journalService.testJournalCreationParameters();
          throw Exception(result.message);
        }
      }
    } catch (e) {
      print('📖 Error saving journal entry: $e');
      
      // Show user-friendly error message
      String errorMessage = 'Failed to save journal entry';
      if (e.toString().contains('network') || e.toString().contains('connection')) {
        errorMessage = 'Network error - please check your connection';
      } else if (e.toString().contains('timeout')) {
        errorMessage = 'Request timed out - please try again';
      } else if (e.toString().contains('authentication') || e.toString().contains('auth')) {
        errorMessage = 'Authentication error - please log in again';
      } else if (e.toString().contains('Unexpected error when creating Daily Journal') || 
                 e.toString().contains('Npgsql')) {
        errorMessage = 'Journal feature is temporarily unavailable. The server team has been notified.';
      }
      
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text(errorMessage),
          backgroundColor: Colors.red,
          behavior: SnackBarBehavior.floating,
          duration: Duration(seconds: 4),
        ),
      );
    } finally {
      setState(() {
        isSaving = false;
      });
    }
  }

  Future<void> _checkForExistingEntry() async {
    setState(() {
      isLoading = true;
    });

    try {
      final today = DateTime.now();
      print('📖 Checking for existing journal entry for today: ${today.toString()}');
      
      // Check for existing journal entry for today
      final result = await _journalService.getDailyJournalForDate(today);
      
      if (result.isSuccess && result.data != null) {
        // Found existing entry - load it for editing
        print('📖 Found existing journal entry for today');
        
        final apiEntry = result.data!;
        final journalEntry = JournalEntry.fromApiModel(apiEntry);
        
        setState(() {
          existingEntry = journalEntry;
          isEditingExistingEntry = true;
          selectedMood = journalEntry.mood;
          
          // Load gratitudes (expand to 5 entries)
          gratitudeItems = List<String>.filled(5, '');
          for (int i = 0; i < journalEntry.gratitudes.length && i < 5; i++) {
            gratitudeItems[i] = journalEntry.gratitudes[i];
          }
          
          reflection = journalEntry.reflection;
          intention = journalEntry.intention;
          _reflectionController.text = reflection;
        });
        
        print('📖 Loaded existing entry: ${journalEntry.gratitudes.length} gratitudes, mood: ${journalEntry.mood.displayName}');
      } else {
        // No existing entry found - start with new entry
        print('📖 No existing journal entry found for today - starting new entry');
        setState(() {
          isEditingExistingEntry = false;
          existingEntry = null;
        });
      }
    } catch (e) {
      print('📖 Error checking for existing entry: $e');
      // Continue with new entry creation on error
      setState(() {
        isEditingExistingEntry = false;
        existingEntry = null;
      });
      
      // Show error to user
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Unable to load existing journal entry'),
          backgroundColor: Colors.orange,
          behavior: SnackBarBehavior.floating,
        ),
      );
    } finally {
      setState(() {
        isLoading = false;
      });
    }
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

  Future<void> _loadEntryForEdit(String entryId) async {
    setState(() {
      isLoading = true;
    });

    try {
      // TODO: Implement journal entry loading by ID with generated API
      // This is a placeholder implementation as journal endpoints are not yet available
      // in the generated API
      
      try {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Journal API not yet implemented. Cannot load entry.'),
            backgroundColor: Colors.orange,
          ),
        );
      } catch (e) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Failed to load journal entry: ${e.toString()}'),
            backgroundColor: Colors.red,
          ),
        );
      }
    } catch (e) {
      print('Error loading entry for edit: $e');
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Error loading journal entry: $e'),
          backgroundColor: Colors.red,
        ),
      );
    } finally {
      setState(() {
        isLoading = false;
      });
    }
  }

  String _formatDate(DateTime date) {
    final months = [
      'January', 'February', 'March', 'April', 'May', 'June',
      'July', 'August', 'September', 'October', 'November', 'December'
    ];
    return '${months[date.month - 1]} ${date.day}, ${date.year}';
  }

  // Handle back button tap in header
  void _handleBackButton() async {
    final shouldPop = await _onWillPop();
    if (shouldPop) {
      Navigator.pop(context);
    }
  }

  // Check if user has unsaved changes
  bool _hasUnsavedChanges() {
    print('📖 Checking for unsaved changes...');
    print('   isEditingExistingEntry: $isEditingExistingEntry');
    print('   selectedMood: ${selectedMood.displayName}');
    print('   gratitudeItems: ${gratitudeItems.where((item) => item.trim().isNotEmpty).toList()}');
    print('   reflection: "${reflection.trim()}"');
    print('   intention: "${intention.trim()}"');
    
    // If we're editing an existing entry, compare with original values
    if (isEditingExistingEntry && existingEntry != null) {
      print('   Comparing with existing entry...');
      // Check if mood changed
      if (selectedMood != existingEntry!.mood) {
        print('   → Mood changed: ${existingEntry!.mood.displayName} → ${selectedMood.displayName}');
        return true;
      }
      
      // Check if gratitudes changed
      final originalGratitudes = List<String>.filled(5, '');
      for (int i = 0; i < existingEntry!.gratitudes.length && i < 5; i++) {
        originalGratitudes[i] = existingEntry!.gratitudes[i];
      }
      for (int i = 0; i < 5; i++) {
        if (gratitudeItems[i].trim() != originalGratitudes[i].trim()) {
          print('   → Gratitude $i changed: "${originalGratitudes[i]}" → "${gratitudeItems[i].trim()}"');
          return true;
        }
      }
      
      // Check if reflection changed
      if (reflection.trim() != existingEntry!.reflection.trim()) {
        print('   → Reflection changed');
        return true;
      }
      
      // Check if intention changed
      if (intention.trim() != existingEntry!.intention.trim()) {
        print('   → Intention changed');
        return true;
      }
      
      print('   → No changes detected in existing entry');
      return false;
    } else {
      print('   Checking new entry for changes...');
      // For new entries, check if any data has been entered
      // Check if mood is not neutral (default)
      if (selectedMood != Mood.neutral) {
        print('   → Mood is not neutral: ${selectedMood.displayName}');
        return true;
      }
      
      // Check if any gratitude items have content
      if (gratitudeItems.any((item) => item.trim().isNotEmpty)) {
        print('   → Has gratitude items');
        return true;
      }
      
      // Check if reflection has content
      if (reflection.trim().isNotEmpty) {
        print('   → Has reflection content');
        return true;
      }
      
      // Check if intention has content
      if (intention.trim().isNotEmpty) {
        print('   → Has intention content');
        return true;
      }
      
      print('   → No changes detected in new entry');
      return false;
    }
  }

  // Handle will pop (back gesture or back button)
  Future<bool> _onWillPop() async {
    // If we're on the summary page or saving, allow normal back navigation
    if (currentPage == 4 || isSaving) {
      return true;
    }
    
    // If editing existing entry, allow normal back navigation without dialog
    if (isEditingExistingEntry) {
      return true;
    }

    // Check if user has unsaved changes
    if (!_hasUnsavedChanges()) {
      return true; // No changes, allow back navigation
    }

    // Show save progress dialog
    return await _showSaveProgressDialog() ?? false;
  }

  // Show dialog asking user if they want to save as draft
  Future<void> _showSaveDraftDialog() async {
    return showDialog<void>(
      context: context,
      barrierDismissible: false,
      builder: (BuildContext context) {
        return AlertDialog(
          backgroundColor: AppTheme.cardBackground,
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(20.r),
          ),
          title: Row(
            children: [
              Container(
                padding: EdgeInsets.all(8.w),
                decoration: BoxDecoration(
                  gradient: LinearGradient(
                    colors: AppTheme.journalGradient,
                  ),
                  borderRadius: BorderRadius.circular(10.r),
                ),
                child: Icon(
                  Icons.save_outlined,
                  color: Colors.white,
                  size: 20.sp,
                ),
              ),
              SizedBox(width: 12.w),
              Expanded(
                child: Text(
                  'Save as Draft?',
                  style: TextStyle(
                    fontSize: 18.sp,
                    fontWeight: FontWeight.bold,
                    color: AppTheme.textLight,
                  ),
                ),
              ),
            ],
          ),
          content: Column(
            mainAxisSize: MainAxisSize.min,
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(
                'Would you like to save your current progress as a draft? You can continue working on it later.',
                style: TextStyle(
                  fontSize: 16.sp,
                  color: AppTheme.textMedium,
                  height: 1.4,
                ),
              ),
              SizedBox(height: 16.h),
              ..._buildUnsavedChangesSummary(),
            ],
          ),
          actions: [
            // Don't Save button
            TextButton(
              onPressed: () => Navigator.of(context).pop(),
              child: Text(
                'Don\'t Save',
                style: TextStyle(
                  color: AppTheme.textMedium,
                  fontSize: 16.sp,
                ),
              ),
            ),
            // Save Draft button
            Container(
              margin: EdgeInsets.only(left: 8.w),
              child: ElevatedButton(
                onPressed: () async {
                  Navigator.of(context).pop(); // Close dialog
                  await _saveDraftProgress(true); // Save the draft with snackbar
                },
                style: ElevatedButton.styleFrom(
                  backgroundColor: AppTheme.journalGradient[0],
                  foregroundColor: Colors.white,
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(12.r),
                  ),
                  padding: EdgeInsets.symmetric(horizontal: 20.w, vertical: 12.h),
                ),
                child: Text(
                  'Save Draft',
                  style: TextStyle(
                    fontSize: 16.sp,
                    fontWeight: FontWeight.w600,
                  ),
                ),
              ),
            ),
          ],
        );
      },
    );
  }

  // Show dialog asking user what to do with unsaved changes
  Future<bool?> _showSaveProgressDialog() async {
    return showDialog<bool>(
      context: context,
      barrierDismissible: false,
      builder: (BuildContext context) {
        return AlertDialog(
          backgroundColor: AppTheme.cardBackground,
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(20.r),
          ),
          title: Row(
            children: [
              Container(
                padding: EdgeInsets.all(8.w),
                decoration: BoxDecoration(
                  gradient: LinearGradient(
                    colors: AppTheme.journalGradient,
                  ),
                  borderRadius: BorderRadius.circular(10.r),
                ),
                child: Icon(
                  Icons.save_outlined,
                  color: Colors.white,
                  size: 20.sp,
                ),
              ),
              SizedBox(width: 12.w),
              Expanded(
                child: Text(
                  'Save Your Progress?',
                  style: TextStyle(
                    fontSize: 18.sp,
                    fontWeight: FontWeight.bold,
                    color: AppTheme.textLight,
                  ),
                ),
              ),
            ],
          ),
          content: Column(
            mainAxisSize: MainAxisSize.min,
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(
                'You have unsaved changes in your journal entry:',
                style: TextStyle(
                  fontSize: 16.sp,
                  color: AppTheme.textMedium,
                  height: 1.4,
                ),
              ),
              SizedBox(height: 16.h),
              ..._buildUnsavedChangesSummary(),
              SizedBox(height: 16.h),
              Text(
                'What would you like to do?',
                style: TextStyle(
                  fontSize: 16.sp,
                  color: AppTheme.textLight,
                  fontWeight: FontWeight.w600,
                ),
              ),
            ],
          ),
          actions: [
            // Cancel button
            TextButton(
              onPressed: () => Navigator.of(context).pop(false),
              child: Text(
                'Cancel',
                style: TextStyle(
                  color: AppTheme.textMedium,
                  fontSize: 16.sp,
                ),
              ),
            ),
            // Discard button
            TextButton(
              onPressed: () => Navigator.of(context).pop(true),
              child: Text(
                'Discard',
                style: TextStyle(
                  color: Colors.red,
                  fontSize: 16.sp,
                  fontWeight: FontWeight.w600,
                ),
              ),
            ),
            // Save as Draft button
            Container(
              margin: EdgeInsets.only(left: 8.w),
              child: ElevatedButton(
                onPressed: () async {
                  // Save draft directly without showing another dialog
                  await _saveDraftProgress();
                  Navigator.of(context).pop(true); // Close dialog and allow navigation
                },
                style: ElevatedButton.styleFrom(
                  backgroundColor: AppTheme.journalGradient[0],
                  foregroundColor: Colors.white,
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(12.r),
                  ),
                  padding: EdgeInsets.symmetric(horizontal: 20.w, vertical: 12.h),
                ),
                child: Text(
                  'Save as Draft',
                  style: TextStyle(
                    fontSize: 16.sp,
                    fontWeight: FontWeight.w600,
                  ),
                ),
              ),
            ),
          ],
        );
      },
    );
  }

  // Build summary of unsaved changes for the dialog
  List<Widget> _buildUnsavedChangesSummary() {
    List<Widget> changes = [];
    
    if (isEditingExistingEntry && existingEntry != null) {
      // For existing entries, show what changed
      if (selectedMood != existingEntry!.mood) {
        changes.add(_buildChangeSummaryItem(
          icon: Icons.mood,
          text: 'Mood updated to ${selectedMood.displayName}',
          color: selectedMood.color,
        ));
      }
      
      // Check gratitude changes
      final originalGratitudes = List<String>.filled(5, '');
      for (int i = 0; i < existingEntry!.gratitudes.length && i < 5; i++) {
        originalGratitudes[i] = existingEntry!.gratitudes[i];
      }
      bool gratitudesChanged = false;
      for (int i = 0; i < 5; i++) {
        if (gratitudeItems[i].trim() != originalGratitudes[i].trim()) {
          gratitudesChanged = true;
          break;
        }
      }
      if (gratitudesChanged) {
        changes.add(_buildChangeSummaryItem(
          icon: Icons.favorite,
          text: 'Gratitude entries modified',
          color: AppTheme.affirmationGradient[0],
        ));
      }
      
      if (reflection.trim() != existingEntry!.reflection.trim()) {
        changes.add(_buildChangeSummaryItem(
          icon: Icons.auto_stories,
          text: reflection.trim().isEmpty ? 'Reflection removed' : 'Reflection updated',
          color: AppTheme.accentGold,
        ));
      }
      
      if (intention.trim() != existingEntry!.intention.trim()) {
        changes.add(_buildChangeSummaryItem(
          icon: Icons.wb_sunny,
          text: intention.trim().isEmpty ? 'Intention removed' : 'Tomorrow\'s intention updated',
          color: AppTheme.goalTrackerGradient[0],
        ));
      }
    } else {
      // For new entries, show what's been entered
      if (selectedMood != Mood.neutral) {
        changes.add(_buildChangeSummaryItem(
          icon: Icons.mood,
          text: 'Mood: ${selectedMood.displayName}',
          color: selectedMood.color,
        ));
      }
      
      final gratitudeCount = gratitudeItems.where((item) => item.trim().isNotEmpty).length;
      if (gratitudeCount > 0) {
        changes.add(_buildChangeSummaryItem(
          icon: Icons.favorite,
          text: '$gratitudeCount gratitude ${gratitudeCount == 1 ? 'entry' : 'entries'}',
          color: AppTheme.affirmationGradient[0],
        ));
      }
      
      if (reflection.trim().isNotEmpty) {
        changes.add(_buildChangeSummaryItem(
          icon: Icons.auto_stories,
          text: 'Daily reflection written',
          color: AppTheme.accentGold,
        ));
      }
      
      if (intention.trim().isNotEmpty) {
        changes.add(_buildChangeSummaryItem(
          icon: Icons.wb_sunny,
          text: 'Tomorrow\'s intention set',
          color: AppTheme.goalTrackerGradient[0],
        ));
      }
    }
    
    return changes;
  }

  // Build individual change summary item
  Widget _buildChangeSummaryItem({
    required IconData icon,
    required String text,
    required Color color,
  }) {
    return Padding(
      padding: EdgeInsets.only(bottom: 8.h),
      child: Row(
        children: [
          Container(
            padding: EdgeInsets.all(6.w),
            decoration: BoxDecoration(
              color: color.withOpacity(0.2),
              borderRadius: BorderRadius.circular(8.r),
            ),
            child: Icon(
              icon,
              color: color,
              size: 16.sp,
            ),
          ),
          SizedBox(width: 12.w),
          Expanded(
            child: Text(
              text,
              style: TextStyle(
                fontSize: 14.sp,
                color: AppTheme.textMedium,
                height: 1.3,
              ),
            ),
          ),
        ],
      ),
    );
  }


  // Load existing draft progress
  Future<void> _loadDraftProgress() async {
    try {
      final prefs = await SharedPreferences.getInstance();
      final today = DateTime.now();
      final dateKey = '${today.year}-${today.month.toString().padLeft(2, '0')}-${today.day.toString().padLeft(2, '0')}';
      
      final draftJson = prefs.getString('journal_draft_$dateKey');
      if (draftJson != null) {
        final draftData = jsonDecode(draftJson) as Map<String, dynamic>;
        
        print('📖 Loading draft progress...');
        print('   Draft data found: ${draftData.keys}');
        
        setState(() {
          // Load mood (find by display name)
          final moodName = draftData['mood'] as String?;
          if (moodName != null) {
            final mood = Mood.values.firstWhere(
              (m) => m.displayName == moodName,
              orElse: () => Mood.neutral,
            );
            selectedMood = mood;
          }
          
          // Load gratitudes
          final draftGratitudes = draftData['gratitudes'] as List<dynamic>?;
          if (draftGratitudes != null) {
            gratitudeItems = List<String>.filled(5, '');
            for (int i = 0; i < draftGratitudes.length && i < 5; i++) {
              gratitudeItems[i] = draftGratitudes[i].toString();
            }
          }
          
          // Load reflection
          final draftReflection = draftData['reflection'] as String?;
          if (draftReflection != null) {
            reflection = draftReflection;
            _reflectionController.text = reflection;
          }
          
          // Load intention
          final draftIntention = draftData['intention'] as String?;
          if (draftIntention != null) {
            intention = draftIntention;
          }
        });
        
        print('📖 Draft progress loaded successfully');
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Row(
              children: [
                Icon(Icons.restore, color: Colors.white, size: 20.sp),
                SizedBox(width: 8.w),
                Text('Draft progress restored'),
              ],
            ),
            backgroundColor: AppTheme.journalGradient[0],
            behavior: SnackBarBehavior.floating,
            duration: Duration(seconds: 2),
          ),
        );
      }
    } catch (e) {
      print('📖 Error loading draft progress: $e');
      // Continue silently without draft
    }
  }

  // Clear draft progress (when entry is saved or no longer needed)
  Future<void> _clearDraftProgress() async {
    try {
      final prefs = await SharedPreferences.getInstance();
      final today = DateTime.now();
      final dateKey = '${today.year}-${today.month.toString().padLeft(2, '0')}-${today.day.toString().padLeft(2, '0')}';
      
      await prefs.remove('journal_draft_$dateKey');
      print('📖 Draft progress cleared');
    } catch (e) {
      print('📖 Error clearing draft progress: $e');
    }
  }

  // Save current progress as draft without creating a finalized journal entry
  Future<bool> _saveDraftProgress([bool showSnackBar = false]) async {
    try {
      print('📖 Saving draft progress...');
      
      // Save draft data to local storage/shared preferences
      final prefs = await SharedPreferences.getInstance();
      final today = DateTime.now();
      final dateKey = '${today.year}-${today.month.toString().padLeft(2, '0')}-${today.day.toString().padLeft(2, '0')}';
      
      // Create draft data map
      final draftData = {
        'mood': selectedMood.displayName,
        'gratitudes': gratitudeItems.where((item) => item.trim().isNotEmpty).toList(),
        'reflection': reflection.trim(),
        'intention': intention.trim(),
        'lastSaved': DateTime.now().toIso8601String(),
      };
      
      // Save as JSON string
      await prefs.setString('journal_draft_$dateKey', jsonEncode(draftData));
      
      print('📖 Draft progress saved successfully');
      print('   Mood: ${selectedMood.displayName}');
      print('   Gratitudes: ${draftData['gratitudes']}');
      print('   Has Reflection: ${reflection.trim().isNotEmpty}');
      print('   Has Intention: ${intention.trim().isNotEmpty}');
      
      // Show confirmation only if requested and context is valid
      if (showSnackBar && mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Row(
              children: [
                Icon(Icons.save_outlined, color: Colors.white, size: 20.sp),
                SizedBox(width: 8.w),
                Text('Progress saved as draft'),
              ],
            ),
            backgroundColor: AppTheme.journalGradient[0],
            behavior: SnackBarBehavior.floating,
            duration: Duration(seconds: 2),
          ),
        );
      }
      
      return true;
    } catch (e) {
      print('📖 Error saving draft progress: $e');
      if (showSnackBar && mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Failed to save progress: ${e.toString()}'),
            backgroundColor: Colors.red,
            behavior: SnackBarBehavior.floating,
          ),
        );
      }
      return false;
    }
  }
}