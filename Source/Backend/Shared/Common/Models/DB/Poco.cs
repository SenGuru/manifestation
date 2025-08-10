using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;
using Framework.Infrastructure.Interfaces.DbAccess;
using LinqToDB.Mapping;

#pragma warning disable SA1310
#pragma warning disable SA1516
#pragma warning disable SA1028 // Code should not contain trailing whitespace
#pragma warning disable SA1402
#pragma warning disable SA1124
#pragma warning disable SA1505
#pragma warning disable SA1121
#pragma warning disable SA1649
#pragma warning disable SA1601
#pragma warning disable CC0021
#pragma warning disable CC0065
#pragma warning disable SA1204

namespace Manifest.Common.Models.DB
{

    [Table("AppExecutionVersion")]
    [Serializable]
    public partial class AppExecutionVersion : IAuditableModel, INewObject<AppExecutionVersion>
    {
        public const string TABLE_NAME = "AppExecutionVersion";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_AppExecutionVersionId = "AppExecutionVersionId";
        public const string COLUMN_Version = "Version";
        public const string COLUMN_RunDate = "RunDate";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_AppExecutionVersionId = false;
        public const bool COLUMN_NULLABLE_Version = false;
        public const bool COLUMN_NULLABLE_RunDate = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("AppExecutionVersionId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long AppExecutionVersionId { get; set; }
        
        [Column("Version")]
        [NotNull]
        public long Version { get; set; }
        
        [Column("RunDate")]
        [NotNull]
        public long RunDate { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"AppExecutionVersionId = {AppExecutionVersionId} , ");
            sb.AppendLine($"Version = {Version} , ");
            sb.AppendLine($"RunDate = {RunDate} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public AppExecutionVersion New(AppExecutionVersion item)
        {
            if (item == null)
                return null;

            return new AppExecutionVersion
            {
            AppExecutionVersionId = item.AppExecutionVersionId,
            Version = item.Version,
            RunDate = item.RunDate,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public AppExecutionVersion NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("AuditOperation")]
    [Serializable]
    public partial class AuditOperation : IAuditableModel, INewObject<AuditOperation>
    {
        public const string TABLE_NAME = "AuditOperation";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_AuditOperationId = "AuditOperationId";
        public const string COLUMN_AuditDateTime = "AuditDateTime";
        public const string COLUMN_PredefinedAuditOperationTypeId = "PredefinedAuditOperationTypeId";
        public const string COLUMN_ChangeDiff = "ChangeDiff";
        public const string COLUMN_UserId = "UserId";
        public const string COLUMN_UserGroupId = "UserGroupId";
        public const string COLUMN_UserGroupUserId = "UserGroupUserId";
        public const string COLUMN_UserPermissionId = "UserPermissionId";
        public const string COLUMN_PredefinedResourceTypeId = "PredefinedResourceTypeId";
        public const string COLUMN_ResourceId = "ResourceId";
        public const string COLUMN_IpAddress = "IpAddress";
        public const string COLUMN_BrowserName = "BrowserName";
        public const string COLUMN_OperatingSystemName = "OperatingSystemName";
        public const string COLUMN_DeviceName = "DeviceName";
        public const string COLUMN_Description = "Description";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_IpAddress = 100;
        public const int COLUMN_SIZE_BrowserName = 200;
        public const int COLUMN_SIZE_OperatingSystemName = 200;
        public const int COLUMN_SIZE_DeviceName = 200;
        public const int COLUMN_SIZE_Description = 500;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_AuditOperationId = false;
        public const bool COLUMN_NULLABLE_AuditDateTime = false;
        public const bool COLUMN_NULLABLE_PredefinedAuditOperationTypeId = false;
        public const bool COLUMN_NULLABLE_ChangeDiff = true;
        public const bool COLUMN_NULLABLE_UserId = true;
        public const bool COLUMN_NULLABLE_UserGroupId = true;
        public const bool COLUMN_NULLABLE_UserGroupUserId = true;
        public const bool COLUMN_NULLABLE_UserPermissionId = true;
        public const bool COLUMN_NULLABLE_PredefinedResourceTypeId = true;
        public const bool COLUMN_NULLABLE_ResourceId = true;
        public const bool COLUMN_NULLABLE_IpAddress = true;
        public const bool COLUMN_NULLABLE_BrowserName = true;
        public const bool COLUMN_NULLABLE_OperatingSystemName = true;
        public const bool COLUMN_NULLABLE_DeviceName = true;
        public const bool COLUMN_NULLABLE_Description = true;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("AuditOperationId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long AuditOperationId { get; set; }
        
        [Column("AuditDateTime")]
        [NotNull]
        public DateTime AuditDateTime { get; set; }
        
        [Column("PredefinedAuditOperationTypeId")]
        [NotNull]
        public long PredefinedAuditOperationTypeId { get; set; }
        
        [Column("ChangeDiff")]
        public string ChangeDiff { get; set; }
        
        [Column("UserId")]
        public long? UserId { get; set; }
        
        [Column("UserGroupId")]
        public long? UserGroupId { get; set; }
        
        [Column("UserGroupUserId")]
        public long? UserGroupUserId { get; set; }
        
        [Column("UserPermissionId")]
        public long? UserPermissionId { get; set; }
        
        [Column("PredefinedResourceTypeId")]
        public long? PredefinedResourceTypeId { get; set; }
        
        [Column("ResourceId")]
        public long? ResourceId { get; set; }
        
        [Column("IpAddress")]
        [StringLength(100)]
        public string IpAddress { get; set; }
        
        [Column("BrowserName")]
        [StringLength(200)]
        public string BrowserName { get; set; }
        
        [Column("OperatingSystemName")]
        [StringLength(200)]
        public string OperatingSystemName { get; set; }
        
        [Column("DeviceName")]
        [StringLength(200)]
        public string DeviceName { get; set; }
        
        [Column("Description")]
        [StringLength(500)]
        public string Description { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"AuditOperationId = {AuditOperationId} , ");
            sb.AppendLine($"AuditDateTime = {AuditDateTime} , ");
            sb.AppendLine($"PredefinedAuditOperationTypeId = {PredefinedAuditOperationTypeId} , ");
            sb.AppendLine($"ChangeDiff = {ChangeDiff} , ");
            sb.AppendLine($"UserId = {UserId} , ");
            sb.AppendLine($"UserGroupId = {UserGroupId} , ");
            sb.AppendLine($"UserGroupUserId = {UserGroupUserId} , ");
            sb.AppendLine($"UserPermissionId = {UserPermissionId} , ");
            sb.AppendLine($"PredefinedResourceTypeId = {PredefinedResourceTypeId} , ");
            sb.AppendLine($"ResourceId = {ResourceId} , ");
            sb.AppendLine($"IpAddress = {IpAddress} , ");
            sb.AppendLine($"BrowserName = {BrowserName} , ");
            sb.AppendLine($"OperatingSystemName = {OperatingSystemName} , ");
            sb.AppendLine($"DeviceName = {DeviceName} , ");
            sb.AppendLine($"Description = {Description} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public AuditOperation New(AuditOperation item)
        {
            if (item == null)
                return null;

            return new AuditOperation
            {
            AuditOperationId = item.AuditOperationId,
            AuditDateTime = item.AuditDateTime,
            PredefinedAuditOperationTypeId = item.PredefinedAuditOperationTypeId,
            ChangeDiff = item.ChangeDiff,
            UserId = item.UserId,
            UserGroupId = item.UserGroupId,
            UserGroupUserId = item.UserGroupUserId,
            UserPermissionId = item.UserPermissionId,
            PredefinedResourceTypeId = item.PredefinedResourceTypeId,
            ResourceId = item.ResourceId,
            IpAddress = item.IpAddress,
            BrowserName = item.BrowserName,
            OperatingSystemName = item.OperatingSystemName,
            DeviceName = item.DeviceName,
            Description = item.Description,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public AuditOperation NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("AuditOperationDetail")]
    [Serializable]
    public partial class AuditOperationDetail : IAuditableModel, INewObject<AuditOperationDetail>
    {
        public const string TABLE_NAME = "AuditOperationDetail";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_AuditOperationDetailId = "AuditOperationDetailId";
        public const string COLUMN_AuditOperationId = "AuditOperationId";
        public const string COLUMN_DifferenceJson = "DifferenceJson";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_AuditOperationDetailId = false;
        public const bool COLUMN_NULLABLE_AuditOperationId = false;
        public const bool COLUMN_NULLABLE_DifferenceJson = true;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("AuditOperationDetailId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long AuditOperationDetailId { get; set; }
        
        [Column("AuditOperationId")]
        [NotNull]
        public long AuditOperationId { get; set; }
        
        [Column("DifferenceJson")]
        public string DifferenceJson { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"AuditOperationDetailId = {AuditOperationDetailId} , ");
            sb.AppendLine($"AuditOperationId = {AuditOperationId} , ");
            sb.AppendLine($"DifferenceJson = {DifferenceJson} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public AuditOperationDetail New(AuditOperationDetail item)
        {
            if (item == null)
                return null;

            return new AuditOperationDetail
            {
            AuditOperationDetailId = item.AuditOperationDetailId,
            AuditOperationId = item.AuditOperationId,
            DifferenceJson = item.DifferenceJson,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public AuditOperationDetail NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("Category")]
    [Serializable]
    public partial class Category : IAuditableModel, INewObject<Category>
    {
        public const string TABLE_NAME = "Category";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_CategoryId = "CategoryId";
        public const string COLUMN_DefaultCategoryId = "DefaultCategoryId";
        public const string COLUMN_CategoryName = "CategoryName";
        public const string COLUMN_PredefinedCategoryTypeId = "PredefinedCategoryTypeId";
        public const string COLUMN_UserId = "UserId";
        public const string COLUMN_ColorAsInteger = "ColorAsInteger";
        public const string COLUMN_PredefinedIconTypeId = "PredefinedIconTypeId";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_CategoryName = 255;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_CategoryId = false;
        public const bool COLUMN_NULLABLE_DefaultCategoryId = false;
        public const bool COLUMN_NULLABLE_CategoryName = false;
        public const bool COLUMN_NULLABLE_PredefinedCategoryTypeId = true;
        public const bool COLUMN_NULLABLE_UserId = false;
        public const bool COLUMN_NULLABLE_ColorAsInteger = false;
        public const bool COLUMN_NULLABLE_PredefinedIconTypeId = false;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("CategoryId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long CategoryId { get; set; }
        
        [Column("DefaultCategoryId")]
        [NotNull]
        public long DefaultCategoryId { get; set; }
        
        [Column("CategoryName")]
        [NotNull]
        [StringLength(255)]
        public string CategoryName { get; set; }
        
        [Column("PredefinedCategoryTypeId")]
        public long? PredefinedCategoryTypeId { get; set; }
        
        [Column("UserId")]
        [NotNull]
        public long UserId { get; set; }
        
        [Column("ColorAsInteger")]
        [NotNull]
        public long ColorAsInteger { get; set; }
        
        [Column("PredefinedIconTypeId")]
        [NotNull]
        public long PredefinedIconTypeId { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"CategoryId = {CategoryId} , ");
            sb.AppendLine($"DefaultCategoryId = {DefaultCategoryId} , ");
            sb.AppendLine($"CategoryName = {CategoryName} , ");
            sb.AppendLine($"PredefinedCategoryTypeId = {PredefinedCategoryTypeId} , ");
            sb.AppendLine($"UserId = {UserId} , ");
            sb.AppendLine($"ColorAsInteger = {ColorAsInteger} , ");
            sb.AppendLine($"PredefinedIconTypeId = {PredefinedIconTypeId} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public Category New(Category item)
        {
            if (item == null)
                return null;

            return new Category
            {
            CategoryId = item.CategoryId,
            DefaultCategoryId = item.DefaultCategoryId,
            CategoryName = item.CategoryName,
            PredefinedCategoryTypeId = item.PredefinedCategoryTypeId,
            UserId = item.UserId,
            ColorAsInteger = item.ColorAsInteger,
            PredefinedIconTypeId = item.PredefinedIconTypeId,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public Category NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("Chat")]
    [Serializable]
    public partial class Chat : IAuditableModel, INewObject<Chat>
    {
        public const string TABLE_NAME = "Chat";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_ChatId = "ChatId";
        public const string COLUMN_ChatName = "ChatName";
        public const string COLUMN_UserId = "UserId";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_ChatName = 500;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_ChatId = false;
        public const bool COLUMN_NULLABLE_ChatName = false;
        public const bool COLUMN_NULLABLE_UserId = false;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("ChatId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long ChatId { get; set; }
        
        [Column("ChatName")]
        [NotNull]
        [StringLength(500)]
        public string ChatName { get; set; }
        
        [Column("UserId")]
        [NotNull]
        public long UserId { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"ChatId = {ChatId} , ");
            sb.AppendLine($"ChatName = {ChatName} , ");
            sb.AppendLine($"UserId = {UserId} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public Chat New(Chat item)
        {
            if (item == null)
                return null;

            return new Chat
            {
            ChatId = item.ChatId,
            ChatName = item.ChatName,
            UserId = item.UserId,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public Chat NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("ChatMessage")]
    [Serializable]
    public partial class ChatMessage : IAuditableModel, INewObject<ChatMessage>
    {
        public const string TABLE_NAME = "ChatMessage";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_ChatMessageId = "ChatMessageId";
        public const string COLUMN_ChatId = "ChatId";
        public const string COLUMN_UserId = "UserId";
        public const string COLUMN_ChatMessageText = "ChatMessageText";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_ChatMessageText = 2000;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_ChatMessageId = false;
        public const bool COLUMN_NULLABLE_ChatId = false;
        public const bool COLUMN_NULLABLE_UserId = false;
        public const bool COLUMN_NULLABLE_ChatMessageText = false;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("ChatMessageId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long ChatMessageId { get; set; }
        
        [Column("ChatId")]
        [NotNull]
        public long ChatId { get; set; }
        
        [Column("UserId")]
        [NotNull]
        public long UserId { get; set; }
        
        [Column("ChatMessageText")]
        [NotNull]
        [StringLength(2000)]
        public string ChatMessageText { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"ChatMessageId = {ChatMessageId} , ");
            sb.AppendLine($"ChatId = {ChatId} , ");
            sb.AppendLine($"UserId = {UserId} , ");
            sb.AppendLine($"ChatMessageText = {ChatMessageText} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public ChatMessage New(ChatMessage item)
        {
            if (item == null)
                return null;

            return new ChatMessage
            {
            ChatMessageId = item.ChatMessageId,
            ChatId = item.ChatId,
            UserId = item.UserId,
            ChatMessageText = item.ChatMessageText,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public ChatMessage NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("ChatMessageStatus")]
    [Serializable]
    public partial class ChatMessageStatus : IAuditableModel, INewObject<ChatMessageStatus>
    {
        public const string TABLE_NAME = "ChatMessageStatus";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_ChatMessageStatusId = "ChatMessageStatusId";
        public const string COLUMN_ChatMessageId = "ChatMessageId";
        public const string COLUMN_UserId = "UserId";
        public const string COLUMN_ChatMessageViewedDate = "ChatMessageViewedDate";
        public const string COLUMN_ChatMessageSentDate = "ChatMessageSentDate";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_ChatMessageStatusId = false;
        public const bool COLUMN_NULLABLE_ChatMessageId = false;
        public const bool COLUMN_NULLABLE_UserId = false;
        public const bool COLUMN_NULLABLE_ChatMessageViewedDate = true;
        public const bool COLUMN_NULLABLE_ChatMessageSentDate = true;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("ChatMessageStatusId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long ChatMessageStatusId { get; set; }
        
        [Column("ChatMessageId")]
        [NotNull]
        public long ChatMessageId { get; set; }
        
        [Column("UserId")]
        [NotNull]
        public long UserId { get; set; }
        
        [Column("ChatMessageViewedDate")]
        public DateTime? ChatMessageViewedDate { get; set; }
        
        [Column("ChatMessageSentDate")]
        public DateTime? ChatMessageSentDate { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"ChatMessageStatusId = {ChatMessageStatusId} , ");
            sb.AppendLine($"ChatMessageId = {ChatMessageId} , ");
            sb.AppendLine($"UserId = {UserId} , ");
            sb.AppendLine($"ChatMessageViewedDate = {ChatMessageViewedDate} , ");
            sb.AppendLine($"ChatMessageSentDate = {ChatMessageSentDate} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public ChatMessageStatus New(ChatMessageStatus item)
        {
            if (item == null)
                return null;

            return new ChatMessageStatus
            {
            ChatMessageStatusId = item.ChatMessageStatusId,
            ChatMessageId = item.ChatMessageId,
            UserId = item.UserId,
            ChatMessageViewedDate = item.ChatMessageViewedDate,
            ChatMessageSentDate = item.ChatMessageSentDate,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public ChatMessageStatus NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("ChatUser")]
    [Serializable]
    public partial class ChatUser : IAuditableModel, INewObject<ChatUser>
    {
        public const string TABLE_NAME = "ChatUser";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_ChatUserId = "ChatUserId";
        public const string COLUMN_ChatId = "ChatId";
        public const string COLUMN_UserId = "UserId";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_ChatUserId = false;
        public const bool COLUMN_NULLABLE_ChatId = false;
        public const bool COLUMN_NULLABLE_UserId = false;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("ChatUserId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long ChatUserId { get; set; }
        
        [Column("ChatId")]
        [NotNull]
        public long ChatId { get; set; }
        
        [Column("UserId")]
        [NotNull]
        public long UserId { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"ChatUserId = {ChatUserId} , ");
            sb.AppendLine($"ChatId = {ChatId} , ");
            sb.AppendLine($"UserId = {UserId} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public ChatUser New(ChatUser item)
        {
            if (item == null)
                return null;

            return new ChatUser
            {
            ChatUserId = item.ChatUserId,
            ChatId = item.ChatId,
            UserId = item.UserId,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public ChatUser NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("CompanyStorageConfiguration")]
    [Serializable]
    public partial class CompanyStorageConfiguration : IAuditableModel, INewObject<CompanyStorageConfiguration>
    {
        public const string TABLE_NAME = "CompanyStorageConfiguration";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_CompanyStorageConfigurationId = "CompanyStorageConfigurationId";
        public const string COLUMN_CompanyStorageConfigurationGuid = "CompanyStorageConfigurationGuid";
        public const string COLUMN_ProfilePictureFileStorageConnectionString = "ProfilePictureFileStorageConnectionString";
        public const string COLUMN_ReportServerFileStorageConnectionString = "ReportServerFileStorageConnectionString";
        public const string COLUMN_FileAttachmentFileStorageConnectionString = "FileAttachmentFileStorageConnectionString";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_ProfilePictureFileStorageConnectionString = 500;
        public const int COLUMN_SIZE_ReportServerFileStorageConnectionString = 500;
        public const int COLUMN_SIZE_FileAttachmentFileStorageConnectionString = 500;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_CompanyStorageConfigurationId = false;
        public const bool COLUMN_NULLABLE_CompanyStorageConfigurationGuid = false;
        public const bool COLUMN_NULLABLE_ProfilePictureFileStorageConnectionString = true;
        public const bool COLUMN_NULLABLE_ReportServerFileStorageConnectionString = true;
        public const bool COLUMN_NULLABLE_FileAttachmentFileStorageConnectionString = true;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("CompanyStorageConfigurationId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long CompanyStorageConfigurationId { get; set; }
        
        [Column("CompanyStorageConfigurationGuid")]
        [NotNull]
        public System.Guid CompanyStorageConfigurationGuid { get; set; }
        
        [Column("ProfilePictureFileStorageConnectionString")]
        [StringLength(500)]
        public string ProfilePictureFileStorageConnectionString { get; set; }
        
        [Column("ReportServerFileStorageConnectionString")]
        [StringLength(500)]
        public string ReportServerFileStorageConnectionString { get; set; }
        
        [Column("FileAttachmentFileStorageConnectionString")]
        [StringLength(500)]
        public string FileAttachmentFileStorageConnectionString { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"CompanyStorageConfigurationId = {CompanyStorageConfigurationId} , ");
            sb.AppendLine($"CompanyStorageConfigurationGuid = {CompanyStorageConfigurationGuid} , ");
            sb.AppendLine($"ProfilePictureFileStorageConnectionString = {ProfilePictureFileStorageConnectionString} , ");
            sb.AppendLine($"ReportServerFileStorageConnectionString = {ReportServerFileStorageConnectionString} , ");
            sb.AppendLine($"FileAttachmentFileStorageConnectionString = {FileAttachmentFileStorageConnectionString} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public CompanyStorageConfiguration New(CompanyStorageConfiguration item)
        {
            if (item == null)
                return null;

            return new CompanyStorageConfiguration
            {
            CompanyStorageConfigurationId = item.CompanyStorageConfigurationId,
            CompanyStorageConfigurationGuid = item.CompanyStorageConfigurationGuid,
            ProfilePictureFileStorageConnectionString = item.ProfilePictureFileStorageConnectionString,
            ReportServerFileStorageConnectionString = item.ReportServerFileStorageConnectionString,
            FileAttachmentFileStorageConnectionString = item.FileAttachmentFileStorageConnectionString,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public CompanyStorageConfiguration NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("ConfigurationGroup")]
    [Serializable]
    public partial class ConfigurationGroup : IAuditableModel, INewObject<ConfigurationGroup>
    {
        public const string TABLE_NAME = "ConfigurationGroup";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_ConfigurationGroupId = "ConfigurationGroupId";
        public const string COLUMN_GroupName = "GroupName";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_GroupName = 200;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_ConfigurationGroupId = false;
        public const bool COLUMN_NULLABLE_GroupName = false;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("ConfigurationGroupId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long ConfigurationGroupId { get; set; }
        
        [Column("GroupName")]
        [NotNull]
        [StringLength(200)]
        public string GroupName { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"ConfigurationGroupId = {ConfigurationGroupId} , ");
            sb.AppendLine($"GroupName = {GroupName} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public ConfigurationGroup New(ConfigurationGroup item)
        {
            if (item == null)
                return null;

            return new ConfigurationGroup
            {
            ConfigurationGroupId = item.ConfigurationGroupId,
            GroupName = item.GroupName,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public ConfigurationGroup NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("ConfigurationItem")]
    [Serializable]
    public partial class ConfigurationItem : IAuditableModel, INewObject<ConfigurationItem>
    {
        public const string TABLE_NAME = "ConfigurationItem";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_ConfigurationItemId = "ConfigurationItemId";
        public const string COLUMN_ConfigurationGroupId = "ConfigurationGroupId";
        public const string COLUMN_Key = "Key";
        public const string COLUMN_Value = "Value";
        public const string COLUMN_DefaultValue = "DefaultValue";
        public const string COLUMN_Required = "Required";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_Key = 200;
        public const int COLUMN_SIZE_Value = 2000;
        public const int COLUMN_SIZE_DefaultValue = 2000;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_ConfigurationItemId = false;
        public const bool COLUMN_NULLABLE_ConfigurationGroupId = false;
        public const bool COLUMN_NULLABLE_Key = false;
        public const bool COLUMN_NULLABLE_Value = false;
        public const bool COLUMN_NULLABLE_DefaultValue = true;
        public const bool COLUMN_NULLABLE_Required = false;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("ConfigurationItemId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long ConfigurationItemId { get; set; }
        
        [Column("ConfigurationGroupId")]
        [NotNull]
        public long ConfigurationGroupId { get; set; }
        
        [Column("Key")]
        [NotNull]
        [StringLength(200)]
        public string Key { get; set; }
        
        [Column("Value")]
        [NotNull]
        [StringLength(2000)]
        public string Value { get; set; }
        
        [Column("DefaultValue")]
        [StringLength(2000)]
        public string DefaultValue { get; set; }
        
        [Column("Required")]
        [NotNull]
        public bool Required { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"ConfigurationItemId = {ConfigurationItemId} , ");
            sb.AppendLine($"ConfigurationGroupId = {ConfigurationGroupId} , ");
            sb.AppendLine($"Key = {Key} , ");
            sb.AppendLine($"Value = {Value} , ");
            sb.AppendLine($"DefaultValue = {DefaultValue} , ");
            sb.AppendLine($"Required = {Required} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public ConfigurationItem New(ConfigurationItem item)
        {
            if (item == null)
                return null;

            return new ConfigurationItem
            {
            ConfigurationItemId = item.ConfigurationItemId,
            ConfigurationGroupId = item.ConfigurationGroupId,
            Key = item.Key,
            Value = item.Value,
            DefaultValue = item.DefaultValue,
            Required = item.Required,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public ConfigurationItem NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("Country")]
    [Serializable]
    public partial class Country : IAuditableModel, INewObject<Country>
    {
        public const string TABLE_NAME = "Country";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_CountryId = "CountryId";
        public const string COLUMN_CountryName = "CountryName";
        public const string COLUMN_AlternateCountryName1 = "AlternateCountryName1";
        public const string COLUMN_AlternateCountryName2 = "AlternateCountryName2";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_CountryName = 255;
        public const int COLUMN_SIZE_AlternateCountryName1 = 255;
        public const int COLUMN_SIZE_AlternateCountryName2 = 255;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_CountryId = false;
        public const bool COLUMN_NULLABLE_CountryName = false;
        public const bool COLUMN_NULLABLE_AlternateCountryName1 = true;
        public const bool COLUMN_NULLABLE_AlternateCountryName2 = true;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("CountryId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long CountryId { get; set; }
        
        [Column("CountryName")]
        [NotNull]
        [StringLength(255)]
        public string CountryName { get; set; }
        
        [Column("AlternateCountryName1")]
        [StringLength(255)]
        public string AlternateCountryName1 { get; set; }
        
        [Column("AlternateCountryName2")]
        [StringLength(255)]
        public string AlternateCountryName2 { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"CountryId = {CountryId} , ");
            sb.AppendLine($"CountryName = {CountryName} , ");
            sb.AppendLine($"AlternateCountryName1 = {AlternateCountryName1} , ");
            sb.AppendLine($"AlternateCountryName2 = {AlternateCountryName2} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public Country New(Country item)
        {
            if (item == null)
                return null;

            return new Country
            {
            CountryId = item.CountryId,
            CountryName = item.CountryName,
            AlternateCountryName1 = item.AlternateCountryName1,
            AlternateCountryName2 = item.AlternateCountryName2,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public Country NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("DailyAffirmationSession")]
    [Serializable]
    public partial class DailyAffirmationSession : IAuditableModel, INewObject<DailyAffirmationSession>
    {
        public const string TABLE_NAME = "DailyAffirmationSession";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_DailyAffirmationSessionId = "DailyAffirmationSessionId";
        public const string COLUMN_DailyAffirmationSessionDate = "DailyAffirmationSessionDate";
        public const string COLUMN_DailyAffirmationSessionTimeSpentInSeconds = "DailyAffirmationSessionTimeSpentInSeconds";
        public const string COLUMN_UserId = "UserId";
        public const string COLUMN_AddedToFeed = "AddedToFeed";
        public const string COLUMN_AddedToFeedDate = "AddedToFeedDate";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_DailyAffirmationSessionId = false;
        public const bool COLUMN_NULLABLE_DailyAffirmationSessionDate = false;
        public const bool COLUMN_NULLABLE_DailyAffirmationSessionTimeSpentInSeconds = true;
        public const bool COLUMN_NULLABLE_UserId = false;
        public const bool COLUMN_NULLABLE_AddedToFeed = true;
        public const bool COLUMN_NULLABLE_AddedToFeedDate = true;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("DailyAffirmationSessionId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long DailyAffirmationSessionId { get; set; }
        
        [Column("DailyAffirmationSessionDate")]
        [NotNull]
        public DateTime DailyAffirmationSessionDate { get; set; }
        
        [Column("DailyAffirmationSessionTimeSpentInSeconds")]
        public long? DailyAffirmationSessionTimeSpentInSeconds { get; set; }
        
        [Column("UserId")]
        [NotNull]
        public long UserId { get; set; }
        
        [Column("AddedToFeed")]
        public bool? AddedToFeed { get; set; }
        
        [Column("AddedToFeedDate")]
        public DateTime? AddedToFeedDate { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"DailyAffirmationSessionId = {DailyAffirmationSessionId} , ");
            sb.AppendLine($"DailyAffirmationSessionDate = {DailyAffirmationSessionDate} , ");
            sb.AppendLine($"DailyAffirmationSessionTimeSpentInSeconds = {DailyAffirmationSessionTimeSpentInSeconds} , ");
            sb.AppendLine($"UserId = {UserId} , ");
            sb.AppendLine($"AddedToFeed = {AddedToFeed} , ");
            sb.AppendLine($"AddedToFeedDate = {AddedToFeedDate} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public DailyAffirmationSession New(DailyAffirmationSession item)
        {
            if (item == null)
                return null;

            return new DailyAffirmationSession
            {
            DailyAffirmationSessionId = item.DailyAffirmationSessionId,
            DailyAffirmationSessionDate = item.DailyAffirmationSessionDate,
            DailyAffirmationSessionTimeSpentInSeconds = item.DailyAffirmationSessionTimeSpentInSeconds,
            UserId = item.UserId,
            AddedToFeed = item.AddedToFeed,
            AddedToFeedDate = item.AddedToFeedDate,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public DailyAffirmationSession NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("DailyAffirmationSessionCategory")]
    [Serializable]
    public partial class DailyAffirmationSessionCategory : IAuditableModel, INewObject<DailyAffirmationSessionCategory>
    {
        public const string TABLE_NAME = "DailyAffirmationSessionCategory";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_DailyAffirmationSessionCategoryId = "DailyAffirmationSessionCategoryId";
        public const string COLUMN_CategoryId = "CategoryId";
        public const string COLUMN_DailyAffirmationSessionId = "DailyAffirmationSessionId";
        public const string COLUMN_DailyAffirmationSessionCategoryTimeSpentInSeconds = "DailyAffirmationSessionCategoryTimeSpentInSeconds";
        public const string COLUMN_UserId = "UserId";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_DailyAffirmationSessionCategoryId = false;
        public const bool COLUMN_NULLABLE_CategoryId = false;
        public const bool COLUMN_NULLABLE_DailyAffirmationSessionId = false;
        public const bool COLUMN_NULLABLE_DailyAffirmationSessionCategoryTimeSpentInSeconds = true;
        public const bool COLUMN_NULLABLE_UserId = false;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("DailyAffirmationSessionCategoryId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long DailyAffirmationSessionCategoryId { get; set; }
        
        [Column("CategoryId")]
        [NotNull]
        public long CategoryId { get; set; }
        
        [Column("DailyAffirmationSessionId")]
        [NotNull]
        public long DailyAffirmationSessionId { get; set; }
        
        [Column("DailyAffirmationSessionCategoryTimeSpentInSeconds")]
        public long? DailyAffirmationSessionCategoryTimeSpentInSeconds { get; set; }
        
        [Column("UserId")]
        [NotNull]
        public long UserId { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"DailyAffirmationSessionCategoryId = {DailyAffirmationSessionCategoryId} , ");
            sb.AppendLine($"CategoryId = {CategoryId} , ");
            sb.AppendLine($"DailyAffirmationSessionId = {DailyAffirmationSessionId} , ");
            sb.AppendLine($"DailyAffirmationSessionCategoryTimeSpentInSeconds = {DailyAffirmationSessionCategoryTimeSpentInSeconds} , ");
            sb.AppendLine($"UserId = {UserId} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public DailyAffirmationSessionCategory New(DailyAffirmationSessionCategory item)
        {
            if (item == null)
                return null;

            return new DailyAffirmationSessionCategory
            {
            DailyAffirmationSessionCategoryId = item.DailyAffirmationSessionCategoryId,
            CategoryId = item.CategoryId,
            DailyAffirmationSessionId = item.DailyAffirmationSessionId,
            DailyAffirmationSessionCategoryTimeSpentInSeconds = item.DailyAffirmationSessionCategoryTimeSpentInSeconds,
            UserId = item.UserId,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public DailyAffirmationSessionCategory NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("DailyAffirmationText")]
    [Serializable]
    public partial class DailyAffirmationText : IAuditableModel, INewObject<DailyAffirmationText>
    {
        public const string TABLE_NAME = "DailyAffirmationText";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_DailyAffirmationTextId = "DailyAffirmationTextId";
        public const string COLUMN_DailyAffirmationTextValue = "DailyAffirmationTextValue";
        public const string COLUMN_CategoryId = "CategoryId";
        public const string COLUMN_UserId = "UserId";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_DailyAffirmationTextValue = 255;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_DailyAffirmationTextId = false;
        public const bool COLUMN_NULLABLE_DailyAffirmationTextValue = false;
        public const bool COLUMN_NULLABLE_CategoryId = false;
        public const bool COLUMN_NULLABLE_UserId = false;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("DailyAffirmationTextId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long DailyAffirmationTextId { get; set; }
        
        [Column("DailyAffirmationTextValue")]
        [NotNull]
        [StringLength(255)]
        public string DailyAffirmationTextValue { get; set; }
        
        [Column("CategoryId")]
        [NotNull]
        public long CategoryId { get; set; }
        
        [Column("UserId")]
        [NotNull]
        public long UserId { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"DailyAffirmationTextId = {DailyAffirmationTextId} , ");
            sb.AppendLine($"DailyAffirmationTextValue = {DailyAffirmationTextValue} , ");
            sb.AppendLine($"CategoryId = {CategoryId} , ");
            sb.AppendLine($"UserId = {UserId} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public DailyAffirmationText New(DailyAffirmationText item)
        {
            if (item == null)
                return null;

            return new DailyAffirmationText
            {
            DailyAffirmationTextId = item.DailyAffirmationTextId,
            DailyAffirmationTextValue = item.DailyAffirmationTextValue,
            CategoryId = item.CategoryId,
            UserId = item.UserId,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public DailyAffirmationText NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("DailyJournal")]
    [Serializable]
    public partial class DailyJournal : IAuditableModel, INewObject<DailyJournal>
    {
        public const string TABLE_NAME = "DailyJournal";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_DailyJournalId = "DailyJournalId";
        public const string COLUMN_DailyJournalDate = "DailyJournalDate";
        public const string COLUMN_UserId = "UserId";
        public const string COLUMN_PredefinedMoodTypeId = "PredefinedMoodTypeId";
        public const string COLUMN_Gratitude1 = "Gratitude1";
        public const string COLUMN_Gratitude2 = "Gratitude2";
        public const string COLUMN_Gratitude3 = "Gratitude3";
        public const string COLUMN_Gratitude4 = "Gratitude4";
        public const string COLUMN_Gratitude5 = "Gratitude5";
        public const string COLUMN_YourReflection = "YourReflection";
        public const string COLUMN_TommorowsIntentions = "TommorowsIntentions";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_Gratitude1 = 500;
        public const int COLUMN_SIZE_Gratitude2 = 500;
        public const int COLUMN_SIZE_Gratitude3 = 500;
        public const int COLUMN_SIZE_Gratitude4 = 500;
        public const int COLUMN_SIZE_Gratitude5 = 500;
        public const int COLUMN_SIZE_YourReflection = 500;
        public const int COLUMN_SIZE_TommorowsIntentions = 500;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_DailyJournalId = false;
        public const bool COLUMN_NULLABLE_DailyJournalDate = false;
        public const bool COLUMN_NULLABLE_UserId = false;
        public const bool COLUMN_NULLABLE_PredefinedMoodTypeId = false;
        public const bool COLUMN_NULLABLE_Gratitude1 = false;
        public const bool COLUMN_NULLABLE_Gratitude2 = true;
        public const bool COLUMN_NULLABLE_Gratitude3 = true;
        public const bool COLUMN_NULLABLE_Gratitude4 = true;
        public const bool COLUMN_NULLABLE_Gratitude5 = true;
        public const bool COLUMN_NULLABLE_YourReflection = true;
        public const bool COLUMN_NULLABLE_TommorowsIntentions = true;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("DailyJournalId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long DailyJournalId { get; set; }
        
        [Column("DailyJournalDate")]
        [NotNull]
        public DateTime DailyJournalDate { get; set; }
        
        [Column("UserId")]
        [NotNull]
        public long UserId { get; set; }
        
        [Column("PredefinedMoodTypeId")]
        [NotNull]
        public long PredefinedMoodTypeId { get; set; }
        
        [Column("Gratitude1")]
        [NotNull]
        [StringLength(500)]
        public string Gratitude1 { get; set; }
        
        [Column("Gratitude2")]
        [StringLength(500)]
        public string Gratitude2 { get; set; }
        
        [Column("Gratitude3")]
        [StringLength(500)]
        public string Gratitude3 { get; set; }
        
        [Column("Gratitude4")]
        [StringLength(500)]
        public string Gratitude4 { get; set; }
        
        [Column("Gratitude5")]
        [StringLength(500)]
        public string Gratitude5 { get; set; }
        
        [Column("YourReflection")]
        [StringLength(500)]
        public string YourReflection { get; set; }
        
        [Column("TommorowsIntentions")]
        [StringLength(500)]
        public string TommorowsIntentions { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"DailyJournalId = {DailyJournalId} , ");
            sb.AppendLine($"DailyJournalDate = {DailyJournalDate} , ");
            sb.AppendLine($"UserId = {UserId} , ");
            sb.AppendLine($"PredefinedMoodTypeId = {PredefinedMoodTypeId} , ");
            sb.AppendLine($"Gratitude1 = {Gratitude1} , ");
            sb.AppendLine($"Gratitude2 = {Gratitude2} , ");
            sb.AppendLine($"Gratitude3 = {Gratitude3} , ");
            sb.AppendLine($"Gratitude4 = {Gratitude4} , ");
            sb.AppendLine($"Gratitude5 = {Gratitude5} , ");
            sb.AppendLine($"YourReflection = {YourReflection} , ");
            sb.AppendLine($"TommorowsIntentions = {TommorowsIntentions} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public DailyJournal New(DailyJournal item)
        {
            if (item == null)
                return null;

            return new DailyJournal
            {
            DailyJournalId = item.DailyJournalId,
            DailyJournalDate = item.DailyJournalDate,
            UserId = item.UserId,
            PredefinedMoodTypeId = item.PredefinedMoodTypeId,
            Gratitude1 = item.Gratitude1,
            Gratitude2 = item.Gratitude2,
            Gratitude3 = item.Gratitude3,
            Gratitude4 = item.Gratitude4,
            Gratitude5 = item.Gratitude5,
            YourReflection = item.YourReflection,
            TommorowsIntentions = item.TommorowsIntentions,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public DailyJournal NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("DefaultCategory")]
    [Serializable]
    public partial class DefaultCategory : IAuditableModel, INewObject<DefaultCategory>
    {
        public const string TABLE_NAME = "DefaultCategory";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_DefaultCategoryId = "DefaultCategoryId";
        public const string COLUMN_DefaultCategoryName = "DefaultCategoryName";
        public const string COLUMN_PredefinedCategoryTypeId = "PredefinedCategoryTypeId";
        public const string COLUMN_PredefinedIconTypeId = "PredefinedIconTypeId";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_DefaultCategoryName = 255;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_DefaultCategoryId = false;
        public const bool COLUMN_NULLABLE_DefaultCategoryName = false;
        public const bool COLUMN_NULLABLE_PredefinedCategoryTypeId = true;
        public const bool COLUMN_NULLABLE_PredefinedIconTypeId = false;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("DefaultCategoryId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long DefaultCategoryId { get; set; }
        
        [Column("DefaultCategoryName")]
        [NotNull]
        [StringLength(255)]
        public string DefaultCategoryName { get; set; }
        
        [Column("PredefinedCategoryTypeId")]
        public long? PredefinedCategoryTypeId { get; set; }
        
        [Column("PredefinedIconTypeId")]
        [NotNull]
        public long PredefinedIconTypeId { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"DefaultCategoryId = {DefaultCategoryId} , ");
            sb.AppendLine($"DefaultCategoryName = {DefaultCategoryName} , ");
            sb.AppendLine($"PredefinedCategoryTypeId = {PredefinedCategoryTypeId} , ");
            sb.AppendLine($"PredefinedIconTypeId = {PredefinedIconTypeId} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public DefaultCategory New(DefaultCategory item)
        {
            if (item == null)
                return null;

            return new DefaultCategory
            {
            DefaultCategoryId = item.DefaultCategoryId,
            DefaultCategoryName = item.DefaultCategoryName,
            PredefinedCategoryTypeId = item.PredefinedCategoryTypeId,
            PredefinedIconTypeId = item.PredefinedIconTypeId,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public DefaultCategory NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("DefaultDailyAffirmationText")]
    [Serializable]
    public partial class DefaultDailyAffirmationText : IAuditableModel, INewObject<DefaultDailyAffirmationText>
    {
        public const string TABLE_NAME = "DefaultDailyAffirmationText";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_DefaultDailyAffirmationTextId = "DefaultDailyAffirmationTextId";
        public const string COLUMN_DefaultDailyAffirmationTextName = "DefaultDailyAffirmationTextName";
        public const string COLUMN_DefaultCategoryId = "DefaultCategoryId";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_DefaultDailyAffirmationTextName = 255;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_DefaultDailyAffirmationTextId = false;
        public const bool COLUMN_NULLABLE_DefaultDailyAffirmationTextName = false;
        public const bool COLUMN_NULLABLE_DefaultCategoryId = false;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("DefaultDailyAffirmationTextId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long DefaultDailyAffirmationTextId { get; set; }
        
        [Column("DefaultDailyAffirmationTextName")]
        [NotNull]
        [StringLength(255)]
        public string DefaultDailyAffirmationTextName { get; set; }
        
        [Column("DefaultCategoryId")]
        [NotNull]
        public long DefaultCategoryId { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"DefaultDailyAffirmationTextId = {DefaultDailyAffirmationTextId} , ");
            sb.AppendLine($"DefaultDailyAffirmationTextName = {DefaultDailyAffirmationTextName} , ");
            sb.AppendLine($"DefaultCategoryId = {DefaultCategoryId} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public DefaultDailyAffirmationText New(DefaultDailyAffirmationText item)
        {
            if (item == null)
                return null;

            return new DefaultDailyAffirmationText
            {
            DefaultDailyAffirmationTextId = item.DefaultDailyAffirmationTextId,
            DefaultDailyAffirmationTextName = item.DefaultDailyAffirmationTextName,
            DefaultCategoryId = item.DefaultCategoryId,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public DefaultDailyAffirmationText NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("DefaultGoalTracker")]
    [Serializable]
    public partial class DefaultGoalTracker : IAuditableModel, INewObject<DefaultGoalTracker>
    {
        public const string TABLE_NAME = "DefaultGoalTracker";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_DefaultGoalTrackerId = "DefaultGoalTrackerId";
        public const string COLUMN_DefaultGoalTrackerText = "DefaultGoalTrackerText";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_DefaultGoalTrackerText = 255;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_DefaultGoalTrackerId = false;
        public const bool COLUMN_NULLABLE_DefaultGoalTrackerText = false;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("DefaultGoalTrackerId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long DefaultGoalTrackerId { get; set; }
        
        [Column("DefaultGoalTrackerText")]
        [NotNull]
        [StringLength(255)]
        public string DefaultGoalTrackerText { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"DefaultGoalTrackerId = {DefaultGoalTrackerId} , ");
            sb.AppendLine($"DefaultGoalTrackerText = {DefaultGoalTrackerText} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public DefaultGoalTracker New(DefaultGoalTracker item)
        {
            if (item == null)
                return null;

            return new DefaultGoalTracker
            {
            DefaultGoalTrackerId = item.DefaultGoalTrackerId,
            DefaultGoalTrackerText = item.DefaultGoalTrackerText,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public DefaultGoalTracker NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("DefaultGoalTrackerCategory")]
    [Serializable]
    public partial class DefaultGoalTrackerCategory : IAuditableModel, INewObject<DefaultGoalTrackerCategory>
    {
        public const string TABLE_NAME = "DefaultGoalTrackerCategory";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_DefaultGoalTrackerCategoryId = "DefaultGoalTrackerCategoryId";
        public const string COLUMN_DefaultCategoryId = "DefaultCategoryId";
        public const string COLUMN_DefaultGoalTrackerId = "DefaultGoalTrackerId";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_DefaultGoalTrackerCategoryId = false;
        public const bool COLUMN_NULLABLE_DefaultCategoryId = false;
        public const bool COLUMN_NULLABLE_DefaultGoalTrackerId = false;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("DefaultGoalTrackerCategoryId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long DefaultGoalTrackerCategoryId { get; set; }
        
        [Column("DefaultCategoryId")]
        [NotNull]
        public long DefaultCategoryId { get; set; }
        
        [Column("DefaultGoalTrackerId")]
        [NotNull]
        public long DefaultGoalTrackerId { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"DefaultGoalTrackerCategoryId = {DefaultGoalTrackerCategoryId} , ");
            sb.AppendLine($"DefaultCategoryId = {DefaultCategoryId} , ");
            sb.AppendLine($"DefaultGoalTrackerId = {DefaultGoalTrackerId} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public DefaultGoalTrackerCategory New(DefaultGoalTrackerCategory item)
        {
            if (item == null)
                return null;

            return new DefaultGoalTrackerCategory
            {
            DefaultGoalTrackerCategoryId = item.DefaultGoalTrackerCategoryId,
            DefaultCategoryId = item.DefaultCategoryId,
            DefaultGoalTrackerId = item.DefaultGoalTrackerId,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public DefaultGoalTrackerCategory NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("DefaultMeditationFile")]
    [Serializable]
    public partial class DefaultMeditationFile : IAuditableModel, INewObject<DefaultMeditationFile>
    {
        public const string TABLE_NAME = "DefaultMeditationFile";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_DefaultMeditationFileId = "DefaultMeditationFileId";
        public const string COLUMN_DefaultMeditationFileName = "DefaultMeditationFileName";
        public const string COLUMN_PredefinedMeditationTypeId = "PredefinedMeditationTypeId";
        public const string COLUMN_Location = "Location";
        public const string COLUMN_FileName = "FileName";
        public const string COLUMN_FileSize = "FileSize";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_DefaultMeditationFileName = 255;
        public const int COLUMN_SIZE_Location = 1024;
        public const int COLUMN_SIZE_FileName = 255;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_DefaultMeditationFileId = false;
        public const bool COLUMN_NULLABLE_DefaultMeditationFileName = false;
        public const bool COLUMN_NULLABLE_PredefinedMeditationTypeId = false;
        public const bool COLUMN_NULLABLE_Location = true;
        public const bool COLUMN_NULLABLE_FileName = false;
        public const bool COLUMN_NULLABLE_FileSize = false;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("DefaultMeditationFileId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long DefaultMeditationFileId { get; set; }
        
        [Column("DefaultMeditationFileName")]
        [NotNull]
        [StringLength(255)]
        public string DefaultMeditationFileName { get; set; }
        
        [Column("PredefinedMeditationTypeId")]
        [NotNull]
        public long PredefinedMeditationTypeId { get; set; }
        
        [Column("Location")]
        [StringLength(1024)]
        public string Location { get; set; }
        
        [Column("FileName")]
        [NotNull]
        [StringLength(255)]
        public string FileName { get; set; }
        
        [Column("FileSize")]
        [NotNull]
        public long FileSize { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"DefaultMeditationFileId = {DefaultMeditationFileId} , ");
            sb.AppendLine($"DefaultMeditationFileName = {DefaultMeditationFileName} , ");
            sb.AppendLine($"PredefinedMeditationTypeId = {PredefinedMeditationTypeId} , ");
            sb.AppendLine($"Location = {Location} , ");
            sb.AppendLine($"FileName = {FileName} , ");
            sb.AppendLine($"FileSize = {FileSize} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public DefaultMeditationFile New(DefaultMeditationFile item)
        {
            if (item == null)
                return null;

            return new DefaultMeditationFile
            {
            DefaultMeditationFileId = item.DefaultMeditationFileId,
            DefaultMeditationFileName = item.DefaultMeditationFileName,
            PredefinedMeditationTypeId = item.PredefinedMeditationTypeId,
            Location = item.Location,
            FileName = item.FileName,
            FileSize = item.FileSize,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public DefaultMeditationFile NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("DefaultUserTypePermission")]
    [Serializable]
    public partial class DefaultUserTypePermission : IAuditableModel, INewObject<DefaultUserTypePermission>
    {
        public const string TABLE_NAME = "DefaultUserTypePermission";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_DefaultUserTypePermissionId = "DefaultUserTypePermissionId";
        public const string COLUMN_PredefinedUserTypeId = "PredefinedUserTypeId";
        public const string COLUMN_PredefinedUserPermissionTypeId = "PredefinedUserPermissionTypeId";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_DefaultUserTypePermissionId = false;
        public const bool COLUMN_NULLABLE_PredefinedUserTypeId = false;
        public const bool COLUMN_NULLABLE_PredefinedUserPermissionTypeId = false;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("DefaultUserTypePermissionId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long DefaultUserTypePermissionId { get; set; }
        
        [Column("PredefinedUserTypeId")]
        [NotNull]
        public long PredefinedUserTypeId { get; set; }
        
        [Column("PredefinedUserPermissionTypeId")]
        [NotNull]
        public long PredefinedUserPermissionTypeId { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"DefaultUserTypePermissionId = {DefaultUserTypePermissionId} , ");
            sb.AppendLine($"PredefinedUserTypeId = {PredefinedUserTypeId} , ");
            sb.AppendLine($"PredefinedUserPermissionTypeId = {PredefinedUserPermissionTypeId} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public DefaultUserTypePermission New(DefaultUserTypePermission item)
        {
            if (item == null)
                return null;

            return new DefaultUserTypePermission
            {
            DefaultUserTypePermissionId = item.DefaultUserTypePermissionId,
            PredefinedUserTypeId = item.PredefinedUserTypeId,
            PredefinedUserPermissionTypeId = item.PredefinedUserPermissionTypeId,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public DefaultUserTypePermission NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("DefaultVisionBoardPicture")]
    [Serializable]
    public partial class DefaultVisionBoardPicture : IAuditableModel, INewObject<DefaultVisionBoardPicture>
    {
        public const string TABLE_NAME = "DefaultVisionBoardPicture";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_DefaultVisionBoardPictureId = "DefaultVisionBoardPictureId";
        public const string COLUMN_Title = "Title";
        public const string COLUMN_Location = "Location";
        public const string COLUMN_FileName = "FileName";
        public const string COLUMN_FileSize = "FileSize";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_Title = 100;
        public const int COLUMN_SIZE_Location = 1024;
        public const int COLUMN_SIZE_FileName = 255;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_DefaultVisionBoardPictureId = false;
        public const bool COLUMN_NULLABLE_Title = true;
        public const bool COLUMN_NULLABLE_Location = true;
        public const bool COLUMN_NULLABLE_FileName = false;
        public const bool COLUMN_NULLABLE_FileSize = false;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("DefaultVisionBoardPictureId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long DefaultVisionBoardPictureId { get; set; }
        
        [Column("Title")]
        [StringLength(100)]
        public string Title { get; set; }
        
        [Column("Location")]
        [StringLength(1024)]
        public string Location { get; set; }
        
        [Column("FileName")]
        [NotNull]
        [StringLength(255)]
        public string FileName { get; set; }
        
        [Column("FileSize")]
        [NotNull]
        public long FileSize { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"DefaultVisionBoardPictureId = {DefaultVisionBoardPictureId} , ");
            sb.AppendLine($"Title = {Title} , ");
            sb.AppendLine($"Location = {Location} , ");
            sb.AppendLine($"FileName = {FileName} , ");
            sb.AppendLine($"FileSize = {FileSize} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public DefaultVisionBoardPicture New(DefaultVisionBoardPicture item)
        {
            if (item == null)
                return null;

            return new DefaultVisionBoardPicture
            {
            DefaultVisionBoardPictureId = item.DefaultVisionBoardPictureId,
            Title = item.Title,
            Location = item.Location,
            FileName = item.FileName,
            FileSize = item.FileSize,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public DefaultVisionBoardPicture NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("EnumGroup")]
    [Serializable]
    public partial class EnumGroup : IAuditableModel, INewObject<EnumGroup>
    {
        public const string TABLE_NAME = "EnumGroup";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_EnumGroupId = "EnumGroupId";
        public const string COLUMN_EnumGroupName = "EnumGroupName";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_EnumGroupName = 100;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_EnumGroupId = false;
        public const bool COLUMN_NULLABLE_EnumGroupName = false;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("EnumGroupId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long EnumGroupId { get; set; }
        
        [Column("EnumGroupName")]
        [NotNull]
        [StringLength(100)]
        public string EnumGroupName { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"EnumGroupId = {EnumGroupId} , ");
            sb.AppendLine($"EnumGroupName = {EnumGroupName} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public EnumGroup New(EnumGroup item)
        {
            if (item == null)
                return null;

            return new EnumGroup
            {
            EnumGroupId = item.EnumGroupId,
            EnumGroupName = item.EnumGroupName,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public EnumGroup NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("EnumType")]
    [Serializable]
    public partial class EnumType : IAuditableModel, INewObject<EnumType>
    {
        public const string TABLE_NAME = "EnumType";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_EnumTypeId = "EnumTypeId";
        public const string COLUMN_InternalValue = "InternalValue";
        public const string COLUMN_EnumGroupId = "EnumGroupId";
        public const string COLUMN_ParentEnumTypeId = "ParentEnumTypeId";
        public const string COLUMN_EnumTypeName = "EnumTypeName";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_EnumTypeName = 100;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_EnumTypeId = false;
        public const bool COLUMN_NULLABLE_InternalValue = false;
        public const bool COLUMN_NULLABLE_EnumGroupId = false;
        public const bool COLUMN_NULLABLE_ParentEnumTypeId = true;
        public const bool COLUMN_NULLABLE_EnumTypeName = false;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("EnumTypeId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long EnumTypeId { get; set; }
        
        [Column("InternalValue")]
        [NotNull]
        public long InternalValue { get; set; }
        
        [Column("EnumGroupId")]
        [NotNull]
        public long EnumGroupId { get; set; }
        
        [Column("ParentEnumTypeId")]
        public long? ParentEnumTypeId { get; set; }
        
        [Column("EnumTypeName")]
        [NotNull]
        [StringLength(100)]
        public string EnumTypeName { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"EnumTypeId = {EnumTypeId} , ");
            sb.AppendLine($"InternalValue = {InternalValue} , ");
            sb.AppendLine($"EnumGroupId = {EnumGroupId} , ");
            sb.AppendLine($"ParentEnumTypeId = {ParentEnumTypeId} , ");
            sb.AppendLine($"EnumTypeName = {EnumTypeName} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public EnumType New(EnumType item)
        {
            if (item == null)
                return null;

            return new EnumType
            {
            EnumTypeId = item.EnumTypeId,
            InternalValue = item.InternalValue,
            EnumGroupId = item.EnumGroupId,
            ParentEnumTypeId = item.ParentEnumTypeId,
            EnumTypeName = item.EnumTypeName,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public EnumType NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("Feed")]
    [Serializable]
    public partial class Feed : IAuditableModel, INewObject<Feed>
    {
        public const string TABLE_NAME = "Feed";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_FeedId = "FeedId";
        public const string COLUMN_FeedGuid = "FeedGuid";
        public const string COLUMN_UserId = "UserId";
        public const string COLUMN_DailyAffirmationSessionId = "DailyAffirmationSessionId";
        public const string COLUMN_DailyAffirmationSessionDate = "DailyAffirmationSessionDate";
        public const string COLUMN_DailyAffirmationTimeSpentInSeconds = "DailyAffirmationTimeSpentInSeconds";
        public const string COLUMN_VisionBoardSessionId = "VisionBoardSessionId";
        public const string COLUMN_VisionBoardSessionDate = "VisionBoardSessionDate";
        public const string COLUMN_VisionBoardSessionTimeSpentInSeconds = "VisionBoardSessionTimeSpentInSeconds";
        public const string COLUMN_MeditationSessionId = "MeditationSessionId";
        public const string COLUMN_MeditationSessionDate = "MeditationSessionDate";
        public const string COLUMN_MeditationSessionTimeSpentInSeconds = "MeditationSessionTimeSpentInSeconds";
        public const string COLUMN_MeditationFileName = "MeditationFileName";
        public const string COLUMN_PredefinedMeditationTypeId = "PredefinedMeditationTypeId";
        public const string COLUMN_GoalTrackerId = "GoalTrackerId";
        public const string COLUMN_GoalTrackerName = "GoalTrackerName";
        public const string COLUMN_PredefinedGoalStatusTypeId = "PredefinedGoalStatusTypeId";
        public const string COLUMN_GoalTrackerStartDate = "GoalTrackerStartDate";
        public const string COLUMN_GoalTrackerEndDate = "GoalTrackerEndDate";
        public const string COLUMN_GoalTrackerCompletedDate = "GoalTrackerCompletedDate";
        public const string COLUMN_GoalTrackerMileStoneId = "GoalTrackerMileStoneId";
        public const string COLUMN_GoalTrackerMileStoneName = "GoalTrackerMileStoneName";
        public const string COLUMN_GoalTrackerMileStoneStartDate = "GoalTrackerMileStoneStartDate";
        public const string COLUMN_GoalTrackerMileStoneEndDate = "GoalTrackerMileStoneEndDate";
        public const string COLUMN_GoalTrackerMileStoneCompletedDate = "GoalTrackerMileStoneCompletedDate";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_MeditationFileName = 255;
        public const int COLUMN_SIZE_GoalTrackerName = 255;
        public const int COLUMN_SIZE_GoalTrackerMileStoneName = 255;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_FeedId = false;
        public const bool COLUMN_NULLABLE_FeedGuid = false;
        public const bool COLUMN_NULLABLE_UserId = false;
        public const bool COLUMN_NULLABLE_DailyAffirmationSessionId = true;
        public const bool COLUMN_NULLABLE_DailyAffirmationSessionDate = true;
        public const bool COLUMN_NULLABLE_DailyAffirmationTimeSpentInSeconds = true;
        public const bool COLUMN_NULLABLE_VisionBoardSessionId = true;
        public const bool COLUMN_NULLABLE_VisionBoardSessionDate = true;
        public const bool COLUMN_NULLABLE_VisionBoardSessionTimeSpentInSeconds = true;
        public const bool COLUMN_NULLABLE_MeditationSessionId = true;
        public const bool COLUMN_NULLABLE_MeditationSessionDate = true;
        public const bool COLUMN_NULLABLE_MeditationSessionTimeSpentInSeconds = true;
        public const bool COLUMN_NULLABLE_MeditationFileName = true;
        public const bool COLUMN_NULLABLE_PredefinedMeditationTypeId = true;
        public const bool COLUMN_NULLABLE_GoalTrackerId = true;
        public const bool COLUMN_NULLABLE_GoalTrackerName = true;
        public const bool COLUMN_NULLABLE_PredefinedGoalStatusTypeId = true;
        public const bool COLUMN_NULLABLE_GoalTrackerStartDate = true;
        public const bool COLUMN_NULLABLE_GoalTrackerEndDate = true;
        public const bool COLUMN_NULLABLE_GoalTrackerCompletedDate = true;
        public const bool COLUMN_NULLABLE_GoalTrackerMileStoneId = true;
        public const bool COLUMN_NULLABLE_GoalTrackerMileStoneName = true;
        public const bool COLUMN_NULLABLE_GoalTrackerMileStoneStartDate = true;
        public const bool COLUMN_NULLABLE_GoalTrackerMileStoneEndDate = true;
        public const bool COLUMN_NULLABLE_GoalTrackerMileStoneCompletedDate = true;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("FeedId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long FeedId { get; set; }
        
        [Column("FeedGuid")]
        [NotNull]
        public System.Guid FeedGuid { get; set; }
        
        [Column("UserId")]
        [NotNull]
        public long UserId { get; set; }
        
        [Column("DailyAffirmationSessionId")]
        public long? DailyAffirmationSessionId { get; set; }
        
        [Column("DailyAffirmationSessionDate")]
        public DateTime? DailyAffirmationSessionDate { get; set; }
        
        [Column("DailyAffirmationTimeSpentInSeconds")]
        public long? DailyAffirmationTimeSpentInSeconds { get; set; }
        
        [Column("VisionBoardSessionId")]
        public long? VisionBoardSessionId { get; set; }
        
        [Column("VisionBoardSessionDate")]
        public DateTime? VisionBoardSessionDate { get; set; }
        
        [Column("VisionBoardSessionTimeSpentInSeconds")]
        public long? VisionBoardSessionTimeSpentInSeconds { get; set; }
        
        [Column("MeditationSessionId")]
        public long? MeditationSessionId { get; set; }
        
        [Column("MeditationSessionDate")]
        public DateTime? MeditationSessionDate { get; set; }
        
        [Column("MeditationSessionTimeSpentInSeconds")]
        public long? MeditationSessionTimeSpentInSeconds { get; set; }
        
        [Column("MeditationFileName")]
        [StringLength(255)]
        public string MeditationFileName { get; set; }
        
        [Column("PredefinedMeditationTypeId")]
        public long? PredefinedMeditationTypeId { get; set; }
        
        [Column("GoalTrackerId")]
        public long? GoalTrackerId { get; set; }
        
        [Column("GoalTrackerName")]
        [StringLength(255)]
        public string GoalTrackerName { get; set; }
        
        [Column("PredefinedGoalStatusTypeId")]
        public long? PredefinedGoalStatusTypeId { get; set; }
        
        [Column("GoalTrackerStartDate")]
        public DateTime? GoalTrackerStartDate { get; set; }
        
        [Column("GoalTrackerEndDate")]
        public DateTime? GoalTrackerEndDate { get; set; }
        
        [Column("GoalTrackerCompletedDate")]
        public DateTime? GoalTrackerCompletedDate { get; set; }
        
        [Column("GoalTrackerMileStoneId")]
        public long? GoalTrackerMileStoneId { get; set; }
        
        [Column("GoalTrackerMileStoneName")]
        [StringLength(255)]
        public string GoalTrackerMileStoneName { get; set; }
        
        [Column("GoalTrackerMileStoneStartDate")]
        public DateTime? GoalTrackerMileStoneStartDate { get; set; }
        
        [Column("GoalTrackerMileStoneEndDate")]
        public DateTime? GoalTrackerMileStoneEndDate { get; set; }
        
        [Column("GoalTrackerMileStoneCompletedDate")]
        public DateTime? GoalTrackerMileStoneCompletedDate { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"FeedId = {FeedId} , ");
            sb.AppendLine($"FeedGuid = {FeedGuid} , ");
            sb.AppendLine($"UserId = {UserId} , ");
            sb.AppendLine($"DailyAffirmationSessionId = {DailyAffirmationSessionId} , ");
            sb.AppendLine($"DailyAffirmationSessionDate = {DailyAffirmationSessionDate} , ");
            sb.AppendLine($"DailyAffirmationTimeSpentInSeconds = {DailyAffirmationTimeSpentInSeconds} , ");
            sb.AppendLine($"VisionBoardSessionId = {VisionBoardSessionId} , ");
            sb.AppendLine($"VisionBoardSessionDate = {VisionBoardSessionDate} , ");
            sb.AppendLine($"VisionBoardSessionTimeSpentInSeconds = {VisionBoardSessionTimeSpentInSeconds} , ");
            sb.AppendLine($"MeditationSessionId = {MeditationSessionId} , ");
            sb.AppendLine($"MeditationSessionDate = {MeditationSessionDate} , ");
            sb.AppendLine($"MeditationSessionTimeSpentInSeconds = {MeditationSessionTimeSpentInSeconds} , ");
            sb.AppendLine($"MeditationFileName = {MeditationFileName} , ");
            sb.AppendLine($"PredefinedMeditationTypeId = {PredefinedMeditationTypeId} , ");
            sb.AppendLine($"GoalTrackerId = {GoalTrackerId} , ");
            sb.AppendLine($"GoalTrackerName = {GoalTrackerName} , ");
            sb.AppendLine($"PredefinedGoalStatusTypeId = {PredefinedGoalStatusTypeId} , ");
            sb.AppendLine($"GoalTrackerStartDate = {GoalTrackerStartDate} , ");
            sb.AppendLine($"GoalTrackerEndDate = {GoalTrackerEndDate} , ");
            sb.AppendLine($"GoalTrackerCompletedDate = {GoalTrackerCompletedDate} , ");
            sb.AppendLine($"GoalTrackerMileStoneId = {GoalTrackerMileStoneId} , ");
            sb.AppendLine($"GoalTrackerMileStoneName = {GoalTrackerMileStoneName} , ");
            sb.AppendLine($"GoalTrackerMileStoneStartDate = {GoalTrackerMileStoneStartDate} , ");
            sb.AppendLine($"GoalTrackerMileStoneEndDate = {GoalTrackerMileStoneEndDate} , ");
            sb.AppendLine($"GoalTrackerMileStoneCompletedDate = {GoalTrackerMileStoneCompletedDate} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public Feed New(Feed item)
        {
            if (item == null)
                return null;

            return new Feed
            {
            FeedId = item.FeedId,
            FeedGuid = item.FeedGuid,
            UserId = item.UserId,
            DailyAffirmationSessionId = item.DailyAffirmationSessionId,
            DailyAffirmationSessionDate = item.DailyAffirmationSessionDate,
            DailyAffirmationTimeSpentInSeconds = item.DailyAffirmationTimeSpentInSeconds,
            VisionBoardSessionId = item.VisionBoardSessionId,
            VisionBoardSessionDate = item.VisionBoardSessionDate,
            VisionBoardSessionTimeSpentInSeconds = item.VisionBoardSessionTimeSpentInSeconds,
            MeditationSessionId = item.MeditationSessionId,
            MeditationSessionDate = item.MeditationSessionDate,
            MeditationSessionTimeSpentInSeconds = item.MeditationSessionTimeSpentInSeconds,
            MeditationFileName = item.MeditationFileName,
            PredefinedMeditationTypeId = item.PredefinedMeditationTypeId,
            GoalTrackerId = item.GoalTrackerId,
            GoalTrackerName = item.GoalTrackerName,
            PredefinedGoalStatusTypeId = item.PredefinedGoalStatusTypeId,
            GoalTrackerStartDate = item.GoalTrackerStartDate,
            GoalTrackerEndDate = item.GoalTrackerEndDate,
            GoalTrackerCompletedDate = item.GoalTrackerCompletedDate,
            GoalTrackerMileStoneId = item.GoalTrackerMileStoneId,
            GoalTrackerMileStoneName = item.GoalTrackerMileStoneName,
            GoalTrackerMileStoneStartDate = item.GoalTrackerMileStoneStartDate,
            GoalTrackerMileStoneEndDate = item.GoalTrackerMileStoneEndDate,
            GoalTrackerMileStoneCompletedDate = item.GoalTrackerMileStoneCompletedDate,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public Feed NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("FileAttachment")]
    [Serializable]
    public partial class FileAttachment : IAuditableModel, INewObject<FileAttachment>
    {
        public const string TABLE_NAME = "FileAttachment";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_FileAttachmentId = "FileAttachmentId";
        public const string COLUMN_FileAttachmentFolderId = "FileAttachmentFolderId";
        public const string COLUMN_FileAttachmentFileName = "FileAttachmentFileName";
        public const string COLUMN_FileAttachmentFileLocation = "FileAttachmentFileLocation";
        public const string COLUMN_FileAttachmentPreviewFileLocation = "FileAttachmentPreviewFileLocation";
        public const string COLUMN_FileAttachmentHasPreview = "FileAttachmentHasPreview";
        public const string COLUMN_FileAttachmentFileSizeInBytes = "FileAttachmentFileSizeInBytes";
        public const string COLUMN_Description = "Description";
        public const string COLUMN_UserId = "UserId";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_FileAttachmentFileName = 128;
        public const int COLUMN_SIZE_FileAttachmentFileLocation = 1024;
        public const int COLUMN_SIZE_FileAttachmentPreviewFileLocation = 1024;
        public const int COLUMN_SIZE_Description = 200;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_FileAttachmentId = false;
        public const bool COLUMN_NULLABLE_FileAttachmentFolderId = true;
        public const bool COLUMN_NULLABLE_FileAttachmentFileName = false;
        public const bool COLUMN_NULLABLE_FileAttachmentFileLocation = false;
        public const bool COLUMN_NULLABLE_FileAttachmentPreviewFileLocation = true;
        public const bool COLUMN_NULLABLE_FileAttachmentHasPreview = false;
        public const bool COLUMN_NULLABLE_FileAttachmentFileSizeInBytes = false;
        public const bool COLUMN_NULLABLE_Description = true;
        public const bool COLUMN_NULLABLE_UserId = false;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("FileAttachmentId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long FileAttachmentId { get; set; }
        
        [Column("FileAttachmentFolderId")]
        public long? FileAttachmentFolderId { get; set; }
        
        [Column("FileAttachmentFileName")]
        [NotNull]
        [StringLength(128)]
        public string FileAttachmentFileName { get; set; }
        
        [Column("FileAttachmentFileLocation")]
        [NotNull]
        [StringLength(1024)]
        public string FileAttachmentFileLocation { get; set; }
        
        [Column("FileAttachmentPreviewFileLocation")]
        [StringLength(1024)]
        public string FileAttachmentPreviewFileLocation { get; set; }
        
        [Column("FileAttachmentHasPreview")]
        [NotNull]
        public bool FileAttachmentHasPreview { get; set; }
        
        [Column("FileAttachmentFileSizeInBytes")]
        [NotNull]
        public long FileAttachmentFileSizeInBytes { get; set; }
        
        [Column("Description")]
        [StringLength(200)]
        public string Description { get; set; }
        
        [Column("UserId")]
        [NotNull]
        public long UserId { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"FileAttachmentId = {FileAttachmentId} , ");
            sb.AppendLine($"FileAttachmentFolderId = {FileAttachmentFolderId} , ");
            sb.AppendLine($"FileAttachmentFileName = {FileAttachmentFileName} , ");
            sb.AppendLine($"FileAttachmentFileLocation = {FileAttachmentFileLocation} , ");
            sb.AppendLine($"FileAttachmentPreviewFileLocation = {FileAttachmentPreviewFileLocation} , ");
            sb.AppendLine($"FileAttachmentHasPreview = {FileAttachmentHasPreview} , ");
            sb.AppendLine($"FileAttachmentFileSizeInBytes = {FileAttachmentFileSizeInBytes} , ");
            sb.AppendLine($"Description = {Description} , ");
            sb.AppendLine($"UserId = {UserId} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public FileAttachment New(FileAttachment item)
        {
            if (item == null)
                return null;

            return new FileAttachment
            {
            FileAttachmentId = item.FileAttachmentId,
            FileAttachmentFolderId = item.FileAttachmentFolderId,
            FileAttachmentFileName = item.FileAttachmentFileName,
            FileAttachmentFileLocation = item.FileAttachmentFileLocation,
            FileAttachmentPreviewFileLocation = item.FileAttachmentPreviewFileLocation,
            FileAttachmentHasPreview = item.FileAttachmentHasPreview,
            FileAttachmentFileSizeInBytes = item.FileAttachmentFileSizeInBytes,
            Description = item.Description,
            UserId = item.UserId,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public FileAttachment NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("FileAttachmentFolder")]
    [Serializable]
    public partial class FileAttachmentFolder : IAuditableModel, INewObject<FileAttachmentFolder>
    {
        public const string TABLE_NAME = "FileAttachmentFolder";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_FileAttachmentFolderId = "FileAttachmentFolderId";
        public const string COLUMN_FileAttachmentFolderName = "FileAttachmentFolderName";
        public const string COLUMN_ParentFileAttachmentFolderId = "ParentFileAttachmentFolderId";
        public const string COLUMN_Description = "Description";
        public const string COLUMN_UserId = "UserId";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_FileAttachmentFolderName = 128;
        public const int COLUMN_SIZE_Description = 200;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_FileAttachmentFolderId = false;
        public const bool COLUMN_NULLABLE_FileAttachmentFolderName = false;
        public const bool COLUMN_NULLABLE_ParentFileAttachmentFolderId = true;
        public const bool COLUMN_NULLABLE_Description = true;
        public const bool COLUMN_NULLABLE_UserId = false;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("FileAttachmentFolderId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long FileAttachmentFolderId { get; set; }
        
        [Column("FileAttachmentFolderName")]
        [NotNull]
        [StringLength(128)]
        public string FileAttachmentFolderName { get; set; }
        
        [Column("ParentFileAttachmentFolderId")]
        public long? ParentFileAttachmentFolderId { get; set; }
        
        [Column("Description")]
        [StringLength(200)]
        public string Description { get; set; }
        
        [Column("UserId")]
        [NotNull]
        public long UserId { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"FileAttachmentFolderId = {FileAttachmentFolderId} , ");
            sb.AppendLine($"FileAttachmentFolderName = {FileAttachmentFolderName} , ");
            sb.AppendLine($"ParentFileAttachmentFolderId = {ParentFileAttachmentFolderId} , ");
            sb.AppendLine($"Description = {Description} , ");
            sb.AppendLine($"UserId = {UserId} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public FileAttachmentFolder New(FileAttachmentFolder item)
        {
            if (item == null)
                return null;

            return new FileAttachmentFolder
            {
            FileAttachmentFolderId = item.FileAttachmentFolderId,
            FileAttachmentFolderName = item.FileAttachmentFolderName,
            ParentFileAttachmentFolderId = item.ParentFileAttachmentFolderId,
            Description = item.Description,
            UserId = item.UserId,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public FileAttachmentFolder NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("GoalTracker")]
    [Serializable]
    public partial class GoalTracker : IAuditableModel, INewObject<GoalTracker>
    {
        public const string TABLE_NAME = "GoalTracker";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_GoalTrackerId = "GoalTrackerId";
        public const string COLUMN_GoalTrackerName = "GoalTrackerName";
        public const string COLUMN_PredefinedGoalStatusTypeId = "PredefinedGoalStatusTypeId";
        public const string COLUMN_PredefinedGoalPriorityTypeId = "PredefinedGoalPriorityTypeId";
        public const string COLUMN_GoalTrackerStartDate = "GoalTrackerStartDate";
        public const string COLUMN_GoalTrackerEndDate = "GoalTrackerEndDate";
        public const string COLUMN_GoalTrackerCompletedDate = "GoalTrackerCompletedDate";
        public const string COLUMN_UserId = "UserId";
        public const string COLUMN_AddedToFeed = "AddedToFeed";
        public const string COLUMN_AddedToFeedDate = "AddedToFeedDate";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_GoalTrackerName = 255;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_GoalTrackerId = false;
        public const bool COLUMN_NULLABLE_GoalTrackerName = false;
        public const bool COLUMN_NULLABLE_PredefinedGoalStatusTypeId = false;
        public const bool COLUMN_NULLABLE_PredefinedGoalPriorityTypeId = false;
        public const bool COLUMN_NULLABLE_GoalTrackerStartDate = true;
        public const bool COLUMN_NULLABLE_GoalTrackerEndDate = true;
        public const bool COLUMN_NULLABLE_GoalTrackerCompletedDate = true;
        public const bool COLUMN_NULLABLE_UserId = false;
        public const bool COLUMN_NULLABLE_AddedToFeed = true;
        public const bool COLUMN_NULLABLE_AddedToFeedDate = true;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("GoalTrackerId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long GoalTrackerId { get; set; }
        
        [Column("GoalTrackerName")]
        [NotNull]
        [StringLength(255)]
        public string GoalTrackerName { get; set; }
        
        [Column("PredefinedGoalStatusTypeId")]
        [NotNull]
        public long PredefinedGoalStatusTypeId { get; set; }
        
        [Column("PredefinedGoalPriorityTypeId")]
        [NotNull]
        public long PredefinedGoalPriorityTypeId { get; set; }
        
        [Column("GoalTrackerStartDate")]
        public DateTime? GoalTrackerStartDate { get; set; }
        
        [Column("GoalTrackerEndDate")]
        public DateTime? GoalTrackerEndDate { get; set; }
        
        [Column("GoalTrackerCompletedDate")]
        public DateTime? GoalTrackerCompletedDate { get; set; }
        
        [Column("UserId")]
        [NotNull]
        public long UserId { get; set; }
        
        [Column("AddedToFeed")]
        public bool? AddedToFeed { get; set; }
        
        [Column("AddedToFeedDate")]
        public DateTime? AddedToFeedDate { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"GoalTrackerId = {GoalTrackerId} , ");
            sb.AppendLine($"GoalTrackerName = {GoalTrackerName} , ");
            sb.AppendLine($"PredefinedGoalStatusTypeId = {PredefinedGoalStatusTypeId} , ");
            sb.AppendLine($"PredefinedGoalPriorityTypeId = {PredefinedGoalPriorityTypeId} , ");
            sb.AppendLine($"GoalTrackerStartDate = {GoalTrackerStartDate} , ");
            sb.AppendLine($"GoalTrackerEndDate = {GoalTrackerEndDate} , ");
            sb.AppendLine($"GoalTrackerCompletedDate = {GoalTrackerCompletedDate} , ");
            sb.AppendLine($"UserId = {UserId} , ");
            sb.AppendLine($"AddedToFeed = {AddedToFeed} , ");
            sb.AppendLine($"AddedToFeedDate = {AddedToFeedDate} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public GoalTracker New(GoalTracker item)
        {
            if (item == null)
                return null;

            return new GoalTracker
            {
            GoalTrackerId = item.GoalTrackerId,
            GoalTrackerName = item.GoalTrackerName,
            PredefinedGoalStatusTypeId = item.PredefinedGoalStatusTypeId,
            PredefinedGoalPriorityTypeId = item.PredefinedGoalPriorityTypeId,
            GoalTrackerStartDate = item.GoalTrackerStartDate,
            GoalTrackerEndDate = item.GoalTrackerEndDate,
            GoalTrackerCompletedDate = item.GoalTrackerCompletedDate,
            UserId = item.UserId,
            AddedToFeed = item.AddedToFeed,
            AddedToFeedDate = item.AddedToFeedDate,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public GoalTracker NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("GoalTrackerCategory")]
    [Serializable]
    public partial class GoalTrackerCategory : IAuditableModel, INewObject<GoalTrackerCategory>
    {
        public const string TABLE_NAME = "GoalTrackerCategory";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_GoalTrackerCategoryId = "GoalTrackerCategoryId";
        public const string COLUMN_CategoryId = "CategoryId";
        public const string COLUMN_GoalTrackerId = "GoalTrackerId";
        public const string COLUMN_UserId = "UserId";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_GoalTrackerCategoryId = false;
        public const bool COLUMN_NULLABLE_CategoryId = false;
        public const bool COLUMN_NULLABLE_GoalTrackerId = false;
        public const bool COLUMN_NULLABLE_UserId = false;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("GoalTrackerCategoryId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long GoalTrackerCategoryId { get; set; }
        
        [Column("CategoryId")]
        [NotNull]
        public long CategoryId { get; set; }
        
        [Column("GoalTrackerId")]
        [NotNull]
        public long GoalTrackerId { get; set; }
        
        [Column("UserId")]
        [NotNull]
        public long UserId { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"GoalTrackerCategoryId = {GoalTrackerCategoryId} , ");
            sb.AppendLine($"CategoryId = {CategoryId} , ");
            sb.AppendLine($"GoalTrackerId = {GoalTrackerId} , ");
            sb.AppendLine($"UserId = {UserId} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public GoalTrackerCategory New(GoalTrackerCategory item)
        {
            if (item == null)
                return null;

            return new GoalTrackerCategory
            {
            GoalTrackerCategoryId = item.GoalTrackerCategoryId,
            CategoryId = item.CategoryId,
            GoalTrackerId = item.GoalTrackerId,
            UserId = item.UserId,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public GoalTrackerCategory NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("GoalTrackerMileStone")]
    [Serializable]
    public partial class GoalTrackerMileStone : IAuditableModel, INewObject<GoalTrackerMileStone>
    {
        public const string TABLE_NAME = "GoalTrackerMileStone";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_GoalTrackerMileStoneId = "GoalTrackerMileStoneId";
        public const string COLUMN_GoalTrackerId = "GoalTrackerId";
        public const string COLUMN_GoalTrackerMileStoneName = "GoalTrackerMileStoneName";
        public const string COLUMN_GoalTrackerMileStoneStartDate = "GoalTrackerMileStoneStartDate";
        public const string COLUMN_GoalTrackerMileStoneEndDate = "GoalTrackerMileStoneEndDate";
        public const string COLUMN_PredefinedGoalStatusTypeId = "PredefinedGoalStatusTypeId";
        public const string COLUMN_GoalTrackerMileStoneCompletedDate = "GoalTrackerMileStoneCompletedDate";
        public const string COLUMN_UserId = "UserId";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_GoalTrackerMileStoneName = 255;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_GoalTrackerMileStoneId = false;
        public const bool COLUMN_NULLABLE_GoalTrackerId = false;
        public const bool COLUMN_NULLABLE_GoalTrackerMileStoneName = false;
        public const bool COLUMN_NULLABLE_GoalTrackerMileStoneStartDate = true;
        public const bool COLUMN_NULLABLE_GoalTrackerMileStoneEndDate = true;
        public const bool COLUMN_NULLABLE_PredefinedGoalStatusTypeId = false;
        public const bool COLUMN_NULLABLE_GoalTrackerMileStoneCompletedDate = true;
        public const bool COLUMN_NULLABLE_UserId = false;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("GoalTrackerMileStoneId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long GoalTrackerMileStoneId { get; set; }
        
        [Column("GoalTrackerId")]
        [NotNull]
        public long GoalTrackerId { get; set; }
        
        [Column("GoalTrackerMileStoneName")]
        [NotNull]
        [StringLength(255)]
        public string GoalTrackerMileStoneName { get; set; }
        
        [Column("GoalTrackerMileStoneStartDate")]
        public DateTime? GoalTrackerMileStoneStartDate { get; set; }
        
        [Column("GoalTrackerMileStoneEndDate")]
        public DateTime? GoalTrackerMileStoneEndDate { get; set; }
        
        [Column("PredefinedGoalStatusTypeId")]
        [NotNull]
        public long PredefinedGoalStatusTypeId { get; set; }
        
        [Column("GoalTrackerMileStoneCompletedDate")]
        public DateTime? GoalTrackerMileStoneCompletedDate { get; set; }
        
        [Column("UserId")]
        [NotNull]
        public long UserId { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"GoalTrackerMileStoneId = {GoalTrackerMileStoneId} , ");
            sb.AppendLine($"GoalTrackerId = {GoalTrackerId} , ");
            sb.AppendLine($"GoalTrackerMileStoneName = {GoalTrackerMileStoneName} , ");
            sb.AppendLine($"GoalTrackerMileStoneStartDate = {GoalTrackerMileStoneStartDate} , ");
            sb.AppendLine($"GoalTrackerMileStoneEndDate = {GoalTrackerMileStoneEndDate} , ");
            sb.AppendLine($"PredefinedGoalStatusTypeId = {PredefinedGoalStatusTypeId} , ");
            sb.AppendLine($"GoalTrackerMileStoneCompletedDate = {GoalTrackerMileStoneCompletedDate} , ");
            sb.AppendLine($"UserId = {UserId} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public GoalTrackerMileStone New(GoalTrackerMileStone item)
        {
            if (item == null)
                return null;

            return new GoalTrackerMileStone
            {
            GoalTrackerMileStoneId = item.GoalTrackerMileStoneId,
            GoalTrackerId = item.GoalTrackerId,
            GoalTrackerMileStoneName = item.GoalTrackerMileStoneName,
            GoalTrackerMileStoneStartDate = item.GoalTrackerMileStoneStartDate,
            GoalTrackerMileStoneEndDate = item.GoalTrackerMileStoneEndDate,
            PredefinedGoalStatusTypeId = item.PredefinedGoalStatusTypeId,
            GoalTrackerMileStoneCompletedDate = item.GoalTrackerMileStoneCompletedDate,
            UserId = item.UserId,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public GoalTrackerMileStone NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("Job")]
    [Serializable]
    public partial class Job : IAuditableModel, INewObject<Job>
    {
        public const string TABLE_NAME = "Job";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_JobId = "JobId";
        public const string COLUMN_PredefinedJobTypeId = "PredefinedJobTypeId";
        public const string COLUMN_JobTemplateId = "JobTemplateId";
        public const string COLUMN_PredefinedJobOrReportTypeId = "PredefinedJobOrReportTypeId";
        public const string COLUMN_CompletedDate = "CompletedDate";
        public const string COLUMN_SubmittedDate = "SubmittedDate";
        public const string COLUMN_Message = "Message";
        public const string COLUMN_PredefinedJobStatusTypeId = "PredefinedJobStatusTypeId";
        public const string COLUMN_SubmittedByUserId = "SubmittedByUserId";
        public const string COLUMN_ExecutableFolder = "ExecutableFolder";
        public const string COLUMN_ExecutableName = "ExecutableName";
        public const string COLUMN_ExecutableParameter = "ExecutableParameter";
        public const string COLUMN_SerializedJobParameter = "SerializedJobParameter";
        public const string COLUMN_ReportTemplateFile = "ReportTemplateFile";
        public const string COLUMN_ExportDoc = "ExportDOC";
        public const string COLUMN_ExportXls = "ExportXLS";
        public const string COLUMN_ExportPpt = "ExportPPT";
        public const string COLUMN_ExportCsv = "ExportCSV";
        public const string COLUMN_ExportXml = "ExportXML";
        public const string COLUMN_ExportPdf = "ExportPDF";
        public const string COLUMN_ExportCustom = "ExportCustom";
        public const string COLUMN_CustomExportName = "CustomExportName";
        public const string COLUMN_PickupDate = "PickupDate";
        public const string COLUMN_PickupTime = "PickupTime";
        public const string COLUMN_JobName = "JobName";
        public const string COLUMN_IncludeDataSourceFile = "IncludeDataSourceFile";
        public const string COLUMN_ExecutionAppName = "ExecutionAppName";
        public const string COLUMN_ExecutionAppId = "ExecutionAppId";
        public const string COLUMN_ExecutionHostName = "ExecutionHostName";
        public const string COLUMN_ExecutionAppCheckPort = "ExecutionAppCheckPort";
        public const string COLUMN_ExecutionProcessId = "ExecutionProcessId";
        public const string COLUMN_ExecutionThreadId = "ExecutionThreadId";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_Message = 1000;
        public const int COLUMN_SIZE_ExecutableFolder = 200;
        public const int COLUMN_SIZE_ExecutableName = 200;
        public const int COLUMN_SIZE_ExecutableParameter = 200;
        public const int COLUMN_SIZE_SerializedJobParameter = 200;
        public const int COLUMN_SIZE_ReportTemplateFile = 200;
        public const int COLUMN_SIZE_CustomExportName = 20;
        public const int COLUMN_SIZE_JobName = 200;
        public const int COLUMN_SIZE_ExecutionAppName = 100;
        public const int COLUMN_SIZE_ExecutionAppId = 100;
        public const int COLUMN_SIZE_ExecutionHostName = 100;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_JobId = false;
        public const bool COLUMN_NULLABLE_PredefinedJobTypeId = true;
        public const bool COLUMN_NULLABLE_JobTemplateId = false;
        public const bool COLUMN_NULLABLE_PredefinedJobOrReportTypeId = true;
        public const bool COLUMN_NULLABLE_CompletedDate = true;
        public const bool COLUMN_NULLABLE_SubmittedDate = false;
        public const bool COLUMN_NULLABLE_Message = true;
        public const bool COLUMN_NULLABLE_PredefinedJobStatusTypeId = false;
        public const bool COLUMN_NULLABLE_SubmittedByUserId = true;
        public const bool COLUMN_NULLABLE_ExecutableFolder = false;
        public const bool COLUMN_NULLABLE_ExecutableName = false;
        public const bool COLUMN_NULLABLE_ExecutableParameter = true;
        public const bool COLUMN_NULLABLE_SerializedJobParameter = true;
        public const bool COLUMN_NULLABLE_ReportTemplateFile = true;
        public const bool COLUMN_NULLABLE_ExportDoc = false;
        public const bool COLUMN_NULLABLE_ExportXls = false;
        public const bool COLUMN_NULLABLE_ExportPpt = false;
        public const bool COLUMN_NULLABLE_ExportCsv = false;
        public const bool COLUMN_NULLABLE_ExportXml = false;
        public const bool COLUMN_NULLABLE_ExportPdf = false;
        public const bool COLUMN_NULLABLE_ExportCustom = false;
        public const bool COLUMN_NULLABLE_CustomExportName = true;
        public const bool COLUMN_NULLABLE_PickupDate = true;
        public const bool COLUMN_NULLABLE_PickupTime = true;
        public const bool COLUMN_NULLABLE_JobName = true;
        public const bool COLUMN_NULLABLE_IncludeDataSourceFile = false;
        public const bool COLUMN_NULLABLE_ExecutionAppName = true;
        public const bool COLUMN_NULLABLE_ExecutionAppId = true;
        public const bool COLUMN_NULLABLE_ExecutionHostName = true;
        public const bool COLUMN_NULLABLE_ExecutionAppCheckPort = true;
        public const bool COLUMN_NULLABLE_ExecutionProcessId = true;
        public const bool COLUMN_NULLABLE_ExecutionThreadId = true;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("JobId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long JobId { get; set; }
        
        [Column("PredefinedJobTypeId")]
        public long? PredefinedJobTypeId { get; set; }
        
        [Column("JobTemplateId")]
        [NotNull]
        public long JobTemplateId { get; set; }
        
        [Column("PredefinedJobOrReportTypeId")]
        public long? PredefinedJobOrReportTypeId { get; set; }
        
        [Column("CompletedDate")]
        public DateTime? CompletedDate { get; set; }
        
        [Column("SubmittedDate")]
        [NotNull]
        public DateTime SubmittedDate { get; set; }
        
        [Column("Message")]
        [StringLength(1000)]
        public string Message { get; set; }
        
        [Column("PredefinedJobStatusTypeId")]
        [NotNull]
        public long PredefinedJobStatusTypeId { get; set; }
        
        [Column("SubmittedByUserId")]
        public long? SubmittedByUserId { get; set; }
        
        [Column("ExecutableFolder")]
        [NotNull]
        [StringLength(200)]
        public string ExecutableFolder { get; set; }
        
        [Column("ExecutableName")]
        [NotNull]
        [StringLength(200)]
        public string ExecutableName { get; set; }
        
        [Column("ExecutableParameter")]
        [StringLength(200)]
        public string ExecutableParameter { get; set; }
        
        [Column("SerializedJobParameter")]
        [StringLength(200)]
        public string SerializedJobParameter { get; set; }
        
        [Column("ReportTemplateFile")]
        [StringLength(200)]
        public string ReportTemplateFile { get; set; }
        
        [Column("ExportDOC")]
        [NotNull]
        public bool ExportDoc { get; set; }
        
        [Column("ExportXLS")]
        [NotNull]
        public bool ExportXls { get; set; }
        
        [Column("ExportPPT")]
        [NotNull]
        public bool ExportPpt { get; set; }
        
        [Column("ExportCSV")]
        [NotNull]
        public bool ExportCsv { get; set; }
        
        [Column("ExportXML")]
        [NotNull]
        public bool ExportXml { get; set; }
        
        [Column("ExportPDF")]
        [NotNull]
        public bool ExportPdf { get; set; }
        
        [Column("ExportCustom")]
        [NotNull]
        public bool ExportCustom { get; set; }
        
        [Column("CustomExportName")]
        [StringLength(20)]
        public string CustomExportName { get; set; }
        
        [Column("PickupDate")]
        public DateTime? PickupDate { get; set; }
        
        [Column("PickupTime")]
        public long? PickupTime { get; set; }
        
        [Column("JobName")]
        [StringLength(200)]
        public string JobName { get; set; }
        
        [Column("IncludeDataSourceFile")]
        [NotNull]
        public bool IncludeDataSourceFile { get; set; }
        
        [Column("ExecutionAppName")]
        [StringLength(100)]
        public string ExecutionAppName { get; set; }
        
        [Column("ExecutionAppId")]
        [StringLength(100)]
        public string ExecutionAppId { get; set; }
        
        [Column("ExecutionHostName")]
        [StringLength(100)]
        public string ExecutionHostName { get; set; }
        
        [Column("ExecutionAppCheckPort")]
        public long? ExecutionAppCheckPort { get; set; }
        
        [Column("ExecutionProcessId")]
        public long? ExecutionProcessId { get; set; }
        
        [Column("ExecutionThreadId")]
        public long? ExecutionThreadId { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"JobId = {JobId} , ");
            sb.AppendLine($"PredefinedJobTypeId = {PredefinedJobTypeId} , ");
            sb.AppendLine($"JobTemplateId = {JobTemplateId} , ");
            sb.AppendLine($"PredefinedJobOrReportTypeId = {PredefinedJobOrReportTypeId} , ");
            sb.AppendLine($"CompletedDate = {CompletedDate} , ");
            sb.AppendLine($"SubmittedDate = {SubmittedDate} , ");
            sb.AppendLine($"Message = {Message} , ");
            sb.AppendLine($"PredefinedJobStatusTypeId = {PredefinedJobStatusTypeId} , ");
            sb.AppendLine($"SubmittedByUserId = {SubmittedByUserId} , ");
            sb.AppendLine($"ExecutableFolder = {ExecutableFolder} , ");
            sb.AppendLine($"ExecutableName = {ExecutableName} , ");
            sb.AppendLine($"ExecutableParameter = {ExecutableParameter} , ");
            sb.AppendLine($"SerializedJobParameter = {SerializedJobParameter} , ");
            sb.AppendLine($"ReportTemplateFile = {ReportTemplateFile} , ");
            sb.AppendLine($"ExportDoc = {ExportDoc} , ");
            sb.AppendLine($"ExportXls = {ExportXls} , ");
            sb.AppendLine($"ExportPpt = {ExportPpt} , ");
            sb.AppendLine($"ExportCsv = {ExportCsv} , ");
            sb.AppendLine($"ExportXml = {ExportXml} , ");
            sb.AppendLine($"ExportPdf = {ExportPdf} , ");
            sb.AppendLine($"ExportCustom = {ExportCustom} , ");
            sb.AppendLine($"CustomExportName = {CustomExportName} , ");
            sb.AppendLine($"PickupDate = {PickupDate} , ");
            sb.AppendLine($"PickupTime = {PickupTime} , ");
            sb.AppendLine($"JobName = {JobName} , ");
            sb.AppendLine($"IncludeDataSourceFile = {IncludeDataSourceFile} , ");
            sb.AppendLine($"ExecutionAppName = {ExecutionAppName} , ");
            sb.AppendLine($"ExecutionAppId = {ExecutionAppId} , ");
            sb.AppendLine($"ExecutionHostName = {ExecutionHostName} , ");
            sb.AppendLine($"ExecutionAppCheckPort = {ExecutionAppCheckPort} , ");
            sb.AppendLine($"ExecutionProcessId = {ExecutionProcessId} , ");
            sb.AppendLine($"ExecutionThreadId = {ExecutionThreadId} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public Job New(Job item)
        {
            if (item == null)
                return null;

            return new Job
            {
            JobId = item.JobId,
            PredefinedJobTypeId = item.PredefinedJobTypeId,
            JobTemplateId = item.JobTemplateId,
            PredefinedJobOrReportTypeId = item.PredefinedJobOrReportTypeId,
            CompletedDate = item.CompletedDate,
            SubmittedDate = item.SubmittedDate,
            Message = item.Message,
            PredefinedJobStatusTypeId = item.PredefinedJobStatusTypeId,
            SubmittedByUserId = item.SubmittedByUserId,
            ExecutableFolder = item.ExecutableFolder,
            ExecutableName = item.ExecutableName,
            ExecutableParameter = item.ExecutableParameter,
            SerializedJobParameter = item.SerializedJobParameter,
            ReportTemplateFile = item.ReportTemplateFile,
            ExportDoc = item.ExportDoc,
            ExportXls = item.ExportXls,
            ExportPpt = item.ExportPpt,
            ExportCsv = item.ExportCsv,
            ExportXml = item.ExportXml,
            ExportPdf = item.ExportPdf,
            ExportCustom = item.ExportCustom,
            CustomExportName = item.CustomExportName,
            PickupDate = item.PickupDate,
            PickupTime = item.PickupTime,
            JobName = item.JobName,
            IncludeDataSourceFile = item.IncludeDataSourceFile,
            ExecutionAppName = item.ExecutionAppName,
            ExecutionAppId = item.ExecutionAppId,
            ExecutionHostName = item.ExecutionHostName,
            ExecutionAppCheckPort = item.ExecutionAppCheckPort,
            ExecutionProcessId = item.ExecutionProcessId,
            ExecutionThreadId = item.ExecutionThreadId,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public Job NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("JobAttachment")]
    [Serializable]
    public partial class JobAttachment : IAuditableModel, INewObject<JobAttachment>
    {
        public const string TABLE_NAME = "JobAttachment";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_JobAttachmentId = "JobAttachmentId";
        public const string COLUMN_JobId = "JobId";
        public const string COLUMN_FileName = "FileName";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        public const string COLUMN_StoredFilePath = "StoredFilePath";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_FileName = 200;
        public const int COLUMN_SIZE_StoredFilePath = 1000;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_JobAttachmentId = false;
        public const bool COLUMN_NULLABLE_JobId = false;
        public const bool COLUMN_NULLABLE_FileName = false;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        public const bool COLUMN_NULLABLE_StoredFilePath = false;
        #endregion

        [Column("JobAttachmentId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long JobAttachmentId { get; set; }
        
        [Column("JobId")]
        [NotNull]
        public long JobId { get; set; }
        
        [Column("FileName")]
        [NotNull]
        [StringLength(200)]
        public string FileName { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        [Column("StoredFilePath")]
        [NotNull]
        [StringLength(1000)]
        public string StoredFilePath { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"JobAttachmentId = {JobAttachmentId} , ");
            sb.AppendLine($"JobId = {JobId} , ");
            sb.AppendLine($"FileName = {FileName} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            sb.AppendLine($"StoredFilePath = {StoredFilePath} , ");
            return sb.ToString();
        }

        public JobAttachment New(JobAttachment item)
        {
            if (item == null)
                return null;

            return new JobAttachment
            {
            JobAttachmentId = item.JobAttachmentId,
            JobId = item.JobId,
            FileName = item.FileName,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            StoredFilePath = item.StoredFilePath,
            };
        }

        public JobAttachment NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("JobNotificationEmail")]
    [Serializable]
    public partial class JobNotificationEmail : IAuditableModel, INewObject<JobNotificationEmail>
    {
        public const string TABLE_NAME = "JobNotificationEmail";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_JobNotificationEmailId = "JobNotificationEmailId";
        public const string COLUMN_JobId = "JobId";
        public const string COLUMN_Email = "Email";
        public const string COLUMN_SendStatus = "SendStatus";
        public const string COLUMN_Message = "Message";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_Email = 200;
        public const int COLUMN_SIZE_Message = 200;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_JobNotificationEmailId = false;
        public const bool COLUMN_NULLABLE_JobId = false;
        public const bool COLUMN_NULLABLE_Email = false;
        public const bool COLUMN_NULLABLE_SendStatus = false;
        public const bool COLUMN_NULLABLE_Message = true;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("JobNotificationEmailId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long JobNotificationEmailId { get; set; }
        
        [Column("JobId")]
        [NotNull]
        public long JobId { get; set; }
        
        [Column("Email")]
        [NotNull]
        [StringLength(200)]
        public string Email { get; set; }
        
        [Column("SendStatus")]
        [NotNull]
        public long SendStatus { get; set; }
        
        [Column("Message")]
        [StringLength(200)]
        public string Message { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"JobNotificationEmailId = {JobNotificationEmailId} , ");
            sb.AppendLine($"JobId = {JobId} , ");
            sb.AppendLine($"Email = {Email} , ");
            sb.AppendLine($"SendStatus = {SendStatus} , ");
            sb.AppendLine($"Message = {Message} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public JobNotificationEmail New(JobNotificationEmail item)
        {
            if (item == null)
                return null;

            return new JobNotificationEmail
            {
            JobNotificationEmailId = item.JobNotificationEmailId,
            JobId = item.JobId,
            Email = item.Email,
            SendStatus = item.SendStatus,
            Message = item.Message,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public JobNotificationEmail NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("JobTemplate")]
    [Serializable]
    public partial class JobTemplate : IAuditableModel, INewObject<JobTemplate>
    {
        public const string TABLE_NAME = "JobTemplate";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_JobTemplateId = "JobTemplateId";
        public const string COLUMN_PredefinedJobTypeId = "PredefinedJobTypeId";
        public const string COLUMN_Description = "Description";
        public const string COLUMN_PredefinedJobOrReportTypeId = "PredefinedJobOrReportTypeId";
        public const string COLUMN_PredefinedJobUsageForTypeId = "PredefinedJobUsageForTypeId";
        public const string COLUMN_JobTemplateCategoryId = "JobTemplateCategoryId";
        public const string COLUMN_ExecutableFolder = "ExecutableFolder";
        public const string COLUMN_ExecutableName = "ExecutableName";
        public const string COLUMN_ExecutableParameter = "ExecutableParameter";
        public const string COLUMN_CanExportDoc = "CanExportDOC";
        public const string COLUMN_CanExportXls = "CanExportXLS";
        public const string COLUMN_CanExportPpt = "CanExportPPT";
        public const string COLUMN_CanExportCsv = "CanExportCSV";
        public const string COLUMN_CanExportXml = "CanExportXML";
        public const string COLUMN_CanExportPdf = "CanExportPDF";
        public const string COLUMN_CanExportCustom = "CanExportCustom";
        public const string COLUMN_CustomExportName = "CustomExportName";
        public const string COLUMN_TemplateName = "TemplateName";
        public const string COLUMN_CanExportDocDefaultValue = "CanExportDOCDefaultValue";
        public const string COLUMN_CanExportXlsDefaultValue = "CanExportXLSDefaultValue";
        public const string COLUMN_CanExportPptDefaultValue = "CanExportPPTDefaultValue";
        public const string COLUMN_CanExportCsvDefaultValue = "CanExportCSVDefaultValue";
        public const string COLUMN_CanExportXmlDefaultValue = "CanExportXMLDefaultValue";
        public const string COLUMN_CanExportPdfDefaultValue = "CanExportPDFDefaultValue";
        public const string COLUMN_CanExportCustomDefaultValue = "CanExportCustomDefaultValue";
        public const string COLUMN_TaskIdentifierName = "TaskIdentifierName";
        public const string COLUMN_ReportTemplateFile = "ReportTemplateFile";
        public const string COLUMN_DefaultOutputFileName = "DefaultOutputFileName";
        public const string COLUMN_IncludeDataSourceFile = "IncludeDataSourceFile";
        public const string COLUMN_IncludeDataSourceFileDefaultValue = "IncludeDataSourceFileDefaultValue";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_Description = 200;
        public const int COLUMN_SIZE_ExecutableFolder = 200;
        public const int COLUMN_SIZE_ExecutableName = 200;
        public const int COLUMN_SIZE_ExecutableParameter = 200;
        public const int COLUMN_SIZE_CustomExportName = 20;
        public const int COLUMN_SIZE_TemplateName = 50;
        public const int COLUMN_SIZE_TaskIdentifierName = 200;
        public const int COLUMN_SIZE_ReportTemplateFile = 100;
        public const int COLUMN_SIZE_DefaultOutputFileName = 200;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_JobTemplateId = false;
        public const bool COLUMN_NULLABLE_PredefinedJobTypeId = false;
        public const bool COLUMN_NULLABLE_Description = false;
        public const bool COLUMN_NULLABLE_PredefinedJobOrReportTypeId = true;
        public const bool COLUMN_NULLABLE_PredefinedJobUsageForTypeId = true;
        public const bool COLUMN_NULLABLE_JobTemplateCategoryId = false;
        public const bool COLUMN_NULLABLE_ExecutableFolder = false;
        public const bool COLUMN_NULLABLE_ExecutableName = false;
        public const bool COLUMN_NULLABLE_ExecutableParameter = true;
        public const bool COLUMN_NULLABLE_CanExportDoc = false;
        public const bool COLUMN_NULLABLE_CanExportXls = false;
        public const bool COLUMN_NULLABLE_CanExportPpt = false;
        public const bool COLUMN_NULLABLE_CanExportCsv = false;
        public const bool COLUMN_NULLABLE_CanExportXml = false;
        public const bool COLUMN_NULLABLE_CanExportPdf = false;
        public const bool COLUMN_NULLABLE_CanExportCustom = false;
        public const bool COLUMN_NULLABLE_CustomExportName = true;
        public const bool COLUMN_NULLABLE_TemplateName = false;
        public const bool COLUMN_NULLABLE_CanExportDocDefaultValue = false;
        public const bool COLUMN_NULLABLE_CanExportXlsDefaultValue = false;
        public const bool COLUMN_NULLABLE_CanExportPptDefaultValue = false;
        public const bool COLUMN_NULLABLE_CanExportCsvDefaultValue = false;
        public const bool COLUMN_NULLABLE_CanExportXmlDefaultValue = false;
        public const bool COLUMN_NULLABLE_CanExportPdfDefaultValue = false;
        public const bool COLUMN_NULLABLE_CanExportCustomDefaultValue = false;
        public const bool COLUMN_NULLABLE_TaskIdentifierName = false;
        public const bool COLUMN_NULLABLE_ReportTemplateFile = false;
        public const bool COLUMN_NULLABLE_DefaultOutputFileName = false;
        public const bool COLUMN_NULLABLE_IncludeDataSourceFile = false;
        public const bool COLUMN_NULLABLE_IncludeDataSourceFileDefaultValue = false;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("JobTemplateId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long JobTemplateId { get; set; }
        
        [Column("PredefinedJobTypeId")]
        [NotNull]
        public long PredefinedJobTypeId { get; set; }
        
        [Column("Description")]
        [NotNull]
        [StringLength(200)]
        public string Description { get; set; }
        
        [Column("PredefinedJobOrReportTypeId")]
        public long? PredefinedJobOrReportTypeId { get; set; }
        
        [Column("PredefinedJobUsageForTypeId")]
        public long? PredefinedJobUsageForTypeId { get; set; }
        
        [Column("JobTemplateCategoryId")]
        [NotNull]
        public long JobTemplateCategoryId { get; set; }
        
        [Column("ExecutableFolder")]
        [NotNull]
        [StringLength(200)]
        public string ExecutableFolder { get; set; }
        
        [Column("ExecutableName")]
        [NotNull]
        [StringLength(200)]
        public string ExecutableName { get; set; }
        
        [Column("ExecutableParameter")]
        [StringLength(200)]
        public string ExecutableParameter { get; set; }
        
        [Column("CanExportDOC")]
        [NotNull]
        public bool CanExportDoc { get; set; }
        
        [Column("CanExportXLS")]
        [NotNull]
        public bool CanExportXls { get; set; }
        
        [Column("CanExportPPT")]
        [NotNull]
        public bool CanExportPpt { get; set; }
        
        [Column("CanExportCSV")]
        [NotNull]
        public bool CanExportCsv { get; set; }
        
        [Column("CanExportXML")]
        [NotNull]
        public bool CanExportXml { get; set; }
        
        [Column("CanExportPDF")]
        [NotNull]
        public bool CanExportPdf { get; set; }
        
        [Column("CanExportCustom")]
        [NotNull]
        public bool CanExportCustom { get; set; }
        
        [Column("CustomExportName")]
        [StringLength(20)]
        public string CustomExportName { get; set; }
        
        [Column("TemplateName")]
        [NotNull]
        [StringLength(50)]
        public string TemplateName { get; set; }
        
        [Column("CanExportDOCDefaultValue")]
        [NotNull]
        public bool CanExportDocDefaultValue { get; set; }
        
        [Column("CanExportXLSDefaultValue")]
        [NotNull]
        public bool CanExportXlsDefaultValue { get; set; }
        
        [Column("CanExportPPTDefaultValue")]
        [NotNull]
        public bool CanExportPptDefaultValue { get; set; }
        
        [Column("CanExportCSVDefaultValue")]
        [NotNull]
        public bool CanExportCsvDefaultValue { get; set; }
        
        [Column("CanExportXMLDefaultValue")]
        [NotNull]
        public bool CanExportXmlDefaultValue { get; set; }
        
        [Column("CanExportPDFDefaultValue")]
        [NotNull]
        public bool CanExportPdfDefaultValue { get; set; }
        
        [Column("CanExportCustomDefaultValue")]
        [NotNull]
        public bool CanExportCustomDefaultValue { get; set; }
        
        [Column("TaskIdentifierName")]
        [NotNull]
        [StringLength(200)]
        public string TaskIdentifierName { get; set; }
        
        [Column("ReportTemplateFile")]
        [NotNull]
        [StringLength(100)]
        public string ReportTemplateFile { get; set; }
        
        [Column("DefaultOutputFileName")]
        [NotNull]
        [StringLength(200)]
        public string DefaultOutputFileName { get; set; }
        
        [Column("IncludeDataSourceFile")]
        [NotNull]
        public bool IncludeDataSourceFile { get; set; }
        
        [Column("IncludeDataSourceFileDefaultValue")]
        [NotNull]
        public bool IncludeDataSourceFileDefaultValue { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"JobTemplateId = {JobTemplateId} , ");
            sb.AppendLine($"PredefinedJobTypeId = {PredefinedJobTypeId} , ");
            sb.AppendLine($"Description = {Description} , ");
            sb.AppendLine($"PredefinedJobOrReportTypeId = {PredefinedJobOrReportTypeId} , ");
            sb.AppendLine($"PredefinedJobUsageForTypeId = {PredefinedJobUsageForTypeId} , ");
            sb.AppendLine($"JobTemplateCategoryId = {JobTemplateCategoryId} , ");
            sb.AppendLine($"ExecutableFolder = {ExecutableFolder} , ");
            sb.AppendLine($"ExecutableName = {ExecutableName} , ");
            sb.AppendLine($"ExecutableParameter = {ExecutableParameter} , ");
            sb.AppendLine($"CanExportDoc = {CanExportDoc} , ");
            sb.AppendLine($"CanExportXls = {CanExportXls} , ");
            sb.AppendLine($"CanExportPpt = {CanExportPpt} , ");
            sb.AppendLine($"CanExportCsv = {CanExportCsv} , ");
            sb.AppendLine($"CanExportXml = {CanExportXml} , ");
            sb.AppendLine($"CanExportPdf = {CanExportPdf} , ");
            sb.AppendLine($"CanExportCustom = {CanExportCustom} , ");
            sb.AppendLine($"CustomExportName = {CustomExportName} , ");
            sb.AppendLine($"TemplateName = {TemplateName} , ");
            sb.AppendLine($"CanExportDocDefaultValue = {CanExportDocDefaultValue} , ");
            sb.AppendLine($"CanExportXlsDefaultValue = {CanExportXlsDefaultValue} , ");
            sb.AppendLine($"CanExportPptDefaultValue = {CanExportPptDefaultValue} , ");
            sb.AppendLine($"CanExportCsvDefaultValue = {CanExportCsvDefaultValue} , ");
            sb.AppendLine($"CanExportXmlDefaultValue = {CanExportXmlDefaultValue} , ");
            sb.AppendLine($"CanExportPdfDefaultValue = {CanExportPdfDefaultValue} , ");
            sb.AppendLine($"CanExportCustomDefaultValue = {CanExportCustomDefaultValue} , ");
            sb.AppendLine($"TaskIdentifierName = {TaskIdentifierName} , ");
            sb.AppendLine($"ReportTemplateFile = {ReportTemplateFile} , ");
            sb.AppendLine($"DefaultOutputFileName = {DefaultOutputFileName} , ");
            sb.AppendLine($"IncludeDataSourceFile = {IncludeDataSourceFile} , ");
            sb.AppendLine($"IncludeDataSourceFileDefaultValue = {IncludeDataSourceFileDefaultValue} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public JobTemplate New(JobTemplate item)
        {
            if (item == null)
                return null;

            return new JobTemplate
            {
            JobTemplateId = item.JobTemplateId,
            PredefinedJobTypeId = item.PredefinedJobTypeId,
            Description = item.Description,
            PredefinedJobOrReportTypeId = item.PredefinedJobOrReportTypeId,
            PredefinedJobUsageForTypeId = item.PredefinedJobUsageForTypeId,
            JobTemplateCategoryId = item.JobTemplateCategoryId,
            ExecutableFolder = item.ExecutableFolder,
            ExecutableName = item.ExecutableName,
            ExecutableParameter = item.ExecutableParameter,
            CanExportDoc = item.CanExportDoc,
            CanExportXls = item.CanExportXls,
            CanExportPpt = item.CanExportPpt,
            CanExportCsv = item.CanExportCsv,
            CanExportXml = item.CanExportXml,
            CanExportPdf = item.CanExportPdf,
            CanExportCustom = item.CanExportCustom,
            CustomExportName = item.CustomExportName,
            TemplateName = item.TemplateName,
            CanExportDocDefaultValue = item.CanExportDocDefaultValue,
            CanExportXlsDefaultValue = item.CanExportXlsDefaultValue,
            CanExportPptDefaultValue = item.CanExportPptDefaultValue,
            CanExportCsvDefaultValue = item.CanExportCsvDefaultValue,
            CanExportXmlDefaultValue = item.CanExportXmlDefaultValue,
            CanExportPdfDefaultValue = item.CanExportPdfDefaultValue,
            CanExportCustomDefaultValue = item.CanExportCustomDefaultValue,
            TaskIdentifierName = item.TaskIdentifierName,
            ReportTemplateFile = item.ReportTemplateFile,
            DefaultOutputFileName = item.DefaultOutputFileName,
            IncludeDataSourceFile = item.IncludeDataSourceFile,
            IncludeDataSourceFileDefaultValue = item.IncludeDataSourceFileDefaultValue,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public JobTemplate NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("JobTemplateCategory")]
    [Serializable]
    public partial class JobTemplateCategory : IAuditableModel, INewObject<JobTemplateCategory>
    {
        public const string TABLE_NAME = "JobTemplateCategory";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_JobTemplateCategoryId = "JobTemplateCategoryId";
        public const string COLUMN_Name = "Name";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_Name = 100;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_JobTemplateCategoryId = false;
        public const bool COLUMN_NULLABLE_Name = false;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("JobTemplateCategoryId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long JobTemplateCategoryId { get; set; }
        
        [Column("Name")]
        [NotNull]
        [StringLength(100)]
        public string Name { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"JobTemplateCategoryId = {JobTemplateCategoryId} , ");
            sb.AppendLine($"Name = {Name} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public JobTemplateCategory New(JobTemplateCategory item)
        {
            if (item == null)
                return null;

            return new JobTemplateCategory
            {
            JobTemplateCategoryId = item.JobTemplateCategoryId,
            Name = item.Name,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public JobTemplateCategory NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("JobTemplateParameter")]
    [Serializable]
    public partial class JobTemplateParameter : IAuditableModel, INewObject<JobTemplateParameter>
    {
        public const string TABLE_NAME = "JobTemplateParameter";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_JobTemplateParameterId = "JobTemplateParameterId";
        public const string COLUMN_JobTemplateId = "JobTemplateId";
        public const string COLUMN_Name = "Name";
        public const string COLUMN_Value = "Value";
        public const string COLUMN_DefaultValue = "DefaultValue";
        public const string COLUMN_PredefinedFieldControlTypeId = "PredefinedFieldControlTypeId";
        public const string COLUMN_PredefinedFieldTypeId = "PredefinedFieldTypeId";
        public const string COLUMN_PredefinedFieldPopulatorTypeId = "PredefinedFieldPopulatorTypeId";
        public const string COLUMN_Caption = "Caption";
        public const string COLUMN_PopulatorSource = "PopulatorSource";
        public const string COLUMN_Size = "Size";
        public const string COLUMN_Empty = "Empty";
        public const string COLUMN_Position = "Position";
        public const string COLUMN_IsVisible = "IsVisible";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_Name = 100;
        public const int COLUMN_SIZE_Value = 250;
        public const int COLUMN_SIZE_DefaultValue = 250;
        public const int COLUMN_SIZE_Caption = 200;
        public const int COLUMN_SIZE_PopulatorSource = 500;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_JobTemplateParameterId = false;
        public const bool COLUMN_NULLABLE_JobTemplateId = false;
        public const bool COLUMN_NULLABLE_Name = false;
        public const bool COLUMN_NULLABLE_Value = false;
        public const bool COLUMN_NULLABLE_DefaultValue = false;
        public const bool COLUMN_NULLABLE_PredefinedFieldControlTypeId = false;
        public const bool COLUMN_NULLABLE_PredefinedFieldTypeId = false;
        public const bool COLUMN_NULLABLE_PredefinedFieldPopulatorTypeId = false;
        public const bool COLUMN_NULLABLE_Caption = true;
        public const bool COLUMN_NULLABLE_PopulatorSource = true;
        public const bool COLUMN_NULLABLE_Size = true;
        public const bool COLUMN_NULLABLE_Empty = true;
        public const bool COLUMN_NULLABLE_Position = false;
        public const bool COLUMN_NULLABLE_IsVisible = false;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("JobTemplateParameterId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long JobTemplateParameterId { get; set; }
        
        [Column("JobTemplateId")]
        [NotNull]
        public long JobTemplateId { get; set; }
        
        [Column("Name")]
        [NotNull]
        [StringLength(100)]
        public string Name { get; set; }
        
        [Column("Value")]
        [NotNull]
        [StringLength(250)]
        public string Value { get; set; }
        
        [Column("DefaultValue")]
        [NotNull]
        [StringLength(250)]
        public string DefaultValue { get; set; }
        
        [Column("PredefinedFieldControlTypeId")]
        [NotNull]
        public long PredefinedFieldControlTypeId { get; set; }
        
        [Column("PredefinedFieldTypeId")]
        [NotNull]
        public long PredefinedFieldTypeId { get; set; }
        
        [Column("PredefinedFieldPopulatorTypeId")]
        [NotNull]
        public long PredefinedFieldPopulatorTypeId { get; set; }
        
        [Column("Caption")]
        [StringLength(200)]
        public string Caption { get; set; }
        
        [Column("PopulatorSource")]
        [StringLength(500)]
        public string PopulatorSource { get; set; }
        
        [Column("Size")]
        public long? Size { get; set; }
        
        [Column("Empty")]
        public bool? Empty { get; set; }
        
        [Column("Position")]
        [NotNull]
        public long Position { get; set; }
        
        [Column("IsVisible")]
        [NotNull]
        public bool IsVisible { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"JobTemplateParameterId = {JobTemplateParameterId} , ");
            sb.AppendLine($"JobTemplateId = {JobTemplateId} , ");
            sb.AppendLine($"Name = {Name} , ");
            sb.AppendLine($"Value = {Value} , ");
            sb.AppendLine($"DefaultValue = {DefaultValue} , ");
            sb.AppendLine($"PredefinedFieldControlTypeId = {PredefinedFieldControlTypeId} , ");
            sb.AppendLine($"PredefinedFieldTypeId = {PredefinedFieldTypeId} , ");
            sb.AppendLine($"PredefinedFieldPopulatorTypeId = {PredefinedFieldPopulatorTypeId} , ");
            sb.AppendLine($"Caption = {Caption} , ");
            sb.AppendLine($"PopulatorSource = {PopulatorSource} , ");
            sb.AppendLine($"Size = {Size} , ");
            sb.AppendLine($"Empty = {Empty} , ");
            sb.AppendLine($"Position = {Position} , ");
            sb.AppendLine($"IsVisible = {IsVisible} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public JobTemplateParameter New(JobTemplateParameter item)
        {
            if (item == null)
                return null;

            return new JobTemplateParameter
            {
            JobTemplateParameterId = item.JobTemplateParameterId,
            JobTemplateId = item.JobTemplateId,
            Name = item.Name,
            Value = item.Value,
            DefaultValue = item.DefaultValue,
            PredefinedFieldControlTypeId = item.PredefinedFieldControlTypeId,
            PredefinedFieldTypeId = item.PredefinedFieldTypeId,
            PredefinedFieldPopulatorTypeId = item.PredefinedFieldPopulatorTypeId,
            Caption = item.Caption,
            PopulatorSource = item.PopulatorSource,
            Size = item.Size,
            Empty = item.Empty,
            Position = item.Position,
            IsVisible = item.IsVisible,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public JobTemplateParameter NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("MailServer")]
    [Serializable]
    public partial class MailServer : IAuditableModel, INewObject<MailServer>
    {
        public const string TABLE_NAME = "MailServer";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_MailServerId = "MailServerId";
        public const string COLUMN_MailServerCode = "MailServerCode";
        public const string COLUMN_MailServerName = "MailServerName";
        public const string COLUMN_Host = "Host";
        public const string COLUMN_Port = "Port";
        public const string COLUMN_SenderName = "SenderName";
        public const string COLUMN_SenderEmail = "SenderEmail";
        public const string COLUMN_UserName = "UserName";
        public const string COLUMN_Password = "Password";
        public const string COLUMN_IsSecure = "IsSecure";
        public const string COLUMN_IsDefault = "IsDefault";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_MailServerCode = 45;
        public const int COLUMN_SIZE_MailServerName = 100;
        public const int COLUMN_SIZE_Host = 150;
        public const int COLUMN_SIZE_SenderName = 100;
        public const int COLUMN_SIZE_SenderEmail = 150;
        public const int COLUMN_SIZE_UserName = 100;
        public const int COLUMN_SIZE_Password = 200;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_MailServerId = false;
        public const bool COLUMN_NULLABLE_MailServerCode = false;
        public const bool COLUMN_NULLABLE_MailServerName = false;
        public const bool COLUMN_NULLABLE_Host = false;
        public const bool COLUMN_NULLABLE_Port = false;
        public const bool COLUMN_NULLABLE_SenderName = false;
        public const bool COLUMN_NULLABLE_SenderEmail = false;
        public const bool COLUMN_NULLABLE_UserName = false;
        public const bool COLUMN_NULLABLE_Password = true;
        public const bool COLUMN_NULLABLE_IsSecure = true;
        public const bool COLUMN_NULLABLE_IsDefault = false;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("MailServerId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long MailServerId { get; set; }
        
        [Column("MailServerCode")]
        [NotNull]
        [StringLength(45)]
        public string MailServerCode { get; set; }
        
        [Column("MailServerName")]
        [NotNull]
        [StringLength(100)]
        public string MailServerName { get; set; }
        
        [Column("Host")]
        [NotNull]
        [StringLength(150)]
        public string Host { get; set; }
        
        [Column("Port")]
        [NotNull]
        public long Port { get; set; }
        
        [Column("SenderName")]
        [NotNull]
        [StringLength(100)]
        public string SenderName { get; set; }
        
        [Column("SenderEmail")]
        [NotNull]
        [StringLength(150)]
        public string SenderEmail { get; set; }
        
        [Column("UserName")]
        [NotNull]
        [StringLength(100)]
        public string UserName { get; set; }
        
        [Column("Password")]
        [StringLength(200)]
        public string Password { get; set; }
        
        [Column("IsSecure")]
        public bool? IsSecure { get; set; }
        
        [Column("IsDefault")]
        [NotNull]
        public bool IsDefault { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"MailServerId = {MailServerId} , ");
            sb.AppendLine($"MailServerCode = {MailServerCode} , ");
            sb.AppendLine($"MailServerName = {MailServerName} , ");
            sb.AppendLine($"Host = {Host} , ");
            sb.AppendLine($"Port = {Port} , ");
            sb.AppendLine($"SenderName = {SenderName} , ");
            sb.AppendLine($"SenderEmail = {SenderEmail} , ");
            sb.AppendLine($"UserName = {UserName} , ");
            sb.AppendLine($"Password = {Password} , ");
            sb.AppendLine($"IsSecure = {IsSecure} , ");
            sb.AppendLine($"IsDefault = {IsDefault} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public MailServer New(MailServer item)
        {
            if (item == null)
                return null;

            return new MailServer
            {
            MailServerId = item.MailServerId,
            MailServerCode = item.MailServerCode,
            MailServerName = item.MailServerName,
            Host = item.Host,
            Port = item.Port,
            SenderName = item.SenderName,
            SenderEmail = item.SenderEmail,
            UserName = item.UserName,
            Password = item.Password,
            IsSecure = item.IsSecure,
            IsDefault = item.IsDefault,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public MailServer NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("MeditationFile")]
    [Serializable]
    public partial class MeditationFile : IAuditableModel, INewObject<MeditationFile>
    {
        public const string TABLE_NAME = "MeditationFile";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_MeditationFileId = "MeditationFileId";
        public const string COLUMN_PredefinedMeditationTypeId = "PredefinedMeditationTypeId";
        public const string COLUMN_MeditationFileName = "MeditationFileName";
        public const string COLUMN_Location = "Location";
        public const string COLUMN_FileName = "FileName";
        public const string COLUMN_FileSize = "FileSize";
        public const string COLUMN_UserId = "UserId";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_MeditationFileName = 255;
        public const int COLUMN_SIZE_Location = 1024;
        public const int COLUMN_SIZE_FileName = 255;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_MeditationFileId = false;
        public const bool COLUMN_NULLABLE_PredefinedMeditationTypeId = false;
        public const bool COLUMN_NULLABLE_MeditationFileName = false;
        public const bool COLUMN_NULLABLE_Location = true;
        public const bool COLUMN_NULLABLE_FileName = false;
        public const bool COLUMN_NULLABLE_FileSize = false;
        public const bool COLUMN_NULLABLE_UserId = false;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("MeditationFileId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long MeditationFileId { get; set; }
        
        [Column("PredefinedMeditationTypeId")]
        [NotNull]
        public long PredefinedMeditationTypeId { get; set; }
        
        [Column("MeditationFileName")]
        [NotNull]
        [StringLength(255)]
        public string MeditationFileName { get; set; }
        
        [Column("Location")]
        [StringLength(1024)]
        public string Location { get; set; }
        
        [Column("FileName")]
        [NotNull]
        [StringLength(255)]
        public string FileName { get; set; }
        
        [Column("FileSize")]
        [NotNull]
        public long FileSize { get; set; }
        
        [Column("UserId")]
        [NotNull]
        public long UserId { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"MeditationFileId = {MeditationFileId} , ");
            sb.AppendLine($"PredefinedMeditationTypeId = {PredefinedMeditationTypeId} , ");
            sb.AppendLine($"MeditationFileName = {MeditationFileName} , ");
            sb.AppendLine($"Location = {Location} , ");
            sb.AppendLine($"FileName = {FileName} , ");
            sb.AppendLine($"FileSize = {FileSize} , ");
            sb.AppendLine($"UserId = {UserId} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public MeditationFile New(MeditationFile item)
        {
            if (item == null)
                return null;

            return new MeditationFile
            {
            MeditationFileId = item.MeditationFileId,
            PredefinedMeditationTypeId = item.PredefinedMeditationTypeId,
            MeditationFileName = item.MeditationFileName,
            Location = item.Location,
            FileName = item.FileName,
            FileSize = item.FileSize,
            UserId = item.UserId,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public MeditationFile NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("MeditationSession")]
    [Serializable]
    public partial class MeditationSession : IAuditableModel, INewObject<MeditationSession>
    {
        public const string TABLE_NAME = "MeditationSession";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_MeditationSessionId = "MeditationSessionId";
        public const string COLUMN_MeditationSessionDate = "MeditationSessionDate";
        public const string COLUMN_MeditationSessionTimeSpentInSeconds = "MeditationSessionTimeSpentInSeconds";
        public const string COLUMN_MeditationFileId = "MeditationFileId";
        public const string COLUMN_UserId = "UserId";
        public const string COLUMN_AddedToFeed = "AddedToFeed";
        public const string COLUMN_AddedToFeedDate = "AddedToFeedDate";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_MeditationSessionId = false;
        public const bool COLUMN_NULLABLE_MeditationSessionDate = false;
        public const bool COLUMN_NULLABLE_MeditationSessionTimeSpentInSeconds = true;
        public const bool COLUMN_NULLABLE_MeditationFileId = false;
        public const bool COLUMN_NULLABLE_UserId = false;
        public const bool COLUMN_NULLABLE_AddedToFeed = true;
        public const bool COLUMN_NULLABLE_AddedToFeedDate = true;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("MeditationSessionId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long MeditationSessionId { get; set; }
        
        [Column("MeditationSessionDate")]
        [NotNull]
        public DateTime MeditationSessionDate { get; set; }
        
        [Column("MeditationSessionTimeSpentInSeconds")]
        public long? MeditationSessionTimeSpentInSeconds { get; set; }
        
        [Column("MeditationFileId")]
        [NotNull]
        public long MeditationFileId { get; set; }
        
        [Column("UserId")]
        [NotNull]
        public long UserId { get; set; }
        
        [Column("AddedToFeed")]
        public bool? AddedToFeed { get; set; }
        
        [Column("AddedToFeedDate")]
        public DateTime? AddedToFeedDate { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"MeditationSessionId = {MeditationSessionId} , ");
            sb.AppendLine($"MeditationSessionDate = {MeditationSessionDate} , ");
            sb.AppendLine($"MeditationSessionTimeSpentInSeconds = {MeditationSessionTimeSpentInSeconds} , ");
            sb.AppendLine($"MeditationFileId = {MeditationFileId} , ");
            sb.AppendLine($"UserId = {UserId} , ");
            sb.AppendLine($"AddedToFeed = {AddedToFeed} , ");
            sb.AppendLine($"AddedToFeedDate = {AddedToFeedDate} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public MeditationSession New(MeditationSession item)
        {
            if (item == null)
                return null;

            return new MeditationSession
            {
            MeditationSessionId = item.MeditationSessionId,
            MeditationSessionDate = item.MeditationSessionDate,
            MeditationSessionTimeSpentInSeconds = item.MeditationSessionTimeSpentInSeconds,
            MeditationFileId = item.MeditationFileId,
            UserId = item.UserId,
            AddedToFeed = item.AddedToFeed,
            AddedToFeedDate = item.AddedToFeedDate,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public MeditationSession NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("MessageTemplate")]
    [Serializable]
    public partial class MessageTemplate : IAuditableModel, INewObject<MessageTemplate>
    {
        public const string TABLE_NAME = "MessageTemplate";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_MessageTemplateId = "MessageTemplateId";
        public const string COLUMN_PredefinedMessageTemplateTypeId = "PredefinedMessageTemplateTypeId";
        public const string COLUMN_EmailSubject = "EmailSubject";
        public const string COLUMN_EmailBody = "EmailBody";
        public const string COLUMN_SmsBody = "SmsBody";
        public const string COLUMN_AppNotificationSubject = "AppNotificationSubject";
        public const string COLUMN_AppNotificationBody = "AppNotificationBody";
        public const string COLUMN_WebNotificationSubject = "WebNotificationSubject";
        public const string COLUMN_WebNotificationBody = "WebNotificationBody";
        public const string COLUMN_SendEmailForInternalUser = "SendEmailForInternalUser";
        public const string COLUMN_SendSmsForInternalUser = "SendSmsForInternalUser";
        public const string COLUMN_SendAppNotificationForInternalUser = "SendAppNotificationForInternalUser";
        public const string COLUMN_SendEmailForExternalUser = "SendEmailForExternalUser";
        public const string COLUMN_SendSmsForExternalUser = "SendSmsForExternalUser";
        public const string COLUMN_SendAppNotificationForExternalUser = "SendAppNotificationForExternalUser";
        public const string COLUMN_AdditionalRecipients = "AdditionalRecipients";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_EmailSubject = 500;
        public const int COLUMN_SIZE_EmailBody = 2000;
        public const int COLUMN_SIZE_SmsBody = 2000;
        public const int COLUMN_SIZE_AppNotificationSubject = 500;
        public const int COLUMN_SIZE_AppNotificationBody = 2000;
        public const int COLUMN_SIZE_WebNotificationSubject = 500;
        public const int COLUMN_SIZE_WebNotificationBody = 2000;
        public const int COLUMN_SIZE_AdditionalRecipients = 500;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_MessageTemplateId = false;
        public const bool COLUMN_NULLABLE_PredefinedMessageTemplateTypeId = false;
        public const bool COLUMN_NULLABLE_EmailSubject = false;
        public const bool COLUMN_NULLABLE_EmailBody = false;
        public const bool COLUMN_NULLABLE_SmsBody = false;
        public const bool COLUMN_NULLABLE_AppNotificationSubject = false;
        public const bool COLUMN_NULLABLE_AppNotificationBody = false;
        public const bool COLUMN_NULLABLE_WebNotificationSubject = false;
        public const bool COLUMN_NULLABLE_WebNotificationBody = false;
        public const bool COLUMN_NULLABLE_SendEmailForInternalUser = true;
        public const bool COLUMN_NULLABLE_SendSmsForInternalUser = true;
        public const bool COLUMN_NULLABLE_SendAppNotificationForInternalUser = true;
        public const bool COLUMN_NULLABLE_SendEmailForExternalUser = true;
        public const bool COLUMN_NULLABLE_SendSmsForExternalUser = true;
        public const bool COLUMN_NULLABLE_SendAppNotificationForExternalUser = true;
        public const bool COLUMN_NULLABLE_AdditionalRecipients = true;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("MessageTemplateId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long MessageTemplateId { get; set; }
        
        [Column("PredefinedMessageTemplateTypeId")]
        [NotNull]
        public long PredefinedMessageTemplateTypeId { get; set; }
        
        [Column("EmailSubject")]
        [NotNull]
        [StringLength(500)]
        public string EmailSubject { get; set; }
        
        [Column("EmailBody")]
        [NotNull]
        [StringLength(2000)]
        public string EmailBody { get; set; }
        
        [Column("SmsBody")]
        [NotNull]
        [StringLength(2000)]
        public string SmsBody { get; set; }
        
        [Column("AppNotificationSubject")]
        [NotNull]
        [StringLength(500)]
        public string AppNotificationSubject { get; set; }
        
        [Column("AppNotificationBody")]
        [NotNull]
        [StringLength(2000)]
        public string AppNotificationBody { get; set; }
        
        [Column("WebNotificationSubject")]
        [NotNull]
        [StringLength(500)]
        public string WebNotificationSubject { get; set; }
        
        [Column("WebNotificationBody")]
        [NotNull]
        [StringLength(2000)]
        public string WebNotificationBody { get; set; }
        
        [Column("SendEmailForInternalUser")]
        public bool? SendEmailForInternalUser { get; set; }
        
        [Column("SendSmsForInternalUser")]
        public bool? SendSmsForInternalUser { get; set; }
        
        [Column("SendAppNotificationForInternalUser")]
        public bool? SendAppNotificationForInternalUser { get; set; }
        
        [Column("SendEmailForExternalUser")]
        public bool? SendEmailForExternalUser { get; set; }
        
        [Column("SendSmsForExternalUser")]
        public bool? SendSmsForExternalUser { get; set; }
        
        [Column("SendAppNotificationForExternalUser")]
        public bool? SendAppNotificationForExternalUser { get; set; }
        
        [Column("AdditionalRecipients")]
        [StringLength(500)]
        public string AdditionalRecipients { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"MessageTemplateId = {MessageTemplateId} , ");
            sb.AppendLine($"PredefinedMessageTemplateTypeId = {PredefinedMessageTemplateTypeId} , ");
            sb.AppendLine($"EmailSubject = {EmailSubject} , ");
            sb.AppendLine($"EmailBody = {EmailBody} , ");
            sb.AppendLine($"SmsBody = {SmsBody} , ");
            sb.AppendLine($"AppNotificationSubject = {AppNotificationSubject} , ");
            sb.AppendLine($"AppNotificationBody = {AppNotificationBody} , ");
            sb.AppendLine($"WebNotificationSubject = {WebNotificationSubject} , ");
            sb.AppendLine($"WebNotificationBody = {WebNotificationBody} , ");
            sb.AppendLine($"SendEmailForInternalUser = {SendEmailForInternalUser} , ");
            sb.AppendLine($"SendSmsForInternalUser = {SendSmsForInternalUser} , ");
            sb.AppendLine($"SendAppNotificationForInternalUser = {SendAppNotificationForInternalUser} , ");
            sb.AppendLine($"SendEmailForExternalUser = {SendEmailForExternalUser} , ");
            sb.AppendLine($"SendSmsForExternalUser = {SendSmsForExternalUser} , ");
            sb.AppendLine($"SendAppNotificationForExternalUser = {SendAppNotificationForExternalUser} , ");
            sb.AppendLine($"AdditionalRecipients = {AdditionalRecipients} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public MessageTemplate New(MessageTemplate item)
        {
            if (item == null)
                return null;

            return new MessageTemplate
            {
            MessageTemplateId = item.MessageTemplateId,
            PredefinedMessageTemplateTypeId = item.PredefinedMessageTemplateTypeId,
            EmailSubject = item.EmailSubject,
            EmailBody = item.EmailBody,
            SmsBody = item.SmsBody,
            AppNotificationSubject = item.AppNotificationSubject,
            AppNotificationBody = item.AppNotificationBody,
            WebNotificationSubject = item.WebNotificationSubject,
            WebNotificationBody = item.WebNotificationBody,
            SendEmailForInternalUser = item.SendEmailForInternalUser,
            SendSmsForInternalUser = item.SendSmsForInternalUser,
            SendAppNotificationForInternalUser = item.SendAppNotificationForInternalUser,
            SendEmailForExternalUser = item.SendEmailForExternalUser,
            SendSmsForExternalUser = item.SendSmsForExternalUser,
            SendAppNotificationForExternalUser = item.SendAppNotificationForExternalUser,
            AdditionalRecipients = item.AdditionalRecipients,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public MessageTemplate NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("MessageToSend")]
    [Serializable]
    public partial class MessageToSend : IAuditableModel, INewObject<MessageToSend>
    {
        public const string TABLE_NAME = "MessageToSend";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_MessageToSendId = "MessageToSendId";
        public const string COLUMN_PredefinedMessageToSendTypeId = "PredefinedMessageToSendTypeId";
        public const string COLUMN_RealtimeMessageId = "RealtimeMessageId";
        public const string COLUMN_MessageFrom = "MessageFrom";
        public const string COLUMN_MessageTo = "MessageTo";
        public const string COLUMN_MessageFromName = "MessageFromName";
        public const string COLUMN_MessageToName = "MessageToName";
        public const string COLUMN_MessageSubject = "MessageSubject";
        public const string COLUMN_MessageBody = "MessageBody";
        public const string COLUMN_SentDate = "SentDate";
        public const string COLUMN_Sent = "Sent";
        public const string COLUMN_IsSentError = "IsSentError";
        public const string COLUMN_SentErrorMessage = "SentErrorMessage";
        public const string COLUMN_SentErrorDate = "SentErrorDate";
        public const string COLUMN_RetryCount = "RetryCount";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_MessageFrom = 500;
        public const int COLUMN_SIZE_MessageTo = 500;
        public const int COLUMN_SIZE_MessageFromName = 500;
        public const int COLUMN_SIZE_MessageToName = 500;
        public const int COLUMN_SIZE_MessageSubject = 500;
        public const int COLUMN_SIZE_MessageBody = 2000;
        public const int COLUMN_SIZE_SentErrorMessage = 500;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_MessageToSendId = false;
        public const bool COLUMN_NULLABLE_PredefinedMessageToSendTypeId = false;
        public const bool COLUMN_NULLABLE_RealtimeMessageId = true;
        public const bool COLUMN_NULLABLE_MessageFrom = false;
        public const bool COLUMN_NULLABLE_MessageTo = false;
        public const bool COLUMN_NULLABLE_MessageFromName = false;
        public const bool COLUMN_NULLABLE_MessageToName = false;
        public const bool COLUMN_NULLABLE_MessageSubject = true;
        public const bool COLUMN_NULLABLE_MessageBody = false;
        public const bool COLUMN_NULLABLE_SentDate = true;
        public const bool COLUMN_NULLABLE_Sent = true;
        public const bool COLUMN_NULLABLE_IsSentError = true;
        public const bool COLUMN_NULLABLE_SentErrorMessage = true;
        public const bool COLUMN_NULLABLE_SentErrorDate = true;
        public const bool COLUMN_NULLABLE_RetryCount = false;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("MessageToSendId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long MessageToSendId { get; set; }
        
        [Column("PredefinedMessageToSendTypeId")]
        [NotNull]
        public long PredefinedMessageToSendTypeId { get; set; }
        
        [Column("RealtimeMessageId")]
        public long? RealtimeMessageId { get; set; }
        
        [Column("MessageFrom")]
        [NotNull]
        [StringLength(500)]
        public string MessageFrom { get; set; }
        
        [Column("MessageTo")]
        [NotNull]
        [StringLength(500)]
        public string MessageTo { get; set; }
        
        [Column("MessageFromName")]
        [NotNull]
        [StringLength(500)]
        public string MessageFromName { get; set; }
        
        [Column("MessageToName")]
        [NotNull]
        [StringLength(500)]
        public string MessageToName { get; set; }
        
        [Column("MessageSubject")]
        [StringLength(500)]
        public string MessageSubject { get; set; }
        
        [Column("MessageBody")]
        [NotNull]
        [StringLength(2000)]
        public string MessageBody { get; set; }
        
        [Column("SentDate")]
        public DateTime? SentDate { get; set; }
        
        [Column("Sent")]
        public bool? Sent { get; set; }
        
        [Column("IsSentError")]
        public bool? IsSentError { get; set; }
        
        [Column("SentErrorMessage")]
        [StringLength(500)]
        public string SentErrorMessage { get; set; }
        
        [Column("SentErrorDate")]
        public DateTime? SentErrorDate { get; set; }
        
        [Column("RetryCount")]
        [NotNull]
        public long RetryCount { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"MessageToSendId = {MessageToSendId} , ");
            sb.AppendLine($"PredefinedMessageToSendTypeId = {PredefinedMessageToSendTypeId} , ");
            sb.AppendLine($"RealtimeMessageId = {RealtimeMessageId} , ");
            sb.AppendLine($"MessageFrom = {MessageFrom} , ");
            sb.AppendLine($"MessageTo = {MessageTo} , ");
            sb.AppendLine($"MessageFromName = {MessageFromName} , ");
            sb.AppendLine($"MessageToName = {MessageToName} , ");
            sb.AppendLine($"MessageSubject = {MessageSubject} , ");
            sb.AppendLine($"MessageBody = {MessageBody} , ");
            sb.AppendLine($"SentDate = {SentDate} , ");
            sb.AppendLine($"Sent = {Sent} , ");
            sb.AppendLine($"IsSentError = {IsSentError} , ");
            sb.AppendLine($"SentErrorMessage = {SentErrorMessage} , ");
            sb.AppendLine($"SentErrorDate = {SentErrorDate} , ");
            sb.AppendLine($"RetryCount = {RetryCount} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public MessageToSend New(MessageToSend item)
        {
            if (item == null)
                return null;

            return new MessageToSend
            {
            MessageToSendId = item.MessageToSendId,
            PredefinedMessageToSendTypeId = item.PredefinedMessageToSendTypeId,
            RealtimeMessageId = item.RealtimeMessageId,
            MessageFrom = item.MessageFrom,
            MessageTo = item.MessageTo,
            MessageFromName = item.MessageFromName,
            MessageToName = item.MessageToName,
            MessageSubject = item.MessageSubject,
            MessageBody = item.MessageBody,
            SentDate = item.SentDate,
            Sent = item.Sent,
            IsSentError = item.IsSentError,
            SentErrorMessage = item.SentErrorMessage,
            SentErrorDate = item.SentErrorDate,
            RetryCount = item.RetryCount,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public MessageToSend NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("NoteAttachment")]
    [Serializable]
    public partial class NoteAttachment : IAuditableModel, INewObject<NoteAttachment>
    {
        public const string TABLE_NAME = "NoteAttachment";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_NoteAttachmentId = "NoteAttachmentId";
        public const string COLUMN_Title = "Title";
        public const string COLUMN_Description = "Description";
        public const string COLUMN_UserId = "UserId";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_Title = 512;
        public const int COLUMN_SIZE_Description = 512;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_NoteAttachmentId = false;
        public const bool COLUMN_NULLABLE_Title = false;
        public const bool COLUMN_NULLABLE_Description = false;
        public const bool COLUMN_NULLABLE_UserId = false;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("NoteAttachmentId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long NoteAttachmentId { get; set; }
        
        [Column("Title")]
        [NotNull]
        [StringLength(512)]
        public string Title { get; set; }
        
        [Column("Description")]
        [NotNull]
        [StringLength(512)]
        public string Description { get; set; }
        
        [Column("UserId")]
        [NotNull]
        public long UserId { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"NoteAttachmentId = {NoteAttachmentId} , ");
            sb.AppendLine($"Title = {Title} , ");
            sb.AppendLine($"Description = {Description} , ");
            sb.AppendLine($"UserId = {UserId} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public NoteAttachment New(NoteAttachment item)
        {
            if (item == null)
                return null;

            return new NoteAttachment
            {
            NoteAttachmentId = item.NoteAttachmentId,
            Title = item.Title,
            Description = item.Description,
            UserId = item.UserId,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public NoteAttachment NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("PushNotificationServer")]
    [Serializable]
    public partial class PushNotificationServer : IAuditableModel, INewObject<PushNotificationServer>
    {
        public const string TABLE_NAME = "PushNotificationServer";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_PushNotificationServerId = "PushNotificationServerId";
        public const string COLUMN_PushNotificationServerCode = "PushNotificationServerCode";
        public const string COLUMN_PushNotificationServerName = "PushNotificationServerName";
        public const string COLUMN_PredefinedPushNotificationServerTypeId = "PredefinedPushNotificationServerTypeId";
        public const string COLUMN_ProjectId = "ProjectId";
        public const string COLUMN_ApiKey = "ApiKey";
        public const string COLUMN_ApiSecret = "ApiSecret";
        public const string COLUMN_AppleTeamId = "AppleTeamId";
        public const string COLUMN_AppleKeyId = "AppleKeyId";
        public const string COLUMN_AppleAuthKey = "AppleAuthKey";
        public const string COLUMN_IsEnabled = "IsEnabled";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_PushNotificationServerCode = 45;
        public const int COLUMN_SIZE_PushNotificationServerName = 100;
        public const int COLUMN_SIZE_ProjectId = 100;
        public const int COLUMN_SIZE_ApiKey = 255;
        public const int COLUMN_SIZE_ApiSecret = 255;
        public const int COLUMN_SIZE_AppleTeamId = 100;
        public const int COLUMN_SIZE_AppleKeyId = 100;
        public const int COLUMN_SIZE_AppleAuthKey = 2000;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_PushNotificationServerId = false;
        public const bool COLUMN_NULLABLE_PushNotificationServerCode = false;
        public const bool COLUMN_NULLABLE_PushNotificationServerName = false;
        public const bool COLUMN_NULLABLE_PredefinedPushNotificationServerTypeId = false;
        public const bool COLUMN_NULLABLE_ProjectId = true;
        public const bool COLUMN_NULLABLE_ApiKey = true;
        public const bool COLUMN_NULLABLE_ApiSecret = true;
        public const bool COLUMN_NULLABLE_AppleTeamId = true;
        public const bool COLUMN_NULLABLE_AppleKeyId = true;
        public const bool COLUMN_NULLABLE_AppleAuthKey = true;
        public const bool COLUMN_NULLABLE_IsEnabled = false;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("PushNotificationServerId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long PushNotificationServerId { get; set; }
        
        [Column("PushNotificationServerCode")]
        [NotNull]
        [StringLength(45)]
        public string PushNotificationServerCode { get; set; }
        
        [Column("PushNotificationServerName")]
        [NotNull]
        [StringLength(100)]
        public string PushNotificationServerName { get; set; }
        
        [Column("PredefinedPushNotificationServerTypeId")]
        [NotNull]
        public long PredefinedPushNotificationServerTypeId { get; set; }
        
        [Column("ProjectId")]
        [StringLength(100)]
        public string ProjectId { get; set; }
        
        [Column("ApiKey")]
        [StringLength(255)]
        public string ApiKey { get; set; }
        
        [Column("ApiSecret")]
        [StringLength(255)]
        public string ApiSecret { get; set; }
        
        [Column("AppleTeamId")]
        [StringLength(100)]
        public string AppleTeamId { get; set; }
        
        [Column("AppleKeyId")]
        [StringLength(100)]
        public string AppleKeyId { get; set; }
        
        [Column("AppleAuthKey")]
        [StringLength(2000)]
        public string AppleAuthKey { get; set; }
        
        [Column("IsEnabled")]
        [NotNull]
        public bool IsEnabled { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"PushNotificationServerId = {PushNotificationServerId} , ");
            sb.AppendLine($"PushNotificationServerCode = {PushNotificationServerCode} , ");
            sb.AppendLine($"PushNotificationServerName = {PushNotificationServerName} , ");
            sb.AppendLine($"PredefinedPushNotificationServerTypeId = {PredefinedPushNotificationServerTypeId} , ");
            sb.AppendLine($"ProjectId = {ProjectId} , ");
            sb.AppendLine($"ApiKey = {ApiKey} , ");
            sb.AppendLine($"ApiSecret = {ApiSecret} , ");
            sb.AppendLine($"AppleTeamId = {AppleTeamId} , ");
            sb.AppendLine($"AppleKeyId = {AppleKeyId} , ");
            sb.AppendLine($"AppleAuthKey = {AppleAuthKey} , ");
            sb.AppendLine($"IsEnabled = {IsEnabled} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public PushNotificationServer New(PushNotificationServer item)
        {
            if (item == null)
                return null;

            return new PushNotificationServer
            {
            PushNotificationServerId = item.PushNotificationServerId,
            PushNotificationServerCode = item.PushNotificationServerCode,
            PushNotificationServerName = item.PushNotificationServerName,
            PredefinedPushNotificationServerTypeId = item.PredefinedPushNotificationServerTypeId,
            ProjectId = item.ProjectId,
            ApiKey = item.ApiKey,
            ApiSecret = item.ApiSecret,
            AppleTeamId = item.AppleTeamId,
            AppleKeyId = item.AppleKeyId,
            AppleAuthKey = item.AppleAuthKey,
            IsEnabled = item.IsEnabled,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public PushNotificationServer NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("RealtimeMessage")]
    [Serializable]
    public partial class RealtimeMessage : IAuditableModel, INewObject<RealtimeMessage>
    {
        public const string TABLE_NAME = "RealtimeMessage";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_RealtimeMessageId = "RealtimeMessageId";
        public const string COLUMN_PredefinedMessagePriorityTypeId = "PredefinedMessagePriorityTypeId";
        public const string COLUMN_MessageDateTime = "MessageDateTime";
        public const string COLUMN_MessageSubject = "MessageSubject";
        public const string COLUMN_MessageContent = "MessageContent";
        public const string COLUMN_MessageSentDateTime = "MessageSentDateTime";
        public const string COLUMN_MessageViewedDateTime = "MessageViewedDateTime";
        public const string COLUMN_PredefinedRealtimeMessageStatusTypeId = "PredefinedRealtimeMessageStatusTypeId";
        public const string COLUMN_PredefinedNotificationMessageTypeId = "PredefinedNotificationMessageTypeId";
        public const string COLUMN_ResourceName = "ResourceName";
        public const string COLUMN_PredefinedResourceTypeId = "PredefinedResourceTypeId";
        public const string COLUMN_ResourceId = "ResourceId";
        public const string COLUMN_ErrorMessage = "ErrorMessage";
        public const string COLUMN_ErrorOccurredOn = "ErrorOccurredOn";
        public const string COLUMN_UserId = "UserId";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_MessageSubject = 255;
        public const int COLUMN_SIZE_MessageContent = 255;
        public const int COLUMN_SIZE_ResourceName = 100;
        public const int COLUMN_SIZE_ErrorMessage = 2000;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_RealtimeMessageId = false;
        public const bool COLUMN_NULLABLE_PredefinedMessagePriorityTypeId = false;
        public const bool COLUMN_NULLABLE_MessageDateTime = false;
        public const bool COLUMN_NULLABLE_MessageSubject = false;
        public const bool COLUMN_NULLABLE_MessageContent = false;
        public const bool COLUMN_NULLABLE_MessageSentDateTime = true;
        public const bool COLUMN_NULLABLE_MessageViewedDateTime = true;
        public const bool COLUMN_NULLABLE_PredefinedRealtimeMessageStatusTypeId = false;
        public const bool COLUMN_NULLABLE_PredefinedNotificationMessageTypeId = false;
        public const bool COLUMN_NULLABLE_ResourceName = true;
        public const bool COLUMN_NULLABLE_PredefinedResourceTypeId = true;
        public const bool COLUMN_NULLABLE_ResourceId = true;
        public const bool COLUMN_NULLABLE_ErrorMessage = true;
        public const bool COLUMN_NULLABLE_ErrorOccurredOn = true;
        public const bool COLUMN_NULLABLE_UserId = false;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("RealtimeMessageId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long RealtimeMessageId { get; set; }
        
        [Column("PredefinedMessagePriorityTypeId")]
        [NotNull]
        public long PredefinedMessagePriorityTypeId { get; set; }
        
        [Column("MessageDateTime")]
        [NotNull]
        public DateTime MessageDateTime { get; set; }
        
        [Column("MessageSubject")]
        [NotNull]
        [StringLength(255)]
        public string MessageSubject { get; set; }
        
        [Column("MessageContent")]
        [NotNull]
        [StringLength(255)]
        public string MessageContent { get; set; }
        
        [Column("MessageSentDateTime")]
        public DateTime? MessageSentDateTime { get; set; }
        
        [Column("MessageViewedDateTime")]
        public DateTime? MessageViewedDateTime { get; set; }
        
        [Column("PredefinedRealtimeMessageStatusTypeId")]
        [NotNull]
        public long PredefinedRealtimeMessageStatusTypeId { get; set; }
        
        [Column("PredefinedNotificationMessageTypeId")]
        [NotNull]
        public long PredefinedNotificationMessageTypeId { get; set; }
        
        [Column("ResourceName")]
        [StringLength(100)]
        public string ResourceName { get; set; }
        
        [Column("PredefinedResourceTypeId")]
        public long? PredefinedResourceTypeId { get; set; }
        
        [Column("ResourceId")]
        public long? ResourceId { get; set; }
        
        [Column("ErrorMessage")]
        [StringLength(2000)]
        public string ErrorMessage { get; set; }
        
        [Column("ErrorOccurredOn")]
        public DateTime? ErrorOccurredOn { get; set; }
        
        [Column("UserId")]
        [NotNull]
        public long UserId { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"RealtimeMessageId = {RealtimeMessageId} , ");
            sb.AppendLine($"PredefinedMessagePriorityTypeId = {PredefinedMessagePriorityTypeId} , ");
            sb.AppendLine($"MessageDateTime = {MessageDateTime} , ");
            sb.AppendLine($"MessageSubject = {MessageSubject} , ");
            sb.AppendLine($"MessageContent = {MessageContent} , ");
            sb.AppendLine($"MessageSentDateTime = {MessageSentDateTime} , ");
            sb.AppendLine($"MessageViewedDateTime = {MessageViewedDateTime} , ");
            sb.AppendLine($"PredefinedRealtimeMessageStatusTypeId = {PredefinedRealtimeMessageStatusTypeId} , ");
            sb.AppendLine($"PredefinedNotificationMessageTypeId = {PredefinedNotificationMessageTypeId} , ");
            sb.AppendLine($"ResourceName = {ResourceName} , ");
            sb.AppendLine($"PredefinedResourceTypeId = {PredefinedResourceTypeId} , ");
            sb.AppendLine($"ResourceId = {ResourceId} , ");
            sb.AppendLine($"ErrorMessage = {ErrorMessage} , ");
            sb.AppendLine($"ErrorOccurredOn = {ErrorOccurredOn} , ");
            sb.AppendLine($"UserId = {UserId} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public RealtimeMessage New(RealtimeMessage item)
        {
            if (item == null)
                return null;

            return new RealtimeMessage
            {
            RealtimeMessageId = item.RealtimeMessageId,
            PredefinedMessagePriorityTypeId = item.PredefinedMessagePriorityTypeId,
            MessageDateTime = item.MessageDateTime,
            MessageSubject = item.MessageSubject,
            MessageContent = item.MessageContent,
            MessageSentDateTime = item.MessageSentDateTime,
            MessageViewedDateTime = item.MessageViewedDateTime,
            PredefinedRealtimeMessageStatusTypeId = item.PredefinedRealtimeMessageStatusTypeId,
            PredefinedNotificationMessageTypeId = item.PredefinedNotificationMessageTypeId,
            ResourceName = item.ResourceName,
            PredefinedResourceTypeId = item.PredefinedResourceTypeId,
            ResourceId = item.ResourceId,
            ErrorMessage = item.ErrorMessage,
            ErrorOccurredOn = item.ErrorOccurredOn,
            UserId = item.UserId,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public RealtimeMessage NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("SavedJob")]
    [Serializable]
    public partial class SavedJob : IAuditableModel, INewObject<SavedJob>
    {
        public const string TABLE_NAME = "SavedJob";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_SavedJobId = "SavedJobId";
        public const string COLUMN_PredefinedJobTypeId = "PredefinedJobTypeId";
        public const string COLUMN_PredefinedJobOrReportTypeId = "PredefinedJobOrReportTypeId";
        public const string COLUMN_JobTemplateId = "JobTemplateId";
        public const string COLUMN_CompletedDate = "CompletedDate";
        public const string COLUMN_SubmittedDate = "SubmittedDate";
        public const string COLUMN_SubmittedByUserId = "SubmittedByUserId";
        public const string COLUMN_ExecutableFolder = "ExecutableFolder";
        public const string COLUMN_ExecutableName = "ExecutableName";
        public const string COLUMN_ExecutableParameter = "ExecutableParameter";
        public const string COLUMN_SerializedJobParameter = "SerializedJobParameter";
        public const string COLUMN_ReportTemplateFile = "ReportTemplateFile";
        public const string COLUMN_ExportDoc = "ExportDOC";
        public const string COLUMN_ExportXls = "ExportXLS";
        public const string COLUMN_ExportPpt = "ExportPPT";
        public const string COLUMN_ExportCsv = "ExportCSV";
        public const string COLUMN_ExportXml = "ExportXML";
        public const string COLUMN_ExportPdf = "ExportPDF";
        public const string COLUMN_ExportCustom = "ExportCustom";
        public const string COLUMN_CustomExportName = "CustomExportName";
        public const string COLUMN_PickupDate = "PickupDate";
        public const string COLUMN_JobName = "JobName";
        public const string COLUMN_UserId = "UserId";
        public const string COLUMN_IncludeDataSourceFile = "IncludeDataSourceFile";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_ExecutableFolder = 200;
        public const int COLUMN_SIZE_ExecutableName = 200;
        public const int COLUMN_SIZE_ExecutableParameter = 200;
        public const int COLUMN_SIZE_SerializedJobParameter = 200;
        public const int COLUMN_SIZE_ReportTemplateFile = 200;
        public const int COLUMN_SIZE_CustomExportName = 20;
        public const int COLUMN_SIZE_JobName = 200;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_SavedJobId = false;
        public const bool COLUMN_NULLABLE_PredefinedJobTypeId = false;
        public const bool COLUMN_NULLABLE_PredefinedJobOrReportTypeId = true;
        public const bool COLUMN_NULLABLE_JobTemplateId = false;
        public const bool COLUMN_NULLABLE_CompletedDate = true;
        public const bool COLUMN_NULLABLE_SubmittedDate = false;
        public const bool COLUMN_NULLABLE_SubmittedByUserId = true;
        public const bool COLUMN_NULLABLE_ExecutableFolder = false;
        public const bool COLUMN_NULLABLE_ExecutableName = false;
        public const bool COLUMN_NULLABLE_ExecutableParameter = true;
        public const bool COLUMN_NULLABLE_SerializedJobParameter = true;
        public const bool COLUMN_NULLABLE_ReportTemplateFile = true;
        public const bool COLUMN_NULLABLE_ExportDoc = false;
        public const bool COLUMN_NULLABLE_ExportXls = false;
        public const bool COLUMN_NULLABLE_ExportPpt = false;
        public const bool COLUMN_NULLABLE_ExportCsv = false;
        public const bool COLUMN_NULLABLE_ExportXml = false;
        public const bool COLUMN_NULLABLE_ExportPdf = false;
        public const bool COLUMN_NULLABLE_ExportCustom = false;
        public const bool COLUMN_NULLABLE_CustomExportName = true;
        public const bool COLUMN_NULLABLE_PickupDate = true;
        public const bool COLUMN_NULLABLE_JobName = true;
        public const bool COLUMN_NULLABLE_UserId = true;
        public const bool COLUMN_NULLABLE_IncludeDataSourceFile = false;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("SavedJobId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long SavedJobId { get; set; }
        
        [Column("PredefinedJobTypeId")]
        [NotNull]
        public long PredefinedJobTypeId { get; set; }
        
        [Column("PredefinedJobOrReportTypeId")]
        public long? PredefinedJobOrReportTypeId { get; set; }
        
        [Column("JobTemplateId")]
        [NotNull]
        public long JobTemplateId { get; set; }
        
        [Column("CompletedDate")]
        public DateTime? CompletedDate { get; set; }
        
        [Column("SubmittedDate")]
        [NotNull]
        public DateTime SubmittedDate { get; set; }
        
        [Column("SubmittedByUserId")]
        public long? SubmittedByUserId { get; set; }
        
        [Column("ExecutableFolder")]
        [NotNull]
        [StringLength(200)]
        public string ExecutableFolder { get; set; }
        
        [Column("ExecutableName")]
        [NotNull]
        [StringLength(200)]
        public string ExecutableName { get; set; }
        
        [Column("ExecutableParameter")]
        [StringLength(200)]
        public string ExecutableParameter { get; set; }
        
        [Column("SerializedJobParameter")]
        [StringLength(200)]
        public string SerializedJobParameter { get; set; }
        
        [Column("ReportTemplateFile")]
        [StringLength(200)]
        public string ReportTemplateFile { get; set; }
        
        [Column("ExportDOC")]
        [NotNull]
        public bool ExportDoc { get; set; }
        
        [Column("ExportXLS")]
        [NotNull]
        public bool ExportXls { get; set; }
        
        [Column("ExportPPT")]
        [NotNull]
        public bool ExportPpt { get; set; }
        
        [Column("ExportCSV")]
        [NotNull]
        public bool ExportCsv { get; set; }
        
        [Column("ExportXML")]
        [NotNull]
        public bool ExportXml { get; set; }
        
        [Column("ExportPDF")]
        [NotNull]
        public bool ExportPdf { get; set; }
        
        [Column("ExportCustom")]
        [NotNull]
        public bool ExportCustom { get; set; }
        
        [Column("CustomExportName")]
        [StringLength(20)]
        public string CustomExportName { get; set; }
        
        [Column("PickupDate")]
        public DateTime? PickupDate { get; set; }
        
        [Column("JobName")]
        [StringLength(200)]
        public string JobName { get; set; }
        
        [Column("UserId")]
        public long? UserId { get; set; }
        
        [Column("IncludeDataSourceFile")]
        [NotNull]
        public bool IncludeDataSourceFile { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"SavedJobId = {SavedJobId} , ");
            sb.AppendLine($"PredefinedJobTypeId = {PredefinedJobTypeId} , ");
            sb.AppendLine($"PredefinedJobOrReportTypeId = {PredefinedJobOrReportTypeId} , ");
            sb.AppendLine($"JobTemplateId = {JobTemplateId} , ");
            sb.AppendLine($"CompletedDate = {CompletedDate} , ");
            sb.AppendLine($"SubmittedDate = {SubmittedDate} , ");
            sb.AppendLine($"SubmittedByUserId = {SubmittedByUserId} , ");
            sb.AppendLine($"ExecutableFolder = {ExecutableFolder} , ");
            sb.AppendLine($"ExecutableName = {ExecutableName} , ");
            sb.AppendLine($"ExecutableParameter = {ExecutableParameter} , ");
            sb.AppendLine($"SerializedJobParameter = {SerializedJobParameter} , ");
            sb.AppendLine($"ReportTemplateFile = {ReportTemplateFile} , ");
            sb.AppendLine($"ExportDoc = {ExportDoc} , ");
            sb.AppendLine($"ExportXls = {ExportXls} , ");
            sb.AppendLine($"ExportPpt = {ExportPpt} , ");
            sb.AppendLine($"ExportCsv = {ExportCsv} , ");
            sb.AppendLine($"ExportXml = {ExportXml} , ");
            sb.AppendLine($"ExportPdf = {ExportPdf} , ");
            sb.AppendLine($"ExportCustom = {ExportCustom} , ");
            sb.AppendLine($"CustomExportName = {CustomExportName} , ");
            sb.AppendLine($"PickupDate = {PickupDate} , ");
            sb.AppendLine($"JobName = {JobName} , ");
            sb.AppendLine($"UserId = {UserId} , ");
            sb.AppendLine($"IncludeDataSourceFile = {IncludeDataSourceFile} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public SavedJob New(SavedJob item)
        {
            if (item == null)
                return null;

            return new SavedJob
            {
            SavedJobId = item.SavedJobId,
            PredefinedJobTypeId = item.PredefinedJobTypeId,
            PredefinedJobOrReportTypeId = item.PredefinedJobOrReportTypeId,
            JobTemplateId = item.JobTemplateId,
            CompletedDate = item.CompletedDate,
            SubmittedDate = item.SubmittedDate,
            SubmittedByUserId = item.SubmittedByUserId,
            ExecutableFolder = item.ExecutableFolder,
            ExecutableName = item.ExecutableName,
            ExecutableParameter = item.ExecutableParameter,
            SerializedJobParameter = item.SerializedJobParameter,
            ReportTemplateFile = item.ReportTemplateFile,
            ExportDoc = item.ExportDoc,
            ExportXls = item.ExportXls,
            ExportPpt = item.ExportPpt,
            ExportCsv = item.ExportCsv,
            ExportXml = item.ExportXml,
            ExportPdf = item.ExportPdf,
            ExportCustom = item.ExportCustom,
            CustomExportName = item.CustomExportName,
            PickupDate = item.PickupDate,
            JobName = item.JobName,
            UserId = item.UserId,
            IncludeDataSourceFile = item.IncludeDataSourceFile,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public SavedJob NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("SavedJobNotificationEmail")]
    [Serializable]
    public partial class SavedJobNotificationEmail : IAuditableModel, INewObject<SavedJobNotificationEmail>
    {
        public const string TABLE_NAME = "SavedJobNotificationEmail";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_SavedJobNotificationEmailId = "SavedJobNotificationEmailId";
        public const string COLUMN_SavedJobId = "SavedJobId";
        public const string COLUMN_Email = "Email";
        public const string COLUMN_SendStatus = "SendStatus";
        public const string COLUMN_Message = "Message";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_Email = 200;
        public const int COLUMN_SIZE_Message = 200;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_SavedJobNotificationEmailId = false;
        public const bool COLUMN_NULLABLE_SavedJobId = false;
        public const bool COLUMN_NULLABLE_Email = false;
        public const bool COLUMN_NULLABLE_SendStatus = false;
        public const bool COLUMN_NULLABLE_Message = true;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("SavedJobNotificationEmailId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long SavedJobNotificationEmailId { get; set; }
        
        [Column("SavedJobId")]
        [NotNull]
        public long SavedJobId { get; set; }
        
        [Column("Email")]
        [NotNull]
        [StringLength(200)]
        public string Email { get; set; }
        
        [Column("SendStatus")]
        [NotNull]
        public long SendStatus { get; set; }
        
        [Column("Message")]
        [StringLength(200)]
        public string Message { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"SavedJobNotificationEmailId = {SavedJobNotificationEmailId} , ");
            sb.AppendLine($"SavedJobId = {SavedJobId} , ");
            sb.AppendLine($"Email = {Email} , ");
            sb.AppendLine($"SendStatus = {SendStatus} , ");
            sb.AppendLine($"Message = {Message} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public SavedJobNotificationEmail New(SavedJobNotificationEmail item)
        {
            if (item == null)
                return null;

            return new SavedJobNotificationEmail
            {
            SavedJobNotificationEmailId = item.SavedJobNotificationEmailId,
            SavedJobId = item.SavedJobId,
            Email = item.Email,
            SendStatus = item.SendStatus,
            Message = item.Message,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public SavedJobNotificationEmail NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("ScheduledJob")]
    [Serializable]
    public partial class ScheduledJob : IAuditableModel, INewObject<ScheduledJob>
    {
        public const string TABLE_NAME = "ScheduledJob";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_ScheduledJobId = "ScheduledJobId";
        public const string COLUMN_JobTemplateId = "JobTemplateId";
        public const string COLUMN_PredefinedJobTypeId = "PredefinedJobTypeId";
        public const string COLUMN_PredefinedJobOrReportTypeId = "PredefinedJobOrReportTypeId";
        public const string COLUMN_SubmittedByUserId = "SubmittedByUserId";
        public const string COLUMN_ExecutableFolder = "ExecutableFolder";
        public const string COLUMN_ExecutableName = "ExecutableName";
        public const string COLUMN_ExecutableParameter = "ExecutableParameter";
        public const string COLUMN_SerializedJobParameter = "SerializedJobParameter";
        public const string COLUMN_ReportTemplateFile = "ReportTemplateFile";
        public const string COLUMN_ExportDoc = "ExportDOC";
        public const string COLUMN_ExportXls = "ExportXLS";
        public const string COLUMN_ExportPpt = "ExportPPT";
        public const string COLUMN_ExportCsv = "ExportCSV";
        public const string COLUMN_ExportXml = "ExportXML";
        public const string COLUMN_ExportPdf = "ExportPDF";
        public const string COLUMN_ExportCustom = "ExportCustom";
        public const string COLUMN_CustomExportName = "CustomExportName";
        public const string COLUMN_PickupDate = "PickupDate";
        public const string COLUMN_PickupTime = "PickupTime";
        public const string COLUMN_JobName = "JobName";
        public const string COLUMN_RunAsUserId = "RunAsUserId";
        public const string COLUMN_StartDate = "StartDate";
        public const string COLUMN_EndDate = "EndDate";
        public const string COLUMN_StartTime = "StartTime";
        public const string COLUMN_Recurring = "Recurring";
        public const string COLUMN_RunMonday = "RunMonday";
        public const string COLUMN_RunTuesday = "RunTuesday";
        public const string COLUMN_RunWednesday = "RunWednesday";
        public const string COLUMN_RunThursday = "RunThursday";
        public const string COLUMN_RunFriday = "RunFriday";
        public const string COLUMN_RunSaturday = "RunSaturday";
        public const string COLUMN_RunSunday = "RunSunday";
        public const string COLUMN_Active = "Active";
        public const string COLUMN_IsMonthly = "IsMonthly";
        public const string COLUMN_RunMonthlyDay = "RunMonthlyDay";
        public const string COLUMN_RunMonthlyTime = "RunMonthlyTime";
        public const string COLUMN_IsHourly = "IsHourly";
        public const string COLUMN_IsDaily = "IsDaily";
        public const string COLUMN_RunDailyTime = "RunDailyTime";
        public const string COLUMN_IsWeekly = "IsWeekly";
        public const string COLUMN_RunHourlyMinute = "RunHourlyMinute";
        public const string COLUMN_IncludeDataSourceFile = "IncludeDataSourceFile";
        public const string COLUMN_RunOnWeekNumber = "RunOnWeekNumber";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_ExecutableFolder = 200;
        public const int COLUMN_SIZE_ExecutableName = 200;
        public const int COLUMN_SIZE_ExecutableParameter = 200;
        public const int COLUMN_SIZE_SerializedJobParameter = 200;
        public const int COLUMN_SIZE_ReportTemplateFile = 200;
        public const int COLUMN_SIZE_CustomExportName = 20;
        public const int COLUMN_SIZE_JobName = 200;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_ScheduledJobId = false;
        public const bool COLUMN_NULLABLE_JobTemplateId = false;
        public const bool COLUMN_NULLABLE_PredefinedJobTypeId = true;
        public const bool COLUMN_NULLABLE_PredefinedJobOrReportTypeId = true;
        public const bool COLUMN_NULLABLE_SubmittedByUserId = true;
        public const bool COLUMN_NULLABLE_ExecutableFolder = false;
        public const bool COLUMN_NULLABLE_ExecutableName = false;
        public const bool COLUMN_NULLABLE_ExecutableParameter = true;
        public const bool COLUMN_NULLABLE_SerializedJobParameter = true;
        public const bool COLUMN_NULLABLE_ReportTemplateFile = true;
        public const bool COLUMN_NULLABLE_ExportDoc = false;
        public const bool COLUMN_NULLABLE_ExportXls = false;
        public const bool COLUMN_NULLABLE_ExportPpt = false;
        public const bool COLUMN_NULLABLE_ExportCsv = false;
        public const bool COLUMN_NULLABLE_ExportXml = false;
        public const bool COLUMN_NULLABLE_ExportPdf = false;
        public const bool COLUMN_NULLABLE_ExportCustom = false;
        public const bool COLUMN_NULLABLE_CustomExportName = true;
        public const bool COLUMN_NULLABLE_PickupDate = true;
        public const bool COLUMN_NULLABLE_PickupTime = true;
        public const bool COLUMN_NULLABLE_JobName = true;
        public const bool COLUMN_NULLABLE_RunAsUserId = true;
        public const bool COLUMN_NULLABLE_StartDate = true;
        public const bool COLUMN_NULLABLE_EndDate = true;
        public const bool COLUMN_NULLABLE_StartTime = true;
        public const bool COLUMN_NULLABLE_Recurring = false;
        public const bool COLUMN_NULLABLE_RunMonday = false;
        public const bool COLUMN_NULLABLE_RunTuesday = false;
        public const bool COLUMN_NULLABLE_RunWednesday = false;
        public const bool COLUMN_NULLABLE_RunThursday = false;
        public const bool COLUMN_NULLABLE_RunFriday = false;
        public const bool COLUMN_NULLABLE_RunSaturday = false;
        public const bool COLUMN_NULLABLE_RunSunday = false;
        public const bool COLUMN_NULLABLE_Active = false;
        public const bool COLUMN_NULLABLE_IsMonthly = false;
        public const bool COLUMN_NULLABLE_RunMonthlyDay = true;
        public const bool COLUMN_NULLABLE_RunMonthlyTime = true;
        public const bool COLUMN_NULLABLE_IsHourly = false;
        public const bool COLUMN_NULLABLE_IsDaily = false;
        public const bool COLUMN_NULLABLE_RunDailyTime = true;
        public const bool COLUMN_NULLABLE_IsWeekly = false;
        public const bool COLUMN_NULLABLE_RunHourlyMinute = true;
        public const bool COLUMN_NULLABLE_IncludeDataSourceFile = false;
        public const bool COLUMN_NULLABLE_RunOnWeekNumber = true;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("ScheduledJobId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long ScheduledJobId { get; set; }
        
        [Column("JobTemplateId")]
        [NotNull]
        public long JobTemplateId { get; set; }
        
        [Column("PredefinedJobTypeId")]
        public long? PredefinedJobTypeId { get; set; }
        
        [Column("PredefinedJobOrReportTypeId")]
        public long? PredefinedJobOrReportTypeId { get; set; }
        
        [Column("SubmittedByUserId")]
        public long? SubmittedByUserId { get; set; }
        
        [Column("ExecutableFolder")]
        [NotNull]
        [StringLength(200)]
        public string ExecutableFolder { get; set; }
        
        [Column("ExecutableName")]
        [NotNull]
        [StringLength(200)]
        public string ExecutableName { get; set; }
        
        [Column("ExecutableParameter")]
        [StringLength(200)]
        public string ExecutableParameter { get; set; }
        
        [Column("SerializedJobParameter")]
        [StringLength(200)]
        public string SerializedJobParameter { get; set; }
        
        [Column("ReportTemplateFile")]
        [StringLength(200)]
        public string ReportTemplateFile { get; set; }
        
        [Column("ExportDOC")]
        [NotNull]
        public bool ExportDoc { get; set; }
        
        [Column("ExportXLS")]
        [NotNull]
        public bool ExportXls { get; set; }
        
        [Column("ExportPPT")]
        [NotNull]
        public bool ExportPpt { get; set; }
        
        [Column("ExportCSV")]
        [NotNull]
        public bool ExportCsv { get; set; }
        
        [Column("ExportXML")]
        [NotNull]
        public bool ExportXml { get; set; }
        
        [Column("ExportPDF")]
        [NotNull]
        public bool ExportPdf { get; set; }
        
        [Column("ExportCustom")]
        [NotNull]
        public bool ExportCustom { get; set; }
        
        [Column("CustomExportName")]
        [StringLength(20)]
        public string CustomExportName { get; set; }
        
        [Column("PickupDate")]
        public DateTime? PickupDate { get; set; }
        
        [Column("PickupTime")]
        public long? PickupTime { get; set; }
        
        [Column("JobName")]
        [StringLength(200)]
        public string JobName { get; set; }
        
        [Column("RunAsUserId")]
        public long? RunAsUserId { get; set; }
        
        [Column("StartDate")]
        public DateTime? StartDate { get; set; }
        
        [Column("EndDate")]
        public DateTime? EndDate { get; set; }
        
        [Column("StartTime")]
        public DateTime? StartTime { get; set; }
        
        [Column("Recurring")]
        [NotNull]
        public bool Recurring { get; set; }
        
        [Column("RunMonday")]
        [NotNull]
        public bool RunMonday { get; set; }
        
        [Column("RunTuesday")]
        [NotNull]
        public bool RunTuesday { get; set; }
        
        [Column("RunWednesday")]
        [NotNull]
        public bool RunWednesday { get; set; }
        
        [Column("RunThursday")]
        [NotNull]
        public bool RunThursday { get; set; }
        
        [Column("RunFriday")]
        [NotNull]
        public bool RunFriday { get; set; }
        
        [Column("RunSaturday")]
        [NotNull]
        public bool RunSaturday { get; set; }
        
        [Column("RunSunday")]
        [NotNull]
        public bool RunSunday { get; set; }
        
        [Column("Active")]
        [NotNull]
        public bool Active { get; set; }
        
        [Column("IsMonthly")]
        [NotNull]
        public bool IsMonthly { get; set; }
        
        [Column("RunMonthlyDay")]
        public long? RunMonthlyDay { get; set; }
        
        [Column("RunMonthlyTime")]
        public DateTime? RunMonthlyTime { get; set; }
        
        [Column("IsHourly")]
        [NotNull]
        public bool IsHourly { get; set; }
        
        [Column("IsDaily")]
        [NotNull]
        public bool IsDaily { get; set; }
        
        [Column("RunDailyTime")]
        public DateTime? RunDailyTime { get; set; }
        
        [Column("IsWeekly")]
        [NotNull]
        public bool IsWeekly { get; set; }
        
        [Column("RunHourlyMinute")]
        public long? RunHourlyMinute { get; set; }
        
        [Column("IncludeDataSourceFile")]
        [NotNull]
        public bool IncludeDataSourceFile { get; set; }
        
        [Column("RunOnWeekNumber")]
        public long? RunOnWeekNumber { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"ScheduledJobId = {ScheduledJobId} , ");
            sb.AppendLine($"JobTemplateId = {JobTemplateId} , ");
            sb.AppendLine($"PredefinedJobTypeId = {PredefinedJobTypeId} , ");
            sb.AppendLine($"PredefinedJobOrReportTypeId = {PredefinedJobOrReportTypeId} , ");
            sb.AppendLine($"SubmittedByUserId = {SubmittedByUserId} , ");
            sb.AppendLine($"ExecutableFolder = {ExecutableFolder} , ");
            sb.AppendLine($"ExecutableName = {ExecutableName} , ");
            sb.AppendLine($"ExecutableParameter = {ExecutableParameter} , ");
            sb.AppendLine($"SerializedJobParameter = {SerializedJobParameter} , ");
            sb.AppendLine($"ReportTemplateFile = {ReportTemplateFile} , ");
            sb.AppendLine($"ExportDoc = {ExportDoc} , ");
            sb.AppendLine($"ExportXls = {ExportXls} , ");
            sb.AppendLine($"ExportPpt = {ExportPpt} , ");
            sb.AppendLine($"ExportCsv = {ExportCsv} , ");
            sb.AppendLine($"ExportXml = {ExportXml} , ");
            sb.AppendLine($"ExportPdf = {ExportPdf} , ");
            sb.AppendLine($"ExportCustom = {ExportCustom} , ");
            sb.AppendLine($"CustomExportName = {CustomExportName} , ");
            sb.AppendLine($"PickupDate = {PickupDate} , ");
            sb.AppendLine($"PickupTime = {PickupTime} , ");
            sb.AppendLine($"JobName = {JobName} , ");
            sb.AppendLine($"RunAsUserId = {RunAsUserId} , ");
            sb.AppendLine($"StartDate = {StartDate} , ");
            sb.AppendLine($"EndDate = {EndDate} , ");
            sb.AppendLine($"StartTime = {StartTime} , ");
            sb.AppendLine($"Recurring = {Recurring} , ");
            sb.AppendLine($"RunMonday = {RunMonday} , ");
            sb.AppendLine($"RunTuesday = {RunTuesday} , ");
            sb.AppendLine($"RunWednesday = {RunWednesday} , ");
            sb.AppendLine($"RunThursday = {RunThursday} , ");
            sb.AppendLine($"RunFriday = {RunFriday} , ");
            sb.AppendLine($"RunSaturday = {RunSaturday} , ");
            sb.AppendLine($"RunSunday = {RunSunday} , ");
            sb.AppendLine($"Active = {Active} , ");
            sb.AppendLine($"IsMonthly = {IsMonthly} , ");
            sb.AppendLine($"RunMonthlyDay = {RunMonthlyDay} , ");
            sb.AppendLine($"RunMonthlyTime = {RunMonthlyTime} , ");
            sb.AppendLine($"IsHourly = {IsHourly} , ");
            sb.AppendLine($"IsDaily = {IsDaily} , ");
            sb.AppendLine($"RunDailyTime = {RunDailyTime} , ");
            sb.AppendLine($"IsWeekly = {IsWeekly} , ");
            sb.AppendLine($"RunHourlyMinute = {RunHourlyMinute} , ");
            sb.AppendLine($"IncludeDataSourceFile = {IncludeDataSourceFile} , ");
            sb.AppendLine($"RunOnWeekNumber = {RunOnWeekNumber} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public ScheduledJob New(ScheduledJob item)
        {
            if (item == null)
                return null;

            return new ScheduledJob
            {
            ScheduledJobId = item.ScheduledJobId,
            JobTemplateId = item.JobTemplateId,
            PredefinedJobTypeId = item.PredefinedJobTypeId,
            PredefinedJobOrReportTypeId = item.PredefinedJobOrReportTypeId,
            SubmittedByUserId = item.SubmittedByUserId,
            ExecutableFolder = item.ExecutableFolder,
            ExecutableName = item.ExecutableName,
            ExecutableParameter = item.ExecutableParameter,
            SerializedJobParameter = item.SerializedJobParameter,
            ReportTemplateFile = item.ReportTemplateFile,
            ExportDoc = item.ExportDoc,
            ExportXls = item.ExportXls,
            ExportPpt = item.ExportPpt,
            ExportCsv = item.ExportCsv,
            ExportXml = item.ExportXml,
            ExportPdf = item.ExportPdf,
            ExportCustom = item.ExportCustom,
            CustomExportName = item.CustomExportName,
            PickupDate = item.PickupDate,
            PickupTime = item.PickupTime,
            JobName = item.JobName,
            RunAsUserId = item.RunAsUserId,
            StartDate = item.StartDate,
            EndDate = item.EndDate,
            StartTime = item.StartTime,
            Recurring = item.Recurring,
            RunMonday = item.RunMonday,
            RunTuesday = item.RunTuesday,
            RunWednesday = item.RunWednesday,
            RunThursday = item.RunThursday,
            RunFriday = item.RunFriday,
            RunSaturday = item.RunSaturday,
            RunSunday = item.RunSunday,
            Active = item.Active,
            IsMonthly = item.IsMonthly,
            RunMonthlyDay = item.RunMonthlyDay,
            RunMonthlyTime = item.RunMonthlyTime,
            IsHourly = item.IsHourly,
            IsDaily = item.IsDaily,
            RunDailyTime = item.RunDailyTime,
            IsWeekly = item.IsWeekly,
            RunHourlyMinute = item.RunHourlyMinute,
            IncludeDataSourceFile = item.IncludeDataSourceFile,
            RunOnWeekNumber = item.RunOnWeekNumber,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public ScheduledJob NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("ScheduledJobHistory")]
    [Serializable]
    public partial class ScheduledJobHistory : IAuditableModel, INewObject<ScheduledJobHistory>
    {
        public const string TABLE_NAME = "ScheduledJobHistory";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_ScheduledJobHistoryId = "ScheduledJobHistoryId";
        public const string COLUMN_ScheduledJobId = "ScheduledJobId";
        public const string COLUMN_PickupDate = "PickupDate";
        public const string COLUMN_PickupMachineName = "PickupMachineName";
        public const string COLUMN_PickupProcessId = "PickupProcessId";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_PickupMachineName = 200;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_ScheduledJobHistoryId = false;
        public const bool COLUMN_NULLABLE_ScheduledJobId = false;
        public const bool COLUMN_NULLABLE_PickupDate = false;
        public const bool COLUMN_NULLABLE_PickupMachineName = false;
        public const bool COLUMN_NULLABLE_PickupProcessId = false;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("ScheduledJobHistoryId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long ScheduledJobHistoryId { get; set; }
        
        [Column("ScheduledJobId")]
        [NotNull]
        public long ScheduledJobId { get; set; }
        
        [Column("PickupDate")]
        [NotNull]
        public DateTime PickupDate { get; set; }
        
        [Column("PickupMachineName")]
        [NotNull]
        [StringLength(200)]
        public string PickupMachineName { get; set; }
        
        [Column("PickupProcessId")]
        [NotNull]
        public long PickupProcessId { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"ScheduledJobHistoryId = {ScheduledJobHistoryId} , ");
            sb.AppendLine($"ScheduledJobId = {ScheduledJobId} , ");
            sb.AppendLine($"PickupDate = {PickupDate} , ");
            sb.AppendLine($"PickupMachineName = {PickupMachineName} , ");
            sb.AppendLine($"PickupProcessId = {PickupProcessId} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public ScheduledJobHistory New(ScheduledJobHistory item)
        {
            if (item == null)
                return null;

            return new ScheduledJobHistory
            {
            ScheduledJobHistoryId = item.ScheduledJobHistoryId,
            ScheduledJobId = item.ScheduledJobId,
            PickupDate = item.PickupDate,
            PickupMachineName = item.PickupMachineName,
            PickupProcessId = item.PickupProcessId,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public ScheduledJobHistory NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("ScheduledJobNotificationEmail")]
    [Serializable]
    public partial class ScheduledJobNotificationEmail : IAuditableModel, INewObject<ScheduledJobNotificationEmail>
    {
        public const string TABLE_NAME = "ScheduledJobNotificationEmail";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_ScheduledJobNotificationEmailId = "ScheduledJobNotificationEmailId";
        public const string COLUMN_ScheduledJobId = "ScheduledJobId";
        public const string COLUMN_Email = "Email";
        public const string COLUMN_SendStatus = "SendStatus";
        public const string COLUMN_Message = "Message";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_Email = 200;
        public const int COLUMN_SIZE_Message = 200;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_ScheduledJobNotificationEmailId = false;
        public const bool COLUMN_NULLABLE_ScheduledJobId = false;
        public const bool COLUMN_NULLABLE_Email = false;
        public const bool COLUMN_NULLABLE_SendStatus = false;
        public const bool COLUMN_NULLABLE_Message = true;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("ScheduledJobNotificationEmailId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long ScheduledJobNotificationEmailId { get; set; }
        
        [Column("ScheduledJobId")]
        [NotNull]
        public long ScheduledJobId { get; set; }
        
        [Column("Email")]
        [NotNull]
        [StringLength(200)]
        public string Email { get; set; }
        
        [Column("SendStatus")]
        [NotNull]
        public long SendStatus { get; set; }
        
        [Column("Message")]
        [StringLength(200)]
        public string Message { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"ScheduledJobNotificationEmailId = {ScheduledJobNotificationEmailId} , ");
            sb.AppendLine($"ScheduledJobId = {ScheduledJobId} , ");
            sb.AppendLine($"Email = {Email} , ");
            sb.AppendLine($"SendStatus = {SendStatus} , ");
            sb.AppendLine($"Message = {Message} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public ScheduledJobNotificationEmail New(ScheduledJobNotificationEmail item)
        {
            if (item == null)
                return null;

            return new ScheduledJobNotificationEmail
            {
            ScheduledJobNotificationEmailId = item.ScheduledJobNotificationEmailId,
            ScheduledJobId = item.ScheduledJobId,
            Email = item.Email,
            SendStatus = item.SendStatus,
            Message = item.Message,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public ScheduledJobNotificationEmail NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("SingleSignOnServer")]
    [Serializable]
    public partial class SingleSignOnServer : IAuditableModel, INewObject<SingleSignOnServer>
    {
        public const string TABLE_NAME = "SingleSignOnServer";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_SingleSignOnServerId = "SingleSignOnServerId";
        public const string COLUMN_SingleSignOnServerName = "SingleSignOnServerName";
        public const string COLUMN_SingleSignOnServerCode = "SingleSignOnServerCode";
        public const string COLUMN_PredefinedSingleSignOnServerTypeId = "PredefinedSingleSignOnServerTypeId";
        public const string COLUMN_Host = "Host";
        public const string COLUMN_ClientId = "ClientId";
        public const string COLUMN_ClientTenantId = "ClientTenantId";
        public const string COLUMN_ClientSecret = "ClientSecret";
        public const string COLUMN_ForAllCompany = "ForAllCompany";
        public const string COLUMN_IsDefault = "IsDefault";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_SingleSignOnServerName = 100;
        public const int COLUMN_SIZE_SingleSignOnServerCode = 45;
        public const int COLUMN_SIZE_Host = 500;
        public const int COLUMN_SIZE_ClientId = 100;
        public const int COLUMN_SIZE_ClientTenantId = 100;
        public const int COLUMN_SIZE_ClientSecret = 200;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_SingleSignOnServerId = false;
        public const bool COLUMN_NULLABLE_SingleSignOnServerName = false;
        public const bool COLUMN_NULLABLE_SingleSignOnServerCode = false;
        public const bool COLUMN_NULLABLE_PredefinedSingleSignOnServerTypeId = false;
        public const bool COLUMN_NULLABLE_Host = true;
        public const bool COLUMN_NULLABLE_ClientId = true;
        public const bool COLUMN_NULLABLE_ClientTenantId = true;
        public const bool COLUMN_NULLABLE_ClientSecret = true;
        public const bool COLUMN_NULLABLE_ForAllCompany = false;
        public const bool COLUMN_NULLABLE_IsDefault = false;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("SingleSignOnServerId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long SingleSignOnServerId { get; set; }
        
        [Column("SingleSignOnServerName")]
        [NotNull]
        [StringLength(100)]
        public string SingleSignOnServerName { get; set; }
        
        [Column("SingleSignOnServerCode")]
        [NotNull]
        [StringLength(45)]
        public string SingleSignOnServerCode { get; set; }
        
        [Column("PredefinedSingleSignOnServerTypeId")]
        [NotNull]
        public long PredefinedSingleSignOnServerTypeId { get; set; }
        
        [Column("Host")]
        [StringLength(500)]
        public string Host { get; set; }
        
        [Column("ClientId")]
        [StringLength(100)]
        public string ClientId { get; set; }
        
        [Column("ClientTenantId")]
        [StringLength(100)]
        public string ClientTenantId { get; set; }
        
        [Column("ClientSecret")]
        [StringLength(200)]
        public string ClientSecret { get; set; }
        
        [Column("ForAllCompany")]
        [NotNull]
        public bool ForAllCompany { get; set; }
        
        [Column("IsDefault")]
        [NotNull]
        public bool IsDefault { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"SingleSignOnServerId = {SingleSignOnServerId} , ");
            sb.AppendLine($"SingleSignOnServerName = {SingleSignOnServerName} , ");
            sb.AppendLine($"SingleSignOnServerCode = {SingleSignOnServerCode} , ");
            sb.AppendLine($"PredefinedSingleSignOnServerTypeId = {PredefinedSingleSignOnServerTypeId} , ");
            sb.AppendLine($"Host = {Host} , ");
            sb.AppendLine($"ClientId = {ClientId} , ");
            sb.AppendLine($"ClientTenantId = {ClientTenantId} , ");
            sb.AppendLine($"ClientSecret = {ClientSecret} , ");
            sb.AppendLine($"ForAllCompany = {ForAllCompany} , ");
            sb.AppendLine($"IsDefault = {IsDefault} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public SingleSignOnServer New(SingleSignOnServer item)
        {
            if (item == null)
                return null;

            return new SingleSignOnServer
            {
            SingleSignOnServerId = item.SingleSignOnServerId,
            SingleSignOnServerName = item.SingleSignOnServerName,
            SingleSignOnServerCode = item.SingleSignOnServerCode,
            PredefinedSingleSignOnServerTypeId = item.PredefinedSingleSignOnServerTypeId,
            Host = item.Host,
            ClientId = item.ClientId,
            ClientTenantId = item.ClientTenantId,
            ClientSecret = item.ClientSecret,
            ForAllCompany = item.ForAllCompany,
            IsDefault = item.IsDefault,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public SingleSignOnServer NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("SmsServer")]
    [Serializable]
    public partial class SmsServer : IAuditableModel, INewObject<SmsServer>
    {
        public const string TABLE_NAME = "SmsServer";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_SmsServerId = "SmsServerId";
        public const string COLUMN_SmsServerCode = "SmsServerCode";
        public const string COLUMN_SmsServerName = "SmsServerName";
        public const string COLUMN_Host = "Host";
        public const string COLUMN_Port = "Port";
        public const string COLUMN_SenderName = "SenderName";
        public const string COLUMN_SenderEmail = "SenderEmail";
        public const string COLUMN_UserName = "UserName";
        public const string COLUMN_Password = "Password";
        public const string COLUMN_IsSecure = "IsSecure";
        public const string COLUMN_IsDefault = "IsDefault";
        public const string COLUMN_PredefinedSmsAndVoiceServerTypeId = "PredefinedSmsAndVoiceServerTypeId";
        public const string COLUMN_TwilioAccountSid = "TwilioAccountSid";
        public const string COLUMN_TwilioAccountAuthToken = "TwilioAccountAuthToken";
        public const string COLUMN_TwilioAccountPhoneNumber = "TwilioAccountPhoneNumber";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_SmsServerCode = 45;
        public const int COLUMN_SIZE_SmsServerName = 100;
        public const int COLUMN_SIZE_Host = 150;
        public const int COLUMN_SIZE_SenderName = 100;
        public const int COLUMN_SIZE_SenderEmail = 150;
        public const int COLUMN_SIZE_UserName = 100;
        public const int COLUMN_SIZE_Password = 200;
        public const int COLUMN_SIZE_TwilioAccountSid = 255;
        public const int COLUMN_SIZE_TwilioAccountAuthToken = 500;
        public const int COLUMN_SIZE_TwilioAccountPhoneNumber = 255;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_SmsServerId = false;
        public const bool COLUMN_NULLABLE_SmsServerCode = false;
        public const bool COLUMN_NULLABLE_SmsServerName = false;
        public const bool COLUMN_NULLABLE_Host = false;
        public const bool COLUMN_NULLABLE_Port = false;
        public const bool COLUMN_NULLABLE_SenderName = false;
        public const bool COLUMN_NULLABLE_SenderEmail = false;
        public const bool COLUMN_NULLABLE_UserName = false;
        public const bool COLUMN_NULLABLE_Password = true;
        public const bool COLUMN_NULLABLE_IsSecure = true;
        public const bool COLUMN_NULLABLE_IsDefault = false;
        public const bool COLUMN_NULLABLE_PredefinedSmsAndVoiceServerTypeId = false;
        public const bool COLUMN_NULLABLE_TwilioAccountSid = true;
        public const bool COLUMN_NULLABLE_TwilioAccountAuthToken = true;
        public const bool COLUMN_NULLABLE_TwilioAccountPhoneNumber = true;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("SmsServerId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long SmsServerId { get; set; }
        
        [Column("SmsServerCode")]
        [NotNull]
        [StringLength(45)]
        public string SmsServerCode { get; set; }
        
        [Column("SmsServerName")]
        [NotNull]
        [StringLength(100)]
        public string SmsServerName { get; set; }
        
        [Column("Host")]
        [NotNull]
        [StringLength(150)]
        public string Host { get; set; }
        
        [Column("Port")]
        [NotNull]
        public long Port { get; set; }
        
        [Column("SenderName")]
        [NotNull]
        [StringLength(100)]
        public string SenderName { get; set; }
        
        [Column("SenderEmail")]
        [NotNull]
        [StringLength(150)]
        public string SenderEmail { get; set; }
        
        [Column("UserName")]
        [NotNull]
        [StringLength(100)]
        public string UserName { get; set; }
        
        [Column("Password")]
        [StringLength(200)]
        public string Password { get; set; }
        
        [Column("IsSecure")]
        public bool? IsSecure { get; set; }
        
        [Column("IsDefault")]
        [NotNull]
        public bool IsDefault { get; set; }
        
        [Column("PredefinedSmsAndVoiceServerTypeId")]
        [NotNull]
        public long PredefinedSmsAndVoiceServerTypeId { get; set; }
        
        [Column("TwilioAccountSid")]
        [StringLength(255)]
        public string TwilioAccountSid { get; set; }
        
        [Column("TwilioAccountAuthToken")]
        [StringLength(500)]
        public string TwilioAccountAuthToken { get; set; }
        
        [Column("TwilioAccountPhoneNumber")]
        [StringLength(255)]
        public string TwilioAccountPhoneNumber { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"SmsServerId = {SmsServerId} , ");
            sb.AppendLine($"SmsServerCode = {SmsServerCode} , ");
            sb.AppendLine($"SmsServerName = {SmsServerName} , ");
            sb.AppendLine($"Host = {Host} , ");
            sb.AppendLine($"Port = {Port} , ");
            sb.AppendLine($"SenderName = {SenderName} , ");
            sb.AppendLine($"SenderEmail = {SenderEmail} , ");
            sb.AppendLine($"UserName = {UserName} , ");
            sb.AppendLine($"Password = {Password} , ");
            sb.AppendLine($"IsSecure = {IsSecure} , ");
            sb.AppendLine($"IsDefault = {IsDefault} , ");
            sb.AppendLine($"PredefinedSmsAndVoiceServerTypeId = {PredefinedSmsAndVoiceServerTypeId} , ");
            sb.AppendLine($"TwilioAccountSid = {TwilioAccountSid} , ");
            sb.AppendLine($"TwilioAccountAuthToken = {TwilioAccountAuthToken} , ");
            sb.AppendLine($"TwilioAccountPhoneNumber = {TwilioAccountPhoneNumber} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public SmsServer New(SmsServer item)
        {
            if (item == null)
                return null;

            return new SmsServer
            {
            SmsServerId = item.SmsServerId,
            SmsServerCode = item.SmsServerCode,
            SmsServerName = item.SmsServerName,
            Host = item.Host,
            Port = item.Port,
            SenderName = item.SenderName,
            SenderEmail = item.SenderEmail,
            UserName = item.UserName,
            Password = item.Password,
            IsSecure = item.IsSecure,
            IsDefault = item.IsDefault,
            PredefinedSmsAndVoiceServerTypeId = item.PredefinedSmsAndVoiceServerTypeId,
            TwilioAccountSid = item.TwilioAccountSid,
            TwilioAccountAuthToken = item.TwilioAccountAuthToken,
            TwilioAccountPhoneNumber = item.TwilioAccountPhoneNumber,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public SmsServer NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("State")]
    [Serializable]
    public partial class State : IAuditableModel, INewObject<State>
    {
        public const string TABLE_NAME = "State";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_StateId = "StateId";
        public const string COLUMN_StateName = "StateName";
        public const string COLUMN_StateCode = "StateCode";
        public const string COLUMN_CountryId = "CountryId";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_StateName = 255;
        public const int COLUMN_SIZE_StateCode = 255;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_StateId = false;
        public const bool COLUMN_NULLABLE_StateName = false;
        public const bool COLUMN_NULLABLE_StateCode = false;
        public const bool COLUMN_NULLABLE_CountryId = false;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("StateId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long StateId { get; set; }
        
        [Column("StateName")]
        [NotNull]
        [StringLength(255)]
        public string StateName { get; set; }
        
        [Column("StateCode")]
        [NotNull]
        [StringLength(255)]
        public string StateCode { get; set; }
        
        [Column("CountryId")]
        [NotNull]
        public long CountryId { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"StateId = {StateId} , ");
            sb.AppendLine($"StateName = {StateName} , ");
            sb.AppendLine($"StateCode = {StateCode} , ");
            sb.AppendLine($"CountryId = {CountryId} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public State New(State item)
        {
            if (item == null)
                return null;

            return new State
            {
            StateId = item.StateId,
            StateName = item.StateName,
            StateCode = item.StateCode,
            CountryId = item.CountryId,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public State NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("User")]
    [Serializable]
    public partial class User : IAuditableModel, INewObject<User>
    {
        public const string TABLE_NAME = "User";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_UserId = "UserId";
        public const string COLUMN_UserGuid = "UserGuid";
        public const string COLUMN_UserName = "UserName";
        public const string COLUMN_PredefinedUserTypeId = "PredefinedUserTypeId";
        public const string COLUMN_PasswordHash = "PasswordHash";
        public const string COLUMN_PasswordChangedDate = "PasswordChangedDate";
        public const string COLUMN_Email = "Email";
        public const string COLUMN_FirstName = "FirstName";
        public const string COLUMN_LastName = "LastName";
        public const string COLUMN_WorkPhone = "WorkPhone";
        public const string COLUMN_WorkPhoneExt = "WorkPhoneExt";
        public const string COLUMN_CellPhone = "CellPhone";
        public const string COLUMN_Active = "Active";
        public const string COLUMN_NotificationThroughEmail = "NotificationThroughEmail";
        public const string COLUMN_OneTimeToken = "OneTimeToken";
        public const string COLUMN_OneTimeExpireDateTime = "OneTimeExpireDateTime";
        public const string COLUMN_LoginFailedAttempt = "LoginFailedAttempt";
        public const string COLUMN_IsAccountLocked = "IsAccountLocked";
        public const string COLUMN_AccountLockedUntil = "AccountLockedUntil";
        public const string COLUMN_NewUserActivationGuid = "NewUserActivationGuid";
        public const string COLUMN_NewUserActivationCompletedOn = "NewUserActivationCompletedOn";
        public const string COLUMN_ProfileImageGuid = "ProfileImageGuid";
        public const string COLUMN_ProfileImageLocation = "ProfileImageLocation";
        public const string COLUMN_ProfileImageActualFileName = "ProfileImageActualFileName";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_UserName = 50;
        public const int COLUMN_SIZE_PasswordHash = 1000;
        public const int COLUMN_SIZE_Email = 100;
        public const int COLUMN_SIZE_FirstName = 100;
        public const int COLUMN_SIZE_LastName = 100;
        public const int COLUMN_SIZE_WorkPhone = 20;
        public const int COLUMN_SIZE_WorkPhoneExt = 10;
        public const int COLUMN_SIZE_CellPhone = 20;
        public const int COLUMN_SIZE_ProfileImageLocation = 1024;
        public const int COLUMN_SIZE_ProfileImageActualFileName = 125;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_UserId = false;
        public const bool COLUMN_NULLABLE_UserGuid = false;
        public const bool COLUMN_NULLABLE_UserName = false;
        public const bool COLUMN_NULLABLE_PredefinedUserTypeId = false;
        public const bool COLUMN_NULLABLE_PasswordHash = true;
        public const bool COLUMN_NULLABLE_PasswordChangedDate = true;
        public const bool COLUMN_NULLABLE_Email = false;
        public const bool COLUMN_NULLABLE_FirstName = false;
        public const bool COLUMN_NULLABLE_LastName = true;
        public const bool COLUMN_NULLABLE_WorkPhone = true;
        public const bool COLUMN_NULLABLE_WorkPhoneExt = true;
        public const bool COLUMN_NULLABLE_CellPhone = true;
        public const bool COLUMN_NULLABLE_Active = false;
        public const bool COLUMN_NULLABLE_NotificationThroughEmail = true;
        public const bool COLUMN_NULLABLE_OneTimeToken = true;
        public const bool COLUMN_NULLABLE_OneTimeExpireDateTime = true;
        public const bool COLUMN_NULLABLE_LoginFailedAttempt = true;
        public const bool COLUMN_NULLABLE_IsAccountLocked = true;
        public const bool COLUMN_NULLABLE_AccountLockedUntil = true;
        public const bool COLUMN_NULLABLE_NewUserActivationGuid = true;
        public const bool COLUMN_NULLABLE_NewUserActivationCompletedOn = true;
        public const bool COLUMN_NULLABLE_ProfileImageGuid = true;
        public const bool COLUMN_NULLABLE_ProfileImageLocation = true;
        public const bool COLUMN_NULLABLE_ProfileImageActualFileName = true;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("UserId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long UserId { get; set; }
        
        [Column("UserGuid")]
        [NotNull]
        public System.Guid UserGuid { get; set; }
        
        [Column("UserName")]
        [NotNull]
        [StringLength(50)]
        public string UserName { get; set; }
        
        [Column("PredefinedUserTypeId")]
        [NotNull]
        public long PredefinedUserTypeId { get; set; }
        
        [JsonIgnore]
        [Column("PasswordHash")]
        [StringLength(1000)]
        public string PasswordHash { get; set; }
        
        [JsonIgnore]
        [Column("PasswordChangedDate")]
        public DateTime? PasswordChangedDate { get; set; }
        
        [Column("Email")]
        [NotNull]
        [StringLength(100)]
        public string Email { get; set; }
        
        [Column("FirstName")]
        [NotNull]
        [StringLength(100)]
        public string FirstName { get; set; }
        
        [Column("LastName")]
        [StringLength(100)]
        public string LastName { get; set; }
        
        [Column("WorkPhone")]
        [StringLength(20)]
        public string WorkPhone { get; set; }
        
        [Column("WorkPhoneExt")]
        [StringLength(10)]
        public string WorkPhoneExt { get; set; }
        
        [Column("CellPhone")]
        [StringLength(20)]
        public string CellPhone { get; set; }
        
        [Column("Active")]
        [NotNull]
        public bool Active { get; set; }
        
        [Column("NotificationThroughEmail")]
        public bool? NotificationThroughEmail { get; set; }
        
        [JsonIgnore]
        [Column("OneTimeToken")]
        public System.Guid? OneTimeToken { get; set; }
        
        [JsonIgnore]
        [Column("OneTimeExpireDateTime")]
        public DateTime? OneTimeExpireDateTime { get; set; }
        
        [JsonIgnore]
        [Column("LoginFailedAttempt")]
        public long? LoginFailedAttempt { get; set; }
        
        [Column("IsAccountLocked")]
        public bool? IsAccountLocked { get; set; }
        
        [Column("AccountLockedUntil")]
        public DateTime? AccountLockedUntil { get; set; }
        
        [JsonIgnore]
        [Column("NewUserActivationGuid")]
        public System.Guid? NewUserActivationGuid { get; set; }
        
        [JsonIgnore]
        [Column("NewUserActivationCompletedOn")]
        public DateTime? NewUserActivationCompletedOn { get; set; }
        
        [Column("ProfileImageGuid")]
        public System.Guid? ProfileImageGuid { get; set; }
        
        [JsonIgnore]
        [Column("ProfileImageLocation")]
        [StringLength(1024)]
        public string ProfileImageLocation { get; set; }
        
        [Column("ProfileImageActualFileName")]
        [StringLength(125)]
        public string ProfileImageActualFileName { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"UserId = {UserId} , ");
            sb.AppendLine($"UserGuid = {UserGuid} , ");
            sb.AppendLine($"UserName = {UserName} , ");
            sb.AppendLine($"PredefinedUserTypeId = {PredefinedUserTypeId} , ");
            sb.AppendLine($"PasswordHash = {PasswordHash} , ");
            sb.AppendLine($"PasswordChangedDate = {PasswordChangedDate} , ");
            sb.AppendLine($"Email = {Email} , ");
            sb.AppendLine($"FirstName = {FirstName} , ");
            sb.AppendLine($"LastName = {LastName} , ");
            sb.AppendLine($"WorkPhone = {WorkPhone} , ");
            sb.AppendLine($"WorkPhoneExt = {WorkPhoneExt} , ");
            sb.AppendLine($"CellPhone = {CellPhone} , ");
            sb.AppendLine($"Active = {Active} , ");
            sb.AppendLine($"NotificationThroughEmail = {NotificationThroughEmail} , ");
            sb.AppendLine($"OneTimeToken = {OneTimeToken} , ");
            sb.AppendLine($"OneTimeExpireDateTime = {OneTimeExpireDateTime} , ");
            sb.AppendLine($"LoginFailedAttempt = {LoginFailedAttempt} , ");
            sb.AppendLine($"IsAccountLocked = {IsAccountLocked} , ");
            sb.AppendLine($"AccountLockedUntil = {AccountLockedUntil} , ");
            sb.AppendLine($"NewUserActivationGuid = {NewUserActivationGuid} , ");
            sb.AppendLine($"NewUserActivationCompletedOn = {NewUserActivationCompletedOn} , ");
            sb.AppendLine($"ProfileImageGuid = {ProfileImageGuid} , ");
            sb.AppendLine($"ProfileImageLocation = {ProfileImageLocation} , ");
            sb.AppendLine($"ProfileImageActualFileName = {ProfileImageActualFileName} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public User New(User item)
        {
            if (item == null)
                return null;

            return new User
            {
            UserId = item.UserId,
            UserGuid = item.UserGuid,
            UserName = item.UserName,
            PredefinedUserTypeId = item.PredefinedUserTypeId,
            PasswordHash = item.PasswordHash,
            PasswordChangedDate = item.PasswordChangedDate,
            Email = item.Email,
            FirstName = item.FirstName,
            LastName = item.LastName,
            WorkPhone = item.WorkPhone,
            WorkPhoneExt = item.WorkPhoneExt,
            CellPhone = item.CellPhone,
            Active = item.Active,
            NotificationThroughEmail = item.NotificationThroughEmail,
            OneTimeToken = item.OneTimeToken,
            OneTimeExpireDateTime = item.OneTimeExpireDateTime,
            LoginFailedAttempt = item.LoginFailedAttempt,
            IsAccountLocked = item.IsAccountLocked,
            AccountLockedUntil = item.AccountLockedUntil,
            NewUserActivationGuid = item.NewUserActivationGuid,
            NewUserActivationCompletedOn = item.NewUserActivationCompletedOn,
            ProfileImageGuid = item.ProfileImageGuid,
            ProfileImageLocation = item.ProfileImageLocation,
            ProfileImageActualFileName = item.ProfileImageActualFileName,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public User NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("UserGroup")]
    [Serializable]
    public partial class UserGroup : IAuditableModel, INewObject<UserGroup>
    {
        public const string TABLE_NAME = "UserGroup";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_UserGroupId = "UserGroupId";
        public const string COLUMN_UserGroupName = "UserGroupName";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_UserGroupName = 100;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_UserGroupId = false;
        public const bool COLUMN_NULLABLE_UserGroupName = false;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("UserGroupId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long UserGroupId { get; set; }
        
        [Column("UserGroupName")]
        [NotNull]
        [StringLength(100)]
        public string UserGroupName { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"UserGroupId = {UserGroupId} , ");
            sb.AppendLine($"UserGroupName = {UserGroupName} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public UserGroup New(UserGroup item)
        {
            if (item == null)
                return null;

            return new UserGroup
            {
            UserGroupId = item.UserGroupId,
            UserGroupName = item.UserGroupName,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public UserGroup NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("UserGroupUser")]
    [Serializable]
    public partial class UserGroupUser : IAuditableModel, INewObject<UserGroupUser>
    {
        public const string TABLE_NAME = "UserGroupUser";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_UserGroupUserId = "UserGroupUserId";
        public const string COLUMN_UserGroupId = "UserGroupId";
        public const string COLUMN_UserId = "UserId";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_UserGroupUserId = false;
        public const bool COLUMN_NULLABLE_UserGroupId = false;
        public const bool COLUMN_NULLABLE_UserId = false;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("UserGroupUserId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long UserGroupUserId { get; set; }
        
        [Column("UserGroupId")]
        [NotNull]
        public long UserGroupId { get; set; }
        
        [Column("UserId")]
        [NotNull]
        public long UserId { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"UserGroupUserId = {UserGroupUserId} , ");
            sb.AppendLine($"UserGroupId = {UserGroupId} , ");
            sb.AppendLine($"UserId = {UserId} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public UserGroupUser New(UserGroupUser item)
        {
            if (item == null)
                return null;

            return new UserGroupUser
            {
            UserGroupUserId = item.UserGroupUserId,
            UserGroupId = item.UserGroupId,
            UserId = item.UserId,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public UserGroupUser NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("UserPermission")]
    [Serializable]
    public partial class UserPermission : IAuditableModel, INewObject<UserPermission>
    {
        public const string TABLE_NAME = "UserPermission";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_UserPermissionId = "UserPermissionId";
        public const string COLUMN_UserId = "UserId";
        public const string COLUMN_PredefinedUserPermissionTypeId = "PredefinedUserPermissionTypeId";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_UserPermissionId = false;
        public const bool COLUMN_NULLABLE_UserId = false;
        public const bool COLUMN_NULLABLE_PredefinedUserPermissionTypeId = false;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("UserPermissionId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long UserPermissionId { get; set; }
        
        [Column("UserId")]
        [NotNull]
        public long UserId { get; set; }
        
        [Column("PredefinedUserPermissionTypeId")]
        [NotNull]
        public long PredefinedUserPermissionTypeId { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"UserPermissionId = {UserPermissionId} , ");
            sb.AppendLine($"UserId = {UserId} , ");
            sb.AppendLine($"PredefinedUserPermissionTypeId = {PredefinedUserPermissionTypeId} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public UserPermission New(UserPermission item)
        {
            if (item == null)
                return null;

            return new UserPermission
            {
            UserPermissionId = item.UserPermissionId,
            UserId = item.UserId,
            PredefinedUserPermissionTypeId = item.PredefinedUserPermissionTypeId,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public UserPermission NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("VisionBoardPicture")]
    [Serializable]
    public partial class VisionBoardPicture : IAuditableModel, INewObject<VisionBoardPicture>
    {
        public const string TABLE_NAME = "VisionBoardPicture";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_VisionBoardPictureId = "VisionBoardPictureId";
        public const string COLUMN_Title = "Title";
        public const string COLUMN_Location = "Location";
        public const string COLUMN_FileName = "FileName";
        public const string COLUMN_FileSize = "FileSize";
        public const string COLUMN_ThumbnailLocation = "ThumbnailLocation";
        public const string COLUMN_ThumbnailFileName = "ThumbnailFileName";
        public const string COLUMN_ThumbnailFileSize = "ThumbnailFileSize";
        public const string COLUMN_UserId = "UserId";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_Title = 100;
        public const int COLUMN_SIZE_Location = 1024;
        public const int COLUMN_SIZE_FileName = 255;
        public const int COLUMN_SIZE_ThumbnailLocation = 1024;
        public const int COLUMN_SIZE_ThumbnailFileName = 255;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_VisionBoardPictureId = false;
        public const bool COLUMN_NULLABLE_Title = true;
        public const bool COLUMN_NULLABLE_Location = true;
        public const bool COLUMN_NULLABLE_FileName = false;
        public const bool COLUMN_NULLABLE_FileSize = false;
        public const bool COLUMN_NULLABLE_ThumbnailLocation = true;
        public const bool COLUMN_NULLABLE_ThumbnailFileName = true;
        public const bool COLUMN_NULLABLE_ThumbnailFileSize = true;
        public const bool COLUMN_NULLABLE_UserId = false;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("VisionBoardPictureId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long VisionBoardPictureId { get; set; }
        
        [Column("Title")]
        [StringLength(100)]
        public string Title { get; set; }
        
        [Column("Location")]
        [StringLength(1024)]
        public string Location { get; set; }
        
        [Column("FileName")]
        [NotNull]
        [StringLength(255)]
        public string FileName { get; set; }
        
        [Column("FileSize")]
        [NotNull]
        public long FileSize { get; set; }
        
        [Column("ThumbnailLocation")]
        [StringLength(1024)]
        public string ThumbnailLocation { get; set; }
        
        [Column("ThumbnailFileName")]
        [StringLength(255)]
        public string ThumbnailFileName { get; set; }
        
        [Column("ThumbnailFileSize")]
        public long? ThumbnailFileSize { get; set; }
        
        [Column("UserId")]
        [NotNull]
        public long UserId { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"VisionBoardPictureId = {VisionBoardPictureId} , ");
            sb.AppendLine($"Title = {Title} , ");
            sb.AppendLine($"Location = {Location} , ");
            sb.AppendLine($"FileName = {FileName} , ");
            sb.AppendLine($"FileSize = {FileSize} , ");
            sb.AppendLine($"ThumbnailLocation = {ThumbnailLocation} , ");
            sb.AppendLine($"ThumbnailFileName = {ThumbnailFileName} , ");
            sb.AppendLine($"ThumbnailFileSize = {ThumbnailFileSize} , ");
            sb.AppendLine($"UserId = {UserId} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public VisionBoardPicture New(VisionBoardPicture item)
        {
            if (item == null)
                return null;

            return new VisionBoardPicture
            {
            VisionBoardPictureId = item.VisionBoardPictureId,
            Title = item.Title,
            Location = item.Location,
            FileName = item.FileName,
            FileSize = item.FileSize,
            ThumbnailLocation = item.ThumbnailLocation,
            ThumbnailFileName = item.ThumbnailFileName,
            ThumbnailFileSize = item.ThumbnailFileSize,
            UserId = item.UserId,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public VisionBoardPicture NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("VisionBoardSession")]
    [Serializable]
    public partial class VisionBoardSession : IAuditableModel, INewObject<VisionBoardSession>
    {
        public const string TABLE_NAME = "VisionBoardSession";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_VisionBoardSessionId = "VisionBoardSessionId";
        public const string COLUMN_VisionBoardSessionDate = "VisionBoardSessionDate";
        public const string COLUMN_VisionBoardSessionTimeSpentInSeconds = "VisionBoardSessionTimeSpentInSeconds";
        public const string COLUMN_UserId = "UserId";
        public const string COLUMN_AddedToFeed = "AddedToFeed";
        public const string COLUMN_AddedToFeedDate = "AddedToFeedDate";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_VisionBoardSessionId = false;
        public const bool COLUMN_NULLABLE_VisionBoardSessionDate = false;
        public const bool COLUMN_NULLABLE_VisionBoardSessionTimeSpentInSeconds = true;
        public const bool COLUMN_NULLABLE_UserId = false;
        public const bool COLUMN_NULLABLE_AddedToFeed = true;
        public const bool COLUMN_NULLABLE_AddedToFeedDate = true;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("VisionBoardSessionId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long VisionBoardSessionId { get; set; }
        
        [Column("VisionBoardSessionDate")]
        [NotNull]
        public DateTime VisionBoardSessionDate { get; set; }
        
        [Column("VisionBoardSessionTimeSpentInSeconds")]
        public long? VisionBoardSessionTimeSpentInSeconds { get; set; }
        
        [Column("UserId")]
        [NotNull]
        public long UserId { get; set; }
        
        [Column("AddedToFeed")]
        public bool? AddedToFeed { get; set; }
        
        [Column("AddedToFeedDate")]
        public DateTime? AddedToFeedDate { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"VisionBoardSessionId = {VisionBoardSessionId} , ");
            sb.AppendLine($"VisionBoardSessionDate = {VisionBoardSessionDate} , ");
            sb.AppendLine($"VisionBoardSessionTimeSpentInSeconds = {VisionBoardSessionTimeSpentInSeconds} , ");
            sb.AppendLine($"UserId = {UserId} , ");
            sb.AppendLine($"AddedToFeed = {AddedToFeed} , ");
            sb.AppendLine($"AddedToFeedDate = {AddedToFeedDate} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public VisionBoardSession New(VisionBoardSession item)
        {
            if (item == null)
                return null;

            return new VisionBoardSession
            {
            VisionBoardSessionId = item.VisionBoardSessionId,
            VisionBoardSessionDate = item.VisionBoardSessionDate,
            VisionBoardSessionTimeSpentInSeconds = item.VisionBoardSessionTimeSpentInSeconds,
            UserId = item.UserId,
            AddedToFeed = item.AddedToFeed,
            AddedToFeedDate = item.AddedToFeedDate,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public VisionBoardSession NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("VisionBoardSessionPicture")]
    [Serializable]
    public partial class VisionBoardSessionPicture : IAuditableModel, INewObject<VisionBoardSessionPicture>
    {
        public const string TABLE_NAME = "VisionBoardSessionPicture";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_VisionBoardSessionPictureId = "VisionBoardSessionPictureId";
        public const string COLUMN_VisionBoardSessionId = "VisionBoardSessionId";
        public const string COLUMN_VisionBoardSessionPictureDate = "VisionBoardSessionPictureDate";
        public const string COLUMN_VisionBoardPictureId = "VisionBoardPictureId";
        public const string COLUMN_VisionBoardSessionPictureTimeSpentInSeconds = "VisionBoardSessionPictureTimeSpentInSeconds";
        public const string COLUMN_UserId = "UserId";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_VisionBoardSessionPictureId = false;
        public const bool COLUMN_NULLABLE_VisionBoardSessionId = false;
        public const bool COLUMN_NULLABLE_VisionBoardSessionPictureDate = false;
        public const bool COLUMN_NULLABLE_VisionBoardPictureId = false;
        public const bool COLUMN_NULLABLE_VisionBoardSessionPictureTimeSpentInSeconds = true;
        public const bool COLUMN_NULLABLE_UserId = false;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("VisionBoardSessionPictureId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long VisionBoardSessionPictureId { get; set; }
        
        [Column("VisionBoardSessionId")]
        [NotNull]
        public long VisionBoardSessionId { get; set; }
        
        [Column("VisionBoardSessionPictureDate")]
        [NotNull]
        public DateTime VisionBoardSessionPictureDate { get; set; }
        
        [Column("VisionBoardPictureId")]
        [NotNull]
        public long VisionBoardPictureId { get; set; }
        
        [Column("VisionBoardSessionPictureTimeSpentInSeconds")]
        public long? VisionBoardSessionPictureTimeSpentInSeconds { get; set; }
        
        [Column("UserId")]
        [NotNull]
        public long UserId { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"VisionBoardSessionPictureId = {VisionBoardSessionPictureId} , ");
            sb.AppendLine($"VisionBoardSessionId = {VisionBoardSessionId} , ");
            sb.AppendLine($"VisionBoardSessionPictureDate = {VisionBoardSessionPictureDate} , ");
            sb.AppendLine($"VisionBoardPictureId = {VisionBoardPictureId} , ");
            sb.AppendLine($"VisionBoardSessionPictureTimeSpentInSeconds = {VisionBoardSessionPictureTimeSpentInSeconds} , ");
            sb.AppendLine($"UserId = {UserId} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public VisionBoardSessionPicture New(VisionBoardSessionPicture item)
        {
            if (item == null)
                return null;

            return new VisionBoardSessionPicture
            {
            VisionBoardSessionPictureId = item.VisionBoardSessionPictureId,
            VisionBoardSessionId = item.VisionBoardSessionId,
            VisionBoardSessionPictureDate = item.VisionBoardSessionPictureDate,
            VisionBoardPictureId = item.VisionBoardPictureId,
            VisionBoardSessionPictureTimeSpentInSeconds = item.VisionBoardSessionPictureTimeSpentInSeconds,
            UserId = item.UserId,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public VisionBoardSessionPicture NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("VoiceNotificationServer")]
    [Serializable]
    public partial class VoiceNotificationServer : IAuditableModel, INewObject<VoiceNotificationServer>
    {
        public const string TABLE_NAME = "VoiceNotificationServer";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_VoiceNotificationServerId = "VoiceNotificationServerId";
        public const string COLUMN_VoiceNotificationServerCode = "VoiceNotificationServerCode";
        public const string COLUMN_VoiceNotificationServerName = "VoiceNotificationServerName";
        public const string COLUMN_PredefinedSmsAndVoiceServerTypeId = "PredefinedSmsAndVoiceServerTypeId";
        public const string COLUMN_TwilioAccountSid = "TwilioAccountSid";
        public const string COLUMN_TwilioAccountAuthToken = "TwilioAccountAuthToken";
        public const string COLUMN_TwilioAccountPhoneNumber = "TwilioAccountPhoneNumber";
        public const string COLUMN_IsEnabled = "IsEnabled";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_VoiceNotificationServerCode = 45;
        public const int COLUMN_SIZE_VoiceNotificationServerName = 100;
        public const int COLUMN_SIZE_TwilioAccountSid = 255;
        public const int COLUMN_SIZE_TwilioAccountAuthToken = 500;
        public const int COLUMN_SIZE_TwilioAccountPhoneNumber = 255;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_VoiceNotificationServerId = false;
        public const bool COLUMN_NULLABLE_VoiceNotificationServerCode = false;
        public const bool COLUMN_NULLABLE_VoiceNotificationServerName = false;
        public const bool COLUMN_NULLABLE_PredefinedSmsAndVoiceServerTypeId = false;
        public const bool COLUMN_NULLABLE_TwilioAccountSid = true;
        public const bool COLUMN_NULLABLE_TwilioAccountAuthToken = true;
        public const bool COLUMN_NULLABLE_TwilioAccountPhoneNumber = true;
        public const bool COLUMN_NULLABLE_IsEnabled = false;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("VoiceNotificationServerId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long VoiceNotificationServerId { get; set; }
        
        [Column("VoiceNotificationServerCode")]
        [NotNull]
        [StringLength(45)]
        public string VoiceNotificationServerCode { get; set; }
        
        [Column("VoiceNotificationServerName")]
        [NotNull]
        [StringLength(100)]
        public string VoiceNotificationServerName { get; set; }
        
        [Column("PredefinedSmsAndVoiceServerTypeId")]
        [NotNull]
        public long PredefinedSmsAndVoiceServerTypeId { get; set; }
        
        [Column("TwilioAccountSid")]
        [StringLength(255)]
        public string TwilioAccountSid { get; set; }
        
        [Column("TwilioAccountAuthToken")]
        [StringLength(500)]
        public string TwilioAccountAuthToken { get; set; }
        
        [Column("TwilioAccountPhoneNumber")]
        [StringLength(255)]
        public string TwilioAccountPhoneNumber { get; set; }
        
        [Column("IsEnabled")]
        [NotNull]
        public bool IsEnabled { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"VoiceNotificationServerId = {VoiceNotificationServerId} , ");
            sb.AppendLine($"VoiceNotificationServerCode = {VoiceNotificationServerCode} , ");
            sb.AppendLine($"VoiceNotificationServerName = {VoiceNotificationServerName} , ");
            sb.AppendLine($"PredefinedSmsAndVoiceServerTypeId = {PredefinedSmsAndVoiceServerTypeId} , ");
            sb.AppendLine($"TwilioAccountSid = {TwilioAccountSid} , ");
            sb.AppendLine($"TwilioAccountAuthToken = {TwilioAccountAuthToken} , ");
            sb.AppendLine($"TwilioAccountPhoneNumber = {TwilioAccountPhoneNumber} , ");
            sb.AppendLine($"IsEnabled = {IsEnabled} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public VoiceNotificationServer New(VoiceNotificationServer item)
        {
            if (item == null)
                return null;

            return new VoiceNotificationServer
            {
            VoiceNotificationServerId = item.VoiceNotificationServerId,
            VoiceNotificationServerCode = item.VoiceNotificationServerCode,
            VoiceNotificationServerName = item.VoiceNotificationServerName,
            PredefinedSmsAndVoiceServerTypeId = item.PredefinedSmsAndVoiceServerTypeId,
            TwilioAccountSid = item.TwilioAccountSid,
            TwilioAccountAuthToken = item.TwilioAccountAuthToken,
            TwilioAccountPhoneNumber = item.TwilioAccountPhoneNumber,
            IsEnabled = item.IsEnabled,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public VoiceNotificationServer NewFromObject()
        {
            return New(this);
        }
    }
    
    [Table("WhatsAppNotificationServer")]
    [Serializable]
    public partial class WhatsAppNotificationServer : IAuditableModel, INewObject<WhatsAppNotificationServer>
    {
        public const string TABLE_NAME = "WhatsAppNotificationServer";

        public const string ALL_COLUMNS_FOR_SELECT = "*";

        #region Columns
        public const string COLUMN_WhatsAppNotificationServerId = "WhatsAppNotificationServerId";
        public const string COLUMN_WhatsAppNotificationServerCode = "WhatsAppNotificationServerCode";
        public const string COLUMN_WhatsAppNotificationServerName = "WhatsAppNotificationServerName";
        public const string COLUMN_PredefinedSmsAndVoiceServerTypeId = "PredefinedSmsAndVoiceServerTypeId";
        public const string COLUMN_ApiKey = "ApiKey";
        public const string COLUMN_ApiSecret = "ApiSecret";
        public const string COLUMN_AccessToken = "AccessToken";
        public const string COLUMN_VersionNumber = "VersionNumber";
        public const string COLUMN_PhoneNumberId = "PhoneNumberId";
        public const string COLUMN_BusinessAccountId = "BusinessAccountId";
        public const string COLUMN_SenderPhoneNumber = "SenderPhoneNumber";
        public const string COLUMN_IsEnabled = "IsEnabled";
        public const string COLUMN_IsDeleted = "IsDeleted";
        public const string COLUMN_CreatedDate = "CreatedDate";
        public const string COLUMN_CreatedByUserId = "CreatedByUserId";
        public const string COLUMN_ModifiedDate = "ModifiedDate";
        public const string COLUMN_ModifiedByUserId = "ModifiedByUserId";
        #endregion

        #region ColumnSize
        public const int COLUMN_SIZE_WhatsAppNotificationServerCode = 45;
        public const int COLUMN_SIZE_WhatsAppNotificationServerName = 100;
        public const int COLUMN_SIZE_ApiKey = 255;
        public const int COLUMN_SIZE_ApiSecret = 255;
        public const int COLUMN_SIZE_AccessToken = 255;
        public const int COLUMN_SIZE_VersionNumber = 10;
        public const int COLUMN_SIZE_PhoneNumberId = 50;
        public const int COLUMN_SIZE_BusinessAccountId = 50;
        public const int COLUMN_SIZE_SenderPhoneNumber = 20;
        #endregion

        #region ColumnNull
        public const bool COLUMN_NULLABLE_WhatsAppNotificationServerId = false;
        public const bool COLUMN_NULLABLE_WhatsAppNotificationServerCode = false;
        public const bool COLUMN_NULLABLE_WhatsAppNotificationServerName = false;
        public const bool COLUMN_NULLABLE_PredefinedSmsAndVoiceServerTypeId = false;
        public const bool COLUMN_NULLABLE_ApiKey = false;
        public const bool COLUMN_NULLABLE_ApiSecret = false;
        public const bool COLUMN_NULLABLE_AccessToken = false;
        public const bool COLUMN_NULLABLE_VersionNumber = false;
        public const bool COLUMN_NULLABLE_PhoneNumberId = false;
        public const bool COLUMN_NULLABLE_BusinessAccountId = true;
        public const bool COLUMN_NULLABLE_SenderPhoneNumber = true;
        public const bool COLUMN_NULLABLE_IsEnabled = false;
        public const bool COLUMN_NULLABLE_IsDeleted = false;
        public const bool COLUMN_NULLABLE_CreatedDate = false;
        public const bool COLUMN_NULLABLE_CreatedByUserId = true;
        public const bool COLUMN_NULLABLE_ModifiedDate = false;
        public const bool COLUMN_NULLABLE_ModifiedByUserId = true;
        #endregion

        [Column("WhatsAppNotificationServerId")]
        [LinqToDB.Mapping.PrimaryKey]
        [Identity]
        [NotNull]
        public long WhatsAppNotificationServerId { get; set; }
        
        [Column("WhatsAppNotificationServerCode")]
        [NotNull]
        [StringLength(45)]
        public string WhatsAppNotificationServerCode { get; set; }
        
        [Column("WhatsAppNotificationServerName")]
        [NotNull]
        [StringLength(100)]
        public string WhatsAppNotificationServerName { get; set; }
        
        [Column("PredefinedSmsAndVoiceServerTypeId")]
        [NotNull]
        public long PredefinedSmsAndVoiceServerTypeId { get; set; }
        
        [Column("ApiKey")]
        [NotNull]
        [StringLength(255)]
        public string ApiKey { get; set; }
        
        [Column("ApiSecret")]
        [NotNull]
        [StringLength(255)]
        public string ApiSecret { get; set; }
        
        [Column("AccessToken")]
        [NotNull]
        [StringLength(255)]
        public string AccessToken { get; set; }
        
        [Column("VersionNumber")]
        [NotNull]
        [StringLength(10)]
        public string VersionNumber { get; set; }
        
        [Column("PhoneNumberId")]
        [NotNull]
        [StringLength(50)]
        public string PhoneNumberId { get; set; }
        
        [Column("BusinessAccountId")]
        [StringLength(50)]
        public string BusinessAccountId { get; set; }
        
        [Column("SenderPhoneNumber")]
        [StringLength(20)]
        public string SenderPhoneNumber { get; set; }
        
        [Column("IsEnabled")]
        [NotNull]
        public bool IsEnabled { get; set; }
        
        [Column("IsDeleted")]
        [NotNull]
        public bool IsDeleted { get; set; }
        
        [Column("CreatedDate")]
        [NotNull]
        public DateTime CreatedDate { get; set; }
        
        [Column("CreatedByUserId")]
        public long? CreatedByUserId { get; set; }
        
        [Column("ModifiedDate")]
        [NotNull]
        public DateTime ModifiedDate { get; set; }
        
        [Column("ModifiedByUserId")]
        public long? ModifiedByUserId { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"WhatsAppNotificationServerId = {WhatsAppNotificationServerId} , ");
            sb.AppendLine($"WhatsAppNotificationServerCode = {WhatsAppNotificationServerCode} , ");
            sb.AppendLine($"WhatsAppNotificationServerName = {WhatsAppNotificationServerName} , ");
            sb.AppendLine($"PredefinedSmsAndVoiceServerTypeId = {PredefinedSmsAndVoiceServerTypeId} , ");
            sb.AppendLine($"ApiKey = {ApiKey} , ");
            sb.AppendLine($"ApiSecret = {ApiSecret} , ");
            sb.AppendLine($"AccessToken = {AccessToken} , ");
            sb.AppendLine($"VersionNumber = {VersionNumber} , ");
            sb.AppendLine($"PhoneNumberId = {PhoneNumberId} , ");
            sb.AppendLine($"BusinessAccountId = {BusinessAccountId} , ");
            sb.AppendLine($"SenderPhoneNumber = {SenderPhoneNumber} , ");
            sb.AppendLine($"IsEnabled = {IsEnabled} , ");
            sb.AppendLine($"IsDeleted = {IsDeleted} , ");
            sb.AppendLine($"CreatedDate = {CreatedDate} , ");
            sb.AppendLine($"CreatedByUserId = {CreatedByUserId} , ");
            sb.AppendLine($"ModifiedDate = {ModifiedDate} , ");
            sb.AppendLine($"ModifiedByUserId = {ModifiedByUserId} , ");
            return sb.ToString();
        }

        public WhatsAppNotificationServer New(WhatsAppNotificationServer item)
        {
            if (item == null)
                return null;

            return new WhatsAppNotificationServer
            {
            WhatsAppNotificationServerId = item.WhatsAppNotificationServerId,
            WhatsAppNotificationServerCode = item.WhatsAppNotificationServerCode,
            WhatsAppNotificationServerName = item.WhatsAppNotificationServerName,
            PredefinedSmsAndVoiceServerTypeId = item.PredefinedSmsAndVoiceServerTypeId,
            ApiKey = item.ApiKey,
            ApiSecret = item.ApiSecret,
            AccessToken = item.AccessToken,
            VersionNumber = item.VersionNumber,
            PhoneNumberId = item.PhoneNumberId,
            BusinessAccountId = item.BusinessAccountId,
            SenderPhoneNumber = item.SenderPhoneNumber,
            IsEnabled = item.IsEnabled,
            IsDeleted = item.IsDeleted,
            CreatedDate = item.CreatedDate,
            CreatedByUserId = item.CreatedByUserId,
            ModifiedDate = item.ModifiedDate,
            ModifiedByUserId = item.ModifiedByUserId,
            };
        }

        public WhatsAppNotificationServer NewFromObject()
        {
            return New(this);
        }
    }
    }
#pragma warning restore SA1204
#pragma warning restore CC0021
#pragma warning restore CC0065
#pragma warning restore SA1601
#pragma warning restore SA1649
#pragma warning restore SA1121
#pragma warning restore SA1505
#pragma warning restore SA1402
#pragma warning restore SA1124
#pragma warning restore SA1028
#pragma warning restore SA1516
#pragma warning restore SA1310