
using System.Threading;
using System.Threading.Tasks;

namespace RlakeFunctionApp.Interfaces
{
    public interface INotificationService
    {
        Task SendConversationStartedEventAsync(CreateConversationOptions createNoteOptions, CancellationToken ct);
    }
}
