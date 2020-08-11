using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TravianTools.Core.Driver;

namespace TravianTools.StatCollectWorker
{
    public class StatCollectWorker : BackgroundService
    {
        private readonly ILogger<StatCollectWorker> _logger;
        private readonly TravianDriver _travianDriver;

        public StatCollectWorker(ILogger<StatCollectWorker> logger, TravianDriver travianDriver)
        {
            _logger = logger;
            _travianDriver = travianDriver;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // TODO: delete after test
            _travianDriver.Authorize();
            
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}