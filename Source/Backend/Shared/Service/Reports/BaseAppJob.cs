using System.Threading.Tasks;
using Manifest.Repository.Common;
using Manifest.Service.Report;
using Manifest.Service.Storage;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Result;

namespace Manifest.Service.JobServer
{
    public class BaseAppJob : BaseJob
    {
        protected readonly JobFileLocatorService jobFileLocatorService;
        protected readonly IntermediateDBRepository intermediateDBCreationRepository;
        protected readonly NewReportServerFileStorage reportServerFileStorage;

        public BaseAppJob(ILog log, ReportRepository reportRepository, JobFileLocatorService jobFileLocatorService, IntermediateDBRepository intermediateDBCreationRepository, NewReportServerFileStorage reportServerFileStorage)
        : base(log, reportRepository)
        {
            this.jobFileLocatorService = jobFileLocatorService;
            this.intermediateDBCreationRepository = intermediateDBCreationRepository;
            this.reportServerFileStorage = reportServerFileStorage;
        }

        public override async Task<ReturnModel<bool>> RunAsync(long jobId)
        {
            JobId = jobId;

            var result = await InitAsync(jobId);

            if (!result.IsSuccess)
                return result;

            result = await GenerateDataFileAsync(reportServerFileStorage.DBFilePath(jobId.ToString(), "report"));

            if (!result.IsSuccess)
                return result;

            log.Info($"Running Job - {jobId}...");
            return ReturnModel<bool>.Success(true);
        }
    }
}