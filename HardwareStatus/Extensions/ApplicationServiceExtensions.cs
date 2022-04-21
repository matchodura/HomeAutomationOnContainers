using HardwareStatus.API.Profiles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;

namespace HardwareStatus.Extensions
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

            services.AddCors(options => {
                options.AddPolicy("CorsPolicy", builder => builder
                 .WithOrigins("http://localhost:4200/")
                 .SetIsOriginAllowed((host) => true)
                 .AllowAnyMethod()
                 .AllowAnyHeader()
                 .AllowCredentials());
            });

            services.AddAutoMapper(typeof(AutoMapperHardwareProfile).Assembly);

            services.AddSingleton<Serilog.ILogger>(CreateSerilogLogger(config));

            return services;
        }

        private static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            string machineName = Environment.MachineName;
            var seqServerUrl = configuration["Serilog:SeqServerUrl"];

            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("_service", "hardware-status-api")
                .Enrich.WithProperty("_machine", machineName)
                .WriteTo.Seq(string.IsNullOrWhiteSpace(seqServerUrl) ? "http://seq" : seqServerUrl)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

        }

    }
}
