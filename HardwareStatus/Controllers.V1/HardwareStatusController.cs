using AutoMapper;
using HardwareStatus.API.Entities;
using HardwareStatus.API.Infrastructure.Interfaces;
using HardwareStatus.API.NetworkScanner;
using HardwareStatus.Controllers.V1;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Status.Controllers.V1
{
    public class HardwareStatusController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly Serilog.ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HardwareStatusController(IMapper mapper, Serilog.ILogger logger, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {

            return Ok("works!");
        }


        [HttpPost]
        [Route("refresh-devices")]
        public async Task<IActionResult> RefreshDevices()
        {
            _logger.Information("Starting total scan of devices in network!");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var scannedDevices = Scanner.TotalScan();
            stopwatch.Stop();

            _logger.Information($"Scan took {stopwatch.ElapsedMilliseconds} miliseconds, found {scannedDevices.Count} devices!");
            //map found devices to entity in database
            List<Device> devicesToAdd = new List<Device>();

            foreach(var device in scannedDevices)
            {
                devicesToAdd.Add(new Device()
                {
                    Name = device.HostName,
                    DeviceType = HardwareStatus.API.Enums.DeviceType.Unknown,
                    HostName = device.HostName,
                    IP = device.IP,
                    MAC = device.MAC,
                    DeviceStatus = HardwareStatus.API.Enums.DeviceStatus.Unknown
                });
            }


            _unitOfWork.DeviceRepository.AddDevices(devicesToAdd);
            await _unitOfWork.Complete();

            return Ok(scannedDevices);
        }


        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<List<Device>>> GetAllDevices()
        {
            var allDevices = await _unitOfWork.DeviceRepository.GetAllDevices();

            return Ok(allDevices);
        }
    }
}
