using HomeControl.Domain.AggregatesModel.RoomAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeControl.Domain.Events
{
    /// <summary>
    /// Event used when room is created 
    /// </summary>
    public class RoomCreatedDomainEvent : INotification
    {
        public Room Room { get; }

        public RoomCreatedDomainEvent(Room room)
        {
            Room = room;
        }

    }
}
