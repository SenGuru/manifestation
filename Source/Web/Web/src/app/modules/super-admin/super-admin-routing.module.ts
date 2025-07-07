import { RouterModule, Routes } from "@angular/router";

import { AppLogModelListScreenComponent } from "src/app/shared/components/screen/app-log-model-list-screen/app-log-model-list-screen.component";
import { AuditOperationListScreenComponent } from "src/app/shared/components/screen/audit-operation-list-screen/audit-operation-list-screen.component";
import { GlobalSearchDetailsComponent } from "src/app/shared/components/global-search-details/global-search-details.component";
import { JobListScreenComponent } from "src/app/shared/components/screen/job-list-screen/job-list-screen.component";
import { JobTemplateCrudModalComponent } from "src/app/shared/modals/job-template-crud-modal/job-template-crud-modal.component";
import { JobTemplateListScreenComponent } from "src/app/shared/components/screen/job-template-list-screen/job-template-list-screen.component";
import { LayoutComponent } from "src/app/layouts/layout.component";
import { MailServerListScreenComponent } from "src/app/shared/components/screen/mail-server-list-screen/mail-server-list-screen.component";
import { MessageTemplateListScreenComponent } from "src/app/shared/components/screen/message-template-list-screen/message-template-list-screen.component";
import { MessageToSendListScreenComponent } from "src/app/shared/components/screen/message-to-send-list-screen/message-to-send-list-screen.component";
import { NgModule } from "@angular/core";
import { PerfLogModelListScreenComponent } from "src/app/shared/components/screen/perf-log-model-list-screen/perf-log-model-list-screen.component";
import { ProfileComponent } from "src/app/shared/profile/profile.component";
import { PushNotificationServerListScreenComponent } from "src/app/shared/components/screen/push-notification-server-list-screen/push-notification-server-list-screen.component";
import { ScheduledJobListScreenComponent } from "src/app/shared/components/screen/scheduled-job-list-screen/scheduled-job-list-screen.component";
import { SingleSignOnServerListScreenComponent } from "src/app/shared/components/screen/single-sign-on-server-list-screen/single-sign-on-server-list-screen.component";
import { SmsServerListScreenComponent } from "src/app/shared/components/screen/sms-server-list-screen/sms-server-list-screen.component";
import { UserListScreenComponent } from "src/app/shared/components/screen/user-list-screen/user-list-screen.component";
import { VoiceNotificationServerListScreenComponent } from "src/app/shared/components/screen/voice-notification-server-list-screen/voice-notification-server-list-screen.component";
import { WhatsAppNotificationServerListScreenComponent } from "src/app/shared/components/screen/whats-app-notification-server-list-screen/whats-app-notification-server-list-screen.component";

const routes: Routes = [
  {
    path: "",
    component: LayoutComponent,
    children: [
      { path: '', component: UserListScreenComponent },
      { path: "profile", component: ProfileComponent },
      { path: 'jobs', component: JobListScreenComponent },
      { path: 'app-logs', component: AppLogModelListScreenComponent },
      { path: 'perf-logs', component: PerfLogModelListScreenComponent },
      { path: 'audit', component: AuditOperationListScreenComponent },
      { path: 'users', component: UserListScreenComponent },
      { path: 'mail-server-config', component: MailServerListScreenComponent },
      { path: 'sms-server-config', component: SmsServerListScreenComponent },
      { path: 'single-sign-on', component: SingleSignOnServerListScreenComponent },
      { path: 'message-template', component: MessageTemplateListScreenComponent },
      { path: 'push-notification', component: PushNotificationServerListScreenComponent },
      { path: 'voice-notification', component: VoiceNotificationServerListScreenComponent },
      { path: 'whatsapp-notification', component: WhatsAppNotificationServerListScreenComponent },
      {
        path: 'jobs',
        children: [
          { path: 'scheduled-jobs', component: ScheduledJobListScreenComponent }
        ],
      },
      { path: "message-to-send", component: MessageToSendListScreenComponent },
      { path: "job-templates", component: JobTemplateListScreenComponent },
      {
        path: 'job-templates',
        children: [
          { path: 'job-template-item/:actionType/:jobTemplateId', component: JobTemplateCrudModalComponent },
          { path: 'job-template-item/:actionType', component: JobTemplateCrudModalComponent },
        ]
      }, 
      { path: 'global-search/:layoutTypeId', component: GlobalSearchDetailsComponent },
    ],
  },
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SuperAdminRoutingModule { }
