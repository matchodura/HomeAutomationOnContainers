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
using Common.Enums;
using Network.API.Entities;
using Network.API.Services.RabbitMQ;
using Network.API.DTOs;
using Network.API.Services.MQTT;



namespace Network.API.Services
{
    public class DeviceCheckService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly Serilog.ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IMessageBusClient _messageBusClient;
        private readonly IMqttClientService _mqttClientService;
        private Timer _timer = null!;

        public DeviceCheckService(Serilog.ILogger logger, MqttClientServiceProvider provider,
            IMapper mapper, IMessageBusClient messageBusClient)
        {
            _logger = logger;
            _mapper = mapper;           
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
            //var count = Interlocked.Increment(ref executionCount);

            ////var deviceList = await _dbContext.GetAsync();

            //var topics = deviceList.Select(x => x.Topic).ToArray();


            //foreach (var topic in topics)
            //{

            //    string command = $"cmnd/{topic}/state";
            //    string payload = string.Empty;
            //    string response = string.Empty;

            //    try
            //    {
            //        await _mqttClientService.PublishMessage(command, payload);

            //    }
            //    catch (Exception ex)
            //    {
            //        _logger.ForContext("Topic", topic)
            //            .Error($"Error occured for topic: {topic}: {ex.Message}");
            //    }

            //    //var deviceToBeUpdated = deviceList.Single(x => x.Topic == topic);

            //    try
            //    {
            //        //Wait 5 seconds so the client can update the gotten response
            //        Thread.Sleep(5000);
            //        response = _mqttClientService.GetResponse();
            //        _mqttClientService.CleanResponse();
            //        //TODO: double check this, seems not to be working correctly-> after another item is connected(previously disconnected)
            //        //then the item before in the array of topics is being polled and marked as alive
               
            //        if (!string.IsNullOrEmpty(response))
            //        {
            //            var serializedResponse = JsonSerializer.Deserialize<State>(response);

            //            deviceToBeUpdated.State = serializedResponse;
            //            deviceToBeUpdated.LastAlive = DateTime.Now;
            //            deviceToBeUpdated.DeviceStatus = DeviceStatus.Online;

            //        }
            //        else
            //        {
            //            deviceToBeUpdated.DeviceStatus = DeviceStatus.Offline;
            //            deviceToBeUpdated.State = null;

            //            deviceToBeUpdated.LastCheck = DateTime.Now;
            //            await _dbContext.UpdateAsync(deviceToBeUpdated.Id, deviceToBeUpdated);

            //            _logger.ForContext("Name", deviceToBeUpdated.Name)
            //                    .ForContext("Topic", deviceToBeUpdated.Topic)
            //                    .ForContext("IP", deviceToBeUpdated.IP)
            //                    .ForContext("LastCheck", deviceToBeUpdated.LastAlive)
            //                    .ForContext("Status", deviceToBeUpdated.DeviceStatus)
            //                    .Warning($"Warning occured for topic: {topic}: Device {deviceToBeUpdated.Name} is not reachable!");

            //            try
            //            {
            //                var availableDevice = _mapper.Map<AvailableDeviceDTO>(deviceToBeUpdated);

            //                if (deviceToBeUpdated.DeviceStatus == DeviceStatus.Online)
            //                {
            //                    availableDevice.Event = "Device_Alive";
            //                    availableDevice.Status = DeviceStatus.Online;
            //                }
            //                else
            //                {
            //                    availableDevice.Event = "Device_Dead";
            //                    availableDevice.Status = DeviceStatus.Offline;
            //                }

            //                availableDevice.LastUpdated = DateTime.Now;
            //                _messageBusClient.UpdateAvailableDevice(availableDevice);

            //                _logger.ForContext("Name", deviceToBeUpdated.Name)
            //                        .ForContext("Status", deviceToBeUpdated.DeviceStatus)
            //                        .Information(
            //                             "Device Status Service is working. Updating message bus topic {topic}", topic);
            //            }
            //            catch (Exception ex)
            //            {
            //                Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
            //            }

            //            continue;
            //        }



            //        deviceToBeUpdated.LastCheck = DateTime.Now;
            //        await _dbContext.UpdateAsync(deviceToBeUpdated.Id, deviceToBeUpdated);

            //        _logger.ForContext("Name", deviceToBeUpdated.Name)
            //                .ForContext("IP", deviceToBeUpdated.IP)
            //                .ForContext("LastCheck", deviceToBeUpdated.LastCheck)
            //                .ForContext("Status", deviceToBeUpdated.DeviceStatus)
            //                .Information(
            //                     "Device Status Service is working. Checked topic {topic}", topic);




            //        if (deviceToBeUpdated.DeviceType == DeviceType.Switch)
            //        {
            //            //send to queue that the device has been updated and is alive
            //            //data logging api should get that value and update it's own database
            //            //after that it nows that this sensor can be polled so the sensor is alive
            //            //a bit complicated, but what would you do to learn some microservices?

            //            try
            //            {
            //                var availableDevice = _mapper.Map<AvailableDeviceDTO>(deviceToBeUpdated);

            //                if (deviceToBeUpdated.DeviceStatus == DeviceStatus.Online)
            //                {
            //                    availableDevice.Event = "Device_Alive";
            //                    availableDevice.Status = DeviceStatus.Online;
            //                }
            //                else
            //                {
            //                    availableDevice.Event = "Device_Dead";
            //                    availableDevice.Status = DeviceStatus.Offline;
            //                }

            //                availableDevice.DeviceType = deviceToBeUpdated.DeviceType;
            //                availableDevice.LastUpdated = DateTime.Now;
            //                availableDevice.Name = deviceToBeUpdated.Name;
            //                _messageBusClient.UpdateAvailableDevice(availableDevice);

            //                _logger.ForContext("Name", deviceToBeUpdated.Name)
            //                        .ForContext("Type", deviceToBeUpdated.DeviceType)
            //                        .ForContext("Status", deviceToBeUpdated.DeviceStatus)
            //                        .Information(
            //                             "Device Status Service is working. Updating message bus topic {topic}", topic);
            //            }
            //            catch (Exception ex)
            //            {
            //                Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
            //            }

            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        _logger.ForContext("Name", deviceToBeUpdated.Name)
            //            .ForContext("Topic", deviceToBeUpdated.Topic)
            //            .ForContext("IP", deviceToBeUpdated.IP)
            //            .ForContext("LastCheck", deviceToBeUpdated.LastAlive)
            //            .Error($"Error occured for topic: {topic}: {ex.Message}");
            //    }
            //}
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
