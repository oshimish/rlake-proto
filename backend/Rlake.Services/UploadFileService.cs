using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;

namespace Rlake.Services
{
    public class UploadFileService
    {
        public UploadFileService(ILogger<UploadFileService> logger, IConfiguration configuration)
        {
            Logger = logger;

            Host = new Uri(configuration["RabbitMq:Connection"] ?? throw new InvalidOperationException());
        }

        public ILogger<UploadFileService> Logger { get; }

        public Uri Host { get; private set; }

        public void SendMessageToRabbitMQ(string tempFilePath)
        {
            // Connection and channel setup
            var factory = new ConnectionFactory() { Uri = Host }; 
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            // Declare the queue
            string queueName = "file_upload_queue";
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            // Send the message
            var body = Encoding.UTF8.GetBytes(tempFilePath);
            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);

            Logger.LogInformation($"Sent to {queueName}");
        }
    }
}