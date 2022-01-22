using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DHT
{
    public class DHT
    {
        public int Id { get; set; }
        public string SensorName { get; set; }    
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double DewPoint { get; set; }
        public DateTime Time { get; set; }

    }

}
