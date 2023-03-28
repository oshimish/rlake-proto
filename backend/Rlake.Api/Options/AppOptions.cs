using OpenAI.GPT3;
using Rlake.Api.Options;

namespace Rlake.Api.Options
{
    public class AppOptions
    {
        public RabbitMqOptions RabbitMq { get; set; } = new();
        public AzureStorageOptions AzureStorage { get; set; } = new();

        public ChatOptions Chat { get; set; } = new();
    }
}