

namespace Rlake.Api.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options)
            : base(options)
        {
        }

        public DbSet<Upload> Uploads { get; set; }
        public DbSet<Conversation> Conversations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultContainer("Conversations");

            modelBuilder.Entity<Conversation>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(c => c.CreatedAt);
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(c => c.CreatedAt);
            });

            modelBuilder.Entity<Point>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(t => t.Latitude).HasPrecision(18, 6);
                entity.Property(t => t.Longitude).HasPrecision(18, 6);
            });

            modelBuilder.Entity<Upload>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(c => c.CreatedAt);
            });
        }

        public async Task<Conversation?> LoadConversation(Guid id)
        {
            var conversation = await this.Conversations
                .Include(x => x.Posts)
                .FirstOrDefaultAsync(x => x.Id == id);
            return conversation;
        }
    }
}