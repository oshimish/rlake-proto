using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RlakeFunctionApp.Entities
{
    /// <summary>
    /// Single AI Conversation
    /// </summary>
    public class Conversation
    {
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        [JsonPropertyName("id")]
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the published date.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the concurrency token.
        /// </summary>
        public string ETag { get; set; }

        /// <summary>
        /// Gets or sets the list of related posts.
        /// </summary>
        public IList<Post> Posts { get; set; } = new List<Post>();

        /// <summary>
        /// Gets the hash code.
        /// </summary>
        /// <returns>The hash code of the unique identifier.</returns>
        public override int GetHashCode() => Id.GetHashCode();

        /// <summary>
        /// Implements equality.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>A value indicating whether the unique identifiers match.</returns>
        public override bool Equals(object obj) =>
            obj is Conversation document && document.Id == Id;

        /// <summary>
        /// Gets the string representation.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString() =>
            $"Conversation {Id}: {Title}.";
    }

    /// <summary>
    /// Conversation has many Posts
    /// </summary>
    public class Post
    {
        public string Text { get; set; } = "";

        /// <summary>
        /// Gets or sets the published date.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the list of related points.
        /// </summary>
        public IList<Point> Points { get; set; } = new List<Point>();
    }

    /// <summary>
    /// Geo point. Each Post can have few points.
    /// </summary>
    public class Point
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; } = string.Empty;


        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string Reason { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        public string AdditionalInfo { get; set; }

        public int? Order { get; set; }
    }
}