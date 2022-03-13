using System;

namespace HomeControl.API.DTOs.LoggingAPI
{
    public class SensorValueDTO
    {
        public string Topic { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double DewPoint { get; set; }
        public DateTime TimePolled { get; set; }
    }
}
