using System.Collections.Generic;
using System.Threading.Tasks;
using Manifest.Common.Interfaces.Repository.DbAccess;
using Manifest.Common.Models.DB;

namespace Manifest.Repository.Common
{
    public partial class SettingRepository : BaseRepository
    {
        public SettingRepository(IAppDBManager appDBManager)
            : base(appDBManager)
        {
        }

        public Task<List<ConfigurationItem>> GetAllConfigurationEntriesAsync(int configGroupId)
        {
            return appDBManager.SelectAsync<ConfigurationItem>(x => x.ConfigurationGroupId == configGroupId);
        }

        public Task<bool> SaveAllConfigurationEntriesAsync(List<ConfigurationItem> configurationItems, string modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<ConfigurationItem>(configurationItems, modifiedBy);
        }
    }
}