using System;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Infrastructure.Scope
{
    public class CustomServiceScopeFactory : IServiceScopeFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public CustomServiceScopeFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public IServiceScope CreateScope()
        {
            return new MyServiceScopeWithActivityStarter(_serviceProvider.CreateScope());
        }
    }
}
