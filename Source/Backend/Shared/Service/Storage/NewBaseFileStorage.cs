using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Framework.Infrastructure.Exceptions;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Utils;
using Manifest.Common.Enums;
using Manifest.Service.Cache;
using Manifest.Service.Config;
using Stowage;

namespace Manifest.Service.Storage
{
    public abstract class NewBaseFileStorage
    {
        protected ILog log;
        private readonly PredefinedFileStorageTypes customerFileStorageType;
        private readonly CompanyCacheService customerCacheService;
        private readonly AppConfigurationFile appConfigurationFile;

        protected NewBaseFileStorage(PredefinedFileStorageTypes customerFileStorageType, ILog log, CompanyCacheService customerCacheService, AppConfigurationFile appConfigurationFile)
        {
            this.customerFileStorageType = customerFileStorageType;
            this.log = log;
            this.customerCacheService = customerCacheService;
            this.appConfigurationFile = appConfigurationFile;
        }

        public async Task<bool> ExistsAsync(long companyId, IOPath path, CancellationToken cancellationToken = default)
        {
            var fileStorage = await GetFileStorageAsync(companyId, customerFileStorageType);
            return await fileStorage.Exists(path, cancellationToken);
        }

        public async Task<IFileStorage> GetFileStorageAsync(long companyId, PredefinedFileStorageTypes tenantFileStorageType)
        {
            var storageConfig = await customerCacheService.GetCompanyStorageAsync();
            if (storageConfig == null)
                throw new RollbackException("Storage is not configured properly", new { companyId });

            string connectionString;
            switch (tenantFileStorageType)
            {
                case PredefinedFileStorageTypes.ProfilePictureFileStorage:
                    {
                        connectionString = storageConfig.ProfilePictureFileStorageConnectionString;
                        if (connectionString.IsTrimmedStringNullOrEmpty())
                            throw new RollbackException("Profile Picture File Storage is not configured properly", new { companyId });
                        break;
                    }

                case PredefinedFileStorageTypes.ReportServerFileStorage:
                    {
                        connectionString = storageConfig.ReportServerFileStorageConnectionString;
                        if (connectionString.IsTrimmedStringNullOrEmpty())
                            throw new RollbackException("Report Server File Storage is not configured properly", new { companyId });
                        break;
                    }

                case PredefinedFileStorageTypes.FileAttachmentFileStorage:
                    {
                        connectionString = storageConfig.FileAttachmentFileStorageConnectionString;
                        if (connectionString.IsTrimmedStringNullOrEmpty())
                            throw new RollbackException("File Attachment File Storage is not configured properly", new { companyId });
                        break;
                    }

                default:
                    throw new RollbackException("Storage Type is not configured properly", new { companyId });
            }

            if (connectionString.IsTrimmedStringNullOrEmpty())
            {
                throw new RollbackException("Storage setting is not configured properly", new { companyId });
            }

            connectionString = appConfigurationFile.GetReplacedConfigFolderString(connectionString);
            return Files.Of.ConnectionString(connectionString);

            /*
            var uri = new Uri(connectionString);
            if (uri.Scheme == "local_disk")
            {
                var localFileShareFolderName = uri.LocalPath;
                return Files.Of.LocalDisk(localFileShareFolderName);
            }
            else if (uri.Scheme == "aws_s3")
            {
                var awsBucketName = uri.Host;
                var awsAccessKeyId = GetQueryValue(uri,"AccessKeyId");
                var awsSecretAccessKey = GetQueryValue(uri, "SecretAccessKey");
                var awsRegion = GetQueryValue(uri,"Region");
                return Files.Of.AmazonS3(awsBucketName, awsAccessKeyId, awsSecretAccessKey, awsRegion);
            }
            else if (uri.Scheme == "azure_blob")
            {
                var azureBlobStorageContainerName = uri.Host;
                var accountName = GetQueryValue(uri, "AccountName");
                var key = GetQueryValue(uri, "Key");
                return Files.Of.AzureBlobStorage(accountName, key, azureBlobStorageContainerName);
            }
            else if (uri.Scheme == "google_cloud")
            {
                var bucketName = uri.Host;
                return Files.Of.GoogleCloudStorage(bucketName, "");
            }
            else
            {
                throw new RollbackException($"Storage Type {uri.Scheme} is not available", new { companyId });
            }
            */
        }

