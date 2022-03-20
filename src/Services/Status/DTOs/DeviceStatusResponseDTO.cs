namespace Status.API.DTOs
{
    public class DeviceStatusResponseDTO
    {
        public string Name { get; set; } = null!;
        public string Topic { get; set; } = null!;
        public string Ip { get; set; } = null!; 
        public string Status { get; set; } = null!;
        public DateTime LastAlive { get; set; }
        public int UptimeInSeconds { get; set; }
    }
}
