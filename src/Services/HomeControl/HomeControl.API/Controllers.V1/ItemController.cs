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
        public async Task<ActionResult<ItemDeviceDTO>> GetItemByName(string deviceName)
        {
            var item = _grpcClient.GetDeviceFromStatusAPI(deviceName);

            return Ok(item);
        }

        [Route("items/all")]
        [HttpGet]
        public async Task<ActionResult<List<ItemDeviceDTO>>> GetAllItems()
        {
            var items = _grpcClient.GetAllDevicesFromStatusAPI();

            return Ok(items);
        }

        [Route("items")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ItemDTO newItem)
        {
            var roomExists = _unitOfWork.RoomRepository.RoomAlreadyExists(newItem.RoomName);

            if (roomExists != true) return BadRequest("Room doesn't exist!");

            var itemExists = _unitOfWork.RoomItemRepository.ItemAlreadyExists(newItem.DeviceName);

            if (itemExists == true) return Conflict("Item already exists!");

            //query to status service to get more data about the item            
            var itemFromStatusService = _grpcClient.GetDeviceFromStatusAPI(newItem.DeviceName);

            var itemToAdd = _mapper.Map<RoomItem>(itemFromStatusService);

            _unitOfWork.RoomItemRepository.AddItem(itemToAdd);



           // var roomToBeUpdated = await _unitOfWork.RoomRepository.GetRoom(newItem.RoomName);



          

            //_unitOfWork.RoomRepository.UpdateRoom(roomToBeUpdated);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest();
        }

    }
}
