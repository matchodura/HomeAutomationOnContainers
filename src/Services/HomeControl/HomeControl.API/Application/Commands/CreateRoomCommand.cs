using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HomeControl.API.Application.Commands
{
    [DataContract]
    public class CreateRoomCommand : IRequest<bool>
    {
        private readonly List<RoomItemDTO> _roomItems;

        [DataMember]
        public IEnumerable<RoomItemDTO> RoomItems => _roomItems;

        [DataMember]
        public string RoomName { get; private set; }

        [DataMember]
        public int DeviceType { get; private set; }

        public CreateRoomCommand()
        {
            _roomItems = new List<RoomItemDTO>();

        }

        public CreateRoomCommand(List<RoomItemDTO> roomItems) : this()
        {
            _roomItems = roomItems;

        }

        public record RoomItemDTO
        {
            public int DeviceTypeId { get; init; }
            public string Name { get; init; }
            public string IP { get; init; }
            public string Topic { get; init; }
            public string MosquittoUsername { get; init; }
            public string MosquittoPassword { get; init; }

        }
    }
}
