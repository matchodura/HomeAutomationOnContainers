using HomeControl.API.Entities;
using System.Collections.Generic;

namespace HomeControl.API.Infrastructure.Interfaces
{
    public interface IHomeLayoutRepository : IRepository
    {
        HomeLayout Get(int level);
        List<HomeLayout> GetAll();
    }
}
