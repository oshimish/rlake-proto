using Microsoft.Extensions.Options;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels;
using Rlake.Api.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Azure;
using System.Xml;

namespace Rlake.Api.Services
{
    public class ChatService
    {
        public ChatService(IOptions<AppOptions> options,
            IOpenAIService openAiService,
            AzureServiceBusClient busClient,
            IMapper mapper,
            ILogger<ChatService> logger)
        {
            Options = options;
            OpenAiService = openAiService;
            BusClient = busClient;
            Mapper = mapper;
            Logger = logger;
        }

        public IOptions<AppOptions> Options { get; }
        public IOpenAIService OpenAiService { get; }
        public AzureServiceBusClient BusClient { get; }
        public IMapper Mapper { get; }
        public ILogger<ChatService> Logger { get; }

        public async Task<SearchResultDto> Post(string searchText)
        {
            Logger.LogDebug($"Post {searchText}");

            var jsonExample = JsonSerializer.Serialize(new CompletionResult()
            {
                ResponseText = "Friendly helpful geo answer",
                Locations = new List<Location> {
                    new Location()
                    {
                        Title = "What?",
                        Reason = "Why?",
                        Description = "Why it interesting?",
                        Latitude = 12.34,
                        Longitude = 56.78,
                        AdditionalInfo = "More interesting information",
                    } 
                }
            });

            var request = new ChatCompletionCreateRequest
            {
                Messages = new List<ChatMessage>
                {
                    ChatMessage.FromSystem(@"You are a helpful assistant that provides json with places"),
                    //ChatMessage.FromUser(searchText)
                    ChatMessage.FromUser(@"I want to find places on the map."),
                    ChatMessage.FromUser(@"I want list 3-7 places."),
                    ChatMessage.FromUser(@"I want json: " + jsonExample),
                    ChatMessage.FromUser(@"Please finish response"),
                    //ChatMessage.FromAssistant(@"5-20 places with coordinates"),
                    ChatMessage.FromUser($"My query:  {searchText}")
                },
                Model = Models.ChatGpt3_5Turbo,                
                MaxTokens = 1080 // optional
            }; 
            Logger.LogDebug("Request\n {request}", JsonSerializer.Serialize(request, new JsonSerializerOptions() { WriteIndented = true } ));
            var completionResult = await OpenAiService.ChatCompletion.CreateCompletion(request);
            Logger.LogDebug("Result\n {completionResult}", JsonSerializer.Serialize(completionResult, new JsonSerializerOptions() { WriteIndented = true }));

            if (completionResult.Successful)
            {
                // Extract geo points from the response
                var result = ExtractPointsFromResponse(completionResult.Choices.First().Message.Content);

                return Mapper.Map<SearchResultDto>(result);
            }

            // Handle cases when the completion is not successful
            Logger.LogError("Failed to get a successful completion from GPT-3: {error}", completionResult.Error?.Message);
            throw new InvalidOperationException(completionResult.Error?.Message);
        }

        private CompletionResult ExtractPointsFromResponse(string response)
        {
            var parts = response.Split("```"); // code marker
            var responseText = parts[0].Trim();

            // Implement this method to parse the GPT-3 response and convert it into a list of Point objects
            // This will depend on the format of the response from GPT-3
            Logger.LogInformation("Response text\n {responseText}", responseText);
            try
            {                
                var json = parts[1].Trim();
                Logger.LogDebug("Deserializing\n {json}", json);
                if (string.IsNullOrEmpty(json))
                {
                    return new CompletionResult() { ResponseText = responseText, RawResponse = response };
                }

                var result = JsonSerializer.Deserialize<CompletionResult>(json)!;
                result.RawResponse = response;
                return result;
            }
            catch (JsonException e)
            {
                Logger.LogError(e, $"Error parsing the points: {response}");
                return new CompletionResult() { ResponseText = responseText, RawResponse = response };
            }
            catch (Exception e)
            {
                throw new AiException($"Error parsing the response: {responseText}", e) { RawResponse = response  };
            }
        }

        public class CompletionResult
        {
            [JsonPropertyName("response_text")]
            public string? ResponseText { get; set; }

            [JsonPropertyName("locations")]
            public List<Location> Locations { get; set; } = new();

            public string? RawResponse { get; set; }

            public string? Error { get; set; }
        }

        public class Location
        {
            [JsonPropertyName("title")]
            public string? Title { get; set; }

            [JsonPropertyName("reason")]
            public string? Reason { get; set; }

            [JsonPropertyName("description")]
            public string? Description { get; set; }

            [JsonPropertyName("latitude")]
            public double Latitude { get; set; }

            [JsonPropertyName("longitude")]
            public double Longitude { get; set; }

            [JsonPropertyName("additional_info")]
            public string? AdditionalInfo { get; set; }
        }
    }


    [Serializable]
    public class AiException : Exception
    {
        public AiException() { }
        public AiException(string message) : base(message) { }
        public AiException(string message, Exception inner) : base(message, inner) { }
        protected AiException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        public string? RawResponse { get; set; }
    }
}
