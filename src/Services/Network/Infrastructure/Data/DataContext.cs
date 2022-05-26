using Microsoft.EntityFrameworkCore;
using Network.API.Entities;

namespace Network.API.Infrastructure.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Device> Devices { get; set; }
        public DbSet<MosquittoDevice> MosquittoDevices { get; set; }
    }
}
