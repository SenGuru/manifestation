using System;
using System.Threading.Tasks;
using Manifest.Common.Models.DB;
using Manifest.Repository.Common;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Result;

namespace Manifest.Service.JobServer
{
    public abstract class BaseJob
    {
        protected readonly ILog log;
        protected readonly ReportRepository reportRepository;

        protected BaseJob(ILog log, ReportRepository reportRepository)
        {
            this.log = log;
            this.reportRepository = reportRepository;
        }

        public bool DoesGeneratesReportOutputFile { get; internal set; }

        public string IntermediateFilePath { get; internal set; }

        public string SerializedJobParameter { get; internal set; }

        public Job Job { get; internal set; }

        public long JobId { get; internal set; }

        public abstract Task<ReturnModel<bool>> RunAsync(long jobId);

        protected virtual Task<ReturnModel<bool>> GenerateReportAsync()
        {
            //Get the sqlite file parameter and run the command line
            //Copy the files to the report storage
            //Mark the Job as Completed
            //Send emails associated with the Job

            return Task.FromResult(ReturnModel<bool>.Success(true));
        }

        protected virtual async Task<ReturnModel<bool>> InitAsync(long jobId)
        {
            JobId = jobId;
            log.Info($"Running Job - {JobId}...");
            Job = await reportRepository.GetJobByIdAsync(JobId);
            if (Job == null)
            {
                return log.ErrorAndReturnModel<bool>("Unable to find the Job record", new { jobId });
            }

            SerializedJobParameter = Job.SerializedJobParameter;

            return ReturnModel<bool>.Success(true);
        }

        protected virtual async Task<ReturnModel<bool>> GenerateDataFileAsync(string fileName)
        {
            try
            {
                await Task.Delay(1000);
                return ReturnModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>("Error generating the Data", ex);
            }
        }
    }
}