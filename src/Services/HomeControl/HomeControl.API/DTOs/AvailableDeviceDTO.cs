using Entities.Enums;
using System;

namespace HomeControl.API.Entities
{
    public class AvailableDeviceDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Topic { get; set; }
        public DeviceStatus Status { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
