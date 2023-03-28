using Microsoft.Extensions.Options;
using OpenAI.GPT3.Interfaces;
using Rlake.Api.Dto;

namespace Rlake.Api.Services
{
    public class ChatService
    {
        public ChatService(IOptions<AppOptions> options,
            IOpenAIService openAiService,
            AzureServiceBusClient busClient,
            ILogger<ChatService> logger)
        {
            Options = options;
            OpenAiService = openAiService;
            BusClient = busClient;
            Logger = logger;
        }

        public IOptions<AppOptions> Options { get; }
        public IOpenAIService OpenAiService { get; }
        public AzureServiceBusClient BusClient { get; }
        public ILogger<ChatService> Logger { get; }

        public async Task<SearchResult> Post(string searchText)
        {
            Logger.LogDebug($"Post {searchText}");


            // Send request to OpenAI's API
            //var result = await openaiApi.RequestAsync(searchText);

            //// Parse the response to get the related geodata
            //var relatedGeodata = ParseResponse(result);

            //// Use Azure Maps API to get the points to show to the user
            //var points = await azureMapsApi.GetPointsAsync(relatedGeodata);

            List<Location> items = new() {
                new Location(){ Id = Guid.NewGuid(), Title = "Point 1", Latitude = 0.1, Longitude = 0.1 },
                new Location(){ Id = Guid.NewGuid(), Title = "Point 2", Latitude = 0.2, Longitude = 0.1 },
                new Location(){ Id = Guid.NewGuid(), Title = "Point 3", Latitude = 0.2, Longitude = 0.3 },
            };
            return new SearchResult() {
                SearchText = searchText,
                Items = items
            };
        }



        //private List<Geodata> ParseResponse(OpenAIResponse response)
        //{
        //    // Parse the response here and return a list of Geodata objects
        //    // This will depend on the format of the response from OpenAI's API
        //    // For example:
        //    var relatedGeodata = new List<Geodata>();
        //    foreach (var item in response.Data)
        //    {
        //        var geodata = new Geodata
        //        {
        //            Name = item.Name,
        //            Latitude = item.Latitude,
        //            Longitude = item.Longitude
        //        };
        //        relatedGeodata.Add(geodata);
        //    }
        //    return relatedGeodata;
        //}
    }
}