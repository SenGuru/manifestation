using System.Threading.Tasks;
using Manifest.Common.Models.SearchCriteria.System.Parameters;
using Manifest.Service.App;
using Manifest.Service.ObjectMapping;
using Manifest.Service.Report;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Result;

namespace Manifest.Service.JobServer
{
    public class WakeupNotificationProcessorJobRunner : PredefinedJobRunner<WakeupNotificationProcessorJobParameter>
    {
        private readonly SystemService systemService;

        public WakeupNotificationProcessorJobRunner(WakeupNotificationProcessorJobParameter parameter, IntermediateDBRepository intermediateDBCreationRepository, ObjectMapperService objectMapperService, ILog log, SystemService systemService)
            : base(parameter, false, intermediateDBCreationRepository, objectMapperService, log)
        {
            if (parameter == null)
            {
                this.Parameter = new WakeupNotificationProcessorJobParameter { };
            }

            this.systemService = systemService;
        }

        public override Task<ReturnModel<bool>> NonGenerateDataExecuteAsync(long? jobId)
        {
            return systemService.WakeupNotificationProcessorAsync(jobId ?? 0);
        }
    }
}