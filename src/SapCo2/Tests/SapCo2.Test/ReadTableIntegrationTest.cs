using System;
using System.Collections.Generic;
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
    public class ReadTableIntegrationTest
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
        public void GetTable_ShouldRowCountVendorModel()
        {
            using var connection = ServiceProvider.GetService<IRfcConnection>();
            connection.Connect();
            using var tableFunction = ServiceProvider.GetService<IReadTable<Vendor>>();
            var result = tableFunction.GetTable(connection, new List<string> {"BRSCH EQ 'SD00'"}, rowCount: 5);

            Assert.AreEqual(result.Count,5);
        }

        [TestMethod]
        public void GetTable_With_AbapQuery_ShouldVendorModel()
        {
            var rowCount = 5;
            var query=new AbapQuery().Set(QueryOperator.Equal("BRSCH","SD00")).GetQuery();
            
            using var connection = ServiceProvider.GetService<IRfcConnection>();
            connection.Connect();
            using var tableFunction = ServiceProvider.GetService<IReadTable<Vendor>>();
            var result = tableFunction.GetTable(connection, query, rowCount: rowCount);

            Assert.AreEqual(result.Count, rowCount);
        }
    }
}
