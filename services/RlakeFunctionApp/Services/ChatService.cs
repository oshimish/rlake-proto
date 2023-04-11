using Microsoft.Extensions.Options;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels;
using System.Text.RegularExpressions;

namespace RlakeFunctionApp.Services
{
    public class ChatService : IChatService
    {
        public ChatService(IOptions<ChatOptions> options,
            IOpenAIService openAiService,
            ILogger<ChatService> logger)
        {
            Options = options;
            OpenAiService = openAiService;
            Logger = logger;
        }

        public IOptions<ChatOptions> Options { get; }
        public IOpenAIService OpenAiService { get; }
        public ILogger<ChatService> Logger { get; }

        public async Task<CompletionResult> ProcessCompletion(CreateConversationOptions createConversationOptions, CancellationToken ct)
        {
            Logger.LogInformation($"Processing a new completion.");

            var searchText = createConversationOptions.SearchText;
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
                    ChatMessage.FromAssistant(@"find places on the map."),
                    ChatMessage.FromAssistant(@"list any most related 5-13 places."),
                    ChatMessage.FromAssistant(@"finish response, anyway and only single json at the end"),
                    ChatMessage.FromAssistant(@$"required json schema is {jsonExample}"),
                    //ChatMessage.FromUser(@"no need translate"),
                    //ChatMessage.FromAssistant(@"anyway and only one json at the end"),
                    //ChatMessage.FromAssistant(@"answer on last message language"),
                    //ChatMessage.FromAssistant(@"5-20 places with coordinates"),
                    ChatMessage.FromUser($"{searchText}")
                },
                Model = OpenAI.GPT3.ObjectModels.Models.ChatGpt3_5Turbo,
                MaxTokens = 512 // optional
            };
            Logger.LogDebug("Request\n {request}", JsonSerializer.Serialize(request, new JsonSerializerOptions() { WriteIndented = true }));
            var completionResult = await OpenAiService.ChatCompletion.CreateCompletion(request, cancellationToken: ct);
            Logger.LogDebug("Result\n {completionResult}", JsonSerializer.Serialize(completionResult, new JsonSerializerOptions() { WriteIndented = true }));

            if (completionResult.Successful)
            {
                // Extract geo points from the response
                var result = ExtractPointsFromResponse(completionResult.Choices.First().Message.Content);
                return result;
            }

            // Handle cases when the completion is not successful
            Logger.LogError("Failed to get a successful completion from GPT-3: {error}", completionResult.Error?.Message);
            throw new InvalidOperationException(completionResult.Error?.Message);
        }

        private CompletionResult ExtractPointsFromResponse(string response)
        {
            var openToken = response.IndexOf('{');
            // no json
            if (openToken < 0)
            {
                Logger.LogWarning("json not found in result\n {response}", response);
                return new CompletionResult() { ResponseText = response, RawResponse = response };
            }

            var responseText = response.Substring(0, openToken);
            responseText = responseText.Replace("\n\njson:", "");
            responseText = responseText.Replace("```", "").Trim();
            Logger.LogInformation("Response text\n {responseText}", responseText);
            try
            {
                //try to find only json part
                var json = response.Substring(openToken, response.LastIndexOf('}') - openToken + 1);

                try
                {

                    // Implement this method to parse the GPT-3 response and convert it into a list of Point objects
                    // This will depend on the format of the response from GPT-3
                    Logger.LogDebug("Deserializing\n {json}", json);
                    if (!string.IsNullOrEmpty(json))
                    {
                        var result = JsonSerializer.Deserialize<CompletionResult>(json)!;
                        result.RawResponse = response;
                        result.HasData = true;
                        return result;
                    }
                    return new CompletionResult() { ResponseText = responseText, RawResponse = response };
                }
                catch (JsonException e)
                {
                    Logger.LogError(e, $"Error parsing the points: {response}");
                }

                Logger.LogWarning("Try to fix unfinished json", json);

                json = Regex.Replace(json, @",[^,]*$", "}]}");

                // Implement this method to parse the GPT-3 response and convert it into a list of Point objects
                // This will depend on the format of the response from GPT-3
                Logger.LogDebug("Deserializing\n {json}", json);
                if (!string.IsNullOrEmpty(json))
                {
                    var result = JsonSerializer.Deserialize<CompletionResult>(json)!;
                    result.RawResponse = response;
                    result.HasData = true;
                    return result;
                }
                return new CompletionResult() { ResponseText = responseText, RawResponse = response };
            }
            catch (JsonException e)
            {
                Logger.LogError(e, $"Error parsing the points: {response}");
            }
            catch (Exception e)
            {
                Logger.LogError(e, $"Error parsing the response: {responseText}");
                //throw new AiException($"Error parsing the response: {responseText}", e) { RawResponse = response  };
            }
            return new CompletionResult() { ResponseText = responseText, RawResponse = response };
        }

        public class CompletionResult
        {
            [JsonPropertyName("response_text")]
            public string ResponseText { get; set; }

            [JsonPropertyName("locations")]
            public List<Location> Locations { get; set; } = new();

            [JsonIgnore]
            public string RawResponse { get; set; }

            [JsonIgnore]
            public string Error { get; set; }

            [JsonIgnore]
            public bool HasData { get; set; }
        }

        public class Location
        {
            [JsonPropertyName("title")]
            public string Title { get; set; }

            [JsonPropertyName("reason")]
            public string Reason { get; set; }

            [JsonPropertyName("description")]
            public string Description { get; set; }

            [JsonPropertyName("latitude")]
            public double Latitude { get; set; }

            [JsonPropertyName("longitude")]
            public double Longitude { get; set; }

            [JsonPropertyName("additional_info")]
            public string AdditionalInfo { get; set; }
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

        public string RawResponse { get; set; }
    }
}
