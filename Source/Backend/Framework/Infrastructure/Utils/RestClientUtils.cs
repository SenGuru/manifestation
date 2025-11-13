using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using Flurl.Http;
using Framework.Infrastructure.Models;
using Framework.Infrastructure.Models.FileInput;
using Framework.Infrastructure.Models.Result;

namespace Framework.Infrastructure.Utils
{
    public static class RestClientUtils
    {
        public static string Url(string site, string urlFragment = null)
        {
            if (site == null)
                return site;

            if (urlFragment == null || urlFragment == "")
                return site;

            if (!site.EndsWith('/'))
                site = site + "/";

            if (urlFragment.StartsWith('/'))
                urlFragment = urlFragment.Substring(1);

            return site + urlFragment;
        }

        public static async Task<ReturnModel<T>> GetAsync<T>(string url, object queryModel = null, List<KeyValuePair<string, string>> headers = null, string token = null)
        {
            try
            {
                var client = GetClientRequest(url, queryModel, headers, token);
                return await client.GetAsync().ReceiveJson<ReturnModel<T>>();
            }
            catch (FlurlHttpTimeoutException tEx)
            {
                //log error
                return ReturnModel<T>.Error($"Request Timeout when connecting to {url} ", tEx);
            }
            catch (FlurlHttpException hEx)
            {
                //log error

                if (hEx.Call != null && hEx.Call.Response != null)
                {
                    var r = await hEx.Call.Response.GetJsonAsync<ReturnModel<T>>();
                    if (r != null)
                    {
                        return r;
                    }

                    return ReturnModel<T>.Error($"Http Exception when connecting to {url}", hEx, $"status = {hEx.Call.Response.StatusCode}");
                }

                return ReturnModel<T>.Error($"Http Exception when connecting to {url}", hEx);
            }
            catch (Exception ex)
            {
                //log error
                return ReturnModel<T>.Error($"Unexpected error when connecting to {url} ", ex);
            }
        }

        public static ReturnModel<T> Get<T>(string url, object queryModel = null, List<KeyValuePair<string, string>> headers = null, string token = null)
        {
            return GetAsync<T>(url, queryModel, headers, token).Result;
        }

