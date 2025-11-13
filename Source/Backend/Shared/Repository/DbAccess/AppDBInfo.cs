using Framework.Data.DbAccess;
using Framework.Infrastructure.Config;

namespace Manifest.Repository.DbAccess
{
    public class AppDBInfo : DBInfo
    {
        public AppDBInfo(IBaseDbConfiguration config)
            : base("default", config)
        {
        }
    }
}
