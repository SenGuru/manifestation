using System.Collections.Generic;
using Manifest.Common.Models.DB;
using Manifest.Common.Models.DBView;
using Manifest.Common.Models.DBView.System;
using Manifest.Common.Models.Queue;
using Manifest.Common.Models.ViewModels;
using Manifest.Common.Models.ViewModels.ImportExportModels;
using Manifest.Common.Models.ViewModels.System;
using Manifest.Common.Models.ViewModels.UserProfile;
using Riok.Mapperly.Abstractions;

namespace Manifest.Service.ObjectMapping
{
    [Riok.Mapperly.Abstractions.Mapper]
    public partial class ObjectMapperService
    {
        public partial AuditOperationDetail AuditOperationDetailUpdateModelToAuditOperationDetail(AuditOperationDetailUpdateModel model);

        public partial AuditOperationDetail AuditOperationDetailCreateModelToAuditOperationDetail(AuditOperationDetailCreateModel model);

        public partial List<AuditOperationDetailModel> AuditOperationDetailViewListToAuditOperationDetailModelList(List<AuditOperationDetailView> model);

        public partial AuditOperationDetailModel AuditOperationDetailViewToAuditOperationDetailModel(AuditOperationDetailView model);

        public partial List<AuditOperationModel> AuditOperationViewListToAuditOperationModelList(List<AuditOperationView> lst);

        public partial AuditOperationModel AuditOperationViewToAuditOperationModel(AuditOperationView vw);

        public partial AuditOperation AuditOperationCreateModelToAuditOperation(AuditOperationCreateModel model);

        public partial AuditOperation AuditOperationUpdateModelToAuditOperation(AuditOperationUpdateModel model);

        [MapperRequiredMapping(RequiredMappingStrategy.None)]
        public partial ChatCreateModel ChatUpdateModelToChatCreateModel(ChatUpdateModel model);

        public partial List<ChatModel> ChatViewListToChatModelList(List<ChatView> lst);

        public partial ChatModel ChatViewToChatModel(ChatView vw);

        public partial Chat ChatCreateModelToChat(ChatCreateModel model);

        public partial Chat ChatUpdateModelToChat(ChatUpdateModel model);

        //public partial List<ChatExportModel> ChatModelListToChatExportModelList(List<ChatModel> lst);

        public partial List<ChatMessageModel> ChatMessageViewListToChatMessageModelList(List<ChatMessageView> lst);

        public partial ChatMessageModel ChatMessageViewToChatMessageModel(ChatMessageView vw);

        public partial ChatMessage ChatMessageCreateModelToChatMessage(ChatMessageCreateModel model);

        public partial ChatMessage ChatMessageUpdateModelToChatMessage(ChatMessageUpdateModel model);

        //public partial List<ChatMessageExportModel> ChatMessageModelListToChatMessageExportModelList(List<ChatMessageModel> lst);

        public partial List<ChatMessageStatusModel> ChatMessageStatusViewListToChatMessageStatusModelList(List<ChatMessageStatusView> lst);

        public partial ChatMessageStatusModel ChatMessageStatusViewToChatMessageStatusModel(ChatMessageStatusView vw);

        public partial ChatMessageStatus ChatMessageStatusCreateModelToChatMessageStatus(ChatMessageStatusCreateModel model);

        public partial ChatMessageStatus ChatMessageStatusUpdateModelToChatMessageStatus(ChatMessageStatusUpdateModel model);

        //public partial List<ChatMessageStatusExportModel> ChatMessageStatusModelListToChatMessageStatusExportModelList(List<ChatMessageStatusModel> lst);
        public partial List<ChatUserModel> ChatUserViewListToChatUserModelList(List<ChatUserView> lst);

        public partial ChatUserModel ChatUserViewToChatUserModel(ChatUserView vw);

        public partial ChatUser ChatUserCreateModelToChatUser(ChatUserCreateModel model);

        public partial ChatUser ChatUserUpdateModelToChatUser(ChatUserUpdateModel model);

        //public partial List<ChatUserExportModel> ChatUserModelListToChatUserExportModelList(List<ChatUserModel> lst);

        public partial List<CompanyStorageConfigurationModel> CompanyStorageConfigurationViewListToCompanyStorageConfigurationModelList(List<CompanyStorageConfigurationView> lst);

