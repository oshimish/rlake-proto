using Newtonsoft.Json;

namespace RlakeFunctionApp.Models
{
    public class ConversationCreatedRequest
    {
        [JsonProperty("title")]
        public string Title { get; set; } = default!;

        [JsonProperty("description")]
        public string Description { get; set; } = default!;
    }
}
