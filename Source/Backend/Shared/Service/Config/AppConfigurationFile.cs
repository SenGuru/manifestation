using System.IO;
using Framework.Infrastructure.Utils;
using Framework.Infrastructure.Constants;
using Manifest.Common.Constants;

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

        public string GetConfigFileFolder()
        {
            return Path.GetDirectoryName(GetFileName());
        }

        public string GetReplacedConfigFolderString(string value)
        {
            if (value == null)
                return null;

            return value.Replace(Strings.Config.ConfigPath, GetConfigFileFolder());
        }
    }
}
