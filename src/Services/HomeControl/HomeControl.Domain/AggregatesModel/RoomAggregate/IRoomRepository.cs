using HomeControl.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeControl.Domain.AggregatesModel.RoomAggregate
{
    public interface IRoomRepository : IRepository<Room>
    {
        Room Add(Room room);

        void Update(Room room);

        Task<Room> GetByIdAsync(int roomId);
    }
}
