using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Status.API.DTOs;
using Status.API.Entities;
using Status.API.HubConfig;
using Status.API.Services;
using Status.API.TimerFeatures;

namespace Status.Controllers.V1
{
    public class StatusController : BaseApiController
    {
        private readonly MongoDataContext _deviceService;
        private readonly IMapper _mapper;
        private readonly Serilog.ILogger _logger;
        private IHubContext<StatusHub> _hub;

        public StatusController(MongoDataContext deviceService, IMapper mapper, Serilog.ILogger logger, IHubContext<StatusHub> hub)
        {
            _deviceService = deviceService;
            _mapper = mapper;
            _logger = logger;
            _hub = hub;
        }

        //[HttpGet]
        //public async Task<List<Device>> Get() =>
        //    await _deviceService.GetAsync();

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string sensorName)
        {
            //TODO here return correct sensor
            var device = await _deviceService.GetAsync(sensorName);

            Thread.Sleep(5000);

            if (device is null)
            {
                return NotFound();
            }
                     
            var response = _mapper.Map<DeviceStatusResponseDTO>(device);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(DeviceDTO newDevice)
        {              
            var device = _mapper.Map<Device>(newDevice);
                      
            await _deviceService.CreateAsync(device);

            return CreatedAtAction(nameof(Get), new { id = device.Id }, device);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Device updatedDevice)
        {
            var device = await _deviceService.GetAsync(id);

            if (device is null)
            {
                return NotFound();
            }

            updatedDevice.Id = device.Id;

            await _deviceService.UpdateAsync(id, updatedDevice);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var device = await _deviceService.GetAsync(id);

            if (device is null)
            {
                return NotFound();
            }

            await _deviceService.RemoveAsync(device.Id);

            return NoContent();
        }         

    }
}
