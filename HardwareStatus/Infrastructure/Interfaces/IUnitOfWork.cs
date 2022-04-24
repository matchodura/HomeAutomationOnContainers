using HardwareStatus.API.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HardwareStatus.API.Infrastructure.Interfaces
{
    public interface IUnitOfWork
    {
        IDeviceRepository DeviceRepository { get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}
