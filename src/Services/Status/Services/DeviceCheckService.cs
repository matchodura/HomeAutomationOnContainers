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
using Entities.Enums;
using Status.API.Entities;
using Status.API.Services.RabbitMQ;
using Status.API.DTOs;

namespace Status.API.Services
{
    public class DeviceCheckService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly Serilog.ILogger _logger;
        private readonly IMapper _mapper;
        private readonly MongoDataContext _dbContext;
        private readonly IMessageBusClient _messageBusClient;
        private readonly IMqttClientService _mqttClientService;
        private Timer _timer = null!;

        public DeviceCheckService(Serilog.ILogger logger, MqttClientServiceProvider provider,
            IMapper mapper, MongoDataContext dbContext, IMessageBusClient messageBusClient)
        {
            _logger = logger;
            _mapper = mapper;
            _dbContext = dbContext;
            _messageBusClient = messageBusClient;
            _mqttClientService = provider.MqttClientService;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.Information("Device Status Service running.");

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

                try
                {
                    await _mqttClientService.PublishMessage(command, payload);

                }
                catch (Exception ex)
                {
                    _logger.ForContext("Topic", topic)
                        .Error($"Error occured for topic: {topic}: {ex.Message}");
                }

                var deviceToBeUpdated = deviceList.Single(x => x.Topic == topic);

                try
                {
                    //Wait 5 seconds so the client can update the gotten response
                    Thread.Sleep(5000);
                    response = _mqttClientService.GetResponse();
                    var serializedResponse = JsonSerializer.Deserialize<State>(response);

                    if (!string.IsNullOrEmpty(response))
                    {

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

                    _logger.ForContext("Name", deviceToBeUpdated.Name)
                            .ForContext("IP", deviceToBeUpdated.IP)
                            .ForContext("LastCheck", deviceToBeUpdated.LastCheck)
                            .ForContext("Status", deviceToBeUpdated.DeviceStatus)
                            .Information(
                                 "Device Status Service is working. Checked topic {topic}", topic);


                    if(deviceToBeUpdated.DeviceType == DeviceType.Sensor)
                    {
                        //send to queue that the device has been updated and is alive
                        //data logging api should get that value and update it's own database
                        //after that it nows that this sensor can be polled so the sensor is alive
                        //a bit complicated, but what would you do to learn some microservices?

                        try
                        {
                            var availableDevice = _mapper.Map<AvailableDeviceDTO>(deviceToBeUpdated);

                            _messageBusClient.UpdateAvailableDevice(availableDevice);

                            _logger.ForContext("Name", deviceToBeUpdated.Name)
                                    .ForContext("Status", deviceToBeUpdated.DeviceStatus)
                                    .Information(
                                         "Device Status Service is working. Updating message bus topic {topic}", topic);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");

                        }

                    }

                }
                catch (Exception ex)
                {
                    _logger.ForContext("Name", deviceToBeUpdated.Name)
                        .ForContext("Topic", deviceToBeUpdated.Topic)
                        .ForContext("IP", deviceToBeUpdated.IP)
                        .ForContext("LastCheck", deviceToBeUpdated.LastAlive)
                        .Error($"Error occured for topic: {topic}: {ex.Message}");
                }
            }
        }
                
        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.Information("Device Status Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
