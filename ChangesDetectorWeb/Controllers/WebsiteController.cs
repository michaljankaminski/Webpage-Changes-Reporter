using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ChangesDetectorWeb.Models;
using ChangesDetector;
using Microsoft.Extensions.Hosting;
using ChangesDetectorWeb.Services;

namespace ChangesDetectorWeb.Controllers
{
    public class WebsiteController : Controller
    {
        private readonly ILogger<WebsiteController> _logger;
        private readonly DetectorService _service;
        private readonly Manager _manager;

        public WebsiteController(ILogger<WebsiteController> logger, Manager manager, IHostedServiceAccessor<DetectorService> accessor)
        {
            _logger = logger;
            _manager = manager;
            _service = accessor.Service ?? throw new ArgumentNullException(nameof(accessor));
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(string name, string url, int updateFrequency)
        {
            int id = 1;
            var lastItem = _manager.GetWebpages().LastOrDefault();
            if (lastItem != null)
                id = lastItem.Id + 1;

            _manager.AddNewWebpageToReport(new ChangesDetector.model.state.SavedWebpage
            {
                Active = true,
                Name = name,
                Id = id,
                Url = url,
                LastUpdate = DateTime.Now,
                UpdateFrequency = updateFrequency
            });

            return RedirectToAction("Index", "Home");
        }
        [HttpGet("[controller]/[action]/{websiteId}")]
        public IActionResult Changes(int websiteId)
        {
            var result = _service._changes.First();
            return View(result);
        }
        public IActionResult Delete(int id)
        {
            _manager.RemoveWebpage(id);
            return RedirectToAction("Index", "Home");
        }
    }
}
