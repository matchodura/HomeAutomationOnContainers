using Entities.Configuration;
using Logging.API.Data;
using Logging.API.Entities;
using Logging.API.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logging.API.Infrastructure.Data
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly RpiDataContext _context;

        public DeviceRepository(RpiDataContext context)
        {
            _context = context;
        }

        public void AddDevice(AvailableDevice device)
        {
            _context.Devices.Add(device);
        }

        public void DeleteDevice(AvailableDevice device)
        {
            _context.Devices.Remove(device);
        }

        public async Task<List<AvailableDevice>> GetAllDevices()
        {
           return await _context.Devices.OrderByDescending(d => d.Id).ToListAsync();
        }
    }
}
