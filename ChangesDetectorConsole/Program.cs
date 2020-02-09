using System;
using System.Threading.Tasks;
using ChangesDetector;
using ChangesDetectorConsole.service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChangesDetectorConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Manager mn = new Manager();
            mn.Test();
            Console.WriteLine("Finished");

            var host = new HostBuilder()
                 .ConfigureServices((hostContext, services) =>
                 {
                     services.AddScoped<Manager>();
                     services.AddHostedService<DetectorService>();
                 })
                .Build();

            await host.RunAsync();

            
        }
    }
}
