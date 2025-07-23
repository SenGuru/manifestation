import 'dart:io';
import 'dart:convert';
import 'dart:async';
import 'package:flutter/material.dart';
import 'package:path_provider/path_provider.dart';
import 'package:xml/xml.dart';
import '../models/vision_board.dart';
import '../services/vision_board_service.dart';

/// XML Manager for Vision Board local storage and database sync
/// 
/// This handles:
/// 1. Local XML file creation and updates
/// 2. Converting VisionBoard models to/from XML
/// 3. Database sync (upload/download XML files)
/// 4. Incremental modifications (add/edit/delete elements)
class VisionBoardXMLManager {
  static bool _isWriting = false;
  static Timer? _syncTimer;
  static const String _xmlFileName = 'vision_boards.xml';
  static const String _imagesDirectory = 'vision_board_images';
  
  /// Get the local XML file path
  static Future<String> get _xmlFilePath async {
    final directory = await getApplicationDocumentsDirectory();
    return '${directory.path}/$_xmlFileName';
  }
  
  /// Get the images directory path
  static Future<String> get _imagesDirectoryPath async {
    final directory = await getApplicationDocumentsDirectory();
    final imagesDir = '${directory.path}/$_imagesDirectory';
    
    // Create directory if it doesn't exist
    await Directory(imagesDir).create(recursive: true);
    return imagesDir;
  }
  
  // ========================= XML STRUCTURE =========================
  /// XML Schema for Vision Boards:
  /// 
  /// <?xml version="1.0" encoding="UTF-8"?>
  /// <VisionBoards version="1.0" lastModified="2025-01-15T10:30:00Z" userId="1005">
  ///   <VisionBoard id="board_123" title="My Goals 2025" backgroundColor="#FFFFFF" 
  ///                createdAt="2025-01-15T09:00:00Z" updatedAt="2025-01-15T10:30:00Z">
  ///     <Elements>
  ///       <ImageElement id="img_001" x="100.0" y="150.0" width="200.0" height="150.0" 
  ///                     rotation="0.0" zIndex="1" imagePath="local_image_001.jpg" 
  ///                     imageUrl="https://example.com/image.jpg" fit="cover" />
  ///       <TextElement id="txt_001" x="50.0" y="50.0" width="300.0" height="100.0" 
  ///                    rotation="0.0" zIndex="2" text="I am successful" 
  ///                    fontSize="24.0" fontWeight="bold" color="#000000" 
  ///                    textAlign="center" />
  ///     </Elements>
  ///   </VisionBoard>
  /// </VisionBoards>
  
  // ========================= LOCAL XML OPERATIONS =========================
  
  /// Initialize XML file if it doesn't exist
  static Future<void> initializeXMLFile(int userId) async {
    final filePath = await _xmlFilePath;
    final file = File(filePath);
    
    if (!await file.exists()) {
      print('📄 Creating new vision board XML file...');
      final xmlDoc = _createEmptyXMLDocument(userId);
      await file.writeAsString(xmlDoc.toXmlString(pretty: true));
      print('✅ XML file created at: $filePath');
    } else {
      print('📄 XML file already exists at: $filePath');
    }
  }
  
  /// Create empty XML document structure
  static XmlDocument _createEmptyXMLDocument(int userId) {
    final builder = XmlBuilder();
    builder.processing('xml', 'version="1.0" encoding="UTF-8"');
    builder.element('VisionBoards', nest: () {
      builder.attribute('version', '1.0');
      builder.attribute('lastModified', DateTime.now().toUtc().toIso8601String());
      builder.attribute('userId', userId.toString());
    });
    return builder.buildDocument();
  }
  
