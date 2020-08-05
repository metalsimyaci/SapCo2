using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SapCo2.Abstract;
using SapCo2.Core.Abstract;
using SapCo2.Extensions;
using SapCo2.Test.Model;

namespace SapCo2.Test
{
    [TestClass]
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
            var env = Environment.GetEnvironmentVariable(EnviromentVariableName, EnvironmentVariableTarget.Machine) ?? "Development";

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString(SapSectionName);

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSapCo2(connectionString);
            _serviceProvider = serviceCollection.BuildServiceProvider();

        }

        [TestMethod]
        public void TestMethod1()
        {
            using var connection = _serviceProvider.GetService<IRfcConnection>();
            connection.Connect();
            using var tableFunction = _serviceProvider.GetService<IReadTable<Vendor>>();
            var result = tableFunction.GetTable(connection, new List<string> {"BRSCH EQ 'SD00'"}, rowCount: 5);

            Assert.AreEqual(result.Count,5);
        }
    }
}
