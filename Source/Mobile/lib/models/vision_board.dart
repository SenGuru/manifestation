import 'package:flutter/material.dart';
import 'package:fun/constants/colors.dart';

class VisionBoard {
  final String id;
  final String title;
  final List<VisionBoardElement> elements;
  final Color backgroundColor;
  final DateTime createdAt;
  final DateTime updatedAt;

  const VisionBoard({
    required this.id,
    required this.title,
    required this.elements,
    required this.backgroundColor,
    required this.createdAt,
    required this.updatedAt,
  });

  factory VisionBoard.empty() {
    final now = DateTime.now();
    return VisionBoard(
      id: DateTime.now().millisecondsSinceEpoch.toString(),
      title: '',
      elements: [],
      backgroundColor: AppColors.white,
      createdAt: now,
      updatedAt: now,
    );
  }

  factory VisionBoard.fromTemplate(VisionBoardTemplate template) {
    final now = DateTime.now();
    return VisionBoard(
      id: DateTime.now().millisecondsSinceEpoch.toString(),
      title: template.name,
      elements: template.elements,
      backgroundColor: template.backgroundColor,
      createdAt: now,
      updatedAt: now,
    );
  }

  VisionBoard copyWith({
    String? id,
    String? title,
    List<VisionBoardElement>? elements,
    Color? backgroundColor,
    DateTime? createdAt,
    DateTime? updatedAt,
  }) {
    return VisionBoard(
      id: id ?? this.id,
      title: title ?? this.title,
      elements: elements ?? this.elements,
      backgroundColor: backgroundColor ?? this.backgroundColor,
      createdAt: createdAt ?? this.createdAt,
      updatedAt: updatedAt ?? DateTime.now(),
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'title': title,
      'elements': elements.map((e) => e.toJson()).toList(),
      'backgroundColor': backgroundColor.value,
      'createdAt': createdAt.toIso8601String(),
      'updatedAt': updatedAt.toIso8601String(),
    };
  }

  factory VisionBoard.fromJson(Map<String, dynamic> json) {
    return VisionBoard(
      id: json['id'],
      title: json['title'],
      elements: (json['elements'] as List)
          .map((e) => VisionBoardElement.fromJson(e))
          .toList(),
      backgroundColor: Color(json['backgroundColor']),
      createdAt: DateTime.parse(json['createdAt']),
      updatedAt: DateTime.parse(json['updatedAt']),
    );
  }
}

abstract class VisionBoardElement {
  final String id;
  final Offset position;
  final Size size;
  final double rotation;
  final int zIndex;

  const VisionBoardElement({
    required this.id,
    required this.position,
    required this.size,
    this.rotation = 0.0,
    this.zIndex = 0,
  });

  VisionBoardElement copyWith({
    String? id,
    Offset? position,
    Size? size,
    double? rotation,
    int? zIndex,
  });

  Map<String, dynamic> toJson();
  
  factory VisionBoardElement.fromJson(Map<String, dynamic> json) {
    switch (json['type']) {
      case 'image':
        return VisionBoardImageElement.fromJson(json);
      case 'text':
        return VisionBoardTextElement.fromJson(json);
      default:
        throw Exception('Unknown element type: ${json['type']}');
    }
  }
}

class VisionBoardImageElement extends VisionBoardElement {
  final String imagePath;
  final String? imageUrl;
  final BoxFit fit;

  const VisionBoardImageElement({
    required String id,
    required Offset position,
    required Size size,
    required this.imagePath,
    this.imageUrl,
    this.fit = BoxFit.cover,
    double rotation = 0.0,
    int zIndex = 0,
  }) : super(
          id: id,
          position: position,
          size: size,
          rotation: rotation,
          zIndex: zIndex,
        );

  @override
  VisionBoardImageElement copyWith({
    String? id,
    Offset? position,
    Size? size,
    double? rotation,
    int? zIndex,
    String? imagePath,
    String? imageUrl,
    BoxFit? fit,
  }) {
    return VisionBoardImageElement(
      id: id ?? this.id,
      position: position ?? this.position,
      size: size ?? this.size,
      rotation: rotation ?? this.rotation,
      zIndex: zIndex ?? this.zIndex,
      imagePath: imagePath ?? this.imagePath,
      imageUrl: imageUrl ?? this.imageUrl,
      fit: fit ?? this.fit,
    );
  }

  @override
  Map<String, dynamic> toJson() {
    return {
      'type': 'image',
      'id': id,
      'position': {'dx': position.dx, 'dy': position.dy},
      'size': {'width': size.width, 'height': size.height},
      'rotation': rotation,
      'zIndex': zIndex,
      'imagePath': imagePath,
      'imageUrl': imageUrl,
      'fit': fit.index,
    };
  }

  factory VisionBoardImageElement.fromJson(Map<String, dynamic> json) {
    return VisionBoardImageElement(
      id: json['id'],
      position: Offset(json['position']['dx'], json['position']['dy']),
      size: Size(json['size']['width'], json['size']['height']),
      rotation: json['rotation'],
      zIndex: json['zIndex'],
      imagePath: json['imagePath'],
      imageUrl: json['imageUrl'],
      fit: BoxFit.values[json['fit']],
    );
  }
}

class VisionBoardTextElement extends VisionBoardElement {
  final String text;
  final TextStyle style;
  final TextAlign alignment;

  const VisionBoardTextElement({
    required String id,
    required Offset position,
    required Size size,
    required this.text,
    required this.style,
    this.alignment = TextAlign.center,
    double rotation = 0.0,
    int zIndex = 0,
  }) : super(
          id: id,
          position: position,
          size: size,
          rotation: rotation,
          zIndex: zIndex,
        );

