using System;
using System.Linq;
using Framework.Infrastructure.Exceptions;
using Framework.Infrastructure.Models.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Manifest.Web.Filters
{
    [AttributeUsage(AttributeTargets.All)]
    public class ModelStateValidationActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                if (((ControllerActionDescriptor)context.ActionDescriptor).MethodInfo.ReturnType == typeof(IReturnModel))
                {
                    var lstModelErrors = (from modelStateValues in context.ModelState.Values
                                          from error in modelStateValues.Errors
                                          from modelStateKey in context.ModelState.Keys
                                          select new ReturnErrorItem(modelStateKey, error.ErrorMessage)).ToList();

                    context.Result = new JsonResult(ReturnModel<object>.Error("Validation error occurred", lstModelErrors, (string)null, 400));
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
