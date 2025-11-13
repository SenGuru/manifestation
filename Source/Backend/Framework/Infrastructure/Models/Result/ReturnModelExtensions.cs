/**
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see <https://opensource.org/licenses/BSD-3-Clause>.
**/
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Framework.Infrastructure.Constants;
using Framework.Infrastructure.Exceptions;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Utils;

namespace Framework.Infrastructure.Models.Result
{
    public static class ReturnModelExtensions
    {
        public static ReturnModel<T> ErrorAndReturnModel<T>(this ILog log, IReturnModel model, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            log.Error(model?.ErrorHolder, sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            return ReturnModel<T>.Error(model);
        }

        public static ReturnModel<T> ErrorAndReturnModel<T>(this ILog log, IReturnModel model, object logObj, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (model?.ErrorHolder != null)
            {
                model.ErrorHolder.AdditionalLogData = logObj;
            }

            log.Error(model?.ErrorHolder, sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);

            return ReturnModel<T>.Error(model);
        }

        public static ReturnModel<T> ErrorAndReturnModel<T>(this ILog log, string errorMsg, int httpCode, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            log.Error(errorMsg, sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            return ReturnModel<T>.Error(errorMsg, httpCode: httpCode);
        }

        public static ReturnModel<T> ErrorAndReturnModel<T>(this ILog log, string errorMsg, IReturnModel model, int httpCode, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            log.Error(errorMsg + " model error : " + model?.ErrorHolder?.AllErrorAsString(), sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            return ReturnModel<T>.Error(errorMsg, httpCode: httpCode);
        }

        public static ReturnModel<T> ErrorAndReturnModel<T>(this ILog log, string errorMsg, Exception ex = null, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            log.Error(ex, errorMsg, sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            if (ex != null)
            {
                if (ex.GetType() == typeof(RollbackException))
                    throw ex;
            }

            return ReturnModel<T>.Error(errorMsg, ex, httpCode: httpCode);
        }

        public static ReturnModel<T> ErrorAndReturnModel<T>(this ILog log, string errorMsg, ReturnErrorItem errorItem, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            var lst = new List<ReturnErrorItem> { errorItem };
            log.Error(new ReturnError { ErrorItemList = lst }, errorMsg, sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            return ReturnModel<T>.Error(errorMsg, lst, httpCode: httpCode);
        }

        public static ReturnModel<T> ErrorAndReturnModel<T>(this ILog log, string errorMsg, List<ReturnErrorItem> errorList, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            log.Error(new ReturnError { ErrorItemList = errorList }, errorMsg, sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            return ReturnModel<T>.Error(errorMsg, errorList, httpCode: httpCode);
        }

        public static ReturnModel<T> ErrorAndReturnModel<T>(this ILog log, List<ReturnErrorItem> errorList, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (errorList == null || errorList.Count == 0)
            {
                return ErrorAndReturnModel<T>(log, "Unknown error", httpCode, sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            }

            return ErrorAndReturnModel<T>(log, errorList[0].Value, errorList, httpCode: httpCode, sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
        }

        public static ReturnModel<T> ErrorAndReturnModel<T>(this ILog log, string errorMsg, string logMsg, Exception ex = null, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            log.Error(ex, $"{errorMsg} {logMsg}", sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            if (ex != null)
            {
                if (ex.GetType() == typeof(RollbackException))
                    throw ex;
            }

            return ReturnModel<T>.Error(errorMsg, ex, httpCode: httpCode);
        }

        public static ReturnModel<T> ErrorAndReturnModel<T>(this ILog log, string errorMsg, object logObj, Exception ex = null, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            log.Error(ex, $"{errorMsg} {JsonUtils.Serialize(logObj)}", sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            if (ex != null)
            {
                if (ex.GetType() == typeof(RollbackException))
                    throw ex;
            }

            return ReturnModel<T>.Error(errorMsg, ex, httpCode: httpCode);
        }

        public static ReturnModel<T> ErrorAndReturnModel<T>(this ILog log, ReturnError returnError, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            log.Error(returnError, sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            return ReturnModel<T>.Error(returnError, httpCode: httpCode);
        }

        public static ReturnModel<T> ErrorAndReturnModel<T>(this ILog log, string errorMsg, string logMsg, List<ReturnErrorItem> errorList, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            log.Error(new ReturnError { ErrorItemList = errorList }, $"{errorMsg} {logMsg}", sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            return ReturnModel<T>.Error(errorMsg, errorList, httpCode: httpCode);
        }

        public static ReturnModel<T> ErrorAndReturnModel<T>(this ILog log, string errorMsg, object logObj, List<ReturnErrorItem> errorList, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            log.Error($"{errorMsg} {JsonUtils.Serialize(logObj)}", sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            return ReturnModel<T>.Error(errorMsg, errorList, httpCode: httpCode);
        }

        public static ReturnModel<T> ErrorAndReturnModelWithRollbackExceptionHandler<T>(this ILog log, string errorMsg, Exception ex = null, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            return ErrorAndReturnModel<T>(log, errorMsg, ex, httpCode, sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
        }

        public static ReturnModel<T> ErrorAndReturnModelWithRollbackExceptionHandler<T>(this ILog log, string errorMsg, object logObj, Exception ex = null, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            return ErrorAndReturnModel<T>(log, errorMsg, logObj, ex, httpCode, sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
        }

        public static ReturnModel<T> ErrorAndReturnModelWithRollbackExceptionHandler<T>(this ILog log, string errorMsg, object logObj, List<ReturnErrorItem> errorList, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            return ErrorAndReturnModel<T>(log, errorMsg, logObj, errorList, httpCode, sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
        }

        public static ReturnModel<T> ErrorAndReturnModelWithRollbackExceptionHandler<T>(this ILog log, ReturnError returnError, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (returnError?.ErrorItemList != null)
            {
                return ErrorAndReturnModel<T>(log, returnError.ErrorItemList, httpCode, sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            }

            return ErrorAndReturnModel<T>(log, returnError, httpCode, sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
        }

        public static ReturnModel<T> WarnAndReturnModel<T>(this ILog log, string warnMsg, int httpCode, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            log.Warn(warnMsg, sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            return new ReturnModel<T>(warnMsg, httpCode: httpCode);
        }

        public static ReturnModel<T> WarnAndReturnModel<T>(this ILog log, string warnMsg, Exception ex = null, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            log.Warn(ex, $"{warnMsg}", sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            if (ex != null)
            {
                if (ex.GetType() == typeof(RollbackException))
                    throw ex;
            }

            return new ReturnModel<T>(warnMsg, ex, httpCode: httpCode);
        }

        public static ReturnModel<T> WarnAndReturnModel<T>(this ILog log, string warnMsg, List<ReturnErrorItem> errorList, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            log.Warn(new ReturnError { ErrorItemList = errorList }, warnMsg, sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            return new ReturnModel<T>(warnMsg, errorList, httpCode: httpCode);
        }

        public static ReturnModel<T> WarnAndReturnModel<T>(this ILog log, string warnMsg, string logMsg, Exception ex = null, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            log.Warn(ex, $"{warnMsg} {logMsg}", sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            if (ex != null)
            {
                if (ex.GetType() == typeof(RollbackException))
                    throw ex;
            }

            return new ReturnModel<T>(warnMsg, ex, httpCode: httpCode);
        }

        public static ReturnModel<T> WarnAndReturnModel<T>(this ILog log, string warnMsg, object logObj, Exception ex = null, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            log.Warn(ex, $"{warnMsg} {JsonUtils.Serialize(logObj)}", sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            if (ex != null)
            {
                if (ex.GetType() == typeof(RollbackException))
                    throw ex;
            }

            return new ReturnModel<T>(warnMsg, ex, httpCode: httpCode);
        }

        public static ReturnModel<T> WarnAndReturnModel<T>(this ILog log, string warnMsg, string logMsg, List<ReturnErrorItem> errorList, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            log.Warn(new ReturnError { ErrorItemList = errorList }, $"{warnMsg} {logMsg}", sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            return new ReturnModel<T>(warnMsg, errorList, httpCode: httpCode);
        }

        public static ReturnModel<T> WarnAndReturnModel<T>(this ILog log, string warnMsg, object logObj, List<ReturnErrorItem> errorList, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            log.Warn(new ReturnError { ErrorItemList = errorList }, $"{warnMsg} {JsonUtils.Serialize(logObj)}", sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            return new ReturnModel<T>(warnMsg, errorList, httpCode: httpCode);
        }

        public static ReturnListModel<T> ErrorAndReturnListModel<T>(this ILog log, IReturnModel model, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            log.Error(model?.ErrorHolder, sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            return ReturnListModel<T>.Error(model);
        }

        public static ReturnListModel<T> ErrorAndReturnListModel<T>(this ILog log, string errorMsg, int httpCode, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            log.Error(errorMsg, sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            return ReturnListModel<T>.Error(errorMsg, httpCode: httpCode);
        }

        public static ReturnListModel<T> ErrorAndReturnListModel<T>(this ILog log, string errorMsg, Exception ex = null, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            log.Error(ex, errorMsg, sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            if (ex != null)
            {
                if (ex.GetType() == typeof(RollbackException))
                    throw ex;
            }

            return ReturnListModel<T>.Error(errorMsg, ex, httpCode: httpCode);
        }

        public static ReturnListModel<T> ErrorAndReturnListModel<T>(this ILog log, string errorMsg, List<ReturnErrorItem> errorList, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            log.Error(new ReturnError { ErrorItemList = errorList }, errorMsg, sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            return ReturnListModel<T>.Error(errorMsg, errorList, httpCode: httpCode);
        }

        public static ReturnListModel<T> ErrorAndReturnListModel<T>(this ILog log, string errorMsg, ReturnErrorItem errorItem, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            var lst = new List<ReturnErrorItem> { errorItem };
            log.Error(new ReturnError { ErrorItemList = lst }, errorMsg, sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            return ReturnListModel<T>.Error(errorMsg, lst, httpCode: httpCode);
        }

        public static ReturnListModel<T> ErrorAndReturnListModel<T>(this ILog log, string errorMsg, string logMsg, Exception ex = null, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            log.Error(ex, $"{errorMsg} {logMsg}", sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            if (ex != null)
            {
                if (ex.GetType() == typeof(RollbackException))
                    throw ex;
            }

            return ReturnListModel<T>.Error(errorMsg, ex, httpCode: httpCode);
        }

        public static ReturnListModel<T> ErrorAndReturnListModel<T>(this ILog log, string errorMsg, object logObj, Exception ex = null, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            log.Error(ex, $"{errorMsg} {JsonUtils.Serialize(logObj)}", sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            if (ex != null)
            {
                if (ex.GetType() == typeof(RollbackException))
                    throw ex;
            }

            return ReturnListModel<T>.Error(errorMsg, ex, httpCode: httpCode);
        }

        public static ReturnListModel<T> ErrorAndReturnListModel<T>(this ILog log, string errorMsg, string logMsg, List<ReturnErrorItem> errorList, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            log.Error(new ReturnError { ErrorItemList = errorList }, $"{errorMsg} {logMsg}", sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            return ReturnListModel<T>.Error(errorMsg, errorList, httpCode: httpCode);
        }

        public static ReturnListModel<T> ErrorAndReturnListModel<T>(this ILog log, string errorMsg, object logObj, List<ReturnErrorItem> errorList, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            log.Error(new ReturnError { ErrorItemList = errorList }, $"{errorMsg} {JsonUtils.Serialize(logObj)}", sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            return ReturnListModel<T>.Error(errorMsg, errorList, httpCode: httpCode);
        }

        public static ReturnListModel<T> ErrorAndReturnListModelWithRollbackExceptionHandler<T>(this ILog log, string errorMsg, Exception ex = null, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            return ErrorAndReturnListModel<T>(log, errorMsg, ex, httpCode, sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
        }

        public static ReturnListModel<T> ErrorAndReturnListModelWithRollbackExceptionHandler<T>(this ILog log, string errorMsg, object logObj, Exception ex = null, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            return ErrorAndReturnListModel<T>(log, errorMsg, logObj, ex, httpCode, sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
        }

        public static ReturnListModel<T> WarnAndReturnListModel<T>(this ILog log, string warnMsg, int httpCode, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            log.Warn(warnMsg, sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            return new ReturnListModel<T>(warnMsg, httpCode: httpCode);
        }

        public static ReturnListModel<T> WarnAndReturnListModel<T>(this ILog log, string warnMsg, Exception ex = null, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            log.Warn(ex, warnMsg, sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            if (ex != null)
            {
                if (ex.GetType() == typeof(RollbackException))
                    throw ex;
            }

            return new ReturnListModel<T>(warnMsg, ex, httpCode: httpCode);
        }

        public static ReturnListModel<T> WarnAndReturnListModel<T>(this ILog log, string warnMsg, List<ReturnErrorItem> errorList, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            log.Warn(new ReturnError { ErrorItemList = errorList }, warnMsg, sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            return new ReturnListModel<T>(warnMsg, errorList, httpCode: httpCode);
        }

        public static ReturnListModel<T> WarnAndReturnListModel<T>(this ILog log, string warnMsg, string logMsg, Exception ex = null, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            log.Warn(ex, $"{warnMsg} {logMsg}", sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            if (ex != null)
            {
                if (ex.GetType() == typeof(RollbackException))
                    throw ex;
            }

            return new ReturnListModel<T>(warnMsg, ex, httpCode: httpCode);
        }

        public static ReturnListModel<T> WarnAndReturnListModel<T>(this ILog log, string warnMsg, object logObj, Exception ex = null, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            log.Warn(ex, $"{warnMsg} {JsonUtils.Serialize(logObj)}", sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            if (ex != null)
            {
                if (ex.GetType() == typeof(RollbackException))
                    throw ex;
            }

            return new ReturnListModel<T>(warnMsg, ex, httpCode: httpCode);
        }

        public static ReturnListModel<T> WarnAndReturnListModel<T>(this ILog log, string warnMsg, string logMsg, List<ReturnErrorItem> errorList, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            log.Warn(new ReturnError { ErrorItemList = errorList }, $"{warnMsg} {logMsg}", sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            return new ReturnListModel<T>(warnMsg, errorList, httpCode: httpCode);
        }

        public static ReturnListModel<T> WarnAndReturnListModel<T>(this ILog log, string warnMsg, object logObj, List<ReturnErrorItem> errorList, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            log.Warn(new ReturnError { ErrorItemList = errorList }, $"{warnMsg} {JsonUtils.Serialize(logObj)}", sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            return new ReturnListModel<T>(warnMsg, errorList, httpCode: httpCode);
        }

        public static ReturnListWithSearchModel<TModel, TSearch> ErrorAndReturnListWithSearchModel<TModel, TSearch>(this ILog log, string errorMsg, int httpCode, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
            where TSearch : class
        {
            log.Error(errorMsg, sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            return ReturnListWithSearchModel<TModel, TSearch>.Error(errorMsg, httpCode: httpCode);
        }

        public static ReturnListWithSearchModel<TModel, TSearch> ErrorAndReturnListWithSearchModel<TModel, TSearch>(this ILog log, string errorMsg, Exception ex = null, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
            where TSearch : class
        {
            log.Error(ex, errorMsg, sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            if (ex != null)
            {
                if (ex.GetType() == typeof(RollbackException))
                    throw ex;
            }

            return ReturnListWithSearchModel<TModel, TSearch>.Error(errorMsg, ex, httpCode: httpCode);
        }

        public static ReturnListWithSearchModel<TModel, TSearch> ErrorAndReturnListWithSearchModel<TModel, TSearch>(this ILog log, string errorMsg, List<ReturnErrorItem> errorList, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
            where TSearch : class
        {
            log.Error(errorMsg, sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            return ReturnListWithSearchModel<TModel, TSearch>.Error(errorMsg, errorList, httpCode: httpCode);
        }

        public static ReturnListWithSearchModel<TModel, TSearch> ErrorAndReturnListWithSearchModel<TModel, TSearch>(this ILog log, string errorMsg, string logMsg, Exception ex = null, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
            where TSearch : class
        {
            log.Error(ex, $"{errorMsg} {logMsg}", sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            if (ex != null)
            {
                if (ex.GetType() == typeof(RollbackException))
                    throw ex;
            }

            return ReturnListWithSearchModel<TModel, TSearch>.Error(errorMsg, ex, httpCode: httpCode);
        }

        public static ReturnListWithSearchModel<TModel, TSearch> ErrorAndReturnListWithSearchModel<TModel, TSearch>(this ILog log, string errorMsg, object logObj, Exception ex = null, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
            where TSearch : class
        {
            log.Error(ex, $"{errorMsg} {JsonUtils.Serialize(logObj)}", sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            if (ex != null)
            {
                if (ex.GetType() == typeof(RollbackException))
                    throw ex;
            }

            return ReturnListWithSearchModel<TModel, TSearch>.Error(errorMsg, ex, httpCode: httpCode);
        }

        public static ReturnListWithSearchModel<TModel, TSearch> ErrorAndReturnListWithSearchModel<TModel, TSearch>(this ILog log, string errorMsg, string logMsg, List<ReturnErrorItem> errorList, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
            where TSearch : class
        {
            log.Error(new ReturnError { ErrorItemList = errorList }, $"{errorMsg} {logMsg}", sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            return ReturnListWithSearchModel<TModel, TSearch>.Error(errorMsg, errorList, httpCode: httpCode);
        }

        public static ReturnListWithSearchModel<TModel, TSearch> WarnAndReturnListWithSearchModel<TModel, TSearch>(this ILog log, string warnMsg, int httpCode, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
            where TSearch : class
        {
            log.Warn(warnMsg, sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            return new ReturnListWithSearchModel<TModel, TSearch>(warnMsg, httpCode: httpCode);
        }

        public static ReturnListWithSearchModel<TModel, TSearch> WarnAndReturnListWithSearchModel<TModel, TSearch>(this ILog log, string warnMsg, Exception ex = null, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
            where TSearch : class
        {
            log.Warn(ex, warnMsg, sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            if (ex != null)
            {
                if (ex.GetType() == typeof(RollbackException))
                    throw ex;
            }

            return new ReturnListWithSearchModel<TModel, TSearch>(warnMsg, ex, httpCode: httpCode);
        }

        public static ReturnListWithSearchModel<TModel, TSearch> WarnAndReturnListWithSearchModel<TModel, TSearch>(this ILog log, string warnMsg, List<ReturnErrorItem> errorList, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
            where TSearch : class
        {
            log.Warn(new ReturnError { ErrorItemList = errorList }, warnMsg, sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            return new ReturnListWithSearchModel<TModel, TSearch>(warnMsg, errorList, httpCode: httpCode);
        }

        public static ReturnListWithSearchModel<TModel, TSearch> WarnAndReturnListWithSearchModel<TModel, TSearch>(this ILog log, string warnMsg, string logMsg, Exception ex = null, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
            where TSearch : class
        {
            log.Warn(ex, $"{warnMsg} {logMsg}", sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            if (ex != null)
            {
                if (ex.GetType() == typeof(RollbackException))
                    throw ex;
            }

            return new ReturnListWithSearchModel<TModel, TSearch>(warnMsg, ex, httpCode: httpCode);
        }

        public static ReturnListWithSearchModel<TModel, TSearch> WarnAndReturnListWithSearchModel<TModel, TSearch>(this ILog log, string warnMsg, object logObj, Exception ex = null, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
            where TSearch : class
        {
            log.Warn(ex, $"{warnMsg} {JsonUtils.Serialize(logObj)}", sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            if (ex != null)
            {
                if (ex.GetType() == typeof(RollbackException))
                    throw ex;
            }

            return new ReturnListWithSearchModel<TModel, TSearch>(warnMsg, ex, httpCode: httpCode);
        }

        public static ReturnListWithSearchModel<TModel, TSearch> WarnAndReturnListWithSearchModel<TModel, TSearch>(this ILog log, string warnMsg, string logMsg, List<ReturnErrorItem> errorList, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
            where TSearch : class
        {
            log.Warn(new ReturnError { ErrorItemList = errorList }, $"{warnMsg} {logMsg}", sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            return new ReturnListWithSearchModel<TModel, TSearch>(warnMsg, errorList, httpCode: httpCode);
        }

        public static ReturnListWithSearchModel<TModel, TSearch> WarnAndReturnListWithSearchModel<TModel, TSearch>(this ILog log, string warnMsg, object logObj, List<ReturnErrorItem> errorList, int httpCode = HttpCodeContants.Success, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
            where TSearch : class
        {
            log.Warn(new ReturnError { ErrorItemList = errorList }, $"{warnMsg} {JsonUtils.Serialize(logObj)}", sourceLineNumber: sourceLineNumber, memberName: memberName, sourceFilePath: sourceFilePath);
            return new ReturnListWithSearchModel<TModel, TSearch>(warnMsg, errorList, httpCode: httpCode);
        }
    }
}
