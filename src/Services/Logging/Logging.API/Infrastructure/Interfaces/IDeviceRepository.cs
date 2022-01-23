using Entities.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logging.API.Infrastructure.Interfaces
{
    public interface IDeviceRepository
    {
        void AddDevice(Device device);
        Task<List<Device>> GetAllConfiguredDevices();
        //TODO maybe check if device is online?
    }
}
