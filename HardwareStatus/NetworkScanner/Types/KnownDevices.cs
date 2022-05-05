using HardwareStatus.API.Enums;
using System;

namespace HardwareStatus.API.NetworkScanner.Types
{
    public class KnownDevices
    {
        public string IP { get; set; }
        public DateTime TimeOfScan { get; set; }
        public Enums.DeviceStatus Status { get; set; }
    }
}
