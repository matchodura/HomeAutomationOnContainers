using Entities;
using Microsoft.EntityFrameworkCore;
using Logging.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logging.API.Extensions;
using Entities.BLE;

namespace Logging.API.Data
{
    public class MijiaRepository : IMijiaRepository
    {
        private readonly RpiDataContext _context;

        public MijiaRepository(RpiDataContext context)
        {
            _context = context;
        }

        //public void AddDevice(BLEDevice device)
        //{
        //    _context.BLEDevices.Add(device);  
        //}

        public void AddValuesForMijia(Mijia mijia)
        {
            _context.Mijias.Add(mijia);
        }

        public async Task<IEnumerable<Mijia>> GetAllValuesForMijia(string sensorName)
        {
            return await _context.Mijias.Where(x => x.SensorName == sensorName).ToListAsync();
        }
    }
}
