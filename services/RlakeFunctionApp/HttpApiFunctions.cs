using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.ChangeFeedProcessor.Logging;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using RlakeFunctionApp.Contracts.Requests;
using RlakeFunctionApp.Contracts.Responses;
using RlakeFunctionApp.Entities;

namespace RlakeFunctionApp
{
    public class HttpApiFunctions
    {
        private readonly IChatService chatService;
        private readonly IConversationRepository conversationRepository;
        private readonly ILogger<HttpApiFunctions> logger;

        public HttpApiFunctions(IChatService chatService,
            IConversationRepository conversationRepository,
            ILogger<HttpApiFunctions> logger)
        {
            this.chatService = chatService;
            this.conversationRepository = conversationRepository;
            this.logger = logger;
        }

        [FunctionName("StartChatFunction")]
        [OpenApiOperation(operationId: "start", tags: new[] { "chat" }, Description = "Creates a new AI conversation.")]
        [OpenApiParameter(name: "searchQuery", In = ParameterLocation.Query, Required = true, Type = typeof(string))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(CreateConversationResponse), Description = "The OK response")]
        public async Task<ActionResult<CreateConversationResponse>> StartChat(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "chat/search")] CreateConversationRequest createConversationRequest)
        {
            logger.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                string searchText = createConversationRequest.SearchText;

                if (string.IsNullOrWhiteSpace(searchText))
                {
                    return new BadRequestObjectResult("This HTTP triggered functions executed successfully, but you passed in a bad request model for the conversation creation process.");
                }

                var completionrequest = new CreateConversationOptions()
                {
                    SearchText = searchText
                };

                var completion = await chatService.ProcessCompletion(completionrequest, default); //todo: token
                var result = new CreateConversationResponse()
                {
                    SearchText = searchText
                };
                var conversation = new Conversation()
                {
                    Title = result.SearchText,
                };

                var post = new Post()
                {
                    Text = result.SearchText,
                    Points = result.Items
                };
                conversation.Posts.Add(post);

                // hack: store only if any locations
                if (completion.HasData && completion.Locations.Any())
                {
                    await conversationRepository.CreateAsync(conversation);
                }

                result.Conversation = conversation;
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Exception in {nameof(HttpApiFunctions)} -> {nameof(StartChat)} method.");
                return new InternalServerErrorResult();
            }
        }

        [FunctionName("ListConversationsFunction")]
        [OpenApiOperation(operationId: "listConverstations", tags: new[] { "chat" }, Description = "Get last conversations list.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IList<Conversation>), Description = "The OK response")]
        public async Task<ActionResult<IList<Conversation>>> ListConversations(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "chat")] HttpRequest req)
        {
            logger.LogInformation("C# HTTP trigger function processed a request.");
            try
            {
                var data = await conversationRepository.GetList();
                return new OkObjectResult(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Exception in {nameof(HttpApiFunctions)} -> {nameof(ListConversations)} method.");
                return new InternalServerErrorResult();
            }
        }
    }
}

