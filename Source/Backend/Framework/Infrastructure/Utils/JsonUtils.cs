/**
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see <https://opensource.org/licenses/BSD-3-Clause>.
**/
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Framework.Infrastructure.Helpers;

namespace Framework.Infrastructure.Utils
{
    public static class JsonUtils
    {
        private static JsonSerializerOptions opt1 = new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, WriteIndented = false };
        private static JsonSerializerOptions opt1WithTrueIndent = new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, WriteIndented = true };
        private static JsonSerializerOptions opt2 = new JsonSerializerOptions { AllowTrailingCommas = true, PropertyNameCaseInsensitive = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
        private static JsonSerializerOptions opt3 = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, WriteIndented = false };
        private static JsonSerializerOptions opt3WithTrueIndent = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, WriteIndented = false };
        private static JsonSerializerOptions opt4 = new JsonSerializerOptions { AllowTrailingCommas = true, PropertyNameCaseInsensitive = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase, DefaultIgnoreCondition = JsonIgnoreCondition.Never };
        private static JsonSerializerOptions opt4WithTrueIndent = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, DefaultIgnoreCondition = JsonIgnoreCondition.Never, WriteIndented = false };

        static JsonUtils()
        {
            opt2.Converters.Add(new JsonStringEnumConverter());
            opt3.Converters.Add(new JsonStringEnumConverter());
            opt3WithTrueIndent.Converters.Add(new JsonStringEnumConverter());
        }

        public static T DeserializeWithoutPropertyNaming<T>(string jsonStr)
        {
            return JsonSerializer.Deserialize<T>(jsonStr);
        }

        public static (T t, Exception ex) DeserializeWithoutPropertyNamingWithErrorHandled<T>(string jsonStr)
        {
            try
            {
                return (DeserializeWithoutPropertyNaming<T>(jsonStr), null);
            }
            catch (Exception ex1)
            {
                return (default(T), ex1);
            }
        }

        public static string SerializeWithoutPropertyNaming<T>(T obj, bool formatted = false)
        {
            return JsonSerializer.Serialize<T>(obj, formatted ? opt1 : opt1WithTrueIndent);
        }

        public static T Deserialize<T>(string jsonStr)
        {
            return JsonSerializer.Deserialize<T>(jsonStr, opt2);
        }

        public static T DeserializeWithNoIgnore<T>(string jsonStr)
        {
            return JsonSerializer.Deserialize<T>(jsonStr, opt4);
        }

        public static (T t, Exception ex) DeserializeWithErrorHandled<T>(string jsonStr)
        {
            try
            {
                return (Deserialize<T>(jsonStr), null);
            }
            catch (Exception ex1)
            {
                return (default(T), ex1);
            }
        }

        public static string Serialize<T>(T obj, bool formatted = false)
        {
            return JsonSerializer.Serialize<T>(obj, formatted ? opt3 : opt3WithTrueIndent);
        }

        public static string SerializeWithNoIgnore<T>(T obj, bool formatted = false)
        {
            return JsonSerializer.Serialize<T>(obj, formatted ? opt4 : opt4WithTrueIndent);
        }

        public static string SerializeWithoutIdFields<T>(T obj, bool formatted = false)
        {
            var option = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.Never,
                Converters = { new IgnoreIdFieldsConverter<T>() }
            };

            return JsonSerializer.Serialize<T>(obj, option);
        }

        public static string SerializeWithoutFields<T>(T obj, IEnumerable<string> ignoreFields, bool formatted = false)
        {
            var option = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.Never,
                Converters = { new IgnoreFieldsConverter<T>(ignoreFields) }
            };

            return JsonSerializer.Serialize<T>(obj, option);
        }

        public static string SerializeWithoutFieldsAndIds<T>(T obj, IEnumerable<string> ignoreFields, bool formatted = false)
        {
            var option = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.Never,
                Converters = { new IgnoreFieldsAndIdFieldsConverter<T>(ignoreFields) }
            };

            return JsonSerializer.Serialize<T>(obj, option);
        }

        public static MemoryStream GetJsonContent<T>(List<T> obj)
            where T : class
        {
            var mem = new MemoryStream(Encoding.Default.GetBytes(JsonSerializer.Serialize(obj)));
            mem.Position = 0;
            return mem;
        }

        public static MemoryStream GetJsonContent<T>(T obj)
            where T : class
        {
            var mem = new MemoryStream(Encoding.Default.GetBytes(JsonSerializer.Serialize(obj)));
            mem.Position = 0;
            return mem;
        }

        public static bool IsValidJson(string source)
        {
            if (source == null)
                return false;

            try
            {
                JsonDocument.Parse(source);
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }

        public static IEnumerable<T> ParseJsonContent<T>(string jsonContent)
        {
            using (JsonDocument document = JsonDocument.Parse(jsonContent))
            {
                if (document.RootElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (JsonElement element in document.RootElement.EnumerateArray())
                    {
                        var dataModel = System.Text.Json.JsonSerializer.Deserialize<T>(element.GetRawText());
                        yield return dataModel;
                    }
                }
                else
                {
                    var dataModel = System.Text.Json.JsonSerializer.Deserialize<T>(jsonContent);
                    yield return dataModel;
                }
            }
        }

        public static IEnumerable<T> ParseJsonContent<T>(Stream json)
        {
            using (StreamReader reader = new StreamReader(json))
            {
                string jsonString = reader.ReadToEnd();
                using (JsonDocument document = JsonDocument.Parse(jsonString))
                {
                    if (document.RootElement.ValueKind == JsonValueKind.Array)
                    {
                        foreach (JsonElement element in document.RootElement.EnumerateArray())
                        {
                            var dataModel = System.Text.Json.JsonSerializer.Deserialize<T>(element.GetRawText());
                            yield return dataModel;
                        }
                    }
                    else
                    {
                        var dataModel = System.Text.Json.JsonSerializer.Deserialize<T>(jsonString);
                        yield return dataModel;
                    }
                }
            }
        }
    }
}
