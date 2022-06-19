using AutoMapper;
using Common.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Network.API.Entities;
using Network.API.HubConfig;
using Network.API.Infrastructure.Interfaces;
using Network.API.NetworkScanner;
using System.Diagnostics;

namespace Network.Controllers.V1
{
    public class NetworkController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly Serilog.ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<StatusHub> _hub;

        public NetworkController(IMapper mapper, Serilog.ILogger logger, IUnitOfWork unitOfWork, IHubContext<StatusHub> hub)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _hub = hub;
        }

        //scan all devices in the network - total scan
        [HttpGet]
        [Route("total-scan")]
        public async Task<IActionResult> TotalScan()
        {
            _logger.Information("Starting total scan of devices in network!");

            _unitOfWork.DeviceRepository.TruncateTable();
            await _unitOfWork.Complete();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var scannedDevices = Scanner.TotalScan(_logger);
            stopwatch.Stop();

            _logger.Information($"Scan took {stopwatch.ElapsedMilliseconds} miliseconds, found {scannedDevices.Count} devices!");

            //map found devices to entity in database
            List<Device> devicesToAdd = new List<Device>();

            foreach (var device in scannedDevices)
            {
                devicesToAdd.Add(new Device()
                {
                    Name = device.HostName ?? "default",
                    HardwareType = HardwareType.Unknown,
                    HostName = device.HostName ?? "default",
                    IP = device.IP,
                    MAC = device.MAC,
                    DeviceStatus = DeviceStatus.Online
                });
            }

            _unitOfWork.DeviceRepository.AddDevices(devicesToAdd);
            await _unitOfWork.Complete();

            return Ok("completed!");
        }

        //refresh specified device in network
        [HttpPost]
        [Route("refresh-device/{name}")]
        public async Task<IActionResult> RefreshDevice(string name)
        {
            var devicetoCheck = _unitOfWork.DeviceRepository.GetDevice(name).Result;

            var scannedDevice = Scanner.ScanOfKnownDevices(devicetoCheck.IP);

            devicetoCheck.LastCheck = DateTime.UtcNow;
            devicetoCheck.LastAlive = scannedDevice.Status == DeviceStatus.Online ? DateTime.UtcNow : devicetoCheck.LastAlive;
            devicetoCheck.DeviceStatus = scannedDevice.Status;

            _logger.Information($"Refreshing status of device: {name} is {devicetoCheck.DeviceStatus.ToString()}");

            _unitOfWork.DeviceRepository.UpdateDevice(devicetoCheck);

            await _unitOfWork.Complete();

            return Ok("completed!");
        }

        //refreshes all configured devices in network
        [HttpPost]
        [Route("refresh-all")]
        public async Task<IActionResult> RefreshAllDevices()
        {
            var devicesToCheck = _unitOfWork.DeviceRepository.GetAllDevices().Result;

            var refreshedDevices = Scanner.ScanOfKnownDevices(devicesToCheck.Select(x => x.IP).ToArray());


            foreach (var device in devicesToCheck)
            {
                var deviceToUpdate = refreshedDevices.Where(x => x.IP == device.IP).First();

                device.DeviceStatus = deviceToUpdate.Status;

                if(deviceToUpdate.Status == DeviceStatus.Online)
                {
                    device.LastAlive = refreshedDevices.Where(x => x.IP == device.IP).Select(x => x.TimeOfScan).First();
                }

                device.LastCheck = refreshedDevices.Where(x => x.IP == device.IP).Select(x => x.TimeOfScan).First();
            }

            _unitOfWork.DeviceRepository.UpdateDevices(devicesToCheck);
            await _unitOfWork.Complete();

            return Ok("completed!");
        }

        //find new devices in network
        [HttpGet]
        [Route("find-new-devices")]
        public async Task<IActionResult> FindNewDevices()
        {
            //find already configured devices
            var configuredDevices = await _unitOfWork.DeviceRepository.GetAllDevices();

            //find new devices which previously were not configured -> saves some time pinging
            var foundDevices = Scanner.ScanNewDevices(configuredDevices.Select(x => x.IP).ToArray());

            List<Device> devicesToAdd = new List<Device>();

            foreach (var device in foundDevices)
            {
                devicesToAdd.Add(new Device()
                {
                    Name = device.HostName ?? "default",
                    HardwareType = HardwareType.Unknown,
                    HostName = device.HostName ?? "default",
                    IP = device.IP,
                    MAC = device.MAC,
                    DeviceStatus = DeviceStatus.Online
                });
            }                      

            _unitOfWork.DeviceRepository.AddDevices(devicesToAdd);
            await _unitOfWork.Complete();

            return Ok(foundDevices);
        }
    }
}
