using AutoMapper;
using HardwareStatus.API.Entities;
using HardwareStatus.API.HubConfig;
using HardwareStatus.API.Infrastructure.Interfaces;
using HardwareStatus.API.NetworkScanner;
using HardwareStatus.Controllers.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Status.Controllers.V1
{
    public class HardwareStatusController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly Serilog.ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<StatusHub> _hub;

        public HardwareStatusController(IMapper mapper, Serilog.ILogger logger, IUnitOfWork unitOfWork, IHubContext<StatusHub> hub)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _hub = hub;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {

            return Ok("works!");
        }


        [HttpGet]
        [Route("refresh-devices")]
        public async Task<IActionResult> RefreshDevices()
        {
            _logger.Information("Starting total scan of devices in network!");

            _unitOfWork.DeviceRepository.TruncateTable();
            await _unitOfWork.Complete();


            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var scannedDevices = Scanner.TotalScan();
            stopwatch.Stop();

            _logger.Information($"Scan took {stopwatch.ElapsedMilliseconds} miliseconds, found {scannedDevices.Count} devices!");
            //map found devices to entity in database
            List<Device> devicesToAdd = new List<Device>();

            foreach (var device in scannedDevices)
            {
                devicesToAdd.Add(new Device()
                {
                    Name = device.HostName,
                    HardwareType = HardwareStatus.API.Enums.HardwareType.Unknown,
                    HostName = device.HostName,
                    IP = device.IP,
                    MAC = device.MAC,
                    DeviceStatus = HardwareStatus.API.Enums.DeviceStatus.Unknown
                });
            }


            _unitOfWork.DeviceRepository.AddDevices(devicesToAdd);
            await _unitOfWork.Complete();

            //return Ok(scannedDevices);

            return Ok("completed!");
        }


        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<List<Device>>> GetAllDevices()
        {
            var allDevices = await _unitOfWork.DeviceRepository.GetAllDevices();

            return Ok(allDevices);
        }

        [HttpPut]
        [Route("update-device")]
        public async Task<ActionResult> UpdateDevice([FromBody] Device device)
        {
            var deviceToUpdate = await _unitOfWork.DeviceRepository.GetDevice(device.HostName);

            deviceToUpdate.HardwareType = device.HardwareType;
            deviceToUpdate.Name = device.Name;

            _unitOfWork.DeviceRepository.UpdateDevice(deviceToUpdate);

            await _unitOfWork.Complete();

            return Ok();
        }

        [HttpPost]
        [Route("refresh-device")]
        public async Task<IActionResult> RefreshDevics([FromBody] Device device)
        {
            var devicetoCheck = _unitOfWork.DeviceRepository.GetDevice(device.HostName).Result;

            var scannedDevice = Scanner.ScanOfKnownDevices(devicetoCheck.IP);

            devicetoCheck.LastCheck = DateTime.UtcNow;
            devicetoCheck.LastAlive = scannedDevice.Status == HardwareStatus.API.Enums.DeviceStatus.Online ? DateTime.UtcNow : devicetoCheck.LastAlive;
            devicetoCheck.DeviceStatus = scannedDevice.Status;

            _unitOfWork.DeviceRepository.UpdateDevice(devicetoCheck);
            await _unitOfWork.Complete();

            return Ok("completed!");
        }


        [HttpGet]
        [Route("find-new-devices")]
        public async Task<IActionResult> FindNewDevices()
        {
            //find already configured devices
            var configuredDevices = await _unitOfWork.DeviceRepository.GetAllDevices();

            //find new devices which previously were not configured -> saves some time pinging
            var foundDevices = Scanner.ScanNewDevices(configuredDevices.Select(x => x.IP).ToArray());

            //devicetoCheck.LastCheck = DateTime.UtcNow;
            //devicetoCheck.LastAlive = scannedDevice.Status == HardwareStatus.API.Enums.DeviceStatus.Online ? DateTime.UtcNow : devicetoCheck.LastAlive;
            //devicetoCheck.DeviceStatus = scannedDevice.Status;

            //_unitOfWork.DeviceRepository.UpdateDevice(devicetoCheck);
            //wait _unitOfWork.Complete();

            return Ok(foundDevices);
        }
    }
}
