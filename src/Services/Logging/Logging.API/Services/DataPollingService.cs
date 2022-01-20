using AutoMapper;
using Entities.DHT22;
using Logging.API.DTOs;
using Logging.API.Interfaces;
using Logging.API.Services.MQTT;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Logging.API.Services
{
    public class DataPollingService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMqttClientService _mqttClientService;
        private Timer _timer = null!;
        private readonly IServiceScopeFactory _scopeFactory;


        public DataPollingService(ILogger logger)
        {
            _logger = logger;
        }

        //public DataPollingService(ILogger logger, IUnitOfWork unitOfWork, IServiceScopeFactory scopeFactory, MqttClientServiceProvider provider, IMapper mapper)
        //{
        //    _logger = logger;
        //    _mapper = mapper;
        //    _mqttClientService = provider.MqttClientService;
        //    _unitOfWork = unitOfWork;
        //    _scopeFactory = scopeFactory;
        //}

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.Information("Data Polling Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(15));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);

            //TODO rethink it
            //using (var scope = _scopeFactory.CreateScope())
            //{


            //    var homeRepo = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            //    string topic = "cmnd/czujnik/status";
            //    string payload = "10";

            //    _logger.Information("dupa");

            //    string subscribeTopic = "stat/czujnik/STATUS10";

            //    await _mqttClientService.SetupTopic(subscribeTopic);
            //    await _mqttClientService.PublishMessage(topic, payload);
            //    var response = _mqttClientService.GetResponse();

            //        if (!string.IsNullOrEmpty(response))
            //        {
            //            DHT22DTO serializedResponse = JsonSerializer.Deserialize<DHT22DTO>(response);

            //            var result = _mapper.Map<DHT22>(serializedResponse);

            //            result.SensorName = "testowy";

            //            //if not used - failes see-> https://github.com/npgsql/efcore.pg/issues/2000
            //            var currentDate = DateTime.UtcNow;
            //            result.Time = currentDate;

            //            homeRepo.DHTRepository.AddValuesForDHT(result);

            //            await homeRepo.Complete();
            //        }
            //}


            _logger.Information(
                "Data Polling Service is working. Count: {Count}", count);
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.Information("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
