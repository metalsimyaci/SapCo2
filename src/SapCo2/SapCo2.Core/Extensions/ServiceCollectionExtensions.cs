using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SapCo2.Core.Abstract;
using SapCo2.Wrapper.Extension;

namespace SapCo2.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSapCo2Core(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSapCo2Wrapper();
            serviceCollection.TryAddTransient<IRfcConnection, RfcConnection>();
            serviceCollection.TryAddSingleton<IRfcNetWeaverLibrary, RfcNetWeaverLibrary>();
            
            return serviceCollection;
        }
    }
}
