/**
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see <https://opensource.org/licenses/BSD-3-Clause>.
**/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json.Serialization;
using Framework.Infrastructure.Constants;
using Framework.Infrastructure.Exceptions;

namespace Framework.Infrastructure.Models.Result
{
    public class ReturnListWithSearchModel<TModel, TSearch> : IReturnModel, IDisposable
        where TSearch : class
    {
        private bool disposed = false;

        public ReturnListWithSearchModel()
        {
            IsSuccess = false;
            ActivityId = Trace.CorrelationManager.ActivityId.ToString();
            ActivityDateTime = DateTime.UtcNow;
            Server = Environment.MachineName;
            ProcessId = Environment.ProcessId;
        }

        public ReturnListWithSearchModel(TSearch search, List<TModel> items, long totalItems, int httpCode = HttpCodeContants.Success)
        {
            Model = items;
            TotalRecords = totalItems;
            IsSuccess = true;
            Search = search;
            HttpCode = httpCode;
            ActivityId = Trace.CorrelationManager.ActivityId.ToString();
            ActivityDateTime = DateTime.UtcNow;
            Server = Environment.MachineName;
            ProcessId = Environment.ProcessId;
        }

        public ReturnListWithSearchModel(TSearch search, List<TModel> items, int httpCode = HttpCodeContants.Success)
        {
            Model = items;
            TotalRecords = items?.Count ?? 0;
            IsSuccess = true;
            Search = search;
            HttpCode = httpCode;
            ActivityId = Trace.CorrelationManager.ActivityId.ToString();
            ActivityDateTime = DateTime.UtcNow;
            Server = Environment.MachineName;
            ProcessId = Environment.ProcessId;
        }

        public ReturnListWithSearchModel(List<TModel> items, long totalItems, int httpCode = HttpCodeContants.Success)
            : this(null, items, totalItems, httpCode)
        {
        }

        public ReturnListWithSearchModel(DbReturnListModel<TModel> items, int httpCode = HttpCodeContants.Success)
            : this(null, items.Result, items.TotalRows, httpCode)
        {
        }

        public ReturnListWithSearchModel(List<TModel> items, int httpCode = HttpCodeContants.Success)
            : this(null, items, httpCode)
        {
        }

        public ReturnListWithSearchModel(TSearch search, Exception ex, int httpCode = HttpCodeContants.Success)
        {
            Search = search;
            IsSuccess = false;
            HttpCode = httpCode;
            ActivityId = Trace.CorrelationManager.ActivityId.ToString();
            ActivityDateTime = DateTime.UtcNow;
            Server = Environment.MachineName;
            ProcessId = Environment.ProcessId;
            if (ex != null)
            {
                ErrorHolder = new ReturnError(ex);
            }
        }

        public ReturnListWithSearchModel(TSearch search, string errorMsg, Exception ex = null, string internalMsg = null, int httpCode = HttpCodeContants.Success)
        {
            Search = search;
            IsSuccess = false;
            HttpCode = httpCode;
            ActivityId = Trace.CorrelationManager.ActivityId.ToString();
            ActivityDateTime = DateTime.UtcNow;
            Server = Environment.MachineName;
            ProcessId = Environment.ProcessId;
            ErrorHolder = new ReturnError(errorMsg, ex, internalMsg);
        }

        public ReturnListWithSearchModel(TSearch search, string errorMsg, List<ReturnErrorItem> errorList, int httpCode = HttpCodeContants.Success)
        {
            Search = search;
            IsSuccess = false;
            HttpCode = httpCode;
            ActivityId = Trace.CorrelationManager.ActivityId.ToString();
            ActivityDateTime = DateTime.UtcNow;
            Server = Environment.MachineName;
            ProcessId = Environment.ProcessId;
            ErrorHolder = new ReturnError(errorMsg, errorList, errorMsg);
        }

        public ReturnListWithSearchModel(Exception ex, int httpCode = HttpCodeContants.Success)
            : this((TSearch)null, ex, httpCode)
        {
        }

        public ReturnListWithSearchModel(string errorMsg, Exception ex = null, string internalMsg = null, int httpCode = HttpCodeContants.Success)
            : this(null, errorMsg, ex, internalMsg, httpCode)
        {
        }

        public ReturnListWithSearchModel(string errorMsg, List<ReturnErrorItem> errorList, int httpCode = HttpCodeContants.Success)
            : this(null, errorMsg, errorList, httpCode)
        {
        }

        public List<TModel> Model { get; set; }

        public TSearch Search { get; set; }

        [JsonIgnore]
        public int ActiveTab { get; set; }

        public long TotalRecords { get; set; }

        public bool IsSuccess { get; set; }

        public string SuccessMessage { get; set; }

        public ReturnError ErrorHolder { get; set; }

        [JsonIgnore]
        public int HttpCode { get; set; }

        [JsonIgnore]
        public bool AlreadyWrittenToLog { get; protected set; }

        public string ActivityId { get; set; }

        public DateTime ActivityDateTime { get; set; }

        public string Server { get; set; }

        public int ProcessId { get; set; }

        public static ReturnListWithSearchModel<TModel, TSearch> Success(TSearch search, List<TModel> objList, string sucessMessage = null, int httpCode = HttpCodeContants.Success)
        {
            return new ReturnListWithSearchModel<TModel, TSearch>(search, objList) { IsSuccess = true, SuccessMessage = sucessMessage, HttpCode = httpCode };
        }

        public static ReturnListWithSearchModel<TModel, TSearch> Success(DbReturnListModel<TModel> objList, string sucessMessage = null, int httpCode = HttpCodeContants.Success)
        {
            return new ReturnListWithSearchModel<TModel, TSearch>(objList.Result, objList.TotalRows) { IsSuccess = true, SuccessMessage = sucessMessage, HttpCode = httpCode };
        }

        public static ReturnListWithSearchModel<TModel, TSearch> Success(TSearch search, List<TModel> objList, long totalItems, string sucessMessage = null, int httpCode = HttpCodeContants.Success)
        {
            return new ReturnListWithSearchModel<TModel, TSearch>(search, objList, totalItems) { IsSuccess = true, SuccessMessage = sucessMessage, HttpCode = httpCode };
        }

        public static ReturnListWithSearchModel<TModel, TSearch> Success(List<TModel> objList, long totalItems, string sucessMessage = null, int httpCode = HttpCodeContants.Success)
        {
            return new ReturnListWithSearchModel<TModel, TSearch>(objList, totalItems) { IsSuccess = true, SuccessMessage = sucessMessage, HttpCode = httpCode };
        }

        public static ReturnListWithSearchModel<TModel, TSearch> Success(List<TModel> objList, string sucessMessage = null, int httpCode = HttpCodeContants.Success)
        {
            return new ReturnListWithSearchModel<TModel, TSearch>(objList, objList.Count) { IsSuccess = true, SuccessMessage = sucessMessage, HttpCode = httpCode };
        }

        public static ReturnListWithSearchModel<TModel, TSearch> Error(IReturnModel model, bool alreadyWrittenToLog = false)
        {
            return Error(model, model.HttpCode, alreadyWrittenToLog);
        }

        public static ReturnListWithSearchModel<TModel, TSearch> Error(IReturnModel model, int httpCode, bool alreadyWrittenToLog = false)
        {
            return new ReturnListWithSearchModel<TModel, TSearch> { IsSuccess = model.IsSuccess, ErrorHolder = model.ErrorHolder, SuccessMessage = model.SuccessMessage, HttpCode = httpCode, AlreadyWrittenToLog = alreadyWrittenToLog };
        }

        public static ReturnListWithSearchModel<TModel, TSearch> Error(Exception ex, int httpCode = HttpCodeContants.Success, bool alreadyWrittenToLog = false)
        {
            return new ReturnListWithSearchModel<TModel, TSearch>(ex) { IsSuccess = false, HttpCode = httpCode, AlreadyWrittenToLog = alreadyWrittenToLog };
        }

        public static ReturnListWithSearchModel<TModel, TSearch> Error(string errorMsg, int httpCode = HttpCodeContants.Success, bool alreadyWrittenToLog = false)
        {
            return new ReturnListWithSearchModel<TModel, TSearch>(errorMsg) { IsSuccess = false, HttpCode = httpCode, AlreadyWrittenToLog = alreadyWrittenToLog };
        }

        public static ReturnListWithSearchModel<TModel, TSearch> Error(string errorMsg, Exception ex, int httpCode = HttpCodeContants.Success, bool alreadyWrittenToLog = false)
        {
            return new ReturnListWithSearchModel<TModel, TSearch>(errorMsg, ex) { IsSuccess = false, HttpCode = httpCode, AlreadyWrittenToLog = alreadyWrittenToLog };
        }

        public static ReturnListWithSearchModel<TModel, TSearch> Error(string errorMsg, Exception ex, string internalMsg, int httpCode = HttpCodeContants.Success, bool alreadyWrittenToLog = false)
        {
            return new ReturnListWithSearchModel<TModel, TSearch>(errorMsg, ex, internalMsg) { IsSuccess = false, HttpCode = httpCode, AlreadyWrittenToLog = alreadyWrittenToLog };
        }

        public static ReturnListWithSearchModel<TModel, TSearch> Error(string errorMsg, List<ReturnErrorItem> errorList, int httpCode = HttpCodeContants.Success, bool alreadyWrittenToLog = false)
        {
            return new ReturnListWithSearchModel<TModel, TSearch>(errorMsg, errorList) { IsSuccess = false, HttpCode = httpCode, AlreadyWrittenToLog = alreadyWrittenToLog };
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                Model?.Clear();
                Model = null;
            }

            disposed = true;
        }
    }
}