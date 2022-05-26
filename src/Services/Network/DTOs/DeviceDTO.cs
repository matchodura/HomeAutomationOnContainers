using Common.Enums;
using Network.API.Entities;

namespace Network.API.DTOs
{
    public class DeviceDTO
    {
        public string Name { get; set; } = null!;
        public string HostName { get; set; } = null!;
        public DeviceType DeviceType { get; set; }
        public string Topic { get; set; } = null!;
        public string IP { get; set; } = null!;
        public string MAC { get; set; } = null!;

        public string MosquittoUsername { get; set; } = null!;
        public string MosquittoPassword { get; set; } = null!;
    }
}
