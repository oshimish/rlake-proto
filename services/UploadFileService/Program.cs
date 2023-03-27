using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace UploadFileService;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(" Starting...");

        var uri = new Uri(Environment.GetEnvironmentVariable("RABBITMQ_CONNECTION") ?? throw new InvalidOperationException("RABBITMQ_CONNECTION required"));

        var factory = new ConnectionFactory() { Uri = uri };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "file_upload_queue",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            Console.WriteLine(" [*] Waiting for messages.");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);

                // Simulate processing the message
                System.Threading.Thread.Sleep(1000);

                Console.WriteLine(" [x] Done");

                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            channel.BasicConsume(queue: "file_upload_queue",
                                    autoAck: false,
                                    consumer: consumer);

            Console.WriteLine(" Press Ctrl+C to exit.");

            // Use a ManualResetEvent to block the main thread until Ctrl+C is pressed
            var exitEvent = new ManualResetEvent(false);
            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                eventArgs.Cancel = true;
                exitEvent.Set();
            };

            exitEvent.WaitOne();
        }

    }
}
