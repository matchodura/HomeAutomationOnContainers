using AutoMapper;
using Common.Enums;
using Network.API.Entities;
using Network.API.Infrastructure.Interfaces;
using Network.API.NetworkScanner;

namespace Network.API.Services
{
    public class NetworkScanService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly Serilog.ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        private Timer _timer = null!;

        public NetworkScanService(Serilog.ILogger logger, IMapper mapper, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;           
            _mapper = mapper;
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.Information("Network Scan Service is running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(60));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<IUnitOfWork>();

                var allDevices = context.DeviceRepository.GetAllDevices();

                if(allDevices.Result.Count > 0)
                {
                    var ipAdresses = allDevices.Result.Select(x => x.IP).ToArray();

                    var results = Scanner.ScanOfKnownDevices(ipAdresses);

                    var output = allDevices.Result.Join(results,
                        a => a.IP,
                        b => b.IP,
                        (a, b) => new Device
                        {
                            Id = a.Id,
                            Name = a.Name,
                            HardwareType = a.HardwareType,
                            HostName = a.HostName,
                            IP = a.IP,
                            MAC = a.MAC,
                            DateAdded = a.DateAdded,
                            DateModified = a.DateModified,
                            LastCheck = b.TimeOfScan,
                            LastAlive = b.Status == DeviceStatus.Online ? DateTime.UtcNow : a.LastAlive,
                            DeviceStatus = b.Status
                        }).ToList();


                    context.DeviceRepository.UpdateDevices(output);
                    await context.Complete();
                }
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.Information("Network Scan Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
 