        public partial CompanyStorageConfigurationModel CompanyStorageConfigurationViewToCompanyStorageConfigurationModel(CompanyStorageConfigurationView vw);

        public partial CompanyStorageConfiguration CompanyStorageConfigurationCreateModelToCompanyStorageConfiguration(CompanyStorageConfigurationCreateModel model);

        public partial CompanyStorageConfiguration CompanyStorageConfigurationUpdateModelToCompanyStorageConfiguration(CompanyStorageConfigurationUpdateModel model);

        //public partial List<CompanyStorageConfigurationExportModel> CompanyStorageConfigurationModelListToCompanyStorageConfigurationExportModelList(List<CompanyStorageConfigurationModel> lst);
        public partial List<ConfigurationGroupModel> ConfigurationGroupViewListToConfigurationGroupModelList(List<ConfigurationGroupView> lst);

        public partial ConfigurationGroupModel ConfigurationGroupViewToConfigurationGroupModel(ConfigurationGroupView vw);

        public partial ConfigurationGroup ConfigurationGroupCreateModelToConfigurationGroup(ConfigurationGroupCreateModel model);

        public partial ConfigurationGroup ConfigurationGroupUpdateModelToConfigurationGroup(ConfigurationGroupUpdateModel model);

        //public partial List<ConfigurationGroupExportModel> ConfigurationGroupModelListToConfigurationGroupExportModelList(List<ConfigurationGroupModel> lst);
        public partial List<ConfigurationItemModel> ConfigurationItemViewListToConfigurationItemModelList(List<ConfigurationItemView> lst);

        public partial ConfigurationItemModel ConfigurationItemViewToConfigurationItemModel(ConfigurationItemView vw);

        public partial ConfigurationItem ConfigurationItemCreateModelToConfigurationItem(ConfigurationItemCreateModel model);

        public partial ConfigurationItem ConfigurationItemUpdateModelToConfigurationItem(ConfigurationItemUpdateModel model);

        public partial List<CountryModel> CountryListToCountryModelList(List<Country> lst);

        //public partial List<ConfigurationItemExportModel> ConfigurationItemModelListToConfigurationItemExportModelList(List<ConfigurationItemModel> lst);
        //public partial List<CountryExportModel> CountryModelListToCountryExportModelList(List<CountryModel> lst);
        public partial List<EnumGroupModel> EnumGroupViewListToEnumGroupModelList(List<EnumGroupView> lst);

        public partial EnumGroupModel EnumGroupViewToEnumGroupModel(EnumGroupView vw);

        public partial EnumGroup EnumGroupCreateModelToEnumGroup(EnumGroupCreateModel model);

        public partial EnumGroup EnumGroupUpdateModelToEnumGroup(EnumGroupUpdateModel model);

        //public partial List<EnumGroupExportModel> EnumGroupModelListToEnumGroupExportModelList(List<EnumGroupModel> lst);
        public partial List<EnumTypeModel> EnumTypeViewListToEnumTypeModelList(List<EnumTypeView> lst);

        public partial EnumTypeModel EnumTypeViewToEnumTypeModel(EnumTypeView vw);

        public partial EnumType EnumTypeCreateModelToEnumType(EnumTypeCreateModel model);

        public partial EnumType EnumTypeUpdateModelToEnumType(EnumTypeUpdateModel model);

        //public partial List<EnumTypeExportModel> EnumTypeModelListToEnumTypeExportModelList(List<EnumTypeModel> lst);
        public partial List<FileAttachmentModel> FileAttachmentViewListToFileAttachmentModelList(List<FileAttachmentView> lst);

        public partial FileAttachmentModel FileAttachmentViewToFileAttachmentModel(FileAttachmentView vw);

        public partial FileAttachment FileAttachmentCreateModelToFileAttachment(FileAttachmentCreateModel model);

        public partial FileAttachment FileAttachmentUpdateModelToFileAttachment(FileAttachmentUpdateModel model);

        //public partial List<FileAttachmentExportModel> FileAttachmentModelListToFileAttachmentExportModelList(List<FileAttachmentModel> lst);
        public partial List<FileAttachmentFolderModel> FileAttachmentFolderViewListToFileAttachmentFolderModelList(List<FileAttachmentFolderView> lst);

