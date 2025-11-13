/**
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see <https://opensource.org/licenses/BSD-3-Clause>.
**/
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Utils
{
    public static class TranslationUtils
    {
        private static string googlePattern1 = @"(?s)class=""(?:t0|result-container)"">(.*?)<";
        private static string googlePattern2 = @"<div dir=""ltr"" class=""t0"">([\s\S]*?)<\/div>";

        public static async Task<(string result, Exception exception)> TranslateUsingGoogleTranslationWebAsync(string targetLanguage, string sourceLanguage, string text, int timeout)
        {
            var escapedText = Uri.EscapeDataString(text);

            string url = $"https://translate.google.com/m?tl={targetLanguage}&sl={sourceLanguage}&q={escapedText}";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue(Encoding.UTF8.WebName));
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 Edge/16.16299");

                try
                {
                    client.Timeout = TimeSpan.FromSeconds(timeout);
                    var response = await client.GetAsync(url);
                    if (!response.IsSuccessStatusCode)
                    {
                        return (null, new Exception("Unable to get Connect to Google at this time."));
                    }

                    var result = await response.Content.ReadAsStringAsync();

                    MatchCollection matches = Regex.Matches(result, googlePattern1);
                    if (matches.Count == 0)
                    {
                        matches = Regex.Matches(result, googlePattern2);
                        if (matches.Count == 0)
                        {
                            return (null, new Exception("Unable to get Translation Text from Google at this time."));
                        }
                    }

                    return (WebUtility.HtmlDecode(matches[0].Groups[1].Value), null);
                }
                catch (Exception e)
                {
                    return (null, e);
                }
            }
        }
    }
}