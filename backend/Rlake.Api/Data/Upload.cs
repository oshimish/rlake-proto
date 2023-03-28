using System.ComponentModel.DataAnnotations;

namespace Rlake.Api.Data
{
    public class Upload
    {
        [Required]
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;


        //[Required]
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
        public string? BlobPath { get; set; }
    }
}