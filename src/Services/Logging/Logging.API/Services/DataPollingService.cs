using AutoMapper;
using Entities.DHT;
using Logging.API.Data;
using Logging.API.DTOs;
using Logging.API.Profiles;
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
        private readonly IMapper _mapper;
        private readonly IMqttClientService _mqttClientService;
        private Timer _timer = null!;
        private readonly IServiceScopeFactory _scopeFactory;

        public DataPollingService(ILogger logger, IServiceScopeFactory scopeFactory, MqttClientServiceProvider provider, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
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
            //to do query from db for stability and maintability

            string deviceType = "dht";
            string[] tasmotaNames = new string[2] { "AM2301", "AM2301" };
            string tasmotaName = "AM2301";
            string[] rooms = new string[2] { "pokoj", "strych" };

            //await _mqttClientService.SetupSubscriptionTopic(subscriptionTopic);



            // foreach (var (name, index) in rooms.Select((value, i) => (value, i)))
            
            foreach (var name in rooms)
            {
                
                string commandTopic = $"cmnd/{name}/{deviceType}/status";
                string payload = "10";
                string subscriptionTopic = $"stat/{name}/{deviceType}/STATUS10";
                string response = string.Empty;
          
                await _mqttClientService.PublishMessage(commandTopic, payload);

                //Wait 5 seconds so the client can update the gotten response
                Thread.Sleep(5000);
                response = _mqttClientService.GetResponse();

                if (!string.IsNullOrEmpty(response))
                {
                    if (response.Contains(@"DHT11"))
                    {
                        response = response.Replace(@"DHT11", @"Values");
                    }

                    if (response.Contains(@"AM2301"))
                    {
                        response = response.Replace(@"AM2301", @"Values");
                    }

                    DHTDTO serializedResponse = JsonSerializer.Deserialize<DHTDTO>(response);

                    var result = _mapper.Map<DHT>(serializedResponse);

                    result.SensorName = $"{name}/{deviceType}";

                    //link to issue-> https://github.com/npgsql/efcore.pg/issues/2000
                    var currentDate = DateTime.UtcNow;
                    result.Time = currentDate;


                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetService<IUnitOfWork>();


                        context.DHTRepository.AddValuesForDHT(result);

                        await context.Complete();


                    }

                    _logger.ForContext("Sensor", result.SensorName)
                        .ForContext("Temperature", result.Temperature)
                        .ForContext("Humidity", result.Humidity)
                        .ForContext("DewPoint", result.DewPoint)
                        .ForContext("Time", result.Time)
                        .Information(
                             "Data Polling Service is working. Polled sensor {name}", name);
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
