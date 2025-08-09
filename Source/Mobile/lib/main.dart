import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:google_fonts/google_fonts.dart';
import 'screens/splash_screen.dart';
import 'constants/colors.dart';
import 'constants/app_theme.dart';
import 'services/unified_auth_service.dart';
import 'services/vision_board_service.dart';
import 'services/theme_service.dart';
import 'services/subscription_service.dart';
import 'widgets/session_activity_tracker.dart';

void main() async {
  WidgetsFlutterBinding.ensureInitialized();
  
  // Lock app to portrait orientation only
  await SystemChrome.setPreferredOrientations([
    DeviceOrientation.portraitUp,
    DeviceOrientation.portraitDown,
  ]);
  
  // Initialize unified auth service first and wait for it to complete
  await UnifiedAuthService().initialize(baseUrl: 'http://manifestation-qa-1.senthilguru.net');
  
  // Initialize VisionBoardService (this depends on auth being ready)
  await VisionBoardService().initialize(baseUrl: 'http://manifestation-qa-1.senthilguru.net');
  
  // Initialize theme service
  await ThemeService().initialize();
  
  // Initialize subscription service
  await SubscriptionService().initialize();
  
  // Initialize in-app purchase service (don't wait for this as it might fail on simulators)
  print('🚀 All services initialized, starting app with cache-only mode');
  
  runApp(ManifestationApp());
}

class ManifestationApp extends StatefulWidget {
  @override
  _ManifestationAppState createState() => _ManifestationAppState();
}

class _ManifestationAppState extends State<ManifestationApp> {
  final ThemeService _themeService = ThemeService();
  
  @override
  void initState() {
    super.initState();
    _themeService.addListener(_onThemeChanged);
  }
  
  @override
  void dispose() {
    _themeService.removeListener(_onThemeChanged);
    super.dispose();
  }
  
  void _onThemeChanged() {
    setState(() {
      // Rebuild with new theme
    });
  }

  @override
  Widget build(BuildContext context) {
    // Set system UI overlay style based on current theme
    SystemChrome.setSystemUIOverlayStyle(
      SystemUiOverlayStyle(
        statusBarColor: AppTheme.transparent,
        statusBarIconBrightness: AppTheme.isDarkMode ? Brightness.light : Brightness.dark,
        systemNavigationBarColor: AppTheme.backgroundPrimary,
        systemNavigationBarIconBrightness: AppTheme.isDarkMode ? Brightness.light : Brightness.dark,
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
          theme: AppTheme.themeData.copyWith(
            textTheme: GoogleFonts.poppinsTextTheme(
              TextTheme(
              headlineLarge: TextStyle(
                color: AppTheme.textPrimary,
                fontSize: 32.sp,
                fontWeight: FontWeight.bold,
              ),
              headlineMedium: TextStyle(
                color: AppTheme.textPrimary,
                fontSize: 24.sp,
                fontWeight: FontWeight.w600,
              ),
              bodyLarge: TextStyle(
                color: AppTheme.textSecondary,
                fontSize: 16.sp,
                fontWeight: FontWeight.normal,
              ),
              bodyMedium: TextStyle(
                color: AppTheme.textSecondary,
                fontSize: 14.sp,
                fontWeight: FontWeight.normal,
              ),
            ).apply(
              bodyColor: AppTheme.textSecondary,
              displayColor: AppTheme.textPrimary,
            ),
            ),
            // Custom theme for form fields
            inputDecorationTheme: InputDecorationTheme(
              filled: true,
              fillColor: AppTheme.inputBackground,
              border: OutlineInputBorder(
                borderRadius: BorderRadius.circular(16.r),
                borderSide: BorderSide.none,
              ),
              enabledBorder: OutlineInputBorder(
                borderRadius: BorderRadius.circular(16.r),
                borderSide: BorderSide(
                  color: AppTheme.inputBorder,
                  width: 1,
                ),
              ),
              focusedBorder: OutlineInputBorder(
                borderRadius: BorderRadius.circular(16.r),
                borderSide: BorderSide(
                  color: AppTheme.affirmationGradient[0],
                  width: 2,
                ),
              ),
              errorBorder: OutlineInputBorder(
                borderRadius: BorderRadius.circular(16.r),
                borderSide: BorderSide(
                  color: AppTheme.error,
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
                  color: AppTheme.borderPrimary,
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
                  return AppTheme.affirmationGradient[0];
                }
                return Colors.transparent;
              }),
              checkColor: MaterialStateProperty.all(AppTheme.white),
              side: BorderSide(
                color: AppTheme.textSecondary.withOpacity(0.5),
                width: 2,
              ),
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(4.r),
              ),
            ),
            // Custom theme for dialogs
            dialogTheme: DialogThemeData(
              backgroundColor: AppTheme.cardBackground,
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(16.r),
              ),
              titleTextStyle: TextStyle(
                color: AppTheme.textPrimary,
                fontSize: 18.sp,
                fontWeight: FontWeight.bold,
              ),
              contentTextStyle: TextStyle(
                color: AppTheme.textSecondary,
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