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
    [TestCategory("UnitTest")]
    public class UnitTest1
    {
        // ReSharper disable once ArrangeTypeMemberModifiers
        private const string EnviromentVariableName = "ASPNETCORE_ENVIRONMENT";
        // ReSharper disable once ArrangeTypeMemberModifiers
        private const string SapSectionName = "Sap";
        private IServiceProvider _serviceProvider;


        [TestInitialize]
        public void Initialize()
        {
            var env = Environment.GetEnvironmentVariable(EnviromentVariableName,EnvironmentVariableTarget.Machine)??"Development";

            IConfiguration configuration = new ConfigurationBuilder()
             .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
             .AddJsonFile($"appsettings.{env}.json", optional: true)
             .AddEnvironmentVariables()
             .Build();

            var connectionString = configuration.GetConnectionString(SapSectionName);

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSapCo2Core(connectionString);
            _serviceProvider=serviceCollection.BuildServiceProvider();

        }



        [TestMethod]
        public void TestMethod1()
        {
            using var connection = _serviceProvider.GetService<IRfcConnection>();
            connection.Connect();
            using var function =_serviceProvider.GetService<IRfcFunction>().CreateFunction(connection,"ZRFC_READ_TABLE");
            var result= function.Invoke<RfcReadTableOutputParameter>(new RfcReadTableInputParameter
            {
                Query = "LFA1",
                Delimiter = "|",
                NoData = "",
                RowCount = 5,
                RowSkips = 0,
                Fields = new[]
                        {
                            new RfcReadTableField {FieldName = "LIFNR"}, new RfcReadTableField {FieldName = "NAME1"},
                            new RfcReadTableField {FieldName = "NAME2"}, new RfcReadTableField {FieldName = "KUNNR"}
                        },
                Options = new[] { new RfcReadTableOption { Text = "BRSCH EQ 'SD00'" }, }
            });

            Assert.AreEqual(result.Data.Length, 5);
            

        }
    }
}
