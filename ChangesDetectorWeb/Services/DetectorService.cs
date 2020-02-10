using ChangesDetector;
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
        public DetectorService(Manager manager)
        {
            _manager = manager;
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
                        _manager.CheckIfWebpageHasChanged(el);
                        Debug.WriteLine(el.Name);
                    }
                }
                await Task.Delay(1000);
            }   
        }
    }
}
