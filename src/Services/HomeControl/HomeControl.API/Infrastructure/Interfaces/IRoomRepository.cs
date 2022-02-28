using HomeControl.API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeControl.API.Infrastructure.Interfaces
{
    public interface IRoomRepository
    {
        void AddRoom(Room room);
        void DeleteRoom(Room room);
        void UpdateRoom(Room room);
        Task<List<Room>> GetAllRooms();
        Task<Room> GetRoom(string roomName);
        bool RoomAlreadyExists(string roomName);
    }
}
