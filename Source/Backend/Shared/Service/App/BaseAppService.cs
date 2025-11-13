using System;
using System.Collections.Generic;
using Manifest.Service.ObjectMapping;
using Framework.Infrastructure.BaseClasses;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Result;

namespace Manifest.Service.App
{
    public class BaseAppService : BaseService
    {
        protected readonly ObjectMapperService objectMapperService;

        public BaseAppService(ILog log, ObjectMapperService objectMapperService)
            : base(log)
        {
            this.objectMapperService = objectMapperService;
        }

        public ReturnModel<TResult> ConvertToItem<TSource, TResult>(TSource src, Func<TSource, TResult> fnc, string errorMsg = null)
        {
            var item = fnc(src);
            if (item == null)
            {
                return ReturnModel<TResult>.Error(errorMsg ?? "Unable to get the item", (string)null, 404);
            }

            return ReturnModel<TResult>.Success(item);
        }

        public ReturnListModel<TResult> ConvertToList<TSource, TResult>(DbReturnListModel<TSource> src, Func<List<TSource>,List<TResult>> fnc, string errorMsg = null)
        {
            var lst = fnc(src.Result);
            if (lst == null)
            {
                return ReturnListModel<TResult>.Error(errorMsg ?? "Unable to get the list", (Exception)null, 404);
            }

            return ReturnListModel<TResult>.Success(lst, src.TotalRows);
        }

        public ReturnListModel<TResult> ConvertToList<TSource, TResult>(List<TSource> src, Func<List<TSource>, List<TResult>> fnc, string errorMsg = null)
        {
            var lst = fnc(src);
            if (lst == null)
            {
                return ReturnListModel<TResult>.Error(errorMsg ?? "Unable to get the list", (Exception)null, 404);
            }

            return ReturnListModel<TResult>.Success(lst, src.Count);
        }
    }
}
