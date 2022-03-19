using System;
using System.Collections.Generic;

namespace Logging.API.DTOs
{
    public class SensorLoggingDTO
    {
        public string SensorName { get; set; }
        public string Topic { get; set; }
        public List<LoggingValues> Values { get; set; }

    }

    public class LoggingValues
    {
        public DateTime Date { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double DewPoint { get; set; }
    }
}