  /// Save a vision board to local XML file
  static Future<void> saveVisionBoardToXML(VisionBoard visionBoard, int userId) async {
    // Wait if another write operation is in progress
    while (_isWriting) {
      await Future.delayed(Duration(milliseconds: 100));
    }
    
    _isWriting = true;
    try {
      print('💾 Saving vision board "${visionBoard.title}" to XML...');
      
      // Ensure the vision board has a valid ID
      VisionBoard boardToSave = visionBoard;
      if (visionBoard.id.isEmpty) {
        boardToSave = visionBoard.copyWith(
          id: 'board_${DateTime.now().millisecondsSinceEpoch}',
        );
        print('🆔 Generated new ID for vision board: ${boardToSave.id}');
      }
      
      final filePath = await _xmlFilePath;
      final file = File(filePath);
      
      XmlDocument xmlDoc;
      if (await file.exists()) {
        final xmlString = await file.readAsString();
        xmlDoc = XmlDocument.parse(xmlString);
      } else {
        xmlDoc = _createEmptyXMLDocument(userId);
      }
      
      // Update lastModified timestamp
      final rootElement = xmlDoc.rootElement;
      rootElement.setAttribute('lastModified', DateTime.now().toUtc().toIso8601String());
      
      // Remove existing vision board with same ID (if updating)
      final existingBoard = rootElement.children
          .whereType<XmlElement>()
          .where((element) => 
              element.name.local == 'VisionBoard' && 
              element.getAttribute('id') == boardToSave.id)
          .firstOrNull;
      
      if (existingBoard != null) {
        rootElement.children.remove(existingBoard);
        print('🔄 Updated existing vision board');
      } else {
        print('➕ Added new vision board');
      }
      
      // Add the vision board element
      final visionBoardElement = _visionBoardToXmlElement(boardToSave);
      rootElement.children.add(visionBoardElement);
      
      // Save to file
      await file.writeAsString(xmlDoc.toXmlString(pretty: true));
      print('✅ Vision board saved to XML successfully');
      
      // Trigger background sync to server (fire and forget)
      _triggerBackgroundSync();
      
    } catch (e) {
      print('❌ Error saving vision board to XML: $e');
      throw Exception('Failed to save vision board to XML: $e');
    } finally {
      _isWriting = false;
    }
  }
  
  /// Convert VisionBoard model to XML element
  static XmlElement _visionBoardToXmlElement(VisionBoard visionBoard) {
    // Create a new document to build the element in
    final builder = XmlBuilder();
    
    builder.element('VisionBoard', nest: () {
      builder.attribute('id', visionBoard.id);
      builder.attribute('title', visionBoard.title);
      builder.attribute('backgroundColor', '#${visionBoard.backgroundColor.value.toRadixString(16).padLeft(8, '0').substring(2)}');
      builder.attribute('createdAt', visionBoard.createdAt.toUtc().toIso8601String());
      builder.attribute('updatedAt', visionBoard.updatedAt.toUtc().toIso8601String());
      
      builder.element('Elements', nest: () {
        for (final element in visionBoard.elements) {
          if (element is VisionBoardImageElement) {
            builder.element('ImageElement', nest: () {
              builder.attribute('id', element.id);
              builder.attribute('x', element.position.dx.toString());
              builder.attribute('y', element.position.dy.toString());
              builder.attribute('width', element.size.width.toString());
              builder.attribute('height', element.size.height.toString());
              builder.attribute('rotation', element.rotation.toString());
              builder.attribute('zIndex', element.zIndex.toString());
              builder.attribute('imagePath', element.imagePath);
              if (element.imageUrl != null) {
                builder.attribute('imageUrl', element.imageUrl!);
              }
              builder.attribute('fit', element.fit.name);
            });
          } else if (element is VisionBoardTextElement) {
            builder.element('TextElement', nest: () {
              builder.attribute('id', element.id);
              builder.attribute('x', element.position.dx.toString());
              builder.attribute('y', element.position.dy.toString());
              builder.attribute('width', element.size.width.toString());
              builder.attribute('height', element.size.height.toString());
              builder.attribute('rotation', element.rotation.toString());
              builder.attribute('zIndex', element.zIndex.toString());
              builder.attribute('text', element.text);
              builder.attribute('fontSize', (element.style.fontSize ?? 16.0).toString());
              builder.attribute('fontWeight', (element.style.fontWeight?.index ?? 3).toString());
              builder.attribute('color', '#${(element.style.color?.value ?? 0xFF000000).toRadixString(16).padLeft(8, '0').substring(2)}');
              builder.attribute('textAlign', element.alignment.name);
            });
          }
        }
      });
    });
    
    // Build the document and get the root element
    final builtDoc = builder.buildDocument();
    final rootElement = builtDoc.rootElement;
    
    // Create a copy of the element that is detached from its parent
    return rootElement.copy();
  }
  
