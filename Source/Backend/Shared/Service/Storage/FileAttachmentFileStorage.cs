using System;
using Manifest.Common.Enums;
using Manifest.Service.Cache;
using Framework.Infrastructure.Logging;

namespace Manifest.Service.Storage
{
    public class FileAttachmentFileStorage : NewBaseFileStorage
    {
        private readonly string reportStoragePrefix;

        public FileAttachmentFileStorage(ILog log, CompanyCacheService companyCacheService)
            : base(PredefinedFileStorageTypes.FileAttachmentFileStorage, log, companyCacheService)
        {
            reportStoragePrefix = "AttachmentFiles";
        }

        public string FileAttachmentFilePath(string companyId, string fileAttachmentId, string filename, DateTime? date = null)
        {
            return $"{reportStoragePrefix}/{companyId}/{(date ?? DateTime.Now).Year}/{MonthFromNumberAsString((date ?? DateTime.Now).Month)}/{fileAttachmentId}/{filename}";
        }
    }
}
