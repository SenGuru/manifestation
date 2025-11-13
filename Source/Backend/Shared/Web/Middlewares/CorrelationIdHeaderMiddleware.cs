using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Framework.Infrastructure.Utils;
using Microsoft.AspNetCore.Http;

namespace Manifest.Web.Middlewares
{
    public class CorrelationIdHeaderMiddleware
    {
        private const string CorrelationIdHeaderName = "X-Correlation-ID";
        private readonly RequestDelegate _next;

        public CorrelationIdHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task InvokeAsync(HttpContext context)
        {
            var header = context.Request.Headers[CorrelationIdHeaderName];
            string sessionId;

            if (header.Count > 0)
            {
                Trace.CorrelationManager.ActivityId = SafeUtils.Guid(header[0], Guid.NewGuid());
            }
            else
            {
                Trace.CorrelationManager.ActivityId = Guid.NewGuid();
            }

            sessionId = Trace.CorrelationManager.ActivityId.ToString();
            context.Items[CorrelationIdHeaderName] = sessionId;

            return _next(context);
        }
    }
}
