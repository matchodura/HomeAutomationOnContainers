using Common.Enums;
using Network.API.Entities;
using System;

namespace Network.API.Entities
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
        public bool IsMosquitto { get; set; }
        public int? MosquittoDeviceID { get; set; }
        public virtual MosquittoDevice MosquittoDevice { get; set; } = null!;

    }
}
