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
        
        public partial List<VisionBoardSessionPictureModel> VisionBoardSessionPictureViewListToVisionBoardSessionPictureModelList(List<VisionBoardSessionPictureView> lst);
public partial List<VisionBoardSessionPictureModel> VisionBoardSessionPictureListToVisionBoardSessionPictureModelList(List<VisionBoardSessionPicture> lst);
public partial VisionBoardSessionPictureModel VisionBoardSessionPictureViewToVisionBoardSessionPictureModel(VisionBoardSessionPictureView vw);
[MapperRequiredMapping(RequiredMappingStrategy.None)]
public partial VisionBoardSessionPictureModel VisionBoardSessionPictureToVisionBoardSessionPictureModel(VisionBoardSessionPicture model);
public partial VisionBoardSessionPicture VisionBoardSessionPictureCreateModelToVisionBoardSessionPicture(VisionBoardSessionPictureCreateModel model);
public partial VisionBoardSessionPicture VisionBoardSessionPictureUpdateModelToVisionBoardSessionPicture(VisionBoardSessionPictureUpdateModel model);

//public partial List<VisionBoardSessionPictureExportModel> VisionBoardSessionPictureModelListToVisionBoardSessionPictureExportModelList(List<VisionBoardSessionPictureModel> lst);

public partial List<CategoryModel> CategoryViewListToCategoryModelList(List<CategoryView> lst);
public partial List<CategoryModel> CategoryListToCategoryModelList(List<Category> lst);
public partial CategoryModel CategoryViewToCategoryModel(CategoryView vw);
[MapperRequiredMapping(RequiredMappingStrategy.None)]
public partial CategoryModel CategoryToCategoryModel(Category model);
public partial Category CategoryCreateModelToCategory(CategoryCreateModel model);
public partial Category CategoryUpdateModelToCategory(CategoryUpdateModel model);

//public partial List<CategoryExportModel> CategoryModelListToCategoryExportModelList(List<CategoryModel> lst);

public partial List<DailyAffirmationSessionModel> DailyAffirmationSessionViewListToDailyAffirmationSessionModelList(List<DailyAffirmationSessionView> lst);
public partial List<DailyAffirmationSessionModel> DailyAffirmationSessionListToDailyAffirmationSessionModelList(List<DailyAffirmationSession> lst);
public partial DailyAffirmationSessionModel DailyAffirmationSessionViewToDailyAffirmationSessionModel(DailyAffirmationSessionView vw);
[MapperRequiredMapping(RequiredMappingStrategy.None)]
public partial DailyAffirmationSessionModel DailyAffirmationSessionToDailyAffirmationSessionModel(DailyAffirmationSession model);
public partial DailyAffirmationSession DailyAffirmationSessionCreateModelToDailyAffirmationSession(DailyAffirmationSessionCreateModel model);
public partial DailyAffirmationSession DailyAffirmationSessionUpdateModelToDailyAffirmationSession(DailyAffirmationSessionUpdateModel model);

//public partial List<DailyAffirmationSessionExportModel> DailyAffirmationSessionModelListToDailyAffirmationSessionExportModelList(List<DailyAffirmationSessionModel> lst);

public partial List<DailyAffirmationSessionCategoryModel> DailyAffirmationSessionCategoryViewListToDailyAffirmationSessionCategoryModelList(List<DailyAffirmationSessionCategoryView> lst);
public partial List<DailyAffirmationSessionCategoryModel> DailyAffirmationSessionCategoryListToDailyAffirmationSessionCategoryModelList(List<DailyAffirmationSessionCategory> lst);
public partial DailyAffirmationSessionCategoryModel DailyAffirmationSessionCategoryViewToDailyAffirmationSessionCategoryModel(DailyAffirmationSessionCategoryView vw);
[MapperRequiredMapping(RequiredMappingStrategy.None)]
public partial DailyAffirmationSessionCategoryModel DailyAffirmationSessionCategoryToDailyAffirmationSessionCategoryModel(DailyAffirmationSessionCategory model);
public partial DailyAffirmationSessionCategory DailyAffirmationSessionCategoryCreateModelToDailyAffirmationSessionCategory(DailyAffirmationSessionCategoryCreateModel model);
public partial DailyAffirmationSessionCategory DailyAffirmationSessionCategoryUpdateModelToDailyAffirmationSessionCategory(DailyAffirmationSessionCategoryUpdateModel model);

