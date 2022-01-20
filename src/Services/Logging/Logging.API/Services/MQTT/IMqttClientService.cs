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

        Task PublishMessage(string topic, string payload);

        Task SetupTopic(string topic);

        string GetResponse();
    }
}
