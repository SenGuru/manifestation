import 'dart:io';
import 'dart:ui' as ui;
import 'package:flutter/material.dart';
import 'package:flutter/rendering.dart';
import 'package:flutter/services.dart';
import 'constants/app_theme.dart';

void main() {
  runApp(IconGeneratorApp());
}

class IconGeneratorApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      home: IconGeneratorScreen(),
    );
  }
}

class IconGeneratorScreen extends StatefulWidget {
  @override
  _IconGeneratorScreenState createState() => _IconGeneratorScreenState();
}

class _IconGeneratorScreenState extends State<IconGeneratorScreen> {
  final GlobalKey _globalKey = GlobalKey();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.black,
      appBar: AppBar(
        title: Text('Manifestation Icon Generator'),
        backgroundColor: Colors.black,
        foregroundColor: Colors.white,
      ),
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            // This is the exact icon from your splash screen
            RepaintBoundary(
              key: _globalKey,
              child: Container(
                width: 512,
                height: 512,
                color: Colors.transparent,
                child: Container(
                  width: 512,
                  height: 512,
                  decoration: BoxDecoration(
                    gradient: LinearGradient(
                      colors: AppTheme.affirmationGradient,
                      begin: Alignment.topLeft,
                      end: Alignment.bottomRight,
                    ),
                    shape: BoxShape.circle,
                  ),
                  child: Icon(
                    Icons.auto_awesome,
                    color: Colors.white,
                    size: 256,
                  ),
                ),
              ),
            ),
            SizedBox(height: 40),
            ElevatedButton(
              onPressed: _captureAndSaveIcon,
              style: ElevatedButton.styleFrom(
                backgroundColor: AppTheme.affirmationGradient[0],
                foregroundColor: Colors.white,
                padding: EdgeInsets.symmetric(horizontal: 32, vertical: 16),
              ),
              child: Text(
                'Generate & Save Icon',
                style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
              ),
            ),
          ],
        ),
      ),
    );
  }

  Future<void> _captureAndSaveIcon() async {
    try {
      RenderRepaintBoundary boundary = _globalKey.currentContext!
          .findRenderObject() as RenderRepaintBoundary;
      
      // Generate at high resolution for app stores
      ui.Image image = await boundary.toImage(pixelRatio: 1.0);
      
      var byteData = await image.toByteData(format: ui.ImageByteFormat.png);
      var buffer = byteData!.buffer.asUint8List();

      // Save to current directory (project folder)
      final currentDir = Directory.current.path;
      final file = File('$currentDir/manifestation_icon.png');
      await file.writeAsBytes(buffer);
      
      print('✅ Icon saved to: ${file.path}');
      
      // Show success dialog
      showDialog(
        context: context,
        builder: (context) => AlertDialog(
          title: Text('Success! 🎉'),
          content: Text('App icon saved to:\n${file.path}\n\nThis 512x512 PNG is optimized for app stores.'),
          actions: [
            TextButton(
              onPressed: () => Navigator.pop(context),
              child: Text('Perfect!'),
            ),
          ],
        ),
      );
    } catch (e) {
      print('❌ Error generating icon: $e');
      showDialog(
        context: context,
        builder: (context) => AlertDialog(
          title: Text('Error'),
          content: Text('Failed to generate icon: $e'),
          actions: [
            TextButton(
              onPressed: () => Navigator.pop(context),
              child: Text('OK'),
            ),
          ],
        ),
      );
    }
  }
}