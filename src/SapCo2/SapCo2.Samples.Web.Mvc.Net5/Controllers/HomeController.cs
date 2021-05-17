using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SapCo2.Samples.Web.Mvc.Net5.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SapCo2.Models;
using SapCo2.Samples.Core.Abstracts;
using SapCo2.Samples.Core.Models;

namespace SapCo2.Samples.Web.Mvc.Net5.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IServiceProvider _serviceProvider;

        public HomeController(ILogger<HomeController> logger,IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetJobs()
        {
            IJobManager manager = _serviceProvider.GetRequiredService<IJobManager>();
            GetJobOutputParameter jobs = await manager.GetJobsAsync();
            return View(jobs.JobStatuses);
        }

        public async Task<IActionResult> GetVendorsUseBapi()
        {
            const string COMPANY_CODE = "200";

            IVendorManager manager = _serviceProvider.GetRequiredService<IVendorManager>();
            VendorBapiOutputParameter result = await manager.GetVendorsByCompanyCodeAsync(COMPANY_CODE);
            return View(result.Vendors);
        }
        public async Task<IActionResult> GetMaterial()
        {
            const string MATERIAL_CODE = "1DACTV76A201000002";

            IMaterialManager manager = _serviceProvider.GetRequiredService<IMaterialManager>();
            Material result = await manager.GetMaterialWithSubTableAsync(MATERIAL_CODE);
            return View(result);
        }
        public async Task<IActionResult> GetMaterialsByPrefixWithSubTables()
        {
            const string MATERIAL_PREFIX = "11AKPAK";
            const int RECORD_COUNT = 10;

            IMaterialManager manager = _serviceProvider.GetRequiredService<IMaterialManager>();
            List<Material> result = await manager.GetMaterialsByPrefixWithSubTablesAsync(MATERIAL_PREFIX, RECORD_COUNT);
            return View(result);
        }

        public async Task<IActionResult> GetBillOfMaterials()
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

            IBillOfMaterialManager manager = _serviceProvider.GetRequiredService<IBillOfMaterialManager>();
            BomOutputParameter result;
            await Task.WhenAll(billOfMaterialList.Select(material => Task.Run(async () =>
            {
                result = await manager.GetBillOfMaterialAsync(material.Item1, material.Item3, material.Item2);
                manager.Print(result?.Topmat);
            })).ToArray()).ConfigureAwait(false);
            return View();
        }

        public async Task<IActionResult> GetBillOfMaterial()
        {
            const string MATERIAL_CODE = "200DYNA0102000000";
            const string PLANT_CODE = "201";

            IBillOfMaterialManager manager = _serviceProvider.GetRequiredService<IBillOfMaterialManager>();
            BomOutputParameter result = await manager.GetBillOfMaterialAsync(MATERIAL_CODE, PLANT_CODE);
            //return View(result);
            return View();
        }
        public async Task<IActionResult> GetFunctionMetaData()
        {
            const string FUNCTION_NAME = "ZBC_GET_JOBS";

            IFunctionMetaDataManager manager = _serviceProvider.GetRequiredService<IFunctionMetaDataManager>();
            List<ParameterMetaData> metaData = manager.GetFunctionMetaData(FUNCTION_NAME);
            //return View(metaData);
            return View(metaData);
        }
 

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
