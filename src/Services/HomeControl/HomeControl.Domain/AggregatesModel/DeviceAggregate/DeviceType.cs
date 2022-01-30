using HomeControl.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeControl.Domain.AggregatesModel.RoomAggregate
{
    public class DeviceType : Enumeration
    {
        public static DeviceType Sensor = new DeviceType(1, nameof(Sensor).ToLowerInvariant());
        public static DeviceType Actuator = new DeviceType(2, nameof(Actuator).ToLowerInvariant());

        public DeviceType(int id, string name) : base(id, name)
        {
        }
    }
}
