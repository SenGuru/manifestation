using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Manifest.Common.Enums;
using Manifest.Common.Interfaces.Service.App;
using Manifest.Common.Models.DB;
using Manifest.Common.Models.DBView;
using Manifest.Common.Models.Queue;
using Manifest.Common.Models.ViewModels;
using Manifest.Repository.Common;
using Manifest.Service.ObjectMapping;
using Framework.Infrastructure.Exceptions;
using Framework.Infrastructure.Interfaces.DbAccess;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Utils;
using MassTransit;

namespace Manifest.Service.App
{
    public partial class ReportCreationService : BaseAppService
    {
        private readonly ReportRepository reportRepository;
        private readonly ITransactionManager transactionManager;
        private readonly ICurrentSessionService currentSessionService;
        private readonly IBusControl busControl;

        public ReportCreationService(ILog log, ObjectMapperService objectMapperService, ReportRepository reportRepository, CompanyRepository companyRepository, ITransactionManager transactionManager, ICurrentSessionService currentSessionService, PermissionService permissionService, IBusControl busControl)
            : base(log, objectMapperService)
        {
            this.reportRepository = reportRepository;
            this.transactionManager = transactionManager;
            this.currentSessionService = currentSessionService;
            this.busControl = busControl;
        }

        #region Job
        public async Task<ReturnModel<bool>> IsJobParameterValidAsync(Job job)
        {
            if (await reportRepository.GetJobTemplateByIdAsync(job.JobTemplateId) == null)
            {
                return log.ErrorAndReturnModel<bool>($"Unable to Get the JobTemplate Information", new { job.JobTemplateId });
            }

            if (job.ExecutableName != null && job.ExecutableName.Length > JobModel.COLUMN_SIZE_ExecutableName)
            {
                return log.ErrorAndReturnModel<bool>("ExecutableName", $"ExecutableName size should not exceed {JobModel.COLUMN_SIZE_ExecutableName}");
            }

            if (job.CustomExportName != null && job.CustomExportName.Length > JobModel.COLUMN_SIZE_CustomExportName)
            {
                return log.ErrorAndReturnModel<bool>("CustomExportName", $"CustomExportName size should not exceed {JobModel.COLUMN_SIZE_CustomExportName}");
            }

            if (job.JobName != null && job.JobName.Length > JobModel.COLUMN_SIZE_JobName)
            {
                return log.ErrorAndReturnModel<bool>("JobName", $"JobName size should not exceed {JobModel.COLUMN_SIZE_JobName}");
            }

            if (job.ExecutionAppName != null && job.ExecutionAppName.Length > JobModel.COLUMN_SIZE_ExecutionAppName)
            {
                return log.ErrorAndReturnModel<bool>("ExecutionAppName", $"ExecutionAppName size should not exceed {JobModel.COLUMN_SIZE_ExecutionAppName}");
            }

            if (job.ExecutionAppId != null && job.ExecutionAppId.Length > JobModel.COLUMN_SIZE_ExecutionAppId)
            {
                return log.ErrorAndReturnModel<bool>("ExecutionAppId", $"ExecutionAppId size should not exceed {JobModel.COLUMN_SIZE_ExecutionAppId}");
            }

            if (job.ExecutionHostName != null && job.ExecutionHostName.Length > JobModel.COLUMN_SIZE_ExecutionHostName)
            {
                return log.ErrorAndReturnModel<bool>("ExecutionHostName", $"ExecutionHostName size should not exceed {JobModel.COLUMN_SIZE_ExecutionHostName}");
            }

            return ReturnModel<bool>.Success(true);
        }

        public List<ReturnErrorItem> IsJobNotificationEmailModelValidForCreateAsync(JobNotificationEmailUpdateModel jobNotificationEmailModel)
        {
            return IsJobNotificationEmailModelValidForAllAsync(true, jobNotificationEmailModel);
        }

        public List<ReturnErrorItem> IsJobNotificationEmailModelValidForUpdateAsync(JobNotificationEmailUpdateModel jobNotificationEmailModel)
        {
            return IsJobNotificationEmailModelValidForAllAsync(false, jobNotificationEmailModel);
        }

