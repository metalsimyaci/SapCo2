using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SapCo2.Core.Abstract;
using SapCo2.Wrapper.Extension;

namespace SapCo2.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSapCo2Core(this IServiceCollection serviceCollection, Action<RfcConnectionOption> connectionAction)
        {
            serviceCollection.AddSapCo2Wrapper();

            serviceCollection.AddOptions<RfcConnectionOption>();
            serviceCollection.Configure(connectionAction);

            serviceCollection.TryAddTransient<IRfcConnection, RfcConnection>();
            serviceCollection.TryAddTransient<IRfcFunction, RfcFunction>();
            serviceCollection.TryAddSingleton<IRfcLibrary, RfcLibrary>();

            serviceCollection.BuildServiceProvider();
            return serviceCollection;
        }
        public static IServiceCollection AddSapCo2Core(this IServiceCollection serviceCollection, string connectionString)
        {
            return AddSapCo2Core(serviceCollection, o => o.Parse(connectionString));
        }
       
    }
}