//public partial List<DailyAffirmationSessionCategoryExportModel> DailyAffirmationSessionCategoryModelListToDailyAffirmationSessionCategoryExportModelList(List<DailyAffirmationSessionCategoryModel> lst);

public partial List<DailyAffirmationTextModel> DailyAffirmationTextViewListToDailyAffirmationTextModelList(List<DailyAffirmationTextView> lst);
public partial List<DailyAffirmationTextModel> DailyAffirmationTextListToDailyAffirmationTextModelList(List<DailyAffirmationText> lst);
public partial DailyAffirmationTextModel DailyAffirmationTextViewToDailyAffirmationTextModel(DailyAffirmationTextView vw);
[MapperRequiredMapping(RequiredMappingStrategy.None)]
public partial DailyAffirmationTextModel DailyAffirmationTextToDailyAffirmationTextModel(DailyAffirmationText model);
public partial DailyAffirmationText DailyAffirmationTextCreateModelToDailyAffirmationText(DailyAffirmationTextCreateModel model);
public partial DailyAffirmationText DailyAffirmationTextUpdateModelToDailyAffirmationText(DailyAffirmationTextUpdateModel model);

//public partial List<DailyAffirmationTextExportModel> DailyAffirmationTextModelListToDailyAffirmationTextExportModelList(List<DailyAffirmationTextModel> lst);

public partial List<DefaultCategoryModel> DefaultCategoryViewListToDefaultCategoryModelList(List<DefaultCategoryView> lst);
public partial List<DefaultCategoryModel> DefaultCategoryListToDefaultCategoryModelList(List<DefaultCategory> lst);
public partial DefaultCategoryModel DefaultCategoryViewToDefaultCategoryModel(DefaultCategoryView vw);
[MapperRequiredMapping(RequiredMappingStrategy.None)]
public partial DefaultCategoryModel DefaultCategoryToDefaultCategoryModel(DefaultCategory model);
public partial DefaultCategory DefaultCategoryCreateModelToDefaultCategory(DefaultCategoryCreateModel model);
public partial DefaultCategory DefaultCategoryUpdateModelToDefaultCategory(DefaultCategoryUpdateModel model);

//public partial List<DefaultCategoryExportModel> DefaultCategoryModelListToDefaultCategoryExportModelList(List<DefaultCategoryModel> lst);

public partial List<DefaultDailyAffirmationTextModel> DefaultDailyAffirmationTextViewListToDefaultDailyAffirmationTextModelList(List<DefaultDailyAffirmationTextView> lst);
public partial List<DefaultDailyAffirmationTextModel> DefaultDailyAffirmationTextListToDefaultDailyAffirmationTextModelList(List<DefaultDailyAffirmationText> lst);
public partial DefaultDailyAffirmationTextModel DefaultDailyAffirmationTextViewToDefaultDailyAffirmationTextModel(DefaultDailyAffirmationTextView vw);
[MapperRequiredMapping(RequiredMappingStrategy.None)]
public partial DefaultDailyAffirmationTextModel DefaultDailyAffirmationTextToDefaultDailyAffirmationTextModel(DefaultDailyAffirmationText model);
public partial DefaultDailyAffirmationText DefaultDailyAffirmationTextCreateModelToDefaultDailyAffirmationText(DefaultDailyAffirmationTextCreateModel model);
public partial DefaultDailyAffirmationText DefaultDailyAffirmationTextUpdateModelToDefaultDailyAffirmationText(DefaultDailyAffirmationTextUpdateModel model);

//public partial List<DefaultDailyAffirmationTextExportModel> DefaultDailyAffirmationTextModelListToDefaultDailyAffirmationTextExportModelList(List<DefaultDailyAffirmationTextModel> lst);

