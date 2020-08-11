using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SapCo2.Core;
using SapCo2.Extensions;
using SapCo2.Mm.Abstract;

namespace SapCo2.Mm.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMaterialManagement(this IServiceCollection serviceCollection, Action<RfcConnectionOption> connectionAction)
        {
            serviceCollection.AddSapCo2(connectionAction: connectionAction);
           
            serviceCollection.TryAddTransient<IMaterialManager, MaterialManager>();

            serviceCollection.BuildServiceProvider();
            return serviceCollection;
        }
        public static IServiceCollection AddMaterialManagement(this IServiceCollection serviceCollection, string connectionString)
        {
            serviceCollection.AddSapCo2(connectionString: connectionString);

            serviceCollection.TryAddTransient<IMaterialManager, MaterialManager>();

            serviceCollection.BuildServiceProvider();
            return serviceCollection;
        }
    }
}
