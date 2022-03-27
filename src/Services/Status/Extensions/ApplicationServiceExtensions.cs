using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet.Client.Options;
using Serilog;
using Status.API.Entities;
using Status.API.MQTT;
using Status.API.Options;
using Status.API.Profiles;
using Status.API.Services;
using Status.API.Services.MQTT;
using Status.API.Services.RabbitMQ;

namespace Status.Extensions
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

            services.AddAutoMapper(typeof(AutoMapperStatusProfile).Assembly);
            services.AddGrpc();
            //services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<Serilog.ILogger>(CreateSerilogLogger(config));
            //services.AddDbContext<RpiDataContext>(options =>
            //{
            //    string connStr = config.GetConnectionString("DefaultConnection");
            //    options.UseNpgsql(connStr);
            //});

            services.Configure<DeviceDatabaseSettings>(config.GetSection("DeviceDatabase"));
            services.AddSingleton<MongoDataContext>();

            services.AddMqttClientHostedService();

            services.AddHostedService<DeviceCheckService>();
            services.AddHostedService<StatusUpdateService>();
            services.AddSingleton<IMessageBusClient, MessageBusClient>();

            services.AddSignalR();

            return services;
        }

        public static IServiceCollection AddMqttClientHostedService(this IServiceCollection services)
        {
            services.AddMqttClientServiceWithConfig(aspOptionBuilder =>
            {
                var clientSettinigs = AppSettingsProvider.ClientSettings;
                var brokerHostSettings = AppSettingsProvider.BrokerHostSettings;

                aspOptionBuilder
                .WithCredentials(clientSettinigs.UserName, clientSettinigs.Password)
                .WithClientId(clientSettinigs.Id)
                .WithTcpServer(brokerHostSettings.Host, brokerHostSettings.Port);
            });
            return services;
        }

        private static IServiceCollection AddMqttClientServiceWithConfig(this IServiceCollection services, Action<AspCoreMqttClientOptionBuilder> configure)
        {
            services.AddSingleton<IMqttClientOptions>(serviceProvider =>
            {
                var optionBuilder = new AspCoreMqttClientOptionBuilder(serviceProvider);
                configure(optionBuilder);
                return optionBuilder.Build();
            });
            services.AddSingleton<MqttClientService>();
            services.AddSingleton<IHostedService>(serviceProvider =>
            {
                return serviceProvider.GetService<MqttClientService>();
            });
            services.AddSingleton<MqttClientServiceProvider>(serviceProvider =>
            {
                var mqttClientService = serviceProvider.GetService<MqttClientService>();
                var mqttClientServiceProvider = new MqttClientServiceProvider(mqttClientService);
                return mqttClientServiceProvider;
            });
            return services;
        }

        private static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            string machineName = Environment.MachineName;
            var seqServerUrl = configuration["Serilog:SeqServerUrl"];

            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("_service", "status-api")
                .Enrich.WithProperty("_machine", machineName)
                .WriteTo.Seq(string.IsNullOrWhiteSpace(seqServerUrl) ? "http://seq" : seqServerUrl)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

        }

    }
}
