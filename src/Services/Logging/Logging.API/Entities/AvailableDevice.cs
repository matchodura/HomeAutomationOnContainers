using Entities.Enums;
using System;

namespace Logging.API.Entities
{
    public class AvailableDevice
    {
        public int Id { get; set; }
        public string Topic { get; set; }
        public DeviceStatus Status { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
