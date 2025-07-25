/// Constants for goal tracker functionality
/// These correspond to the Manifest.Common.Enums.PredefinedGoalStatusTypes enum on the server
class GoalStatusTypes {
  static const int open = 3300001;      // PredefinedGoalStatusTypes.Open
  static const int completed = 3300002; // PredefinedGoalStatusTypes.Completed  
  static const int abandoned = 3300003; // PredefinedGoalStatusTypes.Abandoned
}

/// Category type enum for filtering categories by their intended use
/// These correspond to the Manifest.Common.Enums.PredefinedCategoryTypes enum on the server
class PredefinedCategoryTypes {
  static const int dailyAffirmation = 3400001; // PredefinedCategoryTypes.DailyAffirmation
  static const int visionBoard = 3400002;      // PredefinedCategoryTypes.VisionBoard
  static const int meditation = 3400003;       // PredefinedCategoryTypes.Meditation
  static const int goalTracker = 3400004;      // PredefinedCategoryTypes.GoalTracker
}

/// Extension to get display names for goal status types
extension GoalStatusTypeExtension on int {
  String get goalStatusDisplayName {
    switch (this) {
      case GoalStatusTypes.open:
        return 'Open';
      case GoalStatusTypes.completed:
        return 'Completed';
      case GoalStatusTypes.abandoned:
        return 'Abandoned';
      default:
        return 'Unknown';
    }
  }
}