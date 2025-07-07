import { PredefinedAuditOperationTypes, PredefinedEmailNotificationStatusTypes, PredefinedFieldControlTypes, PredefinedFieldPopulatorTypes, PredefinedFieldTypes, PredefinedFileStorageTypes, PredefinedJobOrReportTypes, PredefinedJobStatusTypes, PredefinedJobTypes, PredefinedJobUsageForTypes, PredefinedMessagePriorityTypes, PredefinedMessageTemplateTypes, PredefinedMessageToSendTypes, PredefinedNotificationMessageTypes, PredefinedPhoneCallNotificationStatusTypes, PredefinedPushNotificationServerTypes, PredefinedPushNotificationStatusTypes, PredefinedRealtimeMessageStatusTypes, PredefinedResourceTypes, PredefinedSMSNotificationStatusTypes, PredefinedSingleSignOnServerTypes, PredefinedSmsAndVoiceServerTypes, PredefinedUserPermissionTypes, PredefinedUserTypes, PredefinedValidatorTypes, PredefinedWhatsAppNotificationStatusTypes } from "./app-enums";

import { Injectable } from "@angular/core";
import { LogSeverityType } from "./app-custom-model";
import { Router } from "@angular/router";

@Injectable({
  providedIn: 'root'
})

export class EnumViewService {
  public constructor(private router: Router) { }

  public navigateToModule(userTypeId: number): void {
    switch (userTypeId) {
      case PredefinedUserTypes.SuperAdmin:
        this.router.navigate(["/sa/companies"]);
        break; 
      default:
        this.router.navigate(["/login"]);
    }
  }

  public getInititalUrl(userTypeId: number): string {
    switch ((userTypeId)) {
      case PredefinedUserTypes.SuperAdmin:
        return 'sa'; 
      default:
        return 'user';
    }
  }
 
  public getPredefinedAuditOperationTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedAuditOperationTypes.ResourceCreated:
        return 'success';
      case PredefinedAuditOperationTypes.ResourceUpdated:
        return 'primary';
      case PredefinedAuditOperationTypes.ResourceDeleted:
        return 'secondary';
      case PredefinedAuditOperationTypes.ResourceUndeleted:
        return 'danger';
      case PredefinedAuditOperationTypes.ResourceHardDeleted:
        return 'info';
      case PredefinedAuditOperationTypes.ResourceEnable:
        return 'warning';
      case PredefinedAuditOperationTypes.ResourceDisable:
        return 'success';
      case PredefinedAuditOperationTypes.SuperAdminLogin:
        return 'primary';
      case PredefinedAuditOperationTypes.Login:
        return 'secondary';
      case PredefinedAuditOperationTypes.Logout:
        return 'danger';
      case PredefinedAuditOperationTypes.LoginFailed:
        return 'info';
      case PredefinedAuditOperationTypes.ForgotPasswordFailedRequest:
        return 'warning';
      case PredefinedAuditOperationTypes.ResetPassword:
        return 'success';
      case PredefinedAuditOperationTypes.SocialLogin:
        return 'primary';
      case PredefinedAuditOperationTypes.SocialLoginFailed:
        return 'secondary';
      case PredefinedAuditOperationTypes.AlternateLogin:
        return 'danger';
      case PredefinedAuditOperationTypes.AlternateLoginFailed:
        return 'info';
      case PredefinedAuditOperationTypes.ChangedPassword:
        return 'warning';
      case PredefinedAuditOperationTypes.ForgotPasswordRequest:
        return 'success';
      case PredefinedAuditOperationTypes.UserCreated:
        return 'primary';
      case PredefinedAuditOperationTypes.UserUpdated:
        return 'secondary';
      case PredefinedAuditOperationTypes.UserDeleted:
        return 'danger';
      case PredefinedAuditOperationTypes.LoginEnabled:
        return 'info';
      case PredefinedAuditOperationTypes.LoginDisabled:
        return 'warning';
      case PredefinedAuditOperationTypes.UpdateConfiguration:
        return 'success';
      case PredefinedAuditOperationTypes.Report:
        return 'primary';
      case PredefinedAuditOperationTypes.RunReport:
        return 'secondary';
      case PredefinedAuditOperationTypes.ViewReport:
        return 'danger';
      case PredefinedAuditOperationTypes.DownloadReport:
        return 'info';
      default:
        return 'primary';
    }
  }

  public getAppLogSeverityTypeClass(value: LogSeverityType | string): string {
    switch (value) {
      case "CRITICAL":
        return 'bg-red-lt';
      case "DEBUG":
        return 'bg-purple-lt';
      case "ERROR":
        return 'bg-red-lt';
      case "FATAL":
        return 'badge-soft-success';
      case "INFO":
        return 'bg-teal-lt';
      case "SQL":
        return 'badge-soft-info';
      case "SQL-ERROR":
        return 'bg-orangish-lt';
      case "TRACE":
        return 'bg-green-lt';
      case "TRANS-BEGIN":
        return 'bg-yellowish-lt';
      case "TRANS-COMMIT":
        return 'bg-crimson-lt';
      case "TRANS-ROLLBACK":
        return 'bg-orangish-lt';
      case "WARNING":
        return 'bg-orangish-lt';
      default:
        return 'bg-blue-lt';
    }
  }

  public getPredefinedEmailNotificationStatusTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedEmailNotificationStatusTypes.NotNeeded:
        return 'success';
      case PredefinedEmailNotificationStatusTypes.Scheduled:
        return 'primary';
      case PredefinedEmailNotificationStatusTypes.Sent:
        return 'secondary';
      case PredefinedEmailNotificationStatusTypes.Confirmed:
        return 'danger';
      case PredefinedEmailNotificationStatusTypes.NotConfirmed:
        return 'info';
      case PredefinedEmailNotificationStatusTypes.ViewedAndNotConfirmed:
        return 'warning';
      case PredefinedEmailNotificationStatusTypes.Declined:
        return 'success';
      case PredefinedEmailNotificationStatusTypes.NotificationError:
        return 'primary';
      case PredefinedEmailNotificationStatusTypes.ServerNotConfigured:
        return 'secondary';
      default:
        return 'primary';
    }
  }

  public getPredefinedFieldControlTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedFieldControlTypes.SingleLineText:
        return 'success';
      case PredefinedFieldControlTypes.MultiLineText:
        return 'primary';
      case PredefinedFieldControlTypes.CheckBox:
        return 'secondary';
      case PredefinedFieldControlTypes.SingleSelectionCombo:
        return 'danger';
      case PredefinedFieldControlTypes.MultipleSelectionCombo:
        return 'info';
      case PredefinedFieldControlTypes.Integer:
        return 'warning';
      case PredefinedFieldControlTypes.PositiveInteger:
        return 'success';
      case PredefinedFieldControlTypes.Double:
        return 'primary';
      case PredefinedFieldControlTypes.PositiveDouble:
        return 'secondary';
      case PredefinedFieldControlTypes.AnyDateTime:
        return 'danger';
      case PredefinedFieldControlTypes.FutureDateTime:
        return 'info';
      case PredefinedFieldControlTypes.AnyDate:
        return 'warning';
      case PredefinedFieldControlTypes.FutureDate:
        return 'success';
      case PredefinedFieldControlTypes.AnyTime:
        return 'primary';
      default:
        return 'primary';
    }
  }

  public getPredefinedFieldPopulatorTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedFieldPopulatorTypes.None:
        return 'success';
      case PredefinedFieldPopulatorTypes.JsonStringListFromUrl:
        return 'primary';
      case PredefinedFieldPopulatorTypes.StringList:
        return 'secondary';
      default:
        return 'primary';
    }
  }

  public getPredefinedFieldTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedFieldTypes.String:
        return 'success';
      case PredefinedFieldTypes.StringList:
        return 'primary';
      case PredefinedFieldTypes.Integer:
        return 'secondary';
      case PredefinedFieldTypes.IntegerList:
        return 'danger';
      case PredefinedFieldTypes.Double:
        return 'info';
      case PredefinedFieldTypes.DoubleList:
        return 'warning';
      case PredefinedFieldTypes.Boolean:
        return 'success';
      case PredefinedFieldTypes.DateTime:
        return 'primary';
      default:
        return 'primary';
    }
  }

  public getPredefinedFileStorageTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedFileStorageTypes.ReportServerFileStorage:
        return 'success';
      case PredefinedFileStorageTypes.ProfilePictureFileStorage:
        return 'primary';
      case PredefinedFileStorageTypes.FileAttachmentFileStorage:
        return 'secondary';
      default:
        return 'primary';
    }
  }

  public getPredefinedJobOrReportTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedJobOrReportTypes.None:
        return 'success';
      case PredefinedJobOrReportTypes.UserList:
        return 'primary';
      default:
        return 'primary';
    }
  }

  public getPredefinedJobStatusTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedJobStatusTypes.Submitted:
        return 'success';
      case PredefinedJobStatusTypes.Running:
        return 'primary';
      case PredefinedJobStatusTypes.Error:
        return 'secondary';
      case PredefinedJobStatusTypes.Completed:
        return 'danger';
      case PredefinedJobStatusTypes.Waiting:
        return 'info';
      case PredefinedJobStatusTypes.Cancelled:
        return 'warning';
      case PredefinedJobStatusTypes.Skipped:
        return 'success';
      case PredefinedJobStatusTypes.None:
        return 'primary';
      default:
        return 'primary';
    }
  }

  public getPredefinedJobTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedJobTypes.None:
        return 'success';
      case PredefinedJobTypes.Report:
        return 'primary';
      case PredefinedJobTypes.Job:
        return 'secondary';
      case PredefinedJobTypes.UserList:
        return 'danger';
      case PredefinedJobTypes.DisableMovieProcessor:
        return 'info';
      case PredefinedJobTypes.WakeupNotificationProcessor:
        return 'warning';
      default:
        return 'primary';
    }
  }

  public getPredefinedJobUsageForTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedJobUsageForTypes.All:
        return 'success';
      case PredefinedJobUsageForTypes.SuperAdmins:
        return 'primary';
      case PredefinedJobUsageForTypes.CompanyAdmins:
        return 'secondary';
      default:
        return 'primary';
    }
  }

  public getPredefinedMessagePriorityTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedMessagePriorityTypes.Error:
        return 'success';
      case PredefinedMessagePriorityTypes.High:
        return 'primary';
      case PredefinedMessagePriorityTypes.Normal:
        return 'secondary';
      default:
        return 'primary';
    }
  }

  public getPredefinedMessageTemplateTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedMessageTemplateTypes.UserList:
        return 'success';
      case PredefinedMessageTemplateTypes.UserCreation:
        return 'primary';
      case PredefinedMessageTemplateTypes.ForgotPassword:
        return 'secondary';
      case PredefinedMessageTemplateTypes.ChangedPassword:
        return 'danger';
      case PredefinedMessageTemplateTypes.ReportSuccess:
        return 'info';
      case PredefinedMessageTemplateTypes.ReportError:
        return 'warning';
      case PredefinedMessageTemplateTypes.MemberOtp:
        return 'success';
      default:
        return 'primary';
    }
  }

  public getPredefinedMessageToSendTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedMessageToSendTypes.Email:
        return 'success';
      case PredefinedMessageToSendTypes.Sms:
        return 'primary';
      case PredefinedMessageToSendTypes.RealtimeMessage:
        return 'secondary';
      default:
        return 'primary';
    }
  }
 
  public getPredefinedNotificationMessageTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedNotificationMessageTypes.TaskAssigned:
        return 'success';
      default:
        return 'primary';
    }
  }

  public getPredefinedPhoneCallNotificationStatusTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedPhoneCallNotificationStatusTypes.NotNeeded:
        return 'success';
      case PredefinedPhoneCallNotificationStatusTypes.Scheduled:
        return 'primary';
      case PredefinedPhoneCallNotificationStatusTypes.Confirmed:
        return 'secondary';
      case PredefinedPhoneCallNotificationStatusTypes.NotConfirmed:
        return 'danger';
      case PredefinedPhoneCallNotificationStatusTypes.Declined:
        return 'info';
      case PredefinedPhoneCallNotificationStatusTypes.NotificationError:
        return 'warning';
      case PredefinedPhoneCallNotificationStatusTypes.ServerNotConfigured:
        return 'success';
      default:
        return 'primary';
    }
  }

  public getPredefinedPushNotificationServerTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedPushNotificationServerTypes.FCM:
        return 'success';
      case PredefinedPushNotificationServerTypes.AzureNotificationHubs:
        return 'primary';
      case PredefinedPushNotificationServerTypes.OneSignal:
        return 'secondary';
      case PredefinedPushNotificationServerTypes.ApplePushNotificationService:
        return 'danger';
      case PredefinedPushNotificationServerTypes.CustomWebSocket:
        return 'info';
      default:
        return 'primary';
    }
  }

  public getPredefinedPushNotificationStatusTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedPushNotificationStatusTypes.NotNeeded:
        return 'success';
      case PredefinedPushNotificationStatusTypes.Scheduled:
        return 'primary';
      case PredefinedPushNotificationStatusTypes.Sent:
        return 'secondary';
      case PredefinedPushNotificationStatusTypes.Confirmed:
        return 'danger';
      case PredefinedPushNotificationStatusTypes.NotConfirmed:
        return 'info';
      case PredefinedPushNotificationStatusTypes.ViewedAndNotConfirmed:
        return 'warning';
      case PredefinedPushNotificationStatusTypes.Declined:
        return 'success';
      case PredefinedPushNotificationStatusTypes.NotificationError:
        return 'primary';
      case PredefinedPushNotificationStatusTypes.ServerNotConfigured:
        return 'secondary';
      default:
        return 'primary';
    }
  }

  public getPredefinedResourceTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedResourceTypes.Company:
        return 'success';
      case PredefinedResourceTypes.User:
        return 'primary';
      case PredefinedResourceTypes.State:
        return 'secondary';
      case PredefinedResourceTypes.UserPermission:
        return 'danger';
      case PredefinedResourceTypes.AuditOperation:
        return 'info';
      case PredefinedResourceTypes.ConfigurationGroup:
        return 'warning';
      case PredefinedResourceTypes.ConfigurationItem:
        return 'success';
      case PredefinedResourceTypes.EnumGroup:
        return 'primary';
      case PredefinedResourceTypes.EnumType:
        return 'secondary';
      case PredefinedResourceTypes.Job:
        return 'danger';
      case PredefinedResourceTypes.JobAttachment:
        return 'info';
      case PredefinedResourceTypes.JobNotificationEmail:
        return 'warning';
      case PredefinedResourceTypes.JobTemplate:
        return 'success';
      case PredefinedResourceTypes.JobTemplateParameter:
        return 'primary';
      case PredefinedResourceTypes.MetaData:
        return 'secondary';
      case PredefinedResourceTypes.MetaDataGroup:
        return 'danger';
      case PredefinedResourceTypes.MetaDataGroupSection:
        return 'info';
      case PredefinedResourceTypes.RealtimeMessage:
        return 'warning';
      case PredefinedResourceTypes.ScheduledJob:
        return 'success';
      case PredefinedResourceTypes.JobTemplateCategory:
        return 'primary';
      case PredefinedResourceTypes.SavedJob:
        return 'secondary';
      case PredefinedResourceTypes.MessageToSend:
        return 'danger';
      case PredefinedResourceTypes.SavedJobNotificationEmail:
        return 'info';
      case PredefinedResourceTypes.ScheduledJobHistory:
        return 'warning';
      case PredefinedResourceTypes.ScheduledJobNotificationEmail:
        return 'success';
      case PredefinedResourceTypes.CompanyStorageConfiguration:
        return 'primary';
      case PredefinedResourceTypes.MailServer:
        return 'secondary';
      case PredefinedResourceTypes.NoteAttachment:
        return 'danger';
      case PredefinedResourceTypes.SingleSignOnServer:
        return 'info';
      case PredefinedResourceTypes.MessageTemplate:
        return 'warning';
      case PredefinedResourceTypes.AuditOperationDetail:
        return 'success';
      case PredefinedResourceTypes.FileAttachment:
        return 'primary';
      case PredefinedResourceTypes.FileAttachmentFolder:
        return 'secondary';
      case PredefinedResourceTypes.SmsServer:
        return 'danger';
      case PredefinedResourceTypes.Chat:
        return 'info';
      case PredefinedResourceTypes.ChatMessageStatus:
        return 'warning';
      case PredefinedResourceTypes.ChatUser:
        return 'success';
      case PredefinedResourceTypes.ChatMessage:
        return 'primary';
      case PredefinedResourceTypes.PushNotificationServer:
        return 'secondary';
      case PredefinedResourceTypes.VoiceNotificationServer:
        return 'danger';
      case PredefinedResourceTypes.WhatsAppNotificationServer:
        return 'info';
      case PredefinedResourceTypes.UserGroup:
        return 'warning';
      case PredefinedResourceTypes.CompanyEntity:
        return 'info';
      default:
        return 'primary';
    }
  }

  public getPredefinedRealtimeMessageStatusTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedRealtimeMessageStatusTypes.NotYetSent:
        return 'success';
      case PredefinedRealtimeMessageStatusTypes.Sent:
        return 'primary';
      case PredefinedRealtimeMessageStatusTypes.Viewed:
        return 'secondary';
      case PredefinedRealtimeMessageStatusTypes.Deleted:
        return 'danger';
      case PredefinedRealtimeMessageStatusTypes.Error:
        return 'info';
      default:
        return 'primary';
    }
  }

  public getPredefinedSingleSignOnServerTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedSingleSignOnServerTypes.Google:
        return 'success';
      case PredefinedSingleSignOnServerTypes.Microsoft:
        return 'primary';
      case PredefinedSingleSignOnServerTypes.Office360ActiveDirectory:
        return 'secondary';
      case PredefinedSingleSignOnServerTypes.SelfHostedActiveDirectory:
        return 'danger';
      default:
        return 'primary';
    }
  }

  public getPredefinedSmsAndVoiceServerTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedSmsAndVoiceServerTypes.Twilio:
        return 'success';
      case PredefinedSmsAndVoiceServerTypes.Custom:
        return 'primary';
      default:
        return 'primary';
    }
  }

  public getPredefinedSMSNotificationStatusTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedSMSNotificationStatusTypes.NotNeeded:
        return 'success';
      case PredefinedSMSNotificationStatusTypes.Scheduled:
        return 'primary';
      case PredefinedSMSNotificationStatusTypes.Sent:
        return 'secondary';
      case PredefinedSMSNotificationStatusTypes.Confirmed:
        return 'danger';
      case PredefinedSMSNotificationStatusTypes.NotConfirmed:
        return 'info';
      case PredefinedSMSNotificationStatusTypes.ViewedAndNotConfirmed:
        return 'warning';
      case PredefinedSMSNotificationStatusTypes.Declined:
        return 'success';
      case PredefinedSMSNotificationStatusTypes.NotificationError:
        return 'primary';
      case PredefinedSMSNotificationStatusTypes.ServerNotConfigured:
        return 'secondary';
      default:
        return 'primary';
    }
  }
  public getPredefinedUserPermissionTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedUserPermissionTypes.ConfigureSystem:
        return 'success';
      case PredefinedUserPermissionTypes.ConfigureCompany:
        return 'primary';
      case PredefinedUserPermissionTypes.ViewCompany:
        return 'secondary';
      case PredefinedUserPermissionTypes.ManageUser:
        return 'danger';
      case PredefinedUserPermissionTypes.ManageCallNotification:
        return 'info';
      case PredefinedUserPermissionTypes.ViewCallNotification:
        return 'warning';
      case PredefinedUserPermissionTypes.ManagePaymentStatus:
        return 'success';
      case PredefinedUserPermissionTypes.ViewPaymentStatus:
        return 'primary';
      case PredefinedUserPermissionTypes.ViewAnalytics:
        return 'secondary';
      case PredefinedUserPermissionTypes.ViewChat:
        return 'danger';
      default:
        return 'primary';
    }
  }

  public getPredefinedUserTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedUserTypes.SuperAdmin:
        return 'success';
      default:
        return 'primary';
    }
  }

  public getPredefinedValidatorTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedValidatorTypes.None:
        return 'success';
      case PredefinedValidatorTypes.Int:
        return 'primary';
      case PredefinedValidatorTypes.Float:
        return 'secondary';
      case PredefinedValidatorTypes.PositiveInt:
        return 'danger';
      case PredefinedValidatorTypes.PositiveFloat:
        return 'info';
      case PredefinedValidatorTypes.Email:
        return 'warning';
      case PredefinedValidatorTypes.USPhone:
        return 'success';
      case PredefinedValidatorTypes.USZipCode:
        return 'primary';
      case PredefinedValidatorTypes.CustomRegEx:
        return 'secondary';
      default:
        return 'primary';
    }
  }

  public getPredefinedWhatsAppNotificationStatusTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedWhatsAppNotificationStatusTypes.NotNeeded:
        return 'success';
      case PredefinedWhatsAppNotificationStatusTypes.Scheduled:
        return 'primary';
      case PredefinedWhatsAppNotificationStatusTypes.Sent:
        return 'secondary';
      case PredefinedWhatsAppNotificationStatusTypes.Confirmed:
        return 'danger';
      case PredefinedWhatsAppNotificationStatusTypes.NotConfirmed:
        return 'info';
      case PredefinedWhatsAppNotificationStatusTypes.ViewedAndNotConfirmed:
        return 'warning';
      case PredefinedWhatsAppNotificationStatusTypes.Declined:
        return 'success';
      case PredefinedWhatsAppNotificationStatusTypes.NotificationError:
        return 'primary';
      case PredefinedWhatsAppNotificationStatusTypes.ServerNotConfigured:
        return 'secondary';
      default:
        return 'primary';
    }
  }
}