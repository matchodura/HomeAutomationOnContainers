using Common.Enums;

namespace Network.API.DTOs
{
    public class AddMosquittoDeviceDTO
    {
        public string Name { get; set; } = null!;
        public string Topic { get; set; } = null!;
        public string Username {  get; set; } = null!;
        public string Password { get; set; } = null!;
        public DeviceType DeviceType { get; set; }
    }
}
