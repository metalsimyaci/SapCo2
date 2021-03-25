using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SapCo2.Models;
using SapCo2.Samples.Core.Abstracts;
using SapCo2.Samples.Core.Extensions;
using SapCo2.Samples.Core.Models;

namespace SapCo2.Samples.Console.NetFramework48
{
    internal static class Program
    {
        #region Preperation 

        #region Constants

        private const string AppSettingsFileName = "appSettings.json";
        private const string UserSecretId = "583914EA-59A4-4EC0-AD07-C8CBC3C48424";
        private static bool DisplayedWelcomeMessage;
        #endregion

        #region Variables

        private static IServiceCollection ServiceCollection;
        private static IConfigurationBuilder ConfigurationBuilder;
        private static readonly Dictionary<char, Tuple<string, string>> Operations = new Dictionary<char, Tuple<string, string>>()
        {
            { '1', new Tuple<string, string>("RFC Samples", "GetBillOfMaterial") },
            { '2', new Tuple<string, string>("RFC Samples", "GetBillOfMaterials") },
            { '3', new Tuple<string, string>("Table Samples", "GetMaterial") },
            { '4', new Tuple<string, string>("Table Samples", "GetMaterialsByPrefixWithSubTable") },
            { '5', new Tuple<string, string>("Bapi Samples", "GetVendors") },
            { '6', new Tuple<string, string>("RFC Samples", "GetSAPJobs") },
            { '7', new Tuple<string, string>("MetaData Samples", "GetFunctionMetaData") },
        };

        #endregion

        #region Properties

        private static IServiceProvider ServiceProvider { get; set; }
        private static IConfiguration ApplicationConfiguration { get; set; }

        #endregion

        #region Configuration

        private static void ConfigureServices()
        {
            ServiceCollection = new ServiceCollection();
            ServiceCollection.AddSapCo2SampleCore(ApplicationConfiguration);
            ServiceProvider = ServiceCollection.BuildServiceProvider();
        }

        #endregion

        #endregion

        public static async Task Main(string[] args)
        {
            ConfigurationBuilder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile(AppSettingsFileName, true, true)
                .AddUserSecrets(UserSecretId, true)
                .AddEnvironmentVariables();
            ApplicationConfiguration = ConfigurationBuilder.Build();

            ConfigureServices();

            try
            {
                await Menu();
            }
            catch (Exception e)
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine(e);
                throw;
            }
        }

        #region Menu Operation

        private static void WelcomeMessage()
        {
            if (!DisplayedWelcomeMessage)
                return;
            System.Console.WriteLine("".PadLeft(20, '='));
            System.Console.WriteLine("====== WELCOME TO SAPCO2 SAMPLES FOR NET 5.0 ===========");
            System.Console.WriteLine("".PadLeft(20, '='));
            System.Console.WriteLine("\n\n");
            DisplayedWelcomeMessage = true;
        }
        private static async Task Menu()
        {
            ConsoleKeyInfo keyCode;

            do
            {
                WelcomeMessage();
                System.Console.WriteLine("====== MENU ===========");
                System.Console.WriteLine("".PadLeft(20, '='));

                foreach (KeyValuePair<char, Tuple<string, string>> operation in Operations)
                    System.Console.WriteLine($"{operation.Key} - {operation.Value.Item1} - {operation.Value.Item2}");

                System.Console.WriteLine("--------------------------------------");
                System.Console.WriteLine("0 - Exit");
                System.Console.WriteLine("".PadLeft(20, '='));
                System.Console.Write("Please select an operation:");
                keyCode = System.Console.ReadKey();
            } while (keyCode.KeyChar != '0' && !Operations.ContainsKey(keyCode.KeyChar));
            await ExecuteOperation(keyCode.KeyChar);
            await Menu();
        }
        private static async Task ExecuteOperation(char keyCode)
        {
            switch (keyCode)
            {
            case '0':
                Environment.Exit(0);
                break;
            case '1':
                await GetBillOfMaterialAsync();
                break;
            case '2':
                await GetBillOfMaterialsAsync();
                break;
            case '3':
                await GetMaterialAsync();
                break;
            case '4':
                await GetMaterialsByPrefixWithSubTablesAsync();
                break;
            case '5':
                await GetVendorsUseBapi();
                break;
            case '6':
                await GetJobsAsync();
                break;
            case '7':
                GetFunctionMetaData();
                break;
            default:
                System.Console.WriteLine("Menu Key not found!");
                break;
            }
        }

        #endregion

        #region Example Execution Methods

