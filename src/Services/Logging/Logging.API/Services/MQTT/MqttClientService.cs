using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Logging.API.Services.MQTT
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

            var topic = eventArgs?.ApplicationMessage.Topic ?? string.Empty;

            _response = payloadText;

            return Task.CompletedTask;
        }

        public async Task HandleConnectedAsync(MqttClientConnectedEventArgs eventArgs)
        {
            System.Console.WriteLine("connected");                       

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

        public async Task SetupSubscriptionTopics(string[] subscriptionTopics)
        {
            var topicFilters = new List<MqttTopicFilter>();

            foreach(var topic in subscriptionTopics)
            {                
                var topicFilter = new MqttTopicFilter
                {
                    Topic = "stat/" + topic + "/STATUS10",
                    QualityOfServiceLevel = MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce
                };

                topicFilters.Add(topicFilter);

            }

            var test = topicFilters.ToArray();

            await mqttClient.SubscribeAsync(test);

        }
    }
}
