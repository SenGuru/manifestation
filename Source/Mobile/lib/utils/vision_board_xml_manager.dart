import 'dart:io';
import 'package:flutter/material.dart';
import 'package:path_provider/path_provider.dart';
import 'package:xml/xml.dart';
import '../models/vision_board.dart';
import '../services/vision_board_service.dart';

/// XML Manager for Vision Board local storage
/// 
/// This handles:
/// 1. Local XML file creation and updates
/// 2. Converting VisionBoard models to/from XML
/// 3. Incremental modifications (add/edit/delete elements)
class VisionBoardXMLManager {
  static bool _isWriting = false;
  static const String _imagesDirectory = 'vision_board_images';
  
  /// Get the local XML file path for a specific vision board
  static Future<String> _getVisionBoardXmlFilePath(String visionBoardId) async {
    final directory = await getApplicationDocumentsDirectory();
    return '${directory.path}/visionboard_$visionBoardId.xml';
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
  
  /// Initialize vision boards directory (no need for single XML file)
  static Future<void> initializeXMLFile(int userId) async {
    try {
      // Just ensure the directory exists for individual XML files
      final directory = await getApplicationDocumentsDirectory();
      final directoryPath = directory.path;
      
      // Create the main directory if it doesn't exist
      final dir = Directory(directoryPath);
      if (!await dir.exists()) {
        await dir.create(recursive: true);
      }
      
      print('📁 Vision board XML storage initialized at: $directoryPath');
      print('📄 Individual XML files will be created as: visionboard_[ID].xml');
      
    } catch (e) {
      print('❌ Error initializing XML storage: $e');
    }
  }
  
  /// Create XML document for a single vision board
  static XmlDocument _createVisionBoardXMLDocument(VisionBoard visionBoard, int userId) {
    final builder = XmlBuilder();
    builder.processing('xml', 'version="1.0" encoding="UTF-8"');
    builder.element('VisionBoards', nest: () {
      builder.attribute('version', '1.0');
      builder.attribute('lastModified', DateTime.now().toUtc().toIso8601String());
      builder.attribute('userId', userId.toString());
      
      // We need to manually add the vision board content since we can't add XmlElement directly
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
    });
    return builder.buildDocument();
  }
  
  /// Save a vision board to its own individual XML file
  static Future<void> saveVisionBoardToXML(VisionBoard visionBoard, int userId) async {
    // Wait if another write operation is in progress
    while (_isWriting) {
      await Future.delayed(Duration(milliseconds: 100));
    }
    
    _isWriting = true;
    try {
      print('💾 Saving vision board "${visionBoard.title}" to individual XML file...');
      
      // Ensure the vision board has a valid ID
      VisionBoard boardToSave = visionBoard;
      if (visionBoard.id.isEmpty) {
        boardToSave = visionBoard.copyWith(
          id: 'board_${DateTime.now().millisecondsSinceEpoch}',
        );
        print('🆔 Generated new ID for vision board: ${boardToSave.id}');
      }
      
      // Get individual file path for this vision board
      final filePath = await _getVisionBoardXmlFilePath(boardToSave.id);
      final file = File(filePath);
      
      // Create XML document for this single vision board
      final xmlDoc = _createVisionBoardXMLDocument(boardToSave, userId);
      
      // Save to individual file
      await file.writeAsString(xmlDoc.toXmlString(pretty: true));
      print('✅ Vision board saved to individual XML file: $filePath');
      
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
  
  /// Load all vision boards from individual XML files
  static Future<List<VisionBoard>> loadVisionBoardsFromXML() async {
    try {
      print('📖 Loading vision boards from individual XML files...');
      
      final directory = await getApplicationDocumentsDirectory();
      final directoryPath = directory.path;
      
      // Find all vision board XML files
      final dir = Directory(directoryPath);
      final visionBoards = <VisionBoard>[];
      
      if (!await dir.exists()) {
        print('📄 Directory not found, returning empty list');
        return [];
      }
      
      // Look for files that match the pattern: visionboard_*.xml
      await for (final entity in dir.list()) {
        if (entity is File && entity.path.contains('visionboard_') && entity.path.endsWith('.xml')) {
          try {
            print('📄 Found vision board XML file: ${entity.path}');
            
            final xmlString = await entity.readAsString();
            final xmlDoc = XmlDocument.parse(xmlString);
            final rootElement = xmlDoc.rootElement;
            
            // Each individual file should contain exactly one VisionBoard
            for (final boardElement in rootElement.children.whereType<XmlElement>()) {
              if (boardElement.name.local == 'VisionBoard') {
                final visionBoard = _xmlElementToVisionBoard(boardElement);
                visionBoards.add(visionBoard);
                break; // Only one board per file
              }
            }
          } catch (e) {
            print('⚠️ Error reading vision board file ${entity.path}: $e');
            // Continue with other files
          }
        }
      }
      
      // Sort by creation date (newest first)
      visionBoards.sort((a, b) => b.createdAt.compareTo(a.createdAt));
      
      print('✅ Loaded ${visionBoards.length} vision boards from individual XML files');
      return visionBoards;
      
    } catch (e) {
      print('❌ Error loading vision boards from XML files: $e');
      return [];
    }
  }
  
  /// Load a specific vision board by ID from its individual XML file
  static Future<VisionBoard?> loadVisionBoardFromXML(String visionBoardId) async {
    try {
      print('📖 Loading vision board $visionBoardId from individual XML file...');
      
      final filePath = await _getVisionBoardXmlFilePath(visionBoardId);
      final file = File(filePath);
      
      if (!await file.exists()) {
        print('📄 Vision board XML file not found: $filePath');
        return null;
      }
      
      final xmlString = await file.readAsString();
      final xmlDoc = XmlDocument.parse(xmlString);
      final rootElement = xmlDoc.rootElement;
      
      // Find the vision board element (should be only one)
      for (final boardElement in rootElement.children.whereType<XmlElement>()) {
        if (boardElement.name.local == 'VisionBoard') {
          final visionBoard = _xmlElementToVisionBoard(boardElement);
          print('✅ Loaded vision board "${visionBoard.title}" from XML file');
          return visionBoard;
        }
      }
      
      print('⚠️ No VisionBoard element found in XML file');
      return null;
      
    } catch (e) {
      print('❌ Error loading vision board from XML: $e');
      return null;
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
  
  /// Delete a vision board by removing its individual XML file
  static Future<void> deleteVisionBoardFromXML(String visionBoardId) async {
    try {
      print('🗑️ Deleting vision board $visionBoardId by removing its XML file...');
      
      final filePath = await _getVisionBoardXmlFilePath(visionBoardId);
      final file = File(filePath);
      
      if (!await file.exists()) {
        print('📄 Vision board XML file not found, nothing to delete: $filePath');
        return;
      }
      
      // Simply delete the individual XML file
      await file.delete();
      print('✅ Vision board XML file deleted successfully: $filePath');
      
    } catch (e) {
      print('❌ Error deleting vision board XML file: $e');
      throw Exception('Failed to delete vision board XML file: $e');
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
  
  /// Upload all individual vision board XML files to database
  static Future<bool> uploadAllXMLToDatabase({int? userId}) async {
    try {
      print('☁️ Uploading all individual vision board XML files to database...');
      
      final directory = await getApplicationDocumentsDirectory();
      final directoryPath = directory.path;
      final dir = Directory(directoryPath);
      
      if (!await dir.exists()) {
        print('📄 No XML files directory found');
        return false;
      }
      
      final visionBoardService = VisionBoardService();
      await visionBoardService.initialize(baseUrl: 'http://manifestation-qa-1.senthilguru.net');
      
      int uploadedCount = 0;
      int totalFiles = 0;
      
      // Upload each individual XML file
      await for (final entity in dir.list()) {
        if (entity is File && entity.path.contains('visionboard_') && entity.path.endsWith('.xml')) {
          totalFiles++;
          try {
            // Extract vision board ID from filename
            final fileName = entity.path.split('/').last;
            final visionBoardId = fileName.replaceAll('visionboard_', '').replaceAll('.xml', '');
            
            final xmlContent = await entity.readAsString();
            
            final result = await visionBoardService.uploadVisionBoardXML(
              visionBoardId: visionBoardId,
              xmlContent: xmlContent,
              userId: userId ?? 1,
            );
            
            if (result.isSuccess) {
              print('✅ Uploaded vision board $visionBoardId with picture ID: ${result.data}');
              uploadedCount++;
            } else {
              print('❌ Failed to upload vision board $visionBoardId: ${result.message}');
            }
            
          } catch (e) {
            print('❌ Error uploading vision board file ${entity.path}: $e');
          }
        }
      }
      
      print('📊 Upload summary: $uploadedCount/$totalFiles files uploaded successfully');
      return uploadedCount > 0;
      
    } catch (e) {
      print('❌ Error uploading XML files to database: $e');
      return false;
    }
  }
  
  /// Upload a specific vision board XML file to database
  static Future<bool> uploadVisionBoardXMLToDatabase(String visionBoardId, {int? userId}) async {
    try {
      print('☁️ Uploading vision board $visionBoardId XML to database...');
      
      final filePath = await _getVisionBoardXmlFilePath(visionBoardId);
      final file = File(filePath);
      
      if (!await file.exists()) {
        print('📄 Vision board XML file not found: $filePath');
        return false;
      }
      
      final xmlContent = await file.readAsString();
      
      // ===== XML CONTENT VALIDATION LOGGING =====
      print('📋 XML FILE ANALYSIS:');
      print('  - File Path: $filePath');
      print('  - File Exists: ${await file.exists()}');
      print('  - File Size: ${await file.length()} bytes');
      print('  - XML Content Length: ${xmlContent.length} characters');
      print('  - XML Content Valid: ${xmlContent.isNotEmpty && xmlContent.trim().startsWith('<?xml')}');
      final xmlPreview = xmlContent.length > 500 ? '${xmlContent.substring(0, 500)}...' : xmlContent;
      print('  - XML Preview (first 500 chars): $xmlPreview');
      
      final visionBoardService = VisionBoardService();
      await visionBoardService.initialize(baseUrl: 'http://manifestation-qa-1.senthilguru.net');
      
      final result = await visionBoardService.uploadVisionBoardXML(
        visionBoardId: visionBoardId,
        xmlContent: xmlContent,
        userId: userId ?? 1,
      );
      
      if (result.isSuccess) {
        print('✅ Vision board XML uploaded successfully with picture ID: ${result.data}');
        return true;
      } else {
        print('❌ Vision board XML upload failed: ${result.message}');
        return false;
      }
      
    } catch (e) {
      print('❌ Error uploading vision board XML to database: $e');
      return false;
    }
  }
  
  /// Download all vision board XML files from database
  static Future<bool> downloadAllXMLFromDatabase(int userId) async {
    try {
      print('☁️ Downloading all vision board XML files from database...');
      
      final visionBoardService = VisionBoardService();
      await visionBoardService.initialize(baseUrl: 'http://manifestation-qa-1.senthilguru.net');
      
      // Get list of all XML files for the user
      final listResult = await visionBoardService.getVisionBoardXMLList(userId);
      
      if (!listResult.isSuccess || listResult.data == null || listResult.data!.isEmpty) {
        print('⚠️ No XML files found on server: ${listResult.message}');
        return false;
      }
      
      int downloadedCount = 0;
      final totalFiles = listResult.data!.length;
      
      // Download each XML file individually
      for (final xmlInfo in listResult.data!) {
        try {
          final downloadResult = await visionBoardService.downloadVisionBoardXML(xmlInfo.pictureId);
          
          if (downloadResult.isSuccess && downloadResult.data != null) {
            // Save to individual XML file
            final filePath = await _getVisionBoardXmlFilePath(xmlInfo.visionBoardId);
            final file = File(filePath);
            await file.writeAsString(downloadResult.data!);
            
            print('✅ Downloaded vision board ${xmlInfo.visionBoardId} to: $filePath');
            downloadedCount++;
          } else {
            print('❌ Failed to download vision board ${xmlInfo.visionBoardId}: ${downloadResult.message}');
          }
          
        } catch (e) {
          print('❌ Error downloading vision board ${xmlInfo.visionBoardId}: $e');
        }
      }
      
      print('📊 Download summary: $downloadedCount/$totalFiles files downloaded successfully');
      return downloadedCount > 0;
      
    } catch (e) {
      print('❌ Error downloading XML files from database: $e');
      return false;
    }
  }
  
  /// Download a specific vision board XML file from database
  static Future<bool> downloadVisionBoardXMLFromDatabase(String visionBoardId, int userId) async {
    try {
      print('☁️ Downloading vision board $visionBoardId XML from database...');
      
      final visionBoardService = VisionBoardService();
      await visionBoardService.initialize(baseUrl: 'http://manifestation-qa-1.senthilguru.net');
      
      final result = await visionBoardService.downloadVisionBoardXMLById(
        visionBoardId: visionBoardId,
        userId: userId,
      );
      
      if (result.isSuccess && result.data != null) {
        final filePath = await _getVisionBoardXmlFilePath(visionBoardId);
        final file = File(filePath);
        await file.writeAsString(result.data!);
        print('✅ Vision board XML downloaded successfully to: $filePath');
        return true;
      } else {
        print('⚠️ ${result.message}');
        return false;
      }
      
    } catch (e) {
      print('❌ Error downloading vision board XML from database: $e');
      return false;
    }
  }
  
  /// Get list of all vision board XML files for a user
  static Future<List<VisionBoardXMLInfo>> getVisionBoardXMLList(int userId) async {
    try {
      print('📋 Getting vision board XML list from database...');
      
      final visionBoardService = VisionBoardService();
      await visionBoardService.initialize(baseUrl: 'http://manifestation-qa-1.senthilguru.net');
      
      final result = await visionBoardService.getVisionBoardXMLList(userId);
      
      if (result.isSuccess && result.data != null) {
        print('✅ Found ${result.data!.length} XML files');
        return result.data!;
      } else {
        print('⚠️ ${result.message}');
        return [];
      }
      
    } catch (e) {
      print('❌ Error getting XML list from database: $e');
      return [];
    }
  }
  
  /// Download specific vision board XML by ID
  static Future<String?> downloadVisionBoardXMLById({
    required String visionBoardId,
    required int userId,
  }) async {
    try {
      print('📥 Downloading specific vision board XML: $visionBoardId');
      
      final visionBoardService = VisionBoardService();
      await visionBoardService.initialize(baseUrl: 'http://manifestation-qa-1.senthilguru.net');
      
      final result = await visionBoardService.downloadVisionBoardXMLById(
        visionBoardId: visionBoardId,
        userId: userId,
      );
      
      if (result.isSuccess && result.data != null) {
        print('✅ Vision board XML downloaded successfully');
        return result.data;
      } else {
        print('⚠️ ${result.message}');
        return null;
      }
      
    } catch (e) {
      print('❌ Error downloading vision board XML by ID: $e');
      return null;
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
  
  /// Get the last modified timestamp from a specific vision board XML file
  static Future<DateTime?> getVisionBoardXMLTimestamp(String visionBoardId) async {
    try {
      final filePath = await _getVisionBoardXmlFilePath(visionBoardId);
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
      print('❌ Error getting vision board XML timestamp: $e');
      return null;
    }
  }
  
  /// Get the most recent timestamp from all local vision board XML files
  static Future<DateTime?> getLatestLocalXMLTimestamp() async {
    try {
      final directory = await getApplicationDocumentsDirectory();
      final directoryPath = directory.path;
      final dir = Directory(directoryPath);
      
      if (!await dir.exists()) {
        return null;
      }
      
      DateTime? latestTimestamp;
      
      // Check all vision board XML files
      await for (final entity in dir.list()) {
        if (entity is File && entity.path.contains('visionboard_') && entity.path.endsWith('.xml')) {
          try {
            final xmlString = await entity.readAsString();
            final xmlDoc = XmlDocument.parse(xmlString);
            final lastModified = xmlDoc.rootElement.getAttribute('lastModified');
            
            if (lastModified != null) {
              final timestamp = DateTime.parse(lastModified);
              if (latestTimestamp == null || timestamp.isAfter(latestTimestamp)) {
                latestTimestamp = timestamp;
              }
            }
          } catch (e) {
            // Continue with other files
          }
        }
      }
      
      return latestTimestamp;
    } catch (e) {
      print('❌ Error getting latest XML timestamp: $e');
      return null;
    }
  }

}

/// Extension to add firstOrNull method
extension FirstOrNull<T> on Iterable<T> {
  T? get firstOrNull => isEmpty ? null : first;
}