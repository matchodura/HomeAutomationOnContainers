using Status.API.DTOs;

namespace Status.API.Services.RabbitMQ
{
    public interface IMessageBusClient
    {
        void UpdateAvailableDevice(AvailableDeviceDTO availableDeviceDTO);
    }
}
