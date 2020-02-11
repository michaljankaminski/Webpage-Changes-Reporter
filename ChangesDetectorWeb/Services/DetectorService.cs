using ChangesDetector;
using ChangesDetector.model;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChangesDetectorWeb.Services
{
    public class DetectorService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private int Time = 10000;
        public IList<PageChanges> _changes { get; set; }

        public DetectorService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            _changes = new List<PageChanges>();
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<Manager>();
                    var webpages = context.GetWebpages();

                    if (webpages.Count() > 0)
                    {
                        foreach (var el in webpages)
                        {
                            if (el.Active == true)
                            {
                                var res = context.CheckIfWebpageHasChanged(el.Id);
                                if(res != null)
                                {
                                    res.WebsiteId = el.Id;
                                    
                                    var temp = _changes.FirstOrDefault(t => t.WebsiteId == el.Id);
                                    if (temp != null)
                                        _changes.Remove(temp);

                                    _changes.Add(res);
                                    context.UpdateWebpage(el.Id);
                                }
                            }
                        }
                    }
                    var first = webpages.FirstOrDefault();
                    if(first!=null)
                        Time = first.UpdateFrequency*1000;
                }
                await Task.Delay(Time);
            }
        }
    }
}
