using ChangesDetector;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChangesDetectorConsole.service
{
    class DetectorService : BackgroundService
    {
        private readonly Manager _manager;
        public DetectorService(Manager manager)
        {
            _manager = manager;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _manager.Test2();
                await Task.Delay(5000,stoppingToken);
            }
        }
    }
}
