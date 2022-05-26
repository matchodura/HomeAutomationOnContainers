using Network.API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Network.API.Infrastructure.Interfaces
{
    public interface IDeviceRepository
    {
        void AddDevice(Device device);
        void AddDevices(List<Device> devices);
        void DeleteDevice(Device device);
        void UpdateDevice(Device device);
        void UpdateDevices(List<Device> devices);

        void TruncateTable();
        Task<bool> Save();
        Task<List<Device>> GetAllDevices();
        Task<Device> GetDevice(string deviceName);
    }
}
