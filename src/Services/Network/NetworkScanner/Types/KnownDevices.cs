using Common.Enums;
using System;

namespace Network.API.NetworkScanner.Types
{
    public class KnownDevices
    {
        public string IP { get; set; } = null!;
        public DateTime TimeOfScan { get; set; }
        public DeviceStatus Status { get; set; }
    }
}
