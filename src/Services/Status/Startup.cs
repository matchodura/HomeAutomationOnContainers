using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi.Models;
using Status.API.MQTT;
using Status.Extensions;
using Status.API.Services.Grpc;
using Status.API.HubConfig;
using System.Text.Json.Serialization;

namespace Status.API
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration configuration)
        {
            _config = configuration;
            MapConfiguration();
        }

        private void MapConfiguration()
        {
            MapBrokerHostSettings();
            MapClientSettings();
        }

        private void MapBrokerHostSettings()
        {
            BrokerHostSettings brokerHostSettings = new BrokerHostSettings();
            _config.GetSection(nameof(BrokerHostSettings)).Bind(brokerHostSettings);
            AppSettingsProvider.BrokerHostSettings = brokerHostSettings;
        }

        private void MapClientSettings()
        {
            ClientSettings clientSettings = new ClientSettings();
            _config.GetSection(nameof(ClientSettings)).Bind(clientSettings);
            AppSettingsProvider.ClientSettings = clientSettings;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationServices(_config);
            services.AddMvc();
            services.AddControllers().AddJsonOptions(x =>
            {
                  x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                  x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
                       
            services.AddHealthChecks();
               

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Status", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //    app.UseSwagger();
            //    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Logging.API v1"));
            //}
           
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1"));

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapHealthChecks("/health");
                endpoints.MapGrpcService<GrpcStatusService>();
                endpoints.MapGrpcService<GrpcControlService>();
                endpoints.MapHub<StatusHub>("/status-hub");
                endpoints.MapGet("/protos/sensors.proto", async context =>
                {
                    await context.Response.WriteAsync(File.ReadAllText("Protos/item_status.proto"));
                });
            });


        }
    }
}
