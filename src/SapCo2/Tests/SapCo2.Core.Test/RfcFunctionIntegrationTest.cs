using System;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using SapCo2.Core.Extensions;
using SapCo2.Core.Abstract;
using SapCo2.Core.Test.Model;

namespace SapCo2.Core.Test
{
    [TestClass]
    [TestCategory("IntegrationTest")]
    public class RfcFunctionIntegrationTest
    {
        private const string EnvironmentVariableName = "ASPNETCORE_ENVIRONMENT";
        private const string SapSectionName = "Sap";
        private static IServiceProvider ServiceProvider;


        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            var env = Environment.GetEnvironmentVariable(EnvironmentVariableName, EnvironmentVariableTarget.Machine)??"Development";

            IConfiguration configuration = new ConfigurationBuilder()
             .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
             .AddJsonFile($"appsettings.{env}.json", optional: true)
             .AddEnvironmentVariables()
             .Build();

            var connectionString = configuration.GetConnectionString(SapSectionName);

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSapCo2Core(connectionString);
            ServiceProvider=serviceCollection.BuildServiceProvider();

        }



        [TestMethod]
        public void Invoke_WithOutputAndInput_ShouldMapOutput()
        {
            var rowCount = 5;
            using var connection = ServiceProvider.GetService<IRfcConnection>();
            connection.Connect();
            using var function =ServiceProvider.GetService<IRfcFunction>().CreateFunction(connection,"ZRFC_READ_TABLE");
            
            
            var result= function.Invoke<RfcReadTableOutputParameter>(new RfcReadTableInputParameter
            {
                Query = "LFA1",
                Delimiter = "|",
                NoData = "",
                RowCount = rowCount,
                RowSkips = 0,
                Fields = new[]
                        {
                            new RfcReadTableField {FieldName = "LIFNR"}, new RfcReadTableField {FieldName = "NAME1"},
                            new RfcReadTableField {FieldName = "NAME2"}, new RfcReadTableField {FieldName = "KUNNR"}
                        },
                Options = new[] { new RfcReadTableOption { Text = "BRSCH EQ 'SD00'" }, }
            });

            Assert.AreEqual(result.Data.Length, rowCount);
            

        }
    }
}
