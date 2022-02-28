using Entities.Enums;
using HomeControl.API.Entities.Enums;
using System;

namespace HomeControl.API.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }    
        public int Level { get; set; }
        public RoomType RoomType { get; set; }
        public string Topic { get; set; }     
        public DateTime LastModified { get; set; }
        public RoomItem Item { get; set; }
        public RoomValue Value { get; set; }        
    }
}
