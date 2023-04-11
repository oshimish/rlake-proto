using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using RlakeFunctionApp.Contracts.Requests;
using RlakeFunctionApp.Contracts.Responses;

namespace RlakeFunctionApp.Tests.Integration
{
    [Collection(IntegrationTestsCollection.Name)]
    public class HttpApiFunctionsTests : IClassFixture<TestStartup>, IAsyncLifetime
    {
        private readonly HttpApiFunctions? _sut;
        private readonly TestsInitializer _testsInitializer;
        private readonly CosmosClient _cosmosClient;
        private Container? _container;
        private string? _conversationId;
        private CosmosDbSettings _cosmosDbSettings;

        public HttpApiFunctionsTests(TestsInitializer testsInitializer)
        {
            _testsInitializer = testsInitializer;

            _cosmosDbSettings = testsInitializer.ServiceProvider.GetRequiredService<CosmosDbSettings>();
            _cosmosClient = new CosmosClient(_cosmosDbSettings!.ConnectionString);
            _sut = _testsInitializer.ServiceProvider.GetService<HttpApiFunctions>();
        }

        public async Task InitializeAsync()
        {
            var databaseResponse = await _cosmosClient.CreateDatabaseIfNotExistsAsync(_cosmosDbSettings.DatabaseId);
            var database = databaseResponse.Database;

            var containerResponse = await database.CreateContainerIfNotExistsAsync(_cosmosDbSettings.ContainerId, "/id");
            _container = containerResponse.Container;
        }

        [Fact]
        public async void ListConversations_ShouldReturnOK()
        {
            // Arrange
            var conversation = new Conversation
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Note Title from Integration Test"
            };
            _conversationId = conversation.Id;
            await _container!.CreateItemAsync<Conversation>(conversation, new PartitionKey(_conversationId.ToString()));

            //todo
            //var req = new HttpRequest();

            // Act
            var response = await _sut!.ListConversations(null);
            var listResult = Assert.IsType<OkObjectResult>(response.Result);
            var listResponse = Assert.IsAssignableFrom<IList<Conversation>>(listResult.Value);

            // Assert
            listResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            listResponse.Should().NotBeEmpty();
        }


        [Fact]
        public async void Post_ShouldCreateNote_WhenCalledWithValidNoteDetails()
        {
            // Arrange
            var createValidNoteRequest = new CreateConversationRequest
            {
                SearchText = "Note Title from Integration Test"
            };

            // Act
            var response = await _sut!.StartChat(createValidNoteRequest);
            var createdResult = (OkObjectResult)response.Result;
            var createNoteResponse = createdResult.Value as CreateConversationResponse;
            _conversationId = createNoteResponse!.Conversation.Id;

            // Assert
            createdResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            createNoteResponse.SearchText.Should().Be(createValidNoteRequest.SearchText);
            //createNoteResponse.Body.Should().Be("Note Body from Integration Test");
        }

        [Fact]
        public async void Post_ShouldReturnBadRequest_WhenCalledWithInvalidNoteDetails()
        {
            // Arrange
            var createInvalidNoteRequest = new CreateConversationRequest();

            // Act
            var response = await _sut!.StartChat(createInvalidNoteRequest);
            var badRequestObjectResult = (BadRequestObjectResult)response.Result;

            // Assert
            badRequestObjectResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        public async Task DisposeAsync()
        {
            if (!string.IsNullOrEmpty(_conversationId))
            {
                await _container!.DeleteItemAsync<Conversation>(_conversationId, new PartitionKey(_conversationId.ToString()));
            }
        }
    }
}
