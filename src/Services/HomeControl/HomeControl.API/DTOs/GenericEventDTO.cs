using Common.Enums;

namespace HomeControl.API.DTOs
{
    public class GenericEventDTO
    {
        public string Event { get; set; }
        public DeviceType DeviceType { get; set; }
    }
}
