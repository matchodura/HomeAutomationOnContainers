using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Configuration
{
    public class Device
    {    
        public int Id { get; set; }
        public string Room { get; set; }
        public string FriendlyName { get; set; }
        public string TasmotaDevice { get; set; }
        public DeviceFunction Function { get; set; }
        public string IPAddress { get; set; }
        public string MosquittoUsername { get; set; }
        public string MosquittoPassword { get; set; }
        public DateTime DateModified { get; set; }
    }
}
