import 'dart:async';

import 'package:flutter/material.dart';
import 'package:flutter_app/bloc/auth/auth_bloc.dart';
import 'package:flutter_app/bloc/navigation/navigation_bloc.dart';
import 'package:flutter_app/framework/enum/app_enum.dart';
import 'package:flutter_app/framework/infrastructure/log/logger_service.dart';
import 'package:flutter_app/framework/services/api_service.dart';
import 'package:flutter_app/framework/services/app_session_service.dart';
import 'package:flutter_app/home_page.dart';
import 'package:flutter_app/locator/locator_setup.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_easyloading/flutter_easyloading.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:get_it/get_it.dart';
import 'package:google_fonts/google_fonts.dart';

final GlobalKey<NavigatorState> navigatorKey = GlobalKey<NavigatorState>();

Future<void> main() async {
  final LoggerService loggerService = LoggerService();
  final GetIt getIt = GetIt.instance;
  //  catch all framework errors
  FlutterError.onError = (FlutterErrorDetails error) {
    loggerService.writeLog(
        "Flutter Error", LogMessageLevel.error, error.exception, error.stack);
  };

// catch uncatch dart errors
  runZonedGuarded(() {
    WidgetsFlutterBinding.ensureInitialized();
    setupLocator();
    AppSessionService.init(); // init shared preference
    runApp(ScreenUtilInit(
      designSize: const Size(360, 690),
      builder: (context, child) => MaterialApp(
        theme: ThemeData(
          textTheme: GoogleFonts.poppinsTextTheme(Theme.of(context).textTheme)
              .copyWith(
            bodyMedium: GoogleFonts.poppins(
                textStyle: Theme.of(context).textTheme.bodyMedium),
          ),
        ),
        builder: EasyLoading.init(),
        navigatorKey: navigatorKey,
        home: MultiBlocProvider(
          providers: [
            // auth bloc
            BlocProvider<AuthBloc>(
              create: (context) => AuthBloc(getIt<ApiService>(),
                  getIt<AppSessionService>(), getIt<LoggerService>()),
            ),
            // navigation bloc
            BlocProvider(
              create: (context) => NavigationBloc(),
            ),
          ],
          child: HomePage(loggerService: loggerService),
        ),
      ),
    ));
  }, (error, stackTrace) {
    loggerService.writeLog(
        "Flutter Error", LogMessageLevel.error, error, stackTrace);
  });
  loggerService.writeLog("Application Started", LogMessageLevel.info);
}
