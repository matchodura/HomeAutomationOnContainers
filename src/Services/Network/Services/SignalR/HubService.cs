using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Network.API.Entities;
using Network.API.HubConfig;
using Network.API.Infrastructure.Interfaces;

namespace Network.API.Services
{
    public class HubService : IHostedService, IDisposable
    {
        private readonly Serilog.ILogger _logger;
        private readonly IHubContext<StatusHub> _hub;
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        private Timer _timer = null!;

        public HubService(Serilog.ILogger logger, IHubContext<StatusHub> hub,
            IMapper mapper, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _hub = hub;
            _mapper = mapper;
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.Information("SignalR Hub Service is running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(15));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            await _hub.Clients.All.SendAsync("network-devices-status", GetDevices());
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.Information("SignalR Hub Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private List<Device> GetDevices()
        {      
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<IUnitOfWork>();
                var allDevices = context.DeviceRepository.GetAllDevices();

                return allDevices.Result;
            }                           
        }
    }
}
 