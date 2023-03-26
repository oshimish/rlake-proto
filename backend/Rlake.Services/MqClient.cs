using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;

namespace Rlake.Services
{
    public class MqClient
    {
        public MqClient(ILogger<MqClient> logger)
        {
            Logger = logger;
        }

        public ILogger<MqClient> Logger { get; }

        public void SendMessageToRabbitMQ(string tempFilePath)
        {
            // Connection and channel setup
            var factory = new ConnectionFactory() { HostName = "localhost" }; // Replace "localhost" with your RabbitMQ server address
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            // Declare the queue
            string queueName = "file_upload_queue";
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            // Send the message
            var body = Encoding.UTF8.GetBytes(tempFilePath);
            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
        }
    }
}