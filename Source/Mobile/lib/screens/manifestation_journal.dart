import 'dart:async';
import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:mistel/components/gratitude_selection.dart';
import 'package:mistel/models/journey_entry.dart';
import 'package:mistel/constants/colors.dart';
import 'package:mistel/constants/app_theme.dart';
import 'package:mistel/models/mood_selector.dart';
import 'package:mistel/screens/calendar_view_screen.dart';
import 'package:mistel/services/daily_journal_service.dart';
import 'package:mistel/services/guided_tour_service.dart';
import 'package:mistel/widgets/stylish_loading.dart';
import 'package:mistel/widgets/keyboard_dismissal_wrapper.dart';
import 'package:mistel/utils/navigation_utils.dart';
import 'package:mistel/widgets/universal_back_button.dart';

class ManifestationJournalScreen extends StatefulWidget {
  // Enhanced with manual draft saving functionality - debug exit dialog
  @override
  _ManifestationJournalScreenState createState() => _ManifestationJournalScreenState();
}

class _ManifestationJournalScreenState extends State<ManifestationJournalScreen> {
  final PageController _pageController = PageController();
  final TextEditingController _reflectionController = TextEditingController();
  final TextEditingController _intentionController = TextEditingController();
  final DailyJournalService _journalService = DailyJournalService();
  
  // Debounce timers to prevent excessive setState calls during typing
  Timer? _reflectionDebounceTimer;
  Timer? _intentionDebounceTimer;
  
  int currentPage = 0;
  double scrollPosition = 0.0; // Track partial scroll position
  Mood selectedMood = Mood.neutral;
  List<String> gratitudeItems = ['', '', '', '', '']; // Expanded to 5 gratitudes
  String reflection = '';
  String intention = '';
  bool isEditingExistingEntry = false;
  JournalEntry? existingEntry;
  bool isLoading = false;
  bool isSaving = false;
  
  // Track what the user has actually changed during this session
  bool _userHasInteracted = false;
  bool _moodChanged = false;
  bool _gratitudeChanged = false;
  bool _reflectionChanged = false;
  bool _intentionChanged = false;
  
  // Dynamic default state - updates after each save to become the new baseline
  Mood _baselineMood = Mood.neutral;
  List<String> _baselineGratitudes = ['', '', '', '', ''];
  String _baselineReflection = '';
  String _baselineIntention = '';
  
  // Track loaded state to differentiate between user changes and loaded values
  bool _hasLoadedDraft = false;
  
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
    
    // Add scroll listener to track partial scroll positions
    _pageController.addListener(_onPageScroll);
    
    // Add debounced listeners to text controllers to sync with state
    _reflectionController.addListener(() {
      if (_reflectionController.text != reflection) {
        // Cancel previous timer
        _reflectionDebounceTimer?.cancel();
        
        // Set new timer with 300ms delay
        _reflectionDebounceTimer = Timer(const Duration(milliseconds: 300), () {
          if (mounted && _reflectionController.text != reflection) {
            setState(() {
              reflection = _reflectionController.text;
              _userHasInteracted = true;
              _reflectionChanged = true;
              print('📖 REFLECTION CHANGED: "${_reflectionController.text}", _userHasInteracted: $_userHasInteracted');
            });
            _updateDraftProgress();
          }
        });
      }
    });
    
    _intentionController.addListener(() {
      if (_intentionController.text != intention) {
        // Cancel previous timer
        _intentionDebounceTimer?.cancel();
        
        // Set new timer with 300ms delay
        _intentionDebounceTimer = Timer(const Duration(milliseconds: 300), () {
          if (mounted && _intentionController.text != intention) {
            setState(() {
              intention = _intentionController.text;
              _userHasInteracted = true;
              _intentionChanged = true;
            });
            _updateDraftProgress();
          }
        });
      }
    });
    
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

  void _onPageScroll() {
    if (_pageController.hasClients && _pageController.page != null) {
      final page = _pageController.page!;
      // Clamp the page value to ensure it's within valid bounds
      final clampedPage = page.clamp(0.0, 4.0);
      
      setState(() {
        scrollPosition = clampedPage;
        currentPage = clampedPage.round();
      });
    }
  }

