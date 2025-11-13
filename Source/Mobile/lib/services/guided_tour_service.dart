import 'package:flutter/material.dart';
import 'package:shared_preferences/shared_preferences.dart';
import '../widgets/guided_tour_overlay.dart';
import '../constants/app_theme.dart';

class GuidedTourService {
  static final GuidedTourService _instance = GuidedTourService._internal();
  factory GuidedTourService() => _instance;
  GuidedTourService._internal();

  // Keys for tracking tour completion
  static const String _visionBoardTourKey = 'vision_board_tour_completed';
  static const String _affirmationsTourKey = 'affirmations_tour_completed';
  static const String _journalTourKey = 'journal_tour_completed';
  static const String _goalTrackerTourKey = 'goal_tracker_tour_completed';
  static const String _meditationTourKey = 'meditation_tour_completed';

  /// Check if a specific tour has been completed
  Future<bool> hasTourBeenCompleted(String tourKey) async {
    try {
      final prefs = await SharedPreferences.getInstance();
      final completed = prefs.getBool(tourKey) ?? false;
      print('🎯 Tour key "$tourKey" completion status: $completed');
      return completed;
    } catch (e) {
      print('🎯 Error checking tour completion for "$tourKey": $e');
      return false;
    }
  }

  /// Mark a tour as completed
  Future<void> markTourCompleted(String tourKey) async {
    try {
      final prefs = await SharedPreferences.getInstance();
      await prefs.setBool(tourKey, true);
    } catch (e) {
      print('Error marking tour as completed: $e');
    }
  }

  /// Show guided tour if it hasn't been completed
  Future<void> showTourIfNeeded(
    BuildContext context,
    String tourKey,
    List<TourStep> steps,
  ) async {
    print('🎯 Checking guided tour for key: $tourKey');
    final hasCompleted = await hasTourBeenCompleted(tourKey);
    print('🎯 Tour completed status: $hasCompleted');
    if (!hasCompleted) {
      print('🎯 Showing tour for: $tourKey');
      _showTour(context, tourKey, steps);
    } else {
      print('🎯 Tour already completed for: $tourKey');
    }
  }

  /// Force show a tour (for manual triggers)
  void showTour(
    BuildContext context,
    String tourKey,
    List<TourStep> steps,
  ) {
    _showTour(context, tourKey, steps);
  }

  void _showTour(
    BuildContext context,
    String tourKey,
    List<TourStep> steps,
  ) {
    print('🎯 🔥 ATTEMPTING TO SHOW DIALOG for tour: $tourKey');
    print('🎯 🔥 Context: $context');
    print('🎯 🔥 Steps count: ${steps.length}');
    
    try {
      showDialog(
        context: context,
        barrierDismissible: false,
        barrierColor: Colors.transparent,
        builder: (context) {
          print('🎯 🔥 DIALOG BUILDER CALLED for tour: $tourKey');
          return GuidedTourOverlay(
            steps: steps,
            onComplete: () {
              print('🎯 🔥 TOUR COMPLETED: $tourKey');
              Navigator.of(context).pop();
              markTourCompleted(tourKey);
            },
            onSkip: () {
              print('🎯 🔥 TOUR SKIPPED: $tourKey');
              Navigator.of(context).pop();
              markTourCompleted(tourKey);
            },
          );
        },
      );
      print('🎯 🔥 DIALOG SHOW COMPLETE for tour: $tourKey');
    } catch (e) {
      print('🎯 🔥 ERROR SHOWING DIALOG: $e');
    }
  }

  // Predefined tour configurations

  /// Vision Board tour steps
  List<TourStep> getVisionBoardTourSteps() {
    return [
      TourStep(
        title: 'Welcome to Vision Boards!',
        description: 'Create visual representations of your dreams and goals. Let\'s explore how to build your first vision board.',
        icon: Icons.dashboard,
        iconGradient: AppTheme.visionBoardGradient,
      ),
      TourStep(
        title: 'Add Images',
        description: 'Tap the "+" button to add images that represent your goals. You can upload from your gallery or choose from our collection.',
        icon: Icons.add_photo_alternate,
        iconGradient: AppTheme.visionBoardGradient,
      ),
      TourStep(
        title: 'Add Text & Affirmations',
        description: 'Include powerful affirmations and goals as text elements. Words have power - use them to manifest your dreams.',
        icon: Icons.text_fields,
        iconGradient: AppTheme.visionBoardGradient,
      ),
      TourStep(
        title: 'Move & Resize',
        description: 'Drag elements around and resize them to create your perfect layout. Make it feel right for you.',
        icon: Icons.open_with,
        iconGradient: AppTheme.visionBoardGradient,
      ),
      TourStep(
        title: 'Save & Manifest',
        description: 'Save your vision board and visit it daily. Visualization is a powerful tool for manifestation!',
        icon: Icons.save,
        iconGradient: AppTheme.visionBoardGradient,
      ),
    ];
  }

