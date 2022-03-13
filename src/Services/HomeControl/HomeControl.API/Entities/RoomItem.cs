using Entities.Enums;
using System;
using System.Collections.Generic;

namespace HomeControl.API.Entities
{
    public class RoomItem
    {
        public int Id { get; set; }
        public int RoomId { get; set;}
        public string Name { get; set; }
        public int DeviceId { get; set; }//deviceId in the mongoDb under Status.API
        public string Topic { get; set; }
        public DeviceType DeviceType { get; set; }
        public DeviceStatus DeviceStatus { get; set; }
        public DateTime LastChecked { get; set; }
    }
}
