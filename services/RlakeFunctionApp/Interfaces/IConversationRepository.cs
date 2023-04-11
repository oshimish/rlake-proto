using System.Threading;
using System.Threading.Tasks;

namespace RlakeFunctionApp.Interfaces
{
    public interface IConversationRepository
    {
        Task<IList<Conversation>> GetList(CancellationToken ct = default);
        Task<Conversation> CreateAsync(Conversation conversation, CancellationToken ct = default);
    }
}
