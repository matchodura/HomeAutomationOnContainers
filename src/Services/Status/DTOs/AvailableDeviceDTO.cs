using Entities.Enums;

namespace Status.API.DTOs
{
    public class AvailableDeviceDTO
    {
        public string Topic { get; set; } = null!;
        public DeviceStatus Status { get; set; }
        public DateTime LastUpdated { get; set; }
        public string Event { get; set; } = null!;
    }
}
