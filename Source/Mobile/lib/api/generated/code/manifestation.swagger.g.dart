// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'manifestation.swagger.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

AccountCreateModel _$AccountCreateModelFromJson(Map<String, dynamic> json) =>
    AccountCreateModel(
      captchaCode: json['captchaCode'] as String?,
      initialPassword: json['initialPassword'] as String?,
      email: json['email'] as String?,
      firstName: json['firstName'] as String?,
      lastName: json['lastName'] as String?,
    );

Map<String, dynamic> _$AccountCreateModelToJson(AccountCreateModel instance) =>
    <String, dynamic>{
      'captchaCode': instance.captchaCode,
      'initialPassword': instance.initialPassword,
      'email': instance.email,
      'firstName': instance.firstName,
      'lastName': instance.lastName,
    };

AlternateLoginModel _$AlternateLoginModelFromJson(Map<String, dynamic> json) =>
    AlternateLoginModel(
      loginInAsUsername: json['loginInAsUsername'] as String?,
      username: json['username'] as String?,
      password: json['password'] as String?,
    );

Map<String, dynamic> _$AlternateLoginModelToJson(
  AlternateLoginModel instance,
) => <String, dynamic>{
  'loginInAsUsername': instance.loginInAsUsername,
  'username': instance.username,
  'password': instance.password,
};

AnonymousLoginModel _$AnonymousLoginModelFromJson(Map<String, dynamic> json) =>
    AnonymousLoginModel(
      authValue: json['authValue'] as String?,
      authCode: (json['authCode'] as num?)?.toInt(),
      isEmail: json['isEmail'] as bool?,
    );

Map<String, dynamic> _$AnonymousLoginModelToJson(
  AnonymousLoginModel instance,
) => <String, dynamic>{
  'authValue': instance.authValue,
  'authCode': instance.authCode,
  'isEmail': instance.isEmail,
};

AppConfigurationModel _$AppConfigurationModelFromJson(
  Map<String, dynamic> json,
) => AppConfigurationModel(
  enableNewFeatures: json['enableNewFeatures'] as bool?,
  logSettings: json['logSettings'] == null
      ? null
      : LogSettings.fromJson(json['logSettings'] as Map<String, dynamic>),
  cacheHighRefreshInMinutes: (json['cacheHighRefreshInMinutes'] as num?)
      ?.toInt(),
  cachMediumRefreshInMinutes: (json['cachMediumRefreshInMinutes'] as num?)
      ?.toInt(),
  cachLowRefreshInMinutes: (json['cachLowRefreshInMinutes'] as num?)?.toInt(),
  useMiniProfiler: json['useMiniProfiler'] as bool?,
  showAllLinksWithoutPermissionCheck:
      json['showAllLinksWithoutPermissionCheck'] as bool?,
);

Map<String, dynamic> _$AppConfigurationModelToJson(
  AppConfigurationModel instance,
) => <String, dynamic>{
  'enableNewFeatures': instance.enableNewFeatures,
  'logSettings': instance.logSettings?.toJson(),
  'cacheHighRefreshInMinutes': instance.cacheHighRefreshInMinutes,
  'cachMediumRefreshInMinutes': instance.cachMediumRefreshInMinutes,
  'cachLowRefreshInMinutes': instance.cachLowRefreshInMinutes,
  'useMiniProfiler': instance.useMiniProfiler,
  'showAllLinksWithoutPermissionCheck':
      instance.showAllLinksWithoutPermissionCheck,
};

AppConfigurationModelReturnModel _$AppConfigurationModelReturnModelFromJson(
  Map<String, dynamic> json,
) => AppConfigurationModelReturnModel(
  model: json['model'] == null
      ? null
      : AppConfigurationModel.fromJson(json['model'] as Map<String, dynamic>),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$AppConfigurationModelReturnModelToJson(
  AppConfigurationModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

AppLogModel _$AppLogModelFromJson(Map<String, dynamic> json) => AppLogModel(
  logDate: json['logDate'] == null
      ? null
      : DateTime.parse(json['logDate'] as String),
  activityId: json['activityId'] as String?,
  machineName: json['machineName'] as String?,
  userIdentity: json['userIdentity'] as String?,
  appName: json['appName'] as String?,
  severity: json['severity'] as String?,
  processId: json['processId'] as String?,
  threadId: json['threadId'] as String?,
  currentFunction: json['currentFunction'] as String?,
  currentSourceFileName: json['currentSourceFileName'] as String?,
  currentSourceLineNumber: json['currentSourceLineNumber'] as String?,
  elaspedTime: (json['elaspedTime'] as num?)?.toDouble(),
  result: json['result'] as String?,
  message: json['message'] as String?,
  ipAddress: json['ipAddress'] as String?,
);

Map<String, dynamic> _$AppLogModelToJson(AppLogModel instance) =>
    <String, dynamic>{
      'logDate': instance.logDate?.toIso8601String(),
      'activityId': instance.activityId,
      'machineName': instance.machineName,
      'userIdentity': instance.userIdentity,
      'appName': instance.appName,
      'severity': instance.severity,
      'processId': instance.processId,
      'threadId': instance.threadId,
      'currentFunction': instance.currentFunction,
      'currentSourceFileName': instance.currentSourceFileName,
      'currentSourceLineNumber': instance.currentSourceLineNumber,
      'elaspedTime': instance.elaspedTime,
      'result': instance.result,
      'message': instance.message,
      'ipAddress': instance.ipAddress,
    };

AppLogModelReturnListModel _$AppLogModelReturnListModelFromJson(
  Map<String, dynamic> json,
) => AppLogModelReturnListModel(
  model:
      (json['model'] as List<dynamic>?)
          ?.map((e) => AppLogModel.fromJson(e as Map<String, dynamic>))
          .toList() ??
      [],
  search: json['search'] == null
      ? null
      : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
  totalRecords: (json['totalRecords'] as num?)?.toInt(),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$AppLogModelReturnListModelToJson(
  AppLogModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

AuditOperationCreateModel _$AuditOperationCreateModelFromJson(
  Map<String, dynamic> json,
) => AuditOperationCreateModel(
  auditDateTime: json['auditDateTime'] == null
      ? null
      : DateTime.parse(json['auditDateTime'] as String),
  predefinedAuditOperationTypeId:
      (json['predefinedAuditOperationTypeId'] as num?)?.toInt(),
  changeDiff: json['changeDiff'] as String?,
  userId: (json['userId'] as num?)?.toInt(),
  userGroupId: (json['userGroupId'] as num?)?.toInt(),
  userGroupUserId: (json['userGroupUserId'] as num?)?.toInt(),
  userPermissionId: (json['userPermissionId'] as num?)?.toInt(),
  predefinedResourceTypeId: (json['predefinedResourceTypeId'] as num?)?.toInt(),
  resourceId: (json['resourceId'] as num?)?.toInt(),
  ipAddress: json['ipAddress'] as String?,
  browserName: json['browserName'] as String?,
  operatingSystemName: json['operatingSystemName'] as String?,
  deviceName: json['deviceName'] as String?,
  description: json['description'] as String?,
);

Map<String, dynamic> _$AuditOperationCreateModelToJson(
  AuditOperationCreateModel instance,
) => <String, dynamic>{
  'auditDateTime': instance.auditDateTime?.toIso8601String(),
  'predefinedAuditOperationTypeId': instance.predefinedAuditOperationTypeId,
  'changeDiff': instance.changeDiff,
  'userId': instance.userId,
  'userGroupId': instance.userGroupId,
  'userGroupUserId': instance.userGroupUserId,
  'userPermissionId': instance.userPermissionId,
  'predefinedResourceTypeId': instance.predefinedResourceTypeId,
  'resourceId': instance.resourceId,
  'ipAddress': instance.ipAddress,
  'browserName': instance.browserName,
  'operatingSystemName': instance.operatingSystemName,
  'deviceName': instance.deviceName,
  'description': instance.description,
};

AuditOperationDetailCreateModel _$AuditOperationDetailCreateModelFromJson(
  Map<String, dynamic> json,
) => AuditOperationDetailCreateModel(
  auditOperationId: (json['auditOperationId'] as num?)?.toInt(),
  differenceJson: json['differenceJson'] as String?,
);

Map<String, dynamic> _$AuditOperationDetailCreateModelToJson(
  AuditOperationDetailCreateModel instance,
) => <String, dynamic>{
  'auditOperationId': instance.auditOperationId,
  'differenceJson': instance.differenceJson,
};

AuditOperationDetailModel _$AuditOperationDetailModelFromJson(
  Map<String, dynamic> json,
) => AuditOperationDetailModel(
  auditOperationDetailId: (json['auditOperationDetailId'] as num?)?.toInt(),
  auditOperationId: (json['auditOperationId'] as num?)?.toInt(),
  differenceJson: json['differenceJson'] as String?,
  isDeleted: json['isDeleted'] as bool?,
  createdDate: json['createdDate'] == null
      ? null
      : DateTime.parse(json['createdDate'] as String),
  createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
  modifiedDate: json['modifiedDate'] == null
      ? null
      : DateTime.parse(json['modifiedDate'] as String),
  modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
  createdByUserFirstName: json['createdByUserFirstName'] as String?,
  createdByUserLastName: json['createdByUserLastName'] as String?,
  createdByUserEmail: json['createdByUserEmail'] as String?,
  modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
  modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
  modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
  createdBy: json['createdBy'] as String?,
  modifiedBy: json['modifiedBy'] as String?,
);

Map<String, dynamic> _$AuditOperationDetailModelToJson(
  AuditOperationDetailModel instance,
) => <String, dynamic>{
  'auditOperationDetailId': instance.auditOperationDetailId,
  'auditOperationId': instance.auditOperationId,
  'differenceJson': instance.differenceJson,
  'isDeleted': instance.isDeleted,
  'createdDate': instance.createdDate?.toIso8601String(),
  'createdByUserId': instance.createdByUserId,
  'modifiedDate': instance.modifiedDate?.toIso8601String(),
  'modifiedByUserId': instance.modifiedByUserId,
  'createdByUserFirstName': instance.createdByUserFirstName,
  'createdByUserLastName': instance.createdByUserLastName,
  'createdByUserEmail': instance.createdByUserEmail,
  'modifiedByUserFirstName': instance.modifiedByUserFirstName,
  'modifiedByUserLastName': instance.modifiedByUserLastName,
  'modifiedByUserEmail': instance.modifiedByUserEmail,
  'createdBy': instance.createdBy,
  'modifiedBy': instance.modifiedBy,
};

AuditOperationDetailModelReturnListModel
_$AuditOperationDetailModelReturnListModelFromJson(Map<String, dynamic> json) =>
    AuditOperationDetailModelReturnListModel(
      model:
          (json['model'] as List<dynamic>?)
              ?.map(
                (e) => AuditOperationDetailModel.fromJson(
                  e as Map<String, dynamic>,
                ),
              )
              .toList() ??
          [],
      search: json['search'] == null
          ? null
          : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
      totalRecords: (json['totalRecords'] as num?)?.toInt(),
      isSuccess: json['isSuccess'] as bool?,
      successMessage: json['successMessage'] as String?,
      errorHolder: json['errorHolder'] == null
          ? null
          : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
      activityId: json['activityId'] as String?,
      activityDateTime: json['activityDateTime'] == null
          ? null
          : DateTime.parse(json['activityDateTime'] as String),
      server: json['server'] as String?,
      processId: (json['processId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$AuditOperationDetailModelReturnListModelToJson(
  AuditOperationDetailModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

AuditOperationDetailModelReturnModel
_$AuditOperationDetailModelReturnModelFromJson(Map<String, dynamic> json) =>
    AuditOperationDetailModelReturnModel(
      model: json['model'] == null
          ? null
          : AuditOperationDetailModel.fromJson(
              json['model'] as Map<String, dynamic>,
            ),
      isSuccess: json['isSuccess'] as bool?,
      successMessage: json['successMessage'] as String?,
      errorHolder: json['errorHolder'] == null
          ? null
          : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
      activityId: json['activityId'] as String?,
      activityDateTime: json['activityDateTime'] == null
          ? null
          : DateTime.parse(json['activityDateTime'] as String),
      server: json['server'] as String?,
      processId: (json['processId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$AuditOperationDetailModelReturnModelToJson(
  AuditOperationDetailModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

AuditOperationDetailUpdateModel _$AuditOperationDetailUpdateModelFromJson(
  Map<String, dynamic> json,
) => AuditOperationDetailUpdateModel(
  auditOperationDetailId: (json['auditOperationDetailId'] as num?)?.toInt(),
  auditOperationId: (json['auditOperationId'] as num?)?.toInt(),
  differenceJson: json['differenceJson'] as String?,
);

Map<String, dynamic> _$AuditOperationDetailUpdateModelToJson(
  AuditOperationDetailUpdateModel instance,
) => <String, dynamic>{
  'auditOperationDetailId': instance.auditOperationDetailId,
  'auditOperationId': instance.auditOperationId,
  'differenceJson': instance.differenceJson,
};

AuditOperationModel _$AuditOperationModelFromJson(Map<String, dynamic> json) =>
    AuditOperationModel(
      auditOperationId: (json['auditOperationId'] as num?)?.toInt(),
      auditDateTime: json['auditDateTime'] == null
          ? null
          : DateTime.parse(json['auditDateTime'] as String),
      predefinedAuditOperationTypeId:
          (json['predefinedAuditOperationTypeId'] as num?)?.toInt(),
      changeDiff: json['changeDiff'] as String?,
      userId: (json['userId'] as num?)?.toInt(),
      userGroupId: (json['userGroupId'] as num?)?.toInt(),
      userGroupUserId: (json['userGroupUserId'] as num?)?.toInt(),
      userPermissionId: (json['userPermissionId'] as num?)?.toInt(),
      predefinedResourceTypeId: (json['predefinedResourceTypeId'] as num?)
          ?.toInt(),
      resourceId: (json['resourceId'] as num?)?.toInt(),
      ipAddress: json['ipAddress'] as String?,
      browserName: json['browserName'] as String?,
      operatingSystemName: json['operatingSystemName'] as String?,
      deviceName: json['deviceName'] as String?,
      description: json['description'] as String?,
      isDeleted: json['isDeleted'] as bool?,
      createdDate: json['createdDate'] == null
          ? null
          : DateTime.parse(json['createdDate'] as String),
      createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
      modifiedDate: json['modifiedDate'] == null
          ? null
          : DateTime.parse(json['modifiedDate'] as String),
      modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
      predefinedAuditOperationTypeName:
          json['predefinedAuditOperationTypeName'] as String?,
      userName: json['userName'] as String?,
      firstName: json['firstName'] as String?,
      lastName: json['lastName'] as String?,
      predefinedResourceTypeName: json['predefinedResourceTypeName'] as String?,
      resourceName: json['resourceName'] as String?,
      createdByUserFirstName: json['createdByUserFirstName'] as String?,
      createdByUserLastName: json['createdByUserLastName'] as String?,
      createdByUserEmail: json['createdByUserEmail'] as String?,
      modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
      modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
      modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
      createdBy: json['createdBy'] as String?,
      modifiedBy: json['modifiedBy'] as String?,
    );

Map<String, dynamic> _$AuditOperationModelToJson(
  AuditOperationModel instance,
) => <String, dynamic>{
  'auditOperationId': instance.auditOperationId,
  'auditDateTime': instance.auditDateTime?.toIso8601String(),
  'predefinedAuditOperationTypeId': instance.predefinedAuditOperationTypeId,
  'changeDiff': instance.changeDiff,
  'userId': instance.userId,
  'userGroupId': instance.userGroupId,
  'userGroupUserId': instance.userGroupUserId,
  'userPermissionId': instance.userPermissionId,
  'predefinedResourceTypeId': instance.predefinedResourceTypeId,
  'resourceId': instance.resourceId,
  'ipAddress': instance.ipAddress,
  'browserName': instance.browserName,
  'operatingSystemName': instance.operatingSystemName,
  'deviceName': instance.deviceName,
  'description': instance.description,
  'isDeleted': instance.isDeleted,
  'createdDate': instance.createdDate?.toIso8601String(),
  'createdByUserId': instance.createdByUserId,
  'modifiedDate': instance.modifiedDate?.toIso8601String(),
  'modifiedByUserId': instance.modifiedByUserId,
  'predefinedAuditOperationTypeName': instance.predefinedAuditOperationTypeName,
  'userName': instance.userName,
  'firstName': instance.firstName,
  'lastName': instance.lastName,
  'predefinedResourceTypeName': instance.predefinedResourceTypeName,
  'resourceName': instance.resourceName,
  'createdByUserFirstName': instance.createdByUserFirstName,
  'createdByUserLastName': instance.createdByUserLastName,
  'createdByUserEmail': instance.createdByUserEmail,
  'modifiedByUserFirstName': instance.modifiedByUserFirstName,
  'modifiedByUserLastName': instance.modifiedByUserLastName,
  'modifiedByUserEmail': instance.modifiedByUserEmail,
  'createdBy': instance.createdBy,
  'modifiedBy': instance.modifiedBy,
};

AuditOperationModelReturnListModel _$AuditOperationModelReturnListModelFromJson(
  Map<String, dynamic> json,
) => AuditOperationModelReturnListModel(
  model:
      (json['model'] as List<dynamic>?)
          ?.map((e) => AuditOperationModel.fromJson(e as Map<String, dynamic>))
          .toList() ??
      [],
  search: json['search'] == null
      ? null
      : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
  totalRecords: (json['totalRecords'] as num?)?.toInt(),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$AuditOperationModelReturnListModelToJson(
  AuditOperationModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

AuditOperationModelReturnModel _$AuditOperationModelReturnModelFromJson(
  Map<String, dynamic> json,
) => AuditOperationModelReturnModel(
  model: json['model'] == null
      ? null
      : AuditOperationModel.fromJson(json['model'] as Map<String, dynamic>),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$AuditOperationModelReturnModelToJson(
  AuditOperationModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

AuditOperationUpdateModel _$AuditOperationUpdateModelFromJson(
  Map<String, dynamic> json,
) => AuditOperationUpdateModel(
  auditOperationId: (json['auditOperationId'] as num?)?.toInt(),
  auditDateTime: json['auditDateTime'] == null
      ? null
      : DateTime.parse(json['auditDateTime'] as String),
  predefinedAuditOperationTypeId:
      (json['predefinedAuditOperationTypeId'] as num?)?.toInt(),
  changeDiff: json['changeDiff'] as String?,
  userId: (json['userId'] as num?)?.toInt(),
  userGroupId: (json['userGroupId'] as num?)?.toInt(),
  userGroupUserId: (json['userGroupUserId'] as num?)?.toInt(),
  userPermissionId: (json['userPermissionId'] as num?)?.toInt(),
  predefinedResourceTypeId: (json['predefinedResourceTypeId'] as num?)?.toInt(),
  resourceId: (json['resourceId'] as num?)?.toInt(),
  ipAddress: json['ipAddress'] as String?,
  browserName: json['browserName'] as String?,
  operatingSystemName: json['operatingSystemName'] as String?,
  deviceName: json['deviceName'] as String?,
  description: json['description'] as String?,
);

Map<String, dynamic> _$AuditOperationUpdateModelToJson(
  AuditOperationUpdateModel instance,
) => <String, dynamic>{
  'auditOperationId': instance.auditOperationId,
  'auditDateTime': instance.auditDateTime?.toIso8601String(),
  'predefinedAuditOperationTypeId': instance.predefinedAuditOperationTypeId,
  'changeDiff': instance.changeDiff,
  'userId': instance.userId,
  'userGroupId': instance.userGroupId,
  'userGroupUserId': instance.userGroupUserId,
  'userPermissionId': instance.userPermissionId,
  'predefinedResourceTypeId': instance.predefinedResourceTypeId,
  'resourceId': instance.resourceId,
  'ipAddress': instance.ipAddress,
  'browserName': instance.browserName,
  'operatingSystemName': instance.operatingSystemName,
  'deviceName': instance.deviceName,
  'description': instance.description,
};

BaseSearchCriteria _$BaseSearchCriteriaFromJson(Map<String, dynamic> json) =>
    BaseSearchCriteria(
      selectedId: (json['selectedId'] as num?)?.toInt(),
      page: (json['page'] as num?)?.toInt(),
      pageSize: (json['pageSize'] as num?)?.toInt(),
      keyword: json['keyword'] as String?,
      sortBy: json['sortBy'] as String?,
      sortAscending: json['sortAscending'] as bool?,
    );

Map<String, dynamic> _$BaseSearchCriteriaToJson(BaseSearchCriteria instance) =>
    <String, dynamic>{
      'selectedId': instance.selectedId,
      'page': instance.page,
      'pageSize': instance.pageSize,
      'keyword': instance.keyword,
      'sortBy': instance.sortBy,
      'sortAscending': instance.sortAscending,
    };

BooleanReturnModel _$BooleanReturnModelFromJson(Map<String, dynamic> json) =>
    BooleanReturnModel(
      model: json['model'] as bool?,
      isSuccess: json['isSuccess'] as bool?,
      successMessage: json['successMessage'] as String?,
      errorHolder: json['errorHolder'] == null
          ? null
          : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
      activityId: json['activityId'] as String?,
      activityDateTime: json['activityDateTime'] == null
          ? null
          : DateTime.parse(json['activityDateTime'] as String),
      server: json['server'] as String?,
      processId: (json['processId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$BooleanReturnModelToJson(BooleanReturnModel instance) =>
    <String, dynamic>{
      'model': instance.model,
      'isSuccess': instance.isSuccess,
      'successMessage': instance.successMessage,
      'errorHolder': instance.errorHolder?.toJson(),
      'activityId': instance.activityId,
      'activityDateTime': instance.activityDateTime?.toIso8601String(),
      'server': instance.server,
      'processId': instance.processId,
    };

CaptchaUrlResponseModel _$CaptchaUrlResponseModelFromJson(
  Map<String, dynamic> json,
) => CaptchaUrlResponseModel(
  captchaImageUrl: json['captchaImageUrl'] as String?,
  captchaSoundUrl: json['captchaSoundUrl'] as String?,
  reloadIconUrl: json['reloadIconUrl'] as String?,
  soundIconUrl: json['soundIconUrl'] as String?,
);

Map<String, dynamic> _$CaptchaUrlResponseModelToJson(
  CaptchaUrlResponseModel instance,
) => <String, dynamic>{
  'captchaImageUrl': instance.captchaImageUrl,
  'captchaSoundUrl': instance.captchaSoundUrl,
  'reloadIconUrl': instance.reloadIconUrl,
  'soundIconUrl': instance.soundIconUrl,
};

CaptchaUrlResponseModelReturnModel _$CaptchaUrlResponseModelReturnModelFromJson(
  Map<String, dynamic> json,
) => CaptchaUrlResponseModelReturnModel(
  model: json['model'] == null
      ? null
      : CaptchaUrlResponseModel.fromJson(json['model'] as Map<String, dynamic>),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$CaptchaUrlResponseModelReturnModelToJson(
  CaptchaUrlResponseModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

CategoryByDailyAffirmationSessionCategoryIdSearchCriteria
_$CategoryByDailyAffirmationSessionCategoryIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => CategoryByDailyAffirmationSessionCategoryIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  dailyAffirmationSessionCategoryId:
      (json['dailyAffirmationSessionCategoryId'] as num?)?.toInt(),
);

Map<String, dynamic>
_$CategoryByDailyAffirmationSessionCategoryIdSearchCriteriaToJson(
  CategoryByDailyAffirmationSessionCategoryIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'dailyAffirmationSessionCategoryId':
      instance.dailyAffirmationSessionCategoryId,
};

CategoryByDailyAffirmationTextIdSearchCriteria
_$CategoryByDailyAffirmationTextIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => CategoryByDailyAffirmationTextIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  dailyAffirmationTextId: (json['dailyAffirmationTextId'] as num?)?.toInt(),
);

Map<String, dynamic> _$CategoryByDailyAffirmationTextIdSearchCriteriaToJson(
  CategoryByDailyAffirmationTextIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'dailyAffirmationTextId': instance.dailyAffirmationTextId,
};

CategoryByDefaultCategoryIdSearchCriteria
_$CategoryByDefaultCategoryIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => CategoryByDefaultCategoryIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  defaultCategoryId: (json['defaultCategoryId'] as num?)?.toInt(),
);

Map<String, dynamic> _$CategoryByDefaultCategoryIdSearchCriteriaToJson(
  CategoryByDefaultCategoryIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'defaultCategoryId': instance.defaultCategoryId,
};

CategoryByGoalTrackerCategoryIdSearchCriteria
_$CategoryByGoalTrackerCategoryIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => CategoryByGoalTrackerCategoryIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  goalTrackerCategoryId: (json['goalTrackerCategoryId'] as num?)?.toInt(),
);

Map<String, dynamic> _$CategoryByGoalTrackerCategoryIdSearchCriteriaToJson(
  CategoryByGoalTrackerCategoryIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'goalTrackerCategoryId': instance.goalTrackerCategoryId,
};

CategoryByUserIdSearchCriteria _$CategoryByUserIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => CategoryByUserIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  userId: (json['userId'] as num?)?.toInt(),
);

Map<String, dynamic> _$CategoryByUserIdSearchCriteriaToJson(
  CategoryByUserIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'userId': instance.userId,
};

CategoryCreateModel _$CategoryCreateModelFromJson(Map<String, dynamic> json) =>
    CategoryCreateModel(
      defaultCategoryId: (json['defaultCategoryId'] as num?)?.toInt(),
      categoryName: json['categoryName'] as String?,
      predefinedCategoryTypeId: (json['predefinedCategoryTypeId'] as num?)
          ?.toInt(),
      userId: (json['userId'] as num?)?.toInt(),
      colorAsInteger: (json['colorAsInteger'] as num?)?.toInt(),
      predefinedIconTypeId: (json['predefinedIconTypeId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$CategoryCreateModelToJson(
  CategoryCreateModel instance,
) => <String, dynamic>{
  'defaultCategoryId': instance.defaultCategoryId,
  'categoryName': instance.categoryName,
  'predefinedCategoryTypeId': instance.predefinedCategoryTypeId,
  'userId': instance.userId,
  'colorAsInteger': instance.colorAsInteger,
  'predefinedIconTypeId': instance.predefinedIconTypeId,
};

CategoryModel _$CategoryModelFromJson(Map<String, dynamic> json) =>
    CategoryModel(
      categoryId: (json['categoryId'] as num?)?.toInt(),
      defaultCategoryId: (json['defaultCategoryId'] as num?)?.toInt(),
      categoryName: json['categoryName'] as String?,
      predefinedCategoryTypeId: (json['predefinedCategoryTypeId'] as num?)
          ?.toInt(),
      userId: (json['userId'] as num?)?.toInt(),
      colorAsInteger: (json['colorAsInteger'] as num?)?.toInt(),
      predefinedIconTypeId: (json['predefinedIconTypeId'] as num?)?.toInt(),
      isDeleted: json['isDeleted'] as bool?,
      createdDate: json['createdDate'] == null
          ? null
          : DateTime.parse(json['createdDate'] as String),
      createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
      modifiedDate: json['modifiedDate'] == null
          ? null
          : DateTime.parse(json['modifiedDate'] as String),
      modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
      defaultCategoryName: json['defaultCategoryName'] as String?,
      predefinedCategoryTypeName: json['predefinedCategoryTypeName'] as String?,
      userName: json['userName'] as String?,
      createdByUserFirstName: json['createdByUserFirstName'] as String?,
      createdByUserLastName: json['createdByUserLastName'] as String?,
      createdByUserEmail: json['createdByUserEmail'] as String?,
      modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
      modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
      modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
      createdBy: json['createdBy'] as String?,
      modifiedBy: json['modifiedBy'] as String?,
    );

Map<String, dynamic> _$CategoryModelToJson(CategoryModel instance) =>
    <String, dynamic>{
      'categoryId': instance.categoryId,
      'defaultCategoryId': instance.defaultCategoryId,
      'categoryName': instance.categoryName,
      'predefinedCategoryTypeId': instance.predefinedCategoryTypeId,
      'userId': instance.userId,
      'colorAsInteger': instance.colorAsInteger,
      'predefinedIconTypeId': instance.predefinedIconTypeId,
      'isDeleted': instance.isDeleted,
      'createdDate': instance.createdDate?.toIso8601String(),
      'createdByUserId': instance.createdByUserId,
      'modifiedDate': instance.modifiedDate?.toIso8601String(),
      'modifiedByUserId': instance.modifiedByUserId,
      'defaultCategoryName': instance.defaultCategoryName,
      'predefinedCategoryTypeName': instance.predefinedCategoryTypeName,
      'userName': instance.userName,
      'createdByUserFirstName': instance.createdByUserFirstName,
      'createdByUserLastName': instance.createdByUserLastName,
      'createdByUserEmail': instance.createdByUserEmail,
      'modifiedByUserFirstName': instance.modifiedByUserFirstName,
      'modifiedByUserLastName': instance.modifiedByUserLastName,
      'modifiedByUserEmail': instance.modifiedByUserEmail,
      'createdBy': instance.createdBy,
      'modifiedBy': instance.modifiedBy,
    };

CategoryModelReturnListModel _$CategoryModelReturnListModelFromJson(
  Map<String, dynamic> json,
) => CategoryModelReturnListModel(
  model:
      (json['model'] as List<dynamic>?)
          ?.map((e) => CategoryModel.fromJson(e as Map<String, dynamic>))
          .toList() ??
      [],
  search: json['search'] == null
      ? null
      : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
  totalRecords: (json['totalRecords'] as num?)?.toInt(),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$CategoryModelReturnListModelToJson(
  CategoryModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

CategoryModelReturnModel _$CategoryModelReturnModelFromJson(
  Map<String, dynamic> json,
) => CategoryModelReturnModel(
  model: json['model'] == null
      ? null
      : CategoryModel.fromJson(json['model'] as Map<String, dynamic>),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$CategoryModelReturnModelToJson(
  CategoryModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

CategoryUpdateModel _$CategoryUpdateModelFromJson(Map<String, dynamic> json) =>
    CategoryUpdateModel(
      categoryId: (json['categoryId'] as num?)?.toInt(),
      defaultCategoryId: (json['defaultCategoryId'] as num?)?.toInt(),
      categoryName: json['categoryName'] as String?,
      predefinedCategoryTypeId: (json['predefinedCategoryTypeId'] as num?)
          ?.toInt(),
      userId: (json['userId'] as num?)?.toInt(),
      colorAsInteger: (json['colorAsInteger'] as num?)?.toInt(),
      predefinedIconTypeId: (json['predefinedIconTypeId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$CategoryUpdateModelToJson(
  CategoryUpdateModel instance,
) => <String, dynamic>{
  'categoryId': instance.categoryId,
  'defaultCategoryId': instance.defaultCategoryId,
  'categoryName': instance.categoryName,
  'predefinedCategoryTypeId': instance.predefinedCategoryTypeId,
  'userId': instance.userId,
  'colorAsInteger': instance.colorAsInteger,
  'predefinedIconTypeId': instance.predefinedIconTypeId,
};

ChangeForgotPasswordModel _$ChangeForgotPasswordModelFromJson(
  Map<String, dynamic> json,
) => ChangeForgotPasswordModel(
  currentPassword: json['currentPassword'] as String?,
  password: json['password'] as String?,
  retypedPassword: json['retypedPassword'] as String?,
  userLogin: json['userLogin'] as String?,
  sessionId: json['sessionId'] as String?,
);

Map<String, dynamic> _$ChangeForgotPasswordModelToJson(
  ChangeForgotPasswordModel instance,
) => <String, dynamic>{
  'currentPassword': instance.currentPassword,
  'password': instance.password,
  'retypedPassword': instance.retypedPassword,
  'userLogin': instance.userLogin,
  'sessionId': instance.sessionId,
};

ChangePasswordModel _$ChangePasswordModelFromJson(Map<String, dynamic> json) =>
    ChangePasswordModel(
      currentPassword: json['currentPassword'] as String?,
      password: json['password'] as String?,
      retypedPassword: json['retypedPassword'] as String?,
    );

Map<String, dynamic> _$ChangePasswordModelToJson(
  ChangePasswordModel instance,
) => <String, dynamic>{
  'currentPassword': instance.currentPassword,
  'password': instance.password,
  'retypedPassword': instance.retypedPassword,
};

ChatCreateModel _$ChatCreateModelFromJson(
  Map<String, dynamic> json,
) => ChatCreateModel(
  chatName: json['chatName'] as String?,
  userId: (json['userId'] as num?)?.toInt(),
  newChatUsers:
      (json['newChatUsers'] as List<dynamic>?)
          ?.map((e) => ChatUserCreateModel.fromJson(e as Map<String, dynamic>))
          .toList() ??
      [],
  existingChatUsers:
      (json['existingChatUsers'] as List<dynamic>?)
          ?.map((e) => ChatUserUpdateModel.fromJson(e as Map<String, dynamic>))
          .toList() ??
      [],
);

Map<String, dynamic> _$ChatCreateModelToJson(ChatCreateModel instance) =>
    <String, dynamic>{
      'chatName': instance.chatName,
      'userId': instance.userId,
      'newChatUsers': instance.newChatUsers?.map((e) => e.toJson()).toList(),
      'existingChatUsers': instance.existingChatUsers
          ?.map((e) => e.toJson())
          .toList(),
    };

ChatMessageCreateModel _$ChatMessageCreateModelFromJson(
  Map<String, dynamic> json,
) => ChatMessageCreateModel(
  chatId: (json['chatId'] as num?)?.toInt(),
  userId: (json['userId'] as num?)?.toInt(),
  chatMessageText: json['chatMessageText'] as String?,
);

Map<String, dynamic> _$ChatMessageCreateModelToJson(
  ChatMessageCreateModel instance,
) => <String, dynamic>{
  'chatId': instance.chatId,
  'userId': instance.userId,
  'chatMessageText': instance.chatMessageText,
};

ChatMessageModel _$ChatMessageModelFromJson(Map<String, dynamic> json) =>
    ChatMessageModel(
      chatMessageId: (json['chatMessageId'] as num?)?.toInt(),
      chatId: (json['chatId'] as num?)?.toInt(),
      userId: (json['userId'] as num?)?.toInt(),
      chatMessageText: json['chatMessageText'] as String?,
      isDeleted: json['isDeleted'] as bool?,
      createdDate: json['createdDate'] == null
          ? null
          : DateTime.parse(json['createdDate'] as String),
      createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
      modifiedDate: json['modifiedDate'] == null
          ? null
          : DateTime.parse(json['modifiedDate'] as String),
      modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
      userName: json['userName'] as String?,
      createdByUserFirstName: json['createdByUserFirstName'] as String?,
      createdByUserLastName: json['createdByUserLastName'] as String?,
      createdByUserEmail: json['createdByUserEmail'] as String?,
      modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
      modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
      modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
      createdBy: json['createdBy'] as String?,
      modifiedBy: json['modifiedBy'] as String?,
    );

Map<String, dynamic> _$ChatMessageModelToJson(ChatMessageModel instance) =>
    <String, dynamic>{
      'chatMessageId': instance.chatMessageId,
      'chatId': instance.chatId,
      'userId': instance.userId,
      'chatMessageText': instance.chatMessageText,
      'isDeleted': instance.isDeleted,
      'createdDate': instance.createdDate?.toIso8601String(),
      'createdByUserId': instance.createdByUserId,
      'modifiedDate': instance.modifiedDate?.toIso8601String(),
      'modifiedByUserId': instance.modifiedByUserId,
      'userName': instance.userName,
      'createdByUserFirstName': instance.createdByUserFirstName,
      'createdByUserLastName': instance.createdByUserLastName,
      'createdByUserEmail': instance.createdByUserEmail,
      'modifiedByUserFirstName': instance.modifiedByUserFirstName,
      'modifiedByUserLastName': instance.modifiedByUserLastName,
      'modifiedByUserEmail': instance.modifiedByUserEmail,
      'createdBy': instance.createdBy,
      'modifiedBy': instance.modifiedBy,
    };

ChatMessageModelReturnListModel _$ChatMessageModelReturnListModelFromJson(
  Map<String, dynamic> json,
) => ChatMessageModelReturnListModel(
  model:
      (json['model'] as List<dynamic>?)
          ?.map((e) => ChatMessageModel.fromJson(e as Map<String, dynamic>))
          .toList() ??
      [],
  search: json['search'] == null
      ? null
      : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
  totalRecords: (json['totalRecords'] as num?)?.toInt(),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$ChatMessageModelReturnListModelToJson(
  ChatMessageModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

ChatMessageModelReturnModel _$ChatMessageModelReturnModelFromJson(
  Map<String, dynamic> json,
) => ChatMessageModelReturnModel(
  model: json['model'] == null
      ? null
      : ChatMessageModel.fromJson(json['model'] as Map<String, dynamic>),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$ChatMessageModelReturnModelToJson(
  ChatMessageModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

ChatMessageStatusCreateModel _$ChatMessageStatusCreateModelFromJson(
  Map<String, dynamic> json,
) => ChatMessageStatusCreateModel(
  chatMessageId: (json['chatMessageId'] as num?)?.toInt(),
  userId: (json['userId'] as num?)?.toInt(),
  chatMessageViewedDate: json['chatMessageViewedDate'] == null
      ? null
      : DateTime.parse(json['chatMessageViewedDate'] as String),
  chatMessageSentDate: json['chatMessageSentDate'] == null
      ? null
      : DateTime.parse(json['chatMessageSentDate'] as String),
);

Map<String, dynamic> _$ChatMessageStatusCreateModelToJson(
  ChatMessageStatusCreateModel instance,
) => <String, dynamic>{
  'chatMessageId': instance.chatMessageId,
  'userId': instance.userId,
  'chatMessageViewedDate': instance.chatMessageViewedDate?.toIso8601String(),
  'chatMessageSentDate': instance.chatMessageSentDate?.toIso8601String(),
};

ChatMessageStatusModel _$ChatMessageStatusModelFromJson(
  Map<String, dynamic> json,
) => ChatMessageStatusModel(
  chatMessageStatusId: (json['chatMessageStatusId'] as num?)?.toInt(),
  chatMessageId: (json['chatMessageId'] as num?)?.toInt(),
  userId: (json['userId'] as num?)?.toInt(),
  chatMessageViewedDate: json['chatMessageViewedDate'] == null
      ? null
      : DateTime.parse(json['chatMessageViewedDate'] as String),
  chatMessageSentDate: json['chatMessageSentDate'] == null
      ? null
      : DateTime.parse(json['chatMessageSentDate'] as String),
  isDeleted: json['isDeleted'] as bool?,
  createdDate: json['createdDate'] == null
      ? null
      : DateTime.parse(json['createdDate'] as String),
  createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
  modifiedDate: json['modifiedDate'] == null
      ? null
      : DateTime.parse(json['modifiedDate'] as String),
  modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
  userName: json['userName'] as String?,
  userFirstName: json['userFirstName'] as String?,
  userLastName: json['userLastName'] as String?,
  createdByUserFirstName: json['createdByUserFirstName'] as String?,
  createdByUserLastName: json['createdByUserLastName'] as String?,
  createdByUserEmail: json['createdByUserEmail'] as String?,
  modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
  modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
  modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
  createdBy: json['createdBy'] as String?,
  modifiedBy: json['modifiedBy'] as String?,
);

Map<String, dynamic> _$ChatMessageStatusModelToJson(
  ChatMessageStatusModel instance,
) => <String, dynamic>{
  'chatMessageStatusId': instance.chatMessageStatusId,
  'chatMessageId': instance.chatMessageId,
  'userId': instance.userId,
  'chatMessageViewedDate': instance.chatMessageViewedDate?.toIso8601String(),
  'chatMessageSentDate': instance.chatMessageSentDate?.toIso8601String(),
  'isDeleted': instance.isDeleted,
  'createdDate': instance.createdDate?.toIso8601String(),
  'createdByUserId': instance.createdByUserId,
  'modifiedDate': instance.modifiedDate?.toIso8601String(),
  'modifiedByUserId': instance.modifiedByUserId,
  'userName': instance.userName,
  'userFirstName': instance.userFirstName,
  'userLastName': instance.userLastName,
  'createdByUserFirstName': instance.createdByUserFirstName,
  'createdByUserLastName': instance.createdByUserLastName,
  'createdByUserEmail': instance.createdByUserEmail,
  'modifiedByUserFirstName': instance.modifiedByUserFirstName,
  'modifiedByUserLastName': instance.modifiedByUserLastName,
  'modifiedByUserEmail': instance.modifiedByUserEmail,
  'createdBy': instance.createdBy,
  'modifiedBy': instance.modifiedBy,
};

ChatMessageStatusModelReturnListModel
_$ChatMessageStatusModelReturnListModelFromJson(Map<String, dynamic> json) =>
    ChatMessageStatusModelReturnListModel(
      model:
          (json['model'] as List<dynamic>?)
              ?.map(
                (e) =>
                    ChatMessageStatusModel.fromJson(e as Map<String, dynamic>),
              )
              .toList() ??
          [],
      search: json['search'] == null
          ? null
          : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
      totalRecords: (json['totalRecords'] as num?)?.toInt(),
      isSuccess: json['isSuccess'] as bool?,
      successMessage: json['successMessage'] as String?,
      errorHolder: json['errorHolder'] == null
          ? null
          : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
      activityId: json['activityId'] as String?,
      activityDateTime: json['activityDateTime'] == null
          ? null
          : DateTime.parse(json['activityDateTime'] as String),
      server: json['server'] as String?,
      processId: (json['processId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$ChatMessageStatusModelReturnListModelToJson(
  ChatMessageStatusModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

ChatMessageStatusModelReturnModel _$ChatMessageStatusModelReturnModelFromJson(
  Map<String, dynamic> json,
) => ChatMessageStatusModelReturnModel(
  model: json['model'] == null
      ? null
      : ChatMessageStatusModel.fromJson(json['model'] as Map<String, dynamic>),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$ChatMessageStatusModelReturnModelToJson(
  ChatMessageStatusModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

ChatMessageStatusUpdateModel _$ChatMessageStatusUpdateModelFromJson(
  Map<String, dynamic> json,
) => ChatMessageStatusUpdateModel(
  chatMessageStatusId: (json['chatMessageStatusId'] as num?)?.toInt(),
  chatMessageId: (json['chatMessageId'] as num?)?.toInt(),
  userId: (json['userId'] as num?)?.toInt(),
  chatMessageViewedDate: json['chatMessageViewedDate'] == null
      ? null
      : DateTime.parse(json['chatMessageViewedDate'] as String),
  chatMessageSentDate: json['chatMessageSentDate'] == null
      ? null
      : DateTime.parse(json['chatMessageSentDate'] as String),
);

Map<String, dynamic> _$ChatMessageStatusUpdateModelToJson(
  ChatMessageStatusUpdateModel instance,
) => <String, dynamic>{
  'chatMessageStatusId': instance.chatMessageStatusId,
  'chatMessageId': instance.chatMessageId,
  'userId': instance.userId,
  'chatMessageViewedDate': instance.chatMessageViewedDate?.toIso8601String(),
  'chatMessageSentDate': instance.chatMessageSentDate?.toIso8601String(),
};

ChatMessageUpdateModel _$ChatMessageUpdateModelFromJson(
  Map<String, dynamic> json,
) => ChatMessageUpdateModel(
  chatMessageId: (json['chatMessageId'] as num?)?.toInt(),
  chatId: (json['chatId'] as num?)?.toInt(),
  userId: (json['userId'] as num?)?.toInt(),
  chatMessageText: json['chatMessageText'] as String?,
);

Map<String, dynamic> _$ChatMessageUpdateModelToJson(
  ChatMessageUpdateModel instance,
) => <String, dynamic>{
  'chatMessageId': instance.chatMessageId,
  'chatId': instance.chatId,
  'userId': instance.userId,
  'chatMessageText': instance.chatMessageText,
};

ChatModel _$ChatModelFromJson(Map<String, dynamic> json) => ChatModel(
  chatId: (json['chatId'] as num?)?.toInt(),
  chatName: json['chatName'] as String?,
  userId: (json['userId'] as num?)?.toInt(),
  isDeleted: json['isDeleted'] as bool?,
  createdDate: json['createdDate'] == null
      ? null
      : DateTime.parse(json['createdDate'] as String),
  createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
  modifiedDate: json['modifiedDate'] == null
      ? null
      : DateTime.parse(json['modifiedDate'] as String),
  modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
  createdByUserFirstName: json['createdByUserFirstName'] as String?,
  createdByUserLastName: json['createdByUserLastName'] as String?,
  createdByUserEmail: json['createdByUserEmail'] as String?,
  modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
  modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
  modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
  createdBy: json['createdBy'] as String?,
  modifiedBy: json['modifiedBy'] as String?,
);

Map<String, dynamic> _$ChatModelToJson(ChatModel instance) => <String, dynamic>{
  'chatId': instance.chatId,
  'chatName': instance.chatName,
  'userId': instance.userId,
  'isDeleted': instance.isDeleted,
  'createdDate': instance.createdDate?.toIso8601String(),
  'createdByUserId': instance.createdByUserId,
  'modifiedDate': instance.modifiedDate?.toIso8601String(),
  'modifiedByUserId': instance.modifiedByUserId,
  'createdByUserFirstName': instance.createdByUserFirstName,
  'createdByUserLastName': instance.createdByUserLastName,
  'createdByUserEmail': instance.createdByUserEmail,
  'modifiedByUserFirstName': instance.modifiedByUserFirstName,
  'modifiedByUserLastName': instance.modifiedByUserLastName,
  'modifiedByUserEmail': instance.modifiedByUserEmail,
  'createdBy': instance.createdBy,
  'modifiedBy': instance.modifiedBy,
};

ChatModelReturnListModel _$ChatModelReturnListModelFromJson(
  Map<String, dynamic> json,
) => ChatModelReturnListModel(
  model:
      (json['model'] as List<dynamic>?)
          ?.map((e) => ChatModel.fromJson(e as Map<String, dynamic>))
          .toList() ??
      [],
  search: json['search'] == null
      ? null
      : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
  totalRecords: (json['totalRecords'] as num?)?.toInt(),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$ChatModelReturnListModelToJson(
  ChatModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

ChatModelReturnModel _$ChatModelReturnModelFromJson(
  Map<String, dynamic> json,
) => ChatModelReturnModel(
  model: json['model'] == null
      ? null
      : ChatModel.fromJson(json['model'] as Map<String, dynamic>),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$ChatModelReturnModelToJson(
  ChatModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

ChatUpdateModel _$ChatUpdateModelFromJson(
  Map<String, dynamic> json,
) => ChatUpdateModel(
  chatId: (json['chatId'] as num?)?.toInt(),
  chatName: json['chatName'] as String?,
  userId: (json['userId'] as num?)?.toInt(),
  newChatUsers:
      (json['newChatUsers'] as List<dynamic>?)
          ?.map((e) => ChatUserCreateModel.fromJson(e as Map<String, dynamic>))
          .toList() ??
      [],
  existingChatUsers:
      (json['existingChatUsers'] as List<dynamic>?)
          ?.map((e) => ChatUserUpdateModel.fromJson(e as Map<String, dynamic>))
          .toList() ??
      [],
);

Map<String, dynamic> _$ChatUpdateModelToJson(ChatUpdateModel instance) =>
    <String, dynamic>{
      'chatId': instance.chatId,
      'chatName': instance.chatName,
      'userId': instance.userId,
      'newChatUsers': instance.newChatUsers?.map((e) => e.toJson()).toList(),
      'existingChatUsers': instance.existingChatUsers
          ?.map((e) => e.toJson())
          .toList(),
    };

ChatUserCreateModel _$ChatUserCreateModelFromJson(Map<String, dynamic> json) =>
    ChatUserCreateModel(
      chatId: (json['chatId'] as num?)?.toInt(),
      userId: (json['userId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$ChatUserCreateModelToJson(
  ChatUserCreateModel instance,
) => <String, dynamic>{'chatId': instance.chatId, 'userId': instance.userId};

ChatUserModel _$ChatUserModelFromJson(Map<String, dynamic> json) =>
    ChatUserModel(
      chatUserId: (json['chatUserId'] as num?)?.toInt(),
      chatId: (json['chatId'] as num?)?.toInt(),
      userId: (json['userId'] as num?)?.toInt(),
      isDeleted: json['isDeleted'] as bool?,
      createdDate: json['createdDate'] == null
          ? null
          : DateTime.parse(json['createdDate'] as String),
      createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
      modifiedDate: json['modifiedDate'] == null
          ? null
          : DateTime.parse(json['modifiedDate'] as String),
      modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
      userName: json['userName'] as String?,
      predefinedUserTypeId: (json['predefinedUserTypeId'] as num?)?.toInt(),
      predefinedUserTypeName: json['predefinedUserTypeName'] as String?,
      userFirstName: json['userFirstName'] as String?,
      userLastName: json['userLastName'] as String?,
      createdByUserFirstName: json['createdByUserFirstName'] as String?,
      createdByUserLastName: json['createdByUserLastName'] as String?,
      createdByUserEmail: json['createdByUserEmail'] as String?,
      modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
      modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
      modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
      createdBy: json['createdBy'] as String?,
      modifiedBy: json['modifiedBy'] as String?,
    );

Map<String, dynamic> _$ChatUserModelToJson(ChatUserModel instance) =>
    <String, dynamic>{
      'chatUserId': instance.chatUserId,
      'chatId': instance.chatId,
      'userId': instance.userId,
      'isDeleted': instance.isDeleted,
      'createdDate': instance.createdDate?.toIso8601String(),
      'createdByUserId': instance.createdByUserId,
      'modifiedDate': instance.modifiedDate?.toIso8601String(),
      'modifiedByUserId': instance.modifiedByUserId,
      'userName': instance.userName,
      'predefinedUserTypeId': instance.predefinedUserTypeId,
      'predefinedUserTypeName': instance.predefinedUserTypeName,
      'userFirstName': instance.userFirstName,
      'userLastName': instance.userLastName,
      'createdByUserFirstName': instance.createdByUserFirstName,
      'createdByUserLastName': instance.createdByUserLastName,
      'createdByUserEmail': instance.createdByUserEmail,
      'modifiedByUserFirstName': instance.modifiedByUserFirstName,
      'modifiedByUserLastName': instance.modifiedByUserLastName,
      'modifiedByUserEmail': instance.modifiedByUserEmail,
      'createdBy': instance.createdBy,
      'modifiedBy': instance.modifiedBy,
    };

ChatUserModelReturnListModel _$ChatUserModelReturnListModelFromJson(
  Map<String, dynamic> json,
) => ChatUserModelReturnListModel(
  model:
      (json['model'] as List<dynamic>?)
          ?.map((e) => ChatUserModel.fromJson(e as Map<String, dynamic>))
          .toList() ??
      [],
  search: json['search'] == null
      ? null
      : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
  totalRecords: (json['totalRecords'] as num?)?.toInt(),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$ChatUserModelReturnListModelToJson(
  ChatUserModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

ChatUserModelReturnModel _$ChatUserModelReturnModelFromJson(
  Map<String, dynamic> json,
) => ChatUserModelReturnModel(
  model: json['model'] == null
      ? null
      : ChatUserModel.fromJson(json['model'] as Map<String, dynamic>),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$ChatUserModelReturnModelToJson(
  ChatUserModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

ChatUserUpdateModel _$ChatUserUpdateModelFromJson(Map<String, dynamic> json) =>
    ChatUserUpdateModel(
      chatUserId: (json['chatUserId'] as num?)?.toInt(),
      chatId: (json['chatId'] as num?)?.toInt(),
      userId: (json['userId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$ChatUserUpdateModelToJson(
  ChatUserUpdateModel instance,
) => <String, dynamic>{
  'chatUserId': instance.chatUserId,
  'chatId': instance.chatId,
  'userId': instance.userId,
};

CommonConfigurationModel _$CommonConfigurationModelFromJson(
  Map<String, dynamic> json,
) => CommonConfigurationModel(
  privacyPolicyUrl: json['privacyPolicyUrl'] as String?,
  termsOfUseUrl: json['termsOfUseUrl'] as String?,
  helpUrl: json['helpUrl'] as String?,
  supportEmail: json['supportEmail'] as String?,
  copyrightInfo: json['copyrightInfo'] as String?,
  jwtSecretKey: json['jwtSecretKey'] as String?,
  useLocalNppesServer: json['useLocalNppesServer'] as bool?,
  localLogServerUrl: json['localLogServerUrl'] as String?,
  forceSSL: json['forceSSL'] as bool?,
  emailServer: json['emailServer'] as String?,
  emailServerPort: (json['emailServerPort'] as num?)?.toInt(),
  emailServerUserName: json['emailServerUserName'] as String?,
  emailServerPassword: json['emailServerPassword'] as String?,
  emailUseSSL: json['emailUseSSL'] as bool?,
  emailFromName: json['emailFromName'] as String?,
  emailFromEmail: json['emailFromEmail'] as String?,
  adminNameForEmail: json['adminNameForEmail'] as String?,
  adminEmailForEmail: json['adminEmailForEmail'] as String?,
  defaultCommonEmailDomain: json['defaultCommonEmailDomain'] as String?,
  maxZipFileSizeinMB: (json['maxZipFileSizeinMB'] as num?)?.toInt(),
  nativeTempFileLocation: json['nativeTempFileLocation'] as String?,
  zipFileDownloadFileStorageConnectionString:
      json['zipFileDownloadFileStorageConnectionString'] as String?,
  isDedicatedQueue: json['isDedicatedQueue'] as bool?,
  activeMQHost: json['activeMQHost'] as String?,
  activeMQUsername: json['activeMQUsername'] as String?,
  activeMQPassword: json['activeMQPassword'] as String?,
  apiServerUrl: json['apiServerUrl'] as String?,
  whatsAppGraphAPIServerUrl: json['whatsAppGraphAPIServerUrl'] as String?,
  webApplicationServerUrl: json['webApplicationServerUrl'] as String?,
);

Map<String, dynamic> _$CommonConfigurationModelToJson(
  CommonConfigurationModel instance,
) => <String, dynamic>{
  'privacyPolicyUrl': instance.privacyPolicyUrl,
  'termsOfUseUrl': instance.termsOfUseUrl,
  'helpUrl': instance.helpUrl,
  'supportEmail': instance.supportEmail,
  'copyrightInfo': instance.copyrightInfo,
  'jwtSecretKey': instance.jwtSecretKey,
  'useLocalNppesServer': instance.useLocalNppesServer,
  'localLogServerUrl': instance.localLogServerUrl,
  'forceSSL': instance.forceSSL,
  'emailServer': instance.emailServer,
  'emailServerPort': instance.emailServerPort,
  'emailServerUserName': instance.emailServerUserName,
  'emailServerPassword': instance.emailServerPassword,
  'emailUseSSL': instance.emailUseSSL,
  'emailFromName': instance.emailFromName,
  'emailFromEmail': instance.emailFromEmail,
  'adminNameForEmail': instance.adminNameForEmail,
  'adminEmailForEmail': instance.adminEmailForEmail,
  'defaultCommonEmailDomain': instance.defaultCommonEmailDomain,
  'maxZipFileSizeinMB': instance.maxZipFileSizeinMB,
  'nativeTempFileLocation': instance.nativeTempFileLocation,
  'zipFileDownloadFileStorageConnectionString':
      instance.zipFileDownloadFileStorageConnectionString,
  'isDedicatedQueue': instance.isDedicatedQueue,
  'activeMQHost': instance.activeMQHost,
  'activeMQUsername': instance.activeMQUsername,
  'activeMQPassword': instance.activeMQPassword,
  'apiServerUrl': instance.apiServerUrl,
  'whatsAppGraphAPIServerUrl': instance.whatsAppGraphAPIServerUrl,
  'webApplicationServerUrl': instance.webApplicationServerUrl,
};

CommonConfigurationModelReturnModel
_$CommonConfigurationModelReturnModelFromJson(Map<String, dynamic> json) =>
    CommonConfigurationModelReturnModel(
      model: json['model'] == null
          ? null
          : CommonConfigurationModel.fromJson(
              json['model'] as Map<String, dynamic>,
            ),
      isSuccess: json['isSuccess'] as bool?,
      successMessage: json['successMessage'] as String?,
      errorHolder: json['errorHolder'] == null
          ? null
          : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
      activityId: json['activityId'] as String?,
      activityDateTime: json['activityDateTime'] == null
          ? null
          : DateTime.parse(json['activityDateTime'] as String),
      server: json['server'] as String?,
      processId: (json['processId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$CommonConfigurationModelReturnModelToJson(
  CommonConfigurationModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

CompanyStorageConfigurationCreateModel
_$CompanyStorageConfigurationCreateModelFromJson(Map<String, dynamic> json) =>
    CompanyStorageConfigurationCreateModel(
      companyStorageConfigurationGuid:
          json['companyStorageConfigurationGuid'] as String?,
      profilePictureFileStorageConnectionString:
          json['profilePictureFileStorageConnectionString'] as String?,
      reportServerFileStorageConnectionString:
          json['reportServerFileStorageConnectionString'] as String?,
      fileAttachmentFileStorageConnectionString:
          json['fileAttachmentFileStorageConnectionString'] as String?,
    );

Map<String, dynamic> _$CompanyStorageConfigurationCreateModelToJson(
  CompanyStorageConfigurationCreateModel instance,
) => <String, dynamic>{
  'companyStorageConfigurationGuid': instance.companyStorageConfigurationGuid,
  'profilePictureFileStorageConnectionString':
      instance.profilePictureFileStorageConnectionString,
  'reportServerFileStorageConnectionString':
      instance.reportServerFileStorageConnectionString,
  'fileAttachmentFileStorageConnectionString':
      instance.fileAttachmentFileStorageConnectionString,
};

CompanyStorageConfigurationModel _$CompanyStorageConfigurationModelFromJson(
  Map<String, dynamic> json,
) => CompanyStorageConfigurationModel(
  companyStorageConfigurationId: (json['companyStorageConfigurationId'] as num?)
      ?.toInt(),
  companyStorageConfigurationGuid:
      json['companyStorageConfigurationGuid'] as String?,
  profilePictureFileStorageConnectionString:
      json['profilePictureFileStorageConnectionString'] as String?,
  reportServerFileStorageConnectionString:
      json['reportServerFileStorageConnectionString'] as String?,
  fileAttachmentFileStorageConnectionString:
      json['fileAttachmentFileStorageConnectionString'] as String?,
  isDeleted: json['isDeleted'] as bool?,
  createdDate: json['createdDate'] == null
      ? null
      : DateTime.parse(json['createdDate'] as String),
  createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
  modifiedDate: json['modifiedDate'] == null
      ? null
      : DateTime.parse(json['modifiedDate'] as String),
  modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
  createdByUserFirstName: json['createdByUserFirstName'] as String?,
  createdByUserLastName: json['createdByUserLastName'] as String?,
  createdByUserEmail: json['createdByUserEmail'] as String?,
  modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
  modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
  modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
  createdBy: json['createdBy'] as String?,
  modifiedBy: json['modifiedBy'] as String?,
);

Map<String, dynamic> _$CompanyStorageConfigurationModelToJson(
  CompanyStorageConfigurationModel instance,
) => <String, dynamic>{
  'companyStorageConfigurationId': instance.companyStorageConfigurationId,
  'companyStorageConfigurationGuid': instance.companyStorageConfigurationGuid,
  'profilePictureFileStorageConnectionString':
      instance.profilePictureFileStorageConnectionString,
  'reportServerFileStorageConnectionString':
      instance.reportServerFileStorageConnectionString,
  'fileAttachmentFileStorageConnectionString':
      instance.fileAttachmentFileStorageConnectionString,
  'isDeleted': instance.isDeleted,
  'createdDate': instance.createdDate?.toIso8601String(),
  'createdByUserId': instance.createdByUserId,
  'modifiedDate': instance.modifiedDate?.toIso8601String(),
  'modifiedByUserId': instance.modifiedByUserId,
  'createdByUserFirstName': instance.createdByUserFirstName,
  'createdByUserLastName': instance.createdByUserLastName,
  'createdByUserEmail': instance.createdByUserEmail,
  'modifiedByUserFirstName': instance.modifiedByUserFirstName,
  'modifiedByUserLastName': instance.modifiedByUserLastName,
  'modifiedByUserEmail': instance.modifiedByUserEmail,
  'createdBy': instance.createdBy,
  'modifiedBy': instance.modifiedBy,
};

CompanyStorageConfigurationModelReturnModel
_$CompanyStorageConfigurationModelReturnModelFromJson(
  Map<String, dynamic> json,
) => CompanyStorageConfigurationModelReturnModel(
  model: json['model'] == null
      ? null
      : CompanyStorageConfigurationModel.fromJson(
          json['model'] as Map<String, dynamic>,
        ),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$CompanyStorageConfigurationModelReturnModelToJson(
  CompanyStorageConfigurationModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

CompanyStorageConfigurationUpdateModel
_$CompanyStorageConfigurationUpdateModelFromJson(Map<String, dynamic> json) =>
    CompanyStorageConfigurationUpdateModel(
      companyStorageConfigurationId:
          (json['companyStorageConfigurationId'] as num?)?.toInt(),
      companyStorageConfigurationGuid:
          json['companyStorageConfigurationGuid'] as String?,
      profilePictureFileStorageConnectionString:
          json['profilePictureFileStorageConnectionString'] as String?,
      reportServerFileStorageConnectionString:
          json['reportServerFileStorageConnectionString'] as String?,
      fileAttachmentFileStorageConnectionString:
          json['fileAttachmentFileStorageConnectionString'] as String?,
    );

Map<String, dynamic> _$CompanyStorageConfigurationUpdateModelToJson(
  CompanyStorageConfigurationUpdateModel instance,
) => <String, dynamic>{
  'companyStorageConfigurationId': instance.companyStorageConfigurationId,
  'companyStorageConfigurationGuid': instance.companyStorageConfigurationGuid,
  'profilePictureFileStorageConnectionString':
      instance.profilePictureFileStorageConnectionString,
  'reportServerFileStorageConnectionString':
      instance.reportServerFileStorageConnectionString,
  'fileAttachmentFileStorageConnectionString':
      instance.fileAttachmentFileStorageConnectionString,
};

ConfigurationGroupCreateModel _$ConfigurationGroupCreateModelFromJson(
  Map<String, dynamic> json,
) => ConfigurationGroupCreateModel(groupName: json['groupName'] as String?);

Map<String, dynamic> _$ConfigurationGroupCreateModelToJson(
  ConfigurationGroupCreateModel instance,
) => <String, dynamic>{'groupName': instance.groupName};

ConfigurationGroupModel _$ConfigurationGroupModelFromJson(
  Map<String, dynamic> json,
) => ConfigurationGroupModel(
  configurationGroupId: (json['configurationGroupId'] as num?)?.toInt(),
  groupName: json['groupName'] as String?,
  isDeleted: json['isDeleted'] as bool?,
  createdDate: json['createdDate'] == null
      ? null
      : DateTime.parse(json['createdDate'] as String),
  createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
  modifiedDate: json['modifiedDate'] == null
      ? null
      : DateTime.parse(json['modifiedDate'] as String),
  modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
  createdByUserFirstName: json['createdByUserFirstName'] as String?,
  createdByUserLastName: json['createdByUserLastName'] as String?,
  createdByUserEmail: json['createdByUserEmail'] as String?,
  modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
  modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
  modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
  createdBy: json['createdBy'] as String?,
  modifiedBy: json['modifiedBy'] as String?,
);

Map<String, dynamic> _$ConfigurationGroupModelToJson(
  ConfigurationGroupModel instance,
) => <String, dynamic>{
  'configurationGroupId': instance.configurationGroupId,
  'groupName': instance.groupName,
  'isDeleted': instance.isDeleted,
  'createdDate': instance.createdDate?.toIso8601String(),
  'createdByUserId': instance.createdByUserId,
  'modifiedDate': instance.modifiedDate?.toIso8601String(),
  'modifiedByUserId': instance.modifiedByUserId,
  'createdByUserFirstName': instance.createdByUserFirstName,
  'createdByUserLastName': instance.createdByUserLastName,
  'createdByUserEmail': instance.createdByUserEmail,
  'modifiedByUserFirstName': instance.modifiedByUserFirstName,
  'modifiedByUserLastName': instance.modifiedByUserLastName,
  'modifiedByUserEmail': instance.modifiedByUserEmail,
  'createdBy': instance.createdBy,
  'modifiedBy': instance.modifiedBy,
};

ConfigurationGroupModelReturnListModel
_$ConfigurationGroupModelReturnListModelFromJson(Map<String, dynamic> json) =>
    ConfigurationGroupModelReturnListModel(
      model:
          (json['model'] as List<dynamic>?)
              ?.map(
                (e) =>
                    ConfigurationGroupModel.fromJson(e as Map<String, dynamic>),
              )
              .toList() ??
          [],
      search: json['search'] == null
          ? null
          : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
      totalRecords: (json['totalRecords'] as num?)?.toInt(),
      isSuccess: json['isSuccess'] as bool?,
      successMessage: json['successMessage'] as String?,
      errorHolder: json['errorHolder'] == null
          ? null
          : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
      activityId: json['activityId'] as String?,
      activityDateTime: json['activityDateTime'] == null
          ? null
          : DateTime.parse(json['activityDateTime'] as String),
      server: json['server'] as String?,
      processId: (json['processId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$ConfigurationGroupModelReturnListModelToJson(
  ConfigurationGroupModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

ConfigurationGroupModelReturnModel _$ConfigurationGroupModelReturnModelFromJson(
  Map<String, dynamic> json,
) => ConfigurationGroupModelReturnModel(
  model: json['model'] == null
      ? null
      : ConfigurationGroupModel.fromJson(json['model'] as Map<String, dynamic>),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$ConfigurationGroupModelReturnModelToJson(
  ConfigurationGroupModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

ConfigurationGroupUpdateModel _$ConfigurationGroupUpdateModelFromJson(
  Map<String, dynamic> json,
) => ConfigurationGroupUpdateModel(
  configurationGroupId: (json['configurationGroupId'] as num?)?.toInt(),
  groupName: json['groupName'] as String?,
);

Map<String, dynamic> _$ConfigurationGroupUpdateModelToJson(
  ConfigurationGroupUpdateModel instance,
) => <String, dynamic>{
  'configurationGroupId': instance.configurationGroupId,
  'groupName': instance.groupName,
};

ConfigurationItemCreateModel _$ConfigurationItemCreateModelFromJson(
  Map<String, dynamic> json,
) => ConfigurationItemCreateModel(
  configurationGroupId: (json['configurationGroupId'] as num?)?.toInt(),
  key: json['key'] as String?,
  $value: json['value'] as String?,
  defaultValue: json['defaultValue'] as String?,
  required: json['required'] as bool?,
);

Map<String, dynamic> _$ConfigurationItemCreateModelToJson(
  ConfigurationItemCreateModel instance,
) => <String, dynamic>{
  'configurationGroupId': instance.configurationGroupId,
  'key': instance.key,
  'value': instance.$value,
  'defaultValue': instance.defaultValue,
  'required': instance.required,
};

ConfigurationItemModel _$ConfigurationItemModelFromJson(
  Map<String, dynamic> json,
) => ConfigurationItemModel(
  configurationItemId: (json['configurationItemId'] as num?)?.toInt(),
  configurationGroupId: (json['configurationGroupId'] as num?)?.toInt(),
  key: json['key'] as String?,
  $value: json['value'] as String?,
  defaultValue: json['defaultValue'] as String?,
  required: json['required'] as bool?,
  isDeleted: json['isDeleted'] as bool?,
  createdDate: json['createdDate'] == null
      ? null
      : DateTime.parse(json['createdDate'] as String),
  createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
  modifiedDate: json['modifiedDate'] == null
      ? null
      : DateTime.parse(json['modifiedDate'] as String),
  modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
  createdByUserFirstName: json['createdByUserFirstName'] as String?,
  createdByUserLastName: json['createdByUserLastName'] as String?,
  createdByUserEmail: json['createdByUserEmail'] as String?,
  modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
  modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
  modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
  createdBy: json['createdBy'] as String?,
  modifiedBy: json['modifiedBy'] as String?,
);

Map<String, dynamic> _$ConfigurationItemModelToJson(
  ConfigurationItemModel instance,
) => <String, dynamic>{
  'configurationItemId': instance.configurationItemId,
  'configurationGroupId': instance.configurationGroupId,
  'key': instance.key,
  'value': instance.$value,
  'defaultValue': instance.defaultValue,
  'required': instance.required,
  'isDeleted': instance.isDeleted,
  'createdDate': instance.createdDate?.toIso8601String(),
  'createdByUserId': instance.createdByUserId,
  'modifiedDate': instance.modifiedDate?.toIso8601String(),
  'modifiedByUserId': instance.modifiedByUserId,
  'createdByUserFirstName': instance.createdByUserFirstName,
  'createdByUserLastName': instance.createdByUserLastName,
  'createdByUserEmail': instance.createdByUserEmail,
  'modifiedByUserFirstName': instance.modifiedByUserFirstName,
  'modifiedByUserLastName': instance.modifiedByUserLastName,
  'modifiedByUserEmail': instance.modifiedByUserEmail,
  'createdBy': instance.createdBy,
  'modifiedBy': instance.modifiedBy,
};

ConfigurationItemModelReturnListModel
_$ConfigurationItemModelReturnListModelFromJson(Map<String, dynamic> json) =>
    ConfigurationItemModelReturnListModel(
      model:
          (json['model'] as List<dynamic>?)
              ?.map(
                (e) =>
                    ConfigurationItemModel.fromJson(e as Map<String, dynamic>),
              )
              .toList() ??
          [],
      search: json['search'] == null
          ? null
          : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
      totalRecords: (json['totalRecords'] as num?)?.toInt(),
      isSuccess: json['isSuccess'] as bool?,
      successMessage: json['successMessage'] as String?,
      errorHolder: json['errorHolder'] == null
          ? null
          : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
      activityId: json['activityId'] as String?,
      activityDateTime: json['activityDateTime'] == null
          ? null
          : DateTime.parse(json['activityDateTime'] as String),
      server: json['server'] as String?,
      processId: (json['processId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$ConfigurationItemModelReturnListModelToJson(
  ConfigurationItemModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

ConfigurationItemModelReturnModel _$ConfigurationItemModelReturnModelFromJson(
  Map<String, dynamic> json,
) => ConfigurationItemModelReturnModel(
  model: json['model'] == null
      ? null
      : ConfigurationItemModel.fromJson(json['model'] as Map<String, dynamic>),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$ConfigurationItemModelReturnModelToJson(
  ConfigurationItemModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

ConfigurationItemUpdateModel _$ConfigurationItemUpdateModelFromJson(
  Map<String, dynamic> json,
) => ConfigurationItemUpdateModel(
  configurationItemId: (json['configurationItemId'] as num?)?.toInt(),
  configurationGroupId: (json['configurationGroupId'] as num?)?.toInt(),
  key: json['key'] as String?,
  $value: json['value'] as String?,
  defaultValue: json['defaultValue'] as String?,
  required: json['required'] as bool?,
);

Map<String, dynamic> _$ConfigurationItemUpdateModelToJson(
  ConfigurationItemUpdateModel instance,
) => <String, dynamic>{
  'configurationItemId': instance.configurationItemId,
  'configurationGroupId': instance.configurationGroupId,
  'key': instance.key,
  'value': instance.$value,
  'defaultValue': instance.defaultValue,
  'required': instance.required,
};

CoordinatorConfigurationModel _$CoordinatorConfigurationModelFromJson(
  Map<String, dynamic> json,
) => CoordinatorConfigurationModel(
  coordinatorServerPort: (json['coordinatorServerPort'] as num?)?.toInt(),
  coordinatorServerName: (json['coordinatorServerName'] as num?)?.toInt(),
  coordinatorLockTimeoutInSeconds:
      (json['coordinatorLockTimeoutInSeconds'] as num?)?.toInt(),
  retryQueueConnectionString: json['retryQueueConnectionString'] as String?,
  configNotificationQueueConnectionString:
      json['configNotificationQueueConnectionString'] as String?,
);

Map<String, dynamic> _$CoordinatorConfigurationModelToJson(
  CoordinatorConfigurationModel instance,
) => <String, dynamic>{
  'coordinatorServerPort': instance.coordinatorServerPort,
  'coordinatorServerName': instance.coordinatorServerName,
  'coordinatorLockTimeoutInSeconds': instance.coordinatorLockTimeoutInSeconds,
  'retryQueueConnectionString': instance.retryQueueConnectionString,
  'configNotificationQueueConnectionString':
      instance.configNotificationQueueConnectionString,
};

CoordinatorConfigurationModelReturnModel
_$CoordinatorConfigurationModelReturnModelFromJson(Map<String, dynamic> json) =>
    CoordinatorConfigurationModelReturnModel(
      model: json['model'] == null
          ? null
          : CoordinatorConfigurationModel.fromJson(
              json['model'] as Map<String, dynamic>,
            ),
      isSuccess: json['isSuccess'] as bool?,
      successMessage: json['successMessage'] as String?,
      errorHolder: json['errorHolder'] == null
          ? null
          : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
      activityId: json['activityId'] as String?,
      activityDateTime: json['activityDateTime'] == null
          ? null
          : DateTime.parse(json['activityDateTime'] as String),
      server: json['server'] as String?,
      processId: (json['processId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$CoordinatorConfigurationModelReturnModelToJson(
  CoordinatorConfigurationModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

CountryModel _$CountryModelFromJson(Map<String, dynamic> json) => CountryModel(
  countryId: (json['countryId'] as num?)?.toInt(),
  countryName: json['countryName'] as String?,
  alternateCountryName1: json['alternateCountryName1'] as String?,
  alternateCountryName2: json['alternateCountryName2'] as String?,
  isDeleted: json['isDeleted'] as bool?,
  createdDate: json['createdDate'] == null
      ? null
      : DateTime.parse(json['createdDate'] as String),
  createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
  modifiedDate: json['modifiedDate'] == null
      ? null
      : DateTime.parse(json['modifiedDate'] as String),
  modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
);

Map<String, dynamic> _$CountryModelToJson(CountryModel instance) =>
    <String, dynamic>{
      'countryId': instance.countryId,
      'countryName': instance.countryName,
      'alternateCountryName1': instance.alternateCountryName1,
      'alternateCountryName2': instance.alternateCountryName2,
      'isDeleted': instance.isDeleted,
      'createdDate': instance.createdDate?.toIso8601String(),
      'createdByUserId': instance.createdByUserId,
      'modifiedDate': instance.modifiedDate?.toIso8601String(),
      'modifiedByUserId': instance.modifiedByUserId,
    };

CountryModelReturnListModel _$CountryModelReturnListModelFromJson(
  Map<String, dynamic> json,
) => CountryModelReturnListModel(
  model:
      (json['model'] as List<dynamic>?)
          ?.map((e) => CountryModel.fromJson(e as Map<String, dynamic>))
          .toList() ??
      [],
  search: json['search'] == null
      ? null
      : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
  totalRecords: (json['totalRecords'] as num?)?.toInt(),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$CountryModelReturnListModelToJson(
  CountryModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

DailyAffirmationSessionByDailyAffirmationSessionCategoryIdSearchCriteria
_$DailyAffirmationSessionByDailyAffirmationSessionCategoryIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => DailyAffirmationSessionByDailyAffirmationSessionCategoryIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  dailyAffirmationSessionCategoryId:
      (json['dailyAffirmationSessionCategoryId'] as num?)?.toInt(),
);

Map<String, dynamic>
_$DailyAffirmationSessionByDailyAffirmationSessionCategoryIdSearchCriteriaToJson(
  DailyAffirmationSessionByDailyAffirmationSessionCategoryIdSearchCriteria
  instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'dailyAffirmationSessionCategoryId':
      instance.dailyAffirmationSessionCategoryId,
};

DailyAffirmationSessionByFeedIdSearchCriteria
_$DailyAffirmationSessionByFeedIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => DailyAffirmationSessionByFeedIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  feedId: (json['feedId'] as num?)?.toInt(),
);

Map<String, dynamic> _$DailyAffirmationSessionByFeedIdSearchCriteriaToJson(
  DailyAffirmationSessionByFeedIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'feedId': instance.feedId,
};

DailyAffirmationSessionByUserIdSearchCriteria
_$DailyAffirmationSessionByUserIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => DailyAffirmationSessionByUserIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  userId: (json['userId'] as num?)?.toInt(),
);

Map<String, dynamic> _$DailyAffirmationSessionByUserIdSearchCriteriaToJson(
  DailyAffirmationSessionByUserIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'userId': instance.userId,
};

DailyAffirmationSessionCategoryByCategoryIdSearchCriteria
_$DailyAffirmationSessionCategoryByCategoryIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => DailyAffirmationSessionCategoryByCategoryIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  categoryId: (json['categoryId'] as num?)?.toInt(),
);

Map<String, dynamic>
_$DailyAffirmationSessionCategoryByCategoryIdSearchCriteriaToJson(
  DailyAffirmationSessionCategoryByCategoryIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'categoryId': instance.categoryId,
};

DailyAffirmationSessionCategoryByDailyAffirmationSessionIdSearchCriteria
_$DailyAffirmationSessionCategoryByDailyAffirmationSessionIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => DailyAffirmationSessionCategoryByDailyAffirmationSessionIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  dailyAffirmationSessionId: (json['dailyAffirmationSessionId'] as num?)
      ?.toInt(),
);

Map<String, dynamic>
_$DailyAffirmationSessionCategoryByDailyAffirmationSessionIdSearchCriteriaToJson(
  DailyAffirmationSessionCategoryByDailyAffirmationSessionIdSearchCriteria
  instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'dailyAffirmationSessionId': instance.dailyAffirmationSessionId,
};

DailyAffirmationSessionCategoryByUserIdSearchCriteria
_$DailyAffirmationSessionCategoryByUserIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => DailyAffirmationSessionCategoryByUserIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  userId: (json['userId'] as num?)?.toInt(),
);

Map<String, dynamic>
_$DailyAffirmationSessionCategoryByUserIdSearchCriteriaToJson(
  DailyAffirmationSessionCategoryByUserIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'userId': instance.userId,
};

DailyAffirmationSessionCategoryCreateModel
_$DailyAffirmationSessionCategoryCreateModelFromJson(
  Map<String, dynamic> json,
) => DailyAffirmationSessionCategoryCreateModel(
  categoryId: (json['categoryId'] as num?)?.toInt(),
  dailyAffirmationSessionId: (json['dailyAffirmationSessionId'] as num?)
      ?.toInt(),
  dailyAffirmationSessionCategoryTimeSpentInSeconds:
      (json['dailyAffirmationSessionCategoryTimeSpentInSeconds'] as num?)
          ?.toInt(),
  userId: (json['userId'] as num?)?.toInt(),
);

Map<String, dynamic> _$DailyAffirmationSessionCategoryCreateModelToJson(
  DailyAffirmationSessionCategoryCreateModel instance,
) => <String, dynamic>{
  'categoryId': instance.categoryId,
  'dailyAffirmationSessionId': instance.dailyAffirmationSessionId,
  'dailyAffirmationSessionCategoryTimeSpentInSeconds':
      instance.dailyAffirmationSessionCategoryTimeSpentInSeconds,
  'userId': instance.userId,
};

DailyAffirmationSessionCategoryModel
_$DailyAffirmationSessionCategoryModelFromJson(Map<String, dynamic> json) =>
    DailyAffirmationSessionCategoryModel(
      dailyAffirmationSessionCategoryId:
          (json['dailyAffirmationSessionCategoryId'] as num?)?.toInt(),
      categoryId: (json['categoryId'] as num?)?.toInt(),
      dailyAffirmationSessionId: (json['dailyAffirmationSessionId'] as num?)
          ?.toInt(),
      dailyAffirmationSessionCategoryTimeSpentInSeconds:
          (json['dailyAffirmationSessionCategoryTimeSpentInSeconds'] as num?)
              ?.toInt(),
      userId: (json['userId'] as num?)?.toInt(),
      isDeleted: json['isDeleted'] as bool?,
      createdDate: json['createdDate'] == null
          ? null
          : DateTime.parse(json['createdDate'] as String),
      createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
      modifiedDate: json['modifiedDate'] == null
          ? null
          : DateTime.parse(json['modifiedDate'] as String),
      modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
      categoryName: json['categoryName'] as String?,
      userName: json['userName'] as String?,
      createdByUserFirstName: json['createdByUserFirstName'] as String?,
      createdByUserLastName: json['createdByUserLastName'] as String?,
      createdByUserEmail: json['createdByUserEmail'] as String?,
      modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
      modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
      modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
      createdBy: json['createdBy'] as String?,
      modifiedBy: json['modifiedBy'] as String?,
    );

Map<String, dynamic> _$DailyAffirmationSessionCategoryModelToJson(
  DailyAffirmationSessionCategoryModel instance,
) => <String, dynamic>{
  'dailyAffirmationSessionCategoryId':
      instance.dailyAffirmationSessionCategoryId,
  'categoryId': instance.categoryId,
  'dailyAffirmationSessionId': instance.dailyAffirmationSessionId,
  'dailyAffirmationSessionCategoryTimeSpentInSeconds':
      instance.dailyAffirmationSessionCategoryTimeSpentInSeconds,
  'userId': instance.userId,
  'isDeleted': instance.isDeleted,
  'createdDate': instance.createdDate?.toIso8601String(),
  'createdByUserId': instance.createdByUserId,
  'modifiedDate': instance.modifiedDate?.toIso8601String(),
  'modifiedByUserId': instance.modifiedByUserId,
  'categoryName': instance.categoryName,
  'userName': instance.userName,
  'createdByUserFirstName': instance.createdByUserFirstName,
  'createdByUserLastName': instance.createdByUserLastName,
  'createdByUserEmail': instance.createdByUserEmail,
  'modifiedByUserFirstName': instance.modifiedByUserFirstName,
  'modifiedByUserLastName': instance.modifiedByUserLastName,
  'modifiedByUserEmail': instance.modifiedByUserEmail,
  'createdBy': instance.createdBy,
  'modifiedBy': instance.modifiedBy,
};

DailyAffirmationSessionCategoryModelReturnListModel
_$DailyAffirmationSessionCategoryModelReturnListModelFromJson(
  Map<String, dynamic> json,
) => DailyAffirmationSessionCategoryModelReturnListModel(
  model:
      (json['model'] as List<dynamic>?)
          ?.map(
            (e) => DailyAffirmationSessionCategoryModel.fromJson(
              e as Map<String, dynamic>,
            ),
          )
          .toList() ??
      [],
  search: json['search'] == null
      ? null
      : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
  totalRecords: (json['totalRecords'] as num?)?.toInt(),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic>
_$DailyAffirmationSessionCategoryModelReturnListModelToJson(
  DailyAffirmationSessionCategoryModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

DailyAffirmationSessionCategoryModelReturnModel
_$DailyAffirmationSessionCategoryModelReturnModelFromJson(
  Map<String, dynamic> json,
) => DailyAffirmationSessionCategoryModelReturnModel(
  model: json['model'] == null
      ? null
      : DailyAffirmationSessionCategoryModel.fromJson(
          json['model'] as Map<String, dynamic>,
        ),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$DailyAffirmationSessionCategoryModelReturnModelToJson(
  DailyAffirmationSessionCategoryModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

DailyAffirmationSessionCategoryUpdateModel
_$DailyAffirmationSessionCategoryUpdateModelFromJson(
  Map<String, dynamic> json,
) => DailyAffirmationSessionCategoryUpdateModel(
  dailyAffirmationSessionCategoryId:
      (json['dailyAffirmationSessionCategoryId'] as num?)?.toInt(),
  categoryId: (json['categoryId'] as num?)?.toInt(),
  dailyAffirmationSessionId: (json['dailyAffirmationSessionId'] as num?)
      ?.toInt(),
  dailyAffirmationSessionCategoryTimeSpentInSeconds:
      (json['dailyAffirmationSessionCategoryTimeSpentInSeconds'] as num?)
          ?.toInt(),
  userId: (json['userId'] as num?)?.toInt(),
);

Map<String, dynamic> _$DailyAffirmationSessionCategoryUpdateModelToJson(
  DailyAffirmationSessionCategoryUpdateModel instance,
) => <String, dynamic>{
  'dailyAffirmationSessionCategoryId':
      instance.dailyAffirmationSessionCategoryId,
  'categoryId': instance.categoryId,
  'dailyAffirmationSessionId': instance.dailyAffirmationSessionId,
  'dailyAffirmationSessionCategoryTimeSpentInSeconds':
      instance.dailyAffirmationSessionCategoryTimeSpentInSeconds,
  'userId': instance.userId,
};

DailyAffirmationSessionCreateModel _$DailyAffirmationSessionCreateModelFromJson(
  Map<String, dynamic> json,
) => DailyAffirmationSessionCreateModel(
  dailyAffirmationSessionDate: json['dailyAffirmationSessionDate'] == null
      ? null
      : DateTime.parse(json['dailyAffirmationSessionDate'] as String),
  dailyAffirmationSessionTimeSpentInSeconds:
      (json['dailyAffirmationSessionTimeSpentInSeconds'] as num?)?.toInt(),
  userId: (json['userId'] as num?)?.toInt(),
  addedToFeed: json['addedToFeed'] as bool?,
  addedToFeedDate: json['addedToFeedDate'] == null
      ? null
      : DateTime.parse(json['addedToFeedDate'] as String),
);

Map<String, dynamic> _$DailyAffirmationSessionCreateModelToJson(
  DailyAffirmationSessionCreateModel instance,
) => <String, dynamic>{
  'dailyAffirmationSessionDate': instance.dailyAffirmationSessionDate
      ?.toIso8601String(),
  'dailyAffirmationSessionTimeSpentInSeconds':
      instance.dailyAffirmationSessionTimeSpentInSeconds,
  'userId': instance.userId,
  'addedToFeed': instance.addedToFeed,
  'addedToFeedDate': instance.addedToFeedDate?.toIso8601String(),
};

DailyAffirmationSessionModel _$DailyAffirmationSessionModelFromJson(
  Map<String, dynamic> json,
) => DailyAffirmationSessionModel(
  dailyAffirmationSessionId: (json['dailyAffirmationSessionId'] as num?)
      ?.toInt(),
  dailyAffirmationSessionDate: json['dailyAffirmationSessionDate'] == null
      ? null
      : DateTime.parse(json['dailyAffirmationSessionDate'] as String),
  dailyAffirmationSessionTimeSpentInSeconds:
      (json['dailyAffirmationSessionTimeSpentInSeconds'] as num?)?.toInt(),
  userId: (json['userId'] as num?)?.toInt(),
  addedToFeed: json['addedToFeed'] as bool?,
  addedToFeedDate: json['addedToFeedDate'] == null
      ? null
      : DateTime.parse(json['addedToFeedDate'] as String),
  isDeleted: json['isDeleted'] as bool?,
  createdDate: json['createdDate'] == null
      ? null
      : DateTime.parse(json['createdDate'] as String),
  createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
  modifiedDate: json['modifiedDate'] == null
      ? null
      : DateTime.parse(json['modifiedDate'] as String),
  modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
  userName: json['userName'] as String?,
  createdByUserFirstName: json['createdByUserFirstName'] as String?,
  createdByUserLastName: json['createdByUserLastName'] as String?,
  createdByUserEmail: json['createdByUserEmail'] as String?,
  modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
  modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
  modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
  createdBy: json['createdBy'] as String?,
  modifiedBy: json['modifiedBy'] as String?,
);

Map<String, dynamic> _$DailyAffirmationSessionModelToJson(
  DailyAffirmationSessionModel instance,
) => <String, dynamic>{
  'dailyAffirmationSessionId': instance.dailyAffirmationSessionId,
  'dailyAffirmationSessionDate': instance.dailyAffirmationSessionDate
      ?.toIso8601String(),
  'dailyAffirmationSessionTimeSpentInSeconds':
      instance.dailyAffirmationSessionTimeSpentInSeconds,
  'userId': instance.userId,
  'addedToFeed': instance.addedToFeed,
  'addedToFeedDate': instance.addedToFeedDate?.toIso8601String(),
  'isDeleted': instance.isDeleted,
  'createdDate': instance.createdDate?.toIso8601String(),
  'createdByUserId': instance.createdByUserId,
  'modifiedDate': instance.modifiedDate?.toIso8601String(),
  'modifiedByUserId': instance.modifiedByUserId,
  'userName': instance.userName,
  'createdByUserFirstName': instance.createdByUserFirstName,
  'createdByUserLastName': instance.createdByUserLastName,
  'createdByUserEmail': instance.createdByUserEmail,
  'modifiedByUserFirstName': instance.modifiedByUserFirstName,
  'modifiedByUserLastName': instance.modifiedByUserLastName,
  'modifiedByUserEmail': instance.modifiedByUserEmail,
  'createdBy': instance.createdBy,
  'modifiedBy': instance.modifiedBy,
};

DailyAffirmationSessionModelReturnListModel
_$DailyAffirmationSessionModelReturnListModelFromJson(
  Map<String, dynamic> json,
) => DailyAffirmationSessionModelReturnListModel(
  model:
      (json['model'] as List<dynamic>?)
          ?.map(
            (e) => DailyAffirmationSessionModel.fromJson(
              e as Map<String, dynamic>,
            ),
          )
          .toList() ??
      [],
  search: json['search'] == null
      ? null
      : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
  totalRecords: (json['totalRecords'] as num?)?.toInt(),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$DailyAffirmationSessionModelReturnListModelToJson(
  DailyAffirmationSessionModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

DailyAffirmationSessionModelReturnModel
_$DailyAffirmationSessionModelReturnModelFromJson(Map<String, dynamic> json) =>
    DailyAffirmationSessionModelReturnModel(
      model: json['model'] == null
          ? null
          : DailyAffirmationSessionModel.fromJson(
              json['model'] as Map<String, dynamic>,
            ),
      isSuccess: json['isSuccess'] as bool?,
      successMessage: json['successMessage'] as String?,
      errorHolder: json['errorHolder'] == null
          ? null
          : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
      activityId: json['activityId'] as String?,
      activityDateTime: json['activityDateTime'] == null
          ? null
          : DateTime.parse(json['activityDateTime'] as String),
      server: json['server'] as String?,
      processId: (json['processId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$DailyAffirmationSessionModelReturnModelToJson(
  DailyAffirmationSessionModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

DailyAffirmationSessionUpdateModel _$DailyAffirmationSessionUpdateModelFromJson(
  Map<String, dynamic> json,
) => DailyAffirmationSessionUpdateModel(
  dailyAffirmationSessionId: (json['dailyAffirmationSessionId'] as num?)
      ?.toInt(),
  dailyAffirmationSessionDate: json['dailyAffirmationSessionDate'] == null
      ? null
      : DateTime.parse(json['dailyAffirmationSessionDate'] as String),
  dailyAffirmationSessionTimeSpentInSeconds:
      (json['dailyAffirmationSessionTimeSpentInSeconds'] as num?)?.toInt(),
  userId: (json['userId'] as num?)?.toInt(),
  addedToFeed: json['addedToFeed'] as bool?,
  addedToFeedDate: json['addedToFeedDate'] == null
      ? null
      : DateTime.parse(json['addedToFeedDate'] as String),
);

Map<String, dynamic> _$DailyAffirmationSessionUpdateModelToJson(
  DailyAffirmationSessionUpdateModel instance,
) => <String, dynamic>{
  'dailyAffirmationSessionId': instance.dailyAffirmationSessionId,
  'dailyAffirmationSessionDate': instance.dailyAffirmationSessionDate
      ?.toIso8601String(),
  'dailyAffirmationSessionTimeSpentInSeconds':
      instance.dailyAffirmationSessionTimeSpentInSeconds,
  'userId': instance.userId,
  'addedToFeed': instance.addedToFeed,
  'addedToFeedDate': instance.addedToFeedDate?.toIso8601String(),
};

DailyAffirmationTextByCategoryIdSearchCriteria
_$DailyAffirmationTextByCategoryIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => DailyAffirmationTextByCategoryIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  categoryId: (json['categoryId'] as num?)?.toInt(),
);

Map<String, dynamic> _$DailyAffirmationTextByCategoryIdSearchCriteriaToJson(
  DailyAffirmationTextByCategoryIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'categoryId': instance.categoryId,
};

DailyAffirmationTextByUserIdSearchCriteria
_$DailyAffirmationTextByUserIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => DailyAffirmationTextByUserIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  userId: (json['userId'] as num?)?.toInt(),
);

Map<String, dynamic> _$DailyAffirmationTextByUserIdSearchCriteriaToJson(
  DailyAffirmationTextByUserIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'userId': instance.userId,
};

DailyAffirmationTextCreateModel _$DailyAffirmationTextCreateModelFromJson(
  Map<String, dynamic> json,
) => DailyAffirmationTextCreateModel(
  dailyAffirmationTextValue: json['dailyAffirmationTextValue'] as String?,
  categoryId: (json['categoryId'] as num?)?.toInt(),
  userId: (json['userId'] as num?)?.toInt(),
);

Map<String, dynamic> _$DailyAffirmationTextCreateModelToJson(
  DailyAffirmationTextCreateModel instance,
) => <String, dynamic>{
  'dailyAffirmationTextValue': instance.dailyAffirmationTextValue,
  'categoryId': instance.categoryId,
  'userId': instance.userId,
};

DailyAffirmationTextModel _$DailyAffirmationTextModelFromJson(
  Map<String, dynamic> json,
) => DailyAffirmationTextModel(
  dailyAffirmationTextId: (json['dailyAffirmationTextId'] as num?)?.toInt(),
  dailyAffirmationTextValue: json['dailyAffirmationTextValue'] as String?,
  categoryId: (json['categoryId'] as num?)?.toInt(),
  userId: (json['userId'] as num?)?.toInt(),
  isDeleted: json['isDeleted'] as bool?,
  createdDate: json['createdDate'] == null
      ? null
      : DateTime.parse(json['createdDate'] as String),
  createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
  modifiedDate: json['modifiedDate'] == null
      ? null
      : DateTime.parse(json['modifiedDate'] as String),
  modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
  categoryName: json['categoryName'] as String?,
  userName: json['userName'] as String?,
  createdByUserFirstName: json['createdByUserFirstName'] as String?,
  createdByUserLastName: json['createdByUserLastName'] as String?,
  createdByUserEmail: json['createdByUserEmail'] as String?,
  modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
  modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
  modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
  createdBy: json['createdBy'] as String?,
  modifiedBy: json['modifiedBy'] as String?,
);

Map<String, dynamic> _$DailyAffirmationTextModelToJson(
  DailyAffirmationTextModel instance,
) => <String, dynamic>{
  'dailyAffirmationTextId': instance.dailyAffirmationTextId,
  'dailyAffirmationTextValue': instance.dailyAffirmationTextValue,
  'categoryId': instance.categoryId,
  'userId': instance.userId,
  'isDeleted': instance.isDeleted,
  'createdDate': instance.createdDate?.toIso8601String(),
  'createdByUserId': instance.createdByUserId,
  'modifiedDate': instance.modifiedDate?.toIso8601String(),
  'modifiedByUserId': instance.modifiedByUserId,
  'categoryName': instance.categoryName,
  'userName': instance.userName,
  'createdByUserFirstName': instance.createdByUserFirstName,
  'createdByUserLastName': instance.createdByUserLastName,
  'createdByUserEmail': instance.createdByUserEmail,
  'modifiedByUserFirstName': instance.modifiedByUserFirstName,
  'modifiedByUserLastName': instance.modifiedByUserLastName,
  'modifiedByUserEmail': instance.modifiedByUserEmail,
  'createdBy': instance.createdBy,
  'modifiedBy': instance.modifiedBy,
};

DailyAffirmationTextModelReturnListModel
_$DailyAffirmationTextModelReturnListModelFromJson(Map<String, dynamic> json) =>
    DailyAffirmationTextModelReturnListModel(
      model:
          (json['model'] as List<dynamic>?)
              ?.map(
                (e) => DailyAffirmationTextModel.fromJson(
                  e as Map<String, dynamic>,
                ),
              )
              .toList() ??
          [],
      search: json['search'] == null
          ? null
          : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
      totalRecords: (json['totalRecords'] as num?)?.toInt(),
      isSuccess: json['isSuccess'] as bool?,
      successMessage: json['successMessage'] as String?,
      errorHolder: json['errorHolder'] == null
          ? null
          : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
      activityId: json['activityId'] as String?,
      activityDateTime: json['activityDateTime'] == null
          ? null
          : DateTime.parse(json['activityDateTime'] as String),
      server: json['server'] as String?,
      processId: (json['processId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$DailyAffirmationTextModelReturnListModelToJson(
  DailyAffirmationTextModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

DailyAffirmationTextModelReturnModel
_$DailyAffirmationTextModelReturnModelFromJson(Map<String, dynamic> json) =>
    DailyAffirmationTextModelReturnModel(
      model: json['model'] == null
          ? null
          : DailyAffirmationTextModel.fromJson(
              json['model'] as Map<String, dynamic>,
            ),
      isSuccess: json['isSuccess'] as bool?,
      successMessage: json['successMessage'] as String?,
      errorHolder: json['errorHolder'] == null
          ? null
          : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
      activityId: json['activityId'] as String?,
      activityDateTime: json['activityDateTime'] == null
          ? null
          : DateTime.parse(json['activityDateTime'] as String),
      server: json['server'] as String?,
      processId: (json['processId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$DailyAffirmationTextModelReturnModelToJson(
  DailyAffirmationTextModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

DailyAffirmationTextUpdateModel _$DailyAffirmationTextUpdateModelFromJson(
  Map<String, dynamic> json,
) => DailyAffirmationTextUpdateModel(
  dailyAffirmationTextId: (json['dailyAffirmationTextId'] as num?)?.toInt(),
  dailyAffirmationTextValue: json['dailyAffirmationTextValue'] as String?,
  categoryId: (json['categoryId'] as num?)?.toInt(),
  userId: (json['userId'] as num?)?.toInt(),
);

Map<String, dynamic> _$DailyAffirmationTextUpdateModelToJson(
  DailyAffirmationTextUpdateModel instance,
) => <String, dynamic>{
  'dailyAffirmationTextId': instance.dailyAffirmationTextId,
  'dailyAffirmationTextValue': instance.dailyAffirmationTextValue,
  'categoryId': instance.categoryId,
  'userId': instance.userId,
};

DailyJournalByUserIdSearchCriteria _$DailyJournalByUserIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => DailyJournalByUserIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  userId: (json['userId'] as num?)?.toInt(),
);

Map<String, dynamic> _$DailyJournalByUserIdSearchCriteriaToJson(
  DailyJournalByUserIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'userId': instance.userId,
};

DailyJournalCreateModel _$DailyJournalCreateModelFromJson(
  Map<String, dynamic> json,
) => DailyJournalCreateModel(
  dailyJournalDate: json['dailyJournalDate'] == null
      ? null
      : DateTime.parse(json['dailyJournalDate'] as String),
  userId: (json['userId'] as num?)?.toInt(),
  predefinedMoodTypeId: (json['predefinedMoodTypeId'] as num?)?.toInt(),
  gratitude1: json['gratitude1'] as String?,
  gratitude2: json['gratitude2'] as String?,
  gratitude3: json['gratitude3'] as String?,
  gratitude4: json['gratitude4'] as String?,
  gratitude5: json['gratitude5'] as String?,
  yourReflection: json['yourReflection'] as String?,
  tommorowsIntentions: json['tommorowsIntentions'] as String?,
);

Map<String, dynamic> _$DailyJournalCreateModelToJson(
  DailyJournalCreateModel instance,
) => <String, dynamic>{
  'dailyJournalDate': instance.dailyJournalDate?.toIso8601String(),
  'userId': instance.userId,
  'predefinedMoodTypeId': instance.predefinedMoodTypeId,
  'gratitude1': instance.gratitude1,
  'gratitude2': instance.gratitude2,
  'gratitude3': instance.gratitude3,
  'gratitude4': instance.gratitude4,
  'gratitude5': instance.gratitude5,
  'yourReflection': instance.yourReflection,
  'tommorowsIntentions': instance.tommorowsIntentions,
};

DailyJournalModel _$DailyJournalModelFromJson(Map<String, dynamic> json) =>
    DailyJournalModel(
      dailyJournalId: (json['dailyJournalId'] as num?)?.toInt(),
      dailyJournalDate: json['dailyJournalDate'] == null
          ? null
          : DateTime.parse(json['dailyJournalDate'] as String),
      userId: (json['userId'] as num?)?.toInt(),
      predefinedMoodTypeId: (json['predefinedMoodTypeId'] as num?)?.toInt(),
      gratitude1: json['gratitude1'] as String?,
      gratitude2: json['gratitude2'] as String?,
      gratitude3: json['gratitude3'] as String?,
      gratitude4: json['gratitude4'] as String?,
      gratitude5: json['gratitude5'] as String?,
      yourReflection: json['yourReflection'] as String?,
      tommorowsIntentions: json['tommorowsIntentions'] as String?,
      isDeleted: json['isDeleted'] as bool?,
      createdDate: json['createdDate'] == null
          ? null
          : DateTime.parse(json['createdDate'] as String),
      createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
      modifiedDate: json['modifiedDate'] == null
          ? null
          : DateTime.parse(json['modifiedDate'] as String),
      modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
      userName: json['userName'] as String?,
      predefinedMoodTypeName: json['predefinedMoodTypeName'] as String?,
      createdByUserFirstName: json['createdByUserFirstName'] as String?,
      createdByUserLastName: json['createdByUserLastName'] as String?,
      createdByUserEmail: json['createdByUserEmail'] as String?,
      modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
      modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
      modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
      createdBy: json['createdBy'] as String?,
      modifiedBy: json['modifiedBy'] as String?,
    );

Map<String, dynamic> _$DailyJournalModelToJson(DailyJournalModel instance) =>
    <String, dynamic>{
      'dailyJournalId': instance.dailyJournalId,
      'dailyJournalDate': instance.dailyJournalDate?.toIso8601String(),
      'userId': instance.userId,
      'predefinedMoodTypeId': instance.predefinedMoodTypeId,
      'gratitude1': instance.gratitude1,
      'gratitude2': instance.gratitude2,
      'gratitude3': instance.gratitude3,
      'gratitude4': instance.gratitude4,
      'gratitude5': instance.gratitude5,
      'yourReflection': instance.yourReflection,
      'tommorowsIntentions': instance.tommorowsIntentions,
      'isDeleted': instance.isDeleted,
      'createdDate': instance.createdDate?.toIso8601String(),
      'createdByUserId': instance.createdByUserId,
      'modifiedDate': instance.modifiedDate?.toIso8601String(),
      'modifiedByUserId': instance.modifiedByUserId,
      'userName': instance.userName,
      'predefinedMoodTypeName': instance.predefinedMoodTypeName,
      'createdByUserFirstName': instance.createdByUserFirstName,
      'createdByUserLastName': instance.createdByUserLastName,
      'createdByUserEmail': instance.createdByUserEmail,
      'modifiedByUserFirstName': instance.modifiedByUserFirstName,
      'modifiedByUserLastName': instance.modifiedByUserLastName,
      'modifiedByUserEmail': instance.modifiedByUserEmail,
      'createdBy': instance.createdBy,
      'modifiedBy': instance.modifiedBy,
    };

DailyJournalModelReturnListModel _$DailyJournalModelReturnListModelFromJson(
  Map<String, dynamic> json,
) => DailyJournalModelReturnListModel(
  model:
      (json['model'] as List<dynamic>?)
          ?.map((e) => DailyJournalModel.fromJson(e as Map<String, dynamic>))
          .toList() ??
      [],
  search: json['search'] == null
      ? null
      : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
  totalRecords: (json['totalRecords'] as num?)?.toInt(),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$DailyJournalModelReturnListModelToJson(
  DailyJournalModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

DailyJournalModelReturnModel _$DailyJournalModelReturnModelFromJson(
  Map<String, dynamic> json,
) => DailyJournalModelReturnModel(
  model: json['model'] == null
      ? null
      : DailyJournalModel.fromJson(json['model'] as Map<String, dynamic>),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$DailyJournalModelReturnModelToJson(
  DailyJournalModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

DailyJournalUpdateModel _$DailyJournalUpdateModelFromJson(
  Map<String, dynamic> json,
) => DailyJournalUpdateModel(
  dailyJournalId: (json['dailyJournalId'] as num?)?.toInt(),
  dailyJournalDate: json['dailyJournalDate'] == null
      ? null
      : DateTime.parse(json['dailyJournalDate'] as String),
  userId: (json['userId'] as num?)?.toInt(),
  predefinedMoodTypeId: (json['predefinedMoodTypeId'] as num?)?.toInt(),
  gratitude1: json['gratitude1'] as String?,
  gratitude2: json['gratitude2'] as String?,
  gratitude3: json['gratitude3'] as String?,
  gratitude4: json['gratitude4'] as String?,
  gratitude5: json['gratitude5'] as String?,
  yourReflection: json['yourReflection'] as String?,
  tommorowsIntentions: json['tommorowsIntentions'] as String?,
);

Map<String, dynamic> _$DailyJournalUpdateModelToJson(
  DailyJournalUpdateModel instance,
) => <String, dynamic>{
  'dailyJournalId': instance.dailyJournalId,
  'dailyJournalDate': instance.dailyJournalDate?.toIso8601String(),
  'userId': instance.userId,
  'predefinedMoodTypeId': instance.predefinedMoodTypeId,
  'gratitude1': instance.gratitude1,
  'gratitude2': instance.gratitude2,
  'gratitude3': instance.gratitude3,
  'gratitude4': instance.gratitude4,
  'gratitude5': instance.gratitude5,
  'yourReflection': instance.yourReflection,
  'tommorowsIntentions': instance.tommorowsIntentions,
};

DefaultCategoryByCategoryIdSearchCriteria
_$DefaultCategoryByCategoryIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => DefaultCategoryByCategoryIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  categoryId: (json['categoryId'] as num?)?.toInt(),
);

Map<String, dynamic> _$DefaultCategoryByCategoryIdSearchCriteriaToJson(
  DefaultCategoryByCategoryIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'categoryId': instance.categoryId,
};

DefaultCategoryByDefaultDailyAffirmationTextIdSearchCriteria
_$DefaultCategoryByDefaultDailyAffirmationTextIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => DefaultCategoryByDefaultDailyAffirmationTextIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  defaultDailyAffirmationTextId: (json['defaultDailyAffirmationTextId'] as num?)
      ?.toInt(),
);

Map<String, dynamic>
_$DefaultCategoryByDefaultDailyAffirmationTextIdSearchCriteriaToJson(
  DefaultCategoryByDefaultDailyAffirmationTextIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'defaultDailyAffirmationTextId': instance.defaultDailyAffirmationTextId,
};

DefaultCategoryByDefaultGoalTrackerCategoryIdSearchCriteria
_$DefaultCategoryByDefaultGoalTrackerCategoryIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => DefaultCategoryByDefaultGoalTrackerCategoryIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  defaultGoalTrackerCategoryId: (json['defaultGoalTrackerCategoryId'] as num?)
      ?.toInt(),
);

Map<String, dynamic>
_$DefaultCategoryByDefaultGoalTrackerCategoryIdSearchCriteriaToJson(
  DefaultCategoryByDefaultGoalTrackerCategoryIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'defaultGoalTrackerCategoryId': instance.defaultGoalTrackerCategoryId,
};

DefaultCategoryCreateModel _$DefaultCategoryCreateModelFromJson(
  Map<String, dynamic> json,
) => DefaultCategoryCreateModel(
  defaultCategoryName: json['defaultCategoryName'] as String?,
  predefinedCategoryTypeId: (json['predefinedCategoryTypeId'] as num?)?.toInt(),
  predefinedIconTypeId: (json['predefinedIconTypeId'] as num?)?.toInt(),
);

Map<String, dynamic> _$DefaultCategoryCreateModelToJson(
  DefaultCategoryCreateModel instance,
) => <String, dynamic>{
  'defaultCategoryName': instance.defaultCategoryName,
  'predefinedCategoryTypeId': instance.predefinedCategoryTypeId,
  'predefinedIconTypeId': instance.predefinedIconTypeId,
};

DefaultCategoryModel _$DefaultCategoryModelFromJson(
  Map<String, dynamic> json,
) => DefaultCategoryModel(
  defaultCategoryId: (json['defaultCategoryId'] as num?)?.toInt(),
  defaultCategoryName: json['defaultCategoryName'] as String?,
  predefinedCategoryTypeId: (json['predefinedCategoryTypeId'] as num?)?.toInt(),
  predefinedIconTypeId: (json['predefinedIconTypeId'] as num?)?.toInt(),
  isDeleted: json['isDeleted'] as bool?,
  createdDate: json['createdDate'] == null
      ? null
      : DateTime.parse(json['createdDate'] as String),
  createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
  modifiedDate: json['modifiedDate'] == null
      ? null
      : DateTime.parse(json['modifiedDate'] as String),
  modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
  predefinedIconTypeName: json['predefinedIconTypeName'] as String?,
  predefinedCategoryTypeName: json['predefinedCategoryTypeName'] as String?,
  createdByUserFirstName: json['createdByUserFirstName'] as String?,
  createdByUserLastName: json['createdByUserLastName'] as String?,
  createdByUserEmail: json['createdByUserEmail'] as String?,
  modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
  modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
  modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
  createdBy: json['createdBy'] as String?,
  modifiedBy: json['modifiedBy'] as String?,
);

Map<String, dynamic> _$DefaultCategoryModelToJson(
  DefaultCategoryModel instance,
) => <String, dynamic>{
  'defaultCategoryId': instance.defaultCategoryId,
  'defaultCategoryName': instance.defaultCategoryName,
  'predefinedCategoryTypeId': instance.predefinedCategoryTypeId,
  'predefinedIconTypeId': instance.predefinedIconTypeId,
  'isDeleted': instance.isDeleted,
  'createdDate': instance.createdDate?.toIso8601String(),
  'createdByUserId': instance.createdByUserId,
  'modifiedDate': instance.modifiedDate?.toIso8601String(),
  'modifiedByUserId': instance.modifiedByUserId,
  'predefinedIconTypeName': instance.predefinedIconTypeName,
  'predefinedCategoryTypeName': instance.predefinedCategoryTypeName,
  'createdByUserFirstName': instance.createdByUserFirstName,
  'createdByUserLastName': instance.createdByUserLastName,
  'createdByUserEmail': instance.createdByUserEmail,
  'modifiedByUserFirstName': instance.modifiedByUserFirstName,
  'modifiedByUserLastName': instance.modifiedByUserLastName,
  'modifiedByUserEmail': instance.modifiedByUserEmail,
  'createdBy': instance.createdBy,
  'modifiedBy': instance.modifiedBy,
};

DefaultCategoryModelReturnListModel
_$DefaultCategoryModelReturnListModelFromJson(Map<String, dynamic> json) =>
    DefaultCategoryModelReturnListModel(
      model:
          (json['model'] as List<dynamic>?)
              ?.map(
                (e) => DefaultCategoryModel.fromJson(e as Map<String, dynamic>),
              )
              .toList() ??
          [],
      search: json['search'] == null
          ? null
          : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
      totalRecords: (json['totalRecords'] as num?)?.toInt(),
      isSuccess: json['isSuccess'] as bool?,
      successMessage: json['successMessage'] as String?,
      errorHolder: json['errorHolder'] == null
          ? null
          : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
      activityId: json['activityId'] as String?,
      activityDateTime: json['activityDateTime'] == null
          ? null
          : DateTime.parse(json['activityDateTime'] as String),
      server: json['server'] as String?,
      processId: (json['processId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$DefaultCategoryModelReturnListModelToJson(
  DefaultCategoryModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

DefaultCategoryModelReturnModel _$DefaultCategoryModelReturnModelFromJson(
  Map<String, dynamic> json,
) => DefaultCategoryModelReturnModel(
  model: json['model'] == null
      ? null
      : DefaultCategoryModel.fromJson(json['model'] as Map<String, dynamic>),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$DefaultCategoryModelReturnModelToJson(
  DefaultCategoryModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

DefaultCategoryUpdateModel _$DefaultCategoryUpdateModelFromJson(
  Map<String, dynamic> json,
) => DefaultCategoryUpdateModel(
  defaultCategoryId: (json['defaultCategoryId'] as num?)?.toInt(),
  defaultCategoryName: json['defaultCategoryName'] as String?,
  predefinedCategoryTypeId: (json['predefinedCategoryTypeId'] as num?)?.toInt(),
  predefinedIconTypeId: (json['predefinedIconTypeId'] as num?)?.toInt(),
);

Map<String, dynamic> _$DefaultCategoryUpdateModelToJson(
  DefaultCategoryUpdateModel instance,
) => <String, dynamic>{
  'defaultCategoryId': instance.defaultCategoryId,
  'defaultCategoryName': instance.defaultCategoryName,
  'predefinedCategoryTypeId': instance.predefinedCategoryTypeId,
  'predefinedIconTypeId': instance.predefinedIconTypeId,
};

DefaultDailyAffirmationTextByDefaultCategoryIdSearchCriteria
_$DefaultDailyAffirmationTextByDefaultCategoryIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => DefaultDailyAffirmationTextByDefaultCategoryIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  defaultCategoryId: (json['defaultCategoryId'] as num?)?.toInt(),
);

Map<String, dynamic>
_$DefaultDailyAffirmationTextByDefaultCategoryIdSearchCriteriaToJson(
  DefaultDailyAffirmationTextByDefaultCategoryIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'defaultCategoryId': instance.defaultCategoryId,
};

DefaultDailyAffirmationTextCreateModel
_$DefaultDailyAffirmationTextCreateModelFromJson(Map<String, dynamic> json) =>
    DefaultDailyAffirmationTextCreateModel(
      defaultDailyAffirmationTextName:
          json['defaultDailyAffirmationTextName'] as String?,
      defaultCategoryId: (json['defaultCategoryId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$DefaultDailyAffirmationTextCreateModelToJson(
  DefaultDailyAffirmationTextCreateModel instance,
) => <String, dynamic>{
  'defaultDailyAffirmationTextName': instance.defaultDailyAffirmationTextName,
  'defaultCategoryId': instance.defaultCategoryId,
};

DefaultDailyAffirmationTextModel _$DefaultDailyAffirmationTextModelFromJson(
  Map<String, dynamic> json,
) => DefaultDailyAffirmationTextModel(
  defaultDailyAffirmationTextId: (json['defaultDailyAffirmationTextId'] as num?)
      ?.toInt(),
  defaultDailyAffirmationTextName:
      json['defaultDailyAffirmationTextName'] as String?,
  defaultCategoryId: (json['defaultCategoryId'] as num?)?.toInt(),
  isDeleted: json['isDeleted'] as bool?,
  createdDate: json['createdDate'] == null
      ? null
      : DateTime.parse(json['createdDate'] as String),
  createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
  modifiedDate: json['modifiedDate'] == null
      ? null
      : DateTime.parse(json['modifiedDate'] as String),
  modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
  defaultCategoryName: json['defaultCategoryName'] as String?,
  createdByUserFirstName: json['createdByUserFirstName'] as String?,
  createdByUserLastName: json['createdByUserLastName'] as String?,
  createdByUserEmail: json['createdByUserEmail'] as String?,
  modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
  modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
  modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
  createdBy: json['createdBy'] as String?,
  modifiedBy: json['modifiedBy'] as String?,
);

Map<String, dynamic> _$DefaultDailyAffirmationTextModelToJson(
  DefaultDailyAffirmationTextModel instance,
) => <String, dynamic>{
  'defaultDailyAffirmationTextId': instance.defaultDailyAffirmationTextId,
  'defaultDailyAffirmationTextName': instance.defaultDailyAffirmationTextName,
  'defaultCategoryId': instance.defaultCategoryId,
  'isDeleted': instance.isDeleted,
  'createdDate': instance.createdDate?.toIso8601String(),
  'createdByUserId': instance.createdByUserId,
  'modifiedDate': instance.modifiedDate?.toIso8601String(),
  'modifiedByUserId': instance.modifiedByUserId,
  'defaultCategoryName': instance.defaultCategoryName,
  'createdByUserFirstName': instance.createdByUserFirstName,
  'createdByUserLastName': instance.createdByUserLastName,
  'createdByUserEmail': instance.createdByUserEmail,
  'modifiedByUserFirstName': instance.modifiedByUserFirstName,
  'modifiedByUserLastName': instance.modifiedByUserLastName,
  'modifiedByUserEmail': instance.modifiedByUserEmail,
  'createdBy': instance.createdBy,
  'modifiedBy': instance.modifiedBy,
};

DefaultDailyAffirmationTextModelReturnListModel
_$DefaultDailyAffirmationTextModelReturnListModelFromJson(
  Map<String, dynamic> json,
) => DefaultDailyAffirmationTextModelReturnListModel(
  model:
      (json['model'] as List<dynamic>?)
          ?.map(
            (e) => DefaultDailyAffirmationTextModel.fromJson(
              e as Map<String, dynamic>,
            ),
          )
          .toList() ??
      [],
  search: json['search'] == null
      ? null
      : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
  totalRecords: (json['totalRecords'] as num?)?.toInt(),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$DefaultDailyAffirmationTextModelReturnListModelToJson(
  DefaultDailyAffirmationTextModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

DefaultDailyAffirmationTextModelReturnModel
_$DefaultDailyAffirmationTextModelReturnModelFromJson(
  Map<String, dynamic> json,
) => DefaultDailyAffirmationTextModelReturnModel(
  model: json['model'] == null
      ? null
      : DefaultDailyAffirmationTextModel.fromJson(
          json['model'] as Map<String, dynamic>,
        ),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$DefaultDailyAffirmationTextModelReturnModelToJson(
  DefaultDailyAffirmationTextModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

DefaultDailyAffirmationTextUpdateModel
_$DefaultDailyAffirmationTextUpdateModelFromJson(Map<String, dynamic> json) =>
    DefaultDailyAffirmationTextUpdateModel(
      defaultDailyAffirmationTextId:
          (json['defaultDailyAffirmationTextId'] as num?)?.toInt(),
      defaultDailyAffirmationTextName:
          json['defaultDailyAffirmationTextName'] as String?,
      defaultCategoryId: (json['defaultCategoryId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$DefaultDailyAffirmationTextUpdateModelToJson(
  DefaultDailyAffirmationTextUpdateModel instance,
) => <String, dynamic>{
  'defaultDailyAffirmationTextId': instance.defaultDailyAffirmationTextId,
  'defaultDailyAffirmationTextName': instance.defaultDailyAffirmationTextName,
  'defaultCategoryId': instance.defaultCategoryId,
};

DefaultGoalTrackerByDefaultGoalTrackerCategoryIdSearchCriteria
_$DefaultGoalTrackerByDefaultGoalTrackerCategoryIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => DefaultGoalTrackerByDefaultGoalTrackerCategoryIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  defaultGoalTrackerCategoryId: (json['defaultGoalTrackerCategoryId'] as num?)
      ?.toInt(),
);

Map<String, dynamic>
_$DefaultGoalTrackerByDefaultGoalTrackerCategoryIdSearchCriteriaToJson(
  DefaultGoalTrackerByDefaultGoalTrackerCategoryIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'defaultGoalTrackerCategoryId': instance.defaultGoalTrackerCategoryId,
};

DefaultGoalTrackerCategoryByDefaultCategoryIdSearchCriteria
_$DefaultGoalTrackerCategoryByDefaultCategoryIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => DefaultGoalTrackerCategoryByDefaultCategoryIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  defaultCategoryId: (json['defaultCategoryId'] as num?)?.toInt(),
);

Map<String, dynamic>
_$DefaultGoalTrackerCategoryByDefaultCategoryIdSearchCriteriaToJson(
  DefaultGoalTrackerCategoryByDefaultCategoryIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'defaultCategoryId': instance.defaultCategoryId,
};

DefaultGoalTrackerCategoryByDefaultGoalTrackerIdSearchCriteria
_$DefaultGoalTrackerCategoryByDefaultGoalTrackerIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => DefaultGoalTrackerCategoryByDefaultGoalTrackerIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  defaultGoalTrackerId: (json['defaultGoalTrackerId'] as num?)?.toInt(),
);

Map<String, dynamic>
_$DefaultGoalTrackerCategoryByDefaultGoalTrackerIdSearchCriteriaToJson(
  DefaultGoalTrackerCategoryByDefaultGoalTrackerIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'defaultGoalTrackerId': instance.defaultGoalTrackerId,
};

DefaultGoalTrackerCategoryCreateModel
_$DefaultGoalTrackerCategoryCreateModelFromJson(Map<String, dynamic> json) =>
    DefaultGoalTrackerCategoryCreateModel(
      defaultCategoryId: (json['defaultCategoryId'] as num?)?.toInt(),
      defaultGoalTrackerId: (json['defaultGoalTrackerId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$DefaultGoalTrackerCategoryCreateModelToJson(
  DefaultGoalTrackerCategoryCreateModel instance,
) => <String, dynamic>{
  'defaultCategoryId': instance.defaultCategoryId,
  'defaultGoalTrackerId': instance.defaultGoalTrackerId,
};

DefaultGoalTrackerCategoryModel _$DefaultGoalTrackerCategoryModelFromJson(
  Map<String, dynamic> json,
) => DefaultGoalTrackerCategoryModel(
  defaultGoalTrackerCategoryId: (json['defaultGoalTrackerCategoryId'] as num?)
      ?.toInt(),
  defaultCategoryId: (json['defaultCategoryId'] as num?)?.toInt(),
  defaultGoalTrackerId: (json['defaultGoalTrackerId'] as num?)?.toInt(),
  isDeleted: json['isDeleted'] as bool?,
  createdDate: json['createdDate'] == null
      ? null
      : DateTime.parse(json['createdDate'] as String),
  createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
  modifiedDate: json['modifiedDate'] == null
      ? null
      : DateTime.parse(json['modifiedDate'] as String),
  modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
  defaultCategoryName: json['defaultCategoryName'] as String?,
  createdByUserFirstName: json['createdByUserFirstName'] as String?,
  createdByUserLastName: json['createdByUserLastName'] as String?,
  createdByUserEmail: json['createdByUserEmail'] as String?,
  modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
  modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
  modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
  createdBy: json['createdBy'] as String?,
  modifiedBy: json['modifiedBy'] as String?,
);

Map<String, dynamic> _$DefaultGoalTrackerCategoryModelToJson(
  DefaultGoalTrackerCategoryModel instance,
) => <String, dynamic>{
  'defaultGoalTrackerCategoryId': instance.defaultGoalTrackerCategoryId,
  'defaultCategoryId': instance.defaultCategoryId,
  'defaultGoalTrackerId': instance.defaultGoalTrackerId,
  'isDeleted': instance.isDeleted,
  'createdDate': instance.createdDate?.toIso8601String(),
  'createdByUserId': instance.createdByUserId,
  'modifiedDate': instance.modifiedDate?.toIso8601String(),
  'modifiedByUserId': instance.modifiedByUserId,
  'defaultCategoryName': instance.defaultCategoryName,
  'createdByUserFirstName': instance.createdByUserFirstName,
  'createdByUserLastName': instance.createdByUserLastName,
  'createdByUserEmail': instance.createdByUserEmail,
  'modifiedByUserFirstName': instance.modifiedByUserFirstName,
  'modifiedByUserLastName': instance.modifiedByUserLastName,
  'modifiedByUserEmail': instance.modifiedByUserEmail,
  'createdBy': instance.createdBy,
  'modifiedBy': instance.modifiedBy,
};

DefaultGoalTrackerCategoryModelReturnListModel
_$DefaultGoalTrackerCategoryModelReturnListModelFromJson(
  Map<String, dynamic> json,
) => DefaultGoalTrackerCategoryModelReturnListModel(
  model:
      (json['model'] as List<dynamic>?)
          ?.map(
            (e) => DefaultGoalTrackerCategoryModel.fromJson(
              e as Map<String, dynamic>,
            ),
          )
          .toList() ??
      [],
  search: json['search'] == null
      ? null
      : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
  totalRecords: (json['totalRecords'] as num?)?.toInt(),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$DefaultGoalTrackerCategoryModelReturnListModelToJson(
  DefaultGoalTrackerCategoryModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

DefaultGoalTrackerCategoryModelReturnModel
_$DefaultGoalTrackerCategoryModelReturnModelFromJson(
  Map<String, dynamic> json,
) => DefaultGoalTrackerCategoryModelReturnModel(
  model: json['model'] == null
      ? null
      : DefaultGoalTrackerCategoryModel.fromJson(
          json['model'] as Map<String, dynamic>,
        ),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$DefaultGoalTrackerCategoryModelReturnModelToJson(
  DefaultGoalTrackerCategoryModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

DefaultGoalTrackerCategoryUpdateModel
_$DefaultGoalTrackerCategoryUpdateModelFromJson(Map<String, dynamic> json) =>
    DefaultGoalTrackerCategoryUpdateModel(
      defaultGoalTrackerCategoryId:
          (json['defaultGoalTrackerCategoryId'] as num?)?.toInt(),
      defaultCategoryId: (json['defaultCategoryId'] as num?)?.toInt(),
      defaultGoalTrackerId: (json['defaultGoalTrackerId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$DefaultGoalTrackerCategoryUpdateModelToJson(
  DefaultGoalTrackerCategoryUpdateModel instance,
) => <String, dynamic>{
  'defaultGoalTrackerCategoryId': instance.defaultGoalTrackerCategoryId,
  'defaultCategoryId': instance.defaultCategoryId,
  'defaultGoalTrackerId': instance.defaultGoalTrackerId,
};

DefaultGoalTrackerCreateModel _$DefaultGoalTrackerCreateModelFromJson(
  Map<String, dynamic> json,
) => DefaultGoalTrackerCreateModel(
  defaultGoalTrackerText: json['defaultGoalTrackerText'] as String?,
);

Map<String, dynamic> _$DefaultGoalTrackerCreateModelToJson(
  DefaultGoalTrackerCreateModel instance,
) => <String, dynamic>{
  'defaultGoalTrackerText': instance.defaultGoalTrackerText,
};

DefaultGoalTrackerModel _$DefaultGoalTrackerModelFromJson(
  Map<String, dynamic> json,
) => DefaultGoalTrackerModel(
  defaultGoalTrackerId: (json['defaultGoalTrackerId'] as num?)?.toInt(),
  defaultGoalTrackerText: json['defaultGoalTrackerText'] as String?,
  isDeleted: json['isDeleted'] as bool?,
  createdDate: json['createdDate'] == null
      ? null
      : DateTime.parse(json['createdDate'] as String),
  createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
  modifiedDate: json['modifiedDate'] == null
      ? null
      : DateTime.parse(json['modifiedDate'] as String),
  modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
  createdByUserFirstName: json['createdByUserFirstName'] as String?,
  createdByUserLastName: json['createdByUserLastName'] as String?,
  createdByUserEmail: json['createdByUserEmail'] as String?,
  modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
  modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
  modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
  createdBy: json['createdBy'] as String?,
  modifiedBy: json['modifiedBy'] as String?,
);

Map<String, dynamic> _$DefaultGoalTrackerModelToJson(
  DefaultGoalTrackerModel instance,
) => <String, dynamic>{
  'defaultGoalTrackerId': instance.defaultGoalTrackerId,
  'defaultGoalTrackerText': instance.defaultGoalTrackerText,
  'isDeleted': instance.isDeleted,
  'createdDate': instance.createdDate?.toIso8601String(),
  'createdByUserId': instance.createdByUserId,
  'modifiedDate': instance.modifiedDate?.toIso8601String(),
  'modifiedByUserId': instance.modifiedByUserId,
  'createdByUserFirstName': instance.createdByUserFirstName,
  'createdByUserLastName': instance.createdByUserLastName,
  'createdByUserEmail': instance.createdByUserEmail,
  'modifiedByUserFirstName': instance.modifiedByUserFirstName,
  'modifiedByUserLastName': instance.modifiedByUserLastName,
  'modifiedByUserEmail': instance.modifiedByUserEmail,
  'createdBy': instance.createdBy,
  'modifiedBy': instance.modifiedBy,
};

DefaultGoalTrackerModelReturnListModel
_$DefaultGoalTrackerModelReturnListModelFromJson(Map<String, dynamic> json) =>
    DefaultGoalTrackerModelReturnListModel(
      model:
          (json['model'] as List<dynamic>?)
              ?.map(
                (e) =>
                    DefaultGoalTrackerModel.fromJson(e as Map<String, dynamic>),
              )
              .toList() ??
          [],
      search: json['search'] == null
          ? null
          : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
      totalRecords: (json['totalRecords'] as num?)?.toInt(),
      isSuccess: json['isSuccess'] as bool?,
      successMessage: json['successMessage'] as String?,
      errorHolder: json['errorHolder'] == null
          ? null
          : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
      activityId: json['activityId'] as String?,
      activityDateTime: json['activityDateTime'] == null
          ? null
          : DateTime.parse(json['activityDateTime'] as String),
      server: json['server'] as String?,
      processId: (json['processId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$DefaultGoalTrackerModelReturnListModelToJson(
  DefaultGoalTrackerModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

DefaultGoalTrackerModelReturnModel _$DefaultGoalTrackerModelReturnModelFromJson(
  Map<String, dynamic> json,
) => DefaultGoalTrackerModelReturnModel(
  model: json['model'] == null
      ? null
      : DefaultGoalTrackerModel.fromJson(json['model'] as Map<String, dynamic>),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$DefaultGoalTrackerModelReturnModelToJson(
  DefaultGoalTrackerModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

DefaultGoalTrackerUpdateModel _$DefaultGoalTrackerUpdateModelFromJson(
  Map<String, dynamic> json,
) => DefaultGoalTrackerUpdateModel(
  defaultGoalTrackerId: (json['defaultGoalTrackerId'] as num?)?.toInt(),
  defaultGoalTrackerText: json['defaultGoalTrackerText'] as String?,
);

Map<String, dynamic> _$DefaultGoalTrackerUpdateModelToJson(
  DefaultGoalTrackerUpdateModel instance,
) => <String, dynamic>{
  'defaultGoalTrackerId': instance.defaultGoalTrackerId,
  'defaultGoalTrackerText': instance.defaultGoalTrackerText,
};

DefaultMeditationFileCreateModel _$DefaultMeditationFileCreateModelFromJson(
  Map<String, dynamic> json,
) => DefaultMeditationFileCreateModel(
  defaultMeditationFileName: json['defaultMeditationFileName'] as String?,
  predefinedMeditationTypeId: (json['predefinedMeditationTypeId'] as num?)
      ?.toInt(),
  location: json['location'] as String?,
  fileName: json['fileName'] as String?,
  fileSize: (json['fileSize'] as num?)?.toInt(),
);

Map<String, dynamic> _$DefaultMeditationFileCreateModelToJson(
  DefaultMeditationFileCreateModel instance,
) => <String, dynamic>{
  'defaultMeditationFileName': instance.defaultMeditationFileName,
  'predefinedMeditationTypeId': instance.predefinedMeditationTypeId,
  'location': instance.location,
  'fileName': instance.fileName,
  'fileSize': instance.fileSize,
};

DefaultMeditationFileModel _$DefaultMeditationFileModelFromJson(
  Map<String, dynamic> json,
) => DefaultMeditationFileModel(
  defaultMeditationFileId: (json['defaultMeditationFileId'] as num?)?.toInt(),
  defaultMeditationFileName: json['defaultMeditationFileName'] as String?,
  predefinedMeditationTypeId: (json['predefinedMeditationTypeId'] as num?)
      ?.toInt(),
  location: json['location'] as String?,
  fileName: json['fileName'] as String?,
  fileSize: (json['fileSize'] as num?)?.toInt(),
  isDeleted: json['isDeleted'] as bool?,
  createdDate: json['createdDate'] == null
      ? null
      : DateTime.parse(json['createdDate'] as String),
  createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
  modifiedDate: json['modifiedDate'] == null
      ? null
      : DateTime.parse(json['modifiedDate'] as String),
  modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
  predefinedMeditationTypeName: json['predefinedMeditationTypeName'] as String?,
  createdByUserFirstName: json['createdByUserFirstName'] as String?,
  createdByUserLastName: json['createdByUserLastName'] as String?,
  createdByUserEmail: json['createdByUserEmail'] as String?,
  modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
  modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
  modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
  createdBy: json['createdBy'] as String?,
  modifiedBy: json['modifiedBy'] as String?,
);

Map<String, dynamic> _$DefaultMeditationFileModelToJson(
  DefaultMeditationFileModel instance,
) => <String, dynamic>{
  'defaultMeditationFileId': instance.defaultMeditationFileId,
  'defaultMeditationFileName': instance.defaultMeditationFileName,
  'predefinedMeditationTypeId': instance.predefinedMeditationTypeId,
  'location': instance.location,
  'fileName': instance.fileName,
  'fileSize': instance.fileSize,
  'isDeleted': instance.isDeleted,
  'createdDate': instance.createdDate?.toIso8601String(),
  'createdByUserId': instance.createdByUserId,
  'modifiedDate': instance.modifiedDate?.toIso8601String(),
  'modifiedByUserId': instance.modifiedByUserId,
  'predefinedMeditationTypeName': instance.predefinedMeditationTypeName,
  'createdByUserFirstName': instance.createdByUserFirstName,
  'createdByUserLastName': instance.createdByUserLastName,
  'createdByUserEmail': instance.createdByUserEmail,
  'modifiedByUserFirstName': instance.modifiedByUserFirstName,
  'modifiedByUserLastName': instance.modifiedByUserLastName,
  'modifiedByUserEmail': instance.modifiedByUserEmail,
  'createdBy': instance.createdBy,
  'modifiedBy': instance.modifiedBy,
};

DefaultMeditationFileModelReturnListModel
_$DefaultMeditationFileModelReturnListModelFromJson(
  Map<String, dynamic> json,
) => DefaultMeditationFileModelReturnListModel(
  model:
      (json['model'] as List<dynamic>?)
          ?.map(
            (e) =>
                DefaultMeditationFileModel.fromJson(e as Map<String, dynamic>),
          )
          .toList() ??
      [],
  search: json['search'] == null
      ? null
      : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
  totalRecords: (json['totalRecords'] as num?)?.toInt(),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$DefaultMeditationFileModelReturnListModelToJson(
  DefaultMeditationFileModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

DefaultMeditationFileModelReturnModel
_$DefaultMeditationFileModelReturnModelFromJson(Map<String, dynamic> json) =>
    DefaultMeditationFileModelReturnModel(
      model: json['model'] == null
          ? null
          : DefaultMeditationFileModel.fromJson(
              json['model'] as Map<String, dynamic>,
            ),
      isSuccess: json['isSuccess'] as bool?,
      successMessage: json['successMessage'] as String?,
      errorHolder: json['errorHolder'] == null
          ? null
          : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
      activityId: json['activityId'] as String?,
      activityDateTime: json['activityDateTime'] == null
          ? null
          : DateTime.parse(json['activityDateTime'] as String),
      server: json['server'] as String?,
      processId: (json['processId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$DefaultMeditationFileModelReturnModelToJson(
  DefaultMeditationFileModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

DefaultMeditationFileUpdateModel _$DefaultMeditationFileUpdateModelFromJson(
  Map<String, dynamic> json,
) => DefaultMeditationFileUpdateModel(
  defaultMeditationFileId: (json['defaultMeditationFileId'] as num?)?.toInt(),
  defaultMeditationFileName: json['defaultMeditationFileName'] as String?,
  predefinedMeditationTypeId: (json['predefinedMeditationTypeId'] as num?)
      ?.toInt(),
  location: json['location'] as String?,
  fileName: json['fileName'] as String?,
  fileSize: (json['fileSize'] as num?)?.toInt(),
);

Map<String, dynamic> _$DefaultMeditationFileUpdateModelToJson(
  DefaultMeditationFileUpdateModel instance,
) => <String, dynamic>{
  'defaultMeditationFileId': instance.defaultMeditationFileId,
  'defaultMeditationFileName': instance.defaultMeditationFileName,
  'predefinedMeditationTypeId': instance.predefinedMeditationTypeId,
  'location': instance.location,
  'fileName': instance.fileName,
  'fileSize': instance.fileSize,
};

DefaultVisionBoardPictureCreateModel
_$DefaultVisionBoardPictureCreateModelFromJson(Map<String, dynamic> json) =>
    DefaultVisionBoardPictureCreateModel(
      title: json['title'] as String?,
      location: json['location'] as String?,
      fileName: json['fileName'] as String?,
      fileSize: (json['fileSize'] as num?)?.toInt(),
    );

Map<String, dynamic> _$DefaultVisionBoardPictureCreateModelToJson(
  DefaultVisionBoardPictureCreateModel instance,
) => <String, dynamic>{
  'title': instance.title,
  'location': instance.location,
  'fileName': instance.fileName,
  'fileSize': instance.fileSize,
};

DefaultVisionBoardPictureModel _$DefaultVisionBoardPictureModelFromJson(
  Map<String, dynamic> json,
) => DefaultVisionBoardPictureModel(
  defaultVisionBoardPictureId: (json['defaultVisionBoardPictureId'] as num?)
      ?.toInt(),
  title: json['title'] as String?,
  location: json['location'] as String?,
  fileName: json['fileName'] as String?,
  fileSize: (json['fileSize'] as num?)?.toInt(),
  isDeleted: json['isDeleted'] as bool?,
  createdDate: json['createdDate'] == null
      ? null
      : DateTime.parse(json['createdDate'] as String),
  createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
  modifiedDate: json['modifiedDate'] == null
      ? null
      : DateTime.parse(json['modifiedDate'] as String),
  modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
  createdByUserFirstName: json['createdByUserFirstName'] as String?,
  createdByUserLastName: json['createdByUserLastName'] as String?,
  createdByUserEmail: json['createdByUserEmail'] as String?,
  modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
  modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
  modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
  createdBy: json['createdBy'] as String?,
  modifiedBy: json['modifiedBy'] as String?,
);

Map<String, dynamic> _$DefaultVisionBoardPictureModelToJson(
  DefaultVisionBoardPictureModel instance,
) => <String, dynamic>{
  'defaultVisionBoardPictureId': instance.defaultVisionBoardPictureId,
  'title': instance.title,
  'location': instance.location,
  'fileName': instance.fileName,
  'fileSize': instance.fileSize,
  'isDeleted': instance.isDeleted,
  'createdDate': instance.createdDate?.toIso8601String(),
  'createdByUserId': instance.createdByUserId,
  'modifiedDate': instance.modifiedDate?.toIso8601String(),
  'modifiedByUserId': instance.modifiedByUserId,
  'createdByUserFirstName': instance.createdByUserFirstName,
  'createdByUserLastName': instance.createdByUserLastName,
  'createdByUserEmail': instance.createdByUserEmail,
  'modifiedByUserFirstName': instance.modifiedByUserFirstName,
  'modifiedByUserLastName': instance.modifiedByUserLastName,
  'modifiedByUserEmail': instance.modifiedByUserEmail,
  'createdBy': instance.createdBy,
  'modifiedBy': instance.modifiedBy,
};

DefaultVisionBoardPictureModelReturnListModel
_$DefaultVisionBoardPictureModelReturnListModelFromJson(
  Map<String, dynamic> json,
) => DefaultVisionBoardPictureModelReturnListModel(
  model:
      (json['model'] as List<dynamic>?)
          ?.map(
            (e) => DefaultVisionBoardPictureModel.fromJson(
              e as Map<String, dynamic>,
            ),
          )
          .toList() ??
      [],
  search: json['search'] == null
      ? null
      : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
  totalRecords: (json['totalRecords'] as num?)?.toInt(),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$DefaultVisionBoardPictureModelReturnListModelToJson(
  DefaultVisionBoardPictureModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

DefaultVisionBoardPictureModelReturnModel
_$DefaultVisionBoardPictureModelReturnModelFromJson(
  Map<String, dynamic> json,
) => DefaultVisionBoardPictureModelReturnModel(
  model: json['model'] == null
      ? null
      : DefaultVisionBoardPictureModel.fromJson(
          json['model'] as Map<String, dynamic>,
        ),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$DefaultVisionBoardPictureModelReturnModelToJson(
  DefaultVisionBoardPictureModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

DefaultVisionBoardPictureUpdateModel
_$DefaultVisionBoardPictureUpdateModelFromJson(Map<String, dynamic> json) =>
    DefaultVisionBoardPictureUpdateModel(
      defaultVisionBoardPictureId: (json['defaultVisionBoardPictureId'] as num?)
          ?.toInt(),
      title: json['title'] as String?,
      location: json['location'] as String?,
      fileName: json['fileName'] as String?,
      fileSize: (json['fileSize'] as num?)?.toInt(),
    );

Map<String, dynamic> _$DefaultVisionBoardPictureUpdateModelToJson(
  DefaultVisionBoardPictureUpdateModel instance,
) => <String, dynamic>{
  'defaultVisionBoardPictureId': instance.defaultVisionBoardPictureId,
  'title': instance.title,
  'location': instance.location,
  'fileName': instance.fileName,
  'fileSize': instance.fileSize,
};

EnablePremiumFeatureModel _$EnablePremiumFeatureModelFromJson(
  Map<String, dynamic> json,
) => EnablePremiumFeatureModel(
  password: json['password'] as String?,
  validityYears: (json['validityYears'] as num?)?.toInt(),
);

Map<String, dynamic> _$EnablePremiumFeatureModelToJson(
  EnablePremiumFeatureModel instance,
) => <String, dynamic>{
  'password': instance.password,
  'validityYears': instance.validityYears,
};

EnumGroupCreateModel _$EnumGroupCreateModelFromJson(
  Map<String, dynamic> json,
) => EnumGroupCreateModel(enumGroupName: json['enumGroupName'] as String?);

Map<String, dynamic> _$EnumGroupCreateModelToJson(
  EnumGroupCreateModel instance,
) => <String, dynamic>{'enumGroupName': instance.enumGroupName};

EnumGroupModel _$EnumGroupModelFromJson(Map<String, dynamic> json) =>
    EnumGroupModel(
      enumGroupId: (json['enumGroupId'] as num?)?.toInt(),
      enumGroupName: json['enumGroupName'] as String?,
      isDeleted: json['isDeleted'] as bool?,
      createdDate: json['createdDate'] == null
          ? null
          : DateTime.parse(json['createdDate'] as String),
      createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
      modifiedDate: json['modifiedDate'] == null
          ? null
          : DateTime.parse(json['modifiedDate'] as String),
      modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
      createdByUserFirstName: json['createdByUserFirstName'] as String?,
      createdByUserLastName: json['createdByUserLastName'] as String?,
      createdByUserEmail: json['createdByUserEmail'] as String?,
      modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
      modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
      modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
      createdBy: json['createdBy'] as String?,
      modifiedBy: json['modifiedBy'] as String?,
    );

Map<String, dynamic> _$EnumGroupModelToJson(EnumGroupModel instance) =>
    <String, dynamic>{
      'enumGroupId': instance.enumGroupId,
      'enumGroupName': instance.enumGroupName,
      'isDeleted': instance.isDeleted,
      'createdDate': instance.createdDate?.toIso8601String(),
      'createdByUserId': instance.createdByUserId,
      'modifiedDate': instance.modifiedDate?.toIso8601String(),
      'modifiedByUserId': instance.modifiedByUserId,
      'createdByUserFirstName': instance.createdByUserFirstName,
      'createdByUserLastName': instance.createdByUserLastName,
      'createdByUserEmail': instance.createdByUserEmail,
      'modifiedByUserFirstName': instance.modifiedByUserFirstName,
      'modifiedByUserLastName': instance.modifiedByUserLastName,
      'modifiedByUserEmail': instance.modifiedByUserEmail,
      'createdBy': instance.createdBy,
      'modifiedBy': instance.modifiedBy,
    };

EnumGroupModelReturnListModel _$EnumGroupModelReturnListModelFromJson(
  Map<String, dynamic> json,
) => EnumGroupModelReturnListModel(
  model:
      (json['model'] as List<dynamic>?)
          ?.map((e) => EnumGroupModel.fromJson(e as Map<String, dynamic>))
          .toList() ??
      [],
  search: json['search'] == null
      ? null
      : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
  totalRecords: (json['totalRecords'] as num?)?.toInt(),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$EnumGroupModelReturnListModelToJson(
  EnumGroupModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

EnumGroupModelReturnModel _$EnumGroupModelReturnModelFromJson(
  Map<String, dynamic> json,
) => EnumGroupModelReturnModel(
  model: json['model'] == null
      ? null
      : EnumGroupModel.fromJson(json['model'] as Map<String, dynamic>),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$EnumGroupModelReturnModelToJson(
  EnumGroupModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

EnumGroupUpdateModel _$EnumGroupUpdateModelFromJson(
  Map<String, dynamic> json,
) => EnumGroupUpdateModel(
  enumGroupId: (json['enumGroupId'] as num?)?.toInt(),
  enumGroupName: json['enumGroupName'] as String?,
);

Map<String, dynamic> _$EnumGroupUpdateModelToJson(
  EnumGroupUpdateModel instance,
) => <String, dynamic>{
  'enumGroupId': instance.enumGroupId,
  'enumGroupName': instance.enumGroupName,
};

EnumTypeCreateModel _$EnumTypeCreateModelFromJson(Map<String, dynamic> json) =>
    EnumTypeCreateModel(
      internalValue: (json['internalValue'] as num?)?.toInt(),
      enumGroupId: (json['enumGroupId'] as num?)?.toInt(),
      parentEnumTypeId: (json['parentEnumTypeId'] as num?)?.toInt(),
      enumTypeName: json['enumTypeName'] as String?,
    );

Map<String, dynamic> _$EnumTypeCreateModelToJson(
  EnumTypeCreateModel instance,
) => <String, dynamic>{
  'internalValue': instance.internalValue,
  'enumGroupId': instance.enumGroupId,
  'parentEnumTypeId': instance.parentEnumTypeId,
  'enumTypeName': instance.enumTypeName,
};

EnumTypeModel _$EnumTypeModelFromJson(Map<String, dynamic> json) =>
    EnumTypeModel(
      enumTypeId: (json['enumTypeId'] as num?)?.toInt(),
      internalValue: (json['internalValue'] as num?)?.toInt(),
      enumGroupId: (json['enumGroupId'] as num?)?.toInt(),
      parentEnumTypeId: (json['parentEnumTypeId'] as num?)?.toInt(),
      enumTypeName: json['enumTypeName'] as String?,
      isDeleted: json['isDeleted'] as bool?,
      createdDate: json['createdDate'] == null
          ? null
          : DateTime.parse(json['createdDate'] as String),
      createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
      modifiedDate: json['modifiedDate'] == null
          ? null
          : DateTime.parse(json['modifiedDate'] as String),
      modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
      enumGroupName: json['enumGroupName'] as String?,
      createdByUserFirstName: json['createdByUserFirstName'] as String?,
      createdByUserLastName: json['createdByUserLastName'] as String?,
      createdByUserEmail: json['createdByUserEmail'] as String?,
      modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
      modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
      modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
      createdBy: json['createdBy'] as String?,
      modifiedBy: json['modifiedBy'] as String?,
    );

Map<String, dynamic> _$EnumTypeModelToJson(EnumTypeModel instance) =>
    <String, dynamic>{
      'enumTypeId': instance.enumTypeId,
      'internalValue': instance.internalValue,
      'enumGroupId': instance.enumGroupId,
      'parentEnumTypeId': instance.parentEnumTypeId,
      'enumTypeName': instance.enumTypeName,
      'isDeleted': instance.isDeleted,
      'createdDate': instance.createdDate?.toIso8601String(),
      'createdByUserId': instance.createdByUserId,
      'modifiedDate': instance.modifiedDate?.toIso8601String(),
      'modifiedByUserId': instance.modifiedByUserId,
      'enumGroupName': instance.enumGroupName,
      'createdByUserFirstName': instance.createdByUserFirstName,
      'createdByUserLastName': instance.createdByUserLastName,
      'createdByUserEmail': instance.createdByUserEmail,
      'modifiedByUserFirstName': instance.modifiedByUserFirstName,
      'modifiedByUserLastName': instance.modifiedByUserLastName,
      'modifiedByUserEmail': instance.modifiedByUserEmail,
      'createdBy': instance.createdBy,
      'modifiedBy': instance.modifiedBy,
    };

EnumTypeModelReturnListModel _$EnumTypeModelReturnListModelFromJson(
  Map<String, dynamic> json,
) => EnumTypeModelReturnListModel(
  model:
      (json['model'] as List<dynamic>?)
          ?.map((e) => EnumTypeModel.fromJson(e as Map<String, dynamic>))
          .toList() ??
      [],
  search: json['search'] == null
      ? null
      : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
  totalRecords: (json['totalRecords'] as num?)?.toInt(),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$EnumTypeModelReturnListModelToJson(
  EnumTypeModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

EnumTypeModelReturnModel _$EnumTypeModelReturnModelFromJson(
  Map<String, dynamic> json,
) => EnumTypeModelReturnModel(
  model: json['model'] == null
      ? null
      : EnumTypeModel.fromJson(json['model'] as Map<String, dynamic>),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$EnumTypeModelReturnModelToJson(
  EnumTypeModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

EnumTypeUpdateModel _$EnumTypeUpdateModelFromJson(Map<String, dynamic> json) =>
    EnumTypeUpdateModel(
      enumTypeId: (json['enumTypeId'] as num?)?.toInt(),
      internalValue: (json['internalValue'] as num?)?.toInt(),
      enumGroupId: (json['enumGroupId'] as num?)?.toInt(),
      parentEnumTypeId: (json['parentEnumTypeId'] as num?)?.toInt(),
      enumTypeName: json['enumTypeName'] as String?,
    );

Map<String, dynamic> _$EnumTypeUpdateModelToJson(
  EnumTypeUpdateModel instance,
) => <String, dynamic>{
  'enumTypeId': instance.enumTypeId,
  'internalValue': instance.internalValue,
  'enumGroupId': instance.enumGroupId,
  'parentEnumTypeId': instance.parentEnumTypeId,
  'enumTypeName': instance.enumTypeName,
};

ExternalLoginCallBackModel _$ExternalLoginCallBackModelFromJson(
  Map<String, dynamic> json,
) => ExternalLoginCallBackModel(
  provider: json['provider'] as String?,
  callBackUri: json['callBackUri'] as String?,
  singleSignOnIdentifier: json['singleSignOnIdentifier'] as String?,
  code: json['code'] as String?,
  idToken: json['id_token'] as String?,
  accessToken: json['access_token'] as String?,
  error: json['error'] as String?,
  state: json['state'] as String?,
);

Map<String, dynamic> _$ExternalLoginCallBackModelToJson(
  ExternalLoginCallBackModel instance,
) => <String, dynamic>{
  'provider': instance.provider,
  'callBackUri': instance.callBackUri,
  'singleSignOnIdentifier': instance.singleSignOnIdentifier,
  'code': instance.code,
  'id_token': instance.idToken,
  'access_token': instance.accessToken,
  'error': instance.error,
  'state': instance.state,
};

FeedByDailyAffirmationSessionIdSearchCriteria
_$FeedByDailyAffirmationSessionIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => FeedByDailyAffirmationSessionIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  dailyAffirmationSessionId: (json['dailyAffirmationSessionId'] as num?)
      ?.toInt(),
);

Map<String, dynamic> _$FeedByDailyAffirmationSessionIdSearchCriteriaToJson(
  FeedByDailyAffirmationSessionIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'dailyAffirmationSessionId': instance.dailyAffirmationSessionId,
};

FeedByGoalTrackerIdSearchCriteria _$FeedByGoalTrackerIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => FeedByGoalTrackerIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  goalTrackerId: (json['goalTrackerId'] as num?)?.toInt(),
);

Map<String, dynamic> _$FeedByGoalTrackerIdSearchCriteriaToJson(
  FeedByGoalTrackerIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'goalTrackerId': instance.goalTrackerId,
};

FeedByGoalTrackerMileStoneIdSearchCriteria
_$FeedByGoalTrackerMileStoneIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => FeedByGoalTrackerMileStoneIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  goalTrackerMileStoneId: (json['goalTrackerMileStoneId'] as num?)?.toInt(),
);

Map<String, dynamic> _$FeedByGoalTrackerMileStoneIdSearchCriteriaToJson(
  FeedByGoalTrackerMileStoneIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'goalTrackerMileStoneId': instance.goalTrackerMileStoneId,
};

FeedByMeditationSessionIdSearchCriteria
_$FeedByMeditationSessionIdSearchCriteriaFromJson(Map<String, dynamic> json) =>
    FeedByMeditationSessionIdSearchCriteria(
      selectedId: (json['selectedId'] as num?)?.toInt(),
      page: (json['page'] as num?)?.toInt(),
      pageSize: (json['pageSize'] as num?)?.toInt(),
      keyword: json['keyword'] as String?,
      sortBy: json['sortBy'] as String?,
      sortAscending: json['sortAscending'] as bool?,
      meditationSessionId: (json['meditationSessionId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$FeedByMeditationSessionIdSearchCriteriaToJson(
  FeedByMeditationSessionIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'meditationSessionId': instance.meditationSessionId,
};

FeedByUserIdSearchCriteria _$FeedByUserIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => FeedByUserIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  userId: (json['userId'] as num?)?.toInt(),
);

Map<String, dynamic> _$FeedByUserIdSearchCriteriaToJson(
  FeedByUserIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'userId': instance.userId,
};

FeedByVisionBoardSessionIdSearchCriteria
_$FeedByVisionBoardSessionIdSearchCriteriaFromJson(Map<String, dynamic> json) =>
    FeedByVisionBoardSessionIdSearchCriteria(
      selectedId: (json['selectedId'] as num?)?.toInt(),
      page: (json['page'] as num?)?.toInt(),
      pageSize: (json['pageSize'] as num?)?.toInt(),
      keyword: json['keyword'] as String?,
      sortBy: json['sortBy'] as String?,
      sortAscending: json['sortAscending'] as bool?,
      visionBoardSessionId: (json['visionBoardSessionId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$FeedByVisionBoardSessionIdSearchCriteriaToJson(
  FeedByVisionBoardSessionIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'visionBoardSessionId': instance.visionBoardSessionId,
};

FeedCreateModel _$FeedCreateModelFromJson(Map<String, dynamic> json) =>
    FeedCreateModel(
      feedGuid: json['feedGuid'] as String?,
      userId: (json['userId'] as num?)?.toInt(),
      dailyAffirmationSessionId: (json['dailyAffirmationSessionId'] as num?)
          ?.toInt(),
      dailyAffirmationSessionDate: json['dailyAffirmationSessionDate'] == null
          ? null
          : DateTime.parse(json['dailyAffirmationSessionDate'] as String),
      dailyAffirmationTimeSpentInSeconds:
          (json['dailyAffirmationTimeSpentInSeconds'] as num?)?.toInt(),
      visionBoardSessionId: (json['visionBoardSessionId'] as num?)?.toInt(),
      visionBoardSessionDate: json['visionBoardSessionDate'] == null
          ? null
          : DateTime.parse(json['visionBoardSessionDate'] as String),
      visionBoardSessionTimeSpentInSeconds:
          (json['visionBoardSessionTimeSpentInSeconds'] as num?)?.toInt(),
      meditationSessionId: (json['meditationSessionId'] as num?)?.toInt(),
      meditationSessionDate: json['meditationSessionDate'] == null
          ? null
          : DateTime.parse(json['meditationSessionDate'] as String),
      meditationSessionTimeSpentInSeconds:
          (json['meditationSessionTimeSpentInSeconds'] as num?)?.toInt(),
      meditationFileName: json['meditationFileName'] as String?,
      predefinedMeditationTypeId: (json['predefinedMeditationTypeId'] as num?)
          ?.toInt(),
      goalTrackerId: (json['goalTrackerId'] as num?)?.toInt(),
      goalTrackerName: json['goalTrackerName'] as String?,
      predefinedGoalStatusTypeId: (json['predefinedGoalStatusTypeId'] as num?)
          ?.toInt(),
      goalTrackerStartDate: json['goalTrackerStartDate'] == null
          ? null
          : DateTime.parse(json['goalTrackerStartDate'] as String),
      goalTrackerEndDate: json['goalTrackerEndDate'] == null
          ? null
          : DateTime.parse(json['goalTrackerEndDate'] as String),
      goalTrackerCompletedDate: json['goalTrackerCompletedDate'] == null
          ? null
          : DateTime.parse(json['goalTrackerCompletedDate'] as String),
      goalTrackerMileStoneId: (json['goalTrackerMileStoneId'] as num?)?.toInt(),
      goalTrackerMileStoneName: json['goalTrackerMileStoneName'] as String?,
      goalTrackerMileStoneStartDate:
          json['goalTrackerMileStoneStartDate'] == null
          ? null
          : DateTime.parse(json['goalTrackerMileStoneStartDate'] as String),
      goalTrackerMileStoneEndDate: json['goalTrackerMileStoneEndDate'] == null
          ? null
          : DateTime.parse(json['goalTrackerMileStoneEndDate'] as String),
      goalTrackerMileStoneCompletedDate:
          json['goalTrackerMileStoneCompletedDate'] == null
          ? null
          : DateTime.parse(json['goalTrackerMileStoneCompletedDate'] as String),
    );

Map<String, dynamic> _$FeedCreateModelToJson(
  FeedCreateModel instance,
) => <String, dynamic>{
  'feedGuid': instance.feedGuid,
  'userId': instance.userId,
  'dailyAffirmationSessionId': instance.dailyAffirmationSessionId,
  'dailyAffirmationSessionDate': instance.dailyAffirmationSessionDate
      ?.toIso8601String(),
  'dailyAffirmationTimeSpentInSeconds':
      instance.dailyAffirmationTimeSpentInSeconds,
  'visionBoardSessionId': instance.visionBoardSessionId,
  'visionBoardSessionDate': instance.visionBoardSessionDate?.toIso8601String(),
  'visionBoardSessionTimeSpentInSeconds':
      instance.visionBoardSessionTimeSpentInSeconds,
  'meditationSessionId': instance.meditationSessionId,
  'meditationSessionDate': instance.meditationSessionDate?.toIso8601String(),
  'meditationSessionTimeSpentInSeconds':
      instance.meditationSessionTimeSpentInSeconds,
  'meditationFileName': instance.meditationFileName,
  'predefinedMeditationTypeId': instance.predefinedMeditationTypeId,
  'goalTrackerId': instance.goalTrackerId,
  'goalTrackerName': instance.goalTrackerName,
  'predefinedGoalStatusTypeId': instance.predefinedGoalStatusTypeId,
  'goalTrackerStartDate': instance.goalTrackerStartDate?.toIso8601String(),
  'goalTrackerEndDate': instance.goalTrackerEndDate?.toIso8601String(),
  'goalTrackerCompletedDate': instance.goalTrackerCompletedDate
      ?.toIso8601String(),
  'goalTrackerMileStoneId': instance.goalTrackerMileStoneId,
  'goalTrackerMileStoneName': instance.goalTrackerMileStoneName,
  'goalTrackerMileStoneStartDate': instance.goalTrackerMileStoneStartDate
      ?.toIso8601String(),
  'goalTrackerMileStoneEndDate': instance.goalTrackerMileStoneEndDate
      ?.toIso8601String(),
  'goalTrackerMileStoneCompletedDate': instance
      .goalTrackerMileStoneCompletedDate
      ?.toIso8601String(),
};

FeedModel _$FeedModelFromJson(Map<String, dynamic> json) => FeedModel(
  feedId: (json['feedId'] as num?)?.toInt(),
  feedGuid: json['feedGuid'] as String?,
  userId: (json['userId'] as num?)?.toInt(),
  dailyAffirmationSessionId: (json['dailyAffirmationSessionId'] as num?)
      ?.toInt(),
  dailyAffirmationSessionDate: json['dailyAffirmationSessionDate'] == null
      ? null
      : DateTime.parse(json['dailyAffirmationSessionDate'] as String),
  dailyAffirmationTimeSpentInSeconds:
      (json['dailyAffirmationTimeSpentInSeconds'] as num?)?.toInt(),
  visionBoardSessionId: (json['visionBoardSessionId'] as num?)?.toInt(),
  visionBoardSessionDate: json['visionBoardSessionDate'] == null
      ? null
      : DateTime.parse(json['visionBoardSessionDate'] as String),
  visionBoardSessionTimeSpentInSeconds:
      (json['visionBoardSessionTimeSpentInSeconds'] as num?)?.toInt(),
  meditationSessionId: (json['meditationSessionId'] as num?)?.toInt(),
  meditationSessionDate: json['meditationSessionDate'] == null
      ? null
      : DateTime.parse(json['meditationSessionDate'] as String),
  meditationSessionTimeSpentInSeconds:
      (json['meditationSessionTimeSpentInSeconds'] as num?)?.toInt(),
  meditationFileName: json['meditationFileName'] as String?,
  predefinedMeditationTypeId: (json['predefinedMeditationTypeId'] as num?)
      ?.toInt(),
  goalTrackerId: (json['goalTrackerId'] as num?)?.toInt(),
  goalTrackerName: json['goalTrackerName'] as String?,
  predefinedGoalStatusTypeId: (json['predefinedGoalStatusTypeId'] as num?)
      ?.toInt(),
  goalTrackerStartDate: json['goalTrackerStartDate'] == null
      ? null
      : DateTime.parse(json['goalTrackerStartDate'] as String),
  goalTrackerEndDate: json['goalTrackerEndDate'] == null
      ? null
      : DateTime.parse(json['goalTrackerEndDate'] as String),
  goalTrackerCompletedDate: json['goalTrackerCompletedDate'] == null
      ? null
      : DateTime.parse(json['goalTrackerCompletedDate'] as String),
  goalTrackerMileStoneId: (json['goalTrackerMileStoneId'] as num?)?.toInt(),
  goalTrackerMileStoneName: json['goalTrackerMileStoneName'] as String?,
  goalTrackerMileStoneStartDate: json['goalTrackerMileStoneStartDate'] == null
      ? null
      : DateTime.parse(json['goalTrackerMileStoneStartDate'] as String),
  goalTrackerMileStoneEndDate: json['goalTrackerMileStoneEndDate'] == null
      ? null
      : DateTime.parse(json['goalTrackerMileStoneEndDate'] as String),
  goalTrackerMileStoneCompletedDate:
      json['goalTrackerMileStoneCompletedDate'] == null
      ? null
      : DateTime.parse(json['goalTrackerMileStoneCompletedDate'] as String),
  isDeleted: json['isDeleted'] as bool?,
  createdDate: json['createdDate'] == null
      ? null
      : DateTime.parse(json['createdDate'] as String),
  createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
  modifiedDate: json['modifiedDate'] == null
      ? null
      : DateTime.parse(json['modifiedDate'] as String),
  modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
  userName: json['userName'] as String?,
  predefinedMeditationTypeName: json['predefinedMeditationTypeName'] as String?,
  predefinedGoalStatusTypeName: json['predefinedGoalStatusTypeName'] as String?,
  createdByUserFirstName: json['createdByUserFirstName'] as String?,
  createdByUserLastName: json['createdByUserLastName'] as String?,
  createdByUserEmail: json['createdByUserEmail'] as String?,
  modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
  modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
  modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
  createdBy: json['createdBy'] as String?,
  modifiedBy: json['modifiedBy'] as String?,
);

Map<String, dynamic> _$FeedModelToJson(FeedModel instance) => <String, dynamic>{
  'feedId': instance.feedId,
  'feedGuid': instance.feedGuid,
  'userId': instance.userId,
  'dailyAffirmationSessionId': instance.dailyAffirmationSessionId,
  'dailyAffirmationSessionDate': instance.dailyAffirmationSessionDate
      ?.toIso8601String(),
  'dailyAffirmationTimeSpentInSeconds':
      instance.dailyAffirmationTimeSpentInSeconds,
  'visionBoardSessionId': instance.visionBoardSessionId,
  'visionBoardSessionDate': instance.visionBoardSessionDate?.toIso8601String(),
  'visionBoardSessionTimeSpentInSeconds':
      instance.visionBoardSessionTimeSpentInSeconds,
  'meditationSessionId': instance.meditationSessionId,
  'meditationSessionDate': instance.meditationSessionDate?.toIso8601String(),
  'meditationSessionTimeSpentInSeconds':
      instance.meditationSessionTimeSpentInSeconds,
  'meditationFileName': instance.meditationFileName,
  'predefinedMeditationTypeId': instance.predefinedMeditationTypeId,
  'goalTrackerId': instance.goalTrackerId,
  'goalTrackerName': instance.goalTrackerName,
  'predefinedGoalStatusTypeId': instance.predefinedGoalStatusTypeId,
  'goalTrackerStartDate': instance.goalTrackerStartDate?.toIso8601String(),
  'goalTrackerEndDate': instance.goalTrackerEndDate?.toIso8601String(),
  'goalTrackerCompletedDate': instance.goalTrackerCompletedDate
      ?.toIso8601String(),
  'goalTrackerMileStoneId': instance.goalTrackerMileStoneId,
  'goalTrackerMileStoneName': instance.goalTrackerMileStoneName,
  'goalTrackerMileStoneStartDate': instance.goalTrackerMileStoneStartDate
      ?.toIso8601String(),
  'goalTrackerMileStoneEndDate': instance.goalTrackerMileStoneEndDate
      ?.toIso8601String(),
  'goalTrackerMileStoneCompletedDate': instance
      .goalTrackerMileStoneCompletedDate
      ?.toIso8601String(),
  'isDeleted': instance.isDeleted,
  'createdDate': instance.createdDate?.toIso8601String(),
  'createdByUserId': instance.createdByUserId,
  'modifiedDate': instance.modifiedDate?.toIso8601String(),
  'modifiedByUserId': instance.modifiedByUserId,
  'userName': instance.userName,
  'predefinedMeditationTypeName': instance.predefinedMeditationTypeName,
  'predefinedGoalStatusTypeName': instance.predefinedGoalStatusTypeName,
  'createdByUserFirstName': instance.createdByUserFirstName,
  'createdByUserLastName': instance.createdByUserLastName,
  'createdByUserEmail': instance.createdByUserEmail,
  'modifiedByUserFirstName': instance.modifiedByUserFirstName,
  'modifiedByUserLastName': instance.modifiedByUserLastName,
  'modifiedByUserEmail': instance.modifiedByUserEmail,
  'createdBy': instance.createdBy,
  'modifiedBy': instance.modifiedBy,
};

FeedModelReturnListModel _$FeedModelReturnListModelFromJson(
  Map<String, dynamic> json,
) => FeedModelReturnListModel(
  model:
      (json['model'] as List<dynamic>?)
          ?.map((e) => FeedModel.fromJson(e as Map<String, dynamic>))
          .toList() ??
      [],
  search: json['search'] == null
      ? null
      : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
  totalRecords: (json['totalRecords'] as num?)?.toInt(),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$FeedModelReturnListModelToJson(
  FeedModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

FeedModelReturnModel _$FeedModelReturnModelFromJson(
  Map<String, dynamic> json,
) => FeedModelReturnModel(
  model: json['model'] == null
      ? null
      : FeedModel.fromJson(json['model'] as Map<String, dynamic>),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$FeedModelReturnModelToJson(
  FeedModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

FeedUpdateModel _$FeedUpdateModelFromJson(Map<String, dynamic> json) =>
    FeedUpdateModel(
      feedId: (json['feedId'] as num?)?.toInt(),
      feedGuid: json['feedGuid'] as String?,
      userId: (json['userId'] as num?)?.toInt(),
      dailyAffirmationSessionId: (json['dailyAffirmationSessionId'] as num?)
          ?.toInt(),
      dailyAffirmationSessionDate: json['dailyAffirmationSessionDate'] == null
          ? null
          : DateTime.parse(json['dailyAffirmationSessionDate'] as String),
      dailyAffirmationTimeSpentInSeconds:
          (json['dailyAffirmationTimeSpentInSeconds'] as num?)?.toInt(),
      visionBoardSessionId: (json['visionBoardSessionId'] as num?)?.toInt(),
      visionBoardSessionDate: json['visionBoardSessionDate'] == null
          ? null
          : DateTime.parse(json['visionBoardSessionDate'] as String),
      visionBoardSessionTimeSpentInSeconds:
          (json['visionBoardSessionTimeSpentInSeconds'] as num?)?.toInt(),
      meditationSessionId: (json['meditationSessionId'] as num?)?.toInt(),
      meditationSessionDate: json['meditationSessionDate'] == null
          ? null
          : DateTime.parse(json['meditationSessionDate'] as String),
      meditationSessionTimeSpentInSeconds:
          (json['meditationSessionTimeSpentInSeconds'] as num?)?.toInt(),
      meditationFileName: json['meditationFileName'] as String?,
      predefinedMeditationTypeId: (json['predefinedMeditationTypeId'] as num?)
          ?.toInt(),
      goalTrackerId: (json['goalTrackerId'] as num?)?.toInt(),
      goalTrackerName: json['goalTrackerName'] as String?,
      predefinedGoalStatusTypeId: (json['predefinedGoalStatusTypeId'] as num?)
          ?.toInt(),
      goalTrackerStartDate: json['goalTrackerStartDate'] == null
          ? null
          : DateTime.parse(json['goalTrackerStartDate'] as String),
      goalTrackerEndDate: json['goalTrackerEndDate'] == null
          ? null
          : DateTime.parse(json['goalTrackerEndDate'] as String),
      goalTrackerCompletedDate: json['goalTrackerCompletedDate'] == null
          ? null
          : DateTime.parse(json['goalTrackerCompletedDate'] as String),
      goalTrackerMileStoneId: (json['goalTrackerMileStoneId'] as num?)?.toInt(),
      goalTrackerMileStoneName: json['goalTrackerMileStoneName'] as String?,
      goalTrackerMileStoneStartDate:
          json['goalTrackerMileStoneStartDate'] == null
          ? null
          : DateTime.parse(json['goalTrackerMileStoneStartDate'] as String),
      goalTrackerMileStoneEndDate: json['goalTrackerMileStoneEndDate'] == null
          ? null
          : DateTime.parse(json['goalTrackerMileStoneEndDate'] as String),
      goalTrackerMileStoneCompletedDate:
          json['goalTrackerMileStoneCompletedDate'] == null
          ? null
          : DateTime.parse(json['goalTrackerMileStoneCompletedDate'] as String),
    );

Map<String, dynamic> _$FeedUpdateModelToJson(
  FeedUpdateModel instance,
) => <String, dynamic>{
  'feedId': instance.feedId,
  'feedGuid': instance.feedGuid,
  'userId': instance.userId,
  'dailyAffirmationSessionId': instance.dailyAffirmationSessionId,
  'dailyAffirmationSessionDate': instance.dailyAffirmationSessionDate
      ?.toIso8601String(),
  'dailyAffirmationTimeSpentInSeconds':
      instance.dailyAffirmationTimeSpentInSeconds,
  'visionBoardSessionId': instance.visionBoardSessionId,
  'visionBoardSessionDate': instance.visionBoardSessionDate?.toIso8601String(),
  'visionBoardSessionTimeSpentInSeconds':
      instance.visionBoardSessionTimeSpentInSeconds,
  'meditationSessionId': instance.meditationSessionId,
  'meditationSessionDate': instance.meditationSessionDate?.toIso8601String(),
  'meditationSessionTimeSpentInSeconds':
      instance.meditationSessionTimeSpentInSeconds,
  'meditationFileName': instance.meditationFileName,
  'predefinedMeditationTypeId': instance.predefinedMeditationTypeId,
  'goalTrackerId': instance.goalTrackerId,
  'goalTrackerName': instance.goalTrackerName,
  'predefinedGoalStatusTypeId': instance.predefinedGoalStatusTypeId,
  'goalTrackerStartDate': instance.goalTrackerStartDate?.toIso8601String(),
  'goalTrackerEndDate': instance.goalTrackerEndDate?.toIso8601String(),
  'goalTrackerCompletedDate': instance.goalTrackerCompletedDate
      ?.toIso8601String(),
  'goalTrackerMileStoneId': instance.goalTrackerMileStoneId,
  'goalTrackerMileStoneName': instance.goalTrackerMileStoneName,
  'goalTrackerMileStoneStartDate': instance.goalTrackerMileStoneStartDate
      ?.toIso8601String(),
  'goalTrackerMileStoneEndDate': instance.goalTrackerMileStoneEndDate
      ?.toIso8601String(),
  'goalTrackerMileStoneCompletedDate': instance
      .goalTrackerMileStoneCompletedDate
      ?.toIso8601String(),
};

FileAttachmentFolderCreateModel _$FileAttachmentFolderCreateModelFromJson(
  Map<String, dynamic> json,
) => FileAttachmentFolderCreateModel(
  fileAttachmentFolderName: json['fileAttachmentFolderName'] as String?,
  parentFileAttachmentFolderId: (json['parentFileAttachmentFolderId'] as num?)
      ?.toInt(),
  description: json['description'] as String?,
  userId: (json['userId'] as num?)?.toInt(),
);

Map<String, dynamic> _$FileAttachmentFolderCreateModelToJson(
  FileAttachmentFolderCreateModel instance,
) => <String, dynamic>{
  'fileAttachmentFolderName': instance.fileAttachmentFolderName,
  'parentFileAttachmentFolderId': instance.parentFileAttachmentFolderId,
  'description': instance.description,
  'userId': instance.userId,
};

FileAttachmentFolderModel _$FileAttachmentFolderModelFromJson(
  Map<String, dynamic> json,
) => FileAttachmentFolderModel(
  fileAttachmentFolderId: (json['fileAttachmentFolderId'] as num?)?.toInt(),
  fileAttachmentFolderName: json['fileAttachmentFolderName'] as String?,
  parentFileAttachmentFolderId: (json['parentFileAttachmentFolderId'] as num?)
      ?.toInt(),
  description: json['description'] as String?,
  userId: (json['userId'] as num?)?.toInt(),
  isDeleted: json['isDeleted'] as bool?,
  createdDate: json['createdDate'] == null
      ? null
      : DateTime.parse(json['createdDate'] as String),
  createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
  modifiedDate: json['modifiedDate'] == null
      ? null
      : DateTime.parse(json['modifiedDate'] as String),
  modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
  parentFileAttachmentFolderName:
      json['parentFileAttachmentFolderName'] as String?,
  userName: json['userName'] as String?,
  createdByUserFirstName: json['createdByUserFirstName'] as String?,
  createdByUserLastName: json['createdByUserLastName'] as String?,
  createdByUserEmail: json['createdByUserEmail'] as String?,
  modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
  modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
  modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
  createdBy: json['createdBy'] as String?,
  modifiedBy: json['modifiedBy'] as String?,
);

Map<String, dynamic> _$FileAttachmentFolderModelToJson(
  FileAttachmentFolderModel instance,
) => <String, dynamic>{
  'fileAttachmentFolderId': instance.fileAttachmentFolderId,
  'fileAttachmentFolderName': instance.fileAttachmentFolderName,
  'parentFileAttachmentFolderId': instance.parentFileAttachmentFolderId,
  'description': instance.description,
  'userId': instance.userId,
  'isDeleted': instance.isDeleted,
  'createdDate': instance.createdDate?.toIso8601String(),
  'createdByUserId': instance.createdByUserId,
  'modifiedDate': instance.modifiedDate?.toIso8601String(),
  'modifiedByUserId': instance.modifiedByUserId,
  'parentFileAttachmentFolderName': instance.parentFileAttachmentFolderName,
  'userName': instance.userName,
  'createdByUserFirstName': instance.createdByUserFirstName,
  'createdByUserLastName': instance.createdByUserLastName,
  'createdByUserEmail': instance.createdByUserEmail,
  'modifiedByUserFirstName': instance.modifiedByUserFirstName,
  'modifiedByUserLastName': instance.modifiedByUserLastName,
  'modifiedByUserEmail': instance.modifiedByUserEmail,
  'createdBy': instance.createdBy,
  'modifiedBy': instance.modifiedBy,
};

FileAttachmentFolderModelReturnListModel
_$FileAttachmentFolderModelReturnListModelFromJson(Map<String, dynamic> json) =>
    FileAttachmentFolderModelReturnListModel(
      model:
          (json['model'] as List<dynamic>?)
              ?.map(
                (e) => FileAttachmentFolderModel.fromJson(
                  e as Map<String, dynamic>,
                ),
              )
              .toList() ??
          [],
      search: json['search'] == null
          ? null
          : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
      totalRecords: (json['totalRecords'] as num?)?.toInt(),
      isSuccess: json['isSuccess'] as bool?,
      successMessage: json['successMessage'] as String?,
      errorHolder: json['errorHolder'] == null
          ? null
          : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
      activityId: json['activityId'] as String?,
      activityDateTime: json['activityDateTime'] == null
          ? null
          : DateTime.parse(json['activityDateTime'] as String),
      server: json['server'] as String?,
      processId: (json['processId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$FileAttachmentFolderModelReturnListModelToJson(
  FileAttachmentFolderModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

FileAttachmentFolderModelReturnModel
_$FileAttachmentFolderModelReturnModelFromJson(Map<String, dynamic> json) =>
    FileAttachmentFolderModelReturnModel(
      model: json['model'] == null
          ? null
          : FileAttachmentFolderModel.fromJson(
              json['model'] as Map<String, dynamic>,
            ),
      isSuccess: json['isSuccess'] as bool?,
      successMessage: json['successMessage'] as String?,
      errorHolder: json['errorHolder'] == null
          ? null
          : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
      activityId: json['activityId'] as String?,
      activityDateTime: json['activityDateTime'] == null
          ? null
          : DateTime.parse(json['activityDateTime'] as String),
      server: json['server'] as String?,
      processId: (json['processId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$FileAttachmentFolderModelReturnModelToJson(
  FileAttachmentFolderModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

FileAttachmentFolderUpdateModel _$FileAttachmentFolderUpdateModelFromJson(
  Map<String, dynamic> json,
) => FileAttachmentFolderUpdateModel(
  fileAttachmentFolderId: (json['fileAttachmentFolderId'] as num?)?.toInt(),
  fileAttachmentFolderName: json['fileAttachmentFolderName'] as String?,
  parentFileAttachmentFolderId: (json['parentFileAttachmentFolderId'] as num?)
      ?.toInt(),
  description: json['description'] as String?,
  userId: (json['userId'] as num?)?.toInt(),
);

Map<String, dynamic> _$FileAttachmentFolderUpdateModelToJson(
  FileAttachmentFolderUpdateModel instance,
) => <String, dynamic>{
  'fileAttachmentFolderId': instance.fileAttachmentFolderId,
  'fileAttachmentFolderName': instance.fileAttachmentFolderName,
  'parentFileAttachmentFolderId': instance.parentFileAttachmentFolderId,
  'description': instance.description,
  'userId': instance.userId,
};

FileAttachmentModel _$FileAttachmentModelFromJson(Map<String, dynamic> json) =>
    FileAttachmentModel(
      fileAttachmentId: (json['fileAttachmentId'] as num?)?.toInt(),
      fileAttachmentFolderId: (json['fileAttachmentFolderId'] as num?)?.toInt(),
      fileAttachmentFileName: json['fileAttachmentFileName'] as String?,
      fileAttachmentFileLocation: json['fileAttachmentFileLocation'] as String?,
      fileAttachmentPreviewFileLocation:
          json['fileAttachmentPreviewFileLocation'] as String?,
      fileAttachmentHasPreview: json['fileAttachmentHasPreview'] as bool?,
      fileAttachmentFileSizeInBytes:
          (json['fileAttachmentFileSizeInBytes'] as num?)?.toInt(),
      description: json['description'] as String?,
      userId: (json['userId'] as num?)?.toInt(),
      isDeleted: json['isDeleted'] as bool?,
      createdDate: json['createdDate'] == null
          ? null
          : DateTime.parse(json['createdDate'] as String),
      createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
      modifiedDate: json['modifiedDate'] == null
          ? null
          : DateTime.parse(json['modifiedDate'] as String),
      modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
      createdByUserFirstName: json['createdByUserFirstName'] as String?,
      createdByUserLastName: json['createdByUserLastName'] as String?,
      createdByUserEmail: json['createdByUserEmail'] as String?,
      modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
      modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
      modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
      createdBy: json['createdBy'] as String?,
      modifiedBy: json['modifiedBy'] as String?,
    );

Map<String, dynamic> _$FileAttachmentModelToJson(
  FileAttachmentModel instance,
) => <String, dynamic>{
  'fileAttachmentId': instance.fileAttachmentId,
  'fileAttachmentFolderId': instance.fileAttachmentFolderId,
  'fileAttachmentFileName': instance.fileAttachmentFileName,
  'fileAttachmentFileLocation': instance.fileAttachmentFileLocation,
  'fileAttachmentPreviewFileLocation':
      instance.fileAttachmentPreviewFileLocation,
  'fileAttachmentHasPreview': instance.fileAttachmentHasPreview,
  'fileAttachmentFileSizeInBytes': instance.fileAttachmentFileSizeInBytes,
  'description': instance.description,
  'userId': instance.userId,
  'isDeleted': instance.isDeleted,
  'createdDate': instance.createdDate?.toIso8601String(),
  'createdByUserId': instance.createdByUserId,
  'modifiedDate': instance.modifiedDate?.toIso8601String(),
  'modifiedByUserId': instance.modifiedByUserId,
  'createdByUserFirstName': instance.createdByUserFirstName,
  'createdByUserLastName': instance.createdByUserLastName,
  'createdByUserEmail': instance.createdByUserEmail,
  'modifiedByUserFirstName': instance.modifiedByUserFirstName,
  'modifiedByUserLastName': instance.modifiedByUserLastName,
  'modifiedByUserEmail': instance.modifiedByUserEmail,
  'createdBy': instance.createdBy,
  'modifiedBy': instance.modifiedBy,
};

FileAttachmentModelReturnListModel _$FileAttachmentModelReturnListModelFromJson(
  Map<String, dynamic> json,
) => FileAttachmentModelReturnListModel(
  model:
      (json['model'] as List<dynamic>?)
          ?.map((e) => FileAttachmentModel.fromJson(e as Map<String, dynamic>))
          .toList() ??
      [],
  search: json['search'] == null
      ? null
      : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
  totalRecords: (json['totalRecords'] as num?)?.toInt(),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$FileAttachmentModelReturnListModelToJson(
  FileAttachmentModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

FileAttachmentModelReturnModel _$FileAttachmentModelReturnModelFromJson(
  Map<String, dynamic> json,
) => FileAttachmentModelReturnModel(
  model: json['model'] == null
      ? null
      : FileAttachmentModel.fromJson(json['model'] as Map<String, dynamic>),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$FileAttachmentModelReturnModelToJson(
  FileAttachmentModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

FileAttachmentUpdateModel _$FileAttachmentUpdateModelFromJson(
  Map<String, dynamic> json,
) => FileAttachmentUpdateModel(
  fileAttachmentId: (json['fileAttachmentId'] as num?)?.toInt(),
  fileAttachmentFolderId: (json['fileAttachmentFolderId'] as num?)?.toInt(),
  fileAttachmentFileName: json['fileAttachmentFileName'] as String?,
  fileAttachmentFileLocation: json['fileAttachmentFileLocation'] as String?,
  fileAttachmentPreviewFileLocation:
      json['fileAttachmentPreviewFileLocation'] as String?,
  fileAttachmentHasPreview: json['fileAttachmentHasPreview'] as bool?,
  fileAttachmentFileSizeInBytes: (json['fileAttachmentFileSizeInBytes'] as num?)
      ?.toInt(),
  description: json['description'] as String?,
  userId: (json['userId'] as num?)?.toInt(),
);

Map<String, dynamic> _$FileAttachmentUpdateModelToJson(
  FileAttachmentUpdateModel instance,
) => <String, dynamic>{
  'fileAttachmentId': instance.fileAttachmentId,
  'fileAttachmentFolderId': instance.fileAttachmentFolderId,
  'fileAttachmentFileName': instance.fileAttachmentFileName,
  'fileAttachmentFileLocation': instance.fileAttachmentFileLocation,
  'fileAttachmentPreviewFileLocation':
      instance.fileAttachmentPreviewFileLocation,
  'fileAttachmentHasPreview': instance.fileAttachmentHasPreview,
  'fileAttachmentFileSizeInBytes': instance.fileAttachmentFileSizeInBytes,
  'description': instance.description,
  'userId': instance.userId,
};

ForgotPasswordModel _$ForgotPasswordModelFromJson(Map<String, dynamic> json) =>
    ForgotPasswordModel(
      userLogin: json['userLogin'] as String?,
      token: json['token'] as String?,
      password: json['password'] as String?,
      retypedPassword: json['retypedPassword'] as String?,
    );

Map<String, dynamic> _$ForgotPasswordModelToJson(
  ForgotPasswordModel instance,
) => <String, dynamic>{
  'userLogin': instance.userLogin,
  'token': instance.token,
  'password': instance.password,
  'retypedPassword': instance.retypedPassword,
};

GlobalConfigurationModel _$GlobalConfigurationModelFromJson(
  Map<String, dynamic> json,
) => GlobalConfigurationModel(
  isUserLoggedIn: json['isUserLoggedIn'] as bool?,
  loginOptionModel: json['loginOptionModel'] == null
      ? null
      : LoginOptionModel.fromJson(
          json['loginOptionModel'] as Map<String, dynamic>,
        ),
  appVersion: json['appVersion'] as String?,
  dbVersion: json['dbVersion'] as String?,
  appReleaseDate: json['appReleaseDate'] == null
      ? null
      : DateTime.parse(json['appReleaseDate'] as String),
  helpUrl: json['helpUrl'] as String?,
  onboardingCodes:
      (json['onboardingCodes'] as List<dynamic>?)
          ?.map((e) => TextWithValue.fromJson(e as Map<String, dynamic>))
          .toList() ??
      [],
  useMP: json['useMP'] as bool?,
  enableNewFeatures: json['enableNewFeatures'] as bool?,
  showAllLinksWithoutPermissionCheck:
      json['showAllLinksWithoutPermissionCheck'] as bool?,
);

Map<String, dynamic> _$GlobalConfigurationModelToJson(
  GlobalConfigurationModel instance,
) => <String, dynamic>{
  'isUserLoggedIn': instance.isUserLoggedIn,
  'loginOptionModel': instance.loginOptionModel?.toJson(),
  'appVersion': instance.appVersion,
  'dbVersion': instance.dbVersion,
  'appReleaseDate': instance.appReleaseDate?.toIso8601String(),
  'helpUrl': instance.helpUrl,
  'onboardingCodes': instance.onboardingCodes?.map((e) => e.toJson()).toList(),
  'useMP': instance.useMP,
  'enableNewFeatures': instance.enableNewFeatures,
  'showAllLinksWithoutPermissionCheck':
      instance.showAllLinksWithoutPermissionCheck,
};

GlobalConfigurationModelReturnModel
_$GlobalConfigurationModelReturnModelFromJson(Map<String, dynamic> json) =>
    GlobalConfigurationModelReturnModel(
      model: json['model'] == null
          ? null
          : GlobalConfigurationModel.fromJson(
              json['model'] as Map<String, dynamic>,
            ),
      isSuccess: json['isSuccess'] as bool?,
      successMessage: json['successMessage'] as String?,
      errorHolder: json['errorHolder'] == null
          ? null
          : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
      activityId: json['activityId'] as String?,
      activityDateTime: json['activityDateTime'] == null
          ? null
          : DateTime.parse(json['activityDateTime'] as String),
      server: json['server'] as String?,
      processId: (json['processId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$GlobalConfigurationModelReturnModelToJson(
  GlobalConfigurationModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

GlobalSettingModel _$GlobalSettingModelFromJson(
  Map<String, dynamic> json,
) => GlobalSettingModel(
  html5DocumentViewerPath: json['html5DocumentViewerPath'] as String?,
  html5SupportedDocTypes:
      (json['html5SupportedDocTypes'] as List<dynamic>?)
          ?.map((e) => e as String)
          .toList() ??
      [],
  mobileTheme: json['mobileTheme'] as String?,
  mobileTitle: json['mobileTitle'] as String?,
  mobileCopyright: json['mobileCopyright'] as String?,
  desktopTheme: json['desktopTheme'] as String?,
  desktopCourtUrl: json['desktopCourtUrl'] as String?,
  desktopTitle: json['desktopTitle'] as String?,
  desktopCopyright: json['desktopCopyright'] as String?,
  applicationVersion: json['applicationVersion'] as String?,
  databaseVersion: json['databaseVersion'] as String?,
  enableNewFeatures: json['enableNewFeatures'] as bool?,
  enableFrontPageDisplayMessage: json['enableFrontPageDisplayMessage'] as bool?,
  frontPageDisplayMessage: json['frontPageDisplayMessage'] as String?,
  frontPageDisplayMessageTitle: json['frontPageDisplayMessageTitle'] as String?,
  frontPageDisplayMessageDisplayTime:
      (json['frontPageDisplayMessageDisplayTime'] as num?)?.toInt(),
  frontPageDisplayMessageUntilDate:
      json['frontPageDisplayMessageUntilDate'] == null
      ? null
      : DateTime.parse(json['frontPageDisplayMessageUntilDate'] as String),
  readOnlyAccess: json['readOnlyAccess'] as bool?,
  readOnlyAccessMessage: json['readOnlyAccessMessage'] as String?,
  isProduction: json['isProduction'] as bool?,
  showAllLinksWithoutPermissionCheck:
      json['showAllLinksWithoutPermissionCheck'] as bool?,
);

Map<String, dynamic> _$GlobalSettingModelToJson(GlobalSettingModel instance) =>
    <String, dynamic>{
      'html5DocumentViewerPath': instance.html5DocumentViewerPath,
      'html5SupportedDocTypes': instance.html5SupportedDocTypes,
      'mobileTheme': instance.mobileTheme,
      'mobileTitle': instance.mobileTitle,
      'mobileCopyright': instance.mobileCopyright,
      'desktopTheme': instance.desktopTheme,
      'desktopCourtUrl': instance.desktopCourtUrl,
      'desktopTitle': instance.desktopTitle,
      'desktopCopyright': instance.desktopCopyright,
      'applicationVersion': instance.applicationVersion,
      'databaseVersion': instance.databaseVersion,
      'enableNewFeatures': instance.enableNewFeatures,
      'enableFrontPageDisplayMessage': instance.enableFrontPageDisplayMessage,
      'frontPageDisplayMessage': instance.frontPageDisplayMessage,
      'frontPageDisplayMessageTitle': instance.frontPageDisplayMessageTitle,
      'frontPageDisplayMessageDisplayTime':
          instance.frontPageDisplayMessageDisplayTime,
      'frontPageDisplayMessageUntilDate': instance
          .frontPageDisplayMessageUntilDate
          ?.toIso8601String(),
      'readOnlyAccess': instance.readOnlyAccess,
      'readOnlyAccessMessage': instance.readOnlyAccessMessage,
      'isProduction': instance.isProduction,
      'showAllLinksWithoutPermissionCheck':
          instance.showAllLinksWithoutPermissionCheck,
    };

GlobalSettingModelReturnModel _$GlobalSettingModelReturnModelFromJson(
  Map<String, dynamic> json,
) => GlobalSettingModelReturnModel(
  model: json['model'] == null
      ? null
      : GlobalSettingModel.fromJson(json['model'] as Map<String, dynamic>),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$GlobalSettingModelReturnModelToJson(
  GlobalSettingModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

GoalTrackerByFeedIdSearchCriteria _$GoalTrackerByFeedIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => GoalTrackerByFeedIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  feedId: (json['feedId'] as num?)?.toInt(),
);

Map<String, dynamic> _$GoalTrackerByFeedIdSearchCriteriaToJson(
  GoalTrackerByFeedIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'feedId': instance.feedId,
};

GoalTrackerByGoalTrackerCategoryIdSearchCriteria
_$GoalTrackerByGoalTrackerCategoryIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => GoalTrackerByGoalTrackerCategoryIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  goalTrackerCategoryId: (json['goalTrackerCategoryId'] as num?)?.toInt(),
);

Map<String, dynamic> _$GoalTrackerByGoalTrackerCategoryIdSearchCriteriaToJson(
  GoalTrackerByGoalTrackerCategoryIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'goalTrackerCategoryId': instance.goalTrackerCategoryId,
};

GoalTrackerByGoalTrackerMileStoneIdSearchCriteria
_$GoalTrackerByGoalTrackerMileStoneIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => GoalTrackerByGoalTrackerMileStoneIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  goalTrackerMileStoneId: (json['goalTrackerMileStoneId'] as num?)?.toInt(),
);

Map<String, dynamic> _$GoalTrackerByGoalTrackerMileStoneIdSearchCriteriaToJson(
  GoalTrackerByGoalTrackerMileStoneIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'goalTrackerMileStoneId': instance.goalTrackerMileStoneId,
};

GoalTrackerByUserIdSearchCriteria _$GoalTrackerByUserIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => GoalTrackerByUserIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  userId: (json['userId'] as num?)?.toInt(),
);

Map<String, dynamic> _$GoalTrackerByUserIdSearchCriteriaToJson(
  GoalTrackerByUserIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'userId': instance.userId,
};

GoalTrackerCategoryByCategoryIdSearchCriteria
_$GoalTrackerCategoryByCategoryIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => GoalTrackerCategoryByCategoryIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  categoryId: (json['categoryId'] as num?)?.toInt(),
);

Map<String, dynamic> _$GoalTrackerCategoryByCategoryIdSearchCriteriaToJson(
  GoalTrackerCategoryByCategoryIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'categoryId': instance.categoryId,
};

GoalTrackerCategoryByGoalTrackerIdSearchCriteria
_$GoalTrackerCategoryByGoalTrackerIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => GoalTrackerCategoryByGoalTrackerIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  goalTrackerId: (json['goalTrackerId'] as num?)?.toInt(),
);

Map<String, dynamic> _$GoalTrackerCategoryByGoalTrackerIdSearchCriteriaToJson(
  GoalTrackerCategoryByGoalTrackerIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'goalTrackerId': instance.goalTrackerId,
};

GoalTrackerCategoryByUserIdSearchCriteria
_$GoalTrackerCategoryByUserIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => GoalTrackerCategoryByUserIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  userId: (json['userId'] as num?)?.toInt(),
);

Map<String, dynamic> _$GoalTrackerCategoryByUserIdSearchCriteriaToJson(
  GoalTrackerCategoryByUserIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'userId': instance.userId,
};

GoalTrackerCategoryCreateModel _$GoalTrackerCategoryCreateModelFromJson(
  Map<String, dynamic> json,
) => GoalTrackerCategoryCreateModel(
  categoryId: (json['categoryId'] as num?)?.toInt(),
  goalTrackerId: (json['goalTrackerId'] as num?)?.toInt(),
  userId: (json['userId'] as num?)?.toInt(),
);

Map<String, dynamic> _$GoalTrackerCategoryCreateModelToJson(
  GoalTrackerCategoryCreateModel instance,
) => <String, dynamic>{
  'categoryId': instance.categoryId,
  'goalTrackerId': instance.goalTrackerId,
  'userId': instance.userId,
};

GoalTrackerCategoryModel _$GoalTrackerCategoryModelFromJson(
  Map<String, dynamic> json,
) => GoalTrackerCategoryModel(
  goalTrackerCategoryId: (json['goalTrackerCategoryId'] as num?)?.toInt(),
  categoryId: (json['categoryId'] as num?)?.toInt(),
  goalTrackerId: (json['goalTrackerId'] as num?)?.toInt(),
  userId: (json['userId'] as num?)?.toInt(),
  isDeleted: json['isDeleted'] as bool?,
  createdDate: json['createdDate'] == null
      ? null
      : DateTime.parse(json['createdDate'] as String),
  createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
  modifiedDate: json['modifiedDate'] == null
      ? null
      : DateTime.parse(json['modifiedDate'] as String),
  modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
  predefinedCategoryTypeId: (json['predefinedCategoryTypeId'] as num?)?.toInt(),
  predefinedIconTypeId: (json['predefinedIconTypeId'] as num?)?.toInt(),
  categoryName: json['categoryName'] as String?,
  goalTrackerName: json['goalTrackerName'] as String?,
  userName: json['userName'] as String?,
  createdByUserFirstName: json['createdByUserFirstName'] as String?,
  createdByUserLastName: json['createdByUserLastName'] as String?,
  createdByUserEmail: json['createdByUserEmail'] as String?,
  modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
  modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
  modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
  createdBy: json['createdBy'] as String?,
  modifiedBy: json['modifiedBy'] as String?,
);

Map<String, dynamic> _$GoalTrackerCategoryModelToJson(
  GoalTrackerCategoryModel instance,
) => <String, dynamic>{
  'goalTrackerCategoryId': instance.goalTrackerCategoryId,
  'categoryId': instance.categoryId,
  'goalTrackerId': instance.goalTrackerId,
  'userId': instance.userId,
  'isDeleted': instance.isDeleted,
  'createdDate': instance.createdDate?.toIso8601String(),
  'createdByUserId': instance.createdByUserId,
  'modifiedDate': instance.modifiedDate?.toIso8601String(),
  'modifiedByUserId': instance.modifiedByUserId,
  'predefinedCategoryTypeId': instance.predefinedCategoryTypeId,
  'predefinedIconTypeId': instance.predefinedIconTypeId,
  'categoryName': instance.categoryName,
  'goalTrackerName': instance.goalTrackerName,
  'userName': instance.userName,
  'createdByUserFirstName': instance.createdByUserFirstName,
  'createdByUserLastName': instance.createdByUserLastName,
  'createdByUserEmail': instance.createdByUserEmail,
  'modifiedByUserFirstName': instance.modifiedByUserFirstName,
  'modifiedByUserLastName': instance.modifiedByUserLastName,
  'modifiedByUserEmail': instance.modifiedByUserEmail,
  'createdBy': instance.createdBy,
  'modifiedBy': instance.modifiedBy,
};

GoalTrackerCategoryModelReturnListModel
_$GoalTrackerCategoryModelReturnListModelFromJson(Map<String, dynamic> json) =>
    GoalTrackerCategoryModelReturnListModel(
      model:
          (json['model'] as List<dynamic>?)
              ?.map(
                (e) => GoalTrackerCategoryModel.fromJson(
                  e as Map<String, dynamic>,
                ),
              )
              .toList() ??
          [],
      search: json['search'] == null
          ? null
          : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
      totalRecords: (json['totalRecords'] as num?)?.toInt(),
      isSuccess: json['isSuccess'] as bool?,
      successMessage: json['successMessage'] as String?,
      errorHolder: json['errorHolder'] == null
          ? null
          : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
      activityId: json['activityId'] as String?,
      activityDateTime: json['activityDateTime'] == null
          ? null
          : DateTime.parse(json['activityDateTime'] as String),
      server: json['server'] as String?,
      processId: (json['processId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$GoalTrackerCategoryModelReturnListModelToJson(
  GoalTrackerCategoryModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

GoalTrackerCategoryModelReturnModel
_$GoalTrackerCategoryModelReturnModelFromJson(Map<String, dynamic> json) =>
    GoalTrackerCategoryModelReturnModel(
      model: json['model'] == null
          ? null
          : GoalTrackerCategoryModel.fromJson(
              json['model'] as Map<String, dynamic>,
            ),
      isSuccess: json['isSuccess'] as bool?,
      successMessage: json['successMessage'] as String?,
      errorHolder: json['errorHolder'] == null
          ? null
          : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
      activityId: json['activityId'] as String?,
      activityDateTime: json['activityDateTime'] == null
          ? null
          : DateTime.parse(json['activityDateTime'] as String),
      server: json['server'] as String?,
      processId: (json['processId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$GoalTrackerCategoryModelReturnModelToJson(
  GoalTrackerCategoryModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

GoalTrackerCategoryUpdateModel _$GoalTrackerCategoryUpdateModelFromJson(
  Map<String, dynamic> json,
) => GoalTrackerCategoryUpdateModel(
  goalTrackerCategoryId: (json['goalTrackerCategoryId'] as num?)?.toInt(),
  categoryId: (json['categoryId'] as num?)?.toInt(),
  goalTrackerId: (json['goalTrackerId'] as num?)?.toInt(),
  userId: (json['userId'] as num?)?.toInt(),
);

Map<String, dynamic> _$GoalTrackerCategoryUpdateModelToJson(
  GoalTrackerCategoryUpdateModel instance,
) => <String, dynamic>{
  'goalTrackerCategoryId': instance.goalTrackerCategoryId,
  'categoryId': instance.categoryId,
  'goalTrackerId': instance.goalTrackerId,
  'userId': instance.userId,
};

GoalTrackerCreateModel _$GoalTrackerCreateModelFromJson(
  Map<String, dynamic> json,
) => GoalTrackerCreateModel(
  goalTrackerName: json['goalTrackerName'] as String?,
  predefinedGoalStatusTypeId: (json['predefinedGoalStatusTypeId'] as num?)
      ?.toInt(),
  predefinedGoalPriorityTypeId: (json['predefinedGoalPriorityTypeId'] as num?)
      ?.toInt(),
  goalTrackerStartDate: json['goalTrackerStartDate'] == null
      ? null
      : DateTime.parse(json['goalTrackerStartDate'] as String),
  goalTrackerEndDate: json['goalTrackerEndDate'] == null
      ? null
      : DateTime.parse(json['goalTrackerEndDate'] as String),
  goalTrackerCompletedDate: json['goalTrackerCompletedDate'] == null
      ? null
      : DateTime.parse(json['goalTrackerCompletedDate'] as String),
  userId: (json['userId'] as num?)?.toInt(),
  addedToFeed: json['addedToFeed'] as bool?,
  addedToFeedDate: json['addedToFeedDate'] == null
      ? null
      : DateTime.parse(json['addedToFeedDate'] as String),
  newGoalTrackerCategoryModels:
      (json['newGoalTrackerCategoryModels'] as List<dynamic>?)
          ?.map(
            (e) => GoalTrackerCategoryCreateModel.fromJson(
              e as Map<String, dynamic>,
            ),
          )
          .toList() ??
      [],
  existingGoalTrackerCategoryModels:
      (json['existingGoalTrackerCategoryModels'] as List<dynamic>?)
          ?.map(
            (e) => GoalTrackerCategoryUpdateModel.fromJson(
              e as Map<String, dynamic>,
            ),
          )
          .toList() ??
      [],
);

Map<String, dynamic> _$GoalTrackerCreateModelToJson(
  GoalTrackerCreateModel instance,
) => <String, dynamic>{
  'goalTrackerName': instance.goalTrackerName,
  'predefinedGoalStatusTypeId': instance.predefinedGoalStatusTypeId,
  'predefinedGoalPriorityTypeId': instance.predefinedGoalPriorityTypeId,
  'goalTrackerStartDate': instance.goalTrackerStartDate?.toIso8601String(),
  'goalTrackerEndDate': instance.goalTrackerEndDate?.toIso8601String(),
  'goalTrackerCompletedDate': instance.goalTrackerCompletedDate
      ?.toIso8601String(),
  'userId': instance.userId,
  'addedToFeed': instance.addedToFeed,
  'addedToFeedDate': instance.addedToFeedDate?.toIso8601String(),
  'newGoalTrackerCategoryModels': instance.newGoalTrackerCategoryModels
      ?.map((e) => e.toJson())
      .toList(),
  'existingGoalTrackerCategoryModels': instance
      .existingGoalTrackerCategoryModels
      ?.map((e) => e.toJson())
      .toList(),
};

GoalTrackerMileStoneByFeedIdSearchCriteria
_$GoalTrackerMileStoneByFeedIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => GoalTrackerMileStoneByFeedIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  feedId: (json['feedId'] as num?)?.toInt(),
);

Map<String, dynamic> _$GoalTrackerMileStoneByFeedIdSearchCriteriaToJson(
  GoalTrackerMileStoneByFeedIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'feedId': instance.feedId,
};

GoalTrackerMileStoneByGoalTrackerIdSearchCriteria
_$GoalTrackerMileStoneByGoalTrackerIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => GoalTrackerMileStoneByGoalTrackerIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  goalTrackerId: (json['goalTrackerId'] as num?)?.toInt(),
);

Map<String, dynamic> _$GoalTrackerMileStoneByGoalTrackerIdSearchCriteriaToJson(
  GoalTrackerMileStoneByGoalTrackerIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'goalTrackerId': instance.goalTrackerId,
};

GoalTrackerMileStoneByUserIdSearchCriteria
_$GoalTrackerMileStoneByUserIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => GoalTrackerMileStoneByUserIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  userId: (json['userId'] as num?)?.toInt(),
);

Map<String, dynamic> _$GoalTrackerMileStoneByUserIdSearchCriteriaToJson(
  GoalTrackerMileStoneByUserIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'userId': instance.userId,
};

GoalTrackerMileStoneCreateModel _$GoalTrackerMileStoneCreateModelFromJson(
  Map<String, dynamic> json,
) => GoalTrackerMileStoneCreateModel(
  goalTrackerId: (json['goalTrackerId'] as num?)?.toInt(),
  goalTrackerMileStoneName: json['goalTrackerMileStoneName'] as String?,
  goalTrackerMileStoneStartDate: json['goalTrackerMileStoneStartDate'] == null
      ? null
      : DateTime.parse(json['goalTrackerMileStoneStartDate'] as String),
  goalTrackerMileStoneEndDate: json['goalTrackerMileStoneEndDate'] == null
      ? null
      : DateTime.parse(json['goalTrackerMileStoneEndDate'] as String),
  predefinedGoalStatusTypeId: (json['predefinedGoalStatusTypeId'] as num?)
      ?.toInt(),
  goalTrackerMileStoneCompletedDate:
      json['goalTrackerMileStoneCompletedDate'] == null
      ? null
      : DateTime.parse(json['goalTrackerMileStoneCompletedDate'] as String),
  userId: (json['userId'] as num?)?.toInt(),
);

Map<String, dynamic> _$GoalTrackerMileStoneCreateModelToJson(
  GoalTrackerMileStoneCreateModel instance,
) => <String, dynamic>{
  'goalTrackerId': instance.goalTrackerId,
  'goalTrackerMileStoneName': instance.goalTrackerMileStoneName,
  'goalTrackerMileStoneStartDate': instance.goalTrackerMileStoneStartDate
      ?.toIso8601String(),
  'goalTrackerMileStoneEndDate': instance.goalTrackerMileStoneEndDate
      ?.toIso8601String(),
  'predefinedGoalStatusTypeId': instance.predefinedGoalStatusTypeId,
  'goalTrackerMileStoneCompletedDate': instance
      .goalTrackerMileStoneCompletedDate
      ?.toIso8601String(),
  'userId': instance.userId,
};

GoalTrackerMileStoneModel _$GoalTrackerMileStoneModelFromJson(
  Map<String, dynamic> json,
) => GoalTrackerMileStoneModel(
  goalTrackerMileStoneId: (json['goalTrackerMileStoneId'] as num?)?.toInt(),
  goalTrackerId: (json['goalTrackerId'] as num?)?.toInt(),
  goalTrackerMileStoneName: json['goalTrackerMileStoneName'] as String?,
  goalTrackerMileStoneStartDate: json['goalTrackerMileStoneStartDate'] == null
      ? null
      : DateTime.parse(json['goalTrackerMileStoneStartDate'] as String),
  goalTrackerMileStoneEndDate: json['goalTrackerMileStoneEndDate'] == null
      ? null
      : DateTime.parse(json['goalTrackerMileStoneEndDate'] as String),
  predefinedGoalStatusTypeId: (json['predefinedGoalStatusTypeId'] as num?)
      ?.toInt(),
  goalTrackerMileStoneCompletedDate:
      json['goalTrackerMileStoneCompletedDate'] == null
      ? null
      : DateTime.parse(json['goalTrackerMileStoneCompletedDate'] as String),
  userId: (json['userId'] as num?)?.toInt(),
  isDeleted: json['isDeleted'] as bool?,
  createdDate: json['createdDate'] == null
      ? null
      : DateTime.parse(json['createdDate'] as String),
  createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
  modifiedDate: json['modifiedDate'] == null
      ? null
      : DateTime.parse(json['modifiedDate'] as String),
  modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
  goalTrackerName: json['goalTrackerName'] as String?,
  predefinedGoalStatusTypeName: json['predefinedGoalStatusTypeName'] as String?,
  userName: json['userName'] as String?,
  createdByUserFirstName: json['createdByUserFirstName'] as String?,
  createdByUserLastName: json['createdByUserLastName'] as String?,
  createdByUserEmail: json['createdByUserEmail'] as String?,
  modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
  modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
  modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
  createdBy: json['createdBy'] as String?,
  modifiedBy: json['modifiedBy'] as String?,
);

Map<String, dynamic> _$GoalTrackerMileStoneModelToJson(
  GoalTrackerMileStoneModel instance,
) => <String, dynamic>{
  'goalTrackerMileStoneId': instance.goalTrackerMileStoneId,
  'goalTrackerId': instance.goalTrackerId,
  'goalTrackerMileStoneName': instance.goalTrackerMileStoneName,
  'goalTrackerMileStoneStartDate': instance.goalTrackerMileStoneStartDate
      ?.toIso8601String(),
  'goalTrackerMileStoneEndDate': instance.goalTrackerMileStoneEndDate
      ?.toIso8601String(),
  'predefinedGoalStatusTypeId': instance.predefinedGoalStatusTypeId,
  'goalTrackerMileStoneCompletedDate': instance
      .goalTrackerMileStoneCompletedDate
      ?.toIso8601String(),
  'userId': instance.userId,
  'isDeleted': instance.isDeleted,
  'createdDate': instance.createdDate?.toIso8601String(),
  'createdByUserId': instance.createdByUserId,
  'modifiedDate': instance.modifiedDate?.toIso8601String(),
  'modifiedByUserId': instance.modifiedByUserId,
  'goalTrackerName': instance.goalTrackerName,
  'predefinedGoalStatusTypeName': instance.predefinedGoalStatusTypeName,
  'userName': instance.userName,
  'createdByUserFirstName': instance.createdByUserFirstName,
  'createdByUserLastName': instance.createdByUserLastName,
  'createdByUserEmail': instance.createdByUserEmail,
  'modifiedByUserFirstName': instance.modifiedByUserFirstName,
  'modifiedByUserLastName': instance.modifiedByUserLastName,
  'modifiedByUserEmail': instance.modifiedByUserEmail,
  'createdBy': instance.createdBy,
  'modifiedBy': instance.modifiedBy,
};

GoalTrackerMileStoneModelReturnListModel
_$GoalTrackerMileStoneModelReturnListModelFromJson(Map<String, dynamic> json) =>
    GoalTrackerMileStoneModelReturnListModel(
      model:
          (json['model'] as List<dynamic>?)
              ?.map(
                (e) => GoalTrackerMileStoneModel.fromJson(
                  e as Map<String, dynamic>,
                ),
              )
              .toList() ??
          [],
      search: json['search'] == null
          ? null
          : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
      totalRecords: (json['totalRecords'] as num?)?.toInt(),
      isSuccess: json['isSuccess'] as bool?,
      successMessage: json['successMessage'] as String?,
      errorHolder: json['errorHolder'] == null
          ? null
          : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
      activityId: json['activityId'] as String?,
      activityDateTime: json['activityDateTime'] == null
          ? null
          : DateTime.parse(json['activityDateTime'] as String),
      server: json['server'] as String?,
      processId: (json['processId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$GoalTrackerMileStoneModelReturnListModelToJson(
  GoalTrackerMileStoneModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

GoalTrackerMileStoneModelReturnModel
_$GoalTrackerMileStoneModelReturnModelFromJson(Map<String, dynamic> json) =>
    GoalTrackerMileStoneModelReturnModel(
      model: json['model'] == null
          ? null
          : GoalTrackerMileStoneModel.fromJson(
              json['model'] as Map<String, dynamic>,
            ),
      isSuccess: json['isSuccess'] as bool?,
      successMessage: json['successMessage'] as String?,
      errorHolder: json['errorHolder'] == null
          ? null
          : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
      activityId: json['activityId'] as String?,
      activityDateTime: json['activityDateTime'] == null
          ? null
          : DateTime.parse(json['activityDateTime'] as String),
      server: json['server'] as String?,
      processId: (json['processId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$GoalTrackerMileStoneModelReturnModelToJson(
  GoalTrackerMileStoneModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

GoalTrackerMileStoneUpdateModel _$GoalTrackerMileStoneUpdateModelFromJson(
  Map<String, dynamic> json,
) => GoalTrackerMileStoneUpdateModel(
  goalTrackerMileStoneId: (json['goalTrackerMileStoneId'] as num?)?.toInt(),
  goalTrackerId: (json['goalTrackerId'] as num?)?.toInt(),
  goalTrackerMileStoneName: json['goalTrackerMileStoneName'] as String?,
  goalTrackerMileStoneStartDate: json['goalTrackerMileStoneStartDate'] == null
      ? null
      : DateTime.parse(json['goalTrackerMileStoneStartDate'] as String),
  goalTrackerMileStoneEndDate: json['goalTrackerMileStoneEndDate'] == null
      ? null
      : DateTime.parse(json['goalTrackerMileStoneEndDate'] as String),
  predefinedGoalStatusTypeId: (json['predefinedGoalStatusTypeId'] as num?)
      ?.toInt(),
  goalTrackerMileStoneCompletedDate:
      json['goalTrackerMileStoneCompletedDate'] == null
      ? null
      : DateTime.parse(json['goalTrackerMileStoneCompletedDate'] as String),
  userId: (json['userId'] as num?)?.toInt(),
);

Map<String, dynamic> _$GoalTrackerMileStoneUpdateModelToJson(
  GoalTrackerMileStoneUpdateModel instance,
) => <String, dynamic>{
  'goalTrackerMileStoneId': instance.goalTrackerMileStoneId,
  'goalTrackerId': instance.goalTrackerId,
  'goalTrackerMileStoneName': instance.goalTrackerMileStoneName,
  'goalTrackerMileStoneStartDate': instance.goalTrackerMileStoneStartDate
      ?.toIso8601String(),
  'goalTrackerMileStoneEndDate': instance.goalTrackerMileStoneEndDate
      ?.toIso8601String(),
  'predefinedGoalStatusTypeId': instance.predefinedGoalStatusTypeId,
  'goalTrackerMileStoneCompletedDate': instance
      .goalTrackerMileStoneCompletedDate
      ?.toIso8601String(),
  'userId': instance.userId,
};

GoalTrackerModel _$GoalTrackerModelFromJson(
  Map<String, dynamic> json,
) => GoalTrackerModel(
  goalTrackerId: (json['goalTrackerId'] as num?)?.toInt(),
  goalTrackerName: json['goalTrackerName'] as String?,
  predefinedGoalStatusTypeId: (json['predefinedGoalStatusTypeId'] as num?)
      ?.toInt(),
  predefinedGoalPriorityTypeId: (json['predefinedGoalPriorityTypeId'] as num?)
      ?.toInt(),
  goalTrackerStartDate: json['goalTrackerStartDate'] == null
      ? null
      : DateTime.parse(json['goalTrackerStartDate'] as String),
  goalTrackerEndDate: json['goalTrackerEndDate'] == null
      ? null
      : DateTime.parse(json['goalTrackerEndDate'] as String),
  goalTrackerCompletedDate: json['goalTrackerCompletedDate'] == null
      ? null
      : DateTime.parse(json['goalTrackerCompletedDate'] as String),
  userId: (json['userId'] as num?)?.toInt(),
  addedToFeed: json['addedToFeed'] as bool?,
  addedToFeedDate: json['addedToFeedDate'] == null
      ? null
      : DateTime.parse(json['addedToFeedDate'] as String),
  isDeleted: json['isDeleted'] as bool?,
  createdDate: json['createdDate'] == null
      ? null
      : DateTime.parse(json['createdDate'] as String),
  createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
  modifiedDate: json['modifiedDate'] == null
      ? null
      : DateTime.parse(json['modifiedDate'] as String),
  modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
  predefinedGoalPriorityTypeName:
      json['predefinedGoalPriorityTypeName'] as String?,
  predefinedGoalStatusTypeName: json['predefinedGoalStatusTypeName'] as String?,
  userName: json['userName'] as String?,
  createdByUserFirstName: json['createdByUserFirstName'] as String?,
  createdByUserLastName: json['createdByUserLastName'] as String?,
  createdByUserEmail: json['createdByUserEmail'] as String?,
  modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
  modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
  modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
  createdBy: json['createdBy'] as String?,
  modifiedBy: json['modifiedBy'] as String?,
  existingGoalTrackerMileStones:
      (json['existingGoalTrackerMileStones'] as List<dynamic>?)
          ?.map(
            (e) =>
                GoalTrackerMileStoneModel.fromJson(e as Map<String, dynamic>),
          )
          .toList() ??
      [],
  existingCategorys:
      (json['existingCategorys'] as List<dynamic>?)
          ?.map((e) => CategoryModel.fromJson(e as Map<String, dynamic>))
          .toList() ??
      [],
);

Map<String, dynamic> _$GoalTrackerModelToJson(GoalTrackerModel instance) =>
    <String, dynamic>{
      'goalTrackerId': instance.goalTrackerId,
      'goalTrackerName': instance.goalTrackerName,
      'predefinedGoalStatusTypeId': instance.predefinedGoalStatusTypeId,
      'predefinedGoalPriorityTypeId': instance.predefinedGoalPriorityTypeId,
      'goalTrackerStartDate': instance.goalTrackerStartDate?.toIso8601String(),
      'goalTrackerEndDate': instance.goalTrackerEndDate?.toIso8601String(),
      'goalTrackerCompletedDate': instance.goalTrackerCompletedDate
          ?.toIso8601String(),
      'userId': instance.userId,
      'addedToFeed': instance.addedToFeed,
      'addedToFeedDate': instance.addedToFeedDate?.toIso8601String(),
      'isDeleted': instance.isDeleted,
      'createdDate': instance.createdDate?.toIso8601String(),
      'createdByUserId': instance.createdByUserId,
      'modifiedDate': instance.modifiedDate?.toIso8601String(),
      'modifiedByUserId': instance.modifiedByUserId,
      'predefinedGoalPriorityTypeName': instance.predefinedGoalPriorityTypeName,
      'predefinedGoalStatusTypeName': instance.predefinedGoalStatusTypeName,
      'userName': instance.userName,
      'createdByUserFirstName': instance.createdByUserFirstName,
      'createdByUserLastName': instance.createdByUserLastName,
      'createdByUserEmail': instance.createdByUserEmail,
      'modifiedByUserFirstName': instance.modifiedByUserFirstName,
      'modifiedByUserLastName': instance.modifiedByUserLastName,
      'modifiedByUserEmail': instance.modifiedByUserEmail,
      'createdBy': instance.createdBy,
      'modifiedBy': instance.modifiedBy,
      'existingGoalTrackerMileStones': instance.existingGoalTrackerMileStones
          ?.map((e) => e.toJson())
          .toList(),
      'existingCategorys': instance.existingCategorys
          ?.map((e) => e.toJson())
          .toList(),
    };

GoalTrackerModelReturnListModel _$GoalTrackerModelReturnListModelFromJson(
  Map<String, dynamic> json,
) => GoalTrackerModelReturnListModel(
  model:
      (json['model'] as List<dynamic>?)
          ?.map((e) => GoalTrackerModel.fromJson(e as Map<String, dynamic>))
          .toList() ??
      [],
  search: json['search'] == null
      ? null
      : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
  totalRecords: (json['totalRecords'] as num?)?.toInt(),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$GoalTrackerModelReturnListModelToJson(
  GoalTrackerModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

GoalTrackerModelReturnModel _$GoalTrackerModelReturnModelFromJson(
  Map<String, dynamic> json,
) => GoalTrackerModelReturnModel(
  model: json['model'] == null
      ? null
      : GoalTrackerModel.fromJson(json['model'] as Map<String, dynamic>),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$GoalTrackerModelReturnModelToJson(
  GoalTrackerModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

GoalTrackerUpdateModel _$GoalTrackerUpdateModelFromJson(
  Map<String, dynamic> json,
) => GoalTrackerUpdateModel(
  goalTrackerId: (json['goalTrackerId'] as num?)?.toInt(),
  goalTrackerName: json['goalTrackerName'] as String?,
  predefinedGoalStatusTypeId: (json['predefinedGoalStatusTypeId'] as num?)
      ?.toInt(),
  predefinedGoalPriorityTypeId: (json['predefinedGoalPriorityTypeId'] as num?)
      ?.toInt(),
  goalTrackerStartDate: json['goalTrackerStartDate'] == null
      ? null
      : DateTime.parse(json['goalTrackerStartDate'] as String),
  goalTrackerEndDate: json['goalTrackerEndDate'] == null
      ? null
      : DateTime.parse(json['goalTrackerEndDate'] as String),
  goalTrackerCompletedDate: json['goalTrackerCompletedDate'] == null
      ? null
      : DateTime.parse(json['goalTrackerCompletedDate'] as String),
  userId: (json['userId'] as num?)?.toInt(),
  addedToFeed: json['addedToFeed'] as bool?,
  addedToFeedDate: json['addedToFeedDate'] == null
      ? null
      : DateTime.parse(json['addedToFeedDate'] as String),
  newGoalTrackerCategoryModels:
      (json['newGoalTrackerCategoryModels'] as List<dynamic>?)
          ?.map(
            (e) => GoalTrackerCategoryCreateModel.fromJson(
              e as Map<String, dynamic>,
            ),
          )
          .toList() ??
      [],
  existingGoalTrackerCategoryModels:
      (json['existingGoalTrackerCategoryModels'] as List<dynamic>?)
          ?.map(
            (e) => GoalTrackerCategoryUpdateModel.fromJson(
              e as Map<String, dynamic>,
            ),
          )
          .toList() ??
      [],
);

Map<String, dynamic> _$GoalTrackerUpdateModelToJson(
  GoalTrackerUpdateModel instance,
) => <String, dynamic>{
  'goalTrackerId': instance.goalTrackerId,
  'goalTrackerName': instance.goalTrackerName,
  'predefinedGoalStatusTypeId': instance.predefinedGoalStatusTypeId,
  'predefinedGoalPriorityTypeId': instance.predefinedGoalPriorityTypeId,
  'goalTrackerStartDate': instance.goalTrackerStartDate?.toIso8601String(),
  'goalTrackerEndDate': instance.goalTrackerEndDate?.toIso8601String(),
  'goalTrackerCompletedDate': instance.goalTrackerCompletedDate
      ?.toIso8601String(),
  'userId': instance.userId,
  'addedToFeed': instance.addedToFeed,
  'addedToFeedDate': instance.addedToFeedDate?.toIso8601String(),
  'newGoalTrackerCategoryModels': instance.newGoalTrackerCategoryModels
      ?.map((e) => e.toJson())
      .toList(),
  'existingGoalTrackerCategoryModels': instance
      .existingGoalTrackerCategoryModels
      ?.map((e) => e.toJson())
      .toList(),
};

IsPremiumFeatureEnabledModel _$IsPremiumFeatureEnabledModelFromJson(
  Map<String, dynamic> json,
) => IsPremiumFeatureEnabledModel(
  enabled: json['enabled'] as bool?,
  enabledUntilDate: json['enabledUntilDate'] == null
      ? null
      : DateTime.parse(json['enabledUntilDate'] as String),
);

Map<String, dynamic> _$IsPremiumFeatureEnabledModelToJson(
  IsPremiumFeatureEnabledModel instance,
) => <String, dynamic>{
  'enabled': instance.enabled,
  'enabledUntilDate': instance.enabledUntilDate?.toIso8601String(),
};

IsPremiumFeatureEnabledModelReturnModel
_$IsPremiumFeatureEnabledModelReturnModelFromJson(Map<String, dynamic> json) =>
    IsPremiumFeatureEnabledModelReturnModel(
      model: json['model'] == null
          ? null
          : IsPremiumFeatureEnabledModel.fromJson(
              json['model'] as Map<String, dynamic>,
            ),
      isSuccess: json['isSuccess'] as bool?,
      successMessage: json['successMessage'] as String?,
      errorHolder: json['errorHolder'] == null
          ? null
          : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
      activityId: json['activityId'] as String?,
      activityDateTime: json['activityDateTime'] == null
          ? null
          : DateTime.parse(json['activityDateTime'] as String),
      server: json['server'] as String?,
      processId: (json['processId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$IsPremiumFeatureEnabledModelReturnModelToJson(
  IsPremiumFeatureEnabledModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

JobAttachmentCreateModel _$JobAttachmentCreateModelFromJson(
  Map<String, dynamic> json,
) => JobAttachmentCreateModel(
  jobId: (json['jobId'] as num?)?.toInt(),
  fileName: json['fileName'] as String?,
  storedFilePath: json['storedFilePath'] as String?,
);

Map<String, dynamic> _$JobAttachmentCreateModelToJson(
  JobAttachmentCreateModel instance,
) => <String, dynamic>{
  'jobId': instance.jobId,
  'fileName': instance.fileName,
  'storedFilePath': instance.storedFilePath,
};

JobAttachmentModel _$JobAttachmentModelFromJson(Map<String, dynamic> json) =>
    JobAttachmentModel(
      jobAttachmentId: (json['jobAttachmentId'] as num?)?.toInt(),
      jobId: (json['jobId'] as num?)?.toInt(),
      fileName: json['fileName'] as String?,
      isDeleted: json['isDeleted'] as bool?,
      createdDate: json['createdDate'] == null
          ? null
          : DateTime.parse(json['createdDate'] as String),
      createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
      modifiedDate: json['modifiedDate'] == null
          ? null
          : DateTime.parse(json['modifiedDate'] as String),
      modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
      storedFilePath: json['storedFilePath'] as String?,
      jobName: json['jobName'] as String?,
      createdByUserFirstName: json['createdByUserFirstName'] as String?,
      createdByUserLastName: json['createdByUserLastName'] as String?,
      createdByUserEmail: json['createdByUserEmail'] as String?,
      modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
      modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
      modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
      createdBy: json['createdBy'] as String?,
      modifiedBy: json['modifiedBy'] as String?,
    );

Map<String, dynamic> _$JobAttachmentModelToJson(JobAttachmentModel instance) =>
    <String, dynamic>{
      'jobAttachmentId': instance.jobAttachmentId,
      'jobId': instance.jobId,
      'fileName': instance.fileName,
      'isDeleted': instance.isDeleted,
      'createdDate': instance.createdDate?.toIso8601String(),
      'createdByUserId': instance.createdByUserId,
      'modifiedDate': instance.modifiedDate?.toIso8601String(),
      'modifiedByUserId': instance.modifiedByUserId,
      'storedFilePath': instance.storedFilePath,
      'jobName': instance.jobName,
      'createdByUserFirstName': instance.createdByUserFirstName,
      'createdByUserLastName': instance.createdByUserLastName,
      'createdByUserEmail': instance.createdByUserEmail,
      'modifiedByUserFirstName': instance.modifiedByUserFirstName,
      'modifiedByUserLastName': instance.modifiedByUserLastName,
      'modifiedByUserEmail': instance.modifiedByUserEmail,
      'createdBy': instance.createdBy,
      'modifiedBy': instance.modifiedBy,
    };

JobAttachmentModelReturnListModel _$JobAttachmentModelReturnListModelFromJson(
  Map<String, dynamic> json,
) => JobAttachmentModelReturnListModel(
  model:
      (json['model'] as List<dynamic>?)
          ?.map((e) => JobAttachmentModel.fromJson(e as Map<String, dynamic>))
          .toList() ??
      [],
  search: json['search'] == null
      ? null
      : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
  totalRecords: (json['totalRecords'] as num?)?.toInt(),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$JobAttachmentModelReturnListModelToJson(
  JobAttachmentModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

JobAttachmentModelReturnModel _$JobAttachmentModelReturnModelFromJson(
  Map<String, dynamic> json,
) => JobAttachmentModelReturnModel(
  model: json['model'] == null
      ? null
      : JobAttachmentModel.fromJson(json['model'] as Map<String, dynamic>),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$JobAttachmentModelReturnModelToJson(
  JobAttachmentModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

JobAttachmentUpdateModel _$JobAttachmentUpdateModelFromJson(
  Map<String, dynamic> json,
) => JobAttachmentUpdateModel(
  jobAttachmentId: (json['jobAttachmentId'] as num?)?.toInt(),
  jobId: (json['jobId'] as num?)?.toInt(),
  fileName: json['fileName'] as String?,
  storedFilePath: json['storedFilePath'] as String?,
);

Map<String, dynamic> _$JobAttachmentUpdateModelToJson(
  JobAttachmentUpdateModel instance,
) => <String, dynamic>{
  'jobAttachmentId': instance.jobAttachmentId,
  'jobId': instance.jobId,
  'fileName': instance.fileName,
  'storedFilePath': instance.storedFilePath,
};

JobCreateModel _$JobCreateModelFromJson(Map<String, dynamic> json) =>
    JobCreateModel(
      predefinedJobTypeId: (json['predefinedJobTypeId'] as num?)?.toInt(),
      jobTemplateId: (json['jobTemplateId'] as num?)?.toInt(),
      predefinedJobOrReportTypeId: (json['predefinedJobOrReportTypeId'] as num?)
          ?.toInt(),
      completedDate: json['completedDate'] == null
          ? null
          : DateTime.parse(json['completedDate'] as String),
      submittedDate: json['submittedDate'] == null
          ? null
          : DateTime.parse(json['submittedDate'] as String),
      message: json['message'] as String?,
      predefinedJobStatusTypeId: (json['predefinedJobStatusTypeId'] as num?)
          ?.toInt(),
      submittedByUserId: (json['submittedByUserId'] as num?)?.toInt(),
      executableFolder: json['executableFolder'] as String?,
      executableName: json['executableName'] as String?,
      executableParameter: json['executableParameter'] as String?,
      serializedJobParameter: json['serializedJobParameter'] as String?,
      reportTemplateFile: json['reportTemplateFile'] as String?,
      exportDoc: json['exportDoc'] as bool?,
      exportXls: json['exportXls'] as bool?,
      exportPpt: json['exportPpt'] as bool?,
      exportCsv: json['exportCsv'] as bool?,
      exportXml: json['exportXml'] as bool?,
      exportPdf: json['exportPdf'] as bool?,
      exportCustom: json['exportCustom'] as bool?,
      customExportName: json['customExportName'] as String?,
      pickupDate: json['pickupDate'] == null
          ? null
          : DateTime.parse(json['pickupDate'] as String),
      pickupTime: (json['pickupTime'] as num?)?.toInt(),
      jobName: json['jobName'] as String?,
      includeDataSourceFile: json['includeDataSourceFile'] as bool?,
      executionAppName: json['executionAppName'] as String?,
      executionAppId: json['executionAppId'] as String?,
      executionHostName: json['executionHostName'] as String?,
      executionAppCheckPort: (json['executionAppCheckPort'] as num?)?.toInt(),
      executionProcessId: (json['executionProcessId'] as num?)?.toInt(),
      executionThreadId: (json['executionThreadId'] as num?)?.toInt(),
      emails:
          (json['emails'] as List<dynamic>?)
              ?.map(
                (e) => JobNotificationEmailCreateModel.fromJson(
                  e as Map<String, dynamic>,
                ),
              )
              .toList() ??
          [],
    );

Map<String, dynamic> _$JobCreateModelToJson(JobCreateModel instance) =>
    <String, dynamic>{
      'predefinedJobTypeId': instance.predefinedJobTypeId,
      'jobTemplateId': instance.jobTemplateId,
      'predefinedJobOrReportTypeId': instance.predefinedJobOrReportTypeId,
      'completedDate': instance.completedDate?.toIso8601String(),
      'submittedDate': instance.submittedDate?.toIso8601String(),
      'message': instance.message,
      'predefinedJobStatusTypeId': instance.predefinedJobStatusTypeId,
      'submittedByUserId': instance.submittedByUserId,
      'executableFolder': instance.executableFolder,
      'executableName': instance.executableName,
      'executableParameter': instance.executableParameter,
      'serializedJobParameter': instance.serializedJobParameter,
      'reportTemplateFile': instance.reportTemplateFile,
      'exportDoc': instance.exportDoc,
      'exportXls': instance.exportXls,
      'exportPpt': instance.exportPpt,
      'exportCsv': instance.exportCsv,
      'exportXml': instance.exportXml,
      'exportPdf': instance.exportPdf,
      'exportCustom': instance.exportCustom,
      'customExportName': instance.customExportName,
      'pickupDate': instance.pickupDate?.toIso8601String(),
      'pickupTime': instance.pickupTime,
      'jobName': instance.jobName,
      'includeDataSourceFile': instance.includeDataSourceFile,
      'executionAppName': instance.executionAppName,
      'executionAppId': instance.executionAppId,
      'executionHostName': instance.executionHostName,
      'executionAppCheckPort': instance.executionAppCheckPort,
      'executionProcessId': instance.executionProcessId,
      'executionThreadId': instance.executionThreadId,
      'emails': instance.emails?.map((e) => e.toJson()).toList(),
    };

JobModel _$JobModelFromJson(Map<String, dynamic> json) => JobModel(
  jobId: (json['jobId'] as num?)?.toInt(),
  predefinedJobTypeId: (json['predefinedJobTypeId'] as num?)?.toInt(),
  jobTemplateId: (json['jobTemplateId'] as num?)?.toInt(),
  predefinedJobOrReportTypeId: (json['predefinedJobOrReportTypeId'] as num?)
      ?.toInt(),
  completedDate: json['completedDate'] == null
      ? null
      : DateTime.parse(json['completedDate'] as String),
  submittedDate: json['submittedDate'] == null
      ? null
      : DateTime.parse(json['submittedDate'] as String),
  message: json['message'] as String?,
  predefinedJobStatusTypeId: (json['predefinedJobStatusTypeId'] as num?)
      ?.toInt(),
  submittedByUserId: (json['submittedByUserId'] as num?)?.toInt(),
  executableFolder: json['executableFolder'] as String?,
  executableName: json['executableName'] as String?,
  executableParameter: json['executableParameter'] as String?,
  serializedJobParameter: json['serializedJobParameter'] as String?,
  reportTemplateFile: json['reportTemplateFile'] as String?,
  exportDoc: json['exportDoc'] as bool?,
  exportXls: json['exportXls'] as bool?,
  exportPpt: json['exportPpt'] as bool?,
  exportCsv: json['exportCsv'] as bool?,
  exportXml: json['exportXml'] as bool?,
  exportPdf: json['exportPdf'] as bool?,
  exportCustom: json['exportCustom'] as bool?,
  customExportName: json['customExportName'] as String?,
  pickupDate: json['pickupDate'] == null
      ? null
      : DateTime.parse(json['pickupDate'] as String),
  pickupTime: (json['pickupTime'] as num?)?.toInt(),
  jobName: json['jobName'] as String?,
  includeDataSourceFile: json['includeDataSourceFile'] as bool?,
  executionAppName: json['executionAppName'] as String?,
  executionAppId: json['executionAppId'] as String?,
  executionHostName: json['executionHostName'] as String?,
  executionAppCheckPort: (json['executionAppCheckPort'] as num?)?.toInt(),
  executionProcessId: (json['executionProcessId'] as num?)?.toInt(),
  executionThreadId: (json['executionThreadId'] as num?)?.toInt(),
  isDeleted: json['isDeleted'] as bool?,
  createdDate: json['createdDate'] == null
      ? null
      : DateTime.parse(json['createdDate'] as String),
  createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
  modifiedDate: json['modifiedDate'] == null
      ? null
      : DateTime.parse(json['modifiedDate'] as String),
  modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
  predefinedJobTypeName: json['predefinedJobTypeName'] as String?,
  predefinedJobOrReportTypeName:
      json['predefinedJobOrReportTypeName'] as String?,
  predefinedJobStatusTypeName: json['predefinedJobStatusTypeName'] as String?,
  submittedByUserName: json['submittedByUserName'] as String?,
  createdByUserFirstName: json['createdByUserFirstName'] as String?,
  createdByUserLastName: json['createdByUserLastName'] as String?,
  createdByUserEmail: json['createdByUserEmail'] as String?,
  modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
  modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
  modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
  createdBy: json['createdBy'] as String?,
  modifiedBy: json['modifiedBy'] as String?,
  attachments:
      (json['attachments'] as List<dynamic>?)
          ?.map((e) => JobAttachmentModel.fromJson(e as Map<String, dynamic>))
          .toList() ??
      [],
  notificationEmails:
      (json['notificationEmails'] as List<dynamic>?)
          ?.map(
            (e) =>
                JobNotificationEmailModel.fromJson(e as Map<String, dynamic>),
          )
          .toList() ??
      [],
);

Map<String, dynamic> _$JobModelToJson(JobModel instance) => <String, dynamic>{
  'jobId': instance.jobId,
  'predefinedJobTypeId': instance.predefinedJobTypeId,
  'jobTemplateId': instance.jobTemplateId,
  'predefinedJobOrReportTypeId': instance.predefinedJobOrReportTypeId,
  'completedDate': instance.completedDate?.toIso8601String(),
  'submittedDate': instance.submittedDate?.toIso8601String(),
  'message': instance.message,
  'predefinedJobStatusTypeId': instance.predefinedJobStatusTypeId,
  'submittedByUserId': instance.submittedByUserId,
  'executableFolder': instance.executableFolder,
  'executableName': instance.executableName,
  'executableParameter': instance.executableParameter,
  'serializedJobParameter': instance.serializedJobParameter,
  'reportTemplateFile': instance.reportTemplateFile,
  'exportDoc': instance.exportDoc,
  'exportXls': instance.exportXls,
  'exportPpt': instance.exportPpt,
  'exportCsv': instance.exportCsv,
  'exportXml': instance.exportXml,
  'exportPdf': instance.exportPdf,
  'exportCustom': instance.exportCustom,
  'customExportName': instance.customExportName,
  'pickupDate': instance.pickupDate?.toIso8601String(),
  'pickupTime': instance.pickupTime,
  'jobName': instance.jobName,
  'includeDataSourceFile': instance.includeDataSourceFile,
  'executionAppName': instance.executionAppName,
  'executionAppId': instance.executionAppId,
  'executionHostName': instance.executionHostName,
  'executionAppCheckPort': instance.executionAppCheckPort,
  'executionProcessId': instance.executionProcessId,
  'executionThreadId': instance.executionThreadId,
  'isDeleted': instance.isDeleted,
  'createdDate': instance.createdDate?.toIso8601String(),
  'createdByUserId': instance.createdByUserId,
  'modifiedDate': instance.modifiedDate?.toIso8601String(),
  'modifiedByUserId': instance.modifiedByUserId,
  'predefinedJobTypeName': instance.predefinedJobTypeName,
  'predefinedJobOrReportTypeName': instance.predefinedJobOrReportTypeName,
  'predefinedJobStatusTypeName': instance.predefinedJobStatusTypeName,
  'submittedByUserName': instance.submittedByUserName,
  'createdByUserFirstName': instance.createdByUserFirstName,
  'createdByUserLastName': instance.createdByUserLastName,
  'createdByUserEmail': instance.createdByUserEmail,
  'modifiedByUserFirstName': instance.modifiedByUserFirstName,
  'modifiedByUserLastName': instance.modifiedByUserLastName,
  'modifiedByUserEmail': instance.modifiedByUserEmail,
  'createdBy': instance.createdBy,
  'modifiedBy': instance.modifiedBy,
  'attachments': instance.attachments?.map((e) => e.toJson()).toList(),
  'notificationEmails': instance.notificationEmails
      ?.map((e) => e.toJson())
      .toList(),
};

JobModelReturnListModel _$JobModelReturnListModelFromJson(
  Map<String, dynamic> json,
) => JobModelReturnListModel(
  model:
      (json['model'] as List<dynamic>?)
          ?.map((e) => JobModel.fromJson(e as Map<String, dynamic>))
          .toList() ??
      [],
  search: json['search'] == null
      ? null
      : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
  totalRecords: (json['totalRecords'] as num?)?.toInt(),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$JobModelReturnListModelToJson(
  JobModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

JobModelReturnModel _$JobModelReturnModelFromJson(Map<String, dynamic> json) =>
    JobModelReturnModel(
      model: json['model'] == null
          ? null
          : JobModel.fromJson(json['model'] as Map<String, dynamic>),
      isSuccess: json['isSuccess'] as bool?,
      successMessage: json['successMessage'] as String?,
      errorHolder: json['errorHolder'] == null
          ? null
          : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
      activityId: json['activityId'] as String?,
      activityDateTime: json['activityDateTime'] == null
          ? null
          : DateTime.parse(json['activityDateTime'] as String),
      server: json['server'] as String?,
      processId: (json['processId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$JobModelReturnModelToJson(
  JobModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

JobNotificationEmailCreateModel _$JobNotificationEmailCreateModelFromJson(
  Map<String, dynamic> json,
) => JobNotificationEmailCreateModel(
  jobId: (json['jobId'] as num?)?.toInt(),
  email: json['email'] as String?,
  sendStatus: (json['sendStatus'] as num?)?.toInt(),
  message: json['message'] as String?,
);

Map<String, dynamic> _$JobNotificationEmailCreateModelToJson(
  JobNotificationEmailCreateModel instance,
) => <String, dynamic>{
  'jobId': instance.jobId,
  'email': instance.email,
  'sendStatus': instance.sendStatus,
  'message': instance.message,
};

JobNotificationEmailModel _$JobNotificationEmailModelFromJson(
  Map<String, dynamic> json,
) => JobNotificationEmailModel(
  jobNotificationEmailId: (json['jobNotificationEmailId'] as num?)?.toInt(),
  jobId: (json['jobId'] as num?)?.toInt(),
  email: json['email'] as String?,
  sendStatus: (json['sendStatus'] as num?)?.toInt(),
  message: json['message'] as String?,
  isDeleted: json['isDeleted'] as bool?,
  createdDate: json['createdDate'] == null
      ? null
      : DateTime.parse(json['createdDate'] as String),
  createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
  modifiedDate: json['modifiedDate'] == null
      ? null
      : DateTime.parse(json['modifiedDate'] as String),
  modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
  jobName: json['jobName'] as String?,
  createdByUserFirstName: json['createdByUserFirstName'] as String?,
  createdByUserLastName: json['createdByUserLastName'] as String?,
  createdByUserEmail: json['createdByUserEmail'] as String?,
  modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
  modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
  modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
  createdBy: json['createdBy'] as String?,
  modifiedBy: json['modifiedBy'] as String?,
);

Map<String, dynamic> _$JobNotificationEmailModelToJson(
  JobNotificationEmailModel instance,
) => <String, dynamic>{
  'jobNotificationEmailId': instance.jobNotificationEmailId,
  'jobId': instance.jobId,
  'email': instance.email,
  'sendStatus': instance.sendStatus,
  'message': instance.message,
  'isDeleted': instance.isDeleted,
  'createdDate': instance.createdDate?.toIso8601String(),
  'createdByUserId': instance.createdByUserId,
  'modifiedDate': instance.modifiedDate?.toIso8601String(),
  'modifiedByUserId': instance.modifiedByUserId,
  'jobName': instance.jobName,
  'createdByUserFirstName': instance.createdByUserFirstName,
  'createdByUserLastName': instance.createdByUserLastName,
  'createdByUserEmail': instance.createdByUserEmail,
  'modifiedByUserFirstName': instance.modifiedByUserFirstName,
  'modifiedByUserLastName': instance.modifiedByUserLastName,
  'modifiedByUserEmail': instance.modifiedByUserEmail,
  'createdBy': instance.createdBy,
  'modifiedBy': instance.modifiedBy,
};

JobNotificationEmailModelReturnListModel
_$JobNotificationEmailModelReturnListModelFromJson(Map<String, dynamic> json) =>
    JobNotificationEmailModelReturnListModel(
      model:
          (json['model'] as List<dynamic>?)
              ?.map(
                (e) => JobNotificationEmailModel.fromJson(
                  e as Map<String, dynamic>,
                ),
              )
              .toList() ??
          [],
      search: json['search'] == null
          ? null
          : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
      totalRecords: (json['totalRecords'] as num?)?.toInt(),
      isSuccess: json['isSuccess'] as bool?,
      successMessage: json['successMessage'] as String?,
      errorHolder: json['errorHolder'] == null
          ? null
          : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
      activityId: json['activityId'] as String?,
      activityDateTime: json['activityDateTime'] == null
          ? null
          : DateTime.parse(json['activityDateTime'] as String),
      server: json['server'] as String?,
      processId: (json['processId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$JobNotificationEmailModelReturnListModelToJson(
  JobNotificationEmailModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

JobNotificationEmailModelReturnModel
_$JobNotificationEmailModelReturnModelFromJson(Map<String, dynamic> json) =>
    JobNotificationEmailModelReturnModel(
      model: json['model'] == null
          ? null
          : JobNotificationEmailModel.fromJson(
              json['model'] as Map<String, dynamic>,
            ),
      isSuccess: json['isSuccess'] as bool?,
      successMessage: json['successMessage'] as String?,
      errorHolder: json['errorHolder'] == null
          ? null
          : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
      activityId: json['activityId'] as String?,
      activityDateTime: json['activityDateTime'] == null
          ? null
          : DateTime.parse(json['activityDateTime'] as String),
      server: json['server'] as String?,
      processId: (json['processId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$JobNotificationEmailModelReturnModelToJson(
  JobNotificationEmailModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

JobNotificationEmailUpdateModel _$JobNotificationEmailUpdateModelFromJson(
  Map<String, dynamic> json,
) => JobNotificationEmailUpdateModel(
  jobNotificationEmailId: (json['jobNotificationEmailId'] as num?)?.toInt(),
  jobId: (json['jobId'] as num?)?.toInt(),
  email: json['email'] as String?,
  sendStatus: (json['sendStatus'] as num?)?.toInt(),
  message: json['message'] as String?,
);

Map<String, dynamic> _$JobNotificationEmailUpdateModelToJson(
  JobNotificationEmailUpdateModel instance,
) => <String, dynamic>{
  'jobNotificationEmailId': instance.jobNotificationEmailId,
  'jobId': instance.jobId,
  'email': instance.email,
  'sendStatus': instance.sendStatus,
  'message': instance.message,
};

JobTemplateCategoryCreateModel _$JobTemplateCategoryCreateModelFromJson(
  Map<String, dynamic> json,
) => JobTemplateCategoryCreateModel(name: json['name'] as String?);

Map<String, dynamic> _$JobTemplateCategoryCreateModelToJson(
  JobTemplateCategoryCreateModel instance,
) => <String, dynamic>{'name': instance.name};

JobTemplateCategoryModel _$JobTemplateCategoryModelFromJson(
  Map<String, dynamic> json,
) => JobTemplateCategoryModel(
  jobTemplateCategoryId: (json['jobTemplateCategoryId'] as num?)?.toInt(),
  name: json['name'] as String?,
  isDeleted: json['isDeleted'] as bool?,
  createdDate: json['createdDate'] == null
      ? null
      : DateTime.parse(json['createdDate'] as String),
  createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
  modifiedDate: json['modifiedDate'] == null
      ? null
      : DateTime.parse(json['modifiedDate'] as String),
  modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
  createdByUserFirstName: json['createdByUserFirstName'] as String?,
  createdByUserLastName: json['createdByUserLastName'] as String?,
  createdByUserEmail: json['createdByUserEmail'] as String?,
  modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
  modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
  modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
  createdBy: json['createdBy'] as String?,
  modifiedBy: json['modifiedBy'] as String?,
);

Map<String, dynamic> _$JobTemplateCategoryModelToJson(
  JobTemplateCategoryModel instance,
) => <String, dynamic>{
  'jobTemplateCategoryId': instance.jobTemplateCategoryId,
  'name': instance.name,
  'isDeleted': instance.isDeleted,
  'createdDate': instance.createdDate?.toIso8601String(),
  'createdByUserId': instance.createdByUserId,
  'modifiedDate': instance.modifiedDate?.toIso8601String(),
  'modifiedByUserId': instance.modifiedByUserId,
  'createdByUserFirstName': instance.createdByUserFirstName,
  'createdByUserLastName': instance.createdByUserLastName,
  'createdByUserEmail': instance.createdByUserEmail,
  'modifiedByUserFirstName': instance.modifiedByUserFirstName,
  'modifiedByUserLastName': instance.modifiedByUserLastName,
  'modifiedByUserEmail': instance.modifiedByUserEmail,
  'createdBy': instance.createdBy,
  'modifiedBy': instance.modifiedBy,
};

JobTemplateCategoryModelReturnListModel
_$JobTemplateCategoryModelReturnListModelFromJson(Map<String, dynamic> json) =>
    JobTemplateCategoryModelReturnListModel(
      model:
          (json['model'] as List<dynamic>?)
              ?.map(
                (e) => JobTemplateCategoryModel.fromJson(
                  e as Map<String, dynamic>,
                ),
              )
              .toList() ??
          [],
      search: json['search'] == null
          ? null
          : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
      totalRecords: (json['totalRecords'] as num?)?.toInt(),
      isSuccess: json['isSuccess'] as bool?,
      successMessage: json['successMessage'] as String?,
      errorHolder: json['errorHolder'] == null
          ? null
          : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
      activityId: json['activityId'] as String?,
      activityDateTime: json['activityDateTime'] == null
          ? null
          : DateTime.parse(json['activityDateTime'] as String),
      server: json['server'] as String?,
      processId: (json['processId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$JobTemplateCategoryModelReturnListModelToJson(
  JobTemplateCategoryModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

JobTemplateCategoryModelReturnModel
_$JobTemplateCategoryModelReturnModelFromJson(Map<String, dynamic> json) =>
    JobTemplateCategoryModelReturnModel(
      model: json['model'] == null
          ? null
          : JobTemplateCategoryModel.fromJson(
              json['model'] as Map<String, dynamic>,
            ),
      isSuccess: json['isSuccess'] as bool?,
      successMessage: json['successMessage'] as String?,
      errorHolder: json['errorHolder'] == null
          ? null
          : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
      activityId: json['activityId'] as String?,
      activityDateTime: json['activityDateTime'] == null
          ? null
          : DateTime.parse(json['activityDateTime'] as String),
      server: json['server'] as String?,
      processId: (json['processId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$JobTemplateCategoryModelReturnModelToJson(
  JobTemplateCategoryModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

JobTemplateCategoryUpdateModel _$JobTemplateCategoryUpdateModelFromJson(
  Map<String, dynamic> json,
) => JobTemplateCategoryUpdateModel(
  jobTemplateCategoryId: (json['jobTemplateCategoryId'] as num?)?.toInt(),
  name: json['name'] as String?,
);

Map<String, dynamic> _$JobTemplateCategoryUpdateModelToJson(
  JobTemplateCategoryUpdateModel instance,
) => <String, dynamic>{
  'jobTemplateCategoryId': instance.jobTemplateCategoryId,
  'name': instance.name,
};

JobTemplateCreateModel _$JobTemplateCreateModelFromJson(
  Map<String, dynamic> json,
) => JobTemplateCreateModel(
  predefinedJobTypeId: (json['predefinedJobTypeId'] as num?)?.toInt(),
  description: json['description'] as String?,
  predefinedJobOrReportTypeId: (json['predefinedJobOrReportTypeId'] as num?)
      ?.toInt(),
  predefinedJobUsageForTypeId: (json['predefinedJobUsageForTypeId'] as num?)
      ?.toInt(),
  jobTemplateCategoryId: (json['jobTemplateCategoryId'] as num?)?.toInt(),
  executableFolder: json['executableFolder'] as String?,
  executableName: json['executableName'] as String?,
  executableParameter: json['executableParameter'] as String?,
  canExportDoc: json['canExportDoc'] as bool?,
  canExportXls: json['canExportXls'] as bool?,
  canExportPpt: json['canExportPpt'] as bool?,
  canExportCsv: json['canExportCsv'] as bool?,
  canExportXml: json['canExportXml'] as bool?,
  canExportPdf: json['canExportPdf'] as bool?,
  canExportCustom: json['canExportCustom'] as bool?,
  customExportName: json['customExportName'] as String?,
  templateName: json['templateName'] as String?,
  canExportDocDefaultValue: json['canExportDocDefaultValue'] as bool?,
  canExportXlsDefaultValue: json['canExportXlsDefaultValue'] as bool?,
  canExportPptDefaultValue: json['canExportPptDefaultValue'] as bool?,
  canExportCsvDefaultValue: json['canExportCsvDefaultValue'] as bool?,
  canExportXmlDefaultValue: json['canExportXmlDefaultValue'] as bool?,
  canExportPdfDefaultValue: json['canExportPdfDefaultValue'] as bool?,
  canExportCustomDefaultValue: json['canExportCustomDefaultValue'] as bool?,
  taskIdentifierName: json['taskIdentifierName'] as String?,
  reportTemplateFile: json['reportTemplateFile'] as String?,
  defaultOutputFileName: json['defaultOutputFileName'] as String?,
  includeDataSourceFile: json['includeDataSourceFile'] as bool?,
  includeDataSourceFileDefaultValue:
      json['includeDataSourceFileDefaultValue'] as bool?,
  newParameters:
      (json['newParameters'] as List<dynamic>?)
          ?.map(
            (e) => JobTemplateParameterCreateModel.fromJson(
              e as Map<String, dynamic>,
            ),
          )
          .toList() ??
      [],
);

Map<String, dynamic> _$JobTemplateCreateModelToJson(
  JobTemplateCreateModel instance,
) => <String, dynamic>{
  'predefinedJobTypeId': instance.predefinedJobTypeId,
  'description': instance.description,
  'predefinedJobOrReportTypeId': instance.predefinedJobOrReportTypeId,
  'predefinedJobUsageForTypeId': instance.predefinedJobUsageForTypeId,
  'jobTemplateCategoryId': instance.jobTemplateCategoryId,
  'executableFolder': instance.executableFolder,
  'executableName': instance.executableName,
  'executableParameter': instance.executableParameter,
  'canExportDoc': instance.canExportDoc,
  'canExportXls': instance.canExportXls,
  'canExportPpt': instance.canExportPpt,
  'canExportCsv': instance.canExportCsv,
  'canExportXml': instance.canExportXml,
  'canExportPdf': instance.canExportPdf,
  'canExportCustom': instance.canExportCustom,
  'customExportName': instance.customExportName,
  'templateName': instance.templateName,
  'canExportDocDefaultValue': instance.canExportDocDefaultValue,
  'canExportXlsDefaultValue': instance.canExportXlsDefaultValue,
  'canExportPptDefaultValue': instance.canExportPptDefaultValue,
  'canExportCsvDefaultValue': instance.canExportCsvDefaultValue,
  'canExportXmlDefaultValue': instance.canExportXmlDefaultValue,
  'canExportPdfDefaultValue': instance.canExportPdfDefaultValue,
  'canExportCustomDefaultValue': instance.canExportCustomDefaultValue,
  'taskIdentifierName': instance.taskIdentifierName,
  'reportTemplateFile': instance.reportTemplateFile,
  'defaultOutputFileName': instance.defaultOutputFileName,
  'includeDataSourceFile': instance.includeDataSourceFile,
  'includeDataSourceFileDefaultValue':
      instance.includeDataSourceFileDefaultValue,
  'newParameters': instance.newParameters?.map((e) => e.toJson()).toList(),
};

JobTemplateModel _$JobTemplateModelFromJson(Map<String, dynamic> json) =>
    JobTemplateModel(
      jobTemplateId: (json['jobTemplateId'] as num?)?.toInt(),
      predefinedJobTypeId: (json['predefinedJobTypeId'] as num?)?.toInt(),
      description: json['description'] as String?,
      predefinedJobOrReportTypeId: (json['predefinedJobOrReportTypeId'] as num?)
          ?.toInt(),
      predefinedJobUsageForTypeId: (json['predefinedJobUsageForTypeId'] as num?)
          ?.toInt(),
      jobTemplateCategoryId: (json['jobTemplateCategoryId'] as num?)?.toInt(),
      executableFolder: json['executableFolder'] as String?,
      executableName: json['executableName'] as String?,
      executableParameter: json['executableParameter'] as String?,
      canExportDoc: json['canExportDoc'] as bool?,
      canExportXls: json['canExportXls'] as bool?,
      canExportPpt: json['canExportPpt'] as bool?,
      canExportCsv: json['canExportCsv'] as bool?,
      canExportXml: json['canExportXml'] as bool?,
      canExportPdf: json['canExportPdf'] as bool?,
      canExportCustom: json['canExportCustom'] as bool?,
      customExportName: json['customExportName'] as String?,
      templateName: json['templateName'] as String?,
      canExportDocDefaultValue: json['canExportDocDefaultValue'] as bool?,
      canExportXlsDefaultValue: json['canExportXlsDefaultValue'] as bool?,
      canExportPptDefaultValue: json['canExportPptDefaultValue'] as bool?,
      canExportCsvDefaultValue: json['canExportCsvDefaultValue'] as bool?,
      canExportXmlDefaultValue: json['canExportXmlDefaultValue'] as bool?,
      canExportPdfDefaultValue: json['canExportPdfDefaultValue'] as bool?,
      canExportCustomDefaultValue: json['canExportCustomDefaultValue'] as bool?,
      taskIdentifierName: json['taskIdentifierName'] as String?,
      reportTemplateFile: json['reportTemplateFile'] as String?,
      defaultOutputFileName: json['defaultOutputFileName'] as String?,
      includeDataSourceFile: json['includeDataSourceFile'] as bool?,
      includeDataSourceFileDefaultValue:
          json['includeDataSourceFileDefaultValue'] as bool?,
      isDeleted: json['isDeleted'] as bool?,
      createdDate: json['createdDate'] == null
          ? null
          : DateTime.parse(json['createdDate'] as String),
      createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
      modifiedDate: json['modifiedDate'] == null
          ? null
          : DateTime.parse(json['modifiedDate'] as String),
      modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
      predefinedJobTypeName: json['predefinedJobTypeName'] as String?,
      predefinedJobOrReportTypeName:
          json['predefinedJobOrReportTypeName'] as String?,
      createdByUserFirstName: json['createdByUserFirstName'] as String?,
      createdByUserLastName: json['createdByUserLastName'] as String?,
      createdByUserEmail: json['createdByUserEmail'] as String?,
      modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
      modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
      modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
      createdBy: json['createdBy'] as String?,
      modifiedBy: json['modifiedBy'] as String?,
      parameters:
          (json['parameters'] as List<dynamic>?)
              ?.map(
                (e) => JobTemplateParameterModel.fromJson(
                  e as Map<String, dynamic>,
                ),
              )
              .toList() ??
          [],
    );

Map<String, dynamic> _$JobTemplateModelToJson(JobTemplateModel instance) =>
    <String, dynamic>{
      'jobTemplateId': instance.jobTemplateId,
      'predefinedJobTypeId': instance.predefinedJobTypeId,
      'description': instance.description,
      'predefinedJobOrReportTypeId': instance.predefinedJobOrReportTypeId,
      'predefinedJobUsageForTypeId': instance.predefinedJobUsageForTypeId,
      'jobTemplateCategoryId': instance.jobTemplateCategoryId,
      'executableFolder': instance.executableFolder,
      'executableName': instance.executableName,
      'executableParameter': instance.executableParameter,
      'canExportDoc': instance.canExportDoc,
      'canExportXls': instance.canExportXls,
      'canExportPpt': instance.canExportPpt,
      'canExportCsv': instance.canExportCsv,
      'canExportXml': instance.canExportXml,
      'canExportPdf': instance.canExportPdf,
      'canExportCustom': instance.canExportCustom,
      'customExportName': instance.customExportName,
      'templateName': instance.templateName,
      'canExportDocDefaultValue': instance.canExportDocDefaultValue,
      'canExportXlsDefaultValue': instance.canExportXlsDefaultValue,
      'canExportPptDefaultValue': instance.canExportPptDefaultValue,
      'canExportCsvDefaultValue': instance.canExportCsvDefaultValue,
      'canExportXmlDefaultValue': instance.canExportXmlDefaultValue,
      'canExportPdfDefaultValue': instance.canExportPdfDefaultValue,
      'canExportCustomDefaultValue': instance.canExportCustomDefaultValue,
      'taskIdentifierName': instance.taskIdentifierName,
      'reportTemplateFile': instance.reportTemplateFile,
      'defaultOutputFileName': instance.defaultOutputFileName,
      'includeDataSourceFile': instance.includeDataSourceFile,
      'includeDataSourceFileDefaultValue':
          instance.includeDataSourceFileDefaultValue,
      'isDeleted': instance.isDeleted,
      'createdDate': instance.createdDate?.toIso8601String(),
      'createdByUserId': instance.createdByUserId,
      'modifiedDate': instance.modifiedDate?.toIso8601String(),
      'modifiedByUserId': instance.modifiedByUserId,
      'predefinedJobTypeName': instance.predefinedJobTypeName,
      'predefinedJobOrReportTypeName': instance.predefinedJobOrReportTypeName,
      'createdByUserFirstName': instance.createdByUserFirstName,
      'createdByUserLastName': instance.createdByUserLastName,
      'createdByUserEmail': instance.createdByUserEmail,
      'modifiedByUserFirstName': instance.modifiedByUserFirstName,
      'modifiedByUserLastName': instance.modifiedByUserLastName,
      'modifiedByUserEmail': instance.modifiedByUserEmail,
      'createdBy': instance.createdBy,
      'modifiedBy': instance.modifiedBy,
      'parameters': instance.parameters?.map((e) => e.toJson()).toList(),
    };

JobTemplateModelReturnListModel _$JobTemplateModelReturnListModelFromJson(
  Map<String, dynamic> json,
) => JobTemplateModelReturnListModel(
  model:
      (json['model'] as List<dynamic>?)
          ?.map((e) => JobTemplateModel.fromJson(e as Map<String, dynamic>))
          .toList() ??
      [],
  search: json['search'] == null
      ? null
      : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
  totalRecords: (json['totalRecords'] as num?)?.toInt(),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$JobTemplateModelReturnListModelToJson(
  JobTemplateModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

JobTemplateModelReturnModel _$JobTemplateModelReturnModelFromJson(
  Map<String, dynamic> json,
) => JobTemplateModelReturnModel(
  model: json['model'] == null
      ? null
      : JobTemplateModel.fromJson(json['model'] as Map<String, dynamic>),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$JobTemplateModelReturnModelToJson(
  JobTemplateModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

JobTemplateParameterCreateModel _$JobTemplateParameterCreateModelFromJson(
  Map<String, dynamic> json,
) => JobTemplateParameterCreateModel(
  jobTemplateId: (json['jobTemplateId'] as num?)?.toInt(),
  name: json['name'] as String?,
  $value: json['value'] as String?,
  defaultValue: json['defaultValue'] as String?,
  predefinedFieldControlTypeId: (json['predefinedFieldControlTypeId'] as num?)
      ?.toInt(),
  predefinedFieldTypeId: (json['predefinedFieldTypeId'] as num?)?.toInt(),
  predefinedFieldPopulatorTypeId:
      (json['predefinedFieldPopulatorTypeId'] as num?)?.toInt(),
  caption: json['caption'] as String?,
  populatorSource: json['populatorSource'] as String?,
  size: (json['size'] as num?)?.toInt(),
  empty: json['empty'] as bool?,
  position: (json['position'] as num?)?.toInt(),
  isVisible: json['isVisible'] as bool?,
);

Map<String, dynamic> _$JobTemplateParameterCreateModelToJson(
  JobTemplateParameterCreateModel instance,
) => <String, dynamic>{
  'jobTemplateId': instance.jobTemplateId,
  'name': instance.name,
  'value': instance.$value,
  'defaultValue': instance.defaultValue,
  'predefinedFieldControlTypeId': instance.predefinedFieldControlTypeId,
  'predefinedFieldTypeId': instance.predefinedFieldTypeId,
  'predefinedFieldPopulatorTypeId': instance.predefinedFieldPopulatorTypeId,
  'caption': instance.caption,
  'populatorSource': instance.populatorSource,
  'size': instance.size,
  'empty': instance.empty,
  'position': instance.position,
  'isVisible': instance.isVisible,
};

JobTemplateParameterModel _$JobTemplateParameterModelFromJson(
  Map<String, dynamic> json,
) => JobTemplateParameterModel(
  jobTemplateParameterId: (json['jobTemplateParameterId'] as num?)?.toInt(),
  jobTemplateId: (json['jobTemplateId'] as num?)?.toInt(),
  name: json['name'] as String?,
  $value: json['value'] as String?,
  defaultValue: json['defaultValue'] as String?,
  predefinedFieldControlTypeId: (json['predefinedFieldControlTypeId'] as num?)
      ?.toInt(),
  predefinedFieldTypeId: (json['predefinedFieldTypeId'] as num?)?.toInt(),
  predefinedFieldPopulatorTypeId:
      (json['predefinedFieldPopulatorTypeId'] as num?)?.toInt(),
  caption: json['caption'] as String?,
  populatorSource: json['populatorSource'] as String?,
  size: (json['size'] as num?)?.toInt(),
  empty: json['empty'] as bool?,
  position: (json['position'] as num?)?.toInt(),
  isVisible: json['isVisible'] as bool?,
  isDeleted: json['isDeleted'] as bool?,
  createdDate: json['createdDate'] == null
      ? null
      : DateTime.parse(json['createdDate'] as String),
  createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
  modifiedDate: json['modifiedDate'] == null
      ? null
      : DateTime.parse(json['modifiedDate'] as String),
  modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
  predefinedFieldControlTypeName:
      json['predefinedFieldControlTypeName'] as String?,
  predefinedFieldTypeName: json['predefinedFieldTypeName'] as String?,
  predefinedFieldPopulatorTypeName:
      json['predefinedFieldPopulatorTypeName'] as String?,
  createdByUserFirstName: json['createdByUserFirstName'] as String?,
  createdByUserLastName: json['createdByUserLastName'] as String?,
  createdByUserEmail: json['createdByUserEmail'] as String?,
  modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
  modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
  modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
  createdBy: json['createdBy'] as String?,
  modifiedBy: json['modifiedBy'] as String?,
);

Map<String, dynamic> _$JobTemplateParameterModelToJson(
  JobTemplateParameterModel instance,
) => <String, dynamic>{
  'jobTemplateParameterId': instance.jobTemplateParameterId,
  'jobTemplateId': instance.jobTemplateId,
  'name': instance.name,
  'value': instance.$value,
  'defaultValue': instance.defaultValue,
  'predefinedFieldControlTypeId': instance.predefinedFieldControlTypeId,
  'predefinedFieldTypeId': instance.predefinedFieldTypeId,
  'predefinedFieldPopulatorTypeId': instance.predefinedFieldPopulatorTypeId,
  'caption': instance.caption,
  'populatorSource': instance.populatorSource,
  'size': instance.size,
  'empty': instance.empty,
  'position': instance.position,
  'isVisible': instance.isVisible,
  'isDeleted': instance.isDeleted,
  'createdDate': instance.createdDate?.toIso8601String(),
  'createdByUserId': instance.createdByUserId,
  'modifiedDate': instance.modifiedDate?.toIso8601String(),
  'modifiedByUserId': instance.modifiedByUserId,
  'predefinedFieldControlTypeName': instance.predefinedFieldControlTypeName,
  'predefinedFieldTypeName': instance.predefinedFieldTypeName,
  'predefinedFieldPopulatorTypeName': instance.predefinedFieldPopulatorTypeName,
  'createdByUserFirstName': instance.createdByUserFirstName,
  'createdByUserLastName': instance.createdByUserLastName,
  'createdByUserEmail': instance.createdByUserEmail,
  'modifiedByUserFirstName': instance.modifiedByUserFirstName,
  'modifiedByUserLastName': instance.modifiedByUserLastName,
  'modifiedByUserEmail': instance.modifiedByUserEmail,
  'createdBy': instance.createdBy,
  'modifiedBy': instance.modifiedBy,
};

JobTemplateParameterModelReturnListModel
_$JobTemplateParameterModelReturnListModelFromJson(Map<String, dynamic> json) =>
    JobTemplateParameterModelReturnListModel(
      model:
          (json['model'] as List<dynamic>?)
              ?.map(
                (e) => JobTemplateParameterModel.fromJson(
                  e as Map<String, dynamic>,
                ),
              )
              .toList() ??
          [],
      search: json['search'] == null
          ? null
          : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
      totalRecords: (json['totalRecords'] as num?)?.toInt(),
      isSuccess: json['isSuccess'] as bool?,
      successMessage: json['successMessage'] as String?,
      errorHolder: json['errorHolder'] == null
          ? null
          : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
      activityId: json['activityId'] as String?,
      activityDateTime: json['activityDateTime'] == null
          ? null
          : DateTime.parse(json['activityDateTime'] as String),
      server: json['server'] as String?,
      processId: (json['processId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$JobTemplateParameterModelReturnListModelToJson(
  JobTemplateParameterModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

JobTemplateParameterModelReturnModel
_$JobTemplateParameterModelReturnModelFromJson(Map<String, dynamic> json) =>
    JobTemplateParameterModelReturnModel(
      model: json['model'] == null
          ? null
          : JobTemplateParameterModel.fromJson(
              json['model'] as Map<String, dynamic>,
            ),
      isSuccess: json['isSuccess'] as bool?,
      successMessage: json['successMessage'] as String?,
      errorHolder: json['errorHolder'] == null
          ? null
          : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
      activityId: json['activityId'] as String?,
      activityDateTime: json['activityDateTime'] == null
          ? null
          : DateTime.parse(json['activityDateTime'] as String),
      server: json['server'] as String?,
      processId: (json['processId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$JobTemplateParameterModelReturnModelToJson(
  JobTemplateParameterModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

JobTemplateParameterUpdateModel _$JobTemplateParameterUpdateModelFromJson(
  Map<String, dynamic> json,
) => JobTemplateParameterUpdateModel(
  jobTemplateParameterId: (json['jobTemplateParameterId'] as num?)?.toInt(),
  jobTemplateId: (json['jobTemplateId'] as num?)?.toInt(),
  name: json['name'] as String?,
  $value: json['value'] as String?,
  defaultValue: json['defaultValue'] as String?,
  predefinedFieldControlTypeId: (json['predefinedFieldControlTypeId'] as num?)
      ?.toInt(),
  predefinedFieldTypeId: (json['predefinedFieldTypeId'] as num?)?.toInt(),
  predefinedFieldPopulatorTypeId:
      (json['predefinedFieldPopulatorTypeId'] as num?)?.toInt(),
  caption: json['caption'] as String?,
  populatorSource: json['populatorSource'] as String?,
  size: (json['size'] as num?)?.toInt(),
  empty: json['empty'] as bool?,
  position: (json['position'] as num?)?.toInt(),
  isVisible: json['isVisible'] as bool?,
);

Map<String, dynamic> _$JobTemplateParameterUpdateModelToJson(
  JobTemplateParameterUpdateModel instance,
) => <String, dynamic>{
  'jobTemplateParameterId': instance.jobTemplateParameterId,
  'jobTemplateId': instance.jobTemplateId,
  'name': instance.name,
  'value': instance.$value,
  'defaultValue': instance.defaultValue,
  'predefinedFieldControlTypeId': instance.predefinedFieldControlTypeId,
  'predefinedFieldTypeId': instance.predefinedFieldTypeId,
  'predefinedFieldPopulatorTypeId': instance.predefinedFieldPopulatorTypeId,
  'caption': instance.caption,
  'populatorSource': instance.populatorSource,
  'size': instance.size,
  'empty': instance.empty,
  'position': instance.position,
  'isVisible': instance.isVisible,
};

JobTemplateUpdateModel _$JobTemplateUpdateModelFromJson(
  Map<String, dynamic> json,
) => JobTemplateUpdateModel(
  jobTemplateId: (json['jobTemplateId'] as num?)?.toInt(),
  predefinedJobTypeId: (json['predefinedJobTypeId'] as num?)?.toInt(),
  description: json['description'] as String?,
  predefinedJobOrReportTypeId: (json['predefinedJobOrReportTypeId'] as num?)
      ?.toInt(),
  predefinedJobUsageForTypeId: (json['predefinedJobUsageForTypeId'] as num?)
      ?.toInt(),
  jobTemplateCategoryId: (json['jobTemplateCategoryId'] as num?)?.toInt(),
  executableFolder: json['executableFolder'] as String?,
  executableName: json['executableName'] as String?,
  executableParameter: json['executableParameter'] as String?,
  canExportDoc: json['canExportDoc'] as bool?,
  canExportXls: json['canExportXls'] as bool?,
  canExportPpt: json['canExportPpt'] as bool?,
  canExportCsv: json['canExportCsv'] as bool?,
  canExportXml: json['canExportXml'] as bool?,
  canExportPdf: json['canExportPdf'] as bool?,
  canExportCustom: json['canExportCustom'] as bool?,
  customExportName: json['customExportName'] as String?,
  templateName: json['templateName'] as String?,
  canExportDocDefaultValue: json['canExportDocDefaultValue'] as bool?,
  canExportXlsDefaultValue: json['canExportXlsDefaultValue'] as bool?,
  canExportPptDefaultValue: json['canExportPptDefaultValue'] as bool?,
  canExportCsvDefaultValue: json['canExportCsvDefaultValue'] as bool?,
  canExportXmlDefaultValue: json['canExportXmlDefaultValue'] as bool?,
  canExportPdfDefaultValue: json['canExportPdfDefaultValue'] as bool?,
  canExportCustomDefaultValue: json['canExportCustomDefaultValue'] as bool?,
  taskIdentifierName: json['taskIdentifierName'] as String?,
  reportTemplateFile: json['reportTemplateFile'] as String?,
  defaultOutputFileName: json['defaultOutputFileName'] as String?,
  includeDataSourceFile: json['includeDataSourceFile'] as bool?,
  includeDataSourceFileDefaultValue:
      json['includeDataSourceFileDefaultValue'] as bool?,
  existingParameters:
      (json['existingParameters'] as List<dynamic>?)
          ?.map(
            (e) => JobTemplateParameterUpdateModel.fromJson(
              e as Map<String, dynamic>,
            ),
          )
          .toList() ??
      [],
  newParameters:
      (json['newParameters'] as List<dynamic>?)
          ?.map(
            (e) => JobTemplateParameterCreateModel.fromJson(
              e as Map<String, dynamic>,
            ),
          )
          .toList() ??
      [],
);

Map<String, dynamic> _$JobTemplateUpdateModelToJson(
  JobTemplateUpdateModel instance,
) => <String, dynamic>{
  'jobTemplateId': instance.jobTemplateId,
  'predefinedJobTypeId': instance.predefinedJobTypeId,
  'description': instance.description,
  'predefinedJobOrReportTypeId': instance.predefinedJobOrReportTypeId,
  'predefinedJobUsageForTypeId': instance.predefinedJobUsageForTypeId,
  'jobTemplateCategoryId': instance.jobTemplateCategoryId,
  'executableFolder': instance.executableFolder,
  'executableName': instance.executableName,
  'executableParameter': instance.executableParameter,
  'canExportDoc': instance.canExportDoc,
  'canExportXls': instance.canExportXls,
  'canExportPpt': instance.canExportPpt,
  'canExportCsv': instance.canExportCsv,
  'canExportXml': instance.canExportXml,
  'canExportPdf': instance.canExportPdf,
  'canExportCustom': instance.canExportCustom,
  'customExportName': instance.customExportName,
  'templateName': instance.templateName,
  'canExportDocDefaultValue': instance.canExportDocDefaultValue,
  'canExportXlsDefaultValue': instance.canExportXlsDefaultValue,
  'canExportPptDefaultValue': instance.canExportPptDefaultValue,
  'canExportCsvDefaultValue': instance.canExportCsvDefaultValue,
  'canExportXmlDefaultValue': instance.canExportXmlDefaultValue,
  'canExportPdfDefaultValue': instance.canExportPdfDefaultValue,
  'canExportCustomDefaultValue': instance.canExportCustomDefaultValue,
  'taskIdentifierName': instance.taskIdentifierName,
  'reportTemplateFile': instance.reportTemplateFile,
  'defaultOutputFileName': instance.defaultOutputFileName,
  'includeDataSourceFile': instance.includeDataSourceFile,
  'includeDataSourceFileDefaultValue':
      instance.includeDataSourceFileDefaultValue,
  'existingParameters': instance.existingParameters
      ?.map((e) => e.toJson())
      .toList(),
  'newParameters': instance.newParameters?.map((e) => e.toJson()).toList(),
};

JobUpdateModel _$JobUpdateModelFromJson(Map<String, dynamic> json) =>
    JobUpdateModel(
      jobId: (json['jobId'] as num?)?.toInt(),
      predefinedJobTypeId: (json['predefinedJobTypeId'] as num?)?.toInt(),
      jobTemplateId: (json['jobTemplateId'] as num?)?.toInt(),
      predefinedJobOrReportTypeId: (json['predefinedJobOrReportTypeId'] as num?)
          ?.toInt(),
      completedDate: json['completedDate'] == null
          ? null
          : DateTime.parse(json['completedDate'] as String),
      submittedDate: json['submittedDate'] == null
          ? null
          : DateTime.parse(json['submittedDate'] as String),
      message: json['message'] as String?,
      predefinedJobStatusTypeId: (json['predefinedJobStatusTypeId'] as num?)
          ?.toInt(),
      submittedByUserId: (json['submittedByUserId'] as num?)?.toInt(),
      executableFolder: json['executableFolder'] as String?,
      executableName: json['executableName'] as String?,
      executableParameter: json['executableParameter'] as String?,
      serializedJobParameter: json['serializedJobParameter'] as String?,
      reportTemplateFile: json['reportTemplateFile'] as String?,
      exportDoc: json['exportDoc'] as bool?,
      exportXls: json['exportXls'] as bool?,
      exportPpt: json['exportPpt'] as bool?,
      exportCsv: json['exportCsv'] as bool?,
      exportXml: json['exportXml'] as bool?,
      exportPdf: json['exportPdf'] as bool?,
      exportCustom: json['exportCustom'] as bool?,
      customExportName: json['customExportName'] as String?,
      pickupDate: json['pickupDate'] == null
          ? null
          : DateTime.parse(json['pickupDate'] as String),
      pickupTime: (json['pickupTime'] as num?)?.toInt(),
      jobName: json['jobName'] as String?,
      includeDataSourceFile: json['includeDataSourceFile'] as bool?,
      executionAppName: json['executionAppName'] as String?,
      executionAppId: json['executionAppId'] as String?,
      executionHostName: json['executionHostName'] as String?,
      executionAppCheckPort: (json['executionAppCheckPort'] as num?)?.toInt(),
      executionProcessId: (json['executionProcessId'] as num?)?.toInt(),
      executionThreadId: (json['executionThreadId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$JobUpdateModelToJson(JobUpdateModel instance) =>
    <String, dynamic>{
      'jobId': instance.jobId,
      'predefinedJobTypeId': instance.predefinedJobTypeId,
      'jobTemplateId': instance.jobTemplateId,
      'predefinedJobOrReportTypeId': instance.predefinedJobOrReportTypeId,
      'completedDate': instance.completedDate?.toIso8601String(),
      'submittedDate': instance.submittedDate?.toIso8601String(),
      'message': instance.message,
      'predefinedJobStatusTypeId': instance.predefinedJobStatusTypeId,
      'submittedByUserId': instance.submittedByUserId,
      'executableFolder': instance.executableFolder,
      'executableName': instance.executableName,
      'executableParameter': instance.executableParameter,
      'serializedJobParameter': instance.serializedJobParameter,
      'reportTemplateFile': instance.reportTemplateFile,
      'exportDoc': instance.exportDoc,
      'exportXls': instance.exportXls,
      'exportPpt': instance.exportPpt,
      'exportCsv': instance.exportCsv,
      'exportXml': instance.exportXml,
      'exportPdf': instance.exportPdf,
      'exportCustom': instance.exportCustom,
      'customExportName': instance.customExportName,
      'pickupDate': instance.pickupDate?.toIso8601String(),
      'pickupTime': instance.pickupTime,
      'jobName': instance.jobName,
      'includeDataSourceFile': instance.includeDataSourceFile,
      'executionAppName': instance.executionAppName,
      'executionAppId': instance.executionAppId,
      'executionHostName': instance.executionHostName,
      'executionAppCheckPort': instance.executionAppCheckPort,
      'executionProcessId': instance.executionProcessId,
      'executionThreadId': instance.executionThreadId,
    };

LogConfigurationModel _$LogConfigurationModelFromJson(
  Map<String, dynamic> json,
) => LogConfigurationModel(
  logTrace: json['logTrace'] as bool?,
  logDebug: json['logDebug'] as bool?,
  logInfo: json['logInfo'] as bool?,
  logSql: json['logSql'] as bool?,
  logWarn: json['logWarn'] as bool?,
  logError: json['logError'] as bool?,
  logPerformance: json['logPerformance'] as bool?,
  logLocation: json['logLocation'] as String?,
  logToFile: json['logToFile'] as bool?,
  logToConsole: json['logToConsole'] as bool?,
  logToDebugger: json['logToDebugger'] as bool?,
  logToServer: json['logToServer'] as bool?,
  logServerHost: json['logServerHost'] as String?,
  logServerPort: (json['logServerPort'] as num?)?.toInt(),
  logServerUserName: json['logServerUserName'] as String?,
  logServerPassword: json['logServerPassword'] as String?,
  logServerAccessKey: json['logServerAccessKey'] as String?,
  logPerformanceToMainLogger: json['logPerformanceToMainLogger'] as bool?,
);

Map<String, dynamic> _$LogConfigurationModelToJson(
  LogConfigurationModel instance,
) => <String, dynamic>{
  'logTrace': instance.logTrace,
  'logDebug': instance.logDebug,
  'logInfo': instance.logInfo,
  'logSql': instance.logSql,
  'logWarn': instance.logWarn,
  'logError': instance.logError,
  'logPerformance': instance.logPerformance,
  'logLocation': instance.logLocation,
  'logToFile': instance.logToFile,
  'logToConsole': instance.logToConsole,
  'logToDebugger': instance.logToDebugger,
  'logToServer': instance.logToServer,
  'logServerHost': instance.logServerHost,
  'logServerPort': instance.logServerPort,
  'logServerUserName': instance.logServerUserName,
  'logServerPassword': instance.logServerPassword,
  'logServerAccessKey': instance.logServerAccessKey,
  'logPerformanceToMainLogger': instance.logPerformanceToMainLogger,
};

LogConfigurationModelReturnModel _$LogConfigurationModelReturnModelFromJson(
  Map<String, dynamic> json,
) => LogConfigurationModelReturnModel(
  model: json['model'] == null
      ? null
      : LogConfigurationModel.fromJson(json['model'] as Map<String, dynamic>),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$LogConfigurationModelReturnModelToJson(
  LogConfigurationModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

LogSettings _$LogSettingsFromJson(Map<String, dynamic> json) => LogSettings(
  logTrace: json['logTrace'] as bool?,
  logDebug: json['logDebug'] as bool?,
  logInfo: json['logInfo'] as bool?,
  logSql: json['logSql'] as bool?,
  logWarn: json['logWarn'] as bool?,
  logError: json['logError'] as bool?,
  logPerformance: json['logPerformance'] as bool?,
  logLocation: json['logLocation'] as String?,
  logToFile: json['logToFile'] as bool?,
  logToConsole: json['logToConsole'] as bool?,
  logToDebugger: json['logToDebugger'] as bool?,
  logToServer: json['logToServer'] as bool?,
  logServerHost: json['logServerHost'] as String?,
  logServerPort: (json['logServerPort'] as num?)?.toInt(),
  logServerUserName: json['logServerUserName'] as String?,
  logServerPassword: json['logServerPassword'] as String?,
  logServerAccessKey: json['logServerAccessKey'] as String?,
);

Map<String, dynamic> _$LogSettingsToJson(LogSettings instance) =>
    <String, dynamic>{
      'logTrace': instance.logTrace,
      'logDebug': instance.logDebug,
      'logInfo': instance.logInfo,
      'logSql': instance.logSql,
      'logWarn': instance.logWarn,
      'logError': instance.logError,
      'logPerformance': instance.logPerformance,
      'logLocation': instance.logLocation,
      'logToFile': instance.logToFile,
      'logToConsole': instance.logToConsole,
      'logToDebugger': instance.logToDebugger,
      'logToServer': instance.logToServer,
      'logServerHost': instance.logServerHost,
      'logServerPort': instance.logServerPort,
      'logServerUserName': instance.logServerUserName,
      'logServerPassword': instance.logServerPassword,
      'logServerAccessKey': instance.logServerAccessKey,
    };

LoggedInUserModel _$LoggedInUserModelFromJson(Map<String, dynamic> json) =>
    LoggedInUserModel(
      userProfileGuid: json['userProfileGuid'] as String?,
      userName: json['userName'] as String?,
      firstName: json['firstName'] as String?,
      lastName: json['lastName'] as String?,
      userTypeId: (json['userTypeId'] as num?)?.toInt(),
      userTypeName: json['userTypeName'] as String?,
      userId: (json['userId'] as num?)?.toInt(),
      predefinedCurrencyTypeId: (json['predefinedCurrencyTypeId'] as num?)
          ?.toInt(),
      token: json['token'] as String?,
      permissions:
          (json['permissions'] as List<dynamic>?)
              ?.map((e) => (e as num).toInt())
              .toList() ??
          [],
    );

Map<String, dynamic> _$LoggedInUserModelToJson(LoggedInUserModel instance) =>
    <String, dynamic>{
      'userProfileGuid': instance.userProfileGuid,
      'userName': instance.userName,
      'firstName': instance.firstName,
      'lastName': instance.lastName,
      'userTypeId': instance.userTypeId,
      'userTypeName': instance.userTypeName,
      'userId': instance.userId,
      'predefinedCurrencyTypeId': instance.predefinedCurrencyTypeId,
      'token': instance.token,
      'permissions': instance.permissions,
    };

LoggedInUserModelReturnModel _$LoggedInUserModelReturnModelFromJson(
  Map<String, dynamic> json,
) => LoggedInUserModelReturnModel(
  model: json['model'] == null
      ? null
      : LoggedInUserModel.fromJson(json['model'] as Map<String, dynamic>),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$LoggedInUserModelReturnModelToJson(
  LoggedInUserModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

LoginModel _$LoginModelFromJson(Map<String, dynamic> json) => LoginModel(
  username: json['username'] as String?,
  password: json['password'] as String?,
  rememberMe: json['rememberMe'] as bool?,
);

Map<String, dynamic> _$LoginModelToJson(LoginModel instance) =>
    <String, dynamic>{
      'username': instance.username,
      'password': instance.password,
      'rememberMe': instance.rememberMe,
    };

LoginOptionModel _$LoginOptionModelFromJson(Map<String, dynamic> json) =>
    LoginOptionModel(
      useBuiltInAuthentication: json['useBuiltInAuthentication'] as bool?,
      signInUrl: json['signInUrl'] as String?,
      companyDesktopUrl: json['companyDesktopUrl'] as String?,
      companyMobileUrl: json['companyMobileUrl'] as String?,
      companySlogan: json['companySlogan'] as String?,
      companyEntityName: json['companyEntityName'] as String?,
    );

Map<String, dynamic> _$LoginOptionModelToJson(LoginOptionModel instance) =>
    <String, dynamic>{
      'useBuiltInAuthentication': instance.useBuiltInAuthentication,
      'signInUrl': instance.signInUrl,
      'companyDesktopUrl': instance.companyDesktopUrl,
      'companyMobileUrl': instance.companyMobileUrl,
      'companySlogan': instance.companySlogan,
      'companyEntityName': instance.companyEntityName,
    };

LoginOptionModelReturnModel _$LoginOptionModelReturnModelFromJson(
  Map<String, dynamic> json,
) => LoginOptionModelReturnModel(
  model: json['model'] == null
      ? null
      : LoginOptionModel.fromJson(json['model'] as Map<String, dynamic>),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$LoginOptionModelReturnModelToJson(
  LoginOptionModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

MailServerCreateModel _$MailServerCreateModelFromJson(
  Map<String, dynamic> json,
) => MailServerCreateModel(
  mailServerCode: json['mailServerCode'] as String?,
  mailServerName: json['mailServerName'] as String?,
  host: json['host'] as String?,
  port: (json['port'] as num?)?.toInt(),
  senderName: json['senderName'] as String?,
  senderEmail: json['senderEmail'] as String?,
  userName: json['userName'] as String?,
  password: json['password'] as String?,
  isSecure: json['isSecure'] as bool?,
  isDefault: json['isDefault'] as bool?,
);

Map<String, dynamic> _$MailServerCreateModelToJson(
  MailServerCreateModel instance,
) => <String, dynamic>{
  'mailServerCode': instance.mailServerCode,
  'mailServerName': instance.mailServerName,
  'host': instance.host,
  'port': instance.port,
  'senderName': instance.senderName,
  'senderEmail': instance.senderEmail,
  'userName': instance.userName,
  'password': instance.password,
  'isSecure': instance.isSecure,
  'isDefault': instance.isDefault,
};

MailServerModel _$MailServerModelFromJson(Map<String, dynamic> json) =>
    MailServerModel(
      mailServerId: (json['mailServerId'] as num?)?.toInt(),
      mailServerCode: json['mailServerCode'] as String?,
      mailServerName: json['mailServerName'] as String?,
      host: json['host'] as String?,
      port: (json['port'] as num?)?.toInt(),
      senderName: json['senderName'] as String?,
      senderEmail: json['senderEmail'] as String?,
      userName: json['userName'] as String?,
      password: json['password'] as String?,
      isSecure: json['isSecure'] as bool?,
      isDefault: json['isDefault'] as bool?,
      isDeleted: json['isDeleted'] as bool?,
      createdDate: json['createdDate'] == null
          ? null
          : DateTime.parse(json['createdDate'] as String),
      createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
      modifiedDate: json['modifiedDate'] == null
          ? null
          : DateTime.parse(json['modifiedDate'] as String),
      modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
      createdByUserFirstName: json['createdByUserFirstName'] as String?,
      createdByUserLastName: json['createdByUserLastName'] as String?,
      createdByUserEmail: json['createdByUserEmail'] as String?,
      modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
      modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
      modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
      createdBy: json['createdBy'] as String?,
      modifiedBy: json['modifiedBy'] as String?,
    );

Map<String, dynamic> _$MailServerModelToJson(MailServerModel instance) =>
    <String, dynamic>{
      'mailServerId': instance.mailServerId,
      'mailServerCode': instance.mailServerCode,
      'mailServerName': instance.mailServerName,
      'host': instance.host,
      'port': instance.port,
      'senderName': instance.senderName,
      'senderEmail': instance.senderEmail,
      'userName': instance.userName,
      'password': instance.password,
      'isSecure': instance.isSecure,
      'isDefault': instance.isDefault,
      'isDeleted': instance.isDeleted,
      'createdDate': instance.createdDate?.toIso8601String(),
      'createdByUserId': instance.createdByUserId,
      'modifiedDate': instance.modifiedDate?.toIso8601String(),
      'modifiedByUserId': instance.modifiedByUserId,
      'createdByUserFirstName': instance.createdByUserFirstName,
      'createdByUserLastName': instance.createdByUserLastName,
      'createdByUserEmail': instance.createdByUserEmail,
      'modifiedByUserFirstName': instance.modifiedByUserFirstName,
      'modifiedByUserLastName': instance.modifiedByUserLastName,
      'modifiedByUserEmail': instance.modifiedByUserEmail,
      'createdBy': instance.createdBy,
      'modifiedBy': instance.modifiedBy,
    };

MailServerModelReturnListModel _$MailServerModelReturnListModelFromJson(
  Map<String, dynamic> json,
) => MailServerModelReturnListModel(
  model:
      (json['model'] as List<dynamic>?)
          ?.map((e) => MailServerModel.fromJson(e as Map<String, dynamic>))
          .toList() ??
      [],
  search: json['search'] == null
      ? null
      : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
  totalRecords: (json['totalRecords'] as num?)?.toInt(),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$MailServerModelReturnListModelToJson(
  MailServerModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

MailServerModelReturnModel _$MailServerModelReturnModelFromJson(
  Map<String, dynamic> json,
) => MailServerModelReturnModel(
  model: json['model'] == null
      ? null
      : MailServerModel.fromJson(json['model'] as Map<String, dynamic>),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$MailServerModelReturnModelToJson(
  MailServerModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

MailServerUpdateModel _$MailServerUpdateModelFromJson(
  Map<String, dynamic> json,
) => MailServerUpdateModel(
  mailServerId: (json['mailServerId'] as num?)?.toInt(),
  mailServerCode: json['mailServerCode'] as String?,
  mailServerName: json['mailServerName'] as String?,
  host: json['host'] as String?,
  port: (json['port'] as num?)?.toInt(),
  senderName: json['senderName'] as String?,
  senderEmail: json['senderEmail'] as String?,
  userName: json['userName'] as String?,
  password: json['password'] as String?,
  isSecure: json['isSecure'] as bool?,
  isDefault: json['isDefault'] as bool?,
);

Map<String, dynamic> _$MailServerUpdateModelToJson(
  MailServerUpdateModel instance,
) => <String, dynamic>{
  'mailServerId': instance.mailServerId,
  'mailServerCode': instance.mailServerCode,
  'mailServerName': instance.mailServerName,
  'host': instance.host,
  'port': instance.port,
  'senderName': instance.senderName,
  'senderEmail': instance.senderEmail,
  'userName': instance.userName,
  'password': instance.password,
  'isSecure': instance.isSecure,
  'isDefault': instance.isDefault,
};

ManifestWebConfigurationModel _$ManifestWebConfigurationModelFromJson(
  Map<String, dynamic> json,
) => ManifestWebConfigurationModel(
  opWebServerPort: (json['opWebServerPort'] as num?)?.toInt(),
  desktopSocialLoginErrorUrlPath:
      json['desktopSocialLoginErrorUrlPath'] as String?,
  desktopSocialLoginSuccessUrlPath:
      json['desktopSocialLoginSuccessUrlPath'] as String?,
  viewUrl: json['viewUrl'] as String?,
  readOnlyViewUrl: json['readOnlyViewUrl'] as String?,
  enableFrontPageDisplayMessage: json['enableFrontPageDisplayMessage'] as bool?,
  frontPageDisplayMessage: json['frontPageDisplayMessage'] as String?,
  frontPageDisplayMessageTitle: json['frontPageDisplayMessageTitle'] as String?,
  frontPageDisplayMessageDisplayTime:
      (json['frontPageDisplayMessageDisplayTime'] as num?)?.toInt(),
  frontPageDisplayMessageUntilDate:
      json['frontPageDisplayMessageUntilDate'] == null
      ? null
      : DateTime.parse(json['frontPageDisplayMessageUntilDate'] as String),
  readOnlyAccess: json['readOnlyAccess'] as bool?,
  readOnlyAccessMessage: json['readOnlyAccessMessage'] as String?,
  isProduction: json['isProduction'] as bool?,
  websiteUrl: json['websiteUrl'] as String?,
  letterTemplateFileStorageConnectionString:
      json['letterTemplateFileStorageConnectionString'] as String?,
);

Map<String, dynamic> _$ManifestWebConfigurationModelToJson(
  ManifestWebConfigurationModel instance,
) => <String, dynamic>{
  'opWebServerPort': instance.opWebServerPort,
  'desktopSocialLoginErrorUrlPath': instance.desktopSocialLoginErrorUrlPath,
  'desktopSocialLoginSuccessUrlPath': instance.desktopSocialLoginSuccessUrlPath,
  'viewUrl': instance.viewUrl,
  'readOnlyViewUrl': instance.readOnlyViewUrl,
  'enableFrontPageDisplayMessage': instance.enableFrontPageDisplayMessage,
  'frontPageDisplayMessage': instance.frontPageDisplayMessage,
  'frontPageDisplayMessageTitle': instance.frontPageDisplayMessageTitle,
  'frontPageDisplayMessageDisplayTime':
      instance.frontPageDisplayMessageDisplayTime,
  'frontPageDisplayMessageUntilDate': instance.frontPageDisplayMessageUntilDate
      ?.toIso8601String(),
  'readOnlyAccess': instance.readOnlyAccess,
  'readOnlyAccessMessage': instance.readOnlyAccessMessage,
  'isProduction': instance.isProduction,
  'websiteUrl': instance.websiteUrl,
  'letterTemplateFileStorageConnectionString':
      instance.letterTemplateFileStorageConnectionString,
};

ManifestWebConfigurationModelReturnModel
_$ManifestWebConfigurationModelReturnModelFromJson(Map<String, dynamic> json) =>
    ManifestWebConfigurationModelReturnModel(
      model: json['model'] == null
          ? null
          : ManifestWebConfigurationModel.fromJson(
              json['model'] as Map<String, dynamic>,
            ),
      isSuccess: json['isSuccess'] as bool?,
      successMessage: json['successMessage'] as String?,
      errorHolder: json['errorHolder'] == null
          ? null
          : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
      activityId: json['activityId'] as String?,
      activityDateTime: json['activityDateTime'] == null
          ? null
          : DateTime.parse(json['activityDateTime'] as String),
      server: json['server'] as String?,
      processId: (json['processId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$ManifestWebConfigurationModelReturnModelToJson(
  ManifestWebConfigurationModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

MeditationFileByMeditationSessionIdSearchCriteria
_$MeditationFileByMeditationSessionIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => MeditationFileByMeditationSessionIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  meditationSessionId: (json['meditationSessionId'] as num?)?.toInt(),
);

Map<String, dynamic> _$MeditationFileByMeditationSessionIdSearchCriteriaToJson(
  MeditationFileByMeditationSessionIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'meditationSessionId': instance.meditationSessionId,
};

MeditationFileByUserIdSearchCriteria
_$MeditationFileByUserIdSearchCriteriaFromJson(Map<String, dynamic> json) =>
    MeditationFileByUserIdSearchCriteria(
      selectedId: (json['selectedId'] as num?)?.toInt(),
      page: (json['page'] as num?)?.toInt(),
      pageSize: (json['pageSize'] as num?)?.toInt(),
      keyword: json['keyword'] as String?,
      sortBy: json['sortBy'] as String?,
      sortAscending: json['sortAscending'] as bool?,
      userId: (json['userId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$MeditationFileByUserIdSearchCriteriaToJson(
  MeditationFileByUserIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'userId': instance.userId,
};

MeditationFileCreateModel _$MeditationFileCreateModelFromJson(
  Map<String, dynamic> json,
) => MeditationFileCreateModel(
  predefinedMeditationTypeId: (json['predefinedMeditationTypeId'] as num?)
      ?.toInt(),
  meditationFileName: json['meditationFileName'] as String?,
  location: json['location'] as String?,
  fileName: json['fileName'] as String?,
  fileSize: (json['fileSize'] as num?)?.toInt(),
  userId: (json['userId'] as num?)?.toInt(),
);

Map<String, dynamic> _$MeditationFileCreateModelToJson(
  MeditationFileCreateModel instance,
) => <String, dynamic>{
  'predefinedMeditationTypeId': instance.predefinedMeditationTypeId,
  'meditationFileName': instance.meditationFileName,
  'location': instance.location,
  'fileName': instance.fileName,
  'fileSize': instance.fileSize,
  'userId': instance.userId,
};

MeditationFileModel _$MeditationFileModelFromJson(Map<String, dynamic> json) =>
    MeditationFileModel(
      meditationFileId: (json['meditationFileId'] as num?)?.toInt(),
      predefinedMeditationTypeId: (json['predefinedMeditationTypeId'] as num?)
          ?.toInt(),
      meditationFileName: json['meditationFileName'] as String?,
      location: json['location'] as String?,
      fileName: json['fileName'] as String?,
      fileSize: (json['fileSize'] as num?)?.toInt(),
      userId: (json['userId'] as num?)?.toInt(),
      isDeleted: json['isDeleted'] as bool?,
      createdDate: json['createdDate'] == null
          ? null
          : DateTime.parse(json['createdDate'] as String),
      createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
      modifiedDate: json['modifiedDate'] == null
          ? null
          : DateTime.parse(json['modifiedDate'] as String),
      modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
      predefinedMeditationTypeName:
          json['predefinedMeditationTypeName'] as String?,
      userName: json['userName'] as String?,
      createdByUserFirstName: json['createdByUserFirstName'] as String?,
      createdByUserLastName: json['createdByUserLastName'] as String?,
      createdByUserEmail: json['createdByUserEmail'] as String?,
      modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
      modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
      modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
      createdBy: json['createdBy'] as String?,
      modifiedBy: json['modifiedBy'] as String?,
    );

Map<String, dynamic> _$MeditationFileModelToJson(
  MeditationFileModel instance,
) => <String, dynamic>{
  'meditationFileId': instance.meditationFileId,
  'predefinedMeditationTypeId': instance.predefinedMeditationTypeId,
  'meditationFileName': instance.meditationFileName,
  'location': instance.location,
  'fileName': instance.fileName,
  'fileSize': instance.fileSize,
  'userId': instance.userId,
  'isDeleted': instance.isDeleted,
  'createdDate': instance.createdDate?.toIso8601String(),
  'createdByUserId': instance.createdByUserId,
  'modifiedDate': instance.modifiedDate?.toIso8601String(),
  'modifiedByUserId': instance.modifiedByUserId,
  'predefinedMeditationTypeName': instance.predefinedMeditationTypeName,
  'userName': instance.userName,
  'createdByUserFirstName': instance.createdByUserFirstName,
  'createdByUserLastName': instance.createdByUserLastName,
  'createdByUserEmail': instance.createdByUserEmail,
  'modifiedByUserFirstName': instance.modifiedByUserFirstName,
  'modifiedByUserLastName': instance.modifiedByUserLastName,
  'modifiedByUserEmail': instance.modifiedByUserEmail,
  'createdBy': instance.createdBy,
  'modifiedBy': instance.modifiedBy,
};

MeditationFileModelReturnListModel _$MeditationFileModelReturnListModelFromJson(
  Map<String, dynamic> json,
) => MeditationFileModelReturnListModel(
  model:
      (json['model'] as List<dynamic>?)
          ?.map((e) => MeditationFileModel.fromJson(e as Map<String, dynamic>))
          .toList() ??
      [],
  search: json['search'] == null
      ? null
      : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
  totalRecords: (json['totalRecords'] as num?)?.toInt(),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$MeditationFileModelReturnListModelToJson(
  MeditationFileModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

MeditationFileModelReturnModel _$MeditationFileModelReturnModelFromJson(
  Map<String, dynamic> json,
) => MeditationFileModelReturnModel(
  model: json['model'] == null
      ? null
      : MeditationFileModel.fromJson(json['model'] as Map<String, dynamic>),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$MeditationFileModelReturnModelToJson(
  MeditationFileModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

MeditationFileUpdateModel _$MeditationFileUpdateModelFromJson(
  Map<String, dynamic> json,
) => MeditationFileUpdateModel(
  meditationFileId: (json['meditationFileId'] as num?)?.toInt(),
  predefinedMeditationTypeId: (json['predefinedMeditationTypeId'] as num?)
      ?.toInt(),
  meditationFileName: json['meditationFileName'] as String?,
  location: json['location'] as String?,
  fileName: json['fileName'] as String?,
  fileSize: (json['fileSize'] as num?)?.toInt(),
  userId: (json['userId'] as num?)?.toInt(),
);

Map<String, dynamic> _$MeditationFileUpdateModelToJson(
  MeditationFileUpdateModel instance,
) => <String, dynamic>{
  'meditationFileId': instance.meditationFileId,
  'predefinedMeditationTypeId': instance.predefinedMeditationTypeId,
  'meditationFileName': instance.meditationFileName,
  'location': instance.location,
  'fileName': instance.fileName,
  'fileSize': instance.fileSize,
  'userId': instance.userId,
};

MeditationSessionByFeedIdSearchCriteria
_$MeditationSessionByFeedIdSearchCriteriaFromJson(Map<String, dynamic> json) =>
    MeditationSessionByFeedIdSearchCriteria(
      selectedId: (json['selectedId'] as num?)?.toInt(),
      page: (json['page'] as num?)?.toInt(),
      pageSize: (json['pageSize'] as num?)?.toInt(),
      keyword: json['keyword'] as String?,
      sortBy: json['sortBy'] as String?,
      sortAscending: json['sortAscending'] as bool?,
      feedId: (json['feedId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$MeditationSessionByFeedIdSearchCriteriaToJson(
  MeditationSessionByFeedIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'feedId': instance.feedId,
};

MeditationSessionByMeditationFileIdSearchCriteria
_$MeditationSessionByMeditationFileIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => MeditationSessionByMeditationFileIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  meditationFileId: (json['meditationFileId'] as num?)?.toInt(),
);

Map<String, dynamic> _$MeditationSessionByMeditationFileIdSearchCriteriaToJson(
  MeditationSessionByMeditationFileIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'meditationFileId': instance.meditationFileId,
};

MeditationSessionByUserIdSearchCriteria
_$MeditationSessionByUserIdSearchCriteriaFromJson(Map<String, dynamic> json) =>
    MeditationSessionByUserIdSearchCriteria(
      selectedId: (json['selectedId'] as num?)?.toInt(),
      page: (json['page'] as num?)?.toInt(),
      pageSize: (json['pageSize'] as num?)?.toInt(),
      keyword: json['keyword'] as String?,
      sortBy: json['sortBy'] as String?,
      sortAscending: json['sortAscending'] as bool?,
      userId: (json['userId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$MeditationSessionByUserIdSearchCriteriaToJson(
  MeditationSessionByUserIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'userId': instance.userId,
};

MeditationSessionCreateModel _$MeditationSessionCreateModelFromJson(
  Map<String, dynamic> json,
) => MeditationSessionCreateModel(
  meditationSessionDate: json['meditationSessionDate'] == null
      ? null
      : DateTime.parse(json['meditationSessionDate'] as String),
  meditationSessionTimeSpentInSeconds:
      (json['meditationSessionTimeSpentInSeconds'] as num?)?.toInt(),
  meditationFileId: (json['meditationFileId'] as num?)?.toInt(),
  userId: (json['userId'] as num?)?.toInt(),
  addedToFeed: json['addedToFeed'] as bool?,
  addedToFeedDate: json['addedToFeedDate'] == null
      ? null
      : DateTime.parse(json['addedToFeedDate'] as String),
);

Map<String, dynamic> _$MeditationSessionCreateModelToJson(
  MeditationSessionCreateModel instance,
) => <String, dynamic>{
  'meditationSessionDate': instance.meditationSessionDate?.toIso8601String(),
  'meditationSessionTimeSpentInSeconds':
      instance.meditationSessionTimeSpentInSeconds,
  'meditationFileId': instance.meditationFileId,
  'userId': instance.userId,
  'addedToFeed': instance.addedToFeed,
  'addedToFeedDate': instance.addedToFeedDate?.toIso8601String(),
};

MeditationSessionModel _$MeditationSessionModelFromJson(
  Map<String, dynamic> json,
) => MeditationSessionModel(
  meditationSessionId: (json['meditationSessionId'] as num?)?.toInt(),
  meditationSessionDate: json['meditationSessionDate'] == null
      ? null
      : DateTime.parse(json['meditationSessionDate'] as String),
  meditationSessionTimeSpentInSeconds:
      (json['meditationSessionTimeSpentInSeconds'] as num?)?.toInt(),
  meditationFileId: (json['meditationFileId'] as num?)?.toInt(),
  userId: (json['userId'] as num?)?.toInt(),
  addedToFeed: json['addedToFeed'] as bool?,
  addedToFeedDate: json['addedToFeedDate'] == null
      ? null
      : DateTime.parse(json['addedToFeedDate'] as String),
  isDeleted: json['isDeleted'] as bool?,
  createdDate: json['createdDate'] == null
      ? null
      : DateTime.parse(json['createdDate'] as String),
  createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
  modifiedDate: json['modifiedDate'] == null
      ? null
      : DateTime.parse(json['modifiedDate'] as String),
  modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
  meditationFileName: json['meditationFileName'] as String?,
  userName: json['userName'] as String?,
  createdByUserFirstName: json['createdByUserFirstName'] as String?,
  createdByUserLastName: json['createdByUserLastName'] as String?,
  createdByUserEmail: json['createdByUserEmail'] as String?,
  modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
  modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
  modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
  createdBy: json['createdBy'] as String?,
  modifiedBy: json['modifiedBy'] as String?,
);

Map<String, dynamic> _$MeditationSessionModelToJson(
  MeditationSessionModel instance,
) => <String, dynamic>{
  'meditationSessionId': instance.meditationSessionId,
  'meditationSessionDate': instance.meditationSessionDate?.toIso8601String(),
  'meditationSessionTimeSpentInSeconds':
      instance.meditationSessionTimeSpentInSeconds,
  'meditationFileId': instance.meditationFileId,
  'userId': instance.userId,
  'addedToFeed': instance.addedToFeed,
  'addedToFeedDate': instance.addedToFeedDate?.toIso8601String(),
  'isDeleted': instance.isDeleted,
  'createdDate': instance.createdDate?.toIso8601String(),
  'createdByUserId': instance.createdByUserId,
  'modifiedDate': instance.modifiedDate?.toIso8601String(),
  'modifiedByUserId': instance.modifiedByUserId,
  'meditationFileName': instance.meditationFileName,
  'userName': instance.userName,
  'createdByUserFirstName': instance.createdByUserFirstName,
  'createdByUserLastName': instance.createdByUserLastName,
  'createdByUserEmail': instance.createdByUserEmail,
  'modifiedByUserFirstName': instance.modifiedByUserFirstName,
  'modifiedByUserLastName': instance.modifiedByUserLastName,
  'modifiedByUserEmail': instance.modifiedByUserEmail,
  'createdBy': instance.createdBy,
  'modifiedBy': instance.modifiedBy,
};

MeditationSessionModelReturnListModel
_$MeditationSessionModelReturnListModelFromJson(Map<String, dynamic> json) =>
    MeditationSessionModelReturnListModel(
      model:
          (json['model'] as List<dynamic>?)
              ?.map(
                (e) =>
                    MeditationSessionModel.fromJson(e as Map<String, dynamic>),
              )
              .toList() ??
          [],
      search: json['search'] == null
          ? null
          : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
      totalRecords: (json['totalRecords'] as num?)?.toInt(),
      isSuccess: json['isSuccess'] as bool?,
      successMessage: json['successMessage'] as String?,
      errorHolder: json['errorHolder'] == null
          ? null
          : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
      activityId: json['activityId'] as String?,
      activityDateTime: json['activityDateTime'] == null
          ? null
          : DateTime.parse(json['activityDateTime'] as String),
      server: json['server'] as String?,
      processId: (json['processId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$MeditationSessionModelReturnListModelToJson(
  MeditationSessionModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

MeditationSessionModelReturnModel _$MeditationSessionModelReturnModelFromJson(
  Map<String, dynamic> json,
) => MeditationSessionModelReturnModel(
  model: json['model'] == null
      ? null
      : MeditationSessionModel.fromJson(json['model'] as Map<String, dynamic>),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$MeditationSessionModelReturnModelToJson(
  MeditationSessionModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

MeditationSessionUpdateModel _$MeditationSessionUpdateModelFromJson(
  Map<String, dynamic> json,
) => MeditationSessionUpdateModel(
  meditationSessionId: (json['meditationSessionId'] as num?)?.toInt(),
  meditationSessionDate: json['meditationSessionDate'] == null
      ? null
      : DateTime.parse(json['meditationSessionDate'] as String),
  meditationSessionTimeSpentInSeconds:
      (json['meditationSessionTimeSpentInSeconds'] as num?)?.toInt(),
  meditationFileId: (json['meditationFileId'] as num?)?.toInt(),
  userId: (json['userId'] as num?)?.toInt(),
  addedToFeed: json['addedToFeed'] as bool?,
  addedToFeedDate: json['addedToFeedDate'] == null
      ? null
      : DateTime.parse(json['addedToFeedDate'] as String),
);

Map<String, dynamic> _$MeditationSessionUpdateModelToJson(
  MeditationSessionUpdateModel instance,
) => <String, dynamic>{
  'meditationSessionId': instance.meditationSessionId,
  'meditationSessionDate': instance.meditationSessionDate?.toIso8601String(),
  'meditationSessionTimeSpentInSeconds':
      instance.meditationSessionTimeSpentInSeconds,
  'meditationFileId': instance.meditationFileId,
  'userId': instance.userId,
  'addedToFeed': instance.addedToFeed,
  'addedToFeedDate': instance.addedToFeedDate?.toIso8601String(),
};

MessageTemplateCreateModel _$MessageTemplateCreateModelFromJson(
  Map<String, dynamic> json,
) => MessageTemplateCreateModel(
  predefinedMessageTemplateTypeId:
      (json['predefinedMessageTemplateTypeId'] as num?)?.toInt(),
  emailSubject: json['emailSubject'] as String?,
  emailBody: json['emailBody'] as String?,
  smsBody: json['smsBody'] as String?,
  appNotificationSubject: json['appNotificationSubject'] as String?,
  appNotificationBody: json['appNotificationBody'] as String?,
  webNotificationSubject: json['webNotificationSubject'] as String?,
  webNotificationBody: json['webNotificationBody'] as String?,
  sendEmailForInternalUser: json['sendEmailForInternalUser'] as bool?,
  sendSmsForInternalUser: json['sendSmsForInternalUser'] as bool?,
  sendAppNotificationForInternalUser:
      json['sendAppNotificationForInternalUser'] as bool?,
  sendEmailForExternalUser: json['sendEmailForExternalUser'] as bool?,
  sendSmsForExternalUser: json['sendSmsForExternalUser'] as bool?,
  sendAppNotificationForExternalUser:
      json['sendAppNotificationForExternalUser'] as bool?,
  additionalRecipients: json['additionalRecipients'] as String?,
);

Map<String, dynamic> _$MessageTemplateCreateModelToJson(
  MessageTemplateCreateModel instance,
) => <String, dynamic>{
  'predefinedMessageTemplateTypeId': instance.predefinedMessageTemplateTypeId,
  'emailSubject': instance.emailSubject,
  'emailBody': instance.emailBody,
  'smsBody': instance.smsBody,
  'appNotificationSubject': instance.appNotificationSubject,
  'appNotificationBody': instance.appNotificationBody,
  'webNotificationSubject': instance.webNotificationSubject,
  'webNotificationBody': instance.webNotificationBody,
  'sendEmailForInternalUser': instance.sendEmailForInternalUser,
  'sendSmsForInternalUser': instance.sendSmsForInternalUser,
  'sendAppNotificationForInternalUser':
      instance.sendAppNotificationForInternalUser,
  'sendEmailForExternalUser': instance.sendEmailForExternalUser,
  'sendSmsForExternalUser': instance.sendSmsForExternalUser,
  'sendAppNotificationForExternalUser':
      instance.sendAppNotificationForExternalUser,
  'additionalRecipients': instance.additionalRecipients,
};

MessageTemplateModel _$MessageTemplateModelFromJson(
  Map<String, dynamic> json,
) => MessageTemplateModel(
  messageTemplateId: (json['messageTemplateId'] as num?)?.toInt(),
  predefinedMessageTemplateTypeId:
      (json['predefinedMessageTemplateTypeId'] as num?)?.toInt(),
  emailSubject: json['emailSubject'] as String?,
  emailBody: json['emailBody'] as String?,
  smsBody: json['smsBody'] as String?,
  appNotificationSubject: json['appNotificationSubject'] as String?,
  appNotificationBody: json['appNotificationBody'] as String?,
  webNotificationSubject: json['webNotificationSubject'] as String?,
  webNotificationBody: json['webNotificationBody'] as String?,
  sendEmailForInternalUser: json['sendEmailForInternalUser'] as bool?,
  sendSmsForInternalUser: json['sendSmsForInternalUser'] as bool?,
  sendAppNotificationForInternalUser:
      json['sendAppNotificationForInternalUser'] as bool?,
  sendEmailForExternalUser: json['sendEmailForExternalUser'] as bool?,
  sendSmsForExternalUser: json['sendSmsForExternalUser'] as bool?,
  sendAppNotificationForExternalUser:
      json['sendAppNotificationForExternalUser'] as bool?,
  additionalRecipients: json['additionalRecipients'] as String?,
  isDeleted: json['isDeleted'] as bool?,
  createdDate: json['createdDate'] == null
      ? null
      : DateTime.parse(json['createdDate'] as String),
  createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
  modifiedDate: json['modifiedDate'] == null
      ? null
      : DateTime.parse(json['modifiedDate'] as String),
  modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
  predefinedMessageTemplateTypeName:
      json['predefinedMessageTemplateTypeName'] as String?,
  createdByUserFirstName: json['createdByUserFirstName'] as String?,
  createdByUserLastName: json['createdByUserLastName'] as String?,
  createdByUserEmail: json['createdByUserEmail'] as String?,
  modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
  modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
  modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
  createdBy: json['createdBy'] as String?,
  modifiedBy: json['modifiedBy'] as String?,
);

Map<String, dynamic> _$MessageTemplateModelToJson(
  MessageTemplateModel instance,
) => <String, dynamic>{
  'messageTemplateId': instance.messageTemplateId,
  'predefinedMessageTemplateTypeId': instance.predefinedMessageTemplateTypeId,
  'emailSubject': instance.emailSubject,
  'emailBody': instance.emailBody,
  'smsBody': instance.smsBody,
  'appNotificationSubject': instance.appNotificationSubject,
  'appNotificationBody': instance.appNotificationBody,
  'webNotificationSubject': instance.webNotificationSubject,
  'webNotificationBody': instance.webNotificationBody,
  'sendEmailForInternalUser': instance.sendEmailForInternalUser,
  'sendSmsForInternalUser': instance.sendSmsForInternalUser,
  'sendAppNotificationForInternalUser':
      instance.sendAppNotificationForInternalUser,
  'sendEmailForExternalUser': instance.sendEmailForExternalUser,
  'sendSmsForExternalUser': instance.sendSmsForExternalUser,
  'sendAppNotificationForExternalUser':
      instance.sendAppNotificationForExternalUser,
  'additionalRecipients': instance.additionalRecipients,
  'isDeleted': instance.isDeleted,
  'createdDate': instance.createdDate?.toIso8601String(),
  'createdByUserId': instance.createdByUserId,
  'modifiedDate': instance.modifiedDate?.toIso8601String(),
  'modifiedByUserId': instance.modifiedByUserId,
  'predefinedMessageTemplateTypeName':
      instance.predefinedMessageTemplateTypeName,
  'createdByUserFirstName': instance.createdByUserFirstName,
  'createdByUserLastName': instance.createdByUserLastName,
  'createdByUserEmail': instance.createdByUserEmail,
  'modifiedByUserFirstName': instance.modifiedByUserFirstName,
  'modifiedByUserLastName': instance.modifiedByUserLastName,
  'modifiedByUserEmail': instance.modifiedByUserEmail,
  'createdBy': instance.createdBy,
  'modifiedBy': instance.modifiedBy,
};

MessageTemplateModelReturnListModel
_$MessageTemplateModelReturnListModelFromJson(Map<String, dynamic> json) =>
    MessageTemplateModelReturnListModel(
      model:
          (json['model'] as List<dynamic>?)
              ?.map(
                (e) => MessageTemplateModel.fromJson(e as Map<String, dynamic>),
              )
              .toList() ??
          [],
      search: json['search'] == null
          ? null
          : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
      totalRecords: (json['totalRecords'] as num?)?.toInt(),
      isSuccess: json['isSuccess'] as bool?,
      successMessage: json['successMessage'] as String?,
      errorHolder: json['errorHolder'] == null
          ? null
          : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
      activityId: json['activityId'] as String?,
      activityDateTime: json['activityDateTime'] == null
          ? null
          : DateTime.parse(json['activityDateTime'] as String),
      server: json['server'] as String?,
      processId: (json['processId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$MessageTemplateModelReturnListModelToJson(
  MessageTemplateModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

MessageTemplateModelReturnModel _$MessageTemplateModelReturnModelFromJson(
  Map<String, dynamic> json,
) => MessageTemplateModelReturnModel(
  model: json['model'] == null
      ? null
      : MessageTemplateModel.fromJson(json['model'] as Map<String, dynamic>),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$MessageTemplateModelReturnModelToJson(
  MessageTemplateModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

MessageTemplateUpdateModel _$MessageTemplateUpdateModelFromJson(
  Map<String, dynamic> json,
) => MessageTemplateUpdateModel(
  messageTemplateId: (json['messageTemplateId'] as num?)?.toInt(),
  predefinedMessageTemplateTypeId:
      (json['predefinedMessageTemplateTypeId'] as num?)?.toInt(),
  emailSubject: json['emailSubject'] as String?,
  emailBody: json['emailBody'] as String?,
  smsBody: json['smsBody'] as String?,
  appNotificationSubject: json['appNotificationSubject'] as String?,
  appNotificationBody: json['appNotificationBody'] as String?,
  webNotificationSubject: json['webNotificationSubject'] as String?,
  webNotificationBody: json['webNotificationBody'] as String?,
  sendEmailForInternalUser: json['sendEmailForInternalUser'] as bool?,
  sendSmsForInternalUser: json['sendSmsForInternalUser'] as bool?,
  sendAppNotificationForInternalUser:
      json['sendAppNotificationForInternalUser'] as bool?,
  sendEmailForExternalUser: json['sendEmailForExternalUser'] as bool?,
  sendSmsForExternalUser: json['sendSmsForExternalUser'] as bool?,
  sendAppNotificationForExternalUser:
      json['sendAppNotificationForExternalUser'] as bool?,
  additionalRecipients: json['additionalRecipients'] as String?,
);

Map<String, dynamic> _$MessageTemplateUpdateModelToJson(
  MessageTemplateUpdateModel instance,
) => <String, dynamic>{
  'messageTemplateId': instance.messageTemplateId,
  'predefinedMessageTemplateTypeId': instance.predefinedMessageTemplateTypeId,
  'emailSubject': instance.emailSubject,
  'emailBody': instance.emailBody,
  'smsBody': instance.smsBody,
  'appNotificationSubject': instance.appNotificationSubject,
  'appNotificationBody': instance.appNotificationBody,
  'webNotificationSubject': instance.webNotificationSubject,
  'webNotificationBody': instance.webNotificationBody,
  'sendEmailForInternalUser': instance.sendEmailForInternalUser,
  'sendSmsForInternalUser': instance.sendSmsForInternalUser,
  'sendAppNotificationForInternalUser':
      instance.sendAppNotificationForInternalUser,
  'sendEmailForExternalUser': instance.sendEmailForExternalUser,
  'sendSmsForExternalUser': instance.sendSmsForExternalUser,
  'sendAppNotificationForExternalUser':
      instance.sendAppNotificationForExternalUser,
  'additionalRecipients': instance.additionalRecipients,
};

MessageToSendCreateModel _$MessageToSendCreateModelFromJson(
  Map<String, dynamic> json,
) => MessageToSendCreateModel(
  predefinedMessageToSendTypeId: (json['predefinedMessageToSendTypeId'] as num?)
      ?.toInt(),
  realtimeMessageId: (json['realtimeMessageId'] as num?)?.toInt(),
  messageFrom: json['messageFrom'] as String?,
  messageTo: json['messageTo'] as String?,
  messageFromName: json['messageFromName'] as String?,
  messageToName: json['messageToName'] as String?,
  messageSubject: json['messageSubject'] as String?,
  messageBody: json['messageBody'] as String?,
  sentDate: json['sentDate'] == null
      ? null
      : DateTime.parse(json['sentDate'] as String),
  sent: json['sent'] as bool?,
  isSentError: json['isSentError'] as bool?,
  sentErrorMessage: json['sentErrorMessage'] as String?,
  sentErrorDate: json['sentErrorDate'] == null
      ? null
      : DateTime.parse(json['sentErrorDate'] as String),
  retryCount: (json['retryCount'] as num?)?.toInt(),
);

Map<String, dynamic> _$MessageToSendCreateModelToJson(
  MessageToSendCreateModel instance,
) => <String, dynamic>{
  'predefinedMessageToSendTypeId': instance.predefinedMessageToSendTypeId,
  'realtimeMessageId': instance.realtimeMessageId,
  'messageFrom': instance.messageFrom,
  'messageTo': instance.messageTo,
  'messageFromName': instance.messageFromName,
  'messageToName': instance.messageToName,
  'messageSubject': instance.messageSubject,
  'messageBody': instance.messageBody,
  'sentDate': instance.sentDate?.toIso8601String(),
  'sent': instance.sent,
  'isSentError': instance.isSentError,
  'sentErrorMessage': instance.sentErrorMessage,
  'sentErrorDate': instance.sentErrorDate?.toIso8601String(),
  'retryCount': instance.retryCount,
};

MessageToSendModel _$MessageToSendModelFromJson(Map<String, dynamic> json) =>
    MessageToSendModel(
      messageToSendId: (json['messageToSendId'] as num?)?.toInt(),
      predefinedMessageToSendTypeId:
          (json['predefinedMessageToSendTypeId'] as num?)?.toInt(),
      realtimeMessageId: (json['realtimeMessageId'] as num?)?.toInt(),
      messageFrom: json['messageFrom'] as String?,
      messageTo: json['messageTo'] as String?,
      messageFromName: json['messageFromName'] as String?,
      messageToName: json['messageToName'] as String?,
      messageSubject: json['messageSubject'] as String?,
      messageBody: json['messageBody'] as String?,
      sentDate: json['sentDate'] == null
          ? null
          : DateTime.parse(json['sentDate'] as String),
      sent: json['sent'] as bool?,
      isSentError: json['isSentError'] as bool?,
      sentErrorMessage: json['sentErrorMessage'] as String?,
      sentErrorDate: json['sentErrorDate'] == null
          ? null
          : DateTime.parse(json['sentErrorDate'] as String),
      retryCount: (json['retryCount'] as num?)?.toInt(),
      isDeleted: json['isDeleted'] as bool?,
      createdDate: json['createdDate'] == null
          ? null
          : DateTime.parse(json['createdDate'] as String),
      createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
      modifiedDate: json['modifiedDate'] == null
          ? null
          : DateTime.parse(json['modifiedDate'] as String),
      modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
      predefinedMessageToSendTypeName:
          json['predefinedMessageToSendTypeName'] as String?,
      createdByUserFirstName: json['createdByUserFirstName'] as String?,
      createdByUserLastName: json['createdByUserLastName'] as String?,
      createdByUserEmail: json['createdByUserEmail'] as String?,
      modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
      modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
      modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
      createdBy: json['createdBy'] as String?,
      modifiedBy: json['modifiedBy'] as String?,
    );

Map<String, dynamic> _$MessageToSendModelToJson(
  MessageToSendModel instance,
) => <String, dynamic>{
  'messageToSendId': instance.messageToSendId,
  'predefinedMessageToSendTypeId': instance.predefinedMessageToSendTypeId,
  'realtimeMessageId': instance.realtimeMessageId,
  'messageFrom': instance.messageFrom,
  'messageTo': instance.messageTo,
  'messageFromName': instance.messageFromName,
  'messageToName': instance.messageToName,
  'messageSubject': instance.messageSubject,
  'messageBody': instance.messageBody,
  'sentDate': instance.sentDate?.toIso8601String(),
  'sent': instance.sent,
  'isSentError': instance.isSentError,
  'sentErrorMessage': instance.sentErrorMessage,
  'sentErrorDate': instance.sentErrorDate?.toIso8601String(),
  'retryCount': instance.retryCount,
  'isDeleted': instance.isDeleted,
  'createdDate': instance.createdDate?.toIso8601String(),
  'createdByUserId': instance.createdByUserId,
  'modifiedDate': instance.modifiedDate?.toIso8601String(),
  'modifiedByUserId': instance.modifiedByUserId,
  'predefinedMessageToSendTypeName': instance.predefinedMessageToSendTypeName,
  'createdByUserFirstName': instance.createdByUserFirstName,
  'createdByUserLastName': instance.createdByUserLastName,
  'createdByUserEmail': instance.createdByUserEmail,
  'modifiedByUserFirstName': instance.modifiedByUserFirstName,
  'modifiedByUserLastName': instance.modifiedByUserLastName,
  'modifiedByUserEmail': instance.modifiedByUserEmail,
  'createdBy': instance.createdBy,
  'modifiedBy': instance.modifiedBy,
};

MessageToSendModelReturnListModel _$MessageToSendModelReturnListModelFromJson(
  Map<String, dynamic> json,
) => MessageToSendModelReturnListModel(
  model:
      (json['model'] as List<dynamic>?)
          ?.map((e) => MessageToSendModel.fromJson(e as Map<String, dynamic>))
          .toList() ??
      [],
  search: json['search'] == null
      ? null
      : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
  totalRecords: (json['totalRecords'] as num?)?.toInt(),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$MessageToSendModelReturnListModelToJson(
  MessageToSendModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

MessageToSendModelReturnModel _$MessageToSendModelReturnModelFromJson(
  Map<String, dynamic> json,
) => MessageToSendModelReturnModel(
  model: json['model'] == null
      ? null
      : MessageToSendModel.fromJson(json['model'] as Map<String, dynamic>),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$MessageToSendModelReturnModelToJson(
  MessageToSendModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

MinimalUserProfileModel _$MinimalUserProfileModelFromJson(
  Map<String, dynamic> json,
) => MinimalUserProfileModel(
  userId: (json['userId'] as num?)?.toInt(),
  email: json['email'] as String?,
  firstName: json['firstName'] as String?,
  lastName: json['lastName'] as String?,
);

Map<String, dynamic> _$MinimalUserProfileModelToJson(
  MinimalUserProfileModel instance,
) => <String, dynamic>{
  'userId': instance.userId,
  'email': instance.email,
  'firstName': instance.firstName,
  'lastName': instance.lastName,
};

MultipleCategoryCreateModel _$MultipleCategoryCreateModelFromJson(
  Map<String, dynamic> json,
) => MultipleCategoryCreateModel(
  newCategorys:
      (json['newCategorys'] as List<dynamic>?)
          ?.map((e) => CategoryCreateModel.fromJson(e as Map<String, dynamic>))
          .toList() ??
      [],
);

Map<String, dynamic> _$MultipleCategoryCreateModelToJson(
  MultipleCategoryCreateModel instance,
) => <String, dynamic>{
  'newCategorys': instance.newCategorys?.map((e) => e.toJson()).toList(),
};

MultipleChatCreateModel _$MultipleChatCreateModelFromJson(
  Map<String, dynamic> json,
) => MultipleChatCreateModel(
  newChats:
      (json['newChats'] as List<dynamic>?)
          ?.map((e) => ChatCreateModel.fromJson(e as Map<String, dynamic>))
          .toList() ??
      [],
);

Map<String, dynamic> _$MultipleChatCreateModelToJson(
  MultipleChatCreateModel instance,
) => <String, dynamic>{
  'newChats': instance.newChats?.map((e) => e.toJson()).toList(),
};

MultipleChatMessageCreateModel _$MultipleChatMessageCreateModelFromJson(
  Map<String, dynamic> json,
) => MultipleChatMessageCreateModel(
  newChatMessages:
      (json['newChatMessages'] as List<dynamic>?)
          ?.map(
            (e) => ChatMessageCreateModel.fromJson(e as Map<String, dynamic>),
          )
          .toList() ??
      [],
);

Map<String, dynamic> _$MultipleChatMessageCreateModelToJson(
  MultipleChatMessageCreateModel instance,
) => <String, dynamic>{
  'newChatMessages': instance.newChatMessages?.map((e) => e.toJson()).toList(),
};

MultipleChatMessageStatusCreateModel
_$MultipleChatMessageStatusCreateModelFromJson(Map<String, dynamic> json) =>
    MultipleChatMessageStatusCreateModel(
      newChatMessageStatuss:
          (json['newChatMessageStatuss'] as List<dynamic>?)
              ?.map(
                (e) => ChatMessageStatusCreateModel.fromJson(
                  e as Map<String, dynamic>,
                ),
              )
              .toList() ??
          [],
    );

Map<String, dynamic> _$MultipleChatMessageStatusCreateModelToJson(
  MultipleChatMessageStatusCreateModel instance,
) => <String, dynamic>{
  'newChatMessageStatuss': instance.newChatMessageStatuss
      ?.map((e) => e.toJson())
      .toList(),
};

MultipleChatUserCreateModel _$MultipleChatUserCreateModelFromJson(
  Map<String, dynamic> json,
) => MultipleChatUserCreateModel(
  newChatUsers:
      (json['newChatUsers'] as List<dynamic>?)
          ?.map((e) => ChatUserCreateModel.fromJson(e as Map<String, dynamic>))
          .toList() ??
      [],
);

Map<String, dynamic> _$MultipleChatUserCreateModelToJson(
  MultipleChatUserCreateModel instance,
) => <String, dynamic>{
  'newChatUsers': instance.newChatUsers?.map((e) => e.toJson()).toList(),
};

MultipleDailyAffirmationSessionCategoryCreateModel
_$MultipleDailyAffirmationSessionCategoryCreateModelFromJson(
  Map<String, dynamic> json,
) => MultipleDailyAffirmationSessionCategoryCreateModel(
  newDailyAffirmationSessionCategorys:
      (json['newDailyAffirmationSessionCategorys'] as List<dynamic>?)
          ?.map(
            (e) => DailyAffirmationSessionCategoryCreateModel.fromJson(
              e as Map<String, dynamic>,
            ),
          )
          .toList() ??
      [],
);

Map<String, dynamic> _$MultipleDailyAffirmationSessionCategoryCreateModelToJson(
  MultipleDailyAffirmationSessionCategoryCreateModel instance,
) => <String, dynamic>{
  'newDailyAffirmationSessionCategorys': instance
      .newDailyAffirmationSessionCategorys
      ?.map((e) => e.toJson())
      .toList(),
};

MultipleDailyAffirmationSessionCreateModel
_$MultipleDailyAffirmationSessionCreateModelFromJson(
  Map<String, dynamic> json,
) => MultipleDailyAffirmationSessionCreateModel(
  newDailyAffirmationSessions:
      (json['newDailyAffirmationSessions'] as List<dynamic>?)
          ?.map(
            (e) => DailyAffirmationSessionCreateModel.fromJson(
              e as Map<String, dynamic>,
            ),
          )
          .toList() ??
      [],
);

Map<String, dynamic> _$MultipleDailyAffirmationSessionCreateModelToJson(
  MultipleDailyAffirmationSessionCreateModel instance,
) => <String, dynamic>{
  'newDailyAffirmationSessions': instance.newDailyAffirmationSessions
      ?.map((e) => e.toJson())
      .toList(),
};

MultipleDailyAffirmationTextCreateModel
_$MultipleDailyAffirmationTextCreateModelFromJson(Map<String, dynamic> json) =>
    MultipleDailyAffirmationTextCreateModel(
      newDailyAffirmationTexts:
          (json['newDailyAffirmationTexts'] as List<dynamic>?)
              ?.map(
                (e) => DailyAffirmationTextCreateModel.fromJson(
                  e as Map<String, dynamic>,
                ),
              )
              .toList() ??
          [],
    );

Map<String, dynamic> _$MultipleDailyAffirmationTextCreateModelToJson(
  MultipleDailyAffirmationTextCreateModel instance,
) => <String, dynamic>{
  'newDailyAffirmationTexts': instance.newDailyAffirmationTexts
      ?.map((e) => e.toJson())
      .toList(),
};

MultipleDailyJournalCreateModel _$MultipleDailyJournalCreateModelFromJson(
  Map<String, dynamic> json,
) => MultipleDailyJournalCreateModel(
  newDailyJournals:
      (json['newDailyJournals'] as List<dynamic>?)
          ?.map(
            (e) => DailyJournalCreateModel.fromJson(e as Map<String, dynamic>),
          )
          .toList() ??
      [],
);

Map<String, dynamic> _$MultipleDailyJournalCreateModelToJson(
  MultipleDailyJournalCreateModel instance,
) => <String, dynamic>{
  'newDailyJournals': instance.newDailyJournals
      ?.map((e) => e.toJson())
      .toList(),
};

MultipleDefaultCategoryCreateModel _$MultipleDefaultCategoryCreateModelFromJson(
  Map<String, dynamic> json,
) => MultipleDefaultCategoryCreateModel(
  newDefaultCategorys:
      (json['newDefaultCategorys'] as List<dynamic>?)
          ?.map(
            (e) =>
                DefaultCategoryCreateModel.fromJson(e as Map<String, dynamic>),
          )
          .toList() ??
      [],
);

Map<String, dynamic> _$MultipleDefaultCategoryCreateModelToJson(
  MultipleDefaultCategoryCreateModel instance,
) => <String, dynamic>{
  'newDefaultCategorys': instance.newDefaultCategorys
      ?.map((e) => e.toJson())
      .toList(),
};

MultipleDefaultDailyAffirmationTextCreateModel
_$MultipleDefaultDailyAffirmationTextCreateModelFromJson(
  Map<String, dynamic> json,
) => MultipleDefaultDailyAffirmationTextCreateModel(
  newDefaultDailyAffirmationTexts:
      (json['newDefaultDailyAffirmationTexts'] as List<dynamic>?)
          ?.map(
            (e) => DefaultDailyAffirmationTextCreateModel.fromJson(
              e as Map<String, dynamic>,
            ),
          )
          .toList() ??
      [],
);

Map<String, dynamic> _$MultipleDefaultDailyAffirmationTextCreateModelToJson(
  MultipleDefaultDailyAffirmationTextCreateModel instance,
) => <String, dynamic>{
  'newDefaultDailyAffirmationTexts': instance.newDefaultDailyAffirmationTexts
      ?.map((e) => e.toJson())
      .toList(),
};

MultipleDefaultGoalTrackerCategoryCreateModel
_$MultipleDefaultGoalTrackerCategoryCreateModelFromJson(
  Map<String, dynamic> json,
) => MultipleDefaultGoalTrackerCategoryCreateModel(
  newDefaultGoalTrackerCategorys:
      (json['newDefaultGoalTrackerCategorys'] as List<dynamic>?)
          ?.map(
            (e) => DefaultGoalTrackerCategoryCreateModel.fromJson(
              e as Map<String, dynamic>,
            ),
          )
          .toList() ??
      [],
);

Map<String, dynamic> _$MultipleDefaultGoalTrackerCategoryCreateModelToJson(
  MultipleDefaultGoalTrackerCategoryCreateModel instance,
) => <String, dynamic>{
  'newDefaultGoalTrackerCategorys': instance.newDefaultGoalTrackerCategorys
      ?.map((e) => e.toJson())
      .toList(),
};

MultipleDefaultGoalTrackerCreateModel
_$MultipleDefaultGoalTrackerCreateModelFromJson(Map<String, dynamic> json) =>
    MultipleDefaultGoalTrackerCreateModel(
      newDefaultGoalTrackers:
          (json['newDefaultGoalTrackers'] as List<dynamic>?)
              ?.map(
                (e) => DefaultGoalTrackerCreateModel.fromJson(
                  e as Map<String, dynamic>,
                ),
              )
              .toList() ??
          [],
    );

Map<String, dynamic> _$MultipleDefaultGoalTrackerCreateModelToJson(
  MultipleDefaultGoalTrackerCreateModel instance,
) => <String, dynamic>{
  'newDefaultGoalTrackers': instance.newDefaultGoalTrackers
      ?.map((e) => e.toJson())
      .toList(),
};

MultipleDefaultMeditationFileCreateModel
_$MultipleDefaultMeditationFileCreateModelFromJson(Map<String, dynamic> json) =>
    MultipleDefaultMeditationFileCreateModel(
      newDefaultMeditationFiles:
          (json['newDefaultMeditationFiles'] as List<dynamic>?)
              ?.map(
                (e) => DefaultMeditationFileCreateModel.fromJson(
                  e as Map<String, dynamic>,
                ),
              )
              .toList() ??
          [],
    );

Map<String, dynamic> _$MultipleDefaultMeditationFileCreateModelToJson(
  MultipleDefaultMeditationFileCreateModel instance,
) => <String, dynamic>{
  'newDefaultMeditationFiles': instance.newDefaultMeditationFiles
      ?.map((e) => e.toJson())
      .toList(),
};

MultipleDefaultVisionBoardPictureCreateModel
_$MultipleDefaultVisionBoardPictureCreateModelFromJson(
  Map<String, dynamic> json,
) => MultipleDefaultVisionBoardPictureCreateModel(
  newDefaultVisionBoardPictures:
      (json['newDefaultVisionBoardPictures'] as List<dynamic>?)
          ?.map(
            (e) => DefaultVisionBoardPictureCreateModel.fromJson(
              e as Map<String, dynamic>,
            ),
          )
          .toList() ??
      [],
);

Map<String, dynamic> _$MultipleDefaultVisionBoardPictureCreateModelToJson(
  MultipleDefaultVisionBoardPictureCreateModel instance,
) => <String, dynamic>{
  'newDefaultVisionBoardPictures': instance.newDefaultVisionBoardPictures
      ?.map((e) => e.toJson())
      .toList(),
};

MultipleFeedCreateModel _$MultipleFeedCreateModelFromJson(
  Map<String, dynamic> json,
) => MultipleFeedCreateModel(
  newFeeds:
      (json['newFeeds'] as List<dynamic>?)
          ?.map((e) => FeedCreateModel.fromJson(e as Map<String, dynamic>))
          .toList() ??
      [],
);

Map<String, dynamic> _$MultipleFeedCreateModelToJson(
  MultipleFeedCreateModel instance,
) => <String, dynamic>{
  'newFeeds': instance.newFeeds?.map((e) => e.toJson()).toList(),
};

MultipleFileAttachmentFolderCreateModel
_$MultipleFileAttachmentFolderCreateModelFromJson(Map<String, dynamic> json) =>
    MultipleFileAttachmentFolderCreateModel(
      newFileAttachmentFolders:
          (json['newFileAttachmentFolders'] as List<dynamic>?)
              ?.map(
                (e) => FileAttachmentFolderCreateModel.fromJson(
                  e as Map<String, dynamic>,
                ),
              )
              .toList() ??
          [],
    );

Map<String, dynamic> _$MultipleFileAttachmentFolderCreateModelToJson(
  MultipleFileAttachmentFolderCreateModel instance,
) => <String, dynamic>{
  'newFileAttachmentFolders': instance.newFileAttachmentFolders
      ?.map((e) => e.toJson())
      .toList(),
};

MultipleGoalTrackerCategoryCreateModel
_$MultipleGoalTrackerCategoryCreateModelFromJson(Map<String, dynamic> json) =>
    MultipleGoalTrackerCategoryCreateModel(
      newGoalTrackerCategorys:
          (json['newGoalTrackerCategorys'] as List<dynamic>?)
              ?.map(
                (e) => GoalTrackerCategoryCreateModel.fromJson(
                  e as Map<String, dynamic>,
                ),
              )
              .toList() ??
          [],
    );

Map<String, dynamic> _$MultipleGoalTrackerCategoryCreateModelToJson(
  MultipleGoalTrackerCategoryCreateModel instance,
) => <String, dynamic>{
  'newGoalTrackerCategorys': instance.newGoalTrackerCategorys
      ?.map((e) => e.toJson())
      .toList(),
};

MultipleGoalTrackerCreateModel _$MultipleGoalTrackerCreateModelFromJson(
  Map<String, dynamic> json,
) => MultipleGoalTrackerCreateModel(
  newGoalTrackers:
      (json['newGoalTrackers'] as List<dynamic>?)
          ?.map(
            (e) => GoalTrackerCreateModel.fromJson(e as Map<String, dynamic>),
          )
          .toList() ??
      [],
);

Map<String, dynamic> _$MultipleGoalTrackerCreateModelToJson(
  MultipleGoalTrackerCreateModel instance,
) => <String, dynamic>{
  'newGoalTrackers': instance.newGoalTrackers?.map((e) => e.toJson()).toList(),
};

MultipleGoalTrackerMileStoneCreateModel
_$MultipleGoalTrackerMileStoneCreateModelFromJson(Map<String, dynamic> json) =>
    MultipleGoalTrackerMileStoneCreateModel(
      newGoalTrackerMileStones:
          (json['newGoalTrackerMileStones'] as List<dynamic>?)
              ?.map(
                (e) => GoalTrackerMileStoneCreateModel.fromJson(
                  e as Map<String, dynamic>,
                ),
              )
              .toList() ??
          [],
    );

Map<String, dynamic> _$MultipleGoalTrackerMileStoneCreateModelToJson(
  MultipleGoalTrackerMileStoneCreateModel instance,
) => <String, dynamic>{
  'newGoalTrackerMileStones': instance.newGoalTrackerMileStones
      ?.map((e) => e.toJson())
      .toList(),
};

MultipleMeditationFileCreateModel _$MultipleMeditationFileCreateModelFromJson(
  Map<String, dynamic> json,
) => MultipleMeditationFileCreateModel(
  newMeditationFiles:
      (json['newMeditationFiles'] as List<dynamic>?)
          ?.map(
            (e) =>
                MeditationFileCreateModel.fromJson(e as Map<String, dynamic>),
          )
          .toList() ??
      [],
);

Map<String, dynamic> _$MultipleMeditationFileCreateModelToJson(
  MultipleMeditationFileCreateModel instance,
) => <String, dynamic>{
  'newMeditationFiles': instance.newMeditationFiles
      ?.map((e) => e.toJson())
      .toList(),
};

MultipleMeditationSessionCreateModel
_$MultipleMeditationSessionCreateModelFromJson(Map<String, dynamic> json) =>
    MultipleMeditationSessionCreateModel(
      newMeditationSessions:
          (json['newMeditationSessions'] as List<dynamic>?)
              ?.map(
                (e) => MeditationSessionCreateModel.fromJson(
                  e as Map<String, dynamic>,
                ),
              )
              .toList() ??
          [],
    );

Map<String, dynamic> _$MultipleMeditationSessionCreateModelToJson(
  MultipleMeditationSessionCreateModel instance,
) => <String, dynamic>{
  'newMeditationSessions': instance.newMeditationSessions
      ?.map((e) => e.toJson())
      .toList(),
};

MultiplePushNotificationServerCreateModel
_$MultiplePushNotificationServerCreateModelFromJson(
  Map<String, dynamic> json,
) => MultiplePushNotificationServerCreateModel(
  newPushNotificationServers:
      (json['newPushNotificationServers'] as List<dynamic>?)
          ?.map(
            (e) => PushNotificationServerCreateModel.fromJson(
              e as Map<String, dynamic>,
            ),
          )
          .toList() ??
      [],
);

Map<String, dynamic> _$MultiplePushNotificationServerCreateModelToJson(
  MultiplePushNotificationServerCreateModel instance,
) => <String, dynamic>{
  'newPushNotificationServers': instance.newPushNotificationServers
      ?.map((e) => e.toJson())
      .toList(),
};

MultipleUserGroupCreateModel _$MultipleUserGroupCreateModelFromJson(
  Map<String, dynamic> json,
) => MultipleUserGroupCreateModel(
  newUserGroups:
      (json['newUserGroups'] as List<dynamic>?)
          ?.map((e) => UserGroupCreateModel.fromJson(e as Map<String, dynamic>))
          .toList() ??
      [],
);

Map<String, dynamic> _$MultipleUserGroupCreateModelToJson(
  MultipleUserGroupCreateModel instance,
) => <String, dynamic>{
  'newUserGroups': instance.newUserGroups?.map((e) => e.toJson()).toList(),
};

MultipleVisionBoardPictureCreateModel
_$MultipleVisionBoardPictureCreateModelFromJson(Map<String, dynamic> json) =>
    MultipleVisionBoardPictureCreateModel(
      newVisionBoardPictures:
          (json['newVisionBoardPictures'] as List<dynamic>?)
              ?.map(
                (e) => VisionBoardPictureCreateModel.fromJson(
                  e as Map<String, dynamic>,
                ),
              )
              .toList() ??
          [],
    );

Map<String, dynamic> _$MultipleVisionBoardPictureCreateModelToJson(
  MultipleVisionBoardPictureCreateModel instance,
) => <String, dynamic>{
  'newVisionBoardPictures': instance.newVisionBoardPictures
      ?.map((e) => e.toJson())
      .toList(),
};

MultipleVisionBoardSessionCreateModel
_$MultipleVisionBoardSessionCreateModelFromJson(Map<String, dynamic> json) =>
    MultipleVisionBoardSessionCreateModel(
      newVisionBoardSessions:
          (json['newVisionBoardSessions'] as List<dynamic>?)
              ?.map(
                (e) => VisionBoardSessionCreateModel.fromJson(
                  e as Map<String, dynamic>,
                ),
              )
              .toList() ??
          [],
    );

Map<String, dynamic> _$MultipleVisionBoardSessionCreateModelToJson(
  MultipleVisionBoardSessionCreateModel instance,
) => <String, dynamic>{
  'newVisionBoardSessions': instance.newVisionBoardSessions
      ?.map((e) => e.toJson())
      .toList(),
};

MultipleVisionBoardSessionPictureCreateModel
_$MultipleVisionBoardSessionPictureCreateModelFromJson(
  Map<String, dynamic> json,
) => MultipleVisionBoardSessionPictureCreateModel(
  newVisionBoardSessionPictures:
      (json['newVisionBoardSessionPictures'] as List<dynamic>?)
          ?.map(
            (e) => VisionBoardSessionPictureCreateModel.fromJson(
              e as Map<String, dynamic>,
            ),
          )
          .toList() ??
      [],
);

Map<String, dynamic> _$MultipleVisionBoardSessionPictureCreateModelToJson(
  MultipleVisionBoardSessionPictureCreateModel instance,
) => <String, dynamic>{
  'newVisionBoardSessionPictures': instance.newVisionBoardSessionPictures
      ?.map((e) => e.toJson())
      .toList(),
};

MultipleVoiceNotificationServerCreateModel
_$MultipleVoiceNotificationServerCreateModelFromJson(
  Map<String, dynamic> json,
) => MultipleVoiceNotificationServerCreateModel(
  newVoiceNotificationServers:
      (json['newVoiceNotificationServers'] as List<dynamic>?)
          ?.map(
            (e) => VoiceNotificationServerCreateModel.fromJson(
              e as Map<String, dynamic>,
            ),
          )
          .toList() ??
      [],
);

Map<String, dynamic> _$MultipleVoiceNotificationServerCreateModelToJson(
  MultipleVoiceNotificationServerCreateModel instance,
) => <String, dynamic>{
  'newVoiceNotificationServers': instance.newVoiceNotificationServers
      ?.map((e) => e.toJson())
      .toList(),
};

MultipleWhatsAppNotificationServerCreateModel
_$MultipleWhatsAppNotificationServerCreateModelFromJson(
  Map<String, dynamic> json,
) => MultipleWhatsAppNotificationServerCreateModel(
  newWhatsAppNotificationServers:
      (json['newWhatsAppNotificationServers'] as List<dynamic>?)
          ?.map(
            (e) => WhatsAppNotificationServerCreateModel.fromJson(
              e as Map<String, dynamic>,
            ),
          )
          .toList() ??
      [],
);

Map<String, dynamic> _$MultipleWhatsAppNotificationServerCreateModelToJson(
  MultipleWhatsAppNotificationServerCreateModel instance,
) => <String, dynamic>{
  'newWhatsAppNotificationServers': instance.newWhatsAppNotificationServers
      ?.map((e) => e.toJson())
      .toList(),
};

PerfLogModel _$PerfLogModelFromJson(Map<String, dynamic> json) => PerfLogModel(
  logDate: json['logDate'] == null
      ? null
      : DateTime.parse(json['logDate'] as String),
  activityId: json['activityId'] as String?,
  machineName: json['machineName'] as String?,
  userIdentity: json['userIdentity'] as String?,
  appName: json['appName'] as String?,
  appModule: json['appModule'] as String?,
  appFunction: json['appFunction'] as String?,
  httpVerb: json['httpVerb'] as String?,
  statusCode: json['statusCode'] as String?,
  startTime: json['startTime'] as String?,
  endTime: json['endTime'] as String?,
  elapsedTimeInMs: (json['elapsedTimeInMs'] as num?)?.toDouble(),
  ipAddress: json['ipAddress'] as String?,
  elapsedTimeInSeconds: (json['elapsedTimeInSeconds'] as num?)?.toDouble(),
);

Map<String, dynamic> _$PerfLogModelToJson(PerfLogModel instance) =>
    <String, dynamic>{
      'logDate': instance.logDate?.toIso8601String(),
      'activityId': instance.activityId,
      'machineName': instance.machineName,
      'userIdentity': instance.userIdentity,
      'appName': instance.appName,
      'appModule': instance.appModule,
      'appFunction': instance.appFunction,
      'httpVerb': instance.httpVerb,
      'statusCode': instance.statusCode,
      'startTime': instance.startTime,
      'endTime': instance.endTime,
      'elapsedTimeInMs': instance.elapsedTimeInMs,
      'ipAddress': instance.ipAddress,
      'elapsedTimeInSeconds': instance.elapsedTimeInSeconds,
    };

PerfLogModelReturnListModel _$PerfLogModelReturnListModelFromJson(
  Map<String, dynamic> json,
) => PerfLogModelReturnListModel(
  model:
      (json['model'] as List<dynamic>?)
          ?.map((e) => PerfLogModel.fromJson(e as Map<String, dynamic>))
          .toList() ??
      [],
  search: json['search'] == null
      ? null
      : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
  totalRecords: (json['totalRecords'] as num?)?.toInt(),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$PerfLogModelReturnListModelToJson(
  PerfLogModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

PushNotificationServerCreateModel _$PushNotificationServerCreateModelFromJson(
  Map<String, dynamic> json,
) => PushNotificationServerCreateModel(
  pushNotificationServerCode: json['pushNotificationServerCode'] as String?,
  pushNotificationServerName: json['pushNotificationServerName'] as String?,
  predefinedPushNotificationServerTypeId:
      (json['predefinedPushNotificationServerTypeId'] as num?)?.toInt(),
  projectId: json['projectId'] as String?,
  apiKey: json['apiKey'] as String?,
  apiSecret: json['apiSecret'] as String?,
  appleTeamId: json['appleTeamId'] as String?,
  appleKeyId: json['appleKeyId'] as String?,
  appleAuthKey: json['appleAuthKey'] as String?,
  isEnabled: json['isEnabled'] as bool?,
);

Map<String, dynamic> _$PushNotificationServerCreateModelToJson(
  PushNotificationServerCreateModel instance,
) => <String, dynamic>{
  'pushNotificationServerCode': instance.pushNotificationServerCode,
  'pushNotificationServerName': instance.pushNotificationServerName,
  'predefinedPushNotificationServerTypeId':
      instance.predefinedPushNotificationServerTypeId,
  'projectId': instance.projectId,
  'apiKey': instance.apiKey,
  'apiSecret': instance.apiSecret,
  'appleTeamId': instance.appleTeamId,
  'appleKeyId': instance.appleKeyId,
  'appleAuthKey': instance.appleAuthKey,
  'isEnabled': instance.isEnabled,
};

PushNotificationServerModel _$PushNotificationServerModelFromJson(
  Map<String, dynamic> json,
) => PushNotificationServerModel(
  pushNotificationServerId: (json['pushNotificationServerId'] as num?)?.toInt(),
  pushNotificationServerCode: json['pushNotificationServerCode'] as String?,
  pushNotificationServerName: json['pushNotificationServerName'] as String?,
  predefinedPushNotificationServerTypeId:
      (json['predefinedPushNotificationServerTypeId'] as num?)?.toInt(),
  projectId: json['projectId'] as String?,
  apiKey: json['apiKey'] as String?,
  apiSecret: json['apiSecret'] as String?,
  appleTeamId: json['appleTeamId'] as String?,
  appleKeyId: json['appleKeyId'] as String?,
  appleAuthKey: json['appleAuthKey'] as String?,
  isEnabled: json['isEnabled'] as bool?,
  isDeleted: json['isDeleted'] as bool?,
  createdDate: json['createdDate'] == null
      ? null
      : DateTime.parse(json['createdDate'] as String),
  createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
  modifiedDate: json['modifiedDate'] == null
      ? null
      : DateTime.parse(json['modifiedDate'] as String),
  modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
  createdByUserFirstName: json['createdByUserFirstName'] as String?,
  createdByUserLastName: json['createdByUserLastName'] as String?,
  createdByUserEmail: json['createdByUserEmail'] as String?,
  modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
  modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
  modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
  createdBy: json['createdBy'] as String?,
  modifiedBy: json['modifiedBy'] as String?,
);

Map<String, dynamic> _$PushNotificationServerModelToJson(
  PushNotificationServerModel instance,
) => <String, dynamic>{
  'pushNotificationServerId': instance.pushNotificationServerId,
  'pushNotificationServerCode': instance.pushNotificationServerCode,
  'pushNotificationServerName': instance.pushNotificationServerName,
  'predefinedPushNotificationServerTypeId':
      instance.predefinedPushNotificationServerTypeId,
  'projectId': instance.projectId,
  'apiKey': instance.apiKey,
  'apiSecret': instance.apiSecret,
  'appleTeamId': instance.appleTeamId,
  'appleKeyId': instance.appleKeyId,
  'appleAuthKey': instance.appleAuthKey,
  'isEnabled': instance.isEnabled,
  'isDeleted': instance.isDeleted,
  'createdDate': instance.createdDate?.toIso8601String(),
  'createdByUserId': instance.createdByUserId,
  'modifiedDate': instance.modifiedDate?.toIso8601String(),
  'modifiedByUserId': instance.modifiedByUserId,
  'createdByUserFirstName': instance.createdByUserFirstName,
  'createdByUserLastName': instance.createdByUserLastName,
  'createdByUserEmail': instance.createdByUserEmail,
  'modifiedByUserFirstName': instance.modifiedByUserFirstName,
  'modifiedByUserLastName': instance.modifiedByUserLastName,
  'modifiedByUserEmail': instance.modifiedByUserEmail,
  'createdBy': instance.createdBy,
  'modifiedBy': instance.modifiedBy,
};

PushNotificationServerModelReturnListModel
_$PushNotificationServerModelReturnListModelFromJson(
  Map<String, dynamic> json,
) => PushNotificationServerModelReturnListModel(
  model:
      (json['model'] as List<dynamic>?)
          ?.map(
            (e) =>
                PushNotificationServerModel.fromJson(e as Map<String, dynamic>),
          )
          .toList() ??
      [],
  search: json['search'] == null
      ? null
      : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
  totalRecords: (json['totalRecords'] as num?)?.toInt(),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$PushNotificationServerModelReturnListModelToJson(
  PushNotificationServerModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

PushNotificationServerModelReturnModel
_$PushNotificationServerModelReturnModelFromJson(Map<String, dynamic> json) =>
    PushNotificationServerModelReturnModel(
      model: json['model'] == null
          ? null
          : PushNotificationServerModel.fromJson(
              json['model'] as Map<String, dynamic>,
            ),
      isSuccess: json['isSuccess'] as bool?,
      successMessage: json['successMessage'] as String?,
      errorHolder: json['errorHolder'] == null
          ? null
          : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
      activityId: json['activityId'] as String?,
      activityDateTime: json['activityDateTime'] == null
          ? null
          : DateTime.parse(json['activityDateTime'] as String),
      server: json['server'] as String?,
      processId: (json['processId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$PushNotificationServerModelReturnModelToJson(
  PushNotificationServerModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

PushNotificationServerUpdateModel _$PushNotificationServerUpdateModelFromJson(
  Map<String, dynamic> json,
) => PushNotificationServerUpdateModel(
  pushNotificationServerId: (json['pushNotificationServerId'] as num?)?.toInt(),
  pushNotificationServerCode: json['pushNotificationServerCode'] as String?,
  pushNotificationServerName: json['pushNotificationServerName'] as String?,
  predefinedPushNotificationServerTypeId:
      (json['predefinedPushNotificationServerTypeId'] as num?)?.toInt(),
  projectId: json['projectId'] as String?,
  apiKey: json['apiKey'] as String?,
  apiSecret: json['apiSecret'] as String?,
  appleTeamId: json['appleTeamId'] as String?,
  appleKeyId: json['appleKeyId'] as String?,
  appleAuthKey: json['appleAuthKey'] as String?,
  isEnabled: json['isEnabled'] as bool?,
);

Map<String, dynamic> _$PushNotificationServerUpdateModelToJson(
  PushNotificationServerUpdateModel instance,
) => <String, dynamic>{
  'pushNotificationServerId': instance.pushNotificationServerId,
  'pushNotificationServerCode': instance.pushNotificationServerCode,
  'pushNotificationServerName': instance.pushNotificationServerName,
  'predefinedPushNotificationServerTypeId':
      instance.predefinedPushNotificationServerTypeId,
  'projectId': instance.projectId,
  'apiKey': instance.apiKey,
  'apiSecret': instance.apiSecret,
  'appleTeamId': instance.appleTeamId,
  'appleKeyId': instance.appleKeyId,
  'appleAuthKey': instance.appleAuthKey,
  'isEnabled': instance.isEnabled,
};

RealtimeMessageCreateModel _$RealtimeMessageCreateModelFromJson(
  Map<String, dynamic> json,
) => RealtimeMessageCreateModel(
  predefinedMessagePriorityTypeId:
      (json['predefinedMessagePriorityTypeId'] as num?)?.toInt(),
  messageDateTime: json['messageDateTime'] == null
      ? null
      : DateTime.parse(json['messageDateTime'] as String),
  messageSubject: json['messageSubject'] as String?,
  messageContent: json['messageContent'] as String?,
  messageSentDateTime: json['messageSentDateTime'] == null
      ? null
      : DateTime.parse(json['messageSentDateTime'] as String),
  messageViewedDateTime: json['messageViewedDateTime'] == null
      ? null
      : DateTime.parse(json['messageViewedDateTime'] as String),
  predefinedRealtimeMessageStatusTypeId:
      (json['predefinedRealtimeMessageStatusTypeId'] as num?)?.toInt(),
  predefinedNotificationMessageTypeId:
      (json['predefinedNotificationMessageTypeId'] as num?)?.toInt(),
  resourceName: json['resourceName'] as String?,
  predefinedResourceTypeId: (json['predefinedResourceTypeId'] as num?)?.toInt(),
  resourceId: (json['resourceId'] as num?)?.toInt(),
  errorMessage: json['errorMessage'] as String?,
  errorOccurredOn: json['errorOccurredOn'] == null
      ? null
      : DateTime.parse(json['errorOccurredOn'] as String),
  userId: (json['userId'] as num?)?.toInt(),
);

Map<String, dynamic> _$RealtimeMessageCreateModelToJson(
  RealtimeMessageCreateModel instance,
) => <String, dynamic>{
  'predefinedMessagePriorityTypeId': instance.predefinedMessagePriorityTypeId,
  'messageDateTime': instance.messageDateTime?.toIso8601String(),
  'messageSubject': instance.messageSubject,
  'messageContent': instance.messageContent,
  'messageSentDateTime': instance.messageSentDateTime?.toIso8601String(),
  'messageViewedDateTime': instance.messageViewedDateTime?.toIso8601String(),
  'predefinedRealtimeMessageStatusTypeId':
      instance.predefinedRealtimeMessageStatusTypeId,
  'predefinedNotificationMessageTypeId':
      instance.predefinedNotificationMessageTypeId,
  'resourceName': instance.resourceName,
  'predefinedResourceTypeId': instance.predefinedResourceTypeId,
  'resourceId': instance.resourceId,
  'errorMessage': instance.errorMessage,
  'errorOccurredOn': instance.errorOccurredOn?.toIso8601String(),
  'userId': instance.userId,
};

RealtimeMessageModel _$RealtimeMessageModelFromJson(
  Map<String, dynamic> json,
) => RealtimeMessageModel(
  realtimeMessageId: (json['realtimeMessageId'] as num?)?.toInt(),
  predefinedMessagePriorityTypeId:
      (json['predefinedMessagePriorityTypeId'] as num?)?.toInt(),
  messageDateTime: json['messageDateTime'] == null
      ? null
      : DateTime.parse(json['messageDateTime'] as String),
  messageSubject: json['messageSubject'] as String?,
  messageContent: json['messageContent'] as String?,
  messageSentDateTime: json['messageSentDateTime'] == null
      ? null
      : DateTime.parse(json['messageSentDateTime'] as String),
  messageViewedDateTime: json['messageViewedDateTime'] == null
      ? null
      : DateTime.parse(json['messageViewedDateTime'] as String),
  predefinedRealtimeMessageStatusTypeId:
      (json['predefinedRealtimeMessageStatusTypeId'] as num?)?.toInt(),
  predefinedNotificationMessageTypeId:
      (json['predefinedNotificationMessageTypeId'] as num?)?.toInt(),
  resourceName: json['resourceName'] as String?,
  predefinedResourceTypeId: (json['predefinedResourceTypeId'] as num?)?.toInt(),
  resourceId: (json['resourceId'] as num?)?.toInt(),
  errorMessage: json['errorMessage'] as String?,
  errorOccurredOn: json['errorOccurredOn'] == null
      ? null
      : DateTime.parse(json['errorOccurredOn'] as String),
  userId: (json['userId'] as num?)?.toInt(),
  isDeleted: json['isDeleted'] as bool?,
  createdDate: json['createdDate'] == null
      ? null
      : DateTime.parse(json['createdDate'] as String),
  createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
  modifiedDate: json['modifiedDate'] == null
      ? null
      : DateTime.parse(json['modifiedDate'] as String),
  modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
  predefinedMessagePriorityTypeName:
      json['predefinedMessagePriorityTypeName'] as String?,
  predefinedRealtimeMessageStatusTypeName:
      json['predefinedRealtimeMessageStatusTypeName'] as String?,
  predefinedNotificationMessageTypeName:
      json['predefinedNotificationMessageTypeName'] as String?,
  predefinedResourceTypeName: json['predefinedResourceTypeName'] as String?,
  userName: json['userName'] as String?,
  createdByUserFirstName: json['createdByUserFirstName'] as String?,
  createdByUserLastName: json['createdByUserLastName'] as String?,
  createdByUserEmail: json['createdByUserEmail'] as String?,
  modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
  modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
  modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
  createdBy: json['createdBy'] as String?,
  modifiedBy: json['modifiedBy'] as String?,
);

Map<String, dynamic> _$RealtimeMessageModelToJson(
  RealtimeMessageModel instance,
) => <String, dynamic>{
  'realtimeMessageId': instance.realtimeMessageId,
  'predefinedMessagePriorityTypeId': instance.predefinedMessagePriorityTypeId,
  'messageDateTime': instance.messageDateTime?.toIso8601String(),
  'messageSubject': instance.messageSubject,
  'messageContent': instance.messageContent,
  'messageSentDateTime': instance.messageSentDateTime?.toIso8601String(),
  'messageViewedDateTime': instance.messageViewedDateTime?.toIso8601String(),
  'predefinedRealtimeMessageStatusTypeId':
      instance.predefinedRealtimeMessageStatusTypeId,
  'predefinedNotificationMessageTypeId':
      instance.predefinedNotificationMessageTypeId,
  'resourceName': instance.resourceName,
  'predefinedResourceTypeId': instance.predefinedResourceTypeId,
  'resourceId': instance.resourceId,
  'errorMessage': instance.errorMessage,
  'errorOccurredOn': instance.errorOccurredOn?.toIso8601String(),
  'userId': instance.userId,
  'isDeleted': instance.isDeleted,
  'createdDate': instance.createdDate?.toIso8601String(),
  'createdByUserId': instance.createdByUserId,
  'modifiedDate': instance.modifiedDate?.toIso8601String(),
  'modifiedByUserId': instance.modifiedByUserId,
  'predefinedMessagePriorityTypeName':
      instance.predefinedMessagePriorityTypeName,
  'predefinedRealtimeMessageStatusTypeName':
      instance.predefinedRealtimeMessageStatusTypeName,
  'predefinedNotificationMessageTypeName':
      instance.predefinedNotificationMessageTypeName,
  'predefinedResourceTypeName': instance.predefinedResourceTypeName,
  'userName': instance.userName,
  'createdByUserFirstName': instance.createdByUserFirstName,
  'createdByUserLastName': instance.createdByUserLastName,
  'createdByUserEmail': instance.createdByUserEmail,
  'modifiedByUserFirstName': instance.modifiedByUserFirstName,
  'modifiedByUserLastName': instance.modifiedByUserLastName,
  'modifiedByUserEmail': instance.modifiedByUserEmail,
  'createdBy': instance.createdBy,
  'modifiedBy': instance.modifiedBy,
};

RealtimeMessageModelReturnListModel
_$RealtimeMessageModelReturnListModelFromJson(Map<String, dynamic> json) =>
    RealtimeMessageModelReturnListModel(
      model:
          (json['model'] as List<dynamic>?)
              ?.map(
                (e) => RealtimeMessageModel.fromJson(e as Map<String, dynamic>),
              )
              .toList() ??
          [],
      search: json['search'] == null
          ? null
          : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
      totalRecords: (json['totalRecords'] as num?)?.toInt(),
      isSuccess: json['isSuccess'] as bool?,
      successMessage: json['successMessage'] as String?,
      errorHolder: json['errorHolder'] == null
          ? null
          : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
      activityId: json['activityId'] as String?,
      activityDateTime: json['activityDateTime'] == null
          ? null
          : DateTime.parse(json['activityDateTime'] as String),
      server: json['server'] as String?,
      processId: (json['processId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$RealtimeMessageModelReturnListModelToJson(
  RealtimeMessageModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

RealtimeMessageModelReturnModel _$RealtimeMessageModelReturnModelFromJson(
  Map<String, dynamic> json,
) => RealtimeMessageModelReturnModel(
  model: json['model'] == null
      ? null
      : RealtimeMessageModel.fromJson(json['model'] as Map<String, dynamic>),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$RealtimeMessageModelReturnModelToJson(
  RealtimeMessageModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

RealtimeMessageUpdateModel _$RealtimeMessageUpdateModelFromJson(
  Map<String, dynamic> json,
) => RealtimeMessageUpdateModel(
  realtimeMessageId: (json['realtimeMessageId'] as num?)?.toInt(),
  predefinedMessagePriorityTypeId:
      (json['predefinedMessagePriorityTypeId'] as num?)?.toInt(),
  messageDateTime: json['messageDateTime'] == null
      ? null
      : DateTime.parse(json['messageDateTime'] as String),
  messageSubject: json['messageSubject'] as String?,
  messageContent: json['messageContent'] as String?,
  messageSentDateTime: json['messageSentDateTime'] == null
      ? null
      : DateTime.parse(json['messageSentDateTime'] as String),
  messageViewedDateTime: json['messageViewedDateTime'] == null
      ? null
      : DateTime.parse(json['messageViewedDateTime'] as String),
  predefinedRealtimeMessageStatusTypeId:
      (json['predefinedRealtimeMessageStatusTypeId'] as num?)?.toInt(),
  predefinedNotificationMessageTypeId:
      (json['predefinedNotificationMessageTypeId'] as num?)?.toInt(),
  resourceName: json['resourceName'] as String?,
  predefinedResourceTypeId: (json['predefinedResourceTypeId'] as num?)?.toInt(),
  resourceId: (json['resourceId'] as num?)?.toInt(),
  errorMessage: json['errorMessage'] as String?,
  errorOccurredOn: json['errorOccurredOn'] == null
      ? null
      : DateTime.parse(json['errorOccurredOn'] as String),
  userId: (json['userId'] as num?)?.toInt(),
);

Map<String, dynamic> _$RealtimeMessageUpdateModelToJson(
  RealtimeMessageUpdateModel instance,
) => <String, dynamic>{
  'realtimeMessageId': instance.realtimeMessageId,
  'predefinedMessagePriorityTypeId': instance.predefinedMessagePriorityTypeId,
  'messageDateTime': instance.messageDateTime?.toIso8601String(),
  'messageSubject': instance.messageSubject,
  'messageContent': instance.messageContent,
  'messageSentDateTime': instance.messageSentDateTime?.toIso8601String(),
  'messageViewedDateTime': instance.messageViewedDateTime?.toIso8601String(),
  'predefinedRealtimeMessageStatusTypeId':
      instance.predefinedRealtimeMessageStatusTypeId,
  'predefinedNotificationMessageTypeId':
      instance.predefinedNotificationMessageTypeId,
  'resourceName': instance.resourceName,
  'predefinedResourceTypeId': instance.predefinedResourceTypeId,
  'resourceId': instance.resourceId,
  'errorMessage': instance.errorMessage,
  'errorOccurredOn': instance.errorOccurredOn?.toIso8601String(),
  'userId': instance.userId,
};

RecoverPasswordRequestModel _$RecoverPasswordRequestModelFromJson(
  Map<String, dynamic> json,
) => RecoverPasswordRequestModel(
  userName: json['userName'] as String?,
  clientRedirectURL: json['clientRedirectURL'] as String?,
);

Map<String, dynamic> _$RecoverPasswordRequestModelToJson(
  RecoverPasswordRequestModel instance,
) => <String, dynamic>{
  'userName': instance.userName,
  'clientRedirectURL': instance.clientRedirectURL,
};

ReportServerConfigurationModel _$ReportServerConfigurationModelFromJson(
  Map<String, dynamic> json,
) => ReportServerConfigurationModel(
  reportServerFileStorageConnectionString:
      json['reportServerFileStorageConnectionString'] as String?,
  reportServerQueueConnectionString:
      json['reportServerQueueConnectionString'] as String?,
  reportServerProcessThreadCount:
      (json['reportServerProcessThreadCount'] as num?)?.toInt(),
  enableReportServerAutoCleanup: json['enableReportServerAutoCleanup'] as bool?,
  reportServerAutoCleanUpAfterDays:
      (json['reportServerAutoCleanUpAfterDays'] as num?)?.toInt(),
  reportServerViewerWebPath: json['reportServerViewerWebPath'] as String?,
  reportStoragePrefix: json['reportStoragePrefix'] as String?,
  reportIntermediateDataFilePath:
      json['reportIntermediateDataFilePath'] as String?,
);

Map<String, dynamic> _$ReportServerConfigurationModelToJson(
  ReportServerConfigurationModel instance,
) => <String, dynamic>{
  'reportServerFileStorageConnectionString':
      instance.reportServerFileStorageConnectionString,
  'reportServerQueueConnectionString':
      instance.reportServerQueueConnectionString,
  'reportServerProcessThreadCount': instance.reportServerProcessThreadCount,
  'enableReportServerAutoCleanup': instance.enableReportServerAutoCleanup,
  'reportServerAutoCleanUpAfterDays': instance.reportServerAutoCleanUpAfterDays,
  'reportServerViewerWebPath': instance.reportServerViewerWebPath,
  'reportStoragePrefix': instance.reportStoragePrefix,
  'reportIntermediateDataFilePath': instance.reportIntermediateDataFilePath,
};

ReportServerConfigurationModelReturnModel
_$ReportServerConfigurationModelReturnModelFromJson(
  Map<String, dynamic> json,
) => ReportServerConfigurationModelReturnModel(
  model: json['model'] == null
      ? null
      : ReportServerConfigurationModel.fromJson(
          json['model'] as Map<String, dynamic>,
        ),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$ReportServerConfigurationModelReturnModelToJson(
  ReportServerConfigurationModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

ResetPasswordRequestModel _$ResetPasswordRequestModelFromJson(
  Map<String, dynamic> json,
) => ResetPasswordRequestModel(
  loginRedirecturl: json['loginRedirecturl'] as String?,
  token: json['token'] as String?,
  newPassword: json['newPassword'] as String?,
  confirmPassword: json['confirmPassword'] as String?,
);

Map<String, dynamic> _$ResetPasswordRequestModelToJson(
  ResetPasswordRequestModel instance,
) => <String, dynamic>{
  'loginRedirecturl': instance.loginRedirecturl,
  'token': instance.token,
  'newPassword': instance.newPassword,
  'confirmPassword': instance.confirmPassword,
};

ReturnError _$ReturnErrorFromJson(Map<String, dynamic> json) => ReturnError(
  friendlyMessage: json['friendlyMessage'] as String?,
  internalErrorMessage: json['internalErrorMessage'] as String?,
  errorItemList:
      (json['errorItemList'] as List<dynamic>?)
          ?.map((e) => ReturnErrorItem.fromJson(e as Map<String, dynamic>))
          .toList() ??
      [],
  exception: json['exception'] == null
      ? null
      : ReturnException.fromJson(json['exception'] as Map<String, dynamic>),
);

Map<String, dynamic> _$ReturnErrorToJson(ReturnError instance) =>
    <String, dynamic>{
      'friendlyMessage': instance.friendlyMessage,
      'internalErrorMessage': instance.internalErrorMessage,
      'errorItemList': instance.errorItemList?.map((e) => e.toJson()).toList(),
      'exception': instance.exception?.toJson(),
    };

ReturnErrorItem _$ReturnErrorItemFromJson(Map<String, dynamic> json) =>
    ReturnErrorItem(
      key: json['key'] as String?,
      $value: json['value'] as String?,
      tags:
          (json['tags'] as List<dynamic>?)?.map((e) => e as String).toList() ??
          [],
    );

Map<String, dynamic> _$ReturnErrorItemToJson(ReturnErrorItem instance) =>
    <String, dynamic>{
      'key': instance.key,
      'value': instance.$value,
      'tags': instance.tags,
    };

ReturnException _$ReturnExceptionFromJson(Map<String, dynamic> json) =>
    ReturnException(
      stackTrace: json['stackTrace'] as String?,
      source: json['source'] as String?,
      message: json['message'] as String?,
      innerException: json['innerException'] == null
          ? null
          : ReturnException.fromJson(
              json['innerException'] as Map<String, dynamic>,
            ),
    );

Map<String, dynamic> _$ReturnExceptionToJson(ReturnException instance) =>
    <String, dynamic>{
      'stackTrace': instance.stackTrace,
      'source': instance.source,
      'message': instance.message,
      'innerException': instance.innerException?.toJson(),
    };

SavedJobCreateModel _$SavedJobCreateModelFromJson(Map<String, dynamic> json) =>
    SavedJobCreateModel(
      predefinedJobTypeId: (json['predefinedJobTypeId'] as num?)?.toInt(),
      predefinedJobOrReportTypeId: (json['predefinedJobOrReportTypeId'] as num?)
          ?.toInt(),
      jobTemplateId: (json['jobTemplateId'] as num?)?.toInt(),
      completedDate: json['completedDate'] == null
          ? null
          : DateTime.parse(json['completedDate'] as String),
      submittedDate: json['submittedDate'] == null
          ? null
          : DateTime.parse(json['submittedDate'] as String),
      submittedByUserId: (json['submittedByUserId'] as num?)?.toInt(),
      executableFolder: json['executableFolder'] as String?,
      executableName: json['executableName'] as String?,
      executableParameter: json['executableParameter'] as String?,
      serializedJobParameter: json['serializedJobParameter'] as String?,
      reportTemplateFile: json['reportTemplateFile'] as String?,
      exportDoc: json['exportDoc'] as bool?,
      exportXls: json['exportXls'] as bool?,
      exportPpt: json['exportPpt'] as bool?,
      exportCsv: json['exportCsv'] as bool?,
      exportXml: json['exportXml'] as bool?,
      exportPdf: json['exportPdf'] as bool?,
      exportCustom: json['exportCustom'] as bool?,
      customExportName: json['customExportName'] as String?,
      pickupDate: json['pickupDate'] == null
          ? null
          : DateTime.parse(json['pickupDate'] as String),
      jobName: json['jobName'] as String?,
      userId: (json['userId'] as num?)?.toInt(),
      includeDataSourceFile: json['includeDataSourceFile'] as bool?,
    );

Map<String, dynamic> _$SavedJobCreateModelToJson(
  SavedJobCreateModel instance,
) => <String, dynamic>{
  'predefinedJobTypeId': instance.predefinedJobTypeId,
  'predefinedJobOrReportTypeId': instance.predefinedJobOrReportTypeId,
  'jobTemplateId': instance.jobTemplateId,
  'completedDate': instance.completedDate?.toIso8601String(),
  'submittedDate': instance.submittedDate?.toIso8601String(),
  'submittedByUserId': instance.submittedByUserId,
  'executableFolder': instance.executableFolder,
  'executableName': instance.executableName,
  'executableParameter': instance.executableParameter,
  'serializedJobParameter': instance.serializedJobParameter,
  'reportTemplateFile': instance.reportTemplateFile,
  'exportDoc': instance.exportDoc,
  'exportXls': instance.exportXls,
  'exportPpt': instance.exportPpt,
  'exportCsv': instance.exportCsv,
  'exportXml': instance.exportXml,
  'exportPdf': instance.exportPdf,
  'exportCustom': instance.exportCustom,
  'customExportName': instance.customExportName,
  'pickupDate': instance.pickupDate?.toIso8601String(),
  'jobName': instance.jobName,
  'userId': instance.userId,
  'includeDataSourceFile': instance.includeDataSourceFile,
};

SavedJobModel _$SavedJobModelFromJson(Map<String, dynamic> json) =>
    SavedJobModel(
      savedJobId: (json['savedJobId'] as num?)?.toInt(),
      predefinedJobTypeId: (json['predefinedJobTypeId'] as num?)?.toInt(),
      predefinedJobOrReportTypeId: (json['predefinedJobOrReportTypeId'] as num?)
          ?.toInt(),
      jobTemplateId: (json['jobTemplateId'] as num?)?.toInt(),
      completedDate: json['completedDate'] == null
          ? null
          : DateTime.parse(json['completedDate'] as String),
      submittedDate: json['submittedDate'] == null
          ? null
          : DateTime.parse(json['submittedDate'] as String),
      submittedByUserId: (json['submittedByUserId'] as num?)?.toInt(),
      executableFolder: json['executableFolder'] as String?,
      executableName: json['executableName'] as String?,
      executableParameter: json['executableParameter'] as String?,
      serializedJobParameter: json['serializedJobParameter'] as String?,
      reportTemplateFile: json['reportTemplateFile'] as String?,
      exportDoc: json['exportDoc'] as bool?,
      exportXls: json['exportXls'] as bool?,
      exportPpt: json['exportPpt'] as bool?,
      exportCsv: json['exportCsv'] as bool?,
      exportXml: json['exportXml'] as bool?,
      exportPdf: json['exportPdf'] as bool?,
      exportCustom: json['exportCustom'] as bool?,
      customExportName: json['customExportName'] as String?,
      pickupDate: json['pickupDate'] == null
          ? null
          : DateTime.parse(json['pickupDate'] as String),
      jobName: json['jobName'] as String?,
      userId: (json['userId'] as num?)?.toInt(),
      includeDataSourceFile: json['includeDataSourceFile'] as bool?,
      isDeleted: json['isDeleted'] as bool?,
      createdDate: json['createdDate'] == null
          ? null
          : DateTime.parse(json['createdDate'] as String),
      createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
      modifiedDate: json['modifiedDate'] == null
          ? null
          : DateTime.parse(json['modifiedDate'] as String),
      modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
      predefinedJobTypeName: json['predefinedJobTypeName'] as String?,
      predefinedJobOrReportTypeName:
          json['predefinedJobOrReportTypeName'] as String?,
      submittedByUserName: json['submittedByUserName'] as String?,
      userName: json['userName'] as String?,
      createdByUserFirstName: json['createdByUserFirstName'] as String?,
      createdByUserLastName: json['createdByUserLastName'] as String?,
      createdByUserEmail: json['createdByUserEmail'] as String?,
      modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
      modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
      modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
      createdBy: json['createdBy'] as String?,
      modifiedBy: json['modifiedBy'] as String?,
      savedNotificationEmails:
          (json['savedNotificationEmails'] as List<dynamic>?)
              ?.map(
                (e) => SavedJobNotificationEmailModel.fromJson(
                  e as Map<String, dynamic>,
                ),
              )
              .toList() ??
          [],
    );

Map<String, dynamic> _$SavedJobModelToJson(SavedJobModel instance) =>
    <String, dynamic>{
      'savedJobId': instance.savedJobId,
      'predefinedJobTypeId': instance.predefinedJobTypeId,
      'predefinedJobOrReportTypeId': instance.predefinedJobOrReportTypeId,
      'jobTemplateId': instance.jobTemplateId,
      'completedDate': instance.completedDate?.toIso8601String(),
      'submittedDate': instance.submittedDate?.toIso8601String(),
      'submittedByUserId': instance.submittedByUserId,
      'executableFolder': instance.executableFolder,
      'executableName': instance.executableName,
      'executableParameter': instance.executableParameter,
      'serializedJobParameter': instance.serializedJobParameter,
      'reportTemplateFile': instance.reportTemplateFile,
      'exportDoc': instance.exportDoc,
      'exportXls': instance.exportXls,
      'exportPpt': instance.exportPpt,
      'exportCsv': instance.exportCsv,
      'exportXml': instance.exportXml,
      'exportPdf': instance.exportPdf,
      'exportCustom': instance.exportCustom,
      'customExportName': instance.customExportName,
      'pickupDate': instance.pickupDate?.toIso8601String(),
      'jobName': instance.jobName,
      'userId': instance.userId,
      'includeDataSourceFile': instance.includeDataSourceFile,
      'isDeleted': instance.isDeleted,
      'createdDate': instance.createdDate?.toIso8601String(),
      'createdByUserId': instance.createdByUserId,
      'modifiedDate': instance.modifiedDate?.toIso8601String(),
      'modifiedByUserId': instance.modifiedByUserId,
      'predefinedJobTypeName': instance.predefinedJobTypeName,
      'predefinedJobOrReportTypeName': instance.predefinedJobOrReportTypeName,
      'submittedByUserName': instance.submittedByUserName,
      'userName': instance.userName,
      'createdByUserFirstName': instance.createdByUserFirstName,
      'createdByUserLastName': instance.createdByUserLastName,
      'createdByUserEmail': instance.createdByUserEmail,
      'modifiedByUserFirstName': instance.modifiedByUserFirstName,
      'modifiedByUserLastName': instance.modifiedByUserLastName,
      'modifiedByUserEmail': instance.modifiedByUserEmail,
      'createdBy': instance.createdBy,
      'modifiedBy': instance.modifiedBy,
      'savedNotificationEmails': instance.savedNotificationEmails
          ?.map((e) => e.toJson())
          .toList(),
    };

SavedJobModelReturnListModel _$SavedJobModelReturnListModelFromJson(
  Map<String, dynamic> json,
) => SavedJobModelReturnListModel(
  model:
      (json['model'] as List<dynamic>?)
          ?.map((e) => SavedJobModel.fromJson(e as Map<String, dynamic>))
          .toList() ??
      [],
  search: json['search'] == null
      ? null
      : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
  totalRecords: (json['totalRecords'] as num?)?.toInt(),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$SavedJobModelReturnListModelToJson(
  SavedJobModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

SavedJobModelReturnModel _$SavedJobModelReturnModelFromJson(
  Map<String, dynamic> json,
) => SavedJobModelReturnModel(
  model: json['model'] == null
      ? null
      : SavedJobModel.fromJson(json['model'] as Map<String, dynamic>),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$SavedJobModelReturnModelToJson(
  SavedJobModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

SavedJobNotificationEmailCreateModel
_$SavedJobNotificationEmailCreateModelFromJson(Map<String, dynamic> json) =>
    SavedJobNotificationEmailCreateModel(
      savedJobId: (json['savedJobId'] as num?)?.toInt(),
      email: json['email'] as String?,
      sendStatus: (json['sendStatus'] as num?)?.toInt(),
      message: json['message'] as String?,
    );

Map<String, dynamic> _$SavedJobNotificationEmailCreateModelToJson(
  SavedJobNotificationEmailCreateModel instance,
) => <String, dynamic>{
  'savedJobId': instance.savedJobId,
  'email': instance.email,
  'sendStatus': instance.sendStatus,
  'message': instance.message,
};

SavedJobNotificationEmailModel _$SavedJobNotificationEmailModelFromJson(
  Map<String, dynamic> json,
) => SavedJobNotificationEmailModel(
  savedJobNotificationEmailId: (json['savedJobNotificationEmailId'] as num?)
      ?.toInt(),
  savedJobId: (json['savedJobId'] as num?)?.toInt(),
  email: json['email'] as String?,
  sendStatus: (json['sendStatus'] as num?)?.toInt(),
  message: json['message'] as String?,
  isDeleted: json['isDeleted'] as bool?,
  createdDate: json['createdDate'] == null
      ? null
      : DateTime.parse(json['createdDate'] as String),
  createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
  modifiedDate: json['modifiedDate'] == null
      ? null
      : DateTime.parse(json['modifiedDate'] as String),
  modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
  companyId: (json['companyId'] as num?)?.toInt(),
  createdByUserFirstName: json['createdByUserFirstName'] as String?,
  createdByUserLastName: json['createdByUserLastName'] as String?,
  createdByUserEmail: json['createdByUserEmail'] as String?,
  modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
  modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
  modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
  createdBy: json['createdBy'] as String?,
  modifiedBy: json['modifiedBy'] as String?,
);

Map<String, dynamic> _$SavedJobNotificationEmailModelToJson(
  SavedJobNotificationEmailModel instance,
) => <String, dynamic>{
  'savedJobNotificationEmailId': instance.savedJobNotificationEmailId,
  'savedJobId': instance.savedJobId,
  'email': instance.email,
  'sendStatus': instance.sendStatus,
  'message': instance.message,
  'isDeleted': instance.isDeleted,
  'createdDate': instance.createdDate?.toIso8601String(),
  'createdByUserId': instance.createdByUserId,
  'modifiedDate': instance.modifiedDate?.toIso8601String(),
  'modifiedByUserId': instance.modifiedByUserId,
  'companyId': instance.companyId,
  'createdByUserFirstName': instance.createdByUserFirstName,
  'createdByUserLastName': instance.createdByUserLastName,
  'createdByUserEmail': instance.createdByUserEmail,
  'modifiedByUserFirstName': instance.modifiedByUserFirstName,
  'modifiedByUserLastName': instance.modifiedByUserLastName,
  'modifiedByUserEmail': instance.modifiedByUserEmail,
  'createdBy': instance.createdBy,
  'modifiedBy': instance.modifiedBy,
};

SavedJobNotificationEmailModelReturnListModel
_$SavedJobNotificationEmailModelReturnListModelFromJson(
  Map<String, dynamic> json,
) => SavedJobNotificationEmailModelReturnListModel(
  model:
      (json['model'] as List<dynamic>?)
          ?.map(
            (e) => SavedJobNotificationEmailModel.fromJson(
              e as Map<String, dynamic>,
            ),
          )
          .toList() ??
      [],
  search: json['search'] == null
      ? null
      : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
  totalRecords: (json['totalRecords'] as num?)?.toInt(),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$SavedJobNotificationEmailModelReturnListModelToJson(
  SavedJobNotificationEmailModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

SavedJobNotificationEmailModelReturnModel
_$SavedJobNotificationEmailModelReturnModelFromJson(
  Map<String, dynamic> json,
) => SavedJobNotificationEmailModelReturnModel(
  model: json['model'] == null
      ? null
      : SavedJobNotificationEmailModel.fromJson(
          json['model'] as Map<String, dynamic>,
        ),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$SavedJobNotificationEmailModelReturnModelToJson(
  SavedJobNotificationEmailModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

SavedJobNotificationEmailUpdateModel
_$SavedJobNotificationEmailUpdateModelFromJson(Map<String, dynamic> json) =>
    SavedJobNotificationEmailUpdateModel(
      savedJobNotificationEmailId: (json['savedJobNotificationEmailId'] as num?)
          ?.toInt(),
      savedJobId: (json['savedJobId'] as num?)?.toInt(),
      email: json['email'] as String?,
      sendStatus: (json['sendStatus'] as num?)?.toInt(),
      message: json['message'] as String?,
    );

Map<String, dynamic> _$SavedJobNotificationEmailUpdateModelToJson(
  SavedJobNotificationEmailUpdateModel instance,
) => <String, dynamic>{
  'savedJobNotificationEmailId': instance.savedJobNotificationEmailId,
  'savedJobId': instance.savedJobId,
  'email': instance.email,
  'sendStatus': instance.sendStatus,
  'message': instance.message,
};

SavedJobUpdateModel _$SavedJobUpdateModelFromJson(Map<String, dynamic> json) =>
    SavedJobUpdateModel(
      savedJobId: (json['savedJobId'] as num?)?.toInt(),
      predefinedJobTypeId: (json['predefinedJobTypeId'] as num?)?.toInt(),
      predefinedJobOrReportTypeId: (json['predefinedJobOrReportTypeId'] as num?)
          ?.toInt(),
      jobTemplateId: (json['jobTemplateId'] as num?)?.toInt(),
      completedDate: json['completedDate'] == null
          ? null
          : DateTime.parse(json['completedDate'] as String),
      submittedDate: json['submittedDate'] == null
          ? null
          : DateTime.parse(json['submittedDate'] as String),
      submittedByUserId: (json['submittedByUserId'] as num?)?.toInt(),
      executableFolder: json['executableFolder'] as String?,
      executableName: json['executableName'] as String?,
      executableParameter: json['executableParameter'] as String?,
      serializedJobParameter: json['serializedJobParameter'] as String?,
      reportTemplateFile: json['reportTemplateFile'] as String?,
      exportDoc: json['exportDoc'] as bool?,
      exportXls: json['exportXls'] as bool?,
      exportPpt: json['exportPpt'] as bool?,
      exportCsv: json['exportCsv'] as bool?,
      exportXml: json['exportXml'] as bool?,
      exportPdf: json['exportPdf'] as bool?,
      exportCustom: json['exportCustom'] as bool?,
      customExportName: json['customExportName'] as String?,
      pickupDate: json['pickupDate'] == null
          ? null
          : DateTime.parse(json['pickupDate'] as String),
      jobName: json['jobName'] as String?,
      userId: (json['userId'] as num?)?.toInt(),
      includeDataSourceFile: json['includeDataSourceFile'] as bool?,
    );

Map<String, dynamic> _$SavedJobUpdateModelToJson(
  SavedJobUpdateModel instance,
) => <String, dynamic>{
  'savedJobId': instance.savedJobId,
  'predefinedJobTypeId': instance.predefinedJobTypeId,
  'predefinedJobOrReportTypeId': instance.predefinedJobOrReportTypeId,
  'jobTemplateId': instance.jobTemplateId,
  'completedDate': instance.completedDate?.toIso8601String(),
  'submittedDate': instance.submittedDate?.toIso8601String(),
  'submittedByUserId': instance.submittedByUserId,
  'executableFolder': instance.executableFolder,
  'executableName': instance.executableName,
  'executableParameter': instance.executableParameter,
  'serializedJobParameter': instance.serializedJobParameter,
  'reportTemplateFile': instance.reportTemplateFile,
  'exportDoc': instance.exportDoc,
  'exportXls': instance.exportXls,
  'exportPpt': instance.exportPpt,
  'exportCsv': instance.exportCsv,
  'exportXml': instance.exportXml,
  'exportPdf': instance.exportPdf,
  'exportCustom': instance.exportCustom,
  'customExportName': instance.customExportName,
  'pickupDate': instance.pickupDate?.toIso8601String(),
  'jobName': instance.jobName,
  'userId': instance.userId,
  'includeDataSourceFile': instance.includeDataSourceFile,
};

ScheduledJobCreateModel _$ScheduledJobCreateModelFromJson(
  Map<String, dynamic> json,
) => ScheduledJobCreateModel(
  jobTemplateId: (json['jobTemplateId'] as num?)?.toInt(),
  predefinedJobTypeId: (json['predefinedJobTypeId'] as num?)?.toInt(),
  predefinedJobOrReportTypeId: (json['predefinedJobOrReportTypeId'] as num?)
      ?.toInt(),
  submittedByUserId: (json['submittedByUserId'] as num?)?.toInt(),
  executableFolder: json['executableFolder'] as String?,
  executableName: json['executableName'] as String?,
  executableParameter: json['executableParameter'] as String?,
  serializedJobParameter: json['serializedJobParameter'] as String?,
  reportTemplateFile: json['reportTemplateFile'] as String?,
  exportDoc: json['exportDoc'] as bool?,
  exportXls: json['exportXls'] as bool?,
  exportPpt: json['exportPpt'] as bool?,
  exportCsv: json['exportCsv'] as bool?,
  exportXml: json['exportXml'] as bool?,
  exportPdf: json['exportPdf'] as bool?,
  exportCustom: json['exportCustom'] as bool?,
  customExportName: json['customExportName'] as String?,
  pickupDate: json['pickupDate'] == null
      ? null
      : DateTime.parse(json['pickupDate'] as String),
  pickupTime: (json['pickupTime'] as num?)?.toInt(),
  jobName: json['jobName'] as String?,
  runAsUserId: (json['runAsUserId'] as num?)?.toInt(),
  startDate: json['startDate'] == null
      ? null
      : DateTime.parse(json['startDate'] as String),
  endDate: json['endDate'] == null
      ? null
      : DateTime.parse(json['endDate'] as String),
  startTime: json['startTime'] == null
      ? null
      : DateTime.parse(json['startTime'] as String),
  recurring: json['recurring'] as bool?,
  runMonday: json['runMonday'] as bool?,
  runTuesday: json['runTuesday'] as bool?,
  runWednesday: json['runWednesday'] as bool?,
  runThursday: json['runThursday'] as bool?,
  runFriday: json['runFriday'] as bool?,
  runSaturday: json['runSaturday'] as bool?,
  runSunday: json['runSunday'] as bool?,
  active: json['active'] as bool?,
  isMonthly: json['isMonthly'] as bool?,
  runMonthlyDay: (json['runMonthlyDay'] as num?)?.toInt(),
  runMonthlyTime: json['runMonthlyTime'] == null
      ? null
      : DateTime.parse(json['runMonthlyTime'] as String),
  isHourly: json['isHourly'] as bool?,
  isDaily: json['isDaily'] as bool?,
  runDailyTime: json['runDailyTime'] == null
      ? null
      : DateTime.parse(json['runDailyTime'] as String),
  isWeekly: json['isWeekly'] as bool?,
  runHourlyMinute: (json['runHourlyMinute'] as num?)?.toInt(),
  includeDataSourceFile: json['includeDataSourceFile'] as bool?,
  runOnWeekNumber: (json['runOnWeekNumber'] as num?)?.toInt(),
);

Map<String, dynamic> _$ScheduledJobCreateModelToJson(
  ScheduledJobCreateModel instance,
) => <String, dynamic>{
  'jobTemplateId': instance.jobTemplateId,
  'predefinedJobTypeId': instance.predefinedJobTypeId,
  'predefinedJobOrReportTypeId': instance.predefinedJobOrReportTypeId,
  'submittedByUserId': instance.submittedByUserId,
  'executableFolder': instance.executableFolder,
  'executableName': instance.executableName,
  'executableParameter': instance.executableParameter,
  'serializedJobParameter': instance.serializedJobParameter,
  'reportTemplateFile': instance.reportTemplateFile,
  'exportDoc': instance.exportDoc,
  'exportXls': instance.exportXls,
  'exportPpt': instance.exportPpt,
  'exportCsv': instance.exportCsv,
  'exportXml': instance.exportXml,
  'exportPdf': instance.exportPdf,
  'exportCustom': instance.exportCustom,
  'customExportName': instance.customExportName,
  'pickupDate': instance.pickupDate?.toIso8601String(),
  'pickupTime': instance.pickupTime,
  'jobName': instance.jobName,
  'runAsUserId': instance.runAsUserId,
  'startDate': instance.startDate?.toIso8601String(),
  'endDate': instance.endDate?.toIso8601String(),
  'startTime': instance.startTime?.toIso8601String(),
  'recurring': instance.recurring,
  'runMonday': instance.runMonday,
  'runTuesday': instance.runTuesday,
  'runWednesday': instance.runWednesday,
  'runThursday': instance.runThursday,
  'runFriday': instance.runFriday,
  'runSaturday': instance.runSaturday,
  'runSunday': instance.runSunday,
  'active': instance.active,
  'isMonthly': instance.isMonthly,
  'runMonthlyDay': instance.runMonthlyDay,
  'runMonthlyTime': instance.runMonthlyTime?.toIso8601String(),
  'isHourly': instance.isHourly,
  'isDaily': instance.isDaily,
  'runDailyTime': instance.runDailyTime?.toIso8601String(),
  'isWeekly': instance.isWeekly,
  'runHourlyMinute': instance.runHourlyMinute,
  'includeDataSourceFile': instance.includeDataSourceFile,
  'runOnWeekNumber': instance.runOnWeekNumber,
};

ScheduledJobHistoryCreateModel _$ScheduledJobHistoryCreateModelFromJson(
  Map<String, dynamic> json,
) => ScheduledJobHistoryCreateModel(
  scheduledJobId: (json['scheduledJobId'] as num?)?.toInt(),
  pickupDate: json['pickupDate'] == null
      ? null
      : DateTime.parse(json['pickupDate'] as String),
  pickupMachineName: json['pickupMachineName'] as String?,
  pickupProcessId: (json['pickupProcessId'] as num?)?.toInt(),
);

Map<String, dynamic> _$ScheduledJobHistoryCreateModelToJson(
  ScheduledJobHistoryCreateModel instance,
) => <String, dynamic>{
  'scheduledJobId': instance.scheduledJobId,
  'pickupDate': instance.pickupDate?.toIso8601String(),
  'pickupMachineName': instance.pickupMachineName,
  'pickupProcessId': instance.pickupProcessId,
};

ScheduledJobHistoryModel _$ScheduledJobHistoryModelFromJson(
  Map<String, dynamic> json,
) => ScheduledJobHistoryModel(
  scheduledJobHistoryId: (json['scheduledJobHistoryId'] as num?)?.toInt(),
  scheduledJobId: (json['scheduledJobId'] as num?)?.toInt(),
  pickupDate: json['pickupDate'] == null
      ? null
      : DateTime.parse(json['pickupDate'] as String),
  pickupMachineName: json['pickupMachineName'] as String?,
  pickupProcessId: (json['pickupProcessId'] as num?)?.toInt(),
  isDeleted: json['isDeleted'] as bool?,
  createdDate: json['createdDate'] == null
      ? null
      : DateTime.parse(json['createdDate'] as String),
  createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
  modifiedDate: json['modifiedDate'] == null
      ? null
      : DateTime.parse(json['modifiedDate'] as String),
  modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
  pickupProcessName: json['pickupProcessName'] as String?,
  createdByUserFirstName: json['createdByUserFirstName'] as String?,
  createdByUserLastName: json['createdByUserLastName'] as String?,
  createdByUserEmail: json['createdByUserEmail'] as String?,
  modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
  modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
  modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
  createdBy: json['createdBy'] as String?,
  modifiedBy: json['modifiedBy'] as String?,
);

Map<String, dynamic> _$ScheduledJobHistoryModelToJson(
  ScheduledJobHistoryModel instance,
) => <String, dynamic>{
  'scheduledJobHistoryId': instance.scheduledJobHistoryId,
  'scheduledJobId': instance.scheduledJobId,
  'pickupDate': instance.pickupDate?.toIso8601String(),
  'pickupMachineName': instance.pickupMachineName,
  'pickupProcessId': instance.pickupProcessId,
  'isDeleted': instance.isDeleted,
  'createdDate': instance.createdDate?.toIso8601String(),
  'createdByUserId': instance.createdByUserId,
  'modifiedDate': instance.modifiedDate?.toIso8601String(),
  'modifiedByUserId': instance.modifiedByUserId,
  'pickupProcessName': instance.pickupProcessName,
  'createdByUserFirstName': instance.createdByUserFirstName,
  'createdByUserLastName': instance.createdByUserLastName,
  'createdByUserEmail': instance.createdByUserEmail,
  'modifiedByUserFirstName': instance.modifiedByUserFirstName,
  'modifiedByUserLastName': instance.modifiedByUserLastName,
  'modifiedByUserEmail': instance.modifiedByUserEmail,
  'createdBy': instance.createdBy,
  'modifiedBy': instance.modifiedBy,
};

ScheduledJobHistoryModelReturnListModel
_$ScheduledJobHistoryModelReturnListModelFromJson(Map<String, dynamic> json) =>
    ScheduledJobHistoryModelReturnListModel(
      model:
          (json['model'] as List<dynamic>?)
              ?.map(
                (e) => ScheduledJobHistoryModel.fromJson(
                  e as Map<String, dynamic>,
                ),
              )
              .toList() ??
          [],
      search: json['search'] == null
          ? null
          : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
      totalRecords: (json['totalRecords'] as num?)?.toInt(),
      isSuccess: json['isSuccess'] as bool?,
      successMessage: json['successMessage'] as String?,
      errorHolder: json['errorHolder'] == null
          ? null
          : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
      activityId: json['activityId'] as String?,
      activityDateTime: json['activityDateTime'] == null
          ? null
          : DateTime.parse(json['activityDateTime'] as String),
      server: json['server'] as String?,
      processId: (json['processId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$ScheduledJobHistoryModelReturnListModelToJson(
  ScheduledJobHistoryModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

ScheduledJobHistoryModelReturnModel
_$ScheduledJobHistoryModelReturnModelFromJson(Map<String, dynamic> json) =>
    ScheduledJobHistoryModelReturnModel(
      model: json['model'] == null
          ? null
          : ScheduledJobHistoryModel.fromJson(
              json['model'] as Map<String, dynamic>,
            ),
      isSuccess: json['isSuccess'] as bool?,
      successMessage: json['successMessage'] as String?,
      errorHolder: json['errorHolder'] == null
          ? null
          : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
      activityId: json['activityId'] as String?,
      activityDateTime: json['activityDateTime'] == null
          ? null
          : DateTime.parse(json['activityDateTime'] as String),
      server: json['server'] as String?,
      processId: (json['processId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$ScheduledJobHistoryModelReturnModelToJson(
  ScheduledJobHistoryModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

ScheduledJobHistoryUpdateModel _$ScheduledJobHistoryUpdateModelFromJson(
  Map<String, dynamic> json,
) => ScheduledJobHistoryUpdateModel(
  scheduledJobHistoryId: (json['scheduledJobHistoryId'] as num?)?.toInt(),
  scheduledJobId: (json['scheduledJobId'] as num?)?.toInt(),
  pickupDate: json['pickupDate'] == null
      ? null
      : DateTime.parse(json['pickupDate'] as String),
  pickupMachineName: json['pickupMachineName'] as String?,
  pickupProcessId: (json['pickupProcessId'] as num?)?.toInt(),
);

Map<String, dynamic> _$ScheduledJobHistoryUpdateModelToJson(
  ScheduledJobHistoryUpdateModel instance,
) => <String, dynamic>{
  'scheduledJobHistoryId': instance.scheduledJobHistoryId,
  'scheduledJobId': instance.scheduledJobId,
  'pickupDate': instance.pickupDate?.toIso8601String(),
  'pickupMachineName': instance.pickupMachineName,
  'pickupProcessId': instance.pickupProcessId,
};

ScheduledJobModel _$ScheduledJobModelFromJson(Map<String, dynamic> json) =>
    ScheduledJobModel(
      scheduledJobId: (json['scheduledJobId'] as num?)?.toInt(),
      jobTemplateId: (json['jobTemplateId'] as num?)?.toInt(),
      predefinedJobTypeId: (json['predefinedJobTypeId'] as num?)?.toInt(),
      predefinedJobOrReportTypeId: (json['predefinedJobOrReportTypeId'] as num?)
          ?.toInt(),
      submittedByUserId: (json['submittedByUserId'] as num?)?.toInt(),
      executableFolder: json['executableFolder'] as String?,
      executableName: json['executableName'] as String?,
      executableParameter: json['executableParameter'] as String?,
      serializedJobParameter: json['serializedJobParameter'] as String?,
      reportTemplateFile: json['reportTemplateFile'] as String?,
      exportDoc: json['exportDoc'] as bool?,
      exportXls: json['exportXls'] as bool?,
      exportPpt: json['exportPpt'] as bool?,
      exportCsv: json['exportCsv'] as bool?,
      exportXml: json['exportXml'] as bool?,
      exportPdf: json['exportPdf'] as bool?,
      exportCustom: json['exportCustom'] as bool?,
      customExportName: json['customExportName'] as String?,
      pickupDate: json['pickupDate'] == null
          ? null
          : DateTime.parse(json['pickupDate'] as String),
      pickupTime: (json['pickupTime'] as num?)?.toInt(),
      jobName: json['jobName'] as String?,
      runAsUserId: (json['runAsUserId'] as num?)?.toInt(),
      startDate: json['startDate'] == null
          ? null
          : DateTime.parse(json['startDate'] as String),
      endDate: json['endDate'] == null
          ? null
          : DateTime.parse(json['endDate'] as String),
      startTime: json['startTime'] == null
          ? null
          : DateTime.parse(json['startTime'] as String),
      recurring: json['recurring'] as bool?,
      runMonday: json['runMonday'] as bool?,
      runTuesday: json['runTuesday'] as bool?,
      runWednesday: json['runWednesday'] as bool?,
      runThursday: json['runThursday'] as bool?,
      runFriday: json['runFriday'] as bool?,
      runSaturday: json['runSaturday'] as bool?,
      runSunday: json['runSunday'] as bool?,
      active: json['active'] as bool?,
      isMonthly: json['isMonthly'] as bool?,
      runMonthlyDay: (json['runMonthlyDay'] as num?)?.toInt(),
      runMonthlyTime: json['runMonthlyTime'] == null
          ? null
          : DateTime.parse(json['runMonthlyTime'] as String),
      isHourly: json['isHourly'] as bool?,
      isDaily: json['isDaily'] as bool?,
      runDailyTime: json['runDailyTime'] == null
          ? null
          : DateTime.parse(json['runDailyTime'] as String),
      isWeekly: json['isWeekly'] as bool?,
      runHourlyMinute: (json['runHourlyMinute'] as num?)?.toInt(),
      includeDataSourceFile: json['includeDataSourceFile'] as bool?,
      runOnWeekNumber: (json['runOnWeekNumber'] as num?)?.toInt(),
      isDeleted: json['isDeleted'] as bool?,
      createdDate: json['createdDate'] == null
          ? null
          : DateTime.parse(json['createdDate'] as String),
      createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
      modifiedDate: json['modifiedDate'] == null
          ? null
          : DateTime.parse(json['modifiedDate'] as String),
      modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
      enumTypeName: json['enumTypeName'] as String?,
      submittedByUserName: json['submittedByUserName'] as String?,
      runAsUserName: json['runAsUserName'] as String?,
      predefinedJobTypeName: json['predefinedJobTypeName'] as String?,
      createdByUserFirstName: json['createdByUserFirstName'] as String?,
      createdByUserLastName: json['createdByUserLastName'] as String?,
      createdByUserEmail: json['createdByUserEmail'] as String?,
      modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
      modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
      modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
      createdBy: json['createdBy'] as String?,
      modifiedBy: json['modifiedBy'] as String?,
      notificationEmails:
          (json['notificationEmails'] as List<dynamic>?)
              ?.map(
                (e) => ScheduledJobNotificationEmailModel.fromJson(
                  e as Map<String, dynamic>,
                ),
              )
              .toList() ??
          [],
    );

Map<String, dynamic> _$ScheduledJobModelToJson(ScheduledJobModel instance) =>
    <String, dynamic>{
      'scheduledJobId': instance.scheduledJobId,
      'jobTemplateId': instance.jobTemplateId,
      'predefinedJobTypeId': instance.predefinedJobTypeId,
      'predefinedJobOrReportTypeId': instance.predefinedJobOrReportTypeId,
      'submittedByUserId': instance.submittedByUserId,
      'executableFolder': instance.executableFolder,
      'executableName': instance.executableName,
      'executableParameter': instance.executableParameter,
      'serializedJobParameter': instance.serializedJobParameter,
      'reportTemplateFile': instance.reportTemplateFile,
      'exportDoc': instance.exportDoc,
      'exportXls': instance.exportXls,
      'exportPpt': instance.exportPpt,
      'exportCsv': instance.exportCsv,
      'exportXml': instance.exportXml,
      'exportPdf': instance.exportPdf,
      'exportCustom': instance.exportCustom,
      'customExportName': instance.customExportName,
      'pickupDate': instance.pickupDate?.toIso8601String(),
      'pickupTime': instance.pickupTime,
      'jobName': instance.jobName,
      'runAsUserId': instance.runAsUserId,
      'startDate': instance.startDate?.toIso8601String(),
      'endDate': instance.endDate?.toIso8601String(),
      'startTime': instance.startTime?.toIso8601String(),
      'recurring': instance.recurring,
      'runMonday': instance.runMonday,
      'runTuesday': instance.runTuesday,
      'runWednesday': instance.runWednesday,
      'runThursday': instance.runThursday,
      'runFriday': instance.runFriday,
      'runSaturday': instance.runSaturday,
      'runSunday': instance.runSunday,
      'active': instance.active,
      'isMonthly': instance.isMonthly,
      'runMonthlyDay': instance.runMonthlyDay,
      'runMonthlyTime': instance.runMonthlyTime?.toIso8601String(),
      'isHourly': instance.isHourly,
      'isDaily': instance.isDaily,
      'runDailyTime': instance.runDailyTime?.toIso8601String(),
      'isWeekly': instance.isWeekly,
      'runHourlyMinute': instance.runHourlyMinute,
      'includeDataSourceFile': instance.includeDataSourceFile,
      'runOnWeekNumber': instance.runOnWeekNumber,
      'isDeleted': instance.isDeleted,
      'createdDate': instance.createdDate?.toIso8601String(),
      'createdByUserId': instance.createdByUserId,
      'modifiedDate': instance.modifiedDate?.toIso8601String(),
      'modifiedByUserId': instance.modifiedByUserId,
      'enumTypeName': instance.enumTypeName,
      'submittedByUserName': instance.submittedByUserName,
      'runAsUserName': instance.runAsUserName,
      'predefinedJobTypeName': instance.predefinedJobTypeName,
      'createdByUserFirstName': instance.createdByUserFirstName,
      'createdByUserLastName': instance.createdByUserLastName,
      'createdByUserEmail': instance.createdByUserEmail,
      'modifiedByUserFirstName': instance.modifiedByUserFirstName,
      'modifiedByUserLastName': instance.modifiedByUserLastName,
      'modifiedByUserEmail': instance.modifiedByUserEmail,
      'createdBy': instance.createdBy,
      'modifiedBy': instance.modifiedBy,
      'notificationEmails': instance.notificationEmails
          ?.map((e) => e.toJson())
          .toList(),
    };

ScheduledJobModelReturnListModel _$ScheduledJobModelReturnListModelFromJson(
  Map<String, dynamic> json,
) => ScheduledJobModelReturnListModel(
  model:
      (json['model'] as List<dynamic>?)
          ?.map((e) => ScheduledJobModel.fromJson(e as Map<String, dynamic>))
          .toList() ??
      [],
  search: json['search'] == null
      ? null
      : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
  totalRecords: (json['totalRecords'] as num?)?.toInt(),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$ScheduledJobModelReturnListModelToJson(
  ScheduledJobModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

ScheduledJobModelReturnModel _$ScheduledJobModelReturnModelFromJson(
  Map<String, dynamic> json,
) => ScheduledJobModelReturnModel(
  model: json['model'] == null
      ? null
      : ScheduledJobModel.fromJson(json['model'] as Map<String, dynamic>),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$ScheduledJobModelReturnModelToJson(
  ScheduledJobModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

ScheduledJobNotificationEmailCreateModel
_$ScheduledJobNotificationEmailCreateModelFromJson(Map<String, dynamic> json) =>
    ScheduledJobNotificationEmailCreateModel(
      scheduledJobId: (json['scheduledJobId'] as num?)?.toInt(),
      email: json['email'] as String?,
      sendStatus: (json['sendStatus'] as num?)?.toInt(),
      message: json['message'] as String?,
    );

Map<String, dynamic> _$ScheduledJobNotificationEmailCreateModelToJson(
  ScheduledJobNotificationEmailCreateModel instance,
) => <String, dynamic>{
  'scheduledJobId': instance.scheduledJobId,
  'email': instance.email,
  'sendStatus': instance.sendStatus,
  'message': instance.message,
};

ScheduledJobNotificationEmailModel _$ScheduledJobNotificationEmailModelFromJson(
  Map<String, dynamic> json,
) => ScheduledJobNotificationEmailModel(
  scheduledJobNotificationEmailId:
      (json['scheduledJobNotificationEmailId'] as num?)?.toInt(),
  scheduledJobId: (json['scheduledJobId'] as num?)?.toInt(),
  email: json['email'] as String?,
  sendStatus: (json['sendStatus'] as num?)?.toInt(),
  message: json['message'] as String?,
  isDeleted: json['isDeleted'] as bool?,
  createdDate: json['createdDate'] == null
      ? null
      : DateTime.parse(json['createdDate'] as String),
  createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
  modifiedDate: json['modifiedDate'] == null
      ? null
      : DateTime.parse(json['modifiedDate'] as String),
  modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
  companyId: (json['companyId'] as num?)?.toInt(),
  createdByUserFirstName: json['createdByUserFirstName'] as String?,
  createdByUserLastName: json['createdByUserLastName'] as String?,
  createdByUserEmail: json['createdByUserEmail'] as String?,
  modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
  modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
  modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
  createdBy: json['createdBy'] as String?,
  modifiedBy: json['modifiedBy'] as String?,
);

Map<String, dynamic> _$ScheduledJobNotificationEmailModelToJson(
  ScheduledJobNotificationEmailModel instance,
) => <String, dynamic>{
  'scheduledJobNotificationEmailId': instance.scheduledJobNotificationEmailId,
  'scheduledJobId': instance.scheduledJobId,
  'email': instance.email,
  'sendStatus': instance.sendStatus,
  'message': instance.message,
  'isDeleted': instance.isDeleted,
  'createdDate': instance.createdDate?.toIso8601String(),
  'createdByUserId': instance.createdByUserId,
  'modifiedDate': instance.modifiedDate?.toIso8601String(),
  'modifiedByUserId': instance.modifiedByUserId,
  'companyId': instance.companyId,
  'createdByUserFirstName': instance.createdByUserFirstName,
  'createdByUserLastName': instance.createdByUserLastName,
  'createdByUserEmail': instance.createdByUserEmail,
  'modifiedByUserFirstName': instance.modifiedByUserFirstName,
  'modifiedByUserLastName': instance.modifiedByUserLastName,
  'modifiedByUserEmail': instance.modifiedByUserEmail,
  'createdBy': instance.createdBy,
  'modifiedBy': instance.modifiedBy,
};

ScheduledJobNotificationEmailModelReturnListModel
_$ScheduledJobNotificationEmailModelReturnListModelFromJson(
  Map<String, dynamic> json,
) => ScheduledJobNotificationEmailModelReturnListModel(
  model:
      (json['model'] as List<dynamic>?)
          ?.map(
            (e) => ScheduledJobNotificationEmailModel.fromJson(
              e as Map<String, dynamic>,
            ),
          )
          .toList() ??
      [],
  search: json['search'] == null
      ? null
      : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
  totalRecords: (json['totalRecords'] as num?)?.toInt(),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$ScheduledJobNotificationEmailModelReturnListModelToJson(
  ScheduledJobNotificationEmailModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

ScheduledJobNotificationEmailModelReturnModel
_$ScheduledJobNotificationEmailModelReturnModelFromJson(
  Map<String, dynamic> json,
) => ScheduledJobNotificationEmailModelReturnModel(
  model: json['model'] == null
      ? null
      : ScheduledJobNotificationEmailModel.fromJson(
          json['model'] as Map<String, dynamic>,
        ),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$ScheduledJobNotificationEmailModelReturnModelToJson(
  ScheduledJobNotificationEmailModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

ScheduledJobNotificationEmailUpdateModel
_$ScheduledJobNotificationEmailUpdateModelFromJson(Map<String, dynamic> json) =>
    ScheduledJobNotificationEmailUpdateModel(
      scheduledJobNotificationEmailId:
          (json['scheduledJobNotificationEmailId'] as num?)?.toInt(),
      scheduledJobId: (json['scheduledJobId'] as num?)?.toInt(),
      email: json['email'] as String?,
      sendStatus: (json['sendStatus'] as num?)?.toInt(),
      message: json['message'] as String?,
    );

Map<String, dynamic> _$ScheduledJobNotificationEmailUpdateModelToJson(
  ScheduledJobNotificationEmailUpdateModel instance,
) => <String, dynamic>{
  'scheduledJobNotificationEmailId': instance.scheduledJobNotificationEmailId,
  'scheduledJobId': instance.scheduledJobId,
  'email': instance.email,
  'sendStatus': instance.sendStatus,
  'message': instance.message,
};

ScheduledJobUpdateModel _$ScheduledJobUpdateModelFromJson(
  Map<String, dynamic> json,
) => ScheduledJobUpdateModel(
  scheduledJobId: (json['scheduledJobId'] as num?)?.toInt(),
  jobTemplateId: (json['jobTemplateId'] as num?)?.toInt(),
  predefinedJobTypeId: (json['predefinedJobTypeId'] as num?)?.toInt(),
  predefinedJobOrReportTypeId: (json['predefinedJobOrReportTypeId'] as num?)
      ?.toInt(),
  submittedByUserId: (json['submittedByUserId'] as num?)?.toInt(),
  executableFolder: json['executableFolder'] as String?,
  executableName: json['executableName'] as String?,
  executableParameter: json['executableParameter'] as String?,
  serializedJobParameter: json['serializedJobParameter'] as String?,
  reportTemplateFile: json['reportTemplateFile'] as String?,
  exportDoc: json['exportDoc'] as bool?,
  exportXls: json['exportXls'] as bool?,
  exportPpt: json['exportPpt'] as bool?,
  exportCsv: json['exportCsv'] as bool?,
  exportXml: json['exportXml'] as bool?,
  exportPdf: json['exportPdf'] as bool?,
  exportCustom: json['exportCustom'] as bool?,
  customExportName: json['customExportName'] as String?,
  pickupDate: json['pickupDate'] == null
      ? null
      : DateTime.parse(json['pickupDate'] as String),
  pickupTime: (json['pickupTime'] as num?)?.toInt(),
  jobName: json['jobName'] as String?,
  runAsUserId: (json['runAsUserId'] as num?)?.toInt(),
  startDate: json['startDate'] == null
      ? null
      : DateTime.parse(json['startDate'] as String),
  endDate: json['endDate'] == null
      ? null
      : DateTime.parse(json['endDate'] as String),
  startTime: json['startTime'] == null
      ? null
      : DateTime.parse(json['startTime'] as String),
  recurring: json['recurring'] as bool?,
  runMonday: json['runMonday'] as bool?,
  runTuesday: json['runTuesday'] as bool?,
  runWednesday: json['runWednesday'] as bool?,
  runThursday: json['runThursday'] as bool?,
  runFriday: json['runFriday'] as bool?,
  runSaturday: json['runSaturday'] as bool?,
  runSunday: json['runSunday'] as bool?,
  active: json['active'] as bool?,
  isMonthly: json['isMonthly'] as bool?,
  runMonthlyDay: (json['runMonthlyDay'] as num?)?.toInt(),
  runMonthlyTime: json['runMonthlyTime'] == null
      ? null
      : DateTime.parse(json['runMonthlyTime'] as String),
  isHourly: json['isHourly'] as bool?,
  isDaily: json['isDaily'] as bool?,
  runDailyTime: json['runDailyTime'] == null
      ? null
      : DateTime.parse(json['runDailyTime'] as String),
  isWeekly: json['isWeekly'] as bool?,
  runHourlyMinute: (json['runHourlyMinute'] as num?)?.toInt(),
  includeDataSourceFile: json['includeDataSourceFile'] as bool?,
  runOnWeekNumber: (json['runOnWeekNumber'] as num?)?.toInt(),
);

Map<String, dynamic> _$ScheduledJobUpdateModelToJson(
  ScheduledJobUpdateModel instance,
) => <String, dynamic>{
  'scheduledJobId': instance.scheduledJobId,
  'jobTemplateId': instance.jobTemplateId,
  'predefinedJobTypeId': instance.predefinedJobTypeId,
  'predefinedJobOrReportTypeId': instance.predefinedJobOrReportTypeId,
  'submittedByUserId': instance.submittedByUserId,
  'executableFolder': instance.executableFolder,
  'executableName': instance.executableName,
  'executableParameter': instance.executableParameter,
  'serializedJobParameter': instance.serializedJobParameter,
  'reportTemplateFile': instance.reportTemplateFile,
  'exportDoc': instance.exportDoc,
  'exportXls': instance.exportXls,
  'exportPpt': instance.exportPpt,
  'exportCsv': instance.exportCsv,
  'exportXml': instance.exportXml,
  'exportPdf': instance.exportPdf,
  'exportCustom': instance.exportCustom,
  'customExportName': instance.customExportName,
  'pickupDate': instance.pickupDate?.toIso8601String(),
  'pickupTime': instance.pickupTime,
  'jobName': instance.jobName,
  'runAsUserId': instance.runAsUserId,
  'startDate': instance.startDate?.toIso8601String(),
  'endDate': instance.endDate?.toIso8601String(),
  'startTime': instance.startTime?.toIso8601String(),
  'recurring': instance.recurring,
  'runMonday': instance.runMonday,
  'runTuesday': instance.runTuesday,
  'runWednesday': instance.runWednesday,
  'runThursday': instance.runThursday,
  'runFriday': instance.runFriday,
  'runSaturday': instance.runSaturday,
  'runSunday': instance.runSunday,
  'active': instance.active,
  'isMonthly': instance.isMonthly,
  'runMonthlyDay': instance.runMonthlyDay,
  'runMonthlyTime': instance.runMonthlyTime?.toIso8601String(),
  'isHourly': instance.isHourly,
  'isDaily': instance.isDaily,
  'runDailyTime': instance.runDailyTime?.toIso8601String(),
  'isWeekly': instance.isWeekly,
  'runHourlyMinute': instance.runHourlyMinute,
  'includeDataSourceFile': instance.includeDataSourceFile,
  'runOnWeekNumber': instance.runOnWeekNumber,
};

SingleSignOnServerCreateModel _$SingleSignOnServerCreateModelFromJson(
  Map<String, dynamic> json,
) => SingleSignOnServerCreateModel(
  singleSignOnServerName: json['singleSignOnServerName'] as String?,
  singleSignOnServerCode: json['singleSignOnServerCode'] as String?,
  predefinedSingleSignOnServerTypeId:
      (json['predefinedSingleSignOnServerTypeId'] as num?)?.toInt(),
  host: json['host'] as String?,
  clientId: json['clientId'] as String?,
  clientTenantId: json['clientTenantId'] as String?,
  clientSecret: json['clientSecret'] as String?,
  forAllCompany: json['forAllCompany'] as bool?,
  isDefault: json['isDefault'] as bool?,
);

Map<String, dynamic> _$SingleSignOnServerCreateModelToJson(
  SingleSignOnServerCreateModel instance,
) => <String, dynamic>{
  'singleSignOnServerName': instance.singleSignOnServerName,
  'singleSignOnServerCode': instance.singleSignOnServerCode,
  'predefinedSingleSignOnServerTypeId':
      instance.predefinedSingleSignOnServerTypeId,
  'host': instance.host,
  'clientId': instance.clientId,
  'clientTenantId': instance.clientTenantId,
  'clientSecret': instance.clientSecret,
  'forAllCompany': instance.forAllCompany,
  'isDefault': instance.isDefault,
};

SingleSignOnServerModel _$SingleSignOnServerModelFromJson(
  Map<String, dynamic> json,
) => SingleSignOnServerModel(
  singleSignOnServerId: (json['singleSignOnServerId'] as num?)?.toInt(),
  singleSignOnServerName: json['singleSignOnServerName'] as String?,
  singleSignOnServerCode: json['singleSignOnServerCode'] as String?,
  predefinedSingleSignOnServerTypeId:
      (json['predefinedSingleSignOnServerTypeId'] as num?)?.toInt(),
  host: json['host'] as String?,
  clientId: json['clientId'] as String?,
  clientTenantId: json['clientTenantId'] as String?,
  clientSecret: json['clientSecret'] as String?,
  forAllCompany: json['forAllCompany'] as bool?,
  isDefault: json['isDefault'] as bool?,
  isDeleted: json['isDeleted'] as bool?,
  createdDate: json['createdDate'] == null
      ? null
      : DateTime.parse(json['createdDate'] as String),
  createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
  modifiedDate: json['modifiedDate'] == null
      ? null
      : DateTime.parse(json['modifiedDate'] as String),
  modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
  singleSignOnServerTypeName: json['singleSignOnServerTypeName'] as String?,
  createdByUserFirstName: json['createdByUserFirstName'] as String?,
  createdByUserLastName: json['createdByUserLastName'] as String?,
  createdByUserEmail: json['createdByUserEmail'] as String?,
  modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
  modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
  modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
  createdBy: json['createdBy'] as String?,
  modifiedBy: json['modifiedBy'] as String?,
);

Map<String, dynamic> _$SingleSignOnServerModelToJson(
  SingleSignOnServerModel instance,
) => <String, dynamic>{
  'singleSignOnServerId': instance.singleSignOnServerId,
  'singleSignOnServerName': instance.singleSignOnServerName,
  'singleSignOnServerCode': instance.singleSignOnServerCode,
  'predefinedSingleSignOnServerTypeId':
      instance.predefinedSingleSignOnServerTypeId,
  'host': instance.host,
  'clientId': instance.clientId,
  'clientTenantId': instance.clientTenantId,
  'clientSecret': instance.clientSecret,
  'forAllCompany': instance.forAllCompany,
  'isDefault': instance.isDefault,
  'isDeleted': instance.isDeleted,
  'createdDate': instance.createdDate?.toIso8601String(),
  'createdByUserId': instance.createdByUserId,
  'modifiedDate': instance.modifiedDate?.toIso8601String(),
  'modifiedByUserId': instance.modifiedByUserId,
  'singleSignOnServerTypeName': instance.singleSignOnServerTypeName,
  'createdByUserFirstName': instance.createdByUserFirstName,
  'createdByUserLastName': instance.createdByUserLastName,
  'createdByUserEmail': instance.createdByUserEmail,
  'modifiedByUserFirstName': instance.modifiedByUserFirstName,
  'modifiedByUserLastName': instance.modifiedByUserLastName,
  'modifiedByUserEmail': instance.modifiedByUserEmail,
  'createdBy': instance.createdBy,
  'modifiedBy': instance.modifiedBy,
};

SingleSignOnServerModelReturnListModel
_$SingleSignOnServerModelReturnListModelFromJson(Map<String, dynamic> json) =>
    SingleSignOnServerModelReturnListModel(
      model:
          (json['model'] as List<dynamic>?)
              ?.map(
                (e) =>
                    SingleSignOnServerModel.fromJson(e as Map<String, dynamic>),
              )
              .toList() ??
          [],
      search: json['search'] == null
          ? null
          : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
      totalRecords: (json['totalRecords'] as num?)?.toInt(),
      isSuccess: json['isSuccess'] as bool?,
      successMessage: json['successMessage'] as String?,
      errorHolder: json['errorHolder'] == null
          ? null
          : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
      activityId: json['activityId'] as String?,
      activityDateTime: json['activityDateTime'] == null
          ? null
          : DateTime.parse(json['activityDateTime'] as String),
      server: json['server'] as String?,
      processId: (json['processId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$SingleSignOnServerModelReturnListModelToJson(
  SingleSignOnServerModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

SingleSignOnServerModelReturnModel _$SingleSignOnServerModelReturnModelFromJson(
  Map<String, dynamic> json,
) => SingleSignOnServerModelReturnModel(
  model: json['model'] == null
      ? null
      : SingleSignOnServerModel.fromJson(json['model'] as Map<String, dynamic>),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$SingleSignOnServerModelReturnModelToJson(
  SingleSignOnServerModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

SingleSignOnServerUpdateModel _$SingleSignOnServerUpdateModelFromJson(
  Map<String, dynamic> json,
) => SingleSignOnServerUpdateModel(
  singleSignOnServerId: (json['singleSignOnServerId'] as num?)?.toInt(),
  singleSignOnServerName: json['singleSignOnServerName'] as String?,
  singleSignOnServerCode: json['singleSignOnServerCode'] as String?,
  predefinedSingleSignOnServerTypeId:
      (json['predefinedSingleSignOnServerTypeId'] as num?)?.toInt(),
  host: json['host'] as String?,
  clientId: json['clientId'] as String?,
  clientTenantId: json['clientTenantId'] as String?,
  clientSecret: json['clientSecret'] as String?,
  forAllCompany: json['forAllCompany'] as bool?,
  isDefault: json['isDefault'] as bool?,
);

Map<String, dynamic> _$SingleSignOnServerUpdateModelToJson(
  SingleSignOnServerUpdateModel instance,
) => <String, dynamic>{
  'singleSignOnServerId': instance.singleSignOnServerId,
  'singleSignOnServerName': instance.singleSignOnServerName,
  'singleSignOnServerCode': instance.singleSignOnServerCode,
  'predefinedSingleSignOnServerTypeId':
      instance.predefinedSingleSignOnServerTypeId,
  'host': instance.host,
  'clientId': instance.clientId,
  'clientTenantId': instance.clientTenantId,
  'clientSecret': instance.clientSecret,
  'forAllCompany': instance.forAllCompany,
  'isDefault': instance.isDefault,
};

SmsServerCreateModel _$SmsServerCreateModelFromJson(
  Map<String, dynamic> json,
) => SmsServerCreateModel(
  smsServerCode: json['smsServerCode'] as String?,
  smsServerName: json['smsServerName'] as String?,
  host: json['host'] as String?,
  port: (json['port'] as num?)?.toInt(),
  senderName: json['senderName'] as String?,
  senderEmail: json['senderEmail'] as String?,
  userName: json['userName'] as String?,
  password: json['password'] as String?,
  isSecure: json['isSecure'] as bool?,
  isDefault: json['isDefault'] as bool?,
  predefinedSmsAndVoiceServerTypeId:
      (json['predefinedSmsAndVoiceServerTypeId'] as num?)?.toInt(),
  twilioAccountSid: json['twilioAccountSid'] as String?,
  twilioAccountAuthToken: json['twilioAccountAuthToken'] as String?,
  twilioAccountPhoneNumber: json['twilioAccountPhoneNumber'] as String?,
);

Map<String, dynamic> _$SmsServerCreateModelToJson(
  SmsServerCreateModel instance,
) => <String, dynamic>{
  'smsServerCode': instance.smsServerCode,
  'smsServerName': instance.smsServerName,
  'host': instance.host,
  'port': instance.port,
  'senderName': instance.senderName,
  'senderEmail': instance.senderEmail,
  'userName': instance.userName,
  'password': instance.password,
  'isSecure': instance.isSecure,
  'isDefault': instance.isDefault,
  'predefinedSmsAndVoiceServerTypeId':
      instance.predefinedSmsAndVoiceServerTypeId,
  'twilioAccountSid': instance.twilioAccountSid,
  'twilioAccountAuthToken': instance.twilioAccountAuthToken,
  'twilioAccountPhoneNumber': instance.twilioAccountPhoneNumber,
};

SmsServerModel _$SmsServerModelFromJson(Map<String, dynamic> json) =>
    SmsServerModel(
      smsServerId: (json['smsServerId'] as num?)?.toInt(),
      smsServerCode: json['smsServerCode'] as String?,
      smsServerName: json['smsServerName'] as String?,
      host: json['host'] as String?,
      port: (json['port'] as num?)?.toInt(),
      senderName: json['senderName'] as String?,
      senderEmail: json['senderEmail'] as String?,
      userName: json['userName'] as String?,
      password: json['password'] as String?,
      isSecure: json['isSecure'] as bool?,
      isDefault: json['isDefault'] as bool?,
      predefinedSmsAndVoiceServerTypeId:
          (json['predefinedSmsAndVoiceServerTypeId'] as num?)?.toInt(),
      twilioAccountSid: json['twilioAccountSid'] as String?,
      twilioAccountAuthToken: json['twilioAccountAuthToken'] as String?,
      twilioAccountPhoneNumber: json['twilioAccountPhoneNumber'] as String?,
      isDeleted: json['isDeleted'] as bool?,
      createdDate: json['createdDate'] == null
          ? null
          : DateTime.parse(json['createdDate'] as String),
      createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
      modifiedDate: json['modifiedDate'] == null
          ? null
          : DateTime.parse(json['modifiedDate'] as String),
      modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
      predefinedSmsAndVoiceServerTypeName:
          json['predefinedSmsAndVoiceServerTypeName'] as String?,
      createdByUserFirstName: json['createdByUserFirstName'] as String?,
      createdByUserLastName: json['createdByUserLastName'] as String?,
      createdByUserEmail: json['createdByUserEmail'] as String?,
      modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
      modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
      modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
      createdBy: json['createdBy'] as String?,
      modifiedBy: json['modifiedBy'] as String?,
    );

Map<String, dynamic> _$SmsServerModelToJson(SmsServerModel instance) =>
    <String, dynamic>{
      'smsServerId': instance.smsServerId,
      'smsServerCode': instance.smsServerCode,
      'smsServerName': instance.smsServerName,
      'host': instance.host,
      'port': instance.port,
      'senderName': instance.senderName,
      'senderEmail': instance.senderEmail,
      'userName': instance.userName,
      'password': instance.password,
      'isSecure': instance.isSecure,
      'isDefault': instance.isDefault,
      'predefinedSmsAndVoiceServerTypeId':
          instance.predefinedSmsAndVoiceServerTypeId,
      'twilioAccountSid': instance.twilioAccountSid,
      'twilioAccountAuthToken': instance.twilioAccountAuthToken,
      'twilioAccountPhoneNumber': instance.twilioAccountPhoneNumber,
      'isDeleted': instance.isDeleted,
      'createdDate': instance.createdDate?.toIso8601String(),
      'createdByUserId': instance.createdByUserId,
      'modifiedDate': instance.modifiedDate?.toIso8601String(),
      'modifiedByUserId': instance.modifiedByUserId,
      'predefinedSmsAndVoiceServerTypeName':
          instance.predefinedSmsAndVoiceServerTypeName,
      'createdByUserFirstName': instance.createdByUserFirstName,
      'createdByUserLastName': instance.createdByUserLastName,
      'createdByUserEmail': instance.createdByUserEmail,
      'modifiedByUserFirstName': instance.modifiedByUserFirstName,
      'modifiedByUserLastName': instance.modifiedByUserLastName,
      'modifiedByUserEmail': instance.modifiedByUserEmail,
      'createdBy': instance.createdBy,
      'modifiedBy': instance.modifiedBy,
    };

SmsServerModelReturnListModel _$SmsServerModelReturnListModelFromJson(
  Map<String, dynamic> json,
) => SmsServerModelReturnListModel(
  model:
      (json['model'] as List<dynamic>?)
          ?.map((e) => SmsServerModel.fromJson(e as Map<String, dynamic>))
          .toList() ??
      [],
  search: json['search'] == null
      ? null
      : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
  totalRecords: (json['totalRecords'] as num?)?.toInt(),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$SmsServerModelReturnListModelToJson(
  SmsServerModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

SmsServerModelReturnModel _$SmsServerModelReturnModelFromJson(
  Map<String, dynamic> json,
) => SmsServerModelReturnModel(
  model: json['model'] == null
      ? null
      : SmsServerModel.fromJson(json['model'] as Map<String, dynamic>),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$SmsServerModelReturnModelToJson(
  SmsServerModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

SmsServerUpdateModel _$SmsServerUpdateModelFromJson(
  Map<String, dynamic> json,
) => SmsServerUpdateModel(
  smsServerId: (json['smsServerId'] as num?)?.toInt(),
  smsServerCode: json['smsServerCode'] as String?,
  smsServerName: json['smsServerName'] as String?,
  host: json['host'] as String?,
  port: (json['port'] as num?)?.toInt(),
  senderName: json['senderName'] as String?,
  senderEmail: json['senderEmail'] as String?,
  userName: json['userName'] as String?,
  password: json['password'] as String?,
  isSecure: json['isSecure'] as bool?,
  isDefault: json['isDefault'] as bool?,
  predefinedSmsAndVoiceServerTypeId:
      (json['predefinedSmsAndVoiceServerTypeId'] as num?)?.toInt(),
  twilioAccountSid: json['twilioAccountSid'] as String?,
  twilioAccountAuthToken: json['twilioAccountAuthToken'] as String?,
  twilioAccountPhoneNumber: json['twilioAccountPhoneNumber'] as String?,
);

Map<String, dynamic> _$SmsServerUpdateModelToJson(
  SmsServerUpdateModel instance,
) => <String, dynamic>{
  'smsServerId': instance.smsServerId,
  'smsServerCode': instance.smsServerCode,
  'smsServerName': instance.smsServerName,
  'host': instance.host,
  'port': instance.port,
  'senderName': instance.senderName,
  'senderEmail': instance.senderEmail,
  'userName': instance.userName,
  'password': instance.password,
  'isSecure': instance.isSecure,
  'isDefault': instance.isDefault,
  'predefinedSmsAndVoiceServerTypeId':
      instance.predefinedSmsAndVoiceServerTypeId,
  'twilioAccountSid': instance.twilioAccountSid,
  'twilioAccountAuthToken': instance.twilioAccountAuthToken,
  'twilioAccountPhoneNumber': instance.twilioAccountPhoneNumber,
};

StateModel _$StateModelFromJson(Map<String, dynamic> json) => StateModel(
  stateId: (json['stateId'] as num?)?.toInt(),
  stateName: json['stateName'] as String?,
  stateCode: json['stateCode'] as String?,
  countryId: (json['countryId'] as num?)?.toInt(),
  isDeleted: json['isDeleted'] as bool?,
  createdDate: json['createdDate'] == null
      ? null
      : DateTime.parse(json['createdDate'] as String),
  createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
  modifiedDate: json['modifiedDate'] == null
      ? null
      : DateTime.parse(json['modifiedDate'] as String),
  modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
  countryName: json['countryName'] as String?,
  createdByUserFirstName: json['createdByUserFirstName'] as String?,
  createdByUserLastName: json['createdByUserLastName'] as String?,
  createdByUserEmail: json['createdByUserEmail'] as String?,
  modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
  modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
  modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
  createdBy: json['createdBy'] as String?,
  modifiedBy: json['modifiedBy'] as String?,
);

Map<String, dynamic> _$StateModelToJson(StateModel instance) =>
    <String, dynamic>{
      'stateId': instance.stateId,
      'stateName': instance.stateName,
      'stateCode': instance.stateCode,
      'countryId': instance.countryId,
      'isDeleted': instance.isDeleted,
      'createdDate': instance.createdDate?.toIso8601String(),
      'createdByUserId': instance.createdByUserId,
      'modifiedDate': instance.modifiedDate?.toIso8601String(),
      'modifiedByUserId': instance.modifiedByUserId,
      'countryName': instance.countryName,
      'createdByUserFirstName': instance.createdByUserFirstName,
      'createdByUserLastName': instance.createdByUserLastName,
      'createdByUserEmail': instance.createdByUserEmail,
      'modifiedByUserFirstName': instance.modifiedByUserFirstName,
      'modifiedByUserLastName': instance.modifiedByUserLastName,
      'modifiedByUserEmail': instance.modifiedByUserEmail,
      'createdBy': instance.createdBy,
      'modifiedBy': instance.modifiedBy,
    };

StateModelReturnListModel _$StateModelReturnListModelFromJson(
  Map<String, dynamic> json,
) => StateModelReturnListModel(
  model:
      (json['model'] as List<dynamic>?)
          ?.map((e) => StateModel.fromJson(e as Map<String, dynamic>))
          .toList() ??
      [],
  search: json['search'] == null
      ? null
      : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
  totalRecords: (json['totalRecords'] as num?)?.toInt(),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$StateModelReturnListModelToJson(
  StateModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

StateModelReturnModel _$StateModelReturnModelFromJson(
  Map<String, dynamic> json,
) => StateModelReturnModel(
  model: json['model'] == null
      ? null
      : StateModel.fromJson(json['model'] as Map<String, dynamic>),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$StateModelReturnModelToJson(
  StateModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

StringReturnListModel _$StringReturnListModelFromJson(
  Map<String, dynamic> json,
) => StringReturnListModel(
  model:
      (json['model'] as List<dynamic>?)?.map((e) => e as String).toList() ?? [],
  search: json['search'] == null
      ? null
      : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
  totalRecords: (json['totalRecords'] as num?)?.toInt(),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$StringReturnListModelToJson(
  StringReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model,
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

SystemSummaryModel _$SystemSummaryModelFromJson(
  Map<String, dynamic> json,
) => SystemSummaryModel(
  usersCount: (json['usersCount'] as num?)?.toInt(),
  currentRunningJobsCount: (json['currentRunningJobsCount'] as num?)?.toInt(),
  messageToSendTotalCount: (json['messageToSendTotalCount'] as num?)?.toInt(),
  messageToSendSuccessCount: (json['messageToSendSuccessCount'] as num?)
      ?.toInt(),
  messageToSendErrorCount: (json['messageToSendErrorCount'] as num?)?.toInt(),
  totalJobCount: (json['totalJobCount'] as num?)?.toInt(),
  jobSuccessCount: (json['jobSuccessCount'] as num?)?.toInt(),
  jobErrorCount: (json['jobErrorCount'] as num?)?.toInt(),
);

Map<String, dynamic> _$SystemSummaryModelToJson(SystemSummaryModel instance) =>
    <String, dynamic>{
      'usersCount': instance.usersCount,
      'currentRunningJobsCount': instance.currentRunningJobsCount,
      'messageToSendTotalCount': instance.messageToSendTotalCount,
      'messageToSendSuccessCount': instance.messageToSendSuccessCount,
      'messageToSendErrorCount': instance.messageToSendErrorCount,
      'totalJobCount': instance.totalJobCount,
      'jobSuccessCount': instance.jobSuccessCount,
      'jobErrorCount': instance.jobErrorCount,
    };

SystemSummaryModelReturnModel _$SystemSummaryModelReturnModelFromJson(
  Map<String, dynamic> json,
) => SystemSummaryModelReturnModel(
  model: json['model'] == null
      ? null
      : SystemSummaryModel.fromJson(json['model'] as Map<String, dynamic>),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$SystemSummaryModelReturnModelToJson(
  SystemSummaryModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

TextWithValue _$TextWithValueFromJson(Map<String, dynamic> json) =>
    TextWithValue(
      text: json['text'] as String?,
      $value: json['value'] as String?,
      code: json['code'] as String?,
    );

Map<String, dynamic> _$TextWithValueToJson(TextWithValue instance) =>
    <String, dynamic>{
      'text': instance.text,
      'value': instance.$value,
      'code': instance.code,
    };

UserByCategoryIdSearchCriteria _$UserByCategoryIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => UserByCategoryIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  categoryId: (json['categoryId'] as num?)?.toInt(),
);

Map<String, dynamic> _$UserByCategoryIdSearchCriteriaToJson(
  UserByCategoryIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'categoryId': instance.categoryId,
};

UserByDailyAffirmationSessionCategoryIdSearchCriteria
_$UserByDailyAffirmationSessionCategoryIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => UserByDailyAffirmationSessionCategoryIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  dailyAffirmationSessionCategoryId:
      (json['dailyAffirmationSessionCategoryId'] as num?)?.toInt(),
);

Map<String, dynamic>
_$UserByDailyAffirmationSessionCategoryIdSearchCriteriaToJson(
  UserByDailyAffirmationSessionCategoryIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'dailyAffirmationSessionCategoryId':
      instance.dailyAffirmationSessionCategoryId,
};

UserByDailyAffirmationSessionIdSearchCriteria
_$UserByDailyAffirmationSessionIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => UserByDailyAffirmationSessionIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  dailyAffirmationSessionId: (json['dailyAffirmationSessionId'] as num?)
      ?.toInt(),
);

Map<String, dynamic> _$UserByDailyAffirmationSessionIdSearchCriteriaToJson(
  UserByDailyAffirmationSessionIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'dailyAffirmationSessionId': instance.dailyAffirmationSessionId,
};

UserByDailyAffirmationTextIdSearchCriteria
_$UserByDailyAffirmationTextIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => UserByDailyAffirmationTextIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  dailyAffirmationTextId: (json['dailyAffirmationTextId'] as num?)?.toInt(),
);

Map<String, dynamic> _$UserByDailyAffirmationTextIdSearchCriteriaToJson(
  UserByDailyAffirmationTextIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'dailyAffirmationTextId': instance.dailyAffirmationTextId,
};

UserByDailyJournalIdSearchCriteria _$UserByDailyJournalIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => UserByDailyJournalIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  dailyJournalId: (json['dailyJournalId'] as num?)?.toInt(),
);

Map<String, dynamic> _$UserByDailyJournalIdSearchCriteriaToJson(
  UserByDailyJournalIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'dailyJournalId': instance.dailyJournalId,
};

UserByFeedIdSearchCriteria _$UserByFeedIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => UserByFeedIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  feedId: (json['feedId'] as num?)?.toInt(),
);

Map<String, dynamic> _$UserByFeedIdSearchCriteriaToJson(
  UserByFeedIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'feedId': instance.feedId,
};

UserByGoalTrackerCategoryIdSearchCriteria
_$UserByGoalTrackerCategoryIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => UserByGoalTrackerCategoryIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  goalTrackerCategoryId: (json['goalTrackerCategoryId'] as num?)?.toInt(),
);

Map<String, dynamic> _$UserByGoalTrackerCategoryIdSearchCriteriaToJson(
  UserByGoalTrackerCategoryIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'goalTrackerCategoryId': instance.goalTrackerCategoryId,
};

UserByGoalTrackerIdSearchCriteria _$UserByGoalTrackerIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => UserByGoalTrackerIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  goalTrackerId: (json['goalTrackerId'] as num?)?.toInt(),
);

Map<String, dynamic> _$UserByGoalTrackerIdSearchCriteriaToJson(
  UserByGoalTrackerIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'goalTrackerId': instance.goalTrackerId,
};

UserByGoalTrackerMileStoneIdSearchCriteria
_$UserByGoalTrackerMileStoneIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => UserByGoalTrackerMileStoneIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  goalTrackerMileStoneId: (json['goalTrackerMileStoneId'] as num?)?.toInt(),
);

Map<String, dynamic> _$UserByGoalTrackerMileStoneIdSearchCriteriaToJson(
  UserByGoalTrackerMileStoneIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'goalTrackerMileStoneId': instance.goalTrackerMileStoneId,
};

UserByMeditationFileIdSearchCriteria
_$UserByMeditationFileIdSearchCriteriaFromJson(Map<String, dynamic> json) =>
    UserByMeditationFileIdSearchCriteria(
      selectedId: (json['selectedId'] as num?)?.toInt(),
      page: (json['page'] as num?)?.toInt(),
      pageSize: (json['pageSize'] as num?)?.toInt(),
      keyword: json['keyword'] as String?,
      sortBy: json['sortBy'] as String?,
      sortAscending: json['sortAscending'] as bool?,
      meditationFileId: (json['meditationFileId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$UserByMeditationFileIdSearchCriteriaToJson(
  UserByMeditationFileIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'meditationFileId': instance.meditationFileId,
};

UserByMeditationSessionIdSearchCriteria
_$UserByMeditationSessionIdSearchCriteriaFromJson(Map<String, dynamic> json) =>
    UserByMeditationSessionIdSearchCriteria(
      selectedId: (json['selectedId'] as num?)?.toInt(),
      page: (json['page'] as num?)?.toInt(),
      pageSize: (json['pageSize'] as num?)?.toInt(),
      keyword: json['keyword'] as String?,
      sortBy: json['sortBy'] as String?,
      sortAscending: json['sortAscending'] as bool?,
      meditationSessionId: (json['meditationSessionId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$UserByMeditationSessionIdSearchCriteriaToJson(
  UserByMeditationSessionIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'meditationSessionId': instance.meditationSessionId,
};

UserByVisionBoardPictureIdSearchCriteria
_$UserByVisionBoardPictureIdSearchCriteriaFromJson(Map<String, dynamic> json) =>
    UserByVisionBoardPictureIdSearchCriteria(
      selectedId: (json['selectedId'] as num?)?.toInt(),
      page: (json['page'] as num?)?.toInt(),
      pageSize: (json['pageSize'] as num?)?.toInt(),
      keyword: json['keyword'] as String?,
      sortBy: json['sortBy'] as String?,
      sortAscending: json['sortAscending'] as bool?,
      visionBoardPictureId: (json['visionBoardPictureId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$UserByVisionBoardPictureIdSearchCriteriaToJson(
  UserByVisionBoardPictureIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'visionBoardPictureId': instance.visionBoardPictureId,
};

UserByVisionBoardSessionIdSearchCriteria
_$UserByVisionBoardSessionIdSearchCriteriaFromJson(Map<String, dynamic> json) =>
    UserByVisionBoardSessionIdSearchCriteria(
      selectedId: (json['selectedId'] as num?)?.toInt(),
      page: (json['page'] as num?)?.toInt(),
      pageSize: (json['pageSize'] as num?)?.toInt(),
      keyword: json['keyword'] as String?,
      sortBy: json['sortBy'] as String?,
      sortAscending: json['sortAscending'] as bool?,
      visionBoardSessionId: (json['visionBoardSessionId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$UserByVisionBoardSessionIdSearchCriteriaToJson(
  UserByVisionBoardSessionIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'visionBoardSessionId': instance.visionBoardSessionId,
};

UserByVisionBoardSessionPictureIdSearchCriteria
_$UserByVisionBoardSessionPictureIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => UserByVisionBoardSessionPictureIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  visionBoardSessionPictureId: (json['visionBoardSessionPictureId'] as num?)
      ?.toInt(),
);

Map<String, dynamic> _$UserByVisionBoardSessionPictureIdSearchCriteriaToJson(
  UserByVisionBoardSessionPictureIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'visionBoardSessionPictureId': instance.visionBoardSessionPictureId,
};

UserGroupCreateModel _$UserGroupCreateModelFromJson(
  Map<String, dynamic> json,
) => UserGroupCreateModel(userGroupName: json['userGroupName'] as String?);

Map<String, dynamic> _$UserGroupCreateModelToJson(
  UserGroupCreateModel instance,
) => <String, dynamic>{'userGroupName': instance.userGroupName};

UserGroupModel _$UserGroupModelFromJson(Map<String, dynamic> json) =>
    UserGroupModel(
      userGroupId: (json['userGroupId'] as num?)?.toInt(),
      userGroupName: json['userGroupName'] as String?,
      isDeleted: json['isDeleted'] as bool?,
      createdDate: json['createdDate'] == null
          ? null
          : DateTime.parse(json['createdDate'] as String),
      createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
      modifiedDate: json['modifiedDate'] == null
          ? null
          : DateTime.parse(json['modifiedDate'] as String),
      modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
      companyEntityName: json['companyEntityName'] as String?,
      createdByUserFirstName: json['createdByUserFirstName'] as String?,
      createdByUserLastName: json['createdByUserLastName'] as String?,
      createdByUserEmail: json['createdByUserEmail'] as String?,
      modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
      modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
      modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
      createdBy: json['createdBy'] as String?,
      modifiedBy: json['modifiedBy'] as String?,
    );

Map<String, dynamic> _$UserGroupModelToJson(UserGroupModel instance) =>
    <String, dynamic>{
      'userGroupId': instance.userGroupId,
      'userGroupName': instance.userGroupName,
      'isDeleted': instance.isDeleted,
      'createdDate': instance.createdDate?.toIso8601String(),
      'createdByUserId': instance.createdByUserId,
      'modifiedDate': instance.modifiedDate?.toIso8601String(),
      'modifiedByUserId': instance.modifiedByUserId,
      'companyEntityName': instance.companyEntityName,
      'createdByUserFirstName': instance.createdByUserFirstName,
      'createdByUserLastName': instance.createdByUserLastName,
      'createdByUserEmail': instance.createdByUserEmail,
      'modifiedByUserFirstName': instance.modifiedByUserFirstName,
      'modifiedByUserLastName': instance.modifiedByUserLastName,
      'modifiedByUserEmail': instance.modifiedByUserEmail,
      'createdBy': instance.createdBy,
      'modifiedBy': instance.modifiedBy,
    };

UserGroupModelReturnListModel _$UserGroupModelReturnListModelFromJson(
  Map<String, dynamic> json,
) => UserGroupModelReturnListModel(
  model:
      (json['model'] as List<dynamic>?)
          ?.map((e) => UserGroupModel.fromJson(e as Map<String, dynamic>))
          .toList() ??
      [],
  search: json['search'] == null
      ? null
      : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
  totalRecords: (json['totalRecords'] as num?)?.toInt(),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$UserGroupModelReturnListModelToJson(
  UserGroupModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

UserGroupModelReturnModel _$UserGroupModelReturnModelFromJson(
  Map<String, dynamic> json,
) => UserGroupModelReturnModel(
  model: json['model'] == null
      ? null
      : UserGroupModel.fromJson(json['model'] as Map<String, dynamic>),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$UserGroupModelReturnModelToJson(
  UserGroupModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

UserGroupUpdateModel _$UserGroupUpdateModelFromJson(
  Map<String, dynamic> json,
) => UserGroupUpdateModel(
  userGroupId: (json['userGroupId'] as num?)?.toInt(),
  userGroupName: json['userGroupName'] as String?,
);

Map<String, dynamic> _$UserGroupUpdateModelToJson(
  UserGroupUpdateModel instance,
) => <String, dynamic>{
  'userGroupId': instance.userGroupId,
  'userGroupName': instance.userGroupName,
};

UserModel _$UserModelFromJson(Map<String, dynamic> json) => UserModel(
  userId: (json['userId'] as num?)?.toInt(),
  userGuid: json['userGuid'] as String?,
  userName: json['userName'] as String?,
  predefinedUserTypeId: (json['predefinedUserTypeId'] as num?)?.toInt(),
  email: json['email'] as String?,
  firstName: json['firstName'] as String?,
  lastName: json['lastName'] as String?,
  workPhone: json['workPhone'] as String?,
  workPhoneExt: json['workPhoneExt'] as String?,
  cellPhone: json['cellPhone'] as String?,
  active: json['active'] as bool?,
  notificationThroughEmail: json['notificationThroughEmail'] as bool?,
  isAccountLocked: json['isAccountLocked'] as bool?,
  accountLockedUntil: json['accountLockedUntil'] == null
      ? null
      : DateTime.parse(json['accountLockedUntil'] as String),
  profileImageGuid: json['profileImageGuid'] as String?,
  profileImageActualFileName: json['profileImageActualFileName'] as String?,
  hasUserPurchasedPremiumFeature:
      json['hasUserPurchasedPremiumFeature'] as bool?,
  premiumFeatureValidUntil: json['premiumFeatureValidUntil'] == null
      ? null
      : DateTime.parse(json['premiumFeatureValidUntil'] as String),
  isDeleted: json['isDeleted'] as bool?,
  createdDate: json['createdDate'] == null
      ? null
      : DateTime.parse(json['createdDate'] as String),
  createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
  modifiedDate: json['modifiedDate'] == null
      ? null
      : DateTime.parse(json['modifiedDate'] as String),
  modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
  predefinedUserTypeName: json['predefinedUserTypeName'] as String?,
  modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
  modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
  modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
  existingPermissionTypeIds:
      (json['existingPermissionTypeIds'] as List<dynamic>?)
          ?.map((e) => (e as num).toInt())
          .toList() ??
      [],
);

Map<String, dynamic> _$UserModelToJson(UserModel instance) => <String, dynamic>{
  'userId': instance.userId,
  'userGuid': instance.userGuid,
  'userName': instance.userName,
  'predefinedUserTypeId': instance.predefinedUserTypeId,
  'email': instance.email,
  'firstName': instance.firstName,
  'lastName': instance.lastName,
  'workPhone': instance.workPhone,
  'workPhoneExt': instance.workPhoneExt,
  'cellPhone': instance.cellPhone,
  'active': instance.active,
  'notificationThroughEmail': instance.notificationThroughEmail,
  'isAccountLocked': instance.isAccountLocked,
  'accountLockedUntil': instance.accountLockedUntil?.toIso8601String(),
  'profileImageGuid': instance.profileImageGuid,
  'profileImageActualFileName': instance.profileImageActualFileName,
  'hasUserPurchasedPremiumFeature': instance.hasUserPurchasedPremiumFeature,
  'premiumFeatureValidUntil': instance.premiumFeatureValidUntil
      ?.toIso8601String(),
  'isDeleted': instance.isDeleted,
  'createdDate': instance.createdDate?.toIso8601String(),
  'createdByUserId': instance.createdByUserId,
  'modifiedDate': instance.modifiedDate?.toIso8601String(),
  'modifiedByUserId': instance.modifiedByUserId,
  'predefinedUserTypeName': instance.predefinedUserTypeName,
  'modifiedByUserFirstName': instance.modifiedByUserFirstName,
  'modifiedByUserLastName': instance.modifiedByUserLastName,
  'modifiedByUserEmail': instance.modifiedByUserEmail,
  'existingPermissionTypeIds': instance.existingPermissionTypeIds,
};

UserModelReturnListModel _$UserModelReturnListModelFromJson(
  Map<String, dynamic> json,
) => UserModelReturnListModel(
  model:
      (json['model'] as List<dynamic>?)
          ?.map((e) => UserModel.fromJson(e as Map<String, dynamic>))
          .toList() ??
      [],
  search: json['search'] == null
      ? null
      : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
  totalRecords: (json['totalRecords'] as num?)?.toInt(),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$UserModelReturnListModelToJson(
  UserModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

UserModelReturnModel _$UserModelReturnModelFromJson(
  Map<String, dynamic> json,
) => UserModelReturnModel(
  model: json['model'] == null
      ? null
      : UserModel.fromJson(json['model'] as Map<String, dynamic>),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$UserModelReturnModelToJson(
  UserModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

UserPermissionCreateModel _$UserPermissionCreateModelFromJson(
  Map<String, dynamic> json,
) => UserPermissionCreateModel(
  userId: (json['userId'] as num?)?.toInt(),
  predefinedUserPermissionTypeId:
      (json['predefinedUserPermissionTypeId'] as num?)?.toInt(),
);

Map<String, dynamic> _$UserPermissionCreateModelToJson(
  UserPermissionCreateModel instance,
) => <String, dynamic>{
  'userId': instance.userId,
  'predefinedUserPermissionTypeId': instance.predefinedUserPermissionTypeId,
};

UserPermissionModel _$UserPermissionModelFromJson(Map<String, dynamic> json) =>
    UserPermissionModel(
      userPermissionId: (json['userPermissionId'] as num?)?.toInt(),
      userId: (json['userId'] as num?)?.toInt(),
      predefinedUserPermissionTypeId:
          (json['predefinedUserPermissionTypeId'] as num?)?.toInt(),
      isDeleted: json['isDeleted'] as bool?,
      createdDate: json['createdDate'] == null
          ? null
          : DateTime.parse(json['createdDate'] as String),
      createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
      modifiedDate: json['modifiedDate'] == null
          ? null
          : DateTime.parse(json['modifiedDate'] as String),
      modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
      userName: json['userName'] as String?,
      predefinedUserPermissionTypeName:
          json['predefinedUserPermissionTypeName'] as String?,
      createdByUserFirstName: json['createdByUserFirstName'] as String?,
      createdByUserLastName: json['createdByUserLastName'] as String?,
      createdByUserEmail: json['createdByUserEmail'] as String?,
      modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
      modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
      modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
      createdBy: json['createdBy'] as String?,
      modifiedBy: json['modifiedBy'] as String?,
    );

Map<String, dynamic> _$UserPermissionModelToJson(
  UserPermissionModel instance,
) => <String, dynamic>{
  'userPermissionId': instance.userPermissionId,
  'userId': instance.userId,
  'predefinedUserPermissionTypeId': instance.predefinedUserPermissionTypeId,
  'isDeleted': instance.isDeleted,
  'createdDate': instance.createdDate?.toIso8601String(),
  'createdByUserId': instance.createdByUserId,
  'modifiedDate': instance.modifiedDate?.toIso8601String(),
  'modifiedByUserId': instance.modifiedByUserId,
  'userName': instance.userName,
  'predefinedUserPermissionTypeName': instance.predefinedUserPermissionTypeName,
  'createdByUserFirstName': instance.createdByUserFirstName,
  'createdByUserLastName': instance.createdByUserLastName,
  'createdByUserEmail': instance.createdByUserEmail,
  'modifiedByUserFirstName': instance.modifiedByUserFirstName,
  'modifiedByUserLastName': instance.modifiedByUserLastName,
  'modifiedByUserEmail': instance.modifiedByUserEmail,
  'createdBy': instance.createdBy,
  'modifiedBy': instance.modifiedBy,
};

UserPermissionModelReturnListModel _$UserPermissionModelReturnListModelFromJson(
  Map<String, dynamic> json,
) => UserPermissionModelReturnListModel(
  model:
      (json['model'] as List<dynamic>?)
          ?.map((e) => UserPermissionModel.fromJson(e as Map<String, dynamic>))
          .toList() ??
      [],
  search: json['search'] == null
      ? null
      : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
  totalRecords: (json['totalRecords'] as num?)?.toInt(),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$UserPermissionModelReturnListModelToJson(
  UserPermissionModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

UserPermissionModelReturnModel _$UserPermissionModelReturnModelFromJson(
  Map<String, dynamic> json,
) => UserPermissionModelReturnModel(
  model: json['model'] == null
      ? null
      : UserPermissionModel.fromJson(json['model'] as Map<String, dynamic>),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$UserPermissionModelReturnModelToJson(
  UserPermissionModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

UserPermissionUpdateModel _$UserPermissionUpdateModelFromJson(
  Map<String, dynamic> json,
) => UserPermissionUpdateModel(
  userPermissionId: (json['userPermissionId'] as num?)?.toInt(),
  userId: (json['userId'] as num?)?.toInt(),
  predefinedUserPermissionTypeId:
      (json['predefinedUserPermissionTypeId'] as num?)?.toInt(),
);

Map<String, dynamic> _$UserPermissionUpdateModelToJson(
  UserPermissionUpdateModel instance,
) => <String, dynamic>{
  'userPermissionId': instance.userPermissionId,
  'userId': instance.userId,
  'predefinedUserPermissionTypeId': instance.predefinedUserPermissionTypeId,
};

UserProfileModel _$UserProfileModelFromJson(Map<String, dynamic> json) =>
    UserProfileModel(
      userId: (json['userId'] as num?)?.toInt(),
      isPrivate: json['isPrivate'] as bool?,
      userName: json['userName'] as String?,
      firstName: json['firstName'] as String?,
      lastName: json['lastName'] as String?,
      email: json['email'] as String?,
      workPhone: json['workPhone'] as String?,
      cellPhone: json['cellPhone'] as String?,
      billingAddress1: json['billingAddress1'] as String?,
      billingAddress2: json['billingAddress2'] as String?,
      locationCity: json['locationCity'] as String?,
      locationState: json['locationState'] as String?,
      locationZip: json['locationZip'] as String?,
      locationCountry: json['locationCountry'] as String?,
      countryGuid: (json['countryGuid'] as num?)?.toInt(),
      stateGuid: (json['stateGuid'] as num?)?.toInt(),
    );

Map<String, dynamic> _$UserProfileModelToJson(UserProfileModel instance) =>
    <String, dynamic>{
      'userId': instance.userId,
      'isPrivate': instance.isPrivate,
      'userName': instance.userName,
      'firstName': instance.firstName,
      'lastName': instance.lastName,
      'email': instance.email,
      'workPhone': instance.workPhone,
      'cellPhone': instance.cellPhone,
      'billingAddress1': instance.billingAddress1,
      'billingAddress2': instance.billingAddress2,
      'locationCity': instance.locationCity,
      'locationState': instance.locationState,
      'locationZip': instance.locationZip,
      'locationCountry': instance.locationCountry,
      'countryGuid': instance.countryGuid,
      'stateGuid': instance.stateGuid,
    };

UserProfileModelReturnModel _$UserProfileModelReturnModelFromJson(
  Map<String, dynamic> json,
) => UserProfileModelReturnModel(
  model: json['model'] == null
      ? null
      : UserProfileModel.fromJson(json['model'] as Map<String, dynamic>),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$UserProfileModelReturnModelToJson(
  UserProfileModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

VisionBoardPictureByUserIdSearchCriteria
_$VisionBoardPictureByUserIdSearchCriteriaFromJson(Map<String, dynamic> json) =>
    VisionBoardPictureByUserIdSearchCriteria(
      selectedId: (json['selectedId'] as num?)?.toInt(),
      page: (json['page'] as num?)?.toInt(),
      pageSize: (json['pageSize'] as num?)?.toInt(),
      keyword: json['keyword'] as String?,
      sortBy: json['sortBy'] as String?,
      sortAscending: json['sortAscending'] as bool?,
      userId: (json['userId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$VisionBoardPictureByUserIdSearchCriteriaToJson(
  VisionBoardPictureByUserIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'userId': instance.userId,
};

VisionBoardPictureByVisionBoardSessionPictureIdSearchCriteria
_$VisionBoardPictureByVisionBoardSessionPictureIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => VisionBoardPictureByVisionBoardSessionPictureIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  visionBoardSessionPictureId: (json['visionBoardSessionPictureId'] as num?)
      ?.toInt(),
);

Map<String, dynamic>
_$VisionBoardPictureByVisionBoardSessionPictureIdSearchCriteriaToJson(
  VisionBoardPictureByVisionBoardSessionPictureIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'visionBoardSessionPictureId': instance.visionBoardSessionPictureId,
};

VisionBoardPictureCreateModel _$VisionBoardPictureCreateModelFromJson(
  Map<String, dynamic> json,
) => VisionBoardPictureCreateModel(
  title: json['title'] as String?,
  location: json['location'] as String?,
  fileName: json['fileName'] as String?,
  fileSize: (json['fileSize'] as num?)?.toInt(),
  thumbnailLocation: json['thumbnailLocation'] as String?,
  thumbnailFileName: json['thumbnailFileName'] as String?,
  thumbnailFileSize: (json['thumbnailFileSize'] as num?)?.toInt(),
  userId: (json['userId'] as num?)?.toInt(),
);

Map<String, dynamic> _$VisionBoardPictureCreateModelToJson(
  VisionBoardPictureCreateModel instance,
) => <String, dynamic>{
  'title': instance.title,
  'location': instance.location,
  'fileName': instance.fileName,
  'fileSize': instance.fileSize,
  'thumbnailLocation': instance.thumbnailLocation,
  'thumbnailFileName': instance.thumbnailFileName,
  'thumbnailFileSize': instance.thumbnailFileSize,
  'userId': instance.userId,
};

VisionBoardPictureModel _$VisionBoardPictureModelFromJson(
  Map<String, dynamic> json,
) => VisionBoardPictureModel(
  visionBoardPictureId: (json['visionBoardPictureId'] as num?)?.toInt(),
  title: json['title'] as String?,
  location: json['location'] as String?,
  fileName: json['fileName'] as String?,
  fileSize: (json['fileSize'] as num?)?.toInt(),
  thumbnailLocation: json['thumbnailLocation'] as String?,
  thumbnailFileName: json['thumbnailFileName'] as String?,
  thumbnailFileSize: (json['thumbnailFileSize'] as num?)?.toInt(),
  userId: (json['userId'] as num?)?.toInt(),
  isDeleted: json['isDeleted'] as bool?,
  createdDate: json['createdDate'] == null
      ? null
      : DateTime.parse(json['createdDate'] as String),
  createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
  modifiedDate: json['modifiedDate'] == null
      ? null
      : DateTime.parse(json['modifiedDate'] as String),
  modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
  userName: json['userName'] as String?,
  createdByUserFirstName: json['createdByUserFirstName'] as String?,
  createdByUserLastName: json['createdByUserLastName'] as String?,
  createdByUserEmail: json['createdByUserEmail'] as String?,
  modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
  modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
  modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
  createdBy: json['createdBy'] as String?,
  modifiedBy: json['modifiedBy'] as String?,
);

Map<String, dynamic> _$VisionBoardPictureModelToJson(
  VisionBoardPictureModel instance,
) => <String, dynamic>{
  'visionBoardPictureId': instance.visionBoardPictureId,
  'title': instance.title,
  'location': instance.location,
  'fileName': instance.fileName,
  'fileSize': instance.fileSize,
  'thumbnailLocation': instance.thumbnailLocation,
  'thumbnailFileName': instance.thumbnailFileName,
  'thumbnailFileSize': instance.thumbnailFileSize,
  'userId': instance.userId,
  'isDeleted': instance.isDeleted,
  'createdDate': instance.createdDate?.toIso8601String(),
  'createdByUserId': instance.createdByUserId,
  'modifiedDate': instance.modifiedDate?.toIso8601String(),
  'modifiedByUserId': instance.modifiedByUserId,
  'userName': instance.userName,
  'createdByUserFirstName': instance.createdByUserFirstName,
  'createdByUserLastName': instance.createdByUserLastName,
  'createdByUserEmail': instance.createdByUserEmail,
  'modifiedByUserFirstName': instance.modifiedByUserFirstName,
  'modifiedByUserLastName': instance.modifiedByUserLastName,
  'modifiedByUserEmail': instance.modifiedByUserEmail,
  'createdBy': instance.createdBy,
  'modifiedBy': instance.modifiedBy,
};

VisionBoardPictureModelReturnListModel
_$VisionBoardPictureModelReturnListModelFromJson(Map<String, dynamic> json) =>
    VisionBoardPictureModelReturnListModel(
      model:
          (json['model'] as List<dynamic>?)
              ?.map(
                (e) =>
                    VisionBoardPictureModel.fromJson(e as Map<String, dynamic>),
              )
              .toList() ??
          [],
      search: json['search'] == null
          ? null
          : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
      totalRecords: (json['totalRecords'] as num?)?.toInt(),
      isSuccess: json['isSuccess'] as bool?,
      successMessage: json['successMessage'] as String?,
      errorHolder: json['errorHolder'] == null
          ? null
          : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
      activityId: json['activityId'] as String?,
      activityDateTime: json['activityDateTime'] == null
          ? null
          : DateTime.parse(json['activityDateTime'] as String),
      server: json['server'] as String?,
      processId: (json['processId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$VisionBoardPictureModelReturnListModelToJson(
  VisionBoardPictureModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

VisionBoardPictureModelReturnModel _$VisionBoardPictureModelReturnModelFromJson(
  Map<String, dynamic> json,
) => VisionBoardPictureModelReturnModel(
  model: json['model'] == null
      ? null
      : VisionBoardPictureModel.fromJson(json['model'] as Map<String, dynamic>),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$VisionBoardPictureModelReturnModelToJson(
  VisionBoardPictureModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

VisionBoardSessionByFeedIdSearchCriteria
_$VisionBoardSessionByFeedIdSearchCriteriaFromJson(Map<String, dynamic> json) =>
    VisionBoardSessionByFeedIdSearchCriteria(
      selectedId: (json['selectedId'] as num?)?.toInt(),
      page: (json['page'] as num?)?.toInt(),
      pageSize: (json['pageSize'] as num?)?.toInt(),
      keyword: json['keyword'] as String?,
      sortBy: json['sortBy'] as String?,
      sortAscending: json['sortAscending'] as bool?,
      feedId: (json['feedId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$VisionBoardSessionByFeedIdSearchCriteriaToJson(
  VisionBoardSessionByFeedIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'feedId': instance.feedId,
};

VisionBoardSessionByUserIdSearchCriteria
_$VisionBoardSessionByUserIdSearchCriteriaFromJson(Map<String, dynamic> json) =>
    VisionBoardSessionByUserIdSearchCriteria(
      selectedId: (json['selectedId'] as num?)?.toInt(),
      page: (json['page'] as num?)?.toInt(),
      pageSize: (json['pageSize'] as num?)?.toInt(),
      keyword: json['keyword'] as String?,
      sortBy: json['sortBy'] as String?,
      sortAscending: json['sortAscending'] as bool?,
      userId: (json['userId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$VisionBoardSessionByUserIdSearchCriteriaToJson(
  VisionBoardSessionByUserIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'userId': instance.userId,
};

VisionBoardSessionByVisionBoardSessionPictureIdSearchCriteria
_$VisionBoardSessionByVisionBoardSessionPictureIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => VisionBoardSessionByVisionBoardSessionPictureIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  visionBoardSessionPictureId: (json['visionBoardSessionPictureId'] as num?)
      ?.toInt(),
);

Map<String, dynamic>
_$VisionBoardSessionByVisionBoardSessionPictureIdSearchCriteriaToJson(
  VisionBoardSessionByVisionBoardSessionPictureIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'visionBoardSessionPictureId': instance.visionBoardSessionPictureId,
};

VisionBoardSessionCreateModel _$VisionBoardSessionCreateModelFromJson(
  Map<String, dynamic> json,
) => VisionBoardSessionCreateModel(
  visionBoardSessionDate: json['visionBoardSessionDate'] == null
      ? null
      : DateTime.parse(json['visionBoardSessionDate'] as String),
  visionBoardSessionTimeSpentInSeconds:
      (json['visionBoardSessionTimeSpentInSeconds'] as num?)?.toInt(),
  userId: (json['userId'] as num?)?.toInt(),
  addedToFeed: json['addedToFeed'] as bool?,
  addedToFeedDate: json['addedToFeedDate'] == null
      ? null
      : DateTime.parse(json['addedToFeedDate'] as String),
);

Map<String, dynamic> _$VisionBoardSessionCreateModelToJson(
  VisionBoardSessionCreateModel instance,
) => <String, dynamic>{
  'visionBoardSessionDate': instance.visionBoardSessionDate?.toIso8601String(),
  'visionBoardSessionTimeSpentInSeconds':
      instance.visionBoardSessionTimeSpentInSeconds,
  'userId': instance.userId,
  'addedToFeed': instance.addedToFeed,
  'addedToFeedDate': instance.addedToFeedDate?.toIso8601String(),
};

VisionBoardSessionModel _$VisionBoardSessionModelFromJson(
  Map<String, dynamic> json,
) => VisionBoardSessionModel(
  visionBoardSessionId: (json['visionBoardSessionId'] as num?)?.toInt(),
  visionBoardSessionDate: json['visionBoardSessionDate'] == null
      ? null
      : DateTime.parse(json['visionBoardSessionDate'] as String),
  visionBoardSessionTimeSpentInSeconds:
      (json['visionBoardSessionTimeSpentInSeconds'] as num?)?.toInt(),
  userId: (json['userId'] as num?)?.toInt(),
  addedToFeed: json['addedToFeed'] as bool?,
  addedToFeedDate: json['addedToFeedDate'] == null
      ? null
      : DateTime.parse(json['addedToFeedDate'] as String),
  isDeleted: json['isDeleted'] as bool?,
  createdDate: json['createdDate'] == null
      ? null
      : DateTime.parse(json['createdDate'] as String),
  createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
  modifiedDate: json['modifiedDate'] == null
      ? null
      : DateTime.parse(json['modifiedDate'] as String),
  modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
  userName: json['userName'] as String?,
  createdByUserFirstName: json['createdByUserFirstName'] as String?,
  createdByUserLastName: json['createdByUserLastName'] as String?,
  createdByUserEmail: json['createdByUserEmail'] as String?,
  modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
  modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
  modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
  createdBy: json['createdBy'] as String?,
  modifiedBy: json['modifiedBy'] as String?,
);

Map<String, dynamic> _$VisionBoardSessionModelToJson(
  VisionBoardSessionModel instance,
) => <String, dynamic>{
  'visionBoardSessionId': instance.visionBoardSessionId,
  'visionBoardSessionDate': instance.visionBoardSessionDate?.toIso8601String(),
  'visionBoardSessionTimeSpentInSeconds':
      instance.visionBoardSessionTimeSpentInSeconds,
  'userId': instance.userId,
  'addedToFeed': instance.addedToFeed,
  'addedToFeedDate': instance.addedToFeedDate?.toIso8601String(),
  'isDeleted': instance.isDeleted,
  'createdDate': instance.createdDate?.toIso8601String(),
  'createdByUserId': instance.createdByUserId,
  'modifiedDate': instance.modifiedDate?.toIso8601String(),
  'modifiedByUserId': instance.modifiedByUserId,
  'userName': instance.userName,
  'createdByUserFirstName': instance.createdByUserFirstName,
  'createdByUserLastName': instance.createdByUserLastName,
  'createdByUserEmail': instance.createdByUserEmail,
  'modifiedByUserFirstName': instance.modifiedByUserFirstName,
  'modifiedByUserLastName': instance.modifiedByUserLastName,
  'modifiedByUserEmail': instance.modifiedByUserEmail,
  'createdBy': instance.createdBy,
  'modifiedBy': instance.modifiedBy,
};

VisionBoardSessionModelReturnListModel
_$VisionBoardSessionModelReturnListModelFromJson(Map<String, dynamic> json) =>
    VisionBoardSessionModelReturnListModel(
      model:
          (json['model'] as List<dynamic>?)
              ?.map(
                (e) =>
                    VisionBoardSessionModel.fromJson(e as Map<String, dynamic>),
              )
              .toList() ??
          [],
      search: json['search'] == null
          ? null
          : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
      totalRecords: (json['totalRecords'] as num?)?.toInt(),
      isSuccess: json['isSuccess'] as bool?,
      successMessage: json['successMessage'] as String?,
      errorHolder: json['errorHolder'] == null
          ? null
          : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
      activityId: json['activityId'] as String?,
      activityDateTime: json['activityDateTime'] == null
          ? null
          : DateTime.parse(json['activityDateTime'] as String),
      server: json['server'] as String?,
      processId: (json['processId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$VisionBoardSessionModelReturnListModelToJson(
  VisionBoardSessionModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

VisionBoardSessionModelReturnModel _$VisionBoardSessionModelReturnModelFromJson(
  Map<String, dynamic> json,
) => VisionBoardSessionModelReturnModel(
  model: json['model'] == null
      ? null
      : VisionBoardSessionModel.fromJson(json['model'] as Map<String, dynamic>),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$VisionBoardSessionModelReturnModelToJson(
  VisionBoardSessionModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

VisionBoardSessionPictureByUserIdSearchCriteria
_$VisionBoardSessionPictureByUserIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => VisionBoardSessionPictureByUserIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  userId: (json['userId'] as num?)?.toInt(),
);

Map<String, dynamic> _$VisionBoardSessionPictureByUserIdSearchCriteriaToJson(
  VisionBoardSessionPictureByUserIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'userId': instance.userId,
};

VisionBoardSessionPictureByVisionBoardPictureIdSearchCriteria
_$VisionBoardSessionPictureByVisionBoardPictureIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => VisionBoardSessionPictureByVisionBoardPictureIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  visionBoardPictureId: (json['visionBoardPictureId'] as num?)?.toInt(),
);

Map<String, dynamic>
_$VisionBoardSessionPictureByVisionBoardPictureIdSearchCriteriaToJson(
  VisionBoardSessionPictureByVisionBoardPictureIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'visionBoardPictureId': instance.visionBoardPictureId,
};

VisionBoardSessionPictureByVisionBoardSessionIdSearchCriteria
_$VisionBoardSessionPictureByVisionBoardSessionIdSearchCriteriaFromJson(
  Map<String, dynamic> json,
) => VisionBoardSessionPictureByVisionBoardSessionIdSearchCriteria(
  selectedId: (json['selectedId'] as num?)?.toInt(),
  page: (json['page'] as num?)?.toInt(),
  pageSize: (json['pageSize'] as num?)?.toInt(),
  keyword: json['keyword'] as String?,
  sortBy: json['sortBy'] as String?,
  sortAscending: json['sortAscending'] as bool?,
  visionBoardSessionId: (json['visionBoardSessionId'] as num?)?.toInt(),
);

Map<String, dynamic>
_$VisionBoardSessionPictureByVisionBoardSessionIdSearchCriteriaToJson(
  VisionBoardSessionPictureByVisionBoardSessionIdSearchCriteria instance,
) => <String, dynamic>{
  'selectedId': instance.selectedId,
  'page': instance.page,
  'pageSize': instance.pageSize,
  'keyword': instance.keyword,
  'sortBy': instance.sortBy,
  'sortAscending': instance.sortAscending,
  'visionBoardSessionId': instance.visionBoardSessionId,
};

VisionBoardSessionPictureCreateModel
_$VisionBoardSessionPictureCreateModelFromJson(
  Map<String, dynamic> json,
) => VisionBoardSessionPictureCreateModel(
  visionBoardSessionId: (json['visionBoardSessionId'] as num?)?.toInt(),
  visionBoardSessionPictureDate: json['visionBoardSessionPictureDate'] == null
      ? null
      : DateTime.parse(json['visionBoardSessionPictureDate'] as String),
  visionBoardPictureId: (json['visionBoardPictureId'] as num?)?.toInt(),
  visionBoardSessionPictureTimeSpentInSeconds:
      (json['visionBoardSessionPictureTimeSpentInSeconds'] as num?)?.toInt(),
  userId: (json['userId'] as num?)?.toInt(),
);

Map<String, dynamic> _$VisionBoardSessionPictureCreateModelToJson(
  VisionBoardSessionPictureCreateModel instance,
) => <String, dynamic>{
  'visionBoardSessionId': instance.visionBoardSessionId,
  'visionBoardSessionPictureDate': instance.visionBoardSessionPictureDate
      ?.toIso8601String(),
  'visionBoardPictureId': instance.visionBoardPictureId,
  'visionBoardSessionPictureTimeSpentInSeconds':
      instance.visionBoardSessionPictureTimeSpentInSeconds,
  'userId': instance.userId,
};

VisionBoardSessionPictureModel _$VisionBoardSessionPictureModelFromJson(
  Map<String, dynamic> json,
) => VisionBoardSessionPictureModel(
  visionBoardSessionPictureId: (json['visionBoardSessionPictureId'] as num?)
      ?.toInt(),
  visionBoardSessionId: (json['visionBoardSessionId'] as num?)?.toInt(),
  visionBoardSessionPictureDate: json['visionBoardSessionPictureDate'] == null
      ? null
      : DateTime.parse(json['visionBoardSessionPictureDate'] as String),
  visionBoardPictureId: (json['visionBoardPictureId'] as num?)?.toInt(),
  visionBoardSessionPictureTimeSpentInSeconds:
      (json['visionBoardSessionPictureTimeSpentInSeconds'] as num?)?.toInt(),
  userId: (json['userId'] as num?)?.toInt(),
  isDeleted: json['isDeleted'] as bool?,
  createdDate: json['createdDate'] == null
      ? null
      : DateTime.parse(json['createdDate'] as String),
  createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
  modifiedDate: json['modifiedDate'] == null
      ? null
      : DateTime.parse(json['modifiedDate'] as String),
  modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
  userName: json['userName'] as String?,
  createdByUserFirstName: json['createdByUserFirstName'] as String?,
  createdByUserLastName: json['createdByUserLastName'] as String?,
  createdByUserEmail: json['createdByUserEmail'] as String?,
  modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
  modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
  modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
  createdBy: json['createdBy'] as String?,
  modifiedBy: json['modifiedBy'] as String?,
);

Map<String, dynamic> _$VisionBoardSessionPictureModelToJson(
  VisionBoardSessionPictureModel instance,
) => <String, dynamic>{
  'visionBoardSessionPictureId': instance.visionBoardSessionPictureId,
  'visionBoardSessionId': instance.visionBoardSessionId,
  'visionBoardSessionPictureDate': instance.visionBoardSessionPictureDate
      ?.toIso8601String(),
  'visionBoardPictureId': instance.visionBoardPictureId,
  'visionBoardSessionPictureTimeSpentInSeconds':
      instance.visionBoardSessionPictureTimeSpentInSeconds,
  'userId': instance.userId,
  'isDeleted': instance.isDeleted,
  'createdDate': instance.createdDate?.toIso8601String(),
  'createdByUserId': instance.createdByUserId,
  'modifiedDate': instance.modifiedDate?.toIso8601String(),
  'modifiedByUserId': instance.modifiedByUserId,
  'userName': instance.userName,
  'createdByUserFirstName': instance.createdByUserFirstName,
  'createdByUserLastName': instance.createdByUserLastName,
  'createdByUserEmail': instance.createdByUserEmail,
  'modifiedByUserFirstName': instance.modifiedByUserFirstName,
  'modifiedByUserLastName': instance.modifiedByUserLastName,
  'modifiedByUserEmail': instance.modifiedByUserEmail,
  'createdBy': instance.createdBy,
  'modifiedBy': instance.modifiedBy,
};

VisionBoardSessionPictureModelReturnListModel
_$VisionBoardSessionPictureModelReturnListModelFromJson(
  Map<String, dynamic> json,
) => VisionBoardSessionPictureModelReturnListModel(
  model:
      (json['model'] as List<dynamic>?)
          ?.map(
            (e) => VisionBoardSessionPictureModel.fromJson(
              e as Map<String, dynamic>,
            ),
          )
          .toList() ??
      [],
  search: json['search'] == null
      ? null
      : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
  totalRecords: (json['totalRecords'] as num?)?.toInt(),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$VisionBoardSessionPictureModelReturnListModelToJson(
  VisionBoardSessionPictureModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

VisionBoardSessionPictureModelReturnModel
_$VisionBoardSessionPictureModelReturnModelFromJson(
  Map<String, dynamic> json,
) => VisionBoardSessionPictureModelReturnModel(
  model: json['model'] == null
      ? null
      : VisionBoardSessionPictureModel.fromJson(
          json['model'] as Map<String, dynamic>,
        ),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$VisionBoardSessionPictureModelReturnModelToJson(
  VisionBoardSessionPictureModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

VisionBoardSessionPictureUpdateModel
_$VisionBoardSessionPictureUpdateModelFromJson(
  Map<String, dynamic> json,
) => VisionBoardSessionPictureUpdateModel(
  visionBoardSessionPictureId: (json['visionBoardSessionPictureId'] as num?)
      ?.toInt(),
  visionBoardSessionId: (json['visionBoardSessionId'] as num?)?.toInt(),
  visionBoardSessionPictureDate: json['visionBoardSessionPictureDate'] == null
      ? null
      : DateTime.parse(json['visionBoardSessionPictureDate'] as String),
  visionBoardPictureId: (json['visionBoardPictureId'] as num?)?.toInt(),
  visionBoardSessionPictureTimeSpentInSeconds:
      (json['visionBoardSessionPictureTimeSpentInSeconds'] as num?)?.toInt(),
  userId: (json['userId'] as num?)?.toInt(),
);

Map<String, dynamic> _$VisionBoardSessionPictureUpdateModelToJson(
  VisionBoardSessionPictureUpdateModel instance,
) => <String, dynamic>{
  'visionBoardSessionPictureId': instance.visionBoardSessionPictureId,
  'visionBoardSessionId': instance.visionBoardSessionId,
  'visionBoardSessionPictureDate': instance.visionBoardSessionPictureDate
      ?.toIso8601String(),
  'visionBoardPictureId': instance.visionBoardPictureId,
  'visionBoardSessionPictureTimeSpentInSeconds':
      instance.visionBoardSessionPictureTimeSpentInSeconds,
  'userId': instance.userId,
};

VisionBoardSessionUpdateModel _$VisionBoardSessionUpdateModelFromJson(
  Map<String, dynamic> json,
) => VisionBoardSessionUpdateModel(
  visionBoardSessionId: (json['visionBoardSessionId'] as num?)?.toInt(),
  visionBoardSessionDate: json['visionBoardSessionDate'] == null
      ? null
      : DateTime.parse(json['visionBoardSessionDate'] as String),
  visionBoardSessionTimeSpentInSeconds:
      (json['visionBoardSessionTimeSpentInSeconds'] as num?)?.toInt(),
  userId: (json['userId'] as num?)?.toInt(),
  addedToFeed: json['addedToFeed'] as bool?,
  addedToFeedDate: json['addedToFeedDate'] == null
      ? null
      : DateTime.parse(json['addedToFeedDate'] as String),
);

Map<String, dynamic> _$VisionBoardSessionUpdateModelToJson(
  VisionBoardSessionUpdateModel instance,
) => <String, dynamic>{
  'visionBoardSessionId': instance.visionBoardSessionId,
  'visionBoardSessionDate': instance.visionBoardSessionDate?.toIso8601String(),
  'visionBoardSessionTimeSpentInSeconds':
      instance.visionBoardSessionTimeSpentInSeconds,
  'userId': instance.userId,
  'addedToFeed': instance.addedToFeed,
  'addedToFeedDate': instance.addedToFeedDate?.toIso8601String(),
};

VoiceNotificationServerCreateModel _$VoiceNotificationServerCreateModelFromJson(
  Map<String, dynamic> json,
) => VoiceNotificationServerCreateModel(
  voiceNotificationServerCode: json['voiceNotificationServerCode'] as String?,
  voiceNotificationServerName: json['voiceNotificationServerName'] as String?,
  predefinedSmsAndVoiceServerTypeId:
      (json['predefinedSmsAndVoiceServerTypeId'] as num?)?.toInt(),
  twilioAccountSid: json['twilioAccountSid'] as String?,
  twilioAccountAuthToken: json['twilioAccountAuthToken'] as String?,
  twilioAccountPhoneNumber: json['twilioAccountPhoneNumber'] as String?,
  isEnabled: json['isEnabled'] as bool?,
);

Map<String, dynamic> _$VoiceNotificationServerCreateModelToJson(
  VoiceNotificationServerCreateModel instance,
) => <String, dynamic>{
  'voiceNotificationServerCode': instance.voiceNotificationServerCode,
  'voiceNotificationServerName': instance.voiceNotificationServerName,
  'predefinedSmsAndVoiceServerTypeId':
      instance.predefinedSmsAndVoiceServerTypeId,
  'twilioAccountSid': instance.twilioAccountSid,
  'twilioAccountAuthToken': instance.twilioAccountAuthToken,
  'twilioAccountPhoneNumber': instance.twilioAccountPhoneNumber,
  'isEnabled': instance.isEnabled,
};

VoiceNotificationServerModel _$VoiceNotificationServerModelFromJson(
  Map<String, dynamic> json,
) => VoiceNotificationServerModel(
  voiceNotificationServerId: (json['voiceNotificationServerId'] as num?)
      ?.toInt(),
  voiceNotificationServerCode: json['voiceNotificationServerCode'] as String?,
  voiceNotificationServerName: json['voiceNotificationServerName'] as String?,
  predefinedSmsAndVoiceServerTypeId:
      (json['predefinedSmsAndVoiceServerTypeId'] as num?)?.toInt(),
  twilioAccountSid: json['twilioAccountSid'] as String?,
  twilioAccountAuthToken: json['twilioAccountAuthToken'] as String?,
  twilioAccountPhoneNumber: json['twilioAccountPhoneNumber'] as String?,
  isEnabled: json['isEnabled'] as bool?,
  isDeleted: json['isDeleted'] as bool?,
  createdDate: json['createdDate'] == null
      ? null
      : DateTime.parse(json['createdDate'] as String),
  createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
  modifiedDate: json['modifiedDate'] == null
      ? null
      : DateTime.parse(json['modifiedDate'] as String),
  modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
  enumTypeName: json['enumTypeName'] as String?,
  createdByUserFirstName: json['createdByUserFirstName'] as String?,
  createdByUserLastName: json['createdByUserLastName'] as String?,
  createdByUserEmail: json['createdByUserEmail'] as String?,
  modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
  modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
  modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
  createdBy: json['createdBy'] as String?,
  modifiedBy: json['modifiedBy'] as String?,
);

Map<String, dynamic> _$VoiceNotificationServerModelToJson(
  VoiceNotificationServerModel instance,
) => <String, dynamic>{
  'voiceNotificationServerId': instance.voiceNotificationServerId,
  'voiceNotificationServerCode': instance.voiceNotificationServerCode,
  'voiceNotificationServerName': instance.voiceNotificationServerName,
  'predefinedSmsAndVoiceServerTypeId':
      instance.predefinedSmsAndVoiceServerTypeId,
  'twilioAccountSid': instance.twilioAccountSid,
  'twilioAccountAuthToken': instance.twilioAccountAuthToken,
  'twilioAccountPhoneNumber': instance.twilioAccountPhoneNumber,
  'isEnabled': instance.isEnabled,
  'isDeleted': instance.isDeleted,
  'createdDate': instance.createdDate?.toIso8601String(),
  'createdByUserId': instance.createdByUserId,
  'modifiedDate': instance.modifiedDate?.toIso8601String(),
  'modifiedByUserId': instance.modifiedByUserId,
  'enumTypeName': instance.enumTypeName,
  'createdByUserFirstName': instance.createdByUserFirstName,
  'createdByUserLastName': instance.createdByUserLastName,
  'createdByUserEmail': instance.createdByUserEmail,
  'modifiedByUserFirstName': instance.modifiedByUserFirstName,
  'modifiedByUserLastName': instance.modifiedByUserLastName,
  'modifiedByUserEmail': instance.modifiedByUserEmail,
  'createdBy': instance.createdBy,
  'modifiedBy': instance.modifiedBy,
};

VoiceNotificationServerModelReturnListModel
_$VoiceNotificationServerModelReturnListModelFromJson(
  Map<String, dynamic> json,
) => VoiceNotificationServerModelReturnListModel(
  model:
      (json['model'] as List<dynamic>?)
          ?.map(
            (e) => VoiceNotificationServerModel.fromJson(
              e as Map<String, dynamic>,
            ),
          )
          .toList() ??
      [],
  search: json['search'] == null
      ? null
      : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
  totalRecords: (json['totalRecords'] as num?)?.toInt(),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$VoiceNotificationServerModelReturnListModelToJson(
  VoiceNotificationServerModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

VoiceNotificationServerModelReturnModel
_$VoiceNotificationServerModelReturnModelFromJson(Map<String, dynamic> json) =>
    VoiceNotificationServerModelReturnModel(
      model: json['model'] == null
          ? null
          : VoiceNotificationServerModel.fromJson(
              json['model'] as Map<String, dynamic>,
            ),
      isSuccess: json['isSuccess'] as bool?,
      successMessage: json['successMessage'] as String?,
      errorHolder: json['errorHolder'] == null
          ? null
          : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
      activityId: json['activityId'] as String?,
      activityDateTime: json['activityDateTime'] == null
          ? null
          : DateTime.parse(json['activityDateTime'] as String),
      server: json['server'] as String?,
      processId: (json['processId'] as num?)?.toInt(),
    );

Map<String, dynamic> _$VoiceNotificationServerModelReturnModelToJson(
  VoiceNotificationServerModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

VoiceNotificationServerUpdateModel _$VoiceNotificationServerUpdateModelFromJson(
  Map<String, dynamic> json,
) => VoiceNotificationServerUpdateModel(
  voiceNotificationServerId: (json['voiceNotificationServerId'] as num?)
      ?.toInt(),
  voiceNotificationServerCode: json['voiceNotificationServerCode'] as String?,
  voiceNotificationServerName: json['voiceNotificationServerName'] as String?,
  predefinedSmsAndVoiceServerTypeId:
      (json['predefinedSmsAndVoiceServerTypeId'] as num?)?.toInt(),
  twilioAccountSid: json['twilioAccountSid'] as String?,
  twilioAccountAuthToken: json['twilioAccountAuthToken'] as String?,
  twilioAccountPhoneNumber: json['twilioAccountPhoneNumber'] as String?,
  isEnabled: json['isEnabled'] as bool?,
);

Map<String, dynamic> _$VoiceNotificationServerUpdateModelToJson(
  VoiceNotificationServerUpdateModel instance,
) => <String, dynamic>{
  'voiceNotificationServerId': instance.voiceNotificationServerId,
  'voiceNotificationServerCode': instance.voiceNotificationServerCode,
  'voiceNotificationServerName': instance.voiceNotificationServerName,
  'predefinedSmsAndVoiceServerTypeId':
      instance.predefinedSmsAndVoiceServerTypeId,
  'twilioAccountSid': instance.twilioAccountSid,
  'twilioAccountAuthToken': instance.twilioAccountAuthToken,
  'twilioAccountPhoneNumber': instance.twilioAccountPhoneNumber,
  'isEnabled': instance.isEnabled,
};

WhatsAppNotificationServerCreateModel
_$WhatsAppNotificationServerCreateModelFromJson(Map<String, dynamic> json) =>
    WhatsAppNotificationServerCreateModel(
      whatsAppNotificationServerCode:
          json['whatsAppNotificationServerCode'] as String?,
      whatsAppNotificationServerName:
          json['whatsAppNotificationServerName'] as String?,
      predefinedSmsAndVoiceServerTypeId:
          (json['predefinedSmsAndVoiceServerTypeId'] as num?)?.toInt(),
      apiKey: json['apiKey'] as String?,
      apiSecret: json['apiSecret'] as String?,
      accessToken: json['accessToken'] as String?,
      versionNumber: json['versionNumber'] as String?,
      phoneNumberId: json['phoneNumberId'] as String?,
      businessAccountId: json['businessAccountId'] as String?,
      senderPhoneNumber: json['senderPhoneNumber'] as String?,
      isEnabled: json['isEnabled'] as bool?,
    );

Map<String, dynamic> _$WhatsAppNotificationServerCreateModelToJson(
  WhatsAppNotificationServerCreateModel instance,
) => <String, dynamic>{
  'whatsAppNotificationServerCode': instance.whatsAppNotificationServerCode,
  'whatsAppNotificationServerName': instance.whatsAppNotificationServerName,
  'predefinedSmsAndVoiceServerTypeId':
      instance.predefinedSmsAndVoiceServerTypeId,
  'apiKey': instance.apiKey,
  'apiSecret': instance.apiSecret,
  'accessToken': instance.accessToken,
  'versionNumber': instance.versionNumber,
  'phoneNumberId': instance.phoneNumberId,
  'businessAccountId': instance.businessAccountId,
  'senderPhoneNumber': instance.senderPhoneNumber,
  'isEnabled': instance.isEnabled,
};

WhatsAppNotificationServerModel _$WhatsAppNotificationServerModelFromJson(
  Map<String, dynamic> json,
) => WhatsAppNotificationServerModel(
  whatsAppNotificationServerId: (json['whatsAppNotificationServerId'] as num?)
      ?.toInt(),
  whatsAppNotificationServerCode:
      json['whatsAppNotificationServerCode'] as String?,
  whatsAppNotificationServerName:
      json['whatsAppNotificationServerName'] as String?,
  predefinedSmsAndVoiceServerTypeId:
      (json['predefinedSmsAndVoiceServerTypeId'] as num?)?.toInt(),
  apiKey: json['apiKey'] as String?,
  apiSecret: json['apiSecret'] as String?,
  accessToken: json['accessToken'] as String?,
  versionNumber: json['versionNumber'] as String?,
  phoneNumberId: json['phoneNumberId'] as String?,
  businessAccountId: json['businessAccountId'] as String?,
  senderPhoneNumber: json['senderPhoneNumber'] as String?,
  isEnabled: json['isEnabled'] as bool?,
  isDeleted: json['isDeleted'] as bool?,
  createdDate: json['createdDate'] == null
      ? null
      : DateTime.parse(json['createdDate'] as String),
  createdByUserId: (json['createdByUserId'] as num?)?.toInt(),
  modifiedDate: json['modifiedDate'] == null
      ? null
      : DateTime.parse(json['modifiedDate'] as String),
  modifiedByUserId: (json['modifiedByUserId'] as num?)?.toInt(),
  enumTypeName: json['enumTypeName'] as String?,
  phoneNumberName: json['phoneNumberName'] as String?,
  businessAccountName: json['businessAccountName'] as String?,
  companyId: (json['companyId'] as num?)?.toInt(),
  createdByUserFirstName: json['createdByUserFirstName'] as String?,
  createdByUserLastName: json['createdByUserLastName'] as String?,
  createdByUserEmail: json['createdByUserEmail'] as String?,
  modifiedByUserFirstName: json['modifiedByUserFirstName'] as String?,
  modifiedByUserLastName: json['modifiedByUserLastName'] as String?,
  modifiedByUserEmail: json['modifiedByUserEmail'] as String?,
  createdBy: json['createdBy'] as String?,
  modifiedBy: json['modifiedBy'] as String?,
);

Map<String, dynamic> _$WhatsAppNotificationServerModelToJson(
  WhatsAppNotificationServerModel instance,
) => <String, dynamic>{
  'whatsAppNotificationServerId': instance.whatsAppNotificationServerId,
  'whatsAppNotificationServerCode': instance.whatsAppNotificationServerCode,
  'whatsAppNotificationServerName': instance.whatsAppNotificationServerName,
  'predefinedSmsAndVoiceServerTypeId':
      instance.predefinedSmsAndVoiceServerTypeId,
  'apiKey': instance.apiKey,
  'apiSecret': instance.apiSecret,
  'accessToken': instance.accessToken,
  'versionNumber': instance.versionNumber,
  'phoneNumberId': instance.phoneNumberId,
  'businessAccountId': instance.businessAccountId,
  'senderPhoneNumber': instance.senderPhoneNumber,
  'isEnabled': instance.isEnabled,
  'isDeleted': instance.isDeleted,
  'createdDate': instance.createdDate?.toIso8601String(),
  'createdByUserId': instance.createdByUserId,
  'modifiedDate': instance.modifiedDate?.toIso8601String(),
  'modifiedByUserId': instance.modifiedByUserId,
  'enumTypeName': instance.enumTypeName,
  'phoneNumberName': instance.phoneNumberName,
  'businessAccountName': instance.businessAccountName,
  'companyId': instance.companyId,
  'createdByUserFirstName': instance.createdByUserFirstName,
  'createdByUserLastName': instance.createdByUserLastName,
  'createdByUserEmail': instance.createdByUserEmail,
  'modifiedByUserFirstName': instance.modifiedByUserFirstName,
  'modifiedByUserLastName': instance.modifiedByUserLastName,
  'modifiedByUserEmail': instance.modifiedByUserEmail,
  'createdBy': instance.createdBy,
  'modifiedBy': instance.modifiedBy,
};

WhatsAppNotificationServerModelReturnListModel
_$WhatsAppNotificationServerModelReturnListModelFromJson(
  Map<String, dynamic> json,
) => WhatsAppNotificationServerModelReturnListModel(
  model:
      (json['model'] as List<dynamic>?)
          ?.map(
            (e) => WhatsAppNotificationServerModel.fromJson(
              e as Map<String, dynamic>,
            ),
          )
          .toList() ??
      [],
  search: json['search'] == null
      ? null
      : BaseSearchCriteria.fromJson(json['search'] as Map<String, dynamic>),
  totalRecords: (json['totalRecords'] as num?)?.toInt(),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$WhatsAppNotificationServerModelReturnListModelToJson(
  WhatsAppNotificationServerModelReturnListModel instance,
) => <String, dynamic>{
  'model': instance.model?.map((e) => e.toJson()).toList(),
  'search': instance.search?.toJson(),
  'totalRecords': instance.totalRecords,
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

WhatsAppNotificationServerModelReturnModel
_$WhatsAppNotificationServerModelReturnModelFromJson(
  Map<String, dynamic> json,
) => WhatsAppNotificationServerModelReturnModel(
  model: json['model'] == null
      ? null
      : WhatsAppNotificationServerModel.fromJson(
          json['model'] as Map<String, dynamic>,
        ),
  isSuccess: json['isSuccess'] as bool?,
  successMessage: json['successMessage'] as String?,
  errorHolder: json['errorHolder'] == null
      ? null
      : ReturnError.fromJson(json['errorHolder'] as Map<String, dynamic>),
  activityId: json['activityId'] as String?,
  activityDateTime: json['activityDateTime'] == null
      ? null
      : DateTime.parse(json['activityDateTime'] as String),
  server: json['server'] as String?,
  processId: (json['processId'] as num?)?.toInt(),
);

Map<String, dynamic> _$WhatsAppNotificationServerModelReturnModelToJson(
  WhatsAppNotificationServerModelReturnModel instance,
) => <String, dynamic>{
  'model': instance.model?.toJson(),
  'isSuccess': instance.isSuccess,
  'successMessage': instance.successMessage,
  'errorHolder': instance.errorHolder?.toJson(),
  'activityId': instance.activityId,
  'activityDateTime': instance.activityDateTime?.toIso8601String(),
  'server': instance.server,
  'processId': instance.processId,
};

WhatsAppNotificationServerUpdateModel
_$WhatsAppNotificationServerUpdateModelFromJson(Map<String, dynamic> json) =>
    WhatsAppNotificationServerUpdateModel(
      whatsAppNotificationServerId:
          (json['whatsAppNotificationServerId'] as num?)?.toInt(),
      whatsAppNotificationServerCode:
          json['whatsAppNotificationServerCode'] as String?,
      whatsAppNotificationServerName:
          json['whatsAppNotificationServerName'] as String?,
      predefinedSmsAndVoiceServerTypeId:
          (json['predefinedSmsAndVoiceServerTypeId'] as num?)?.toInt(),
      apiKey: json['apiKey'] as String?,
      apiSecret: json['apiSecret'] as String?,
      accessToken: json['accessToken'] as String?,
      versionNumber: json['versionNumber'] as String?,
      phoneNumberId: json['phoneNumberId'] as String?,
      businessAccountId: json['businessAccountId'] as String?,
      senderPhoneNumber: json['senderPhoneNumber'] as String?,
      isEnabled: json['isEnabled'] as bool?,
    );

Map<String, dynamic> _$WhatsAppNotificationServerUpdateModelToJson(
  WhatsAppNotificationServerUpdateModel instance,
) => <String, dynamic>{
  'whatsAppNotificationServerId': instance.whatsAppNotificationServerId,
  'whatsAppNotificationServerCode': instance.whatsAppNotificationServerCode,
  'whatsAppNotificationServerName': instance.whatsAppNotificationServerName,
  'predefinedSmsAndVoiceServerTypeId':
      instance.predefinedSmsAndVoiceServerTypeId,
  'apiKey': instance.apiKey,
  'apiSecret': instance.apiSecret,
  'accessToken': instance.accessToken,
  'versionNumber': instance.versionNumber,
  'phoneNumberId': instance.phoneNumberId,
  'businessAccountId': instance.businessAccountId,
  'senderPhoneNumber': instance.senderPhoneNumber,
  'isEnabled': instance.isEnabled,
};

ApiV1CompanyUploaduserpicturePost$RequestBody
_$ApiV1CompanyUploaduserpicturePost$RequestBodyFromJson(
  Map<String, dynamic> json,
) => ApiV1CompanyUploaduserpicturePost$RequestBody(
  userId: (json['UserId'] as num?)?.toInt(),
  uploadedFile: json['UploadedFile'] as String?,
);

Map<String, dynamic> _$ApiV1CompanyUploaduserpicturePost$RequestBodyToJson(
  ApiV1CompanyUploaduserpicturePost$RequestBody instance,
) => <String, dynamic>{
  'UserId': instance.userId,
  'UploadedFile': instance.uploadedFile,
};

ApiV1CompanyCreateuserPost$RequestBody
_$ApiV1CompanyCreateuserPost$RequestBodyFromJson(Map<String, dynamic> json) =>
    ApiV1CompanyCreateuserPost$RequestBody(
      uploadedFile: json['UploadedFile'] as String?,
      captchaCode: json['CaptchaCode'] as String?,
      newPermissionTypeIds:
          (json['NewPermissionTypeIds'] as List<dynamic>?)
              ?.map((e) => (e as num).toInt())
              .toList() ??
          [],
      initialPassword: json['InitialPassword'] as String?,
      modifiedByUserFirstName: json['ModifiedByUserFirstName'] as String?,
      modifiedByUserLastName: json['ModifiedByUserLastName'] as String?,
      modifiedByUserEmail: json['ModifiedByUserEmail'] as String?,
      userGuid: json['UserGuid'] as String?,
      userName: json['UserName'] as String?,
      predefinedUserTypeId: (json['PredefinedUserTypeId'] as num?)?.toInt(),
      passwordHash: json['PasswordHash'] as String?,
      passwordChangedDate: json['PasswordChangedDate'] == null
          ? null
          : DateTime.parse(json['PasswordChangedDate'] as String),
      email: json['Email'] as String?,
      firstName: json['FirstName'] as String?,
      lastName: json['LastName'] as String?,
      workPhone: json['WorkPhone'] as String?,
      workPhoneExt: json['WorkPhoneExt'] as String?,
      cellPhone: json['CellPhone'] as String?,
      active: json['Active'] as bool?,
      notificationThroughEmail: json['NotificationThroughEmail'] as bool?,
      oneTimeToken: json['OneTimeToken'] as String?,
      oneTimeExpireDateTime: json['OneTimeExpireDateTime'] == null
          ? null
          : DateTime.parse(json['OneTimeExpireDateTime'] as String),
      loginFailedAttempt: (json['LoginFailedAttempt'] as num?)?.toInt(),
      isAccountLocked: json['IsAccountLocked'] as bool?,
      accountLockedUntil: json['AccountLockedUntil'] == null
          ? null
          : DateTime.parse(json['AccountLockedUntil'] as String),
      newUserActivationGuid: json['NewUserActivationGuid'] as String?,
      newUserActivationCompletedOn: json['NewUserActivationCompletedOn'] == null
          ? null
          : DateTime.parse(json['NewUserActivationCompletedOn'] as String),
      profileImageGuid: json['ProfileImageGuid'] as String?,
      profileImageLocation: json['ProfileImageLocation'] as String?,
      profileImageActualFileName: json['ProfileImageActualFileName'] as String?,
      hasUserPurchasedPremiumFeature:
          json['HasUserPurchasedPremiumFeature'] as bool?,
      premiumFeatureValidUntil: json['PremiumFeatureValidUntil'] == null
          ? null
          : DateTime.parse(json['PremiumFeatureValidUntil'] as String),
      isDeleted: json['IsDeleted'] as bool?,
    );

Map<String, dynamic> _$ApiV1CompanyCreateuserPost$RequestBodyToJson(
  ApiV1CompanyCreateuserPost$RequestBody instance,
) => <String, dynamic>{
  'UploadedFile': instance.uploadedFile,
  'CaptchaCode': instance.captchaCode,
  'NewPermissionTypeIds': instance.newPermissionTypeIds,
  'InitialPassword': instance.initialPassword,
  'ModifiedByUserFirstName': instance.modifiedByUserFirstName,
  'ModifiedByUserLastName': instance.modifiedByUserLastName,
  'ModifiedByUserEmail': instance.modifiedByUserEmail,
  'UserGuid': instance.userGuid,
  'UserName': instance.userName,
  'PredefinedUserTypeId': instance.predefinedUserTypeId,
  'PasswordHash': instance.passwordHash,
  'PasswordChangedDate': instance.passwordChangedDate?.toIso8601String(),
  'Email': instance.email,
  'FirstName': instance.firstName,
  'LastName': instance.lastName,
  'WorkPhone': instance.workPhone,
  'WorkPhoneExt': instance.workPhoneExt,
  'CellPhone': instance.cellPhone,
  'Active': instance.active,
  'NotificationThroughEmail': instance.notificationThroughEmail,
  'OneTimeToken': instance.oneTimeToken,
  'OneTimeExpireDateTime': instance.oneTimeExpireDateTime?.toIso8601String(),
  'LoginFailedAttempt': instance.loginFailedAttempt,
  'IsAccountLocked': instance.isAccountLocked,
  'AccountLockedUntil': instance.accountLockedUntil?.toIso8601String(),
  'NewUserActivationGuid': instance.newUserActivationGuid,
  'NewUserActivationCompletedOn': instance.newUserActivationCompletedOn
      ?.toIso8601String(),
  'ProfileImageGuid': instance.profileImageGuid,
  'ProfileImageLocation': instance.profileImageLocation,
  'ProfileImageActualFileName': instance.profileImageActualFileName,
  'HasUserPurchasedPremiumFeature': instance.hasUserPurchasedPremiumFeature,
  'PremiumFeatureValidUntil': instance.premiumFeatureValidUntil
      ?.toIso8601String(),
  'IsDeleted': instance.isDeleted,
};

ApiV1CompanyUpdateuserbyadminPost$RequestBody
_$ApiV1CompanyUpdateuserbyadminPost$RequestBodyFromJson(
  Map<String, dynamic> json,
) => ApiV1CompanyUpdateuserbyadminPost$RequestBody(
  uploadedFile: json['UploadedFile'] as String?,
  existingPermissionTypeIds:
      (json['ExistingPermissionTypeIds'] as List<dynamic>?)
          ?.map((e) => (e as num).toInt())
          .toList() ??
      [],
  modifiedByUserFirstName: json['ModifiedByUserFirstName'] as String?,
  modifiedByUserLastName: json['ModifiedByUserLastName'] as String?,
  modifiedByUserEmail: json['ModifiedByUserEmail'] as String?,
  userId: (json['UserId'] as num?)?.toInt(),
  userGuid: json['UserGuid'] as String?,
  userName: json['UserName'] as String?,
  predefinedUserTypeId: (json['PredefinedUserTypeId'] as num?)?.toInt(),
  passwordHash: json['PasswordHash'] as String?,
  passwordChangedDate: json['PasswordChangedDate'] == null
      ? null
      : DateTime.parse(json['PasswordChangedDate'] as String),
  email: json['Email'] as String?,
  firstName: json['FirstName'] as String?,
  lastName: json['LastName'] as String?,
  workPhone: json['WorkPhone'] as String?,
  workPhoneExt: json['WorkPhoneExt'] as String?,
  cellPhone: json['CellPhone'] as String?,
  active: json['Active'] as bool?,
  notificationThroughEmail: json['NotificationThroughEmail'] as bool?,
  oneTimeToken: json['OneTimeToken'] as String?,
  oneTimeExpireDateTime: json['OneTimeExpireDateTime'] == null
      ? null
      : DateTime.parse(json['OneTimeExpireDateTime'] as String),
  loginFailedAttempt: (json['LoginFailedAttempt'] as num?)?.toInt(),
  isAccountLocked: json['IsAccountLocked'] as bool?,
  accountLockedUntil: json['AccountLockedUntil'] == null
      ? null
      : DateTime.parse(json['AccountLockedUntil'] as String),
  newUserActivationGuid: json['NewUserActivationGuid'] as String?,
  newUserActivationCompletedOn: json['NewUserActivationCompletedOn'] == null
      ? null
      : DateTime.parse(json['NewUserActivationCompletedOn'] as String),
  profileImageGuid: json['ProfileImageGuid'] as String?,
  profileImageLocation: json['ProfileImageLocation'] as String?,
  profileImageActualFileName: json['ProfileImageActualFileName'] as String?,
  hasUserPurchasedPremiumFeature:
      json['HasUserPurchasedPremiumFeature'] as bool?,
  premiumFeatureValidUntil: json['PremiumFeatureValidUntil'] == null
      ? null
      : DateTime.parse(json['PremiumFeatureValidUntil'] as String),
  isDeleted: json['IsDeleted'] as bool?,
);

Map<String, dynamic> _$ApiV1CompanyUpdateuserbyadminPost$RequestBodyToJson(
  ApiV1CompanyUpdateuserbyadminPost$RequestBody instance,
) => <String, dynamic>{
  'UploadedFile': instance.uploadedFile,
  'ExistingPermissionTypeIds': instance.existingPermissionTypeIds,
  'ModifiedByUserFirstName': instance.modifiedByUserFirstName,
  'ModifiedByUserLastName': instance.modifiedByUserLastName,
  'ModifiedByUserEmail': instance.modifiedByUserEmail,
  'UserId': instance.userId,
  'UserGuid': instance.userGuid,
  'UserName': instance.userName,
  'PredefinedUserTypeId': instance.predefinedUserTypeId,
  'PasswordHash': instance.passwordHash,
  'PasswordChangedDate': instance.passwordChangedDate?.toIso8601String(),
  'Email': instance.email,
  'FirstName': instance.firstName,
  'LastName': instance.lastName,
  'WorkPhone': instance.workPhone,
  'WorkPhoneExt': instance.workPhoneExt,
  'CellPhone': instance.cellPhone,
  'Active': instance.active,
  'NotificationThroughEmail': instance.notificationThroughEmail,
  'OneTimeToken': instance.oneTimeToken,
  'OneTimeExpireDateTime': instance.oneTimeExpireDateTime?.toIso8601String(),
  'LoginFailedAttempt': instance.loginFailedAttempt,
  'IsAccountLocked': instance.isAccountLocked,
  'AccountLockedUntil': instance.accountLockedUntil?.toIso8601String(),
  'NewUserActivationGuid': instance.newUserActivationGuid,
  'NewUserActivationCompletedOn': instance.newUserActivationCompletedOn
      ?.toIso8601String(),
  'ProfileImageGuid': instance.profileImageGuid,
  'ProfileImageLocation': instance.profileImageLocation,
  'ProfileImageActualFileName': instance.profileImageActualFileName,
  'HasUserPurchasedPremiumFeature': instance.hasUserPurchasedPremiumFeature,
  'PremiumFeatureValidUntil': instance.premiumFeatureValidUntil
      ?.toIso8601String(),
  'IsDeleted': instance.isDeleted,
};

ApiV1ManifestCreatefileattachmentPost$RequestBody
_$ApiV1ManifestCreatefileattachmentPost$RequestBodyFromJson(
  Map<String, dynamic> json,
) => ApiV1ManifestCreatefileattachmentPost$RequestBody(
  uploadedFile: json['UploadedFile'] as String?,
  fileAttachmentFolderId: (json['FileAttachmentFolderId'] as num?)?.toInt(),
  fileAttachmentFileName: json['FileAttachmentFileName'] as String?,
  fileAttachmentFileLocation: json['FileAttachmentFileLocation'] as String?,
  fileAttachmentPreviewFileLocation:
      json['FileAttachmentPreviewFileLocation'] as String?,
  fileAttachmentHasPreview: json['FileAttachmentHasPreview'] as bool?,
  fileAttachmentFileSizeInBytes: (json['FileAttachmentFileSizeInBytes'] as num?)
      ?.toInt(),
  description: json['Description'] as String?,
  userId: (json['UserId'] as num?)?.toInt(),
);

Map<String, dynamic> _$ApiV1ManifestCreatefileattachmentPost$RequestBodyToJson(
  ApiV1ManifestCreatefileattachmentPost$RequestBody instance,
) => <String, dynamic>{
  'UploadedFile': instance.uploadedFile,
  'FileAttachmentFolderId': instance.fileAttachmentFolderId,
  'FileAttachmentFileName': instance.fileAttachmentFileName,
  'FileAttachmentFileLocation': instance.fileAttachmentFileLocation,
  'FileAttachmentPreviewFileLocation':
      instance.fileAttachmentPreviewFileLocation,
  'FileAttachmentHasPreview': instance.fileAttachmentHasPreview,
  'FileAttachmentFileSizeInBytes': instance.fileAttachmentFileSizeInBytes,
  'Description': instance.description,
  'UserId': instance.userId,
};

ApiV1ManifestCreatevisionboardpicturePost$RequestBody
_$ApiV1ManifestCreatevisionboardpicturePost$RequestBodyFromJson(
  Map<String, dynamic> json,
) => ApiV1ManifestCreatevisionboardpicturePost$RequestBody(
  uploadedFile: json['UploadedFile'] as String,
  title: json['Title'] as String?,
  location: json['Location'] as String?,
  fileName: json['FileName'] as String?,
  fileSize: (json['FileSize'] as num?)?.toInt(),
  thumbnailLocation: json['ThumbnailLocation'] as String?,
  thumbnailFileName: json['ThumbnailFileName'] as String?,
  thumbnailFileSize: (json['ThumbnailFileSize'] as num?)?.toInt(),
  userId: (json['UserId'] as num?)?.toInt(),
);

Map<String, dynamic>
_$ApiV1ManifestCreatevisionboardpicturePost$RequestBodyToJson(
  ApiV1ManifestCreatevisionboardpicturePost$RequestBody instance,
) => <String, dynamic>{
  'UploadedFile': instance.uploadedFile,
  'Title': instance.title,
  'Location': instance.location,
  'FileName': instance.fileName,
  'FileSize': instance.fileSize,
  'ThumbnailLocation': instance.thumbnailLocation,
  'ThumbnailFileName': instance.thumbnailFileName,
  'ThumbnailFileSize': instance.thumbnailFileSize,
  'UserId': instance.userId,
};

ApiV1ManifestUpdatevisionboardpicturePost$RequestBody
_$ApiV1ManifestUpdatevisionboardpicturePost$RequestBodyFromJson(
  Map<String, dynamic> json,
) => ApiV1ManifestUpdatevisionboardpicturePost$RequestBody(
  uploadedFile: json['UploadedFile'] as String,
  visionBoardPictureId: (json['VisionBoardPictureId'] as num?)?.toInt(),
  title: json['Title'] as String?,
  location: json['Location'] as String?,
  fileName: json['FileName'] as String?,
  fileSize: (json['FileSize'] as num?)?.toInt(),
  thumbnailLocation: json['ThumbnailLocation'] as String?,
  thumbnailFileName: json['ThumbnailFileName'] as String?,
  thumbnailFileSize: (json['ThumbnailFileSize'] as num?)?.toInt(),
  userId: (json['UserId'] as num?)?.toInt(),
);

Map<String, dynamic>
_$ApiV1ManifestUpdatevisionboardpicturePost$RequestBodyToJson(
  ApiV1ManifestUpdatevisionboardpicturePost$RequestBody instance,
) => <String, dynamic>{
  'UploadedFile': instance.uploadedFile,
  'VisionBoardPictureId': instance.visionBoardPictureId,
  'Title': instance.title,
  'Location': instance.location,
  'FileName': instance.fileName,
  'FileSize': instance.fileSize,
  'ThumbnailLocation': instance.thumbnailLocation,
  'ThumbnailFileName': instance.thumbnailFileName,
  'ThumbnailFileSize': instance.thumbnailFileSize,
  'UserId': instance.userId,
};

ApiV1ManifestUploadvisionboardpicturethumbnailPost$RequestBody
_$ApiV1ManifestUploadvisionboardpicturethumbnailPost$RequestBodyFromJson(
  Map<String, dynamic> json,
) => ApiV1ManifestUploadvisionboardpicturethumbnailPost$RequestBody(
  visionBoardPictureId: (json['VisionBoardPictureId'] as num?)?.toInt(),
  uploadedFile: json['UploadedFile'] as String?,
);

Map<String, dynamic>
_$ApiV1ManifestUploadvisionboardpicturethumbnailPost$RequestBodyToJson(
  ApiV1ManifestUploadvisionboardpicturethumbnailPost$RequestBody instance,
) => <String, dynamic>{
  'VisionBoardPictureId': instance.visionBoardPictureId,
  'UploadedFile': instance.uploadedFile,
};

ApiV1TwilioHandlegatherPost$RequestBody
_$ApiV1TwilioHandlegatherPost$RequestBodyFromJson(Map<String, dynamic> json) =>
    ApiV1TwilioHandlegatherPost$RequestBody(digits: json['digits'] as String?);

Map<String, dynamic> _$ApiV1TwilioHandlegatherPost$RequestBodyToJson(
  ApiV1TwilioHandlegatherPost$RequestBody instance,
) => <String, dynamic>{'digits': instance.digits};

ApiV1TwilioHandlestatusPost$RequestBody
_$ApiV1TwilioHandlestatusPost$RequestBodyFromJson(Map<String, dynamic> json) =>
    ApiV1TwilioHandlestatusPost$RequestBody(
      callStatus: json['callStatus'] as String?,
      smsStatus: json['smsStatus'] as String?,
    );

Map<String, dynamic> _$ApiV1TwilioHandlestatusPost$RequestBodyToJson(
  ApiV1TwilioHandlestatusPost$RequestBody instance,
) => <String, dynamic>{
  'callStatus': instance.callStatus,
  'smsStatus': instance.smsStatus,
};
