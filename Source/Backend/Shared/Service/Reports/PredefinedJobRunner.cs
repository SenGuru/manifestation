using System.Collections.Generic;
using Manifest.Common.Models.SearchCriteria.System.Parameters;
using Manifest.Service.ObjectMapping;
using Manifest.Service.Report;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Utils;

namespace Manifest.Service.JobServer
{
    public abstract class PredefinedJobRunner<TParameter> : BasePredefinedJobRunner
        where TParameter : BaseJobParameter
    {
        protected readonly IntermediateDBRepository intermediateDBCreationRepository;
        protected readonly ObjectMapperService objectMapperService;
        protected ILog log;

        protected PredefinedJobRunner(TParameter parameter, bool doesGenerateIntermediateFile, IntermediateDBRepository intermediateDBCreationRepository, ObjectMapperService objectMapperService, ILog log)
        {
            Parameter = parameter;
            DoesGenerateIntermediateFile = doesGenerateIntermediateFile;
            this.intermediateDBCreationRepository = intermediateDBCreationRepository;
            this.objectMapperService = objectMapperService;
            this.log = log;
        }

        public TParameter Parameter { get; internal set; }

        public bool DoesGenerateIntermediateFile { get; internal set; }

        public override List<long> GetParameterTenantIds()
        {
            return null;
        }
    }
}