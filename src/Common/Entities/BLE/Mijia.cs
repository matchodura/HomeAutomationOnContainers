using System;
using System.Text.Json.Serialization;

namespace Entities
{
    public class Mijia
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("temperature")]
        public float Temperature { get; set; }

        [JsonPropertyName("humidity")]
        public float Humidity { get; set; }

        [JsonPropertyName("voltage")]
        public float Voltage { get; set; }

        [JsonPropertyName("battery")]
        public int Battery { get; set; }

        [JsonPropertyName("sensor")]
        public string SensorName { get; set; }

        [JsonPropertyName("address")]
        public string MacAddress { get; set; }

        [JsonPropertyName("timestamp")]
        public double TimestampLinux { get; set; }
    }
}
