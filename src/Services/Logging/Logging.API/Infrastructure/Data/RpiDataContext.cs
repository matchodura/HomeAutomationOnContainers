﻿using Entities;
using Entities.BLE;
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
        public DbSet<Device> Devices { get; set; }
        public DbSet<DHT> DHTs { get; set; } 

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Device>()
                .Property(x => x.Type)
                .HasConversion<string>();
        }
    }
}
