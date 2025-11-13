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

/// Priority types for goal trackers
/// These correspond to the Manifest.Common.Enums.PredefinedGoalPriorityTypes enum on the server
class GoalPriorityTypes {
  static const int low = 3800001;      // PredefinedGoalPriorityTypes.Low
  static const int medium = 3800002;   // PredefinedGoalPriorityTypes.Medium
  static const int high = 3800003;     // PredefinedGoalPriorityTypes.High
  static const int critical = 3800004; // PredefinedGoalPriorityTypes.Critical
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

/// Extension to get display names for goal priority types
extension GoalPriorityTypeExtension on int {
  String get goalPriorityDisplayName {
    switch (this) {
      case GoalPriorityTypes.low:
        return 'Low';
      case GoalPriorityTypes.medium:
        return 'Medium';
      case GoalPriorityTypes.high:
        return 'High';
      case GoalPriorityTypes.critical:
        return 'Critical';
      default:
        return 'Unknown';
    }
  }
}