  @override
  VisionBoardTextElement copyWith({
    String? id,
    Offset? position,
    Size? size,
    double? rotation,
    int? zIndex,
    String? text,
    TextStyle? style,
    TextAlign? alignment,
  }) {
    return VisionBoardTextElement(
      id: id ?? this.id,
      position: position ?? this.position,
      size: size ?? this.size,
      rotation: rotation ?? this.rotation,
      zIndex: zIndex ?? this.zIndex,
      text: text ?? this.text,
      style: style ?? this.style,
      alignment: alignment ?? this.alignment,
    );
  }

  @override
  Map<String, dynamic> toJson() {
    return {
      'type': 'text',
      'id': id,
      'position': {'dx': position.dx, 'dy': position.dy},
      'size': {'width': size.width, 'height': size.height},
      'rotation': rotation,
      'zIndex': zIndex,
      'text': text,
      'style': {
        'fontSize': style.fontSize,
        'color': style.color?.value,
        'fontWeight': style.fontWeight?.index,
        'fontStyle': style.fontStyle?.index,
      },
      'alignment': alignment.index,
    };
  }

  factory VisionBoardTextElement.fromJson(Map<String, dynamic> json) {
    final styleJson = json['style'];
    return VisionBoardTextElement(
      id: json['id'],
      position: Offset(json['position']['dx'], json['position']['dy']),
      size: Size(json['size']['width'], json['size']['height']),
      rotation: json['rotation'],
      zIndex: json['zIndex'],
      text: json['text'],
      style: TextStyle(
        fontSize: styleJson['fontSize'],
        color: styleJson['color'] != null ? Color(styleJson['color']) : null,
        fontWeight: styleJson['fontWeight'] != null 
            ? FontWeight.values[styleJson['fontWeight']] 
            : null,
        fontStyle: styleJson['fontStyle'] != null 
            ? FontStyle.values[styleJson['fontStyle']] 
            : null,
      ),
      alignment: TextAlign.values[json['alignment']],
    );
  }
}

class VisionBoardTemplate {
  final String name;
  final String description;
  final IconData icon;
  final Color color;
  final Color backgroundColor;
  final List<VisionBoardElement> elements;

  const VisionBoardTemplate({
    required this.name,
    required this.description,
    required this.icon,
    required this.color,
    required this.backgroundColor,
    required this.elements,
  });

  static List<VisionBoardTemplate> getDefaultTemplates() {
    return [
      VisionBoardTemplate(
        name: 'Career Goals',
        description: 'Professional aspirations and achievements',
        icon: Icons.business_center,
        color: AppColors.gradientBlue,
        backgroundColor: AppColors.white,
        elements: [],
      ),
      VisionBoardTemplate(
        name: 'Travel Dreams',
        description: 'Places to visit and adventures to have',
        icon: Icons.flight,
        color: AppColors.secondaryTeal,
        backgroundColor: AppColors.white,
        elements: [],
      ),
      VisionBoardTemplate(
        name: 'Love & Family',
        description: 'Relationships and personal connections',
        icon: Icons.favorite,
        color: AppColors.gradientPink,
        backgroundColor: AppColors.white,
        elements: [],
      ),
      VisionBoardTemplate(
        name: 'Health & Wellness',
        description: 'Physical and mental well-being goals',
        icon: Icons.spa,
        color: AppColors.accentMint,
        backgroundColor: AppColors.white,
        elements: [],
      ),
      VisionBoardTemplate(
        name: 'Financial Freedom',
        description: 'Money goals and abundance mindset',
        icon: Icons.account_balance_wallet,
        color: AppColors.accentGold,
        backgroundColor: AppColors.white,
        elements: [],
      ),
      VisionBoardTemplate(
        name: 'Personal Growth',
        description: 'Self-improvement and learning goals',
        icon: Icons.self_improvement,
        color: AppColors.gradientPurple,
        backgroundColor: AppColors.white,
        elements: [],
      ),
    ];
  }
}

class VisionBoardXMLInfo {
  final int pictureId;
  final String visionBoardId;
  final String fileName;
  final DateTime? createdDate;
  final int fileSize;

  const VisionBoardXMLInfo({
    required this.pictureId,
    required this.visionBoardId,
    required this.fileName,
    this.createdDate,
    required this.fileSize,
  });

  Map<String, dynamic> toJson() {
    return {
      'pictureId': pictureId,
      'visionBoardId': visionBoardId,
      'fileName': fileName,
      'createdDate': createdDate?.toIso8601String(),
      'fileSize': fileSize,
    };
  }

  factory VisionBoardXMLInfo.fromJson(Map<String, dynamic> json) {
    return VisionBoardXMLInfo(
      pictureId: json['pictureId'],
      visionBoardId: json['visionBoardId'],
      fileName: json['fileName'],
      createdDate: json['createdDate'] != null 
          ? DateTime.parse(json['createdDate']) 
          : null,
      fileSize: json['fileSize'],
    );
  }
}

enum VisionBoardTool {
  select,
  move,
  resize,
  rotate,
  delete,
}

extension VisionBoardToolExtension on VisionBoardTool {
  IconData get icon {
    switch (this) {
      case VisionBoardTool.select:
        return Icons.touch_app;
      case VisionBoardTool.move:
        return Icons.open_with;
      case VisionBoardTool.resize:
        return Icons.photo_size_select_large;
      case VisionBoardTool.rotate:
        return Icons.rotate_right;
      case VisionBoardTool.delete:
        return Icons.delete;
    }
  }

  String get name {
    switch (this) {
      case VisionBoardTool.select:
        return 'Select';
      case VisionBoardTool.move:
        return 'Move';
      case VisionBoardTool.resize:
        return 'Resize';
      case VisionBoardTool.rotate:
        return 'Rotate';
      case VisionBoardTool.delete:
        return 'Delete';
    }
  }
}