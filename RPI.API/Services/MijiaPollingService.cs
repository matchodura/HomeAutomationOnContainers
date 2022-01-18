using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RaspberryPI.API.Utilities;
using RPI.API.Interfaces;
using SensorLogging.API.Data;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RPI.API.Services
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

        private void CheckConfirmedGracePeriodOrders()
        {
            _logger.Debug("Checking confirmed grace period orders");

            string scriptPath = Constants.MIJIA_SCRIPT_PATH +
                        Constants.CLI_DELIMITER +
                        "4:C1:38:48:31:DF" +
                        Constants.CLI_DELIMITER +
                        "test";

            string pythonResult = string.Empty;
            try
            {
                _logger.Information($"Running python script at path: {scriptPath}");
                pythonResult = await SciptRunner.RunPythonScript(scriptPath);

            }
            catch (Exception ex)
            {
                _logger.Error("An error occurred. {ErrorMessage} - {StackTrace}", ex.Message, ex.StackTrace);
                return BadRequest("Error occured!");
            }

            if (string.IsNullOrEmpty(pythonResult))
            {
                _logger.Error("Failed obtaining results from running python script!");

                return BadRequest("Failed getting python results!");
            }


            var result = JsonSerializer.Deserialize<Mijia>(pythonResult);


            _unitOfWork.MijiaRepository.AddValuesForMijia(result);

            if (await _unitOfWork.Complete()) return Ok(result);


        }
               


    }
}
