
namespace RlakeFunctionApp.Tests.Integration
{
    public class MockChatService : IChatService
    {
        public Task<ChatService.CompletionResult> ProcessCompletion(CreateConversationOptions createConversationOptions, CancellationToken ct)
        {
            return Task.FromResult(new ChatService.CompletionResult()
            {
                ResponseText = createConversationOptions.SearchText,
                HasData = true,
                Locations = new List<ChatService.Location>()
                {
                    new ChatService.Location()
                    {
                        Title = "title 1",
                        Latitude = 1,
                        Longitude = 2
                    }
                }
            });
        }
    }
}
