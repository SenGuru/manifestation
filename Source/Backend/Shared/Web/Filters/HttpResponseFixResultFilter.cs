using System.Net;
using Framework.Infrastructure.Models.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Manifest.Web.Filters
{
    public class HttpResponseFixResultFilter : IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {
            //do nothing here
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result == null)
            {
                return;
            }

            if (!(context.Result is ObjectResult result))
            {
                return;
            }

            if (result.Value == null)
            {
                return;
            }

            if (!(result.Value is IReturnModel model))
            {
                return;
            }

            context.HttpContext.Response.StatusCode = model.HttpCode == 0 ? (int)HttpStatusCode.OK : model.HttpCode;
        }
    }
}
