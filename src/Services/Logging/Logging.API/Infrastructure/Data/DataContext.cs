using Entities;
using Entities.BLE;
using Entities.DHT;
using Logging.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logging.API.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Mijia> Mijias { get; set; }
        public DbSet<DHT> DHTs { get; set; } 
        public DbSet<AvailableDevice> Devices { get; set; } 

    }
}
