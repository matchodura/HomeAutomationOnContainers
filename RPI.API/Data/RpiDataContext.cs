using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPI.API.Data
{
    public class RpiDataContext : DbContext
    {

        public RpiDataContext(DbContextOptions<RpiDataContext> options) : base(options)
        {
        }

        public DbSet<Mijia> Mijias { get; set; }
    }
}
