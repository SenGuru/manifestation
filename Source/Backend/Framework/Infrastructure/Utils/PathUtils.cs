using System;
using System.Collections.Generic;
using System.IO;

namespace Framework.Infrastructure.Utils
{
    public static class PathUtils
    {
        public static string CreatePath(string basePath, string fileName)
        {
            string filepath;

            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            filepath = FileUtils.Combine(basePath, fileName);
            return filepath;
        }

        public static string GetConfigPath(string configPath)
        {
            if (configPath.IsTrimmedStringNotNullOrEmpty())
            {
                if (Directory.Exists(configPath))
                {
                    return configPath;
                }
                else
                {
                    Directory.CreateDirectory(configPath);
                    return configPath;
                }
            }
            else
            {
                throw new Exception("Error in path creation.");
            }
        }

        public static string CreateMultiLevelPath(string basePath, List<string> lstString)
        {
            string path = basePath;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            foreach (var str in lstString)
            {
                path = FileUtils.Combine(path, str);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }

            return path;
        }
    }
}