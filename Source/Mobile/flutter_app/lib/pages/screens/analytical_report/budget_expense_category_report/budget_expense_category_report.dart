import 'package:flimi_project/api/generated/code/filmicall.swagger.dart';
import 'package:flimi_project/common_widget/dialogs/error_dialog.dart';
import 'package:flimi_project/common_widget/styles/content_style.dart';
import 'package:flimi_project/constant/constant.dart';
import 'package:flimi_project/framework/enum/app_enum.dart';
import 'package:flimi_project/framework/infrastructure/log/logger_service.dart';
import 'package:flimi_project/framework/services/api_service.dart';
import 'package:flimi_project/models/app_custom_model.dart';
import 'package:flimi_project/pages/fl_chart/chart_view/fl_pie_chart_widget.dart';
import 'package:flimi_project/pages/screens/analytical_report/budget_expense_category_report/budget_expense_category_report_filter_widget.dart';
import 'package:flimi_project/screen_utils/color_utils.dart';
import 'package:flimi_project/themedata/themecolor.dart';
import 'package:flutter/material.dart';
import 'package:flutter_easyloading/flutter_easyloading.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:get_it/get_it.dart';

class BudgetExpenseCategoryReport extends StatefulWidget {
  final int movieId;
  final int companyId;
  final int predefinedCurrencyTypeId;
  const BudgetExpenseCategoryReport(
      {super.key,
      required this.movieId,
      required this.predefinedCurrencyTypeId,
      required this.companyId});

  @override
  State<BudgetExpenseCategoryReport> createState() =>
      _BudgetExpenseCategoryReportState();
}