        public partial FileAttachmentFolderModel FileAttachmentFolderViewToFileAttachmentFolderModel(FileAttachmentFolderView vw);

        public partial FileAttachmentFolder FileAttachmentFolderCreateModelToFileAttachmentFolder(FileAttachmentFolderCreateModel model);

        public partial FileAttachmentFolder FileAttachmentFolderUpdateModelToFileAttachmentFolder(FileAttachmentFolderUpdateModel model);

                //public partial List<FileAttachmentFolderExportModel> FileAttachmentFolderModelListToFileAttachmentFolderExportModelList(List<FileAttachmentFolderModel> lst);
        public partial List<JobModel> JobViewListToJobModelList(List<JobView> lst);

        [MapperRequiredMapping(RequiredMappingStrategy.None)]
        public partial JobModel JobViewToJobModel(JobView vw);

        public partial Job JobCreateModelToJob(JobCreateModel model);

        public partial Job JobUpdateModelToJob(JobUpdateModel model);

                //public partial List<JobExportModel> JobModelListToJobExportModelList(List<JobModel> lst);
        public partial List<JobAttachmentModel> JobAttachmentViewListToJobAttachmentModelList(List<JobAttachmentView> lst);

        public partial JobAttachmentModel JobAttachmentViewToJobAttachmentModel(JobAttachmentView vw);

        public partial JobAttachment JobAttachmentCreateModelToJobAttachment(JobAttachmentCreateModel model);

        public partial JobAttachment JobAttachmentUpdateModelToJobAttachment(JobAttachmentUpdateModel model);

                //public partial List<JobAttachmentExportModel> JobAttachmentModelListToJobAttachmentExportModelList(List<JobAttachmentModel> lst);
        public partial List<JobNotificationEmailModel> JobNotificationEmailViewListToJobNotificationEmailModelList(List<JobNotificationEmailView> lst);

        public partial JobNotificationEmailModel JobNotificationEmailViewToJobNotificationEmailModel(JobNotificationEmailView vw);

        public partial JobNotificationEmail JobNotificationEmailCreateModelToJobNotificationEmail(JobNotificationEmailCreateModel model);

        public partial JobNotificationEmail JobNotificationEmailUpdateModelToJobNotificationEmail(JobNotificationEmailUpdateModel model);

        [MapperRequiredMapping(RequiredMappingStrategy.None)]
        public partial JobModel JobTemplateModelToJobModel(JobTemplateModel model);

        //public partial List<JobNotificationEmailExportModel> JobNotificationEmailModelListToJobNotificationEmailExportModelList(List<JobNotificationEmailModel> lst);
        public partial List<JobTemplateModel> JobTemplateViewListToJobTemplateModelList(List<JobTemplateView> lst);

        [MapperRequiredMapping(RequiredMappingStrategy.None)]
        public partial JobTemplateModel JobTemplateViewToJobTemplateModel(JobTemplateView vw);

        public partial JobTemplate JobTemplateCreateModelToJobTemplate(JobTemplateCreateModel model);

        public partial JobTemplate JobTemplateUpdateModelToJobTemplate(JobTemplateUpdateModel model);

                //public partial List<JobTemplateExportModel> JobTemplateModelListToJobTemplateExportModelList(List<JobTemplateModel> lst);
        public partial List<JobTemplateCategoryModel> JobTemplateCategoryViewListToJobTemplateCategoryModelList(List<JobTemplateCategoryView> lst);

        public partial JobTemplateCategoryModel JobTemplateCategoryViewToJobTemplateCategoryModel(JobTemplateCategoryView vw);

        public partial JobTemplateCategory JobTemplateCategoryCreateModelToJobTemplateCategory(JobTemplateCategoryCreateModel model);

        public partial JobTemplateCategory JobTemplateCategoryUpdateModelToJobTemplateCategory(JobTemplateCategoryUpdateModel model);

                //public partial List<JobTemplateCategoryExportModel> JobTemplateCategoryModelListToJobTemplateCategoryExportModelList(List<JobTemplateCategoryModel> lst);
        public partial List<JobTemplateParameterModel> JobTemplateParameterViewListToJobTemplateParameterModelList(List<JobTemplateParameterView> lst);

