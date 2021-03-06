using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Logging.API.Data;
using Logging.API.Interfaces;
using Serilog;
using System;
using Logging.API.Profiles;
using Logging.API.EventProcessing;

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

            services.AddCors(options => {
                options.AddPolicy("CorsPolicy", builder => builder
                 .WithOrigins("http://localhost:4200/")
                 .SetIsOriginAllowed((host) => true)
                 .AllowAnyMethod()
                 .AllowAnyHeader()
                 .AllowCredentials());
            });

            services.AddAutoMapper(typeof(AutoMapperLoggingProfile).Assembly);
            services.AddGrpc();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<Serilog.ILogger>(CreateSerilogLogger(config));
            services.AddDbContext<DataContext>(options =>
            {
                string connStr = config.GetConnectionString("DefaultConnection");
                options.UseNpgsql(connStr);             
            });

            //services.AddMqttClientHostedService();
            //services.AddHostedService<DataPollingService>();
            //services.AddHostedService<MessageBusSubscriber>();

            services.AddSingleton<IEventProcessor, EventProcessor>();

            return services;
        }

        //public static IServiceCollection AddMqttClientHostedService(this IServiceCollection services)
        //{
        //    services.AddMqttClientServiceWithConfig(aspOptionBuilder =>
        //    {
        //        var clientSettinigs = AppSettingsProvider.ClientSettings;
        //        var brokerHostSettings = AppSettingsProvider.BrokerHostSettings;

        //        aspOptionBuilder
        //        .WithCredentials(clientSettinigs.UserName, clientSettinigs.Password)
        //        .WithClientId(clientSettinigs.Id)
        //        .WithTcpServer(brokerHostSettings.Host, brokerHostSettings.Port);
        //    });
        //    return services;
        //}

        //private static IServiceCollection AddMqttClientServiceWithConfig(this IServiceCollection services, Action<AspCoreMqttClientOptionBuilder> configure)
        //{
        //    services.AddSingleton<IMqttClientOptions>(serviceProvider =>
        //    {
        //        var optionBuilder = new AspCoreMqttClientOptionBuilder(serviceProvider);
        //        configure(optionBuilder);
        //        return optionBuilder.Build();
        //    });
        //    services.AddSingleton<MqttClientService>();
        //    services.AddSingleton<IHostedService>(serviceProvider =>
        //    {
        //        return serviceProvider.GetService<MqttClientService>();
        //    });
        //    services.AddSingleton<MqttClientServiceProvider>(serviceProvider =>
        //    {
        //        var mqttClientService = serviceProvider.GetService<MqttClientService>();
        //        var mqttClientServiceProvider = new MqttClientServiceProvider(mqttClientService);
        //        return mqttClientServiceProvider;
        //    });
        //    return services;
        //}



        private static ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            string machineName = Environment.MachineName;
            var seqServerUrl = configuration["Serilog:SeqServerUrl"];

            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("_service", "logging-api")
                .Enrich.WithProperty("_machine", machineName)
                .WriteTo.Console()
                .WriteTo.Seq(string.IsNullOrWhiteSpace(seqServerUrl) ? "http://seq" : seqServerUrl)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

        }
    }
}


