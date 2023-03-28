using Azure.Messaging.ServiceBus;

namespace Rlake.Api.Services
{
    public class AzureServiceBusClient
    {
        private readonly ServiceBusClient _client;

        public AzureServiceBusClient(string connectionString)
        {
            _client = new ServiceBusClient(connectionString);
        }

        public async Task SendMessageAsync(string message, string queueName)
        {
            var sender = _client.CreateSender(queueName);

            var messageBytes = Encoding.UTF8.GetBytes(message);
            var messageData = new ServiceBusMessage(messageBytes);
            await sender.SendMessageAsync(messageData);
        }
    }
}