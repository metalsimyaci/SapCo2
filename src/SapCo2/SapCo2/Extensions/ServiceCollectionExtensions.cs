using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SapCo2.Abstract;
using SapCo2.Core;
using SapCo2.Core.Extensions;
using SapCo2.Utility;

namespace SapCo2.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSapCo2(this IServiceCollection serviceCollection, Action<RfcConnectionOption> connectionAction)
        {
            serviceCollection.AddSapCo2Core(connectionAction: connectionAction);
           
            serviceCollection.TryAddTransient<IPropertyCache, PropertyCache>();
            serviceCollection.TryAddTransient(typeof(IReadTable<>),typeof(ReadTable<>));

            serviceCollection.BuildServiceProvider();
            return serviceCollection;
        }
        public static IServiceCollection AddSapCo2(this IServiceCollection serviceCollection, string connectionString)
        {
            serviceCollection.AddSapCo2Core(connectionString: connectionString);

            serviceCollection.TryAddTransient<IPropertyCache, PropertyCache>();
            serviceCollection.TryAddTransient(typeof(IReadTable<>), typeof(ReadTable<>));

            serviceCollection.BuildServiceProvider();
            return serviceCollection;
        }
    }
}
