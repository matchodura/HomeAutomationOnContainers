using HomeControl.API.Entities;
using HomeControl.API.Entities.Enums;
using HomeControl.API.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeControl.API.Infrastructure.Data
{
    public class HomeLayoutRepository : IHomeLayoutRepository
    {
        private readonly DataContext _context;

        public HomeLayoutRepository(DataContext context)
        {
            _context = context;

        }

        public void Add<T>(T item) where T : class
        {
            _context.Add(item);
        }

        public HomeLayout Get(int level)
        {
            return _context.Layouts.FirstOrDefault(x => x.Level == (RoomLevel)level); 
        }

        public List<HomeLayout> GetAll()
        {
            return _context.Layouts.ToList();
        }
    }
}
