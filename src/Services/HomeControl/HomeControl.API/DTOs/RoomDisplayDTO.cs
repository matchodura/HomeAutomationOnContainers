using HomeControl.API.Entities.Enums;
using System;

namespace HomeControl.API.DTOs
{
    public class RoomDisplayDTO
    {
        public string Name { get; set; }
        public RoomLevel Level { get; set; }
        public RoomType RoomType { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double DewPoint { get; set; }
        public int AliveDevicesCount { get; set; }
        public DateTime LastPolled { get; set; }
        public DateTime LastModified { get; set; }
    }
}
