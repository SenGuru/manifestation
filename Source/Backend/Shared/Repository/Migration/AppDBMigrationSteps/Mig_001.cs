#pragma warning disable SA1310
#pragma warning disable SA1516
#pragma warning disable SA1028 // Code should not contain trailing whitespace
#pragma warning disable SA1402
#pragma warning disable SA1124
#pragma warning disable SA1507
#pragma warning disable S1481
#pragma warning disable CS0168
#pragma warning disable SA1505
#pragma warning disable SA1649
#pragma warning disable SA1601
using FluentMigrator;

namespace Manifest.Repository.Migration.Application
{
    [Migration(001)]
#pragma warning disable S101 // Types should be named in PascalCase
    public class Mig_001 : BaseMigration
#pragma warning restore S101 // Types should be named in PascalCase
    {
        public override void Up()
        {
            //Table definitions
            Create.Table("AppExecutionVersion")
                .WithColumn("AppExecutionVersionId").AsInt64().PrimaryKey("PK_AppExecutionVersion").Identity().NotNullable()
                .WithColumn("Version").AsInt64().NotNullable()
                .WithColumn("RunDate").AsInt64().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "AppExecutionVersion", "AppExecutionVersionId", 1000);
            Create.Table("AuditOperation")
                .WithColumn("AuditOperationId").AsInt64().PrimaryKey("PK_AuditOperation").Identity().NotNullable()
                .WithColumn("AuditDateTime").AsDateTime().NotNullable()
                .WithColumn("PredefinedAuditOperationTypeId").AsInt64().NotNullable()
                .WithColumn("ChangeDiff").AsString(2147483647).Nullable()
                .WithColumn("UserId").AsInt64().Nullable()
                .WithColumn("UserGroupId").AsInt64().Nullable()
                .WithColumn("UserGroupUserId").AsInt64().Nullable()
                .WithColumn("UserPermissionId").AsInt64().Nullable()
                .WithColumn("PredefinedResourceTypeId").AsInt64().Nullable()
                .WithColumn("ResourceId").AsInt64().Nullable()
                .WithColumn("IpAddress").AsString(100).Nullable()
                .WithColumn("BrowserName").AsString(200).Nullable()
                .WithColumn("OperatingSystemName").AsString(200).Nullable()
                .WithColumn("DeviceName").AsString(200).Nullable()
                .WithColumn("Description").AsString(500).Nullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "AuditOperation", "AuditOperationId", 1000);
            Create.Table("AuditOperationDetail")
                .WithColumn("AuditOperationDetailId").AsInt64().PrimaryKey("PK_AuditOperationDetail").Identity().NotNullable()
                .WithColumn("AuditOperationId").AsInt64().NotNullable()
                .WithColumn("DifferenceJson").AsString(2147483647).Nullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "AuditOperationDetail", "AuditOperationDetailId", 1000);
            Create.Table("Category")
                .WithColumn("CategoryId").AsInt64().PrimaryKey("PK_Category").Identity().NotNullable()
                .WithColumn("DefaultCategoryId").AsInt64().NotNullable()
                .WithColumn("CategoryName").AsString(255).NotNullable()
                .WithColumn("PredefinedCategoryTypeId").AsInt64().Nullable()
                .WithColumn("UserId").AsInt64().NotNullable()
                .WithColumn("ColorAsInteger").AsInt64().NotNullable()
                .WithColumn("PredefinedIconTypeId").AsInt64().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "Category", "CategoryId", 1000);
            Create.Table("Chat")
                .WithColumn("ChatId").AsInt64().PrimaryKey("PK_Chat").Identity().NotNullable()
                .WithColumn("ChatName").AsString(500).NotNullable()
                .WithColumn("UserId").AsInt64().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "Chat", "ChatId", 1000);
            Create.Table("ChatMessage")
                .WithColumn("ChatMessageId").AsInt64().PrimaryKey("PK_ChatMessage").Identity().NotNullable()
                .WithColumn("ChatId").AsInt64().NotNullable()
                .WithColumn("UserId").AsInt64().NotNullable()
                .WithColumn("ChatMessageText").AsString(2000).NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "ChatMessage", "ChatMessageId", 1000);
            Create.Table("ChatMessageStatus")
                .WithColumn("ChatMessageStatusId").AsInt64().PrimaryKey("PK_ChatMessageStatusId").Identity().NotNullable()
                .WithColumn("ChatMessageId").AsInt64().NotNullable()
                .WithColumn("UserId").AsInt64().NotNullable()
                .WithColumn("ChatMessageViewedDate").AsDateTime().Nullable()
                .WithColumn("ChatMessageSentDate").AsDateTime().Nullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "ChatMessageStatus", "ChatMessageStatusId", 1000);
            Create.Table("ChatUser")
                .WithColumn("ChatUserId").AsInt64().PrimaryKey("PK_ChatUser").Identity().NotNullable()
                .WithColumn("ChatId").AsInt64().NotNullable()
                .WithColumn("UserId").AsInt64().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "ChatUser", "ChatUserId", 1000);
            Create.Table("CompanyStorageConfiguration")
                .WithColumn("CompanyStorageConfigurationId").AsInt64().PrimaryKey("PK_CompanyStorageConfiguration").Identity().NotNullable()
                .WithColumn("CompanyStorageConfigurationGuid").AsGuid().NotNullable()
                .WithColumn("ProfilePictureFileStorageConnectionString").AsString(500).Nullable()
                .WithColumn("ReportServerFileStorageConnectionString").AsString(500).Nullable()
                .WithColumn("FileAttachmentFileStorageConnectionString").AsString(500).Nullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "CompanyStorageConfiguration", "CompanyStorageConfigurationId", 1000);
            Create.Table("ConfigurationGroup")
                .WithColumn("ConfigurationGroupId").AsInt64().PrimaryKey("PK_ConfigurationGroup").NotNullable()
                .WithColumn("GroupName").AsString(200).NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            Create.Table("ConfigurationItem")
                .WithColumn("ConfigurationItemId").AsInt64().PrimaryKey("PK_ConfigurationItem").NotNullable()
                .WithColumn("ConfigurationGroupId").AsInt64().NotNullable()
                .WithColumn("Key").AsString(200).NotNullable()
                .WithColumn("Value").AsString(2000).NotNullable()
                .WithColumn("DefaultValue").AsString(2000).Nullable()
                .WithColumn("Required").AsBoolean().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            Create.Table("Country")
                .WithColumn("CountryId").AsInt64().PrimaryKey("PK_Country").Identity().NotNullable()
                .WithColumn("CountryName").AsString(255).NotNullable()
                .WithColumn("AlternateCountryName1").AsString(255).Nullable()
                .WithColumn("AlternateCountryName2").AsString(255).Nullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "Country", "CountryId", 1000);
            Create.Table("DailyAffirmationSession")
                .WithColumn("DailyAffirmationSessionId").AsInt64().PrimaryKey("PK_DailyAffirmationSession").Identity().NotNullable()
                .WithColumn("DailyAffirmationSessionDate").AsDateTime().NotNullable()
                .WithColumn("DailyAffirmationSessionTimeSpentInSeconds").AsInt64().Nullable()
                .WithColumn("UserId").AsInt64().NotNullable()
                .WithColumn("AddedToFeed").AsBoolean().Nullable()
                .WithColumn("AddedToFeedDate").AsDateTime().Nullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "DailyAffirmationSession", "DailyAffirmationSessionId", 1000);
            Create.Table("DailyAffirmationSessionCategory")
                .WithColumn("DailyAffirmationSessionCategoryId").AsInt64().PrimaryKey("PK_DailyAffirmationSessionCategory").Identity().NotNullable()
                .WithColumn("CategoryId").AsInt64().NotNullable()
                .WithColumn("DailyAffirmationSessionId").AsInt64().NotNullable()
                .WithColumn("DailyAffirmationSessionCategoryTimeSpentInSeconds").AsInt64().Nullable()
                .WithColumn("UserId").AsInt64().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "DailyAffirmationSessionCategory", "DailyAffirmationSessionCategoryId", 1000);
            Create.Table("DailyAffirmationText")
                .WithColumn("DailyAffirmationTextId").AsInt64().PrimaryKey("PK_DailyAffirmationText").Identity().NotNullable()
                .WithColumn("DailyAffirmationTextValue").AsString(255).NotNullable()
                .WithColumn("CategoryId").AsInt64().NotNullable()
                .WithColumn("UserId").AsInt64().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "DailyAffirmationText", "DailyAffirmationTextId", 1000);
            Create.Table("DailyJournal")
                .WithColumn("DailyJournalId").AsInt64().PrimaryKey("PK_DailyJournal").Identity().NotNullable()
                .WithColumn("DailyJournalDate").AsDateTime().NotNullable()
                .WithColumn("UserId").AsInt64().NotNullable()
                .WithColumn("PredefinedMoodTypeId").AsInt64().NotNullable()
                .WithColumn("Gratitude1").AsString(500).NotNullable()
                .WithColumn("Gratitude2").AsString(500).Nullable()
                .WithColumn("Gratitude3").AsString(500).Nullable()
                .WithColumn("Gratitude4").AsString(500).Nullable()
                .WithColumn("Gratitude5").AsString(500).Nullable()
                .WithColumn("YourReflection").AsString(500).Nullable()
                .WithColumn("TommorowsIntentions").AsString(500).Nullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "DailyJournal", "DailyJournalId", 1000);
            Create.Table("DefaultCategory")
                .WithColumn("DefaultCategoryId").AsInt64().PrimaryKey("PK_DefaultCategory").Identity().NotNullable()
                .WithColumn("DefaultCategoryName").AsString(255).NotNullable()
                .WithColumn("PredefinedCategoryTypeId").AsInt64().Nullable()
                .WithColumn("PredefinedIconTypeId").AsInt64().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "DefaultCategory", "DefaultCategoryId", 1000);
            Create.Table("DefaultDailyAffirmationText")
                .WithColumn("DefaultDailyAffirmationTextId").AsInt64().PrimaryKey("PK_DefaultDailyAffirmationText").Identity().NotNullable()
                .WithColumn("DefaultDailyAffirmationTextName").AsString(255).NotNullable()
                .WithColumn("DefaultCategoryId").AsInt64().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "DefaultDailyAffirmationText", "DefaultDailyAffirmationTextId", 1000);
            Create.Table("DefaultGoalTracker")
                .WithColumn("DefaultGoalTrackerId").AsInt64().PrimaryKey("PK_DefaultGoalTracker").Identity().NotNullable()
                .WithColumn("DefaultGoalTrackerText").AsString(255).NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "DefaultGoalTracker", "DefaultGoalTrackerId", 1000);
            Create.Table("DefaultGoalTrackerCategory")
                .WithColumn("DefaultGoalTrackerCategoryId").AsInt64().PrimaryKey("PK_DefaultGoalTrackerCategory").Identity().NotNullable()
                .WithColumn("DefaultCategoryId").AsInt64().NotNullable()
                .WithColumn("DefaultGoalTrackerId").AsInt64().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "DefaultGoalTrackerCategory", "DefaultGoalTrackerCategoryId", 1000);
            Create.Table("DefaultMeditationFile")
                .WithColumn("DefaultMeditationFileId").AsInt64().PrimaryKey("PK_DefaultMeditationFile").Identity().NotNullable()
                .WithColumn("DefaultMeditationFileName").AsString(255).NotNullable()
                .WithColumn("PredefinedMeditationTypeId").AsInt64().NotNullable()
                .WithColumn("Location").AsString(1024).Nullable()
                .WithColumn("FileName").AsString(255).NotNullable()
                .WithColumn("FileSize").AsInt64().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "DefaultMeditationFile", "DefaultMeditationFileId", 1000);
            Create.Table("DefaultUserTypePermission")
                .WithColumn("DefaultUserTypePermissionId").AsInt64().PrimaryKey("PK_DefaultUserTypePermission").Identity().NotNullable()
                .WithColumn("PredefinedUserTypeId").AsInt64().NotNullable()
                .WithColumn("PredefinedUserPermissionTypeId").AsInt64().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "DefaultUserTypePermission", "DefaultUserTypePermissionId", 1000);
            Create.Table("DefaultVisionBoardPicture")
                .WithColumn("DefaultVisionBoardPictureId").AsInt64().PrimaryKey("PK_DefaultVisionBoardPicture").Identity().NotNullable()
                .WithColumn("Title").AsString(100).Nullable()
                .WithColumn("Location").AsString(1024).Nullable()
                .WithColumn("FileName").AsString(255).NotNullable()
                .WithColumn("FileSize").AsInt64().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "DefaultVisionBoardPicture", "DefaultVisionBoardPictureId", 1000);
            Create.Table("EnumGroup")
                .WithColumn("EnumGroupId").AsInt64().PrimaryKey("PK_EnumGroup").NotNullable()
                .WithColumn("EnumGroupName").AsString(100).NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            Create.Table("EnumType")
                .WithColumn("EnumTypeId").AsInt64().PrimaryKey("PK_EnumType").NotNullable()
                .WithColumn("InternalValue").AsInt64().NotNullable()
                .WithColumn("EnumGroupId").AsInt64().NotNullable()
                .WithColumn("ParentEnumTypeId").AsInt64().Nullable()
                .WithColumn("EnumTypeName").AsString(100).NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            Create.Table("Feed")
                .WithColumn("FeedId").AsInt64().PrimaryKey("PK_Feed").Identity().NotNullable()
                .WithColumn("FeedGuid").AsGuid().NotNullable()
                .WithColumn("UserId").AsInt64().NotNullable()
                .WithColumn("DailyAffirmationSessionId").AsInt64().Nullable()
                .WithColumn("DailyAffirmationSessionDate").AsDateTime().Nullable()
                .WithColumn("DailyAffirmationTimeSpentInSeconds").AsInt64().Nullable()
                .WithColumn("VisionBoardSessionId").AsInt64().Nullable()
                .WithColumn("VisionBoardSessionDate").AsDateTime().Nullable()
                .WithColumn("VisionBoardSessionTimeSpentInSeconds").AsInt64().Nullable()
                .WithColumn("MeditationSessionId").AsInt64().Nullable()
                .WithColumn("MeditationSessionDate").AsDateTime().Nullable()
                .WithColumn("MeditationSessionTimeSpentInSeconds").AsInt64().Nullable()
                .WithColumn("MeditationFileName").AsString(255).Nullable()
                .WithColumn("PredefinedMeditationTypeId").AsInt64().Nullable()
                .WithColumn("GoalTrackerId").AsInt64().Nullable()
                .WithColumn("GoalTrackerName").AsString(255).Nullable()
                .WithColumn("PredefinedGoalStatusTypeId").AsInt64().Nullable()
                .WithColumn("GoalTrackerStartDate").AsDateTime().Nullable()
                .WithColumn("GoalTrackerEndDate").AsDateTime().Nullable()
                .WithColumn("GoalTrackerCompletedDate").AsDateTime().Nullable()
                .WithColumn("GoalTrackerMileStoneId").AsInt64().Nullable()
                .WithColumn("GoalTrackerMileStoneName").AsString(255).Nullable()
                .WithColumn("GoalTrackerMileStoneStartDate").AsDateTime().Nullable()
                .WithColumn("GoalTrackerMileStoneEndDate").AsDateTime().Nullable()
                .WithColumn("GoalTrackerMileStoneCompletedDate").AsDateTime().Nullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "Feed", "FeedId", 1000);
            Create.Table("FileAttachment")
                .WithColumn("FileAttachmentId").AsInt64().PrimaryKey("PK_FileAttachment").Identity().NotNullable()
                .WithColumn("FileAttachmentFolderId").AsInt64().Nullable()
                .WithColumn("FileAttachmentFileName").AsString(128).NotNullable()
                .WithColumn("FileAttachmentFileLocation").AsString(1024).NotNullable()
                .WithColumn("FileAttachmentPreviewFileLocation").AsString(1024).Nullable()
                .WithColumn("FileAttachmentHasPreview").AsBoolean().NotNullable()
                .WithColumn("FileAttachmentFileSizeInBytes").AsInt64().NotNullable()
                .WithColumn("Description").AsString(200).Nullable()
                .WithColumn("UserId").AsInt64().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "FileAttachment", "FileAttachmentId", 1000);
            Create.Table("FileAttachmentFolder")
                .WithColumn("FileAttachmentFolderId").AsInt64().PrimaryKey("PK_FileAttachmentFolder").Identity().NotNullable()
                .WithColumn("FileAttachmentFolderName").AsString(128).NotNullable()
                .WithColumn("ParentFileAttachmentFolderId").AsInt64().Nullable()
                .WithColumn("Description").AsString(200).Nullable()
                .WithColumn("UserId").AsInt64().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "FileAttachmentFolder", "FileAttachmentFolderId", 1000);
            Create.Table("GoalTracker")
                .WithColumn("GoalTrackerId").AsInt64().PrimaryKey("PK_GoalTracker").Identity().NotNullable()
                .WithColumn("GoalTrackerName").AsString(255).NotNullable()
                .WithColumn("PredefinedGoalStatusTypeId").AsInt64().NotNullable()
                .WithColumn("PredefinedGoalPriorityTypeId").AsInt64().NotNullable()
                .WithColumn("GoalTrackerStartDate").AsDateTime().Nullable()
                .WithColumn("GoalTrackerEndDate").AsDateTime().Nullable()
                .WithColumn("GoalTrackerCompletedDate").AsDateTime().Nullable()
                .WithColumn("UserId").AsInt64().NotNullable()
                .WithColumn("AddedToFeed").AsBoolean().Nullable()
                .WithColumn("AddedToFeedDate").AsDateTime().Nullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "GoalTracker", "GoalTrackerId", 1000);
            Create.Table("GoalTrackerCategory")
                .WithColumn("GoalTrackerCategoryId").AsInt64().PrimaryKey("PK_GoalTrackerCategory").Identity().NotNullable()
                .WithColumn("CategoryId").AsInt64().NotNullable()
                .WithColumn("GoalTrackerId").AsInt64().NotNullable()
                .WithColumn("UserId").AsInt64().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "GoalTrackerCategory", "GoalTrackerCategoryId", 1000);
            Create.Table("GoalTrackerMileStone")
                .WithColumn("GoalTrackerMileStoneId").AsInt64().PrimaryKey("PK_GoalTrackerMileStone").Identity().NotNullable()
                .WithColumn("GoalTrackerId").AsInt64().NotNullable()
                .WithColumn("GoalTrackerMileStoneName").AsString(255).NotNullable()
                .WithColumn("GoalTrackerMileStoneStartDate").AsDateTime().Nullable()
                .WithColumn("GoalTrackerMileStoneEndDate").AsDateTime().Nullable()
                .WithColumn("PredefinedGoalStatusTypeId").AsInt64().NotNullable()
                .WithColumn("GoalTrackerMileStoneCompletedDate").AsDateTime().Nullable()
                .WithColumn("UserId").AsInt64().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "GoalTrackerMileStone", "GoalTrackerMileStoneId", 1000);
            Create.Table("Job")
                .WithColumn("JobId").AsInt64().PrimaryKey("PK_Job").Identity().NotNullable()
                .WithColumn("PredefinedJobTypeId").AsInt64().Nullable()
                .WithColumn("JobTemplateId").AsInt64().NotNullable()
                .WithColumn("PredefinedJobOrReportTypeId").AsInt64().Nullable()
                .WithColumn("CompletedDate").AsDateTime().Nullable()
                .WithColumn("SubmittedDate").AsDateTime().NotNullable()
                .WithColumn("Message").AsString(1000).Nullable()
                .WithColumn("PredefinedJobStatusTypeId").AsInt64().NotNullable()
                .WithColumn("SubmittedByUserId").AsInt64().Nullable()
                .WithColumn("ExecutableFolder").AsString(200).NotNullable()
                .WithColumn("ExecutableName").AsString(200).NotNullable()
                .WithColumn("ExecutableParameter").AsString(200).Nullable()
                .WithColumn("SerializedJobParameter").AsString(200).Nullable()
                .WithColumn("ReportTemplateFile").AsString(200).Nullable()
                .WithColumn("ExportDOC").AsBoolean().NotNullable()
                .WithColumn("ExportXLS").AsBoolean().NotNullable()
                .WithColumn("ExportPPT").AsBoolean().NotNullable()
                .WithColumn("ExportCSV").AsBoolean().NotNullable()
                .WithColumn("ExportXML").AsBoolean().NotNullable()
                .WithColumn("ExportPDF").AsBoolean().NotNullable()
                .WithColumn("ExportCustom").AsBoolean().NotNullable()
                .WithColumn("CustomExportName").AsString(20).Nullable()
                .WithColumn("PickupDate").AsDateTime().Nullable()
                .WithColumn("PickupTime").AsInt64().Nullable()
                .WithColumn("JobName").AsString(200).Nullable()
                .WithColumn("IncludeDataSourceFile").AsBoolean().NotNullable()
                .WithColumn("ExecutionAppName").AsString(100).Nullable()
                .WithColumn("ExecutionAppId").AsString(100).Nullable()
                .WithColumn("ExecutionHostName").AsString(100).Nullable()
                .WithColumn("ExecutionAppCheckPort").AsInt64().Nullable()
                .WithColumn("ExecutionProcessId").AsInt64().Nullable()
                .WithColumn("ExecutionThreadId").AsInt64().Nullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "Job", "JobId", 1000);
            Create.Table("JobAttachment")
                .WithColumn("JobAttachmentId").AsInt64().PrimaryKey("PK_JobAttachment").Identity().NotNullable()
                .WithColumn("JobId").AsInt64().NotNullable()
                .WithColumn("FileName").AsString(200).NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
                .WithColumn("StoredFilePath").AsString(1000).NotNullable()
