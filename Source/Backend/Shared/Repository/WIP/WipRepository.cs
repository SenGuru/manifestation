using Manifest.Common.Interfaces.Repository.DbAccess;

namespace Manifest.Repository.Common
{
    public partial class WipRepository : BaseRepository
    {
        public WipRepository(IAppDBManager appDBManager)
            : base(appDBManager)
        {
        }
    }
}
