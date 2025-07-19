import 'package:flutter/material.dart';
import 'package:flutter_tabler_icons/flutter_tabler_icons.dart';

/// Predefined icon types for meditation/manifestation categories
enum PredefinedIconTypes {
  flower(3600001, 'Flower'),
  heart(3600002, 'Heart'),
  brain(3600003, 'Brain'),
  leaf(3600004, 'Leaf'),
  star(3600005, 'Star'),
  diamond(3600006, 'Diamond'),
  target(3600007, 'Target'),
  trophy(3600008, 'Trophy'),
  sun(3600009, 'Sun'),
  moon(3600010, 'Moon');

  const PredefinedIconTypes(this.value, this.displayName);
  
  final int value;
  final String displayName;
  
  /// Get IconData for this icon type
  IconData get iconData {
    switch (this) {
      case PredefinedIconTypes.flower:
        return TablerIcons.flower;
      case PredefinedIconTypes.heart:
        return TablerIcons.heart;
      case PredefinedIconTypes.brain:
        return TablerIcons.brain;
      case PredefinedIconTypes.leaf:
        return TablerIcons.leaf;
      case PredefinedIconTypes.star:
        return TablerIcons.star;
      case PredefinedIconTypes.diamond:
        return TablerIcons.diamond;
      case PredefinedIconTypes.target:
        return TablerIcons.target;
      case PredefinedIconTypes.trophy:
        return TablerIcons.trophy;
      case PredefinedIconTypes.sun:
        return TablerIcons.sun;
      case PredefinedIconTypes.moon:
        return TablerIcons.moon;
    }
  }
  
  /// Get icon type from integer value
  static PredefinedIconTypes? fromValue(int value) {
    for (final iconType in PredefinedIconTypes.values) {
      if (iconType.value == value) {
        return iconType;
      }
    }
    return null;
  }
  
  /// Get all available icons as a list
  static List<PredefinedIconTypes> getAllIcons() {
    return PredefinedIconTypes.values;
  }
}

/// Category icon utilities
class CategoryIconUtils {
  /// Get default icon for category name
  static PredefinedIconTypes getDefaultIconForCategory(String categoryName) {
    final name = categoryName.toLowerCase();
    
    switch (name) {
      case 'love':
      case 'relationships':
        return PredefinedIconTypes.heart;
      case 'abundance':
      case 'prosperity':
      case 'wealth':
        return PredefinedIconTypes.diamond;
      case 'health':
      case 'wellness':
        return PredefinedIconTypes.leaf;
      case 'success':
      case 'achievement':
        return PredefinedIconTypes.trophy;
      case 'confidence':
      case 'self-esteem':
        return PredefinedIconTypes.star;
      case 'balance':
      case 'harmony':
      case 'peace':
        return PredefinedIconTypes.moon;
      case 'mindfulness':
      case 'meditation':
        return PredefinedIconTypes.flower;
      case 'goals':
      case 'focus':
        return PredefinedIconTypes.target;
      case 'energy':
      case 'vitality':
        return PredefinedIconTypes.sun;
      case 'wisdom':
      case 'knowledge':
        return PredefinedIconTypes.brain;
      default:
        return PredefinedIconTypes.flower; // Default meditation icon
    }
  }
  
  /// Get icon data from integer value
  static IconData getIconFromValue(int? iconValue) {
    if (iconValue == null) return TablerIcons.flower;
    
    final iconType = PredefinedIconTypes.fromValue(iconValue);
    return iconType?.iconData ?? TablerIcons.flower;
  }
  
  /// Get a random icon from available icons
  static PredefinedIconTypes getRandomIcon() {
    final icons = PredefinedIconTypes.getAllIcons();
    return icons[DateTime.now().millisecondsSinceEpoch % icons.length];
  }
}