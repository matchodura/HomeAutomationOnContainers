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
            return await _context.Devices.ToListAsync();
        }

        public async Task<Device> GetDevice(string roomName)
        {
            return await _context.Devices.FirstOrDefaultAsync(x => x.Name == roomName);
        }

        public void UpdateDevice(Device room)
        {
            var oldDevice = _context.Devices.First(x => x.Name == room.Name);

            _context.Devices.Update(oldDevice);
        }
    }
}
