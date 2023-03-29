using Microsoft.EntityFrameworkCore;

namespace Rlake.Api.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options)
            : base(options)
        {
        }

        public DbSet<Point> Locations { get; set; }

        public DbSet<Upload> Uploads { get; set; }
    }
}