  /// Daily Affirmations tour steps
  List<TourStep> getAffirmationsTourSteps() {
    return [
      TourStep(
        title: 'Daily Affirmations',
        description: 'Start each day with powerful affirmations that align your mindset with your goals.',
        icon: Icons.favorite,
        iconGradient: AppTheme.affirmationGradient,
      ),
      TourStep(
        title: 'Swipe for More',
        description: 'Swipe left or right to explore different affirmations. Find the ones that resonate with you.',
        icon: Icons.swipe,
        iconGradient: AppTheme.affirmationGradient,
      ),
      TourStep(
        title: 'Read Aloud',
        description: 'Tap the play button to hear affirmations spoken aloud. Speaking affirmations increases their power.',
        icon: Icons.volume_up,
        iconGradient: AppTheme.affirmationGradient,
      ),
      TourStep(
        title: 'Set Reminders',
        description: 'Tap the bell icon to set daily reminders. Consistency is key to rewiring your mindset.',
        icon: Icons.notifications,
        iconGradient: AppTheme.affirmationGradient,
      ),
      TourStep(
        title: 'Make it a Habit',
        description: 'Try to read affirmations daily, especially in the morning. Watch your thoughts transform!',
        icon: Icons.auto_awesome,
        iconGradient: AppTheme.affirmationGradient,
      ),
    ];
  }

  /// Journal tour steps
  List<TourStep> getJournalTourSteps() {
    return [
      TourStep(
        title: 'Manifestation Journal',
        description: 'Document your manifestation journey, track your progress, and reflect on your growth.',
        icon: Icons.book,
        iconGradient: AppTheme.journalGradient,
      ),
      TourStep(
        title: 'Daily Entries',
        description: 'Write about your goals, thoughts, and feelings. Regular journaling clarifies your desires.',
        icon: Icons.edit,
        iconGradient: AppTheme.journalGradient,
      ),
      TourStep(
        title: 'Gratitude Practice',
        description: 'Include what you\'re grateful for each day. Gratitude raises your vibration and attracts more good.',
        icon: Icons.favorite_border,
        iconGradient: AppTheme.journalGradient,
      ),
      TourStep(
        title: 'Track Manifestations',
        description: 'Record when your desires manifest! This builds faith and shows the power of your thoughts.',
        icon: Icons.star,
        iconGradient: AppTheme.journalGradient,
      ),
      TourStep(
        title: 'Review & Reflect',
        description: 'Look back at past entries to see your growth and patterns. Your journey is inspiring!',
        icon: Icons.history,
        iconGradient: AppTheme.journalGradient,
      ),
    ];
  }

  /// Goal Tracker tour steps
  List<TourStep> getGoalTrackerTourSteps() {
    return [
      TourStep(
        title: 'Goal Tracker',
        description: 'Set clear goals and track your progress toward manifesting them into reality.',
        icon: Icons.track_changes,
        iconGradient: AppTheme.goalTrackerGradient,
      ),
      TourStep(
        title: 'Create Goals',
        description: 'Tap "+" to add new goals. Be specific and write them as if they\'ve already happened.',
        icon: Icons.add_task,
        iconGradient: AppTheme.goalTrackerGradient,
      ),
      TourStep(
        title: 'Set Deadlines',
        description: 'Give your goals a timeframe. This creates urgency and helps the universe align with your timing.',
        icon: Icons.schedule,
        iconGradient: AppTheme.goalTrackerGradient,
      ),
      TourStep(
        title: 'Track Progress',
        description: 'Update your progress regularly. Seeing advancement keeps you motivated and focused.',
        icon: Icons.trending_up,
        iconGradient: AppTheme.goalTrackerGradient,
      ),
      TourStep(
        title: 'Celebrate Wins',
        description: 'Mark goals as complete and celebrate! Acknowledging success attracts more of it.',
        icon: Icons.celebration,
        iconGradient: AppTheme.goalTrackerGradient,
      ),
    ];
  }

  /// Meditation tour steps
  List<TourStep> getMeditationTourSteps() {
    return [
      TourStep(
        title: 'Visual Meditation',
        description: 'Use guided meditation to calm your mind and align with your highest self.',
        icon: Icons.visibility,
        iconGradient: AppTheme.visualMeditationGradient,
      ),
      TourStep(
        title: 'Choose Your Focus',
        description: 'Select from different meditation styles - candle gazing, nature sounds, or guided visualization.',
        icon: Icons.select_all,
        iconGradient: AppTheme.visualMeditationGradient,
      ),
      TourStep(
        title: 'Set Your Duration',
        description: 'Start with shorter sessions and gradually increase. Even 5 minutes can make a difference.',
        icon: Icons.timer,
        iconGradient: AppTheme.visualMeditationGradient,
      ),
      TourStep(
        title: 'Focus & Breathe',
        description: 'Follow the guidance and focus on your breath. Let go of thoughts and be present.',
        icon: Icons.air,
        iconGradient: AppTheme.visualMeditationGradient,
      ),
      TourStep(
        title: 'Daily Practice',
        description: 'Make meditation a daily habit. It raises your vibration and enhances manifestation power.',
        icon: Icons.self_improvement,
        iconGradient: AppTheme.visualMeditationGradient,
      ),
    ];
  }

  // Tour key constants for easy reference
  static String get visionBoardTourKey => _visionBoardTourKey;
  static String get affirmationsTourKey => _affirmationsTourKey;
  static String get journalTourKey => _journalTourKey;
  static String get goalTrackerTourKey => _goalTrackerTourKey;
  static String get meditationTourKey => _meditationTourKey;
}