using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Manifest.Web.Utils
{
    public static class HttpRequestUtils
    {
        public static string GetBearerToken(this HttpRequest httpRequest)
        {
            string authorizationHeader = httpRequest.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                return string.Empty;
            }

            return authorizationHeader.Substring("Bearer ".Length).Trim();
        }
    }
}