using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Logging.API.Data;
using Logging.API.Interfaces;
using Serilog;
using System;

namespace Logging.API.Extensions
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


            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<Serilog.ILogger>(CreateSerilogLogger(config));
            services.AddDbContext<RpiDataContext>(options =>
            {

                string connStr = config.GetConnectionString("DefaultConnection");


                options.UseNpgsql(connStr);

            });


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
                .Enrich.WithProperty("_service", "Logging")
                .Enrich.WithProperty("_machine", machineName)
                .WriteTo.Console()
                .WriteTo.Seq(string.IsNullOrWhiteSpace(seqServerUrl) ? "http://seq" : seqServerUrl)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

        }
    }
}


