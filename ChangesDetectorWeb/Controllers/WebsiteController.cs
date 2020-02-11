using ChangesDetector;
using ChangesDetectorWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

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
        public IActionResult Add(string name, string url, int updateFrequency, bool active)
        {
            int id = 0;
            var lastItem = _manager.GetWebpages().OrderByDescending(a => a.Id).FirstOrDefault();
            if (lastItem != null)
                id = lastItem.Id + 1;

            _manager.AddNewWebpageToReport(new ChangesDetector.model.state.SavedWebpage
            {
                Active = active,
                Name = name,
                Id = id,
                Url = url,
                LastUpdate = DateTime.Now,
                UpdateFrequency = updateFrequency
            });

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("[controller]/[action]/{websiteId}/")]
        public IActionResult Changes(int websiteId)
        {

            var result = _service._changes.Where(c => c.WebsiteId == websiteId).SingleOrDefault();
            if (result != null)
                return View(result);
            else
                return View();
        }

        public IActionResult Diff(int websiteId, int index)
        {
            var result = _service._changes.Where(c => c.WebsiteId == websiteId).SingleOrDefault();
            var temp = result.Changes.ElementAt(index - 1);
            return View(temp);
        }

        public IActionResult Delete(int id)
        {
            _manager.RemoveWebpage(id);
            return RedirectToAction("Index", "Home");
        }
    }
}