        public List<ReturnErrorItem> IsJobNotificationEmailModelValidForAllAsync(bool isCreate, JobNotificationEmailUpdateModel jobNotificationEmailModel)
        {
            var result = new List<ReturnErrorItem>();

            if (isCreate)
            {
                if (jobNotificationEmailModel.JobNotificationEmailId > 0)
                {
                    result.Add(new ReturnErrorItem("", "JobNotificationEmail Identifier is not valid"));
                }
            }
            else
            {
                if (jobNotificationEmailModel.JobNotificationEmailId <= 0)
                {
                    result.Add(new ReturnErrorItem("", "JobNotificationEmail Unique Identifier is not valid"));
                }
            }

            if (!ValidationUtils.IsGuidValid(jobNotificationEmailModel.JobId))
            {
                result.Add(new ReturnErrorItem("JobId", "Job cannot be empty"));
            }

            if (jobNotificationEmailModel.Email != null && jobNotificationEmailModel.Email.Length > JobNotificationEmailModel.COLUMN_SIZE_Email)
            {
                result.Add(new ReturnErrorItem("Email", $"Email size should not exceed {JobNotificationEmailModel.COLUMN_SIZE_Email}"));
            }

            if (jobNotificationEmailModel.Email == null)
            {
                result.Add(new ReturnErrorItem("Email", "Email is required "));
            }

            if (!ValidationUtils.IsEmail(jobNotificationEmailModel.Email))
            {
                result.Add(new ReturnErrorItem("Email", "Email is not valid."));
            }

            if (jobNotificationEmailModel.Message != null && jobNotificationEmailModel.Message.Length > JobNotificationEmailModel.COLUMN_SIZE_Message)
            {
                result.Add(new ReturnErrorItem("Message", $"Message size should not exceed {JobNotificationEmailModel.COLUMN_SIZE_Message}"));
            }

            return result.Count > 0 ? result : null;
        }

        #endregion

        #region Test

        public Task<ReturnModel<JobModel>> CreateJobFromSavedJobFromJobServerAsync(long savedJobId)
        {
            return CreateJobFromSavedJobInternalAsync(savedJobId, useCurrentSession: false);
        }

        public Task<ReturnModel<JobModel>> CreateJobFromSavedJobAsync(long savedJobId)
        {
            return CreateJobFromSavedJobInternalAsync(savedJobId, useCurrentSession: true);
        }

        public async Task<ReturnModel<JobModel>> CreateJobFromSavedJobInternalAsync(long savedJobId, bool useCurrentSession)
        {
            try
            {
                var savedJob = await reportRepository.GetSavedJobByIdAsync(savedJobId);
                if (savedJob == null)
                {
                    return log.ErrorAndReturnModel<JobModel>($"Unable to find the Saved Job", new { savedJobId });
                }

                var job = new Job
                {
                    JobName = savedJob.JobName,
                    PredefinedJobStatusTypeId = (int)PredefinedJobStatusTypes.Submitted,
                    ExecutionAppCheckPort = 0,
                    ExecutionAppId = "",
                    ExecutionAppName = "",
                    ExecutionHostName = "",
                    ExecutionProcessId = 0,
                    ExecutionThreadId = 0,
                    ReportTemplateFile = savedJob.ReportTemplateFile,
                    PredefinedJobTypeId = (int)savedJob.PredefinedJobTypeId,
                    JobTemplateId = savedJob.JobTemplateId,
                    PredefinedJobOrReportTypeId = savedJob.PredefinedJobTypeId,
                    CompletedDate = savedJob.CompletedDate,
                    SubmittedDate = savedJob.SubmittedDate,
                    SubmittedByUserId = savedJob.SubmittedByUserId,
                    ExecutableFolder = savedJob.ExecutableFolder ?? "",
                    ExecutableName = savedJob.ExecutableName ?? "",
                    SerializedJobParameter = savedJob.SerializedJobParameter ?? "",
                    ExecutableParameter = savedJob.ExecutableParameter ?? "",
                    ExportDoc = savedJob.ExportDoc,
                    ExportXls = savedJob.ExportXls,
                    ExportPpt = savedJob.ExportPpt,
                    ExportCsv = savedJob.ExportCsv,
                    ExportXml = savedJob.ExportXml,
                    ExportPdf = savedJob.ExportPdf,
                    ExportCustom = savedJob.ExportCustom,
                    CustomExportName = savedJob.CustomExportName ?? "",
                    PickupDate = savedJob.PickupDate,
                    IncludeDataSourceFile = savedJob.IncludeDataSourceFile,
                    IsDeleted = false
                };

                return await CreateJobAndSendToQueueInternalAsync(job);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<JobModel>($"Unexpected error when getting Jobs", ex);
            }
        }

