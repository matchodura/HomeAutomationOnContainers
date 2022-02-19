using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeControl.Infrastructure.Interfaces
{
    public interface IUnitOfWork
    {
        Task<bool> Complete();
        bool HasChanges();
    }
}

