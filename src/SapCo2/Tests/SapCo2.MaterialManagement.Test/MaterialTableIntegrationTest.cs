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

        private const string SapSectionName = "SapServerConnections:Sap";
        private static IServiceProvider ServiceProvider;
        
        #endregion

        #region Initializer
        
        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddUserSecrets("4C01C11E-B306-4D85-8947-C06AA454C358")
                .AddJsonFile("appsettings.json", true)
                .AddEnvironmentVariables()
                .Build();
            var connectionString = configuration.GetSection(SapSectionName).Value;

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
        public async Task GetMaterialAsync_materialCodeAndIncludeAllQueryOptions_shouldMaterial()
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
