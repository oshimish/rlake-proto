using Microsoft.EntityFrameworkCore;

namespace Rlake.Api.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options)
            : base(options)
        {
        }

        public DbSet<Location> Locations { get; set; }
    }
}