import 'package:fl_chart/fl_chart.dart';
import 'package:flimi_project/api/generated/code/filmicall.swagger.dart';
import 'package:flimi_project/api/services/enum_badge_service.dart';
import 'package:flimi_project/api/services/enum_text_color_service.dart';
import 'package:flimi_project/common_widget/dialogs/error_dialog.dart';
import 'package:flimi_project/common_widget/form_fields/placeholder/generic_label_with_value_widget.dart';
import 'package:flimi_project/common_widget/no_data/no_data_found_widget.dart';
import 'package:flimi_project/common_widget/styles/content_style.dart';
import 'package:flimi_project/constant/assets.dart';
import 'package:flimi_project/framework/enum/app_enum.dart';
import 'package:flimi_project/framework/infrastructure/log/logger_service.dart';
import 'package:flimi_project/framework/services/api_service.dart';
import 'package:flimi_project/pages/screens/analytical_report/budget_expense_cast_crew_report/budget_expense_cast_crew_report_filter_widget.dart';
import 'package:flimi_project/screen_utils/currency_utils.dart';
import 'package:flimi_project/themedata/custom_text_scaler.dart';
import 'package:flutter/material.dart';
import 'package:flutter_easyloading/flutter_easyloading.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:get_it/get_it.dart';

import '../../../../themedata/themecolor.dart';

class BudgetExpenseCastAndCrewReport extends StatefulWidget {
  final int movieId;
  final int companyId;
  final int predefinedCurrencyTypeId;
  const BudgetExpenseCastAndCrewReport(
      {super.key,
      required this.movieId,
      required this.predefinedCurrencyTypeId,
      required this.companyId});

  @override
  State<BudgetExpenseCastAndCrewReport> createState() =>
      _BudgetExpenseCastAndCrewReportState();
}

