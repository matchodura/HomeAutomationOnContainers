using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Logging.API.Utilities;
using Logging.API.Interfaces;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;
using HomeControl.API.Data;
using Entities;

namespace Logging.API.Services
{
    public class MijiaPollingService : BackgroundService
    {
        private int executionCount = 0;
        private readonly BackgroundTaskSettings _settings;
        private readonly ILogger _logger;
        private Timer _timer = null!;
        private readonly IUnitOfWork _unitOfWork;

        //TODO ladnie napisac polling
        public MijiaPollingService(IOptions<BackgroundTaskSettings> settings, ILogger logger, IUnitOfWork unitOfWork)
        {
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.Debug("MijiaPollingService is starting.");

            stoppingToken.Register(() => _logger.Debug("#1 MijiaPollingService background task is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.Debug("MijiaPollingService background task is doing background work.");



                await Task.Delay(_settings.CheckUpdateTime, stoppingToken);
            }

            _logger.Debug("MijiaPollingService background task is stopping.");
        }     
    }
}
