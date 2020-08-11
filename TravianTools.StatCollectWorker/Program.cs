using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TravianTools.Core.Driver;

namespace TravianTools.StatCollectWorker
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                await CreateHostBuilder(args).Build().RunAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostContext, services) =>
                {
                    services.AddJsonFile("appsettings-secrets.json");
                })
                .ConfigureServices((hostContext, services) =>
                {
                    var cfg = hostContext.Configuration;
                    services.Configure<TravianDriverSettings>(o => cfg.GetSection("Driver").Bind(o));
                    services.AddTransient<TravianDriver>();
                    services.AddHostedService<StatCollectWorker>();
                });
    }
}