using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace RlakeFunctionApp.Repositories
{
    public class ConversationRepository : IConversationRepository
    {
        private readonly CosmosClient _cosmosClient;
        private readonly Container _container;

        public ConversationRepository(CosmosDbSettings cosmosDbSettings)
        {
            _cosmosClient = new CosmosClient(cosmosDbSettings.ConnectionString);
            _container = _cosmosClient.GetContainer(cosmosDbSettings.DatabaseId, cosmosDbSettings.ContainerId);
        }
        public async Task<Conversation> CreateAsync(Conversation conversation, CancellationToken ct)
        {
            conversation.Id = Guid.NewGuid().ToString();
            conversation.Posts.Clear();

            var itemResponse = await _container.CreateItemAsync(conversation, new PartitionKey(conversation.Id), cancellationToken: ct);
            return itemResponse.Resource;
        }

        public async Task<IList<Conversation>> GetList(CancellationToken ct)
        {
            using FeedIterator<Conversation> iterator = _container.GetItemLinqQueryable<Conversation>()
                .OrderByDescending(x => x.CreatedAt)
                .Take(20)
                .ToFeedIterator();

            List<Conversation> results = new();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync(ct);
                results.AddRange(response);
            }

            // hack: Remove no points posts
            //results = results.Where(x => x.Posts.SelectMany(p => p.Points).Any()).ToList();
            return results;
        }
    }
}
