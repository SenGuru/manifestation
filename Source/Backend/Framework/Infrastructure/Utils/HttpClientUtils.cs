/**
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see <https://opensource.org/licenses/BSD-3-Clause>.
**/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Framework.Infrastructure.Attributes;
using Framework.Infrastructure.Models.FileInput;
using Framework.Infrastructure.Utils;

namespace AspNetCore.Http.Extensions
{
    public static class HttpClientUtils
    {
        public static async Task<(HttpResponseMessage msg, T result)> DeleteAndReadAsJsonAsync<T>(
            this HttpClient httpClient, string url, object headerObj = null)
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(url, UriKind.Relative),
                Method = HttpMethod.Delete,
            };
            AddToHttpHeaders(request.Headers, headerObj);

            var msg = await httpClient.SendAsync(request);
            if (msg.StatusCode != HttpStatusCode.OK && msg.StatusCode != HttpStatusCode.Created)
                return (msg, await SafeReadAsJsonAsync<T>(msg.Content, default(T)));

            var t = await ReadAsJsonAsync<T>(msg.Content);

            return (msg, t);
        }

        public static async Task<HttpResponseMessage> PostAsStringAsync<T>(
            this HttpClient httpClient, string url, T data, object headerObj = null)
            where T : struct
        {
            var dataAsString = data.ToString();
            var content = new StringContent(dataAsString);
            AddToHttpHeaders(content.Headers, headerObj);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            try
            {
                var result = await httpClient.PostAsync(url, content);
                return result;
            }
            finally
            {
                content.Dispose();
#pragma warning disable IDE0059, S1854  // Unnecessary assignment of a value
                dataAsString = null;
#pragma warning restore IDE0059, S1854  // Unnecessary assignment of a value
            }
        }

        public static async Task<(HttpResponseMessage msg, T result)> PostAsStringAndReadAsJsonAsync<T>(
            this HttpClient httpClient, string url, T data, object headerObj = null)
            where T : struct
        {
            var msg = await PostAsStringAsync(httpClient, url, data, headerObj);
            if (msg.StatusCode != HttpStatusCode.OK && msg.StatusCode != HttpStatusCode.Created)
                return (msg, await SafeReadAsJsonAsync<T>(msg.Content, default(T)));

            var t = await ReadAsJsonAsync<T>(msg.Content);

            return (msg, t);
        }

        public static async Task<(HttpResponseMessage msg, TOutput result)> PostAsStringAndReadAsJsonAsync<TOutput, TInput>(
            this HttpClient httpClient, string url, TInput data, object headerObj = null)
            where TInput : struct
        {
            var msg = await PostAsStringAsync(httpClient, url, data, headerObj);
            if (msg.StatusCode != HttpStatusCode.OK && msg.StatusCode != HttpStatusCode.Created)
                return (msg, await SafeReadAsJsonAsync<TOutput>(msg.Content, default(TOutput)));

            var t = await ReadAsJsonAsync<TOutput>(msg.Content);

            return (msg, t);
        }

        public static async Task<HttpResponseMessage> PostAsJsonAsync<T>(
            this HttpClient httpClient, string url, T data, object headerObj = null)
        {
            var dataAsString = JsonUtils.Serialize(data);
            var content = new StringContent(dataAsString);

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };

            AddToHttpHeaders(request.Headers, headerObj);

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            try
            {
                var result = await httpClient.PostAsync(url, content);
                return result;
            }
            finally
            {
                content.Dispose();
#pragma warning disable IDE0059, S1854  // Unnecessary assignment of a value
                dataAsString = null;
#pragma warning restore IDE0059, S1854  // Unnecessary assignment of a value
            }
        }

        public static Task<HttpResponseMessage> PostAsFormAsync<T>(
            this HttpClient httpClient, string url, T data, object headerObj = null)
        {
            var content = new MultipartFormDataContent();
            foreach (var prop in data.GetType().GetProperties())
            {
                var value = prop.GetValue(data);
                if (value is FileStreamModel)
                {
                    var file = value as FileStreamModel;
                    content.Add(new StreamContent(file.FileData), prop.Name, file.FileName);
                    content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = prop.Name, FileName = file.FileName };
                }
                else if (value is Array || value is IList)
                {
                    IEnumerable valueList = value as IEnumerable;
                    if (valueList != null)
                    {
                        foreach (object val in valueList)
                        {
                            content.Add(new StringContent(JsonUtils.Serialize(val)), prop.Name);
                        }
                    }
                }
                else if (value is string)
                {
                    var v = value as string;
                    if (v == null)
                        continue;

                    content.Add(new StringContent(v), prop.Name);
                }
                else
                {
                    if (value == null)
                        continue;
                    content.Add(new StringContent(value.ToString()), prop.Name);
                }
            }

            AddToHttpHeaders(content.Headers, headerObj);
            return httpClient.PostAsync(url, content);
        }

        public static async Task<(HttpResponseMessage msg, T result)> PostAndReadAsJsonAsync<T>(
            this HttpClient httpClient, string url, T data, object headerObj = null)
        {
            var msg = await PostAsJsonAsync(httpClient, url, data, headerObj);
            if (msg.StatusCode != HttpStatusCode.OK && msg.StatusCode != HttpStatusCode.Created)
                return (msg, await SafeReadAsJsonAsync<T>(msg.Content, default(T)));

            var t = await ReadAsJsonAsync<T>(msg.Content);

            return (msg, t);
        }

        public static async Task<(HttpResponseMessage msg, TOutput result)> PostAndReadAsJsonAsync<TOutput, TInput>(
            this HttpClient httpClient, string url, TInput data, object headerObj = null)
        {
            var msg = await PostAsJsonAsync(httpClient, url, data, headerObj);
            if (msg.StatusCode != HttpStatusCode.OK && msg.StatusCode != HttpStatusCode.Created)
                return (msg, await SafeReadAsJsonAsync<TOutput>(msg.Content, default(TOutput)));

            var t = await ReadAsJsonAsync<TOutput>(msg.Content);

            return (msg, t);
        }

        public static async Task<(HttpResponseMessage msg, TOutput result)> PostAsJsonAndReadAsXmlAsync<TOutput, TInput>(
            this HttpClient httpClient, string url, TInput data, object headerObj = null)
        {
            var msg = await PostAsJsonAsync(httpClient, url, data, headerObj);
            if (msg.StatusCode != HttpStatusCode.OK && msg.StatusCode != HttpStatusCode.Created)
                return (msg, await SafeReadAsXmlAsync<TOutput>(msg.Content, default(TOutput)));

            var t = await ReadAsXmlAsync<TOutput>(msg.Content);

            return (msg, t);
        }

        public static async Task<(HttpResponseMessage msg, TOutput result)> PostAsFormAndReadAsJsonAsync<TOutput, TInput>(
            this HttpClient httpClient, string url, TInput data, object headerObj = null)
        {
            var msg = await PostAsFormAsync(httpClient, url, data, headerObj);
            if (msg.StatusCode != HttpStatusCode.OK && msg.StatusCode != HttpStatusCode.Created)
                return (msg, await SafeReadAsJsonAsync<TOutput>(msg.Content, default(TOutput)));

            var t = await ReadAsJsonAsync<TOutput>(msg.Content);

            return (msg, t);
        }

        public static async Task<HttpResponseMessage> PutAsStringAsync<T>(
            this HttpClient httpClient, string url, T data, object headerObj = null)
            where T : struct
        {
            var dataAsString = data.ToString();
            var content = new StringContent(dataAsString);
            AddToHttpHeaders(content.Headers, headerObj);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            try
            {
                var result = await httpClient.PutAsync(url, content);
                return result;
            }
            finally
            {
                content.Dispose();
#pragma warning disable IDE0059, S1854  // Unnecessary assignment of a value
                dataAsString = null;
#pragma warning restore IDE0059, S1854  // Unnecessary assignment of a value
            }
        }

        public static async Task<(HttpResponseMessage msg, T result)> PutAsStringAndReadAsJsonAsync<T>(
            this HttpClient httpClient, string url, T data, object headerObj = null)
            where T : struct
        {
            var msg = await PutAsStringAsync(httpClient, url, data, headerObj);
            if (msg.StatusCode != HttpStatusCode.OK && msg.StatusCode != HttpStatusCode.Created)
                return (msg, await SafeReadAsJsonAsync<T>(msg.Content, default(T)));

            var t = await ReadAsJsonAsync<T>(msg.Content);

            return (msg, t);
        }

        public static async Task<(HttpResponseMessage msg, TOutput result)> PutAsStringAndReadAsJsonAsync<TOutput, TInput>(
            this HttpClient httpClient, string url, TInput data, object headerObj = null)
            where TInput : struct
        {
            var msg = await PutAsStringAsync(httpClient, url, data, headerObj);
            if (msg.StatusCode != HttpStatusCode.OK && msg.StatusCode != HttpStatusCode.Created)
                return (msg, await SafeReadAsJsonAsync<TOutput>(msg.Content, default(TOutput)));

            var t = await ReadAsJsonAsync<TOutput>(msg.Content);

            return (msg, t);
        }

        public static async Task<HttpResponseMessage> PutAsJsonAsync<T>(
            this HttpClient httpClient, string url, T data, object headerObj = null)
        {
            var dataAsString = JsonUtils.Serialize(data);
            var content = new StringContent(dataAsString);
            AddToHttpHeaders(content.Headers, headerObj);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            try
            {
                var result = await httpClient.PutAsync(url, content);
                return result;
            }
            finally
            {
                content.Dispose();
#pragma warning disable IDE0059, S1854 // Unnecessary assignment of a value
                dataAsString = null;
#pragma warning restore IDE0059, S1854 // Unnecessary assignment of a value
            }
        }

        public static async Task<(HttpResponseMessage msg, T result)> PutAndReadAsJsonAsync<T>(
            this HttpClient httpClient, string url, T data, object headerObj = null)
        {
            var msg = await PutAsJsonAsync(httpClient, url, data, headerObj);
            if (msg.StatusCode != HttpStatusCode.OK && msg.StatusCode != HttpStatusCode.Created)
                return (msg, await SafeReadAsJsonAsync<T>(msg.Content, default(T)));

            var t = await ReadAsJsonAsync<T>(msg.Content);

            return (msg, t);
        }

        public static async Task<(HttpResponseMessage msg, TOutput result)> PutAndReadAsJsonAsync<TOutput, TInput>(
            this HttpClient httpClient, string url, TInput data, object headerObj = null)
        {
            var msg = await PutAsJsonAsync(httpClient, url, data, headerObj);
            if (msg.StatusCode != HttpStatusCode.OK && msg.StatusCode != HttpStatusCode.Created)
                return (msg, await SafeReadAsJsonAsync<TOutput>(msg.Content, default(TOutput)));

            var t = await ReadAsJsonAsync<TOutput>(msg.Content);

            return (msg, t);
        }

        public static async Task<(HttpResponseMessage msg, T result)> GetAndReadAsJsonAsync<T>(
            this HttpClient httpClient, string url, object headerObj = null)
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(url, UriKind.RelativeOrAbsolute),
                Method = HttpMethod.Get,
            };
            AddToHttpHeaders(request.Headers, headerObj);

            var msg = await httpClient.SendAsync(request);
            if (msg.StatusCode != HttpStatusCode.OK && msg.StatusCode != HttpStatusCode.Created)
                return (msg, await SafeReadAsJsonAsync<T>(msg.Content, default(T)));

            var t = await ReadAsJsonAsync<T>(msg.Content);

            return (msg, t);
        }

        public static async Task<(HttpResponseMessage msg, string result)> GetAndReadAsStringAsync(
            this HttpClient httpClient, string url, object headerObj = null)
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(url, UriKind.Relative),
                Method = HttpMethod.Get,
            };
            AddToHttpHeaders(request.Headers, headerObj);

            var msg = await httpClient.SendAsync(request);
            if (msg.StatusCode != HttpStatusCode.OK && msg.StatusCode != HttpStatusCode.Created)
                return (msg, null);

            var t = await msg.Content.ReadAsStringAsync();

            return (msg, t);
        }

        public static async Task<(HttpResponseMessage msg, T result)> GetAndReadAsXmlAsync<T>(
            this HttpClient httpClient, string url, object headerObj = null)
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(url, UriKind.Relative),
                Method = HttpMethod.Get,
            };
            AddToHttpHeaders(request.Headers, headerObj);

            var msg = await httpClient.SendAsync(request);
            if (msg.StatusCode != HttpStatusCode.OK && msg.StatusCode != HttpStatusCode.Created)
                return (msg, await SafeReadAsXmlAsync<T>(msg.Content, default(T)));

            var t = await ReadAsXmlAsync<T>(msg.Content);

            return (msg, t);
        }

        public static async Task<(HttpResponseMessage msg, FileStreamModel result)> GetAndReadAsFormAsync(
            this HttpClient httpClient, string url, object headerObj = null)
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(url, UriKind.Relative),
                Method = HttpMethod.Get,
            };
            AddToHttpHeaders(request.Headers, headerObj);

            var msg = await httpClient.SendAsync(request);
            if (msg.StatusCode != HttpStatusCode.OK && msg.StatusCode != HttpStatusCode.Created)
                return (msg, await SafeReadAsFormAsync(msg.Content, default(FileStreamModel)));

            var t = await ReadAsFormAsync(msg.Content);

            return (msg, t);
        }

        public static async Task<T> SafeReadAsJsonAsync<T>(this HttpContent content, T defaultValue)
        {
            try
            {
                return await ReadAsJsonAsync<T>(content);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
        {
            var dataAsString = await content.ReadAsStringAsync();
            return JsonUtils.Deserialize<T>(dataAsString);
        }

        public static async Task<FileStreamModel> SafeReadAsFormAsync(this HttpContent content, FileStreamModel defaultValue)
        {
            try
            {
                return await ReadAsFormAsync(content);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static async Task<FileStreamModel> ReadAsFormAsync(this HttpContent content)
        {
            return new FileStreamModel()
            {
                FileData = await content.ReadAsStreamAsync(),
                FileName = content.Headers?.ContentDisposition?.FileName,
                ContentType = content.Headers?.ContentType?.MediaType
            };
        }

        public static async Task<T> SafeReadAsXmlAsync<T>(this HttpContent content, T defaultValue)
        {
            try
            {
                return await ReadAsXmlAsync<T>(content);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static async Task<T> ReadAsXmlAsync<T>(this HttpContent content)
        {
            var dataAsString = await content.ReadAsStringAsync();

            using (StringReader reader = new StringReader(dataAsString))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(reader);
            }
        }

        public static void AddToHttpHeaders(HttpHeaders headers, object obj)
        {
            if (headers == null || obj == null)
            {
                return;
            }

            foreach (var p in obj.GetType().GetProperties())
            {
                var alst = p.GetCustomAttributes(typeof(AlternateNameAttribute), true);
                var name = p.Name;
                if (alst != null && alst.Length > 0)
                {
                    name = (alst[0] as AlternateNameAttribute).Name;
                }

                var value = p.GetValue(obj, null)?.ToString();
                if (value != null)
                {
                    if (name.Equals("Authorization", StringComparison.OrdinalIgnoreCase))
                    {
                        if (headers is HttpRequestHeaders requestHeaders)
                        {
                            requestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", value);
                        }
                    }
                    else
                    {
                        headers.Add(name, value);
                    }
                }
            }
        }

        public static string GetQueryString(object obj, string baseUrl = null)
        {
            if (obj == null)
            {
                return TrimFirstSlash((baseUrl != null) ? baseUrl : "");
            }

            var properties = new List<string>();

            foreach (var p in obj.GetType().GetProperties())
            {
                var v = p.GetValue(obj, null);
                if (v == null)
                    continue;

                if (p.PropertyType.IsGenericType && v is IEnumerable)
                {
                    var e = v as IEnumerable;
                    foreach (var eI in e)
                    {
                        properties.Add(p.Name + "=" + WebUtility.UrlEncode(eI.ToString()));
                    }
                }
                else
                {
                    properties.Add(p.Name + "=" + WebUtility.UrlEncode(v.ToString()));
                }
            }

            var qry = string.Join("&", properties.ToArray());

            if (baseUrl == null)
                return TrimFirstSlash(qry);

            if (baseUrl.IndexOf("?") == -1)
                qry = "?" + qry;

            return TrimFirstSlash(baseUrl + qry);
        }

        private static string TrimFirstSlash(string str)
        {
            if (str.IsTrimmedStringNullOrEmpty())
                return str;
            if (str[0] != '/')
                return str;
            str = str.Substring(1);
            return str;
        }
    }
}