;
            UpdateSequenceStartValue("", "JobAttachment", "JobAttachmentId", 1000);
            Create.Table("JobNotificationEmail")
                .WithColumn("JobNotificationEmailId").AsInt64().PrimaryKey("PK_JobNotificationEmail").Identity().NotNullable()
                .WithColumn("JobId").AsInt64().NotNullable()
                .WithColumn("Email").AsString(200).NotNullable()
                .WithColumn("SendStatus").AsInt64().NotNullable()
                .WithColumn("Message").AsString(200).Nullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "JobNotificationEmail", "JobNotificationEmailId", 1000);
            Create.Table("JobTemplate")
                .WithColumn("JobTemplateId").AsInt64().PrimaryKey("PK_JobTemplate").Identity().NotNullable()
                .WithColumn("PredefinedJobTypeId").AsInt64().NotNullable()
                .WithColumn("Description").AsString(200).NotNullable()
                .WithColumn("PredefinedJobOrReportTypeId").AsInt64().Nullable()
                .WithColumn("PredefinedJobUsageForTypeId").AsInt64().Nullable()
                .WithColumn("JobTemplateCategoryId").AsInt64().NotNullable()
                .WithColumn("ExecutableFolder").AsString(200).NotNullable()
                .WithColumn("ExecutableName").AsString(200).NotNullable()
                .WithColumn("ExecutableParameter").AsString(200).Nullable()
                .WithColumn("CanExportDOC").AsBoolean().NotNullable()
                .WithColumn("CanExportXLS").AsBoolean().NotNullable()
                .WithColumn("CanExportPPT").AsBoolean().NotNullable()
                .WithColumn("CanExportCSV").AsBoolean().NotNullable()
                .WithColumn("CanExportXML").AsBoolean().NotNullable()
                .WithColumn("CanExportPDF").AsBoolean().NotNullable()
                .WithColumn("CanExportCustom").AsBoolean().NotNullable()
                .WithColumn("CustomExportName").AsString(20).Nullable()
                .WithColumn("TemplateName").AsString(50).NotNullable()
                .WithColumn("CanExportDOCDefaultValue").AsBoolean().NotNullable()
                .WithColumn("CanExportXLSDefaultValue").AsBoolean().NotNullable()
                .WithColumn("CanExportPPTDefaultValue").AsBoolean().NotNullable()
                .WithColumn("CanExportCSVDefaultValue").AsBoolean().NotNullable()
                .WithColumn("CanExportXMLDefaultValue").AsBoolean().NotNullable()
                .WithColumn("CanExportPDFDefaultValue").AsBoolean().NotNullable()
                .WithColumn("CanExportCustomDefaultValue").AsBoolean().NotNullable()
                .WithColumn("TaskIdentifierName").AsString(200).NotNullable()
                .WithColumn("ReportTemplateFile").AsString(100).NotNullable()
                .WithColumn("DefaultOutputFileName").AsString(200).NotNullable()
                .WithColumn("IncludeDataSourceFile").AsBoolean().NotNullable()
                .WithColumn("IncludeDataSourceFileDefaultValue").AsBoolean().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "JobTemplate", "JobTemplateId", 1000);
            Create.Table("JobTemplateCategory")
                .WithColumn("JobTemplateCategoryId").AsInt64().PrimaryKey("PK_JobTemplateCategory").Identity().NotNullable()
                .WithColumn("Name").AsString(100).NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "JobTemplateCategory", "JobTemplateCategoryId", 1000);
            Create.Table("JobTemplateParameter")
                .WithColumn("JobTemplateParameterId").AsInt64().PrimaryKey("PK_JobTemplateParameter").Identity().NotNullable()
                .WithColumn("JobTemplateId").AsInt64().NotNullable()
                .WithColumn("Name").AsString(100).NotNullable()
                .WithColumn("Value").AsString(250).NotNullable()
                .WithColumn("DefaultValue").AsString(250).NotNullable()
                .WithColumn("PredefinedFieldControlTypeId").AsInt64().NotNullable()
                .WithColumn("PredefinedFieldTypeId").AsInt64().NotNullable()
                .WithColumn("PredefinedFieldPopulatorTypeId").AsInt64().NotNullable()
                .WithColumn("Caption").AsString(200).Nullable()
                .WithColumn("PopulatorSource").AsString(500).Nullable()
                .WithColumn("Size").AsInt64().Nullable()
                .WithColumn("Empty").AsBoolean().Nullable()
                .WithColumn("Position").AsInt64().NotNullable()
                .WithColumn("IsVisible").AsBoolean().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "JobTemplateParameter", "JobTemplateParameterId", 1000);
            Create.Table("MailServer")
                .WithColumn("MailServerId").AsInt64().PrimaryKey("PK_MailServer").Identity().NotNullable()
                .WithColumn("MailServerCode").AsString(45).NotNullable()
                .WithColumn("MailServerName").AsString(100).NotNullable()
                .WithColumn("Host").AsString(150).NotNullable()
                .WithColumn("Port").AsInt64().NotNullable()
                .WithColumn("SenderName").AsString(100).NotNullable()
                .WithColumn("SenderEmail").AsString(150).NotNullable()
                .WithColumn("UserName").AsString(100).NotNullable()
                .WithColumn("Password").AsString(200).Nullable()
                .WithColumn("IsSecure").AsBoolean().Nullable()
                .WithColumn("IsDefault").AsBoolean().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "MailServer", "MailServerId", 1000);
            Create.Table("MeditationFile")
                .WithColumn("MeditationFileId").AsInt64().PrimaryKey("PK_MeditationFile").Identity().NotNullable()
                .WithColumn("PredefinedMeditationTypeId").AsInt64().NotNullable()
                .WithColumn("MeditationFileName").AsString(255).NotNullable()
                .WithColumn("Location").AsString(1024).Nullable()
                .WithColumn("FileName").AsString(255).NotNullable()
                .WithColumn("FileSize").AsInt64().NotNullable()
                .WithColumn("UserId").AsInt64().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "MeditationFile", "MeditationFileId", 1000);
            Create.Table("MeditationSession")
                .WithColumn("MeditationSessionId").AsInt64().PrimaryKey("PK_MeditationSession").Identity().NotNullable()
                .WithColumn("MeditationSessionDate").AsDateTime().NotNullable()
                .WithColumn("MeditationSessionTimeSpentInSeconds").AsInt64().Nullable()
                .WithColumn("MeditationFileId").AsInt64().NotNullable()
                .WithColumn("UserId").AsInt64().NotNullable()
                .WithColumn("AddedToFeed").AsBoolean().Nullable()
                .WithColumn("AddedToFeedDate").AsDateTime().Nullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "MeditationSession", "MeditationSessionId", 1000);
            Create.Table("MessageTemplate")
                .WithColumn("MessageTemplateId").AsInt64().PrimaryKey("PK_MessageTemplate").Identity().NotNullable()
                .WithColumn("PredefinedMessageTemplateTypeId").AsInt64().NotNullable()
                .WithColumn("EmailSubject").AsString(500).NotNullable()
                .WithColumn("EmailBody").AsString(2000).NotNullable()
                .WithColumn("SmsBody").AsString(2000).NotNullable()
                .WithColumn("AppNotificationSubject").AsString(500).NotNullable()
                .WithColumn("AppNotificationBody").AsString(2000).NotNullable()
                .WithColumn("WebNotificationSubject").AsString(500).NotNullable()
                .WithColumn("WebNotificationBody").AsString(2000).NotNullable()
                .WithColumn("SendEmailForInternalUser").AsBoolean().Nullable()
                .WithColumn("SendSmsForInternalUser").AsBoolean().Nullable()
                .WithColumn("SendAppNotificationForInternalUser").AsBoolean().Nullable()
                .WithColumn("SendEmailForExternalUser").AsBoolean().Nullable()
                .WithColumn("SendSmsForExternalUser").AsBoolean().Nullable()
                .WithColumn("SendAppNotificationForExternalUser").AsBoolean().Nullable()
                .WithColumn("AdditionalRecipients").AsString(500).Nullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "MessageTemplate", "MessageTemplateId", 1000);
            Create.Table("MessageToSend")
                .WithColumn("MessageToSendId").AsInt64().PrimaryKey("PK_MessageToSend").Identity().NotNullable()
                .WithColumn("PredefinedMessageToSendTypeId").AsInt64().NotNullable()
                .WithColumn("RealtimeMessageId").AsInt64().Nullable()
                .WithColumn("MessageFrom").AsString(500).NotNullable()
                .WithColumn("MessageTo").AsString(500).NotNullable()
                .WithColumn("MessageFromName").AsString(500).NotNullable()
                .WithColumn("MessageToName").AsString(500).NotNullable()
                .WithColumn("MessageSubject").AsString(500).Nullable()
                .WithColumn("MessageBody").AsString(2000).NotNullable()
                .WithColumn("SentDate").AsDateTime().Nullable()
                .WithColumn("Sent").AsBoolean().Nullable()
                .WithColumn("IsSentError").AsBoolean().Nullable()
                .WithColumn("SentErrorMessage").AsString(500).Nullable()
                .WithColumn("SentErrorDate").AsDateTime().Nullable()
                .WithColumn("RetryCount").AsInt64().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "MessageToSend", "MessageToSendId", 1000);
            Create.Table("NoteAttachment")
                .WithColumn("NoteAttachmentId").AsInt64().PrimaryKey("PK_NoteAttachment").Identity().NotNullable()
                .WithColumn("Title").AsString(512).NotNullable()
                .WithColumn("Description").AsString(512).NotNullable()
                .WithColumn("UserId").AsInt64().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "NoteAttachment", "NoteAttachmentId", 1000);
            Create.Table("PushNotificationServer")
                .WithColumn("PushNotificationServerId").AsInt64().PrimaryKey("PK_PushNotificationServer").Identity().NotNullable()
                .WithColumn("PushNotificationServerCode").AsString(45).NotNullable()
                .WithColumn("PushNotificationServerName").AsString(100).NotNullable()
                .WithColumn("PredefinedPushNotificationServerTypeId").AsInt64().NotNullable()
                .WithColumn("ProjectId").AsString(100).Nullable()
                .WithColumn("ApiKey").AsString(255).Nullable()
                .WithColumn("ApiSecret").AsString(255).Nullable()
                .WithColumn("AppleTeamId").AsString(100).Nullable()
                .WithColumn("AppleKeyId").AsString(100).Nullable()
                .WithColumn("AppleAuthKey").AsString(2000).Nullable()
                .WithColumn("IsEnabled").AsBoolean().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "PushNotificationServer", "PushNotificationServerId", 1000);
            Create.Table("RealtimeMessage")
                .WithColumn("RealtimeMessageId").AsInt64().PrimaryKey("PK_RealtimeMessage").Identity().NotNullable()
                .WithColumn("PredefinedMessagePriorityTypeId").AsInt64().NotNullable()
                .WithColumn("MessageDateTime").AsDateTime().NotNullable()
                .WithColumn("MessageSubject").AsString(255).NotNullable()
                .WithColumn("MessageContent").AsString(255).NotNullable()
                .WithColumn("MessageSentDateTime").AsDateTime().Nullable()
                .WithColumn("MessageViewedDateTime").AsDateTime().Nullable()
                .WithColumn("PredefinedRealtimeMessageStatusTypeId").AsInt64().NotNullable()
                .WithColumn("PredefinedNotificationMessageTypeId").AsInt64().NotNullable()
                .WithColumn("ResourceName").AsString(100).Nullable()
                .WithColumn("PredefinedResourceTypeId").AsInt64().Nullable()
                .WithColumn("ResourceId").AsInt64().Nullable()
                .WithColumn("ErrorMessage").AsString(2000).Nullable()
                .WithColumn("ErrorOccurredOn").AsDateTime().Nullable()
                .WithColumn("UserId").AsInt64().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "RealtimeMessage", "RealtimeMessageId", 1000);
            Create.Table("SavedJob")
                .WithColumn("SavedJobId").AsInt64().PrimaryKey("PK_SavedJob").Identity().NotNullable()
                .WithColumn("PredefinedJobTypeId").AsInt64().NotNullable()
                .WithColumn("PredefinedJobOrReportTypeId").AsInt64().Nullable()
                .WithColumn("JobTemplateId").AsInt64().NotNullable()
                .WithColumn("CompletedDate").AsDateTime().Nullable()
                .WithColumn("SubmittedDate").AsDateTime().NotNullable()
                .WithColumn("SubmittedByUserId").AsInt64().Nullable()
                .WithColumn("ExecutableFolder").AsString(200).NotNullable()
                .WithColumn("ExecutableName").AsString(200).NotNullable()
                .WithColumn("ExecutableParameter").AsString(200).Nullable()
                .WithColumn("SerializedJobParameter").AsString(200).Nullable()
                .WithColumn("ReportTemplateFile").AsString(200).Nullable()
                .WithColumn("ExportDOC").AsBoolean().NotNullable()
                .WithColumn("ExportXLS").AsBoolean().NotNullable()
                .WithColumn("ExportPPT").AsBoolean().NotNullable()
                .WithColumn("ExportCSV").AsBoolean().NotNullable()
                .WithColumn("ExportXML").AsBoolean().NotNullable()
                .WithColumn("ExportPDF").AsBoolean().NotNullable()
                .WithColumn("ExportCustom").AsBoolean().NotNullable()
                .WithColumn("CustomExportName").AsString(20).Nullable()
                .WithColumn("PickupDate").AsDateTime().Nullable()
                .WithColumn("JobName").AsString(200).Nullable()
                .WithColumn("UserId").AsInt64().Nullable()
                .WithColumn("IncludeDataSourceFile").AsBoolean().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "SavedJob", "SavedJobId", 1000);
            Create.Table("SavedJobNotificationEmail")
                .WithColumn("SavedJobNotificationEmailId").AsInt64().PrimaryKey("PK_SavedJobNotificationEmail").Identity().NotNullable()
                .WithColumn("SavedJobId").AsInt64().NotNullable()
                .WithColumn("Email").AsString(200).NotNullable()
                .WithColumn("SendStatus").AsInt64().NotNullable()
                .WithColumn("Message").AsString(200).Nullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "SavedJobNotificationEmail", "SavedJobNotificationEmailId", 1000);
            Create.Table("ScheduledJob")
                .WithColumn("ScheduledJobId").AsInt64().PrimaryKey("PK_ScheduledJob").Identity().NotNullable()
                .WithColumn("JobTemplateId").AsInt64().NotNullable()
                .WithColumn("PredefinedJobTypeId").AsInt64().Nullable()
                .WithColumn("PredefinedJobOrReportTypeId").AsInt64().Nullable()
                .WithColumn("SubmittedByUserId").AsInt64().Nullable()
                .WithColumn("ExecutableFolder").AsString(200).NotNullable()
                .WithColumn("ExecutableName").AsString(200).NotNullable()
                .WithColumn("ExecutableParameter").AsString(200).Nullable()
                .WithColumn("SerializedJobParameter").AsString(200).Nullable()
                .WithColumn("ReportTemplateFile").AsString(200).Nullable()
                .WithColumn("ExportDOC").AsBoolean().NotNullable()
                .WithColumn("ExportXLS").AsBoolean().NotNullable()
                .WithColumn("ExportPPT").AsBoolean().NotNullable()
                .WithColumn("ExportCSV").AsBoolean().NotNullable()
                .WithColumn("ExportXML").AsBoolean().NotNullable()
                .WithColumn("ExportPDF").AsBoolean().NotNullable()
                .WithColumn("ExportCustom").AsBoolean().NotNullable()
                .WithColumn("CustomExportName").AsString(20).Nullable()
                .WithColumn("PickupDate").AsDateTime().Nullable()
                .WithColumn("PickupTime").AsInt64().Nullable()
                .WithColumn("JobName").AsString(200).Nullable()
                .WithColumn("RunAsUserId").AsInt64().Nullable()
                .WithColumn("StartDate").AsDateTime().Nullable()
                .WithColumn("EndDate").AsDateTime().Nullable()
                .WithColumn("StartTime").AsDateTime().Nullable()
                .WithColumn("Recurring").AsBoolean().NotNullable()
                .WithColumn("RunMonday").AsBoolean().NotNullable()
                .WithColumn("RunTuesday").AsBoolean().NotNullable()
                .WithColumn("RunWednesday").AsBoolean().NotNullable()
                .WithColumn("RunThursday").AsBoolean().NotNullable()
                .WithColumn("RunFriday").AsBoolean().NotNullable()
                .WithColumn("RunSaturday").AsBoolean().NotNullable()
                .WithColumn("RunSunday").AsBoolean().NotNullable()
                .WithColumn("Active").AsBoolean().NotNullable()
                .WithColumn("IsMonthly").AsBoolean().NotNullable()
                .WithColumn("RunMonthlyDay").AsInt64().Nullable()
                .WithColumn("RunMonthlyTime").AsDateTime().Nullable()
                .WithColumn("IsHourly").AsBoolean().NotNullable()
                .WithColumn("IsDaily").AsBoolean().NotNullable()
                .WithColumn("RunDailyTime").AsDateTime().Nullable()
                .WithColumn("IsWeekly").AsBoolean().NotNullable()
                .WithColumn("RunHourlyMinute").AsInt64().Nullable()
                .WithColumn("IncludeDataSourceFile").AsBoolean().NotNullable()
                .WithColumn("RunOnWeekNumber").AsInt64().Nullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "ScheduledJob", "ScheduledJobId", 1000);
            Create.Table("ScheduledJobHistory")
                .WithColumn("ScheduledJobHistoryId").AsInt64().PrimaryKey("PK_ScheduledJobHistory").Identity().NotNullable()
                .WithColumn("ScheduledJobId").AsInt64().NotNullable()
                .WithColumn("PickupDate").AsDateTime().NotNullable()
                .WithColumn("PickupMachineName").AsString(200).NotNullable()
                .WithColumn("PickupProcessId").AsInt64().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "ScheduledJobHistory", "ScheduledJobHistoryId", 1000);
            Create.Table("ScheduledJobNotificationEmail")
                .WithColumn("ScheduledJobNotificationEmailId").AsInt64().PrimaryKey("PK_ScheduledTaskNotificationEmail").Identity().NotNullable()
                .WithColumn("ScheduledJobId").AsInt64().NotNullable()
                .WithColumn("Email").AsString(200).NotNullable()
                .WithColumn("SendStatus").AsInt64().NotNullable()
                .WithColumn("Message").AsString(200).Nullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "ScheduledJobNotificationEmail", "ScheduledJobNotificationEmailId", 1000);
            Create.Table("SingleSignOnServer")
                .WithColumn("SingleSignOnServerId").AsInt64().PrimaryKey("PK_SingleSignOnServer").Identity().NotNullable()
                .WithColumn("SingleSignOnServerName").AsString(100).NotNullable()
                .WithColumn("SingleSignOnServerCode").AsString(45).NotNullable()
                .WithColumn("PredefinedSingleSignOnServerTypeId").AsInt64().NotNullable()
                .WithColumn("Host").AsString(500).Nullable()
                .WithColumn("ClientId").AsString(100).Nullable()
                .WithColumn("ClientTenantId").AsString(100).Nullable()
                .WithColumn("ClientSecret").AsString(200).Nullable()
                .WithColumn("ForAllCompany").AsBoolean().NotNullable()
                .WithColumn("IsDefault").AsBoolean().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "SingleSignOnServer", "SingleSignOnServerId", 1000);
            Create.Table("SmsServer")
                .WithColumn("SmsServerId").AsInt64().PrimaryKey("PK_SmsServer").Identity().NotNullable()
                .WithColumn("SmsServerCode").AsString(45).NotNullable()
                .WithColumn("SmsServerName").AsString(100).NotNullable()
                .WithColumn("Host").AsString(150).NotNullable()
                .WithColumn("Port").AsInt64().NotNullable()
                .WithColumn("SenderName").AsString(100).NotNullable()
                .WithColumn("SenderEmail").AsString(150).NotNullable()
                .WithColumn("UserName").AsString(100).NotNullable()
                .WithColumn("Password").AsString(200).Nullable()
                .WithColumn("IsSecure").AsBoolean().Nullable()
                .WithColumn("IsDefault").AsBoolean().NotNullable()
                .WithColumn("PredefinedSmsAndVoiceServerTypeId").AsInt64().NotNullable()
                .WithColumn("TwilioAccountSid").AsString(255).Nullable()
                .WithColumn("TwilioAccountAuthToken").AsString(500).Nullable()
                .WithColumn("TwilioAccountPhoneNumber").AsString(255).Nullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "SmsServer", "SmsServerId", 1000);
            Create.Table("State")
                .WithColumn("StateId").AsInt64().PrimaryKey("PK_State").Identity().NotNullable()
                .WithColumn("StateName").AsString(255).NotNullable()
                .WithColumn("StateCode").AsString(255).NotNullable()
                .WithColumn("CountryId").AsInt64().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "State", "StateId", 1000);
            Create.Table("User")
                .WithColumn("UserId").AsInt64().PrimaryKey("PK_User").Identity().NotNullable()
                .WithColumn("UserGuid").AsGuid().NotNullable()
                .WithColumn("UserName").AsString(50).NotNullable()
                .WithColumn("PredefinedUserTypeId").AsInt64().NotNullable()
                .WithColumn("PasswordHash").AsString(1000).Nullable()
                .WithColumn("PasswordChangedDate").AsDateTime().Nullable()
                .WithColumn("Email").AsString(100).NotNullable()
                .WithColumn("FirstName").AsString(100).NotNullable()
                .WithColumn("LastName").AsString(100).Nullable()
                .WithColumn("WorkPhone").AsString(20).Nullable()
                .WithColumn("WorkPhoneExt").AsString(10).Nullable()
                .WithColumn("CellPhone").AsString(20).Nullable()
                .WithColumn("Active").AsBoolean().NotNullable()
                .WithColumn("NotificationThroughEmail").AsBoolean().Nullable()
                .WithColumn("OneTimeToken").AsGuid().Nullable()
                .WithColumn("OneTimeExpireDateTime").AsDateTime().Nullable()
                .WithColumn("LoginFailedAttempt").AsInt64().Nullable()
                .WithColumn("IsAccountLocked").AsBoolean().Nullable()
                .WithColumn("AccountLockedUntil").AsDateTime().Nullable()
                .WithColumn("NewUserActivationGuid").AsGuid().Nullable()
                .WithColumn("NewUserActivationCompletedOn").AsDateTime().Nullable()
                .WithColumn("ProfileImageGuid").AsGuid().Nullable()
                .WithColumn("ProfileImageLocation").AsString(1024).Nullable()
                .WithColumn("ProfileImageActualFileName").AsString(125).Nullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "User", "UserId", 1000);
            Create.Table("UserGroup")
                .WithColumn("UserGroupId").AsInt64().PrimaryKey("PK_UserGroup").Identity().NotNullable()
                .WithColumn("UserGroupName").AsString(100).NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "UserGroup", "UserGroupId", 1000);
            Create.Table("UserGroupUser")
                .WithColumn("UserGroupUserId").AsInt64().PrimaryKey("PK_UserGroupUser").Identity().NotNullable()
                .WithColumn("UserGroupId").AsInt64().NotNullable()
                .WithColumn("UserId").AsInt64().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "UserGroupUser", "UserGroupUserId", 1000);
            Create.Table("UserPermission")
                .WithColumn("UserPermissionId").AsInt64().PrimaryKey("PK_UserPermission").Identity().NotNullable()
                .WithColumn("UserId").AsInt64().NotNullable()
                .WithColumn("PredefinedUserPermissionTypeId").AsInt64().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "UserPermission", "UserPermissionId", 1000);
            Create.Table("VisionBoardPicture")
                .WithColumn("VisionBoardPictureId").AsInt64().PrimaryKey("PK_VisionBoardPicture").Identity().NotNullable()
                .WithColumn("Title").AsString(100).Nullable()
                .WithColumn("Location").AsString(1024).Nullable()
                .WithColumn("FileName").AsString(255).NotNullable()
                .WithColumn("FileSize").AsInt64().NotNullable()
                .WithColumn("ThumbnailLocation").AsString(1024).Nullable()
                .WithColumn("ThumbnailFileName").AsString(255).Nullable()
                .WithColumn("ThumbnailFileSize").AsInt64().Nullable()
                .WithColumn("UserId").AsInt64().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "VisionBoardPicture", "VisionBoardPictureId", 1000);
            Create.Table("VisionBoardSession")
                .WithColumn("VisionBoardSessionId").AsInt64().PrimaryKey("PK_VisionBoardSession").Identity().NotNullable()
                .WithColumn("VisionBoardSessionDate").AsDateTime().NotNullable()
                .WithColumn("VisionBoardSessionTimeSpentInSeconds").AsInt64().Nullable()
                .WithColumn("UserId").AsInt64().NotNullable()
                .WithColumn("AddedToFeed").AsBoolean().Nullable()
                .WithColumn("AddedToFeedDate").AsDateTime().Nullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "VisionBoardSession", "VisionBoardSessionId", 1000);
            Create.Table("VisionBoardSessionPicture")
                .WithColumn("VisionBoardSessionPictureId").AsInt64().PrimaryKey("PK_VisionBoardSessionPicture").Identity().NotNullable()
                .WithColumn("VisionBoardSessionId").AsInt64().NotNullable()
                .WithColumn("VisionBoardSessionPictureDate").AsDateTime().NotNullable()
                .WithColumn("VisionBoardPictureId").AsInt64().NotNullable()
                .WithColumn("VisionBoardSessionPictureTimeSpentInSeconds").AsInt64().Nullable()
                .WithColumn("UserId").AsInt64().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "VisionBoardSessionPicture", "VisionBoardSessionPictureId", 1000);
            Create.Table("VoiceNotificationServer")
                .WithColumn("VoiceNotificationServerId").AsInt64().PrimaryKey("PK_VoiceNotificationServer").Identity().NotNullable()
                .WithColumn("VoiceNotificationServerCode").AsString(45).NotNullable()
                .WithColumn("VoiceNotificationServerName").AsString(100).NotNullable()
                .WithColumn("PredefinedSmsAndVoiceServerTypeId").AsInt64().NotNullable()
                .WithColumn("TwilioAccountSid").AsString(255).Nullable()
                .WithColumn("TwilioAccountAuthToken").AsString(500).Nullable()
                .WithColumn("TwilioAccountPhoneNumber").AsString(255).Nullable()
                .WithColumn("IsEnabled").AsBoolean().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "VoiceNotificationServer", "VoiceNotificationServerId", 1000);
            Create.Table("WhatsAppNotificationServer")
                .WithColumn("WhatsAppNotificationServerId").AsInt64().PrimaryKey("PK_WhatsAppNotificationServer").Identity().NotNullable()
                .WithColumn("WhatsAppNotificationServerCode").AsString(45).NotNullable()
                .WithColumn("WhatsAppNotificationServerName").AsString(100).NotNullable()
                .WithColumn("PredefinedSmsAndVoiceServerTypeId").AsInt64().NotNullable()
                .WithColumn("ApiKey").AsString(255).NotNullable()
                .WithColumn("ApiSecret").AsString(255).NotNullable()
                .WithColumn("AccessToken").AsString(255).NotNullable()
                .WithColumn("VersionNumber").AsString(10).NotNullable()
                .WithColumn("PhoneNumberId").AsString(50).NotNullable()
                .WithColumn("BusinessAccountId").AsString(50).Nullable()
                .WithColumn("SenderPhoneNumber").AsString(20).Nullable()
                .WithColumn("IsEnabled").AsBoolean().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("CreatedByUserId").AsInt64().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedByUserId").AsInt64().Nullable()
