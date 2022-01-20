using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTTClient
{
    public class Client : IClient
    {

        private readonly string _clientId;
        private readonly string _clientUsername;
        private readonly string _clientPassword;
        private readonly string _brokerAddress;
        private readonly int _brokerPort;

        private IMqttClient? _mqttClient;

        public IMqttClient? MqttClient
        { 
            get { return _mqttClient; }
            set { _mqttClient = value; }
        }

        public Client(string clientId, string clientUsername, string clientPassword, string brokerAddress, int brokerPort)
        {
            _clientId = clientId;
            _clientUsername = clientUsername;
            _clientPassword = clientPassword;
            _brokerAddress = brokerAddress;
            _brokerPort = brokerPort;
        }

        public async Task<MqttClientConnectResult> SetupClientConnection()
        {
            var factory = new MqttFactory();

            var clientOptions = new MqttClientOptionsBuilder()
                .WithClientId(_clientId)
                .WithCredentials(_clientUsername, _clientPassword)
                .WithTcpServer(_brokerAddress, _brokerPort)
                .Build();

            _mqttClient = factory.CreateMqttClient();

            var result = await _mqttClient.ConnectAsync(clientOptions, CancellationToken.None);

            return result;
        }

       // Topic = "stat/czujnik/STATUS10"

        public async Task SubscribeToTopics(string[] topics)
        {
            foreach(var topic in topics)
            {
                await _mqttClient.SubscribeAsync(new MqttTopicFilter { Topic = topic });
            }                               

        }

        public async Task<string> BuildMessageAndPublish(string topic, string payload)
        {
            var messageA = new MqttApplicationMessageBuilder()
                  .WithTopic(topic)
                  .WithPayload(payload)
                  .Build();

            await _mqttClient.PublishAsync(messageA);


            string returnMessage = string.Empty;



            return returnMessage; 
        }


    }
}
