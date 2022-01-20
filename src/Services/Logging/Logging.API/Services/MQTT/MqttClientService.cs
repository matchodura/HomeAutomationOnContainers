using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Logging.API.Services.MQTT
{
    public class MqttClientService : IMqttClientService
    {
        private IMqttClient mqttClient;
        private IMqttClientOptions options;
        private readonly ILogger _logger;

        private string _response;
        private string _topic;

        public MqttClientService(IMqttClientOptions options)
        {
            //ILogger logger,
           // _logger = logger;
            this.options = options;
            mqttClient = new MqttFactory().CreateMqttClient();
            ConfigureMqttClient();
        }

        private void ConfigureMqttClient()
        {
            mqttClient.ConnectedHandler = this;
            mqttClient.DisconnectedHandler = this;
            mqttClient.ApplicationMessageReceivedHandler = this;
        }

        public Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eventArgs)
        {

            var payloadText = Encoding.UTF8.GetString(
                eventArgs?.ApplicationMessage?.Payload ?? Array.Empty<byte>());


            _response = payloadText;

            return Task.CompletedTask;
        }

        //public async Task HandleConnectedAsync(MqttClientConnectedEventArgs eventArgs)
        //{
        //    System.Console.WriteLine("connected");
        //    await mqttClient.SubscribeAsync(_topic);
        //}


        public Task HandleDisconnectedAsync(MqttClientDisconnectedEventArgs eventArgs)
        {
            throw new System.NotImplementedException();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await mqttClient.ConnectAsync(options);
            if (!mqttClient.IsConnected)
            {
                await mqttClient.ReconnectAsync();
            }
        }

        public async Task PublishMessage(string topic, string payload)
        {
            var messageA = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(payload)
                    .Build();

            await mqttClient.PublishAsync(messageA);
        }

        public string GetResponse()
        {
            return _response;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                var disconnectOption = new MqttClientDisconnectOptions
                {
                    ReasonCode = MqttClientDisconnectReason.NormalDisconnection,
                    ReasonString = "NormalDiconnection"
                };
                await mqttClient.DisconnectAsync(disconnectOption, cancellationToken);
            }
            await mqttClient.DisconnectAsync();
        }

        public async Task SetupTopic(string topic)
        {
            _topic = topic;
            await mqttClient.SubscribeAsync(_topic);
        }

        public Task HandleConnectedAsync(MqttClientConnectedEventArgs eventArgs)
        {
            //_logger.LogInformation("MQTT client connected!");

            return Task.CompletedTask;
        }
    }
}
