using HardwareStatus.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace HardwareStatus.API.Infrastructure.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Device> Devices { get; set; }
    }
}
