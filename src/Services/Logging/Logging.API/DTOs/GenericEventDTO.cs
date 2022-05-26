using Common.Enums;

namespace Logging.API.DTOs
{
    public class GenericEventDTO
    {
        public string Event { get; set; }
        public DeviceType DeviceType { get; set; }
    }
}
