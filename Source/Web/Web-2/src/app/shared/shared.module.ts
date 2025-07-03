import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { NgbAccordionModule, NgbDropdownModule, NgbModule, NgbNavModule, NgbPaginationModule } from "@ng-bootstrap/ng-bootstrap";

import { AppLogModelListScreenComponent } from "./components/screen/app-log-model-list-screen/app-log-model-list-screen.component";
import { AppSidebarComponent } from "./components/app-sidebar/app-sidebar.component";
import { AppTopBarComponent } from "./components/app-top-bar/app-top-bar.component";
import { AuditOperationCrudModalComponent } from "./modals/audit-operation-crud-modal/audit-operation-crud-modal.component";
import { AuditOperationListScreenComponent } from "./components/screen/audit-operation-list-screen/audit-operation-list-screen.component";
import { BlockUiComponent } from "./components/block-ui/block-ui.component";
import { ChangePasswordComponent } from "./modals/change-password/change-password.component";
import { CommonModule } from "@angular/common";
import { CompanyCrudModalComponent } from "./modals/company-crud-modal/company-crud-modal.component";
import { CompanyHeaderComponent } from "./components/screen/company-header/company-header.component";
import { CompanyListScreenComponent } from "./components/screen/company-list-screen/company-list-screen.component";
import { ConfigurationFileStorageEditorComponent } from "./modals/configuration-file-storage-editor/configuration-file-storage-editor.component";
import { ConfirmDialogueComponent } from "./modals/confirm-dialogue/confirm-dialogue.component";
import { CsvFileImportModalComponent } from "./modals/csv-file-import-modal/csv-file-import-modal.component";
import { DeleteModalComponent } from "./modals/delete-modal/delete-modal.component";
import { EmailvalidatorDirective } from "./directives/email-validatior.directive";
import { ErrorModalComponent } from "./modals/error-modal/error-modal.component";
import { FlatpickrModule } from "angularx-flatpickr";
import { FooterComponent } from "./components/footer/footer.component";
import { IconsModule } from "../modules/icons/icons.module";
import { JobCrudModalComponent } from "./modals/job-crud-modal/job-crud-modal.component";
import { JobListScreenComponent } from "./components/screen/job-list-screen/job-list-screen.component";
import { JobTemplateCategoryCrudModalComponent } from "./modals/job-template-category-crud-modal/job-template-category-crud-modal.component";
import { JobTemplateCategoryListScreenComponent } from "./components/screen/job-template-category-list-screen/job-template-category-list-screen.component";
import { JobTemplateCrudModalComponent } from "./modals/job-template-crud-modal/job-template-crud-modal.component";
import { JobTemplateListScreenComponent } from "./components/screen/job-template-list-screen/job-template-list-screen.component";
import { JobTemplateParameterCrudModalComponent } from "./modals/job-template-parameter-crud-modal/job-template-parameter-crud-modal.component";
import { JobTemplateParameterListScreenComponent } from "./components/screen/job-template-parameter-list-screen/job-template-parameter-list-screen.component";
import { LastItemPipe } from "src/common/pipes/last-item.pipe";
import { LogoutComponent } from "./components/logout/logout.component";
import { MailServerCrudModalComponent } from "./modals/mail-server-crud-modal/mail-server-crud-modal.component";
import { MailServerListScreenComponent } from "./components/screen/mail-server-list-screen/mail-server-list-screen.component";
import { MessageTemplateCrudModalComponent } from "./modals/message-template-crud-modal/message-template-crud-modal.component";
import { MessageTemplateListScreenComponent } from "./components/screen/message-template-list-screen/message-template-list-screen.component";
import { MessageToSendCrudModalComponent } from "./modals/message-to-send-crud-modal/message-to-send-crud-modal.component";
import { MessageToSendListScreenComponent } from "./components/screen/message-to-send-list-screen/message-to-send-list-screen.component";
import { NavbarTitleComponent } from "./components/navbar-title/navbar-title.component";
import { NgSelectModule } from "@ng-select/ng-select";
import { NgxEditorModule } from "ngx-editor";
import { NgxExtendedPdfViewerModule } from "ngx-extended-pdf-viewer";
import { NoDataFoundComponent } from "./components/no-data-found/no-data-found.component";
import { NotificationCardComponent } from "./components/notification-card/notification-card.component";
import { PaginationComponent } from "./components/pagination/pagination.component";
import { PerfLogModelListScreenComponent } from "./components/screen/perf-log-model-list-screen/perf-log-model-list-screen.component";
import { ProfileComponent } from "./profile/profile.component";
import { ProfileTitleComponent } from "./components/profile-title/profile-title.component";
import { PushNotificationServerCrudModalComponent } from "./modals/push-notification-server-crud-modal/push-notification-server-crud-modal.component";
import { PushNotificationServerListScreenComponent } from "./components/screen/push-notification-server-list-screen/push-notification-server-list-screen.component";
import { ResetPasswordComponent } from "./components/screen/reset-password/reset-password.component";
import { RouterModule } from "@angular/router";
import { RunJobModalComponent } from "./modals/run-job-modal/run-job-modal.component";
import { SavedJobCrudModalComponent } from "./modals/saved-job-crud-modal/saved-job-crud-modal.component";
import { SavedJobListScreenComponent } from "./components/screen/saved-job-list-screen/saved-job-list-screen.component";
import { SavedJobNotificationEmailCrudModalComponent } from "./modals/saved-job-notification-email-crud-modal/saved-job-notification-email-crud-modal.component";
import { SavedJobNotificationEmailListScreenComponent } from "./components/screen/saved-job-notification-email-list-screen/saved-job-notification-email-list-screen.component";
import { ScheduledJobCrudModalComponent } from "./modals/scheduled-job-crud-modal/scheduled-job-crud-modal.component";
import { ScheduledJobHistoryCrudModalComponent } from "./modals/scheduled-job-history-crud-modal/scheduled-job-history-crud-modal.component";
import { ScheduledJobHistoryListScreenComponent } from "./components/screen/scheduled-job-history-list-screen/scheduled-job-history-list-screen.component";
import { ScheduledJobListScreenComponent } from "./components/screen/scheduled-job-list-screen/scheduled-job-list-screen.component";
import { ScheduledJobNotificationEmailCrudModalComponent } from "./modals/scheduled-job-notification-email-crud-modal/scheduled-job-notification-email-crud-modal.component";
import { ScheduledJobNotificationEmailListScreenComponent } from "./components/screen/scheduled-job-notification-email-list-screen/scheduled-job-notification-email-list-screen.component";
import { SecondsToHourPipe } from "src/common/services/seconds-to-hour/seconds-to-hour.pipe";
import { SingleSignOnServerCrudModalComponent } from "./modals/single-sign-on-server-crud-modal/single-sign-on-server-crud-modal.component";
import { SingleSignOnServerListScreenComponent } from "./components/screen/single-sign-on-server-list-screen/single-sign-on-server-list-screen.component";
import { SmsServerCrudModalComponent } from "./modals/sms-server-crud-modal/sms-server-crud-modal.component";
import { SmsServerListScreenComponent } from "./components/screen/sms-server-list-screen/sms-server-list-screen.component";
import { SsoCallbackScreenComponent } from "./components/screen/sso-callback-screen/sso-callback-screen.component";
import { SuccessModalComponent } from "./modals/success-modal/success-modal.component";
import { SuperAdminSummaryHeaderComponent } from "./components/screen/super-admin-summary-header/super-admin-summary-header.component";
import { UserCrudModalComponent } from "./modals/user-crud-modal/user-crud-modal.component";
import { UserListScreenComponent } from "./components/screen/user-list-screen/user-list-screen.component";
import { UtcToBrowserTimePipe } from "src/common/services/utc-to-browser/utc-to-browser-time.pipe";
import { VoiceNotificationServerCrudModalComponent } from "./modals/voice-notification-server-crud-modal/voice-notification-server-crud-modal.component";
import { VoiceNotificationServerListScreenComponent } from "./components/screen/voice-notification-server-list-screen/voice-notification-server-list-screen.component";
import { WhatsAppNotificationServerCrudModalComponent } from "./modals/whats-app-notification-server-crud-modal/whats-app-notification-server-crud-modal.component";
import { WhatsAppNotificationServerListScreenComponent } from "./components/screen/whats-app-notification-server-list-screen/whats-app-notification-server-list-screen.component";