  @override
  void dispose() {
    // Cancel debounce timers to prevent memory leaks
    _reflectionDebounceTimer?.cancel();
    _intentionDebounceTimer?.cancel();
    
    _pageController.removeListener(_onPageScroll);
    _pageController.dispose();
    _reflectionController.dispose();
    _intentionController.dispose();
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
            child: UniversalBackButton(),
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
                color: AppTheme.textMedium.withOpacity(0.2),
                borderRadius: BorderRadius.circular(2.r),
              ),
              child: LayoutBuilder(
                builder: (context, constraints) {
                  // Get real-time scroll position directly from controller
                  final realTimePosition = _pageController.hasClients 
                    ? (_pageController.page ?? 0.0).clamp(0.0, 4.0)
                    : 0.0;
                  
                  return Stack(
                    children: [
                      // Flowing progress overlay for this segment
                      Container(
                        height: 3.h,
                        width: _calculateSegmentWidth(index, constraints.maxWidth, realTimePosition),
                        decoration: BoxDecoration(
                          color: AppTheme.journalGradient[0],
                          borderRadius: BorderRadius.circular(2.r),
                        ),
                      ),
                    ],
                  );
                },
              ),
            ),
          );
        }),
      ),
    );
  }
  
  double _calculateSegmentWidth(int segmentIndex, double maxWidth, double position) {
    // Calculate what percentage of this segment should be filled based on scroll position
    if (position <= segmentIndex) {
      // Haven't reached this segment yet
      return 0.0;
    } else if (position >= segmentIndex + 1) {
      // This segment is fully completed
      return maxWidth;
    } else {
      // Partially filled segment - fill from left to right
      final segmentProgress = position - segmentIndex;
      return maxWidth * segmentProgress.clamp(0.0, 1.0);
    }
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
                  _userHasInteracted = true;
                  _moodChanged = true;
                  print('📖 MOOD CHANGED: $mood, _userHasInteracted: $_userHasInteracted');
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
                  _userHasInteracted = true;
                  _gratitudeChanged = true;
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
                    _userHasInteracted = true;
                    _reflectionChanged = true;
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
                    _userHasInteracted = true;
                    _intentionChanged = true;
                  });
                },
                controller: _intentionController,
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
              onTap: isSaving ? null : (currentPage < 4 ? _nextPage : _completeJournal),
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
                              'Completing...',
                              style: TextStyle(
                                fontSize: 15.sp,
                                fontWeight: FontWeight.w600,
                                color: AppTheme.white,
                              ),
                            ),
                          ],
                        )
                      : Text(
                          currentPage < 4 ? 'Next' : (isEditingExistingEntry ? 'Complete' : 'Complete'),
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

  Future<void> _completeJournal() async {
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
          // Reset all interaction flags since entry is saved
          _userHasInteracted = false;
          _moodChanged = false;
          _gratitudeChanged = false;
          _reflectionChanged = false;
          _intentionChanged = false;
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
          // Reset all interaction flags since entry is saved
          _userHasInteracted = false;
          _moodChanged = false;
          _gratitudeChanged = false;
          _reflectionChanged = false;
          _intentionChanged = false;
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
          _intentionController.text = intention;
          
          // Reset interaction flags when loading existing entry
          _userHasInteracted = false;
          _moodChanged = false;
          _gratitudeChanged = false;
          _reflectionChanged = false;
          _intentionChanged = false;
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
    final result = await pushWithoutSwipe(
      context,
      CalendarViewScreen(),
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

  // Check if user has made changes AND values differ from baseline
  bool _hasUnsavedChanges() {
    print('🔍 _hasUnsavedChanges check:');
    print('   _userHasInteracted: $_userHasInteracted');
    print('   selectedMood: $selectedMood (baseline: $_baselineMood)');
    print('   reflection: "$reflection" (baseline: "$_baselineReflection")');
    print('   intention: "$intention" (baseline: "$_baselineIntention")');
    
    // Must have user interaction
    if (!_userHasInteracted) {
      print('   → No user interaction, returning false');
      return false;
    }
    
    // Check if current state is same as original defaults - if so, no need to save
    bool isAtOriginalDefaults = (
      selectedMood == Mood.neutral &&
      gratitudeItems.every((item) => item.trim().isEmpty) &&
      reflection.trim().isEmpty &&
      intention.trim().isEmpty
    );
    
    if (isAtOriginalDefaults) {
      print('   → Current state matches original defaults, no need to save, returning false');
      return false;
    }
    
    // Check if any value differs from baseline
    if (selectedMood != _baselineMood) {
      print('   → Mood changed from baseline, returning true');
      return true;
    }
    
    // Check gratitudes
    for (int i = 0; i < gratitudeItems.length; i++) {
      if (gratitudeItems[i].trim() != _baselineGratitudes[i]) {
        print('   → Gratitude $i changed from baseline: "${gratitudeItems[i]}" vs "${_baselineGratitudes[i]}", returning true');
        return true;
      }
    }
    
    // Check text fields
    if (reflection.trim() != _baselineReflection) {
      print('   → Reflection changed from baseline, returning true');
      return true;
    }
    if (intention.trim() != _baselineIntention) {
      print('   → Intention changed from baseline, returning true');
      return true;
    }
    
    print('   → No changes detected from baseline, returning false');
    return false;
  }
  
  // Reset everything to original defaults (for complete reset)
  void _revertToDefaults() {
    setState(() {
      selectedMood = Mood.neutral;
      gratitudeItems = ['', '', '', '', ''];
      reflection = '';
      intention = '';
      
      // Update text controllers
      _reflectionController.text = '';
      _intentionController.text = '';
      
      // Reset baseline to original defaults
      _baselineMood = Mood.neutral;
      _baselineGratitudes = ['', '', '', '', ''];
      _baselineReflection = '';
      _baselineIntention = '';
      
      // Reset all flags
      _userHasInteracted = false;
      _moodChanged = false;
      _gratitudeChanged = false;
      _reflectionChanged = false;
      _intentionChanged = false;
      _hasLoadedDraft = false;
    });
    
    // Clear any saved draft
    _clearDraftProgress();
  }
  
  // Revert to baseline (for discarding unsaved changes)
  void _revertToBaseline() {
    setState(() {
      selectedMood = _baselineMood;
      gratitudeItems = List.from(_baselineGratitudes);
      reflection = _baselineReflection;
      intention = _baselineIntention;
      
      // Update text controllers
      _reflectionController.text = _baselineReflection;
      _intentionController.text = _baselineIntention;
      
      // Reset interaction flags - we're back to the baseline state
      _userHasInteracted = false;
      _moodChanged = false;
      _gratitudeChanged = false;
      _reflectionChanged = false;
      _intentionChanged = false;
    });
  }

  // Handle will pop (back gesture or back button)
  Future<bool> _onWillPop() async {
    print('🔍 _onWillPop called - currentPage: $currentPage, isSaving: $isSaving');
    print('   Current values:');
    print('     selectedMood: $selectedMood');
    print('     gratitudeItems: $gratitudeItems');
    print('     reflection: "$reflection"');
    print('     intention: "$intention"');
    print('   Baseline values:');
    print('     _baselineMood: $_baselineMood');
    print('     _baselineGratitudes: $_baselineGratitudes');
    print('     _baselineReflection: "$_baselineReflection"');
    print('     _baselineIntention: "$_baselineIntention"');
    
    // If we're on the summary page or saving, allow normal back navigation
    if (currentPage == 4 || isSaving) {
      print('   → Allowing navigation (summary page or saving)');
      return true;
    }
    
    // Check if user has unsaved changes
    final hasChanges = _hasUnsavedChanges();
    print('   → _hasUnsavedChanges returned: $hasChanges');
    
    if (!hasChanges) {
      print('   → No changes detected, returning TRUE to allow navigation');
      return true; // No changes, allow back navigation
    }

    print('   → Changes detected, showing save dialog');
    // Show save progress dialog
    final result = await _showSaveProgressDialog();
    print('   → Save dialog result: $result');
    return result ?? false;
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
        return Dialog(
          backgroundColor: Colors.transparent,
          child: Container(
            width: double.infinity,
            margin: EdgeInsets.symmetric(horizontal: 20.w),
            decoration: BoxDecoration(
              color: AppTheme.cardBackground,
              borderRadius: BorderRadius.circular(24.r),
              boxShadow: [
                BoxShadow(
                  color: AppTheme.shadow,
                  blurRadius: 20.r,
                  offset: Offset(0, 10.h),
                ),
              ],
            ),
            child: Padding(
              padding: EdgeInsets.all(24.w),
              child: Column(
                mainAxisSize: MainAxisSize.min,
                children: [
                  // Header with icon
                  Container(
                    width: 64.w,
                    height: 64.h,
                    decoration: BoxDecoration(
                      gradient: LinearGradient(
                        colors: AppTheme.journalGradient,
                      ),
                      shape: BoxShape.circle,
                    ),
                    child: Icon(
                      Icons.edit_note_rounded,
                      color: Colors.white,
                      size: 32.sp,
                    ),
                  ),
                  
                  SizedBox(height: 20.h),
                  
                  // Title
                  Text(
                    'Unsaved Changes',
                    style: TextStyle(
                      fontSize: 22.sp,
                      fontWeight: FontWeight.bold,
                      color: AppTheme.textPrimary,
                    ),
                  ),
                  
                  SizedBox(height: 12.h),
                  
                  // Description
                  Text(
                    'You have progress in your journal that hasn\'t been saved yet.',
                    textAlign: TextAlign.center,
                    style: TextStyle(
                      fontSize: 16.sp,
                      color: AppTheme.textSecondary,
                      height: 1.5,
                    ),
                  ),
                  
                  SizedBox(height: 24.h),
                  
                  // Changes summary in a card
                  Container(
                    width: double.infinity,
                    padding: EdgeInsets.all(16.w),
                    decoration: BoxDecoration(
                      color: AppTheme.backgroundSecondary,
                      borderRadius: BorderRadius.circular(16.r),
                      border: Border.all(
                        color: AppTheme.borderSecondary,
                      ),
                    ),
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Row(
                          children: [
                            Icon(
                              Icons.pending_actions,
                              color: AppTheme.journalGradient[0],
                              size: 18.sp,
                            ),
                            SizedBox(width: 8.w),
                            Text(
                              'Your Progress',
                              style: TextStyle(
                                fontSize: 14.sp,
                                fontWeight: FontWeight.w600,
                                color: AppTheme.textPrimary,
                              ),
                            ),
                          ],
                        ),
                        SizedBox(height: 12.h),
                        ..._buildUnsavedChangesSummary(),
                      ],
                    ),
                  ),
                  
                  SizedBox(height: 32.h),
                  
                  // Action buttons
                  Column(
                    children: [
                      // Save as Draft button (primary action)
                      Container(
                        width: double.infinity,
                        height: 48.h,
                        child: ElevatedButton(
                          onPressed: () async {
                            await _saveDraftProgress();
                            Navigator.of(context).pop(true);
                          },
                          style: ElevatedButton.styleFrom(
                            backgroundColor: AppTheme.journalGradient[0],
                            foregroundColor: Colors.white,
                            elevation: 0,
                            shape: RoundedRectangleBorder(
                              borderRadius: BorderRadius.circular(16.r),
                            ),
                          ),
                          child: Row(
                            mainAxisAlignment: MainAxisAlignment.center,
                            children: [
                              Icon(
                                Icons.save_rounded,
                                size: 20.sp,
                              ),
                              SizedBox(width: 8.w),
                              Text(
                                'Save as Draft',
                                style: TextStyle(
                                  fontSize: 16.sp,
                                  fontWeight: FontWeight.w600,
                                ),
                              ),
                            ],
                          ),
                        ),
                      ),
                      
                      SizedBox(height: 12.h),
                      
                      // Secondary actions row
                      Row(
                        children: [
                          // Cancel button
                          Expanded(
                            child: Container(
                              height: 44.h,
                              child: TextButton(
                                onPressed: () => Navigator.of(context).pop(false),
                                style: TextButton.styleFrom(
                                  backgroundColor: AppTheme.backgroundSecondary,
                                  shape: RoundedRectangleBorder(
                                    borderRadius: BorderRadius.circular(12.r),
                                  ),
                                ),
                                child: Text(
                                  'Cancel',
                                  style: TextStyle(
                                    color: AppTheme.textSecondary,
                                    fontSize: 15.sp,
                                    fontWeight: FontWeight.w600,
                                  ),
                                ),
                              ),
                            ),
                          ),
                          
                          SizedBox(width: 12.w),
                          
                          // Discard button
                          Expanded(
                            child: Container(
                              height: 44.h,
                              child: TextButton(
                                onPressed: () {
                                  _revertToBaseline();
                                  Navigator.of(context).pop(true);
                                },
                                style: TextButton.styleFrom(
                                  backgroundColor: Colors.red.withOpacity(0.1),
                                  shape: RoundedRectangleBorder(
                                    borderRadius: BorderRadius.circular(12.r),
                                  ),
                                ),
                                child: Text(
                                  'Discard',
                                  style: TextStyle(
                                    color: Colors.red,
                                    fontSize: 15.sp,
                                    fontWeight: FontWeight.w600,
                                  ),
                                ),
                              ),
                            ),
                          ),
                        ],
                      ),
                    ],
                  ),
                ],
              ),
            ),
          ),
        );
      },
    );
  }

  // Build summary of unsaved changes (categories only, not details)
  List<Widget> _buildUnsavedChangesSummary() {
    List<Widget> changes = [];
    
    // Compare against baseline values (what was last saved), not original defaults
    if (selectedMood != _baselineMood) {
      changes.add(_buildChangeSummaryItem(
        icon: Icons.mood,
        text: 'Mood',
        color: selectedMood.color,
      ));
    }
    
    // Check gratitude changes against baseline
    bool gratitudesChanged = false;
    for (int i = 0; i < 5; i++) {
      if (gratitudeItems[i].trim() != _baselineGratitudes[i].trim()) {
        gratitudesChanged = true;
        break;
      }
    }
    if (gratitudesChanged) {
      changes.add(_buildChangeSummaryItem(
        icon: Icons.favorite,
        text: 'Gratitudes',
        color: AppTheme.affirmationGradient[0],
      ));
    }
    
    // Check reflection changes against baseline
    if (reflection.trim() != _baselineReflection.trim()) {
      changes.add(_buildChangeSummaryItem(
        icon: Icons.auto_stories,
        text: 'Daily Reflection',
        color: AppTheme.accentGold,
      ));
    }
    
    // Check intention changes against baseline
    if (intention.trim() != _baselineIntention.trim()) {
      changes.add(_buildChangeSummaryItem(
        icon: Icons.wb_sunny,
        text: 'Tomorrow\'s Intention',
        color: AppTheme.goalTrackerGradient[0],
      ));
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
            _intentionController.text = intention;
          }
        });
        
        print('📖 Draft progress loaded successfully');
        
        // Set loaded values as the baseline for change detection
        _baselineMood = selectedMood;
        _baselineGratitudes = List.from(gratitudeItems);
        _baselineReflection = reflection.trim();
        _baselineIntention = intention.trim();
        
        // Reset interaction flags when loading draft
        _userHasInteracted = false;
        _moodChanged = false;
        _gratitudeChanged = false;
        _reflectionChanged = false;
        _intentionChanged = false;
        _hasLoadedDraft = true;
        
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
      
      // Update baseline and reset interaction flags
      setState(() {
        // Current state becomes the new baseline for future change detection
        _baselineMood = selectedMood;
        _baselineGratitudes = List.from(gratitudeItems);
        _baselineReflection = reflection.trim();
        _baselineIntention = intention.trim();
        
        print('📖 NEW BASELINE SET AFTER SAVE:');
        print('   Baseline mood: $_baselineMood');
        print('   Baseline gratitudes: $_baselineGratitudes');  
        print('   Baseline reflection: "$_baselineReflection"');
        print('   Baseline intention: "$_baselineIntention"');
        
        // Reset interaction flags after saving - current state is now the baseline
        _userHasInteracted = false;
        _moodChanged = false;
        _gratitudeChanged = false;
        _reflectionChanged = false;
        _intentionChanged = false;
        
        print('📖 Flags reset after save - _userHasInteracted: $_userHasInteracted');
      });
      
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

  /// Update draft progress (debounced version for text input)
  void _updateDraftProgress() {
    // Call the existing save draft method without showing snackbar
    _saveDraftProgress(false);
  }
}