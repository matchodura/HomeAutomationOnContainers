using Common.Enums;
using Network.API.Entities;

namespace Network.API.DTOs
{
    public class AddDeviceDTO
    {
        public string Name { get; set; } = null!;
        public string HostName { get; set; } = null!;
        public HardwareType HardwareType { get; set; }
        public string IP { get; set; } = null!;
        public string MAC { get; set; } = null!;
    }
}
