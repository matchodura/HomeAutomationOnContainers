using HomeControl.API.Profiles;
using HomeControl.API.SyncDataServices.Grpc;
using Logging.API.Profiles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;


namespace HomeControl.API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

            //services.AddHttpClient("rpi", c =>
            //{
            //    c.BaseAddress = new Uri("https://api.github.com/");
            //    // Github API versioning
            //    c.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
            //    // Github requires a user-agent
            //    c.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
            //});


            services.AddAutoMapper(typeof(AutoMapperHomeControlProfile).Assembly);
            services.AddHttpClient();
            services.AddSingleton<ILoggingDataClient, LoggingDataClient>();       
            services.AddSingleton<Serilog.ILogger>(CreateSerilogLogger(config));

            //services.AddHostedService<DataPollingService>();

            return services;
        }

        private static ILogger CreateSerilogLogger(IConfiguration configuration)
        { 
            string machineName = Environment.MachineName;
            var seqServerUrl = configuration["Serilog:SeqServerUrl"];
            
            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("_service", "SensorLogging")
                .Enrich.WithProperty("_machine", machineName)
                .WriteTo.Console()
                .WriteTo.Seq(string.IsNullOrWhiteSpace(seqServerUrl) ? "http://seq" : seqServerUrl)
                .ReadFrom.Configuration(configuration)                
                .CreateLogger();

        }
    }
    
}


