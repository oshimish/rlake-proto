using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Rlake.Api.Data
{
    /// <summary>
    /// Single AI Conversation
    /// </summary>
    public class Conversation
    {
        [Required]
        [Key]
        public Guid Id { get; set; }
        
        public string Title { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public IList<Post> Posts { get; set; } = new List<Post>();
    }

    /// <summary>
    /// Conversation has many Posts
    /// </summary>
    public class Post
    {
        [Required]
        [Key]
        public Guid Id { get; set; }

        public string Text { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Guid ConversationId { get; set; }

        [JsonIgnore]
        public Conversation Conversation { get; set; } = default!;

        public IList<Point> Points { get; set; } = new List<Point>();
    }

    /// <summary>
    /// Geo point. Each Post can have few points.
    /// </summary>
    public class Point
    {
        [Required]
        [Key]
        public Guid Id { get; set; }

        public string? Title { get; set; } = string.Empty;


        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string? Reason { get; set; }
        public string? Description { get; set; }
        public string? AdditionalInfo { get; set; }

        public int? Order { get; set; }

        public Guid PostId { get; set; }

        [JsonIgnore]
        public Post Post { get; set; } = default!;
    }

    /// <summary>
    /// An file uploaded to the blob.
    /// </summary>

    public class Upload
    {
        [Required]
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;


        public string? FileName { get; set; }
        public string? ContentType { get; set; }

        [Required]
        public string BlobPath { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}