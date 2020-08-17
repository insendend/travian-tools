using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TravianTools.Core.Driver;
using TravianTools.DAL;

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
                    services.AddDbContext<TravianToolsContext>(o => o.UseSqlite("Data Source=TravianTools.db"));
                    services.AddScoped<ITravianToolsContext, TravianToolsContext>();
                    services.Configure<TravianDriverSettings>(o => cfg.GetSection("Driver").Bind(o));
                    services.Configure<StatWorkerSettings>(o => cfg.GetSection("StatWorkerSettings").Bind(o));
                    services.AddTransient<ITravianDriver, TravianDriver>();
                    services.AddTransient<ICountryInformation, CountryInformation>();
                    services.AddHostedService<StatCollectWorker>();
                });
    }
}