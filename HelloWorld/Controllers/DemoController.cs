using HelloWorld.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement.Mvc;

namespace AppConfigDemo.Controllers
{
    [FeatureGate(FeatureFlag.Demo)]
    public class DemoController : Controller
    {
        private readonly ILogger<DemoController> _logger;

        public DemoController(ILogger<DemoController> logger)
        {
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

    }
}
