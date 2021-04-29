using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using SapCo2.Core.Extensions;
using SapCo2.Extensions;
using SapCo2.Samples.Core.Abstracts;
using SapCo2.Samples.Core.Managers;

namespace SapCo2.Samples.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSapCo2SampleCore(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddOptions<IConfiguration>();
            services.Configure<IOptions<IConfiguration>>(configuration);
            services.AddSapCo2(s=>s.ReadFromConfiguration(configuration));

            services.TryAddTransient<IJobManager, JobManager>();
            services.TryAddTransient<IVendorManager, VendorManager>();
            services.TryAddSingleton<IMaterialManager, MaterialManager>();
            services.TryAddSingleton<IBillOfMaterialManager, BillOfMaterialManager>();
            services.TryAddSingleton<IFunctionMetaDataManager, FunctionMetaDataManager>();
            services.TryAddSingleton<IMaterialSaveDataManager, MaterialSaveDataManager>();
            services.TryAddSingleton<IGoodsMovementManager, GoodsMovementManager>();
            services.BuildServiceProvider();

            return services;
        }
    }
}
