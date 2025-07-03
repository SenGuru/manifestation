/**
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see <https://opensource.org/licenses/BSD-3-Clause>.
**/
using System;
using System.Collections.Generic;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Framework.Web.Filters
{
    [AttributeUsage(AttributeTargets.All)]
    public class PerformanceFilterAttribute : ActionFilterAttribute, IExceptionFilter
    {
        private readonly ILog log;
        private DateTime startTime;

        public PerformanceFilterAttribute(ILog log)
        {
            this.log = log;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            startTime = DateTime.UtcNow;
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            HandlePerfLog(context.ActionDescriptor, context.HttpContext, context.Exception);
            base.OnActionExecuted(context);
        }

        public void OnException(ExceptionContext context)
        {
            HandlePerfLog(context.ActionDescriptor, context.HttpContext, context.Exception);
        }

        private void HandlePerfLog(ActionDescriptor descriptor, HttpContext httpContext, Exception ex)
        {
            var endTime = DateTime.UtcNow;
            var parametersForLog = new List<KeyValuePair<string, object>>();
            var controllerName = "";
            string actionName;

            if (!(descriptor is ControllerActionDescriptor controllerActionDescriptor))
            {
                actionName = httpContext.Request.Path;
            }
            else
            {
                controllerName = controllerActionDescriptor.ControllerName;
                actionName = controllerActionDescriptor.ActionName;
                if (actionName.IsTrimmedStringNullOrEmpty())
                {
                    actionName = httpContext.Request.Path;
                }

                foreach (var param in controllerActionDescriptor.Parameters)
                {
                    parametersForLog.Add(new KeyValuePair<string, object>(param.Name, new object()));
                }
            }

            log.Performance(controllerName, actionName, startTime, endTime, parametersForLog, httpContext.Request.Method, httpContext.Response.StatusCode, ex == null ? "Completed" : "Error", ex == null ? string.Empty : "Exception - " + ExceptionUtils.RecursivelyGetExceptionMessage(ex));
        }
    }
}
