using Entities.Enums;
using System;
using System.Text.Json.Serialization;

namespace Logging.API.DTOs
{
    public class DeviceDTO
    {
        [JsonPropertyName("room")]
        public string Room { get; set; }

        [JsonPropertyName("name")]
        public string FriendlyName { get; set; }

        [JsonPropertyName("tasmotaName")]
        public string TasmotaDevice { get; set; }

        [JsonPropertyName("function")]
        public DeviceFunction Function { get; set; }

        [JsonPropertyName("ip")]
        public string IPAddress { get; set; }

        [JsonPropertyName("mqttusername")]
        public string MosquittoUsername { get; set; }

        [JsonPropertyName("mqttpassword")]
        public string MosquittoPassword { get; set; }
    }
}
