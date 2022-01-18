using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SensorLogging.API.Infrasctructure.Services
{
    public class DataPollingService : BackgroundService
    {
        private int executionCount = 0;
        private readonly ILogger _logger;
        private Timer _timer = null!;

        public DataPollingService(ILogger logger)
        {
            _logger = logger;
        }

        //public override async Task ExecuteAsync(CancellationToken stoppingToken)
        //{
        //    _logger.Information("Timed Hosted Service running.");

        //    _timer = new Timer(DoWork, null, TimeSpan.Zero,
        //        TimeSpan.FromSeconds(5));

        //}

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.Information("DataPollingService is starting.");

            stoppingToken.Register(() => _logger.Information("#1 DataPollingService background task is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.Information("DataPollingService background task is doing background work.");

                //CheckConfirmedGracePeriodOrders();

                await Task.Delay(5000, stoppingToken);
            }

            _logger.Information("DataPollingService background task is stopping.");
        }


        //private void DoWork(object? state)
        //{
        //    var count = Interlocked.Increment(ref executionCount);

        //    _logger.Information(
        //        "Timed Hosted Service is working. Count: {Count}", count);
        //}

        //public async Task StopAsync(CancellationToken stoppingToken)
        //{
        //    _logger.Information("Timed Hosted Service is stopping.");

        //    _timer?.Change(Timeout.Infinite, 0);

        //     await Task.CompletedTask;
        //}

    }
}