        public partial JobTemplateParameterModel JobTemplateParameterViewToJobTemplateParameterModel(JobTemplateParameterView vw);

        public partial JobTemplateParameter JobTemplateParameterCreateModelToJobTemplateParameter(JobTemplateParameterCreateModel model);

        public partial JobTemplateParameter JobTemplateParameterUpdateModelToJobTemplateParameter(JobTemplateParameterUpdateModel model);

        //public partial List<JobTemplateParameterExportModel> JobTemplateParameterModelListToJobTemplateParameterExportModelList(List<JobTemplateParameterModel> lst);

        [MapperRequiredMapping(RequiredMappingStrategy.None)]
        public partial MailServerModel MailServerToMailServerModel(MailServer model);

        public partial List<MailServerModel> MailServerViewListToMailServerModelList(List<MailServerView> lst);

        public partial MailServerModel MailServerViewToMailServerModel(MailServerView vw);

        public partial MailServer MailServerCreateModelToMailServer(MailServerCreateModel model);

        public partial MailServer MailServerUpdateModelToMailServer(MailServerUpdateModel model);

                //public partial List<MailServerExportModel> MailServerModelListToMailServerExportModelList(List<MailServerModel> lst);
        public partial List<MessageTemplateModel> MessageTemplateViewListToMessageTemplateModelList(List<MessageTemplateView> lst);

        public partial MessageTemplateModel MessageTemplateViewToMessageTemplateModel(MessageTemplateView vw);

        public partial MessageTemplate MessageTemplateCreateModelToMessageTemplate(MessageTemplateCreateModel model);

        public partial MessageTemplate MessageTemplateUpdateModelToMessageTemplate(MessageTemplateUpdateModel model);

        public partial List<MessageToSendModel> MessageToSendListToMessageToSendModelList(List<MessageToSend> model);

        [MapperRequiredMapping(RequiredMappingStrategy.None)]
        public partial MessageToSendModel MessageToSendToMessageToSendModel(MessageToSend model);

        //public partial List<MessageTemplateExportModel> MessageTemplateModelListToMessageTemplateExportModelList(List<MessageTemplateModel> lst);
        public partial List<MessageToSendModel> MessageToSendViewListToMessageToSendModelList(List<MessageToSendView> lst);

        public partial MessageToSendModel MessageToSendViewToMessageToSendModel(MessageToSendView vw);

        public partial MessageToSend MessageToSendCreateModelToMessageToSend(MessageToSendCreateModel model);

        public partial MessageToSend MessageToSendUpdateModelToMessageToSend(MessageToSendUpdateModel model);

                //public partial List<MetaDataGroupSectionExportModel> MetaDataGroupSectionModelListToMetaDataGroupSectionExportModelList(List<MetaDataGroupSectionModel> lst);
        public partial List<NoteAttachmentModel> NoteAttachmentViewListToNoteAttachmentModelList(List<NoteAttachmentView> lst);

        public partial NoteAttachmentModel NoteAttachmentViewToNoteAttachmentModel(NoteAttachmentView vw);

        public partial NoteAttachment NoteAttachmentCreateModelToNoteAttachment(NoteAttachmentCreateModel model);

        public partial NoteAttachment NoteAttachmentUpdateModelToNoteAttachment(NoteAttachmentUpdateModel model);

                //public partial List<NoteAttachmentExportModel> NoteAttachmentModelListToNoteAttachmentExportModelList(List<NoteAttachmentModel> lst);
        public partial List<PushNotificationServerModel> PushNotificationServerViewListToPushNotificationServerModelList(List<PushNotificationServerView> lst);

        public partial PushNotificationServerModel PushNotificationServerViewToPushNotificationServerModel(PushNotificationServerView vw);

        public partial PushNotificationServer PushNotificationServerCreateModelToPushNotificationServer(PushNotificationServerCreateModel model);

        public partial PushNotificationServer PushNotificationServerUpdateModelToPushNotificationServer(PushNotificationServerUpdateModel model);

