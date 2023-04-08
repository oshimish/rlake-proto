
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
            var convs = await DbContext.Conversations.Take(10).ToListAsync();
            var items = convs.SelectMany(x => x.Posts).SelectMany(x => x.Points).ToList();
            items.Add(new Point() { Id = Guid.NewGuid(), Title = "Sydney Opera House", Latitude = -33.8568, Longitude = 151.2153 });
            return items;
        }        
    }
}