using Entities.DHT22;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logging.API.Interfaces
{
    public interface IDHTRepository
    {
        void AddValuesForDHT(DHT22 dht);
        Task<IEnumerable<DHT22>> GetAllValuesForDht(string sensorName);
    }
}
