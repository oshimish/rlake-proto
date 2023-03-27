
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
            // Save the uploaded file to a temporary location
            string tempFilePath = Path.GetTempFileName();
            using (var stream = new FileStream(tempFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Send a message to RabbitMQ with the temp file path
            UploadFileService.SendMessageToRabbitMQ(tempFilePath);

            return Ok();
        }
    }
}