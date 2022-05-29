using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Network.API.DTOs;
using Network.API.HubConfig;
using Network.API.Infrastructure.Interfaces;

namespace Network.API.Services
{
    public class StatusUpdateService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly Serilog.ILogger _logger;
        private readonly IHubContext<StatusHub> _hub;
        private readonly IMapper _mapper;

        private Timer _timer = null!;

        public StatusUpdateService(Serilog.ILogger logger, IHubContext<StatusHub> hub,
            IMapper mapper)
        {
            _logger = logger;
            _hub = hub;           
            _mapper = mapper;      

        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.Information("Device Status Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            await _hub.Clients.All.SendAsync("status-data", GetMappedData());
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.Information("Device Status Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private List<DeviceStatusResponseDTO> GetMappedData()
        {
            //var allDevices = _dbContext.GetAllSync();
            // var response = _mapper.Map<List<DeviceStatusResponseDTO>>(allDevices);
            // return response;
            return null;
        }
    }
}
 