using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using SapCo2.Core.Abstract;

namespace SapCo2.Core
{
    public class RfcConnectionPoolServiceFactory:IRfcConnectionPoolServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public RfcConnectionPoolServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IRfcConnectionPool GetService(string sapServerAlias)
        {
            IEnumerable<IRfcConnectionPool> services = _serviceProvider.GetServices<IRfcConnectionPool>();

            return services.FirstOrDefault(s => s.ServerAlias == sapServerAlias);
        }
    }
}
