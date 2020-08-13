using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SapCo2.Extensions;
using SapCo2.Mm;
using SapCo2.Samples.NetCore.BapiExamples;
using SapCo2.Samples.NetCore.RfcExamples;
using SapCo2.Samples.NetCore.RfcExamples.Models;
using SapCo2.Samples.NetCore.TableExamples;

namespace SapCo2.Samples.NetCore
{
    internal static class Program
    {
        private static IServiceProvider ServiceProvider;
        private static bool DisplayedWelcomeMessage;
        private const string SapSectionName = "SapServerConnections:Sap";
        private static void Main()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddUserSecrets("d43bb0e9-3a7e-4cb4-9ebb-3cf7f9d826bd")
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetSection(SapSectionName).Value;

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSapCo2(connectionString);
            ServiceProvider = serviceCollection.BuildServiceProvider();

            Menu();
        }


        private static void WelcomeMessage()
        {
            if (!DisplayedWelcomeMessage)
                return;
            Console.WriteLine("".PadLeft(20, '='));
            Console.WriteLine("====== WELCOME TO SAPCO2 SAMPLES ===========");
            Console.WriteLine("".PadLeft(20, '='));
            Console.WriteLine("\n\n");
            DisplayedWelcomeMessage = true;
        }
        private static void Menu()
        {
            int keyCode = -1;
            do
            {
                WelcomeMessage();
                Console.WriteLine("====== MENU ===========");
                Console.WriteLine("".PadLeft(20, '='));
                Console.WriteLine("1 - Rfc Samples - GetBillOfMaterial");
                Console.WriteLine("2 - Table Samples - GetMaterials");
                Console.WriteLine("3 - Bapi Samples - GetVendors");
                Console.WriteLine("--------------------------------------");
                Console.WriteLine("0 - Exit");
                Console.WriteLine("".PadLeft(20, '='));
                Console.Write("Please select an operation:");
                keyCode = Console.Read();
            } while (keyCode != 0 && keyCode != 1 && keyCode != 2 && keyCode != 3);
            ShowMenu(keyCode);
        }

        private static void ShowMenu(int keyCode)
        {
            switch (keyCode)
            {
            case 0:
                Environment.Exit(0);
                break;
            case 1:
                GetRfcBillOfMaterial();
                break;
            case 2:
                GetMaterials();
                break;
            case 3:
                GetBapiVendors();
                break;
            default:
                Console.WriteLine("Menu Key not found!");
                break;
            }
        }


        private static void GetRfcBillOfMaterial()
        {
            string materialCode = "200ARGS0203000000";
            string plantcode = "201";

            var manager = new BillOfMaterialManager(ServiceProvider);
            BomOutputParameter bom = manager.GetBillOfMaterial(materialCode, plantcode);
            manager.Print(bom);
        }
        private static void GetBapiVendors()
        {
            string companyCode = "201";

            var manager = new VendorManager(ServiceProvider);
            var ressult = manager.GetVerdorsByCompanyCode(companyCode);
            manager.Print(ressult);
        }
        private static void GetMaterials()
        {
            string materialPrefix = "200";
            int rowCount = 5;

            var manager = new MaterialManager(ServiceProvider);
            //Material Category Table spesfic development table in ABAP
            //Not used Material Category change MaterialQueryOptions
            manager.GetMaterialsByPrefixAsync(materialPrefix, new MaterialQueryOptions {IncludeAll = true}, true, rowCount)
                .ContinueWith(async s =>
                    manager.Print(await s.ConfigureAwait(false)));
        }
    }
}
