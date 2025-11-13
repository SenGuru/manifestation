using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Caching
{
    public class ScopedCache : IDisposable
    {
        private readonly Dictionary<string, object> _cache = new Dictionary<string, object>();
        private bool _disposed = false;

        ~ScopedCache()
        {
            Dispose(false);
        }

        public object Get(string key)
        {
            _cache.TryGetValue(key, out var value);
            return value;
        }

        public T Get<T>(string key)
            where T : class
        {
            object returnValue = Get(key);

            if (returnValue is T)
                return returnValue as T;

            return null;
        }

        public void Set(string key, object value)
        {
            _cache[key] = value;
        }

        public void Set<T>(string key, T value)
            where T : class
        {
            _cache[key] = value;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _cache.Clear();
                }

                _disposed = true;
            }
        }
    }
}