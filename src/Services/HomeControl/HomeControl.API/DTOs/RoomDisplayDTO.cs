using HomeControl.API.DTOs.LoggingAPI;
using HomeControl.API.Entities.Enums;
using System;

namespace HomeControl.API.DTOs
{
    public class RoomDisplayDTO
    {
        public RoomDTO Room { get; set; }
        public SensorValueDTO Sensor { get; set; }
    }
}
