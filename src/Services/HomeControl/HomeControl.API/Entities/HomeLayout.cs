using HomeControl.API.Entities.Enums;
using System;

namespace HomeControl.API.Entities
{
    public class HomeLayout
    {            
        public int Id { get; set; }
        public RoomLevel Level { get; set; }

        public byte[] File { get; set; }

        public DateTime LastModified { get; set; }
    }
}
