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
       
        public IActionResult Delete(int id)
        {
            _manager.RemoveWebpage(id);
            return RedirectToAction("Index", "Home");
        }
    }
}