  /// Load all vision boards from local XML file
  static Future<List<VisionBoard>> loadVisionBoardsFromXML() async {
    try {
      print('📖 Loading vision boards from XML...');
      
      final filePath = await _xmlFilePath;
      final file = File(filePath);
      
      if (!await file.exists()) {
        print('📄 No XML file found, returning empty list');
        return [];
      }
      
      final xmlString = await file.readAsString();
      final xmlDoc = XmlDocument.parse(xmlString);
      final rootElement = xmlDoc.rootElement;
      
      final visionBoards = <VisionBoard>[];
      
      for (final boardElement in rootElement.children.whereType<XmlElement>()) {
        if (boardElement.name.local == 'VisionBoard') {
          final visionBoard = _xmlElementToVisionBoard(boardElement);
          visionBoards.add(visionBoard);
        }
      }
      
      print('✅ Loaded ${visionBoards.length} vision boards from XML');
      return visionBoards;
      
    } catch (e) {
      print('❌ Error loading vision boards from XML: $e');
      return [];
    }
  }
  
  /// Convert XML element to VisionBoard model
  static VisionBoard _xmlElementToVisionBoard(XmlElement element) {
    final id = element.getAttribute('id')!;
    final title = element.getAttribute('title')!;
    final backgroundColorHex = element.getAttribute('backgroundColor')!;
    final backgroundColor = Color(int.parse(backgroundColorHex.replaceAll('#', 'FF'), radix: 16));
    final createdAt = DateTime.parse(element.getAttribute('createdAt')!);
    final updatedAt = DateTime.parse(element.getAttribute('updatedAt')!);
    
    final elements = <VisionBoardElement>[];
    final elementsNode = element.children
        .whereType<XmlElement>()
        .firstWhere((e) => e.name.local == 'Elements');
    
    for (final elementNode in elementsNode.children.whereType<XmlElement>()) {
      if (elementNode.name.local == 'ImageElement') {
        elements.add(_xmlElementToImageElement(elementNode));
      } else if (elementNode.name.local == 'TextElement') {
        elements.add(_xmlElementToTextElement(elementNode));
      }
    }
    
    return VisionBoard(
      id: id,
      title: title,
      elements: elements,
      backgroundColor: backgroundColor,
      createdAt: createdAt,
      updatedAt: updatedAt,
    );
  }
  
  /// Convert XML element to VisionBoardImageElement
  static VisionBoardImageElement _xmlElementToImageElement(XmlElement element) {
    return VisionBoardImageElement(
      id: element.getAttribute('id')!,
      position: Offset(
        double.parse(element.getAttribute('x')!),
        double.parse(element.getAttribute('y')!),
      ),
      size: Size(
        double.parse(element.getAttribute('width')!),
        double.parse(element.getAttribute('height')!),
      ),
      rotation: double.parse(element.getAttribute('rotation')!),
      zIndex: int.parse(element.getAttribute('zIndex')!),
      imagePath: element.getAttribute('imagePath')!,
      imageUrl: element.getAttribute('imageUrl'),
      fit: BoxFit.values.firstWhere((fit) => fit.name == element.getAttribute('fit')),
    );
  }
  
