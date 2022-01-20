﻿using Entities.DHT22;
using Logging.API.Data;
using Logging.API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logging.API.Infrastructure.Data
{
    public class DHTRepository : IDHTRepository
    {
        private readonly RpiDataContext _context;

        public DHTRepository(RpiDataContext context)
        {
            _context = context;
        }

        public void AddValuesForDHT(DHT22 dht)
        {
            _context.DHTs.Add(dht);
        }

        public async Task<IEnumerable<DHT22>> GetAllValuesForDht(string sensorName)
        {
            return await _context.DHTs.Where(x => x.SensorName == sensorName).ToListAsync();
        }

    }
}
