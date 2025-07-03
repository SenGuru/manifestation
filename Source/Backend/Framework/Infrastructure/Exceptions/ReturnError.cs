/**
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see <https://opensource.org/licenses/BSD-3-Clause>.
**/
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Framework.Infrastructure.Utils;

namespace Framework.Infrastructure.Exceptions
{
    [Serializable]
    public class ReturnError : IDisposable
    {
        private bool disposed = false;

        public ReturnError()
        {
        }

        public ReturnError(string friendlyMessage, List<ReturnErrorItem> errorItemList = null, string internalErrorMessage = null)
        {
            FriendlyMessage = friendlyMessage;
            InternalErrorMessage = internalErrorMessage ?? friendlyMessage;
            ErrorItemList = errorItemList;
            Exception = null;
        }

        public ReturnError(string friendlyMessage, List<ReturnErrorItem> errorItemList = null, Exception ex = null, string internalErrorMessage = null)
        {
            FriendlyMessage = friendlyMessage;
            InternalErrorMessage = internalErrorMessage ?? friendlyMessage;
            ErrorItemList = errorItemList;
            if (ex != null)
                Exception = new ReturnException(ex);
        }

        public ReturnError(Exception ex)
        {
            if (ex != null)
            {
                FriendlyMessage = ex.Message;
                InternalErrorMessage = ex.Message;
                ErrorItemList = null;
                Exception = new ReturnException(ex);
            }
        }

        public ReturnError(string friendlyMessage, Exception ex, string internalMessage = null)
        {
            FriendlyMessage = friendlyMessage;
            InternalErrorMessage = internalMessage ?? friendlyMessage;
            ErrorItemList = null;
            if (ex != null)
                Exception = new ReturnException(ex);
        }

        public string FriendlyMessage { get; set; }

        public string InternalErrorMessage { get; set; }

        public List<ReturnErrorItem> ErrorItemList { get; set; }

        public ReturnException Exception { get; set; }

        [JsonIgnore]
        public object AdditionalLogData { get; set; }

        public string AllErrorAsString()
        {
            return JsonUtils.Serialize(this);
        }

        public string AllAdditionalLogDataAsString()
        {
            if (AdditionalLogData == null)
                return "";
            return JsonUtils.Serialize(AdditionalLogData);
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
                ErrorItemList?.Clear();
                ErrorItemList = null;
                Exception = null;
            }

            disposed = true;
        }
    }
}
