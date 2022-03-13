using HomeControl.API.Entities;
using HomeControl.API.Infrastructure.Interfaces;
using System.Collections.Generic;
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
            throw new System.NotImplementedException();
        }

        public void DeleteValue(RoomValue value)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<RoomValue>> GetAllValues()
        {
            throw new System.NotImplementedException();
        }

        public Task<RoomValue> GetValue(string roomName)
        {
            throw new System.NotImplementedException();
        }

        public bool RoomItemExists(string topic)
        {
            throw new System.NotImplementedException();
        }

        public bool RoomValueAlreadyExists(string roomName)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateValue(RoomValue rovalueom)
        {
            throw new System.NotImplementedException();
        }
    }
}
