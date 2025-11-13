using System;
using Framework.Infrastructure.Logging;
using Manifest.Common.Enums;
using Manifest.Service.Cache;
using Manifest.Service.Config;

namespace Manifest.Service.Storage
{
    public class FileAttachmentFileStorage : NewBaseFileStorage
    {
        private const string reportStoragePrefix = "AttachmentFiles";
        private const string userStoragePrefix = "UserFiles";

        public FileAttachmentFileStorage(ILog log, CompanyCacheService companyCacheService, AppConfigurationFile appConfigurationFile)
            : base(PredefinedFileStorageTypes.FileAttachmentFileStorage, log, companyCacheService, appConfigurationFile)
        {
        }

        public string FileAttachmentFilePath(string companyId, string fileAttachmentId, string filename, DateTime? date = null)
        {
            return $"{reportStoragePrefix}/{companyId}/{(date ?? DateTime.Now).Year}/{MonthFromNumberAsString((date ?? DateTime.Now).Month)}/{fileAttachmentId}/{filename}";
        }

        public string VisionBoardFilePath(string userId, string visionBoardPictureId, string filename)
        {
            return $"{userStoragePrefix}/{userId}/VisionBoard/{visionBoardPictureId}/{filename}";
        }

        public string VisionBoardThumbnailFilePath(string userId, string visionBoardPictureId, string filename)
        {
            return $"{userStoragePrefix}/{userId}/VisionBoard/{visionBoardPictureId}/Thumbnail/{filename}";
        }
    }
}
