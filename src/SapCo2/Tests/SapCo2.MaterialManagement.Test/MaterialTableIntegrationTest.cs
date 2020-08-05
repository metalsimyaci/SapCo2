using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SapCo2.Extensions;

namespace SapCo2.MaterialManagement.Test
{
    [TestClass]
    [TestCategory("IntegrationTest")]
    public class MaterialTableIntegrationTest
    {
        #region Variables

        private const string EnvironmentVariableName = "ASPNETCORE_ENVIRONMENT";
        private const string SapSectionName = "Sap";
        private static IServiceProvider ServiceProvider;
        
        #endregion

        #region Initializer
        
        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            var env = Environment.GetEnvironmentVariable(EnvironmentVariableName, EnvironmentVariableTarget.Machine) ??
                      "Development";

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
        
        #endregion

        #region Test Methods
        
        [TestMethod]
        public async Task GetMaterialAsync_materialCode_shouldMaterial()
        {
            var materialCode = "1DACTV51A201000001";

            var materialManager = new MaterialManager(ServiceProvider);
            var result =
                await materialManager.GetMaterialAsync(materialCode, new MaterialQueryOptions() {IncludeAll = true});

            Assert.AreEqual(result.Code, materialCode);
        }
        
        [TestMethod]
        public async Task GetMaterialsAsync_materialCodePrefix_shouldTop5Material()
        {
            var materialCodePrefix = "1DACTV";
            var rowCount = 5;

            var materialManager = new MaterialManager(ServiceProvider);
            var result = await materialManager.GetMaterialsAsync(materialCodePrefix, 
                    new MaterialQueryOptions() {IncludeAll = true},true, rowCount);

            Assert.AreEqual(result.Count, rowCount);
        }
        
        #endregion
    }
}