        public Task<ReturnModel<JobModel>> CreateJobFromJobFromJobServerAsync(long jobId)
        {
            return CreateJobFromJobInternalAsync(jobId, useCurrentSession: false);
        }

        public Task<ReturnModel<JobModel>> CreateJobFromJobAsync(long jobId)
        {
            return CreateJobFromJobInternalAsync(jobId, useCurrentSession: true);
        }

        public async Task<ReturnModel<JobModel>> CreateJobFromJobInternalAsync(long jobId, bool useCurrentSession)
        {
            try
            {
                var oldJob = await reportRepository.GetJobByIdAsync(jobId);
                if (oldJob == null)
                {
                    return log.ErrorAndReturnModel<JobModel>($"Unable to find the Original Job", new { jobId });
                }

                var job = new Job
                {
                    JobName = oldJob.JobName,
                    PredefinedJobStatusTypeId = (int)PredefinedJobStatusTypes.Submitted,
                    ExecutionAppCheckPort = 0,
                    ExecutionAppId = "",
                    ExecutionAppName = "",
                    ExecutionHostName = "",
                    ExecutionProcessId = 0,
                    ExecutionThreadId = 0,
                    PredefinedJobTypeId = (int)oldJob.PredefinedJobTypeId,
                    JobTemplateId = oldJob.JobTemplateId,
                    PredefinedJobOrReportTypeId = oldJob.PredefinedJobTypeId,
                    CompletedDate = oldJob.CompletedDate,
                    SubmittedDate = oldJob.SubmittedDate,
                    SubmittedByUserId = oldJob.SubmittedByUserId,
                    ExecutableFolder = oldJob.ExecutableFolder ?? "",
                    ExecutableName = oldJob.ExecutableName ?? "",
                    ExecutableParameter = oldJob.ExecutableParameter ?? "",
                    SerializedJobParameter = oldJob.SerializedJobParameter ?? "",
                    ReportTemplateFile = oldJob.ReportTemplateFile ?? "",
                    ExportDoc = oldJob.ExportDoc,
                    ExportXls = oldJob.ExportXls,
                    ExportPpt = oldJob.ExportPpt,
                    ExportCsv = oldJob.ExportCsv,
                    ExportXml = oldJob.ExportXml,
                    ExportPdf = oldJob.ExportPdf,
                    ExportCustom = oldJob.ExportCustom,
                    CustomExportName = oldJob.CustomExportName,
                    PickupDate = oldJob.PickupDate,
                    IncludeDataSourceFile = oldJob.IncludeDataSourceFile,
                    IsDeleted = false
                };

                return await CreateJobAndSendToQueueInternalAsync(job);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<JobModel>($"Unexpected error when getting Jobs", ex);
            }
        }

        public Task<ReturnModel<JobModel>> CreateJobFromScheduledJobFromJobServerAsync(long scheduleJobId, object parameter = null)
        {
            return CreateJobFromScheduledJobInternalAsync(scheduleJobId, useCurrentSession: false, parameter: parameter);
        }

        public async Task<ReturnModel<JobModel>> CreateJobFromScheduledJobByPredefinedJobTypeAsync(int predefinedJobTypeId)
        {
            var scheduledJob = await reportRepository.GetScheduledJobViewFromPredefinedJobTypeIdAsync(predefinedJobTypeId);
            if (scheduledJob == null)
                return null;

            return await CreateJobFromScheduledJobInternalAsync(scheduledJob.ScheduledJobId, useCurrentSession: false, null);
        }

        public Task<ReturnModel<JobModel>> CreateJobFromScheduledJobAsync(long scheduleJobId, object parameter = null)
        {
            return CreateJobFromScheduledJobInternalAsync(scheduleJobId, useCurrentSession: true, parameter: parameter);
        }

