import 'package:dropdown_button2/dropdown_button2.dart';
import 'package:flimi_project/api/services/select-service.dart';
import 'package:flimi_project/common_widget/styles/button_style.dart';
import 'package:flimi_project/framework/infrastructure/log/logger_service.dart';
import 'package:flimi_project/framework/model/framework_model.dart';
import 'package:flimi_project/framework/services/api_service.dart';
import 'package:flimi_project/models/app-enums.dart';
import 'package:flimi_project/themedata/custom_text_scaler.dart';
import 'package:flimi_project/themedata/themecolor.dart';
import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:get_it/get_it.dart';

typedef OnTabMovieShootDayBudgetSubmitFilter = void Function({
  int? selectedPredefinedBudgetOrExpenseTypeId,
  int? selectedPredefinedBudgetDivisionId,
});
typedef OnTabCastAndCrewReportResetFilter = void Function();
Future<dynamic> showBudgetExpenseCastAndCrewFilterWidget({
  required BuildContext context,
  required OnTabMovieShootDayBudgetSubmitFilter onSubmitFilter,
  required OnTabCastAndCrewReportResetFilter onResetFilter,
  required int? companyId,
  required int? movieId,
  required int? predefinedBudgetOrExpenseTypeId,
  required int? predefinedBudgetDivisionId,
}) {
  return showModalBottomSheet(
      isScrollControlled: true,
      context: context,
      builder: (builder) {
        return Container(
          color: ThemeColor.white,
          height: 0.40.sh,
          width: 1.sw,
          child: MovieShootDayBudgetFilterWidget(
            movieId: movieId,
            companyId: companyId,
            selectedPredefinedBudgetOrExpenseTypeId:
                predefinedBudgetOrExpenseTypeId,
            selectedPredefinedBudgetDivisionTypeId: predefinedBudgetDivisionId,
            onSubmitFilter: ({
              selectedPredefinedBudgetOrExpenseTypeId,
              selectedPredefinedBudgetDivisionId,
            }) {
              onSubmitFilter(
                selectedPredefinedBudgetOrExpenseTypeId:
                    selectedPredefinedBudgetOrExpenseTypeId,
                selectedPredefinedBudgetDivisionId:
                    selectedPredefinedBudgetDivisionId,
              );
            },
            onResetFilter: () {
              onResetFilter();
            },
          ),
        );
      });
}

class MovieShootDayBudgetFilterWidget extends StatefulWidget {
  final OnTabMovieShootDayBudgetSubmitFilter onSubmitFilter;
  final int? companyId;
  final OnTabCastAndCrewReportResetFilter onResetFilter;
  final int? movieId;
  final int? selectedPredefinedBudgetOrExpenseTypeId;
  final int? selectedPredefinedBudgetDivisionTypeId;
  const MovieShootDayBudgetFilterWidget({
    super.key,
    required this.onSubmitFilter,
    required this.onResetFilter,
    required this.companyId,
    required this.movieId,
    required this.selectedPredefinedBudgetOrExpenseTypeId,
    required this.selectedPredefinedBudgetDivisionTypeId,
  });

  @override
  State<MovieShootDayBudgetFilterWidget> createState() =>
      _MovieShootDayBudgetFilterWidgetState();
}

