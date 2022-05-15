using AutoMapper;
using Entities.Enums;
using HomeControl.API.DTOs;
using HomeControl.API.Entities;
using HomeControl.API.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace HomeControl.API.EventProcessing
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
             UpdateDevice(message);    
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

        private async Task UpdateDevice(string message)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                var eventType = DetermineEvent(message);

                var availableDevice = JsonSerializer.Deserialize<AvailableDeviceDTO>(message);

                switch (eventType)
                {
                    case EventType.DeviceAlive:
                        availableDevice.Status = DeviceStatus.Alive;
                        break;
                    case EventType.DeviceDead:
                        availableDevice.Status = DeviceStatus.Dead;
                        break;
                    default:
                        break;
                }                          

                try
                {
                    if (!repo.RoomItemRepository.ItemExists(availableDevice.Name))
                    {
                        Console.Write("--> Item doesn't exist, nothing to update!");
                    }
                    else
                    {
                        var deviceToUpdate = await repo.RoomItemRepository.GetItem(availableDevice.Name);
                        deviceToUpdate.DeviceStatus = availableDevice.Status;
                        var currentDate = DateTime.UtcNow;
                        deviceToUpdate.LastChecked = currentDate;
                        deviceToUpdate.Status = availableDevice.Status.ToString();
                        repo.RoomItemRepository.UpdateItem(deviceToUpdate);

                        await repo.Complete();
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
        Undetermined,
        Unsupported
    }
}
