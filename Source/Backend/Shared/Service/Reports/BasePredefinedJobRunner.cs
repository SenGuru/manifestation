using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework.Infrastructure.Models.Result;

namespace Manifest.Service.JobServer
{
    public abstract class BasePredefinedJobRunner
    {
        public virtual Task<ReturnModel<bool>> GenerateDataAsync(string fileName, string reportFile, string jasperExeFilePath)
        {
            throw new NotImplementedException();
        }

        public virtual Task<ReturnModel<bool>> NonGenerateDataExecuteAsync(long? jobId)
        {
            throw new NotImplementedException();
        }

        public virtual Task<ReturnModel<bool>> NonGenerateDataMatchExecuteAsync()
        {
            throw new NotImplementedException();
        }

        public virtual List<long> GetParameterTenantIds()
        {
            throw new NotImplementedException();
        }
    }
}