        public static async Task<ReturnListModel<T>> GetListAsync<T>(string url, object queryModel = null, List<KeyValuePair<string, string>> headers = null, string token = null)
        {
            try
            {
                return await GetClientRequest(url, queryModel, headers, token).GetAsync().ReceiveJson<ReturnListModel<T>>();
            }
            catch (FlurlHttpTimeoutException tEx)
            {
                //log error
                return ReturnListModel<T>.Error($"Request Timeout when connecting to {url} ", tEx);
            }
            catch (FlurlHttpException hEx)
            {
                //log error

                if (hEx.Call != null && hEx.Call.Response != null)
                {
                    try
                    {
                        var r = await hEx.Call.Response.GetJsonAsync<ReturnListModel<T>>();
                        if (r != null)
                        {
                            return r;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception in Json Parsing in GetListAsync ${ex.ToString()}");
                        return ReturnListModel<T>.Error($"Http Exception when connecting to {url}", hEx, $"status = {hEx.Call.Response.StatusCode}");
                    }
                }

                return ReturnListModel<T>.Error($"Http Exception when connecting to {url}", hEx);
            }
            catch (Exception ex)
            {
                //log error
                return ReturnListModel<T>.Error($"Unexpected error when connecting to {url} ", ex);
            }
        }

        public static ReturnListModel<T> GetList<T>(string url, object queryModel = null, List<KeyValuePair<string, string>> headers = null, string token = null)
        {
            return GetListAsync<T>(url, queryModel, headers, token).Result;
        }

        public static async Task<ReturnModel<T>> PutAsync<T>(string url, object queryModel = null, List<KeyValuePair<string, string>> headers = null, string token = null)
        {
            try
            {
                return await GetClientRequest(url, queryModel, headers, token).PutJsonAsync("").ReceiveJson<ReturnModel<T>>();
            }
            catch (FlurlHttpTimeoutException tEx)
            {
                //log error
                return ReturnModel<T>.Error($"Request Timeout when connecting to {url} ", tEx);
            }
            catch (FlurlHttpException hEx)
            {
                //log error

                if (hEx.Call != null && hEx.Call.Response != null)
                {
                    var r = await hEx.Call.Response.GetJsonAsync<ReturnModel<T>>();
                    if (r != null)
                    {
                        return r;
                    }

                    return ReturnModel<T>.Error($"Http Exception when connecting to {url}", hEx, $"status = {hEx.Call.Response.StatusCode}");
                }

                return ReturnModel<T>.Error($"Http Exception when connecting to {url}", hEx);
            }
            catch (Exception ex)
            {
                //log error
                return ReturnModel<T>.Error($"Unexpected error when connecting to {url} ", ex);
            }
        }

        public static ReturnModel<T> Put<T>(string url, object queryModel = null, List<KeyValuePair<string, string>> headers = null, string token = null)
        {
            return PutAsync<T>(url, queryModel, headers, token).Result;
        }

        public static async Task<ReturnListModel<T>> PutListAsync<T>(string url, object queryModel = null, List<KeyValuePair<string, string>> headers = null, string token = null)
        {
            try
            {
                return await GetClientRequest(url, queryModel, headers, token).PutJsonAsync("").ReceiveJson<ReturnListModel<T>>();
            }
            catch (FlurlHttpTimeoutException tEx)
            {
                //log error
                return ReturnListModel<T>.Error($"Request Timeout when connecting to {url} ", tEx);
            }
            catch (FlurlHttpException hEx)
            {
                //log error
                if (hEx.Call != null && hEx.Call.Response != null)
                {
                    var r = await hEx.Call.Response.GetJsonAsync<ReturnListModel<T>>();
                    if (r != null)
                    {
                        return r;
                    }

                    return ReturnListModel<T>.Error($"Http Exception when connecting to {url}", hEx, $"status = {hEx.Call.Response.StatusCode}");
                }

                return ReturnListModel<T>.Error($"Http Exception when connecting to {url}", hEx);
            }
            catch (Exception ex)
            {
                //log error
                return ReturnListModel<T>.Error($"Unexpected error when connecting to {url} ", ex);
            }
        }

        public static ReturnListModel<T> PutList<T>(string url, object queryModel = null, List<KeyValuePair<string, string>> headers = null, string token = null)
        {
            return PutListAsync<T>(url, queryModel, headers, token).Result;
        }

        public static async Task<ReturnModel<T>> PutWithModelAsync<T, TModel>(string url, TModel model, object queryModel = null, List<KeyValuePair<string, string>> headers = null, string token = null)
        {
            try
            {
                return await GetClientRequest(url, queryModel, headers, token).PutJsonAsync(model).ReceiveJson<ReturnModel<T>>();
            }
            catch (FlurlHttpTimeoutException tEx)
            {
                //log error
                return ReturnModel<T>.Error($"Request Timeout when connecting to {url} ", tEx);
            }
            catch (FlurlHttpException hEx)
            {
                //log error
                if (hEx.Call != null && hEx.Call.Response != null)
                {
                    var r = await hEx.Call.Response.GetJsonAsync<ReturnModel<T>>();
                    if (r != null)
                    {
                        return r;
                    }

                    return ReturnModel<T>.Error($"Http Exception when connecting to {url}", hEx, $"status = {hEx.Call.Response.StatusCode}");
                }

                return ReturnModel<T>.Error($"Http Exception when connecting to {url}", hEx);
            }
            catch (Exception ex)
            {
                //log error
                return ReturnModel<T>.Error($"Unexpected error when connecting to {url} ", ex);
            }
        }

        public static ReturnModel<T> PutWithModel<T, TModel>(string url, TModel model, object queryModel = null, List<KeyValuePair<string, string>> headers = null, string token = null)
        {
            return PutWithModelAsync<T, TModel>(url, model, queryModel, headers, token).Result;
        }

        public static async Task<ReturnListModel<T>> PutListWithModelAsync<T, TModel>(string url, TModel model, object queryModel = null, List<KeyValuePair<string, string>> headers = null, string token = null)
        {
            try
            {
                return await GetClientRequest(url, queryModel, headers, token).PutJsonAsync(model).ReceiveJson<ReturnListModel<T>>();
            }
            catch (FlurlHttpTimeoutException tEx)
            {
                //log error
                return ReturnListModel<T>.Error($"Request Timeout when connecting to {url} ", tEx);
            }
            catch (FlurlHttpException hEx)
            {
                //log error
                if (hEx.Call != null && hEx.Call.Response != null)
                {
                    var r = await hEx.Call.Response.GetJsonAsync<ReturnListModel<T>>();
                    if (r != null)
                    {
                        return r;
                    }

                    return ReturnListModel<T>.Error($"Http Exception when connecting to {url}", hEx, $"status = {hEx.Call.Response.StatusCode}");
                }

                return ReturnListModel<T>.Error($"Http Exception when connecting to {url}", hEx);
            }
            catch (Exception ex)
            {
                //log error
                return ReturnListModel<T>.Error($"Unexpected error when connecting to {url} ", ex);
            }
        }

        public static ReturnListModel<T> PutListWithModel<T, TModel>(string url, TModel model, object queryModel = null, List<KeyValuePair<string, string>> headers = null, string token = null)
        {
            return PutListWithModelAsync<T, TModel>(url, model, queryModel, headers, token).Result;
        }

        public static async Task<ReturnModel<T>> PostAsync<T>(string url, object queryModel = null, List<KeyValuePair<string, string>> headers = null, string token = null)
        {
            try
            {
                return await GetClientRequest(url, queryModel, headers, token).PostJsonAsync("").ReceiveJson<ReturnModel<T>>();
            }
            catch (FlurlHttpTimeoutException tEx)
            {
                //log error
                return ReturnModel<T>.Error($"Request Timeout when connecting to {url} ", tEx);
            }
            catch (FlurlHttpException hEx)
            {
                //log error
                if (hEx.Call != null && hEx.Call.Response != null)
                {
                    var r = await hEx.Call.Response.GetJsonAsync<ReturnModel<T>>();
                    if (r != null)
                    {
                        return r;
                    }

                    return ReturnModel<T>.Error($"Http Exception when connecting to {url}", hEx, $"status = {hEx.Call.Response.StatusCode}");
                }

                return ReturnModel<T>.Error($"Http Exception when connecting to {url}", hEx);
            }
            catch (Exception ex)
            {
                //log error
                return ReturnModel<T>.Error($"Unexpected error when connecting to {url} ", ex);
            }
        }

        public static ReturnModel<T> Post<T>(string url, object queryModel = null, List<KeyValuePair<string, string>> headers = null, string token = null)
        {
            return PostAsync<T>(url, queryModel, headers, token).Result;
        }

        public static async Task<ReturnListModel<T>> PostListAsync<T>(string url, object queryModel = null, List<KeyValuePair<string, string>> headers = null, string token = null)
        {
            try
            {
                return await GetClientRequest(url, queryModel, headers, token).PostJsonAsync("").ReceiveJson<ReturnListModel<T>>();
            }
            catch (FlurlHttpTimeoutException tEx)
            {
                //log error
                return ReturnListModel<T>.Error($"Request Timeout when connecting to {url} ", tEx);
            }
            catch (FlurlHttpException hEx)
            {
                //log error
                if (hEx.Call != null && hEx.Call.Response != null)
                {
                    var r = await hEx.Call.Response.GetJsonAsync<ReturnListModel<T>>();
                    if (r != null)
                    {
                        return r;
                    }

                    return ReturnListModel<T>.Error($"Http Exception when connecting to {url}", hEx, $"status = {hEx.Call.Response.StatusCode}");
                }

                return ReturnListModel<T>.Error($"Http Exception when connecting to {url}", hEx);
            }
            catch (Exception ex)
            {
                //log error
                return ReturnListModel<T>.Error($"Unexpected error when connecting to {url} ", ex);
            }
        }

        public static ReturnListModel<T> PostList<T>(string url, object queryModel = null, List<KeyValuePair<string, string>> headers = null, string token = null)
        {
            return PostListAsync<T>(url, queryModel, headers, token).Result;
        }

        public static async Task<ReturnModel<T>> PostWithModelAsync<T, TModel>(string url, TModel model, object queryModel = null, List<KeyValuePair<string, string>> headers = null, string token = null)
        {
            try
            {
                return await GetClientRequest(url, queryModel, headers, token).PostJsonAsync(model).ReceiveJson<ReturnModel<T>>();
            }
            catch (FlurlHttpTimeoutException tEx)
            {
                //log error
                return ReturnModel<T>.Error($"Request Timeout when connecting to {url} ", tEx);
            }
            catch (FlurlHttpException hEx)
            {
                //log error
                if (hEx.Call != null && hEx.Call.Response != null)
                {
                    var r = await hEx.Call.Response.GetJsonAsync<ReturnModel<T>>();
                    if (r != null)
                    {
                        return r;
                    }

                    return ReturnModel<T>.Error($"Http Exception when connecting to {url}", hEx, $"status = {hEx.Call.Response.StatusCode}");
                }

                return ReturnModel<T>.Error($"Http Exception when connecting to {url}", hEx);
            }
            catch (Exception ex)
            {
                //log error
                return ReturnModel<T>.Error($"Unexpected error when connecting to {url} ", ex);
            }
        }

        public static ReturnModel<T> PostWithModel<T, TModel>(string url, TModel model, object queryModel = null, List<KeyValuePair<string, string>> headers = null, string token = null)
        {
            return PostWithModelAsync<T, TModel>(url, model, queryModel, headers, token).Result;
        }

        public static ReturnModel<T> PostWithFormModel<T, TModel>(string url, TModel formModel, object queryModel = null, List<KeyValuePair<string, string>> headers = null, string token = null)
        {
            try
            {
                return GetClientRequest(url, queryModel, headers, token).PostMultipartAsync(x =>
                {
                    foreach (var prop in formModel.GetType().GetProperties())
                    {
                        var value = prop.GetValue(formModel);
                        var lowerCasePropName = prop.Name.ToLower().Substring(0, 1) + prop.Name.Substring(1);
                        if (value is FileStreamModel)
                        {
                            var file = value as FileStreamModel;
                            x.AddFile(lowerCasePropName, file.FileData, file.FileName);
                        }
                        else if (value != null)
                        {
                            x.AddJson(lowerCasePropName, value);
                        }
                    }
                }).ReceiveJson<ReturnModel<T>>().Result;
            }
            catch (FlurlHttpTimeoutException tEx)
            {
                //log error
                return ReturnModel<T>.Error($"Request Timeout when connecting to {url} ", tEx);
            }
            catch (FlurlHttpException hEx)
            {
                //log error

                var r = hEx.GetResponseJsonAsync<ReturnModel<T>>().Result;
                if (r != null)
                {
                    return r;
                }

                return ReturnModel<T>.Error($"Http Exception when connecting to {url}", hEx, $"status = {hEx.Call.Response.StatusCode}");
            }
            catch (Exception ex)
            {
                //log error
                return ReturnModel<T>.Error($"Unexpected error when connecting to {url} ", ex);
            }
        }

        public static async Task<ReturnListModel<T>> PostListWithModelAsync<T, TModel>(string url, TModel model, object queryModel = null, List<KeyValuePair<string, string>> headers = null, string token = null)
        {
            try
            {
                return await GetClientRequest(url, queryModel, headers, token).PostJsonAsync(model).ReceiveJson<ReturnListModel<T>>();
            }
            catch (FlurlHttpTimeoutException tEx)
            {
                //log error
                return ReturnListModel<T>.Error($"Request Timeout when connecting to {url} ", tEx);
            }
            catch (FlurlHttpException hEx)
            {
                //log error
                if (hEx.Call != null && hEx.Call.Response != null)
                {
                    var r = await hEx.Call.Response.GetJsonAsync<ReturnListModel<T>>();
                    if (r != null)
                    {
                        return r;
                    }

                    return ReturnListModel<T>.Error($"Http Exception when connecting to {url}", hEx, $"status = {hEx.Call.Response.StatusCode}");
                }

                return ReturnListModel<T>.Error($"Http Exception when connecting to {url}", hEx);
            }
            catch (Exception ex)
            {
                //log error
                return ReturnListModel<T>.Error($"Unexpected error when connecting to {url} ", ex);
            }
        }

        public static ReturnListModel<T> PostListWithModel<T, TModel>(string url, TModel model, object queryModel = null, List<KeyValuePair<string, string>> headers = null, string token = null)
        {
            return PostListWithModelAsync<T, TModel>(url, model, queryModel, headers, token).Result;
        }

        public static async Task<ReturnModel<T>> DeleteAsync<T>(string url, object queryModel = null, List<KeyValuePair<string, string>> headers = null, string token = null)
        {
            try
            {
                return await GetClientRequest(url, queryModel, headers, token).DeleteAsync().ReceiveJson<ReturnModel<T>>();
            }
            catch (FlurlHttpTimeoutException tEx)
            {
                //log error
                return ReturnModel<T>.Error($"Request Timeout when connecting to {url} ", tEx);
            }
            catch (FlurlHttpException hEx)
            {
                //log error
                if (hEx.Call != null && hEx.Call.Response != null)
                {
                    var r = await hEx.Call.Response.GetJsonAsync<ReturnModel<T>>();
                    if (r != null)
                    {
                        return r;
                    }

                    return ReturnModel<T>.Error($"Http Exception when connecting to {url}", hEx, $"status = {hEx.Call.Response.StatusCode}");
                }

                return ReturnModel<T>.Error($"Http Exception when connecting to {url}", hEx);
            }
            catch (Exception ex)
            {
                //log error
                return ReturnModel<T>.Error($"Unexpected error when connecting to {url} ", ex);
            }
        }

        public static ReturnModel<T> Delete<T>(string url, object queryModel = null, List<KeyValuePair<string, string>> headers = null, string token = null)
        {
            return DeleteAsync<T>(url, queryModel, headers, token).Result;
        }

        public static async Task<ReturnListModel<T>> DeleteListAsync<T>(string url, object queryModel = null, List<KeyValuePair<string, string>> headers = null, string token = null)
        {
            try
            {
                return await GetClientRequest(url, queryModel, headers, token).DeleteAsync().ReceiveJson<ReturnListModel<T>>();
            }
            catch (FlurlHttpTimeoutException tEx)
            {
                //log error
                return ReturnListModel<T>.Error($"Request Timeout when connecting to {url} ", tEx);
            }
            catch (FlurlHttpException hEx)
            {
                //log error
                if (hEx.Call != null && hEx.Call.Response != null)
                {
                    var r = await hEx.Call.Response.GetJsonAsync<ReturnListModel<T>>();
                    if (r != null)
                    {
                        return r;
                    }

                    return ReturnListModel<T>.Error($"Http Exception when connecting to {url}", hEx, $"status = {hEx.Call.Response.StatusCode}");
                }

                return ReturnListModel<T>.Error($"Http Exception when connecting to {url}", hEx);
            }
            catch (Exception ex)
            {
                //log error
                return ReturnListModel<T>.Error($"Unexpected error when connecting to {url} ", ex);
            }
        }

        public static ReturnListModel<T> DeleteList<T>(string url, object queryModel = null, List<KeyValuePair<string, string>> headers = null, string token = null)
        {
            return DeleteListAsync<T>(url, queryModel, headers, token).Result;
        }

        private static IFlurlRequest GetClientRequest(string url, object queryModel = null, List<KeyValuePair<string, string>> headers = null, string token = null)
        {
            if (url == null)
                throw new Exception("Url is not valid");

            var result = url.WithOAuthBearerToken(token);

            if (queryModel != null)
            {
                foreach (var p in queryModel.GetType().GetProperties())
                {
                    result.SetQueryParam(p.Name, p.GetValue(queryModel));
                }
            }

            if (headers != null && headers.Count > 0)
            {
                var eo = new ExpandoObject();
                var eoColl = (ICollection<KeyValuePair<string, object>>)eo;

                foreach (var kvp in headers)
                {
                    eoColl.Add(new KeyValuePair<string, object>(kvp.Key, kvp.Value));
                }

                dynamic eoDynamic = eo;

                result.WithHeaders((object)eoDynamic);
            }

            return result;
        }
    }
}
