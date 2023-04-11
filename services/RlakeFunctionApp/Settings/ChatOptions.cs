using OpenAI.GPT3;

namespace RlakeFunctionApp.Settings
{
    public class ChatOptions
    {
        public OpenAiOptions OpenAi { get; set; } = new();

        public string ServiceBusQueueName { get; set; } = "";
    }
}