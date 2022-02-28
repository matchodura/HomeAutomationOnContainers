using HomeControl.API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeControl.API.Infrastructure.Interfaces
{
    public interface IRoomItemRepository
    {
        void AddItem(RoomItem item);
        void DeleteItem(RoomItem item);
        void UpdateItem(RoomItem item);
        Task<List<RoomItem>> GetAllItems();
        Task<RoomItem> GetItem(string roomName);
        bool ItemAlreadyExists(string roomName);
    }
}
