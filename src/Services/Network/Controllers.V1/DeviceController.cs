using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Network.API.DTOs;
using Network.API.Entities;
using Network.API.HubConfig;
using Network.API.Services;
using Network.API.TimerFeatures;

namespace Network.Controllers.V1
{
    public class DeviceController : BaseApiController
    {    
        private readonly IMapper _mapper;
        private readonly Serilog.ILogger _logger;
        private IHubContext<StatusHub> _hub;

        public DeviceController(IMapper mapper, Serilog.ILogger logger, IHubContext<StatusHub> hub)
        {
        
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
            //var device = await _deviceService.GetAsync(sensorName);

            //Thread.Sleep(5000);

            //if (device is null)
            //{
            //    return NotFound();
            //}

            //var response = _mapper.Map<DeviceStatusResponseDTO>(device);
            //return Ok(response);
            return Ok(null);
        }

        [HttpPost]
        [Route("add-device")]
        public async Task<IActionResult> Post([FromBody] DeviceDTO newDevice)
        {
            //var device = _mapper.Map<Device>(newDevice);

            //device.DateAdded = DateTime.UtcNow;
            //device.DateModified = DateTime.UtcNow;
            //await _deviceService.CreateAsync(device);

            //return CreatedAtAction(nameof(Get), new { id = device.Id }, device);
            return Ok(null);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Device updatedDevice)
        {
            //var device = await _deviceService.GetAsync(id);

            //if (device is null)
            //{
            //    return NotFound();
            //}

            //updatedDevice.Id = device.Id;

            //await _deviceService.UpdateAsync(id, updatedDevice);

            //return NoContent();
            return Ok(null);
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            //var device = await _deviceService.GetAsync(id);

            //if (device is null)
            //{
            //    return NotFound();
            //}

            //await _deviceService.RemoveAsync(device.Id);
            //return NoContent();
            return Ok(null);
        }         

    }
}
