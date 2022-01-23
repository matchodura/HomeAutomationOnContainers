using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.DHT
{
    public class DHT
    {        
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("sensorName")]
        public string SensorName { get; set; }
        [JsonPropertyName("temperature")]
        public double Temperature { get; set; }
        [JsonPropertyName("humidity")]
        public double Humidity { get; set; }
        [JsonPropertyName("dewpoint")]
        public double DewPoint { get; set; }
        [JsonPropertyName("time")]
        public DateTime Time { get; set; }

    }

}
