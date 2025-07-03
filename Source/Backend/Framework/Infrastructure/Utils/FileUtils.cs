/**
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see <https://opensource.org/licenses/BSD-3-Clause>.
**/
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HeyRed.Mime;

namespace Framework.Infrastructure.Utils
{
    public static class FileUtils
    {
        public static void CloseSafely(Stream stream)
        {
            if (stream == null)
                return;

#pragma warning disable SA1501,S2486,S108,S1854 // Statement should not be on a single line
            try
            {
                stream.Close();
                stream = null;
            }
            catch
            {
            }
#pragma warning restore SA1501,S2486,S108,S1854 // Statement should not be on a single line
        }

        public static string SanitizeValidFileName(string name)
        {
            string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            return System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "_");
        }

        public static ulong GetDirectorySize(string dir, string filter = "*.*", bool ignoreHiddenFiles = false)
        {
            var filenameList = Directory.GetFiles(dir, filter);
            ulong size = 0;
            foreach (var filename in filenameList)
            {
                var info = new FileInfo(filename);
                if (ignoreHiddenFiles)
                {
                    if ((info.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                    {
                        continue;
                    }
                }

                size += (ulong)info.Length;
            }

            return size;
        }

        public static string GetFileDirectory(string exeName)
        {
            try
            {
                var fInfo = new FileInfo(exeName);
                return fInfo.DirectoryName;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string GetApplicationExeDirectory()
        {
            try
            {
                var fi = new FileInfo(Assembly.GetEntryAssembly().Location);
                return fi.DirectoryName;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static void CreateFileParentDirectoryIsNotExistsFor(string fileName)
        {
            var path = Path.GetDirectoryName(fileName);
            if (!File.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static string Combine(string path1, string path2, params string[] paramstrs)
        {
            return CombineInternal(Path.DirectorySeparatorChar, path1, path2, paramstrs);
        }

        public static string GetCurrentDirectory()
        {
            return Path.GetFullPath(Directory.GetCurrentDirectory());
        }

        public static string GetMimeTypeFromFileName(string fileName)
        {
            if (fileName.IsTrimmedStringNullOrEmpty())
            {
                return string.Empty;
            }

            return MimeTypesMap.GetMimeType(fileName);
        }

        public static double FileSizeInMB(double fileSizeInBytes)
        {
            return (fileSizeInBytes / 1024f) / 1024f;
        }

        public static double FileSizeInGB(double fileSizeInBytes)
        {
            return ((fileSizeInBytes / 1024f) / 1024f) / 1024f;
        }

        public static double FileSizeInTB(double fileSizeInBytes)
        {
            return (((fileSizeInBytes / 1024f) / 1024f) / 1024f) / 1024f;
        }

        public static bool DeleteFileSafely(string fileName)
        {
            try
            {
                if (!File.Exists(fileName))
                    return true;

                File.Delete(fileName);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool DeleteFilesSafely(List<string> fileNames)
        {
            try
            {
                foreach (string fileName in fileNames)
                {
                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string GetFileExtension(string fileName)
        {
            try
            {
                var fInfo = new FileInfo(fileName);
                return fInfo.Extension;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string GetFileName(string fileName)
        {
            try
            {
                var fInfo = new FileInfo(fileName);
                return fInfo.Name;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static long GetFileSize(string fileName)
        {
            var fInfo = new FileInfo(fileName);
            return fInfo.Length;
        }

        public static string GetRandomFileNameInTempDirectory()
        {
            return Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        }

        public static string GetGuidFileNameInTempDirectory()
        {
            return Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        }

        public static string BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }

        public static async Task<FileStream> WaitForFileAsync(string fullPath, int useDelayInMilliseconds = 500)
        {
            for (int numTries = 0; numTries < 10; numTries++)
            {
                FileStream fs = null;
                try
                {
                    fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.None);
                    return fs;
                }
                catch (IOException)
                {
                    if (fs != null)
                    {
                        await fs.DisposeAsync();
                    }

                    await Task.Delay(useDelayInMilliseconds);
                }
            }

            return null;
        }

        public static List<string> GetFileNamesRecursively(string path, string goToOnlyThisfolder = null, string fileSearchPattern = "*", SearchOption fileSearchOption = SearchOption.AllDirectories)
        {
            List<string> fileNames = new List<string>();
#pragma warning disable S108, S2486 // Generic exceptions should not be ignored
            try
            {
                if (goToOnlyThisfolder != null)
                {
                    string[] directories = Directory.GetDirectories(path, goToOnlyThisfolder, SearchOption.AllDirectories);
                    foreach (string directory in directories)
                    {
                        if (directory.EndsWith(goToOnlyThisfolder))
                        {
                            string[] searchPatterns = fileSearchPattern.Split('|');
                            foreach (string pattern in searchPatterns)
                            {
                                fileNames.AddRange(Directory.GetFiles(directory, pattern, fileSearchOption));
                            }
                        }
                    }
                }
                else
                {
                    string[] searchPatterns = fileSearchPattern.Split('|');
                    foreach (string pattern in searchPatterns)
                    {
                        fileNames.AddRange(Directory.GetFiles(path, pattern, fileSearchOption));
                    }
                }
            }
            catch
            {
            }
#pragma warning restore S108, S2486 // Generic exceptions should not be ignored

            return fileNames;
        }

        public static MemoryStream GetFileAsMemoryStream(string filename)
        {
            MemoryStream ms = new MemoryStream();
            using (FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read))
                file.CopyTo(ms);
            ms.Position = 0;
            return ms;
        }

        public static void WriteMemoryStreamToDisk(MemoryStream ms, string filename)
        {
            ms.Position = 0;

            using (FileStream file = new FileStream(filename, FileMode.Create, FileAccess.Write))
                ms.CopyTo(file);
        }

        public static string GetFilePathRelativeToApplicationExe(string relativePath)
        {
            string applicationPath = FileUtils.GetApplicationExeDirectory();

            while (true)
            {
                var invoiceDirectoryPath = Path.Combine(applicationPath, relativePath);
                if (File.Exists(invoiceDirectoryPath))
                {
                    return invoiceDirectoryPath;
                }
                else if (Path.GetPathRoot(applicationPath) == applicationPath)
                {
                    break;
                }
                else
                {
                    var di = Directory.GetParent(applicationPath);
                    if (di == null)
                        break;

                    applicationPath = di.ToString();
                }
            }

            return "";
        }

        private static string CombineInternal(char slash, string path1, string path2, params string[] paramstrs)
        {
            return string.Format("{0}{1}{2}{3}", path1.RemoveLastChar(slash), slash, path2.RemoveFirstChar(slash), (paramstrs == null || paramstrs.Length == 0) ? string.Empty : PathString(paramstrs, slash));
        }

        private static string PathString(string[] paramstrs, char slash)
        {
            var sb = new StringBuilder();
            foreach (var item in paramstrs)
            {
                sb.Append(StringUtils.RemoveLastCharAndAddFirstChar(item, slash));
            }

            return sb.ToString();
        }
    }
}
