namespace RlakeFunctionApp.Interfaces
{
    public interface IChatService
    {
        Task<ChatService.CompletionResult> ProcessCompletion(CreateConversationOptions createConversationOptions, CancellationToken ct);
    }
}