        private static async Task GetJobsAsync()
        {
            IJobManager manager = ServiceProvider.GetRequiredService<IJobManager>();
            GetJobOutputParameter jobs = await manager.GetJobsAsync();
            manager.Print(jobs);
        }
        private static async Task GetVendorsUseBapi()
        {
            const string COMPANY_CODE = "200";

            IVendorManager manager = ServiceProvider.GetRequiredService<IVendorManager>();
            VendorBapiOutputParameter result = await manager.GetVendorsByCompanyCodeAsync(COMPANY_CODE);
            manager.Print(result);
        }
        private static async Task GetMaterialAsync()
        {
            const string MATERIAL_CODE = "1DACTV76A201000002";

            IMaterialManager manager = ServiceProvider.GetRequiredService<IMaterialManager>();
            Material result = await manager.GetMaterialWithSubTableAsync(MATERIAL_CODE);
            manager.Print(result);
        }
        private static async Task GetMaterialsByPrefixWithSubTablesAsync()
        {
            const string MATERIAL_PREFIX = "11AKPAK";
            const int RECORD_COUNT = 10;

            IMaterialManager manager = ServiceProvider.GetRequiredService<IMaterialManager>();
            List<Material> result = await manager.GetMaterialsByPrefixWithSubTablesAsync(MATERIAL_PREFIX, RECORD_COUNT);
            manager.Print(result);
        }
        private static async Task GetBillOfMaterialsAsync()
        {
            var billOfMaterialList = new List<Tuple<string, string, string>>()
            {
                new Tuple<string, string, string>("200ALFS0304000018", "LIVE", "201"),
                new Tuple<string, string, string>("200DYNA0102000000", "LIVE", "201"),
                new Tuple<string, string, string>("200ELAC0303000738", "LIVE", "201"),
                new Tuple<string, string, string>("200RUBA0305000861", "LIVE", "201"),
                new Tuple<string, string, string>("201STBK1603000092", "LIVE", "201"),
                new Tuple<string, string, string>("203BARB2306000021", "LIVE", "201"),
                new Tuple<string, string, string>("203ELAC2D06000000", "LIVE", "201"),
                new Tuple<string, string, string>("204LMAC3K04000260", "LIVE", "201"),
                new Tuple<string, string, string>("204STBKS903000089", "LIVE", "201"),
                new Tuple<string, string, string>("207ALBZ1002000400", "LIVE", "201"),
                new Tuple<string, string, string>("207SLPT0000000000", "LIVE", "201"),
                new Tuple<string, string, string>("207SMCF1602000900", "LIVE", "201"),
                new Tuple<string, string, string>("200ALFS0304000018", "TEST", "201"),
                new Tuple<string, string, string>("200DYNA0102000000", "TEST", "201"),
                new Tuple<string, string, string>("200ELAC0303000738", "TEST", "201"),
                new Tuple<string, string, string>("200RUBA0305000861", "TEST", "201"),
                new Tuple<string, string, string>("201STBK1603000092", "TEST", "201"),
                new Tuple<string, string, string>("203BARB2306000021", "TEST", "201"),
                new Tuple<string, string, string>("203ELAC2D06000000", "TEST", "201"),
                new Tuple<string, string, string>("204LMAC3K04000260", "TEST", "201"),
                new Tuple<string, string, string>("204STBKS903000089", "TEST", "201"),
            };

            IBillOfMaterialManager manager = ServiceProvider.GetRequiredService<IBillOfMaterialManager>();

            await Task.WhenAll(billOfMaterialList.Select(material => Task.Run(async () =>
            {
                BomOutputParameter result = await manager.GetBillOfMaterialAsync(material.Item1, material.Item3, material.Item2);
                manager.Print(result?.Topmat);
            })).ToArray()).ConfigureAwait(false);
        }
        private static async Task GetBillOfMaterialAsync()
        {
            const string MATERIAL_CODE = "200DYNA0102000000";
            const string PLANT_CODE = "201";

            IBillOfMaterialManager manager = ServiceProvider.GetRequiredService<IBillOfMaterialManager>();
            BomOutputParameter result = await manager.GetBillOfMaterialAsync(MATERIAL_CODE, PLANT_CODE);
            manager.Print(result);
        }
        private static void GetFunctionMetaData()
        {
            const string FUNCTION_NAME = "ZBC_GET_JOBS";

            IFunctionMetaDataManager manager = ServiceProvider.GetRequiredService<IFunctionMetaDataManager>();
            List<ParameterMetaData> metaData = manager.GetFunctionMetaData(FUNCTION_NAME);
            manager.Print(metaData);
        }

        #endregion

    }
}
