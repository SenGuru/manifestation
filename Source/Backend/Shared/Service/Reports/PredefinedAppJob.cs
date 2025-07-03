using System;
using System.Threading.Tasks;
using Manifest.Common.Enums;
using Manifest.Common.Interfaces.Service.App;
using Manifest.Common.Models.DB;
using Manifest.Common.Models.SearchCriteria.System.Parameters;
using Manifest.Repository.Common;
using Manifest.Service.App;
using Manifest.Service.ObjectMapping;
using Manifest.Service.Report;
using Manifest.Service.Storage;
using Framework.Infrastructure.Interfaces.DbAccess;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Utils;

namespace Manifest.Service.JobServer
{
    public class PredefinedAppJob : BaseAppJob
    {
        protected readonly ObjectMapperService objectMapperService;
        protected readonly CompanyRepository companyRepository;
        protected readonly CompanyService companyService;
        protected readonly SystemService systemService;
        protected readonly ITransactionManager transactionManager;
        protected readonly ICurrentSessionService currentSessionService;

        public PredefinedAppJob(ILog log, ReportRepository reportRepository, JobFileLocatorService jobFileLocatorService, IntermediateDBRepository intermediateDBCreationRepository, NewReportServerFileStorage reportServerFileStorage, ObjectMapperService objectMapperService, ITransactionManager transactionManager, ICurrentSessionService currentSessionService, CompanyRepository tenantRepository, CompanyService tenantService, SystemService systemService)
            : base(log, reportRepository, jobFileLocatorService, intermediateDBCreationRepository, reportServerFileStorage)
        {
            this.objectMapperService = objectMapperService;
            this.transactionManager = transactionManager;
            this.currentSessionService = currentSessionService;
            this.companyRepository = tenantRepository;
            this.companyService = tenantService;
            this.systemService = systemService;
        }

        protected override async Task<ReturnModel<bool>> GenerateDataFileAsync(string fileName)
        {
            try
            {
                var runner = GetRunner();
                if (this.DoesGeneratesReportOutputFile)
                {
                    var reportFile = jobFileLocatorService.ResolveAndGetReportFile(this.Job.ReportTemplateFile);
                    var reportGeneratorExePath = jobFileLocatorService.ResolveAndGetJasperExecutable();
                    return await runner.GenerateDataAsync(fileName, reportFile, reportGeneratorExePath);
                }

                return await runner.NonGenerateDataExecuteAsync(this.Job.JobId);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>("Unexpected error when running the Runner", ex);
            }
        }

        private BasePredefinedJobRunner GetRunner()
        {
            switch ((PredefinedJobTypes)Job.PredefinedJobTypeId)
            {
                case PredefinedJobTypes.UserList:
                    {
                        UserListJobParameter param = null;
                        if (!SerializedJobParameter.IsTrimmedStringNullOrEmpty())
                        {
                            (param, Exception ex) = JsonUtils.DeserializeWithErrorHandled<UserListJobParameter>(SerializedJobParameter);
                            if (ex != null)
                            {
                                log.Error(ex, "Erroring parsing the Job Parameter");
                                param = null;
                            }
                        }

                        return new UserListJobRunner(param, intermediateDBCreationRepository, objectMapperService, log, companyRepository);
                    }

                case PredefinedJobTypes.DisableMovieProcessor:
                    {
                        DisableMovieProcessorJobParameter param = null;
                        if (!SerializedJobParameter.IsTrimmedStringNullOrEmpty())
                        {
                            (param, Exception ex) = JsonUtils.DeserializeWithErrorHandled<DisableMovieProcessorJobParameter>(SerializedJobParameter);
                            if (ex != null)
                            {
                                log.Error(ex, "Erroring parsing the Job Parameter");
                                param = null;
                            }
                        }

                        return new DisableMovieProcessorJobRunner(param, intermediateDBCreationRepository, objectMapperService, log, systemService);
                    }

                case PredefinedJobTypes.WakeupNotificationProcessor:
                    {
                        WakeupNotificationProcessorJobParameter param = null;
                        if (!SerializedJobParameter.IsTrimmedStringNullOrEmpty())
                        {
                            (param, Exception ex) = JsonUtils.DeserializeWithErrorHandled<WakeupNotificationProcessorJobParameter>(SerializedJobParameter);
                            if (ex != null)
                            {
                                log.Error(ex, "Erroring parsing the Job Parameter");
                                param = null;
                            }
                        }

                        return new WakeupNotificationProcessorJobRunner(param, intermediateDBCreationRepository, objectMapperService, log, systemService);
                    }

                default:
                    throw new Exception("Not Implemented");
            }
        }
    }
}