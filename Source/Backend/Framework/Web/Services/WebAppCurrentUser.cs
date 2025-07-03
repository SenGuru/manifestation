/**
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see <https://opensource.org/licenses/BSD-3-Clause>.
**/
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Framework.Infrastructure.Config;
using Framework.Infrastructure.Constants;
using Framework.Infrastructure.Interfaces.Helpers;
using Framework.Infrastructure.Interfaces.Services;
using Framework.Infrastructure.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Framework.Web.Helpers
{
    public class WebAppCurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IBaseDbConfiguration baseConfiguration;

        public WebAppCurrentUser(IHttpContextAccessor httpContextAccessor, IBaseDbConfiguration baseConfiguration)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.baseConfiguration = baseConfiguration;
        }

        public long GetNotNullableCurrentUserId()
        {
            return SafeUtils.Long(GetCurrentUserId());
        }

        public long? GetCurrentUserId()
        {
            return null;
        }

        public virtual string GetCurrentUserName()
        {
            return httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? $"App-{baseConfiguration.AppName}";
        }

        public virtual string GetCurrentUserToken()
        {
            return null;
        }

        public virtual string GetCurrentUserAddress()
        {
            return httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString();
        }

        public string GetCurrentUserEmail()
        {
            return string.Empty;
        }

        public virtual string GetCurrentUserPropertyAsString(string propertyName)
        {
            if (propertyName == null)
                return null;

            if (httpContextAccessor?.HttpContext?.User?.Identity?.Name == null)
                return null;

            var key = httpContextAccessor.HttpContext.Request.Query.Keys.FirstOrDefault(x => x.ToUpper().Trim() == propertyName.Trim().ToUpper());
            if (key != null)
                return httpContextAccessor.HttpContext.Request.Query[key];

            //If value does not exists in Querystring, lets check the Header
            key = httpContextAccessor.HttpContext.Request.Headers.Keys.FirstOrDefault(x => x.ToUpper().Trim() == propertyName.Trim().ToUpper());
            if (key != null)
                return httpContextAccessor.HttpContext.Request.Headers[key];

            return null;
        }

        public virtual long GetCurrentUserPropertyAsLong(string propertyName)
        {
            return SafeUtils.Long(GetCurrentUserPropertyAsString(propertyName));
        }

        public virtual T GetCurrentUserProperty<T>(string propertyName)
        {
            var v = GetCurrentUserPropertyAsString(propertyName);

            if (v.IsTrimmedStringNullOrEmpty())
                return default(T);
            return SafeUtils.To<T>(v);
        }

        public virtual string GetCurrentClaimProperty(string propertyName)
        {
            if (propertyName == null)
                return null;

            if (httpContextAccessor?.HttpContext?.User?.Identity?.Name == null)
                return null;

            if (httpContextAccessor.HttpContext?.User?.Claims.IsNullOrEmpty() == false)
            {
                var claim = httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type.ToUpper().Trim() == propertyName.Trim().ToUpper());
                if (claim != null)
                    return claim.Value;
            }

            return null;
        }

        public virtual long GetCurrentClaimPropertyAsLong(string propertyName)
        {
            return SafeUtils.Long(GetCurrentClaimProperty(propertyName));
        }

        public List<KeyValuePair<string, string>> GetAllClaimKeyValuePairs()
        {
            var result = new List<KeyValuePair<string, string>>();

            if (httpContextAccessor?.HttpContext?.User?.Identity?.Name == null)
                return result;

            var claims = httpContextAccessor.HttpContext?.User?.Claims;
            if (claims != null)
            {
                foreach (var claim in claims)
                {
                    if (claim == null)
                        continue;

                    result.Add(new KeyValuePair<string, string>(claim.Type, claim.Value));
                }
            }

            return result;
        }

        public bool GetCurrentClaimPropertyAsBool(string propertyName)
        {
            return SafeUtils.Bool(GetCurrentClaimProperty(propertyName));
        }

        public virtual async Task<string> GetAuthenticationTokenAsync()
        {
            if (httpContextAccessor == null || httpContextAccessor.HttpContext == null)
            {
                return null;
            }
#pragma warning disable S2259 // Null pointers should not be dereferenced
            var token = await httpContextAccessor.HttpContext.GetTokenAsync(HttpHeaderConstants.AccessToken);
#pragma warning restore S2259 // Null pointers should not be dereferenced
            return token;
        }

        public string GetUrlRequestLink()
        {
            return httpContextAccessor.HttpContext.Request.Scheme + "://" + httpContextAccessor.HttpContext.Request.Host.Value;
        }

        public long? GetCurrentCompanyOrTenantId()
        {
            var tenantId = GetCurrentClaimPropertyAsLong(ClaimTypes.System);
            if (tenantId > 0)
            {
                return tenantId;
            }

            return null;
        }

        public bool IsSuperAdmin()
        {
            return GetCurrentClaimPropertyAsBool(ClaimTypes.Authentication);
        }

        public bool IsInSession()
        {
            if (httpContextAccessor?.HttpContext?.User?.Identity == null)
                return false;

            if (httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == false)
                return false;

            return true;
        }

        public string GetCurrentUserAgent()
        {
            return (httpContextAccessor?.HttpContext?.Request?.Headers?.UserAgent ?? "").ToString();
        }
    }
}
