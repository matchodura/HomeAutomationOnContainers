using Entities.Enums;

namespace Status.API.DTOs
{
    public class AvailableDeviceDTO
    {
        public string Name { get; set; } = null!;
        public string Topic { get; set; } = null!;
        public DeviceType DeviceType { get; set; }
        public DeviceStatus Status { get; set; }
        public DateTime LastUpdated { get; set; }
        public string Event { get; set; } = null!;
    }
}
