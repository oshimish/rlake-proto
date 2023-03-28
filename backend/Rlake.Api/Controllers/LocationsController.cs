
using Azure.Storage.Blobs;
using Rlake.Api.Services;

namespace Rlake.Api.Controllers
{
    [ApiController]
    [Route("api/locations")]
    public class LocationsController : ControllerBase
    {
        public LocationsController(UploadFileService mqClient,
            ILogger<LocationsController> logger)
        {
            UploadFileService = mqClient;
            Logger = logger;
        }

        public UploadFileService UploadFileService { get; }
        public ILogger<LocationsController> Logger { get; }


        [HttpGet()]
        public IEnumerable<Location> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new Location
            {
                Id = Guid.NewGuid(),
                Title = $"Point {index}"
            })
            .ToArray();
        }
        
        [HttpGet("{id}")]
        public ActionResult<Location> GetById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var loc = new Location()
            {
                Id = id,
                Title = $"Item {id}",
            };
            return Ok(loc);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            // Check if the file is valid
            if (file == null || file.Length == 0)
            {
                return BadRequest("Invalid file.");
            }


            // Upload the file
            using var stream = file.OpenReadStream();
            var blobPath = await UploadFileService.StoreToDataLake(stream, file);

            UploadFileService.SendMessageToBus(blobPath);
            UploadFileService.SendMessageToRabbitMQ(blobPath);

            return Ok("File uploaded successfully.");
        }
    }
}