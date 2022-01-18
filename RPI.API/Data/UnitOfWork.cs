﻿using RPI.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPI.API.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RpiDataContext _context;

        public UnitOfWork(RpiDataContext context)
        {
            _context = context;
        }

        public IMijiaRepository MijiaRepository => new MijiaRepository(_context);

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
