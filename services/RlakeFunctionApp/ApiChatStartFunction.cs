using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Rlake.Api.Dto;

namespace RlakeFunctionApp
{
    public class ApiChatStartFunction
    {
        private readonly ILogger<ApiChatStartFunction> _logger;

        public ApiChatStartFunction(ILogger<ApiChatStartFunction> log)
        {
            _logger = log;
        }

        [FunctionName("StartChatFunction")]
        [OpenApiOperation(operationId: "start", tags: new[] { "chat" }, Description = "Creates a new AI conversation.")]
        [OpenApiParameter(name: "searchQuery", In = ParameterLocation.Query, Required = true, Type = typeof(string))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(SearchResultDto), Description = "The OK response")]
        public async Task<ActionResult<SearchResultDto>> StartChat(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "chat/search")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }

        [FunctionName("ListConversationsFunction")]
        [OpenApiOperation(operationId: "listConverstations", tags: new[] { "chat" }, Description = "Get last conversations list.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IList<Conversation>), Description = "The OK response")]
        public async Task<ActionResult<IList<Conversation>>> ListConversations(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "chat")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["searchQuery"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }

        [FunctionName("UploadFileFunction")]
        [OpenApiOperation(operationId: "upload", tags: new[] { "file" }, Description = "Uploads file to the blob. ")]
        [OpenApiRequestBody(contentType: "multipart/form-data", bodyType: typeof(MultiPartFormDataModel), Required = true, Description = "File to upload")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> UploadFile(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "upload")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            // Get the uploaded file
            var formFile = req.Form.Files.FirstOrDefault();

            // Do something with the file

            return new OkObjectResult("File uploaded successfully");
        }
        public class MultiPartFormDataModel
        {
            public byte[] FileUpload { get; set; }
            public string FileName { get; set; }
        }
    }
}

