using Common.Enums;
using System;

namespace Logging.API.Entities
{
    public class Device
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Topic { get; set; } = null!;
        public DeviceStatus Status { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

    }
}
