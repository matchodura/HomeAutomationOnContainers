using Entities.Enums;
using System;

namespace HomeControl.API.DTOs
{
    public class ItemDeviceDTO
    {
        public string Id { get; set; }
        public string Name { get; set; } = null!;
        public DeviceType DeviceType { get; set; }
        public string Topic { get; set; } = null!;
        public string IP { get; set; } = null!;
        public string MosquittoUsername { get; set; } = null!;
        public string MosquittoPassword { get; set; } = null!;
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public DateTime DateModified { get; set; } = DateTime.Now;
    }
}
