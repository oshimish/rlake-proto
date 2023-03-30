
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Rlake.Api.Services;

namespace Rlake.Api.Controllers
{
    [ApiController]
    [Route("api/locations")]
    public class LocationsController : ControllerBase
    {
        public LocationsController(
            ApiDbContext dbContext,
            ILogger<LocationsController> logger)
        {
            DbContext = dbContext;
            Logger = logger;
        }

        public ApiDbContext DbContext { get; }
        public ILogger<LocationsController> Logger { get; }

        /// <summary>
        /// Get some points.
        /// </summary>
        [HttpGet()]
        public async Task<IList<Point>> Get()
        {
            var items = await DbContext.Points.Take(10).ToListAsync();
            items.Add(new Point() { Id = Guid.NewGuid(), Title = "Sydney Opera House", Latitude = -33.8568, Longitude = 151.2153 });
            return items;
        }
        
        /// <summary>
        /// Get locations details by id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Point>> GetById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var point = await DbContext.Points
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (point == null)
            {
                return NotFound();
            }

            return Ok(point);
        }
    }
}