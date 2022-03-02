using HomeControl.API.Entities.Enums;

namespace HomeControl.API.DTOs
{
    public class RoomDTO
    {
        public string Name { get; set; }
        public RoomLevel Level { get; set; }
        public RoomType RoomType { get; set; }
        public string Topic { get; set; }
    }
}
