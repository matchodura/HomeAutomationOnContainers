using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Network.API.Entities;
using Network.API.HubConfig;
using Network.API.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HardwareStatus.API.Services
{
    public class HardwareStatusUpdateService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly Serilog.ILogger _logger;
        private readonly IHubContext<StatusHub> _hub;
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        private Timer _timer = null!;

        public HardwareStatusUpdateService(Serilog.ILogger logger, IHubContext<StatusHub> hub,
            IMapper mapper, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _hub = hub;
            _mapper = mapper;
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.Information("Hardware Status Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(15));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            await _hub.Clients.All.SendAsync("hardware-status-data", GetCurrentData());
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.Information("Hardware Status Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private List<Device> GetCurrentData()
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
 