using HardwareStatus.API.Entities;
using HardwareStatus.API.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HardwareStatus.API.Infrastructure.Data
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly DataContext _context;

        public DeviceRepository(DataContext context)
        {
            _context = context;
        }

        public void AddDevice(Device device)
        {
            _context.Devices.Add(device);
        }

        public void AddDevices(List<Device> devices)
        {
            _context.Devices.AddRangeAsync(devices);
        }

        public void DeleteDevice(Device device)
        {
            var roomToBeDeleted = _context.Devices.First(x => x.Name == device.Name);

            _context.Devices.Remove(roomToBeDeleted);
        }

        public async Task<List<Device>> GetAllDevices()
        {
            return await _context.Devices.AsNoTracking().ToListAsync();
        }

        public async Task<Device> GetDevice(string hostName)
        {
            return await _context.Devices.FirstOrDefaultAsync(x => x.HostName == hostName);
        }

        public async Task<bool> Save()
        {
             return await _context.SaveChangesAsync() > 0; 
        }

        public void TruncateTable()
        {
            var devicesToDelete = _context.Devices.ToList();
            _context.Devices.RemoveRange(devicesToDelete);
        }

        public void UpdateDevice(Device device)
        {
            var deviceToUpdate = _context.Devices.First(x => x.HostName == device.HostName);

            _context.Devices.Update(deviceToUpdate);
        }

        public void UpdateDevices(List<Device> devices)
        {
            foreach (var dev in devices)
            {
                _context.Devices.Update(dev);
            }
        }
    }
}
