using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Status.API.Services.MQTT;
using Status.API.Entities;

namespace Status.API.Services
{
    public class DeviceCheckService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly Serilog.ILogger _logger;
        private readonly IMapper _mapper;
        private readonly MongoDataContext _dbContext;
        private readonly IMqttClientService _mqttClientService;
        private Timer _timer = null!;
        private readonly IServiceScopeFactory _scopeFactory;

        public DeviceCheckService(Serilog.ILogger logger, IServiceScopeFactory scopeFactory, MqttClientServiceProvider provider,
            IMapper mapper, MongoDataContext dbContext)
        {
            _logger = logger;
            _mapper = mapper;
            _dbContext = dbContext;
            _mqttClientService = provider.MqttClientService;
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.Information("Data Polling Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(30));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);

            var deviceList = await _dbContext.GetAsync();

            var topics = deviceList.Select(x => x.Topic).ToArray();


            foreach (var topic in topics)
            {

                string command = $"cmnd/{topic}/state";
                string payload = String.Empty;
                string response = string.Empty;

                await _mqttClientService.PublishMessage(command, payload);

                var deviceToBeUpdated = deviceList.Single(x => x.Topic == topic);

                try
                {
                    //Wait 5 seconds so the client can update the gotten response
                    Thread.Sleep(5000);
                    response = _mqttClientService.GetResponse();

                    if (!string.IsNullOrEmpty(response))
                    {
                        var serializedResponse = JsonSerializer.Deserialize<State>(response);
                        deviceToBeUpdated.State = serializedResponse;

                        deviceToBeUpdated.LastAlive = DateTime.Now;
                        deviceToBeUpdated.DeviceStatus = DeviceStatus.Alive;
                    }
                    else
                    {
                        deviceToBeUpdated.DeviceStatus = DeviceStatus.Dead;
                        deviceToBeUpdated.State = null;
                    }



                    deviceToBeUpdated.LastCheck = DateTime.Now;
                    await _dbContext.UpdateAsync(deviceToBeUpdated.Id, deviceToBeUpdated);
                }
                catch (Exception ex)
                {
                    _logger.Error($"Error occured for topic: {topic}: {ex.Message}");
                }
            }
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
