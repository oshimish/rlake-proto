using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Rlake.Api.Data
{
    public class Conversation
    {
        [Required]
        [Key]
        public Guid Id { get; set; }
        
        public string Title { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public IList<Post> Posts { get; set; } = new List<Post>();
    }

    [Owned]
    public class Post
    {
        [Required]
        [Key]
        public Guid Id { get; set; }

        public string Text { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Guid ConversationId { get; set; }

        [JsonIgnore]
        public Conversation Conversation { get; set; }
        public IList<Location> Locations { get; set; } = new List<Location>();
    }

    public class Location
    {
        [Required]
        [Key]
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Point { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public GeoData GeoPoint { get; set; } = new GeoData();

        public Guid? PostId { get; set; }

        [JsonIgnore]
        public Post? Post { get; set; }
    }


    [Owned]
    public class GeoData
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

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
    }
}