using Newtonsoft.Json;
using System;

namespace Logging.API.DTOs
{    
    public class StatusSNS
    {
        public DateTime Time { get; set; }
        public Values Values { get; set; }  
        public string TempUnit { get; set; }
    }

    public class DHTDTO
    {
        public StatusSNS StatusSNS { get; set; }
    }

    public class Values
    {
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double DewPoint { get; set; }
    }
}
