using System.IO;
using Manifest.Common.Constants;
using Framework.Infrastructure.Utils;

namespace Manifest.Service.Config
{
    public delegate string OnGetConfigFileName();

    public class AppConfigurationFile
    {
        public string GetFileName()
        {
            var path = ConfigUtils.GetConfigFileName(ConfigurationConstants.ConfigurationFileName);

            if (File.Exists(path))
                return path;

            return ConfigUtils.GetConfigFileName(ConfigurationConstants.ConfigurationFileName, useCurrentDirectory: true);
        }

        public string GetOverrideFileName()
        {
            var path = GetFileName();
            if (path.IsTrimmedStringNullOrEmpty())
                return path;
            return Path.ChangeExtension(path, "override.json");
        }
    }
}
