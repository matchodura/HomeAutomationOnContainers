using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Network.API.DTOs;
using Network.API.Entities;
using Network.API.Infrastructure.Interfaces;
using Network.Controllers.V1;
using System.Net;

namespace Network.API.Controllers.V1
{
    public class MosquittoController : BaseApiController
    {
        private readonly Serilog.ILogger _logger;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MosquittoController(Serilog.ILogger logger, IUnitOfWork unitOfWork, IMapper mapper)
        {           
            _logger = logger;        
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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

        //add mosquitto device
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] AddMosquittoDeviceDTO mosquittoDeviceDTO)
        {
            var deviceToMarkAsMosquitto = _unitOfWork.DeviceRepository.GetDevice(mosquittoDeviceDTO.Name).Result;

            if (deviceToMarkAsMosquitto == null) return Conflict("Device doesn't exist!");


            var mosquittoDeviceToAdd = _mapper.Map<MosquittoDevice>(mosquittoDeviceDTO);

            var deviceToAdd = _mapper.Map<Device>(deviceToMarkAsMosquitto);

            deviceToAdd.IsMosquitto = true;
            deviceToAdd.MosquittoDevice = mosquittoDeviceToAdd;

            var currentDate = DateTime.UtcNow;
            deviceToAdd.DateModified = currentDate;
            deviceToAdd.MosquittoDevice.DateAdded = currentDate;
            deviceToAdd.MosquittoDevice.DateModified = currentDate;
            deviceToAdd.MosquittoDevice.State = "Just Added!";

            _unitOfWork.DeviceRepository.UpdateDevice(deviceToAdd);

            if (await _unitOfWork.Complete()) return CreatedAtAction(nameof(Get), new { deviceName = deviceToAdd.Name }, null);

            return BadRequest();
        }
    }
}
