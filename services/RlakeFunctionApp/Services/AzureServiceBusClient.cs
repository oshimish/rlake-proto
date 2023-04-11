
namespace RlakeFunctionApp.Services
{
    public class AzureServiceBusClient
    {
        //private readonly ServiceBusClient _client;

        public AzureServiceBusClient(string connectionString)
        {
            //_client = new ServiceBusClient(connectionString);
        }

        public async Task SendMessageAsync(string message, string queueName)
        {
            // TODO: functions binding
            //var sender = _client.CreateSender(queueName);

            //var messageBytes = Encoding.UTF8.GetBytes(message);
            //var messageData = new ServiceBusMessage(messageBytes);
            //await sender.SendMessageAsync(messageData);
        }
    }
}