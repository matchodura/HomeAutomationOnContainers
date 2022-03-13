using HomeControl.API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeControl.API.Infrastructure.Interfaces
{
    public interface IRoomValueRepository
    {
        void AddValue(RoomValue value);
        void DeleteValue(RoomValue value);
        void UpdateValue(RoomValue rovalueom);
        Task<List<RoomValue>> GetAllValues();
        Task<RoomValue> GetValue(string roomName);
        bool RoomValueAlreadyExists(string roomName);

        bool RoomItemExists(string topic);

    }
}
