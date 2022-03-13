using Entities.DHT;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logging.API.Interfaces
{
    public interface ISensorRepository
    {
        void AddValuesForDHT(DHT dht);
        Task<List<DHT>> GetAllValuesForDht(string sensorName);
        Task<List<DHT>> GetAllValues();
        Task<DHT> GetLastValueForDht(string topic);
    }
}
