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

        public async Task<SearchResultDto> Post(string searchText)
        {
            Logger.LogDebug($"Post {searchText}");

            //OpenAiService.

            // Send request to OpenAI's API
            //var result = await openaiApi.RequestAsync(searchText);

            //// Parse the response to get the related geodata
            //var relatedGeodata = ParseResponse(result);

            //// Use Azure Maps API to get the points to show to the user
            //var points = await azureMapsApi.GetPointsAsync(relatedGeodata);

            List<Point> items = new()
            {
                new Point() { Id = Guid.NewGuid(), Title = "Golden Gate Bridge", Latitude = 37.8199, Longitude = -122.4783 },
                new Point() { Id = Guid.NewGuid(), Title = "Statue of Liberty", Latitude = 40.6892, Longitude = -74.0445 },
                new Point() { Id = Guid.NewGuid(), Title = "Eiffel Tower", Latitude = 48.8584, Longitude = 2.2945 },
            };
            return new SearchResultDto() {
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