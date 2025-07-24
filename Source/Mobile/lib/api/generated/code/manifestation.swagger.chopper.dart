// dart format width=80
// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'manifestation.swagger.dart';

// **************************************************************************
// ChopperGenerator
// **************************************************************************

// coverage:ignore-file
// ignore_for_file: type=lint
final class _$Manifestation extends Manifestation {
  _$Manifestation([ChopperClient? client]) {
    if (client == null) return;
    this.client = client;
  }

  @override
  final Type definitionType = Manifestation;

  @override
  Future<Response<BooleanReturnModel>> _apiV1AccountPingPost() {
    final Uri $url = Uri.parse('/api/v1/account/ping');
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>> _apiV1AccountAuthorizedpingPost() {
    final Uri $url = Uri.parse('/api/v1/account/authorizedping');
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<LoggedInUserModelReturnModel>> _apiV1AccountLoginPost(
      {required LoginModel? body}) {
    final Uri $url = Uri.parse('/api/v1/account/login');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<LoggedInUserModelReturnModel,
        LoggedInUserModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>> _apiV1AccountUsersetnewpasswordPost(
      {required ResetPasswordRequestModel? body}) {
    final Uri $url = Uri.parse('/api/v1/account/usersetnewpassword');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1AccountUserrecoverforgotpasswordPost(
          {required RecoverPasswordRequestModel? body}) {
    final Uri $url = Uri.parse('/api/v1/account/userrecoverforgotpassword');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1AccountGenerateotpforanonymoususerPost(
          {required AnonymousLoginModel? body}) {
    final Uri $url = Uri.parse('/api/v1/account/generateotpforanonymoususer');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<GlobalConfigurationModelReturnModel>>
      _apiV1AccountGetsiteconfigurationGet({String? site}) {
    final Uri $url = Uri.parse('/api/v1/account/getsiteconfiguration');
    final Map<String, dynamic> $params = <String, dynamic>{'site': site};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<GlobalConfigurationModelReturnModel,
        GlobalConfigurationModelReturnModel>($request);
  }

  @override
  Future<Response<LoginOptionModelReturnModel>> _apiV1AccountGetloginoptionGet({
    String? userName,
    String? callbackUrl,
  }) {
    final Uri $url = Uri.parse('/api/v1/account/getloginoption');
    final Map<String, dynamic> $params = <String, dynamic>{
      'userName': userName,
      'callbackUrl': callbackUrl,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<LoginOptionModelReturnModel,
        LoginOptionModelReturnModel>($request);
  }

  @override
  Future<Response<LoggedInUserModelReturnModel>>
      _apiV1AccountAlternateloginPost({required AlternateLoginModel? body}) {
    final Uri $url = Uri.parse('/api/v1/account/alternatelogin');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<LoggedInUserModelReturnModel,
        LoggedInUserModelReturnModel>($request);
  }

  @override
  Future<Response<LoggedInUserModelReturnModel>>
      _apiV1AccountExternallogincallbackPost(
          {required ExternalLoginCallBackModel? body}) {
    final Uri $url = Uri.parse('/api/v1/account/externallogincallback');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<LoggedInUserModelReturnModel,
        LoggedInUserModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>> _apiV1AccountResetpasswordGet(
      {String? userName}) {
    final Uri $url = Uri.parse('/api/v1/account/resetpassword');
    final Map<String, dynamic> $params = <String, dynamic>{
      'userName': userName
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1AccountIschangepasswordsessionvalidGet({String? sessionId}) {
    final Uri $url = Uri.parse('/api/v1/account/ischangepasswordsessionvalid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'sessionId': sessionId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<LoggedInUserModelReturnModel>>
      _apiV1AccountChangepasswordPost(
          {required ChangeForgotPasswordModel? body}) {
    final Uri $url = Uri.parse('/api/v1/account/changepassword');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<LoggedInUserModelReturnModel,
        LoggedInUserModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>> _apiV1AccountForgotpasswordPost(
      {required ForgotPasswordModel? body}) {
    final Uri $url = Uri.parse('/api/v1/account/forgotpassword');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<LoggedInUserModelReturnModel>>
      _apiV1AccountChangecurrentpasswordPost(
          {required ChangePasswordModel? body}) {
    final Uri $url = Uri.parse('/api/v1/account/changecurrentpassword');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<LoggedInUserModelReturnModel,
        LoggedInUserModelReturnModel>($request);
  }

  @override
  Future<Response<String>> _apiV1AccountDesktopsiteerrorurlGet(
      {String? errorCode}) {
    final Uri $url = Uri.parse('/api/v1/account/desktopsiteerrorurl');
    final Map<String, dynamic> $params = <String, dynamic>{
      'errorCode': errorCode
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<String, String>($request);
  }

  @override
  Future<Response<String>> _apiV1AccountDesktopsiteexternalloginurlGet(
      {String? key}) {
    final Uri $url = Uri.parse('/api/v1/account/desktopsiteexternalloginurl');
    final Map<String, dynamic> $params = <String, dynamic>{'key': key};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<String, String>($request);
  }

  @override
  Future<Response<LoggedInUserModelReturnModel>> _apiV1AccountLoginwithtokenGet(
      {String? accessToken}) {
    final Uri $url = Uri.parse('/api/v1/account/loginwithtoken');
    final Map<String, dynamic> $params = <String, dynamic>{
      'access_token': accessToken
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<LoggedInUserModelReturnModel,
        LoggedInUserModelReturnModel>($request);
  }

  @override
  Future<Response<GlobalSettingModelReturnModel>>
      _apiV1AccountGlobalsettingsGet({String? subdomain}) {
    final Uri $url = Uri.parse('/api/v1/account/globalsettings');
    final Map<String, dynamic> $params = <String, dynamic>{
      'subdomain': subdomain
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<GlobalSettingModelReturnModel,
        GlobalSettingModelReturnModel>($request);
  }

  @override
  Future<Response<dynamic>> _apiV1AccountAzureadloginGet() {
    final Uri $url = Uri.parse('/api/v1/account/azureadlogin');
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
    );
    return client.send<dynamic, dynamic>($request);
  }

  @override
  Future<Response<dynamic>> _apiV1AccountAzureadcallbackGet() {
    final Uri $url = Uri.parse('/api/v1/account/azureadcallback');
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
    );
    return client.send<dynamic, dynamic>($request);
  }

  @override
  Future<Response<LoggedInUserModelReturnModel>> _apiV1AccountCreateaccountPost(
      {required AccountCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/account/createaccount');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<LoggedInUserModelReturnModel,
        LoggedInUserModelReturnModel>($request);
  }

  @override
  Future<Response<CaptchaUrlResponseModelReturnModel>>
      _apiV1AccountGeneratecaptchurlGet() {
    final Uri $url = Uri.parse('/api/v1/account/generatecaptchurl');
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
    );
    return client.send<CaptchaUrlResponseModelReturnModel,
        CaptchaUrlResponseModelReturnModel>($request);
  }

  @override
  Future<Response<AppLogModelReturnListModel>> _apiV1AdminutilsGetapplogsGet({
    List<String>? activityIds,
    String? machineName,
    String? userIdentity,
    String? appName,
    List<String>? severityLevels,
    List<String>? processIds,
    List<String>? threadIds,
    List<String>? currentFunctions,
    String? currentSourceLine,
    String? result,
    String? message,
    String? iPAddress,
    num? elaspedTime,
    DateTime? logDate,
    DateTime? startDate,
    DateTime? endDate,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/adminutils/getapplogs');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ActivityIds': activityIds,
      'MachineName': machineName,
      'UserIdentity': userIdentity,
      'AppName': appName,
      'SeverityLevels': severityLevels,
      'ProcessIds': processIds,
      'ThreadIds': threadIds,
      'CurrentFunctions': currentFunctions,
      'CurrentSourceLine': currentSourceLine,
      'Result': result,
      'Message': message,
      'IPAddress': iPAddress,
      'ElaspedTime': elaspedTime,
      'LogDate': logDate,
      'StartDate': startDate,
      'EndDate': endDate,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client
        .send<AppLogModelReturnListModel, AppLogModelReturnListModel>($request);
  }

  @override
  Future<Response<StringReturnListModel>>
      _apiV1AdminutilsGetloguniqueseveritytypesGet() {
    final Uri $url = Uri.parse('/api/v1/adminutils/getloguniqueseveritytypes');
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
    );
    return client.send<StringReturnListModel, StringReturnListModel>($request);
  }

  @override
  Future<Response<StringReturnListModel>>
      _apiV1AdminutilsGetloguniquefunctiontypesGet() {
    final Uri $url = Uri.parse('/api/v1/adminutils/getloguniquefunctiontypes');
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
    );
    return client.send<StringReturnListModel, StringReturnListModel>($request);
  }

  @override
  Future<Response<StringReturnListModel>>
      _apiV1AdminutilsGetloguniquefilenamesGet() {
    final Uri $url = Uri.parse('/api/v1/adminutils/getloguniquefilenames');
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
    );
    return client.send<StringReturnListModel, StringReturnListModel>($request);
  }

  @override
  Future<Response<PerfLogModelReturnListModel>> _apiV1AdminutilsGetperflogsGet({
    DateTime? logDate,
    List<String>? activityIds,
    String? machineName,
    String? userIdentity,
    String? appName,
    String? appModule,
    List<String>? appFunctions,
    String? httpVerb,
    String? statusCode,
    String? startTime,
    String? endTime,
    num? elapsedTimeMs,
    num? lesserThanMS,
    num? greaterThanMS,
    num? equalToMs,
    String? iPAddress,
    DateTime? startDate,
    DateTime? endDate,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/adminutils/getperflogs');
    final Map<String, dynamic> $params = <String, dynamic>{
      'LogDate': logDate,
      'ActivityIds': activityIds,
      'MachineName': machineName,
      'UserIdentity': userIdentity,
      'AppName': appName,
      'AppModule': appModule,
      'AppFunctions': appFunctions,
      'HttpVerb': httpVerb,
      'StatusCode': statusCode,
      'StartTime': startTime,
      'EndTime': endTime,
      'ElapsedTimeMs': elapsedTimeMs,
      'LesserThanMS': lesserThanMS,
      'GreaterThanMS': greaterThanMS,
      'EqualToMs': equalToMs,
      'IPAddress': iPAddress,
      'StartDate': startDate,
      'EndDate': endDate,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<PerfLogModelReturnListModel,
        PerfLogModelReturnListModel>($request);
  }

  @override
  Future<Response<PerfLogModelReturnListModel>>
      _apiV1AdminutilsGetappfunctionaggregatelogsGet({
    String? appFunction,
    num? lesserThanMedianTimeElapsed,
    num? greaterThanMedianTimeElapsed,
    num? lesserThanMeanTimeElapsed,
    num? greaterThanMeanTimeElapsed,
    num? lesserThanAppFunctionCount,
    num? greaterThanAppFunctionCount,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url =
        Uri.parse('/api/v1/adminutils/getappfunctionaggregatelogs');
    final Map<String, dynamic> $params = <String, dynamic>{
      'AppFunction': appFunction,
      'LesserThanMedianTimeElapsed': lesserThanMedianTimeElapsed,
      'GreaterThanMedianTimeElapsed': greaterThanMedianTimeElapsed,
      'LesserThanMeanTimeElapsed': lesserThanMeanTimeElapsed,
      'GreaterThanMeanTimeElapsed': greaterThanMeanTimeElapsed,
      'LesserThanAppFunctionCount': lesserThanAppFunctionCount,
      'GreaterThanAppFunctionCount': greaterThanAppFunctionCount,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<PerfLogModelReturnListModel,
        PerfLogModelReturnListModel>($request);
  }

  @override
  Future<Response<dynamic>>
      _apiV1BackendtestExportmoviesceneartistsinjsonlformatGet() {
    final Uri $url =
        Uri.parse('/api/v1/backendtest/exportmoviesceneartistsinjsonlformat');
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
    );
    return client.send<dynamic, dynamic>($request);
  }

  @override
  Future<Response<ChatModelReturnListModel>> _apiV1ChatGetchatsGet({
    bool? showDeleted,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/chat/getchats');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client
        .send<ChatModelReturnListModel, ChatModelReturnListModel>($request);
  }

  @override
  Future<Response<ChatModelReturnModel>> _apiV1ChatGetchatGet({int? id}) {
    final Uri $url = Uri.parse('/api/v1/chat/getchat');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<ChatModelReturnModel, ChatModelReturnModel>($request);
  }

  @override
  Future<Response<ChatModelReturnModel>> _apiV1ChatCreatechatPost(
      {required ChatCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/chat/createchat');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<ChatModelReturnModel, ChatModelReturnModel>($request);
  }

  @override
  Future<Response<ChatModelReturnListModel>> _apiV1ChatCreatemultiplechatPost(
      {required MultipleChatCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/chat/createmultiplechat');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client
        .send<ChatModelReturnListModel, ChatModelReturnListModel>($request);
  }

  @override
  Future<Response<ChatModelReturnModel>> _apiV1ChatUpdatechatPost(
      {required ChatUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/chat/updatechat');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<ChatModelReturnModel, ChatModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>> _apiV1ChatMarkchatasdeletedDelete(
      {int? id}) {
    final Uri $url = Uri.parse('/api/v1/chat/markchatasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>> _apiV1ChatMarkchatasnotdeletedPost(
      {required int? body}) {
    final Uri $url = Uri.parse('/api/v1/chat/markchatasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<ChatMessageModelReturnListModel>>
      _apiV1ChatGetchatmessagesGet({
    bool? showDeleted,
    int? userId,
    int? chatId,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/chat/getchatmessages');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'UserId': userId,
      'ChatId': chatId,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<ChatMessageModelReturnListModel,
        ChatMessageModelReturnListModel>($request);
  }

  @override
  Future<Response<ChatMessageModelReturnModel>> _apiV1ChatGetchatmessageGet(
      {int? id}) {
    final Uri $url = Uri.parse('/api/v1/chat/getchatmessage');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<ChatMessageModelReturnModel,
        ChatMessageModelReturnModel>($request);
  }

  @override
  Future<Response<ChatMessageModelReturnModel>> _apiV1ChatCreatechatmessagePost(
      {required ChatMessageCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/chat/createchatmessage');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<ChatMessageModelReturnModel,
        ChatMessageModelReturnModel>($request);
  }

  @override
  Future<Response<ChatMessageModelReturnListModel>>
      _apiV1ChatCreatemultiplechatmessagePost(
          {required MultipleChatMessageCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/chat/createmultiplechatmessage');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<ChatMessageModelReturnListModel,
        ChatMessageModelReturnListModel>($request);
  }

  @override
  Future<Response<ChatMessageModelReturnModel>> _apiV1ChatUpdatechatmessagePost(
      {required ChatMessageUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/chat/updatechatmessage');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<ChatMessageModelReturnModel,
        ChatMessageModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>> _apiV1ChatMarkchatmessageasdeletedDelete(
      {int? id}) {
    final Uri $url = Uri.parse('/api/v1/chat/markchatmessageasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ChatMarkchatmessageasnotdeletedPost({required int? body}) {
    final Uri $url = Uri.parse('/api/v1/chat/markchatmessageasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<ChatMessageStatusModelReturnListModel>>
      _apiV1ChatGetchatmessagestatussGet({
    bool? showDeleted,
    int? chatMessageId,
    int? userId,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/chat/getchatmessagestatuss');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'ChatMessageId': chatMessageId,
      'UserId': userId,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<ChatMessageStatusModelReturnListModel,
        ChatMessageStatusModelReturnListModel>($request);
  }

  @override
  Future<Response<ChatMessageStatusModelReturnModel>>
      _apiV1ChatGetchatmessagestatusGet({int? id}) {
    final Uri $url = Uri.parse('/api/v1/chat/getchatmessagestatus');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<ChatMessageStatusModelReturnModel,
        ChatMessageStatusModelReturnModel>($request);
  }

  @override
  Future<Response<ChatMessageStatusModelReturnModel>>
      _apiV1ChatCreatechatmessagestatusPost(
          {required ChatMessageStatusCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/chat/createchatmessagestatus');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<ChatMessageStatusModelReturnModel,
        ChatMessageStatusModelReturnModel>($request);
  }

  @override
  Future<Response<ChatMessageStatusModelReturnListModel>>
      _apiV1ChatCreatemultiplechatmessagestatusPost(
          {required MultipleChatMessageStatusCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/chat/createmultiplechatmessagestatus');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<ChatMessageStatusModelReturnListModel,
        ChatMessageStatusModelReturnListModel>($request);
  }

  @override
  Future<Response<ChatMessageStatusModelReturnModel>>
      _apiV1ChatUpdatechatmessagestatusPost(
          {required ChatMessageStatusUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/chat/updatechatmessagestatus');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<ChatMessageStatusModelReturnModel,
        ChatMessageStatusModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ChatMarkchatmessagestatusasdeletedDelete({int? id}) {
    final Uri $url = Uri.parse('/api/v1/chat/markchatmessagestatusasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ChatMarkchatmessagestatusasnotdeletedPost({required int? body}) {
    final Uri $url =
        Uri.parse('/api/v1/chat/markchatmessagestatusasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<ChatUserModelReturnListModel>> _apiV1ChatGetchatusersGet({
    bool? showDeleted,
    int? chatId,
    int? userId,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/chat/getchatusers');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'ChatId': chatId,
      'UserId': userId,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<ChatUserModelReturnListModel,
        ChatUserModelReturnListModel>($request);
  }

  @override
  Future<Response<ChatUserModelReturnModel>> _apiV1ChatGetchatuserGet(
      {int? id}) {
    final Uri $url = Uri.parse('/api/v1/chat/getchatuser');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client
        .send<ChatUserModelReturnModel, ChatUserModelReturnModel>($request);
  }

  @override
  Future<Response<ChatUserModelReturnModel>> _apiV1ChatCreatechatuserPost(
      {required ChatUserCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/chat/createchatuser');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client
        .send<ChatUserModelReturnModel, ChatUserModelReturnModel>($request);
  }

  @override
  Future<Response<ChatUserModelReturnListModel>>
      _apiV1ChatCreatemultiplechatuserPost(
          {required MultipleChatUserCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/chat/createmultiplechatuser');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<ChatUserModelReturnListModel,
        ChatUserModelReturnListModel>($request);
  }

  @override
  Future<Response<ChatUserModelReturnModel>> _apiV1ChatUpdatechatuserPost(
      {required ChatUserUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/chat/updatechatuser');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client
        .send<ChatUserModelReturnModel, ChatUserModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>> _apiV1ChatMarkchatuserasdeletedDelete(
      {int? id}) {
    final Uri $url = Uri.parse('/api/v1/chat/markchatuserasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>> _apiV1ChatMarkchatuserasnotdeletedPost(
      {required int? body}) {
    final Uri $url = Uri.parse('/api/v1/chat/markchatuserasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>> _apiV1CompanyUploaduserpicturePost({
    int? userId,
    List<int>? UploadedFile,
  }) {
    final Uri $url = Uri.parse('/api/v1/company/uploaduserpicture');
    final List<PartValue> $parts = <PartValue>[
      PartValue<int?>(
        'UserId',
        userId,
      ),
      PartValueFile<List<int>?>(
        'UploadedFile',
        UploadedFile,
      ),
    ];
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      parts: $parts,
      multipart: true,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>> _apiV1CompanyClearuserpictureDelete(
      {int? userId}) {
    final Uri $url = Uri.parse('/api/v1/company/clearuserpicture');
    final Map<String, dynamic> $params = <String, dynamic>{'userId': userId};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<dynamic>> _apiV1CompanyGetuserpictureGet({
    int? userId,
    String? profileGuid,
  }) {
    final Uri $url = Uri.parse('/api/v1/company/getuserpicture');
    final Map<String, dynamic> $params = <String, dynamic>{
      'userId': userId,
      'profileGuid': profileGuid,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<dynamic, dynamic>($request);
  }

  @override
  Future<Response<UserModelReturnListModel>> _apiV1CompanyGetusersGet({
    bool? active,
    List<int>? predefinedUserTypeIds,
    bool? showDeleted,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/company/getusers');
    final Map<String, dynamic> $params = <String, dynamic>{
      'Active': active,
      'PredefinedUserTypeIds': predefinedUserTypeIds,
      'ShowDeleted': showDeleted,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client
        .send<UserModelReturnListModel, UserModelReturnListModel>($request);
  }

  @override
  Future<Response<UserModelReturnModel>> _apiV1CompanyCreateuserPost({
    List<int>? UploadedFile,
    String? captchaCode,
    List<dynamic>? newPermissionTypeIds,
    String? initialPassword,
    String? companyCode,
    String? modifiedByUserFirstName,
    String? modifiedByUserLastName,
    String? modifiedByUserEmail,
    String? userGuid,
    String? userName,
    int? predefinedUserTypeId,
    String? passwordHash,
    DateTime? passwordChangedDate,
    String? email,
    String? firstName,
    String? lastName,
    String? workPhone,
    String? workPhoneExt,
    String? cellPhone,
    bool? active,
    bool? notificationThroughEmail,
    String? oneTimeToken,
    DateTime? oneTimeExpireDateTime,
    int? loginFailedAttempt,
    bool? isAccountLocked,
    DateTime? accountLockedUntil,
    String? newUserActivationGuid,
    DateTime? newUserActivationCompletedOn,
    String? profileImageGuid,
    String? profileImageLocation,
    String? profileImageActualFileName,
    bool? isDeleted,
  }) {
    final Uri $url = Uri.parse('/api/v1/company/createuser');
    final List<PartValue> $parts = <PartValue>[
      PartValue<String?>(
        'CaptchaCode',
        captchaCode,
      ),
      PartValue<List<dynamic>?>(
        'NewPermissionTypeIds',
        newPermissionTypeIds,
      ),
      PartValue<String?>(
        'InitialPassword',
        initialPassword,
      ),
      PartValue<String?>(
        'CompanyCode',
        companyCode,
      ),
      PartValue<String?>(
        'ModifiedByUserFirstName',
        modifiedByUserFirstName,
      ),
      PartValue<String?>(
        'ModifiedByUserLastName',
        modifiedByUserLastName,
      ),
      PartValue<String?>(
        'ModifiedByUserEmail',
        modifiedByUserEmail,
      ),
      PartValue<String?>(
        'UserGuid',
        userGuid,
      ),
      PartValue<String?>(
        'UserName',
        userName,
      ),
      PartValue<int?>(
        'PredefinedUserTypeId',
        predefinedUserTypeId,
      ),
      PartValue<String?>(
        'PasswordHash',
        passwordHash,
      ),
      PartValue<DateTime?>(
        'PasswordChangedDate',
        passwordChangedDate,
      ),
      PartValue<String?>(
        'Email',
        email,
      ),
      PartValue<String?>(
        'FirstName',
        firstName,
      ),
      PartValue<String?>(
        'LastName',
        lastName,
      ),
      PartValue<String?>(
        'WorkPhone',
        workPhone,
      ),
      PartValue<String?>(
        'WorkPhoneExt',
        workPhoneExt,
      ),
      PartValue<String?>(
        'CellPhone',
        cellPhone,
      ),
      PartValue<bool?>(
        'Active',
        active,
      ),
      PartValue<bool?>(
        'NotificationThroughEmail',
        notificationThroughEmail,
      ),
      PartValue<String?>(
        'OneTimeToken',
        oneTimeToken,
      ),
      PartValue<DateTime?>(
        'OneTimeExpireDateTime',
        oneTimeExpireDateTime,
      ),
      PartValue<int?>(
        'LoginFailedAttempt',
        loginFailedAttempt,
      ),
      PartValue<bool?>(
        'IsAccountLocked',
        isAccountLocked,
      ),
      PartValue<DateTime?>(
        'AccountLockedUntil',
        accountLockedUntil,
      ),
      PartValue<String?>(
        'NewUserActivationGuid',
        newUserActivationGuid,
      ),
      PartValue<DateTime?>(
        'NewUserActivationCompletedOn',
        newUserActivationCompletedOn,
      ),
      PartValue<String?>(
        'ProfileImageGuid',
        profileImageGuid,
      ),
      PartValue<String?>(
        'ProfileImageLocation',
        profileImageLocation,
      ),
      PartValue<String?>(
        'ProfileImageActualFileName',
        profileImageActualFileName,
      ),
      PartValue<bool?>(
        'IsDeleted',
        isDeleted,
      ),
      PartValueFile<List<int>?>(
        'UploadedFile',
        UploadedFile,
      ),
    ];
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      parts: $parts,
      multipart: true,
    );
    return client.send<UserModelReturnModel, UserModelReturnModel>($request);
  }

  @override
  Future<Response<UserModelReturnModel>> _apiV1CompanyGetuserGet({int? id}) {
    final Uri $url = Uri.parse('/api/v1/company/getuser');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<UserModelReturnModel, UserModelReturnModel>($request);
  }

  @override
  Future<Response<UserModelReturnModel>> _apiV1CompanyUpdateuserbyadminPost({
    List<int>? UploadedFile,
    List<dynamic>? existingPermissionTypeIds,
    String? companyCode,
    String? modifiedByUserFirstName,
    String? modifiedByUserLastName,
    String? modifiedByUserEmail,
    int? userId,
    String? userGuid,
    String? userName,
    int? predefinedUserTypeId,
    String? passwordHash,
    DateTime? passwordChangedDate,
    String? email,
    String? firstName,
    String? lastName,
    String? workPhone,
    String? workPhoneExt,
    String? cellPhone,
    bool? active,
    bool? notificationThroughEmail,
    String? oneTimeToken,
    DateTime? oneTimeExpireDateTime,
    int? loginFailedAttempt,
    bool? isAccountLocked,
    DateTime? accountLockedUntil,
    String? newUserActivationGuid,
    DateTime? newUserActivationCompletedOn,
    String? profileImageGuid,
    String? profileImageLocation,
    String? profileImageActualFileName,
    bool? isDeleted,
  }) {
    final Uri $url = Uri.parse('/api/v1/company/updateuserbyadmin');
    final List<PartValue> $parts = <PartValue>[
      PartValue<List<dynamic>?>(
        'ExistingPermissionTypeIds',
        existingPermissionTypeIds,
      ),
      PartValue<String?>(
        'CompanyCode',
        companyCode,
      ),
      PartValue<String?>(
        'ModifiedByUserFirstName',
        modifiedByUserFirstName,
      ),
      PartValue<String?>(
        'ModifiedByUserLastName',
        modifiedByUserLastName,
      ),
      PartValue<String?>(
        'ModifiedByUserEmail',
        modifiedByUserEmail,
      ),
      PartValue<int?>(
        'UserId',
        userId,
      ),
      PartValue<String?>(
        'UserGuid',
        userGuid,
      ),
      PartValue<String?>(
        'UserName',
        userName,
      ),
      PartValue<int?>(
        'PredefinedUserTypeId',
        predefinedUserTypeId,
      ),
      PartValue<String?>(
        'PasswordHash',
        passwordHash,
      ),
      PartValue<DateTime?>(
        'PasswordChangedDate',
        passwordChangedDate,
      ),
      PartValue<String?>(
        'Email',
        email,
      ),
      PartValue<String?>(
        'FirstName',
        firstName,
      ),
      PartValue<String?>(
        'LastName',
        lastName,
      ),
      PartValue<String?>(
        'WorkPhone',
        workPhone,
      ),
      PartValue<String?>(
        'WorkPhoneExt',
        workPhoneExt,
      ),
      PartValue<String?>(
        'CellPhone',
        cellPhone,
      ),
      PartValue<bool?>(
        'Active',
        active,
      ),
      PartValue<bool?>(
        'NotificationThroughEmail',
        notificationThroughEmail,
      ),
      PartValue<String?>(
        'OneTimeToken',
        oneTimeToken,
      ),
      PartValue<DateTime?>(
        'OneTimeExpireDateTime',
        oneTimeExpireDateTime,
      ),
      PartValue<int?>(
        'LoginFailedAttempt',
        loginFailedAttempt,
      ),
      PartValue<bool?>(
        'IsAccountLocked',
        isAccountLocked,
      ),
      PartValue<DateTime?>(
        'AccountLockedUntil',
        accountLockedUntil,
      ),
      PartValue<String?>(
        'NewUserActivationGuid',
        newUserActivationGuid,
      ),
      PartValue<DateTime?>(
        'NewUserActivationCompletedOn',
        newUserActivationCompletedOn,
      ),
      PartValue<String?>(
        'ProfileImageGuid',
        profileImageGuid,
      ),
      PartValue<String?>(
        'ProfileImageLocation',
        profileImageLocation,
      ),
      PartValue<String?>(
        'ProfileImageActualFileName',
        profileImageActualFileName,
      ),
      PartValue<bool?>(
        'IsDeleted',
        isDeleted,
      ),
      PartValueFile<List<int>?>(
        'UploadedFile',
        UploadedFile,
      ),
    ];
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      parts: $parts,
      multipart: true,
    );
    return client.send<UserModelReturnModel, UserModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>> _apiV1CompanyEnableuserbyidGet(
      {int? userId}) {
    final Uri $url = Uri.parse('/api/v1/company/enableuserbyid');
    final Map<String, dynamic> $params = <String, dynamic>{'userId': userId};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>> _apiV1CompanyDisableuserbyidGet(
      {int? userId}) {
    final Uri $url = Uri.parse('/api/v1/company/disableuserbyid');
    final Map<String, dynamic> $params = <String, dynamic>{'userId': userId};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>> _apiV1CompanyMarkuserasdeletedDelete(
      {int? id}) {
    final Uri $url = Uri.parse('/api/v1/company/markuserasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>> _apiV1CompanyMarkuserasnotdeletedPost(
      {required int? body}) {
    final Uri $url = Uri.parse('/api/v1/company/markuserasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<UserPermissionModelReturnListModel>>
      _apiV1CompanyGetuserpermissionsGet({
    bool? showDeleted,
    int? userId,
    List<int>? predefinedUserPermissionTypeIds,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/company/getuserpermissions');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'UserId': userId,
      'PredefinedUserPermissionTypeIds': predefinedUserPermissionTypeIds,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<UserPermissionModelReturnListModel,
        UserPermissionModelReturnListModel>($request);
  }

  @override
  Future<Response<UserPermissionModelReturnModel>>
      _apiV1CompanyGetuserpermissionGet({int? id}) {
    final Uri $url = Uri.parse('/api/v1/company/getuserpermission');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<UserPermissionModelReturnModel,
        UserPermissionModelReturnModel>($request);
  }

  @override
  Future<Response<UserPermissionModelReturnModel>>
      _apiV1CompanyCreateuserpermissionPost(
          {required UserPermissionCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/company/createuserpermission');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<UserPermissionModelReturnModel,
        UserPermissionModelReturnModel>($request);
  }

  @override
  Future<Response<UserPermissionModelReturnModel>>
      _apiV1CompanyUpdateuserpermissionPost(
          {required UserPermissionUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/company/updateuserpermission');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<UserPermissionModelReturnModel,
        UserPermissionModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1CompanyMarkuserpermissionasdeletedDelete({int? id}) {
    final Uri $url = Uri.parse('/api/v1/company/markuserpermissionasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1CompanyMarkuserpermissionasnotdeletedPost({required int? body}) {
    final Uri $url =
        Uri.parse('/api/v1/company/markuserpermissionasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<UserGroupModelReturnListModel>>
      _apiV1CompanyGetusergroupsGet({
    bool? showDeleted,
    int? companyEntityId,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/company/getusergroups');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'CompanyEntityId': companyEntityId,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<UserGroupModelReturnListModel,
        UserGroupModelReturnListModel>($request);
  }

  @override
  Future<Response<UserGroupModelReturnModel>> _apiV1CompanyGetusergroupGet(
      {int? id}) {
    final Uri $url = Uri.parse('/api/v1/company/getusergroup');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client
        .send<UserGroupModelReturnModel, UserGroupModelReturnModel>($request);
  }

  @override
  Future<Response<UserGroupModelReturnModel>> _apiV1CompanyCreateusergroupPost(
      {required UserGroupCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/company/createusergroup');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client
        .send<UserGroupModelReturnModel, UserGroupModelReturnModel>($request);
  }

  @override
  Future<Response<UserGroupModelReturnListModel>>
      _apiV1CompanyCreatemultipleusergroupPost(
          {required MultipleUserGroupCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/company/createmultipleusergroup');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<UserGroupModelReturnListModel,
        UserGroupModelReturnListModel>($request);
  }

  @override
  Future<Response<UserGroupModelReturnModel>> _apiV1CompanyUpdateusergroupPost(
      {required UserGroupUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/company/updateusergroup');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client
        .send<UserGroupModelReturnModel, UserGroupModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1CompanyMarkusergroupasdeletedDelete({int? id}) {
    final Uri $url = Uri.parse('/api/v1/company/markusergroupasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1CompanyMarkusergroupasnotdeletedPost({required int? body}) {
    final Uri $url = Uri.parse('/api/v1/company/markusergroupasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<FileAttachmentFolderModelReturnListModel>>
      _apiV1ManifestGetfileattachmentfoldersGet({
    bool? showDeleted,
    int? parentFileAttachmentFolderId,
    int? userId,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/manifest/getfileattachmentfolders');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'ParentFileAttachmentFolderId': parentFileAttachmentFolderId,
      'UserId': userId,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<FileAttachmentFolderModelReturnListModel,
        FileAttachmentFolderModelReturnListModel>($request);
  }

  @override
  Future<Response<FileAttachmentFolderModelReturnModel>>
      _apiV1ManifestGetfileattachmentfolderGet({int? id}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getfileattachmentfolder');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<FileAttachmentFolderModelReturnModel,
        FileAttachmentFolderModelReturnModel>($request);
  }

  @override
  Future<Response<FileAttachmentFolderModelReturnModel>>
      _apiV1ManifestCreatefileattachmentfolderPost(
          {required FileAttachmentFolderCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/createfileattachmentfolder');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<FileAttachmentFolderModelReturnModel,
        FileAttachmentFolderModelReturnModel>($request);
  }

  @override
  Future<Response<FileAttachmentFolderModelReturnListModel>>
      _apiV1ManifestCreatemultiplefileattachmentfolderPost(
          {required MultipleFileAttachmentFolderCreateModel? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/createmultiplefileattachmentfolder');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<FileAttachmentFolderModelReturnListModel,
        FileAttachmentFolderModelReturnListModel>($request);
  }

  @override
  Future<Response<FileAttachmentFolderModelReturnModel>>
      _apiV1ManifestUpdatefileattachmentfolderPost(
          {required FileAttachmentFolderUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/updatefileattachmentfolder');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<FileAttachmentFolderModelReturnModel,
        FileAttachmentFolderModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestMarkfileattachmentfolderasdeletedDelete({int? id}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/markfileattachmentfolderasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestMarkfileattachmentfolderasnotdeletedPost(
          {required int? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/markfileattachmentfolderasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<FileAttachmentModelReturnListModel>>
      _apiV1ManifestGetfileattachmentsGet({
    bool? showOnlyDeleted,
    int? showModeTypeId,
    List<int>? fileAttachmentIds,
    bool? showDeleted,
    int? fileAttachmentFolderId,
    int? movieId,
    int? userId,
    int? artistId,
    int? vendorId,
    int? equipmentId,
    int? technicianId,
    int? movieArtistId,
    int? contractId,
    int? movieEquipmentId,
    int? movieLocationId,
    int? movieSceneId,
    int? movieSceneArtistId,
    int? movieSceneEquipmentId,
    int? movieSceneTechnicianId,
    int? movieShootDayId,
    int? movieShootDayArtistId,
    int? movieShootDayBudgetId,
    int? movieShootDayEquipmentId,
    int? movieShootDayExpenseId,
    int? movieShootDayRequirementId,
    int? movieShootDayVendorId,
    int? movieShootDaySceneId,
    int? movieShootDayTechnicianId,
    int? movieTechnicianId,
    int? movieUnitId,
    int? movieUserId,
    int? movieVendorId,
    int? moviePropertyId,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/manifest/getfileattachments');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowOnlyDeleted': showOnlyDeleted,
      'ShowModeTypeId': showModeTypeId,
      'FileAttachmentIds': fileAttachmentIds,
      'ShowDeleted': showDeleted,
      'FileAttachmentFolderId': fileAttachmentFolderId,
      'MovieId': movieId,
      'UserId': userId,
      'ArtistId': artistId,
      'VendorId': vendorId,
      'EquipmentId': equipmentId,
      'TechnicianId': technicianId,
      'MovieArtistId': movieArtistId,
      'ContractId': contractId,
      'MovieEquipmentId': movieEquipmentId,
      'MovieLocationId': movieLocationId,
      'MovieSceneId': movieSceneId,
      'MovieSceneArtistId': movieSceneArtistId,
      'MovieSceneEquipmentId': movieSceneEquipmentId,
      'MovieSceneTechnicianId': movieSceneTechnicianId,
      'MovieShootDayId': movieShootDayId,
      'MovieShootDayArtistId': movieShootDayArtistId,
      'MovieShootDayBudgetId': movieShootDayBudgetId,
      'MovieShootDayEquipmentId': movieShootDayEquipmentId,
      'MovieShootDayExpenseId': movieShootDayExpenseId,
      'MovieShootDayRequirementId': movieShootDayRequirementId,
      'MovieShootDayVendorId': movieShootDayVendorId,
      'MovieShootDaySceneId': movieShootDaySceneId,
      'MovieShootDayTechnicianId': movieShootDayTechnicianId,
      'MovieTechnicianId': movieTechnicianId,
      'MovieUnitId': movieUnitId,
      'MovieUserId': movieUserId,
      'MovieVendorId': movieVendorId,
      'MoviePropertyId': moviePropertyId,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<FileAttachmentModelReturnListModel,
        FileAttachmentModelReturnListModel>($request);
  }

  @override
  Future<Response<FileAttachmentModelReturnModel>>
      _apiV1ManifestGetfileattachmentGet({int? id}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getfileattachment');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<FileAttachmentModelReturnModel,
        FileAttachmentModelReturnModel>($request);
  }

  @override
  Future<Response<FileAttachmentModelReturnModel>>
      _apiV1ManifestCreatefileattachmentPost({
    List<int>? UploadedFile,
    int? fileAttachmentFolderId,
    String? fileAttachmentFileName,
    String? fileAttachmentFileLocation,
    String? fileAttachmentPreviewFileLocation,
    bool? fileAttachmentHasPreview,
    int? fileAttachmentFileSizeInBytes,
    String? description,
    int? userId,
  }) {
    final Uri $url = Uri.parse('/api/v1/manifest/createfileattachment');
    final List<PartValue> $parts = <PartValue>[
      PartValue<int?>(
        'FileAttachmentFolderId',
        fileAttachmentFolderId,
      ),
      PartValue<String?>(
        'FileAttachmentFileName',
        fileAttachmentFileName,
      ),
      PartValue<String?>(
        'FileAttachmentFileLocation',
        fileAttachmentFileLocation,
      ),
      PartValue<String?>(
        'FileAttachmentPreviewFileLocation',
        fileAttachmentPreviewFileLocation,
      ),
      PartValue<bool?>(
        'FileAttachmentHasPreview',
        fileAttachmentHasPreview,
      ),
      PartValue<int?>(
        'FileAttachmentFileSizeInBytes',
        fileAttachmentFileSizeInBytes,
      ),
      PartValue<String?>(
        'Description',
        description,
      ),
      PartValue<int?>(
        'UserId',
        userId,
      ),
      PartValueFile<List<int>?>(
        'UploadedFile',
        UploadedFile,
      ),
    ];
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      parts: $parts,
      multipart: true,
    );
    return client.send<FileAttachmentModelReturnModel,
        FileAttachmentModelReturnModel>($request);
  }

  @override
  Future<Response<FileAttachmentModelReturnModel>>
      _apiV1ManifestUpdatefileattachmentPost(
          {required FileAttachmentUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/updatefileattachment');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<FileAttachmentModelReturnModel,
        FileAttachmentModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestMarkfileattachmentasdeletedDelete({int? id}) {
    final Uri $url = Uri.parse('/api/v1/manifest/markfileattachmentasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestMarkfileattachmentasnotdeletedPost({required int? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/markfileattachmentasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestIsfileattachmentfilevalidGet({int? fileAttachmentId}) {
    final Uri $url = Uri.parse('/api/v1/manifest/isfileattachmentfilevalid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'fileAttachmentId': fileAttachmentId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<dynamic>> _apiV1ManifestDownloadfileattachmentfileGet(
      {int? fileAttachmentId}) {
    final Uri $url = Uri.parse('/api/v1/manifest/downloadfileattachmentfile');
    final Map<String, dynamic> $params = <String, dynamic>{
      'fileAttachmentId': fileAttachmentId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<dynamic, dynamic>($request);
  }

  @override
  Future<Response<VisionBoardSessionPictureModelReturnListModel>>
      _apiV1ManifestGetvisionboardsessionpicturesGet({
    bool? showDeleted,
    int? visionBoardSessionId,
    int? visionBoardPictureId,
    int? userId,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getvisionboardsessionpictures');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'VisionBoardSessionId': visionBoardSessionId,
      'VisionBoardPictureId': visionBoardPictureId,
      'UserId': userId,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<VisionBoardSessionPictureModelReturnListModel,
        VisionBoardSessionPictureModelReturnListModel>($request);
  }

  @override
  Future<Response<VisionBoardSessionPictureModelReturnModel>>
      _apiV1ManifestGetvisionboardsessionpictureGet({int? id}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getvisionboardsessionpicture');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<VisionBoardSessionPictureModelReturnModel,
        VisionBoardSessionPictureModelReturnModel>($request);
  }

  @override
  Future<Response<VisionBoardSessionPictureModelReturnModel>>
      _apiV1ManifestCreatevisionboardsessionpicturePost(
          {required VisionBoardSessionPictureCreateModel? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/createvisionboardsessionpicture');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<VisionBoardSessionPictureModelReturnModel,
        VisionBoardSessionPictureModelReturnModel>($request);
  }

  @override
  Future<Response<VisionBoardSessionPictureModelReturnListModel>>
      _apiV1ManifestCreatemultiplevisionboardsessionpicturePost(
          {required MultipleVisionBoardSessionPictureCreateModel? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/createmultiplevisionboardsessionpicture');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<VisionBoardSessionPictureModelReturnListModel,
        VisionBoardSessionPictureModelReturnListModel>($request);
  }

  @override
  Future<Response<VisionBoardSessionPictureModelReturnModel>>
      _apiV1ManifestUpdatevisionboardsessionpicturePost(
          {required VisionBoardSessionPictureUpdateModel? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/updatevisionboardsessionpicture');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<VisionBoardSessionPictureModelReturnModel,
        VisionBoardSessionPictureModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestMarkvisionboardsessionpictureasdeletedDelete({int? id}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/markvisionboardsessionpictureasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestMarkvisionboardsessionpictureasnotdeletedPost(
          {required int? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/markvisionboardsessionpictureasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<VisionBoardSessionPictureModelReturnListModel>>
      _apiV1ManifestGetvisionboardsessionpicturebyvisionboardsessionidGet(
          {int? visionBoardSessionId}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/getvisionboardsessionpicturebyvisionboardsessionid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'visionBoardSessionId': visionBoardSessionId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<VisionBoardSessionPictureModelReturnListModel,
        VisionBoardSessionPictureModelReturnListModel>($request);
  }

  @override
  Future<Response<VisionBoardSessionPictureModelReturnListModel>>
      _apiV1ManifestGetvisionboardsessionpicturebyvisionboardsessionidsearchGet(
          {required VisionBoardSessionPictureByVisionBoardSessionIdSearchCriteria?
              body}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/getvisionboardsessionpicturebyvisionboardsessionidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<VisionBoardSessionPictureModelReturnListModel,
        VisionBoardSessionPictureModelReturnListModel>($request);
  }

  @override
  Future<Response<VisionBoardSessionModelReturnListModel>>
      _apiV1ManifestGetvisionboardsessionbyvisionboardsessionpictureidGet(
          {int? visionBoardSessionPictureId}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/getvisionboardsessionbyvisionboardsessionpictureid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'visionBoardSessionPictureId': visionBoardSessionPictureId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<VisionBoardSessionModelReturnListModel,
        VisionBoardSessionModelReturnListModel>($request);
  }

  @override
  Future<Response<VisionBoardSessionModelReturnListModel>>
      _apiV1ManifestGetvisionboardsessionbyvisionboardsessionpictureidsearchGet(
          {required VisionBoardSessionByVisionBoardSessionPictureIdSearchCriteria?
              body}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/getvisionboardsessionbyvisionboardsessionpictureidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<VisionBoardSessionModelReturnListModel,
        VisionBoardSessionModelReturnListModel>($request);
  }

  @override
  Future<Response<VisionBoardSessionPictureModelReturnListModel>>
      _apiV1ManifestGetvisionboardsessionpicturebyvisionboardpictureidGet(
          {int? visionBoardPictureId}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/getvisionboardsessionpicturebyvisionboardpictureid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'visionBoardPictureId': visionBoardPictureId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<VisionBoardSessionPictureModelReturnListModel,
        VisionBoardSessionPictureModelReturnListModel>($request);
  }

  @override
  Future<Response<VisionBoardSessionPictureModelReturnListModel>>
      _apiV1ManifestGetvisionboardsessionpicturebyvisionboardpictureidsearchGet(
          {required VisionBoardSessionPictureByVisionBoardPictureIdSearchCriteria?
              body}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/getvisionboardsessionpicturebyvisionboardpictureidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<VisionBoardSessionPictureModelReturnListModel,
        VisionBoardSessionPictureModelReturnListModel>($request);
  }

  @override
  Future<Response<VisionBoardPictureModelReturnListModel>>
      _apiV1ManifestGetvisionboardpicturebyvisionboardsessionpictureidGet(
          {int? visionBoardSessionPictureId}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/getvisionboardpicturebyvisionboardsessionpictureid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'visionBoardSessionPictureId': visionBoardSessionPictureId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<VisionBoardPictureModelReturnListModel,
        VisionBoardPictureModelReturnListModel>($request);
  }

  @override
  Future<Response<VisionBoardPictureModelReturnListModel>>
      _apiV1ManifestGetvisionboardpicturebyvisionboardsessionpictureidsearchGet(
          {required VisionBoardPictureByVisionBoardSessionPictureIdSearchCriteria?
              body}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/getvisionboardpicturebyvisionboardsessionpictureidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<VisionBoardPictureModelReturnListModel,
        VisionBoardPictureModelReturnListModel>($request);
  }

  @override
  Future<Response<VisionBoardSessionPictureModelReturnListModel>>
      _apiV1ManifestGetvisionboardsessionpicturebyuseridGet({int? userId}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getvisionboardsessionpicturebyuserid');
    final Map<String, dynamic> $params = <String, dynamic>{'userId': userId};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<VisionBoardSessionPictureModelReturnListModel,
        VisionBoardSessionPictureModelReturnListModel>($request);
  }

  @override
  Future<Response<VisionBoardSessionPictureModelReturnListModel>>
      _apiV1ManifestGetvisionboardsessionpicturebyuseridsearchGet(
          {required VisionBoardSessionPictureByUserIdSearchCriteria? body}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/getvisionboardsessionpicturebyuseridsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<VisionBoardSessionPictureModelReturnListModel,
        VisionBoardSessionPictureModelReturnListModel>($request);
  }

  @override
  Future<Response<UserModelReturnListModel>>
      _apiV1ManifestGetuserbyvisionboardsessionpictureidGet(
          {int? visionBoardSessionPictureId}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getuserbyvisionboardsessionpictureid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'visionBoardSessionPictureId': visionBoardSessionPictureId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client
        .send<UserModelReturnListModel, UserModelReturnListModel>($request);
  }

  @override
  Future<Response<UserModelReturnListModel>>
      _apiV1ManifestGetuserbyvisionboardsessionpictureidsearchGet(
          {required UserByVisionBoardSessionPictureIdSearchCriteria? body}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/getuserbyvisionboardsessionpictureidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client
        .send<UserModelReturnListModel, UserModelReturnListModel>($request);
  }

  @override
  Future<Response<CategoryModelReturnListModel>> _apiV1ManifestGetcategorysGet({
    bool? showDeleted,
    int? defaultCategoryId,
    List<int>? predefinedCategoryTypeIds,
    int? userId,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/manifest/getcategorys');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'DefaultCategoryId': defaultCategoryId,
      'PredefinedCategoryTypeIds': predefinedCategoryTypeIds,
      'UserId': userId,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<CategoryModelReturnListModel,
        CategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<CategoryModelReturnModel>> _apiV1ManifestGetcategoryGet(
      {int? id}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getcategory');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client
        .send<CategoryModelReturnModel, CategoryModelReturnModel>($request);
  }

  @override
  Future<Response<CategoryModelReturnModel>> _apiV1ManifestCreatecategoryPost(
      {required CategoryCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/createcategory');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client
        .send<CategoryModelReturnModel, CategoryModelReturnModel>($request);
  }

  @override
  Future<Response<CategoryModelReturnListModel>>
      _apiV1ManifestCreatemultiplecategoryPost(
          {required MultipleCategoryCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/createmultiplecategory');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<CategoryModelReturnListModel,
        CategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<CategoryModelReturnModel>> _apiV1ManifestUpdatecategoryPost(
      {required CategoryUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/updatecategory');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client
        .send<CategoryModelReturnModel, CategoryModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestMarkcategoryasdeletedDelete({int? id}) {
    final Uri $url = Uri.parse('/api/v1/manifest/markcategoryasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestMarkcategoryasnotdeletedPost({required int? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/markcategoryasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<CategoryModelReturnListModel>>
      _apiV1ManifestGetcategorybydefaultcategoryidGet(
          {int? defaultCategoryId}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getcategorybydefaultcategoryid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'defaultCategoryId': defaultCategoryId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<CategoryModelReturnListModel,
        CategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<CategoryModelReturnListModel>>
      _apiV1ManifestGetcategorybydefaultcategoryidsearchGet(
          {required CategoryByDefaultCategoryIdSearchCriteria? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getcategorybydefaultcategoryidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<CategoryModelReturnListModel,
        CategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<DefaultCategoryModelReturnListModel>>
      _apiV1ManifestGetdefaultcategorybycategoryidGet({int? categoryId}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getdefaultcategorybycategoryid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'categoryId': categoryId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<DefaultCategoryModelReturnListModel,
        DefaultCategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<DefaultCategoryModelReturnListModel>>
      _apiV1ManifestGetdefaultcategorybycategoryidsearchGet(
          {required DefaultCategoryByCategoryIdSearchCriteria? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getdefaultcategorybycategoryidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DefaultCategoryModelReturnListModel,
        DefaultCategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<CategoryModelReturnListModel>>
      _apiV1ManifestGetcategorybyuseridGet({int? userId}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getcategorybyuserid');
    final Map<String, dynamic> $params = <String, dynamic>{'userId': userId};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<CategoryModelReturnListModel,
        CategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<CategoryModelReturnListModel>>
      _apiV1ManifestGetcategoryforcurrentuserGet() {
    final Uri $url = Uri.parse('/api/v1/manifest/getcategoryforcurrentuser');
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
    );
    return client.send<CategoryModelReturnListModel,
        CategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<CategoryModelReturnListModel>>
      _apiV1ManifestGetcategorybyuseridsearchGet(
          {required CategoryByUserIdSearchCriteria? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getcategorybyuseridsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<CategoryModelReturnListModel,
        CategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<UserModelReturnListModel>>
      _apiV1ManifestGetuserbycategoryidGet({int? categoryId}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getuserbycategoryid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'categoryId': categoryId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client
        .send<UserModelReturnListModel, UserModelReturnListModel>($request);
  }

  @override
  Future<Response<UserModelReturnListModel>>
      _apiV1ManifestGetuserbycategoryidsearchGet(
          {required UserByCategoryIdSearchCriteria? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getuserbycategoryidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client
        .send<UserModelReturnListModel, UserModelReturnListModel>($request);
  }

  @override
  Future<Response<DailyAffirmationSessionModelReturnListModel>>
      _apiV1ManifestGetdailyaffirmationsessionsGet({
    bool? showDeleted,
    int? userId,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/manifest/getdailyaffirmationsessions');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'UserId': userId,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<DailyAffirmationSessionModelReturnListModel,
        DailyAffirmationSessionModelReturnListModel>($request);
  }

  @override
  Future<Response<DailyAffirmationSessionModelReturnModel>>
      _apiV1ManifestGetdailyaffirmationsessionGet({int? id}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getdailyaffirmationsession');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<DailyAffirmationSessionModelReturnModel,
        DailyAffirmationSessionModelReturnModel>($request);
  }

  @override
  Future<Response<DailyAffirmationSessionModelReturnModel>>
      _apiV1ManifestCreatedailyaffirmationsessionPost(
          {required DailyAffirmationSessionCreateModel? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/createdailyaffirmationsession');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DailyAffirmationSessionModelReturnModel,
        DailyAffirmationSessionModelReturnModel>($request);
  }

  @override
  Future<Response<DailyAffirmationSessionModelReturnListModel>>
      _apiV1ManifestCreatemultipledailyaffirmationsessionPost(
          {required MultipleDailyAffirmationSessionCreateModel? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/createmultipledailyaffirmationsession');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DailyAffirmationSessionModelReturnListModel,
        DailyAffirmationSessionModelReturnListModel>($request);
  }

  @override
  Future<Response<DailyAffirmationSessionModelReturnModel>>
      _apiV1ManifestUpdatedailyaffirmationsessionPost(
          {required DailyAffirmationSessionUpdateModel? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/updatedailyaffirmationsession');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DailyAffirmationSessionModelReturnModel,
        DailyAffirmationSessionModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestMarkdailyaffirmationsessionasdeletedDelete({int? id}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/markdailyaffirmationsessionasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestMarkdailyaffirmationsessionasnotdeletedPost(
          {required int? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/markdailyaffirmationsessionasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<DailyAffirmationSessionModelReturnListModel>>
      _apiV1ManifestGetdailyaffirmationsessionbyuseridGet({int? userId}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getdailyaffirmationsessionbyuserid');
    final Map<String, dynamic> $params = <String, dynamic>{'userId': userId};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<DailyAffirmationSessionModelReturnListModel,
        DailyAffirmationSessionModelReturnListModel>($request);
  }

  @override
  Future<Response<DailyAffirmationSessionModelReturnListModel>>
      _apiV1ManifestGetdailyaffirmationsessionbyuseridsearchGet(
          {required DailyAffirmationSessionByUserIdSearchCriteria? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getdailyaffirmationsessionbyuseridsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DailyAffirmationSessionModelReturnListModel,
        DailyAffirmationSessionModelReturnListModel>($request);
  }

  @override
  Future<Response<UserModelReturnListModel>>
      _apiV1ManifestGetuserbydailyaffirmationsessionidGet(
          {int? dailyAffirmationSessionId}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getuserbydailyaffirmationsessionid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'dailyAffirmationSessionId': dailyAffirmationSessionId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client
        .send<UserModelReturnListModel, UserModelReturnListModel>($request);
  }

  @override
  Future<Response<UserModelReturnListModel>>
      _apiV1ManifestGetuserbydailyaffirmationsessionidsearchGet(
          {required UserByDailyAffirmationSessionIdSearchCriteria? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getuserbydailyaffirmationsessionidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client
        .send<UserModelReturnListModel, UserModelReturnListModel>($request);
  }

  @override
  Future<Response<DailyAffirmationSessionCategoryModelReturnListModel>>
      _apiV1ManifestGetdailyaffirmationsessioncategorysGet({
    bool? showDeleted,
    int? categoryId,
    int? dailyAffirmationSessionId,
    int? userId,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getdailyaffirmationsessioncategorys');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'CategoryId': categoryId,
      'DailyAffirmationSessionId': dailyAffirmationSessionId,
      'UserId': userId,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<DailyAffirmationSessionCategoryModelReturnListModel,
        DailyAffirmationSessionCategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<DailyAffirmationSessionCategoryModelReturnModel>>
      _apiV1ManifestGetdailyaffirmationsessioncategoryGet({int? id}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getdailyaffirmationsessioncategory');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<DailyAffirmationSessionCategoryModelReturnModel,
        DailyAffirmationSessionCategoryModelReturnModel>($request);
  }

  @override
  Future<Response<DailyAffirmationSessionCategoryModelReturnModel>>
      _apiV1ManifestCreatedailyaffirmationsessioncategoryPost(
          {required DailyAffirmationSessionCategoryCreateModel? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/createdailyaffirmationsessioncategory');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DailyAffirmationSessionCategoryModelReturnModel,
        DailyAffirmationSessionCategoryModelReturnModel>($request);
  }

  @override
  Future<Response<DailyAffirmationSessionCategoryModelReturnListModel>>
      _apiV1ManifestCreatemultipledailyaffirmationsessioncategoryPost(
          {required MultipleDailyAffirmationSessionCategoryCreateModel? body}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/createmultipledailyaffirmationsessioncategory');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DailyAffirmationSessionCategoryModelReturnListModel,
        DailyAffirmationSessionCategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<DailyAffirmationSessionCategoryModelReturnModel>>
      _apiV1ManifestUpdatedailyaffirmationsessioncategoryPost(
          {required DailyAffirmationSessionCategoryUpdateModel? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/updatedailyaffirmationsessioncategory');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DailyAffirmationSessionCategoryModelReturnModel,
        DailyAffirmationSessionCategoryModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestMarkdailyaffirmationsessioncategoryasdeletedDelete(
          {int? id}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/markdailyaffirmationsessioncategoryasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestMarkdailyaffirmationsessioncategoryasnotdeletedPost(
          {required int? body}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/markdailyaffirmationsessioncategoryasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<DailyAffirmationSessionCategoryModelReturnListModel>>
      _apiV1ManifestGetdailyaffirmationsessioncategorybycategoryidGet(
          {int? categoryId}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/getdailyaffirmationsessioncategorybycategoryid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'categoryId': categoryId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<DailyAffirmationSessionCategoryModelReturnListModel,
        DailyAffirmationSessionCategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<DailyAffirmationSessionCategoryModelReturnListModel>>
      _apiV1ManifestGetdailyaffirmationsessioncategorybycategoryidsearchGet(
          {required DailyAffirmationSessionCategoryByCategoryIdSearchCriteria?
              body}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/getdailyaffirmationsessioncategorybycategoryidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DailyAffirmationSessionCategoryModelReturnListModel,
        DailyAffirmationSessionCategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<CategoryModelReturnListModel>>
      _apiV1ManifestGetcategorybydailyaffirmationsessioncategoryidGet(
          {int? dailyAffirmationSessionCategoryId}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/getcategorybydailyaffirmationsessioncategoryid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'dailyAffirmationSessionCategoryId': dailyAffirmationSessionCategoryId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<CategoryModelReturnListModel,
        CategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<CategoryModelReturnListModel>>
      _apiV1ManifestGetcategorybydailyaffirmationsessioncategoryidsearchGet(
          {required CategoryByDailyAffirmationSessionCategoryIdSearchCriteria?
              body}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/getcategorybydailyaffirmationsessioncategoryidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<CategoryModelReturnListModel,
        CategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<DailyAffirmationSessionCategoryModelReturnListModel>>
      _apiV1ManifestGetdailyaffirmationsessioncategorybydailyaffirmationsessionidGet(
          {int? dailyAffirmationSessionId}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/getdailyaffirmationsessioncategorybydailyaffirmationsessionid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'dailyAffirmationSessionId': dailyAffirmationSessionId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<DailyAffirmationSessionCategoryModelReturnListModel,
        DailyAffirmationSessionCategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<DailyAffirmationSessionCategoryModelReturnListModel>>
      _apiV1ManifestGetdailyaffirmationsessioncategorybydailyaffirmationsessionidsearchGet(
          {required DailyAffirmationSessionCategoryByDailyAffirmationSessionIdSearchCriteria?
              body}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/getdailyaffirmationsessioncategorybydailyaffirmationsessionidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DailyAffirmationSessionCategoryModelReturnListModel,
        DailyAffirmationSessionCategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<DailyAffirmationSessionModelReturnListModel>>
      _apiV1ManifestGetdailyaffirmationsessionbydailyaffirmationsessioncategoryidGet(
          {int? dailyAffirmationSessionCategoryId}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/getdailyaffirmationsessionbydailyaffirmationsessioncategoryid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'dailyAffirmationSessionCategoryId': dailyAffirmationSessionCategoryId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<DailyAffirmationSessionModelReturnListModel,
        DailyAffirmationSessionModelReturnListModel>($request);
  }

  @override
  Future<Response<DailyAffirmationSessionModelReturnListModel>>
      _apiV1ManifestGetdailyaffirmationsessionbydailyaffirmationsessioncategoryidsearchGet(
          {required DailyAffirmationSessionByDailyAffirmationSessionCategoryIdSearchCriteria?
              body}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/getdailyaffirmationsessionbydailyaffirmationsessioncategoryidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DailyAffirmationSessionModelReturnListModel,
        DailyAffirmationSessionModelReturnListModel>($request);
  }

  @override
  Future<Response<DailyAffirmationSessionCategoryModelReturnListModel>>
      _apiV1ManifestGetdailyaffirmationsessioncategorybyuseridGet(
          {int? userId}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/getdailyaffirmationsessioncategorybyuserid');
    final Map<String, dynamic> $params = <String, dynamic>{'userId': userId};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<DailyAffirmationSessionCategoryModelReturnListModel,
        DailyAffirmationSessionCategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<DailyAffirmationSessionCategoryModelReturnListModel>>
      _apiV1ManifestGetdailyaffirmationsessioncategorybyuseridsearchGet(
          {required DailyAffirmationSessionCategoryByUserIdSearchCriteria?
              body}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/getdailyaffirmationsessioncategorybyuseridsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DailyAffirmationSessionCategoryModelReturnListModel,
        DailyAffirmationSessionCategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<UserModelReturnListModel>>
      _apiV1ManifestGetuserbydailyaffirmationsessioncategoryidGet(
          {int? dailyAffirmationSessionCategoryId}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/getuserbydailyaffirmationsessioncategoryid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'dailyAffirmationSessionCategoryId': dailyAffirmationSessionCategoryId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client
        .send<UserModelReturnListModel, UserModelReturnListModel>($request);
  }

  @override
  Future<Response<UserModelReturnListModel>>
      _apiV1ManifestGetuserbydailyaffirmationsessioncategoryidsearchGet(
          {required UserByDailyAffirmationSessionCategoryIdSearchCriteria?
              body}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/getuserbydailyaffirmationsessioncategoryidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client
        .send<UserModelReturnListModel, UserModelReturnListModel>($request);
  }

  @override
  Future<Response<DailyAffirmationTextModelReturnListModel>>
      _apiV1ManifestGetdailyaffirmationtextsGet({
    bool? showDeleted,
    int? categoryId,
    int? userId,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/manifest/getdailyaffirmationtexts');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'CategoryId': categoryId,
      'UserId': userId,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<DailyAffirmationTextModelReturnListModel,
        DailyAffirmationTextModelReturnListModel>($request);
  }

  @override
  Future<Response<DailyAffirmationTextModelReturnModel>>
      _apiV1ManifestGetdailyaffirmationtextGet({int? id}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getdailyaffirmationtext');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<DailyAffirmationTextModelReturnModel,
        DailyAffirmationTextModelReturnModel>($request);
  }

  @override
  Future<Response<DailyAffirmationTextModelReturnModel>>
      _apiV1ManifestCreatedailyaffirmationtextPost(
          {required DailyAffirmationTextCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/createdailyaffirmationtext');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DailyAffirmationTextModelReturnModel,
        DailyAffirmationTextModelReturnModel>($request);
  }

  @override
  Future<Response<DailyAffirmationTextModelReturnListModel>>
      _apiV1ManifestCreatemultipledailyaffirmationtextPost(
          {required MultipleDailyAffirmationTextCreateModel? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/createmultipledailyaffirmationtext');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DailyAffirmationTextModelReturnListModel,
        DailyAffirmationTextModelReturnListModel>($request);
  }

  @override
  Future<Response<DailyAffirmationTextModelReturnModel>>
      _apiV1ManifestUpdatedailyaffirmationtextPost(
          {required DailyAffirmationTextUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/updatedailyaffirmationtext');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DailyAffirmationTextModelReturnModel,
        DailyAffirmationTextModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestMarkdailyaffirmationtextasdeletedDelete({int? id}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/markdailyaffirmationtextasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestMarkdailyaffirmationtextasnotdeletedPost(
          {required int? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/markdailyaffirmationtextasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<DailyAffirmationTextModelReturnListModel>>
      _apiV1ManifestGetdailyaffirmationtextbycategoryidGet({int? categoryId}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getdailyaffirmationtextbycategoryid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'categoryId': categoryId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<DailyAffirmationTextModelReturnListModel,
        DailyAffirmationTextModelReturnListModel>($request);
  }

  @override
  Future<Response<DailyAffirmationTextModelReturnListModel>>
      _apiV1ManifestGetdailyaffirmationtextbycategoryidsearchGet(
          {required DailyAffirmationTextByCategoryIdSearchCriteria? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getdailyaffirmationtextbycategoryidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DailyAffirmationTextModelReturnListModel,
        DailyAffirmationTextModelReturnListModel>($request);
  }

  @override
  Future<Response<CategoryModelReturnListModel>>
      _apiV1ManifestGetcategorybydailyaffirmationtextidGet(
          {int? dailyAffirmationTextId}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getcategorybydailyaffirmationtextid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'dailyAffirmationTextId': dailyAffirmationTextId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<CategoryModelReturnListModel,
        CategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<CategoryModelReturnListModel>>
      _apiV1ManifestGetcategorybydailyaffirmationtextidsearchGet(
          {required CategoryByDailyAffirmationTextIdSearchCriteria? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getcategorybydailyaffirmationtextidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<CategoryModelReturnListModel,
        CategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<DailyAffirmationTextModelReturnListModel>>
      _apiV1ManifestGetdailyaffirmationtextforcurrentuserGet() {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getdailyaffirmationtextforcurrentuser');
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
    );
    return client.send<DailyAffirmationTextModelReturnListModel,
        DailyAffirmationTextModelReturnListModel>($request);
  }

  @override
  Future<Response<DailyAffirmationTextModelReturnListModel>>
      _apiV1ManifestGetdailyaffirmationtextbyuseridGet({int? userId}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getdailyaffirmationtextbyuserid');
    final Map<String, dynamic> $params = <String, dynamic>{'userId': userId};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<DailyAffirmationTextModelReturnListModel,
        DailyAffirmationTextModelReturnListModel>($request);
  }

  @override
  Future<Response<DailyAffirmationTextModelReturnListModel>>
      _apiV1ManifestGetdailyaffirmationtextbyuseridsearchGet(
          {required DailyAffirmationTextByUserIdSearchCriteria? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getdailyaffirmationtextbyuseridsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DailyAffirmationTextModelReturnListModel,
        DailyAffirmationTextModelReturnListModel>($request);
  }

  @override
  Future<Response<UserModelReturnListModel>>
      _apiV1ManifestGetuserbydailyaffirmationtextidGet(
          {int? dailyAffirmationTextId}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getuserbydailyaffirmationtextid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'dailyAffirmationTextId': dailyAffirmationTextId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client
        .send<UserModelReturnListModel, UserModelReturnListModel>($request);
  }

  @override
  Future<Response<UserModelReturnListModel>>
      _apiV1ManifestGetuserbydailyaffirmationtextidsearchGet(
          {required UserByDailyAffirmationTextIdSearchCriteria? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getuserbydailyaffirmationtextidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client
        .send<UserModelReturnListModel, UserModelReturnListModel>($request);
  }

  @override
  Future<Response<DefaultCategoryModelReturnListModel>>
      _apiV1ManifestGetdefaultcategorysGet({
    bool? showDeleted,
    List<int>? predefinedCategoryTypeIds,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/manifest/getdefaultcategorys');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'PredefinedCategoryTypeIds': predefinedCategoryTypeIds,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<DefaultCategoryModelReturnListModel,
        DefaultCategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<DefaultCategoryModelReturnModel>>
      _apiV1ManifestGetdefaultcategoryGet({int? id}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getdefaultcategory');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<DefaultCategoryModelReturnModel,
        DefaultCategoryModelReturnModel>($request);
  }

  @override
  Future<Response<DefaultCategoryModelReturnModel>>
      _apiV1ManifestCreatedefaultcategoryPost(
          {required DefaultCategoryCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/createdefaultcategory');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DefaultCategoryModelReturnModel,
        DefaultCategoryModelReturnModel>($request);
  }

  @override
  Future<Response<DefaultCategoryModelReturnListModel>>
      _apiV1ManifestCreatemultipledefaultcategoryPost(
          {required MultipleDefaultCategoryCreateModel? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/createmultipledefaultcategory');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DefaultCategoryModelReturnListModel,
        DefaultCategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<DefaultCategoryModelReturnModel>>
      _apiV1ManifestUpdatedefaultcategoryPost(
          {required DefaultCategoryUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/updatedefaultcategory');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DefaultCategoryModelReturnModel,
        DefaultCategoryModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestMarkdefaultcategoryasdeletedDelete({int? id}) {
    final Uri $url = Uri.parse('/api/v1/manifest/markdefaultcategoryasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestMarkdefaultcategoryasnotdeletedPost({required int? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/markdefaultcategoryasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<DefaultDailyAffirmationTextModelReturnListModel>>
      _apiV1ManifestGetdefaultdailyaffirmationtextsGet({
    int? userId,
    bool? showDeleted,
    int? defaultCategoryId,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getdefaultdailyaffirmationtexts');
    final Map<String, dynamic> $params = <String, dynamic>{
      'UserId': userId,
      'ShowDeleted': showDeleted,
      'DefaultCategoryId': defaultCategoryId,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<DefaultDailyAffirmationTextModelReturnListModel,
        DefaultDailyAffirmationTextModelReturnListModel>($request);
  }

  @override
  Future<Response<DefaultDailyAffirmationTextModelReturnModel>>
      _apiV1ManifestGetdefaultdailyaffirmationtextGet({int? id}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getdefaultdailyaffirmationtext');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<DefaultDailyAffirmationTextModelReturnModel,
        DefaultDailyAffirmationTextModelReturnModel>($request);
  }

  @override
  Future<Response<DefaultDailyAffirmationTextModelReturnModel>>
      _apiV1ManifestCreatedefaultdailyaffirmationtextPost(
          {required DefaultDailyAffirmationTextCreateModel? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/createdefaultdailyaffirmationtext');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DefaultDailyAffirmationTextModelReturnModel,
        DefaultDailyAffirmationTextModelReturnModel>($request);
  }

  @override
  Future<Response<DefaultDailyAffirmationTextModelReturnListModel>>
      _apiV1ManifestCreatemultipledefaultdailyaffirmationtextPost(
          {required MultipleDefaultDailyAffirmationTextCreateModel? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/createmultipledefaultdailyaffirmationtext');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DefaultDailyAffirmationTextModelReturnListModel,
        DefaultDailyAffirmationTextModelReturnListModel>($request);
  }

  @override
  Future<Response<DefaultDailyAffirmationTextModelReturnModel>>
      _apiV1ManifestUpdatedefaultdailyaffirmationtextPost(
          {required DefaultDailyAffirmationTextUpdateModel? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/updatedefaultdailyaffirmationtext');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DefaultDailyAffirmationTextModelReturnModel,
        DefaultDailyAffirmationTextModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestMarkdefaultdailyaffirmationtextasdeletedDelete({int? id}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/markdefaultdailyaffirmationtextasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestMarkdefaultdailyaffirmationtextasnotdeletedPost(
          {required int? body}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/markdefaultdailyaffirmationtextasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<DefaultDailyAffirmationTextModelReturnListModel>>
      _apiV1ManifestGetdefaultdailyaffirmationtextbydefaultcategoryidGet(
          {int? defaultCategoryId}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/getdefaultdailyaffirmationtextbydefaultcategoryid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'defaultCategoryId': defaultCategoryId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<DefaultDailyAffirmationTextModelReturnListModel,
        DefaultDailyAffirmationTextModelReturnListModel>($request);
  }

  @override
  Future<Response<DefaultDailyAffirmationTextModelReturnListModel>>
      _apiV1ManifestGetdefaultdailyaffirmationtextbydefaultcategoryidsearchGet(
          {required DefaultDailyAffirmationTextByDefaultCategoryIdSearchCriteria?
              body}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/getdefaultdailyaffirmationtextbydefaultcategoryidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DefaultDailyAffirmationTextModelReturnListModel,
        DefaultDailyAffirmationTextModelReturnListModel>($request);
  }

  @override
  Future<Response<DefaultCategoryModelReturnListModel>>
      _apiV1ManifestGetdefaultcategorybydefaultdailyaffirmationtextidGet(
          {int? defaultDailyAffirmationTextId}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/getdefaultcategorybydefaultdailyaffirmationtextid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'defaultDailyAffirmationTextId': defaultDailyAffirmationTextId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<DefaultCategoryModelReturnListModel,
        DefaultCategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<DefaultCategoryModelReturnListModel>>
      _apiV1ManifestGetdefaultcategorybydefaultdailyaffirmationtextidsearchGet(
          {required DefaultCategoryByDefaultDailyAffirmationTextIdSearchCriteria?
              body}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/getdefaultcategorybydefaultdailyaffirmationtextidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DefaultCategoryModelReturnListModel,
        DefaultCategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<DefaultGoalTrackerModelReturnListModel>>
      _apiV1ManifestGetdefaultgoaltrackersGet({
    bool? showDeleted,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/manifest/getdefaultgoaltrackers');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<DefaultGoalTrackerModelReturnListModel,
        DefaultGoalTrackerModelReturnListModel>($request);
  }

  @override
  Future<Response<DefaultGoalTrackerModelReturnModel>>
      _apiV1ManifestGetdefaultgoaltrackerGet({int? id}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getdefaultgoaltracker');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<DefaultGoalTrackerModelReturnModel,
        DefaultGoalTrackerModelReturnModel>($request);
  }

  @override
  Future<Response<DefaultGoalTrackerModelReturnModel>>
      _apiV1ManifestCreatedefaultgoaltrackerPost(
          {required DefaultGoalTrackerCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/createdefaultgoaltracker');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DefaultGoalTrackerModelReturnModel,
        DefaultGoalTrackerModelReturnModel>($request);
  }

  @override
  Future<Response<DefaultGoalTrackerModelReturnListModel>>
      _apiV1ManifestCreatemultipledefaultgoaltrackerPost(
          {required MultipleDefaultGoalTrackerCreateModel? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/createmultipledefaultgoaltracker');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DefaultGoalTrackerModelReturnListModel,
        DefaultGoalTrackerModelReturnListModel>($request);
  }

  @override
  Future<Response<DefaultGoalTrackerModelReturnModel>>
      _apiV1ManifestUpdatedefaultgoaltrackerPost(
          {required DefaultGoalTrackerUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/updatedefaultgoaltracker');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DefaultGoalTrackerModelReturnModel,
        DefaultGoalTrackerModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestMarkdefaultgoaltrackerasdeletedDelete({int? id}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/markdefaultgoaltrackerasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestMarkdefaultgoaltrackerasnotdeletedPost(
          {required int? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/markdefaultgoaltrackerasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<DefaultGoalTrackerCategoryModelReturnListModel>>
      _apiV1ManifestGetdefaultgoaltrackercategorysGet({
    bool? showDeleted,
    int? defaultCategoryId,
    int? defaultGoalTrackerId,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getdefaultgoaltrackercategorys');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'DefaultCategoryId': defaultCategoryId,
      'DefaultGoalTrackerId': defaultGoalTrackerId,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<DefaultGoalTrackerCategoryModelReturnListModel,
        DefaultGoalTrackerCategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<DefaultGoalTrackerCategoryModelReturnModel>>
      _apiV1ManifestGetdefaultgoaltrackercategoryGet({int? id}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getdefaultgoaltrackercategory');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<DefaultGoalTrackerCategoryModelReturnModel,
        DefaultGoalTrackerCategoryModelReturnModel>($request);
  }

  @override
  Future<Response<DefaultGoalTrackerCategoryModelReturnModel>>
      _apiV1ManifestCreatedefaultgoaltrackercategoryPost(
          {required DefaultGoalTrackerCategoryCreateModel? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/createdefaultgoaltrackercategory');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DefaultGoalTrackerCategoryModelReturnModel,
        DefaultGoalTrackerCategoryModelReturnModel>($request);
  }

  @override
  Future<Response<DefaultGoalTrackerCategoryModelReturnListModel>>
      _apiV1ManifestCreatemultipledefaultgoaltrackercategoryPost(
          {required MultipleDefaultGoalTrackerCategoryCreateModel? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/createmultipledefaultgoaltrackercategory');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DefaultGoalTrackerCategoryModelReturnListModel,
        DefaultGoalTrackerCategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<DefaultGoalTrackerCategoryModelReturnModel>>
      _apiV1ManifestUpdatedefaultgoaltrackercategoryPost(
          {required DefaultGoalTrackerCategoryUpdateModel? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/updatedefaultgoaltrackercategory');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DefaultGoalTrackerCategoryModelReturnModel,
        DefaultGoalTrackerCategoryModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestMarkdefaultgoaltrackercategoryasdeletedDelete({int? id}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/markdefaultgoaltrackercategoryasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestMarkdefaultgoaltrackercategoryasnotdeletedPost(
          {required int? body}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/markdefaultgoaltrackercategoryasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<DefaultGoalTrackerCategoryModelReturnListModel>>
      _apiV1ManifestGetdefaultgoaltrackercategorybydefaultcategoryidGet(
          {int? defaultCategoryId}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/getdefaultgoaltrackercategorybydefaultcategoryid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'defaultCategoryId': defaultCategoryId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<DefaultGoalTrackerCategoryModelReturnListModel,
        DefaultGoalTrackerCategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<DefaultGoalTrackerCategoryModelReturnListModel>>
      _apiV1ManifestGetdefaultgoaltrackercategorybydefaultcategoryidsearchGet(
          {required DefaultGoalTrackerCategoryByDefaultCategoryIdSearchCriteria?
              body}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/getdefaultgoaltrackercategorybydefaultcategoryidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DefaultGoalTrackerCategoryModelReturnListModel,
        DefaultGoalTrackerCategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<DefaultCategoryModelReturnListModel>>
      _apiV1ManifestGetdefaultcategorybydefaultgoaltrackercategoryidGet(
          {int? defaultGoalTrackerCategoryId}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/getdefaultcategorybydefaultgoaltrackercategoryid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'defaultGoalTrackerCategoryId': defaultGoalTrackerCategoryId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<DefaultCategoryModelReturnListModel,
        DefaultCategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<DefaultCategoryModelReturnListModel>>
      _apiV1ManifestGetdefaultcategorybydefaultgoaltrackercategoryidsearchGet(
          {required DefaultCategoryByDefaultGoalTrackerCategoryIdSearchCriteria?
              body}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/getdefaultcategorybydefaultgoaltrackercategoryidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DefaultCategoryModelReturnListModel,
        DefaultCategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<DefaultGoalTrackerCategoryModelReturnListModel>>
      _apiV1ManifestGetdefaultgoaltrackercategorybydefaultgoaltrackeridGet(
          {int? defaultGoalTrackerId}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/getdefaultgoaltrackercategorybydefaultgoaltrackerid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'defaultGoalTrackerId': defaultGoalTrackerId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<DefaultGoalTrackerCategoryModelReturnListModel,
        DefaultGoalTrackerCategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<DefaultGoalTrackerCategoryModelReturnListModel>>
      _apiV1ManifestGetdefaultgoaltrackercategorybydefaultgoaltrackeridsearchGet(
          {required DefaultGoalTrackerCategoryByDefaultGoalTrackerIdSearchCriteria?
              body}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/getdefaultgoaltrackercategorybydefaultgoaltrackeridsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DefaultGoalTrackerCategoryModelReturnListModel,
        DefaultGoalTrackerCategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<DefaultGoalTrackerModelReturnListModel>>
      _apiV1ManifestGetdefaultgoaltrackerbydefaultgoaltrackercategoryidGet(
          {int? defaultGoalTrackerCategoryId}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/getdefaultgoaltrackerbydefaultgoaltrackercategoryid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'defaultGoalTrackerCategoryId': defaultGoalTrackerCategoryId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<DefaultGoalTrackerModelReturnListModel,
        DefaultGoalTrackerModelReturnListModel>($request);
  }

  @override
  Future<Response<DefaultGoalTrackerModelReturnListModel>>
      _apiV1ManifestGetdefaultgoaltrackerbydefaultgoaltrackercategoryidsearchGet(
          {required DefaultGoalTrackerByDefaultGoalTrackerCategoryIdSearchCriteria?
              body}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/getdefaultgoaltrackerbydefaultgoaltrackercategoryidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DefaultGoalTrackerModelReturnListModel,
        DefaultGoalTrackerModelReturnListModel>($request);
  }

  @override
  Future<Response<DefaultMeditationFileModelReturnListModel>>
      _apiV1ManifestGetdefaultmeditationfilesGet({
    bool? showDeleted,
    List<int>? predefinedMeditationTypeIds,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/manifest/getdefaultmeditationfiles');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'PredefinedMeditationTypeIds': predefinedMeditationTypeIds,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<DefaultMeditationFileModelReturnListModel,
        DefaultMeditationFileModelReturnListModel>($request);
  }

  @override
  Future<Response<DefaultMeditationFileModelReturnModel>>
      _apiV1ManifestGetdefaultmeditationfileGet({int? id}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getdefaultmeditationfile');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<DefaultMeditationFileModelReturnModel,
        DefaultMeditationFileModelReturnModel>($request);
  }

  @override
  Future<Response<DefaultMeditationFileModelReturnModel>>
      _apiV1ManifestCreatedefaultmeditationfilePost(
          {required DefaultMeditationFileCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/createdefaultmeditationfile');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DefaultMeditationFileModelReturnModel,
        DefaultMeditationFileModelReturnModel>($request);
  }

  @override
  Future<Response<DefaultMeditationFileModelReturnListModel>>
      _apiV1ManifestCreatemultipledefaultmeditationfilePost(
          {required MultipleDefaultMeditationFileCreateModel? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/createmultipledefaultmeditationfile');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DefaultMeditationFileModelReturnListModel,
        DefaultMeditationFileModelReturnListModel>($request);
  }

  @override
  Future<Response<DefaultMeditationFileModelReturnModel>>
      _apiV1ManifestUpdatedefaultmeditationfilePost(
          {required DefaultMeditationFileUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/updatedefaultmeditationfile');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DefaultMeditationFileModelReturnModel,
        DefaultMeditationFileModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestMarkdefaultmeditationfileasdeletedDelete({int? id}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/markdefaultmeditationfileasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestMarkdefaultmeditationfileasnotdeletedPost(
          {required int? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/markdefaultmeditationfileasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<DefaultVisionBoardPictureModelReturnListModel>>
      _apiV1ManifestGetdefaultvisionboardpicturesGet({
    bool? showDeleted,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getdefaultvisionboardpictures');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<DefaultVisionBoardPictureModelReturnListModel,
        DefaultVisionBoardPictureModelReturnListModel>($request);
  }

  @override
  Future<Response<DefaultVisionBoardPictureModelReturnModel>>
      _apiV1ManifestGetdefaultvisionboardpictureGet({int? id}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getdefaultvisionboardpicture');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<DefaultVisionBoardPictureModelReturnModel,
        DefaultVisionBoardPictureModelReturnModel>($request);
  }

  @override
  Future<Response<DefaultVisionBoardPictureModelReturnModel>>
      _apiV1ManifestCreatedefaultvisionboardpicturePost(
          {required DefaultVisionBoardPictureCreateModel? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/createdefaultvisionboardpicture');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DefaultVisionBoardPictureModelReturnModel,
        DefaultVisionBoardPictureModelReturnModel>($request);
  }

  @override
  Future<Response<DefaultVisionBoardPictureModelReturnListModel>>
      _apiV1ManifestCreatemultipledefaultvisionboardpicturePost(
          {required MultipleDefaultVisionBoardPictureCreateModel? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/createmultipledefaultvisionboardpicture');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DefaultVisionBoardPictureModelReturnListModel,
        DefaultVisionBoardPictureModelReturnListModel>($request);
  }

  @override
  Future<Response<DefaultVisionBoardPictureModelReturnModel>>
      _apiV1ManifestUpdatedefaultvisionboardpicturePost(
          {required DefaultVisionBoardPictureUpdateModel? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/updatedefaultvisionboardpicture');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DefaultVisionBoardPictureModelReturnModel,
        DefaultVisionBoardPictureModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestMarkdefaultvisionboardpictureasdeletedDelete({int? id}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/markdefaultvisionboardpictureasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestMarkdefaultvisionboardpictureasnotdeletedPost(
          {required int? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/markdefaultvisionboardpictureasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<FeedModelReturnListModel>> _apiV1ManifestGetfeedsGet({
    bool? showDeleted,
    int? userId,
    int? dailyAffirmationSessionId,
    int? visionBoardSessionId,
    int? meditationSessionId,
    List<int>? predefinedMeditationTypeIds,
    int? goalTrackerId,
    List<int>? predefinedGoalStatusTypeIds,
    int? goalTrackerMileStoneId,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/manifest/getfeeds');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'UserId': userId,
      'DailyAffirmationSessionId': dailyAffirmationSessionId,
      'VisionBoardSessionId': visionBoardSessionId,
      'MeditationSessionId': meditationSessionId,
      'PredefinedMeditationTypeIds': predefinedMeditationTypeIds,
      'GoalTrackerId': goalTrackerId,
      'PredefinedGoalStatusTypeIds': predefinedGoalStatusTypeIds,
      'GoalTrackerMileStoneId': goalTrackerMileStoneId,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client
        .send<FeedModelReturnListModel, FeedModelReturnListModel>($request);
  }

  @override
  Future<Response<FeedModelReturnModel>> _apiV1ManifestGetfeedGet({int? id}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getfeed');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<FeedModelReturnModel, FeedModelReturnModel>($request);
  }

  @override
  Future<Response<FeedModelReturnModel>> _apiV1ManifestCreatefeedPost(
      {required FeedCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/createfeed');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<FeedModelReturnModel, FeedModelReturnModel>($request);
  }

  @override
  Future<Response<FeedModelReturnListModel>>
      _apiV1ManifestCreatemultiplefeedPost(
          {required MultipleFeedCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/createmultiplefeed');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client
        .send<FeedModelReturnListModel, FeedModelReturnListModel>($request);
  }

  @override
  Future<Response<FeedModelReturnModel>> _apiV1ManifestUpdatefeedPost(
      {required FeedUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/updatefeed');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<FeedModelReturnModel, FeedModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>> _apiV1ManifestMarkfeedasdeletedDelete(
      {int? id}) {
    final Uri $url = Uri.parse('/api/v1/manifest/markfeedasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>> _apiV1ManifestMarkfeedasnotdeletedPost(
      {required int? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/markfeedasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<FeedModelReturnListModel>> _apiV1ManifestGetfeedbyuseridGet(
      {int? userId}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getfeedbyuserid');
    final Map<String, dynamic> $params = <String, dynamic>{'userId': userId};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client
        .send<FeedModelReturnListModel, FeedModelReturnListModel>($request);
  }

  @override
  Future<Response<FeedModelReturnListModel>>
      _apiV1ManifestGetfeedbyuseridsearchGet(
          {required FeedByUserIdSearchCriteria? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getfeedbyuseridsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client
        .send<FeedModelReturnListModel, FeedModelReturnListModel>($request);
  }

  @override
  Future<Response<UserModelReturnListModel>> _apiV1ManifestGetuserbyfeedidGet(
      {int? feedId}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getuserbyfeedid');
    final Map<String, dynamic> $params = <String, dynamic>{'feedId': feedId};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client
        .send<UserModelReturnListModel, UserModelReturnListModel>($request);
  }

  @override
  Future<Response<UserModelReturnListModel>>
      _apiV1ManifestGetuserbyfeedidsearchGet(
          {required UserByFeedIdSearchCriteria? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getuserbyfeedidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client
        .send<UserModelReturnListModel, UserModelReturnListModel>($request);
  }

  @override
  Future<Response<FeedModelReturnListModel>>
      _apiV1ManifestGetfeedbydailyaffirmationsessionidGet(
          {int? dailyAffirmationSessionId}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getfeedbydailyaffirmationsessionid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'dailyAffirmationSessionId': dailyAffirmationSessionId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client
        .send<FeedModelReturnListModel, FeedModelReturnListModel>($request);
  }

  @override
  Future<Response<FeedModelReturnListModel>>
      _apiV1ManifestGetfeedbydailyaffirmationsessionidsearchGet(
          {required FeedByDailyAffirmationSessionIdSearchCriteria? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getfeedbydailyaffirmationsessionidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client
        .send<FeedModelReturnListModel, FeedModelReturnListModel>($request);
  }

  @override
  Future<Response<DailyAffirmationSessionModelReturnListModel>>
      _apiV1ManifestGetdailyaffirmationsessionbyfeedidGet({int? feedId}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getdailyaffirmationsessionbyfeedid');
    final Map<String, dynamic> $params = <String, dynamic>{'feedId': feedId};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<DailyAffirmationSessionModelReturnListModel,
        DailyAffirmationSessionModelReturnListModel>($request);
  }

  @override
  Future<Response<DailyAffirmationSessionModelReturnListModel>>
      _apiV1ManifestGetdailyaffirmationsessionbyfeedidsearchGet(
          {required DailyAffirmationSessionByFeedIdSearchCriteria? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getdailyaffirmationsessionbyfeedidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DailyAffirmationSessionModelReturnListModel,
        DailyAffirmationSessionModelReturnListModel>($request);
  }

  @override
  Future<Response<FeedModelReturnListModel>>
      _apiV1ManifestGetfeedbyvisionboardsessionidGet(
          {int? visionBoardSessionId}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getfeedbyvisionboardsessionid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'visionBoardSessionId': visionBoardSessionId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client
        .send<FeedModelReturnListModel, FeedModelReturnListModel>($request);
  }

  @override
  Future<Response<FeedModelReturnListModel>>
      _apiV1ManifestGetfeedbyvisionboardsessionidsearchGet(
          {required FeedByVisionBoardSessionIdSearchCriteria? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getfeedbyvisionboardsessionidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client
        .send<FeedModelReturnListModel, FeedModelReturnListModel>($request);
  }

  @override
  Future<Response<VisionBoardSessionModelReturnListModel>>
      _apiV1ManifestGetvisionboardsessionbyfeedidGet({int? feedId}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getvisionboardsessionbyfeedid');
    final Map<String, dynamic> $params = <String, dynamic>{'feedId': feedId};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<VisionBoardSessionModelReturnListModel,
        VisionBoardSessionModelReturnListModel>($request);
  }

  @override
  Future<Response<VisionBoardSessionModelReturnListModel>>
      _apiV1ManifestGetvisionboardsessionbyfeedidsearchGet(
          {required VisionBoardSessionByFeedIdSearchCriteria? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getvisionboardsessionbyfeedidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<VisionBoardSessionModelReturnListModel,
        VisionBoardSessionModelReturnListModel>($request);
  }

  @override
  Future<Response<FeedModelReturnListModel>>
      _apiV1ManifestGetfeedbymeditationsessionidGet(
          {int? meditationSessionId}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getfeedbymeditationsessionid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'meditationSessionId': meditationSessionId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client
        .send<FeedModelReturnListModel, FeedModelReturnListModel>($request);
  }

  @override
  Future<Response<FeedModelReturnListModel>>
      _apiV1ManifestGetfeedbymeditationsessionidsearchGet(
          {required FeedByMeditationSessionIdSearchCriteria? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getfeedbymeditationsessionidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client
        .send<FeedModelReturnListModel, FeedModelReturnListModel>($request);
  }

  @override
  Future<Response<MeditationSessionModelReturnListModel>>
      _apiV1ManifestGetmeditationsessionbyfeedidGet({int? feedId}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getmeditationsessionbyfeedid');
    final Map<String, dynamic> $params = <String, dynamic>{'feedId': feedId};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<MeditationSessionModelReturnListModel,
        MeditationSessionModelReturnListModel>($request);
  }

  @override
  Future<Response<MeditationSessionModelReturnListModel>>
      _apiV1ManifestGetmeditationsessionbyfeedidsearchGet(
          {required MeditationSessionByFeedIdSearchCriteria? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getmeditationsessionbyfeedidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<MeditationSessionModelReturnListModel,
        MeditationSessionModelReturnListModel>($request);
  }

  @override
  Future<Response<FeedModelReturnListModel>>
      _apiV1ManifestGetfeedbygoaltrackeridGet({int? goalTrackerId}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getfeedbygoaltrackerid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'goalTrackerId': goalTrackerId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client
        .send<FeedModelReturnListModel, FeedModelReturnListModel>($request);
  }

  @override
  Future<Response<FeedModelReturnListModel>>
      _apiV1ManifestGetfeedbygoaltrackeridsearchGet(
          {required FeedByGoalTrackerIdSearchCriteria? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getfeedbygoaltrackeridsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client
        .send<FeedModelReturnListModel, FeedModelReturnListModel>($request);
  }

  @override
  Future<Response<GoalTrackerModelReturnListModel>>
      _apiV1ManifestGetgoaltrackerbyfeedidGet({int? feedId}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getgoaltrackerbyfeedid');
    final Map<String, dynamic> $params = <String, dynamic>{'feedId': feedId};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<GoalTrackerModelReturnListModel,
        GoalTrackerModelReturnListModel>($request);
  }

  @override
  Future<Response<GoalTrackerModelReturnListModel>>
      _apiV1ManifestGetgoaltrackerbyfeedidsearchGet(
          {required GoalTrackerByFeedIdSearchCriteria? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getgoaltrackerbyfeedidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<GoalTrackerModelReturnListModel,
        GoalTrackerModelReturnListModel>($request);
  }

  @override
  Future<Response<FeedModelReturnListModel>>
      _apiV1ManifestGetfeedbygoaltrackermilestoneidGet(
          {int? goalTrackerMileStoneId}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getfeedbygoaltrackermilestoneid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'goalTrackerMileStoneId': goalTrackerMileStoneId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client
        .send<FeedModelReturnListModel, FeedModelReturnListModel>($request);
  }

  @override
  Future<Response<FeedModelReturnListModel>>
      _apiV1ManifestGetfeedbygoaltrackermilestoneidsearchGet(
          {required FeedByGoalTrackerMileStoneIdSearchCriteria? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getfeedbygoaltrackermilestoneidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client
        .send<FeedModelReturnListModel, FeedModelReturnListModel>($request);
  }

  @override
  Future<Response<GoalTrackerMileStoneModelReturnListModel>>
      _apiV1ManifestGetgoaltrackermilestonebyfeedidGet({int? feedId}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getgoaltrackermilestonebyfeedid');
    final Map<String, dynamic> $params = <String, dynamic>{'feedId': feedId};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<GoalTrackerMileStoneModelReturnListModel,
        GoalTrackerMileStoneModelReturnListModel>($request);
  }

  @override
  Future<Response<GoalTrackerMileStoneModelReturnListModel>>
      _apiV1ManifestGetgoaltrackermilestonebyfeedidsearchGet(
          {required GoalTrackerMileStoneByFeedIdSearchCriteria? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getgoaltrackermilestonebyfeedidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<GoalTrackerMileStoneModelReturnListModel,
        GoalTrackerMileStoneModelReturnListModel>($request);
  }

  @override
  Future<Response<GoalTrackerModelReturnListModel>>
      _apiV1ManifestGetgoaltrackersbycategoryidGet({int? categoryId}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getgoaltrackersbycategoryid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'categoryId': categoryId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<GoalTrackerModelReturnListModel,
        GoalTrackerModelReturnListModel>($request);
  }

  @override
  Future<Response<GoalTrackerModelReturnListModel>>
      _apiV1ManifestGetgoaltrackersGet({
    bool? showDeleted,
    List<int>? predefinedGoalStatusTypeIds,
    int? userId,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/manifest/getgoaltrackers');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'PredefinedGoalStatusTypeIds': predefinedGoalStatusTypeIds,
      'UserId': userId,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<GoalTrackerModelReturnListModel,
        GoalTrackerModelReturnListModel>($request);
  }

  @override
  Future<Response<GoalTrackerModelReturnModel>> _apiV1ManifestGetgoaltrackerGet(
      {int? id}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getgoaltracker');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<GoalTrackerModelReturnModel,
        GoalTrackerModelReturnModel>($request);
  }

  @override
  Future<Response<GoalTrackerModelReturnModel>>
      _apiV1ManifestCreategoaltrackerPost(
          {required GoalTrackerCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/creategoaltracker');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<GoalTrackerModelReturnModel,
        GoalTrackerModelReturnModel>($request);
  }

  @override
  Future<Response<GoalTrackerModelReturnListModel>>
      _apiV1ManifestCreatemultiplegoaltrackerPost(
          {required MultipleGoalTrackerCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/createmultiplegoaltracker');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<GoalTrackerModelReturnListModel,
        GoalTrackerModelReturnListModel>($request);
  }

  @override
  Future<Response<GoalTrackerModelReturnModel>>
      _apiV1ManifestUpdategoaltrackerPost(
          {required GoalTrackerUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/updategoaltracker');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<GoalTrackerModelReturnModel,
        GoalTrackerModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestMarkgoaltrackerasdeletedDelete({int? id}) {
    final Uri $url = Uri.parse('/api/v1/manifest/markgoaltrackerasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestMarkgoaltrackerasnotdeletedPost({required int? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/markgoaltrackerasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<GoalTrackerModelReturnListModel>>
      _apiV1ManifestGetgoaltrackerbyuseridGet({int? userId}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getgoaltrackerbyuserid');
    final Map<String, dynamic> $params = <String, dynamic>{'userId': userId};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<GoalTrackerModelReturnListModel,
        GoalTrackerModelReturnListModel>($request);
  }

  @override
  Future<Response<GoalTrackerModelReturnListModel>>
      _apiV1ManifestGetgoaltrackerbyuseridsearchGet(
          {required GoalTrackerByUserIdSearchCriteria? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getgoaltrackerbyuseridsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<GoalTrackerModelReturnListModel,
        GoalTrackerModelReturnListModel>($request);
  }

  @override
  Future<Response<UserModelReturnListModel>>
      _apiV1ManifestGetuserbygoaltrackeridGet({int? goalTrackerId}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getuserbygoaltrackerid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'goalTrackerId': goalTrackerId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client
        .send<UserModelReturnListModel, UserModelReturnListModel>($request);
  }

  @override
  Future<Response<UserModelReturnListModel>>
      _apiV1ManifestGetuserbygoaltrackeridsearchGet(
          {required UserByGoalTrackerIdSearchCriteria? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getuserbygoaltrackeridsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client
        .send<UserModelReturnListModel, UserModelReturnListModel>($request);
  }

  @override
  Future<Response<CategoryModelReturnListModel>>
      _apiV1ManifestGetalldistinctcategorysforallgoaltrackerforcurrentuserGet() {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/getalldistinctcategorysforallgoaltrackerforcurrentuser');
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
    );
    return client.send<CategoryModelReturnListModel,
        CategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<GoalTrackerCategoryModelReturnListModel>>
      _apiV1ManifestGetgoaltrackercategorysGet({
    bool? showDeleted,
    int? categoryId,
    int? goalTrackerId,
    int? userId,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/manifest/getgoaltrackercategorys');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'CategoryId': categoryId,
      'GoalTrackerId': goalTrackerId,
      'UserId': userId,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<GoalTrackerCategoryModelReturnListModel,
        GoalTrackerCategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<GoalTrackerCategoryModelReturnModel>>
      _apiV1ManifestGetgoaltrackercategoryGet({int? id}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getgoaltrackercategory');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<GoalTrackerCategoryModelReturnModel,
        GoalTrackerCategoryModelReturnModel>($request);
  }

  @override
  Future<Response<GoalTrackerCategoryModelReturnModel>>
      _apiV1ManifestCreategoaltrackercategoryPost(
          {required GoalTrackerCategoryCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/creategoaltrackercategory');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<GoalTrackerCategoryModelReturnModel,
        GoalTrackerCategoryModelReturnModel>($request);
  }

  @override
  Future<Response<GoalTrackerCategoryModelReturnListModel>>
      _apiV1ManifestCreatemultiplegoaltrackercategoryPost(
          {required MultipleGoalTrackerCategoryCreateModel? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/createmultiplegoaltrackercategory');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<GoalTrackerCategoryModelReturnListModel,
        GoalTrackerCategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<GoalTrackerCategoryModelReturnModel>>
      _apiV1ManifestUpdategoaltrackercategoryPost(
          {required GoalTrackerCategoryUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/updategoaltrackercategory');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<GoalTrackerCategoryModelReturnModel,
        GoalTrackerCategoryModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestMarkgoaltrackercategoryasdeletedDelete({int? id}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/markgoaltrackercategoryasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestMarkgoaltrackercategoryasnotdeletedPost(
          {required int? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/markgoaltrackercategoryasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<GoalTrackerCategoryModelReturnListModel>>
      _apiV1ManifestGetgoaltrackercategorybycategoryidGet({int? categoryId}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getgoaltrackercategorybycategoryid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'categoryId': categoryId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<GoalTrackerCategoryModelReturnListModel,
        GoalTrackerCategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<GoalTrackerCategoryModelReturnListModel>>
      _apiV1ManifestGetgoaltrackercategorybycategoryidsearchGet(
          {required GoalTrackerCategoryByCategoryIdSearchCriteria? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getgoaltrackercategorybycategoryidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<GoalTrackerCategoryModelReturnListModel,
        GoalTrackerCategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<CategoryModelReturnListModel>>
      _apiV1ManifestGetcategorybygoaltrackercategoryidGet(
          {int? goalTrackerCategoryId}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getcategorybygoaltrackercategoryid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'goalTrackerCategoryId': goalTrackerCategoryId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<CategoryModelReturnListModel,
        CategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<CategoryModelReturnListModel>>
      _apiV1ManifestGetcategorybygoaltrackercategoryidsearchGet(
          {required CategoryByGoalTrackerCategoryIdSearchCriteria? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getcategorybygoaltrackercategoryidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<CategoryModelReturnListModel,
        CategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<GoalTrackerCategoryModelReturnListModel>>
      _apiV1ManifestGetgoaltrackercategorybygoaltrackeridGet(
          {int? goalTrackerId}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getgoaltrackercategorybygoaltrackerid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'goalTrackerId': goalTrackerId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<GoalTrackerCategoryModelReturnListModel,
        GoalTrackerCategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<GoalTrackerCategoryModelReturnListModel>>
      _apiV1ManifestGetgoaltrackercategorybygoaltrackeridsearchGet(
          {required GoalTrackerCategoryByGoalTrackerIdSearchCriteria? body}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/getgoaltrackercategorybygoaltrackeridsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<GoalTrackerCategoryModelReturnListModel,
        GoalTrackerCategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<GoalTrackerModelReturnListModel>>
      _apiV1ManifestGetgoaltrackerbygoaltrackercategoryidGet(
          {int? goalTrackerCategoryId}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getgoaltrackerbygoaltrackercategoryid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'goalTrackerCategoryId': goalTrackerCategoryId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<GoalTrackerModelReturnListModel,
        GoalTrackerModelReturnListModel>($request);
  }

  @override
  Future<Response<GoalTrackerModelReturnListModel>>
      _apiV1ManifestGetgoaltrackerbygoaltrackercategoryidsearchGet(
          {required GoalTrackerByGoalTrackerCategoryIdSearchCriteria? body}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/getgoaltrackerbygoaltrackercategoryidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<GoalTrackerModelReturnListModel,
        GoalTrackerModelReturnListModel>($request);
  }

  @override
  Future<Response<GoalTrackerCategoryModelReturnListModel>>
      _apiV1ManifestGetgoaltrackercategorybyuseridGet({int? userId}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getgoaltrackercategorybyuserid');
    final Map<String, dynamic> $params = <String, dynamic>{'userId': userId};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<GoalTrackerCategoryModelReturnListModel,
        GoalTrackerCategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<GoalTrackerCategoryModelReturnListModel>>
      _apiV1ManifestGetgoaltrackercategorybyuseridsearchGet(
          {required GoalTrackerCategoryByUserIdSearchCriteria? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getgoaltrackercategorybyuseridsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<GoalTrackerCategoryModelReturnListModel,
        GoalTrackerCategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<UserModelReturnListModel>>
      _apiV1ManifestGetuserbygoaltrackercategoryidGet(
          {int? goalTrackerCategoryId}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getuserbygoaltrackercategoryid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'goalTrackerCategoryId': goalTrackerCategoryId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client
        .send<UserModelReturnListModel, UserModelReturnListModel>($request);
  }

  @override
  Future<Response<UserModelReturnListModel>>
      _apiV1ManifestGetuserbygoaltrackercategoryidsearchGet(
          {required UserByGoalTrackerCategoryIdSearchCriteria? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getuserbygoaltrackercategoryidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client
        .send<UserModelReturnListModel, UserModelReturnListModel>($request);
  }

  @override
  Future<Response<GoalTrackerMileStoneModelReturnListModel>>
      _apiV1ManifestGetgoaltrackermilestonesGet({
    bool? showDeleted,
    int? goalTrackerId,
    List<int>? predefinedGoalStatusTypeIds,
    int? userId,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/manifest/getgoaltrackermilestones');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'GoalTrackerId': goalTrackerId,
      'PredefinedGoalStatusTypeIds': predefinedGoalStatusTypeIds,
      'UserId': userId,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<GoalTrackerMileStoneModelReturnListModel,
        GoalTrackerMileStoneModelReturnListModel>($request);
  }

  @override
  Future<Response<GoalTrackerMileStoneModelReturnModel>>
      _apiV1ManifestGetgoaltrackermilestoneGet({int? id}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getgoaltrackermilestone');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<GoalTrackerMileStoneModelReturnModel,
        GoalTrackerMileStoneModelReturnModel>($request);
  }

  @override
  Future<Response<GoalTrackerMileStoneModelReturnModel>>
      _apiV1ManifestCreategoaltrackermilestonePost(
          {required GoalTrackerMileStoneCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/creategoaltrackermilestone');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<GoalTrackerMileStoneModelReturnModel,
        GoalTrackerMileStoneModelReturnModel>($request);
  }

  @override
  Future<Response<GoalTrackerMileStoneModelReturnListModel>>
      _apiV1ManifestCreatemultiplegoaltrackermilestonePost(
          {required MultipleGoalTrackerMileStoneCreateModel? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/createmultiplegoaltrackermilestone');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<GoalTrackerMileStoneModelReturnListModel,
        GoalTrackerMileStoneModelReturnListModel>($request);
  }

  @override
  Future<Response<GoalTrackerMileStoneModelReturnModel>>
      _apiV1ManifestUpdategoaltrackermilestonePost(
          {required GoalTrackerMileStoneUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/updategoaltrackermilestone');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<GoalTrackerMileStoneModelReturnModel,
        GoalTrackerMileStoneModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestMarkgoaltrackermilestoneasdeletedDelete({int? id}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/markgoaltrackermilestoneasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestMarkgoaltrackermilestoneasnotdeletedPost(
          {required int? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/markgoaltrackermilestoneasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<GoalTrackerMileStoneModelReturnListModel>>
      _apiV1ManifestGetgoaltrackermilestonebygoaltrackeridGet(
          {int? goalTrackerId}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getgoaltrackermilestonebygoaltrackerid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'goalTrackerId': goalTrackerId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<GoalTrackerMileStoneModelReturnListModel,
        GoalTrackerMileStoneModelReturnListModel>($request);
  }

  @override
  Future<Response<GoalTrackerMileStoneModelReturnListModel>>
      _apiV1ManifestGetgoaltrackermilestonebygoaltrackeridsearchGet(
          {required GoalTrackerMileStoneByGoalTrackerIdSearchCriteria? body}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/getgoaltrackermilestonebygoaltrackeridsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<GoalTrackerMileStoneModelReturnListModel,
        GoalTrackerMileStoneModelReturnListModel>($request);
  }

  @override
  Future<Response<GoalTrackerModelReturnListModel>>
      _apiV1ManifestGetgoaltrackerbygoaltrackermilestoneidGet(
          {int? goalTrackerMileStoneId}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getgoaltrackerbygoaltrackermilestoneid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'goalTrackerMileStoneId': goalTrackerMileStoneId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<GoalTrackerModelReturnListModel,
        GoalTrackerModelReturnListModel>($request);
  }

  @override
  Future<Response<GoalTrackerModelReturnListModel>>
      _apiV1ManifestGetgoaltrackerbygoaltrackermilestoneidsearchGet(
          {required GoalTrackerByGoalTrackerMileStoneIdSearchCriteria? body}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/getgoaltrackerbygoaltrackermilestoneidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<GoalTrackerModelReturnListModel,
        GoalTrackerModelReturnListModel>($request);
  }

  @override
  Future<Response<GoalTrackerMileStoneModelReturnListModel>>
      _apiV1ManifestGetgoaltrackermilestonebyuseridGet({int? userId}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getgoaltrackermilestonebyuserid');
    final Map<String, dynamic> $params = <String, dynamic>{'userId': userId};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<GoalTrackerMileStoneModelReturnListModel,
        GoalTrackerMileStoneModelReturnListModel>($request);
  }

  @override
  Future<Response<GoalTrackerMileStoneModelReturnListModel>>
      _apiV1ManifestGetgoaltrackermilestonebyuseridsearchGet(
          {required GoalTrackerMileStoneByUserIdSearchCriteria? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getgoaltrackermilestonebyuseridsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<GoalTrackerMileStoneModelReturnListModel,
        GoalTrackerMileStoneModelReturnListModel>($request);
  }

  @override
  Future<Response<UserModelReturnListModel>>
      _apiV1ManifestGetuserbygoaltrackermilestoneidGet(
          {int? goalTrackerMileStoneId}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getuserbygoaltrackermilestoneid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'goalTrackerMileStoneId': goalTrackerMileStoneId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client
        .send<UserModelReturnListModel, UserModelReturnListModel>($request);
  }

  @override
  Future<Response<UserModelReturnListModel>>
      _apiV1ManifestGetuserbygoaltrackermilestoneidsearchGet(
          {required UserByGoalTrackerMileStoneIdSearchCriteria? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getuserbygoaltrackermilestoneidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client
        .send<UserModelReturnListModel, UserModelReturnListModel>($request);
  }

  @override
  Future<Response<MeditationFileModelReturnListModel>>
      _apiV1ManifestGetmeditationfilesGet({
    bool? showDeleted,
    List<int>? predefinedMeditationTypeIds,
    int? userId,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/manifest/getmeditationfiles');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'PredefinedMeditationTypeIds': predefinedMeditationTypeIds,
      'UserId': userId,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<MeditationFileModelReturnListModel,
        MeditationFileModelReturnListModel>($request);
  }

  @override
  Future<Response<MeditationFileModelReturnModel>>
      _apiV1ManifestGetmeditationfileGet({int? id}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getmeditationfile');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<MeditationFileModelReturnModel,
        MeditationFileModelReturnModel>($request);
  }

  @override
  Future<Response<MeditationFileModelReturnModel>>
      _apiV1ManifestCreatemeditationfilePost(
          {required MeditationFileCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/createmeditationfile');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<MeditationFileModelReturnModel,
        MeditationFileModelReturnModel>($request);
  }

  @override
  Future<Response<MeditationFileModelReturnListModel>>
      _apiV1ManifestCreatemultiplemeditationfilePost(
          {required MultipleMeditationFileCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/createmultiplemeditationfile');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<MeditationFileModelReturnListModel,
        MeditationFileModelReturnListModel>($request);
  }

  @override
  Future<Response<MeditationFileModelReturnModel>>
      _apiV1ManifestUpdatemeditationfilePost(
          {required MeditationFileUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/updatemeditationfile');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<MeditationFileModelReturnModel,
        MeditationFileModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestMarkmeditationfileasdeletedDelete({int? id}) {
    final Uri $url = Uri.parse('/api/v1/manifest/markmeditationfileasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestMarkmeditationfileasnotdeletedPost({required int? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/markmeditationfileasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<MeditationFileModelReturnListModel>>
      _apiV1ManifestGetmeditationfilebyuseridGet({int? userId}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getmeditationfilebyuserid');
    final Map<String, dynamic> $params = <String, dynamic>{'userId': userId};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<MeditationFileModelReturnListModel,
        MeditationFileModelReturnListModel>($request);
  }

  @override
  Future<Response<MeditationFileModelReturnListModel>>
      _apiV1ManifestGetmeditationfilebyuseridsearchGet(
          {required MeditationFileByUserIdSearchCriteria? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getmeditationfilebyuseridsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<MeditationFileModelReturnListModel,
        MeditationFileModelReturnListModel>($request);
  }

  @override
  Future<Response<UserModelReturnListModel>>
      _apiV1ManifestGetuserbymeditationfileidGet({int? meditationFileId}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getuserbymeditationfileid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'meditationFileId': meditationFileId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client
        .send<UserModelReturnListModel, UserModelReturnListModel>($request);
  }

  @override
  Future<Response<UserModelReturnListModel>>
      _apiV1ManifestGetuserbymeditationfileidsearchGet(
          {required UserByMeditationFileIdSearchCriteria? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getuserbymeditationfileidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client
        .send<UserModelReturnListModel, UserModelReturnListModel>($request);
  }

  @override
  Future<Response<MeditationSessionModelReturnListModel>>
      _apiV1ManifestGetmeditationsessionsGet({
    bool? showDeleted,
    int? meditationFileId,
    int? userId,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/manifest/getmeditationsessions');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'MeditationFileId': meditationFileId,
      'UserId': userId,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<MeditationSessionModelReturnListModel,
        MeditationSessionModelReturnListModel>($request);
  }

  @override
  Future<Response<MeditationSessionModelReturnModel>>
      _apiV1ManifestGetmeditationsessionGet({int? id}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getmeditationsession');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<MeditationSessionModelReturnModel,
        MeditationSessionModelReturnModel>($request);
  }

  @override
  Future<Response<MeditationSessionModelReturnModel>>
      _apiV1ManifestCreatemeditationsessionPost(
          {required MeditationSessionCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/createmeditationsession');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<MeditationSessionModelReturnModel,
        MeditationSessionModelReturnModel>($request);
  }

  @override
  Future<Response<MeditationSessionModelReturnListModel>>
      _apiV1ManifestCreatemultiplemeditationsessionPost(
          {required MultipleMeditationSessionCreateModel? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/createmultiplemeditationsession');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<MeditationSessionModelReturnListModel,
        MeditationSessionModelReturnListModel>($request);
  }

  @override
  Future<Response<MeditationSessionModelReturnModel>>
      _apiV1ManifestUpdatemeditationsessionPost(
          {required MeditationSessionUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/updatemeditationsession');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<MeditationSessionModelReturnModel,
        MeditationSessionModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestMarkmeditationsessionasdeletedDelete({int? id}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/markmeditationsessionasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestMarkmeditationsessionasnotdeletedPost(
          {required int? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/markmeditationsessionasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<MeditationSessionModelReturnListModel>>
      _apiV1ManifestGetmeditationsessionbymeditationfileidGet(
          {int? meditationFileId}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getmeditationsessionbymeditationfileid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'meditationFileId': meditationFileId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<MeditationSessionModelReturnListModel,
        MeditationSessionModelReturnListModel>($request);
  }

  @override
  Future<Response<MeditationSessionModelReturnListModel>>
      _apiV1ManifestGetmeditationsessionbymeditationfileidsearchGet(
          {required MeditationSessionByMeditationFileIdSearchCriteria? body}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/getmeditationsessionbymeditationfileidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<MeditationSessionModelReturnListModel,
        MeditationSessionModelReturnListModel>($request);
  }

  @override
  Future<Response<MeditationFileModelReturnListModel>>
      _apiV1ManifestGetmeditationfilebymeditationsessionidGet(
          {int? meditationSessionId}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getmeditationfilebymeditationsessionid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'meditationSessionId': meditationSessionId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<MeditationFileModelReturnListModel,
        MeditationFileModelReturnListModel>($request);
  }

  @override
  Future<Response<MeditationFileModelReturnListModel>>
      _apiV1ManifestGetmeditationfilebymeditationsessionidsearchGet(
          {required MeditationFileByMeditationSessionIdSearchCriteria? body}) {
    final Uri $url = Uri.parse(
        '/api/v1/manifest/getmeditationfilebymeditationsessionidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<MeditationFileModelReturnListModel,
        MeditationFileModelReturnListModel>($request);
  }

  @override
  Future<Response<MeditationSessionModelReturnListModel>>
      _apiV1ManifestGetmeditationsessionbyuseridGet({int? userId}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getmeditationsessionbyuserid');
    final Map<String, dynamic> $params = <String, dynamic>{'userId': userId};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<MeditationSessionModelReturnListModel,
        MeditationSessionModelReturnListModel>($request);
  }

  @override
  Future<Response<MeditationSessionModelReturnListModel>>
      _apiV1ManifestGetmeditationsessionbyuseridsearchGet(
          {required MeditationSessionByUserIdSearchCriteria? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getmeditationsessionbyuseridsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<MeditationSessionModelReturnListModel,
        MeditationSessionModelReturnListModel>($request);
  }

  @override
  Future<Response<UserModelReturnListModel>>
      _apiV1ManifestGetuserbymeditationsessionidGet(
          {int? meditationSessionId}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getuserbymeditationsessionid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'meditationSessionId': meditationSessionId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client
        .send<UserModelReturnListModel, UserModelReturnListModel>($request);
  }

  @override
  Future<Response<UserModelReturnListModel>>
      _apiV1ManifestGetuserbymeditationsessionidsearchGet(
          {required UserByMeditationSessionIdSearchCriteria? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getuserbymeditationsessionidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client
        .send<UserModelReturnListModel, UserModelReturnListModel>($request);
  }

  @override
  Future<Response<VisionBoardPictureModelReturnListModel>>
      _apiV1ManifestGetvisionboardpicturesGet({
    bool? showDeleted,
    int? userId,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/manifest/getvisionboardpictures');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'UserId': userId,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<VisionBoardPictureModelReturnListModel,
        VisionBoardPictureModelReturnListModel>($request);
  }

  @override
  Future<Response<VisionBoardPictureModelReturnModel>>
      _apiV1ManifestGetvisionboardpictureGet({int? id}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getvisionboardpicture');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<VisionBoardPictureModelReturnModel,
        VisionBoardPictureModelReturnModel>($request);
  }

  @override
  Future<Response<VisionBoardPictureModelReturnModel>>
      _apiV1ManifestCreatevisionboardpicturePost({
    required List<int> UploadedFile,
    String? location,
    String? fileName,
    int? fileSize,
    int? userId,
  }) {
    final Uri $url = Uri.parse('/api/v1/manifest/createvisionboardpicture');
    final List<PartValue> $parts = <PartValue>[
      PartValue<String?>(
        'Location',
        location,
      ),
      PartValue<String?>(
        'FileName',
        fileName,
      ),
      PartValue<int?>(
        'FileSize',
        fileSize,
      ),
      PartValue<int?>(
        'UserId',
        userId,
      ),
      PartValueFile<List<int>>(
        'UploadedFile',
        UploadedFile,
      ),
    ];
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      parts: $parts,
      multipart: true,
    );
    return client.send<VisionBoardPictureModelReturnModel,
        VisionBoardPictureModelReturnModel>($request);
  }

  @override
  Future<Response<VisionBoardPictureModelReturnListModel>>
      _apiV1ManifestCreatemultiplevisionboardpicturePost(
          {required MultipleVisionBoardPictureCreateModel? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/createmultiplevisionboardpicture');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<VisionBoardPictureModelReturnListModel,
        VisionBoardPictureModelReturnListModel>($request);
  }

  @override
  Future<Response<VisionBoardPictureModelReturnModel>>
      _apiV1ManifestUpdatevisionboardpicturePost({
    required List<int> UploadedFile,
    int? visionBoardPictureId,
    String? location,
    String? fileName,
    int? fileSize,
    int? userId,
  }) {
    final Uri $url = Uri.parse('/api/v1/manifest/updatevisionboardpicture');
    final List<PartValue> $parts = <PartValue>[
      PartValue<int?>(
        'VisionBoardPictureId',
        visionBoardPictureId,
      ),
      PartValue<String?>(
        'Location',
        location,
      ),
      PartValue<String?>(
        'FileName',
        fileName,
      ),
      PartValue<int?>(
        'FileSize',
        fileSize,
      ),
      PartValue<int?>(
        'UserId',
        userId,
      ),
      PartValueFile<List<int>>(
        'UploadedFile',
        UploadedFile,
      ),
    ];
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      parts: $parts,
      multipart: true,
    );
    return client.send<VisionBoardPictureModelReturnModel,
        VisionBoardPictureModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestMarkvisionboardpictureasdeletedDelete({int? id}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/markvisionboardpictureasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestMarkvisionboardpictureasnotdeletedPost(
          {required int? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/markvisionboardpictureasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<VisionBoardPictureModelReturnListModel>>
      _apiV1ManifestGetvisionboardpicturebyuseridGet({int? userId}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getvisionboardpicturebyuserid');
    final Map<String, dynamic> $params = <String, dynamic>{'userId': userId};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<VisionBoardPictureModelReturnListModel,
        VisionBoardPictureModelReturnListModel>($request);
  }

  @override
  Future<Response<VisionBoardPictureModelReturnListModel>>
      _apiV1ManifestGetvisionboardpicturebyuseridsearchGet(
          {required VisionBoardPictureByUserIdSearchCriteria? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getvisionboardpicturebyuseridsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<VisionBoardPictureModelReturnListModel,
        VisionBoardPictureModelReturnListModel>($request);
  }

  @override
  Future<Response<UserModelReturnListModel>>
      _apiV1ManifestGetuserbyvisionboardpictureidGet(
          {int? visionBoardPictureId}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getuserbyvisionboardpictureid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'visionBoardPictureId': visionBoardPictureId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client
        .send<UserModelReturnListModel, UserModelReturnListModel>($request);
  }

  @override
  Future<Response<UserModelReturnListModel>>
      _apiV1ManifestGetuserbyvisionboardpictureidsearchGet(
          {required UserByVisionBoardPictureIdSearchCriteria? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getuserbyvisionboardpictureidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client
        .send<UserModelReturnListModel, UserModelReturnListModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestIsvisionboardpicturefilevalidGet(
          {int? visionBoardPictureId}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/isvisionboardpicturefilevalid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'visionBoardPictureId': visionBoardPictureId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<dynamic>> _apiV1ManifestDownloadvisionboardpicturefileGet(
      {int? visionBoardPictureId}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/downloadvisionboardpicturefile');
    final Map<String, dynamic> $params = <String, dynamic>{
      'visionBoardPictureId': visionBoardPictureId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<dynamic, dynamic>($request);
  }

  @override
  Future<Response<VisionBoardSessionModelReturnListModel>>
      _apiV1ManifestGetvisionboardsessionsGet({
    bool? showDeleted,
    int? userId,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/manifest/getvisionboardsessions');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'UserId': userId,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<VisionBoardSessionModelReturnListModel,
        VisionBoardSessionModelReturnListModel>($request);
  }

  @override
  Future<Response<VisionBoardSessionModelReturnModel>>
      _apiV1ManifestGetvisionboardsessionGet({int? id}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getvisionboardsession');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<VisionBoardSessionModelReturnModel,
        VisionBoardSessionModelReturnModel>($request);
  }

  @override
  Future<Response<VisionBoardSessionModelReturnModel>>
      _apiV1ManifestCreatevisionboardsessionPost(
          {required VisionBoardSessionCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/createvisionboardsession');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<VisionBoardSessionModelReturnModel,
        VisionBoardSessionModelReturnModel>($request);
  }

  @override
  Future<Response<VisionBoardSessionModelReturnListModel>>
      _apiV1ManifestCreatemultiplevisionboardsessionPost(
          {required MultipleVisionBoardSessionCreateModel? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/createmultiplevisionboardsession');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<VisionBoardSessionModelReturnListModel,
        VisionBoardSessionModelReturnListModel>($request);
  }

  @override
  Future<Response<VisionBoardSessionModelReturnModel>>
      _apiV1ManifestUpdatevisionboardsessionPost(
          {required VisionBoardSessionUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/updatevisionboardsession');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<VisionBoardSessionModelReturnModel,
        VisionBoardSessionModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestMarkvisionboardsessionasdeletedDelete({int? id}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/markvisionboardsessionasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestMarkvisionboardsessionasnotdeletedPost(
          {required int? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/markvisionboardsessionasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<VisionBoardSessionModelReturnListModel>>
      _apiV1ManifestGetvisionboardsessionbyuseridGet({int? userId}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getvisionboardsessionbyuserid');
    final Map<String, dynamic> $params = <String, dynamic>{'userId': userId};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<VisionBoardSessionModelReturnListModel,
        VisionBoardSessionModelReturnListModel>($request);
  }

  @override
  Future<Response<VisionBoardSessionModelReturnListModel>>
      _apiV1ManifestGetvisionboardsessionbyuseridsearchGet(
          {required VisionBoardSessionByUserIdSearchCriteria? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getvisionboardsessionbyuseridsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<VisionBoardSessionModelReturnListModel,
        VisionBoardSessionModelReturnListModel>($request);
  }

  @override
  Future<Response<UserModelReturnListModel>>
      _apiV1ManifestGetuserbyvisionboardsessionidGet(
          {int? visionBoardSessionId}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getuserbyvisionboardsessionid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'visionBoardSessionId': visionBoardSessionId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client
        .send<UserModelReturnListModel, UserModelReturnListModel>($request);
  }

  @override
  Future<Response<UserModelReturnListModel>>
      _apiV1ManifestGetuserbyvisionboardsessionidsearchGet(
          {required UserByVisionBoardSessionIdSearchCriteria? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getuserbyvisionboardsessionidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client
        .send<UserModelReturnListModel, UserModelReturnListModel>($request);
  }

  @override
  Future<Response<DailyJournalModelReturnListModel>>
      _apiV1ManifestGetdailyjournalsGet({
    bool? showDeleted,
    int? userId,
    List<int>? predefinedMoodTypeIds,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/manifest/getdailyjournals');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'UserId': userId,
      'PredefinedMoodTypeIds': predefinedMoodTypeIds,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<DailyJournalModelReturnListModel,
        DailyJournalModelReturnListModel>($request);
  }

  @override
  Future<Response<DailyJournalModelReturnModel>>
      _apiV1ManifestGetdailyjournalGet({int? id}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getdailyjournal');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<DailyJournalModelReturnModel,
        DailyJournalModelReturnModel>($request);
  }

  @override
  Future<Response<DailyJournalModelReturnModel>>
      _apiV1ManifestCreatedailyjournalPost(
          {required DailyJournalCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/createdailyjournal');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DailyJournalModelReturnModel,
        DailyJournalModelReturnModel>($request);
  }

  @override
  Future<Response<DailyJournalModelReturnListModel>>
      _apiV1ManifestCreatemultipledailyjournalPost(
          {required MultipleDailyJournalCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/createmultipledailyjournal');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DailyJournalModelReturnListModel,
        DailyJournalModelReturnListModel>($request);
  }

  @override
  Future<Response<DailyJournalModelReturnModel>>
      _apiV1ManifestUpdatedailyjournalPost(
          {required DailyJournalUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/updatedailyjournal');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DailyJournalModelReturnModel,
        DailyJournalModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestMarkdailyjournalasdeletedDelete({int? id}) {
    final Uri $url = Uri.parse('/api/v1/manifest/markdailyjournalasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1ManifestMarkdailyjournalasnotdeletedPost({required int? body}) {
    final Uri $url = Uri.parse('/api/v1/manifest/markdailyjournalasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<DailyJournalModelReturnListModel>>
      _apiV1ManifestGetdailyjournalbyuseridGet({int? userId}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getdailyjournalbyuserid');
    final Map<String, dynamic> $params = <String, dynamic>{'userId': userId};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<DailyJournalModelReturnListModel,
        DailyJournalModelReturnListModel>($request);
  }

  @override
  Future<Response<DailyJournalModelReturnListModel>>
      _apiV1ManifestGetdailyjournalbyuseridsearchGet(
          {required DailyJournalByUserIdSearchCriteria? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getdailyjournalbyuseridsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<DailyJournalModelReturnListModel,
        DailyJournalModelReturnListModel>($request);
  }

  @override
  Future<Response<UserModelReturnListModel>>
      _apiV1ManifestGetuserbydailyjournalidGet({int? dailyJournalId}) {
    final Uri $url = Uri.parse('/api/v1/manifest/getuserbydailyjournalid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'dailyJournalId': dailyJournalId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client
        .send<UserModelReturnListModel, UserModelReturnListModel>($request);
  }

  @override
  Future<Response<UserModelReturnListModel>>
      _apiV1ManifestGetuserbydailyjournalidsearchGet(
          {required UserByDailyJournalIdSearchCriteria? body}) {
    final Uri $url =
        Uri.parse('/api/v1/manifest/getuserbydailyjournalidsearch');
    final $body = body;
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client
        .send<UserModelReturnListModel, UserModelReturnListModel>($request);
  }

  @override
  Future<Response<CountryModelReturnListModel>>
      _apiV1SystemGetsupportedcountriesGet() {
    final Uri $url = Uri.parse('/api/v1/system/getsupportedcountries');
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
    );
    return client.send<CountryModelReturnListModel,
        CountryModelReturnListModel>($request);
  }

  @override
  Future<Response<CountryModelReturnListModel>>
      _apiV1SystemGetcountriesbykeywordGet({String? keyword}) {
    final Uri $url = Uri.parse('/api/v1/system/getcountriesbykeyword');
    final Map<String, dynamic> $params = <String, dynamic>{'keyword': keyword};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<CountryModelReturnListModel,
        CountryModelReturnListModel>($request);
  }

  @override
  Future<Response<StateModelReturnListModel>> _apiV1SystemGetstatesGet({
    int? countryId,
    bool? showDeleted,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/system/getstates');
    final Map<String, dynamic> $params = <String, dynamic>{
      'CountryId': countryId,
      'ShowDeleted': showDeleted,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client
        .send<StateModelReturnListModel, StateModelReturnListModel>($request);
  }

  @override
  Future<Response<StateModelReturnModel>> _apiV1SystemGetstateGet({int? id}) {
    final Uri $url = Uri.parse('/api/v1/system/getstate');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<StateModelReturnModel, StateModelReturnModel>($request);
  }

  @override
  Future<Response<LogConfigurationModelReturnModel>>
      _apiV1SystemGetlogconfigurationGet() {
    final Uri $url = Uri.parse('/api/v1/system/getlogconfiguration');
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
    );
    return client.send<LogConfigurationModelReturnModel,
        LogConfigurationModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>> _apiV1SystemSavelogconfigurationPost(
      {required LogConfigurationModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/savelogconfiguration');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<ManifestWebConfigurationModelReturnModel>>
      _apiV1SystemGetwebconfigurationGet() {
    final Uri $url = Uri.parse('/api/v1/system/getwebconfiguration');
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
    );
    return client.send<ManifestWebConfigurationModelReturnModel,
        ManifestWebConfigurationModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>> _apiV1SystemSavewebconfigurationPost(
      {required ManifestWebConfigurationModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/savewebconfiguration');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<AppConfigurationModelReturnModel>>
      _apiV1SystemGetappconfigurationGet() {
    final Uri $url = Uri.parse('/api/v1/system/getappconfiguration');
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
    );
    return client.send<AppConfigurationModelReturnModel,
        AppConfigurationModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>> _apiV1SystemSaveappconfigurationPost(
      {required AppConfigurationModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/saveappconfiguration');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<CommonConfigurationModelReturnModel>>
      _apiV1SystemGetcommonconfigurationGet() {
    final Uri $url = Uri.parse('/api/v1/system/getcommonconfiguration');
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
    );
    return client.send<CommonConfigurationModelReturnModel,
        CommonConfigurationModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>> _apiV1SystemSavecommonconfigurationPost(
      {required CommonConfigurationModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/savecommonconfiguration');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<CoordinatorConfigurationModelReturnModel>>
      _apiV1SystemGetcoordinatorconfigurationGet() {
    final Uri $url = Uri.parse('/api/v1/system/getcoordinatorconfiguration');
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
    );
    return client.send<CoordinatorConfigurationModelReturnModel,
        CoordinatorConfigurationModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemSavecoordinatorconfigurationPost(
          {required CoordinatorConfigurationModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/savecoordinatorconfiguration');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<ReportServerConfigurationModelReturnModel>>
      _apiV1SystemGetreportserverconfigurationGet() {
    final Uri $url = Uri.parse('/api/v1/system/getreportserverconfiguration');
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
    );
    return client.send<ReportServerConfigurationModelReturnModel,
        ReportServerConfigurationModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemSavereportserverconfigurationPost(
          {required ReportServerConfigurationModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/savereportserverconfiguration');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<ConfigurationGroupModelReturnListModel>>
      _apiV1SystemGetconfigurationgroupsGet({
    bool? showDeleted,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/system/getconfigurationgroups');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<ConfigurationGroupModelReturnListModel,
        ConfigurationGroupModelReturnListModel>($request);
  }

  @override
  Future<Response<ConfigurationGroupModelReturnModel>>
      _apiV1SystemGetconfigurationgroupGet({int? id}) {
    final Uri $url = Uri.parse('/api/v1/system/getconfigurationgroup');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<ConfigurationGroupModelReturnModel,
        ConfigurationGroupModelReturnModel>($request);
  }

  @override
  Future<Response<ConfigurationGroupModelReturnModel>>
      _apiV1SystemCreateconfigurationgroupPost(
          {required ConfigurationGroupCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/createconfigurationgroup');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<ConfigurationGroupModelReturnModel,
        ConfigurationGroupModelReturnModel>($request);
  }

  @override
  Future<Response<ConfigurationGroupModelReturnModel>>
      _apiV1SystemUpdateconfigurationgroupPost(
          {required ConfigurationGroupUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/updateconfigurationgroup');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<ConfigurationGroupModelReturnModel,
        ConfigurationGroupModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemMarkconfigurationgroupasdeletedDelete({int? id}) {
    final Uri $url =
        Uri.parse('/api/v1/system/markconfigurationgroupasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemMarkconfigurationgroupasnotdeletedPost({required int? body}) {
    final Uri $url =
        Uri.parse('/api/v1/system/markconfigurationgroupasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<ConfigurationItemModelReturnListModel>>
      _apiV1SystemGetconfigurationitemsGet({
    bool? showDeleted,
    int? configurationGroupId,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/system/getconfigurationitems');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'ConfigurationGroupId': configurationGroupId,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<ConfigurationItemModelReturnListModel,
        ConfigurationItemModelReturnListModel>($request);
  }

  @override
  Future<Response<ConfigurationItemModelReturnModel>>
      _apiV1SystemGetconfigurationitemGet({int? id}) {
    final Uri $url = Uri.parse('/api/v1/system/getconfigurationitem');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<ConfigurationItemModelReturnModel,
        ConfigurationItemModelReturnModel>($request);
  }

  @override
  Future<Response<ConfigurationItemModelReturnModel>>
      _apiV1SystemCreateconfigurationitemPost(
          {required ConfigurationItemCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/createconfigurationitem');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<ConfigurationItemModelReturnModel,
        ConfigurationItemModelReturnModel>($request);
  }

  @override
  Future<Response<ConfigurationItemModelReturnModel>>
      _apiV1SystemUpdateconfigurationitemPost(
          {required ConfigurationItemUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/updateconfigurationitem');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<ConfigurationItemModelReturnModel,
        ConfigurationItemModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemMarkconfigurationitemasdeletedDelete({int? id}) {
    final Uri $url = Uri.parse('/api/v1/system/markconfigurationitemasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemMarkconfigurationitemasnotdeletedPost({required int? body}) {
    final Uri $url =
        Uri.parse('/api/v1/system/markconfigurationitemasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<EnumGroupModelReturnListModel>> _apiV1SystemGetenumgroupsGet({
    bool? showDeleted,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/system/getenumgroups');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<EnumGroupModelReturnListModel,
        EnumGroupModelReturnListModel>($request);
  }

  @override
  Future<Response<EnumGroupModelReturnModel>> _apiV1SystemGetenumgroupGet(
      {int? id}) {
    final Uri $url = Uri.parse('/api/v1/system/getenumgroup');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client
        .send<EnumGroupModelReturnModel, EnumGroupModelReturnModel>($request);
  }

  @override
  Future<Response<EnumGroupModelReturnModel>> _apiV1SystemCreateenumgroupPost(
      {required EnumGroupCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/createenumgroup');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client
        .send<EnumGroupModelReturnModel, EnumGroupModelReturnModel>($request);
  }

  @override
  Future<Response<EnumGroupModelReturnModel>> _apiV1SystemUpdateenumgroupPost(
      {required EnumGroupUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/updateenumgroup');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client
        .send<EnumGroupModelReturnModel, EnumGroupModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>> _apiV1SystemMarkenumgroupasdeletedDelete(
      {int? id}) {
    final Uri $url = Uri.parse('/api/v1/system/markenumgroupasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemMarkenumgroupasnotdeletedPost({required int? body}) {
    final Uri $url = Uri.parse('/api/v1/system/markenumgroupasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<EnumTypeModelReturnListModel>> _apiV1SystemGetenumtypesGet({
    bool? showDeleted,
    int? enumGroupId,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/system/getenumtypes');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'EnumGroupId': enumGroupId,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<EnumTypeModelReturnListModel,
        EnumTypeModelReturnListModel>($request);
  }

  @override
  Future<Response<EnumTypeModelReturnModel>> _apiV1SystemGetenumtypeGet(
      {int? id}) {
    final Uri $url = Uri.parse('/api/v1/system/getenumtype');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client
        .send<EnumTypeModelReturnModel, EnumTypeModelReturnModel>($request);
  }

  @override
  Future<Response<EnumTypeModelReturnModel>> _apiV1SystemCreateenumtypePost(
      {required EnumTypeCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/createenumtype');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client
        .send<EnumTypeModelReturnModel, EnumTypeModelReturnModel>($request);
  }

  @override
  Future<Response<EnumTypeModelReturnModel>> _apiV1SystemUpdateenumtypePost(
      {required EnumTypeUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/updateenumtype');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client
        .send<EnumTypeModelReturnModel, EnumTypeModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>> _apiV1SystemMarkenumtypeasdeletedDelete(
      {int? id}) {
    final Uri $url = Uri.parse('/api/v1/system/markenumtypeasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>> _apiV1SystemMarkenumtypeasnotdeletedPost(
      {required int? body}) {
    final Uri $url = Uri.parse('/api/v1/system/markenumtypeasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<JobModelReturnListModel>> _apiV1SystemGetjobsGet({
    DateTime? startDate,
    DateTime? endDate,
    bool? showDeleted,
    List<int>? predefinedJobTypeIds,
    int? jobTemplateId,
    List<int>? predefinedJobOrReportTypeIds,
    List<int>? predefinedJobStatusTypeIds,
    int? submittedByUserId,
    int? executionProcessId,
    int? executionThreadId,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/system/getjobs');
    final Map<String, dynamic> $params = <String, dynamic>{
      'StartDate': startDate,
      'EndDate': endDate,
      'ShowDeleted': showDeleted,
      'PredefinedJobTypeIds': predefinedJobTypeIds,
      'JobTemplateId': jobTemplateId,
      'PredefinedJobOrReportTypeIds': predefinedJobOrReportTypeIds,
      'PredefinedJobStatusTypeIds': predefinedJobStatusTypeIds,
      'SubmittedByUserId': submittedByUserId,
      'ExecutionProcessId': executionProcessId,
      'ExecutionThreadId': executionThreadId,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client
        .send<JobModelReturnListModel, JobModelReturnListModel>($request);
  }

  @override
  Future<Response<JobModelReturnModel>> _apiV1SystemGetjobGet({int? id}) {
    final Uri $url = Uri.parse('/api/v1/system/getjob');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<JobModelReturnModel, JobModelReturnModel>($request);
  }

  @override
  Future<Response<JobModelReturnModel>> _apiV1SystemCreatejobPost(
      {required JobCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/createjob');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<JobModelReturnModel, JobModelReturnModel>($request);
  }

  @override
  Future<Response<JobModelReturnModel>>
      _apiV1SystemCreatejobfromscheduledjobGet({int? scheduleJobId}) {
    final Uri $url = Uri.parse('/api/v1/system/createjobfromscheduledjob');
    final Map<String, dynamic> $params = <String, dynamic>{
      'scheduleJobId': scheduleJobId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<JobModelReturnModel, JobModelReturnModel>($request);
  }

  @override
  Future<Response<JobModelReturnModel>>
      _apiV1SystemCreatejobfromjobtemplatePost(
          {required JobCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/createjobfromjobtemplate');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<JobModelReturnModel, JobModelReturnModel>($request);
  }

  @override
  Future<Response<JobModelReturnModel>> _apiV1SystemUpdatejobPost(
      {required JobUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/updatejob');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<JobModelReturnModel, JobModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>> _apiV1SystemMarkjobasdeletedDelete(
      {int? id}) {
    final Uri $url = Uri.parse('/api/v1/system/markjobasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>> _apiV1SystemMarkjobasnotdeletedPost(
      {required int? body}) {
    final Uri $url = Uri.parse('/api/v1/system/markjobasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<JobAttachmentModelReturnListModel>>
      _apiV1SystemGetjobattachmentsGet({
    bool? showDeleted,
    int? jobId,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/system/getjobattachments');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'JobId': jobId,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<JobAttachmentModelReturnListModel,
        JobAttachmentModelReturnListModel>($request);
  }

  @override
  Future<Response<JobAttachmentModelReturnModel>>
      _apiV1SystemGetjobattachmentGet({int? id}) {
    final Uri $url = Uri.parse('/api/v1/system/getjobattachment');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<JobAttachmentModelReturnModel,
        JobAttachmentModelReturnModel>($request);
  }

  @override
  Future<Response<JobAttachmentModelReturnModel>>
      _apiV1SystemCreatejobattachmentPost(
          {required JobAttachmentCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/createjobattachment');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<JobAttachmentModelReturnModel,
        JobAttachmentModelReturnModel>($request);
  }

  @override
  Future<Response<JobAttachmentModelReturnModel>>
      _apiV1SystemUpdatejobattachmentPost(
          {required JobAttachmentUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/updatejobattachment');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<JobAttachmentModelReturnModel,
        JobAttachmentModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemMarkjobattachmentasdeletedDelete({int? id}) {
    final Uri $url = Uri.parse('/api/v1/system/markjobattachmentasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemMarkjobattachmentasnotdeletedPost({required int? body}) {
    final Uri $url = Uri.parse('/api/v1/system/markjobattachmentasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<JobNotificationEmailModelReturnListModel>>
      _apiV1SystemGetjobnotificationemailsGet({
    bool? showDeleted,
    int? jobId,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/system/getjobnotificationemails');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'JobId': jobId,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<JobNotificationEmailModelReturnListModel,
        JobNotificationEmailModelReturnListModel>($request);
  }

  @override
  Future<Response<JobNotificationEmailModelReturnModel>>
      _apiV1SystemGetjobnotificationemailGet({int? id}) {
    final Uri $url = Uri.parse('/api/v1/system/getjobnotificationemail');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<JobNotificationEmailModelReturnModel,
        JobNotificationEmailModelReturnModel>($request);
  }

  @override
  Future<Response<JobNotificationEmailModelReturnModel>>
      _apiV1SystemCreatejobnotificationemailPost(
          {required JobNotificationEmailCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/createjobnotificationemail');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<JobNotificationEmailModelReturnModel,
        JobNotificationEmailModelReturnModel>($request);
  }

  @override
  Future<Response<JobNotificationEmailModelReturnModel>>
      _apiV1SystemUpdatejobnotificationemailPost(
          {required JobNotificationEmailUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/updatejobnotificationemail');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<JobNotificationEmailModelReturnModel,
        JobNotificationEmailModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemMarkjobnotificationemailasdeletedDelete({int? id}) {
    final Uri $url =
        Uri.parse('/api/v1/system/markjobnotificationemailasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemMarkjobnotificationemailasnotdeletedPost(
          {required int? body}) {
    final Uri $url =
        Uri.parse('/api/v1/system/markjobnotificationemailasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<JobTemplateCategoryModelReturnListModel>>
      _apiV1SystemGetjobtemplatecategorysGet({
    bool? showDeleted,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/system/getjobtemplatecategorys');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<JobTemplateCategoryModelReturnListModel,
        JobTemplateCategoryModelReturnListModel>($request);
  }

  @override
  Future<Response<JobTemplateCategoryModelReturnModel>>
      _apiV1SystemGetjobtemplatecategoryGet({int? id}) {
    final Uri $url = Uri.parse('/api/v1/system/getjobtemplatecategory');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<JobTemplateCategoryModelReturnModel,
        JobTemplateCategoryModelReturnModel>($request);
  }

  @override
  Future<Response<JobTemplateCategoryModelReturnModel>>
      _apiV1SystemCreatejobtemplatecategoryPost(
          {required JobTemplateCategoryCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/createjobtemplatecategory');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<JobTemplateCategoryModelReturnModel,
        JobTemplateCategoryModelReturnModel>($request);
  }

  @override
  Future<Response<JobTemplateCategoryModelReturnModel>>
      _apiV1SystemUpdatejobtemplatecategoryPost(
          {required JobTemplateCategoryUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/updatejobtemplatecategory');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<JobTemplateCategoryModelReturnModel,
        JobTemplateCategoryModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemMarkjobtemplatecategoryasdeletedDelete({int? id}) {
    final Uri $url =
        Uri.parse('/api/v1/system/markjobtemplatecategoryasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemMarkjobtemplatecategoryasnotdeletedPost(
          {required int? body}) {
    final Uri $url =
        Uri.parse('/api/v1/system/markjobtemplatecategoryasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<JobTemplateModelReturnListModel>>
      _apiV1SystemGetjobtemplatesGet({
    bool? showDeleted,
    List<int>? predefinedJobTypeIds,
    List<int>? predefinedJobOrReportTypeIds,
    int? jobTemplateCategoryId,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/system/getjobtemplates');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'PredefinedJobTypeIds': predefinedJobTypeIds,
      'PredefinedJobOrReportTypeIds': predefinedJobOrReportTypeIds,
      'JobTemplateCategoryId': jobTemplateCategoryId,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<JobTemplateModelReturnListModel,
        JobTemplateModelReturnListModel>($request);
  }

  @override
  Future<Response<JobTemplateModelReturnModel>> _apiV1SystemGetjobtemplateGet(
      {int? id}) {
    final Uri $url = Uri.parse('/api/v1/system/getjobtemplate');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<JobTemplateModelReturnModel,
        JobTemplateModelReturnModel>($request);
  }

  @override
  Future<Response<JobTemplateModelReturnModel>>
      _apiV1SystemCreatejobtemplatePost(
          {required JobTemplateCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/createjobtemplate');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<JobTemplateModelReturnModel,
        JobTemplateModelReturnModel>($request);
  }

  @override
  Future<Response<JobTemplateModelReturnModel>>
      _apiV1SystemUpdatejobtemplatePost(
          {required JobTemplateUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/updatejobtemplate');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<JobTemplateModelReturnModel,
        JobTemplateModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemMarkjobtemplateasdeletedDelete({int? id}) {
    final Uri $url = Uri.parse('/api/v1/system/markjobtemplateasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemMarkjobtemplateasnotdeletedPost({required int? body}) {
    final Uri $url = Uri.parse('/api/v1/system/markjobtemplateasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<JobTemplateParameterModelReturnListModel>>
      _apiV1SystemGetjobtemplateparametersGet({
    bool? showDeleted,
    int? jobTemplateId,
    List<int>? predefinedFieldControlTypeIds,
    List<int>? predefinedFieldTypeIds,
    List<int>? predefinedFieldPopulatorTypeIds,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/system/getjobtemplateparameters');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'JobTemplateId': jobTemplateId,
      'PredefinedFieldControlTypeIds': predefinedFieldControlTypeIds,
      'PredefinedFieldTypeIds': predefinedFieldTypeIds,
      'PredefinedFieldPopulatorTypeIds': predefinedFieldPopulatorTypeIds,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<JobTemplateParameterModelReturnListModel,
        JobTemplateParameterModelReturnListModel>($request);
  }

  @override
  Future<Response<JobTemplateParameterModelReturnModel>>
      _apiV1SystemGetjobtemplateparameterGet({int? id}) {
    final Uri $url = Uri.parse('/api/v1/system/getjobtemplateparameter');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<JobTemplateParameterModelReturnModel,
        JobTemplateParameterModelReturnModel>($request);
  }

  @override
  Future<Response<JobTemplateParameterModelReturnModel>>
      _apiV1SystemCreatejobtemplateparameterPost(
          {required JobTemplateParameterCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/createjobtemplateparameter');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<JobTemplateParameterModelReturnModel,
        JobTemplateParameterModelReturnModel>($request);
  }

  @override
  Future<Response<JobTemplateParameterModelReturnModel>>
      _apiV1SystemUpdatejobtemplateparameterPost(
          {required JobTemplateParameterUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/updatejobtemplateparameter');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<JobTemplateParameterModelReturnModel,
        JobTemplateParameterModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemMarkjobtemplateparameterasdeletedDelete({int? id}) {
    final Uri $url =
        Uri.parse('/api/v1/system/markjobtemplateparameterasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemMarkjobtemplateparameterasnotdeletedPost(
          {required int? body}) {
    final Uri $url =
        Uri.parse('/api/v1/system/markjobtemplateparameterasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<MessageToSendModelReturnListModel>>
      _apiV1SystemGetmessagetosendsGet({
    DateTime? startDate,
    DateTime? endDate,
    bool? isSent,
    bool? showDeleted,
    List<int>? predefinedMessageToSendTypeIds,
    int? realtimeMessageId,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/system/getmessagetosends');
    final Map<String, dynamic> $params = <String, dynamic>{
      'StartDate': startDate,
      'EndDate': endDate,
      'IsSent': isSent,
      'ShowDeleted': showDeleted,
      'PredefinedMessageToSendTypeIds': predefinedMessageToSendTypeIds,
      'RealtimeMessageId': realtimeMessageId,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<MessageToSendModelReturnListModel,
        MessageToSendModelReturnListModel>($request);
  }

  @override
  Future<Response<MessageToSendModelReturnModel>>
      _apiV1SystemGetmessagetosendGet({int? id}) {
    final Uri $url = Uri.parse('/api/v1/system/getmessagetosend');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<MessageToSendModelReturnModel,
        MessageToSendModelReturnModel>($request);
  }

  @override
  Future<Response<MessageToSendModelReturnModel>>
      _apiV1SystemCreatemessagetosendPost(
          {required MessageToSendCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/createmessagetosend');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<MessageToSendModelReturnModel,
        MessageToSendModelReturnModel>($request);
  }

  @override
  Future<Response<RealtimeMessageModelReturnListModel>>
      _apiV1SystemGetrealtimemessagesGet({
    DateTime? startDate,
    DateTime? endDate,
    bool? showDeleted,
    List<int>? predefinedMessagePriorityTypeIds,
    List<int>? predefinedRealtimeMessageStatusTypeIds,
    List<int>? predefinedNotificationMessageTypeIds,
    List<int>? predefinedResourceTypeIds,
    int? resourceId,
    int? userId,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/system/getrealtimemessages');
    final Map<String, dynamic> $params = <String, dynamic>{
      'StartDate': startDate,
      'EndDate': endDate,
      'ShowDeleted': showDeleted,
      'PredefinedMessagePriorityTypeIds': predefinedMessagePriorityTypeIds,
      'PredefinedRealtimeMessageStatusTypeIds':
          predefinedRealtimeMessageStatusTypeIds,
      'PredefinedNotificationMessageTypeIds':
          predefinedNotificationMessageTypeIds,
      'PredefinedResourceTypeIds': predefinedResourceTypeIds,
      'ResourceId': resourceId,
      'UserId': userId,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<RealtimeMessageModelReturnListModel,
        RealtimeMessageModelReturnListModel>($request);
  }

  @override
  Future<Response<RealtimeMessageModelReturnModel>>
      _apiV1SystemGetrealtimemessageGet({int? id}) {
    final Uri $url = Uri.parse('/api/v1/system/getrealtimemessage');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<RealtimeMessageModelReturnModel,
        RealtimeMessageModelReturnModel>($request);
  }

  @override
  Future<Response<RealtimeMessageModelReturnModel>>
      _apiV1SystemCreaterealtimemessagePost(
          {required RealtimeMessageCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/createrealtimemessage');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<RealtimeMessageModelReturnModel,
        RealtimeMessageModelReturnModel>($request);
  }

  @override
  Future<Response<RealtimeMessageModelReturnModel>>
      _apiV1SystemUpdaterealtimemessagePost(
          {required RealtimeMessageUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/updaterealtimemessage');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<RealtimeMessageModelReturnModel,
        RealtimeMessageModelReturnModel>($request);
  }

  @override
  Future<Response<SavedJobModelReturnListModel>> _apiV1SystemGetsavedjobsGet({
    bool? showDeleted,
    List<int>? predefinedJobTypeIds,
    List<int>? predefinedJobOrReportTypeIds,
    int? jobTemplateId,
    int? submittedByUserId,
    int? userId,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/system/getsavedjobs');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'PredefinedJobTypeIds': predefinedJobTypeIds,
      'PredefinedJobOrReportTypeIds': predefinedJobOrReportTypeIds,
      'JobTemplateId': jobTemplateId,
      'SubmittedByUserId': submittedByUserId,
      'UserId': userId,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<SavedJobModelReturnListModel,
        SavedJobModelReturnListModel>($request);
  }

  @override
  Future<Response<SavedJobModelReturnModel>> _apiV1SystemGetsavedjobGet(
      {int? id}) {
    final Uri $url = Uri.parse('/api/v1/system/getsavedjob');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client
        .send<SavedJobModelReturnModel, SavedJobModelReturnModel>($request);
  }

  @override
  Future<Response<SavedJobModelReturnModel>> _apiV1SystemCreatesavedjobPost(
      {required SavedJobCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/createsavedjob');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client
        .send<SavedJobModelReturnModel, SavedJobModelReturnModel>($request);
  }

  @override
  Future<Response<SavedJobModelReturnModel>> _apiV1SystemUpdatesavedjobPost(
      {required SavedJobUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/updatesavedjob');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client
        .send<SavedJobModelReturnModel, SavedJobModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>> _apiV1SystemMarksavedjobasdeletedDelete(
      {int? id}) {
    final Uri $url = Uri.parse('/api/v1/system/marksavedjobasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>> _apiV1SystemMarksavedjobasnotdeletedPost(
      {required int? body}) {
    final Uri $url = Uri.parse('/api/v1/system/marksavedjobasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<ScheduledJobModelReturnListModel>>
      _apiV1SystemGetscheduledjobsGet({
    bool? showDeleted,
    int? jobTemplateId,
    List<int>? predefinedJobOrReportTypeIds,
    int? submittedByUserId,
    int? runAsUserId,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/system/getscheduledjobs');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'JobTemplateId': jobTemplateId,
      'PredefinedJobOrReportTypeIds': predefinedJobOrReportTypeIds,
      'SubmittedByUserId': submittedByUserId,
      'RunAsUserId': runAsUserId,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<ScheduledJobModelReturnListModel,
        ScheduledJobModelReturnListModel>($request);
  }

  @override
  Future<Response<ScheduledJobModelReturnModel>> _apiV1SystemGetscheduledjobGet(
      {int? id}) {
    final Uri $url = Uri.parse('/api/v1/system/getscheduledjob');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<ScheduledJobModelReturnModel,
        ScheduledJobModelReturnModel>($request);
  }

  @override
  Future<Response<ScheduledJobModelReturnModel>>
      _apiV1SystemCreatescheduledjobPost(
          {required ScheduledJobCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/createscheduledjob');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<ScheduledJobModelReturnModel,
        ScheduledJobModelReturnModel>($request);
  }

  @override
  Future<Response<ScheduledJobModelReturnModel>>
      _apiV1SystemUpdatescheduledjobPost(
          {required ScheduledJobUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/updatescheduledjob');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<ScheduledJobModelReturnModel,
        ScheduledJobModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemMarkscheduledjobasdeletedDelete({int? id}) {
    final Uri $url = Uri.parse('/api/v1/system/markscheduledjobasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemMarkscheduledjobasnotdeletedPost({required int? body}) {
    final Uri $url = Uri.parse('/api/v1/system/markscheduledjobasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<AuditOperationModelReturnListModel>>
      _apiV1SystemGetauditoperationsGet({
    DateTime? fromDate,
    DateTime? toDate,
    bool? showDeleted,
    List<int>? predefinedAuditOperationTypeIds,
    int? userId,
    int? movieId,
    List<int>? predefinedResourceTypeIds,
    List<int>? userIds,
    String? ipAddress,
    int? resourceId,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/system/getauditoperations');
    final Map<String, dynamic> $params = <String, dynamic>{
      'FromDate': fromDate,
      'ToDate': toDate,
      'ShowDeleted': showDeleted,
      'PredefinedAuditOperationTypeIds': predefinedAuditOperationTypeIds,
      'UserId': userId,
      'MovieId': movieId,
      'PredefinedResourceTypeIds': predefinedResourceTypeIds,
      'UserIds': userIds,
      'IpAddress': ipAddress,
      'ResourceId': resourceId,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<AuditOperationModelReturnListModel,
        AuditOperationModelReturnListModel>($request);
  }

  @override
  Future<Response<AuditOperationModelReturnModel>>
      _apiV1SystemGetauditoperationGet({int? id}) {
    final Uri $url = Uri.parse('/api/v1/system/getauditoperation');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<AuditOperationModelReturnModel,
        AuditOperationModelReturnModel>($request);
  }

  @override
  Future<Response<AuditOperationModelReturnModel>>
      _apiV1SystemCreateauditoperationPost(
          {required AuditOperationCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/createauditoperation');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<AuditOperationModelReturnModel,
        AuditOperationModelReturnModel>($request);
  }

  @override
  Future<Response<AuditOperationModelReturnModel>>
      _apiV1SystemUpdateauditoperationPost(
          {required AuditOperationUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/updateauditoperation');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<AuditOperationModelReturnModel,
        AuditOperationModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemMarkauditoperationasdeletedDelete({int? id}) {
    final Uri $url = Uri.parse('/api/v1/system/markauditoperationasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemMarkauditoperationasnotdeletedPost({required int? body}) {
    final Uri $url = Uri.parse('/api/v1/system/markauditoperationasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<AuditOperationDetailModelReturnModel>>
      _apiV1SystemGetauditoperationdetailbyauditoperationidGet(
          {int? auditOperationId}) {
    final Uri $url =
        Uri.parse('/api/v1/system/getauditoperationdetailbyauditoperationid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'auditOperationId': auditOperationId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<AuditOperationDetailModelReturnModel,
        AuditOperationDetailModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemIsauditoperationdetailavailablebyauditoperationidGet(
          {int? auditOperationId}) {
    final Uri $url = Uri.parse(
        '/api/v1/system/isauditoperationdetailavailablebyauditoperationid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'auditOperationId': auditOperationId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<AuditOperationDetailModelReturnListModel>>
      _apiV1SystemGetauditoperationdetailsGet({
    bool? showDeleted,
    int? auditOperationId,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/system/getauditoperationdetails');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'AuditOperationId': auditOperationId,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<AuditOperationDetailModelReturnListModel,
        AuditOperationDetailModelReturnListModel>($request);
  }

  @override
  Future<Response<AuditOperationDetailModelReturnModel>>
      _apiV1SystemGetauditoperationdetailGet({int? id}) {
    final Uri $url = Uri.parse('/api/v1/system/getauditoperationdetail');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<AuditOperationDetailModelReturnModel,
        AuditOperationDetailModelReturnModel>($request);
  }

  @override
  Future<Response<AuditOperationDetailModelReturnModel>>
      _apiV1SystemCreateauditoperationdetailPost(
          {required AuditOperationDetailCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/createauditoperationdetail');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<AuditOperationDetailModelReturnModel,
        AuditOperationDetailModelReturnModel>($request);
  }

  @override
  Future<Response<AuditOperationDetailModelReturnModel>>
      _apiV1SystemUpdateauditoperationdetailPost(
          {required AuditOperationDetailUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/updateauditoperationdetail');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<AuditOperationDetailModelReturnModel,
        AuditOperationDetailModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemMarkauditoperationdetailasdeletedDelete({int? id}) {
    final Uri $url =
        Uri.parse('/api/v1/system/markauditoperationdetailasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemMarkauditoperationdetailasnotdeletedPost(
          {required int? body}) {
    final Uri $url =
        Uri.parse('/api/v1/system/markauditoperationdetailasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<SavedJobNotificationEmailModelReturnListModel>>
      _apiV1SystemGetsavedjobnotificationemailsGet({
    bool? showDeleted,
    int? savedJobId,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/system/getsavedjobnotificationemails');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'SavedJobId': savedJobId,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<SavedJobNotificationEmailModelReturnListModel,
        SavedJobNotificationEmailModelReturnListModel>($request);
  }

  @override
  Future<Response<SavedJobNotificationEmailModelReturnModel>>
      _apiV1SystemGetsavedjobnotificationemailGet({int? id}) {
    final Uri $url = Uri.parse('/api/v1/system/getsavedjobnotificationemail');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<SavedJobNotificationEmailModelReturnModel,
        SavedJobNotificationEmailModelReturnModel>($request);
  }

  @override
  Future<Response<SavedJobNotificationEmailModelReturnModel>>
      _apiV1SystemCreatesavedjobnotificationemailPost(
          {required SavedJobNotificationEmailCreateModel? body}) {
    final Uri $url =
        Uri.parse('/api/v1/system/createsavedjobnotificationemail');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<SavedJobNotificationEmailModelReturnModel,
        SavedJobNotificationEmailModelReturnModel>($request);
  }

  @override
  Future<Response<SavedJobNotificationEmailModelReturnModel>>
      _apiV1SystemUpdatesavedjobnotificationemailPost(
          {required SavedJobNotificationEmailUpdateModel? body}) {
    final Uri $url =
        Uri.parse('/api/v1/system/updatesavedjobnotificationemail');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<SavedJobNotificationEmailModelReturnModel,
        SavedJobNotificationEmailModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemMarksavedjobnotificationemailasdeletedDelete({int? id}) {
    final Uri $url =
        Uri.parse('/api/v1/system/marksavedjobnotificationemailasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemMarksavedjobnotificationemailasnotdeletedPost(
          {required int? body}) {
    final Uri $url =
        Uri.parse('/api/v1/system/marksavedjobnotificationemailasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<ScheduledJobHistoryModelReturnListModel>>
      _apiV1SystemGetscheduledjobhistorysGet({
    bool? showDeleted,
    int? scheduledJobId,
    int? pickupProcessId,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/system/getscheduledjobhistorys');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'ScheduledJobId': scheduledJobId,
      'PickupProcessId': pickupProcessId,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<ScheduledJobHistoryModelReturnListModel,
        ScheduledJobHistoryModelReturnListModel>($request);
  }

  @override
  Future<Response<ScheduledJobHistoryModelReturnModel>>
      _apiV1SystemGetscheduledjobhistoryGet({int? id}) {
    final Uri $url = Uri.parse('/api/v1/system/getscheduledjobhistory');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<ScheduledJobHistoryModelReturnModel,
        ScheduledJobHistoryModelReturnModel>($request);
  }

  @override
  Future<Response<ScheduledJobHistoryModelReturnModel>>
      _apiV1SystemCreatescheduledjobhistoryPost(
          {required ScheduledJobHistoryCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/createscheduledjobhistory');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<ScheduledJobHistoryModelReturnModel,
        ScheduledJobHistoryModelReturnModel>($request);
  }

  @override
  Future<Response<ScheduledJobHistoryModelReturnModel>>
      _apiV1SystemUpdatescheduledjobhistoryPost(
          {required ScheduledJobHistoryUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/updatescheduledjobhistory');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<ScheduledJobHistoryModelReturnModel,
        ScheduledJobHistoryModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemMarkscheduledjobhistoryasdeletedDelete({int? id}) {
    final Uri $url =
        Uri.parse('/api/v1/system/markscheduledjobhistoryasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemMarkscheduledjobhistoryasnotdeletedPost(
          {required int? body}) {
    final Uri $url =
        Uri.parse('/api/v1/system/markscheduledjobhistoryasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<ScheduledJobNotificationEmailModelReturnListModel>>
      _apiV1SystemGetscheduledjobnotificationemailsGet({
    bool? showDeleted,
    int? scheduledJobId,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url =
        Uri.parse('/api/v1/system/getscheduledjobnotificationemails');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'ScheduledJobId': scheduledJobId,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<ScheduledJobNotificationEmailModelReturnListModel,
        ScheduledJobNotificationEmailModelReturnListModel>($request);
  }

  @override
  Future<Response<ScheduledJobNotificationEmailModelReturnModel>>
      _apiV1SystemGetscheduledjobnotificationemailGet({int? id}) {
    final Uri $url =
        Uri.parse('/api/v1/system/getscheduledjobnotificationemail');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<ScheduledJobNotificationEmailModelReturnModel,
        ScheduledJobNotificationEmailModelReturnModel>($request);
  }

  @override
  Future<Response<ScheduledJobNotificationEmailModelReturnModel>>
      _apiV1SystemCreatescheduledjobnotificationemailPost(
          {required ScheduledJobNotificationEmailCreateModel? body}) {
    final Uri $url =
        Uri.parse('/api/v1/system/createscheduledjobnotificationemail');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<ScheduledJobNotificationEmailModelReturnModel,
        ScheduledJobNotificationEmailModelReturnModel>($request);
  }

  @override
  Future<Response<ScheduledJobNotificationEmailModelReturnModel>>
      _apiV1SystemUpdatescheduledjobnotificationemailPost(
          {required ScheduledJobNotificationEmailUpdateModel? body}) {
    final Uri $url =
        Uri.parse('/api/v1/system/updatescheduledjobnotificationemail');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<ScheduledJobNotificationEmailModelReturnModel,
        ScheduledJobNotificationEmailModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemMarkscheduledjobnotificationemailasdeletedDelete({int? id}) {
    final Uri $url =
        Uri.parse('/api/v1/system/markscheduledjobnotificationemailasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemMarkscheduledjobnotificationemailasnotdeletedPost(
          {required int? body}) {
    final Uri $url = Uri.parse(
        '/api/v1/system/markscheduledjobnotificationemailasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<CompanyStorageConfigurationModelReturnModel>>
      _apiV1SystemGetcompanystorageconfigurationGet({int? id}) {
    final Uri $url = Uri.parse('/api/v1/system/getcompanystorageconfiguration');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<CompanyStorageConfigurationModelReturnModel,
        CompanyStorageConfigurationModelReturnModel>($request);
  }

  @override
  Future<Response<CompanyStorageConfigurationModelReturnModel>>
      _apiV1SystemGetcompanystorageconfigurationbycompanyidGet(
          {int? companyId}) {
    final Uri $url =
        Uri.parse('/api/v1/system/getcompanystorageconfigurationbycompanyid');
    final Map<String, dynamic> $params = <String, dynamic>{
      'companyId': companyId
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<CompanyStorageConfigurationModelReturnModel,
        CompanyStorageConfigurationModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemDoescompanystorageconfigurationbycompanyidexistsGet() {
    final Uri $url = Uri.parse(
        '/api/v1/system/doescompanystorageconfigurationbycompanyidexists');
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<CompanyStorageConfigurationModelReturnModel>>
      _apiV1SystemCreatecompanystorageconfigurationPost(
          {required CompanyStorageConfigurationCreateModel? body}) {
    final Uri $url =
        Uri.parse('/api/v1/system/createcompanystorageconfiguration');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<CompanyStorageConfigurationModelReturnModel,
        CompanyStorageConfigurationModelReturnModel>($request);
  }

  @override
  Future<Response<CompanyStorageConfigurationModelReturnModel>>
      _apiV1SystemUpdatecompanystorageconfigurationPost(
          {required CompanyStorageConfigurationUpdateModel? body}) {
    final Uri $url =
        Uri.parse('/api/v1/system/updatecompanystorageconfiguration');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<CompanyStorageConfigurationModelReturnModel,
        CompanyStorageConfigurationModelReturnModel>($request);
  }

  @override
  Future<Response<MailServerModelReturnListModel>>
      _apiV1SystemGetmailserversGet({
    bool? showDeleted,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/system/getmailservers');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<MailServerModelReturnListModel,
        MailServerModelReturnListModel>($request);
  }

  @override
  Future<Response<MailServerModelReturnModel>> _apiV1SystemGetmailserverGet(
      {int? id}) {
    final Uri $url = Uri.parse('/api/v1/system/getmailserver');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client
        .send<MailServerModelReturnModel, MailServerModelReturnModel>($request);
  }

  @override
  Future<Response<MailServerModelReturnModel>> _apiV1SystemCreatemailserverPost(
      {required MailServerCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/createmailserver');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client
        .send<MailServerModelReturnModel, MailServerModelReturnModel>($request);
  }

  @override
  Future<Response<MailServerModelReturnModel>> _apiV1SystemUpdatemailserverPost(
      {required MailServerUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/updatemailserver');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client
        .send<MailServerModelReturnModel, MailServerModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemMarkmailserverasdeletedDelete({int? id}) {
    final Uri $url = Uri.parse('/api/v1/system/markmailserverasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemMarkmailserverasnotdeletedPost({required int? body}) {
    final Uri $url = Uri.parse('/api/v1/system/markmailserverasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<SmsServerModelReturnListModel>> _apiV1SystemGetsmsserversGet({
    List<int>? predefinedSmsAndVoiceServerTypeIds,
    bool? showDeleted,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/system/getsmsservers');
    final Map<String, dynamic> $params = <String, dynamic>{
      'PredefinedSmsAndVoiceServerTypeIds': predefinedSmsAndVoiceServerTypeIds,
      'ShowDeleted': showDeleted,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<SmsServerModelReturnListModel,
        SmsServerModelReturnListModel>($request);
  }

  @override
  Future<Response<SmsServerModelReturnModel>> _apiV1SystemGetsmsserverGet(
      {int? id}) {
    final Uri $url = Uri.parse('/api/v1/system/getsmsserver');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client
        .send<SmsServerModelReturnModel, SmsServerModelReturnModel>($request);
  }

  @override
  Future<Response<SmsServerModelReturnModel>> _apiV1SystemCreatesmsserverPost(
      {required SmsServerCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/createsmsserver');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client
        .send<SmsServerModelReturnModel, SmsServerModelReturnModel>($request);
  }

  @override
  Future<Response<SmsServerModelReturnModel>> _apiV1SystemUpdatesmsserverPost(
      {required SmsServerUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/updatesmsserver');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client
        .send<SmsServerModelReturnModel, SmsServerModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>> _apiV1SystemMarksmsserverasdeletedDelete(
      {int? id}) {
    final Uri $url = Uri.parse('/api/v1/system/marksmsserverasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemMarksmsserverasnotdeletedPost({required int? body}) {
    final Uri $url = Uri.parse('/api/v1/system/marksmsserverasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<SingleSignOnServerModelReturnListModel>>
      _apiV1SystemGetsinglesignonserversGet({
    bool? showDeleted,
    List<int>? predefinedSingleSignOnServerTypeIds,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/system/getsinglesignonservers');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'PredefinedSingleSignOnServerTypeIds':
          predefinedSingleSignOnServerTypeIds,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<SingleSignOnServerModelReturnListModel,
        SingleSignOnServerModelReturnListModel>($request);
  }

  @override
  Future<Response<SingleSignOnServerModelReturnModel>>
      _apiV1SystemGetsinglesignonserverGet({int? id}) {
    final Uri $url = Uri.parse('/api/v1/system/getsinglesignonserver');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<SingleSignOnServerModelReturnModel,
        SingleSignOnServerModelReturnModel>($request);
  }

  @override
  Future<Response<SingleSignOnServerModelReturnModel>>
      _apiV1SystemCreatesinglesignonserverPost(
          {required SingleSignOnServerCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/createsinglesignonserver');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<SingleSignOnServerModelReturnModel,
        SingleSignOnServerModelReturnModel>($request);
  }

  @override
  Future<Response<SingleSignOnServerModelReturnModel>>
      _apiV1SystemUpdatesinglesignonserverPost(
          {required SingleSignOnServerUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/updatesinglesignonserver');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<SingleSignOnServerModelReturnModel,
        SingleSignOnServerModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemMarksinglesignonserverasdeletedDelete({int? id}) {
    final Uri $url =
        Uri.parse('/api/v1/system/marksinglesignonserverasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemMarksinglesignonserverasnotdeletedPost({required int? body}) {
    final Uri $url =
        Uri.parse('/api/v1/system/marksinglesignonserverasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<PushNotificationServerModelReturnListModel>>
      _apiV1SystemGetpushnotificationserversGet({
    bool? showDeleted,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/system/getpushnotificationservers');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<PushNotificationServerModelReturnListModel,
        PushNotificationServerModelReturnListModel>($request);
  }

  @override
  Future<Response<PushNotificationServerModelReturnModel>>
      _apiV1SystemGetpushnotificationserverGet({int? id}) {
    final Uri $url = Uri.parse('/api/v1/system/getpushnotificationserver');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<PushNotificationServerModelReturnModel,
        PushNotificationServerModelReturnModel>($request);
  }

  @override
  Future<Response<PushNotificationServerModelReturnModel>>
      _apiV1SystemCreatepushnotificationserverPost(
          {required PushNotificationServerCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/createpushnotificationserver');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<PushNotificationServerModelReturnModel,
        PushNotificationServerModelReturnModel>($request);
  }

  @override
  Future<Response<PushNotificationServerModelReturnListModel>>
      _apiV1SystemCreatemultiplepushnotificationserverPost(
          {required MultiplePushNotificationServerCreateModel? body}) {
    final Uri $url =
        Uri.parse('/api/v1/system/createmultiplepushnotificationserver');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<PushNotificationServerModelReturnListModel,
        PushNotificationServerModelReturnListModel>($request);
  }

  @override
  Future<Response<PushNotificationServerModelReturnModel>>
      _apiV1SystemUpdatepushnotificationserverPost(
          {required PushNotificationServerUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/updatepushnotificationserver');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<PushNotificationServerModelReturnModel,
        PushNotificationServerModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemMarkpushnotificationserverasdeletedDelete({int? id}) {
    final Uri $url =
        Uri.parse('/api/v1/system/markpushnotificationserverasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemMarkpushnotificationserverasnotdeletedPost(
          {required int? body}) {
    final Uri $url =
        Uri.parse('/api/v1/system/markpushnotificationserverasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<VoiceNotificationServerModelReturnListModel>>
      _apiV1SystemGetvoicenotificationserversGet({
    bool? showDeleted,
    List<int>? predefinedSmsAndVoiceServerTypeIds,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/system/getvoicenotificationservers');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'PredefinedSmsAndVoiceServerTypeIds': predefinedSmsAndVoiceServerTypeIds,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<VoiceNotificationServerModelReturnListModel,
        VoiceNotificationServerModelReturnListModel>($request);
  }

  @override
  Future<Response<VoiceNotificationServerModelReturnModel>>
      _apiV1SystemGetvoicenotificationserverGet({int? id}) {
    final Uri $url = Uri.parse('/api/v1/system/getvoicenotificationserver');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<VoiceNotificationServerModelReturnModel,
        VoiceNotificationServerModelReturnModel>($request);
  }

  @override
  Future<Response<VoiceNotificationServerModelReturnModel>>
      _apiV1SystemCreatevoicenotificationserverPost(
          {required VoiceNotificationServerCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/createvoicenotificationserver');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<VoiceNotificationServerModelReturnModel,
        VoiceNotificationServerModelReturnModel>($request);
  }

  @override
  Future<Response<VoiceNotificationServerModelReturnListModel>>
      _apiV1SystemCreatemultiplevoicenotificationserverPost(
          {required MultipleVoiceNotificationServerCreateModel? body}) {
    final Uri $url =
        Uri.parse('/api/v1/system/createmultiplevoicenotificationserver');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<VoiceNotificationServerModelReturnListModel,
        VoiceNotificationServerModelReturnListModel>($request);
  }

  @override
  Future<Response<VoiceNotificationServerModelReturnModel>>
      _apiV1SystemUpdatevoicenotificationserverPost(
          {required VoiceNotificationServerUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/updatevoicenotificationserver');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<VoiceNotificationServerModelReturnModel,
        VoiceNotificationServerModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemMarkvoicenotificationserverasdeletedDelete({int? id}) {
    final Uri $url =
        Uri.parse('/api/v1/system/markvoicenotificationserverasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemMarkvoicenotificationserverasnotdeletedPost(
          {required int? body}) {
    final Uri $url =
        Uri.parse('/api/v1/system/markvoicenotificationserverasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<WhatsAppNotificationServerModelReturnListModel>>
      _apiV1SystemGetwhatsappnotificationserversGet({
    bool? showDeleted,
    List<int>? predefinedSmsAndVoiceServerTypeIds,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/system/getwhatsappnotificationservers');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'PredefinedSmsAndVoiceServerTypeIds': predefinedSmsAndVoiceServerTypeIds,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<WhatsAppNotificationServerModelReturnListModel,
        WhatsAppNotificationServerModelReturnListModel>($request);
  }

  @override
  Future<Response<WhatsAppNotificationServerModelReturnModel>>
      _apiV1SystemGetwhatsappnotificationserverGet({int? id}) {
    final Uri $url = Uri.parse('/api/v1/system/getwhatsappnotificationserver');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<WhatsAppNotificationServerModelReturnModel,
        WhatsAppNotificationServerModelReturnModel>($request);
  }

  @override
  Future<Response<WhatsAppNotificationServerModelReturnModel>>
      _apiV1SystemCreatewhatsappnotificationserverPost(
          {required WhatsAppNotificationServerCreateModel? body}) {
    final Uri $url =
        Uri.parse('/api/v1/system/createwhatsappnotificationserver');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<WhatsAppNotificationServerModelReturnModel,
        WhatsAppNotificationServerModelReturnModel>($request);
  }

  @override
  Future<Response<WhatsAppNotificationServerModelReturnListModel>>
      _apiV1SystemCreatemultiplewhatsappnotificationserverPost(
          {required MultipleWhatsAppNotificationServerCreateModel? body}) {
    final Uri $url =
        Uri.parse('/api/v1/system/createmultiplewhatsappnotificationserver');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<WhatsAppNotificationServerModelReturnListModel,
        WhatsAppNotificationServerModelReturnListModel>($request);
  }

  @override
  Future<Response<WhatsAppNotificationServerModelReturnModel>>
      _apiV1SystemUpdatewhatsappnotificationserverPost(
          {required WhatsAppNotificationServerUpdateModel? body}) {
    final Uri $url =
        Uri.parse('/api/v1/system/updatewhatsappnotificationserver');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<WhatsAppNotificationServerModelReturnModel,
        WhatsAppNotificationServerModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemMarkwhatsappnotificationserverasdeletedDelete({int? id}) {
    final Uri $url =
        Uri.parse('/api/v1/system/markwhatsappnotificationserverasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemMarkwhatsappnotificationserverasnotdeletedPost(
          {required int? body}) {
    final Uri $url =
        Uri.parse('/api/v1/system/markwhatsappnotificationserverasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<MessageTemplateModelReturnListModel>>
      _apiV1SystemGetmessagetemplatesGet({
    bool? showDeleted,
    List<int>? predefinedMessageTemplateTypeIds,
    int? createdByUserId,
    int? modifiedByUserId,
    int? selectedId,
    int? page,
    int? pageSize,
    String? keyword,
    String? sortBy,
    bool? sortAscending,
    int? currentRows,
    int? totalRowCount,
    int? numericPageCount,
  }) {
    final Uri $url = Uri.parse('/api/v1/system/getmessagetemplates');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ShowDeleted': showDeleted,
      'PredefinedMessageTemplateTypeIds': predefinedMessageTemplateTypeIds,
      'CreatedByUserId': createdByUserId,
      'ModifiedByUserId': modifiedByUserId,
      'SelectedId': selectedId,
      'Page': page,
      'PageSize': pageSize,
      'Keyword': keyword,
      'SortBy': sortBy,
      'SortAscending': sortAscending,
      'CurrentRows': currentRows,
      'TotalRowCount': totalRowCount,
      'NumericPageCount': numericPageCount,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<MessageTemplateModelReturnListModel,
        MessageTemplateModelReturnListModel>($request);
  }

  @override
  Future<Response<MessageTemplateModelReturnModel>>
      _apiV1SystemGetmessagetemplateGet({int? id}) {
    final Uri $url = Uri.parse('/api/v1/system/getmessagetemplate');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<MessageTemplateModelReturnModel,
        MessageTemplateModelReturnModel>($request);
  }

  @override
  Future<Response<MessageTemplateModelReturnModel>>
      _apiV1SystemCreatemessagetemplatePost(
          {required MessageTemplateCreateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/createmessagetemplate');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<MessageTemplateModelReturnModel,
        MessageTemplateModelReturnModel>($request);
  }

  @override
  Future<Response<MessageTemplateModelReturnModel>>
      _apiV1SystemUpdatemessagetemplatePost(
          {required MessageTemplateUpdateModel? body}) {
    final Uri $url = Uri.parse('/api/v1/system/updatemessagetemplate');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<MessageTemplateModelReturnModel,
        MessageTemplateModelReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemMarkmessagetemplateasdeletedDelete({int? id}) {
    final Uri $url = Uri.parse('/api/v1/system/markmessagetemplateasdeleted');
    final Map<String, dynamic> $params = <String, dynamic>{'id': id};
    final Request $request = Request(
      'DELETE',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<BooleanReturnModel>>
      _apiV1SystemMarkmessagetemplateasnotdeletedPost({required int? body}) {
    final Uri $url =
        Uri.parse('/api/v1/system/markmessagetemplateasnotdeleted');
    final $body = body;
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      body: $body,
    );
    return client.send<BooleanReturnModel, BooleanReturnModel>($request);
  }

  @override
  Future<Response<SystemSummaryModelReturnModel>>
      _apiV1SystemGetappdetailsGet() {
    final Uri $url = Uri.parse('/api/v1/system/getappdetails');
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
    );
    return client.send<SystemSummaryModelReturnModel,
        SystemSummaryModelReturnModel>($request);
  }

  @override
  Future<Response<dynamic>> _apiV1TwilioInitiatecallGet({
    String? toPhoneNumber,
    String? twilioAccountSid,
    String? twilioAuthToken,
    String? twilioPhoneNumber,
  }) {
    final Uri $url = Uri.parse('/api/v1/twilio/initiatecall');
    final Map<String, dynamic> $params = <String, dynamic>{
      'ToPhoneNumber': toPhoneNumber,
      'TwilioAccountSid': twilioAccountSid,
      'TwilioAuthToken': twilioAuthToken,
      'TwilioPhoneNumber': twilioPhoneNumber,
    };
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<dynamic, dynamic>($request);
  }

  @override
  Future<Response<dynamic>> _apiV1TwilioHandlequestionGet() {
    final Uri $url = Uri.parse('/api/v1/twilio/handlequestion');
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
    );
    return client.send<dynamic, dynamic>($request);
  }

  @override
  Future<Response<dynamic>> _apiV1TwilioHandleresponseGet({String? digits}) {
    final Uri $url = Uri.parse('/api/v1/twilio/handleresponse');
    final Map<String, dynamic> $params = <String, dynamic>{'digits': digits};
    final Request $request = Request(
      'GET',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<dynamic, dynamic>($request);
  }

  @override
  Future<Response<dynamic>> _apiV1TwilioHandlegatherPost({
    int? msdrid,
    int? msdrtid,
    int? ntid,
    String? digits,
  }) {
    final Uri $url = Uri.parse('/api/v1/twilio/handlegather');
    final Map<String, dynamic> $params = <String, dynamic>{
      'msdrid': msdrid,
      'msdrtid': msdrtid,
      'ntid': ntid,
    };
    final List<PartValue> $parts = <PartValue>[
      PartValue<String?>(
        'digits',
        digits,
      )
    ];
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      parts: $parts,
      multipart: true,
      parameters: $params,
    );
    return client.send<dynamic, dynamic>($request);
  }

  @override
  Future<Response<dynamic>> _apiV1TwilioGeneratecalltwimlPost({
    int? msdrid,
    int? msdrtid,
    int? ntid,
    String? question,
  }) {
    final Uri $url = Uri.parse('/api/v1/twilio/generatecalltwiml');
    final Map<String, dynamic> $params = <String, dynamic>{
      'msdrid': msdrid,
      'msdrtid': msdrtid,
      'ntid': ntid,
      'question': question,
    };
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      parameters: $params,
    );
    return client.send<dynamic, dynamic>($request);
  }

  @override
  Future<Response<dynamic>> _apiV1TwilioHandlestatusPost({
    int? msdrid,
    int? msdrtid,
    int? ntid,
    String? callStatus,
    String? smsStatus,
  }) {
    final Uri $url = Uri.parse('/api/v1/twilio/handlestatus');
    final Map<String, dynamic> $params = <String, dynamic>{
      'msdrid': msdrid,
      'msdrtid': msdrtid,
      'ntid': ntid,
    };
    final List<PartValue> $parts = <PartValue>[
      PartValue<String?>(
        'callStatus',
        callStatus,
      ),
      PartValue<String?>(
        'smsStatus',
        smsStatus,
      ),
    ];
    final Request $request = Request(
      'POST',
      $url,
      client.baseUrl,
      parts: $parts,
      multipart: true,
      parameters: $params,
    );
    return client.send<dynamic, dynamic>($request);
  }
}
