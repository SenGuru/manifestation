/// Unified affirmation item for UI display
class AffirmationItem {
  final String id;
  final String text;
  final bool isDefault;
  final String categoryName;
  final bool isFavorite;

  AffirmationItem({
    required this.id,
    required this.text,
    required this.isDefault,
    required this.categoryName,
    this.isFavorite = false,
  });

  /// Convert to JSON for caching
  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'text': text,
      'isDefault': isDefault,
      'categoryName': categoryName,
      'isFavorite': isFavorite,
    };
  }

  /// Create from JSON for caching
  factory AffirmationItem.fromJson(Map<String, dynamic> json) {
    return AffirmationItem(
      id: json['id'] ?? '',
      text: json['text'] ?? '',
      isDefault: json['isDefault'] ?? false,
      categoryName: json['categoryName'] ?? '',
      isFavorite: json['isFavorite'] ?? false,
    );
  }

  @override
  bool operator ==(Object other) {
    if (identical(this, other)) return true;
    
    return other is AffirmationItem &&
        other.id == id &&
        other.text == text &&
        other.isDefault == isDefault &&
        other.categoryName == categoryName &&
        other.isFavorite == isFavorite;
  }

  @override
  int get hashCode {
    return id.hashCode ^ 
           text.hashCode ^ 
           isDefault.hashCode ^ 
           categoryName.hashCode ^ 
           isFavorite.hashCode;
  }

  @override
  String toString() {
    return 'AffirmationItem(id: $id, text: $text, isDefault: $isDefault, categoryName: $categoryName, isFavorite: $isFavorite)';
  }
}