@NgModule({
  declarations: [
    UserCrudModalComponent,
    FooterComponent,
    UserListScreenComponent,
    ErrorModalComponent,
    BlockUiComponent,
    SuccessModalComponent,
    DeleteModalComponent,
    ProfileComponent,
    ConfirmDialogueComponent,
    ChangePasswordComponent,
    LogoutComponent,
    NoDataFoundComponent,
    NotificationCardComponent,
    ProfileTitleComponent,
    UtcToBrowserTimePipe,
    CsvFileImportModalComponent,
    CompanyListScreenComponent,
    CompanyCrudModalComponent,
    EmailvalidatorDirective,
    SuperAdminSummaryHeaderComponent,
    CompanyHeaderComponent,
    SecondsToHourPipe,
    LastItemPipe,
    SsoCallbackScreenComponent,
    NavbarTitleComponent,
    AppSidebarComponent,
    AppTopBarComponent,
    SuperAdminSummaryHeaderComponent,
    SmsServerListScreenComponent,
    SmsServerCrudModalComponent,
    AuditOperationListScreenComponent,
    AuditOperationCrudModalComponent,
    JobTemplateCategoryListScreenComponent,
    JobTemplateCategoryCrudModalComponent,
    JobTemplateCrudModalComponent,
    JobTemplateListScreenComponent,
    JobTemplateParameterListScreenComponent,
    JobTemplateParameterCrudModalComponent,
    MailServerListScreenComponent,
    MailServerCrudModalComponent,
    JobListScreenComponent,
    JobCrudModalComponent,
    MessageTemplateCrudModalComponent,
    MessageTemplateListScreenComponent,
    MessageToSendListScreenComponent,
    MessageToSendCrudModalComponent,
    PerfLogModelListScreenComponent,
    PushNotificationServerListScreenComponent,
    PushNotificationServerCrudModalComponent,
    SavedJobListScreenComponent,
    SavedJobCrudModalComponent,
    SavedJobNotificationEmailListScreenComponent,
    SavedJobNotificationEmailCrudModalComponent,
    ScheduledJobHistoryListScreenComponent,
    ScheduledJobHistoryCrudModalComponent,
    ScheduledJobListScreenComponent,
    ScheduledJobCrudModalComponent,
    ScheduledJobNotificationEmailListScreenComponent,
    ScheduledJobNotificationEmailCrudModalComponent,
    SingleSignOnServerListScreenComponent,
    SingleSignOnServerCrudModalComponent,
    VoiceNotificationServerListScreenComponent,
    VoiceNotificationServerCrudModalComponent,
    WhatsAppNotificationServerListScreenComponent,
    WhatsAppNotificationServerCrudModalComponent,
    RunJobModalComponent,
    ConfigurationFileStorageEditorComponent,
    AppLogModelListScreenComponent,
    ResetPasswordComponent,
    PaginationComponent,
  ],
  imports: [
    CommonModule,
    NgbNavModule,
    NgbModule,
    NgbAccordionModule,
    NgbDropdownModule,
    NgbPaginationModule,
    FormsModule,
    ReactiveFormsModule,
    NgSelectModule,
    RouterModule,
    // pdf preview module
    NgxExtendedPdfViewerModule,
    FlatpickrModule,
    NgxEditorModule,
    IconsModule,
  ],
  exports: [
    UserCrudModalComponent,
    FooterComponent,
    UserListScreenComponent,
    ProfileComponent,
    BlockUiComponent,
    ErrorModalComponent,
    SuccessModalComponent,
    DeleteModalComponent,
    ConfirmDialogueComponent,
    ChangePasswordComponent,
    LogoutComponent,
    NoDataFoundComponent,
    NotificationCardComponent,
    ProfileTitleComponent,
    UtcToBrowserTimePipe,
    CsvFileImportModalComponent,
    CompanyListScreenComponent,
    CompanyCrudModalComponent,
    EmailvalidatorDirective,
    SecondsToHourPipe,
    LastItemPipe,
    SsoCallbackScreenComponent,
    NavbarTitleComponent,
    AppSidebarComponent,
    AppTopBarComponent,
    SuperAdminSummaryHeaderComponent,
    SmsServerListScreenComponent,
    SmsServerCrudModalComponent,
    AuditOperationListScreenComponent,
    AuditOperationCrudModalComponent,
    JobTemplateCategoryListScreenComponent,
    JobTemplateCategoryCrudModalComponent,
    JobTemplateCrudModalComponent,
    JobTemplateListScreenComponent,
    JobTemplateParameterListScreenComponent,
    JobTemplateParameterCrudModalComponent,
    MailServerListScreenComponent,
    MailServerCrudModalComponent,
    JobListScreenComponent,
    JobCrudModalComponent,
    MessageTemplateCrudModalComponent,
    MessageTemplateListScreenComponent,
    MessageToSendListScreenComponent,
    MessageToSendCrudModalComponent,
    PerfLogModelListScreenComponent,
    PushNotificationServerListScreenComponent,
    PushNotificationServerCrudModalComponent,
    SavedJobListScreenComponent,
    SavedJobCrudModalComponent,
    SavedJobNotificationEmailListScreenComponent,
    SavedJobNotificationEmailCrudModalComponent,
    ScheduledJobHistoryListScreenComponent,
    ScheduledJobHistoryCrudModalComponent,
    ScheduledJobListScreenComponent,
    ScheduledJobCrudModalComponent,
    ScheduledJobNotificationEmailListScreenComponent,
    ScheduledJobNotificationEmailCrudModalComponent,
    SingleSignOnServerListScreenComponent,
    SingleSignOnServerCrudModalComponent,
    VoiceNotificationServerListScreenComponent,
    VoiceNotificationServerCrudModalComponent,
    WhatsAppNotificationServerListScreenComponent,
    WhatsAppNotificationServerCrudModalComponent,
    RunJobModalComponent,
    ConfigurationFileStorageEditorComponent,
    AppLogModelListScreenComponent,
    ResetPasswordComponent,
    PaginationComponent,
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class SharedModule {
}
