using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logging.API.Interfaces
{
    public interface IUnitOfWork
    {
        IMijiaRepository MijiaRepository { get; }

        IDHTRepository DHTRepository { get; }

        Task<bool> Complete();

        bool HasChanges();

    }
}
