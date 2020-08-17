using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OpenQA.Selenium;
using TravianTools.Core.Driver;
using TravianTools.Core.Extensions;
using TravianTools.DAL;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace TravianTools.StatCollectWorker
{
    public class StatCollectWorker : BackgroundService
    {
        private readonly ILogger<StatCollectWorker> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ICountryInformation _countryInformation;
        private readonly StatWorkerSettings _statWorkerSettings;
        private readonly TravianDriverSettings _travianDriverSettings;

        public StatCollectWorker(ILogger<StatCollectWorker> logger,
            IServiceScopeFactory serviceScopeFactory,
            ICountryInformation countryInformation,
            IOptions<StatWorkerSettings> statWorkerSettings,
            IOptions<TravianDriverSettings> travianDriverSettings)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _countryInformation = countryInformation;
            _travianDriverSettings = travianDriverSettings.Value;
            _statWorkerSettings = statWorkerSettings.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                var origin = new Point(_statWorkerSettings.OriginX, _statWorkerSettings.OriginY);
                var nearestPoints = origin.GetNearestPoints(_statWorkerSettings.Radius);
                await GetVillagePoints(nearestPoints, origin);

                await Task.Delay(_statWorkerSettings.CheckDelay, stoppingToken);
            }
        }

        private async Task GetVillagePoints(IEnumerable<Point> points, Point origin)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ITravianToolsContext>();

                foreach (var point in points)
                {
                    if (_countryInformation.TryParseVillage(point, out NeighborsVillageInfo neighborVillage))
                    {
                        await context.NeighborsVillageInfos.AddAsync(neighborVillage);
                    }
                }

                await context.SaveChangesAsync();
            }
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            if (!_statWorkerSettings.IsEnabled)
            {
                Console.WriteLine("Service disabled.");
                return;
            }

            Console.WriteLine("I'm starting");

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ITravianToolsContext>();

                var entities = await context.Configs.ToListAsync(cancellationToken: cancellationToken);
                var cookies = entities.Select(x => JsonConvert.DeserializeObject<CookieClone>(x.Value))
                    .Select(x => new Cookie(x.Name, x.Value, x.Domain, x.Path, x.Expiry?.Date))
                    .ToList();


                if (!_countryInformation.Driver.IsLoggedIn())
                {
                    _countryInformation.Driver.Login(_travianDriverSettings.Login, _travianDriverSettings.Password, cookies);

                    foreach (var cookie in _countryInformation.Driver.Driver.Manage().Cookies.AllCookies)
                    {
                        var serialize = JsonSerializer.Serialize(cookie);
                        var c = await context.Configs.SingleOrDefaultAsync(x => x.Key == cookie.Name, cancellationToken: cancellationToken);

                        if (c != null)
                        {
                            c.Value = serialize;
                        }
                        else
                        {
                            await context.Configs.AddAsync(new StateConfig { Key = cookie.Name, Value = serialize }, cancellationToken);
                        }
                    }
                    await context.SaveChangesAsync();
                }
            }

            await base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("I'm finishing");
            return base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            _countryInformation.Driver?.Dispose();
            base.Dispose();
        }
    }

    public class CookieClone
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public string Domain { get; set; }

        public string Path { get; set; }

        public DateTime? Expiry { get; set; }

        public bool IsHttpOnly { get; set; }

        public bool Secure { get; set; }
    }
}