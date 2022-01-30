using HomeControl.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeControl.Domain.AggregatesModel.DeviceAggregate
{
    public interface IDeviceRepository : IRepository<Device>
    {
        Device Add(Device device);

        void Update(Device device);

        Task<Device> GetCurrentValuesById(int deviceId);
    }
}
