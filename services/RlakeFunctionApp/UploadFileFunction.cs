using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;

namespace RlakeFunctionApp
{
    public class UploadFileFunction
    {
        public UploadFileFunction(
            ILogger<HttpApiFunctions> logger)
        {
            Logger = logger;
        }
        public ILogger Logger { get; }
            

        [FunctionName("UploadFileFunction")]
        [OpenApiOperation(operationId: "upload", tags: new[] { "file" }, Description = "Uploads file to the blob. ")]
        [OpenApiRequestBody(contentType: "multipart/form-data", bodyType: typeof(MultiPartFormDataModel), Required = true, Description = "File to upload")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> UploadFile(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "upload")] HttpRequest req)
        {
            Logger.LogInformation("C# HTTP trigger function processed a request.");

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