  /// Convert XML element to VisionBoardTextElement  
  static VisionBoardTextElement _xmlElementToTextElement(XmlElement element) {
    final colorHex = element.getAttribute('color')!;
    final color = Color(int.parse(colorHex.replaceAll('#', 'FF'), radix: 16));
    final fontSize = double.parse(element.getAttribute('fontSize')!);
    final fontWeight = FontWeight.values[int.parse(element.getAttribute('fontWeight')!)];
    final textAlign = TextAlign.values.firstWhere((align) => align.name == element.getAttribute('textAlign'));
    
    return VisionBoardTextElement(
      id: element.getAttribute('id')!,
      position: Offset(
        double.parse(element.getAttribute('x')!),
        double.parse(element.getAttribute('y')!),
      ),
      size: Size(
        double.parse(element.getAttribute('width')!),
        double.parse(element.getAttribute('height')!),
      ),
      rotation: double.parse(element.getAttribute('rotation')!),
      zIndex: int.parse(element.getAttribute('zIndex')!),
      text: element.getAttribute('text')!,
      style: TextStyle(
        fontSize: fontSize,
        fontWeight: fontWeight,
        color: color,
      ),
      alignment: textAlign,
    );
  }
  
  /// Delete a vision board from local XML file
  static Future<void> deleteVisionBoardFromXML(String visionBoardId) async {
    try {
      print('🗑️ Deleting vision board $visionBoardId from XML...');
      
      final filePath = await _xmlFilePath;
      final file = File(filePath);
      
      if (!await file.exists()) {
        print('📄 No XML file found, nothing to delete');
        return;
      }
      
      final xmlString = await file.readAsString();
      final xmlDoc = XmlDocument.parse(xmlString);
      final rootElement = xmlDoc.rootElement;
      
      // Find and remove the vision board
      final boardToRemove = rootElement.children
          .whereType<XmlElement>()
          .where((element) => 
              element.name.local == 'VisionBoard' && 
              element.getAttribute('id') == visionBoardId)
          .firstOrNull;
      
      if (boardToRemove != null) {
        rootElement.children.remove(boardToRemove);
        
        // Update lastModified timestamp
        rootElement.setAttribute('lastModified', DateTime.now().toUtc().toIso8601String());
        
        // Save to file
        await file.writeAsString(xmlDoc.toXmlString(pretty: true));
        print('✅ Vision board deleted from XML successfully');
      } else {
        print('⚠️ Vision board not found in XML');
      }
      
    } catch (e) {
      print('❌ Error deleting vision board from XML: $e');
      throw Exception('Failed to delete vision board from XML: $e');
    }
  }
  
  // ========================= IMAGE MANAGEMENT =========================
  
  /// Save image file locally and return local path
  static Future<String> saveImageLocally(File imageFile, String elementId) async {
    try {
      final imagesDir = await _imagesDirectoryPath;
      final extension = imageFile.path.split('.').last;
      final localFileName = '${elementId}_${DateTime.now().millisecondsSinceEpoch}.$extension';
      final localPath = '$imagesDir/$localFileName';
      
      await imageFile.copy(localPath);
      print('💾 Image saved locally: $localPath');
      
      return localPath;
    } catch (e) {
      print('❌ Error saving image locally: $e');
      throw Exception('Failed to save image locally: $e');
    }
  }
  
  /// Get local image file
  static Future<File?> getLocalImageFile(String imagePath) async {
    try {
      final file = File(imagePath);
      if (await file.exists()) {
        return file;
      } else {
        print('⚠️ Local image file not found: $imagePath');
        return null;
      }
    } catch (e) {
      print('❌ Error getting local image file: $e');
      return null;
    }
  }
  
  // ========================= DATABASE SYNC =========================
  
