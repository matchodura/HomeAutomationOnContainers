using Network.API.DTOs;

namespace Network.API.Services.RabbitMQ
{
    public interface IMessageBusClient
    {
        void UpdateAvailableDevice(AvailableDeviceDTO availableDeviceDTO);
    }
}
