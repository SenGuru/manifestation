using Manifest.Common.Interfaces.Repository.DbAccess;
using Framework.Data.DbAccess;
using Framework.Infrastructure.Config;
using Framework.Infrastructure.Interfaces.Helpers;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Config;

namespace Manifest.Repository.DbAccess
{
    public class AppDBManager : DBManager, IAnotherInstanceAppDBManager
    {
        public AppDBManager(IBaseDbConfiguration config, ILog log, IBaseAppConfiguration appconfig, ILogConfiguration logConfiguration, ICurrentUser currentUser)
            : base(log, new AppDBInfo(config), appconfig.UseMiniProfiler, logConfiguration, currentUser)
        {
        }
    }
}