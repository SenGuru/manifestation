using Manifest.Common.Interfaces.Repository.DbAccess;
using Framework.Data.DbAccess;
using Framework.Infrastructure.Config;

namespace Manifest.Repository.DbAccess
{
    public class ConfigDBManager : DBManager, IConfigDBManager, IAppDBManager
    {
        public ConfigDBManager(IBaseDbConfiguration config)
            : base(null, new AppDBInfo(config), false, null, null)
        {
        }
    }
}