  /// Upload XML file to database
  static Future<bool> uploadXMLToDatabase() async {
    try {
      print('☁️ Uploading XML file to database...');
      
      final filePath = await _xmlFilePath;
      final file = File(filePath);
      
      if (!await file.exists()) {
        print('📄 No XML file to upload');
        return false;
      }
      
      final xmlContent = await file.readAsString();
      
      // Upload via VisionBoardService
      final visionBoardService = VisionBoardService();
      await visionBoardService.initialize();
      final result = await visionBoardService.uploadXMLFile(xmlContent);
      
      if (result.isSuccess) {
        print('✅ XML file uploaded to database successfully');
        return true;
      } else {
        print('❌ XML upload failed: ${result.message}');
        return false;
      }
      
    } catch (e) {
      print('❌ Error uploading XML to database: $e');
      return false;
    }
  }
  
  /// Download XML file from database
  static Future<bool> downloadXMLFromDatabase(int userId) async {
    try {
      print('☁️ Downloading XML file from database...');
      
      // TODO: Implement API call to download XML content
      // final result = await visionBoardService.downloadXMLFile(userId);
      
      // if (result.isSuccess && result.data != null) {
      //   final filePath = await _xmlFilePath;
      //   final file = File(filePath);
      //   await file.writeAsString(result.data!);
      //   print('✅ XML file downloaded from database successfully');
      //   return true;
      // }
      
      print('⚠️ No XML file found in database or download failed');
      return false;
      
    } catch (e) {
      print('❌ Error downloading XML from database: $e');
      return false;
    }
  }
  
  /// Check if local XML is newer than database version
  static Future<bool> isLocalXMLNewer() async {
    try {
      // TODO: Implement comparison with database timestamp
      // final localTimestamp = await getLocalXMLTimestamp();
      // final databaseTimestamp = await getDatabaseXMLTimestamp();
      // return localTimestamp.isAfter(databaseTimestamp);
      
      return false; // Placeholder
    } catch (e) {
      print('❌ Error checking XML timestamp: $e');
      return false;
    }
  }
  
  /// Get the last modified timestamp from local XML
  static Future<DateTime?> getLocalXMLTimestamp() async {
    try {
      final filePath = await _xmlFilePath;
      final file = File(filePath);
      
      if (!await file.exists()) {
        return null;
      }
      
      final xmlString = await file.readAsString();
      final xmlDoc = XmlDocument.parse(xmlString);
      final lastModified = xmlDoc.rootElement.getAttribute('lastModified');
      
      if (lastModified != null) {
        return DateTime.parse(lastModified);
      }
      
      return null;
    } catch (e) {
      print('❌ Error getting local XML timestamp: $e');
      return null;
    }
  }

  // ========================= BACKGROUND SYNC =========================
  
  /// Trigger background sync with debouncing (fire and forget) 
  static void _triggerBackgroundSync() {
    // Cancel any existing timer
    _syncTimer?.cancel();
    
    // Set new timer to sync after 5 seconds (debounced)
    _syncTimer = Timer(Duration(seconds: 5), () async {
      try {
        print('🔄 Starting background sync...');
        final success = await uploadXMLToDatabase();
        if (success) {
          print('✅ Background sync completed');
        } else {
          print('⚠️ Background sync failed - will retry later');
        }
      } catch (e) {
        print('❌ Background sync error: $e');
      }
    });
  }
  
  /// Start periodic background sync (call once during app initialization)
  static void startPeriodicSync({Duration interval = const Duration(minutes: 15)}) {
    Timer.periodic(interval, (timer) async {
      try {
        print('⏰ Periodic sync starting...');
        final success = await uploadXMLToDatabase();
        if (success) {
          print('✅ Periodic sync completed');
        } else {
          print('⚠️ Periodic sync failed');
        }
      } catch (e) {
        print('❌ Periodic sync error: $e');
      }
    });
  }
}

/// Extension to add firstOrNull method
extension FirstOrNull<T> on Iterable<T> {
  T? get firstOrNull => isEmpty ? null : first;
}