        public async Task<ReturnModel<JobModel>> CreateJobFromScheduledJobByJobTypeAsync(PredefinedJobTypes predefinedJobType, object customParam = null)
        {
            try
            {
                var scheduleJob = await reportRepository.GetScheduledJobByPredefinedJobTypeAsync((int)predefinedJobType);
                if (scheduleJob == null)
                {
                    return log.ErrorAndReturnModel<JobModel>($"Unable to find the Scheduled Job", new { predefinedJobType });
                }

                var scheduleJobId = scheduleJob.ScheduledJobId;
                object parameter = null;
                if (customParam != null)
                {
                    parameter = customParam;
                }
                else
                {
                    if (!await currentSessionService.IsCurrentUserASuperAdminAsync())
                    {
                        parameter = new { };
                    }
                }

                return await CreateJobFromScheduledJobInternalAsync(scheduleJobId, useCurrentSession: true, parameter: parameter);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<JobModel>($"Unexpected error when getting creating background Job", new { predefinedJobType }, ex);
            }
        }

        public async Task<ReturnModel<JobModel>> CreateJobFromScheduledJobInternalAsync(long scheduleJobId, bool useCurrentSession, object parameter = null)
        {
            try
            {
                var scheduleJob = await reportRepository.GetScheduledJobByIdAsync(scheduleJobId);
                if (scheduleJob == null)
                {
                    return log.ErrorAndReturnModel<JobModel>($"Unable to find the Scheduled Job", new { scheduleJobId });
                }

                //var jobTemplate = await reportRepository.GetJobTemplateByIdAsync(scheduleJob.JobTemplateId);

                var job = new Job
                {
                    JobName = scheduleJob.JobName,
                    ReportTemplateFile = scheduleJob.ReportTemplateFile,
                    PredefinedJobStatusTypeId = (int)PredefinedJobStatusTypes.Submitted,
                    ExecutionAppCheckPort = 0,
                    ExecutionAppId = "",
                    ExecutionAppName = "",
                    ExecutionHostName = "",
                    ExecutionProcessId = 0,
                    ExecutionThreadId = 0,
                    JobTemplateId = scheduleJob.JobTemplateId,
                    PredefinedJobTypeId = scheduleJob.PredefinedJobTypeId,
                    SubmittedDate = DateTime.UtcNow,
                    SubmittedByUserId = scheduleJob.SubmittedByUserId,
                    ExecutableFolder = scheduleJob.ExecutableFolder ?? "",
                    ExecutableName = scheduleJob.ExecutableName ?? "",
                    SerializedJobParameter = parameter != null ? JsonUtils.Serialize(parameter) : scheduleJob.SerializedJobParameter ?? "",
                    ExecutableParameter = scheduleJob.ExecutableParameter ?? "",
                    ExportDoc = scheduleJob.ExportDoc,
                    ExportXls = scheduleJob.ExportXls,
                    ExportPpt = scheduleJob.ExportPpt,
                    ExportCsv = scheduleJob.ExportCsv,
                    ExportXml = scheduleJob.ExportXml,
                    ExportPdf = scheduleJob.ExportPdf,
                    ExportCustom = scheduleJob.ExportCustom,
                    CustomExportName = scheduleJob.CustomExportName ?? "",
                    PickupDate = scheduleJob.PickupDate,
                    PickupTime = scheduleJob.PickupTime,
                    IncludeDataSourceFile = scheduleJob.IncludeDataSourceFile,
                    IsDeleted = false
                };

                return await CreateJobAndSendToQueueInternalAsync(job);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<JobModel>($"Unexpected error when getting Jobs", ex);
            }
        }

        public Task<ReturnModel<JobModel>> CreateJobFromJobTemplateFromJobServerAsync(JobCreateModel jobTemplateJob)
        {
            return CreateJobFromJobTemplateInternalAsync(jobTemplateJob, useCurrentSession: false);
        }

        public Task<ReturnModel<JobModel>> CreateJobFromJobTemplateAsync(JobCreateModel jobTemplateJob)
        {
            return CreateJobFromJobTemplateInternalAsync(jobTemplateJob, useCurrentSession: true);
        }

