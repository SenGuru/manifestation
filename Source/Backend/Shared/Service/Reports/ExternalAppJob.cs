using System;
using System.Threading.Tasks;
using Manifest.Common.Models.DB;
using Manifest.Repository.Common;
using Manifest.Service.Report;
using Manifest.Service.Storage;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Result;

namespace Manifest.Service.JobServer
{
    public class ExternalAppJob : BaseAppJob
    {
        public ExternalAppJob(ILog log, ReportRepository reportRepository, JobFileLocatorService jobFileLocatorService, IntermediateDBRepository intermediateDBCreationRepository, NewReportServerFileStorage reportServerFileStorage)
            : base(log, reportRepository, jobFileLocatorService, intermediateDBCreationRepository, reportServerFileStorage)
        {
        }

        protected override Task<ReturnModel<bool>> GenerateDataFileAsync(string fileName)
        {
            try
            {
                var exeName = jobFileLocatorService.ResolveAndGetJobExecutable(this.Job.ExecutableName, this.Job.ExecutableName);
                var reportFile = jobFileLocatorService.ResolveAndGetReportFile(this.Job.ReportTemplateFile);
                log.Trace($"Call {exeName} /jobId {Job.JobId} /reportfilename {reportFile}");
                return Task.FromResult(ReturnModel<bool>.Success(true));
            }
            catch (Exception ex)
            {
                return Task.FromResult(log.ErrorAndReturnModel<bool>("Unexpected error when running the Runner", ex));
            }
        }
    }
}