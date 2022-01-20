﻿using Newtonsoft.Json;
using System;

namespace Logging.API.DTOs
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AM2301
    {
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double DewPoint { get; set; }
    }

    public class StatusSNS
    {
        public DateTime Time { get; set; }
        public AM2301 AM2301 { get; set; }
        public string TempUnit { get; set; }
    }

    public class DHT22
    {
        public StatusSNS StatusSNS { get; set; }
    }
}
