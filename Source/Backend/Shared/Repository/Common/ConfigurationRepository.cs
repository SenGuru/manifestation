using System.Collections.Generic;
using System.Threading.Tasks;
using Manifest.Common.Interfaces.Repository.DbAccess;
using Manifest.Common.Models.DB;

namespace Manifest.Repository.Common
{
    public partial class ConfigurationRepository : BaseRepository
    {
        public ConfigurationRepository(IAppDBManager dbManager)
            : base(dbManager)
        {
        }

        public Task<List<ConfigurationItem>> GetAllConfigurationEntriesAsync(long configGroupId)
        {
            return appDBManager.SelectAsync<ConfigurationItem>(x => x.ConfigurationGroupId == configGroupId);
        }

        public Task<bool> SaveAllConfigurationEntriesAsync(List<ConfigurationItem> configurationItems, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<ConfigurationItem>(configurationItems, modifiedBy);
        }
    }
}