class _MovieShootDayBudgetFilterWidgetState
    extends State<MovieShootDayBudgetFilterWidget> {
  GetIt getIt = GetIt.instance;
  late final ApiService _apiService;
  late final LoggerService _loggerService;
  late final SelectService _selectService;

  List<Status>? predefinedBudgetOrExpenseTypes = [];
  Status? selectedPredefinedBudgetOrExpenseType;
  List<Status>? predefineBudgetDivisions = [];
  Status? selectedPredefinedBudgetDivision;

  @override
  initState() {
    super.initState();
    _apiService = getIt<ApiService>();
    _loggerService = getIt<LoggerService>();
    _selectService = getIt<SelectService>();

    predefinedBudgetOrExpenseTypes =
        _selectService.GetPredefinedBudgetOrExpenseTypes()
            .where((item) =>
                (item.id != PredefinedBudgetOrExpenseTypes.custom.value))
            .toList();
    predefineBudgetDivisions = _selectService.GetPredefinedBudgetDivisionTypes()
        .where((item) =>
            (item.id == PredefinedBudgetDivisionTypes.preProduction.value) ||
            (item.id == PredefinedBudgetDivisionTypes.production.value) ||
            (item.id == PredefinedBudgetDivisionTypes.postProduction.value) ||
            (item.id == PredefinedBudgetDivisionTypes.others.value))
        .toList();

    if (widget.selectedPredefinedBudgetOrExpenseTypeId != null)
      selectedPredefinedBudgetOrExpenseType =
          predefinedBudgetOrExpenseTypes?.firstWhere((item) =>
              item.id == widget.selectedPredefinedBudgetOrExpenseTypeId);
    if (widget.selectedPredefinedBudgetDivisionTypeId != null)
      selectedPredefinedBudgetDivision = predefineBudgetDivisions?.firstWhere(
          (item) => item.id == widget.selectedPredefinedBudgetDivisionTypeId);
  }

  applyFilter() {
    widget.onSubmitFilter(
      selectedPredefinedBudgetOrExpenseTypeId:
          selectedPredefinedBudgetOrExpenseType?.id,
      selectedPredefinedBudgetDivisionId: selectedPredefinedBudgetDivision?.id,
    );
    Navigator.pop(context);
  }

  resetFilter() {
    setState(() {
      selectedPredefinedBudgetOrExpenseType = null;
      selectedPredefinedBudgetDivision = null;
    });
    widget.onResetFilter();
    Navigator.pop(context);
  }

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.all(20),
      child: Column(
        mainAxisAlignment: MainAxisAlignment.spaceBetween,
        children: [
          Expanded(
              child: SingleChildScrollView(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                SizedBox(
                  height: 20.h,
                ),
                Text("Type"),
                SizedBox(height: 10.h),
                DropdownButtonFormField2<Status>(
                  isExpanded: false,
                  buttonStyleData: const ButtonStyleData(
                    padding: EdgeInsets.only(right: 8),
                  ),
                  iconStyleData: const IconStyleData(
                    icon: Icon(
                      Icons.arrow_drop_down,
                      color: Colors.black45,
                    ),
                    iconSize: 24,
                  ),
                  dropdownStyleData: DropdownStyleData(
                    maxHeight: 200.h,
                    decoration: BoxDecoration(
                      borderRadius: BorderRadius.circular(15),
                    ),
                  ),
                  menuItemStyleData: const MenuItemStyleData(
                    padding: EdgeInsets.symmetric(horizontal: 16),
                  ),
                  decoration: InputDecoration(
                    contentPadding: const EdgeInsets.symmetric(vertical: 16),
                    border: OutlineInputBorder(
                      borderRadius: BorderRadius.circular(15),
                    ),
                  ),
                  alignment: Alignment.centerLeft,
                  value: selectedPredefinedBudgetOrExpenseType,
                  hint: const Text("Select Type"),
                  items: predefinedBudgetOrExpenseTypes?.map((item) {
                    return DropdownMenuItem<Status>(
                      value: item,
                      alignment: AlignmentDirectional.centerStart,
                      child: Text(
                        item.type ?? "",
                        style: const TextStyle(
                          fontSize: 14,
                          fontWeight: FontWeight.bold,
                          color: Colors.black87,
                          overflow: TextOverflow.ellipsis,
                        ),
                      ),
                    );
                  }).toList(),
                  onChanged: (Status? newValue) {
                    setState(() {
                      selectedPredefinedBudgetOrExpenseType = newValue;
                    });
                  },
                ),
                SizedBox(
                  height: 20.h,
                ),
                Text("Stage"),
                SizedBox(height: 10.h),
                DropdownButtonFormField2<Status>(
                  isExpanded: false,
                  buttonStyleData: const ButtonStyleData(
                    padding: EdgeInsets.only(right: 8),
                  ),
                  iconStyleData: const IconStyleData(
                    icon: Icon(
                      Icons.arrow_drop_down,
                      color: Colors.black45,
                    ),
                    iconSize: 24,
                  ),
                  dropdownStyleData: DropdownStyleData(
                    maxHeight: 200.h,
                    decoration: BoxDecoration(
                      borderRadius: BorderRadius.circular(15),
                    ),
                  ),
                  menuItemStyleData: const MenuItemStyleData(
                    padding: EdgeInsets.symmetric(horizontal: 16),
                  ),
                  decoration: InputDecoration(
                    contentPadding: const EdgeInsets.symmetric(vertical: 16),
                    border: OutlineInputBorder(
                      borderRadius: BorderRadius.circular(15),
                    ),
                  ),
                  alignment: Alignment.centerLeft,
                  value: selectedPredefinedBudgetDivision,
                  hint: const Text("Select Budget Division"),
                  items: predefineBudgetDivisions?.map((item) {
                    return DropdownMenuItem<Status>(
                      value: item,
                      alignment: AlignmentDirectional.centerStart,
                      child: Text(
                        item.type ?? "",
                        style: const TextStyle(
                          fontSize: 14,
                          fontWeight: FontWeight.bold,
                          color: Colors.black87,
                          overflow: TextOverflow.ellipsis,
                        ),
                      ),
                    );
                  }).toList(),
                  onChanged: (Status? newValue) {
                    setState(() {
                      selectedPredefinedBudgetDivision = newValue;
                    });
                  },
                ),
                SizedBox(
                  height: 20.h,
                ),
              ],
            ),
          )),
          Row(
            crossAxisAlignment: CrossAxisAlignment.start,
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              TextButton(
                style: cancelButtonStyle(),
                onPressed: () {
                  resetFilter();
                },
                child: const Text(
                  "Reset",
                  textScaler: CustomTextScaler.headerTwo,
                  style: TextStyle(color: Colors.black87),
                ),
              ),
              SizedBox(
                width: 10.w,
              ),
              TextButton(
                style: submitButtonStyle(),
                onPressed: () {
                  applyFilter();
                },
                child: const Text(
                  "Apply Filter",
                  textScaler: CustomTextScaler.headerTwo,
                  style: TextStyle(color: Colors.white),
                ),
              )
            ],
          )
        ],
      ),
    );
  }
}
