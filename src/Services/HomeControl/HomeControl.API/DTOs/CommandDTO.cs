namespace HomeControl.API.DTOs
{
    public class CommandDTO
    {
        public int RoomId { get; set; }
        public string Topic { get; set; }
        public string Command { get; set; }
    }
}