class _BudgetExpenseCastAndCrewReportState
    extends State<BudgetExpenseCastAndCrewReport> {
  GetIt getIt = GetIt.instance;
  late ApiService _apiService;
  late LoggerService _loggerService;
  double maxAmountValue = 0;
  double totalBudgetAmount = 0;
  double totalExpenseAmount = 0;
  double totalPaidAmount = 0;
  late EnumBadgeService _enumBadgeService;
  late EnumTextColorService _enumTextColorService;
  int touchedGroupIndex = -1;
  final double barRodWidth = 20;
  List<BarChartGroupData> rawBarGroups = [];
  List<BarChartGroupData> showingBarGroups = [];
  bool? isFilterActive;
  int? selectedPredefinedBudgetDivisionTypeId;
  int? selectedPredefinedBudgetOrExpenseTypeId;

  List<MovieResourceBudgetExpenseSummaryModel> movieResourceBudgetExpenseList =
      [];
  List<String> titles = [];

  @override
  void initState() {
    super.initState();
    _apiService = getIt<ApiService>();
    _enumTextColorService = getIt<EnumTextColorService>();
    _loggerService = getIt<LoggerService>();
    _enumBadgeService = getIt<EnumBadgeService>();
    isFilterActive = false;
    WidgetsBinding.instance.addPostFrameCallback((_) async {
      await getMovieBudgetExpenseSummary(movieId: widget.movieId);
    });
  }

  void onResetMovieBudgetSummary() async {
    if (mounted) {
      setState(() {
        isFilterActive = false;
        selectedPredefinedBudgetDivisionTypeId = null;
        selectedPredefinedBudgetOrExpenseTypeId = null;
      });
    }
    await getMovieBudgetExpenseSummary(movieId: widget.movieId);
  }

  Future<void> getMovieBudgetExpenseSummary(
      {required int movieId,
      int? predefinedBudgetDivisionTypeId,
      int? predefinedBudgetOrExpenseTypeId}) async {
    try {
      EasyLoading.show();
      if (mounted) {
        setState(() {
          selectedPredefinedBudgetDivisionTypeId =
              predefinedBudgetDivisionTypeId;
          selectedPredefinedBudgetOrExpenseTypeId =
              predefinedBudgetOrExpenseTypeId;
        });
      }
      var summaryResult = await _apiService
          .getConsolidatedBudgetExpenseReportForAllMovieResource(
        movieId: movieId,
        predefinedBudgetDivisionTypeId: predefinedBudgetDivisionTypeId,
        predefinedBudgetOrExpenseTypeId: predefinedBudgetOrExpenseTypeId,
      );
      if (summaryResult == null) {
        _loggerService.writeLog(
            "getConsolidatedBudgetExpenseReportForAllMovieResource: Unable to get movie budget expense summary",
            LogMessageLevel.error);
        showErrorDialog(context, "Unable to get movie budget expense summary");

        return;
      } else if (!summaryResult.success) {
        _loggerService.writeLog(
            "getConsolidatedBudgetExpenseReportForAllMovieResource: no data found - ${summaryResult.errorMsg}",
            LogMessageLevel.error);
        showErrorDialog(context,
            "No movie budget expense summary data found: ${summaryResult.errorMsg}");
        return;
      }
      if (mounted) {
        setState(() {
          movieResourceBudgetExpenseList = summaryResult.result?.model ?? [];
          totalBudgetAmount = 0;
          totalExpenseAmount = 0;
          totalPaidAmount = 0;

          for (var summary in movieResourceBudgetExpenseList) {
            totalBudgetAmount += summary.budgetTotalAmount ?? 0;
            totalExpenseAmount += summary.expenseTotalAmount ?? 0;
            totalPaidAmount += summary.paidTotalAmount ?? 0;
          }
        });
      }
    } catch (e, stackTrace) {
      _loggerService.writeLog(
          "getConsolidatedBudgetExpenseReportForAllMovieResource: Unable to get movie budget expense summary",
          LogMessageLevel.error,
          e,
          stackTrace);
      showErrorDialog(context, "Unable to get movie budget expense summary");
    } finally {
      EasyLoading.dismiss();
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          backgroundColor: Colors.transparent,
          titleTextStyle: TextStyle(
              fontSize: 20,
              color: ThemeColor.mainThemeColor,
              overflow: TextOverflow.ellipsis),
          leadingWidth: 30.w,
          title: Row(
            crossAxisAlignment: CrossAxisAlignment.center,
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              const Text("Cast And Crew Report"),
              Container(
                decoration: BoxDecoration(
                    color: isFilterActive == true
                        ? ThemeColor.mainThemeColor
                        : Colors.transparent,
                    borderRadius: const BorderRadius.all(Radius.circular(10))),
                width: 40.w,
                height: 30.h,
                child: IconButton(
                    onPressed: () {
                      showBudgetExpenseCastAndCrewFilterWidget(
                        context: context,
                        predefinedBudgetOrExpenseTypeId:
                            selectedPredefinedBudgetOrExpenseTypeId,
                        predefinedBudgetDivisionId:
                            selectedPredefinedBudgetDivisionTypeId,
                        onResetFilter: () {
                          onResetMovieBudgetSummary();
                        },
                        movieId: widget.movieId,
                        companyId: widget.companyId,
                        onSubmitFilter: (
                            {selectedPredefinedBudgetDivisionId,
                            selectedPredefinedBudgetOrExpenseTypeId}) {
                          {
                            isFilterActive = true;
                            getMovieBudgetExpenseSummary(
                              movieId: widget.movieId,
                              predefinedBudgetOrExpenseTypeId:
                                  selectedPredefinedBudgetOrExpenseTypeId,
                              predefinedBudgetDivisionTypeId:
                                  selectedPredefinedBudgetDivisionId,
                            );
                          }
                        },
                      );
                    },
                    icon: Icon(
                      Icons.filter_alt_outlined,
                      color: isFilterActive == true
                          ? Colors.white
                          : Colors.black87,
                    )),
              ),
            ],
          ),
          leading: TextButton(
              onPressed: () {
                Navigator.pop(context);
              },
              child: const Icon(Icons.keyboard_arrow_left_outlined)),
        ),
        backgroundColor: ThemeColor.white,
        body: Column(
          children: [
            if (movieResourceBudgetExpenseList.isNotEmpty) ...[
              Container(
                padding: ContentStyle.contentSmallPadding,
                height: 0.7.sh,
                child: SingleChildScrollView(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    mainAxisAlignment: MainAxisAlignment.start,
                    children: [
                      for (var data in movieResourceBudgetExpenseList) ...[
                        Container(
                          padding: ContentStyle.contentSmallPadding,
                          child: Container(
                              decoration: BoxDecoration(
                                  borderRadius: BorderRadius.circular(20)),
                              child: Row(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                mainAxisAlignment: MainAxisAlignment.start,
                                children: [
                                  Expanded(
                                    child: Column(
                                      crossAxisAlignment:
                                          CrossAxisAlignment.center,
                                      mainAxisAlignment:
                                          MainAxisAlignment.start,
                                      children: [
                                        Container(
                                            padding: ContentStyle
                                                .contentSmallPadding,
                                            decoration: ContentStyle
                                                .contentBoxDecoration,
                                            child: Row(
                                              mainAxisAlignment:
                                                  MainAxisAlignment
                                                      .spaceBetween,
                                              children: [
                                                Row(
                                                  crossAxisAlignment:
                                                      CrossAxisAlignment.center,
                                                  children: [
                                                    SizedBox(
                                                      width: 200.w,
                                                      child: Column(
                                                        mainAxisAlignment:
                                                            MainAxisAlignment
                                                                .start,
                                                        crossAxisAlignment:
                                                            CrossAxisAlignment
                                                                .start,
                                                        children: [
                                                          Text(
                                                            data.movieResourceName ??
                                                                "",
                                                            textScaler:
                                                                CustomTextScaler
                                                                    .headerThree,
                                                            style:
                                                                const TextStyle(
                                                              color:
                                                                  Colors.black,
                                                              fontWeight:
                                                                  FontWeight
                                                                      .bold,
                                                            ),
                                                            overflow:
                                                                TextOverflow
                                                                    .ellipsis,
                                                          ),
                                                          SizedBox(
                                                            height: 3.h,
                                                          ),
                                                          Row(
                                                            children: [
                                                              Text(
                                                                "Status: ",
                                                                textScaler:
                                                                    CustomTextScaler
                                                                        .normalText,
                                                                style:
                                                                    TextStyle(
                                                                  color: ThemeColor
                                                                      .darkGrey,
                                                                  fontWeight:
                                                                      FontWeight
                                                                          .w600,
                                                                ),
                                                                overflow:
                                                                    TextOverflow
                                                                        .ellipsis,
                                                              ),
                                                              _enumTextColorService
                                                                  .getPredefinedBudgetOrExpenseTypeText(
                                                                      data.predefinedBudgetOrExpenseTypeId ??
                                                                          0),
                                                            ],
                                                          ),
                                                        ],
                                                      ),
                                                    ),
                                                  ],
                                                ),
                                                TextButton(
                                                    onPressed: () {
                                                      //print(widget.movieSceneModel!.);
                                                    },
                                                    child: Icon(
                                                      Icons
                                                          .keyboard_arrow_right,
                                                    ))
                                              ],
                                            )),
                                        Container(
                                            decoration: BoxDecoration(
                                              border: Border(
                                                  bottom: BorderSide(
                                                    color: ThemeColor.lightGrey,
                                                  ),
                                                  left: BorderSide(
                                                      color:
                                                          ThemeColor.lightGrey),
                                                  right: BorderSide(
                                                      color: ThemeColor
                                                          .lightGrey)),
                                              borderRadius: BorderRadius.only(
                                                bottomLeft: Radius.circular(10),
                                                bottomRight:
                                                    Radius.circular(10),
                                              ),
                                              color: Colors.white,
                                            ),
                                            child: Column(
                                              children: [
                                                Row(
                                                  mainAxisAlignment:
                                                      MainAxisAlignment.start,
                                                  children: [
                                                    Container(
                                                      padding:
                                                          EdgeInsets.fromLTRB(
                                                              10.w,
                                                              10.h,
                                                              1.w,
                                                              10.h),
                                                      width: 300.w,
                                                      child: Column(
                                                        mainAxisAlignment:
                                                            MainAxisAlignment
                                                                .start,
                                                        crossAxisAlignment:
                                                            CrossAxisAlignment
                                                                .start,
                                                        children: [
                                                          SizedBox(
                                                            height: 10,
                                                          ),
                                                          Row(
                                                            mainAxisAlignment:
                                                                MainAxisAlignment
                                                                    .spaceBetween,
                                                            children: [
                                                              GenericLabelAndValueWidget(
                                                                titleText:
                                                                    "Stage",
                                                                imageSrc:
                                                                    AppImageAssets
                                                                        .movieSceneImage,
                                                                subText: "",
                                                                subWidget:
                                                                    SizedBox(
                                                                  child: _enumBadgeService
                                                                      .getPredefinedBudgetDivisionTypeBadge(
                                                                          data.predefinedBudgetDivisionTypeId ??
                                                                              0),
                                                                ),
                                                              ),
                                                              GenericLabelAndValueWidget(
                                                                titleText:
                                                                    "Budget Amount",
                                                                imageSrc:
                                                                    AppImageAssets
                                                                        .locationImage,
                                                                subText: CurrencyConversionUtils.getFormattedCurrencyApproxValue(
                                                                    amount:
                                                                        data.budgetTotalAmount ??
                                                                            0,
                                                                    predefinedCurrencyTypeId:
                                                                        widget
                                                                            .predefinedCurrencyTypeId),
                                                              ),
                                                            ],
                                                          ),
                                                          Row(
                                                            mainAxisAlignment:
                                                                MainAxisAlignment
                                                                    .spaceBetween,
                                                            children: [
                                                              GenericLabelAndValueWidget(
                                                                titleText:
                                                                    "Expense Amount",
                                                                imageSrc:
                                                                    AppImageAssets
                                                                        .houseSvgIcon,
                                                                subText: CurrencyConversionUtils.getFormattedCurrencyApproxValue(
                                                                    amount:
                                                                        data.expenseTotalAmount ??
                                                                            0,
                                                                    predefinedCurrencyTypeId:
                                                                        widget
                                                                            .predefinedCurrencyTypeId),
                                                              ),
                                                              GenericLabelAndValueWidget(
                                                                  titleText:
                                                                      "Paid Amount",
                                                                  imageSrc:
                                                                      AppImageAssets
                                                                          .clockPngImage,
                                                                  subText: CurrencyConversionUtils.getFormattedCurrencyApproxValue(
                                                                      amount:
                                                                          data.paidTotalAmount ??
                                                                              0,
                                                                      predefinedCurrencyTypeId:
                                                                          widget
                                                                              .predefinedCurrencyTypeId)),
                                                            ],
                                                          ),
                                                        ],
                                                      ),
                                                    ),
                                                  ],
                                                ),
                                              ],
                                            ))
                                      ],
                                    ),
                                  )
                                ],
                              )),
                        )
                      ]
                    ],
                  ),
                ),
              ),
              Container(
                  padding: ContentStyle.contentSmallPadding,
                  child: Column(
                    children: <Widget>[
                      SizedBox(
                        height: 10.h,
                      ),
                      Row(
                        crossAxisAlignment: CrossAxisAlignment.center,
                        children: <Widget>[
                          SizedBox(
                            width: 20.w,
                          ),
                          Container(
                            child: Row(
                              children: [
                                SizedBox(
                                  width: 10.w,
                                ),
                                Text(
                                  "Total Budget Amount : ${CurrencyConversionUtils.getFormattedCurrencyApproxValue(amount: totalBudgetAmount, predefinedCurrencyTypeId: widget.predefinedCurrencyTypeId)}",
                                  textScaler: CustomTextScaler.headerThree,
                                  style: TextStyle(
                                    fontWeight: FontWeight.bold,
                                  ),
                                ),
                                const Divider(
                                  color: Colors.black,
                                  thickness: 2,
                                ),
                              ],
                            ),
                          ),
                        ],
                      ),
                      SizedBox(
                        height: 8.h,
                      ),
                      Row(
                        crossAxisAlignment: CrossAxisAlignment.center,
                        children: [
                          SizedBox(
                            width: 20.w,
                          ),
                          Container(
                            child: Row(
                              children: [
                                SizedBox(
                                  width: 10.w,
                                ),
                                Text(
                                  "Total Expense Amount : ${CurrencyConversionUtils.getFormattedCurrencyApproxValue(amount: totalExpenseAmount, predefinedCurrencyTypeId: widget.predefinedCurrencyTypeId)}",
                                  textScaler: CustomTextScaler.headerThree,
                                  style: TextStyle(
                                    fontWeight: FontWeight.bold,
                                  ),
                                ),
                              ],
                            ),
                          ),
                        ],
                      ),
                      SizedBox(
                        height: 8.h,
                      ),
                      Row(
                        crossAxisAlignment: CrossAxisAlignment.center,
                        children: [
                          SizedBox(
                            width: 20.w,
                          ),
                          Container(
                            child: Row(
                              children: [
                                SizedBox(
                                  width: 10.w,
                                ),
                                Text(
                                  "Total Paid Amount : ${CurrencyConversionUtils.getFormattedCurrencyApproxValue(amount: totalPaidAmount, predefinedCurrencyTypeId: widget.predefinedCurrencyTypeId)}",
                                  textScaler: CustomTextScaler.headerThree,
                                  style: TextStyle(
                                    fontWeight: FontWeight.bold,
                                  ),
                                ),
                              ],
                            ),
                          ),
                        ],
                      ),
                    ],
                  )),
            ] else ...[
              SizedBox(
                height: 150.h,
              ),
              NoDataFoundWidget(canShowMessage: true),
            ]
          ],
        ));
  }
}
