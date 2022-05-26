using Logging.API.Data;
using Logging.API.Entities;
using Logging.API.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
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
            var deviceToDelete = _context.Devices.First(x => x.Topic == device.Topic);
            _context.Devices.Remove(deviceToDelete);
        }

        public async Task<List<string>> GetAllAvailableTopics()
        {
            return await _context.Devices.Select(x => x.Topic).ToListAsync();
        }

        public async Task<List<AvailableDevice>> GetAllDevices()
        {
           return await _context.Devices.OrderByDescending(d => d.Id).ToListAsync();
        }

        public bool TopicAlreadyExists(string topic)
        {
            return _context.Devices.Any(x => x.Topic == topic);
        }

        public void UpdateDevice(AvailableDevice device)
        {
            var oldDevice = _context.Devices.First(x => x.Topic == device.Topic);

            oldDevice.Status = device.Status;

            oldDevice.LastUpdated = device.LastUpdated;


            _context.Devices.Update(oldDevice);
        }
    }
}
