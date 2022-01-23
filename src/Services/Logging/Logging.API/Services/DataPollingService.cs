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


        //public DataPollingService(ILogger logger)
        //{
        //    _logger = logger;
        //}

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
                TimeSpan.FromSeconds(60));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);

            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<IUnitOfWork>();

                string[] sensorNames = new string[2] {"czujnik_1", "czujnik_2"};
                string[] tasmotaNames = new string[2] { "DHT11", "AM2301" };

               
                foreach (var (name, index) in sensorNames.Select((value, i) => (value,i)))
                {
                    string commandTopic = $"cmnd/pokoj/{name}/status";
                    string payload = "10";
                    string subscriptionTopic = $"stat/pokoj/{name}/STATUS10";
                    string response = string.Empty;
                    await _mqttClientService.SetupSubscriptionTopic(subscriptionTopic);
                    int i = 0;

                    do
                    {                      

                        await _mqttClientService.PublishMessage(commandTopic, payload);
                        response = _mqttClientService.GetResponse();

                        if (string.IsNullOrEmpty(response)) continue;
                        i++;
                        if (response.Contains(tasmotaNames[index])) break;

                    } while (i > 10);

                    //if we didnt receive data for specified sensor, move to the next one
                    if (i > 10) continue;

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

                        result.SensorName = name;

                        //link to issue-> https://github.com/npgsql/efcore.pg/issues/2000
                        var currentDate = DateTime.UtcNow;
                        result.Time = currentDate;
                       
                        context.DHTRepository.AddValuesForDHT(result);

                        if(await context.Complete())
                        {
                            continue;
                        };
                                            

                        _logger.Information(
                                 "Data Polling Service is working. Polled sensor {name}", name);
                    }                                   
                
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
