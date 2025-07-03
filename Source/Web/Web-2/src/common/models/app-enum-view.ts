import { PredefinedAddressTypes, PredefinedAuditOperationTypes, PredefinedCompanyLocationTypes, PredefinedCompanyTypes, PredefinedCompanyUserCreditHistoryTypes, PredefinedDeliveryPartnerFirmTypes, PredefinedDeliveryPartnerTypes, PredefinedDeliveryScheduleTypes, PredefinedFieldControlTypes, PredefinedFieldPopulatorTypes, PredefinedFieldTypes, PredefinedFileStorageTypes, PredefinedInvoiceStatusTypes, PredefinedJobOrReportTypes, PredefinedJobStatusTypes, PredefinedJobTypes, PredefinedJobUsageForTypes, PredefinedMessagePriorityTypes, PredefinedMessageTemplateTypes, PredefinedMessageToSendTypes, PredefinedMetaDataGroupTypes, PredefinedNotificationMessageTypes, PredefinedOrderApprovalWorkflowTypes, PredefinedOrderDeliveryPartnerStatusTypes, PredefinedOrderStatusTypes, PredefinedPaymentProcessorTypes, PredefinedProductInventoryHistoryTypes, PredefinedProductTypes, PredefinedRealtimeMessageStatusTypes, PredefinedResourceTypes, PredefinedSingleSignOnServerTypes, PredefinedStatementTemplateTypes, PredefinedUserLoyaltyPointHistoryTypes, PredefinedUserLoyaltyPointRemovalTypes, PredefinedUserPermissionTypes, PredefinedUserTypes } from "./app-enums";

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
      case PredefinedUserTypes.HostAdmin:
        this.router.navigate(["/host-admin/companies"]);
        break;
      default:
        this.router.navigate(["/login"]);
    }
  }

  public getInititalUrl(userTypeId: number): string {
    switch ((userTypeId)) {
      case PredefinedUserTypes.SuperAdmin:
        return 'sa';
      case PredefinedUserTypes.HostAdmin:
        return "host-admin";
      default:
        return 'user';
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
      case PredefinedAuditOperationTypes.StenoMachineInfoView:
        return 'primary';
      case PredefinedAuditOperationTypes.StenoMachineInfoDownloadLog:
        return 'secondary';
      case PredefinedAuditOperationTypes.UserCreated:
        return 'danger';
      case PredefinedAuditOperationTypes.UserUpdated:
        return 'info';
      case PredefinedAuditOperationTypes.UserDeleted:
        return 'warning';
      case PredefinedAuditOperationTypes.LoginEnabled:
        return 'success';
      case PredefinedAuditOperationTypes.LoginDisabled:
        return 'primary';
      case PredefinedAuditOperationTypes.UpdateConfiguration:
        return 'secondary';
      case PredefinedAuditOperationTypes.Report:
        return 'danger';
      case PredefinedAuditOperationTypes.RunReport:
        return 'info';
      case PredefinedAuditOperationTypes.ViewReport:
        return 'warning';
      case PredefinedAuditOperationTypes.DownloadReport:
        return 'success';
      default:
        return 'primary';
    }
  }

  public getPredefinedCompanyTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedCompanyTypes.System:
        return 'success';
      case PredefinedCompanyTypes.Host:
        return 'primary';
      case PredefinedCompanyTypes.MarketPlace:
        return 'secondary';
      case PredefinedCompanyTypes.Customer:
        return 'danger';
      case PredefinedCompanyTypes.Vendor:
        return 'info';
      default:
        return 'primary';
    }
  }

  public getPredefinedUserTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedUserTypes.SuperAdmin:
        return 'success';
      case PredefinedUserTypes.HostAdmin:
        return 'primary';
      case PredefinedUserTypes.HostManager:
        return 'secondary';
      case PredefinedUserTypes.HostDeliveryManager:
        return 'danger';
      case PredefinedUserTypes.HostDeliveryPersonnel:
        return 'info';
      case PredefinedUserTypes.MarketPlaceAdmin:
        return 'warning';
      case PredefinedUserTypes.MarketPlaceManager:
        return 'success';
      case PredefinedUserTypes.MarketPlaceDeliveryManager:
        return 'primary';
      case PredefinedUserTypes.MarketPlaceDeliveryPersonnel:
        return 'secondary';
      case PredefinedUserTypes.CustomerAdmin:
        return 'danger';
      case PredefinedUserTypes.CustomerManager:
        return 'info';
      case PredefinedUserTypes.CustomerHr:
        return 'warning';
      case PredefinedUserTypes.Employee:
        return 'success';
      case PredefinedUserTypes.EmployeeReferral:
        return 'primary';
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
        return 'info';
      case PredefinedResourceTypes.AuditOperation:
        return 'warning';
      case PredefinedResourceTypes.ConfigurationGroup:
        return 'success';
      case PredefinedResourceTypes.ConfigurationItem:
        return 'primary';
      case PredefinedResourceTypes.EnumGroup:
        return 'secondary';
      case PredefinedResourceTypes.EnumType:
        return 'danger';
      case PredefinedResourceTypes.Job:
        return 'info';
      case PredefinedResourceTypes.JobAttachment:
        return 'warning';
      case PredefinedResourceTypes.JobNotificationEmail:
        return 'success';
      case PredefinedResourceTypes.JobTemplate:
        return 'primary';
      case PredefinedResourceTypes.JobTemplateParameter:
        return 'secondary';
      case PredefinedResourceTypes.MetaData:
        return 'danger';
      case PredefinedResourceTypes.MetaDataGroup:
        return 'info';
      case PredefinedResourceTypes.MetaDataGroupSection:
        return 'warning';
      case PredefinedResourceTypes.RealtimeMessage:
        return 'success';
      case PredefinedResourceTypes.ScheduledJob:
        return 'primary';
      case PredefinedResourceTypes.JobTemplateCategory:
        return 'secondary';
      case PredefinedResourceTypes.SavedJob:
        return 'danger';
      case PredefinedResourceTypes.MessageToSend:
        return 'info';
      case PredefinedResourceTypes.SavedJobNotificationEmail:
        return 'secondary';
      case PredefinedResourceTypes.ScheduledJobHistory:
        return 'danger';
      case PredefinedResourceTypes.ScheduledJobNotificationEmail:
        return 'info';
      case PredefinedResourceTypes.CompanyStorageConfiguration:
        return 'warning';
      case PredefinedResourceTypes.MailServer:
        return 'success';
      case PredefinedResourceTypes.NoteAttachment:
        return 'success';
      case PredefinedResourceTypes.SingleSignOnServer:
        return 'danger';
      case PredefinedResourceTypes.MessageTemplate:
        return 'info';
      case PredefinedResourceTypes.AuditOperationDetail:
        return 'warning';
      case PredefinedResourceTypes.FileAttachment:
        return 'success';
      case PredefinedResourceTypes.Catalog:
        return 'primary';
      case PredefinedResourceTypes.CatalogProductPrice:
        return 'secondary';
      case PredefinedResourceTypes.CatalogProductPriceHistory:
        return 'danger';
      case PredefinedResourceTypes.Product:
        return 'info';
      case PredefinedResourceTypes.ProductBrand:
        return 'warning';
      case PredefinedResourceTypes.ProductCategory:
        return 'success';
      case PredefinedResourceTypes.ProductInventoryHistory:
        return 'primary';
      case PredefinedResourceTypes.CompanyConfiguration:
        return 'secondary';
      case PredefinedResourceTypes.CompanyUserCredit:
        return 'danger';
      case PredefinedResourceTypes.CompanyUser:
        return 'info';
      case PredefinedResourceTypes.CompanyCreditTier:
        return 'success';
      case PredefinedResourceTypes.CompanyUserCreditHistory:
        return 'primary';
      case PredefinedResourceTypes.UserGroup:
        return 'danger';
      case PredefinedResourceTypes.UserGroupUser:
        return 'info';
      case PredefinedResourceTypes.Invoice:
        return 'warning';
      case PredefinedResourceTypes.InvoiceOrder:
        return 'success';
      case PredefinedResourceTypes.Order:
        return 'primary';
      case PredefinedResourceTypes.OrderDeliveryPartner:
        return 'secondary';
      case PredefinedResourceTypes.OrderPayment:
        return 'danger';
      case PredefinedResourceTypes.OrderProduct:
        return 'info';
      case PredefinedResourceTypes.UserLoyaltyPointHistory:
        return 'warning';
      case PredefinedResourceTypes.OtpServer:
        return 'success';
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
      case PredefinedJobTypes.InvoiceGenerationProcessor:
        return 'info';
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

  public getPredefinedMetaDataGroupTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedMetaDataGroupTypes.ActorType:
        return 'success';
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

  public getPredefinedNotificationMessageTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedNotificationMessageTypes.TaskAssigned:
        return 'success';
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

  public getPredefinedFileStorageTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedFileStorageTypes.ReportServerFileStorage:
        return 'success';
      case PredefinedFileStorageTypes.ProfilePictureFileStorage:
        return 'primary';
      case PredefinedFileStorageTypes.LetterTemplateFileStorage:
        return 'secondary';
      case PredefinedFileStorageTypes.ContractFileStorage:
        return 'danger';
      case PredefinedFileStorageTypes.FileAttachmentFileStorage:
        return 'info';
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

  public getPredefinedAddressTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedAddressTypes.Home:
        return 'success';
      case PredefinedAddressTypes.Office:
        return 'primary';
      default:
        return 'primary';
    }
  }
  public getPredefinedCompanyLocationTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedCompanyLocationTypes.DistributionCenter:
        return 'success';
      case PredefinedCompanyLocationTypes.Office:
        return 'primary';
      case PredefinedCompanyLocationTypes.Home:
        return 'secondary';
      default:
        return 'primary';
    }
  }

  public getPredefinedCompanyUserCreditHistoryTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedCompanyUserCreditHistoryTypes.Credit:
        return 'success';
      case PredefinedCompanyUserCreditHistoryTypes.Expense:
        return 'primary';
      default:
        return 'primary';
    }
  }

  public getPredefinedDeliveryPartnerFirmTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedDeliveryPartnerFirmTypes.External:
        return 'success';
      case PredefinedDeliveryPartnerFirmTypes.Own:
        return 'primary';
      default:
        return 'primary';
    }
  }

  public getPredefinedDeliveryPartnerTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedDeliveryPartnerTypes.Internal:
        return 'success';
      case PredefinedDeliveryPartnerTypes.External:
        return 'primary';
      default:
        return 'primary';
    }
  }

  public getPredefinedDeliveryScheduleTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedDeliveryScheduleTypes.BiMonthly:
        return 'success';
      case PredefinedDeliveryScheduleTypes.Monthly:
        return 'primary';
      case PredefinedDeliveryScheduleTypes.BiWeekly:
        return 'secondary';
      case PredefinedDeliveryScheduleTypes.Weekly:
        return 'danger';
      default:
        return 'primary';
    }
  }

  public getPredefinedInvoiceStatusTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedInvoiceStatusTypes.Unpaid:
        return 'success';
      case PredefinedInvoiceStatusTypes.Paid:
        return 'primary';
      case PredefinedInvoiceStatusTypes.PartiallyPaid:
        return 'secondary';
      case PredefinedInvoiceStatusTypes.Void:
        return 'danger';
      case PredefinedInvoiceStatusTypes.Cancelled:
        return 'info';
      default:
        return 'primary';
    }
  }

  public getPredefinedOrderApprovalWorkflowTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedOrderApprovalWorkflowTypes.Simple:
        return 'primary';
      case PredefinedOrderApprovalWorkflowTypes.WithHRApproval:
        return 'secondary';
      default:
        return 'primary';
    }
  }

  public getPredefinedOrderDeliveryPartnerStatusTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedOrderDeliveryPartnerStatusTypes.Assigned:
        return 'success';
      case PredefinedOrderDeliveryPartnerStatusTypes.Completed:
        return 'primary';
      case PredefinedOrderDeliveryPartnerStatusTypes.Cancelled:
        return 'secondary';
      default:
        return 'primary';
    }
  }

  public getPredefinedOrderStatusTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedOrderStatusTypes.Submitted:
        return 'success';
      case PredefinedOrderStatusTypes.WaitingApproval:
        return 'primary';
      case PredefinedOrderStatusTypes.Confirmed:
        return 'secondary';
      case PredefinedOrderStatusTypes.Processing:
        return 'warning';
      case PredefinedOrderStatusTypes.OutForDelivery:
        return 'info';
      case PredefinedOrderStatusTypes.Delivered:
        return 'success';
      case PredefinedOrderStatusTypes.DeliveryFailed:
        return 'danger';
      case PredefinedOrderStatusTypes.AssignedToHR:
        return 'secondary';
      case PredefinedOrderStatusTypes.Rejected:
        return 'danger';
      case PredefinedOrderStatusTypes.NotDeliveredWillRetry:
        return 'info';
      default:
        return 'primary';
    }
  }

  public getPredefinedPaymentProcessorTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedPaymentProcessorTypes.CreditCard:
        return 'success';
      case PredefinedPaymentProcessorTypes.COD:
        return 'primary';
      case PredefinedPaymentProcessorTypes.CompanyCredit:
        return 'secondary';
      default:
        return 'primary';
    }
  }

  public getPredefinedProductTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedProductTypes.General:
        return 'success';
      case PredefinedProductTypes.Independent:
        return 'primary';
      default:
        return 'primary';
    }
  }

  public getPredefinedUserLoyaltyPointHistoryTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedUserLoyaltyPointHistoryTypes.Addition:
        return 'success';
      case PredefinedUserLoyaltyPointHistoryTypes.Redeemed:
        return 'primary';
      case PredefinedUserLoyaltyPointHistoryTypes.Expiration:
        return 'secondary';
      default:
        return 'primary';
    }
  }

  public getPredefinedUserLoyaltyPointRemovalTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedUserLoyaltyPointRemovalTypes.ForOrder:
        return 'success';
      case PredefinedUserLoyaltyPointRemovalTypes.YearEndExpiration:
        return 'primary';
      default:
        return 'primary';
    }
  }

  public getPredefinedProductInventoryHistoryTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedProductInventoryHistoryTypes.Addition:
        return 'success';
      case PredefinedProductInventoryHistoryTypes.RemovalForOrder:
        return 'primary';
      default:
        return 'primary';
    }
  }

  public getPredefinedStatementTemplateTypeClass(typeId: number): string {
    switch (typeId) {
      case PredefinedStatementTemplateTypes.Invoice:
        return 'success';
      case PredefinedStatementTemplateTypes.Order:
        return 'primary';
      case PredefinedStatementTemplateTypes.DeliveryNote:
        return 'secondary';
      default:
        return 'primary';
    }
  }
}