public partial List<DefaultGoalTrackerModel> DefaultGoalTrackerViewListToDefaultGoalTrackerModelList(List<DefaultGoalTrackerView> lst);
public partial List<DefaultGoalTrackerModel> DefaultGoalTrackerListToDefaultGoalTrackerModelList(List<DefaultGoalTracker> lst);
public partial DefaultGoalTrackerModel DefaultGoalTrackerViewToDefaultGoalTrackerModel(DefaultGoalTrackerView vw);
[MapperRequiredMapping(RequiredMappingStrategy.None)]
public partial DefaultGoalTrackerModel DefaultGoalTrackerToDefaultGoalTrackerModel(DefaultGoalTracker model);
public partial DefaultGoalTracker DefaultGoalTrackerCreateModelToDefaultGoalTracker(DefaultGoalTrackerCreateModel model);
public partial DefaultGoalTracker DefaultGoalTrackerUpdateModelToDefaultGoalTracker(DefaultGoalTrackerUpdateModel model);

//public partial List<DefaultGoalTrackerExportModel> DefaultGoalTrackerModelListToDefaultGoalTrackerExportModelList(List<DefaultGoalTrackerModel> lst);

public partial List<DefaultGoalTrackerCategoryModel> DefaultGoalTrackerCategoryViewListToDefaultGoalTrackerCategoryModelList(List<DefaultGoalTrackerCategoryView> lst);
public partial List<DefaultGoalTrackerCategoryModel> DefaultGoalTrackerCategoryListToDefaultGoalTrackerCategoryModelList(List<DefaultGoalTrackerCategory> lst);
public partial DefaultGoalTrackerCategoryModel DefaultGoalTrackerCategoryViewToDefaultGoalTrackerCategoryModel(DefaultGoalTrackerCategoryView vw);
[MapperRequiredMapping(RequiredMappingStrategy.None)]
public partial DefaultGoalTrackerCategoryModel DefaultGoalTrackerCategoryToDefaultGoalTrackerCategoryModel(DefaultGoalTrackerCategory model);
public partial DefaultGoalTrackerCategory DefaultGoalTrackerCategoryCreateModelToDefaultGoalTrackerCategory(DefaultGoalTrackerCategoryCreateModel model);
public partial DefaultGoalTrackerCategory DefaultGoalTrackerCategoryUpdateModelToDefaultGoalTrackerCategory(DefaultGoalTrackerCategoryUpdateModel model);

//public partial List<DefaultGoalTrackerCategoryExportModel> DefaultGoalTrackerCategoryModelListToDefaultGoalTrackerCategoryExportModelList(List<DefaultGoalTrackerCategoryModel> lst);

public partial List<DefaultMeditationFileModel> DefaultMeditationFileViewListToDefaultMeditationFileModelList(List<DefaultMeditationFileView> lst);
public partial List<DefaultMeditationFileModel> DefaultMeditationFileListToDefaultMeditationFileModelList(List<DefaultMeditationFile> lst);
public partial DefaultMeditationFileModel DefaultMeditationFileViewToDefaultMeditationFileModel(DefaultMeditationFileView vw);
[MapperRequiredMapping(RequiredMappingStrategy.None)]
public partial DefaultMeditationFileModel DefaultMeditationFileToDefaultMeditationFileModel(DefaultMeditationFile model);
public partial DefaultMeditationFile DefaultMeditationFileCreateModelToDefaultMeditationFile(DefaultMeditationFileCreateModel model);
public partial DefaultMeditationFile DefaultMeditationFileUpdateModelToDefaultMeditationFile(DefaultMeditationFileUpdateModel model);

//public partial List<DefaultMeditationFileExportModel> DefaultMeditationFileModelListToDefaultMeditationFileExportModelList(List<DefaultMeditationFileModel> lst);

public partial List<DefaultVisionBoardPictureModel> DefaultVisionBoardPictureViewListToDefaultVisionBoardPictureModelList(List<DefaultVisionBoardPictureView> lst);
public partial List<DefaultVisionBoardPictureModel> DefaultVisionBoardPictureListToDefaultVisionBoardPictureModelList(List<DefaultVisionBoardPicture> lst);
public partial DefaultVisionBoardPictureModel DefaultVisionBoardPictureViewToDefaultVisionBoardPictureModel(DefaultVisionBoardPictureView vw);
[MapperRequiredMapping(RequiredMappingStrategy.None)]
public partial DefaultVisionBoardPictureModel DefaultVisionBoardPictureToDefaultVisionBoardPictureModel(DefaultVisionBoardPicture model);
public partial DefaultVisionBoardPicture DefaultVisionBoardPictureCreateModelToDefaultVisionBoardPicture(DefaultVisionBoardPictureCreateModel model);
public partial DefaultVisionBoardPicture DefaultVisionBoardPictureUpdateModelToDefaultVisionBoardPicture(DefaultVisionBoardPictureUpdateModel model);

