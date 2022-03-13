using HomeControl.API.Infrastructure.Interfaces;
using HomeControl.API.Interfaces;
using System.Threading.Tasks;

namespace HomeControl.API.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;

        public UnitOfWork(DataContext context)
        {
            _context = context;
        }

        public IRoomRepository RoomRepository => new RoomRepository(_context);

        public IRoomItemRepository RoomItemRepository => new RoomItemRepository(_context);

        public IRoomValueRepository RoomValueRepository => new RoomValueRepository(_context);

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
