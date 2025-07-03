using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manifest.Common.Enums;
using Manifest.Common.Interfaces.Repository.DbAccess;
using Manifest.Common.Models.DB;
using Manifest.Common.Models.DBView;
using Manifest.Common.Models.SearchCriteria;
using Framework.Data.Utils;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Utils;
using LinqToDB;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Manifest.Repository.Common
{
    public partial class ReportRepository : BaseRepository
    {
        public ReportRepository(IAppDBManager dbManager)
            : base(dbManager)
        {
        }

        #region Job

        public IQueryable<JobView> GetJobsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<Job>()
                   join a in appDBManager.GetTable<EnumType>() on item.PredefinedJobTypeId equals a.EnumTypeId
                   join b in appDBManager.GetTable<JobTemplate>() on item.JobTemplateId equals b.JobTemplateId
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new JobView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,

                       ReportTemplateFile = item.ReportTemplateFile,
                       PredefinedJobTypeId = item.PredefinedJobTypeId,
                       PredefinedJobTypeName = a.EnumTypeName,
                       JobId = item.JobId,
                       PredefinedJobOrReportTypeId = item.PredefinedJobTypeId,
                       JobTemplateId = item.JobTemplateId,
                       CompletedDate = item.CompletedDate,
                       SubmittedDate = item.SubmittedDate,
                       Message = item.Message,
                       PredefinedJobStatusTypeId = item.PredefinedJobStatusTypeId,
                       SubmittedByUserId = item.SubmittedByUserId,
                       ExecutableFolder = item.ExecutableFolder,
                       ExecutableName = item.ExecutableName,
                       ExecutableParameter = item.ExecutableParameter,
                       SerializedJobParameter = item.SerializedJobParameter,
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

        public async Task<DbReturnListModel<JobView>> GetJobsInternalAsync(JobSearchCriteria search)
        {
            var sql1 = GetJobsViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.JobId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.Message.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.ExecutableName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.ExecutableParameter.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.CustomExportName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.JobName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.ExecutionAppId.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.ExecutionHostName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper()));
            }

            if (search.PredefinedJobTypeIds != null && search.PredefinedJobTypeIds.Count > 0)
                sql1 = sql1.Where(x => search.PredefinedJobTypeIds.Contains(x.PredefinedJobTypeId ?? 0));

            if (search.PredefinedJobStatusTypeIds != null && search.PredefinedJobStatusTypeIds.Count > 0)
                sql1 = sql1.Where(x => search.PredefinedJobStatusTypeIds.Contains(x.PredefinedJobStatusTypeId));

            if (search.JobTemplateId != null && search.JobTemplateId > 0)
                sql1 = sql1.Where(x => search.JobTemplateId == x.JobTemplateId);

            if (search.SubmittedByUserId != null && search.SubmittedByUserId > 0)
                sql1 = sql1.Where(x => search.SubmittedByUserId == x.SubmittedByUserId);

            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            if (search.StartDate.IsValidDate() && search.EndDate.IsValidDate())
            {
                if (search.StartDate.Value <= search.EndDate.Value)
                {
                    sql1 = sql1.Where(x => x.SubmittedDate >= search.StartDate.Value.StartOfDay() && x.SubmittedDate <= search.EndDate.Value.EndOfDay());
                }
            }
            else if (search.StartDate.IsValidDate())
            {
                sql1 = sql1.Where(x => x.SubmittedDate >= search.StartDate.Value.StartOfDay());
            }
            else if (search.EndDate.IsValidDate())
            {
                sql1 = sql1.Where(x => x.SubmittedDate <= search.EndDate.Value.EndOfDay());
            }
            else
            {
                var today = DateTime.Now;
                var todayMinus30 = today.AddDays(-30);
                sql1 = sql1.Where(x => x.SubmittedDate >= todayMinus30.StartOfDay() && x.SubmittedDate <= today.EndOfDay());
            }

            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.JobId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetJobsViewAsIQueryable().FirstOrDefaultAsync(x => x.JobId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public async Task<bool> SetJobStatusAsync(long jobId, PredefinedJobStatusTypes status, string message = null, DateTime? startTime = null, DateTime? endTime = null)
        {
            var sql = appDBManager.GetTable<Job>().Where(x => x.JobId == jobId).Set(x => x.PredefinedJobStatusTypeId, (int)status);
            if (message != null)
                sql = sql.Set(x => x.Message, message);

            if (startTime != null)
                sql = sql.Set(x => x.PickupDate, startTime);

            if (endTime != null)
                sql = sql.Set(x => x.CompletedDate, endTime);

            return await sql.UpdateWithAuditAsync(null) > 0;
        }

        public Task<bool> SetJobStatusAsRunningAsync(long jobId, string message = null)
        {
            return SetJobStatusAsync(jobId, PredefinedJobStatusTypes.Running, message, startTime: DateTime.UtcNow);
        }

        public Task<bool> SetJobStatusAsErrorAsync(long jobId, string message = null)
        {
            return SetJobStatusAsync(jobId, PredefinedJobStatusTypes.Error, message, endTime: DateTime.UtcNow);
        }

        public Task<bool> SetJobStatusAsCompleted(long jobId, string message = null)
        {
            return SetJobStatusAsync(jobId, PredefinedJobStatusTypes.Completed, message, endTime: DateTime.UtcNow);
        }

        public Task<DbReturnListModel<JobView>> GetJobsAsync(JobSearchCriteria search)
        {
            return GetJobsInternalAsync(search);
        }

        public Task<Job> GetJobFromAnyByIdAsync(long jobId)
        {
            return appDBManager.FirstOrDefaultAsync<Job>(x => x.JobId == jobId);
        }

        public Task<JobView> GetJobViewFromAnyByIdAsync(long jobId)
        {
            return GetJobsViewAsIQueryable().FirstOrDefaultAsync(x => x.JobId == jobId);
        }

        public Task<Job> GetJobByIdAsync(long jobId)
        {
            return appDBManager.FirstOrDefaultAsync<Job>(x => x.JobId == jobId && x.IsDeleted == false);
        }

        public Task<JobView> GetJobViewByIdAsync(long jobId)
        {
            var sql1 = GetJobsViewAsIQueryable();

            sql1 = sql1.Where(x => x.JobId == jobId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<Job>> GetAllJobs()
        {
            return appDBManager.SelectAllAsync<Job>();
        }

        public async Task<Job> CreateJobAsync(Job job, long? createdBy)
        {
            job.JobId = await appDBManager.InsertWithAuditAsync<Job, long>(job, createdBy);
            return job;
        }

        public async Task<List<Job>> CreateJobsAsync(List<Job> jobs, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<Job, long>(jobs, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                jobs[i].JobId = lst[i];
            return jobs;
        }

        public Task<bool> UpdateJobAsync(Job job, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<Job>(job, modifiedBy);
        }

        public async Task<bool> MarkJobAsNotDeletedByIdAsync(long jobId, long? modifiedBy)
        {
            return await appDBManager.GetTable<Job>().Where(x => x.JobId == jobId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkJobAsDeletedByIdAsync(long jobId, long? modifiedBy)
        {
            return await appDBManager.GetTable<Job>().Where(x => x.JobId == jobId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteJobByIdAsync(long jobId)
        {
            return await appDBManager.GetTable<Job>().Where(x => x.JobId == jobId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetJobActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.Job);

            return GetActivitiesByExpressionAsync(srch, search);
        }
        #endregion

        #region JobAttachment

        public IQueryable<JobAttachmentView> GetJobAttachmentsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<JobAttachment>()
                   join a in appDBManager.GetTable<Job>() on item.JobId equals a.JobId
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new JobAttachmentView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,
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

        public async Task<DbReturnListModel<JobAttachmentView>> GetJobAttachmentsInternalAsync(JobAttachmentSearchCriteria search)
        {
            var sql1 = GetJobAttachmentsViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.JobAttachmentId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.FileName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.StoredFilePath.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper()));
            }

            if (search.JobId != null && search.JobId > 0)
                sql1 = sql1.Where(x => search.JobId == x.JobId);

            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.JobAttachmentId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetJobAttachmentsViewAsIQueryable().FirstOrDefaultAsync(x => x.JobAttachmentId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<JobAttachmentView>> GetJobAttachmentsAsync(JobAttachmentSearchCriteria search)
        {
            return GetJobAttachmentsInternalAsync(search);
        }

        public Task<JobAttachment> GetJobAttachmentFromAnyByIdAsync(long jobAttachmentId)
        {
            return appDBManager.FirstOrDefaultAsync<JobAttachment>(x => x.JobAttachmentId == jobAttachmentId);
        }

        public Task<JobAttachmentView> GetJobAttachmentViewFromAnyByIdAsync(long jobAttachmentId)
        {
            return GetJobAttachmentsViewAsIQueryable().FirstOrDefaultAsync(x => x.JobAttachmentId == jobAttachmentId);
        }

        public Task<JobAttachment> GetJobAttachmentByIdAsync(long jobAttachmentId)
        {
            return appDBManager.FirstOrDefaultAsync<JobAttachment>(x => x.JobAttachmentId == jobAttachmentId && x.IsDeleted == false);
        }

        public Task<JobAttachmentView> GetJobAttachmentViewByIdAsync(long jobAttachmentId)
        {
            var sql1 = GetJobAttachmentsViewAsIQueryable();

            sql1 = sql1.Where(x => x.JobAttachmentId == jobAttachmentId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<JobAttachment>> GetAllJobAttachments()
        {
            return appDBManager.SelectAllAsync<JobAttachment>();
        }

        public async Task<JobAttachment> CreateJobAttachmentAsync(JobAttachment jobAttachment, long? createdBy)
        {
            jobAttachment.JobAttachmentId = await appDBManager.InsertWithAuditAsync<JobAttachment, long>(jobAttachment, createdBy);
            return jobAttachment;
        }

        public async Task<List<JobAttachment>> CreateJobAttachmentsAsync(List<JobAttachment> jobAttachments, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<JobAttachment, long>(jobAttachments, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                jobAttachments[i].JobAttachmentId = lst[i];
            return jobAttachments;
        }

        public Task<bool> UpdateJobAttachmentAsync(JobAttachment jobAttachment, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<JobAttachment>(jobAttachment, modifiedBy);
        }

        public async Task<bool> MarkJobAttachmentAsNotDeletedByIdAsync(long jobAttachmentId, long? modifiedBy)
        {
            return await appDBManager.GetTable<JobAttachment>().Where(x => x.JobAttachmentId == jobAttachmentId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkJobAttachmentAsDeletedByIdAsync(long jobAttachmentId, long? modifiedBy)
        {
            return await appDBManager.GetTable<JobAttachment>().Where(x => x.JobAttachmentId == jobAttachmentId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteJobAttachmentByIdAsync(long jobAttachmentId)
        {
            return await appDBManager.GetTable<JobAttachment>().Where(x => x.JobAttachmentId == jobAttachmentId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetJobAttachmentActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.JobAttachment);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        #endregion

        #region JobNotificationEmail

        public IQueryable<JobNotificationEmailView> GetJobNotificationEmailsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<JobNotificationEmail>()
                   join a in appDBManager.GetTable<Job>() on item.JobId equals a.JobId
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new JobNotificationEmailView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,
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

        public async Task<DbReturnListModel<JobNotificationEmailView>> GetJobNotificationEmailsInternalAsync(JobNotificationEmailSearchCriteria search)
        {
            var sql1 = GetJobNotificationEmailsViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.JobNotificationEmailId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.Email.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.Message.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper()));
            }

            if (search.JobId != null && search.JobId > 0)
                sql1 = sql1.Where(x => search.JobId == x.JobId);

            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.JobNotificationEmailId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetJobNotificationEmailsViewAsIQueryable().FirstOrDefaultAsync(x => x.JobNotificationEmailId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<JobNotificationEmailView>> GetJobNotificationEmailsAsync(JobNotificationEmailSearchCriteria search)
        {
            return GetJobNotificationEmailsInternalAsync(search);
        }

        public Task<JobNotificationEmail> GetJobNotificationEmailFromAnyByIdAsync(long jobNotificationEmailId)
        {
            return appDBManager.FirstOrDefaultAsync<JobNotificationEmail>(x => x.JobNotificationEmailId == jobNotificationEmailId);
        }

        public Task<JobNotificationEmailView> GetJobNotificationEmailViewFromAnyByIdAsync(long jobNotificationEmailId)
        {
            return GetJobNotificationEmailsViewAsIQueryable().FirstOrDefaultAsync(x => x.JobNotificationEmailId == jobNotificationEmailId);
        }

        public Task<List<JobNotificationEmail>> GetJobNotificationEmailByJobIdAsync(long jobId)
        {
            return appDBManager.SelectAsync<JobNotificationEmail>(x => x.JobId == jobId && x.IsDeleted == false);
        }

        public Task<JobNotificationEmail> GetJobNotificationEmailByIdAsync(long jobNotificationEmailId)
        {
            return appDBManager.FirstOrDefaultAsync<JobNotificationEmail>(x => x.JobNotificationEmailId == jobNotificationEmailId && x.IsDeleted == false);
        }

        public Task<JobNotificationEmailView> GetJobNotificationEmailViewByIdAsync(long jobNotificationEmailId)
        {
            var sql1 = GetJobNotificationEmailsViewAsIQueryable();

            sql1 = sql1.Where(x => x.JobNotificationEmailId == jobNotificationEmailId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<JobNotificationEmail>> GetAllJobNotificationEmails()
        {
            return appDBManager.SelectAllAsync<JobNotificationEmail>();
        }

        public async Task<JobNotificationEmail> CreateJobNotificationEmailAsync(JobNotificationEmail jobNotificationEmail, long? createdBy)
        {
            jobNotificationEmail.JobNotificationEmailId = await appDBManager.InsertWithAuditAsync<JobNotificationEmail, long>(jobNotificationEmail, createdBy);
            return jobNotificationEmail;
        }

        public async Task<List<JobNotificationEmail>> CreateJobNotificationEmailsAsync(List<JobNotificationEmail> jobNotificationEmails, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<JobNotificationEmail, long>(jobNotificationEmails, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                jobNotificationEmails[i].JobNotificationEmailId = lst[i];
            return jobNotificationEmails;
        }

        public Task<bool> UpdateJobNotificationEmailAsync(JobNotificationEmail jobNotificationEmail, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<JobNotificationEmail>(jobNotificationEmail, modifiedBy);
        }

        public async Task<bool> MarkJobNotificationEmailAsNotDeletedByIdAsync(long jobNotificationEmailId, long? modifiedBy)
        {
            return await appDBManager.GetTable<JobNotificationEmail>().Where(x => x.JobNotificationEmailId == jobNotificationEmailId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkJobNotificationEmailAsDeletedByIdAsync(long jobNotificationEmailId, long? modifiedBy)
        {
            return await appDBManager.GetTable<JobNotificationEmail>().Where(x => x.JobNotificationEmailId == jobNotificationEmailId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteJobNotificationEmailByIdAsync(long jobNotificationEmailId)
        {
            return await appDBManager.GetTable<JobNotificationEmail>().Where(x => x.JobNotificationEmailId == jobNotificationEmailId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetJobNotificationEmailActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.JobNotificationEmail);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        #endregion

        #region JobTemplate

        public IQueryable<JobTemplateView> GetJobTemplatesViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<JobTemplate>()
                   join a in appDBManager.GetTable<EnumType>() on item.PredefinedJobTypeId equals a.EnumTypeId
                   join b in appDBManager.GetTable<JobTemplateCategory>() on item.JobTemplateCategoryId equals b.JobTemplateCategoryId
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new JobTemplateView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,
                       //PredefinedJobUsageForTypeId = item.PredefinedJobUsageForTypeId,
                       PredefinedJobTypeId = item.PredefinedJobTypeId,
                       PredefinedJobTypeName = a.EnumTypeName,
                       JobTemplateId = item.JobTemplateId,
                       PredefinedJobOrReportTypeId = item.PredefinedJobTypeId,
                       Description = item.Description,
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
                       //ForUserRoleTypeId = item.PredefinedUserRoleTypeId,
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

        public async Task<DbReturnListModel<JobTemplateView>> GetJobTemplatesInternalAsync(JobTemplateSearchCriteria search)
        {
            var sql1 = GetJobTemplatesViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.JobTemplateId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.Description.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.ExecutableName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.CustomExportName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.TemplateName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.TaskIdentifierName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.ReportTemplateFile.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.DefaultOutputFileName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper()));
            }

            if (search.PredefinedJobTypeIds != null && search.PredefinedJobTypeIds.Count > 0)
                sql1 = sql1.Where(x => search.PredefinedJobTypeIds.Contains(x.PredefinedJobTypeId));

            if (search.JobTemplateCategoryId != null && search.JobTemplateCategoryId > 0)
                sql1 = sql1.Where(x => search.JobTemplateCategoryId == x.JobTemplateCategoryId);

            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.JobTemplateId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetJobTemplatesViewAsIQueryable().FirstOrDefaultAsync(x => x.JobTemplateId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<JobTemplateView>> GetJobTemplatesAsync(JobTemplateSearchCriteria search)
        {
            return GetJobTemplatesInternalAsync(search);
        }

        public Task<JobTemplate> GetJobTemplateFromAnyByIdAsync(long jobTemplateId)
        {
            return appDBManager.FirstOrDefaultAsync<JobTemplate>(x => x.JobTemplateId == jobTemplateId);
        }

        public Task<JobTemplateView> GetJobTemplateViewFromAnyByIdAsync(long jobTemplateId)
        {
            return GetJobTemplatesViewAsIQueryable().FirstOrDefaultAsync(x => x.JobTemplateId == jobTemplateId);
        }

        public Task<JobTemplate> GetJobTemplateByIdAsync(long jobTemplateId)
        {
            return appDBManager.FirstOrDefaultAsync<JobTemplate>(x => x.JobTemplateId == jobTemplateId && x.IsDeleted == false);
        }

        public Task<JobTemplateView> GetJobTemplateViewByIdAsync(long jobTemplateId)
        {
            var sql1 = GetJobTemplatesViewAsIQueryable();

            sql1 = sql1.Where(x => x.JobTemplateId == jobTemplateId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<JobTemplate>> GetAllJobTemplates()
        {
            return appDBManager.SelectAllAsync<JobTemplate>();
        }

        public async Task<JobTemplate> CreateJobTemplateAsync(JobTemplate jobTemplate, long? createdBy)
        {
            jobTemplate.JobTemplateId = await appDBManager.InsertWithAuditAsync<JobTemplate, long>(jobTemplate, createdBy);
            return jobTemplate;
        }

        public async Task<List<JobTemplate>> CreateJobTemplatesAsync(List<JobTemplate> jobTemplates, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<JobTemplate, long>(jobTemplates, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                jobTemplates[i].JobTemplateId = lst[i];
            return jobTemplates;
        }

        public Task<bool> UpdateJobTemplateAsync(JobTemplate jobTemplate, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<JobTemplate>(jobTemplate, modifiedBy);
        }

        public async Task<bool> MarkJobTemplateAsNotDeletedByIdAsync(long jobTemplateId, long? modifiedBy)
        {
            return await appDBManager.GetTable<JobTemplate>().Where(x => x.JobTemplateId == jobTemplateId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkJobTemplateAsDeletedByIdAsync(long jobTemplateId, long? modifiedBy)
        {
            return await appDBManager.GetTable<JobTemplate>().Where(x => x.JobTemplateId == jobTemplateId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteJobTemplateByIdAsync(long jobTemplateId)
        {
            return await appDBManager.GetTable<JobTemplate>().Where(x => x.JobTemplateId == jobTemplateId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetJobTemplateActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.JobTemplate);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        #endregion

        #region JobTemplateCategory

        public IQueryable<JobTemplateCategoryView> GetJobTemplateCategorysViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<JobTemplateCategory>()
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new JobTemplateCategoryView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,
                       JobTemplateCategoryId = item.JobTemplateCategoryId,
                       Name = item.Name,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,
                   };
        }

        public async Task<DbReturnListModel<JobTemplateCategoryView>> GetJobTemplateCategorysInternalAsync(JobTemplateCategorySearchCriteria search)
        {
            var sql1 = GetJobTemplateCategorysViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.JobTemplateCategoryId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.Name.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper()));
            }

            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.JobTemplateCategoryId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetJobTemplateCategorysViewAsIQueryable().FirstOrDefaultAsync(x => x.JobTemplateCategoryId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<JobTemplateCategoryView>> GetJobTemplateCategorysAsync(JobTemplateCategorySearchCriteria search)
        {
            return GetJobTemplateCategorysInternalAsync(search);
        }

        public Task<JobTemplateCategory> GetJobTemplateCategoryFromAnyByIdAsync(long jobTemplateCategoryId)
        {
            return appDBManager.FirstOrDefaultAsync<JobTemplateCategory>(x => x.JobTemplateCategoryId == jobTemplateCategoryId);
        }

        public Task<JobTemplateCategoryView> GetJobTemplateCategoryViewFromAnyByIdAsync(long jobTemplateCategoryId)
        {
            return GetJobTemplateCategorysViewAsIQueryable().FirstOrDefaultAsync(x => x.JobTemplateCategoryId == jobTemplateCategoryId);
        }

        public Task<JobTemplateCategory> GetJobTemplateCategoryByIdAsync(long jobTemplateCategoryId)
        {
            return appDBManager.FirstOrDefaultAsync<JobTemplateCategory>(x => x.JobTemplateCategoryId == jobTemplateCategoryId && x.IsDeleted == false);
        }

        public Task<JobTemplateCategoryView> GetJobTemplateCategoryViewByIdAsync(long jobTemplateCategoryId)
        {
            var sql1 = GetJobTemplateCategorysViewAsIQueryable();

            sql1 = sql1.Where(x => x.JobTemplateCategoryId == jobTemplateCategoryId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<JobTemplateCategory>> GetAllJobTemplateCategorys()
        {
            return appDBManager.SelectAllAsync<JobTemplateCategory>();
        }

        public async Task<JobTemplateCategory> CreateJobTemplateCategoryAsync(JobTemplateCategory jobTemplateCategory, long? createdBy)
        {
            jobTemplateCategory.JobTemplateCategoryId = await appDBManager.InsertWithAuditAsync<JobTemplateCategory, long>(jobTemplateCategory, createdBy);
            return jobTemplateCategory;
        }

        public async Task<List<JobTemplateCategory>> CreateJobTemplateCategorysAsync(List<JobTemplateCategory> jobTemplateCategorys, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<JobTemplateCategory, long>(jobTemplateCategorys, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                jobTemplateCategorys[i].JobTemplateCategoryId = lst[i];
            return jobTemplateCategorys;
        }

        public Task<bool> UpdateJobTemplateCategoryAsync(JobTemplateCategory jobTemplateCategory, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<JobTemplateCategory>(jobTemplateCategory, modifiedBy);
        }

        public async Task<bool> MarkJobTemplateCategoryAsNotDeletedByIdAsync(long jobTemplateCategoryId, long? modifiedBy)
        {
            return await appDBManager.GetTable<JobTemplateCategory>().Where(x => x.JobTemplateCategoryId == jobTemplateCategoryId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkJobTemplateCategoryAsDeletedByIdAsync(long jobTemplateCategoryId, long? modifiedBy)
        {
            return await appDBManager.GetTable<JobTemplateCategory>().Where(x => x.JobTemplateCategoryId == jobTemplateCategoryId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteJobTemplateCategoryByIdAsync(long jobTemplateCategoryId)
        {
            return await appDBManager.GetTable<JobTemplateCategory>().Where(x => x.JobTemplateCategoryId == jobTemplateCategoryId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetJobTemplateCategoryActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.JobTemplateCategory);
            return GetActivitiesByExpressionAsync(srch, search);
        }

        #endregion

        #region JobTemplateParameter

        public IQueryable<JobTemplateParameterView> GetJobTemplateParametersViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<JobTemplateParameter>()
                   join a in appDBManager.GetTable<JobTemplate>() on item.JobTemplateId equals a.JobTemplateId
                   join b in appDBManager.GetTable<EnumType>() on item.PredefinedFieldControlTypeId equals b.EnumTypeId
                   join c in appDBManager.GetTable<EnumType>() on item.PredefinedFieldTypeId equals c.EnumTypeId
                   join d in appDBManager.GetTable<EnumType>() on item.PredefinedFieldPopulatorTypeId equals d.EnumTypeId
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new JobTemplateParameterView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,
                       PredefinedFieldControlTypeName = b.EnumTypeName,
                       PredefinedFieldTypeName = c.EnumTypeName,
                       PredefinedFieldPopulatorTypeName = d.EnumTypeName,
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

        public async Task<DbReturnListModel<JobTemplateParameterView>> GetJobTemplateParametersInternalAsync(JobTemplateParameterSearchCriteria search)
        {
            var sql1 = GetJobTemplateParametersViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.JobTemplateParameterId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.Name.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.Value.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.DefaultValue.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.Caption.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.PopulatorSource.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper()));
            }

            if (search.JobTemplateId != null && search.JobTemplateId > 0)
                sql1 = sql1.Where(x => search.JobTemplateId == x.JobTemplateId);
            if (search.PredefinedFieldControlTypeIds != null && search.PredefinedFieldControlTypeIds.Count > 0)
                sql1 = sql1.Where(x => search.PredefinedFieldControlTypeIds.Contains(x.PredefinedFieldControlTypeId));
            if (search.PredefinedFieldTypeIds != null && search.PredefinedFieldTypeIds.Count > 0)
                sql1 = sql1.Where(x => search.PredefinedFieldTypeIds.Contains(x.PredefinedFieldTypeId));
            if (search.PredefinedFieldPopulatorTypeIds != null && search.PredefinedFieldPopulatorTypeIds.Count > 0)
                sql1 = sql1.Where(x => search.PredefinedFieldPopulatorTypeIds.Contains(x.PredefinedFieldPopulatorTypeId));

            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.JobTemplateParameterId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetJobTemplateParametersViewAsIQueryable().FirstOrDefaultAsync(x => x.JobTemplateParameterId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<JobTemplateParameterView>> GetJobTemplateParametersAsync(JobTemplateParameterSearchCriteria search)
        {
            return GetJobTemplateParametersInternalAsync(search);
        }

        public Task<JobTemplateParameter> GetJobTemplateParameterFromAnyByIdAsync(long jobTemplateParameterId)
        {
            return appDBManager.FirstOrDefaultAsync<JobTemplateParameter>(x => x.JobTemplateParameterId == jobTemplateParameterId);
        }

        public Task<JobTemplateParameterView> GetJobTemplateParameterViewFromAnyByIdAsync(long jobTemplateParameterId)
        {
            return GetJobTemplateParametersViewAsIQueryable().FirstOrDefaultAsync(x => x.JobTemplateParameterId == jobTemplateParameterId);
        }

        public Task<JobTemplateParameter> GetJobTemplateParameterByIdAsync(long jobTemplateParameterId)
        {
            return appDBManager.FirstOrDefaultAsync<JobTemplateParameter>(x => x.JobTemplateParameterId == jobTemplateParameterId && x.IsDeleted == false);
        }

        public Task<JobTemplateParameterView> GetJobTemplateParameterViewByIdAsync(long jobTemplateParameterId)
        {
            var sql1 = GetJobTemplateParametersViewAsIQueryable();

            sql1 = sql1.Where(x => x.JobTemplateParameterId == jobTemplateParameterId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<JobTemplateParameter>> GetAllJobTemplateParameters()
        {
            return appDBManager.SelectAllAsync<JobTemplateParameter>();
        }

        public async Task<JobTemplateParameter> CreateJobTemplateParameterAsync(JobTemplateParameter jobTemplateParameter, long? createdBy)
        {
            jobTemplateParameter.JobTemplateParameterId = await appDBManager.InsertWithAuditAsync<JobTemplateParameter, long>(jobTemplateParameter, createdBy);
            return jobTemplateParameter;
        }

        public async Task<List<JobTemplateParameter>> CreateJobTemplateParametersAsync(List<JobTemplateParameter> jobTemplateParameters, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<JobTemplateParameter, long>(jobTemplateParameters, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                jobTemplateParameters[i].JobTemplateParameterId = lst[i];
            return jobTemplateParameters;
        }

        public Task<bool> UpdateJobTemplateParameterAsync(JobTemplateParameter jobTemplateParameter, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<JobTemplateParameter>(jobTemplateParameter, modifiedBy);
        }

        public async Task<bool> MarkJobTemplateParameterAsNotDeletedByIdAsync(long jobTemplateParameterId, long? modifiedBy)
        {
            return await appDBManager.GetTable<JobTemplateParameter>().Where(x => x.JobTemplateParameterId == jobTemplateParameterId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkJobTemplateParameterAsDeletedByIdAsync(long jobTemplateParameterId, long? modifiedBy)
        {
            return await appDBManager.GetTable<JobTemplateParameter>().Where(x => x.JobTemplateParameterId == jobTemplateParameterId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteJobTemplateParameterByIdAsync(long jobTemplateParameterId)
        {
            return await appDBManager.GetTable<JobTemplateParameter>().Where(x => x.JobTemplateParameterId == jobTemplateParameterId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetJobTemplateParameterActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.JobTemplateParameter);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        #endregion

        #region SavedJob

        public IQueryable<SavedJobView> GetSavedJobsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<SavedJob>()
                   join a in appDBManager.GetTable<EnumType>() on item.PredefinedJobTypeId equals a.EnumTypeId
                   join b in appDBManager.GetTable<JobTemplate>() on item.JobTemplateId equals b.JobTemplateId
                   join od in appDBManager.GetTable<User>() on item.UserId equals od.UserId into dTemp
                   from d in dTemp.DefaultIfEmpty()
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new SavedJobView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,

                       PredefinedJobTypeId = item.PredefinedJobTypeId,
                       ReportTemplateFile = item.ReportTemplateFile,
                       PredefinedJobTypeName = a.EnumTypeName,
                       UserName = d.UserName,
                       SavedJobId = item.SavedJobId,
                       PredefinedJobOrReportTypeId = item.PredefinedJobTypeId,
                       JobTemplateId = item.JobTemplateId,
                       CompletedDate = item.CompletedDate,
                       SubmittedDate = item.SubmittedDate,
                       SubmittedByUserId = item.SubmittedByUserId,
                       ExecutableFolder = item.ExecutableFolder,
                       ExecutableName = item.ExecutableName,
                       ExecutableParameter = item.ExecutableParameter,
                       SerializedJobParameter = item.SerializedJobParameter,
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

        public async Task<DbReturnListModel<SavedJobView>> GetSavedJobsInternalAsync(SavedJobSearchCriteria search)
        {
            var sql1 = GetSavedJobsViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.SavedJobId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.ExecutableName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.ExecutableParameter.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.CustomExportName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.JobName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper()));
            }

            if (search.PredefinedJobTypeIds != null && search.PredefinedJobTypeIds.Count > 0)
                sql1 = sql1.Where(x => search.PredefinedJobTypeIds.Contains(x.PredefinedJobTypeId));
            if (search.JobTemplateId != null && search.JobTemplateId > 0)
                sql1 = sql1.Where(x => search.JobTemplateId == x.JobTemplateId);
            if (search.SubmittedByUserId != null && search.SubmittedByUserId > 0)
                sql1 = sql1.Where(x => search.SubmittedByUserId == x.SubmittedByUserId);
            if (search.UserId != null && search.UserId > 0)
                sql1 = sql1.Where(x => search.UserId == x.UserId);

            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.SavedJobId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetSavedJobsViewAsIQueryable().FirstOrDefaultAsync(x => x.SavedJobId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<SavedJobView>> GetSavedJobsAsync(SavedJobSearchCriteria search)
        {
            return GetSavedJobsInternalAsync(search);
        }

        public Task<SavedJob> GetSavedJobFromAnyByIdAsync(long savedJobId)
        {
            return appDBManager.FirstOrDefaultAsync<SavedJob>(x => x.SavedJobId == savedJobId);
        }

        public Task<SavedJobView> GetSavedJobViewFromAnyByIdAsync(long savedJobId)
        {
            return GetSavedJobsViewAsIQueryable().FirstOrDefaultAsync(x => x.SavedJobId == savedJobId);
        }

        public Task<SavedJob> GetSavedJobByIdAsync(long savedJobId)
        {
            return appDBManager.FirstOrDefaultAsync<SavedJob>(x => x.SavedJobId == savedJobId && x.IsDeleted == false);
        }

        public Task<SavedJobView> GetSavedJobViewByIdAsync(long savedJobId)
        {
            var sql1 = GetSavedJobsViewAsIQueryable();

            sql1 = sql1.Where(x => x.SavedJobId == savedJobId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<SavedJob>> GetAllSavedJobs()
        {
            return appDBManager.SelectAllAsync<SavedJob>();
        }

        public async Task<SavedJob> CreateSavedJobAsync(SavedJob savedJob, long? createdBy)
        {
            savedJob.SavedJobId = await appDBManager.InsertWithAuditAsync<SavedJob, long>(savedJob, createdBy);
            return savedJob;
        }

        public async Task<List<SavedJob>> CreateSavedJobsAsync(List<SavedJob> savedJobs, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<SavedJob, long>(savedJobs, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                savedJobs[i].SavedJobId = lst[i];
            return savedJobs;
        }

        public Task<bool> UpdateSavedJobAsync(SavedJob savedJob, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<SavedJob>(savedJob, modifiedBy);
        }

        public async Task<bool> MarkSavedJobAsNotDeletedByIdAsync(long savedJobId, long? modifiedBy)
        {
            return await appDBManager.GetTable<SavedJob>().Where(x => x.SavedJobId == savedJobId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkSavedJobAsDeletedByIdAsync(long savedJobId, long? modifiedBy)
        {
            return await appDBManager.GetTable<SavedJob>().Where(x => x.SavedJobId == savedJobId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteSavedJobByIdAsync(long savedJobId)
        {
            return await appDBManager.GetTable<SavedJob>().Where(x => x.SavedJobId == savedJobId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetSavedJobActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.SavedJob);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        #endregion

        #region SavedJobNotificationEmail

        public IQueryable<SavedJobNotificationEmailView> GetSavedJobNotificationEmailsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<SavedJobNotificationEmail>()
                   join a in appDBManager.GetTable<SavedJob>() on item.SavedJobId equals a.SavedJobId
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new SavedJobNotificationEmailView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,

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

        public async Task<DbReturnListModel<SavedJobNotificationEmailView>> GetSavedJobNotificationEmailsInternalAsync(SavedJobNotificationEmailSearchCriteria search)
        {
            var sql1 = GetSavedJobNotificationEmailsViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.SavedJobNotificationEmailId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.Email.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.Message.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper()));
            }

            if (search.SavedJobId != null && search.SavedJobId > 0)
                sql1 = sql1.Where(x => search.SavedJobId == x.SavedJobId);

            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.SavedJobNotificationEmailId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetSavedJobNotificationEmailsViewAsIQueryable().FirstOrDefaultAsync(x => x.SavedJobNotificationEmailId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<SavedJobNotificationEmailView>> GetSavedJobNotificationEmailsAsync(SavedJobNotificationEmailSearchCriteria search)
        {
            return GetSavedJobNotificationEmailsInternalAsync(search);
        }

        public Task<SavedJobNotificationEmail> GetSavedJobNotificationEmailFromAnyByIdAsync(long savedJobNotificationEmailId)
        {
            return appDBManager.FirstOrDefaultAsync<SavedJobNotificationEmail>(x => x.SavedJobNotificationEmailId == savedJobNotificationEmailId);
        }

        public Task<SavedJobNotificationEmailView> GetSavedJobNotificationEmailViewFromAnyByIdAsync(long savedJobNotificationEmailId)
        {
            return GetSavedJobNotificationEmailsViewAsIQueryable().FirstOrDefaultAsync(x => x.SavedJobNotificationEmailId == savedJobNotificationEmailId);
        }

        public Task<SavedJobNotificationEmail> GetSavedJobNotificationEmailByIdAsync(long savedJobNotificationEmailId)
        {
            return appDBManager.FirstOrDefaultAsync<SavedJobNotificationEmail>(x => x.SavedJobNotificationEmailId == savedJobNotificationEmailId && x.IsDeleted == false);
        }

        public Task<SavedJobNotificationEmailView> GetSavedJobNotificationEmailViewByIdAsync(long savedJobNotificationEmailId)
        {
            var sql1 = GetSavedJobNotificationEmailsViewAsIQueryable();

            sql1 = sql1.Where(x => x.SavedJobNotificationEmailId == savedJobNotificationEmailId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<SavedJobNotificationEmail>> GetAllSavedJobNotificationEmails()
        {
            return appDBManager.SelectAllAsync<SavedJobNotificationEmail>();
        }

        public async Task<SavedJobNotificationEmail> CreateSavedJobNotificationEmailAsync(SavedJobNotificationEmail savedJobNotificationEmail, long? createdBy)
        {
            savedJobNotificationEmail.SavedJobNotificationEmailId = await appDBManager.InsertWithAuditAsync<SavedJobNotificationEmail, long>(savedJobNotificationEmail, createdBy);
            return savedJobNotificationEmail;
        }

        public async Task<List<SavedJobNotificationEmail>> CreateSavedJobNotificationEmailsAsync(List<SavedJobNotificationEmail> savedJobNotificationEmails, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<SavedJobNotificationEmail, long>(savedJobNotificationEmails, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                savedJobNotificationEmails[i].SavedJobNotificationEmailId = lst[i];
            return savedJobNotificationEmails;
        }

        public Task<bool> UpdateSavedJobNotificationEmailAsync(SavedJobNotificationEmail savedJobNotificationEmail, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<SavedJobNotificationEmail>(savedJobNotificationEmail, modifiedBy);
        }

        public async Task<bool> MarkSavedJobNotificationEmailAsNotDeletedByIdAsync(long savedJobNotificationEmailId, long? modifiedBy)
        {
            return await appDBManager.GetTable<SavedJobNotificationEmail>().Where(x => x.SavedJobNotificationEmailId == savedJobNotificationEmailId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkSavedJobNotificationEmailAsDeletedByIdAsync(long savedJobNotificationEmailId, long? modifiedBy)
        {
            return await appDBManager.GetTable<SavedJobNotificationEmail>().Where(x => x.SavedJobNotificationEmailId == savedJobNotificationEmailId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteSavedJobNotificationEmailByIdAsync(long savedJobNotificationEmailId)
        {
            return await appDBManager.GetTable<SavedJobNotificationEmail>().Where(x => x.SavedJobNotificationEmailId == savedJobNotificationEmailId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetSavedJobNotificationEmailActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.SavedJobNotificationEmail);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        #endregion

        #region ScheduledJob

        public IQueryable<ScheduledJobView> GetScheduledJobsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<ScheduledJob>()
                   join a in appDBManager.GetTable<JobTemplate>() on item.JobTemplateId equals a.JobTemplateId
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new ScheduledJobView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,
                       PredefinedJobOrReportTypeId = item.PredefinedJobOrReportTypeId,
                       ReportTemplateFile = item.ReportTemplateFile,
                       ScheduledJobId = item.ScheduledJobId,
                       JobTemplateId = item.JobTemplateId,
                       SubmittedByUserId = item.SubmittedByUserId,
                       ExecutableFolder = item.ExecutableFolder,
                       ExecutableName = item.ExecutableName,
                       ExecutableParameter = item.ExecutableParameter,
                       SerializedJobParameter = item.SerializedJobParameter,
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

        public async Task<DbReturnListModel<ScheduledJobView>> GetScheduledJobsInternalAsync(ScheduledJobSearchCriteria search)
        {
            var sql1 = GetScheduledJobsViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.ScheduledJobId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.ExecutableName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.ExecutableParameter.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.CustomExportName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.JobName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper()));
            }

            if (search.JobTemplateId != null && search.JobTemplateId > 0)
                sql1 = sql1.Where(x => search.JobTemplateId == x.JobTemplateId);
            if (search.SubmittedByUserId != null && search.SubmittedByUserId > 0)
                sql1 = sql1.Where(x => search.SubmittedByUserId == x.SubmittedByUserId);
            if (search.RunAsUserId != null && search.RunAsUserId > 0)
                sql1 = sql1.Where(x => search.RunAsUserId == x.RunAsUserId);

            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.ScheduledJobId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetScheduledJobsViewAsIQueryable().FirstOrDefaultAsync(x => x.ScheduledJobId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<ScheduledJobView>> GetScheduledJobsAsync(ScheduledJobSearchCriteria search)
        {
            return GetScheduledJobsInternalAsync(search);
        }

        public Task<ScheduledJob> GetScheduledJobFromAnyByIdAsync(long scheduledJobId)
        {
            return appDBManager.FirstOrDefaultAsync<ScheduledJob>(x => x.ScheduledJobId == scheduledJobId);
        }

        public Task<ScheduledJobView> GetScheduledJobViewFromAnyByIdAsync(long scheduledJobId)
        {
            return GetScheduledJobsViewAsIQueryable().FirstOrDefaultAsync(x => x.ScheduledJobId == scheduledJobId);
        }

        public Task<ScheduledJobView> GetScheduledJobViewFromPredefinedJobTypeIdAsync(int predefinedJobTypeId)
        {
            return GetScheduledJobsViewAsIQueryable().FirstOrDefaultAsync(x => x.PredefinedJobOrReportTypeId == predefinedJobTypeId);
        }

        public Task<ScheduledJob> GetScheduledJobByIdAsync(long scheduledJobId)
        {
            return appDBManager.FirstOrDefaultAsync<ScheduledJob>(x => x.ScheduledJobId == scheduledJobId && x.IsDeleted == false);
        }

        public Task<ScheduledJob> GetScheduledJobByPredefinedJobTypeAsync(long predefinedJobTypeId)
        {
            return appDBManager.FirstOrDefaultAsync<ScheduledJob>(x => x.PredefinedJobOrReportTypeId == predefinedJobTypeId && x.IsDeleted == false);
        }

        public Task<ScheduledJobView> GetScheduledJobViewByIdAsync(long scheduledJobId)
        {
            var sql1 = GetScheduledJobsViewAsIQueryable();

            sql1 = sql1.Where(x => x.ScheduledJobId == scheduledJobId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<ScheduledJob>> GetAllScheduledJobsAsync()
        {
            return appDBManager.SelectAllAsync<ScheduledJob>();
        }

        public async Task<ScheduledJob> CreateScheduledJobAsync(ScheduledJob scheduledJob, long? createdBy)
        {
            scheduledJob.ScheduledJobId = await appDBManager.InsertWithAuditAsync<ScheduledJob, long>(scheduledJob, createdBy);
            return scheduledJob;
        }

        public async Task<List<ScheduledJob>> CreateScheduledJobsAsync(List<ScheduledJob> scheduledJobs, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<ScheduledJob, long>(scheduledJobs, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                scheduledJobs[i].ScheduledJobId = lst[i];
            return scheduledJobs;
        }

        public Task<bool> UpdateScheduledJobAsync(ScheduledJob scheduledJob, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<ScheduledJob>(scheduledJob, modifiedBy);
        }

        public async Task<bool> MarkScheduledJobAsNotDeletedByIdAsync(long scheduledJobId, long? modifiedBy)
        {
            return await appDBManager.GetTable<ScheduledJob>().Where(x => x.ScheduledJobId == scheduledJobId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkScheduledJobAsDeletedByIdAsync(long scheduledJobId, long? modifiedBy)
        {
            return await appDBManager.GetTable<ScheduledJob>().Where(x => x.ScheduledJobId == scheduledJobId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteScheduledJobByIdAsync(long scheduledJobId)
        {
            return await appDBManager.GetTable<ScheduledJob>().Where(x => x.ScheduledJobId == scheduledJobId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetScheduledJobActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.ScheduledJob);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        #endregion

        #region ScheduledJobHistory

        public IQueryable<ScheduledJobHistoryView> GetScheduledJobHistorysViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<ScheduledJobHistory>()
                   join a in appDBManager.GetTable<ScheduledJob>() on item.ScheduledJobId equals a.ScheduledJobId
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new ScheduledJobHistoryView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,
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

        public async Task<DbReturnListModel<ScheduledJobHistoryView>> GetScheduledJobHistorysInternalAsync(ScheduledJobHistorySearchCriteria search)
        {
            var sql1 = GetScheduledJobHistorysViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.ScheduledJobHistoryId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.PickupMachineName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper()));
            }

            if (search.ScheduledJobId != null && search.ScheduledJobId > 0)
                sql1 = sql1.Where(x => search.ScheduledJobId == x.ScheduledJobId);
            if (search.PickupProcessId != null && search.PickupProcessId > 0)
                sql1 = sql1.Where(x => search.PickupProcessId == x.PickupProcessId);

            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.ScheduledJobHistoryId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetScheduledJobHistorysViewAsIQueryable().FirstOrDefaultAsync(x => x.ScheduledJobHistoryId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<ScheduledJobHistoryView>> GetScheduledJobHistorysAsync(ScheduledJobHistorySearchCriteria search)
        {
            return GetScheduledJobHistorysInternalAsync(search);
        }

        public Task<ScheduledJobHistory> GetScheduledJobHistoryFromAnyByIdAsync(long scheduledJobHistoryId)
        {
            return appDBManager.FirstOrDefaultAsync<ScheduledJobHistory>(x => x.ScheduledJobHistoryId == scheduledJobHistoryId);
        }

        public Task<ScheduledJobHistoryView> GetScheduledJobHistoryViewFromAnyByIdAsync(long scheduledJobHistoryId)
        {
            return GetScheduledJobHistorysViewAsIQueryable().FirstOrDefaultAsync(x => x.ScheduledJobHistoryId == scheduledJobHistoryId);
        }

        public Task<ScheduledJobHistory> GetScheduledJobHistoryByIdAsync(long scheduledJobHistoryId)
        {
            return appDBManager.FirstOrDefaultAsync<ScheduledJobHistory>(x => x.ScheduledJobHistoryId == scheduledJobHistoryId && x.IsDeleted == false);
        }

        public Task<ScheduledJobHistoryView> GetScheduledJobHistoryViewByIdAsync(long scheduledJobHistoryId)
        {
            var sql1 = GetScheduledJobHistorysViewAsIQueryable();

            sql1 = sql1.Where(x => x.ScheduledJobHistoryId == scheduledJobHistoryId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<ScheduledJobHistory>> GetAllScheduledJobHistorys()
        {
            return appDBManager.SelectAllAsync<ScheduledJobHistory>();
        }

        public async Task<ScheduledJobHistory> CreateScheduledJobHistoryAsync(ScheduledJobHistory scheduledJobHistory, long? createdBy)
        {
            scheduledJobHistory.ScheduledJobHistoryId = await appDBManager.InsertWithAuditAsync<ScheduledJobHistory, long>(scheduledJobHistory, createdBy);
            return scheduledJobHistory;
        }

        public async Task<List<ScheduledJobHistory>> CreateScheduledJobHistorysAsync(List<ScheduledJobHistory> scheduledJobHistorys, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<ScheduledJobHistory, long>(scheduledJobHistorys, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                scheduledJobHistorys[i].ScheduledJobHistoryId = lst[i];
            return scheduledJobHistorys;
        }

        public Task<bool> UpdateScheduledJobHistoryAsync(ScheduledJobHistory scheduledJobHistory, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<ScheduledJobHistory>(scheduledJobHistory, modifiedBy);
        }

        public async Task<bool> MarkScheduledJobHistoryAsNotDeletedByIdAsync(long scheduledJobHistoryId, long? modifiedBy)
        {
            return await appDBManager.GetTable<ScheduledJobHistory>().Where(x => x.ScheduledJobHistoryId == scheduledJobHistoryId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkScheduledJobHistoryAsDeletedByIdAsync(long scheduledJobHistoryId, long? modifiedBy)
        {
            return await appDBManager.GetTable<ScheduledJobHistory>().Where(x => x.ScheduledJobHistoryId == scheduledJobHistoryId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteScheduledJobHistoryByIdAsync(long scheduledJobHistoryId)
        {
            return await appDBManager.GetTable<ScheduledJobHistory>().Where(x => x.ScheduledJobHistoryId == scheduledJobHistoryId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetScheduledJobHistoryActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.ScheduledJobHistory);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        #endregion

        #region ScheduledJobNotificationEmail

        public IQueryable<ScheduledJobNotificationEmailView> GetScheduledJobNotificationEmailsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<ScheduledJobNotificationEmail>()
                   join a in appDBManager.GetTable<ScheduledJob>() on item.ScheduledJobId equals a.ScheduledJobId
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new ScheduledJobNotificationEmailView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,

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

        public async Task<DbReturnListModel<ScheduledJobNotificationEmailView>> GetScheduledJobNotificationEmailsInternalAsync(ScheduledJobNotificationEmailSearchCriteria search)
        {
            var sql1 = GetScheduledJobNotificationEmailsViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.ScheduledJobNotificationEmailId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.Email.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.Message.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper()));
            }

            if (search.ScheduledJobId != null && search.ScheduledJobId > 0)
                sql1 = sql1.Where(x => search.ScheduledJobId == x.ScheduledJobId);

            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.ScheduledJobNotificationEmailId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetScheduledJobNotificationEmailsViewAsIQueryable().FirstOrDefaultAsync(x => x.ScheduledJobNotificationEmailId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<ScheduledJobNotificationEmailView>> GetScheduledJobNotificationEmailsAsync(ScheduledJobNotificationEmailSearchCriteria search)
        {
            return GetScheduledJobNotificationEmailsInternalAsync(search);
        }

        public Task<ScheduledJobNotificationEmail> GetScheduledJobNotificationEmailFromAnyByIdAsync(long scheduledJobNotificationEmailId)
        {
            return appDBManager.FirstOrDefaultAsync<ScheduledJobNotificationEmail>(x => x.ScheduledJobNotificationEmailId == scheduledJobNotificationEmailId);
        }

        public Task<ScheduledJobNotificationEmailView> GetScheduledJobNotificationEmailViewFromAnyByIdAsync(long scheduledJobNotificationEmailId)
        {
            return GetScheduledJobNotificationEmailsViewAsIQueryable().FirstOrDefaultAsync(x => x.ScheduledJobNotificationEmailId == scheduledJobNotificationEmailId);
        }

        public Task<ScheduledJobNotificationEmail> GetScheduledJobNotificationEmailByIdAsync(long scheduledJobNotificationEmailId)
        {
            return appDBManager.FirstOrDefaultAsync<ScheduledJobNotificationEmail>(x => x.ScheduledJobNotificationEmailId == scheduledJobNotificationEmailId && x.IsDeleted == false);
        }

        public Task<ScheduledJobNotificationEmailView> GetScheduledJobNotificationEmailViewByIdAsync(long scheduledJobNotificationEmailId)
        {
            var sql1 = GetScheduledJobNotificationEmailsViewAsIQueryable();

            sql1 = sql1.Where(x => x.ScheduledJobNotificationEmailId == scheduledJobNotificationEmailId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<ScheduledJobNotificationEmail>> GetAllScheduledJobNotificationEmails()
        {
            return appDBManager.SelectAllAsync<ScheduledJobNotificationEmail>();
        }

        public async Task<ScheduledJobNotificationEmail> CreateScheduledJobNotificationEmailAsync(ScheduledJobNotificationEmail scheduledJobNotificationEmail, long? createdBy)
        {
            scheduledJobNotificationEmail.ScheduledJobNotificationEmailId = await appDBManager.InsertWithAuditAsync<ScheduledJobNotificationEmail, long>(scheduledJobNotificationEmail, createdBy);
            return scheduledJobNotificationEmail;
        }

        public async Task<List<ScheduledJobNotificationEmail>> CreateScheduledJobNotificationEmailsAsync(List<ScheduledJobNotificationEmail> scheduledJobNotificationEmails, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<ScheduledJobNotificationEmail, long>(scheduledJobNotificationEmails, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                scheduledJobNotificationEmails[i].ScheduledJobNotificationEmailId = lst[i];
            return scheduledJobNotificationEmails;
        }

        public Task<bool> UpdateScheduledJobNotificationEmailAsync(ScheduledJobNotificationEmail scheduledJobNotificationEmail, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<ScheduledJobNotificationEmail>(scheduledJobNotificationEmail, modifiedBy);
        }

        public async Task<bool> MarkScheduledJobNotificationEmailAsNotDeletedByIdAsync(long scheduledJobNotificationEmailId, long? modifiedBy)
        {
            return await appDBManager.GetTable<ScheduledJobNotificationEmail>().Where(x => x.ScheduledJobNotificationEmailId == scheduledJobNotificationEmailId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkScheduledJobNotificationEmailAsDeletedByIdAsync(long scheduledJobNotificationEmailId, long? modifiedBy)
        {
            return await appDBManager.GetTable<ScheduledJobNotificationEmail>().Where(x => x.ScheduledJobNotificationEmailId == scheduledJobNotificationEmailId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteScheduledJobNotificationEmailByIdAsync(long scheduledJobNotificationEmailId)
        {
            return await appDBManager.GetTable<ScheduledJobNotificationEmail>().Where(x => x.ScheduledJobNotificationEmailId == scheduledJobNotificationEmailId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetScheduledJobNotificationEmailActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.ScheduledJobNotificationEmail);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        #endregion

    }
}
