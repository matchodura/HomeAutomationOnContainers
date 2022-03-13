using HomeControl.API.Entities;
using HomeControl.API.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeControl.API.Infrastructure.Data
{
    public class RoomValueRepository : IRoomValueRepository
    {
        private readonly DataContext _context;

        public RoomValueRepository(DataContext context)
        {
            _context = context;

        }

        public void AddValue(RoomValue value)
        {
            _context.Values.Add(value);
        }

        public void DeleteValue(RoomValue value)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<RoomValue>> GetAllValues()
        {
            throw new System.NotImplementedException();
        }

        public RoomValue GetValue(int roomId)
        {
            return  _context.Values.FirstOrDefault(x => x.RoomId == roomId);
        }

        public bool RoomItemExists(string topic)
        {
            return _context.Values.Any(x => x.Topic.Equals(topic));
        }

        public bool RoomValueAlreadyExists(string roomName)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateValue(RoomValue roomValue)
        {
            var oldValue = _context.Values.First(x => x.RoomId == roomValue.RoomId);

            _context.Values.Update(oldValue);
        }
    }
}