        public async Task<ReturnModel<JobModel>> CreateJobFromJobTemplateInternalAsync(JobCreateModel jobTemplateJob, bool useCurrentSession)
        {
            try
            {
                var jobTemplate = await reportRepository.GetJobTemplateByIdAsync(jobTemplateJob.JobTemplateId);
                if (jobTemplate == null)
                {
                    return log.ErrorAndReturnModel<JobModel>($"Unable to find the Job template", new { jobTemplateJob.JobTemplateId });
                }

                if (!jobTemplateJob.Emails.IsNullOrEmpty())
                {
                    foreach (var item in jobTemplateJob.Emails)
                    {
                        var lst = IsJobNotificationEmailModelValidForCreateAsync(item);
                        if (lst?.Count > 0)
                        {
                            return log.ErrorAndReturnModel<JobModel>(lst);
                        }
                    }
                }

                var job = new Job
                {
                    JobName = jobTemplateJob.JobName,
                    PredefinedJobStatusTypeId = (int)PredefinedJobStatusTypes.Submitted,
                    ExecutionAppCheckPort = 0,
                    ExecutionAppId = "",
                    ExecutionAppName = "",
                    ExecutionHostName = "",
                    ExecutionProcessId = 0,
                    ExecutionThreadId = 0,
                    ReportTemplateFile = jobTemplate.ReportTemplateFile,
                    PredefinedJobTypeId = jobTemplate.PredefinedJobTypeId,
                    JobTemplateId = jobTemplateJob.JobTemplateId,
                    PredefinedJobOrReportTypeId = jobTemplateJob.PredefinedJobTypeId,
                    CompletedDate = jobTemplateJob.CompletedDate,
                    SubmittedDate = jobTemplateJob.SubmittedDate,
                    SubmittedByUserId = jobTemplateJob.SubmittedByUserId,
                    ExecutableFolder = jobTemplateJob.ExecutableFolder,
                    ExecutableName = jobTemplateJob.ExecutableName,
                    ExecutableParameter = jobTemplateJob.ExecutableParameter,
                    SerializedJobParameter = jobTemplateJob.SerializedJobParameter,
                    ExportDoc = jobTemplateJob.ExportDoc,
                    ExportXls = jobTemplateJob.ExportXls,
                    ExportPpt = jobTemplateJob.ExportPpt,
                    ExportCsv = jobTemplateJob.ExportCsv,
                    ExportXml = jobTemplateJob.ExportXml,
                    ExportPdf = jobTemplateJob.ExportPdf,
                    ExportCustom = jobTemplateJob.ExportCustom,
                    CustomExportName = jobTemplateJob.CustomExportName,
                    PickupDate = jobTemplateJob.PickupDate,
                    IncludeDataSourceFile = jobTemplateJob.IncludeDataSourceFile,
                    IsDeleted = false
                };

                var validationResult = await IsJobParameterValidAsync(job);
                if (!validationResult.IsSuccess)
                    return log.ErrorAndReturnModel<JobModel>(validationResult);

                return await CreateJobAndSendToQueueInternalAsync(job);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<JobModel>($"Unexpected error when getting Jobs", ex);
            }
        }

        public async Task<ReturnModel<JobModel>> CreateJobAndSendToQueueInternalAsync(Job job, List<JobNotificationEmail> emails = null)
        {
            try
            {
                var isValid = await IsJobParameterValidAsync(job);

                if (!isValid.IsSuccess)
                    return log.ErrorAndReturnModel<JobModel>(isValid);

                Job newJob;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    newJob = await reportRepository.CreateJobAsync(job, null);
                    if (emails?.Count > 0)
                    {
                        await reportRepository.CreateJobNotificationEmailsAsync(emails, null);
                    }

                    await trx.CompleteAsync();
                }

                var jobView = await reportRepository.GetJobViewByIdAsync(newJob.JobId);

                if (jobView == null)
                {
                    return log.ErrorAndReturnModel<JobModel>($"Unable to find the Job", new { newJob.JobId });
                }

                await busControl.Publish<JobServerMessageQueueModel>(new JobServerMessageQueueModel { JobId = jobView.JobId });

                return this.ConvertToItem(jobView, objectMapperService.JobViewToJobModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<JobModel>($"Unexpected error when getting Jobs", ex);
            }
        }

        #endregion
    }
}
