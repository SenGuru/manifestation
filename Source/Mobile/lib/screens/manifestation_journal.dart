import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:fun/components/gratitude_selection.dart';
import 'package:fun/components/journal_prompt.dart' hide GratitudeSection;
import 'package:fun/models/journey_entry.dart';
import 'package:fun/constants/colors.dart';
import 'package:fun/models/mood_selector.dart';
import 'package:fun/screens/calendar_view_screen.dart';
import 'package:fun/services/daily_journal_service.dart';
import 'package:fun/services/guided_tour_service.dart';
import 'package:uuid/uuid.dart';

class ManifestationJournalScreen extends StatefulWidget {
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
        backgroundColor: AppColors.backgroundDark,
        body: Center(
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              CircularProgressIndicator(
                valueColor: AlwaysStoppedAnimation<Color>(AppColors.gradientPurple),
              ),
              SizedBox(height: 16.h),
              Text(
                'Loading journal...',
                style: TextStyle(
                  color: AppColors.textMedium,
                  fontSize: 16.sp,
                ),
              ),
            ],
          ),
        ),
      );
    }

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
                border: Border.all(
                      color: AppColors.textMedium.withOpacity(0.1),
                    ),
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
              onTap: isSaving ? null : (currentPage < 4 ? _nextPage : _saveEntry),
              child: Container(
                height: 50.h,
                decoration: BoxDecoration(
                  gradient: LinearGradient(
                    colors: AppColors.journalGradient,
                  ),
                  borderRadius: BorderRadius.circular(12.r),
                ),
                child: Center(
                  child: isSaving && currentPage == 4
                      ? Row(
                          mainAxisSize: MainAxisSize.min,
                          children: [
                            SizedBox(
                              width: 16.w,
                              height: 16.h,
                              child: CircularProgressIndicator(
                                strokeWidth: 2,
                                valueColor: AlwaysStoppedAnimation<Color>(AppColors.white),
                              ),
                            ),
                            SizedBox(width: 8.w),
                            Text(
                              isEditingExistingEntry ? 'Updating...' : 'Saving...',
                              style: TextStyle(
                                fontSize: 16.sp,
                                fontWeight: FontWeight.w600,
                                color: AppColors.white,
                              ),
                            ),
                          ],
                        )
                      : Text(
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
}