        public string GetQueryValue(Uri uri, string queryName)
        {
            Uri myUri = uri;
            NameValueCollection queryParameters = HttpUtility.ParseQueryString(myUri.Query);
            return queryParameters[queryName];
        }

        public async Task<IReadOnlyCollection<IOEntry>> LsAsync(long companyId, IOPath path = null, bool recurse = false, CancellationToken cancellationToken = default)
        {
            var fileStorage = await GetFileStorageAsync(companyId, customerFileStorageType);
            return await fileStorage.Ls(path, recurse, cancellationToken);
        }

        public async Task<Stream> OpenReadAsync(long companyId, IOPath path, CancellationToken cancellationToken = default)
        {
            var fileStorage = await GetFileStorageAsync(companyId, customerFileStorageType);
            return await fileStorage.OpenRead(path, cancellationToken);
        }

        public async Task<Stream> OpenWriteAsync(long companyId, IOPath path, CancellationToken cancellationToken = default)
        {
            var fileStorage = await GetFileStorageAsync(companyId, customerFileStorageType);
            return await fileStorage.OpenWrite(path, cancellationToken);
        }

        public async Task<string> ReadTextAsync(long companyId, IOPath path, Encoding encoding = null, CancellationToken cancellationToken = default)
        {
            var fileStorage = await GetFileStorageAsync(companyId, customerFileStorageType);
            return await fileStorage.ReadText(path, encoding, cancellationToken);
        }

        public async Task RenAsync(long companyId, IOPath name, IOPath newName, CancellationToken cancellationToken = default)
        {
            var fileStorage = await GetFileStorageAsync(companyId, customerFileStorageType);
            await fileStorage.Ren(name, newName, cancellationToken);
        }

        public async Task RmAsync(long companyId, IOPath path, CancellationToken cancellationToken = default)
        {
            var fileStorage = await GetFileStorageAsync(companyId, customerFileStorageType);
            await fileStorage.Rm(path, cancellationToken);
        }

        public async Task WriteAsJsonAsync(long companyId, IOPath path, object value, bool pretty = false, CancellationToken cancellationToken = default)
        {
            var fileStorage = await GetFileStorageAsync(companyId, customerFileStorageType);
            await fileStorage.WriteAsJson(path, value, pretty, cancellationToken);
        }

        public async Task WriteTextAsync(long companyId, IOPath path, string contents, Encoding encoding = null, CancellationToken cancellationToken = default)
        {
            var fileStorage = await GetFileStorageAsync(companyId, customerFileStorageType);
            await fileStorage.WriteText(path, contents, encoding, cancellationToken);
        }

        public async Task WriteFileAsync(long companyId, string destinationFilePath, string sourceFilePath, CancellationToken cancellationToken = default)
        {
            using (Stream stream = File.OpenRead(sourceFilePath))
            {
                if (stream == null)
                    throw new Exception($"Unable to open Source File - {sourceFilePath}");

                using (Stream destinationStream = await OpenWriteAsync(companyId, destinationFilePath))
                {
                    if (destinationStream == null)
                        throw new Exception($"Unable to open destination File - {destinationFilePath}");

                    await stream.CopyToAsync(destinationStream, cancellationToken: cancellationToken, bufferSize: 81920);
                }
            }
        }

        public async Task WriteAsync(long companyId, string fullPath, Stream sourceStream, bool append = false, CancellationToken cancellationToken = default)
        {
            using (Stream destinationStream = await OpenWriteAsync(companyId, fullPath))
            {
                if (destinationStream == null)
                    throw new Exception($"Unable to open destination File - {fullPath}");

                await sourceStream.CopyToAsync(destinationStream, cancellationToken: cancellationToken, bufferSize: 81920);
            }
        }

        public Task DeleteAsync(long companyId, string fullPath, CancellationToken cancellationToken = default)
        {
            return RmAsync(companyId, fullPath, cancellationToken: cancellationToken);
        }

        public string MonthFromNumberAsString(int month)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
        }
    }
}
