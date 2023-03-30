
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Rlake.Api.Services;

namespace Rlake.Api.Controllers
{
    [ApiController]
    [Route("api/upload")]
    public class UploadController : ControllerBase
    {
        public UploadController(UploadFileService mqClient,
            ApiDbContext dbContext,
            ILogger<UploadController> logger)
        {
            UploadFileService = mqClient;
            DbContext = dbContext;
            Logger = logger;
        }

        public UploadFileService UploadFileService { get; }
        public ApiDbContext DbContext { get; }
        public ILogger Logger { get; }
               
        /// <summary>
        /// Uploads file to the blob. 
        /// </summary>
        [HttpPost()]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            // Check if the file is valid
            if (file == null || file.Length == 0)
            {
                return BadRequest("Invalid file.");
            }

            // Upload the file
            using var stream = file.OpenReadStream();
            var blobPath = await UploadFileService.Upload(stream, file);

            var upload = new Upload()
            {
                BlobPath = blobPath,
                ContentType = file.ContentType,
                FileName = file.FileName,
                Title = file.Name
            };

            DbContext.Add(upload);

            await DbContext.SaveChangesAsync();

            return Ok("File uploaded successfully.");
        }
    }
}