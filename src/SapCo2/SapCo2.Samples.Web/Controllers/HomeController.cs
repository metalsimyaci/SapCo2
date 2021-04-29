using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SapCo2.Samples.Web.Models;
using System.Diagnostics;
using System.Threading.Tasks;
using SapCo2.Samples.Web.TableExamples;

namespace SapCo2.Samples.Web.Controllers
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

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> GetMaterial()
        {
            try
            {
                string materialPrefix = "11AKPAK";
                int rowCount = 10;

                var manager = new MaterialManager(_serviceProvider);
                var result = await manager.GetMaterialsByPrefixAsync(materialPrefix, new MaterialQueryOptions { IncludeAll = true }, true, rowCount);

                return View(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
