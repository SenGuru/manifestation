using System;
using System.Threading.Tasks;
using Manifest.Common.Enums;
using Manifest.Common.Interfaces.Service.App;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Manifest.Web.Filters
{
    [AttributeUsage(AttributeTargets.All)]
    public class AllowPermissionAttribute : ActionFilterAttribute
    {
        private readonly PredefinedUserPermissionTypes[] roles;

        public AllowPermissionAttribute(params PredefinedUserPermissionTypes[] roles)
        {
            this.roles = roles;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new ObjectResult(ReturnModel<object>.Error("User is not authenticated", null, 401));
                context.HttpContext.Response.StatusCode = 401;
                return;
            }
            else
            {
                if (roles.IsNullOrEmpty())
                {
                    context.Result = new ObjectResult(ReturnModel<object>.Error("User is not authorized.", null, 403));
                    context.HttpContext.Response.StatusCode = 403;
                    return;
                }
                else
                {
                    var svc = context.HttpContext.RequestServices.GetService<ICurrentSessionService>();
                    if (!svc.DoesUserHavePermission(roles))
                    {
                        context.Result = new ObjectResult(ReturnModel<object>.Error("User is not authorized.", null, 403));
                        context.HttpContext.Response.StatusCode = 403;
                        return;
                    }
                }
            }

            await next();
        }
    }
}
