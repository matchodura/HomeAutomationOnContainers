using RabbitMQ.Client;
using Network.API.DTOs;
using System.Text;
using System.Text.Json;

namespace Network.API.Services.RabbitMQ
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;


        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;

            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"])
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                Console.WriteLine("--> Connected to Message bus");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not connect to Message bus: {ex.ToString()}");
            }
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");
        }

        public void UpdateAvailableDevice(AvailableDeviceDTO availableDeviceDTO)
        {
            var message = JsonSerializer.Serialize(availableDeviceDTO);

            if (_connection.IsOpen)
            {
                Console.WriteLine("--> RabbitMQ Connection Open, Sending message...");
                SendMessage(message);
            }
            else
            {
                Console.WriteLine("--> RabbitMQ Connection Closed, not sending");
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(
                            exchange: "trigger",
                            routingKey: "",
                            basicProperties: null,
                            body: body
                            );

            Console.WriteLine($"--> We have sent {message}");
        }

        public void Dispose()
        {
            Console.WriteLine("--> MessageBus Disposed");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }
    }
}
