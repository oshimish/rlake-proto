using RlakeFunctionApp.Entities;

namespace RlakeFunctionApp.Contracts.Responses
{
    public class CreateConversationResponse
    {
        public string SearchText { get; set; } = "";

        [Required]
        public Conversation Conversation { get; set; } = default!;

        [Required]
        public List<Point> Items { get; set; } = new();
    }
}