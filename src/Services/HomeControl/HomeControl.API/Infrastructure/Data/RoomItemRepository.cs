using Common.Enums;
using HomeControl.API.Entities;
using HomeControl.API.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeControl.API.Infrastructure.Data
{
    public class RoomItemRepository : IRoomItemRepository
    {
        private readonly DataContext _context;

        public RoomItemRepository(DataContext context)
        {
            _context = context;
        }

        public void AddItem(RoomItem item)
        {
            _context.Items.Add(item);
        }

        public void DeleteItem(string itemName)
        {
            var itemToBeDeleted = _context.Items.First(x => x.Name == itemName);

            _context.Items.Remove(itemToBeDeleted);
        }

        public async Task<List<RoomItem>> GetAllItems()
        {
            return await _context.Items.ToListAsync();
        }

        public async Task<List<RoomItem>> GetAllItemsByRoomId(int roomID)
        {
            return await _context.Items.Where(x => x.RoomId == roomID).ToListAsync();
        }

        public IEnumerable<RoomItem> GetAllSensors()
        {
            return _context.Items.Where(x => x.DeviceType == DeviceType.Sensor).ToList();
        }

        public async Task<RoomItem> GetItem(string itemName)
        {
            return await _context.Items.FirstOrDefaultAsync(x => x.Name == itemName);
        }

        public bool ItemExists(string itemName)
        {
            return _context.Items.Any(x => x.Name.Equals(itemName));
        }

        public void UpdateItem(RoomItem item)
        {
            _context.Items.Update(item);
        }
    }
}
