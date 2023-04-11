
namespace RlakeFunctionApp.Tests.Integration
{
    public class MockNotificationService : INotificationService
    {
        public Task SendConversationStartedEventAsync(CreateConversationOptions createNoteOptions, CancellationToken ct)
        {
            return Task.CompletedTask;
        }
    }
}
