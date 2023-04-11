using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Rlake.Api.Services
{

    public class UploadFileService
    {
        public UploadFileService(ILogger<UploadFileService> logger,
            IOptions<AzureStorageOptions> storageOptions,
            AzureServiceBusClient busClient)
        {
            Logger = logger;
            StorageOptions = storageOptions;
            BusClient = busClient;
        }

        public ILogger<UploadFileService> Logger { get; }
        public IOptions<AzureStorageOptions> StorageOptions { get; }
        public AzureServiceBusClient BusClient { get; }

        public async Task<string> Upload(Stream stream, IFormFile file)
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

            SendMessageToBus(blobName);
            return blobName;
        }               
      
        public async void SendMessageToBus(string blobName)
        {
            string queueName = "file_upload_queue";
            await BusClient.SendMessageAsync(blobName, queueName);

            Logger.LogInformation($"Sent to {queueName}");
        }
    }
}