using Entities.DHT;
using Logging.API.Filters;
using System;
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
        Task<List<DHT>> GetAllValuesForSensorWithTimeSpan(string sensorName, DateFilter dateFilter);
        Task<string[]> GetDevicesWithValuesInDatabase();
    }
}