        //public partial List<PushNotificationServerExportModel> PushNotificationServerModelListToPushNotificationServerExportModelList(List<PushNotificationServerModel> lst);
        [MapperRequiredMapping(RequiredMappingStrategy.None)]
        public partial RealtimeMessageQueueModel RealtimeMessageViewToRealtimeMessageQueueModel(RealtimeMessageView model);

        public partial List<RealtimeMessageModel> RealtimeMessageViewListToRealtimeMessageModelList(List<RealtimeMessageView> lst);

        public partial RealtimeMessageModel RealtimeMessageViewToRealtimeMessageModel(RealtimeMessageView vw);

        public partial RealtimeMessage RealtimeMessageCreateModelToRealtimeMessage(RealtimeMessageCreateModel model);

        public partial RealtimeMessage RealtimeMessageUpdateModelToRealtimeMessage(RealtimeMessageUpdateModel model);

        public partial List<JobNotificationEmailModel> SavedJobNotificationEmailModelListToJobNotificationEmailModelList(List<SavedJobNotificationEmailModel> lst);

        [MapperRequiredMapping(RequiredMappingStrategy.None)]
        public partial JobNotificationEmailModel SavedJobNotificationEmailModelToJobNotificationEmailModel(SavedJobNotificationEmailModel model);

        [MapperRequiredMapping(RequiredMappingStrategy.None)]
        public partial JobModel SavedJobModelToJobModel(SavedJobModel model);

        //public partial List<RealtimeMessageExportModel> RealtimeMessageModelListToRealtimeMessageExportModelList(List<RealtimeMessageModel> lst);
        public partial List<SavedJobModel> SavedJobViewListToSavedJobModelList(List<SavedJobView> lst);

        [MapperRequiredMapping(RequiredMappingStrategy.None)]
        public partial SavedJobModel SavedJobViewToSavedJobModel(SavedJobView vw);

        public partial SavedJob SavedJobCreateModelToSavedJob(SavedJobCreateModel model);

        public partial SavedJob SavedJobUpdateModelToSavedJob(SavedJobUpdateModel model);

                //public partial List<SavedJobExportModel> SavedJobModelListToSavedJobExportModelList(List<SavedJobModel> lst);
        public partial List<SavedJobNotificationEmailModel> SavedJobNotificationEmailViewListToSavedJobNotificationEmailModelList(List<SavedJobNotificationEmailView> lst);

        public partial SavedJobNotificationEmailModel SavedJobNotificationEmailViewToSavedJobNotificationEmailModel(SavedJobNotificationEmailView vw);

        public partial SavedJobNotificationEmail SavedJobNotificationEmailCreateModelToSavedJobNotificationEmail(SavedJobNotificationEmailCreateModel model);

        public partial SavedJobNotificationEmail SavedJobNotificationEmailUpdateModelToSavedJobNotificationEmail(SavedJobNotificationEmailUpdateModel model);

        [MapperRequiredMapping(RequiredMappingStrategy.None)]
        public partial JobNotificationEmailModel ScheduledJobNotificationEmailModelToJobNotificationEmailModel(ScheduledJobNotificationEmailModel model);

        public partial List<JobNotificationEmailModel> ScheduledJobNotificationEmailModelListToJobNotificationEmailModelList(List<ScheduledJobNotificationEmailModel> lst);

        //public partial List<SavedJobNotificationEmailExportModel> SavedJobNotificationEmailModelListToSavedJobNotificationEmailExportModelList(List<SavedJobNotificationEmailModel> lst);
        [MapperRequiredMapping(RequiredMappingStrategy.None)]
        public partial JobModel ScheduledJobModelToJobModel(ScheduledJobModel model);

        public partial List<ScheduledJobModel> ScheduledJobViewListToScheduledJobModelList(List<ScheduledJobView> lst);

        [MapperRequiredMapping(RequiredMappingStrategy.None)]
        public partial ScheduledJobModel ScheduledJobViewToScheduledJobModel(ScheduledJobView vw);

        public partial ScheduledJob ScheduledJobCreateModelToScheduledJob(ScheduledJobCreateModel model);

        public partial ScheduledJob ScheduledJobUpdateModelToScheduledJob(ScheduledJobUpdateModel model);

