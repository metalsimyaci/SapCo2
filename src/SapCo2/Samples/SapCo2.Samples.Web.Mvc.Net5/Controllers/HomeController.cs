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

        public HomeController(ILogger<HomeController> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Sample Methods
        
        public async Task<IActionResult> GetJobs()
        {
            try
            {
                IJobManager manager = _serviceProvider.GetRequiredService<IJobManager>();
                GetJobOutputParameter jobs = await manager.GetJobsAsync();
                return View(jobs.JobStatuses);
            }
            catch (Exception e)
            {
                _logger.LogError(e,"GetJobs Execution Error");
                throw;
            }
            
        }
        public async Task<IActionResult> GetVendorsUseBapi()
        {
            try
            {
                const string COMPANY_CODE = "200";

                IVendorManager manager = _serviceProvider.GetRequiredService<IVendorManager>();
                VendorBapiOutputParameter result = await manager.GetVendorsByCompanyCodeAsync(COMPANY_CODE);
                return View(result.Vendors);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GetVendorsUseBapi Execution Error");
                throw;
            }
            
        }
        public async Task<IActionResult> GetMaterial()
        {
            try
            {
                const string MATERIAL_CODE = "1DACTV76A201000002";

                IMaterialManager manager = _serviceProvider.GetRequiredService<IMaterialManager>();
                Material result = await manager.GetMaterialWithSubTableAsync(MATERIAL_CODE);
                return View(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GetMaterial Execution Error");
                throw;
            }
           
        }
        public async Task<IActionResult> GetMaterialsByPrefixWithSubTables()
        {
            try
            {
                const string MATERIAL_PREFIX = "11AKPAK";
                const int RECORD_COUNT = 10;

                IMaterialManager manager = _serviceProvider.GetRequiredService<IMaterialManager>();
                List<Material> result = await manager.GetMaterialsByPrefixWithSubTablesAsync(MATERIAL_PREFIX, RECORD_COUNT);
                return View(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GetMaterialsByPrefixWithSubTables Execution Error");
                throw;
            }
            
        }
        public async Task<IActionResult> GetBillOfMaterials()
        {
            try
            {
                var billOfMaterialList = new List<Tuple<string, string, string>>()
                {
                    new Tuple<string, string, string>("200ALFS0304000018", "LIVE", "201"),
                    new Tuple<string, string, string>("200DYNA0102000000", "LIVE", "201"),
                    new Tuple<string, string, string>("204LMAC3K04000260", "TEST", "201"),
                    new Tuple<string, string, string>("204STBKS903000089", "TEST", "201"),
                };

                IBillOfMaterialManager manager = _serviceProvider.GetRequiredService<IBillOfMaterialManager>();
                var results = new List<BomOutputParameter>();
                await Task.WhenAll(billOfMaterialList.Select(material => Task.Run(async () =>
                {
                    BomOutputParameter result = await manager.GetBillOfMaterialAsync(material.Item1, material.Item3, material.Item2);
                    results.Add(result);
                })).ToArray()).ConfigureAwait(false);
                return View(results);

            }
            catch (Exception e)
            {
                _logger.LogError(e, "GetBillOfMaterials Execution Error");
                throw;
            }
        }
        public async Task<IActionResult> GetBillOfMaterial()
        {
            try
            {
                const string MATERIAL_CODE = "200DYNA0102000000";
                const string PLANT_CODE = "201";

                IBillOfMaterialManager manager = _serviceProvider.GetRequiredService<IBillOfMaterialManager>();
                BomOutputParameter result = await manager.GetBillOfMaterialAsync(MATERIAL_CODE, PLANT_CODE);
                return View(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GetBillOfMaterial Execution Error");
                throw;
            }
        }
        public IActionResult GetFunctionMetaData()
        {
            try
            {
                const string FUNCTION_NAME = "ZBC_GET_JOBS";

                IFunctionMetaDataManager manager = _serviceProvider.GetRequiredService<IFunctionMetaDataManager>();
                List<ParameterMetaData> metaData = manager.GetFunctionMetaData(FUNCTION_NAME);
                return View(metaData);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GetFunctionMetaData Execution Error");
                throw;
            }
        }

        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
