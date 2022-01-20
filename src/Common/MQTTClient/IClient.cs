using MQTTnet.Client.Connecting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTTClient
{
    public interface IClient
    {
        Task<MqttClientConnectResult> SetupClientConnection();
        Task SubscribeToTopics(string[] topics);
        Task<string> BuildMessageAndPublish(string topic, string payload);
    }
}