                //public partial List<ScheduledJobExportModel> ScheduledJobModelListToScheduledJobExportModelList(List<ScheduledJobModel> lst);
        public partial List<ScheduledJobHistoryModel> ScheduledJobHistoryViewListToScheduledJobHistoryModelList(List<ScheduledJobHistoryView> lst);

        public partial ScheduledJobHistoryModel ScheduledJobHistoryViewToScheduledJobHistoryModel(ScheduledJobHistoryView vw);

        public partial ScheduledJobHistory ScheduledJobHistoryCreateModelToScheduledJobHistory(ScheduledJobHistoryCreateModel model);

        public partial ScheduledJobHistory ScheduledJobHistoryUpdateModelToScheduledJobHistory(ScheduledJobHistoryUpdateModel model);

                //public partial List<ScheduledJobHistoryExportModel> ScheduledJobHistoryModelListToScheduledJobHistoryExportModelList(List<ScheduledJobHistoryModel> lst);
        public partial List<ScheduledJobNotificationEmailModel> ScheduledJobNotificationEmailViewListToScheduledJobNotificationEmailModelList(List<ScheduledJobNotificationEmailView> lst);

        public partial ScheduledJobNotificationEmailModel ScheduledJobNotificationEmailViewToScheduledJobNotificationEmailModel(ScheduledJobNotificationEmailView vw);

        public partial ScheduledJobNotificationEmail ScheduledJobNotificationEmailCreateModelToScheduledJobNotificationEmail(ScheduledJobNotificationEmailCreateModel model);

        public partial ScheduledJobNotificationEmail ScheduledJobNotificationEmailUpdateModelToScheduledJobNotificationEmail(ScheduledJobNotificationEmailUpdateModel model);

        //public partial List<ScheduledJobNotificationEmailExportModel> ScheduledJobNotificationEmailModelListToScheduledJobNotificationEmailExportModelList(List<ScheduledJobNotificationEmailModel> lst);
        public partial List<SingleSignOnServerModel> SingleSignOnServerViewListToSingleSignOnServerModelList(List<SingleSignOnServerView> lst);

        public partial SingleSignOnServerModel SingleSignOnServerViewToSingleSignOnServerModel(SingleSignOnServerView vw);

        [MapperRequiredMapping(RequiredMappingStrategy.None)]
        public partial SingleSignOnServerModel SingleSignOnServerCreateModelToSingleSignOnServerModel(SingleSignOnServerCreateModel model);

        [MapperRequiredMapping(RequiredMappingStrategy.None)]
        public partial SingleSignOnServerModel SingleSignOnServerUpdateModelToSingleSignOnServerModel(SingleSignOnServerUpdateModel model);


        public partial SingleSignOnServer SingleSignOnServerCreateModelToSingleSignOnServer(SingleSignOnServerCreateModel model);

        public partial SingleSignOnServer SingleSignOnServerUpdateModelToSingleSignOnServer(SingleSignOnServerUpdateModel model);


        //public partial List<SingleSignOnServerExportModel> SingleSignOnServerModelListToSingleSignOnServerExportModelList(List<SingleSignOnServerModel> lst);

        [MapperRequiredMapping(RequiredMappingStrategy.None)]
        public partial SmsServerModel SmsServerToSmsServerModel(SmsServer model);

        public partial List<SmsServerModel> SmsServerViewListToSmsServerModelList(List<SmsServerView> lst);

        public partial SmsServerModel SmsServerViewToSmsServerModel(SmsServerView vw);

        public partial SmsServer SmsServerCreateModelToSmsServer(SmsServerCreateModel model);

        public partial SmsServer SmsServerUpdateModelToSmsServer(SmsServerUpdateModel model);

                //public partial List<SmsServerExportModel> SmsServerModelListToSmsServerExportModelList(List<SmsServerModel> lst);
        public partial List<StateModel> StateViewListToStateModelList(List<StateView> lst);

        public partial StateModel StateViewToStateModel(StateView vw);

        public partial State StateCreateModelToState(StateCreateModel model);

        public partial State StateUpdateModelToState(StateUpdateModel model);

        //public partial List<StateExportModel> StateModelListToStateExportModelList(List<StateModel> lst);
        public partial List<UserExportModel> UserModelListToUserExportModelList(List<UserModel> lst);

