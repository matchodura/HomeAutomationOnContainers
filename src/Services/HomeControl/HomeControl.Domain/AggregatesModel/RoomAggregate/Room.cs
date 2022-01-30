using HomeControl.Domain.Events;
using HomeControl.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeControl.Domain.AggregatesModel.RoomAggregate
{
    public class Room : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        public int RoomID { get; private set; }

        private bool _isDraft;

        private readonly List<RoomItem> _roomItems;
        public IReadOnlyCollection<RoomItem> RoomItems => _roomItems;

        public RoomType RoomType { get; private set; }
        private int _roomTypeId;



        public static Room NewDraft()
        {
            var room = new Room();
            room._isDraft = true;
            return room;
        }

        protected Room()
        {            
            _roomItems = new List<RoomItem>();
            _isDraft = false;
        }

        public Room(string name, int roomType) : this()
        {
            Name = name;
            _roomTypeId = RoomType.
            AddRoomCreatedDomainEvent();
        }


        public void AddRoomItems(int deviceId)
        {
            var roomItem = new RoomItem(deviceId);

            _roomItems.Add(roomItem);
        }

        private void AddRoomCreatedDomainEvent()
        {
            var orderStartedDomainEvent = new RoomCreatedDomainEvent(this);

            this.AddDomainEvent(orderStartedDomainEvent);
        }
    }
}
