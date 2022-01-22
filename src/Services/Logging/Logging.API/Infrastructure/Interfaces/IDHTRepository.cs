using Entities.DHT;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logging.API.Interfaces
{
    public interface IDHTRepository
    {
        void AddValuesForDHT(DHT dht);
        Task<IEnumerable<DHT>> GetAllValuesForDht(string sensorName);
    }
}
