﻿using Entities.Enums;
using System;
using System.Collections.Generic;

namespace HomeControl.API.Entities
{
    public class RoomItem
    {
        public int Id { get; set; }
        public int RoomId { get; set;}
        public string Topic { get; set; }
        public DeviceType DeviceType { get; set; }
        public DeviceStatus DeviceStatus { get; set; }
        public DateTime LastChecked { get; set; }
        public ICollection<Room> Rooms { get; set; }
    }
}
