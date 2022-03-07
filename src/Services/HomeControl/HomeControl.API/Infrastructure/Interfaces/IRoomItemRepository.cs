using HomeControl.API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeControl.API.Infrastructure.Interfaces
{
    public interface IRoomItemRepository
    {
        void AddItem(RoomItem item);
        void DeleteItem(string itemName);
        void UpdateItem(string itemName);
        Task<List<RoomItem>> GetAllItems();
        Task<RoomItem> GetItem(string itemName);
        bool ItemAlreadyExists(string itemName);
    }
}
