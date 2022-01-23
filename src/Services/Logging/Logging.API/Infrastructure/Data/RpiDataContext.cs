using Entities;
using Entities.BLE;
using Entities.Configuration;
using Entities.DHT;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logging.API.Data
{
    public class RpiDataContext : DbContext
    {

        public RpiDataContext(DbContextOptions<RpiDataContext> options) : base(options)
        {
        }

        public DbSet<Mijia> Mijias { get; set; }
       // public DbSet<BLEDevice> BLEDevices { get; set; }
        public DbSet<DHT> DHTs { get; set; } 
        public DbSet<Device> Devices { get; set; } 

        //protected override void OnModelCreating(ModelBuilder builder)
        //{

        //}
    }
}
