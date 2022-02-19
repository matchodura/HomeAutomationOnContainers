using Entities.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace Status.API.Entities
{
    public class Device
    {
        [BsonId]
        public string? Id { get; set; }              
        public string Name { get; set; } = null!;

        [JsonConverter(typeof(StringEnumConverter))]  
        [BsonRepresentation(BsonType.String)]
        public DeviceType DeviceType { get; set; }
        public string Topic { get; set; } = null!;  
        public string IP { get; set; } = null!;
        public string MosquittoUsername { get; set; } = null!;
        public string MosquittoPassword { get; set; } = null!;
        public DateTime DateAdded { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime LastCheck{ get; set; }
        public DateTime LastAlive { get; set; }
        public State State { get; set; } = null!;

        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public DeviceStatus DeviceStatus { get; set; }

    }


    public class DeviceDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string DeviceCollectionName { get; set; } = null!;
    }
}
