using Microsoft.Extensions.Hosting;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Receiving;
using System.Threading.Tasks;

namespace Logging.API.Services.MQTT
{
    public interface IMqttClientService : IHostedService,
                                          IMqttClientConnectedHandler,
                                          IMqttClientDisconnectedHandler,
                                          IMqttApplicationMessageReceivedHandler
    {

        Task PublishMessage(string commandTopic, string payload);
        //Task SetupSubscriptionTopic(string subscriptionTopic);
        string GetResponse();
        string ReturnCurrentTopic();
    }
}
