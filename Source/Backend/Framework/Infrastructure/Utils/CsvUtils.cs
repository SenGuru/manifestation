/**
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see <https://opensource.org/licenses/BSD-3-Clause>.
**/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Security;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;

namespace Framework.Infrastructure.Utils
{
    public static class CsvUtils
    {
        public static void AppendCsvDataWithout<T>(string fileName, List<T> data)
        {
            AppendCsvDataWithoutInternal<T>(fileName, null, data);
        }

        public static Task AppendCsvDataWithoutAsync<T>(string fileName, List<T> data)
        {
            return AppendCsvDataWithoutInternalAsync<T>(fileName, null, data);
        }

        public static void AppendCsvDataToStreamWithout<T>(Stream stream, List<T> data)
        {
            AppendCsvDataWithoutInternal<T>(null, stream, data);
        }

        public static Task AppendCsvDataToStreamWithoutAsync<T>(Stream stream, List<T> data)
        {
            return AppendCsvDataWithoutInternalAsync<T>(null, stream, data);
        }

        public static void WriteCsvDataWithHeader<T>(string fileName, List<T> data)
        {
            WriteCsvDataWithHeaderInternal(fileName, null, data);
        }

        public static Task WriteCsvDataWithHeaderAsync<T>(string fileName, List<T> data)
        {
            return WriteCsvDataWithHeaderInternalAsync(fileName, null, data);
        }

        public static void WriteCsvDataToStreamWithHeader<T>(Stream stream, List<T> data)
        {
            WriteCsvDataWithHeaderInternal(null, stream, data);
        }

        public static Task WriteCsvDataToStreamWithHeaderAsync<T>(Stream stream, List<T> data)
        {
            return WriteCsvDataWithHeaderInternalAsync(null, stream, data);
        }

        public static Task AppendCsvDataWithoutHeaderInternalAsyncExcludeIds<T>(string fileName, Stream stream, List<T> data)
        {
            return AppendCsvDataWithoutHeaderInternalAsync(fileName, stream, data, columnName => columnName.EndsWith("id", StringComparison.OrdinalIgnoreCase) || columnName.EndsWith("ids", StringComparison.OrdinalIgnoreCase));
        }

        public static Task AppendCsvDataWithoutHeaderInternalAsyncExcludeColumns<T>(string fileName, Stream stream, List<T> data, IEnumerable<string> excludeColumns)
        {
            return AppendCsvDataWithoutHeaderInternalAsync(fileName, stream, data, columnName => excludeColumns.Contains(columnName, StringComparer.OrdinalIgnoreCase));
        }

        public static Task AppendCsvDataWithoutHeaderInternalAsyncExcludeBoth<T>(string fileName, Stream stream, List<T> data, IEnumerable<string> excludeColumns)
        {
            return AppendCsvDataWithoutHeaderInternalAsync(fileName, stream, data, columnName => excludeColumns.Contains(columnName, StringComparer.OrdinalIgnoreCase) || columnName.EndsWith("id", StringComparison.OrdinalIgnoreCase) || columnName.EndsWith("ids", StringComparison.OrdinalIgnoreCase));
        }

        public static Task WriteCsvDataWithHeaderInternalAsyncExcludeIds<T>(string fileName, Stream stream, List<T> data)
        {
            return WriteCsvDataWithHeaderInternalAsync(fileName, stream, data, columnName => columnName.EndsWith("id", StringComparison.OrdinalIgnoreCase) || columnName.EndsWith("ids", StringComparison.OrdinalIgnoreCase));
        }

        public static Task WriteCsvDataWithHeaderInternalAsyncExcludeColumns<T>(string fileName, Stream stream, List<T> data, IEnumerable<string> excludeColumns)
        {
            return WriteCsvDataWithHeaderInternalAsync(fileName, stream, data, columnName => excludeColumns.Contains(columnName, StringComparer.OrdinalIgnoreCase));
        }

        public static Task WriteCsvDataWithHeaderInternalAsyncExcludeBoth<T>(string fileName, Stream stream, List<T> data, IEnumerable<string> excludeColumns)
        {
            return WriteCsvDataWithHeaderInternalAsync(fileName, stream, data, columnName => excludeColumns.Contains(columnName, StringComparer.OrdinalIgnoreCase) || columnName.EndsWith("id", StringComparison.OrdinalIgnoreCase) || columnName.EndsWith("ids", StringComparison.OrdinalIgnoreCase));
        }

        public static MemoryStream GetCsvFileContent<T>(List<T> obj)
            where T : class
        {
            var config = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                Delimiter = ",",
                HasHeaderRecord = true,
            };

            var mem = new MemoryStream();
            var writer = new StreamWriter(mem);
            using (var csvWriter = new CsvWriter(writer, config, true))
            {
                csvWriter.WriteHeader<T>();
                csvWriter.NextRecord();
                csvWriter.WriteRecords(obj);
                writer.Flush();
            }

            mem.Position = 0;
            return mem;
        }

