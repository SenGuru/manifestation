using System;
using System.Collections.Generic;
using Manifest.Common.Models.DB;
using Framework.Data.DbAccess;
using Framework.Infrastructure.Constants;
using Framework.Infrastructure.Utils;

namespace Manifest.Service.Config
{
    public abstract class DefaultConfigurationPopulator
    {
        private readonly IConfigDBManager appDBManager;
        private readonly int groupId;
        private readonly AppConfigurationFile appConfigurationFile;

        protected DefaultConfigurationPopulator(IConfigDBManager appDBManager, int groupId, AppConfigurationFile appConfigurationFile)
        {
            this.appDBManager = appDBManager;
            this.groupId = groupId;
            this.appConfigurationFile = appConfigurationFile;
        }

        protected void RefreshConfig()
        {
            PopulateConfig(groupId);
        }

        protected void PopulateConfig(int groupId)
        {
            var configLocation = appConfigurationFile.GetFileName();
            var dict = new Dictionary<string, string>();
#pragma warning disable S2486,S108 // Generic exceptions should not be ignored
            try
            {
                var overrideConfigLocation = appConfigurationFile.GetOverrideFileName();
                if (System.IO.File.Exists(overrideConfigLocation))
                {
                    var json = System.IO.File.ReadAllText(overrideConfigLocation);
                    (dict, Exception ex2) = JsonUtils.DeserializeWithErrorHandled<Dictionary<string, string>>(json);
                    if (ex2 != null)
                        dict = new Dictionary<string, string>();
                }
            }
            catch
            {
                dict = new Dictionary<string, string>();
            }
#pragma warning restore S2486,S108 // Generic exceptions should not be ignored

            var configItems = appDBManager.Select<ConfigurationItem>(x => x.ConfigurationGroupId == groupId);
            foreach (var item in configItems)
            {
                if (item.Key.IsTrimmedStringNullOrEmpty() || item.Value.IsTrimmedStringNullOrEmpty())
                {
                    continue;
                }

                foreach (var prop in this.GetType().GetProperties())
                {
                    if (!item.Key.IsSameWithCaseIgnoreTrimmed(prop.Name))
                    {
                        continue;
                    }

                    string overriddenValue = null;
                    if (dict.Keys.Count > 0)
                    {
                        foreach (var k in dict.Keys)
                        {
                            if (item.Key.IsSameWithCaseIgnoreTrimmed(k))
                            {
                                overriddenValue = dict[k];
                                break;
                            }
                        }
                    }

                    var str = overriddenValue.IsTrimmedStringNullOrEmpty() ? item.Value : overriddenValue;

                    if (str != null && (!str.IsTrimmedStringNullOrEmpty()) && str.Contains(Strings.Config.ConfigPath))
                    {
                        str = str.Replace(Strings.Config.ConfigPath, FileUtils.GetFileDirectory(configLocation));
                    }

                    ReflectionUtils.SetPropertyValueFromString(this, prop, str, "");
                }
            }

            appDBManager.Dispose();
        }
    }
}