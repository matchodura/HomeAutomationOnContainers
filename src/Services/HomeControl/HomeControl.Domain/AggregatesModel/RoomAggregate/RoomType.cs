using HomeControl.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeControl.Domain.AggregatesModel.RoomAggregate
{
    public class RoomType : Enumeration
    {
        public static RoomType Kitchen = new(1, nameof(Kitchen));
        public static RoomType Bedroom = new(2, nameof(Bedroom));
        public static RoomType Utility = new(3, nameof(Utility));

        public RoomType(int id, string name) : base(id, name)
        {
        }
    }
}
