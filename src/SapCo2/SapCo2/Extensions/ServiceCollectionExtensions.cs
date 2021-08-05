using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using SapCo2.Abstract;
using SapCo2.Core;
using SapCo2.Core.Abstract;
using SapCo2.Core.Extensions;
using SapCo2.Core.Models;
using SapCo2.Utility;

namespace SapCo2.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSapCo2(this IServiceCollection services, Action<RfcConfiguration> configurationAction)
        {
            services.AddOptions();
            services.Configure(configurationAction);
            services.AddSapCo2Core();
            services.TryAddTransient<IRfcClient, RfcClient>();
            services.TryAddTransient<IRfcConnectionPoolServiceFactory, RfcConnectionPoolServiceFactory>();
            services.TryAddSingleton<IPropertyCache, PropertyCache>();

            RfcConfiguration sapConfiguration = services.BuildServiceProvider().GetRequiredService<IOptions<RfcConfiguration>>().Value;

            GenerateConnectionPools(services, sapConfiguration);
            return services;
        }
        private static void GenerateConnectionPools(IServiceCollection services, RfcConfiguration rfcConfiguration)
        {
            foreach (RfcServer sapServerConnection in rfcConfiguration.RfcServers)
            {
                if (sapServerConnection.ConnectionPooling.Enabled)
                {
                    services.AddSingleton<IRfcConnectionPool>(s => new RfcConnectionPool(s, sapServerConnection.Alias, rfcConfiguration));
                }
            }
        }
    }
}
