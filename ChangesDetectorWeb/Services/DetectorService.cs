using ChangesDetector;
using ChangesDetector.model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChangesDetectorWeb.Services
{
    public class DetectorService : BackgroundService
    {
        private readonly Manager _manager;
        public IList<PageChanges> _changes { get; set; }
        public DetectorService(Manager manager)
        {
            _manager = manager;
            _changes = new List<PageChanges>();
        }
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var webpages = _manager.GetWebpages();
                foreach(var el in webpages)
                {
                    if(el.Active == true)
                    {
                        var res = _manager.CheckIfWebpageHasChanged(el.Id);
                        _changes.Add(res);
                    }
                }
                await Task.Delay(2000);
            }   
        }
    }
}
