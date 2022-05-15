using HomeControl.API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeControl.API.Infrastructure.Interfaces
{
    public interface IRoomItemRepository
    {
        void AddItem(RoomItem item);
        void DeleteItem(string itemName);
        void UpdateItem(RoomItem item);
        Task<List<RoomItem>> GetAllItems();
        Task<List<RoomItem>> GetAllItemsByRoomId(int roomID);
        IEnumerable<RoomItem> GetAllSensors();
        Task<RoomItem> GetItem(string itemName);
        bool ItemExists(string itemName);
    }
}
