using MediatR;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HomeControl.API.Application.Commands
{
    //public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, bool>
    //{
    //    private readonly IMediator _mediator;
    //    private readonly IRoomRepository _roomRepository;
    //    private readonly ILogger _logger;

    //    public CreateRoomCommandHandler(IMediator mediator, IRoomRepository roomRepository, ILogger logger)
    //    {
    //        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    //        _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
    //        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    //    }

        //public Task<bool> Handle(CreateRoomCommand message, CancellationToken cancellationToken)
        //{
        //    var room = new Room(message.RoomName);

        //    foreach(var item in message.RoomItems)
        //    {
        //        var deviceAddress = new DeviceAddress(item.Name, item.IP, item.Topic, item.MosquittoUsername, item.MosquittoPassword);

        //        room.AddRoomItems(deviceAddress, message.DeviceType);
        //    }

        //    _roomRepository.Add(room);

        //    return await _orderRepository.
        //}

        


    
}
