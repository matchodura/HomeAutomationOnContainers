using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Status.API.DTOs;
using Status.API.Entities;
using Status.API.Services;

namespace Status.Controllers.V1
{
    public class StatusController : BaseApiController
    {
        private readonly DeviceService _deviceService;
        private readonly IMapper _mapper;
        private readonly Serilog.ILogger _logger;

        public StatusController(DeviceService deviceService, IMapper mapper, Serilog.ILogger logger)
        {
            _deviceService = deviceService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<List<Device>> Get() =>
            await _deviceService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Device>> Get(string id)
        {
            var device = await _deviceService.GetAsync(id);

            if (device is null)
            {
                return NotFound();
            }

            return device;
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
