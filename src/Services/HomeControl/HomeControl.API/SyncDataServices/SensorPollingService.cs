using AutoMapper;
using HomeControl.API.Entities;
using HomeControl.API.Interfaces;
using HomeControl.API.SyncDataServices.Grpc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace HomeControl.API.Services
{
    public class SensorPollingService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private Timer _timer = null!;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IGrpcClient _grpcClient;

        public SensorPollingService(ILogger logger, IServiceScopeFactory scopeFactory, IMapper mapper, IGrpcClient grpcClient)
        {
            _logger = logger;
            _mapper = mapper;
            _grpcClient = grpcClient;
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.Information("Data Polling Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(45));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);

            List<string> topics = new List<string>();

            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<IUnitOfWork>();
                    var sensorsToPoll = context.RoomItemRepository.GetAllSensors();

                    foreach(var sensor in sensorsToPoll)
                    {
                        if (!context.RoomValueRepository.RoomItemExists(sensor.Topic))
                        {
                            var roomValue = new RoomValue()
                            {
                                RoomId = sensor.RoomId,
                                Topic = sensor.Topic
                            };

                            context.RoomValueRepository.AddValue(roomValue);
                            await context.Complete();
                        }

                        else
                        {
                            //var response = _grpcClient.ReturnLastSensorValue(sensor.Topic);

                            //var valueToUpdate = context.RoomValueRepository.GetValue(sensor.RoomId);

                            //valueToUpdate.Temperature = response.Temperature;
                            //valueToUpdate.Humidity = response.Humidity;
                            //valueToUpdate.DewPoint = response.DewPoint;

                            //var currentDate = DateTime.UtcNow;
                            //valueToUpdate.LastModified = currentDate;
                            
                            //context.RoomValueRepository.UpdateValue(valueToUpdate);
                            //await context.Complete();
                        }

                    }

                }


            }
            catch (Exception ex)
            {
                _logger.Error($"Something wrong {ex.Message}");
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
