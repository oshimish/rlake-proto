﻿using Azure.Storage.Blobs.Models;
using Microsoft.EntityFrameworkCore;

namespace Rlake.Api.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options)
            : base(options)
        {
        }

        public DbSet<Point> Points { get; set; }
        public DbSet<Post> Posts { get; set; }

        public DbSet<Upload> Uploads { get; set; }
        public DbSet<Conversation> Conversations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Conversation>(entity =>
            {
                entity.HasMany(e => e.Posts)
                    .WithOne(e => e.Conversation)
                    .HasForeignKey(e => e.ConversationId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(c => c.CreatedAt)
                    .HasDefaultValueSql("GetDate()");

                entity.Navigation(c => c.Posts)
                    .AutoInclude();
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasMany(e => e.Points)
                    .WithOne(e => e.Post)
                    .HasForeignKey(e => e.PostId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(c => c.CreatedAt)
                    .HasDefaultValueSql("GetDate()");


                entity.Navigation(c => c.Points)
                    .AutoInclude();
            });

            modelBuilder.Entity<Point>(entity =>
            {
                entity.Property(t => t.Latitude).HasPrecision(18, 6);
                entity.Property(t => t.Longitude).HasPrecision(18, 6);
            });

            modelBuilder.Entity<Upload>(entity =>
            {
                entity.Property(c => c.CreatedAt)
                    .HasDefaultValueSql("GetDate()");
            });
        }

        public async Task<Conversation?> LoadConversation(Guid id)
        {
            var conversation = await this.Conversations
                .Include(x => x.Posts)
                .FirstOrDefaultAsync(x => x.Id == id);
            return conversation;
        }

        public async Task<Post?> LoadPost(Guid id)
        {
            var post = await this.Posts
                .Include(x => x.Points)
                .FirstOrDefaultAsync(x => x.Id == id);
            return post;
        }
    }
}