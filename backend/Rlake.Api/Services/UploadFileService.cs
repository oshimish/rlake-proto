using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Rlake.Api.Options;
using System.Text;

namespace Rlake.Api.Services
{

    public class UploadFileService
    {
        public UploadFileService(ILogger<UploadFileService> logger,
            IOptions<AzureStorageOptions> storageOptions,
            IOptions<RabbitMqOptions> mqOptions,
            AzureServiceBusClient busClient)
        {
            Logger = logger;
            StorageOptions = storageOptions;
            MqOptions = mqOptions;
            BusClient = busClient;
        }

        public ILogger<UploadFileService> Logger { get; }
        public IOptions<AzureStorageOptions> StorageOptions { get; }
        public IOptions<RabbitMqOptions> MqOptions { get; }
        public AzureServiceBusClient BusClient { get; }

        public async Task<string> StoreToDataLake(Stream stream, IFormFile file)
        {
            // Create a BlobServiceClient
            var blobServiceClient = new BlobServiceClient(StorageOptions.Value.ConnectionString);

            // Replace "your-container-name" with the name of the container you want to upload the file to
            var containerClient = blobServiceClient.GetBlobContainerClient(StorageOptions.Value.ContainerName);

            // Create the container if it doesn't exist
            await containerClient.CreateIfNotExistsAsync();

            string directoryPath = "uploads/user_id";
            string blobName = $"{directoryPath}/{file.FileName}";

            // Use the file name with directory path as the blob name
            var blobClient = containerClient.GetBlobClient(blobName);

            // Upload the file
            await blobClient.UploadAsync(stream, overwrite: true);

            return blobName;
        }

        public void SendMessageToRabbitMQ(string blobName)
        {
            // Connection and channel setup
            var factory = new ConnectionFactory() { Uri = MqOptions.Value.Connection };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            // Declare the queue
            string queueName = "file_upload_queue";
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            // Send the message
            var body = Encoding.UTF8.GetBytes(blobName);
            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);

            Logger.LogInformation($"Sent to {queueName}");
        }

        public async void SendMessageToBus(string blobName)
        {
            string queueName = "file_upload_queue";
            await BusClient.SendMessageAsync(blobName, queueName);

            Logger.LogInformation($"Sent to {queueName}");
        }
    }
}