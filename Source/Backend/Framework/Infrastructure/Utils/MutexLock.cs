/*
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see https://opensource.org/licenses/BSD-3-Clause.
*/
using System;
using System.Threading;

namespace Framework.Infrastructure.Utils
{
    public class MutexLock : IDisposable
    {
        private readonly Mutex mutex;
        private bool disposed = false;

        public MutexLock(string name)
        {
            try
            {
                mutex = new Mutex(true, name, out bool isCreatedNew);
                if (!isCreatedNew)
                    mutex.WaitOne();
            }
            catch (AbandonedMutexException ex)
            {
                if (ex.Mutex != null)
                    ex.Mutex.ReleaseMutex();
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
                if (mutex != null)
                {
                    mutex.ReleaseMutex();
                    mutex.Dispose();
                }
            }

            disposed = true;
        }
    }
}
