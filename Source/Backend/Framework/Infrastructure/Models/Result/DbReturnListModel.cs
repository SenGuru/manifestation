/**
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see <https://opensource.org/licenses/BSD-3-Clause>.
**/
using System;
using System.Collections.Generic;

namespace Framework.Infrastructure.Models.Result
{
    public class DbReturnListModel<T> : IDisposable
    {
        private bool disposed = false;

        public DbReturnListModel()
        {
        }

        public DbReturnListModel(List<T> result)
        {
            this.Result = result;
            this.TotalRows = result.Count;
        }

        public DbReturnListModel(List<T> result, long totalRows)
        {
            this.Result = result;
            this.TotalRows = totalRows;
        }

        public List<T> Result { get; set; }

        public long TotalRows { get; set; }

        public void AppendItem(T item)
        {
            if (item != null)
            {
                Result.Add(item);
            }
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
                Result?.Clear();
                Result = null;
            }

            disposed = true;
        }
    }
}
