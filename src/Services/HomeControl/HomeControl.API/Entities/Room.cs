using Entities.Enums;
using HomeControl.API.Entities.Enums;
using System;

namespace HomeControl.API.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }    
        public RoomLevel Level { get; set; }
        public RoomType RoomType { get; set; }
        public string FrontendID { get; set; }
        public string Topic { get; set; }     
        public int AliveDevicesCount { get; set; }
        public DateTime LastModified { get; set; }    
        public RoomValue RoomValue { get; set; }
    }
}
