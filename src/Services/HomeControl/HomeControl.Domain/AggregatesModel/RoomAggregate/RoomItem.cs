using HomeControl.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeControl.Domain.AggregatesModel.RoomAggregate
{
    public class RoomItem : Entity
    {
        public int DeviceId { get; private set; }

        protected RoomItem() { }

        public RoomItem(int deviceId)
        {
            DeviceId = deviceId;
        }
    }
}
