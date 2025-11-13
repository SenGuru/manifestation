using Manifest.Service.ObjectMapping;
using Framework.Infrastructure.Logging;

namespace Manifest.Service.App
{
    public class DummyDataGenerationService : BaseAppService
    {
        public DummyDataGenerationService(
                           ILog log,
                           ObjectMapperService objectMapperService)
            : base(log, objectMapperService)
        {
        }
    }
}
