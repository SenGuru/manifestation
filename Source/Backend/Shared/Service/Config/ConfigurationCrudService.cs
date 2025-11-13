using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manifest.Common.Attributes;
using Manifest.Common.Constants;
using Manifest.Common.Models.ConfigModels;
using Manifest.Common.Models.DB;
using Manifest.Repository.Common;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Utils;

namespace Manifest.Service.Completed.Config
{
    public class ConfigurationCrudService
    {
        private readonly ConfigurationRepository configurationRepository;
        private readonly ILog log;

        public ConfigurationCrudService(ILog log, ConfigurationRepository configurationRepository)
        {
            this.log = log;
            this.configurationRepository = configurationRepository;
        }

        public async Task<ReturnModel<T>> GeConfigurationByGroupIdAsync<T>(long groupId)
            where T : class, new()
        {
            try
            {
                var configItems = await configurationRepository.GetAllConfigurationEntriesAsync(groupId);
                return ReturnModel<T>.Success(ConfigurationToObject<T>(configItems));
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<T>($"Unexpected error when getting configurations", new { groupId }, ex);
            }
        }

        public void ConfigurationToObjectInternal(object t, List<ConfigurationItem> configurationItems)
        {
            var lst = t.GetType().GetProperties().ToList();
            foreach (var config in configurationItems)
            {
                var prop = lst.FirstOrDefault(x => x.Name.Trim().ToLower() == config.Key.Trim().ToLower());
                if (prop == null)
                    continue;
                var catts = prop.GetCustomAttributes(typeof(ComplexConfigItemAttribute), true);
                if (catts != null && catts.Length > 0)
                    return;

                ReflectionUtils.SetPropertyValueFromString(t, prop, config.Value, null);
            }

            /*foreach (var prop in lst)
            {
                var atts = prop.GetCustomAttributes(typeof(ComplexConfigItemAttribute), true);

                if (atts == null || atts.Length == 0)
                    continue;

                ConfigurationToObjectInternal(prop.GetValue(t), configurationItems);
            }*/
        }

        public T ConfigurationToObject<T>(List<ConfigurationItem> configurationItems)
            where T : class, new()
        {
            var t = new T();
            ConfigurationToObjectInternal(t, configurationItems);
            return t;
        }

        public List<ConfigurationItem> ObjectToConfiguration<T>(List<ConfigurationItem> dbConfigurationItems, T obj)
            where T : class
        {
            var result = new List<ConfigurationItem>();
            foreach (var prop in obj.GetType().GetProperties())
            {
                var config = dbConfigurationItems.FirstOrDefault(x => x.Key.Trim().ToLower() == prop.Name.Trim().ToLower());
                if (config == null)
                    continue;
                var pValue = prop.GetValue(obj);
                config.Value = pValue == null ? " " : pValue.ToString();
                result.Add(config);
                //ReflectionUtils.SetPropertyValueFromString(obj, prop, config.Value ?? " "," ");
            }

            return result;
        }

        public async Task<ReturnModel<bool>> SaveSearchConfigurationByGroupIdAsync<T>(long groupId, T obj)
            where T : class
        {
            try
            {
                var existingConfigs = await configurationRepository.GetAllConfigurationEntriesAsync(groupId);
                var configurationItems = ObjectToConfiguration<T>(existingConfigs, obj);
                await configurationRepository.SaveAllConfigurationEntriesAsync(configurationItems, null);
                return ReturnModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when saving the configurations", new { groupId, obj }, ex);
            }
        }

        public Task<ReturnModel<ManifestWebConfigurationModel>> GetWebConfigurationAsync()
        {
            return GeConfigurationByGroupIdAsync<ManifestWebConfigurationModel>(ConfigurationConstants.ConfigurationGroup.ManifestWebConfiguration);
        }

        public Task<ReturnModel<bool>> SaveWebConfigurationAsync(ManifestWebConfigurationModel obj)
        {
            return SaveSearchConfigurationByGroupIdAsync(ConfigurationConstants.ConfigurationGroup.ManifestWebConfiguration, obj);
        }

        public Task<ReturnModel<LogConfigurationModel>> GetLogConfigurationAsync()
        {
            return GeConfigurationByGroupIdAsync<LogConfigurationModel>(ConfigurationConstants.ConfigurationGroup.BaseConfiguration);
        }

        public Task<ReturnModel<bool>> SaveLogConfigurationAsync(LogConfigurationModel obj)
        {
            return SaveSearchConfigurationByGroupIdAsync(ConfigurationConstants.ConfigurationGroup.BaseConfiguration, obj);
        }

        public Task<ReturnModel<AppConfigurationModel>> GetAppConfigurationAsync()
        {
            return GeConfigurationByGroupIdAsync<AppConfigurationModel>(ConfigurationConstants.ConfigurationGroup.BaseConfiguration);
        }

        public Task<ReturnModel<bool>> SaveAppConfigurationAsync(AppConfigurationModel obj)
        {
            return SaveSearchConfigurationByGroupIdAsync(ConfigurationConstants.ConfigurationGroup.BaseConfiguration, obj);
        }

        public Task<ReturnModel<CommonConfigurationModel>> GetCommonConfigurationAsync()
        {
            return GeConfigurationByGroupIdAsync<CommonConfigurationModel>(ConfigurationConstants.ConfigurationGroup.CommonConfiguration);
        }

        public Task<ReturnModel<bool>> SaveCommonConfigurationAsync(CommonConfigurationModel obj)
        {
            return SaveSearchConfigurationByGroupIdAsync(ConfigurationConstants.ConfigurationGroup.CommonConfiguration, obj);
        }

        public Task<ReturnModel<CoordinatorConfigurationModel>> GetCoordinatorConfigurationAsync()
        {
            return GeConfigurationByGroupIdAsync<CoordinatorConfigurationModel>(ConfigurationConstants.ConfigurationGroup.CoordinatorConfiguration);
        }

        public Task<ReturnModel<bool>> SaveCoordinatorConfigurationAsync(CoordinatorConfigurationModel obj)
        {
            return SaveSearchConfigurationByGroupIdAsync(ConfigurationConstants.ConfigurationGroup.CoordinatorConfiguration, obj);
        }

        public Task<ReturnModel<ReportServerConfigurationModel>> GetReportServerConfigurationAsync()
        {
            return GeConfigurationByGroupIdAsync<ReportServerConfigurationModel>(ConfigurationConstants.ConfigurationGroup.ReportServerConfiguration);
        }

        public Task<ReturnModel<bool>> SaveReportServerConfigurationAsync(ReportServerConfigurationModel obj)
        {
            return SaveSearchConfigurationByGroupIdAsync(ConfigurationConstants.ConfigurationGroup.ReportServerConfiguration, obj);
        }
    }
}
