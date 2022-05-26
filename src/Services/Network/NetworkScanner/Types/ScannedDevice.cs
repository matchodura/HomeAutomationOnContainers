using System;

namespace Network.API.NetworkScanner.Types
{
    public class ScannedDevice
    {
        public string IP { get; set; }
        public string MAC { get;set; }
        public string HostName { get;set;}
        public DateTime TimeOfScan { get; set; }
    }
}
