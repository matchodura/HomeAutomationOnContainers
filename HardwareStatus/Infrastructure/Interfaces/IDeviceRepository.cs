using HardwareStatus.API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HardwareStatus.API.Infrastructure.Interfaces
{
    public interface IDeviceRepository
    {
        void AddDevice(Device device);
        void AddDevices(List<Device> devices);
        void DeleteDevice(Device device);
        void UpdateDevice(Device device);
        Task<List<Device>> GetAllDevices();
        Task<Device> GetDevice(string deviceName);
    }
}
