﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeControl.Domain.SeedWork
{
   public interface IRepository<T> where T : IAggregateRoot
    {
    }
}
