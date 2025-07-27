import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'screens/splash_screen.dart';
import 'constants/colors.dart';
import 'services/unified_auth_service.dart';
import 'services/vision_board_service.dart';
import 'widgets/session_activity_tracker.dart';

void main() async {
  WidgetsFlutterBinding.ensureInitialized();
  
  // Initialize unified auth service first and wait for it to complete
  await UnifiedAuthService().initialize(baseUrl: 'http://manifestation-qa-1.senthilguru.net');
  
  // Initialize VisionBoardService (this depends on auth being ready)
  await VisionBoardService().initialize(baseUrl: 'http://manifestation-qa-1.senthilguru.net');
  
  print('🚀 All services initialized, starting app with cache-only mode');
  
  runApp(ManifestationApp());
}

class ManifestationApp extends StatefulWidget {
  @override
  _ManifestationAppState createState() => _ManifestationAppState();
}

class _ManifestationAppState extends State<ManifestationApp> {
  @override
  void dispose() {
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    // Set system UI overlay style
    SystemChrome.setSystemUIOverlayStyle(
      SystemUiOverlayStyle(
        statusBarColor: AppColors.transparent,
        statusBarIconBrightness: Brightness.light,
        systemNavigationBarColor: AppColors.backgroundDark,
        systemNavigationBarIconBrightness: Brightness.light,
      ),
    );

    return ScreenUtilInit(
      designSize: Size(430, 932), // iPhone 15 Pro Max dimensions
      minTextAdapt: true,
      splitScreenMode: true,
      builder: (context, child) {
        return SessionActivityTracker(
          child: MaterialApp(
          title: 'Manifestation App',
          debugShowCheckedModeBanner: false,
          theme: ThemeData(
            primarySwatch: Colors.purple,
            scaffoldBackgroundColor: AppColors.backgroundDark,
            fontFamily: 'SF Pro Display', // iOS default font
            textTheme: TextTheme(
              headlineLarge: TextStyle(
                color: AppColors.textLight,
                fontSize: 32.sp,
                fontWeight: FontWeight.bold,
              ),
              headlineMedium: TextStyle(
                color: AppColors.textLight,
                fontSize: 24.sp,
                fontWeight: FontWeight.w600,
              ),
              bodyLarge: TextStyle(
                color: AppColors.textMedium,
                fontSize: 16.sp,
                fontWeight: FontWeight.normal,
              ),
              bodyMedium: TextStyle(
                color: AppColors.textMedium,
                fontSize: 14.sp,
                fontWeight: FontWeight.normal,
              ),
            ),
            // Custom theme for form fields
            inputDecorationTheme: InputDecorationTheme(
              filled: true,
              fillColor: AppColors.cardBackground,
              border: OutlineInputBorder(
                borderRadius: BorderRadius.circular(16.r),
                borderSide: BorderSide.none,
              ),
              enabledBorder: OutlineInputBorder(
                borderRadius: BorderRadius.circular(16.r),
                borderSide: BorderSide(
                  color: AppColors.textMedium.withOpacity(0.1),
                  width: 1,
                ),
              ),
              focusedBorder: OutlineInputBorder(
                borderRadius: BorderRadius.circular(16.r),
                borderSide: BorderSide(
                  color: AppColors.affirmationGradient[0],
                  width: 2,
                ),
              ),
              errorBorder: OutlineInputBorder(
                borderRadius: BorderRadius.circular(16.r),
                borderSide: BorderSide(
                  color: Colors.red,
                  width: 1,
                ),
              ),
              contentPadding: EdgeInsets.symmetric(
                horizontal: 16.w,
                vertical: 16.h,
              ),
            ),
            // Custom theme for elevated buttons
            elevatedButtonTheme: ElevatedButtonThemeData(
              style: ElevatedButton.styleFrom(
                elevation: 0,
                shadowColor: Colors.transparent,
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(16.r),
                ),
                textStyle: TextStyle(
                  fontSize: 16.sp,
                  fontWeight: FontWeight.w600,
                ),
              ),
            ),
            // Custom theme for outlined buttons
            outlinedButtonTheme: OutlinedButtonThemeData(
              style: OutlinedButton.styleFrom(
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(16.r),
                ),
                side: BorderSide(
                  color: AppColors.textMedium.withOpacity(0.3),
                  width: 1,
                ),
                textStyle: TextStyle(
                  fontSize: 16.sp,
                  fontWeight: FontWeight.w600,
                ),
              ),
            ),
            // Custom theme for text buttons
            textButtonTheme: TextButtonThemeData(
              style: TextButton.styleFrom(
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(12.r),
                ),
                textStyle: TextStyle(
                  fontSize: 16.sp,
                  fontWeight: FontWeight.w600,
                ),
              ),
            ),
            // Custom theme for checkboxes
            checkboxTheme: CheckboxThemeData(
              fillColor: MaterialStateProperty.resolveWith((states) {
                if (states.contains(MaterialState.selected)) {
                  return AppColors.affirmationGradient[0];
                }
                return Colors.transparent;
              }),
              checkColor: MaterialStateProperty.all(AppColors.white),
              side: BorderSide(
                color: AppColors.textMedium.withOpacity(0.5),
                width: 2,
              ),
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(4.r),
              ),
            ),
            // Custom theme for dialogs
            dialogTheme: DialogThemeData(
              backgroundColor: AppColors.cardBackground,
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(16.r),
              ),
              titleTextStyle: TextStyle(
                color: AppColors.textLight,
                fontSize: 18.sp,
                fontWeight: FontWeight.bold,
              ),
              contentTextStyle: TextStyle(
                color: AppColors.textMedium,
                fontSize: 14.sp,
              ),
            ),
            // Custom theme for snack bars
            snackBarTheme: SnackBarThemeData(
              behavior: SnackBarBehavior.floating,
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(12.r),
              ),
              contentTextStyle: TextStyle(
                fontSize: 14.sp,
                fontWeight: FontWeight.w500,
              ),
            ),
          ),
          // Start with the welcome screen
          home: SplashScreen(),
          ),
        );
      },
    );
  }
}