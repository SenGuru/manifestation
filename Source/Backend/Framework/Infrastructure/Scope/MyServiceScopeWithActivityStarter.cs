using System;
using System.Diagnostics;
using Framework.Infrastructure.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Infrastructure.Scope
{
    public class MyServiceScopeWithActivityStarter : IServiceScope
    {
        private readonly IServiceScope _innerScope;
        private bool disposedValue;

        public MyServiceScopeWithActivityStarter(IServiceScope innerScope)
        {
            _innerScope = innerScope;
            Trace.CorrelationManager.ActivityId = Guid.NewGuid();
        }

        public IServiceProvider ServiceProvider => _innerScope.ServiceProvider;

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _innerScope.Dispose();
                }

                disposedValue = true;
            }
        }
    }
}
