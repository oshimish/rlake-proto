
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
        public IEnumerable<Point> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new Point
            {
                Id = Guid.NewGuid(),
                Title = $"Point {index}"
            })
            .ToArray();
        }
        
        [HttpGet("{id}")]
        public ActionResult<Point> GetById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var loc = new Point()
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
            var blobPath = await UploadFileService.Upload(stream, file);

            return Ok("File uploaded successfully.");
        }
    }
}