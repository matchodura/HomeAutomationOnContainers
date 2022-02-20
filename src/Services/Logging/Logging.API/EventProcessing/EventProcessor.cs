using AutoMapper;
using Logging.API.DTOs;
using Logging.API.Entities;
using Logging.API.Infrastructure.Interfaces;
using Logging.API.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Text.Json;

namespace Logging.API.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper, ILogger logger)
        {
            _mapper = mapper;
            _logger = logger;
            _scopeFactory = scopeFactory;

        }
        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.DeviceAlive:
                    AddDevice(message);
                    break;
                case EventType.DeviceDead:
                    RemoveDevice(message);
                    break;
                default:
                    break;
            }
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            Console.Write("--> Determining Event");

            var eventType = JsonSerializer.Deserialize<GenericEventDTO>(notificationMessage);

            switch (eventType.Event)
            {
                case "Device_Alive":
                    Console.Write("--> Device Alive event Detected");
                    return EventType.DeviceAlive;
                case "Device_Dead":
                    Console.Write("--> Device Dead event Detected");
                    return EventType.DeviceDead;
                default:
                    Console.WriteLine("--> Could not determine event type");
                    return EventType.Undetermined;
            }
        }

        private void AddDevice(string statusCheckPublishedMessage)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var repo = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    var availableDevice = JsonSerializer.Deserialize<AvailableDevice>(statusCheckPublishedMessage);
                    var currentDate = DateTime.UtcNow;
                    availableDevice.LastUpdated = currentDate;

                    try
                    {

                        if (!repo.DeviceRepository.TopicAlreadyExists(availableDevice.Topic))
                        {
                            repo.DeviceRepository.AddDevice(availableDevice);
                            repo.Complete();

                            _logger.ForContext("Status", availableDevice.Status)
                                    .ForContext("LastUpdated", availableDevice.LastUpdated)
                                    .Information(
                                         "Data Polling Event Processor is working. Added sensor at {topic}", availableDevice.Topic);
                        }
                        else
                        {
                            repo.DeviceRepository.UpdateDevice(availableDevice);
                            repo.Complete();

                            _logger.ForContext("Status", availableDevice.Status)
                                .ForContext("LastUpdated", availableDevice.LastUpdated)
                                .Information(
                                     "Data Polling Event Processor is working. Updated sensor at {topic}", availableDevice.Topic);

                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"--> Could not add Device to DB: {ex.Message}");
                    }

                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

           
        }

        private void RemoveDevice(string statusCheckPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                var availableDevice = JsonSerializer.Deserialize<AvailableDevice>(statusCheckPublishedMessage);
                var currentDate = DateTime.UtcNow;
                availableDevice.LastUpdated = currentDate;

                try
                {
                    if (!repo.DeviceRepository.TopicAlreadyExists(availableDevice.Topic))
                    {                       
                        Console.Write("--> Device doesn't exist, nothing to delete!");
                    }
                    else
                    {
                        repo.DeviceRepository.DeleteDevice(availableDevice);
                        repo.Complete();
                        _logger.ForContext("Status", availableDevice.Status)
                            .ForContext("LastUpdated", availableDevice.LastUpdated)
                            .Information(
                                 "Data Polling Event Processor is working. Deleted sensor at {topic}", availableDevice.Topic);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not delete device from DB: {ex.Message}");
                }

            }
        }
    }

    enum EventType
    {
        DeviceAlive,
        DeviceDead,
        Undetermined
    }
}
