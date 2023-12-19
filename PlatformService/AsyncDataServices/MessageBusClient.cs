using System.Text;
using System.Text.Json;
using PlatformService.DTOs;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        private readonly string ExchangeTrigger = "trigger";

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            ConnectionFactory factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port = Int32.Parse(_configuration["RabbitMQPort"] ?? "5672")
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(exchange: ExchangeTrigger, type: ExchangeType.Fanout);
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                Console.WriteLine("--> Connected to Message bus");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not connect to the Message Bus: {ex.Message}");
            }
        }
        public void PublishNewPlatform(PlatformPublishDto publishDto)
        {
            string message = JsonSerializer.Serialize(publishDto);

            if (_connection.IsOpen)
            {
                SendMessage(message);
            }
            else
            {
                Console.WriteLine("--> Messagebus connection not open");
            }
        }

        private void SendMessage(string message)
        {
            Byte[] body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: ExchangeTrigger, routingKey: "", basicProperties: null, body: body);
            Console.WriteLine($"--> Published {message}");
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

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ connection shutdown");
        }
    }
}