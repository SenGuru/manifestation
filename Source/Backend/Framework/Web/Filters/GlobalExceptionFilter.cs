/**
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see <https://opensource.org/licenses/BSD-3-Clause>.
**/
using System.Threading.Tasks;
using Framework.Infrastructure.Constants;
using Framework.Infrastructure.Exceptions;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Framework.Web.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILog log;

        public GlobalExceptionFilter(ILog log)
        {
            this.log = log;
        }

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;

            log.Error(exception);

            if (!(context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor))
                return;

            if (controllerActionDescriptor.MethodInfo.ReturnType == null)
                return;

            var typeToCheck = controllerActionDescriptor.MethodInfo.ReturnType;
            if (!typeof(IReturnModel).IsAssignableFrom(typeToCheck))
            {
                if (typeToCheck.IsGenericType && typeToCheck.GetGenericTypeDefinition() == typeof(Task<>))
                {
                    var actualType = typeToCheck.GetGenericArguments()[0];
                    if (!typeof(IReturnModel).IsAssignableFrom(actualType))
                        return;
                }
            }

            context.HttpContext.Response.StatusCode = HttpCodeContants.Success;

            if (exception is RollbackException)
            {
                var re = exception as RollbackException;
                if (re.Error != null)
                    context.Result = new JsonResult(new ReturnModel<object>(re.Error));
                else
                    context.Result = new JsonResult(new ReturnModel<object>(re));
            }
            else
            {
                context.Result = new JsonResult(new ReturnModel<object>(exception));
            }
        }
    }
}