//public partial List<DefaultVisionBoardPictureExportModel> DefaultVisionBoardPictureModelListToDefaultVisionBoardPictureExportModelList(List<DefaultVisionBoardPictureModel> lst);

public partial List<FeedModel> FeedViewListToFeedModelList(List<FeedView> lst);
public partial List<FeedModel> FeedListToFeedModelList(List<Feed> lst);
public partial FeedModel FeedViewToFeedModel(FeedView vw);
[MapperRequiredMapping(RequiredMappingStrategy.None)]
public partial FeedModel FeedToFeedModel(Feed model);
public partial Feed FeedCreateModelToFeed(FeedCreateModel model);
public partial Feed FeedUpdateModelToFeed(FeedUpdateModel model);

//public partial List<FeedExportModel> FeedModelListToFeedExportModelList(List<FeedModel> lst);

public partial List<GoalTrackerModel> GoalTrackerViewListToGoalTrackerModelList(List<GoalTrackerView> lst);
public partial List<GoalTrackerModel> GoalTrackerListToGoalTrackerModelList(List<GoalTracker> lst);
public partial GoalTrackerModel GoalTrackerViewToGoalTrackerModel(GoalTrackerView vw);
[MapperRequiredMapping(RequiredMappingStrategy.None)]
public partial GoalTrackerModel GoalTrackerToGoalTrackerModel(GoalTracker model);
public partial GoalTracker GoalTrackerCreateModelToGoalTracker(GoalTrackerCreateModel model);
public partial GoalTracker GoalTrackerUpdateModelToGoalTracker(GoalTrackerUpdateModel model);

//public partial List<GoalTrackerExportModel> GoalTrackerModelListToGoalTrackerExportModelList(List<GoalTrackerModel> lst);

public partial List<GoalTrackerCategoryModel> GoalTrackerCategoryViewListToGoalTrackerCategoryModelList(List<GoalTrackerCategoryView> lst);
public partial List<GoalTrackerCategoryModel> GoalTrackerCategoryListToGoalTrackerCategoryModelList(List<GoalTrackerCategory> lst);
public partial GoalTrackerCategoryModel GoalTrackerCategoryViewToGoalTrackerCategoryModel(GoalTrackerCategoryView vw);
[MapperRequiredMapping(RequiredMappingStrategy.None)]
public partial GoalTrackerCategoryModel GoalTrackerCategoryToGoalTrackerCategoryModel(GoalTrackerCategory model);
public partial GoalTrackerCategory GoalTrackerCategoryCreateModelToGoalTrackerCategory(GoalTrackerCategoryCreateModel model);
public partial GoalTrackerCategory GoalTrackerCategoryUpdateModelToGoalTrackerCategory(GoalTrackerCategoryUpdateModel model);

//public partial List<GoalTrackerCategoryExportModel> GoalTrackerCategoryModelListToGoalTrackerCategoryExportModelList(List<GoalTrackerCategoryModel> lst);

public partial List<GoalTrackerMileStoneModel> GoalTrackerMileStoneViewListToGoalTrackerMileStoneModelList(List<GoalTrackerMileStoneView> lst);
public partial List<GoalTrackerMileStoneModel> GoalTrackerMileStoneListToGoalTrackerMileStoneModelList(List<GoalTrackerMileStone> lst);
public partial GoalTrackerMileStoneModel GoalTrackerMileStoneViewToGoalTrackerMileStoneModel(GoalTrackerMileStoneView vw);
[MapperRequiredMapping(RequiredMappingStrategy.None)]
public partial GoalTrackerMileStoneModel GoalTrackerMileStoneToGoalTrackerMileStoneModel(GoalTrackerMileStone model);
public partial GoalTrackerMileStone GoalTrackerMileStoneCreateModelToGoalTrackerMileStone(GoalTrackerMileStoneCreateModel model);
public partial GoalTrackerMileStone GoalTrackerMileStoneUpdateModelToGoalTrackerMileStone(GoalTrackerMileStoneUpdateModel model);

