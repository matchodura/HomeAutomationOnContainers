using System;
using System.Collections.Generic;

namespace HomeControl.API.Entities
{
    public class RoomValue
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string Topic { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double DewPoint { get; set; }
        public DateTime LastModified { get; set; }
    }
}
