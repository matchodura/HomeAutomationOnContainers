using Entities.DHT;
using Logging.API.Data;
using Logging.API.Filters;
using Logging.API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logging.API.Infrastructure.Data
{
    public class SensorRepository : ISensorRepository
    {
        private readonly DataContext _context;

        public SensorRepository(DataContext context)
        {
            _context = context;
        }

        public void AddValuesForDHT(DHT dht)
        {
            _context.DHTs.Add(dht);
        }

        public async Task<List<DHT>> GetAllValues()
        {
            return await _context.DHTs.OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<List<DHT>> GetAllValuesForDht(string sensorName)
        {
            return await _context.DHTs.Where(x => x.SensorName == sensorName).ToListAsync();
        }

        public async Task<List<DHT>> GetAllValuesForSensorWithTimeSpan(string sensorName, DateFilter dateFilter)
        {
            var timeFrom = dateFilter.TimeFrom.ToUniversalTime();
            var timeTo = dateFilter.TimeTo.ToUniversalTime();

            return await _context.DHTs.Where(x => x.SensorName == sensorName &&
                        (x.Time > timeFrom && x.Time < timeTo))
                        .ToListAsync();
        }

        public async Task<string[]> GetDevicesWithValuesInDatabase()
        {
            return await _context.DHTs.Select(x => x.SensorName).Distinct().ToArrayAsync();
        }

        public async Task<DHT> GetLastValueForDht(string topic)
        {
            return await _context.DHTs.OrderByDescending(x => x.SensorName == topic).FirstAsync();
        } 
    }
}