//public partial List<GoalTrackerMileStoneExportModel> GoalTrackerMileStoneModelListToGoalTrackerMileStoneExportModelList(List<GoalTrackerMileStoneModel> lst);

public partial List<MeditationFileModel> MeditationFileViewListToMeditationFileModelList(List<MeditationFileView> lst);
public partial List<MeditationFileModel> MeditationFileListToMeditationFileModelList(List<MeditationFile> lst);
public partial MeditationFileModel MeditationFileViewToMeditationFileModel(MeditationFileView vw);
[MapperRequiredMapping(RequiredMappingStrategy.None)]
public partial MeditationFileModel MeditationFileToMeditationFileModel(MeditationFile model);
public partial MeditationFile MeditationFileCreateModelToMeditationFile(MeditationFileCreateModel model);
public partial MeditationFile MeditationFileUpdateModelToMeditationFile(MeditationFileUpdateModel model);

//public partial List<MeditationFileExportModel> MeditationFileModelListToMeditationFileExportModelList(List<MeditationFileModel> lst);

public partial List<MeditationSessionModel> MeditationSessionViewListToMeditationSessionModelList(List<MeditationSessionView> lst);
public partial List<MeditationSessionModel> MeditationSessionListToMeditationSessionModelList(List<MeditationSession> lst);
public partial MeditationSessionModel MeditationSessionViewToMeditationSessionModel(MeditationSessionView vw);
[MapperRequiredMapping(RequiredMappingStrategy.None)]
public partial MeditationSessionModel MeditationSessionToMeditationSessionModel(MeditationSession model);
public partial MeditationSession MeditationSessionCreateModelToMeditationSession(MeditationSessionCreateModel model);
public partial MeditationSession MeditationSessionUpdateModelToMeditationSession(MeditationSessionUpdateModel model);

//public partial List<MeditationSessionExportModel> MeditationSessionModelListToMeditationSessionExportModelList(List<MeditationSessionModel> lst);

public partial List<VisionBoardPictureModel> VisionBoardPictureViewListToVisionBoardPictureModelList(List<VisionBoardPictureView> lst);
public partial List<VisionBoardPictureModel> VisionBoardPictureListToVisionBoardPictureModelList(List<VisionBoardPicture> lst);
public partial VisionBoardPictureModel VisionBoardPictureViewToVisionBoardPictureModel(VisionBoardPictureView vw);
[MapperRequiredMapping(RequiredMappingStrategy.None)]
public partial VisionBoardPictureModel VisionBoardPictureToVisionBoardPictureModel(VisionBoardPicture model);
public partial VisionBoardPicture VisionBoardPictureCreateModelToVisionBoardPicture(VisionBoardPictureCreateModel model);
public partial VisionBoardPicture VisionBoardPictureUpdateModelToVisionBoardPicture(VisionBoardPictureUpdateModel model);

//public partial List<VisionBoardPictureExportModel> VisionBoardPictureModelListToVisionBoardPictureExportModelList(List<VisionBoardPictureModel> lst);

public partial List<VisionBoardSessionModel> VisionBoardSessionViewListToVisionBoardSessionModelList(List<VisionBoardSessionView> lst);
public partial List<VisionBoardSessionModel> VisionBoardSessionListToVisionBoardSessionModelList(List<VisionBoardSession> lst);
public partial VisionBoardSessionModel VisionBoardSessionViewToVisionBoardSessionModel(VisionBoardSessionView vw);
[MapperRequiredMapping(RequiredMappingStrategy.None)]
public partial VisionBoardSessionModel VisionBoardSessionToVisionBoardSessionModel(VisionBoardSession model);
public partial VisionBoardSession VisionBoardSessionCreateModelToVisionBoardSession(VisionBoardSessionCreateModel model);
public partial VisionBoardSession VisionBoardSessionUpdateModelToVisionBoardSession(VisionBoardSessionUpdateModel model);

//public partial List<VisionBoardSessionExportModel> VisionBoardSessionModelListToVisionBoardSessionExportModelList(List<VisionBoardSessionModel> lst);


    }
}
