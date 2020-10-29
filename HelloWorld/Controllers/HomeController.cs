using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using System.Threading.Tasks;
using HelloWorld.Models;

namespace AppConfigDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFeatureManager _featureManager;

        public HomeController(ILogger<HomeController> logger, IFeatureManager featureManager)
        {
            this._logger = logger;
            this._featureManager = featureManager;
        }

        public async Task<IActionResult> Index()
        {
            var demoFeatureEnabled = await this._featureManager.IsEnabledAsync(nameof(FeatureFlag.Demo));
            var messageAboutFeature = demoFeatureEnabled ? "Demo feature is enabled" : "Demo feature is disabled";

            var model = new DemoViewModel
            {
                Message = messageAboutFeature
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
