using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ChangesDetectorWeb.Models;
using ChangesDetector;

namespace ChangesDetectorWeb.Controllers
{
    public class WebsiteController : Controller
    {
        private readonly ILogger<WebsiteController> _logger;
        private readonly Manager _manager;

        public WebsiteController(ILogger<WebsiteController> logger, Manager manager)
        {
            _logger = logger;
            _manager = manager;
        }
       
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(string webpageName, string webpageUrl)
        {
            //_manager.AddNewWebpageToReport(string webpageName, string webpageUrl);
            return RedirectToAction("Index", "Home");
        }
    }
}
