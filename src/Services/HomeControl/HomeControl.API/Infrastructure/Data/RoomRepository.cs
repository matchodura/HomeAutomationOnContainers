using HomeControl.API.Entities;
using HomeControl.API.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeControl.API.Infrastructure.Data
{
    public class RoomRepository : IRoomRepository
    {
        private readonly DataContext _context;

        public RoomRepository(DataContext context)
        {
            _context = context;
        }

        public void AddRoom(Room room)
        {
            _context.Rooms.Add(room);
        }

        public void DeleteRoom(string roomName)
        {
            var roomToBeDeleted = _context.Rooms.First(x => x.Name == roomName);

            _context.Rooms.Remove(roomToBeDeleted);
        }

        public async Task<List<Room>> GetAllRooms()
        {
            return await _context.Rooms.Include(x => x.RoomValue).ToListAsync();
        }

        public async Task<Room> GetRoom(string roomName)
        {
            return await _context.Rooms.Include(x => x.RoomValue).FirstOrDefaultAsync(x => x.Name == roomName);
        }

        public bool RoomAlreadyExists(string roomName)
        {
            return _context.Rooms.Any(x => x.Name.Equals(roomName));
        }

        public void UpdateRoom(Room room)
        {
            var oldRoom = _context.Rooms.First(x => x.Name == room.Name);

            _context.Rooms.Update(oldRoom);
        }
    }
}
