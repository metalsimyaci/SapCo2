using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SapCo2.Abstract;
using SapCo2.Core.Abstract;
using SapCo2.Extensions;
using SapCo2.Query;
using SapCo2.Test.Model;

namespace SapCo2.Test
{
    [TestClass]
    [TestCategory("IntegrationTest")]
    public class MaterialTableIntegrationTest
    {
        private const string EnvironmentVariableName = "ASPNETCORE_ENVIRONMENT";
        private const string SapSectionName = "Sap";
        private static IServiceProvider ServiceProvider;


        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            var env = Environment.GetEnvironmentVariable(EnvironmentVariableName, EnvironmentVariableTarget.Machine) ?? "Development";

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString(SapSectionName);

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSapCo2(connectionString);
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        [ClassCleanup]
        public static void Cleaner()
        {
            ServiceProvider = null;
        }


        [TestMethod]
        public async Task GetMaterial_With_AbapQuery_ShouldVendorModel()
        {
            var materialCode = "1DACTV51A201000001";

            var materialManager = new MaterialManager(ServiceProvider);
            var result = await materialManager.GetMaterialAsync(materialCode,new MaterialQueryOptions(){IncludeAll = true},true);

            Assert.AreEqual(result.Code, materialCode);
        }
    }
}
