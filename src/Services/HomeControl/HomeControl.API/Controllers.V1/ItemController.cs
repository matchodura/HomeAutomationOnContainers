using Microsoft.AspNetCore.Mvc;
using HomeControl.API.Controllers.v1;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeControl.API.DTOs;
using HomeControl.API.Interfaces;
using AutoMapper;
using HomeControl.API.Entities;
using HomeControl.API.SyncDataServices.Grpc;

namespace HomeControl.API.Controllers
{
    public class ItemController : BaseApiController
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IGrpcClient _grpcClient;

        public ItemController(ILogger logger, IUnitOfWork unitOfWork, IMapper mapper, IGrpcClient grpcClient)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _grpcClient = grpcClient;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }

        //[Route("items/{id}")]
        //[HttpGet]
        //public async Task<ActionResult<ItemDTO>> Get(string deviceName)
        //{
        //    var item = await _unitOfWork.RoomItemRepository.GetItem(deviceName);

        //    if (item == null) return NotFound("Room with that name does not exist!");

        //    var resultToDisplay = _mapper.Map<RoomDTO>(room);

        //    return Ok(resultToDisplay);
        //}

        [Route("test")]
        [HttpGet]
        public async Task<IActionResult> GetAllItems(string deviceName)
        {
            var test = _grpcClient.GetDeviceFromStatusAPI(deviceName);

            //if (rooms.Count == 0) return NotFound("There are no configured rooms!");

            //var resultsToDisplay = _mapper.Map<RoomDTO>(rooms);

            //return Ok(resultsToDisplay);
            return Ok();
        }

        //[Route("items/all")]
        //[HttpGet]
        //public async Task<ActionResult<List<ItemDeviceDTO>>> GetAllItems()
        //{
        //    var rooms = await _unitOfWork.RoomRepository.GetAllRooms();

        //    if (rooms.Count == 0) return NotFound("There are no configured rooms!");

        //    var resultsToDisplay = _mapper.Map<RoomDTO>(rooms);

        //    return Ok(resultsToDisplay);
        //}

        //[Route("items")]
        //[HttpPost]
        //public async Task<IActionResult> Create([FromBody] ItemDTO newItem)
        //{
        //    var roomExists = _unitOfWork.RoomItemRepository.ItemAlreadyExists(newItem.DeviceID);

        //    if (roomExists == true) return Conflict("Item already exists!");

        //    //query to status service to get more data about the item

        //    var item = _mapper.Map<RoomItem>(itemFromStatusService);

        //    var currentDate = DateTime.UtcNow;
        //    room.LastModified  = currentDate;

        //    _unitOfWork.RoomRepository.AddRoom(room);

        //    if (await _unitOfWork.Complete()) return CreatedAtAction(nameof(Get), new { roomName = room.Name }, room);

        //    return BadRequest();
        //}



    }
}
