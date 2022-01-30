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
        private DeviceAddress _deviceAddress;
        private DeviceType _deviceType;

        public int DeviceId { get; private set; }

        protected RoomItem() { }

        public RoomItem(DeviceAddress deviceAddress, int deviceId)
        {
            _deviceAddress = deviceAddress;
            DeviceId = deviceId;
        }

        public DeviceAddress GetDeviceAddress()
        {
            return _deviceAddress;
        }
    }
}