        public static IEnumerable<T> ReadCommaSeparatedFile<T>(string filePath, bool hasHeaderRecord = false)
        {
            using (var reader = new StreamReader(File.Open(filePath, FileMode.Open)))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = hasHeaderRecord,
                HeaderValidated = null,
                MissingFieldFound = null
            }))
            {
                while (csv.Read())
                {
                    var record = csv.GetRecord<T>();
                    yield return record;
                }
            }
        }

        public static IEnumerable<T> ReadCommaSeparatedFile<T>(Stream stream, bool hasHeaderRecord = false)
        {
            using (var reader = new StreamReader(stream))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = hasHeaderRecord,
                HeaderValidated = null,
                MissingFieldFound = null
            }))
            {
                if (hasHeaderRecord)
                {
                    csv.Read();
                    csv.ReadHeader();
                }

                var classMap = CreateDynamicClassMap<T>(csv.HeaderRecord);
                if (classMap != null)
                {
                    csv.Context.RegisterClassMap(classMap);

                    while (csv.Read())
                    {
                        var record = csv.GetRecord<T>();
                        yield return record;
                    }
                }
                else
                {
                    throw new InvalidOperationException("Failed to create a valid class map for the provided headers.");
                }
            }
        }

        public static IEnumerable<T> ReadCommaSeparatedFile<T>(ZipArchiveEntry tabDelimitedEntry, bool hasHeaderRecord = false)
        {
            using (var reader = new StreamReader(tabDelimitedEntry.Open()))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = hasHeaderRecord,
                HeaderValidated = null,
                MissingFieldFound = null
            }))
            {
                while (csv.Read())
                {
                    var record = csv.GetRecord<T>();
                    yield return record;
                }
            }
        }

        public static IEnumerable<T> ReadTabDelimitedFile<T>(string filePath, bool hasHeaderRecord = false)
        {
            using (var reader = new StreamReader(File.Open(filePath, FileMode.Open)))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = "\t",
                HasHeaderRecord = hasHeaderRecord,
                HeaderValidated = null,
                MissingFieldFound = null
            }))
            {
                while (csv.Read())
                {
                    var record = csv.GetRecord<T>();
                    yield return record;
                }
            }
        }

        public static IEnumerable<T> ReadTabDelimitedFile<T>(Stream stream, bool hasHeaderRecord = false)
        {
            using (var reader = new StreamReader(stream))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = "\t",
                HasHeaderRecord = hasHeaderRecord,
                HeaderValidated = null,
                MissingFieldFound = null
            }))
            {
                while (csv.Read())
                {
                    var record = csv.GetRecord<T>();
                    yield return record;
                }
            }
        }

        public static IEnumerable<T> ReadTabDelimitedFile<T>(ZipArchiveEntry tabDelimitedEntry, bool hasHeaderRecord = false)
        {
            using (var reader = new StreamReader(tabDelimitedEntry.Open()))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = "\t",
                HasHeaderRecord = hasHeaderRecord,
                HeaderValidated = null,
                MissingFieldFound = null
            }))
            {
                while (csv.Read())
                {
                    var record = csv.GetRecord<T>();
                    yield return record;
                }
            }
        }

        private static ClassMap<T> CreateDynamicClassMap<T>(string[] headers)
        {
            var classMap = new DefaultClassMap<T>();
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                if (headers.Contains(property.Name, StringComparer.OrdinalIgnoreCase))
                {
                    classMap.Map(typeof(T), property);
                }
            }

            return classMap;
        }

        private static void AppendCsvDataWithoutInternal<T>(string fileName, Stream stream, List<T> data)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
            };

            var csvStream = fileName == null ? stream : File.Open(fileName, FileMode.Append);
            var writer = new StreamWriter(csvStream);
            using (var csv = new CsvWriter(writer, config, true))
            {
                csv.WriteRecords(data);
                writer.Flush();
            }
        }

        private static async Task AppendCsvDataWithoutInternalAsync<T>(string fileName, Stream stream, List<T> data)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
            };

            var csvStream = fileName == null ? stream : File.Open(fileName, FileMode.Append);
            var writer = new StreamWriter(csvStream);
            using (var csv = new CsvWriter(writer, config, true))
            {
                await csv.WriteRecordsAsync(data);
                await writer.FlushAsync();
            }
        }

        private static async Task AppendCsvDataWithoutHeaderInternalAsync<T>(string fileName, Stream stream, List<T> data, Func<string, bool> excludeColumn = null)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
            };

            var csvStream = fileName == null ? stream : File.Open(fileName, FileMode.Append);
            var writer = new StreamWriter(csvStream);

            using (var csv = new CsvWriter(writer, config, true))
            {
                if (excludeColumn != null)
                {
                    csv.Context.AutoMap<T>();
                    foreach (var member in csv.Context.AutoMap<T>().MemberMaps)
                    {
                        if (excludeColumn(member.Data.Names.FirstOrDefault()))
                        {
                            member.Ignore();
                        }
                    }
                }

                await csv.WriteRecordsAsync(data);
                await writer.FlushAsync();
            }
        }

        private static void WriteCsvDataWithHeaderInternal<T>(string fileName, Stream stream, List<T> data)
        {
            var configForHeader = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            };

            using (var writer = fileName == null ? new StreamWriter(stream) : new StreamWriter(fileName))
            using (var csv = new CsvWriter(writer, configForHeader))
            {
                csv.WriteRecords(data);
            }
        }

        private static async Task WriteCsvDataWithHeaderInternalAsync<T>(string fileName, Stream stream, List<T> data, Func<string, bool> excludeColumn = null)
        {
            var configForHeader = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            };

            using (var writer = fileName == null ? new StreamWriter(stream) : new StreamWriter(fileName))
            using (var csv = new CsvHelper.CsvWriter(writer, configForHeader))
            {
                if (excludeColumn != null)
                {
                    csv.Context.AutoMap<T>();
                    foreach (var member in csv.Context.AutoMap<T>().MemberMaps)
                    {
                        if (excludeColumn(member.Data.Names.FirstOrDefault()))
                        {
                            member.Ignore();
                        }
                    }
                }

                await csv.WriteRecordsAsync(data);
            }
        }
    }
}
