using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Camera.API.Services
{
    public class CameraService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger _logger;
        private Timer _timer = null!;


        public CameraService(ILogger logger){
            _logger = logger;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.Information("Camera Service is running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);

            string sourceURL = Constants.CameraUrl;
            string path = Constants.Path + DateTime.Now.ToString("yyyy-dd-M-HH-mm-ss") + ".jpg";

            WebClient webClient = new WebClient();           

            using (Stream webStream = webClient.OpenRead(sourceURL))
            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                var buffer = new byte[32768];
                int bytesRead;
                Int64 bytesReadComplete = 0;  // Use Int64 for files larger than 2 gb

                // Get the size of the file to download
                Int64 bytesTotal = Convert.ToInt64(webClient.ResponseHeaders["Content-Length"]);

                // Download file in chunks
                while ((bytesRead = webStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    bytesReadComplete += bytesRead;
                    fileStream.Write(buffer, 0, bytesRead);
                }

            }

            _logger.Information(
                            "Camera Service is working. {count}", count);
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.Information("Camera Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
