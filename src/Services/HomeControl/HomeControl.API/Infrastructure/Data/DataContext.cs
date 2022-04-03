using HomeControl.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomeControl.API.Infrastructure.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomItem> Items { get; set; }
        public DbSet<RoomValue> Values { get; set; }
        public DbSet<HomeLayout> Layouts { get; set; }
    }
}
