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
using Common.Enums;
using System.Net;

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
        [Route("{roomId}")]
        public async Task<ActionResult<List<RoomItem>>> GetItemsByRoom(int roomId)
        {
            var items = await _unitOfWork.RoomItemRepository.GetAllItemsByRoomId(roomId);

            if (!items.Any()) return NotFound("Room doesn't have any items configured!");

            return Ok(items);
        }


        [HttpGet]
        [Route("all-items")]
        public async Task<ActionResult<List<RoomItemDTO>>> GetAllItems()
        {
            var items = await _unitOfWork.RoomItemRepository.GetAllItems();

            if (!items.Any()) return NotFound("No Items were configured!");


            var itemsToDisplay = _mapper.Map<List<RoomItemDTO>>(items);
            return Ok(itemsToDisplay);
        }


        [HttpGet]
        [Route("status/{deviceName}")]
        [ProducesResponseType(typeof(ItemDeviceDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ItemDeviceDTO>> GetItemByNameFromStatus(string deviceName)
        {
            var item = _grpcClient.GetDeviceFromStatusAPI(deviceName);

            if(item == null) return NotFound("Item doesn't exist!");

            return Ok(item);
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(List<ItemDeviceDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<List<ItemDeviceDTO>>> GetAllItemsFromStatus()
        {
            var items = _grpcClient.GetAllDevicesFromStatusAPI();

            if(items == null) return NotFound("Devices were not found!");

            return Ok(items);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Create([FromBody] ItemDTO newItem)
        {
            var roomExists = _unitOfWork.RoomRepository.RoomAlreadyExists(newItem.RoomName);

            if (roomExists != true) return BadRequest("Room doesn't exist!");

            var itemExists = _unitOfWork.RoomItemRepository.ItemExists(newItem.DeviceName);

            if (itemExists == true) return Conflict("Item already exists!");

            //query to status service to get more data about the item            
            var itemFromStatusService = _grpcClient.GetDeviceFromStatusAPI(newItem.DeviceName);

            var itemToAdd = _mapper.Map<RoomItem>(itemFromStatusService);


            Enum.TryParse(itemFromStatusService.DeviceType, out DeviceType deviceType);
            itemToAdd.DeviceType = deviceType;
            
            var roomToBeUpdated = await _unitOfWork.RoomRepository.GetRoom(newItem.RoomName);

            itemToAdd.RoomId = roomToBeUpdated.Id;
            var currentDate = DateTime.UtcNow;
            itemToAdd.LastChecked = currentDate;

            _unitOfWork.RoomItemRepository.AddItem(itemToAdd);

            if (await _unitOfWork.Complete()) return Ok();
                    
            //TODO
            //CreatedAtAction(nameof(GeItemByTopic), new { deviceName = itemToAdd.Topic }, null);

            return BadRequest();
        }
         

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [Route("control-switch")]
        public async Task<IActionResult> ControlSwitch([FromBody] SwitchControlDTO control)
        {

            var itemToControl = await _unitOfWork.RoomItemRepository.GetItem(control.DeviceName);

            CommandDTO command = new CommandDTO()
            {
                RoomId = itemToControl.RoomId,
                Command = control.Command,
                Topic = itemToControl.Topic
            };

            //send command to the status service via grpc -> on/off switch
            var statusOfSwitch = _grpcClient.SendCommandToStatusService(command);

            itemToControl.Status = statusOfSwitch;

            var currentDate = DateTime.UtcNow;
            itemToControl.LastChecked = currentDate;

            _unitOfWork.RoomItemRepository.UpdateItem(itemToControl);
            await _unitOfWork.Complete();

            return Ok(statusOfSwitch);
        }

    }
}
