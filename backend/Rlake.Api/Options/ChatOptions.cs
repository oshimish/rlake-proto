namespace Rlake.Api.Options
{
    public class ChatOptions
    {
        public OpenAiOptions OpenAi { get; set; } = new();

        public string ServiceBusQueueName { get; set; } = "";
    }
}