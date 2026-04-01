using Framework.Infrastructure.Exceptions;
using Framework.Infrastructure.Interfaces.DbAccess;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Utils;
using Manifest.Common.Enums;
using Manifest.Common.Interfaces.Service.App;
using Manifest.Common.Models.DB;
using Manifest.Common.Models.DBView;
using Manifest.Common.Models.SearchCriteria;
using Manifest.Common.Models.ViewModels;
using Manifest.Repository.Common;
using Manifest.Service.JobServer;
using Manifest.Service.ObjectMapping;
using Stowage.Impl.Databricks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manifest.Service.App
{
    public partial class ReportService : BaseAppService
    {
        private readonly ReportRepository reportRepository;
        private readonly CompanyRepository companyRepository;
        private readonly ITransactionManager transactionManager;
        private readonly ExternalAppJob externalAppJob;
        private readonly PredefinedAppJob predefinedAppJob;
        private readonly ICurrentSessionService currentSessionService;
        private readonly ReportCreationService jobCreationService;

        public ReportService(ILog log, ObjectMapperService objectMapperService, ReportRepository reportRepository, CompanyRepository companyRepository, ITransactionManager transactionManager, ExternalAppJob externalAppJob, PredefinedAppJob predefinedAppJob, ICurrentSessionService currentSessionService, ReportCreationService jobCreationService)
            : base(log, objectMapperService)
        {
            this.reportRepository = reportRepository;
            this.companyRepository = companyRepository;
            this.transactionManager = transactionManager;
            this.externalAppJob = externalAppJob;
            this.predefinedAppJob = predefinedAppJob;
            this.currentSessionService = currentSessionService;
            this.jobCreationService = jobCreationService;
        }

        #region Job

        public async Task<ReturnModel<bool>> RunJobAsync(long jobId)
        {
            var job = await reportRepository.GetJobByIdAsync(jobId);
            if (job == null)
            {
                return log.ErrorAndReturnModel<bool>("Unable to find the job", new { jobId });
            }

            //Rachita: Update the Status before stating
            await reportRepository.SetJobStatusAsRunningAsync(jobId, "Starting...");

            var predefinedJobTypeId = job.PredefinedJobTypeId;

            ReturnModel<bool> result;
            if (predefinedJobTypeId == (int)PredefinedJobTypes.None)
                result = await externalAppJob.RunAsync(jobId);
            else
                result = await predefinedAppJob.RunAsync(jobId);

            if (result.IsSuccess)
            {
                await reportRepository.SetJobStatusAsCompleted(jobId, "Completed");
            }
            else
            {
                await reportRepository.SetJobStatusAsErrorAsync(jobId, result.ErrorHolder?.FriendlyMessage);
            }

            var emails = await reportRepository.GetJobNotificationEmailByJobIdAsync(jobId);
            if (emails.Count > 0)
            {
                log.Info("//Rachita: contract the email as per the status and the send emails");
            }

            return result;
        }

        public Task<ReturnModel<bool>> IsJobParameterValidAsync(Common.Models.DB.Job job)
        {
            return jobCreationService.IsJobParameterValidAsync(job);
        }

        public async Task<ReturnListModel<JobModel>> GetJobsAsync(JobSearchCriteria search)
        {
            try
            {
                return this.ConvertToList(await reportRepository.GetJobsAsync(search),objectMapperService.JobViewListToJobModelList);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<JobModel>($"Unexpected error when getting Jobs", ex);
            }
        }

        public Task<ReturnModel<JobModel>> GetJobByIdForReadAsync(long jobId)
        {
            return GetJobByIdInternalAsync(forWrite: false, jobId);
        }

        public Task<ReturnModel<JobModel>> GetJobByIdForWriteAsync(long jobId)
        {
            return GetJobByIdInternalAsync(forWrite: true, jobId);
        }

        public async Task<ReturnModel<JobModel>> GetJobByIdInternalAsync(bool forWrite, long jobId)
        {
            try
            {
                var item = await reportRepository.GetJobViewByIdAsync(jobId);
                if (item == null)
                {
                    return log.ErrorAndReturnModel<JobModel>($"Unable to find the job", new { jobId });
                }

                var result = this.ConvertToItem(item, objectMapperService.JobViewToJobModel);
                if (result.IsSuccess)
                {
                    result.Model.Attachments = objectMapperService.JobAttachmentViewListToJobAttachmentModelList((await reportRepository.GetJobAttachmentsAsync(new JobAttachmentSearchCriteria { JobId = jobId, PageSize = int.MaxValue })).Result);
                    result.Model.NotificationEmails = objectMapperService.JobNotificationEmailViewListToJobNotificationEmailModelList((await reportRepository.GetJobNotificationEmailsAsync(new JobNotificationEmailSearchCriteria { JobId = jobId, PageSize = int.MaxValue })).Result);
                }

                return result;
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<JobModel>($"Unexpected error when getting job", new { jobId }, ex);
            }
        }
        #endregion

        #region JobAttachment

        public Task<ReturnModel<JobAttachmentModel>> GetJobAttachmentByIdForReadAsync(long jobAttachmentId)
        {
            return GetJobAttachmentByIdInternalAsync(forWrite: false, jobAttachmentId);
        }

        public Task<ReturnModel<JobAttachmentModel>> GetJobAttachmentByIdForWriteAsync(long jobAttachmentId)
        {
            return GetJobAttachmentByIdInternalAsync(forWrite: true, jobAttachmentId);
        }

        public async Task<ReturnModel<JobAttachmentModel>> GetJobAttachmentByIdInternalAsync(bool forWrite, long jobAttachmentId)
        {
            try
            {
                var item = await reportRepository.GetJobAttachmentViewByIdAsync(jobAttachmentId);
                if (item == null)
                {
                    return log.ErrorAndReturnModel<JobAttachmentModel>($"Unable to find the jobAttachment", new { jobAttachmentId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanViewJobAttachmentPermissionParamsAsync(item.CompanyId, item.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<JobAttachmentModel>(errorStr);
                //}

                return this.ConvertToItem(item, objectMapperService.JobAttachmentViewToJobAttachmentModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<JobAttachmentModel>($"Unexpected error when getting jobAttachment", new { jobAttachmentId }, ex);
            }
        }

        public async Task<ReturnModel<JobAttachmentModel>> CreateJobAttachmentAsync(JobAttachmentCreateModel jobAttachment)
        {
            try
            {
                if (jobAttachment == null)
                {
                    return log.ErrorAndReturnModel<JobAttachmentModel>($"Unable to create JobAttachment", new ReturnErrorItem("", "JobAttachment information is not valid"));
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.CanCreateJobAttachmentAsync();

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<JobAttachmentModel>(errorStr);
                //}

                //if (canAssign)
                //{
                //    jobAttachment.CompanyId = await currentSessionService.GetCurrentTenantIdAsync();
                //    jobAttachment.TenantEntityId = await currentSessionService.GetCurrentTenantEntityIdAsNullableAsync();
                //}

                var errorList = IsJobAttachmentModelValidForCreateAsync(jobAttachment);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<JobAttachmentModel>($"Unable to create jobAttachment", errorList);
                }

                jobAttachment.IsDeleted = false;

                var item = objectMapperService.JobAttachmentCreateModelToJobAttachment(jobAttachment);

                if (await reportRepository.GetJobTemplateByIdAsync(jobAttachment.JobId) == null)
                {
                    return log.ErrorAndReturnModel<JobAttachmentModel>($"Unable to Get the Job Information", new { jobAttachment.JobId });
                }

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    item = await reportRepository.CreateJobAttachmentAsync(item, await currentSessionService.GetCurrentUserIdAsync());
                    if (item == null)
                    {
                        throw new Exception($"Unable to create jobAttachment");
                    }

                    await trx.CompleteAsync();
                }

                // await auditService.JobAttachmentCreatedAsync(item.JobAttachmentId);

                var view = await reportRepository.GetJobAttachmentViewByIdAsync(item.JobAttachmentId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<JobAttachmentModel>($"Unable to find the jobAttachment", new { item.JobAttachmentId });
                }

                return this.ConvertToItem(view, objectMapperService.JobAttachmentViewToJobAttachmentModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModelWithRollbackExceptionHandler<JobAttachmentModel>($"Unexpected error when creating JobAttachment {jobAttachment?.JobAttachmentId}", ex);
            }
        }

        public async Task<ReturnModel<JobAttachmentModel>> UpdateJobAttachmentAsync(JobAttachmentUpdateModel jobAttachment)
        {
            try
            {
                if (jobAttachment == null)
                {
                    return log.ErrorAndReturnModel<JobAttachmentModel>($"Unable to update JobAttachment", new ReturnErrorItem("", "JobAttachment information is not valid"));
                }

                var existingJobAttachment = await reportRepository.GetJobAttachmentViewFromAnyByIdAsync(jobAttachment.JobAttachmentId);
                if (existingJobAttachment == null)
                {
                    return log.ErrorAndReturnModel<JobAttachmentModel>($"Unable to find the jobAttachment", new { jobAttachment.JobAttachmentId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanManageJobAttachmentPermissionParamsAsync(existingJobAttachment.CompanyId, existingJobAttachment.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<JobAttachmentModel>(errorStr);
                //}

                //if (canAssign)
                //{
                //    jobAttachment.CompanyId = await currentSessionService.GetCurrentTenantIdAsync();
                //    //jobAttachment.TenantEntityId = await currentSessionService.GetCurrentTenantEntityIdAsNullableAsync();
                //}

                jobAttachment.CreatedByUserId = existingJobAttachment.CreatedByUserId;
                jobAttachment.CreatedDate = existingJobAttachment.CreatedDate;
                jobAttachment.IsDeleted = existingJobAttachment.IsDeleted;

                var errorList = IsJobAttachmentModelValidForUpdateAsync(jobAttachment);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<JobAttachmentModel>($"Unable to update jobAttachment", errorList);
                }

                var item = objectMapperService.JobAttachmentUpdateModelToJobAttachment(jobAttachment);

                if (await reportRepository.GetJobTemplateByIdAsync(jobAttachment.JobId) == null)
                {
                    return log.ErrorAndReturnModel<JobAttachmentModel>($"Unable to Get the Job Information", new { jobAttachment.JobId });
                }

                item.JobId = jobAttachment.JobId;

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await reportRepository.UpdateJobAttachmentAsync(item, await currentSessionService.GetCurrentUserIdAsync());
                    if (result == false)
                    {
                        throw new RollbackException($"Unable to update jobAttachment");
                    }

                    await trx.CompleteAsync();
                }

                // await auditService.JobAttachmentUpdatedAsync(jobAttachment.JobAttachmentId);

                var view = await reportRepository.GetJobAttachmentViewByIdAsync(jobAttachment.JobAttachmentId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<JobAttachmentModel>($"Unable to find the jobAttachment", new { jobAttachment.JobAttachmentId });
                }

                return this.ConvertToItem(view, objectMapperService.JobAttachmentViewToJobAttachmentModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<JobAttachmentModel>($"Unexpected error when updating JobAttachment {jobAttachment?.JobAttachmentId}", ex);
            }
        }

        public async Task<ReturnModel<bool>> MarkJobAttachmentAsNotDeletedByIdAsync(long jobAttachmentId)
        {
            try
            {
                var jobAttachment = await reportRepository.GetJobAttachmentViewFromAnyByIdAsync(jobAttachmentId);
                if (jobAttachment == null)
                {
                    return log.ErrorAndReturnModel<bool>($"Unable to find the jobAttachment", new { jobAttachmentId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanManageJobAttachmentPermissionParamsAsync(jobAttachment.CompanyId, jobAttachment.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<bool>(errorStr);
                //}

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await reportRepository.MarkJobAttachmentAsNotDeletedByIdAsync(jobAttachmentId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                // await auditService.JobAttachmentDeletedAsync(jobAttachment.JobAttachmentId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking jobAttachment as not deleted {jobAttachmentId}", ex);
            }
        }

        public async Task<ReturnModel<bool>> MarkJobAttachmentAsDeletedByIdAsync(long jobAttachmentId)
        {
            try
            {
                var jobAttachment = await reportRepository.GetJobAttachmentViewFromAnyByIdAsync(jobAttachmentId);
                if (jobAttachment == null)
                {
                    return log.ErrorAndReturnModel<bool>($"Unable to find the jobAttachment", new { jobAttachmentId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanManageJobAttachmentPermissionParamsAsync(jobAttachment.CompanyId, jobAttachment.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<bool>(errorStr);
                //}

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await reportRepository.MarkJobAttachmentAsDeletedByIdAsync(jobAttachmentId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                // await auditService.JobAttachmentDeletedAsync(jobAttachment.JobAttachmentId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking jobAttachment as deleted {jobAttachmentId}", ex);
            }
        }

        public async Task<ReturnModel<bool>> HardDeleteJobAttachmentByAsync(long jobAttachmentId)
        {
            try
            {
                var jobAttachment = await reportRepository.GetJobAttachmentViewFromAnyByIdAsync(jobAttachmentId);
                if (jobAttachment == null)
                {
                    return log.ErrorAndReturnModel<bool>($"Unable to find the jobAttachment", new { jobAttachmentId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanManageJobAttachmentPermissionParamsAsync(jobAttachment.CompanyId, jobAttachment.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<bool>(errorStr);
                //}

                bool result;

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await reportRepository.HardDeleteJobAttachmentByIdAsync(jobAttachmentId);
                    await trx.CompleteAsync();
                }

                // await auditService.HardDeleteJobAttachmentAsync(jobAttachment.JobAttachmentId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when deleting jobAttachment {jobAttachmentId}", ex);
            }
        }

        public List<ReturnErrorItem> IsJobAttachmentModelValidForCreateAsync(JobAttachmentUpdateModel jobAttachmentModel)
        {
            return IsJobAttachmentModelValidForAllAsync(true, jobAttachmentModel);
        }

        public List<ReturnErrorItem> IsJobAttachmentModelValidForUpdateAsync(JobAttachmentUpdateModel jobAttachmentModel)
        {
            return IsJobAttachmentModelValidForAllAsync(false, jobAttachmentModel);
        }

        public List<ReturnErrorItem> IsJobAttachmentModelValidForAllAsync(bool isCreate, JobAttachmentUpdateModel jobAttachmentModel)
        {
            var result = new List<ReturnErrorItem>();

            if (isCreate)
            {
                if (jobAttachmentModel.JobAttachmentId > 0)
                {
                    result.Add(new ReturnErrorItem("", "JobAttachment Identifier is not valid"));
                }
            }
            else
            {
                if (jobAttachmentModel.JobAttachmentId <= 0)
                {
                    result.Add(new ReturnErrorItem("", "JobAttachment Unique Identifier is not valid"));
                }
            }

            if (!ValidationUtils.IsGuidValid(jobAttachmentModel.JobId))
            {
                result.Add(new ReturnErrorItem("JobId", "Job cannot be empty"));
            }

            if (jobAttachmentModel.FileName != null && jobAttachmentModel.FileName.Length > JobAttachmentModel.COLUMN_SIZE_FileName)
            {
                result.Add(new ReturnErrorItem("FileName", $"FileName size should not exceed {JobAttachmentModel.COLUMN_SIZE_FileName}"));
            }

            if (jobAttachmentModel.FileName.IsTrimmedStringNullOrEmpty())
            {
                result.Add(new ReturnErrorItem("FileName", "FileName is required "));
            }

            if (jobAttachmentModel.StoredFilePath != null && jobAttachmentModel.StoredFilePath.Length > JobAttachmentModel.COLUMN_SIZE_StoredFilePath)
            {
                result.Add(new ReturnErrorItem("StoredFilePath", $"StoredFilePath size should not exceed {JobAttachmentModel.COLUMN_SIZE_StoredFilePath}"));
            }

            if (jobAttachmentModel.StoredFilePath.IsTrimmedStringNullOrEmpty())
            {
                result.Add(new ReturnErrorItem("StoredFilePath", "StoredFilePath is required "));
            }

            return result.Count > 0 ? result : null;
        }
        #endregion

        #region JobNotificationEmail

        public async Task<ReturnListModel<JobNotificationEmailModel>> GetJobNotificationEmailsAsync(JobNotificationEmailSearchCriteria search)
        {
            try
            {
                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanViewJobNotificationEmailPermissionParamsAsync();

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnListModel<JobNotificationEmailModel>(errorStr);
                //}

                //if (canAssign)
                //{
                //    search.CompanyId = await currentSessionService.GetCurrentCompanyIdAsync();
                //    //search.TenantEntityId = await currentSessionService.GetCurrentTenantEntityIdAsNullableAsync();
                //}

                return this.ConvertToList(await reportRepository.GetJobNotificationEmailsAsync(search), objectMapperService.JobNotificationEmailViewListToJobNotificationEmailModelList);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<JobNotificationEmailModel>($"Unexpected error when getting JobNotificationEmails", ex);
            }
        }

        public Task<ReturnModel<JobNotificationEmailModel>> GetJobNotificationEmailByIdForReadAsync(long jobNotificationEmailId)
        {
            return GetJobNotificationEmailByIdInternalAsync(forWrite: false, jobNotificationEmailId);
        }

        public Task<ReturnModel<JobNotificationEmailModel>> GetJobNotificationEmailByIdForWriteAsync(long jobNotificationEmailId)
        {
            return GetJobNotificationEmailByIdInternalAsync(forWrite: true, jobNotificationEmailId);
        }

        public async Task<ReturnModel<JobNotificationEmailModel>> GetJobNotificationEmailByIdInternalAsync(bool forWrite, long jobNotificationEmailId)
        {
            try
            {
                var item = await reportRepository.GetJobNotificationEmailViewByIdAsync(jobNotificationEmailId);
                if (item == null)
                {
                    return log.ErrorAndReturnModel<JobNotificationEmailModel>($"Unable to find the jobNotificationEmail", new { jobNotificationEmailId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanViewJobNotificationEmailPermissionParamsAsync(item.CompanyId, item.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<JobNotificationEmailModel>(errorStr);
                //}

                return this.ConvertToItem(item, objectMapperService.JobNotificationEmailViewToJobNotificationEmailModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<JobNotificationEmailModel>($"Unexpected error when getting jobNotificationEmail", new { jobNotificationEmailId }, ex);
            }
        }

        public async Task<ReturnModel<JobNotificationEmailModel>> CreateJobNotificationEmailAsync(JobNotificationEmailCreateModel jobNotificationEmail)
        {
            try
            {
                if (jobNotificationEmail == null)
                {
                    return log.ErrorAndReturnModel<JobNotificationEmailModel>($"Unable to create JobNotificationEmail", new ReturnErrorItem("", "JobNotificationEmail information is not valid"));
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.CanCreateJobNotificationEmailAsync();

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<JobNotificationEmailModel>(errorStr);
                //}

                //if (canAssign)
                //{
                //    jobNotificationEmail.CompanyId = await currentSessionService.GetCurrentTenantIdAsync();
                //    jobNotificationEmail.TenantEntityId = await currentSessionService.GetCurrentTenantEntityIdAsNullableAsync();
                //}

                var errorList = IsJobNotificationEmailModelValidForCreateAsync(jobNotificationEmail);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<JobNotificationEmailModel>($"Unable to create jobNotificationEmail", errorList);
                }

                jobNotificationEmail.IsDeleted = false;

                var item = objectMapperService.JobNotificationEmailCreateModelToJobNotificationEmail(jobNotificationEmail);

                if (await reportRepository.GetJobByIdAsync(jobNotificationEmail.JobId) == null)
                {
                    return log.ErrorAndReturnModel<JobNotificationEmailModel>($"Unable to Get the Job Information", new { jobNotificationEmail.JobId });
                }

                item.JobId = jobNotificationEmail.JobId;

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    item = await reportRepository.CreateJobNotificationEmailAsync(item, await currentSessionService.GetCurrentUserIdAsync());
                    if (item == null)
                    {
                        throw new Exception($"Unable to create jobNotificationEmail");
                    }

                    await trx.CompleteAsync();
                }

                // await auditService.JobNotificationEmailCreatedAsync(item.JobNotificationEmailId);

                var view = await reportRepository.GetJobNotificationEmailViewByIdAsync(item.JobNotificationEmailId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<JobNotificationEmailModel>($"Unable to find the jobNotificationEmail", new { item.JobNotificationEmailId });
                }

                return this.ConvertToItem(view, objectMapperService.JobNotificationEmailViewToJobNotificationEmailModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModelWithRollbackExceptionHandler<JobNotificationEmailModel>($"Unexpected error when creating JobNotificationEmail {jobNotificationEmail?.JobNotificationEmailId}", ex);
            }
        }

        public async Task<ReturnModel<JobNotificationEmailModel>> UpdateJobNotificationEmailAsync(JobNotificationEmailUpdateModel jobNotificationEmail)
        {
            try
            {
                if (jobNotificationEmail == null)
                {
                    return log.ErrorAndReturnModel<JobNotificationEmailModel>($"Unable to update JobNotificationEmail", new ReturnErrorItem("", "JobNotificationEmail information is not valid"));
                }

                var existingJobNotificationEmail = await reportRepository.GetJobNotificationEmailViewFromAnyByIdAsync(jobNotificationEmail.JobNotificationEmailId);
                if (existingJobNotificationEmail == null)
                {
                    return log.ErrorAndReturnModel<JobNotificationEmailModel>($"Unable to find the jobNotificationEmail", new { jobNotificationEmail.JobNotificationEmailId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanManageJobNotificationEmailPermissionParamsAsync(existingJobNotificationEmail.CompanyId, existingJobNotificationEmail.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<JobNotificationEmailModel>(errorStr);
                //}

                //if (canAssign)
                //{
                //    jobNotificationEmail.CompanyId = await currentSessionService.GetCurrentTenantIdAsync();
                //    //jobNotificationEmail.TenantEntityId = await currentSessionService.GetCurrentTenantEntityIdAsNullableAsync();
                //}

                jobNotificationEmail.CreatedByUserId = existingJobNotificationEmail.CreatedByUserId;
                jobNotificationEmail.CreatedDate = existingJobNotificationEmail.CreatedDate;
                jobNotificationEmail.IsDeleted = existingJobNotificationEmail.IsDeleted;

                var errorList = IsJobNotificationEmailModelValidForUpdateAsync(jobNotificationEmail);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<JobNotificationEmailModel>($"Unable to update jobNotificationEmail", errorList);
                }

                var item = objectMapperService.JobNotificationEmailUpdateModelToJobNotificationEmail(jobNotificationEmail);

                if (await reportRepository.GetJobByIdAsync(jobNotificationEmail.JobId) == null)
                {
                    return log.ErrorAndReturnModel<JobNotificationEmailModel>($"Unable to Get the Job Information", new { jobNotificationEmail.JobId });
                }

                item.JobId = jobNotificationEmail.JobId;

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await reportRepository.UpdateJobNotificationEmailAsync(item, await currentSessionService.GetCurrentUserIdAsync());
                    if (result == false)
                    {
                        throw new RollbackException($"Unable to update jobNotificationEmail");
                    }

                    await trx.CompleteAsync();
                }

                // await auditService.JobNotificationEmailUpdatedAsync(jobNotificationEmail.JobNotificationEmailId);

                var view = await reportRepository.GetJobNotificationEmailViewByIdAsync(jobNotificationEmail.JobNotificationEmailId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<JobNotificationEmailModel>($"Unable to find the jobNotificationEmail", new { jobNotificationEmail.JobNotificationEmailId });
                }

                return this.ConvertToItem(view, objectMapperService.JobNotificationEmailViewToJobNotificationEmailModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<JobNotificationEmailModel>($"Unexpected error when updating JobNotificationEmail {jobNotificationEmail?.JobNotificationEmailId}", ex);
            }
        }

        public async Task<ReturnModel<bool>> MarkJobNotificationEmailAsNotDeletedByIdAsync(long jobNotificationEmailId)
        {
            try
            {
                var jobNotificationEmail = await reportRepository.GetJobNotificationEmailViewFromAnyByIdAsync(jobNotificationEmailId);
                if (jobNotificationEmail == null)
                {
                    return log.ErrorAndReturnModel<bool>($"Unable to find the jobNotificationEmail", new { jobNotificationEmailId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanManageJobNotificationEmailPermissionParamsAsync(jobNotificationEmail.CompanyId, jobNotificationEmail.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<bool>(errorStr);
                //}

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await reportRepository.MarkJobNotificationEmailAsNotDeletedByIdAsync(jobNotificationEmailId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                // await auditService.JobNotificationEmailDeletedAsync(jobNotificationEmail.JobNotificationEmailId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking jobNotificationEmail as not deleted {jobNotificationEmailId}", ex);
            }
        }

        public async Task<ReturnModel<bool>> MarkJobNotificationEmailAsDeletedByIdAsync(long jobNotificationEmailId)
        {
            try
            {
                var jobNotificationEmail = await reportRepository.GetJobNotificationEmailViewFromAnyByIdAsync(jobNotificationEmailId);
                if (jobNotificationEmail == null)
                {
                    return log.ErrorAndReturnModel<bool>($"Unable to find the jobNotificationEmail", new { jobNotificationEmailId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanManageJobNotificationEmailPermissionParamsAsync(jobNotificationEmail.CompanyId, jobNotificationEmail.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<bool>(errorStr);
                //}

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await reportRepository.MarkJobNotificationEmailAsDeletedByIdAsync(jobNotificationEmailId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                // await auditService.JobNotificationEmailDeletedAsync(jobNotificationEmail.JobNotificationEmailId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking jobNotificationEmail as deleted {jobNotificationEmailId}", ex);
            }
        }

        public async Task<ReturnModel<bool>> HardDeleteJobNotificationEmailByAsync(long jobNotificationEmailId)
        {
            try
            {
                var jobNotificationEmail = await reportRepository.GetJobNotificationEmailViewFromAnyByIdAsync(jobNotificationEmailId);
                if (jobNotificationEmail == null)
                {
                    return log.ErrorAndReturnModel<bool>($"Unable to find the jobNotificationEmail", new { jobNotificationEmailId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanManageJobNotificationEmailPermissionParamsAsync(jobNotificationEmail.CompanyId, jobNotificationEmail.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<bool>(errorStr);
                //}

                bool result;

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await reportRepository.HardDeleteJobNotificationEmailByIdAsync(jobNotificationEmailId);
                    await trx.CompleteAsync();
                }

                // await auditService.HardDeleteJobNotificationEmailAsync(jobNotificationEmail.JobNotificationEmailId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when deleting jobNotificationEmail {jobNotificationEmailId}", ex);
            }
        }

        public List<ReturnErrorItem> IsJobNotificationEmailModelValidForCreateAsync(JobNotificationEmailUpdateModel jobNotificationEmailModel)
        {
            return jobCreationService.IsJobNotificationEmailModelValidForCreateAsync(jobNotificationEmailModel);
        }

        public List<ReturnErrorItem> IsJobNotificationEmailModelValidForUpdateAsync(JobNotificationEmailUpdateModel jobNotificationEmailModel)
        {
            return jobCreationService.IsJobNotificationEmailModelValidForUpdateAsync(jobNotificationEmailModel);
        }
        #endregion

        #region JobTemplate

        public async Task<ReturnListModel<JobTemplateModel>> GetJobTemplatesAsync(JobTemplateSearchCriteria search)
        {
            try
            {
                return this.ConvertToList(await reportRepository.GetJobTemplatesAsync(search), objectMapperService.JobTemplateViewListToJobTemplateModelList);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<JobTemplateModel>($"Unexpected error when getting JobTemplates", ex);
            }
        }

        public async Task<ReturnModel<JobModel>> RunJobTemplateAsync(long jobTemplateId)
        {
            try
            {
                var item = await GetJobTemplateByIdForReadAsync(jobTemplateId);
                if (!item.IsSuccess)
                {
                    return log.ErrorAndReturnModel<JobModel>(item);
                }

                var result = this.ConvertToItem(item.Model, objectMapperService.JobTemplateModelToJobModel);
                return result;
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<JobModel>($"Unexpected error when getting jobTemplate", new { jobTemplateId }, ex);
            }
        }

        public Task<ReturnModel<JobTemplateModel>> GetJobTemplateByIdForReadAsync(long jobTemplateId)
        {
            return GetJobTemplateByIdInternalAsync(forWrite: false, jobTemplateId);
        }

        public Task<ReturnModel<JobTemplateModel>> GetJobTemplateByIdForWriteAsync(long jobTemplateId)
        {
            return GetJobTemplateByIdInternalAsync(forWrite: true, jobTemplateId);
        }

        public async Task<ReturnModel<JobTemplateModel>> GetJobTemplateByIdInternalAsync(bool forWrite, long jobTemplateId)
        {
            try
            {
                var item = await reportRepository.GetJobTemplateViewByIdAsync(jobTemplateId);
                if (item == null)
                {
                    return log.ErrorAndReturnModel<JobTemplateModel>($"Unable to find the jobTemplate", new { jobTemplateId });
                }

                var result = this.ConvertToItem(item, objectMapperService.JobTemplateViewToJobTemplateModel);
                if (result.IsSuccess)
                {
                    result.Model.Parameters = objectMapperService.JobTemplateParameterViewListToJobTemplateParameterModelList((await reportRepository.GetJobTemplateParametersAsync(new JobTemplateParameterSearchCriteria { JobTemplateId = jobTemplateId, PageSize = int.MaxValue })).Result);
                }

                return result;
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<JobTemplateModel>($"Unexpected error when getting jobTemplate", new { jobTemplateId }, ex);
            }
        }

        public async Task<ReturnModel<JobTemplateModel>> CreateJobTemplateAsync(JobTemplateCreateModel jobTemplate)
        {
            try
            {
                if (jobTemplate == null)
                {
                    return log.ErrorAndReturnModel<JobTemplateModel>($"Unable to create JobTemplate", new ReturnErrorItem("", "JobTemplate information is not valid"));
                }

                if (!await currentSessionService.IsCurrentUserASuperAdminAsync())
                {
                    return log.ErrorAndReturnModel<JobTemplateModel>($"You don't have access to create Job Template");
                }

                var errorList = await IsJobTemplateModelValidForCreateAsync(jobTemplate);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<JobTemplateModel>($"Unable to create jobTemplate", errorList);
                }

                jobTemplate.IsDeleted = false;

                var item = objectMapperService.JobTemplateCreateModelToJobTemplate(jobTemplate);

                var jobTemplateCategoryItem = await reportRepository.GetJobTemplateCategoryViewByIdAsync(jobTemplate.JobTemplateCategoryId);
                if (jobTemplateCategoryItem == null)
                {
                    return log.ErrorAndReturnModel<JobTemplateModel>($"Unable to Get the JobTemplateCategory Information", new { jobTemplate.JobTemplateCategoryId });
                }

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    item = await reportRepository.CreateJobTemplateAsync(item, await currentSessionService.GetCurrentUserIdAsync());
                    if (item == null)
                    {
                        throw new RollbackException($"Unable to create jobTemplate");
                    }

                    foreach (var param in jobTemplate.NewParameters)
                    {
                        param.JobTemplateId = item.JobTemplateId;
                        var paramresult = await CreateJobTemplateParameterAsync(param);
                        if (!paramresult.IsSuccess)
                            throw new RollbackException(paramresult);
                    }

                    await trx.CompleteAsync();
                }

                // await auditService.JobTemplateCreatedAsync(item.JobTemplateId);

                var view = await reportRepository.GetJobTemplateViewByIdAsync(item.JobTemplateId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<JobTemplateModel>($"Unable to find the jobTemplate", new { item.JobTemplateId });
                }

                return this.ConvertToItem(view, objectMapperService.JobTemplateViewToJobTemplateModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModelWithRollbackExceptionHandler<JobTemplateModel>($"Unexpected error when creating JobTemplate {jobTemplate?.JobTemplateId}", ex);
            }
        }

        public async Task<ReturnModel<JobTemplateModel>> UpdateJobTemplateAsync(JobTemplateUpdateModel jobTemplate)
        {
            try
            {
                if (jobTemplate == null)
                {
                    return log.ErrorAndReturnModel<JobTemplateModel>($"Unable to update JobTemplate", new ReturnErrorItem("", "JobTemplate information is not valid"));
                }

                var existingJobTemplate = await reportRepository.GetJobTemplateViewFromAnyByIdAsync(jobTemplate.JobTemplateId);
                if (existingJobTemplate == null)
                {
                    return log.ErrorAndReturnModel<JobTemplateModel>($"Unable to find the jobTemplate", new { jobTemplate.JobTemplateId });
                }

                if (!await currentSessionService.IsCurrentUserASuperAdminAsync())
                {
                    return log.ErrorAndReturnModel<JobTemplateModel>($"You dont have access to creste Job Template");
                }

                jobTemplate.CreatedByUserId = existingJobTemplate.CreatedByUserId;
                jobTemplate.CreatedDate = existingJobTemplate.CreatedDate;
                jobTemplate.IsDeleted = existingJobTemplate.IsDeleted;

                var errorList = IsJobTemplateModelValidForUpdateAsync(jobTemplate);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<JobTemplateModel>($"Unable to update jobTemplate", errorList);
                }

                var item = objectMapperService.JobTemplateUpdateModelToJobTemplate(jobTemplate);

                var jobTemplateCategoryItem = await reportRepository.GetJobTemplateCategoryViewByIdAsync(jobTemplate.JobTemplateCategoryId);
                if (jobTemplateCategoryItem == null)
                {
                    return log.ErrorAndReturnModel<JobTemplateModel>($"Unable to Get the JobTemplateCategory Information", new { jobTemplate.JobTemplateCategoryId });
                }

                item.JobTemplateCategoryId = jobTemplateCategoryItem.JobTemplateCategoryId;
 
                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await reportRepository.UpdateJobTemplateAsync(item, await currentSessionService.GetCurrentUserIdAsync());
                    if (result == false)
                    {
                        throw new RollbackException($"Unable to update jobTemplate");
                    }

                    foreach (var param in jobTemplate.NewParameters)
                    {
                        param.JobTemplateId = item.JobTemplateId;
                        var paramresult = await CreateJobTemplateParameterAsync(param);
                        if (!paramresult.IsSuccess)
                            throw new RollbackException(paramresult);
                    }

                    await trx.CompleteAsync();
                }

                // await auditService.JobTemplateUpdatedAsync(jobTemplate.JobTemplateId);

                var view = await reportRepository.GetJobTemplateViewByIdAsync(jobTemplate.JobTemplateId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<JobTemplateModel>($"Unable to find the jobTemplate", new { jobTemplate.JobTemplateId });
                }

                return this.ConvertToItem(view, objectMapperService.JobTemplateViewToJobTemplateModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<JobTemplateModel>($"Unexpected error when updating JobTemplate {jobTemplate?.JobTemplateId}", ex);
            }
        }

        public async Task<ReturnModel<bool>> MarkJobTemplateAsNotDeletedByIdAsync(long jobTemplateId)
        {
            try
            {
                var jobTemplate = await reportRepository.GetJobTemplateViewFromAnyByIdAsync(jobTemplateId);
                if (jobTemplate == null)
                {
                    return log.ErrorAndReturnModel<bool>($"Unable to find the jobTemplate", new { jobTemplateId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanManageJobTemplatePermissionParamsAsync(jobTemplate.CompanyId, jobTemplate.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<bool>(errorStr);
                //}

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await reportRepository.MarkJobTemplateAsNotDeletedByIdAsync(jobTemplateId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                // await auditService.JobTemplateDeletedAsync(jobTemplate.JobTemplateId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking jobTemplate as not deleted {jobTemplateId}", ex);
            }
        }

        public async Task<ReturnModel<bool>> MarkJobTemplateAsDeletedByIdAsync(long jobTemplateId)
        {
            try
            {
                var jobTemplate = await reportRepository.GetJobTemplateViewFromAnyByIdAsync(jobTemplateId);
                if (jobTemplate == null)
                {
                    return log.ErrorAndReturnModel<bool>($"Unable to find the jobTemplate", new { jobTemplateId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanManageJobTemplatePermissionParamsAsync(jobTemplate.CompanyId, jobTemplate.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<bool>(errorStr);
                //}

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await reportRepository.MarkJobTemplateAsDeletedByIdAsync(jobTemplateId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                // await auditService.JobTemplateDeletedAsync(jobTemplate.JobTemplateId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking jobTemplate as deleted {jobTemplateId}", ex);
            }
        }

        public Task<List<ReturnErrorItem>> IsJobTemplateModelValidForCreateAsync(JobTemplateUpdateModel jobTemplateModel)
        {
            return IsJobTemplateModelValidForAllAsync(true, jobTemplateModel);
        }

        public Task<List<ReturnErrorItem>> IsJobTemplateModelValidForUpdateAsync(JobTemplateUpdateModel jobTemplateModel)
        {
            return IsJobTemplateModelValidForAllAsync(false, jobTemplateModel);
        }

        public async Task<List<ReturnErrorItem>> IsJobTemplateModelValidForAllAsync(bool isCreate, JobTemplateUpdateModel jobTemplateModel)
        {
            var result = new List<ReturnErrorItem>();

            if (!await currentSessionService.IsCurrentUserASuperAdminAsync())
            {
                result.Add(new ReturnErrorItem("", "You don't have access to create Job Template"));
                return result;
            }

            if (isCreate)
            {
                if (jobTemplateModel.JobTemplateId > 0)
                {
                    result.Add(new ReturnErrorItem("", "JobTemplate Identifier is not valid"));
                }
            }
            else
            {
                if (jobTemplateModel.JobTemplateId <= 0)
                {
                    result.Add(new ReturnErrorItem("", "JobTemplate Unique Identifier is not valid"));
                }
            }

            if (jobTemplateModel.Description != null && jobTemplateModel.Description.Length > JobTemplateModel.COLUMN_SIZE_Description)
            {
                result.Add(new ReturnErrorItem("Description", $"Description size should not exceed {JobTemplateModel.COLUMN_SIZE_Description}"));
            }

            if (jobTemplateModel.Description.IsTrimmedStringNullOrEmpty())
            {
                result.Add(new ReturnErrorItem("Description", "Description is required "));
            }

            if (!ValidationUtils.IsLongValid(jobTemplateModel.JobTemplateCategoryId))
            {
                result.Add(new ReturnErrorItem("JobTemplateCategoryId", "JobTemplateCategory cannot be empty"));
            }

            if (jobTemplateModel.ExecutableName != null && jobTemplateModel.ExecutableName.Length > JobTemplateModel.COLUMN_SIZE_ExecutableName)
            {
                result.Add(new ReturnErrorItem("ExecutableName", $"ExecutableName size should not exceed {JobTemplateModel.COLUMN_SIZE_ExecutableName}"));
            }

            if (jobTemplateModel.ExecutableName.IsTrimmedStringNullOrEmpty())
            {
                result.Add(new ReturnErrorItem("ExecutableName", "ExecutableName is required "));
            }

            if (jobTemplateModel.CustomExportName != null && jobTemplateModel.CustomExportName.Length > JobTemplateModel.COLUMN_SIZE_CustomExportName)
            {
                result.Add(new ReturnErrorItem("CustomExportName", $"CustomExportName size should not exceed {JobTemplateModel.COLUMN_SIZE_CustomExportName}"));
            }

            if (jobTemplateModel.TemplateName != null && jobTemplateModel.TemplateName.Length > JobTemplateModel.COLUMN_SIZE_TemplateName)
            {
                result.Add(new ReturnErrorItem("TemplateName", $"TemplateName size should not exceed {JobTemplateModel.COLUMN_SIZE_TemplateName}"));
            }

            if (jobTemplateModel.TemplateName.IsTrimmedStringNullOrEmpty())
            {
                result.Add(new ReturnErrorItem("TemplateName", "TemplateName is required "));
            }

            if (jobTemplateModel.TaskIdentifierName != null && jobTemplateModel.TaskIdentifierName.Length > JobTemplateModel.COLUMN_SIZE_TaskIdentifierName)
            {
                result.Add(new ReturnErrorItem("TaskIdentifierName", $"TaskIdentifierName size should not exceed {JobTemplateModel.COLUMN_SIZE_TaskIdentifierName}"));
            }

            if (jobTemplateModel.TaskIdentifierName.IsTrimmedStringNullOrEmpty())
            {
                result.Add(new ReturnErrorItem("TaskIdentifierName", "TaskIdentifierName is required "));
            }

            if (jobTemplateModel.ReportTemplateFile != null && jobTemplateModel.ReportTemplateFile.Length > JobTemplateModel.COLUMN_SIZE_ReportTemplateFile)
            {
                result.Add(new ReturnErrorItem("ReportTemplateFile", $"ReportTemplateFile size should not exceed {JobTemplateModel.COLUMN_SIZE_ReportTemplateFile}"));
            }

            if (jobTemplateModel.ReportTemplateFile.IsTrimmedStringNullOrEmpty())
            {
                result.Add(new ReturnErrorItem("ReportTemplateFile", "ReportTemplateFile is required "));
            }

            if (jobTemplateModel.DefaultOutputFileName != null && jobTemplateModel.DefaultOutputFileName.Length > JobTemplateModel.COLUMN_SIZE_DefaultOutputFileName)
            {
                result.Add(new ReturnErrorItem("DefaultOutputFileName", $"DefaultOutputFileName size should not exceed {JobTemplateModel.COLUMN_SIZE_DefaultOutputFileName}"));
            }

            if (jobTemplateModel.DefaultOutputFileName.IsTrimmedStringNullOrEmpty())
            {
                result.Add(new ReturnErrorItem("DefaultOutputFileName", "DefaultOutputFileName is required "));
            }

            return result.Count > 0 ? result : null;
        }
        #endregion

        #region JobTemplateCategory

        public async Task<ReturnListModel<JobTemplateCategoryModel>> GetJobTemplateCategorysAsync(JobTemplateCategorySearchCriteria search)
        {
            try
            {
                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanViewJobTemplateCategoryPermissionParamsAsync();

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnListModel<JobTemplateCategoryModel>(errorStr);
                //}

                //if (canAssign)
                //{
                //    search.CompanyId = await currentSessionService.GetCurrentCompanyIdAsync();
                //    //search.TenantEntityId = await currentSessionService.GetCurrentTenantEntityIdAsNullableAsync();
                //}

                return this.ConvertToList(await reportRepository.GetJobTemplateCategorysAsync(search), objectMapperService.JobTemplateCategoryViewListToJobTemplateCategoryModelList);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<JobTemplateCategoryModel>($"Unexpected error when getting JobTemplateCategorys", ex);
            }
        }

        public Task<ReturnModel<JobTemplateCategoryModel>> GetJobTemplateCategoryByIdForReadAsync(long jobTemplateCategoryId)
        {
            return GetJobTemplateCategoryByIdInternalAsync(forWrite: false, jobTemplateCategoryId);
        }

        public Task<ReturnModel<JobTemplateCategoryModel>> GetJobTemplateCategoryByIdForWriteAsync(long jobTemplateCategoryId)
        {
            return GetJobTemplateCategoryByIdInternalAsync(forWrite: true, jobTemplateCategoryId);
        }

        public async Task<ReturnModel<JobTemplateCategoryModel>> GetJobTemplateCategoryByIdInternalAsync(bool forWrite, long jobTemplateCategoryId)
        {
            try
            {
                var item = await reportRepository.GetJobTemplateCategoryViewByIdAsync(jobTemplateCategoryId);
                if (item == null)
                {
                    return log.ErrorAndReturnModel<JobTemplateCategoryModel>($"Unable to find the jobTemplateCategory", new { jobTemplateCategoryId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanViewJobTemplateCategoryPermissionParamsAsync(item.CompanyId, item.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<JobTemplateCategoryModel>(errorStr);
                //}

                return this.ConvertToItem(item, objectMapperService.JobTemplateCategoryViewToJobTemplateCategoryModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<JobTemplateCategoryModel>($"Unexpected error when getting jobTemplateCategory", new { jobTemplateCategoryId }, ex);
            }
        }

        public async Task<ReturnModel<JobTemplateCategoryModel>> CreateJobTemplateCategoryAsync(JobTemplateCategoryCreateModel jobTemplateCategory)
        {
            try
            {
                if (jobTemplateCategory == null)
                {
                    return log.ErrorAndReturnModel<JobTemplateCategoryModel>($"Unable to create JobTemplateCategory", new ReturnErrorItem("", "JobTemplateCategory information is not valid"));
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.CanCreateJobTemplateCategoryAsync();

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<JobTemplateCategoryModel>(errorStr);
                //}

                //if (canAssign)
                //{
                //    jobTemplateCategory.CompanyId = await currentSessionService.GetCurrentTenantIdAsync();
                //    jobTemplateCategory.TenantEntityId = await currentSessionService.GetCurrentTenantEntityIdAsNullableAsync();
                //}

                var errorList = IsJobTemplateCategoryModelValidForCreateAsync(jobTemplateCategory);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<JobTemplateCategoryModel>($"Unable to create jobTemplateCategory", errorList);
                }

                jobTemplateCategory.IsDeleted = false;

                var item = objectMapperService.JobTemplateCategoryCreateModelToJobTemplateCategory(jobTemplateCategory);

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    item = await reportRepository.CreateJobTemplateCategoryAsync(item, await currentSessionService.GetCurrentUserIdAsync());
                    if (item == null)
                    {
                        throw new Exception($"Unable to create jobTemplateCategory");
                    }

                    await trx.CompleteAsync();
                }

                // await auditService.JobTemplateCategoryCreatedAsync(item.JobTemplateCategoryId);

                var view = await reportRepository.GetJobTemplateCategoryViewByIdAsync(item.JobTemplateCategoryId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<JobTemplateCategoryModel>($"Unable to find the jobTemplateCategory", new { item.JobTemplateCategoryId });
                }

                return this.ConvertToItem(view, objectMapperService.JobTemplateCategoryViewToJobTemplateCategoryModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModelWithRollbackExceptionHandler<JobTemplateCategoryModel>($"Unexpected error when creating JobTemplateCategory {jobTemplateCategory?.JobTemplateCategoryId}", ex);
            }
        }

        public async Task<ReturnModel<JobTemplateCategoryModel>> UpdateJobTemplateCategoryAsync(JobTemplateCategoryUpdateModel jobTemplateCategory)
        {
            try
            {
                if (jobTemplateCategory == null)
                {
                    return log.ErrorAndReturnModel<JobTemplateCategoryModel>($"Unable to update JobTemplateCategory", new ReturnErrorItem("", "JobTemplateCategory information is not valid"));
                }

                var existingJobTemplateCategory = await reportRepository.GetJobTemplateCategoryViewFromAnyByIdAsync(jobTemplateCategory.JobTemplateCategoryId);
                if (existingJobTemplateCategory == null)
                {
                    return log.ErrorAndReturnModel<JobTemplateCategoryModel>($"Unable to find the jobTemplateCategory", new { jobTemplateCategory.JobTemplateCategoryId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanManageJobTemplateCategoryPermissionParamsAsync(existingJobTemplateCategory.CompanyId, existingJobTemplateCategory.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<JobTemplateCategoryModel>(errorStr);
                //}

                //if (canAssign)
                //{
                //    jobTemplateCategory.CompanyId = await currentSessionService.GetCurrentTenantIdAsync();
                //    //jobTemplateCategory.TenantEntityId = await currentSessionService.GetCurrentTenantEntityIdAsNullableAsync();
                //}

                jobTemplateCategory.CreatedByUserId = existingJobTemplateCategory.CreatedByUserId;
                jobTemplateCategory.CreatedDate = existingJobTemplateCategory.CreatedDate;
                jobTemplateCategory.IsDeleted = existingJobTemplateCategory.IsDeleted;

                var errorList = IsJobTemplateCategoryModelValidForUpdateAsync(jobTemplateCategory);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<JobTemplateCategoryModel>($"Unable to update jobTemplateCategory", errorList);
                }

                var item = objectMapperService.JobTemplateCategoryUpdateModelToJobTemplateCategory(jobTemplateCategory);

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await reportRepository.UpdateJobTemplateCategoryAsync(item, await currentSessionService.GetCurrentUserIdAsync());
                    if (result == false)
                    {
                        throw new RollbackException($"Unable to update jobTemplateCategory");
                    }

                    await trx.CompleteAsync();
                }

                // await auditService.JobTemplateCategoryUpdatedAsync(jobTemplateCategory.JobTemplateCategoryId);

                var view = await reportRepository.GetJobTemplateCategoryViewByIdAsync(jobTemplateCategory.JobTemplateCategoryId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<JobTemplateCategoryModel>($"Unable to find the jobTemplateCategory", new { jobTemplateCategory.JobTemplateCategoryId });
                }

                return this.ConvertToItem(view, objectMapperService.JobTemplateCategoryViewToJobTemplateCategoryModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<JobTemplateCategoryModel>($"Unexpected error when updating JobTemplateCategory {jobTemplateCategory?.JobTemplateCategoryId}", ex);
            }
        }

        public async Task<ReturnModel<bool>> MarkJobTemplateCategoryAsNotDeletedByIdAsync(long jobTemplateCategoryId)
        {
            try
            {
                var jobTemplateCategory = await reportRepository.GetJobTemplateCategoryViewFromAnyByIdAsync(jobTemplateCategoryId);
                if (jobTemplateCategory == null)
                {
                    return log.ErrorAndReturnModel<bool>($"Unable to find the jobTemplateCategory", new { jobTemplateCategoryId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanManageJobTemplateCategoryPermissionParamsAsync(jobTemplateCategory.CompanyId, jobTemplateCategory.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<bool>(errorStr);
                //}

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await reportRepository.MarkJobTemplateCategoryAsNotDeletedByIdAsync(jobTemplateCategoryId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                // await auditService.JobTemplateCategoryDeletedAsync(jobTemplateCategory.JobTemplateCategoryId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking jobTemplateCategory as not deleted {jobTemplateCategoryId}", ex);
            }
        }

        public async Task<ReturnModel<bool>> MarkJobTemplateCategoryAsDeletedByIdAsync(long jobTemplateCategoryId)
        {
            try
            {
                var jobTemplateCategory = await reportRepository.GetJobTemplateCategoryViewFromAnyByIdAsync(jobTemplateCategoryId);
                if (jobTemplateCategory == null)
                {
                    return log.ErrorAndReturnModel<bool>($"Unable to find the jobTemplateCategory", new { jobTemplateCategoryId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanManageJobTemplateCategoryPermissionParamsAsync(jobTemplateCategory.CompanyId, jobTemplateCategory.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<bool>(errorStr);
                //}

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await reportRepository.MarkJobTemplateCategoryAsDeletedByIdAsync(jobTemplateCategoryId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                // await auditService.JobTemplateCategoryDeletedAsync(jobTemplateCategory.JobTemplateCategoryId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking jobTemplateCategory as deleted {jobTemplateCategoryId}", ex);
            }
        }

        public async Task<ReturnModel<bool>> HardDeleteJobTemplateCategoryByAsync(long jobTemplateCategoryId)
        {
            try
            {
                var jobTemplateCategory = await reportRepository.GetJobTemplateCategoryViewFromAnyByIdAsync(jobTemplateCategoryId);
                if (jobTemplateCategory == null)
                {
                    return log.ErrorAndReturnModel<bool>($"Unable to find the jobTemplateCategory", new { jobTemplateCategoryId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanManageJobTemplateCategoryPermissionParamsAsync(jobTemplateCategory.CompanyId, jobTemplateCategory.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<bool>(errorStr);
                //}

                bool result;

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await reportRepository.HardDeleteJobTemplateCategoryByIdAsync(jobTemplateCategoryId);
                    await trx.CompleteAsync();
                }

                // await auditService.HardDeleteJobTemplateCategoryAsync(jobTemplateCategory.JobTemplateCategoryId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when deleting jobTemplateCategory {jobTemplateCategoryId}", ex);
            }
        }

        public List<ReturnErrorItem> IsJobTemplateCategoryModelValidForCreateAsync(JobTemplateCategoryUpdateModel jobTemplateCategoryModel)
        {
            return IsJobTemplateCategoryModelValidForAllAsync(true, jobTemplateCategoryModel);
        }

        public List<ReturnErrorItem> IsJobTemplateCategoryModelValidForUpdateAsync(JobTemplateCategoryUpdateModel jobTemplateCategoryModel)
        {
            return IsJobTemplateCategoryModelValidForAllAsync(false, jobTemplateCategoryModel);
        }

        public List<ReturnErrorItem> IsJobTemplateCategoryModelValidForAllAsync(bool isCreate, JobTemplateCategoryUpdateModel jobTemplateCategoryModel)
        {
            var result = new List<ReturnErrorItem>();

            if (isCreate)
            {
                if (jobTemplateCategoryModel.JobTemplateCategoryId > 0)
                {
                    result.Add(new ReturnErrorItem("", "JobTemplateCategory Identifier is not valid"));
                }
            }
            else
            {
                if (jobTemplateCategoryModel.JobTemplateCategoryId <= 0)
                {
                    result.Add(new ReturnErrorItem("", "JobTemplateCategory Unique Identifier is not valid"));
                }
            }

            if (jobTemplateCategoryModel.Name != null && jobTemplateCategoryModel.Name.Length > JobTemplateCategoryModel.COLUMN_SIZE_Name)
            {
                result.Add(new ReturnErrorItem("Name", $"Name size should not exceed {JobTemplateCategoryModel.COLUMN_SIZE_Name}"));
            }

            if (jobTemplateCategoryModel.Name.IsTrimmedStringNullOrEmpty())
            {
                result.Add(new ReturnErrorItem("Name", "Name is required "));
            }

            return result.Count > 0 ? result : null;
        }
        #endregion

        #region JobTemplateParameter

        public async Task<ReturnListModel<JobTemplateParameterModel>> GetJobTemplateParametersAsync(JobTemplateParameterSearchCriteria search)
        {
            try
            {
                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanViewJobTemplateParameterPermissionParamsAsync();

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnListModel<JobTemplateParameterModel>(errorStr);
                //}

                //if (canAssign)
                //{
                //    search.CompanyId = await currentSessionService.GetCurrentCompanyIdAsync();
                //    //search.TenantEntityId = await currentSessionService.GetCurrentTenantEntityIdAsNullableAsync();
                //}

                return this.ConvertToList(await reportRepository.GetJobTemplateParametersAsync(search), objectMapperService.JobTemplateParameterViewListToJobTemplateParameterModelList);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<JobTemplateParameterModel>($"Unexpected error when getting JobTemplateParameters", ex);
            }
        }

        public Task<ReturnModel<JobTemplateParameterModel>> GetJobTemplateParameterByIdForReadAsync(long jobTemplateParameterId)
        {
            return GetJobTemplateParameterByIdInternalAsync(forWrite: false, jobTemplateParameterId);
        }

        public Task<ReturnModel<JobTemplateParameterModel>> GetJobTemplateParameterByIdForWriteAsync(long jobTemplateParameterId)
        {
            return GetJobTemplateParameterByIdInternalAsync(forWrite: true, jobTemplateParameterId);
        }

        public async Task<ReturnModel<JobTemplateParameterModel>> GetJobTemplateParameterByIdInternalAsync(bool forWrite, long jobTemplateParameterId)
        {
            try
            {
                var item = await reportRepository.GetJobTemplateParameterViewByIdAsync(jobTemplateParameterId);
                if (item == null)
                {
                    return log.ErrorAndReturnModel<JobTemplateParameterModel>($"Unable to find the jobTemplateParameter", new { jobTemplateParameterId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanViewJobTemplateParameterPermissionParamsAsync(item.CompanyId, item.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<JobTemplateParameterModel>(errorStr);
                //}

                return this.ConvertToItem(item, objectMapperService.JobTemplateParameterViewToJobTemplateParameterModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<JobTemplateParameterModel>($"Unexpected error when getting jobTemplateParameter", new { jobTemplateParameterId }, ex);
            }
        }

        public async Task<ReturnModel<JobTemplateParameterModel>> CreateJobTemplateParameterAsync(JobTemplateParameterCreateModel jobTemplateParameter)
        {
            try
            {
                if (jobTemplateParameter == null)
                {
                    return log.ErrorAndReturnModel<JobTemplateParameterModel>($"Unable to create JobTemplateParameter", new ReturnErrorItem("", "JobTemplateParameter information is not valid"));
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.CanCreateJobTemplateParameterAsync();

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<JobTemplateParameterModel>(errorStr);
                //}

                //if (canAssign)
                //{
                //    jobTemplateParameter.CompanyId = await currentSessionService.GetCurrentTenantIdAsync();
                //    jobTemplateParameter.TenantEntityId = await currentSessionService.GetCurrentTenantEntityIdAsNullableAsync();
                //}

                var errorList = IsJobTemplateParameterModelValidForCreateAsync(jobTemplateParameter);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<JobTemplateParameterModel>($"Unable to create jobTemplateParameter", errorList);
                }

                jobTemplateParameter.IsDeleted = false;

                var item = objectMapperService.JobTemplateParameterCreateModelToJobTemplateParameter(jobTemplateParameter);

                var jobTemplateItem = await reportRepository.GetJobTemplateViewByIdAsync(jobTemplateParameter.JobTemplateId);
                if (jobTemplateItem == null)
                {
                    return log.ErrorAndReturnModel<JobTemplateParameterModel>($"Unable to Get the JobTemplate Information", new { jobTemplateParameter.JobTemplateId });
                }

                item.JobTemplateId = jobTemplateItem.JobTemplateId;

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    item = await reportRepository.CreateJobTemplateParameterAsync(item, await currentSessionService.GetCurrentUserIdAsync());
                    if (item == null)
                    {
                        throw new Exception($"Unable to create jobTemplateParameter");
                    }

                    await trx.CompleteAsync();
                }

                // await auditService.JobTemplateParameterCreatedAsync(item.JobTemplateParameterId);

                var view = await reportRepository.GetJobTemplateParameterViewByIdAsync(item.JobTemplateParameterId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<JobTemplateParameterModel>($"Unable to find the jobTemplateParameter", new { item.JobTemplateParameterId });
                }

                return this.ConvertToItem(view, objectMapperService.JobTemplateParameterViewToJobTemplateParameterModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModelWithRollbackExceptionHandler<JobTemplateParameterModel>($"Unexpected error when creating JobTemplateParameter {jobTemplateParameter?.JobTemplateParameterId}", ex);
            }
        }

        public async Task<ReturnModel<JobTemplateParameterModel>> UpdateJobTemplateParameterAsync(JobTemplateParameterUpdateModel jobTemplateParameter)
        {
            try
            {
                if (jobTemplateParameter == null)
                {
                    return log.ErrorAndReturnModel<JobTemplateParameterModel>($"Unable to update JobTemplateParameter", new ReturnErrorItem("", "JobTemplateParameter information is not valid"));
                }

                var existingJobTemplateParameter = await reportRepository.GetJobTemplateParameterViewFromAnyByIdAsync(jobTemplateParameter.JobTemplateParameterId);
                if (existingJobTemplateParameter == null)
                {
                    return log.ErrorAndReturnModel<JobTemplateParameterModel>($"Unable to find the jobTemplateParameter", new { jobTemplateParameter.JobTemplateParameterId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanManageJobTemplateParameterPermissionParamsAsync(existingJobTemplateParameter.CompanyId, existingJobTemplateParameter.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<JobTemplateParameterModel>(errorStr);
                //}

                //if (canAssign)
                //{
                //    jobTemplateParameter.CompanyId = await currentSessionService.GetCurrentTenantIdAsync();
                //    //jobTemplateParameter.TenantEntityId = await currentSessionService.GetCurrentTenantEntityIdAsNullableAsync();
                //}

                jobTemplateParameter.CreatedByUserId = existingJobTemplateParameter.CreatedByUserId;
                jobTemplateParameter.CreatedDate = existingJobTemplateParameter.CreatedDate;
                jobTemplateParameter.IsDeleted = existingJobTemplateParameter.IsDeleted;
                //jobTemplateParameter.CompanyId = existingJobTemplateParameter.CompanyId;

                var errorList = IsJobTemplateParameterModelValidForUpdateAsync(jobTemplateParameter);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<JobTemplateParameterModel>($"Unable to update jobTemplateParameter", errorList);
                }

                var item = objectMapperService.JobTemplateParameterUpdateModelToJobTemplateParameter(jobTemplateParameter);

                var jobTemplateItem = await reportRepository.GetJobTemplateViewByIdAsync(jobTemplateParameter.JobTemplateId);
                if (jobTemplateItem == null)
                {
                    return log.ErrorAndReturnModel<JobTemplateParameterModel>($"Unable to Get the JobTemplate Information", new { jobTemplateParameter.JobTemplateId });
                }

                item.JobTemplateId = jobTemplateItem.JobTemplateId;

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await reportRepository.UpdateJobTemplateParameterAsync(item, await currentSessionService.GetCurrentUserIdAsync());
                    if (result == false)
                    {
                        throw new RollbackException($"Unable to update jobTemplateParameter");
                    }

                    await trx.CompleteAsync();
                }

                // await auditService.JobTemplateParameterUpdatedAsync(jobTemplateParameter.JobTemplateParameterId);

                var view = await reportRepository.GetJobTemplateParameterViewByIdAsync(jobTemplateParameter.JobTemplateParameterId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<JobTemplateParameterModel>($"Unable to find the jobTemplateParameter", new { jobTemplateParameter.JobTemplateParameterId });
                }

                return this.ConvertToItem(view, objectMapperService.JobTemplateParameterViewToJobTemplateParameterModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<JobTemplateParameterModel>($"Unexpected error when updating JobTemplateParameter {jobTemplateParameter?.JobTemplateParameterId}", ex);
            }
        }

        public async Task<ReturnModel<bool>> MarkJobTemplateParameterAsNotDeletedByIdAsync(long jobTemplateParameterId)
        {
            try
            {
                var jobTemplateParameter = await reportRepository.GetJobTemplateParameterViewFromAnyByIdAsync(jobTemplateParameterId);
                if (jobTemplateParameter == null)
                {
                    return log.ErrorAndReturnModel<bool>($"Unable to find the jobTemplateParameter", new { jobTemplateParameterId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanManageJobTemplateParameterPermissionParamsAsync(jobTemplateParameter.CompanyId, jobTemplateParameter.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<bool>(errorStr);
                //}

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await reportRepository.MarkJobTemplateParameterAsNotDeletedByIdAsync(jobTemplateParameterId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                // await auditService.JobTemplateParameterDeletedAsync(jobTemplateParameter.JobTemplateParameterId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking jobTemplateParameter as not deleted {jobTemplateParameterId}", ex);
            }
        }

        public async Task<ReturnModel<bool>> MarkJobTemplateParameterAsDeletedByIdAsync(long jobTemplateParameterId)
        {
            try
            {
                var jobTemplateParameter = await reportRepository.GetJobTemplateParameterViewFromAnyByIdAsync(jobTemplateParameterId);
                if (jobTemplateParameter == null)
                {
                    return log.ErrorAndReturnModel<bool>($"Unable to find the jobTemplateParameter", new { jobTemplateParameterId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanManageJobTemplateParameterPermissionParamsAsync(jobTemplateParameter.CompanyId, jobTemplateParameter.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<bool>(errorStr);
                //}

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await reportRepository.MarkJobTemplateParameterAsDeletedByIdAsync(jobTemplateParameterId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                // await auditService.JobTemplateParameterDeletedAsync(jobTemplateParameter.JobTemplateParameterId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking jobTemplateParameter as deleted {jobTemplateParameterId}", ex);
            }
        }

        public async Task<ReturnModel<bool>> HardDeleteJobTemplateParameterByAsync(long jobTemplateParameterId)
        {
            try
            {
                var jobTemplateParameter = await reportRepository.GetJobTemplateParameterViewFromAnyByIdAsync(jobTemplateParameterId);
                if (jobTemplateParameter == null)
                {
                    return log.ErrorAndReturnModel<bool>($"Unable to find the jobTemplateParameter", new { jobTemplateParameterId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanManageJobTemplateParameterPermissionParamsAsync(jobTemplateParameter.CompanyId, jobTemplateParameter.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<bool>(errorStr);
                //}

                bool result;

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await reportRepository.HardDeleteJobTemplateParameterByIdAsync(jobTemplateParameterId);
                    await trx.CompleteAsync();
                }

                // await auditService.HardDeleteJobTemplateParameterAsync(jobTemplateParameter.JobTemplateParameterId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when deleting jobTemplateParameter {jobTemplateParameterId}", ex);
            }
        }

        public List<ReturnErrorItem> IsJobTemplateParameterModelValidForCreateAsync(JobTemplateParameterUpdateModel jobTemplateParameterModel)
        {
            return IsJobTemplateParameterModelValidForAllAsync(true, jobTemplateParameterModel);
        }

        public List<ReturnErrorItem> IsJobTemplateParameterModelValidForUpdateAsync(JobTemplateParameterUpdateModel jobTemplateParameterModel)
        {
            return IsJobTemplateParameterModelValidForAllAsync(false, jobTemplateParameterModel);
        }

        public List<ReturnErrorItem> IsJobTemplateParameterModelValidForAllAsync(bool isCreate, JobTemplateParameterUpdateModel jobTemplateParameterModel)
        {
            var result = new List<ReturnErrorItem>();

            if (isCreate)
            {
                if (jobTemplateParameterModel.JobTemplateParameterId > 0)
                {
                    result.Add(new ReturnErrorItem("", "JobTemplateParameter Identifier is not valid"));
                }
            }
            else
            {
                if (jobTemplateParameterModel.JobTemplateParameterId <= 0)
                {
                    result.Add(new ReturnErrorItem("", "JobTemplateParameter Unique Identifier is not valid"));
                }
            }

            if (!ValidationUtils.IsGuidValid(jobTemplateParameterModel.JobTemplateId))
            {
                result.Add(new ReturnErrorItem("JobTemplateId", "JobTemplate cannot be empty"));
            }

            if (jobTemplateParameterModel.Name != null && jobTemplateParameterModel.Name.Length > JobTemplateParameterModel.COLUMN_SIZE_Name)
            {
                result.Add(new ReturnErrorItem("Name", $"Name size should not exceed {JobTemplateParameterModel.COLUMN_SIZE_Name}"));
            }

            if (jobTemplateParameterModel.Name.IsTrimmedStringNullOrEmpty())
            {
                result.Add(new ReturnErrorItem("Name", "Name is required "));
            }

            if (jobTemplateParameterModel.Value != null && jobTemplateParameterModel.Value.Length > JobTemplateParameterModel.COLUMN_SIZE_Value)
            {
                result.Add(new ReturnErrorItem("Value", $"Value size should not exceed {JobTemplateParameterModel.COLUMN_SIZE_Value}"));
            }

            if (jobTemplateParameterModel.Value.IsTrimmedStringNullOrEmpty())
            {
                result.Add(new ReturnErrorItem("Value", "Value is required "));
            }

            if (jobTemplateParameterModel.DefaultValue != null && jobTemplateParameterModel.DefaultValue.Length > JobTemplateParameterModel.COLUMN_SIZE_DefaultValue)
            {
                result.Add(new ReturnErrorItem("DefaultValue", $"DefaultValue size should not exceed {JobTemplateParameterModel.COLUMN_SIZE_DefaultValue}"));
            }

            if (jobTemplateParameterModel.DefaultValue.IsTrimmedStringNullOrEmpty())
            {
                result.Add(new ReturnErrorItem("DefaultValue", "DefaultValue is required "));
            }

            if (jobTemplateParameterModel.PredefinedFieldControlTypeId == 0)
            {
                result.Add(new ReturnErrorItem("PredefinedFieldControlTypeId", "Field Control Type cannot be empty"));
            }
            else if (!ValidationUtils.IsEnumValid<PredefinedFieldControlTypes, long>(jobTemplateParameterModel.PredefinedFieldControlTypeId))
            {
                result.Add(new ReturnErrorItem("PredefinedFieldControlTypeId", "Field Control Type is not valid"));
            }

            if (jobTemplateParameterModel.PredefinedFieldTypeId == 0)
            {
                result.Add(new ReturnErrorItem("PredefinedFieldTypeId", "Field Type cannot be empty"));
            }
            else if (!ValidationUtils.IsEnumValid<PredefinedFieldTypes, long>(jobTemplateParameterModel.PredefinedFieldTypeId))
            {
                result.Add(new ReturnErrorItem("PredefinedFieldTypeId", "Field Type is not valid"));
            }

            if (jobTemplateParameterModel.PredefinedFieldPopulatorTypeId == 0)
            {
                result.Add(new ReturnErrorItem("PredefinedFieldPopulatorTypeId", "Populator Type cannot be empty"));
            }
            else if (!ValidationUtils.IsEnumValid<PredefinedFieldPopulatorTypes, long>(jobTemplateParameterModel.PredefinedFieldPopulatorTypeId))
            {
                result.Add(new ReturnErrorItem("PredefinedFieldPopulatorTypeId", "Populator Type is not valid"));
            }

            if (jobTemplateParameterModel.Caption != null && jobTemplateParameterModel.Caption.Length > JobTemplateParameterModel.COLUMN_SIZE_Caption)
            {
                result.Add(new ReturnErrorItem("Caption", $"Caption size should not exceed {JobTemplateParameterModel.COLUMN_SIZE_Caption}"));
            }

            if (jobTemplateParameterModel.PopulatorSource != null && jobTemplateParameterModel.PopulatorSource.Length > JobTemplateParameterModel.COLUMN_SIZE_PopulatorSource)
            {
                result.Add(new ReturnErrorItem("PopulatorSource", $"PopulatorSource size should not exceed {JobTemplateParameterModel.COLUMN_SIZE_PopulatorSource}"));
            }

            return result.Count > 0 ? result : null;
        }
        #endregion

        #region SavedJob

        public async Task<ReturnListModel<SavedJobModel>> GetSavedJobsAsync(SavedJobSearchCriteria search)
        {
            try
            {
                return this.ConvertToList(await reportRepository.GetSavedJobsAsync(search), objectMapperService.SavedJobViewListToSavedJobModelList);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<SavedJobModel>($"Unexpected error when getting SavedJobs", ex);
            }
        }

        public Task<ReturnModel<SavedJobModel>> GetSavedJobByIdForReadAsync(long savedJobId)
        {
            return GetSavedJobByIdInternalAsync(forWrite: false, savedJobId);
        }

        public Task<ReturnModel<SavedJobModel>> GetSavedJobByIdForWriteAsync(long savedJobId)
        {
            return GetSavedJobByIdInternalAsync(forWrite: true, savedJobId);
        }

        public async Task<ReturnModel<SavedJobModel>> GetSavedJobByIdInternalAsync(bool forWrite, long savedJobId)
        {
            try
            {
                var item = await reportRepository.GetSavedJobViewByIdAsync(savedJobId);
                if (item == null)
                {
                    return log.ErrorAndReturnModel<SavedJobModel>($"Unable to find the savedJob", new { savedJobId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanViewSavedJobPermissionParamsAsync(item.CompanyId, item.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<SavedJobModel>(errorStr);
                //}

                var result = this.ConvertToItem(item, objectMapperService.SavedJobViewToSavedJobModel);
                if (result.IsSuccess)
                {
                    result.Model.SavedNotificationEmails = objectMapperService.SavedJobNotificationEmailViewListToSavedJobNotificationEmailModelList((await reportRepository.GetSavedJobNotificationEmailsAsync(new SavedJobNotificationEmailSearchCriteria { SavedJobId = savedJobId, PageSize = int.MaxValue })).Result);
                }

                return result;
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<SavedJobModel>($"Unexpected error when getting savedJob", new { savedJobId }, ex);
            }
        }

        public async Task<ReturnModel<JobModel>> RunSavedJobAsync(long savedJobId)
        {
            try
            {
                var item = await GetSavedJobByIdForReadAsync(savedJobId);
                if (!item.IsSuccess)
                {
                    return log.ErrorAndReturnModel<JobModel>(item);
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanViewSavedJobPermissionParamsAsync(item.CompanyId, item.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<SavedJobModel>(errorStr);
                //}

                var result = this.ConvertToItem(item.Model, objectMapperService.SavedJobModelToJobModel);
                return result;
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<JobModel>($"Unexpected error when getting savedJob", new { savedJobId }, ex);
            }
        }

        public async Task<ReturnModel<SavedJobModel>> CreateSavedJobAsync(SavedJobCreateModel savedJob)
        {
            try
            {
                if (savedJob == null)
                {
                    return log.ErrorAndReturnModel<SavedJobModel>($"Unable to create SavedJob", new ReturnErrorItem("", "SavedJob information is not valid"));
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.CanCreateSavedJobAsync();

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<SavedJobModel>(errorStr);
                //}

                //if (canAssign)
                //{
                //    savedJob.CompanyId = await currentSessionService.GetCurrentTenantIdAsync();
                //    savedJob.TenantEntityId = await currentSessionService.GetCurrentTenantEntityIdAsNullableAsync();
                //}

                var errorList = IsSavedJobModelValidForCreateAsync(savedJob);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<SavedJobModel>($"Unable to create savedJob", errorList);
                }

                savedJob.IsDeleted = false;

                var item = objectMapperService.SavedJobCreateModelToSavedJob(savedJob);

                var jobTemplateItem = await reportRepository.GetJobTemplateViewByIdAsync(savedJob.JobTemplateId);
                if (jobTemplateItem == null)
                {
                    return log.ErrorAndReturnModel<SavedJobModel>($"Unable to Get the JobTemplate Information", new { savedJob.JobTemplateId });
                }

                item.JobTemplateId = jobTemplateItem.JobTemplateId;

                var submittedByUserItem = await companyRepository.GetUserViewByIdAsync(savedJob.SubmittedByUserId ?? 0);
                if (submittedByUserItem == null)
                {
                    return log.ErrorAndReturnModel<SavedJobModel>($"Unable to Get the SubmittedByUser Information", new { savedJob.SubmittedByUserId });
                }

                item.SubmittedByUserId = submittedByUserItem.UserId;
                if (savedJob.UserId > 0)
                {
                    var userItem = await companyRepository.GetUserViewByIdAsync(savedJob.UserId ?? 0);
                    if (userItem == null)
                    {
                        return log.ErrorAndReturnModel<SavedJobModel>($"Unable to Get the User Information", new { savedJob.UserId });
                    }

                    item.UserId = userItem.UserId;
                }

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    item = await reportRepository.CreateSavedJobAsync(item, await currentSessionService.GetCurrentUserIdAsync());
                    if (item == null)
                    {
                        throw new Exception($"Unable to create savedJob");
                    }

                    await trx.CompleteAsync();
                }

                // await auditService.SavedJobCreatedAsync(item.SavedJobId);

                var view = await reportRepository.GetSavedJobViewByIdAsync(item.SavedJobId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<SavedJobModel>($"Unable to find the savedJob", new { item.SavedJobId });
                }

                return this.ConvertToItem(view, objectMapperService.SavedJobViewToSavedJobModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModelWithRollbackExceptionHandler<SavedJobModel>($"Unexpected error when creating SavedJob {savedJob?.SavedJobId}", ex);
            }
        }

        public async Task<ReturnModel<SavedJobModel>> UpdateSavedJobAsync(SavedJobUpdateModel savedJob)
        {
            try
            {
                if (savedJob == null)
                {
                    return log.ErrorAndReturnModel<SavedJobModel>($"Unable to update SavedJob", new ReturnErrorItem("", "SavedJob information is not valid"));
                }

                var existingSavedJob = await reportRepository.GetSavedJobViewFromAnyByIdAsync(savedJob.SavedJobId);
                if (existingSavedJob == null)
                {
                    return log.ErrorAndReturnModel<SavedJobModel>($"Unable to find the savedJob", new { savedJob.SavedJobId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanManageSavedJobPermissionParamsAsync(existingSavedJob.CompanyId, existingSavedJob.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<SavedJobModel>(errorStr);
                //}

                //if (canAssign)
                //{
                //    savedJob.CompanyId = await currentSessionService.GetCurrentTenantIdAsync();
                //    //savedJob.TenantEntityId = await currentSessionService.GetCurrentTenantEntityIdAsNullableAsync();
                //}

                savedJob.CreatedByUserId = existingSavedJob.CreatedByUserId;
                savedJob.CreatedDate = existingSavedJob.CreatedDate;
                savedJob.IsDeleted = existingSavedJob.IsDeleted;

                var errorList = IsSavedJobModelValidForUpdateAsync(savedJob);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<SavedJobModel>($"Unable to update savedJob", errorList);
                }

                var item = objectMapperService.SavedJobUpdateModelToSavedJob(savedJob);

                var jobTemplateItem = await reportRepository.GetJobTemplateViewByIdAsync(savedJob.JobTemplateId);
                if (jobTemplateItem == null)
                {
                    return log.ErrorAndReturnModel<SavedJobModel>($"Unable to Get the JobTemplate Information", new { savedJob.JobTemplateId });
                }

                item.JobTemplateId = jobTemplateItem.JobTemplateId;

                var submittedByUserItem = await companyRepository.GetUserViewByIdAsync(savedJob.SubmittedByUserId ?? 0);
                if (submittedByUserItem == null)
                {
                    return log.ErrorAndReturnModel<SavedJobModel>($"Unable to Get the SubmittedByUser Information", new { savedJob.SubmittedByUserId });
                }

                item.SubmittedByUserId = submittedByUserItem.UserId;
                if (savedJob.UserId > 0)
                {
                    var userItem = await companyRepository.GetUserViewByIdAsync(savedJob.UserId ?? 0);
                    if (userItem == null)
                    {
                        return log.ErrorAndReturnModel<SavedJobModel>($"Unable to Get the User Information", new { savedJob.UserId });
                    }

                    item.UserId = userItem.UserId;
                }

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await reportRepository.UpdateSavedJobAsync(item, await currentSessionService.GetCurrentUserIdAsync());
                    if (result == false)
                    {
                        throw new RollbackException($"Unable to update savedJob");
                    }

                    await trx.CompleteAsync();
                }

                // await auditService.SavedJobUpdatedAsync(savedJob.SavedJobId);

                var view = await reportRepository.GetSavedJobViewByIdAsync(savedJob.SavedJobId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<SavedJobModel>($"Unable to find the savedJob", new { savedJob.SavedJobId });
                }

                return this.ConvertToItem(view, objectMapperService.SavedJobViewToSavedJobModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<SavedJobModel>($"Unexpected error when updating SavedJob {savedJob?.SavedJobId}", ex);
            }
        }

        public async Task<ReturnModel<bool>> MarkSavedJobAsNotDeletedByIdAsync(long savedJobId)
        {
            try
            {
                var savedJob = await reportRepository.GetSavedJobViewFromAnyByIdAsync(savedJobId);
                if (savedJob == null)
                {
                    return log.ErrorAndReturnModel<bool>($"Unable to find the savedJob", new { savedJobId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanManageSavedJobPermissionParamsAsync(savedJob.CompanyId, savedJob.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<bool>(errorStr);
                //}

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await reportRepository.MarkSavedJobAsNotDeletedByIdAsync(savedJobId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                // await auditService.SavedJobDeletedAsync(savedJob.SavedJobId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking savedJob as not deleted {savedJobId}", ex);
            }
        }

        public async Task<ReturnModel<bool>> MarkSavedJobAsDeletedByIdAsync(long savedJobId)
        {
            try
            {
                var savedJob = await reportRepository.GetSavedJobViewFromAnyByIdAsync(savedJobId);
                if (savedJob == null)
                {
                    return log.ErrorAndReturnModel<bool>($"Unable to find the savedJob", new { savedJobId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanManageSavedJobPermissionParamsAsync(savedJob.CompanyId, savedJob.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<bool>(errorStr);
                //}

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await reportRepository.MarkSavedJobAsDeletedByIdAsync(savedJobId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                // await auditService.SavedJobDeletedAsync(savedJob.SavedJobId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking savedJob as deleted {savedJobId}", ex);
            }
        }

        public async Task<ReturnModel<bool>> HardDeleteSavedJobByAsync(long savedJobId)
        {
            try
            {
                var savedJob = await reportRepository.GetSavedJobViewFromAnyByIdAsync(savedJobId);
                if (savedJob == null)
                {
                    return log.ErrorAndReturnModel<bool>($"Unable to find the savedJob", new { savedJobId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanManageSavedJobPermissionParamsAsync(savedJob.CompanyId, savedJob.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<bool>(errorStr);
                //}

                bool result;

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await reportRepository.HardDeleteSavedJobByIdAsync(savedJobId);
                    await trx.CompleteAsync();
                }

                // await auditService.HardDeleteSavedJobAsync(savedJob.SavedJobId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when deleting savedJob {savedJobId}", ex);
            }
        }

        public List<ReturnErrorItem> IsSavedJobModelValidForCreateAsync(SavedJobUpdateModel savedJobModel)
        {
            return IsSavedJobModelValidForAllAsync(true, savedJobModel);
        }

        public List<ReturnErrorItem> IsSavedJobModelValidForUpdateAsync(SavedJobUpdateModel savedJobModel)
        {
            return IsSavedJobModelValidForAllAsync(false, savedJobModel);
        }

        public List<ReturnErrorItem> IsSavedJobModelValidForAllAsync(bool isCreate, SavedJobUpdateModel savedJobModel)
        {
            var result = new List<ReturnErrorItem>();

            if (isCreate)
            {
                if (savedJobModel.SavedJobId > 0)
                {
                    result.Add(new ReturnErrorItem("", "SavedJob Identifier is not valid"));
                }
            }
            else
            {
                if (savedJobModel.SavedJobId <= 0)
                {
                    result.Add(new ReturnErrorItem("", "SavedJob Unique Identifier is not valid"));
                }
            }

            if (!ValidationUtils.IsGuidValid(savedJobModel.JobTemplateId))
            {
                result.Add(new ReturnErrorItem("JobTemplateId", "JobTemplate cannot be empty"));
            }

            if (savedJobModel.CompletedDate != null && !DateUtils.IsValidDate(savedJobModel.CompletedDate.Value))
            {
                result.Add(new ReturnErrorItem("CompletedDate", $"CompletedDate is not a valid date"));
            }

            if (!DateUtils.IsValidDate(savedJobModel.SubmittedDate))
            {
                result.Add(new ReturnErrorItem("SubmittedDate", $"SubmittedDate is not a valid date"));
            }

            if (!ValidationUtils.IsGuidValid(savedJobModel.SubmittedByUserId))
            {
                result.Add(new ReturnErrorItem("SubmittedByUserId", "SubmittedByUser cannot be empty"));
            }

            if (savedJobModel.ExecutableName != null && savedJobModel.ExecutableName.Length > SavedJobModel.COLUMN_SIZE_ExecutableName)
            {
                result.Add(new ReturnErrorItem("ExecutableName", $"ExecutableName size should not exceed {SavedJobModel.COLUMN_SIZE_ExecutableName}"));
            }

            if (savedJobModel.ExecutableName.IsTrimmedStringNullOrEmpty())
            {
                result.Add(new ReturnErrorItem("ExecutableName", "ExecutableName is required "));
            }

            if (savedJobModel.CustomExportName != null && savedJobModel.CustomExportName.Length > SavedJobModel.COLUMN_SIZE_CustomExportName)
            {
                result.Add(new ReturnErrorItem("CustomExportName", $"CustomExportName size should not exceed {SavedJobModel.COLUMN_SIZE_CustomExportName}"));
            }

            if (savedJobModel.PickupDate != null && !DateUtils.IsValidDate(savedJobModel.PickupDate.Value))
            {
                result.Add(new ReturnErrorItem("PickupDate", $"PickupDate is not a valid date"));
            }

            if (savedJobModel.JobName != null && savedJobModel.JobName.Length > SavedJobModel.COLUMN_SIZE_JobName)
            {
                result.Add(new ReturnErrorItem("JobName", $"JobName size should not exceed {SavedJobModel.COLUMN_SIZE_JobName}"));
            }

            return result.Count > 0 ? result : null;
        }
        #endregion

        #region SavedJobNotificationEmail

        public async Task<ReturnListModel<SavedJobNotificationEmailModel>> GetSavedJobNotificationEmailsAsync(SavedJobNotificationEmailSearchCriteria search)
        {
            try
            {
                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanViewSavedJobNotificationEmailPermissionParamsAsync();

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnListModel<SavedJobNotificationEmailModel>(errorStr);
                //}

                //if (canAssign)
                //{
                //    search.CompanyId = await currentSessionService.GetCurrentCompanyIdAsync();
                //    //search.TenantEntityId = await currentSessionService.GetCurrentTenantEntityIdAsNullableAsync();
                //}

                return this.ConvertToList(await reportRepository.GetSavedJobNotificationEmailsAsync(search), objectMapperService.SavedJobNotificationEmailViewListToSavedJobNotificationEmailModelList);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<SavedJobNotificationEmailModel>($"Unexpected error when getting SavedJobNotificationEmails", ex);
            }
        }

        public Task<ReturnModel<SavedJobNotificationEmailModel>> GetSavedJobNotificationEmailByIdForReadAsync(long savedJobNotificationEmailId)
        {
            return GetSavedJobNotificationEmailByIdInternalAsync(forWrite: false, savedJobNotificationEmailId);
        }

        public Task<ReturnModel<SavedJobNotificationEmailModel>> GetSavedJobNotificationEmailByIdForWriteAsync(long savedJobNotificationEmailId)
        {
            return GetSavedJobNotificationEmailByIdInternalAsync(forWrite: true, savedJobNotificationEmailId);
        }

        public async Task<ReturnModel<SavedJobNotificationEmailModel>> GetSavedJobNotificationEmailByIdInternalAsync(bool forWrite, long savedJobNotificationEmailId)
        {
            try
            {
                var item = await reportRepository.GetSavedJobNotificationEmailViewByIdAsync(savedJobNotificationEmailId);
                if (item == null)
                {
                    return log.ErrorAndReturnModel<SavedJobNotificationEmailModel>($"Unable to find the savedJobNotificationEmail", new { savedJobNotificationEmailId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanViewSavedJobNotificationEmailPermissionParamsAsync(item.CompanyId, item.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<SavedJobNotificationEmailModel>(errorStr);
                //}

                return this.ConvertToItem(item, objectMapperService.SavedJobNotificationEmailViewToSavedJobNotificationEmailModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<SavedJobNotificationEmailModel>($"Unexpected error when getting savedJobNotificationEmail", new { savedJobNotificationEmailId }, ex);
            }
        }

        public async Task<ReturnModel<SavedJobNotificationEmailModel>> CreateSavedJobNotificationEmailAsync(SavedJobNotificationEmailCreateModel savedJobNotificationEmail)
        {
            try
            {
                if (savedJobNotificationEmail == null)
                {
                    return log.ErrorAndReturnModel<SavedJobNotificationEmailModel>($"Unable to create SavedJobNotificationEmail", new ReturnErrorItem("", "SavedJobNotificationEmail information is not valid"));
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.CanCreateSavedJobNotificationEmailAsync();

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<SavedJobNotificationEmailModel>(errorStr);
                //}

                //if (canAssign)
                //{
                //    savedJobNotificationEmail.CompanyId = await currentSessionService.GetCurrentTenantIdAsync();
                //    savedJobNotificationEmail.TenantEntityId = await currentSessionService.GetCurrentTenantEntityIdAsNullableAsync();
                //}

                var errorList = IsSavedJobNotificationEmailModelValidForCreateAsync(savedJobNotificationEmail);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<SavedJobNotificationEmailModel>($"Unable to create savedJobNotificationEmail", errorList);
                }

                savedJobNotificationEmail.IsDeleted = false;

                var item = objectMapperService.SavedJobNotificationEmailCreateModelToSavedJobNotificationEmail(savedJobNotificationEmail);

                var savedJobItem = await reportRepository.GetSavedJobViewByIdAsync(savedJobNotificationEmail.SavedJobId);
                if (savedJobItem == null)
                {
                    return log.ErrorAndReturnModel<SavedJobNotificationEmailModel>($"Unable to Get the SavedJob Information", new { savedJobNotificationEmail.SavedJobId });
                }

                item.SavedJobId = savedJobItem.SavedJobId;

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    item = await reportRepository.CreateSavedJobNotificationEmailAsync(item, await currentSessionService.GetCurrentUserIdAsync());
                    if (item == null)
                    {
                        throw new Exception($"Unable to create savedJobNotificationEmail");
                    }

                    await trx.CompleteAsync();
                }

                // await auditService.SavedJobNotificationEmailCreatedAsync(item.SavedJobNotificationEmailId);

                var view = await reportRepository.GetSavedJobNotificationEmailViewByIdAsync(item.SavedJobNotificationEmailId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<SavedJobNotificationEmailModel>($"Unable to find the savedJobNotificationEmail", new { item.SavedJobNotificationEmailId });
                }

                return this.ConvertToItem(view, objectMapperService.SavedJobNotificationEmailViewToSavedJobNotificationEmailModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModelWithRollbackExceptionHandler<SavedJobNotificationEmailModel>($"Unexpected error when creating SavedJobNotificationEmail {savedJobNotificationEmail?.SavedJobNotificationEmailId}", ex);
            }
        }

        public async Task<ReturnModel<SavedJobNotificationEmailModel>> UpdateSavedJobNotificationEmailAsync(SavedJobNotificationEmailUpdateModel savedJobNotificationEmail)
        {
            try
            {
                if (savedJobNotificationEmail == null)
                {
                    return log.ErrorAndReturnModel<SavedJobNotificationEmailModel>($"Unable to update SavedJobNotificationEmail", new ReturnErrorItem("", "SavedJobNotificationEmail information is not valid"));
                }

                var existingSavedJobNotificationEmail = await reportRepository.GetSavedJobNotificationEmailViewFromAnyByIdAsync(savedJobNotificationEmail.SavedJobNotificationEmailId);
                if (existingSavedJobNotificationEmail == null)
                {
                    return log.ErrorAndReturnModel<SavedJobNotificationEmailModel>($"Unable to find the savedJobNotificationEmail", new { savedJobNotificationEmail.SavedJobNotificationEmailId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanManageSavedJobNotificationEmailPermissionParamsAsync(existingSavedJobNotificationEmail.CompanyId, existingSavedJobNotificationEmail.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<SavedJobNotificationEmailModel>(errorStr);
                //}

                //if (canAssign)
                //{
                //    savedJobNotificationEmail.CompanyId = await currentSessionService.GetCurrentTenantIdAsync();
                //    //savedJobNotificationEmail.TenantEntityId = await currentSessionService.GetCurrentTenantEntityIdAsNullableAsync();
                //}

                savedJobNotificationEmail.CreatedByUserId = existingSavedJobNotificationEmail.CreatedByUserId;
                savedJobNotificationEmail.CreatedDate = existingSavedJobNotificationEmail.CreatedDate;
                savedJobNotificationEmail.IsDeleted = existingSavedJobNotificationEmail.IsDeleted;
                //savedJobNotificationEmail.CompanyId = existingSavedJobNotificationEmail.CompanyId;

                var errorList = IsSavedJobNotificationEmailModelValidForUpdateAsync(savedJobNotificationEmail);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<SavedJobNotificationEmailModel>($"Unable to update savedJobNotificationEmail", errorList);
                }

                var item = objectMapperService.SavedJobNotificationEmailUpdateModelToSavedJobNotificationEmail(savedJobNotificationEmail);

                var savedJobItem = await reportRepository.GetSavedJobViewByIdAsync(savedJobNotificationEmail.SavedJobId);
                if (savedJobItem == null)
                {
                    return log.ErrorAndReturnModel<SavedJobNotificationEmailModel>($"Unable to Get the SavedJob Information", new { savedJobNotificationEmail.SavedJobId });
                }

                item.SavedJobId = savedJobItem.SavedJobId;

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await reportRepository.UpdateSavedJobNotificationEmailAsync(item, await currentSessionService.GetCurrentUserIdAsync());
                    if (result == false)
                    {
                        throw new RollbackException($"Unable to update savedJobNotificationEmail");
                    }

                    await trx.CompleteAsync();
                }

                // await auditService.SavedJobNotificationEmailUpdatedAsync(savedJobNotificationEmail.SavedJobNotificationEmailId);

                var view = await reportRepository.GetSavedJobNotificationEmailViewByIdAsync(savedJobNotificationEmail.SavedJobNotificationEmailId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<SavedJobNotificationEmailModel>($"Unable to find the savedJobNotificationEmail", new { savedJobNotificationEmail.SavedJobNotificationEmailId });
                }

                return this.ConvertToItem(view, objectMapperService.SavedJobNotificationEmailViewToSavedJobNotificationEmailModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<SavedJobNotificationEmailModel>($"Unexpected error when updating SavedJobNotificationEmail {savedJobNotificationEmail?.SavedJobNotificationEmailId}", ex);
            }
        }

        public async Task<ReturnModel<bool>> MarkSavedJobNotificationEmailAsNotDeletedByIdAsync(long savedJobNotificationEmailId)
        {
            try
            {
                var savedJobNotificationEmail = await reportRepository.GetSavedJobNotificationEmailViewFromAnyByIdAsync(savedJobNotificationEmailId);
                if (savedJobNotificationEmail == null)
                {
                    return log.ErrorAndReturnModel<bool>($"Unable to find the savedJobNotificationEmail", new { savedJobNotificationEmailId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanManageSavedJobNotificationEmailPermissionParamsAsync(savedJobNotificationEmail.CompanyId, savedJobNotificationEmail.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<bool>(errorStr);
                //}

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await reportRepository.MarkSavedJobNotificationEmailAsNotDeletedByIdAsync(savedJobNotificationEmailId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                // await auditService.SavedJobNotificationEmailDeletedAsync(savedJobNotificationEmail.SavedJobNotificationEmailId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking savedJobNotificationEmail as not deleted {savedJobNotificationEmailId}", ex);
            }
        }

        public async Task<ReturnModel<bool>> MarkSavedJobNotificationEmailAsDeletedByIdAsync(long savedJobNotificationEmailId)
        {
            try
            {
                var savedJobNotificationEmail = await reportRepository.GetSavedJobNotificationEmailViewFromAnyByIdAsync(savedJobNotificationEmailId);
                if (savedJobNotificationEmail == null)
                {
                    return log.ErrorAndReturnModel<bool>($"Unable to find the savedJobNotificationEmail", new { savedJobNotificationEmailId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanManageSavedJobNotificationEmailPermissionParamsAsync(savedJobNotificationEmail.CompanyId, savedJobNotificationEmail.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<bool>(errorStr);
                //}

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await reportRepository.MarkSavedJobNotificationEmailAsDeletedByIdAsync(savedJobNotificationEmailId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                // await auditService.SavedJobNotificationEmailDeletedAsync(savedJobNotificationEmail.SavedJobNotificationEmailId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking savedJobNotificationEmail as deleted {savedJobNotificationEmailId}", ex);
            }
        }

        public async Task<ReturnModel<bool>> HardDeleteSavedJobNotificationEmailByAsync(long savedJobNotificationEmailId)
        {
            try
            {
                var savedJobNotificationEmail = await reportRepository.GetSavedJobNotificationEmailViewFromAnyByIdAsync(savedJobNotificationEmailId);
                if (savedJobNotificationEmail == null)
                {
                    return log.ErrorAndReturnModel<bool>($"Unable to find the savedJobNotificationEmail", new { savedJobNotificationEmailId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanManageSavedJobNotificationEmailPermissionParamsAsync(savedJobNotificationEmail.CompanyId, savedJobNotificationEmail.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<bool>(errorStr);
                //}

                bool result;

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await reportRepository.HardDeleteSavedJobNotificationEmailByIdAsync(savedJobNotificationEmailId);
                    await trx.CompleteAsync();
                }

                // await auditService.HardDeleteSavedJobNotificationEmailAsync(savedJobNotificationEmail.SavedJobNotificationEmailId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when deleting savedJobNotificationEmail {savedJobNotificationEmailId}", ex);
            }
        }

        public List<ReturnErrorItem> IsSavedJobNotificationEmailModelValidForCreateAsync(SavedJobNotificationEmailUpdateModel savedJobNotificationEmailModel)
        {
            return IsSavedJobNotificationEmailModelValidForAllAsync(true, savedJobNotificationEmailModel);
        }

        public List<ReturnErrorItem> IsSavedJobNotificationEmailModelValidForUpdateAsync(SavedJobNotificationEmailUpdateModel savedJobNotificationEmailModel)
        {
            return IsSavedJobNotificationEmailModelValidForAllAsync(false, savedJobNotificationEmailModel);
        }

        public List<ReturnErrorItem> IsSavedJobNotificationEmailModelValidForAllAsync(bool isCreate, SavedJobNotificationEmailUpdateModel savedJobNotificationEmailModel)
        {
            var result = new List<ReturnErrorItem>();

            if (isCreate)
            {
                if (savedJobNotificationEmailModel.SavedJobNotificationEmailId > 0)
                {
                    result.Add(new ReturnErrorItem("", "SavedJobNotificationEmail Identifier is not valid"));
                }
            }
            else
            {
                if (savedJobNotificationEmailModel.SavedJobNotificationEmailId <= 0)
                {
                    result.Add(new ReturnErrorItem("", "SavedJobNotificationEmail Unique Identifier is not valid"));
                }
            }

            if (!ValidationUtils.IsGuidValid(savedJobNotificationEmailModel.SavedJobId))
            {
                result.Add(new ReturnErrorItem("SavedJobId", "SavedJob cannot be empty"));
            }

            if (savedJobNotificationEmailModel.Email != null && savedJobNotificationEmailModel.Email.Length > SavedJobNotificationEmailModel.COLUMN_SIZE_Email)
            {
                result.Add(new ReturnErrorItem("Email", $"Email size should not exceed {SavedJobNotificationEmailModel.COLUMN_SIZE_Email}"));
            }

            if (savedJobNotificationEmailModel.Email.IsTrimmedStringNullOrEmpty())
            {
                result.Add(new ReturnErrorItem("Email", "Email is required"));
            }

            if (!ValidationUtils.IsEmail(savedJobNotificationEmailModel.Email))
            {
                result.Add(new ReturnErrorItem("Email", "Email is not valid."));
            }

            if (savedJobNotificationEmailModel.Message != null && savedJobNotificationEmailModel.Message.Length > SavedJobNotificationEmailModel.COLUMN_SIZE_Message)
            {
                result.Add(new ReturnErrorItem("Message", $"Message size should not exceed {SavedJobNotificationEmailModel.COLUMN_SIZE_Message}"));
            }

            return result.Count > 0 ? result : null;
        }
        #endregion

        #region ScheduledJob

        public async Task<ReturnListModel<ScheduledJobModel>> GetScheduledJobsAsync(ScheduledJobSearchCriteria search)
        {
            try
            {
                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanViewScheduledJobPermissionParamsAsync();

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnListModel<ScheduledJobModel>(errorStr);
                //}

                //if (canAssign)
                //{
                //    search.CompanyId = await currentSessionService.GetCurrentCompanyIdAsync();
                //    //search.TenantEntityId = await currentSessionService.GetCurrentTenantEntityIdAsNullableAsync();
                //}

                return this.ConvertToList(await reportRepository.GetScheduledJobsAsync(search), objectMapperService.ScheduledJobViewListToScheduledJobModelList);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<ScheduledJobModel>($"Unexpected error when getting ScheduledJobs", ex);
            }
        }

        public Task<ReturnModel<ScheduledJobModel>> GetScheduledJobByIdForReadAsync(long scheduledJobId)
        {
            return GetScheduledJobByIdInternalAsync(forWrite: false, scheduledJobId);
        }

        public Task<ReturnModel<ScheduledJobModel>> GetScheduledJobByIdForWriteAsync(long scheduledJobId)
        {
            return GetScheduledJobByIdInternalAsync(forWrite: true, scheduledJobId);
        }

        public async Task<ReturnModel<ScheduledJobModel>> GetScheduledJobByIdInternalAsync(bool forWrite, long scheduledJobId)
        {
            try
            {
                var item = await reportRepository.GetScheduledJobViewByIdAsync(scheduledJobId);
                if (item == null)
                {
                    return log.ErrorAndReturnModel<ScheduledJobModel>($"Unable to find the scheduledJob", new { scheduledJobId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanViewScheduledJobPermissionParamsAsync(item.CompanyId, item.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<ScheduledJobModel>(errorStr);
                //}

                var result = this.ConvertToItem(item, objectMapperService.ScheduledJobViewToScheduledJobModel);
                if (result.IsSuccess)
                {
                    result.Model.NotificationEmails = objectMapperService.ScheduledJobNotificationEmailViewListToScheduledJobNotificationEmailModelList((await reportRepository.GetScheduledJobNotificationEmailsAsync(new ScheduledJobNotificationEmailSearchCriteria { ScheduledJobId = scheduledJobId, PageSize = int.MaxValue })).Result);
                }

                return result;
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<ScheduledJobModel>($"Unexpected error when getting scheduledJob", new { scheduledJobId }, ex);
            }
        }

        public async Task<ReturnModel<JobModel>> RunScheduledJobAsync(long scheduledJobId)
        {
            try
            {
                var item = await GetScheduledJobByIdForReadAsync(scheduledJobId);
                if (!item.IsSuccess)
                {
                    return log.ErrorAndReturnModel<JobModel>(item);
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanViewScheduledJobPermissionParamsAsync(item.CompanyId, item.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<ScheduledJobModel>(errorStr);
                //}

                var result = this.ConvertToItem(item.Model, objectMapperService.ScheduledJobModelToJobModel);
                return result;
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<JobModel>($"Unexpected error when getting scheduledJob", new { scheduledJobId }, ex);
            }
        }

        public async Task<ReturnModel<ScheduledJobModel>> CreateScheduledJobAsync(ScheduledJobCreateModel scheduledJob)
        {
            try
            {
                if (scheduledJob == null)
                {
                    return log.ErrorAndReturnModel<ScheduledJobModel>($"Unable to create ScheduledJob", new ReturnErrorItem("", "ScheduledJob information is not valid"));
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.CanCreateScheduledJobAsync();

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<ScheduledJobModel>(errorStr);
                //}

                //if (canAssign)
                //{
                //    scheduledJob.CompanyId = await currentSessionService.GetCurrentTenantIdAsync();
                //    scheduledJob.TenantEntityId = await currentSessionService.GetCurrentTenantEntityIdAsNullableAsync();
                //}

                var errorList = IsScheduledJobModelValidForCreateAsync(scheduledJob);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<ScheduledJobModel>($"Unable to create scheduledJob", errorList);
                }

                scheduledJob.IsDeleted = false;

                var item = objectMapperService.ScheduledJobCreateModelToScheduledJob(scheduledJob);

                var jobTemplateItem = await reportRepository.GetJobTemplateViewByIdAsync(scheduledJob.JobTemplateId);
                if (jobTemplateItem == null)
                {
                    return log.ErrorAndReturnModel<ScheduledJobModel>($"Unable to Get the JobTemplate Information", new { scheduledJob.JobTemplateId });
                }

                item.JobTemplateId = jobTemplateItem.JobTemplateId;

                var submittedByUserItem = await companyRepository.GetUserViewByIdAsync(scheduledJob.SubmittedByUserId ?? 0);
                if (submittedByUserItem == null)
                {
                    return log.ErrorAndReturnModel<ScheduledJobModel>($"Unable to Get the SubmittedByUser Information", new { scheduledJob.SubmittedByUserId });
                }

                item.SubmittedByUserId = submittedByUserItem.UserId;
                var runAsUserItem = await companyRepository.GetUserViewByIdAsync(scheduledJob.RunAsUserId ?? 0);
                if (runAsUserItem == null)
                {
                    return log.ErrorAndReturnModel<ScheduledJobModel>($"Unable to Get the RunAsUser Information", new { scheduledJob.RunAsUserId });
                }

                item.RunAsUserId = runAsUserItem.UserId;

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    item = await reportRepository.CreateScheduledJobAsync(item, await currentSessionService.GetCurrentUserIdAsync());
                    if (item == null)
                    {
                        throw new Exception($"Unable to create scheduledJob");
                    }

                    await trx.CompleteAsync();
                }

                // await auditService.ScheduledJobCreatedAsync(item.ScheduledJobId);

                var view = await reportRepository.GetScheduledJobViewByIdAsync(item.ScheduledJobId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<ScheduledJobModel>($"Unable to find the scheduledJob", new { item.ScheduledJobId });
                }

                return this.ConvertToItem(view, objectMapperService.ScheduledJobViewToScheduledJobModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModelWithRollbackExceptionHandler<ScheduledJobModel>($"Unexpected error when creating ScheduledJob {scheduledJob?.ScheduledJobId}", ex);
            }
        }

        public async Task<ReturnModel<ScheduledJobModel>> UpdateScheduledJobAsync(ScheduledJobUpdateModel scheduledJob)
        {
            try
            {
                if (scheduledJob == null)
                {
                    return log.ErrorAndReturnModel<ScheduledJobModel>($"Unable to update ScheduledJob", new ReturnErrorItem("", "ScheduledJob information is not valid"));
                }

                var existingScheduledJob = await reportRepository.GetScheduledJobViewFromAnyByIdAsync(scheduledJob.ScheduledJobId);
                if (existingScheduledJob == null)
                {
                    return log.ErrorAndReturnModel<ScheduledJobModel>($"Unable to find the scheduledJob", new { scheduledJob.ScheduledJobId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanManageScheduledJobPermissionParamsAsync(existingScheduledJob.CompanyId, existingScheduledJob.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<ScheduledJobModel>(errorStr);
                //}

                //if (canAssign)
                //{
                //    scheduledJob.CompanyId = await currentSessionService.GetCurrentTenantIdAsync();
                //    //scheduledJob.TenantEntityId = await currentSessionService.GetCurrentTenantEntityIdAsNullableAsync();
                //}

                scheduledJob.CreatedByUserId = existingScheduledJob.CreatedByUserId;
                scheduledJob.CreatedDate = existingScheduledJob.CreatedDate;
                scheduledJob.IsDeleted = existingScheduledJob.IsDeleted;

                var errorList = IsScheduledJobModelValidForUpdateAsync(scheduledJob);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<ScheduledJobModel>($"Unable to update scheduledJob", errorList);
                }

                var item = objectMapperService.ScheduledJobUpdateModelToScheduledJob(scheduledJob);

                var jobTemplateItem = await reportRepository.GetJobTemplateViewByIdAsync(scheduledJob.JobTemplateId);
                if (jobTemplateItem == null)
                {
                    return log.ErrorAndReturnModel<ScheduledJobModel>($"Unable to Get the JobTemplate Information", new { scheduledJob.JobTemplateId });
                }

                item.JobTemplateId = jobTemplateItem.JobTemplateId;

                var submittedByUserItem = await companyRepository.GetUserViewByIdAsync(scheduledJob.SubmittedByUserId ?? 0);
                if (submittedByUserItem == null)
                {
                    return log.ErrorAndReturnModel<ScheduledJobModel>($"Unable to Get the SubmittedByUser Information", new { scheduledJob.SubmittedByUserId });
                }

                item.SubmittedByUserId = submittedByUserItem.UserId;
                var runAsUserItem = await companyRepository.GetUserViewByIdAsync(scheduledJob.RunAsUserId ?? 0);
                if (runAsUserItem == null)
                {
                    return log.ErrorAndReturnModel<ScheduledJobModel>($"Unable to Get the RunAsUser Information", new { scheduledJob.RunAsUserId });
                }

                item.RunAsUserId = runAsUserItem.UserId;

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await reportRepository.UpdateScheduledJobAsync(item, await currentSessionService.GetCurrentUserIdAsync());
                    if (result == false)
                    {
                        throw new RollbackException($"Unable to update scheduledJob");
                    }

                    await trx.CompleteAsync();
                }

                // await auditService.ScheduledJobUpdatedAsync(scheduledJob.ScheduledJobId);

                var view = await reportRepository.GetScheduledJobViewByIdAsync(scheduledJob.ScheduledJobId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<ScheduledJobModel>($"Unable to find the scheduledJob", new { scheduledJob.ScheduledJobId });
                }

                return this.ConvertToItem(view, objectMapperService.ScheduledJobViewToScheduledJobModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<ScheduledJobModel>($"Unexpected error when updating ScheduledJob {scheduledJob?.ScheduledJobId}", ex);
            }
        }

        public async Task<ReturnModel<bool>> MarkScheduledJobAsNotDeletedByIdAsync(long scheduledJobId)
        {
            try
            {
                var scheduledJob = await reportRepository.GetScheduledJobViewFromAnyByIdAsync(scheduledJobId);
                if (scheduledJob == null)
                {
                    return log.ErrorAndReturnModel<bool>($"Unable to find the scheduledJob", new { scheduledJobId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanManageScheduledJobPermissionParamsAsync(scheduledJob.CompanyId, scheduledJob.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<bool>(errorStr);
                //}

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await reportRepository.MarkScheduledJobAsNotDeletedByIdAsync(scheduledJobId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                // await auditService.ScheduledJobDeletedAsync(scheduledJob.ScheduledJobId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking scheduledJob as not deleted {scheduledJobId}", ex);
            }
        }

        public async Task<ReturnModel<bool>> MarkScheduledJobAsDeletedByIdAsync(long scheduledJobId)
        {
            try
            {
                var scheduledJob = await reportRepository.GetScheduledJobViewFromAnyByIdAsync(scheduledJobId);
                if (scheduledJob == null)
                {
                    return log.ErrorAndReturnModel<bool>($"Unable to find the scheduledJob", new { scheduledJobId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanManageScheduledJobPermissionParamsAsync(scheduledJob.CompanyId, scheduledJob.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<bool>(errorStr);
                //}

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await reportRepository.MarkScheduledJobAsDeletedByIdAsync(scheduledJobId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                // await auditService.ScheduledJobDeletedAsync(scheduledJob.ScheduledJobId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking scheduledJob as deleted {scheduledJobId}", ex);
            }
        }

        public async Task<ReturnModel<bool>> HardDeleteScheduledJobByAsync(long scheduledJobId)
        {
            try
            {
                var scheduledJob = await reportRepository.GetScheduledJobViewFromAnyByIdAsync(scheduledJobId);
                if (scheduledJob == null)
                {
                    return log.ErrorAndReturnModel<bool>($"Unable to find the scheduledJob", new { scheduledJobId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanManageScheduledJobPermissionParamsAsync(scheduledJob.CompanyId, scheduledJob.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<bool>(errorStr);
                //}

                bool result;

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await reportRepository.HardDeleteScheduledJobByIdAsync(scheduledJobId);
                    await trx.CompleteAsync();
                }

                // await auditService.HardDeleteScheduledJobAsync(scheduledJob.ScheduledJobId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when deleting scheduledJob {scheduledJobId}", ex);
            }
        }

        public List<ReturnErrorItem> IsScheduledJobModelValidForCreateAsync(ScheduledJobUpdateModel scheduledJobModel)
        {
            return IsScheduledJobModelValidForAllAsync(true, scheduledJobModel);
        }

        public List<ReturnErrorItem> IsScheduledJobModelValidForUpdateAsync(ScheduledJobUpdateModel scheduledJobModel)
        {
            return IsScheduledJobModelValidForAllAsync(false, scheduledJobModel);
        }

        public List<ReturnErrorItem> IsScheduledJobModelValidForAllAsync(bool isCreate, ScheduledJobUpdateModel scheduledJobModel)
        {
            var result = new List<ReturnErrorItem>();

            if (isCreate)
            {
                if (scheduledJobModel.ScheduledJobId > 0)
                {
                    result.Add(new ReturnErrorItem("", "ScheduledJob Identifier is not valid"));
                }
            }
            else
            {
                if (scheduledJobModel.ScheduledJobId <= 0)
                {
                    result.Add(new ReturnErrorItem("", "ScheduledJob Unique Identifier is not valid"));
                }
            }

            if (!ValidationUtils.IsGuidValid(scheduledJobModel.JobTemplateId))
            {
                result.Add(new ReturnErrorItem("JobTemplateId", "JobTemplate cannot be empty"));
            }

            if (!ValidationUtils.IsGuidValid(scheduledJobModel.SubmittedByUserId))
            {
                result.Add(new ReturnErrorItem("SubmittedByUserId", "SubmittedByUser cannot be empty"));
            }

            if (scheduledJobModel.ExecutableName != null && scheduledJobModel.ExecutableName.Length > ScheduledJobModel.COLUMN_SIZE_ExecutableName)
            {
                result.Add(new ReturnErrorItem("ExecutableName", $"ExecutableName size should not exceed {ScheduledJobModel.COLUMN_SIZE_ExecutableName}"));
            }

            if (scheduledJobModel.ExecutableName.IsTrimmedStringNullOrEmpty())
            {
                result.Add(new ReturnErrorItem("ExecutableName", "ExecutableName is required "));
            }

            if (scheduledJobModel.CustomExportName != null && scheduledJobModel.CustomExportName.Length > ScheduledJobModel.COLUMN_SIZE_CustomExportName)
            {
                result.Add(new ReturnErrorItem("CustomExportName", $"CustomExportName size should not exceed {ScheduledJobModel.COLUMN_SIZE_CustomExportName}"));
            }

            if (scheduledJobModel.PickupDate != null && !DateUtils.IsValidDate(scheduledJobModel.PickupDate.Value))
            {
                result.Add(new ReturnErrorItem("PickupDate", $"PickupDate is not a valid date"));
            }

            if (scheduledJobModel.JobName != null && scheduledJobModel.JobName.Length > ScheduledJobModel.COLUMN_SIZE_JobName)
            {
                result.Add(new ReturnErrorItem("JobName", $"JobName size should not exceed {ScheduledJobModel.COLUMN_SIZE_JobName}"));
            }

            if (!ValidationUtils.IsGuidValid(scheduledJobModel.RunAsUserId))
            {
                result.Add(new ReturnErrorItem("RunAsUserId", "RunAsUser cannot be empty"));
            }

            if (!DateUtils.IsValidDate(scheduledJobModel.StartDate))
            {
                result.Add(new ReturnErrorItem("StartDate", $"StartDate is not a valid date"));
            }

            if (scheduledJobModel.EndDate != null && !DateUtils.IsValidDate(scheduledJobModel.EndDate.Value))
            {
                result.Add(new ReturnErrorItem("EndDate", $"EndDate is not a valid date"));
            }

            if (scheduledJobModel.StartTime != null && !DateUtils.IsValidDate(scheduledJobModel.StartTime.Value))
            {
                result.Add(new ReturnErrorItem("StartTime", $"StartTime is not a valid date"));
            }

            if (!DateUtils.IsValidDate(scheduledJobModel.RunMonthlyTime))
            {
                result.Add(new ReturnErrorItem("RunMonthlyTime", $"RunMonthlyTime is not a valid date"));
            }

            if (!DateUtils.IsValidDate(scheduledJobModel.RunDailyTime))
            {
                result.Add(new ReturnErrorItem("RunDailyTime", $"RunDailyTime is not a valid date"));
            }

            return result.Count > 0 ? result : null;
        }
        #endregion

        #region ScheduledJobHistory

        public async Task<ReturnListModel<ScheduledJobHistoryModel>> GetScheduledJobHistorysAsync(ScheduledJobHistorySearchCriteria search)
        {
            try
            {
                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanViewScheduledJobHistoryPermissionParamsAsync();

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnListModel<ScheduledJobHistoryModel>(errorStr);
                //}

                //if (canAssign)
                //{
                //    search.CompanyId = await currentSessionService.GetCurrentCompanyIdAsync();
                //    //search.TenantEntityId = await currentSessionService.GetCurrentTenantEntityIdAsNullableAsync();
                //}

                return this.ConvertToList(await reportRepository.GetScheduledJobHistorysAsync(search), objectMapperService.ScheduledJobHistoryViewListToScheduledJobHistoryModelList);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<ScheduledJobHistoryModel>($"Unexpected error when getting Scheduled Job Historys", ex);
            }
        }

        public Task<ReturnModel<ScheduledJobHistoryModel>> GetScheduledJobHistoryByIdForReadAsync(long scheduledJobHistoryId)
        {
            return GetScheduledJobHistoryByIdInternalAsync(forWrite: false, scheduledJobHistoryId);
        }

        public Task<ReturnModel<ScheduledJobHistoryModel>> GetScheduledJobHistoryByIdForWriteAsync(long scheduledJobHistoryId)
        {
            return GetScheduledJobHistoryByIdInternalAsync(forWrite: true, scheduledJobHistoryId);
        }

        public async Task<ReturnModel<ScheduledJobHistoryModel>> GetScheduledJobHistoryByIdInternalAsync(bool forWrite, long scheduledJobHistoryId)
        {
            try
            {
                var item = await reportRepository.GetScheduledJobHistoryViewByIdAsync(scheduledJobHistoryId);
                if (item == null)
                {
                    return log.ErrorAndReturnModel<ScheduledJobHistoryModel>($"Unable to find the scheduledJobHistory", new { scheduledJobHistoryId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanViewScheduledJobHistoryPermissionParamsAsync(item.CompanyId, item.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<ScheduledJobHistoryModel>(errorStr);
                //}

                return this.ConvertToItem(item, objectMapperService.ScheduledJobHistoryViewToScheduledJobHistoryModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<ScheduledJobHistoryModel>($"Unexpected error when getting scheduledJobHistory", new { scheduledJobHistoryId }, ex);
            }
        }

        public async Task<ReturnModel<ScheduledJobHistoryModel>> CreateScheduledJobHistoryAsync(ScheduledJobHistoryCreateModel scheduledJobHistory)
        {
            try
            {
                if (scheduledJobHistory == null)
                {
                    return log.ErrorAndReturnModel<ScheduledJobHistoryModel>($"Unable to create ScheduledJobHistory", new ReturnErrorItem("", "ScheduledJobHistory information is not valid"));
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.CanCreateScheduledJobHistoryAsync();

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<ScheduledJobHistoryModel>(errorStr);
                //}

                //if (canAssign)
                //{
                //    scheduledJobHistory.CompanyId = await currentSessionService.GetCurrentTenantIdAsync();
                //    scheduledJobHistory.TenantEntityId = await currentSessionService.GetCurrentTenantEntityIdAsNullableAsync();
                //}

                var errorList = IsScheduledJobHistoryModelValidForCreateAsync(scheduledJobHistory);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<ScheduledJobHistoryModel>($"Unable to create scheduledJobHistory", errorList);
                }

                scheduledJobHistory.IsDeleted = false;

                var item = objectMapperService.ScheduledJobHistoryCreateModelToScheduledJobHistory(scheduledJobHistory);

                var scheduledJobItem = await reportRepository.GetScheduledJobViewByIdAsync(scheduledJobHistory.ScheduledJobId);
                if (scheduledJobItem == null)
                {
                    return log.ErrorAndReturnModel<ScheduledJobHistoryModel>($"Unable to Get the ScheduledJob Information", new { scheduledJobHistory.ScheduledJobId });
                }

                item.ScheduledJobId = scheduledJobItem.ScheduledJobId;
                item.PickupProcessId = scheduledJobHistory.PickupProcessId;

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    item = await reportRepository.CreateScheduledJobHistoryAsync(item, await currentSessionService.GetCurrentUserIdAsync());
                    if (item == null)
                    {
                        throw new Exception($"Unable to create scheduledJobHistory");
                    }

                    await trx.CompleteAsync();
                }

                // await auditService.ScheduledJobHistoryCreatedAsync(item.ScheduledJobHistoryId);

                var view = await reportRepository.GetScheduledJobHistoryViewByIdAsync(item.ScheduledJobHistoryId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<ScheduledJobHistoryModel>($"Unable to find the scheduledJobHistory", new { item.ScheduledJobHistoryId });
                }

                return this.ConvertToItem(view, objectMapperService.ScheduledJobHistoryViewToScheduledJobHistoryModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModelWithRollbackExceptionHandler<ScheduledJobHistoryModel>($"Unexpected error when creating ScheduledJobHistory {scheduledJobHistory?.ScheduledJobHistoryId}", ex);
            }
        }

        public async Task<ReturnModel<ScheduledJobHistoryModel>> UpdateScheduledJobHistoryAsync(ScheduledJobHistoryUpdateModel scheduledJobHistory)
        {
            try
            {
                if (scheduledJobHistory == null)
                {
                    return log.ErrorAndReturnModel<ScheduledJobHistoryModel>($"Unable to update ScheduledJobHistory", new ReturnErrorItem("", "ScheduledJobHistory information is not valid"));
                }

                var existingScheduledJobHistory = await reportRepository.GetScheduledJobHistoryViewFromAnyByIdAsync(scheduledJobHistory.ScheduledJobHistoryId);
                if (existingScheduledJobHistory == null)
                {
                    return log.ErrorAndReturnModel<ScheduledJobHistoryModel>($"Unable to find the scheduledJobHistory", new { scheduledJobHistory.ScheduledJobHistoryId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanManageScheduledJobHistoryPermissionParamsAsync(existingScheduledJobHistory.CompanyId, existingScheduledJobHistory.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<ScheduledJobHistoryModel>(errorStr);
                //}

                //if (canAssign)
                //{
                //    scheduledJobHistory.CompanyId = await currentSessionService.GetCurrentTenantIdAsync();
                //    //scheduledJobHistory.TenantEntityId = await currentSessionService.GetCurrentTenantEntityIdAsNullableAsync();
                //}

                scheduledJobHistory.CreatedByUserId = existingScheduledJobHistory.CreatedByUserId;
                scheduledJobHistory.CreatedDate = existingScheduledJobHistory.CreatedDate;
                scheduledJobHistory.IsDeleted = existingScheduledJobHistory.IsDeleted;

                var errorList = IsScheduledJobHistoryModelValidForUpdateAsync(scheduledJobHistory);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<ScheduledJobHistoryModel>($"Unable to update scheduledJobHistory", errorList);
                }

                var item = objectMapperService.ScheduledJobHistoryUpdateModelToScheduledJobHistory(scheduledJobHistory);

                var scheduledJobItem = await reportRepository.GetScheduledJobViewByIdAsync(scheduledJobHistory.ScheduledJobId);
                if (scheduledJobItem == null)
                {
                    return log.ErrorAndReturnModel<ScheduledJobHistoryModel>($"Unable to Get the ScheduledJob Information", new { scheduledJobHistory.ScheduledJobId });
                }

                item.ScheduledJobId = scheduledJobItem.ScheduledJobId;
                item.PickupProcessId = scheduledJobHistory.PickupProcessId;

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await reportRepository.UpdateScheduledJobHistoryAsync(item, await currentSessionService.GetCurrentUserIdAsync());
                    if (result == false)
                    {
                        throw new RollbackException($"Unable to update scheduledJobHistory");
                    }

                    await trx.CompleteAsync();
                }

                // await auditService.ScheduledJobHistoryUpdatedAsync(scheduledJobHistory.ScheduledJobHistoryId);

                var view = await reportRepository.GetScheduledJobHistoryViewByIdAsync(scheduledJobHistory.ScheduledJobHistoryId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<ScheduledJobHistoryModel>($"Unable to find the scheduledJobHistory", new { scheduledJobHistory.ScheduledJobHistoryId });
                }

                return this.ConvertToItem(view, objectMapperService.ScheduledJobHistoryViewToScheduledJobHistoryModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<ScheduledJobHistoryModel>($"Unexpected error when updating ScheduledJobHistory {scheduledJobHistory?.ScheduledJobHistoryId}", ex);
            }
        }

        public async Task<ReturnModel<bool>> MarkScheduledJobHistoryAsNotDeletedByIdAsync(long scheduledJobHistoryId)
        {
            try
            {
                var scheduledJobHistory = await reportRepository.GetScheduledJobHistoryViewFromAnyByIdAsync(scheduledJobHistoryId);
                if (scheduledJobHistory == null)
                {
                    return log.ErrorAndReturnModel<bool>($"Unable to find the scheduledJobHistory", new { scheduledJobHistoryId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanManageScheduledJobHistoryPermissionParamsAsync(scheduledJobHistory.CompanyId, scheduledJobHistory.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<bool>(errorStr);
                //}

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await reportRepository.MarkScheduledJobHistoryAsNotDeletedByIdAsync(scheduledJobHistoryId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                // await auditService.ScheduledJobHistoryDeletedAsync(scheduledJobHistory.ScheduledJobHistoryId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking scheduledJobHistory as not deleted {scheduledJobHistoryId}", ex);
            }
        }

        public async Task<ReturnModel<bool>> MarkScheduledJobHistoryAsDeletedByIdAsync(long scheduledJobHistoryId)
        {
            try
            {
                var scheduledJobHistory = await reportRepository.GetScheduledJobHistoryViewFromAnyByIdAsync(scheduledJobHistoryId);
                if (scheduledJobHistory == null)
                {
                    return log.ErrorAndReturnModel<bool>($"Unable to find the scheduledJobHistory", new { scheduledJobHistoryId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanManageScheduledJobHistoryPermissionParamsAsync(scheduledJobHistory.CompanyId, scheduledJobHistory.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<bool>(errorStr);
                //}

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await reportRepository.MarkScheduledJobHistoryAsDeletedByIdAsync(scheduledJobHistoryId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                // await auditService.ScheduledJobHistoryDeletedAsync(scheduledJobHistory.ScheduledJobHistoryId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking scheduledJobHistory as deleted {scheduledJobHistoryId}", ex);
            }
        }

        public async Task<ReturnModel<bool>> HardDeleteScheduledJobHistoryByAsync(long scheduledJobHistoryId)
        {
            try
            {
                var scheduledJobHistory = await reportRepository.GetScheduledJobHistoryViewFromAnyByIdAsync(scheduledJobHistoryId);
                if (scheduledJobHistory == null)
                {
                    return log.ErrorAndReturnModel<bool>($"Unable to find the scheduledJobHistory", new { scheduledJobHistoryId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanManageScheduledJobHistoryPermissionParamsAsync(scheduledJobHistory.CompanyId, scheduledJobHistory.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<bool>(errorStr);
                //}

                bool result;

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await reportRepository.HardDeleteScheduledJobHistoryByIdAsync(scheduledJobHistoryId);
                    await trx.CompleteAsync();
                }

                // await auditService.HardDeleteScheduledJobHistoryAsync(scheduledJobHistory.ScheduledJobHistoryId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when deleting scheduledJobHistory {scheduledJobHistoryId}", ex);
            }
        }

        public List<ReturnErrorItem> IsScheduledJobHistoryModelValidForCreateAsync(ScheduledJobHistoryUpdateModel scheduledJobHistoryModel)
        {
            return IsScheduledJobHistoryModelValidForAllAsync(true, scheduledJobHistoryModel);
        }

        public List<ReturnErrorItem> IsScheduledJobHistoryModelValidForUpdateAsync(ScheduledJobHistoryUpdateModel scheduledJobHistoryModel)
        {
            return IsScheduledJobHistoryModelValidForAllAsync(false, scheduledJobHistoryModel);
        }

        public List<ReturnErrorItem> IsScheduledJobHistoryModelValidForAllAsync(bool isCreate, ScheduledJobHistoryUpdateModel scheduledJobHistoryModel)
        {
            var result = new List<ReturnErrorItem>();

            if (isCreate)
            {
                if (scheduledJobHistoryModel.ScheduledJobHistoryId > 0)
                {
                    result.Add(new ReturnErrorItem("", "ScheduledJobHistory Identifier is not valid"));
                }
            }
            else
            {
                if (scheduledJobHistoryModel.ScheduledJobHistoryId <= 0)
                {
                    result.Add(new ReturnErrorItem("", "ScheduledJobHistory Unique Identifier is not valid"));
                }
            }

            if (!ValidationUtils.IsGuidValid(scheduledJobHistoryModel.ScheduledJobId))
            {
                result.Add(new ReturnErrorItem("ScheduledJobId", "ScheduledJob cannot be empty"));
            }

            if (!DateUtils.IsValidDate(scheduledJobHistoryModel.PickupDate))
            {
                result.Add(new ReturnErrorItem("PickupDate", $"PickupDate is not a valid date"));
            }

            if (scheduledJobHistoryModel.PickupMachineName != null && scheduledJobHistoryModel.PickupMachineName.Length > ScheduledJobHistoryModel.COLUMN_SIZE_PickupMachineName)
            {
                result.Add(new ReturnErrorItem("PickupMachineName", $"PickupMachineName size should not exceed {ScheduledJobHistoryModel.COLUMN_SIZE_PickupMachineName}"));
            }

            if (scheduledJobHistoryModel.PickupMachineName.IsTrimmedStringNullOrEmpty())
            {
                result.Add(new ReturnErrorItem("PickupMachineName", "PickupMachineName is required "));
            }

            if (!ValidationUtils.IsGuidValid(scheduledJobHistoryModel.PickupProcessId))
            {
                result.Add(new ReturnErrorItem("PickupProcessId", "PickupProcess cannot be empty"));
            }

            return result.Count > 0 ? result : null;
        }
        #endregion

        #region ScheduledJobNotificationEmail

        public async Task<ReturnListModel<ScheduledJobNotificationEmailModel>> GetScheduledJobNotificationEmailsAsync(ScheduledJobNotificationEmailSearchCriteria search)
        {
            try
            {
                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanViewScheduledJobNotificationEmailPermissionParamsAsync();

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnListModel<ScheduledJobNotificationEmailModel>(errorStr);
                //}

                //if (canAssign)
                //{
                //    search.CompanyId = await currentSessionService.GetCurrentCompanyIdAsync();
                //    //search.TenantEntityId = await currentSessionService.GetCurrentTenantEntityIdAsNullableAsync();
                //}

                return this.ConvertToList(await reportRepository.GetScheduledJobNotificationEmailsAsync(search), objectMapperService.ScheduledJobNotificationEmailViewListToScheduledJobNotificationEmailModelList);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<ScheduledJobNotificationEmailModel>($"Unexpected error when getting ScheduledJobNotificationEmails", ex);
            }
        }

        public Task<ReturnModel<ScheduledJobNotificationEmailModel>> GetScheduledJobNotificationEmailByIdForReadAsync(long scheduledJobNotificationEmailId)
        {
            return GetScheduledJobNotificationEmailByIdInternalAsync(forWrite: false, scheduledJobNotificationEmailId);
        }

        public Task<ReturnModel<ScheduledJobNotificationEmailModel>> GetScheduledJobNotificationEmailByIdForWriteAsync(long scheduledJobNotificationEmailId)
        {
            return GetScheduledJobNotificationEmailByIdInternalAsync(forWrite: true, scheduledJobNotificationEmailId);
        }

        public async Task<ReturnModel<ScheduledJobNotificationEmailModel>> GetScheduledJobNotificationEmailByIdInternalAsync(bool forWrite, long scheduledJobNotificationEmailId)
        {
            try
            {
                var item = await reportRepository.GetScheduledJobNotificationEmailViewByIdAsync(scheduledJobNotificationEmailId);
                if (item == null)
                {
                    return log.ErrorAndReturnModel<ScheduledJobNotificationEmailModel>($"Unable to find the scheduledJobNotificationEmail", new { scheduledJobNotificationEmailId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanViewScheduledJobNotificationEmailPermissionParamsAsync(item.CompanyId, item.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<ScheduledJobNotificationEmailModel>(errorStr);
                //}

                return this.ConvertToItem(item, objectMapperService.ScheduledJobNotificationEmailViewToScheduledJobNotificationEmailModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<ScheduledJobNotificationEmailModel>($"Unexpected error when getting scheduledJobNotificationEmail", new { scheduledJobNotificationEmailId }, ex);
            }
        }

        public async Task<ReturnModel<ScheduledJobNotificationEmailModel>> CreateScheduledJobNotificationEmailAsync(ScheduledJobNotificationEmailCreateModel scheduledJobNotificationEmail)
        {
            try
            {
                if (scheduledJobNotificationEmail == null)
                {
                    return log.ErrorAndReturnModel<ScheduledJobNotificationEmailModel>($"Unable to create ScheduledJobNotificationEmail", new ReturnErrorItem("", "ScheduledJobNotificationEmail information is not valid"));
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.CanCreateScheduledJobNotificationEmailAsync();

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<ScheduledJobNotificationEmailModel>(errorStr);
                //}

                //if (canAssign)
                //{
                //    scheduledJobNotificationEmail.CompanyId = await currentSessionService.GetCurrentTenantIdAsync();
                //    scheduledJobNotificationEmail.TenantEntityId = await currentSessionService.GetCurrentTenantEntityIdAsNullableAsync();
                //}

                var errorList = IsScheduledJobNotificationEmailModelValidForCreateAsync(scheduledJobNotificationEmail);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<ScheduledJobNotificationEmailModel>($"Unable to create scheduledJobNotificationEmail", errorList);
                }

                scheduledJobNotificationEmail.IsDeleted = false;

                var item = objectMapperService.ScheduledJobNotificationEmailCreateModelToScheduledJobNotificationEmail(scheduledJobNotificationEmail);

                var scheduledJobItem = await reportRepository.GetScheduledJobViewByIdAsync(scheduledJobNotificationEmail.ScheduledJobId);
                if (scheduledJobItem == null)
                {
                    return log.ErrorAndReturnModel<ScheduledJobNotificationEmailModel>($"Unable to Get the ScheduledJob Information", new { scheduledJobNotificationEmail.ScheduledJobId });
                }

                item.ScheduledJobId = scheduledJobItem.ScheduledJobId;

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    item = await reportRepository.CreateScheduledJobNotificationEmailAsync(item, await currentSessionService.GetCurrentUserIdAsync());
                    if (item == null)
                    {
                        throw new Exception($"Unable to create scheduledJobNotificationEmail");
                    }

                    await trx.CompleteAsync();
                }

                // await auditService.ScheduledJobNotificationEmailCreatedAsync(item.ScheduledJobNotificationEmailId);

                var view = await reportRepository.GetScheduledJobNotificationEmailViewByIdAsync(item.ScheduledJobNotificationEmailId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<ScheduledJobNotificationEmailModel>($"Unable to find the scheduledJobNotificationEmail", new { item.ScheduledJobNotificationEmailId });
                }

                return this.ConvertToItem(view, objectMapperService.ScheduledJobNotificationEmailViewToScheduledJobNotificationEmailModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModelWithRollbackExceptionHandler<ScheduledJobNotificationEmailModel>($"Unexpected error when creating ScheduledJobNotificationEmail {scheduledJobNotificationEmail?.ScheduledJobNotificationEmailId}", ex);
            }
        }

        public async Task<ReturnModel<ScheduledJobNotificationEmailModel>> UpdateScheduledJobNotificationEmailAsync(ScheduledJobNotificationEmailUpdateModel scheduledJobNotificationEmail)
        {
            try
            {
                if (scheduledJobNotificationEmail == null)
                {
                    return log.ErrorAndReturnModel<ScheduledJobNotificationEmailModel>($"Unable to update ScheduledJobNotificationEmail", new ReturnErrorItem("", "ScheduledJobNotificationEmail information is not valid"));
                }

                var existingScheduledJobNotificationEmail = await reportRepository.GetScheduledJobNotificationEmailViewFromAnyByIdAsync(scheduledJobNotificationEmail.ScheduledJobNotificationEmailId);
                if (existingScheduledJobNotificationEmail == null)
                {
                    return log.ErrorAndReturnModel<ScheduledJobNotificationEmailModel>($"Unable to find the scheduledJobNotificationEmail", new { scheduledJobNotificationEmail.ScheduledJobNotificationEmailId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanManageScheduledJobNotificationEmailPermissionParamsAsync(existingScheduledJobNotificationEmail.CompanyId, existingScheduledJobNotificationEmail.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<ScheduledJobNotificationEmailModel>(errorStr);
                //}

                //if (canAssign)
                //{
                //    scheduledJobNotificationEmail.CompanyId = await currentSessionService.GetCurrentTenantIdAsync();
                //    //scheduledJobNotificationEmail.TenantEntityId = await currentSessionService.GetCurrentTenantEntityIdAsNullableAsync();
                //}

                scheduledJobNotificationEmail.CreatedByUserId = existingScheduledJobNotificationEmail.CreatedByUserId;
                scheduledJobNotificationEmail.CreatedDate = existingScheduledJobNotificationEmail.CreatedDate;
                scheduledJobNotificationEmail.IsDeleted = existingScheduledJobNotificationEmail.IsDeleted;
                //scheduledJobNotificationEmail.CompanyId = existingScheduledJobNotificationEmail.CompanyId;

                var errorList = IsScheduledJobNotificationEmailModelValidForUpdateAsync(scheduledJobNotificationEmail);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<ScheduledJobNotificationEmailModel>($"Unable to update scheduledJobNotificationEmail", errorList);
                }

                var item = objectMapperService.ScheduledJobNotificationEmailUpdateModelToScheduledJobNotificationEmail(scheduledJobNotificationEmail);

                var scheduledJobItem = await reportRepository.GetScheduledJobViewByIdAsync(scheduledJobNotificationEmail.ScheduledJobId);
                if (scheduledJobItem == null)
                {
                    return log.ErrorAndReturnModel<ScheduledJobNotificationEmailModel>($"Unable to Get the ScheduledJob Information", new { scheduledJobNotificationEmail.ScheduledJobId });
                }

                item.ScheduledJobId = scheduledJobItem.ScheduledJobId;

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await reportRepository.UpdateScheduledJobNotificationEmailAsync(item, await currentSessionService.GetCurrentUserIdAsync());
                    if (result == false)
                    {
                        throw new RollbackException($"Unable to update scheduledJobNotificationEmail");
                    }

                    await trx.CompleteAsync();
                }

                // await auditService.ScheduledJobNotificationEmailUpdatedAsync(scheduledJobNotificationEmail.ScheduledJobNotificationEmailId);

                var view = await reportRepository.GetScheduledJobNotificationEmailViewByIdAsync(scheduledJobNotificationEmail.ScheduledJobNotificationEmailId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<ScheduledJobNotificationEmailModel>($"Unable to find the scheduledJobNotificationEmail", new { scheduledJobNotificationEmail.ScheduledJobNotificationEmailId });
                }

                return this.ConvertToItem(view, objectMapperService.ScheduledJobNotificationEmailViewToScheduledJobNotificationEmailModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<ScheduledJobNotificationEmailModel>($"Unexpected error when updating ScheduledJobNotificationEmail {scheduledJobNotificationEmail?.ScheduledJobNotificationEmailId}", ex);
            }
        }

        public async Task<ReturnModel<bool>> MarkScheduledJobNotificationEmailAsNotDeletedByIdAsync(long scheduledJobNotificationEmailId)
        {
            try
            {
                var scheduledJobNotificationEmail = await reportRepository.GetScheduledJobNotificationEmailViewFromAnyByIdAsync(scheduledJobNotificationEmailId);
                if (scheduledJobNotificationEmail == null)
                {
                    return log.ErrorAndReturnModel<bool>($"Unable to find the scheduledJobNotificationEmail", new { scheduledJobNotificationEmailId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanManageScheduledJobNotificationEmailPermissionParamsAsync(scheduledJobNotificationEmail.CompanyId, scheduledJobNotificationEmail.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<bool>(errorStr);
                //}

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await reportRepository.MarkScheduledJobNotificationEmailAsNotDeletedByIdAsync(scheduledJobNotificationEmailId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                // await auditService.ScheduledJobNotificationEmailDeletedAsync(scheduledJobNotificationEmail.ScheduledJobNotificationEmailId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking scheduledJobNotificationEmail as not deleted {scheduledJobNotificationEmailId}", ex);
            }
        }

        public async Task<ReturnModel<bool>> MarkScheduledJobNotificationEmailAsDeletedByIdAsync(long scheduledJobNotificationEmailId)
        {
            try
            {
                var scheduledJobNotificationEmail = await reportRepository.GetScheduledJobNotificationEmailViewFromAnyByIdAsync(scheduledJobNotificationEmailId);
                if (scheduledJobNotificationEmail == null)
                {
                    return log.ErrorAndReturnModel<bool>($"Unable to find the scheduledJobNotificationEmail", new { scheduledJobNotificationEmailId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanManageScheduledJobNotificationEmailPermissionParamsAsync(scheduledJobNotificationEmail.CompanyId, scheduledJobNotificationEmail.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<bool>(errorStr);
                //}

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await reportRepository.MarkScheduledJobNotificationEmailAsDeletedByIdAsync(scheduledJobNotificationEmailId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                // await auditService.ScheduledJobNotificationEmailDeletedAsync(scheduledJobNotificationEmail.ScheduledJobNotificationEmailId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking scheduledJobNotificationEmail as deleted {scheduledJobNotificationEmailId}", ex);
            }
        }

        public async Task<ReturnModel<bool>> HardDeleteScheduledJobNotificationEmailByAsync(long scheduledJobNotificationEmailId)
        {
            try
            {
                var scheduledJobNotificationEmail = await reportRepository.GetScheduledJobNotificationEmailViewFromAnyByIdAsync(scheduledJobNotificationEmailId);
                if (scheduledJobNotificationEmail == null)
                {
                    return log.ErrorAndReturnModel<bool>($"Unable to find the Scheduled Job Notification Email", new { scheduledJobNotificationEmailId });
                }

                //var (resultValue, canAssign, errorStr) = await permissionService.GetCanManageScheduledJobNotificationEmailPermissionParamsAsync(scheduledJobNotificationEmail.CompanyId, scheduledJobNotificationEmail.TenantEntityId);

                //if (resultValue == false)
                //{
                //    return log.ErrorAndReturnModel<bool>(errorStr);
                //}

                bool result;

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await reportRepository.HardDeleteScheduledJobNotificationEmailByIdAsync(scheduledJobNotificationEmailId);
                    await trx.CompleteAsync();
                }

                // await auditService.HardDeleteScheduledJobNotificationEmailAsync(scheduledJobNotificationEmail.ScheduledJobNotificationEmailId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when deleting Scheduled Job Notification Email", new { scheduledJobNotificationEmailId }, ex);
            }
        }

        public List<ReturnErrorItem> IsScheduledJobNotificationEmailModelValidForCreateAsync(ScheduledJobNotificationEmailUpdateModel scheduledJobNotificationEmailModel)
        {
            return IsScheduledJobNotificationEmailModelValidForAllAsync(true, scheduledJobNotificationEmailModel);
        }

        public List<ReturnErrorItem> IsScheduledJobNotificationEmailModelValidForUpdateAsync(ScheduledJobNotificationEmailUpdateModel scheduledJobNotificationEmailModel)
        {
            return IsScheduledJobNotificationEmailModelValidForAllAsync(false, scheduledJobNotificationEmailModel);
        }

        public List<ReturnErrorItem> IsScheduledJobNotificationEmailModelValidForAllAsync(bool isCreate, ScheduledJobNotificationEmailUpdateModel scheduledJobNotificationEmailModel)
        {
            var result = new List<ReturnErrorItem>();

            if (isCreate)
            {
                if (scheduledJobNotificationEmailModel.ScheduledJobNotificationEmailId > 0)
                {
                    result.Add(new ReturnErrorItem("", "Scheduled Job Notification Email Identifier is not valid"));
                }
            }
            else
            {
                if (scheduledJobNotificationEmailModel.ScheduledJobNotificationEmailId <= 0)
                {
                    result.Add(new ReturnErrorItem("", "Scheduled Job Notification Email Unique Identifier is not valid"));
                }
            }

            if (!ValidationUtils.IsGuidValid(scheduledJobNotificationEmailModel.ScheduledJobId))
            {
                result.Add(new ReturnErrorItem("ScheduledJobId", "Scheduled Job cannot be empty"));
            }

            if (scheduledJobNotificationEmailModel.Email != null && scheduledJobNotificationEmailModel.Email.Length > ScheduledJobNotificationEmailModel.COLUMN_SIZE_Email)
            {
                result.Add(new ReturnErrorItem("Email", $"Email size should not exceed {ScheduledJobNotificationEmailModel.COLUMN_SIZE_Email}"));
            }

            if (scheduledJobNotificationEmailModel.Email.IsTrimmedStringNullOrEmpty())
            {
                result.Add(new ReturnErrorItem("Email", "Email is required "));
            }

            if (!ValidationUtils.IsEmail(scheduledJobNotificationEmailModel.Email))
            {
                result.Add(new ReturnErrorItem("Email", "Email is not valid."));
            }

            if (scheduledJobNotificationEmailModel.Message != null && scheduledJobNotificationEmailModel.Message.Length > ScheduledJobNotificationEmailModel.COLUMN_SIZE_Message)
            {
                result.Add(new ReturnErrorItem("Message", $"Message size should not exceed {ScheduledJobNotificationEmailModel.COLUMN_SIZE_Message}"));
            }

            return result.Count > 0 ? result : null;
        }
        #endregion
    }
}
