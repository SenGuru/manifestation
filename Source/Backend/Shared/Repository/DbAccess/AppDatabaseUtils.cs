using Manifest.Common.Interfaces.Repository.DbAccess;
using Framework.Data.Utils;
using Framework.Infrastructure.Config;

namespace Manifest.Repository.DbAccess
{
    public class AppDatabaseUtils : DatabaseUtils, IAppDatabaseUtils
    {
        public AppDatabaseUtils(IBaseDbConfiguration config)
            : base(new AppDBInfo(config))
        {
        }
    }
}