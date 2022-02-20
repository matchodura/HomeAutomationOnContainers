using Entities.Configuration;
using Logging.API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logging.API.Infrastructure.Interfaces
{
    public interface IDeviceRepository
    {
        void AddDevice(AvailableDevice device);
        void DeleteDevice(AvailableDevice device);
        Task<List<AvailableDevice>> GetAllDevices();
        bool TopicAlreadyExists(string topic);
        void UpdateDevice(AvailableDevice device);
        Task<List<string>> GetAllAvailableTopics();
    }
}
