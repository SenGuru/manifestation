using System;
using System.Threading.Tasks;
using Framework.Infrastructure.Logging;
using Microsoft.AspNetCore.Http;

namespace Manifest.Web.Middlewares
{
    public class PerformanceLogMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILog log;

        public PerformanceLogMiddleware(RequestDelegate next, ILog log)
        {
            this.next = next;
            this.log = log;
        }

        public Task InvokeAsync(HttpContext context)
        {
            //var watch = new Stopwatch();
            //watch.Start();
            var startTime = DateTime.UtcNow;
            context.Response.OnStarting(() =>
            {
                //watch.Stop();
                string controllerName = context.Request?.RouteValues["controller"]?.ToString();
                string actionName = context.Request?.RouteValues["action"]?.ToString();
                string httpVerb = context.Request?.Method;

                if ((controllerName == null || controllerName == "") && (actionName == null && actionName == ""))
                {
                    actionName = context.Request?.Path.ToString();
                }

                //var responseTimeForCompleteRequest = watch.ElapsedMilliseconds;
#pragma warning disable S3236 // Caller information arguments should not be provided explicitly
                log.Performance(controllerName, actionName, startTime, DateTime.UtcNow, null, httpVerb, context.Response.StatusCode, "", "", 0, "", "");
#pragma warning restore S3236 // Caller information arguments should not be provided explicitly
                return Task.CompletedTask;
            });
            return this.next(context);
        }
    }
}
