using ChangesDetector;
using ChangesDetectorWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ChangesDetectorWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Manager _manager;

        public HomeController(ILogger<HomeController> logger, Manager manager)
        {
            _logger = logger;
            _manager = manager;
        }

        public IActionResult Index()
        {
            ViewData["webpages"] = _manager.GetWebpages();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