;
            UpdateSequenceStartValue("", "WhatsAppNotificationServer", "WhatsAppNotificationServerId", 1000);

            //Indexes definitions
            Create.Index("IX_AppExecutionVersion_CreatedByUserId").OnTable("AppExecutionVersion")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_AppExecutionVersion_ModifiedByUserId").OnTable("AppExecutionVersion")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_AuditOperation_CreatedByUserId").OnTable("AuditOperation")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_AuditOperation_ModifiedByUserId").OnTable("AuditOperation")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_AuditOperation_PredefinedAuditOperationTypeId").OnTable("AuditOperation")
                .OnColumn("PredefinedAuditOperationTypeId")
            
            ;
            Create.Index("IX_AuditOperation_PredefinedResourceTypeId").OnTable("AuditOperation")
                .OnColumn("PredefinedResourceTypeId")
            
            ;
            Create.Index("IX_AuditOperation_ResourceId").OnTable("AuditOperation")
                .OnColumn("ResourceId")
            
            ;
            Create.Index("IX_AuditOperation_UserGroupId").OnTable("AuditOperation")
                .OnColumn("UserGroupId")
            
            ;
            Create.Index("IX_AuditOperation_UserGroupUserId").OnTable("AuditOperation")
                .OnColumn("UserGroupUserId")
            
            ;
            Create.Index("IX_AuditOperation_UserId").OnTable("AuditOperation")
                .OnColumn("UserId")
            
            ;
            Create.Index("IX_AuditOperation_UserPermissionId").OnTable("AuditOperation")
                .OnColumn("UserPermissionId")
            
            ;
            Create.Index("IX_AuditOperationDetail_AuditOperationId").OnTable("AuditOperationDetail")
                .OnColumn("AuditOperationId")
            
            ;
            Create.Index("IX_AuditOperationDetail_CreatedByUserId").OnTable("AuditOperationDetail")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_AuditOperationDetail_ModifiedByUserId").OnTable("AuditOperationDetail")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_Category_CreatedByUserId").OnTable("Category")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_Category_DefaultCategoryId").OnTable("Category")
                .OnColumn("DefaultCategoryId")
            
            ;
            Create.Index("IX_Category_ModifiedByUserId").OnTable("Category")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_Category_PredefinedCategoryTypeId").OnTable("Category")
                .OnColumn("PredefinedCategoryTypeId")
            
            ;
            Create.Index("IX_Category_PredefinedIconTypeId").OnTable("Category")
                .OnColumn("PredefinedIconTypeId")
            
            ;
            Create.Index("IX_Category_UserId").OnTable("Category")
                .OnColumn("UserId")
            
            ;
            Create.Index("IX_Chat_CreatedByUserId").OnTable("Chat")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_Chat_ModifiedByUserId").OnTable("Chat")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_Chat_UserId").OnTable("Chat")
                .OnColumn("UserId")
            
            ;
            Create.Index("IX_ChatMessage_ChatId").OnTable("ChatMessage")
                .OnColumn("ChatId")
            
            ;
            Create.Index("IX_ChatMessage_CreatedByUserId").OnTable("ChatMessage")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_ChatMessage_ModifiedByUserId").OnTable("ChatMessage")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_ChatMessage_UserId").OnTable("ChatMessage")
                .OnColumn("UserId")
            
            ;
            Create.Index("IX_ChatMessageStatus_ChatMessageId").OnTable("ChatMessageStatus")
                .OnColumn("ChatMessageId")
            
            ;
            Create.Index("IX_ChatMessageStatus_CreatedByUserId").OnTable("ChatMessageStatus")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_ChatMessageStatus_ModifiedByUserId").OnTable("ChatMessageStatus")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_ChatMessageStatus_UserId").OnTable("ChatMessageStatus")
                .OnColumn("UserId")
            
            ;
            Create.Index("IX_ChatUser_ChatId").OnTable("ChatUser")
                .OnColumn("ChatId")
            
            ;
            Create.Index("IX_ChatUser_CreatedByUserId").OnTable("ChatUser")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_ChatUser_ModifiedByUserId").OnTable("ChatUser")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_ChatUser_UserId").OnTable("ChatUser")
                .OnColumn("UserId")
            
            ;
            Create.Index("IX_CompanyStorageConfiguration_CreatedByUserId").OnTable("CompanyStorageConfiguration")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_CompanyStorageConfiguration_ModifiedByUserId").OnTable("CompanyStorageConfiguration")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_ConfigurationGroup_CreatedByUserId").OnTable("ConfigurationGroup")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_ConfigurationGroup_ModifiedByUserId").OnTable("ConfigurationGroup")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_ConfigurationItem_ConfigurationGroupId").OnTable("ConfigurationItem")
                .OnColumn("ConfigurationGroupId")
            
            ;
            Create.Index("IX_ConfigurationItem_CreatedByUserId").OnTable("ConfigurationItem")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_ConfigurationItem_ModifiedByUserId").OnTable("ConfigurationItem")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_Country_CreatedByUserId").OnTable("Country")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_Country_ModifiedByUserId").OnTable("Country")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_DailyAffirmationSession_CreatedByUserId").OnTable("DailyAffirmationSession")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_DailyAffirmationSession_ModifiedByUserId").OnTable("DailyAffirmationSession")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_DailyAffirmationSession_UserId").OnTable("DailyAffirmationSession")
                .OnColumn("UserId")
            
            ;
            Create.Index("IX_DailyAffirmationSessionCategory_CategoryId").OnTable("DailyAffirmationSessionCategory")
                .OnColumn("CategoryId")
            
            ;
            Create.Index("IX_DailyAffirmationSessionCategory_CreatedByUserId").OnTable("DailyAffirmationSessionCategory")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_DailyAffirmationSessionCategory_DailyAffirmationSessionId").OnTable("DailyAffirmationSessionCategory")
                .OnColumn("DailyAffirmationSessionId")
            
            ;
            Create.Index("IX_DailyAffirmationSessionCategory_ModifiedByUserId").OnTable("DailyAffirmationSessionCategory")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_DailyAffirmationSessionCategory_UserId").OnTable("DailyAffirmationSessionCategory")
                .OnColumn("UserId")
            
            ;
            Create.Index("IX_DailyAffirmationText_CategoryId").OnTable("DailyAffirmationText")
                .OnColumn("CategoryId")
            
            ;
            Create.Index("IX_DailyAffirmationText_CreatedByUserId").OnTable("DailyAffirmationText")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_DailyAffirmationText_ModifiedByUserId").OnTable("DailyAffirmationText")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_DailyAffirmationText_UserId").OnTable("DailyAffirmationText")
                .OnColumn("UserId")
            
            ;
            Create.Index("IX_DailyJournal_CreatedByUserId").OnTable("DailyJournal")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_DailyJournal_ModifiedByUserId").OnTable("DailyJournal")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_DailyJournal_PredefinedMoodTypeId").OnTable("DailyJournal")
                .OnColumn("PredefinedMoodTypeId")
            
            ;
            Create.Index("IX_DailyJournal_UserId").OnTable("DailyJournal")
                .OnColumn("UserId")
            
            ;
            Create.Index("IX_DefaultCategory_CreatedByUserId").OnTable("DefaultCategory")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_DefaultCategory_ModifiedByUserId").OnTable("DefaultCategory")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_DefaultCategory_PredefinedCategoryTypeId").OnTable("DefaultCategory")
                .OnColumn("PredefinedCategoryTypeId")
            
            ;
            Create.Index("IX_DefaultCategory_PredefinedIconTypeId").OnTable("DefaultCategory")
                .OnColumn("PredefinedIconTypeId")
            
            ;
            Create.Index("IX_DefaultDailyAffirmationText_CreatedByUserId").OnTable("DefaultDailyAffirmationText")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_DefaultDailyAffirmationText_DefaultCategoryId").OnTable("DefaultDailyAffirmationText")
                .OnColumn("DefaultCategoryId")
            
            ;
            Create.Index("IX_DefaultDailyAffirmationText_ModifiedByUserId").OnTable("DefaultDailyAffirmationText")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_DefaultGoalTracker_CreatedByUserId").OnTable("DefaultGoalTracker")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_DefaultGoalTracker_ModifiedByUserId").OnTable("DefaultGoalTracker")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_DefaultGoalTrackerCategory_CreatedByUserId").OnTable("DefaultGoalTrackerCategory")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_DefaultGoalTrackerCategory_DefaultCategoryId").OnTable("DefaultGoalTrackerCategory")
                .OnColumn("DefaultCategoryId")
            
            ;
            Create.Index("IX_DefaultGoalTrackerCategory_DefaultGoalTrackerId").OnTable("DefaultGoalTrackerCategory")
                .OnColumn("DefaultGoalTrackerId")
            
            ;
            Create.Index("IX_DefaultGoalTrackerCategory_ModifiedByUserId").OnTable("DefaultGoalTrackerCategory")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_DefaultMeditationFile_CreatedByUserId").OnTable("DefaultMeditationFile")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_DefaultMeditationFile_ModifiedByUserId").OnTable("DefaultMeditationFile")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_DefaultMeditationFile_PredefinedMeditationTypeId").OnTable("DefaultMeditationFile")
                .OnColumn("PredefinedMeditationTypeId")
            
            ;
            Create.Index("IX_DefaultUserTypePermission_CreatedByUserId").OnTable("DefaultUserTypePermission")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_DefaultUserTypePermission_ModifiedByUserId").OnTable("DefaultUserTypePermission")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_DefaultUserTypePermission_PredefinedUserPermissionTypeId").OnTable("DefaultUserTypePermission")
                .OnColumn("PredefinedUserPermissionTypeId")
            
            ;
            Create.Index("IX_DefaultUserTypePermission_PredefinedUserTypeId").OnTable("DefaultUserTypePermission")
                .OnColumn("PredefinedUserTypeId")
            
            ;
            Create.Index("IX_DefaultVisionBoardPicture_CreatedByUserId").OnTable("DefaultVisionBoardPicture")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_DefaultVisionBoardPicture_ModifiedByUserId").OnTable("DefaultVisionBoardPicture")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_EnumGroup_CreatedByUserId").OnTable("EnumGroup")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_EnumGroup_ModifiedByUserId").OnTable("EnumGroup")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_EnumType_CreatedByUserId").OnTable("EnumType")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_EnumType_EnumGroupId").OnTable("EnumType")
                .OnColumn("EnumGroupId")
            
            ;
            Create.Index("IX_EnumType_ModifiedByUserId").OnTable("EnumType")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_EnumType_ParentEnumTypeId").OnTable("EnumType")
                .OnColumn("ParentEnumTypeId")
            
            ;
            Create.Index("IX_Feed_CreatedByUserId").OnTable("Feed")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_Feed_DailyAffirmationSessionId").OnTable("Feed")
                .OnColumn("DailyAffirmationSessionId")
            
            ;
            Create.Index("IX_Feed_GoalTrackerId").OnTable("Feed")
                .OnColumn("GoalTrackerId")
            
            ;
            Create.Index("IX_Feed_GoalTrackerMileStoneId").OnTable("Feed")
                .OnColumn("GoalTrackerMileStoneId")
            
            ;
            Create.Index("IX_Feed_MeditationSessionId").OnTable("Feed")
                .OnColumn("MeditationSessionId")
            
            ;
            Create.Index("IX_Feed_ModifiedByUserId").OnTable("Feed")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_Feed_PredefinedGoalStatusTypeId").OnTable("Feed")
                .OnColumn("PredefinedGoalStatusTypeId")
            
            ;
            Create.Index("IX_Feed_PredefinedMeditationTypeId").OnTable("Feed")
                .OnColumn("PredefinedMeditationTypeId")
            
            ;
            Create.Index("IX_Feed_UserId").OnTable("Feed")
                .OnColumn("UserId")
            
            ;
            Create.Index("IX_Feed_VisionBoardSessionId").OnTable("Feed")
                .OnColumn("VisionBoardSessionId")
            
            ;
            Create.Index("IX_FileAttachment_CreatedByUserId").OnTable("FileAttachment")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_FileAttachment_FileAttachmentFolderId").OnTable("FileAttachment")
                .OnColumn("FileAttachmentFolderId")
            
            ;
            Create.Index("IX_FileAttachment_ModifiedByUserId").OnTable("FileAttachment")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_FileAttachment_UserId").OnTable("FileAttachment")
                .OnColumn("UserId")
            
            ;
            Create.Index("IX_FileAttachmentFolder_CreatedByUserId").OnTable("FileAttachmentFolder")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_FileAttachmentFolder_ModifiedByUserId").OnTable("FileAttachmentFolder")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_FileAttachmentFolder_ParentFileAttachmentFolderId").OnTable("FileAttachmentFolder")
                .OnColumn("ParentFileAttachmentFolderId")
            
            ;
            Create.Index("IX_FileAttachmentFolder_UserId").OnTable("FileAttachmentFolder")
                .OnColumn("UserId")
            
            ;
            Create.Index("IX_GoalTracker_CreatedByUserId").OnTable("GoalTracker")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_GoalTracker_ModifiedByUserId").OnTable("GoalTracker")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_GoalTracker_PredefinedGoalPriorityTypeId").OnTable("GoalTracker")
                .OnColumn("PredefinedGoalPriorityTypeId")
            
            ;
            Create.Index("IX_GoalTracker_PredefinedGoalStatusTypeId").OnTable("GoalTracker")
                .OnColumn("PredefinedGoalStatusTypeId")
            
            ;
            Create.Index("IX_GoalTracker_UserId").OnTable("GoalTracker")
                .OnColumn("UserId")
            
            ;
            Create.Index("IX_GoalTrackerCategory_CategoryId").OnTable("GoalTrackerCategory")
                .OnColumn("CategoryId")
            
            ;
            Create.Index("IX_GoalTrackerCategory_CreatedByUserId").OnTable("GoalTrackerCategory")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_GoalTrackerCategory_GoalTrackerId").OnTable("GoalTrackerCategory")
                .OnColumn("GoalTrackerId")
            
            ;
            Create.Index("IX_GoalTrackerCategory_ModifiedByUserId").OnTable("GoalTrackerCategory")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_GoalTrackerCategory_UserId").OnTable("GoalTrackerCategory")
                .OnColumn("UserId")
            
            ;
            Create.Index("IX_GoalTrackerMileStone_CreatedByUserId").OnTable("GoalTrackerMileStone")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_GoalTrackerMileStone_GoalTrackerId").OnTable("GoalTrackerMileStone")
                .OnColumn("GoalTrackerId")
            
            ;
            Create.Index("IX_GoalTrackerMileStone_ModifiedByUserId").OnTable("GoalTrackerMileStone")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_GoalTrackerMileStone_PredefinedGoalStatusTypeId").OnTable("GoalTrackerMileStone")
                .OnColumn("PredefinedGoalStatusTypeId")
            
            ;
            Create.Index("IX_GoalTrackerMileStone_UserId").OnTable("GoalTrackerMileStone")
                .OnColumn("UserId")
            
            ;
            Create.Index("IX_Job_CreatedByUserId").OnTable("Job")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_Job_ExecutionAppId").OnTable("Job")
                .OnColumn("ExecutionAppId")
            
            ;
            Create.Index("IX_Job_ExecutionProcessId").OnTable("Job")
                .OnColumn("ExecutionProcessId")
            
            ;
            Create.Index("IX_Job_ExecutionThreadId").OnTable("Job")
                .OnColumn("ExecutionThreadId")
            
            ;
            Create.Index("IX_Job_JobTemplateId").OnTable("Job")
                .OnColumn("JobTemplateId")
            
            ;
            Create.Index("IX_Job_ModifiedByUserId").OnTable("Job")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_Job_PredefinedJobOrReportTypeId").OnTable("Job")
                .OnColumn("PredefinedJobOrReportTypeId")
            
            ;
            Create.Index("IX_Job_PredefinedJobStatusTypeId").OnTable("Job")
                .OnColumn("PredefinedJobStatusTypeId")
            
            ;
            Create.Index("IX_Job_PredefinedJobTypeId").OnTable("Job")
                .OnColumn("PredefinedJobTypeId")
            
            ;
            Create.Index("IX_Job_SubmittedByUserId").OnTable("Job")
                .OnColumn("SubmittedByUserId")
            
            ;
            Create.Index("IX_JobAttachment_CreatedByUserId").OnTable("JobAttachment")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_JobAttachment_JobId").OnTable("JobAttachment")
                .OnColumn("JobId")
            
            ;
            Create.Index("IX_JobAttachment_ModifiedByUserId").OnTable("JobAttachment")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_JobNotificationEmail_CreatedByUserId").OnTable("JobNotificationEmail")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_JobNotificationEmail_JobId").OnTable("JobNotificationEmail")
                .OnColumn("JobId")
            
            ;
            Create.Index("IX_JobNotificationEmail_ModifiedByUserId").OnTable("JobNotificationEmail")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_JobTemplate_CreatedByUserId").OnTable("JobTemplate")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_JobTemplate_JobTemplateCategoryId").OnTable("JobTemplate")
                .OnColumn("JobTemplateCategoryId")
            
            ;
            Create.Index("IX_JobTemplate_ModifiedByUserId").OnTable("JobTemplate")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_JobTemplate_PredefinedJobOrReportTypeId").OnTable("JobTemplate")
                .OnColumn("PredefinedJobOrReportTypeId")
            
            ;
            Create.Index("IX_JobTemplate_PredefinedJobTypeId").OnTable("JobTemplate")
                .OnColumn("PredefinedJobTypeId")
            
            ;
            Create.Index("IX_JobTemplate_PredefinedJobUsageForTypeId").OnTable("JobTemplate")
                .OnColumn("PredefinedJobUsageForTypeId")
            
            ;
            Create.Index("IX_JobTemplateCategory_CreatedByUserId").OnTable("JobTemplateCategory")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_JobTemplateCategory_ModifiedByUserId").OnTable("JobTemplateCategory")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_JobTemplateParameter_CreatedByUserId").OnTable("JobTemplateParameter")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_JobTemplateParameter_JobTemplateId").OnTable("JobTemplateParameter")
                .OnColumn("JobTemplateId")
            
            ;
            Create.Index("IX_JobTemplateParameter_ModifiedByUserId").OnTable("JobTemplateParameter")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_JobTemplateParameter_PredefinedFieldControlTypeId").OnTable("JobTemplateParameter")
                .OnColumn("PredefinedFieldControlTypeId")
            
            ;
            Create.Index("IX_JobTemplateParameter_PredefinedFieldPopulatorTypeId").OnTable("JobTemplateParameter")
                .OnColumn("PredefinedFieldPopulatorTypeId")
            
            ;
            Create.Index("IX_JobTemplateParameter_PredefinedFieldTypeId").OnTable("JobTemplateParameter")
                .OnColumn("PredefinedFieldTypeId")
            
            ;
            Create.Index("IX_MailServer_CreatedByUserId").OnTable("MailServer")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_MailServer_ModifiedByUserId").OnTable("MailServer")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_MeditationFile_CreatedByUserId").OnTable("MeditationFile")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_MeditationFile_ModifiedByUserId").OnTable("MeditationFile")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_MeditationFile_PredefinedMeditationTypeId").OnTable("MeditationFile")
                .OnColumn("PredefinedMeditationTypeId")
            
            ;
            Create.Index("IX_MeditationFile_UserId").OnTable("MeditationFile")
                .OnColumn("UserId")
            
            ;
            Create.Index("IX_MeditationSession_CreatedByUserId").OnTable("MeditationSession")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_MeditationSession_MeditationFileId").OnTable("MeditationSession")
                .OnColumn("MeditationFileId")
            
            ;
            Create.Index("IX_MeditationSession_ModifiedByUserId").OnTable("MeditationSession")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_MeditationSession_UserId").OnTable("MeditationSession")
                .OnColumn("UserId")
            
            ;
            Create.Index("IX_MessageTemplate_CreatedByUserId").OnTable("MessageTemplate")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_MessageTemplate_ModifiedByUserId").OnTable("MessageTemplate")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_MessageTemplate_PredefinedMessageTemplateTypeId").OnTable("MessageTemplate")
                .OnColumn("PredefinedMessageTemplateTypeId")
            
            ;
            Create.Index("IX_MessageToSend_CreatedByUserId").OnTable("MessageToSend")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_MessageToSend_ModifiedByUserId").OnTable("MessageToSend")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_MessageToSend_PredefinedMessageToSendTypeId").OnTable("MessageToSend")
                .OnColumn("PredefinedMessageToSendTypeId")
            
            ;
            Create.Index("IX_MessageToSend_RealtimeMessageId").OnTable("MessageToSend")
                .OnColumn("RealtimeMessageId")
            
            ;
            Create.Index("IX_NoteAttachment_CreatedByUserId").OnTable("NoteAttachment")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_NoteAttachment_ModifiedByUserId").OnTable("NoteAttachment")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_NoteAttachment_UserId").OnTable("NoteAttachment")
                .OnColumn("UserId")
            
            ;
            Create.Index("IX_PushNotificationServer_CreatedByUserId").OnTable("PushNotificationServer")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_PushNotificationServer_ModifiedByUserId").OnTable("PushNotificationServer")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_PushNotificationServer_PredefinedPushNotificationServerTypeId").OnTable("PushNotificationServer")
                .OnColumn("PredefinedPushNotificationServerTypeId")
            
            ;
            Create.Index("IX_RealtimeMessage_CreatedByUserId").OnTable("RealtimeMessage")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_RealtimeMessage_ModifiedByUserId").OnTable("RealtimeMessage")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_RealtimeMessage_PredefinedMessagePriorityTypeId").OnTable("RealtimeMessage")
                .OnColumn("PredefinedMessagePriorityTypeId")
            
            ;
            Create.Index("IX_RealtimeMessage_PredefinedNotificationMessageTypeId").OnTable("RealtimeMessage")
                .OnColumn("PredefinedNotificationMessageTypeId")
            
            ;
            Create.Index("IX_RealtimeMessage_PredefinedRealtimeMessageStatusTypeId").OnTable("RealtimeMessage")
                .OnColumn("PredefinedRealtimeMessageStatusTypeId")
            
            ;
            Create.Index("IX_RealtimeMessage_PredefinedResourceTypeId").OnTable("RealtimeMessage")
                .OnColumn("PredefinedResourceTypeId")
            
            ;
            Create.Index("IX_RealtimeMessage_ResourceId").OnTable("RealtimeMessage")
                .OnColumn("ResourceId")
            
            ;
            Create.Index("IX_RealtimeMessage_UserId").OnTable("RealtimeMessage")
                .OnColumn("UserId")
            
            ;
            Create.Index("IX_SavedJob_CreatedByUserId").OnTable("SavedJob")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_SavedJob_JobTemplateId").OnTable("SavedJob")
                .OnColumn("JobTemplateId")
            
            ;
            Create.Index("IX_SavedJob_ModifiedByUserId").OnTable("SavedJob")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_SavedJob_PredefinedJobOrReportTypeId").OnTable("SavedJob")
                .OnColumn("PredefinedJobOrReportTypeId")
            
            ;
            Create.Index("IX_SavedJob_PredefinedJobTypeId").OnTable("SavedJob")
                .OnColumn("PredefinedJobTypeId")
            
            ;
            Create.Index("IX_SavedJob_SubmittedByUserId").OnTable("SavedJob")
                .OnColumn("SubmittedByUserId")
            
            ;
            Create.Index("IX_SavedJob_UserId").OnTable("SavedJob")
                .OnColumn("UserId")
            
            ;
            Create.Index("IX_SavedJobNotificationEmail_CreatedByUserId").OnTable("SavedJobNotificationEmail")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_SavedJobNotificationEmail_ModifiedByUserId").OnTable("SavedJobNotificationEmail")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_SavedJobNotificationEmail_SavedJobId").OnTable("SavedJobNotificationEmail")
                .OnColumn("SavedJobId")
            
            ;
            Create.Index("IX_ScheduledJob_CreatedByUserId").OnTable("ScheduledJob")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_ScheduledJob_JobTemplateId").OnTable("ScheduledJob")
                .OnColumn("JobTemplateId")
            
            ;
            Create.Index("IX_ScheduledJob_ModifiedByUserId").OnTable("ScheduledJob")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_ScheduledJob_PredefinedJobOrReportTypeId").OnTable("ScheduledJob")
                .OnColumn("PredefinedJobOrReportTypeId")
            
            ;
            Create.Index("IX_ScheduledJob_PredefinedJobTypeId").OnTable("ScheduledJob")
                .OnColumn("PredefinedJobTypeId")
            
            ;
            Create.Index("IX_ScheduledJob_RunAsUserId").OnTable("ScheduledJob")
                .OnColumn("RunAsUserId")
            
            ;
            Create.Index("IX_ScheduledJob_SubmittedByUserId").OnTable("ScheduledJob")
                .OnColumn("SubmittedByUserId")
            
            ;
            Create.Index("IX_ScheduledJobHistory_CreatedByUserId").OnTable("ScheduledJobHistory")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_ScheduledJobHistory_ModifiedByUserId").OnTable("ScheduledJobHistory")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_ScheduledJobHistory_PickupProcessId").OnTable("ScheduledJobHistory")
                .OnColumn("PickupProcessId")
            
            ;
            Create.Index("IX_ScheduledJobHistory_ScheduledJobId").OnTable("ScheduledJobHistory")
                .OnColumn("ScheduledJobId")
            
            ;
            Create.Index("IX_ScheduledJobNotificationEmail_CreatedByUserId").OnTable("ScheduledJobNotificationEmail")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_ScheduledJobNotificationEmail_ModifiedByUserId").OnTable("ScheduledJobNotificationEmail")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_ScheduledJobNotificationEmail_ScheduledJobId").OnTable("ScheduledJobNotificationEmail")
                .OnColumn("ScheduledJobId")
            
            ;
            Create.Index("IX_SingleSignOnServer_CreatedByUserId").OnTable("SingleSignOnServer")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_SingleSignOnServer_ModifiedByUserId").OnTable("SingleSignOnServer")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_SingleSignOnServer_PredefinedSingleSignOnServerTypeId").OnTable("SingleSignOnServer")
                .OnColumn("PredefinedSingleSignOnServerTypeId")
            
            ;
            Create.Index("IX_SmsServer_CreatedByUserId").OnTable("SmsServer")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_SmsServer_ModifiedByUserId").OnTable("SmsServer")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_SmsServer_PredefinedSmsAndVoiceServerTypeId").OnTable("SmsServer")
                .OnColumn("PredefinedSmsAndVoiceServerTypeId")
            
            ;
            Create.Index("IX_SmsServer_TwilioAccountSid").OnTable("SmsServer")
                .OnColumn("TwilioAccountSid")
            
            ;
            Create.Index("IX_State_CountryId").OnTable("State")
                .OnColumn("CountryId")
            
            ;
            Create.Index("IX_State_CreatedByUserId").OnTable("State")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_State_ModifiedByUserId").OnTable("State")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_User_CreatedByUserId").OnTable("User")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_User_ModifiedByUserId").OnTable("User")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_User_PredefinedUserTypeId").OnTable("User")
                .OnColumn("PredefinedUserTypeId")
            
            ;
            Create.Index("IX_UserGroup_CreatedByUserId").OnTable("UserGroup")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_UserGroup_ModifiedByUserId").OnTable("UserGroup")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_UserGroupUser_CreatedByUserId").OnTable("UserGroupUser")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_UserGroupUser_ModifiedByUserId").OnTable("UserGroupUser")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_UserGroupUser_UserGroupId").OnTable("UserGroupUser")
                .OnColumn("UserGroupId")
            
            ;
            Create.Index("IX_UserGroupUser_UserId").OnTable("UserGroupUser")
                .OnColumn("UserId")
            
            ;
            Create.Index("IX_UserPermission_CreatedByUserId").OnTable("UserPermission")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_UserPermission_ModifiedByUserId").OnTable("UserPermission")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_UserPermission_PredefinedUserPermissionTypeId").OnTable("UserPermission")
                .OnColumn("PredefinedUserPermissionTypeId")
            
            ;
            Create.Index("IX_UserPermission_UserId").OnTable("UserPermission")
                .OnColumn("UserId")
            
            ;
            Create.Index("IX_VisionBoardPicture_CreatedByUserId").OnTable("VisionBoardPicture")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_VisionBoardPicture_ModifiedByUserId").OnTable("VisionBoardPicture")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_VisionBoardPicture_UserId").OnTable("VisionBoardPicture")
                .OnColumn("UserId")
            
            ;
            Create.Index("IX_VisionBoardSession_CreatedByUserId").OnTable("VisionBoardSession")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_VisionBoardSession_ModifiedByUserId").OnTable("VisionBoardSession")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_VisionBoardSession_UserId").OnTable("VisionBoardSession")
                .OnColumn("UserId")
            
            ;
            Create.Index("IX_VisionBoardSessionPicture_CreatedByUserId").OnTable("VisionBoardSessionPicture")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_VisionBoardSessionPicture_ModifiedByUserId").OnTable("VisionBoardSessionPicture")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_VisionBoardSessionPicture_UserId").OnTable("VisionBoardSessionPicture")
                .OnColumn("UserId")
            
            ;
            Create.Index("IX_VisionBoardSessionPicture_VisionBoardPictureId").OnTable("VisionBoardSessionPicture")
                .OnColumn("VisionBoardPictureId")
            
            ;
            Create.Index("IX_VisionBoardSessionPicture_VisionBoardSessionId").OnTable("VisionBoardSessionPicture")
                .OnColumn("VisionBoardSessionId")
            
            ;
            Create.Index("IX_VoiceNotificationServer_CreatedByUserId").OnTable("VoiceNotificationServer")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_VoiceNotificationServer_ModifiedByUserId").OnTable("VoiceNotificationServer")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_VoiceNotificationServer_PredefinedSmsAndVoiceServerTypeId").OnTable("VoiceNotificationServer")
                .OnColumn("PredefinedSmsAndVoiceServerTypeId")
            
            ;
            Create.Index("IX_WhatsAppNotificationServer_CreatedByUserId").OnTable("WhatsAppNotificationServer")
                .OnColumn("CreatedByUserId")
            
            ;
            Create.Index("IX_WhatsAppNotificationServer_ModifiedByUserId").OnTable("WhatsAppNotificationServer")
                .OnColumn("ModifiedByUserId")
            
            ;
            Create.Index("IX_WhatsAppNotificationServer_PredefinedSmsAndVoiceServerTypeId").OnTable("WhatsAppNotificationServer")
                .OnColumn("PredefinedSmsAndVoiceServerTypeId")
            
            ;

            //UniqueKeys definitions

            //ForeignKeys definitions
            Create.ForeignKey("FK_AppExecutionVersion_CreatedByUserId").FromTable("AppExecutionVersion").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_AppExecutionVersion_ModifiedByUserId").FromTable("AppExecutionVersion").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_AuditOperation_CreatedByUserId").FromTable("AuditOperation").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_AuditOperation_ModifiedByUserId").FromTable("AuditOperation").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_AuditOperation_PredefinedAuditOperationTypeId").FromTable("AuditOperation").ForeignColumns("PredefinedAuditOperationTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
            Create.ForeignKey("FK_AuditOperation_PredefinedResourceTypeId").FromTable("AuditOperation").ForeignColumns("PredefinedResourceTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
            Create.ForeignKey("FK_AuditOperation_UserGroupId").FromTable("AuditOperation").ForeignColumns("UserGroupId")
                .ToTable("UserGroup").PrimaryColumns("UserGroupId");
            Create.ForeignKey("FK_AuditOperation_UserGroupUserId").FromTable("AuditOperation").ForeignColumns("UserGroupUserId")
                .ToTable("UserGroupUser").PrimaryColumns("UserGroupUserId");
            Create.ForeignKey("FK_AuditOperation_UserId").FromTable("AuditOperation").ForeignColumns("UserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_AuditOperation_UserPermissionId").FromTable("AuditOperation").ForeignColumns("UserPermissionId")
                .ToTable("UserPermission").PrimaryColumns("UserPermissionId");
            Create.ForeignKey("FK_AuditOperationDetail_AuditOperationId").FromTable("AuditOperationDetail").ForeignColumns("AuditOperationId")
                .ToTable("AuditOperation").PrimaryColumns("AuditOperationId");
            Create.ForeignKey("FK_AuditOperationDetail_CreatedByUserId").FromTable("AuditOperationDetail").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_AuditOperationDetail_ModifiedByUserId").FromTable("AuditOperationDetail").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_Category_CreatedByUserId").FromTable("Category").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_Category_DefaultCategoryId").FromTable("Category").ForeignColumns("DefaultCategoryId")
                .ToTable("DefaultCategory").PrimaryColumns("DefaultCategoryId");
            Create.ForeignKey("FK_Category_ModifiedByUserId").FromTable("Category").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_Category_PredefinedCategoryTypeId").FromTable("Category").ForeignColumns("PredefinedCategoryTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
            Create.ForeignKey("FK_Category_PredefinedIconTypeId").FromTable("Category").ForeignColumns("PredefinedIconTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
            Create.ForeignKey("FK_Category_UserId").FromTable("Category").ForeignColumns("UserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_Chat_CreatedByUserId").FromTable("Chat").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_Chat_ModifiedByUserId").FromTable("Chat").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_Chat_UserId").FromTable("Chat").ForeignColumns("UserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_ChatMessage_ChatId").FromTable("ChatMessage").ForeignColumns("ChatId")
                .ToTable("Chat").PrimaryColumns("ChatId");
            Create.ForeignKey("FK_ChatMessage_CreatedByUserId").FromTable("ChatMessage").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_ChatMessage_ModifiedByUserId").FromTable("ChatMessage").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_ChatMessage_UserId").FromTable("ChatMessage").ForeignColumns("UserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_ChatMessageStatus_ChatMessageId").FromTable("ChatMessageStatus").ForeignColumns("ChatMessageId")
                .ToTable("ChatMessage").PrimaryColumns("ChatMessageId");
            Create.ForeignKey("FK_ChatMessageStatus_CreatedByUserId").FromTable("ChatMessageStatus").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_ChatMessageStatus_ModifiedByUserId").FromTable("ChatMessageStatus").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_ChatMessageStatus_UserId").FromTable("ChatMessageStatus").ForeignColumns("UserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_ChatUser_ChatId").FromTable("ChatUser").ForeignColumns("ChatId")
                .ToTable("Chat").PrimaryColumns("ChatId");
            Create.ForeignKey("FK_ChatUser_CreatedByUserId").FromTable("ChatUser").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_ChatUser_ModifiedByUserId").FromTable("ChatUser").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_ChatUser_UserId").FromTable("ChatUser").ForeignColumns("UserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_CompanyStorageConfiguration_CreatedByUserId").FromTable("CompanyStorageConfiguration").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_CompanyStorageConfiguration_ModifiedByUserId").FromTable("CompanyStorageConfiguration").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_ConfigurationGroup_CreatedByUserId").FromTable("ConfigurationGroup").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_ConfigurationGroup_ModifiedByUserId").FromTable("ConfigurationGroup").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_ConfigurationItem_ConfigurationGroupId").FromTable("ConfigurationItem").ForeignColumns("ConfigurationGroupId")
                .ToTable("ConfigurationGroup").PrimaryColumns("ConfigurationGroupId");
            Create.ForeignKey("FK_ConfigurationItem_CreatedByUserId").FromTable("ConfigurationItem").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_ConfigurationItem_ModifiedByUserId").FromTable("ConfigurationItem").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_Country_CreatedByUserId").FromTable("Country").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_Country_ModifiedByUserId").FromTable("Country").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_DailyAffirmationSession_CreatedByUserId").FromTable("DailyAffirmationSession").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_DailyAffirmationSession_ModifiedByUserId").FromTable("DailyAffirmationSession").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_DailyAffirmationSession_UserId").FromTable("DailyAffirmationSession").ForeignColumns("UserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_DailyAffirmationSessionCategory_CategoryId").FromTable("DailyAffirmationSessionCategory").ForeignColumns("CategoryId")
                .ToTable("Category").PrimaryColumns("CategoryId");
            Create.ForeignKey("FK_DailyAffirmationSessionCategory_CreatedByUserId").FromTable("DailyAffirmationSessionCategory").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_DailyAffirmationSessionCategory_DailyAffirmationSessionId").FromTable("DailyAffirmationSessionCategory").ForeignColumns("DailyAffirmationSessionId")
                .ToTable("DailyAffirmationSession").PrimaryColumns("DailyAffirmationSessionId");
            Create.ForeignKey("FK_DailyAffirmationSessionCategory_ModifiedByUserId").FromTable("DailyAffirmationSessionCategory").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_DailyAffirmationSessionCategory_UserId").FromTable("DailyAffirmationSessionCategory").ForeignColumns("UserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_DailyAffirmationText_CategoryId").FromTable("DailyAffirmationText").ForeignColumns("CategoryId")
                .ToTable("Category").PrimaryColumns("CategoryId");
            Create.ForeignKey("FK_DailyAffirmationText_CreatedByUserId").FromTable("DailyAffirmationText").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_DailyAffirmationText_ModifiedByUserId").FromTable("DailyAffirmationText").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_DailyAffirmationText_UserId").FromTable("DailyAffirmationText").ForeignColumns("UserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_DailyJournal_CreatedByUserId").FromTable("DailyJournal").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_DailyJournal_ModifiedByUserId").FromTable("DailyJournal").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_DailyJournal_PredefinedMoodTypeId").FromTable("DailyJournal").ForeignColumns("PredefinedMoodTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
            Create.ForeignKey("FK_DailyJournal_UserId").FromTable("DailyJournal").ForeignColumns("UserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_DefaultCategory_CreatedByUserId").FromTable("DefaultCategory").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_DefaultCategory_ModifiedByUserId").FromTable("DefaultCategory").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_DefaultCategory_PredefinedCategoryTypeId").FromTable("DefaultCategory").ForeignColumns("PredefinedCategoryTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
            Create.ForeignKey("FK_DefaultCategory_PredefinedIconTypeId").FromTable("DefaultCategory").ForeignColumns("PredefinedIconTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
            Create.ForeignKey("FK_DefaultDailyAffirmationText_CreatedByUserId").FromTable("DefaultDailyAffirmationText").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_DefaultDailyAffirmationText_DefaultCategoryId").FromTable("DefaultDailyAffirmationText").ForeignColumns("DefaultCategoryId")
                .ToTable("DefaultCategory").PrimaryColumns("DefaultCategoryId");
            Create.ForeignKey("FK_DefaultDailyAffirmationText_ModifiedByUserId").FromTable("DefaultDailyAffirmationText").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_DefaultGoalTracker_CreatedByUserId").FromTable("DefaultGoalTracker").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_DefaultGoalTracker_ModifiedByUserId").FromTable("DefaultGoalTracker").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_DefaultGoalTrackerCategory_CreatedByUserId").FromTable("DefaultGoalTrackerCategory").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_DefaultGoalTrackerCategory_DefaultCategoryId").FromTable("DefaultGoalTrackerCategory").ForeignColumns("DefaultCategoryId")
                .ToTable("DefaultCategory").PrimaryColumns("DefaultCategoryId");
            Create.ForeignKey("FK_DefaultGoalTrackerCategory_DefaultGoalTrackerId").FromTable("DefaultGoalTrackerCategory").ForeignColumns("DefaultGoalTrackerId")
                .ToTable("DefaultGoalTracker").PrimaryColumns("DefaultGoalTrackerId");
            Create.ForeignKey("FK_DefaultGoalTrackerCategory_ModifiedByUserId").FromTable("DefaultGoalTrackerCategory").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_DefaultMeditationFile_CreatedByUserId").FromTable("DefaultMeditationFile").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_DefaultMeditationFile_ModifiedByUserId").FromTable("DefaultMeditationFile").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_DefaultMeditationFile_PredefinedMeditationTypeId").FromTable("DefaultMeditationFile").ForeignColumns("PredefinedMeditationTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
            Create.ForeignKey("FK_DefaultUserTypePermission_CreatedByUserId").FromTable("DefaultUserTypePermission").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_DefaultUserTypePermission_ModifiedByUserId").FromTable("DefaultUserTypePermission").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_DefaultUserTypePermission_PredefinedUserPermissionTypeId").FromTable("DefaultUserTypePermission").ForeignColumns("PredefinedUserPermissionTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
            Create.ForeignKey("FK_DefaultUserTypePermission_PredefinedUserTypeId").FromTable("DefaultUserTypePermission").ForeignColumns("PredefinedUserTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
            Create.ForeignKey("FK_DefaultVisionBoardPicture_CreatedByUserId").FromTable("DefaultVisionBoardPicture").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_DefaultVisionBoardPicture_ModifiedByUserId").FromTable("DefaultVisionBoardPicture").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_EnumGroup_CreatedByUserId").FromTable("EnumGroup").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_EnumGroup_ModifiedByUserId").FromTable("EnumGroup").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_EnumType_CreatedByUserId").FromTable("EnumType").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_EnumType_EnumGroupId").FromTable("EnumType").ForeignColumns("EnumGroupId")
                .ToTable("EnumGroup").PrimaryColumns("EnumGroupId");
            Create.ForeignKey("FK_EnumType_ModifiedByUserId").FromTable("EnumType").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_EnumType_ParentEnumTypeId").FromTable("EnumType").ForeignColumns("ParentEnumTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
            Create.ForeignKey("FK_Feed_CreatedByUserId").FromTable("Feed").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_Feed_DailyAffirmationSessionId").FromTable("Feed").ForeignColumns("DailyAffirmationSessionId")
                .ToTable("DailyAffirmationSession").PrimaryColumns("DailyAffirmationSessionId");
            Create.ForeignKey("FK_Feed_GoalTrackerId").FromTable("Feed").ForeignColumns("GoalTrackerId")
                .ToTable("GoalTracker").PrimaryColumns("GoalTrackerId");
            Create.ForeignKey("FK_Feed_GoalTrackerMileStoneId").FromTable("Feed").ForeignColumns("GoalTrackerMileStoneId")
                .ToTable("GoalTrackerMileStone").PrimaryColumns("GoalTrackerMileStoneId");
            Create.ForeignKey("FK_Feed_MeditationSessionId").FromTable("Feed").ForeignColumns("MeditationSessionId")
                .ToTable("MeditationSession").PrimaryColumns("MeditationSessionId");
            Create.ForeignKey("FK_Feed_ModifiedByUserId").FromTable("Feed").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_Feed_PredefinedGoalStatusTypeId").FromTable("Feed").ForeignColumns("PredefinedGoalStatusTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
            Create.ForeignKey("FK_Feed_PredefinedMeditationTypeId").FromTable("Feed").ForeignColumns("PredefinedMeditationTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
            Create.ForeignKey("FK_Feed_UserId").FromTable("Feed").ForeignColumns("UserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_Feed_VisionBoardSessionId").FromTable("Feed").ForeignColumns("VisionBoardSessionId")
                .ToTable("VisionBoardSession").PrimaryColumns("VisionBoardSessionId");
            Create.ForeignKey("FK_FileAttachment_CreatedByUserId").FromTable("FileAttachment").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_FileAttachment_FileAttachmentFolderId").FromTable("FileAttachment").ForeignColumns("FileAttachmentFolderId")
                .ToTable("FileAttachmentFolder").PrimaryColumns("FileAttachmentFolderId");
            Create.ForeignKey("FK_FileAttachment_ModifiedByUserId").FromTable("FileAttachment").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_FileAttachment_UserId").FromTable("FileAttachment").ForeignColumns("UserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_FileAttachmentFolder_CreatedByUserId").FromTable("FileAttachmentFolder").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_FileAttachmentFolder_ModifiedByUserId").FromTable("FileAttachmentFolder").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_FileAttachmentFolder_ParentFileAttachmentFolderId").FromTable("FileAttachmentFolder").ForeignColumns("ParentFileAttachmentFolderId")
                .ToTable("FileAttachmentFolder").PrimaryColumns("FileAttachmentFolderId");
            Create.ForeignKey("FK_FileAttachmentFolder_UserId").FromTable("FileAttachmentFolder").ForeignColumns("UserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_GoalTracker_CreatedByUserId").FromTable("GoalTracker").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_GoalTracker_ModifiedByUserId").FromTable("GoalTracker").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_GoalTracker_PredefinedGoalPriorityTypeId").FromTable("GoalTracker").ForeignColumns("PredefinedGoalPriorityTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
            Create.ForeignKey("FK_GoalTracker_PredefinedGoalStatusTypeId").FromTable("GoalTracker").ForeignColumns("PredefinedGoalStatusTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
            Create.ForeignKey("FK_GoalTracker_UserId").FromTable("GoalTracker").ForeignColumns("UserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_GoalTrackerCategory_CategoryId").FromTable("GoalTrackerCategory").ForeignColumns("CategoryId")
                .ToTable("Category").PrimaryColumns("CategoryId");
            Create.ForeignKey("FK_GoalTrackerCategory_CreatedByUserId").FromTable("GoalTrackerCategory").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_GoalTrackerCategory_GoalTrackerId").FromTable("GoalTrackerCategory").ForeignColumns("GoalTrackerId")
                .ToTable("GoalTracker").PrimaryColumns("GoalTrackerId");
            Create.ForeignKey("FK_GoalTrackerCategory_ModifiedByUserId").FromTable("GoalTrackerCategory").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_GoalTrackerCategory_UserId").FromTable("GoalTrackerCategory").ForeignColumns("UserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_GoalTrackerMileStone_CreatedByUserId").FromTable("GoalTrackerMileStone").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_GoalTrackerMileStone_GoalTrackerId").FromTable("GoalTrackerMileStone").ForeignColumns("GoalTrackerId")
                .ToTable("GoalTracker").PrimaryColumns("GoalTrackerId");
            Create.ForeignKey("FK_GoalTrackerMileStone_ModifiedByUserId").FromTable("GoalTrackerMileStone").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_GoalTrackerMileStone_PredefinedGoalStatusTypeId").FromTable("GoalTrackerMileStone").ForeignColumns("PredefinedGoalStatusTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
            Create.ForeignKey("FK_GoalTrackerMileStone_UserId").FromTable("GoalTrackerMileStone").ForeignColumns("UserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_Job_CreatedByUserId").FromTable("Job").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_Job_JobTemplateId").FromTable("Job").ForeignColumns("JobTemplateId")
                .ToTable("JobTemplate").PrimaryColumns("JobTemplateId");
            Create.ForeignKey("FK_Job_ModifiedByUserId").FromTable("Job").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_Job_PredefinedJobOrReportTypeId").FromTable("Job").ForeignColumns("PredefinedJobOrReportTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
            Create.ForeignKey("FK_Job_PredefinedJobStatusTypeId").FromTable("Job").ForeignColumns("PredefinedJobStatusTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
            Create.ForeignKey("FK_Job_PredefinedJobTypeId").FromTable("Job").ForeignColumns("PredefinedJobTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
            Create.ForeignKey("FK_Job_SubmittedByUserId").FromTable("Job").ForeignColumns("SubmittedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_JobAttachment_CreatedByUserId").FromTable("JobAttachment").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_JobAttachment_JobId").FromTable("JobAttachment").ForeignColumns("JobId")
                .ToTable("Job").PrimaryColumns("JobId");
            Create.ForeignKey("FK_JobAttachment_ModifiedByUserId").FromTable("JobAttachment").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_JobNotificationEmail_CreatedByUserId").FromTable("JobNotificationEmail").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_JobNotificationEmail_JobId").FromTable("JobNotificationEmail").ForeignColumns("JobId")
                .ToTable("Job").PrimaryColumns("JobId");
            Create.ForeignKey("FK_JobNotificationEmail_ModifiedByUserId").FromTable("JobNotificationEmail").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_JobTemplate_CreatedByUserId").FromTable("JobTemplate").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_JobTemplate_JobTemplateCategoryId").FromTable("JobTemplate").ForeignColumns("JobTemplateCategoryId")
                .ToTable("JobTemplateCategory").PrimaryColumns("JobTemplateCategoryId");
            Create.ForeignKey("FK_JobTemplate_ModifiedByUserId").FromTable("JobTemplate").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_JobTemplate_PredefinedJobOrReportTypeId").FromTable("JobTemplate").ForeignColumns("PredefinedJobOrReportTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
            Create.ForeignKey("FK_JobTemplate_PredefinedJobTypeId").FromTable("JobTemplate").ForeignColumns("PredefinedJobTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
            Create.ForeignKey("FK_JobTemplate_PredefinedJobUsageForTypeId").FromTable("JobTemplate").ForeignColumns("PredefinedJobUsageForTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
            Create.ForeignKey("FK_JobTemplateCategory_CreatedByUserId").FromTable("JobTemplateCategory").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_JobTemplateCategory_ModifiedByUserId").FromTable("JobTemplateCategory").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_JobTemplateParameter_CreatedByUserId").FromTable("JobTemplateParameter").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_JobTemplateParameter_JobTemplateId").FromTable("JobTemplateParameter").ForeignColumns("JobTemplateId")
                .ToTable("JobTemplate").PrimaryColumns("JobTemplateId");
            Create.ForeignKey("FK_JobTemplateParameter_ModifiedByUserId").FromTable("JobTemplateParameter").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_JobTemplateParameter_PredefinedFieldControlTypeId").FromTable("JobTemplateParameter").ForeignColumns("PredefinedFieldControlTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
            Create.ForeignKey("FK_JobTemplateParameter_PredefinedFieldPopulatorTypeId").FromTable("JobTemplateParameter").ForeignColumns("PredefinedFieldPopulatorTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
            Create.ForeignKey("FK_JobTemplateParameter_PredefinedFieldTypeId").FromTable("JobTemplateParameter").ForeignColumns("PredefinedFieldTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
            Create.ForeignKey("FK_MailServer_CreatedByUserId").FromTable("MailServer").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_MailServer_ModifiedByUserId").FromTable("MailServer").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_MeditationFile_CreatedByUserId").FromTable("MeditationFile").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_MeditationFile_ModifiedByUserId").FromTable("MeditationFile").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_MeditationFile_PredefinedMeditationTypeId").FromTable("MeditationFile").ForeignColumns("PredefinedMeditationTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
            Create.ForeignKey("FK_MeditationFile_UserId").FromTable("MeditationFile").ForeignColumns("UserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_MeditationSession_CreatedByUserId").FromTable("MeditationSession").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_MeditationSession_MeditationFileId").FromTable("MeditationSession").ForeignColumns("MeditationFileId")
                .ToTable("MeditationFile").PrimaryColumns("MeditationFileId");
            Create.ForeignKey("FK_MeditationSession_ModifiedByUserId").FromTable("MeditationSession").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_MeditationSession_UserId").FromTable("MeditationSession").ForeignColumns("UserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_MessageTemplate_CreatedByUserId").FromTable("MessageTemplate").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_MessageTemplate_ModifiedByUserId").FromTable("MessageTemplate").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_MessageTemplate_PredefinedMessageTemplateTypeId").FromTable("MessageTemplate").ForeignColumns("PredefinedMessageTemplateTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
            Create.ForeignKey("FK_MessageToSend_CreatedByUserId").FromTable("MessageToSend").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_MessageToSend_ModifiedByUserId").FromTable("MessageToSend").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_MessageToSend_PredefinedMessageToSendTypeId").FromTable("MessageToSend").ForeignColumns("PredefinedMessageToSendTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
            Create.ForeignKey("FK_MessageToSend_RealtimeMessageId").FromTable("MessageToSend").ForeignColumns("RealtimeMessageId")
                .ToTable("RealtimeMessage").PrimaryColumns("RealtimeMessageId");
            Create.ForeignKey("FK_NoteAttachment_CreatedByUserId").FromTable("NoteAttachment").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_NoteAttachment_ModifiedByUserId").FromTable("NoteAttachment").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_NoteAttachment_UserId").FromTable("NoteAttachment").ForeignColumns("UserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_PushNotificationServer_CreatedByUserId").FromTable("PushNotificationServer").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_PushNotificationServer_ModifiedByUserId").FromTable("PushNotificationServer").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_PushNotificationServer_PredefinedPushNotificationServerTypeId").FromTable("PushNotificationServer").ForeignColumns("PredefinedPushNotificationServerTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
            Create.ForeignKey("FK_RealtimeMessage_CreatedByUserId").FromTable("RealtimeMessage").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_RealtimeMessage_ModifiedByUserId").FromTable("RealtimeMessage").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_RealtimeMessage_PredefinedMessagePriorityTypeId").FromTable("RealtimeMessage").ForeignColumns("PredefinedMessagePriorityTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
            Create.ForeignKey("FK_RealtimeMessage_PredefinedNotificationMessageTypeId").FromTable("RealtimeMessage").ForeignColumns("PredefinedNotificationMessageTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
            Create.ForeignKey("FK_RealtimeMessage_PredefinedRealtimeMessageStatusTypeId").FromTable("RealtimeMessage").ForeignColumns("PredefinedRealtimeMessageStatusTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
            Create.ForeignKey("FK_RealtimeMessage_PredefinedResourceTypeId").FromTable("RealtimeMessage").ForeignColumns("PredefinedResourceTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
            Create.ForeignKey("FK_RealtimeMessage_UserId").FromTable("RealtimeMessage").ForeignColumns("UserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_SavedJob_CreatedByUserId").FromTable("SavedJob").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_SavedJob_JobTemplateId").FromTable("SavedJob").ForeignColumns("JobTemplateId")
                .ToTable("JobTemplate").PrimaryColumns("JobTemplateId");
            Create.ForeignKey("FK_SavedJob_ModifiedByUserId").FromTable("SavedJob").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_SavedJob_PredefinedJobOrReportTypeId").FromTable("SavedJob").ForeignColumns("PredefinedJobOrReportTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
            Create.ForeignKey("FK_SavedJob_PredefinedJobTypeId").FromTable("SavedJob").ForeignColumns("PredefinedJobTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
            Create.ForeignKey("FK_SavedJob_SubmittedByUserId").FromTable("SavedJob").ForeignColumns("SubmittedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_SavedJob_UserId").FromTable("SavedJob").ForeignColumns("UserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_SavedJobNotificationEmail_CreatedByUserId").FromTable("SavedJobNotificationEmail").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_SavedJobNotificationEmail_ModifiedByUserId").FromTable("SavedJobNotificationEmail").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_SavedJobNotificationEmail_SavedJobId").FromTable("SavedJobNotificationEmail").ForeignColumns("SavedJobId")
                .ToTable("SavedJob").PrimaryColumns("SavedJobId");
            Create.ForeignKey("FK_ScheduledJob_CreatedByUserId").FromTable("ScheduledJob").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_ScheduledJob_JobTemplateId").FromTable("ScheduledJob").ForeignColumns("JobTemplateId")
                .ToTable("JobTemplate").PrimaryColumns("JobTemplateId");
            Create.ForeignKey("FK_ScheduledJob_ModifiedByUserId").FromTable("ScheduledJob").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_ScheduledJob_PredefinedJobOrReportTypeId").FromTable("ScheduledJob").ForeignColumns("PredefinedJobOrReportTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
            Create.ForeignKey("FK_ScheduledJob_PredefinedJobTypeId").FromTable("ScheduledJob").ForeignColumns("PredefinedJobTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
            Create.ForeignKey("FK_ScheduledJob_RunAsUserId").FromTable("ScheduledJob").ForeignColumns("RunAsUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_ScheduledJob_SubmittedByUserId").FromTable("ScheduledJob").ForeignColumns("SubmittedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_ScheduledJobHistory_CreatedByUserId").FromTable("ScheduledJobHistory").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_ScheduledJobHistory_ModifiedByUserId").FromTable("ScheduledJobHistory").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_ScheduledJobHistory_ScheduledJobId").FromTable("ScheduledJobHistory").ForeignColumns("ScheduledJobId")
                .ToTable("ScheduledJob").PrimaryColumns("ScheduledJobId");
            Create.ForeignKey("FK_ScheduledJobNotificationEmail_CreatedByUserId").FromTable("ScheduledJobNotificationEmail").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_ScheduledJobNotificationEmail_ModifiedByUserId").FromTable("ScheduledJobNotificationEmail").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_ScheduledJobNotificationEmail_ScheduledJobId").FromTable("ScheduledJobNotificationEmail").ForeignColumns("ScheduledJobId")
                .ToTable("ScheduledJob").PrimaryColumns("ScheduledJobId");
            Create.ForeignKey("FK_SingleSignOnServer_CreatedByUserId").FromTable("SingleSignOnServer").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_SingleSignOnServer_ModifiedByUserId").FromTable("SingleSignOnServer").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_SingleSignOnServer_PredefinedSingleSignOnServerTypeId").FromTable("SingleSignOnServer").ForeignColumns("PredefinedSingleSignOnServerTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
            Create.ForeignKey("FK_SmsServer_CreatedByUserId").FromTable("SmsServer").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_SmsServer_ModifiedByUserId").FromTable("SmsServer").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_SmsServer_PredefinedSmsAndVoiceServerTypeId").FromTable("SmsServer").ForeignColumns("PredefinedSmsAndVoiceServerTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
            Create.ForeignKey("FK_State_CountryId").FromTable("State").ForeignColumns("CountryId")
                .ToTable("Country").PrimaryColumns("CountryId");
            Create.ForeignKey("FK_State_CreatedByUserId").FromTable("State").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_State_ModifiedByUserId").FromTable("State").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_User_CreatedByUserId").FromTable("User").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_User_ModifiedByUserId").FromTable("User").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_User_PredefinedUserTypeId").FromTable("User").ForeignColumns("PredefinedUserTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
            Create.ForeignKey("FK_UserGroup_CreatedByUserId").FromTable("UserGroup").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_UserGroup_ModifiedByUserId").FromTable("UserGroup").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_UserGroupUser_CreatedByUserId").FromTable("UserGroupUser").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_UserGroupUser_ModifiedByUserId").FromTable("UserGroupUser").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_UserGroupUser_UserGroupId").FromTable("UserGroupUser").ForeignColumns("UserGroupId")
                .ToTable("UserGroup").PrimaryColumns("UserGroupId");
            Create.ForeignKey("FK_UserGroupUser_UserId").FromTable("UserGroupUser").ForeignColumns("UserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_UserPermission_CreatedByUserId").FromTable("UserPermission").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_UserPermission_ModifiedByUserId").FromTable("UserPermission").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_UserPermission_PredefinedUserPermissionTypeId").FromTable("UserPermission").ForeignColumns("PredefinedUserPermissionTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
            Create.ForeignKey("FK_UserPermission_UserId").FromTable("UserPermission").ForeignColumns("UserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_VisionBoardPicture_CreatedByUserId").FromTable("VisionBoardPicture").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_VisionBoardPicture_ModifiedByUserId").FromTable("VisionBoardPicture").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_VisionBoardPicture_UserId").FromTable("VisionBoardPicture").ForeignColumns("UserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_VisionBoardSession_CreatedByUserId").FromTable("VisionBoardSession").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_VisionBoardSession_ModifiedByUserId").FromTable("VisionBoardSession").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_VisionBoardSession_UserId").FromTable("VisionBoardSession").ForeignColumns("UserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_VisionBoardSessionPicture_CreatedByUserId").FromTable("VisionBoardSessionPicture").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_VisionBoardSessionPicture_ModifiedByUserId").FromTable("VisionBoardSessionPicture").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_VisionBoardSessionPicture_UserId").FromTable("VisionBoardSessionPicture").ForeignColumns("UserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_VisionBoardSessionPicture_VisionBoardPictureId").FromTable("VisionBoardSessionPicture").ForeignColumns("VisionBoardPictureId")
                .ToTable("VisionBoardPicture").PrimaryColumns("VisionBoardPictureId");
            Create.ForeignKey("FK_VisionBoardSessionPicture_VisionBoardSessionId").FromTable("VisionBoardSessionPicture").ForeignColumns("VisionBoardSessionId")
                .ToTable("VisionBoardSession").PrimaryColumns("VisionBoardSessionId");
            Create.ForeignKey("FK_VoiceNotificationServer_CreatedByUserId").FromTable("VoiceNotificationServer").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_VoiceNotificationServer_ModifiedByUserId").FromTable("VoiceNotificationServer").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_VoiceNotificationServer_PredefinedSmsAndVoiceServerTypeId").FromTable("VoiceNotificationServer").ForeignColumns("PredefinedSmsAndVoiceServerTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
            Create.ForeignKey("FK_WhatsAppNotificationServer_CreatedByUserId").FromTable("WhatsAppNotificationServer").ForeignColumns("CreatedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_WhatsAppNotificationServer_ModifiedByUserId").FromTable("WhatsAppNotificationServer").ForeignColumns("ModifiedByUserId")
                .ToTable("User").PrimaryColumns("UserId");
            Create.ForeignKey("FK_WhatsAppNotificationServer_PredefinedSmsAndVoiceServerTypeId").FromTable("WhatsAppNotificationServer").ForeignColumns("PredefinedSmsAndVoiceServerTypeId")
                .ToTable("EnumType").PrimaryColumns("EnumTypeId");
        }
    }
}
#pragma warning restore SA1601
#pragma warning restore SA1649
#pragma warning restore SA1505
#pragma warning restore CS0168
#pragma warning restore S1481
#pragma warning restore SA1507
#pragma warning restore SA1402
#pragma warning restore SA1124
#pragma warning restore SA1028
#pragma warning restore SA1516
#pragma warning restore SA1310