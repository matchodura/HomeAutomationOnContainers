using HomeControl.API.Entities.Enums;

namespace HomeControl.API.DTOs
{
    public class HomeLayoutDTO
    {
        public int Id { get; set; }
        public RoomLevel Level { get; set; }
        public string Layout { get; set; }
    }
}