class _BudgetExpenseCategoryReportState
    extends State<BudgetExpenseCategoryReport> {
  GetIt getIt = GetIt.instance;
  late ApiService _apiService;
  late UserModel _userModel;
  late LoggerService _loggerService;
  List<MovieBudgetExpenseSummaryPieChartContainer>?
      movieBudgetExpenseCategorySummaryList;
  double totalBudgetAmount = 0;
  double totalExpenseAmount = 0;
  double totalPaidAmount = 0;
  bool? isFilterActive;
  int? selectedPredefinedBudgetDivisionTypeId;

  @override
  void initState() {
    super.initState();
    _apiService = getIt<ApiService>();
    _loggerService = getIt<LoggerService>();

    WidgetsBinding.instance.addPostFrameCallback((_) async {
      await getMovieBudgetExpenseSummary(movieId: widget.movieId);
    });
  }

  void onResetMovieBudgetSummary() async {
    if (mounted) {
      setState(() {
        isFilterActive = false;
        selectedPredefinedBudgetDivisionTypeId = null;
      });
    }
    await getMovieBudgetExpenseSummary(movieId: widget.movieId);
  }

  Future<void> getMovieBudgetExpenseSummary(
      {required int movieId, int? predefinedBudgetDivisionTypeId}) async {
    if (predefinedBudgetDivisionTypeId == null) {
      await getMovieBudgetExpenseSummaryByPredefinedDivisionTypeId(
          movieId: movieId);
    } else {
      await getMovieBudgetExpenseSummaryByCategoryName(
          movieId: movieId,
          predefinedBudgetDivisionTypeId: predefinedBudgetDivisionTypeId);
    }
  }

  Future<void> getMovieBudgetExpenseSummaryByPredefinedDivisionTypeId(
      {required int movieId, int? predefinedBudgetDivisionTypeId}) async {
    try {
      EasyLoading.show();
      if (mounted) {
        setState(() {
          selectedPredefinedBudgetDivisionTypeId =
              predefinedBudgetDivisionTypeId;
        });
      }

      var summaryResult = await _apiService
          .getConsolidatedMovieBudgetExpenseSummaryForAllMovieBudgetCategoryTypeByPredefinedBudgetDivisionTypeId(
              movieId: movieId,
              predefinedBudgetDivisionTypeId: predefinedBudgetDivisionTypeId);

      if (summaryResult == null) {
        _loggerService.writeLog(
            "getConsolidatedMovieBudgetExpenseSummaryForAllMovieBudgetCategoryTypeByPredefinedBudgetDivisionTypeId: Unable to get movie budget expense summary",
            LogMessageLevel.error);
        showErrorDialog(context, "Unable to get movie budget expense summary");

        return;
      } else if (!summaryResult.success) {
        _loggerService.writeLog(
            "getConsolidatedMovieBudgetExpenseSummaryForAllMovieBudgetCategoryTypeByPredefinedBudgetDivisionTypeId: no data found - ${summaryResult.errorMsg}",
            LogMessageLevel.error);
        showErrorDialog(context,
            "No movie budget expense summary data found: ${summaryResult.errorMsg}");
        return;
      }
      var budgetExpenseSummaryList = summaryResult.result?.model ?? [];
      if (mounted) {
        setState(() {
          movieBudgetExpenseCategorySummaryList = [];
          totalBudgetAmount = 0;
          totalExpenseAmount = 0;
          totalPaidAmount = 0;

          for (var i = 0; i < budgetExpenseSummaryList.length; i++) {
            var summary = budgetExpenseSummaryList[i];
            var container = MovieBudgetExpenseSummaryPieChartContainer(
                categoryColor: ColorUtils.getRandomColor(i),
                totalBudgetAmount: summary.totalBudgetAmount ?? 0,
                totalExpenseAmount: summary.totalExpenseAmount ?? 0,
                totalPaidAmount: summary.totalPaidAmount ?? 0,
                sectionName:
                    summary.budgetCategoryTypeName ?? "Not Valid Name");
            movieBudgetExpenseCategorySummaryList?.add(container);
            // calculate budget amount
            totalBudgetAmount += summary.totalBudgetAmount ?? 0;
            totalExpenseAmount += summary.totalExpenseAmount ?? 0;
            totalPaidAmount += summary.totalPaidAmount ?? 0;
          }
        });
      }
    } catch (e, stackTrace) {
      _loggerService.writeLog(
          "getConsolidatedMovieBudgetExpenseSummaryForAllPredefinedBudgetDivisionTypes: Unable to get movie budget expense summary",
          LogMessageLevel.error,
          e,
          stackTrace);
      showErrorDialog(context, "Unable to get movie budget expense summary");
    } finally {
      EasyLoading.dismiss();
    }
  }

  Future<void> getMovieBudgetExpenseSummaryByCategoryName(
      {required int movieId, int? predefinedBudgetDivisionTypeId}) async {
    try {
      EasyLoading.show();
      if (mounted) {
        setState(() {
          selectedPredefinedBudgetDivisionTypeId =
              predefinedBudgetDivisionTypeId;
        });
      }

      var summaryResult = await _apiService
          .getConsolidatedMovieBudgetExpenseSummaryByBudgetCategoryName(
              movieId: movieId,
              predefinedBudgetDivisionTypeId: predefinedBudgetDivisionTypeId);

      if (summaryResult == null) {
        _loggerService.writeLog(
            "getConsolidatedMovieBudgetExpenseSummaryForAllMovieBudgetCategoryTypeByPredefinedBudgetDivisionTypeId: Unable to get movie budget expense summary",
            LogMessageLevel.error);
        showErrorDialog(context, "Unable to get movie budget expense summary");

        return;
      } else if (!summaryResult.success) {
        _loggerService.writeLog(
            "getConsolidatedMovieBudgetExpenseSummaryForAllMovieBudgetCategoryTypeByPredefinedBudgetDivisionTypeId: no data found - ${summaryResult.errorMsg}",
            LogMessageLevel.error);
        showErrorDialog(context,
            "No movie budget expense summary data found: ${summaryResult.errorMsg}");
        return;
      }
      var budgetExpenseSummaryList = summaryResult.result?.model ?? [];
      if (mounted) {
        setState(() {
          movieBudgetExpenseCategorySummaryList = [];
          totalBudgetAmount = 0;
          totalExpenseAmount = 0;
          totalPaidAmount = 0;

          for (var i = 0; i < budgetExpenseSummaryList.length; i++) {
            var summary = budgetExpenseSummaryList[i];
            var container = MovieBudgetExpenseSummaryPieChartContainer(
                categoryColor: ColorUtils.getRandomColor(i),
                totalBudgetAmount: summary.totalBudgetAmount ?? 0,
                totalExpenseAmount: summary.totalExpenseAmount ?? 0,
                totalPaidAmount: summary.totalPaidAmount ?? 0,
                sectionName: summary.budgetCategoryName ?? "Not Valid Name");
            movieBudgetExpenseCategorySummaryList?.add(container);
            // calculate budget amount
            totalBudgetAmount += summary.totalBudgetAmount ?? 0;
            totalExpenseAmount += summary.totalExpenseAmount ?? 0;
            totalPaidAmount += summary.totalPaidAmount ?? 0;
          }
        });
      }
    } catch (e, stackTrace) {
      _loggerService.writeLog(
          "getConsolidatedMovieBudgetExpenseSummaryForAllPredefinedBudgetDivisionTypes: Unable to get movie budget expense summary",
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
          leadingWidth: 30.w,
          titleTextStyle: TextStyle(
              fontSize: 20,
              color: ThemeColor.mainThemeColor,
              overflow: TextOverflow.ellipsis),
          title: Row(
            crossAxisAlignment: CrossAxisAlignment.center,
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              const Text("Category Report"),
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
                      showBudgetExpenseCategoryFilterWidget(
                        context: context,
                        predefinedBudgetDivisionTypeId:
                            selectedPredefinedBudgetDivisionTypeId,
                        onResetFilter: () {
                          onResetMovieBudgetSummary();
                        },
                        movieId: widget.movieId,
                        companyId: widget.companyId,
                        onSubmitFilter: (
                            {selectedPredefinedBudgetDivisionTypeId}) {
                          isFilterActive = true;
                          getMovieBudgetExpenseSummary(
                            movieId: widget.movieId,
                            predefinedBudgetDivisionTypeId:
                                selectedPredefinedBudgetDivisionTypeId,
                          );
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
        body: SingleChildScrollView(
            padding: ContentStyle.contentSmallPadding,
            child: Container(
              width: 300.w,
              child: DefaultTabController(
                length: 3,
                child: Column(
                  children: <Widget>[
                    Container(
                      padding: const EdgeInsets.fromLTRB(10, 0, 10, 0),
                      child: TabBar(
                        tabAlignment: TabAlignment.start,
                        onTap: (value) {},
                        isScrollable: true,
                        indicatorSize: TabBarIndicatorSize.label,
                        labelColor: Colors.white,
                        indicator: BoxDecoration(
                            borderRadius: BorderRadius.all(Radius.circular(12)),
                            color: ThemeColor.mainThemeColor),
                        indicatorPadding:
                            const EdgeInsets.fromLTRB(-10, 10, -10, 10),
                        labelPadding: const EdgeInsets.symmetric(
                            horizontal: 10.0, vertical: 2),
                        splashBorderRadius:
                            const BorderRadius.all(Radius.circular(12)),
                        dividerColor: Colors.transparent,
                        tabs: const [
                          Tab(text: "Budget"),
                          Tab(child: Text(" Expense")),
                          Tab(child: Text(" Paid")),
                        ],
                      ),
                    ),
                    SizedBox(
                      height: 1.sh, // Set a fixed height
                      child: TabBarView(
                        children: [
                          FlPieChartWidget(
                            movieBudgetExpenseCategoryList:
                                movieBudgetExpenseCategorySummaryList,
                            totalAmountValue: totalBudgetAmount,
                            pieChartViewTypeId: PieChartViewTypes.budgetView,
                            totalBudgetAmountValue: totalBudgetAmount,
                            totalExpenseAmountValue: totalExpenseAmount,
                            totalPaidAmountValue: totalPaidAmount,
                            chartTitle: "Budget",
                            predefinedCurrencyTypeId:
                                widget.predefinedCurrencyTypeId,
                          ),
                          FlPieChartWidget(
                            movieBudgetExpenseCategoryList:
                                movieBudgetExpenseCategorySummaryList,
                            totalAmountValue: totalExpenseAmount,
                            chartTitle: "Expense",
                            totalBudgetAmountValue: totalBudgetAmount,
                            totalExpenseAmountValue: totalExpenseAmount,
                            totalPaidAmountValue: totalPaidAmount,
                            pieChartViewTypeId: PieChartViewTypes.expenseView,
                            predefinedCurrencyTypeId:
                                widget.predefinedCurrencyTypeId,
                          ),
                          FlPieChartWidget(
                            movieBudgetExpenseCategoryList:
                                movieBudgetExpenseCategorySummaryList,
                            totalAmountValue: totalPaidAmount,
                            totalBudgetAmountValue: totalBudgetAmount,
                            totalExpenseAmountValue: totalExpenseAmount,
                            totalPaidAmountValue: totalPaidAmount,
                            predefinedCurrencyTypeId:
                                widget.predefinedCurrencyTypeId,
                            pieChartViewTypeId: PieChartViewTypes.paidView,
                            chartTitle: "Paid",
                          ),
                        ],
                      ),
                    )
                  ],
                ),
              ),
            )));
  }
}
