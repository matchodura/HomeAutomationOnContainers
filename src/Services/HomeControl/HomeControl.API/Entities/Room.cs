using HomeControl.API.Entities.Enums;
using System;
using System.Collections.Generic;

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
        public List<RoomItem> RoomItem { get; set; }
    }
}
