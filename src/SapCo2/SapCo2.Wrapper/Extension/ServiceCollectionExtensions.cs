using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SapCo2.Wrapper.Abstract;
using SapCo2.Wrapper.Interop;

namespace SapCo2.Wrapper.Extension
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSapCo2Wrapper(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<IRfcInterop,RfcInterop>();
            serviceCollection.BuildServiceProvider();
            return serviceCollection;
        }
    }
}
