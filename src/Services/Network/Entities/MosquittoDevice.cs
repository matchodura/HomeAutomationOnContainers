using Common.Enums;

namespace Network.API.Entities
{
    public class MosquittoDevice
    { 
        public int Id { get; set; }
        public string Topic { get; set; } = null!;
        public string MosquittoUsername { get; set; } = null!;
        public string MosquittoPassword { get; set; } = null!;
        public DateTime DateAdded { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime LastCheck { get; set; }
        public DateTime LastAlive { get; set; }
        public string State { get; set; } = null!;//as JSON TODO
        public DeviceType DeviceType { get; set; }
    }
}
