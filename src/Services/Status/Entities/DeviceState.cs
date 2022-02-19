namespace Status.API.Entities
{
    public class Wifi
    {
        public int AP { get; set; }
        public string SSId { get; set; } = null!;
        public string BSSId { get; set; } = null!;
        public int Channel { get; set; }
        public string Mode { get; set; } = null!;
        public int RSSI { get; set; }
        public int Signal { get; set; }
        public int LinkCount { get; set; }
        public string Downtime { get; set; } = null!;
    }

    public class State
    {
        public DateTime Time { get; set; }
        public string Uptime { get; set; } = null!;
        public int UptimeSec { get; set; }
        public int Heap { get; set; }
        public string SleepMode { get; set; } = null!;
        public int Sleep { get; set; }
        public int LoadAvg { get; set; }
        public int MqttCount { get; set; }
        public Wifi Wifi { get; set; } = null!;
    }
}
