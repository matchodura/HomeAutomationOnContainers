using Logging.API.Infrastructure.Data;
using Logging.API.Infrastructure.Interfaces;
using Logging.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logging.API.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;

        public UnitOfWork(DataContext context)
        {
            _context = context;
        }

        public IMijiaRepository MijiaRepository => new MijiaRepository(_context);
        public ISensorRepository SensorRepository => new SensorRepository(_context);
        public IDeviceRepository DeviceRepository => new DeviceRepository(_context);

        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }
    }
}
