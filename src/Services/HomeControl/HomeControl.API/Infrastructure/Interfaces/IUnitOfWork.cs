using HomeControl.API.Infrastructure.Interfaces;
using Logging.API.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logging.API.Interfaces
{
    public interface IUnitOfWork
    {
        IRoomRepository RoomRepository { get; }
        IRoomItemRepository RoomItemRepository { get; }
        IRoomValueRepository RoomValueRepository { get; }

        Task<bool> Complete();
        bool HasChanges();
    }
}
