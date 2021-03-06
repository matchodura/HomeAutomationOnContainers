using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Network.API.Services.MQTT
{
    public class MqttClientService : IMqttClientService
    {
        private IMqttClient mqttClient;
        private IMqttClientOptions options;

        private string _response;
        private string _topic;

        public MqttClientService(IMqttClientOptions options)
        {            
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

        public async Task HandleConnectedAsync(MqttClientConnectedEventArgs eventArgs)
        {
            System.Console.WriteLine("connected");

            //TODO make it configurable
            await mqttClient.SubscribeAsync(
            new MqttTopicFilter
            {
                Topic = "stat/pokoj/mati/dht/RESULT",
                QualityOfServiceLevel = MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce
            },
            new MqttTopicFilter
            {
                Topic = "stat/pokoj/mati/gniazdko/RESULT",
                QualityOfServiceLevel = MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce
            });
        }
         


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

        public async Task PublishMessage(string commandTopic, string payload)
        {
            var messageA = new MqttApplicationMessageBuilder()
                    .WithTopic(commandTopic)
                    .WithPayload(payload)
                    .Build();

            await mqttClient.PublishAsync(messageA);
        }

        public string GetResponse()
        {
            return _response;
        }

        public void CleanResponse()
        {
            _response = "";
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

        public string ReturnCurrentTopic()
        {
            return _topic;
        }

        public Task SetupSubscriptionTopic(string subscriptionTopic)
        {
            throw new NotImplementedException();
        }
    }
}
