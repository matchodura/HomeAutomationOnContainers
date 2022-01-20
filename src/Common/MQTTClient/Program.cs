using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Subscribing;
using MQTTnet.Extensions.ManagedClient;
using Newtonsoft.Json;
using System.Text;

namespace MQTTClient
{
    public class Program
    {
        static async Task Main(string[] _)
        {
            var factory = new MqttFactory();
            
            var clientOptions = new MqttClientOptionsBuilder()
                .WithClientId("TestClient")
                .WithCredentials("dotnet-app", "dotnet-app")
                .WithTcpServer("192.168.1.181", 1883)
                .Build();



            var client = factory.CreateMqttClient();


 

            var test1= await client.SubscribeAsync(
                new MqttTopicFilter
                {
                    Topic = "tele/czujnik/SENSOR"
                },
                new MqttTopicFilter
                {
                    Topic = "stat/czujnik/STATUS10"
                }
            );


            client.UseApplicationMessageReceivedHandler(msg =>
            {
                var payloadText = Encoding.UTF8.GetString(
                    msg?.ApplicationMessage?.Payload ?? Array.Empty<byte>());

                Console.WriteLine($"Received msg: {payloadText}");
            });

            var messageA = new MqttApplicationMessageBuilder()
                .WithTopic("cmnd/czujnik/status")
                .WithPayload("10")
                .Build();

            await client.PublishAsync(messageA);


            Console.ReadLine();

         //   await server.StopAsync();
        }

    }
}       


