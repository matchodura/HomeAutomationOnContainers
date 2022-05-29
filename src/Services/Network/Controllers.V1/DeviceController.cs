using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Network.API.DTOs;
using Network.API.Entities;
using Network.API.HubConfig;
using Network.API.Infrastructure.Interfaces;
using Network.API.Services;
using Network.API.TimerFeatures;
using System.Net;

namespace Network.Controllers.V1
{
    public class DeviceController : BaseApiController
    {    
        private readonly IMapper _mapper;
        private readonly Serilog.ILogger _logger;
        private IHubContext<StatusHub> _hub;
        private IUnitOfWork _unitOfWork;

        public DeviceController(IMapper mapper, Serilog.ILogger logger, IHubContext<StatusHub> hub, IUnitOfWork unitOfWork)
        {

            _mapper = mapper;
            _logger = logger;
            _hub = hub;
            _unitOfWork = unitOfWork;
        }

        //get all devices
        [HttpGet]
        [ProducesResponseType(typeof(List<Device>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Device>> Get()
        {
            var devicesToReturn = await _unitOfWork.DeviceRepository.GetAllDevices();

            if (devicesToReturn == null) return NotFound("No devices are yet configured!");

            return Ok(devicesToReturn);
        }

        //get device
        [HttpGet("{deviceName}")]
        [ProducesResponseType(typeof(Device), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Device>> Get(string deviceName)
        {
            if (string.IsNullOrEmpty(deviceName)) return BadRequest("Invalid name of the device!");

            var deviceToReturn = await _unitOfWork.DeviceRepository.GetDevice(deviceName);

            if (deviceToReturn == null) return NotFound("Device with that name does not exist!");

            return Ok(deviceToReturn);
        }

        //add device
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] AddDeviceDTO newDevice)
        {
            var deviceExists = _unitOfWork.DeviceRepository.DeviceExists(newDevice.Name);

            if (deviceExists == true) return Conflict("Device already exists!");

            var deviceToAdd = _mapper.Map<Device>(newDevice);

            var currentDate = DateTime.UtcNow;
            deviceToAdd.DateModified = currentDate;
            deviceToAdd.DateAdded = currentDate;

            _unitOfWork.DeviceRepository.AddDevice(deviceToAdd);

            if (await _unitOfWork.Complete()) return CreatedAtAction(nameof(Get), new { deviceName = deviceToAdd.Name }, null);

            return BadRequest();
        }

        //TODO update device
        //[HttpPut("{deviceName")]
        //[ProducesResponseType((int)HttpStatusCode.NotFound)]
        //[ProducesResponseType((int)HttpStatusCode.Created)]
        //public async Task<IActionResult> Update(string deviceName, [FromBody]  updatedRoom)
        //{
        //    var roomExists = _unitOfWork.RoomRepository.RoomAlreadyExists(updatedRoom.Name);

        //    if (roomExists == false) return NotFound("Room doesn't exist!");

        //    var room = _mapper.Map<Room>(updatedRoom);

        //    var currentDate = DateTime.UtcNow;
        //    room.LastModified = currentDate;

        //    _unitOfWork.RoomRepository.UpdateRoom(room);

        //    if (await _unitOfWork.Complete()) return CreatedAtAction(nameof(Get), new { roomName = room.Name }, null);

        //    return BadRequest();
        //}

        //delete device
        [HttpDelete("{deviceName}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Delete(string deviceName)
        {
            var deviceExists = _unitOfWork.DeviceRepository.DeviceExists(deviceName);

            if (deviceExists == false) return NotFound("Device doesn't exist!");

            var deviceToBeDeleted = await _unitOfWork.DeviceRepository.GetDevice(deviceName);

            _unitOfWork.DeviceRepository.DeleteDevice(deviceToBeDeleted);

            if (await _unitOfWork.Complete()) return Ok("Device has been deleted sucessfuly!");

            return BadRequest();
        }
    }
}
