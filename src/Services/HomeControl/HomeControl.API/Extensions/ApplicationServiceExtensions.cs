using HomeControl.API.EventProcessing;
using HomeControl.API.Infrastructure.Data;
using HomeControl.API.Interfaces;
using HomeControl.API.Profiles;
using HomeControl.API.Services;
using HomeControl.API.SyncDataServices.Grpc;
using HomeCtronol.API.Services.RabbitMQ;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            services.AddCors(options => {
                options.AddPolicy("CorsPolicy", builder => builder
                 .WithOrigins("http://localhost:4200/")
                 .SetIsOriginAllowed((host) => true)
                 .AllowAnyMethod()
                 .AllowAnyHeader()
                 .AllowCredentials());
            });

            services.AddDbContext<DataContext>(options =>
            {
                string connStr = config.GetConnectionString("DefaultConnection");
                options.UseNpgsql(connStr);
            });

            services.AddAutoMapper(typeof(AutoMapperHomeControlProfile).Assembly);
            services.AddHttpClient();
            services.AddSingleton<IGrpcClient, GrpcClient>();       
            services.AddSingleton<Serilog.ILogger>(CreateSerilogLogger(config));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddHostedService<SensorPollingService>();
            services.AddSingleton<IEventProcessor, EventProcessor>();
            services.AddHostedService<MessageBusSubscriber>();
            return services;
        }

        private static ILogger CreateSerilogLogger(IConfiguration configuration)
        { 
            string machineName = Environment.MachineName;
            var seqServerUrl = configuration["Serilog:SeqServerUrl"];
            
            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("_service", "homecontrol-api")
                .Enrich.WithProperty("_machine", machineName)
                .WriteTo.Console()
                .WriteTo.Seq(string.IsNullOrWhiteSpace(seqServerUrl) ? "http://seq" : seqServerUrl)
                .ReadFrom.Configuration(configuration)                
                .CreateLogger();
        }
    }
    
}