        public partial UserExportModel UserModelToUserExportModel(UserModel model);

        [MapperRequiredMapping(RequiredMappingStrategy.None)]
        public partial UserUpdateModel UserCreateModelToUserUpdateModel(UserCreateModel model);

        [MapperRequiredMapping(RequiredMappingStrategy.None)]
        public partial UserProfileModel UserViewToUserProfileModel(UserView model);

        public partial List<UserModel> UserViewListToUserModelList(List<UserView> lst);

        [MapperRequiredMapping(RequiredMappingStrategy.None)]
        public partial UserModel UserViewToUserModel(UserView vw);

        public partial User UserCreateModelToUser(UserCreateModel model);

        public partial User UserUpdateModelToUser(UserUpdateModel model);

        [MapperRequiredMapping(RequiredMappingStrategy.None)]
        public partial UserModel UserToUserModel(User usr);

        [MapperRequiredMapping(RequiredMappingStrategy.None)]
        public partial UserInfoImportExportModel UserViewToUserInfoImportExportModel(UserView model);

        public partial List<UserInfoImportExportModel> UserViewListToUserInfoImportExportModelList(List<UserView> lst);

        //public partial List<UserExportModel> UserModelListToUserExportModelList(List<UserModel> lst);
        public partial List<UserPermissionModel> UserPermissionViewListToUserPermissionModelList(List<UserPermissionView> lst);

        public partial UserPermissionModel UserPermissionViewToUserPermissionModel(UserPermissionView vw);

        public partial UserPermission UserPermissionCreateModelToUserPermission(UserPermissionCreateModel model);

        public partial UserPermission UserPermissionUpdateModelToUserPermission(UserPermissionUpdateModel model);

                //public partial List<UserPermissionExportModel> UserPermissionModelListToUserPermissionExportModelList(List<UserPermissionModel> lst);
        public partial List<VoiceNotificationServerModel> VoiceNotificationServerViewListToVoiceNotificationServerModelList(List<VoiceNotificationServerView> lst);

        public partial VoiceNotificationServerModel VoiceNotificationServerViewToVoiceNotificationServerModel(VoiceNotificationServerView vw);

        public partial VoiceNotificationServer VoiceNotificationServerCreateModelToVoiceNotificationServer(VoiceNotificationServerCreateModel model);

        public partial VoiceNotificationServer VoiceNotificationServerUpdateModelToVoiceNotificationServer(VoiceNotificationServerUpdateModel model);

                //public partial List<VoiceNotificationServerExportModel> VoiceNotificationServerModelListToVoiceNotificationServerExportModelList(List<VoiceNotificationServerModel> lst);
        public partial List<WhatsAppNotificationServerModel> WhatsAppNotificationServerViewListToWhatsAppNotificationServerModelList(List<WhatsAppNotificationServerView> lst);

        public partial WhatsAppNotificationServerModel WhatsAppNotificationServerViewToWhatsAppNotificationServerModel(WhatsAppNotificationServerView vw);

        public partial WhatsAppNotificationServer WhatsAppNotificationServerCreateModelToWhatsAppNotificationServer(WhatsAppNotificationServerCreateModel model);

        public partial WhatsAppNotificationServer WhatsAppNotificationServerUpdateModelToWhatsAppNotificationServer(WhatsAppNotificationServerUpdateModel model);

        //public partial List<WhatsAppNotificationServerExportModel> WhatsAppNotificationServerModelListToWhatsAppNotificationServerExportModelList(List<WhatsAppNotificationServerModel> lst);

        public partial List<UserGroupModel> UserGroupViewListToUserGroupModelList(List<UserGroupView> lst);
        public partial List<UserGroupModel> UserGroupListToUserGroupModelList(List<UserGroup> lst);
        public partial UserGroupModel UserGroupViewToUserGroupModel(UserGroupView vw);
        [MapperRequiredMapping(RequiredMappingStrategy.None)]
        public partial UserGroupModel UserGroupToUserGroupModel(UserGroup model);
        public partial UserGroup UserGroupCreateModelToUserGroup(UserGroupCreateModel model);
        public partial UserGroup UserGroupUpdateModelToUserGroup(UserGroupUpdateModel model);
    }
}
