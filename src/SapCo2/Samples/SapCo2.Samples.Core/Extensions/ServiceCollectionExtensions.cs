using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using SapCo2.Core.Extensions;
using SapCo2.Core.Models;
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
            //services.AddSapCo2(s =>
            //{
            //    s.DefaultServer = "LIVE";
            //    s.RfcServers = new List<RfcServer>()
            //    {
            //        new RfcServer()
            //        {
            //            Alias = "LIVE",
            //            ConnectionString =
            //                "Name=LIVE;User=USER;Password=PASSWORD;Client=CLIENT_CODE;SystemId:xxx;Language=EN;AppServerHost=HOST_NAME;SystemNumber=00;MaxPoolSize:100;PoolSize=50;IdleTimeout:600;Trace=0;",
            //            ConnectionPooling = new RfcConnectionPoolingOption() {Enabled = true, PoolSize = 10}
            //        }
            //    };
            //});
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
