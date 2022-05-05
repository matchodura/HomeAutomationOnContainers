using HardwareStatus.API.Enums;
using System;

namespace HardwareStatus.API.Entities
{
    public class Device
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public HardwareType HardwareType { get; set; }
        public string HostName { get; set; } = null!;
        public string IP { get; set; } = null!;
        public string MAC { get; set; } = null!;
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
        public DateTime DateModified { get; set; } = DateTime.UtcNow;
        public DateTime LastCheck { get; set; } = DateTime.UtcNow;
        public DateTime LastAlive { get; set; } = DateTime.UtcNow;
        public DeviceStatus DeviceStatus { get; set; }
    }
}
