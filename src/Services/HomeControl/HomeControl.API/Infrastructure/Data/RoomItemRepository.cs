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

        public async Task<RoomItem> GetItem(string itemName)
        {
            return await _context.Items.FirstOrDefaultAsync(x => x.Name == itemName);
        }

        public bool ItemAlreadyExists(string itemName)
        {
            return _context.Items.Any(x => x.DeviceId.Equals(itemName));
        }

        public void UpdateItem(string itemName)
        {
            var oldItem = _context.Items.First(x => x.Name == itemName);

            _context.Items.Update(oldItem);
        }
    }
}
